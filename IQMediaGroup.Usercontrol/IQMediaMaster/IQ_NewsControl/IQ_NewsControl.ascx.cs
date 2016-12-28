using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQ_NewsControl
{
    public partial class IQ_NewsControl : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            GetIQNews();
        }


        private void GetIQNews()
        {
            try
            {
                IIQ_NewsController _IIQ_NewsController = _ControllerFactory.CreateObject<IIQ_NewsController>();
                List<IQ_News> _ListOfIQ_News = _IIQ_NewsController.GetIQNews();

                dlnews.DataSource = _ListOfIQ_News;
                dlnews.DataBind();
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }
    }
}