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
using Recognizers.Text.DateTime.Wrapper.Models.Generics;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;

/// <summary>
///     A DateTime GenericRange that contains a Start DateTime and an End DateTime. Do not use the Date component from
///     either the Start or End DateTime.
/// </summary>
public class DateTimeV2TimeRange : DateTimeV2ObjectWithValue<ComparableRange<System.DateTime>>
{
    internal DateTimeV2TimeRange(IDictionary<String, String> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        if (!System.DateTime.TryParseExact(value["start"], "HH:mm:ss", null, default,
                out System.DateTime start))
        {
            throw new ArgumentException($"start value {value["start"]} is not in the format HH:mm:ss");
        }

        if (!System.DateTime.TryParseExact(value["end"], "HH:mm:ss", null, default,
                out System.DateTime end))
        {
            throw new ArgumentException($"end value {value["end"]} is not in the format HH:mm:ss");
        }

        this.Value = new ComparableRange<System.DateTime>(start, end);
    }
}