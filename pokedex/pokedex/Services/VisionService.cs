using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace pokedex.Services
{
    /// <summary>
    /// Class to help retrieving data from the service
    /// This is a single classification result
    /// </summary>
    internal class Prediction
    {
        public float Probability { get; set; }
        public string TagId { get; set; }
        public string TagName { get; set; }
    }

    /// <summary>
    /// Class to help retrieving data from the service
    /// This is the main answer for a single prediction
    /// </summary>
    internal class PredictionResult
    {
        public string Id { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public string Created { get; set; }
        public Prediction[] Predictions { get; set; }
    }

    internal static class VisionService
    {
        // Create variables essential for HTTP Requests
        private static readonly Uri requestUri = new Uri("https://pokemonrecognizer-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/95b11e94-9e62-40a2-8ef2-56106535db28/classify/iterations/PokemonRecognizer/image");
        private static readonly string predictionKey = "9a5fa2cfc20446e78c89172a1671a4be";
        private static readonly HttpClient webClient = new HttpClient();

        /// <summary>
        /// Perform a prediction and get the pokemon number to match
        /// </summary>
        /// <param name="imagePath">?</param>
        /// <returns>The number of the pokemon with the closest match, null on error / no result</returns>
        public static async Task<int> GetPrediction(string imagePath)
        {
            // The try/catch allows us to handle errors thrown, we kinda just ignore them
            try
            {
                // Create the content for the HTTP Request
                HttpContent imageContent = new StreamContent(new FileStream(imagePath, FileMode.Open));

                // Add the correct header
                webClient.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                // Prepare for a request that contains a stream of data
                using var formData = new MultipartFormDataContent { imageContent };

                // Perform the request as POST
                HttpResponseMessage response = await webClient.PostAsync(requestUri, formData);

                // Unpack the result and translate it to the Prediction Class
                string content = await response.Content.ReadAsStringAsync();
                PredictionResult result = JsonConvert.DeserializeObject<PredictionResult>(content);

                // Return the tag of the closest match
                return int.Parse(result.Predictions[0].TagName);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                // Something went wrong, don't care what, just continue
                return -1;
            }
        }
    }
}