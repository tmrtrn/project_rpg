using Core.Services.Data;

namespace Data.Hero
{
    public interface IHeroAsset : IGameAsset
    {
        public HeroAttributeAsset Attributes { get; }
        public HeroViewAsset ViewAsset { get; }
    }
}