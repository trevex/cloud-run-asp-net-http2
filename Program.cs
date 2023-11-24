var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var url = $"http://0.0.0.0:{port}";

// Let's only use HTTP2!
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(Int32.Parse(port), listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/ping", () =>
{
    return "pong";
});


app.MapGet("/numbers", async (HttpContext context)  =>
{
    Random random = new Random();
    TimeSpan duration = TimeSpan.FromSeconds(600);
    DateTime startedAt = DateTime.UtcNow;

    context.Response.ContentType = "text/plain; charset=utf-8; x-subtype=json";
    for (int i = 1; i <= Int32.MaxValue; i++)
    {
        if (DateTime.UtcNow > startedAt + duration)
            break;

        byte[] data = System.Text.Encoding.UTF8.GetBytes($"{i}{Environment.NewLine}");
        await context.Response.Body.WriteAsync(data, 0, data.Length);
        await context.Response.Body.FlushAsync();
        // Let's slow this intentionally down
        await Task.Delay(TimeSpan.FromSeconds(.5));
    }
}).WithOpenApi();

app.Run(url);
