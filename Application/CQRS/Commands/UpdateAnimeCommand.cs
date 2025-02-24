using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Commands;

public class UpdateAnimeCommand : IRequest<bool>
{
    [Required(ErrorMessage = "Id é obrigatório")]
    public required int Id { get; set; }

    [Required(ErrorMessage = "Name do anime é obrigatório")]
    [StringLength(255, ErrorMessage = "Nome muito longo")]
    [MinLength(3, ErrorMessage = "Nome muito curto")]
    public required string Nome { get; set; }


    [Required(ErrorMessage = "Nome do diretor do anime é obrigatório")]
    [StringLength(255, ErrorMessage = "Nome muito longo")]
    [MinLength(3, ErrorMessage = "Nome muito curto")]
    public required string Diretor { get; set; }

    [Required(ErrorMessage = "Resumo do anime é obrigatório")]
    [StringLength(2000, ErrorMessage = "SResumo é muito longo")]
    [MinLength(3, ErrorMessage = "Resumo muito curto")]
    public required string Resumo { get; set; }
}