using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public CategoryEntity(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }

}
