using Android.Views;
using System;
using static Android.Views.GestureDetector;

namespace pokedex.Services
{
    /// <summary>
    /// We need a SwipeService because of the way Android handles touch events.
    /// If we didn't use this, we would need to implement every single possible swipe action.
    /// </summary>
    internal class SwipeService : SimpleOnGestureListener
    {
        // Create a constant that holds the main activity
        public MainActivity mainActivity;

        // Create a constant that is used to calculate whether a swipe has been made
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
            // Get the direction of the swipe, this becomes an empty string if the movement is not a valid swipe
            string direction = GetSwipeDirection(e2.GetX() - e1.GetX(), e2.GetY() - e1.GetY(), velocityX, velocityY);

            // Determine the action based on the direction
            switch (direction)
            {
                case "Right":   Console.WriteLine(direction);               return true;    // Show camera
                case "Left":    mainActivity.NavigateToPokemonList();       return true;    // Show pokemon list
                case "Up":      Console.WriteLine(direction);               return true;    // Show pokemon details
                case "Down":    mainActivity.LoadPokemonData(null);         return true;    // Get random pokemon
                default:                                                    return false;
            }
        }
    }
}