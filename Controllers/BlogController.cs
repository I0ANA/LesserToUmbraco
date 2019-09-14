using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Archetype.Models;
using LesserToUmbraco.Models;
using Umbraco.Core.Models;
using umbraco.uicontrols;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace LesserToUmbraco.Controllers
{
    public class BlogController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Blog/";

        // GET: Blog
        public ActionResult RenderPostList(int? numberOfBlogs)
        {
            var model = new List<BlogPreview>();
            var blogPage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias == "blog").FirstOrDefault();
            var noOfBlogs = numberOfBlogs ?? blogPage.Children.Count();

            foreach (IPublishedContent page in blogPage.Children.OrderByDescending( x=> x.UpdateDate).Take(noOfBlogs))
            {
                var imageId = page.GetPropertyValue<int>("articleImage");
                var mediaItem = Umbraco.Media(imageId);
                var imageUrl = mediaItem.Url;

                model.Add(new BlogPreview(page.Name, page.GetPropertyValue<string>("articleIntro"), imageUrl, page.Url));
            }

            return View(PARTIAL_VIEW_FOLDER + "_PostList.cshtml", model);
        }
    }
}