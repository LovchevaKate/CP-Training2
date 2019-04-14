using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings2.Models
{
    public class Indicator
    {
        public Indicator()
        {
        }

        public Indicator(int indicatorsID, int trainingID, int pulseUnWork, int pulseWork, int pressureSistol, int pressureDiastol)
        {
            this.indicatorsID = indicatorsID;
            this.trainingID = trainingID;
            this.pulseUnWork = pulseUnWork;
            this.pulseWork = pulseWork;
            this.pressureSistol = pressureSistol;
            this.pressureDiastol = pressureDiastol;
        }

        public int indicatorsID { get; set; }

        public int trainingID { get; set; }

        public int pulseUnWork { get; set; }

        public int pulseWork { get; set; }

        public int pressureSistol { get; set; }

        public int pressureDiastol { get; set; }
    }
}
