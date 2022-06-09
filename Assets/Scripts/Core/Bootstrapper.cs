using Core.Services.Event;
using Core.Services.Logging;
using DefaultNamespace;
using UnityEngine;

namespace Core
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            // targets Unity logging
            Log.SetLogTarget(new UnityLogTarget());
        }

    }
}