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

using NodaTime.Text;
using NUnit.Framework;
using Recognizers.Text.DateTime.Wrapper;
using Recognizers.Text.DateTime.Wrapper.Models;
using Recognizers.Text.DateTime.Wrapper.Models.Enums;
using Recognizers.Text.DateTime.Wrapper.NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject1.DateTimeV2;

[TestFixture]
public class DateTimeV2RecognizerNodaTimeTests
{
    [TestCase("2016-02-30")]
    [TestCase("2016-02-31")]
    [TestCase("2016-04-31")]
    [TestCase("2016-04-32")]
    public void TestRecognizerIgnoresInvalidDates(string content)
    {
        IEnumerable<DateTimeV2Model> results =
            DateTimeV2Recognizer.RecognizeDateTimes(content, factory: typeof(NodaTimeDateTimeV2ObjectFactory));
        Assert.IsEmpty(results);
    }

    [TestCase("\"Within 3 years\", he said this 5 years ago.", "2018-03-14T00:00:00", "2018-03-14", "2021-03-14")]
    public void TestDateRanges(string content, string refTime, string start, string end)
    {
        IEnumerable<DateTimeV2Model> results = DateTimeV2Recognizer.RecognizeDateTimes(content,
            factory: typeof(NodaTimeDateTimeV2ObjectFactory),
            refTime: DateTime.ParseExact(refTime, "yyyy-MM-ddTHH:mm:ss", null),
            typeFilter: new HashSet<DateTimeV2Type> {DateTimeV2Type.DateRange});
        NodaDateTimeV2DateRange result = results.First().Resolution.Values.Cast<NodaDateTimeV2DateRange>().First();

        LocalDatePattern pattern = LocalDatePattern.CreateWithInvariantCulture("uuuu-MM-dd");
        Assert.AreEqual(result.Value.Start,
            pattern.Parse(start).Value);

        Assert.AreEqual(result.Value.End,
            pattern.Parse(end).Value);
    }
}