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
using Recognizers.Text.DateTime.Wrapper.Models.BaseClasses;
using Recognizers.Text.DateTime.Wrapper.Models.Enums;
using Recognizers.Text.DateTime.Wrapper.Models.Interfaces;
using Recognizers.Text.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizers.Text.DateTime.Wrapper.Models;

public sealed class DateTimeV2Model : RecognizerObjectModel<MultiResolution<DateTimeV2Object>, DateTimeV2Type>
{
    private static readonly int parseStart = "datetimeV2.".Length;
    private readonly IDateTimeV2ObjectFactory factory;

    internal DateTimeV2Model(ModelResult modelResult, IDateTimeV2ObjectFactory factory) : base(modelResult)
    {
        this.factory = factory;
    }

    protected override void InitializeResolution(IDictionary<String, Object> resolution)
    {
        this.Resolution = new MultiResolution<DateTimeV2Object>(
            ((List<Dictionary<string, string>>)resolution["values"]).Select(
                dict => this.factory.Create(this.Type, dict)));
    }

    protected override void InitializeType(String typename)
    {
        this.Type = Enum.Parse<DateTimeV2Type>(typename[parseStart..]);
    }
}