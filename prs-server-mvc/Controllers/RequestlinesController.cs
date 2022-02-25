using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prs_server_mvc.Models;

namespace prs_server_mvc.Controllers
{
    public class RequestlinesController : Controller
    {
        private readonly PrsDbContext _context;

        public RequestlinesController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: Requestlines
        public async Task<IActionResult> Index()
        {
            var prsDbContext = _context.Requestlines.Include(r => r.Product).Include(r => r.Request);
            return View(await prsDbContext.ToListAsync());
        }

        // GET: Requestlines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestline = await _context.Requestlines
                .Include(r => r.Product)
                .Include(r => r.Request)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestline == null)
            {
                return NotFound();
            }

            return View(requestline);
        }

        // GET: Requestlines/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["RequestId"] = new SelectList(_context.Requests, "Id", "DeliveryMode");
            return View();
        }

        // POST: Requestlines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,RequestId,ProductId")] Requestline requestline)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", requestline.ProductId);
            ViewData["RequestId"] = new SelectList(_context.Requests, "Id", "DeliveryMode", requestline.RequestId);
            return View(requestline);
        }

        // GET: Requestlines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestline = await _context.Requestlines.FindAsync(id);
            if (requestline == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", requestline.ProductId);
            ViewData["RequestId"] = new SelectList(_context.Requests, "Id", "DeliveryMode", requestline.RequestId);
            return View(requestline);
        }

        // POST: Requestlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,RequestId,ProductId")] Requestline requestline)
        {
            if (id != requestline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestlineExists(requestline.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", requestline.ProductId);
            ViewData["RequestId"] = new SelectList(_context.Requests, "Id", "DeliveryMode", requestline.RequestId);
            return View(requestline);
        }

        // GET: Requestlines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestline = await _context.Requestlines
                .Include(r => r.Product)
                .Include(r => r.Request)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestline == null)
            {
                return NotFound();
            }

            return View(requestline);
        }

        // POST: Requestlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestline = await _context.Requestlines.FindAsync(id);
            _context.Requestlines.Remove(requestline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestlineExists(int id)
        {
            return _context.Requestlines.Any(e => e.Id == id);
        }
    }
}
