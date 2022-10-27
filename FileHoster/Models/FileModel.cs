using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace FileHoster.Models
{
    public sealed class FileModel
    {
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
    }
}
