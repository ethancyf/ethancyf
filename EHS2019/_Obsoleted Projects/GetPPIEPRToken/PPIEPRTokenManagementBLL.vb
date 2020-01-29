Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.Component.ServiceProvider
Imports Common.Format
Imports Common.Component
Imports Common.Component.RSA_Manager
Imports GetPPIEPRToken.ProxyClass
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security

Namespace PPIEPRTokenManagement

    Public Class PPIEPRTokenManagementBLL

        Private udtDB As Database = New Database()

        Public Function getPPIePRSerialNo(ByVal strHKID As String) As String
            Dim strRes As String = String.Empty
            Dim strtemp As String = String.Empty
            Dim udtComfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
            Dim strURL As String = String.Empty
            Dim strBackupURL As String = String.Empty
            Dim strPasscode As String = String.Empty
            Dim strvalue2 As String = String.Empty
            Dim blnSysParameter As Boolean = False

            Dim bResult As Boolean = False
            'Result format returned from PPI-ePR web service
            '<TokenInfo>
            '    <TokenSN>XXX</TokenSN>
            '    <UserID>XXX</UserID>
            '    <ProjectCode>XXX</ProjectCode>
            '</TokenInfo>
            Try
                'strHKID = "K7897899"

                blnSysParameter = udtComfunct.getSystemParameter("PPIePRWSLink", strURL, strBackupURL)
                'blnSysParameter = udtComfunct.getSystemParameter("PPIePRWSPasscode", strPasscode, strvalue2)
                blnSysParameter = udtComfunct.getSystemParameterPassword("PPIePRWSPasscode", strPasscode)
                Dim wsPPI As New ProxyClass.PPI_EVS_WS

                'strURL = "http://hoppisv01:8012/PPI/PPI_Webservices/HAInternal/PPI_EVS_WS.asmx"

                'strBackupURL = strURL

                ' Try Primary Connection
                Try
                    Dim resp As HttpWebResponse = Nothing
                    wsPPI.Url = strURL.Trim()
                    Dim req As HttpWebRequest = CType(WebRequest.Create(wsPPI.Url), HttpWebRequest)
                    req.Credentials = CredentialCache.DefaultCredentials
                    Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                    strtemp = wsPPI.getPPIeHSRSATokenSerialNoByHKID(strHKID, strPasscode)
                    'strtemp = wsPPI.getPPIRSATokenSerialNoByHKID(strHKID, strPasscode)

                    If strtemp.Equals(String.Empty) Then
                        strRes = String.Empty
                        bResult = True
                    Else
                        strtemp = strtemp.Substring(strtemp.IndexOf("<TokenSN>"))
                        strtemp = strtemp.Substring(0, strtemp.IndexOf("</TokenSN>"))
                        strtemp = strtemp.Replace("<TokenSN>", String.Empty)
                        strRes = strtemp.Trim()
                        bResult = True
                    End If

                Catch ex As Exception

                End Try


                If Not bResult Then

                    Try
                        Dim resp As HttpWebResponse = Nothing
                        wsPPI.Url = strBackupURL.Trim()
                        Dim req As HttpWebRequest = CType(WebRequest.Create(wsPPI.Url), HttpWebRequest)
                        req.Credentials = CredentialCache.DefaultCredentials
                        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                        strtemp = wsPPI.getPPIeHSRSATokenSerialNoByHKID(strHKID, strPasscode)
                        'strtemp = wsPPI.getPPIRSATokenSerialNoByHKID(strHKID, strPasscode)

                        If strtemp.Equals(String.Empty) Then
                            strRes = String.Empty
                            bResult = True
                        Else
                            strtemp = strtemp.Substring(strtemp.IndexOf("<TokenSN>"))
                            strtemp = strtemp.Substring(0, strtemp.IndexOf("</TokenSN>"))
                            strtemp = strtemp.Replace("<TokenSN>", String.Empty)
                            strRes = strtemp.Trim()
                            bResult = True
                        End If

                    Catch ex As Exception
                        Throw ex
                    End Try

                End If

            Catch ex As Exception
                Throw ex

            End Try

            Return strRes

        End Function

        Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            'Return True to force the certificate to be accepted.
            Return True
        End Function


    End Class
End Namespace
