using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LesserToUmbraco.Controllers
{
    public class TestCodeSnippetszzzController : SurfaceController
    {
        // GET: TestCodeSnippets
        public ActionResult Index()
        {
            //return View("TestCodeSnippets");
            return Content("Yes it is the controller");
        }
    }
}