

namespace HelpDesk.Common.Vips;

public record VipRequestMessage
{
    public required string UserSubject { get; init; }
}

public record VipResponseMessage
{
    public required string UserSubject { get; init; }
    public required bool IsVip { get; init; }
}

