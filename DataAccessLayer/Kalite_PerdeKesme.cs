using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Kalite_PerdeKesme
    {
        public int ID { get; set; }
        public string Barcode { get; set; }
        public byte QualityID { get; set; }
        public string Quality { get; set; }
        public int? ResultID { get; set; }
        public string Result { get; set; }
        public DateTime Datetime { get; set; }
        public byte QualityPersonalID { get; set; }
        public string QualityPersonal { get; set; }
    }
}
