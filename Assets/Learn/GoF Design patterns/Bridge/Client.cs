using Learn.GoF_Design_patterns.Bridge.Outputs;
using Learn.GoF_Design_patterns.Bridge.Reports;

namespace Learn.GoF_Design_patterns.Bridge
{
    public class Client
    {
        private static void Main()
        {
            var report = new MonthlyReport();
            
            report.PrintReport(new XMLOutput());
        }
    }
}