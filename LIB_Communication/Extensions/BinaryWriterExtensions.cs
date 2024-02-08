using Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Extensions
{
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// Writes an object of type <interface cref="ISerializableBase"/> to the BinaryWriter buffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bw"></param>
        /// <param name="obj"></param>
        public static void WriteObject<T>(this BinaryWriter bw,T obj)
            where T : ISerializableBase, new()
        {
            bw.Write(obj.Serialize());
        }
        /// <summary>
        /// Writers a list of objects of type <interface cref="ISerializableBase"/> to the BinaryWriter buffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bw"></param>
        /// <param name="list"></param>
        public static void WriteObjectList<T>(this BinaryWriter bw, IEnumerable<T> list)
            where T : ISerializableBase, new()
        {
            foreach(T item in list)
            {
                bw.WriteObject(item);
            }
        }
    }
}
