using System;
using System.Text.Json;
using Snap2Json.Model;

namespace Snap2Json.Json
{
    public class JsonGenerator
    {
        public static string GenerateOutput(
            string folderPath, bool includeHiddenFile, bool includeSystemFile, Action<string> showMessageAction)
        {
            var infoParams = new GetFolderInfoParams
            {
                FilePath = folderPath,
            };
            
            var rootFolderData = FolderHelper.GetFolderInfo(infoParams,  includeHiddenFile, includeSystemFile, showMessageAction);
            
            var output = new Output
            {
                DirCountInteger = infoParams.DirCount,
                FileCountInteger = infoParams.FileCount,
                Root = folderPath,
                TotalSizeInteger = rootFolderData.SizeInteger
            };
            
            output.Data.Add(rootFolderData);
            
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            return JsonSerializer.Serialize(output, jsonSerializerOptions);
        }
        
    }
}