using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using GameLibraryAPI.Data;
using GameLibraryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

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
        _dbContext.Games.Add(game);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = game.Id }, game);
    }

    /*[HttpGet]
    public IActionResult Get(List<String> Genres)
    {
        if (_dbContext.Games == null)
            return NotFound(new {HttpResponseMessage = "Game Library is Empty"});
        return Ok(_dbContext.Games);
    }*/
    
    [HttpGet]
    public IActionResult Get()
    {
        if (_dbContext.Games == null)
            return NotFound(new {HttpResponseMessage = "Game Library is Empty"});
        return Ok(_dbContext.Games);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        if (_dbContext.Games == null)
            return NotFound();
        var game = _dbContext.Games.Find(id);
        if (game == null)
            return NotFound();
        return Ok(game);
    }

    [HttpPut]
    public IActionResult Put(GameLibrary game)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
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
    public IActionResult DeleteAll()
    {
        foreach (var g in _dbContext.Games)
        {
            _dbContext.Games.Remove(g);
        }

        _dbContext.SaveChanges();
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (_dbContext.Games == null)
            return NotFound();

        var game = _dbContext.Games.Find(id);
        if (game == null)
        {
            return NotFound();
        }

        _dbContext.Games.Remove(game);
        _dbContext.SaveChanges();
        return Ok(new { HttpResponseMessage = "Deleted successfully"});
    }

}