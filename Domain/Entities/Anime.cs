namespace Domain.Entities
{
    public class Anime
    {
        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public string? Diretor { get; set; }

        public string? Resumo { get; set; }
    }
}
