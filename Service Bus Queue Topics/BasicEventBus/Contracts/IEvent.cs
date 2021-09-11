using System.Collections.Generic;

namespace BasicEventBus.Contracts
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/dotnet3-migration/dev-dotnet3/src/BuildingBlocks/EventBus/EventBus/Events/IntegrationEvent.cs
    /// </summary>
    public interface IEvent
    {
        string EventName { get; set; }
        IList<string> FilterKeys { get; set; }
    }
}