namespace HelpDesk.Vips.Api.Management.Models;

public class VipInactiveReadModel
{
    public Guid Id { get; set; }
    public string UserSubject { get; set; } = string.Empty;
    public string ReasonDeactivation { get; set; } = string.Empty;
    public DateTimeOffset Deactivated { get; set; } 
}