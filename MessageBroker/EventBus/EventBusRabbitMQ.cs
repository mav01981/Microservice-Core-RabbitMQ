using MicroServiceOne.ServiceBus.Events;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MicroServiceOne
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        public readonly IEventBusSubscriptionsManager _subscriptionManager;
        private readonly IOptions<AppSettings> _appSettings;

        public EventBusRabbitMQ(IOptions<AppSettings> settings, IEventBusSubscriptionsManager subscriptionManager)
        {
            _appSettings = settings;
            _subscriptionManager = subscriptionManager;
        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;

            var factory = new ConnectionFactory() { HostName = _appSettings.Value.Server };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _appSettings.Value.BrokerName,
                    type: "direct");

                string message = JsonConvert.SerializeObject(@event);

                channel.BasicPublish(exchange: _appSettings.Value.BrokerName,
                    routingKey: eventName,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(message));
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subscriptionManager.GetEventKey<T>();
            Subscribe(eventName);

            _subscriptionManager.AddSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {

        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {

        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {

        }

        private void Subscribe(string eventName)
        {
            var factory = new ConnectionFactory() { HostName = _appSettings.Value.Server };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueBind(queue: _appSettings.Value.QueueName,
                              exchange: _appSettings.Value.BrokerName,
                              routingKey: eventName);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
           {
               var _eventName = ea.RoutingKey;
               var message = Encoding.UTF8.GetString(ea.Body);

               await ProcessEvent(_eventName, message);

               channel.BasicAck(ea.DeliveryTag, multiple: false);
           };

            channel.BasicConsume(queue: _appSettings.Value.QueueName,
                                 autoAck: false,
                                 consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                Debug.WriteLine(ea.Exception);
            };

        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);

                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        var handler = Activator.CreateInstance(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                        dynamic eventData = JObject.Parse(message);
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var eventType = _subscriptionManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                        var handler = Activator.CreateInstance(subscription.HandlerType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
