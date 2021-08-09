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
using System.Diagnostics.CodeAnalysis;

namespace Recognizers.Text.Wrapper
{
    public abstract class RecognizerObjectModel<TResolution, TEnum>
        where TEnum : Enum
        where TResolution : notnull, Resolution
    {
        /// <summary>
        /// The Text that the recognizer was used on
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The start of the text at which the recognizer found this resolution(s)
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// The end of the text at which the recognizer found this resolution(s)
        /// </summary>
        public int End { get; private set; }

        /// <summary>
        /// The type of Resolution found.
        /// </summary>
        public TEnum Type { get; protected set; }

        /// <summary>
        /// The resolution that was found.
        /// </summary>
        public TResolution Resolution { get; protected set; }

        /// <summary>
        /// This is never used. If it is used, something is wrong. It's private because it should never be used.
        /// </summary>
        private RecognizerObjectModel()
        {
            this.Text = default!;
            this.Type = default!;
            this.Resolution = default!;
        }

        /// <summary>
        /// The default constructor used. 
        /// It initializes the Text, Start, and End properties, 
        /// and then relies on the defined InitializeType and 
        /// InitializeResolution to initialize both of those properties.
        /// </summary>
        /// <param name="modelResult">The Microsoft Recognizer provided ModelResult</param>
        protected RecognizerObjectModel(ModelResult modelResult)
        {
            this.Text = modelResult.Text;
            this.Start = modelResult.Start;
            this.End = modelResult.End;
            this.InitializeType(modelResult.TypeName);
            this.InitializeResolution(modelResult.Resolution);
        }

        [MemberNotNull("Resolution")]
        protected abstract void InitializeResolution(IDictionary<string, object> resolution);

        [MemberNotNull("Type")]
        protected abstract void InitializeType(string typename);
    }
}
