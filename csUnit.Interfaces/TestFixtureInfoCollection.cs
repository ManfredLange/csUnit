#region Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, and Piers Lawson. All rights reserved.
////////////////////////////////////////////////////////////////////////////////
// Copyright © 2002-2011 by Manfred Lange, Markus Renschler, Jake Anderson, 
//                       and Piers Lawson. All rights reserved.
//
// This software is provided 'as-is', without any express or implied warranty. 
// In no event will the authors be held liable for any damages arising from the
// use of this software.
//
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not claim 
//    that you wrote the original software. If you use this software in a 
//    product, an acknowledgment in the product documentation would be 
//    appreciated but is not required.
//
// 2. Altered source versions must be plainly marked as such, and must not be 
//    misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.
////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace csUnit.Interfaces {
   /// <summary>
   /// Summary description for TextFixtureInfoCollection.
   /// </summary>
   [Serializable]
   public class TestFixtureInfoCollection : ICollection<ITestFixtureInfo>, ISerializable {
      public TestFixtureInfoCollection() {
      }

      protected TestFixtureInfoCollection(SerializationInfo info, 
         StreamingContext context) {
         _testFixtureInfos = info.GetValue("data", typeof(List<ITestFixtureInfo>)) 
            as List<ITestFixtureInfo>;
      }

      #region IEnumerable Members

      public IEnumerator<ITestFixtureInfo> GetEnumerator() {
         return _testFixtureInfos.GetEnumerator();
      }

      #endregion
   
      #region ISerializable Members

      public void GetObjectData(SerializationInfo info, StreamingContext context) {
         info.AddValue("data", _testFixtureInfos);
      }

      #endregion

      #region ICollection<ITestFixtureInfo> Members

      public void Add(ITestFixtureInfo item) {
         _testFixtureInfos.Add(item);
      }

      public void Clear() {
         _testFixtureInfos.Clear();
      }

      public bool Contains(ITestFixtureInfo item) {
         return _testFixtureInfos.Contains(item);
      }

      public void CopyTo(ITestFixtureInfo[] array, int arrayIndex) {
         _testFixtureInfos.CopyTo(array, arrayIndex);
      }

      public int Count {
         get {
            return _testFixtureInfos.Count;
         }
      }

      public bool IsReadOnly {
         get {
            return false;
         }
      }

      public bool Remove(ITestFixtureInfo item) {
         return _testFixtureInfos.Remove(item);
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
         return _testFixtureInfos.GetEnumerator();
      }

      #endregion

      private readonly List<ITestFixtureInfo> _testFixtureInfos = new List<ITestFixtureInfo>();
   }
}
