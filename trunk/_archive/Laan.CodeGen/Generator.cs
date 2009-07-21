using System.IO;

using NVelocity;
using NVelocity.App;
using System;
using System.Diagnostics;

namespace Laan.CodeGen
{
    class Token
    {
        public string Field { get; set; }
        public object Entity { get; set; }
    }

    internal class Generator
    {
        private VelocityEngine _engine;
        private Template       _template;

        public Generator()
        {
            _engine = new VelocityEngine();
            _engine.Init();
        }

        public void Execute(Token[] tokens)
        {
            _template = _engine.GetTemplate(Template);
            _template.ModificationCheckInterval = 10;

            VelocityContext context = new VelocityContext();

            foreach (var token in tokens)
                context.Put(token.Field, token.Entity);

            using (StringWriter writer = new StringWriter())
            {
                // Merge template, and write it to disk
                _template.Merge(context, writer);
                string outputFile = Path.Combine(OutputPath, FileName);

                Laan.Utilities.IO.Directory.ForceDirectory(outputFile);

                if (File.Exists(outputFile))
                    File.Delete(outputFile);

                File.WriteAllText(outputFile, writer.ToString());

                Debug.WriteLine("Output: " + outputFile);
            }
        }

        internal string Template { get; set; }
        internal string OutputPath { get; set; }
        internal string FileName { get; set; }
    }
}
