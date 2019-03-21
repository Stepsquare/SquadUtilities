using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.IO;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Utilities.Pdf
{
    public class PdfManager
    {
        private string filepath;
        private MemoryStream pdfStream;

        public PdfManager()
        {
            pdfStream = new MemoryStream();
        }

        /// <summary>
        /// Specifies the template if it's needed to place content.
        /// </summary>
        /// <param name="name"></param>
        public void SetFilePath(string path)
        {
            filepath = path;
        }

        /// <summary>
        /// Place an image on the pdf. Receives the page number, and position where it's going to be placed.
        /// </summary>
        /// <param name="pageNr"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public void PlaceImage(int pageNr, double positionX, double positionY)
        {
            // Open the template file as image
            XImage image = XImage.FromFile(filepath);

            // Open an existing document for editing and loop through its pages.
            var document = PdfReader.Open(filepath);
            // Set version to PDF 1.4 (Acrobat 5) because we use transparency.
            if (document.Version < 14)
                document.Version = 14;


            if (pageNr > document.Pages.Count)
            {
                throw new System.Exception("PageNr can't be superior to the number of pages in the pdf.");
            }

            var page = document.Pages[pageNr-1];

            // Get an XGraphics object for drawing beneath the existing content.
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

            gfx.DrawImage(image, positionX, positionY);

            document.Save(pdfStream);
        }

        /// <summary>
        /// Gets the file as a memorystream.
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetPdf()
        {
            return this.pdfStream;
        }

        /// <summary>
        /// Gets a file as memorystream from a url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public MemoryStream GetPdfFromUrl(string url)
        {
            string html = File.ReadAllText(url);
            PdfDocument pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.Undefined);

            MemoryStream ms = new MemoryStream();
            pdf.Save(ms);

            return ms;
        }
    }
}
