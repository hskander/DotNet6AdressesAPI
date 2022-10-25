using DotNet6AdressesAPI.Data;
using DotNet6AdressesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Reflection;

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

        /*[HttpGet("test")]
        public async Task<ActionResult<List<Address>>> getTest()
        {
            Address a=new Address();
            return Ok( a.GetType().GetProperties().ToList() ) ;
        }*/

        [HttpGet("/Search/{value}")]
        public async Task<ActionResult<List<Address>>> Search(string value)
        {
           
                 return Ok(await this._appDbContext.Addresses.Where(a => a.HouseNumber.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.Id.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.ZipCode.ToString().ToLower().Contains(value.ToLower())
                                                                  ||a.Country.ToLower().Contains(value.ToLower())
                                                                  ||a.City.ToLower().Contains(value.ToLower())
                                                                  ||a.Street.ToLower().Contains(value.ToLower())).ToListAsync());

               /* return Ok(await this._appDbContext.Addresses
                    . Where(a => Array.IndexOf(a.GetType().GetProperties(),value)>-1)
                    .ToListAsync()); */

           
        }

    }
}
