using HelpDesk.Api.Employee.Models;

namespace HelpDesk.Api.Employee.Handlers;

public record ProcessEmployeeIssue(EmployeeIssueReadModel EmployeeIssue);
public record CheckForVipStatus(Guid IssueId, Guid EmployeeId);
public record CheckForSupportedSoftware(Guid IssueId, Guid SoftwareId);
