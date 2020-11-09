﻿namespace Mediator.PeriodicTable.Contracts
{
    public interface IElement
    {
        int ElementId { get; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }
}
