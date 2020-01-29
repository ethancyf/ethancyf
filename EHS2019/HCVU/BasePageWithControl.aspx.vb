Imports Common.ComObject
Imports Common.Component
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports HCVU.Component.FunctionInformation

Partial Public MustInherit Class BasePageWithControl
    Inherits BasePageWithGridView

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Audit Log Description"
    Public Class SF_AuditLogDescription
        Public Const SearchStart_ID As String = LogID.LOG00001
        Public Const SearchStart As String = "Search start"

        Public Const SearchSuccess_ID As String = LogID.LOG00002
        Public Const SearchSuccess As String = "Search success"

        Public Const SearchSuccess_NoRecord_ID As String = LogID.LOG00003
        Public Const SearchSuccess_NoRecord As String = "Search success - No record"

        Public Const SearchFail_ID As String = LogID.LOG00004
        Public Const SearchFail As String = "Search fail"

        Public Const SearchFail_ValidateFail_ID As String = LogID.LOG00005
        Public Const SearchFail_ValidateFail As String = "Search fail - Validate fail"

        Public Const SearchFail_Over1stLimit_ID As String = LogID.LOG00006
        Public Const SearchFail_Over1stLimit As String = "Search fail - Over 1st limit"

        Public Const SearchFail_OverOverrideLimit_ID As String = LogID.LOG00007
        Public Const SearchFail_OverOverrideLimit As String = "Search fail - Over override limit"
    End Class
#End Region

#Region "Enum"
    Protected Enum SearchResultEnum
        Success = 0
        ValidationFail = 1
        NoRecordFound = 2
        OverResultList1stLimit_PopUp = 3
        OverResultList1stLimit_Alert = 4
        OverResultListOverrideLimit = 5
    End Enum
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#Region "Constant"
    Private Const TokenPopup_IsShow As String = "TokenPopup_IsShow"
    Private Const Tokenpopup_FunctionCode As String = "TokenPopup_FunctionCode"
#End Region

#Region "Fields"
    Dim lstTokenPopup As List(Of Control)
    Dim udtFuncInfoBLL As New FunctionInformationBLL()

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Dim _udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(BASE_PAGE_FUNCT_CODE)
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BuildTokenModalPopupExtender(CType(ViewState(Tokenpopup_FunctionCode), String), CType(ViewState(TokenPopup_IsShow), Boolean))
    End Sub

    Public Sub BuildTokenModalPopupExtender(ByVal strFuncCode As String, ByVal blnIsShow As Boolean)
        Dim panToken As New Panel()
        Dim udcPUInputToken As ucInputToken = LoadControl("~/UIControl/Token/ucInputToken.ascx")
        Dim hdDummy As New HiddenField()
        Dim ModalPopupToken As New AjaxControlToolkit.ModalPopupExtender()
        Dim myMasterPage As MasterPage = Me.Master

        ViewState(Tokenpopup_FunctionCode) = strFuncCode

        If (blnIsShow) Then
            hdDummy.ID = "hdDummy"

            panToken.ID = "panToken"
            panToken.BackColor = Drawing.Color.White
            panToken.Controls.Add(udcPUInputToken)

            AddHandler udcPUInputToken.Confirm_Click, AddressOf ibtnPopupConfirm_Click
            AddHandler udcPUInputToken.Cancel_Click, AddressOf ibtnPopupCancel_Click
            udcPUInputToken.ID = "udcPUInputToken"
            udcPUInputToken.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmEnquiryHeader")
            udcPUInputToken.Message = Me.GetGlobalResourceObject("Text", "ConfirmEnquiryMessage")
            udcPUInputToken.Build(Me.Page)

            myMasterPage.UpdatePanelTemplate.ContentTemplateContainer.Controls.Add(hdDummy)
            myMasterPage.UpdatePanelTemplate.ContentTemplateContainer.Controls.Add(panToken)
            myMasterPage.UpdatePanelTemplate.ContentTemplateContainer.Controls.Add(ModalPopupToken)

            ModalPopupToken.ID = "ModalPopupToken"
            ModalPopupToken.BackgroundCssClass = "modalBackgroundTransparent"
            ModalPopupToken.TargetControlID = hdDummy.ID
            ModalPopupToken.PopupControlID = panToken.ID
            ModalPopupToken.PopupDragHandleControlID = ""
            ModalPopupToken.RepositionMode = AjaxControlToolkit.ModalPopupRepositionMode.None
            ModalPopupToken.PopupDragHandleControlID = udcPUInputToken.Header.ClientID

            lstTokenPopup = New List(Of Control)
            lstTokenPopup.Add(hdDummy)
            lstTokenPopup.Add(panToken)
            lstTokenPopup.Add(ModalPopupToken)

            ViewState(TokenPopup_IsShow) = True

            SetControls(lstTokenPopup, CType(ViewState(TokenPopup_IsShow), Boolean))
            ModalPopupToken.Show()
            'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'udcPUInputToken.Focus()
            'CRE17-008 (Remind Delist Practice) [End][Chris YIM]

        Else

            ViewState(TokenPopup_IsShow) = False

            SetControls(lstTokenPopup, CType(ViewState(TokenPopup_IsShow), Boolean))

        End If

    End Sub

    Public Function IsPopupShow() As Boolean
        Dim blnRes As Boolean = False

        If ViewState(TokenPopup_IsShow) = True Then
            blnRes = True
        End If

        Return blnRes

    End Function

    Protected Sub ibtnPopupConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ViewState(TokenPopup_IsShow) = False
        SetControls(lstTokenPopup, CType(ViewState(TokenPopup_IsShow), Boolean))

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Me.SF_ConfirmSearch_Click()
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        'Dim udtAuditLog As New AuditLogEntry(CType(ViewState(Tokenpopup_FunctionCode), String), Me)
        'udtAuditLog.WriteLog(AuditLogDesc.MaskIdentityDocumentNoSuccess_ID, AuditLogDesc.MaskIdentityDocumentNoSuccess)
    End Sub

    Protected Sub ibtnPopupCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ViewState(TokenPopup_IsShow) = False
        SetControls(lstTokenPopup, CType(ViewState(TokenPopup_IsShow), Boolean))

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Me.SF_CancelSearch_Click()
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Sub

    Private Sub SetControls(ByVal lstControl As List(Of Control), ByVal blnIsBuild As Boolean)

        Dim intLoopIndex As Integer
        Dim myMasterPage As MasterPage = Me.Master

        If Not lstControl Is Nothing Then

            For intLoopIndex = 0 To lstControl.Count - 1
                If blnIsBuild Then
                    myMasterPage.UpdatePanelTemplate.ContentTemplateContainer.Controls.Add(lstControl(intLoopIndex))
                Else
                    myMasterPage.UpdatePanelTemplate.ContentTemplateContainer.Controls.Remove(lstControl(intLoopIndex))

                End If

            Next

        End If

    End Sub

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Abstract Method"
    ' This method would be triggered once in the beginning of the method - [StartSearchFlow].
    ' All initialization of "Search Flow" should be placed into this method.
    ' 
    ' Input Param:
    '   - [udtAuditLogEntry] : [AuditLogEntry] passed from "Search Flow" for the usage of local [Page]
    Protected MustOverride Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    ' This method would be triggered once only if the validation feature is active of the method - [StartSearchFlow].
    ' All validation of "Search Flow" should be placed into this method.
    '
    ' Input Param:
    '   - [udtAuditLogEntry] : [AuditLogEntry] passed from "Search Flow" for the usage of local [Page]
    '
    ' Output Param:
    '   - [Boolean] : The validation result
    Protected MustOverride Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean

    ' This method would be triggered once if there is no validation feature or the validation is success of the method - [StartSearchFlow].
    ' The actual searching of "Search Flow" should be placed into this method.
    '
    ' Input Param:
    '   - [udtAuditLogEntry] : [AuditLogEntry] passed from "Search Flow" for the usage of local [Page]
    '   - [blnOverrideResultLimit] : To indicate the search result overriding feature from "Search Flow"
    '
    ' Output Param:
    '   - [BaseBLL.BLLSearchResult] : The search result
    Protected MustOverride Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult

    ' This method would be triggered once only if there is no exception from the search method - [SF_Search].
    ' All data binding of search result should be placed into this method.
    '
    ' Input Param:
    '   - [udtAuditLogEntry] : [AuditLogEntry] passed from "Search Flow" for the usage of local [Page]
    '   - [udtBLLSearchResult] : The search result from the search method - [SF_Search]
    '
    ' Output Param:
    '   - [Integer] : The total no. of row of the search result
    Protected MustOverride Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer

    ' This method would be triggered once in the ending of the method - [StartSearchFlow].
    ' All finalization of "Search Flow" should be placed into this method.
    ' 
    ' Input Param:
    '   - [udtAuditLogEntry] : [AuditLogEntry] passed from "Search Flow" for the usage of local [Page]
    Protected MustOverride Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    ' This method would be triggered once when the "Confirm" Button has been clicked from the "Pop-up Window for search result overriding feature".
    Protected MustOverride Sub SF_ConfirmSearch_Click()

    ' This method would be triggered once when the "Cancel" Button has been clicked from the "Pop-up Window for search result overriding feature".
    Protected MustOverride Sub SF_CancelSearch_Click()
#End Region

#Region "Search Flow"
    ' The sub-class may call this method to trigger "Search Flow" for local [Page].
    '
    ' Input Param:
    '   - [strFunctionCode] : Function Code of function
    '   - [udtAuditLogEntry] : [AuditLogEntry] of local [Page] to pass into each Abstract Methods
    '   - [udcMessageBox] : [MessageBox] of local [Page] to pass into "Search Flow" for alert message display. It can be [Nothing]
    '   - [udcInfoMessageBox] : [InfoMessageBox] of local [Page] to pass into "Search Flow" for information message display. It can be [Nothing]
    '   - [blnNeedValidate] : To turn on / off the validation feature of "Search Flow"
    '   - [blnOverrideResultLimit] : To turn on / off the search result overriding feature of "Search Flow"
    '
    ' Output Param:
    '   - [SearchResultEnum] : The result of "Search Flow"
    '
    ' The Abstract Methods would be triggered in the following sequence:
    '   - [SF_InitSearch]
    '   - [SF_ValidateSearch] (* if validation is active)
    '   - [SF_Search] (* if no validation / validation is success)
    '       - [SF_BindSearchResult] (* if searching has no exception)
    '       - [SF_ConfirmSearch_Click] / [SF_CancelSearch_Click] (* if "Pop-up Windows" has been shown for search result overriding)
    '   - [SF_FinalizeSearch]
    Protected Function StartSearchFlow(ByVal strFunctionCode As String, ByRef udtAuditLogEntry As AuditLogEntry, ByRef udcMessageBox As CustomControls.MessageBox, ByRef udcInfoMessageBox As CustomControls.InfoMessageBox, Optional ByVal blnNeedValidate As Boolean = True, Optional ByVal blnOverrideResultLimit As Boolean = False) As SearchResultEnum
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
        Dim enumSearchResultEnum As SearchResultEnum

        _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
        _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
        _udtAuditLogEntry.WriteStartLog(SF_AuditLogDescription.SearchStart_ID, SF_AuditLogDescription.SearchStart)

        ' Initialization
        Me.SF_InitSearch(udtAuditLogEntry)

        ' Validation
        If blnNeedValidate Then
            If Not Me.SF_ValidateSearch(udtAuditLogEntry) Then
                _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
                _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
                _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchFail_ValidateFail_ID, SF_AuditLogDescription.SearchFail_ValidateFail)

                ' Finalization
                SF_FinalizeSearch(udtAuditLogEntry)

                Return SearchResultEnum.ValidationFail
            End If
        End If

        ' Search
        Try
            udtBLLSearchResult = Me.SF_Search(udtAuditLogEntry, blnOverrideResultLimit)

        Catch sqlException As SqlException
            _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
            _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
            _udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
            _udtAuditLogEntry.AddDescripton("Message", sqlException.Message)
            _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchFail_ID, SF_AuditLogDescription.SearchFail)
            Throw

        Catch exception As Exception
            _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
            _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
            _udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            _udtAuditLogEntry.AddDescripton("Message", exception.Message)
            _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchFail_ID, SF_AuditLogDescription.SearchFail)
            Throw

        End Try

        ' Handle Search Result
        Select Case udtBLLSearchResult.SqlErrorMessage
            Case BaseBLL.EnumSqlErrorMessage.Normal
                ' Normal Case

                Dim intRowCount As Integer

                intRowCount = Me.SF_BindSearchResult(udtAuditLogEntry, udtBLLSearchResult)

                If intRowCount > 0 Then
                    ' Record Found
                    _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
                    _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
                    _udtAuditLogEntry.AddDescripton("No of record", intRowCount)
                    _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchSuccess_ID, SF_AuditLogDescription.SearchSuccess)
                    enumSearchResultEnum = SearchResultEnum.Success

                ElseIf intRowCount = 0 Then
                    ' No Record Found

                    If Not udcInfoMessageBox Is Nothing Then
                        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                        udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                        udcInfoMessageBox.BuildMessageBox()
                    End If

                    _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
                    _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
                    _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchSuccess_NoRecord_ID, SF_AuditLogDescription.SearchSuccess_NoRecord)
                    enumSearchResultEnum = SearchResultEnum.NoRecordFound

                Else
                    ' Unexpected Error

                    Throw New Exception("Error: Class = [HCVU.BasePageWithControl], Method = [StartSearchFlow], Message = The record count cannot be < 0")

                End If

            Case BaseBLL.EnumSqlErrorMessage.OverResultList1stLimit
                ' Over 1st Result Limit Case

                If udtBLLSearchResult.ResultLimitOverrideEnable Then
                    ' Override 1st Result Limit Allowed

                    _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
                    _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
                    _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchFail_Over1stLimit_ID, SF_AuditLogDescription.SearchFail_Over1stLimit)
                    BuildTokenModalPopupExtender(strFunctionCode, True)

                    enumSearchResultEnum = SearchResultEnum.OverResultList1stLimit_PopUp

                Else
                    ' Override 1st Result Limit Not Allowed

                    _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
                    _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
                    If udcMessageBox Is Nothing Then
                        _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchFail_Over1stLimit_ID, SF_AuditLogDescription.SearchFail_Over1stLimit)
                    Else
                        udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00009))
                        udcMessageBox.BuildMessageBox("SearchFail", _udtAuditLogEntry, SF_AuditLogDescription.SearchFail_Over1stLimit_ID, SF_AuditLogDescription.SearchFail_Over1stLimit)
                    End If

                    enumSearchResultEnum = SearchResultEnum.OverResultList1stLimit_Alert

                End If

            Case BaseBLL.EnumSqlErrorMessage.OverResultListOverrideLimit
                ' Over Override Result Limit Case

                _udtAuditLogEntry.AddDescripton("Function Code", Me.FunctionCode)
                _udtAuditLogEntry.AddDescripton("Override 1st limit", IIf(blnOverrideResultLimit, "Y", "N"))
                If udcMessageBox Is Nothing Then
                    _udtAuditLogEntry.WriteEndLog(SF_AuditLogDescription.SearchFail_OverOverrideLimit_ID, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                Else
                    udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00017))
                    udcMessageBox.BuildMessageBox("SearchFail", _udtAuditLogEntry, SF_AuditLogDescription.SearchFail_OverOverrideLimit_ID, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                End If

                enumSearchResultEnum = SearchResultEnum.OverResultListOverrideLimit

            Case Else
                Throw New Exception("Error: Class = [HCVU.BasePageWithControl], Method = [StartSearchFlow], Message = The type of [EnumSqlErrorMessage] of [Common.Component.BaseBLL] mis-matched")

        End Select

        ' Finalization
        SF_FinalizeSearch(udtAuditLogEntry)

        Return enumSearchResultEnum
    End Function
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

End Class