using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class AccountGroupDto
    {
        public int AccountGroupId { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int GroupOrderBy { get; set; }
        public ICollection<AccountGroupTranslationDto> Translations { get; set; } = new List<AccountGroupTranslationDto>();
    }
}
