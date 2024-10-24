using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.Common.Rsp
{
    public class FormatRsp
    {
        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
