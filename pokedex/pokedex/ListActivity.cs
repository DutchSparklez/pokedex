using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using pokedex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pokedex
{
    [Activity(Label = "ListActivity")]
    public class ListActivity : Activity
    {
        private List<Pokemon> pokemonData;
        private string[] filteredNumbers = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Create an instance of the class
            base.OnCreate(savedInstanceState);

            // Filter the Pokemon Data
            List<Pokemon> filteredPokemon = pokemonData
                .Where((Pokemon pokemon) => filteredNumbers == null || filteredNumbers.Contains(pokemon.Number))
                .ToList();

            // Load the data

            // Show the view
            SetContentView(Resource.Layout.pokemon_list);
        }
    }
}