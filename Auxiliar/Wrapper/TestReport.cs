using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliar.Wrapper
{
    public class TestReport
    {
        public EntityTestReport TestReportAgg { get; set; } = new EntityTestReport();
        public ConcurrentDictionary<int, EntityTestReport> ReportProEntity { get; set; } = new ConcurrentDictionary<int, EntityTestReport>();
        
        public TestReport()
        {
        }

        public void SetNrOfEntities(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ReportProEntity.TryAdd(i, new EntityTestReport());
            }
        }

        public void AddCorrect(int? index = null)
        {
            TestReportAgg.AddCorrect();

            if (index != null)
            {
                ReportProEntity[index.Value].AddCorrect();
            }
        }

        public void AddIncorrect(int? index = null)
        {
            TestReportAgg.AddIncorrect();

            if (index != null)
            {
                ReportProEntity[index.Value].AddIncorrect();
            }
        }
    }
}
