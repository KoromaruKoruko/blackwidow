using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDLib.Net
{
    public interface BDSocketBase
    {
        bool Connect(string host, int port);
        void Disconnect(bool Reuse);
    }
}
