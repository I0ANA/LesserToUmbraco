using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using LesserToUmbraco.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace LesserToUmbraco.Controllers
{
    public class SiteLayoutController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/SiteLayout/";

        /// <summary>
        /// Renders the top navigation partial
        /// </summary>
        /// <returns>Partial view with a model</returns>
        public ActionResult RenderHeader()
        {
            //return PartialView(PARTIAL_VIEW_FOLDER + "_Header.cshtml");

            //List<NavigationListItem> nav = GetNavigationModelFromDatabase();
            List<NavigationListItem> nav = GetObjectFromCache<List<NavigationListItem>>("mainNav", 5, GetNavigationModelFromDatabase);

            return PartialView(PARTIAL_VIEW_FOLDER + "_Header.cshtml", nav);
        }

        public ActionResult RenderIntro()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "_Intro.cshtml");
        }
        public ActionResult RenderTitleSection()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "_Title.cshtml");
        }

        public ActionResult RenderFooter()
        {
            return PartialView(PARTIAL_VIEW_FOLDER + "_Footer.cshtml");
        }


        /// <summary>
        /// Finds the home page and gets the navigation structure based on it and it's children
        /// </summary>
        /// <returns>A List of NavigationListItems, representing the structure of the site.</returns>
        private List<NavigationListItem> GetNavigationModelFromDatabase()
        {

            IPublishedContent homePage;
            //const int HOME_PAGE_POSITION_IN_PATH = 1;
            //int homePageId = int.Parse(CurrentPage.Path.Split(',')[HOME_PAGE_POSITION_IN_PATH]);
            //IPublishedContent homePage = Umbraco.Content(homePageId);
            try
            {
                homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias == "home").FirstOrDefault();
            }
            catch (Exception e)
            {
                var umbracoContext = UmbracoContext.Current;
                var umbracoHelper = new UmbracoHelper(umbracoContext);
                homePage = umbracoHelper.TypedContentAtRoot().FirstOrDefault();
            }
            var nav = new List<NavigationListItem>();
            nav.Add(new NavigationListItem(new NavigationLink(homePage.Url, homePage.Name)));
            nav.AddRange(GetChildNavigationList(homePage));
            return nav;
        }

        /// <summary>
        /// Loops through the child pages of a given page and their children to get the structure of the site.
        /// </summary>
        /// <param name="page">The parent page which you want the child structure for</param>
        /// <returns>A List of NavigationListItems, representing the structure of the pages below a page.</returns>
        private List<NavigationListItem> GetChildNavigationList(IPublishedContent page)
        {
            List<NavigationListItem> listItems = null;
            var childPages = page.Children.Where("Visible").Where(x => x.Level <= 2)
                .Where( x=> !x.HasValue("excludeFromTopNavigation") 
                            || x.HasValue("excludeFromTopNavigation") && !x.GetPropertyValue<bool>("excludeFromTopNavigation"));
            if (childPages != null && childPages.Any() && childPages.Count() > 0)
            {
                listItems = new List<NavigationListItem>();
                foreach (var childPage in childPages)
                {
                    var listItem = new NavigationListItem(new NavigationLink(childPage.Url, childPage.Name));
                    listItem.Items = GetChildNavigationList(childPage);
                    listItems.Add(listItem);
                }
            }
            return listItems;
        }

        /// <summary>
        /// A generic function for getting and setting objects to the memory cache.
        /// </summary>
        /// <typeparam name="T">The type of the object to be returned.</typeparam>
        /// <param name="cacheItemName">The name to be used when storing this object in the cache.</param>
        /// <param name="cacheTimeInMinutes">How long to cache this object for.</param>
        /// <param name="objectSettingFunction">A parameterless function to call if the object isn't in the cache and you need to set it.</param>
        /// <returns>An object of the type you asked for</returns>
        private static T GetObjectFromCache<T>(string cacheItemName, int cacheTimeInMinutes, Func<T> objectSettingFunction)
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedObject = (T)cache[cacheItemName];
            if (cachedObject == null)
            {
                var policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes)
                };
                cachedObject = objectSettingFunction();
                cache.Set(cacheItemName, cachedObject, policy);
            }
            return cachedObject;
        }

    }
}