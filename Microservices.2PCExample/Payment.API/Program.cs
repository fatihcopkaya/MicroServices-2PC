var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



var app = builder.Build();



app.MapGet("/ready", () =>
{
    Console.WriteLine("Payment service is ready");
    return false;
});
app.MapGet("/commit", () =>
{
    Console.WriteLine("Payment service is commited");
    return true;

});
app.MapGet("/rollback", () =>
{
    Console.WriteLine("Payment service is rollbacked");

});

app.Run();
