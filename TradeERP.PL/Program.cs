using TradeERP.PL.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppSettings();

var app = builder.Build();

await app.ConfigureRequestPipeline();

app.Run();
