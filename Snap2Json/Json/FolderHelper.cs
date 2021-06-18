using System;
using System.IO;
using System.Linq;
using System.Numerics;
using Snap2Json.Model;

namespace Snap2Json.Json
{
    public static class FolderHelper
    {
        
        public static FolderData GetFolderInfo(GetFolderInfoParams infoParams, bool includeHiddenFile, bool includeSystemFile, Action<string> showMessageAction)
        {
            var size = new BigInteger(0);
            
            var currentDirectoryInfo = new DirectoryInfo(infoParams.FilePath);
            
            showMessageAction($"{currentDirectoryInfo.Name}...");
            
            var folderData = new FolderData{ Name = currentDirectoryInfo.Name};
            //
            var filesInfo = currentDirectoryInfo.EnumerateFiles();
            if (!includeHiddenFile)
                filesInfo = filesInfo.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));
            if (!includeSystemFile)
                filesInfo = filesInfo.Where(f => !f.Attributes.HasFlag(FileAttributes.System));
            
            foreach (var fileInfo in filesInfo)
            {
                var file = new FileData
                {
                    Name =  fileInfo.Name,
                    Size = new BigInteger(fileInfo.Length),
                    LastModified = ToUnixTimestamp(fileInfo.LastWriteTime.ToLocalTime())
                };
                folderData.Files.Add(file.ToString());
                //
                size += fileInfo.Length;
                infoParams.FileCount++;
            }
            //
            var directoriesInfo = currentDirectoryInfo.EnumerateDirectories();
            if (!includeHiddenFile)
                directoriesInfo = directoriesInfo.Where(d => !d.Attributes.HasFlag(FileAttributes.Hidden));

            if (!includeSystemFile)
                directoriesInfo = directoriesInfo.Where(d => !d.Attributes.HasFlag(FileAttributes.System));
            
            foreach(var dirInfo in directoriesInfo)
            {
                infoParams.FilePath = dirInfo.FullName;
                var currentFolderData = GetFolderInfo(infoParams, includeHiddenFile, includeSystemFile,showMessageAction);
                size += currentFolderData.SizeInteger;
                infoParams.DirCount++;
                folderData.Directories.Add(currentFolderData);
            }
            
            folderData.SizeInteger = size;
            
            return folderData;
        }
        
        private static int ToUnixTimestamp( DateTime value )
        {
            return (int)Math.Truncate( ( value.ToUniversalTime().Subtract( new DateTime( 1970, 1, 1 ) ) ).TotalSeconds );
        }
    }
}