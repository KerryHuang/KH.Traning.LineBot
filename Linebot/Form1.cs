using System;
using System.Windows.Forms;

namespace Linebot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const string ChannelAccessToken = "~~~請改成你的Linebot的ChannelAccessToken~~~";
            const string AdminUserId = "~~~改成你的AdminUserId~~~";

            //建立Bot instance
            isRock.LineBot.Bot bot =
                new isRock.LineBot.Bot(ChannelAccessToken);  //傳入Channel access token
            //發送 CarouselTemplate
            bot.PushMessage(AdminUserId, "測試訊息");
        }
    }
}
