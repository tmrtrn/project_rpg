using System;
using System.Collections;
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

        public int Count()
        {
            return Members.Length;
        }

        public IEnumerator GetEnumerator()
        {
            return Members.GetEnumerator();
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

    }
}