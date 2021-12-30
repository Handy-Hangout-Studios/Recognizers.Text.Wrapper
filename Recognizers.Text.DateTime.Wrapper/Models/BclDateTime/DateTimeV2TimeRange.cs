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
///     A DateTime GenericRange that contains a Start DateTime and an End DateTime, or uses a DateTimeModifier.
///     Do not use the Date component from either the Start or End DateTime.
///
///     If <see cref="TimeModifier"/> is <see cref="TimeModifier.Before"/> or <see cref="TimeModifier.Until"/> then
///     <see cref="DateTimeV2TimeRange.Value"/> has Start set to the minimum possible DateTime
///
///     If <see cref="TimeModifier"/> is <see cref="TimeModifier.Since"/> or <see cref="TimeModifier.After"/> then
///     <see cref="DateTimeV2TimeRange.Value"/> has End set to the maximum possible DateTime
/// </summary>
public class DateTimeV2TimeRange : DateTimeV2ObjectWithValue<DateTimeV2Range<System.DateTime, TimeModifier>>
{
    internal DateTimeV2TimeRange(IDictionary<String, String> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        bool parseStart = true;
        bool parseEnd = true;
        TimeModifier modifier = TimeModifier.None;
        if (value.ContainsKey("Mod"))
        {
            modifier = Enum.Parse<TimeModifier>(value["Mod"], true);
            switch (modifier)
            {
                case TimeModifier.None:
                    break;
                case TimeModifier.Before:
                case TimeModifier.Until:
                    parseStart = false;
                    break;
                case TimeModifier.Since:
                case TimeModifier.After:
                    parseEnd = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier), modifier,
                        $"{nameof(modifier)} was not a valid TimeModifier");
            }
        }

        System.DateTime start = System.DateTime.MinValue;
        if (parseStart && !System.DateTime.TryParseExact(value["start"], "HH:mm:ss", null, default,
                out start))
        {
            throw new ArgumentException($"start value {value["start"]} is not in the format HH:mm:ss");
        }

        System.DateTime end = System.DateTime.MaxValue;
        if (parseEnd && !System.DateTime.TryParseExact(value["end"], "HH:mm:ss", null, default,
                out end))
        {
            throw new ArgumentException($"end value {value["end"]} is not in the format HH:mm:ss");
        }

        this.Value = new DateTimeV2Range<System.DateTime, TimeModifier>(modifier, start, end);
    }
}