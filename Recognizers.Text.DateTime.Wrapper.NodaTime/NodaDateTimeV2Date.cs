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
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.NodaTime;

/// <summary>
///     A DateTime Value containing only the LocalDate that was recognized
/// </summary>
public class NodaDateTimeV2Date : DateTimeV2ObjectWithValue<LocalDate>
{
    internal NodaDateTimeV2Date(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<string, string> value)
    {
        ParseResult<LocalDate> dateParsed =
            LocalDatePattern.CreateWithInvariantCulture("uuuu-MM-dd").Parse(value["value"]);
        if (!dateParsed.TryGetValue(LocalDate.MinIsoValue, out LocalDate result))
        {
            throw new ArgumentException($"Failed to parse the value \"{value["value"]}\" with the format uuuu-MM-dd",
                dateParsed.Exception);
        }

        this.Value = result;
    }
}