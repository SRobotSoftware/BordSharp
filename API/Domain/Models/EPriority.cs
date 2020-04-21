using System.ComponentModel;

namespace API.Domain.Models
{
    public enum EPriority : byte
    {
        [Description("")]
        low = 1,
        [Description("!")]
        medium = 2,
        [Description("!!")]
        high = 3
    }
}
