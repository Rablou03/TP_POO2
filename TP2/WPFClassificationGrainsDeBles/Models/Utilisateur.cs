using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WPFClassificationGrainsDeBles.Models
{
    public class Utilisateur
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

       
        public string NomComplet => $"{FirstName} {LastName}";
    }

    public class ReponseUtilisateurs
    {
        [JsonPropertyName("users")]
        public List<Utilisateur> Users { get; set; }
    }
}