using DotNet6AdressesAPI.Data;
using DotNet6AdressesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoogleMaps.LocationServices;
using System.Net;
using Geolocation;
using System.Reflection.Metadata.Ecma335;
using Geocoding.Google;
using Geocoding;
using Address = DotNet6AdressesAPI.Models.Address;
using Velyo.Google.Services.Models;
using Velyo.Google.Services;

namespace DotNet6AdressesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public AddressesController(AppDbContext context)
        {
            this._appDbContext = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<Address>>> AddAddress(Address address)
        {
            this._appDbContext.Add(address);
            await this._appDbContext.SaveChangesAsync();

            return Ok(await this._appDbContext.Addresses.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<Address>>> GetAllAddresses()
        {
            return Ok(await this._appDbContext.Addresses.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddressById(int id)
        {
            var address = await this._appDbContext.Addresses.FindAsync(id);
            if (address == null)
            {
                return BadRequest("Address Not Found.");
            }
            return Ok(address);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Address>>> DeleteAddressById(int id)
        {
            var address = await this._appDbContext.Addresses.FindAsync(id);
            if (address == null)
            {
                return BadRequest("Address Not Found.");
            }
            else
            {
                 this._appDbContext.Addresses.Remove(address);
                await this._appDbContext.SaveChangesAsync();

            }
            return Ok(await this._appDbContext.Addresses.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<Address>> EditAddress(Address newAddress)
        {
            var address = await this._appDbContext.Addresses.FindAsync(newAddress.Id);
            if (address == null)
            {
                return BadRequest("Address with Id= "+ newAddress.Id +" Not Found.");
            }
            else
            {
                address.Street= newAddress.Street;
                address.City= newAddress.City;
                address.HouseNumber= newAddress.HouseNumber;
                address.ZipCode= newAddress.ZipCode;
                address.Country= newAddress.Country;
                this._appDbContext.Addresses.Update(address);
                await this._appDbContext.SaveChangesAsync();

            }
            return Ok(newAddress);
        }

        [HttpGet("/Sort/{AscOrDesc}")]
        public async Task<ActionResult<List<Address>>> GetAllAscending(string AscOrDesc)
        {
            if(AscOrDesc == "Desc")
            {
                try
                {
                    return Ok(await this._appDbContext.Addresses.OrderByDescending(a => a.HouseNumber).ToListAsync());
                }
                catch (Exception ex)
                {
                    return BadRequest("Bad Request");
                }
            }
            try
            {
                return Ok(await this._appDbContext.Addresses.OrderBy(a => a.HouseNumber).ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Bad Request");
            }

        }


        [HttpGet("/Search")]
        public async Task<ActionResult<List<Address>>> Search(string value)
        {
           
                 return Ok(await this._appDbContext.Addresses.Where(a => a.HouseNumber.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.Id.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.ZipCode.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.Country.ToLower().Contains(value.ToLower())
                                                                  ||a.City.ToLower().Contains(value.ToLower())
                                                                  ||a.Street.ToLower().Contains(value.ToLower())).ToListAsync());
        }
        //in this method i tried to pass 2 parameters in the request for the first and the second address that i want to
        //know the distance between them if the 2 addresses are not null, then i used the package
        //GoogleMaps.LocationServices (https://www.nuget.org/packages/GoogleMaps.LocationServices)
        // that suposed to give me the coordinates of each address, then when i have the coordinates 
        // the package GeoLocation (https://www.nuget.org/packages/Geolocation) calculate the distance between them 
        // issue: the package Geolocation work successfuly, but GoogleMaps.LocationServices doesen't work 
        /*
        [HttpGet("/distance")]
        public async Task<ActionResult<float>> DistanceBetweenAddresses(int address1Id, int address2Id)
        {
           // var gls = new GoogleLocationService(apikey: "AIzaSyANT5_7rXyIM3aY6rT69sJ73M0HAwtXC3E");
            var gls = new GoogleLocationService();
             var address1 = await this._appDbContext.Addresses.FindAsync(address1Id);
            var address2 = await this._appDbContext.Addresses.FindAsync(address2Id);
            if (address1 == null || address2 == null)
            {
                return BadRequest("Address with Id= " + address1Id + " or Id="+ address2Id + " Not Found Check your Data.");
            }
            else
            {
                var latlong = gls.GetLatLongFromAddress(address1.HouseNumber.ToString() + " " + address1.Street + " " +address1.ZipCode + " " +address1.City + ", " + address1.Country);
                var Latitude = latlong.Latitude;
                var Longitude = latlong.Longitude;
                var latlong2 = gls.GetLatLongFromAddress(address2.HouseNumber.ToString() + " " + address2.Street + " " +address1.ZipCode + " " + address2.City + ", " + address2.Country);
                var Latitude2 = latlong2.Latitude;
                var Longitude2 = latlong2.Longitude;
                double distance = GeoCalculator.GetDistance(Latitude, -Longitude, Latitude2, -Longitude2, 1);
                return Ok(distance * 1.60934);
            }
        }
        */

        //this method return the same output as the previous method, i just tried to change the package GoogleMaps.LocationServices
        //by Geocording.core(https://www.nuget.org/packages/Geocoding.Core) but it also doesen't work 
        /*
        [HttpGet("/distance2")]
        public async Task<ActionResult<float>> DistanceBetweenAddresses2()
        {
         
            GeocodingRequest request = new GeocodingRequest("Europalaan 100, 3526 KS Utrecht, Netherlands");
            GeocodingResponse response = request.GetResponse();
            // GeocodingResponse response = request.GetResponseAsync();
            LatLng location = response.Results[0].Geometry.Location;
            double latitude = location.Latitude;
            double longitude = location.Longitude;

            GeocodingRequest request1 = new GeocodingRequest("Verlengde Hoogravenseweg 63, 3525 BB Utrecht, Netherlands");
            GeocodingResponse response1 = request1.GetResponse();
            // GeocodingResponse response = request.GetResponseAsync();
            LatLng location1 = response1.Results[0].Geometry.Location;
            double latitude1 = location1.Latitude;
            double longitude1 = location1.Longitude;

            
            double distance = GeoCalculator.GetDistance(latitude, -longitude, latitude1, -longitude1 ,1);
            return Ok(distance * 1.60934);
        }
        */
        // a method that calculate the distance between Paris and Utrecht
        [HttpGet("/distanceTest")]
        public async Task<ActionResult<float>> distanceBetweenTostaticCoordinates()
        {
            double distance = GeoCalculator.GetDistance(52.0907, -5.1214, 48.8566, -2.3522, 1);
            return Ok(distance * 1.60934 +"KM");
        }

    }
}
