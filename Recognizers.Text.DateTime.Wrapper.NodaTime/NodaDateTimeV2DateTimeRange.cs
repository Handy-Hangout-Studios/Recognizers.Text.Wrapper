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
///     A DateTime DateTimeV2Range Value containing the LocalDateTime start and LocalDateTime end recognized.
/// </summary>
internal class NodaDateTimeV2DateTimeRange : DateTimeV2ObjectWithValue<DateTimeV2Range<LocalDateTime, DateTimeModifier>>
{
    internal NodaDateTimeV2DateTimeRange(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        bool parseStart = true;
        bool parseEnd = true;
        DateTimeModifier modifier = DateTimeModifier.None;
        if (value.ContainsKey("Mod"))
        {
            modifier = Enum.Parse<DateTimeModifier>(value["Mod"], true);
            switch (modifier)
            {
                case DateTimeModifier.None:
                    break;
                case DateTimeModifier.Before:
                case DateTimeModifier.Until:
                    parseStart = false;
                    break;
                case DateTimeModifier.Since:
                case DateTimeModifier.After:
                    parseEnd = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier), modifier,
                        $"{nameof(modifier)} was not a valid DateTimeModifier");
            }
        }

        LocalDateTimePattern pattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu-MM-dd HH:mm:ss");

        LocalDateTime startDate = LocalDate.MinIsoValue + LocalTime.Midnight;
        if (parseStart)
        {
            ParseResult<LocalDateTime> startDateTimeParsed = pattern.Parse(value["start"]);
            if (!startDateTimeParsed.TryGetValue(LocalDate.MinIsoValue + LocalTime.MinValue, out startDate))
            {
                throw new ArgumentException(
                    $"Failed to parse the start value \"{value["start"]}\" with the format \"uuuu-MM-dd HH:mm:ss\"",
                    startDateTimeParsed.Exception);
            }
        }

        LocalDateTime endDate = LocalDate.MaxIsoValue + LocalTime.Midnight;
        if (parseEnd)
        {
            ParseResult<LocalDateTime> endDateTimeParsed = pattern.Parse(value["end"]);
            if (!endDateTimeParsed.TryGetValue(LocalDate.MinIsoValue + LocalTime.MinValue, out endDate))
            {
                throw new ArgumentException(
                    $"Failed to parse the end value \"{value["end"]}\" with the format \"uuuu-MM-dd HH:mm:ss\"",
                    endDateTimeParsed.Exception);
            }
        }

        this.Value = new DateTimeV2Range<LocalDateTime, DateTimeModifier>(modifier, startDate, endDate);
    }
}