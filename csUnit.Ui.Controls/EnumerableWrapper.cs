#region Copyright © 2012 by Agile Utilities New Zealand Ltd. All rights reserved.
////////////////////////////////////////////////////////////////////////////////
//
// Copyright © 2012 by Agile Utilities New Zealand Ltd. All rights reserved.
//                  http://www.agileutilities.com
//
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

namespace csUnit.Ui.Controls {
   using System.Collections.Generic;

   /// <summary>
   /// This class serves as a generic adapter for turning IEnumerable into the
   /// generic, typesafe IEnumerable&lt;T&gt; and for turning IEnumerator into
   /// the generic, typesafe IEnumerator&lt;T&gt; of the same type.
   /// </summary>
   /// <typeparam name="T">Type of the element to be enumerated.</typeparam>
   /// <remarks>Some custom collections don't provide a typesafe way of 
   /// iterating, such as Control.Controls, which returns a ControlCollection
   /// object. The EnumerableWrapper&lt;T&gt; can be used to attach a generics 
   /// based enumerator.</remarks>
   internal class EnumerableWrapper<T> : IEnumerable<T> {
      public EnumerableWrapper(System.Collections.IEnumerable enumerable) {
         _enumerable = enumerable;
      }

      private class EnumeratorWrapper<U> : IEnumerator<U> {
         public EnumeratorWrapper(System.Collections.IEnumerator enumerator) {
            _enumerator = enumerator;
         }

         #region IDisposable Members

         public void Dispose() {
            _enumerator = null;
         }

         #endregion

         #region IEnumerator<U> Members

         U IEnumerator<U>.Current {
            get {
               return (U)_enumerator.Current;
            }
         }

         #endregion

         #region IEnumerator Members

         public bool MoveNext() {
            return _enumerator.MoveNext();
         }

         public void Reset() {
            _enumerator.Reset();
         }

         object System.Collections.IEnumerator.Current {
            get {
               return _enumerator.Current;
            }
         }

         #endregion

         private System.Collections.IEnumerator _enumerator;
      }

      #region IEnumerable<T> Members

      public IEnumerator<T> GetEnumerator() {
         return new EnumeratorWrapper<T>(_enumerable.GetEnumerator());
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
         return new EnumeratorWrapper<T>(_enumerable.GetEnumerator());
      }

      #endregion

      private readonly System.Collections.IEnumerable _enumerable;
   }
}