using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Domain
{
    public class UserDTO : LoginDTO
    {
        public string UserName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Role { get; set; }
    }

    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
