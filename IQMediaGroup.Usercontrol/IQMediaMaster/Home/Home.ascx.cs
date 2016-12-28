using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.Home
{
    public partial class Home : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetBanner();
        }


        private void SetBanner()
        {
            try
            {

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/images/home/Banner"), "*.png");

                if (filePaths.Count() > 0)
                {
                    Random randomNumber = new Random();
                    Int32 finalNumber = randomNumber.Next(1, filePaths.Count() + 1);

                    StreamReader text = new StreamReader(Server.MapPath("~/images/home/Text/" + finalNumber + ".txt"));
                    imgBanner.Src = "~/images/home/Banner/" + finalNumber + ".png";
                    imgSubBanner.Src = "~/images/home/SubBanner/" + finalNumber + ".png";
                    pText.InnerText = text.ReadToEnd();
                    text.Close();
                }


                //Random randomNumber = new Random();

                //int randomNumber = Math.Round(new Random());
            }
            catch (Exception)
            {
                try
                {
                    StreamReader text = new StreamReader(Server.MapPath("~/images/home/Text/1.txt"));
                    imgBanner.Src = "~/images/home/Banner/1.png";
                    imgSubBanner.Src = "~/images/home/SubBanner/1.png";
                    pText.InnerText = text.ReadToEnd();
                    text.Close();
                }
                catch (Exception)
                {


                }

            }
        }
    }
}