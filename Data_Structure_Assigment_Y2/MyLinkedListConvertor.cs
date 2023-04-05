using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * This code defines a class named MyLinkedListConverter<T>, which is a custom JSON converter for the MyLinkedList<T> class where T is a generic type derived from the Book class. 
 * The MyLinkedListConverter<T> class inherits from the JsonConverter<MyLinkedList<T>> class provided by the Json.NET library.
 * The purpose of this converter is to define how to serialize and deserialize MyLinkedList<T> objects to and from JSON format. 
 * It is particularly useful when using Json.NET for handling JSON data containing MyLinkedList<T> objects.
 */

public class MyLinkedListConverter<T> : JsonConverter<MyLinkedList<T>> where T : Book
{
    public override MyLinkedList<T> ReadJson(JsonReader reader, Type objectType, MyLinkedList<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        // Load the JSON data into a JArray
        JArray array = JArray.Load(reader);

        // Create a new MyLinkedList<T> object
        MyLinkedList<T> list = new MyLinkedList<T>();

        // Add each element from the JArray to the list
        foreach (JToken token in array)
        {
            T value = token.ToObject<T>();
            list.AddLast(value);
        }

        return list;
    }

    public override void WriteJson(JsonWriter writer, MyLinkedList<T> value, JsonSerializer serializer)
    {
        // Create a new JArray from the MyLinkedList<T> object
        JArray array = new JArray();
        foreach (T item in value)
        {
            JToken token = JToken.FromObject(item);
            array.Add(token);
        }

        // Write the JArray to the JSON writer
        array.WriteTo(writer);
    }


}
