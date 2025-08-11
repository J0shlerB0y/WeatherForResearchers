using Domain;
using System.Collections.Generic;
using UseCases.Mutations;

namespace UseCases.RepoSpecifications
{
    public class UsersCityCriteriaSpec : BaseCriteriaSpec<UsersCity>
    {
        public UsersCityCriteriaSpec(int userId, int cityId)
            : base(c => c.User.Id == userId && c.City.Id == cityId)
        {
        }
    }

    public class UsersCityWithCountrySpec : BaseIncludesSpec<UsersCity>
    {
        public UsersCityWithCountrySpec()
        {
            AddInclude(c => c.City.Country);
        }
    }
}