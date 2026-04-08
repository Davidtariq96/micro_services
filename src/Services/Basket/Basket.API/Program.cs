
var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

//Add Service to Building container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{ 
    config.RegisterServicesFromAssembly(assembly);
    //ADDING The Validation behaviour for request
    config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

//create a BasketRepo to Implement the IBasketRepo interfaces, so that it could be decorated using SCRUTOR LIBRARY
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//Decorator / Proxy pattern serving as middleware via CachedBasketRepository & internally calling BasketRepository
//that implemented the interfaces
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis"));
var app = builder.Build();

//Configure HTTPS Request Pipeline
app.MapCarter();
app.UseExceptionHandler(appError => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();