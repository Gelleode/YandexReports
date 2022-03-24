using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexReports
{
    public class Persons
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal TotalMoney { get; set; } = 0;
        public int TotalDelievery { get; set; }
    }
    public class Days
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalMoney { get; set; }
        public int TotalDelievery { get; set; }
    }
}
