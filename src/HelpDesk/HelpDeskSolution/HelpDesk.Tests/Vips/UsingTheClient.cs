using HelpDesk.Api.HttpClients;
using HelpDesk.Tests.Demos;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace HelpDesk.Tests.Vips;

[Collection("WireMockFixture")]
public class UsingTheClient(DemoFixture fixture)
{
    [Fact]
    public async Task IsVip()
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
                    IsVip = true,
                    UserSubject = "bob@company.com"
                })
            );
        var employeeId = Guid.Parse("2F227AD9-B448-4616-9057-05E7763716EA");
        var client = fixture.Scope.ServiceProvider.GetRequiredService<VipApiClient>();
        
        var result = await client.CheckIfEmployeeIsVipAsync(employeeId);
        
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