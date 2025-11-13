using System.Text.Json;
using HelpDesk.Api.HttpClients;
using HelpDesk.Common.Vips;
using HelpDesk.Tests.Demos;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace HelpDesk.Tests.Vips;

[Collection("WireMockFixture")]
[Trait("Category", "UnitIntegration")]
public class UsingTheClient(DemoFixture fixture)
{
    [Fact]
    public async Task IsVip()
    {

        // Given This Environment 
        // - Some comes from the collection, some in this test.
        // - Have a UserIdentity with the Id of 2F227AD9-B448-4616-9057-05E7763716EA and the name of bob@company.com (From the Fixture)
        // - I have an API that implements POST /vip-check (server is WireMock, the request is below)
        var expectedRequest = new VipRequestMessage { UserSubject = "bob@company.com" };
        
        var formattedRequestMessage = System.Text.Json.JsonSerializer.Serialize(expectedRequest, new JsonSerializerOptions
        {
            // add the stuff here to match what the api is using...
           // PropertyNamingPolicy = new CamelCaseNamingStrategy
        });

        fixture.MockServer
            .Given(Request.Create()
                .WithPath("/vip-check")
                .UsingMethod("POST")
                .WithBodyAsJson(new  // talk about this - hard lesson relearned about 100 times.
                {
                    userSubject = "bob@company.com"
                }, MatchBehaviour.AcceptOnMatch)
                ).RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new VipResponseMessage()
                {
                    IsVip = true,
                    UserSubject = "bob@company.com"
                })
            );
        var employeeId = Guid.Parse("2F227AD9-B448-4616-9057-05E7763716EA");
        var client = fixture.Scope.ServiceProvider.GetRequiredService<VipApiClient>();
        
        // When I ask teh VipApiClient if this employee is a VIP
        var result = await client.CheckIfEmployeeIsVipAsync(employeeId);
        // Then they are a VIP. Yay!
        Assert.True(result);
    }
    [Fact]
    public async Task NonVip()
    {
        fixture.MockServer
            .Given(Request.Create()
                .WithPath("/vip-check")
                .UsingMethod("POST")
                .WithBodyAsJson(new 
                {
                    userSubject = "bob@company.com"
                }, MatchBehaviour.AcceptOnMatch)
            ).RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new VipResponseMessage()
                {
                    IsVip = false,
                    UserSubject = "bob@company.com"
                })
            );
        var employeeId = Guid.Parse("2F227AD9-B448-4616-9057-05E7763716EA");
        var client = fixture.Scope.ServiceProvider.GetRequiredService<VipApiClient>();
        
        var result = await client.CheckIfEmployeeIsVipAsync(employeeId);
        
        Assert.False(result);
    }
}