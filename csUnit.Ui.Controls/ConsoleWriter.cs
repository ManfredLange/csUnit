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

using System.IO;
using System.Windows.Forms;

namespace csUnit.Ui.Controls {
   /// <summary>
   /// The ConsoleWriter class redirects its input to a control. Its typical
   /// use is to redirect the console output to the "Output" tab of the main
   /// form of csUnitRunner.
   /// </summary>
   internal class ConsoleWriter : StringWriter {
      /// <summary>
      /// Constructs a ConsoleWriter object.
      /// </summary>
      /// <param name="control">The control to which the output is written.</param>
      public ConsoleWriter(Control control) {
         _control = control;
         _lineCount = 0;
      }

      /// <summary>
      /// Clears the content of the console writer. Sets the control text to
      /// an empty string.
      /// </summary>
      public void Clear() {
         base.GetStringBuilder().Length = 0;
         UpdateControlText(base.ToString());
         _lineCount = 0;
      }

      #region Overrides
      public override void Write(string value) {
         base.Write(value);
         UpdateControlText(base.ToString());
      }

      public override void Write(char[] buffer, int index, int count) {
         base.Write(buffer, index, count);
         UpdateControlText(base.ToString());
      }

      public override void Write(char value) {
         base.Write(value);
         UpdateControlText(base.ToString());
      }

      public override void WriteLine(string value) {
         if(ConfigCurrentUser.ShowLineNumbersInOutput) {
            value = ++_lineCount + ": " + value;
         }
         base.WriteLine(value);
         UpdateControlText(base.ToString());
      }
      #endregion

      /// <summary>
      /// This method updates the text property of the underlying control. If
      /// necessary Invoke() will be used to set the text on the proper thread.
      /// </summary>
      /// <param name="text"></param>
      private void UpdateControlText(string text) {
         if(_control.InvokeRequired) {
            _control.Invoke(new UpdateDelegate(UpdateControlText), new object[] { text });
         }
         else {
            _control.Text = text;
         }
      }

      private delegate void UpdateDelegate(string text);

      private readonly Control _control;
      private int _lineCount;
   }
}
