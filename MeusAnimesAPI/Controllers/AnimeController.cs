using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Domain.Entities;
using Domain.Exceptions;
using MeusAnimesAPI.Models;

namespace MeusAnimesAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnimeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AnimeController> _logger;

    public AnimeController(IMediator mediator, ILogger<AnimeController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get All Animes
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Anime>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllAnimes()
    {
        try
        {
            _logger.LogInformation("Obtendo todos os Animes cadastrados...");
            var result = await _mediator.Send(new GetAllAnimesQuery());
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar obter o Anime.");

            var error = new ErrorModel { Message = ex.Message, StatusCode = 400 };
            return BadRequest(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar recuperar os Animes.");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Get Anime By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int:min(1)}")]
    [ProducesResponseType(typeof(Anime), 200)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAnimeById([FromRoute] int id)
    {
        try
        {
            _logger.LogError("Obtendo o Anime com o id correspondente...");
            var result = await _mediator.Send(new GetAnimeByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar obter o Anime.");

            var error = new ErrorModel { Message = ex.Message, StatusCode = 400 };
            return BadRequest(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar obter o Anime.");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Create Anime
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Anime), 201)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAnime([FromBody] CreateAnimeCommand command)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogError("Adicionando anime à base de dados...");
            var result = await _mediator.Send(command);
            _logger.LogError("Anime adicionado");
            return CreatedAtAction(nameof(GetAnimeById), new { id = result.Id }, result);
        }
        catch (DuplicatedEntityException ex)
        {
            _logger.LogWarning(ex, "Ocorreu um erro ao tentar adicionar o anime.");

            var error = new ErrorModel { Message = "Anime já foi cadastrado.", StatusCode = 400 };
            return BadRequest(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar adicionar o anime.");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Update Anime
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    [HttpPut("{id:int:min(1)}")]
    [ProducesResponseType(typeof(UpdateAnimeCommand), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAnimeCommand command)
    {
        try
        {
            if (id != command.Id) throw new BadRequestException("Os identificadores não são semelhantes.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Atualizando as informações do anime.");
            var result = await _mediator.Send(command);
            if (!result) return NotFound();

            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Anime não encontrado");

            var error = new ErrorModel { Message = "Anime não encontrado", StatusCode = 404 };
            return NotFound(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar atualizar as informações do anime.");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Delete Anime
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(1)}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Deletando o anime do banco de dados...");

            var result = await _mediator.Send(new DeleteAnimeCommand(id));
            if (!result)
            {
                _logger.LogWarning("O anime não pode ser deletado.");

                var error = new ErrorModel { Message = "Anime não encontrado", StatusCode = 404 };
                return NotFound(error);
            }
            _logger.LogInformation("Anime deletado.");

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Anime não encontrado");

            var error = new ErrorModel { Message = "Anime não encontrado", StatusCode = 404 };
            return NotFound(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar atualizar as informações do anime.");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Get Anime By Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(IEnumerable<Anime>), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAnimeByName([FromRoute] string name)
    {
        try
        {
            _logger.LogError("Obtendo o Anime com o nome correspondente...");
            var result = await _mediator.Send(new GetAnimesByNameQuery(name));
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar obter o Anime.");

            var error = new ErrorModel { Message = ex.Message, StatusCode = 400 };
            return BadRequest(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar obter o Anime.");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Get Anime By Name Of Director
    /// </summary>
    /// <param name="nameDirector"></param>
    /// <returns></returns>
    [HttpGet("director/{nameDirector}")]
    [ProducesResponseType(typeof(IEnumerable<Anime>), 200)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAnimeByNameDirector([FromRoute] string nameDirector)
    {
        try
        {
            _logger.LogError("Obtendo o Anime com o nome de diretor correspondente...");
            var result = await _mediator.Send(new GetAnimesByDiretorQuery(nameDirector));
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar obter o Anime.");

            var error = new ErrorModel { Message = ex.Message, StatusCode = 400 };
            return BadRequest(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao tentar obter o Anime.");
            return StatusCode(500);
        }
    }
}