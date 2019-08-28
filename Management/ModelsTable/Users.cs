using System;
using System.Collections.Generic;

namespace Management.ModelsTable
{
    public partial class Users
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? Level { get; set; }
    }
}
