using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using pokedex.Services;
using System.Collections.Generic;
using System.Linq;

namespace pokedex
{
    /// <summary>
    /// The ListActivity class represent the activity where a Pokémon can be chosen from a list
    /// </summary>
    [Activity(Label = "ListActivity", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class ListActivity : Activity
    {
        // Get the list view from the view
        ListView pokemonList;

        /// <summary>
        /// The function that runs when the activity starts
        /// </summary>
        /// <param name="savedInstanceState">Build in parameter for reloading the activity</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Create an instance of the class
            base.OnCreate(savedInstanceState);

            // Filter the Pokemon Data
            IEnumerable<(string number, string name)> pokemon = PokedexService.GetPokemonList();
            string[] pokemonNames = pokemon.Select(mon => mon.number + ' ' + mon.name).ToArray();      

            // Show the view
            SetContentView(Resource.Layout.pokemon_list);

            // Load the data
            pokemonList = FindViewById<ListView>(Resource.Id.pokemonlist);
            pokemonList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, pokemonNames);

            // Create a click event
            pokemonList.ItemClick += (s, eventTrigger) => {
                // Get the selected pokemon
                (string number, string name) = pokemon.ElementAt(eventTrigger.Position);

                // Save the required data
                Intent intent = new Intent();
                intent.PutExtra("number", number);

                // Set the result and end the activity
                SetResult(Result.Ok, intent);
                Finish();
            };
        }
    }
}