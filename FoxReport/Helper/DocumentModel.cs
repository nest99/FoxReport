using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mshtml;
using log4net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Linq;
using OpenXmlPowerTools;

namespace FoxReport.Helper
{
    public class DocumentModel : Loggable<DocumentModel>
    {
        public string BaseDirectory
        {
            get;
            set;
        }
        private string workingDirectory;
        private string absolutePath;
        private string content;
        ~DocumentModel()
        {
        }

        public static DocumentModel Load(string uri, string baseDirectory, Identity identity)
        {
            HttpSteamQuerier querier = new HttpSteamQuerier()
            {
                Identity = identity
            };
            try
            {
                Stream stream = querier.ReadAsStreamAsync(uri).Result;
                return Load(stream, baseDirectory);
            }
            catch (Exception ex)
            {
                Logger.Error("Load Document Error", ex);
                throw ex;
            }

        }
        public static DocumentModel Load(Stream stream, string baseDirectory)
        {

            StreamReader reader = new StreamReader(stream, true);
            return Load(reader.ReadToEnd(), baseDirectory);
        }
        public static DocumentModel Load(string html, string baseDirectory)
        {
            DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            var doc = new DocumentModel()
            {
                BaseDirectory = baseDirectory,
                workingDirectory = directory.FullName
            };
            doc.content = html;
            doc.ReplaceToAbsolutePath();
            doc.absolutePath = Path.Combine(doc.workingDirectory, "document.html");
            File.AppendAllText(doc.absolutePath, doc.content);

            return doc;
        }

        private void ReplaceToAbsolutePath()
        {
            Regex reg = new Regex("(href|src)=([\"'])([^\"']*)([\"'])", RegexOptions.IgnoreCase);
            try
            {
                this.content = reg.Replace(this.content, m =>{
                    return string.Format("{0}={1}{2}{3}", m.Groups[1].Value, m.Groups[2].Value,
                        !m.Groups[1].Value.StartsWith("http") 
                        ? Path.Combine(this.BaseDirectory.ToString(), m.Groups[3].Value)
                        : m.Groups[3].Value, m.Groups[4].Value);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DocumentModel Transform(Action<HTMLDocumentClass> interop)
        {
            HTMLDocumentClass myDocument = new HTMLDocumentClass();
            IHTMLDocument2 doc2 = myDocument;
            doc2.write(new object[] { this.content });
            interop(myDocument);
            this.content = myDocument.documentElement.outerHTML;
            string fileName = string.Format("{0}-transformed.html", DateTime.Now.ToString("hh:mm:ss-fffff"));
            this.absolutePath = Path.Combine(this.workingDirectory, fileName);
            File.WriteAllText(this.absolutePath, this.content, Encoding.UTF8);
            return this;
        }

        public DocumentModel RemoveElementsByIds(params string[] elements)
        {
            return Transform(doc =>
            {
                elements.ToList().ForEach(element =>
                {
                    var eleRemove = doc.getElementById(element) as IHTMLDOMNode;
                    if (eleRemove != null)
                    {
                        eleRemove.parentNode.removeChild(eleRemove);
                    }
                });
            });
        }

        public DocumentModel RemoveElementsByTagName(string tagName)
        {
            return Transform(doc =>
            {
                doc.getElementsByTagName(tagName).OfType<IHTMLDOMNode>().ToList().ForEach(eleRemove =>
                {
                    eleRemove.parentNode.removeChild(eleRemove);
                });
            });
        }

        public string SaveAs(string saveTo)
        {
            XElement xhtml = HtmlToWmlReadAsXElement.ReadAsXElement(new FileInfo(this.absolutePath));
            xhtml.Descendants().Where(i => i.Name.LocalName.ToLower() == "script").Remove();
            string linkCss = "";
            string headerCss = "";
            try
            {
                var linkStyleSheets = xhtml.Descendants().Where(d => d.Name.LocalName.ToLower() == "link"
                    && d.Attribute("rel").Value.ToLower() == "stylesheet").Select(d => File.ReadAllText(d.Attribute("href").Value));
                linkCss = HtmlToWmlConverter.CleanUpCss(string.Join("\r\n", linkStyleSheets));
                headerCss = HtmlToWmlConverter.CleanUpCss((string)xhtml.Descendants().FirstOrDefault(d => d.Name.LocalName.ToLower() == "style"));
                linkCss = linkCss.Trim('\r', '\n');
            }
            catch (Exception ecss)
            {
                Logger.Error("获取CSS出错", ecss);
            }
            File.WriteAllText(Path.Combine(this.workingDirectory, "dump-link.css"), linkCss);
            File.WriteAllText(Path.Combine(this.workingDirectory, "dump-header.css"), headerCss);
            HtmlToWmlConverterSettings settings = HtmlToWmlConverter.GetDefaultSettings();
            try
            {
                WmlDocument word = HtmlToWmlConverter.ConvertHtmlToWml(defaultCss, linkCss, headerCss, xhtml, settings);
                word.SaveAs(saveTo);
            }
            catch (Exception ew)
            {
                Logger.Error("保存Word出错", ew);
            }
#if !DEBUG
            try
            {
                Directory.Delete(this.workingDirectory, true);
            }
            catch (Exception ex)
            {
                Logger.Error("Delete Directory error. Directory=" + this.workingDirectory, ex);
            }
#endif
            return saveTo;
        }
        private static string defaultCss = Resource1.defaultWord;
    }
}