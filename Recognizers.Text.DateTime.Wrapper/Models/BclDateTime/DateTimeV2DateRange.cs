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

using Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;
using Recognizers.Text.DateTime.Wrapper.Models.Modifiers;
using Recognizers.Text.DateTime.Wrapper.Models.Range;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;

/// <summary>
///     A DateTime GenericRange with both a Start and End DateTime that contain the start Date and end Date recognized.
///     Do not use the Time component of Start or End DateTimes.
/// </summary>
public class DateTimeV2DateRange : DateTimeV2ObjectWithValue<DateTimeV2Range<System.DateTime, DateModifier>>
{
    internal DateTimeV2DateRange(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        bool parseStart = true;
        bool parseEnd = true;
        DateModifier modifier = DateModifier.None;
        if (value.ContainsKey("Mod"))
        {
            modifier = Enum.Parse<DateModifier>(value["Mod"], true);
            switch (modifier)
            {
                case DateModifier.None:
                    break;
                case DateModifier.Before:
                case DateModifier.Until:
                    parseStart = false;
                    break;
                case DateModifier.Since:
                case DateModifier.After:
                    parseEnd = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier), modifier,
                        $"{nameof(modifier)} was not a valid DateModifier");
            }
        }

        System.DateTime start = System.DateTime.MinValue;
        if (parseStart && !System.DateTime.TryParseExact(value["start"], "yyyy-MM-dd", null, default,
                out start))
        {
            throw new ArgumentException($"start value {value["start"]} is not in the format yyyy-MM-dd");
        }

        System.DateTime end = System.DateTime.MaxValue;
        if (parseEnd && !System.DateTime.TryParseExact(value["end"], "yyyy-MM-dd", null, default,
                out end))
        {
            throw new ArgumentException($"end value {value["end"]} is not in the format yyyy-MM-dd");
        }

        this.Value = new DateTimeV2Range<System.DateTime, DateModifier>(modifier, start, end);
    }
}