using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Task02.Models
{
    public class NoteContext : DbContext
    {
        public NoteContext()
           : base("UserDB")
        { }

        public DbSet<Note> Notes { get; set; }
    }
}