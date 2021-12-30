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
using Recognizers.Text.DateTime.Wrapper.Models.Enums;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;

/// <summary>
///     A factory to produce the appropriate BCL DateTime .NET objects for each <see cref="DateTimeV2Type" />.
/// </summary>
public sealed class BclDateTimeV2ObjectFactory : DateTimeV2ObjectFactory
{
    protected override DateTimeV2Object CreateDate(IDictionary<String, String> dict)
    {
        return new DateTimeV2Date(dict);
    }

    protected override DateTimeV2Object CreateDateRange(IDictionary<String, String> dict)
    {
        return new DateTimeV2DateRange(dict);
    }

    protected override DateTimeV2Object CreateDateTime(IDictionary<String, String> dict)
    {
        return new DateTimeV2DateTime(dict);
    }

    protected override DateTimeV2Object CreateDateTimeRange(IDictionary<String, String> dict)
    {
        return new DateTimeV2DateTimeRange(dict);
    }

    protected override DateTimeV2Object CreateDuration(IDictionary<String, String> dict)
    {
        return new DateTimeV2Duration(dict);
    }

    protected override DateTimeV2Object CreateSet(IDictionary<String, String> dict)
    {
        return new DateTimeV2Set(dict);
    }

    protected override DateTimeV2Object CreateTime(IDictionary<String, String> dict)
    {
        return new DateTimeV2Time(dict);
    }

    protected override DateTimeV2Object CreateTimeRange(IDictionary<String, String> dict)
    {
        return new DateTimeV2TimeRange(dict);
    }
}