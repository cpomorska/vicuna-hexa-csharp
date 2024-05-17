using Microsoft.AspNetCore.Diagnostics;
using vicuna_ddd.Shared.Provider;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();

   // using var scope = app.Services.CreateScope();
   // var services = scope.ServiceProvider;
   // var initialiser = services.GetRequiredService<DbInitializer>();
   // initialiser.Run();

    using (var scope = app.Services.CreateScope())
    {
        //replace DataContext with your Db Context name
        var dataContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        dataContext.Database.EnsureCreated();
    }
}

app.UseExceptionHandler(errorHandlingPath: "/error");
app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()?
        .Error;
    var response = new { exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

