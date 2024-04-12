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
    public class ConnectionsController : Controller
    {
        private readonly VisualizzaLogContext _context;

        public ConnectionsController(VisualizzaLogContext context)
        {
            _context = context;
        }

        //// GET: Connections
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Connections.ToListAsync());
        //}

        // GET: Connections
        public async Task<IActionResult> Index(string protocol, string srcAddress, string srcPort, string dstAddress, string dstPort)
        {
            var connections = _context.Connections.AsQueryable();

            if (!string.IsNullOrEmpty(protocol))
            {
                connections = connections.Where(c => c.Protocol.Contains(protocol));
            }

            if (!string.IsNullOrEmpty(srcAddress))
            {
                connections = connections.Where(c => c.SRCAddress.Contains(srcAddress));
            }

            if (!string.IsNullOrEmpty(srcPort))
            {
                connections = connections.Where(c => c.SRCPort.Contains(srcPort));
            }

            if (!string.IsNullOrEmpty(dstAddress))
            {
                connections = connections.Where(c => c.DSTAddress.Contains(dstAddress));
            }

            if (!string.IsNullOrEmpty(dstPort))
            {
                connections = connections.Where(c => c.DSTPort.Contains(dstPort));
            }

            return View(await connections.ToListAsync());
        }


        // GET: Connections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connections = await _context.Connections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connections == null)
            {
                return NotFound();
            }

            return View(connections);
        }

        // GET: Connections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Connections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Timestamp,Flag,NatFlag,Protocol,SRCAddress,SRCPort,DSTAddress,DSTPort,TCPState")] Connection connections)
        {
            if (ModelState.IsValid)
            {
                _context.Add(connections);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(connections);
        }

        // GET: Connections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connections = await _context.Connections.FindAsync(id);
            if (connections == null)
            {
                return NotFound();
            }
            return View(connections);
        }

        // POST: Connections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Timestamp,Flag,NatFlag,Protocol,SRCAddress,SRCPort,DSTAddress,DSTPort,TCPState")] Connection connections)
        {
            if (id != connections.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(connections);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConnectionsExists(connections.Id))
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
            return View(connections);
        }

        // GET: Connections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connections = await _context.Connections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connections == null)
            {
                return NotFound();
            }

            return View(connections);
        }

        // POST: Connections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var connections = await _context.Connections.FindAsync(id);
            if (connections != null)
            {
                _context.Connections.Remove(connections);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnectionsExists(int id)
        {
            return _context.Connections.Any(e => e.Id == id);
        }
    }
}
