using HelpDesk.Api.Employee.Handlers;
using JasperFx.Events;

namespace HelpDesk.Api.Employee.Models;

public record IssueHistoryReadModel
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public List<IssueHistoryItem> Items { get; set; } = new List<IssueHistoryItem>();

    public static IssueHistoryReadModel Create(IEvent<EmployeeSubmittedIssue> @event)
    {
        return new IssueHistoryReadModel
        {
          Items = new List<IssueHistoryItem>()
          {
              new IssueHistoryItem
              {
                  WhenItHappened = @event.Timestamp,
                  EventName = @event.DotNetTypeName
              }
          }
        };
    }
    public static IssueHistoryReadModel Apply(IEvent<VipIssueReported> @event, IssueHistoryReadModel model)
    {
        return model with
        {
            Items = [..model.Items, new IssueHistoryItem {
                    WhenItHappened = @event.Timestamp,
                  EventName = @event.DotNetTypeName
        }]
        };
    }

}


public record IssueHistoryItem
{
    public DateTimeOffset WhenItHappened { get; set; }
    public string EventName { get; set; } = string.Empty;
}