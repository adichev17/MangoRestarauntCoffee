using Mango.Services.Email.DBContexts;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        //DbContext - Singlton.
        private readonly DbContextOptions<ApplicationDbContext> _dBContexts;

        public EmailRepository(DbContextOptions<ApplicationDbContext> dBContextsOptions)
        {
            _dBContexts = dBContextsOptions;
        }

        public async Task SendAndLogEmail(UpdatePaymentResultMessage message)
        {
            //implement an email sender or call class other class library
            EmailLog emailLog = new()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully."
            };

            await using var _db = new ApplicationDbContext(_dBContexts);
            _db.EmailLogs.Add(emailLog);
            await _db.SaveChangesAsync();
        }
    }
}
