using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsApi.Data;
using SportsApi.Queries.People;

namespace SportsApi.Handlers
{
    public class GetPersonSportHandler : IRequestHandler<GetPersonSportQuery, IQueryable<PersonSportQuery>>
    {
        private readonly DataContext _context;

        public GetPersonSportHandler(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<IQueryable<PersonSportQuery>> Handle(GetPersonSportQuery request, CancellationToken cancellationToken)
        {
            return (from person in _context.People
                    where person.Id == request.id
                    select new PersonSportQuery
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        FavouriteSport = person.Sports.First() == null ? "No favourite sport" : person.Sports.First().Name
                    });
        }
    }
}
