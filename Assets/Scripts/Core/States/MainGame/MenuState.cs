using System;
using Core.Events;
using Core.Services.Event;
using Core.Services.Logging;
using Models;
using Renders;
using Object = UnityEngine.Object;

namespace Core.States.MainGame
{
    public class MenuState : BaseGameState
    {
        private MainMenuRenderState _mainRenderer;
        private readonly IEventDispatcher _eventService;
        private readonly GameController _gameController;

        private Action _unsubCardInput;

        public MenuState(
            IEventDispatcher eventService,
            GameController gameController)
        {
            _eventService = eventService;
            _gameController = gameController;
        }

        public override void Enter(object context)
        {
            base.Enter(context);
            _mainRenderer = Object.FindObjectOfType<MainMenuRenderState>();
            _mainRenderer.Inject(_eventService, _gameController.GetRuntimeState());
            _mainRenderer.Enter();

            _unsubCardInput = _eventService.Subscribe<HeroCardInputEvent>(OnHeroCardInputEvent);
        }

        private void OnHeroCardInputEvent(HeroCardInputEvent inputEvent)
        {
            string heroId = inputEvent.card.GetHeroId();

            if (inputEvent.inputType == HeroCardInputEvent.InputType.Hold)
            {
                _mainRenderer.RenderCardInfoPopup(heroId);
            }
            else if (inputEvent.inputType == HeroCardInputEvent.InputType.Tap)
            {
                RuntimeGameModel runtimeGameState = _gameController.GetRuntimeState();

                bool changed = runtimeGameState.AddOrRemoveFromTeam(heroId);
                if (changed)
                {
                    if (runtimeGameState.IsHeroInPlayerTeam(heroId))
                    {
                        inputEvent.card.Select();
                    }
                    else
                    {
                        inputEvent.card.Deselect();
                    }

                    _mainRenderer.UpdateBattleButtonState();
                }
            }
            else
            {
                Log.Warning("undefined input event handling");
            }

        }


        public override void Exit()
        {
            base.Exit();
            _mainRenderer.Exit();
            _unsubCardInput();
        }
    }
}