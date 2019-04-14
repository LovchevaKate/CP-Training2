using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings2.Models
{
    public class Food
    {
        public Food()
        {
        }

        public Food(int idfood, string foodname, int protein, int fat, int carbohydrates, int calories)
        {
            this.idfood = idfood;
            this.foodname = foodname ?? throw new ArgumentNullException(nameof(foodname));
            this.protein = protein;
            this.fat = fat;
            this.carbohydrates = carbohydrates;
            this.calories = calories;
        }

        public int idfood { get; set; }
        public string foodname { get; set; }
        public int protein { get; set; }
        public int fat { get; set; }
        public int carbohydrates { get; set; }
        public int calories { get; set; }
    }
}
