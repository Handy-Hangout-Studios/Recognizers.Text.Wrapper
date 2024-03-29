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

using NodaTime;
using Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;
using Recognizers.Text.DateTime.Wrapper.Models.Modifiers;
using System;
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.NodaTime;

/// <summary>
///     An object containing a <see cref="Duration" /> value containing the number of seconds that were parsed out
/// </summary>
public class NodaDateTimeV2Duration : DateTimeV2ObjectWithModValue<Duration, DurationModifier>
{
    internal NodaDateTimeV2Duration(IDictionary<String, String> value) : base(value) { }

    protected override void InitializeModifier(IDictionary<String, String> value)
    {
        this.Modifier = value.ContainsKey("Mod")
            ? Enum.Parse<DurationModifier>(value["Mod"], true)
            : DurationModifier.None;
    }

    protected override void InitializeValue(IDictionary<String, String> value)
    {
        if (!long.TryParse(value["value"], out long result))
        {
            throw new ArgumentException(
                $"{value["value"]} is not a long corresponding to the seconds in the duration or failed to be parsed by the long parser");
        }

        this.Value = Duration.FromSeconds(result);
    }
}