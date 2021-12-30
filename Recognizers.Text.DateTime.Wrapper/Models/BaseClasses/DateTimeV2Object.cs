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

using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;

/// <summary>
///     The DateTimeV2 objects base class. Contains the Timex expression of the recognized value
/// </summary>
public abstract class DateTimeV2Object : IEquatable<DateTimeV2Object>
{
    private DateTimeV2Object()
    {
        this.Timex = null!;
    }

    protected DateTimeV2Object(IDictionary<string, string> value)
    {
        this.Timex = new TimexProperty(value["timex"]);
    }

    /// <summary>
    ///     The Timex value of the DateTimeV2 object recognized by the recognizer.
    /// </summary>
    public TimexProperty Timex { get; }

    public bool Equals(DateTimeV2Object? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Timex.TimexValue.Equals(other.Timex.TimexValue);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return this.Equals((DateTimeV2Object)obj);
    }

    public override int GetHashCode()
    {
        return this.Timex.GetHashCode();
    }

    public override String ToString()
    {
        return $"Timex: {this.Timex}";
    }
}