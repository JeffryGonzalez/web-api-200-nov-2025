namespace HelpDesk.Vips.Api.Management.Models;

public class VipReadModel
{
    public Guid Id { get; set; }
    public string UserSubject { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; } 
}