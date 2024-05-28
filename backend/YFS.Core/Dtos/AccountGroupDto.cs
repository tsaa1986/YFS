namespace YFS.Core.Dtos
{
    public class AccountGroupDto
    {
        public int AccountGroupId { get; set; }
        public string UserId { get; set; } = null!;
        public ICollection<AccountGroupTranslationDto> Translations { get; set; } = new List<AccountGroupTranslationDto>();
        public int GroupOrderBy { get; set; }
    }
}
