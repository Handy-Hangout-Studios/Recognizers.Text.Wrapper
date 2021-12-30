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

namespace Recognizers.Text.DateTime.Wrapper.Models.Modifiers;

/// <summary>
///     A Modifier indicating what kind of relationship the DateRange has.
/// </summary>
public enum DateModifier
{
    /// <summary>
    ///     None implies that the date range goes from Start to End
    /// </summary>
    None = 0,

    /// <summary>
    ///     Before implies that the date range goes from some point in the past to the End
    /// </summary>
    Before = 1,

    /// <summary>
    ///     Until implies that the date range goes from some point in the past to the End
    /// </summary>
    Until = 2,

    /// <summary>
    ///     Until implies that the date range goes from the Start to some point in the future
    /// </summary>
    After = 3,

    /// <summary>
    ///     Until implies that the date range goes from the Start to some point in the future
    /// </summary>
    Since = 4,
}