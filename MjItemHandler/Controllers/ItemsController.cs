using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MjItemHandler.Data;
using MjItemHandler.Migrations;
using MjItemHandler.Models;

namespace MjItemHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemContext _context;

        public ItemsController(ItemContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            return await _context.Item.ToListAsync();
        }

        // GET: api/Items/MaxPrice
        [HttpGet("MaxPrice")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemNaxPrice()
        {
            var item = await _context.Item
                .ToListAsync();

            var groupedItems = item.GroupBy(i => i.Name)
                .Select(n => n.OrderByDescending(c => c.COST).First())
                .ToList();

            return groupedItems;
        }

        // GET: api/Items/Name
        [HttpGet("{Name}")]
        public async Task<ActionResult<Item>> GetItem(string Name)
        {

            var matchedItems = await _context.Item
                .Where(i => i.Name.Contains(Name))
                .ToListAsync();

            if (matchedItems == null)
            {
                return NotFound();
            }

            var matchCount = matchedItems.Count();

            var groupedItems = matchedItems.GroupBy(i => i.Name)
                .Select(n => n.OrderByDescending(c => c.COST).First())
                .FirstOrDefault();

            return groupedItems;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.ID)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.ID }, item);
            /*            return CreatedAtAction(nameof(GetItem), new { id = item.ID }, item);
            */
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ID == id);
        }
    }
}
