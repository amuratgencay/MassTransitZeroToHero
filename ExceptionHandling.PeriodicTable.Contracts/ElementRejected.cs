namespace ExceptionHandling.PeriodicTable.Contracts
{
    public interface ElementRejected
    {
        string ElementId { get; }
        public string Reason { get; set; }        
    }
}
