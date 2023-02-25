using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApi.Data;
using SportsApi.Models;
using SportsApi.Queries.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly DataContext _context;

        public SportsController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<FavouriteSportQuery>>> GetSports()
        {
            List<FavouriteSportQuery> favouriteSports = await (from sport in _context.Sports
                                                               where sport.IsEnabled == true
                                                               select new FavouriteSportQuery
                                                               {
                                                                   Name = sport.Name,
                                                                   Favourites = sport.People.Count
                                                               }).ToListAsync();

            if (favouriteSports.Count == 0)
            {
                return BadRequest("There are no sports to return.");
            }

            return Ok(favouriteSports);
        }

    }
}
