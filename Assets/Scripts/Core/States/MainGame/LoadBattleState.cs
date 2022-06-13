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
        private readonly GameController _gameController;

        public LoadBattleState(ISceneService sceneService, IEventDispatcher eventDispatcher, GameController gameController)
        {
            _sceneService = sceneService;
            _eventDispatcher = eventDispatcher;
            _gameController = gameController;
        }

        public override void Enter(object context)
        {
            base.Enter(context);

            _eventDispatcher.SubscribeOnce<SceneLoaded>(BattleSceneLoaded);
            _sceneService.LoadScene(GameConstants.SceneNameBattle);
        }

        private void BattleSceneLoaded(SceneLoaded loadedEvent)
        {
            if (!loadedEvent.SceneName.Equals(GameConstants.SceneNameBattle)) return;
            // just inform battle scene is ready
            _eventDispatcher.Publish(new BattleSceneLoadedEvent());
        }
    }
}