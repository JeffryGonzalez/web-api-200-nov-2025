using HelpDesk.Api.Services;
using Marten;

namespace HelpDesk.Api;

public static class DatabaseSeeder
{
    public static async Task SeedUsers(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        using (scope)
        {
            var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
            session.Store(new UserIdentity()
            {
                Id =Guid.Parse("2F227AD9-B448-4616-9057-05E7763716EA"),
                Name = "bob@company.com"
            });
            session.Store(new UserIdentity()
            {
                Id = Guid.Parse("171D52A7-A55F-4AAF-8CDA-F09D406C7DF4"),
                Name = "sue@company.com"
            });
            await session.SaveChangesAsync();

        }
    }
}