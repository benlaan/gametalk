using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laan.CodeGen
{
    public interface IHelperPlugin
    {
        object CreateHelper();
        string Name { get; }
    }
}
