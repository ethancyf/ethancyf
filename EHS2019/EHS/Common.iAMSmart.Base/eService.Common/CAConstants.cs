using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class CAConstants
    {
        public static string CA_SIGN_FILE_PATH_FORMAT = "yyyyMMdd";

        public static string CA_SIGN_DEPARTMENT = Constants.SOURCE_RESOURCE["ca.sign.department"];
        public static string CA_SIGN_SERVICE_NAME = Constants.SOURCE_RESOURCE["ca.sign.serviceName"];
        public static string CA_SIGN_DOCUMENT_NAME = Constants.SOURCE_RESOURCE["ca.sign.documentName"];
        public static string CA_SIGN_PDF_NAS = Constants.SOURCE_RESOURCE["ca.sign.pdf.nas"];
        public static string[] CA_SIGN_FILE_EXTS = Constants.SOURCE_RESOURCE["ca.sign.file.exts"].Split(',');
        public static string CA_SIGN_ACK_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["ca.sign.ack.url"];

        public static string CA_FROM_TO_PDF_HASH_ALGORITHM = Constants.SOURCE_RESOURCE["ca.from.to.pdf.hash.algorithm"];
        public static string CA_UPLOAD_FILE_HASH_ALGORITHM = Constants.SOURCE_RESOURCE["ca.upload.file.hash.algorithm"];
        public static bool CA_PDF_SIGNATURE_LONG_TERM_VALIDATION = Convert.ToBoolean(Constants.SOURCE_RESOURCE["ca.pdf.signature.long.term.validation"]);

        public static string CA_HASH_SIGN_SIGNING_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["ca.hash.sign.signing.url"];
        public static string CA_HASH_SIGN_REDIRECT_URL = Constants.ESERVICE_URL + Constants.SOURCE_RESOURCE["ca.hash.sign.redirect.url"];
        public static string CA_HASH_SIGN_ENCRYPTION_ALGORITHM = Constants.SOURCE_RESOURCE["ca.hash.sign.encryption.algorithm"];
        public static string CA_HASH_SIGN_HASH_ALGORITHM = Constants.SOURCE_RESOURCE["ca.hash.sign.hash.algorithm"];

        public static string CA_PDF_SIGN_SIGNING_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["ca.pdf.sign.signing.url"];
        public static string CA_PDF_SIGN_REDIRECT_URL = Constants.ESERVICE_URL + Constants.SOURCE_RESOURCE["ca.pdf.sign.redirect.url"];
        public static string CA_PDF_SIGN_ENCRYPTION_ALGORITHM = Constants.SOURCE_RESOURCE["ca.pdf.sign.encryption.algorithm"];
        public static string CA_PDF_SIGN_HASH_ALGORITHM = Constants.SOURCE_RESOURCE["ca.pdf.sign.hash.algorithm"];

        public static string CA_ANONYMOUS_HASH_SIGN_SIGNING_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["ca.anonymous.hash.sign.signing.url"];
        public static string CA_ANONYMOUS_HASH_SIGN_RESULT_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["ca.anonymous.hash.sign.result.url"];
        public static string CA_ANONYMOUS_HASH_SIGN_ENCRYPTION_ALGORITHM = Constants.SOURCE_RESOURCE["ca.anonymous.hash.sign.encryption.algorithm"];
        public static string CA_ANONYMOUS_HASH_SIGN_HASH_ALGORITHM = Constants.SOURCE_RESOURCE["ca.anonymous.hash.sign.hash.algorithm"];

        public static string CA_ANONYMOUS_PDF_SIGN_SIGNING_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["ca.anonymous.pdf.sign.signing.url"];
        public static string CA_ANONYMOUS_PDF_SIGN_RESULT_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["ca.anonymous.pdf.sign.result.url"];
        public static string CA_ANONYMOUS_PDF_SIGN_ENCRYPTION_ALGORITHM =Constants.SOURCE_RESOURCE["ca.anonymous.pdf.sign.encryption.algorithm"];
        public static string CA_ANONYMOUS_PDF_SIGN_HASH_ALGORITHM = Constants.SOURCE_RESOURCE["ca.anonymous.pdf.sign.hash.algorithm"];


    }
}
