using DotNet6AdressesAPI.Data;
using DotNet6AdressesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<List<Address>>> GetAddressById(int id)
        {
            var address = await this._appDbContext.Addresses.FindAsync(id);
            if(address == null)
            {
                return BadRequest("Address Not Found.");
            }
            return Ok(address);
        }


    }
}
