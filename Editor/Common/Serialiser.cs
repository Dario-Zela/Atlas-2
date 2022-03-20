using System;
using System.IO;
using System.Runtime.Serialization;

namespace Editor
{
    //The implementation of a DataContract Serialiser
    public static class Serialiser
    {
        //Writes a class to a file
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                //Make a create type filestream
                using var filestream = new FileStream(path, FileMode.Create);

                //Make the serialiser and write the object to the file stream
                var serialiser = new DataContractSerializer(typeof(T));
                serialiser.WriteObject(filestream, instance);
            }
            catch (Exception ex)
            {
                Logger.Log(MessageType.Error, $"Serialisation Failed: {ex.Message}");
            }
        }

        public static T? FromFile<T>(string path)
        {
            try
            {
                //Make a read type filestream
                using var filestream = new FileStream(path, FileMode.Open);

                //Make the serialiser and read the object from the file stream
                var serialiser = new DataContractSerializer(typeof(T));
                return (T)(serialiser.ReadObject(filestream) ?? throw new("The object initialised is null"));
            }
            catch (Exception ex)
            {
                Logger.Log(MessageType.Error, $"Deserialisation Failed: {ex.Message}");
                return default;
            }
        }
    }
}
