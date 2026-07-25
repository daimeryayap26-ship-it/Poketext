namespace Poketext.Models
{
    public class Pokemon
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Type { get; set; } = "";

        public ICollection<UserPokemon> UserPokemons { get; set; }
            = new List<UserPokemon>();
    }
}