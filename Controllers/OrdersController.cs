using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TH4.Data;
using TH4.Models;

namespace TH4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly AppDbContext _context;

		public OrdersController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Order>> GetOrder(int id)
		{
			var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
			if (order == null) return NotFound();
			return order;
		}

		[HttpPost]
		public async Task<ActionResult<Order>> CreateOrder(Order order)
		{
			order.CreatedAt = DateTime.Now;
			order.UpdatedAt = DateTime.Now;
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOrder(int id, Order order)
		{
			if (id != order.Id) return BadRequest();
			var existingOrder = await _context.Orders.FindAsync(id);
			if (existingOrder == null) return NotFound();

			existingOrder.Status = order.Status;
			existingOrder.UpdatedAt = DateTime.Now;

			_context.Entry(existingOrder).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order == null) return NotFound();

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
