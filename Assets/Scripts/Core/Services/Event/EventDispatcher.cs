using System;
using System.Collections.Generic;

namespace Core.Services.Event
{
    public class EventDispatcher : IEventDispatcher
    {
        /// <summary>
        /// A list of SubscriberGroups.
        /// </summary>
        private readonly List<MessageSubscriberGroup> _groups = new List<MessageSubscriberGroup>();


        public Action Subscribe<T>(Action<T> subscriber)
        {
            return Group(typeof(T))
                .AddSubscriber<T>((message, unsub) => subscriber(message));
        }

        public Action SubscribeOnce<T>(Action<T> subscriber) where T : IGameEvent
        {
            return Group(typeof(T))
                .AddSubscriber<T>((message, unsub) => subscriber(message), true);
        }

        public void Publish<T>(T message) where T : IGameEvent
        {
            Type messageType = message.GetType();
            for (int i = 0, len = _groups.Count; i < len; i++)
            {
                var group = _groups[i];
                if (group.MessageType == messageType)
                {
                    group.Publish(message);
                    break;
                }
            }
        }


        private MessageSubscriberGroup Group(Type messageType)
        {
            MessageSubscriberGroup subscribers = null;
            for (int i = 0, len = _groups.Count; i < len; i++)
            {
                var group = _groups[i];
                if (group.MessageType == messageType)
                {
                    subscribers = group;

                    break;
                }
            }

            if (null == subscribers)
            {
                subscribers = new MessageSubscriberGroup(messageType);
                _groups.Add(subscribers);
            }
            return subscribers;
        }
    }
}