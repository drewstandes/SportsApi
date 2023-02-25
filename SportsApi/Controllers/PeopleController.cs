using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApi.Data;
using SportsApi.Models;
using SportsApi.Queries.People;
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
            List<string> noSportsList = new List<string> { "None" };
            List<PersonFavouritesQuery> personFavourites = await (from person in _context.People
                                                                  select new PersonFavouritesQuery
                                                                  {
                                                                      FirstName = person.FirstName,
                                                                      LastName = person.LastName,
                                                                      IsEnabled = person.IsEnabled,
                                                                      IsValid = person.IsValid,
                                                                      IsAuthorised = person.IsAuthorised,
                                                                      FavouriteSports = person.Sports != null ? person.Sports.Select(s => s.Name).ToList() : noSportsList
                                                                  }).ToListAsync();

            return Ok(personFavourites);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonSportQuery>> GetPerson(long id)
        {
            var tempPersonSport = (from person in _context.People
                               where person.Id == id
                               select new PersonSportQuery
                               {
                                   FirstName = person.FirstName,
                                   LastName = person.LastName,
                                   FavouriteSport = person.Sports.First() == null ? "No favourite sport" : person.Sports.First().Name
                               });

            if (tempPersonSport.Count() == 0)
            {
                return BadRequest("There is no person with that Id.");
            }

            PersonSportQuery personSport = (PersonSportQuery)tempPersonSport.Single();
            return Ok(personSport);
        }

    }
}
