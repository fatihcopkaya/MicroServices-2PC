using Cordinator.Enums;

namespace Cordinator.Models
{
    public record NodeState(Guid TransactionId)
    {
        public Guid Id { get; set; }
        // ilk aşamada serviceler hazır mı?
        public ReadyType IsReady { get; set; }
        //2. aşama sonucunda işlemin başarılı olup olmadığını ifade ediyor.
        public TransactionState TransactionState { get; set; }
        public Node Node { get; set; }
    }
}
