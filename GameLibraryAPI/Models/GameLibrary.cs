using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GameLibraryAPI.Models;

public class GameLibrary
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? Developer { get; set; }
    
    [Required]
    public List<string?> Genres
    {
        get
        {
            List<string?> tempGenres = new List<string?>();
            foreach (var g in GenresId)
            {
                tempGenres.Add(g.ToString());
            }
            return tempGenres;
        }
        set
        {
            GenresId.Clear();
            foreach (var g in value)
            {
                GenresId.Add((Genre)Enum.Parse(typeof(Genre), g));
            }
        }
    }

    [NotMapped] private List<Genre> GenresId { get; set; } = new();
}

public enum Genre
{
    Fantasy = 0,
    Adventure = 1,
    Horror = 2,
    Sport = 3,
    RPG = 4
}