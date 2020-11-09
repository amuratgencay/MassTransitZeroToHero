using System;

namespace RabbitMQ.PeriodicTable.Contracts
{
    public class Element : IElement
    {
        public int ElementId { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }


        public Element() { }
        public Element(IElement e)
            => (ElementId, ShortName, LongName)
                = (e.ElementId, e.ShortName, e.LongName);


        public override string ToString()
            => string.Format("Element({0}. [{1}] - {2})",
                ElementId, ShortName, LongName);
    }
}
