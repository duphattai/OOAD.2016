using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarManager.Areas.Admin.Controllers
{
    public class ChannelController : BaseController
    {
        // GET: Admin/Channel
        public ActionResult Index()
        {
            return View();
        }
    }
}