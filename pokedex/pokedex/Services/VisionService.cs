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
        /// <param name="image">?</param>
        /// <returns>The number of the pokemon with the closest match, null on error / no result</returns>
        public static async Task<string> GetPrediction(string image)
        {
            try
            {
                // Create the content for the HTTP Request (POST)
                HttpContent imageContent = new StreamContent(new FileStream(image, FileMode.Open));
                imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                imageContent.Headers.Add("Prediction-Key", predictionKey);
                MultipartFormDataContent formDataContent = new MultipartFormDataContent { imageContent };

                // Perform the request
                HttpResponseMessage response = await webClient.PostAsync(requestUri, formDataContent);
                string content = await response.Content.ReadAsStringAsync();
                PredictionResult result = JsonConvert.DeserializeObject<PredictionResult>(content);

                // Return the tag of the closest match
                return result.Predictions[0].TagName;
            }

            catch
            {
                // Something went wrong, don't care what, just continue
                return null;
            }
        }
    }
}