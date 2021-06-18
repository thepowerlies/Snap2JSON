using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Snap2Json.Model
{
    public class Output
    {
        private string _root;
        
        public Output()
        {
            Date = DateTime.Now.ToString();
            Version = 1;
        }
        
        public string Date { get; }
        
        [JsonIgnore]
        public BigInteger DirCountInteger { get; set; }

        public string DirCount => DirCountInteger.ToString();
        
        [JsonIgnore]
        public BigInteger TotalSizeInteger{ get; set; }
        
        public string TotalSize => TotalSizeInteger.ToString();
        
        [JsonIgnore]
        public BigInteger FileCountInteger { get; set; }

        public string FileCount => FileCountInteger.ToString();

        public string Root
        {
            get => _root.Replace('\\', '/');
            set => _root = value;
        }
        
        public List<FolderData> Data { get; } = new List<FolderData>();
        
        public int Version { get; }
    }
}