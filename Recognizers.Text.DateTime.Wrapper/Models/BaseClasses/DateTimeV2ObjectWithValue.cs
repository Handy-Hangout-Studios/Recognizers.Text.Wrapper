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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Recognizers.Text.DateTime.Wrapper.Models.BaseClasses
{
    /// <summary>
    /// The DateTimeV2 objects base class. Contains the Timex expression of the recognized value
    /// </summary>
    public abstract class DateTimeV2ObjectWithValue<TValue> : DateTimeV2Object where TValue : notnull
    {
        /// <summary>
        /// The Value of the resolution found.
        /// </summary>
        public TValue Value { get; protected set; }

        protected DateTimeV2ObjectWithValue(IDictionary<string, string> value) : base(value)
        {
            this.InitializeValue(value);
        }

        /// <summary>
        /// Initialize the Value property
        /// </summary>
        /// <param name="value">The value dictionary with all components necessary to create a Value object</param>
        [MemberNotNull("Value")]
        protected abstract void InitializeValue(IDictionary<string, string> value);
    }
}
