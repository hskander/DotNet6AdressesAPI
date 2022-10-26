using DotNet6AdressesAPI.Data;
using DotNet6AdressesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoogleMaps.LocationServices;
using System.Net;
using Geolocation;
using System.Reflection.Metadata.Ecma335;

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


        [HttpGet("/Search/{value}")]
        public async Task<ActionResult<List<Address>>> Search(string value)
        {
           
                 return Ok(await this._appDbContext.Addresses.Where(a => a.HouseNumber.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.Id.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.ZipCode.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.Country.ToLower().Contains(value.ToLower())
                                                                  ||a.City.ToLower().Contains(value.ToLower())
                                                                  ||a.Street.ToLower().Contains(value.ToLower())).ToListAsync());
        }

        [HttpGet("/Search/{address1Id}/{address2Id}")]
        public async Task<ActionResult<float>> DistanceBetweenAddresses(int address1Id, int address2Id)
        {
            var gls = new GoogleLocationService();
           // var address1 = await this._appDbContext.Addresses.FindAsync(address1Id);
            //var address2 = await this._appDbContext.Addresses.FindAsync(address2Id);
            try
            {
                // var latlong = gls.GetLatLongFromAddress(address1.City);
                //var Latitude = latlong.Latitude;
                //var Longitude = latlong.Longitude;
                // System.Console.WriteLine("Address ({0}) is at {1},{2}", address1, Latitude, Longitude);
                //var latlong2 = gls.GetLatLongFromAddress(address2.City);
                //var Latitude2 = latlong.Latitude;
                //var Longitude2 = latlong.Longitude;
                //System.Console.WriteLine("Address ({0}) is at {1},{2}", address1, Latitude, Longitude);
                double distance = GeoCalculator.GetDistance(48.8566, -2.3522, 45.7640, -4.8357, 1);
                return Ok(distance* 1.60934);
            }
            catch (Exception ex)
            {
                return BadRequest("Bad Request");
            }

        }
        
    }
}
