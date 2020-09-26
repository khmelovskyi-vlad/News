using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Models;
using News.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace News.Views.Topics.Components.LeftTopicNavbar
{
    public class LeftTopicNavbarViewComponent : ViewComponent
    {
        private readonly NewsContext newsContext;

        public LeftTopicNavbarViewComponent(NewsContext newsContext)
        {
            this.newsContext = newsContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(List<Topic> topics, Guid? subTopicId)
        {
            return View(await Task.Run(() => GetLeftTopicNavbarModel(topics, subTopicId)));
        }
        private LeftTopicNavbarModel GetLeftTopicNavbarModel(List<Topic> topics, Guid? subTopicId)
        {
            return new LeftTopicNavbarModel() { Topics = topics, SubTopicId = subTopicId};
        }
    }
}
