using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                        Sports = new List<Sport> { sport1, sport2 }
                    },
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Cena",
                        IsAuthorised = true,
                        IsEnabled = true,
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
            PeopleController peopleController = new PeopleController( dataContext );
            PersonSportQuery expectedPersonSportQuery = new PersonSportQuery
            {
                FirstName = "John",
                LastName = "Snow",
                FavouriteSport = "American Football"
            };

            // Act 
            ActionResult<PersonSportQuery> actionResult = await peopleController.GetPerson(1L);
            
            // Assert
            

        }
    }
}