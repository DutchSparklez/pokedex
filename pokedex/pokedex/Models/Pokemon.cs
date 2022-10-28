using Android.Content;
using Java.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace pokedex.Models
{
    /// <summary>
    /// The object template that hold the data of a single pokemon.
    /// This class is used when converting JSON objects to C# objects.
    /// </summary>
    class Pokemon
    {
        // The data of the Pokemon
        public string Number;
        public string Name;
        public string Description;
        public string Category;
        public string ImageUrl;
        public string Length;
        public string Weight;
        public string[] Abilities;
        public string[] Typing;

        /// <summary>
        /// This function uses the Text To Speech engine of the phone to say a subset of the Pokemon's data.
        /// This function fails if Text To Speech hasn't been settup at the phone.
        /// </summary>
        public async void SayDetails()
        {
            // Create the string that will be spoken and say it
            await TextToSpeech.SpeakAsync($"{Name}. {GetCategoryString()}. {GetTypingString()}. {Description}");
        }

        /// <summary>
        /// This function turns the category of the Pokemon into the desired format for display and announcing.
        /// </summary>
        /// <returns>A string in the following format: [The {Category} Pokémon]</returns>
        public string GetCategoryString()
        {
            // Return the string
            return $"The {Category} Pokémon";
        }

        /// <summary>
        /// This function turns the typing of the Pokemon into the desired format for displaying and announcing.
        /// It forms a grammatically correct sentence.
        /// </summary>
        /// <returns>A string in the following format: [A(n) {Type1} (and {Type2}) type]</returns>
        public string GetTypingString()
        {
            // Create a list of vowels, used to check whether we need A or An at the start of our output
            char[] vowels = { 'a', 'e', 'o', 'u', 'i' };

            // Start the output
            string output = "A";

            // Determine whether we need to start the output with A or An
            output += vowels.Contains(Typing[0].ToLower().First()) ? "n " : " ";

            // Add the types to the sentence
            output += Typing.Length == 2 ? $"{Typing[0]} and {Typing[1]}" : $"{Typing[0]}";

            // End the sentence and return the result
            return output + " type";
        }
    }
}