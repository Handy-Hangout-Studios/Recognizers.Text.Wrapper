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
using Recognizers.Text.DateTime.Wrapper.Models.Modifiers;
using Recognizers.Text.DateTime.Wrapper.Models.Range;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;

/// <summary>
///     A DateTime GenericRange Value containing the DateTime start and DateTime end recognized.
///
///     If <see cref="DateTimeModifier"/> is <see cref="DateTimeModifier.Before"/> or <see cref="DateTimeModifier.Until"/> then
///     <see cref="DateTimeV2DateTimeRange.Value"/> has Start set to the minimum possible DateTime
///
///     If <see cref="DateTimeModifier"/> is <see cref="DateTimeModifier.Since"/> or <see cref="DateTimeModifier.After"/> then
///     <see cref="DateTimeV2DateTimeRange.Value"/> has End set to the maximum possible DateTime
/// </summary>
internal class DateTimeV2DateTimeRange : DateTimeV2ObjectWithValue<DateTimeV2Range<System.DateTime, DateTimeModifier>>
{
    internal DateTimeV2DateTimeRange(IDictionary<string, string> value) : base(value) { }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        bool parseStart = true;
        bool parseEnd = true;
        DateTimeModifier modifier = DateTimeModifier.None;
        if (value.ContainsKey("Mod"))
        {
            modifier = Enum.Parse<DateTimeModifier>(value["Mod"], true);
            switch (modifier)
            {
                case DateTimeModifier.None:
                    break;
                case DateTimeModifier.Before:
                case DateTimeModifier.Until:
                    parseStart = false;
                    break;
                case DateTimeModifier.Since:
                case DateTimeModifier.After:
                    parseEnd = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier), modifier,
                        $"{nameof(modifier)} was not a valid DateTimeModifier");
            }
        }

        System.DateTime start = System.DateTime.MinValue;
        if (parseStart && !System.DateTime.TryParseExact(value["start"], "yyyy-MM-dd HH:mm:ss", null, default,
                out start))
        {
            throw new ArgumentException($"{value["start"]} is not of the format yyyy-MM-dd HH:mm:ss");
        }

        System.DateTime end = System.DateTime.MaxValue;
        if (parseEnd && !System.DateTime.TryParseExact(value["end"], "yyyy-MM-dd HH:mm:ss", null, default,
                out end))
        {
            throw new ArgumentException($"{value["end"]} is not of the format yyyy-MM-dd HH:mm:ss");
        }

        this.Value = new DateTimeV2Range<System.DateTime, DateTimeModifier>(modifier, start, end);
    }
}