using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocsvisionTest.Models;
using Microsoft.Extensions.Configuration;

namespace DocsvisionTest.Controllers
{
    public class MainController : Controller
    {
        private IConfiguration configuration;
        private readonly DBInterface _DBInterface;

        public MainController (IConfiguration _configuration, DBInterface dBInterface)
        {
            configuration = _configuration;
            _DBInterface = dBInterface;
        }
        //Метод для главной страницы
        public IActionResult Index()
        {
            return View(_DBInterface.GetSenderList());
        }
        //Метод для отображения всех писем или писем по пользователю
        public IActionResult MessageList(int sender_id)
        {
            if (sender_id == 0) return View(_DBInterface.GetMessageList());
            else return View(_DBInterface.GetMessageList(sender_id));
        }
        //Get-метод отображения данных письма с последующим редактированием
        [HttpGet]
        public IActionResult Message(int message_id, int tags_count)
        {
            ViewBag.TagsCount = tags_count;
            return View(_DBInterface.GetMessage(message_id));
        }
        //Post-метод для принятия изменений в письме с сохранением в БД
        [HttpPost]
        public IActionResult Message(Message message)
        {
            
            _DBInterface.EditMessage(message);
            return RedirectToAction("Index");
        }

        //Тестовый метод сортировки писем
        //public IActionResult MessageListSamples(string sortby)
        //{
        //    switch(sortby)
        //    {
        //        case "byDateTime": _DBInterface.GetMessageList(_DBInterface.GetMessageListByDatetime(DateTime.Now, DateTime.UtcNow));
        //            break;
        //        case "ByRecipient":
        //            _DBInterface.GetMessageList(_DBInterface.GetMessageListByRecipient("test@test.com"));
        //            break;
        //        case "BySender":
        //            _DBInterface.GetMessageList(_DBInterface.GetMessageListBySender(new Sender { Address = "tanyaivanova@dv.com" }));
        //            break;
        //        case "ByTag":
        //            _DBInterface.GetMessageList(_DBInterface.GetMessageListByTag("Важное"));
        //            break;
        //        default: return RedirectToAction("Index");
        //    }
        //    return View();
        //}
    }
}
