using Microsoft.AspNetCore.Mvc;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Views.Shared.Components.EditingTools
{
    public class EditingToolsViewComponent : ViewComponent
    {
        public EditingToolsViewComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync(Article article)
        {
            return View(await Task.Run(() => GetArticle(article)));
            //return View(await Task.Run(() => GetLeftTopicNavbarModel(topics, subTopicId)));
        }
        private Article GetArticle(Article article)
        {
            return article;
        }
        //private LeftTopicNavbarModel GetLeftTopicNavbarModel(List<Topic> topics, Guid? subTopicId)
        //{
        //    return new LeftTopicNavbarModel() { Topics = topics, SubTopicId = subTopicId };
        //}
    }
}
