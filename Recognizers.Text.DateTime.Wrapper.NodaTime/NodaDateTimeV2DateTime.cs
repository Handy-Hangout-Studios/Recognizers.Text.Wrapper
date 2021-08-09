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
    /// <summary>
    /// A DateTime Value containing the LocalDateTime recognized.
    /// </summary>
    public class NodaDateTimeV2DateTime : DateTimeV2ObjectWithValue<LocalDateTime>
    {
        internal NodaDateTimeV2DateTime(IDictionary<string, string> value) : base(value) { }

        protected override void InitializeValue(IDictionary<string, string> value)
        {
            LocalDateTimePattern pattern = LocalDateTimePattern.CreateWithInvariantCulture("uuuu-MM-dd HH:mm:ss");
            ParseResult<LocalDateTime> dateTimeParsed = pattern.Parse(value["value"]);
            if (!dateTimeParsed.TryGetValue(LocalDate.MinIsoValue + LocalTime.MinValue, out LocalDateTime result))
            {
                throw new ArgumentException($"Failed to parse the value \"{value["value"]}\" with the format \"uuuu-MM-dd HH:mm:ss\"", dateTimeParsed.Exception);
            }
            this.Value = result;
        }
    }
}
