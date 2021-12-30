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
using System.Collections.Generic;

namespace Recognizers.Text.Wrapper;

/// <summary>
///     A value that holds a single value and it's Type
/// </summary>
/// <typeparam name="TValue">The Type of value that is being returned for this Resolution</typeparam>
/// <typeparam name="TSubType">The Types that are available for this kind of resolution</typeparam>
public sealed class SingleResolution<TValue, TSubType> : Resolution, IEquatable<SingleResolution<TValue, TSubType>>
    where TSubType : Enum
{
    public SingleResolution(TValue value, TSubType subtype)
    {
        this.Value = value;
        this.SubType = subtype;
    }

    /// <summary>
    ///     The subtype of resolution value that this value is.
    /// </summary>
    public TSubType SubType { get; }

    /// <summary>
    ///     The value of the resolution.
    /// </summary>
    public TValue Value { get; }

    public bool Equals(SingleResolution<TValue, TSubType>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return EqualityComparer<TSubType>.Default.Equals(this.SubType, other.SubType) &&
               EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is SingleResolution<TValue, TSubType> other && this.Equals(other));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.SubType, this.Value);
    }

    public override Boolean Equals(Resolution? other)
    {
        return !ReferenceEquals(null, other) && this.Equals((object)other);
    }
}