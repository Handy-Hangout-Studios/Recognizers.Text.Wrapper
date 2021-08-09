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
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.NodaTime
{
    public class NodaDateTimeV2TimeRange : DateTimeV2ObjectWithValue<DateTimeV2Range<LocalTime>>
    {
        internal NodaDateTimeV2TimeRange(IDictionary<String, String> value) : base(value) { }

        protected override void InitializeValue(IDictionary<String, String> value)
        {
            LocalTimePattern pattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss");
            ParseResult<LocalTime> startTimeParsed = pattern.Parse(value["start"]);
            if (!startTimeParsed.TryGetValue(LocalTime.MinValue, out LocalTime startTime))
            {
                throw new ArgumentException($"Failed to parse start value \"{value["start"]}\" with the format \"HH:mm:ss\"", startTimeParsed.Exception);
            }

            ParseResult<LocalTime> endTimeParsed = pattern.Parse(value["end"]);
            if (!endTimeParsed.TryGetValue(LocalTime.MinValue, out LocalTime endTime))
            {
                throw new ArgumentException($"Failed to parse end value \"{value["end"]}\" with the format \"HH:mm:ss\"", endTimeParsed.Exception);
            }

            this.Value = new DateTimeV2Range<LocalTime>(startTime, endTime);
        }
    }
}
