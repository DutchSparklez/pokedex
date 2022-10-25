using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Android.Content.PM;
using Newtonsoft.Json;
using pokedex.Models;
using System.Collections.Generic;
using System.IO;

namespace pokedex
{
    /// <summary>
    /// The main activity: the starting point of the application
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : AppCompatActivity
    {
        private List<Pokemon> pokemonData = new List<Pokemon>();

        /// <summary>
        /// This function creates the main activity and places it on the screen.
        /// </summary>
        /// <param name="savedInstanceState">If you keep variables in your application, this is how they're loaded in after sleep.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Make the app full screen
            Window.AddFlags(WindowManagerFlags.Fullscreen);

            // Get the Pokémon data
            GetPokemonData();

            // Create the activity
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Display the activity on screen
            SetContentView(Resource.Layout.activity_main);
        }

        /// <summary>
        /// This function reads all the Pokémon data from the included JSON file.
        /// It saves all the Pokémon data as Pokémon objects in the pokemonData property.
        /// </summary>
        public void GetPokemonData()
        {
            // Create a data stream which reads from the included json file
            Stream dataStream = Assets.Open("pokemon.json");

            // Start reading the file
            using StreamReader reader = new StreamReader(dataStream);

            // Make a single string out of the entire file
            string json = reader.ReadToEnd();

            // Convert the data to a list of Pokémon objects
            pokemonData = JsonConvert.DeserializeObject<List<Pokemon>>(json);
        }

        // Build in function to manage android permission
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            // Set all the required permission
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
