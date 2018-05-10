// <copyright file="Int16Array.cs" company="Antidecorum">
// Copyright (c) Antidecorum. All rights reserved.
// </copyright>

namespace ArrayViews
{
    using System;

    /// <summary>
    /// Describes a view over a byte array as a signed short array.
    /// </summary>
    public class Int16Array : ITypedArray<short>
    {
        private byte[] buffer;
        private int byteOffset;
        private bool isLittleEndian;

        /// <summary>
        /// Initializes a new instance of the <see cref="Int16Array"/> class.
        /// </summary>
        /// <param name="buffer">The underlying byte buffer.</param>
        /// <param name="byteOffset">Optionally sets the initial element of the array to view over.</param>
        /// <param name="length">Optionally sets the length of the array, starting from the offset. When zero, uses the floor of the evaluation of the length of the buffer, minus the offset, divided by the number of bytes per element.</param>
        /// <param name="isLittleEndian">Optionally sets the endianness of the buffer. When null, uses the platform's default.</param>
        public Int16Array(byte[] buffer, int byteOffset = 0, int length = 0, bool? isLittleEndian = null)
        {
            this.buffer = buffer;

            if (isLittleEndian.HasValue)
            {
                this.isLittleEndian = isLittleEndian.Value;
            }
            else
            {
                this.isLittleEndian = BitConverter.IsLittleEndian;
            }

            if (byteOffset < buffer.Length && byteOffset >= 0)
            {
                this.byteOffset = byteOffset;
            }
            else
            {
                throw new ArgumentOutOfRangeException("byteOffset", byteOffset, "Offset cannot be longer than the range of the array or less than zero.");
            }

            if (length * this.BytesPerElement <= buffer.Length - byteOffset && length > 0)
            {
                this.Length = length;
            }
            else if (length == 0)
            {
                this.Length = (buffer.Length - byteOffset) / this.BytesPerElement;
            }
            else
            {
                throw new ArgumentOutOfRangeException("length", length, "Length cannot be longer than the floor of the evaluation of the buffer length minus the offset divided by the number of bytes per element, or less than zero.");
            }
        }

        /// <summary>
        /// Gets the number of bytes per array element.
        /// </summary>
        public int BytesPerElement => 2;

        /// <summary>
        /// Gets the length of the typed array.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the primitive types are represented
        /// in the byte array as little endian. When changed, converts the array elements.
        /// </summary>
        public bool IsLittleEndian
        {
            get
            {
                return this.isLittleEndian;
            }

            set
            {
                if (value != this.isLittleEndian)
                {
                    for (var i = 0; i < this.Length; i++)
                    {
                        byte b1 = this.buffer[i * this.BytesPerElement];
                        byte b2 = this.buffer[(i * this.BytesPerElement) + 1];

                        this.buffer[i * this.BytesPerElement] = b2;
                        this.buffer[(i * this.BytesPerElement) + 1] = b1;
                    }

                    this.isLittleEndian = value;
                }
            }
        }

        /// <summary>
        /// Accesses the short at the corresponding index, adjusted for offset.
        /// and length.
        /// </summary>
        /// <param name="index">The index of the typed array.</param>
        /// <returns>A short at the provided index.</returns>
        public short this[int index]
        {
            get
            {
                if (this.Length > index && index >= 0)
                {
                    if (this.IsLittleEndian)
                    {
                        // [0x0A,0x0B] = 0x0B0A
                        return (short)((this.buffer[this.byteOffset + (index * this.BytesPerElement) + 1] << 8) | this.buffer[this.byteOffset + (index * this.BytesPerElement)]);
                    }
                    else
                    {
                        // [0x0A,0x0B] = 0x0A0B
                        return (short)((this.buffer[this.byteOffset + (index * this.BytesPerElement)] << 8) | this.buffer[this.byteOffset + (index * this.BytesPerElement) + 1]);
                    }
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
                    if (this.IsLittleEndian)
                    {
                        this.buffer[this.byteOffset + (index * this.BytesPerElement) + 1] = (byte)((value & 0xFF00) >> 8);
                        this.buffer[this.byteOffset + (index * this.BytesPerElement)] = (byte)value;
                    }
                    else
                    {
                        this.buffer[this.byteOffset + (index * this.BytesPerElement)] = (byte)((value & 0xFF00) >> 8);
                        this.buffer[this.byteOffset + (index * this.BytesPerElement) + 1] = (byte)value;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index", index, "Index is outside of the range of possible values for the array.");
                }
            }
        }
    }
}
