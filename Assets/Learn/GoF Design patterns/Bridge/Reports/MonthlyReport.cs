namespace Learn.GoF_Design_patterns.Bridge.Reports
{
    public class MonthlyReport : Report
    {
        public override void PrintReport(Output output)
        {
            output.Print();
        }
    }
}