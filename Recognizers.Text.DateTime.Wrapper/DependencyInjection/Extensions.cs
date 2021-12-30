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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Recognizers.Text;
using Recognizers.Text.DateTime.Wrapper.Models.BclDateTime;

namespace Recognizers.Text.DateTime.Wrapper.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddDateTimeV2Recognizer<TFactory>(this IServiceCollection serviceCollection, string culture = Culture.English)
    {
        return serviceCollection.AddSingleton(_ => new DateTimeV2Recognizer(culture, typeof(TFactory)));
    }

    public static IServiceCollection AddDateTimeV2Recognizer(this IServiceCollection serviceCollection, string culture = Culture.English)
        => serviceCollection.AddDateTimeV2Recognizer<BclDateTimeV2ObjectFactory>(culture);
}