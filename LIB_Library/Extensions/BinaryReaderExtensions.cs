using LIB.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
        /// <summary>
        /// Reads a GUID from the BinaryReader
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static Guid ReadGuid(this BinaryReader br)
        {
            return new Guid(br.ReadBytes(GUID_LENGTH));
        }
        /// <summary>
        /// Reads an object of type <interface cref="ISerializableBase"/> from the BinaryReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="br"></param>
        /// <returns></returns>
        public static T ReadObject<T>(this BinaryReader br)
            where T : ISerializableBase, new()
        {
            T result = new T();
            result.Deserialize(br.ReadBytes(result.GetSize()));
            return result;
        }
        /// <summary>
        /// Reads a list ob object of type <interface cref="ISerializableBase"/> from the BinaryReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="br"></param>
        /// <param name="ArraySize"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadObjectList<T>(this BinaryReader br, int ArraySize)
            where T: ISerializableBase, new()
        {
            if (ArraySize <= 0)
                throw new ArgumentException($"Invalid Array size: <{ArraySize}>");
            T item;
            for(int i = 0; i < ArraySize; i++)
            {
                item = br.ReadObject<T>();
                yield return item;
            }
        }
    }
}
