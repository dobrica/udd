using Nest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace udd.Model
{
    public class Author
    {
        Author() { }

        public Author(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Text(Analyzer = "serbian")]
        [DataMember(Name = "firstname")]
        public string Firstname { get; set; }

        [Text(Analyzer = "serbian")]
        [DataMember(Name = "lastname")]
        public string Lastname { get; set; }
    }
}
