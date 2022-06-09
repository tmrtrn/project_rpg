using System;
using System.Collections;
using UnityEngine;

namespace Core.Services.Async
{
    public class UnityAsyncService : MonoBehaviour, IAsyncService
    {
        public void BootstrapCoroutine(IEnumerator coroutine, Action callback = null)
        {
            WaitCoroutine(coroutine, callback);
        }

        public void Wait(float milliseconds, Action callback)
        {
            BootstrapCoroutine(WaitEnumerator(milliseconds, callback));
        }

        /// <summary>
        /// Coroutine fires a callback when is over
        /// </summary>
        /// <param name="coroutine"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator WaitCoroutine(IEnumerator coroutine, Action callback)
        {
            yield return StartCoroutine(coroutine);
            if (callback != null)
            {
                callback();
            }
        }

        /// <summary>
        /// Coroutine that uses Unity's wait method.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private static IEnumerator WaitEnumerator(float milliseconds, Action callback)
        {
            yield return new WaitForSecondsRealtime(milliseconds / 1000);

            callback();
        }
    }
}