using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsApi.Data;
using SportsApi.Queries.Sports;

namespace SportsApi.Handlers
{
    public class GetListFavouriteSportHandler : IRequestHandler<GetListFavouriteSportQuery, List<FavouriteSportQuery>>
    {
        private readonly DataContext _context;

        public GetListFavouriteSportHandler(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<List<FavouriteSportQuery>> Handle(GetListFavouriteSportQuery request, CancellationToken cancellationToken)
        {
            return await (from sport in _context.Sports
                          where sport.IsEnabled == true
                          select new FavouriteSportQuery
                          {
                              Name = sport.Name,
                              Favourites = sport.People.Count
                          }).ToListAsync();
        }
    }
}
