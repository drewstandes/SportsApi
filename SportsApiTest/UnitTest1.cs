using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using SportsApi.Controllers;
using SportsApi.Data;
using SportsApi.Models;

namespace SportsApiTest
{
    public class UnitTest1
    {
        private DbContextMock<DataContext> getDbContext(Sport[] initialSports, Person[] initialPeople)
        {
            DbContextMock<DataContext> dbContextMock = new DbContextMock<DataContext>(new DbContextOptionsBuilder<DataContext>().Options);
            dbContextMock.CreateDbSetMock(x => x.People, initialPeople);
            return dbContextMock;
        }

        private PeopleController InitPeopleController(DbContextMock<DataContext> dbContextMock)
        {
            return new PeopleController(dbContextMock.Object);
        }

        private Person[] getPeople()
        {
            Person[] people = new Person[]
            {
                new Person
                {
                    Id = 1,
                   FirstName = "John",
                   LastName = "Snow",
                   IsEnabled = true,
                   IsAuthorised = true,
                   IsValid = true,
                   Sports = getSports().Where(s => s.Id == 1).ToList(),
                },
                new Person
                {
                    Id = 2,
                   FirstName = "Bob",
                   LastName = "Peters",
                   IsEnabled = true,
                   IsAuthorised = true,
                   IsValid = true,
                   Sports = getSports(),
                }
            };
           
            return people;
        }

        private Sport[] getSports()
        {
            Sport[] sports = new Sport[]
            {
                new Sport
                {
                    Id = 1,
                    Name = "American Football",
                    IsEnabled = true,
                },
                new Sport
                {
                    Id = 2,
                    Name = "Basketball",
                    IsEnabled = true,
                },
            };
            return sports;
        }

        [Fact]
        public void Test1()
        {
            DbContextMock<DataContext> dbContextMock = getDbContext(getSports(), getPeople());
            PeopleController peopleController = InitPeopleController(dbContextMock);

        }
    }
}