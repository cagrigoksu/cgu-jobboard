using System.ComponentModel;

namespace JobBoard.Enums
{
    public enum ApplicationStatusEnum : int
    {
        [Description("Under Consideration")]
        UnderConsideration = 0,
        [Description("In Progress")]
        InProgress = 1,
        [Description("Not Selected")]
        NotSelected = 2,
    }
}
