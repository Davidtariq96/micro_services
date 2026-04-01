var builder = WebApplication.CreateBuilder(args);

//Add Service to Building container
var app = builder.Build();

//Configure HTTPS Request Pipeline


app.Run();