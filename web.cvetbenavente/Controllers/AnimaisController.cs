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
    public class AnimaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Animais
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Animais.Include(a => a.Cliente).Include(a => a.Especie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Animais/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animais
                .Include(a => a.Cliente)
                .Include(a => a.Especie)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animais/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "Id", "Nome");
            return View();
        }

        // POST: Animais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCliente,Nome,Genero,IdEspecie")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                animal.Id = Guid.NewGuid();
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nome", animal.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "Id", "Nome", animal.IdEspecie);
            return View(animal);
        }

        // GET: Animais/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animais.SingleOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nome", animal.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "Id", "Nome", animal.IdEspecie);
            return View(animal);
        }

        // POST: Animais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,IdCliente,Nome,Genero,IdEspecie")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nome", animal.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "Id", "Nome", animal.IdEspecie);
            return View(animal);
        }

        // GET: Animais/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animais
                .Include(a => a.Cliente)
                .Include(a => a.Especie)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var animal = await _context.Animais.SingleOrDefaultAsync(m => m.Id == id);
            _context.Animais.Remove(animal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AnimalExists(Guid id)
        {
            return _context.Animais.Any(e => e.Id == id);
        }
    }
}
