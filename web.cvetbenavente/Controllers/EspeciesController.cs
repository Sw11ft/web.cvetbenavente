using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.cvetbenavente.Data;
using web.cvetbenavente.Models;

namespace web.cvetbenavente.Controllers
{
    public class EspeciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EspeciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Especies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Especies.ToListAsync());
        }

        // GET: Especies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .SingleOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }

            return View(especie);
        }

        // GET: Especies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Active")] Especie especie)
        {
            if (ModelState.IsValid)
            {
                especie.Id = Guid.NewGuid();
                _context.Add(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(especie);
        }

        // GET: Especies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies.SingleOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }
            return View(especie);
        }

        // POST: Especies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nome,Active")] Especie especie)
        {
            if (id != especie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(especie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecieExists(especie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(especie);
        }

        // GET: Especies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .SingleOrDefaultAsync(m => m.Id == id);
            if (especie == null)
            {
                return NotFound();
            }

            return View(especie);
        }

        // POST: Especies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var especie = await _context.Especies.SingleOrDefaultAsync(m => m.Id == id);
            _context.Especies.Remove(especie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool EspecieExists(Guid id)
        {
            return _context.Especies.Any(e => e.Id == id);
        }
    }
}
