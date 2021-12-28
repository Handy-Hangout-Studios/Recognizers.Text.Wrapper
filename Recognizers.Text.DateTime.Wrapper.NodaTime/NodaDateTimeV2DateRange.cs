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
///     A DateTime <see cref="ComparableRange{TRangeType}" /> with both a Start and End LocalDate that contain the start
///     LocalDate \
///     and end LocalDate recognized.
/// </summary>
public class NodaDateTimeV2DateRange : DateTimeV2ObjectWithValue<ComparableRange<LocalDate>>
{
    internal NodaDateTimeV2DateRange(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<string, string> value)
    {
        LocalDatePattern pattern = LocalDatePattern.CreateWithInvariantCulture("uuuu-MM-dd");
        ParseResult<LocalDate> startDateParsed = pattern.Parse(value["start"]);
        if (!startDateParsed.TryGetValue(LocalDate.MinIsoValue, out LocalDate startDate))
        {
            throw new ArgumentException(
                $"Failed to parse the start value \"{value["start"]}\" with the expected format \"uuuu-MM-dd\"",
                startDateParsed.Exception);
        }

        ParseResult<LocalDate> endDateParsed = pattern.Parse(value["end"]);
        if (!endDateParsed.TryGetValue(LocalDate.MinIsoValue, out LocalDate endDate))
        {
            throw new ArgumentException(
                $"Failed to parse the end value \"{value["end"]}\" with the expected format \"uuuu-MM-dd\"",
                endDateParsed.Exception);
        }


        this.Value = new ComparableRange<LocalDate>(startDate, endDate);
    }
}