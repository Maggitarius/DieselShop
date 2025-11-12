using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apidiesel.Models;

namespace apidiesel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly dieselContext _context;

        public CartsController(dieselContext context)
        {
            _context = context;
        }

        // 📦 Получить корзину конкретного покупателя
        [HttpGet("{customerId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCartByCustomer(int customerId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.CustomerId == customerId)
                .Join(_context.Products,
                    cart => cart.ProductId,
                    product => product.ProductId,
                    (cart, product) => new
                    {
                        cart.CartId,
                        cart.ProductId,
                        product.ProductName,
                        product.SalePrice,
                        cart.Quantity,
                        product.ImageUrl,
                        cart.AddedDate
                    })
                .ToListAsync();

            return Ok(cartItems);
        }

        // ➕ Добавить товар в корзину
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] Cart cart)
        {
            // Проверим, есть ли уже этот товар у пользователя
            var existingItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.CustomerId == cart.CustomerId && c.ProductId == cart.ProductId);

            if (existingItem != null)
            {
                // Если есть — просто обновляем количество
                existingItem.Quantity += cart.Quantity;
                existingItem.AddedDate = DateTime.Now;
            }
            else
            {
                cart.AddedDate = DateTime.Now;
                _context.Carts.Add(cart);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Товар добавлен в корзину" });
        }

        // ✏️ Изменить количество товара
        [HttpPut("{cartId}")]
        public async Task<IActionResult> UpdateQuantity(int cartId, [FromBody] int quantity)
        {
            var cartItem = await _context.Carts.FindAsync(cartId);
            if (cartItem == null)
                return NotFound();

            if (quantity <= 0)
                _context.Carts.Remove(cartItem);
            else
                cartItem.Quantity = quantity;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Количество обновлено" });
        }

        // ❌ Удалить товар из корзины
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCartItem(int cartId)
        {
            var cartItem = await _context.Carts.FindAsync(cartId);
            if (cartItem == null)
                return NotFound();

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Товар удалён из корзины" });
        }

        // 🗑️ Очистить корзину пользователя
        [HttpDelete("clear/{customerId}")]
        public async Task<IActionResult> ClearCart(int customerId)
        {
            var items = _context.Carts.Where(c => c.CustomerId == customerId);
            _context.Carts.RemoveRange(items);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Корзина очищена" });
        }
    }
}
