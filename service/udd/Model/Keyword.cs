using Nest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace udd.Model
{
    public class Keyword
    {
        public Keyword(string title)
        {
            Title = title;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [Text(Analyzer = "serbian")]
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
