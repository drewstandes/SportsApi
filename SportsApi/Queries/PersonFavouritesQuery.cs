using System.ComponentModel.DataAnnotations;

namespace SportsApi.Queries
{
    public class PersonFavouritesQuery
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAuthorised { get; set; }

        public bool IsValid { get; set; }

        public bool IsEnabled { get; set; }
        public List<string> FavouriteSports { get; set; }
    }
}
