// Recognizers.Text.Wrapper - An API wrapper for the Microsoft.Recognizers.Text suite of recognizers
// Copyright (C) 2021  John Marsden
// 
// This program is free software:you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.If not, see <https://www.gnu.org/licenses/>.

using Recognizers.Text.DateTime.Wrapper.Models.Enums;
using Recognizers.Text.DateTime.Wrapper.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;

/// <summary>
/// An abstract class to simplify implementation of <see cref="IDateTimeV2ObjectFactory"/>s
/// </summary>
public abstract class DateTimeV2ObjectFactory : IDateTimeV2ObjectFactory
{
    DateTimeV2Object IDateTimeV2ObjectFactory.Create(DateTimeV2Type type, IDictionary<String, String> dict)
    {
        return type switch
        {
            DateTimeV2Type.Date => this.CreateDate(dict),
            DateTimeV2Type.DateRange => this.CreateDateRange(dict),
            DateTimeV2Type.DateTime => this.CreateDateTime(dict),
            DateTimeV2Type.DateTimeRange => this.CreateDateTimeRange(dict),
            DateTimeV2Type.Duration => this.CreateDuration(dict),
            DateTimeV2Type.Set => this.CreateSet(dict),
            DateTimeV2Type.Time => this.CreateTime(dict),
            DateTimeV2Type.TimeRange => this.CreateTimeRange(dict),
            object o => throw new ArgumentException($"{o} is not a recognized datetime V2 type"),
        };
    }

    protected abstract DateTimeV2Object CreateDate(IDictionary<String, String> dict);
    protected abstract DateTimeV2Object CreateDateRange(IDictionary<String, String> dict);
    protected abstract DateTimeV2Object CreateDateTime(IDictionary<String, String> dict);
    protected abstract DateTimeV2Object CreateDateTimeRange(IDictionary<String, String> dict);
    protected abstract DateTimeV2Object CreateDuration(IDictionary<String, String> dict);
    protected abstract DateTimeV2Object CreateSet(IDictionary<String, String> dict);
    protected abstract DateTimeV2Object CreateTime(IDictionary<String, String> dict);
    protected abstract DateTimeV2Object CreateTimeRange(IDictionary<String, String> dict);
}