
using Domain;


namespace UseCases.RepoSpecifications.Filters;

public class UsersCityFilterCriteriaSpec : BaseCriteriaSpec<UsersCity>
{
    public UsersCityFilterCriteriaSpec(User user) : base(
        c => c.User.Id == user.Id
        )
    { }

    public UsersCityFilterCriteriaSpec(FilterCityParametr filter) : base(
        c => (string.IsNullOrEmpty(filter.Country) ||filter.City == c.City.CityTitle_en) &&
            (string.IsNullOrEmpty(filter.Country) || filter.Country == c.City.Country.CountryTitle_en)
        )
    {}
}