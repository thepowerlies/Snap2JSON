using System.Numerics;

namespace Snap2Json.Json
{
    public class GetFolderInfoParams
    {
        public GetFolderInfoParams()
        {
            DirCount = new BigInteger(1);
            FileCount = new BigInteger();
        }
        
        public string FilePath { get; set; }
        
        public BigInteger DirCount { get; set; }
        
        
        public BigInteger FileCount { get; set; }
    }
}