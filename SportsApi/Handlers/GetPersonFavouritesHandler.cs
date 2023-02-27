using MediatR;
using SportsApi.Data;
using SportsApi.Queries.People;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsApi.Models;
using System;

namespace SportsApi.Handlers
{
    public class GetPersonFavouritesHandler : IRequestHandler<GetListPersonFavouritesQuery, List<PersonFavouritesQuery>>
    {
        private readonly DataContext _context;

        public GetPersonFavouritesHandler(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<List<PersonFavouritesQuery>> Handle(GetListPersonFavouritesQuery request, CancellationToken cancellationToken)
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

            return personFavourites;
        }
    }
}
