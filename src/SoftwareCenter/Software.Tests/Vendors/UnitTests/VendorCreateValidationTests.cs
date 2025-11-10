using FluentValidation.TestHelper;
using Software.Api.Vendors;
using Software.Api.Vendors.Models;

namespace Software.Tests.Vendors.UnitTests;

[Trait("Category", "UnitIntegration")]
public class VendorCreateValidationTests
{
    [Theory]
    [MemberData(nameof(ValidExamples))]
    public void ValidVendorCreateRequests(VendorCreateModel model, string failureMessage)
    {
        var validation = new VendorCreateModelValidator();
        var validations = validation.TestValidate(model);
        Assert.True(validations.IsValid, failureMessage);
    }

    [Theory]
    [MemberData(nameof(InvalidExamples))]
    public void InvalidVendorCreateRequests(VendorCreateModel model, string failureMessage)
    {
        var validation = new VendorCreateModelValidator();
        var validations = validation.TestValidate(model);
        Assert.False(validations.IsValid, failureMessage);
    }

    private const int VNameMinLength = 3;
    private const int VNameMaxLength = 100;
    private static readonly PointOfContact ValidPoc = new() { Name = "John", Email = "joe@aol.com", Phone = "555-1212" };
    public static IEnumerable<object[]> ValidExamples() =>
    [
        [
            new VendorCreateModel {Name = new string('X', VNameMinLength), Contact = ValidPoc, Url= "https://dog.com"},
            $"The minimum length of vendor name should be {VNameMinLength}"
    ],
        [
            new VendorCreateModel {Name = new string('X', VNameMaxLength), Contact = ValidPoc, Url= "https://dog.com"},
              $"The maximum length of vendor name should be {VNameMaxLength}"
    ],
         [
            new VendorCreateModel { Name = new string('X', VNameMinLength), Url="https://dog.com", Contact = new PointOfContact { Name="Paul", Phone="555-1212" } },

            "Point of Contact must have a phone or email"
            ],
        [
            new VendorCreateModel { Name = new string('X', VNameMinLength), Url="https://dog.com", Contact = new PointOfContact { Name="Paul", Email="paul@aol.com" } },

            "Point of Contact must have a phone or email"
            ],
          [
            new VendorCreateModel { Name = new string('X', VNameMinLength), Url="https://dog.com", Contact = new PointOfContact { Name="Paul", Email="paul@aol.com", Phone="555-1212" } },

            "POC can have both a phone and email"
            ]
        ];

    public static IEnumerable<object[]> InvalidExamples() =>
   [
        [
            new VendorCreateModel {Name = "", Contact = ValidPoc, Url= "https://dog.com"},
         $"The Name Cannot Be Empty and should have a minimum length of  {VNameMinLength}"
        ],
        //new object[]
        [
            new VendorCreateModel { Name = new string('X', VNameMinLength -1), Contact= ValidPoc, Url = "https://dog.com"},
             $"The Name Cannot Be Empty and should have a minimum length of  {VNameMinLength}"
        ],
        [
            new VendorCreateModel { Name = new string('X', VNameMaxLength + 1), Contact= ValidPoc, Url = "https://dog.com"},
              $"The maximum length of vendor name should be {VNameMaxLength}"
        ],
        [
            new VendorCreateModel { Name = new string('X', VNameMinLength), Url="", Contact = ValidPoc },
            "The Url cannot be empty"
            ],
         [
            new VendorCreateModel { Name = new string('X', VNameMinLength), Url="", Contact = new PointOfContact { Name="Paul" } },

            "Point of Contact must have a phone or email"
            ]
        ];

}
