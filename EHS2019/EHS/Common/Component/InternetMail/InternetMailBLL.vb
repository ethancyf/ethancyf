Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports Common.DataAccess

Namespace Component.InternetMail

    ''' <summary>
    ''' Internet Mail BLL To Handle [dbo].[InternetMail] And [dbo].[MailTemplate]
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InternetMailBLL

        Private udtDB As New Database()

        Public Sub New()
        End Sub

        Public Function GetMailSentCountByDay() As Integer
            Dim dtResult As DataTable = New DataTable
            Dim intRes As Integer = 0
            Try
                udtDB.RunProc("proc_InternetMailLogRowCount_get_ByDay", dtResult)

                If dtResult.Rows(0)(0) > 0 Then
                    intRes = CInt(dtResult.Rows(0)(0))
                End If
                Return intRes
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetMailSentCountByHour() As Integer
            Dim dtResult As DataTable = New DataTable
            Dim intRes As Integer = 0
            Try
                udtDB.RunProc("proc_InternetMailLogRowCount_get_ByHour", dtResult)

                If dtResult.Rows(0)(0) > 0 Then
                    intRes = CInt(dtResult.Rows(0)(0))
                End If
                Return intRes
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetMailToBeSent(ByVal intMailCount As Integer) As InternetMailModelCollection

            Dim udtInternetMailModelCollection As New InternetMailModelCollection()
            Dim dtmSystemDtm, dtmSentDtm As Nullable(Of DateTime)
            Dim strMailID, strVersion, strMailAddress, strMailLanguage, strEngParam, strChiParam, strSendStatus, strSPID As String

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@Record_count", SqlDbType.Int, 4, intMailCount)}
                Dim dtResult As New DataTable()
                '[System_Dtm], [Mail_ID], [Version], [Mail_Address], [Mail_Language],
                '[Eng_Parameter], [Chi_Parameter], [Send_Status], [Sent_Dtm]
                udtDB.RunProc("proc_InternetMail_get_ToSend", params, dtResult)

                For i As Integer = 0 To dtResult.Rows.Count - 1
                    Dim drRow As DataRow = dtResult.Rows(i)

                    If IsDBNull(drRow("System_Dtm")) Then dtmSystemDtm = Nothing Else dtmSystemDtm = Convert.ToDateTime(drRow("System_Dtm"))
                    If IsDBNull(drRow("Mail_ID")) Then strMailID = Nothing Else strMailID = drRow("Mail_ID").ToString().Trim()
                    If IsDBNull(drRow("Version")) Then strVersion = Nothing Else strVersion = drRow("Version").ToString().Trim()
                    If IsDBNull(drRow("Mail_Address")) Then strMailAddress = Nothing Else strMailAddress = drRow("Mail_Address").ToString().Trim()
                    If IsDBNull(drRow("Mail_Language")) Then strMailLanguage = Nothing Else strMailLanguage = drRow("Mail_Language").ToString().Trim()
                    If IsDBNull(drRow("Eng_Parameter")) Then strEngParam = Nothing Else strEngParam = drRow("Eng_Parameter").ToString().Trim()
                    If IsDBNull(drRow("Chi_Parameter")) Then strChiParam = Nothing Else strChiParam = drRow("Chi_Parameter").ToString().Trim()
                    If IsDBNull(drRow("Send_Status")) Then strSendStatus = Nothing Else strSendStatus = drRow("Send_Status").ToString().Trim()
                    If IsDBNull(drRow("Sent_Dtm")) Then dtmSentDtm = Nothing Else dtmSentDtm = Convert.ToDateTime(drRow("Sent_Dtm"))

                    ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    If IsDBNull(drRow("SP_ID")) Then strSPID = Nothing Else strSPID = drRow("SP_ID").ToString().Trim()

                    Dim udtInternetMailModel As InternetMailModel = New InternetMailModel(dtmSystemDtm, strMailID, strVersion, _
                        strMailAddress, strMailLanguage, strEngParam, strChiParam, strSendStatus, dtmSentDtm, strSPID)
                    ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

                    udtInternetMailModelCollection.Add(udtInternetMailModel)
                Next

            Catch ex As Exception
                Throw ex
            End Try

            Return udtInternetMailModelCollection
        End Function

        ''' <summary>
        ''' [External] To Retrieve Lastest Version Mail Template 
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strMailID">Mail Template ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMailTemplate(ByRef udtDB As Database, ByVal strMailID As String) As MailTemplateModel
            Dim udtMailTemplateModel As MailTemplateModel = Nothing
            Dim dtmCreateDtm, dtmUpdateDtm As Nullable(Of DateTime)
            Dim strVersion, strMailType, strMailSubjectEng, strMailSubjectChi As String
            Dim strMailBodyEng, strMailBodyChi, strRecordStatus, strCreateBy, strUpdateBy As String

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@Mail_ID", MailTemplateModel.Mail_IDDataType, MailTemplateModel.Mail_IDDataSize, strMailID)}

                Dim dtResult As New DataTable()
                ' [Mail_ID], [Version], [Mail_Type], [Mail_Subject_Eng], [Mail_Subject_Chi],
                ' [Mail_Body_Eng], [Mail_Body_Chi], [Record_Status], 
                ' [Create_By], [Create_Dtm], [Update_By], [Update_Dtm]
                udtDB.RunProc("proc_MailTemplate_get_ByMailID", params, dtResult)

                If dtResult.Rows.Count > 0 Then
                    Dim drRow As DataRow = dtResult.Rows(0)

                    strVersion = drRow("Version").ToString().Trim()
                    If IsDBNull(drRow("Mail_Type")) Then strMailType = Nothing Else strMailType = drRow("Mail_Type").ToString().Trim()
                    If IsDBNull(drRow("Mail_Subject_Eng")) Then strMailSubjectEng = Nothing Else strMailSubjectEng = drRow("Mail_Subject_Eng").ToString().Trim()
                    If IsDBNull(drRow("Mail_Subject_Chi")) Then strMailSubjectChi = Nothing Else strMailSubjectChi = drRow("Mail_Subject_Chi").ToString().Trim()
                    If IsDBNull(drRow("Mail_Body_Eng")) Then strMailBodyEng = Nothing Else strMailBodyEng = drRow("Mail_Body_Eng").ToString().Trim()
                    If IsDBNull(drRow("Mail_Body_Chi")) Then strMailBodyChi = Nothing Else strMailBodyChi = drRow("Mail_Body_Chi").ToString().Trim()
                    If IsDBNull(drRow("Record_Status")) Then strRecordStatus = Nothing Else strRecordStatus = drRow("Record_Status").ToString().Trim()

                    If IsDBNull(drRow("Create_By")) Then strCreateBy = Nothing Else strCreateBy = drRow("Create_By").ToString().Trim()
                    If IsDBNull(drRow("Create_Dtm")) Then dtmCreateDtm = Nothing Else dtmCreateDtm = Convert.ToDateTime(drRow("Create_Dtm"))
                    If IsDBNull(drRow("Update_By")) Then strUpdateBy = Nothing Else strUpdateBy = drRow("Update_By").ToString().Trim()
                    If IsDBNull(drRow("Update_Dtm")) Then dtmUpdateDtm = Nothing Else dtmUpdateDtm = Convert.ToDateTime(drRow("Update_Dtm"))

                    udtMailTemplateModel = New MailTemplateModel(strMailID, strVersion, strMailType, strMailSubjectEng, strMailSubjectChi, _
                        strMailBodyEng, strMailBodyChi, strRecordStatus, strCreateBy, dtmCreateDtm, strUpdateBy, dtmUpdateDtm)
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtMailTemplateModel
        End Function

        Public Function GetMailTemplate(ByVal strMailID As String, ByVal strVersion As String) As MailTemplateModel
            Dim udtMailTemplateModel As MailTemplateModel = Nothing

            Dim dtmCreateDtm, dtmUpdateDtm As Nullable(Of DateTime)
            Dim strMailType, strMailSubjectEng, strMailSubjectChi As String
            Dim strMailBodyEng, strMailBodyChi, strRecordStatus, strCreateBy, strUpdateBy As String

            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Mail_ID", MailTemplateModel.Mail_IDDataType, MailTemplateModel.Mail_IDDataSize, strMailID), _
                    udtDB.MakeInParam("@Version", MailTemplateModel.VersionDataType, MailTemplateModel.VersionDataSize, strVersion)}

                Dim dtResult As New DataTable()
                ' [Mail_ID], [Version], [Mail_Type], [Mail_Subject_Eng], [Mail_Subject_Chi],
                ' [Mail_Body_Eng], [Mail_Body_Chi], [Record_Status], 
                ' [Create_By], [Create_Dtm], [Update_By], [Update_Dtm]
                udtDB.RunProc("proc_MailTemplate_get_ByKey", params, dtResult)

                If dtResult.Rows.Count > 0 Then
                    Dim drRow As DataRow = dtResult.Rows(0)

                    If IsDBNull(drRow("Mail_Type")) Then strMailType = Nothing Else strMailType = drRow("Mail_Type").ToString().Trim()
                    If IsDBNull(drRow("Mail_Subject_Eng")) Then strMailSubjectEng = Nothing Else strMailSubjectEng = drRow("Mail_Subject_Eng").ToString().Trim()
                    If IsDBNull(drRow("Mail_Subject_Chi")) Then strMailSubjectChi = Nothing Else strMailSubjectChi = drRow("Mail_Subject_Chi").ToString().Trim()
                    If IsDBNull(drRow("Mail_Body_Eng")) Then strMailBodyEng = Nothing Else strMailBodyEng = drRow("Mail_Body_Eng").ToString().Trim()
                    If IsDBNull(drRow("Mail_Body_Chi")) Then strMailBodyChi = Nothing Else strMailBodyChi = drRow("Mail_Body_Chi").ToString().Trim()
                    If IsDBNull(drRow("Record_Status")) Then strRecordStatus = Nothing Else strRecordStatus = drRow("Record_Status").ToString().Trim()

                    If IsDBNull(drRow("Create_By")) Then strCreateBy = Nothing Else strCreateBy = drRow("Create_By").ToString().Trim()
                    If IsDBNull(drRow("Create_Dtm")) Then dtmCreateDtm = Nothing Else dtmCreateDtm = Convert.ToDateTime(drRow("Create_Dtm"))
                    If IsDBNull(drRow("Update_By")) Then strUpdateBy = Nothing Else strUpdateBy = drRow("Update_By").ToString().Trim()
                    If IsDBNull(drRow("Update_Dtm")) Then dtmUpdateDtm = Nothing Else dtmUpdateDtm = Convert.ToDateTime(drRow("Update_Dtm"))

                    udtMailTemplateModel = New MailTemplateModel(strMailID, strVersion, strMailType, strMailSubjectEng, strMailSubjectChi, _
                        strMailBodyEng, strMailBodyChi, strRecordStatus, strCreateBy, dtmCreateDtm, strUpdateBy, dtmUpdateDtm)
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return udtMailTemplateModel
        End Function

        ''' <summary>
        ''' [External] Add Internet Mail to The Mail Queue
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtInternetMail"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AddInternetMail(ByRef udtDB As Database, ByVal udtInternetMail As InternetMailModel) As Boolean
            Try
                If udtInternetMail.MailAddress Is Nothing OrElse udtInternetMail.MailAddress.Trim() = "" Then
                    Throw New ArgumentException("Mail Address Must Be Fill in AddInternetMail")
                End If
                If udtInternetMail.MailLanguage Is Nothing OrElse udtInternetMail.MailLanguage.Trim() = "" Then
                    Throw New ArgumentException("Mail Language Must Be Fill in AddInternetMail")
                End If

                ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Dim prams() As SqlParameter = { _
                              udtDB.MakeInParam("@Mail_ID", InternetMailModel.Mail_IDDataType, InternetMailModel.Mail_IDDataSize, udtInternetMail.MailID), _
                              udtDB.MakeInParam("@Version", InternetMailModel.VersionDataType, InternetMailModel.VersionDataSize, udtInternetMail.Version), _
                              udtDB.MakeInParam("@Mail_Address", InternetMailModel.Mail_AddressDataType, InternetMailModel.Mail_AddressDataSize, udtInternetMail.MailAddress), _
                              udtDB.MakeInParam("@Mail_Language", InternetMailModel.Mail_LanguageDataType, InternetMailModel.Mail_LanguageDataSize, udtInternetMail.MailLanguage), _
                              udtDB.MakeInParam("@Eng_Parameter", InternetMailModel.Eng_ParameterDataType, InternetMailModel.Eng_ParameterDataSize, IIf(udtInternetMail.EngParameter Is Nothing, DBNull.Value, udtInternetMail.EngParameter)), _
                              udtDB.MakeInParam("@Chi_Parameter", InternetMailModel.Chi_ParameterDataType, InternetMailModel.Chi_ParameterDataSize, IIf(udtInternetMail.ChiParameter Is Nothing, DBNull.Value, udtInternetMail.ChiParameter)), _
                              udtDB.MakeInParam("@SP_ID", InternetMailModel.SPIDDataType, InternetMailModel.SPIDDataSize, IIf(udtInternetMail.SPID Is Nothing, DBNull.Value, udtInternetMail.SPID))}
                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

                udtDB.RunProc("proc_InternetMail_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ''' <summary>
        ''' [Public] Update The Internet Mail Status to Sent
        ''' </summary>
        ''' <param name="udtInternetMail"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateInternetMailSent(ByVal udtInternetMail As InternetMailModel)
            Try

                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@System_Dtm", InternetMailModel.System_DtmDataType, InternetMailModel.System_DtmDataSize, udtInternetMail.SystemDtm), _
                    udtDB.MakeInParam("@Mail_ID", InternetMailModel.Mail_IDDataType, InternetMailModel.Mail_IDDataSize, udtInternetMail.MailID), _
                    udtDB.MakeInParam("@Version", InternetMailModel.VersionDataType, InternetMailModel.VersionDataSize, udtInternetMail.Version)}

                udtDB.RunProc("proc_InternetMail_upd_sent", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function GetMailByMinuteBefore(ByVal strSPID As String, ByVal strMailID As String, ByVal intMinuteBefore As Integer) As DataTable
            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                  udtDB.MakeInParam("@SP_ID", InternetMailModel.SPIDDataType, InternetMailModel.SPIDDataSize, IIf(IsNothing(strSPID), DBNull.Value, strSPID)), _
                  udtDB.MakeInParam("@Mail_ID", InternetMailModel.Mail_IDDataType, InternetMailModel.Mail_IDDataSize, IIf(IsNothing(strMailID), DBNull.Value, strMailID)), _
                  udtDB.MakeInParam("@Minute_Before", SqlDbType.Int, 8, intMinuteBefore) _
            }

            udtDB.RunProc("proc_InternetMail_ByMinuteBefore", prams, dt)

            Return dt
        End Function
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        Private Function GetServiceProviderModel(ByVal udtDB As Database, ByVal strSPID As String) As ServiceProvider.ServiceProviderModel
            Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL()
            Dim udtSPModel As ServiceProvider.ServiceProviderModel = udtSPBLL.GetServiceProviderBySPID(udtDB, strSPID)

            ' Get Default Language
            Dim dtUser As New DataTable()
            udtDB.RunProc("proc_HCSPUserAC_get", New SqlParameter() {udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, strSPID)}, dtUser)

            If dtUser.Rows.Count <= 0 Then
                Throw New ArgumentException("HCSP Account not found for SP:[" + strSPID + "]")
            End If

            udtSPModel.DefaultLanguage = dtUser.Rows(0)("Default_Language").ToString().Trim()
            Return udtSPModel

        End Function

        ''' <summary>
        ''' [Public] Submit Account Activation Email By EHRSS / EHS(S) (With DB access supplied)
        ''' </summary>
        ''' <param name="udtDB">Database Access object</param>
        ''' <param name="strSPID">Service Provider ID</param>
        ''' <param name="strCode">Account Activation Code</param>
        ''' <param name="blnIsJoinEHRSS">Whether the user has joined EHRSS</param>
        ''' <param name="blnNewEnrolment">Whether it is a enrolment confirmation email</param>
        ''' <param name="MSchemeCodeArrayList">list of scheme codes, which will be printed on the email</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SubmitAccountActivationEmail(ByRef udtDB As Database, ByVal strSPID As String, ByVal strCode As String, ByVal blnIsJoinEHRSS As Boolean, ByVal blnNewEnrolment As Boolean, ByVal MSchemeCodeArrayList As ArrayList) As Boolean
            Try
                ' Init
                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
                Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
                Dim udtEmail As New InternetMailModel()
                Dim udtMailTemplate As MailTemplateModel = Nothing

                ' Retrieve Mail Template Information
                If blnNewEnrolment Then
                    udtMailTemplate = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.AccountActivationEmail)
                Else
                    udtMailTemplate = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.SchemeEnrolmentEmail)
                End If


                udtEmail.MailID = udtMailTemplate.MailID
                udtEmail.Version = udtMailTemplate.Version

                ' Retrieve Service Provider Information
                Dim udtSPModel As ServiceProvider.ServiceProviderModel = Me.GetServiceProviderModel(udtDB, strSPID)
                udtEmail.MailAddress = udtSPModel.Email


                udtEmail.MailLanguage = Common.Component.InternetMailLanguage.Both
                ' Construct Parameter : [%Name%], [%Link%], [%TelNum%]
                Dim udtEngParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()
                Dim udtChiParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()


                Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
                Dim udtShemeBackOfficeModel As Scheme.SchemeBackOfficeModel
                'Modify the display of sentences due to existence of certain logos
                Dim blnLogoProvided As Boolean = False
                For Each strSchemeCode As String In MSchemeCodeArrayList
                    'udtShemeBackOfficeModel = udtSchemeBackOfficeBLL.getSchemeBackOfficeBySchemeCode(strSchemeCode.Trim)
                    udtShemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(strSchemeCode.Trim)
                    If udtShemeBackOfficeModel.ReturnLogoEnabled Then
                        blnLogoProvided = True
                        Exit For
                    End If
                Next

                Dim langCHI As CultureInfo = New CultureInfo("zh-TW")
                If blnNewEnrolment Then
                    If Not blnIsJoinEHRSS Then
                        If blnLogoProvided Then
                            udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNE", langCHI))
                            udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNE"))
                            'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到保安編碼器，服務提供者編號及計劃的標誌。")
                            'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID, token and Scheme logo(s) sent with our Enrolment Confirmation Letter within days.")
                        Else
                            udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNER", langCHI))
                            udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNER"))
                            'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到保安編碼器及服務提供者編號。")
                            'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID and token sent with our Enrolment Confirmation Letter within days.")
                        End If
                    Else
                        Dim udtTokenBLL As Token.TokenBLL = New Token.TokenBLL
                        'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------

                        ''CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                        ''-----------------------------------------------------------------------------------------
                        ''If udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSPModel.SPID, New Database).Project = TokenProjectType.PPIEPR Then
                        'If CStr(udtTokenBLL.GetTokenSerialNoByUserID(udtSPModel.SPID, New Database).Rows(0).Item("Project")).Trim = TokenProjectType.PPIEPR Then
                        '    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                        '    If blnLogoProvided Then
                        '        udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNP", langCHI))
                        '        udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNP"))
                        '        'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到你的服務提供者編號及計劃的標誌。")
                        '        'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID and Scheme logo(s) sent with our Enrolment Confirmation Letter within days.")
                        '    Else
                        '        udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNPR", langCHI))
                        '        udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNPR"))
                        '        'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到你的服務提供者編號。")
                        '        'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID sent with our Enrolment Confirmation Letter within days.")
                        '    End If
                        'Else
                        '    If blnLogoProvided Then
                        '        udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNE", langCHI))
                        '        udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNE"))
                        '        'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到保安編碼器，服務提供者編號及計劃的標誌。")
                        '        'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID, token and Scheme logo(s) sent with our Enrolment Confirmation Letter within days.")
                        '    Else
                        '        udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNER", langCHI))
                        '        udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNER"))
                        '        'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到保安編碼器及服務提供者編號。")
                        '        'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID and token sent with our Enrolment Confirmation Letter within days.")
                        '    End If
                        'End If

                        If CStr(udtTokenBLL.GetTokenSerialNoByUserID(udtSPModel.SPID, New Database).Rows(0).Item("Project")).Trim = TokenProjectType.EHCVS Then
                            If blnLogoProvided Then
                                udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNE", langCHI))
                                udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNE"))
                                'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到保安編碼器，服務提供者編號及計劃的標誌。")
                                'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID, token and Scheme logo(s) sent with our Enrolment Confirmation Letter within days.")
                            Else
                                udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNER", langCHI))
                                udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNER"))
                                'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到保安編碼器及服務提供者編號。")
                                'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID and token sent with our Enrolment Confirmation Letter within days.")
                            End If
                        Else
                            If blnLogoProvided Then
                                udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNP", langCHI))
                                udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNP"))
                                'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到你的服務提供者編號及計劃的標誌。")
                                'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID and Scheme logo(s) sent with our Enrolment Confirmation Letter within days.")
                            Else
                                udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNPR", langCHI))
                                udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentNPR"))
                                'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到你的服務提供者編號。")
                                'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Service Provider ID sent with our Enrolment Confirmation Letter within days.")
                            End If
                        End If

                        'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
                    End If
                Else
                    If Not blnIsJoinEHRSS Then
                        If blnLogoProvided Then
                            udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentSL", langCHI))
                            udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentSL"))
                            'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到計劃標誌。")
                            'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Scheme logo(s) sent with our Enrolment Confirmation Letter within days.")
                        Else
                            udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentS", langCHI))
                            udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentS"))
                            'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內收到登記確認書。")
                            'udtEngParamCollection.AddParam("EmailDescription1", "You will receive our Enrolment Confirmation Letter within days.")
                        End If
                    Else
                        If blnLogoProvided Then
                            udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentSL", langCHI))
                            udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentSL"))
                            'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內隨登記確認書收到計劃標誌。")
                            'udtEngParamCollection.AddParam("EmailDescription1", "You will receive your Scheme logo(s) sent with our Enrolment Confirmation Letter within days.")
                        Else
                            udtChiParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentS", langCHI))
                            udtEngParamCollection.AddParam("EmailDescription1", HttpContext.GetGlobalResourceObject("Text", "EmailContentS"))
                            'udtChiParamCollection.AddParam("EmailDescription1", "你會於日內收到登記確認書。")
                            'udtEngParamCollection.AddParam("EmailDescription1", "You will receive our Enrolment Confirmation Letter within days.")
                        End If
                    End If
                End If

                'Share the use of phone no with printout
                udtEngParamCollection.AddParam("RMPTel", udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentTelNo").Trim)
                udtChiParamCollection.AddParam("RMPTel", udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentTelNo").Trim)
                udtEngParamCollection.AddParam("ProfTel", udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentTelNo2").Trim)
                udtChiParamCollection.AddParam("ProfTel", udtGeneralFunction.getUserDefinedParameter("Printout", "EnrolmentTelNo2").Trim)

                'Select Description for new enrolment and scheme enrolement
                Dim strChiDesc As String
                Dim strEngDesc As String
                If blnNewEnrolment Then
                    Dim strAppLink As String = String.Empty
                    udtGeneralFunction.getSystemParameter("AppLink", strAppLink, String.Empty)
                    Dim strHCSPLink As String = String.Empty
                    udtGeneralFunction.getSystemParameter("HCSPAppPath", strHCSPLink, String.Empty)
                    strEngDesc = String.Format(HttpContext.GetGlobalResourceObject("Text", "EmailContentN1"), _
                                                strAppLink + "/" + strHCSPLink + "/AccountActivation/AccountActivation.aspx?lang=EN&code=" + strCode)
                    strChiDesc = String.Format(HttpContext.GetGlobalResourceObject("Text", "EmailContentN1", langCHI), _
                                                strAppLink + "/" + strHCSPLink + "/AccountActivation/AccountActivation.aspx?lang=ZH&code=" + strCode)
                    'strEngDesc = String.Format("You must complete the account activation process before you can use the eHealth System.  Please activate your Service Provider Account as soon as possible by clicking the following link : <br><a href='{0}'>{0}</a>", _
                    '                            strAppLink + "/" + strHCSPLink + "/AccountActivation/AccountActivation.aspx?lang=EN&code=" + strCode)
                    'strChiDesc = String.Format("你必須完成戶口啟動才能登入醫健通。請盡快點擊以下超連結，以啟動你的服務提供者戶口。<br><a href='{0}'>{0}</a>", _
                    '                            strAppLink + "/" + strHCSPLink + "/AccountActivation/AccountActivation.aspx?lang=ZH&code=" + strCode)
                Else
                    strChiDesc = HttpContext.GetGlobalResourceObject("Text", "EmailContentS1", langCHI)
                    strEngDesc = HttpContext.GetGlobalResourceObject("Text", "EmailContentS1")
                    'strChiDesc = "請使用你現有的服務提供者編號及保安編碼器登入醫健通，並操作新登記計劃的相關功能。"
                    'strEngDesc = "Please use the existing SPID and token for login and use the relevant functions with respect to the newly enrolled scheme(s)."
                End If
                udtEngParamCollection.AddParam("EmailDescription2", strEngDesc)
                udtChiParamCollection.AddParam("EmailDescription2", strChiDesc)

                'Prepare Schemes List
                Dim strSchemeEngList As String = String.Empty
                Dim strSchemeChiList As String = String.Empty

                For Each strSchemeCode As String In MSchemeCodeArrayList
                    udtShemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(strSchemeCode.Trim)

                    If strSchemeCode.Trim.Equals(SchemeBackOfficeSchemeCode.CIVSS) Then
                        Dim udtSP As ServiceProvider.ServiceProviderBLL = New ServiceProvider.ServiceProviderBLL
                        If udtSP.CheckServiceProviderExistInIVSS(udtSPModel.HKID, New Database) Then
                            'strSchemeEngList = strSchemeEngList + String.Format("- (Renew) {0} ({1})", _
                            '                udtVoucherScheme.getSchemeDescriptionFromCode(strSchemeCode).Trim, _
                            '                udtSchemeBLL.getExternalSchemeCodeFromMasterSchemeCode(strSchemeCode).Trim)
                            'strSchemeChiList = strSchemeChiList + String.Format("- (繼續參加) {0}", _
                            '                udtVoucherScheme.getSchemeChiDescriptionFromCode(strSchemeCode).Trim)
                            strSchemeEngList = strSchemeEngList + String.Format("- ({0}) {1} ({2})", _
                                            HttpContext.GetGlobalResourceObject("Text", "EmailContentRN").ToString.Trim, _
                                            udtShemeBackOfficeModel.SchemeDesc.Trim, _
                                            udtShemeBackOfficeModel.DisplayCode.Trim)
                            strSchemeChiList = strSchemeChiList + String.Format("- ({0}) {1}", _
                                            HttpContext.GetGlobalResourceObject("Text", "EmailContentRN", langCHI).ToString.Trim, _
                                            udtShemeBackOfficeModel.SchemeDescChi.Trim)
                        Else
                            strSchemeEngList = strSchemeEngList + String.Format("- {0} ({1})", _
                                            udtShemeBackOfficeModel.SchemeDesc.Trim, _
                                            udtShemeBackOfficeModel.DisplayCode.Trim)
                            strSchemeChiList = strSchemeChiList + String.Format("- {0}", _
                                            udtShemeBackOfficeModel.SchemeDescChi.Trim)
                        End If
                    Else
                        If Not udtShemeBackOfficeModel.ReturnLogoEnabled Then
                            'strSchemeEngList = strSchemeEngList + String.Format("- {0} ({1}) [No Scheme logo will be provided for this scheme]", _
                            '                udtVoucherScheme.getSchemeDescriptionFromCode(strSchemeCode).Trim, _
                            '                udtSchemeBLL.getExternalSchemeCodeFromMasterSchemeCode(strSchemeCode).Trim)
                            'strSchemeChiList = strSchemeChiList + String.Format("- {0}[此計劃將不會有計劃的標誌]", _
                            '                udtVoucherScheme.getSchemeChiDescriptionFromCode(strSchemeCode).Trim)
                            strSchemeEngList = strSchemeEngList + String.Format("- {0} ({1}) [{2}]", _
                                            udtShemeBackOfficeModel.SchemeDesc.Trim, _
                                            udtShemeBackOfficeModel.DisplayCode.Trim, _
                                            HttpContext.GetGlobalResourceObject("Text", "EmailContentWL").ToString.Trim)
                            strSchemeChiList = strSchemeChiList + String.Format("- {0}[{1}]", _
                                            udtShemeBackOfficeModel.SchemeDescChi.Trim, _
                                            HttpContext.GetGlobalResourceObject("Text", "EmailContentWL", langCHI).ToString.Trim)
                        Else
                            strSchemeEngList = strSchemeEngList + String.Format("- {0} ({1})", _
                                            udtShemeBackOfficeModel.SchemeDesc.Trim, _
                                            udtShemeBackOfficeModel.DisplayCode.Trim)
                            strSchemeChiList = strSchemeChiList + String.Format("- {0}", _
                                            udtShemeBackOfficeModel.SchemeDescChi.Trim)
                        End If
                    End If
                    strSchemeEngList = strSchemeEngList + "<br>"
                    strSchemeChiList = strSchemeChiList + "<br>"
                Next
                ' Replace last <br>
                strSchemeChiList = strSchemeChiList.Trim.Substring(0, strSchemeChiList.Trim.Length - 4)
                strSchemeEngList = strSchemeEngList.Trim.Substring(0, strSchemeEngList.Trim.Length - 4)


                udtEngParamCollection.AddParam("Schemes", strSchemeEngList)
                udtChiParamCollection.AddParam("Schemes", strSchemeChiList)

                'Show Scheme
                udtEmail.EngParameter = udtParamFunction.GetParameterString(udtEngParamCollection)
                udtEmail.ChiParameter = udtParamFunction.GetParameterString(udtChiParamCollection)

                ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                udtEmail.SPID = strSPID
                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

                Me.AddInternetMail(udtDB, udtEmail)

                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ''' <summary>
        ''' [Public] Submit Account Activation Email By EHRSS / EHS(S) (Own DataBase Access object)
        ''' </summary>
        ''' <param name="strSPID">Service Provider ID</param>
        ''' <param name="strCode">Account Activation Code</param>
        ''' <param name="blnIsJoinEHRSS">Whether the user has joined EHRSS</param>
        ''' <param name="blnNewEnrolment">Whether it is a enrolment confirmation email</param>
        ''' <param name="strSchemCodeArrayList">list of scheme codes, which will be printed on the email</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SubmitAccountActivationEmail(ByVal strSPID As String, ByVal strCode As String, ByVal blnIsJoinEHRSS As Boolean, ByVal blnNewEnrolment As Boolean, ByVal strSchemCodeArrayList As ArrayList) As Boolean
            Try
                Me.udtDB.BeginTransaction()
                Dim blnReturn As Boolean = Me.SubmitAccountActivationEmail(Me.udtDB, strSPID, strCode, blnIsJoinEHRSS, blnNewEnrolment, strSchemCodeArrayList)
                If blnReturn Then
                    Me.udtDB.CommitTransaction()
                    Return True
                Else
                    Me.udtDB.RollBackTranscation()
                    Return False
                End If

            Catch ex As Exception
                Me.udtDB.RollBackTranscation()
                Throw ex
                Return False
            End Try

        End Function

        ''' <summary>
        ''' [External] Submit Forgot Password Email
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strCode"></param>
        ''' <returns></returns>
        ''' <remarks>'[%Name%]:Service Provider Name, [%Link%]:Activation Link, [%TelNum%]:HCVU Telphone Number</remarks>
        Public Function SubmitForgotPasswordEmail(ByRef udtDB As Database, ByVal strSPID As String, ByVal strCode As String) As Boolean
            Try
                ' Init
                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
                Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
                Dim udtEmail As New InternetMailModel()

                ' Retrieve Mail Template Information
                Dim udtMailTemplate As MailTemplateModel = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.ResetPasswordEmail)

                udtEmail.MailID = udtMailTemplate.MailID
                udtEmail.Version = udtMailTemplate.Version

                ' Retrieve Service Provider Information
                Dim udtSPModel As ServiceProvider.ServiceProviderModel = Me.GetServiceProviderModel(udtDB, strSPID)
                udtEmail.MailAddress = udtSPModel.Email

                If udtSPModel.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.Chinese Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.ChiHeader
                ElseIf udtSPModel.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.English Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                Else
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                End If

                ' Construct Parameter : [%Name%], [%Link%], [%TelNum%]
                Dim udtEngParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()
                Dim udtChiParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()

                ' [%Name%]: Service Provider Name:
                udtEngParamCollection.AddParam("Name", udtSPModel.EnglishName)
                If udtSPModel.ChineseName Is Nothing OrElse udtSPModel.ChineseName.Trim() = "" Then
                    udtChiParamCollection.AddParam("Name", udtSPModel.EnglishName)
                Else
                    udtChiParamCollection.AddParam("Name", udtSPModel.ChineseName)
                End If

                ' [%TelNum%]: HCVU Telphone Number
                Dim strTelNum As String = udtGeneralFunction.getUserDefinedParameter(Common.Component.UserParamCategory.Mail, "HCVUEmailTelNum")
                'udtGeneralFunction.getSystemParameter("HCVUEmailTelNum", strTelNum, String.Empty)
                udtEngParamCollection.AddParam("TelNum", strTelNum)
                udtChiParamCollection.AddParam("TelNum", strTelNum)

                ' [%Link%]: AppLink + Function + ActivationCode
                Dim strAppLink As String = String.Empty
                udtGeneralFunction.getSystemParameter("AppLink", strAppLink, String.Empty)
                Dim strHCSPLink As String = String.Empty
                udtGeneralFunction.getSystemParameter("HCSPAppPath", strHCSPLink, String.Empty)

                udtEngParamCollection.AddParam("Link", strAppLink + "/" + strHCSPLink + "/ForgotPassword/ForgotPassword.aspx?lang=EN&code=" + strCode)
                udtChiParamCollection.AddParam("Link", strAppLink + "/" + strHCSPLink + "/ForgotPassword/ForgotPassword.aspx?lang=ZH&code=" + strCode)

                udtEmail.EngParameter = udtParamFunction.GetParameterString(udtEngParamCollection)
                udtEmail.ChiParameter = udtParamFunction.GetParameterString(udtChiParamCollection)

                ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                udtEmail.SPID = strSPID
                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

                Me.AddInternetMail(udtDB, udtEmail)

                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        ''' <summary>
        ''' Submit Notification of Delisting Email
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strSPID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SubmitDelistNotificationEmail(ByRef udtDB As Database, ByVal strSPID As String, ByVal strScheme As String) As Boolean
            Try
                ' Init
                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
                Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
                Dim udtEmail As New InternetMailModel()

                ' Retrieve Service Provider Information
                Dim udtSP As ServiceProvider.ServiceProviderModel = GetServiceProviderModel(udtDB, strSPID)

                Dim blnAllSchemeDelisted As Boolean = True
                For Each udtScheme As SchemeInformation.SchemeInformationModel In udtSP.SchemeInfoList.Values
                    If udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary AndAlso udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
                        blnAllSchemeDelisted = False
                        Exit For
                    End If
                Next

                ' Retrieve Mail Template Information
                Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
                Dim udtSchemeBO As Scheme.SchemeBackOfficeModel = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(strScheme)

                Dim udtMailTemplate As MailTemplateModel = Nothing

                Dim intOption As Integer = 0
                If blnAllSchemeDelisted Then intOption += 1
                If udtSchemeBO.ReturnLogoEnabled Then intOption += 2

                Select Case intOption
                    Case 0
                        ' Not all schemes delisted + no logo
                        udtMailTemplate = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.DelistingNotificationMailWithNothing)
                    Case 1
                        ' All schemes delisted + no logo
                        udtMailTemplate = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.DelistingNotificationMailWithToken)
                    Case 2
                        ' Not all schemes delisted + have logo
                        udtMailTemplate = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.DelistingNotificationMailWithLogo)
                    Case 3
                        ' All schemes delisted + have logo
                        udtMailTemplate = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.DelistingNotificationMailWithLogoToken)
                End Select

                udtEmail.MailID = udtMailTemplate.MailID
                udtEmail.Version = udtMailTemplate.Version
                udtEmail.MailAddress = udtSP.Email

                If udtSP.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.Chinese Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.ChiHeader
                ElseIf udtSP.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.English Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                Else
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                End If

                ' Construct Parameter : [%Name%]
                Dim udtEngParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()
                Dim udtChiParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()

                ' [%Name%]: Service Provider Name:
                udtEngParamCollection.AddParam("Name", udtSP.EnglishName)
                If IsNothing(udtSP.ChineseName) OrElse udtSP.ChineseName.Trim = String.Empty Then
                    udtChiParamCollection.AddParam("Name", udtSP.EnglishName)
                Else
                    udtChiParamCollection.AddParam("Name", udtSP.ChineseName)
                End If

                ' [%Scheme%]: Scheme Name:
                udtEngParamCollection.AddParam("Scheme", udtSchemeBO.SchemeDesc.Trim + " (" + udtSchemeBO.DisplayCode.Trim + ")")
                udtChiParamCollection.AddParam("Scheme", udtSchemeBO.SchemeDescChi.Trim)

                udtEmail.EngParameter = udtParamFunction.GetParameterString(udtEngParamCollection)
                udtEmail.ChiParameter = udtParamFunction.GetParameterString(udtChiParamCollection)

                ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                udtEmail.SPID = strSPID
                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

                Me.AddInternetMail(udtDB, udtEmail)

                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Submit Change Email Confirmation Email
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strCode">Activation Code</param>
        ''' <returns></returns>
        ''' <remarks>The New Email is retrieve from Service Provider Tentative Email</remarks>
        Public Function SubmitEmailAddressChangeConfirmationEmail(ByRef udtDB As Database, ByVal strSPID As String, ByVal strCode As String) As Boolean
            Try

                ' Retrieve the New Email Address from Service Provider Tentative Email

                ' Init
                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
                Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
                Dim udtEmail As New InternetMailModel()

                ' Retrieve Mail Template Information
                Dim udtMailTemplate As MailTemplateModel = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.ConfirmationChangeEmail)

                udtEmail.MailID = udtMailTemplate.MailID
                udtEmail.Version = udtMailTemplate.Version

                ' Retrieve Service Provider Information
                Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL()
                Dim udtSPModel As ServiceProvider.ServiceProviderModel = Me.GetServiceProviderModel(udtDB, strSPID)

                ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
                'Dim strEmail As String = udtSPBLL.GetserviceProviderPermanentTentativeEmail(strSPID, udtDB)
                'udtEmail.MailAddress = strEmail
                udtEmail.MailAddress = udtSPModel.TentativeEmail
                ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]

                If udtSPModel.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.Chinese Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.ChiHeader
                ElseIf udtSPModel.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.English Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                Else
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                End If

                ' Construct Parameter : [%Name%], [%Link%], [%TelNum%]
                Dim udtEngParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()
                Dim udtChiParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()

                ' [%Name%]: Service Provider Name:
                udtEngParamCollection.AddParam("Name", udtSPModel.EnglishName)
                If udtSPModel.ChineseName Is Nothing OrElse udtSPModel.ChineseName.Trim() = "" Then
                    udtChiParamCollection.AddParam("Name", udtSPModel.EnglishName)
                Else
                    udtChiParamCollection.AddParam("Name", udtSPModel.ChineseName)
                End If

                ' [%TelNum%]: HCVU Telphone Number
                Dim strTelNum As String = udtGeneralFunction.getUserDefinedParameter(Common.Component.UserParamCategory.Mail, "HCVUEmailTelNum")
                'udtGeneralFunction.getSystemParameter("HCVUEmailTelNum", strTelNum, String.Empty)
                udtEngParamCollection.AddParam("TelNum", strTelNum)
                udtChiParamCollection.AddParam("TelNum", strTelNum)

                ' [%Link%]: AppLink + Function + ActivationCode
                Dim strAppLink As String = String.Empty
                udtGeneralFunction.getSystemParameter("AppLink", strAppLink, String.Empty)
                Dim strHCSPLink As String = String.Empty
                udtGeneralFunction.getSystemParameter("HCSPAppPath", strHCSPLink, String.Empty)

                udtEngParamCollection.AddParam("Link", strAppLink + "/" + strHCSPLink + "/ChangeEmail/ChangeEmail.aspx?lang=EN&code=" + strCode)
                udtChiParamCollection.AddParam("Link", strAppLink + "/" + strHCSPLink + "/ChangeEmail/ChangeEmail.aspx?lang=ZH&code=" + strCode)

                udtEmail.EngParameter = udtParamFunction.GetParameterString(udtEngParamCollection)
                udtEmail.ChiParameter = udtParamFunction.GetParameterString(udtChiParamCollection)

                ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                udtEmail.SPID = strSPID
                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

                Me.AddInternetMail(udtDB, udtEmail)

                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' [External] Submit Reset Password Email
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="strCode"></param>
        ''' <param name="strRefTime"></param>
        ''' <returns></returns>
        ''' <remarks>'[%Name%]:Service Provider Name, [%Code%]:Verification Code, [%RefTime%]:Code Generated Time </remarks>
        Public Function SubmitResetPasswordEmail(ByRef udtDB As Database, ByVal strSPID As String, ByVal strCode As String, ByVal strRefTime As String) As Boolean
            Try
                ' Init
                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
                Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
                Dim udtEmail As New InternetMailModel()

                ' Retrieve Mail Template Information
                Dim udtMailTemplate As MailTemplateModel = Me.GetMailTemplate(udtDB, Common.Component.MailTemplateID.ResetPasswordEmail)

                udtEmail.MailID = udtMailTemplate.MailID
                udtEmail.Version = udtMailTemplate.Version

                ' Retrieve Service Provider Information
                Dim udtSPModel As ServiceProvider.ServiceProviderModel = Me.GetServiceProviderModel(udtDB, strSPID)
                udtEmail.MailAddress = udtSPModel.Email

                If udtSPModel.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.Chinese Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.ChiHeader
                ElseIf udtSPModel.DefaultLanguage = Common.Component.ServiceProviderDefaultLanguage.English Then
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                Else
                    udtEmail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader
                End If

                ' Construct Parameter : [%Name%], [%Code%], [%RefTime%]
                Dim udtEngParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()
                Dim udtChiParamCollection As New Common.ComFunction.ParameterFunction.ParameterCollection()

                ' [%Name%]: Service Provider Name:
                udtEngParamCollection.AddParam("Name", udtSPModel.EnglishName)
                If udtSPModel.ChineseName Is Nothing OrElse udtSPModel.ChineseName.Trim() = "" Then
                    udtChiParamCollection.AddParam("Name", udtSPModel.EnglishName)
                Else
                    udtChiParamCollection.AddParam("Name", udtSPModel.ChineseName)
                End If

                ' [%Code%]: Verification Code
                udtEngParamCollection.AddParam("Code", strCode)
                udtChiParamCollection.AddParam("Code", strCode)

                ' [%RefTime%]: Verification Code Generated Time
                udtEngParamCollection.AddParam("RefTime", strRefTime)
                udtChiParamCollection.AddParam("RefTime", strRefTime)

                udtEmail.EngParameter = udtParamFunction.GetParameterString(udtEngParamCollection)
                udtEmail.ChiParameter = udtParamFunction.GetParameterString(udtChiParamCollection)

                udtEmail.SPID = strSPID

                Me.AddInternetMail(udtDB, udtEmail)

                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        End Function
    End Class
End Namespace

