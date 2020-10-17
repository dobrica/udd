using Nest;

namespace udd.Model
{
    /**
     ScientificPaperDocuments are indexed in Elasticsearch
    */
    public class ScientificPaperDocument
    {
        public ScientificPaper ScientificPaper { get; set; }
        public string Content { get; set; }
        public Attachment Attachment { get; set; }
    }
}
