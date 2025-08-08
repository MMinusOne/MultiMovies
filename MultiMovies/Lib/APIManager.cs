using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MultiMovies.Lib
{
    public class APIManager
    {
        static APIManager _instance;
        public static APIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new APIManager();
                }
                return _instance;
            }
        }

        HttpClient client;

        private string embedMovieBaseURL = "http://localhost:3500";
        private string m3u8ServerBaseURL = "";

        public APIManager()
        {
            _instance = this;
            client = new HttpClient();
        }

        public async Task<TMDBSearchResult[]> GetPopular(string id, int page = 1)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{embedMovieBaseURL}/get-popular?page={page}");

                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();

                TMDBSearchResult[] mostPopular = JsonConvert.DeserializeObject<TMDBSearchResult[]>(responseString);

                return mostPopular;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        public async Task<TMDBMovieResponse> GetMovieDetails(string id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{embedMovieBaseURL}/movie-details?query={id}");

                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();

                TMDBMovieResponse movieDetailsData = JsonConvert.DeserializeObject<TMDBMovieResponse>(responseString);

                return movieDetailsData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        public async Task<TMDBSearchResult[]> SearchMovies(String query, int page = 1)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{embedMovieBaseURL}/search-movie?query={query}&page={page}");

                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();

                TMDBSearchResult[] searchResultData = JsonConvert.DeserializeObject<TMDBSearchResult[]>(responseString);

                return searchResultData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<TMDBEmbedMovieResult> GetStreamingUrls(string id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{embedMovieBaseURL}/movie/{id}");

                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();

                TMDBEmbedMovieResult movieDetailsData = JsonConvert.DeserializeObject<TMDBEmbedMovieResult>(responseString);

                return movieDetailsData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

    }
}
