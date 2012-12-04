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

using System.Collections.Generic;
using System.IO;

namespace csUnit.Core {
   /// <summary>
   /// The TextWriterMultiplexer is a special StringWriter derivate that can be
   /// used to have stream being written to one or more other TextWriters.
   /// </summary>
   /// <remarks>The console takes only a single text writer, but csUnitCore
   /// should only intercept the console for its own purposes but allow tests
   /// to redirect the console as well. The TextWriterMultiplexer allows for
   /// exactly that.</remarks>
   internal class TextWriterMultiplexer : StringWriter {
      public void AddTextWriter(TextWriter writer) {
         if(writer == null) {
            return;
         }
         if(!_writers.Contains(writer)) {
            _writers.Add(writer);
         }
      }

      #region Write overrides
      public override void Write(bool value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(char value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(char[] buffer) {
         foreach(TextWriter writer in _writers) {
            writer.Write(buffer);
         }
      }

      public override void Write(char[] buffer, int index, int count) {
         foreach(TextWriter writer in _writers) {
            writer.Write(buffer, index, count);
         }
      }

      public override void Write(decimal value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(double value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(float value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(int value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(long value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(object value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(string format, object arg0) {
         foreach(TextWriter writer in _writers) {
            writer.Write(format, arg0);
         }
      }

      public override void Write(string format, object arg0, object arg1) {
         foreach(TextWriter writer in _writers) {
            writer.Write(format, arg0, arg1);
         }
      }

      public override void Write(string format, object arg0, object arg1, object arg2) {
         foreach(TextWriter writer in _writers) {
            writer.Write(format, arg0, arg1, arg2);
         }
      }

      public override void Write(string format, params object[] arg) {
         foreach(TextWriter writer in _writers) {
            writer.Write(format, arg);
         }
      }

      public override void Write(string value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(uint value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }

      public override void Write(ulong value) {
         foreach(TextWriter writer in _writers) {
            writer.Write(value);
         }
      }
      #endregion // Write

      #region WriteLine
      public override void WriteLine(char[] buffer) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(buffer);
         }
      }

      public override void WriteLine(char[] buffer, int index, int count) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(buffer, index, count);
         }
      }

      public override void WriteLine(decimal value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(double value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(float value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(int value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(long value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(object value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(string format, object arg0) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(format, arg0);
         }
      }

      public override void WriteLine(string format, object arg0, object arg1) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(format, arg0, arg1);
         }
      }

      public override void WriteLine(string format, object arg0, object arg1, object arg2) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(format, arg0, arg1, arg2);
         }
      }

      public override void WriteLine(string format, params object[] arg) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(format, arg);
         }
      }

      public override void WriteLine(string value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(uint value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(ulong value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine() {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine();
         }
      }

      public override void WriteLine(char value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }

      public override void WriteLine(bool value) {
         foreach(TextWriter writer in _writers) {
            writer.WriteLine(value);
         }
      }
      #endregion // WriteLine

      private readonly List<TextWriter> _writers = new List<TextWriter>();
   }

}
