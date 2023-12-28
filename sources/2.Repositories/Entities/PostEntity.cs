using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class PostEntity
    {
        public int Id { get; set; }
        public int PublisherId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PublicationType PublicationType { get; set; }
        public string Content { get; set; }
        public List<int> CommentsIds { get; set; } = new List<int>();

        public PostEntity(int id, int publisher, int category, string title,
            string description, PublicationType publicationType, string content, List<int> comments)
        {
            Id = id;
            PublisherId = publisher;
            CategoryId = category;
            Title = title;
            Description = description;
            PublicationType = publicationType;
            Content = content;
            CommentsIds = comments;
        }
    }
}
