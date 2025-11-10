using FluentValidation;

namespace Software.Api.Vendors.Models;

public record PointOfContact
{
   
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}

public class PointOfContactValidator : AbstractValidator<PointOfContact>
{
    public PointOfContactValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Email).NotEmpty().When(c => c.Phone == "");
        RuleFor(c => c.Phone).NotEmpty().When(c => c.Email == "");
    }
}