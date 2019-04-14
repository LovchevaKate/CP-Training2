using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings2.Models
{
    public class Meal : Food
    {
        public Meal()
        {
        }

        public Meal(int mealid, string mealname)
        {
            this.mealid = mealid;
            this.mealname = mealname ?? throw new ArgumentNullException(nameof(mealname));
        }

        public int mealid { get; set; }
        public string mealname { get; set; }
    }
}
