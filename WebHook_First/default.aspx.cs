using isRock.LineBot;
using System;
using System.Collections.Generic;
using WebHook_First.Services;

namespace WebHook_First
{
    public partial class _default : System.Web.UI.Page
    {
        private const string channelAccessToken = "~~~請改成你的Linebot的ChannelAccessToken~~~";
        private const string AdminUserId = "~~~改成你的AdminUserId~~~";

        protected void Page_Load(object sender, EventArgs e)
        {
            //如果從callback.aspx導回此頁，應該可以取得token
            if (!this.IsPostBack)
            {
                //如果有，則保留於text
                //(注意這不安全，應該要保留在後端，此為範例)
                this.txb_token.Value = Request.QueryString["token"];
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, $"測試 {DateTime.Now.ToString()} ! ");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, 1,2);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            // 建立一個Buttons Template Message 物件
            var ButtonsTemplateMsg = new isRock.LineBot.ButtonsTemplate();

            // 設定thumbanilImgageUrl
            ButtonsTemplateMsg.altText = "Kerry 和 Jill 終身大事"; // 無法顯示時的替代文字
            ButtonsTemplateMsg.thumbnailImageUrl = new Uri("https://kerrylinebot.azurewebsites.net/Images/user01.jpg"); // 圖片Url
            ButtonsTemplateMsg.text = "Kerry 和 Jill 終身大事";
            ButtonsTemplateMsg.title = "你會來參加嗎？"; // 標題

            //建立actions
            var actions = new List<isRock.LineBot.TemplateActionBase>
            {
                new isRock.LineBot.PostbackAction() { label = "會", data = "product=clothes&type=yes" },
                new isRock.LineBot.PostbackAction() { label = "不會", data = "product=clothes&type=no" },
                new isRock.LineBot.PostbackAction() { label = "考慮中", data = "product=clothes&type=thinking" }
            };

            // 將建立好的actions加入
            ButtonsTemplateMsg.actions = actions;

            // 建立bot instance
            isRock.LineBot.Bot bot = new Bot(channelAccessToken);

            // Send ButtonsTemplateMsg
            using (UserService userService = new UserService())
            {
                //bot.PushMessage(AdminUserId, ButtonsTemplateMsg);
                var users = userService.GetAll();
                if (users.Count >0)
                {
                    foreach(var user in users)
                    {
                        bot.PushMessage(user.UserID, ButtonsTemplateMsg);
                    }
                }
            }
        }

        protected void ButtonSend_Click(object sender, EventArgs e)
        {
            //透過LineNotSDK中的API，傳送
            var ret = isRock.LineNotify.Utility.SendNotify(this.txb_token.Value, this.txb_msg.Value);
            msg.InnerText = $"send '{ this.txb_msg.Value}'..." + ret.message;
        }
    }
}