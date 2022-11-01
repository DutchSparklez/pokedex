using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using pokedex.Models;
using pokedex.Services;
using Xamarin.Essentials;
using Android.Content;

namespace pokedex
{
    /// <summary>
    /// The main activity: the starting point of the application.
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        // The app needs an object that detects gestures
        private GestureDetectorCompat gestureDetector;

        // Create variables that store android views
        private TextView pokemonNumber;
        private ImageView pokemonImage;

        /// <summary>
        /// This function creates the main activity and places it on the screen.
        /// </summary>
        /// <param name="savedInstanceState">If you keep variables in your application, this is how they're loaded in after sleep</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Make the app full screen
            Window.AddFlags(WindowManagerFlags.Fullscreen);

            // Create the activity
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            // Display the activity on screen
            SetContentView(Resource.Layout.activity_main);

            // Enable Gesture Detection
            gestureDetector = new GestureDetectorCompat(this, new SwipeService { mainActivity = this });

            // Initialize the TTS engine to reduce waiting times
            TextToSpeech.SpeakAsync("");

            // Get the interface components
            pokemonNumber = FindViewById<TextView>(Resource.Id.pokemonNumber);
            pokemonImage = FindViewById<ImageView>(Resource.Id.pokemonImage);

            // Load the Pokémon data; this has to be last to have everything loaded
            PokedexService.LoadPokemonData(Assets.Open("pokemon.json"));
        }

        /// <summary>
        /// This function loads a pokemon into the activity. The pokemon is first set as active,
        /// after which all data will be loaded into their individual views.
        /// This function also activates the TTS of the pokemon.
        /// </summary>
        /// <param name="pokemon">The Pokemon object to load, randomized if null</param>
        internal void LoadPokemonData(Pokemon pokemon)
        {
            // If the pokemon object is null, get a random one
            pokemon ??= PokedexService.GetRandomPokemon();

            // Load the interface elements
            pokemonNumber.Text = pokemon.Number;
            pokemonImage.SetImageBitmap(pokemon.GetImage());

            // Let the app speak out the details of the pokemon
            pokemon.SayDetails();
        }

        /// <summary>
        /// Adds the List Activity to the activity stack.
        /// </summary>
        /// <param name="filteredNumbers">If this array contains pokedex numbers, those are the only ones to show</param>
        public void NavigateToPokemonList(string[] filteredNumbers)
        {
            // Create a bundle to hold the filtered numbers
            Bundle bundle = new Bundle();
            bundle.PutStringArray("filteredNumbers", filteredNumbers);

            // Create an intent and add the bundle
            Intent listActivity = new Intent(this, typeof(ListActivity));
            listActivity.PutExtras(bundle);

            // Start the activity
            StartActivity(listActivity);
        }

        /// <summary>
        /// This function directs touch events to the MyGestureListener class.
        /// </summary>
        /// <param name="e">A motion event, contains information on the interaction with the screen</param>
        /// <returns>A boolean that tells whether a touch event has been registered and handled</returns>
        public override bool OnTouchEvent(MotionEvent e)
        {
            // Direct the touch event to MyGestureListeren
            gestureDetector.OnTouchEvent(e);
            return base.OnTouchEvent(e);
        }

        // Build in function to manage android permission
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            // Set all the required permission
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
