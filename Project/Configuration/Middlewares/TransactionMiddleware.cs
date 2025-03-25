using Microsoft.EntityFrameworkCore.Storage;
using Project.DB;
using System.Data.SqlClient;

namespace Project.Configuration.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public TransactionMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await _next(context);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
