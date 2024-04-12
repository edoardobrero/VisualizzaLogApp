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
    public class ViolationsController : Controller
    {
        private readonly VisualizzaLogContext _context;

        public ViolationsController(VisualizzaLogContext context)
        {
            _context = context;
        }

        // GET: Violations
        public async Task<IActionResult> Index()
        {
            var visualizzaLogContext = _context.Violations.Include(v => v.Connection).Include(v => v.Rule);
            return View(await visualizzaLogContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> GetViolations()
        {
            CheckViolations check = new CheckViolations();
            var rules = await _context.Rules.ToListAsync();
            var connections = await _context.Connections.ToListAsync();

            foreach (var rule in rules)
            {
                if(!rule.Tipo.Equals("Indirizzo Mac"))
                {
                    foreach (var connection in connections)
                    {
                        if (check.CheckRuleViolation(connection, rule))
                        {
                            if (!_context.Violations.Any(v => v.ConnectionId == connection.Id && v.RuleId == rule.Id))
                            {
                                _context.Violations.Add(new Violation { ConnectionId = connection.Id, RuleId = rule.Id });
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
            _context.Violations.RemoveRange(_context.Violations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Violations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var violation = await _context.Violations
                .Include(v => v.Connection)
                .Include(v => v.Rule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (violation == null)
            {
                return NotFound();
            }

            return View(violation);
        }

        // GET: Violations/Create
        public IActionResult Create()
        {
            ViewData["ConnectionId"] = new SelectList(_context.Connections, "Id", "DSTAddress");
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto");
            return View();
        }

        // POST: Violations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ConnectionId,RuleId")] Violation violation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(violation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConnectionId"] = new SelectList(_context.Connections, "Id", "DSTAddress", violation.ConnectionId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto", violation.RuleId);
            return View(violation);
        }

        // GET: Violations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var violation = await _context.Violations.FindAsync(id);
            if (violation == null)
            {
                return NotFound();
            }
            ViewData["ConnectionId"] = new SelectList(_context.Connections, "Id", "DSTAddress", violation.ConnectionId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto", violation.RuleId);
            return View(violation);
        }

        // POST: Violations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ConnectionId,RuleId")] Violation violation)
        {
            if (id != violation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(violation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViolationExists(violation.Id))
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
            ViewData["ConnectionId"] = new SelectList(_context.Connections, "Id", "DSTAddress", violation.ConnectionId);
            ViewData["RuleId"] = new SelectList(_context.Rules, "Id", "Contenuto", violation.RuleId);
            return View(violation);
        }

        // GET: Violations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var violation = await _context.Violations
                .Include(v => v.Connection)
                .Include(v => v.Rule)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (violation == null)
            {
                return NotFound();
            }

            return View(violation);
        }

        // POST: Violations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var violation = await _context.Violations.FindAsync(id);
            if (violation != null)
            {
                _context.Violations.Remove(violation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViolationExists(int id)
        {
            return _context.Violations.Any(e => e.Id == id);
        }
    }
}
