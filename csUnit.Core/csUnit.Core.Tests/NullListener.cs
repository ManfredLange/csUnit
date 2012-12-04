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

using csUnit.Interfaces;

namespace csUnit.Core.Tests {
   /// <summary>
   /// NullListener implements ITestListener as no-ops and is used for testing
   /// the core only. If a test for the core is interested in something more
   /// specific, it typically implements a private listener deriving from this
   /// class.
   /// </summary>
   public class NullListener : ITestListener {
      #region Implementation of ITestListener
      public virtual void OnAssemblyLoaded(object sender, AssemblyEventArgs args){
         // Intentionally empty.
      }
      
      public virtual void OnAssemblyStarted(object sender, AssemblyEventArgs args) {
         // Intentionally empty.
      }
      
      public virtual void OnAssemblyFinished(object sender, AssemblyEventArgs args) {
         // Intentionally empty.
      }
      
      public virtual void OnTestsAborted(object sender, AssemblyEventArgs args) {
         // Intentionally empty.
      }

      public virtual void OnTestStarted(object sender, TestResultEventArgs args) {
         // Intentionally empty.
      }
      
      public virtual void OnTestSkipped(object sender, TestResultEventArgs args) {
         // Intentionally empty.
      }
      
      public virtual void OnTestError(object sender, TestResultEventArgs args) {
         // Intentionally empty.
      }
      
      public virtual void OnTestPassed(object sender, TestResultEventArgs args) {
         // Intentionally empty.
      }
      
      public virtual void OnTestFailed(object sender, TestResultEventArgs args) {
         // Intentionally empty.
      }
      
      #endregion
   }
}
