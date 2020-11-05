using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nest;
using System;
using System.IO;
using System.Linq;
using udd.Database;
using udd.Model;

namespace udd
{
    public class ScientificCenterESClient : ElasticClient
    {
        public static readonly string DEFAULT_INDEX = "scientific_papers";
        public static readonly string PRE_TAG_HIGHLIGHER = "<span class=\"highlighter\">";
        public static readonly string POST_TAG_HIGHLIGHER = "</span>";

        private static readonly string MAGAZINE_TITLE_FIELD = "scientificPaper.magazineTitle";
        private static readonly string TITLE_FIELD = "scientificPaper.title";
        private static readonly string AUTHOR_FIRSTNAME_FIELD = "scientificPaper.authors.firstname";
        private static readonly string AUTHOR_LASTNAME_FIELD = "scientificPaper.authors.lastname";
        private static readonly string KEYWORDS_FIELD = "scientificPaper.keywords.title";
        private static readonly string SCIENTIFIC_FIELDS_FIELD = "scientificPaper.scientificFields.title";
        private static readonly string CONTENT_FIELD = "attachment.content";

        public ScientificCenterESClient(ConnectionSettings settings) : base(settings) { }

        public ISearchResponse<ScientificPaperDocument> Search(ScientificPaperSearchRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string searchStringQuery = GetQueryString(request);

            if (searchStringQuery.Equals(string.Empty))
            {
                return new SearchResponse<ScientificPaperDocument>();
            }

            return Search<ScientificPaperDocument>(s => s
                .Index(DEFAULT_INDEX)
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(searchStringQuery)))
                .Highlight(h => h.PreTags(PRE_TAG_HIGHLIGHER).PostTags(POST_TAG_HIGHLIGHER).Encoder(HighlighterEncoder.Html)
                    .Fields(
                        f => f.Field(MAGAZINE_TITLE_FIELD),
                        f => f.Field(TITLE_FIELD),
                        f => f.Field(AUTHOR_FIRSTNAME_FIELD),
                        f => f.Field(AUTHOR_LASTNAME_FIELD),
                        f => f.Field(KEYWORDS_FIELD),
                        f => f.Field(SCIENTIFIC_FIELDS_FIELD),
                        f => f.Field(CONTENT_FIELD))));
        }

        public ISearchResponse<ScientificPaperDocument> MoreLikeThis(ScientificPaperSearchRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return Search<ScientificPaperDocument>(s => s
                .Index(DEFAULT_INDEX)
                .Query(q => q
                    .MoreLikeThis(mlt => mlt
                        .Fields(f => f
                            .Field(MAGAZINE_TITLE_FIELD)
                            .Field(TITLE_FIELD)
                            .Field(AUTHOR_FIRSTNAME_FIELD)
                            .Field(AUTHOR_LASTNAME_FIELD)
                            .Field(KEYWORDS_FIELD)
                            .Field(SCIENTIFIC_FIELDS_FIELD)
                            .Field(CONTENT_FIELD))
                                .Like(l => l
                                    .Text(request.MoreLikeThisQuery))
                                .MinDocumentFrequency(1)
                                .MinTermFrequency(1)))
                .Highlight(h => h.PreTags(PRE_TAG_HIGHLIGHER).PostTags(POST_TAG_HIGHLIGHER).Encoder(HighlighterEncoder.Html)
                    .Fields(
                        f => f.Field(MAGAZINE_TITLE_FIELD)
                            .HighlightQuery(q => q
                                .Match(m => m
                                    .Field(p => p.ScientificPaper.MagazineTitle)
                                    .Query(request.MoreLikeThisQuery)
                                )
                            ),
                        f => f.Field(TITLE_FIELD)
                            .HighlightQuery(q => q
                                .Match(m => m
                                    .Field(p => p.ScientificPaper.Title)
                                    .Query(request.MoreLikeThisQuery)
                                )
                            ),
                        f => f.Field(AUTHOR_FIRSTNAME_FIELD)
                            .HighlightQuery(q => q
                                    .Match(m => m
                                        .Field(p => p.ScientificPaper.Authors.First().Firstname)
                                        .Query(request.MoreLikeThisQuery)
                                    )
                                ),
                        f => f.Field(AUTHOR_LASTNAME_FIELD)
                            .HighlightQuery(q => q
                                    .Match(m => m
                                        .Field(p => p.ScientificPaper.Authors.First().Lastname)
                                        .Query(request.MoreLikeThisQuery)
                                    )
                                ),
                        f => f.Field(KEYWORDS_FIELD)
                            .HighlightQuery(q => q
                                    .Match(m => m
                                        .Field(p => p.ScientificPaper.Keywords.First().Title)
                                        .Query(request.MoreLikeThisQuery)
                                    )
                                ),
                        f => f.Field(SCIENTIFIC_FIELDS_FIELD)
                            .HighlightQuery(q => q
                                    .Match(m => m
                                        .Field(p => p.ScientificPaper.ScientificFields.First().Title)
                                        .Query(request.MoreLikeThisQuery)
                                    )
                                ),
                        f => f.Field(CONTENT_FIELD)
                            .HighlightQuery(q => q
                                    .Match(m => m
                                        .Field(p => p.Attachment.Content)
                                        .Query(request.MoreLikeThisQuery))))));
        }

        public CreateIndexResponse CreateIndex()
        {
            return Indices.Create(DEFAULT_INDEX,
                c => c.Map<ScientificPaperDocument>(mp => mp.AutoMap()
                    .Properties(ps => ps
                        .Object<Attachment>(a => a
                          .Name(n => n.Attachment)
                          .Properties(ps => ps
                            .Text(s => s
                            .Name(n => n.Content)
                            .Analyzer("serbian")))
                          .AutoMap()))));
        }

        public PutPipelineResponse PutPipeline()
        {
            return Ingest.PutPipeline("attachments",
                p => p.Description("Document attachment pipeline")
                    .Processors(pr => pr
                      .Attachment<ScientificPaperDocument>(a => a
                        .Field(f => f.Content)
                        .TargetField(f => f.Attachment)
                      )
                      .Remove<ScientificPaperDocument>(r => r
                        .Field(ff => ff
                          .Field(f => f.Content)))));
        }

        public string AddAndIndexTestdata(ScientificCenterDbContext dbContext)
        {
            var directory = Directory.GetCurrentDirectory();

            if (!dbContext.ScientificPapers.Any())
            {
                char separator = Path.DirectorySeparatorChar;
                string script = File.ReadAllText($"Database{separator}InitDb.sql");
                dbContext.Database.ExecuteSqlRaw(script);
            }
            else
            {
                return "Database already contains data!";
            }

            var scientificPapers = dbContext.ScientificPapers
                .Include(a => a.Authors)
                .Include(k => k.Keywords)
                .Include(s => s.ScientificFields).ToList();

            var indexedPapers = Search<ScientificPaperDocument>(s => s.Index(DEFAULT_INDEX)).Documents.Select(s => s.ScientificPaper).ToList();

            // skip indexing of test data if it is already indexed in ES
            if (indexedPapers.Any(scientificPapers.Contains))
            {
                return "Test data already indexed!";
            }

            foreach (var sp in scientificPapers)
            {
                IndexScientitifPaper(directory, sp.PdfFileName, sp);
            }

            return "Successfully added and indexed test data!";
        }

        public void IndexScientitifPaper(string directory, string filename, ScientificPaper newScientificPaper)
        {
            var base64File = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(directory, "files", filename)));
            var doc = new ScientificPaperDocument { ScientificPaper = newScientificPaper, Content = base64File };
            Index(doc, i => i.Index(DEFAULT_INDEX).Pipeline("attachments"));
        }

        public void DeleteIndexFromES(int id)
        {
            var paper = Search<ScientificPaperDocument>(s => s.Index(DEFAULT_INDEX)).Hits.ToList().Single(s => s.Source.ScientificPaper.Id == id);
            Delete<ScientificPaperDocument>(paper.Id);
        }

        private string GetQueryString(ScientificPaperSearchRequest request)
        {
            string magazineTitleQuery = CreateQuery(MAGAZINE_TITLE_FIELD, request.MagazineTitle, request.Operation);
            string titleQuery = CreateQuery(TITLE_FIELD, request.Title, request.Operation);
            string authorFirstnameQuery = CreateQuery(AUTHOR_FIRSTNAME_FIELD, request.AuthorFirstname, request.Operation);
            string authorLastnameQuery = CreateQuery(AUTHOR_LASTNAME_FIELD, request.AuthorLastname, request.Operation);
            string keywordsQuery = CreateQuery(KEYWORDS_FIELD, request.Keyword, request.Operation);
            string scientificFieldsQuery = CreateQuery(SCIENTIFIC_FIELDS_FIELD, request.ScientificField, request.Operation);
            string attachmentQuery = CreateQuery(CONTENT_FIELD, request.Content, request.Operation);

            string searchStringQuery = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                magazineTitleQuery, titleQuery, authorFirstnameQuery, authorLastnameQuery, keywordsQuery, scientificFieldsQuery, attachmentQuery).Trim();

            if (searchStringQuery.Length < 1)
            {
                return string.Empty;
            }

            // searchString can't have operator at the end: remove last operator and return searchString
            return searchStringQuery.Substring(0, searchStringQuery.Length - request.Operation.Length).Trim();
        }

        private string CreateQuery(string indexedFieldName, string searchValue, string operation)
        {
            if (searchValue == null || searchValue == string.Empty)
            {
                return string.Empty;
            }

            return string.Format("({0}:{1}) {2}", indexedFieldName, searchValue, operation).Trim();
        }
    }
}