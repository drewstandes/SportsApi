using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SportsApi.Controllers;
using SportsApi.Data;
using SportsApi.Models;
using SportsApi.Queries;

namespace SportsApiTest
{
    public class UnitTest
    {
        private async Task<DataContext> getDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dataContext = new DataContext(options);
            dataContext.Database.EnsureCreated();

            var sport1 = new Sport { Name = "American Football", IsEnabled = true };
            var sport2 = new Sport { Name = "Basketball", IsEnabled = true };
            var sport3 = new Sport { Name = "Baseball", IsEnabled = true };

            if (await dataContext.People.CountAsync() <= 0)
            {
                dataContext.People.AddRange(
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Snow",
                        IsAuthorised = true,
                        IsEnabled = true,
                        IsValid = true,
                        Sports = new List<Sport> { sport1, sport2, sport3 }
                    },
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Wick",
                        IsAuthorised = true,
                        IsEnabled = true,
                        IsValid = true,
                        Sports = new List<Sport> { sport2, sport3 }
                    },
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Cena",
                        IsAuthorised = true,
                        IsEnabled = false,
                        IsValid = true,
                        Sports = new List<Sport> { sport1 }
                    }
                    );
                await dataContext.SaveChangesAsync();
            }
            return dataContext;

        }


        [Fact]
        public async void GetPerson_Returns_PersonSportQuery()
        {
            // Arrange 
            var dataContext = await getDbContext();
            PeopleController peopleController = new PeopleController(dataContext);
            PersonSportQuery expectedPersonSportQuery = new PersonSportQuery
            {
                FirstName = "John",
                LastName = "Snow",
                FavouriteSport = "American Football"
            };

            // Act 
            ActionResult<PersonSportQuery> actionResult = await peopleController.GetPerson(1L);
            var result = actionResult.Result as OkObjectResult;
            var actualPersonSportQuery = result.Value as PersonSportQuery;

            // Assert
            actualPersonSportQuery.Should().BeEquivalentTo(expectedPersonSportQuery);

        }


        [Fact]
        public async void GetPersonInvalidId_Returns_BadRequest()
        {
            // TODO
            var dataContext = await getDbContext();
            PeopleController peopleController = new PeopleController(dataContext);
            PersonSportQuery expectedPersonSportQuery = new PersonSportQuery
            {
                FirstName = "John",
                LastName = "Snow",
                FavouriteSport = "American Football"
            };

            // Act 
            ActionResult<PersonSportQuery> actionResult = await peopleController.GetPerson(4L);
            var result = actionResult.Result as BadRequestObjectResult;
            var query = result.Value as EntityQueryable<PersonSportQuery>;

            var actualPersonSportQuery = query.First();

            // Assert
            actualPersonSportQuery.Should().BeEquivalentTo(expectedPersonSportQuery);

        }

        [Fact]
        public async void GetPeople_Returns_ListOf_PersonFavouritesQuery()
        {
            // Arrange 
            var dataContext = await getDbContext();
            PeopleController peopleController = new PeopleController(dataContext);
            List<PersonFavouritesQuery> expectedListPersonFavouritesQuery = new List<PersonFavouritesQuery>
            { new PersonFavouritesQuery
            {
                        FirstName = "John",
                        LastName = "Snow",
                        IsAuthorised = true,
                        IsEnabled = true,
                        IsValid = true,
                        FavouriteSports = new List<string> {"American Football", "Basketball", "Baseball"}
            },
            new PersonFavouritesQuery
            {
                        FirstName = "John",
                        LastName = "Wick",
                        IsAuthorised = true,
                        IsEnabled = true,
                        IsValid = true,
                        FavouriteSports = new List<string> {"Basketball", "Baseball"}
            },
            new PersonFavouritesQuery
            {
                        FirstName = "John",
                        LastName = "Cena",
                        IsAuthorised = true,
                        IsEnabled = false,
                        IsValid = true,
                        FavouriteSports = new List<string> {"American Football"}
            },
            };


            // Act 
            ActionResult<List<PersonFavouritesQuery>> actionResult = await peopleController.GetPeople();
            var result = actionResult.Result as OkObjectResult;
            var actualListPersonFavouritesQuery = result.Value as List<PersonFavouritesQuery>;

            // Assert
            actualListPersonFavouritesQuery.Should().BeEquivalentTo(expectedListPersonFavouritesQuery);

        }

        [Fact]
        public async void GetSports_Returns_ListOf_FavouriteSportsQuery()
        {
            // TODO
            var dataContext = await getDbContext();
            SportsController sportController = new SportsController(dataContext);
            List<FavouriteSportQuery> expectedListFavouriteSportQuery = new List<FavouriteSportQuery>
            { new FavouriteSportQuery
            {
                       Name = "American Football",
                       Favourites = 2
            },
            new FavouriteSportQuery
            {
                       Name = "Baseball",
                       Favourites = 2
            },
            new FavouriteSportQuery
            {
                       Name = "Basketball",
                       Favourites = 2
            },
            };

            // Act 
            ActionResult<List<FavouriteSportQuery>> actionResult = await sportController.GetSports();
            var result = actionResult.Result as OkObjectResult;
            var actualListFavouriteSportQuery = result.Value as List<FavouriteSportQuery>;

            // Assert
            actualListFavouriteSportQuery.Should().BeEquivalentTo(expectedListFavouriteSportQuery);

        }



    }
}