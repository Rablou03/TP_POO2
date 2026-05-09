using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WPFClassificationGrainsDeBles.Models;

namespace WPFClassificationGrainsDeBles.Services
{
    public class UtilisateurService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string URL = "https://dummyjson.com/users";

        public async Task<List<Utilisateur>> GetUtilisateursAsync()
        {
            try
            {
                string reponse = await _httpClient.GetStringAsync(URL);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var resultat = JsonSerializer.Deserialize<ReponseUtilisateurs>(reponse, options);
                return resultat?.Users ?? new List<Utilisateur>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'appel API : {ex.Message}");
            }
        }
    }
}