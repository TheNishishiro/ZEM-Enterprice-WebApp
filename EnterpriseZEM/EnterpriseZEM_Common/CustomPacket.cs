using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnterpriseZEM_Common
{
    [Serializable]
    public enum HeaderTypes
    {
        basic = 0,
        error = 1
    };

    [Serializable]
    public enum FlagType
    {
        basic = -1,
        ping = 0,
        endConnection = 1,
        notInTech = 100,
        notInDeclared = 101,
        isKanban = 102,
        isDeleted = 103,
        quantityIncorrect = 110,
        quantityOverLimit = 111,
        codeExists = 200,
        codeExistsBack = 201,
        codeFitsBack = 300,
        binNotFound = 400,
        nonScanned = 410,
    };

    [Serializable]
    public class CustomPacket
    {
        public FlagType Flag { get; set; }
        public HeaderTypes Header { get; set; }
        public string Command { get; set; }
        public List<string> Args { get; set; }
        public object Payload { get; set; }

        public CustomPacket()
        {
            Flag = FlagType.basic;
            Header = HeaderTypes.basic;
            Command = "NULL";
            Args = new List<string>();
            Payload = null;
        }

        public CustomPacket(FlagType Flag, HeaderTypes basic, string Command, List<string> Args, object Payload)
        {
            this.Flag = Flag;
            this.Header = Header;
            this.Command = Command;
            this.Args = Args;
            this.Payload = Payload;
        }

        public void Print()
        {
            Console.WriteLine($"FLAG: {Flag.ToString()}\nHeader: {Header.ToString()}\nCommand: {Command}\nArgs: ");
            foreach(string arg in Args)
            {
                Console.WriteLine(arg);
            }
        }
    }
}
