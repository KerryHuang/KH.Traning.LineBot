using isRock.LineBot.Conversation;
using System;
using System.Web.Http;
using WebHook_First.Models.Entity;

namespace WebHook_First.Controllers
{
    public class CICTestController : ApiController
    {
        private const string ChannelAccessToken = "~~~請改成你的Linebot的ChannelAccessToken~~~";
        private const string AdminUserId = "~~~改成你的AdminUserId~~~";

        [HttpPost]
        public IHttpActionResult POST()
        {
            var responseMsg = "";

            try
            {
                //定義資訊蒐集者
                isRock.LineBot.Conversation.InformationCollector<LeaveRequest> CIC =
                    new isRock.LineBot.Conversation.InformationCollector<LeaveRequest>(ChannelAccessToken);
                CIC.OnMessageTypeCheck += (s, e) => {
                    switch (e.CurrentPropertyName)
                    {
                        case "代理人":
                            if (e.ReceievedMessage != "kant")
                            {
                                e.isMismatch = true;
                                e.ResponseMessage = "你的代理人只有kant，代理人請找kant...";
                            }
                            break;
                        case "假別":
                            if (e.ReceievedMessage != "事假" && e.ReceievedMessage != "病假" && e.ReceievedMessage != "公假")
                            {
                                e.isMismatch = true;
                                e.ResponseMessage = "你只能輸入事假,病假,事假其中之一";
                            }
                            break;
                        default:
                            break;
                    }

                };

                //取得 http Post RawData(should be JSO
                string postData = Request.Content.ReadAsStringAsync().Result;
                //剖析JSON
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                foreach (var LineItem in ReceivedMessage.events)
                {
                    //定義接收CIC結果的類別
                    ProcessResult<LeaveRequest> result;
                    if (LineItem.message.text == "我要請假")
                    {
                        //把訊息丟給CIC 
                        result = CIC.Process(LineItem, true);
                        responseMsg = "開始請假程序\n";
                    }
                    else
                    {
                        //把訊息丟給CIC 
                        result = CIC.Process(LineItem);
                    }

                    if (LineItem.message.text == "bye")
                    {
                        // 回覆用戶                        
                        isRock.LineBot.Utility.ReplyMessage(LineItem.replyToken, "bye-bye", ChannelAccessToken);
                        // 離開
                        if (string.Equals(LineItem.source.type, "room", StringComparison.OrdinalIgnoreCase))
                        {
                            isRock.LineBot.Utility.LeaveRoom(LineItem.source.roomId, ChannelAccessToken);
                        }
                        if (string.Equals(LineItem.source.type, "group", StringComparison.OrdinalIgnoreCase))
                        {
                            isRock.LineBot.Utility.LeaveGroup(LineItem.source.groupId, ChannelAccessToken);
                        }
                        //回覆API OK
                        return Ok();
                    }

                    //處理 CIC回覆的結果
                    switch (result.ProcessResultStatus)
                    {
                        case ProcessResultStatus.Processed:
                            if (result.ResponseButtonsTemplateCandidate != null)
                            {
                                //如果有template Message，直接回覆，否則放到後面一起回覆
                                isRock.LineBot.Utility.ReplyTemplateMessage(
                                    LineItem.replyToken,
                                    result.ResponseButtonsTemplateCandidate,
                                    ChannelAccessToken);
                                return Ok();
                            }
                            //取得候選訊息發送
                            responseMsg += result.ResponseMessageCandidate;
                            break;
                        case ProcessResultStatus.Done:
                            responseMsg += result.ResponseMessageCandidate;
                            responseMsg += $"蒐集到的資料有...\n";
                            responseMsg += Newtonsoft.Json.JsonConvert.SerializeObject(result.ConversationState.ConversationEntity);
                            break;
                        case ProcessResultStatus.Pass:
                            //responseMsg = $"你說的 '{LineItem.message.text}' 我看不懂，如果想要請假，請跟我說 : 『我要請假』";
                            break;
                        case ProcessResultStatus.Exception:
                            //取得候選訊息發送
                            responseMsg += result.ResponseMessageCandidate;
                            break;
                        case ProcessResultStatus.Break:
                            //取得候選訊息發送
                            responseMsg += result.ResponseMessageCandidate;
                            break;
                        case ProcessResultStatus.InputDataFitError:
                            responseMsg += "\n資料型態不合\n";
                            responseMsg += result.ResponseMessageCandidate;
                            break;
                        default:
                            //取得候選訊息發送
                            responseMsg += result.ResponseMessageCandidate;
                            break;
                    }
                    //回覆用戶訊息
                    if (!string.IsNullOrWhiteSpace(responseMsg))
                    {
                        isRock.LineBot.Utility.ReplyMessage(LineItem.replyToken, responseMsg, ChannelAccessToken);
                    }
                }

                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果你要偵錯的話
                isRock.LineBot.Utility.PushMessage(AdminUserId, ex.Message, ChannelAccessToken);
                return Ok();
                //throw ex;
            }
        }
    }
}
