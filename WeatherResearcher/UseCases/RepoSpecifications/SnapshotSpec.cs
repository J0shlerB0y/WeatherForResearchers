using Domain;
using System.Collections.Generic;
using UseCases.Mutations;

namespace UseCases.RepoSpecifications
{
    public class SnapshotCriteriaSpec : BaseCriteriaSpec<Snapshot>
    {
        public SnapshotCriteriaSpec( int snapshotId)
            : base(c =>c.Id == snapshotId)
        {
        }
        public SnapshotCriteriaSpec(User user)
            : base(c => c.User.Login == user.Login)
        {
        }
    }

    public class SnapshotWithCountrySpec : BaseIncludesSpec<Snapshot>
    {
        public SnapshotWithCountrySpec()
        {
            AddInclude(s => s.City);
            AddInclude(s => s.City.Country);
            AddInclude(s => s.User);
        }
    }
}