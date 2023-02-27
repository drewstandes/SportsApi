using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SportsApi.Controllers;
using SportsApi.Data;
using SportsApi.Models;
using SportsApi.Queries.People;
using SportsApi.Queries.Sports;

namespace SportsApiTest
{
    public class UnitTest
    {
        private Person[] getPeople(Sport[] sports)
        {
           return new Person[]
         {
             new Person
                    {
                        FirstName = "John",
                        LastName = "Snow",
                        IsAuthorised = true,
                        IsEnabled = true,
                        IsValid = true,
                        Sports = new List<Sport> { sports[0], sports[1], sports[2] }
                    },
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Wick",
                        IsAuthorised = true,
                        IsEnabled = true,
                        IsValid = true,
                        Sports = new List<Sport> { sports[1], sports[2] }
                    },
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Cena",
                        IsAuthorised = true,
                        IsEnabled = false,
                        IsValid = true,
                        Sports = new List<Sport> { sports[0] }
                    }
         };
        }

        private Sport[] getSports()
        {
            return new Sport[] {
            new Sport { Name = "American Football", IsEnabled = true },
            new Sport { Name = "Basketball", IsEnabled = true },
            new Sport { Name = "Baseball", IsEnabled = true }
        };
        }

        private async Task<DataContext> getDbContext(Person[] people)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dataContext = new DataContext(options);
            dataContext.Database.EnsureCreated();

            if (await dataContext.People.CountAsync() <= 0)
            {
                dataContext.People.AddRange(
                    people
                    );
                await dataContext.SaveChangesAsync();
            }
            return dataContext;

        }

        private DataContext getEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dataContext = new DataContext(options);
            dataContext.Database.EnsureCreated();
            return dataContext;
        }


        [Fact]
        public async void GetPerson_Returns_PersonSportQuery()
        {
            // Arrange 
            Person[] actualPeople = getPeople(getSports());
            var dataContext = await getDbContext(actualPeople);
            PeopleController peopleController = new PeopleController(dataContext);
            PersonSportQuery expectedPersonSportQuery = new PersonSportQuery
            {
                FirstName = actualPeople[1].FirstName,
                LastName = actualPeople[1].LastName,
                FavouriteSport = actualPeople[1].Sports.First().Name
            };

            // Act 
            ActionResult<PersonSportQuery> actionResult = await peopleController.GetPerson(actualPeople[1].Id);
            var result = actionResult.Result as OkObjectResult;
            var actualPersonSportQuery = result.Value as PersonSportQuery;

            // Assert
            actualPersonSportQuery.Should().BeEquivalentTo(expectedPersonSportQuery);

        }


        [Fact]
        public async void GetPersonInvalidId_Returns_BadRequest()
        {
            // Arrange
            var dataContext = await getDbContext(getPeople(getSports()));
            PeopleController peopleController = new PeopleController(dataContext);

            // Act 
            ActionResult<PersonSportQuery> actionResult = await peopleController.GetPerson(4L);
            var result = actionResult.Result as BadRequestObjectResult;


            // Assert
            result.Value.Should().Be("There is no person with that Id.");

        }

        [Fact]
        public async void GetPeople_Returns_ListOf_PersonFavouritesQuery()
        {
            // Arrange 
            Person[] actualPeople = getPeople(getSports());
            var dataContext = await getDbContext(actualPeople);
            List<PersonFavouritesQuery> expectedListPersonFavouritesQuery = new List<PersonFavouritesQuery>
            { new PersonFavouritesQuery
            {
                        FirstName = actualPeople[0].FirstName,
                        LastName = actualPeople[0].LastName,
                        IsAuthorised = actualPeople[0].IsAuthorised,
                        IsEnabled = actualPeople[0].IsEnabled,
                        IsValid = actualPeople[0].IsValid,
                        FavouriteSports = new List<string> { actualPeople[0].Sports.ElementAt(0).Name, actualPeople[0].Sports.ElementAt(1).Name, actualPeople[0].Sports.ElementAt(2).Name }
            },
            new PersonFavouritesQuery
            {
                         FirstName = actualPeople[1].FirstName,
                        LastName = actualPeople[1].LastName,
                        IsAuthorised = actualPeople[1].IsAuthorised,
                        IsEnabled = actualPeople[1].IsEnabled,
                        IsValid = actualPeople[1].IsValid,
                        FavouriteSports = new List<string> { actualPeople[1].Sports.ElementAt(0).Name, actualPeople[1].Sports.ElementAt(1).Name}
            },
            new PersonFavouritesQuery
            {
                        FirstName = actualPeople[2].FirstName,
                        LastName = actualPeople[2].LastName,
                        IsAuthorised = actualPeople[2].IsAuthorised,
                        IsEnabled = actualPeople[2].IsEnabled,
                        IsValid = actualPeople[2].IsValid,
                        FavouriteSports = new List<string> { actualPeople[2].Sports.ElementAt(0).Name}
            },
            };

            PeopleController peopleController = new PeopleController(dataContext);



            // Act 
            ActionResult<List<PersonFavouritesQuery>> actionResult = await peopleController.GetPeople();
            var result = actionResult.Result as OkObjectResult;
            var actualListPersonFavouritesQuery = result.Value as List<PersonFavouritesQuery>;

            // Assert
            actualListPersonFavouritesQuery.Should().BeEquivalentTo(expectedListPersonFavouritesQuery);

        }

        [Fact]
        public async void GetPeople_NoSports_Returns_With_None()
        {
            // Arrange 
            var dataContext = getEmptyDbContext();
            Person actualPerson = getPeople(getSports()).ElementAt(0);
            // Empty list of sports 
            actualPerson.Sports = new List<Sport> { };
            if (await dataContext.People.CountAsync() <= 0)
            {
                dataContext.People.Add(
                   actualPerson
                    );
                await dataContext.SaveChangesAsync();
            }

            List<PersonFavouritesQuery> expectedListPersonFavouritesQuery = new List<PersonFavouritesQuery>
            { new PersonFavouritesQuery
            {
                        FirstName = actualPerson.FirstName,
                        LastName = actualPerson.LastName,
                        IsAuthorised = actualPerson.IsAuthorised,
                        IsEnabled = actualPerson.IsEnabled,
                        IsValid = actualPerson.IsValid,
                        FavouriteSports = new List<string> { }
            } };
            PeopleController peopleController = new PeopleController(dataContext);

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
            var dataContext = await getDbContext(getPeople(getSports()));
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

        [Fact]
        public async void GetSports_On_EmptyTable_Returns_BadRequest()
        {
            // Arrange
            var dataContext = getEmptyDbContext();
            SportsController sportsController = new SportsController(dataContext);

            // Act 
            ActionResult<List<FavouriteSportQuery>> actionResult = await sportsController.GetSports();
            var result = actionResult.Result as BadRequestObjectResult;


            // Assert
            result.Value.Should().Be("There are no sports to return.");

        }



    }
}