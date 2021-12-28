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
///     A DateTime GenericRange Value containing the DateTime start and DateTime end recognized.
/// </summary>
internal class DateTimeV2DateTimeRange : DateTimeV2ObjectWithValue<ComparableRange<System.DateTime>>
{
    internal DateTimeV2DateTimeRange(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        if (!System.DateTime.TryParseExact(value["start"], "uuuu-MM-dd HH:mm:ss", null, default,
                out System.DateTime start))
        {
            throw new ArgumentException($"{value["start"]} is not of the format uuuu-MM-dd HH:mm:ss");
        }

        if (!System.DateTime.TryParseExact(value["end"], "uuuu-MM-dd HH:mm:ss", null, default,
                out System.DateTime end))
        {
            throw new ArgumentException($"{value["end"]} is not of the format uuuu-MM-dd HH:mm:ss");
        }

        this.Value = new ComparableRange<System.DateTime>(start, end);
    }
}