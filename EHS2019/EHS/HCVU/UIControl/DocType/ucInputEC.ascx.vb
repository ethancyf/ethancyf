Imports System.Web.Security.AntiXss
Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component
Imports Common.Component.DocType


Partial Public Class ucInputEC
    Inherits ucInputDocTypeBase

    '----------------For Amending Records Used Only
    Private _strSerialNumber As String
    Private _strECReference As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strGender As String
    Private _strHKID As String
    Private _strDOB As String
    Private _strIsExactDOB As String
    Private _udtDOBSelection As DOBSelection
    Private _strECAge As String
    'For DOB Age
    Private _strECAgeDORDay As String = String.Empty
    Private _strECAgeDORMonth As String = String.Empty
    Private _strECAgeDORYear As String = String.Empty

    Private _strECDateOfRegistration As String
    Private _blnDOBTypeSelected As Boolean

    'For DOI 
    Private _strECDateDay As String
    Private _strECDateMonth As String
    Private _strECDateYear As String

    Private _strDOD As String ' CRE14-016

    Private _blnSerialNumberNotProvided As Boolean ' Indicate whether the Serial No. is not provided [True|False]
    Private _blnReferenceNoOtherFormat As Boolean ' Indicate whether the Reference is in free format [True|False]

    Private commfunct As Common.ComFunction.GeneralFunction

#Region "Session"

    Private Class VS
        Public Const SerialNumberNotProvided As String = "ucInputEC_SerialNumberNotProvided"
        Public Const ReferenceOtherFormat As String = "ucInputEC_ReferenceOtherFormat"
    End Class

#End Region

#Region "Enum Class (DOB Selection)"
    Public Enum DOBSelection
        ExactDOB
        YearOfBirthReported
        RecordOnTravDoc
        AgeWithDateOfRegistration
        NoValue
    End Enum
#End Region

    Protected Overrides Sub Setup(ByVal mode As BuildMode)

        MyBase.Mode = mode

        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim formatter As Formatter = New Formatter

        Me.pnlNew.Visible = False
        Me.pnlModify.Visible = False

        If Me.Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(mode, False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                'Pre-Fill before entering account creation page (eHS maintenance)
                Me._strHKID = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
                Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName

                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
                Me._strIsExactDOB = MyBase.EHSPersonalInfoAmend.ExactDOB

                If MyBase.EHSPersonalInfoAmend.ECAge.HasValue() Then
                    Me._strECAge = MyBase.EHSPersonalInfoAmend.ECAge.Value.ToString()
                End If
                If MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.HasValue() Then
                    Me._strECAgeDORDay = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.Day.ToString()
                    Me._strECAgeDORYear = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.Year.ToString()
                    Me._strECAgeDORMonth = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.Month.ToString()
                End If

                Me._strSerialNumber = MyBase.EHSPersonalInfoAmend.ECSerialNo
                Me._strECReference = MyBase.EHSPersonalInfoAmend.ECReferenceNo

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                Me.SetDOD()

                '' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                'If MyBase.EHSPersonalInfoAmend.Deceased Then
                '    Me._strDOD = MyBase.EHSPersonalInfoAmend.FormattedDOD
                '    Me.lblDODText.Visible = True
                '    Me.lblDOD.Visible = True
                '    Me.imgDOD.Visible = True
                'Else
                '    Me.lblDODText.Visible = False
                '    Me.lblDOD.Visible = False
                '    Me.imgDOD.Visible = False
                'End If
                '' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

                Me.txtNewHKIC.Enabled = False
                SetValue(BuildMode.Creation)
            End If

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            Me.ddlNewDOIMonth.DataSource = commfunct.GetMonthSelection(Common.Component.CultureLanguage.English)
            Me.ddlNewDOIMonth.DataValueField = "Value"
            Me.ddlNewDOIMonth.DataTextField = "Display"
            Me.ddlNewDOIMonth.DataBind()

            If MyBase.EHSPersonalInfoAmend.ECSerialNoNotProvided AndAlso IsNothing(ViewState(VS.SerialNumberNotProvided)) Then ViewState(VS.SerialNumberNotProvided) = "Y"
            _blnSerialNumberNotProvided = MyBase.EHSPersonalInfoAmend.ECSerialNoNotProvided
            Me._strSerialNumber = MyBase.EHSPersonalInfoAmend.ECSerialNo

            If MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat AndAlso IsNothing(ViewState(VS.ReferenceOtherFormat)) Then ViewState(VS.ReferenceOtherFormat) = "Y"
            _blnReferenceNoOtherFormat = MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat
            Me._strECReference = MyBase.EHSPersonalInfoAmend.ECReferenceNo

            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strCName = MyBase.EHSPersonalInfoAmend.CName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strHKID = formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty), False)
            Me._blnDOBTypeSelected = MyBase.EHSPersonalInfoAmend.DOBTypeSelected
            Me._strDOB = formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), CInt(Me._strECAge), MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfoAmend.ExactDOB

            If MyBase.EHSPersonalInfoAmend.ECAge.HasValue Then
                If MyBase.EHSPersonalInfoAmend.ExactDOB.Trim = "A" Then
                    Me._strECAge = MyBase.EHSPersonalInfoAmend.ECAge.Value.ToString()
                    Me._strECDateOfRegistration = formatter.formatECDORegistration(Session("Language"), MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
                End If
            Else
                Me._strECAge = -1
            End If
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Me.SetDOD()
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            'DOI
            If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                Me._strECDateDay = MyBase.EHSPersonalInfoAmend.DateofIssue.Value.Day
                Me._strECDateMonth = MyBase.EHSPersonalInfoAmend.DateofIssue.Value.ToString("MM")
                Me._strECDateYear = MyBase.EHSPersonalInfoAmend.DateofIssue.Value.Year
            End If

            'DOB age (Date of Registration)
            If MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.HasValue Then
                Me._strECAgeDORDay = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.Day
                Me._strECAgeDORMonth = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.ToString("MM")
                Me._strECAgeDORYear = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.Year
            End If
        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True

            Me.ddlECDateMonth.DataSource = commfunct.GetMonthSelection(Common.Component.CultureLanguage.English)
            Me.ddlECDateMonth.DataValueField = "Value"
            Me.ddlECDateMonth.DataTextField = "Display"
            Me.ddlECDateMonth.DataBind()

            '------------------------------------------- For Amending Reocd ----------------------------------------
            _blnSerialNumberNotProvided = MyBase.EHSPersonalInfoAmend.ECSerialNoNotProvided
            Me._strSerialNumber = MyBase.EHSPersonalInfoAmend.ECSerialNo

            _blnReferenceNoOtherFormat = MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat
            Me._strECReference = MyBase.EHSPersonalInfoAmend.ECReferenceNo

            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strCName = MyBase.EHSPersonalInfoAmend.CName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strHKID = formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty), False)
            Me._blnDOBTypeSelected = MyBase.EHSPersonalInfoAmend.DOBTypeSelected
            Me._strDOB = formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), CInt(Me._strECAge), MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            Me._strIsExactDOB = MyBase.EHSPersonalInfoAmend.ExactDOB

            If MyBase.EHSPersonalInfoAmend.ECAge.HasValue Then
                If MyBase.EHSPersonalInfoAmend.ExactDOB.Trim = "A" Then
                    Me._strECAge = MyBase.EHSPersonalInfoAmend.ECAge.Value.ToString()
                    Me._strECDateOfRegistration = formatter.formatECDORegistration(Session("Language"), MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
                End If
            Else
                Me._strECAge = -1
            End If

            'DOI
            If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                Me._strECDateDay = MyBase.EHSPersonalInfoAmend.DateofIssue.Value.Day
                Me._strECDateMonth = MyBase.EHSPersonalInfoAmend.DateofIssue.Value.ToString("MM")
                Me._strECDateYear = MyBase.EHSPersonalInfoAmend.DateofIssue.Value.Year
            End If

            'DOB age (Date of Registration)
            If MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.HasValue Then
                Me._strECAgeDORDay = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.Day
                Me._strECAgeDORMonth = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.ToString("MM")
                Me._strECAgeDORYear = MyBase.EHSPersonalInfoAmend.ECDateOfRegistration.Value.Year
            End If
            '-----------------------------------------------------------------------------------------------------
        End If


        Me.SetupFieldsStatus(mode)

    End Sub

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
        'Table title
        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        'Me.lblECHKIDOriginalText.Text = Me.GetGlobalResourceObject("Text", "HKID")
        Me.lblECSerialNoOriginalText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
        Me.lblECReferenceNoOriginalText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
        Me.lblECDateOriginalText.Text = Me.GetGlobalResourceObject("Text", "ECDate")
        Me.lblNameOriginalText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblDOBOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.lblGenderOriginalText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblCNameOriginalText.Text = Me.GetGlobalResourceObject("Text", "ChineseName")

        Dim errorButtonImageURL As String = Me.GetGlobalResourceObject("ImageUrl", "ErrorBtn")
        Dim errorButtonImageALT As String = Me.GetGlobalResourceObject("AlternateText", "ErrorBtn")


        Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")

        If MyBase.Mode = BuildMode.Modification Then
            Me.lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "AmendingRecord")
        Else
            If Not MyBase.UseDefaultAmendingHeader Then
                lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "PendingImmgValid")
            Else
                lblAmendingRecordText.Text = Me.GetGlobalResourceObject("Text", "AmendingRecord")
            End If
        End If

        'Gender Radio button list
        Me.rbECGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbECGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgECSerialNo.ImageUrl = strErrorImageURL
        Me.imgECSerialNo.AlternateText = strErrorImageALT
        Me.imgECRefence.ImageUrl = strErrorImageURL
        Me.imgECRefence.AlternateText = strErrorImageALT
        Me.imgECDate.ImageUrl = strErrorImageURL
        Me.imgECDate.AlternateText = strErrorImageALT
        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT
        Me.imgECGender.ImageUrl = strErrorImageURL
        Me.imgECGender.AlternateText = strErrorImageALT

        'Tips
        'Me.lblECSerialNoHints.Text = Me.GetGlobalResourceObject("Text", "ECSerialNoHint")
        'Me.lblECReferenceHints.Text = Me.GetGlobalResourceObject("Text", "ECReferenceHint")
        'Me.lblECIssueDateHints.Text = Me.GetGlobalResourceObject("Text", "ECIssueDateHint")

        Me.lblECDOBDateText.Text = "(" + Me.GetGlobalResourceObject("Text", "DOBLong") + ")"
        Me.lblECDOBOr1Text.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblECDOBReportText.Text = "(" + Me.GetGlobalResourceObject("Text", "YOB") + ")"
        Me.lblECDOBOr2Text.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblECDOBTravelText.Text = "(" + Me.GetGlobalResourceObject("Text", "TravelDoc") + ")"
        Me.lblECDOBOr3Text.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblECDOAText.Text = Me.GetGlobalResourceObject("Text", "Age")
        Me.lblECDOAOnText.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")
        'Me.lblECHKIDText.Text = Me.GetGlobalResourceObject("Text", "HKID")

        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
            Me.ECDOARenderLanguage(False)
        Else
            Me.ECDOARenderLanguage(True)
        End If

        'Get Documnet type full name
        Dim strDocumentTypeFullName As String
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()
        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese) Then
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocNameChi
        Else
            strDocumentTypeFullName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocName
        End If
        lblDocumentTypeOriginal.Text = strDocumentTypeFullName

        ' -------------------------- Creation --------------------------------------------------------------------------------------------
        'Get Documnet identity no name


        Dim strDocIdentityNoName As String

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        strDocIdentityNoName = udtDocTypeModelList.Filter(DocTypeCode.EC).DocIdentityDesc(Session("Language").ToString())
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Me.lblNewHKICText.Text = strDocIdentityNoName
        Me.lblECHKIDOriginalText.Text = strDocIdentityNoName

        Me.lblNewSerialNoText.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo")
        Me.lblNewRefNoText.Text = Me.GetGlobalResourceObject("Text", "ECReference")
        Me.lblNewDOIText.Text = Me.GetGlobalResourceObject("Text", "ECDate")
        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewCNameText.Text = Me.GetGlobalResourceObject("Text", "ChineseName")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        Me.lblDODText.Text = Me.GetGlobalResourceObject("Text", "DateOfDeath")
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgSerialNoErr.ImageUrl = strErrorImageURL
        Me.imgSerialNoErr.AlternateText = strErrorImageALT
        Me.imgRefNoErr.ImageUrl = strErrorImageURL
        Me.imgRefNoErr.AlternateText = strErrorImageALT
        Me.imgNewDOIErr.ImageUrl = strErrorImageURL
        Me.imgNewDOIErr.AlternateText = strErrorImageALT
        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT
        Me.imgNewCNameErr.ImageUrl = strErrorImageURL
        Me.imgNewCNameErr.AlternateText = strErrorImageALT
        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        'Tips
        Me.lblECSerialNoHints.Text = Me.GetGlobalResourceObject("Text", "ECSerialNoHint")
        Me.lblECReferenceHints.Text = Me.GetGlobalResourceObject("Text", "ECReferenceHint")
        Me.lblECIssueDateHints.Text = Me.GetGlobalResourceObject("Text", "ECIssueDateHint")

        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")

        Me.lblNewECDOBDateText.Text = "(" + Me.GetGlobalResourceObject("Text", "DOBLong") + ")"
        Me.lblNewECDOBOr1Text.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblNewECDOBReportText.Text = "(" + Me.GetGlobalResourceObject("Text", "YOB") + ")"
        Me.lblNewECDOBOr2Text.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblNewECDOBTravelText.Text = "(" + Me.GetGlobalResourceObject("Text", "TravelDoc") + ")"
        Me.lblNewECDOBOr3Text.Text = Me.GetGlobalResourceObject("Text", "Or")
        Me.lblNewECDOAText.Text = Me.GetGlobalResourceObject("Text", "Age")
        Me.lblNewECDOAOnText.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")

        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
            Me.ECDOARenderLanguage_New(False)
        Else
            Me.ECDOARenderLanguage_New(True)
        End If

    End Sub

    Private Sub SetupFieldsStatus(ByVal modeType As ucInputDocTypeBase.BuildMode)

        If Not modeType = ucInputDocTypeBase.BuildMode.Creation Then
            Me.SetupENameFirstName()
            Me.SetupENameSurName()
            Me.SetupCName()
            Me.SetupECDate()
            Me.SetupGender()
            Me.SetupHKID()
        End If

        If modeType = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            '--------------------------------------------------------
            'Modify Read Only Mode
            'ReadOnly, 2 sides showing both original and amending record 
            '--------------------------------------------------------
            Me.pnlModify.Visible = True
            Me.pnlNew.Visible = False

            Me.txtECSerialNo.Enabled = False
            Me.cboECSerialNoNotProvided.Enabled = False
            Me.txtECRefence1.Enabled = False
            Me.txtECRefence2.Enabled = False
            Me.txtECRefence3.Enabled = False
            Me.txtECRefence4.Enabled = False
            Me.txtECRefFree.Enabled = False
            Me.txtECDateDay.Enabled = False
            Me.ddlECDateMonth.Enabled = False
            Me.txtECDateYear.Enabled = False
            Me.txtENameSurname.Enabled = False
            Me.txtENameFirstname.Enabled = False
            Me.txtCName.Enabled = False
            Me.rbECGender.Enabled = False

            Me.rbECDOA.Enabled = False
            Me.rbECDOBDate.Enabled = False
            Me.rbECDOBtravel.Enabled = False
            Me.rbECDOByear.Enabled = False
            Me.txtDOBtravel.Enabled = False

            Me.txtDOBDate.Enabled = False
            Me.txtECDOAAge.Enabled = False
            Me.txtECDOADayEn.Enabled = False
            Me.txtECDOADayChi.Enabled = False
            Me.txtECDOAYearEn.Enabled = False
            Me.txtECDOAYearChi.Enabled = False
            Me.ddlECDOAMonth.Enabled = False
            Me.txtDOBtravel.Enabled = False
        ElseIf modeType = ucInputDocTypeBase.BuildMode.Modification Then
            '--------------------------------------------------------
            'Modification Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = True
            Me.pnlNew.Visible = False

            Me.txtECSerialNo.Enabled = True
            Me.cboECSerialNoNotProvided.Enabled = True
            Me.txtECRefence1.Enabled = True
            Me.txtECRefence2.Enabled = True
            Me.txtECRefence3.Enabled = True
            Me.txtECRefence4.Enabled = True
            Me.txtECRefFree.Enabled = True
            Me.txtECDateDay.Enabled = True
            Me.ddlECDateMonth.Enabled = True
            Me.txtECDateYear.Enabled = True
            Me.txtENameSurname.Enabled = True
            Me.txtENameFirstname.Enabled = True
            Me.txtCName.Enabled = True
            Me.rbECGender.Enabled = True
            Me.txtDOBtravel.Enabled = True
            Me.rbECDOA.Enabled = True
            Me.rbECDOBDate.Enabled = True
            Me.rbECDOBtravel.Enabled = True
            Me.rbECDOByear.Enabled = True
            Me.txtDOBtravel.Enabled = True

            Me.txtDOBDate.Enabled = True
            Me.txtECDOAAge.Enabled = True
            Me.txtECDOADayEn.Enabled = True
            Me.txtECDOADayChi.Enabled = True
            Me.txtECDOAYearEn.Enabled = True
            Me.txtECDOAYearChi.Enabled = True
            Me.ddlECDOAMonth.Enabled = True
        ElseIf modeType = ucInputDocTypeBase.BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = False
            Me.pnlNew.Visible = True

            Me.lblNewHKIC.Visible = False
            Me.txtNewHKIC.Visible = True
            Me.txtNewHKIC.Enabled = False
            Me.txtNewSerialNo.Enabled = True
            Me.cboNewECSerialNoNotProvided.Enabled = True
            Me.txtNewRefNo1.Enabled = True
            Me.txtNewRefNo2.Enabled = True
            Me.txtNewRefNo3.Enabled = True
            Me.txtNewRefNo4.Enabled = True
            Me.txtNewRefNoFree.Enabled = True
            Me.txtNewDOIDay.Enabled = True
            Me.ddlNewDOIMonth.Enabled = True
            Me.txtNewDOIYear.Enabled = True
            Me.txtNewSurname.Enabled = True
            Me.txtNewGivenName.Enabled = True
            Me.txtNewCName.Enabled = True
            Me.rboNewGender.Enabled = True

            'Me.rbNewECDOBDate.Enabled = True
            'Me.txtNewDOBDate.Enabled = True

            'Me.rbNewECDOByear.Enabled = True
            'Me.txtNewDOByear.Enabled = True

            'Me.rbNewECDOBtravel.Enabled = True
            'Me.txtNewDOBtravel.Enabled = True

            'Me.rbNewECDOA.Enabled = True
            'Me.txtNewECDOAAge.Enabled = True
            'Me.txtNewECDOADayEn.Enabled = True
            'Me.txtNewECDOAYearChi.Enabled = True
            'Me.ddlNewECDOAMonth.Enabled = True
            'Me.txtNewECDOAYearEn.Enabled = True
            'Me.txtNewECDOADayChi.Enabled = True
        Else
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlModify.Visible = False
            Me.pnlNew.Visible = True

            Me.lblNewHKIC.Visible = True
            Me.txtNewHKIC.Visible = False
            Me.txtNewHKIC.Enabled = False
            Me.txtNewSerialNo.Enabled = True
            Me.cboNewECSerialNoNotProvided.Enabled = True
            Me.txtNewRefNo1.Enabled = True
            Me.txtNewRefNo2.Enabled = True
            Me.txtNewRefNo3.Enabled = True
            Me.txtNewRefNo4.Enabled = True
            Me.txtNewRefNoFree.Enabled = True
            Me.txtNewDOIDay.Enabled = True
            Me.ddlNewDOIMonth.Enabled = True
            Me.txtNewDOIYear.Enabled = True
            Me.txtNewSurname.Enabled = True
            Me.txtNewGivenName.Enabled = True
            Me.txtNewCName.Enabled = True
            Me.rboNewGender.Enabled = True

            Me.rbNewECDOBDate.Enabled = True
            Me.txtNewDOBDate.Enabled = True

            Me.rbNewECDOByear.Enabled = True
            Me.txtNewDOByear.Enabled = True

            Me.rbNewECDOBtravel.Enabled = True
            Me.txtNewDOBtravel.Enabled = True

            Me.rbNewECDOA.Enabled = True
            Me.txtNewECDOAAge.Enabled = True
            Me.txtNewECDOADayEn.Enabled = True
            Me.txtNewECDOAYearChi.Enabled = True
            Me.ddlNewECDOAMonth.Enabled = True
            Me.txtNewECDOAYearEn.Enabled = True
            Me.txtNewECDOADayChi.Enabled = True
        End If

        MyBase.Mode = modeType
        Me.SetupDOB()

        'Error Image
        If MyBase.ActiveViewChanged Then
            Me.SetHKICNoError(False)
            Me.SetECSerialNoError(False)
            Me.SetECReferenceError(False)
            Me.SetECDateError(False)
            Me.SetENameError(False)
            Me.SetCNameError(False)
            Me.SetGenderError(False)

            Me.SetDOBDateError(False)
            Me.SetDOByearError(False)
            Me.SetDOBTravelDocError(False)
            Me.SetDOBAgeError(False)
            Me.SetDateOfRegError(False)
        End If

        'Tips
        If modeType = ucInputDocTypeBase.BuildMode.Creation Then
            Me.lblECSerialNoHints.Visible = True
            Me.lblECReferenceHints.Visible = True
            Me.lblECIssueDateHints.Visible = True
        Else
            Me.lblECSerialNoHints.Visible = False
            Me.lblECReferenceHints.Visible = False
            Me.lblECIssueDateHints.Visible = False
        End If

        Me.SetupSerialNumber()
        Me.SetupECReferenceNo()

        If modeType = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            ibtnOtherFormat.Visible = False
            ibtnSpecificFormat.Visible = False
            txtECSerialNo.Enabled = False
        End If

    End Sub

#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            Me.SetValueCreation()
        Else
            Me.SetValue()
        End If

    End Sub

    Public Overloads Sub SetValue()
        Me.SetupSerialNumber()
        Me.SetupECReferenceNo()
        Me.SetupENameFirstName()
        Me.SetupCName()
        Me.SetupECDate()
        Me.SetupGender()
        Me.SetupHKID()
        Me.SetupDOB()
        Me.SetDOD()
    End Sub

    Public Sub SetValueCreation()
        Me.SetupHKID()
        Me.SetupENameFirstName()
        Me.SetupENameSurName()
        Me.SetupDOB()
        Me.SetDOD()
    End Sub

    Public Sub SetupSerialNumber()

        If Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.txtNewSerialNo.Text = Me._strSerialNumber

            If ViewState(VS.SerialNumberNotProvided) = "Y" Then
                cboNewECSerialNoNotProvided.Checked = True
                EnableSerialNo_New(False)
            Else
                EnableSerialNo_New(True)
            End If

        ElseIf Mode = BuildMode.Modification_OneSide Then
            '-------------------------------------------------------
            'For Modification (One Side) Mode
            '-------------------------------------------------------
            Me.txtNewSerialNo.Text = Me._strSerialNumber

            If Me._blnSerialNumberNotProvided Then
                cboNewECSerialNoNotProvided.Checked = True
                EnableSerialNo_New(False)
            Else
                cboNewECSerialNoNotProvided.Checked = False
                EnableSerialNo_New(True)
            End If

        Else
            '--------------------------------------------------------
            'Modification Mode
            '--------------------------------------------------------
            'Amend
            Me.txtECSerialNo.Text = Me._strSerialNumber

            If Me._blnSerialNumberNotProvided Then
                cboECSerialNoNotProvided.Checked = True
                EnableSerialNo(False)
            Else
                cboECSerialNoNotProvided.Checked = False
                EnableSerialNo(True)
            End If

            'Original
            Dim strOrigSerialNo As String = MyBase.EHSPersonalInfoOriginal.ECSerialNo
            If strOrigSerialNo = String.Empty Then strOrigSerialNo = Me.GetGlobalResourceObject("Text", "NotProvided")

            Me.lblECSerialNoOriginal.Text = strOrigSerialNo
        End If

    End Sub

    Public Sub SetupECReferenceNo()

        If Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            If ViewState(VS.ReferenceOtherFormat) = "Y" Then
                EnableReferenceOtherFormat_New(True)
                txtNewRefNoFree.Text = _strECReference
            Else
                EnableReferenceOtherFormat_New(False)
                If Not IsNothing(_strECReference) AndAlso _strECReference.Length = 14 Then
                    txtNewRefNo1.Text = _strECReference.Substring(0, 4)
                    txtNewRefNo2.Text = _strECReference.Substring(4, 7)
                    txtNewRefNo3.Text = _strECReference.Substring(11, 2)
                    txtNewRefNo4.Text = _strECReference.Substring(13, 1)
                End If
            End If

        ElseIf Mode = BuildMode.Modification_OneSide Then
            '-------------------------------------------------------
            'For Modification (One Side) Mode
            '-------------------------------------------------------
            If ViewState(VS.ReferenceOtherFormat) = "Y" Then
                EnableReferenceOtherFormat_New(True)
                txtNewRefNoFree.Text = _strECReference
            Else
                EnableReferenceOtherFormat_New(False)
                If Not IsNothing(_strECReference) AndAlso _strECReference.Length = 14 Then
                    txtNewRefNo1.Text = _strECReference.Substring(0, 4)
                    txtNewRefNo2.Text = _strECReference.Substring(4, 7)
                    txtNewRefNo3.Text = _strECReference.Substring(11, 2)
                    txtNewRefNo4.Text = _strECReference.Substring(13, 1)
                End If
            End If

        Else
            '--------------------------------------------------------
            'Modification Mode
            '--------------------------------------------------------
            'Amend
            If _blnReferenceNoOtherFormat Then
                EnableReferenceOtherFormat(True)
                txtECRefFree.Text = _strECReference
            Else
                EnableReferenceOtherFormat(False)
                If Not IsNothing(_strECReference) AndAlso _strECReference.Length = 14 Then
                    txtECRefence1.Text = _strECReference.Substring(0, 4)
                    txtECRefence2.Text = _strECReference.Substring(4, 7)
                    txtECRefence3.Text = _strECReference.Substring(11, 2)
                    txtECRefence4.Text = _strECReference.Substring(13, 1)
                End If

            End If

            'Original
            Dim strOrigReference As String = MyBase.EHSPersonalInfoOriginal.ECReferenceNo
            If MyBase.EHSPersonalInfoOriginal.ECReferenceNoOtherFormat = False Then strOrigReference = MyBase.Formatter.formatReferenceNo(strOrigReference, False)

            Me.lblECReferenceNoOriginal.Text = strOrigReference
        End If

    End Sub

    Public Sub SetupENameFirstName()

        If Mode = BuildMode.Creation Then
            Me.txtNewGivenName.Text = Me._strENameFirstName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewGivenName.Text = Me._strENameFirstName

        Else
            'Amend
            Me.txtENameFirstname.Text = Me._strENameFirstName

            'Original
            Me.lblENameOriginal.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfoOriginal.ENameSurName, MyBase.EHSPersonalInfoOriginal.ENameFirstName)
        End If

    End Sub

    Public Sub SetupENameSurName()

        If Mode = BuildMode.Creation Then
            Me.txtNewSurname.Text = Me._strENameSurName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewSurname.Text = Me._strENameSurName

        Else
            'Amend
            Me.txtENameSurname.Text = Me._strENameSurName
        End If

    End Sub

    Public Sub SetupCName()
        If Mode = BuildMode.Creation Then
            Me.txtNewCName.Text = String.Empty

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewCName.Text = Me._strCName

        Else
            'Amend
            Me.txtCName.Text = Me._strCName
            'Original
            Me.lblCNameOriginal.Text = MyBase.EHSPersonalInfoOriginal.CName
        End If

    End Sub

    Public Sub SetupECDate()
        'Data of Issue

        If Mode = BuildMode.Creation Then
            Me.txtNewDOIDay.Text = String.Empty
            Me.ddlNewDOIMonth.SelectedValue = String.Empty
            Me.txtNewDOIYear.Text = String.Empty

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOIDay.Text = Me._strECDateDay
            Me.ddlNewDOIMonth.SelectedValue = Me._strECDateMonth
            Me.txtNewDOIYear.Text = Me._strECDateYear

        Else
            'Amend
            Me.txtECDateDay.Text = Me._strECDateDay
            Me.ddlECDateMonth.SelectedValue = Me._strECDateMonth
            Me.txtECDateYear.Text = Me._strECDateYear

            'Original
            Me.lblECDateOriginal.Text = MyBase.Formatter.formatECDOI(MyBase.EHSPersonalInfoOriginal.DateofIssue)
        End If

    End Sub

    Public Sub SetupGender()

        If Mode = BuildMode.Creation Then
            Me.rboNewGender.ClearSelection()

        ElseIf Mode = BuildMode.Modification_OneSide Then
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rboNewGender.SelectedValue = Me._strGender
            End If

        Else
            'Amend
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rbECGender.SelectedValue = Me._strGender
            End If

            'Original
            Select Case MyBase.EHSPersonalInfoOriginal.Gender.Trim
                Case "M"
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Case "F"
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                Case Else
                    lblGenderOriginal.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End Select
        End If

    End Sub

    Public Sub SetupHKID()

        If Mode = BuildMode.Creation Then
            Me.txtNewHKIC.Text = Me._strHKID

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.lblNewHKIC.Text = Me._strHKID

        Else
            'Amend
            Me.lblECHKIC.Text = Me._strHKID
            'original
            Me.lblECHKIDOriginal.Text = Formatter.formatHKID(MyBase.EHSPersonalInfoOriginal.IdentityNum.Replace("(", String.Empty).Replace(")", String.Empty), False)
        End If

    End Sub

    Public Sub SetupDOB()
        Dim formatter As Formatter = New Formatter

        If Mode = BuildMode.Creation Then

            Me.txtNewECDOAAge.Text = String.Empty
            Me.txtNewECDOADayEn.Text = String.Empty
            Me.txtNewECDOADayChi.Text = String.Empty
            Me.txtNewECDOAYearEn.Text = String.Empty
            Me.txtNewECDOAYearChi.Text = String.Empty

            'Pre-fill
            If Not IsNothing(_strIsExactDOB) Then
                Select Case Me._strIsExactDOB.Trim
                    Case "D", "M", "Y"
                        Me.SwitchECSearchControl_New(True, False, False, False)
                        rbNewECDOBDate.Checked = True
                        rbNewECDOByear.Checked = False
                        rbNewECDOBtravel.Checked = False
                        rbNewECDOA.Checked = False
                        Me.txtNewDOBDate.Text = _strDOB
                    Case "R"
                        Me.SwitchECSearchControl_New(False, True, False, False)
                        rbNewECDOBDate.Checked = False
                        rbNewECDOByear.Checked = True
                        rbNewECDOBtravel.Checked = False
                        rbNewECDOA.Checked = False
                        Me.txtNewDOByear.Text = _strDOB
                    Case "T", "U", "V"
                        Me.SwitchECSearchControl_New(False, False, True, False)
                        rbNewECDOBDate.Checked = False
                        rbNewECDOByear.Checked = False
                        rbNewECDOBtravel.Checked = True
                        rbNewECDOA.Checked = False
                        Me.txtNewDOBtravel.Text = _strDOB
                    Case "A"
                        Me.SwitchECSearchControl_New(False, False, False, True)
                        rbNewECDOBDate.Checked = False
                        rbNewECDOByear.Checked = False
                        rbNewECDOBtravel.Checked = False
                        rbNewECDOA.Checked = True
                        Me.txtNewECDOAAge.Text = Me._strECAge.ToString()
                        Me.txtNewECDOADayEn.Text = Me._strECAgeDORDay
                        Me.txtNewECDOADayChi.Text = Me._strECAgeDORDay
                        Me.txtNewECDOAYearEn.Text = Me._strECAgeDORYear
                        Me.txtNewECDOAYearChi.Text = Me._strECAgeDORYear
                        Me.ddlNewECDOAMonth.SelectedIndex = Me._strECAgeDORMonth
                    Case Else
                        rbNewECDOBDate.Checked = False
                        rbNewECDOByear.Checked = False
                        rbNewECDOBtravel.Checked = False
                        rbNewECDOA.Checked = False
                        Me.SwitchECSearchControl_New(False, False, False, False)
                End Select
            End If

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Select Case Me._strIsExactDOB.Trim
                Case "D", "M", "Y"
                    Me.SwitchECSearchControl_New(True, False, False, False)
                    rbNewECDOBDate.Checked = True
                    rbNewECDOByear.Checked = False
                    rbNewECDOBtravel.Checked = False
                    rbNewECDOA.Checked = False
                    Me.txtNewDOBDate.Text = _strDOB
                Case "R"
                    Me.SwitchECSearchControl_New(False, True, False, False)
                    rbNewECDOBDate.Checked = False
                    rbNewECDOByear.Checked = True
                    rbNewECDOBtravel.Checked = False
                    rbNewECDOA.Checked = False
                    Me.txtNewDOByear.Text = _strDOB
                Case "T", "U", "V"
                    Me.SwitchECSearchControl_New(False, False, True, False)
                    rbNewECDOBDate.Checked = False
                    rbNewECDOByear.Checked = False
                    rbNewECDOBtravel.Checked = True
                    rbNewECDOA.Checked = False
                    Me.txtNewDOBtravel.Text = _strDOB
                Case "A"
                    Me.SwitchECSearchControl_New(False, False, False, True)
                    rbNewECDOBDate.Checked = False
                    rbNewECDOByear.Checked = False
                    rbNewECDOBtravel.Checked = False
                    rbNewECDOA.Checked = True
                    Me.txtNewECDOAAge.Text = Me._strECAge.ToString()
                    Me.txtNewECDOADayEn.Text = Me._strECAgeDORDay
                    Me.txtNewECDOADayChi.Text = Me._strECAgeDORDay
                    Me.txtNewECDOAYearEn.Text = Me._strECAgeDORYear
                    Me.txtNewECDOAYearChi.Text = Me._strECAgeDORYear
                    Me.ddlNewECDOAMonth.SelectedIndex = Me._strECAgeDORMonth
            End Select

        Else
            'Amend
            Select Case Me._strIsExactDOB.Trim
                Case "D", "M", "Y"
                    Me.SwitchECSearchControl(True, False, False, False)
                    rbECDOBDate.Checked = True
                    rbECDOByear.Checked = False
                    rbECDOBtravel.Checked = False
                    rbECDOA.Checked = False
                    'Me.txtDOBDate.Text = formatter.formatDOB(Me._strDOB, Me._strIsExactDOB, Session("Language"), CInt(Me._strECAge), Nothing)
                    Me.txtDOBDate.Text = _strDOB
                Case "R"
                    Me.SwitchECSearchControl(False, True, False, False)
                    rbECDOBDate.Checked = False
                    rbECDOByear.Checked = True
                    rbECDOBtravel.Checked = False
                    rbECDOA.Checked = False
                    Me.txtDOByear.Text = _strDOB
                Case "T", "U", "V"
                    Me.SwitchECSearchControl(False, False, True, False)
                    rbECDOBDate.Checked = False
                    rbECDOByear.Checked = False
                    rbECDOBtravel.Checked = True
                    rbECDOA.Checked = False
                    Me.txtDOBtravel.Text = _strDOB
                Case "A"
                    Me.SwitchECSearchControl(False, False, False, True)
                    rbECDOBDate.Checked = False
                    rbECDOByear.Checked = False
                    rbECDOBtravel.Checked = False
                    rbECDOA.Checked = True
                    Me.txtECDOAAge.Text = Me._strECAge.ToString()
                    Me.txtECDOADayEn.Text = Me._strECAgeDORDay
                    Me.txtECDOADayChi.Text = Me._strECAgeDORDay
                    Me.txtECDOAYearEn.Text = Me._strECAgeDORYear
                    Me.txtECDOAYearChi.Text = Me._strECAgeDORYear
                    Me.ddlECDOAMonth.SelectedIndex = Me._strECAgeDORMonth
            End Select

            'Original
            Me.lblDOBOriginal.Text = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, String.Empty, _
                                                    MyBase.EHSPersonalInfoOriginal.ECAge, MyBase.EHSPersonalInfoOriginal.ECDateOfRegistration)
        End If


    End Sub

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
    Public Sub SetDOD()
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        If MyBase.EHSPersonalInfoAmend.Deceased Then
            Me._strDOD = MyBase.EHSPersonalInfoAmend.FormattedDOD
            Me.lblDOD.Text = Me._strDOD
            Me.trDOD.Visible = True
        Else
            Me.trDOD.Visible = False
        End If

        'If Mode = BuildMode.Creation Then
        '    Me.lblDOD.Text = Me._strDOD
        'End If

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
    End Sub
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

#End Region

#Region "Set Up Error Image"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetHKICNoError(visible)
        Me.SetECSerialNoError(visible)
        Me.SetECReferenceError(visible)
        Me.SetECDateError(visible)
        Me.SetENameError(visible)
        Me.SetCNameError(visible)
        Me.SetGenderError(visible)

        Me.SetDOBDateError(visible)
        Me.SetDOByearError(visible)
        Me.SetDOBTravelDocError(visible)
        Me.SetDOBAgeError(visible)
        Me.SetDateOfRegError(visible)
        'Me.SetModificationErrorImage(visible)

    End Sub

    Public Overloads Sub SetModificationErrorImage(ByVal visible As Boolean)
        Me.SetHKICNoError(visible)
        Me.SetECSerialNoError(visible)
        Me.SetECReferenceError(visible)
        Me.SetECDateError(visible)
        Me.SetENameError(visible)
        Me.SetCNameError(visible)
        Me.SetGenderError(visible)

        Me.SetDOBDateError(visible)
        Me.SetDOByearError(visible)
        Me.SetDOBTravelDocError(visible)
        Me.SetDOBAgeError(visible)
        Me.SetDateOfRegError(visible)
    End Sub

    Public Sub SetHKICNoError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewRegNoErr.Visible = visible
        End If
    End Sub

    Public Sub SetECSerialNoError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgSerialNoErr.Visible = visible
        Else
            Me.imgECSerialNo.Visible = visible
        End If

    End Sub

    Public Sub SetECReferenceError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgRefNoErr.Visible = visible
        Else
            Me.imgECRefence.Visible = visible
        End If

    End Sub

    Public Sub SetECDateError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewDOIErr.Visible = visible
        Else
            Me.imgECDate.Visible = visible
        End If

    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewENameErr.Visible = visible
        Else
            Me.imgEName.Visible = visible
        End If

    End Sub

    Public Sub SetCNameError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewCNameErr.Visible = visible
        Else
            Me.imgCName.Visible = visible
        End If

    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewGenderErr.Visible = visible
        Else
            Me.imgECGender.Visible = visible
        End If

    End Sub

    Public Overloads Sub SetErrorImage(ByVal visible As Boolean)
        Me.SetDOBDateError(visible)
        Me.SetDOByearError(visible)
        Me.SetDOBTravelDocError(visible)
        Me.SetDOBAgeError(visible)
    End Sub

    Public Sub SetDOBDateError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewECDOBerror.Visible = visible
        Else
            Me.imgECDOBerror.Visible = visible
        End If

    End Sub

    Public Sub SetDOByearError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewECDOByearerror.Visible = visible
        Else
            Me.imgECDOByearerror.Visible = visible
        End If

    End Sub

    Public Sub SetDOBTravelDocError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewECDOBTravelerror.Visible = visible
        Else
            Me.imgECDOBTravelerror.Visible = visible
        End If

    End Sub

    Public Sub SetDOBAgeError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewECAgeerror.Visible = visible
        Else
            Me.imgECAgeerror.Visible = visible
        End If

    End Sub

    Public Sub SetDateOfRegError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewECDORerror.Visible = visible
        Else
            Me.imgECDORerror.Visible = visible
        End If

    End Sub
#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        MyBase.Mode = mode

        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me.SetPropertyCreation()
        ElseIf mode = ucInputDocTypeBase.BuildMode.Modification_OneSide Then
            Me.SetPropertyModfiyOneSide()
        Else
            Me.SetPropertyModify()
        End If
    End Sub

    Public Sub SetPropertyCreation()
        Me._strECDateDay = Me.txtNewDOIDay.Text.Trim()
        Me._strECDateMonth = Me.ddlNewDOIMonth.SelectedValue.Trim()
        Me._strECDateYear = Me.txtNewDOIYear.Text.Trim()
        Me._strDOD = Me.lblDOD.Text.Trim ' CRE14-016

        'HKID
        _strHKID = txtNewHKIC.Text.Trim

        ' Serial No.
        _blnSerialNumberNotProvided = cboNewECSerialNoNotProvided.Checked

        If _blnSerialNumberNotProvided Then
            _strSerialNumber = String.Empty
        Else
            _strSerialNumber = Me.txtNewSerialNo.Text.Trim
        End If

        ' Reference Other Format
        '_blnReferenceNoOtherFormat = MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat

        ' Reference
        If ViewState(VS.ReferenceOtherFormat) = "Y" Then
            _blnReferenceNoOtherFormat = True
            _strECReference = txtNewRefNoFree.Text.Trim.ToUpper
        Else
            _blnReferenceNoOtherFormat = False
            _strECReference = String.Format("{0}{1}{2}{3}", Me.txtNewRefNo1.Text.Trim(), Me.txtNewRefNo2.Text.Trim(), Me.txtNewRefNo3.Text.Trim(), Me.txtNewRefNo4.Text.Trim())
        End If

        Me._strENameFirstName = Me.txtNewGivenName.Text.Trim()
        Me._strENameSurName = Me.txtNewSurname.Text.Trim()
        Me._strCName = Me.txtNewCName.Text.Trim()
        Me._strGender = Me.rboNewGender.SelectedValue.Trim()

        'Date Of Birth
        Dim strDOBtype As String = String.Empty

        If Me.rbNewECDOBDate.Checked Then
            _udtDOBSelection = DOBSelection.ExactDOB
            Me._strDOB = Me.txtNewDOBDate.Text.Trim
            Me._strECDateOfRegistration = String.Empty

            strDOBtype = "D"
        Else
            'Year of birth reported
            If Me.rbNewECDOByear.Checked Then
                _udtDOBSelection = DOBSelection.YearOfBirthReported
                Me._strDOB = Me.txtNewDOByear.Text.Trim
                Me._strECDateOfRegistration = String.Empty
                strDOBtype = "R"
            Else
                'Recorded on Travel Doc
                If Me.rbNewECDOBtravel.Checked Then
                    _udtDOBSelection = DOBSelection.RecordOnTravDoc
                    Me._strDOB = Me.txtNewDOBtravel.Text.Trim
                    Me._strECDateOfRegistration = String.Empty
                    strDOBtype = "T"
                Else
                    'Age & DOR
                    If Me.rbNewECDOA.Checked Then
                        _udtDOBSelection = DOBSelection.AgeWithDateOfRegistration
                        Me._strDOB = String.Empty
                        Me._strECAge = Me.txtNewECDOAAge.Text.Trim()
                        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                            Me._strECAgeDORDay = Me.txtNewECDOADayChi.Text.Trim
                            Me._strECAgeDORYear = Me.txtNewECDOAYearChi.Text.Trim
                        Else
                            Me._strECAgeDORDay = Me.txtNewECDOADayEn.Text.Trim
                            Me._strECAgeDORYear = Me.txtNewECDOAYearEn.Text.Trim
                        End If
                        Me._strECAgeDORMonth = Me.ddlNewECDOAMonth.SelectedValue
                        strDOBtype = "A"
                    End If
                End If
            End If
        End If

        Me._strIsExactDOB = strDOBtype
        If _strDOB Is Nothing Then
            _strDOB = String.Empty
        End If
    End Sub

    Public Sub SetPropertyModify()
        Me._strECDateDay = Me.txtECDateDay.Text.Trim()
        Me._strECDateMonth = Me.ddlECDateMonth.SelectedValue.Trim()
        Me._strECDateYear = Me.txtECDateYear.Text.Trim()

        ' Serial No.
        _blnSerialNumberNotProvided = cboECSerialNoNotProvided.Checked

        If _blnSerialNumberNotProvided Then
            _strSerialNumber = String.Empty
        Else
            _strSerialNumber = Me.txtECSerialNo.Text.Trim
        End If

        ' Reference Other Format
        _blnReferenceNoOtherFormat = MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat

        ' Reference
        If _blnReferenceNoOtherFormat = True Then
            _strECReference = txtECRefFree.Text.Trim.ToUpper
        Else
            _strECReference = String.Format("{0}{1}{2}{3}", Me.txtECRefence1.Text.Trim(), Me.txtECRefence2.Text.Trim(), Me.txtECRefence3.Text.Trim(), Me.txtECRefence4.Text.Trim())
        End If

        Me._strENameFirstName = Me.txtENameFirstname.Text.Trim()
        Me._strENameSurName = Me.txtENameSurname.Text.Trim()
        Me._strCName = Me.txtCName.Text.Trim()
        Me._strGender = Me.rbECGender.SelectedValue.Trim()

        'Date Of Birth
        Dim strDOBtype As String = String.Empty

        If Me.rbECDOBDate.Checked Then
            _udtDOBSelection = DOBSelection.ExactDOB
            Me._strDOB = Me.txtDOBDate.Text.Trim
            Me._strECDateOfRegistration = String.Empty

            strDOBtype = "D"
        Else
            'Year of birth reported
            If Me.rbECDOByear.Checked Then
                _udtDOBSelection = DOBSelection.YearOfBirthReported
                Me._strDOB = Me.txtDOByear.Text.Trim
                Me._strECDateOfRegistration = String.Empty
                strDOBtype = "R"
            Else
                'Recorded on Travel Doc
                If Me.rbECDOBtravel.Checked Then
                    _udtDOBSelection = DOBSelection.RecordOnTravDoc
                    Me._strDOB = Me.txtDOBtravel.Text.Trim
                    Me._strECDateOfRegistration = String.Empty
                    strDOBtype = "T"
                Else
                    'Age & DOR
                    If Me.rbECDOA.Checked Then
                        _udtDOBSelection = DOBSelection.AgeWithDateOfRegistration
                        Me._strDOB = String.Empty
                        Me._strECAge = Me.txtECDOAAge.Text.Trim()
                        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                            Me._strECAgeDORDay = Me.txtECDOADayChi.Text.Trim
                            Me._strECAgeDORYear = Me.txtECDOAYearChi.Text.Trim
                        Else
                            Me._strECAgeDORDay = Me.txtECDOADayEn.Text.Trim
                            Me._strECAgeDORYear = Me.txtECDOAYearEn.Text.Trim
                        End If
                        Me._strECAgeDORMonth = Me.ddlECDOAMonth.SelectedValue
                        strDOBtype = "A"
                    End If
                End If
            End If
        End If

        Me._strIsExactDOB = strDOBtype
        If _strDOB Is Nothing Then
            _strDOB = String.Empty
        End If
    End Sub

    Public Sub SetPropertyModfiyOneSide()
        Me._strECDateDay = Me.txtNewDOIDay.Text.Trim()
        Me._strECDateMonth = Me.ddlNewDOIMonth.SelectedValue.Trim()
        Me._strECDateYear = Me.txtNewDOIYear.Text.Trim()

        ' Serial No.
        _blnSerialNumberNotProvided = cboNewECSerialNoNotProvided.Checked

        If _blnSerialNumberNotProvided Then
            _strSerialNumber = String.Empty
        Else
            _strSerialNumber = Me.txtNewSerialNo.Text.Trim
        End If

        ' Reference Other Format
        '_blnReferenceNoOtherFormat = MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat

        ' Reference
        If ViewState(VS.ReferenceOtherFormat) = "Y" Then
            _blnReferenceNoOtherFormat = True
            _strECReference = txtNewRefNoFree.Text.Trim.ToUpper
        Else
            _blnReferenceNoOtherFormat = False
            _strECReference = String.Format("{0}{1}{2}{3}", Me.txtNewRefNo1.Text.Trim(), Me.txtNewRefNo2.Text.Trim(), Me.txtNewRefNo3.Text.Trim(), Me.txtNewRefNo4.Text.Trim())
        End If

        Me._strENameFirstName = Me.txtNewGivenName.Text.Trim()
        Me._strENameSurName = Me.txtNewSurname.Text.Trim()
        Me._strCName = Me.txtNewCName.Text.Trim()
        Me._strGender = Me.rboNewGender.SelectedValue.Trim()

        'Date Of Birth
        Dim strDOBtype As String = String.Empty

        If Me.rbNewECDOBDate.Checked Then
            _udtDOBSelection = DOBSelection.ExactDOB
            Me._strDOB = Me.txtNewDOBDate.Text
            Me._strECDateOfRegistration = String.Empty

            strDOBtype = "D"
        Else
            'Year of birth reported
            If Me.rbNewECDOByear.Checked Then
                _udtDOBSelection = DOBSelection.YearOfBirthReported
                Me._strDOB = Me.txtNewDOByear.Text
                Me._strECDateOfRegistration = String.Empty
                strDOBtype = "R"
            Else
                'Recorded on Travel Doc
                If Me.rbNewECDOBtravel.Checked Then
                    _udtDOBSelection = DOBSelection.RecordOnTravDoc
                    Me._strDOB = Me.txtNewDOBtravel.Text
                    Me._strECDateOfRegistration = String.Empty
                    strDOBtype = "T"
                Else
                    'Age & DOR
                    If Me.rbNewECDOA.Checked Then
                        _udtDOBSelection = DOBSelection.AgeWithDateOfRegistration
                        Me._strDOB = String.Empty
                        Me._strECAge = Me.txtNewECDOAAge.Text.Trim()
                        If Session("Language").ToString().ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                            Me._strECAgeDORDay = Me.txtNewECDOADayChi.Text
                            Me._strECAgeDORYear = Me.txtNewECDOAYearChi.Text
                        Else
                            Me._strECAgeDORDay = Me.txtNewECDOADayEn.Text
                            Me._strECAgeDORYear = Me.txtNewECDOAYearEn.Text
                        End If
                        Me._strECAgeDORMonth = Me.ddlNewECDOAMonth.SelectedValue
                        strDOBtype = "A"
                    End If
                End If
            End If
        End If

        Me._strIsExactDOB = strDOBtype
        If _strDOB Is Nothing Then
            _strDOB = String.Empty
        End If
    End Sub

    Public Property SerialNumber() As String
        Get
            Return Me._strSerialNumber
        End Get
        Set(ByVal value As String)
            Me._strSerialNumber = value
        End Set
    End Property

    Public Property SerialNumberNotProvided() As Boolean
        Get
            Return _blnSerialNumberNotProvided
        End Get
        Set(ByVal value As Boolean)
            _blnSerialNumberNotProvided = value
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return Me._strECReference
        End Get
        Set(ByVal value As String)
            Me._strECReference = value
        End Set
    End Property

    Public Property ReferenceOtherFormat() As Boolean
        Get
            Return _blnReferenceNoOtherFormat
        End Get
        Set(ByVal value As Boolean)
            _blnReferenceNoOtherFormat = value
        End Set
    End Property

    Public Property ENameFirstName() As String
        Get
            Return Me._strENameFirstName
        End Get
        Set(ByVal value As String)
            Me._strENameFirstName = value
        End Set
    End Property

    Public Property ENameSurName() As String
        Get
            Return Me._strENameSurName
        End Get
        Set(ByVal value As String)
            Me._strENameSurName = value
        End Set
    End Property

    Public Property CName() As String
        Get
            Return Me._strCName
        End Get
        Set(ByVal value As String)
            Me._strCName = value
        End Set
    End Property

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property HKID() As String
        Get
            Return Me._strHKID
        End Get
        Set(ByVal value As String)
            Me._strHKID = value
        End Set
    End Property

    Public Property DOBtype() As DOBSelection
        Get
            Return Me._udtDOBSelection
        End Get
        Set(ByVal value As DOBSelection)
            Me._udtDOBSelection = value
        End Set
    End Property

    'DOB----------------------------------------------------------

    Public Property DOB() As String
        Get
            Return Me._strDOB
        End Get
        Set(ByVal value As String)
            Me._strDOB = value
        End Set
    End Property

    Public Property IsExactDOB() As String
        Get
            Return Me._strIsExactDOB
        End Get
        Set(ByVal value As String)
            Me._strIsExactDOB = value
        End Set
    End Property

    'DOB - Age - Date of Registration 
    Public Property ECDateOfRegDay() As String
        Get
            Return Me._strECAgeDORDay
        End Get
        Set(ByVal value As String)
            Me._strECAgeDORDay = value
        End Set
    End Property

    Public Property ECDateOfRegMonth() As String
        Get
            Return Me._strECAgeDORMonth
        End Get
        Set(ByVal value As String)
            Me._strECAgeDORMonth = value
        End Set
    End Property

    Public Property ECDateOfRegYear() As String
        Get
            Return Me._strECAgeDORYear
        End Get
        Set(ByVal value As String)
            Me._strECAgeDORYear = value
        End Set
    End Property

    Public Property ECDateOfRegistration() As String
        Get
            Return Me._strECDateOfRegistration
        End Get
        Set(ByVal value As String)
            Me._strECDateOfRegistration = value
        End Set
    End Property

    Public Property ECAge() As String
        Get
            Return Me._strECAge
        End Get
        Set(ByVal value As String)
            Me._strECAge = value
        End Set
    End Property

    '-------DOI----------------------------------------------------

    Public Property ECDateDay() As String
        Get
            Return Me._strECDateDay
        End Get
        Set(ByVal value As String)
            Me._strECDateDay = value
        End Set
    End Property

    Public Property ECDateMonth() As String
        Get
            Return Me._strECDateMonth
        End Get
        Set(ByVal value As String)
            Me._strECDateMonth = value
        End Set
    End Property

    Public Property ECDateYear() As String
        Get
            Return Me._strECDateYear
        End Get
        Set(ByVal value As String)
            Me._strECDateYear = value
        End Set
    End Property
    '--------------------------------------------------------------

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
    Public Property DOD() As String
        Get
            Return Me._strDOD
        End Get
        Set(ByVal value As String)
            Me._strDOD = value
        End Set
    End Property
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

#End Region

#Region "EC Search Setup"

    'For Modification Mode use only
    Private Sub ECDOARenderLanguage(ByVal blnIsEnglish As Boolean)
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
            End If

            If Me.txtECDOADayChi.Text <> String.Empty Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.txtECDOADayEn.Text = AntiXssEncoder.HtmlEncode(txtECDOADayChi.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
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
            End If

            If Me.txtECDOADayEn.Text <> String.Empty Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.txtECDOADayChi.Text = AntiXssEncoder.HtmlEncode(txtECDOADayEn.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If

            Me.BindECDate(Me.ddlECDOAMonth, Common.Component.CultureLanguage.TradChinese)
        End If
    End Sub

    'For Modification Mode use only
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

    'For Modification Mode use only
    Private Sub SwitchECSearchControl(ByVal blnDateChecked As Boolean, ByVal blnYearChecked As Boolean, ByVal blnTravelDocChecked As Boolean, ByVal blnAgeChecked As Boolean)
        'Date
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtDOBDate.Enabled = False
        Else
            Me.txtDOBDate.Enabled = blnDateChecked
        End If
        If blnDateChecked Then
            'Me.txtDOBDate.BackColor = Drawing.Color.White
        Else
            'Me.txtDOBDate.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOBDate.Text = String.Empty
            End If
        End If

        'Year
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtDOByear.Enabled = False
        Else
            Me.txtDOByear.Enabled = blnYearChecked
        End If
        If blnYearChecked Then
            'Me.txtDOByear.BackColor = Drawing.Color.White
        Else
            'Me.txtDOByear.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOByear.Text = String.Empty
            End If
        End If

        'Travel
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtDOBtravel.Enabled = False
        Else
            Me.txtDOBtravel.Enabled = blnTravelDocChecked
        End If
        If blnTravelDocChecked Then
            'Me.txtDOBtravel.BackColor = Drawing.Color.White
        Else
            'Me.txtDOBtravel.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOBtravel.Text = String.Empty
            End If
        End If

        'Age
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtECDOAAge.Enabled = False
            Me.txtECDOADayEn.Enabled = False
            Me.txtECDOAYearEn.Enabled = False
            Me.ddlECDOAMonth.Enabled = False
            Me.txtECDOAYearChi.Enabled = False
            Me.txtECDOADayChi.Enabled = False
        Else
            Me.txtECDOAAge.Enabled = blnAgeChecked
            Me.txtECDOADayEn.Enabled = blnAgeChecked
            Me.txtECDOAYearEn.Enabled = blnAgeChecked
            Me.ddlECDOAMonth.Enabled = blnAgeChecked
            Me.txtECDOAYearChi.Enabled = blnAgeChecked
            Me.txtECDOADayChi.Enabled = blnAgeChecked
        End If
        If Not blnAgeChecked Then
            'Me.txtECDOAAge.BackColor = Drawing.Color.Silver
            'Me.txtECDOADayEn.BackColor = Drawing.Color.Silver
            'Me.txtECDOAYearEn.BackColor = Drawing.Color.Silver
            'Me.ddlECDOAMonth.BackColor = Drawing.Color.Silver
            'Me.txtECDOAYearChi.BackColor = Drawing.Color.Silver
            'Me.txtECDOADayChi.BackColor = Drawing.Color.Silver

            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtECDOAAge.Text = String.Empty
                Me.txtECDOADayEn.Text = String.Empty
                Me.txtECDOAYearEn.Text = String.Empty
                Me.ddlECDOAMonth.Text = String.Empty
                Me.txtECDOAYearChi.Text = String.Empty
                Me.txtECDOADayChi.Text = String.Empty
            End If
        Else
            'Me.txtECDOAAge.BackColor = Drawing.Color.White
            'Me.txtECDOADayEn.BackColor = Drawing.Color.White
            'Me.txtECDOAYearEn.BackColor = Drawing.Color.White
            'Me.ddlECDOAMonth.BackColor = Drawing.Color.White
            'Me.txtECDOAYearChi.BackColor = Drawing.Color.White
            'Me.txtECDOADayChi.BackColor = Drawing.Color.White
        End If

        If MyBase.ActiveViewChanged Then
            Me.SetDOBDateError(False)
            Me.SetDOByearError(False)
            Me.SetDOBTravelDocError(False)
            Me.SetDOBAgeError(False)
        End If
    End Sub


    'For Creation Mode and Modification One Side Mode use only
    Private Sub ECDOARenderLanguage_New(ByVal blnIsEnglish As Boolean)
        'DOA TextBoxs
        Me.txtNewECDOADayChi.Visible = Not blnIsEnglish
        Me.txtNewECDOAYearChi.Visible = Not blnIsEnglish
        Me.txtNewECDOADayEn.Visible = blnIsEnglish
        Me.txtNewECDOAYearEn.Visible = blnIsEnglish

        'DOA Labels
        Me.lblNewECDOAYearChiText.Visible = Not blnIsEnglish
        Me.lblNewECDOAMonthChiText.Visible = Not blnIsEnglish
        Me.lblNewECDOADayChiText.Visible = Not blnIsEnglish

        Me.lblNewECDOAYearEnText.Visible = blnIsEnglish
        Me.lblNewECDOAMonthEnText.Visible = blnIsEnglish
        Me.lblNewECDOADayEnText.Visible = blnIsEnglish

        If blnIsEnglish Then
            Me.lblNewECDOAYearEnText.Text = Me.GetGlobalResourceObject("Text", "Year")
            Me.lblNewECDOADayEnText.Text = Me.GetGlobalResourceObject("Text", "Day")
            Me.lblNewECDOAMonthEnText.Text = Me.GetGlobalResourceObject("Text", "Month")

            If Me.txtNewECDOAYearChi.Text <> String.Empty Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.txtNewECDOAYearEn.Text = AntiXssEncoder.HtmlEncode(txtECDOAYearChi.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If

            If Me.txtNewECDOADayChi.Text <> String.Empty Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.txtNewECDOADayEn.Text = AntiXssEncoder.HtmlEncode(txtECDOADayChi.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If

            Me.BindECDate_New(Me.ddlNewECDOAMonth, Common.Component.CultureLanguage.English)
        Else
            Me.lblNewECDOAYearChiText.Text = Me.GetGlobalResourceObject("Text", "Year")
            Me.lblNewECDOADayChiText.Text = Me.GetGlobalResourceObject("Text", "Day")
            Me.lblNewECDOAMonthChiText.Text = Me.GetGlobalResourceObject("Text", "Month")

            If Me.txtNewECDOAYearEn.Text <> String.Empty Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.txtNewECDOAYearChi.Text = AntiXssEncoder.HtmlEncode(txtECDOAYearEn.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If

            If Me.txtNewECDOADayEn.Text <> String.Empty Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                Me.txtNewECDOADayChi.Text = AntiXssEncoder.HtmlEncode(txtNewECDOADayEn.Text, True)
                ' I-CRE16-003 Fix XSS [End][Lawrence]
            End If

            Me.BindECDate_New(Me.ddlNewECDOAMonth, Common.Component.CultureLanguage.TradChinese)
        End If
    End Sub

    'For Creation Mode and Modification One Side Mode use only
    Private Sub BindECDate_New(ByVal ddlECDate As DropDownList, ByVal strLanguage As String)
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

    'For Creation Mode and Modification One Side Mode use only
    Private Sub SwitchECSearchControl_New(ByVal blnDateChecked As Boolean, ByVal blnYearChecked As Boolean, ByVal blnTravelDocChecked As Boolean, ByVal blnAgeChecked As Boolean)
        'Date
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtNewDOBDate.Enabled = False
        Else
            Me.txtNewDOBDate.Enabled = blnDateChecked
        End If
        If blnDateChecked Then
            'Me.txtNewDOBDate.BackColor = Drawing.Color.White
        Else
            'Me.txtNewDOBDate.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtNewDOBDate.Text = String.Empty
            End If
        End If

        'Year
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtNewDOByear.Enabled = False
        Else
            Me.txtNewDOByear.Enabled = blnYearChecked
        End If
        If blnYearChecked Then
            'Me.txtNewDOByear.BackColor = Drawing.Color.White
        Else
            'Me.txtNewDOByear.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtNewDOByear.Text = String.Empty
            End If
        End If

        'Travel
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtNewDOBtravel.Enabled = False
        Else
            Me.txtNewDOBtravel.Enabled = blnTravelDocChecked
        End If
        If blnTravelDocChecked Then
            'Me.txtNewDOBtravel.BackColor = Drawing.Color.White
        Else
            'Me.txtNewDOBtravel.BackColor = Drawing.Color.Silver
            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtNewDOBtravel.Text = String.Empty
            End If
        End If

        'Age
        If MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
            Me.txtNewECDOAAge.Enabled = False
            Me.txtNewECDOADayEn.Enabled = False
            Me.txtNewECDOAYearEn.Enabled = False
            Me.ddlNewECDOAMonth.Enabled = False
            Me.txtNewECDOAYearChi.Enabled = False
            Me.txtNewECDOADayChi.Enabled = False
        Else
            Me.txtNewECDOAAge.Enabled = blnAgeChecked
            Me.txtNewECDOADayEn.Enabled = blnAgeChecked
            Me.txtNewECDOAYearEn.Enabled = blnAgeChecked
            Me.ddlNewECDOAMonth.Enabled = blnAgeChecked
            Me.txtNewECDOAYearChi.Enabled = blnAgeChecked
            Me.txtNewECDOADayChi.Enabled = blnAgeChecked
        End If
        If Not blnAgeChecked Then
            'Me.txtNewECDOAAge.BackColor = Drawing.Color.Silver
            'Me.txtNewECDOADayEn.BackColor = Drawing.Color.Silver
            'Me.txtNewECDOAYearEn.BackColor = Drawing.Color.Silver
            'Me.ddlNewECDOAMonth.BackColor = Drawing.Color.Silver
            'Me.txtNewECDOAYearChi.BackColor = Drawing.Color.Silver
            'Me.txtNewECDOADayChi.BackColor = Drawing.Color.Silver

            If Not MyBase.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtNewECDOAAge.Text = String.Empty
                Me.txtNewECDOADayEn.Text = String.Empty
                Me.txtNewECDOAYearEn.Text = String.Empty
                Me.ddlNewECDOAMonth.Text = String.Empty
                Me.txtNewECDOAYearChi.Text = String.Empty
                Me.txtNewECDOADayChi.Text = String.Empty
            End If
        Else
            'Me.txtNewECDOAAge.BackColor = Drawing.Color.White
            'Me.txtNewECDOADayEn.BackColor = Drawing.Color.White
            'Me.txtNewECDOAYearEn.BackColor = Drawing.Color.White
            'Me.ddlNewECDOAMonth.BackColor = Drawing.Color.White
            'Me.txtNewECDOAYearChi.BackColor = Drawing.Color.White
            'Me.txtNewECDOADayChi.BackColor = Drawing.Color.White
        End If

        If MyBase.ActiveViewChanged Then
            Me.SetDOBDateError(False)
            Me.SetDOByearError(False)
            Me.SetDOBTravelDocError(False)
            Me.SetDOBAgeError(False)
        End If
    End Sub
#End Region

#Region "Events"

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Mode = BuildMode.Creation Then
            Me._strECDateMonth = Me.ddlNewDOIMonth.SelectedValue

            'Bind the values of DOI drop down list
            Me.ddlNewDOIMonth.DataSource = commfunct.GetMonthSelection(Common.Component.CultureLanguage.English)
            Me.ddlNewDOIMonth.DataValueField = "Value"
            Me.ddlNewDOIMonth.DataTextField = "Display"
            Me.ddlNewDOIMonth.DataBind()
            Me.ddlNewDOIMonth.SelectedValue = _strECDateMonth
        End If

    End Sub

    Protected Sub rbECDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbECDOBDate.CheckedChanged, rbECDOByear.CheckedChanged, rbECDOBtravel.CheckedChanged, rbECDOA.CheckedChanged
        Me.SwitchECSearchControl(Me.rbECDOBDate.Checked, rbECDOByear.Checked, rbECDOBtravel.Checked, rbECDOA.Checked)
    End Sub

    Protected Sub rbNewECDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbNewECDOBDate.CheckedChanged, rbNewECDOByear.CheckedChanged, rbNewECDOBtravel.CheckedChanged, rbNewECDOA.CheckedChanged
        Me.SwitchECSearchControl_New(Me.rbNewECDOBDate.Checked, rbNewECDOByear.Checked, rbNewECDOBtravel.Checked, rbNewECDOA.Checked)
    End Sub
    '---------------------------------------------------------------------------------------

    'For Modification Mode use only
    Protected Sub cboECSerialNoNotProvided_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MyBase.AuditLogEntry.AddDescripton("Previous Serial No.", txtECSerialNo.Text)
        MyBase.AuditLogEntry.AddDescripton("Checked after", IIf(cboECSerialNoNotProvided.Checked, "Y", "N"))
        MyBase.AuditLogEntry.WriteLog(LogID.LOG00094, "Serial No. Not Provided checked")

        _blnSerialNumberNotProvided = cboECSerialNoNotProvided.Checked

        EnableSerialNo(Not cboECSerialNoNotProvided.Checked)
    End Sub

    'For Modification Mode use only
    Private Sub EnableSerialNo(ByVal blnEnable As Boolean)
        If blnEnable = False Then
            txtECSerialNo.Enabled = False
            txtECSerialNo.Text = String.Empty
            txtECSerialNo.BackColor = Drawing.Color.LightGray
        Else
            txtECSerialNo.Enabled = True
            txtECSerialNo.BackColor = Nothing
        End If

    End Sub

    'For Modification Mode use only
    Protected Sub ibtnOtherFormat_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        MyBase.AuditLogEntry.AddDescripton("Previous Reference 1", txtECRefence1.Text)
        MyBase.AuditLogEntry.AddDescripton("Previous Reference 2", txtECRefence2.Text)
        MyBase.AuditLogEntry.AddDescripton("Previous Reference 3", txtECRefence3.Text)
        MyBase.AuditLogEntry.AddDescripton("Previous Reference 4", txtECRefence4.Text)
        MyBase.AuditLogEntry.WriteLog(LogID.LOG00095, "Reference Other Formats clicked")

        EnableReferenceOtherFormat(True)
        'ViewState(VS.ReferenceOtherFormat) = "Y"
        _blnReferenceNoOtherFormat = True

        MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat = True
    End Sub

    'For Modification Mode use only
    Protected Sub ibtnSpecificFormat_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        MyBase.AuditLogEntry.AddDescripton("Previous Reference", txtECRefFree.Text)
        MyBase.AuditLogEntry.WriteLog(LogID.LOG00096, "Reference Specific Format clicked")

        EnableReferenceOtherFormat(False)
        'ViewState(VS.ReferenceOtherFormat) = "N"
        _blnReferenceNoOtherFormat = False

        MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat = False

    End Sub

    'For Modification Mode use only
    Private Sub EnableReferenceOtherFormat(ByVal blnOtherFormat As Boolean)

        txtECRefence1.Visible = Not blnOtherFormat
        txtECRefence2.Visible = Not blnOtherFormat
        txtECRefence3.Visible = Not blnOtherFormat
        txtECRefence4.Visible = Not blnOtherFormat

      

        lblReferenceSep1.Visible = Not blnOtherFormat
        lblReferenceSep2.Visible = Not blnOtherFormat
        lblReferenceSep3.Visible = Not blnOtherFormat
        lblReferenceSep4.Visible = Not blnOtherFormat

        txtECRefFree.Visible = blnOtherFormat

        ibtnOtherFormat.Visible = Not blnOtherFormat
        ibtnSpecificFormat.Visible = blnOtherFormat

        ' Clear the text
        txtECRefence1.Text = String.Empty
        txtECRefence2.Text = String.Empty
        txtECRefence3.Text = String.Empty
        txtECRefence4.Text = String.Empty
        txtECRefFree.Text = String.Empty
    End Sub


    '---------------------------------------------------------------------------------------

    'For Creation Mode and Modification One Side Mode use only
    Protected Sub cboNewECSerialNoNotProvided_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'MyBase.AuditLogEntry.AddDescripton("Previous Serial No.", txtECSerialNo.Text)
        'MyBase.AuditLogEntry.AddDescripton("Checked after", IIf(cboECSerialNoNotProvided.Checked, "Y", "N"))
        'MyBase.AuditLogEntry.WriteLog(LogID.LOG00094, "Serial No. Not Provided checked")

        If Me.Mode = BuildMode.Creation Then
            If cboNewECSerialNoNotProvided.Checked Then
                ViewState(VS.SerialNumberNotProvided) = "Y"
            Else
                ViewState(VS.SerialNumberNotProvided) = "N"
            End If
        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            _blnSerialNumberNotProvided = cboNewECSerialNoNotProvided.Checked
        End If

        EnableSerialNo_New(Not cboNewECSerialNoNotProvided.Checked)
    End Sub

    'For Creation Mode and Modification One Side Mode use only
    Private Sub EnableSerialNo_New(ByVal blnEnable As Boolean)
        If blnEnable = False Then
            txtNewSerialNo.Enabled = False
            txtNewSerialNo.Text = String.Empty
            txtNewSerialNo.BackColor = Drawing.Color.LightGray
        Else
            txtNewSerialNo.Enabled = True
            txtNewSerialNo.BackColor = Nothing
        End If

    End Sub

    'For Creation Mode and Modification One Side Mode use only
    Protected Sub ibtnNewRefNoOtherFormat_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'MyBase.AuditLogEntry.AddDescripton("Previous Reference 1", txtECRefence1.Text)
        'MyBase.AuditLogEntry.AddDescripton("Previous Reference 2", txtECRefence2.Text)
        'MyBase.AuditLogEntry.AddDescripton("Previous Reference 3", txtECRefence3.Text)
        'MyBase.AuditLogEntry.AddDescripton("Previous Reference 4", txtECRefence4.Text)
        'MyBase.AuditLogEntry.WriteLog(LogID.LOG00095, "Reference Other Formats clicked")

        EnableReferenceOtherFormat_New(True)

        'If Mode = BuildMode.Creation Then
        ViewState(VS.ReferenceOtherFormat) = "Y"
        'End If
        _blnReferenceNoOtherFormat = True

        'MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat = True
    End Sub

    'For Creation Mode and Modification One Side Mode use only
    Protected Sub ibtnNewRefMoSpecificFormat_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'MyBase.AuditLogEntry.AddDescripton("Previous Reference", txtECRefFree.Text)
        'MyBase.AuditLogEntry.WriteLog(LogID.LOG00096, "Reference Specific Format clicked")

        EnableReferenceOtherFormat_New(False)

        'If Mode = BuildMode.Creation Then
        ViewState(VS.ReferenceOtherFormat) = "N"
        'End If
        _blnReferenceNoOtherFormat = False

        'MyBase.EHSPersonalInfoAmend.ECReferenceNoOtherFormat = False

    End Sub

    'For Creation Mode and Modification One Side Mode use only
    Private Sub EnableReferenceOtherFormat_New(ByVal blnOtherFormat As Boolean)

        txtNewRefNo1.Visible = Not blnOtherFormat
        txtNewRefNo2.Visible = Not blnOtherFormat
        txtNewRefNo3.Visible = Not blnOtherFormat
        txtNewRefNo4.Visible = Not blnOtherFormat

        lblNewRefSep1.Visible = Not blnOtherFormat
        lblNewRefSep2.Visible = Not blnOtherFormat
        lblNewRefSep3.Visible = Not blnOtherFormat
        lblNewRefSep4.Visible = Not blnOtherFormat

        txtNewRefNoFree.Visible = blnOtherFormat

        ibtnNewRefNoOtherFormat.Visible = Not blnOtherFormat
        ibtnNewRefMoSpecificFormat.Visible = blnOtherFormat

        ' Clear the text
        txtNewRefNo1.Text = String.Empty
        txtNewRefNo2.Text = String.Empty
        txtNewRefNo3.Text = String.Empty
        txtNewRefNo4.Text = String.Empty
        txtNewRefNoFree.Text = String.Empty
    End Sub


#End Region

    Public Sub CleanField()
        Me.txtECDOAAge.Text = String.Empty
        Me.txtECDOADayChi.Text = String.Empty
        Me.txtECDOADayEn.Text = String.Empty
        Me.txtECDOAYearChi.Text = String.Empty
        Me.txtECDOAYearEn.Text = String.Empty

        Me.txtDOBDate.Text = String.Empty
        Me.txtDOByear.Text = String.Empty
        Me.txtDOBtravel.Text = String.Empty


        rbECDOBDate.Checked = True
        rbECDOByear.Checked = False
        rbECDOBtravel.Checked = False
        rbECDOA.Checked = False
        Me.ddlECDOAMonth.SelectedIndex = -1
    End Sub

End Class