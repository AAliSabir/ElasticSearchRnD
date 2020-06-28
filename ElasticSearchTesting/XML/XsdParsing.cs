using ElasticSearchTesting.XML.XmlGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;


namespace ElasticSearchTesting.XML
{
    public class XsdParsing
    {

        public void XsdToXml(string xsdPath, string xmlPath)
        {
            //XSD -> XML and Getting all XPATH from XML work?

            Console.WriteLine("Reading Xsd ...");

            XmlTextReader reader = new XmlTextReader(xsdPath);
            XmlSchema myschema = XmlSchema.Read(reader, ValidationCallback);
            //myschema.Write(Console.Out);

            Console.WriteLine("Xsd Reading Complete");

            Console.WriteLine("Creating Xml ...");

            XmlTextWriter textWriter = new XmlTextWriter(xmlPath, null);
            textWriter.Formatting = Formatting.Indented;
            XmlQualifiedName qname = new XmlQualifiedName("Document", "http://tempuri.org");

            XmlSampleGenerator generator = new XmlSampleGenerator(myschema, qname);
            generator.WriteXml(textWriter);

            Console.WriteLine("Xml Created and Stored Successfully !");

        }

        void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
    }
}
