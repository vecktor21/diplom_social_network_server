using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.ViewModels;

namespace server.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class CountryController : Controller
    {
        private ApplicationContext db;
        public CountryController(ApplicationContext ct)
        {
            this.db = ct;
        }
        [HttpGet]
        public IEnumerable<Country> GetCountries()
        {
            return db.Countries.ToList();
        }
        [HttpPut]
        public async Task Put ([FromBody]CountryViewModel country)
        {
            db.Countries.Add(new Country { CountryNameEn= country.CountryNameEn, CountryNameRu = country.CountryNameRu});
            await db.SaveChangesAsync();
        }
    }
}
