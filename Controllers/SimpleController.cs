using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace LesserToUmbraco.Controllers
{
    public class SimpleController : Controller
    {
        private readonly UmbracoContext _umbracoContext;

        public SimpleController()
        {
            _umbracoContext = UmbracoContext.Current;
        }

        /*Including a MVC Controller into the Umbraco routing!!! My CustomUmbracoApplication : UmbracoApplication wis replacing the UmbracoApplication in the Global.asax
         so my SimpleController : Controller will be included in the Umbraco routing*/
        public ActionResult Index()
        {
            //being that we are in a custom class, outside of an Umbraco controller, we have to instantiate our own Helper
            var umbracoHelper = new Umbraco.Web.UmbracoHelper(_umbracoContext);

            //On most website builds, you may only have one node as the root node
            //In these scenarios, you can use this snippet to get the homepage from the Umbraco Helper:
            var rootNodes = umbracoHelper.TypedContentAtRoot();
            var homePage = rootNodes.FirstOrDefault();

            var homeNodeById = rootNodes.First(x => x.Id == 1053);

            //If like me you're not very keen on hardcoding ID in your code, a more sage approach would be to filter by document type alias,
            //because in theory you should never have more than one homepage
            var homeNodeByAlias = rootNodes.FirstOrDefault(x => x.DocumentTypeAlias == "umbHomePage");

            //@* Get the top item in the content tree, this will always be the Last ancestor found *@
            //var websiteRoot = Model.AncestorsOrSelf().Last();


            var documentType = homePage.GetPropertyValue<string>("documentType");

            var documentTypeAlias = homePage.DocumentTypeAlias;

            ViewBag.Intro = homePage.GetPropertyValue<string>("intro");

            ViewBag.Id = homePage.GetPropertyValue("_umb_id");

            ViewBag.HomePageDocType = "test";

            //Umbraco expects a model of type RenderModel

            var model = new RenderModel(_umbracoContext.ContentCache.GetById(1053), Thread.CurrentThread.CurrentCulture);

            return View(model);
        }
    }
}