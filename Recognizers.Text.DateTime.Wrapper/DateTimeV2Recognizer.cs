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

using Microsoft.Recognizers.Text.DateTime;
using Recognizers.Text.DateTime.Wrapper.Models;
using Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;
using Recognizers.Text.DateTime.Wrapper.Models.Enums;
using Recognizers.Text.DateTime.Wrapper.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Wrapper
{
    public class DateTimeV2Recognizer
    {
        private readonly DateTimeModel model;
        private readonly IDateTimeV2ObjectFactory factory;

        public DateTimeV2Recognizer(string culture = Culture.English, IDateTimeV2ObjectFactory? factory = null)
        {
            this.model = new DateTimeRecognizer(culture).GetDateTimeModel();
            this.factory = factory ?? new BclDateTimeV2ObjectFactory();
        }

        public IEnumerable<DateTimeV2Model> RecognizeDateTimes(string content, System.DateTime? refTime = null, ISet<DateTimeV2Type>? typeFilter = null)
        {
            List<ModelResult> modelResults;
            if (!refTime.HasValue)
            {
                modelResults = this.model.Parse(content);
            }
            else
            {
                modelResults = this.model.Parse(content, refTime.Value);
            }

            if (typeFilter is null)
            {
                return modelResults
                    .Select(mr => new DateTimeV2Model(mr, this.factory));
            }
            else
            {
                return modelResults
                    .Select(mr => new DateTimeV2Model(mr, this.factory))
                    .Where(dtm => typeFilter.Contains(dtm.Type));
            }

        }

        private static readonly Dictionary<(string, Type), DateTimeV2Recognizer> cached = new();

        public static IEnumerable<DateTimeV2Model> RecognizeDateTimes(
            string content,
            string culture = Culture.English,
            System.DateTime? refTime = null,
            IDateTimeV2ObjectFactory? factory = null,
            ISet<DateTimeV2Type>? typeFilter = null
            )
        {
            Type type = factory == null ? typeof(BclDateTimeV2ObjectFactory) : factory.GetType();
            if (!cached.TryGetValue((culture, type), out DateTimeV2Recognizer? recognizer))
            {
                cached[(culture, type)] = new DateTimeV2Recognizer(culture, factory);
                recognizer = cached[(culture, type)];
            }

            return recognizer.RecognizeDateTimes(content, refTime, typeFilter);
        }

        public static IEnumerable<DateTimeV2Model> RecognizeDateTimes(
            string content,
            string culture = Culture.English,
            System.DateTime? refTime = null,
            ISet<DateTimeV2Type>? typeFilter = null)
            => RecognizeDateTimes(content, culture, refTime, null, typeFilter);

        public static IEnumerable<DateTimeV2Model> RecognizeDateTimes(
            string content,
            string culture = Culture.English,
            ISet<DateTimeV2Type>? typeFilter = null)
            => RecognizeDateTimes(content, culture, null, null, typeFilter);

        public static IEnumerable<DateTimeV2Model> RecognizeDateTimes(
            string content,
            string culture = Culture.English)
            => RecognizeDateTimes(content, culture, null, null, null);
    }
}
