using System;
using System.Diagnostics;
using System.Windows.Forms;
using Laan.Utilities;
using Laan.Library.Debugging;
using Laan.Risk.Entities.CodeGen;

namespace Laan.CodeGen
{
    internal class CodeGenerator
    {
        internal static bool Run(string entityMap, string output)
        {
            bool hasErrors = false;

            Debug.WriteLine("start: Execute");
            try
            {
                GameLibraryGenerator generator = new GameLibraryGenerator();
                generator.OutputPath = output;
                generator.Execute(entityMap);

                Debug.WriteLine("end: Execute");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                hasErrors = true;
            }
            return hasErrors;
        }
    }
}
