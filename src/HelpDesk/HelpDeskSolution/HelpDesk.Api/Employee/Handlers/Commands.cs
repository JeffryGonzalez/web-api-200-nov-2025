using HelpDesk.Api.Employee.Models;

namespace HelpDesk.Api.Employee.Handlers;
// Commands initiate an activity - either synchronously or asynchronously.
// They are a way to (in a loosely coupled way) have one thing pass the buck - someone else do this
// Commands still have a cause and effect pattern - one thing handles the command. Many places in your
// application can publish (or invoke) the command.

public record ProcessEmployeeIssue(EmployeeIssueReadModel EmployeeIssue);
public record CheckForVipStatus(Guid IssueId, Guid EmployeeId);
public record CheckForSupportedSoftware(Guid IssueId, Guid SoftwareId);
