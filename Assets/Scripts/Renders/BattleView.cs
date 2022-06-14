using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Events;
using Core.Events.GameEvents;
using Core.Services.Event;
using Models;
using TMPro;
using UnityEngine;

namespace Renders
{
    public class BattleView : MonoBehaviour, IBattleRenderer
    {
        [SerializeField] private GameObject playerHeroCardPrefab;
        [SerializeField] private Transform playerHeroPanel;

        [SerializeField] private GameObject opponentHeroCardPrefab;
        [SerializeField] private Transform opponentHeroPanel;

        [SerializeField] private TMP_Text turnText;
        [SerializeField] private DamageText _damageText;

        [SerializeField] private GameObject parentCanvas;
        [SerializeField] private InfoPopUpView popupView;

        private IGameController _gameController;
        private IEventDispatcher _eventService;

        private Action _unSubPlayingStateStarted;
        private Action _unsubCardInput;


        private Dictionary<string, IHeroBattleCardView> _playerHeroCards;
        private Dictionary<string, IHeroBattleCardView> _opponentHeroCards;

        public void InjectServices(IGameController gameController, IEventDispatcher eventService)
        {
            _gameController = gameController;
            _eventService = eventService;
        }

        public void Enter(object context)
        {
            parentCanvas.SetActive(true);
            RuntimeGameModel runtimeState = _gameController.GetRuntimeState();

            // instantiate player team
            string[] playerTeam = runtimeState.GetPlayerTeam();
            _playerHeroCards = new Dictionary<string, IHeroBattleCardView>(playerTeam.Length);
            foreach (string heroId in playerTeam)
            {
                HeroModel hero = runtimeState.GetPlayerHeroModel(heroId);
                IHeroBattleCardView card = CreatePlayerHeroCard(hero);
                _playerHeroCards.Add(heroId, card);
            }
            // instantiate opponent team
            string[] opponentTeam = runtimeState.GetEnemyTeam();
            _opponentHeroCards = new Dictionary<string, IHeroBattleCardView>(opponentTeam.Length);
            foreach (string heroId in opponentTeam)
            {
                HeroModel hero = runtimeState.GetOpponentHeroModel(heroId);
                IHeroBattleCardView card = CreateOpponentHeroCard(hero);
                _opponentHeroCards.Add(heroId, card);
            }

            _unSubPlayingStateStarted = _eventService.Subscribe<PlayingStateChangedEvent>(OnPlayingStateChanged);
            _unsubCardInput = _eventService.Subscribe<HeroCardInputEvent>(OnHeroCardInputEvent);
        }

        public void UpdateState(float deltaTime)
        {

        }

        IHeroBattleCardView CreatePlayerHeroCard(HeroModel heroModel)
        {
            GameObject heroViewObj = Instantiate(playerHeroCardPrefab, playerHeroPanel);
            IHeroBattleCardView card = heroViewObj.GetComponent<IHeroBattleCardView>();
            card.Render(heroModel, _eventService);
            return card;
        }

        IHeroBattleCardView CreateOpponentHeroCard(HeroModel heroModel)
        {
            GameObject heroViewObj = Instantiate(opponentHeroCardPrefab, opponentHeroPanel);
            IHeroBattleCardView card = heroViewObj.GetComponent<IHeroBattleCardView>();
            card.Render(heroModel, _eventService, true);
            return card;
        }

        void OnPlayingStateChanged(PlayingStateChangedEvent @event)
        {
            if (@event.isGameOver)
            {
                turnText.text = "PROCESSING RESULT";
            }
            else
            {
                turnText.text = @event.isPlayerTurn ? "YOUR TURN" : "OPPONENT TURN";
            }
        }

        void OnHeroCardInputEvent(HeroCardInputEvent inputEvent)
        {
            if (inputEvent.inputType == HeroCardInputEvent.InputType.Hold)
            {
                string id = inputEvent.card.GetHeroId();
                IHeroModelAttribute model = inputEvent.isOpponentCard
                    ? _gameController.GetRuntimeState().GetOpponentHeroModel(id)
                    : _gameController.GetRuntimeState().GetPlayerHeroModel(id);
                popupView.Render(model, true);
            }
        }

        public void Exit()
        {
            _unSubPlayingStateStarted();
            _unsubCardInput();
            parentCanvas.SetActive(false);
        }

        /// <summary>
        /// Visualize the attack, do not change any model
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="attacker"></param>
        /// <param name="playerAttack"></param>
        /// <param name="damage"></param>
        /// <returns></returns>
        public IEnumerator AttackToTarget(string targetId, string attacker, bool playerAttack, float damage)
        {
            IHeroBattleCardView attackerView = playerAttack ?
                _playerHeroCards[attacker] :
                _opponentHeroCards[attacker];

            IHeroBattleCardView targetView = playerAttack ?
                _opponentHeroCards[targetId] :
                _playerHeroCards[targetId];

            attackerView.Select();
            bool animCompleted = false;
            _damageText.MakeMove(targetView.TargetPoint.position, damage, () =>
            {
                animCompleted = true;
            });

            yield return new WaitForSeconds(0.2f);
            targetView.UpdateHealth();

            while (!animCompleted)
            {
                yield return null;
            }
            attackerView.Deselect();
        }
    }
}