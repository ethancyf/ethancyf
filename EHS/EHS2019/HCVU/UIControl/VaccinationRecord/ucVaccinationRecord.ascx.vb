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

Partial Public Class ucVaccinationRecord
    Inherits System.Web.UI.UserControl

#Region "Private Class"

    Private Class SESS
        Public Const TranDetailList As String = "ucVaccinationRecord_TranDetailList"
    End Class

#End Region

#Region "Property"

    Private _udtHAVaccineResult As HAVaccineResult
    Private _udtDHVaccineResult As DHVaccineResult

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
    ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
    Public Sub Build(ByVal udtEHSAccount As EHSAccountModel, ByVal udtAuditLogEntry As AuditLogEntry, Optional blnShowAccountID As Boolean = False)

        BuildEHSAccount(udtEHSAccount, blnShowAccountID)
        ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        'Dim htRecordSummary As Hashtable = Nothing
        Dim htRecordSummary As New Hashtable

        'Dim udtHATranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        'Dim udtDHTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        'Dim htHARecordSummary As Hashtable = Nothing
        'Dim htDHRecordSummary As Hashtable = Nothing

        If _udtHAVaccineResult Is Nothing Then _udtHAVaccineResult = New HAVaccineResult(Common.WebService.Interface.HAVaccineResult.enumReturnCode.Error)
        If _udtDHVaccineResult Is Nothing Then _udtDHVaccineResult = New DHVaccineResult(Common.WebService.Interface.DHVaccineResult.enumReturnCode.UnexpectedError)

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = _udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = _udtHAVaccineResult

        Dim udtVaccinationBLL As New VaccinationBLL
        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, udtAuditLogEntry)

        _udtDHVaccineResult = udtVaccineResultBag.DHVaccineResult
        _udtHAVaccineResult = udtVaccineResultBag.HAVaccineResult

        'Build System Message
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

        ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
        lblVaccRecordBaseOnIDDoc.Visible = blnShowAccountID
        ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

        If gvVaccinationRecord.Rows.Count <> 0 Then
            gvVaccinationRecord.Visible = True
            lblNoVaccinationRecord.Visible = False
        Else
            gvVaccinationRecord.Visible = False
            lblNoVaccinationRecord.Visible = True
        End If

    End Sub

    ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
    Public Sub BuildEHSAccount(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal blnShowAccountID As Boolean = False)
        udcReadOnlyDocumnetType.Clear()

        If Not udtEHSAccount Is Nothing Then
            udcReadOnlyDocumnetType.DocumentType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim()
            udcReadOnlyDocumnetType.EHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList(0)
            udcReadOnlyDocumnetType.Vertical = False
            udcReadOnlyDocumnetType.Width = 160
            udcReadOnlyDocumnetType.EHSAccountModel = udtEHSAccount
            If blnShowAccountID Then
                lblPersonalParticularsText.Text = Me.GetGlobalResourceObject("Text", "VRInformation")
                lblVaccinationRecordText.Text = Me.GetGlobalResourceObject("Text", "PersonalVaccinationRecord")
                udcReadOnlyDocumnetType.ShowAccountIDAsBtn = False
                udcReadOnlyDocumnetType.ShowAccountID = True
            End If
            udcReadOnlyDocumnetType.Build()
        End If

    End Sub
    ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

    Public Sub RebuildVaccinationRecordGrid()
        gvVaccinationRecord.DataSource = Session(SESS.TranDetailList)
        gvVaccinationRecord.DataBind()

        If gvVaccinationRecord.Rows.Count <> 0 Then
            gvVaccinationRecord.Visible = True

        Else
            gvVaccinationRecord.Visible = False

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

        Dim blnFirstItem As Boolean = True

        ' Init
        lblEHSText.Visible = False
        lblEHS.Visible = False

        panHA.Visible = False
        lblHAText.Visible = False
        lblHA.Visible = False

        panDH.Visible = False
        lblDHText.Visible = False
        lblDH.Visible = False

        ' No. of records:
        lblNoOfRecord.Text = Me.GetGlobalResourceObject("Text", "NoOfRecord")

        ' eHealth System
        If htRecordSummary.Contains(VaccinationBLL.VaccineRecordProvider.EHS) Then
            blnFirstItem = False

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

        ' DH
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
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00030))
                    blnHANotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00272))
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
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00043))
                    blnDHNotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00411))
                    blnDHError = True
            End Select
        End If

        If blnHANotMatch And blnDHNotMatch Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00044))
        End If

        If blnHAError And blnDHError Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00412))
        End If

        For Each udtSystemMessage In udtSystemMessageList
            If Not udtSystemMessage Is Nothing Then
                Select Case udtSystemMessage.SeverityCode
                    Case "I"
                        udcInfoMessageBox.AddMessage(udtSystemMessage)
                        udcInfoMessageBox.BuildMessageBox()
                    Case "E"
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
            .Add("AvailableItemDesc", GetType(String))
            .Add("Provider", GetType(String))
            .Add("Location", GetType(String))
            .Add("Remark", GetType(String))
            .Add("RecordCreationDtm", GetType(Date))
            .Add("ExternalReference", GetType(String))
        End With

        ' Convert each TransactionDetailModel to datarow
        For Each udtTranDetailVaccine As TransactionDetailVaccineModel In udtTranDetailVaccineList
            Dim drVaccineRecord As DataRow = dtVaccineRecord.NewRow

            drVaccineRecord("ServiceReceiveDtm") = udtTranDetailVaccine.ServiceReceiveDtm
            drVaccineRecord("SubsidizeDesc") = udtTranDetailVaccine.SubsidizeDesc
            drVaccineRecord("AvailableItemDesc") = udtTranDetailVaccine.AvailableItemDesc
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If udtTranDetailVaccine.SchemeCode = Common.Component.Scheme.SchemeClaimModel.RVP Then
                drVaccineRecord("Provider") = TransactionDetailVaccineModel.ProviderClass.RVP
            Else
                drVaccineRecord("Provider") = udtTranDetailVaccine.Provider
            End If
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]
            drVaccineRecord("Remark") = udtTranDetailVaccine.RecordType
            drVaccineRecord("Location") = udtTranDetailVaccine.PracticeName
            drVaccineRecord("RecordCreationDtm") = udtTranDetailVaccine.TransactionDtm
            drVaccineRecord("ExternalReference") = udtTranDetailVaccine.ExtRefStatus
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

            ' Injection Date
            Dim lblGInjectionDate As Label = e.Row.FindControl("lblGInjectionDate")
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblGInjectionDate.Text = udtFormatter.formatDate(lblGInjectionDate.Text.Trim, CultureLanguage.English)
            lblGInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblGInjectionDate.Text.Trim), CultureLanguage.English)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' Information Provider
            Dim lblGProvider As Label = e.Row.FindControl("lblGProvider")
            Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, lblGProvider.Text, String.Empty)

            ' Remarks
            Dim lblGRemark As Label = e.Row.FindControl("lblGRemark")
            Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.RecordTypeClass.ClassCode, lblGRemark.Text.Trim, lblGRemark.Text, String.Empty)

            ' Record Creation Time
            Dim lblGRecordCreationDtm As Label = e.Row.FindControl("lblGRecordCreationDtm")
            lblGRecordCreationDtm.Text = udtFormatter.formatDateTime(lblGRecordCreationDtm.Text.Trim, CultureLanguage.English)

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

End Class