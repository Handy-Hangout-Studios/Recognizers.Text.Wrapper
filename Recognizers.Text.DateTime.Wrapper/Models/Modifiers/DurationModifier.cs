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

public enum DurationModifier
{
    /// <summary>
    ///     The duration should be exactly as long as specified
    /// </summary>
    None = 0,

    /// <summary>
    ///     The duration should be as long as specified or longer
    /// </summary>
    More = 1,

    /// <summary>
    ///     The duration should be as long as specified or shorter
    /// </summary>
    Less = 2,
}