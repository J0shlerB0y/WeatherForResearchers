using Domain;
using System.Collections.Generic;
using UseCases.Mutations;

namespace UseCases.RepoSpecifications
{
    public class CityCriteriaSpec : BaseCriteriaSpec<City>
    {
        public CityCriteriaSpec(string cityName)
            : base(c => c.CityTitle_en == cityName)
        {
        }
    }

    public class CityWithCountrySpec : BaseIncludesSpec<City>
    {
        public CityWithCountrySpec()
        {
            AddInclude(c => c.Country);
        }
    }

}