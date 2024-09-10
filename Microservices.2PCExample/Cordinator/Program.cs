using Cordinator.Context;
using Cordinator.Services;
using Cordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TwoPhaseCommitContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddHttpClient("OrderAPI", client => client.BaseAddress = new("https://localhost:7222"));
builder.Services.AddHttpClient("StockAPI", client => client.BaseAddress = new("https://localhost:7040"));
builder.Services.AddHttpClient("PaymentAPI", client => client.BaseAddress = new("https://localhost:7245"));
builder.Services.AddTransient<ITransactionService,TransactionService>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/create-order-transaction",async (ITransactionService _transactionService) =>
{
    //pashe 1 - Prepare
    var transactionId = await _transactionService.CreateTransactionAsync();
    await _transactionService.PrepareServicesAsync(transactionId);
    bool transactionState = await _transactionService.CheckReadyServicesAsync(transactionId);

    if (transactionState)
    {
        //pashe2 - Commit
        await _transactionService.CommitAsync(transactionId);
        transactionState = await _transactionService.CheckTransactionStateServicesAsync(transactionId);
        
    }
    if(!transactionState)
    {
        await _transactionService.RollBackAsync(transactionId);
    }
});
app.Run();
