// Copyright 2009 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using System;
using System.Text;
using NodaTime.TimeZones;
using NodaTime.Utility;

namespace NodaTime.TzdbCompiler.Tzdb
{
    /// <summary>
    ///   Defines one time zone rule with a validitity range.
    /// </summary>
    /// <remarks>
    ///   Immutable, threadsafe.
    /// </remarks>
    internal class ZoneRule : IEquatable<ZoneRule>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="ZoneRule" /> class.
        /// </summary>
        /// <param name="recurrence">The recurrence definition of this rule.</param>
        /// <param name="letterS">The daylight savings indicator letter for time zone names.</param>
        public ZoneRule(ZoneRecurrence recurrence, string letterS)
        {
            this.Recurrence = recurrence;
            this.LetterS = letterS;
        }

        public string LetterS { get; }

        public string Name => Recurrence.Name;

        public ZoneRecurrence Recurrence { get; }

        #region IEquatable<ZoneRule> Members
        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter;
        ///   otherwise, false.
        /// </returns>
        public bool Equals(ZoneRule other) => other != null && Equals(Recurrence, other.Recurrence) && Equals(LetterS, other.LetterS);
        #endregion

        #region Operator overloads
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ZoneRule left, ZoneRule right) =>        
            ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ZoneRule left, ZoneRule right) => !(left == right);
        #endregion

        /// <summary>
        ///   Formats the name.
        /// </summary>
        /// <param name="nameFormat">The name format.</param>
        public String FormatName(String nameFormat)
        {
            Preconditions.CheckNotNull(nameFormat, "nameFormat");
            int index = nameFormat.IndexOf("/", StringComparison.Ordinal);
            if (index > 0)
            {
                return Recurrence.Savings == Offset.Zero ? nameFormat.Substring(0, index) : nameFormat.Substring(index + 1);
            }
            index = nameFormat.IndexOf("%s", StringComparison.Ordinal);
            if (index < 0)
            {
                return nameFormat;
            }
            var left = nameFormat.Substring(0, index);
            var right = nameFormat.Substring(index + 2);
            return LetterS == null ? left + right : left + LetterS + right;
        }

        #region Object overrides
        /// <summary>
        ///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance;
        ///   otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => Equals(obj as ZoneRule);

        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data
        ///   structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => HashCodeHelper.Hash(Recurrence, LetterS);

        /// <summary>
        ///   Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Recurrence);
            if (LetterS != null)
            {
                builder.Append(" \"").Append(LetterS).Append("\"");
            }
            return builder.ToString();
        }
        #endregion Object overrides
    }
}
