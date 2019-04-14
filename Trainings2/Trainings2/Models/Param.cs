using System;

namespace Trainings2.Models
{
    public class Param
    {
        public Param()
        {
        }

        public Param(DateTime date, int idsportsman, int height, int weight, int arms, int waist, int chest, int leg, int hip)
        {
            this.date = date;
            this.idsportsman = idsportsman;
            this.height = height;
            this.weight = weight;
            this.arms = arms;
            this.waist = waist;
            this.chest = chest;
            this.leg = leg;
            this.hip = hip;
        }

        public DateTime date { get; set; }
        public int idsportsman { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public int arms { get; set; }
        public int waist { get; set; }
        public int chest { get; set; }
        public int leg { get; set; }
        public int hip { get; set; }
    }
}
