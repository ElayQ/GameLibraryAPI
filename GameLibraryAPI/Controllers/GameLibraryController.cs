using GameLibraryAPI.Data;
using GameLibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GameLibraryAPI.Controllers;

[Route("api/[controller]")]
public class GameLibraryController : Controller
{
    private readonly GameLibraryContext _dbContext;

    public GameLibraryController(GameLibraryContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public IActionResult Post(GameLibrary game)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (game.Genres.IsNullOrEmpty()) 
            return BadRequest(new {HttpResponseMessage = "Incorrect genre input" });
        _dbContext.Games.Add(game);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = game.Id }, game);
    }
    
    [HttpGet]
    public IActionResult Get(string[] genresFilter)
    {
        if (_dbContext.Games.IsNullOrEmpty())
            return NotFound(new {HttpResponseMessage = "Game Library is Empty"});
        if (genresFilter.IsNullOrEmpty()) return Ok(_dbContext.Games);
        int count = 0;
        //string[] genres = genresFilter[0].Split(',');
        List<GameLibrary> games = new List<GameLibrary>();
        foreach (var game in _dbContext.Games)
        {
            if (genresFilter.Length == 1)
            {
                if (game.Genres.Contains(genresFilter[0])) games.Add(game);
            }
            else foreach (var genre in genresFilter)
            {
                if (game.Genres.Contains(genre)) count++;
                if (count == genresFilter.Length) games.Add(game);
            }
            count = 0;
        }
        
        return Ok(games);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        if (_dbContext.Games.IsNullOrEmpty())
            return NotFound(new {HttpResponseMessage = "Game Library is Empty"});
        var game = _dbContext.Games.Find(id);
        if (game == null)
            return NotFound(new {HttpResponseMessage = "Game with input ID not found"});
        return Ok(game);
    }

    [HttpPut]
    public IActionResult Put(GameLibrary game)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (game.Genres.IsNullOrEmpty()) 
            return BadRequest(new {HttpResponseMessage = "Incorrect genre input" });
        var existingGame = _dbContext.Games.SingleOrDefault(g => g.Id == game.Id);
        if (existingGame == null) return NotFound();
        existingGame.Name = game.Name;
        existingGame.Developer = game.Developer;
        existingGame.Genres = game.Genres;
        _dbContext.Games.Attach(existingGame);
        _dbContext.SaveChanges();
        return Ok(existingGame);
    }
    
    [HttpDelete]
    public IActionResult Delete()
    {
        if (_dbContext.Games.IsNullOrEmpty())
            return NotFound(new {HttpResponseMessage = "Game Library is Empty"});
        
        foreach (var g in _dbContext.Games)
        {
            _dbContext.Games.Remove(g);
        }

        _dbContext.SaveChanges();
        return Ok(new { HttpResponseMessage = "Deleted successfully"});
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (_dbContext.Games.IsNullOrEmpty())
            return NotFound(new {HttpResponseMessage = "Game Library is Empty"});

        var game = _dbContext.Games.Find(id);
        if (game == null)
        {
            return NotFound(new {HttpResponseMessage = "Game with input ID not found"});
        }

        _dbContext.Games.Remove(game);
        _dbContext.SaveChanges();
        return Ok(new { HttpResponseMessage = "Deleted successfully"});
    }

}