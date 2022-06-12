using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class HeroTeam
    {
        private HeroTeamMember[] Members;

        public HeroTeam(int capacity)
        {
            // create a list with null members
            Members = new HeroTeamMember[capacity];
        }

        public bool IsMember(string heroId)
        {
            foreach (HeroTeamMember member in Members)
            {
                if (member == null) continue;
                if (member.id.Equals(heroId)) return true;
            }

            return false;
        }

        public bool IsFull()
        {
            if (Members.Length == 0) throw new Exception("Member size must be greater than 0");
            foreach (HeroTeamMember member in Members)
            {
                if (member == null) return false;
            }

            return true;
        }

        public bool AddHeroToTeam(string id)
        {
            if (IsMember(id))
            {
                // hero is already member
                return false;
            }

            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i] == null)
                {
                    Members[i] = new HeroTeamMember(id);
                    return true;
                }
            }

            // capacity is full
            return false;
        }

        public bool RemoveFromTeam(string id)
        {
            if (!IsMember(id))
            {
                return false; //hero is not in the list
            }

            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i] != null && Members[i].id.Equals(id))
                {
                    Members[i] = null;
                    return true;
                }
            }

            throw new Exception("player must be in the list");
        }
    }
}