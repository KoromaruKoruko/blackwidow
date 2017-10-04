using System.Collections.Generic;

namespace BDLib.BDLibInfo
{
    public static class Info
    {
        public const string Version = "1.2 Beta Release";
        private static readonly string[] Items = new string[]
        {
            "BDLibInfo/Info.cs - <Static Indexer has no version>",
            "Crypto/Hash/OneKeyHasher.cs - v1.10",
            "Crypto/Hash/BDFileCheckSum.cs - v1",
            "Crypto/AES.cs - v1.1",
            "Crypto/BDCrypto.cs - v2.4",
            "Crypto/BDCryptoV2.cs - v1.3",
            "DataBase/SqliteDataBase.cs - v1.5",
            "DataTypes/S_Int.cs - v1",//new version/better of DynamicInt biggest number is 2,147,483,647 digits large IE int.MaxValue bytes so ye be cairfull with ram
            //^^ is broken so comming soon
			"Internal/Chat/InternalAlerts.cs - v1.3",
            "Internal/Chat/InternalChat.cs - v1.1",
            "Internal/CommandLine/DynamicCMD.cs - v2.5",
            "Internal/DataStream/DataStream.cs - v1.4",
            "Internal/DataStream/TwoWayDataStream.cs - v1.2",
            //"Net/Http/HttpServer.cs - PreRelase Build", working version comming soon
            "Net/Ssh/SSHClient.cs - v1",
            "Net/Sockets.cs - v1.1",
            "Net/TcpClientAsync.cs -v1.1",
            "NeuralNetworks/Learning/BackProb.cs - v1.1",
            "NeuralNetworks/Learning/SOMLearnig.cs - v1.1",
            "NeuralNetworks/NeuralNetwork.cs - v1.2",
            "Reg/Regedit.cs - v1.4",
            "Resorces/ResorceManager.cs - v1.3",
            "Text/ByteHelpers.cs - v2.4",
            "Text/Hex.cs - v1.1",
            "Text/Logger.cs - v2.4.2",
            "Text/StringHelpers.cs - v1.1",
            //"UserConfigs/BDConfig.cs - Unbuilt", comming soon
            "Youtube/YoutubeDownloader.cs - v1.1"
        };
        internal static List<string> Moduls = new List<string>();
        public static readonly string[] Dependencys = new string[]
        {
            "AForge - v2.2.5",
            "AForge.Genetic - v2.2.5",
            "AForge.Math - v2.2.5",
            "AForge.Neuro - v2.2.5",
            "EntityFrameWork - v6.0.0",
            "System.Data.SQLite - v1.0.105.2",
            "System.Data.SQLite.Core - v1.0.105.2",
            "System.Data.SQLite.EF6 - v1.0.105.2",
            "System.Data.SQLite.Linq - v1.0.105.2",
            "VideoLibrary - v1.3.5",
            "SSH.Net - v2016.0.0"
        };
        public const string SourcePage = "https://github.com/dedady157/blackwidow/tree/master/BDLib/BDLib";

        public static List<string> ActiveModuls => Moduls;
    }
}
