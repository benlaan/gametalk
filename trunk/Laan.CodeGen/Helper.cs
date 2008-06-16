using System;
using System.Linq;
using System.Text.RegularExpressions;

using Laan.CodeGen;
using Laan.Utilities.Extensions;
using System.Diagnostics;

namespace Laan.Risk.Entities.CodeGen
{
    public class Helper
    {
        public string ToCamelCase(string text)
        {
            return text.ToCamelCase();
        }

        public int MaxWidthType(Entity entity)
        {
            return entity.Properties.Max<Property>(p => GetDeclaredType(p).Length);
        }

        public int MaxWidthName(Entity entity)
        {
            return entity.Properties.Max<Property>(p => p.Name.Length);
        }

        public string FormattedWidth(string text, int width)
        {
            string format = String.Format("{{0, -{0}}}", width);
            return String.Format(format, text);
        }

        public string GetConstructor(Entity entity)
        {
            string constructor = "";
            string baseConstructor = "";

            var properties = 
                from p in entity.Properties
                where !p.List && p.InConstructor
                select p;

            foreach (var property in properties)
            {
                constructor += String.Format("{0} {1}, ", property.Type, property.Name.ToCamelCase());
                baseConstructor += String.Format("{0}, ", property.Name.ToCamelCase());
            }

            Char[] trim = new Char[] { ' ', ',' };
            return String.Format("({0}) : base({1})", constructor.TrimEnd(trim), baseConstructor.TrimEnd(trim));
        }

        public string GetBase(string name, string origin)
        {
            return String.Format("{0}.{1}.{0}", name, origin);
        }

        public string GetBaseClass(Entity entity, string origin)
        {
            string result;
            if (!String.IsNullOrEmpty(entity.BaseClass))
                result = String.Format("{0}.{1}.{2}", entity.BaseClass, origin, entity.BaseClass);
            else
                result = "BaseEntity" + origin;
            return result;
        }

        public string DeriveClassName(Entity entity, string section)
        {
            return String.Format("{0}.{1}", section, entity.Name);
        }

        public string GetInterfaceType(string propType)
        {
            if (!IsPrimitive(propType))
                return "I" + propType;
            else
                return propType;
        }

        public string GetTermEntity(string nameSpace)
        {
            string[] parts = Regex.Split(nameSpace, @"\.");
            return parts[parts.Length - 1];
        }

        public string GetModifyValue(Property property)
        {
            if (IsPrimitive(property.Type))
                return "value";
            else
                return String.Format("_{0}ID", property.Name.ToCamelCase());
        }

        public string GetModifyUpdate(Property property)
        {
            return GetModifyValue(property) + " = value.ID;";
        }

        public string GetDeclaredType(Property property)
        {
            if (IsPrimitive(property.Type))
                return property.Type;
            else
                if (!property.List)
                    return String.Format("{0}s.{0}", property.Type);
                else
                    return String.Format("{0}s.{1}", property.Type.Substring(0, property.Type.Length - 4), property.Type);
        }

        public bool NonInheritedProps(Entity entity)
        {
            return entity.Properties.Exists(p => !p.Inherited);
        }

        public bool IsPrimitive(string typeName)
        {
            switch (typeName)
            {
                case "Int32":
                case "String":
                case "DateTime":
                case "Boolean":
                case "System.Drawing.Color":
                    return true;
                default:
                    return false;
            }
        }
    }

}
