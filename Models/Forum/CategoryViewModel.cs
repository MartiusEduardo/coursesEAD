using System.Collections.Generic;

namespace Simulacao.Models{
    public class CategoryViewModel{
        public Category Category { get; set; }
        public List<Topic> Topics { get; set; }
        public List<PostingViewModel> Postings { get; set; }
    }
}