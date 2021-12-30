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

using Microsoft.Recognizers.Text;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.Wrapper;

/// <summary>
///     A Abstract Base Class <see cref="Microsoft.Recognizers.Text.ModelResult" /> wrapper that can be used for any kind
///     of
///     <see cref="Microsoft.Recognizers.Text.ModelResult" /> for any kind of Microsoft Recognizer.
/// </summary>
/// <typeparam name="TResolution">The Type of Resolution that is being used here</typeparam>
/// <typeparam name="TEnum">The Enum representing the types used for this kind of Recognizer Object</typeparam>
public abstract class RecognizerObjectModel<TResolution, TEnum> : IEquatable<RecognizerObjectModel<TResolution, TEnum>>
    where TEnum : Enum
    where TResolution : Resolution
{
    /// <summary>
    ///     This is never used. If it is used, something is wrong. It's private because it should never be used.
    /// </summary>
    private RecognizerObjectModel()
    {
        this.Text = default!;
        this.Type = default!;
        this.Resolution = default!;
    }

    /// <summary>
    ///     The default constructor used.
    ///     It initializes the Text, Start, and End properties,
    ///     and then relies on the defined InitializeType and
    ///     InitializeResolution to initialize both of those properties.
    /// </summary>
    /// <param name="modelResult">The Microsoft Recognizer provided ModelResult</param>
    protected RecognizerObjectModel(ModelResult modelResult, TEnum type, TResolution resolution)
    {
        this.Text = modelResult.Text;
        this.Start = modelResult.Start;
        this.End = modelResult.End;
        this.Type = type;
        this.Resolution = resolution;
    }

    /// <summary>
    ///     The Text that the recognizer was used on
    /// </summary>
    public string Text { get; }

    /// <summary>
    ///     The start of the text at which the recognizer found this resolution(s)
    /// </summary>
    public int Start { get; }

    /// <summary>
    ///     The end of the text at which the recognizer found this resolution(s)
    /// </summary>
    public int End { get; }

    /// <summary>
    ///     The type of Resolution found.
    /// </summary>
    public TEnum Type { get; protected init; }

    /// <summary>
    ///     The resolution that was found.
    /// </summary>
    public TResolution Resolution { get; protected init; }

    public bool Equals(RecognizerObjectModel<TResolution, TEnum>? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(this.Text, other.Text, StringComparison.OrdinalIgnoreCase) &&
               this.Start == other.Start &&
               this.End == other.End &&
               EqualityComparer<TEnum>.Default.Equals(this.Type, other.Type) &&
               this.Resolution.Equals(other.Resolution);
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

        return this.Equals((RecognizerObjectModel<TResolution, TEnum>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Text, this.Start, this.End, this.Type, this.Resolution);
    }

    public override String ToString()
    {
        return
            $"==================\nText: {this.Text}\nStart: {this.Start}\nEnd: {this.End}\nResolution\n==================\n{this.Resolution}\n==================";
    }
}