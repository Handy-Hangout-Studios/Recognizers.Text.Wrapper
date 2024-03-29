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

namespace Recognizers.Text.DateTime.Wrapper.Models.Enums;

/// <summary>
///     The types of DateTimeV2 objects that exist as found in
///     the Microsoft.Recognizers.Text testing specs.
/// </summary>
public enum DateTimeV2Type
{
    Date,
    Time,
    DateRange,
    TimeRange,
    DateTimeRange,
    DateTime,
    Duration,
    Set,
}