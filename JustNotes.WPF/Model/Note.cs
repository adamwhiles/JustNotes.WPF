using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustNotes.WPF.Models
{

    public interface HasId
    {
        string Id { get; set; }
    }

    public class Note : HasId
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Content { get; set; }
    }
}
