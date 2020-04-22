Imports Common.Component
Imports Common.Component.District
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Profession
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports System.Globalization

Partial Public Class ucTypeOfPracticeGrid
    Inherits System.Web.UI.UserControl

    Private Const SESS_Practice As String = "PracticeBank"
    Private Const SESS_MO As String = "MO"

    Private udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
    Private udtDistrictBLL As DistrictBLL = New DistrictBLL
    Private GeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"

    Public Enum EnumColumn
        CheckBox = 0
        PracticeNo
        PracticeInfo
        Profession
        SchemeName
        TypeOfPractice
        TypeOfPracticeView
    End Enum

    Public Enum EnumMode
        View
        Create
        Transfer
    End Enum

    Private Const VIEWSTATE_MODE As String = "MODE"

    Public ReadOnly Property GridView() As GridView
        Get
            Return Me.gvPracticeInfo
        End Get
    End Property

    Public Function GetThirdPartyAdditionalFieldEnrolmentCollection() As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection

        Dim udtThirdPartyEnrolmentCollection As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection = New ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection

        Dim udtThirdPartyEnrolmentModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel = Nothing

        For Each r As GridViewRow In Me.gvPracticeInfo.Rows

            Dim rdoTypeOfPractice As RadioButtonList = CType(r.FindControl("rdlTypeOfPractice"), RadioButtonList)
            Dim lblPracticeDisplaySeq As Label = CType(r.FindControl("lblPracticeDisplaySeq"), Label)

            If rdoTypeOfPractice.SelectedIndex >= 0 Then
                udtThirdPartyEnrolmentModel = New ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel
                udtThirdPartyEnrolmentModel.EnrolRefNo = String.Empty
                udtThirdPartyEnrolmentModel.AdditionalFieldID = Common.PCD.EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString()
                udtThirdPartyEnrolmentModel.AdditionalFieldValueCode = rdoTypeOfPractice.SelectedValue
                udtThirdPartyEnrolmentModel.PracticeDisplaySeq = CType(lblPracticeDisplaySeq.Text, Integer)
                udtThirdPartyEnrolmentModel.SysCode = ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD
                udtThirdPartyEnrolmentCollection.Add(udtThirdPartyEnrolmentModel)
            End If

        Next
        Return udtThirdPartyEnrolmentCollection
    End Function

    Public Sub LoadThirdPartyEnrolmentCollection(ByVal udtThirdPartyEnrolmentCollection As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection)

        If Not IsNothing(udtThirdPartyEnrolmentCollection) Then

            'For Each udtThirdPartyEnrolmentModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel In udtThirdPartyEnrolmentCollection.Values
            '    For Each r As GridViewRow In Me.gvPracticeInfo.Rows
            '        Dim rdoTypeOfPractice As RadioButtonList = CType(r.FindControl("rdlTypeOfPractice"), RadioButtonList)
            '        Dim chkSelect As CheckBox = CType(r.FindControl("chkSelect"), CheckBox)
            '        Dim lblTypeOfPractice As Label = CType(r.FindControl("lblTypeOfPractice"), Label)
            '        rdoTypeOfPractice.Enabled = False
            '        rdoTypeOfPractice.SelectedIndex = -1
            '        lblTypeOfPractice.Text = String.Empty
            '        chkSelect.Checked = False
            '    Next
            'Next

            For Each r As GridViewRow In Me.gvPracticeInfo.Rows
                If Me.Mode = EnumMode.View Then
                    r.Visible = False
                End If

                Dim rdoTypeOfPractice As RadioButtonList = CType(r.FindControl("rdlTypeOfPractice"), RadioButtonList)
                Dim lblPracticeDisplaySeq As Label = CType(r.FindControl("lblPracticeDisplaySeq"), Label)
                Dim lblTypeOfPractice As Label = CType(r.FindControl("lblTypeOfPractice"), Label)
                Dim chkSelect As CheckBox = CType(r.FindControl("chkSelect"), CheckBox)

                For Each udtThirdPartyEnrolmentModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel In udtThirdPartyEnrolmentCollection.GetListBySysCode(ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD)

                    If udtThirdPartyEnrolmentModel.AdditionalFieldID <> Common.PCD.EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString() Then Continue For

                    If (udtThirdPartyEnrolmentModel.PracticeDisplaySeq = CType(lblPracticeDisplaySeq.Text, Integer)) Then
                        r.Visible = True
                        If Not (udtThirdPartyEnrolmentModel.AdditionalFieldValueCode.Trim = String.Empty) Then
                            rdoTypeOfPractice.Enabled = True
                            rdoTypeOfPractice.SelectedValue = udtThirdPartyEnrolmentModel.AdditionalFieldValueCode
                            lblTypeOfPractice.Text = GetTypeOfPracticeName(udtThirdPartyEnrolmentModel.AdditionalFieldValueCode)
                            chkSelect.Checked = True
                        End If

                    End If

                Next
            Next

        End If

    End Sub

    Private Function GetTypeOfPracticeName(ByVal code As String)
        Dim selectedLanguageValue As String = LCase(Session("language"))
        Dim strName As String = String.Empty
        If selectedLanguageValue.Equals(English) Then
            strName = Common.Component.PracticeType_PCD.PracticeType_PCDBLL.GetPracticeTypeByCode(code).DataValue
        Else
            strName = Common.Component.PracticeType_PCD.PracticeType_PCDBLL.GetPracticeTypeByCode(code).DataValueChi
        End If
        Return strName
    End Function

    Public Property Mode() As EnumMode
        Get
            Return IIf(Me.ViewState(VIEWSTATE_MODE) Is Nothing, EnumMode.View, Me.ViewState(VIEWSTATE_MODE))
        End Get
        Set(ByVal value As EnumMode)
            Me.ViewState(VIEWSTATE_MODE) = value
        End Set
    End Property


    'Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim chkSelectAll As CheckBox
    '    chkSelectAll = CType(sender, CheckBox)
    '    If chkSelectAll.ClientID.Contains("chkSelectAll") Then
    '        If chkSelectAll.Checked Then
    '            SetAllChkSelect(True)
    '        Else
    '            SetAllChkSelect(False)
    '        End If
    '    End If
    '    EnableTypeOfPractice()
    'End Sub

    'Protected Sub chkSelect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    EnableTypeOfPractice()
    'End Sub

    'Private Sub SetAllChkSelect(ByVal value As Boolean)
    '    For Each r As GridViewRow In gvPracticeInfo.Rows
    '        CType(r.FindControl("chkSelect"), CheckBox).Checked = value
    '    Next
    'End Sub

    'Public Sub ClearSelection()
    '    Dim rdlTypeOfPractice As RadioButtonList
    '    Dim chkSelect As CheckBox

    '    For Each r As GridViewRow In gvPracticeInfo.Rows

    '        rdlTypeOfPractice = CType(r.FindControl("rdlTypeOfPractice"), RadioButtonList)
    '        chkSelect = CType(r.FindControl("chkSelect"), CheckBox)

    '        For i As Integer = 0 To rdlTypeOfPractice.Items.Count - 1
    '            chkSelect.Checked = False
    '            rdlTypeOfPractice.Enabled = False
    '            rdlTypeOfPractice.SelectedIndex = -1
    '        Next

    '    Next
    'End Sub

    Public Sub EnableTypeOfPractice()
        If Me.Mode = EnumMode.View Then Exit Sub

        Dim chkSelect As CheckBox

        If Not IsNothing(gvPracticeInfo) Then
            For Each r As GridViewRow In gvPracticeInfo.Rows
                chkSelect = CType(r.FindControl("chkSelect"), CheckBox)
                CheckTypeOfPractice(chkSelect.ClientID)
            Next
        End If

    End Sub

    Protected Sub CheckTypeOfPractice(ByVal strId As String)
        Dim JavaScript As String = String.Empty
        JavaScript = "TypeOfPracticeRadioSetup('" & strId & "');"
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, strId, JavaScript, True)
    End Sub

    Public Function PracticeSelected() As Boolean

        Dim blnSelected As Boolean = False

        Dim rdlTypeOfPractice As RadioButtonList
        Dim chkSelect As CheckBox

        For Each r As GridViewRow In gvPracticeInfo.Rows

            rdlTypeOfPractice = CType(r.FindControl("rdlTypeOfPractice"), RadioButtonList)
            chkSelect = CType(r.FindControl("chkSelect"), CheckBox)

            For i As Integer = 0 To rdlTypeOfPractice.Items.Count - 1
                If chkSelect.Checked Then
                    blnSelected = True
                End If
            Next

        Next

        Return blnSelected

    End Function

    Public Function TypeOfPracticeSelected() As Boolean

        Dim blnSelected As Boolean = True

        Dim rdlTypeOfPractice As RadioButtonList
        Dim chkSelect As CheckBox
        Dim imgAlert As Image

        For Each r As GridViewRow In gvPracticeInfo.Rows

            rdlTypeOfPractice = CType(r.FindControl("rdlTypeOfPractice"), RadioButtonList)
            chkSelect = CType(r.FindControl("chkSelect"), CheckBox)
            imgAlert = CType(r.FindControl("imgAlert"), Image)

            For i As Integer = 0 To rdlTypeOfPractice.Items.Count - 1
                If chkSelect.Checked And rdlTypeOfPractice.SelectedIndex = -1 Then
                    'rdlTypeOfPractice.ForeColor = Drawing.Color.Red
                    imgAlert.Visible = True
                    blnSelected = False
                Else
                    'rdlTypeOfPractice.ForeColor = Drawing.Color.Black
                    imgAlert.Visible = False
                End If
            Next

        Next

        Return blnSelected

    End Function

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Sub LoadPractice(ByVal objSP As ServiceProviderModel)    
    Public Sub LoadPractice(ByVal objSP As ServiceProviderModel, ByVal objPracticeList As PracticeModelCollection)
        Select Case Me.Mode
            Case EnumMode.View
                LoadPracticeView(objSP, objPracticeList)
            Case EnumMode.Create
                LoadPracticeCreate(objSP, objPracticeList)
            Case EnumMode.Transfer
                LoadPracticeTransfer(objSP, objPracticeList)
            Case Else
                Throw New Exception(String.Format("[ucTypeOfPracticeGrid] Unhandled Mode ({0})", Me.Mode))
        End Select
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
        EnableTypeOfPractice()
    End Sub

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Private Sub LoadPracticeView(ByVal objSP As ServiceProviderModel)
    Private Sub LoadPracticeView(ByVal objSP As ServiceProviderModel, ByVal objPracticeList As PracticeModelCollection)
        Me.gvPracticeInfo.Columns(EnumColumn.CheckBox).Visible = False
        Me.gvPracticeInfo.Columns(EnumColumn.TypeOfPractice).Visible = False
        Me.gvPracticeInfo.Columns(EnumColumn.TypeOfPracticeView).Visible = True
        'BindPractice(objSP)
        BindPractice(objSP, objPracticeList)
    End Sub
    ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Private Sub LoadPracticeCreate(ByVal objSP As ServiceProviderModel)
    Private Sub LoadPracticeCreate(ByVal objSP As ServiceProviderModel, ByVal objPracticeList As PracticeModelCollection)
        Me.gvPracticeInfo.Columns(EnumColumn.CheckBox).Visible = True
        Me.gvPracticeInfo.Columns(EnumColumn.TypeOfPractice).Visible = True
        Me.gvPracticeInfo.Columns(EnumColumn.TypeOfPracticeView).Visible = False
        'BindPractice(objSP)
        BindPractice(objSP, objPracticeList)
    End Sub
    ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Private Sub LoadPracticeTransfer(ByVal objSP As ServiceProviderModel)
    Private Sub LoadPracticeTransfer(ByVal objSP As ServiceProviderModel, ByVal objPracticeList As PracticeModelCollection)
        Me.gvPracticeInfo.Columns(EnumColumn.CheckBox).Visible = True
        Me.gvPracticeInfo.Columns(EnumColumn.TypeOfPractice).Visible = True
        Me.gvPracticeInfo.Columns(EnumColumn.TypeOfPracticeView).Visible = False
        'BindPractice(objSP)
        BindPractice(objSP, objPracticeList)
    End Sub
    ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Private Sub BindPractice(ByVal objSP As ServiceProviderModel)
    Private Sub BindPractice(ByVal objSP As ServiceProviderModel, ByVal objPracticeList As PracticeModelCollection)
        'Me.gvPracticeInfo.DataSource = objSP.PracticeList.FilterByPCD().Values
        Me.gvPracticeInfo.DataSource = objPracticeList.Values
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Me.gvPracticeInfo.DataBind()

        If Not IsNothing(objSP.ThirdPartyAdditionalFieldEnrolmentList) Then
            LoadThirdPartyEnrolmentCollection(objSP.ThirdPartyAdditionalFieldEnrolmentList)
        End If

    End Sub

#Region "Formatting functions"
    'Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
    '    Return (New Common.Format.Formatter).formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
    'End Function

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
        Dim strAreacode As String
        strAreacode = getAreaString(strDistrict)
        Return (New Common.Format.Formatter).formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
    End Function

    Protected Function formatChiAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
        Dim strAreacode As String
        strAreacode = getAreaString(strDistrict)
        Return (New Common.Format.Formatter).formatAddressChi(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return (New Common.Format.Formatter).formatChineseName(strChineseString)
    End Function

    Public Function getAreaString(ByVal strDistrict As String) As String
        Dim strAreaCode As String

        If strDistrict.Equals(String.Empty) Then
            strAreaCode = String.Empty
        Else
            strAreaCode = GetAreaByDistrictCode(strDistrict)
        End If

        Return strAreaCode
    End Function

    Public Function GetAreaByDistrictCode(ByVal strDistrictCode As String) As String
        Dim udtDistrictModelCollection As DistrictModelCollection = GetDistrict()
        Dim udtDistrictModel As DistrictModel

        udtDistrictModel = udtDistrictModelCollection.Item(strDistrictCode)

        Return udtDistrictModel.Area_ID
    End Function

    ''' <summary>
    ''' Get the lists of district by area code
    ''' </summary>
    ''' <param name="strAreaCode"></param>
    ''' <returns>DistrictModelCollection</returns>
    ''' <remarks></remarks>
    Public Function GetDistrict(Optional ByVal strAreaCode As String = "") As DistrictModelCollection
        Dim udtDistrictModelCollection As DistrictModelCollection = New DistrictModelCollection
        If strAreaCode.Equals(String.Empty) Then
            udtDistrictModelCollection = udtDistrictBLL.GetDistrictList
        Else
            udtDistrictModelCollection = udtDistrictBLL.GetDistrictListByAreaCode(strAreaCode)
        End If

        Return udtDistrictModelCollection
    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        If Session("language") = "zh-tw" Then
            Return ProfessionBLL.GetProfessionListByServiceCategoryCode(strHealthProfCode).ServiceCategoryDescChi
        Else
            Return ProfessionBLL.GetProfessionListByServiceCategoryCode(strHealthProfCode).ServiceCategoryDesc
        End If
    End Function
#End Region

    Private Sub gvPracticeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPracticeInfo.RowDataBound
        Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
        Dim udtSchemeBOList As SchemeBackOfficeModelCollection = Nothing
        Dim udtFormatter As New Common.Format.Formatter

        Select Case e.Row.RowType
            Case DataControlRowType.DataRow
                Dim objPractice As PracticeModel = e.Row.DataItem

                ' Practice Information

                ' Check whether non-clinic
                Dim blnNonClinic As Boolean = False

                For Each udtPSI As PracticeSchemeInfoModel In objPractice.PracticeSchemeInfoList.Values
                    If udtPSI.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                        blnNonClinic = True
                        Exit For
                    End If
                Next

                ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Dim lblRegBankPracticeEName As Label = e.Row.FindControl("lblRegBankPracticeEName")
                Dim lblRegBankPracticeCName As Label = e.Row.FindControl("lblRegBankPracticeCName")

                Dim lblRegBankPracticeAddress As Label = e.Row.FindControl("lblRegBankPracticeAddress")
                Dim lblRegBankPracticeAddressChi As Label = e.Row.FindControl("lblRegBankPracticeAddressChi")

                Dim strEngAddrWithoutDistrict As String = String.Empty
                Dim strChiAddrWithoutDistrict As String = String.Empty
                Dim strPracAddrRemarksEng As String = String.Empty
                Dim strPracAddrRemarksChi As String = String.Empty

                Dim blnPracticeRemarksTrim As Boolean = False
                Dim intPracticeRemarksMaxLength As Integer = 150

                If Not objPractice.PracticeAddress Is Nothing Then
                    strEngAddrWithoutDistrict = udtFormatter.FormatAddressWithoutDistrict(objPractice.PracticeAddress)
                    strChiAddrWithoutDistrict = udtFormatter.FormatAddressChiWithoutDistrict(objPractice.PracticeAddress)
                End If

                'Practice Eng Address:
                ' Display:  ({Remarks}) {Practice Address with District}
                ' To PCD:   ({Remarks}) {Practice Address w/o District}, Remarks will be trimmed if too long
                If objPractice.RemarksDesc <> String.Empty Then
                    strPracAddrRemarksEng = String.Format("({0}) {1}", objPractice.RemarksDesc, strEngAddrWithoutDistrict).Trim

                    If strPracAddrRemarksEng.Length > intPracticeRemarksMaxLength Then
                        lblRegBankPracticeAddress.Text = String.Format("({0}#) {1}", objPractice.RemarksDesc, lblRegBankPracticeAddress.Text)
                        blnPracticeRemarksTrim = True
                    Else
                        lblRegBankPracticeAddress.Text = String.Format("({0}) {1}", objPractice.RemarksDesc, lblRegBankPracticeAddress.Text).Trim
                    End If
                End If

                'Practice Chi Address: 
                ' Display:  {Practice Address with District} ({Remarks})
                ' To PCD:   {Practice Address w/o District} ({Remarks}), Remarks will be trimmed if too long
                If objPractice.RemarksDescChi <> String.Empty Then
                    strPracAddrRemarksChi = String.Format("{0} ({1})", strChiAddrWithoutDistrict, objPractice.RemarksDescChi).Trim

                    If strPracAddrRemarksChi.Length > intPracticeRemarksMaxLength Then
                        lblRegBankPracticeAddressChi.Text = String.Format("{0} ({1}#)", lblRegBankPracticeAddressChi.Text, objPractice.RemarksDescChi).Trim
                        blnPracticeRemarksTrim = True
                    Else
                        lblRegBankPracticeAddressChi.Text = String.Format("{0} ({1})", lblRegBankPracticeAddressChi.Text, objPractice.RemarksDescChi).Trim
                    End If
                End If

                'Format Practice Chinese Name
                lblRegBankPracticeCName.Text = formatChineseString(lblRegBankPracticeCName.Text)

                'Format Practice Chinese Address
                lblRegBankPracticeAddressChi.Text = formatChineseString(lblRegBankPracticeAddressChi.Text)
                ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

                ' Non Clinic
                Dim lblRegBankPracticeNonClinic As Label = DirectCast(e.Row.FindControl("lblRegBankPracticeNonClinic"), Label)
                Dim trRegBankPracticeNonClinic As HtmlTableRow = DirectCast(e.Row.FindControl("trRegBankPracticeNonClinic"), HtmlTableRow)

                trRegBankPracticeNonClinic.Visible = blnNonClinic
                lblRegBankPracticeNonClinic.Text = String.Format(" * {0}", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))

                ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                ' Remarks Trim
                Dim lblRegBankPracticeRemarksTrim As Label = DirectCast(e.Row.FindControl("lblRegBankPracticeRemarksTrim"), Label)
                Dim trRegBankPracticeRemarksTrim As HtmlTableRow = DirectCast(e.Row.FindControl("trRegBankPracticeRemarksTrim"), HtmlTableRow)

                trRegBankPracticeRemarksTrim.Visible = blnPracticeRemarksTrim
                lblRegBankPracticeRemarksTrim.Text = String.Format("# {0}", HttpContext.GetGlobalResourceObject("Text", "PracticeRemarksTrim"))

                Dim trRegBankPracticeNotes As HtmlTableRow = DirectCast(e.Row.FindControl("trRegBankPracticeNotes"), HtmlTableRow)
                trRegBankPracticeNotes.Visible = blnNonClinic OrElse blnPracticeRemarksTrim
                ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                Dim rdl As RadioButtonList = DirectCast(e.Row.FindControl("rdlTypeOfPractice"), RadioButtonList)
                Dim objProfession As ProfessionModel = ProfessionBLL.GetProfessionListByServiceCategoryCode(objPractice.Professional.ServiceCategoryCode)
                If objProfession.AllowJoinPCD Then
                    rdl.DataSource = objProfession.PracticeTypePCD
                    rdl.DataBind()
                End If

                If Me.gvPracticeInfo.Columns(EnumColumn.SchemeName).Visible Then
                    If udtSchemeBOList Is Nothing Then udtSchemeBOList = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup()

                    Dim lblSchemeName As Label = DirectCast(e.Row.FindControl("lblSchemeName"), Label)
                    Dim hashScheme As New Hashtable
                    Dim udtSchemeModel As PracticeSchemeInfo.PracticeSchemeInfoModel
                    Dim strSchemeList As String = String.Empty
                    For Each udtSchemeModel In objPractice.PracticeSchemeInfoList.Values
                        If Not hashScheme.Contains(udtSchemeModel.SchemeCode) Then
                            hashScheme.Add(udtSchemeModel.SchemeCode, udtSchemeModel.SchemeCode)
                            strSchemeList += udtSchemeBOList.Filter(udtSchemeModel.SchemeCode).DisplayCode + "<br/>"
                        End If
                    Next
                    lblSchemeName.Text = strSchemeList
                End If

                If Me.Mode <> EnumMode.View Then
                    CType(e.Row.FindControl("chkSelect"), CheckBox).Attributes.Add("onclick", "javascript:TypeOfPracticeRadioSetup('" & e.Row.FindControl("chkSelect").ClientID & "');")
                End If

                ' Prevent chinese profession name wrap in table
                Dim lblHealthProf As Label = DirectCast(e.Row.FindControl("lblHealthProf"), Label)
                Dim selectedLanguageValue As String = LCase(Session("language"))
                Dim strName As String = String.Empty
                If selectedLanguageValue.Equals(English) Then
                    lblHealthProf.Style.Item("white-space") = String.Empty
                Else
                    lblHealthProf.Style.Item("white-space") = "nowrap"
                End If
        End Select

    End Sub

End Class