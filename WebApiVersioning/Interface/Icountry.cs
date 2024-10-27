using WebApiVersioning.Models.Domain;

namespace WebApiVersioning.Interface
{
    public interface Icountry
    {
         Task<List<Country>> GetCountriesAsync();
    }
}
