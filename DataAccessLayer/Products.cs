using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Products
    {
        public int ID { get; set; }
        public string Barcode { get; set; }
        public int ProductCodeID { get; set; }
        public string ProductCode { get; set; }
        public DateTime CastDate { get; set; }
        public int CastPersonal { get; set; }
        public int GlazingTerritory { get; set; }
        public int QualityID { get; set; }
        public string Quality { get; set; }
        public int FaultID { get; set; }
        public string Fault { get; set; }
        public DateTime ControlDate { get; set; }
        public int KilnID { get; set; }
        public string Kiln { get; set; }
        public int FiringID { get; set; }
        public string Firing { get; set; }
        public int ColorID { get; set; }
        public string Color { get; set; }
        public int StockTerritoryID { get; set; }
        public string StockTerritory { get; set; }
        public string IsItTest { get; set; }
        public DateTime ControlTime { get; set; }
    }
}
