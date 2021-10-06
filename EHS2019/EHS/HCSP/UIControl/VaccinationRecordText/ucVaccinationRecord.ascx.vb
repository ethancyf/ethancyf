Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.HATransaction
Imports Common.Component.Scheme
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

    Public Sub Build(ByVal udtEHSAccount As EHSAccountModel, _
                          ByVal udtCachedHAVaccineResult As HAVaccineResult, _
                          ByVal udtCachedDHVaccineResult As DHVaccineResult, _
                          ByVal udtAuditLogEntry As AuditLogEntry, _
                          ByVal blnSupportDevice As Boolean, _
                          ByRef dicSystemMessageList As Dictionary(Of Integer, SystemMessage), _
                          ByRef dicFindList As Dictionary(Of Integer, String), _
                          ByRef dicReplaceList As Dictionary(Of Integer, String))

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

        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, _
                                               udtTranDetailVaccineList, _
                                               udtVaccineResultBag, _
                                               htRecordSummary, _
                                               udtAuditLogEntry, _
                                               String.Empty, _
                                               udtVaccineResultBagSession)

        _udtDHVaccineResult = udtVaccineResultBag.DHVaccineResult
        _udtHAVaccineResult = udtVaccineResultBag.HAVaccineResult

        ' Build system message
        BuildSystemMessage(htRecordSummary, udtVaccineResultBag, udtTranDetailVaccineList, dicSystemMessageList, dicFindList, dicReplaceList)

        ' Build record summary
        BuildRecordSummary(htRecordSummary, udtVaccineResultBag)

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

        ' Save the external status to session
        Dim udtSessionHandler As New SessionHandler
        Dim udtHAVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass = Nothing
        Dim udtDHVaccineRefStatus As EHSTransactionModel.ExtRefStatusClass = Nothing

        If (New DocTypeBLL).CheckVaccinationRecordAvailable(udtEHSAccount.SearchDocCode, "HA") Then
            udtHAVaccineRefStatus = New EHSTransactionModel.ExtRefStatusClass(udtVaccineResultBag.HAVaccineResult, udtEHSAccount.SearchDocCode)
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

    End Sub

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

    Private Sub BuildSystemMessage(ByVal htRecordSummary As Hashtable, _
                                   ByVal udtVaccineResultBag As VaccineResultCollection, _
                                   ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                   ByRef dicSystemMessageList As Dictionary(Of Integer, SystemMessage), _
                                   ByRef dicFindList As Dictionary(Of Integer, String), _
                                   ByRef dicReplaceList As Dictionary(Of Integer, String))

        Dim udtSystemMessage As SystemMessage = Nothing
        Dim strFind As String = String.Empty
        Dim strReplace As String = String.Empty

        Dim blnShowInfo As Boolean = False
        Dim blnShowError As Boolean = False

        '1. CMS / CIMS system message
        Build_CMS_CIMS_SystemMessage(htRecordSummary, udtVaccineResultBag, dicSystemMessageList, dicFindList, dicReplaceList)

        '2. COVID19 system message
        Build_COVID19_SystemMessage(udtTranDetailVaccineList, dicSystemMessageList, dicFindList, dicReplaceList)

    End Sub

    Private Sub Build_CMS_CIMS_SystemMessage(ByVal htRecordSummary As Hashtable, _
                                             ByVal udtVaccineResultBag As VaccineResultCollection, _
                                             ByRef dicSystemMessageList As Dictionary(Of Integer, SystemMessage), _
                                             ByRef dicFindList As Dictionary(Of Integer, String), _
                                             ByRef dicReplaceList As Dictionary(Of Integer, String))

        Dim blnHAError As Boolean = False
        Dim blnHANotMatch As Boolean = False
        Dim blnDHError As Boolean = False
        Dim blnDHNotMatch As Boolean = False
        Dim intCount As Integer

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.HAReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    intCount = dicSystemMessageList.Count + 1
                    dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00026))
                    dicFindList.Add(intCount, String.Empty)
                    dicReplaceList.Add(intCount, String.Empty)
                    blnHANotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    intCount = dicSystemMessageList.Count + 1
                    dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00254))
                    dicFindList.Add(intCount, String.Empty)
                    dicReplaceList.Add(intCount, String.Empty)
                    blnHAError = True
            End Select
        End If

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.DHReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
                    If udtVaccineResultBag.DHVaccineResult.SingleClient.ReturnClientCIMSCode = DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord Then
                        If CInt(htRecordSummary(VaccinationBLL.VaccineRecordProvider.DH)) > 0 Then
                            intCount = dicSystemMessageList.Count + 1
                            dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00048))
                            dicFindList.Add(intCount, String.Empty)
                            dicReplaceList.Add(intCount, String.Empty)
                        End If
                    End If
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    intCount = dicSystemMessageList.Count + 1
                    dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00041))
                    dicFindList.Add(intCount, String.Empty)
                    dicReplaceList.Add(intCount, String.Empty)
                    blnDHNotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    intCount = dicSystemMessageList.Count + 1
                    dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00409))
                    dicFindList.Add(intCount, String.Empty)
                    dicReplaceList.Add(intCount, String.Empty)
                    blnDHError = True
            End Select
        End If

        If blnHANotMatch And blnDHNotMatch Then
            dicSystemMessageList.Clear()
            dicFindList.Clear()
            dicReplaceList.Clear()

            intCount = dicSystemMessageList.Count + 1
            dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00042))
            dicFindList.Add(intCount, String.Empty)
            dicReplaceList.Add(intCount, String.Empty)

        End If

        If blnHAError And blnDHError Then
            dicSystemMessageList.Clear()
            dicFindList.Clear()
            dicReplaceList.Clear()

            intCount = dicSystemMessageList.Count + 1
            dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00410))
            dicFindList.Add(intCount, String.Empty)
            dicReplaceList.Add(intCount, String.Empty)

        End If

    End Sub

    Private Sub Build_COVID19_SystemMessage(ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                            ByRef dicSystemMessageList As Dictionary(Of Integer, SystemMessage), _
                                            ByRef dicFindList As Dictionary(Of Integer, String), _
                                            ByRef dicReplaceList As Dictionary(Of Integer, String))

        Dim dtmNow As DateTime = (New GeneralFunction).GetSystemDateTime.Date()

        Dim intCount As Integer

        Dim udtTranDetailVaccineC19List As TransactionDetailVaccineModelCollection = udtTranDetailVaccineList.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

        Dim strInterval As String = (New Common.ComFunction.GeneralFunction).GetSystemParameterParmValue1("COVID19_Received_Warning_Interval")
        Dim intInterval As Integer

        If Not Integer.TryParse(strInterval, intInterval) Then
            Throw New Exception(String.Format("Invalid value({0}) of [COVID19_Received_Warning_Interval] in DB SystemParameters.", strInterval))
        End If

        If udtTranDetailVaccineC19List.Count > 0 Then
            Dim udtTranDetailVaccineC19Latest As TransactionDetailVaccineModel = udtTranDetailVaccineC19List.FilterFindNearestRecord()

            If udtTranDetailVaccineC19Latest IsNot Nothing AndAlso _
                Math.Abs(DateDiff(DateInterval.Day, udtTranDetailVaccineC19Latest.ServiceReceiveDtm, dtmNow)) < intInterval Then

                intCount = dicSystemMessageList.Count + 1

                'If _strFunctionCode = FunctCode.FUNT020801 Then
                '    dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00056))
                'Else
                dicSystemMessageList.Add(intCount, New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00055))
                'End If

                dicFindList.Add(intCount, "%s")
                dicReplaceList.Add(intCount, intInterval.ToString)
            End If

        End If

    End Sub

    Public Sub BuildSystemMessageBox(ByVal dicSystemMessageList As Dictionary(Of Integer, SystemMessage), _
                                     ByVal dicFindList As Dictionary(Of Integer, String), _
                                     ByVal dicReplaceList As Dictionary(Of Integer, String), _
                                     ByRef udcMsgBoxInfo As CustomControls.TextOnlyInfoMessageBox, _
                                     ByRef udcMsgBoxErr As CustomControls.TextOnlyMessageBox)

        Dim udtSystemMessage As SystemMessage = Nothing
        Dim strFind As String = String.Empty
        Dim strReplace As String = String.Empty

        Dim blnShowInfo As Boolean = False
        Dim blnShowError As Boolean = False

        'Apply system message to message box
        For intCount As Integer = 1 To dicSystemMessageList.Count
            udtSystemMessage = dicSystemMessageList.Item(intCount)
            strFind = dicFindList.Item(intCount)
            strReplace = dicReplaceList.Item(intCount)

            If Not udtSystemMessage Is Nothing Then
                Select Case udtSystemMessage.SeverityCode
                    Case SeverityCode.SEVI
                        If strFind <> String.Empty Then
                            udcMsgBoxInfo.AddMessage(udtSystemMessage.FunctionCode, udtSystemMessage.SeverityCode, udtSystemMessage.MessageCode, strFind, strReplace)
                        Else
                            udcMsgBoxInfo.AddMessage(udtSystemMessage)
                        End If

                        blnShowInfo = True

                    Case SeverityCode.SEVE
                        If strFind <> String.Empty Then
                            udcMsgBoxErr.AddMessage(udtSystemMessage.FunctionCode, udtSystemMessage.SeverityCode, udtSystemMessage.MessageCode, strFind, strReplace)
                        Else
                            udcMsgBoxErr.AddMessage(udtSystemMessage)
                        End If

                        blnShowError = True

                    Case Else
                        'Not to show MessageBox
                End Select
            End If
        Next

        If blnShowInfo Then
            udcMsgBoxInfo.BuildMessageBox()
        End If

        If blnShowError Then
            udcMsgBoxErr.BuildMessageBox("ConnectionFail")
        End If

    End Sub

    Private Sub BuildHeader()
        Dim dt As New DataTable
        Dim dr As DataRow = dt.NewRow
        dt.Rows.Add(dr)

        gvVaccinationRecordHeader.DataSource = dt
        gvVaccinationRecordHeader.DataBind()

    End Sub

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