// Recognizers.Text.Wrapper - An API wrapper for the Microsoft.Recognizers.Text suite of recognizers
// Copyright (C) 2021  John Marsden
// 
// This program is free software:you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Recognizers.Text;
using NUnit.Framework;
using Recognizers.Text.DateTime.Wrapper;
using Recognizers.Text.DateTime.Wrapper.Models;
using Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;
using Recognizers.Text.DateTime.Wrapper.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestProject1.DateTimeV2;

[TestFixture]
public class MicrosoftTests
{
    private static JsonSerializerOptions SerializerOptions => _serializerOptions ??= new JsonSerializerOptions
    {
        Converters = {new MicrosoftDataConverter()},
    };

    private static JsonSerializerOptions? _serializerOptions;

    [Test]
    public void TestDeserializeMicrosoftData()
    {
        IDateTimeV2ObjectFactory factory = new BclDateTimeV2ObjectFactory();
        const string json = "[{\"Input\":\"\\\"Within 3 years\\\", he said this 5 years ago.\",\"Context\":{\"ReferenceDateTime\":\"2018-03-14T00:00:00\"},\"NotSupported\":\"javascript, python\",\"Results\":[{\"Text\":\"Within 3 years\",\"Type\":\"datetimeV2.daterange\",\"Value\":{\"values\":[{\"timex\":\"(2018-03-14,2021-03-14,P3Y)\",\"type\":\"daterange\",\"start\":\"2018-03-14\",\"end\":\"2021-03-14\"}]},\"Start\":1,\"Length\":14},{\"Text\":\"5 years ago\",\"Start\":31,\"Type\":\"datetimeV2.date\",\"Value\":{\"values\":[{\"timex\":\"2013-03-14\",\"type\":\"date\",\"value\":\"2013-03-14\"}]},\"Length\":11}]}]";
        List<MicrosoftData>? data = JsonSerializer.Deserialize<List<MicrosoftData>>(json, SerializerOptions);
        Debug.Assert(data != null, nameof(data) + " != null");
        var expected = new
        {
            Input = "\"Within 3 years\", he said this 5 years ago.",
            RefTime = DateTime.Parse("2018-03-14T00:00:00", CultureInfo.GetCultureInfo(Culture.English)),
            FirstModelResult = new ModelResult
            {
                Text = "Within 3 years",
                TypeName = "datetimeV2.daterange",
                Start = 1,
                End = 14,
                Resolution = new SortedDictionary<string, object>
                {
                    {
                        "values", new List<Dictionary<string, string>>
                        {
                            new()
                            {
                                {"timex", "(2018-03-14,2021-03-14,P3Y)"},
                                {"type", "daterange"},
                                {"start", "2018-03-14"},
                                {"end", "2021-03-14"},
                            },
                        }
                    },
                },
            },
            SecondModelResult = new ModelResult
            {
                Text = "5 years ago",
                TypeName = "datetimeV2.date",
                Start = 31,
                End = 41,
                Resolution = new SortedDictionary<string, object>
                {
                    {
                        "values",
                        new List<Dictionary<string, string>>
                        {
                            new() {{"timex", "2013-03-14"}, {"type", "date"}, {"value", "2013-03-14"}},
                        }
                    },
                },
            },
        };
        Assert.AreEqual(data[0].Input, expected.Input);
        Assert.AreEqual(data[0].RefTime, expected.RefTime);
        Assert.AreEqual(data[0].Expected[0], DateTimeV2Model.Create(expected.FirstModelResult, factory));
        Assert.AreEqual(data[0].Expected[1], DateTimeV2Model.Create(expected.SecondModelResult, factory));
    }

    [Test]
    [TestCaseSource(nameof(GetMicrosoftData))]
    public void TestParserVsMicrosoftData(MicrosoftData data)
    {
        (String? input, DateTime dateTime, List<DateTimeV2Model>? dateTimeV2Models) = data;
        IEnumerable<DateTimeV2Model> results =
            DateTimeV2Recognizer.RecognizeDateTimes(input, refTime: dateTime);

        foreach ((DateTimeV2Model first, DateTimeV2Model second) in dateTimeV2Models.Zip(results))
        {
            Assert.AreEqual(first, second);
        }
    }


    public record MicrosoftData(string Input, DateTime RefTime, List<DateTimeV2Model> Expected);

    public static IEnumerable<MicrosoftData> GetMicrosoftData()
    {
        using HttpClient httpClient = new();
        HttpResponseMessage response = httpClient
            .GetAsync(
                "https://raw.githubusercontent.com/microsoft/Recognizers-Text/master/Specs/DateTime/English/MergedParser.json")
            .Result;

        string jsonSpec = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<List<MicrosoftData>>(jsonSpec, SerializerOptions) ??
               throw new JsonException("Deserialization failed to return any value");
    }

    private class MicrosoftDataConverter : JsonConverter<List<MicrosoftData>>
    {
        private static readonly IDateTimeV2ObjectFactory Factory = new BclDateTimeV2ObjectFactory();

        public override List<MicrosoftData> Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            List<MicrosoftData> data = new();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return data;
                }

                reader.Read();
                // Read the Input value
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string? propertyName = reader.GetString();
                if (propertyName != "Input")
                {
                    throw new JsonException();
                }

                reader.Read();
                string input = reader.GetString() ?? throw new JsonException("Should not be null Input");

                reader.Read();
                // Read the Reference DateTime
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException(input);
                }

                do
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        propertyName = reader.GetString();
                        reader.Read();
                    }
                    else
                    {
                        reader.Read();
                    }
                } while (propertyName != "Context");

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException(input);
                }

                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException(input);
                }

                reader.Read();
                DateTime refTime = reader.GetDateTime();

                reader.Read();
                if (reader.TokenType != JsonTokenType.EndObject)
                {
                    throw new JsonException(input);
                }

                reader.Read();

                do
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        propertyName = reader.GetString();
                        reader.Read();
                    }
                    else
                    {
                        reader.Read();
                    }
                } while (propertyName != "Results");


                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException(input);
                }

                reader.Read();
                List<ModelResult> modelResults = new();
                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    reader.Read();
                    Dictionary<string, object> values = new();
                    while (reader.TokenType != JsonTokenType.EndObject)
                    {
                        values[reader.GetString() ?? throw new JsonException(input)] =
                            (reader.Read() ? reader.TokenType : throw new JsonException()) switch
                            {
                                JsonTokenType.String => reader.GetString(),
                                JsonTokenType.Number => reader.GetInt32(),
                                JsonTokenType.StartObject => (object)GetValues(ref reader, input),
                                _ => throw new JsonException(input),
                            } ?? throw new JsonException(input);
                        reader.Read();
                    }

                    modelResults.Add(new ModelResult
                    {
                        Start = (int)values["Start"],
                        End = (int)values["Start"] + (int)values["Length"] - 1,
                        Text = (string)values["Text"],
                        TypeName = (string)values["Type"],
                        Resolution = new SortedDictionary<string, object>
                        {
                            {"values", (List<Dictionary<string, string>>)values["Value"]},
                        },
                    });
                    reader.Read();
                }

                data.Add(new MicrosoftData(
                        input,
                        refTime,
                        modelResults
                            .Select(mr => DateTimeV2Model.Create(mr, Factory))
                            .ToList()!
                    )
                );
                reader.Read();
            }

            throw new JsonException();
        }

        private static List<Dictionary<string, string>> GetValues(ref Utf8JsonReader reader, string input)
        {
            List<Dictionary<string, string>> temp = new();
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException(input);
            }

            string propertyName = reader.GetString() ?? throw new JsonException();
            if (propertyName != "values")
            {
                throw new JsonException(input);
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException(input);
            }

            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                Dictionary<string, string> newObj = new();
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException(input);
                }

                reader.Read();
                while (reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException(input);
                    }

                    string key = reader.GetString() ?? throw new JsonException(input);
                    reader.Read();
                    newObj[key] = reader.GetString() ?? throw new JsonException(input);
                    reader.Read();
                }

                temp.Add(newObj);
                reader.Read();
            }

            reader.Read();
            return temp;
        }

        public override void Write(Utf8JsonWriter writer, List<MicrosoftData> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}