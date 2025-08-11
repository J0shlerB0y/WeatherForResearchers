using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Domain;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UseCases.Mutations;
using UseCases.RepoSpecifications;

namespace UseCases.Services
{
	public class AccountDataChecker : IAccountDataChecker
	{
        private IHasher hasher;
        private IReadWithAdditionalToolsRepository<User> repo;

        public AccountDataChecker(IHasher hasher, IReadWithAdditionalToolsRepository<User> repo)
        {
            this.hasher = hasher;
            this.repo = repo;
        }

		public bool CheckLoginData(string login, string password)
		{
			if (login != null && password != null)
			{
                repo.Filter(new UserCriteriaSpec(login));
                IEnumerable<User> userInList = repo.GetTheRest();
				if (userInList is not null && userInList.Count() == 1)
				{
					User user = userInList.FirstOrDefault();
					// Hash the password using the user's salt
					string hashedPassword = hasher.Hash(password, user.Salt.Split('-').Select(hex => Convert.ToByte(hex)).ToArray());
					if (hashedPassword == user.Password) return true;
				}
			}
			return false;
		}

		public bool CheckRegistrData(string login, string password, ViewDataDictionary ViewData = null)
		{
            if (login != null && password != null)
            {
                if (password.Length >= 8 && password.Length <= 50)
                {
                    if (repo.GetByName(new UserCriteriaSpec(login)) is null)
                    {
                        return true;

                    }
                    else
                    {
                        ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\"><h3>Login already in use</h3></div><br />";
						return false;
                    }
                }
                else
                {
                    ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\"><h3>Line length must be between 8 and 50 characters</h3></div><br />";
                    return false;

                }
            }
            else
            {
                ViewData["WrongLoginOrPasswordMessage"] = "<br /><div id=\"errorMessage\" class=\"error-message\"><h3>Login or Password is empty</h3></div><br />";
                return false;
            }
            return false;
        }
	}
}