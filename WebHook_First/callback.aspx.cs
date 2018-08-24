using isRock.LineNotify;
using System;

namespace WebHook_First
{
    public partial class callback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           //取得返回的code
            var code = Request.QueryString["code"];
            if (code == null)
            {
                Response.Write("沒有正確回應code");
                Response.End();
            }
            //顯示，測試用
            Response.Write("<br/> code : " + code);
            //從Code取回toke
            //var token = isRock.LineNotify.Utility.GetTokenFromCode(code,
            //    "XU9s7p4T7zGIXqH5ZP9KVa",  //TODO:請更正為你自己的 client_id
            //    "qMqRSkthkqr276ULMocERysjAQIn2OR5DoaBSiMAn2t", //TODO:請更正為你自己的 client_secret
            //    "http://localhost:4334/callback.aspx");

            var token = isRock.LineLoginV21.Utility.GetTokenFromCode(code,
                "**********",  //TODO:請更正為你自己的 client_id
                "**********", //TODO:請更正為你自己的 client_secret
                "http://localhost:4334/callback.aspx");  //TODO:請更正為你自己的 callback url

            //顯示，測試用
            Response.Write("<br/> token : " + token.access_token);

            //利用token順手取得用戶資訊
            var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);

            //利用token發各測試訊息
            Utility.SendNotify(token.access_token, "msg test - " + user.displayName + System.DateTime.Now.ToString());
            //導入首頁，帶入token
            //(注意這是範例，token不該用明碼傳遞，也不該出現在用戶端，你應該自行記錄在資料庫或ServerSite session中)
            Response.Redirect("default.aspx?token=" + token.access_token);
        }
    }
}