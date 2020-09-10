using Demo.Common.Extensions.Grid;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.Common.Converters
{

    public class DataSourceSortConverter : JsonConverter<List<Sort>>
    {
        public override List<Sort> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) {

            return new List<Sort>();
        }


        public override void Write(Utf8JsonWriter writer, List<Sort> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

}

