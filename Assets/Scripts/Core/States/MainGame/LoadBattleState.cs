using Constants;
using Core.Events;
using Core.Services.Event;
using Core.Services.Scene;
using Models;

namespace Core.States.MainGame
{
    /// <summary>
    /// Prepare and load the battle scene, we can implement a loading bar in here.
    /// </summary>
    public class LoadBattleState : BaseGameState
    {
        private readonly ISceneService _sceneService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IGameController _gameController;

        public LoadBattleState(ISceneService sceneService, IEventDispatcher eventDispatcher, IGameController gameController)
        {
            _sceneService = sceneService;
            _eventDispatcher = eventDispatcher;
            _gameController = gameController;
        }

        public override void Enter(object context)
        {
            base.Enter(context);

            _eventDispatcher.Publish(new ChangeSceneEvent(GameConstants.SceneNameBattle));
        }
    }
}