using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Enum
{
    public enum SessionStatusEnum
    {
        NotStart = 1,
        InStage = 2,
        HaventTranferYet = 3,
        Complete = 4,
        Fail = 5,
        Received = 6,
        ErrorItem = 7,
        Delete = -1
    }
}
