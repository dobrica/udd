using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using udd.Database;
using udd.Model;

namespace udd.Controllers
{
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ScientificCenterController : ControllerBase
    {
        private static Uri node;
        private static ConnectionSettings settings;
        private ScientificCenterDbContext dbContext;
        private ScientificCenterESClient client;

        public ScientificCenterController(ScientificCenterDbContext dbContext)
        {
            node = new Uri("http://localhost:9200");
            settings = new ConnectionSettings(node);
            settings.DefaultIndex(ScientificCenterESClient.DEFAULT_INDEX);
            client = new ScientificCenterESClient(settings);
            this.dbContext = dbContext;

            var response = client.Cluster.Health(ScientificCenterESClient.DEFAULT_INDEX);

            if (response.NumberOfNodes < 1)
            {
                throw new Exception("Elasticsearch is not running! \n\n" + response.DebugInformation);
            }

            client.CreateIndex();
            client.PutPipeline();
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("/testdata")]
        public string IndexTestData()
        {
            return client.AddAndIndexTestdata(dbContext);
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Route("/add")]
        public async void AddAndIndexNewScientificPaperAsync([FromForm] IFormFile file, [FromForm] string json)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var request = JsonConvert.DeserializeObject<AddScientificPaperRequest>(json);
            var newScientificPaper = GetScientificPaperFromRequest(file.FileName, request);
            dbContext.ScientificPapers.Add(newScientificPaper);
            dbContext.SaveChanges();

            var directory = Directory.GetCurrentDirectory();

            await UploadFile(directory, file);

            client.IndexScientitifPaper(directory, file.FileName, newScientificPaper);
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Route("/delete")]
        public void DeleteScientificPaperAndRemoveIndex(DeleteScientificPaperRequest request)
        {
            var scientificPaper = dbContext.ScientificPapers
                    .Include(a => a.Authors)
                    .Include(k => k.Keywords)
                    .Include(s => s.ScientificFields).ToList().Single(s => s.Id == int.Parse(request.Id));
            if (scientificPaper != null)
            {
                dbContext.ScientificPapers.RemoveRange(scientificPaper);
                dbContext.SaveChanges();
            }

            client.DeleteIndexFromES(int.Parse(request.Id));
        }

        [HttpPost]
        [Route("/search")]
        public string SearchScientificPaper(ScientificPaperSearchRequest request)
        {
            ISearchResponse<ScientificPaperDocument> response;
            if (request.MoreLikeThisEnabled)
            {
                response = client.MoreLikeThis(request);
            }
            else
            {
                response = client.Search(request);
            }

            return JsonConvert.SerializeObject(response.Hits.ToList(), Formatting.Indented);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("/file/{id}")]
        public IActionResult GetFileById(int id)
        {
            ScientificPaper sp = dbContext.ScientificPapers.FirstOrDefault(s => s.Id.Equals(id));
            char separator = Path.DirectorySeparatorChar;
            string filepath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"..{separator}..{separator}..{separator}files{separator}", sp.PdfFileName));

            return new PhysicalFileResult(filepath, "application/pdf");
        }

        private ScientificPaper GetScientificPaperFromRequest(string filename, AddScientificPaperRequest request)
        {
            var authors = new List<Author>();
            authors.Add(new Author(request.AuthorFirstname, request.AuthorLastname));

            var keywords = new List<Keyword>();
            foreach (var k in request.Keyword.Split(',').ToList())
            {
                keywords.Add(new Keyword(k));
            }

            var scientificFields = new List<ScientificField>();
            foreach (var sf in request.ScientificField.Split(',').ToList())
            {
                scientificFields.Add(new ScientificField(sf));
            }

            return new ScientificPaper
            {
                MagazineTitle = request.MagazineTitle,
                Title = request.Title,
                Authors = authors,
                Keywords = keywords,
                ScientificFields = scientificFields,
                PdfFileName = filename
            };
        }

        private async Task UploadFile(string directory, IFormFile file)
        {
            var filepath = Path.Combine(directory, "files", file.FileName);
            using (var fileStream = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}