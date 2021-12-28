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
using System.Collections.Generic;

namespace Recognizers.Text.DateTime.Wrapper.Models.Interfaces;

/// <summary>
///     Implements the functionality necessary to create the appropriate .NET objects for all of the
///     <see cref="DateTimeV2Type" />s
/// </summary>
public interface IDateTimeV2ObjectFactory
{
    /// <summary>
    ///     Convert IDictionary from the DateTimeV2 Value into the corresponding DateTimeV2Object
    /// </summary>
    /// <param name="type">The type of DateTimeV2Object to create</param>
    /// <param name="dict">The dictionary of values used to create the DateTimeV2Object</param>
    /// <returns>The appropriate DateTimeV2Object for that DateTimeV2Type</returns>
    internal DateTimeV2Object Create(DateTimeV2Type type, IDictionary<string, string> dict);
}