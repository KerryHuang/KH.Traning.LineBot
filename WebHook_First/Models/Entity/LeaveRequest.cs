using isRock.LineBot.Conversation;
using System;
namespace WebHook_First.Models.Entity
{
    public class LeaveRequest : ConversationEntity
    {
        //[Question("請問您要請的假別是?")]
        [ButtonsTemplateQuestion("詢問", "請問您要請的假別是?", "https://kerrylinebot.azurewebsites.net/Images/logo.jpg", "事假", "病假", "公假", "婚假")]
        [Order(1)]
        public string 假別 { get; set; }

        [Question("請問您的代理人是誰?")]
        [Order(2)]
        public string 代理人 { get; set; }

        [Question("請問您的請假日期是?")]
        [Order(3)]
        public DateTime 請假日期 { get; set; }

        [Question("請問您的開始時間是幾點幾分?")]
        [Order(4)]
        public DateTime 開始時間 { get; set; }

        [Question("請問您要請幾小時?")]
        [Order(5)]
        public float 請假時數 { get; set; }
    }
}