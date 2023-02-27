using MediatR;
using SportsApi.Queries.People;

namespace SportsApi.Queries.Sports
{
    public class GetListFavouriteSportQuery : IRequest<List<FavouriteSportQuery>>
    {
    }
}
