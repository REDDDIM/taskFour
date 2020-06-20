using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Task02.Models
{
    public class UserContext : DbContext
    {
        public UserContext()
            : base("UserDB")
        { }

        public DbSet<User> Users { get; set; }
    }
}