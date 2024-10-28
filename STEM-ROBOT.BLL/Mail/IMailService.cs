
using STEM_ROBOT.Common.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEM_ROBOT.BLL.Mail
{
    public  interface IMailService
    {
        Task SendEmailAsync(MailReq mailRequest);
    }
}
