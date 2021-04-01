using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;

namespace RollingLineSavegameFix.Services
{

    public class NfoReaderService : INfoReaderService
    {
        private readonly IFileSystem _fileSystem;

        public NfoReaderService() : this(new FileSystem())
        { }

        public NfoReaderService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public string ReadNfo()
        { 
            
           try
           {                
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var encoding = Encoding.GetEncoding(865);
                using StreamReader reader = new StreamReader("Resources\\nfl.nfo", encoding);
                return reader.ReadToEnd();
           }
           catch
           {             
                return string.Empty;
           }            
        }
    }
}
