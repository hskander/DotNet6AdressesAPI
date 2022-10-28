using DotNet6AdressesAPI.Data;
using DotNet6AdressesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Address = DotNet6AdressesAPI.Models.Address;

namespace DotNet6AdressesAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _appDbContext;
        public AddressService(AppDbContext context)
        {
            this._appDbContext = context;
        }

        public async Task<ActionResult<List<Address>>> AddAddress(Address address)
        {
           await this._appDbContext.AddAsync(address);
            await _appDbContext.SaveChangesAsync();
            return await this._appDbContext.Addresses.ToListAsync();
        }

        public async Task<ActionResult<List<Address>>> DeleteAddressById(int id)
        {
            var address = _appDbContext.Addresses.Find(id);
            if (address != null)
            {
                this._appDbContext.Addresses.Remove(address);
                await this._appDbContext.SaveChangesAsync();
            }
            return await this._appDbContext.Addresses.ToListAsync();
        }

        public async Task<ActionResult<Address>> EditAddress(Address newAddress)
        {
            var address = await this._appDbContext.Addresses.FindAsync(newAddress.Id);
                if(address != null)
            {
                address.Street = newAddress.Street;
                address.City = newAddress.City;
                address.HouseNumber = newAddress.HouseNumber;
                address.ZipCode = newAddress.ZipCode;
                address.Country = newAddress.Country;
                this._appDbContext.Addresses.Update(address);
                await this._appDbContext.SaveChangesAsync();
                return address;
            }
               
            return new Address();
        }

        public async Task<ActionResult<Address>> GetAddressById(int id)
        {
            var address = await _appDbContext.Addresses.FindAsync(id);
            return address;
        }

        public async Task<ActionResult<List<Address>>> GetAllAddresses()
        {
           return  await this._appDbContext.Addresses.ToListAsync();
        }

        public async Task<ActionResult<List<Address>>> GetAllSorted(string AscOrDesc)
        {
            if (AscOrDesc == "Desc")
            {
                    return await this._appDbContext.Addresses.OrderByDescending(a => a.HouseNumber).ToListAsync();
            }
          
                return await this._appDbContext.Addresses.OrderBy(a => a.HouseNumber).ToListAsync();
        }

        public async Task<ActionResult<List<Address>>> Search(string Value)
        {
            return await this._appDbContext.Addresses.Where(a => a.HouseNumber.ToString().ToLower().Contains(Value.ToLower())
                                                                    || a.Id.ToString().ToLower().Contains(Value.ToLower())
                                                                    || a.ZipCode.ToString().ToLower().Contains(Value.ToLower())
                                                                    || a.Country.ToLower().Contains(Value.ToLower())
                                                                    || a.City.ToLower().Contains(Value.ToLower())
                                                                    || a.Street.ToLower().Contains(Value.ToLower())).ToListAsync();
        }
    }
}
