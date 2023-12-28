﻿namespace ControlDoc.Models.Models.Components.Language.Response
{
    public class PhraseDtoResponse
    {
        public int PhraseId { get; set; }
        public int LanguageId { get; set; }
        public int KeyPhraseId { get; set; }
        public string TextPhrase { get; set; } = null!;
        public KeyPhraseDtoResponse KeyPhrase { get; set; } = null!;
        public LanguageDtoResponse Language { get; set; } = null!;
    }
}
