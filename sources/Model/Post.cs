using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model
{
    public class Post
    {
        public int Id { get; set; }
        public Profile Publisher { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PublicationType PublicationType { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();

        public override string ToString()
        {
            string publisherName = Publisher != null ? Publisher.ToString() : "Anonymous";
            string categoryName = Category != null ? Category.ToString() : "Uncategorized";
            string publicationTypeName = PublicationType != null ? PublicationType.ToString() : "Unknown";
            string commentsCount = Comments != null ? Comments.Count.ToString() : "0";
            string exercisesCount = Exercises != null ? Exercises.Count.ToString() : "0";
        
            return $"Post Id: {Id}, Publisher: {publisherName}, Category: {categoryName}, Title: \"{Title}\", Description: \"{Description}\", Publication Type: {publicationTypeName}, Content: \"{Content}\", Comments Count: {commentsCount}, Exercises Count: {exercisesCount}";
        }
    }
}
