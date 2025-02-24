
using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using MeusAnimesAPI.Controllers;
using MeusAnimesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MeusAnimesAPI.Tests.Controllers;

public class AnimeControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<AnimeController>> _loggerMock;
    private readonly AnimeController _controller;

    public AnimeControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<AnimeController>>();
        _controller = new AnimeController(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllAnimes_ShouldReturnOkWithAnimes()
    {
        // Arrange
        var mockAnimes = new List<Anime>
            {
                new Anime { Id = 1, Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um anime sobre ninjas." },
                new Anime { Id = 2, Nome = "Attack on Titan", Diretor = "Tetsurō Araki", Resumo = "Um dos animes já feitos." }
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllAnimesQuery>(), default))
            .ReturnsAsync(mockAnimes);

        // Act
        var result = await _controller.GetAllAnimes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAnimes = Assert.IsAssignableFrom<IEnumerable<Anime>>(okResult.Value);
        Assert.Equal(mockAnimes.Count, returnedAnimes.ToList().Count);
    }

    [Fact]
    public async Task GetAllAnimes_WhenExceptionOccurs_ShouldReturn500()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllAnimesQuery>(), default))
            .ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var result = await _controller.GetAllAnimes();

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetAnimeById_ShouldReturnOkWithAnime()
    {
        // Arrange
        var mockAnime = new Anime { Id = 1, Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um anime sobre ninjas." };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetAnimeByIdQuery>(q => q.Id == 1), default))
            .ReturnsAsync(mockAnime);

        // Act
        var result = await _controller.GetAnimeById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedAnime = Assert.IsType<Anime>(okResult.Value);
        Assert.Equal(mockAnime.Id, returnedAnime.Id);
        Assert.Equal(mockAnime.Nome, returnedAnime.Nome);
    }

    [Fact]
    public async Task GetAnimeById_WhenAnimeNotFound_ShouldReturnNotFound()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.Is<GetAnimeByIdQuery>(q => q.Id == 1), default))
            .ReturnsAsync((Anime)null); // Simula a ausência do anime.

        // Act
        var result = await _controller.GetAnimeById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAnimeById_WhenNotFoundExceptionOccurs_ShouldReturnBadRequest()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimeByIdQuery>(), default))
            .ThrowsAsync(new NotFoundException("Anime não encontrado"));

        // Act
        var result = await _controller.GetAnimeById(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorModel = Assert.IsType<ErrorModel>(badRequestResult.Value);
        Assert.Equal("Anime não encontrado", errorModel.Message);
        Assert.Equal(400, errorModel.StatusCode);
    }

    [Fact]
    public async Task GetAnimeById_WhenUnexpectedExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimeByIdQuery>(), default))
            .ThrowsAsync(new System.Exception("Erro inesperado"));

        // Act
        var result = await _controller.GetAnimeById(1);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task CreateAnime_ShouldReturnCreatedAtActionWithAnime()
    {
        // Arrange
        var command = new CreateAnimeCommand { Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um dos animes já feito" };
        var mockAnime = new Anime { Id = 1, Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um dos animes já feito" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateAnimeCommand>(), default))
            .ReturnsAsync(mockAnime);

        // Act
        var result = await _controller.CreateAnime(command);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedAnime = Assert.IsType<Anime>(createdResult.Value);

        Assert.Equal(nameof(AnimeController.GetAnimeById), createdResult.ActionName);
        Assert.Equal(mockAnime.Id, ((Anime)returnedAnime).Id);
        Assert.Equal(mockAnime.Nome, ((Anime)returnedAnime).Nome);
    }

    [Fact]
    public async Task CreateAnime_WhenModelStateIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório");

        var command = new CreateAnimeCommand { Nome = "", Diretor = "Hayato Date", Resumo = "Um dos animes já feito" };

        // Act
        var result = await _controller.CreateAnime(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task CreateAnime_WhenDuplicatedEntityExceptionOccurs_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new CreateAnimeCommand { Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um dos animes já feito" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateAnimeCommand>(), default))
            .ThrowsAsync(new DuplicatedEntityException("Anime já foi cadastrado."));

        // Act
        var result = await _controller.CreateAnime(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorModel = Assert.IsType<ErrorModel>(badRequestResult.Value);
        Assert.Equal("Anime já foi cadastrado.", errorModel.Message);
        Assert.Equal(400, errorModel.StatusCode);
    }

    [Fact]
    public async Task CreateAnime_WhenUnexpectedExceptionOccurs_ShouldReturnInternalServerError()
    {
        // Arrange
        var command = new CreateAnimeCommand { Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um dos animes já feito" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateAnimeCommand>(), default))
            .ThrowsAsync(new System.Exception("Erro inesperado"));

        // Act
        var result = await _controller.CreateAnime(command);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnOkWhenUpdateIsSuccessful()
    {
        // Arrange
        var id = 1;
        var command = new UpdateAnimeCommand
        {
            Id = id,
            Nome = "Naruto Shippuden",
            Diretor = "Hayato Date",
            Resumo = "Continuação da jornada do jovem ninja."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateAnimeCommand>(), default))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequestWhenModelStateIsInvalid()
    {
        // Arrange
        var id = 1;
        _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório");

        var command = new UpdateAnimeCommand
        {
            Id = id,
            Nome = "",
            Diretor = "Hayato Date",
            Resumo = "Resumo inválido."
        };

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task Update_ShouldReturnInternalServerErrorWhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var id = 1;
        var command = new UpdateAnimeCommand
        {
            Id = id,
            Nome = "Naruto Shippuden",
            Diretor = "Hayato Date",
            Resumo = "Continuação da jornada do jovem ninja."
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateAnimeCommand>(), default))
            .ThrowsAsync(new Exception("Erro inesperado"));

        // Act
        var result = await _controller.Update(id, command);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContentWhenDeletionIsSuccessful()
    {
        // Arrange
        var id = 1;
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteAnimeCommand>(), default))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundWhenAnimeDoesNotExist()
    {
        // Arrange
        var id = 1;
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteAnimeCommand>(), default))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorModel = Assert.IsType<ErrorModel>(notFoundResult.Value);
        Assert.Equal(404, errorModel.StatusCode);
        Assert.Equal("Anime não encontrado", errorModel.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundWhenNotFoundExceptionIsThrown()
    {
        // Arrange
        var id = 1;
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteAnimeCommand>(), default))
            .ThrowsAsync(new NotFoundException("Anime não encontrado"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorModel = Assert.IsType<ErrorModel>(notFoundResult.Value);
        Assert.Equal(404, errorModel.StatusCode);
        Assert.Equal("Anime não encontrado", errorModel.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnInternalServerErrorWhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var id = 1;
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteAnimeCommand>(), default))
            .ThrowsAsync(new Exception("Erro inesperado"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetAnimeByName_ShouldReturnOkWithResultWhenAnimeIsFound()
    {
        // Arrange
        var name = "Naruto";
        var expectedResult = new List<Anime>
            {
                new Anime { Id = 1, Nome = "Naruto", Diretor = "Hayato Date", Resumo = "Um anime sobre ninjas." }
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByNameQuery>(), default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAnimeByName(name);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<List<Anime>>(okResult.Value);
        Assert.Single(actualResult);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task GetAnimeByName_ShouldReturnNotFoundWhenResultIsNull()
    {
        // Arrange
        var name = "NonExistentAnime";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByNameQuery>(), default))
            .ReturnsAsync((List<Anime>)null);

        // Act
        var result = await _controller.GetAnimeByName(name);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAnimeByName_ShouldReturnBadRequestWhenNotFoundExceptionIsThrown()
    {
        // Arrange
        var name = "InvalidAnime";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByNameQuery>(), default))
            .ThrowsAsync(new NotFoundException("Anime não encontrado"));

        // Act
        var result = await _controller.GetAnimeByName(name);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorModel = Assert.IsType<ErrorModel>(badRequestResult.Value);
        Assert.Equal(400, errorModel.StatusCode);
        Assert.Equal("Anime não encontrado", errorModel.Message);
    }

    [Fact]
    public async Task GetAnimeByName_ShouldReturnInternalServerErrorWhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var name = "AnyAnime";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByNameQuery>(), default))
            .ThrowsAsync(new Exception("Erro inesperado"));

        // Act
        var result = await _controller.GetAnimeByName(name);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetAnimeByNameDirector_ShouldReturnOkWithResultWhenAnimesAreFound()
    {
        // Arrange
        var nameDirector = "Hayao Miyazaki";
        var expectedResult = new List<Anime>
            {
                new Anime { Id = 1, Nome = "Spirited Away", Diretor = "Hayao Miyazaki", Resumo = "Uma garota entra em um mundo mágico." },
                new Anime { Id = 2, Nome = "My Neighbor Totoro", Diretor = "Hayao Miyazaki", Resumo = "A história de duas irmãs e um espírito amigável." }
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByDiretorQuery>(), default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAnimeByNameDirector(nameDirector);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<List<Anime>>(okResult.Value);
        Assert.Equal(expectedResult.Count, actualResult.Count);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task GetAnimeByNameDirector_ShouldReturnNotFoundWhenResultIsNull()
    {
        // Arrange
        var nameDirector = "NonExistentDirector";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByDiretorQuery>(), default))
            .ReturnsAsync((List<Anime>)null);

        // Act
        var result = await _controller.GetAnimeByNameDirector(nameDirector);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAnimeByNameDirector_ShouldReturnBadRequestWhenNotFoundExceptionIsThrown()
    {
        // Arrange
        var nameDirector = "InvalidDirector";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByDiretorQuery>(), default))
            .ThrowsAsync(new NotFoundException("Diretor não encontrado"));

        // Act
        var result = await _controller.GetAnimeByNameDirector(nameDirector);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorModel = Assert.IsType<ErrorModel>(badRequestResult.Value);
        Assert.Equal(400, errorModel.StatusCode);
        Assert.Equal("Diretor não encontrado", errorModel.Message);
    }

    [Fact]
    public async Task GetAnimeByNameDirector_ShouldReturnInternalServerErrorWhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var nameDirector = "AnyDirector";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAnimesByDiretorQuery>(), default))
            .ThrowsAsync(new Exception("Erro inesperado"));

        // Act
        var result = await _controller.GetAnimeByNameDirector(nameDirector);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }


}