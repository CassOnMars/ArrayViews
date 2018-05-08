// <copyright file="ITypedArray.cs" company="Antidecorum">
// Copyright (c) Antidecorum. All rights reserved.
// </copyright>

namespace ArrayViews
{
    /// <summary>
    /// Describes a view over a byte array as another primitive type.
    /// </summary>
    /// <typeparam name="T">The primitive array type to view the byte array as.</typeparam>
    public interface ITypedArray<T>
    {
        /// <summary>
        /// Gets the number of bytes per array element.
        /// </summary>
        int BytesPerElement { get; }

        /// <summary>
        /// Gets the length of the array.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets a value indicating whether the primitive types are represented
        /// in the byte array as little endian.
        /// </summary>
        bool IsLittleEndian { get; }

        /// <summary>
        /// Accesses the byte array as the primitive type at the typed array's
        /// index.
        /// </summary>
        /// <param name="index">The index of the typed array.</param>
        /// <returns>The element of the typed array at index.</returns>
        T this[int index] { get; set; }
    }
}
