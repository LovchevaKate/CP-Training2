using System;

namespace Trainings2.Models
{
    public class Sportsman : Person
    {
        public int sportsmanid;
        public int trenerid;
        Person person;

        public Sportsman()
        {
        }

        public Sportsman(int sportsmanid, int trenerid, Person person)
        {
            this.sportsmanid = sportsmanid;
            this.trenerid = trenerid;
            this.person = person ?? throw new ArgumentNullException(nameof(person));
        }
    }
}
