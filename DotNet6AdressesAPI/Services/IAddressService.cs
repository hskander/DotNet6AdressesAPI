using DotNet6AdressesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNet6AdressesAPI.Services
{
    public interface IAddressService
    {
        Task<ActionResult<List<Address>>> AddAddress(Address address);
        Task<ActionResult<List<Address>>> GetAllAddresses();
        Task<ActionResult<Address>> GetAddressById(int id);
        Task<ActionResult<List<Address>>> DeleteAddressById(int id);
        Task<ActionResult<Address>> EditAddress(Address newAddress);
        Task<ActionResult<List<Address>>> GetAllSorted(string AscOrDesc);
        Task<ActionResult<List<Address>>> Search(String Value);
    }
}
