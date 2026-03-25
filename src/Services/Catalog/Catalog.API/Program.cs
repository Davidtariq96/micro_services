var builder = WebApplication.CreateBuilder(args);
//Add services to container
var app = builder.Build();

//Configure HTTPS Request Pipeline

app.Run();