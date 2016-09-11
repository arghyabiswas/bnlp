using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLPToken;

namespace BNLP2008
{
    public partial class Google : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            litBengali.Text = NLPGoogle.TranslateGoogle(txtInput.Text);
        }
    }
}
