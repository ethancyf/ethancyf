Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DataEntryUser
Imports Common.Component.EHSAccount
Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports System.Data.SqlClient
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Namespace BLL

    Public Class IdeasBLL

        Public Shared DeptCode As String = ConfigurationManager.AppSettings("SmartID_DeptCode").ToString()
        Public Shared RaCode As String = ConfigurationManager.AppSettings("SmartID_RaCode").ToString()
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Shared RaCode_IDEAS2 As String = ConfigurationManager.AppSettings("SmartID_RaCode_IDEAS2").ToString()
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Enum EnumIdeasVersion
            One
            Two
            TwoGender
            Combo
            ComboGender
        End Enum
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

#Region "Shared Function"

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Shared Function GetCardFaceDataEHSAccount(ByVal cfData As IdeasRM.CardFaceData, ByVal udtSchemeClaim As Scheme.SchemeClaimModel, ByVal strFunctionCode As String, ByVal udtSmartIDContent As BLL.SmartIDContentModel) As EHSAccountModel

            Return GetCardFaceDataEHSAccount(cfData, udtSchemeClaim.SchemeCode, strFunctionCode, udtSmartIDContent)

        End Function
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Add IdeasVersion
        Shared Function GetCardFaceDataEHSAccount(ByVal cfData As IdeasRM.CardFaceData, ByVal strSchemeClaim As String, ByVal strFunctionCode As String, ByVal udtSmartIDContent As BLL.SmartIDContentModel) As EHSAccountModel
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
            Dim isValid As Boolean = True

            Dim udtFormatter As Formatter = New Formatter
            Dim udtValidator As Validator = New Validator
            Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
            Dim udtVAMaintBLL As VoucherAccountMaintenanceBLL = New VoucherAccountMaintenanceBLL()

            ' English Name: Card Face Data Only Contain 1 EName Field, and put in UnstructureName
            Dim strSurName As String = String.Empty
            Dim strFirstName As String = String.Empty
            Dim strExactDOB As String = String.Empty
            Dim strHKID As String = String.Empty
            Dim strCCCode1 As String = String.Empty
            Dim strCCCode2 As String = String.Empty
            Dim strCCCode3 As String = String.Empty
            Dim strCCCode4 As String = String.Empty
            Dim strCCCode5 As String = String.Empty
            Dim strCCCode6 As String = String.Empty
            Dim strCName As String = String.Empty
            Dim dtDOB As Date

            Dim eIdeasVersion As BLL.IdeasBLL.EnumIdeasVersion = udtSmartIDContent.IdeasVersion

            udtFormatter.seperateEName(cfData.PersonalEnglishNameDetails.UnstructuredName.Value.ToUpper, strSurName, strFirstName)

            If Not udtValidator.chkEngName(strSurName.Trim(), strFirstName.Trim(), DocType.DocTypeModel.DocTypeCode.HKIC) Is Nothing Then
                isValid = False
            End If

            'Check HKID
            If isValid Then

                strHKID = cfData.HKIDNumberDetails.Identifier.Value + cfData.HKIDNumberDetails.CheckDigit.Value
                If Not udtValidator.chkHKID(strHKID) Is Nothing Then
                    isValid = False
                End If
            End If

            'Check DOB
            If isValid Then
                Dim strConvertDOB As String = String.Empty

                If cfData.DateOfBirth.EndsWith("-00-00") Then
                    strConvertDOB = cfData.DateOfBirth.Substring(0, 4)
                ElseIf cfData.DateOfBirth.EndsWith("-00") Then
                    strConvertDOB = cfData.DateOfBirth.Substring(5, 2) + "-" + cfData.DateOfBirth.Substring(0, 4)
                Else
                    strConvertDOB = cfData.DateOfBirth.Substring(8, 2) + "-" + cfData.DateOfBirth.Substring(5, 2) + "-" + cfData.DateOfBirth.Substring(0, 4)
                End If

                isValid = udtGeneralFunction.chkDOBtype(strConvertDOB, dtDOB, strExactDOB)
            End If

            'Check CCCode
            If isValid Then
                If Not cfData.PersonalChineseNameHKSCSEncodeDetails Is Nothing AndAlso Not cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode Is Nothing Then

                    If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 0 Then
                        strCCCode1 = cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(0).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(0).ExtensionNumber.Value
                        'strCName += udtVAMaintBLL.getCCCTail(strCCCode1.Substring(0, 4)).Rows(Integer.Parse(strCCCode1.Substring(4, 1)) - 1)("Big5").ToString()
                        strCName += udtVAMaintBLL.getCCCodeBig5(strCCCode1)
                    End If
                    If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 1 Then
                        strCCCode2 = cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(1).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(1).ExtensionNumber.Value
                        'strCName += udtVAMaintBLL.getCCCTail(strCCCode2.Substring(0, 4)).Rows(Integer.Parse(strCCCode2.Substring(4, 1)) - 1)("Big5").ToString()
                        strCName += udtVAMaintBLL.getCCCodeBig5(strCCCode2)
                    End If
                    If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 2 Then
                        strCCCode3 = cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(2).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(2).ExtensionNumber.Value
                        'strCName += udtVAMaintBLL.getCCCTail(strCCCode3.Substring(0, 4)).Rows(Integer.Parse(strCCCode3.Substring(4, 1)) - 1)("Big5").ToString()
                        strCName += udtVAMaintBLL.getCCCodeBig5(strCCCode3)
                    End If
                    If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 3 Then
                        strCCCode4 = cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(3).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(3).ExtensionNumber.Value
                        'strCName += udtVAMaintBLL.getCCCTail(strCCCode4.Substring(0, 4)).Rows(Integer.Parse(strCCCode4.Substring(4, 1)) - 1)("Big5").ToString()
                        strCName += udtVAMaintBLL.getCCCodeBig5(strCCCode4)
                    End If
                    If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 4 Then
                        strCCCode5 = cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(4).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(4).ExtensionNumber.Value
                        'strCName += udtVAMaintBLL.getCCCTail(strCCCode5.Substring(0, 4)).Rows(Integer.Parse(strCCCode5.Substring(4, 1)) - 1)("Big5").ToString()
                        strCName += udtVAMaintBLL.getCCCodeBig5(strCCCode5)
                    End If
                    If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 5 Then
                        strCCCode6 = cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(5).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(5).ExtensionNumber.Value
                        'strCName += udtVAMaintBLL.getCCCTail(strCCCode6.Substring(0, 4)).Rows(Integer.Parse(strCCCode6.Substring(4, 1)) - 1)("Big5").ToString()
                        strCName += udtVAMaintBLL.getCCCodeBig5(strCCCode6)
                    End If

                    If Not udtValidator.chkCCCode_UsingDDL(strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6) Is Nothing Then
                        isValid = False
                    End If
                End If
            End If

            'DOI in card face data is datatime format

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Check Gender (If provided)
            If isValid Then
                If cfData.Gender <> String.Empty Then
                    If Not udtValidator.chkGender(cfData.Gender) Is Nothing Then
                        isValid = False
                    End If
                End If
            End If
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            If isValid Then
                Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL
                Dim udtEHSAccount As EHSAccountModel = New EHSAccountModel()
                Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel

                udtFormatter.seperateEName(cfData.PersonalEnglishNameDetails.UnstructuredName.Value.ToUpper(), strSurName, strFirstName)

                udtEHSAccount = udtEHSClaimBLL.ConstructEHSTemporaryVoucherAccount((New Formatter).formatDocumentIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, strHKID), DocType.DocTypeModel.DocTypeCode.HKIC, strExactDOB, dtDOB, strSchemeClaim, String.Empty)
                udtEHSAccount.VoucherAccID = String.Empty

                udtEHSPersonalInfo = udtEHSAccount.EHSPersonalInformationList(0)

                udtEHSPersonalInfo.ENameSurName = strSurName
                udtEHSPersonalInfo.ENameFirstName = strFirstName
                udtEHSPersonalInfo.CCCode1 = strCCCode1
                udtEHSPersonalInfo.CCCode2 = strCCCode2
                udtEHSPersonalInfo.CCCode3 = strCCCode3
                udtEHSPersonalInfo.CCCode4 = strCCCode4
                udtEHSPersonalInfo.CCCode5 = strCCCode5
                udtEHSPersonalInfo.CCCode6 = strCCCode6
                udtEHSPersonalInfo.CName = strCName
                udtEHSPersonalInfo.DateofIssue = cfData.DateOfIssue

                udtEHSPersonalInfo.CreateBySmartID = True

                ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                udtEHSPersonalInfo.Gender = cfData.Gender

                Select Case eIdeasVersion
                    Case BLL.IdeasBLL.EnumIdeasVersion.One
                        udtEHSPersonalInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS1

                    Case BLL.IdeasBLL.EnumIdeasVersion.Two
                        udtEHSPersonalInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS2

                    Case BLL.IdeasBLL.EnumIdeasVersion.TwoGender
                        udtEHSPersonalInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS2_WithGender

                    Case BLL.IdeasBLL.EnumIdeasVersion.Combo
                        If udtSmartIDContent.CardVersion = 1 Then
                            udtEHSPersonalInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS_Combo_Old
                        End If

                        If udtSmartIDContent.CardVersion = 2 Then
                            udtEHSPersonalInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS_Combo_New
                        End If

                    Case BLL.IdeasBLL.EnumIdeasVersion.ComboGender
                        If udtSmartIDContent.CardVersion = 1 Then
                            udtEHSPersonalInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS_Combo_Old
                        End If

                        If udtSmartIDContent.CardVersion = 2 Then
                            udtEHSPersonalInfo.SmartIDVer = Common.Component.SmartIDVersion.IDEAS_Combo_New_WithGender
                        End If

                End Select
                ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

                udtEHSAccount.SetSearchDocCode(DocType.DocTypeModel.DocTypeCode.HKIC)
                Return udtEHSAccount
            Else
                PrintCardFaceData(cfData, strFunctionCode)
                Return Nothing
            End If

        End Function

        Shared Sub PrintCardFaceData(ByVal cfData As IdeasRM.CardFaceData, ByVal strFunctionCode As String)
            Dim udtAuditLogEntry As New AuditLogEntry(strFunctionCode)

            udtAuditLogEntry.AddDescripton("English Name", cfData.PersonalEnglishNameDetails.UnstructuredName.Value)
            udtAuditLogEntry.AddDescripton("HKIC No.", cfData.HKIDNumberDetails.Identifier.Value + cfData.HKIDNumberDetails.CheckDigit.Value)
            udtAuditLogEntry.AddDescripton("DOB", cfData.DateOfBirth)

            If Not cfData.PersonalChineseNameHKSCSEncodeDetails Is Nothing AndAlso Not cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode Is Nothing Then

                If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 0 Then
                    udtAuditLogEntry.AddDescripton("CCCode1", cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(0).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(0).ExtensionNumber.Value)
                End If

                If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 1 Then
                    udtAuditLogEntry.AddDescripton("CCCode2", cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(1).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(1).ExtensionNumber.Value)
                End If

                If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 2 Then
                    udtAuditLogEntry.AddDescripton("CCCode3", cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(2).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(2).ExtensionNumber.Value)
                End If

                If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 3 Then
                    udtAuditLogEntry.AddDescripton("CCCode4", cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(3).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(3).ExtensionNumber.Value)
                End If

                If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 4 Then
                    udtAuditLogEntry.AddDescripton("CCCode5", cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(4).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(4).ExtensionNumber.Value)
                End If

                If cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode.Length > 5 Then
                    udtAuditLogEntry.AddDescripton("CCCode6", cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(5).FourDigitCode.Value + cfData.PersonalChineseNameHKSCSEncodeDetails.ChineseCommercialCode(5).ExtensionNumber.Value)
                End If
            Else
                udtAuditLogEntry.AddDescripton("Chinese Name", "")
            End If

            udtAuditLogEntry.AddDescripton("DOI", cfData.DateOfIssue.ToString)

            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00071, "CFD Details")

        End Sub

#End Region

#Region "Property"

        Public ReadOnly Property Artifact() As String
            Get
                Return Web.HttpContext.Current.Request.QueryString("SAMLart")
            End Get
        End Property

        Shared ReadOnly Property AppRefId() As String
            Get
                If Not HttpContext.Current Is Nothing Then
                    Return HttpContext.Current.Session.SessionID.Trim().Substring(0, 20) + DateTime.Now.ToString("yyyyMMddHHmmss")
                Else
                    Return DateTime.Now.ToString("yyyyMMddHHmmss")
                End If
            End Get
        End Property

#End Region

#Region "Supporting Function"

        ' INT14-0033 - Enforce HCSP accept server cert for connecting IDEAS Testing server [Start][Lawrence]
        Public Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            'Return True to force the certificate to be accepted.
            Return True
        End Function
        ' INT14-0033 - Enforce HCSP accept server cert for connecting IDEAS Testing server [End][Lawrence]

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Shared Function GetIdeasVersion(ByVal enumIDEASVersion As EnumIdeasVersion) As EnumIdeasVersion
            Dim enumResult As EnumIdeasVersion = Nothing

            Dim strReadGender As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("SmartID_IDEAS2_ReadGender", strReadGender, String.Empty)

            Select Case enumIDEASVersion
                Case IdeasBLL.EnumIdeasVersion.One
                    enumResult = enumIDEASVersion.One

                Case IdeasBLL.EnumIdeasVersion.Two
                    If strReadGender = YesNo.Yes Then
                        ' IDEAS2 / 2.5 ( Not read gender / Read Gender)
                        enumResult = enumIDEASVersion.TwoGender
                    Else
                        enumResult = enumIDEASVersion.Two
                    End If

                Case IdeasBLL.EnumIdeasVersion.Combo
                    If strReadGender = YesNo.Yes Then
                        enumResult = enumIDEASVersion.ComboGender
                    Else
                        enumResult = enumIDEASVersion.Combo
                    End If

            End Select

            Return enumResult

        End Function
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Shared Function ConvertIdeasVersion(ByVal eIdeasVersion As EnumIdeasVersion) As String
            Select Case eIdeasVersion
                Case EnumIdeasVersion.One
                    Return "1"
                Case EnumIdeasVersion.Two
                    Return "2"
                Case EnumIdeasVersion.TwoGender
                    Return "2.5"
                Case EnumIdeasVersion.Combo
                    Return "Combo"
                Case EnumIdeasVersion.ComboGender
                    Return "ComboGender"
                Case Else
                    Return String.Empty
            End Select
        End Function
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	


        Public Shared Function GetToken(ByVal eIdeasVersion As EnumIdeasVersion, ByVal strURL As String, ByVal strLang As String, ByVal strRemoveCard As String) As IdeasRM.TokenResponse
            Dim ideasTokenResponse As IdeasRM.TokenResponse = Nothing
            Dim ideasHelper As IdeasRM.IHelper = IdeasRM.HelpFactory.createHelper()

            Select Case eIdeasVersion
                Case IdeasBLL.EnumIdeasVersion.One
                    ideasTokenResponse = ideasHelper.getToken(IdeasBLL.DeptCode, IdeasBLL.RaCode, IdeasBLL.AppRefId, _
                                                              RedirectHandler.AppendPageKeyToURL(strURL), "Target", strLang, strRemoveCard, ConvertIdeasVersion(eIdeasVersion))

                Case IdeasBLL.EnumIdeasVersion.Two
                    ideasTokenResponse = ideasHelper.getToken(IdeasBLL.DeptCode, IdeasBLL.RaCode, IdeasBLL.AppRefId, _
                                                              RedirectHandler.AppendPageKeyToURL(strURL), "Target", strLang, strRemoveCard, ConvertIdeasVersion(eIdeasVersion))

                Case IdeasBLL.EnumIdeasVersion.TwoGender
                    ideasTokenResponse = ideasHelper.getToken(IdeasBLL.DeptCode, IdeasBLL.RaCode_IDEAS2, IdeasBLL.AppRefId, _
                                                              RedirectHandler.AppendPageKeyToURL(strURL), "Target", strLang, strRemoveCard, ConvertIdeasVersion(eIdeasVersion))

                Case IdeasBLL.EnumIdeasVersion.Combo
                    ideasTokenResponse = ideasHelper.registerBrokerService(IdeasBLL.DeptCode, IdeasBLL.RaCode, IdeasBLL.AppRefId, _
                                                                           RedirectHandler.AppendPageKeyToURL(strURL), "Target", "", "", "", "", strLang, "", strRemoveCard, "", False)

                Case IdeasBLL.EnumIdeasVersion.ComboGender
                    ideasTokenResponse = ideasHelper.registerBrokerService(IdeasBLL.DeptCode, IdeasBLL.RaCode, IdeasBLL.AppRefId, _
                                                                           RedirectHandler.AppendPageKeyToURL(strURL), "Target", "", "", "", "", strLang, "", strRemoveCard, "", True)
            End Select

            Return ideasTokenResponse
        End Function

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Sub UpdateIDEASComboInfo(ByVal udtUserAC As UserACModel, ByVal strInstalled As String, ByVal strVersion As String)
            Dim db As New Database

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)

                Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SPID", SqlDbType.Char, 8, udtServiceProvider.SPID), _
                                db.MakeInParam("@DataEntryID", SqlDbType.VarChar, 20, String.Empty), _
                                db.MakeInParam("@Installed", SqlDbType.Char, 1, IIf(strInstalled Is Nothing, YesNo.No, strInstalled)), _
                                db.MakeInParam("@Version", SqlDbType.VarChar, 20, IIf(strVersion Is Nothing, String.Empty, strVersion)), _
                                db.MakeInParam("@LastUpdateDtm", SqlDbType.DateTime, 8, DateTime.Now) _
                                }

                db.RunProc("proc_IDEASComboClientLogBook_add_upd", parms)

            Else
                Dim udtDataEntryUser As DataEntryUserModel = CType(udtUserAC, DataEntryUserModel)

                Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SPID", SqlDbType.Char, 8, udtDataEntryUser.SPID), _
                                db.MakeInParam("@DataEntryID", SqlDbType.VarChar, 20, udtDataEntryUser.DataEntryAccount), _
                                db.MakeInParam("@Installed", SqlDbType.Char, 1, IIf(strInstalled Is Nothing, YesNo.No, strInstalled)), _
                                db.MakeInParam("@Version", SqlDbType.VarChar, 20, IIf(strVersion Is Nothing, String.Empty, strVersion)), _
                                db.MakeInParam("@LastUpdateDtm", SqlDbType.DateTime, 8, DateTime.Now) _
                                }

                db.RunProc("proc_IDEASComboClientLogBook_add_upd", parms)

            End If

        End Sub
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

#End Region

    End Class

End Namespace