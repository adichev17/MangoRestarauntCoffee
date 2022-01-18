using Mango.Services.OrderAPI.DBContexts;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        //DbContext - Singlton.
        private readonly DbContextOptions<ApplicationDbContext> _dBContexts;

        public OrderRepository(DbContextOptions<ApplicationDbContext> dBContextsOptions)
        {
            _dBContexts = dBContextsOptions;
        }
        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _db = new ApplicationDbContext(_dBContexts);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dBContexts);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }
        }
    }
}
