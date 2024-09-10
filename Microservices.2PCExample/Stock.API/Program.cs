var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



var app = builder.Build();



app.MapGet("/ready", () =>
{
    Console.WriteLine("Stock service is ready");
    return true;
});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Stock service is commited");
    return true;

});
app.MapGet("/rollback", () =>
{
    Console.WriteLine("Stock service is rollbacked");

});

app.Run();
