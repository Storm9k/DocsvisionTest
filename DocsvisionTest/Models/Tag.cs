using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsvisionTest.Models
{
    //Модель тэга
    public class Tag
    {
        public int ID { get; set; }
        public string TagName { get; set; }
        public int MessageID { get; set; }
    }
}
