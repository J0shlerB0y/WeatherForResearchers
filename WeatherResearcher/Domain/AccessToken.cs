using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AccessToken : IEntity
    {
        [Key]
        public static string deffaultAccessTokenName = "accessToken";

        public int Id { get; set; } = 0;
        public string Hash { get; set; }
        public string Salt { get; set; }

        public DateTime? Time { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
