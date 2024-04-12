using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VisualizzaLog.Data;
using VisualizzaLog.Models;

namespace VisualizzaLog.Controllers
{
    [Authorize]
    public class ArplogsController : Controller
    {
        private readonly VisualizzaLogContext _context;

        public ArplogsController(VisualizzaLogContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string address, string macAddress)
        {
            var arplogs = from a in _context.Arplogs
                          select a;

            if (!string.IsNullOrEmpty(address))
            {
                arplogs = arplogs.Where(a => a.Address.Contains(address));
            }

            if (!string.IsNullOrEmpty(macAddress))
            {
                arplogs = arplogs.Where(a => a.MacAddress.Contains(macAddress));
            }

            return View(await arplogs.ToListAsync());
        }


        // GET: Arplogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arplog = await _context.Arplogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arplog == null)
            {
                return NotFound();
            }

            return View(arplog);
        }

        // GET: Arplogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Arplogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Timestamp,Flag,Address,MacAddress,Interface")] Arplog arplog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arplog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(arplog);
        }

        // GET: Arplogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arplog = await _context.Arplogs.FindAsync(id);
            if (arplog == null)
            {
                return NotFound();
            }
            return View(arplog);
        }

        // POST: Arplogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Timestamp,Flag,Address,MacAddress,Interface")] Arplog arplog)
        {
            if (id != arplog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arplog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArplogExists(arplog.Id))
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
            return View(arplog);
        }

        // GET: Arplogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arplog = await _context.Arplogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arplog == null)
            {
                return NotFound();
            }

            return View(arplog);
        }

        // POST: Arplogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var arplog = await _context.Arplogs.FindAsync(id);
            if (arplog != null)
            {
                _context.Arplogs.Remove(arplog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArplogExists(int id)
        {
            return _context.Arplogs.Any(e => e.Id == id);
        }
    }
}
