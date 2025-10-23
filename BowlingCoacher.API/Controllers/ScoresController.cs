using BowlingCoacher.API.Interfaces;
using BowlingCoacher.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
namespace BowlingCoacher.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoresController (IGameStatisticsRepository repository): ControllerBase {
    private readonly IGameStatisticsRepository _repository = repository;

    [HttpGet]
    public IActionResult GetAll() => Ok(_repository.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById (int id){
        var stat = _repository.GetById(id);
        return stat is null ? NotFound() : Ok(stat);
    }

    [HttpPost]
    public IActionResult Create (GameStatistics stats){
        var created = _repository.Add(stats);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update (int id, GameStatistics stats){
        var success = _repository.Update(id, stats);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete (int id){
        var success = _repository.Delete(id);
        return success ? NoContent() : NotFound();
    }
}