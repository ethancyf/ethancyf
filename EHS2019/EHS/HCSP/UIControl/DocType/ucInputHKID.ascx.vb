Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Partial Public Class ucInputHKID
    Inherits ucInputDocTypeBase

    'Values
    Public Event SelectChineseName(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Private _strENameFirstName As String
    Private _strENameSurName As String
    Private _strCName As String
    Private _strCCCode1 As String
    Private _strCCCode2 As String
    Private _strCCCode3 As String
    Private _strCCCode4 As String
    Private _strCCCode5 As String
    Private _strCCCode6 As String
    Private _strDOB As String
    Private _strHKIDIssuseDate As String
    Private _strGender As String
    Private _strHKID As String

    Private _strReferenceNo As String = String.Empty
    Private _strTransNo As String = String.Empty

    Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
        If Me.Mode = BuildMode.Creation Then
            '-------------------------------------Account Creation Mode-------------------------------------------
            Me.btnSearchCCCode.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChineseNameSBtn")
            Me.btnSearchCCCode.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChineseNameBtn")

            'Table title
            Me.lblENameComma.Text = Me.GetGlobalResourceObject("Text", "Comma")

            'Gender Radio button list
            Me.rbGender.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGender.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            'Error Image
            Me.imgENameError.ImageUrl = strErrorImageURL
            Me.imgENameError.AlternateText = strErrorImageALT

            Me.imgCCCodeError.ImageUrl = strErrorImageURL
            Me.imgCCCodeError.AlternateText = strErrorImageALT

            Me.imgGenderError.ImageUrl = strErrorImageURL
            Me.imgGenderError.AlternateText = strErrorImageALT

            Me.imgHKIDIssueDateError.ImageUrl = strErrorImageURL
            Me.imgHKIDIssueDateError.AlternateText = strErrorImageALT

        Else
            '-------------------------------------Modification Mode-------------------------------------------
            Me.btnSearchCCCodeModification.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChineseNameSBtn")
            Me.btnSearchCCCodeModification.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChineseNameBtn")

            Me.lblENameSurnameModification.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Surname"))
            Me.lblENameFirstnameModification.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "Givenname"))

            'Table title
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKIC)

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.lblHKICNoModificationText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            Me.lblENameCommaModification.Text = Me.GetGlobalResourceObject("Text", "Comma")
            Me.lblReferenceNoModificationText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            'Me.lblHKICNoModificationText.Text = Me.GetGlobalResourceObject("Text", "HKID")
            Me.lblDOBModificationText.Text = Me.GetGlobalResourceObject("Text", "DOBLong")
            Me.lblENameModificationText.Text = Me.GetGlobalResourceObject("Text", "EnglishName")
            Me.lblGenderModificationText.Text = Me.GetGlobalResourceObject("Text", "Gender")
            Me.lblCCCodeModificationText.Text = Me.GetGlobalResourceObject("Text", "CCCODE")
            Me.lblCNameModificationText.Text = Me.GetGlobalResourceObject("Text", "ChineseName")
            Me.lblDOIModificationText.Text = Me.GetGlobalResourceObject("Text", "DateOfIssue")
            Me.lblTransactionNoModificationText.Text = Me.GetGlobalResourceObject("Text", "TransactionNo")

            'Gender Radio button list
            Me.rbGenderModification.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGenderModification.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

            'Error Image
            Me.imgENameModificationError.ImageUrl = strErrorImageURL
            Me.imgENameModificationError.AlternateText = strErrorImageALT

            Me.imgCCCodeModificationError.ImageUrl = strErrorImageURL
            Me.imgCCCodeModificationError.AlternateText = strErrorImageALT

            Me.imgGenderModificationError.ImageUrl = strErrorImageURL
            Me.imgGenderModificationError.AlternateText = strErrorImageALT

            Me.imgDOIModificationError.ImageUrl = strErrorImageURL
            Me.imgDOIModificationError.AlternateText = strErrorImageALT

            'div gender
            Me.lblIFemale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderFemale", New System.Globalization.CultureInfo(CultureLanguage.English))
            Me.lblIFemaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Female", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

            Me.lblIMale.Text = HttpContext.GetGlobalResourceObject("Text", "GenderMale", New System.Globalization.CultureInfo(CultureLanguage.English))
            Me.lblIMaleChi.Text = HttpContext.GetGlobalResourceObject("Text", "Male", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))


        End If
    End Sub

    Protected Overrides Sub Setup(ByVal mode As BuildMode)
        'Add Client Event--------------------------------------------------------------------------------------------
        Me.txtCCCode1.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode2.ClientID + ",4 );")
        Me.txtCCCode2.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode3.ClientID + ",4 );")
        Me.txtCCCode3.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode4.ClientID + ",4 );")
        Me.txtCCCode4.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode5.ClientID + ",4 );")
        Me.txtCCCode5.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode6.ClientID + ",4 );")

        Me.txtCCCode1Modification.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode2Modification.ClientID + ",4 );")
        Me.txtCCCode2Modification.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode3Modification.ClientID + ",4 );")
        Me.txtCCCode3Modification.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode4Modification.ClientID + ",4 );")
        Me.txtCCCode4Modification.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode5Modification.ClientID + ",4 );")
        Me.txtCCCode5Modification.Attributes.Add("onKeyUp", "autoTab(this," + Me.txtCCCode6Modification.ClientID + ",4 );")

        'If Not Me.SchemeClaim Is Nothing Then
        '    Me.rbGender.Items(0).Attributes.Add("onclick", String.Format("document.getElementById('{0}').style.backgroundImage = 'url(../Images/HKID/HKID_{1}_Female.jpg)';", Me.hkid_cell.ClientID, Me.SchemeClaim.SchemeCode))
        '    Me.rbGender.Items(1).Attributes.Add("onclick", String.Format("document.getElementById('{0}').style.backgroundImage = 'url(../Images/HKID/HKID_{1}_Male.jpg)';", Me.hkid_cell.ClientID, Me.SchemeClaim.SchemeCode))
        'End If

        Me._strHKID = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum, False)
        Me._strDOB = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, Me.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)

        If MyBase.UpdateValue Then
            Me._strENameFirstName = MyBase.EHSPersonalInfo.ENameFirstName
            Me._strENameSurName = MyBase.EHSPersonalInfo.ENameSurName
            Me._strGender = MyBase.EHSPersonalInfo.Gender

            'CName may assiged for display only
            'CName will update by the changing of CCCode(s), after step of "Enter-Detail" Confirmed
            Me._strCName = MyBase.EHSPersonalInfo.CName

            If MyBase.EHSPersonalInfo.DateofIssue.HasValue Then
                Me._strHKIDIssuseDate = MyBase.Formatter.formatHKIDIssueDate(MyBase.EHSPersonalInfo.DateofIssue)
            End If

            If Not MyBase.EHSPersonalInfo.CCCode1 Is Nothing Then
                Me._strCCCode1 = MyBase.EHSPersonalInfo.CCCode1.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode2 Is Nothing Then
                Me._strCCCode2 = MyBase.EHSPersonalInfo.CCCode2.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode3 Is Nothing Then
                Me._strCCCode3 = MyBase.EHSPersonalInfo.CCCode3.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode4 Is Nothing Then
                Me._strCCCode4 = MyBase.EHSPersonalInfo.CCCode4.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode5 Is Nothing Then
                Me._strCCCode5 = MyBase.EHSPersonalInfo.CCCode5.Trim()
            End If

            If Not MyBase.EHSPersonalInfo.CCCode6 Is Nothing Then
                Me._strCCCode6 = MyBase.EHSPersonalInfo.CCCode6.Trim()
            End If

            'Me.SetCName()
            'Me.SetEName()
            'If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
            '    Me.SetGender()
            'End If
            'Me.SetHKIDIssuseDate()
        End If

        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me.panEnterDetailCreation.Visible = True
            Me.panEnterDetailModify.Visible = False

            Me.SetHKID()
            Me.SetDOB()

            If MyBase.UpdateValue Then

                Me.SetEName()
                If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                    Me.SetGender()
                End If
                Me.SetHKIDIssuseDate()
            End If

            If MyBase.ActiveViewChanged Then
                Me.SetCName()
                Me.SetCCCodeError(False)
                Me.SetENameError(False)
                Me.SetGenderError(False)
                Me.SetHKIDIssueDateError(False)
            End If

            If MyBase.FixEnglishNameGender Then
                txtENameSurname.Enabled = False
                txtENameFirstrname.Enabled = False
                rbGender.Enabled = False
            Else
                txtENameSurname.Enabled = True
                txtENameFirstrname.Enabled = True
                rbGender.Enabled = True
            End If

        Else
            Me.panEnterDetailCreation.Visible = False
            Me.panEnterDetailModify.Visible = True

            Me.SetHKIDModification()
            Me.SetDOBModification()

            Me.SetENameModification()
            If Not Me._strGender Is Nothing AndAlso Not Me._strGender.Equals(String.Empty) Then
                Me.SetGenderModification()
            End If
            Me.SetHKIDIssuseDateModification()
            Me.SetReferenceNoModification()
            Me.SetTransactionNoModification()

            If MyBase.ActiveViewChanged Then
                ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
                Me.SetCNameModification()
                ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

                Me.SetCCCodeModificationError(False)
                Me.SetENameModificationError(False)
                Me.SetGenderModificationError(False)
                Me.SetHKIDIssueDateModificationError(False)
                Me.SetDOBModificationError(False)
            End If

            If mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly Then
                Me.txtDOBModification.Enabled = False
                Me.txtENameSurnameModification.Enabled = False
                Me.txtENameFirstnameModification.Enabled = False
                Me.txtCCCode1Modification.Enabled = False
                Me.txtCCCode2Modification.Enabled = False
                Me.txtCCCode3Modification.Enabled = False
                Me.txtCCCode4Modification.Enabled = False
                Me.txtCCCode5Modification.Enabled = False
                Me.txtCCCode6Modification.Enabled = False
                Me.rbGenderModification.Enabled = False
                SetGenderReadOnlyStyle(True)
                Me.txtDOIModification.Enabled = False
                Me.btnSearchCCCodeModification.Visible = False
            Else
                SetGenderReadOnlyStyle(False)

                If MyBase.EHSPersonalInfo.CreateBySmartID Then
                    Me.txtDOBModification.Enabled = False
                    Me.txtENameSurnameModification.Enabled = False
                    Me.txtENameFirstnameModification.Enabled = False
                    Me.txtCCCode1Modification.Enabled = False
                    Me.txtCCCode2Modification.Enabled = False
                    Me.txtCCCode3Modification.Enabled = False
                    Me.txtCCCode4Modification.Enabled = False
                    Me.txtCCCode5Modification.Enabled = False
                    Me.txtCCCode6Modification.Enabled = False

                    Me.txtDOIModification.Enabled = False
                    Me.btnSearchCCCodeModification.Visible = False
                Else
                    Me.txtDOBModification.Enabled = True
                    Me.txtENameSurnameModification.Enabled = True
                    Me.txtENameFirstnameModification.Enabled = True
                    Me.txtCCCode1Modification.Enabled = True
                    Me.txtCCCode2Modification.Enabled = True
                    Me.txtCCCode3Modification.Enabled = True
                    Me.txtCCCode4Modification.Enabled = True
                    Me.txtCCCode5Modification.Enabled = True
                    Me.txtCCCode6Modification.Enabled = True

                    Me.txtDOIModification.Enabled = True
                    Me.btnSearchCCCodeModification.Visible = True
                End If
            End If
            divFemale.Attributes.Add("onclick", "document.getElementById('" & rbGenderModification.ClientID & "_0').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGenderModification.ClientID & "\',\'\')', 0); ")
            divFemale.Attributes.Add("onmouseover", "document.getElementById('" & divFemale.ClientID & "').style.left='-1px'; document.getElementById('" & divFemale.ClientID & "').style.top='-1px'; ")
            divFemale.Attributes.Add("onmouseout", "document.getElementById('" & divFemale.ClientID & "').style.left='0px'; document.getElementById('" & divFemale.ClientID & "').style.top='0px'; ")
            divMale.Attributes.Add("onclick", "document.getElementById('" & rbGenderModification.ClientID & "_1').checked=true;javascript:setTimeout('__doPostBack(\'" & rbGenderModification.ClientID & "\',\'\')', 0); ")
            divMale.Attributes.Add("onmouseover", "document.getElementById('" & divMale.ClientID & "').style.left='-1px'; document.getElementById('" & divMale.ClientID & "').style.top='-1px'; ")
            divMale.Attributes.Add("onmouseout", "document.getElementById('" & divMale.ClientID & "').style.left='0px'; document.getElementById('" & divMale.ClientID & "').style.top='0px'; ")

        End If
    End Sub


#Region "Set Up Text Box Value  (Creation Mode)"

    Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)
        If mode = BuildMode.Creation Then
            Me.SetValue()
        Else
            Me.SetValueModification()
        End If
    End Sub

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Overloads Sub SetValue()
        Me.SetCName()
        Me.SetHKID()
        Me.SetEName()
        Me.SetDOB()
        Me.SetGender()
        Me.SetHKIDIssuseDate()
    End Sub

    Public Sub SetCName()
        Dim udtVAMaintBLL As BLL.VoucherAccountMaintenanceBLL = New BLL.VoucherAccountMaintenanceBLL()
        Dim strDBCName As String = String.Empty


        If Not Me._strCCCode1 Is Nothing Then
            If Me._strCCCode1.Length > 4 Then
                Me.txtCCCode1.Text = Me._strCCCode1.Substring(0, 4)
            Else
                Me.txtCCCode1.Text = Me._strCCCode1
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode1)
        Else
            Me.txtCCCode1.Text = String.Empty
        End If

        If Not Me._strCCCode2 Is Nothing AndAlso Me._strCCCode2.Trim().Length > 4 Then
            If Me._strCCCode2.Length > 4 Then
                Me.txtCCCode2.Text = Me._strCCCode2.Substring(0, 4)
            Else
                Me.txtCCCode2.Text = Me._strCCCode2
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode2)
        Else
            Me.txtCCCode2.Text = String.Empty
        End If

        If Not Me._strCCCode3 Is Nothing AndAlso Me._strCCCode3.Trim().Length > 4 Then
            If Me._strCCCode3.Length > 4 Then
                Me.txtCCCode3.Text = Me._strCCCode3.Substring(0, 4)
            Else
                Me.txtCCCode3.Text = Me._strCCCode3
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode3)
        Else
            Me.txtCCCode3.Text = String.Empty
        End If

        If Not Me._strCCCode4 Is Nothing AndAlso Me._strCCCode4.Trim().Length > 4 Then
            If Me._strCCCode4.Length > 4 Then
                Me.txtCCCode4.Text = Me._strCCCode4.Substring(0, 4)
            Else
                Me.txtCCCode4.Text = Me._strCCCode4
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode4)
        Else
            Me.txtCCCode4.Text = String.Empty
        End If

        If Not Me._strCCCode5 Is Nothing AndAlso Me._strCCCode5.Trim().Length > 4 Then
            If Me._strCCCode5.Length > 4 Then
                Me.txtCCCode5.Text = Me._strCCCode5.Substring(0, 4)
            Else
                Me.txtCCCode5.Text = Me._strCCCode5
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode5)
        Else
            Me.txtCCCode5.Text = String.Empty
        End If

        If Not Me._strCCCode6 Is Nothing AndAlso Me._strCCCode6.Trim().Length > 4 Then
            If Me._strCCCode6.Length > 4 Then
                Me.txtCCCode6.Text = Me._strCCCode6.Substring(0, 4)
            Else
                Me.txtCCCode6.Text = Me._strCCCode6
            End If

            'strDBCName += udtVAMaintBLL.getChiChar(Me._strCCCode6.Substring(0, 4), Me._strCCCode6.Substring(4, 1))
            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode6)
        Else
            Me.txtCCCode6.Text = String.Empty
        End If

        If strDBCName = String.Empty Then
            'If Me._strCName Is Nothing Then
            '    Me._strCName = String.Empty
            'End If
        Else
            Me._strCName = strDBCName
        End If

        If Me._strCName Is Nothing Then
            Me._strCName = String.Empty
        End If

        Me.SetCName(Me._strCName)

        'If strDBCName = String.Empty Then
        '    Me.SetCName(Me._strCName)
        'Else
        '    Me._strCName = strDBCName
        '    Me.SetCName(strDBCName)
        'End If

    End Sub

    Public Sub SetCName(ByVal strCName As String)
        Me.lblCName.Text = strCName
    End Sub

    Public Sub SetHKID()
        'Fill Data - hkid only
        Me.txtHKID.Text = Me._strHKID
    End Sub

    Public Sub SetEName()
        'Fill Data - English only
        Me.txtENameSurname.Text = Me._strENameSurName
        Me.txtENameFirstrname.Text = Me._strENameFirstName
    End Sub

    Public Sub SetDOB()
        'Fill Data - DOB only
        Me.txtDOB.Text = Me._strDOB
    End Sub

    Public Sub SetGender()
        'Fill Data - Gender only
        Me.rbGender.SelectedValue = Me._strGender
        If Not Me._strGender Is Nothing Then
            Me.changeHKID(Me.EHSPersonalInfo.DOB, Me._strGender)
        End If

    End Sub

    Public Sub SetHKIDIssuseDate()
        'Fill Data - HKID DOI only
        Me.txtHKIDIssueDate.Text = Me._strHKIDIssuseDate
        Me.txtHKID.Text = Me._strHKID
    End Sub

    Public Function CCCodeIsEmpty() As Boolean
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
    End Function

#End Region

#Region "Set Up Text Box Value  (Modification Mode)"
    '--------------------------------------------------------------------------------------------------------------
    'Set Up Text Box Value
    '--------------------------------------------------------------------------------------------------------------
    Public Sub SetValueModification()
        Me.SetCNameModification()
        Me.SetHKIDModification()
        Me.SetENameModification()
        Me.SetDOBModification()
        Me.SetGenderModification()
        Me.SetHKIDIssuseDateModification()
    End Sub

    Public Sub SetCNameModification()
        Dim udtVAMaintBLL As BLL.VoucherAccountMaintenanceBLL = New BLL.VoucherAccountMaintenanceBLL()
        Dim strDBCName As String = String.Empty


        If Not Me._strCCCode1 Is Nothing Then
            If Me._strCCCode1.Length > 4 Then
                Me.txtCCCode1Modification.Text = Me._strCCCode1.Substring(0, 4)
            Else
                Me.txtCCCode1Modification.Text = Me._strCCCode1
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode1)
        Else
            Me.txtCCCode1Modification.Text = String.Empty
        End If

        If Not Me._strCCCode2 Is Nothing Then

            If Me._strCCCode2.Length > 4 Then
                Me.txtCCCode2Modification.Text = Me._strCCCode2.Substring(0, 4)
            Else
                Me.txtCCCode2Modification.Text = Me._strCCCode2
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode2)
        Else
            Me.txtCCCode2Modification.Text = String.Empty
        End If

        If Not Me._strCCCode3 Is Nothing Then
            If Me._strCCCode3.Length > 4 Then
                Me.txtCCCode3Modification.Text = Me._strCCCode3.Substring(0, 4)
            Else
                Me.txtCCCode3Modification.Text = Me._strCCCode3
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode3)
        Else
            Me.txtCCCode3Modification.Text = String.Empty
        End If

        If Not Me._strCCCode4 Is Nothing Then
            If Me._strCCCode4.Length > 4 Then
                Me.txtCCCode4Modification.Text = Me._strCCCode4.Substring(0, 4)
            Else
                Me.txtCCCode4Modification.Text = Me._strCCCode4
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode4)
        Else
            Me.txtCCCode4Modification.Text = String.Empty
        End If

        If Not Me._strCCCode5 Is Nothing Then
            If Me._strCCCode5.Length > 4 Then
                Me.txtCCCode5Modification.Text = Me._strCCCode5.Substring(0, 4)
            Else
                Me.txtCCCode5Modification.Text = Me._strCCCode5
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode5)
        Else
            Me.txtCCCode5Modification.Text = String.Empty
        End If

        If Not Me._strCCCode6 Is Nothing AndAlso Me._strCCCode6.Trim().Length > 4 Then
            If Me._strCCCode6.Length > 4 Then
                Me.txtCCCode6Modification.Text = Me._strCCCode6.Substring(0, 4)
            Else
                Me.txtCCCode6Modification.Text = Me._strCCCode6
            End If

            strDBCName += udtVAMaintBLL.getCCCodeBig5(Me._strCCCode6)
        Else
            Me.txtCCCode6Modification.Text = String.Empty
        End If

        If strDBCName = String.Empty Then
            Me.SetCNameModification(Me._strCName)
        Else
            Me._strCName = strDBCName
            Me.SetCNameModification(strDBCName)
        End If

    End Sub

    Public Sub SetCNameModification(ByVal strCName As String)
        Me.lblCNameModification.Text = strCName
    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetHKIDModification()
        Me.lblHKICNoModification.Visible = False
        Me.txtHKICNoModification.Visible = False

        'Fill Data - hkid only
        If MyBase.EditDocumentNo Then
            Me.txtHKICNoModification.Text = Me._strHKID
            Me.txtHKICNoModification.Visible = True
        Else
            Me.lblHKICNoModification.Text = Me._strHKID
            Me.lblHKICNoModification.Visible = True
        End If

    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Public Sub SetENameModification()
        'Fill Data - English only
        Me.txtENameSurnameModification.Text = Me._strENameSurName
        Me.txtENameFirstnameModification.Text = Me._strENameFirstName
    End Sub

    Public Sub SetDOBModification()
        'Fill Data - DOB only
        Me.txtDOBModification.Text = Me._strDOB
    End Sub

    Public Sub SetGenderModification()
        'Fill Data - Gender only        
        Dim strGender As String
        Me.rbGenderModification.SelectedValue = Me._strGender

        If Me._strGender = "M" Then
            strGender = "GenderMale"
        Else
            strGender = "GenderFemale"
        End If
        Me.lblReadonlyGender.Text = Me.GetGlobalResourceObject("Text", strGender)
        HandleDivGenderStyle(Me._strGender)
    End Sub

    Public Sub SetHKIDIssuseDateModification()
        'Fill Data - HKID DOI only
        Me.txtDOIModification.Text = Me._strHKIDIssuseDate
        Me.lblHKICNoModification.Text = Me._strHKID
    End Sub

    Public Function CCCodeIsEmptyModification() As Boolean
        If Me.txtCCCode1Modification.Text = String.Empty AndAlso _
            Me.txtCCCode2Modification.Text = String.Empty AndAlso _
            Me.txtCCCode3Modification.Text = String.Empty AndAlso _
            Me.txtCCCode4Modification.Text = String.Empty AndAlso _
            Me.txtCCCode5Modification.Text = String.Empty AndAlso _
            Me.txtCCCode6Modification.Text = String.Empty Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub SetReferenceNoModification()
        If Me._strReferenceNo.Trim.Equals(String.Empty) Then
            Me.trReferenceNoModification.Visible = False
        Else
            Me.trReferenceNoModification.Visible = True
            Me.lblReferenceNoModification.Text = Me._strReferenceNo
        End If
    End Sub

    Public Sub SetTransactionNoModification()
        If Me._strTransNo.Trim.Equals(String.Empty) Then
            Me.trTransactionNoModification.Visible = False
        Else
            Me.trTransactionNoModification.Visible = True
            Me.lblTransactionNoModification.Text = Me._strTransNo
        End If
    End Sub

#End Region

#Region "Set Up Error Image (Creation Mode)"

    Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
        If mode = BuildMode.Creation Then
            Me.SetErrorImage(visible)
        Else
            Me.SetErrorImage_M(visible)
        End If
    End Sub

    Public Overloads Sub SetErrorImage(ByVal visible As Boolean)
        Me.SetCCCodeError(visible)
        Me.SetENameError(visible)
        Me.SetGenderError(visible)
        Me.SetHKIDIssueDateError(visible)
    End Sub

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Sub SetCCCodeError(ByVal visible As Boolean)
        Me.imgCCCodeError.Visible = visible
    End Sub

    Public Sub SetENameError(ByVal visible As Boolean)
        Me.imgENameError.Visible = visible
    End Sub

    Public Sub SetGenderError(ByVal visible As Boolean)
        Me.imgGenderError.Visible = visible
    End Sub

    Public Sub SetHKIDIssueDateError(ByVal visible As Boolean)
        Me.imgHKIDIssueDateError.Visible = visible
    End Sub

#End Region

#Region "Set Up Error Image (Modification Mode)"

    Public Overloads Sub SetErrorImage_M(ByVal visible As Boolean)
        Me.SetCCCodeModificationError(visible)
        Me.SetENameModificationError(visible)
        Me.SetGenderModificationError(visible)
        Me.SetHKIDIssueDateModificationError(visible)
        Me.SetDOBModificationError(visible)
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.SetHKICNoModificationError(visible)
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
    End Sub

    '--------------------------------------------------------------------------------------------------------------
    'Set Up Error Image
    '--------------------------------------------------------------------------------------------------------------
    Public Sub SetCCCodeModificationError(ByVal visible As Boolean)
        Me.imgCCCodeModificationError.Visible = visible
    End Sub

    Public Sub SetENameModificationError(ByVal visible As Boolean)
        Me.imgENameModificationError.Visible = visible
    End Sub

    Public Sub SetGenderModificationError(ByVal visible As Boolean)
        Me.imgGenderModificationError.Visible = visible
    End Sub

    Public Sub SetHKIDIssueDateModificationError(ByVal visible As Boolean)
        Me.imgDOIModificationError.Visible = visible
    End Sub

    Public Sub SetDOBModificationError(ByVal visible As Boolean)
        Me.imgDOBModificationError.Visible = visible
    End Sub

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub SetHKICNoModificationError(ByVal visible As Boolean)
        Me.imgHKICNoModificationError.Visible = visible
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region

#Region "Events"
    '--------------------------------------------------------------------------------------------------------------
    'Events
    '--------------------------------------------------------------------------------------------------------------
    Protected Sub btnSearchCCCode_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchCCCode.Click, btnSearchCCCodeModification.Click
        RaiseEvent SelectChineseName(sender, e)
    End Sub

    Private Sub rbGender_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbGender.SelectedIndexChanged
        Me.changeHKID(Me.EHSPersonalInfo.DOB, CType(sender, RadioButtonList).SelectedValue)
    End Sub

    Protected Sub rbGenderModification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbGenderModification.SelectedIndexChanged
        Dim rbGender As RadioButtonList = CType(sender, RadioButtonList)
        HandleDivGenderStyle(rbGender.SelectedValue)
    End Sub

    Private Sub HandleDivGenderStyle(ByVal strGender As String)
        Select Case strGender
            Case "M"
                divFemale.Style.Add("outline-color", "black")
                divFemale.Style.Add("outline-width", "2px")

                divMale.Style.Add("outline-color", "#3198FF")
                divMale.Style.Add("outline-width", "8px")

            Case "F"
                divFemale.Style.Add("outline-color", "#3198FF")
                divFemale.Style.Add("outline-width", "8px")

                divMale.Style.Add("outline-color", "black")
                divMale.Style.Add("outline-width", "2px")
        End Select
    End Sub

    Private Sub SetGenderReadOnlyStyle(ByVal ReadOnlyMode As Boolean)
        If ReadOnlyMode = True Then
            Me.divGender.Visible = False
            Me.lblReadonlyGender.Visible = True
            Me.trGenderImageInput.Style.Remove("height")
            Me.rbGender.Enabled = False
        Else
            Me.divGender.Visible = True
            Me.lblReadonlyGender.Visible = False
            Me.rbGender.Enabled = True
        End If
    End Sub



#End Region

#Region "Property"
    Public Overrides Sub SetProperty(ByVal mode As BuildMode)
        ' I-CRP16-002 Fix invalid input on English name [Start][Lawrence]
        If mode = ucInputDocTypeBase.BuildMode.Creation Then
            Me._strENameFirstName = Me.txtENameFirstrname.Text.Trim
            Me._strENameSurName = Me.txtENameSurname.Text.Trim
            Me._strCName = Me.lblCName.Text
            Me._strGender = Me.rbGender.SelectedValue
            Me._strHKID = Me.txtHKID.Text.Trim
            Me._strDOB = Me.txtDOB.Text.Trim
            Me._strHKIDIssuseDate = Me.txtHKIDIssueDate.Text.Trim
            Me._strCCCode1 = Me.txtCCCode1.Text.Trim
            Me._strCCCode2 = Me.txtCCCode2.Text.Trim
            Me._strCCCode3 = Me.txtCCCode3.Text.Trim
            Me._strCCCode4 = Me.txtCCCode4.Text.Trim
            Me._strCCCode5 = Me.txtCCCode5.Text.Trim
            Me._strCCCode6 = Me.txtCCCode6.Text.Trim
        Else

            Me._strENameFirstName = Me.txtENameFirstnameModification.Text.Trim
            Me._strENameSurName = Me.txtENameSurnameModification.Text.Trim
            Me._strCName = Me.lblCNameModification.Text
            Me._strGender = Me.rbGenderModification.SelectedValue
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If MyBase.EditDocumentNo Then
                Me._strHKID = Me.txtHKICNoModification.Text.Trim
            Else
                Me._strHKID = Me.lblHKICNoModification.Text
            End If
            ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
            Me._strDOB = Me.txtDOBModification.Text.Trim
            Me._strHKIDIssuseDate = Me.txtDOIModification.Text.Trim
            Me._strCCCode1 = Me.txtCCCode1Modification.Text.Trim
            Me._strCCCode2 = Me.txtCCCode2Modification.Text.Trim
            Me._strCCCode3 = Me.txtCCCode3Modification.Text.Trim
            Me._strCCCode4 = Me.txtCCCode4Modification.Text.Trim
            Me._strCCCode5 = Me.txtCCCode5Modification.Text.Trim
            Me._strCCCode6 = Me.txtCCCode6Modification.Text.Trim
        End If
        ' I-CRP16-002 Fix invalid input on English name [End][Lawrence]
    End Sub

    Public Property ENameSurName() As String
        Get
            Return Me._strENameSurName
        End Get
        Set(ByVal value As String)
            Me._strENameSurName = value
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

    Public Property DOB() As String
        Get
            Return Me._strDOB
        End Get
        Set(ByVal value As String)
            Me._strDOB = value
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

    Public Property HKIDIssuseDate() As String
        Get
            Return Me._strHKIDIssuseDate
        End Get
        Set(ByVal value As String)
            Me._strHKIDIssuseDate = value
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

    Public Property ReferenceNo() As String
        Get
            Return Me._strReferenceNo
        End Get
        Set(ByVal value As String)
            Me._strReferenceNo = value
        End Set
    End Property

    Public Property TransactionNo() As String
        Get
            Return Me._strTransNo
        End Get
        Set(ByVal value As String)
            Me._strTransNo = value
        End Set
    End Property

#End Region

#Region "Other"

    Public Sub changeHKID(ByVal dtmDOB As DateTime, ByVal strRadioButtonSelectedGender As String)
        Dim commonFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim dtmCurrentDate As DateTime = commonFunction.GetSystemDateTime()
        Dim strGender As String


        If strRadioButtonSelectedGender Is Nothing OrElse strRadioButtonSelectedGender.Equals(String.Empty) Then
            Me.hkid_cell.Style.Item("background-image") = "url(../Images/HKID/HKID_Empty.jpg)"
        Else
            If strRadioButtonSelectedGender = "M" Then
                strGender = "Male"
            Else
                strGender = "Female"
            End If

            If dtmCurrentDate.Year - dtmDOB.Year <= 11 Then
                Me.hkid_cell.Style.Item("background-image") = String.Format("url(../Images/HKID/HKID_Children_{0}.jpg)", strGender)
            ElseIf dtmCurrentDate.Year - dtmDOB.Year < 65 Then
                Me.hkid_cell.Style.Item("background-image") = String.Format("url(../Images/HKID/HKID_Teenage_{0}.jpg)", strGender)
            Else
                Me.hkid_cell.Style.Item("background-image") = String.Format("url(../Images/HKID/HKID_Elder_{0}.jpg)", strGender)
            End If
        End If
    End Sub

    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 1
    'Compare input and session CCCode head, return session CCCode if matched
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
    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 1

    Public Function IsValidCCCodeInput() As Boolean
        Return (Me.txtCCCode1.Text.Length = 4 OrElse Me.txtCCCode1.Text.Length = 0) AndAlso _
               (Me.txtCCCode2.Text.Length = 4 OrElse Me.txtCCCode2.Text.Length = 0) AndAlso _
               (Me.txtCCCode3.Text.Length = 4 OrElse Me.txtCCCode3.Text.Length = 0) AndAlso _
               (Me.txtCCCode4.Text.Length = 4 OrElse Me.txtCCCode4.Text.Length = 0) AndAlso _
               (Me.txtCCCode5.Text.Length = 4 OrElse Me.txtCCCode5.Text.Length = 0) AndAlso _
               (Me.txtCCCode6.Text.Length = 4 OrElse Me.txtCCCode6.Text.Length = 0)
    End Function

    Public Function IsValidCCCodeModificationInput() As Boolean
        Return (Me.txtCCCode1Modification.Text.Length = 4 OrElse Me.txtCCCode1Modification.Text.Length = 0) AndAlso _
               (Me.txtCCCode2Modification.Text.Length = 4 OrElse Me.txtCCCode2Modification.Text.Length = 0) AndAlso _
               (Me.txtCCCode3Modification.Text.Length = 4 OrElse Me.txtCCCode3Modification.Text.Length = 0) AndAlso _
               (Me.txtCCCode4Modification.Text.Length = 4 OrElse Me.txtCCCode4Modification.Text.Length = 0) AndAlso _
               (Me.txtCCCode5Modification.Text.Length = 4 OrElse Me.txtCCCode5Modification.Text.Length = 0) AndAlso _
               (Me.txtCCCode6Modification.Text.Length = 4 OrElse Me.txtCCCode6Modification.Text.Length = 0)
    End Function

#End Region

End Class

#Region "comment Code"

'Private _udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
'Private _updateValue As Boolean
'Private udtVRAcct As VoucherRecipientAccountModel
'Private _udtFormatter As Common.Format.Formatter = New Common.Format.Formatter

'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'    'Dim scriptManager As ScriptManagerProxy = Me.ScriptManagerProxy1


'    Me.Setup()
'    Me.RenderLanguage()
'    'scriptManager.SetFocus(txtENameSurname)
'End Sub


'Script in Page Load----------------------------------------------------------------
'Me.rbGender.Items(0).Attributes.Add("onclick", String.Format("document.getElementById('{0}').style.backgroundImage = 'url(../Images/HKID/HKID_Female.jpg)';", Me.hkid_cell.ClientID))
'Me.rbGender.Items(1).Attributes.Add("onclick", String.Format("document.getElementById('{0}').style.backgroundImage = 'url(../Images/HKID/HKID_Male.jpg)';", Me.hkid_cell.ClientID))



'Public Property DocumentType() As String
'    Get
'        Return Me._strDocumentType
'    End Get
'    Set(ByVal value As String)
'        Me._strDocumentType = value.Trim()
'    End Set
'End Property

'Public Property EHealthAccount() As EHSAccountModel.EHSPersonalInformationModel
'    Get
'        Return Me._udtEHSAccountPersonalInfo
'    End Get
'    Set(ByVal value As EHSAccountModel.EHSPersonalInformationModel)
'        Me._udtEHSAccountPersonalInfo = value
'    End Set
'End Property

'Public Property UpdateValue() As Boolean
'    Get
'        Return Me._updateValue
'    End Get
'    Set(ByVal value As Boolean)
'        Me._updateValue = value
'    End Set
'End Property
#End Region