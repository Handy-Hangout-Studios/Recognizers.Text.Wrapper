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
using System.Diagnostics.CodeAnalysis;

namespace Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;

/// <summary>
///     The DateTimeV2 objects base class. Contains the Timex expression of the recognized value
/// </summary>
public abstract class DateTimeV2ObjectWithValue<TValue> : DateTimeV2Object,
    IEquatable<DateTimeV2ObjectWithValue<TValue>> where TValue : notnull
{
    protected DateTimeV2ObjectWithValue(IDictionary<string, string> value) : base(value)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        // This will not cause problems as any initialization should not rely on other initialized values in the object.
        this.InitializeValue(value);
    }

    /// <summary>
    ///     The Value of the resolution found.
    /// </summary>
    public TValue Value { get; protected set; }

    public bool Equals(DateTimeV2ObjectWithValue<TValue>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);
    }

    /// <summary>
    ///     Initialize the Value property.
    ///     <para>
    ///         NOTE: Do not rely on any kind of initialized values from derived classes as the constructor for the
    ///         base classes run before the constructor for derived classes which means that this will not be initialized
    ///         yet
    ///     </para>
    /// </summary>
    /// <param name="value">The value dictionary with all components necessary to create a Value object</param>
    [MemberNotNull("Value")]
    protected abstract void InitializeValue(IDictionary<string, string> value);

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

        return obj.GetType() == this.GetType() && this.Equals((DateTimeV2ObjectWithValue<TValue>)obj);
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        // The value shouldn't ever change after initialization
        return HashCode.Combine(base.GetHashCode(), this.Value);
    }

    public override string ToString()
    {
        return $"{base.ToString()}\nValue: {this.Value}\n";
    }
}