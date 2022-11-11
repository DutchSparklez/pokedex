using Java.Lang;
using Newtonsoft.Json;
using pokedex.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace pokedex.Services
{
    /// <summary>
    /// The PokedexService class is used to connect to the Pokedex API,
    /// as well as reading the included JSON file containing the data of all pokemon.
    /// </summary>
    internal static class PokedexService
    {
        // Create a variable that holds the Pokemon Data List
        static List<Pokemon> pokemonData;

        // Create a constant that contains the base API address
        static readonly string ApiAddress = "http://penguin-plaza.nl/pokedex/";

        /// <summary>
        /// This function reads all the Pokémon data from the included JSON file.
        /// It saves all the Pokémon data as Pokémon objects in the pokemonData property.
        /// </summary>
        /// <param name="dataStream">The JSON file included with the application</param>
        /// <returns></returns>
        public static void LoadPokemonData(Stream dataStream)
        {
            // Start reading the file
            using StreamReader reader = new StreamReader(dataStream);

            // Make a single string out of the entire file
            string json = reader.ReadToEnd();

            // Convert the data to a list of Pokémon objects
            pokemonData = JsonConvert.DeserializeObject<List<Pokemon>>(json);
        }

        /// <summary>
        /// Return a single pokémon object.
        /// </summary>
        /// <param name="pokedexNumber">The Pokédex number of the Pokémon to be returned</param>
        /// <returns></returns>
        public static Pokemon GetPokemon(int pokedexNumber) => pokemonData[pokedexNumber - 1];

        /// <summary>
        /// Call the base API address to get a random pokemon number.
        /// </summary>
        /// <returns>An integer represeting the pokedex number of a random pokemone</returns>
        public static Pokemon GetRandomPokemon()
        {
            // Perform a webrequest to the base of the API
            WebRequest request          = WebRequest.Create(ApiAddress);
            using StreamReader reader   = new StreamReader(request.GetResponse().GetResponseStream());

            // Parse the answer as an integer
            return GetPokemon(int.Parse(reader.ReadToEnd()));
        }

        /// <summary>
        /// Get a list of all available Pokémon.
        /// </summary>
        /// <returns>An enumarable containing tuples with the number and name of all Pokémon</returns>
        public static IEnumerable<(string number, string name)> GetPokemonList()
        {
            // Get the number and the name of every pokemon
            return pokemonData.Select(pokemon => (pokemon.Number, pokemon.Name));
        }
    }
}