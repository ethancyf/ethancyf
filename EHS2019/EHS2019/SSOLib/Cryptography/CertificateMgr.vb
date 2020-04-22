' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Manager
' Detail            : Handle Certificate
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Security.Cryptography.X509Certificates

Namespace Cryptography
    Public Class CertificateMgr

        Public Shared Function findCertificate(ByVal location As StoreLocation, ByVal name As StoreName, ByVal findType As X509FindType, ByVal findValue As String) As X509Certificate2


            Dim store As New X509Store(name, location)
            Try

                ' create and open store for read-only access
                store.Open(OpenFlags.[ReadOnly])

                ' search store
                Dim col As X509Certificate2Collection = store.Certificates.Find(findType, findValue, False)

                If col.Count = 0 Then

                    Return Nothing
                End If
                ' return first certificate found

                Return col(0)
            Catch ex As Exception
                Throw New Exception("Error at CertificateMgr.findCertificate(). Find value='" + findValue + "'", ex)
            Finally
                ' always close the store
                If store IsNot Nothing Then
                    store.Close()

                End If
            End Try

        End Function


        Public Shared Function VerifyCert(ByVal objCertificate As X509Certificate2) As Boolean
            Dim blnChkRst As Boolean = False
            'get checking vonfiguration
            Dim strSSOCertChkTimeValidaity As String = SSOUtil.SSOAppConfigMgr.getSSOCertChkTimeValidaity()
            Dim strSSOCertChkTrustChain As String = SSOUtil.SSOAppConfigMgr.getSSOCertChkTrustChain()
            Dim strSSOCertChkCRL As String = SSOUtil.SSOAppConfigMgr.getSSOCertChkCRL()

            If strSSOCertChkTimeValidaity.Trim().ToUpper() = "N" AndAlso strSSOCertChkTrustChain.Trim().ToUpper() = "N" AndAlso strSSOCertChkCRL.Trim().ToUpper() = "N" Then
                'no checking is required
                Return True
            End If

            Dim chain As New X509Chain()

            If strSSOCertChkCRL = "Y" Then

                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EndCertificateOnly
                chain.ChainPolicy.UrlRetrievalTimeout = New TimeSpan(0, 2, 0)
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online
            Else

                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck
            End If

            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag

            If strSSOCertChkTimeValidaity = "Y" Then
            Else
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreNotTimeValid
            End If


            chain.Build(objCertificate)

            If chain.ChainStatus.Length = 0 Then
                'no exception

                Return True
            End If

            Dim sbChainStatus As New StringBuilder()

            blnChkRst = True
            For i As Integer = 0 To chain.ChainStatus.Length - 1
                Dim strChainStatus As String = chain.ChainStatus(i).Status.ToString().Trim().ToUpper()
                If strSSOCertChkTrustChain.Trim().ToUpper() = "N" AndAlso (strChainStatus = "PARTIALCHAIN" OrElse strChainStatus = "UNTRUSTEDROOT") Then
                    sbChainStatus.AppendLine(vbTab & vbTab & " Skipped: " + chain.ChainStatus(i).Status.ToString() + "-" + chain.ChainStatus(i).StatusInformation)
                Else
                    sbChainStatus.AppendLine(vbTab & vbTab & " Non skipped: " + chain.ChainStatus(i).Status.ToString() + "-" + chain.ChainStatus(i).StatusInformation)
                    blnChkRst = False


                End If
            Next

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Certificate verification status for " + objCertificate.Thumbprint + ":" & vbCr & vbLf & " " + sbChainStatus.ToString()))

            Return blnChkRst

        End Function
    End Class
End Namespace
