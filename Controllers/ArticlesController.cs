using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private ApplicationContext db;
        public ArticlesController(ApplicationContext db) 
        {
            this.db = db;
        } 

    }
}
