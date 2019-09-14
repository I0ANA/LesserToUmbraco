using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LesserToUmbraco.Models
{
    public class LatestBlogPost
    {
        public string Title { get; set; }
        public string Introduction { get; set; }

        public LatestBlogPost(string title, string introduction)
        {
            Title = title;
            Introduction = introduction;
        }
    }
}