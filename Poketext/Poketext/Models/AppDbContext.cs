using Microsoft.EntityFrameworkCore;

namespace Poketext.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Pokemon> Pokemons { get; set; }

        public DbSet<UserPokemon> UserPokemons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserPokemon>()
                .HasIndex(up => new
                {
                    up.UserId,
                    up.PokemonId
                })
                .IsUnique();

            modelBuilder.Entity<Pokemon>().HasData(
                new Pokemon { Id = 1, Name = "Bulbasaur", Type = "Grass" },
                new Pokemon { Id = 2, Name = "Ivysaur", Type = "Grass" },
                new Pokemon { Id = 3, Name = "Venusaur", Type = "Grass" },
                new Pokemon { Id = 4, Name = "Charmander", Type = "Fire" },
                new Pokemon { Id = 5, Name = "Charmeleon", Type = "Fire" },
                new Pokemon { Id = 6, Name = "Charizard", Type = "Fire" },
                new Pokemon { Id = 7, Name = "Squirtle", Type = "Water" },
                new Pokemon { Id = 8, Name = "Wartortle", Type = "Water" },
                new Pokemon { Id = 9, Name = "Blastoise", Type = "Water" },
                new Pokemon { Id = 10, Name = "Caterpie", Type = "Bug" },
                new Pokemon { Id = 11, Name = "Metapod", Type = "Bug" },
                new Pokemon { Id = 12, Name = "Butterfree", Type = "Bug" },
                new Pokemon { Id = 13, Name = "Pikachu", Type = "Electric" },
                new Pokemon { Id = 14, Name = "Raichu", Type = "Electric" },
                new Pokemon { Id = 15, Name = "Jigglypuff", Type = "Fairy" },
                new Pokemon { Id = 16, Name = "Meowth", Type = "Normal" },
                new Pokemon { Id = 17, Name = "Psyduck", Type = "Water" },
                new Pokemon { Id = 18, Name = "Machop", Type = "Fighting" },
                new Pokemon { Id = 19, Name = "Gengar", Type = "Ghost" },
                new Pokemon { Id = 20, Name = "Eevee", Type = "Normal" }
            );
        }
    }
}