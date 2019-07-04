﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Misaka.Domain
{
    public class ValueObject<T> where T : class
    {
        public static T Empty => Activator.CreateInstance<T>();

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return NotEqualOperator(a, b);
        }

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            return EqualOperator(a, b);
        }

        /// <summary>
        ///     Helper function for implementing overloaded equality operator.
        /// </summary>
        /// <param name="left">Left-hand side object.</param>
        /// <param name="right">Right-hand side object.</param>
        /// <returns></returns>
        protected static bool EqualOperator(ValueObject<T> left, ValueObject<T> right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left is null || left.Equals(right);
        }

        /// <summary>
        ///     Helper function for implementing overloaded inequality operator.
        /// </summary>
        /// <param name="left">Left-hand side object.</param>
        /// <param name="right">Right-hand side object.</param>
        /// <returns></returns>
        protected static bool NotEqualOperator(ValueObject<T> left, ValueObject<T> right)
        {
            return !EqualOperator(left, right);
        }

        /// <summary>
        ///     To be overridden in inheriting classes for providing a collection of atomic values of
        ///     this Value Object.
        /// </summary>
        /// <returns>Collection of atomic values.</returns>
        protected virtual IEnumerable<object> GetAtomicValues()
        {
            return GetType().GetProperties().Select(p => p.GetValue(this, null));
        }

        /// <summary>
        ///     Compares two Value Objects according to atomic values returned by <see cref="GetAtomicValues" />.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>True if objects are considered equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (ValueObject<T>)obj;

            using (var thisValues = GetAtomicValues().GetEnumerator())
            {
                using (var otherValues = other.GetAtomicValues().GetEnumerator())
                {
                    while (thisValues.MoveNext() && otherValues.MoveNext())
                    {
                        if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                        {
                            return false;
                        }
                        if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                        {
                            return false;
                        }
                    }
                    return !thisValues.MoveNext() && !otherValues.MoveNext();
                }
            }
        }

        /// <summary>
        ///     Returns hashcode value calculated according to a collection of atomic values
        ///     returned by <see cref="GetAtomicValues" />.
        /// </summary>
        /// <returns>Hashcode value.</returns>
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
