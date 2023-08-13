namespace RumineActivity.View
{
    public class DiagramComponent : ReportComponent
    {
        protected string GetValueForAttr(double value)
        {
            return value.ToString().Replace(',', '.');
        }
    }
}
