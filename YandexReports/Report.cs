//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YandexReports
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        public int ID { get; set; }
        public int DayID { get; set; }
        public int AmountDelievery { get; set; }
        public int MoneyForDelievery { get; set; }
        public int MoneyForDistance { get; set; }
        public int Tips { get; set; }
        public int TipsFromYandex { get; set; }
        public int TransportCompensation { get; set; }
    
        public virtual Day Day { get; set; }
    }
}
