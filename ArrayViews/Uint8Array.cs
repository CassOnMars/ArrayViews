// <copyright file="Uint8Array.cs" company="Antidecorum">
// Copyright (c) Antidecorum. All rights reserved.
// </copyright>

namespace ArrayViews
{
    using System;

    /// <summary>
    /// Describes a view over a byte array as an unsigned byte array.
    /// </summary>
    public class Uint8Array : ITypedArray<byte>
    {
        private byte[] buffer;
        private int byteOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="Uint8Array"/> class.
        /// </summary>
        /// <param name="buffer">The underlying byte buffer.</param>
        /// <param name="byteOffset">Optionally sets the initial element of the array to view over.</param>
        /// <param name="length">Optionally sets the length of the array, starting from the offset. When zero, uses the length of the buffer, minus the offset.</param>
        public Uint8Array(byte[] buffer, int byteOffset = 0, int length = 0)
        {
            this.buffer = buffer;

            if (byteOffset < buffer.Length && byteOffset >= 0)
            {
                this.byteOffset = byteOffset;
            }
            else
            {
                throw new ArgumentOutOfRangeException("byteOffset", byteOffset, "Offset cannot be longer than the range of the array or less than zero.");
            }

            if (length <= buffer.Length - byteOffset && length > 0)
            {
                this.Length = length;
            }
            else if (length == 0)
            {
                this.Length = buffer.Length - byteOffset;
            }
            else
            {
                throw new ArgumentOutOfRangeException("length", length, "Length cannot be longer than the buffer length minus the offset, or less than zero.");
            }
        }

        /// <summary>
        /// Gets the number of bytes per array element.
        /// </summary>
        public int BytesPerElement => 1;

        /// <summary>
        /// Gets the length of the typed array.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the primitive types are represented
        /// in the byte array as little endian.
        ///
        /// For bytes, we'll just return whatever the active
        /// architecture is, because either answer is correct.
        /// </summary>
        public bool IsLittleEndian => BitConverter.IsLittleEndian;

        /// <summary>
        /// Accesses the byte at the corresponding index, adjusted for offset
        /// and length.
        /// </summary>
        /// <param name="index">The index of the typed array.</param>
        /// <returns>A byte at the provided index.</returns>
        public byte this[int index]
        {
            get
            {
                if (this.Length > index && index >= 0)
                {
                    return this.buffer[this.byteOffset + index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index", index, "Index is outside of the range of possible values for the array.");
                }
            }

            set
            {
                if (this.Length > index && index >= 0)
                {
                    this.buffer[this.byteOffset + index] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index", index, "Index is outside of the range of possible values for the array.");
                }
            }
        }
    }
}
