//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Demo.Helpers.Converters
//{
//    public class DataSourceSortConverter : Newtonsoft.Json.JsonConverter
//    {
//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            var token = JToken.Load(reader);
//            var list = Activator.CreateInstance(objectType) as System.Collections.IList;
//            var itemType = objectType.GenericTypeArguments[0];
//            foreach (var child in token.Children())  //mod here
//            {
//                var newObject = Activator.CreateInstance(itemType);
//                serializer.Populate(child.CreateReader(), newObject); //mod here
//                list.Add(newObject);
//            }
//            return list;
//        }

//        public override bool CanConvert(Type objectType)
//        {

//            if (!objectType.IsGenericType)
//                return false;

//            var type = objectType;

//            if (!type.IsGenericTypeDefinition)
//                type = type.GetGenericTypeDefinition();

//            return type == typeof(List<>);
//        }

//        public override bool CanWrite => false;
//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

//    }
//}

