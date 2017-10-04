using System.Net.Sockets;

namespace BDLib.Net
{
    public static class AutoSocket
    {
        public static Socket Tcp
        {
            get
            {
                if (Socket.OSSupportsIPv4)
                    return IPv4.Sockets.Tcp;
                if (Socket.OSSupportsIPv6)
                    return IPv4.Sockets.Tcp;
                return null;
            }
        }
        public static Socket Udp
        {
            get
            {
                if (Socket.OSSupportsIPv4)
                    return IPv4.Sockets.Udp;
                if (Socket.OSSupportsIPv6)
                    return IPv4.Sockets.Udp;
                return null;
            }
        }
    }
}
namespace BDLib.Net.IPv4
{
    public static class Sockets
    {
        public static Socket Tcp
        {
            get { return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); }
        }
        public static Socket Udp
        {
            get { return new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); }
        }
    }
}
namespace BDLib.Net.IPv6
{
    public static class Sockets
    {
        public static Socket Tcp
        {
            get { return new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp); }
        }
        public static Socket Udp
        {
            get { return new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp); }
        }
    }
}