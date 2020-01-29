Imports SSODAL
Imports SSODataType
Imports System.Globalization


Public Class SSOAuthenMgr

    'Public redirectErrorCode As [Enum]

    'Public Class RedirectErrCode
    '    Public Const Success = ""
    '    Public Const VerifyAuthenTicketFail = "VERIFY_AUTH_TICKET_FAIL"
    '    Public Const AuthenTicketNotFound = "AUTH_TICKET_NOT_FOUND"
    '    Public Const OtherErr = "OTHER_ERR"
    '    Public Const AuthenTicketExpired = "AUTH_TICKET_EXPIRED"
    'End Class

    Public Const intAuthenTicketLength As Integer = 100
    Public Const intRedirectTicketLength As Integer = 100

#Region "Constructor"

    Private Shared _SSOAuthenMgr As SSOAuthenMgr

    Private Sub New()
    End Sub

    Public Shared Function getInstance() As SSOAuthenMgr
        If _SSOAuthenMgr Is Nothing Then
            _SSOAuthenMgr = New SSOAuthenMgr()
        End If

        Return _SSOAuthenMgr
    End Function

#End Region

    ''' <summary>
    ''' Generate SSO Authen when perform Login
    ''' </summary>
    ''' <param name="udtSystemDtm"></param>
    ''' <param name="strSourceAppID"></param>
    ''' <param name="strUserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function generateSSOAuthentication_Login(ByVal udtSystemDtm As DateTime, ByVal strSourceAppID As String, ByVal strUserID As String) As SSODataType.SSOAuthen

        ' ------------------------------------------------------
        ' SSOAuthen
        ' ------------------------------------------------------
        Dim objSSOAuthen As New SSODataType.SSOAuthen()
        objSSOAuthen.SystemDtm = udtSystemDtm
        objSSOAuthen.AuthenDtm = udtSystemDtm

        ' Random Authen Ticket
        Dim strSSOAuthenTicketLength As String = ""
        Dim intSSOAuthenTicketLength As Integer = 0
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        udcGeneralF.getSystemParameter("SSOAuthenTicketLength", strSSOAuthenTicketLength, String.Empty)
        If Int32.TryParse(strSSOAuthenTicketLength, intSSOAuthenTicketLength) Then
            objSSOAuthen.AuthenTicket = Cryptography.RandomNumberGenerator.getUniqueKey(intSSOAuthenTicketLength)
        Else
            objSSOAuthen.AuthenTicket = Cryptography.RandomNumberGenerator.getUniqueKey(intAuthenTicketLength)
        End If


        ' Cert for Digital signature
        Dim strSSOCertificateThumbprintToPerformDigitalSignature As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintToPerformDigitalSignature()
        Dim strXMLAuthenTicket As String = SSOUtil.SSOHelper.getAuthenTicketXML(objSSOAuthen.AuthenTicket, objSSOAuthen.SystemDtm)
        objSSOAuthen.SignedAuthenTicket = Cryptography.XMLDigitialSignatureMgr.signXML(strSSOCertificateThumbprintToPerformDigitalSignature, strXMLAuthenTicket, SSOUtil.SSOHelper.SignAuthenTicketElementId, SSOUtil.SSOHelper.AuthenTagName)

        objSSOAuthen.SourceAppID = strSourceAppID

        ' ------------------------------------------------------
        ' SSOLoginUser
        ' ------------------------------------------------------
        Dim objSSOLoginUser As New SSODataType.SSOLoginUser(objSSOAuthen.SystemDtm, objSSOAuthen.AuthenTicket, strUserID)

        ' ------------------------------------------------------
        ' Save SSOAuthen & SSOLoginUser
        ' ------------------------------------------------------
        SSODAL.SSOAuthenticationDAL.getInstance().insertSSOAuthentication(objSSOAuthen, objSSOLoginUser)

        Return objSSOAuthen
    End Function

    ''' <summary>
    ''' Generate SSO Authen when perform SSO Authen
    ''' </summary>
    ''' <param name="udtSystemDtm"></param>
    ''' <param name="strSourceAppID"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strRelySecuredAuthenTicket"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function generateSSOAuthentication_SSOAuthen(ByVal udtSystemDtm As DateTime, ByVal strSourceAppID As String, ByVal strUserID As String, ByVal strRelySecuredAuthenTicket As String) As SSODataType.SSOAuthen

        ' ------------------------------------------------------
        ' SSOAuthen
        ' ------------------------------------------------------
        Dim objSSOAuthen As New SSODataType.SSOAuthen()
        objSSOAuthen.SystemDtm = udtSystemDtm
        objSSOAuthen.AuthenDtm = udtSystemDtm

        ' Random Authen Ticket
        Dim strSSOAuthenTicketLength As String = ""
        Dim intSSOAuthenTicketLength As Integer = 0
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        udcGeneralF.getSystemParameter("SSOAuthenTicketLength", strSSOAuthenTicketLength, String.Empty)
        If Int32.TryParse(strSSOAuthenTicketLength, intSSOAuthenTicketLength) Then
            objSSOAuthen.AuthenTicket = Cryptography.RandomNumberGenerator.getUniqueKey(intSSOAuthenTicketLength)
        Else
            objSSOAuthen.AuthenTicket = Cryptography.RandomNumberGenerator.getUniqueKey(intAuthenTicketLength)
        End If

        ' Cert for Digital signature
        Dim strSSOCertificateThumbprintToPerformDigitalSignature As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintToPerformDigitalSignature()
        Dim strXMLAuthenTicket As String = SSOUtil.SSOHelper.getAuthenTicketXML(objSSOAuthen.AuthenTicket, objSSOAuthen.SystemDtm)
        objSSOAuthen.SignedAuthenTicket = Cryptography.XMLDigitialSignatureMgr.signXML(strSSOCertificateThumbprintToPerformDigitalSignature, strXMLAuthenTicket, SSOUtil.SSOHelper.SignAuthenTicketElementId, SSOUtil.SSOHelper.AuthenTagName)

        objSSOAuthen.SourceAppID = strSourceAppID

        ' ------------------------------------------------------
        ' SSOLoginUser
        ' ------------------------------------------------------
        Dim objSSOLoginUser As New SSODataType.SSOLoginUser(objSSOAuthen.SystemDtm, objSSOAuthen.AuthenTicket, strUserID)
        ' ------------------------------------------------------
        ' SSOAuthen App (e.g. PPI-ePR Authen info)
        ' ------------------------------------------------------
        Dim objSSOAuthenApp As New SSODataType.SSOAuthenApp(objSSOAuthen.SystemDtm, objSSOAuthen.AuthenTicket, strSourceAppID, strRelySecuredAuthenTicket)


        SSODAL.SSOAuthenticationDAL.getInstance().insertSSOAuthentication(objSSOAuthen, objSSOAuthenApp, objSSOLoginUser)

        Return objSSOAuthen

    End Function


    Public Function verifySSOAuthenTicket(ByVal strSecuredAuthenTicket As String, ByRef objSSOAuthen As SSODataType.SSOAuthen) As String

        Dim strSSOCertificateThumbprintToPerformDigitalSignature As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintToPerformDigitalSignature()
        Dim blnVerify As Boolean = Cryptography.XMLDigitialSignatureMgr.verifySignedXML(strSSOCertificateThumbprintToPerformDigitalSignature, strSecuredAuthenTicket, SSOUtil.SSOHelper.SignAuthenTicketElementId, True)

        If blnVerify Then
            'retrieve SSOAuthen
            Dim strAuthenTicket As String = String.Empty
            Dim dtmSystemDtm As DateTime
            Dim intSSOAuthenTimeoutInMin As Integer
            Dim blnExtract As Boolean = SSOUtil.SSOHelper.extractAuthentTicket(strSecuredAuthenTicket, strAuthenTicket, dtmSystemDtm)

            If blnExtract Then

                'Retreieve SSO_Authen from DB
                objSSOAuthen = SSOAuthenticationDAL.getInstance().getSSOAuthen(dtmSystemDtm, strAuthenTicket)

                If IsNothing(objSSOAuthen) Then
                    Return RedirectErrCode.AuthenTicketNotFound
                Else

                    'Check whether the auth ticket is expired
                    Dim strSSOAuthenTimeoutInMin As String = SSOUtil.SSOAppConfigMgr.getSSOAuthenTicketTimeoutInMin()
                    If strSSOAuthenTimeoutInMin Is Nothing OrElse strSSOAuthenTimeoutInMin.Trim() = "" Then
                        Return RedirectErrCode.OtherErr + ": SSOAuthenTimeoutInMin not found"
                    End If

                    If Int32.TryParse(strSSOAuthenTimeoutInMin, intSSOAuthenTimeoutInMin) = False Then
                        Return RedirectErrCode.OtherErr + ": invalid SSOAuthenTimeoutInMin format"
                    Else
                        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
                        Dim dtmAuthenTicketCutOffTime As DateTime = objSSOAuthen.AuthenDtm.AddMinutes(intSSOAuthenTimeoutInMin)
                        Dim dtmCurrentTime As DateTime = udcGeneralF.GetSystemDateTime()

                        If dtmCurrentTime > dtmAuthenTicketCutOffTime Then
                            Return RedirectErrCode.AuthenTicketExpired
                        Else
                            Return RedirectErrCode.Success
                        End If
                    End If

                End If
            Else
                Return RedirectErrCode.VerifyAuthenTicketFail
            End If
        Else
            Return RedirectErrCode.VerifyAuthenTicketFail
        End If

        Return RedirectErrCode.Success
    End Function

    Public Function generateSSORedirectTicket_SSORedirect(ByVal udtSystemDtm As DateTime, ByVal strAuthenTicket As String) As SSORedirect

        Dim objSSORedirect As New SSORedirect
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        objSSORedirect.SystemDtm = udtSystemDtm

        Dim strSSORedirectTicketLength As String = ""
        Dim intSSORedirectTicketLength As Integer = 0
        udcGeneralF.getSystemParameter("SSORedirectTicketLength", strSSORedirectTicketLength, String.Empty)
        If Int32.TryParse(strSSORedirectTicketLength, intSSORedirectTicketLength) Then
            objSSORedirect.RedirectTicket = Cryptography.RandomNumberGenerator.getUniqueKey(intSSORedirectTicketLength)
        Else
            objSSORedirect.RedirectTicket = Cryptography.RandomNumberGenerator.getUniqueKey(intRedirectTicketLength)
        End If

        objSSORedirect.ReadCount = 0
        objSSORedirect.RedirectStartDtm = udcGeneralF.GetSystemDateTime()

        objSSORedirect.AuthenTicket = strAuthenTicket

        Return objSSORedirect

    End Function

    Public Function checkSSORedirectValidity(ByVal strRedirectTicket As String, ByRef objSSORedirect As SSORedirect) As String

        objSSORedirect = SSOAuthenticationDAL.getInstance().getSSORedirectByRedirectTicket(strRedirectTicket)

        'Update Record Count
        If objSSORedirect Is Nothing Then
            Return RedirectErrCode.RedirectTicketNotFound
        Else
            'Check whether the Redirect ticket is expired
            Dim intSSORedirectTimeoutInSec As Integer
            Dim strSSORedirectTimeoutInSec As String = SSOUtil.SSOAppConfigMgr.getSSORedirectTicketTimeoutInSec()
            If strSSORedirectTimeoutInSec Is Nothing OrElse strSSORedirectTimeoutInSec.Trim() = "" Then
                Return RedirectErrCode.OtherErr
            End If

            If Int32.TryParse(strSSORedirectTimeoutInSec, intSSORedirectTimeoutInSec) = False Then
                Return RedirectErrCode.OtherErr
            Else
                Dim udcGeneralF As New Common.ComFunction.GeneralFunction
                Dim dtmRedirectTicketCutOffTime As DateTime = objSSORedirect.RedirectStartDtm.AddSeconds(intSSORedirectTimeoutInSec)
                Dim dtmCurrentTime As DateTime = udcGeneralF.GetSystemDateTime()

                If dtmCurrentTime > dtmRedirectTicketCutOffTime Then
                    Return RedirectErrCode.RedirectTicketExpired
                End If
            End If
        End If

        'Check Read Count
        If objSSORedirect.ReadCount = 0 Then

            'Increase Read count by 1
            SSOAuthenticationDAL.getInstance().increaseSSORedirectReadCount(objSSORedirect)

            Return RedirectErrCode.Success
        Else
            Return RedirectErrCode.RedirectTicketExceedLimt
        End If

    End Function

    Public Function CheckSPIDwithSSOLoginRecord(ByVal strSPID As String, ByVal dtSystemDtm As DateTime, ByVal strAuthenTicket As String) As String

        Dim objSSOLoginUser As SSOLoginUser = SSOAuthenticationDAL.getInstance().getSSOLoginUser(dtSystemDtm, strAuthenTicket)

        If IsNothing(objSSOLoginUser) Then
            Return LoginErrCode.SSOLoginUserNotFound
        Else
            If objSSOLoginUser.UserID.Trim = strSPID.Trim Then
                Return LoginErrCode.Success
            Else
                Return LoginErrCode.SSOLoginUserNotFound
            End If
        End If

    End Function

End Class
