using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexReports
{
    public class Persona
    {
        public int PersonID { get; set; }
        public int TotalMoney { get; set; }
    }
    public class Persons
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TotalMoney { get; set; } = 0;
        public int TotalDelievery { get; set; } = 0;
    }
    public class Days
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public DateTime Date { get; set; }
        public int TotalMoney { get; set; }
        public int TotalDelievery { get; set; }
    }
}
