using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalSkill.DAL
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }

        public string SourceID { get; set; }
        public string Created_At { get; set; }
        public long Views { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")] 
        public virtual Category Categories { get; set; }
    }
}
