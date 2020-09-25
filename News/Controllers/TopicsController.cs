using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Models;
using News.Services;

namespace News.Controllers
{
    public class TopicsController : Controller
    {
        private readonly NewsContext _context;
        private readonly IArticleService _articleService;

        public TopicsController(NewsContext context, IArticleService articleService)
        {
            _context = context;
            _articleService = articleService;
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
            var topics = await _context
                .Topica
                .Include(topic => topic.SubTopics)
                .ThenInclude(subTopic => subTopic.Articles)
                .ToListAsync();
            ViewData["Articles"] = topics.SelectMany(topic => topic.SubTopics).SelectMany(subTopic => subTopic.Articles);
            return View(topics);
        }

        //public async Task<IActionResult> GetArticles(Guid? id)
        //{
        //    var topics = await _context
        //        .Topica
        //        .Where(topic => topic.Id == id)
        //        .Include(topic => topic.SubTopics)
        //        .ThenInclude(subTopic => subTopic.Articles)
        //        .ToListAsync();
        //    ViewData["Articles"] = topics.SelectMany(topic => topic.SubTopics).SelectMany(subTopic => subTopic.Articles);
        //    return View("Index", topics);
        //}
        private const int ElementsOnPage = 20;
        public async Task<IActionResult> GetArticles(Guid? id, Guid? subtopicId, string fieldName, int pageNumber)
        {
            var topics = await _context
                .Topica
                .Where(topicc => topicc.Id == id)
                .Include(topicc => topicc.SubTopics)
                .ThenInclude(subTopic => subTopic.Articles)
                .ToListAsync();
            ViewData["Articles"] = await _articleService.SortArticles(
                _context.Articles
                .Where(article => article.SubTopic.Topic.Id == id && subtopicId == null ? true : article.SubTopic.Id == subtopicId),
                new Sort() { FieldName = fieldName, IsDescending = false, PageNumber = pageNumber })
                .ToListAsync();
            ViewBag.TopicId = id;
            ViewBag.SubTopicId = subtopicId;
            ViewBag.FieldName = fieldName;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageCount = topics
                .SelectMany(topic => topic.SubTopics)
                .Where(subTopic => subtopicId == null ? true : subTopic.Id == subtopicId)
                .SelectMany(subTopic => subTopic.Articles).Count() / ElementsOnPage + 1;
            return View("Index", topics);
        }
        //GET: Topics/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topica
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.Id = Guid.NewGuid();
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topica.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Value")] Topic topic)
        {
            if (id != topic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topica
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var topic = await _context.Topica.FindAsync(id);
            _context.Topica.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(Guid id)
        {
            return _context.Topica.Any(e => e.Id == id);
        }
    }
}
