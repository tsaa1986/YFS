namespace YFS.Core.Dtos
{
    public class ApiTokenDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? TokenType { get; set; }
        public string TokenValue { get; set; } = null!;
        public string? Url { get; set; }
        public string? Note { get; set; }
    }
}
