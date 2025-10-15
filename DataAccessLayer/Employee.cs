using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Employee
    {
        public byte ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NameSurname { get; set; }
        public byte Status { get; set; }
        public string PcName { get; set; }
        public string Version { get; set; }
        public string ShortName { get; set; }
        public string Department { get; set; }
    }
}
