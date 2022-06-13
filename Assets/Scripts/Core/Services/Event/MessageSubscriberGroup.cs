using System;
using System.Collections.Generic;
using Core.Services.Logging;

namespace Core.Services.Event
{
    internal class MessageSubscriberGroup
    {
        /// <summary>
        /// List of subscribers.
        /// </summary>
        private readonly List<Delegate> _subscribers = new List<Delegate>();

        /// <summary>
        /// List of subscribers that unsubscribed
        /// to refrain "Collection was modified" exception while dispatching
        /// </summary>
        private readonly List<Delegate> _toUnsubscribe = new List<Delegate>();

        /// <summary>
        /// message type
        /// </summary>
        public Type MessageType { get; private set; }

        /// <summary>
        /// True iff currently dispatching
        /// </summary>
        public bool IsDispatching { get; private set; }

        private object _message;

        /// <summary>
        /// Message we are currently dispatching. Only set whilst IsDispatching
        /// is true.
        /// </summary>
        public object Message {
            get
            {
                return _message;
            }
            private set
            {
                _message = value;

                IsDispatching = null != _message;
            }
        }


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
                if (IsDispatching)
                {
                    // do not remove from the list while message group is dispatching
                    // "collection modified exception"
                    _toUnsubscribe.Add(action);
                }
                else
                {
                    _subscribers.Remove(action);
                }
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
            Message = message;
            foreach (Action<T> handler in _subscribers)
            {
                try
                {
                    handler(message);
                }
                catch (Exception exception)
                {
                    Log.Error($"Exception while publishing the message {MessageType}", exception.Message);
                }
            }

            // unsubscribes
            var length = _toUnsubscribe.Count;
            if (length > 0)
            {
                for (var i = 0; i < length; i++)
                {
                    _subscribers.Remove(_toUnsubscribe[i]);
                }
                _toUnsubscribe.Clear();
            }

            Message = null;
        }

    }
}