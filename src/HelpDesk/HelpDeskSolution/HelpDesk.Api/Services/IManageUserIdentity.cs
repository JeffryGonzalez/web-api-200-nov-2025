using Marten;

namespace HelpDesk.Api.Services;

public interface IManageUserIdentity
{
    Task<Guid> GetUserIdFromRequestingContextAsync();
}

// The "Real" Implementation of this thing

public class UserIdentityManager(IHttpContextAccessor context, IDocumentSession session) : IManageUserIdentity
{
    public async Task<Guid> GetUserIdFromRequestingContextAsync()
    {
        var userSub = context.HttpContext.User.Identity.Name;

        // look up in the database if that user is already here, if it is return the id for that user
        var savedUser = await session.Query<UserIdentity>()
            .Where(u => u.Name == userSub)
            .SingleOrDefaultAsync();
        if(savedUser != null)
        {
            return savedUser.Id;
        } else
        {
            var newUser = new UserIdentity
            {
                Id = Guid.NewGuid(),
                Name = userSub
            };
            session.Store(newUser);
            await session.SaveChangesAsync();
            return newUser.Id;
        }
        // else, give them an id, save it to the database, etc.

    }
}

public class UserIdentity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}