Imports Common.Component.EHSAccount
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.Component.CCCode

Public Class ucInputROP140
    Inherits ucInputDocTypeBase

    'Values
    Public Event SelectChineseName(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SelectChineseName_CreateMode(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    'For Original Record
    Private _strTDNumber As String
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strCCCode1 As String
    Private _strCCCode2 As String
    Private _strCCCode3 As String
    Private _strCCCode4 As String
    Private _strCCCode5 As String
    Private _strCCCode6 As String
    Private _strGender As String
    Private _strDOB As String
    Private _strDOI As String

    'For Amending Record
    Private _strTDNumberAmend As String
    Private _strENameFirstNameAmend As String
    Private _strENameSurNameAmend As String
    Private _strCNameAmend As String
    Private _strCCCode1Amend As String
    Private _strCCCode2Amend As String
    Private _strCCCode3Amend As String
    Private _strCCCode4Amend As String
    Private _strCCCode5Amend As String
    Private _strCCCode6Amend As String
    Private _strGenderAmend As String
    Private _strDOBAmend As String
    Private _strDOIAmend As String

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
    Private _eHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)

        'Table title
        Me.lblDocumentTypeOriginalText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")
        Me.lblNameOrignialText.Text = Me.GetGlobalResourceObject("Text", "Name")
        Me.lblGenderOriginalText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblDOBOriginalText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
        Me.btnSearchCCCode.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChineseNameSBtn")
        Me.btnSearchCCCode.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChineseNameBtn")

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
        Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgTDNo.ImageUrl = strErrorImageURL
        Me.imgTDNo.AlternateText = strErrorImageALT

        Me.imgEName.ImageUrl = strErrorImageURL
        Me.imgEName.AlternateText = strErrorImageALT

        Me.imgGender.ImageUrl = strErrorImageURL
        Me.imgGender.AlternateText = strErrorImageALT

        Me.imgDOBDate.ImageUrl = strErrorImageURL
        Me.imgDOBDate.AlternateText = strErrorImageALT

        Me.imgDOIDate.ImageUrl = strErrorImageURL
        Me.imgDOIDate.AlternateText = strErrorImageALT

        Me.imgCCCodeError.ImageUrl = strErrorImageURL
        Me.imgCCCodeError.AlternateText = strErrorImageALT

        'Get Documnet type full name
        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocTypeModelCollection
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()
        ' ---------------------------------------------------------------------------------------------------------
        Me.lblDocumentTypeOriginal.Text = udtDocTypeModelList.Filter(DocTypeCode.ROP140).DocName(Session("Language").ToString())
        Me.lblTravelDocNoOriginalText.Text = udtDocTypeModelList.Filter(DocTypeCode.ROP140).DocIdentityDesc(Session("Language").ToString())

        ' -------------------------- Creation ------------------------------
        Me.lblNewTravelDocNoText.Text = udtDocTypeModelList.Filter(DocTypeCode.ROP140).DocIdentityDesc(Session("Language").ToString())

        Me.lblNewNameText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
        Me.lblNewCCCodeText.Text = Me.GetGlobalResourceObject("Text", "CCCODE")
        Me.lblNewCNameText.Text = Me.GetGlobalResourceObject("Text", "ChineseName")
        Me.lblNewENameSurnameTips.Text = Me.GetGlobalResourceObject("Text", "Surname")
        Me.lblNewENameGivenNameTips.Text = Me.GetGlobalResourceObject("Text", "Givenname")
        Me.lblNewGenderText.Text = Me.GetGlobalResourceObject("Text", "Gender")
        Me.lblNewDOBText.Text = Me.GetGlobalResourceObject("Text", "DOB")
        Me.lblNewEnameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")
        Me.lblNewDOIText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")

        'Gender Radio button list
        Me.rboNewGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
        Me.rboNewGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        'error message
        Me.imgNewTravelDocNoErr.ImageUrl = strErrorImageURL
        Me.imgNewTravelDocNoErr.AlternateText = strErrorImageALT

        Me.imgNewENameErr.ImageUrl = strErrorImageURL
        Me.imgNewENameErr.AlternateText = strErrorImageALT

        Me.imgNewGenderErr.ImageUrl = strErrorImageURL
        Me.imgNewGenderErr.AlternateText = strErrorImageALT

        Me.imgNewDOBErr.ImageUrl = strErrorImageURL
        Me.imgNewDOBErr.AlternateText = strErrorImageALT

        Me.imgNewDOIErr.ImageUrl = strErrorImageURL
        Me.imgNewDOIErr.AlternateText = strErrorImageALT
        ' ------------------------------------------------------------------
    End Sub

    Protected Overrides Sub Setup(ByVal modeType As ucInputDocTypeBase.BuildMode)

        Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Me.pnlNew.Visible = False
        Me.pnlModify.Visible = False

        If Me.Mode = BuildMode.Creation Then
            '--------------------------------------------------------
            'For Creation Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If

            '-------temporary code------------------------------
            If Not IsNothing(MyBase.EHSPersonalInfoAmend) Then
                'Pre-Fill before entering account creation page (eHS maintenance)
                Me._strTDNumber = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
                Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
                If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                    Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.DateofIssue))

                Else
                    Me._strDOI = String.Empty
                End If


                Me.txtNewTravelDocNo.Enabled = False
                SetValue(BuildMode.Creation)
            End If

        ElseIf Me.Mode = BuildMode.Modification_OneSide Then
            '--------------------------------------------------------
            'For Modification (One Side) Mode
            '--------------------------------------------------------
            Me.pnlNew.Visible = True

            Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

            If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.DateofIssue))
            Else
                Me._strDOI = String.Empty
            End If

            'CName may assiged for display only
            'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
            Me._strCName = MyBase.EHSPersonalInfoAmend.CName

            If Not MyBase.EHSPersonalInfoAmend.CCCode1 Is Nothing Then
                Me._strCCCode1 = MyBase.EHSPersonalInfoAmend.CCCode1.Trim()
            End If

            If Not MyBase.EHSPersonalInfoAmend.CCCode2 Is Nothing Then
                Me._strCCCode2 = MyBase.EHSPersonalInfoAmend.CCCode2.Trim()
            End If

            If Not MyBase.EHSPersonalInfoAmend.CCCode3 Is Nothing Then
                Me._strCCCode3 = MyBase.EHSPersonalInfoAmend.CCCode3.Trim()
            End If

            If Not MyBase.EHSPersonalInfoAmend.CCCode4 Is Nothing Then
                Me._strCCCode4 = MyBase.EHSPersonalInfoAmend.CCCode4.Trim()
            End If

            If Not MyBase.EHSPersonalInfoAmend.CCCode5 Is Nothing Then
                Me._strCCCode5 = MyBase.EHSPersonalInfoAmend.CCCode5.Trim()
            End If

            If Not MyBase.EHSPersonalInfoAmend.CCCode6 Is Nothing Then
                Me._strCCCode6 = MyBase.EHSPersonalInfoAmend.CCCode6.Trim()
            End If


            'Fill values
            Me.SetValue(Mode)

            Me.txtNewTravelDocNo.Visible = False
            Me.lblNewTravelDocNo.Visible = True

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If
        Else
            '--------------------------------------------------------
            'For Modification Mode (AND) Modify Read Only Mode
            '--------------------------------------------------------

            'Me._strTDNumber = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            'Me._strENameFirstName = MyBase.EHSPersonalInfoAmend.ENameFirstName
            'Me._strENameSurName = MyBase.EHSPersonalInfoAmend.ENameSurName
            'Me._strGender = MyBase.EHSPersonalInfoAmend.Gender
            'Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("Language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)
            'If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
            '    Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.DateofIssue))
            'Else
            '    Me._strDOI = String.Empty
            'End If


            ''CName may assiged for display only
            ''CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
            'Me._strCName = MyBase.EHSPersonalInfoAmend.CName

            'If Not MyBase.EHSPersonalInfoAmend.CCCode1 Is Nothing Then
            '    Me._strCCCode1 = MyBase.EHSPersonalInfoAmend.CCCode1.Trim()
            'End If

            'If Not MyBase.EHSPersonalInfoAmend.CCCode2 Is Nothing Then
            '    Me._strCCCode2 = MyBase.EHSPersonalInfoAmend.CCCode2.Trim()
            'End If

            'If Not MyBase.EHSPersonalInfoAmend.CCCode3 Is Nothing Then
            '    Me._strCCCode3 = MyBase.EHSPersonalInfoAmend.CCCode3.Trim()
            'End If

            'If Not MyBase.EHSPersonalInfoAmend.CCCode4 Is Nothing Then
            '    Me._strCCCode4 = MyBase.EHSPersonalInfoAmend.CCCode4.Trim()
            'End If

            'If Not MyBase.EHSPersonalInfoAmend.CCCode5 Is Nothing Then
            '    Me._strCCCode5 = MyBase.EHSPersonalInfoAmend.CCCode5.Trim()
            'End If

            'If Not MyBase.EHSPersonalInfoAmend.CCCode6 Is Nothing Then
            '    Me._strCCCode6 = MyBase.EHSPersonalInfoAmend.CCCode6.Trim()
            'End If

            'Add Client Event--------------------------------------------------------------------------------------------
            Me.txtCCCode1.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode2.ClientID + ",4 );")
            Me.txtCCCode2.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode3.ClientID + ",4 );")
            Me.txtCCCode3.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode4.ClientID + ",4 );")
            Me.txtCCCode4.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode5.ClientID + ",4 );")
            Me.txtCCCode5.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode6.ClientID + ",4 );")

            '----------------------------------- For Original Record ----------------------------------------------------
            Me.pnlModify.Visible = True

            Me._strTDNumber = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
            Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, Session("language"), MyBase.EHSPersonalInfoOriginal.ECAge, MyBase.EHSPersonalInfoOriginal.ECDateOfRegistration)

            If MyBase.UpdateValue Then
                Me._strENameFirstName = MyBase.EHSPersonalInfoOriginal.ENameFirstName
                Me._strENameSurName = MyBase.EHSPersonalInfoOriginal.ENameSurName
                Me._strGender = MyBase.EHSPersonalInfoOriginal.Gender

                'CName may assiged for display only
                'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
                Me._strCName = MyBase.EHSPersonalInfoOriginal.CName

                If MyBase.EHSPersonalInfoOriginal.DateofIssue.HasValue Then
                    Me._strDOI = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoOriginal.DateofIssue))
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode1 Is Nothing Then
                    Me._strCCCode1 = MyBase.EHSPersonalInfoOriginal.CCCode1.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode2 Is Nothing Then
                    Me._strCCCode2 = MyBase.EHSPersonalInfoOriginal.CCCode2.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode3 Is Nothing Then
                    Me._strCCCode3 = MyBase.EHSPersonalInfoOriginal.CCCode3.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode4 Is Nothing Then
                    Me._strCCCode4 = MyBase.EHSPersonalInfoOriginal.CCCode4.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode5 Is Nothing Then
                    Me._strCCCode5 = MyBase.EHSPersonalInfoOriginal.CCCode5.Trim()
                End If

                If Not MyBase.EHSPersonalInfoOriginal.CCCode6 Is Nothing Then
                    Me._strCCCode6 = MyBase.EHSPersonalInfoOriginal.CCCode6.Trim()
                End If
            End If

            '----------------------------------- For Amending Record ----------------------------------------------------
            Me.pnlModify.Visible = True
            Me._strTDNumberAmend = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfoAmend.IdentityNum, False)
            Me._strDOBAmend = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoAmend.DOB, MyBase.EHSPersonalInfoAmend.ExactDOB, Session("language"), MyBase.EHSPersonalInfoAmend.ECAge, MyBase.EHSPersonalInfoAmend.ECDateOfRegistration)

            If MyBase.UpdateValue Then
                Me._strENameFirstNameAmend = MyBase.EHSPersonalInfoAmend.ENameFirstName
                Me._strENameSurNameAmend = MyBase.EHSPersonalInfoAmend.ENameSurName
                Me._strGenderAmend = MyBase.EHSPersonalInfoAmend.Gender

                'CName may assiged for display only
                'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
                Me._strCNameAmend = MyBase.EHSPersonalInfoAmend.CName

                If MyBase.EHSPersonalInfoAmend.DateofIssue.HasValue Then
                    Me._strDOIAmend = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoAmend.DateofIssue))
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode1 Is Nothing Then
                    Me._strCCCode1Amend = MyBase.EHSPersonalInfoAmend.CCCode1.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode2 Is Nothing Then
                    Me._strCCCode2Amend = MyBase.EHSPersonalInfoAmend.CCCode2.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode3 Is Nothing Then
                    Me._strCCCode3Amend = MyBase.EHSPersonalInfoAmend.CCCode3.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode4 Is Nothing Then
                    Me._strCCCode4Amend = MyBase.EHSPersonalInfoAmend.CCCode4.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode5 Is Nothing Then
                    Me._strCCCode5Amend = MyBase.EHSPersonalInfoAmend.CCCode5.Trim()
                End If

                If Not MyBase.EHSPersonalInfoAmend.CCCode6 Is Nothing Then
                    Me._strCCCode6Amend = MyBase.EHSPersonalInfoAmend.CCCode6.Trim()
                End If
            End If
            '---------------------------------------------------------------------------------------------------------------------

            Me.SetValue(modeType)

            'Mode related Settings

            Me.lblTDNo.Visible = True
            Me.txtTDNo.Visible = False

            If modeType = ucInputDocTypeBase.BuildMode.Modification Then
                '--------------------------------------------------------
                'Modification Mode
                '--------------------------------------------------------
                Me.txtDOB.Enabled = True
                Me.txtENameSurname.Enabled = True
                Me.txtENameFirstname.Enabled = True
                Me.rbGender.Enabled = True
                Me.txtDOI.Enabled = True
                Me.txtCCCode1.Enabled = True
                Me.txtCCCode2.Enabled = True
                Me.txtCCCode3.Enabled = True
                Me.txtCCCode4.Enabled = True
                Me.txtCCCode5.Enabled = True
                Me.txtCCCode6.Enabled = True
                Me.btnSearchCCCode.Visible = True
            Else
                '--------------------------------------------------------
                'Modify Read Only Mode
                'ReadOnly, 2 sides showing both original and amending record 
                '--------------------------------------------------------
                Me.txtENameSurname.Enabled = False
                Me.txtENameFirstname.Enabled = False
                Me.rbGender.Enabled = False
                Me.txtDOB.Enabled = False
                Me.txtDOI.Enabled = False
                Me.txtCCCode1.Enabled = False
                Me.txtCCCode2.Enabled = False
                Me.txtCCCode3.Enabled = False
                Me.txtCCCode4.Enabled = False
                Me.txtCCCode5.Enabled = False
                Me.txtCCCode6.Enabled = False
                Me.btnSearchCCCode.Visible = False
            End If

            If MyBase.ActiveViewChanged Then
                Me.SetErrorImage(Mode, False)
            End If
        End If

    End Sub

#Region "Set Up Text Box Value"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            SetTDNumber()
            SetENameFirstName()
            SetENameSurName()
            SetDOB()
            SetDOI()
            SetCName()
        Else
            SetTDNumber()
            SetENameFirstName()
            SetENameSurName()
            SetGender()
            SetDOB()
            SetDOI()
            'SetCName()
            If MyBase.ActiveViewChanged Then
                Me.SetCName()
            End If
        End If

    End Sub

    Public Sub SetCName()
        If Mode = BuildMode.Creation Then
            'Me.lblNewCName.Text = String.Empty
            If Not IsNothing(_strCName) Then
                Me.lblNewCName.Text = _strCName
            End If

        ElseIf Mode = BuildMode.Modification_OneSide Then

            Dim strDBCName As String = String.Empty
            Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL

            If Not Me._strCCCode1 Is Nothing Then
                If Me._strCCCode1.Length > 4 Then
                    Me.txtNewCCCode1.Text = Me._strCCCode1.Substring(0, 4)
                Else
                    Me.txtNewCCCode1.Text = String.Empty
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode1)
            End If

            If Not Me._strCCCode2 Is Nothing Then
                If Me._strCCCode2.Trim().Length > 4 Then
                    Me.txtNewCCCode2.Text = Me._strCCCode2.Substring(0, 4)
                Else
                    Me.txtNewCCCode2.Text = String.Empty
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode2)
            End If

            If Not Me._strCCCode3 Is Nothing Then
                If Me._strCCCode3.Length > 4 Then
                    Me.txtNewCCCode3.Text = Me._strCCCode3.Substring(0, 4)
                Else
                    Me.txtNewCCCode3.Text = String.Empty
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode3)
            End If

            If Not Me._strCCCode4 Is Nothing Then
                If Me._strCCCode4.Length > 4 Then
                    Me.txtNewCCCode4.Text = Me._strCCCode4.Substring(0, 4)
                Else
                    Me.txtNewCCCode4.Text = String.Empty
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode4)
            End If

            If Not Me._strCCCode5 Is Nothing Then
                If Me._strCCCode5.Length > 4 Then
                    Me.txtNewCCCode5.Text = Me._strCCCode5.Substring(0, 4)
                Else
                    Me.txtNewCCCode5.Text = String.Empty
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode5)
            End If

            If Not Me._strCCCode6 Is Nothing Then
                If Me._strCCCode6.Length > 4 Then
                    Me.txtNewCCCode6.Text = Me._strCCCode6.Substring(0, 4)
                Else
                    Me.txtNewCCCode6.Text = String.Empty
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode6)
            End If

            If strDBCName = String.Empty Then
                If Not MyBase.EHSPersonalInfoOriginal.CName.Equals(String.Empty) Then
                    Me.lblNewCName.Text = MyBase.EHSPersonalInfoOriginal.CName
                    Me._strCName = MyBase.EHSPersonalInfoOriginal.CName
                End If
            Else
                Me._strCName = strDBCName
                Me.lblNewCName.Text = strDBCName
            End If

        Else
            'Amending Record
            Dim strDBCNameAmend As String = String.Empty
            Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL

            If Not Me._strCCCode1Amend Is Nothing Then
                If Me._strCCCode1Amend.Length > 4 Then
                    Me.txtCCCode1.Text = Me._strCCCode1Amend.Substring(0, 4)
                Else
                    Me.txtCCCode1.Text = String.Empty
                End If

                strDBCNameAmend += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode1Amend)
            End If


            If Not Me._strCCCode2Amend Is Nothing Then
                If Me._strCCCode2Amend.Length > 4 Then
                    Me.txtCCCode2.Text = Me._strCCCode2Amend.Substring(0, 4)
                Else
                    Me.txtCCCode2.Text = String.Empty
                End If

                strDBCNameAmend += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode2Amend)
            End If

            If Not Me._strCCCode3Amend Is Nothing Then
                If Me._strCCCode3Amend.Length > 4 Then
                    Me.txtCCCode3.Text = Me._strCCCode3Amend.Substring(0, 4)
                Else
                    Me.txtCCCode3.Text = String.Empty
                End If

                strDBCNameAmend += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode3Amend)
            End If

            If Not Me._strCCCode4Amend Is Nothing Then
                If Me._strCCCode4Amend.Length > 4 Then
                    Me.txtCCCode4.Text = Me._strCCCode4Amend.Substring(0, 4)
                Else
                    Me.txtCCCode4.Text = String.Empty
                End If

                strDBCNameAmend += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode4Amend)
            End If

            If Not Me._strCCCode5Amend Is Nothing Then
                If Me._strCCCode5Amend.Length > 4 Then
                    Me.txtCCCode5.Text = Me._strCCCode5Amend.Substring(0, 4)
                Else
                    Me.txtCCCode5.Text = String.Empty
                End If

                strDBCNameAmend += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode5Amend)
            End If

            If Not Me._strCCCode6Amend Is Nothing Then
                If Me._strCCCode6Amend.Length > 4 Then
                    Me.txtCCCode6.Text = Me._strCCCode6Amend.Substring(0, 4)
                Else
                    Me.txtCCCode6.Text = String.Empty
                End If

                strDBCNameAmend += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode6Amend)
            End If

            If strDBCNameAmend = String.Empty Then

                Me.lblCName.Text = MyBase.EHSPersonalInfoAmend.CName
            Else
                Me._strCNameAmend = strDBCNameAmend
                Me.lblCName.Text = strDBCNameAmend
            End If

            'ReadOnly (Original Record)
            'Amending Record
            Dim strDBCName As String = String.Empty
            Dim strCNameCode As String = String.Empty

            If Not Me._strCCCode1 Is Nothing Then
                If Me._strCCCode1.Length > 4 Then
                    strCNameCode += Me._strCCCode1.Substring(0, 4) + " "
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode1)
            End If

            If Not Me._strCCCode2 Is Nothing Then
                If Me._strCCCode2.Length > 4 Then
                    strCNameCode += Me._strCCCode2.Substring(0, 4) + " "
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode2)
            End If

            If Not Me._strCCCode3 Is Nothing Then
                If Me._strCCCode3.Length > 4 Then
                    strCNameCode += Me._strCCCode3.Substring(0, 4) + " "
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode3)
            End If

            If Not Me._strCCCode4 Is Nothing Then
                If Me._strCCCode4.Length > 4 Then
                    strCNameCode += Me._strCCCode4.Substring(0, 4) + " "
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode4)
            End If

            If Not Me._strCCCode5 Is Nothing Then
                If Me._strCCCode5.Length > 4 Then
                    strCNameCode += Me._strCCCode5.Substring(0, 4) + " "
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode5)
            End If

            If Not Me._strCCCode6 Is Nothing Then
                If Me._strCCCode6.Length > 4 Then
                    strCNameCode += Me._strCCCode6.Substring(0, 4) + " "
                End If

                strDBCName += udtCCCodeBLL.getChiCharByCCCode(Me._strCCCode6)
            End If

            Me.lblCCCodeOrginal.Text = strCNameCode
            Me.lblCNameOriginal.Text = strDBCName

        End If

    End Sub

    'Public Sub SetCName(ByVal strCName As String)
    '    If Mode = BuildMode.Creation Or Mode = BuildMode.Modification_OneSide Then
    '        Me.lblNewCName.Text = strCName
    '    Else
    '        Me.lblCName.Text = strCName
    '    End If
    '    _strCName = strCName
    'End Sub

    'Update Chinese name in Amending record only
    Public Sub SetCnameAmend(ByVal strCName As String)

        If Mode = BuildMode.Creation Or Mode = BuildMode.Modification_OneSide Then
            Me.lblNewCName.Text = strCName
        Else
            Me.lblCName.Text = strCName
        End If

        _strCNameAmend = strCName
        _strCName = strCName
    End Sub

    Public Sub SetTDNumber()
        If Mode = BuildMode.Creation Then
            txtNewTravelDocNo.Text = Me._strTDNumber

        ElseIf Mode = BuildMode.Modification_OneSide Then
            lblNewTravelDocNo.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
        Else
            'Amend
            Me.txtTDNo.Text = Me._strTDNumber
            Me.lblTDNo.Text = Me._strTDNumber
            'original
            lblTravelDocNoOriginal.Text = MyBase.Formatter.FormatDocIdentityNoForDisplay(DocTypeCode.ROP140, MyBase.EHSPersonalInfoOriginal.IdentityNum, False)
        End If

    End Sub

    Public Sub SetENameFirstName()
        If Mode = BuildMode.Creation Then
            Me.txtNewGivenName.Text = Me._strENameFirstName

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewGivenName.Text = Me._strENameFirstName

        Else
            'Amend
            Me.txtENameFirstname.Text = Me._strENameFirstNameAmend
            'Original
            Me.lblNameOriginal.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfoOriginal.ENameSurName, MyBase.EHSPersonalInfoOriginal.ENameFirstName)
        End If

    End Sub

    Public Sub SetENameSurName()

        If Mode = BuildMode.Creation Then
            Me.txtNewSurname.Text = Me._strENameSurName
        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewSurname.Text = Me._strENameSurName
        Else
            'Amend
            Me.txtENameSurname.Text = Me._strENameSurNameAmend
        End If

    End Sub

    Public Sub SetGender()

        If Mode = BuildMode.Creation Then
            Me.rboNewGender.ClearSelection()

        ElseIf Mode = BuildMode.Modification_OneSide Then
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rboNewGender.SelectedValue = Me._strGender
            End If

        Else
            'Amend
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.rbGender.SelectedValue = Me._strGenderAmend
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

    Public Sub SetDOB()

        If Mode = BuildMode.Creation Then
            Me.txtNewDOB.Text = Me._strDOB

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOB.Text = Me._strDOB

        Else
            'Amend
            Me.txtDOB.Text = Me._strDOBAmend
            'Orignal
            Me.lblDOBOriginal.Text = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfoOriginal.DOB, MyBase.EHSPersonalInfoOriginal.ExactDOB, String.Empty, Nothing, Nothing)
        End If

    End Sub

    Public Sub SetDOI()

        If Mode = BuildMode.Creation Then
            Me.txtNewDOI.Text = Me._strDOI

        ElseIf Mode = BuildMode.Modification_OneSide Then
            Me.txtNewDOI.Text = Me._strDOI
        Else
            'Amend
            Me.txtDOI.Text = Me._strDOIAmend
            'Original
            If MyBase.EHSPersonalInfoOriginal.DateofIssue.HasValue Then
                Me.lblDOIOriginal.Text = MyBase.Formatter.formatInputDate(CDate(MyBase.EHSPersonalInfoOriginal.DateofIssue))
            Else
                Me.lblDOIOriginal.Text = String.Empty
            End If
        End If
    End Sub

    Public Function CCCodeIsEmpty() As Boolean

        If Mode = BuildMode.Creation Then
            If Me.txtNewCCCode1.Text = String.Empty AndAlso _
                Me.txtNewCCCode2.Text = String.Empty AndAlso _
                Me.txtNewCCCode3.Text = String.Empty AndAlso _
                Me.txtNewCCCode4.Text = String.Empty AndAlso _
                Me.txtNewCCCode5.Text = String.Empty AndAlso _
                Me.txtNewCCCode6.Text = String.Empty Then
                Return True
            Else
                Return False
            End If

        ElseIf Mode = BuildMode.Modification_OneSide Then
            If Me.txtNewCCCode1.Text = String.Empty AndAlso _
                Me.txtNewCCCode2.Text = String.Empty AndAlso _
                Me.txtNewCCCode3.Text = String.Empty AndAlso _
                Me.txtNewCCCode4.Text = String.Empty AndAlso _
                Me.txtNewCCCode5.Text = String.Empty AndAlso _
                Me.txtNewCCCode6.Text = String.Empty Then
                Return True
            Else
                Return False
            End If
        Else
            'Amend
            If Me.txtCCCode1.Text = String.Empty AndAlso _
                Me.txtCCCode2.Text = String.Empty AndAlso _
                Me.txtCCCode3.Text = String.Empty AndAlso _
                Me.txtCCCode4.Text = String.Empty AndAlso _
                Me.txtCCCode5.Text = String.Empty AndAlso _
                Me.txtCCCode6.Text = String.Empty Then
                Return True
            Else
                Return False
            End If
        End If

    End Function


#End Region

#Region "Set Up Error Image"

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        Me.SetTDError(visible)
        Me.SetCCCodeError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetDOBError(visible)
        Me.SetDOIError(visible)
    End Sub

    Public Sub SetCCCodeError(ByVal visible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewCCCodeErr.Visible = visible
        Else
            Me.imgCCCodeError.Visible = visible
        End If
    End Sub

    Public Sub SetTDError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewTravelDocNoErr.Visible = blnVisible
        Else
            Me.imgTDNo.Visible = blnVisible
        End If
    End Sub

    Public Sub SetENameError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            Me.imgNewENameErr.Visible = blnVisible
        Else
            Me.imgEName.Visible = blnVisible
        End If

    End Sub

    Public Sub SetGenderError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewGenderErr.Visible = blnVisible
        Else
            Me.imgGender.Visible = blnVisible
        End If
    End Sub

    Public Sub SetDOBError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewDOBErr.Visible = blnVisible
        Else
            Me.imgDOBDate.Visible = blnVisible
        End If

    End Sub

    Public Sub SetDOIError(ByVal blnVisible As Boolean)
        If Me.Mode = BuildMode.Creation Or Me.Mode = BuildMode.Modification_OneSide Then
            imgNewDOIErr.Visible = blnVisible
        Else
            Me.imgDOIDate.Visible = blnVisible
        End If
    End Sub



#End Region

#Region "Events"
    '--------------------------------------------------------------------------------------------------------------
    'Events
    '--------------------------------------------------------------------------------------------------------------
    Protected Sub btnSearchCCCode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchCCCode.Click
        RaiseEvent SelectChineseName(sender, e)
        RaiseEvent SelectChineseName_CreateMode(ucInputDocTypeBase.BuildMode.Modification, sender, e)
    End Sub

    Protected Sub ibtnNewCCCode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNewCCCode.Click
        RaiseEvent SelectChineseName_CreateMode(ucInputDocTypeBase.BuildMode.Creation, sender, e)
    End Sub

#End Region

#Region "Property"

    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        If mode = BuildMode.Creation Then
            Me._strTDNumber = Me.txtNewTravelDocNo.Text.Trim.Replace("(", "").Replace(")", "")
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOI = Me.txtNewDOI.Text.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim

            Me._strCName = Me.lblNewCName.Text
            Me._strCCCode1 = Me.txtNewCCCode1.Text.Trim
            Me._strCCCode2 = Me.txtNewCCCode2.Text.Trim
            Me._strCCCode3 = Me.txtNewCCCode3.Text.Trim
            Me._strCCCode4 = Me.txtNewCCCode4.Text.Trim
            Me._strCCCode5 = Me.txtNewCCCode5.Text.Trim
            Me._strCCCode6 = Me.txtNewCCCode6.Text.Trim

        ElseIf mode = BuildMode.Modification_OneSide Then
            Me._strENameFirstName = Me.txtNewGivenName.Text.Trim
            Me._strENameSurName = Me.txtNewSurname.Text.Trim
            Me._strGender = Me.rboNewGender.SelectedValue.Trim
            Me._strDOI = Me.txtNewDOI.Text.Trim
            Me._strDOB = Me.txtNewDOB.Text.Trim


            Me._strCName = Me.lblNewCName.Text
            Me._strCCCode1 = Me.txtNewCCCode1.Text.Trim
            Me._strCCCode2 = Me.txtNewCCCode2.Text.Trim
            Me._strCCCode3 = Me.txtNewCCCode3.Text.Trim
            Me._strCCCode4 = Me.txtNewCCCode4.Text.Trim
            Me._strCCCode5 = Me.txtNewCCCode5.Text.Trim
            Me._strCCCode6 = Me.txtNewCCCode6.Text.Trim
        Else
            Me._strENameFirstName = Me.txtENameFirstname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strGender = Me.rbGender.SelectedValue.Trim
            Me._strDOI = Me.txtDOI.Text.Trim
            Me._strDOB = Me.txtDOB.Text.Trim

            Me._strCName = Me.lblCName.Text
            Me._strCCCode1 = Me.txtCCCode1.Text.Trim
            Me._strCCCode2 = Me.txtCCCode2.Text.Trim
            Me._strCCCode3 = Me.txtCCCode3.Text.Trim
            Me._strCCCode4 = Me.txtCCCode4.Text.Trim
            Me._strCCCode5 = Me.txtCCCode5.Text.Trim
            Me._strCCCode6 = Me.txtCCCode6.Text.Trim
        End If

    End Sub

    Public Property TravelDocNo() As String
        Get
            Return Me._strTDNumber
        End Get
        Set(ByVal value As String)
            Me._strTDNumber = value
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

    Public Property Gender() As String
        Get
            Return Me._strGender
        End Get
        Set(ByVal value As String)
            Me._strGender = value
        End Set
    End Property

    Public Property DOB() As String
        Get
            Return Me._strDOB
        End Get
        Set(ByVal value As String)
            Me._strDOB = value
        End Set
    End Property

    Public Property DateOfIssue() As String
        Get
            Return Me._strDOI
        End Get
        Set(ByVal value As String)
            Me._strDOI = value
        End Set
    End Property

    Public Property CCCode1() As String
        Get
            Return Me._strCCCode1
        End Get
        Set(ByVal value As String)
            Me._strCCCode1 = value.Trim()
        End Set
    End Property

    Public Property CCCode2() As String
        Get
            Return Me._strCCCode2
        End Get
        Set(ByVal value As String)
            Me._strCCCode2 = value.Trim()
        End Set
    End Property

    Public Property CCCode3() As String
        Get
            Return Me._strCCCode3
        End Get
        Set(ByVal value As String)
            Me._strCCCode3 = value.Trim()
        End Set
    End Property

    Public Property CCCode4() As String
        Get
            Return Me._strCCCode4
        End Get
        Set(ByVal value As String)
            Me._strCCCode4 = value.Trim()
        End Set
    End Property

    Public Property CCCode5() As String
        Get
            Return Me._strCCCode5
        End Get
        Set(ByVal value As String)
            Me._strCCCode5 = value.Trim()
        End Set
    End Property

    Public Property CCCode6() As String
        Get
            Return Me._strCCCode6
        End Get
        Set(ByVal value As String)
            Me._strCCCode6 = value.Trim()
        End Set
    End Property

    Public Property CName() As String
        Get
            Return Me._strCName
        End Get
        Set(ByVal value As String)
            Me._strCName = value.Trim()
        End Set
    End Property

#End Region


#Region "Supported Function"
    Public Function GetCCCode(ByVal strInputCCCode As String, ByVal strExistCCCode As String) As String

        If strInputCCCode Is Nothing Then
            Return String.Empty
        End If

        If Not strInputCCCode.Equals(String.Empty) AndAlso strInputCCCode.Length >= 4 Then

            'check session CCCode exist
            If Not strExistCCCode Is Nothing AndAlso Not strExistCCCode.Equals(String.Empty) AndAlso strExistCCCode.Length > 4 Then
                'check if code head match
                If strInputCCCode.Substring(0, 4) = strExistCCCode.Substring(0, 4) Then
                    Return strExistCCCode
                End If
            End If
        End If

        Return strInputCCCode

    End Function

    Public Function IsValidCCCodeInput() As Boolean
        Return (Me.txtCCCode1.Text.Length = 4 OrElse Me.txtCCCode1.Text.Length = 0) AndAlso _
               (Me.txtCCCode2.Text.Length = 4 OrElse Me.txtCCCode2.Text.Length = 0) AndAlso _
               (Me.txtCCCode3.Text.Length = 4 OrElse Me.txtCCCode3.Text.Length = 0) AndAlso _
               (Me.txtCCCode4.Text.Length = 4 OrElse Me.txtCCCode4.Text.Length = 0) AndAlso _
               (Me.txtCCCode5.Text.Length = 4 OrElse Me.txtCCCode5.Text.Length = 0) AndAlso _
               (Me.txtCCCode6.Text.Length = 4 OrElse Me.txtCCCode6.Text.Length = 0)
    End Function

    Public Function IsValidCCCodeNewInput() As Boolean
        Return (Me.txtNewCCCode1.Text.Length = 4 OrElse Me.txtNewCCCode1.Text.Length = 0) AndAlso _
               (Me.txtNewCCCode2.Text.Length = 4 OrElse Me.txtNewCCCode2.Text.Length = 0) AndAlso _
               (Me.txtNewCCCode3.Text.Length = 4 OrElse Me.txtNewCCCode3.Text.Length = 0) AndAlso _
               (Me.txtNewCCCode4.Text.Length = 4 OrElse Me.txtNewCCCode4.Text.Length = 0) AndAlso _
               (Me.txtNewCCCode5.Text.Length = 4 OrElse Me.txtNewCCCode5.Text.Length = 0) AndAlso _
               (Me.txtNewCCCode6.Text.Length = 4 OrElse Me.txtNewCCCode6.Text.Length = 0)
    End Function

#End Region




End Class