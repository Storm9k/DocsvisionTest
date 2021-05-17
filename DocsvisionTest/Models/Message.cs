using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsvisionTest.Models
{   
    //Модель письма
    public class Message
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
        public string Recipient { get; set; }
        public int SenderID { get; set; }
        public Sender Sender { get; set; }
        public string Content { get; set; }
        public bool Important { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
