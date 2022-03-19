using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace Editor
{
    public static class Serialiser
    {
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var filestream = new FileStream(path, FileMode.Create);
                var serialiser = new DataContractSerializer(typeof(T));
                serialiser.WriteObject(filestream, instance);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static T? FromFile<T>(string path)
        {
            try
            {
                using var filestream = new FileStream(path, FileMode.Open);
                var serialiser = new DataContractSerializer(typeof(T));
                return (T?)serialiser.ReadObject(filestream);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
        }
    }
}
