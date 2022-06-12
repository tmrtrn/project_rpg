using UnityEngine;

namespace Renders
{
    public interface IGameRenderState
    {
        void Enter();
        void Exit();
    }
}