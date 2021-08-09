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

namespace Recognizers.Text.DateTime.Wrapper.Models.BclDateTime
{
    /// <summary>
    /// A DateTime GenericRange with both a Start and End DateTime that contain the start Date and end Date recognized. 
    /// Do not use the Time component of Start or End DateTimes.
    /// </summary>
    public class DateTimeV2DateRange : DateTimeV2ObjectWithValue<ComparableRange<System.DateTime>>
    {
        internal DateTimeV2DateRange(IDictionary<string, string> value) : base(value) { }

        protected override void InitializeValue(IDictionary<String, String> value)
        {
            if (!System.DateTime.TryParseExact(value["start"], "uuuu-MM-dd", null, default, out System.DateTime start))
            {
                throw new ArgumentException($"start value {value["start"]} is not in the format uuuu-MM-dd");
            }


            if (!System.DateTime.TryParseExact(value["start"], "uuuu-MM-dd", null, default, out System.DateTime end))
            {
                throw new ArgumentException($"end value {value["end"]} is not in the format uuuu-MM-dd");
            }

            this.Value = new ComparableRange<System.DateTime>(start, end);
        }
    }
}
