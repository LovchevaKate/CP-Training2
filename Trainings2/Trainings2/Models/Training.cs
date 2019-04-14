using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings2.Models
{
    public class Training
    {
        public int idtraining { get; set; }
        public int idcoach { get; set; }
        public int idsportsman { get; set; }
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }
        public string type { get; set; }

        public Training()
        {
        }

        public Training(int idtraining, int idcoach, int idsportsman, DateTime date, TimeSpan time, string type)
        {
            this.idtraining = idtraining;
            this.idcoach = idcoach;
            this.idsportsman = idsportsman;
            this.date = date;
            this.time = time;
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
