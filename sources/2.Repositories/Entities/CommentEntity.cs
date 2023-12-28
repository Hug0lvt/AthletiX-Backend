using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public int IdParent { get; set; }
        public DateTime PublishDate { get; set; }
        public int IdPublisher { get; set; }
        public string Content { get; set; }

        public CommentEntity(int id, int idParent, DateTime publishDate, int idPublisher, string content) 
        {
            Id = id;
            IdParent = idParent;
            IdPublisher = idPublisher;
            Content = content;
            PublishDate = publishDate;
        }
    }
}
