namespace InvestCloud.HelpApi
{
    public partial class Numbers
    {
        public class ApiResponse<TValue>
        {
            public TValue Value { get; set; }
            public string Cause { get; set; }
            public bool Success { get; set; }
        }
    }
}
