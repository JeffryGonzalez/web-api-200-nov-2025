using HelpDesk.Common.Vips;
var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.MapPost("/vip-check", (VipRequestMessage request) =>
{
    return TypedResults.Ok(new VipResponseMessage { IsVip = false, UserSubject = request.UserSubject });
});

app.Run();
