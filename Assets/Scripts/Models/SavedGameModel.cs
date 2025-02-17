using System;
using System.Collections.Generic;
using Constants;
using Core.Services.Data;

namespace Models
{
    [Serializable]
    public class SavedGameModel : ISerializeModel
    {
        /// <summary>
        /// Player and opponent
        /// </summary>
        private const int PlayerCount = 2;
        /// <summary>
        /// Player have hero collection
        /// </summary>
        public List<SavedHeroModel> heroCollection;

        /// <summary>
        /// Keeps opponent values in battle
        /// </summary>
        public List<SavedHeroModel> enemyHeroCollectionInBattle;

        /// <summary>
        /// Player's team model for battle
        /// </summary>
        public string[] playerTeam;

        /// <summary>
        /// Enemy's team for battle
        /// </summary>
        public string[] enemyTeam;

        /// <summary>
        /// Active battle saved data
        /// </summary>
        public bool isPlayingBattle;

        public int whoisTurn;
        public int turnCounter;

        /// <summary>
        /// move count of whois turn
        /// </summary>
        public int moves;

        public int playedBattleCount;


        public void Reset()
        {
            whoisTurn = -1;
            turnCounter = 0;
            isPlayingBattle = false;
            moves = 0;
        }


        public bool IsPlayerTeamMember(string id)
        {
            return IsTeamMember(id, playerTeam);
        }

        public bool IsEnemyTeamMember(string id)
        {
            return IsTeamMember(id, enemyTeam);
        }

        private bool IsTeamMember(string id, string[] team)
        {
            if (team == null) return false;
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("null member id");
            }
            for (int i = 0; i < team.Length; i++)
            {
                if (id.Equals(team[i])) return true;
            }

            return false;
        }

        public bool AddHeroToPlayerTeam(string id)
        {
            return AddHeroToTeam(id, playerTeam);
        }

        public bool RemoveFromPlayerTeam(string id)
        {
            return RemoveFromTeam(id, playerTeam);
        }


        private bool AddHeroToTeam(string id, string[] team)
        {
            if (IsTeamMember(id, team))
            {
                // hero is already member
                return false;
            }

            for (int i = 0; i < team.Length; i++)
            {
                if (string.IsNullOrEmpty(team[i]))
                {
                    team[i] = id;
                    return true;
                }
            }

            // capacity is full
            return false;
        }

        private bool RemoveFromTeam(string id, string[] team)
        {
            if (!IsTeamMember(id, team))
            {
                return false; //hero is not in the list
            }

            for (int i = 0; i < team.Length; i++)
            {
                if (!string.IsNullOrEmpty(team[i]) && team[i].Equals(id))
                {
                    team[i] = null;
                    return true;
                }
            }

            throw new Exception("player must be in the list");
        }

        public bool IsPlayerTeamFull()
        {
            if (playerTeam.Length == 0) throw new Exception("Member size must be greater than 0");
            foreach (string member in playerTeam)
            {
                if (string.IsNullOrEmpty(member)) return false;
            }

            return true;
        }

        public int GetNextTurnSide()
        {
            int next = whoisTurn + 1;
            if (next >= PlayerCount)
            {
                next = 0;
            }

            return next;
        }

        public void EndTurn()
        {
            int next = GetNextTurnSide();
            turnCounter++;
            whoisTurn = next;
            // reset move count
            moves = GameConstants.MoveCountPerTurn;
        }

        public bool IsTurnOver()
        {
            return moves == 0;
        }

        public void SetMoveSuccess()
        {
            moves = Math.Max(0, moves - 1);
        }

    }
}