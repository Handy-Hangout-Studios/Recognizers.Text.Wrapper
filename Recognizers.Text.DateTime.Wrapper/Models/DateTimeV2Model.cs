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
    private static readonly int ParseStart = "datetimeV2.".Length;

    private DateTimeV2Model(
        ModelResult modelResult,
        DateTimeV2Type type,
        MultiResolution<DateTimeV2Object> resolution)
        : base(modelResult, type, resolution)
    {
    }

    internal static DateTimeV2Model? Create(ModelResult modelResult, IDateTimeV2ObjectFactory factory)
    {
        DateTimeV2Type type = Enum.Parse<DateTimeV2Type>(modelResult.TypeName[ParseStart..], true);
        List<Dictionary<string, string>> values = (List<Dictionary<string, string>>)modelResult.Resolution["values"];
        DateTimeV2Model model = new(
            modelResult,
            type,
            new MultiResolution<DateTimeV2Object>(values
                .Where(dict => dict["type"] != "date" || dict["value"] != "not resolved")
                .Select(dict => factory.Create(type, dict))
            )
        );

        return model.Resolution.Values.Any() ? model : null;
    }
}