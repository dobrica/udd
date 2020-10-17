using Nest;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace udd.Model
{
    public class ScientificPaper
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataMember(Name = "doi")]
        public string DOI { get; set; }

        [Text(Analyzer = "serbian")]
        [DataMember(Name = "magazineTitle")]
        public string MagazineTitle { get; set; }

        [Text(Analyzer = "serbian")]
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "authors")]
        public List<Author> Authors { get; set; }

        [DataMember(Name = "keywords")]
        public List<Keyword> Keywords { get; set; }

        [DataMember(Name = "scientificFields")]
        public List<ScientificField> ScientificFields { get; set; }

        [Text(Analyzer = "serbian")]
        [DataMember(Name = "abstract")]
        public string Abstract { get; set; }

        [DataMember(Name = "pdfFileName")]
        public string PdfFileName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                return (Id == ((ScientificPaper)obj).Id && DOI == ((ScientificPaper)obj).DOI);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
