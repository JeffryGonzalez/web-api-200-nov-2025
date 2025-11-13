using JasperFx.Events;
using Marten.Events.Aggregation;

namespace HelpDesk.Vips.Api.Management.Models;

public class VipReadModelProjection : SingleStreamProjection<VipReadModel, Guid>
{


    public VipReadModelProjection()
    {
        DeleteEvent<VipDeactivated>();
    }

    public static VipReadModel Create(IEvent<VipAdded> @event)
    {
        
        return new VipReadModel
        {
            Id = @event.Data.Id,
            Created = @event.Timestamp,
            Reason = @event.Data.Data.Reason,
            UserSubject = @event.Data.Data.UserSubject,
        };
    }


}
