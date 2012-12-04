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

namespace csUnit.Ui.Controls.TabPages {
   using System;
   using System.Collections;
   using System.Windows.Forms;

   /// <summary>
   /// Summary description for StatisticsListSorter.
   /// </summary>
   internal class StatisticsListSorter : IComparer {
      public int SortColumn {
         set {
            if( _column == value ) {
               _ascending = _ascending == 1 ? -1 : 1;
            }
            _column = value;
         }
      }

      #region Implementation of IComparer
      public int Compare(object x, object y) {
         ListViewItem itemx = (ListViewItem) x;
         ListViewItem itemy = (ListViewItem) y;

         switch(_column) {
            // TODO: use constants instead of numbers for better understanding
            // of them. [2006feb11, ml]
            case 0:
            case 1:
               return _ascending * itemx.SubItems[_column].Text.CompareTo(itemy.SubItems[_column].Text);
            case 2:
            case 3:
            case 4:
               double valuex = Convert.ToDouble(itemx.SubItems[_column].Text);
               double valuey = Convert.ToDouble(itemy.SubItems[_column].Text);
               return _ascending * valuex.CompareTo(valuey);
         }

         return _ascending * itemx.Text.CompareTo(itemy.Text);
      }
      #endregion

      private int _column = 0;
      private int _ascending = 1;
   }
}
