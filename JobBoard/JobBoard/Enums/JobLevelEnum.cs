using System.ComponentModel;

namespace JobBoard.Enums
{
    public enum JobLevelEnum: int
    {
        [Description("Inexperienced")]
        Inexperienced = 0,
        [Description("Internship")]
        Internship = 1,
        [Description("Entry Level")]
        EntryLevel = 2,
        [Description("Associate")]
        Associate = 3,
        [Description("Mid-Senior")]
        MidSenior = 4,
        [Description("Director")]
        Director = 5,
        [Description("Executive")]
        Executive = 6
    }
}
