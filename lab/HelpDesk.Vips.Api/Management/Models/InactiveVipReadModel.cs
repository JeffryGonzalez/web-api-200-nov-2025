using JasperFx.Events;
using Marten.Events.Aggregation;
using Wolverine;

namespace HelpDesk.Vips.Api.Management.Models;

public class InactiveVipReadModel
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string UserSubject { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Deactivated { get; set; }
}


public class InactiveVipReadModelProjection : SingleStreamProjection<InactiveVipReadModel, Guid>
{
    public static InactiveVipReadModel Create(IEvent<VipDeactivated> @event)
    {
        return new InactiveVipReadModel
        {
          
            Created = @event.Data.DeactivatedVip.Created,
            Reason = @event.Data.DeactivatedVip.Reason,
            UserSubject = @event.Data.DeactivatedVip.UserSubject,
            Deactivated = @event.Timestamp

        };
    }
}