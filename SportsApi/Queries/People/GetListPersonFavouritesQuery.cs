using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SportsApi.Queries.People
{
    public class GetListPersonFavouritesQuery : IRequest<List<PersonFavouritesQuery>>
    {
    }
}
