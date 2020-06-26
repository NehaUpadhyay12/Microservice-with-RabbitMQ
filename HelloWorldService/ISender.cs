using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorldService
{
    public interface ISender
    {
        void SendMessage(string message);
    }
}
