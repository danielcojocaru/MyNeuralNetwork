using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auxiliar.Wrapper
{
    public class EntityTestReport
    {
        private int _tested;
        public int Tested { get => _tested; }

        private int _correct;
        public int Correct { get => _correct; }

        //private int _incorrect;
        public int Incorrect { get => Tested - Correct; }

        public double Percentage { get => Math.Round((double)Correct / (double)Tested * 100, 2); }

        public EntityTestReport()
        {
        }

        public void AddCorrect()
        {
            Interlocked.Increment(ref _tested);
            Interlocked.Increment(ref _correct);
        }

        public void AddIncorrect()
        {
            Interlocked.Increment(ref _tested);
            //Interlocked.Increment(ref _incorrect);
        }

        public override string ToString()
        {
            return string.Format("Percentage = {0} / Correct = {1} / Tested = {2}", Percentage, Correct, Tested);
        }
    }
}
