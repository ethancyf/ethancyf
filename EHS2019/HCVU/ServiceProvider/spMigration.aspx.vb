Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject
Imports Common.Component.BankAcct
Imports Common.Component.PracticeT
Imports Common.Component.ServiceProviderT
Imports Common.Component.Token
Imports Common.Component.VoucherScheme
Imports Common.Component.MedicalOrganizationT
Imports Common.DataAccess
Imports HCVU.AccountChangeMaintenance
Imports Common.Component.StaticData
Imports Common.Component.Status


Partial Public Class spMigration
    Inherits BasePageWithGridView

#Region "Fields"
    Private udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
    Dim udcGeneralFun As New GeneralFunction
    Dim udcValidator As New Common.Validation.Validator
    Private udtFormatter As Common.Format.Formatter = New Common.Format.Formatter

    Private udtAcctChangeMaintenanceBLL As New AccountChangeMaintenanceBLL
    Private Formatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
    Private udtVoucherSchemeBLL As VoucherSchemeBLL = New VoucherSchemeBLL
    Private udtSPAccountUpateBLL As New SPAccountUpdateBLL
    Private udtSPVerificationBLL As New ServiceProviderVerificationBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
    Private SM As Common.ComObject.SystemMessage
    Private udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

    Private spStatus As String
    Private udtDB As Database = New Database
    'Protected Focus As String = String.Empty
    'Protected ControlType As String = String.Empty
    'Private Const SCRIPT_DOFOCUS As String = "window.setTimeout('DoFocus()', 1);Function DoFocus(){ try { document.getElementById('REQUEST_LASTFOCUS').focus(); } catch (ex) {} }"

    Dim udtSPmodel As ServiceProviderModel = Nothing
    Dim dtSP As New DataTable
#End Region

#Region "Constant"

    Private Const FUNCTION_CODE As String = FunctCode.FUNT010205
    Private Const strLinkToSPDataEntry As String = "~/ServiceProvider/spDataEntry.aspx"
    Private Const strYes As String = "Y"

#End Region

#Region "Session Constants"

    Public Const SESS_SPMigrationSearchCriteria As String = "SPMigrationSearchCriteria"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Not IsPostBack Then
            'Session(IsPageSessionAlive) = "Y"

            'Write Audit Log
            Dim udtAuditLogEntry As AuditLogEntry
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Service Provider Data Migration loaded")

            'Dim DDLSPMigrationStatuslist As SortedList = New SortedList()
            'DDLSPMigrationStatuslist.Add("", "Any")
            'DDLSPMigrationStatuslist.Add(SPMigrationStatus.NotMigrated, "Unprocesesd")
            'DDLSPMigrationStatuslist.Add(SPMigrationStatus.ReadyToMigrate, "Processed")
            'ddlMigrationStatus.DataSource = DDLSPMigrationStatuslist
            'ddlMigrationStatus.DataTextField = "value"
            'ddlMigrationStatus.DataValueField = "key"
            'ddlMigrationStatus.DataBind()

            ' Bind Status
            ddlMigrationStatus.DataSource = GetDescriptionListFromDBEnumCode("DataMigrationStatus")
            ddlMigrationStatus.DataValueField = "Status_Value"
            ddlMigrationStatus.DataTextField = "Status_Description"
            ddlMigrationStatus.DataBind()


            ' Bind Migration Status
            'ddlMigrationStatus.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("DataMigrationStatus")
            'ddlMigrationStatus.DataValueField = "Status_Description"
            ''If Session("language") = "zh-tw" Then
            ''    ddlMigrationStatus.DataTextField = "DataValueChi"
            ''Else
            'ddlMigrationStatus.DataTextField = "DataValue"
            ''End If
            'ddlMigrationStatus.DataBind()

            If Not IsNothing(Session(SESS_SPMigrationSearchCriteria)) Then
                txtSPID.Text = String.Empty
                txtSPHKID.Text = Formatter.formatHKIDInternal(Session(SESS_SPMigrationSearchCriteria).ToString.Trim)
                txtEnrolRefNo.Text = String.Empty
                ddlMigrationStatus.SelectedValue = String.Empty

                Session.Remove(SESS_SPMigrationSearchCriteria)
                hfBackToDataEntry.Value = strYes

                ibtnSearch_Click(Nothing, Nothing)
            End If
        End If

        'Add javascript to get the index from gridview where combobox district or rbo area is changed
        Dim getIndexScript As New StringBuilder
        getIndexScript.Append("<Script language='JavaScript'>")
        getIndexScript.Append("function getGridviewIndex(index) {")
        getIndexScript.Append("document.getElementById('" + Me.hfGridviewIndex.ClientID + "').value = index;")
        getIndexScript.Append("}")
        getIndexScript.Append("</Script>")
        ClientScript.RegisterStartupScript(Me.GetType(), "GetIndexScript", getIndexScript.ToString())

        Dim getPracticeIndexScript As New StringBuilder
        getPracticeIndexScript.Append("<Script language='JavaScript'>")
        getPracticeIndexScript.Append("function getPracticeGridviewIndex(index) {")
        getPracticeIndexScript.Append("document.getElementById('" + Me.hfPracticeGridviewIndex.ClientID + "').value = index;")
        getPracticeIndexScript.Append("}")
        getPracticeIndexScript.Append("</Script>")
        ClientScript.RegisterStartupScript(Me.GetType(), "getPracticeIndexScript", getPracticeIndexScript.ToString())



        'If (Not IsPostBack) Then
        '    HookOnFocus(CType(Me.Page, Control))
        'End If

        'Dim getFocusScript As New StringBuilder
        'getFocusScript.Append("<Script language='JavaScript'>")
        'getFocusScript.Append("window.setTimeout('DoFocus()', 1);")
        'getFocusScript.Append("function DoFocus()")
        'getFocusScript.Append("{")
        'getFocusScript.Append("try {")
        'getFocusScript.Append("document.getElementById('REQUEST_LASTFOCUS').focus();")
        'getFocusScript.Append("} catch (ex) {}")
        'getFocusScript.Append("}")
        'getFocusScript.Append("</Script>")
        ''replaces REQUEST_LASTFOCUS in SCRIPT_DOFOCUS with the posted value from Request ["__LASTFOCUS"]and registers the script to start after the page was rendered
        ''ClientScript.RegisterStartupScript(Me.GetType(), "ScriptDoFocus", SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request("__LASTFOCUS")), True)
        'ClientScript.RegisterStartupScript(Me.GetType, "ScriptDoFocus", getFocusScript.ToString.Replace("REQUEST_LASTFOCUS", Request("__LASTFOCUS")))
        'If Not IsNothing(Session("SetFocusTotxtMOCName")) AndAlso Session("SetFocusTotxtMOCName") Then

        '    Dim row As Integer
        '    For row = 0 To gvPracticeBank.Rows.Count - 1
        '        Dim ddlPracticeMO As DropDownList = CType(gvPracticeBank.Rows(row).FindControl("ddlPracticeMO"), DropDownList)
        '        ddlPracticeMO.Attributes.Add("ondatabinding", "LoseFocus(this)")
        '    Next
        '    Dim txtMOCName As TextBox = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtMOCName")
        '    txtMOCName.Focus()
        '    ClientScript.RegisterStartupScript(Me.GetType, "ScriptDoFocus", getFocusScript.ToString.Replace("REQUEST_LASTFOCUS", txtMOCName.ClientID))
        '    Session("SetFocusTotxtMOCName") = False
        'End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Select Case Me.MultiViewSPMigration.ActiveViewIndex
            Case 0
                ScriptManager1.SetFocus(Me.txtEnrolRefNo)
                Me.pnlDataEntry.DefaultButton = ibtnSearch.ID
        End Select


        'If Not IsNothing(Session("SetFocusTotxtMOCName")) AndAlso Session("SetFocusTotxtMOCName") Then
        '    CSBSetFocus()
        '    Session("SetFocusTotxtMOCName") = False
        'End If
    End Sub

#End Region

    'Private Sub CSBSetFocus()
    '    Dim strJS As String
    '    strJS = "<script language='javascript'>"
    '    strJS += "var o = document.getElementById('" + Focus.ToString() + "'); "
    '    strJS += "if (o != null) o.focus(); "
    '    strJS += "if('" + ControlType.ToString() + "' == 'TextArea') "
    '    strJS += "document.all['" + Focus.ToString() + "'].select(); "
    '    strJS += "</script>"
    '    ClientScript.RegisterStartupScript(Me.GetType(), "CSB-focus-function", strJS.ToString())
    'End Sub

    'Private Sub HookOnFocus(ByVal CurrentControl As Control)

    '    'checks if control is one of TextBox, DropDownList, ListBox or Button
    '    If ((TypeOf CurrentControl Is TextBox) Or (TypeOf CurrentControl Is DropDownList) Or (TypeOf CurrentControl Is ListBox) Or (TypeOf CurrentControl Is Button)) Then
    '        'adds a script which saves active control on receiving focus in the hidden field __LASTFOCUS.
    '        CType(CurrentControl, WebControl).Attributes.Add("onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}")
    '    End If
    '    'checks if the control has children
    '    If (CurrentControl.HasControls()) Then
    '        'if yes do them all recursively
    '        For Each CurrentChildControl As Control In CurrentControl.Controls
    '            HookOnFocus(CurrentChildControl)
    '        Next
    '    End If
    'End Sub

    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click
        Dim err As Boolean = False
        Dim dt As DataTable

        dt = New DataTable
        Dim blnRes As Boolean = False

        Dim udtSPMigrationBLL As New SPMigrationBLL
        Dim udcLoginBll As New BLL.LoginBLL

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Session("FirstBind") = True
        Session("PracticeChiAddressFilterd") = False
        Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
        Dim udtHCVUUser As HCVUUser.HCVUUserModel
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Try
            Dim strERN As String = String.Empty
            txtEnrolRefNo.Text = UCase(txtEnrolRefNo.Text)

            If Not txtEnrolRefNo.Text.Trim.Equals(String.Empty) Then
                If udcValidator.chkSystemNumber(txtEnrolRefNo.Text.Trim) Then
                    strERN = Formatter.ReverseSystemNumber(txtEnrolRefNo.Text.Trim)
                Else
                    strERN = txtEnrolRefNo.Text.Trim
                End If
            End If

            'Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.AddDescripton("ERN", strERN)
            udtAuditLogEntry.AddDescripton("SPID", txtSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP HKID", txtSPHKID.Text.Trim)
            udtAuditLogEntry.AddDescripton("Migration Status", ddlMigrationStatus.SelectedValue.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search")

            udcInfoMessageBox.Clear()

            If strERN.Trim = String.Empty And txtSPID.Text.Trim = String.Empty And txtSPHKID.Text.Trim = String.Empty Then
                'Batch Search
                dt = udtSPMigrationBLL.SPDataMigrationSearch(txtSPID.Text.Trim, txtSPHKID.Text.Trim, strERN, ddlMigrationStatus.SelectedValue)

                If dt.Rows.Count = 0 Then
                    ' No record found
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, "Search Completed: No Record")

                    udcInfoMessageBox.AddMessage(New SystemMessage("990000", "I", "00001"))
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                Else
                    If txtEnrolRefNo.Text.Trim.Equals(String.Empty) Then
                        lblResultERN.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        lblResultERN.Text = txtEnrolRefNo.Text.Trim
                    End If

                    If txtSPID.Text.Trim.Equals(String.Empty) Then
                        lblResultSPID.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        lblResultSPID.Text = txtSPID.Text.Trim
                    End If

                    If txtSPHKID.Text.Trim.Equals(String.Empty) Then
                        lblResultSPHKID.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        lblResultSPHKID.Text = txtSPHKID.Text.Trim
                    End If

                    If ddlMigrationStatus.SelectedIndex = 0 Then
                        lblResultMigrationRecordStatus.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        lblResultMigrationRecordStatus.Text = ddlMigrationStatus.SelectedItem.Text
                    End If

                    Session("MigrationResult") = dt

                    Me.GridViewDataBind(gvResult, dt, "Enrolment_Ref_No", "ASC", False)

                    ibtnBack.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "BackBtn")
                    ibtnBack.AlternateText = Me.GetGlobalResourceObject("AlternateText", "BackBtn")
                    Me.MultiViewSPMigration.ActiveViewIndex = 1
                End If
                'Else
                '    If strERN = String.Empty And txtSPID.Text.Trim = String.Empty And txtSPHKID.Text.Trim = String.Empty Then
                '        ' No Input Criteria
                '        udcInfoMessageBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00041))
                '        udcInfoMessageBox.BuildMessageBox()
                '        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                '        udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "No Input Criteria")
            Else
                'dt = udtAcctChangeMaintenanceBLL.MaintenanceSearch(strERN, txtSPID.Text.Trim, _
                '        udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), String.Empty, String.Empty, _
                '        String.Empty, String.Empty)
                dt = udtSPMigrationBLL.SPDataMigrationSearch(txtSPID.Text.Trim, txtSPHKID.Text.Trim.Replace("(", "").Replace(")", ""), strERN, ddlMigrationStatus.SelectedValue)

                If dt.Rows.Count = 0 Then
                    ' No record found
                    udcInfoMessageBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, "I", MsgCode.MSG00001))
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search Completed: No Record")

                Else
                    'udtSPmodel = udtServiceProviderBLL.GetServiceProviderStagingByERN(dt.Rows.Item(0).Item(2), New Database)
                    udtSPmodel = udtServiceProviderBLL.GetServiceProviderStagingByERN_NoReader(dt.Rows.Item(0).Item("Enrolment_Ref_No"), New Database)

                    If IsNothing(udtSPmodel) Then
                        udtSPmodel = udtServiceProviderBLL.GetServiceProviderBySPID(New Database, dt.Rows(0)("SP_ID"))
                    Else
                        'Get MO
                        'Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
                        'udtSPmodel.MOList = udtMOBLL.GetMOListFromStagingByERN(udtSPmodel.EnrolRefNo, udtDB)

                        ' Get Practice, Bank, Practice Scheme Information
                        Dim udtPracticeBLL As New PracticeBLL
                        udtSPmodel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSPmodel.EnrolRefNo, udtDB)
                    End If
                    Session("udtTempPracticeModelCollection") = udtSPmodel.PracticeList

                    'Get SPMigration Status, TSMP
                    Dim dtSPMigration As DataTable = udtSPMigrationBLL.GetSPMigrationStatus(String.Empty, udtSPmodel.HKID, String.Empty)
                    If dtSPMigration.Rows.Count = 0 Then
                        spStatus = SPMigrationStatus.NotMigrated
                        udtSPMigrationBLL.SPMigrationAdd(udtSPmodel.HKID, SPMigrationStatus.NotMigrated)
                        'take TSMP
                        dtSPMigration = udtSPMigrationBLL.GetSPMigrationStatus(String.Empty, udtSPmodel.HKID, String.Empty)
                        Session(SPMigrationBLL.SESS_SPMigrationTSMP) = CType(dtSPMigration.Rows(0).Item("TSMP"), Byte())
                    Else
                        spStatus = dtSPMigration.Rows(0).Item("Record_Status")
                        Session(SPMigrationBLL.SESS_SPMigrationTSMP) = CType(dtSPMigration.Rows(0).Item("TSMP"), Byte())
                    End If

                    Session("MigrationStatus") = spStatus
                    'If (spStatus.Trim = "N" Or spStatus.Trim = "R") And (udtSPmodel.RecordStatus <> ServiceProviderStatus.Delisted And (Not blnAllPracticeDislisted)) Then
                    '(spStatus.Trim = SPMigrationStatus.NotMigrated Or spStatus.Trim = SPMigrationStatus.ReadyToMigrate) And 
                    If (udtSPmodel.RecordStatus <> ServiceProviderStatus.Delisted) Then
                        udtSPMigrationBLL.UpdateMOListFromMOMigrationByERN_T(udtSPmodel.EnrolRefNo, udtHCVUUser.UserID, udtSPmodel)
                        udtSPMigrationBLL.UpdatePracticeMigrationDetailsByERN_T(udtSPmodel.EnrolRefNo, udtSPmodel.HKID, udtHCVUUser.UserID, udtSPmodel.PracticeList)

                        'Remove address code
                        For Each udtMOModel As MedicalOrganizationModel In udtSPmodel.MOList.Values
                            udtMOModel.MOAddress.Address_Code = Nothing
                        Next
                        For Each udtPracticeModel As PracticeModel In udtSPmodel.PracticeList.Values
                            udtPracticeModel.PracticeAddress.Address_Code = Nothing
                        Next

                        udtServiceProviderBLL.SaveToSession(udtSPmodel)
                        blnRes = BindSPSummaryView()

                        ibtnBack.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "BackBtn")
                        ibtnBack.AlternateText = Me.GetGlobalResourceObject("AlternateText", "BackBtn")
                        Me.MultiViewSPMigration.ActiveViewIndex = 2

                        udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search Completed")
                    Else
                        ' No record found
                        udcInfoMessageBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, "I", MsgCode.MSG00001))
                        udcInfoMessageBox.BuildMessageBox()
                        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search Completed: No Record")
                    End If

                End If
            End If
            Me.udcErrorMessage.BuildMessageBox("ValidationFail")
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcErrorMessage.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990001, "D", eSQL.Message))

                If udcErrorMessage.GetCodeTable.Rows.Count = 0 Then
                    udcErrorMessage.Visible = False
                Else
                    udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search failed")
                End If

            Else
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If txtEnrolRefNo.Text.Trim = String.Empty AndAlso txtSPID.Text.Trim = String.Empty AndAlso txtSPHKID.Text.Trim = String.Empty Then
            MultiViewSPMigration.ActiveViewIndex = 1
        Else
            MultiViewSPMigration.ActiveViewIndex = 0
            txtSPID.Text = String.Empty
            txtSPHKID.Text = String.Empty
            txtEnrolRefNo.Text = String.Empty
        End If

        hfGridviewIndex.Value = String.Empty

        udcErrorMessage.Clear()
        udcInfoMessageBox.Clear()
    End Sub

    Protected Sub ibtnCompleteBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.MultiViewSPMigration.ActiveViewIndex = 0
        udcInfoMessageBox.Clear()
        udcErrorMessage.Clear()
        Me.txtSPID.Text = String.Empty
        Me.txtSPHKID.Text = String.Empty
        Me.txtEnrolRefNo.Text = String.Empty

        Me.hfGridviewIndex.Value = String.Empty
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtServiceProviderModel As ServiceProviderModel
        udtServiceProviderModel = udtServiceProviderBLL.GetSP

        'Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        Dim strOld As String() = {"%r", "%s"}

        'MO
        For Each r As GridViewRow In gvMO.Rows
            Dim lblMOIndex As Label = CType(r.FindControl("lblMOIndex"), Label)
            Dim txtMOBRCodeText As TextBox = CType(r.FindControl("txtMOBRCodeText"), TextBox)
            Dim txtMOContactNoText As TextBox = CType(r.FindControl("txtMOContactNoText"), TextBox)
            'Dim txtMOChiAddressText As TextBox = CType(r.FindControl("txtMOChiAddress"), TextBox)
            Dim txtMOBuilding As TextBox = CType(r.FindControl("txtMOBuilding"), TextBox)
            Dim ddlMOEditDistrict As DropDownList = CType(r.FindControl("ddlMOEditDistrict"), DropDownList)
            'Dim rboMOEditArea As RadioButtonList = CType(r.FindControl("rboMOEditArea"), RadioButtonList)
            Dim txtMORoom As TextBox = CType(r.FindControl("txtMORoom"), TextBox)
            Dim txtMOFloor As TextBox = CType(r.FindControl("txtMOFloor"), TextBox)
            Dim txtMOBlock As TextBox = CType(r.FindControl("txtMOBlock"), TextBox)
            Dim txtMOEmail As TextBox = CType(r.FindControl("txtMOEmail"), TextBox)
            Dim txtMOFax As TextBox = CType(r.FindControl("txtMOFax"), TextBox)

            Dim txtMOCNameText As TextBox = CType(r.FindControl("txtMOCName"), TextBox)
            Dim txtMOENameText As TextBox = CType(r.FindControl("txtMOEName"), TextBox)

            Dim rboEditMORelation As RadioButtonList = r.FindControl("rboEditMORelation")
            Dim txtEditMORelationRemark As TextBox = r.FindControl("txtEditMORelationRemark")

            Dim imgEditMORelationRemarksAlert As Image = CType(r.FindControl("imgEditMORelationRemarksAlert"), Image)
            Dim imgEditMOBRCodeAlert As Image = CType(r.FindControl("imgEditMOBRCodeAlert"), Image)
            Dim imgEditMOContactNoAlert As Image = CType(r.FindControl("imgEditMOContactNoAlert"), Image)
            Dim imgEditMOENameAlert As Image = CType(r.FindControl("imgEditMOENameAlert"), Image)
            Dim imgMOBuildingAlert As Image = CType(r.FindControl("imgMOBuildingAlert"), Image)
            Dim imgMOEditDistrictAlert As Image = CType(r.FindControl("imgMOEditDistrictAlert"), Image)
            Dim imgEditMOEmailAlert As Image = CType(r.FindControl("imgEditMOEmailAlert"), Image)

            For Each udtMedicalOrganizationModel As MedicalOrganizationModel In udtServiceProviderModel.MOList.Values
                If udtMedicalOrganizationModel.DisplaySeq = lblMOIndex.Text.Trim Then
                    udtMedicalOrganizationModel.MOEngName = txtMOENameText.Text.Trim
                    udtMedicalOrganizationModel.MOChiName = txtMOCNameText.Text.Trim
                    udtMedicalOrganizationModel.Fax = txtMOFax.Text.Trim
                    udtMedicalOrganizationModel.Email = txtMOEmail.Text.Trim
                    udtMedicalOrganizationModel.BrCode = txtMOBRCodeText.Text.Trim
                    udtMedicalOrganizationModel.PhoneDaytime = txtMOContactNoText.Text.Trim

                    udtMedicalOrganizationModel.MOAddress.Block = txtMOBlock.Text.Trim
                    udtMedicalOrganizationModel.MOAddress.Room = txtMORoom.Text.Trim
                    udtMedicalOrganizationModel.MOAddress.Floor = txtMOFloor.Text.Trim
                    'udtMedicalOrganizationModel.MOAddress.ChiBuilding = txtMOChiAddressText.Text
                    udtMedicalOrganizationModel.MOAddress.Building = txtMOBuilding.Text.Trim
                    udtMedicalOrganizationModel.MOAddress.District = ddlMOEditDistrict.SelectedValue.Trim
                    'udtMedicalOrganizationModel.MOAddress.AreaCode = rboMOEditArea.SelectedValue

                    udtMedicalOrganizationModel.Relationship = rboEditMORelation.SelectedValue.Trim
                    udtMedicalOrganizationModel.RelationshipRemark = txtEditMORelationRemark.Text.Trim

                    ''Add audit log
                    'udtAuditLogEntry.AddDescripton("DisplaySeq ", lblMOIndex.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("MOEngName ", txtMOENameText.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("MOChiName ", txtMOCNameText.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Fax ", txtMOFax.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Email ", txtMOEmail.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("BrCode ", txtMOBRCodeText.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("PhoneDaytime ", txtMOContactNoText.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Room ", txtMORoom.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Block ", txtMORoom.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Floor ", txtMOFloor.Text.Trim)
                    ''udtAuditLogEntry.AddDescripton("ChiBuilding ", txtMOChiAddressText.Text)
                    'udtAuditLogEntry.AddDescripton("Building ", txtMOBuilding.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("District ", ddlMOEditDistrict.SelectedValue.Trim)
                    ''udtAuditLogEntry.AddDescripton("AreaCode ", rboMOEditArea.SelectedValue)
                    'udtAuditLogEntry.AddDescripton("Relationship ", rboEditMORelation.SelectedValue.Trim.Trim)
                    'udtAuditLogEntry.AddDescripton("RelationshipRemark ", txtEditMORelationRemark.Text.Trim.Trim)
                    'udtAuditLogEntry.WriteStartLog(LogID.LOG00006, "Edit MO :")
                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00007, "Edit MO completed.")


                    ''check english medical organisation name
                    'If txtMOENameText.Text = String.Empty Then
                    '    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00174)
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    '    imgEditMOENameAlert.Visible = True
                    'Else
                    '    imgEditMOENameAlert.Visible = False
                    'End If

                    ''check BR code
                    'If txtMOBRCodeText.Text.Trim() = String.Empty Then
                    '    imgEditMOBRCodeAlert.Visible = True
                    '    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00175)
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    'Else
                    '    imgEditMOBRCodeAlert.Visible = False
                    'End If

                    ''check MO phone no
                    'If txtMOContactNoText.Text.Trim() = String.Empty Then
                    '    imgEditMOContactNoAlert.Visible = True
                    '    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00176)
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    'Else
                    '    imgEditMOContactNoAlert.Visible = False
                    'End If

                    ''check email
                    'If txtMOEmail.Text.Trim <> String.Empty Then
                    '    If Not IsNothing(udcValidator.chkEmailAddress(txtMOEmail.Text)) Then
                    '        SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00181)
                    '        Dim strNew As String() = {"", ""}
                    '        strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    '        strNew(1) = (r.RowIndex + 1).ToString
                    '        udcErrorMessage.AddMessage(SM, strOld, strNew)
                    '        imgEditMOEmailAlert.Visible = True
                    '    Else
                    '        imgEditMOEmailAlert.Visible = False
                    '    End If
                    'End If

                    ''check address
                    'If (txtMOBuilding.Text.Trim() = String.Empty) Or (ddlMOEditDistrict.SelectedValue = ".H" Or ddlMOEditDistrict.SelectedValue = ".K" Or ddlMOEditDistrict.SelectedValue = ".N" Or ddlMOEditDistrict.SelectedValue = String.Empty) Or (rboMOEditArea.SelectedIndex = -1) Then
                    '    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00180)
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = ""
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    'End If

                    'If txtMOBuilding.Text.Trim() = String.Empty Then
                    '    imgMOBuildingAlert.Visible = True
                    'Else
                    '    imgMOBuildingAlert.Visible = False
                    'End If

                    'If (ddlMOEditDistrict.SelectedValue = ".H" Or ddlMOEditDistrict.SelectedValue = ".K" Or ddlMOEditDistrict.SelectedValue = ".N" Or ddlMOEditDistrict.SelectedValue = String.Empty) Then
                    '    imgMOEditDistrictAlert.Visible = True
                    'Else
                    '    imgMOEditDistrictAlert.Visible = False
                    'End If

                    ''check the relation of MO
                    'SM = udcValidator.chkMORelation(rboEditMORelation.SelectedValue.Trim)
                    'If Not IsNothing(SM) Then
                    '    imgEditMORelationRemarksAlert.Visible = True
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    'Else
                    '    imgEditMORelationRemarksAlert.Visible = False
                    'End If

                    'If rboEditMORelation.SelectedValue.Trim.Equals("O") Then
                    '    If udcValidator.IsEmpty(txtEditMORelationRemark.Text.Trim) Then
                    '        imgEditMORelationRemarksAlert.Visible = True
                    '        SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00178)
                    '        Dim strNew As String() = {"", ""}
                    '        strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    '        strNew(1) = (r.RowIndex + 1).ToString
                    '        udcErrorMessage.AddMessage(SM, strOld, strNew)
                    '    Else
                    '        imgEditMORelationRemarksAlert.Visible = False
                    '    End If
                    'End If

                End If
            Next

            'reset MO relation remark textbox
            Dim hfEditMORelation As HiddenField = r.FindControl("hfEditMORelation")
            hfEditMORelation.Value = rboEditMORelation.SelectedValue.Trim
            If hfEditMORelation.Value.Trim.Equals("O") Then
                txtEditMORelationRemark.BackColor = Nothing
                txtEditMORelationRemark.Attributes.Remove("readonly")
            Else
                txtEditMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtEditMORelationRemark.Attributes.Add("readonly", "readonly")
            End If
        Next


        'practice
        For Each r As GridViewRow In gvPracticeBank.Rows
            Dim lblPracticeBankIndex As Label = CType(r.FindControl("lblPracticeBankIndex"), Label)
            Dim txtPracticeChiName As TextBox = CType(r.FindControl("txtPracticeChiName"), TextBox)
            Dim txtPracticeChiAddress As TextBox = CType(r.FindControl("txtPracticeChiAddress"), TextBox)
            Dim txtPracticePhone As TextBox = CType(r.FindControl("txtPracticePhone"), TextBox)
            Dim ddlPracticeMO As DropDownList = CType(r.FindControl("ddlPracticeMO"), DropDownList)
            Dim ddlPracticeEditDistrict As DropDownList = CType(r.FindControl("ddlPracticeEditDistrict"), DropDownList)
            'Dim rboPracticeEditArea As RadioButtonList = CType(r.FindControl("rboPracticeEditArea"), RadioButtonList)
            Dim txtPracticeBuilding As TextBox = CType(r.FindControl("txtPracticeBuilding"), TextBox)
            Dim txtPracticeRoom As TextBox = CType(r.FindControl("txtPracticeRoom"), TextBox)
            Dim txtPracticeFloor As TextBox = CType(r.FindControl("txtPracticeFloor"), TextBox)
            Dim txtPracticeBlock As TextBox = CType(r.FindControl("txtPracticeBlock"), TextBox)

            Dim imgEditPracticePhoneAlert As Image = CType(r.FindControl("imgEditPracticePhoneAlert"), Image)
            Dim imgEditPracticeMOAlert As Image = CType(r.FindControl("imgEditPracticeMOAlert"), Image)
            Dim imgPracticeBuildingAlert As Image = CType(r.FindControl("imgPracticeBuildingAlert"), Image)
            Dim imgPracticeEditDistrictAlert As Image = CType(r.FindControl("imgPracticeEditDistrictAlert"), Image)


            For Each udtPracticeModel As PracticeModel In udtServiceProviderModel.PracticeList.Values
                If udtPracticeModel.DisplaySeq = lblPracticeBankIndex.Text.Trim Then
                    udtPracticeModel.PracticeNameChi = txtPracticeChiName.Text.Trim
                    udtPracticeModel.PhoneDaytime = txtPracticePhone.Text.Trim

                    udtPracticeModel.MODisplaySeq = ddlPracticeMO.SelectedValue.Trim

                    udtPracticeModel.PracticeAddress.Building = txtPracticeBuilding.Text.Trim
                    udtPracticeModel.PracticeAddress.ChiBuilding = txtPracticeChiAddress.Text.Trim
                    udtPracticeModel.PracticeAddress.Room = txtPracticeRoom.Text.Trim
                    udtPracticeModel.PracticeAddress.Floor = txtPracticeFloor.Text.Trim
                    udtPracticeModel.PracticeAddress.Block = txtPracticeBlock.Text.Trim
                    udtPracticeModel.PracticeAddress.District = ddlPracticeEditDistrict.SelectedValue.Trim
                    'udtPracticeModel.PracticeAddress.AreaCode = rboPracticeEditArea.SelectedValue

                    ''Add audit log
                    'udtAuditLogEntry.AddDescripton("DisplaySeq ", lblPracticeBankIndex.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("PracticeNameChi ", txtPracticeChiName.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Building ", txtPracticeBuilding.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("District ", ddlPracticeEditDistrict.SelectedValue.Trim)
                    ''udtAuditLogEntry.AddDescripton("AreaCode ", rboPracticeEditArea.SelectedValue)
                    'udtAuditLogEntry.AddDescripton("Room ", txtPracticeRoom.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Floor ", txtPracticeFloor.Text.Trim)
                    'udtAuditLogEntry.AddDescripton("Block ", txtPracticeBlock.Text.Trim)
                    'udtAuditLogEntry.WriteStartLog(LogID.LOG00008, "Edit Practice :")
                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Edit Practice completed.")

                    'check MO
                    'If ddlPracticeMO.SelectedValue = "0" Then
                    '    'If ddlPracticeMO.SelectedValue.Trim() = Me.GetGlobalResourceObject("Text", "SelectMO") Then
                    '    'If ddlPracticeMO.SelectedValue = "0" Then
                    '    imgEditPracticeMOAlert.Visible = True
                    '    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00179)
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = Me.GetGlobalResourceObject("Text", "Practice")
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    '    'Else
                    '    '    imgEditPracticeMOAlert.Visible = False
                    '    'End If
                    'Else
                    '    'udtPracticeModel.MODisplaySeq = ddlPracticeMO.SelectedValue.Substring(0, ddlPracticeMO.SelectedValue.IndexOf("."))
                    '    udtPracticeModel.MODisplaySeq = ddlPracticeMO.SelectedValue
                    '    imgEditPracticeMOAlert.Visible = False
                    'End If

                    ''check address
                    'If (txtPracticeBuilding.Text.Trim() = String.Empty) Or (ddlPracticeEditDistrict.SelectedValue = ".H" Or ddlPracticeEditDistrict.SelectedValue = ".K" Or ddlPracticeEditDistrict.SelectedValue = ".N" Or ddlPracticeEditDistrict.SelectedValue = String.Empty) Or (rboPracticeEditArea.SelectedIndex = -1) Then
                    '    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00177)
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = ""
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    'End If

                    'If txtPracticeBuilding.Text.Trim() = String.Empty Then
                    '    imgPracticeBuildingAlert.Visible = True
                    'Else
                    '    imgPracticeBuildingAlert.Visible = False
                    'End If

                    'If (ddlPracticeEditDistrict.SelectedValue = ".H" Or ddlPracticeEditDistrict.SelectedValue = ".K" Or ddlPracticeEditDistrict.SelectedValue = ".N" Or ddlPracticeEditDistrict.SelectedValue = String.Empty) Then
                    '    imgPracticeEditDistrictAlert.Visible = True
                    'Else
                    '    imgPracticeEditDistrictAlert.Visible = False
                    'End If

                    ''check Practice phone no
                    'If txtPracticePhone.Text.Trim() = String.Empty Then
                    '    imgEditPracticePhoneAlert.Visible = True
                    '    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00176)
                    '    Dim strNew As String() = {"", ""}
                    '    strNew(0) = Me.GetGlobalResourceObject("Text", "Practice")
                    '    strNew(1) = (r.RowIndex + 1).ToString
                    '    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    'Else
                    '    imgEditPracticePhoneAlert.Visible = False
                    'End If
                End If
            Next
        Next



        If Me.editValidation() Then
            'udcErrorMessage.Visible = False
            Me.MultiViewSPMigration.ActiveViewIndex = 3

            udcInfoMessageBox.AddMessage(FunctCode.FUNT010205, "I", "00001")
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            BindPRSummaryView()
        Else
            'udcErrorMessage.Visible = True
            'Me.udcErrorMessage.BuildMessageBox("ValidationFail")

            'Me.MultiViewSPMigration.ActiveViewIndex = 1
            'udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search failed")
        End If


    End Sub



    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.MultiViewSPMigration.ActiveViewIndex = 0
        BindSPSummaryView()
        udcInfoMessageBox.Clear()
        udcErrorMessage.Clear()

        Me.hfGridviewIndex.Value = String.Empty
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.MultiViewSPMigration.ActiveViewIndex = 0
        BindSPSummaryView()
        udcInfoMessageBox.Clear()
        udcErrorMessage.Clear()

        Me.hfGridviewIndex.Value = String.Empty
    End Sub

    Protected Sub ibtnPRBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.MultiViewSPMigration.ActiveViewIndex = 2
        'BindSPSummaryView()
        udcInfoMessageBox.Clear()
        udcErrorMessage.Clear()

        Me.hfGridviewIndex.Value = String.Empty
    End Sub



    Protected Sub ibtnPRConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
        Dim udtHCVUUser As HCVUUser.HCVUUserModel
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        'update record
        Dim udtSPMigrationBLL As New SPMigrationBLL()
        Dim udtServiceProviderModel As ServiceProviderModel
        Dim blnResult As Boolean

        udtServiceProviderModel = udtServiceProviderBLL.GetSP

        Try
            udtAuditLogEntry.AddDescripton("ERN", udtServiceProviderModel.EnrolRefNo)
            udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderModel.SPID)
            udtAuditLogEntry.AddDescripton("SP HKID", udtServiceProviderModel.HKID)
            For Each udtMedicalOrganizationModel As MedicalOrganizationModel In udtServiceProviderModel.MOList.Values
                udtAuditLogEntry.AddDescripton("MO", "MO")
                udtAuditLogEntry.AddDescripton("DisplaySeq ", udtMedicalOrganizationModel.DisplaySeq)
                udtAuditLogEntry.AddDescripton("MOEngName ", udtMedicalOrganizationModel.MOEngName)
                udtAuditLogEntry.AddDescripton("MOChiName ", udtMedicalOrganizationModel.MOChiName)
                udtAuditLogEntry.AddDescripton("Fax ", udtMedicalOrganizationModel.Fax)
                udtAuditLogEntry.AddDescripton("Email ", udtMedicalOrganizationModel.Email)
                udtAuditLogEntry.AddDescripton("BrCode ", udtMedicalOrganizationModel.BrCode)
                udtAuditLogEntry.AddDescripton("PhoneDaytime ", udtMedicalOrganizationModel.PhoneDaytime)
                udtAuditLogEntry.AddDescripton("Room ", udtMedicalOrganizationModel.MOAddress.Room)
                udtAuditLogEntry.AddDescripton("Block ", udtMedicalOrganizationModel.MOAddress.Block)
                udtAuditLogEntry.AddDescripton("Floor ", udtMedicalOrganizationModel.MOAddress.Floor)
                udtAuditLogEntry.AddDescripton("Building ", udtMedicalOrganizationModel.MOAddress.Building)
                udtAuditLogEntry.AddDescripton("District ", udtMedicalOrganizationModel.MOAddress.District)
                udtAuditLogEntry.AddDescripton("Relationship ", udtMedicalOrganizationModel.Relationship)
                udtAuditLogEntry.AddDescripton("RelationshipRemark ", udtMedicalOrganizationModel.RelationshipRemark)
                'udtAuditLogEntry.WriteLog(LogID.LOG00006, "Edit MO :")
            Next
            For Each udtPracticeModel As PracticeModel In udtServiceProviderModel.PracticeList.Values
                If udtPracticeModel.RecordStatus <> Common.Component.PracticeStatus.Delisted And udtPracticeModel.RecordStatus <> Common.Component.DelistStatus.Involuntary And udtPracticeModel.RecordStatus <> Common.Component.DelistStatus.Voluntary Then
                    udtAuditLogEntry.AddDescripton("Practice", "Practice")
                    udtAuditLogEntry.AddDescripton("DisplaySeq ", udtPracticeModel.DisplaySeq)
                    udtAuditLogEntry.AddDescripton("PracticeNameEng ", udtPracticeModel.PracticeName)
                    udtAuditLogEntry.AddDescripton("PracticeNameChi ", udtPracticeModel.PracticeNameChi)
                    udtAuditLogEntry.AddDescripton("PracticePhone", udtPracticeModel.PhoneDaytime)
                    udtAuditLogEntry.AddDescripton("MO_Display_Seq", udtPracticeModel.MODisplaySeq)
                    udtAuditLogEntry.AddDescripton("Building ", udtPracticeModel.PracticeAddress.Building)
                    udtAuditLogEntry.AddDescripton("ChiBuilding ", udtPracticeModel.PracticeAddress.ChiBuilding)
                    udtAuditLogEntry.AddDescripton("District ", udtPracticeModel.PracticeAddress.District)
                    udtAuditLogEntry.AddDescripton("Room ", udtPracticeModel.PracticeAddress.Room)
                    udtAuditLogEntry.AddDescripton("Floor ", udtPracticeModel.PracticeAddress.Floor)
                    udtAuditLogEntry.AddDescripton("Block ", udtPracticeModel.PracticeAddress.Block)
                    'udtAuditLogEntry.WriteLog(LogID.LOG00008, "Edit Practice :")
                End If
            Next
            udtAuditLogEntry.WriteStartLog(LogID.LOG00011, "Confirm update MO & Practice")

            For Each udtMOModel As MedicalOrganizationModel In udtServiceProviderModel.MOList.Values
                udtMOModel.CreateBy = udtHCVUUser.UserID
                udtMOModel.UpdateBy = udtHCVUUser.UserID
            Next

            For Each udtPModel As PracticeModel In udtServiceProviderModel.PracticeList.Values
                udtPModel.CreateBy = udtHCVUUser.UserID
                udtPModel.UpdateBy = udtHCVUUser.UserID
            Next

            Dim recordTSMP As Byte() = CType(HttpContext.Current.Session(SPMigrationBLL.SESS_SPMigrationTSMP), Byte())
            blnResult = udtSPMigrationBLL.AddTransitionRecordsAndUpdateSPMigrationRecordStatusT(udtServiceProviderModel.MOList, udtServiceProviderModel.PracticeList, udtServiceProviderModel.HKID, SPMigrationStatus.ReadyToMigrate, udtServiceProviderModel.EnrolRefNo, recordTSMP)
            If blnResult Then
                'udtSPMigrationBLL.UpdateSPMigrationStatus(udtServiceProviderModel.HKID, SPMigrationStatus.ReadyToMigrate, udtServiceProviderModel.EnrolRefNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Confirm update MO & Practice successful")

                udcInfoMessageBox.AddMessage(FunctCode.FUNT010205, "I", "00002")
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                Me.MultiViewSPMigration.ActiveViewIndex = 4
                Me.hfGridviewIndex.Value = String.Empty
            Else
                udcInfoMessageBox.Visible = False
                Me.udcErrorMessage.AddMessage(New SystemMessage(FunctCode.FUNT010205, "E", MsgCode.MSG00001))
                Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00013, "Confirm update MO & Practice fail")
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                udcErrorMessage.AddMessage(SM)
                If udcErrorMessage.GetCodeTable.Rows.Count = 0 Then
                    udcErrorMessage.Visible = False
                Else
                    udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00013, "Confirm update MO & Practice fail.")
                End If
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Confirm update MO & Practice fail")
            Throw ex
        End Try
    End Sub

    Protected Sub btnSpDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Session("FirstBind") = True
        Dim dt As DataTable
        dt = New DataTable
        Dim strOld As String() = {"%s"}
        Dim strNew As String() = {""}
        Dim strMessageCode As String = String.Empty
        Dim blnRes As Boolean = True
        Dim udtSPMigrationBLL As New SPMigrationBLL

        'dt = udtAcctChangeMaintenanceBLL.MaintenanceSearch(hfSearchERN.Value, txtSPID.Text.Trim, _
        '        String.Empty, String.Empty, String.Empty, _
        '        String.Empty, String.Empty)
        'udtSPmodel = udtServiceProviderBLL.GetServiceProviderStagingByERN(dt.Rows.Item(0).Item("Enrolment_Ref_No"), New Database)
        'dt = udtSPMigrationBLL.SPDataMigrationSearch(String.Empty, String.Empty, hfSearchERN.Value, String.Empty)

        'If dt.Rows.Count <> 0 Then

        Session("PracticeChiAddressFilterd") = False

        Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
        Dim udtHCVUUser As HCVUUser.HCVUUserModel
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        udtSPmodel = udtServiceProviderBLL.GetServiceProviderStagingByERN_NoReader(hfSearchERN.Value, New Database)
        If IsNothing(udtSPmodel) Then
            udtSPmodel = udtServiceProviderBLL.GetServiceProviderBySPID(New Database, hfSearchSPID.Value)
        Else
            'Get MO
            'Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
            'udtSPmodel.MOList = udtMOBLL.GetMOListFromStagingByERN(udtSPmodel.EnrolRefNo, udtDB)

            ' Get Practice, Bank, Practice Scheme Information
            Dim udtPracticeBLL As New PracticeBLL
            'udtSPmodel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSPmodel.EnrolRefNo, udtDB)
            udtSPmodel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSPmodel.EnrolRefNo, udtDB)
        End If
        Session("udtTempPracticeModelCollection") = udtSPmodel.PracticeList

        Dim dtSPMigration As DataTable = udtSPMigrationBLL.GetSPMigrationStatus(String.Empty, udtSPmodel.HKID, String.Empty)
        If dtSPMigration.Rows.Count = 0 Then
            spStatus = SPMigrationStatus.NotMigrated
            udtSPMigrationBLL.SPMigrationAdd(udtSPmodel.HKID, SPMigrationStatus.NotMigrated)
            'take TSMP
            dtSPMigration = udtSPMigrationBLL.GetSPMigrationStatus(String.Empty, udtSPmodel.HKID, String.Empty)
            Session(SPMigrationBLL.SESS_SPMigrationTSMP) = CType(dtSPMigration.Rows(0).Item("TSMP"), Byte())
        Else
            spStatus = dtSPMigration.Rows(0).Item("Record_Status").ToString().Trim
            Session(SPMigrationBLL.SESS_SPMigrationTSMP) = CType(dtSPMigration.Rows(0).Item("TSMP"), Byte())
        End If
        Session("MigrationStatus") = spStatus

        '(spStatus.Trim = SPMigrationStatus.NotMigrated Or spStatus.Trim = SPMigrationStatus.ReadyToMigrate) And 
        If (udtSPmodel.RecordStatus <> ServiceProviderStatus.Delisted) Then
            udtSPMigrationBLL.UpdateMOListFromMOMigrationByERN_T(udtSPmodel.EnrolRefNo, udtHCVUUser.UserID, udtSPmodel)
            udtSPMigrationBLL.UpdatePracticeMigrationDetailsByERN_T(udtSPmodel.EnrolRefNo, udtSPmodel.HKID, udtHCVUUser.UserID, udtSPmodel.PracticeList)

            'Remove address code
            If Not IsNothing(udtSPmodel.MOList) Then
                For Each udtMOModel As MedicalOrganizationModel In udtSPmodel.MOList.Values
                    udtMOModel.MOAddress.Address_Code = Nothing
                Next
            End If

            If Not IsNothing(udtSPmodel.PracticeList) Then
                For Each udtPracticeModel As PracticeModel In udtSPmodel.PracticeList.Values
                    udtPracticeModel.PracticeAddress.Address_Code = Nothing
                Next
            End If

            udtServiceProviderBLL.SaveToSession(udtSPmodel)
            blnRes = BindSPSummaryView()

            Me.MultiViewSPMigration.ActiveViewIndex = 2
            udcInfoMessageBox.Clear()
        End If
        'Else
        '    blnRes = False
        'End If

        If Not blnRes Then
            'strMessageCode = "00015"
            'udcInfoMessageBox.AddMessage("990000", "I", strMessageCode, strOld, strNew)
            'udcInfoMessageBox.BuildMessageBox()
            'udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990000, "I", MsgCode.MSG00001))
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            MultiViewSPMigration.ActiveViewIndex = 1
        End If
    End Sub




    Private Function BindSPSummaryView() As Boolean
        Dim udtAuditLogEntry As AuditLogEntry
        udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE, Me)
        Dim blnAbleToBind As Boolean = False
        'Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        'udtAuditLogEntry.AddDescripton("SPID", Me.txtSPID.Text.Trim())

        'If Not Me.txtSPID.Text.Trim.Equals(String.Empty) Then

        'Select Case Session("MigrationStatus")
        '    Case SPMigrationStatus.NotMigrated
        '        GetDescriptionFromDBCode("SPMigrationStatus", "Unprocessed", Me.lblMigrationStatus.Text, String.Empty)
        '        GetDescriptionFromDBCode("SPMigrationStatus", "Unprocessed", Me.lblPRMigrationStatus.Text, String.Empty)
        '    Case SPMigrationStatus.ReadyToMigrate
        '        GetDescriptionFromDBCode("SPMigrationStatus", "Processed", Me.lblMigrationStatus.Text, String.Empty)
        '        GetDescriptionFromDBCode("SPMigrationStatus", "Processed", Me.lblPRMigrationStatus.Text, String.Empty)
        'End Select

        Try
            'if record status = 'P', set to 'R', beacause the record is not completed
            If Not IsNothing(Session("MigrationStatus")) AndAlso Session("MigrationStatus").ToString().Equals(SPMigrationStatus.Processed) Then
                Session("MigrationStatus") = SPMigrationStatus.NotMigrated
            End If

            If Not IsNothing(Session("MigrationStatus")) AndAlso Not Session("MigrationStatus").ToString().Equals(String.Empty) Then
                GetDescriptionFromDBCode(DataMigrationStatus.ClassCode, Session("MigrationStatus").ToString().Trim, Me.lblMigrationStatus.Text, String.Empty)
                GetDescriptionFromDBCode(DataMigrationStatus.ClassCode, Session("MigrationStatus").ToString().Trim, Me.lblPRMigrationStatus.Text, String.Empty)
            End If

            blnAbleToBind = True

            Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
            Dim udtHCVUUser As HCVUUser.HCVUUserModel
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            If blnAbleToBind Then
                If udtServiceProviderBLL.Exist Then
                    Dim udtServiceProviderModel As ServiceProviderModel
                    udtServiceProviderModel = udtServiceProviderBLL.GetSP

                    udtAuditLogEntry.AddDescripton("ERN", udtServiceProviderModel.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderModel.SPID)
                    udtAuditLogEntry.WriteLog(LogID.LOG00005, "Select")

                    'if no MO, add 1 empty MO
                    If udtServiceProviderModel.MOList.Count = 0 Then
                        Dim udtNewMOModel As MedicalOrganizationModel = New MedicalOrganizationModel()
                        Dim udtNewAddress As Address.AddressModel = New Address.AddressModel()

                        udtNewMOModel.EnrolRefNo = udtServiceProviderModel.EnrolRefNo
                        udtNewMOModel.RecordStatus = "A"
                        udtNewMOModel.UpdateBy = udtHCVUUser.UserID
                        udtNewMOModel.CreateBy = udtHCVUUser.UserID
                        udtNewMOModel.CreateDtm = Now
                        udtNewMOModel.UpdateDtm = Now
                        udtNewMOModel.DisplaySeq = 1
                        udtNewMOModel.DisplaySeqMOName = udtNewMOModel.DisplaySeq.Value.ToString + ". < " + Me.GetGlobalResourceObject("Text", "MedicalOrganization") + " >"
                        udtNewMOModel.MOAddress = udtNewAddress
                        udtNewMOModel.SPID = udtServiceProviderModel.SPID

                        udtServiceProviderModel.MOList.Add(udtNewMOModel)
                        udtServiceProviderBLL.SaveToSession(udtServiceProviderModel)
                    End If

                    If udtServiceProviderModel.SPID Is Nothing OrElse udtServiceProviderModel.SPID.Trim() = "" Then
                        Me.buildSpProfileObject(udtServiceProviderModel, TableLocation.Permanent)
                    Else
                        Me.buildSpProfileObject(udtServiceProviderModel, TableLocation.Permanent)
                    End If
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select successful")
                End If
            Else

            End If
            'End If

            Return blnAbleToBind
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00007, "Select fail")
            Throw ex
        End Try

    End Function

    Private Function BindPRSummaryView() As Boolean
        Dim blnAbleToBind As Boolean = False
        'Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE)
        'udtAuditLogEntry.AddDescripton("SPID", Me.txtSPID.Text.Trim())

        'If Not Me.txtSPID.Text.Trim.Equals(String.Empty) Then

        blnAbleToBind = True

        If blnAbleToBind Then
            If udtServiceProviderBLL.Exist Then
                Dim udtServiceProviderModel As ServiceProviderModel
                udtServiceProviderModel = udtServiceProviderBLL.GetSP
                'udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderModel.SPID)
                'udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00005, "Select:")

                If udtServiceProviderModel.SPID Is Nothing OrElse udtServiceProviderModel.SPID.Trim() = "" Then
                    Me.buildSpReviewProfileObject(udtServiceProviderModel, TableLocation.Permanent)
                Else
                    Me.buildSpReviewProfileObject(udtServiceProviderModel, TableLocation.Permanent)
                End If

            End If
        Else

        End If
        'End If

        Return blnAbleToBind
    End Function




    Public Sub buildSpReviewProfileObject(ByVal udtSP As ServiceProviderModel, ByVal strTableLocation As String)
        ' Hidden fields for later usage
        hfPRERN.Value = udtSP.EnrolRefNo
        hfPRTableLocation.Value = strTableLocation

        ' Enrolment Reference No.
        lblPRERN.Text = udtFormatter.formatSystemNumber(udtSP.EnrolRefNo)

        If udtSP.SPID.Trim.Equals(String.Empty) Then
            ' Service Provider ID
            lblPRSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' Effective Date / Submission Time
            'lblPRDateText.Text = Me.GetGlobalResourceObject("Text", "SubmissionDtmTime")
            'lblPRDate.Text = udtFormatter.convertDateTime(udtSP.EnrolDate)

        Else
            ' Service Provider ID
            'lblPRSPID.Text = udtSP.SPID + IIf(udtSP.UnderModification.Equals(String.Empty), "", " (Under Amendment)")
            lblPRSPID.Text = udtSP.SPID
            ' Effective Date / Request Time
            'Select Case strTableLocation
            '    Case TableLocation.Permanent
            '        lblDateText.Text = Me.GetGlobalResourceObject("Text", "EffectiveDate")
            '        lblDate.Text = udtFormatter.convertDateTime(udtSP.EffectiveDtm)

            '    Case TableLocation.Staging
            '        lblDateText.Text = Me.GetGlobalResourceObject("Text", "RequestTime")
            '        lblDate.Text = udtFormatter.convertDateTime(udtSP.CreateDtm)
            '
            'End Select

        End If

        ' Name
        lblPREname.Text = udtSP.EnglishName
        lblPRCname.Text = udtFormatter.formatChineseName(udtSP.ChineseName)

        ' HKIC No.
        lblPRHKID.Text = udtFormatter.formatHKID(udtSP.HKID, False)

        ' Correspondence Address
        lblPRAddress.Text = udtFormatter.formatAddress(udtSP.SpAddress.Room, udtSP.SpAddress.Floor, udtSP.SpAddress.Block, _
                                                        udtSP.SpAddress.Building, udtSP.SpAddress.District, udtSP.SpAddress.AreaCode)

        ' Email Address
        lblPREmail.Text = udtSP.Email
        imgPREditEmail.Visible = IIf(strTableLocation = TableLocation.Permanent AndAlso udtSP.EmailChanged = EmailChanged.Changed, True, False)

        ' Daytime Contact Phone No.
        lblPRContactNo.Text = udtSP.Phone

        ' Fax No.
        If udtSP.Fax.Trim.Equals(String.Empty) Then
            lblPRFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
        Else
            lblPRFax.Text = udtSP.Fax
        End If

        'Submitted via
        'subitted via
        If udtSP.SubmitMethod.Trim.Equals(SubmitChannel.Paper) Then
            lblPRSubmittedVia.Text = "Paper"
        ElseIf udtSP.SubmitMethod.Trim.Equals(SubmitChannel.Electronic) Then
            lblPRSubmittedVia.Text = "Electronic"
        End If

        'Service Provider Status - Complicated!!
        Select Case strTableLocation
            Case TableLocation.Permanent
                'Status.GetDescriptionFromDBCode(SPMaintenanceDisplayStatus.ClassCode, udtSP.RecordStatus, lblPRSPStatus.Text, String.Empty)

            Case TableLocation.Staging
                Dim udtSPAccUpdate As SPAccountUpdateModel

                If udtSPAccountUpateBLL.Exist Then
                    udtSPAccUpdate = udtSPAccountUpateBLL.GetSPAccountUpdate
                Else
                    udtSPAccUpdate = udtSPAccountUpateBLL.GetSPAccountUpdateByERN(udtSP.EnrolRefNo, New Database)

                    If IsNothing(udtSPAccUpdate) Then
                        udtSPAccUpdate = New SPAccountUpdateModel
                    End If
                End If

                If IsNothing(udtSPAccUpdate) _
                        OrElse IsNothing(udtSPAccUpdate.ProgressStatus) _
                        OrElse udtSPAccUpdate.ProgressStatus.Trim.Equals(String.Empty) Then
                    udtSPAccUpdate.ProgressStatus = SPAccountUpdateProgressStatus.DataEntryStage
                End If

                Dim strProgressStatus As String = String.Empty
                Status.GetDescriptionFromDBCode(SPAccountUpdateProgressStatus.ClassCode, udtSPAccUpdate.ProgressStatus, strProgressStatus, String.Empty)

                Dim strVerificationStatus As String = String.Empty

                Select Case udtSPAccUpdate.ProgressStatus
                    Case SPAccountUpdateProgressStatus.DataEntryStage
                        'lblPRRecordStatus.Text = strProgressStatus

                    Case SPAccountUpdateProgressStatus.VettingStage
                        Dim udtSPVerificationModel As ServiceProviderVerificationModel

                        If udtSPVerificationBLL.Exist Then
                            udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
                        Else
                            udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(udtSP.EnrolRefNo, New Database)
                        End If

                        Status.GetDescriptionFromDBCode(ServiceProviderVerificationStatus.ClassCode, udtSPVerificationModel.RecordStatus, strVerificationStatus, String.Empty)

                        'lblRecordStatus.Text = strVerificationStatus + "(" + strProgressStatus + ")"

                    Case SPAccountUpdateProgressStatus.BankAcctVerification
                        Dim dtBankVer As DataTable = udtSearchEngineBLL.SearchBankTSMP(udtSP.EnrolRefNo)

                        If dtBankVer.Rows.Count = 1 Then
                            strVerificationStatus = dtBankVer.Rows(0).Item("bankStatus")
                            'lblRecordStatus.Text = strVerificationStatus + "(" + strProgressStatus + ")"
                        Else
                            'lblRecordStatus.Text = strProgressStatus
                        End If

                    Case SPAccountUpdateProgressStatus.ProfessionalVerification, SPAccountUpdateProgressStatus.WaitingForIssueToken
                        'lblRecordStatus.Text = strProgressStatus

                    Case Else
                        'lblRecordStatus.Text = strProgressStatus

                End Select

                'If udtSP.SPID.Equals(String.Empty) Then
                '    lblPRSPStatusText.Visible = True
                '    lblPRSPStatus.Visible = True
                '    lblPRSPStatus.Text = "New Enrolment (" + strProgressStatus + ")"
                'Else
                '    lblPRSPStatusText.Visible = False
                '    lblPRSPStatus.Visible = False
                'End If

                '    Case TableLocation.Enrolment
                '        lblPRSPStatus.Text = "Unprocessed"

        End Select

        ' Scheme Information
        'gvEnrolledScheme.DataSource = udtSP.SchemeInfoList.Values
        'gvEnrolledScheme.DataBind()

        'If strTableLocation = TableLocation.Permanent Then
        '    '    Dim strRecordStatus As String = udtSP.RecordStatus.Trim

        '    '     If the SP is delisted, show the Token Return Date
        '    '    If strRecordStatus = SPMaintenanceDisplayStatus.DelistedInvoluntary _
        '    '            OrElse strRecordStatus = SPMaintenanceDisplayStatus.DelistedVoluntary _
        '    '            OrElse strRecordStatus = SPMaintenanceDisplayStatus.SPPendingDelist Then
        '    '        If udtSP.TokenReturnDtm = String.Empty Then
        '    '            lblPRTokenReturn.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '    '        Else
        '    '            lblPRTokenReturn.Text = udtSP.TokenReturnDtm
        '    '        End If

        '    '        lblPRTokenReturnText.Visible = True
        '    '        lblPRTokenReturn.Visible = True

        '    '    Else
        '    '        lblPRTokenReturnText.Visible = False
        '    '        lblPRTokenReturn.Visible = False

        '    '    End If

        '    ' Token Serial No.
        '    lblPRTokenSN.Text = udtSPProfileBLL.GetTokenTokenSerialNoBySPID(udtSP.SPID)

        '    Dim udtTokenModel As TokenModel = udtSPProfileBLL.GetTokenModelBySPID(udtSP.SPID)

        '    If IsNothing(udtTokenModel) OrElse udtTokenModel.TokenSerialNoReplacement.Equals(String.Empty) Then
        '        lblPRTokenReplacedSNText.Visible = False
        '        lblPRTokenReplacedSN.Visible = False
        '    Else
        '        lblPRTokenReplacedSNText.Visible = True
        '        lblPRTokenReplacedSN.Visible = True
        '        lblPRTokenReplacedSN.Text = udtTokenModel.TokenSerialNoReplacement.Trim
        '    End If

        'Else
        '    ' In Staging and Enrolment tables, the following 4 items do not need to show
        '    lblPRTokenSNText.Visible = False
        '    lblPRTokenSN.Visible = False

        '    lblPRTokenReplacedSNText.Visible = False
        '    lblPRTokenReplacedSN.Visible = False
        'End If


        '' Scheme Information
        'gvPREnrolledScheme.DataSource = udtSP.SchemeInfoList.Values
        'gvPREnrolledScheme.DataBind()


        ' Medical Organization Information
        If udtSP.MOList.Values.Count = 0 Then
            lblMONA.Visible = True
            gvPRMO.Visible = False
        Else
            gvPRMO.DataSource = udtSP.MOList.Values
            gvPRMO.DataBind()
            lblMONA.Visible = False
            gvPRMO.Visible = True
        End If

        'check whether all practice are delisted
        Dim blnAllPracticeDislisted As Boolean = True
        If Not IsNothing(udtSP.PracticeList) Then
            For Each udtPracticeModel As PracticeModel In udtSP.PracticeList.Values
                If udtPracticeModel.RecordStatus <> PracticeStatus.Delisted Then
                    blnAllPracticeDislisted = False
                End If
            Next
        End If

        If blnAllPracticeDislisted Then
            gvPRPracticeBank.Visible = False
            lblPRPracticeBankInfo.Visible = False
            panPRPracticeBankInfo.Visible = False
        Else
            gvPRPracticeBank.Visible = True
            lblPRPracticeBankInfo.Visible = True
            panPRPracticeBankInfo.Visible = True
            ' Practice and Bank Information
            gvPRPracticeBank.DataSource = udtSP.PracticeList.Values
            gvPRPracticeBank.DataBind()
        End If

    End Sub

    Public Sub buildSpProfileObject(ByVal udtSP As ServiceProviderModel, ByVal strTableLocation As String)
        ' Hidden fields for later usage
        hfERN.Value = udtSP.EnrolRefNo
        hfTableLocation.Value = strTableLocation

        ' Enrolment Reference No.
        lblERN.Text = udtFormatter.formatSystemNumber(udtSP.EnrolRefNo)

        If udtSP.SPID.Trim.Equals(String.Empty) Then
            ' Service Provider ID
            lblSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")

            ' Effective Date / Submission Time
            'lblDateText.Text = Me.GetGlobalResourceObject("Text", "SubmissionDtmTime")
            'lblDate.Text = udtFormatter.convertDateTime(udtSP.EnrolDate)

        Else
            ' Service Provider ID
            'lblSPID.Text = udtSP.SPID + IIf(udtSP.UnderModification.Equals(String.Empty), "", " (Under Amendment)")
            lblSPID.Text = udtSP.SPID
            ' Effective Date / Request Time
            'Select Case strTableLocation
            '    Case TableLocation.Permanent
            '        'lblDateText.Text = Me.GetGlobalResourceObject("Text", "EffectiveDate")
            '        'lblDate.Text = udtFormatter.convertDateTime(udtSP.EffectiveDtm)

            '    Case TableLocation.Staging
            '        'lblDateText.Text = Me.GetGlobalResourceObject("Text", "RequestTime")
            '        'lblDate.Text = udtFormatter.convertDateTime(udtSP.CreateDtm)

            'End Select

        End If

        'subitted via
        If udtSP.SubmitMethod.Trim.Equals(SubmitChannel.Paper) Then
            lblSubmittedVia.Text = "Paper"
        ElseIf udtSP.SubmitMethod.Trim.Equals(SubmitChannel.Electronic) Then
            lblSubmittedVia.Text = "Electronic"
        End If

        ' Name
        lblEname.Text = udtSP.EnglishName
        lblCname.Text = udtFormatter.formatChineseName(udtSP.ChineseName)

        ' HKIC No.
        lblHKID.Text = udtFormatter.formatHKID(udtSP.HKID, False)

        ' Correspondence Address
        lblAddress.Text = udtFormatter.formatAddress(udtSP.SpAddress.Room, udtSP.SpAddress.Floor, udtSP.SpAddress.Block, _
                                                        udtSP.SpAddress.Building, udtSP.SpAddress.District, udtSP.SpAddress.AreaCode)

        ' Email Address
        lblEmail.Text = udtSP.Email
        imgEditEmail.Visible = IIf(strTableLocation = TableLocation.Permanent AndAlso udtSP.EmailChanged = EmailChanged.Changed, True, False)

        ' Daytime Contact Phone No.
        lblContactNo.Text = udtSP.Phone

        ' Fax No.
        If udtSP.Fax.Trim.Equals(String.Empty) Then
            lblFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
        Else
            lblFax.Text = udtSP.Fax
        End If


        ' Service Provider Status - Complicated!!
        Select Case strTableLocation
            Case TableLocation.Permanent
                'Status.GetDescriptionFromDBCode(SPMaintenanceDisplayStatus.ClassCode, udtSP.RecordStatus, lblSPStatus.Text, String.Empty)

            Case TableLocation.Staging
                Dim udtSPAccUpdate As SPAccountUpdateModel

                If udtSPAccountUpateBLL.Exist Then
                    udtSPAccUpdate = udtSPAccountUpateBLL.GetSPAccountUpdate
                Else
                    udtSPAccUpdate = udtSPAccountUpateBLL.GetSPAccountUpdateByERN(udtSP.EnrolRefNo, New Database)

                    If IsNothing(udtSPAccUpdate) Then
                        udtSPAccUpdate = New SPAccountUpdateModel
                    End If
                End If

                If IsNothing(udtSPAccUpdate) _
                        OrElse IsNothing(udtSPAccUpdate.ProgressStatus) _
                        OrElse udtSPAccUpdate.ProgressStatus.Trim.Equals(String.Empty) Then
                    udtSPAccUpdate.ProgressStatus = SPAccountUpdateProgressStatus.DataEntryStage
                End If

                Dim strProgressStatus As String = String.Empty
                Status.GetDescriptionFromDBCode(SPAccountUpdateProgressStatus.ClassCode, udtSPAccUpdate.ProgressStatus, strProgressStatus, String.Empty)

                Dim strVerificationStatus As String = String.Empty

                Select Case udtSPAccUpdate.ProgressStatus
                    Case SPAccountUpdateProgressStatus.DataEntryStage
                        'lblRecordStatus.Text = strProgressStatus

                    Case SPAccountUpdateProgressStatus.VettingStage
                        Dim udtSPVerificationModel As ServiceProviderVerificationModel

                        If udtSPVerificationBLL.Exist Then
                            udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
                        Else
                            udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(udtSP.EnrolRefNo, New Database)
                        End If

                        Status.GetDescriptionFromDBCode(ServiceProviderVerificationStatus.ClassCode, udtSPVerificationModel.RecordStatus, strVerificationStatus, String.Empty)

                        'lblRecordStatus.Text = strVerificationStatus + "(" + strProgressStatus + ")"

                    Case SPAccountUpdateProgressStatus.BankAcctVerification
                        Dim dtBankVer As DataTable = udtSearchEngineBLL.SearchBankTSMP(udtSP.EnrolRefNo)

                        If dtBankVer.Rows.Count = 1 Then
                            strVerificationStatus = dtBankVer.Rows(0).Item("bankStatus")
                            'lblRecordStatus.Text = strVerificationStatus + "(" + strProgressStatus + ")"
                        Else
                            'lblRecordStatus.Text = strProgressStatus
                        End If

                    Case SPAccountUpdateProgressStatus.ProfessionalVerification, SPAccountUpdateProgressStatus.WaitingForIssueToken
                        'lblRecordStatus.Text = strProgressStatus

                    Case Else
                        'lblRecordStatus.Text = strProgressStatus

                End Select

                'If udtSP.SPID.Equals(String.Empty) Then
                '    lblSPStatusText.Visible = True
                '    lblSPStatus.Visible = True
                '    lblSPStatus.Text = "New Enrolment (" + strProgressStatus + ")"
                'Else
                '    lblSPStatusText.Visible = False
                '    lblSPStatus.Visible = False
                'End If

            Case TableLocation.Enrolment
                'lblSPStatus.Text = "Unprocessed"

        End Select

        ' Scheme Information
        'gvEnrolledScheme.DataSource = udtSP.SchemeInfoList.Values
        'gvEnrolledScheme.DataBind()

        'If strTableLocation = TableLocation.Permanent Then
        '    Dim strRecordStatus As String = udtSP.RecordStatus.Trim

        '    ' If the SP is delisted, show the Token Return Date
        '    If strRecordStatus = SPMaintenanceDisplayStatus.DelistedInvoluntary _
        '            OrElse strRecordStatus = SPMaintenanceDisplayStatus.DelistedVoluntary _
        '            OrElse strRecordStatus = SPMaintenanceDisplayStatus.SPPendingDelist Then
        '        If udtSP.TokenReturnDtm = String.Empty Then
        '            lblTokenReturn.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblTokenReturn.Text = udtSP.TokenReturnDtm
        '        End If

        '        lblTokenReturnText.Visible = True
        '        lblTokenReturn.Visible = True

        '    Else
        '        lblTokenReturnText.Visible = False
        '        lblTokenReturn.Visible = False

        '    End If

        ' Token Serial No.
        'lblTokenSN.Text = udtSPProfileBLL.GetTokenTokenSerialNoBySPID(udtSP.SPID)

        '    Dim udtTokenModel As TokenModel = udtSPProfileBLL.GetTokenModelBySPID(udtSP.SPID)

        '    If IsNothing(udtTokenModel) OrElse udtTokenModel.TokenSerialNoReplacement.Equals(String.Empty) Then
        '        'lblTokenReplacedSNText.Visible = False
        '        'lblTokenReplacedSN.Visible = False
        '    Else
        '        'lblTokenReplacedSNText.Visible = True
        '        'lblTokenReplacedSN.Visible = True
        '        'lblTokenReplacedSN.Text = udtTokenModel.TokenSerialNoReplacement.Trim
        '    End If

        '    ' Web Account Username
        '    If udtSP.AliasAccount.Equals(String.Empty) Then
        '        'lblSPUsernameText.Visible = False
        '        'lblSPUsername.Visible = False
        '    Else
        '        'lblSPUsernameText.Visible = True
        '        'lblSPUsername.Visible = True
        '        'lblSPUsername.Text = udtSP.AliasAccount
        '    End If

        'Else
        '    ' In Staging and Enrolment tables, the following 4 items do not need to show
        '    'lblTokenSNText.Visible = False
        '    'lblTokenSN.Visible = False

        '    'lblTokenReplacedSNText.Visible = False
        '    'lblTokenReplacedSN.Visible = False

        '    lblTokenReturnText.Visible = False
        '    lblTokenReturn.Visible = False

        '    'lblSPUsernameText.Visible = False
        '    'lblSPUsername.Visible = False

        'End If

        ' Medical Organization Information
        If udtSP.MOList.Values.Count = 0 Then
            lblMONA.Visible = True
            gvMO.Visible = False
        Else
            gvMO.DataSource = udtSP.MOList.Values
            gvMO.DataBind()
            lblMONA.Visible = False
            gvMO.Visible = True
        End If

        'check whether all practice are delisted
        Dim blnAllPracticeDislisted As Boolean = True
        If Not IsNothing(udtSP.PracticeList) Then
            For Each udtPracticeModel As PracticeModel In udtSP.PracticeList.Values
                If udtPracticeModel.RecordStatus <> PracticeStatus.Delisted Then
                    blnAllPracticeDislisted = False
                End If
            Next
        End If

        If blnAllPracticeDislisted Then
            gvPracticeBank.Visible = False
            lblPracticeBankInfo.Visible = False
            panPracticeBankInfo.Visible = False
        Else
            gvPracticeBank.Visible = True
            lblPracticeBankInfo.Visible = True
            panPracticeBankInfo.Visible = True
            ' Practice and Bank Information
            gvPracticeBank.DataSource = udtSP.PracticeList.Values
            gvPracticeBank.DataBind()
        End If



        'show reprint button
        'If spStatus.Trim = "R" Then
        '    Me.ibtnReprintButton.Visible = True
        'Else
        '    Me.ibtnReprintButton.Visible = False
        'End If
    End Sub





    Protected Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, "MigrationResult")
    End Sub

    Protected Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        Me.GridViewPreRenderHandler(sender, e, "MigrationResult")
    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' Enrolment Reference No.
            Dim lnkbtnERN As LinkButton = CType(e.Row.FindControl("lnkbtnERN"), LinkButton)
            lnkbtnERN.Text = udtFormatter.formatSystemNumber(lnkbtnERN.CommandArgument.Trim)
            lnkbtnERN.OnClientClick = "javascript:getERN('" & lnkbtnERN.CommandArgument.Trim & "','" & CType(e.Row.FindControl("lblRSPHKID"), Label).Text & "','" & CType(e.Row.FindControl("lnkbtnRSPID"), LinkButton).Text & "')"
            lnkbtnERN.Attributes.Add("onclick", "return false;")

            ' Service Provider ID
            Dim lnkbtnRSPID As LinkButton = CType(e.Row.FindControl("lnkbtnRSPID"), LinkButton)
            If lnkbtnRSPID.Text.Trim.Equals(String.Empty) Then
                lnkbtnRSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lnkbtnRSPID.CommandArgument = String.Empty
                lnkbtnRSPID.Enabled = False
            Else
                lnkbtnRSPID.OnClientClick = "javascript:getERN('" & lnkbtnERN.CommandArgument.Trim & "','" & CType(e.Row.FindControl("lblRSPHKID"), Label).Text & "','" & CType(e.Row.FindControl("lnkbtnRSPID"), LinkButton).Text & "')"
                lnkbtnRSPID.Attributes.Add("onclick", "return false;")
            End If

            ' Service Provider HKIC No.
            Dim lblRSPHKID As Label = CType(e.Row.FindControl("lblRSPHKID"), Label)
            lblRSPHKID.Text = udtFormatter.formatHKID(lblRSPHKID.Text, False)

            ' Service Provider Name
            Dim lblRCname As Label = CType(e.Row.FindControl("lblRCname"), Label)
            lblRCname.Text = udtFormatter.formatChineseName(lblRCname.Text.Trim)

            ' Status
            Dim lblRStatus As Label = CType(e.Row.FindControl("lblRStatus"), Label)

            'Select Case lblRStatus.Text.Trim
            '    Case SPMigrationStatus.NotMigrated
            '        GetDescriptionFromDBCode("SPMigrationStatus", "Unprocessed", lblRStatus.Text, String.Empty)
            '    Case SPMigrationStatus.ReadyToMigrate
            '        GetDescriptionFromDBCode("SPMigrationStatus", "Processed", lblRStatus.Text, String.Empty)
            'End Select
            GetDescriptionFromDBCode(DataMigrationStatus.ClassCode, lblRStatus.Text.Trim, lblRStatus.Text, String.Empty)
        End If
    End Sub

    Protected Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, "MigrationResult")
    End Sub

    Protected Sub gvMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMO.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim lblMOStatus As Label = CType(e.Row.FindControl("lblMOStatus"), Label)
            Dim rboEditMORelation As RadioButtonList = e.Row.FindControl("rboEditMORelation")
            Dim txtEditMORelationRemark As TextBox = e.Row.FindControl("txtEditMORelationRemark")
            Dim ddlMOEditDistrict As DropDownList = CType(e.Row.FindControl("ddlMOEditDistrict"), DropDownList)
            'Dim rboMOEditArea As RadioButtonList = CType(e.Row.FindControl("rboMOEditArea"), RadioButtonList)
            Dim txtMOEName As TextBox = CType(e.Row.FindControl("txtMOEName"), TextBox)
            Dim ibtnDeleteMO As ImageButton = CType(e.Row.FindControl("ibtnDeleteMO"), ImageButton)

            Dim hfMOEditDistrict As HiddenField = CType(e.Row.FindControl("hfMOEditDistrict"), HiddenField)
            'Dim hfMOEditArea As HiddenField = CType(e.Row.FindControl("hfMOEditArea"), HiddenField)
            Dim hfEditMORelation As HiddenField = e.Row.FindControl("hfEditMORelation")

            'Status.GetDescriptionFromDBCode(MedicalOrganizationStatus.ClassCode, lblMOStatus.Text.Trim, lblMOStatus.Text, String.Empty)

            rboEditMORelation.DataSource = udtSPProfileBLL.GetPracticeType
            rboEditMORelation.DataValueField = "ItemNo"
            rboEditMORelation.DataTextField = "DataValue"
            rboEditMORelation.DataBind()
            rboEditMORelation.SelectedValue = hfEditMORelation.Value.Trim

            rboEditMORelation.Attributes.Add("onclick", "javascript:enableRemarkTextbox('" + rboEditMORelation.ClientID + "', '" + txtEditMORelationRemark.ClientID + "')")

            If hfEditMORelation.Value.Trim.Equals("O") Then
                txtEditMORelationRemark.BackColor = Nothing
                txtEditMORelationRemark.Attributes.Remove("readonly")
            Else
                txtEditMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtEditMORelationRemark.Attributes.Add("readonly", "readonly")
            End If


            ''Area
            'rboMOEditArea.DataSource = udtSPProfileBLL.GetArea.Values 'AreaBLL.GetAreaList.Values
            'rboMOEditArea.DataValueField = "Area_ID"
            'rboMOEditArea.DataTextField = "Area_Name"
            'rboMOEditArea.DataBind()
            'If hfMOEditArea.Value.Equals(String.Empty) Then
            'Else
            '    rboMOEditArea.SelectedValue = hfMOEditArea.Value.Trim
            If Not hfMOEditDistrict.Value.Trim.Equals("") Then
                ddlMOEditDistrict.SelectedValue = hfMOEditDistrict.Value.Trim
            End If
            'End If

            'district
            bindDistrict(ddlMOEditDistrict, String.Empty, False)

            'Disable MO delete button
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            If udtSP.MOList.Count = 1 Then
                'ibtnDeleteMO.Visible = False
                ibtnDeleteMO.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                ibtnDeleteMO.Enabled = False
            Else
                'ibtnDeleteMO.Visible = True
                ibtnDeleteMO.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DeleteSBtn")
                ibtnDeleteMO.Enabled = True
            End If

            'rboMOEditArea.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            ddlMOEditDistrict.Attributes.Add("onfocus", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            txtMOEName.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            txtMOEName.Attributes.Add("onfocus", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            ibtnDeleteMO.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
        End If
    End Sub

    Protected Sub gvPRMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPRMO.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim lblMOStatus As Label = CType(e.Row.FindControl("lblPRMOStatus"), Label)

            'Status.GetDescriptionFromDBCode(MedicalOrganizationStatus.ClassCode, lblMOStatus.Text.Trim, lblMOStatus.Text, String.Empty)

        End If
    End Sub


    Protected Sub gvPracticeBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPracticeBank.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Convert Medical Organization No. to Medical Organization Name
            'Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblPracticeMO"), Label)
            Dim txtPracticeChiName As TextBox = CType(e.Row.FindControl("txtPracticeChiName"), TextBox)
            Dim txtPracticeChiAddress As TextBox = CType(e.Row.FindControl("txtPracticeChiAddress"), TextBox)
            Dim txtPracticePhone As TextBox = CType(e.Row.FindControl("txtPracticePhone"), TextBox)
            Dim ibtnPracticeChiNameHelp As LinkButton = CType(e.Row.FindControl("ibtnPracticeChiNameHelp"), LinkButton)
            Dim ibtnPracticePhoneHelp As LinkButton = CType(e.Row.FindControl("ibtnPracticePhoneHelp"), LinkButton)
            Dim ibtnPracticeChiAddressHelp As LinkButton = CType(e.Row.FindControl("ibtnPracticeChiAddressHelp"), LinkButton)
            'Dim intMODisplaySeq As Integer = CInt(lblPracticeMO.Text.Trim)
            Dim ddlPracticeMO As DropDownList = CType(e.Row.FindControl("ddlPracticeMO"), DropDownList)
            Dim ddlPracticeEditDistrict As DropDownList = CType(e.Row.FindControl("ddlPracticeEditDistrict"), DropDownList)
            'Dim rboPracticeEditArea As RadioButtonList = CType(e.Row.FindControl("rboPracticeEditArea"), RadioButtonList)

            'Dim hfPracticeEditArea As HiddenField = CType(e.Row.FindControl("hfPracticeEditArea"), HiddenField)
            Dim hfPracticeEditDistrict As HiddenField = CType(e.Row.FindControl("hfPracticeEditDistrict"), HiddenField)
            Dim hfPracticeMOSeq As HiddenField = CType(e.Row.FindControl("hfPracticeMOSeq"), HiddenField)
            Dim hfPracticeDisplaySeq As HiddenField = CType(e.Row.FindControl("hfPracticeDisplaySeq"), HiddenField)

            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

            'For Each udtMO As MedicalOrganization.MedicalOrganizationModel In udtSP.MOList.Values
            '    If udtMO.DisplaySeq.Value = intMODisplaySeq Then
            '        lblPracticeMO.Text = udtMO.DisplaySeqMOName 'udtMO.MOEngName
            '        Exit For
            '    End If
            'Next

            ' If there is no schemes, show "N/A"
            'Dim gvPracticeSchemeInfo As GridView = e.Row.FindControl("gvPracticeSchemeInfo")
            'If gvPracticeSchemeInfo.Rows.Count = 0 Then
            '    CType(e.Row.FindControl("lblPracticeSchemeInfoNA"), Label).Visible = True
            '    gvPracticeSchemeInfo.Visible = False
            'End If

            ' If there is no banks, show "N/A"
            'If CType(e.Row.FindControl("lblBankName"), Label).Text = String.Empty Then
            '    CType(e.Row.FindControl("pnlBankNA"), Panel).Visible = True
            'Else
            '    CType(e.Row.FindControl("pnlBank"), Panel).Visible = True
            'End If


            'Medical organization selection
            Dim DDLlist As SortedList = New SortedList()

            If udtSP.MOList.Count > 1 Then
                DDLlist.Add(0, Me.GetGlobalResourceObject("Text", "SelectMO"))
                ddlPracticeMO.Enabled = True
            Else
                ddlPracticeMO.Enabled = False
            End If

            Dim strSelectedDisplaySeqMOName As String = String.Empty
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                DDLlist.Add(udtMO.DisplaySeq, udtMO.DisplaySeqMOName)
                If udtMO.DisplaySeq = hfPracticeMOSeq.Value.Trim Then
                    strSelectedDisplaySeqMOName = udtMO.DisplaySeq
                End If
            Next

            If ddlPracticeMO.SelectedIndex = -1 Or ddlPracticeMO.SelectedIndex = 1 Then
                ddlPracticeMO.DataSource = DDLlist
                ddlPracticeMO.DataTextField = "value"
                ddlPracticeMO.DataValueField = "key"
                ddlPracticeMO.DataBind()

                If strSelectedDisplaySeqMOName.Trim <> String.Empty Then
                    ddlPracticeMO.SelectedValue = strSelectedDisplaySeqMOName
                End If
            Else
                ddlPracticeMO.DataSource = DDLlist
                ddlPracticeMO.DataTextField = "value"
                ddlPracticeMO.DataValueField = "key"
                ddlPracticeMO.DataBind()

                ddlPracticeMO.SelectedIndex = 0
            End If

            'Area
            'rboPracticeEditArea.DataSource = udtSPProfileBLL.GetArea.Values 'AreaBLL.GetAreaList.Values
            'rboPracticeEditArea.DataValueField = "Area_ID"
            'rboPracticeEditArea.DataTextField = "Area_Name"
            'rboPracticeEditArea.DataBind()
            'If hfPracticeEditArea.Value.Equals(String.Empty) Then
            'Else
            '    rboPracticeEditArea.SelectedValue = hfPracticeEditArea.Value.Trim
            If Not hfPracticeEditDistrict.Value.Trim.Equals("") Then
                ddlPracticeEditDistrict.SelectedValue = hfPracticeEditDistrict.Value.Trim
            End If
            'End If

            'district
            bindDistrict(ddlPracticeEditDistrict, String.Empty, False)

            ddlPracticeEditDistrict.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            'rboPracticeEditArea.Attributes.Add("onfocus", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")


            'Filtering
            Dim dr As PracticeModel = CType(e.Row.DataItem, PracticeModel)
            Dim lblPracticeStatus As Label
            Dim strPracticeStatus As String = String.Empty

            lblPracticeStatus = CType(e.Row.FindControl("lblPracticeStatus"), Label)
            strPracticeStatus = dr.RecordStatus

            Select Case strPracticeStatus
                Case PracticeStatus.Delisted
                    e.Row.Visible = False
            End Select

            txtPracticeChiName.Attributes.Add("onclick", "javascript:getPracticeGridviewIndex('" & e.Row.RowIndex & "');")
            ibtnPracticeChiNameHelp.Attributes.Add("onclick", "javascript:getPracticeGridviewIndex('" & e.Row.RowIndex & "');")
            ibtnPracticePhoneHelp.Attributes.Add("onclick", "javascript:getPracticeGridviewIndex('" & e.Row.RowIndex & "');")
            ibtnPracticeChiAddressHelp.Attributes.Add("onclick", "javascript:getPracticeGridviewIndex('" & e.Row.RowIndex & "');")

            Dim udtSPMigrationBLL As New SPMigrationBLL


            'show reference 
            If Session("FirstBind") Then
                Dim tmpdt As DataTable
                tmpdt = udtSPMigrationBLL.GetPracticeReferenceT(udtSP.EnrolRefNo)
                If tmpdt.Rows.Count = 0 Then
                    ibtnPracticeChiNameHelp.Visible = False
                    ibtnPracticePhoneHelp.Visible = False
                    Session(ibtnPracticeChiNameHelp.ClientID) = False
                    Session(ibtnPracticePhoneHelp.ClientID) = False
                Else
                    Dim blnChiNameExisted As Boolean = False
                    Dim blnPhoneExisted As Boolean = False
                    For Each drp As DataRow In tmpdt.Rows
                        If Not IsNothing(drp("practice_name_chi")) AndAlso Not drp("practice_name_chi").ToString.Trim.Equals(String.Empty) Then
                            blnChiNameExisted = True
                        End If
                        If Not IsNothing(drp("phone_daytime")) AndAlso Not drp("phone_daytime").ToString.Trim.Equals(String.Empty) Then
                            blnPhoneExisted = True
                        End If
                    Next

                    If txtPracticeChiName.Text <> "" Then
                        ibtnPracticeChiNameHelp.Visible = False
                        Session(ibtnPracticeChiNameHelp.ClientID) = False
                    Else
                        If blnChiNameExisted Then
                            ibtnPracticeChiNameHelp.Visible = True
                            Session(ibtnPracticeChiNameHelp.ClientID) = True
                        Else
                            ibtnPracticeChiNameHelp.Visible = False
                            Session(ibtnPracticeChiNameHelp.ClientID) = False
                        End If
                    End If

                    If txtPracticePhone.Text <> "" Then
                        ibtnPracticePhoneHelp.Visible = False
                        Session(ibtnPracticePhoneHelp.ClientID) = False
                    Else
                        If blnPhoneExisted Then
                            ibtnPracticePhoneHelp.Visible = True
                            Session(ibtnPracticePhoneHelp.ClientID) = True
                        Else
                            ibtnPracticePhoneHelp.Visible = False
                            Session(ibtnPracticePhoneHelp.ClientID) = False
                        End If
                    End If
                End If
            Else
                ibtnPracticePhoneHelp.Visible = Session(ibtnPracticePhoneHelp.ClientID)
                ibtnPracticeChiNameHelp.Visible = Session(ibtnPracticeChiNameHelp.ClientID)
            End If

            If Session("FirstBind") Then
                'If Session("MigrationStatus") = SPMigrationStatus.NotMigrated Then
                'handling Practice Chinesse address (complicated!)
                'If udtSPMigrationBLL.PracticeMigrationGetByERNDisplaySeq(udtSP.EnrolRefNo, hfPracticeDisplaySeq.Value, New Database).Rows.Count = 0 Then
                '    'Handle case WITHOUT migration records in PracticeMigration table
                '    ibtnPracticeChiAddressHelp.Visible = False

                '    If Not txtPracticeChiAddress.Text.Trim.Equals(String.Empty) Then
                '        ibtnPracticeChiAddressHelp.Visible = False
                '        Session(ibtnPracticeChiAddressHelp.ClientID) = False
                '    Else
                '        ibtnPracticeChiAddressHelp.Visible = True
                '        Session(ibtnPracticeChiAddressHelp.ClientID) = True
                '    End If

                '    txtPracticeChiAddress.Text = ""
                'Else
                'Handle case WITH migration records in PracticeMigration table

                'check existing practice chi address
                Dim blnExisted As Boolean = False
                For Each udtExistingPracticeModel As PracticeModel In CType(Session("udtTempPracticeModelCollection"), PracticeModelCollection).Values
                    'If udtExistingPracticeModel.DisplaySeq = hfPracticeDisplaySeq.Value Then
                    If Not IsNothing(udtExistingPracticeModel.PracticeAddress) Then
                        If Not IsNothing(udtExistingPracticeModel.PracticeAddress.ChiBuilding) AndAlso Not udtExistingPracticeModel.PracticeAddress.ChiBuilding.Trim.Equals(String.Empty) Then
                            blnExisted = True
                            Exit For
                        End If
                    End If
                    'End If
                Next
                If blnExisted Then
                    'ibtnPracticeChiAddressHelp.Visible = True
                    'Session(ibtnPracticeChiAddressHelp.ClientID) = True
                    If Not txtPracticeChiAddress.Text.Trim.Equals(String.Empty) Then
                        ibtnPracticeChiAddressHelp.Visible = False
                        Session(ibtnPracticeChiAddressHelp.ClientID) = False
                    Else
                        ibtnPracticeChiAddressHelp.Visible = True
                        Session(ibtnPracticeChiAddressHelp.ClientID) = True
                    End If
                Else
                    ibtnPracticeChiAddressHelp.Visible = False
                    Session(ibtnPracticeChiAddressHelp.ClientID) = False
                End If
                'End If
                'Else
                '    'If txtPracticeChiAddress.Text <> "" Then
                '    ibtnPracticeChiAddressHelp.Visible = False
                '    Session(ibtnPracticeChiAddressHelp.ClientID) = False
                '    'Else
                '    '    ibtnPracticeChiAddressHelp.Visible = True
                '    '    Session(ibtnPracticeChiAddressHelp.ClientID) = True
                '    'End If
                'End If
            Else
                ibtnPracticeChiAddressHelp.Visible = Session(ibtnPracticeChiAddressHelp.ClientID)
            End If

        End If
    End Sub

    Protected Sub gvPRPracticeBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPRPracticeBank.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Convert Medical Organization No. to Medical Organization Name
            Dim lblPRPracticeMO As Label = CType(e.Row.FindControl("lblPRPracticeMO"), Label)
            Dim intMODisplaySeq As Integer = CInt(lblPRPracticeMO.Text.Trim)

            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                If udtMO.DisplaySeq.Value = intMODisplaySeq Then
                    lblPRPracticeMO.Text = udtMO.DisplaySeqMOName 'udtMO.MOEngName
                    Exit For
                End If
            Next

            'Filtering
            Dim dr As PracticeModel = CType(e.Row.DataItem, PracticeModel)
            Dim strPracticeStatus As String = String.Empty

            strPracticeStatus = dr.RecordStatus

            Select Case strPracticeStatus
                Case PracticeStatus.Delisted
                    e.Row.Visible = False
            End Select

            ' If there is no schemes, show "N/A"
            'Dim gvPracticeSchemeInfo As GridView = e.Row.FindControl("gvPRPracticeSchemeInfo")
            'If gvPracticeSchemeInfo.Rows.Count = 0 Then
            '    CType(e.Row.FindControl("lblPRPracticeSchemeInfoNA"), Label).Visible = True
            '    gvPracticeSchemeInfo.Visible = False
            'End If


        End If
    End Sub


    Protected Sub gvPracticeSchemeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim lblPracticeSchemeType As Label = CType(e.Row.FindControl("lblPracticeSchemeType"), Label)
        '    Dim lblPracticeSchemeStatus As Label = CType(e.Row.FindControl("lblPracticeSchemeStatus"), Label)
        '    Dim lblPracticeSchemeRemark As Label = CType(e.Row.FindControl("lblPracticeSchemeRemark"), Label)
        '    Dim lblPracticeSchemeUpdateDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeUpdateDtm"), Label)
        '    Dim lblPracticeSchemeEffectiveDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeEffectiveDtm"), Label)
        '    Dim lblPracticeSchemeDelistDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeDelistDtm"), Label)
        '    Dim lblPracticeSchemeDesc As Label = CType(e.Row.FindControl("lblPracticeSchemeDesc"), Label)

        '    ' Change Header Text: Status -> Status [Remark]
        '    CType(sender, GridView).HeaderRow.Cells(1).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

        '    ' Reformat the output
        '    ' Scheme Name
        '    Dim udtVoucherSchemeBLL As New VoucherScheme.VoucherSchemeBLL
        '    lblPracticeSchemeType.Text = udtVoucherSchemeBLL.getSchemeDisplayNameFromCode(lblPracticeSchemeType.Text)

        '    ' Status [Remark]
        '    Status.GetDescriptionFromDBCode(IIf(hfTableLocation.Value.Trim = TableLocation.Permanent, SPMaintenanceDisplayStatus.ClassCode, PracticeSchemeInfoStagingStatus.ClassCode), lblPracticeSchemeStatus.Text.Trim, lblPracticeSchemeStatus.Text, String.Empty)
        '    If Not lblPracticeSchemeRemark.Text.Trim.Equals(String.Empty) Then
        '        lblPracticeSchemeStatus.Text += " [" + lblPracticeSchemeRemark.Text.Trim + "]"
        '    End If

        '    ' Status Update Date
        '    If Not IsNothing(lblPracticeSchemeUpdateDtm) Then
        '        If lblPracticeSchemeUpdateDtm.Text.Equals(String.Empty) Then
        '            lblPracticeSchemeUpdateDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblPracticeSchemeUpdateDtm.Text = udtFormatter.convertDateTime(lblPracticeSchemeUpdateDtm.Text.Trim)
        '        End If
        '    End If

        '    ' Effective Date
        '    If Not IsNothing(lblPracticeSchemeEffectiveDtm) Then
        '        If lblPracticeSchemeEffectiveDtm.Text.Equals(String.Empty) Then
        '            lblPracticeSchemeEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblPracticeSchemeEffectiveDtm.Text = udtFormatter.convertDateTime(lblPracticeSchemeEffectiveDtm.Text.Trim)
        '        End If
        '    End If

        '    ' Delisting Date
        '    If Not IsNothing(lblPracticeSchemeDelistDtm) Then
        '        If lblPracticeSchemeDelistDtm.Text.Equals(String.Empty) Then
        '            lblPracticeSchemeDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblPracticeSchemeDelistDtm.Text = udtFormatter.convertDateTime(lblPracticeSchemeDelistDtm.Text.Trim)
        '        End If
        '    End If

        '    ' Service Fee
        '    If Not IsNothing(lblPracticeSchemeDesc) Then
        '        If lblPracticeSchemeDesc.Text.Equals(String.Empty) Then
        '            lblPracticeSchemeDesc.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblPracticeSchemeDesc.Text = "$" + lblPracticeSchemeDesc.Text.Trim
        '        End If
        '    End If

        'End If
    End Sub

    Protected Sub gvPRPracticeSchemeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    Dim lblPRPracticeSchemeType As Label = CType(e.Row.FindControl("lblPRPracticeSchemeType"), Label)
        '    Dim lblPRPracticeSchemeStatus As Label = CType(e.Row.FindControl("lblPRPracticeSchemeStatus"), Label)
        '    Dim lblPRPracticeSchemeRemark As Label = CType(e.Row.FindControl("lblPRPracticeSchemeRemark"), Label)
        '    Dim lblPRPracticeSchemeUpdateDtm As Label = CType(e.Row.FindControl("lblPRPracticeSchemeUpdateDtm"), Label)
        '    Dim lblPRPracticeSchemeEffectiveDtm As Label = CType(e.Row.FindControl("lblPRPracticeSchemeEffectiveDtm"), Label)
        '    Dim lblPRPracticeSchemeDelistDtm As Label = CType(e.Row.FindControl("lblPRPracticeSchemeDelistDtm"), Label)
        '    Dim lblPRPracticeSchemeDesc As Label = CType(e.Row.FindControl("lblPRPracticeSchemeDesc"), Label)

        '    ' Change Header Text: Status -> Status [Remark]
        '    CType(sender, GridView).HeaderRow.Cells(1).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

        '    ' Reformat the output
        '    ' Scheme Name
        '    Dim udtVoucherSchemeBLL As New VoucherScheme.VoucherSchemeBLL
        '    lblPRPracticeSchemeType.Text = udtVoucherSchemeBLL.getSchemeDisplayNameFromCode(lblPRPracticeSchemeType.Text)

        '    ' Status [Remark]
        '    Status.GetDescriptionFromDBCode(IIf(hfTableLocation.Value.Trim = TableLocation.Permanent, SPMaintenanceDisplayStatus.ClassCode, PracticeSchemeInfoStagingStatus.ClassCode), lblPRPracticeSchemeStatus.Text.Trim, lblPRPracticeSchemeStatus.Text, String.Empty)
        '    If Not lblPRPracticeSchemeRemark.Text.Trim.Equals(String.Empty) Then
        '        lblPRPracticeSchemeStatus.Text += " [" + lblPRPracticeSchemeRemark.Text.Trim + "]"
        '    End If

        '    ' Effective Date
        '    If Not IsNothing(lblPRPracticeSchemeEffectiveDtm) Then
        '        If lblPRPracticeSchemeEffectiveDtm.Text.Equals(String.Empty) Then
        '            lblPRPracticeSchemeEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblPRPracticeSchemeEffectiveDtm.Text = udtFormatter.convertDateTime(lblPRPracticeSchemeEffectiveDtm.Text.Trim)
        '        End If
        '    End If

        '    ' Delisting Date
        '    If Not IsNothing(lblPRPracticeSchemeDelistDtm) Then
        '        If lblPRPracticeSchemeDelistDtm.Text.Equals(String.Empty) Then
        '            lblPRPracticeSchemeDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblPRPracticeSchemeDelistDtm.Text = udtFormatter.convertDateTime(lblPRPracticeSchemeDelistDtm.Text.Trim)
        '        End If
        '    End If

        '    ' Service Fee
        '    If Not IsNothing(lblPRPracticeSchemeDesc) Then
        '        If lblPRPracticeSchemeDesc.Text.Equals(String.Empty) Then
        '            lblPRPracticeSchemeDesc.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '        Else
        '            lblPRPracticeSchemeDesc.Text = "$" + lblPRPracticeSchemeDesc.Text.Trim
        '        End If
        '    End If

        'End If
    End Sub


    Private Sub bindDistrict(ByVal ddl As DropDownList, ByVal strAreaCode As String, ByVal blnReset As Boolean)
        ddl.Items.Clear()
        ddl.DataSource = udtSPProfileBLL.GetDistrict(strAreaCode)
        ddl.DataValueField = "District_ID"
        ddl.DataTextField = "District_Name"
        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "SelectDistrict"), ""))
        If blnReset Then
            ddl.SelectedIndex = 0
        End If
    End Sub



    ' Used in .aspx

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If IsNothing(strPracticeCode) Then
            strPracticeTypeName = String.Empty
        Else
            If strPracticeCode.Trim.Equals(String.Empty) Then
                strPracticeTypeName = String.Empty
            Else
                If Session("language") = "zh-tw" Then
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValueChi
                Else
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValue
                End If
            End If
        End If
        Return strPracticeTypeName
    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        Dim strHealthProfName As String

        If IsNothing(strHealthProfCode) Then
            strHealthProfName = String.Empty
        Else
            If strHealthProfCode.Trim.Equals(String.Empty) Then
                strHealthProfName = String.Empty
            Else

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------

                If Session("language") = "zh-tw" Then
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
                Else
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc
                End If

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            End If
        End If

        Return strHealthProfName
    End Function

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return udtFormatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
    End Function

    Protected Function formatAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddress(udtAddressModel)
    End Function

    Protected Function formatChiAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddressChi(udtAddressModel).Replace("(", "").Replace(")", "")
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return udtFormatter.formatChineseName(strChineseString).Replace("(", "").Replace(")", "")
    End Function


    Protected Function formatRemark(ByVal strRemarkString As String) As String
        If strRemarkString.Trim = String.Empty Then
            Return strRemarkString
        Else
            Return "(" + strRemarkString.Trim + ")"
        End If
    End Function

    Protected Function formatDate(ByVal d As Object) As String
        Return udtFormatter.formatDate(Convert.ToDateTime(d))
    End Function





    'Pop Up
    Protected Sub ibtnMOBRCodeHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

            If IsNothing(udtSP) Then
                udcInfoMessageBox.AddMessage("990000", "I", "00015")
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Dim udpSPMigrationBLL As New SPMigrationBLL
                BrCodeView.DataSource = udpSPMigrationBLL.GetBRCode(udtSP.HKID)
                BrCodeView.DataBind()
                Me.ModalPopupExtenderDataImgration.Show()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnDataImgratioClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderDataImgration.Hide()
    End Sub

    Protected Sub ibtnPracticeTypeHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

            If IsNothing(udtSP) Then
                udcInfoMessageBox.AddMessage("990000", "I", "00015")
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Dim udpSPMigrationBLL As New SPMigrationBLL
                PracticeTypeView.DataSource = udpSPMigrationBLL.GetPracticeType(udtSP.HKID)
                PracticeTypeView.DataBind()
                Me.ModalPopupExtenderPracticeType.Show()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnPracticeChiNameClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderPracticeType.Hide()
    End Sub

    Protected Sub ibtnPracticeChiNameHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

            If IsNothing(udtSP) Then
                udcInfoMessageBox.AddMessage("990000", "I", "00015")
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Dim udpSPMigrationBLL As New SPMigrationBLL
                PracticeChiNameView.DataSource = udpSPMigrationBLL.GetPracticeReferenceT(udtSP.EnrolRefNo)
                PracticeChiNameView.DataBind()
                Me.ModalPopupExtenderPracticeChiName.Show()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnPracticeTypeClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderPracticeChiName.Hide()
    End Sub

    Protected Sub ibtnPracticePhoneHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

            If IsNothing(udtSP) Then
                udcInfoMessageBox.AddMessage("990000", "I", "00015")
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Dim udpSPMigrationBLL As New SPMigrationBLL
                PracticePhoneView.DataSource = udpSPMigrationBLL.GetPracticeReferenceT(udtSP.EnrolRefNo)
                PracticePhoneView.DataBind()
                Me.ModalPopupExtenderPracticePhone.Show()

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnPracticePhoneClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderPracticePhone.Hide()
    End Sub

    Protected Sub ibtnPracticeChiAddressHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

            If IsNothing(udtSP) Then
                udcInfoMessageBox.AddMessage("990000", "I", "00015")
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Dim udpSPMigrationBLL As New SPMigrationBLL
                'PracticeChiAddressView.DataSource = udpSPMigrationBLL.GetPracticeReference(udtSP.EnrolRefNo)
                If Not Session("PracticeChiAddressFilterd") Then
                    Dim PracticeAddresList As List(Of String) = New List(Of String)
                    Dim PracticeCollectionWithChiAddress As PracticeModelCollection = New PracticeModelCollection
                    For Each udtPracticeModel As PracticeModel In CType(Session("udtTempPracticeModelCollection"), PracticeModelCollection).Values
                        If Not IsNothing(udtPracticeModel.PracticeAddress.ChiBuilding) AndAlso Not udtPracticeModel.PracticeAddress.ChiBuilding.Trim.Equals(String.Empty) AndAlso Not PracticeAddresList.Contains(udtPracticeModel.PracticeAddress.ChiBuilding) Then
                            PracticeAddresList.Add(udtPracticeModel.PracticeAddress.ChiBuilding)
                            PracticeCollectionWithChiAddress.Add(udtPracticeModel)
                        End If
                    Next
                    Session("udtTempPracticeModelCollection") = PracticeCollectionWithChiAddress
                    Session("PracticeChiAddressFilterd") = True
                End If
                PracticeChiAddressView.DataSource = CType(Session("udtTempPracticeModelCollection"), PracticeModelCollection).Values
                'PracticeChiAddressView.DataSource = PracticeAddresList
                PracticeChiAddressView.DataBind()
                Me.ModalPopupExtenderPracticeChiAddress.Show()

                End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnPracticeChiAddressClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderPracticeChiAddress.Hide()
    End Sub

    'Area Code & District (MO)
    Protected Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlDistrict As DropDownList = Nothing
        'Dim rboArea As RadioButtonList = Nothing

        If Not hfGridviewIndex.Value.Trim.Equals(String.Empty) Then
            ddlDistrict = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlMOEditDistrict")
            'rboArea = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("rboMOEditArea")
        End If

        If Not IsNothing(ddlDistrict) Then
            Dim strSelectedDistrictValue As String
            strSelectedDistrictValue = ddlDistrict.SelectedValue

            If strSelectedDistrictValue.Trim.Equals(String.Empty) Then
                'rboArea.ClearSelection()
            Else
                ' Dim strAreaCode As String

                'strAreaCode = udtSPProfileBLL.GetAreaByDistrictCode(ddlDistrict.SelectedValue)

                'rboArea.SelectedValue = strAreaCode
                ddlDistrict.SelectedValue = strSelectedDistrictValue
            End If
        End If
    End Sub

    'Protected Sub rboArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim ddlDistrict As DropDownList = Nothing
    '    Dim rboArea As RadioButtonList = Nothing

    '    If Not hfGridviewIndex.Value.Equals(String.Empty) Then
    '        ddlDistrict = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlMOEditDistrict")
    '        rboArea = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("rboMOEditArea")
    '    End If


    '    If Not IsNothing(ddlDistrict) And Not IsNothing(rboArea) Then
    '        Select Case rboArea.SelectedValue
    '            Case 1
    '                ddlDistrict.SelectedValue = ".H"
    '            Case 2
    '                ddlDistrict.SelectedValue = ".K"
    '            Case 3
    '                ddlDistrict.SelectedValue = ".N"
    '        End Select
    '    End If
    'End Sub


    'Area Code & District (Practice)
    Protected Sub ddlPracticeDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlDistrict As DropDownList = Nothing
        'Dim rboArea As RadioButtonList = Nothing

        If Not hfGridviewIndex.Value.Trim.Equals(String.Empty) Then
            ddlDistrict = gvPracticeBank.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlPracticeEditDistrict")
            'rboArea = gvPracticeBank.Rows(CInt(hfGridviewIndex.Value)).FindControl("rboPracticeEditArea")
        End If

        If Not IsNothing(ddlDistrict) Then
            Dim strSelectedDistrictValue As String
            strSelectedDistrictValue = ddlDistrict.SelectedValue

            If strSelectedDistrictValue.Trim.Equals(String.Empty) Then
                'rboArea.ClearSelection()
            Else
                'Dim strAreaCode As String

                'strAreaCode = udtSPProfileBLL.GetAreaByDistrictCode(ddlDistrict.SelectedValue)

                'rboArea.SelectedValue = strAreaCode
                ddlDistrict.SelectedValue = strSelectedDistrictValue
            End If
        End If
    End Sub

    'Protected Sub rboPracticeArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim ddlDistrict As DropDownList = Nothing
    '    Dim rboArea As RadioButtonList = Nothing

    '    If Not hfGridviewIndex.Value.Equals(String.Empty) Then
    '        ddlDistrict = gvPracticeBank.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlPracticeEditDistrict")
    '        rboArea = gvPracticeBank.Rows(CInt(hfGridviewIndex.Value)).FindControl("rboPracticeEditArea")
    '    End If


    '    If Not IsNothing(ddlDistrict) And Not IsNothing(rboArea) Then
    '        Select Case rboArea.SelectedValue
    '            Case 1
    '                ddlDistrict.SelectedValue = ".H"
    '            Case 2
    '                ddlDistrict.SelectedValue = ".K"
    '            Case 3
    '                ddlDistrict.SelectedValue = ".N"
    '        End Select
    '    End If
    'End Sub



    'Update Practice MO selection drop down list
    Protected Sub txtMOEName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim txtMOEName As TextBox = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtMOEName")
        Dim hfMOSeq As HiddenField = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfMOSeq")
        Dim txtMOCName As TextBox = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtMOCName")
        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
        For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
            If udtMO.DisplaySeq = hfMOSeq.Value.Trim Then
                udtMO.MOEngName = txtMOEName.Text
                If txtMOEName.Text.Trim <> String.Empty Then
                    udtMO.DisplaySeqMOName = hfMOSeq.Value.Trim + ". " + txtMOEName.Text
                Else
                    udtMO.DisplaySeqMOName = hfMOSeq.Value.Trim + ". < " + Me.GetGlobalResourceObject("Text", "MedicalOrganization") + " >"
                End If
            End If
        Next

        Dim row As Integer
        For row = 0 To gvPracticeBank.Rows.Count - 1
            Dim ddlPracticeMO As DropDownList = CType(gvPracticeBank.Rows(row).FindControl("ddlPracticeMO"), DropDownList)
            Dim DDLlist As SortedList = New SortedList()
            Dim intSelectIndex As Integer = ddlPracticeMO.SelectedIndex

            If udtSP.MOList.Count > 1 Then
                DDLlist.Add(0, Me.GetGlobalResourceObject("Text", "SelectMO"))
                ddlPracticeMO.Enabled = True
            Else
                ddlPracticeMO.Enabled = False
            End If

            Dim strSelectedDisplaySeqMOName As String = String.Empty
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                DDLlist.Add(udtMO.DisplaySeq, udtMO.DisplaySeqMOName)
            Next

            ddlPracticeMO.DataSource = DDLlist
            ddlPracticeMO.DataTextField = "value"
            ddlPracticeMO.DataValueField = "key"
            ddlPracticeMO.DataBind()
            ddlPracticeMO.SelectedIndex = intSelectIndex
        Next


        'Focus = txtMOCName.ClientID
        'ControlType = "TextBox"

        'Session("SetFocusTotxtMOCName") = True
        'txtMOCName.Focus()
    End Sub

    'Add New MO
    Protected Sub ibtnAddMO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'If editValidation() Then
        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
        rebindSPModel(udtSP, False, "0")
        Dim udtNewMOModel As MedicalOrganizationModel = New MedicalOrganizationModel()
        Dim udtNewAddress As Address.AddressModel = New Address.AddressModel()

        'For Each r As GridViewRow In gvPracticeBank.Rows
        '    Dim ddlPracticeMO As DropDownList = CType(r.FindControl("ddlPracticeMO"), DropDownList)
        '    Dim hfPracticeDisplaySeq As HiddenField = CType(r.FindControl("hfPracticeDisplaySeq"), HiddenField)

        '    udtSP.PracticeList.Item(CType(hfPracticeDisplaySeq.Value, Integer)).MODisplaySeq = ddlPracticeMO.SelectedIndex
        'Next

        Dim intMaxMODisplaySeq As Integer = 0
        For Each udtMOModel As MedicalOrganizationModel In udtSP.MOList.Values
            If udtMOModel.DisplaySeq.Value > intMaxMODisplaySeq Then
                intMaxMODisplaySeq = udtMOModel.DisplaySeq.Value
            End If
        Next

        udtNewMOModel.DisplaySeq = intMaxMODisplaySeq + 1
        udtNewMOModel.DisplaySeqMOName = udtNewMOModel.DisplaySeq.Value.ToString + ". < " + Me.GetGlobalResourceObject("Text", "MedicalOrganization") + " >"
        udtNewMOModel.MOAddress = udtNewAddress
        udtNewMOModel.SPID = udtSP.SPID

        udtSP.MOList.Add(udtNewMOModel)
        udtServiceProviderBLL.SaveToSession(udtSP)
        Session("FirstBind") = False
        BindSPSummaryView()
        'End If
    End Sub

    'Delete MO
    Protected Sub ibtnDeleteMO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim hfMOSeq As HiddenField = gvMO.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfMOSeq")
        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
        rebindSPModel(udtSP, True, hfMOSeq.Value)
        'For Each r As GridViewRow In gvPracticeBank.Rows
        '    Dim ddlPracticeMO As DropDownList = CType(r.FindControl("ddlPracticeMO"), DropDownList)
        '    Dim hfPracticeDisplaySeq As HiddenField = CType(r.FindControl("hfPracticeDisplaySeq"), HiddenField)

        '    udtSP.PracticeList.Item(CType(hfPracticeDisplaySeq.Value, Integer)).MODisplaySeq = ddlPracticeMO.SelectedIndex
        'Next

        If udtSP.MOList.Count > 1 Then
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                If udtMO.DisplaySeq = hfMOSeq.Value.Trim Then
                    udtSP.MOList.Remove(udtMO)
                    Exit For
                End If
            Next

            'Reset Display Seq no
            Dim intDispSeq As Integer = 1
            Dim udtNewMOCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                udtMO.DisplaySeq = intDispSeq
                udtMO.DisplaySeqMOName = intDispSeq.ToString.Trim + udtMO.DisplaySeqMOName.Substring(udtMO.DisplaySeqMOName.IndexOf("."))
                udtNewMOCollection.Add(udtMO)
                intDispSeq += intDispSeq
            Next

            udtSP.MOList = udtNewMOCollection
            udtServiceProviderBLL.SaveToSession(udtSP)
            Session("DeletingMOSeq") = hfMOSeq.Value
            Session("FirstBind") = False
            BindSPSummaryView()

            'editValidation()
        End If
    End Sub

    Private Function editValidation() As Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        Dim strOld As String() = {"%r", "%s"}

        'MO
        For Each r As GridViewRow In gvMO.Rows
            Dim lblMOIndex As Label = CType(r.FindControl("lblMOIndex"), Label)
            Dim txtMOBRCodeText As TextBox = CType(r.FindControl("txtMOBRCodeText"), TextBox)
            Dim txtMOContactNoText As TextBox = CType(r.FindControl("txtMOContactNoText"), TextBox)
            'Dim txtMOChiAddressText As TextBox = CType(r.FindControl("txtMOChiAddress"), TextBox)
            Dim txtMOBuilding As TextBox = CType(r.FindControl("txtMOBuilding"), TextBox)
            Dim ddlMOEditDistrict As DropDownList = CType(r.FindControl("ddlMOEditDistrict"), DropDownList)
            'Dim rboMOEditArea As RadioButtonList = CType(r.FindControl("rboMOEditArea"), RadioButtonList)
            Dim txtMORoom As TextBox = CType(r.FindControl("txtMORoom"), TextBox)
            Dim txtMOFloor As TextBox = CType(r.FindControl("txtMOFloor"), TextBox)
            Dim txtMOBlock As TextBox = CType(r.FindControl("txtMOBlock"), TextBox)
            Dim txtMOEmail As TextBox = CType(r.FindControl("txtMOEmail"), TextBox)
            Dim txtMOFax As TextBox = CType(r.FindControl("txtMOFax"), TextBox)

            Dim txtMOCNameText As TextBox = CType(r.FindControl("txtMOCName"), TextBox)
            Dim txtMOENameText As TextBox = CType(r.FindControl("txtMOEName"), TextBox)

            Dim rboEditMORelation As RadioButtonList = r.FindControl("rboEditMORelation")
            Dim txtEditMORelationRemark As TextBox = r.FindControl("txtEditMORelationRemark")

            Dim imgEditMORelationRemarksAlert As Image = CType(r.FindControl("imgEditMORelationRemarksAlert"), Image)
            Dim imgEditMOBRCodeAlert As Image = CType(r.FindControl("imgEditMOBRCodeAlert"), Image)
            Dim imgEditMOContactNoAlert As Image = CType(r.FindControl("imgEditMOContactNoAlert"), Image)
            Dim imgEditMOENameAlert As Image = CType(r.FindControl("imgEditMOENameAlert"), Image)
            Dim imgMOBuildingAlert As Image = CType(r.FindControl("imgMOBuildingAlert"), Image)
            Dim imgMOEditDistrictAlert As Image = CType(r.FindControl("imgMOEditDistrictAlert"), Image)
            Dim imgEditMOEmailAlert As Image = CType(r.FindControl("imgEditMOEmailAlert"), Image)

            'Add audit log
            udtAuditLogEntry.AddDescripton("MigrationStatus", Me.lblMigrationStatus.Text.Trim)
            udtAuditLogEntry.AddDescripton("MO", "MO")
            udtAuditLogEntry.AddDescripton("DisplaySeq ", lblMOIndex.Text.Trim)
            udtAuditLogEntry.AddDescripton("MOEngName ", txtMOENameText.Text.Trim)
            udtAuditLogEntry.AddDescripton("MOChiName ", txtMOCNameText.Text.Trim)
            udtAuditLogEntry.AddDescripton("Fax ", txtMOFax.Text.Trim)
            udtAuditLogEntry.AddDescripton("Email ", txtMOEmail.Text.Trim)
            udtAuditLogEntry.AddDescripton("BrCode ", txtMOBRCodeText.Text.Trim)
            udtAuditLogEntry.AddDescripton("PhoneDaytime ", txtMOContactNoText.Text.Trim)
            udtAuditLogEntry.AddDescripton("Room ", txtMORoom.Text.Trim)
            udtAuditLogEntry.AddDescripton("Block ", txtMOBlock.Text.Trim)
            udtAuditLogEntry.AddDescripton("Floor ", txtMOFloor.Text.Trim)
            udtAuditLogEntry.AddDescripton("Building ", txtMOBuilding.Text.Trim)
            udtAuditLogEntry.AddDescripton("District ", ddlMOEditDistrict.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Relationship ", rboEditMORelation.SelectedValue.Trim.Trim)
            udtAuditLogEntry.AddDescripton("RelationshipRemark ", txtEditMORelationRemark.Text.Trim.Trim)

            'check english medical organisation name
            If txtMOENameText.Text.Trim.Equals(String.Empty) Then
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00174)
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
                imgEditMOENameAlert.Visible = True
            Else
                imgEditMOENameAlert.Visible = False
            End If

            'check BR code
            If txtMOBRCodeText.Text.Trim.Equals(String.Empty) Then
                imgEditMOBRCodeAlert.Visible = True
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00175)
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
            Else
                imgEditMOBRCodeAlert.Visible = False
            End If

            'check MO phone no
            If txtMOContactNoText.Text.Trim.Equals(String.Empty) Then
                imgEditMOContactNoAlert.Visible = True
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00176)
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
            Else
                imgEditMOContactNoAlert.Visible = False
            End If

            'check email
            If Not txtMOEmail.Text.Trim.Equals(String.Empty) Then
                If Not IsNothing(udcValidator.chkEmailAddress(txtMOEmail.Text.Trim)) Then
                    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00181)
                    Dim strNew As String() = {"", ""}
                    strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    strNew(1) = (r.RowIndex + 1).ToString
                    udcErrorMessage.AddMessage(SM, strOld, strNew)
                    imgEditMOEmailAlert.Visible = True
                Else
                    imgEditMOEmailAlert.Visible = False
                End If
            End If

            'check address
            If (txtMOBuilding.Text.Trim.Equals(String.Empty)) Or (ddlMOEditDistrict.SelectedValue = ".H" Or ddlMOEditDistrict.SelectedValue = ".K" Or ddlMOEditDistrict.SelectedValue = ".N" Or ddlMOEditDistrict.SelectedValue = String.Empty) Then
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00180)
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
            End If

            If txtMOBuilding.Text.Trim() = String.Empty Then
                imgMOBuildingAlert.Visible = True
            Else
                imgMOBuildingAlert.Visible = False
            End If

            If (ddlMOEditDistrict.SelectedValue = ".H" Or ddlMOEditDistrict.SelectedValue = ".K" Or ddlMOEditDistrict.SelectedValue = ".N" Or ddlMOEditDistrict.SelectedValue = String.Empty) Then
                imgMOEditDistrictAlert.Visible = True
            Else
                imgMOEditDistrictAlert.Visible = False
            End If

            'check the relation of MO
            If udcValidator.IsEmpty(rboEditMORelation.SelectedValue.Trim) Then
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00182)
                imgEditMORelationRemarksAlert.Visible = True
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
            Else
                imgEditMORelationRemarksAlert.Visible = False
            End If


            If rboEditMORelation.SelectedValue.Trim.Equals("O") Then
                If udcValidator.IsEmpty(txtEditMORelationRemark.Text.Trim) Then
                    imgEditMORelationRemarksAlert.Visible = True
                    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00178)
                    Dim strNew As String() = {"", ""}
                    strNew(0) = Me.GetGlobalResourceObject("Text", "MedicalOrganization")
                    strNew(1) = (r.RowIndex + 1).ToString
                    udcErrorMessage.AddMessage(SM, strOld, strNew)
                Else
                    imgEditMORelationRemarksAlert.Visible = False
                End If
            End If

            'reset MO relation remark textbox
            Dim hfEditMORelation As HiddenField = r.FindControl("hfEditMORelation")
            hfEditMORelation.Value = rboEditMORelation.SelectedValue.Trim
            If hfEditMORelation.Value.Equals("O") Then
                txtEditMORelationRemark.BackColor = Nothing
                txtEditMORelationRemark.Attributes.Remove("readonly")
            Else
                txtEditMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtEditMORelationRemark.Attributes.Add("readonly", "readonly")
            End If
        Next


        'practice
        For Each r As GridViewRow In gvPracticeBank.Rows
            Dim lblPracticeBankIndex As Label = CType(r.FindControl("lblPracticeBankIndex"), Label)
            Dim lblPracticeName As Label = CType(r.FindControl("lblPracticeName"), Label)
            Dim txtPracticeChiName As TextBox = CType(r.FindControl("txtPracticeChiName"), TextBox)
            Dim txtPracticeChiAddress As TextBox = CType(r.FindControl("txtPracticeChiAddress"), TextBox)
            Dim txtPracticePhone As TextBox = CType(r.FindControl("txtPracticePhone"), TextBox)
            Dim ddlPracticeMO As DropDownList = CType(r.FindControl("ddlPracticeMO"), DropDownList)
            Dim ddlPracticeEditDistrict As DropDownList = CType(r.FindControl("ddlPracticeEditDistrict"), DropDownList)
            'Dim rboPracticeEditArea As RadioButtonList = CType(r.FindControl("rboPracticeEditArea"), RadioButtonList)
            Dim txtPracticeBuilding As TextBox = CType(r.FindControl("txtPracticeBuilding"), TextBox)
            Dim txtPracticeRoom As TextBox = CType(r.FindControl("txtPracticeRoom"), TextBox)
            Dim txtPracticeFloor As TextBox = CType(r.FindControl("txtPracticeFloor"), TextBox)
            Dim txtPracticeBlock As TextBox = CType(r.FindControl("txtPracticeBlock"), TextBox)

            Dim imgEditPracticePhoneAlert As Image = CType(r.FindControl("imgEditPracticePhoneAlert"), Image)
            Dim imgEditPracticeMOAlert As Image = CType(r.FindControl("imgEditPracticeMOAlert"), Image)
            Dim imgPracticeBuildingAlert As Image = CType(r.FindControl("imgPracticeBuildingAlert"), Image)
            Dim imgPracticeEditDistrictAlert As Image = CType(r.FindControl("imgPracticeEditDistrictAlert"), Image)

            Dim hfPracticeStatus As HiddenField = CType(r.FindControl("hfPracticeStatus"), HiddenField)
            If hfPracticeStatus.Value.Trim.Equals(PracticeStatus.Delisted) Then
                Continue For
            End If

            'Add audit log
            udtAuditLogEntry.AddDescripton("Practice", "Practice")
            udtAuditLogEntry.AddDescripton("DisplaySeq ", lblPracticeBankIndex.Text.Trim)
            udtAuditLogEntry.AddDescripton("PracticeName ", lblPracticeName.Text.Trim)
            udtAuditLogEntry.AddDescripton("PracticeNameChi ", txtPracticeChiName.Text.Trim)
            udtAuditLogEntry.AddDescripton("PracticePhone", txtPracticePhone.Text.Trim)
            udtAuditLogEntry.AddDescripton("MO_Display_Seq", ddlPracticeMO.SelectedValue)
            udtAuditLogEntry.AddDescripton("Building ", txtPracticeBuilding.Text.Trim)
            udtAuditLogEntry.AddDescripton("BuildingChi ", txtPracticeChiAddress.Text.Trim)
            udtAuditLogEntry.AddDescripton("District ", ddlPracticeEditDistrict.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Room ", txtPracticeRoom.Text.Trim)
            udtAuditLogEntry.AddDescripton("Floor ", txtPracticeFloor.Text.Trim)
            udtAuditLogEntry.AddDescripton("Block ", txtPracticeBlock.Text.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00008, "Validate MO & Practice")

            'check MO
            If ddlPracticeMO.SelectedValue.Trim = "0" Then
                'If ddlPracticeMO.SelectedValue.Trim() = Me.GetGlobalResourceObject("Text", "SelectMO") Then
                'If ddlPracticeMO.SelectedValue = "0" Then
                imgEditPracticeMOAlert.Visible = True
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00179)
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "Practice")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
                'Else
                '    imgEditPracticeMOAlert.Visible = False
                'End If
            Else
                imgEditPracticeMOAlert.Visible = False
            End If

            'check address
            If (txtPracticeBuilding.Text.Trim() = String.Empty) Or (ddlPracticeEditDistrict.SelectedValue = ".H" Or ddlPracticeEditDistrict.SelectedValue = ".K" Or ddlPracticeEditDistrict.SelectedValue = ".N" Or ddlPracticeEditDistrict.SelectedValue = String.Empty) Then
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00180)
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "Practice")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
            End If

            If txtPracticeBuilding.Text.Trim() = String.Empty Then
                imgPracticeBuildingAlert.Visible = True
            Else
                imgPracticeBuildingAlert.Visible = False
            End If

            If (ddlPracticeEditDistrict.SelectedValue = ".H" Or ddlPracticeEditDistrict.SelectedValue = ".K" Or ddlPracticeEditDistrict.SelectedValue = ".N" Or ddlPracticeEditDistrict.SelectedValue = String.Empty) Then
                imgPracticeEditDistrictAlert.Visible = True
            Else
                imgPracticeEditDistrictAlert.Visible = False
            End If

            'check Practice phone no
            If txtPracticePhone.Text.Trim() = String.Empty Then
                imgEditPracticePhoneAlert.Visible = True
                SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00176)
                Dim strNew As String() = {"", ""}
                strNew(0) = Me.GetGlobalResourceObject("Text", "Practice")
                strNew(1) = (r.RowIndex + 1).ToString
                udcErrorMessage.AddMessage(SM, strOld, strNew)
            Else
                imgEditPracticePhoneAlert.Visible = False
            End If
        Next

        If udcErrorMessage.GetCodeTable.Rows.Count = 0 Then
            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Validate MO & Practice successful")
            udcErrorMessage.Visible = False
            Return True
        Else
            udcErrorMessage.Visible = True
            'Me.udcErrorMessage.BuildMessageBox("ValidationFail", )
            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00010, "Validate MO & Practice fail")
            Return False
        End If
    End Function


    'Link Button (Practice Chinese Name)
    Protected Sub ibtnPracticeChiName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ibtnPracticeChiName As LinkButton = CType(sender, LinkButton)
        Dim udtServiceProviderModel As ServiceProviderModel
        udtServiceProviderModel = udtServiceProviderBLL.GetSP

        rebindSPModel(udtServiceProviderModel, False, "0")
        For Each r As GridViewRow In gvPracticeBank.Rows
            If r.RowIndex = Me.hfPracticeGridviewIndex.Value Then
                Dim hfPracticeDisplaySeq As HiddenField = CType(r.FindControl("hfPracticeDisplaySeq"), HiddenField)
                udtServiceProviderModel.PracticeList.Item(CType(hfPracticeDisplaySeq.Value, Integer)).PracticeNameChi = ibtnPracticeChiName.Text
                Exit For
            End If
        Next
        udtServiceProviderBLL.SaveToSession(udtServiceProviderModel)
        Session("FirstBind") = False
        BindSPSummaryView()
    End Sub

    'Link Button (Practice Contact Phone)
    Protected Sub ibtnPracticePhone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ibtnPracticePhone As LinkButton = CType(sender, LinkButton)
        Dim udtServiceProviderModel As ServiceProviderModel
        udtServiceProviderModel = udtServiceProviderBLL.GetSP

        rebindSPModel(udtServiceProviderModel, False, "0")
        For Each r As GridViewRow In gvPracticeBank.Rows
            If r.RowIndex = Me.hfPracticeGridviewIndex.Value Then
                Dim hfPracticeDisplaySeq As HiddenField = CType(r.FindControl("hfPracticeDisplaySeq"), HiddenField)
                udtServiceProviderModel.PracticeList.Item(CType(hfPracticeDisplaySeq.Value, Integer)).PhoneDaytime = ibtnPracticePhone.Text
                Exit For
            End If
        Next
        udtServiceProviderBLL.SaveToSession(udtServiceProviderModel)
        Session("FirstBind") = False
        BindSPSummaryView()
    End Sub

    'Link Button (Practice Chinese Address)
    Protected Sub ibtnPracticeChiAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ibtnPracticeChiAddress As LinkButton = CType(sender, LinkButton)
        Dim udtServiceProviderModel As ServiceProviderModel
        udtServiceProviderModel = udtServiceProviderBLL.GetSP

        rebindSPModel(udtServiceProviderModel, False, "0")
        For Each r As GridViewRow In gvPracticeBank.Rows
            If r.RowIndex = Me.hfPracticeGridviewIndex.Value Then
                Dim hfPracticeDisplaySeq As HiddenField = CType(r.FindControl("hfPracticeDisplaySeq"), HiddenField)
                udtServiceProviderModel.PracticeList.Item(CType(hfPracticeDisplaySeq.Value, Integer)).PracticeAddress.ChiBuilding = ibtnPracticeChiAddress.Text
                Exit For
            End If
        Next
        udtServiceProviderBLL.SaveToSession(udtServiceProviderModel)
        Session("FirstBind") = False
        BindSPSummaryView()
    End Sub

    Private Sub rebindSPModel(ByRef udtServiceProviderModel As ServiceProviderModel, ByVal blnDeletingMO As Boolean, ByVal MOSeqDeleting As Integer)
        'MO
        For Each r As GridViewRow In gvMO.Rows
            Dim lblMOIndex As Label = CType(r.FindControl("lblMOIndex"), Label)
            Dim txtMOBRCodeText As TextBox = CType(r.FindControl("txtMOBRCodeText"), TextBox)
            Dim txtMOContactNoText As TextBox = CType(r.FindControl("txtMOContactNoText"), TextBox)
            'Dim txtMOChiAddressText As TextBox = CType(r.FindControl("txtMOChiAddress"), TextBox)
            Dim txtMOBuilding As TextBox = CType(r.FindControl("txtMOBuilding"), TextBox)
            Dim ddlMOEditDistrict As DropDownList = CType(r.FindControl("ddlMOEditDistrict"), DropDownList)
            'Dim rboMOEditArea As RadioButtonList = CType(r.FindControl("rboMOEditArea"), RadioButtonList)
            Dim txtMORoom As TextBox = CType(r.FindControl("txtMORoom"), TextBox)
            Dim txtMOFloor As TextBox = CType(r.FindControl("txtMOFloor"), TextBox)
            Dim txtMOBlock As TextBox = CType(r.FindControl("txtMOBlock"), TextBox)
            Dim txtMOEmail As TextBox = CType(r.FindControl("txtMOEmail"), TextBox)
            Dim txtMOFax As TextBox = CType(r.FindControl("txtMOFax"), TextBox)

            Dim txtMOCNameText As TextBox = CType(r.FindControl("txtMOCName"), TextBox)
            Dim txtMOENameText As TextBox = CType(r.FindControl("txtMOEName"), TextBox)

            Dim rboEditMORelation As RadioButtonList = r.FindControl("rboEditMORelation")
            Dim txtEditMORelationRemark As TextBox = r.FindControl("txtEditMORelationRemark")


            For Each udtMedicalOrganizationModel As MedicalOrganizationModel In udtServiceProviderModel.MOList.Values
                If udtMedicalOrganizationModel.DisplaySeq = lblMOIndex.Text Then
                    udtMedicalOrganizationModel.MOEngName = txtMOENameText.Text
                    udtMedicalOrganizationModel.MOChiName = txtMOCNameText.Text
                    udtMedicalOrganizationModel.Fax = txtMOFax.Text
                    udtMedicalOrganizationModel.Email = txtMOEmail.Text
                    udtMedicalOrganizationModel.BrCode = txtMOBRCodeText.Text
                    udtMedicalOrganizationModel.PhoneDaytime = txtMOContactNoText.Text
                    If txtMOENameText.Text = String.Empty Then
                        udtMedicalOrganizationModel.DisplaySeqMOName = udtMedicalOrganizationModel.DisplaySeq.Value.ToString + ". < " + Me.GetGlobalResourceObject("Text", "MedicalOrganization") + " >"
                    End If

                    udtMedicalOrganizationModel.MOChiName = txtMOCNameText.Text
                    udtMedicalOrganizationModel.MOAddress.Block = txtMOBlock.Text
                    udtMedicalOrganizationModel.MOAddress.Room = txtMORoom.Text
                    udtMedicalOrganizationModel.MOAddress.Floor = txtMOFloor.Text
                    'udtMedicalOrganizationModel.MOAddress.ChiBuilding = txtMOChiAddressText.Text
                    udtMedicalOrganizationModel.MOAddress.Building = txtMOBuilding.Text
                    udtMedicalOrganizationModel.MOAddress.District = ddlMOEditDistrict.SelectedValue
                    'udtMedicalOrganizationModel.MOAddress.AreaCode = rboMOEditArea.SelectedValue

                    udtMedicalOrganizationModel.Relationship = rboEditMORelation.SelectedValue.Trim
                    udtMedicalOrganizationModel.RelationshipRemark = txtEditMORelationRemark.Text.Trim
                End If
            Next

            'reset MO relation remark textbox
            Dim hfEditMORelation As HiddenField = r.FindControl("hfEditMORelation")
            hfEditMORelation.Value = rboEditMORelation.SelectedValue.Trim
            If hfEditMORelation.Value.Equals("O") Then
                txtEditMORelationRemark.BackColor = Nothing
                txtEditMORelationRemark.Attributes.Remove("readonly")
            Else
                txtEditMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtEditMORelationRemark.Attributes.Add("readonly", "readonly")
            End If
        Next


        'practice
        For Each r As GridViewRow In gvPracticeBank.Rows
            Dim lblPracticeBankIndex As Label = CType(r.FindControl("lblPracticeBankIndex"), Label)
            Dim txtPracticeChiName As TextBox = CType(r.FindControl("txtPracticeChiName"), TextBox)
            Dim txtPracticeChiAddress As TextBox = CType(r.FindControl("txtPracticeChiAddress"), TextBox)
            Dim txtPracticePhone As TextBox = CType(r.FindControl("txtPracticePhone"), TextBox)
            Dim ddlPracticeMO As DropDownList = CType(r.FindControl("ddlPracticeMO"), DropDownList)
            Dim ddlPracticeEditDistrict As DropDownList = CType(r.FindControl("ddlPracticeEditDistrict"), DropDownList)
            'Dim rboPracticeEditArea As RadioButtonList = CType(r.FindControl("rboPracticeEditArea"), RadioButtonList)
            Dim txtPracticeBuilding As TextBox = CType(r.FindControl("txtPracticeBuilding"), TextBox)
            Dim txtPracticeRoom As TextBox = CType(r.FindControl("txtPracticeRoom"), TextBox)
            Dim txtPracticeFloor As TextBox = CType(r.FindControl("txtPracticeFloor"), TextBox)
            Dim txtPracticeBlock As TextBox = CType(r.FindControl("txtPracticeBlock"), TextBox)

            Dim imgEditPracticePhoneAlert As Image = CType(r.FindControl("imgEditPracticePhoneAlert"), Image)
            Dim imgEditPracticeMOAlert As Image = CType(r.FindControl("imgEditPracticeMOAlert"), Image)
            Dim imgPracticeBuildingAlert As Image = CType(r.FindControl("imgPracticeBuildingAlert"), Image)
            Dim imgPracticeEditDistrictAlert As Image = CType(r.FindControl("imgPracticeEditDistrictAlert"), Image)


            For Each udtPracticeModel As PracticeModel In udtServiceProviderModel.PracticeList.Values
                If udtPracticeModel.DisplaySeq = lblPracticeBankIndex.Text Then
                    udtPracticeModel.PracticeNameChi = txtPracticeChiName.Text
                    udtPracticeModel.PhoneDaytime = txtPracticePhone.Text

                    If blnDeletingMO Then
                        If ddlPracticeMO.SelectedValue = MOSeqDeleting Then
                            udtPracticeModel.MODisplaySeq = 0
                        End If
                    Else
                        udtPracticeModel.MODisplaySeq = ddlPracticeMO.SelectedValue
                    End If

                    udtPracticeModel.PracticeAddress.Building = txtPracticeBuilding.Text
                    udtPracticeModel.PracticeAddress.ChiBuilding = txtPracticeChiAddress.Text
                    udtPracticeModel.PracticeAddress.Room = txtPracticeRoom.Text
                    udtPracticeModel.PracticeAddress.Floor = txtPracticeFloor.Text
                    udtPracticeModel.PracticeAddress.Block = txtPracticeBlock.Text
                    udtPracticeModel.PracticeAddress.District = ddlPracticeEditDistrict.SelectedValue
                    'udtPracticeModel.PracticeAddress.AreaCode = rboPracticeEditArea.SelectedValue
                End If
            Next
        Next
    End Sub

    'print letter
    'Private Sub DisplaySchemeEnrolmentPrintOut(ByVal SP_ID As String)
    '    Dim udtDB As New Database
    '    Dim udtSPModel As ServiceProviderModel
    '    Dim udtSPBLL As New ServiceProviderBLL
    '    udtSPModel = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(SP_ID, udtDB)

    '    'Select Print Letter
    '    If Not IsNothing(Session(SESS_Enrolment_Status)) Then
    '        Session(SESS_Enrolment_Status) = Nothing
    '    End If

    '    If udtSPModel.SPID = String.Empty Then
    '        'New Enrolment

    '        Select Case udtSPModel.AlreadyJoinHAPPI
    '            'Without HA PPI-ePR
    '            Case JoinPPIePRStatus.No, JoinPPIePRStatus.NA
    '                For Each udtPracticeModel As Practice.PracticeModel In udtSPModel.PracticeList.Values
    '                    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
    '                        If (udtPracticeSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.NewAdd) Then
    '                            Select Case udtPracticeSchemeInfoModel.SchemeCode
    '                                Case SchemeCode.EHCVS
    '                                    Select Case Session(SESS_Enrolment_Status)
    '                                        Case EnrolmentStatus.Fresh_of_IVSS
    '                                            'Letter 1a
    '                                            Session(SESS_Enrolment_Status) = EnrolmentStatus.Fresh_of_HCVS_IVSS
    '                                        Case EnrolmentStatus.Fresh_of_HCVS_IVSS
    '                                            'Letter 1a
    '                                            Session(SESS_Enrolment_Status) = EnrolmentStatus.Fresh_of_HCVS_IVSS
    '                                        Case Else
    '                                            'Letter 1b
    '                                            Session(SESS_Enrolment_Status) = EnrolmentStatus.Fresh_of_HCVS
    '                                    End Select
    '                                Case SchemeCode.IVSS
    '                                    Select Case Session(SESS_Enrolment_Status)
    '                                        Case EnrolmentStatus.Fresh_of_HCVS
    '                                            'Letter 1a
    '                                            Session(SESS_Enrolment_Status) = EnrolmentStatus.Fresh_of_HCVS_IVSS
    '                                        Case EnrolmentStatus.Fresh_of_HCVS_IVSS
    '                                            'Letter 1a
    '                                            Session(SESS_Enrolment_Status) = EnrolmentStatus.Fresh_of_HCVS_IVSS
    '                                        Case Else
    '                                            'Letter 1c
    '                                            Session(SESS_Enrolment_Status) = EnrolmentStatus.Fresh_of_IVSS
    '                                    End Select
    '                            End Select
    '                        End If
    '                    Next
    '                Next
    '            Case JoinPPIePRStatus.Yes
    '                'With HA PPI-ePR
    '                If udtSPModel.AlreadyJoinHAPPI = JoinPPIePRStatus.Yes Then
    '                    For Each udtPracticeModel As Practice.PracticeModel In udtSPModel.PracticeList.Values
    '                        For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
    '                            If (udtPracticeSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.NewAdd) Then
    '                                Select Case udtPracticeSchemeInfoModel.SchemeCode
    '                                    Case SchemeCode.EHCVS
    '                                        Select Case Session(SESS_Enrolment_Status)
    '                                            Case EnrolmentStatus.PPiePR_Join_IVSS
    '                                                'Letter 4c
    '                                                Session(SESS_Enrolment_Status) = EnrolmentStatus.PPiePR_Join_HCVS_IVSS
    '                                            Case EnrolmentStatus.PPiePR_Join_HCVS_IVSS
    '                                                'Letter 4c
    '                                                Session(SESS_Enrolment_Status) = EnrolmentStatus.PPiePR_Join_HCVS_IVSS
    '                                            Case Else
    '                                                'Letter 4d
    '                                                Session(SESS_Enrolment_Status) = EnrolmentStatus.PPiePR_Join_HCVS
    '                                        End Select
    '                                    Case SchemeCode.IVSS
    '                                        Select Case Session(SESS_Enrolment_Status)
    '                                            Case EnrolmentStatus.PPiePR_Join_HCVS
    '                                                'Letter 4c
    '                                                Session(SESS_Enrolment_Status) = EnrolmentStatus.PPiePR_Join_HCVS_IVSS
    '                                            Case EnrolmentStatus.PPiePR_Join_HCVS_IVSS
    '                                                'Letter 4c
    '                                                Session(SESS_Enrolment_Status) = EnrolmentStatus.PPiePR_Join_HCVS_IVSS
    '                                            Case Else
    '                                                'Letter 4b
    '                                                Session(SESS_Enrolment_Status) = EnrolmentStatus.PPiePR_Join_IVSS
    '                                        End Select
    '                                End Select
    '                            End If
    '                        Next
    '                    Next
    '                End If
    '        End Select
    '    Else
    '        'Scheme Enrolment   Existing HCVS users
    '        For Each udtPracticeModel As Practice.PracticeModel In udtSPModel.PracticeList.Values
    '            For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
    '                If (udtPracticeSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.NewAdd) Then
    '                    Select Case udtPracticeSchemeInfoModel.SchemeCode
    '                        Case SchemeCode.IVSS
    '                            Select Case udtSPModel.AlreadyJoinHAPPI
    '                                Case JoinPPIePRStatus.Yes
    '                                    'Letter 2c
    '                                    Session(SESS_Enrolment_Status) = EnrolmentStatus.HCVS_PPiePR_Join_IVSS
    '                                    Exit For
    '                                Case JoinPPIePRStatus.No
    '                                    'Letter 2a
    '                                    Session(SESS_Enrolment_Status) = EnrolmentStatus.HCVS_Join_IVSS
    '                                    Exit For
    '                            End Select
    '                    End Select
    '                End If
    '            Next
    '        Next
    '    End If

    '    udtSPBLL.SaveToSession(udtSPModel)

    '    'TODO: use the new printout
    '    PrintOutClick = True
    '    Dim strEID As String
    '    strEID = String.Format("DH_HCV002A{0}{1}{2}{3}{4}{5}{6}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)

    '    ScriptManager.RegisterStartupScript(Me, Page.GetType, FunctionCode, "javascript:openNewWin('spEnrolmentPrintOutViewer.aspx?EID=" + strEID + "')", True)
    'End Sub




    '

    'Private Sub EnablePracticeChangeIndicator(ByVal blnEnable As Boolean, ByVal intSeq As Integer)
    '    For Each r As GridViewRow In gvPracticeBank.Rows
    '        If r.RowType = DataControlRowType.DataRow Then
    '            If CType(r.FindControl("lblPracticeBankIndex"), Label).Text = CStr(intSeq) Then
    '                ' Indicator
    '                Dim lblPracticeStatusInd As Label = CType(r.FindControl("lblPracticeStatusInd"), Label)

    '                lblPracticeStatusInd.CssClass = "tableTitle"
    '                lblPracticeStatusInd.Font.Size = New FontUnit(12)
    '                lblPracticeStatusInd.Visible = blnEnable

    '                ' Status Text
    '                Dim lblPracticeStatus As Label = CType(r.FindControl("lblPracticeStatus"), Label)

    '                If blnEnable Then
    '                    lblPracticeStatus.ForeColor = Drawing.Color.Red
    '                Else
    '                    lblPracticeStatus.ForeColor = Drawing.Color.Empty
    '                End If

    '                Exit For

    '            End If
    '        End If
    '    Next

    'End Sub

    'Private Sub EnableIndicator(ByVal blnEnable As Boolean, ByVal strField As String)
    '    Dim lblControl As Label = Nothing

    '    Select Case strField
    '        Case spProfile.ServiceProviderComparator.EnglishName, spProfile.ServiceProviderComparator.ChineseName
    '            lblControl = lblNameInd
    '        Case spProfile.ServiceProviderComparator.SpAddress
    '            lblControl = lblAddressInd
    '        Case spProfile.ServiceProviderComparator.Email
    '            lblControl = lblEmailInd
    '        Case spProfile.ServiceProviderComparator.Phone
    '            lblControl = lblContactNoInd
    '        Case spProfile.ServiceProviderComparator.Fax
    '            lblControl = lblFaxInd
    '    End Select

    '    If Not lblControl Is Nothing Then
    '        lblControl.CssClass = "tableTitle"
    '        lblControl.Font.Size = New FontUnit(12)
    '        lblControl.Visible = blnEnable
    '    End If
    'End Sub

    '
#Region "Implement IWorkingData (CRE11-004)"

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

End Class