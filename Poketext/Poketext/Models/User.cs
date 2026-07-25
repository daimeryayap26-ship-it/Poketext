namespace Poketext.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public int CapturedPokemon { get; set; }

        public ICollection<UserPokemon> UserPokemons { get; set; }
            = new List<UserPokemon>();
    }
}