using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Alba;
using HelpDesk.Api.Demos;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Time.Testing;

namespace HelpDesk.Tests.Demos;
public class StillOpen2Tests
{

  
  

    [Fact]
    public async Task WhenWeAreStillOpen()
    {
    var FakeClock = new FakeTimeProvider(new DateTimeOffset(1969, 4, 20, 16, 59, 59, TimeSpan.FromHours(-5)));
    var Host = await AlbaHost.For<Program>(config =>
    {
        config.UseSetting("services:software:http:0", "https://not-real");
        config.ConfigureTestServices(sp =>
        {
            sp.AddSingleton<TimeProvider>(_ => FakeClock);
        });
    });

        var response = await Host.Scenario(api =>
        {
            api.Get.Url("/demos/still-open");
            api.StatusCodeShouldBe(200);
        });

        var body = response.ReadAsJson<BusinessHoursResponse>();
        Assert.NotNull(body);
        Assert.True(body.StillOpen);
    }
    [Fact]
    public async Task WhenWeAreClosed()
    {
        var FakeClock = new FakeTimeProvider(new DateTimeOffset(1969, 4, 20, 17, 00, 00, TimeSpan.FromHours(-5)));
        var Host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("services:software:http:0", "https://not-real");
            config.ConfigureTestServices(sp =>
            {
                sp.AddSingleton<TimeProvider>(_ => FakeClock);
            });
        });
        var response = await Host.Scenario(api =>
        {
            api.Get.Url("/demos/still-open");
            api.StatusCodeShouldBe(200);
        });

        var body = response.ReadAsJson<BusinessHoursResponse>();
        Assert.NotNull(body);
        Assert.False(body.StillOpen);
    }


}
