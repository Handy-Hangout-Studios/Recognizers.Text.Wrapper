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
    // ReSharper disable twice UseDeconstructionOnParameter
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
    ///     <see cref="DateTimeV2Recognizer.RecognizeDateTimes{T}(string, string, System.DateTime?, ISet{DateTimeV2Type}?)" />
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
    /// <param name="factoryType">The factory type to use to produce appropriate .NET objects for each of the
    ///     <see cref="DateTimeV2Type" />s. The factory used will define what types you will be working with.
    ///     - Defaults to an object factory that produces .NET DateTime objects</param>
    // ReSharper disable once MemberCanBePrivate.Global
    protected DateTimeV2Recognizer(string culture = Culture.English, Type? factoryType = null)
    {
        this._model = new DateTimeRecognizer(culture).GetDateTimeModel();
        this._factory = (IDateTimeV2ObjectFactory)Activator.CreateInstance(factoryType ?? typeof(BclDateTimeV2ObjectFactory))!;
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
    // ReSharper disable once MemberCanBePrivate.Global
    public IEnumerable<DateTimeV2Model> RecognizeDateTimes(string content, System.DateTime? refTime = null,
        ISet<DateTimeV2Type>? typeFilter = null)
    {
        List<ModelResult> modelResults =
            !refTime.HasValue ? this._model.Parse(content) : this._model.Parse(content, refTime.Value);

        IEnumerable<DateTimeV2Model> transformedModels = modelResults
            .Select(mr => DateTimeV2Model.Create(mr, this._factory))
            .Where(dtm => dtm is not null)!;

        return typeFilter is null ? transformedModels : transformedModels.Where(dtm => typeFilter.Contains(dtm.Type));
    }

    /// <summary>
    ///     Use the Microsoft <see cref="Microsoft.Recognizers.Text.DateTime.DateTimeRecognizer" /> to parse out any
    ///     datetimes in the content string and then return them as .NET objects using the factory that was provided or the
    ///     cached recognizer for the culture and factory type passed in if those have been used before - This overload
    ///     uses the <see cref="BclDateTimeV2ObjectFactory"/> which produces default .NET DateTime like objects.
    /// </summary>
    /// <param name="content">The string to extract datetimes from</param>
    /// <param name="culture">
    ///     The culture to use when parsing - Also used for caching recognizers - Defaults to English
    /// </param>
    /// <param name="refTime">The time to use as the current time when parsing</param>
    /// <param name="typeFilter">A set containing the <see cref="DateTimeV2Type"/>s of <see cref="Recognizers.Text.DateTime.Wrapper.Models.BaseClasses.DateTimeV2Object"/>s to return</param>
    /// <returns></returns>
    public static IEnumerable<DateTimeV2Model> RecognizeDateTimes(
        string content,
        string culture = Culture.English,
        System.DateTime? refTime = null,
        ISet<DateTimeV2Type>? typeFilter = null
    ) => RecognizeDateTimes<BclDateTimeV2ObjectFactory>(content, culture, refTime, typeFilter);
    
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
    /// <param name="typeFilter">A set containing the <see cref="DateTimeV2Type"/>s of <see cref="Recognizers.Text.DateTime.Wrapper.Models.BaseClasses.DateTimeV2Object"/>s to return</param>
    /// <typeparam name="TFactory">The type of factory to use in producing the .NET objects appropriate for each
    /// <see cref="DateTimeV2Type"/>.</typeparam>
    /// <returns></returns>
    public static IEnumerable<DateTimeV2Model> RecognizeDateTimes<TFactory>(
        string content,
        string culture = Culture.English,
        System.DateTime? refTime = null,
        ISet<DateTimeV2Type>? typeFilter = null
    )
    {
        DateTimeV2Recognizer recognizer =
            Cached.GetOrAdd((culture, typeof(TFactory)), e =>
                new DateTimeV2Recognizer<TFactory>(e.culture)
            );

        return recognizer.RecognizeDateTimes(content, refTime, typeFilter);
    }
}

/// <summary>
/// An object used to parse <see cref="DateTimeV2Model"/>s from a content sentence
/// </summary>
/// <typeparam name="TFactory">The type of factory to use in producing the .NET objects appropriate for each
/// <see cref="DateTimeV2Type"/>.</typeparam>
public class DateTimeV2Recognizer<TFactory> : DateTimeV2Recognizer
{
    public DateTimeV2Recognizer(string culture) : base(culture, typeof(TFactory))
    {
        
    }
}