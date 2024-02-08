using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace LIB.Extensions
{
    public static class BinaryReaderExtensions
    {
        private const int GUID_LENGTH = 16;
        public static Guid ReadGuid(this BinaryReader br)
        {
            return new Guid(br.ReadBytes(GUID_LENGTH));
        }
    }
}
