using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class ScoreCategoryRes
    {
        public string Description { get; set; }
        public int Bonus { get; set; }
        public int Minus { get; set; }
        public int GenreId { get; set; }
    }
}
