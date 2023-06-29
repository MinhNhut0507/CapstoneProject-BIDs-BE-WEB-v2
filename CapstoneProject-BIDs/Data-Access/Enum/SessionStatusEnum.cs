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
        OutOfDate = 5,
        Delete = -1
    }
}
