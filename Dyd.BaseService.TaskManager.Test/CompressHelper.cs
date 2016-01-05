using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCompress.Archive;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace Dyd.BaseService.TaskManager.Test
{
    public class CompressHelper
    {
        public static void UnCompress(string compressfilepath,string uncompressdir)
        {
            string ext = Path.GetExtension(compressfilepath).ToLower();
            if (ext == ".rar")
                UnRar(compressfilepath, uncompressdir);
            else if (ext == ".zip")
                UnZip(compressfilepath,uncompressdir);
        }
        public static void UnRar(string compressfilepath, string uncompressdir)
        {
            using (Stream stream = File.OpenRead(compressfilepath))
            {
                using (var reader = ReaderFactory.Open(stream))
                { 
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            Console.WriteLine(reader.Entry.FilePath);
                            reader.WriteEntryToDirectory(uncompressdir, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                        }
                    }
                }
            }
        }
        public static void UnZip(string compressfilepath, string uncompressdir)
        {
            using (var archive = ArchiveFactory.Open(compressfilepath))
            { 
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        Console.WriteLine(entry.FilePath);
                        entry.WriteToDirectory(uncompressdir, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
        }
    }
}
