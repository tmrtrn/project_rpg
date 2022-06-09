using System;

namespace Core.Services.Event
{
    /// <summary>
    /// interface for sending and receiving messages
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Subscribes to a specific messageType and returns a method to unsubscribe.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Action Subscribe<T>(Action<T> subscriber);

        /// <summary>
        /// Subscribes to a specific messageType only once and returns a method to unsubscribe. When the
        /// first message is received, the subscriber is immediately unsubscribed.
        /// </summary>
        /// <param name="subscriber"></param>
        Action SubscribeOnce<T>(Action<T> subscriber) where T : IGameEvent;

        /// <summary>
        /// Publishes a message that will call the subscribers
        /// </summary>
        /// <param name="message"></param>
        void Publish<T>(T message) where T : IGameEvent;
    }
}