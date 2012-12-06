using System;
using System.Collections.Generic;

namespace NUnit_2._4._7 {
   class Calculator {
      public void Push(int i) {
         _stack.Push(i);
      }

      public void Multiply() {
         int a = _stack.Pop();
         int b = _stack.Pop();
         _stack.Push(a * b);
      }

      public int Peek() {
         if (_stack.Count > 0) {
            return _stack.Peek();
         }
         else {
            throw new ApplicationException("Stack is empty.");
         }
      }

      private readonly Stack<int> _stack = new Stack<int>();
   }
}
