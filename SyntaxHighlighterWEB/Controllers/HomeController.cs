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
        ISHTokenizer tokenizer = null;
        ISHFilter tokenFilter = null;

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
        private void SetUpTokenizers(string language)
        {
            switch (language)
            {
                case "Java":
                    tokenizer = new JavaSHTokenizer();
                    tokenFilter = new JavaTokenFilter(tokenizer.GetTokenRecognizer());
                    break;
                case "CSharp":
                    tokenizer = new CSharpSHTokenizer();
                    tokenFilter = new CSharpTokenFilter(tokenizer.GetTokenRecognizer());
                    break;
                default:
                    tokenizer = new CSharpSHTokenizer();
                    tokenFilter = new CSharpTokenFilter(tokenizer.GetTokenRecognizer());
                    break;
            }
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
        public void DownloadXml(string path, string language)
        {
            // read string data from tmp file and then delete
            string data = System.IO.File.ReadAllText(path);
            System.IO.File.Delete(path);

            SetUpTokenizers(language);

            tokenizer.SetInput(new StringReader(data));
            tokenFilter.SetSource(tokenizer);
            var revisedTokens = tokenFilter.ReviseTokens();

            ISHFormater formater = new XmlSHFormatter();
            formater.SetSource(revisedTokens);
            formater.Run();


            byte[] byteArray = null;

            if (formater is XmlSHFormatter)
                byteArray = ((XmlSHFormatter)formater).byteArray;
            else throw new Exception();

            // Send the XML file to the web browser for download.
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=XMLFile.xml");
            Response.AppendHeader("Content-Length", byteArray.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(byteArray);
            Response.End();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FormatTokensHtml(string data, string language)
        {
            SetUpTokenizers(language);

            tokenizer.SetInput(new StringReader(data));

            tokenFilter.SetSource(tokenizer);
            var revisedTokens = tokenFilter.ReviseTokens();

            ISHFormater formater = new HtmlSHFormatter();
            formater.SetSource(revisedTokens);
            formater.Run();

            var formatedToknes = formater.GetFormatedTokens();

            return Json(formatedToknes);
        }
    }
}