Imports Common.Component
Imports Common.ComObject
Imports Common.Component.Address
Imports Common.Component.ServiceProvider
Imports Common.Component.Profession
Imports Common.Component.BankAcct
Imports Common.Component.Scheme
Imports Common.Component.District
Imports Common.Component.StaticData
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.Professional
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.SchemeInformation
Imports Common.Format
Imports System.Threading


Partial Public Class ucEnrolmentCopyPopup
    Inherits System.Web.UI.UserControl


#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const CloseButtonClick As String = "Enrolment Copy Popup - Close button click"
        Public Const CloseButtonClick_ID As String = LogID.LOG00305
    End Class
#End Region

#Region "Private Variables"
    Private _udtAuditLog As AuditLogEntry = Nothing
#End Region

    Private Const NOT_AVAILABLE As String = "N/A"

    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"

    Private udtFormatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtSchemeBLL As New SchemeBackOfficeBLL
    Private udtStaticDataBLL As New StaticDataBLL
    Private udtDistrictBLL As New DistrictBLL
    Private udtSPBLL As New ServiceProviderBLL
    Private udtSchemeEFormBLL As New SchemeEFormBLL
    Private udtGeneralFunction As New Common.ComFunction.GeneralFunction

    Private udtSPOBLL As SPProfileBLL = New SPProfileBLL
    Private udtSP As ServiceProviderModel = New ServiceProviderModel
    Private udtProfModelList As ProfessionalModelCollection = New ProfessionalModelCollection
    Private udtBankAcctList As BankAcctModelCollection = New BankAcctModelCollection
    Private udtPracSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection



    Public Enum enumButtonClick
        Close
    End Enum

    Public Event ButtonClick(ByVal e As enumButtonClick)

    ''' <summary>
    ''' Use for handle mouse click and move popup numpad (ModalPopupExtender)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Header() As Control
        Get
            Return Me.panHeader
        End Get
    End Property

    Public ReadOnly Property ButtonClose() As ImageButton
        Get
            Return Me.ibtnClose
        End Get
    End Property


#Region "Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Setup()
    End Sub

    Private Sub ibtnClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnClose.Click
        Me._udtAuditLog.WriteLog(AuditLogDesc.CloseButtonClick_ID, AuditLogDesc.CloseButtonClick)
        RaiseEvent ButtonClick(enumButtonClick.Close)
    End Sub

#End Region

    Public Sub Setup()
        ' Init audit log object
        Me._udtAuditLog = New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode, Me.Page)
    End Sub

    Public Sub LoadRecord()
        '---------- Debug Data ------------
        'A11906000000086
        'A12120000000180
        'Session("language") = English
        'Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(English)
        '---------- Debug Data ------------
        'FunctionCode = FunctCode.FUNT010202

        ' load a service provider
        Dim strERN As String = GetSP.EnrolRefNo
        udtSPOBLL.GetServiceProviderOriginalProfile(strERN, udtSP, udtProfModelList, udtBankAcctList, udtPracSchemeInfoList)

        LoadEnrollmentInfo()
        LoadPersonalParticulars()
        LoadSelectedScheme()
        LoadMO(udtSP)
        LoadPractice(udtSP)
        LoadTypeOfPractice(udtSP)
        LoadOtherSystem()
    End Sub

    Private Function GetSP() As ServiceProviderModel
        Dim bll As New ServiceProviderBLL
        Return bll.GetSP()
    End Function

    Private Sub LoadEnrollmentInfo()
        Me.lblERN.Text = udtFormatter.formatSystemNumber(udtSP.EnrolRefNo)
        Me.lblEnrolDtm.Text = udtFormatter.convertDateTime(udtSP.EnrolDate)
    End Sub

    Private Sub LoadPersonalParticulars()
        With udtSP
            lblConfirmEname.Text = .EnglishName
            lblConfirmCname.Text = udtFormatter.formatChineseName(.ChineseName)
            lblConfirmHKID.Text = udtFormatter.formatHKID(.HKID, False)
            lblConfirmAddress.Text = formatAddress(.SpAddress.Room, .SpAddress.Floor, .SpAddress.Block, .SpAddress.Building, .SpAddress.District)
            lblConfirmEmail.Text = .Email
            lblConfirmContactNo.Text = .Phone
            If .Fax.Equals(String.Empty) Then
                lblConfirmFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblConfirmFax.Text = .Fax
            End If
        End With
    End Sub

    Private Sub LoadSelectedScheme()
        If udtPracSchemeInfoList Is Nothing Then Exit Sub
        If udtPracSchemeInfoList.Count = 0 Then Exit Sub

        Dim udtSchemeBO As SchemeBackOfficeModel = Nothing
        Dim udtSchemeList As SchemeBackOfficeModelCollection = udtSchemeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup()

        For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
            udtSchemeBO = udtSchemeList.Filter(udtScheme.SchemeCode.Trim)

            Dim newLi As New HtmlGenericControl
            newLi.ID = "li" & udtScheme.SchemeCode.Trim
            newLi.TagName = "li"
            If IsEnglish() Then
                newLi.InnerText = udtSchemeBO.SchemeDesc
            Else
                newLi.InnerText = udtSchemeBO.SchemeDescChi
            End If
            pnlConfirmScheme.Controls.Add(newLi)

        Next
    End Sub

    Private Sub LoadMO(ByVal udtSP As ServiceProviderModel)
        If udtSP.MOList Is Nothing Then Exit Sub
        If udtSP.MOList.Count = 0 Then Exit Sub

        Me.gvMO.Visible = True
        Me.gvMO.DataSource = udtSP.MOList.Values
        Me.gvMO.DataBind()
    End Sub

    Private Sub LoadPractice(ByVal udtSP As ServiceProviderModel)
        If udtSP.PracticeList Is Nothing Then Exit Sub
        If udtSP.PracticeList.Count = 0 Then Exit Sub

        Me.gvPractice.Visible = True
        Me.gvPractice.DataSource = udtSP.PracticeList.Values
        Me.gvPractice.DataBind()

        'udtSP.PracticeList(0).BankAcct.BankAcctOwner
    End Sub

    Private Sub LoadOtherSystem()
        With udtSP

            ' CRE15-018 Remove PPIePR Enrolment - Hide / Display PPIePR panel [Start][Winnie] 
            If .AlreadyJoinEHR.Equals(JoinEHRSSStatus.NA) Then
                panHadJoinEHRSS.Visible = False
                panEHRSS.Visible = False
            ElseIf .AlreadyJoinEHR.Equals(JoinEHRSSStatus.Yes) Then
                panHadJoinEHRSS.Visible = True
                panEHRSS.Visible = True
                lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "Yes")
            ElseIf .AlreadyJoinEHR.Equals(JoinEHRSSStatus.No) Then
                panHadJoinEHRSS.Visible = True
                panEHRSS.Visible = True
                lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "No")
            End If
            ' CRE15-018 Remove PPIePR Enrolment - Hide / Display PPIePR panel [End][Winnie]

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'If .ThirdPartyAdditionalFieldEnrolmentList.GetListBySysCode(ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD).Count > 0 Then
            '    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "Yes")
            'Else
            '    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No")
            'End If

            Select Case .JoinPCD
                Case JoinPCDStatus.Yes
                    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "Yes")
                Case JoinPCDStatus.Enrolled
                    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_JoinedPCD")
                Case JoinPCDStatus.No
                    lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_NotJoinPCD")
                Case JoinPCDStatus.NA
                    panPCD.Visible = False
            End Select
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
        End With

    End Sub

    Private Sub LoadTypeOfPractice(ByVal udtSP As ServiceProviderModel)
        panPCD.Visible = False
        If udtSP.ThirdPartyAdditionalFieldEnrolmentList Is Nothing Then Exit Sub
        If udtSP.ThirdPartyAdditionalFieldEnrolmentList.Count = 0 Then Exit Sub
        If udtSP.ThirdPartyAdditionalFieldEnrolmentList.GetListBySysCode(ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD).Count = 0 Then Exit Sub

        panPCD.Visible = True
        Me.ucTypeOfPracticeGrid.Mode = HCVU.ucTypeOfPracticeGrid.EnumMode.View

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Me.ucTypeOfPracticeGrid.LoadPractice(udtSP)
        Dim udtPracticeList As PracticeModelCollection = udtSP.PracticeList.FilterByPCD(TableLocation.Enrolment)
        Me.ucTypeOfPracticeGrid.LoadPractice(udtSP, udtPracticeList)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
    End Sub

    Private Sub gvMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMO.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblMOEmail As Label = CType(e.Row.FindControl("lblMOEmail"), Label)
            Dim lblMOFax As Label = CType(e.Row.FindControl("lblMOFax"), Label)
            Dim lblMOBRCode As Label = CType(e.Row.FindControl("lblMOBRCode"), Label)

            If lblMOEmail.Text.Trim.Equals(String.Empty) Then
                lblMOEmail.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblMOFax.Text.Trim.Equals(String.Empty) Then
                lblMOFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblMOBRCode.Text.Trim.Equals(String.Empty) Then
                lblMOBRCode.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If
        End If
    End Sub

    Private Sub gvPractice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPractice.RowDataBound
        Dim udtPrac As PracticeModel = Nothing
        Dim udtMO As MedicalOrganizationModel = Nothing
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblPracticeIndex As Label = CType(e.Row.FindControl("lblPracticeIndex"), Label)
            Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblPracticeMO"), Label)

            udtPrac = udtSP.PracticeList(CInt(lblPracticeIndex.Text))
            udtMO = udtSP.MOList(udtPrac.MODisplaySeq)

            lblPracticeMO.Text = udtMO.MOEngName



            Dim pnlServiceFee As Panel = e.Row.FindControl("pnlServiceFee")

            Dim lblBankName As Label = e.Row.FindControl("lblBankName")
            Dim lblBranchName As Label = e.Row.FindControl("lblBranchName")
            Dim lblBankAcc As Label = e.Row.FindControl("lblBankAcc")
            Dim lblBankOwner As Label = e.Row.FindControl("lblBankOwner")

            If udtPrac.BankAcct IsNot Nothing Then
                lblBankName.Text = udtPrac.BankAcct.BankName
                lblBranchName.Text = udtPrac.BankAcct.BranchName
                lblBankAcc.Text = udtPrac.BankAcct.BankAcctNo
                lblBankOwner.Text = udtPrac.BankAcct.BankAcctOwner
            Else
                lblBankName.Text = String.Empty
                lblBranchName.Text = String.Empty
                lblBankAcc.Text = String.Empty
                lblBankOwner.Text = String.Empty
            End If

            If lblBankName.Text.Trim.Equals(String.Empty) Then
                lblBankName.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblBranchName.Text.Trim.Equals(String.Empty) Then
                lblBranchName.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblBankAcc.Text.Trim.Equals(String.Empty) Then
                lblBankAcc.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            If lblBankOwner.Text.Trim.Equals(String.Empty) Then
                lblBankOwner.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If


            Dim udtSubsidzeGroupEFormList As New SubsidizeGroupEFormModelCollection

            If udtSchemeEFormBLL.ExistSession_SubsidizeGroupEForm Then
                udtSubsidzeGroupEFormList = udtSchemeEFormBLL.GetSession_SubsidizeGroupEForm
            End If

            Dim intPracticeIndex As Integer = CInt(lblPracticeIndex.Text.Trim) - 1

            Dim blnTable As Boolean = False
            Dim tbl As New Table



            ' Scheme to Enroll
            Dim pnlSchemeToEnroll As Panel = e.Row.FindControl("pnlSchemeToEnroll")
            Dim lstScheme As New List(Of String)
            Dim lstNonClinicScheme As New List(Of String)

            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPrac.PracticeSchemeInfoList.Values
                If lstScheme.Contains(udtPracticeSchemeInfo.SchemeCode) = False Then lstScheme.Add(udtPracticeSchemeInfo.SchemeCode)

                If udtPracticeSchemeInfo.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                    If lstNonClinicScheme.Contains(udtPracticeSchemeInfo.SchemeCode) = False Then lstNonClinicScheme.Add(udtPracticeSchemeInfo.SchemeCode)
                End If

            Next

            Dim udtSchemeEFList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetAllSchemeEForm

            For Each udtSchemeEF As SchemeEFormModel In udtSchemeEFList
                If lstScheme.Contains(udtSchemeEF.SchemeCode) Then
                    Dim li As New HtmlGenericControl
                    li.ID = "li" & udtSchemeEF.SchemeCode
                    li.TagName = "li"
                    li.Style.Add("margin-left", "15px")
                    li.InnerText = udtSchemeEF.SchemeDesc

                    pnlSchemeToEnroll.Controls.Add(li)

                    If lstNonClinicScheme.Contains(udtSchemeEF.SchemeCode) Then
                        Dim div As New HtmlGenericControl
                        div.ID = "div" & udtSchemeEF.SchemeCode
                        div.TagName = "div"
                        div.Style.Add("padding-left", "30px")

                        div.InnerText = String.Format("({0})", Me.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))

                        pnlSchemeToEnroll.Controls.Add(div)

                    End If

                End If

            Next

            ' Scheme Information
            Dim intIndex As Integer = 1

            For Each udtSchemeEF As SchemeEFormModel In udtSchemeEFList
                If lstScheme.Contains(udtSchemeEF.SchemeCode) AndAlso udtSchemeEF.ServiceFeeEnabled Then
                    ' Show the row
                    e.Row.FindControl("trSchemeInformation").Visible = True

                    ' Show the div
                    Dim div As Control = e.Row.FindControl(String.Format("divSchemeInformation{0}", intIndex))

                    If IsNothing(div) Then Exit For

                    div.Visible = True

                    ' Scheme
                    DirectCast(e.Row.FindControl(String.Format("lblSchemeInformation{0}", intIndex)), Label).Text = udtSchemeEF.SchemeDesc

                    ' Grid
                    Dim dt As New DataTable
                    dt.Columns.Add("Category", GetType(String))
                    dt.Columns.Add("Subsidy", GetType(String))
                    dt.Columns.Add("ServiceFee", GetType(String))

                    Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPrac.PracticeSchemeInfoList.Filter(udtSchemeEF.SchemeCode)

                    For Each udtSubsidizeGroupEF As SubsidizeGroupEFormModel In udtSchemeEFormBLL.GetAllSubsidizeGroupEForm
                        Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel = udtPracticeSchemeInfoList.Filter(udtSubsidizeGroupEF.SchemeCode, udtSubsidizeGroupEF.SubsidizeCode)

                        If Not IsNothing(udtPracticeSchemeInfo) AndAlso udtPracticeSchemeInfo.ProvideService Then
                            Dim dr As DataRow = dt.NewRow
                            dr("Category") = udtSubsidizeGroupEF.CategoryName
                            dr("Subsidy") = udtSubsidizeGroupEF.SubsidizeDisplayCode.Replace("#", String.Empty).Trim
                            dr("ServiceFee") = udtFormatter.formatMoney(udtPracticeSchemeInfo.ServiceFee.Value, True)
                            dt.Rows.Add(dr)

                        End If

                    Next

                    Dim gvSchemeInformation As GridView = e.Row.FindControl(String.Format("gvSchemeInformation{0}", intIndex))
                    gvSchemeInformation.DataSource = dt
                    gvSchemeInformation.DataBind()

                    intIndex += 1

                End If
            Next

        End If

    End Sub

    Protected Sub gvSchemeInformation_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvSchemeInformation As GridView = sender
        Dim strPreviousCategory As String = "--"
        Dim blnHaveCategory As Boolean = False

        For Each r1 As GridViewRow In gvSchemeInformation.Rows
            Dim strCategory As String = DirectCast(r1.FindControl("lblGCategory"), Label).Text

            If strCategory <> String.Empty Then blnHaveCategory = True

            If strCategory <> strPreviousCategory Then
                Dim intRowCount As Integer = 0

                For Each r2 As GridViewRow In gvSchemeInformation.Rows
                    If DirectCast(r2.FindControl("lblGCategory"), Label).Text = strCategory Then
                        intRowCount += 1
                    End If
                Next

                r1.Cells(0).RowSpan = intRowCount

            Else
                r1.Cells(0).Visible = False

            End If

            strPreviousCategory = strCategory

        Next

        If blnHaveCategory = False Then
            gvSchemeInformation.Columns(0).Visible = False
        End If

    End Sub


    '

    Private Function IsEnglish() As Boolean
        Return Not Thread.CurrentThread.CurrentUICulture.Name.ToUpper.Equals(TradChinese.ToUpper)
    End Function

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
        Dim strAreacode As String
        strAreacode = udtDistrictBLL.GetDistrictNameByDistrictCode(strDistrict).Area_ID
        Return udtFormatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
    End Function

    Protected Function formatChiAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
        Dim strAreacode As String
        strAreacode = udtDistrictBLL.GetDistrictNameByDistrictCode(strDistrict).Area_ID
        Return udtFormatter.formatAddressChi(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return udtFormatter.formatChineseName(strChineseString)
    End Function

    Protected Function formatBankAcct(ByVal strBankcode As String, ByVal strBranchCode As String, ByVal strBankAcct As String) As String
        Return udtFormatter.formatBankAcct(strBankcode, strBranchCode, strBankAcct)
    End Function

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If strPracticeCode.Equals(String.Empty) Then
            strPracticeTypeName = String.Empty
        Else
            If IsEnglish() Then
                strPracticeTypeName = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", strPracticeCode).DataValue
            Else
                strPracticeTypeName = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", strPracticeCode).DataValueChi
            End If
        End If

        Return strPracticeTypeName
    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        Dim strHealthProfName As String

        If strHealthProfCode.Equals(String.Empty) Then
            strHealthProfName = String.Empty
        Else
            If IsEnglish() Then
                strHealthProfName = ProfessionBLL.GetProfessionListByServiceCategoryCode(strHealthProfCode).ServiceCategoryDesc
            Else
                strHealthProfName = ProfessionBLL.GetProfessionListByServiceCategoryCode(strHealthProfCode).ServiceCategoryDescChi
            End If
        End If

        Return strHealthProfName
    End Function
End Class