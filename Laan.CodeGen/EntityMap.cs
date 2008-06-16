using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Laan.CodeGen
{
    public class PropertyList : List<Property>
    {
        public List<Property> Inherited
        {
            get { return this.Where<Property>(p => p.Inherited).ToList<Property>(); }
        }

        public List<Property> NonInherited
        {
            get { return this.Where<Property>(p => !p.Inherited).ToList<Property>(); }
        }
    }

    [Serializable]
    [XmlRoot("entityMap")]
    public class EntityMap
    {
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlArray("entities"), XmlArrayItem("entity")]
        public List<Entity> Entities { get; set; }
    }

    [Serializable]
    public class Entity
    {
        [XmlArray("properties"), XmlArrayItem("property")]
        public PropertyList Properties { get; set; }

        [XmlArray("requirements"), XmlArrayItem("requirement")]
        public List<Requirement> Requirements { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("baseClass")]
        public string BaseClass { get; set; }

        [XmlAttribute("implements")]
        public string Implements { get; set; }

        [XmlAttribute("abstract")]
        public bool Abstract { get; set; }

        [XmlAttribute("namespace")]
        public string Namespace { get; set; }
    }

    [Serializable]
    public class Property
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("list")]
        public bool List { get; set; }

        [XmlAttribute("serverProtectedOnly")]
        public string ServerProtectedOnly { get; set; }

        [XmlAttribute("clientProtectedOnly")]
        public string ClientProtectedOnly { get; set; }
        
        [XmlAttribute("inherited")]
        public bool Inherited { get; set; }
        
        [XmlAttribute("inConstructor")]
        public bool InConstructor { get; set; }

        [XmlAttribute("shortType")]
        public string ShortType { get; set; }
    }

    [Serializable]
    public class Requirement
    {
        [XmlAttribute("namespace")]
        public string Namespace { get; set; }
    }
}
