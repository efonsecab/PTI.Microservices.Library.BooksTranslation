using System;
using System.Collections.Generic;
using System.Text;

namespace PTI.Microservices.Library.Configuration
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BooksTranslationConfiguration
    {
        public string Endpoint { get; set; } = "https://books-translation-and-analysis-by-pti.p.rapidapi.com/api/1.0/Books";
        public string Key { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
