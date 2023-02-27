using MediatR;
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
        private readonly IMediator _mediator;
        private readonly DataContext _context;

        public PeopleController(IMediator mediator, DataContext dataContext) {
            _mediator = mediator;
            _context = dataContext;
        }


        [HttpGet]
        public async Task<ActionResult<List<PersonFavouritesQuery>>> GetPeople()
        {

            List<PersonFavouritesQuery> personFavouritesQueries = await _mediator.Send(new GetListPersonFavouritesQuery());

            if (personFavouritesQueries.Count == 0)
            {
                return BadRequest("There are no people to return.");
            }

            return Ok(personFavouritesQueries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonSportQuery>> GetPerson(long id)
        {
            var tempPersonSport = await _mediator.Send(new GetPersonSportQuery(id));

            if (tempPersonSport.Count() == 0)
            {
                return BadRequest("There is no person with that Id.");
            }

            PersonSportQuery personSport = tempPersonSport.Single();
            return Ok(personSport);
        }

    }
}
