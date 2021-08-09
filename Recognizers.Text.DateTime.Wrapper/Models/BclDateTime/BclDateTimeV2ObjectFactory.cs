﻿// Recognizers.Text.Wrapper - An API wrapper for the Microsoft.Recognizers.Text suite of recognizers
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
using Recognizers.Text.DateTime.Wrapper.Models.Enums;
using Recognizers.Text.DateTime.Wrapper.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BclDateTime
{
    public sealed class BclDateTimeV2ObjectFactory : IDateTimeV2ObjectFactory
    {
        public DateTimeV2Object Create(DateTimeV2Type type, IDictionary<String, String> dict)
            => type switch
            {
                DateTimeV2Type.Date => new DateTimeV2Date(dict),
                DateTimeV2Type.DateRange => new DateTimeV2DateRange(dict),
                DateTimeV2Type.DateTime => new DateTimeV2DateTime(dict),
                DateTimeV2Type.DateTimeRange => new DateTimeV2DateTimeRange(dict),
                DateTimeV2Type.Duration => new DateTimeV2Duration(dict),
                DateTimeV2Type.Set => new DateTimeV2Set(dict),
                DateTimeV2Type.Time => new DateTimeV2Time(dict),
                DateTimeV2Type.TimeRange => new DateTimeV2TimeRange(dict),
                object o => throw new ArgumentException($"{o} is not a recognized datetime V2 type")
            };
    }
}