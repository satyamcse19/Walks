using WebApiVersioning.Interface;
using WebApiVersioning.Models.Domain;

namespace WebApiVersioning.Repositories
{
    public class CountryRepository : Icountry
    {
        public async Task<List<Country>> GetCountriesAsync()
        {
             var countryList = new List<Country>
        {
            new Country { Id = 1, Name = "United States" },
            new Country { Id = 2, Name = "Canada" },
            new Country { Id = 3, Name = "Mexico" },
            new Country { Id = 4, Name = "Brazil" },
            new Country { Id = 5, Name = "United Kingdom" },
            new Country { Id = 6, Name = "Germany" },
            new Country { Id = 7, Name = "France" },
            new Country { Id = 8, Name = "Italy" },
            new Country { Id = 9, Name = "Spain" },
            new Country { Id = 10, Name = "Australia" },
            new Country { Id = 11, Name = "India" },
            new Country { Id = 12, Name = "China" },
            new Country { Id = 13, Name = "Japan" },
            new Country { Id = 14, Name = "South Korea" },
            new Country { Id = 15, Name = "Russia" },
            new Country { Id = 16, Name = "South Africa" },
            new Country { Id = 17, Name = "Argentina" },
            new Country { Id = 18, Name = "Egypt" },
            new Country { Id = 19, Name = "Saudi Arabia" },
            new Country { Id = 20, Name = "Turkey" }
        };

            // Simulate asynchronous operation
             await Task.Delay(100); // Simulating async work
             return countryList;
        }
    }
}
