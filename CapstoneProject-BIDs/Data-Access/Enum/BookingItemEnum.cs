using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Enum
{
    public enum BookingItemEnum
    {
        Waitting = 1,
        Accepted = 2,
        NotCreateSessionYet = 3,
        Denied = 4,
        SessionWaitting = 5,
        InActive = -1
    }
}
