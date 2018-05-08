using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArrayViews.Test
{
    [TestClass]
    public class Uint8ArrayTests
    {
        [TestMethod]
        public void Uint8Array_indexed_setter_alters_source_buffer_contents()
        {
            byte[] buf = new byte[] { 0x00, 0x01, 0x02 };
            var u8arr = new Uint8Array(buf, 0);
            u8arr[0] = 0xFF;
            Assert.AreEqual(buf[0], 0xFF);
        }

        [TestMethod]
        public void Uint8Array_throws_ArgumentOutOfRangeException_when_accessing_outside_length()
        {
            byte[] buf = new byte[] { 0x00, 0x01, 0x02 };
            var u8arr = new Uint8Array(buf, 0, 1);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => u8arr[1]);
        }


    }
}
