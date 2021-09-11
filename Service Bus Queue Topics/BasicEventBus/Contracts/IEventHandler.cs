using System.Threading.Tasks;

namespace BasicEventBus.Contracts
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/dotnet3-migration/dev-dotnet3/src/BuildingBlocks/EventBus/EventBus/Abstractions/IIntegrationEventHandler.cs
    /// </summary>
    /// <typeparam name="BaseEventType"></typeparam>
    public interface IEventHandler<in BaseEventType> : IIntegrationEventHandler
                                                                        where BaseEventType : IEvent
    {
        Task HandleAsync(BaseEventType @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
