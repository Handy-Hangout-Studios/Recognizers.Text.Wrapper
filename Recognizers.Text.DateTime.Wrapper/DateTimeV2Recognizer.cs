// Recognizers.Text.Wrapper - An API wrapper for the Microsoft.Recognizers.Text suite of recognizers
// Copyright (C) 2021  John Marsden
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Recognizers.Text.DateTime.Wrapper.Models;
using Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;
using Recognizers.Text.DateTime.Wrapper.Models.Enums;
using Recognizers.Text.DateTime.Wrapper.Models.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Recognizers.Text.DateTime.Wrapper;

internal class StringTypeComparer : IEqualityComparer<(string culture, Type type)>
{
    /// <summary>
    ///     Check if two string type value tuples are equal
    /// </summary>
    /// <param name="first">The first string type value tuple</param>
    /// <param name="second">The second string type value tuple</param>
    /// <returns>Whether the two string type value tuples are equal</returns>
    public bool Equals((string culture, Type type) first, (string culture, Type type) second)
    {
        return first.culture == second.culture && first.type == second.type;
    }

    /// <summary>
    ///     Generate a hash code for the provided string type value tuple
    /// </summary>
    /// <param name="obj">The string type value tuple to produce a hash code for</param>
    /// <returns>An integer hash code for the string type value tuple</returns>
    public int GetHashCode((string culture, Type type) obj)
    {
        return HashCode.Combine(obj.culture, obj.type.FullName);
    }
}

public class DateTimeV2Recognizer
{
    /// <summary>
    ///     A cache for all <see cref="DateTimeV2Recognizer" />s of each culture and type that are used in the static method
    ///     <see cref="DateTimeV2Recognizer.RecognizeDateTimes(string, string, System.DateTime?, Type?, ISet{DateTimeV2Type}?)" />
    ///     to help reduce costs of producing the recognizers repeatedly.
    /// </summary>
    private static readonly ConcurrentDictionary<(string culture, Type type), DateTimeV2Recognizer> Cached =
        new(new StringTypeComparer());

    /// <summary>
    ///     The factory used to produce appropriate .NET objects for each of the <see cref="DateTimeV2Type" />s.
    /// </summary>
    private readonly IDateTimeV2ObjectFactory _factory;

    /// <summary>
    ///     A <see cref="DateTimeModel" /> from the Microsoft Recognizers library that can be used to parse a string of
    ///     content
    /// </summary>
    private readonly DateTimeModel _model;

    /// <summary>
    ///     Create a DateTimeV2Recognizer that can be used to parse different <see cref="DateTimeV2Type" />s from a string
    /// </summary>
    /// <param name="culture">The culture to use when parsing - Defaults to English</param>
    /// <param name="factory">
    ///     A factory used to produce appropriate .NET objects for each of the
    ///     <see cref="DateTimeV2Type" />s. The factory used will define what types you will be working with.
    ///     - Defaults to an object factory that produces .NET DateTime objects
    /// </param>
    public DateTimeV2Recognizer(string culture = Culture.English, IDateTimeV2ObjectFactory? factory = null)
    {
        this._model = new DateTimeRecognizer(culture).GetDateTimeModel();
        this._factory = factory ?? new BclDateTimeV2ObjectFactory();
    }

    /// <summary>
    ///     Use the Microsoft <see cref="Microsoft.Recognizers.Text.DateTime.DateTimeRecognizer" /> to parse out any
    ///     datetimes in the content string and then return them as .NET objects using the factory that was provided to this
    ///     <see cref="DateTimeV2Recognizer" />.
    /// </summary>
    /// <param name="content">The string to extract datetimes from</param>
    /// <param name="refTime">The time to use as the current time when parsing</param>
    /// <param name="typeFilter">The types of <see cref="DateTimeV2Type" />s to return</param>
    /// <returns>An enumerable of datetimes parsed from the content string</returns>
    public IEnumerable<DateTimeV2Model> RecognizeDateTimes(string content, System.DateTime? refTime = null,
        ISet<DateTimeV2Type>? typeFilter = null)
    {
        List<ModelResult> modelResults =
            !refTime.HasValue ? this._model.Parse(content) : this._model.Parse(content, refTime.Value);

        if (typeFilter is null)
        {
            return modelResults
                .Select(mr => new DateTimeV2Model(mr, this._factory));
        }

        return modelResults
            .Select(mr => new DateTimeV2Model(mr, this._factory))
            .Where(dtm => typeFilter.Contains(dtm.Type));
    }

    /// <summary>
    ///     Use the Microsoft <see cref="Microsoft.Recognizers.Text.DateTime.DateTimeRecognizer" /> to parse out any
    ///     datetimes in the content string and then return them as .NET objects using the factory that was provided or the
    ///     cached recognizer for the culture and factory type passed in if those have been used before
    /// </summary>
    /// <param name="content">The string to extract datetimes from</param>
    /// <param name="culture">
    ///     The culture to use when parsing - Also used for caching recognizers - Defaults to English
    /// </param>
    /// <param name="refTime">The time to use as the current time when parsing</param>
    /// <param name="factory">
    ///     The type of factory to use to produce .NET objects from the results extracted by the
    ///     Microsoft <see cref="Microsoft.Recognizers.Text.DateTime.DateTimeRecognizer" /> - Defaults to using the
    ///     <see cref="BclDateTimeV2ObjectFactory" /> type of factory
    /// </param>
    /// <param name="typeFilter"></param>
    /// <returns></returns>
    public static IEnumerable<DateTimeV2Model> RecognizeDateTimes(
        string content,
        string culture = Culture.English,
        System.DateTime? refTime = null,
        Type? factory = null,
        ISet<DateTimeV2Type>? typeFilter = null
    )
    {
        Type type = factory ?? typeof(BclDateTimeV2ObjectFactory);
        DateTimeV2Recognizer recognizer =
            Cached.GetOrAdd((culture, type), e =>
                new DateTimeV2Recognizer(e.culture, (IDateTimeV2ObjectFactory)Activator.CreateInstance(type)!)
            );

        return recognizer.RecognizeDateTimes(content, refTime, typeFilter);
    }
}