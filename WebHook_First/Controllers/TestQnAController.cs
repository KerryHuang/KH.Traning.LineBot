using isRock.LineBot;
using System;
using System.Linq;
using System.Web.Http;
using WebHook_First.Services;

namespace StudyHostExampleLinebot.Controllers
{
    public class TestQnAController : isRock.LineBot.LineWebHookControllerBase
    {
        private const string channelAccessToken = "~~~請改成你的Linebot的ChannelAccessToken~~~";
        private const string AdminUserId = "~~~改成你的AdminUserId~~~";
        private const string QnAKey = "~~~改成你的QnAKey~~~";
        private const string EndPoint = "~~~改成你的EndPoint~~~"; //ex.westus
        private const string UnknowAnswer = " 不好意思，您可以換個方式問嗎? 我不太明白您的意思...";

        [Route("api/TestQnA")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                // 設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                // 取得Line Event(範例，只取第一個)
                //var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                // 取得Line Event                
                foreach (var LineItem in this.ReceivedMessage.events)
                {
                    //配合Line verify 
                    if (LineItem.replyToken == "00000000000000000000000000000000") continue;

                    //回覆訊息
                    string responseText = string.Empty;
                    switch (LineItem.type.ToLower())
                    {
                        case "join":
                            #region Join
                            responseText = "有人把我加入" + LineItem.source.type + "中了，大家好啊~";
                            // 回覆用戶
                            this.ReplyMessage(LineItem.replyToken, responseText);
                            #endregion
                            break;
                        case "message":
                            #region Message
                            switch (LineItem.message.type.ToLower())
                            {
                                case "text": //收到文字
                                    if (LineItem.message.text == "bye")
                                    {
                                        // 回覆用戶
                                        this.ReplyMessage(LineItem.replyToken, "bye-bye");
                                        // 離開
                                        if (string.Equals(LineItem.source.type, "room", StringComparison.OrdinalIgnoreCase))
                                        {
                                            isRock.LineBot.Utility.LeaveRoom(LineItem.source.roomId, ChannelAccessToken);
                                        }
                                        if (string.Equals(LineItem.source.type, "group", StringComparison.OrdinalIgnoreCase))
                                        {
                                            isRock.LineBot.Utility.LeaveGroup(LineItem.source.groupId, ChannelAccessToken);
                                        }
                                        break;
                                    }

                                    // 取得用戶名稱 
                                    LineUserInfo UserInfo = null;
                                    if (string.Equals(LineItem.source.type, "room", StringComparison.OrdinalIgnoreCase))
                                    {
                                        UserInfo = isRock.LineBot.Utility.GetRoomMemberProfile(
                                           LineItem.source.roomId, LineItem.source.userId, this.ChannelAccessToken);
                                    }
                                    if (string.Equals(LineItem.source.type, "group", StringComparison.OrdinalIgnoreCase))
                                    {
                                        UserInfo = isRock.LineBot.Utility.GetGroupMemberProfile(
                                           LineItem.source.groupId, LineItem.source.userId, this.ChannelAccessToken);
                                    }
                                    if (string.Equals(LineItem.source.type, "user", StringComparison.OrdinalIgnoreCase))
                                    {
                                        UserInfo = isRock.LineBot.Utility.GetUserInfo(
                                           LineItem.source.userId, this.ChannelAccessToken);

                                        if (UserInfo != null)
                                        {
                                            try
                                            {
                                                using (UserService userService = new UserService())
                                                {
                                                    userService.AddUser(LineItem.source.userId, UserInfo.displayName);
                                                }
                                            }
                                            catch { }
                                        }
                                    }

                                    // 顯示用戶名稱
                                    if (UserInfo != null)
                                    {
                                        responseText = UserInfo.displayName + "\n";
                                    }

                                    //建立 MsQnAMaker Client
                                    var helper = new isRock.MsQnAMaker.Client(new Uri(EndPoint), QnAKey);
                                    var QnAResponse = helper.GetResponse(LineItem.message.text.Trim());
                                    var ret = (from c in QnAResponse.answers
                                               orderby c.score descending
                                               select c
                                            ).Take(1);

                                    // 預設
                                    responseText += UnknowAnswer;
                                    if (ret.FirstOrDefault()?.score > 0)
                                    {
                                        responseText = ret.FirstOrDefault()?.answer;
                                    }

                                    //回覆
                                    this.ReplyMessage(LineItem.replyToken, responseText);
                                    break;

                                case "sticker": //收到貼圖
                                    this.ReplyMessage(LineItem.replyToken, 1, 2);
                                    break;
                                case "image":
                                    break;
                                case "video":
                                    break;
                                case "audio":
                                    break;
                                case "file":
                                    break;
                                case "location":
                                    break;
                            }
                            #endregion
                            break;
                        case "follow":
                            #region Follow
                            // 顯示用戶名稱
                            if (!string.Equals(LineItem.source.type, "user", StringComparison.OrdinalIgnoreCase))
                            {
                                LineUserInfo UserInfo = isRock.LineBot.Utility.GetUserInfo(
                                       LineItem.source.userId, ChannelAccessToken);

                                if (UserInfo != null)
                                {
                                    try
                                    {
                                        using (UserService userService = new UserService())
                                        {
                                            userService.AddUser(LineItem.source.userId, UserInfo.displayName);
                                        }
                                    }
                                    catch { }
                                }
                            }
                            #endregion
                            break;
                        case "unfollow":
                            #region Unfollow
                            #endregion
                            break;
                        case "leave":
                            #region Leave
                            #endregion
                            break;
                        case "postback":
                            // 抓取postback的data
                            var postdata = LineItem.postback.data;
                            // 剖析postdata
                            var data = System.Web.HttpUtility.ParseQueryString(postdata);
                            // 準備顯示訊息
                            var msg = "哈囉，我收到您的訊息\n"; // 收到訊息
                            foreach (var item in data.AllKeys)
                            {
                                //msg += $" Key:{item} value:{data[item]}";
                                if (item == "type")
                                {
                                    switch (data[item])
                                    {
                                        case "yes":
                                            msg += "很高興您會來我的婚禮，期待與您見面。";
                                            break;
                                        case "no":
                                            msg += "很可惜您不會來我的婚禮，但還是謝謝您的祝福。";
                                            break;
                                        case "thinking":
                                            msg += "很期待您會來我的婚禮，也謝謝您的祝福。";
                                            break;
                                    }
                                }
                            }
                            this.ReplyMessage(LineItem.replyToken, msg);
                            break;
                        case "beacon":
                            break;
                        case "accountlink":
                            break;
                    }
                }

                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}
