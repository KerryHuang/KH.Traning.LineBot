using System;

namespace WebHook_First
{
    public partial class logincallback : System.Web.UI.Page
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
            var token = isRock.LineLoginV21.Utility.GetTokenFromCode(code,
                "*********",  //TODO:請更正為你自己的 client_id            
                "*********", //TODO:請更正為你自己的 client_secret
                "http://localhost:4334/logincallback.aspx");  //TODO:請更正為你自己的 callback url

            //顯示，測試用
            //(注意這是範例，token不該用明碼傳遞，也不該出現在用戶端，你應該自行記錄在資料庫或ServerSite session中)
            Response.Write("<br/> token : " + token.access_token);

            //利用token順手取得用戶資訊
            var user = isRock.LineLoginV21.Utility.GetUserProfile(token.access_token);
            Response.Write("<br/> user: " + user.displayName);
        }
    }
}