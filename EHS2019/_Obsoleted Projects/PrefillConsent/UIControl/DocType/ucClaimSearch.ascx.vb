Imports Common.Component
Imports Common.Component.DocType

Partial Public Class ucClaimSearch
    Inherits System.Web.UI.UserControl

    'Objects 
    Dim commfunct As Common.ComFunction.GeneralFunction

    'Values
    Private _strDocType As String
    Private _strDocumentIdentityNo As String
    Private _strDOB As String

    Private _strECAge As String
    Private _strECDOADay As String
    Private _strECDOAMonth As String
    Private _strECDOAYear As String
    Private _blnECDOBSelected As Boolean

    Public Class ViewStateName
        Public Const DocType As String = "UCCLAIMSEARCH_DOCTYPE"
    End Class

    'Events 
    Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.commfunct = New Common.ComFunction.GeneralFunction
        Me.RenderLanguage(Me.DocType)
    End Sub

    Public Sub RenderLanguage(ByVal documentType As String)
        Dim strlanguage As String = Session("language").ToString()
        Me.DocType = documentType

        Me.lblSearchShortDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")

        Me.SetupShortIdentityNoSearch(True)

        Select Case documentType
            Case DocTypeModel.DocTypeCode.HKIC
                Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "HKID")
                Me.imgSearchShortIdentityTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HKICSampleImg")

            Case DocTypeModel.DocTypeCode.EC
                Me.lblECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                Me.lblHKIDECHint.Text = Me.GetGlobalResourceObject("Text", "HKICHint")
                Me.lblECDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBYOB")
                Me.lblECDOBOrText.Text = Me.GetGlobalResourceObject("Text", "Or")
                Me.lblDOBECHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint")

                If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.ECDOARenderLanguage(False)
                Else
                    Me.ECDOARenderLanguage(True)
                End If

            Case DocTypeModel.DocTypeCode.HKBC
                Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "RegistrationNo")
                Me.imgSearchShortIdentityTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HKBCSearchImg")

            Case DocTypeModel.DocTypeCode.REPMT
                Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintREPMT")
                Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "ReentryPermitNo")

            Case DocTypeModel.DocTypeCode.VISA
                Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintVISA")
                Me.lblSearchLongIdentityNo.Text = Me.GetGlobalResourceObject("Text", "VisaRefNo")

            Case DocTypeModel.DocTypeCode.ID235B
                Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintID235B")
                Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")

            Case DocTypeModel.DocTypeCode.DI
                Me.lblSearchLongDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI")
                'Me.lblSearchLongIdentityNo.Text = Me.GetGlobalResourceObject("Text", "TravelDocNo")
                Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "IdentityDocNo")

            Case DocTypeModel.DocTypeCode.ADOPC

                Me.lblSearchLongDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintADOPC")
                Me.lblSearchLongIdentityNo.Text = Me.GetGlobalResourceObject("Text", "NoOfEntry")

            Case String.Empty
                Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "IdentityDocNo")
                Me.SetupShortIdentityNoSearch(False)

        End Select

    End Sub

    Private Sub SetupShortIdentityNoSearch(ByVal enable As Boolean)
        Me.txtSearchShortIdentityNo.Enabled = enable
        Me.txtSearchShortDOB.Enabled = enable
        Me.btnShortIdentityNoSearch.Enabled = enable


        If enable Then
            Me.txtSearchShortIdentityNo.BackColor = Drawing.Color.White
            Me.lblSearchShortIdentityNo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#666666")

            Me.txtSearchShortDOB.BackColor = Drawing.Color.White
            Me.lblSearchShortDOB.ForeColor = System.Drawing.ColorTranslator.FromHtml("#666666")

            Me.btnShortIdentityNoSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")
        Else
            Me.txtSearchShortIdentityNo.BackColor = Drawing.Color.Silver
            Me.lblSearchShortIdentityNo.ForeColor = Drawing.Color.LightGray

            Me.txtSearchShortDOB.BackColor = Drawing.Color.Silver
            Me.lblSearchShortDOB.ForeColor = Drawing.Color.LightGray

            Me.btnShortIdentityNoSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")

            Me.txtSearchShortIdentityNo.Text = String.Empty
            Me.txtSearchShortDOB.Text = String.Empty
        End If

    End Sub

    Public Sub CleanField()

        Me.txtSearchShortIdentityNo.Text = String.Empty
        Me.txtSearchShortDOB.Text = String.Empty

        Me.txtSearchLongIdentityNo.Text = String.Empty
        Me.txtSearchLongDOB.Text = String.Empty

        Me.txtECDOAAge.Text = String.Empty
        Me.txtECDOADayChi.Text = String.Empty
        Me.txtECDOADayEn.Text = String.Empty
        Me.txtECDOAYearChi.Text = String.Empty
        Me.txtECDOAYearEn.Text = String.Empty
        Me.txtECDOB.Text = String.Empty
        Me.txtECHKID.Text = String.Empty

        Me.rbECDOB.Checked = True
        Me.rbECDOA.Checked = False
        Me.ddlECDOAMonth.SelectedIndex = -1

        Me.SetECError(False)
        Me.SetSearchLongError(False)
        Me.SetSearchShortError(False)
    End Sub

    Public Sub Build(ByVal documentType As String)
        ' Me.panSearchHKIC.Visible = False
        Me.panSearchEC.Visible = False
        Me.panSearchShortNo.Visible = False
        Me.panSearchLongNo.Visible = False

        Select Case documentType
            Case DocTypeModel.DocTypeCode.HKIC, _
                DocTypeModel.DocTypeCode.HKBC

                'tips
                Me.lblSearchShortIdentityNoTips.Visible = False
                Me.lblSearchShortDOBTips.Visible = False

                Me.panSearchShortNo.Visible = True
                Me.imgSearchShortIdentityTips.Visible = True

                Me.filteredSearchShortIdentityNo.ValidChars = "()"
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:formatHKID(this);"

            Case DocTypeModel.DocTypeCode.EC
                Me.panSearchEC.Visible = True
                Me.SwitchECSearchControl(Me.rbECDOB.Checked)

            Case DocTypeModel.DocTypeCode.REPMT, _
                DocTypeModel.DocTypeCode.DI, _
                DocTypeModel.DocTypeCode.ID235B

                'tips
                Me.lblSearchShortIdentityNoTips.Visible = True
                Me.lblSearchShortDOBTips.Visible = True

                Me.panSearchShortNo.Visible = True
                Me.imgSearchShortIdentityTips.Visible = False

                Me.filteredSearchShortIdentityNo.ValidChars = ""
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:UpperIndentityNo(this);"

            Case DocTypeModel.DocTypeCode.ADOPC
                'Setup input textbox
                Me.txtSearchLongIdentityNo.Width = 100

                'tips
                Me.lblSearchLongIdentityNoTips.Visible = True
                Me.lblSearchLongDOBTips.Visible = True

                Me.filteredSearchLongIdentityNo.ValidChars = "/"
                Me.panSearchLongNo.Visible = True

            Case DocTypeModel.DocTypeCode.VISA
                'Setup input textbox
                Me.txtSearchLongIdentityNo.Width = 140

                'tips
                Me.lblSearchLongIdentityNoTips.Visible = True
                Me.lblSearchLongDOBTips.Visible = True

                Me.filteredSearchLongIdentityNo.ValidChars = "-()"
                Me.txtSearchLongIdentityNo.Attributes("onChange") = "javascript:formatVISA(this);"
                Me.panSearchLongNo.Visible = True

            Case String.Empty
                Me.panSearchShortNo.Visible = True
                Me.imgSearchShortIdentityTips.Visible = False
                Me.lblSearchLongIdentityNoTips.Visible = False
                Me.lblSearchLongDOBTips.Visible = False
        End Select
        Me.RenderLanguage(documentType)
    End Sub

#Region "Set Up Error Image"

    ''HKIC Error-----------------------------------------------------------------------
    'Public Sub SetHKICHKIDError(ByVal isValid As Boolean)
    '    Me.ErrHKICHKID.Visible = isValid
    'End Sub

    'Public Sub SetHKICDOBError(ByVal isValid As Boolean)
    '    Me.ErrHKICDOB.Visible = isValid
    'End Sub

    'EC Error-----------------------------------------------------------------------
    Public Sub SetECError(ByVal visible As Boolean)
        Me.SetECHKIDError(visible)
        Me.SetECDOBError(visible)
        Me.SetECDOAError(visible)
        Me.SetECDOAAgeError(visible)
    End Sub

    Public Sub SetECHKIDError(ByVal visible As Boolean)
        Me.ErrECHKID.Visible = visible
    End Sub

    Public Sub SetECDOBError(ByVal visible As Boolean)
        Me.ErrECDOB.Visible = visible
    End Sub

    Public Sub SetECDOAError(ByVal visible As Boolean)
        Me.ErrECDOA.Visible = visible
    End Sub

    Public Sub SetECDOAAgeError(ByVal visible As Boolean)
        Me.ErrECDOAAge.Visible = visible
    End Sub

    'Search Short document Identity No Error-----------------------------------------------------------------------
    Public Sub SetSearchShortError(ByVal visible As Boolean)
        Me.SetSearchShortIdentityNoError(visible)
        Me.SetSearchShortDOBError(visible)
    End Sub

    Public Sub SetSearchShortIdentityNoError(ByVal visible As Boolean)
        Me.ErrSearchShortIdentityNo.Visible = visible
    End Sub

    Public Sub SetSearchShortDOBError(ByVal visible As Boolean)
        Me.ErrSearchShortDOB.Visible = visible
    End Sub

    'Search Long document Identity No Error-----------------------------------------------------------------------
    Public Sub SetSearchLongError(ByVal visible As Boolean)
        Me.SetSearchLongIdentityNoError(visible)
        Me.SetSearchLongDOBError(visible)
    End Sub

    Public Sub SetSearchLongIdentityNoError(ByVal visible As Boolean)
        Me.ErrSearchLongIdentityNo.Visible = visible
    End Sub

    Public Sub SetSearchLongDOBError(ByVal visible As Boolean)
        Me.ErrSearchLongDOB.Visible = visible
    End Sub
#End Region

#Region "Set Text Box Display Value"
    'Short document Identity No Display Value-----------------------------------------------------------------------
    Public Sub SetSearchShortIdentityNo(ByVal strIdentityNo As String)
        Me.txtSearchShortIdentityNo.Text = strIdentityNo
    End Sub

    Public Sub SetSearchShortDOB(ByVal strDOB As String)
        Me.txtSearchShortDOB.Text = strDOB
    End Sub

    'Long document Identity No Display Value-----------------------------------------------------------------------
    Public Sub SetSearchLongIdentityNo(ByVal strIdentityNo As String)
        Me.txtSearchLongIdentityNo.Text = strIdentityNo
    End Sub

    Public Sub SetSearchLongDOB(ByVal strDOB As String)
        Me.txtSearchLongDOB.Text = strDOB
    End Sub

#End Region

#Region "Property"

    Public Sub SetProperty(ByVal documentType As String)
        Select Case documentType
            Case DocTypeModel.DocTypeCode.HKIC, _
                DocTypeModel.DocTypeCode.HKBC, _
                DocTypeModel.DocTypeCode.REPMT, _
                DocTypeModel.DocTypeCode.ID235B, _
                DocTypeModel.DocTypeCode.DI

                Me._strDocumentIdentityNo = Me.txtSearchShortIdentityNo.Text
                Me._strDOB = Me.txtSearchShortDOB.Text

            Case DocTypeModel.DocTypeCode.ADOPC, _
                DocTypeModel.DocTypeCode.VISA

                Me._strDocumentIdentityNo = Me.txtSearchLongIdentityNo.Text
                Me._strDOB = Me.txtSearchLongDOB.Text

            Case DocTypeModel.DocTypeCode.EC
                Me._strDocumentIdentityNo = Me.txtECHKID.Text
                Me._blnECDOBSelected = Me.rbECDOB.Checked
                Me._strDOB = Me.txtECDOB.Text
                Me._strECAge = Me.txtECDOAAge.Text
                If Session("language").ToString().ToUpper.Equals("zh-tw".ToUpper()) Then
                    Me._strECDOADay = Me.txtECDOADayChi.Text
                    Me._strECDOAYear = Me.txtECDOAYearChi.Text
                Else
                    Me._strECDOADay = Me.txtECDOADayEn.Text
                    Me._strECDOAYear = Me.txtECDOAYearEn.Text
                End If
                Me._strECDOAMonth = Me.ddlECDOAMonth.SelectedValue

        End Select
    End Sub

    Public ReadOnly Property IdentityNo() As String
        Get
            Return Me._strDocumentIdentityNo.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty).Replace("/", String.Empty)
        End Get
    End Property

    Public ReadOnly Property DOB() As String
        Get
            Return Me._strDOB
        End Get
    End Property

    'EC Case-------------------------------------------------------------------
    Public ReadOnly Property ECAge() As String
        Get
            Return Me._strECAge
        End Get
    End Property

    Public ReadOnly Property ECDOADay() As String
        Get
            Return Me._strECDOADay
        End Get
    End Property

    Public ReadOnly Property ECDOAMonth() As String
        Get
            Return Me._strECDOAMonth
        End Get
    End Property

    Public ReadOnly Property ECDOAYear() As String
        Get
            Return Me._strECDOAYear
        End Get
    End Property

    Public ReadOnly Property ECDOBSelected() As String
        Get
            Return Me._blnECDOBSelected
        End Get
    End Property

    Public Property DocType() As String
        Get
            Me._strDocType = Me.ViewState(ViewStateName.DocType)
            Return Me._strDocType
        End Get
        Set(ByVal value As String)
            Me.ViewState(ViewStateName.DocType) = value
        End Set
    End Property

    ''BC Case-------------------------------------------------------------------
    'Public ReadOnly Property BCRegistration() As String
    '    Get
    '        Return Me._strRegistrationNo
    '    End Get
    'End Property

    ''HKSAR Passport-------------------------------------------------------------------
    'Public ReadOnly Property HKSARPassportNo() As String
    '    Get
    '        Return Me._strHKSARPassortNo
    '    End Get
    'End Property
#End Region

#Region "Events"
    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _
        btnECSearch.Click, btnLongIdentityNoSearch.Click, btnShortIdentityNoSearch.Click 'btnHKICSearch.Click,
        RaiseEvent SearchButtonClick(sender, e)
    End Sub

    Protected Sub rbECDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbECDOB.CheckedChanged, rbECDOA.CheckedChanged
        Me.SwitchECSearchControl(Me.rbECDOB.Checked)
    End Sub
#End Region


#Region "EC Search Setup"
    Private Sub ECDOARenderLanguage(ByVal blnIsEnglish As Boolean)
        Me.rbECDOA.Text = Me.GetGlobalResourceObject("Text", "Age")
        Me.lblECDOAOnText.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")
        'DOA TextBoxs
        Me.txtECDOADayChi.Visible = Not blnIsEnglish
        Me.txtECDOAYearChi.Visible = Not blnIsEnglish
        Me.txtECDOADayEn.Visible = blnIsEnglish
        Me.txtECDOAYearEn.Visible = blnIsEnglish

        'DOA Labels
        Me.lblECDOAYearChiText.Visible = Not blnIsEnglish
        Me.lblECDOAMonthChiText.Visible = Not blnIsEnglish
        Me.lblECDOADayChiText.Visible = Not blnIsEnglish

        Me.lblECDOAYearEnText.Visible = blnIsEnglish
        Me.lblECDOAMonthEnText.Visible = blnIsEnglish
        Me.lblECDOADayEnText.Visible = blnIsEnglish

        If blnIsEnglish Then
            Me.lblECDOAYearEnText.Text = Me.GetGlobalResourceObject("Text", "Year")
            Me.lblECDOADayEnText.Text = Me.GetGlobalResourceObject("Text", "Day")
            Me.lblECDOAMonthEnText.Text = Me.GetGlobalResourceObject("Text", "Month")

            If Me.txtECDOAYearChi.Text <> String.Empty Then
                Me.txtECDOAYearEn.Text = Me.txtECDOAYearChi.Text
            End If

            If Me.txtECDOADayChi.Text <> String.Empty Then
                Me.txtECDOADayEn.Text = Me.txtECDOADayChi.Text
            End If

            Me.BindECDate(Me.ddlECDOAMonth, Common.Component.CultureLanguage.English)
        Else
            Me.lblECDOAYearChiText.Text = Me.GetGlobalResourceObject("Text", "Year")
            Me.lblECDOADayChiText.Text = Me.GetGlobalResourceObject("Text", "Day")
            Me.lblECDOAMonthChiText.Text = Me.GetGlobalResourceObject("Text", "Month")

            If Me.txtECDOAYearEn.Text <> String.Empty Then
                Me.txtECDOAYearChi.Text = Me.txtECDOAYearEn.Text
            End If

            If Me.txtECDOADayEn.Text <> String.Empty Then
                Me.txtECDOADayChi.Text = Me.txtECDOADayEn.Text
            End If

            Me.BindECDate(Me.ddlECDOAMonth, Common.Component.CultureLanguage.TradChinese)
        End If
    End Sub

    Private Sub BindECDate(ByVal ddlECDate As DropDownList, ByVal strLanguage As String)
        Dim strECDateMonth As String
        Me.commfunct = New Common.ComFunction.GeneralFunction

        strECDateMonth = ddlECDate.SelectedValue()
        ddlECDate.DataSource = Me.commfunct.GetMonthSelection(strLanguage)
        ddlECDate.DataValueField = "Value"
        ddlECDate.DataTextField = "Display"
        ddlECDate.DataBind()

        If Not strECDateMonth Is Nothing Then
            ddlECDate.SelectedValue = strECDateMonth
        End If
    End Sub

    Private Sub SwitchECSearchControl(ByVal blnSearchByDOB As Boolean)
        Me.lblDOBECHint.Visible = blnSearchByDOB
        Me.txtECDOB.Enabled = blnSearchByDOB

        Me.lblDOAECHint.Visible = Not blnSearchByDOB
        Me.txtECDOAAge.Enabled = Not blnSearchByDOB
        Me.txtECDOADayEn.Enabled = Not blnSearchByDOB
        Me.txtECDOAYearEn.Enabled = Not blnSearchByDOB
        Me.ddlECDOAMonth.Enabled = Not blnSearchByDOB
        Me.txtECDOAYearChi.Enabled = Not blnSearchByDOB
        Me.txtECDOADayChi.Enabled = Not blnSearchByDOB

        If blnSearchByDOB Then
            Me.txtECDOAAge.BackColor = Drawing.Color.Silver
            Me.txtECDOADayEn.BackColor = Drawing.Color.Silver
            Me.txtECDOAYearEn.BackColor = Drawing.Color.Silver
            Me.ddlECDOAMonth.BackColor = Drawing.Color.Silver
            Me.txtECDOAYearChi.BackColor = Drawing.Color.Silver
            Me.txtECDOADayChi.BackColor = Drawing.Color.Silver

            Me.txtECDOB.BackColor = Drawing.Color.White
        Else
            Me.txtECDOAAge.BackColor = Drawing.Color.White
            Me.txtECDOADayEn.BackColor = Drawing.Color.White
            Me.txtECDOAYearEn.BackColor = Drawing.Color.White
            Me.ddlECDOAMonth.BackColor = Drawing.Color.White
            Me.txtECDOAYearChi.BackColor = Drawing.Color.White
            Me.txtECDOADayChi.BackColor = Drawing.Color.White

            Me.txtECDOB.BackColor = Drawing.Color.Silver
        End If

        'Me.txtECDOB.Text = String.Empty
        'Me.txtECDOAAge.Text = String.Empty
        'Me.txtECDOADayEn.Text = String.Empty
        'Me.txtECDOAYearEn.Text = String.Empty
        'Me.txtECDOAYearChi.Text = String.Empty
        'Me.txtECDOADayChi.Text = String.Empty
        'Me.ddlECDOAMonth.SelectedIndex = -1

        Me.SetECDOAAgeError(False)
        Me.SetECDOAError(False)
        Me.SetECDOBError(False)
    End Sub
#End Region
End Class