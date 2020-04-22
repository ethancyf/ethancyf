Imports Common.Component.DocType

Partial Public Class ucInputTips
    Inherits System.Web.UI.UserControl

    Public Enum InputTipsType As Integer
        SearchHKIC = 0
        SearchECDOB = 1
        SearchECYOB = 2
        SearchDI = 3
        SearchHKBC = 4
        SearchReentry = 5
        SearchID235B = 6
        SearchVisa = 7
        SearchAdoption = 8
        SearchHKICwithSymbol = 9

        InputHKIC = 20
        InputEC = 21
        InputDI = 22
        InputHKBC = 23
        InputReentry = 24
        InputID235B = 25
        InputVisa = 26
        InputAdoption = 27
        InputSmartIDGender = 28
    End Enum

    Private Enum ActiveViewIndex As Integer
        SearchDocType = 0
        InputHKICDocType = 1
        InputECDocType = 2
        InputHKBCDocType = 3
        InputDIDocType = 4
        InputReentryDocType = 5
        InputID235BDocType = 6
        InputVisaDocType = 7
        InputAdoptionDocType = 8
        InputSmartIDGender = 9
    End Enum

    Public Sub LoadTip()
        Dim strType As String = Me.Attributes("type")
        If Not String.IsNullOrEmpty(strType) Then
            LoadTip(CType(Convert.ToInt32(strType), InputTipsType))
        End If
    End Sub

    Public Sub LoadTip(ByVal type As InputTipsType)
        LoadTip(type, New BLL.SessionHandler().Language())
        Me.Attributes("type") = type
    End Sub

    Public Sub LoadTip(ByVal type As InputTipsType, ByVal strLanguage As String)
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeList As DocTypeModelCollection
        udtDocTypeList = udtDocTypeBLL.getAllDocType
        panSearchTipsHKICSymbol.Visible = False

        Select Case type
            Case InputTipsType.SearchHKIC, InputTipsType.SearchHKICwithSymbol
                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If type = InputTipsType.SearchHKICwithSymbol Then
                    panSearchTipsHKICSymbol.Visible = True
                    lblInputHKICSymbolText.Text = Me.GetGlobalResourceObject("Text", "HKICSymbolLong")
                    lblInputHKICSymbolHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKIC).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKIC).DocIdentityDesc
                End If

                'lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "HKICHint")
                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "DOBHint")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchECDOB
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.EC).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.EC).DocIdentityDesc
                End If

                'lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "HKICHint")
                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                lblSearchDocDOBHint.Text = String.Format("{0}<BR/>{1}", Me.GetGlobalResourceObject("Text", "ECDOBHint"), Me.GetGlobalResourceObject("Text", "ECDOBHint2"))

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchECYOB
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.EC).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.EC).DocIdentityDesc
                End If

                'lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "HKICHint")
                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "YOB")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "ECDORegisterAgeHint")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchDI
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.DI).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.DI).DocIdentityDesc
                End If

                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchHKBC
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKBC).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.HKBC).DocIdentityDesc
                End If

                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "DOBHintHKBC")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchReentry
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.REPMT).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.REPMT).DocIdentityDesc
                End If

                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "DOBHintREPMT")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchID235B
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ID235B).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ID235B).DocIdentityDesc
                End If

                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "DOBHintID235B")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchVisa
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.VISA).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.VISA).DocIdentityDesc
                End If

                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "DOBHintVISA")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

            Case InputTipsType.SearchAdoption
                If strLanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ADOPC).DocIdentityDescChi
                Else
                    Me.lblSearchDocIDText.Text = udtDocTypeList.Filter(DocTypeModel.DocTypeCode.ADOPC).DocIdentityDesc
                End If

                lblSearchDocIDHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")

                lblSearchDocDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
                lblSearchDocDOBHint.Text = Me.GetGlobalResourceObject("Text", "DOBHintADOPC")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.SearchDocType

                'For input contol

            Case InputTipsType.InputHKIC
                lblInputHKICENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
                lblInputHKICENameHint.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

                lblInputHKICDOIText.Text = Me.GetGlobalResourceObject("Text", "DOILong")
                lblInputHKICDOIHint.Text = Me.GetGlobalResourceObject("Text", "DOIHintHKID")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputHKICDocType

            Case InputTipsType.InputEC
                lblInputECSerialText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
                lblInputECSerialHint.Text = Me.GetGlobalResourceObject("Text", "ECSerialNoHint")

                lblInputECRefText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
                lblInputECRefHint.Text = Me.GetGlobalResourceObject("Text", "ECReferenceHint")

                lblInputECDOIText.Text = Me.GetGlobalResourceObject("Text", "ECDate")
                lblInputECDOIHint.Text = Me.GetGlobalResourceObject("Text", "ECIssueDateHint")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputECDocType

            Case InputTipsType.InputDI
                lblInputDIENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
                lblInputDIENameHint.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

                lblInputDIDOIText.Text = Me.GetGlobalResourceObject("Text", "DOILong")
                lblInputDIDOIHint.Text = Me.GetGlobalResourceObject("Text", "DOIHintDI")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputDIDocType

            Case InputTipsType.InputHKBC
                lblInputHKBCENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
                lblInputHKBCENameHint.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputHKBCDocType

            Case InputTipsType.InputReentry
                lblInputREPMTENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
                lblInputREPMTENameHint.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

                lblInputREPMTDOIText.Text = Me.GetGlobalResourceObject("Text", "DOILong")
                lblInputREPMTDOIHint.Text = Me.GetGlobalResourceObject("Text", "DOIHintREPMT")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputReentryDocType

            Case InputTipsType.InputID235B
                lblInputID235BENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
                lblInputID235BENameHint.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

                lblInputID235BPermitText.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain")
                lblInputID235BPermitHint.Text = Me.GetGlobalResourceObject("Text", "PMTRemainHintID235B")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputID235BDocType

            Case InputTipsType.InputVisa
                lblInputVISAENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
                lblInputVISAENameHint.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputVisaDocType

            Case InputTipsType.InputAdoption
                lblInputADOPCENameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
                lblInputADOPCENameHint.Text = Me.GetGlobalResourceObject("Text", "EnameHint")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputAdoptionDocType

            Case InputTipsType.InputSmartIDGender
                Me.lblInputSmartIDGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
                Me.lblInputSmartIDGender.Text = Me.GetGlobalResourceObject("Text", "SmartICGenderTips")

                Me.mvInputTips.ActiveViewIndex = ActiveViewIndex.InputSmartIDGender

        End Select

    End Sub

End Class