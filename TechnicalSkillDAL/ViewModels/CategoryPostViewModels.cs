using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSkill.DAL
{
    public class CategoryPostViewModels
    {
        public CategoryPostViewModels()
        {

        }
        public CategoryPostViewModels(Post p)
        {
            Id = p.Id;
            Title = p.Title;
            Content = p.Content;
            Description = p.Description;
            SourceID = p.SourceID;
            Created_At = p.Created_At;
            CategoryId = p.CategoryId;
            CategoryName = p.Categories.Name;

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string SourceID { get; set; }
        public string Created_At { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
