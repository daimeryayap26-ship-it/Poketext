namespace Poketext.Models
{
    public class UserPokemon
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public int PokemonId { get; set; }

        public Pokemon? Pokemon { get; set; }
    }
}