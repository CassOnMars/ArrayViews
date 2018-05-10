using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayViews.Test
{
    [TestClass]
    public class Int16ArrayTests
    {
        [TestMethod]
        public void Int16Array_indexed_setter_alters_source_buffer_contents_according_to_native_endianness()
        {
            byte[] buf = new byte[] { 0x00, 0x01, 0x02, 0x03 };
            var s16arr = new Int16Array(buf, 0);
            s16arr[0] = 255; // 0x00FF, stored as [0x00, 0xFF] on big endian archs and [0xFF, 0x00] on little endian archs.

            if (BitConverter.IsLittleEndian)
            {
                Assert.AreEqual(buf[0], 0xFF);
            }
            else
            {
                Assert.AreEqual(buf[1], 0xFF);
            }
        }

        [TestMethod]
        public void Int16Array_indexed_setter_alters_source_buffer_contents_according_to_set_endianness_when_specified()
        {
            byte[] buf = new byte[] { 0x00, 0x01, 0x02, 0x03 };
            var s16arr = new Int16Array(buf, 0, 0, !BitConverter.IsLittleEndian);
            s16arr[0] = 255;

            if (BitConverter.IsLittleEndian)
            {
                Assert.IsTrue(!s16arr.IsLittleEndian);
                Assert.AreEqual(buf[1], 0xFF);
            }
            else
            {
                Assert.IsTrue(s16arr.IsLittleEndian);
                Assert.AreEqual(buf[0], 0xFF);
            }
        }

        [TestMethod]
        public void Int16Array_indexed_setter_stores_and_retrieves_with_correct_signedness()
        {
            byte[] buf = new byte[65536 * 2]; // Total number of possible shorts * number of bytes required to store them.

            var s16arr = new Int16Array(buf, 0, 0);

            for (var i = 0; i < 65536; i++)
            {
                s16arr[i] = (short)(short.MinValue + i);
            }

            for (int i = short.MinValue, j = 0; i <= short.MaxValue; i++, j++)
            {
                Assert.AreEqual(s16arr[j], i);
            }
        }
    }
}
