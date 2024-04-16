using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FuckingSchoolProject
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext():base("DefaultConnection")
        {

        }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Class> Classes { get; set; }
    }
}
