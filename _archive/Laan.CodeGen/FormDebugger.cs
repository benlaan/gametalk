using System.Diagnostics;

using Laan.Library.Debugging;

namespace Laan.CodeGen
{
    public class FormDebugger : DefaultTraceListener
    {
        public FormDebugger(IDebuggable form)
        {
            _form = form;
        }

        private IDebuggable _form;

        public override void Write(string data)
        {
            _form.Write(data);
        }

        public override void WriteLine(string data)
        {
            _form.WriteLine(data);
        }
    }
}
