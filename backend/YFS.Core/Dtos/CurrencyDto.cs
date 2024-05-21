namespace YFS.Core.Dtos
{
    public class CurrencyDto
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; } = null!;
        public int Number { get; set; }
        public string Country { get; set; } = null!;
        public string? ShortNameUs { get; set; }
        public string? Name_ru { get; set; }
        public string? Name_ua { get; set; }
        public string? Name_en { get; set; }
        public string Symbol { get; set; } = null!;
    }
}

