using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.FeeModule.Response
{
    public class FeeResponseStaff
    {
        public string FeeName { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double ParticipationFee { get; set; }
        public double DepositFee { get; set; }
        public double Surcharge { get; set; }
    }
}
