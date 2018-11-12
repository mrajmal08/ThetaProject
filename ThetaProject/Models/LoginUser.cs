using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThetaProject.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public String FName { get; set; }
        public String LName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Gender { get; set; }
        public DateTime? DOB { get; set; }
        public String City { get; set; }
        public string Country { get; set; }
    }
}
