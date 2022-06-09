using System;
using System.Collections;

namespace Core.Services.Async
{
    public interface IAsyncService
    {
        void BootstrapCoroutine(IEnumerator coroutine, Action callback = null);
        void Wait(float milliseconds, Action callback);
    }
}