Imports HCVU.Component.Menu
Imports HCVU.Component.RoleType
Imports Common.Component.HCVUUser
Imports Common.Component.UserRole
Imports HCVU.Component.RoleSecurity
Imports HCVU.Component.FileGeneration
Imports Common.Encryption
Imports Common.ComObject
Imports Common.Validation
Imports Common.ComFunction
Imports Common.Format
Imports HCVU.BLL
Imports Common.Component.Token
Imports Common.Component
Imports Common.DataAccess
Imports Common.ComFunction.AccountSecurity
Imports Common.Component.StaticData

Partial Public Class UserAccountMaint
    Inherits BasePage

    Public Enum StateType
        LOADED = 0
        EDIT = 1
        ADD = 2
    End Enum

    ' CRE19-022 Inspection Module [Start][Winnie]
    Public Class Gender
        Public Const Male As String = "M"
        Public Const Female As String = "F"
        Public Const NotProvided As String = "N"
    End Class
    ' CRE19-022 Inspection Module [End][Winnie]

    Private blnUserListIndexChange As Boolean = True
    Private Const SESS_USERACCOUNTMAINT As String = "SESS_USERACCOUNTMAINT"
    Private blnResetScrollPos As Boolean = False

    Private Sub SetControl(ByVal blnEdit As Boolean)

        Me.ddlUserList.Enabled = Not blnEdit
        Me.chkDisplayInactive.Enabled = Not blnEdit

        Me.ibtnEdit.Visible = Not blnEdit
        Me.ibtnNew.Visible = Not blnEdit
        Me.ibtnResetPassword.Visible = Not blnEdit
        Me.ibtnSave.Visible = blnEdit
        Me.ibtnCancel.Visible = blnEdit

        Me.lblHKID.Visible = Not blnEdit
        Me.lblUserName.Visible = Not blnEdit
        Me.lblEffectiveDate.Visible = Not blnEdit
        Me.lblExpiryDate.Visible = Not blnEdit
        Me.lblTokenSN.Visible = Not blnEdit

        Me.txtSurname.Visible = blnEdit
        Me.txtGivenname.Visible = blnEdit
        Me.lblUserNameComma.Visible = blnEdit

        Me.txtHKID.Visible = blnEdit
        Me.txtEffectiveDate.Visible = blnEdit
        Me.txtExpiryDate.Visible = blnEdit
        Me.txtTokenSN.Visible = blnEdit

        Me.ibtnEffectiveDate.Visible = blnEdit
        Me.ibtnExpiryDate.Visible = blnEdit

        Me.chklRole.Enabled = blnEdit
        Me.chkSuspended.Enabled = blnEdit

        Me.ckbScheme.Enabled = blnEdit

        ' CRE19-022 - Inspection [Begin][Golden]
        lblChineseName.Visible = Not blnEdit
        txtChineseName.Visible = blnEdit

        lblGender.Visible = Not blnEdit
        rdlGender.Visible = blnEdit

        txtContactNo.Visible = blnEdit
        lblContactNo.Visible = Not blnEdit
        ' CRE19-022 - Inspection [End][Golden]

    End Sub

    Private Sub SetControlState(ByVal state As StateType)
        ViewState("state") = state
        Select Case state
            Case StateType.LOADED
                Me.lblLoginID.Visible = True
                Me.txtLoginID.Visible = False
                Me.ibtnDeActivateToken.Visible = False
                Me.chkAccountLocked.Enabled = False
                SetControl(False)
            Case StateType.EDIT
                Me.lblLoginID.Visible = True
                Me.txtLoginID.Visible = False
                SetControl(True)
                Me.ibtnDeActivateToken.Visible = False
                Me.txtHKID.Visible = False
                Me.lblHKID.Visible = True
                If Me.chkAccountLocked.Checked Then
                    Me.chkAccountLocked.Enabled = True
                Else
                    Me.chkAccountLocked.Enabled = False
                End If
            Case StateType.ADD
                Me.lblLoginID.Visible = False
                Me.txtLoginID.Visible = True
                Me.ibtnDeActivateToken.Visible = False
                Me.chkAccountLocked.Enabled = False
                SetControl(True)
        End Select
    End Sub

    Private Sub ResetControls()
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnCancel)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDialogCancel)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDialogConfirm)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnEdit)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnNew)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnSave)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnResetPassword)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        'Me.udcInfoMessageBox.Visible = False
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        FunctionCode = FunctCode.FUNT010501

        If Not IsPostBack Then

            ResetControls()

            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010501, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "User Account Maintenance loaded")

            Dim dtMenuGroup As DataTable
            Dim dtMenuItem As DataTable
            Dim udtMenuBLL As New MenuBLL

            dtMenuGroup = udtMenuBLL.GetMenuGroupTable
            dtMenuItem = udtMenuBLL.GetMenuItemTable

            Dim dtFuncAccRight As New DataTable

            dtFuncAccRight.Columns.Add(New DataColumn("Function_Code", GetType(System.String)))
            dtFuncAccRight.Columns.Add(New DataColumn("Description", GetType(System.String)))

            SetControlState(StateType.LOADED)

            Dim i, j As Integer
            Dim dr() As DataRow
            Dim r As DataRow
            Dim strGroupName As String
            Dim strResourceKey As String

            For i = 0 To dtMenuGroup.Rows.Count - 1
                strGroupName = dtMenuGroup.Rows(i).Item("Group_Name")
                dr = dtMenuItem.Select("Group_Name = '" & strGroupName & "'", "Display_Seq")
                If dtMenuGroup.Rows(i).Item("Effective_Date") Is DBNull.Value OrElse CType(dtMenuGroup.Rows(i).Item("Effective_Date"), DateTime) < DateTime.Now() Then
                    'dtmGroupEffectiveDate = CType(dtMenuGroup.Rows(i).Item("Effective_Date"), DateTime)
                    For j = 0 To dr.Length - 1
                        If dr(j).Item("Effective_Date") Is DBNull.Value OrElse CType(dr(j).Item("Effective_Date"), DateTime) < DateTime.Now() Then
                            ' Bypass the items without URL (parent item)
                            If IsDBNull(dr(j)("URL")) Then Continue For

                            r = dtFuncAccRight.NewRow
                            r.Item("Function_Code") = dr(j).Item("Function_Code")
                            strResourceKey = dr(j).Item("Resource_Key")
                            If Not strResourceKey.Trim = "" Then
                                r.Item("Description") = Me.GetGlobalResourceObject("Text", strGroupName) & " - " & Me.GetGlobalResourceObject("Text", strResourceKey)
                            Else
                                r.Item("Description") = Me.GetGlobalResourceObject("Text", strGroupName)
                            End If
                            dtFuncAccRight.Rows.Add(r)
                        End If
                    Next
                End If
            Next

            Me.txtUserName.ToolTip = Me.GetGlobalResourceObject("ToolTip", "EnglishNameHint") & vbCrLf & "           " & Me.GetGlobalResourceObject("ToolTip", "EnglishNameExample")
            Me.txtHKID.ToolTip = Me.GetGlobalResourceObject("Text", "HKIDHint") & vbCrLf & "           " & Me.GetGlobalResourceObject("Text", "HKIDExample")

            Me.chklFuncAccessRight.DataSource = dtFuncAccRight
            Me.chklFuncAccessRight.DataBind()

            Dim udtFileGenerationBLL As New FileGenerationBLL
            Dim udtFileGenerationCollection As FileGenerationModelCollection
            udtFileGenerationCollection = udtFileGenerationBLL.GetFileGenerationModelCollection()
            udtFileGenerationCollection.Sort()

            Me.chklFileGenerationRight.DataSource = udtFileGenerationCollection
            Me.chklFileGenerationRight.DataBind()

            Dim dtRoleType As DataTable
            Dim udtRoleTypeBLL As New RoleTypeBLL

            dtRoleType = udtRoleTypeBLL.GetRoleTypeTable()

            Me.chklRole.DataSource = dtRoleType
            Me.chklRole.DataBind()

            BindUserList()

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
            udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)

            'Load Scheme List
            'Dim udtVoucherSchemeBLL As New VoucherScheme.VoucherSchemeBLL
            'Dim udtSchemeModelCollection As VoucherScheme.VoucherSchemeModelCollection
            'udtSchemeModelCollection = udtVoucherSchemeBLL.LoadActiveVoucheSchemeModelCollection()

            'Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL
            'Dim udtSchemeBackOfficeModelCollection As Scheme.SchemeBackOfficeModelCollection
            'udtSchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.getAllSchemeBackOfficeWithSubsidizeGroup
            Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL
            Dim udtSchemeClaimModelCollection As Scheme.SchemeClaimModelCollection
            ' CRE13-001 - EHAPP [Start][Koala]
            ' -------------------------------------------------------------------------------------
            udtSchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup()
            'udtSchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()
            ' CRE13-001 - EHAPP [End][Koala]
            ckbScheme.DataTextField = "DisplayCode"
            ckbScheme.DataValueField = "SchemeCode"
            'Dim udtSchemeBLL As New Scheme.SchemeBLL
            'Dim udtMSchemeList As Scheme.MasterSchemeModelCollection
            'udtMSchemeList = udtSchemeBLL.getAllMasterSchemeWithSubScheme()
            'ckbScheme.DataTextField = "ExternalCode"
            'ckbScheme.DataValueField = "MSchemeCode"
            Me.ckbScheme.DataSource = udtSchemeClaimModelCollection
            Me.ckbScheme.DataBind()

            ' CRE19-022 -  [Begin][Golden]
            Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
            Me.rdlGender.Items.Clear()
            Me.rdlGender.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("Gender")
            rdlGender.DataTextField = "DataValue"
            rdlGender.DataValueField = "ItemNo"
            rdlGender.DataBind()
            ' CRE19-022 -  [End][Golden]

        End If

        Dim strScript As String
        strScript = "document.getElementById('" & Me.pnlRole.ClientID & "').onscroll = pnlRoleOnScroll;" & vbCrLf
        strScript &= "document.getElementById('" & Me.pnlFuncAccessRight.ClientID & "').onscroll = pnlFuncAccessRightOnScroll;" & vbCrLf
        strScript &= "document.getElementById('" & Me.pnlFileGenerationRight.ClientID & "').onscroll = pnlFileGenerationRightOnScroll;" & vbCrLf
        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "PnlOnScroll", strScript, True)

        KeepScrollPos()

    End Sub

    Private Sub KeepScrollPos()
        Dim strScript As String
        strScript = "pnlRoleScroll();" & vbCrLf
        strScript &= "pnlFuncAccessRightScroll();" & vbCrLf
        strScript &= "pnlFileGenerationRightScroll();" & vbCrLf
        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "PnlScroll", strScript, True)
    End Sub

    Private Sub ResetScrollPos()
        Me.hfRoleScrollTop.Value = 0
        Me.hfFuncAccessRightScrollTop.Value = 0
        Me.hfFileGenerationRightScrollTop.Value = 0
    End Sub

    Private Sub BindUserList()
        Dim dtUser As DataTable
        Dim udtHCVUBLL As New HCVUUserBLL
        dtUser = udtHCVUBLL.GetHCVUUserList(Me.chkDisplayInactive.Checked)

        dtUser.Columns.Add(New DataColumn("Display_Text"))

        Dim i As Integer
        For i = 0 To dtUser.Rows.Count - 1
            dtUser.Rows(i).Item("Display_Text") = CStr(dtUser.Rows(i).Item("User_ID")).Trim.ToUpper() & " - " & CStr(dtUser.Rows(i).Item("User_Name")).Trim
        Next

        Me.ddlUserList.DataSource = dtUser
        Me.ddlUserList.DataBind()

        Me.ddlUserList.Items.Insert(0, New ListItem(CStr(Me.GetGlobalResourceObject("Text", "PleaseSelect")), "-1"))

    End Sub

    Private Sub ClearUserInfo()
        Me.lblLoginID.Text = ""
        Me.lblUserName.Text = ""
        Me.lblHKID.Text = ""
        Me.lblEffectiveDate.Text = ""
        Me.lblExpiryDate.Text = ""
        Me.lblTokenSN.Text = ""

        Me.txtLoginID.Text = ""
        Me.txtUserName.Text = ""
        Me.txtSurname.Text = ""
        Me.txtGivenname.Text = ""
        Me.txtHKID.Text = ""
        Me.txtEffectiveDate.Text = ""
        Me.txtExpiryDate.Text = ""
        Me.txtTokenSN.Text = ""

        ' CRE19-022 - Inspection [Begin][Golden]
        Me.lblChineseName.Text = ""
        Me.lblGender.Text = ""
        Me.lblContactNo.Text = ""
        Me.txtChineseName.Text = ""
        Me.txtContactNo.Text = ""

        Me.rdlGender.SelectedValue = Gender.NotProvided
        ' CRE19-022 - Inspection [End][Golden]


        Me.chkSuspended.Checked = False
        Me.chkAccountLocked.Checked = False
    End Sub

    Private Sub SetUserList(ByVal strUserID As String)
        Dim i As Integer
        For i = 0 To Me.ddlUserList.Items.Count - 1
            If Me.ddlUserList.Items(i).Value.Trim = strUserID.Trim Then
                Me.ddlUserList.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Private Function SetUserInfo(ByVal strUserID As String) As HCVUUserModel

        ClearUserInfo()

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtFormatter As New Formatter

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUserInfo(strUserID)

        Me.lblLoginID.Text = udtHCVUUser.UserID.ToUpper()
        Me.txtLoginID.Text = udtHCVUUser.UserID.ToUpper()

        Dim strUserName As String
        Dim strSurname As String
        Dim strGivenname As String = ""

        strUserName = udtHCVUUser.UserName.Trim.ToUpper
        strSurname = strUserName.Split(",")(0).Trim
        If strUserName.Split(",").Length > 1 Then
            strGivenname = strUserName.Split(",")(1).Trim
        End If

        Me.lblUserName.Text = udtHCVUUser.UserName
        Me.txtUserName.Text = udtHCVUUser.UserName
        Me.txtSurname.Text = strSurname
        Me.txtGivenname.Text = strGivenname

        Me.lblHKID.Text = udtFormatter.formatHKID(udtHCVUUser.HKID, False)
        Me.txtHKID.Text = udtFormatter.formatHKID(udtHCVUUser.HKID, False)


        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Me.lblEffectiveDate.Text = udtFormatter.formatDate(udtHCVUUser.EffectiveDate)
        Me.lblEffectiveDate.Text = udtFormatter.formatDisplayDate(udtHCVUUser.EffectiveDate)
        'Me.txtEffectiveDate.Text = udtFormatter.formatEnterDate(udtHCVUUser.EffectiveDate)
        Me.txtEffectiveDate.Text = udtFormatter.formatInputTextDate(udtHCVUUser.EffectiveDate)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


        If udtHCVUUser.ExpiryDate.HasValue Then

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Me.lblExpiryDate.Text = udtFormatter.formatDate(udtHCVUUser.ExpiryDate.Value)
            Me.lblExpiryDate.Text = udtFormatter.formatDisplayDate(udtHCVUUser.ExpiryDate.Value)
            'Me.txtExpiryDate.Text = udtFormatter.formatEnterDate(udtHCVUUser.ExpiryDate.Value)
            Me.txtExpiryDate.Text = udtFormatter.formatInputTextDate(udtHCVUUser.ExpiryDate.Value)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Else
            Me.lblExpiryDate.Text = "--"
        End If

        If udtHCVUUser.Token.TokenSerialNo = "" Then
            Me.lblTokenSN.Text = "--"
        Else
            Me.lblTokenSN.Text = udtHCVUUser.Token.TokenSerialNo
        End If

        Me.txtTokenSN.Text = udtHCVUUser.Token.TokenSerialNo

        Me.chkSuspended.Checked = udtHCVUUser.Suspended
        Me.chkAccountLocked.Checked = udtHCVUUser.Locked

        ' CRE19-022 - Inspection [Begin][Golden]
        If udtHCVUUser.ChineseName = "" Then
            Me.lblChineseName.Text = "--"
        Else
            Me.lblChineseName.Text = udtHCVUUser.ChineseName
        End If

        Me.lblGender.Text = GetGenderText(udtHCVUUser.Gender)

        If udtHCVUUser.ContactNo = "" Then
            Me.lblContactNo.Text = "--"
        Else
            Me.lblContactNo.Text = udtHCVUUser.ContactNo
        End If

        Me.txtChineseName.Text = udtHCVUUser.ChineseName
        Me.rdlGender.SelectedValue = If(udtHCVUUser.Gender = String.Empty, Gender.NotProvided, udtHCVUUser.Gender)
        Me.txtContactNo.Text = udtHCVUUser.ContactNo

        ' CRE19-022 - Inspection [End][Golden]

        Session(SESS_USERACCOUNTMAINT) = udtHCVUUser

        Return udtHCVUUser
    End Function
    Private Function GetGenderText(Gender As String) As String
        For Each item As ListItem In rdlGender.Items
            If Gender.Trim = item.Value.Trim Then
                Return item.Text
            End If
        Next
        Return String.Empty
    End Function
    Private Sub ClearUserRole()
        Dim li As ListItem
        For Each li In Me.chklRole.Items
            li.Selected = False
        Next
    End Sub

    Private Sub SetUserRole(ByRef udtHCVUUser As HCVUUserModel)
        Dim strUserID As String
        strUserID = udtHCVUUser.UserID

        ClearUserRole()

        Dim udtUserRoleBLL As New UserRoleBLL
        Dim udtUserRoleCollection As UserRoleModelCollection

        udtHCVUUser.UserRoleCollection = udtUserRoleBLL.GetUserRoleCollection(strUserID)
        udtUserRoleCollection = udtHCVUUser.UserRoleCollection

        Dim li As ListItem
        'Dim chkRole As CheckBox
        For Each li In Me.chklRole.Items
            For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                If udtUserRoleModel.RoleType.ToString.Trim.Equals(li.Value.Trim) Then
                    li.Selected = True
                End If
            Next
            'If Not udtUserRoleCollection.Item(li.Value) Is Nothing Then
            '    li.Selected = True
            'End If
        Next

    End Sub

    Private Sub ClearAccessRight()
        Dim li As ListItem
        For Each li In Me.chklFuncAccessRight.Items
            li.Selected = False
        Next
    End Sub

    Private Sub SetAccessRight()
        ClearAccessRight()

        Dim udtRoleSecurityBLL As New RoleSecurityBLL
        Dim dtRoleSecurity As DataTable = udtRoleSecurityBLL.GetRoleSecurityTable()
        Dim dr As DataRow
        Dim drList() As DataRow
        Dim li As ListItem
        Dim liRole As ListItem
        For Each liRole In chklRole.Items
            If liRole.Selected Then
                drList = dtRoleSecurity.Select("Role_Type = '" & liRole.Value & "'")
                For Each dr In drList
                    For Each li In Me.chklFuncAccessRight.Items
                        If li.Value = dr.Item("Function_Code") Then
                            li.Selected = True
                        End If
                    Next
                Next
            End If
        Next

    End Sub

    Private Sub ClearFileGenerationRight()
        Dim li As ListItem
        For Each li In Me.chklFileGenerationRight.Items
            li.Selected = False
        Next
    End Sub

    Private Sub SetFileGenerationRight()
        ClearFileGenerationRight()

        Dim udtFileGenerationBLL As New FileGenerationBLL
        Dim dtRoleTypeFileGeneration As DataTable = udtFileGenerationBLL.GetRoleTypeFileGeneration
        Dim dr As DataRow
        Dim drList() As DataRow
        Dim li As ListItem
        Dim liRole As ListItem
        For Each liRole In chklRole.Items
            If liRole.Selected Then
                drList = dtRoleTypeFileGeneration.Select("Role_Type = '" & liRole.Value & "'")
                For Each dr In drList
                    For Each li In Me.chklFileGenerationRight.Items
                        If li.Value = dr.Item("File_ID") Then
                            li.Selected = True
                        End If
                    Next
                Next
            End If
        Next
    End Sub

    Private Sub ClearUserScheme()
        Dim li As ListItem
        For Each li In Me.ckbScheme.Items
            li.Selected = False
        Next
    End Sub

    Private Sub SetUserScheme(ByRef udtHCVUUser As HCVUUserModel)
        Dim strUserID As String
        strUserID = udtHCVUUser.UserID

        ClearUserScheme()

        Dim udtUserRoleBLL As New UserRoleBLL
        Dim udtUserRoleCollection As UserRoleModelCollection

        udtHCVUUser.UserRoleCollection = udtUserRoleBLL.GetUserRoleCollection(strUserID)
        udtUserRoleCollection = udtHCVUUser.UserRoleCollection

        Dim li As ListItem
        For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
            For Each li In Me.ckbScheme.Items
                If udtUserRoleModel.SchemeCode.Trim.Equals(li.Value.Trim) Then
                    li.Selected = True
                End If
            Next
        Next
    End Sub

    Private Sub ClearAll()
        Me.ClearUserInfo()
        Me.ClearUserRole()
        Me.ClearAccessRight()
        Me.ClearFileGenerationRight()
        ResetScrollPos()
        Me.ibtnDeActivateToken.Visible = False
        Me.ClearUserScheme()
    End Sub

    Private Sub ddlUserList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUserList.SelectedIndexChanged
        Dim udtHCVUUser As HCVUUserModel
        Dim strUserID As String
        Dim udtGeneralFunction As New GeneralFunction

        Me.udcMessageBox.Visible = False

        If Me.ddlUserList.SelectedIndex = 0 Then
            ClearAll()
            udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
            udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)
            Me.ibtnDeActivateToken.Visible = False

        Else
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010501, Me)
            udtAuditLogEntry.AddDescripton("User_ID", Me.ddlUserList.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Load User Account information")

            Try
                strUserID = Me.ddlUserList.SelectedValue
                udtHCVUUser = SetUserInfo(strUserID)

                SetUserRole(udtHCVUUser)
                SetAccessRight()
                SetFileGenerationRight()

                udtGeneralFunction.UpdateImageURL(ibtnEdit, True)
                udtGeneralFunction.UpdateImageURL(ibtnNew, True)
                Me.ibtnDeActivateToken.Visible = False
                If Me.chkDisplayInactive.Checked = False Then
                    udtGeneralFunction.UpdateImageURL(ibtnResetPassword, True)
                Else
                    If Me.lblTokenSN.Text <> "--" Then
                        'Me.ibtnDeActivateToken.Visible = True
                        Me.ibtnDeActivateToken.Visible = False
                    End If
                End If

                'Set the scheme code for the user
                Me.SetUserScheme(udtHCVUUser)
                'Me.ckbScheme.Items(0).Selected = True
                'Me.ckbScheme.Items(1).Selected = True

                ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                displayInfoResetPWMsgBox(udtHCVUUser)
                ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]



                udtAuditLogEntry.AddDescripton("User_ID", Me.ddlUserList.SelectedValue)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Load User Account information successful")
            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("User_ID", Me.ddlUserList.SelectedValue)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Load User Account information failed")
                Throw ex
            End Try
        End If

    End Sub

    Private Sub ibtnEdit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnEdit.Click
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("User_ID", Me.ddlUserList.SelectedValue)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, "Edit Click")
        ' CRE11-021 log the missed essential information [End]

        SetControlState(StateType.EDIT)
        Me.imgTokenSNAlert.Visible = False
    End Sub

    Private Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Me.udcInfoMessageBox.Visible = False
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        Dim udtFormatter As New Formatter
        Me.imgTokenSNAlert.Visible = False

        ViewState("ddlUserList_SelectedValue") = ddlUserList.SelectedValue
        ViewState("ddlUserList_SelectedIndex") = ddlUserList.SelectedIndex

        Me.ddlUserList.Items.Insert(0, New ListItem("(New User)", -1))
        Me.ddlUserList.SelectedItem.Selected = False
        Me.ddlUserList.SelectedIndex = 0

        blnUserListIndexChange = False

        Me.ClearUserInfo()
        Me.ClearUserRole()
        Me.ClearAccessRight()
        Me.ClearFileGenerationRight()

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Me.txtEffectiveDate.Text = udtFormatter.formatEnterDate(DateTime.Now())
        Me.txtEffectiveDate.Text = udtFormatter.formatInputTextDate(DateTime.Now())
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        SetControlState(StateType.ADD)
        Me.ScriptManager1.SetFocus(Me.txtLoginID)

    End Sub

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("User_ID", Me.ddlUserList.SelectedValue)
        udtAuditLogEntry.WriteLog(LogID.LOG00015, "Cancel Click")
        ' CRE11-021 log the missed essential information [End]

        Dim strUserID As String

        Me.udcInfoMessageBox.Visible = False
        Me.udcMessageBox.Visible = False

        Me.ResetAlertImage()

        If ViewState("state") = StateType.ADD Then
            Dim intSelectedIndex As Integer
            strUserID = CStr(ViewState("ddlUserList_SelectedValue"))
            intSelectedIndex = CInt(ViewState("ddlUserList_SelectedIndex"))
            If intSelectedIndex <> 0 Then
                Reload(strUserID)
            Else
                BindUserList()
                ClearAll()
                Dim udtGeneralFunction As New GeneralFunction
                udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
                udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)
                SetControlState(StateType.LOADED)
            End If
        Else
            strUserID = Me.ddlUserList.SelectedValue
            Reload(strUserID)
        End If


    End Sub

    Private Sub Reload(ByVal strUserID As String)
        SetControlState(StateType.LOADED)
        blnUserListIndexChange = False
        BindUserList()

        Dim udtHCVUUser As HCVUUserModel

        SetUserList(strUserID)
        udtHCVUUser = SetUserInfo(strUserID)
        SetUserRole(udtHCVUUser)
        SetAccessRight()
        SetFileGenerationRight()

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        displayInfoResetPWMsgBox(udtHCVUUser)
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]


        Me.udcMessageBox.Visible = False
        Me.ibtnDeActivateToken.Visible = False
        If Me.chkDisplayInactive.Checked Then
            If Me.lblTokenSN.Text <> "--" Then
                'Me.ibtnDeActivateToken.Visible = True
                Me.ibtnDeActivateToken.Visible = False
            End If
        End If
        ResetAlertImage()

    End Sub

    Private Sub chklRole_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chklRole.SelectedIndexChanged

        Me.udcMessageBox.Visible = False

        KeepScrollPos()
        SetAccessRight()
        SetFileGenerationRight()
    End Sub

    Private Sub ibtnResetPassword_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnResetPassword.Click
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("User_ID", Me.ddlUserList.SelectedValue)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, "Reset Password Click")
        ' CRE11-021 log the missed essential information [End]

        Me.udcMessageBox.Visible = False

        Me.ddlUserList.Enabled = False
        Me.chkDisplayInactive.Enabled = False
        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
        udtGeneralFunction.UpdateImageURL(ibtnNew, False)
        udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)

        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "ibtnResetPasswordScript", "setTimeout('showResetPasswordConfirm()', 1)", True)

    End Sub

    Private Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click

        Dim udtAuditLogEntry As AuditLogEntry
        Dim strAuditLogDesc As String
        Dim strLogID As String
        Dim strSuccessLogID As String
        Dim strFailLogID As String
        udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010501, Me)
        If ViewState("state") = StateType.ADD Then
            strAuditLogDesc = "Save New User Account"
            strLogID = LogID.LOG00004
            strSuccessLogID = LogID.LOG00005
            strFailLogID = LogID.LOG00006
            udtAuditLogEntry.AddDescripton("User_ID", Me.txtLoginID.Text)
        Else
            strAuditLogDesc = "Save Edit User Account"
            strLogID = LogID.LOG00007
            strSuccessLogID = LogID.LOG00008
            strFailLogID = LogID.LOG00009
            udtAuditLogEntry.AddDescripton("User_ID", Me.lblLoginID.Text.Trim)
        End If
        udtAuditLogEntry.AddDescripton("User_Name", Me.txtUserName.Text)
        udtAuditLogEntry.AddDescripton("HKID", Me.txtHKID.Text)
        udtAuditLogEntry.AddDescripton("Effective_Date", Me.txtEffectiveDate.Text)
        udtAuditLogEntry.AddDescripton("Expiry_Date", Me.txtExpiryDate.Text)
        udtAuditLogEntry.AddDescripton("Token_Serial_No", Me.txtTokenSN.Text)
        udtAuditLogEntry.AddDescripton("Record_Status", Me.chkSuspended.Checked)

        Dim strTemp As String = String.Empty

        Dim liRole As ListItem
        For Each liRole In Me.chklRole.Items
            If liRole.Selected Then
                If strTemp.Equals(String.Empty) Then
                    strTemp = liRole.Text
                Else
                    strTemp = strTemp & ", " & liRole.Text
                End If
            End If
        Next
        udtAuditLogEntry.AddDescripton("Role", strTemp)
        strTemp = String.Empty

        'Check for Scheme selection
        Dim liScheme As ListItem
        For Each liScheme In Me.ckbScheme.Items
            If liScheme.Selected Then
                If strTemp.Equals(String.Empty) Then
                    strTemp = liScheme.Text
                Else
                    strTemp = strTemp & ", " & liScheme.Text
                End If
            End If
        Next
        udtAuditLogEntry.AddDescripton("Scheme", strTemp)

        udtAuditLogEntry.WriteStartLog(strLogID, strAuditLogDesc)

        udtAuditLogEntry.AddDescripton("User_Name", Me.txtUserName.Text)
        udtAuditLogEntry.AddDescripton("HKID", Me.txtHKID.Text)
        udtAuditLogEntry.AddDescripton("Effective_Date", Me.txtEffectiveDate.Text)
        udtAuditLogEntry.AddDescripton("Expiry_Date", Me.txtExpiryDate.Text)
        udtAuditLogEntry.AddDescripton("Token_Serial_No", Me.txtTokenSN.Text)
        udtAuditLogEntry.AddDescripton("Record_Status", Me.chkSuspended.Checked)

        If IsValidated(udtAuditLogEntry, strFailLogID) Then
            Try
                Dim udtHCVUUserBLL As New HCVUUserBLL
                Dim udtUserAccountMaintBLL As New UserAccountMaintBLL
                Dim udtHCVUUser As HCVUUserModel = Nothing
                Dim strUserID As String = ""
                Dim udtCurHCVUUser As HCVUUserModel
                Dim udtTokenBLL As New TokenBLL
                udtCurHCVUUser = udtHCVUUserBLL.GetHCVUUser
                udtHCVUUser = Fill()
                Dim udtToken As New TokenModel

                udtToken.UserID = udtHCVUUser.UserID
                udtToken.UpdateBy = udtCurHCVUUser.UserID
                udtToken.IssueBy = udtCurHCVUUser.UserID
                udtToken.RecordStatus = TokenStatus.Active
                udtToken.Project = TokenProjectType.EHCVS
                udtToken.TokenSerialNo = Me.txtTokenSN.Text.Trim

                If ViewState("state") = StateType.ADD Then
                    'strPassword = Encrypt.MD5hash(udtUserAccountMaintBLL.GetDefaultPassword(udtHCVUUser))
                    'udtUserAccountMaintBLL.AddHCVUUserInfo(udtHCVUUser, strPassword, udtCurHCVUUser.UserID)

                    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                    Dim udtPassword As HashModel = Hash(udtUserAccountMaintBLL.GetDefaultPassword(udtHCVUUser))
                    udtUserAccountMaintBLL.AddHCVUUserInfo(udtHCVUUser, udtPassword, udtCurHCVUUser.UserID)
                    Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
                    Me.udcInfoMessageBox.AddMessage("010501", "I", "00001", "%s", "" & udtHCVUUser.UserID & "")
                    Me.udcInfoMessageBox.BuildMessageBox()
                    udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & " successful")
                Else
                    Dim udtUpdHCVUUser As HCVUUserModel
                    udtUpdHCVUUser = CType(Session(SESS_USERACCOUNTMAINT), HCVUUserModel)
                    udtUserAccountMaintBLL.UpdateHCVUUserInfo(udtHCVUUser, udtCurHCVUUser.UserID, udtUpdHCVUUser)
                    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
                    Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
                    Me.udcInfoMessageBox.AddMessage("010501", "I", "00002", "%s", "" & udtHCVUUser.UserID & "")
                    Me.udcInfoMessageBox.BuildMessageBox()
                    udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & " successful")

                    ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
                    ' When unlock account, reset RSA Token
                    If udtUpdHCVUUser.Locked <> udtHCVUUser.Locked AndAlso udtHCVUUser.Token.TokenSerialNo <> String.Empty Then
                        If (New TokenBLL).IsEnableToken Then

                            ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                            Dim udtDB As New Database
                            Try
                                udtDB.BeginTransaction()

                                Dim RSA_Manager As New Common.Component.RSA_Manager.RSAServerHandler
                                If RSA_Manager.IsParallelRun Then
                                    Call (New TokenBLL).UpdateRSASingletonTSMP(udtDB)
                                End If
                                RSA_Manager.resetRSAUserToken(udtHCVUUser.Token.TokenSerialNo.Trim)

                                udtDB.CommitTransaction()
                            Catch ex As Exception
                                udtDB.RollBackTranscation()
                                Throw
                            End Try
                            ' CRE15-001 RSA Server Upgrade [End][Winnie]
                        End If
                    End If
                    ' CRE13-029 - RSA Server Upgrade [End][Lawrence]

                End If

                Me.chkDisplayInactive.Checked = False
                BindUserList()
                ClearAll()
                Dim udtGeneralFunction As New GeneralFunction
                udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
                udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)
                SetControlState(StateType.LOADED)
            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim strmsg As String
                    strmsg = eSQL.Message
                    Dim udtSytemMessage As Common.ComObject.SystemMessage
                    udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                    Me.udcMessageBox.AddMessage(udtSytemMessage)
                    If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                        udcMessageBox.Visible = False
                    Else
                        udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, strFailLogID, strAuditLogDesc & " failed")
                        If strmsg = "00012" Then
                            'The Login ID is used  by others.
                            Me.imgLoginIDAlert.Visible = True
                        ElseIf strmsg = "00013" Then
                            'The HKIC No. already has an account.
                            Me.imgHKIDAlert.Visible = True
                        End If
                    End If
                Else
                    Throw eSQL
                End If
            Catch ex As Exception
                'If ex.Message = "Unable to add RSA User" Then
                If ex.Message.Contains("Unable to add RSA User") Then
                    Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00011)
                    Me.udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, strFailLogID, strAuditLogDesc & " failed")
                    Me.imgTokenSNAlert.Visible = True
                    'ElseIf ex.Message = "Unable to update RSA User" Then
                ElseIf ex.Message.Contains("Unable to update RSA User") Then
                    Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00012)
                    Me.udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, strFailLogID, strAuditLogDesc & " failed")
                    Me.imgTokenSNAlert.Visible = True
                    'ElseIf ex.Message = "Unable to delete RSA User" Then
                ElseIf ex.Message.Contains("Unable to delete RSA User") Then
                    Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00013)
                    Me.udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, strFailLogID, strAuditLogDesc & " failed")
                    Me.imgTokenSNAlert.Visible = True
                Else
                    Throw ex
                End If
            End Try

        End If

    End Sub

    Private Sub ResetAlertImage()
        Me.imgLoginIDAlert.Visible = False
        Me.imgUserNameAlert.Visible = False
        Me.imgHKIDAlert.Visible = False
        Me.imgEffectiveDateAlert.Visible = False
        Me.imgExpiryDateAlert.Visible = False
        Me.imgTokenSNAlert.Visible = False
        Me.imgRoleAlert.Visible = False
        Me.imgSchemeAlert.Visible = False
        Me.imgGenderAlert.Visible = False
    End Sub

    Private Function chkLoginID(ByVal strLoginID As String) As SystemMessage
        Dim udtValidator As New Validator
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim i As Integer

        'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence]
        Dim strUserIDRegEx As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameter("LoginIDRegEx", strUserIDRegEx, String.Empty)
        'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]

        If strLoginID = "" Then
            udtSystemMessage = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00042)
        ElseIf strLoginID.ToUpper.Equals("ADMINISTRATOR") Then
            udtSystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017)
        ElseIf strLoginID.Length < 4 Or strLoginID.Length > 20 Then
            udtSystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
        ElseIf strLoginID.Contains(" ") Then
            udtSystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016)
        ElseIf strLoginID.Length = 8 Then
            Try
                i = CInt(strLoginID)
                udtSystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017)
            Catch ex As Exception

            End Try

            'CRE14-001 Revise creation criteria for user id in eHS [Start] [Lawrence] 
        ElseIf (New Regex(strUserIDRegEx)).IsMatch(strLoginID) = False Then
            udtSystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016)
        End If
        'CRE14-001 Revise creation criteria for user id in eHS [End] [Lawrence]

        Return udtSystemMessage
    End Function

    Public Function IsValidated(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strFailLogID As String) As Boolean
        Dim udtStateType As StateType
        Dim udtValidator As New Validator
        Dim udtFormatter As New Formatter
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim strAuditLogDesc As String = ""

        Dim blnChangedEffectiveDate As Boolean = True
        Dim blnChangedExpiryDate As Boolean = True

        Dim blnOriginalGenderProvided As Boolean = False

        Me.udcMessageBox.Visible = False
        ResetAlertImage()

        udtStateType = ViewState("state")

        Select Case udtStateType
            Case StateType.ADD
                Dim udtSMLoginID As SystemMessage = Nothing
                udtSMLoginID = chkLoginID(Me.txtLoginID.Text.Trim)
                If Not udtSMLoginID Is Nothing Then
                    Me.udcMessageBox.AddMessage(udtSMLoginID)
                    Me.imgLoginIDAlert.Visible = True
                    strAuditLogDesc = "Save New User Account failed"
                End If
            Case StateType.EDIT
                strAuditLogDesc = "Save Edit User Account failed"
                Dim udtHCVUUser As HCVUUserModel
                udtHCVUUser = CType(Session(SESS_USERACCOUNTMAINT), HCVUUserModel)
                Dim _dtmEffectiveDate As DateTime
                Dim _dtmExpiryDate As Nullable(Of DateTime)
                _dtmEffectiveDate = udtHCVUUser.EffectiveDate
                _dtmExpiryDate = udtHCVUUser.ExpiryDate

                Dim _strEffectiveDate As String = ""
                Dim _strExpiryDate As String = ""

                _strEffectiveDate = _dtmEffectiveDate.ToString(udtFormatter.EnterDateFormat)
                If _dtmExpiryDate.HasValue Then
                    _strExpiryDate = _dtmExpiryDate.Value.ToString(udtFormatter.EnterDateFormat)
                End If

                If _strEffectiveDate.Trim = Me.txtEffectiveDate.Text.Trim Then
                    blnChangedEffectiveDate = False
                End If
                If _strExpiryDate.Trim = Me.txtExpiryDate.Text.Trim Then
                    blnChangedExpiryDate = False
                End If

                ' CRE19-022 Inspection Module [Start][Winnie]
                Dim strOriginalGender As String = udtHCVUUser.Gender
                If strOriginalGender <> Gender.NotProvided Then
                    blnOriginalGenderProvided = True
                End If
                ' CRE19-022 Inspection Module [End][Winnie]

            Case StateType.LOADED

        End Select

        Dim udtSMEngName As SystemMessage = Nothing
        udtSMEngName = udtValidator.chkEngName(Me.txtSurname.Text.Trim, Me.txtGivenname.Text.Trim)
        If Not udtSMEngName Is Nothing Then
            Me.udcMessageBox.AddMessage(udtSMEngName)
            Me.imgUserNameAlert.Visible = True
        End If

        If Me.txtHKID.Text.Trim = "" Then
            Me.udcMessageBox.AddMessage("990000", "E", "00001")
            Me.imgHKIDAlert.Visible = True
        Else
            Dim strHKID As String
            strHKID = udtFormatter.formatHKIDInternal(Me.txtHKID.Text.Trim)
            udtSystemMessage = udtValidator.chkHKID(strHKID)
            Me.udcMessageBox.AddMessage(udtSystemMessage)
            If Not udtSystemMessage Is Nothing Then
                Me.imgHKIDAlert.Visible = True
            End If
        End If

        Dim blnEffectiveDateValid As Boolean = True
        Dim dtmEffectiveDate As DateTime
        Dim dtmExpiryDate As DateTime

        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If Me.txtEffectiveDate.Text.Trim = "" Then
        '    Me.udcMessageBox.AddMessage("010501", "E", "00002")
        '    Me.imgEffectiveDateAlert.Visible = True
        '    blnEffectiveDateValid = False
        'Else
        'udtSystemMessage = udtValidator.chkInputDate("010501", Me.txtEffectiveDate.Text, "00002", "00005", "")
        udtSystemMessage = udtValidator.chkInputDate(Me.txtEffectiveDate.Text, True, False)

        If Not udtSystemMessage Is Nothing Then
            If Not udtSystemMessage.MessageCode = "" Then
                'Me.udcMessageBox.AddMessage(udtSystemMessage)
                Me.udcMessageBox.AddMessage(udtSystemMessage, "%s", lblEffectiveDateText.Text)
                blnEffectiveDateValid = False
                Me.imgEffectiveDateAlert.Visible = True
            End If
        Else
            If blnChangedEffectiveDate Then
                dtmEffectiveDate = DateTime.ParseExact(Me.txtEffectiveDate.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
                If dtmEffectiveDate < DateTime.Today.Date Then
                    Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00014)
                    Me.imgEffectiveDateAlert.Visible = True
                End If
            End If
        End If
        'End If
        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

        If Not Me.txtExpiryDate.Text.Trim = "" Then

            'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'udtSystemMessage = udtValidator.chkInputDate("010501", Me.txtExpiryDate.Text, "", "00006", "")
            udtSystemMessage = udtValidator.chkInputDate(Me.txtExpiryDate.Text, False, False)

            If Not udtSystemMessage Is Nothing Then
                If Not udtSystemMessage.MessageCode = "" Then
                    'Me.udcMessageBox.AddMessage(udtSystemMessage)
                    Me.udcMessageBox.AddMessage(udtSystemMessage, "%s", lblExpiryDateText.Text)
                    Me.imgExpiryDateAlert.Visible = True
                End If
            Else
                If blnEffectiveDateValid = True Then
                    dtmEffectiveDate = DateTime.ParseExact(Me.txtEffectiveDate.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
                    dtmExpiryDate = DateTime.ParseExact(Me.txtExpiryDate.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
                    If dtmExpiryDate < DateTime.Today.Date AndAlso blnChangedExpiryDate Then
                        Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00015)
                        Me.imgExpiryDateAlert.Visible = True
                    Else
                        If dtmEffectiveDate >= dtmExpiryDate AndAlso (blnChangedExpiryDate Or blnChangedEffectiveDate) Then
                            Me.udcMessageBox.AddMessage("010501", "E", "00009")
                            Me.imgExpiryDateAlert.Visible = True
                        End If
                    End If
                End If
            End If
            'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
        End If

        'If txtTokenSN.Text.Trim.Equals(String.Empty) Then
        '    Me.udcMessageBox.AddMessage("010501", "E", "00003")
        '    Me.imgTokenSNAlert.Visible = True
        'End If

        'Check for at least one scheme selected
        Dim bSchemeSelected As Boolean = False
        Dim liScheme As ListItem
        For Each liScheme In Me.ckbScheme.Items
            If liScheme.Selected Then
                bSchemeSelected = True
                Exit For
            End If
        Next
        If bSchemeSelected = False Then
            Me.udcMessageBox.AddMessage("990000", "E", "00183")
            Me.imgSchemeAlert.Visible = True
        End If

        'Check for at least one role selected
        Dim blnSelected As Boolean = False
        Dim liRole As ListItem
        For Each liRole In Me.chklRole.Items
            If liRole.Selected Then
                blnSelected = True
                Exit For
            End If
        Next
        If blnSelected = False Then
            Me.udcMessageBox.AddMessage("010501", "E", "00004")
            Me.imgRoleAlert.Visible = True
        End If

        'Check Gender whether must be provided
        If rdlGender.SelectedValue = Gender.NotProvided Then
            Dim blnMustProvideGender As Boolean = False

            ' If Gender has been provided before
            If blnOriginalGenderProvided Then
                blnMustProvideGender = True
            Else

                'if inspection officer is selected
                Dim isInspectionOfficerSelected As Boolean = False
                For Each item As ListItem In chklRole.Items
                    If item.Selected AndAlso item.Value = RoleType.InspectionOfficer Then
                        blnMustProvideGender = True
                        Exit For
                    End If
                Next
            End If

            If blnMustProvideGender Then
                Me.imgGenderAlert.Visible = True
                Me.udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00445, "%s", Me.GetGlobalResourceObject("Text", "Gender"))
            End If
        End If

        ' 
        Dim blnValidated As Boolean = True

        If Me.udcMessageBox.GetCodeTable.Rows.Count > 0 Then
            blnValidated = False
        End If

        Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, strFailLogID, strAuditLogDesc)

        Return blnValidated

    End Function

    Private Function Fill() As HCVUUserModel
        Dim udtHCVUUser As HCVUUserModel
        udtHCVUUser = New HCVUUserModel
        Dim udtFormatter As New Formatter
        Dim liRole As ListItem
        Dim i As Integer
        Dim udtUserRole As UserRoleModel
        Dim udtUserRoleCollection As New UserRoleModelCollection

        With udtHCVUUser
            .UserID = Me.txtLoginID.Text.Trim.ToUpper()

            If Not Me.txtGivenname.Text.Trim = "" Then
                .UserName = Me.txtSurname.Text.Trim & ", " & Me.txtGivenname.Text.Trim
            Else
                .UserName = Me.txtSurname.Text.Trim
            End If
            .HKID = udtFormatter.formatHKIDInternal(Me.txtHKID.Text.Trim)
            .EffectiveDate = DateTime.ParseExact(Me.txtEffectiveDate.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
            .Suspended = Me.chkSuspended.Checked
            .Locked = Me.chkAccountLocked.Checked
            If Not Me.txtExpiryDate.Text.Trim = "" Then
                .ExpiryDate = DateTime.ParseExact(Me.txtExpiryDate.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
            End If
            For i = 0 To Me.chklRole.Items.Count - 1
                liRole = CType(Me.chklRole.Items(i), ListItem)
                If liRole.Selected Then
                    For Each liS As ListItem In Me.ckbScheme.Items
                        If liS.Selected = True Then
                            udtUserRole = New UserRoleModel
                            udtUserRole.UserID = .UserID
                            udtUserRole.RoleType = liRole.Value
                            udtUserRole.SchemeCode = liS.Value

                            If Not IsNothing(udtUserRole) Then
                                udtUserRoleCollection.Add(udtUserRole)
                            End If
                        End If
                    Next
                End If
            Next
            .UserRoleCollection = udtUserRoleCollection

            Dim udtToken As New TokenModel
            udtToken.TokenSerialNo = Me.txtTokenSN.Text.Trim
            .Token = udtToken
        End With

        ' CRE19-022 - Inspection [Begin][Golden]
        udtHCVUUser.ChineseName = txtChineseName.Text.Trim
        udtHCVUUser.Gender = rdlGender.SelectedValue
        udtHCVUUser.ContactNo = txtContactNo.Text
        ' CRE19-022 - Inspection [End][Golden]

        Return udtHCVUUser
    End Function

    Private Sub chkDisplayInactive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDisplayInactive.CheckedChanged

        BindUserList()
        ClearAll()
        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
        udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)

    End Sub

    Private Sub ibtnDialogConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogConfirm.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010501, Me)


        Me.ddlUserList.Enabled = True
        Me.chkDisplayInactive.Enabled = True
        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "ibtnDialogConfirmScript", "setTimeout('hiddenResetPasswordConfirm()', 1)", True)

        Dim udtUserAccountMaintBLL As New UserAccountMaintBLL
        Dim udtHCVUUser As HCVUUserModel = Nothing

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtCurHCVUUser As HCVUUserModel
        udtCurHCVUUser = udtHCVUUserBLL.GetHCVUUser
        udtAuditLogEntry.AddDescripton("User_ID", udtCurHCVUUser.UserID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Reset Password")

        Dim dtHCVUUser As DataTable = Nothing
        Dim udtDB As Database = New Database

        udtHCVUUser = CType(Session(SESS_USERACCOUNTMAINT), HCVUUserModel)

        Try
            udtDB.BeginTransaction()

            udtUserAccountMaintBLL.ResetPassword(udtHCVUUser, udtCurHCVUUser.UserID)
            'Reset Token
            ' CRE13-029 - RSA server upgrade [Start][Lawrence]
            If (New TokenBLL).IsEnableToken Then
                Dim udtRSA As New Common.Component.RSA_Manager.RSAServerHandler

                ' CRE15-001 RSA Server Upgrade [Start][Winnie]
                If udtRSA.IsParallelRun Then
                    Call (New TokenBLL).UpdateRSASingletonTSMP(udtDB)
                End If
                ' CRE15-001 RSA Server Upgrade [End][Winnie]

                udtRSA.resetRSAUserToken(udtHCVUUser.Token.TokenSerialNo.Trim)
            End If
            ' CRE13-029 - RSA server upgrade [End][Lawrence]

            udtDB.CommitTransaction()

        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message
                Dim udtSytemMessage As Common.ComObject.SystemMessage
                udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                Me.udcMessageBox.AddMessage(udtSytemMessage)
                'udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, "Reset Password failed")
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
            If udtHCVUUser.UserID.Trim = udtCurHCVUUser.UserID.Trim Then
                dtHCVUUser = udtHCVUUserBLL.GetHCVUUserForLogin(udtCurHCVUUser.UserID)
                If dtHCVUUser.Rows.Count = 1 Then
                    Dim drHCVUUser As DataRow
                    drHCVUUser = dtHCVUUser.Rows(0)
                    udtCurHCVUUser.LastPwdChangeDtm = IIf(drHCVUUser.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Pwd_Change_Dtm"))
                    udtCurHCVUUser.LastPwdChangeDuration = IIf(drHCVUUser.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Pwd_Change_Duration"))
                End If
                udtHCVUUserBLL.SaveToSession(udtCurHCVUUser)
            End If
            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
            Me.udcInfoMessageBox.AddMessage("010501", "I", "00003", "%s", "" & udtHCVUUser.UserID & "")
            Me.udcInfoMessageBox.BuildMessageBox()

            Me.chkDisplayInactive.Checked = False
            BindUserList()
            ClearAll()
            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
            udtGeneralFunction.UpdateImageURL(ibtnNew, True)
            udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)
            SetControlState(StateType.LOADED)
            udtAuditLogEntry.AddDescripton("User_ID", udtCurHCVUUser.UserID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Reset Password successful")
        Else
            Me.ddlUserList.Enabled = True
            Me.chkDisplayInactive.Enabled = True
            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.UpdateImageURL(ibtnEdit, True)
            udtGeneralFunction.UpdateImageURL(ibtnNew, True)
            udtGeneralFunction.UpdateImageURL(ibtnResetPassword, True)
            udtAuditLogEntry.AddDescripton("User_ID", udtCurHCVUUser.UserID)
            udcMessageBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00012, "Reset Password failed")
        End If

    End Sub

    Private Sub ibtnDialogCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogCancel.Click
        Me.ddlUserList.Enabled = True
        Me.chkDisplayInactive.Enabled = True
        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.UpdateImageURL(ibtnEdit, True)
        udtGeneralFunction.UpdateImageURL(ibtnNew, True)
        udtGeneralFunction.UpdateImageURL(ibtnResetPassword, True)
        ScriptManager.RegisterStartupScript(Me, Page.GetType(), "ibtnDialogCancelScript", "setTimeout('hiddenResetPasswordConfirm()', 1)", True)
    End Sub

    Private Sub ibtnDeActivateToken_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDeActivateToken.Click

        'Dim udtHCVUUserBLL As New HCVUUserBLL
        'Dim udtUserAccountMaintBLL As New UserAccountMaintBLL
        'Dim udtHCVUUser As HCVUUserModel = Nothing
        'Dim strUserID As String = ""
        'Dim udtCurHCVUUser As HCVUUserModel
        'Dim udtTokenBLL As New TokenBLL
        'udtCurHCVUUser = udtHCVUUserBLL.GetHCVUUser
        'udtHCVUUser = Fill()
        'Dim udtToken As New TokenModel

        'udtToken.UserID = udtHCVUUser.UserID
        'udtToken.UpdateBy = udtCurHCVUUser.UserID
        'udtToken.IssueBy = udtCurHCVUUser.UserID
        'udtToken.RecordStatus = TokenStatus.Active
        'udtToken.Project = TokenProjectType.EHCVS
        'udtToken.TokenSerialNo = Me.txtTokenSN.Text.Trim

        'Try
        '    Dim udtUpdHCVUUser As HCVUUserModel
        '    udtUpdHCVUUser = CType(Session(SESS_USERACCOUNTMAINT), HCVUUserModel)
        '    udtUserAccountMaintBLL.DeleteToken(udtHCVUUser, udtCurHCVUUser.UserID, udtUpdHCVUUser)
        '    Me.udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004, "%s", "" & udtHCVUUser.UserID & "")
        '    Me.udcInfoMessageBox.BuildMessageBox()

        '    Me.chkDisplayInactive.Checked = False
        '    BindUserList()
        '    ClearAll()
        '    Dim udtGeneralFunction As New GeneralFunction
        '    udtGeneralFunction.UpdateImageURL(ibtnEdit, False)
        '    udtGeneralFunction.UpdateImageURL(ibtnResetPassword, False)
        '    SetControlState(StateType.LOADED)

        'Catch ex As Exception
        '    If ex.Message = "Unable to add RSA User" Then
        '        Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00011)
        '        Me.udcMessageBox.BuildMessageBox("UpdateFail")
        '        Me.imgTokenSNAlert.Visible = True
        '    ElseIf ex.Message = "Unable to update RSA User" Then
        '        Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00012)
        '        Me.udcMessageBox.BuildMessageBox("UpdateFail")
        '        Me.imgTokenSNAlert.Visible = True
        '    ElseIf ex.Message = "Unable to delete RSA User" Then
        '        Me.udcMessageBox.AddMessage(FunctCode.FUNT010501, SeverityCode.SEVE, MsgCode.MSG00013)
        '        Me.udcMessageBox.BuildMessageBox("UpdateFail")
        '        Me.imgTokenSNAlert.Visible = True
        '    Else
        '        Throw ex
        '    End If
        'End Try
    End Sub

    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    Private Sub displayInfoResetPWMsgBox(udtHCVUUser As HCVUUserModel)
        Dim intPasswordLevel As Integer = udtHCVUUser.PasswordLevel
        Dim strUserID As String = udtHCVUUser.UserID

        Dim blnVerifyPWLevel As Boolean = VerifyPasswordLevel(intPasswordLevel)

        If blnVerifyPWLevel Then
            udcInfoMessageBox.Visible = False
        Else
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcInfoMessageBox.AddMessage(FunctCode.FUNT010501, "I", "00005", "%s", "" & strUserID & "")
            Me.udcInfoMessageBox.BuildMessageBox()
        End If
    End Sub
    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

#Region "Implement IWorkingData (CRE11-004)"

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
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
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

#End Region

End Class