Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.Address

Partial Public Class MedicalOrganization
    'Inherits System.Web.UI.Page
    Inherits BasePage

    Private Const LocalFunctionCode As String = FunctCode.FUNT020101
    Private Const GlobalFunctionCode As String = FunctCode.FUNT990000
    Private Const DatabaseFunctionCode As String = FunctCode.FUNT990001
    Private Const SESS_Practice As String = "PracticeBank"
    Private Const SESS_MO As String = "MO"
    Private Const SESS_Page As String = "PracticePage"
    Private Const SESS_PerviousPage As String = "PerviousPage"

    Private Const MaxRowNo As Integer = 5

    Private udtValidator As Common.Validation.Validator = New Common.Validation.Validator
    Private udtFormatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtSM As Common.ComObject.SystemMessage

    Private udtControlBLL As ControlBLL = New ControlBLL
    Private udtEFormBLL As eFormBLL = New eFormBLL
    Private udtSPBLL As ServiceProvider.ServiceProviderBLL = New ServiceProvider.ServiceProviderBLL

    Private Function LoadDemoData(ByVal index As Integer) As String
        Dim strDemoData As String = String.Empty
        If index = 0 Then
            strDemoData = "Medical Limited"
        End If
        If index = 1 Then
            strDemoData = "醫學公司"
        End If
        If index = 2 Then
            strDemoData = "BR123456"
        End If
        If index = 3 Then
            strDemoData = "23456789"
        End If
        If index = 4 Then
            strDemoData = "medical@medical.com"
        End If
        If index = 5 Then
            strDemoData = "12346570"
        End If
        If index = 6 Then
            strDemoData = "2"
        End If
        If index = 7 Then
            strDemoData = "3"
        End If
        If index = 8 Then
            strDemoData = "4"
        End If
        If index = 9 Then
            strDemoData = "Medical Road"
        End If
        If index = 10 Then
            strDemoData = "TM"
        End If
        If index = 11 Then
            strDemoData = "P"
        End If
        Return strDemoData
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            Dim strAbleToAccessThisPage As String = String.Empty
            strAbleToAccessThisPage = Session(eFormBLL.SESS_MedicalOrganization)
            udtEFormBLL.ClearRedirectPageSession()

            If IsNothing(strAbleToAccessThisPage) OrElse Not strAbleToAccessThisPage.Trim.Equals("Y") Then
                Response.Redirect("~/main.aspx")
            Else
                Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me) ''Begin Writing Audit Log

                Dim udtSP As ServiceProvider.ServiceProviderModel
                udtSP = udtSPBLL.GetSP

                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00006, "Medical Organization Page Loaded")

                Session(SESS_Page) = 1
                Me.lblMOPaging.Visible = True
                Dim strPageInfo As String

                strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

                If Not IsNothing(Session(SESS_MO)) Then

                    Dim dt As DataTable = Session(SESS_MO)
                    Me.formatMOGV(dt, False)

                    Dim doublePageCount As Double
                    doublePageCount = Math.Ceiling(CType(dt.Rows.Count, Double) / 5)

                    strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
                    strPageInfo = strPageInfo.Replace("%e", CStr(doublePageCount))
                    strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))

                Else
                    Me.gvMo.DataSource = Me.emptyMODataTable
                    Me.gvMo.DataBind()

                    strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
                    strPageInfo = strPageInfo.Replace("%e", "1")
                    strPageInfo = strPageInfo.Replace("%f", "1")

                    Dim content As ContentPlaceHolder
                    content = Page.Master.FindControl("ContentPlaceHolder1")

                    For i As Integer = 1 To 20
                        Dim l As LinkButton = CType(content.FindControl("lnkBtnPage" & i.ToString), LinkButton)

                        If Not IsNothing(l) Then
                            If i > 1 Then
                                l.Visible = False
                            Else
                                l.Visible = True
                            End If
                        End If
                    Next
                End If

                Me.lblMOPaging.Text = strPageInfo

                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Me.iBtnLoadDemoData.Visible = Me.IsDemo
                ' CRE12-001 eHS and PCD integration [End][Koala]

                Session(SESS_PerviousPage) = String.Empty
                Session.Remove(SESS_PerviousPage)
            End If

        End If

        txtCopyImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "CopyBtn")
        txtCopyDisableImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "CopyDisableBtn")

    End Sub

    Protected Sub ibtnRegMOBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        'udtAuditLogEntry.AddDescripton("HKID", lblConfirmHKID.Text)
        udcMsgBox.Visible = False

        Dim dt As DataTable
        dt = getMOFromGridView(False)


        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00013, "Press Back to Personal Particulars")
       
        udtEFormBLL.ClearRedirectPageSession()
        Session(eFormBLL.SESS_PersonalParticular) = "Y"
        Response.Redirect("~/PersonalPacticulars.aspx")
    End Sub


    Private Function getMOFromGridView(ByVal blnchecking As Boolean) As DataTable

        Dim i(6) As Integer
        Dim s(6) As String

        Dim smMO(6) As Common.ComObject.SystemMessage

        Dim grid As GridView = gvMo

        Dim dt As New DataTable
        If IsNothing(Session(SESS_MO)) Then
            dt = Me.emptyMODataTable
        Else
            dt = Session(SESS_MO)
        End If

        Dim blnCreateDT As Boolean = False

        For Each row As GridViewRow In grid.Rows

            Dim intMOIndex As Integer = CInt(CType(row.FindControl("lblRegMOIndex"), Label).Text.Trim) - 1
            Dim strRegMOEName As String = CType(row.FindControl("txtRegMOEName"), TextBox).Text.Trim
            Dim strRegMOCName As String = CType(row.FindControl("txtRegMOCName"), TextBox).Text.Trim
            Dim strRegMOBRCode As String = CType(row.FindControl("txtRegMOBRCode"), TextBox).Text.Trim
            Dim strRegMOContactNo As String = CType(row.FindControl("txtRegMOContactNo"), TextBox).Text.Trim
            Dim strRegMOEmail As String = CType(row.FindControl("txtRegMOEmail"), TextBox).Text.Trim
            Dim strRegMOFax As String = CType(row.FindControl("txtRegMOFax"), TextBox).Text.Trim
            Dim strRegMORoom As String = CType(row.FindControl("txtRegMORoom"), TextBox).Text.Trim
            Dim strRegMOFloor As String = CType(row.FindControl("txtRegMOFloor"), TextBox).Text.Trim
            Dim strRegMOBlock As String = CType(row.FindControl("txtRegMOBlock"), TextBox).Text.Trim
            Dim strRegMOEAddress As String = CType(row.FindControl("txtRegMOEAddress"), TextBox).Text.Trim
            Dim strRegMODistrict As String = CType(row.FindControl("ddlRegMODistrict"), DropDownList).SelectedValue.Trim

            Dim strRegMOArea As String = udtEFormBLL.getAreaString(strRegMODistrict)
            Dim strRegMORelation As String = CType(row.FindControl("rboRegMORelation"), RadioButtonList).SelectedValue.Trim
            Dim strRegMORelationRemark As String = CType(row.FindControl("txtRegMORelationRemark"), TextBox).Text.Trim

            Dim imgRegMOENameAlert As Image = row.FindControl("imgRegMOENameAlert")
            Dim imgRegMOCNameAlert As Image = row.FindControl("imgRegMOCNameAlert")
            Dim imgRegMOBRCodeAlert As Image = row.FindControl("imgRegMOBRCodeAlert")
            Dim imgRegMOContactNoAlert As Image = row.FindControl("imgRegMOContactNoAlert")
            Dim imgRegMOEmailAlert As Image = row.FindControl("imgRegMOEmailAlert")
            Dim imgRegMOFaxAlert As Image = row.FindControl("imgRegMOFaxAlert")
            Dim imgRegMOEAddressAlert As Image = row.FindControl("imgRegMOEAddressAlert")
            Dim imgRegMODistrictAlert As Image = row.FindControl("imgRegMODistrictAlert")
            Dim imgRegMORelationAlert As Image = row.FindControl("imgRegMORelationAlert")
            Dim imgRegMORelationRemarksAlert As Image = row.FindControl("imgRegMORelationRemarksAlert")

            imgRegMOENameAlert.Visible = False
            imgRegMOCNameAlert.Visible = False
            imgRegMOBRCodeAlert.Visible = False
            imgRegMOContactNoAlert.Visible = False
            imgRegMOEmailAlert.Visible = False
            imgRegMOFaxAlert.Visible = False
            imgRegMOEAddressAlert.Visible = False
            imgRegMODistrictAlert.Visible = False
            imgRegMORelationAlert.Visible = False
            imgRegMORelationRemarksAlert.Visible = False

            'If strRegMOEName.Equals(String.Empty) And strRegMOCName.Equals(String.Empty) And _
            '   strRegMOBRCode.Equals(String.Empty) And strRegMOContactNo.Equals(String.Empty) And _
            '   strRegMOEmail.Equals(String.Empty) And strRegMOFax.Equals(String.Empty) And _
            '   strRegMORoom.Equals(String.Empty) And strRegMOFloor.Equals(String.Empty) And _
            '   strRegMOBlock.Equals(String.Empty) And strRegMOEAddress.Equals(String.Empty) And _
            '   strRegMODistrict.Equals(String.Empty) And strRegMORelation.Equals(String.Empty) And _
            '   strRegMORelationRemark.Equals(String.Empty) Then

            '    blnCreateDT = False
            'Else
            '    blnCreateDT = True
            'End If


            dt.Rows(intMOIndex)("MOEName") = strRegMOEName
            dt.Rows(intMOIndex)("MOCName") = strRegMOCName
            dt.Rows(intMOIndex)("MOBRCode") = strRegMOBRCode
            dt.Rows(intMOIndex)("MOContactNo") = strRegMOContactNo
            dt.Rows(intMOIndex)("MOEmail") = strRegMOEmail
            dt.Rows(intMOIndex)("MOFax") = strRegMOFax
            dt.Rows(intMOIndex)("MORoom") = strRegMORoom
            dt.Rows(intMOIndex)("MOFloor") = strRegMOFloor
            dt.Rows(intMOIndex)("MOBlock") = strRegMOBlock
            dt.Rows(intMOIndex)("MOEAddress") = strRegMOEAddress
            dt.Rows(intMOIndex)("MODistrict") = strRegMODistrict
            dt.Rows(intMOIndex)("MORelation") = strRegMORelation
            dt.Rows(intMOIndex)("MORelationRemarks") = strRegMORelationRemark
            dt.Rows(intMOIndex)("Page") = Math.Ceiling(CType(intMOIndex + 1, Double) / 5)


            If blnchecking Then
                'check the english name of MO
                udtSM = udtValidator.chkMOEnglishName(strRegMOEName)
                If Not IsNothing(udtSM) Then
                    imgRegMOENameAlert.Visible = True
                    i(0) = i(0) + 1
                    s(0) = s(0) + ", " + (intMOIndex + 1).ToString
                    smMO(0) = udtSM
                End If

                ''check the business registration number of MO
                'udtSM = udtValidator.chkMOBRCode(strRegMOBRCode)
                'If Not IsNothing(udtSM) Then
                '    imgRegMOBRCodeAlert.Visible = True
                '    i(1) = i(1) + 1
                '    s(1) = s(1) + ", " + (intMOIndex + 1).ToString
                '    smMO(1) = udtSM
                'End If

                'check the contact daytime contact tel no of MO
                udtSM = udtValidator.chkMOContactNo(strRegMOContactNo)
                If Not IsNothing(udtSM) Then
                    imgRegMOContactNoAlert.Visible = True
                    i(1) = i(1) + 1
                    s(1) = s(1) + ", " + (intMOIndex + 1).ToString
                    smMO(1) = udtSM
                End If

                'check the contact email address of MO
                If Not strRegMOEmail.Equals(String.Empty) Then
                    udtSM = udtValidator.chkMOEmail(strRegMOEmail)
                    If Not IsNothing(udtSM) Then
                        imgRegMOEmailAlert.Visible = True
                        i(2) = i(2) + 1
                        s(2) = s(2) + ", " + (intMOIndex + 1).ToString
                        smMO(2) = udtSM
                    End If
                End If

                'check the address of MO
                udtSM = udtValidator.chkMOAddress(strRegMOEAddress, strRegMODistrict, strRegMOArea)
                If Not udtSM Is Nothing Then
                    If udtValidator.IsEmpty(strRegMOEAddress) Then
                        imgRegMOEAddressAlert.Visible = True
                    End If

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                    'If udtValidator.IsEmpty(strRegMODistrict) OrElse strRegMODistrict.Equals(".H") OrElse strRegMODistrict.Equals(".K") OrElse _
                    '    strRegMODistrict.Equals(".N") Then
                    If udtValidator.IsEmpty(strRegMODistrict) OrElse strRegMODistrict.StartsWith(".") Then
                        'CRE13-019-02 Extend HCVS to China [End][Winnie]
                        imgRegMODistrictAlert.Visible = True
                    End If

                    i(3) = i(3) + 1
                    s(3) = s(3) + ", " + (intMOIndex + 1).ToString
                    smMO(3) = udtSM

                End If

                'check the relation of MO
                udtSM = udtValidator.chkMORelation(strRegMORelation)
                If Not IsNothing(udtSM) Then
                    imgRegMORelationAlert.Visible = True
                    i(4) = i(4) + 1
                    s(4) = s(4) + ", " + (intMOIndex + 1).ToString
                    smMO(4) = udtSM
                End If


                If strRegMORelation.Equals("O") Then
                    If udtValidator.IsEmpty(strRegMORelationRemark) Then
                        imgRegMORelationRemarksAlert.Visible = True
                        udtSM = New Common.ComObject.SystemMessage(GlobalFunctionCode, Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00155)
                        i(5) = i(5) + 1
                        s(5) = s(5) + ", " + (intMOIndex + 1).ToString
                        smMO(5) = udtSM
                    End If
                End If

            End If

        Next

        If blnchecking Then


            If i(0) > 0 Then
                Dim str() As String = {s(0)}
                udcMsgBox.AddMessage(smMO(0), "%s", s(0).Substring(1))
            End If
            If i(1) > 0 Then
                Dim str() As String = {s(1)}
                udcMsgBox.AddMessage(smMO(1), "%s", s(1).Substring(1))
            End If
            If i(2) > 0 Then
                Dim str() As String = {s(2)}
                udcMsgBox.AddMessage(smMO(2), "%s", s(2).Substring(1))
            End If
            If i(3) > 0 Then
                Dim str() As String = {s(3)}
                udcMsgBox.AddMessage(smMO(3), "%s", s(3).Substring(1))
            End If
            If i(4) > 0 Then
                Dim str() As String = {s(4)}
                udcMsgBox.AddMessage(smMO(4), "%s", s(4).Substring(1))
            End If
            If i(5) > 0 Then
                Dim str() As String = {s(5)}
                udcMsgBox.AddMessage(smMO(5), "%s", s(5).Substring(1))
            End If
            'If i(6) > 0 Then
            '    Dim str() As String = {s(6)}
            '    udcMsgBox.AddMessage(smMO(6), "%s", s(6).Substring(1))
            'End If

        End If

        Session(SESS_MO) = dt
        Return dt

    End Function

    Private Function emptyMODataTable() As DataTable

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("MOEName"))
        dt.Columns.Add(New DataColumn("MOCName"))
        dt.Columns.Add(New DataColumn("MOBRCode"))
        dt.Columns.Add(New DataColumn("MOContactNo"))
        dt.Columns.Add(New DataColumn("MOEmail"))
        dt.Columns.Add(New DataColumn("MOFax"))
        dt.Columns.Add(New DataColumn("MORoom"))
        dt.Columns.Add(New DataColumn("MOFloor"))
        dt.Columns.Add(New DataColumn("MOBlock"))
        dt.Columns.Add(New DataColumn("MOEAddress"))
        dt.Columns.Add(New DataColumn("MODistrict"))
        dt.Columns.Add(New DataColumn("MORelation"))
        dt.Columns.Add(New DataColumn("MORelationRemarks"))
        dt.Columns.Add(New DataColumn("HCVS"))
        dt.Columns.Add(New DataColumn("IVSS"))
        dt.Columns.Add(New DataColumn("Page"))
        Return addEmptyDataRowToDataTable(dt)
    End Function

    Private Function addEmptyDataRowToDataTable(ByVal dt As DataTable) As DataTable
        Dim dr As DataRow
        dr = dt.NewRow

        Dim i As Integer

        For i = 0 To dt.Columns.Count - 1
            dr(i) = String.Empty
            'dr(i) = LoadDemoData(i)
        Next

        dt.Rows.Add(dr)

        Return dt
    End Function


    'Protected Sub ibtnCopyConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim i As Integer = 0
    '    Dim intComboBox As Integer = 0

    '    If Not IsNothing(Session(SESS_MO)) Then

    '        Dim dtMO As New DataTable
    '        dtMO = Session(SESS_MO)

    '        If Not ddlMOList.SelectedValue.Trim.Equals(String.Empty) Then
    '            i = CInt(ddlMOList.SelectedValue.Trim)
    '        End If

    '        Dim dt As DataTable = New DataTable

    '        dt = addEmptyDataRowToDataTable(dtMO)
    '        Dim intNewRow As Integer = dt.Rows.Count - 1

    '        ' MO Name Information
    '        If Me.choCopyList.Items(0).Selected Then
    '            dt.Rows(intNewRow).Item("MOEName") = dt.Rows(i).Item("MOEName")
    '            dt.Rows(intNewRow).Item("MOCName") = dt.Rows(i).Item("MOCName")
    '        End If

    '        ' MO BrCode Information
    '        If Me.choCopyList.Items(1).Selected Then
    '            dt.Rows(intNewRow).Item("MOBRCode") = dt.Rows(i).Item("MOBRCode")
    '        End If

    '        ' MO Phone Information
    '        If Me.choCopyList.Items(2).Selected Then
    '            dt.Rows(intNewRow).Item("MOContactNo") = dt.Rows(i).Item("MOContactNo")
    '        End If

    '        ' MO Address Information
    '        If Me.choCopyList.Items(3).Selected Then
    '            dt.Rows(intNewRow).Item("MORoom") = dt.Rows(i).Item("MORoom")
    '            dt.Rows(intNewRow).Item("MOFloor") = dt.Rows(i).Item("MOFloor")
    '            dt.Rows(intNewRow).Item("MOBlock") = dt.Rows(i).Item("MOBlock")
    '            dt.Rows(intNewRow).Item("MOEAddress") = dt.Rows(i).Item("MOEAddress")
    '            dt.Rows(intNewRow).Item("MODistrict") = dt.Rows(i).Item("MODistrict")
    '        End If

    '        ' Relation Information
    '        If Me.choCopyList.Items(4).Selected Then
    '            dt.Rows(intNewRow).Item("MORelation") = dt.Rows(i).Item("MORelation")
    '            dt.Rows(intNewRow).Item("MORelationRemarks") = dt.Rows(i).Item("MORelationRemarks")
    '        End If


    '        dt.Rows(intNewRow).Item("Page") = Math.Ceiling(CType(intNewRow + 1, Double) / 5)

    '        formatMOGV(dt, False)

    '        Me.ModalPopupExtenderCopyList.Hide()
    '        Me.choCopyList.ClearSelection()
    '    Else
    '        ModalPopupExtenderCopyList.Hide()
    '    End If

    'End Sub

    'Protected Sub ibtnNewMO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim dtMO As DataTable = New DataTable
    '    dtMO = Session(SESS_MO)
    '    'gvMo.DataSource = addEmptyDataRowToDataTable(dtMO)
    '    'gvMo.DataBind()
    '    formatMOGV(dtMO, True)
    '    Me.ModalPopupExtenderCopyList.Hide()
    '    Me.choCopyList.ClearSelection()

    'End Sub

    'Protected Sub ibtnCopyCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Me.ModalPopupExtenderCopyList.Hide()
    '    Me.choCopyList.ClearSelection()
    'End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        For Each gvr As GridViewRow In Me.gvMo.Rows
            Dim rboRegMORelation As RadioButtonList = gvr.FindControl("rboRegMORelation")
            Dim txtRegMORelationRemark As TextBox = gvr.FindControl("txtRegMORelationRemark")
            Dim lblPleaseSpecify As Label = gvr.FindControl("lblPleaseSpecify")

            If Not rboRegMORelation.SelectedValue.Trim.Equals(String.Empty) Then

                If rboRegMORelation.SelectedValue.Trim.Equals("O") Then
                    'txtRegMORelationRemark.BackColor = Nothing
                    txtRegMORelationRemark.Attributes.Remove("readonly")
                    lblPleaseSpecify.CssClass = ""
                Else
                    'txtRegMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                    txtRegMORelationRemark.Attributes.Add("readonly", "readonly")
                    lblPleaseSpecify.CssClass = "dimText"
                End If
            Else

                'txtRegMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtRegMORelationRemark.Attributes.Add("readonly", "readonly")
                lblPleaseSpecify.CssClass = "dimText"
            End If

        Next

        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) Then
                RenderLanguage()
            End If
        End If
    End Sub

    Private Sub RenderLanguage()

        If Not IsNothing(gvMo) Then
            For Each row As GridViewRow In gvMo.Rows
                Dim ddlRegMODistrict As DropDownList = CType(row.FindControl("ddlRegMODistrict"), DropDownList)

                If Not IsNothing(ddlRegMODistrict) Then
                    ddlRegMODistrict.Items(0).Text = Me.GetGlobalResourceObject("Text", "SelectDistrict")
                End If

                Dim rboRegMORelation As RadioButtonList = CType(row.FindControl("rboRegMORelation"), RadioButtonList)

                If Not IsNothing(rboRegMORelation) Then
                    Dim strMORelation As String = rboRegMORelation.SelectedValue.Trim

                    udtControlBLL.bindMORelationship(rboRegMORelation)
                    If Not strMORelation.Equals(String.Empty) Then
                        rboRegMORelation.SelectedValue = strMORelation
                    End If

                End If

            Next

            If Me.lblMOPaging.Visible Then

                Dim dtMO As DataTable
                dtMO = Session(SESS_MO)

                Dim strPageInfo As String
                Dim doublePageCount As Double

                strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")
                If IsNothing(dtMO) Then
                    doublePageCount = Math.Ceiling(1 / 5)
                Else
                    doublePageCount = Math.Ceiling(CType(dtMO.Rows.Count, Double) / 5)
                End If


                strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
                strPageInfo = strPageInfo.Replace("%e", CStr(doublePageCount))
                If IsNothing(dtMO) Then
                    strPageInfo = strPageInfo.Replace("%f", "1")
                Else
                    strPageInfo = strPageInfo.Replace("%f", CStr(dtMO.Rows.Count))
                End If


                Me.lblMOPaging.Text = strPageInfo

            End If
        End If
    End Sub

    Private Sub formatMOGV(ByVal dt As DataTable, ByVal blnNewRow As Boolean)
        Dim dv As DataView

        If blnNewRow Then
            Dim dtMO As DataTable
            dtMO = addEmptyDataRowToDataTable(dt)
            dv = New DataView(dtMO)
        Else
            dv = New DataView(dt)
        End If


        Dim intPage As Integer

        intPage = Math.Ceiling(CType(dt.Rows.Count, Double) / 5)

        Session(SESS_Page) = intPage

        dv.RowFilter = "Page = '" + intPage.ToString + "' or Page = ''"
        gvMo.DataSource = dv
        gvMo.DataBind()


        Me.lblMOPaging.Visible = True

        Dim strPageInfo As String

        strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

        strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
        strPageInfo = strPageInfo.Replace("%e", CStr(intPage))
        strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))

        Me.lblMOPaging.Text = strPageInfo

        Dim content As ContentPlaceHolder
        content = Page.Master.FindControl("ContentPlaceHolder1")

        For i As Integer = 1 To 20
            Dim l As LinkButton = CType(content.FindControl("lnkBtnPage" & i.ToString), LinkButton)

            If Not IsNothing(l) Then
                If i > intPage Then
                    l.Visible = False
                Else
                    l.Visible = True
                End If
            End If
        Next

    End Sub

    Protected Sub LinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnPage1.Click, lnkBtnPage2.Click, lnkBtnPage3.Click, lnkBtnPage4.Click, lnkBtnPage5.Click, lnkBtnPage6.Click, lnkBtnPage7.Click, lnkBtnPage8.Click, lnkBtnPage9.Click, lnkBtnPage10.Click, lnkBtnPage11.Click, lnkBtnPage12.Click, lnkBtnPage13.Click, lnkBtnPage14.Click, lnkBtnPage15.Click, lnkBtnPage16.Click, lnkBtnPage17.Click, lnkBtnPage18.Click, lnkBtnPage19.Click, lnkBtnPage20.Click
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim lnkBtn As New LinkButton
        lnkBtn = CType(sender, LinkButton)

        'lnkBtn.Enabled = False

        Dim intPage As Integer
        intPage = CInt(lnkBtn.Text)

        Dim content As ContentPlaceHolder
        content = Page.Master.FindControl("ContentPlaceHolder1")

        'For i As Integer = 1 To Session(SESS_Page)
        '    If i <> intPage Then
        '        Dim l As LinkButton = CType(content.FindControl("lnkBtnPage" & i.ToString), LinkButton)
        '        If Not IsNothing(l) Then
        '            l.Enabled = True
        '        End If
        '    End If
        'Next

        Dim intExistPage As Integer
        intExistPage = Session(SESS_Page)

        Dim blnOtherPage As Boolean = False

        Session(SESS_Page) = intPage

        If intExistPage <> intPage Then
            blnOtherPage = True
        End If

        Dim dt As DataTable

        If blnOtherPage Then
            dt = Me.getMOFromGridView(True)
        Else
            dt = Me.getMOFromGridView(udcMsgBox.Visible)
        End If

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
        udtAuditLogEntry.AddDescripton("Go to Page", intPage)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00015, "Click Page Link in MO")

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False

            If blnOtherPage Then
                If Not IsNothing(Session(SESS_MO)) Then

                    Dim dv As DataView = New DataView(dt)

                    dv.RowFilter = "Page = '" + intPage.ToString + "' or Page = ''"
                    gvMo.DataSource = dv
                    gvMo.DataBind()

                    Me.lblMOPaging.Visible = True

                    Dim strPageInfo As String

                    strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

                    strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
                    strPageInfo = strPageInfo.Replace("%e", CStr(intPage))
                    strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))

                    Me.lblMOPaging.Text = strPageInfo

                    udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                    udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
                    udtAuditLogEntry.AddDescripton("Go to Page", intPage)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00016, "Click Page Link in MO Complete")

                End If
            Else
                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
                udtAuditLogEntry.AddDescripton("Go to Page", intPage)
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00065, "Click Page Link in MO Complete at the same page")
            End If

        Else
            'udcMsgBox.BuildMessageBox("ValidationFail")
            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
            udtAuditLogEntry.AddDescripton("Go to Page", intPage)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00017, "Click Page Link in MO Fail")
        End If

    End Sub


    Protected Sub ibtnRegMONext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        'udtAuditLogEntry.AddDescripton("HKID", lblConfirmHKID.Text)
        udcMsgBox.Visible = False

        Dim dt As DataTable
        dt = getMOFromGridView(True)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00010, "Input Medical Organization")

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False

            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00011, "Input Medical Organization Complete")
            udtEFormBLL.ClearRedirectPageSession()

            Session(eFormBLL.SESS_Practice) = "Y"
            Response.Redirect("~/Practice.aspx")

        Else
            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00012, "Input Medical Organization Fail")
        End If

    End Sub

    Private Sub gvMo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlRegMODistrict As DropDownList = e.Row.FindControl("ddlRegMODistrict")
            Dim hfRegMODistrict As HiddenField = e.Row.FindControl("hfRegMODistrict")

            udtControlBLL.bindDistrict(ddlRegMODistrict, String.Empty, False)
            ddlRegMODistrict.SelectedValue = hfRegMODistrict.Value

            Dim rboRegMORelation As RadioButtonList = e.Row.FindControl("rboRegMORelation")
            Dim hfRegMORelation As HiddenField = e.Row.FindControl("hfRegMORelation")
            Dim txtRegMORelationRemark As TextBox = e.Row.FindControl("txtRegMORelationRemark")
            Dim lblPleaseSpecify As Label = e.Row.FindControl("lblPleaseSpecify")

            udtControlBLL.bindMORelationship(rboRegMORelation)
            rboRegMORelation.Attributes.Add("onclick", "javascript:enableRemarkTextbox('" + rboRegMORelation.ClientID + "', '" + txtRegMORelationRemark.ClientID + "', '" + lblPleaseSpecify.ClientID + "')")


            If Not hfRegMORelation.Value.Equals(String.Empty) Then
                rboRegMORelation.SelectedValue = hfRegMORelation.Value

                If hfRegMORelation.Value.Trim.Equals("O") Then
                    'txtRegMORelationRemark.BackColor = Nothing
                    txtRegMORelationRemark.Attributes.Remove("readonly")
                    lblPleaseSpecify.CssClass = ""
                Else
                    'txtRegMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                    txtRegMORelationRemark.Attributes.Add("readonly", "readonly")
                    lblPleaseSpecify.CssClass = "dimText"
                End If
            Else

                'txtRegMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtRegMORelationRemark.Attributes.Add("readonly", "readonly")
                lblPleaseSpecify.CssClass = "dimText"
            End If
        End If
    End Sub

    Private Sub gvMo_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvMo.RowDeleting
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        udcMsgBox.Visible = False

        Dim dtMO As New DataTable
        dtMO = Me.getMOFromGridView(False)

        Dim gvRow As GridViewRow = gvMo.Rows(e.RowIndex)

        Dim intIndex As Integer = CInt(CType(gvRow.FindControl("lblRegMOIndex"), Label).Text.Trim) - 1

        dtMO.Rows(intIndex).Delete()

        Dim i As Integer = 0
        For Each dr As DataRow In dtMO.Rows
            dr.Item("Page") = Math.Ceiling(CType(i + 1, Double) / 5)
            i = i + 1
        Next

        If dtMO.Rows.Count = 0 Then
            gvMo.DataSource = Me.emptyMODataTable()
            gvMo.DataBind()
            Session(SESS_MO) = Nothing

        Else
            'dtMO = Me.getMOFromGridView(False)
            Session(SESS_MO) = dtMO

            gvMo.DataSource = dtMO
            gvMo.DataBind()

            formatMOGV(dtMO, False)
        End If

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of MO", dtMO.Rows.Count)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00063, "Delete Medical Organization")

    End Sub


    Protected Function formatPracticeNo(ByVal strGVIndex As String) As String
        Dim intGVIndex As Integer
        intGVIndex = CInt(strGVIndex)

        Dim intResIndex As Integer

        If Not IsNothing(Session(SESS_Page)) Then
            Dim intPage As Integer = Session(SESS_Page)
            intResIndex = intGVIndex + ((intPage - 1) * MaxRowNo)
        Else
            intResIndex = intGVIndex
        End If

        Return intResIndex
    End Function

    Protected Sub ibtnMOAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)
       
        udcMsgBox.Visible = False

        Dim dtMO As New DataTable
        dtMO = Me.getMOFromGridView(True)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of MO", dtMO.Rows.Count)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00007, "Add Medical Organization")

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False

            formatMOGV(dtMO, True)

            'Dim i As Integer = 0

            'Me.ddlMOList.Items.Clear()
            'For Each row As DataRow In dtPractice.Rows
            '    If Not IsNothing(row.Item("MOEname")) Then
            '        If Not CStr(row.Item("MOEname")).Equals(String.Empty) Then
            '            Dim newItem As New ListItem
            '            newItem.Text = row.Item("MOEname")
            '            newItem.Value = i
            '            ddlMOList.Items.Insert(i, newItem)
            '            i = i + 1
            '        End If
            '    End If
            'Next

            'choCopyList.ClearSelection()
            'Me.ibtnCopyConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CopyDisableBtn")

            'Me.ModalPopupExtenderCopyList.Show()
            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of MO", dtMO.Rows.Count)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Add Medical Organization Complete")

        Else
            'udcMsgBox.BuildMessageBox("ValidationFail")
            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of MO", dtMO.Rows.Count)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00009, "Add Medical Organization Fail")
        End If
    End Sub

    Protected Sub lnkBtnPersonal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        'udtAuditLogEntry.AddDescripton("HKID", lblConfirmHKID.Text)
        udcMsgBox.Visible = False

        Dim dt As DataTable
        dt = getMOFromGridView(False)


        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of MO", dt.Rows.Count)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00014, "Press Tab to Personal Particulars in MO")
      
        udtEFormBLL.ClearRedirectPageSession()
        Session(eFormBLL.SESS_PersonalParticular) = "Y"
        Response.Redirect("~/PersonalPacticulars.aspx")
    End Sub

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
        If IsNothing(Me.udtSPBLL.GetSP) Then
            Return Nothing
        Else
            Return Me.udtSPBLL.GetSP
        End If
    End Function

#End Region

    Private Sub iBtnLoadDemoData_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnLoadDemoData.Click

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Dim strPageInfo As String = Me.GetGlobalResourceObject("Text", "GridPageInfo")
        Dim ds As DataSet = Me.GetDemoData
        If ds Is Nothing Then Exit Sub

        Dim dt As DataTable = ds.Tables("MO")
        Session(SESS_MO) = dt.Copy
        Me.formatMOGV(dt, False)

        Dim doublePageCount As Double
        doublePageCount = Math.Ceiling(CType(dt.Rows.Count, Double) / 5)

        strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
        strPageInfo = strPageInfo.Replace("%e", CStr(doublePageCount))
        strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))
        ' CRE12-001 eHS and PCD integration [End][Koala]
    End Sub
End Class