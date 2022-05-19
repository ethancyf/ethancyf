using Common.Component.iAMSmart;
using eService.DTO.JSONSearializer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace eService.Common
{
    public static class CAUtils
    {
        private static string HKICPattern = "^([A-Z]{1,2}[0-9]{6})\\(([A0-9])\\)$";// len 8~9

        public static readonly LogUtils LogUtils = new LogUtils(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SHA-256withRSA验证签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static bool VerifySignature(byte[] data, byte[] signature, AsymmetricKeyParameter publicKey)
        {
            try
            {
                ISigner signer = SignerUtilities.GetSigner("SHA-256withRSA");
                signer.Init(false, publicKey);
                signer.BlockUpdate(data, 0, data.Length);

                return signer.VerifySignature(signature); //验签结果
            }
            catch (Exception ex)
            {
                LogUtils.Error("SHA-256withRSA VerifySignature failed!exception:" + ex.Message);
                return false;
            }

        }

        public static string GetPDFSignTempPath()
        {
            string tempPath = CAConstants.CA_SIGN_PDF_NAS + Path.DirectorySeparatorChar
                + DateTimeUtils.FormatDateTime(DateTime.Now, CAConstants.CA_SIGN_FILE_PATH_FORMAT) + Path.DirectorySeparatorChar
                 + "pdfsigning" + Path.DirectorySeparatorChar;
            LogUtils.Debug("get pdfSign tempPath:" + tempPath);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        public static string GetHashSignUpLoadTempPath()
        {
            string tempPath = CAConstants.CA_SIGN_PDF_NAS + Path.DirectorySeparatorChar
                + DateTimeUtils.FormatDateTime(DateTime.Now, CAConstants.CA_SIGN_FILE_PATH_FORMAT) + Path.DirectorySeparatorChar
                + "signing" + Path.DirectorySeparatorChar + "upload" + Path.DirectorySeparatorChar;
            LogUtils.Debug("GetHashSignUpLoadTempPath tempPath:" + tempPath);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        public static string GetHashSignXMLPath()
        {
            string tempPath = CAConstants.CA_SIGN_PDF_NAS + Path.DirectorySeparatorChar
                + DateTimeUtils.FormatDateTime(DateTime.Now, CAConstants.CA_SIGN_FILE_PATH_FORMAT) + Path.DirectorySeparatorChar
                + "signing" + Path.DirectorySeparatorChar + "xml" + Path.DirectorySeparatorChar;
            LogUtils.Debug("GetHashSignXMLPath Path:" + tempPath);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        public static string GetPDFSignXMLPath()
        {
            string tempPath = CAConstants.CA_SIGN_PDF_NAS + Path.DirectorySeparatorChar
                + DateTimeUtils.FormatDateTime(DateTime.Now, CAConstants.CA_SIGN_FILE_PATH_FORMAT) + Path.DirectorySeparatorChar
                + "pdfsigning" + Path.DirectorySeparatorChar + "xml" + Path.DirectorySeparatorChar;
            LogUtils.Debug("GetPDFSignXMLPath Path:" + tempPath);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        public static string GetHKICHashByProfile(string profileStr)
        {
            if (string.IsNullOrEmpty(profileStr))
            {
                return null;
            }
            var profileDTO = JsonUtils.Deserialize<ProfileDTO>(profileStr);
            if (profileDTO == null || profileDTO.IDNo == null)
            {
                return null;
            }
            string identification = profileDTO.IDNo.Identification;
            string checkDigit = profileDTO.IDNo.CheckDigit;

            if (string.IsNullOrEmpty(identification) || string.IsNullOrEmpty(checkDigit))
            {
                return null;
            }

            string hkic = identification + "(" + checkDigit + ")";
            if (!CheckHKIC(hkic))
            {
                return null;
            }
            string trimIdentification = identification.Trim();
            return CommonHelper.EncodeBase64(EncryptUtils.SHA256EncryptToByte(trimIdentification));
        }

        public static byte[] GetSignHash(string formData, string businessID, string docID, RedisHelper redisHelper)
        {
            string formDataStr = CommonHelper.DecodeBase64(formData, "utf-8");
            string temp_path = GetHashSignFormTempPath();
            string formPdfPath = temp_path + businessID + ".pdf";
            HtmlToPdf(formDataStr, formPdfPath);

            //将保存的pdf文件创建为 SHA256 hash
            FileStream fsFormPdf = new FileStream(formPdfPath, FileMode.Open);
            fsFormPdf.Position = 0;
            byte[] formPdfDigest = EncryptUtils.SHA256EncryptToByte(fsFormPdf);
            byte[] sourceBytes = new byte[formPdfDigest.Length];
            Array.Copy(formPdfDigest, 0, sourceBytes, 0, formPdfDigest.Length);
            fsFormPdf.Close();

            if (!string.IsNullOrEmpty(docID))
            {
                string[] docs = docID.Split(',');
                byte[] sourceBytesTemp = null;
                foreach (var doc in docs)
                {
                    //string docPath = redisHelper.Get<string>(CacheKeyUtils.GetSignFileExtKey(doc));
                    iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                    string docPath = udtIAMSmartBLL.GetCacheValueByKey(CacheKeyUtils.GetSignFileExtKey(doc));

                    FileStream fsDoc = new FileStream(docPath, FileMode.Open);
                    fsDoc.Position = 0;
                    byte[] docDigest = EncryptUtils.SHA256EncryptToByte(fsDoc);
                    sourceBytesTemp = new byte[sourceBytes.Length];
                    Array.Copy(sourceBytes, 0, sourceBytesTemp, 0, sourceBytes.Length);
                    sourceBytes = new byte[sourceBytesTemp.Length + docDigest.Length];

                    Array.Copy(sourceBytesTemp, 0, sourceBytes, 0, sourceBytesTemp.Length);
                    Array.Copy(docDigest, 0, sourceBytes, sourceBytesTemp.Length, docDigest.Length);
                }
            }
            return sourceBytes;
        }

        public static string GetHashSignFormTempPath()
        {
            string tempPath = CAConstants.CA_SIGN_PDF_NAS + Path.DirectorySeparatorChar
                + DateTimeUtils.FormatDateTime(DateTime.Now, CAConstants.CA_SIGN_FILE_PATH_FORMAT) + Path.DirectorySeparatorChar
                + "signing" + Path.DirectorySeparatorChar + "form" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        /// <summary>
        /// 获取证书
        /// </summary>
        /// <param name="certBytes"></param>
        /// <returns></returns>
        public static Org.BouncyCastle.X509.X509Certificate GetCertificate(byte[] certBytes)
        {
            X509Certificate x509Certificate = new X509Certificate(certBytes);
            var bcX509Cert = Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(x509Certificate);
            return bcX509Cert;
        }

        /// <summary>
        /// HTML转换成PDF
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pdfPath"></param>
        public static void HtmlToPdf(string html, string pdfPath)
        {
            html = CommonHelper.UrlDecode(html);
            LogUtils.Debug("Html to PDF before format content:" + html);
            //避免當htmlText無任何html tag標籤的純文字時，轉PDF時會掛掉，所以一律加上<p>標籤
            //html = "<p>" + html + "</p>";
            //在转换成PDF之前格式化HTML
            html = FormatHTML(html);
            LogUtils.Debug("Html to PDF after format content:" + html);

            MemoryStream outputStream = new MemoryStream();//要把PDF寫到哪個串流
            byte[] data = Encoding.UTF8.GetBytes(html);//字串轉成byte[]
            MemoryStream msInput = new MemoryStream(data);
            Document doc = new Document();//要寫PDF的文件，建構子沒填的話預設直式A4

            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件預設開檔時的縮放為100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //開啟Document文件 
            doc.Open();
            //使用XMLWorkerHelper把Html parse到PDF檔裡
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            //将pdfDest设定的资料写到PDF档
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);

            doc.Close();
            msInput.Close();
            outputStream.Close();

            //保存PDF檔案 
            //如果存在pdf文件就删除
            if (File.Exists(pdfPath))
            {
                File.Delete(pdfPath);
            }

            //定义pdf的byte数组
            var outputByte = outputStream.ToArray();

            FileStream fs = new FileStream(pdfPath, FileMode.Create);
            fs.Write(outputByte, 0, outputByte.Length);
            fs.Dispose();
        }

        /// <summary>
        /// XML转换成PDF
        /// </summary>
        /// <param name="xmlDataStr"></param>
        /// <param name="xmlPath"></param>
        /// <param name="pdfPath"></param>
        public static void XMLToPdf(string xmlDataStr, string xmlPath, string pdfPath)
        {
            //Url解码 xml数据
            string xmlStr = CommonHelper.UrlDecode(xmlDataStr);
            LogUtils.Debug(string.Format("xmlToPdf pdfPath:{0} xmlPath:{1} xml:{2}", pdfPath, xmlPath, xmlStr));

            //判断PDF文件是否存在，如果存在，则删除
            if (File.Exists(pdfPath))
            {
                File.Delete(pdfPath);
            }
            //声明文件流
            FileStream fs = new FileStream(pdfPath, FileMode.Create);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            //原始XML存储
            xmlDoc.Save(xmlPath);

            XmlNode rootNode = xmlDoc.DocumentElement;

            Document document = new Document();//要寫PDF的文件，建構子沒填的話預設直式A4
            PdfWriter writer = PdfWriter.GetInstance(document, fs);  //将PDF文档写入创建的文件中
            //指定文件預設開檔時的縮放為100%
            //PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, document.PageSize.Height, 1f);
            //開啟Document文件 
            document.Open();

            //设置字体
            Font font = new UnicodeFontFactory().GetFont();
            //添加标题
            document.Add(new Paragraph(150, "Form Data To PDF document."));
            //添加副标题
            Paragraph paragraph = new Paragraph("Form Data", font);
            paragraph.SpacingAfter = 10;
            document.Add(paragraph);


            PdfPTable tableFather = new PdfPTable(1);
            ////tableFather.SetWidthPercentage(new float[] { 100 }, new Rectangle(0, 0));

            PdfPTable tableChild = new PdfPTable(2);
            tableChild.AddCell(GetCell("Label", 1, 1, font));
            tableChild.AddCell(GetCell("Value", 1, 1, font));

            XmlNodeList nodeList = rootNode.ChildNodes;

            for (int i = 0; i < nodeList.Count; i++)
            {
                tableChild.AddCell(GetCell(nodeList[i].Name, 1, 1, font));
                tableChild.AddCell(GetCell(nodeList[i].InnerText, 1, 1, font));
            }

            PdfPCell tableItem = new PdfPCell(tableChild);
            tableItem.BorderWidth = 2f;
            tableItem.BorderColor = new BaseColor(227, 230, 232);
            tableItem.Padding = 8f;
            tableFather.AddCell(tableItem);
            document.Add(tableFather);
            document.Close();
            fs.Close();
            writer.Close();
        }


        public static byte[] GetHashSignSourceData(string xmlData, string businessID, string docID, RedisHelper redisHelper)
        {
            try
            {
                string xmlDataStr = CommonHelper.DecodeBase64(xmlData, "utf-8");
                string xmlStr = CommonHelper.UrlDecode(xmlDataStr);
                string xmlPath = GetHashSignXMLPath() + businessID + ".xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNode node = xmlDoc.DocumentElement;
                var rootElement = node.FirstChild;

                if (!string.IsNullOrEmpty(docID))
                {
                    string[] docs = docID.Split(',');
                    for (int i = 0; i < docs.Length; i++)
                    {
                        string docNameKey = CacheKeyUtils.GetSignFileNameKey(docs[i]);
                        //string docName = redisHelper.Get<string>(docNameKey);
                        iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                        string docName = udtIAMSmartBLL.GetCacheValueByKey(redisHelper.Get<string>(docNameKey));

                        var tempElement = xmlDoc.CreateElement("file" + (i + 1));
                        tempElement.InnerText = docName;
                        rootElement.AppendChild(tempElement);
                        redisHelper.Remove(docNameKey);
                    }
                }
                xmlDoc.Save(xmlPath);

                //将保存的xml文件创建为 SHA256 hash
                FileStream fsXml = new FileStream(xmlPath, FileMode.Open);
                fsXml.Position = 0;
                byte[] xmlDigest = EncryptUtils.SHA256EncryptToByte(fsXml);
                byte[] sourceBytes = new byte[xmlDigest.Length];
                Array.Copy(xmlDigest, 0, sourceBytes, 0, xmlDigest.Length);
                fsXml.Close();

                if (!string.IsNullOrEmpty(docID))
                {
                    string[] docs = docID.Split(',');
                    byte[] sourceBytesTemp = null;
                    foreach (var doc in docs)
                    {
                        //string docPath = redisHelper.Get<string>(CacheKeyUtils.GetSignFilePathKey(doc));
                        iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                        string docPath = udtIAMSmartBLL.GetCacheValueByKey(CacheKeyUtils.GetSignFilePathKey(doc));

                        FileStream inStream = new FileStream(docPath, FileMode.Open);
                        byte[] docDigest = EncryptUtils.SHA256EncryptToByte(inStream);
                        sourceBytesTemp = new byte[sourceBytes.Length];
                        Array.Copy(sourceBytes, 0, sourceBytesTemp, 0, sourceBytes.Length);
                        sourceBytes = new byte[sourceBytesTemp.Length + docDigest.Length];
                        Array.Copy(sourceBytesTemp, 0, sourceBytes, 0, sourceBytesTemp.Length);
                        Array.Copy(docDigest, 0, sourceBytes, sourceBytesTemp.Length, docDigest.Length);
                        inStream.Close();
                    }
                    sourceBytes = EncryptUtils.SHA256EncryptToByte(sourceBytes);
                }
                return sourceBytes;
            }
            catch (Exception ex)
            {
                LogUtils.Error("GetHashSignSourceData error:" + ex.Message);
                return null;
            }
        }

        public static bool CheckHKIC(string hkic)
        {
            var isMatch = Regex.IsMatch(hkic, HKICPattern);
            return isMatch;
        }

        private static string FormatHTML(string html)
        {
            //所有的单引号替换为双引号
            html = html.Replace('\'', '\"');

            //input框替换为label
            Regex regValue = new Regex("value=\".*\"");

            Match matchValue = null;

            Regex regInput1 = new Regex("<input[^<>]+/>");
            MatchCollection matchCollection1 = regInput1.Matches(html);
            if (matchCollection1.Count > 0)
            {
                foreach (Match input in matchCollection1)
                {
                    matchValue = regValue.Match(input.Value);
                    var value = matchValue.Value.Split('=')[1].Trim('\"');
                    html = html.Replace(input.Value, string.Format(@"<label>{0}</label>", value));
                }
            }
            return html;
        }

        private static PdfPCell GetCell(string cellValue, int colspan, int rowSpan, Font font)
        {
            PdfPCell cell = new PdfPCell();
            try
            {
                cell = new PdfPCell(new Phrase(cellValue, font));
                cell.Border = Rectangle.NO_BORDER;
                cell.PaddingTop = 2f;
                cell.Rowspan = rowSpan;
                cell.Colspan = colspan;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            catch (Exception ex)
            {
                LogUtils.Error("GetCell error:" + ex.Message);
            }
            return cell;
        }
    }
}
