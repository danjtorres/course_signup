using System;
using System.Threading.Tasks;
using Courses.Domain.Messages;
using EasyNetQ;

namespace Courses.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        bool IsConnected { get; }
        IAdvancedBus AdvancedBus { get; }

        Task PublishAsync<T>(T message) where T : IntegrationEvent;

        void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class;

    }
}
