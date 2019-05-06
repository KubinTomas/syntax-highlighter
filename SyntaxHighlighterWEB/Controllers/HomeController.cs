using SyntaxHighlighter.Filters;
using SyntaxHighlighter.Formatter;
using SyntaxHighlighter.Interfaces;
using SyntaxHighlighter.Tokenizers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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