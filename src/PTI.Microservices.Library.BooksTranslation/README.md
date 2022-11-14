# PTI.Microservices.Library.BooksTranslation

This is part of PTI.Microservices.Library set of packages

The purpose of this package is to facilitate Books Translation, while maintaining a consistent usage pattern among the different services in the group

**Examples:**

## Translate DocX File From Url
    CustomHttpClient customHttpClient = new CustomHttpClient(new CustomHttpClientHandler(null));
    BooksTranslationService booksTranslationService = new BooksTranslationService(null, this.BooksTranslationConfiguration,
        customHttpClient);
    string testFileUrl = TEST_DOCX_URL;
    var result = await booksTranslationService.TranslateDocXFileFromUrlAsync(testFileUrl, TranslationLanguage.English, TranslationLanguage.Spanish,
            BookTranslationMode.KeepFormatting, emailAddress: TEST_EMAIL_ADDRESS);