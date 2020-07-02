using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ElasticSearchTesting.XML.XmlGenerator
{
    public class XPathGenerator
    {
        public List<String> xPathList = new List<string>();
        public List<String> xPath_Leaf = new List<string>();


        public void ExamineNode(XmlNode node, String parentPath)
        {
             String nodePath = parentPath + '/' + node.Name;

            if (!(node is XmlText))
            {
                xPathList.Add(nodePath);

                foreach (XmlNode childNode in node.ChildNodes)
                    ExamineNode(childNode, nodePath);
            }
            else
            {
                var path = nodePath.Replace("/#text", "");
                if (!xPath_Leaf.Contains(path))
                xPath_Leaf.Add(path);
            }
        }
    }
}
