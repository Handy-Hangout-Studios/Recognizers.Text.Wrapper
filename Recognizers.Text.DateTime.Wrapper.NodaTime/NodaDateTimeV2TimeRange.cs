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

using NodaTime;
using NodaTime.Text;
using Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;
using Recognizers.Text.DateTime.Wrapper.Models.Modifiers;
using Recognizers.Text.DateTime.Wrapper.Models.Range;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.NodaTime;

/// <summary>
/// </summary>
public class NodaDateTimeV2TimeRange : DateTimeV2ObjectWithValue<DateTimeV2Range<LocalTime, TimeModifier>>
{
    internal NodaDateTimeV2TimeRange(IDictionary<String, String> value) : base(value) { }

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

        LocalTimePattern pattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss");

        LocalTime startTime = LocalTime.MinValue;
        if (parseStart)
        {
            ParseResult<LocalTime> startTimeParsed = pattern.Parse(value["start"]);
            if (!startTimeParsed.TryGetValue(LocalTime.MinValue, out startTime))
            {
                throw new ArgumentException(
                    $"Failed to parse start value \"{value["start"]}\" with the format \"HH:mm:ss\"",
                    startTimeParsed.Exception);
            }
        }

        LocalTime endTime = LocalTime.MaxValue;
        if (parseEnd)
        {
            ParseResult<LocalTime> endTimeParsed = pattern.Parse(value["end"]);
            if (!endTimeParsed.TryGetValue(LocalTime.MinValue, out endTime))
            {
                throw new ArgumentException(
                    $"Failed to parse end value \"{value["end"]}\" with the format \"HH:mm:ss\"",
                    endTimeParsed.Exception);
            }
        }

        this.Value = new DateTimeV2Range<LocalTime, TimeModifier>(modifier, startTime, endTime);
    }
}