using System;
using System.Collections.Generic;
using Core.Services;
using Core.Services.State;

namespace Core.States
{
    public class BaseGameState : IState
    {
        public virtual void Enter(object context)
        {

        }

        public virtual void UpdateState(float deltaTime)
        {

        }

        public virtual void Exit()
        {

        }
    }
}