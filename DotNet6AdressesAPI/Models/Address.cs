using Microsoft.OpenApi.Any;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace DotNet6AdressesAPI.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Street is required")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "HouseNumber is required")]
        public int HouseNumber { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        public int ZipCode { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string? Country { get; set; }

    }
}
