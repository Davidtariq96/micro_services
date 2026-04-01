
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

    var builder = WebApplication.CreateBuilder(args);
    var assembly = typeof(Program).Assembly;
    
    //Add services to ASP.NET Dependency container
    builder.Services.AddMediatR(config =>
    { 
        config.RegisterServicesFromAssembly(assembly);
        //ADDING The Validation behaviour for request
        config.AddOpenBehavior(typeof(ValidationBehaviours<,>));
        config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    });
    // Application / cross-cutting concerns
    builder.Services.AddValidatorsFromAssembly(assembly);
    //Carter version don't have the config method to give it an assembly(Program.cs), hence it was moved to Catalog.API Service
    builder.Services.AddCarter();
    //Third-party infrastructure
    builder.Services.AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions();
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.InitializeMartenWith<CatalogInitialData>();
    }
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
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