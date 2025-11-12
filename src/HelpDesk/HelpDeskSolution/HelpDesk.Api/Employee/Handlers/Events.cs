using HelpDesk.Api.Employee.Models;
using HelpDesk.Api.HttpClients;

namespace HelpDesk.Api.Employee.Handlers;


// Events are not commands. They are usually the result of commands. "This command did this or these things"
// Event - a thing that happens at a point in time.
// Events don't have a clear cause and effect like commands.
// Multiple places in code can "raise" (or publish) an event.
// Multiple places in the code can respond to that event.
// That includes nothing. You can publish events and nobody "cares" - no problem.
// Events are always related to a "Stream" - they are not "global"
// These events will be appended to a "log" of things that happen to a particular thing.
// That is called an "Event Stream"
// Event streams can be used to create "views" of the things that happened to a stream over time.
// These are called "aggregations" or "projections"

public record EmployeeSubmittedIssue(EmployeeIssueReadModel EmployeeIssue);

public record VipIssueReported();
public record NonVipIssueReported();

public record SupportedSoftwareReported(SoftwareCatalogItem Item);
public record UnsupportedSoftwareReported();

// sample
public record EmployeeIssueClosedAsResolved(string MessageFromTech);