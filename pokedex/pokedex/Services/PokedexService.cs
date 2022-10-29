using Newtonsoft.Json;
using Android.Content.PM;
using pokedex.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace pokedex.Services
{
    /// <summary>
    /// The PokedexService class is used to connect to the Pokedex API,
    /// as well as reading the included JSON file containing the data of all pokemon.
    /// </summary>
    internal static class PokedexService
    {
        // Create a constant that contains the base API address
        static readonly string ApiAddress = "http://penguin-plaza.nl/pokedex/";

        /// <summary>
        /// This function reads all the Pokémon data from the included JSON file.
        /// It saves all the Pokémon data as Pokémon objects in the pokemonData property.
        /// </summary>
        /// <param name="dataStream">The JSON file included with the application</param>
        /// <returns></returns>
        public static List<Pokemon> LoadPokemonData(Stream dataStream)
        {
            // Start reading the file
            using StreamReader reader = new StreamReader(dataStream);

            // Make a single string out of the entire file
            string json = reader.ReadToEnd();

            // Convert the data to a list of Pokémon objects
            return JsonConvert.DeserializeObject<List<Pokemon>>(json);
        }

        /// <summary>
        /// Call the base API address to get a random pokemon number.
        /// </summary>
        /// <returns>An integer represeting the pokedex number of a random pokemone</returns>
        public static int GetRandomPokemon()
        {
            // Perform a webrequest to the base of the API
            WebRequest request          = WebRequest.Create(ApiAddress);
            using StreamReader reader   = new StreamReader(request.GetResponse().GetResponseStream());

            // Parse the answer as an integer
            return int.Parse(reader.ReadToEnd());
        }
    }
}