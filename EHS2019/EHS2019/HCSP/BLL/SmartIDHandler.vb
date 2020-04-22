Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.ComFunction

Namespace BLL

    Public Class SmartIDHandler

        Public Enum SmartIDResultStatus
            'default value 
            Empty

            '13, 14
            DocTypeNotExist

            ' For UI Flow

            '--------------------------------------------------------------------------------
            'for validated account
            '--------------------------------------------------------------------------------
            ValidateAccountExist_SameDetail 'Go to Enter Claim Detail

            ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB 'go to account modify (may be typo error before)

            ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB_NoCCCode ' go to Enter Claim Detail (update CCCode Only)

            ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI 'go to next (Create a new Temp Account)

            'ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_NotSameDOB 'Block Claim (no new/old card)(IMMD validation passed before)
            ValidateAccountExist_DiffDetail_SameDOI_DiffDOB 'Block Claim (no new/old card)(IMMD validation passed before)

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'ValidateAccountExist_DiffDetail_SameDOI_CreateBySmartID 'Block Claim (no new/old card)
            ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithGender 'Block Claim (no new/old card)

            ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName  'go to account modify Create a new Temp Account (may be selected wrong gender before, for ideas2 & 2.5 case)

            ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_DiffName 'Block Claim (Same Card but different Name)

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ValidateAccountExist_DiffDetail_DiffDOI_SmallerDOI 'Block Claim (Old Card)

            '--------------------------------------------------------------------------------
            'for Temp account
            '--------------------------------------------------------------------------------
            '8
            EHSAccountNotfound '1a1
            '9
            TempAccountExist_SameDetail '1a2    
            '10
            TempAccountExist_DiffDetail_NotCreateBySmartID '1a1

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            '11
            'TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB
            TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithGender ' Block (all existing personal info are read from smart id)
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            '12
            TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB  '1a1

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            '13
            TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName  '1a1 (may be selected wrong gender before, for ideas2 & 2.5 case)

            '14
            TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_DiffName ' Block
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            '' Rectify, Own Temp VRA Validation Fail, and no other TempVRA found
            'TempVAROtherNotfound
        End Enum

        Shared Function CheckSmartIDCardStatus(ByRef udtSmartIDEHSAccount As EHSAccountModel, ByRef udtExistingEHSAccount As EHSAccountModel) As SmartIDResultStatus

            Dim Formatter As New Common.Format.Formatter()

            ' *********** Please fill in the Logic ********************

            ' The Validation

            If udtExistingEHSAccount Is Nothing Then
                '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                ' No Accoun Created 
                '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                Return SmartIDResultStatus.EHSAccountNotfound

            Else
                Dim udtExistingPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
                Dim udtSmartIDPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
                Dim isSameEName As Boolean = True
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim blnIsGenderProvided As Boolean = False
                Dim isSameGender As Boolean = False
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                udtExistingPersonalInfo = udtExistingEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC)

                If udtExistingPersonalInfo Is Nothing Then
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    ' Account have no HKIC PersonalInformation may be HKBC 
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------


                    Return SmartIDResultStatus.DocTypeNotExist
                End If

                udtSmartIDPersonalInfo = udtSmartIDEHSAccount.getPersonalInformation(DocType.DocTypeModel.DocTypeCode.HKIC)

                If udtExistingPersonalInfo.CCCode1 Is Nothing Then udtExistingPersonalInfo.CCCode1 = String.Empty
                If udtExistingPersonalInfo.CCCode2 Is Nothing Then udtExistingPersonalInfo.CCCode2 = String.Empty
                If udtExistingPersonalInfo.CCCode3 Is Nothing Then udtExistingPersonalInfo.CCCode3 = String.Empty
                If udtExistingPersonalInfo.CCCode4 Is Nothing Then udtExistingPersonalInfo.CCCode4 = String.Empty
                If udtExistingPersonalInfo.CCCode5 Is Nothing Then udtExistingPersonalInfo.CCCode5 = String.Empty
                If udtExistingPersonalInfo.CCCode6 Is Nothing Then udtExistingPersonalInfo.CCCode6 = String.Empty
                If udtExistingPersonalInfo.ENameSurName Is Nothing Then udtExistingPersonalInfo.ENameSurName = String.Empty
                If udtExistingPersonalInfo.ENameFirstName Is Nothing Then udtExistingPersonalInfo.ENameFirstName = String.Empty
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If udtExistingPersonalInfo.Gender Is Nothing Then udtExistingPersonalInfo.Gender = String.Empty
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                If udtSmartIDPersonalInfo.CCCode1 Is Nothing Then udtSmartIDPersonalInfo.CCCode1 = String.Empty
                If udtSmartIDPersonalInfo.CCCode2 Is Nothing Then udtSmartIDPersonalInfo.CCCode2 = String.Empty
                If udtSmartIDPersonalInfo.CCCode3 Is Nothing Then udtSmartIDPersonalInfo.CCCode3 = String.Empty
                If udtSmartIDPersonalInfo.CCCode4 Is Nothing Then udtSmartIDPersonalInfo.CCCode4 = String.Empty
                If udtSmartIDPersonalInfo.CCCode5 Is Nothing Then udtSmartIDPersonalInfo.CCCode5 = String.Empty
                If udtSmartIDPersonalInfo.CCCode6 Is Nothing Then udtSmartIDPersonalInfo.CCCode6 = String.Empty
                If udtSmartIDPersonalInfo.ENameSurName Is Nothing Then udtSmartIDPersonalInfo.ENameSurName = String.Empty
                If udtSmartIDPersonalInfo.ENameFirstName Is Nothing Then udtSmartIDPersonalInfo.ENameFirstName = String.Empty


                isSameEName = SmartIDHandler.IsSameEName(udtExistingPersonalInfo, udtSmartIDPersonalInfo)

                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If udtSmartIDPersonalInfo.Gender Is Nothing Then udtSmartIDPersonalInfo.Gender = String.Empty

                If udtSmartIDPersonalInfo.Gender <> String.Empty Then
                    blnIsGenderProvided = True
                    If udtSmartIDPersonalInfo.Gender.Trim().Equals(udtExistingPersonalInfo.Gender.Trim()) Then
                        isSameGender = True
                    End If
                End If
                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                If udtExistingEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Add Checking for Gender if provided
                    If isSameEName _
                        AndAlso udtSmartIDPersonalInfo.DateofIssue.Equals(udtExistingPersonalInfo.DateofIssue) _
                        AndAlso udtSmartIDPersonalInfo.CCCode1.Trim().Equals(udtExistingPersonalInfo.CCCode1.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode2.Trim().Equals(udtExistingPersonalInfo.CCCode2.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode3.Trim().Equals(udtExistingPersonalInfo.CCCode3.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode4.Trim().Equals(udtExistingPersonalInfo.CCCode4.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode5.Trim().Equals(udtExistingPersonalInfo.CCCode5.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode6.Trim().Equals(udtExistingPersonalInfo.CCCode6.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.DOB.Equals(udtExistingPersonalInfo.DOB) _
                        AndAlso (Not blnIsGenderProvided OrElse isSameGender) Then
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        ' EHS Account Personal Informations are exact Match
                        '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        Return SmartIDResultStatus.ValidateAccountExist_SameDetail

                    Else
                        ' Different Detail
                        If udtSmartIDPersonalInfo.DateofIssue.Equals(udtExistingPersonalInfo.DateofIssue) Then
                            '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            ' At least DOI are matched
                            '-------------------------------------------------------------------------------------------------------------------------------------------------------------------

                            If udtSmartIDPersonalInfo.DOB.Equals(udtExistingPersonalInfo.DOB) Then
                                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                                ' DOB are matched
                                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                                'Validated Account -> is same DOI -> is same DOB

                                If udtExistingPersonalInfo.CreateBySmartID Then
                                    'Validated Account -> is same DOI -> is same DOB -> Created by SmartID

                                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                                    ' ----------------------------------------------------------------------------------------
                                    If udtExistingPersonalInfo.SmartIDVer = SmartIDVersion.IDEAS2_WithGender Then
                                        'Validated Account -> is same DOI -> is same DOB -> Created by SmartID -> With Gender
                                        'Block Claim, all info (include Gender) should be same for the same card
                                        Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithGender

                                    Else
                                        'Validated Account -> is same DOI -> is same DOB -> Created by SmartID -> Without Gender

                                        If isSameEName _
                                            AndAlso udtSmartIDPersonalInfo.CCCode1.Trim().Equals(udtExistingPersonalInfo.CCCode1.Trim()) _
                                            AndAlso udtSmartIDPersonalInfo.CCCode2.Trim().Equals(udtExistingPersonalInfo.CCCode2.Trim()) _
                                            AndAlso udtSmartIDPersonalInfo.CCCode3.Trim().Equals(udtExistingPersonalInfo.CCCode3.Trim()) _
                                            AndAlso udtSmartIDPersonalInfo.CCCode4.Trim().Equals(udtExistingPersonalInfo.CCCode4.Trim()) _
                                            AndAlso udtSmartIDPersonalInfo.CCCode5.Trim().Equals(udtExistingPersonalInfo.CCCode5.Trim()) _
                                            AndAlso udtSmartIDPersonalInfo.CCCode6.Trim().Equals(udtExistingPersonalInfo.CCCode6.Trim()) Then

                                            'Validated Account -> is same DOI -> is same DOB -> Created by SmartID -> Without Gender -> Same Name (Diff Gender)
                                            'Go to account modify Create a new Temp Account, may be selected wrong gender before, for ideas2 & 2.5 case)
                                            Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_SameName
                                        Else
                                            'Validated Account -> is same DOI -> is same DOB -> Created by SmartID -> Without Gender -> Diff Name
                                            'Block Claim, all info other than Gender is different
                                            Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOIDOB_CreateBySmartID_WithoutGender_DiffName
                                        End If
                                    End If
                                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                                Else
                                    'Validated Account -> is same DOI -> is same DOB -> not is Created by SmartID 

                                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                                    ' ----------------------------------------------------------------------------------------
                                    'If isSameEName Then
                                    If isSameEName AndAlso (Not blnIsGenderProvided OrElse isSameGender) Then
                                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                                        If udtExistingPersonalInfo.CCCode1.Trim().Equals(String.Empty) _
                                            AndAlso udtExistingPersonalInfo.CCCode2.Trim().Equals(String.Empty) _
                                            AndAlso udtExistingPersonalInfo.CCCode3.Trim().Equals(String.Empty) _
                                            AndAlso udtExistingPersonalInfo.CCCode4.Trim().Equals(String.Empty) _
                                            AndAlso udtExistingPersonalInfo.CCCode5.Trim().Equals(String.Empty) _
                                            AndAlso udtExistingPersonalInfo.CCCode6.Trim().Equals(String.Empty) Then
                                            '--------------------------------------------------------------------------------------------------------------------------------------------------------
                                            ' EHS Account in databse is manual created without CCCode
                                            '--------------------------------------------------------------------------------------------------------------------------------------------------------
                                            ' request to update CCCode
                                            Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB_NoCCCode
                                        Else
                                            ' Validated Account -> is same DOI -> is not created by Samrt IC -> is same DOB
                                            Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB
                                        End If
                                    Else
                                        ' Validated Account -> is same DOI -> is not created by Samrt IC -> is same DOB
                                        Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_NotCreateBySmartID_SameDOB
                                    End If

                                End If

                            Else
                                'Validated Account -> is same DOI -> is not same DOB
                                'Block Claim (no new/old card)(IMMD validation passed before)
                                Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_SameDOI_DiffDOB
                            End If
                            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        Else
                            '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            ' DOI are not matched
                            '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            '3.1.b: validated Account -> Diff DOI

                            If udtSmartIDPersonalInfo.DateofIssue.Value < udtExistingPersonalInfo.DateofIssue.Value Then
                                'Block Claim (Old Card)
                                '3.1.b.I : validated Account -> is not same DOI -> CFD DOI < EHS DOI
                                Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_SmallerDOI
                            Else
                                'Smart ID is new Card (Create a new Temp Account)
                                '3.1.b.II : validated Account -> is not same DOI -> is not CFD DOI < EHS DOI 
                                Return SmartIDResultStatus.ValidateAccountExist_DiffDetail_DiffDOI_LargerDOI
                            End If
                        End If
                    End If

                ElseIf udtExistingEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    ' Case Search Temp Account By Smart ID HKID 
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    '3.3: Temp VRA Found

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Add Checking for Gender if provided
                    If isSameEName _
                        AndAlso udtSmartIDPersonalInfo.DateofIssue.Equals(udtExistingPersonalInfo.DateofIssue) _
                        AndAlso udtSmartIDPersonalInfo.CCCode1.Trim().Equals(udtExistingPersonalInfo.CCCode1.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode2.Trim().Equals(udtExistingPersonalInfo.CCCode2.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode3.Trim().Equals(udtExistingPersonalInfo.CCCode3.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode4.Trim().Equals(udtExistingPersonalInfo.CCCode4.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode5.Trim().Equals(udtExistingPersonalInfo.CCCode5.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.CCCode6.Trim().Equals(udtExistingPersonalInfo.CCCode6.Trim()) _
                        AndAlso udtSmartIDPersonalInfo.DOB.Equals(udtExistingPersonalInfo.DOB) _
                        AndAlso (Not blnIsGenderProvided OrElse isSameGender) Then
                        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                        'Case 9: Temp VRA Found -> Account details are same
                        Return SmartIDResultStatus.TempAccountExist_SameDetail
                    Else

                        If udtExistingPersonalInfo.CreateBySmartID Then

                            If udtSmartIDPersonalInfo.DateofIssue.Equals(udtExistingPersonalInfo.DateofIssue) AndAlso _
                                udtSmartIDPersonalInfo.DOB.Equals(udtExistingPersonalInfo.DOB) Then

                                ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                                ' ----------------------------------------------------------------------------------------
                                If udtExistingPersonalInfo.SmartIDVer = SmartIDVersion.IDEAS2_WithGender Then
                                    'case 11: Temp VRA Found -> Diff detail -> is created by smart ID -> Same DOI DOB -> With Gender
                                    'Block Claim (Same Card)
                                    Return SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithGender

                                Else
                                    'Temp VRA Found -> Diff detail -> is created by smart ID -> Same DOI DOB -> Without Gender
                                    If isSameEName _
                                        AndAlso udtSmartIDPersonalInfo.CCCode1.Trim().Equals(udtExistingPersonalInfo.CCCode1.Trim()) _
                                        AndAlso udtSmartIDPersonalInfo.CCCode2.Trim().Equals(udtExistingPersonalInfo.CCCode2.Trim()) _
                                        AndAlso udtSmartIDPersonalInfo.CCCode3.Trim().Equals(udtExistingPersonalInfo.CCCode3.Trim()) _
                                        AndAlso udtSmartIDPersonalInfo.CCCode4.Trim().Equals(udtExistingPersonalInfo.CCCode4.Trim()) _
                                        AndAlso udtSmartIDPersonalInfo.CCCode5.Trim().Equals(udtExistingPersonalInfo.CCCode5.Trim()) _
                                        AndAlso udtSmartIDPersonalInfo.CCCode6.Trim().Equals(udtExistingPersonalInfo.CCCode6.Trim()) Then

                                        'Temp VRA Found -> Diff detail -> is created by smart ID -> Same DOI DOB -> Without Gender -> Same Name (Diff Gender)
                                        Return SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_SameName

                                    Else
                                        'Temp VRA Found -> Diff detail -> is created by smart ID -> Same DOI DOB -> Without Gender -> Diff Name
                                        Return SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_SameDOIDOB_WithoutGender_DiffName
                                    End If
                                End If
                                ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                            Else
                                'Case 12: Temp VRA Found -> Account details are not same -> is created by smart ID -> DOI and DOB are not same as existing account
                                Return SmartIDResultStatus.TempAccountExist_DiffDetail_CreateBySmartID_DiffDOIDOB
                            End If
                        Else
                            'case 10: Temp VRA Found -> Account details are not same -> Not is created by smart ID
                            Return SmartIDResultStatus.TempAccountExist_DiffDetail_NotCreateBySmartID

                        End If
                    End If
                End If

                Return SmartIDResultStatus.Empty
            End If
        End Function

        Public Shared Function IsSameEName(ByVal udtExistingPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSmartIDPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean
            Dim udtFormatter As New Common.Format.Formatter()
            Return udtFormatter.formatEnglishName(udtSmartIDPersonalInfo.ENameSurName.ToUpper(), udtSmartIDPersonalInfo.ENameFirstName.ToUpper()).Equals( _
                        udtFormatter.formatEnglishName(udtExistingPersonalInfo.ENameSurName, udtExistingPersonalInfo.ENameFirstName))
        End Function

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Shared Function IsExistingOldSmartIDVersion(ByVal strExistSmartIDVer As String, ByVal strCurrentSmartIDVer As String) As Boolean
            Return CompareVersion(strExistSmartIDVer, strCurrentSmartIDVer, "<")
        End Function

        Public Shared Function CompareVersion(ByVal strExistSmartIDVer As String, ByVal strCurrentSmartIDVer As String, ByVal strOperator As String) As Boolean
            Dim blnSame As Boolean = False

            ' Assume Empty version be the oldest version
            If strExistSmartIDVer = String.Empty Then
                strExistSmartIDVer = SmartIDVersion.IDEAS1
            End If

            If strCurrentSmartIDVer = String.Empty Then
                strCurrentSmartIDVer = SmartIDVersion.IDEAS1
            End If

            Select Case strOperator.Trim()
                Case ">"
                    Return (CInt(strExistSmartIDVer) > CInt(strCurrentSmartIDVer))
                Case ">="
                    Return (CInt(strExistSmartIDVer) >= CInt(strCurrentSmartIDVer))
                Case "="
                    Return (CInt(strExistSmartIDVer) = CInt(strCurrentSmartIDVer))
                Case "<"
                    Return (CInt(strExistSmartIDVer) < CInt(strCurrentSmartIDVer))
                Case "<="
                    Return (CInt(strExistSmartIDVer) <= CInt(strCurrentSmartIDVer))
                Case "<>"
                    Return (CInt(strExistSmartIDVer) <> CInt(strCurrentSmartIDVer))
                Case Else
                    Return False

            End Select
        End Function

        ' Check web config to see if enable, Hide Smart ID if not enabled
        Public Shared ReadOnly Property EnableSmartID() As Boolean
            Get
                Dim strEnableSmartID As String = ConfigurationManager.AppSettings("EnableSmartID")

                If Not IsNothing(strEnableSmartID) AndAlso strEnableSmartID = YesNo.Yes Then
                    Return True
                End If

                Return False
            End Get
        End Property

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Check system parameters to see if turn on, Smart ID will be dimmed if off
        Public Shared ReadOnly Property TurnOnSmartID() As Boolean
            Get
                Dim strParmValue As String = String.Empty
                Dim udtGeneralFunction As New GeneralFunction
                udtGeneralFunction.getSystemParameter("TurnOnSmartID", strParmValue, String.Empty)

                If strParmValue = YesNo.Yes Then
                    Return True
                End If

                Return False
            End Get
        End Property
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
    End Class

    <Serializable()> Public Class SmartIDContentModel

        Dim _udtIdeasTokenResponse As IdeasRM.TokenResponse
        Dim _enumSmartIDReadStatus As SmartIDHandler.SmartIDResultStatus
        Dim _udEHSAccount As EHSAccountModel
        Dim _udEHSValidatedAccount As EHSAccountModel
        Dim _isDemoVersion As Boolean
        Dim _isReadSmartID As Boolean = False
        Dim _isEndOfReadSmartID As Boolean
        Dim _isHighLightGender As Boolean = False
        Dim _isCreateAmendEHSAccount As Boolean = False
        Dim _isPilotRunSP As Boolean = False
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim _enumIdeasVersion As IdeasBLL.EnumIdeasVersion
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        Public Sub New()
            If Not _isPilotRunSP AndAlso _isReadSmartID Then
                Throw New Exception("Service Provider is not in the list of Pilot Run!")
            End If
        End Sub


        Public Property TokenResponse() As IdeasRM.TokenResponse
            Get
                Return Me._udtIdeasTokenResponse
            End Get
            Set(ByVal value As IdeasRM.TokenResponse)
                Me._udtIdeasTokenResponse = value
            End Set
        End Property

        Public Property SmartIDReadStatus() As SmartIDHandler.SmartIDResultStatus
            Get
                Return Me._enumSmartIDReadStatus
            End Get
            Set(ByVal value As SmartIDHandler.SmartIDResultStatus)
                Me._enumSmartIDReadStatus = value
            End Set
        End Property

        Public Property EHSAccount() As EHSAccountModel
            Get
                Return Me._udEHSAccount
            End Get
            Set(ByVal value As EHSAccountModel)
                Me._udEHSAccount = value
            End Set
        End Property

        Public Property EHSValidatedAccount() As EHSAccountModel
            Get
                Return Me._udEHSValidatedAccount
            End Get
            Set(ByVal value As EHSAccountModel)
                Me._udEHSValidatedAccount = value
            End Set
        End Property


        Public Property IsReadSmartID() As Boolean
            Get
                Return Me._isReadSmartID
            End Get
            Set(ByVal value As Boolean)
                Me._isReadSmartID = value
            End Set
        End Property

        Public Property IsDemonVersion() As Boolean
            Get
                Return _isDemoVersion
            End Get
            Set(ByVal value As Boolean)
                Me._isDemoVersion = value
            End Set
        End Property

        'Public Property IsPrintedChangeForm() As Boolean
        '    Get
        '        Return _isPrintedChangeForm
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Me._isPrintedChangeForm = value
        '    End Set
        'End Property

        Public Property IsEndOfReadSmartID() As Boolean
            Get
                Return _isEndOfReadSmartID
            End Get
            Set(ByVal value As Boolean)
                Me._isEndOfReadSmartID = value
            End Set
        End Property

        Public Property HighLightGender() As Boolean
            Get
                Return _isHighLightGender
            End Get
            Set(ByVal value As Boolean)
                Me._isHighLightGender = value
            End Set
        End Property

        Public Property IsCreateAmendEHSAccount() As Boolean
            Get
                Return _isCreateAmendEHSAccount
            End Get
            Set(ByVal value As Boolean)
                Me._isCreateAmendEHSAccount = value
            End Set
        End Property

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Property IdeasVersion() As IdeasBLL.EnumIdeasVersion
            Get
                Return _enumIdeasVersion
            End Get
            Set(ByVal value As IdeasBLL.EnumIdeasVersion)
                Me._enumIdeasVersion = value
            End Set
        End Property

        Public ReadOnly Property SmartIDVer() As String
            Get
                Dim strSmartIDVer As String = String.Empty

                Select Case Me._enumIdeasVersion
                    Case IdeasBLL.EnumIdeasVersion.One
                        strSmartIDVer = SmartIDVersion.IDEAS1
                    Case IdeasBLL.EnumIdeasVersion.Two
                        strSmartIDVer = SmartIDVersion.IDEAS2
                    Case IdeasBLL.EnumIdeasVersion.TwoGender
                        strSmartIDVer = SmartIDVersion.IDEAS2_WithGender
                End Select

                Return strSmartIDVer
            End Get
        End Property
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
    End Class

End Namespace