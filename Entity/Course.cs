using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971.Entity
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

  //      public string Notes { get; set; }
        public string Instructor { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Notifications { get; set; }

    }
}
