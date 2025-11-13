using HelpDesk.Vips.Api.Management.Models;

namespace HelpDesk.Vips.Api.Management;


public record VipAdded(Guid Id, VipCreateModel Data);

public record VipDeactivated(VipReadModel DeactivatedVip);
