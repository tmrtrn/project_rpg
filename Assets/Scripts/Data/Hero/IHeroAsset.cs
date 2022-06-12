using Core.Services.Data;

namespace Data.Hero
{
    public interface IHeroAsset : IGameAsset
    {
        public string Id { get; }
        public HeroAttributeAsset Attributes { get; }
        public HeroViewAsset ViewAsset { get; }
    }
}