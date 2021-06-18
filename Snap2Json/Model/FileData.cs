using System.Numerics;

namespace Snap2Json.Model
{
    public class FileData
    {
        
        public string Name { get; set; }
        
        public BigInteger Size { get; set; }
        
        public BigInteger LastModified { get; set; }
        
        public override string ToString()
        {
            return $"{Name}*{Size.ToString()}*{LastModified.ToString()}";
        }
    }
}