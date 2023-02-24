using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApi.Data;
using SportsApi.Models;
using SportsApi.Queries;
using System;

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
        public async Task<ActionResult<PersonSportQuery>> GetPerson(long id)
        {
            var personSport = (from person in _context.People
                               where person.Id == id
                               select new PersonSportQuery
                               {
                                   FirstName = person.FirstName,
                                   LastName = person.LastName,
                                   FavouriteSport = person.Sports.First() == null ? "No favourite sport" : person.Sports.First().Name
                               });

            return Ok(personSport);
        }

    }
}
