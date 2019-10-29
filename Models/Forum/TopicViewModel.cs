using System;
using System.Collections.Generic;

namespace Simulacao.Models{
    public class TopicViewModel{
        public Topic topic { get; set; }
        public PaginatedList<Posting> postings { get; set; }
        public List<PostingViewModel> postingsViewModel { get; set; }
        public Posting Post { get; set; }
    }
}