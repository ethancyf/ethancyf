﻿Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.HATransaction
Imports Common.Component.SortedGridviewHeader
Imports Common.Component.StaticData
Imports Common.Format
Imports Common.WebService.Interface
Imports HCSP.BLL

Partial Public Class ucVaccinationRecord
    Inherits System.Web.UI.UserControl

#Region "Private Class"

    Private Class ViewIndex
        Public Const Control As Integer = 0
        Public Const Simple As Integer = 1
        Public Const SimpleNoName As Integer = 2
    End Class

    Private Class SESS
        Public Const TranDetailList As String = "ucVaccinationRecord_TranDetailList"
    End Class

#End Region

#Region "Field"

    Private _udtHAVaccineResult As HAVaccineResult
    Private _udtDHVaccineResult As DHVaccineResult

#End Region

#Region "Property"

    Public Property HAVaccineResult() As HAVaccineResult
        Get
            Return _udtHAVaccineResult
        End Get
        Set(ByVal value As HAVaccineResult)
            _udtHAVaccineResult = value
        End Set
    End Property

    Public Property DHVaccineResult() As DHVaccineResult
        Get
            Return _udtDHVaccineResult
        End Get
        Set(ByVal value As DHVaccineResult)
            _udtDHVaccineResult = value
        End Set
    End Property

#End Region

    Public Sub Build(ByVal udtEHSAccount As EHSAccountModel, ByVal udtCachedHAVaccineResult As HAVaccineResult, ByVal udtCachedDHVaccineResult As DHVaccineResult, ByVal udtAuditLogEntry As AuditLogEntry)

        BuildEHSAccount(udtEHSAccount)

        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim htRecordSummary As Hashtable = Nothing

        Dim udtVaccinationBLL As New VaccinationBLL
        If _udtHAVaccineResult Is Nothing Then _udtHAVaccineResult = New HAVaccineResult(Common.WebService.Interface.HAVaccineResult.enumReturnCode.Error)
        If _udtDHVaccineResult Is Nothing Then _udtDHVaccineResult = New DHVaccineResult(Common.WebService.Interface.DHVaccineResult.enumReturnCode.UnexpectedError)

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = _udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = _udtHAVaccineResult

        Dim udtVaccineResultBagSession As New VaccineResultCollection
        udtVaccineResultBagSession.DHVaccineResult = udtCachedDHVaccineResult
        udtVaccineResultBagSession.HAVaccineResult = udtCachedHAVaccineResult

        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, udtAuditLogEntry, String.Empty, udtVaccineResultBagSession)

        _udtDHVaccineResult = udtVaccineResultBag.DHVaccineResult
        _udtHAVaccineResult = udtVaccineResultBag.HAVaccineResult

        ' Build record summary
        BuildSystemMessage(udtVaccineResultBag)

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Build record summary
        BuildRecordSummary(htRecordSummary, udtVaccineResultBag)
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        If Not IsNothing(udtTranDetailVaccineList) Then
            ' Vaccination record found
            Dim dtVaccineRecord As DataTable = TransactionDetailListToDataTable(udtTranDetailVaccineList)

            Session(SESS.TranDetailList) = dtVaccineRecord

            gvVaccinationRecord.PageIndex = 0

            ' Sort by ServiceReceiveDtm DESC
            ViewState("SortExpression_" + gvVaccinationRecord.ID) = "ServiceReceiveDtm"
            ViewState("SortDirection_" + gvVaccinationRecord.ID) = "DESC"

            gvVaccinationRecord.DataSource = dtVaccineRecord
            gvVaccinationRecord.DataBind()

        End If

        If gvVaccinationRecord.Rows.Count <> 0 Then
            gvVaccinationRecord.Visible = True
            lblNoVaccinationRecord.Visible = False
        Else
            gvVaccinationRecord.Visible = False
            lblNoVaccinationRecord.Visible = True
        End If

        ' Save the external status to session
        Dim udtSessionHandler As New SessionHandler
        Dim udtHAVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtVaccineResultBag.HAVaccineResult, udtEHSAccount.SearchDocCode)
        Dim udtDHVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtVaccineResultBag.DHVaccineResult, udtEHSAccount.SearchDocCode)

        udtSessionHandler.ExtRefStatusSaveToSession(udtHAVaccineRefStatus)
        udtSessionHandler.DHExtRefStatusSaveToSession(udtDHVaccineRefStatus)

    End Sub



    Public Sub Build(ByVal udtEHSAccount As EHSAccountModel, ByVal udtAuditLogEntry As AuditLogEntry)
        'Build(udtEHSAccount, Nothing, udtAuditLogEntry)
        Build(udtEHSAccount, Nothing, Nothing, udtAuditLogEntry)
    End Sub

    Public Sub BuildEHSAccount(ByVal udtEHSAccount As EHSAccountModel)
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        'Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)
        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]

        If udtEHSPersonalInfo.ENameSurName = String.Empty Then
            mvDocumentType.ActiveViewIndex = ViewIndex.SimpleNoName

            ' Recipient Information
            lblAccountInformation.Text = Me.GetGlobalResourceObject("Text", "RecipientInformation")

            Dim strLanguage As String = Session("language")

            Dim udtDocType As DocTypeModel = (New DocTypeBLL).getAllDocType.Filter(udtEHSPersonalInfo.DocCode)

            ' Document Type
            lblSNDocumentType.Text = udtDocType.DocName(strLanguage)
            lblSNDocumentNoText.Text = udtDocType.DocIdentityDesc(strLanguage)

            ' Document No.
            Dim udtFormatter As New Formatter
            lblSNDocumentNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, False, udtEHSPersonalInfo.AdoptionPrefixNum)

            ' Date of Birth
            lblSNDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Session("language"), _
                                                    udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)

        ElseIf udtEHSAccount.VoucherAccID = String.Empty OrElse udtEHSAccount.VoucherAccID = "New" Then
            mvDocumentType.ActiveViewIndex = ViewIndex.Simple

            ' Recipient Information
            lblAccountInformation.Text = Me.GetGlobalResourceObject("Text", "RecipientInformation")

            Dim strLanguage As String = Session("language")

            Dim udtDocType As DocTypeModel = (New DocTypeBLL).getAllDocType.Filter(udtEHSPersonalInfo.DocCode)

            ' Document Type
            lblSDocumentType.Text = udtDocType.DocName(strLanguage)
            lblSDocumentNoText.Text = udtDocType.DocIdentityDesc(strLanguage)

            ' Name
            Dim udtFormatter As New Formatter
            If Not IsNothing(udtEHSPersonalInfo.ENameSurName) Then
                lblSEName.Text = udtFormatter.formatEnglishName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
            End If
            lblSCName.Text = udtFormatter.formatChineseName(udtEHSPersonalInfo.CName)

            ' Document No.
            lblSDocumentNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, False, udtEHSPersonalInfo.AdoptionPrefixNum)

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            If udtEHSPersonalInfo.DocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                If udtEHSPersonalInfo.ECSerialNoNotProvided = True Then
                    lblSECSerialNo.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
                Else
                    lblSECSerialNo.Text = udtEHSPersonalInfo.ECSerialNo
                End If
                lblSECSerialNoText.Visible = True
                lblSECSerialNo.Visible = True
            Else
                lblSECSerialNoText.Visible = False
                lblSECSerialNo.Visible = False
            End If
            ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

            ' Date of Birth
            lblSDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Session("language"), _
                                                    udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration)

            ' In word (HKBC and ADOPC)
            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKBC OrElse udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ADOPC Then
                If udtEHSPersonalInfo.ExactDOB = "T" OrElse udtEHSPersonalInfo.ExactDOB = "U" OrElse udtEHSPersonalInfo.ExactDOB = "V" Then
                    Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("DOBInWordType", udtEHSPersonalInfo.OtherInfo)

                    If strLanguage = CultureLanguage.TradChinese Then
                        lblSDOB.Text = udtStaticDataModel.DataValueChi.ToString.Trim + " " + lblSDOB.Text
                    ElseIf strLanguage = CultureLanguage.SimpChinese Then
                        lblSDOB.Text = udtStaticDataModel.DataValueCN.ToString.Trim + " " + lblSDOB.Text
                    Else
                        lblSDOB.Text = udtStaticDataModel.DataValue.ToString.Trim + " " + lblSDOB.Text
                    End If
                End If
            End If

            ' Gender
            If Not IsNothing(udtEHSPersonalInfo.Gender) Then
                lblSGender.Text = Me.GetGlobalResourceObject("Text", IIf(udtEHSPersonalInfo.Gender = "M", "GenderMale", "GenderFemale"))
            End If

        Else
            mvDocumentType.ActiveViewIndex = ViewIndex.Control

            ' Account Information
            lblAccountInformation.Text = Me.GetGlobalResourceObject("Text", "AccountInfo")

            udcReadOnlyDocumnetType.Clear()

            udcReadOnlyDocumnetType.DocumentType = udtEHSPersonalInfo.DocCode
            udcReadOnlyDocumnetType.EHSAccount = udtEHSAccount
            udcReadOnlyDocumnetType.Vertical = False
            udcReadOnlyDocumnetType.ShowAccountRefNo = False
            udcReadOnlyDocumnetType.ShowTempAccountNotice = False
            udcReadOnlyDocumnetType.ShowAccountCreationDate = False
            udcReadOnlyDocumnetType.TableTitleWidth = 160
            udcReadOnlyDocumnetType.Built()

        End If

    End Sub

    Public Sub RebuildVaccinationRecordGrid()
        gvVaccinationRecord.DataSource = Session(SESS.TranDetailList)
        gvVaccinationRecord.DataBind()

        If gvVaccinationRecord.Rows.Count <> 0 Then
            gvVaccinationRecord.Visible = True
            lblNoVaccinationRecord.Visible = False
        Else
            gvVaccinationRecord.Visible = False
            lblNoVaccinationRecord.Visible = True
        End If

        ' Record Summary
        lblNoOfRecord.Text = Me.GetGlobalResourceObject("Text", "NoOfRecord")
        If lblEHSText.Visible Then lblEHSText.Text = Me.GetGlobalResourceObject("Text", "eHealthSystem")
        If lblHAText.Visible Then lblHAText.Text = Me.GetGlobalResourceObject("Text", "HospitalAuthority")
        If lblHA.Visible Then
            If hfHA.Value = VaccinationBLL.RecordSummaryHAResult.ConnectionFail Then
                lblHA.Text = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryHAResult.ConnectionFail)

            ElseIf hfHA.Value = VaccinationBLL.RecordSummaryHAResult.DemographicsNotMatch Then
                lblHA.Text = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryHAResult.DemographicsNotMatch)

            End If
        End If
        If lblDHText.Visible Then lblDHText.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")
        If lblDH.Visible Then
            If hfDH.Value = VaccinationBLL.RecordSummaryDHResult.ConnectionFail Then
                lblDH.Text = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryDHResult.ConnectionFail)

            ElseIf hfDH.Value = VaccinationBLL.RecordSummaryDHResult.DemographicsNotMatch Then
                lblDH.Text = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryDHResult.DemographicsNotMatch)

            End If

            ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If hfDH.Value.Contains(",PartialRecordReturned") Then
                Dim strDHValue As String = Split(hfDH.Value, ",")(0)
                lblDH.Text = String.Format("{0} ({1})", strDHValue, Me.GetGlobalResourceObject("Text", "PartialRecordReturned"))

            End If
            ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        End If

    End Sub

    ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub BuildRecordSummary(ByVal htRecordSummary As Hashtable, ByVal udtVaccineResultBag As VaccineResultCollection)
        ' Init
        lblEHSText.Visible = False
        lblEHS.Visible = False
        '----------------
        panHA.Visible = False
        lblHAText.Visible = False
        lblHA.Visible = False
        '----------------
        panDH.Visible = False
        lblDHText.Visible = False
        lblDH.Visible = False

        ' No. of records:
        lblNoOfRecord.Text = Me.GetGlobalResourceObject("Text", "NoOfRecord")

        ' eHealth System
        If htRecordSummary.Contains(VaccinationBLL.VaccineRecordProvider.EHS) Then
            lblEHSText.Visible = True
            lblEHSText.Text = Me.GetGlobalResourceObject("Text", "eHealthSystem")

            lblEHS.Visible = True
            lblEHS.Text = htRecordSummary(VaccinationBLL.VaccineRecordProvider.EHS)

        End If

        ' Hospital Authority
        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If htRecordSummary.Contains(VaccinationBLL.VaccineRecordProvider.HA) Then
                panHA.Visible = True
                lblHAText.Visible = True
                lblHAText.Text = Me.GetGlobalResourceObject("Text", "HospitalAuthority")

                Dim strHAValue As String = htRecordSummary(VaccinationBLL.VaccineRecordProvider.HA)
                hfHA.Value = strHAValue

                If strHAValue = VaccinationBLL.RecordSummaryHAResult.ConnectionFail Then
                    strHAValue = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryHAResult.ConnectionFail)
                    lblHA.Style("color") = "red"

                ElseIf strHAValue = VaccinationBLL.RecordSummaryHAResult.DemographicsNotMatch Then
                    strHAValue = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryHAResult.DemographicsNotMatch)
                    lblHA.Style("color") = "blue"

                Else
                    lblHA.Style.Remove("color")

                End If

                lblHA.Visible = True
                lblHA.Text = strHAValue

            End If

        End If

        ' Department of Health
        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If htRecordSummary.Contains(VaccinationBLL.VaccineRecordProvider.DH) Then
                panDH.Visible = True
                lblDHText.Visible = True
                lblDHText.Text = Me.GetGlobalResourceObject("Text", "DepartmentOfHealth")

                Dim strDHValue As String = htRecordSummary(VaccinationBLL.VaccineRecordProvider.DH)
                hfDH.Value = strDHValue

                lblDH.Style.Remove("color")

                If strDHValue = VaccinationBLL.RecordSummaryDHResult.ConnectionFail Then
                    strDHValue = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryDHResult.ConnectionFail)
                    lblDH.Style("color") = "red"

                ElseIf strDHValue = VaccinationBLL.RecordSummaryDHResult.DemographicsNotMatch Then
                    strDHValue = Me.GetGlobalResourceObject("Text", VaccinationBLL.RecordSummaryDHResult.DemographicsNotMatch)
                    lblDH.Style("color") = "blue"

                End If

                If udtVaccineResultBag.DHVaccineResult.SingleClient.ReturnClientCIMSCode = DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord Then
                    'Override hidden value: 3 -> 3,PartialRecordReturned
                    hfDH.Value = String.Format("{0},{1}", strDHValue, "PartialRecordReturned")
                    strDHValue = String.Format("{0} ({1})", strDHValue, Me.GetGlobalResourceObject("Text", "PartialRecordReturned"))
                    lblDH.Style("color") = "blue"

                End If

                lblDH.Visible = True
                lblDH.Text = strDHValue

            End If

        End If

    End Sub
    ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

    ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub BuildSystemMessage(ByVal udtVaccineResultBag As VaccineResultCollection)
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtSystemMessageList As New List(Of SystemMessage)
        Dim blnHAError As Boolean = False
        Dim blnHANotMatch As Boolean = False
        Dim blnDHError As Boolean = False
        Dim blnDHNotMatch As Boolean = False

        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.HAReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00026))
                    blnHANotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00254))
                    blnHAError = True
            End Select
        End If

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.DHReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
                    If udtVaccineResultBag.DHVaccineResult.SingleClient.ReturnClientCIMSCode = DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord Then
                        udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00048))
                    End If
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00041))
                    blnDHNotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00409))
                    blnDHError = True
            End Select
        End If

        If blnHANotMatch And blnDHNotMatch Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00042))
        End If

        If blnHAError And blnDHError Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00410))
        End If

        For Each udtSystemMessage In udtSystemMessageList
            If Not udtSystemMessage Is Nothing Then
                Select Case udtSystemMessage.SeverityCode
                    Case SeverityCode.SEVI
                        udcInfoMessageBox.AddMessage(udtSystemMessage)
                        udcInfoMessageBox.BuildMessageBox()
                    Case SeverityCode.SEVE
                        udcMessageBox.AddMessage(udtSystemMessage)
                        udcMessageBox.BuildMessageBox("ConnectionFail")
                    Case Else
                        'Not to show MessageBox
                End Select
            End If
        Next
    End Sub
    ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

    Private Function TransactionDetailListToDataTable(ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection) As DataTable
        Dim dtVaccineRecord As New DataTable

        ' Columns
        With dtVaccineRecord.Columns
            .Add("ServiceReceiveDtm", GetType(Date))
            .Add("SubsidizeDesc", GetType(String))
            .Add("SubsidizeDescChi", GetType(String))
            .Add("AvailableItemDesc", GetType(String))
            .Add("AvailableItemDescChi", GetType(String))
            .Add("Provider", GetType(String))
            .Add("Remark", GetType(String))
        End With

        ' Convert each TransactionDetailModel to datarow
        For Each udtTranDetailVaccine As TransactionDetailVaccineModel In udtTranDetailVaccineList
            Dim drVaccineRecord As DataRow = dtVaccineRecord.NewRow

            drVaccineRecord("ServiceReceiveDtm") = udtTranDetailVaccine.ServiceReceiveDtm
            drVaccineRecord("SubsidizeDesc") = udtTranDetailVaccine.SubsidizeDesc
            drVaccineRecord("SubsidizeDescChi") = udtTranDetailVaccine.SubsidizeDescChi
            drVaccineRecord("AvailableItemDesc") = udtTranDetailVaccine.AvailableItemDesc
            drVaccineRecord("AvailableItemDescChi") = udtTranDetailVaccine.AvailableItemDescChi
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If udtTranDetailVaccine.SchemeCode = Common.Component.Scheme.SchemeClaimModel.RVP Then
                drVaccineRecord("Provider") = TransactionDetailVaccineModel.ProviderClass.RVP
            Else
                drVaccineRecord("Provider") = udtTranDetailVaccine.Provider
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
            drVaccineRecord("Remark") = udtTranDetailVaccine.RecordType

            dtVaccineRecord.Rows.Add(drVaccineRecord)
        Next

        ' Sort the datatable
        Dim dtResult As DataTable = dtVaccineRecord.Clone

        For Each dr As DataRow In dtVaccineRecord.Select(String.Empty, "ServiceReceiveDtm DESC")
            dtResult.ImportRow(dr)
        Next

        Return dtResult

    End Function

    '

    Protected Sub gvVaccinationRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim udtFormatter As New Formatter
            Dim blnIsChinese As Boolean = (LCase(Session("language")) = CultureLanguage.TradChinese)

            ' Injection Date
            Dim lblGInjectionDate As Label = e.Row.FindControl("lblGInjectionDate")
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'If blnIsChinese Then
            '    lblGInjectionDate.Text = udtFormatter.formatDate(lblGInjectionDate.Text.Trim, CultureLanguage.TradChinese)
            'Else
            '    lblGInjectionDate.Text = udtFormatter.formatDate(lblGInjectionDate.Text.Trim, CultureLanguage.English)
            'End If
            If blnIsChinese Then
                lblGInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblGInjectionDate.Text.Trim), CultureLanguage.TradChinese)
            Else
                lblGInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblGInjectionDate.Text.Trim), CultureLanguage.English)
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' Vaccination
            Dim lblGVaccination As Label = e.Row.FindControl("lblGVaccination")
            Dim lblGVaccinationChi As Label = e.Row.FindControl("lblGVaccinationChi")

            lblGVaccination.Visible = Not blnIsChinese
            lblGVaccinationChi.Visible = blnIsChinese

            ' Dose
            Dim lblGDose As Label = e.Row.FindControl("lblGDose")
            Dim lblGDoseChi As Label = e.Row.FindControl("lblGDoseChi")

            lblGDose.Visible = Not blnIsChinese
            lblGDoseChi.Visible = blnIsChinese

            ' Information Provider
            Dim lblGProvider As Label = e.Row.FindControl("lblGProvider")
            If blnIsChinese Then
                Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, String.Empty, lblGProvider.Text)
            Else
                Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, lblGProvider.Text, String.Empty)
            End If

            ' Remarks
            Dim lblGRemark As Label = e.Row.FindControl("lblGRemark")
            If blnIsChinese Then
                Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.RecordTypeClass.ClassCode, lblGRemark.Text.Trim, String.Empty, lblGRemark.Text)
            Else
                Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.RecordTypeClass.ClassCode, lblGRemark.Text.Trim, lblGRemark.Text, String.Empty)
            End If

        End If
    End Sub

    Protected Sub gvVaccinationRecord_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Dim strDataSource As String = SESS.TranDetailList
        Dim gvSort As GridView = CType(sender, GridView)
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))
        ViewState("SortDirection_" & gvSort.ID) = gvFunction.GridViewSortDirection
        ViewState("SortExpression_" & gvSort.ID) = gvFunction.GridViewSortExpression

        gvSort.DataSource = gvFunction.SortDataTable(Session(strDataSource), True)
        gvSort.PageIndex = e.NewPageIndex
        gvSort.DataBind()

    End Sub

    Protected Sub gvVaccinationRecord_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strDataSource As String = SESS.TranDetailList
        Dim gvSort As GridView = CType(sender, GridView)
        SetSortImg(gvSort)
        SetPageInfo(gvSort, strDataSource)

    End Sub

    Protected Sub gvVaccinationRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Dim strDataSource As String = SESS.TranDetailList
        Dim gvSort As GridView = CType(sender, GridView)
        Dim gvFunction As Common.ComFunction.GridviewFunction = New Common.ComFunction.GridviewFunction(ViewState("SortDirection_" & gvSort.ID), ViewState("SortExpression_" & gvSort.ID))

        gvFunction.GridViewSortExpression = e.SortExpression
        If ViewState("SortExpression_" & gvSort.ID) <> e.SortExpression Then
            If ViewState("SortDirection_" & gvSort.ID) = "ASC" Then
                gvFunction.GridViewSortDirection = "DESC"
            Else
                gvFunction.GridViewSortDirection = "ASC"
            End If
        End If

        Dim pageIndex As Integer = gvSort.PageIndex

        Dim dv As DataView = CType(gvFunction.SortDataTable(Session(strDataSource), False), DataView)
        ViewState("SortDirection_" & gvSort.ID) = gvFunction.GridViewSortDirection
        ViewState("SortExpression_" & gvSort.ID) = gvFunction.GridViewSortExpression

        gvSort.DataSource = dv
        gvSort.DataBind()
        gvSort.PageIndex = pageIndex

    End Sub

    '

    Private Sub SetSortImg(ByRef gvSort As GridView)
        Dim strImgArrowUp As String = "~/Images/others/arrowup.png"
        Dim strImgArrowDown As String = "~/Images/others/arrowdown.png"
        Dim strImgArrowBlank As String = "~/Images/others/arrowblank.png"

        If gvSort.Rows.Count > 0 Then

            Dim gvrHeaderRow As GridViewRow
            gvrHeaderRow = gvSort.HeaderRow

            Dim cell As TableCell
            For Each cell In gvrHeaderRow.Cells
                If cell.HasControls Then
                    If TypeOf cell.Controls(0) Is LinkButton Then

                        Dim lbtnHeader As LinkButton = CType(cell.Controls(0), LinkButton)

                        If Not lbtnHeader Is Nothing Then

                            Dim imgHeader As New Image
                            Dim lblHeader As New Label
                            lblHeader.Text = "<br>"
                            imgHeader.ImageUrl = strImgArrowBlank

                            If ViewState("SortExpression_" & gvSort.ID) = lbtnHeader.CommandArgument Then
                                If ViewState("SortDirection_" & gvSort.ID) = "ASC" Then
                                    imgHeader.ImageUrl = strImgArrowUp
                                ElseIf ViewState("SortDirection_" & gvSort.ID) = "DESC" Then
                                    imgHeader.ImageUrl = strImgArrowDown
                                End If
                            End If

                            cell.Controls.Add(lblHeader)
                            cell.Controls.Add(imgHeader)
                        End If
                    Else
                        If cell.Controls.Count > 1 Then
                            If TypeOf cell.Controls(1) Is CheckBox Then

                                Dim imgHeader As New Image
                                imgHeader.ImageUrl = strImgArrowBlank
                                cell.Controls.Add(imgHeader)

                            End If
                        End If
                    End If
                Else
                    If Not cell.Text.Trim.Equals(String.Empty) Then
                        Dim lblHeader As New Label
                        lblHeader.Text = cell.Text

                        Dim lblHeaderBR As New Label
                        lblHeaderBR.Text = "<br>"

                        Dim imgHeader As New Image
                        imgHeader.ImageUrl = strImgArrowBlank

                        cell.Controls.Add(lblHeader)
                        cell.Controls.Add(lblHeaderBR)
                        cell.Controls.Add(imgHeader)
                    End If
                End If

            Next
        End If
    End Sub

    Private Sub SetPageInfo(ByRef gvSort As GridView, ByVal strDataSource As String)
        If gvSort.Rows.Count > 0 Then

            Dim lblPageInfo As New Label
            Dim dt As DataTable
            dt = CType(Session(strDataSource), DataTable)
            Dim intPageIndex As Integer
            intPageIndex = gvSort.PageIndex + 1

            Dim strPageInfo As String

            strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

            strPageInfo = strPageInfo.Replace("%d", CStr(intPageIndex))
            strPageInfo = strPageInfo.Replace("%e", CStr(gvSort.PageCount))
            strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))
            lblPageInfo.Text = strPageInfo

            Dim grv As GridViewRow = gvSort.BottomPagerRow
            grv.Visible = True
            Dim i As Integer
            i = grv.Cells(0).Controls.Count - 1

            Dim tc As TableCell
            Dim tr As TableRow
            tr = CType(grv.Cells(0).Controls(0).Controls(0), TableRow)

            If gvSort.PageCount = 1 Then
                tc = tr.Cells(0)

                Dim lblPage As Label
                lblPage = CType(tc.Controls(0), Label)
                lblPage.Visible = False

                tc.Controls.Add(lblPageInfo)
                tr.Cells.Add(tc)

            Else
                tc = New TableCell
                tc.Width = Unit.Pixel(20)
                tr.Cells.Add(tc)

                tc = New TableCell
                tc.Controls.Add(lblPageInfo)
                tr.Cells.Add(tc)
            End If
        End If
    End Sub

    ' INT12-011 Vaccination record enquiry viewstate fix [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Public Sub Clear()
        Me.udcReadOnlyDocumnetType.Clear()
    End Sub
    ' INT12-011 Vaccination record enquiry viewstate fix [End][Koala]

End Class