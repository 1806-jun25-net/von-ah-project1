using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Project1.Library.Models
{
    public static class Serialization
    {
        public static void SerializeToFile(string fileName, List<Order> OrderHistory)
        {
            var serializer = new XmlSerializer(typeof(List<Order>));
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(fileName, FileMode.Create); //this creates a new file everytime. change?
                serializer.Serialize(fileStream, OrderHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                fileStream?.Dispose(); //checks if filestream is null
            }
            
        }
        public static List<Order> DeserializeFromFile(string fileName)
        {
            var serializer = new XmlSerializer(typeof(List<Order>));
            // we CAN do try/finally like this, but the using statement is easier
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            try
            { 

                var result = (List<Order>)serializer.Deserialize(fileStream);
                return result;
            }
            finally
            {
                fileStream.Dispose();
            }
        }
    }


    
}
