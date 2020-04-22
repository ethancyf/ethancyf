Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.DocType
Imports System.Web.Security.AntiXss

Namespace UIControl.DocTypeText

    Partial Public Class ucClaimSearch
        Inherits System.Web.UI.UserControl

        'Objects 
        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        'Values
        Private _strDocType As String
        Private _strDocumentIdentityNo As String
        Private _strDOB As String
        Private _strDocumentIdentityNoPrefix As String

        Private _strECAge As String
        Private _strECDOADay As String
        Private _strECDOAMonth As String
        Private _strECDOAYear As String
        Private _blnECDOBSelected As Boolean

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private _blnEnabledHKICSymbol As Boolean = False
        Private _blnDisplayHKICSymbol As Boolean = False
        Private _strHKICSymbol As String = String.Empty
        Private _strSchemeCode As String = String.Empty
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL


        Public Class ViewStateName
            Public Const DocType As String = "UCCLAIMSEARCH_DOCTYPE"
        End Class

        'Events 
        Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Public Event ShowInputTipsClick(ByVal type As ucInputTips.InputTipsType)
        Public Event HKICSymbolListClick(ByVal sender As System.Object, ByVal e As EventArgs)

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.RenderLanguage(Me.DocType)
        End Sub

        Public Sub RenderLanguage(ByVal documentType As String)
            Dim strlanguage As String = Session("language").ToString()
            Me.DocType = documentType

            Me.lblSearchShortDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")

            Me.SetupShortIdentityNoSearch(True)

            Dim udtDocTypeList As DocTypeModelCollection
            udtDocTypeList = udtDocTypeBLL.getAllDocType

            Select Case documentType
                Case DocTypeModel.DocTypeCode.HKIC
                    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblSearchHKICIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKIC).DocIdentityDescChi
                    Else
                        Me.lblSearchHKICIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKIC).DocIdentityDesc
                    End If
                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                Case DocTypeModel.DocTypeCode.EC
                    'Me.lblECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblECHKIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.EC).DocIdentityDescChi
                    Else
                        Me.lblECHKIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.EC).DocIdentityDesc
                    End If

                    Me.lblECDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBYOB")
                    Me.lblECDOBOrText.Text = Me.GetGlobalResourceObject("Text", "Or")

                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.ECDOARenderLanguage(False)
                    Else
                        Me.ECDOARenderLanguage(True)
                    End If

                Case DocTypeModel.DocTypeCode.HKBC
                    ' Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "BCRegNo")
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKBC).DocIdentityDescChi
                    Else
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKBC).DocIdentityDesc
                    End If

                Case DocTypeModel.DocTypeCode.REPMT
                    'Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "ReentryPermitNo")
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.REPMT).DocIdentityDescChi
                    Else
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.REPMT).DocIdentityDesc
                    End If

                Case DocTypeModel.DocTypeCode.VISA
                    'Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "VisaRefNo")
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.VISA).DocIdentityDescChi
                    Else
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.VISA).DocIdentityDesc
                    End If

                Case DocTypeModel.DocTypeCode.ID235B
                    'Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "BirthEntryNo")
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ID235B).DocIdentityDescChi
                    Else
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ID235B).DocIdentityDesc
                    End If


                Case DocTypeModel.DocTypeCode.ADOPC
                    'Me.lblADOPCIdentityNoText.Text = Me.GetGlobalResourceObject("Text", "NoOfEntry")
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblADOPCIdentityNoText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ADOPC).DocIdentityDescChi
                    Else
                        Me.lblADOPCIdentityNoText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ADOPC).DocIdentityDesc
                    End If

                Case DocTypeModel.DocTypeCode.DI
                    'Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "IdentityDocNo")
                    If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.DI).DocIdentityDescChi
                    Else
                        Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.DI).DocIdentityDesc
                    End If

                Case String.Empty
                    Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "IdentityDocNo")
                    Me.SetupShortIdentityNoSearch(False)

            End Select

        End Sub

        Private Sub SetupShortIdentityNoSearch(ByVal enable As Boolean)
            Me.txtSearchShortIdentityNo.Enabled = enable
            Me.txtSearchShortDOB.Enabled = enable

            If enable Then
                'Me.txtSearchShortIdentityNo.BackColor = Drawing.Color.White
                'Me.lblSearchShortIdentityNo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#666666")
                'Me.txtSearchShortDOB.BackColor = Drawing.Color.White
                'Me.lblSearchShortDOB.ForeColor = System.Drawing.ColorTranslator.FromHtml("#666666")

                Me.txtSearchShortIdentityNo.BackColor = System.Drawing.Color.Empty
                Me.lblSearchShortIdentityNo.ForeColor = System.Drawing.Color.Empty

                Me.txtSearchShortDOB.BackColor = System.Drawing.Color.Empty
                Me.lblSearchShortDOB.ForeColor = System.Drawing.Color.Empty

            Else
                Me.txtSearchShortIdentityNo.BackColor = Drawing.Color.Silver
                Me.lblSearchShortIdentityNo.ForeColor = Drawing.Color.LightGray

                Me.txtSearchShortDOB.BackColor = Drawing.Color.Silver
                Me.lblSearchShortDOB.ForeColor = Drawing.Color.LightGray

                Me.txtSearchShortIdentityNo.Text = String.Empty
                Me.txtSearchShortDOB.Text = String.Empty
            End If

        End Sub

        Public Sub CleanField()

            Me.txtSearchShortIdentityNo.Text = String.Empty
            Me.txtSearchShortDOB.Text = String.Empty

            Me.txtADOPCIdentityNo.Text = String.Empty
            Me.txtADOPCIdentityNoPrefix.Text = String.Empty
            Me.txtADOPCDOB.Text = String.Empty

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

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.SetHKICError(False)
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            Me.SetECError(False)
            Me.SetADOPCError(False)
            Me.SetSearchShortError(False)
        End Sub

        Public Sub Build(ByVal strDocumentType As String)
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Me.panSearchEC.Visible = False
            Me.panSearchHKIC.Visible = False
            Me.panSearchShortNo.Visible = False
            Me.panSearchADOPC.Visible = False

            Select Case strDocumentType
                Case DocTypeModel.DocTypeCode.HKIC
                    ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'Me.rblHKICSymbol.Width = 150
                    ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
                    Me.txtSearchHKICIdentityNo.Width = 85
                    Me.txtSearchHKICIdentityNo.MaxLength = 11

                    Me.panSearchHKIC.Visible = True
                    EnableHKICSymbolRadioButtonList(_blnEnabledHKICSymbol)

                Case DocTypeModel.DocTypeCode.HKBC
                    Me.txtSearchShortIdentityNo.Width = 85
                    Me.txtSearchShortIdentityNo.MaxLength = 11

                    Me.panSearchShortNo.Visible = True

                Case DocTypeModel.DocTypeCode.EC
                    Me.panSearchEC.Visible = True

                Case DocTypeModel.DocTypeCode.REPMT, _
                    DocTypeModel.DocTypeCode.DI

                    Me.txtSearchShortIdentityNo.Width = 85
                    Me.txtSearchShortIdentityNo.MaxLength = 9

                    Me.panSearchShortNo.Visible = True

                Case DocTypeModel.DocTypeCode.ID235B
                    Me.txtSearchShortIdentityNo.Width = 85
                    Me.txtSearchShortIdentityNo.MaxLength = 8

                    Me.panSearchShortNo.Visible = True

                Case DocTypeModel.DocTypeCode.VISA
                    'Setup input textbox
                    Me.txtSearchShortIdentityNo.Width = 140
                    Me.txtSearchShortIdentityNo.MaxLength = 18

                    Me.panSearchShortNo.Visible = True


                Case DocTypeModel.DocTypeCode.ADOPC
                    Me.panSearchADOPC.Visible = True

                Case String.Empty
                    Me.panSearchShortNo.Visible = True

            End Select

            Me.RenderLanguage(strDocumentType)

            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        End Sub

#Region "Set Up Error Image"
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        'HKIC Error-----------------------------------------------------------------------
        Public Sub SetHKICError(ByVal blnVisable As Boolean)
            Me.SetResidentialError(blnVisable)
            Me.SetHKICNoError(blnVisable)
            Me.SetHKICDOBError(blnVisable)
        End Sub
        Public Sub SetResidentialError(ByVal blnVisable As Boolean)
            Me.ErrHKICSymbol.Visible = blnVisable
        End Sub
        Public Sub SetHKICNoError(ByVal blnVisable As Boolean)
            Me.ErrSearchHKICIdentityNo.Visible = blnVisable
        End Sub

        Public Sub SetHKICDOBError(ByVal blnVisable As Boolean)
            Me.ErrSearchHKICDOB.Visible = blnVisable
        End Sub

        'EC Error-----------------------------------------------------------------------
        Public Sub SetECError(ByVal blnVisable As Boolean)
            Me.SetECHKIDError(blnVisable)
            Me.SetECDOBError(blnVisable)
            Me.SetECDOAError(blnVisable)
            Me.SetECDOAAgeError(blnVisable)
        End Sub

        Public Sub SetECHKIDError(ByVal blnVisable As Boolean)
            Me.ErrECHKID.Visible = blnVisable
        End Sub

        Public Sub SetECDOBError(ByVal blnVisable As Boolean)
            Me.ErrECDOB.Visible = blnVisable
        End Sub

        Public Sub SetECDOAError(ByVal blnVisable As Boolean)
            Me.ErrECDOA.Visible = blnVisable
        End Sub

        Public Sub SetECDOAAgeError(ByVal blnVisable As Boolean)
            Me.ErrECDOAAge.Visible = blnVisable
        End Sub

        'Search Short document Identity No Error-----------------------------------------------------------------------
        Public Sub SetSearchShortError(ByVal blnVisable As Boolean)
            Me.SetSearchShortIdentityNoError(blnVisable)
            Me.SetSearchShortDOBError(blnVisable)
        End Sub

        Public Sub SetSearchShortIdentityNoError(ByVal blnVisable As Boolean)
            Me.ErrSearchShortIdentityNo.Visible = blnVisable
        End Sub

        Public Sub SetSearchShortDOBError(ByVal blnVisable As Boolean)
            Me.ErrSearchShortDOB.Visible = blnVisable
        End Sub

        'Document Identity Error-----------------------------------------------------------------------
        Public Sub SetADOPCError(ByVal blnVisable As Boolean)
            Me.SetADOPCIdentityNoError(blnVisable)
            Me.SetADOPCDOBError(blnVisable)
        End Sub

        Public Sub SetADOPCIdentityNoError(ByVal blnVisable As Boolean)
            Me.ErrADOPCIdentityNo.Visible = blnVisable
        End Sub

        Public Sub SetADOPCDOBError(ByVal blnVisable As Boolean)
            Me.ErrADOPCDOB.Visible = blnVisable
        End Sub

        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
#End Region

#Region "Reset"
        Public Sub ResetEC()
            Me.txtECHKID.Text = String.Empty
            Me.txtECDOB.Text = String.Empty
            Me.txtECDOAAge.Text = String.Empty
            Me.txtECDOADayEn.Text = String.Empty
            Me.txtECDOADayChi.Text = String.Empty
            Me.ddlECDOAMonth.SelectedIndex = -1
            Me.txtECDOAYearEn.Text = String.Empty
            Me.txtECDOAYearChi.Text = String.Empty
            Me.rbECDOB.Checked = True
            Me.rbECDOA.Checked = False

            SetECError(False)
        End Sub

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub ResetSearchShort()
            Me.txtSearchShortIdentityNo.Text = String.Empty
            Me.txtSearchShortDOB.Text = String.Empty
        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Public Sub ResetADOPC()
            Me.txtADOPCIdentityNoPrefix.Text = String.Empty
            Me.txtADOPCIdentityNo.Text = String.Empty
            Me.txtADOPCDOB.Text = String.Empty
        End Sub

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub ResetHKIC()
            Me.rblHKICSymbol.SelectedValue = Nothing
            Me.rblHKICSymbol.SelectedIndex = -1
            Me.txtSearchHKICIdentityNo.Text = String.Empty
            Me.txtSearchHKICDOB.Text = String.Empty
        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
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
        Public Sub SetDIIdentityNo(ByVal strIdentityNoPrefix As String, ByVal strIdentityNo As String)
            Me.txtADOPCIdentityNoPrefix.Text = strIdentityNoPrefix
            Me.txtADOPCIdentityNo.Text = strIdentityNo
        End Sub

        Public Sub SetDIDOB(ByVal strDOB As String)
            Me.txtADOPCDOB.Text = strDOB
        End Sub

#End Region

#Region "Property"

        Public Sub SetProperty(ByVal documentType As String)
            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            Select Case documentType
                Case DocTypeModel.DocTypeCode.HKIC
                    Me._strHKICSymbol = Me.rblHKICSymbol.SelectedValue
                    Me._strDocumentIdentityNo = Me.txtSearchHKICIdentityNo.Text
                    Me._strDOB = Me.txtSearchHKICDOB.Text

                Case DocTypeModel.DocTypeCode.HKBC, _
                    DocTypeModel.DocTypeCode.REPMT, _
                    DocTypeModel.DocTypeCode.ID235B, _
                    DocTypeModel.DocTypeCode.DI

                    Me._strDocumentIdentityNo = Me.txtSearchShortIdentityNo.Text
                    Me._strDOB = Me.txtSearchShortDOB.Text

                Case DocTypeModel.DocTypeCode.ADOPC
                    Me._strDocumentIdentityNo = Me.txtADOPCIdentityNo.Text
                    Me._strDocumentIdentityNoPrefix = Me.txtADOPCIdentityNoPrefix.Text
                    Me._strDOB = Me.txtADOPCDOB.Text

                Case DocTypeModel.DocTypeCode.VISA
                    Me._strDocumentIdentityNo = Me.txtSearchShortIdentityNo.Text
                    Me._strDOB = Me.txtSearchShortDOB.Text

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
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        End Sub

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(value As String)
                _strSchemeCode = value
            End Set
        End Property
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property UIEnableHKICSymbol() As Boolean
            Get
                Return Me._blnEnabledHKICSymbol
            End Get
            Set(value As Boolean)
                _blnEnabledHKICSymbol = value
            End Set
        End Property
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property UIDisplayHKICSymbol() As Boolean
            Get
                Return Me._blnDisplayHKICSymbol
            End Get
        End Property
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property HKICSymbol() As String
            Get
                If String.IsNullOrEmpty(Me._strHKICSymbol) Then
                    Return String.Empty
                Else
                    Return Me._strHKICSymbol
                End If
            End Get
        End Property
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public ReadOnly Property HKICSymbolSelectedValue() As String
            Get
                Return Me.rblHKICSymbol.SelectedValue
            End Get
        End Property
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]


        Public ReadOnly Property IdentityNo() As String
            Get
                If String.IsNullOrEmpty(Me._strDocumentIdentityNo) Then
                    Return String.Empty
                Else
                    Return Me._strDocumentIdentityNo.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty).Replace("/", String.Empty).Replace(" ", "")
                End If
            End Get
        End Property

        Public ReadOnly Property IdentityNoPrefix() As String
            Get
                If String.IsNullOrEmpty(Me._strDocumentIdentityNoPrefix) Then
                    Return String.Empty
                Else
                    Return Me._strDocumentIdentityNoPrefix.Trim()
                End If
            End Get
        End Property

        Public ReadOnly Property DOB() As String
            Get
                If String.IsNullOrEmpty(Me._strDOB) Then
                    Return String.Empty
                Else
                    Return Me._strDOB.Trim()
                End If
            End Get
        End Property

        'EC Case-------------------------------------------------------------------
        Public ReadOnly Property ECAge() As String
            Get
                If String.IsNullOrEmpty(Me._strECAge) Then
                    Return String.Empty
                Else
                    Return Me._strECAge.Trim()
                End If
            End Get
        End Property

        Public ReadOnly Property ECDOADay() As String
            Get
                If String.IsNullOrEmpty(Me._strECDOADay) Then
                    Return String.Empty
                Else
                    Return Me._strECDOADay.Trim()
                End If
            End Get
        End Property

        Public ReadOnly Property ECDOAMonth() As String
            Get
                If String.IsNullOrEmpty(Me._strECDOAMonth) Then
                    Return String.Empty
                Else
                    Return Me._strECDOAMonth.Trim()
                End If
            End Get
        End Property

        Public ReadOnly Property ECDOAYear() As String
            Get
                If String.IsNullOrEmpty(Me._strECDOAYear) Then
                    Return String.Empty
                Else
                    Return Me._strECDOAYear.Trim()
                End If
            End Get
        End Property

        Public ReadOnly Property ECDOBSelected() As Boolean
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

#End Region

#Region "Events"
        'Protected Sub rbECDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbECDOB.CheckedChanged, rbECDOA.CheckedChanged
        '    Me.SwitchECSearchControl(Me.rbECDOB.Checked)
        'End Sub

        ' Input Tip Link Button Eevent
        Protected Sub lnkECTips_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkECDocIDTips.Click
            RaiseEvent ShowInputTipsClick(IIf(rbECDOB.Checked, ucInputTips.InputTipsType.SearchECDOB, ucInputTips.InputTipsType.SearchECYOB))
        End Sub

        Protected Sub lnkSearchDocIDTips_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkSearchDocIDTips.Click, lnkSearchHKICTips.Click, lnkSearchHKICwithSymbolTips.Click
            Select Case Me._strDocType.Trim()
                Case DocTypeModel.DocTypeCode.HKIC.Trim()
                    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    If panHKICSymbol.Visible Then
                        RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchHKICwithSymbol)
                    Else
                        RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchHKIC)
                    End If
                    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Case DocTypeModel.DocTypeCode.DI
                    RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchDI)
                Case DocTypeModel.DocTypeCode.HKBC
                    RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchHKBC)
                Case DocTypeModel.DocTypeCode.REPMT
                    RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchReentry)
                Case DocTypeModel.DocTypeCode.ID235B
                    RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchID235B)
                Case DocTypeModel.DocTypeCode.VISA
                    RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchVisa)
            End Select
        End Sub

        Protected Sub lblADOPCTips_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lblADOPCDocIDTips.Click
            RaiseEvent ShowInputTipsClick(ucInputTips.InputTipsType.SearchAdoption)
        End Sub

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub rblHKICSymbol_DataBound(sender As Object, e As EventArgs) Handles rblHKICSymbol.DataBound
            Dim rbl As RadioButtonList = CType(sender, RadioButtonList)

            For idx As Integer = 0 To rbl.Items.Count - 1
                rbl.Items(idx).Value = rbl.Items(idx).Value.Trim
            Next

        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub rblHKICSymbol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblHKICSymbol.SelectedIndexChanged
            Me._strHKICSymbol = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rblHKICSymbol.UniqueID), True)

            RaiseEvent HKICSymbolListClick(sender, e)
        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
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
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOAYearEn.Text = AntiXssEncoder.HtmlEncode(txtECDOAYearChi.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOAYearChi.Text = String.Empty
                End If

                If Me.txtECDOADayChi.Text <> String.Empty Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOADayEn.Text = AntiXssEncoder.HtmlEncode(txtECDOADayChi.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOADayChi.Text = String.Empty
                End If

                Me.BindECDate(Me.ddlECDOAMonth, Common.Component.CultureLanguage.English)
            Else
                Me.lblECDOAYearChiText.Text = Me.GetGlobalResourceObject("Text", "Year")
                Me.lblECDOADayChiText.Text = Me.GetGlobalResourceObject("Text", "Day")
                Me.lblECDOAMonthChiText.Text = Me.GetGlobalResourceObject("Text", "Month")

                If Me.txtECDOAYearEn.Text <> String.Empty Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOAYearChi.Text = AntiXssEncoder.HtmlEncode(txtECDOAYearEn.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOAYearEn.Text = String.Empty
                End If

                If Me.txtECDOADayEn.Text <> String.Empty Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOADayChi.Text = AntiXssEncoder.HtmlEncode(txtECDOADayEn.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOADayEn.Text = String.Empty
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

        'Private Sub SwitchECSearchControl(ByVal blnSearchByDOB As Boolean)
        '    Me.txtECDOB.Enabled = blnSearchByDOB
        '    Me.txtECDOAAge.Enabled = Not blnSearchByDOB
        '    Me.txtECDOADayEn.Enabled = Not blnSearchByDOB
        '    Me.txtECDOAYearEn.Enabled = Not blnSearchByDOB
        '    Me.ddlECDOAMonth.Enabled = Not blnSearchByDOB
        '    Me.txtECDOAYearChi.Enabled = Not blnSearchByDOB
        '    Me.txtECDOADayChi.Enabled = Not blnSearchByDOB
        '    If blnSearchByDOB Then
        '        Me.txtECDOAAge.BackColor = Drawing.Color.Silver
        '        Me.txtECDOADayEn.BackColor = Drawing.Color.Silver
        '        Me.txtECDOAYearEn.BackColor = Drawing.Color.Silver
        '        Me.ddlECDOAMonth.BackColor = Drawing.Color.Silver
        '        Me.txtECDOAYearChi.BackColor = Drawing.Color.Silver
        '        Me.txtECDOADayChi.BackColor = Drawing.Color.Silver
        '        Me.txtECDOB.BackColor = Drawing.Color.White
        '        Me.SetECDOAAgeError(False)
        '        Me.SetECDOAError(False)
        '    Else
        '        Me.txtECDOAAge.BackColor = Drawing.Color.White
        '        Me.txtECDOADayEn.BackColor = Drawing.Color.White
        '        Me.txtECDOAYearEn.BackColor = Drawing.Color.White
        '        Me.ddlECDOAMonth.BackColor = Drawing.Color.White
        '        Me.txtECDOAYearChi.BackColor = Drawing.Color.White
        '        Me.txtECDOADayChi.BackColor = Drawing.Color.White
        '        Me.txtECDOB.BackColor = Drawing.Color.Silver
        '        Me.SetECDOBError(False)
        '    End If
        'End Sub
#End Region

#Region "Functions"

        Public Function IsEmpty(ByVal documentType As String) As Boolean
            Select Case documentType
                Case DocTypeModel.DocTypeCode.HKIC, _
                    DocTypeModel.DocTypeCode.HKBC, _
                    DocTypeModel.DocTypeCode.REPMT, _
                    DocTypeModel.DocTypeCode.DI, _
                    DocTypeModel.DocTypeCode.ID235B, _
                    DocTypeModel.DocTypeCode.VISA


                    If Not Me.txtSearchShortIdentityNo.Text.Trim().Equals(String.Empty) OrElse _
                       Not Me.txtSearchShortDOB.Text.Trim().Equals(String.Empty) Then
                        Return True
                    End If

                Case DocTypeModel.DocTypeCode.EC
                    If Not Me.txtECDOAAge.Text.Trim().Equals(String.Empty) OrElse _
                       Not Me.txtECDOADayChi.Text.Trim().Equals(String.Empty) OrElse _
                        Not Me.txtECDOADayEn.Text.Trim().Equals(String.Empty) OrElse _
                        Not Me.txtECDOAYearChi.Text.Trim().Equals(String.Empty) OrElse _
                       Not Me.txtECDOAYearEn.Text.Trim().Equals(String.Empty) OrElse _
                       Not Me.txtECDOB.Text.Trim().Equals(String.Empty) OrElse _
                       Not Me.txtECHKID.Text.Trim().Equals(String.Empty) OrElse _
                       Not Me.ddlECDOAMonth.SelectedIndex.Equals(0) Then
                        Return True
                    End If

                Case DocTypeModel.DocTypeCode.ADOPC

                    If Not Me.txtADOPCIdentityNo.Text.Trim().Equals(String.Empty) OrElse _
                        Not Me.txtADOPCIdentityNoPrefix.Text.Trim().Equals(String.Empty) OrElse _
                        Not Me.txtADOPCDOB.Text.Trim().Equals(String.Empty) Then
                        Return True
                    End If

                Case String.Empty

            End Select

            Return False
        End Function

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub EnableHKICSymbolRadioButtonList(ByVal blnShow As Boolean)
            If blnShow Then
                ' Check system parameters to see if enable
                If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(_strSchemeCode) Then
                    Dim strSelectedValue As String = String.Empty

                    'Store selected value into temp variable
                    If rblHKICSymbol.SelectedValue <> String.Empty Then
                        strSelectedValue = rblHKICSymbol.SelectedValue
                    End If

                    'Clear radio button list
                    rblHKICSymbol.Items.Clear()
                    ClearHKICSymbolSelection()

                    'Reload radio button list
                    rblHKICSymbol.DataSource = Status.GetDescriptionListFromDBEnumCode("HKICSymbol")

                    Select Case Session("language")
                        Case CultureLanguage.English
                            rblHKICSymbol.DataTextField = "Status_Description"
                        Case CultureLanguage.TradChinese
                            rblHKICSymbol.DataTextField = "Status_Description_Chi"
                        Case CultureLanguage.SimpChinese
                            rblHKICSymbol.DataTextField = "Status_Description_CN"
                        Case Else
                            rblHKICSymbol.DataTextField = "Status_Description"
                    End Select

                    rblHKICSymbol.DataValueField = "Status_Value"
                    rblHKICSymbol.DataBind()

                    'Restore selected value from temp variable
                    If strSelectedValue <> String.Empty Then
                        rblHKICSymbol.SelectedValue = strSelectedValue
                    End If

                    _blnDisplayHKICSymbol = True
                    panHKICSymbol.Visible = True
                    lnkSearchHKICwithSymbolTips.Visible = True
                    lnkSearchHKICTips.Visible = False

                Else
                    _blnDisplayHKICSymbol = False
                    panHKICSymbol.Visible = False
                    lnkSearchHKICwithSymbolTips.Visible = False
                    lnkSearchHKICTips.Visible = True

                End If

            Else
                _blnDisplayHKICSymbol = False
                panHKICSymbol.Visible = False
                lnkSearchHKICwithSymbolTips.Visible = False
                lnkSearchHKICTips.Visible = True

            End If

        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Function TurnOnHKICSymbol() As Boolean
            Dim strStartDate As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction

            If _strSchemeCode = String.Empty Then
                udtGeneralFunction.getSystemParameter("OCSSSStartDate", strStartDate, String.Empty)
            Else
                udtGeneralFunction.getSystemParameter("OCSSSStartDate", strStartDate, String.Empty, _strSchemeCode)
            End If

            If strStartDate = String.Empty Then
                Return False
            End If

            Dim dtmStartDate As DateTime = Convert.ToDateTime(strStartDate)

            Return (Now >= dtmStartDate)

        End Function
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub ClearHKICSymbolSelection()
            Me.rblHKICSymbol.SelectedIndex = -1
            Me.rblHKICSymbol.SelectedValue = Nothing
        End Sub

        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

#End Region
    End Class

End Namespace