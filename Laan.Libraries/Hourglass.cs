using System;
using System.Windows.Forms;

namespace Laan.Utilities
{
    public class Hourglass : IDisposable
    {
        Form   _form;
        Cursor _cursor;

        static int _refCount = 0;

        public Hourglass(Form form)
        {
            _form = form;
            
            _refCount++;
            if (_refCount == 1)
            {
                // Store for later
                _cursor = form.Cursor;
                form.Cursor = Cursors.WaitCursor;
            }
        }

        public void Dispose()
        {
            _refCount--;

            // Restore to previous cursor
            if (_refCount == 0)
                _form.Cursor = _cursor;
        }
    }
}
