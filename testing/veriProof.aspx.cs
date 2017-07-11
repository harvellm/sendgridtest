using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace testing
{
    public partial class veriProof : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod()]
        public string UploadFile(string fileName, string fileData)
        {
            byte[] file = System.Convert.FromBase64String(fileData);
            System.IO.File.WriteAllBytes(@"\\ver-fileserver\Veritas\Test\Temp\verivdp testing\" + fileName, file);
            return "";
        }
    }
}