using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings2.Models
{
    public class Trener : Person
    {
        public int trenerid { get; set; }
        Person person;

        public Trener()
        {
        }

        public Trener(int trenerid, Person person)
        {
            this.trenerid = trenerid;
            this.person = person ?? throw new ArgumentNullException(nameof(person));
        }
    }
}
