using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SportsApi.Queries.People
{
    public record GetPersonSportQuery(long id) : IRequest<IQueryable<PersonSportQuery>>;
}
