using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNew.Worker
{
    public class TrainingStepDoneArgs : EventArgs
    {
        public int StepsToCalculatePrecision { get; set; }
        public int Correct { get; set; }

        public TrainingStepDoneArgs(int stepsToCalculatePrecision, int correct)
        {
            this.StepsToCalculatePrecision = stepsToCalculatePrecision;
            this.Correct = correct;
        }
    }
}
