using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LesserToUmbraco.Models;
using Umbraco.Web;
using Umbraco.Core.Models;
using Archetype;
using Archetype.Models;

namespace LesserToUmbraco.Controllers
{
    public class HomeController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Home/";

        // GET: Home
        public ActionResult RenderFeatured()
        {
            var model = new List<FeaturedItem>();

            IPublishedContent home = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias == "home").FirstOrDefault();

            ArchetypeModel featuredItems = home.GetPropertyValue<ArchetypeModel>("featuredItems");

            foreach (ArchetypeFieldsetModel fieldset in featuredItems)
            {
                //Media Picker
                int imageId = fieldset.GetValue<int>("image");
                var mediaItem = Umbraco.Media(imageId);
                string imageUrl = mediaItem.Url;

                //Content Picker
                int pageId = fieldset.GetValue<int>("page");
                IPublishedContent linkedToPage = Umbraco.TypedContent(pageId);
                string linkUrl = linkedToPage.Url;

                //Textstring
                var name = fieldset.GetValue<string>("name");

                model.Add(new FeaturedItem(name, fieldset.GetValue("category"), imageUrl, linkUrl));
            }

            
            return PartialView(PARTIAL_VIEW_FOLDER + "_Featured.cshtml", model);
        }

        public ActionResult RenderServices()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "_Services.cshtml");
        }

        public ActionResult RenderBlog()
        {
            var homePage = CurrentPage.AncestorOrSelf("home");

            var title = homePage.GetPropertyValue<string>("latestBlogPostsTitle");
            var introduction = homePage.GetPropertyValue("latestBlogPostsIntroduction").ToString();

            var latestBlogModel = new LatestBlogPost(title, introduction);

            return PartialView(PARTIAL_VIEW_FOLDER + "_Blog.cshtml", latestBlogModel);
        }

        public ActionResult RenderClients()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "_Clients.cshtml");
        }
    }
}