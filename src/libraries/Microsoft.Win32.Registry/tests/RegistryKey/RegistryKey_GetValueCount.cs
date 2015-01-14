// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;
using Microsoft.Win32;
using System;
using System.Threading;

namespace Microsoft.Win32.RegistryTests
{
    public class RegistryKey_GetValueCount : IDisposable
    {
        // Variables needed
        private RegistryKey _rk1, _rk2;
        private String _testKeyName = "REG_TEST_10";
        private static int s_keyCount = 0;

        public void TestInitialize()
        {
            var counter = Interlocked.Increment(ref s_keyCount);
            _testKeyName += counter.ToString();
            _rk1 = Microsoft.Win32.Registry.CurrentUser;
            if (_rk1.OpenSubKey(_testKeyName) != null)
                _rk1.DeleteSubKeyTree(_testKeyName);
            if (_rk1.GetValue(_testKeyName) != null)
                _rk1.DeleteValue(_testKeyName);
        }

        public RegistryKey_GetValueCount()
        {
            TestInitialize();
        }

        [Fact]
        public void Test01()
        {
            // [] Add several values  and check the count

            _rk2 = _rk1.CreateSubKey(_testKeyName);
            if (_rk2.ValueCount != 0)
            {
                Assert.False(true, "Error Incorrect count in subkey");
            }
        }

        [Fact]
        public void Test02()
        {
            // [] Edit one value and check the count
            _rk2 = _rk1.CreateSubKey(_testKeyName);
            _rk2.SetValue(_testKeyName, 5);
            if (_rk2.ValueCount != 1)
            {
                Assert.False(true, "Error Incorrect count in subkey");
            }
        }

        [Fact]
        public void Test03()
        {
            // [] Delete values and check the count
            _rk2 = _rk1.CreateSubKey(_testKeyName);
            if (_rk2.ValueCount != 0)
            {
                Assert.False(true, "Error Incorrect count in subkey");
            }
        }

        [Fact]
        public void Test04()
        {
            // [] Add multiple values and check the count


            _rk2 = _rk1.CreateSubKey(_testKeyName);

            Random rand = new Random(-55);

            var objArr = new Object[16];
            objArr[0] = ((Byte)(Byte)rand.Next(Byte.MinValue, Byte.MaxValue));
            objArr[1] = ((SByte)(SByte)rand.Next(SByte.MinValue, SByte.MaxValue));
            objArr[2] = ((Int16)(Int16)rand.Next(Int16.MinValue, Int16.MaxValue));
            objArr[3] = ((UInt16)(UInt16)rand.Next(UInt16.MinValue, UInt16.MaxValue));
            objArr[4] = ((Int32)rand.Next(Int32.MinValue, Int32.MaxValue));
            objArr[5] = ((UInt32)(UInt32)rand.Next((int)UInt32.MinValue, Int32.MaxValue));
            objArr[6] = ((Int64)(Int64)rand.Next(Int32.MinValue, Int32.MaxValue));
            objArr[7] = new Decimal(((Double)Decimal.MaxValue) * rand.NextDouble());
            objArr[8] = new Decimal(((Double)Decimal.MinValue) * rand.NextDouble());
            objArr[9] = new Decimal(((Double)Decimal.MinValue) * rand.NextDouble());
            objArr[10] = new Decimal(((Double)Decimal.MaxValue) * rand.NextDouble());
            objArr[11] = (Double)Int32.MaxValue * rand.NextDouble();
            objArr[12] = (Double)Int32.MinValue * rand.NextDouble();
            objArr[13] = (float)Int32.MaxValue * (float)rand.NextDouble();
            objArr[14] = (float)Int32.MinValue * (float)rand.NextDouble();
            objArr[15] = ((UInt64)(UInt64)rand.Next((int)UInt64.MinValue, Int32.MaxValue));

            for (int i = 0; i < objArr.Length; i++)
                _rk2.SetValue("Test_" + i.ToString(), objArr[i]);

            if (_rk2.ValueCount != objArr.Length)
            {
                Assert.False(true, "Error expected==" + objArr.Length + " , got value==" + _rk2.ValueCount);
            }

            for (int i = 0; i < objArr.Length; i++)
            {
                Object v = _rk2.GetValue("Test_" + i.ToString());
                if (!v.ToString().Equals(objArr[i].ToString()))
                {
                    Assert.False(true, "Error Expected==" + objArr[i].ToString() + " , got value==" + v.ToString());
                }
            }
        }

        public void Dispose()
        {
            _rk1 = Microsoft.Win32.Registry.CurrentUser;
            if (_rk1.OpenSubKey(_testKeyName) != null)
                _rk1.DeleteSubKeyTree(_testKeyName);
            if (_rk1.GetValue(_testKeyName) != null)
                _rk1.DeleteValue(_testKeyName);
        }
    }
}