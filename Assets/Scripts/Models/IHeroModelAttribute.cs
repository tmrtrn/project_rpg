namespace Models
{
    public interface IHeroModelAttribute
    {
        public string Id { get; }
        public string Name { get; }
        public int Level { get; }
        public float FullHealth { get; }
        public float FullAttack { get; }
        public int Experience { get; }
    }
}