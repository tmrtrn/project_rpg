using System.Collections.Generic;
using Core.Services.Data;

namespace Models
{
    public class SavedGameModel : ISerializeModel
    {
        /// <summary>
        /// Player have hero collection
        /// </summary>
        public List<SavedHeroModel> heroCollection;

        public HeroTeam playerTeam;
    }
}