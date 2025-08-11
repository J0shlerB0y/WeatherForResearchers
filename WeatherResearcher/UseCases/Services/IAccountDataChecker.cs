using Domain;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Mutations;

namespace UseCases.Services
{
    public interface IAccountDataChecker
    {
        public bool CheckLoginData(string login, string password);
        public bool CheckRegistrData(string login, string password, ViewDataDictionary ViewData);
    }
}
