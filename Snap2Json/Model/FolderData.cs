using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Snap2Json.Model
{
    public class  FolderData
    {
        [JsonPropertyName("n")]
        public string Name { get; set; }
        
        [JsonIgnore]
        public BigInteger SizeInteger { get; set; }

        [JsonPropertyName("s")]
        public string Size => SizeInteger.ToString();
        
        
        //public string LastModify
        
        [JsonPropertyName("d")]
        public List<FolderData> Directories { get; } = new List<FolderData>();
        
        [JsonPropertyName("f")]
        public List<string> Files { get; } = new List<string>();
        
    }
}