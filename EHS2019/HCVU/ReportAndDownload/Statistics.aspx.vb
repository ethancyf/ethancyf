Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject
Imports Common.Format
Imports Common.Validation
Imports System
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.FileGeneration
Imports Common.Component.Inbox
Imports Common.Component.FileGeneration.FileGenerationBLL
Imports Common.Component.UserAC
Imports Common.Component.HCVUUser
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Partial Public Class Statistics
    'Inherits System.Web.UI.Page
    Inherits BasePageWithGridView


#Region "Variable"

    Private udcFormater As New Formatter
    Private udtValidator As New Validator
    Private _udtAuditLogEntry As AuditLogEntry

    Private Const _generalInteger = "#"
    Private Const _generalNumber = "#,##0.00_);(#,##0.00)"
    Private Const _date = "d mmm yyyy h:mm"

#End Region

#Region "Enum"

    Public Enum mvStatisticsEnum
        Enquiry = 0
        Criteria = 1
        Result = 2
    End Enum

    Public Enum mvTimePeriodEnum
        OpeningHour = 0
        ClosingHour = 1
    End Enum

#End Region

#Region "Session and constants"

    Private Const FUNCTION_CODE As String = "010703"
    Private Const DynamicColumn_DefaultWidth As Integer = 50
    Private Const DynamicGrid_MinWidth As Integer = 200
    Private Const CUT_OFF_DATE_DATATYPE_ID As String = "dbEVS_Enquiry"

    Private Class SESS
        Public Const ReportList = FUNCTION_CODE + "_ReportList"
        Public Const DynamicGridData = FUNCTION_CODE + "_DynamicGridData"
        Public Const StatisticsCriteriaUC = FUNCTION_CODE + "_StatisticsCriteriaUC"
        Public Const StatisticsResultUC = FUNCTION_CODE + "_StatisticsResultUC"
        Public Const StatisticsResultGenTime = FUNCTION_CODE + "_GenTime"
        Public Const StatisticsCutOffDate = FUNCTION_CODE + "_CutOffDate"
        Public Const ResultSetup = FUNCTION_CODE + "_ResultSetup"
        Public Const ExportSetup = FUNCTION_CODE + "_ExportSetup"
    End Class

    Private Class DynamicGridItem
        Public Const ReportID As String = "ReportID"
        Public Const ColumnName As String = "ColumnName"
        Public Const SortSeq As String = "SortSeq"
        Public Const WidthUnit As String = "WidthUnit"
        Public Const DisplayColumnText As String = "DisplayColumnText"
    End Class

    ' For layout (VS, ActiveTabClass, MessageTabClass)
    Private Class VS
        Public Const ActiveTab As String = "ActiveTab"
        Public Const LastActiveTab As String = "LastActiveTab"
        Public Const ContentMessageID As String = "ContentMessageID"
        Public Const MessageTab As String = "MessageTab"
    End Class

    Private Class ActiveTabClass
        Public Const StatList As String = "StatList"
        Public Const Criteria As String = "StatCriteria"
        Public Const Content As String = "Content"
    End Class

    Private Class MessageTabClass
        Public Const Criteria As String = "StatCriteria"
        Public Const Content As String = "Content"
    End Class

    ' Dynamic grid layout setting field
    Private Class ResultSetup
        Public Const CName As String = "CName"
        Public Const DescResource As String = "DescResource"
        Public Const CWidth As String = "CWidth"
        Public Const ValueFormat As String = "ValueFormat"
    End Class

    ' Excel export layout setting field
    Private Class ExportSetup
        Public Const CName As String = "CName"
        Public Const DescResource As String = "DescResource"
        Public Const CWidth As String = "CWidth"
        Public Const ValueFormat As String = "ValueFormat"
    End Class

#End Region

#Region "Audit Log Description"

    Public Class AuditLogDesc
        Public Const PageLoad_ID As String = LogID.LOG00000
        Public Const PageLoad As String = "Statistic Enquiry Page Loaded"

        Public Const StatEnq_List_PageLoad_Started_ID As String = LogID.LOG00001
        Public Const StatEnq_List_PageLoad_Started As String = "Statistic Enquiry List View Load Started"

        Public Const StatEnq_List_PageLoad_Succeeded_ID As String = LogID.LOG00002
        Public Const StatEnq_List_PageLoad_Succeeded As String = "Statistic Enquiry List View Load Succeeded"

        Public Const StatEnq_List_PageLoad_Failed_ID As String = LogID.LOG00003
        Public Const StatEnq_List_PageLoad_Failed As String = "Statistic Enquiry List Page View Failed"

        Public Const StatEnq_NotAvailable_ID As String = LogID.LOG00004
        Public Const StatEnq_NotAvailable As String = "Statistic Enquiry Not Available"

        Public Const StatEnq_ListItem_Selected_ID As String = LogID.LOG00005
        Public Const StatEnq_ListItem_Selected As String = "Statistic Enquiry List Item Selected"

        Public Const StatEnq_Criteria_PageLoad_Started_ID As String = LogID.LOG00006
        Public Const StatEnq_Criteria_PageLoad_Started As String = "Statistic Enquiry Criteria View Load Started"

        Public Const StatEnq_Criteria_PageLoad_Succeeded_ID As String = LogID.LOG00007
        Public Const StatEnq_Criteria_PageLoad_Succeeded As String = "Statistic Enquiry Criteria View Load Succeeded"

        Public Const StatEnq_Criteria_PageLoad_Failed_ID As String = LogID.LOG00008
        Public Const StatEnq_Criteria_PageLoad_Failed As String = "Statistic Enquiry Criteria View Load Failed"

        Public Const StatEnq_Criteria_Cancel_Clicked_ID As String = LogID.LOG00009
        Public Const StatEnq_Criteria_Cancel_Clicked As String = "Statistic Enquiry Criteria Cancel Clicked"

        Public Const StatEnq_Criteria_Submit_Clicked_ID As String = LogID.LOG00010
        Public Const StatEnq_Criteria_Submit_Clicked As String = "Statistic Enquiry Criteria Submit Clicked"

        Public Const StatResult_PageLoad_Started_ID As String = LogID.LOG00011
        Public Const StatResult_PageLoad_Started As String = "Statistic Result View Load Started"

        Public Const StatResult_PageLoad_Succeeded_ID As String = LogID.LOG00012
        Public Const StatResult_PageLoad_Succeeded As String = "Statistic Result View Load Succeeded"

        Public Const StatResult_PageLoad_Failed_ID As String = LogID.LOG00013
        Public Const StatResult_PageLoad_Failed As String = "Statistic Result View Load Failed"

        'Public Const StatResult_ShowCriteria_Clicked_ID As String = LogID.LOG00014
        'Public Const StatResult_ShowCriteria_Clicked As String = "Statistic Result Show Criteria Clicked"

        'Public Const StatResult_HideCriteria_Clicked_ID As String = LogID.LOG00015
        'Public Const StatResult_HideCriteria_Clicked As String = "Statistic Result Hide Criteria Clicked"

        'Public Const StatResult_OpenLegend_Clicked_ID As String = LogID.LOG00016
        'Public Const StatResult_OpenLegend_Clicked As String = "Statistic Result Open Legend Clicked"

        'Public Const StatResult_CloseLegend_Clicked_ID As String = LogID.LOG00017
        'Public Const StatResult_CloseLegend_Clicked As String = "Statistic Result Close Legend Clicked"

        Public Const StatResult_Back_Clicked_ID As String = LogID.LOG00014
        Public Const StatResult_Back_Clicked As String = "Statistic Result Back Clicked"

        Public Const StatResult_ExportRecord_Clicked_ID As String = LogID.LOG00015
        Public Const StatResult_ExportRecord_Clicked As String = "Statistic Result Export Records(s) Clicked"

        Public Const StatResult_ExportRecord_Succeeded_ID As String = LogID.LOG00016
        Public Const StatResult_ExportRecord_Succeeded As String = "Statistic Result Export Records(s) Succeeded"

        Public Const StatResult_ExportRecord_Failed_ID As String = LogID.LOG00017
        Public Const StatResult_ExportRecord_Failed As String = "Statistic Result Export Records(s) Failed"

        Public Const StatResult_Download_Yes_Clicked_ID As String = LogID.LOG00018
        Public Const StatResult_Download_Yes_Clicked As String = "Statistic Result Download Yes Clicked"

        Public Const StatResult_Download_No_Clicked_ID As String = LogID.LOG00019
        Public Const StatResult_Download_No_Clicked As String = "Statistic Result Download No Clicked"

        Public Const StatSidebar_MenuItem_Clicked_ID As String = LogID.LOG00020
        Public Const StatSidebar_MenuItem_Clicked As String = "Statistic Sidebar Menu Item Clicked"

        Public Const StatSidebar_Header_Clicked_ID As String = LogID.LOG00021
        Public Const StatSidebar_Header_Clicked As String = "Statistic Main Tab Clicked"

        Public Const StatTab_Clicked_ID As String = LogID.LOG00022
        Public Const StatTab_Clicked As String = "Statistic Sub Tab Clicked"

        Public Const StatTab_Close_Clicked_ID As String = LogID.LOG00023
        Public Const StatTab_Close_Clicked As String = "Statistic Sub Tab Close Clicked"

        Public Const Statistics_ID As String = "Statistics ID"
        Public Const Statistics_CutOff_Date As String = "Cutoff Date"
        Public Const Statistics_Enquiry_DateTime As String = "Enquiry DateTime"

        Public Const Statistics_Parameters As String = "Statistics Parameters"

        Public Const Generation_ID As String = "Generation ID"

        Public Const Message_ID As String = "Message ID"

        'Old Audit Log Desc by Nick
        'Public Const StatListLoadFail As String = "Statistic List Loaded Fail"
        'Public Const StatListLoadFail_ID As String = LogID.LOG00004

        'Public Const ViewStatistic As String = "View Statistic"
        'Public Const ViewStatistic_ID As String = LogID.LOG00006

        'Public Const ClickBack As String = "Click Back"
        'Public Const ClickBack_ID As String = LogID.LOG00009

        'Public Const ClickExportRecords As String = "Click Export Record(s)"
        'Public Const ClickExportRecords_ID As String = LogID.LOG00010

        'Public Const ClickCornerTab As String = "Click Corner Tab"
        'Public Const ClickCornerTab_ID As String = LogID.LOG00011

        'Public Const ClickMessageTab As String = "Click Message Tab"
        'Public Const ClickMessageTab_ID As String = LogID.LOG00012

        'Public Const ClickCloseTab As String = "Click Close Tab"
        'Public Const ClickCloseTab_ID As String = LogID.LOG00013

        'Public Const ClickSideBarStatisticEnquiry As String = "Click Sidebar Statistic Enquiry"
        'Public Const ClickSideBarStatisticEnquiry_ID As String = LogID.LOG00014

        'Public Const ExportPopupClickYes As String = "Export Popup - Click Yes"
        'Public Const ExportPopupClickYes_ID As String = LogID.LOG00015

        'Public Const ExportPopupClickNo As String = "Export Popup - Click No"
        'Public Const ExportPopupClickNo_ID As String = LogID.LOG00016
    End Class

#End Region

#Region "Property"

    Public Overrides ReadOnly Property InfoMessageBox() As CustomControls.InfoMessageBox
        Get
            Return Me.udcInfoMessageBox
        End Get
    End Property

    Public Overrides ReadOnly Property MessageBox() As CustomControls.MessageBox
        Get
            Return Me.udcErrorMessage
        End Get
    End Property

#End Region

#Region "Page event"

    Public Sub New()
        MyBase.FunctionCode = FUNCTION_CODE
    End Sub

    Private Sub mvStatistics_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvStatistics.ActiveViewChanged
        '_udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)
        'If Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Enquiry Then
        '    _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatEnq_List_PageLoad_Started_ID, AuditLogDesc.StatEnq_List_PageLoad_Started)
        'End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ' Init audit log object
        _udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)

        ' Hardcode part [Start]
        ' --------------------------------------------------------------
        If Not IsNothing(Session(SESS.StatisticsCriteriaUC)) Then
            Dim udtStatisticsModel As StatisticsModel = CType(Session(SESS.StatisticsCriteriaUC), StatisticsModel)
            ucStatisticsCriteriaBase.Build(udtStatisticsModel.CriteriaSetup)
        End If

        If Not IsNothing(Session(SESS.StatisticsResultUC)) Then
            ucStatisticsResultBase.Build(Session(SESS.StatisticsResultUC))
        End If

        'If Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Enquiry Then
        '    _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatEnq_List_PageLoad_Started_ID, AuditLogDesc.StatEnq_List_PageLoad_Started)
        'End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            _udtAuditLogEntry.WriteLog(AuditLogDesc.PageLoad_ID, AuditLogDesc.PageLoad)

            InitPage()
            InitLayout()
            ' Check Enquiry opening hour (First time check by BasePage)
            'CheckOpeningHour()
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        ' Statistics new layout [Start]
        ' ----------------------------------------------------------------------------
        imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeftDisable")
        tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddleDisable")))
        imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRightDisable")
        imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeftDisable")
        tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddleDisable")))
        imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRightDisable")
        tdTabHeaderInboxL.Attributes("class") = Nothing
        tdTabHeaderInboxM.Attributes("class") = Nothing
        tdTabHeaderInboxR.Attributes("class") = Nothing
        tdTabHeaderContentL.Attributes("class") = Nothing
        tdTabHeaderContentM.Attributes("class") = Nothing
        tdTabHeaderContentR.Attributes("class") = Nothing

        'Disabled hyperlink underline
        lbtnTabHeaderStatList.Attributes("class") = "InboxDisabledUnderline"
        lbtnTabHeaderContent.Attributes("class") = "InboxDisabledUnderline"

        'Disabled message tab close button
        ibtnTabHeaderContentClose.Visible = False

        tdSidebarStatList.Attributes("class") = "SideBar"
        tdSidebarStatList.Attributes.Remove("onmouseover")
        tdSidebarStatList.Attributes.Remove("onmouseout")

        'Disabled hyperlink underline
        lbtnSidebarStatList.Attributes("class") = "InboxDisabledUnderline"

        'Disabled side bar bold font
        lbtnSidebarStatList.Font.Bold = False

        Select Case ViewState(VS.ActiveTab)
            Case ActiveTabClass.StatList
                imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                tdTabHeaderInboxL.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderInboxM.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderInboxR.Attributes("class") = "TabHeaderSelected"
                lbtnTabHeaderStatList.Text = Me.GetGlobalResourceObject("Text", "StatisticEnquiry")

                tdSidebarStatList.Attributes("class") = "SideBarSelected"
                lbtnSidebarStatList.Font.Bold = True

                Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Enquiry

            Case ActiveTabClass.Criteria
                imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                tdTabHeaderContentL.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderContentM.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderContentR.Attributes("class") = "TabHeaderSelected"

                ibtnTabHeaderContentClose.Visible = True

                Select Case ViewState(VS.LastActiveTab)
                    Case ActiveTabClass.StatList
                        tdSidebarStatList.Attributes("class") = "SideBarSelected"
                        lbtnTabHeaderStatList.Text = Me.GetGlobalResourceObject("Text", "StatisticEnquiry")
                        lbtnSidebarStatList.Font.Bold = True

                End Select

                Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Criteria

            Case ActiveTabClass.Content
                imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                tdTabHeaderContentL.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderContentM.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderContentR.Attributes("class") = "TabHeaderSelected"

                ibtnTabHeaderContentClose.Visible = True

                Select Case ViewState(VS.LastActiveTab)
                    Case ActiveTabClass.StatList
                        tdSidebarStatList.Attributes("class") = "SideBarSelected"
                        lbtnTabHeaderStatList.Text = Me.GetGlobalResourceObject("Text", "StatisticEnquiry")
                        lbtnSidebarStatList.Font.Bold = True

                End Select

                Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Result

        End Select

        If ViewState(VS.ContentMessageID) = String.Empty Then
            tdTabHeaderContentL.Visible = False
            tdTabHeaderContentM.Visible = False
            tdTabHeaderContentR.Visible = False

        Else
            tdTabHeaderContentL.Visible = True
            tdTabHeaderContentM.Visible = True
            tdTabHeaderContentR.Visible = True

        End If
        ' Statistics new layout [End]

    End Sub

#End Region

#Region "Common function"

    Private Sub InitPage()
        Session.Remove(SESS.ReportList)
        ClearEnquiryCriteria()
        GetReportListData()
    End Sub

    Private Sub ClearEnquiryCriteria()
        Session.Remove(SESS.DynamicGridData)

        Me.ucStatisticsCriteriaBase.Clear()
        Session.Remove(SESS.StatisticsCriteriaUC)

        Me.ucStatisticsResultBase.Clear()
        Session.Remove(SESS.StatisticsResultUC)

        Session.Remove(SESS.StatisticsResultGenTime)

        Session.Remove(SESS.StatisticsCutOffDate)

        Session.Remove(SESS.ResultSetup)

        Session.Remove(SESS.ExportSetup)

        panEnquiryCriteria.Visible = False
    End Sub

    Private Sub ClearEnquiryResult()
        Session.Remove(SESS.DynamicGridData)

        Me.ucStatisticsResultBase.Clear()
        Session.Remove(SESS.StatisticsResultUC)

        Session.Remove(SESS.StatisticsResultGenTime)

        Session.Remove(SESS.StatisticsCutOffDate)

        Session.Remove(SESS.ResultSetup)

        Session.Remove(SESS.ExportSetup)

        panEnquiryCriteria.Visible = True
    End Sub

    Private Sub ClearMessage()
        Me.udcErrorMessage.Clear()
        Me.udcInfoMessageBox.Clear()
    End Sub

    Private Sub InitLayout()
        ViewState(VS.ActiveTab) = ActiveTabClass.StatList
        ViewState(VS.ContentMessageID) = String.Empty
        ViewState(VS.MessageTab) = String.Empty
    End Sub

    Private Function GetTabSubjectMaxLength(ByVal strSubject As String) As String
        Dim strReturnSubject As String = String.Empty
        If strSubject.Length <= 40 Then
            strReturnSubject = strSubject
        Else
            strReturnSubject = strSubject.Substring(0, 40)
            strReturnSubject = String.Format("{0}...", strReturnSubject)
        End If

        Return strReturnSubject
    End Function

    Public Sub BuildTabContentText(ByVal strSubject As String, ByVal strContentID As String)
        lbtnTabHeaderContent.Text = GetTabSubjectMaxLength(strSubject)
        ViewState(VS.ContentMessageID) = strContentID
    End Sub

    Public Sub ClearTabContent()
        If Not IsNothing(lbtnTabHeaderContent.Text) Then
            lbtnTabHeaderContent.Text = String.Empty
            Me.ClearMessage()
            'Session(MessageID) = String.Empty
            'Session(MessageType) = String.Empty
            ViewState(VS.ContentMessageID) = String.Empty
            ViewState(VS.MessageTab) = String.Empty
        End If
    End Sub

    ' For page load
    Public Sub CheckOpeningHour()
        MyBase.CheckOpenHour()
    End Sub

    ' For enquire button
    Public Function CheckEnquireHour() As Boolean
        Return MyBase.CheckActionAccessibility()
    End Function

#End Region

#Region "Result setup function"

    Private Function IsExistResultSetupValue(ByVal strColumnKey As String, ByVal strColumnField As String) As Boolean
        Dim blnRes As Boolean = False
        Dim dicResultSetting As New Dictionary(Of String, Dictionary(Of String, String))

        If Session(SESS.ResultSetup) Is Nothing Then
            blnRes = False
        Else
            dicResultSetting = CType(Session(SESS.ResultSetup), Dictionary(Of String, Dictionary(Of String, String)))
            If dicResultSetting.ContainsKey(strColumnKey) Then
                Dim dicResultSettingColumn As New Dictionary(Of String, String)
                dicResultSettingColumn = dicResultSetting(strColumnKey)

                If dicResultSettingColumn.ContainsKey(strColumnField) Then
                    blnRes = True
                End If

            End If
        End If

        Return blnRes
    End Function

    Private Function GetResultSetupSetting(ByVal strColumnKey As String, ByVal strColumnField As String) As String
        Dim strResult As String = String.Empty
        Dim dicResultSetting As New Dictionary(Of String, Dictionary(Of String, String))

        If Not Session(SESS.ResultSetup) Is Nothing Then
            dicResultSetting = CType(Session(SESS.ResultSetup), Dictionary(Of String, Dictionary(Of String, String)))
            If dicResultSetting.ContainsKey(strColumnKey) Then
                Dim dicResultSettingColumn As New Dictionary(Of String, String)
                dicResultSettingColumn = dicResultSetting(strColumnKey)

                If dicResultSettingColumn.ContainsKey(strColumnField) Then
                    If strColumnField = ResultSetup.DescResource Then
                        strResult = Me.GetGlobalResourceObject("Text", dicResultSettingColumn(strColumnField).ToString.Trim)
                    Else
                        strResult = dicResultSettingColumn(strColumnField).ToString.Trim
                    End If
                End If

            End If
        End If

        Return strResult
    End Function

#End Region

#Region "Export setup function"

    Private Function IsExistExportSetupValue(ByVal strColumnKey As String, ByVal strColumnField As String) As Boolean
        Dim blnRes As Boolean = False
        Dim dicExportSetting As New Dictionary(Of String, Dictionary(Of String, String))

        If Session(SESS.ExportSetup) Is Nothing Then
            blnRes = False
        Else
            dicExportSetting = CType(Session(SESS.ExportSetup), Dictionary(Of String, Dictionary(Of String, String)))
            If dicExportSetting.ContainsKey(strColumnKey) Then
                Dim dicExportSettingColumn As New Dictionary(Of String, String)
                dicExportSettingColumn = dicExportSetting(strColumnKey)

                If dicExportSettingColumn.ContainsKey(strColumnField) Then
                    blnRes = True
                End If

            End If
        End If

        Return blnRes
    End Function

    Private Function GetExportSetupSetting(ByVal strColumnKey As String, ByVal strColumnField As String) As String
        Dim strResult As String = String.Empty
        Dim dicExportSetting As New Dictionary(Of String, Dictionary(Of String, String))

        If Not Session(SESS.ExportSetup) Is Nothing Then
            dicExportSetting = CType(Session(SESS.ExportSetup), Dictionary(Of String, Dictionary(Of String, String)))
            If dicExportSetting.ContainsKey(strColumnKey) Then
                Dim dicExportSettingColumn As New Dictionary(Of String, String)
                dicExportSettingColumn = dicExportSetting(strColumnKey)

                If dicExportSettingColumn.ContainsKey(strColumnField) Then
                    If strColumnField = ExportSetup.DescResource Then
                        strResult = Me.GetGlobalResourceObject("Text", dicExportSettingColumn(strColumnField).ToString.Trim)
                    Else
                        strResult = dicExportSettingColumn(strColumnField).ToString.Trim
                    End If
                End If

            End If
        End If

        Return strResult
    End Function

    Private Function ExportDataMassage(ByVal dt As DataTable) As DataTable
        Dim dtMassage As New DataTable

        dtMassage = dt
        For Each dtColumn As DataColumn In dtMassage.Columns
            If IsExistExportSetupValue(dtColumn.ColumnName, ExportSetup.DescResource) Then
                If Not GetExportSetupSetting(dtColumn.ColumnName, ExportSetup.DescResource) = String.Empty Then
                    dtColumn.ColumnName = GetExportSetupSetting(dtColumn.ColumnName, ExportSetup.DescResource)
                End If
            End If
        Next

        Return dtMassage
    End Function

#End Region

#Region "View 1 - Enquiry"

    Public Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblEnquiryReportID.Text)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.StatEnq_Criteria_Cancel_Clicked_ID, AuditLogDesc.StatEnq_Criteria_Cancel_Clicked)

        Me.ClearMessage()

        ' Layout part [Start]
        ' ------------------------------------------------------------
        ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
        ViewState(VS.ContentMessageID) = String.Empty

        ClearTabContent()
        ' Layout part [End]

        Me.gvReportList.SelectedIndex = -1
        ClearEnquiryCriteria()

        ' Check Enquiry opening hour
        CheckOpeningHour()
    End Sub

    Public Sub ibtnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSubmit.Click

        Dim strEnquiryDateTime As String = String.Empty

        '_udtAuditLogEntry.AddDescripton("Stat ID", lblEnquiryReportID.Text)
        '_udtAuditLogEntry.WriteLog(AuditLogDesc.ClickSubmit_ID, AuditLogDesc.ClickSubmit)

        'Tommy Tse

        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblEnquiryReportID.Text)

        Dim udtParameterString As ParameterCollection = ucStatisticsCriteriaBase.GetParameterString
        Dim strParameters As String = String.Empty

        For Each udtParameter As ParameterObject In udtParameterString
            If TypeOf udtParameter Is ParameterObjectList Then
                Dim strList As String = String.Empty
                Dim IsFirst As Boolean = True
                Dim listParam As ParameterObjectList = DirectCast(udtParameter, ParameterObjectList)
                For Each strValue As String In listParam.ParamValueList
                    If IsFirst Then
                        strList = strValue
                        IsFirst = False
                    Else
                        strList += ", " + strValue
                    End If
                Next
                _udtAuditLogEntry.AddDescripton(udtParameter.ParamName, strList)
            ElseIf TypeOf udtParameter Is ParameterObject Then
                _udtAuditLogEntry.AddDescripton(udtParameter.ParamName, udtParameter.ParamValue)
            End If
        Next

        '_udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblEnquiryReportID.Text)
        '_udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_Parameters, strParameters)

        _udtAuditLogEntry.WriteLog(AuditLogDesc.StatEnq_Criteria_Submit_Clicked_ID, AuditLogDesc.StatEnq_Criteria_Submit_Clicked)

        Me.ClearMessage()

        ' Check enquire hour is avaliable [Start]
        If CheckEnquireHour() = True Then

            Dim dt As DataTable = New DataTable
            Dim strReportID As String = lblEnquiryReportID.Text

            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblEnquiryReportID.Text)
            _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatResult_PageLoad_Started_ID, AuditLogDesc.StatResult_PageLoad_Started)

            If Not udtValidator.IsEmpty(strReportID) Then
                'Check criteria is valid or not first
                Dim lstSysMsg As New List(Of SystemMessage)
                Dim lstSysMsgParam1 As New List(Of String)
                Dim lstSysMsgParam2 As New List(Of String)

                ucStatisticsCriteriaBase.ValidateCriteriaInput(strReportID, lstSysMsg, lstSysMsgParam1, lstSysMsgParam2)

                Session(SESS.StatisticsResultGenTime) = DateTime.Now.ToString((New Formatter).DisplayDateTimeFormat)
                strEnquiryDateTime = CType(Session(SESS.StatisticsResultGenTime), String)

                If lstSysMsg.Count > 0 Then
                    'Validation fail
                    For i As Integer = 0 To lstSysMsg.Count - 1
                        If lstSysMsgParam1.Count - 1 >= i Then
                            If lstSysMsgParam2.Count - 1 >= i Then
                                Me.udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s", "%t"}, New String() {lstSysMsgParam1(i).Trim, lstSysMsgParam2(i).Trim})
                            Else
                                Me.udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s"}, New String() {lstSysMsgParam1(i).Trim})
                            End If

                        Else
                            Me.udcErrorMessage.AddMessage(lstSysMsg(i))
                        End If
                    Next

                    _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblEnquiryReportID.Text)
                    _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_Enquiry_DateTime, strEnquiryDateTime)
                    Me.udcErrorMessage.BuildMessageBox("ValidationFail", _udtAuditLogEntry, AuditLogDesc.StatResult_PageLoad_Failed_ID, AuditLogDesc.StatResult_PageLoad_Failed)

                    Return

                End If

                'id and desc = xxxBLL(strReportID)

                ' Hardcode start
                ' --------------------------------------------------
                lblResultReportID.Text = lblEnquiryReportID.Text
                lblResultDesc.Text = lblEnquiryDesc.Text
                lblResultCutOffDate.Text = lblCutOffDate.Text

                Dim udtParameterList As ParameterCollection = ucStatisticsCriteriaBase.GetParameterList
                Session(SESS.StatisticsResultUC) = udtParameterList
                ucStatisticsResultBase.Build(udtParameterList)

                ' Build generation time control [Start]
                'ucStatisticsResultBase.Build(Me.GetGlobalResourceObject("Text", "EnquiryTime"), Session(SESS.StatisticsResultGenTime))
                lblEnquiryTimeText.Text = strEnquiryDateTime
                ' Build generation time control [End]

                ' Hardcode end

                ' Layout part [Start]
                ' -------------------------------------------------------------------------
                'BuildTabContentText(lblResultReportID.Text, lblResultDesc.Text)

                ViewState(VS.LastActiveTab) = ActiveTabClass.StatList
                ViewState(VS.ActiveTab) = ActiveTabClass.Content

                Select Case ViewState(VS.LastActiveTab)
                    Case ActiveTabClass.StatList
                        ViewState(VS.MessageTab) = MessageTabClass.Content

                End Select
                ' Layout part [End]

                Dim udtStatisticsModel As StatisticsModel
                Dim udtStatisticsBLL As New StatisticsBLL

                udtStatisticsModel = CType(Session(SESS.StatisticsCriteriaUC), StatisticsModel)
                dt = udtStatisticsBLL.GetEnquireDataByCriteria(ucStatisticsCriteriaBase.GetCriteriaInput, udtStatisticsModel)
                'dt = Me.GetDynamicGridData(strReportID)
                Session(SESS.DynamicGridData) = dt

                ' Set collapsbile (Always True)
                ucCollapsibleSearchCriteriaReview.Collapsed = True
                ucCollapsibleSearchCriteriaReview.ClientState = "True"

                'Dynamic grid display setting
                Me.SetDynamicGridWidth(dt)
                Me.SetDynamicGridColumnDisplay(dt)

                Me.gvDynamicGrid.PageIndex = 0
                Me.gvDynamicGrid.DataSource = dt
                Me.gvDynamicGrid.PageSize = 20
                Me.gvDynamicGrid.DataBind()

                Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Result

                ' If dataRow.Count = 0, set export button enabled = false
                If dt.Rows.Count = 0 Then
                    panNoRecordsFound.Visible = True
                    ibtnExport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExportRecordDisableBtn")
                    ibtnExport.Enabled = False
                Else
                    panNoRecordsFound.Visible = False
                    ibtnExport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExportRecordBtn")
                    ibtnExport.Enabled = True
                End If

                _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblEnquiryReportID.Text)
                _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_Enquiry_DateTime, strEnquiryDateTime)
                _udtAuditLogEntry.WriteEndLog(AuditLogDesc.StatResult_PageLoad_Succeeded_ID, AuditLogDesc.StatResult_PageLoad_Succeeded)
            End If
        Else
            ' CheckEnquireHour = False
            Return

        End If
        ' Check enquire hour is avaliable [End]       

    End Sub

#End Region

#Region "View 1 - gvReportList function"

    Protected Sub gvReportList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReportList.PageIndexChanging
        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()

        Me.gvReportList.SelectedIndex = -1
        Me.GridViewPageIndexChangingHandler(sender, e, SESS.ReportList)
    End Sub

    Protected Sub gvReportList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReportList.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS.ReportList)
    End Sub

    Protected Sub gvReportList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReportList.RowCommand

    End Sub

    Protected Sub gvReportList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReportList.RowCreated

    End Sub

    Protected Sub gvReportList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvReportList.RowDataBound
        ' Enable Grid View Row Select
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            If IsDBNull(dr("STSU_Statistic_ID")) Then
                DirectCast(e.Row.FindControl("lblDescription"), Label).Text = Me.GetGlobalResourceObject("Text", "NoRecordsFound")
                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).ColumnSpan = e.Row.Cells.Count
                e.Row.Cells(2).Visible = False
                Return
            End If

            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvReportList, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")
        End If
    End Sub

    Protected Sub gvReportList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReportList.SelectedIndexChanged

        Dim strCutOffDate As String = String.Empty

        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, CType(Me.gvReportList.Rows(Me.gvReportList.SelectedIndex).Cells(0).FindControl("lblReportID"), Label).Text.Trim)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.StatEnq_ListItem_Selected_ID, AuditLogDesc.StatEnq_ListItem_Selected)

        Me.ClearMessage()
        Me.ClearEnquiryCriteria()
        ' Check Enquiry opening hour
        CheckOpeningHour()

        Dim strStatisticsID As String = CType(Me.gvReportList.Rows(Me.gvReportList.SelectedIndex).Cells(0).FindControl("lblReportID"), Label).Text.Trim

        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, strStatisticsID)
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatEnq_Criteria_PageLoad_Started_ID, AuditLogDesc.StatEnq_Criteria_PageLoad_Started)

        Try

            Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Criteria

            Me.gvDynamicGrid.DataSource = Nothing
            Me.gvDynamicGrid.DataBind()

            panEnquiryCriteria.Visible = True
            lblEnquiryReportID.Text = CType(Me.gvReportList.Rows(Me.gvReportList.SelectedIndex).Cells(0).FindControl("lblReportID"), Label).Text.Trim
            lblEnquiryDesc.Text = CType(Me.gvReportList.Rows(Me.gvReportList.SelectedIndex).Cells(0).FindControl("lblDescription"), Label).Text.Trim
            lblEnquiryScheme.Text = CType(Me.gvReportList.Rows(Me.gvReportList.SelectedIndex).Cells(0).FindControl("lblScheme"), Label).Text.Trim
            Me.gvReportList.SelectedIndex = -1

            ' Hardcode part [Start]
            ' --------------------------------------------------------------------------
            'Session(SESS.StatisticsCriteriaUC) = LoadDisplaySeqList(lblEnquiryReportID.Text)
            'ucStatisticsCriteriaBase.Build(LoadDisplaySeqList(lblEnquiryReportID.Text))
            Dim udtStatisticsBLL As New StatisticsBLL
            Dim udtStatisticsModel As StatisticsModel

            udtStatisticsModel = udtStatisticsBLL.GetStatisticsByStatisticsID(lblEnquiryReportID.Text)

            Session(SESS.StatisticsCriteriaUC) = udtStatisticsModel
            ucStatisticsCriteriaBase.Build(udtStatisticsModel.CriteriaSetup)

            ' Display cut off date [Start]
            Session(SESS.StatisticsCutOffDate) = udtStatisticsBLL.GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
            strCutOffDate = CType(Session(SESS.StatisticsCutOffDate), DateTime).ToString((New Formatter).DisplayDateFormat)
            lblCutOffDate.Text = strCutOffDate
            ' Display cut off date [End]

            ' Display remark [Start]
            Dim strRemarkResource As String = String.Empty
            strRemarkResource = udtStatisticsModel.Remark

            If strRemarkResource.Trim.Length > 0 Then
                ucNoticePopupRemark.MessageText = strRemarkResource.Trim
                iBtnRemark.Visible = True
            Else
                iBtnRemark.Visible = False
            End If

            ' Display remark [End]

            'Session(SESS.StatisticsCriteriaUC) = GetCriteriaXML(lblEnquiryReportID.Text)
            'ucStatisticsCriteriaBase.Build(GetCriteriaXML(lblEnquiryReportID.Text))
            ' Hardcode part [End]

            ' Layout part [Start]
            ' -------------------------------------------------------------------------
            BuildTabContentText(lblEnquiryReportID.Text, lblEnquiryDesc.Text)

            ViewState(VS.LastActiveTab) = ActiveTabClass.StatList
            ViewState(VS.ActiveTab) = ActiveTabClass.Criteria

            Select Case ViewState(VS.LastActiveTab)
                Case ActiveTabClass.StatList
                    ViewState(VS.MessageTab) = MessageTabClass.Criteria

            End Select
            ' Layout part [End]

            ' Result setup dictionary [Start]
            Session(SESS.ResultSetup) = (New Creator).GetResultSetup(udtStatisticsModel.ResultSetup)
            ' Result setup dictionary [End]

            ' Export setup dictionary [Start]
            Session(SESS.ExportSetup) = (New Creator).GetResultSetup(udtStatisticsModel.ExportSetup)
            ' Export setup dictionary [End]

        Catch ex As Exception
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, strStatisticsID)
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_CutOff_Date, strCutOffDate)
            _udtAuditLogEntry.WriteEndLog(AuditLogDesc.StatEnq_Criteria_PageLoad_Failed_ID, AuditLogDesc.StatEnq_Criteria_PageLoad_Failed)
            Throw
        End Try
        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, strStatisticsID)
        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_CutOff_Date, strCutOffDate)
        _udtAuditLogEntry.WriteEndLog(AuditLogDesc.StatEnq_Criteria_PageLoad_Succeeded_ID, AuditLogDesc.StatEnq_Criteria_PageLoad_Succeeded)
    End Sub

    Protected Sub gvReportList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvReportList.Sorting
        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()

        Me.gvReportList.SelectedIndex = -1
        Me.GridViewSortingHandler(sender, e, SESS.ReportList)
    End Sub

    Private Sub GetReportListData()

        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatEnq_List_PageLoad_Started_ID, AuditLogDesc.StatEnq_List_PageLoad_Started)

        Dim dt As DataTable = New DataTable
        Dim udtStatisticsBLL As New StatisticsBLL

        Try
            'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'dt = udtStatisticsBLL.GetStatisticsListByRecordStatus("A")
            dt = udtStatisticsBLL.GetStatisticsListByRecordStatus("A", "S")
            'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]
        Catch ex As Exception
            _udtAuditLogEntry.WriteEndLog(AuditLogDesc.StatEnq_List_PageLoad_Failed_ID, AuditLogDesc.StatEnq_List_PageLoad_Failed)
            Throw
        End Try

        BindReportListData(dt)
        Me.gvDynamicGrid.PageIndex = 0

        _udtAuditLogEntry.WriteEndLog(AuditLogDesc.StatEnq_List_PageLoad_Succeeded_ID, AuditLogDesc.StatEnq_List_PageLoad_Succeeded)

    End Sub

    Private Sub BindReportListData(ByVal dtReportList As DataTable)
        If dtReportList.Rows.Count = 0 Then
            Session(SESS.ReportList) = dtReportList.Clone
            dtReportList.Rows.Add(dtReportList.NewRow)
            Me.gvReportList.AllowSorting = False
        Else
            Session(SESS.ReportList) = dtReportList
            Me.gvReportList.AllowSorting = True
        End If

        Me.gvReportList.PageIndex = 0
        Me.GridViewDataBind(gvReportList, dtReportList)
    End Sub

#End Region

#Region "View 2 - Result"

    Public Sub ibtnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnBack.Click
        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblEnquiryReportID.Text)
        _udtAuditLogEntry.WriteLog(AuditLogDesc.StatResult_Back_Clicked_ID, AuditLogDesc.StatResult_Back_Clicked)

        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()

        BindReportListData(CType(Session(SESS.ReportList), DataTable))
        Me.gvReportList.SelectedIndex = -1

        ' Layout part [Start]
        ' --------------------------------------------------------------
        ViewState(VS.LastActiveTab) = ActiveTabClass.StatList
        ViewState(VS.ActiveTab) = ActiveTabClass.Criteria

        Select Case ViewState(VS.LastActiveTab)
            Case ActiveTabClass.StatList
                ViewState(VS.MessageTab) = MessageTabClass.Criteria

        End Select
        ' Layout part [End]

        Me.gvDynamicGrid.DataSource = Nothing
        Me.gvDynamicGrid.DataBind()
    End Sub

    Public Sub ibtnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnExport.Click
        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()

        'Tommy Tse
        _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblResultReportID.Text)
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatResult_ExportRecord_Clicked_ID, AuditLogDesc.StatResult_ExportRecord_Clicked)

        Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()

        Dim ds As DataSet = New DataSet
        Dim dt As DataTable = CType(Session(SESS.DynamicGridData), DataTable).Clone
        ds.Tables.Add(GenerateContentTable(lblResultReportID.Text, lblResultDesc.Text))
        ds.Tables.Add(GenerateCriteriaTable)
        ds.Tables.Add(GenerateReportTable(lblResultReportID.Text, ExportDataMassage(dt)))

        Dim strGenerationID As String = String.Empty
        Dim strMessageID As String = String.Empty

        If udtFileGenerationBLL.Export(lblResultReportID.Text, ds, strGenerationID, strMessageID) Then
            'Tommy Tse
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblResultReportID.Text)
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Generation_ID, strGenerationID)
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Message_ID, strMessageID)
            _udtAuditLogEntry.WriteEndLog(AuditLogDesc.StatResult_ExportRecord_Succeeded_ID, AuditLogDesc.StatResult_ExportRecord_Succeeded)
            Me.popupNoticeExportRedirect.Show()
        Else
            'Tommy Tse
            udcErrorMessage.AddMessage("010703", "E", "00012")
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblResultReportID.Text)
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Generation_ID, strGenerationID)
            _udtAuditLogEntry.AddDescripton(AuditLogDesc.Message_ID, strMessageID)
            '_udtAuditLogEntry.WriteEndLog(AuditLogDesc.StatResult_ExportRecord_Failed_ID, AuditLogDesc.StatResult_ExportRecord_Failed)
            'udcErrorMessage.BuildMessageBox("ExportFailure")
            udcErrorMessage.BuildMessageBox("ExportFailure", _udtAuditLogEntry, AuditLogDesc.StatResult_ExportRecord_Failed_ID, AuditLogDesc.StatResult_ExportRecord_Failed)
        End If

        ' [CRE12-004] Statistic Enquiry [Start][Tommy Tse]

        ' [CRE12-004] Statistic Enquiry [End][Tommy Tse]

    End Sub

    Function GenerateContentTable(ByVal strFileID As String, ByVal strFileDesc As String) As DataTable
        Dim dt As DataTable = New DataTable

        Dim column As DataColumn
        Dim row As DataRow

        Dim strReportID As String = "Sub Report ID"
        Dim strReportName As String = "Sub Report Name"

        column = New DataColumn()
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = strReportID
        dt.Columns.Add(column)

        column = New DataColumn()
        column.DataType = System.Type.GetType("System.String")
        column.ColumnName = strReportName
        dt.Columns.Add(column)

        row = dt.NewRow()
        For i As Integer = 0 To dt.Columns.Count - 1
            row(dt.Columns(i).ColumnName) = dt.Columns(i).ColumnName
        Next
        dt.Rows.Add(row)

        row = dt.NewRow()
        row(strReportID) = strFileID + "-01"
        row(strReportName) = strFileDesc
        dt.Rows.Add(row)

        row = dt.NewRow()
        row(strReportID) = ""
        row(strReportName) = ""
        dt.Rows.Add(row)

        row = dt.NewRow()
        row(strReportID) = "Enquiry Time: " + Now.ToString("yyyy/MM/dd HH:mm")
        row(strReportName) = ""
        dt.Rows.Add(row)

        dt.TableName = "Content"

        Return dt
    End Function

    Function GenerateCriteriaTable() As DataTable
        Dim dt As DataTable = New DataTable

        Dim columnCriteria As DataColumn
        Dim rowCriteria As DataRow

        Dim udtParameterList As ParameterCollection = ucStatisticsCriteriaBase.GetParameterList
        Session(SESS.StatisticsResultUC) = udtParameterList

        Dim strCriteriaHeader As String = "Criteria"
        Dim strValueHeader As String = "Value"

        columnCriteria = New DataColumn()
        columnCriteria.DataType = System.Type.GetType("System.String")
        columnCriteria.ColumnName = strCriteriaHeader
        dt.Columns.Add(columnCriteria)

        columnCriteria = New DataColumn()
        columnCriteria.DataType = System.Type.GetType("System.String")
        columnCriteria.ColumnName = strValueHeader
        dt.Columns.Add(columnCriteria)

        For Each udtParameter As ParameterObject In udtParameterList
            rowCriteria = dt.NewRow()
            If TypeOf udtParameter Is ParameterObjectList Then
                Dim strList As String = String.Empty
                Dim IsFirst As Boolean = True
                Dim listParam As ParameterObjectList = DirectCast(udtParameter, ParameterObjectList)
                For Each strValue As String In listParam.ParamValueList
                    If IsFirst Then
                        strList = strValue
                        IsFirst = False
                    Else
                        strList += ", " + strValue
                    End If
                Next
                rowCriteria(strCriteriaHeader) = udtParameter.ParamName
                rowCriteria(strValueHeader) = strList
            ElseIf TypeOf udtParameter Is ParameterObject Then
                rowCriteria(strCriteriaHeader) = udtParameter.ParamName
                rowCriteria(strValueHeader) = udtParameter.ParamValue
            End If

            dt.Rows.Add(rowCriteria)
        Next

        dt.TableName = "Criteria"

        Return dt
    End Function

    Function GenerateReportTable(ByVal strFileID As String, ByVal dtFileResult As DataTable) As DataTable
        Dim dt As DataTable = New DataTable
        dtFileResult.TableName = strFileID

        Dim column As DataColumn
        For i As Integer = 0 To dtFileResult.Columns.Count - 1
            column = New DataColumn()
            column.DataType = System.Type.GetType("System.String")
            column.ColumnName = dtFileResult.Columns(i).ColumnName
            dt.Columns.Add(column)
        Next

        Dim row As DataRow
        row = dt.NewRow()
        For i As Integer = 0 To dt.Columns.Count - 1
            row(dt.Columns(i).ColumnName) = dt.Columns(i).ColumnName
        Next
        dt.Rows.Add(row)

        For Each dr As DataRow In dtFileResult.Rows
            dt.ImportRow(dr)
        Next

        Return dt
    End Function

#End Region

#Region "View 2 - gvDynamicGrid function"

    Protected Sub gvDynamicGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDynamicGrid.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS.DynamicGridData)
    End Sub

    Protected Sub gvDynamicGrid_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDynamicGrid.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS.DynamicGridData)
    End Sub

    Protected Sub gvDynamicGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDynamicGrid.RowCommand
        'Nothing
    End Sub

    Protected Sub gvDynamicGrid_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDynamicGrid.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim dt As DataTable = New DataTable
            dt = Session(SESS.DynamicGridData)

            For intCount As Integer = 0 To dt.Columns.Count - 1
                Dim cell As TableCell = e.Row.Cells(intCount)

                ' Set Column width
                If IsExistResultSetupValue(cell.Text.Trim, ResultSetup.CWidth) Then
                    If Not GetResultSetupSetting(cell.Text.Trim, ResultSetup.CWidth) = String.Empty Then
                        cell.Width = Unit.Pixel(CType(GetResultSetupSetting(cell.Text.Trim, ResultSetup.CWidth), Integer))
                    Else
                        cell.Width = Unit.Pixel(DynamicColumn_DefaultWidth)
                    End If
                Else
                    cell.Width = Unit.Pixel(DynamicColumn_DefaultWidth)
                End If

                ' Set Description
                If IsExistResultSetupValue(cell.Text.Trim, ResultSetup.DescResource) Then
                    If Not GetResultSetupSetting(cell.Text.Trim, ResultSetup.DescResource) = String.Empty Then
                        cell.Text = GetResultSetupSetting(cell.Text.Trim, ResultSetup.DescResource).Trim
                    Else
                        ' Do nothing
                    End If
                Else
                    ' Do nothing
                End If

            Next

        End If
    End Sub

    Protected Sub gvDynamicGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDynamicGrid.RowDataBound
        'Nothing
    End Sub

    Protected Sub gvDynamicGrid_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvDynamicGrid.Sorting
        Me.GridViewSortingHandler(sender, e, SESS.DynamicGridData)
    End Sub

    Private Sub SetDynamicGridWidth(ByVal dt As DataTable)
        Dim intWidth As Integer = 0

        intWidth = Me.GetDynamicGridWidth(dt)
        gvDynamicGrid.Width = Unit.Pixel(intWidth)
    End Sub

    Private Function GetDynamicGridWidth(ByVal dt As DataTable) As Integer
        Dim intWidth As Integer = 0

        Dim dicResultSetup As New Dictionary(Of String, Dictionary(Of String, String))
        dicResultSetup = CType(Session(SESS.ResultSetup), Dictionary(Of String, Dictionary(Of String, String)))

        For Each column As DataColumn In dt.Columns
            If IsExistResultSetupValue(column.ColumnName, ResultSetup.CWidth) Then
                If Not GetResultSetupSetting(column.ColumnName, ResultSetup.CWidth) = String.Empty Then
                    intWidth += CType(GetResultSetupSetting(column.ColumnName, ResultSetup.CWidth), Integer)
                Else
                    intWidth += DynamicColumn_DefaultWidth
                End If
            Else
                intWidth += DynamicColumn_DefaultWidth
            End If
        Next

        If intWidth < DynamicGrid_MinWidth Then
            intWidth = DynamicGrid_MinWidth
        End If

        Return intWidth
    End Function

    Private Sub SetDynamicGridColumnDisplay(ByVal dt As DataTable)
        gvDynamicGrid.Columns.Clear()
        For Each dtColumn As DataColumn In dt.Columns
            Dim bField As BoundField = New BoundField
            bField.DataField = dtColumn.ColumnName
            bField.HeaderText = dtColumn.ColumnName

            ' Dynamic grid case - Integer [Start]
            ' -------------------------------------------------------------------
            If dtColumn.DataType.Equals(Type.GetType("System.Int32")) Then
                bField.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            End If
            ' Dynamic grid case - Integer [End]

            gvDynamicGrid.Columns.Add(bField)
        Next

    End Sub

#End Region

#Region "View 2 - Report download popup function"

    Private Sub ucNoticeExportRedirect_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticeExportRedirect.ButtonClick
        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                '_udtAuditLogEntry.AddDescripton("Stat ID", lblEnquiryReportID.Text)
                '_udtAuditLogEntry.WriteLog(AuditLogDesc.ExportPopupClickYes_ID, AuditLogDesc.ExportPopupClickYes)

                'Tommy Tse
                _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblResultReportID.Text)
                _udtAuditLogEntry.WriteLog(AuditLogDesc.StatResult_Download_Yes_Clicked_ID, AuditLogDesc.StatResult_Download_Yes_Clicked)

                Me.popupNoticeExportRedirect.Hide()

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
                'Response.Redirect("~/ReportAndDownload/Datadownload.aspx")
                RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010702))
                ' CRE19-026 (HCVS hotline service) [End][Winnie]        

            Case ucNoticePopUp.enumButtonClick.Cancel
                '_udtAuditLogEntry.AddDescripton("Stat ID", lblEnquiryReportID.Text)
                '_udtAuditLogEntry.WriteLog(AuditLogDesc.ExportPopupClickNo_ID, AuditLogDesc.ExportPopupClickNo)

                'Tommy Tse
                _udtAuditLogEntry.AddDescripton(AuditLogDesc.Statistics_ID, lblResultReportID.Text)
                _udtAuditLogEntry.WriteLog(AuditLogDesc.StatResult_Download_No_Clicked_ID, AuditLogDesc.StatResult_Download_No_Clicked)

                Me.popupNoticeExportRedirect.Hide()

        End Select
    End Sub

#End Region

#Region "Must override function - Master Page"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

#Region "Tab and sidebar function handle"

    Protected Sub lbtnTabHeaderStatList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatSidebar_Header_Clicked_ID, AuditLogDesc.StatSidebar_Header_Clicked)

        'If ViewState(VS.ActiveTab) = ActiveTabClass.StatList Then
        '    _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatEnq_List_PageLoad_Started_ID, AuditLogDesc.StatEnq_List_PageLoad_Started)
        'End If

        If ViewState(VS.ActiveTab) = ActiveTabClass.StatList Then
            Me.udcErrorMessage.BuildMessageBox()
            Return
        End If

        ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
        Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Enquiry

        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()
    End Sub

    Protected Sub lbtnTabHeaderContent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatTab_Clicked_ID, AuditLogDesc.StatTab_Clicked)

        If ViewState(VS.ActiveTab) = ActiveTabClass.Content OrElse ViewState(VS.ActiveTab) = ActiveTabClass.Criteria Then
            Return
        End If

        ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
        'ViewState(VS.ActiveTab) = ActiveTabClass.Content
        Select Case ViewState(VS.MessageTab)
            Case MessageTabClass.Criteria
                ViewState(VS.ActiveTab) = ActiveTabClass.Criteria
            Case MessageTabClass.Content
                ViewState(VS.ActiveTab) = ActiveTabClass.Content
        End Select

        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()
    End Sub

    Protected Sub ibtnTabHeaderContentClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatTab_Close_Clicked_ID, AuditLogDesc.StatTab_Close_Clicked)

        ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
        ViewState(VS.ContentMessageID) = String.Empty

        ClearEnquiryCriteria()
        Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Enquiry

        Me.gvDynamicGrid.DataSource = Nothing
        Me.gvDynamicGrid.DataBind()

        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()
    End Sub

    Protected Sub lbtnSidebarStatList_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatSidebar_MenuItem_Clicked_ID, AuditLogDesc.StatSidebar_MenuItem_Clicked)

        'If ViewState(VS.ActiveTab) = ActiveTabClass.StatList Then
        '    _udtAuditLogEntry.WriteStartLog(AuditLogDesc.StatEnq_List_PageLoad_Started_ID, AuditLogDesc.StatEnq_List_PageLoad_Started)
        'End If

        ViewState(VS.ActiveTab) = ActiveTabClass.StatList

        'If Not ViewState(VS.MessageTab) = MessageTabClass.StatList Then
        '    ClearTabContent()
        'End If

        Me.mvStatistics.ActiveViewIndex = mvStatisticsEnum.Enquiry

        Me.ClearMessage()
        ' Check Enquiry opening hour
        CheckOpeningHour()
    End Sub

#End Region


End Class