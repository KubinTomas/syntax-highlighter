using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyntaxHighlighter.Formatter
{
    public class XmlSHFormatter : AbstractSHFormatter
    {
        public byte[] byteArray { get; private set; }

        public override string Footer()
        {
            return "";
        }

        public override string Format(Token token)
        {
            return "";
        }

        public override string Header()
        {
            return "";
        }
        public override void Run()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Create an XML document. Write our specific values into the document.
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.ASCII);

                // Write the XML document header.
                xmlWriter.WriteStartDocument();

                // Write our first XML header.
                xmlWriter.WriteStartElement("Tokens");

                // Write an element representing a single web application object.
                foreach (var token in source)
                {
                    xmlWriter.WriteStartElement("Token");

                    xmlWriter.WriteElementString("Text", token.Text);
                    xmlWriter.WriteElementString("Type", token.Type.ToString());
                    xmlWriter.WriteElementString("Position:ROW-NOT-WORKING", token.TokenPosition.RowIndex.ToString());
                    xmlWriter.WriteElementString("Position:COL-NOT-WORKING", token.TokenPosition.ColIndex.ToString());

                    // End the element Token
                    xmlWriter.WriteEndElement();
                }

                // End the element Tokens
                xmlWriter.WriteEndElement();

                // Finilize the XML document by writing any required closing tag.
                xmlWriter.WriteEndDocument();

                // To be safe, flush the document to the memory stream.
                xmlWriter.Flush();

                // Convert the memory stream to an array of bytes.
                byteArray = stream.ToArray();

                xmlWriter.Close();
            }
        }
    }
}
