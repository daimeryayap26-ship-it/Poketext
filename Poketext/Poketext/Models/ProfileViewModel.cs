namespace Poketext.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; } = new User();

        public List<Pokemon> Pokemons { get; set; }
            = new List<Pokemon>();

        public List<int> CapturedPokemonIds { get; set; }
            = new List<int>();
    }
}