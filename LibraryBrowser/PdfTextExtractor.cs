using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;

namespace DirectoryContentTool
{
    public static class PdfTextExtractor
    {
        public static string ReadPdfDocument(string path)
        {
            PdfDocument document = new PdfDocument(new PdfReader(path));
            string text = string.Empty;
            for (int page = 1; page <= document.GetNumberOfPages(); page++)
            {
                var a = document.GetPage(page);
            }
            return text;
        }
    }
}
