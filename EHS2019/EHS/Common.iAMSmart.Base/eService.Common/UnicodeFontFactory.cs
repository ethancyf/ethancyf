using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class UnicodeFontFactory : FontFactoryImp
    {
        //private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Font),
        //            "arialuni.ttf");//arial unicode MS是完整的unicode字型。
        //private static readonly string 標楷體Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        //  "KAIU.TTF");//標楷體


        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
            bool cached)
        {
            //string fontPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/");
            BaseFont baseFont = BaseFont.CreateFont("C://WINDOWS//Fonts//simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font f = new Font(baseFont, 10, Font.NORMAL);
            return f;
            ////可用Arial或標楷體，自己選一個
            //BaseFont baseFont = BaseFont.CreateFont(標楷體Path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //return new Font(baseFont, size, style, color);
        }

        public override Font GetFont(string fontname = "")
        {
            if (string.IsNullOrEmpty(fontname))
            {
                fontname = "C://WINDOWS//Fonts//simsun.ttc,1";
            }
            BaseFont baseFont = BaseFont.CreateFont(fontname, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font f = new Font(baseFont, 10, Font.NORMAL);
            return f;
        }
    }
}
