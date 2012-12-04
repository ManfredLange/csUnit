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

namespace csUnit.Ui.Controls {
   using System;
   using System.Windows.Forms;

   /// <summary>
   /// Summary description for CsUnitControlEventArgs.
   /// </summary>
   [Serializable]
   public class CsUnitControlEventArgs : EventArgs {
      public CsUnitControlEventArgs(ContextMenuStrip menu, UiElementInfo elementInfo) {
         _menu = menu;
         _elementInfo = elementInfo;
      }

      public ContextMenuStrip Menu {
         get {
            return _menu;
         }
      }

      public UiElementInfo ElementInfo {
         get {
            return _elementInfo;
         }
      }

      private readonly ContextMenuStrip   _menu = null;
      private readonly UiElementInfo      _elementInfo = null;
   }

   /// <summary>
   /// Delegate for handling events sent by a CsUnitControl instance.
   /// </summary>
   public delegate void CsUnitControlEventHandler(object sender, CsUnitControlEventArgs args);
}
