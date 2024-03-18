using Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Extensions
{
    public static class BinaryReaderExtensions
    {
        private const int GUID_LENGTH = 16;
        /// <summary>
        ///     Reads a GUID from the BinaryReader
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static Guid ReadGuid(this BinaryReader br)
        {
            return new Guid(br.ReadBytes(GUID_LENGTH));
        }
        /// <summary>
        ///     Reads an object of type <interface cref="ISerializableBase"/> from the BinaryReader using all the remaining bytes
        /// </summary>
        /// <remarks>
        ///     The object to read must be positioned at the end of the stream since this method takes all the remaining bytes of it.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="br"></param>
        /// <returns></returns>
        public static T ReadDynamicObject<T>(this BinaryReader br)
            where T : ISerializableBase, new()
        {
            T result = new T();
            long remaining_bytes = br.BaseStream.Length - br.BaseStream.Position;
            result.Deserialize(br.ReadBytes((int)remaining_bytes));
            return result;
        }
        /// <summary>
        ///     Reads an object of type <interface cref="ISerializableBase"/> from the BinaryReader
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
        ///     Reads a list ob object of type <interface cref="ISerializableBase"/> from the BinaryReader
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
        /// <summary>
        ///     Reads a string of length <paramref name="StringLength"/> from the BinaryReader
        /// </summary>
        /// <param name="br"></param>
        /// <param name="StringLength"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ReadString(this BinaryReader br, int StringLength)
        {
            if (StringLength <= 0)
                throw new ArgumentException($"Invalid String length: <{StringLength}>");

            char[] st = br.ReadChars(StringLength);
            return new string(st);
        }
    }
}
