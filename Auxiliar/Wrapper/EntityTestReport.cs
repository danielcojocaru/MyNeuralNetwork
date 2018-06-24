using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliar.Wrapper
{
    public class EntityTestReport
    {
        public int Tested { get; private set; }
        public int Correct { get; private set; }
        public int Incorrect { get => Tested - Correct; }
        public double Percentage { get => Math.Round((double)Correct / (double)Tested * 100, 2); }

        public EntityTestReport()
        {
        }

        public void AddCorrect()
        {
            Tested++;
            Correct++;
        }

        public void AddIncorrect()
        {
            Tested++;
        }

        public override string ToString()
        {
            return string.Format("Correct = {0} ~ Tested = {1} ~ Percentage = {2}", Correct, Tested, Percentage);
        }
    }
}
