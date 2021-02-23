Imports Common.ComFunction
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel

Namespace UIControl.DocTypeText

    Partial Public Class ucInputHKIDSmartID
        Inherits ucInputDocTypeBase

        'Values
        Public Event SelectedGender(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event GenderTips(ByVal sender As Object, ByVal e As System.EventArgs)

        Private _strCName As String
        Private _strEName As String
        Private _strCCCode1 As String
        Private _strCCCode2 As String
        Private _strCCCode3 As String
        Private _strCCCode4 As String
        Private _strCCCode5 As String
        Private _strCCCode6 As String
        Private _strDOB As String
        Private _strDOI As String
        Private _strHKIDIssuseDate As String
        Private _strGender As String
        Private _strHKID As String

        Private _strReferenceNo As String = String.Empty
        Private _strTransNo As String = String.Empty

        Private udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Private _udtSmartIDContent As BLL.SmartIDContentModel

        Protected Overrides Sub RenderLanguage(ByVal strErrorImageURL As String, ByVal strErrorImageALT As String)
            'Table title
            Dim udtDocTypeModel As DocTypeModel = udtDocTypeBLL.getAllDocType.Filter(DocTypeCode.HKIC)
            Dim strDOBText As String = Me.GetGlobalResourceObject("Text", "DOBLong")
            Dim strDOIText As String = Me.GetGlobalResourceObject("Text", "DateOfIssue")
            Dim strENameText As String = Me.GetGlobalResourceObject("Text", "Name")
            Dim strGenderText As String = Me.GetGlobalResourceObject("Text", "Gender")
            Dim strCCCodeText As String = Me.GetGlobalResourceObject("Text", "CCCODE")

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me.lblDocumentType.Text = udtDocTypeModel.DocName(MyBase.SessionHandler.Language())
            Me.lblHKICNoText.Text = udtDocTypeModel.DocIdentityDesc(MyBase.SessionHandler.Language())
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            Me.lbleHealthAccountText.Text = Me.GetGlobalResourceObject("Text", "eHealthAccount")
            Me.lblDiffInSmartIDText.Text = Me.GetGlobalResourceObject("Text", "DiffInSmartID")

            Me.lblDocumentTypeText.Text = Me.GetGlobalResourceObject("Text", "DocumentType")


            Me.lblDOBText.Text = strDOBText
            Me.lblENameText.Text = strENameText
            Me.lblGenderText.Text = strGenderText
            Me.lblDOIText.Text = strDOIText
            Me.lblCCCodeText.Text = strCCCodeText

            Me.lblDOBText.BackColor = Drawing.Color.FromArgb(225, 225, 225)
            Me.lblENameText.BackColor = Drawing.Color.FromArgb(225, 225, 225)
            Me.lblGenderText.BackColor = Drawing.Color.FromArgb(225, 225, 225)
            Me.lblDOIText.BackColor = Drawing.Color.FromArgb(225, 225, 225)
            Me.lblCCCodeText.BackColor = Drawing.Color.FromArgb(225, 225, 225)

            'Me.lblDOBSmartIDText.Text = strDOBText
            'Me.lblENameSmartIDText.Text = strENameText
            'Me.lblGenderSmartIDText.Text = strGenderText
            'Me.lblDOISmartIDText.Text = strDOIText
            'Me.lblCCCodeSmartIDText.Text = strCCCodeText

            'Gender Radio button list
            Me.rbGenderSmartID.Items(0).Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            Me.rbGenderSmartID.Items(1).Text = Me.GetGlobalResourceObject("Text", "GenderMale")

        End Sub

        Protected Overrides Sub Setup(ByVal mode As BuildMode)
            Dim udtEHSAccountSmartID As EHSAccountModel = Me._udtSmartIDContent.EHSAccount

            '-------------------------------------------------------------------------------------------------------------------------
            'Existing Account Information
            '-------------------------------------------------------------------------------------------------------------------------
            Me.lblDOB.Text = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, Me.SessionHandler.Language, MyBase.EHSPersonalInfo.ECAge, MyBase.EHSPersonalInfo.ECDateOfRegistration)
            Me.lblEName.Text = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfo.ENameSurName, MyBase.EHSPersonalInfo.ENameFirstName)
            Me.lblDOI.Text = MyBase.Formatter.formatDOI(DocTypeCode.HKIC, MyBase.EHSPersonalInfo.DateofIssue)
            Me.lblCName.Text = MyBase.Formatter.formatChineseName(MyBase.EHSPersonalInfo.CName)
            Me.lblCCCode1.Text = Me.GetCCCode(MyBase.EHSPersonalInfo.CCCode1)
            Me.lblCCCode2.Text = Me.GetCCCode(MyBase.EHSPersonalInfo.CCCode2)
            Me.lblCCCode3.Text = Me.GetCCCode(MyBase.EHSPersonalInfo.CCCode3)
            Me.lblCCCode4.Text = Me.GetCCCode(MyBase.EHSPersonalInfo.CCCode4)
            Me.lblCCCode5.Text = Me.GetCCCode(MyBase.EHSPersonalInfo.CCCode5)
            Me.lblCCCode6.Text = Me.GetCCCode(MyBase.EHSPersonalInfo.CCCode6)

            If Not String.IsNullOrEmpty(Me.lblCCCode4.Text) AndAlso Not String.IsNullOrEmpty(Me.lblCCCode5.Text) AndAlso Not String.IsNullOrEmpty(Me.lblCCCode6.Text) Then
                Me.panCCCode2.Visible = True
            Else
                Me.panCCCode2.Visible = False
            End If

            If MyBase.EHSPersonalInfo.Gender = "M" Then
                Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
            Else
                Me.lblGender.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
            End If

            Dim blnMaskIdentityNumber As Boolean = True
            If SmartIDShowRealID() Then blnMaskIdentityNumber = False

            Me._strHKID = MyBase.Formatter.formatHKID(MyBase.EHSPersonalInfo.IdentityNum, blnMaskIdentityNumber)
            Me.SetHKID()

            '-------------------------------------------------------------------------------------------------------------------------
            'Fill SmartID Account Information
            '-------------------------------------------------------------------------------------------------------------------------
            Dim udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccountSmartID.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
            Me._strGender = udtPersonalInfoSmartID.Gender


            If Not Me.SameDOB(udtPersonalInfoSmartID) Then
                'Me.lblDOBSmartIDText.Visible = True
                Me.SetDOB()

                'Else
                '    Me.lblDOBSmartIDText.Visible = False
            End If

            If Not Me.SameDOI(udtPersonalInfoSmartID) Then
                'Me.lblDOISmartIDText.Visible = True
                Me._strHKIDIssuseDate = MyBase.Formatter.formatHKIDIssueDate(udtPersonalInfoSmartID.DateofIssue)
                Me.SetDOI()
                'Else
                '    Me.lblDOISmartIDText.Visible = False
            End If

            If Not Me.SameEName(udtPersonalInfoSmartID) OrElse Not Me.SameCCCode(udtPersonalInfoSmartID) Then
                Me._strCCCode1 = udtPersonalInfoSmartID.CCCode1
                Me._strCCCode2 = udtPersonalInfoSmartID.CCCode2
                Me._strCCCode3 = udtPersonalInfoSmartID.CCCode3
                Me._strCCCode4 = udtPersonalInfoSmartID.CCCode4
                Me._strCCCode5 = udtPersonalInfoSmartID.CCCode5
                Me._strCCCode6 = udtPersonalInfoSmartID.CCCode6
                Me._strEName = MyBase.Formatter.formatEnglishName(udtPersonalInfoSmartID.ENameSurName, udtPersonalInfoSmartID.ENameFirstName)
                Me.SetCName(udtPersonalInfoSmartID)
                Me.SetENameSmartID()
                'Me.lblENameSmartIDText.Visible = True
                'Me.lblCCCodeSmartIDText.Visible = True

                If Not String.IsNullOrEmpty(Me._strCCCode4) Then
                    Me.panCCCodeSmartID2.Visible = True
                Else
                    Me.panCCCodeSmartID2.Visible = False
                End If

                Me.panEnameSmartID.Visible = True
                Me.panCCCodeSmartID1.Visible = True
            Else
                'Me.lblENameSmartIDText.Visible = False
                'Me.lblCCCodeSmartIDText.Visible = False
                Me.panEnameSmartID.Visible = False
                Me.panCCCodeSmartID1.Visible = False
            End If

            'If MyBase.UpdateValue Then
            '    Me.SetGender()
            'End If

            '' Show / Hide Diff Personal Detail in Smart CFD
            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Me._strGender = udtPersonalInfoSmartID.Gender

            If _udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.TwoGender Or _
               _udtSmartIDContent.IdeasVersion = BLL.IdeasBLL.EnumIdeasVersion.ComboGender Then
                If Not Me.SameGender(udtPersonalInfoSmartID) Then
                    Me.SetGender(True)
                End If
            Else
                'Select Gender
                If MyBase.ActiveViewChanged Then
                    Me._strGender = udtPersonalInfoSmartID.Gender

                    If MyBase.UpdateValue Then
                        Me.SetGender(False)
                    End If
                End If
            End If

            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

            If MyBase.ActiveViewChanged Then
                Me.SetGenderSmartIDError(False)
            End If

        End Sub

        Private Function FillValue(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            If String.IsNullOrEmpty(udtEHSPersonalInfo.Gender) Then
                Return False
            End If

            Return True
        End Function

#Region "Set Up Text Box Value (Creation Mode)"

        Public Overrides Sub SetValue(ByVal mode As ucInputDocTypeBase.BuildMode)

            Me.SetValue()

        End Sub

        Public Sub SetDOI()
            'Fill Data - hkid only
            Me.lblDOISmartID.Text = Me._strHKIDIssuseDate

        End Sub

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Text Box Value
        '--------------------------------------------------------------------------------------------------------------
        Public Overloads Sub SetValue()
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'SetGender()
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        End Sub

        Public Sub SetENameSmartID()
            'Fill Data - hkid only
            Me.lblENameSmartID.Text = Me._strEName

        End Sub

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Text Box Value
        '--------------------------------------------------------------------------------------------------------------
        Public Sub SetHKID()
            'Fill Data - hkid only
            Me.lblHKICNo.Text = Me._strHKID

        End Sub

        Public Sub SetDOB()
            'Fill Data - hkid only
            Me.lblDOBSmartID.Text = Me._strDOB

        End Sub

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------        
        'Public Sub SetGender()
        '    Me.rbGenderSmartID.SelectedValue = Me._strGender
        'End Sub

        Public Sub SetGender(ByVal blnReadOnly As Boolean)
            Me.lblGenderSmartID.Visible = False
            Me.rbGenderSmartID.Visible = False

            If blnReadOnly Then
                Me.lblGenderSmartID.Visible = True
                If Me._strGender = "M" Then
                    Me.lblGenderSmartID.Text = Me.GetGlobalResourceObject("Text", "GenderMale")
                Else
                    Me.lblGenderSmartID.Text = Me.GetGlobalResourceObject("Text", "GenderFemale")
                End If

            Else
                Me.rbGenderSmartID.Visible = True

                If Me._strGender = String.Empty Then
                    Me.rbGenderSmartID.SelectedValue = Nothing
                Else
                    Me.rbGenderSmartID.SelectedValue = Me._strGender
                End If
            End If
        End Sub
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        'Private Sub SetSmartICLabelVisibility()
        '    ' Show/Hide Label
        '    Dim isShowLabel As Boolean

        '    ' English Name
        '    isShowLabel = Not String.IsNullOrEmpty(Me.lblENameSmartID.Text)
        '    Me.lblENameSmartIDText.Visible = isShowLabel
        '    Me.lblENameSmartID.Visible = isShowLabel

        '    ' DOB
        '    isShowLabel = Not String.IsNullOrEmpty(Me.lblDOBSmartID.Text)
        '    Me.lblDOBSmartIDText.Visible = isShowLabel
        '    Me.lblDOBSmartID.Visible = isShowLabel

        '    ' DOI
        '    isShowLabel = Not String.IsNullOrEmpty(Me.lblDOISmartID.Text)
        '    Me.lblDOISmartIDText.Visible = isShowLabel
        '    Me.lblDOISmartID.Visible = isShowLabel

        'End Sub

        Private Function GetCCCode(ByVal strCCCode As String) As String
            If Not strCCCode Is Nothing AndAlso strCCCode.Length > 4 Then
                Return strCCCode.Substring(0, 4)
            End If
            Return String.Empty
        End Function

        Public Sub SetCNameSmartID(ByVal strCName As String)
            Me.lblCNameSmartID.Text = MyBase.Formatter.formatChineseName(strCName)
        End Sub

        Public Sub SetCName(ByVal udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel)
            Dim strDBCName As String = BLL.VoucherAccountMaintenanceBLL.GetCName(udtPersonalInfoSmartID)

            Me.SetCCCodeLabel(Me.lblCCCode1SmartID, Me._strCCCode1)
            Me.SetCCCodeLabel(Me.lblCCCode2SmartID, Me._strCCCode2)
            Me.SetCCCodeLabel(Me.lblCCCode3SmartID, Me._strCCCode3)
            Me.SetCCCodeLabel(Me.lblCCCode4SmartID, Me._strCCCode4)
            Me.SetCCCodeLabel(Me.lblCCCode5SmartID, Me._strCCCode5)
            Me.SetCCCodeLabel(Me.lblCCCode6SmartID, Me._strCCCode6)

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

            Me.SetCNameSmartID(Me._strCName)

            'If strDBCName = String.Empty Then
            '    Me.SetCNameSmartID(Me._strCName)
            'Else
            '    Me._strCName = strDBCName
            '    Me.SetCNameSmartID(strDBCName)
            'End If

        End Sub

#End Region

#Region "Check Difference"

        Private Function SameEName(ByVal udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            Dim strEName As String = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfo.ENameSurName, MyBase.EHSPersonalInfo.ENameFirstName)
            Dim strENameSmartID As String = MyBase.Formatter.formatEnglishName(udtPersonalInfoSmartID.ENameSurName, udtPersonalInfoSmartID.ENameFirstName)

            If strEName.Equals(strENameSmartID) Then
                Return True
            Else
                Me._strEName = strENameSmartID
                Return False
            End If

        End Function

        Private Function SameCCCode(ByVal udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            Dim strEName As String = MyBase.Formatter.formatEnglishName(MyBase.EHSPersonalInfo.ENameSurName, MyBase.EHSPersonalInfo.ENameFirstName)
            Dim strENameSmartID As String = MyBase.Formatter.formatEnglishName(udtPersonalInfoSmartID.ENameSurName, udtPersonalInfoSmartID.ENameFirstName)

            Return MyBase.EHSPersonalInfo.CCCode1 = udtPersonalInfoSmartID.CCCode1 AndAlso MyBase.EHSPersonalInfo.CCCode2 = udtPersonalInfoSmartID.CCCode2 AndAlso _
            MyBase.EHSPersonalInfo.CCCode3 = udtPersonalInfoSmartID.CCCode3 AndAlso MyBase.EHSPersonalInfo.CCCode4 = udtPersonalInfoSmartID.CCCode4 AndAlso _
            MyBase.EHSPersonalInfo.CCCode5 = udtPersonalInfoSmartID.CCCode5 AndAlso MyBase.EHSPersonalInfo.CCCode6 = udtPersonalInfoSmartID.CCCode6
        End Function

        Private Function SameDOB(ByVal udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            Dim strDOB As String = MyBase.Formatter.formatDOB(MyBase.EHSPersonalInfo.DOB, MyBase.EHSPersonalInfo.ExactDOB, Common.Component.CultureLanguage.English, Nothing, Nothing)
            Dim strDOBSmartID As String = MyBase.Formatter.formatDOB(udtPersonalInfoSmartID.DOB, udtPersonalInfoSmartID.ExactDOB, Common.Component.CultureLanguage.English, Nothing, Nothing)


            If strDOB.Equals(strDOBSmartID) Then
                Return True
            Else
                Me._strDOB = strDOBSmartID
                Return False
            End If

        End Function

        Private Function SameDOI(ByVal udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            Dim strDOI As String = MyBase.Formatter.formatDOI(DocTypeModel.DocTypeCode.HKIC, MyBase.EHSPersonalInfo.DateofIssue)
            Dim strDOISmartID As String = MyBase.Formatter.formatDOI(DocTypeModel.DocTypeCode.HKIC, udtPersonalInfoSmartID.DateofIssue)

            If strDOI.Equals(strDOISmartID) Then
                Return True
            Else
                Me._strDOI = strDOISmartID
                Return False
            End If

        End Function

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private Function SameGender(ByVal udtPersonalInfoSmartID As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            Dim strGender As String = MyBase.EHSPersonalInfo.Gender
            Dim strGenderSmartID As String = udtPersonalInfoSmartID.Gender

            If strGender.Equals(strGenderSmartID) Then
                Return True
            Else
                Me._strGender = strGenderSmartID
                Return False
            End If

        End Function
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
#End Region

#Region "Set Up Error Image (Modification Mode)"

        Public Overrides Sub SetErrorImage(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal visible As Boolean)
            Me.SetGenderSmartIDError(visible)
        End Sub

        '--------------------------------------------------------------------------------------------------------------
        'Set Up Error Image
        '--------------------------------------------------------------------------------------------------------------
        Public Sub SetGenderSmartIDError(ByVal visible As Boolean)
            Me.lblGenderSmartIDError.Visible = visible
        End Sub

#End Region

#Region "Events"

        Protected Sub rbGenderSmartID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbGenderSmartID.SelectedIndexChanged
            RaiseEvent SelectedGender(sender, e)
        End Sub

        'Protected Sub lnkGenderSmartIDTips_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkGenderSmartIDTips.Click
        '    RaiseEvent GenderTips(sender, e)
        'End Sub

#End Region

#Region "Property"

        Public Overrides Sub SetProperty(ByVal mode As BuildMode)
            Me._strGender = Me.rbGenderSmartID.SelectedValue
        End Sub

        Public Property SmartIDContentModel() As HCSP.BLL.SmartIDContentModel
            Get
                Return Me._udtSmartIDContent
            End Get
            Set(ByVal value As HCSP.BLL.SmartIDContentModel)
                Me._udtSmartIDContent = value
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


        Public ReadOnly Property Gender() As String
            Get
                Return Me._strGender
            End Get
        End Property
#End Region

#Region "Supporting Function"

        Private Function SmartIDShowRealID() As Boolean
            Dim udtGeneralFunction As New GeneralFunction
            Dim strParmValue As String = String.Empty
            udtGeneralFunction.getSystemParameter("SmartIDShowRealID", strParmValue, String.Empty)
            Return strParmValue.Trim = "Y"
        End Function

#End Region

    End Class

End Namespace