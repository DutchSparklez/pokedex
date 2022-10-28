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
using static Android.Views.GestureDetector;
using System.Runtime.InteropServices;
using AndroidX.Core.View;
using System;
using Android.Widget;

namespace pokedex
{
    /// <summary>
    /// The main activity: the starting point of the application
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        private List<Pokemon> pokemonData = new List<Pokemon>();
        private GestureDetectorCompat gestureDetector;

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

            // Enable Gesture Detection
            gestureDetector = new GestureDetectorCompat(this, new MyGestureListener());
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

            // Save the data of a single pokemon
            Pokemon test = pokemonData[0];

            // Let the app speak out the details of the pokemon
            test.SayDetails();
        }

        // Build in function to manage android permission
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            // Set all the required permission
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        /// <summary>
        /// We need a MyGestureClass because of the way Android handles touch events.
        /// If we didn't use this, we would need to implement every single possible swipe action.
        /// </summary>
        internal class MyGestureListener : SimpleOnGestureListener
        {
            // Create a constant that is used to calculate whether a swipe has been made.
            private readonly int MinimumVelocity = 200;

            /// <summary>
            /// This function checks whether the user has swiped across the screen, then determines the direction of said swipe.
            /// </summary>
            /// <param name="differenceX">The distance between the start and endpoint of the swipe on the X-axis</param>
            /// <param name="differenceY">The distance between the start and endpoint of the swipe on the Y-axis</param>
            /// <param name="velocityX">The speed of the swipe across the X-axis</param>
            /// <param name="velocityY">The speed of the swipe across the Y-axis</param>
            /// <returns>A string with the direction if a swipe has been detected, an empty string otherwise</returns>
            private string GetSwipeDirection(float differenceX, float differenceY, float velocityX, float velocityY)
            {
                // Check whether the motion counts as a horizontal swipe
                if (Math.Abs(differenceX) > Math.Abs(differenceY) && Math.Abs(differenceX) > MinimumVelocity && Math.Abs(velocityX) > MinimumVelocity)
                {
                    // Return the direction of the swipe
                    return differenceX > 0 ? "Right" : "Left";
                }
                
                // Check whether the motion counts as a vertical swipe
                if (Math.Abs(differenceY) > MinimumVelocity && Math.Abs(velocityY) > MinimumVelocity)
                {
                    // Return the direction of the swipe
                    return differenceY > 0 ? "Down" : "Up";
                }

                // No valid swipe was detected, return an empty string
                return "";
            }

            /// <summary>
            /// The function that get's called automatically when the user flings their finger across the screen.
            /// </summary>
            /// <param name="e1">The startingpoint of the swipe</param>
            /// <param name="e2">The endpoint of the swipe</param>
            /// <param name="velocityX">The speed of the swipe across the X-axis</param>
            /// <param name="velocityY">The speed of the swipe across the Y-axis</param>
            /// <returns>True if the event was handled, false it wasn't</returns>
            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                // Get the direction of the swipe, this becomes an empty string if the movement is not a valid swipe.
                string direction = GetSwipeDirection(e2.GetX() - e1.GetX(), e2.GetY() - e1.GetY(), velocityX, velocityY);

                // Determine the action based on the direction
                switch (direction)
                {
                    case "Right": Console.WriteLine(direction); return true;
                    case "Left": Console.WriteLine(direction); return true;
                    case "Up": Console.WriteLine(direction); return true;
                    case "Down": Console.WriteLine(direction); return true;
                    default: return false;
                }
            }
        }
    }
}
