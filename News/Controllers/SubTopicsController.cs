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
    public class SubTopicsController : Controller
    {
        private readonly NewsContext _context;
        private readonly IArticleService _articleService;

        public SubTopicsController(NewsContext context, IArticleService articleService)
        {
            _context = context;
            _articleService = articleService;
        }

        //public async Task<IActionResult> GetArticles(Guid? id)
        //{
        //    var subTopics = await _context
        //        .SubTopics
        //        .Where(subTopic => subTopic.Id == id)
        //        .Include(subTopic => subTopic.Articles)
        //        .ToListAsync();
        //    ViewData["Articles"] = subTopics.SelectMany(subTopic => subTopic.Articles);
        //    ViewData["SubTopic"] = id;
        //    return View("Index", subTopics);
        //}
        private const int ElementsOnPage = 20;
        public async Task<IActionResult> GetArticles(Guid? id, string fieldName, int pageNumber)
        {
            var subTopics = await _context
                .SubTopics
                .Where(subTopic => subTopic.Id == id)
                .Include(subTopic => subTopic.Articles)
                .ToListAsync();
            //var art = await _articleService.SortArticles(_context.Articles.Where(article => article.SubTopic.Id == id), 
            //    new Sort() { FieldName = fieldName, IsDescending= false, PageNumber=pageNumber}).ToListAsync();
            ViewData["Articles"] = await _articleService.SortArticles(_context.Articles.Where(article => article.SubTopic.Id == id),
                new Sort() { FieldName = fieldName, IsDescending = false, PageNumber = pageNumber }).ToListAsync();
            ViewBag.SubTopicId = id;
            ViewBag.FieldName = fieldName;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageCount = subTopics.SelectMany(subTopic => subTopic.Articles).Count() / ElementsOnPage + 1;
            return View("Index", subTopics);
        }
        // GET: SubTopics
        public async Task<IActionResult> Index()
        {
            var newsContext = _context.SubTopics.Include(s => s.Topic);
            return View(await newsContext.ToListAsync());
        }

        // GET: SubTopics/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTopic = await _context.SubTopics
                .Include(s => s.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subTopic == null)
            {
                return NotFound();
            }

            return View(subTopic);
        }

        // GET: SubTopics/Create
        public IActionResult Create()
        {
            ViewData["TopicId"] = new SelectList(_context.Topica, "Id", "Id");
            return View();
        }

        // POST: SubTopics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value,TopicId")] SubTopic subTopic)
        {
            if (ModelState.IsValid)
            {
                subTopic.Id = Guid.NewGuid();
                _context.Add(subTopic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TopicId"] = new SelectList(_context.Topica, "Id", "Id", subTopic.TopicId);
            return View(subTopic);
        }

        // GET: SubTopics/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTopic = await _context.SubTopics.FindAsync(id);
            if (subTopic == null)
            {
                return NotFound();
            }
            ViewData["TopicId"] = new SelectList(_context.Topica, "Id", "Id", subTopic.TopicId);
            return View(subTopic);
        }

        // POST: SubTopics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Value,TopicId")] SubTopic subTopic)
        {
            if (id != subTopic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subTopic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubTopicExists(subTopic.Id))
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
            ViewData["TopicId"] = new SelectList(_context.Topica, "Id", "Id", subTopic.TopicId);
            return View(subTopic);
        }

        // GET: SubTopics/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subTopic = await _context.SubTopics
                .Include(s => s.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subTopic == null)
            {
                return NotFound();
            }

            return View(subTopic);
        }

        // POST: SubTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subTopic = await _context.SubTopics.FindAsync(id);
            _context.SubTopics.Remove(subTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubTopicExists(Guid id)
        {
            return _context.SubTopics.Any(e => e.Id == id);
        }
    }
}
