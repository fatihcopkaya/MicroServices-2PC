using Cordinator.Context;
using Cordinator.Models;
using Cordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Cordinator.Services
{
    public class TransactionService(IHttpClientFactory _httpClientFactory ,TwoPhaseCommitContext _context) : ITransactionService
    {
        HttpClient _orderHttpClient = _httpClientFactory.CreateClient("OrderAPI");
        HttpClient _stockHttpClient = _httpClientFactory.CreateClient("StockAPI");
        HttpClient _paymentHttpClient = _httpClientFactory.CreateClient("PaymentAPI");
        public async Task<Guid> CreateTransactionAsync()
        {
            Guid transactionId = Guid.NewGuid();
            var nodes = await _context.Nodes.ToListAsync();

            nodes.ForEach(node => node.NodeStates = new List<NodeState>()
            {
                new(transactionId)
                {
                    IsReady = Enums.ReadyType.Pending,
                    TransactionState = Enums.TransactionState.Pending,
                }
            });
            await _context.SaveChangesAsync();
            return transactionId;

        }
        public async Task PrepareServicesAsync(Guid transactionId)
        {
           var transactionNodes = await _context.NodeStates.Include(ns => ns.Node)
                .Where(ns => ns.TransactionId == transactionId)
                .ToListAsync();
            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response = await (transactionNode.Node.Name  switch
                    {
                        "Order.API" => _orderHttpClient.GetAsync("ready"),
                        "Stock.API" => _stockHttpClient.GetAsync("ready"),
                        "Payment.API" => _paymentHttpClient.GetAsync("ready"),

                    });
                    var result = bool.Parse(await response.Content.ReadAsStringAsync());
                    transactionNode.IsReady = result ? Enums.ReadyType.Ready : Enums.ReadyType.Unready;
                }
                catch (Exception ex)
                {
                    transactionNode.IsReady = Enums.ReadyType.Unready;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckReadyServicesAsync(Guid transactionId) => 
               (await _context.NodeStates
                     .Where(ns=>ns.TransactionId == transactionId).ToListAsync())
                     .TrueForAll(ns=>ns.IsReady == Enums.ReadyType.Ready);
        
        public async Task CommitAsync(Guid transactionId)
        {
            var transactionNodes = _context.NodeStates
                        .Where(x=>x.TransactionId == transactionId)
                        .Include(x=>x.Node);
            foreach (var transactionNode in transactionNodes)
            {
                try
                {
                    var response = await (transactionNode.Node.Name switch
                    {
                        "Order.API" => _orderHttpClient.GetAsync("commit"),
                        "Stock.API" => _stockHttpClient.GetAsync("commit"),
                        "Payment.API" => _paymentHttpClient.GetAsync("commit"),
                    });
                    var result  = bool.Parse(await response.Content.ReadAsStringAsync());
                    transactionNode.TransactionState = result ? Enums.TransactionState.Done : Enums.TransactionState.Abort;  
                }
                catch (Exception)
                {

                    transactionNode.TransactionState = Enums.TransactionState.Abort;
                }
            }
           await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckTransactionStateServicesAsync(Guid transactionId) =>
                                (await _context.NodeStates
                                .Where(x => x.TransactionId == transactionId)
                                .ToListAsync()).TrueForAll(x=>x.TransactionState == Enums.TransactionState.Done);

        public async Task RollBackAsync(Guid transactionId)
        {
            var transactionNodes = await _context.NodeStates
                 .Where(x => x.TransactionId == transactionId)
                 .Include(x => x.Node).ToListAsync();
            foreach (var transactionNode in transactionNodes)
            {
                if(!(transactionNode.TransactionState == Enums.TransactionState.Done))
                {
                    try
                    {
                        _ = transactionNode.Node.Name switch
                        {
                            "Order.API" => _orderHttpClient.GetAsync("rollback"),
                            "Stock.API" => _stockHttpClient.GetAsync("rollback"),
                            "Payment.API" => _paymentHttpClient.GetAsync("rollback"),
                        };
                        transactionNode.TransactionState = Enums.TransactionState.Abort;
                    }
                    catch (Exception)
                    {

                        transactionNode.TransactionState = Enums.TransactionState.Abort;
                    }

                }
                
            }
          
            await _context.SaveChangesAsync();
        }
    }
}
