
using HelpDesk.Api.Employee.Data;
using Marten;

namespace HelpDesk.Api.Employee.BackgroundWorker;

public class IssueProcessor(ILogger<IssueProcessor> logger, IServiceProvider sp) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting the IssueProcessor Background Worker");
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            var scope = sp.CreateScope();
            using var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
            var newIssues = await session.Query<IssueEntity>()
                .Where(issue => issue.Status == IssueStatus.AwaitingVerification)
                .ToListAsync();
            foreach (var newIssue in newIssues)
            {
                logger.LogInformation("Looking at issue {id}", newIssue.Id);
                await CheckSoftwareCenterAsync(); // call the software center api - worked!
                                                  // update the entity and save it in the database...
                                                  // 5. See if it is a VIP, etc.
                await CheckForVipStatus();// check to see if they are a VIP 
                                          // if so, update the entity, save it... 
                                          // 4. Assign it to a tech
                await AssignItToATech();

                newIssue.Status = IssueStatus.Verified;
                session.Store(newIssue);
                await session.SaveChangesAsync();
            }

        }
       
    }

    private static async Task AssignItToATech()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(500));
    }

    private static async Task CheckForVipStatus()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(500));
    }

    private static async Task CheckSoftwareCenterAsync()
    {
        // is (or still is) in the catalog...
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
