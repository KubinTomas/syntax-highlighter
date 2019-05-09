using SyntaxHighlighter;
using SyntaxHighlighter.Filters;
using SyntaxHighlighter.Formatter;
using SyntaxHighlighter.Interfaces;
using SyntaxHighlighter.Tokenizers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace SyntaxHighlighterWEB.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveTokens(string data)
        {
            // create tmp path with string data
            var fileName = GenerateFileName("txt");
            var path = Server.MapPath("~/" + fileName);
            System.IO.File.WriteAllText(path, data);
            return Json(path);
        }
        public static string GenerateFileName(string extension = "")
        {
            return string.Concat(Path.GetRandomFileName().Replace(".", ""),
                (!string.IsNullOrEmpty(extension)) ? (extension.StartsWith(".") ? extension : string.Concat(".", extension)) : "");
        }
        [ValidateInput(false)]
        public void DownloadXml(string path)
        {
            // read string data from tmp file and then delete
            string data = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);

            ISHTokenizer tokenizer = new CSharpSHTokenizer();
            tokenizer.SetInput(new StringReader(data));
            ISHFilter tokenFilter = new CSharpTokenFilter(tokenizer.GetTokenRecognizer());
            tokenFilter.SetSource(tokenizer);
            var revisedTokens = tokenFilter.ReviseTokens();

            //ISHFormater formater = new HtmlSHFormatter(); 
            //formater.SetSource(revisedTokens);
            //formater.Run();

            //var formatedToknes = formater.GetFormatedTokens();

            using (MemoryStream stream = new MemoryStream())
            {
                // Create an XML document. Write our specific values into the document.
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.ASCII);

                // Write the XML document header.
                xmlWriter.WriteStartDocument();

                // Write our first XML header.
                xmlWriter.WriteStartElement("Tokens");

                // Write an element representing a single web application object.
                foreach (var token in revisedTokens)
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
                byte[] byteArray = stream.ToArray();

                // Send the XML file to the web browser for download.
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=XMLFile.xml");
                Response.AppendHeader("Content-Length", byteArray.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(byteArray);
                xmlWriter.Close();
                Response.End();

                //string xmlString = xmlWriter.ToString();
                //string fileName = "test" + ".xml";
                //return File(Encoding.UTF8.GetBytes(xmlString), "application/xml", fileName);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FormatTokens(string data)
        {
              ISHTokenizer tokenizer = new CSharpSHTokenizer();
            tokenizer.SetInput(new StringReader(data));

            ISHFilter tokenFilter = new CSharpTokenFilter(tokenizer.GetTokenRecognizer());
            tokenFilter.SetSource(tokenizer);
            var revisedTokens = tokenFilter.ReviseTokens();

            ISHFormater formater = new HtmlSHFormatter(); // vytvoříme formátor
            formater.SetSource(revisedTokens);
            //formater.SetOutput(new StreamWriter("formated.txt"));
            formater.Run();

            var formatedToknes = formater.GetFormatedTokens();

            return Json(formatedToknes);
        }
    }
}