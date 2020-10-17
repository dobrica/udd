using Newtonsoft.Json;

namespace udd.Model
{
    public class ScientificPaperSearchRequest
    {
        ScientificPaperSearchRequest() { }

        [JsonProperty("magazineTitle")]
        public string MagazineTitle { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("authorFirstname")]
        public string AuthorFirstname { get; set; }

        [JsonProperty("authorLastname")]
        public string AuthorLastname { get; set; }

        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("scientificField")]
        public string ScientificField { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("moreLikeThisEnabled")]
        public bool MoreLikeThisEnabled { get; set; }

        [JsonProperty("moreLikeThisQuery")]
        public string MoreLikeThisQuery { get; set; }
    }
}