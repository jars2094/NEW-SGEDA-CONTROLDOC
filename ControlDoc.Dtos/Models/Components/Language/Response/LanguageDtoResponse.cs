namespace ControlDoc.Models.Models.Components.Language.Response
{
    public class LanguageDtoResponse
    {
        public int LanguageId { get; set; }
        public string CodeLanguage { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string NameTraslated { get; set; } = null!;
    }
}
