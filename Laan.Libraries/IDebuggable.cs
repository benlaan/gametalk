using System;

namespace Laan.Library.Debugging
{
    public interface IDebuggable
    {
        void Write(string message);
        void WriteLine(string message);
    }
}