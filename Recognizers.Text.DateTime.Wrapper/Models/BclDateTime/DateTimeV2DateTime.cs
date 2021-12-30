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
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;

/// <summary>
///     A DateTime Value containing the DateTime recognized.
/// </summary>
public class DateTimeV2DateTime : DateTimeV2ObjectWithValue<System.DateTime>
{
    internal DateTimeV2DateTime(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        if (!System.DateTime.TryParseExact(value["value"], "yyyy-MM-dd HH:mm:ss", null, default,
                out System.DateTime result))
        {
            throw new ArgumentException($"value {value["value"]} is not of the format yyyy-MM-dd HH:mm:ss");
        }

        this.Value = result;
    }
}