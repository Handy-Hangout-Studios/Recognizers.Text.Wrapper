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

using System;

namespace Recognizers.Text.DateTime.Wrapper.Models.Generics
{
    /// <summary>
    /// Represents a range of comparable generics
    /// </summary>
    /// <typeparam name="TRangeType">A type for the range.</typeparam>
    public struct ComparableRange<TRangeType> : IEquatable<ComparableRange<TRangeType>> where TRangeType : notnull, IComparable<TRangeType>
    {
        /// <summary>
        /// Start of the range of generics
        /// </summary>
        public TRangeType Start { get; private set; }

        /// <summary>
        /// End of the range of generics
        /// </summary>
        public TRangeType End { get; private set; }

        public ComparableRange(TRangeType start, TRangeType end)
        {
            if (start.CompareTo(end) > 0)
            {
                throw new ArgumentException($"{start} is greater than {end} which is not supported");
            }
            this.Start = start;
            this.End = end;
        }

        public bool Equals(ComparableRange<TRangeType> other)
        {
            return this.Start.Equals(other.Start) && this.End.Equals(other.End);
        }

        public override bool Equals(object? obj)
        {
            if (obj is ComparableRange<TRangeType> second)
                return this.Equals(second);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ComparableRange<TRangeType> left, ComparableRange<TRangeType> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ComparableRange<TRangeType> left, ComparableRange<TRangeType> right)
        {
            return !(left == right);
        }
    }
}
