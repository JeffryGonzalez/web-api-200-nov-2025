using FluentValidation;
using Software.Api.Vendors.Data;

namespace Software.Api.Vendors.Models;

public record VendorCreateModel
{

    public required string Name { get; init; } = string.Empty;
    public required string Url { get; init; } = string.Empty;
    public required PointOfContact Contact { get; init; }
    public VendorEntity MapToEntity(Guid id, string createdBy)
    {
        return new VendorEntity
        {
            Id = id,
            Contact = Contact,
            Name = Name,
            CreatedBy = createdBy,
            CreatedOn = DateTimeOffset.UtcNow,
            Url = Url,


        };
    }
};

public class VendorCreateModelValidator : AbstractValidator<VendorCreateModel>
{
    public VendorCreateModelValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(v => v.Url).NotEmpty();
        RuleFor(v => v.Contact).NotNull().SetValidator(validator: new PointOfContactValidator()); // The warning was because I had Contact as a nullable reference type.
    }
}
