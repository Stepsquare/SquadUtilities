using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using ZXing;

namespace SQUtilitiesNetFramework.QRCode
{
    /// <summary>
    /// Library to read QR codes inside PDF files and images. Limitation of one QR code per page or per image.
    /// </summary>
    public static class QRCodeManager
    {
        #region PUBLIC
        /// <summary>
        /// Gets the information of a QR code inside a PDF. Return a List of QR codes found on the pages
        /// of the pdf file.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static List<string> QRDecodePDF(string folder)
        {
            var pages = new PDFiumSharp.PdfDocument(folder).Pages;
            List<string> results = new List<string>();

            foreach (var i in pages)
            {
                using (var bmp = new PDFiumSharp.PDFiumBitmap((int)i.Width, (int)i.Height, false))
                {
                    i.Render(bmp);

                    MemoryStream ms = new MemoryStream();
                    bmp.Save(ms);
                    using (ms)
                    {
                        Bitmap img = new Bitmap(ms);
                        var res = QRDecode(img);
                        results.Add(res);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Gets the information of a QRCode that's inside an image.
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static string QRDecode(Bitmap bmp)
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            // load a bitmap
            var barcodeBitmap = bmp;
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(barcodeBitmap);
            // do something with the result
            if (result != null)
            {
                //var res1 = result.BarcodeFormat.ToString();
                var res2 = result.Text;

                return res2;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a bitmap of a given file.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static Bitmap GetBitmap(string pathtofile)
        {

            return (Bitmap)System.Drawing.Image.FromFile(pathtofile);
        }
        #endregion
    }
}
