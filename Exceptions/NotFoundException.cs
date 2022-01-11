using System;
namespace SignalRApi.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName) : base($"Cannot not found entity type of {entityName} based on given data")
        {
        }
    }
}
