using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Simulacao.Models {
    public class AreaViewModel {
        public Area Area { get; set; }
        public List<Subarea> Subareas { get; set; }
        public string StatusMessage { get; set; }
        public string imgSrc { get; set; }
    }
}