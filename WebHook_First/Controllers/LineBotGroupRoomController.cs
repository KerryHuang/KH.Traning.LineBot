using isRock.LineBot;
using System;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace WebHook_First.Controllers
{
    /// <summary>
    /// 訊息來自聊天室或群組
    /// </summary>
    /// <seealso cref="isRock.LineBot.LineWebHookControllerBase" />
    public class LineBotGroupRoomController : isRock.LineBot.LineWebHookControllerBase
    {
        //private const string ChannelAccessToken = "~~~請改成你的Linebot的ChannelAccessToken~~~";
        private const string AdminUserId = "~~~改成你的AdminUserId~~~";

        [Route("api/LineBotGroupRoom")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            // TODO:請換成你自己的token
            //this.ChannelAccessToken = "";
            
            try
            {
                // 回覆訊息
                StringBuilder sbMessage = new StringBuilder();
                var item = this.ReceivedMessage.events.FirstOrDefault();

                //用戶傳來message的type
                //sbMessage.Append("收到的 event 的類型為：").AppendLine(item.type);

                switch (item.type)
                {
                    case "join":
                        sbMessage.Append("有人把我加入").Append(item.source.type).AppendLine("中了，大家好啊~");
                        // 回覆用戶
                        this.ReplyMessage(ReceivedMessage.events[0].replyToken, sbMessage.ToString());
                        break;
                    case "message":
                        if (item.message.text == "bye")
                        {
                            // 回覆用戶
                            this.ReplyMessage(item.replyToken, "bye-bye");
                            // 離開
                            if (string.Equals(item.source.type, "room", StringComparison.OrdinalIgnoreCase))
                                isRock.LineBot.Utility.LeaveRoom(item.source.roomId, ChannelAccessToken);
                            if (string.Equals(item.source.type, "group", StringComparison.OrdinalIgnoreCase))
                                isRock.LineBot.Utility.LeaveGroup(item.source.roomId, ChannelAccessToken);

                            break;
                        }
                        sbMessage.Append("你說了:").AppendLine(ReceivedMessage.events[0].message.text);
                        // 取得用戶名稱 
                        LineUserInfo UserInfo = null;
                        if (string.Equals(item.source.type, "room", StringComparison.OrdinalIgnoreCase))
                        {
                            UserInfo = isRock.LineBot.Utility.GetRoomMemberProfile(
                               item.source.roomId, item.source.userId, this.ChannelAccessToken);
                        }

                        if (string.Equals(item.source.type, "group", StringComparison.OrdinalIgnoreCase))
                        {
                            UserInfo = isRock.LineBot.Utility.GetGroupMemberProfile(
                               item.source.groupId, item.source.userId, this.ChannelAccessToken);
                        }
                        // 顯示用戶名稱
                        if (!string.Equals(item.source.type, "user", StringComparison.OrdinalIgnoreCase))
                            sbMessage.Append("\n你是:").AppendLine(UserInfo.displayName);

                        // 回覆用戶
                        this.ReplyMessage(item.replyToken, sbMessage.ToString());
                        break;
                    default:
                        break;
                }

                // 回覆API OK
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
