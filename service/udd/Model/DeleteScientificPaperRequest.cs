using Newtonsoft.Json;

namespace udd.Model
{
    public class DeleteScientificPaperRequest
    {
        public DeleteScientificPaperRequest() {}

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
