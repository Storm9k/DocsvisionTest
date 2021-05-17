using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsvisionTest.Models
{
    //Модель отправителя
    public class Sender
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
    }
}
