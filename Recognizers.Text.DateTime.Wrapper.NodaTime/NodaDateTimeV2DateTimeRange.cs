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
using Recognizers.Text.DateTime.Wrapper.Models.Generics;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.NodaTime;

/// <summary>
///     A DateTime DateTimeV2Range Value containing the LocalDateTime start and LocalDateTime end recognized.
/// </summary>
internal class NodaDateTimeV2DateTimeRange : DateTimeV2ObjectWithValue<ComparableRange<LocalDateTime>>
{
    internal NodaDateTimeV2DateTimeRange(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        LocalDateTimePattern pattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu-MM-dd HH:mm:ss");
        ParseResult<LocalDateTime> startDateTimeParsed = pattern.Parse(value["start"]);
        if (!startDateTimeParsed.TryGetValue(LocalDate.MinIsoValue + LocalTime.MinValue, out LocalDateTime startDate))
        {
            throw new ArgumentException(
                $"Failed to parse the start value \"{value["start"]}\" with the format \"uuuu-MM-dd HH:mm:ss\"",
                startDateTimeParsed.Exception);
        }

        ParseResult<LocalDateTime> endDateTimeParsed = pattern.Parse(value["end"]);
        if (!endDateTimeParsed.TryGetValue(LocalDate.MinIsoValue + LocalTime.MinValue, out LocalDateTime endDate))
        {
            throw new ArgumentException(
                $"Failed to parse the end value \"{value["end"]}\" with the format \"uuuu-MM-dd HH:mm:ss\"",
                endDateTimeParsed.Exception);
        }

        this.Value = new ComparableRange<LocalDateTime>(startDate, endDate);
    }
}