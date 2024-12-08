using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TH4.Data;
using TH4.Models;

namespace TH4.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderItemsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public OrderItemsController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/orderitems
		[HttpGet]
		public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
		{
			return await _context.OrderItems.ToListAsync();
		}

		// GET: api/orderitems/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
		{
			var orderItem = await _context.OrderItems.FindAsync(id);

			if (orderItem == null)
			{
				return NotFound();
			}

			return orderItem;
		}

		// POST: api/orderitems
		[HttpPost]
		public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
		{
			// Kiểm tra nếu sản phẩm có trong đơn hàng hay không (ví dụ kiểm tra tồn kho)
			// Đây là một ví dụ đơn giản, bạn có thể mở rộng thêm logic tùy thuộc vào yêu cầu
			var existingOrderItem = await _context.OrderItems
				.Where(oi => oi.OrderId == orderItem.OrderId && oi.ProductId == orderItem.ProductId)
				.FirstOrDefaultAsync();

			if (existingOrderItem != null)
			{
				return BadRequest("Sản phẩm đã có trong đơn hàng.");
			}

			_context.OrderItems.Add(orderItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetOrderItem", new { id = orderItem.Id }, orderItem);
		}

		// PUT: api/orderitems/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
		{
			if (id != orderItem.Id)
			{
				return BadRequest();
			}

			_context.Entry(orderItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderItemExists(id))
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

		// DELETE: api/orderitems/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrderItem(int id)
		{
			var orderItem = await _context.OrderItems.FindAsync(id);
			if (orderItem == null)
			{
				return NotFound();
			}

			_context.OrderItems.Remove(orderItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool OrderItemExists(int id)
		{
			return _context.OrderItems.Any(e => e.Id == id);
		}
	}
}
