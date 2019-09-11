using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Classes
{
    public class UserProfile
    {
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }

    }
    public class UserImg
    {
        public long UserId { get; set; }
        public string Photo { get; set; }
    }
}
