using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Comment
    {
        public int Id { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime PublishDate { get; set; }
        public Profile Publisher { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public List<Comment> Answers { get; set; } = new List<Comment>();
        
        public override string ToString()
        {
            string publisherName = Publisher != null ? Publisher.ToString() : "Anonymous";
            string parentCommentInfo = ParentCommentId.HasValue ? ParentCommentId.Value.ToString() : "No Parent";
            string answersCount = Answers != null ? Answers.Count.ToString() : "0";
            
            return $"Comment Id: {Id}, Parent Comment Id: {parentCommentInfo}, Publish Date: {PublishDate}, Publisher: {publisherName}, Post: {PostId}, Content: \"{Content}\", Answers Count: {answersCount}";
        }
    }
    
}
