Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.HATransaction
Imports Common.Component.SortedGridviewHeader
Imports Common.Format
Imports Common.WebService.Interface
Imports HCSP.BLL

Partial Public Class ucVaccinationRecord1
    Inherits System.Web.UI.UserControl

#Region "Private Class"

    Private Class SESS
        Public Const TranDetailList As String = "ucVaccinationRecord_TranDetailList"
    End Class

    Private Class ViewIndex
        Public Const HeaderShow As Integer = 0
        Public Const HeaderHide As Integer = 1
    End Class

    Private Const strNA As String = "N/A"

#End Region

    Private _udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult
    Private _udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult
    Public Property HAVaccineResult() As Common.WebService.Interface.HAVaccineResult
        Get
            Return _udtHAVaccineResult
        End Get
        Set(ByVal value As Common.WebService.Interface.HAVaccineResult)
            _udtHAVaccineResult = value
        End Set
    End Property
    Public Property DHVaccineResult() As Common.WebService.Interface.DHVaccineResult
        Get
            Return _udtDHVaccineResult
        End Get
        Set(ByVal value As Common.WebService.Interface.DHVaccineResult)
            _udtDHVaccineResult = value
        End Set
    End Property

    Public Function Build(ByVal udtEHSAccount As EHSAccountModel, _
                          ByVal udtCachedHAVaccineResult As HAVaccineResult, _
                          ByVal udtCachedDHVaccineResult As DHVaccineResult, _
                          ByVal udtAuditLogEntry As AuditLogEntry, _
                          ByVal blnSupportDevice As Boolean) As List(Of SystemMessage)

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

        Dim udtSystemMessageList As List(Of SystemMessage)

        udtSystemMessageList = BuildSystemMessage(udtVaccineResultBag)

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Build record summary
        BuildRecordSummary(htRecordSummary, udtVaccineResultBag)
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        ' Default as no header
        mvVaccinationRecordView.ActiveViewIndex = ViewIndex.HeaderHide

        ' Grid view
        If udtTranDetailVaccineList.Count = 0 Then
            gvVaccinationRecord.Visible = False
            lblNoVaccinationRecord.Visible = True

            btnShowHeader.Visible = False
            btnHideHeader.Visible = False

        Else
            gvVaccinationRecord.Visible = True
            lblNoVaccinationRecord.Visible = False

            Dim dtVaccineRecord As DataTable = TransactionDetailListToDataTable(udtTranDetailVaccineList)

            Session(SESS.TranDetailList) = dtVaccineRecord

            ' Set the page size
            If blnSupportDevice Then
                Dim udtGeneralFunction As New GeneralFunction
                Dim strParm1 As String = String.Empty
                udtGeneralFunction.getSystemParameter("VaccinationRecordTextPageSize", strParm1, String.Empty)
                Dim intPageSize As Integer = CInt(strParm1)

                gvVaccinationRecord.AllowPaging = True
                gvVaccinationRecord.PageIndex = 0
                gvVaccinationRecord.PageSize = intPageSize

            Else
                gvVaccinationRecord.AllowPaging = False

            End If

            gvVaccinationRecord.DataSource = dtVaccineRecord
            gvVaccinationRecord.DataBind()

            SwitchView()

        End If

        ' Total N records
        lblNoOfRecord.Text = Me.GetGlobalResourceObject("Text", "NoOfRecord")

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ' Save the external status to session
        Dim udtSessionHandler As New SessionHandler
        Dim udtHAVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass = Nothing
        Dim udtDHVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass = Nothing

        ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [Start][Koala]
        ' -------------------------------------------------------------------------------------
        If (New DocTypeBLL).CheckVaccinationRecordAvailable(udtEHSAccount.SearchDocCode, "HA") Then
            udtHAVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtVaccineResultBag.HAVaccineResult, udtEHSAccount.SearchDocCode)
            'If (New DocTypeBLL).getAllDocType.Filter(udtEHSAccount.EHSPersonalInformationList(0).DocCode).VaccinationRecordAvailable Then
            '    udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass(_udtHAVaccineResult, udtEHSAccount.EHSPersonalInformationList(0).DocCode)
            ' INT13-0021 - Fix use HKBC on smart IC claim incorrectly [End][Koala]
        Else
            udtHAVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.Yes, _
                                                        EHSTransactionModel.ExtRefStatusClass.ExtSourceMatchEnum.DocumentNotAvailable, _
                                                        EHSTransactionModel.ExtRefStatusClass.RecordReturnEnum.No)
        End If

        If (New DocTypeBLL).CheckVaccinationRecordAvailable(udtEHSAccount.SearchDocCode, "DH") Then
            udtDHVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtVaccineResultBag.DHVaccineResult, udtEHSAccount.SearchDocCode)
        Else
            udtDHVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(EHSTransactionModel.ExtRefStatusClass.ResultShownEnum.Yes, _
                                                        EHSTransactionModel.ExtRefStatusClass.ExtSourceMatchEnum.DocumentNotAvailable, _
                                                        EHSTransactionModel.ExtRefStatusClass.RecordReturnEnum.No)
        End If

        udtSessionHandler.ExtRefStatusSaveToSession(udtHAVaccineRefStatus)
        udtSessionHandler.DHExtRefStatusSaveToSession(udtDHVaccineRefStatus)

        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]





        Return udtSystemMessageList

    End Function

    Public Function Build(ByVal udtEHSAccount As EHSAccountModel, ByVal udtAuditLogEntry As AuditLogEntry, ByVal blnSupportDevice As Boolean) As List(Of SystemMessage)
        'Return Build(udtEHSAccount, Nothing, udtAuditLogEntry, blnSupportDevice)
        Return Build(udtEHSAccount, Nothing, Nothing, udtAuditLogEntry, blnSupportDevice)
    End Function

    Public Sub RebuildVaccinationRecordGrid()
        Dim dtVaccineRecord As DataTable = Session(SESS.TranDetailList)

        gvVaccinationRecord.DataSource = dtVaccineRecord
        gvVaccinationRecord.DataBind()

        ' Build header
        If mvVaccinationRecordView.ActiveViewIndex = ViewIndex.HeaderShow Then BuildHeader()

        ' Total N records
        lblNoOfRecord.Text = Me.GetGlobalResourceObject("Text", "NoOfRecord")

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

        ' Show Header / Hide Header
        btnShowHeader.Text = Me.GetGlobalResourceObject("AlternateText", "ShowHeaderBtn")
        btnHideHeader.Text = Me.GetGlobalResourceObject("AlternateText", "HideHeaderBtn")

    End Sub

    ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub BuildRecordSummary(ByVal htRecordSummary As Hashtable, ByVal udtVaccineResultBag As VaccineResultCollection)
        Dim blnFirstItem As Boolean = True

        ' Init
        lblEHSText.Visible = False
        lblEHS.Visible = False
        lblHAStart.Visible = False

        lblHAOpen.Visible = False
        lblHAText.Visible = False
        lblHAClose.Visible = False
        lblHA.Visible = False

        lblDHStart.Visible = False
        lblDHOpen.Visible = False
        lblDHText.Visible = False
        lblDHClose.Visible = False
        lblDH.Visible = False

        ' No. of records:
        lblNoOfRecord.Text = Me.GetGlobalResourceObject("Text", "NoOfRecord")

        ' eHealth System
        If htRecordSummary.Contains(VaccinationBLL.VaccineRecordProvider.EHS) Then
            blnFirstItem = False

            lblEHSText.Visible = True
            lblEHSText.Text = Me.GetGlobalResourceObject("Text", "eHealthSystem")

            lblEHS.Visible = True
            lblEHS.Text = String.Format("[ {0} ]", htRecordSummary(VaccinationBLL.VaccineRecordProvider.EHS))

        End If

        ' Hospital Authority
        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If htRecordSummary.Contains(VaccinationBLL.VaccineRecordProvider.HA) Then
                ' If not first item
                If Not blnFirstItem Then lblHAStart.Visible = True

                blnFirstItem = False

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

                lblHAOpen.Visible = True
                lblHAClose.Visible = True

            End If

        End If

        ' Department of Health
        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            If htRecordSummary.Contains(VaccinationBLL.VaccineRecordProvider.DH) Then
                ' If not first item
                If Not blnFirstItem Then lblDHStart.Visible = True

                blnFirstItem = False

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

                lblDHOpen.Visible = True
                lblDHClose.Visible = True

            End If

        End If

    End Sub
    ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

    ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function BuildSystemMessage(ByVal udtVaccineResultBag As VaccineResultCollection) As List(Of SystemMessage)
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtSystemMessageList As New List(Of SystemMessage)
        Dim blnHAError As Boolean = False
        Dim blnHANotMatch As Boolean = False
        Dim blnDHError As Boolean = False
        Dim blnDHNotMatch As Boolean = False

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

        Return udtSystemMessageList

    End Function
    ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

    Private Sub BuildHeader()
        Dim dt As New DataTable
        Dim dr As DataRow = dt.NewRow
        dt.Rows.Add(dr)

        gvVaccinationRecordHeader.DataSource = dt
        gvVaccinationRecordHeader.DataBind()

    End Sub

    '

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
            Dim hfGDose As HiddenField = e.Row.FindControl("hfGDose")
            Dim hfGDoseChi As HiddenField = e.Row.FindControl("hfGDoseChi")

            If hfGDose.Value <> strNA Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                If blnIsChinese Then
                    lblGVaccinationChi.Text += String.Format(" [{0}]", AntiXssEncoder.HtmlEncode(hfGDoseChi.Value, True))
                Else
                    lblGVaccination.Text += String.Format(" [{0}]", AntiXssEncoder.HtmlEncode(hfGDose.Value, True))
                End If
                ' I-CRE16-003 Fix XSS [End][Lawrence]

            End If

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

            If lblGRemark.Text = String.Empty Then
                lblGRemark.Visible = False

                Dim td As HtmlTableCell = e.Row.FindControl("tdGProvider")
                td.Attributes.Remove("class")
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

    '

    Protected Sub mvVaccinationRecordView_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SwitchView()

    End Sub

    Private Sub SwitchView()
        If mvVaccinationRecordView.ActiveViewIndex = ViewIndex.HeaderHide Then
            btnHideHeader.Visible = False
            btnShowHeader.Visible = True
            btnShowHeader.Text = Me.GetGlobalResourceObject("AlternateText", "ShowHeaderBtn")

        Else
            btnHideHeader.Visible = True
            btnHideHeader.Text = Me.GetGlobalResourceObject("AlternateText", "HideHeaderBtn")
            btnShowHeader.Visible = False

        End If

    End Sub

    '

    Protected Sub btnShowHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvVaccinationRecordView.ActiveViewIndex = ViewIndex.HeaderShow
        BuildHeader()
    End Sub

    Protected Sub btnHideHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        mvVaccinationRecordView.ActiveViewIndex = ViewIndex.HeaderHide
    End Sub


End Class