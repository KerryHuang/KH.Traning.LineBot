using System;
using System.Linq;
using System.Web.Http;
using System.IO;
using isRock.LineBot;

namespace WebHook_First.Controllers
{
    /// <summary>
    /// 取得用戶傳來的圖片、照片、聲音檔
    /// </summary>
    /// <seealso cref="isRock.LineBot.LineWebHookControllerBase" />
    public class ContentProcessController : isRock.LineBot.LineWebHookControllerBase
    {
        //private const string ChannelAccessToken = "~~~請改成你的Linebot的ChannelAccessToken~~~";
        private const string AdminUserId = "~~~改成你的AdminUserId~~~";

        [Route("api/ContentProcess")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            string path = System.Web.HttpContext.Current.Request.MapPath("/tempfolder/");
            string filename, fileURL;
            byte[] filebody;

            var lineEvent = this.ReceivedMessage.events.FirstOrDefault();

            try
            {
                // 如果不是訊息
                if (lineEvent.type != "message") return Ok();

                // 用戶傳來message的type
                //string message = $"用戶傳來message的type:{lineEvent.message.type.ToLower()}";

                // 依照訊息處理
                switch (lineEvent.message.type.ToLower())
                {
                    case "text":
                        // 訊息回覆
                        this.ReplyMessage(lineEvent.replyToken, $"我只收到訊息{lineEvent.message.text}");
                        break;
                    case "image":
                        //建立唯一名稱
                        filename = Guid.NewGuid() + ".png";
                        // 取得contentid
                        filebody = Utility.GetUserUploadedContent(lineEvent.message.id, ChannelAccessToken);
                        // save
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        System.IO.File.WriteAllBytes(path + filename, filebody);
                        fileURL = $"http://{System.Web.HttpContext.Current.Request.Url.Host}/tempfolder/{filename}";
                        // 回覆訊息
                        this.ReplyMessage(lineEvent.replyToken, $"我收到一個圖檔，位於\n{fileURL}");
                        break;
                    case "audio":
                        filename = Guid.NewGuid() + ".mp3";
                        // 取得contentid
                        filebody = Utility.GetUserUploadedContent(lineEvent.message.id, ChannelAccessToken);
                        // save
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        System.IO.File.WriteAllBytes(path + filename, filebody);
                        fileURL = $"http://{System.Web.HttpContext.Current.Request.Url.Host}/tempfolder/{filename}";
                        // 回覆訊息
                        this.ReplyMessage(lineEvent.replyToken, $"我收到一個聲音檔，位於\n{fileURL}");
                        break;
                }
            }
            catch (Exception ex)
            {
                // 回覆訊息
                string message = "我錯了，錯在 " + ex.Message;

                // 回覆用戶                
                this.PushMessage(AdminUserId, message);                
            }

            return Ok();
        }
    }
}
