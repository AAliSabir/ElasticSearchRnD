using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ElasticSearchTesting.XML.XmlGenerator
{
    public class Schema_XElement
    {
        public XmlSchemaSet set;
        public void getSchemaElement()
        {
            var xDoc = XDocument.Load("D://Sparrow//rnd//ElasticSearchRnD//ElasticSearchTesting//XML//pacs.028.001.04_0.xsd");
            var ns = XNamespace.Get(@"http://www.w3.org/2001/XMLSchema");

            var length = (from sType in xDoc.Element(ns + "schema").Elements(ns + "Cd")
                          where sType.Attribute("name").Value == "ExternalAccountIdentification1Code"
                          from r in sType.Elements(ns + "restriction")
                          select r.Element(ns + "maxLength").Attribute("value")
                                  .Value).FirstOrDefault();
        }


        public string GetXMLAsString(XmlDocument myxml)
        {

            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            myxml.WriteTo(tx);

            string str = sw.ToString();// 
            return str;
        }

        public void loadXMLSchemaElements()
        {
            var dictionary = new List<SchemaTypesXSD>();
            var typeRestrictionList = new List<SchemaRestrictionsJson>();
            XmlDocument schemadoc = new XmlDocument();
            schemadoc.Load("D://Sparrow//rnd//ElasticSearchRnD//ElasticSearchTesting//XML//pacs.028.001.04_0.xsd");
            string xml = GetXMLAsString(schemadoc);
            var xs = XNamespace.Get("http://www.w3.org/2001/XMLSchema");
            var doc = XDocument.Parse(xml);
            // if you have a file: var doc = XDocument.Load(<path to xml file>)
            foreach (var element in doc.Descendants(xs + "element"))
            {
                dictionary.Add(new SchemaTypesXSD(element.Attribute("name").Value,element.Attribute("type").Value));
                Console.WriteLine(element.Attribute("name").Value+"              " + "                       " + element.Attribute("type").Value);
            }

            foreach (var element in doc.Descendants(xs + "simpleType"))
            {
                string typeName = element.Attribute("name").Value;
                var list = element.Nodes();

                foreach (var item in list)
                {
                    Restriction restrictionInfo = ConvertNode<Restriction>(item);
                    typeRestrictionList.Add(new SchemaRestrictionsJson(typeName, JsonConvert.SerializeObject(restrictionInfo)));
                }
            }
        }

        public void loadSchema()
        {
            using (var fs = new FileStream("D://Sparrow//rnd//ElasticSearchRnD//ElasticSearchTesting//XML//pacs.028.001.04_0.xsd", FileMode.Open))
            {
                var schema = XmlSchema.Read(fs, null);

                set = new XmlSchemaSet();
                set.Add(schema);
                set.Compile();
            }

        }


        public Dictionary<string, int> GetElementMaxLength(String xsdElementName)
        {
            if (xsdElementName == null) throw new ArgumentException();
            // if your XSD has a target namespace, you need to replace null with the namespace name
            var qname = new XmlQualifiedName(xsdElementName, null);

            // find the type you want in the XmlSchemaSet    
            var parentType = set.GlobalTypes[qname];

            // call GetAllMaxLength with the parentType as parameter
            var results = GetAllMaxLength(parentType);

            return results;
        }

        private Dictionary<string, int> GetAllMaxLength(XmlSchemaObject obj)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            // do some type checking on the XmlSchemaObject
            if (obj is XmlSchemaSimpleType)
            {
                // if it is a simple type, then call GetMaxLength to get the MaxLength restriction
                var st = obj as XmlSchemaSimpleType;
                dict[st.QualifiedName.Name] = GetMaxLength(st);
            }
            else if (obj is XmlSchemaComplexType)
            {

                // if obj is a complexType, cast the particle type to a sequence
                //  and iterate the sequence
                //  warning - this will fail if it is not a sequence, so you might need
                //  to make some adjustments if you have something other than a xs:sequence
                var ct = obj as XmlSchemaComplexType;
                var seq = ct.ContentTypeParticle as XmlSchemaSequence;

                foreach (var item in seq.Items)
                {
                    // item will be an XmlSchemaObject, so just call this same method
                    //  with item as the parameter to parse it out
                    var rng = GetAllMaxLength(item);

                    // add the results to the dictionary
                    foreach (var kvp in rng)
                    {
                        dict[kvp.Key] = kvp.Value;
                    }
                }
            }
            else if (obj is XmlSchemaElement)
            {
                // if obj is an XmlSchemaElement, the you need to find the type
                //  based on the SchemaTypeName property.  This is why your 
                //  XmlSchemaSet needs to have class-level scope
                var ele = obj as XmlSchemaElement;
                var type = set.GlobalTypes[ele.SchemaTypeName];

                // once you have the type, call this method again and get the dictionary result
                var rng = GetAllMaxLength(type);

                // put the results in this dictionary.  The difference here is the dictionary
                //  key is put in the format you specified
                foreach (var kvp in rng)
                {
                    dict[String.Format("{0}/{1}", ele.QualifiedName.Name, kvp.Key)] = kvp.Value;
                }
            }

            return dict;
        }

        private Int32 GetMaxLength(XmlSchemaSimpleType xsdSimpleType)
        {
            // get the content of the simple type
            var restriction = xsdSimpleType.Content as XmlSchemaSimpleTypeRestriction;

            // if it is null, then there are no restrictions and return -1 as a marker value
            if (restriction == null) return -1;

            Int32 result = -1;

            // iterate the facets in the restrictions, look for a MaxLengthFacet and parse the value
            foreach (XmlSchemaObject facet in restriction.Facets)
            {
                if (facet is XmlSchemaMaxLengthFacet)
                {
                    result = int.Parse(((XmlSchemaFacet)facet).Value);
                    break;
                }
            }

            return result;
        }

        private static T ConvertNode<T>(XNode node) where T : class
        {
            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.ToString());
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(T));
            T result = (ser.Deserialize(stm) as T);

            return result;
        }

    }

    public class SchemaTypesXSD
    {
        public string name { get; set; }
        public string type { get; set; }

        public SchemaTypesXSD(string _name,string _type)
        {
            name = _name;
            type = _type;
        }
    }

    public class SchemaRestrictions
    {
        public string type { get; set; }
        public Restriction restriction { get; set; }

        public SchemaRestrictions(string _type, Restriction _restriction)
        {
            type = _type;
            restriction = _restriction;
        }
    }

    public class SchemaRestrictionsJson
    {
        public string type { get; set; }
        public string restriction { get; set; }

        public SchemaRestrictionsJson(string _type, string _restriction)
        {
            type = _type;
            restriction = _restriction;
        }
    }


}
