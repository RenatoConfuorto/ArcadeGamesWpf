using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core.Helpers
{
    public class XmlSerializerBase
    {
        public static void SerializeObject(string fileName, object obj)
        {
            SerializeObject(fileName, obj, false);
        }

        public static void SerializeObject(string fileName, object obj, bool append)
        {
            XmlSerializer mySerializer = new XmlSerializer(obj.GetType());
            StreamWriter myWriter = new StreamWriter(fileName, append);
            mySerializer.Serialize(myWriter, obj);
            myWriter.Dispose();
            myWriter.Close();
        }

        public static object DeserializeObject(string fileName, Type objType)
        {
            XmlSerializer mySerializer = new XmlSerializer(objType);
            FileStream myFileStream = new FileStream(fileName, FileMode.Open);
            var result = mySerializer.Deserialize(myFileStream);

            myFileStream.Close();       
            return result;
        }

        public static string SerializeObjectToString(object obj)
        {
            XmlSerializer mySerializer = new XmlSerializer(obj.GetType());
            MemoryStream myWriter = new MemoryStream();
            mySerializer.Serialize(myWriter, obj);

            var result = Encoding.UTF8.GetString(myWriter.ToArray());

            myWriter.Close();

            return result;
        }

        public static object DeserializeObjectFromString(string serializedItem, Type objType)
        {
            XmlSerializer mySerializer = new XmlSerializer(objType);

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(serializedItem);
            writer.Flush();
            stream.Position = 0;

            var result = mySerializer.Deserialize(stream);
            writer.Close();
            stream.Close();

            return result;          
        }
    }
}
