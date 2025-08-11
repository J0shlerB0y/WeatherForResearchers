using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.RepoSpecifications
{
    public class AccessTokenCriteriaSpec : BaseCriteriaSpec<AccessToken>
    {
        public AccessTokenCriteriaSpec(string accessToken)
            : base(c => c.Hash == accessToken)
        {
        }

        public AccessTokenCriteriaSpec(int accessTokenId)
            : base(c => c.Id == accessTokenId)
        {
        }
    }

    public class AccessTokenyWithUserSpec : BaseIncludesSpec<AccessToken>
    {
        public AccessTokenyWithUserSpec()
        {
            AddInclude(c => c.User);
        }
    }
}
