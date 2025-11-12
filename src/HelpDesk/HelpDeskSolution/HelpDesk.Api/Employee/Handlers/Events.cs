using HelpDesk.Api.Employee.Models;
using HelpDesk.Api.HttpClients;

namespace HelpDesk.Api.Employee.Handlers;

public record EmployeeSubmittedIssue(EmployeeIssueReadModel EmployeeIssue);

public record VipIssueReported();
public record NonVipIssueReported();

public record SupportedSoftwareReported(SoftwareCatalogItem Item);
public record UnsupportedSoftwareReported();