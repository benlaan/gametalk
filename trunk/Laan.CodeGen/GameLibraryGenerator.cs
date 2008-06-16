using System.Diagnostics;

using Laan.Utilities.Xml;
using Laan.Utilities.Extensions;
using System;
using Laan.Risk.Entities.CodeGen;

namespace Laan.CodeGen
{
    internal class GameLibraryGenerator
    {
        private Helper      _helper;
        private Generator   _generator;

        public GameLibraryGenerator()
        {
            _generator = new Generator();
            _helper    = new Helper();
        }

        private void GenerateFile(Entity entity, string template, string fileNameFormat)
        {
            string fileName = fileNameFormat.Formatted(entity.Name);
            Debug.WriteLine("{0,-30} -> {1}".Formatted(template, fileName));

            _generator.Template = template;
            _generator.OutputPath = OutputPath;
            _generator.FileName = fileName;
            _generator.Execute(
                new Token[]
                {
                    new Token { Field = "entity", Entity = entity },
                    new Token { Field = "helper", Entity = _helper }
                }
            );
        }

        public void Execute(string entityMap)
        {
            EntityMap map = XmlPersistence<EntityMap>.LoadFromFile(entityMap);

            foreach (Entity entity in map.Entities)
            {
                GenerateFile(entity, @"..\..\BaseEntity.vm", "{0}.g.cs");
                GenerateFile(entity, @"..\..\Entity.vm", "{0}.cs");
            }
        }

        public string OutputPath { get; set; }
    }
}
