using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Services
{
    public interface IJwtManager
    {
        public User GetUser();
        public bool DeleteJwt();
        public void SetJwt(string token);
        public string GetJwt();
    }
}
