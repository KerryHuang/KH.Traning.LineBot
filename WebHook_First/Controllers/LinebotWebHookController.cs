using System;
using System.Text;
using System.Web.Http;

namespace WebHook_First.Controllers
{
    /// <summary>
    /// 透過ReplyToken回覆訊息
    /// </summary>
    public class LinebotWebHookController : ApiController
    {
        private const string ChannelAccessToken = "~~~請改成你的Linebot的ChannelAccessToken~~~";
        private const string AdminUserId = "~~~改成你的AdminUserId~~~";

        [HttpPost]
        public IHttpActionResult POST()
        {
            string replyToken = "";

            try
            {
                // 回覆訊息
                StringBuilder sbMessage = new StringBuilder();

                //取得 http Post RawData(should be JSON)
                string postData = Request.Content.ReadAsStringAsync().Result;
                //sbMessage.Append("收到的 RawData：").AppendLine(postData);

                //剖析JSON
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                replyToken = ReceivedMessage.events[0].replyToken;

                // 取得使用者資訊
                string userId = ReceivedMessage.events[0].source.userId;
                isRock.LineBot.Bot bot = new isRock.LineBot.Bot(ChannelAccessToken);
                string displayName = "無名";
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var userInfo = bot.GetUserInfo(userId);
                    // 取得使用者詳細資訊
                    displayName = userInfo.displayName;
                }

                // 用戶傳來message的type
                //sbMessage.Append("收到的 event 的類型為：").AppendLine(ReceivedMessage.events[0].type);

                if (ReceivedMessage.events[0].type == "message")
                {
                    //回覆訊息
                    sbMessage.Append(displayName).Append("說了：").AppendLine(ReceivedMessage.events[0].message.text);
                }

                if (ReceivedMessage.events[0].type == "sticker")
                {
                    sbMessage.Append("packageId：").Append(ReceivedMessage.events[0].message.packageId).AppendLine();
                    sbMessage.Append("stickerId：").Append(ReceivedMessage.events[0].message.stickerId).AppendLine();
                }

                // 取得地理位置
                if (ReceivedMessage.events[0].type == "address")
                {
                    sbMessage.Append("address：").AppendLine(ReceivedMessage.events[0].message.address);
                    sbMessage.Append("latitude：").Append(ReceivedMessage.events[0].message.latitude).AppendLine();
                    sbMessage.Append("longitude：").Append(ReceivedMessage.events[0].message.longitude).AppendLine();
                }

                //回覆用戶
                isRock.LineBot.Utility.ReplyMessage(replyToken, sbMessage.ToString(), ChannelAccessToken);

                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                //回覆訊息
                string message = "我錯了，錯在 " + ex.Message;

                //回覆用戶
                this.PushMessage(AdminUserId, message);

                return Ok();
            }
        }
    }
}
