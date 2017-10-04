using Renci.SshNet;
using System;
using System.IO;
using BDLib.BDLibInfo;

namespace BDLib.Net.Ssh
{
    public class SSHClient : IDisposable
    {
        //members
        SshClient Soc;
        ShellStream SshShell;
        bool STDMode = false;
        
        public Stream GetStream()
        {
            if(STDMode)
            {
                return SshShell;
            }
            else
                throw new InvalidOperationException("STD mode only");
        }

        public SSHClient(string ip, int port, string Username, string Password)
        {
            if (!Info.Moduls.Contains("Net/Ssh/SSHClient.cs"))
                Info.Moduls.Add("Net/Ssh/SSHClient.cs");

            Soc = new SshClient(ip, port, Username, Password);
            Soc.Connect();
            Soc.KeepAliveInterval = TimeSpan.FromMilliseconds(200);
        }

        public ShellStream INITStream(uint Colams, uint Rows)
        {
            STDMode = false;
            return Soc.CreateShellStream(
                "BDSshterminal",
                Colams,
                Rows,
                Colams*2,
                Rows*2,
                512);
        }

        public void STDINIT()
        {
            STDMode = true;
            SshShell = Soc.CreateShellStream
                (
                "BDSshterminal",
                200,
                20,
                800,
                80,
                512
                );
        }

        public void SendCommand(string command)
        {
            if(STDMode)
                SshShell.WriteLine(command);
            else
                throw new InvalidOperationException("STD mode only");
        }
        public void SendBytes(byte[] data)
        {
            if(STDMode)
                SshShell.Write(data, 0, data.Length);
            else
                throw new InvalidOperationException("STD mode only");
        }

        public string ReadLine()
        {
            if (STDMode)
                return SshShell.ReadLine();
            else
                throw new InvalidOperationException("STD mode only");
        }
        public byte[] ReadBytes(int length)
        {
            if (STDMode)
            {
                byte[] buf = new byte[length];
                SshShell.Read(buf, 0, length);
                return buf;
            }
            else
                throw new InvalidOperationException("STD mode only");
        }

        public void CloseStream()
        {
            if (STDMode)
            {
                SshShell.Close();
            }
            else
                throw new InvalidOperationException("STD mode only");
        }

        public void Dispose()
        {
            if(STDMode)
            {
                SshShell.Close();
            }
            Soc.Disconnect();
        }
    }
}
