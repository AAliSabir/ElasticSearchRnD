using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ElasticSearchTesting.XML.XmlGenerator
{

		[XmlRoot(ElementName = "fractionDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class FractionDigits
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "totalDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class TotalDigits
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "minInclusive", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class MinInclusive
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "pattern", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class Pattern
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "enumeration", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class Enumeration
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "minLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class MinLength
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "maxLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class MaxLength
		{
			[XmlAttribute(AttributeName = "value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "restriction", Namespace = "http://www.w3.org/2001/XMLSchema")]
		public class Restriction
		{
			[XmlElement(ElementName = "fractionDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public FractionDigits FractionDigits { get; set; }
			[XmlElement(ElementName = "totalDigits", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public TotalDigits TotalDigits { get; set; }
			[XmlElement(ElementName = "minInclusive", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public MinInclusive MinInclusive { get; set; }
			[XmlElement(ElementName = "pattern", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public Pattern Pattern { get; set; }
			[XmlElement(ElementName = "enumeration", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public List<Enumeration> Enumeration { get; set; }
			[XmlElement(ElementName = "minLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public MinLength MinLength { get; set; }
			[XmlElement(ElementName = "maxLength", Namespace = "http://www.w3.org/2001/XMLSchema")]
			public MaxLength MaxLength { get; set; }
			[XmlAttribute(AttributeName = "base")]
			public string Base { get; set; }
			[XmlAttribute(AttributeName = "xs", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Xs { get; set; }
		}


}
