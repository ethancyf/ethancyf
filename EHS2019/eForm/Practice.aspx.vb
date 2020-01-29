Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.Address
Imports Common.Component.Scheme

Partial Public Class Practice
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
    Private udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

    Private udtControlBLL As ControlBLL = New ControlBLL
    Private udtEFormBLL As eFormBLL = New eFormBLL
    Private udtSPBLL As New ServiceProvider.ServiceProviderBLL
    Private udtSchemeEFormBLL As New SchemeEFormBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then

            Dim strAbleToAccessThisPage As String = String.Empty
            strAbleToAccessThisPage = Session(eFormBLL.SESS_Practice)
            udtEFormBLL.ClearRedirectPageSession()

            If IsNothing(strAbleToAccessThisPage) OrElse Not strAbleToAccessThisPage.Trim.Equals("Y") Then
                Response.Redirect("~/main.aspx")
            Else
                Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me) ''Begin Writing Audit Log

                Dim udtSP As ServiceProvider.ServiceProviderModel
                udtSP = udtSPBLL.GetSP

                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00018, "Practice Page Loaded")

                If Not udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                    Dim udtSchemeEFromList As SchemeEFormModelCollection

                    udtSchemeEFromList = udtSchemeEFormBLL.getAllEffectiveSchemeEFormWithSubsidizeGroupFromCache
                    udtSchemeEFormBLL.SaveToSession(udtSchemeEFromList)
                End If

                Session(SESS_Page) = 1
                Me.lblPracticePaging.Visible = True

                Dim strPageInfo As String
                strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

                If Not IsNothing(Session(SESS_Practice)) Then

                    Dim dt As DataTable = Session(SESS_Practice)

                    Me.formatPracticeGV(dt, False)

                    Dim doublePageCount As Double
                    doublePageCount = Math.Ceiling(CType(dt.Rows.Count, Double) / 5)

                    strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
                    strPageInfo = strPageInfo.Replace("%e", CStr(doublePageCount))
                    strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))

                Else
                    Me.gvRegPractice.DataSource = Me.emptyPracticeBankDataTable
                    Me.gvRegPractice.DataBind()

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

                Me.lblPracticePaging.Text = strPageInfo

                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Me.iBtnLoadDemoData.Visible = Me.IsDemo
                ' CRE12-001 eHS and PCD integration [End][Koala]

                Session(SESS_PerviousPage) = String.Empty
                Session.Remove(SESS_PerviousPage)
            End If

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
        Else
            If Not IsNothing(Session(SESS_Practice)) Then

                Dim grid As GridView = gvRegPractice

                For Each row As GridViewRow In grid.Rows
                    Dim cblSchemeList As HtmlGenericControl
                    cblSchemeList = row.FindControl("cblSchemeList")
                    cblSchemeList.InnerHtml = ""

                    If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                        'Create checkbox
                        Dim chkScheme As CheckBox
                        Dim imgRegPracticeSchemeToEnroll As Image
                        Dim ltlConBr As LiteralControl
                        Dim chkAllowNonClinicSetting As CheckBox
                        Dim ltlConBrOption As LiteralControl
                        Dim lblAllowNonClinicSetting As Label
                        Dim ltlConBrStatment As LiteralControl

                        Dim udtSchemeEFormList As SchemeEFormModelCollection

                        udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                        For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                            '1. Checkbox
                            chkScheme = New CheckBox
                            chkScheme.Visible = False

                            If Session("language").Equals(English) Then
                                chkScheme.Text = udtSchemeEForm.SchemeDesc
                            Else
                                chkScheme.Text = udtSchemeEForm.SchemeDescChi
                            End If

                            chkScheme.Attributes.Add("class", "Checkbox_Label")
                            chkScheme.ID = "chk" + udtSchemeEForm.SchemeCode

                            chkScheme.AutoPostBack = True
                            AddHandler chkScheme.CheckedChanged, AddressOf chkScheme_CheckedChanged

                            cblSchemeList.Controls.Add(chkScheme)

                            '2. Alert image
                            imgRegPracticeSchemeToEnroll = New Image
                            imgRegPracticeSchemeToEnroll.Visible = False
                            imgRegPracticeSchemeToEnroll.ImageUrl = "~/Images/others/icon_caution.gif"
                            imgRegPracticeSchemeToEnroll.AlternateText = GetGlobalResourceObject("AlternateText", "ErrorImg")
                            imgRegPracticeSchemeToEnroll.Style.Add("position", "relative")
                            imgRegPracticeSchemeToEnroll.Style.Add("top", "4px")

                            imgRegPracticeSchemeToEnroll.ID = "imgError" + udtSchemeEForm.SchemeCode

                            cblSchemeList.Controls.Add(imgRegPracticeSchemeToEnroll)

                            '3. Line break
                            ltlConBr = New LiteralControl("<br/>")
                            ltlConBr.Visible = False
                            ltlConBr.ID = "ltlConBr" + udtSchemeEForm.SchemeCode

                            cblSchemeList.Controls.Add(ltlConBr)

                            '4 Non Clinic Setting
                            'If Non Clinic Setting is enabled, add one more checkbox and label
                            If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                                '4.1 checkbox
                                chkAllowNonClinicSetting = New CheckBox
                                chkAllowNonClinicSetting.Visible = False
                                chkAllowNonClinicSetting.Text = GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting")
                                chkAllowNonClinicSetting.Style.Add("margin-left", "31px")
                                chkAllowNonClinicSetting.Attributes.Add("class", "Checkbox_Label")

                                chkAllowNonClinicSetting.ID = "chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode

                                chkAllowNonClinicSetting.AutoPostBack = True
                                AddHandler chkAllowNonClinicSetting.CheckedChanged, AddressOf chkAllowNonClinicSetting_CheckedChanged

                                cblSchemeList.Controls.Add(chkAllowNonClinicSetting)

                                '4.2 Line break
                                ltlConBrOption = New LiteralControl("<br/>")
                                ltlConBrOption.Visible = False
                                ltlConBrOption.ID = "ltlConBrOption" + udtSchemeEForm.SchemeCode

                                cblSchemeList.Controls.Add(ltlConBrOption)

                                '4.1.1 Reminder
                                lblAllowNonClinicSetting = New Label
                                lblAllowNonClinicSetting.Visible = False
                                lblAllowNonClinicSetting.Text = GetGlobalResourceObject("Text", "ClinicAndNonClinicSettingToEnrollTwoPractices")
                                lblAllowNonClinicSetting.Style.Add("left", "65px")
                                lblAllowNonClinicSetting.Style.Add("position", "relative")
                                lblAllowNonClinicSetting.Style.Add("top", "-1px")
                                lblAllowNonClinicSetting.Style.Add("Background", "Yellow")
                                lblAllowNonClinicSetting.Style.Add("display", "inline-block")
                                lblAllowNonClinicSetting.Style.Add("width", "480px")

                                lblAllowNonClinicSetting.ID = "lblAllowNonClinicSetting" + udtSchemeEForm.SchemeCode

                                cblSchemeList.Controls.Add(lblAllowNonClinicSetting)

                                '4.1.1
                                ltlConBrStatment = New LiteralControl("<br/>")
                                ltlConBrStatment.Visible = False
                                ltlConBrStatment.ID = "ltlConBrStatment" + udtSchemeEForm.SchemeCode

                                cblSchemeList.Controls.Add(ltlConBrStatment)
                            End If
                        Next
                    End If
                Next
            End If
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If

        txtCopyImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "CopyBtn")
        txtCopyDisableImageUrl.Text = Me.GetGlobalResourceObject("ImageUrl", "CopyDisableBtn")
    End Sub

    Private Function getPracticeFromGridView(ByVal blnChecking As Boolean) As DataTable

        Dim i(7) As Integer
        Dim s(7) As String

        Dim smPractice(7) As Common.ComObject.SystemMessage

        Dim grid As GridView = gvRegPractice

        Dim intstartIndex As Integer = 0

        Dim dt As New DataTable
        If Session(SESS_Practice) Is Nothing Then
            dt = Me.emptyPracticeBankDataTable
        Else
            dt = Session(SESS_Practice)
        End If

        For Each row As GridViewRow In grid.Rows
            'Practice Information
            Dim intPracticeIndex As Integer = CInt(CType(row.FindControl("lblRegPracticeIndex"), Label).Text.Trim) - 1

            Dim strMO As String = CType(row.FindControl("ddlMO"), DropDownList).SelectedIndex '+ 1
            Dim strRegPracticeName As String = CType(row.FindControl("txtRegPracticeName"), TextBox).Text.Trim
            Dim strRegPracticeCName As String = CType(row.FindControl("txtRegPracticeCName"), TextBox).Text.Trim
            Dim strRegPracticeRoom As String = CType(row.FindControl("txtRegPracticeRoom"), TextBox).Text.Trim
            Dim strRegPracticeFloor As String = CType(row.FindControl("txtRegPracticeFloor"), TextBox).Text.Trim
            Dim strRegPracticeBlock As String = CType(row.FindControl("txtRegPracticeBlock"), TextBox).Text.Trim
            Dim strRegPracticeEAddress As String = CType(row.FindControl("txtRegPracticeEAddress"), TextBox).Text.Trim
            Dim strRegPracticeCAddress As String = CType(row.FindControl("txtRegPracticeCAddress"), TextBox).Text.Trim
            Dim strRegPracticeDistrict As String = CType(row.FindControl("ddlRegPracticeDistrict"), DropDownList).SelectedValue.Trim
            Dim strRegPracticeTel As String = CType(row.FindControl("txtRegPracticeTel"), TextBox).Text.Trim
            Dim strRegPracticeHealthProf As String = CType(row.FindControl("ddlRegPracticeHealthProf"), DropDownList).SelectedValue.Trim
            Dim strRegPracticeRegCode As String = UCase(CType(row.FindControl("txtRegPracticeRegCode"), TextBox).Text.Trim)

            Dim strRegPracticeArea As String = udtEFormBLL.getAreaString(strRegPracticeDistrict)

            Dim imgRegPracticeMONameAlert As Image = row.FindControl("imgRegPracticeMONameAlert")
            Dim imgRegPracticeNameAlert As Image = row.FindControl("imgRegPracticeNameAlert")
            Dim imgRegPracticeCNameAlert As Image = row.FindControl("imgRegPracticeCNameAlert")
            Dim imgRegPracticeEAddressAlert As Image = row.FindControl("imgRegPracticeEAddressAlert")
            Dim imgRegPracticeDistrictAlert As Image = row.FindControl("imgRegPracticeDistrictAlert")
            Dim imgRegPracticeTelAlert As Image = row.FindControl("imgRegPracticeTelAlert")
            Dim imgRegPracticeHealthProfAlert As Image = row.FindControl("imgRegPracticeHealthProfAlert")
            Dim imgRegPracticeRegCodeAlert As Image = row.FindControl("imgRegPracticeRegCodeAlert")

            imgRegPracticeMONameAlert.Visible = False
            imgRegPracticeNameAlert.Visible = False
            imgRegPracticeCNameAlert.Visible = False
            imgRegPracticeEAddressAlert.Visible = False
            imgRegPracticeDistrictAlert.Visible = False
            imgRegPracticeTelAlert.Visible = False
            imgRegPracticeHealthProfAlert.Visible = False
            imgRegPracticeRegCodeAlert.Visible = False

            dt.Rows(intPracticeIndex)("MOIndex") = strMO
            dt.Rows(intPracticeIndex)("PracticeName") = strRegPracticeName
            dt.Rows(intPracticeIndex)("PracticeNameChi") = strRegPracticeCName
            dt.Rows(intPracticeIndex)("Room") = strRegPracticeRoom
            dt.Rows(intPracticeIndex)("Floor") = strRegPracticeFloor
            dt.Rows(intPracticeIndex)("Block") = strRegPracticeBlock
            dt.Rows(intPracticeIndex)("Building") = strRegPracticeEAddress
            dt.Rows(intPracticeIndex)("ChiBuilding") = strRegPracticeCAddress
            dt.Rows(intPracticeIndex)("District") = strRegPracticeDistrict
            dt.Rows(intPracticeIndex)("PhoneDaytime") = strRegPracticeTel

            'If strMO.Equals("0") Then
            '    strMO = String.Empty
            'End If

            If intPracticeIndex = 0 Then
                dt.Rows(intPracticeIndex)("ServiceCategoryCode") = strRegPracticeHealthProf
                dt.Rows(intPracticeIndex)("RegistrationCode") = strRegPracticeRegCode
            Else
                dt.Rows(intPracticeIndex)("ServiceCategoryCode") = dt.Rows(0)("ServiceCategoryCode")
                dt.Rows(intPracticeIndex)("RegistrationCode") = dt.Rows(0)("RegistrationCode")
            End If

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                Dim udtSchemeEFormList As SchemeEFormModelCollection

                udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                    Dim chkScheme As CheckBox = row.FindControl("chk" + udtSchemeEForm.SchemeCode)
                    Dim blnActiveScheme As Boolean = False
                    Dim blnActiveNonClinicSetting As Boolean = False

                    If Not chkScheme Is Nothing Then
                        If chkScheme.Visible Then
                            dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes
                        Else
                            dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.No
                        End If

                        If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                            If chkScheme.Checked Then
                                If dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.No Then
                                    blnActiveScheme = True
                                End If
                            Else
                                If dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then
                                    blnActiveScheme = True
                                End If
                            End If

                            Dim chkAllowNonClinicSetting As CheckBox = row.FindControl("chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)

                            If Not chkAllowNonClinicSetting Is Nothing Then
                                If chkAllowNonClinicSetting.Checked Then
                                    If dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.No Then
                                        blnActiveNonClinicSetting = True
                                    End If
                                Else
                                    If dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                        blnActiveNonClinicSetting = True
                                    End If
                                End If
                            End If
                        End If

                        If chkScheme.Checked Then
                            dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes
                        Else
                            dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.No

                            If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                                dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = String.Empty
                            End If

                            For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                If Not IsNothing(udtSubsidizeGroupEForm) Then
                                    dt.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided") = String.Empty
                                    dt.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_ServiceFee") = String.Empty
                                    dt.Rows(intPracticeIndex)(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsNoServiceFee") = String.Empty
                                End If
                            Next
                        End If

                        If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                            Dim chkAllowNonClinicSetting As CheckBox = row.FindControl("chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)

                            If Not chkAllowNonClinicSetting Is Nothing Then
                                If chkAllowNonClinicSetting.Checked Then
                                    dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes
                                Else
                                    dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.No
                                End If
                            End If

                            If dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.No And _
                                dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes Then

                                If blnActiveScheme Then
                                    dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.No
                                    dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.No
                                End If

                                If blnActiveNonClinicSetting Then
                                    dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes
                                    dt.Rows(intPracticeIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes
                                End If
                            End If
                        End If

                    End If

                    Dim imgRegPracticeSchemeToEnroll As Image = row.FindControl("imgError" + udtSchemeEForm.SchemeCode)

                    If Not imgRegPracticeSchemeToEnroll Is Nothing Then
                        imgRegPracticeSchemeToEnroll.Visible = False
                    End If
                Next

            End If

            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            dt.Rows(intPracticeIndex)("Page") = Math.Ceiling(CType(intPracticeIndex + 1, Double) / 5)

            'Valdiation
            If blnChecking Then

                'check mo selection
                udtSM = udtValidator.chkPracticeMOName(CType(row.FindControl("ddlMO"), DropDownList).Items(strMO).Value)
                If Not IsNothing(udtSM) Then
                    imgRegPracticeMONameAlert.Visible = True
                    i(0) = i(0) + 1
                    s(0) = s(0) + ", " + (intPracticeIndex + 1).ToString
                    smPractice(0) = udtSM
                End If

                'Check the name of practice
                udtSM = udtValidator.chkPracticeName(strRegPracticeName)
                If Not udtSM Is Nothing Then
                    imgRegPracticeNameAlert.Visible = True
                    i(1) = i(1) + 1
                    s(1) = s(1) + ", " + (intPracticeIndex + 1).ToString
                    smPractice(1) = udtSM
                End If


                'Check the address of practice
                udtSM = udtValidator.chkPracticeAddress(strRegPracticeEAddress, strRegPracticeDistrict, strRegPracticeArea)
                If Not udtSM Is Nothing Then
                    If udtValidator.IsEmpty(strRegPracticeEAddress) Then
                        imgRegPracticeEAddressAlert.Visible = True
                    End If

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                    'If udtValidator.IsEmpty(strRegPracticeDistrict) OrElse strRegPracticeDistrict.Equals(".H") OrElse strRegPracticeDistrict.Equals(".K") OrElse _
                    '    strRegPracticeDistrict.Equals(".N") Then
                    If udtValidator.IsEmpty(strRegPracticeDistrict) OrElse strRegPracticeDistrict.StartsWith(".") Then
                        'CRE13-019-02 Extend HCVS to China [End][Winnie]
                        imgRegPracticeDistrictAlert.Visible = True
                    End If

                    If udtSM.MessageCode = "00024" Then
                        i(2) = i(2) + 1
                        s(2) = s(2) + ", " + (intPracticeIndex + 1).ToString
                        smPractice(2) = udtSM
                    Else
                        i(3) = i(3) + 1
                        s(3) = s(3) + "," + (intPracticeIndex + 1).ToString
                        smPractice(3) = udtSM
                    End If

                End If

                'Check the type of Health Profession
                udtSM = udtValidator.chkHealthProf(strRegPracticeHealthProf)
                If Not udtSM Is Nothing Then
                    imgRegPracticeHealthProfAlert.Visible = True
                    i(5) = i(5) + 1
                    s(5) = s(5) + ", " + (intPracticeIndex + 1).ToString
                    smPractice(5) = udtSM
                End If

                'Check the profession registration no.
                udtSM = udtValidator.chkRegCode(strRegPracticeRegCode)
                If Not udtSM Is Nothing Then
                    imgRegPracticeRegCodeAlert.Visible = True
                    i(6) = i(6) + 1
                    s(6) = s(6) + ", " + (intPracticeIndex + 1).ToString
                    smPractice(6) = udtSM
                End If

                'Check the Phone daytime
                udtSM = udtValidator.chkPracticeTel(strRegPracticeTel)
                If Not udtSM Is Nothing Then
                    imgRegPracticeTelAlert.Visible = True
                    i(4) = i(4) + 1
                    s(4) = s(4) + ", " + (intPracticeIndex + 1).ToString
                    smPractice(4) = udtSM
                End If

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Check the at least one of scheme should be selected
                Dim lblSchemeSelected As Boolean = False
                If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                    Dim udtSchemeEFormList As SchemeEFormModelCollection

                    udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        Dim chkScheme As CheckBox = row.FindControl("chk" + udtSchemeEForm.SchemeCode)

                        If Not chkScheme Is Nothing Then
                            If chkScheme.Visible And chkScheme.Checked Then
                                lblSchemeSelected = True
                            End If
                        End If
                    Next

                    If Not strRegPracticeHealthProf = String.Empty AndAlso Not lblSchemeSelected Then
                        For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                            If udtSchemeEForm.EligibleProfesional(strRegPracticeHealthProf) Then
                                Dim imgRegPracticeSchemeToEnroll As Image = row.FindControl("imgError" + udtSchemeEForm.SchemeCode)
                                imgRegPracticeSchemeToEnroll.Visible = True
                            End If
                        Next

                        If smPractice(7) Is Nothing Then
                            udtSM = New SystemMessage(GlobalFunctionCode, SeverityCode.SEVE, MsgCode.MSG00384)
                            smPractice(7) = udtSM
                        End If
                        i(7) = i(7) + 1
                        s(7) = s(7) + ", " + (intPracticeIndex + 1).ToString
                    End If
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]

            End If

        Next

        If blnChecking Then
            If i(0) > 0 Then
                udcMsgBox.AddMessage(smPractice(0), "%s", s(0).Substring(1))
            End If
            If i(1) > 0 Then
                Dim str() As String = {s(1)}
                udcMsgBox.AddMessage(smPractice(1), "%s", s(1).Substring(1))
            End If
            If i(2) > 0 Then
                Dim str() As String = {s(2)}
                udcMsgBox.AddMessage(smPractice(2), "%s", s(2).Substring(1))
            End If
            If i(3) > 0 Then
                Dim str() As String = {s(3)}
                udcMsgBox.AddMessage(smPractice(3), "%s", s(3).Substring(1))
            End If
            If i(4) > 0 Then
                Dim str() As String = {s(4)}
                udcMsgBox.AddMessage(smPractice(4), "%s", s(4).Substring(1))
            End If
            If i(5) > 0 Then
                Dim str() As String = {s(5)}
                udcMsgBox.AddMessage(smPractice(5), "%s", s(5).Substring(1))
            End If
            If i(6) > 0 Then
                Dim str() As String = {s(6)}
                udcMsgBox.AddMessage(smPractice(6), "%s", s(6).Substring(1))
            End If
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If i(7) > 0 Then
                Dim str() As String = {s(7)}
                udcMsgBox.AddMessage(smPractice(7), "%s", s(7).Substring(1))
            End If
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If

        Session(SESS_Practice) = dt

        Return dt

    End Function

    Protected Sub ibtnRegPracticeAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        udcMsgBox.Visible = False

        Dim dtPractice As DataTable = New DataTable
        dtPractice = getPracticeFromGridView(True)

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strRegPracticeHealthProf As String = String.Empty
        If dtPractice.Rows.Count > 0 Then
            strRegPracticeHealthProf = CStr(dtPractice.Rows(0).Item("ServiceCategoryCode")).Trim
        End If

        For Each row As GridViewRow In gvRegPractice.Rows
            If strRegPracticeHealthProf = String.Empty Then
                Dim cblSchemeList As HtmlGenericControl
                cblSchemeList = row.FindControl("cblSchemeList")
                cblSchemeList.InnerHtml = ""

                'When not yet select the Health Profession, the remark should be shown.
                Dim lblPleaseSelectScheme As New Label
                lblPleaseSelectScheme.Visible = True
                lblPleaseSelectScheme.Text = GetGlobalResourceObject("Text", "SelectHealthProf")
                'lblPleaseSelectScheme.Style.Item("color") = "#CCCCCC"
                'lblPleaseSelectScheme.Style.Item("font-style") = "italic"

                cblSchemeList.Controls.Add(lblPleaseSelectScheme)
            Else
                If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                    Dim udtSchemeEFormList As SchemeEFormModelCollection

                    udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        Dim chkScheme As CheckBox = row.FindControl("chk" + udtSchemeEForm.SchemeCode.Trim)
                        Dim ltlConBr As LiteralControl = row.FindControl("ltlConBr" + udtSchemeEForm.SchemeCode.Trim)
                        Dim chkAllowNonClinicSetting As CheckBox = row.FindControl("chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                        Dim ltlConBrOption As LiteralControl = row.FindControl("ltlConBrOption" + udtSchemeEForm.SchemeCode)
                        Dim lblAllowNonClinicSetting As Label = row.FindControl("lblAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                        Dim ltlConBrStatment As LiteralControl = row.FindControl("ltlConBrStatment" + udtSchemeEForm.SchemeCode)

                        If Not chkScheme Is Nothing And Not ltlConBr Is Nothing Then
                            If CStr(dtPractice.Rows(row.RowIndex).Item(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession")).Trim = YesNo.Yes Then
                                chkScheme.Visible = True
                                ltlConBr.Visible = True

                                If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                                    If Not chkAllowNonClinicSetting Is Nothing And Not ltlConBrOption Is Nothing Then
                                        chkAllowNonClinicSetting.Visible = False
                                        ltlConBrOption.Visible = False

                                        'Tick Non-clinic checkbox from settings
                                        If dtPractice.Rows(row.RowIndex)(Mid(chkScheme.ID, Len("chk") + 1) + "_Selected") = YesNo.Yes Then
                                            chkAllowNonClinicSetting.Visible = True
                                            ltlConBrOption.Visible = True
                                        End If

                                        If Not lblAllowNonClinicSetting Is Nothing And Not ltlConBrStatment Is Nothing Then
                                            If dtPractice.Rows(row.RowIndex)(Mid(chkScheme.ID, Len("chk") + 1) + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                                chkScheme.Checked = True
                                                chkAllowNonClinicSetting.Checked = True
                                                lblAllowNonClinicSetting.Visible = True
                                                ltlConBrStatment.Visible = True
                                            End If
                                        End If

                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        Next
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count + 1)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00019, "Add Practice")

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            Dim dt As DataTable = New DataTable

            dt = addEmptyDataRowToDataTable(dtPractice)
            Dim intNewRow As Integer = dt.Rows.Count - 1

            dt.Rows(intNewRow).Item("ServiceCategoryCode") = dt.Rows(0).Item("ServiceCategoryCode")
            dt.Rows(intNewRow).Item("RegistrationCode") = dt.Rows(0).Item("RegistrationCode")

            dt.Rows(intNewRow).Item("Page") = Math.Ceiling(CType(intNewRow + 1, Double) / 5)

            Me.formatPracticeGV(dt, False)

            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count + 1)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00020, "Add Practice Complete")

        Else
            'udcMsgBox.BuildMessageBox("ValidationFail")
            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count + 1)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00021, "Add Practice Fail")
        End If

        'Dim dt As DataTable = New DataTable

        'dt = addEmptyDataRowToDataTable(dtPractice)
        'Dim intNewRow As Integer = dt.Rows.Count - 1

        'dt.Rows(intNewRow).Item("ServiceCategoryCode") = dt.Rows(0).Item("ServiceCategoryCode")
        'dt.Rows(intNewRow).Item("RegistrationCode") = dt.Rows(0).Item("RegistrationCode")

        'dt.Rows(intNewRow).Item("Page") = Math.Ceiling(CType(intNewRow + 1, Double) / 5)

        'Me.formatPracticeGV(dt, False)

        'Dim dtPractice As New DataTable
        'dtPractice = getPracticeFromGridView(True)

        'If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
        '    udcMsgBox.Visible = False

        '    Dim i As Integer = 0

        '    Me.ddlPracticeList.Items.Clear()
        '    For Each row As DataRow In dtPractice.Rows
        '        If Not IsNothing(row.Item("PracticeName")) Then
        '            If Not CStr(row.Item("PracticeName")).Equals(String.Empty) Then
        '                Dim newItem As New ListItem
        '                newItem.Text = row.Item("PracticeName")
        '                newItem.Value = i
        '                ddlPracticeList.Items.Insert(i, newItem)
        '                i = i + 1
        '            End If
        '        End If
        '    Next

        '    If dtPractice.Rows.Count = 1 Then
        '        Me.ddlPracticeList.Enabled = False
        '    Else
        '        Me.ddlPracticeList.Enabled = True
        '    End If

        '    choCopyList.ClearSelection()
        '    choCopyList.Items(0).Selected = True
        '    'choCopyList.Items(4).Selected = True
        '    'choCopyList.Items(5).Selected = True

        '    Dim dtMO As New DataTable
        '    dtMO = Session(SESS_MO)

        '    If Not IsNothing(dtMO) Then
        '        If dtMO.Rows.Count = 1 Then
        '            choCopyList.Items(0).Attributes.Add("onclick", "return false;")

        '            For Each li As ListItem In choCopyList.Items
        '                If li.Value <> 0 Then
        '                    li.Attributes.Add("onclick", "chkCopyChanged();")
        '                End If
        '            Next
        '        Else
        '            choCopyList.Attributes.Add("onclick", "chkCopyChanged();")
        '        End If
        '    End If



        '    Me.ModalPopupExtenderCopyList.Show()

        'Else
        '    udcMsgBox.BuildMessageBox("ValidationFail")
        'End If
    End Sub

    Private Function emptyPracticeBankDataTable() As DataTable
        Dim dt As New DataTable

        dt.Columns.Add(New DataColumn("MOIndex"))
        dt.Columns.Add(New DataColumn("PracticeName"))
        dt.Columns.Add(New DataColumn("PracticeNameChi"))
        dt.Columns.Add(New DataColumn("Room"))
        dt.Columns.Add(New DataColumn("Floor"))
        dt.Columns.Add(New DataColumn("Block"))
        dt.Columns.Add(New DataColumn("Building"))
        dt.Columns.Add(New DataColumn("ChiBuilding"))
        dt.Columns.Add(New DataColumn("District"))
        dt.Columns.Add(New DataColumn("PhoneDaytime"))
        dt.Columns.Add(New DataColumn("ServiceCategoryCode"))
        dt.Columns.Add(New DataColumn("RegistrationCode"))
        dt.Columns.Add(New DataColumn("Bank"))
        dt.Columns.Add(New DataColumn("Branch"))
        dt.Columns.Add(New DataColumn("BankCode"))
        dt.Columns.Add(New DataColumn("BranchCode"))
        dt.Columns.Add(New DataColumn("BankAcc"))
        dt.Columns.Add(New DataColumn("Holder"))
        dt.Columns.Add(New DataColumn("Page"))
        dt.Columns.Add(New DataColumn("PracticeIndex"))

        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
            Dim udtSchemeEFormList As SchemeEFormModelCollection

            udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

            If Not IsNothing(udtSchemeEFormList) Then
                For Each udtSchemeEFrom As SchemeEFormModel In udtSchemeEFormList
                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    dt.Columns.Add(New DataColumn(udtSchemeEFrom.SchemeCode.Trim + "_EligibleForProfession"))
                    dt.Columns.Add(New DataColumn(udtSchemeEFrom.SchemeCode.Trim + "_Selected"))

                    If udtSchemeEFrom.AllowNonClinicSetting = YesNo.Yes Then
                        dt.Columns.Add(New DataColumn(udtSchemeEFrom.SchemeCode.Trim + "_NonClinicSetting_Selected"))
                    End If

                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                    If Not IsNothing(udtSchemeEFrom.SubsidizeGroupEFormList) Then
                        For Each udtSubsidizeGroupeform As SubsidizeGroupEFormModel In udtSchemeEFrom.SubsidizeGroupEFormList
                            If Not IsNothing(udtSubsidizeGroupeform) Then
                                'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                'dt.Columns.Add(New DataColumn(udtSubsidizeGroupeform.SubsidizeCode.Trim))
                                dt.Columns.Add(New DataColumn(udtSubsidizeGroupeform.SubsidizeCode.Trim + "_IsProvided"))
                                dt.Columns.Add(New DataColumn(udtSubsidizeGroupeform.SubsidizeCode.Trim + "_ServiceFee"))
                                dt.Columns.Add(New DataColumn(udtSubsidizeGroupeform.SubsidizeCode.Trim + "_IsNoServiceFee"))
                                'CRE15-004 (TIV and QIV) [End][Chris YIM]

                            End If
                        Next

                    End If
                Next

            End If

        End If


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

    Private Sub gvRegPractice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRegPractice.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Practice District
            Dim ddlRegPracticeDistrict As DropDownList = e.Row.FindControl("ddlRegPracticeDistrict")
            Dim hfDistrict As HiddenField = e.Row.FindControl("hfRegPracticeDistrict")

            Dim txtRegPracticeRegCode As TextBox = e.Row.FindControl("txtRegPracticeRegCode")

            udtControlBLL.bindDistrict(ddlRegPracticeDistrict, String.Empty, False)
            ddlRegPracticeDistrict.SelectedValue = hfDistrict.Value

            'Health Profession
            Dim ddlRegPracticeHealthProf As DropDownList = e.Row.FindControl("ddlRegPracticeHealthProf")
            Dim hfRegPracticeHealthProf As HiddenField = e.Row.FindControl("hfRegPracticeHealthProf")

            ddlRegPracticeHealthProf.DataSource = udtEFormBLL.GetHealthProf

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ddlRegPracticeHealthProf.DataValueField = "ServiceCategoryCode"
            If Session("language") = "zh-tw" Then
                ddlRegPracticeHealthProf.DataTextField = "ServiceCategoryDescChi"
            Else
                ddlRegPracticeHealthProf.DataTextField = "ServiceCategoryDesc"
            End If

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ddlRegPracticeHealthProf.DataBind()

            ddlRegPracticeHealthProf.SelectedValue = hfRegPracticeHealthProf.Value

            Dim lblRegPracticeIndex As Label = CType(e.Row.FindControl("lblRegPracticeIndex"), Label)

            If CInt(lblRegPracticeIndex.Text.Trim) - 1 > 0 Then
                ddlRegPracticeHealthProf.Enabled = False
                txtRegPracticeRegCode.Enabled = False
            Else
                ddlRegPracticeHealthProf.Enabled = True
                txtRegPracticeRegCode.Enabled = True
            End If

            'MO
            Dim ddlMO As DropDownList = e.Row.FindControl("ddlMO")
            Dim hfMO As HiddenField = e.Row.FindControl("hfMO")
            If Not IsNothing(Session(SESS_MO)) Then
                Dim dt As DataTable
                dt = Session(SESS_MO)
                ddlMO.DataSource = dt
                ddlMO.DataTextField = "MOEname"
                ddlMO.DataValueField = "MOEname"
                ddlMO.DataBind()

                If Not hfMO.Value.Trim.Equals(String.Empty) Then
                    If CInt(hfMO.Value.Trim) <> 0 Then
                        ddlMO.SelectedIndex = CInt(hfMO.Value.Trim) ' - 1
                    End If
                End If

                If dt.Rows.Count = 1 Then
                    ddlMO.SelectedIndex = 1
                    ddlMO.Enabled = False
                Else
                    ddlMO.Enabled = True
                End If
            End If

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Scheme to enroll
            Dim cblSchemeList As HtmlGenericControl
            cblSchemeList = e.Row.FindControl("cblSchemeList")
            cblSchemeList.InnerHtml = ""

            If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                'Create checkbox
                Dim chkScheme As CheckBox
                Dim imgRegPracticeSchemeToEnroll As Image
                Dim ltlConBr As LiteralControl
                Dim chkAllowNonClinicSetting As CheckBox
                Dim ltlConBrOption As LiteralControl
                Dim lblAllowNonClinicSetting As Label
                Dim ltlConBrStatment As LiteralControl

                Dim udtSchemeEFormList As SchemeEFormModelCollection

                udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                    '1. Checkbox 
                    chkScheme = New CheckBox
                    chkScheme.Visible = False

                    If Session("language") = "zh-tw" Then
                        chkScheme.Text = udtSchemeEForm.SchemeDescChi
                    Else
                        chkScheme.Text = udtSchemeEForm.SchemeDesc
                    End If
                    chkScheme.Attributes.Add("class", "Checkbox_Label")

                    chkScheme.ID = "chk" + udtSchemeEForm.SchemeCode

                    chkScheme.AutoPostBack = True
                    AddHandler chkScheme.CheckedChanged, AddressOf chkScheme_CheckedChanged

                    cblSchemeList.Controls.Add(chkScheme)

                    '2. Alert image
                    imgRegPracticeSchemeToEnroll = New Image
                    imgRegPracticeSchemeToEnroll.Visible = False
                    imgRegPracticeSchemeToEnroll.ImageUrl = "~/Images/others/icon_caution.gif"
                    imgRegPracticeSchemeToEnroll.AlternateText = GetGlobalResourceObject("AlternateText", "ErrorImg")
                    imgRegPracticeSchemeToEnroll.Style.Add("position", "relative")
                    imgRegPracticeSchemeToEnroll.Style.Add("top", "4px")
                    imgRegPracticeSchemeToEnroll.ID = "imgError" + udtSchemeEForm.SchemeCode

                    cblSchemeList.Controls.Add(imgRegPracticeSchemeToEnroll)

                    '3. Line Break
                    ltlConBr = New LiteralControl("<br/>")
                    ltlConBr.Visible = False
                    ltlConBr.ID = "ltlConBr" + udtSchemeEForm.SchemeCode

                    cblSchemeList.Controls.Add(ltlConBr)

                    '4 Non Clinic Setting
                    'If Non Clinic Setting is enabled, add one more checkbox and label
                    If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                        '4.1 checkbox
                        chkAllowNonClinicSetting = New CheckBox
                        chkAllowNonClinicSetting.Visible = False
                        chkAllowNonClinicSetting.Text = GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting")
                        chkAllowNonClinicSetting.Style.Add("margin-left", "31px")
                        chkAllowNonClinicSetting.Attributes.Add("class", "Checkbox_Label")

                        chkAllowNonClinicSetting.ID = "chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode

                        chkAllowNonClinicSetting.AutoPostBack = True
                        AddHandler chkAllowNonClinicSetting.CheckedChanged, AddressOf chkAllowNonClinicSetting_CheckedChanged

                        cblSchemeList.Controls.Add(chkAllowNonClinicSetting)

                        '4.2 Line break
                        ltlConBrOption = New LiteralControl("<br/>")
                        ltlConBrOption.Visible = False
                        ltlConBrOption.ID = "ltlConBrOption" + udtSchemeEForm.SchemeCode

                        cblSchemeList.Controls.Add(ltlConBrOption)

                        '4.1.1 Reminder
                        lblAllowNonClinicSetting = New Label
                        lblAllowNonClinicSetting.Visible = False
                        lblAllowNonClinicSetting.Text = GetGlobalResourceObject("Text", "ClinicAndNonClinicSettingToEnrollTwoPractices")
                        lblAllowNonClinicSetting.Style.Add("left", "65px")
                        lblAllowNonClinicSetting.Style.Add("position", "relative")
                        lblAllowNonClinicSetting.Style.Add("top", "-1px")
                        lblAllowNonClinicSetting.Style.Add("Background", "Yellow")
                        lblAllowNonClinicSetting.Style.Add("display", "inline-block")
                        lblAllowNonClinicSetting.Style.Add("width", "480px")

                        lblAllowNonClinicSetting.ID = "lblAllowNonClinicSetting" + udtSchemeEForm.SchemeCode

                        cblSchemeList.Controls.Add(lblAllowNonClinicSetting)

                        '4.1.1
                        ltlConBrStatment = New LiteralControl("<br/>")
                        ltlConBrStatment.Visible = False
                        ltlConBrStatment.ID = "ltlConBrStatment" + udtSchemeEForm.SchemeCode

                        cblSchemeList.Controls.Add(ltlConBrStatment)
                    End If
                Next

                'By setting, hide or show checkbox
                If Not hfRegPracticeHealthProf.Value = String.Empty Then
                    Dim ctScheme As Integer = 0
                    Dim chkLastCreated As CheckBox = Nothing
                    Dim lblExistPracticeSession As Boolean = False

                    Dim dtPractice As DataTable = Nothing
                    If Not IsNothing(Session(SESS_Practice)) Then
                        dtPractice = Session(SESS_Practice)
                        lblExistPracticeSession = True
                    End If

                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        chkScheme = e.Row.FindControl("chk" + udtSchemeEForm.SchemeCode)
                        ltlConBr = e.Row.FindControl("ltlConBr" + udtSchemeEForm.SchemeCode)
                        chkAllowNonClinicSetting = e.Row.FindControl("chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                        ltlConBrOption = e.Row.FindControl("ltlConBrOption" + udtSchemeEForm.SchemeCode)
                        lblAllowNonClinicSetting = e.Row.FindControl("lblAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                        ltlConBrStatment = e.Row.FindControl("ltlConBrStatment" + udtSchemeEForm.SchemeCode)

                        If udtSchemeEForm.EligibleProfesional(hfRegPracticeHealthProf.Value) Then
                            'Show checkbox
                            chkScheme.Visible = True
                            ltlConBr.Visible = True

                            chkLastCreated = chkScheme
                            ctScheme = ctScheme + 1

                            'Tick Scheme checkbox from settings
                            If lblExistPracticeSession Then
                                If dtPractice.Rows(e.Row.RowIndex)(Mid(chkScheme.ID, Len("chk") + 1) + "_Selected") = YesNo.Yes Then
                                    chkScheme.Checked = True
                                End If
                            End If

                            If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then

                                If Not chkAllowNonClinicSetting Is Nothing And Not ltlConBrOption Is Nothing Then
                                    chkAllowNonClinicSetting.Visible = False
                                    ltlConBrOption.Visible = False

                                    'Tick Non-clinic checkbox from settings
                                    If lblExistPracticeSession Then
                                        If dtPractice.Rows(e.Row.RowIndex)(Mid(chkScheme.ID, Len("chk") + 1) + "_Selected") = YesNo.Yes Then
                                            chkAllowNonClinicSetting.Visible = True
                                            'chkAllowNonClinicSetting.Checked = False
                                            ltlConBrOption.Visible = True
                                        End If

                                        If Not lblAllowNonClinicSetting Is Nothing And Not ltlConBrStatment Is Nothing Then
                                            If dtPractice.Rows(e.Row.RowIndex)(Mid(chkScheme.ID, Len("chk") + 1) + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                                chkScheme.Checked = True
                                                chkAllowNonClinicSetting.Checked = True
                                                lblAllowNonClinicSetting.Visible = True
                                                ltlConBrStatment.Visible = True
                                            End If
                                        End If

                                    End If
                                End If
                            End If

                        Else
                            'Hide checkbox
                            chkScheme.Visible = False
                            ltlConBr.Visible = False
                            If Not chkAllowNonClinicSetting Is Nothing Then
                                chkAllowNonClinicSetting.Visible = False
                                ltlConBrOption.Visible = False
                            End If
                        End If
                    Next

                    'When only one scheme is shown, the checkbox always has tick.
                    If ctScheme = 1 AndAlso Not chkLastCreated Is Nothing Then
                        chkLastCreated.Checked = True
                        chkLastCreated.Attributes.Add("onclick", "return false;")
                    End If
                Else
                    'When not yet select the Health Profession, the remark should be shown.
                    Dim lblPleaseSelectScheme As New Label
                    lblPleaseSelectScheme.Text = GetGlobalResourceObject("Text", "SelectHealthProf")
                    'lblPleaseSelectScheme.Style.Item("color") = "#CCCCCC"
                    'lblPleaseSelectScheme.Style.Item("font-style") = "italic"

                    cblSchemeList.Controls.Add(lblPleaseSelectScheme)
                End If

            End If

            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End If
    End Sub

    Private Sub gvRegPractice_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvRegPractice.RowDeleting
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        udcMsgBox.Visible = False
        Dim dtPractice As New DataTable
        dtPractice = Me.getPracticeFromGridView(False)

        Dim gvRow As GridViewRow = gvRegPractice.Rows(e.RowIndex)

        Dim intIndex As Integer = CInt(CType(gvRow.FindControl("lblRegPracticeIndex"), Label).Text.Trim) - 1

        dtPractice.Rows(intIndex).Delete()

        Dim i As Integer = 0
        For Each dr As DataRow In dtPractice.Rows
            dr.Item("Page") = Math.Ceiling(CType(i + 1, Double) / 5)
            i = i + 1
        Next

        If dtPractice.Rows.Count = 0 Then
            Me.gvRegPractice.DataSource = Me.emptyPracticeBankDataTable()
            gvRegPractice.DataBind()

            Session(SESS_Practice) = Nothing

        Else
            ' dtPractice = Me.getPracticeFromGridView(False)
            Session(SESS_Practice) = dtPractice

            Me.gvRegPractice.DataSource = dtPractice
            gvRegPractice.DataBind()

            Me.formatPracticeGV(dtPractice, False)
        End If

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        'For Each udtThirdPartyEnrolmentModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel In udtSP.ThirdPartyAdditionalFieldEnrolmentList.GetListBySysCode(ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD)
        '    If udtThirdPartyEnrolmentModel.PracticeDisplaySeq = intIndex + 1 Then
        '        udtSP.ThirdPartyAdditionalFieldEnrolmentList.Remove(udtThirdPartyEnrolmentModel)
        '        Exit For
        '    End If
        'Next
        'CRE15-004 TIV & QIV [Start][Philip]
        If Not IsNothing(udtSP.ThirdPartyAdditionalFieldEnrolmentList) AndAlso udtSP.ThirdPartyAdditionalFieldEnrolmentList.Count > 0 Then
            udtSP.ThirdPartyAdditionalFieldEnrolmentList.Remove(udtSP.ThirdPartyAdditionalFieldEnrolmentList.GetByValueCode(ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, _
                                                                                                                            intIndex + 1, _
                                                                                                                            Common.PCD.EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString()))
        End If
        'CRE15-004 TIV & QIV [End][Philip]

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count)

        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00064, "Delete Practice")

    End Sub

    Protected Sub ibtnRegPracticeBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim dt As DataTable
        dt = Me.getPracticeFromGridView(False)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00067, "Press Back to MO")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_MedicalOrganization) = "Y"
        Response.Redirect("~/MedicalOrganization.aspx")
    End Sub

    Protected Sub ibtnRegPracticeNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        'udtAuditLogEntry.AddDescripton("HKID", lblConfirmHKID.Text)
        udcMsgBox.Visible = False

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim dt As DataTable
        'dt = getPracticeFromGridView(True)

        Dim dtPractice As DataTable
        dtPractice = getPracticeFromGridView(True)

        Dim strRegPracticeHealthProf As String = String.Empty
        If dtPractice.Rows.Count > 0 Then
            strRegPracticeHealthProf = CStr(dtPractice.Rows(0).Item("ServiceCategoryCode")).Trim
        End If

        For Each row As GridViewRow In gvRegPractice.Rows
            If strRegPracticeHealthProf = String.Empty Then
                Dim cblSchemeList As HtmlGenericControl
                cblSchemeList = row.FindControl("cblSchemeList")
                cblSchemeList.InnerHtml = ""

                'When not yet select the Health Profession, the remark should be shown.
                Dim lblPleaseSelectScheme As New Label
                lblPleaseSelectScheme.Visible = True
                lblPleaseSelectScheme.Text = GetGlobalResourceObject("Text", "SelectHealthProf")
                'lblPleaseSelectScheme.Style.Item("color") = "#CCCCCC"
                'lblPleaseSelectScheme.Style.Item("font-style") = "italic"

                cblSchemeList.Controls.Add(lblPleaseSelectScheme)
            Else
                If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                    Dim udtSchemeEFormList As SchemeEFormModelCollection

                    udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                    For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                        Dim chkScheme As CheckBox = row.FindControl("chk" + udtSchemeEForm.SchemeCode.Trim)
                        Dim ltlConBr As LiteralControl = row.FindControl("ltlConBr" + udtSchemeEForm.SchemeCode.Trim)
                        Dim chkAllowNonClinicSetting As CheckBox = row.FindControl("chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                        Dim ltlConBrOption As LiteralControl = row.FindControl("ltlConBrOption" + udtSchemeEForm.SchemeCode)
                        Dim lblAllowNonClinicSetting As Label = row.FindControl("lblAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                        Dim ltlConBrStatment As LiteralControl = row.FindControl("ltlConBrStatment" + udtSchemeEForm.SchemeCode)

                        If Not chkScheme Is Nothing And Not ltlConBr Is Nothing Then
                            If CStr(dtPractice.Rows(row.RowIndex).Item(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession")).Trim = YesNo.Yes Then
                                chkScheme.Visible = True
                                ltlConBr.Visible = True

                                If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                                    If Not chkAllowNonClinicSetting Is Nothing And Not ltlConBrOption Is Nothing Then
                                        chkAllowNonClinicSetting.Visible = False
                                        ltlConBrOption.Visible = False

                                        'Tick Non-clinic checkbox from settings
                                        If dtPractice.Rows(row.RowIndex)(Mid(chkScheme.ID, Len("chk") + 1) + "_Selected") = YesNo.Yes Then
                                            chkAllowNonClinicSetting.Visible = True
                                            ltlConBrOption.Visible = True
                                        End If

                                        If Not lblAllowNonClinicSetting Is Nothing And Not ltlConBrStatment Is Nothing Then
                                            If dtPractice.Rows(row.RowIndex)(Mid(chkScheme.ID, Len("chk") + 1) + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                                chkScheme.Checked = True
                                                chkAllowNonClinicSetting.Checked = True
                                                lblAllowNonClinicSetting.Visible = True
                                                ltlConBrStatment.Visible = True
                                            End If
                                        End If

                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        Next

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        'udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
        udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00022, "Input Practice")

        Dim i As Integer = 0
        'For Each row As DataRow In dt.Rows
        For Each row As DataRow In dtPractice.Rows
            row.Item("PracticeIndex") = i
            i = i + 1
        Next

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        If Not IsNothing(udtSP.ThirdPartyAdditionalFieldEnrolmentList) Then
            i = 0
            For Each udtThirdPartyEnrolmentModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel In udtSP.ThirdPartyAdditionalFieldEnrolmentList.GetListBySysCode(ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD)
                udtThirdPartyEnrolmentModel.PracticeDisplaySeq = i + 1
                i = i + 1
            Next
        End If

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        Dim dtMO As DataTable
        dtMO = Session(SESS_MO)

        Dim strMOIndex As String = String.Empty

        Dim blnNoPracticeInMO As Boolean = False

        For j As Integer = 0 To dtMO.Rows.Count - 1
            'Dim dv As DataView = New DataView(dt)
            Dim dv As DataView = New DataView(dtPractice)
            dv.RowFilter = "MOIndex = '" + (j + 1).ToString.Trim + "'"

            If dv.Count = 0 Then
                blnNoPracticeInMO = True
                strMOIndex = strMOIndex + ", " + dtMO.Rows(j).Item("MOEName")
            End If
        Next

        If strMOIndex.Length > 2 Then
            strMOIndex = strMOIndex.Substring(2)
        End If

        If blnNoPracticeInMO Then

            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            'udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
            udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count)

            udtSM = New SystemMessage(LocalFunctionCode, SeverityCode.SEVE, MsgCode.MSG00020)
            udcMsgBox.AddMessage(udtSM, "%s", strMOIndex)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00024, "Input Practice Fail")
        Else

            If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                udcMsgBox.Visible = False

                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                'udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
                udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count)
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00023, "Input Practice Completed")

                udtEFormBLL.ClearRedirectPageSession()

                Session(eFormBLL.SESS_Bank) = "Y"
                Response.Redirect("~/BankAccount.aspx")

            Else
                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                'udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
                udtAuditLogEntry.AddDescripton("No. of Practice", dtPractice.Rows.Count)
                udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00024, "Input Practice Fail")
            End If
        End If
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

    End Sub

    Protected Sub ibtnCopyConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim i As Integer = 0
        Dim intComboBox As Integer = 0

        If Not IsNothing(Session(SESS_Practice)) Then

            Dim dtPractice As New DataTable
            dtPractice = Session(SESS_Practice)

            If Not Me.ddlPracticeList.SelectedValue.Trim.Equals(String.Empty) Then
                i = CInt(ddlPracticeList.SelectedValue.Trim)
            End If

            Dim dt As DataTable = New DataTable

            dt = addEmptyDataRowToDataTable(dtPractice)
            Dim intNewRow As Integer = dt.Rows.Count - 1

            ' Practice Name Information
            If Me.choCopyList.Items(0).Selected Then
                dt.Rows(intNewRow).Item("MOIndex") = dt.Rows(i).Item("MOIndex")
            End If

            ' Practice Name Information
            If Me.choCopyList.Items(1).Selected Then
                dt.Rows(intNewRow).Item("PracticeName") = dt.Rows(i).Item("PracticeName")
                dt.Rows(intNewRow).Item("PracticeNameChi") = dt.Rows(i).Item("PracticeNameChi")
            End If

            ' Practice Address Information
            If Me.choCopyList.Items(2).Selected Then
                dt.Rows(intNewRow).Item("Room") = dt.Rows(i).Item("Room")
                dt.Rows(intNewRow).Item("Floor") = dt.Rows(i).Item("Floor")
                dt.Rows(intNewRow).Item("Block") = dt.Rows(i).Item("Block")
                dt.Rows(intNewRow).Item("Building") = dt.Rows(i).Item("Building")
                dt.Rows(intNewRow).Item("ChiBuilding") = dt.Rows(i).Item("ChiBuilding")
                dt.Rows(intNewRow).Item("District") = dt.Rows(i).Item("District")
            End If

            ' PracticeTel Information
            If Me.choCopyList.Items(3).Selected Then
                dt.Rows(intNewRow).Item("PhoneDaytime") = dt.Rows(i).Item("PhoneDaytime")
            End If

            ' HealthProf Information
            'If Me.choCopyList.Items(4).Selected Then
            dt.Rows(intNewRow).Item("ServiceCategoryCode") = dt.Rows(i).Item("ServiceCategoryCode")
            'End If

            ' RegCode Information
            'If Me.choCopyList.Items(5).Selected Then
            dt.Rows(intNewRow).Item("RegistrationCode") = dt.Rows(i).Item("RegistrationCode")
            'End If


            dt.Rows(intNewRow).Item("Page") = Math.Ceiling(CType(intNewRow + 1, Double) / 5)

            Me.formatPracticeGV(dt, False)

            Me.ModalPopupExtenderCopyList.Hide()
            Me.choCopyList.ClearSelection()
        Else
            ModalPopupExtenderCopyList.Hide()
        End If
    End Sub

    Protected Sub ibtnNewPractice_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dtPractice As DataTable = New DataTable
        dtPractice = Session(SESS_Practice)

        Dim dt As DataTable = New DataTable

        dt = addEmptyDataRowToDataTable(dtPractice)
        Dim intNewRow As Integer = dt.Rows.Count - 1

        dt.Rows(intNewRow).Item("ServiceCategoryCode") = dt.Rows(0).Item("ServiceCategoryCode")
        dt.Rows(intNewRow).Item("RegistrationCode") = dt.Rows(0).Item("RegistrationCode")

        dt.Rows(intNewRow).Item("Page") = Math.Ceiling(CType(intNewRow + 1, Double) / 5)

        Me.formatPracticeGV(dt, False)

        Me.ModalPopupExtenderCopyList.Hide()
        Me.choCopyList.ClearSelection()
    End Sub

    Protected Sub ibtnCopyCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ModalPopupExtenderCopyList.Hide()
        Me.choCopyList.ClearSelection()
    End Sub

    Protected Sub LinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnPage1.Click, lnkBtnPage2.Click, lnkBtnPage3.Click, lnkBtnPage4.Click, lnkBtnPage5.Click, lnkBtnPage6.Click, lnkBtnPage7.Click, lnkBtnPage8.Click, lnkBtnPage9.Click, lnkBtnPage10.Click, lnkBtnPage11.Click, lnkBtnPage12.Click, lnkBtnPage13.Click, lnkBtnPage14.Click, lnkBtnPage15.Click, lnkBtnPage16.Click, lnkBtnPage17.Click, lnkBtnPage18.Click, lnkBtnPage19.Click, lnkBtnPage20.Click
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim lnkBtn As New LinkButton
        lnkBtn = CType(sender, LinkButton)

        Dim intPage As Integer
        intPage = CInt(lnkBtn.Text)

        Dim content As ContentPlaceHolder
        content = Page.Master.FindControl("ContentPlaceHolder1")

        Dim intExistPage As Integer
        intExistPage = Session(SESS_Page)

        Dim blnOtherPage As Boolean = False

        Session(SESS_Page) = intPage

        If intExistPage <> intPage Then
            blnOtherPage = True
        End If

        Dim dt As DataTable
        If blnOtherPage Then
            dt = Me.getPracticeFromGridView(True)
        Else
            dt = Me.getPracticeFromGridView(udcMsgBox.Visible)
        End If

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
        udtAuditLogEntry.AddDescripton("Go to Page", intPage)

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00028, "Click Page Link in Practice")


        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False

            If blnOtherPage Then
                If Not IsNothing(Session(SESS_Practice)) Then

                    Dim dv As DataView = New DataView(dt)

                    dv.RowFilter = "Page = '" + intPage.ToString + "' or Page = ''"
                    Me.gvRegPractice.DataSource = dv
                    gvRegPractice.DataBind()

                    Me.lblPracticePaging.Visible = True

                    Dim strPageInfo As String

                    strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

                    strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
                    strPageInfo = strPageInfo.Replace("%e", CStr(intPage))
                    strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))

                    Me.lblPracticePaging.Text = strPageInfo

                    udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                    udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
                    udtAuditLogEntry.AddDescripton("Go to Page", intPage)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00029, "Click Page Link in Practice Complete")

                End If
            Else
                udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
                udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
                udtAuditLogEntry.AddDescripton("Go to Page", intPage)
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00066, "Click Page Link in Practice Complete at the same page")
            End If

        Else
            'udcMsgBox.BuildMessageBox("ValidationFail")
            udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
            udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
            udtAuditLogEntry.AddDescripton("Go to Page", intPage)
            udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00030, "Click Page Link in Practice Fail")
        End If

    End Sub

    Private Sub formatPracticeGV(ByVal dt As DataTable, ByVal blnFirstPage As Boolean)
        Dim dv As DataView

        dv = New DataView(dt)

        Dim intPage As Integer
        intPage = Math.Ceiling(CType(dt.Rows.Count, Double) / 5)

        If blnFirstPage Then
            Session(SESS_Page) = 1
            dv.RowFilter = "Page = '1' or Page = ''"
        Else
            Session(SESS_Page) = intPage
            dv.RowFilter = "Page = '" + intPage.ToString + "' or Page = ''"
        End If

        Me.gvRegPractice.DataSource = dv
        gvRegPractice.DataBind()


        Me.lblPracticePaging.Visible = True

        Dim strPageInfo As String

        strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")

        strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
        strPageInfo = strPageInfo.Replace("%e", CStr(intPage))
        strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))

        Me.lblPracticePaging.Text = strPageInfo

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

    Protected Sub lnkBtnPersonal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim dt As DataTable
        dt = Me.getPracticeFromGridView(False)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00026, "Press Tab to Personal Particulars in Practice")

        udtEFormBLL.ClearRedirectPageSession()
        Session(eFormBLL.SESS_PersonalParticular) = "Y"
        Response.Redirect("~/PersonalPacticulars.aspx")

    End Sub


    Protected Sub lnkBtnMO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

        Dim dt As DataTable
        dt = Me.getPracticeFromGridView(False)

        Dim udtSP As ServiceProvider.ServiceProviderModel
        udtSP = udtSPBLL.GetSP

        udtAuditLogEntry.AddDescripton("HKID", udtSP.HKID)
        udtAuditLogEntry.AddDescripton("No. of Practice", dt.Rows.Count)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00027, "Press Tab to MO in Practice")

        udtEFormBLL.ClearRedirectPageSession()

        Session(eFormBLL.SESS_MedicalOrganization) = "Y"
        Response.Redirect("~/MedicalOrganization.aspx")
    End Sub

    Private Sub RenderLanguage()

        If Not IsNothing(Me.gvRegPractice) Then
            For Each row As GridViewRow In gvRegPractice.Rows
                Dim ddlRegPracticeDistrict As DropDownList = CType(row.FindControl("ddlRegPracticeDistrict"), DropDownList)

                If Not IsNothing(ddlRegPracticeDistrict) Then
                    ddlRegPracticeDistrict.Items(0).Text = Me.GetGlobalResourceObject("Text", "SelectDistrict")
                End If

                Dim ddlRegPracticeHealthProf As DropDownList = CType(row.FindControl("ddlRegPracticeHealthProf"), DropDownList)
                Dim hfRegPracticeHealthProf As HiddenField = CType(row.FindControl("hfRegPracticeHealthProf"), HiddenField)

                ddlRegPracticeHealthProf.Items.Clear()
                ddlRegPracticeHealthProf.DataSource = udtEFormBLL.GetHealthProf

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------

                ddlRegPracticeHealthProf.DataValueField = "ServiceCategoryCode"
                If Session("language") = "zh-tw" Then
                    ddlRegPracticeHealthProf.DataTextField = "ServiceCategoryDescChi"
                Else
                    ddlRegPracticeHealthProf.DataTextField = "ServiceCategoryDesc"
                End If

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

                ddlRegPracticeHealthProf.DataBind()
                ddlRegPracticeHealthProf.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "SelectHealthProf"), ""))

                ddlRegPracticeHealthProf.SelectedValue = hfRegPracticeHealthProf.Value

            Next

            If Me.lblPracticePaging.Visible Then

                Dim dtPractice As DataTable
                dtPractice = Session(SESS_Practice)

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strRegPracticeHealthProf As String = String.Empty
                If Not dtPractice Is Nothing AndAlso dtPractice.Rows.Count > 0 Then
                    strRegPracticeHealthProf = CStr(dtPractice.Rows(0).Item("ServiceCategoryCode")).Trim
                End If

                For Each row As GridViewRow In gvRegPractice.Rows
                    If strRegPracticeHealthProf = String.Empty Then
                        Dim cblSchemeList As HtmlGenericControl
                        cblSchemeList = row.FindControl("cblSchemeList")
                        cblSchemeList.InnerHtml = ""

                        'When not yet select the Health Profession, the remark should be shown.
                        Dim lblPleaseSelectScheme As New Label
                        lblPleaseSelectScheme.Visible = True
                        lblPleaseSelectScheme.Text = GetGlobalResourceObject("Text", "SelectHealthProf")
                        'lblPleaseSelectScheme.Style.Item("color") = "#CCCCCC"
                        'lblPleaseSelectScheme.Style.Item("font-style") = "italic"

                        cblSchemeList.Controls.Add(lblPleaseSelectScheme)
                    Else
                        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                            Dim udtSchemeEFormList As SchemeEFormModelCollection

                            udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                            For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                                Dim chkScheme As CheckBox = row.FindControl("chk" + udtSchemeEForm.SchemeCode.Trim)
                                Dim ltlConBr As LiteralControl = row.FindControl("ltlConBr" + udtSchemeEForm.SchemeCode.Trim)
                                Dim chkAllowNonClinicSetting As CheckBox = row.FindControl("chkAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                                Dim ltlConBrOption As LiteralControl = row.FindControl("ltlConBrOption" + udtSchemeEForm.SchemeCode)
                                Dim lblAllowNonClinicSetting As Label = row.FindControl("lblAllowNonClinicSetting" + udtSchemeEForm.SchemeCode)
                                Dim ltlConBrStatment As LiteralControl = row.FindControl("ltlConBrStatment" + udtSchemeEForm.SchemeCode)

                                If Not chkScheme Is Nothing And Not ltlConBr Is Nothing Then
                                    If CStr(dtPractice.Rows(row.RowIndex).Item(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession")).Trim = YesNo.Yes Then
                                        chkScheme.Visible = True
                                        ltlConBr.Visible = True

                                        If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes Then
                                            If Not chkAllowNonClinicSetting Is Nothing And Not ltlConBrOption Is Nothing Then
                                                chkAllowNonClinicSetting.Visible = False
                                                ltlConBrOption.Visible = False

                                                'Tick Non-clinic checkbox from settings
                                                If dtPractice.Rows(row.RowIndex)(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then
                                                    chkAllowNonClinicSetting.Visible = True
                                                    ltlConBrOption.Visible = True
                                                End If

                                                If Not lblAllowNonClinicSetting Is Nothing And Not ltlConBrStatment Is Nothing Then
                                                    If dtPractice.Rows(row.RowIndex)(udtSchemeEForm.SchemeCode.Trim + "_NonClinicSetting_Selected") = YesNo.Yes Then
                                                        chkScheme.Checked = True
                                                        chkAllowNonClinicSetting.Checked = True
                                                        lblAllowNonClinicSetting.Visible = True
                                                        ltlConBrStatment.Visible = True
                                                    End If
                                                End If

                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next

                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                Dim strPageInfo As String
                Dim doublePageCount As Double

                strPageInfo = Me.GetGlobalResourceObject("Text", "GridPageInfo")
                If IsNothing(dtPractice) Then
                    doublePageCount = Math.Ceiling(1 / 5)
                Else
                    doublePageCount = Math.Ceiling(CType(dtPractice.Rows.Count, Double) / 5)
                End If


                strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
                strPageInfo = strPageInfo.Replace("%e", CStr(doublePageCount))
                If IsNothing(dtPractice) Then
                    strPageInfo = strPageInfo.Replace("%f", "1")
                Else
                    strPageInfo = strPageInfo.Replace("%f", CStr(dtPractice.Rows.Count))
                End If


                Me.lblPracticePaging.Text = strPageInfo
            End If
        End If
    End Sub

    Protected Sub ddlRegPracticeHealthProf_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        udcMsgBox.Visible = False
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim dtPractice As New DataTable
        dtPractice = Me.getPracticeFromGridView(False)

        If Not IsNothing(dtPractice) Then

            If dtPractice.Rows.Count > 0 Then
                Dim strProf As String = String.Empty

                strProf = CStr(dtPractice.Rows(0).Item("ServiceCategoryCode")).Trim

                For Each dr As DataRow In dtPractice.Rows
                    dr.Item("ServiceCategoryCode") = strProf

                    ''CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    ''-----------------------------------------------------------------------------------------
                    'When Health Profession is changed, all checkboxes are reset.
                    If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then

                        Dim udtSchemeEFormList As SchemeEFormModelCollection

                        udtSchemeEFormList = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                        For Each udtSchemeEForm As SchemeEFormModel In udtSchemeEFormList
                            dr.Item(udtSchemeEForm.SchemeCode + "_Selected") = YesNo.No
                        Next
                    End If
                    ''CRE16-002 (Revamp VSS) [End][Chris YIM]
                Next

                Session(SESS_Practice) = dtPractice

            End If

        End If

        Me.formatPracticeGV(dtPractice, True)

        dtPractice = Me.getPracticeFromGridView(udcMsgBox.Visible)

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False
        Else
            udcMsgBox.BuildMessageBox("ValidationFail")
        End If



    End Sub

    Protected Sub txtRegPracticeRegCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        udcMsgBox.Visible = False
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Dim dtPractice As New DataTable
        dtPractice = Me.getPracticeFromGridView(False)

        If Not IsNothing(dtPractice) Then

            If dtPractice.Rows.Count > 0 Then
                Dim strRegCode As String = String.Empty

                strRegCode = CStr(dtPractice.Rows(0).Item("RegistrationCode")).Trim

                For Each dr As DataRow In dtPractice.Rows
                    dr.Item("RegistrationCode") = strRegCode
                Next

                Session(SESS_Practice) = dtPractice
            End If

        End If

        Me.formatPracticeGV(dtPractice, True)

        dtPractice = Me.getPracticeFromGridView(udcMsgBox.Visible)

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False
        Else
            udcMsgBox.BuildMessageBox("ValidationFail")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) Then
                RenderLanguage()
            End If
        End If
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

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Sub iBtnLoadDemoData_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles iBtnLoadDemoData.Click
        Me.LoadDemoData()
    End Sub

    Private Sub LoadDemoData()
        Dim strPageInfo As String = Me.GetGlobalResourceObject("Text", "GridPageInfo")

        Dim ds As DataSet = Me.GetDemoData
        If ds Is Nothing Then Exit Sub

        Dim dt As DataTable = ds.Tables("Practice")
        Session(SESS_Practice) = dt.Copy
        Me.formatPracticeGV(dt, False)

        Dim doublePageCount As Double
        doublePageCount = Math.Ceiling(CType(dt.Rows.Count, Double) / 5)

        strPageInfo = strPageInfo.Replace("%d", CStr(Session(SESS_Page)))
        strPageInfo = strPageInfo.Replace("%e", CStr(doublePageCount))
        strPageInfo = strPageInfo.Replace("%f", CStr(dt.Rows.Count))
    End Sub
    ' CRE12-001 eHS and PCD integration [End][Koala]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub chkScheme_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        udcMsgBox.Visible = False

        Dim dtPractice As New DataTable
        dtPractice = Me.getPracticeFromGridView(False)

        If Not IsNothing(dtPractice) Then

            If dtPractice.Rows.Count > 0 Then
                Session(SESS_Practice) = dtPractice
            End If

        End If

        Me.formatPracticeGV(dtPractice, True)

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False
        Else
            udcMsgBox.BuildMessageBox("ValidationFail")
        End If

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub chkAllowNonClinicSetting_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        udcMsgBox.Visible = False

        Dim dtPractice As New DataTable
        dtPractice = Me.getPracticeFromGridView(False)

        If Not IsNothing(dtPractice) Then

            If dtPractice.Rows.Count > 0 Then

                Session(SESS_Practice) = dtPractice
            End If

        End If

        Me.formatPracticeGV(dtPractice, True)

        If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
            udcMsgBox.Visible = False
        Else
            udcMsgBox.BuildMessageBox("ValidationFail")
        End If

    End Sub
    'CRE16-002 (Revamp VSS) [End][Chris YIM]
End Class