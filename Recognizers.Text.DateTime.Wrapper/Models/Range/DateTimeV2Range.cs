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

using System;

namespace Recognizers.Text.DateTime.Wrapper.Models.Range;

/// <summary>
///     Represents a range of dates, times, or datetimes
/// </summary>
/// <typeparam name="TRangeType">A type for the range.</typeparam>
/// <typeparam name="TRangeModifier">The enum modifier modifying how to read a range</typeparam>
public readonly struct
    DateTimeV2Range<TRangeType, TRangeModifier> : IEquatable<DateTimeV2Range<TRangeType, TRangeModifier>>
    where TRangeType : notnull
    where TRangeModifier : Enum
{
    /// <summary>
    ///     Start of the range of generics
    /// </summary>
    public TRangeType Start { get; }

    /// <summary>
    ///     End of the range of generics
    /// </summary>
    public TRangeType End { get; }

    /// <summary>
    ///     The modifier for this <see cref="DateTimeV2Range{TRangeType,TRangeModifier}"/> 
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public TRangeModifier Modifier { get; }

    public DateTimeV2Range(TRangeModifier mod, TRangeType start, TRangeType end)
    {
        this.Modifier = mod;
        this.Start = start;
        this.End = end;
    }

    public bool Equals(DateTimeV2Range<TRangeType, TRangeModifier> other)
    {
        return this.Start.Equals(other.Start) && this.End.Equals(other.End);
    }

    public override bool Equals(object? obj)
    {
        if (obj is DateTimeV2Range<TRangeType, TRangeModifier> second)
        {
            return this.Equals(second);
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Start, this.End);
    }

    public static bool operator ==(DateTimeV2Range<TRangeType, TRangeModifier> left,
        DateTimeV2Range<TRangeType, TRangeModifier> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(DateTimeV2Range<TRangeType, TRangeModifier> left,
        DateTimeV2Range<TRangeType, TRangeModifier> right)
    {
        return !(left == right);
    }

    public override String ToString()
    {
        return $"[{this.Start}, {this.End}]";
    }
}