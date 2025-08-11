using Domain;

namespace UseCases.RepoSpecifications
{
    public class UserCriteriaSpec : BaseCriteriaSpec<User>
    {
        public UserCriteriaSpec(string userName)
            : base(u => u.Login == userName)
        {
        }
    }
    public class UserIncludeSpec : BaseIncludesSpec<User>
    {
        public UserIncludeSpec()
        {
        }
    }
}