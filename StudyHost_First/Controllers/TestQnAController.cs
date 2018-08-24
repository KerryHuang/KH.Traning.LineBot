using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudyHostExampleLinebot.Controllers
{
    public class TestQnAController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "D1ROMH7Hu++foipm1KaRfwYUlIU+mGbJUQzeiPhzMGj33btrcs6PkdY+NkD6gKvutuFeCeKwImKNvfMb3p05cktdhHnZb7UPxenXKolw1OiIfqJj3SvWCRbnm7RDCL4ArYm8EaGIX6aLW13ol5aPuAdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U3751a1c67f36199a3fefe8760cd306d3";
        const string QnAKey = "1db7560e-58e6-48f1-9923-c4b61aee1136";
        const string EndPoint = "https://kerryqnamaker.azurewebsites.net/qnamaker/knowledgebases/e04c6b7d-7b60-4eaa-83d6-82d7086ccfc1/generateAnswer"; //ex.westus
        const string UnknowAnswer = "不好意思，您可以換個方式問嗎? 我不太明白您的意思...";

        [Route("api/TestQnA")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    //var repmsg = "";
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        //建立 MsQnAMaker Client
                        var helper = new isRock.MsQnAMaker.Client(
                            new Uri(EndPoint), QnAKey);
                        var QnAResponse = helper.GetResponse(LineEvent.message.text.Trim());
                        var ret = (from c in QnAResponse.answers
                                   orderby c.score descending
                                   select c
                                ).Take(1);

                        var responseText = UnknowAnswer;
                        if (ret.FirstOrDefault().score > 0)
                            responseText = ret.FirstOrDefault().answer;
                        //回覆
                        this.ReplyMessage(LineEvent.replyToken, responseText);
                    }
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
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
