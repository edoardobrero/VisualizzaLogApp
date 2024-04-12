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
using VisualizzaLog.Services;

namespace VisualizzaLog.Controllers
{
    [Authorize]
    public class ArplogViolationsController : Controller
    {
        private readonly VisualizzaLogContext _context;

        public ArplogViolationsController(VisualizzaLogContext context)
        {
            _context = context;
        }

        // GET: ArplogViolations
        public async Task<IActionResult> Index()
        {
            var visualizzaLogContext = _context.ArplogViolation.Include(a => a.Arplog).Include(a => a.Rule);
            return View(await visualizzaLogContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> GetViolations()
        {
            CheckViolations check = new CheckViolations();
            var rules = await _context.Rules.ToListAsync();
            var arplogs = await _context.Arplogs.ToListAsync();

            foreach (var rule in rules)
            {
                if(rule.Tipo.Equals("IP sorgente") || rule.Tipo.Equals("Indirizzo Mac"))
                {
                    foreach (var arplog in arplogs)
                    {
                        if (check.CheckArplogRuleViolation(arplog, rule))
                        {
                            if (!_context.ArplogViolation.Any(v => v.ArplogId == arplog.Id && v.RuleId == rule.Id))
                            {
                                _context.ArplogViolation.Add(new ArplogViolation { ArplogId = arplog.Id, RuleId = rule.Id });
                            }
                        }
                    }
                }
                
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Clear()
        {
            _context.ArplogViolation.RemoveRange(_context.ArplogViolation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ArplogViolations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arplogViolation = await _context.ArplogViolation
                .Include(a => a.Arplog)
                .Include(a => a.Rule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arplogViolation == null)
            {
                return NotFound();
            }

            return View(arplogViolation);
        }

        // GET: ArplogViolations/Create
        public IActionResult Create()
        {
            ViewData["ArplogId"] = new SelectList(_context.Arplogs, "Id", "Address");
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto");
            return View();
        }

        // POST: ArplogViolations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArplogId,RuleId")] ArplogViolation arplogViolation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arplogViolation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArplogId"] = new SelectList(_context.Arplogs, "Id", "Address", arplogViolation.ArplogId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto", arplogViolation.RuleId);
            return View(arplogViolation);
        }

        // GET: ArplogViolations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arplogViolation = await _context.ArplogViolation.FindAsync(id);
            if (arplogViolation == null)
            {
                return NotFound();
            }
            ViewData["ArplogId"] = new SelectList(_context.Arplogs, "Id", "Address", arplogViolation.ArplogId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto", arplogViolation.RuleId);
            return View(arplogViolation);
        }

        // POST: ArplogViolations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArplogId,RuleId")] ArplogViolation arplogViolation)
        {
            if (id != arplogViolation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arplogViolation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArplogViolationExists(arplogViolation.Id))
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
            ViewData["ArplogId"] = new SelectList(_context.Arplogs, "Id", "Address", arplogViolation.ArplogId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto", arplogViolation.RuleId);
            return View(arplogViolation);
        }

        // GET: ArplogViolations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arplogViolation = await _context.ArplogViolation
                .Include(a => a.Arplog)
                .Include(a => a.Rule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arplogViolation == null)
            {
                return NotFound();
            }

            return View(arplogViolation);
        }

        // POST: ArplogViolations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var arplogViolation = await _context.ArplogViolation.FindAsync(id);
            if (arplogViolation != null)
            {
                _context.ArplogViolation.Remove(arplogViolation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArplogViolationExists(int id)
        {
            return _context.ArplogViolation.Any(e => e.Id == id);
        }
    }
}
