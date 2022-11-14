using Microsoft.Extensions.Logging;
using PTI.Microservices.Library.Configuration;
using PTI.Microservices.Library.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTI.Microservices.Library.Services.Specialized
{
    /// <summary>
    /// Translation Language
    /// </summary>
    public enum TranslationLanguage
    {
        /// <summary>
        /// Ennglish Language
        /// </summary>
        English = 0,
        /// <summary>
        /// Spanish Language
        /// </summary>
        Spanish = 1
    }

    /// <summary>
    /// Translation Mode
    /// </summary>
    public enum BookTranslationMode
    {
        /// <summary>
        /// Keeps the document formatting. Reduces translation accuracy
        /// </summary>
        KeepFormatting
    }
    /// <summary>
    /// Allows you to easily retriev data from Customer Finder.
    /// In order to the a Key for Customer Finder, you need to subscribe here:
    /// https://rapidapi.com/pti-costa-rica-pti-costa-rica-default/api/books-translation-and-analysis-by-pti/pricing
    /// </summary>
    public sealed class BooksTranslationService
    {
        private ILogger<BooksTranslationService> Logger { get; }
        private BooksTranslationConfiguration BooksTranslationConfiguration { get; }
        private CustomHttpClient CustomHttpClient { get; }

        /// <summary>
        /// Creates a new instance of <see cref="BooksTranslationService"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="booksTranslationConfiguration"></param>
        /// <param name="customHttpClient"></param>
        public BooksTranslationService(ILogger<BooksTranslationService> logger, BooksTranslationConfiguration booksTranslationConfiguration,
            CustomHttpClient customHttpClient)
        {
            this.Logger = logger;
            this.BooksTranslationConfiguration = booksTranslationConfiguration;
            this.CustomHttpClient = customHttpClient;
            this.CustomHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-rapidapi-host", "books-translation-and-analysis-by-pti.p.rapidapi.com");
            this.CustomHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-rapidapi-key", this.BooksTranslationConfiguration.Key);
            this.CustomHttpClient.DefaultRequestHeaders.TryAddWithoutValidation("useQueryString", "true");
        }

        /// <summary>
        /// Translates the specified word document
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="sourceLanguage"></param>
        /// <param name="destLanguage"></param>
        /// <param name="bookTranslationMode"></param>
        /// <param name="emailAddress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<byte[]> TranslateDocXFileFromUrlAsync(string fileUrl, TranslationLanguage sourceLanguage, TranslationLanguage destLanguage,
            BookTranslationMode bookTranslationMode, string emailAddress, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string requestUrl = $"{this.BooksTranslationConfiguration.Endpoint}/TranslateBookFromWordFile" +
                    $"?bookSourceLanguage={sourceLanguage}" +
                    $"&bookOutputLanguage={destLanguage}" +
                    $"&translationMode={bookTranslationMode}" +
                    $"&emailAddressForResults={emailAddress}" +
                    $"&sourceFileUrl={System.Net.WebUtility.UrlDecode(fileUrl)}";
                var response = await this.CustomHttpClient.PostAsync(requestUrl, null);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsByteArrayAsync();
                    return result;
                }
                else
                {
                    string reason = response.ReasonPhrase;
                    string detailedError = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Reason: {reason}. Details: {detailedError}");
                }
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
