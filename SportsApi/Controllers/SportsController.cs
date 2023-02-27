using MediatR;
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
        private readonly IMediator _mediator;

        public SportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<FavouriteSportQuery>>> GetSports()
        {
            List<FavouriteSportQuery> favouriteSportQueries = await _mediator.Send(new GetListFavouriteSportQuery());

            if (favouriteSportQueries.Count == 0)
            {
                return BadRequest("There are no sports to return.");
            }

            return Ok(favouriteSportQueries);
        }

    }
}
