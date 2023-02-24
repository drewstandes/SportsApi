using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApi.Data;
using SportsApi.Models;
using SportsApi.Queries;

namespace SportsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly DataContext _context;

        public PeopleController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<Person>>> AddPerson(Person person)
        {
            if (person == null) { return  BadRequest(); }

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return Ok(await _context.People.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonFavouritesQuery>>> GetPeople()
        {
            List<PersonFavouritesQuery> personFavourites = await (from person in _context.People
                                                 select new PersonFavouritesQuery
                                                 {
                                                     FirstName = person.FirstName,
                                                     LastName = person.LastName,
                                                     IsEnabled = person.IsEnabled,
                                                     IsValid = person.IsValid,
                                                     IsAuthorised = person.IsAuthorised,
                                                     FavouriteSports = person.Sports.Select(s => s.Name).ToList()
                                                 }).ToListAsync();

            return Ok(personFavourites);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return BadRequest("Person not found.");
            }

            return Ok(person);
        }

    }
}
