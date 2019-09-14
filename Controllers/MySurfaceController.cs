using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LesserToUmbraco.Models;
using Umbraco.Web.Mvc;

namespace LesserToUmbraco.Controllers
{
    public class MySurfaceController : SurfaceController
    {
        // GET: SimpleSurface
        public ActionResult Index(ContactViewModel model)
        {
            return RedirectToCurrentUmbracoPage();
        }
    }
}