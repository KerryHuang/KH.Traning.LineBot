using isRock.LineBot.Conversation;
using System;

namespace LineBotApi.Models.Entity
{
    public class LeaveRequest : ConversationEntity
    {
        [Question("請問您要請的假別是?")]
        [Order(1)]
        public string 假別 { get; set; }

        [Question("請問您的請假日期是?")]
        [Order(2)]
        public DateTime 請假日期 { get; set; }

        [Question("請問您的開始時間是幾點幾分?")]
        [Order(3)]
        public DateTime 開始時間 { get; set; }

        [Question("請問您要請幾小時?")]
        [Order(4)]
        public float 請假時數 { get; set; }
    }
}