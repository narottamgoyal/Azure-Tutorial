using System.Threading.Tasks;

namespace BasicEventBus.Contracts
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/dotnet3-migration/dev-dotnet3/src/BuildingBlocks/EventBus/EventBus/Abstractions/IEventBus.cs
    /// </summary>
    public interface IEventBusService
    {
        Task Publish<T>(T eventMessage) where T : class, IEvent;

        void Subscribe<T, TH>() where T : IEvent
                                where TH : IEventHandler<T>;

        //void Unsubscribe<T, TH>() where T : IBaseEvent
        //                          where TH : IBaseEventHandler<T>;
    }
}
