using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Req
{
    public class GenreReq
    {

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool? IsTop { get; set; }

        public string? HintRule { get; set; }

        public string? HintScore { get; set; }

    }
}