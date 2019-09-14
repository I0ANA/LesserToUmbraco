using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web;

namespace LesserToUmbraco.Custom
{
    /*Including a MVC Controller into the Umbraco routing!!!  My CustomUmbracoApplication : UmbracoApplication wis replacing the UmbracoApplication in the Global.asax
     so my SimpleController : Controller will be included in the Umbraco routing*/
    public class CustomUmbracoApplication : UmbracoApplication
    {

        protected override IBootManager GetBootManager()
        {
            return new CustomWebBootManager(this);
        }

        class CustomWebBootManager : WebBootManager 
        {
            public CustomWebBootManager(UmbracoApplicationBase application) : base(application)
            {

            }

            public override IBootManager Complete(Action<ApplicationContext> afterComplete)
            { 
                RouteTable.Routes.MapRoute(
                    "TestSimpleControllerUmbracoRouting",
                    "simple/index",
                    new {controller = "Simple", action = "Index"}
                );

                return base.Complete(afterComplete);
            }
        }
    }
}