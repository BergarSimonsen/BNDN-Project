using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestService.IO_Messages;

namespace RestService
{
    interface IController
    {
        Response Call(Request request);
    }
}
