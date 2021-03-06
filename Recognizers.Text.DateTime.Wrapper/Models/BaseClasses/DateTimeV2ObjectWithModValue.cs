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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;

/// <summary>
/// A <see cref="DateTimeV2Object"/> containing a value of type <see cref="TValue"/> modified by the enum <see cref="TModifier"/>
/// </summary>
/// <typeparam name="TValue">The type of the value contained by the <see cref="DateTimeV2Object"/></typeparam>
/// <typeparam name="TModifier">The type of enum modifier that modifies the <see cref="TValue"/> value</typeparam>
public abstract class DateTimeV2ObjectWithModValue<TValue, TModifier> : DateTimeV2ObjectWithValue<TValue>
    where TValue : notnull
    where TModifier : Enum
{
    // ReSharper disable once PublicConstructorInAbstractClass
    public DateTimeV2ObjectWithModValue(IDictionary<String, String> value) : base(value)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        // This will not cause problems as any initialization should not rely on other initialized values in the object.
        this.InitializeModifier(value);
    }

    /// <summary>
    /// The modifier modifying the Value
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public TModifier Modifier { get; protected set; }

    [MemberNotNull("Modifier")]
    protected abstract void InitializeModifier(IDictionary<string, string> value);

    protected abstract override void InitializeValue(IDictionary<String, String> value);
}