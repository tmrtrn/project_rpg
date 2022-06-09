using System;
using System.Collections.Generic;
using Core.Services.Logging;

namespace Core.Services.Event
{
    internal class MessageSubscriberGroup
    {
        private delegate void EventHandler<T>(T msg) where T : IGameEvent;
        /// <summary>
        /// List of subscribers.
        /// </summary>
        private readonly List<Delegate> _subscribers = new List<Delegate>();

        /// <summary>
        /// message type
        /// </summary>
        public Type MessageType { get; private set; }


        public MessageSubscriberGroup(Type messageType)
        {
            MessageType = messageType;
        }


        public Action AddSubscriber<T>(Action<T, Action> subscriber, bool once = false)
        {
            Action unsub = null;
            Action<T> action = message =>
            {
                subscriber(message, unsub);

                if (once)
                {
                    unsub();
                }
            };

            unsub = () =>
            {
                _subscribers.Remove(action);
            };

            _subscribers.Add(action);

            return unsub;
        }



        /// <summary>
        /// Publishes a message
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="???"></exception>
        /// <exception cref="AggregateException"></exception>
        public void Publish<T>(T message) where T : IGameEvent
        {

            foreach (EventHandler<T> handler in _subscribers)
            {
                try
                {
                    handler(message);
                }
                catch (Exception exception)
                {
                    Log.Error($"Exception while publishing the message {MessageType}", exception);
                }

            }
        }

    }
}