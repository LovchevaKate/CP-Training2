using System;

namespace Trainings2.Models
{
    public class Diet
    {
        public Diet()
        {
        }

        public Diet(int dietid, int sportsmanid, DateTime date, string dietperiod, int mealid)
        {
            this.dietid = dietid;
            this.sportsmanid = sportsmanid;
            this.date = date;
            this.dietperiod = dietperiod ?? throw new ArgumentNullException(nameof(dietperiod));
            this.mealid = mealid;
        }

        public Diet(int dietid, int sportsmanid, DateTime date, string dietperiod, int mealid, string morning, string lunch, string diner) : this(dietid, sportsmanid, date, dietperiod, mealid)
        {
            this.morning = morning ?? throw new ArgumentNullException(nameof(morning));
            this.lunch = lunch ?? throw new ArgumentNullException(nameof(lunch));
            this.diner = diner ?? throw new ArgumentNullException(nameof(diner));
        }

        public int dietid { get; set; }
        public int sportsmanid { get; set; }
        public DateTime date { get; set; }
        public string dietperiod { get; set; }
        public int mealid { get; set; }
        public string morning { get; set; }
        public string lunch { get; set; }
        public string diner { get; set; }
    }
}
