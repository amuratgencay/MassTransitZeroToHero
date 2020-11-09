namespace ExceptionHandling.PeriodicTable.Contracts
{
    public interface ElementAccepted
    {
        string ElementId { get; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }
}
