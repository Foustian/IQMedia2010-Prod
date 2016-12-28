using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using System.Text;

namespace IQMedia.WebApplication.Controllers
{
    public class PQArticleController : Controller
    {
        //
        // GET: /PQArticle/

        public ActionResult Index(string ID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ID))
                {
                    throw new Exception("Blank ID.");
                }                
                
                var pqLgc = (PQLogic)(IQMedia.Web.Logic.Base.LogicFactory.GetLogic(Web.Logic.Base.LogicType.PQ));
                var pqArticle = pqLgc.GetPQArticleByAgentResultID(ID);                

                ViewBag.IsSuccess = true;

                return View(pqArticle);
            }
            catch (Exception ex)
            {
                UtilityLogic.WriteException(ex);
                ViewBag.IsSuccess = false;
            }

            return View();
        }

    }
}
