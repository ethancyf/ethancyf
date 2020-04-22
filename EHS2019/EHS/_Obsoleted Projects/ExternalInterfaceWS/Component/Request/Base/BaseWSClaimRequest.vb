Imports Microsoft.VisualBasic
Imports System.Xml
Imports Common.Component.DocType
Imports Common.Validation
Imports Common.Component.StaticData
Imports Common.Component.EHSTransaction
Imports Common.Component.ReasonForVisit
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSClaim
Imports ExternalInterfaceWS.BLL
Imports Common.Component
Imports Common.DataAccess
Imports Common.Component.RVPHomeList
Imports Common.ComObject
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports System.Globalization
Imports Common.ComFunction.GeneralFunction

Namespace Component.Request.Base

    Public MustInherit Class BaseWSClaimRequest
        Inherits BaseWSAccountRequest

#Region "Protected Constant"

        Protected Const TAG_CLAIM_INFO As String = "ClaimInfo"
        Protected Const TAG_CLAIM_DETAIL As String = "ClaimDetail"
        Protected Const TAG_SERVICE_DATE As String = "ServiceDate"
        Protected Const TAG_SCHEME_CODE As String = "SchemeCode"
        Protected Const TAG_VOUCHER_INFO As String = "VoucherInfo"
        Protected Const TAG_VOUCHER_CLAIMED As String = "VoucherClaimed"

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Protected Const TAG_COPAYMENT_FEE As String = "CoPaymentFee"

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Protected Const TAG_REASON_FOR_VISIT As String = "ReasonForVisit"
        Protected Const TAG_PROF_CODE As String = "ProfCode"
        Protected Const TAG_L1_CODE As String = "L1Code"
        Protected Const TAG_L1_DESC_ENG As String = "L1DescEng"
        Protected Const TAG_L2_CODE As String = "L2Code"
        Protected Const TAG_L2_DESC_ENG As String = "L2DescEng"
        Protected Const TAG_VACCINE_INFO As String = "VaccineInfo"
        Protected Const TAG_SUBSIDY_CODE As String = "SubsidyCode"
        Protected Const TAG_DOSE_SEQ As String = "DoseSeq"
        Protected Const TAG_RCH_CODE As String = "RCHCode"
        Protected Const TAG_PRE_SCHOOL_IND As String = "PreSchoolInd"
        Protected Const TAG_DOSE_INTERVAL_IND As String = "DoseIntervalInd"
        Protected Const TAG_TSW_IND As String = "TSWInd"

#End Region

#Region "Properties"

        Private _strServiceDate = String.Empty
        Private _dtmServiceDate As Nullable(Of Date)
        Public Property ServiceDate() As Nullable(Of Date)
            Get
                Return Me._dtmServiceDate
            End Get
            Set(ByVal value As Nullable(Of Date))
                Me._dtmServiceDate = value
            End Set
        End Property

        Private _udtWSClaimDetaillList As WSClaimDetailModelCollection
        Public Property WSClaimDetailList() As WSClaimDetailModelCollection
            Get
                Return Me._udtWSClaimDetaillList
            End Get
            Set(ByVal value As WSClaimDetailModelCollection)
                Me._udtWSClaimDetaillList = value
            End Set
        End Property

        Private _ServiceDate_Received As Boolean = False
        Public Property ServiceDate_Received() As Boolean
            Get
                Return _ServiceDate_Received
            End Get
            Set(ByVal value As Boolean)
                _ServiceDate_Received = value
            End Set
        End Property


#End Region

#Region "(Step 3) Fields Validation"

        Protected Function ValidateClaimInfo(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            Dim isValid As Boolean = True
            Dim udtSchemeClaimBLL As New Common.Component.Scheme.SchemeClaimBLL
            Dim udtSchemeClaim As SchemeClaimModel = Nothing
            Dim udtSM As Common.ComObject.SystemMessage = Nothing
            Dim strMessageCode As String = String.Empty

            'Service date
            Dim strServiceDate As String = String.Empty
            Dim dtServiceDate As DateTime
            If _strServiceDate.trim = String.Empty Then
                _strServiceDate = _dtmServiceDate.Value.ToString("dd-MM-yyyy")
            End If
            strServiceDate = Me.udtformatter.formatDateBeforValidation_DDMMYYYY(_strServiceDate)
            strMessageCode = Me.udtvalidator.chkValidSearchDate(strServiceDate)
            If Not strMessageCode.Trim = String.Empty Then
                udtErrorList.Add(ErrorCodeList.I00037) 'Incorrect input parameter of "Service Date" 
            Else
                dtServiceDate = CDate(Me.udtformatter.convertDate(Me.udtformatter.formatInputDate(strServiceDate), Common.Component.CultureLanguage.English))
                Me.ServiceDate = dtServiceDate
            End If

            For Each udtWSClaimDetail As WSClaimDetailModel In Me._udtWSClaimDetaillList
                If Not IsNothing(udtWSClaimDetail) Then

                    'Check Scheme Code
                    Dim blnSchemeCodefound As Boolean = False
                    ' CRE13-001 - EHAPP [Start][Koala]
                    ' -------------------------------------------------------------------------------------
                    Dim blnSchemePCSClaimAvailable As Boolean = False
                    ' CRE13-001 - EHAPP [End][Koala]
                    For Each udtScheme As SchemeClaimModel In udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup
                        If udtScheme.SchemeCode.Trim.ToUpper = udtWSClaimDetail.SchemeCode.Trim.ToUpper Then
                            blnSchemeCodefound = True
                            blnSchemePCSClaimAvailable = udtScheme.PCSClaimAvailable
                            udtSchemeClaim = udtScheme
                            Exit For
                        End If
                    Next

                    ' CRE13-001 - EHAPP [Start][Koala]
                    ' -------------------------------------------------------------------------------------
                    If Not blnSchemeCodefound Or Not blnSchemePCSClaimAvailable Then
                        udtErrorList.Add(ErrorCodeList.I00038) 'Incorrect input parameter of Scheme Code
                        Return False
                    Else
                        udtWSClaimDetail.SchemeCode = udtWSClaimDetail.SchemeCode.Trim.ToUpper
                    End If
                    ' CRE13-001 - EHAPP [End][Koala]

                    'Check RCH Code
                    If Not IsNothing(udtWSClaimDetail.RCHCode) AndAlso udtWSClaimDetail.RCHCode.Trim <> String.Empty Then
                        Dim udtRVPHomeListBLL As RVPHomeListBLL = New RVPHomeListBLL()
                        Dim dtResult As DataTable
                        dtResult = udtRVPHomeListBLL.getRVPHomeListActiveByCode(udtWSClaimDetail.RCHCode)

                        If dtResult.Rows.Count = 0 Then
                            udtErrorList.Add(ErrorCodeList.I00044) 'Incorrect input parameter of claim information
                            Return False
                        End If
                    End If

                    '-----------------------------------------------
                    ' Check Voucher
                    '-----------------------------------------------
                    If isValid AndAlso Not IsNothing(udtWSClaimDetail.WSVoucherList) Then
                        For Each WSVoucher As WSVoucherModel In udtWSClaimDetail.WSVoucherList
                            isValid = Me.ValidateVoucherInfo(WSVoucher, udtErrorList)
                            If Not isValid Then
                                Return False
                            End If
                        Next
                    End If
                    '-----------------------------------------------
                    ' Check Vaccine
                    '-----------------------------------------------
                    If isValid AndAlso Not IsNothing(udtWSClaimDetail.WSVaccineDetailList) AndAlso udtWSClaimDetail.WSVaccineDetailList.Count > 0 Then
                        For Each udtWSVaccineDetail As WSVaccineDetailModel In udtWSClaimDetail.WSVaccineDetailList
                            isValid = Me.ValidateVaccineInfo(udtWSVaccineDetail, udtErrorList)
                            If Not isValid Then
                                Return False
                            End If
                        Next
                    End If
                    '-----------------------------------------------
                    'Check Indicator
                    '-----------------------------------------------
                    'PreSchoolInd
                    If udtWSClaimDetail.PreSchoolInd_Received Then
                        If udtWSClaimDetail.PreSchoolInd.Trim.ToUpper = "Y" Or udtWSClaimDetail.PreSchoolInd.Trim.ToUpper = "N" Then
                            udtWSClaimDetail.PreSchoolInd = udtWSClaimDetail.PreSchoolInd.Trim.ToUpper
                        Else
                            udtErrorList.Add(ErrorCodeList.I00045) 'Incorrect input parameter of "PreSchoolInd"
                            Return False
                        End If
                    End If

                    'DoseIntervalInd
                    If udtWSClaimDetail.DoseIntervalInd_Received Then
                        'If Not (udtWSClaimDetail.SchemeCode.Trim.ToUpper = "CSIV" Or _
                        '        udtWSClaimDetail.SchemeCode.Trim.ToUpper = "RVP") Then
                        '    udtErrorList.Add(ErrorCodeList.I00011) 'Invalid input message
                        '    Return False
                        'End If

                        ' CRE13-001 - EHAPP [Start][Koala]
                        ' -------------------------------------------------------------------------------------
                        If Not (New VaccinationBLL).SchemeContainVaccine(udtSchemeClaim) Then
                            Return False
                        End If
                        ' CRE13-001 - EHAPP [End][Koala]

                        If udtWSClaimDetail.DoseIntervalInd.Trim.ToUpper = "Y" Or udtWSClaimDetail.DoseIntervalInd.Trim.ToUpper = "N" Then
                            udtWSClaimDetail.DoseIntervalInd = udtWSClaimDetail.DoseIntervalInd.Trim.ToUpper
                        Else
                            udtErrorList.Add(ErrorCodeList.I00056) 'Incorrect input parameter of "DoseIntervalInd"
                            Return False
                        End If
                    End If

                    'TSWInd
                    If udtWSClaimDetail.TSWInd_Received Then
                        If Not udtWSClaimDetail.SchemeCode.Trim.ToUpper = "HCVS" Then
                            udtErrorList.Add(ErrorCodeList.I00011) 'Invalid input message
                            Return False
                        End If
                        If udtWSClaimDetail.TSWInd.Trim.ToUpper = "Y" Or udtWSClaimDetail.TSWInd.Trim.ToUpper = "N" Then
                            udtWSClaimDetail.TSWInd = udtWSClaimDetail.TSWInd.Trim.ToUpper
                        Else
                            udtErrorList.Add(ErrorCodeList.I00057) 'Incorrect input parameter of "TSWInd"
                            Return False
                        End If
                    End If

                End If
            Next

            Return isValid
        End Function

        Private Function ValidateVoucherInfo(ByVal udtWSVoucher As WSVoucherModel, ByRef udtErrorList As ErrorInfoModelCollection) As Boolean

            'Voucher Claimed
            Dim intVoucherClaimed As Integer
            If Not IsNothing(udtWSVoucher.VoucherClaimed) AndAlso Integer.TryParse(udtWSVoucher.VoucherClaimed, intVoucherClaimed) Then
                If intVoucherClaimed < 1 Or intVoucherClaimed > 99 Then
                    udtErrorList.Add(ErrorCodeList.I00039) 'Incorrect input parameter of claim information
                    Return False
                End If
            Else
                udtErrorList.Add(ErrorCodeList.I00039) 'Incorrect input parameter of claim information
                Return False
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            'CoPayment Fee
            Dim intCoPaymentFee As Integer
            If Not IsNothing(udtWSVoucher.CoPaymentFee) AndAlso Integer.TryParse(udtWSVoucher.CoPaymentFee, intCoPaymentFee) Then
                If intCoPaymentFee < 0 Or intCoPaymentFee > 9999 Then
                    udtErrorList.Add(ErrorCodeList.I00113) 'Incorrect input parameter of Co-Payment Fee
                    Return False
                End If
            End If

            For Each RFVModel As ReasonForVisitModel In udtWSVoucher.ReasonForVisitModelCollection

                If IsNothing(RFVModel.ProfCode) Or IsNothing(RFVModel.PriorityCode) Or IsNothing(RFVModel.L1Code) Or IsNothing(RFVModel.L1DescEng) Or IsNothing(RFVModel.L2Code) Or IsNothing(RFVModel.L2DescEng) Then
                    udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                    Return False
                End If

                Dim udtSystemMessage As SystemMessage
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                udtSystemMessage = udtvalidator.chkReasonForVisit(RFVModel.ProfCode, RFVModel.L1Code, RFVModel.L2Code)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                If Not udtSystemMessage Is Nothing Then
                    udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                    Return False
                End If


                Dim udtReasonForVisitBLL As New ReasonForVisitBLL
                Dim dtReasonForVisit As DataTable

                'L1 

                dtReasonForVisit = udtReasonForVisitBLL.getReasonForVisitL1(RFVModel.ProfCode, RFVModel.L1Code)
                If dtReasonForVisit.Rows.Count = 0 Then
                    udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                    Return False
                End If

                If dtReasonForVisit.Rows(0).Item("Reason_L1").ToString.Trim <> RFVModel.L1DescEng.Trim Then
                    udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                    Return False
                End If

                'L2

                dtReasonForVisit = udtReasonForVisitBLL.getReasonForVisitL2(RFVModel.ProfCode, RFVModel.L1Code, RFVModel.L2Code)
                If dtReasonForVisit.Rows.Count = 0 Then
                    udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                    Return False
                End If

                If dtReasonForVisit.Rows(0).Item("Reason_L2").ToString.Trim <> RFVModel.L2DescEng.Trim Then
                    udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                    Return False
                End If

                'Dim blnValidL2Reason As Boolean = False
                'For Each dtRow As DataRow In dtReasonForVisit.Rows
                '    If dtRow.Item("Reason_L2_Code").ToString.Trim = RFVModel.L2Code.ToString().Trim And dtRow.Item("Reason_L2").ToString.Trim = RFVModel.L2DescEng.Trim Then
                '        blnValidL2Reason = True
                '        Exit For
                '    End If
                'Next

                'If Not blnValidL2Reason Then
                '    udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                '    Return False
                'End If

            Next

            'Validation Rules For Multiple Reason For Visit

            If udtWSVoucher.ReasonForVisitModelCollection.Count > Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length Then
                udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                Return False
            End If

            If Not udtWSVoucher.PassPriorityCodeRangeCheck Then
                udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                Return False
            End If

            If Not udtWSVoucher.PriorityCodeDistinct Then
                udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                Return False
            End If


            If udtWSVoucher.ProfCodeUnmatched Then
                udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                Return False
            End If

            If udtWSVoucher.ReasonForVisitedDuplicated Then
                udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                Return False
            End If

            If Not udtWSVoucher.HasPrimaryReasonForVisit And udtWSVoucher.HasSecondaryReasonForVisit Then
                udtErrorList.Add(ErrorCodeList.I00040) 'Incorrect input parameter of Reason For Visit
                Return False
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Return True
        End Function

        Private Function ValidateVaccineInfo(ByVal udtWSVaccineDetail As WSVaccineDetailModel, ByRef udtErrorList As ErrorInfoModelCollection) As Boolean
            'Subsidy Code
            Dim udtSchemeClaimBLL As New Common.Component.Scheme.SchemeClaimBLL
            Dim blnSchemeCodefound As Boolean = False
            For Each udtScheme As SchemeClaimModel In udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup
                For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtScheme.SubsidizeGroupClaimList
                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    If UploadClaimBLL.UseParticularSubsidyCode Then
                        If udtSubsidizeGroupClaim.DisplayCodeForClaim = udtWSVaccineDetail.SubsidyCode Then
                            blnSchemeCodefound = True
                        End If
                    Else
                        If udtSubsidizeGroupClaim.SubsidizeDisplayCode = udtWSVaccineDetail.SubsidyCode Then
                            blnSchemeCodefound = True
                        End If
                    End If

                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
                Next
            Next
            If Not blnSchemeCodefound Then
                udtErrorList.Add(ErrorCodeList.I00041) 'Incorrect input parameter of claim information
                Return False
            End If

            'Dose Seq
            If udtWSVaccineDetail.DoseSeq.Trim.ToUpper <> "1ST" And udtWSVaccineDetail.DoseSeq.Trim.ToUpper <> "2ND" And udtWSVaccineDetail.DoseSeq.Trim.ToUpper <> "N/A" Then
                udtErrorList.Add(ErrorCodeList.I00042) 'Incorrect input parameter of claim information
                Return False
            Else
                If udtWSVaccineDetail.DoseSeq.Trim.ToUpper = "N/A" Then
                    udtWSVaccineDetail.DoseSeq = "NA"
                End If
                If udtWSVaccineDetail.DoseSeq.Trim.ToUpper = "1ST" Then
                    udtWSVaccineDetail.DoseSeq = "1ST"
                End If
                If udtWSVaccineDetail.DoseSeq.Trim.ToUpper = "2ND" Then
                    udtWSVaccineDetail.DoseSeq = "2ND"
                End If

            End If

            Return True

        End Function

#End Region

#Region "(Step 2) Check whether there is missing or duplicate fields"

        Protected Function CheckClaimXMLField(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            Dim IsAllReceived As Boolean

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            'Service date
            If Me._ServiceDate_Received = False Then
                Return False
            End If

            For Each udtWSClaimDetail As WSClaimDetailModel In Me._udtWSClaimDetaillList
                If Not IsNothing(udtWSClaimDetail) Then

                    'Check Scheme Code
                    If udtWSClaimDetail.SchemeCode_Received = False Then
                        udtErrorList.Add(ErrorCodeList.I00010)
                        Return False
                    End If

                    If udtWSClaimDetail.SchemeCode.Trim.ToUpper = "HCVS" Then
                        ' Check Voucher
                        If IsNothing(udtWSClaimDetail.WSVoucherList) Then
                            udtErrorList.Add(ErrorCodeList.I00010)
                            Return False
                        Else
                            If udtWSClaimDetail.WSVoucherList.Count > 0 Then
                                For Each udtWSVoucher As WSVoucherModel In udtWSClaimDetail.WSVoucherList

                                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

                                    ' -----------------------------------------------------------------------------------------

                                    IsAllReceived = True

                                    If Not udtWSVoucher.VoucherClaimed_Received Then
                                        IsAllReceived = False
                                    End If

                                    'If Not udtWSVoucher.CoPaymentFee_Received Then
                                    '    IsAllReceived = False
                                    'End If

                                    'For Each udtReasonForVisitModel As ReasonForVisitModel In udtWSVoucher.ReasonForVisitModelCollection
                                    '    If Not udtReasonForVisitModel.ProfCode_Received Or Not udtReasonForVisitModel.PriorityCode_Received Or Not udtReasonForVisitModel.L1Code_Received Or Not udtReasonForVisitModel.L1DescEng_Received Or Not udtReasonForVisitModel.L2Code_Received Or Not udtReasonForVisitModel.L2DescEng_Received Then
                                    '        IsAllReceived = False
                                    '    End If
                                    'Next

                                    If Not IsAllReceived Then
                                        udtErrorList.Add(ErrorCodeList.I00010)
                                        Return False
                                    End If

                                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                                Next
                            End If
                        End If
                    Else
                        ' Check Vaccine
                        If IsValid AndAlso Not IsNothing(udtWSClaimDetail.WSVaccineDetailList) AndAlso udtWSClaimDetail.WSVaccineDetailList.Count > 0 Then
                            For Each udtWSVaccineDetail As WSVaccineDetailModel In udtWSClaimDetail.WSVaccineDetailList
                                If udtWSVaccineDetail.SubsidyCode_Received = False Or _
                                    udtWSVaccineDetail.DoseSeq_Received = False Then
                                    udtErrorList.Add(ErrorCodeList.I00010)
                                    Return False
                                End If
                            Next
                        End If
                    End If

                End If
            Next

            Return True

        End Function

#End Region

#Region "(Step 1) Read XML"

#Region "Read Claim Info"

        Protected Sub ReadClaimInfo(ByVal xml As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

            Dim strSchemeCode As String = String.Empty
            Dim blnSchemeCode As Boolean = False
            Dim strPreSchoolInd As String = String.Empty
            Dim blnPreSchoolInd As Boolean = False
            Dim strDoseIntervalInd As String = String.Empty
            Dim blnDoseIntervalInd As Boolean = False
            Dim strTSWInd As String = String.Empty
            Dim blnTSWInd As Boolean = False
            Dim udtWSClaimDetail As WSClaimDetailModel = Nothing
            Dim strRCHCode As String = String.Empty
            Dim blnRCHCode As Boolean = False

            Dim nlClaimInfo As XmlNodeList = xml.GetElementsByTagName(TAG_CLAIM_INFO)

            If nlClaimInfo.Count = 0 Then
                udtErrorList.Add(ErrorCodeList.I00004)  'Incorrect XML format
                Return
            ElseIf nlClaimInfo.Count > 1 Then
                udtErrorList.Add(ErrorCodeList.I00004)  'Incorrect XML format
                Return
            End If

            'Read Service Date
            ReadServiceDate(nlClaimInfo.Item(0), udtErrorList)

            Dim nlClaimDetails As XmlNodeList = nlClaimInfo.Item(0).SelectNodes("./" + TAG_CLAIM_DETAIL)
            Dim nodeClaimDetail As XmlNode
            If IsNothing(_udtWSClaimDetaillList) Then
                _udtWSClaimDetaillList = New WSClaimDetailModelCollection()
                '_udtWSClaimDetaillList.Clear()
            End If

            For i As Integer = 0 To nlClaimDetails.Count - 1
                nodeClaimDetail = nlClaimDetails.Item(i)

                'Scheme Code
                ReadSchemeCode(nodeClaimDetail, udtErrorList, strSchemeCode, blnSchemeCode)
                udtWSClaimDetail = New WSClaimDetailModel()
                udtWSClaimDetail.SchemeCode = strSchemeCode
                udtWSClaimDetail.SchemeCode_Received = blnSchemeCode

                'RCH Code
                ReadRCHCode(nodeClaimDetail, udtErrorList, strRCHCode, blnRCHCode)
                udtWSClaimDetail.RCHCode = strRCHCode
                udtWSClaimDetail.RCHCode_Received = blnRCHCode

                '-----------------------------
                'Indicator 
                '-----------------------------
                'PreSchoolInd
                ReadPreSchoolInd(nodeClaimDetail, udtErrorList, strPreSchoolInd, blnPreSchoolInd)
                udtWSClaimDetail.PreSchoolInd = strPreSchoolInd
                udtWSClaimDetail.PreSchoolInd_Received = blnPreSchoolInd
                'DoseIntervalInd
                ReadDoseIntervalInd(nodeClaimDetail, udtErrorList, strDoseIntervalInd, blnDoseIntervalInd)
                udtWSClaimDetail.DoseIntervalInd = strDoseIntervalInd
                udtWSClaimDetail.DoseIntervalInd_Received = blnDoseIntervalInd
                'TSWInd
                ReadTSWInd(nodeClaimDetail, udtErrorList, strTSWInd, blnTSWInd)
                udtWSClaimDetail.TSWInd = strTSWInd
                udtWSClaimDetail.TSWInd_Received = blnTSWInd

                _udtWSClaimDetaillList.Add(udtWSClaimDetail)

                '-----------------------------
                'Voucher (Multi)
                '-----------------------------
                Dim nlVoucherInfo As XmlNodeList = nodeClaimDetail.SelectNodes("./" + TAG_VOUCHER_INFO)
                Dim nodeVoucher As XmlNode
                Dim udtVoucher As WSVoucherModel = Nothing

                If nlVoucherInfo.Count > 0 Then
                    udtWSClaimDetail.WSVoucherList = New WSVoucherModelCollection()
                    For j As Integer = 0 To nlVoucherInfo.Count - 1
                        nodeVoucher = nlVoucherInfo.Item(i)
                        udtVoucher = New WSVoucherModel()
                        ReadVoucherInfo(nodeVoucher, udtErrorList, udtVoucher)
                        udtWSClaimDetail.WSVoucherList.Add(udtVoucher)
                    Next
                End If

                '-----------------------------
                'Vaccine (Multi)
                '-----------------------------
                Dim nlVaccineInfo As XmlNodeList = nodeClaimDetail.SelectNodes("./" + TAG_VACCINE_INFO)
                Dim nodeVaccineDetail As XmlNode
                Dim udtVaccineDetail As WSVaccineDetailModel = Nothing

                If nlVaccineInfo.Count > 0 Then
                    udtWSClaimDetail.WSVaccineDetailList = New WSVaccineDetailModelCollection()
                    For j As Integer = 0 To nlVaccineInfo.Count - 1
                        nodeVaccineDetail = nlVaccineInfo.Item(i)
                        udtVaccineDetail = New WSVaccineDetailModel()
                        ReadVaccineInfo(nodeVaccineDetail, udtErrorList, udtVaccineDetail)
                        udtWSClaimDetail.WSVaccineDetailList.Add(udtVaccineDetail)
                    Next
                End If

            Next
        End Sub

        Private Sub ReadServiceDate(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            _strServiceDate = ReadString(nodeClaimDetail, TAG_SERVICE_DATE, udtErrorList, ErrorCodeList.I00010, False, _ServiceDate_Received)
        End Sub

        Private Sub ReadSchemeCode(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                    ByRef SchemeCode As String, ByRef _SchemeCode_Received As Boolean)
            SchemeCode = ReadString(nodeClaimDetail, TAG_SCHEME_CODE, udtErrorList, ErrorCodeList.I00010, False, _SchemeCode_Received)
        End Sub

        Private Sub ReadRCHCode(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                            ByRef RCHCode As String, ByRef _RCHCode_Received As Boolean)
            RCHCode = ReadString(nodeClaimDetail, TAG_RCH_CODE, udtErrorList, ErrorCodeList.I00010, False, _RCHCode_Received)
        End Sub


#End Region

#Region "Read Voucher Info"

        Private Sub ReadVoucherInfo(ByVal nodeVoucherInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, ByRef udtWSVoucherModel As WSVoucherModel)

            'Voucher Claimed
            ReadVoucherClaimed(nodeVoucherInfo, udtErrorList, udtWSVoucherModel.VoucherClaimed, udtWSVoucherModel.VoucherClaimed_Received)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            ReadCoPaymentFee(nodeVoucherInfo, udtErrorList, udtWSVoucherModel.CoPaymentFee, udtWSVoucherModel.CoPaymentFee_Received)

            Dim nlReasonForVisit As XmlNodeList = nodeVoucherInfo.SelectNodes("./" + TAG_REASON_FOR_VISIT)

            For Each udtReasonForVisitModel As ReasonForVisitModel In udtWSVoucherModel.ReasonForVisitModelCollection
                'Prof Code 
                If udtErrorList.Count = 0 Then
                    ReadProfCode(nlReasonForVisit.Item(0), udtErrorList, udtReasonForVisitModel.ProfCode, udtReasonForVisitModel.ProfCode_Received)
                Else
                    Exit Sub
                End If
                'L1 Code
                If udtErrorList.Count = 0 Then
                    ReadL1Code(nlReasonForVisit.Item(0), udtErrorList, udtReasonForVisitModel.L1Code, udtReasonForVisitModel.L1Code_Received)
                Else
                    Exit Sub
                End If
                'L1 Desc Eng
                If udtErrorList.Count = 0 Then
                    ReadL1DescEng(nlReasonForVisit.Item(0), udtErrorList, udtReasonForVisitModel.L1DescEng, udtReasonForVisitModel.L1DescEng_Received)
                Else
                    Exit Sub
                End If
                'L2 Code
                If udtErrorList.Count = 0 Then
                    ReadL2Code(nlReasonForVisit.Item(0), udtErrorList, udtReasonForVisitModel.L2Code, udtReasonForVisitModel.L2Code_Received)
                Else
                    Exit Sub
                End If
                'L2 Desc Eng
                If udtErrorList.Count = 0 Then
                    ReadL2DescEng(nlReasonForVisit.Item(0), udtErrorList, udtReasonForVisitModel.L2DescEng, udtReasonForVisitModel.L2DescEng_Received)
                Else
                    Exit Sub
                End If
            Next

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        End Sub

        Private Sub ReadVoucherClaimed(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                        ByRef intVoucherClaimed As Integer, ByRef blnVoucherClaimed_Received As Boolean)
            intVoucherClaimed = ReadInteger(nodeClaimDetail, TAG_VOUCHER_CLAIMED, udtErrorList, ErrorCodeList.I00010, False, blnVoucherClaimed_Received)
        End Sub


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Private Sub ReadCoPaymentFee(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                        ByRef intCoPaymentFee As Integer, ByRef blnCoPaymentFee_Received As Boolean)
            intCoPaymentFee = ReadInteger(nodeClaimDetail, TAG_COPAYMENT_FEE, udtErrorList, ErrorCodeList.I00010, False, blnCoPaymentFee_Received)
        End Sub

        Private Sub ReadProfCode(ByVal nodeReasonForVisit As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                    ByRef strProfCode As String, ByRef blnProfCode_Received As Boolean)
            strProfCode = ReadString(nodeReasonForVisit, TAG_PROF_CODE, udtErrorList, ErrorCodeList.I00010, False, blnProfCode_Received)
        End Sub

        Private Sub ReadL1Code(ByVal nodeReasonForVisit As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                ByRef strL1Code As String, ByRef blnL1Code_Received As Boolean)
            strL1Code = ReadInteger(nodeReasonForVisit, TAG_L1_CODE, udtErrorList, ErrorCodeList.I00010, False, blnL1Code_Received)
        End Sub

        Private Sub ReadL1DescEng(ByVal nodeReasonForVisit As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                    ByRef strL1DescEng As String, ByRef blnL1DescEng_Received As Boolean)
            strL1DescEng = ReadString(nodeReasonForVisit, TAG_L1_DESC_ENG, udtErrorList, ErrorCodeList.I00010, False, blnL1DescEng_Received)
        End Sub

        Private Sub ReadL2Code(ByVal nodeReasonForVisit As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                ByRef strL2Code As String, ByRef blnL1DescEng_Received As Boolean)
            strL2Code = ReadInteger(nodeReasonForVisit, TAG_L2_CODE, udtErrorList, ErrorCodeList.I00010, False, blnL1DescEng_Received)
        End Sub

        Private Sub ReadL2DescEng(ByVal nodeReasonForVisit As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                    ByRef strL2DescEng As String, ByRef blnL2DescEng_Received As Boolean)
            strL2DescEng = ReadString(nodeReasonForVisit, TAG_L2_DESC_ENG, udtErrorList, ErrorCodeList.I00010, False, blnL2DescEng_Received)
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#End Region

#Region "Read Vaccine Info"
        Private Sub ReadVaccineInfo(ByVal nodeVoucherInfo As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, ByRef udtVaccineDetail As WSVaccineDetailModel)
            'Subsidy Code
            If udtErrorList.Count = 0 Then
                ReadSubsidyCode(nodeVoucherInfo, udtErrorList, udtVaccineDetail.SubsidyCode, udtVaccineDetail.SubsidyCode_Received)
            Else
                Exit Sub
            End If
            'Dose Seq
            If udtErrorList.Count = 0 Then
                ReadDoseSeq(nodeVoucherInfo, udtErrorList, udtVaccineDetail.DoseSeq, udtVaccineDetail.DoseSeq_Received)
            Else
                Exit Sub
            End If

        End Sub

        Private Sub ReadSubsidyCode(ByVal nodeVaccine As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                            ByRef strSubsidyCode As String, ByRef blnSubsidyCode_Received As Boolean)
            strSubsidyCode = ReadString(nodeVaccine, TAG_SUBSIDY_CODE, udtErrorList, ErrorCodeList.I00010, False, blnSubsidyCode_Received)
        End Sub

        Private Sub ReadDoseSeq(ByVal nodeVaccine As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                                    ByRef strDoseSeq As String, ByRef blnDoseSeq_Received As Boolean)
            strDoseSeq = ReadString(nodeVaccine, TAG_DOSE_SEQ, udtErrorList, ErrorCodeList.I00010, False, blnDoseSeq_Received)
        End Sub

#End Region

#Region "Read Indicator"

        Private Sub ReadPreSchoolInd(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
                    ByRef PreSchoolInd As String, ByRef _PreSchoolInd_Received As Boolean)
            PreSchoolInd = ReadString(nodeClaimDetail, TAG_PRE_SCHOOL_IND, udtErrorList, ErrorCodeList.I00010, False, _PreSchoolInd_Received)
        End Sub

        Private Sub ReadDoseIntervalInd(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
            ByRef DoseIntervalInd As String, ByRef _DoseIntervalInd_Received As Boolean)
            DoseIntervalInd = ReadString(nodeClaimDetail, TAG_DOSE_INTERVAL_IND, udtErrorList, ErrorCodeList.I00010, False, _DoseIntervalInd_Received)
        End Sub

        Private Sub ReadTSWInd(ByVal nodeClaimDetail As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection, _
            ByRef TSWInd As String, ByRef _TSWInd_Received As Boolean)
            TSWInd = ReadString(nodeClaimDetail, TAG_TSW_IND, udtErrorList, ErrorCodeList.I00010, False, _TSWInd_Received)
        End Sub

#End Region



#End Region

#Region "Public Function"

        Public Sub FillWarningIndicatorList(ByRef udtWarningIndicatorList As Hashtable)
            Dim iSeq = 0
            For Each udtWSClaimDetail As WSClaimDetailModel In Me.WSClaimDetailList
                'PreSchoolInd
                If udtWSClaimDetail.PreSchoolInd_Received Then
                    udtWarningIndicatorList.Add(iSeq.ToString() + "PreSchoolInd", udtWSClaimDetail.PreSchoolInd)
                End If
                'DoseIntervalInd
                If udtWSClaimDetail.DoseIntervalInd_Received Then
                    udtWarningIndicatorList.Add(iSeq.ToString() + "DoseIntervalInd", udtWSClaimDetail.DoseIntervalInd)
                End If
                'TSWInd
                If udtWSClaimDetail.TSWInd_Received Then
                    udtWarningIndicatorList.Add(iSeq.ToString() + "TSWInd", udtWSClaimDetail.TSWInd)
                End If
                iSeq = iSeq + 1
            Next
        End Sub

        Public Overridable Function FillEHSTransactionModel(ByRef udtEHSTransactions As EHSTransactionModelCollection, ByVal udtEHSAccount As EHSAccountModel, _
                                                            ByVal strSPID As String, ByVal intPracticeID As String, Optional ByRef udtWarningIndicatorList As Hashtable = Nothing, _
                                                            Optional ByRef _udtReturnErrorCodes As ExternalInterfaceWS.Component.ErrorInfo.ErrorInfoModelCollection = Nothing) As Boolean

            Dim udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL()
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()
            Dim udtPracticeBankAccBLL As PracticeBankAcctBLL = New PracticeBankAcctBLL()

            Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Nothing
            Dim udtEHSTransaction As EHSTransactionModel = Nothing
            Dim udtSchemeClaim As SchemeClaimModel = Nothing
            Dim udtPracticeDisplay As PracticeDisplayModel = Nothing
            Dim udtPracticeDisplays As PracticeDisplayModelCollection = Nothing
            Dim dtmServiceDate As Date
            Dim strSchemeCode As String
            Dim strServiceType As String = String.Empty
            Dim intVoucherClaimed As Integer
            Dim strRCHCode As String

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            Dim udtTransactionAdditionalFieldMList As New TransactionAdditionalFieldModelCollection()
            Dim udtTransactAdditionfield As New TransactionAdditionalFieldModel()


            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            If IsValid Then

                If _udtWSClaimDetaillList.Count > 0 Then

                    '-----------------------------
                    ' For Each Claim Info 
                    '-----------------------------



                    For i As Integer = 0 To _udtWSClaimDetaillList.Count - 1
                        '-----------------------------
                        'Voucher (HCVS)
                        '-----------------------------
                        If Not IsNothing(_udtWSClaimDetaillList.Item(i).WSVoucherList) AndAlso _udtWSClaimDetaillList.Item(i).WSVoucherList.Count > 0 Then

                            For j As Integer = 0 To _udtWSClaimDetaillList.Item(i).WSVoucherList.Count - 1
                                dtmServiceDate = _dtmServiceDate.Value
                                strSchemeCode = _udtWSClaimDetaillList.Item(i).SchemeCode
                                intVoucherClaimed = _udtWSClaimDetaillList.Item(i).WSVoucherList(j).VoucherClaimed

                                udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))

                                udtEHSAccount.SetSearchDocCode(udtEHSAccount.EHSPersonalInformationList(0).DocCode)
                                udtPracticeDisplays = udtPracticeBankAccBLL.getActivePractice(strSPID)

                                For Each udtPracticeDisplayTemp As PracticeDisplayModel In udtPracticeDisplays
                                    If udtPracticeDisplayTemp.PracticeID = intPracticeID Then
                                        udtPracticeDisplay = udtPracticeDisplayTemp
                                        Exit For
                                    End If
                                Next

                                udtEHSTransaction = udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, udtEHSAccount, udtPracticeDisplay)
                                udtEHSTransaction.EHSAcct = udtEHSAccount


                                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

                                ' -----------------------------------------------------------------------------------------

                                strServiceType = udtPracticeDisplay.ServiceCategoryCode


                                If _udtWSClaimDetaillList.Item(i).WSVoucherList(j).CoPaymentFee.HasValue Then
                                    ' Co-Payment Fee
                                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee
                                    udtTransactAdditionfield.AdditionalFieldValueCode = _udtWSClaimDetaillList.Item(i).WSVoucherList(j).CoPaymentFee
                                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                    'udtTransactAdditionfield.SchemeCode = Scheme.SchemeClaimModel.HCVS
                                    'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
                                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                                    udtTransactionAdditionalFieldMList.Add(udtTransactAdditionfield)
                                End If

                                For k As Integer = 0 To _udtWSClaimDetaillList.Item(i).WSVoucherList(j).ReasonForVisitModelCollection.Count - 1

                                    ' Reason For Visit Level1
                                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(k)
                                    udtTransactAdditionfield.AdditionalFieldValueCode = _udtWSClaimDetaillList.Item(i).WSVoucherList(j).ReasonForVisitModelCollection(k).L1Code
                                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                    'udtTransactAdditionfield.SchemeCode = Scheme.SchemeClaimModel.HCVS
                                    'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
                                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                                    udtTransactionAdditionalFieldMList.Add(udtTransactAdditionfield)

                                    ' Reason For Visit Level2
                                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(k)
                                    udtTransactAdditionfield.AdditionalFieldValueCode = _udtWSClaimDetaillList.Item(i).WSVoucherList(j).ReasonForVisitModelCollection(k).L2Code
                                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                    'udtTransactAdditionfield.SchemeCode = Scheme.SchemeClaimModel.HCVS
                                    'udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SchemeSeq
                                    udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                                    udtTransactAdditionfield.SchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                                    udtTransactAdditionfield.SubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode
                                    udtTransactionAdditionalFieldMList.Add(udtTransactAdditionfield)
                                Next


                                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                                'Insert Related Information to the claim
                                udtEHSTransaction.UpdateBy = strSPID
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                'udtEHSTransaction.SchemeCode = Scheme.SchemeClaimModel.HCVS
                                udtEHSTransaction.SchemeCode = strSchemeCode
                                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                                udtEHSTransaction.VoucherClaim = intVoucherClaimed
                                udtEHSTransaction.ServiceDate = dtmServiceDate
                                udtEHSTransaction.ServiceType = strServiceType

                                udtEHSAccount.AvailableVoucher = udtEHSTransactionBLL.getAvailableVoucher(udtSchemeClaim, udtEHSAccount.EHSPersonalInformationList(0))

                                udtEHSTransaction.VoucherBeforeRedeem = udtEHSAccount.AvailableVoucher
                                udtEHSTransaction.VoucherAfterRedeem = udtEHSAccount.AvailableVoucher.Value - udtEHSTransaction.VoucherClaim
                                udtEHSTransaction.ServiceProviderID = strSPID
                                udtEHSTransaction.PracticeID = intPracticeID


                                udtEHSTransaction.DataEntryBy = String.Empty
                                udtEHSTransaction.PrintedConsentForm = False
                                udtEHSTransaction.RecordStatus = "A"
                                udtEHSTransaction.CreateBy = strSPID
                                'udtEHSTransaction.TSWCase = IIf(TSW = "1", True, False)

                                'EHS Transaction Detail Model
                                udtEHSClaimBLL.ConstructEHSTransactionDetails(strSPID, Nothing, udtEHSTransaction, udtEHSAccount)

                                udtEHSTransactions.Add(udtEHSTransaction)
                            Next

                            If udtTransactionAdditionalFieldMList.Count > 0 Then
                                udtEHSTransaction.TransactionAdditionFields = udtTransactionAdditionalFieldMList
                            End If

                        End If

                        '-----------------------------
                        'Vaccine 
                        '-----------------------------
                        If Not IsNothing(_udtWSClaimDetaillList.Item(i).WSVaccineDetailList) AndAlso _udtWSClaimDetaillList.Item(i).WSVaccineDetailList.Count > 0 Then

                            '-----------------------------
                            'For Each Vaccination 
                            '-----------------------------


                            dtmServiceDate = _dtmServiceDate.Value
                            strSchemeCode = _udtWSClaimDetaillList.Item(i).SchemeCode
                            strRCHCode = _udtWSClaimDetaillList.Item(i).RCHCode

                            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                            ' -----------------------------------------------------------------------------------------
                            'Scheme Claim Model
                            'udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
                            udtSchemeClaim = udtSchemeClaimBLL.getLatestValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
                            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                            If udtSchemeClaim Is Nothing Then
                                _udtReturnErrorCodes.Add(ErrorCodeList.I00108)
                                Exit For
                            End If

                            Dim blnSubsidyFound = False
                            For j As Integer = 0 To _udtWSClaimDetaillList.Item(i).WSVaccineDetailList.Count - 1
                                blnSubsidyFound = False
                                For Each udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                                    ' -----------------------------------------------------------------------------------------
                                    ' Use new Display Code for Claim as compare value
                                    If UploadClaimBLL.UseParticularSubsidyCode Then
                                        If udtSubsidizeGroupClaim.DisplayCodeForClaim = _udtWSClaimDetaillList.Item(i).WSVaccineDetailList(j).SubsidyCode Then
                                            blnSubsidyFound = True
                                        End If
                                    Else
                                        If udtSubsidizeGroupClaim.SubsidizeDisplayCode = _udtWSClaimDetaillList.Item(i).WSVaccineDetailList(j).SubsidyCode Then
                                            blnSubsidyFound = True
                                        End If
                                    End If
                                    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
                                Next
                                If blnSubsidyFound = False Then
                                    Exit For
                                End If
                            Next

                            If blnSubsidyFound = False Then
                                _udtReturnErrorCodes.Add(ErrorCodeList.I00108)
                                Exit For
                            End If

                            udtEHSAccount.SetSearchDocCode(udtEHSAccount.EHSPersonalInformationList(0).DocCode)
                            udtPracticeDisplays = udtPracticeBankAccBLL.getActivePractice(strSPID)

                            For Each udtPracticeDisplayTemp As PracticeDisplayModel In udtPracticeDisplays
                                If udtPracticeDisplayTemp.PracticeID = intPracticeID Then
                                    udtPracticeDisplay = udtPracticeDisplayTemp
                                    Exit For
                                End If
                            Next

                            'EHS Transaction Model
                            udtEHSTransaction = udtEHSClaimBLL.ConstructNewEHSTransaction(udtSchemeClaim, udtEHSAccount, udtPracticeDisplay)
                            udtEHSTransaction.EHSAcct = udtEHSAccount

                            'Insert Related Information to the claim
                            udtEHSTransaction.UpdateBy = strSPID
                            udtEHSTransaction.ServiceDate = dtmServiceDate

                            'EHS Claim Vaccine Model
                            'udtEHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
                            udtEHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel2(udtSchemeClaim, udtEHSAccount.EHSPersonalInformationList(0).DocCode, udtEHSAccount, udtEHSTransaction.ServiceDate, True, String.Empty)

                            'Loop through the XML vaccine models
                            ' The subsidy code 
                            For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                                For j As Integer = 0 To _udtWSClaimDetaillList.Item(i).WSVaccineDetailList.Count - 1

                                    If _udtWSClaimDetaillList.Item(i).WSVaccineDetailList(j).SubsidyCode = udtSubsidize.SubsidizeDisplayCode Then
                                        ' Set selected subsidy
                                        udtSubsidize.Selected = True

                                        ' Get Category Code
                                        Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL()
                                        Dim strCategoryCode As String = String.Empty
                                        Dim strCategoryName As String = String.Empty
                                        Dim udtClaimCategoryModelCollection As ClaimCategory.ClaimCategoryModelCollection = udtClaimCategoryBLL.getAllCategoryCache()

                                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                                        ' -----------------------------------------------------------------------------------------
                                        udtClaimCategoryModelCollection = udtClaimCategoryModelCollection.Filter(udtSchemeClaim.SchemeCode, udtSubsidize.SchemeSeq, udtSubsidize.SubsidizeCode)
                                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                                        If Not IsNothing(udtClaimCategoryModelCollection) AndAlso udtClaimCategoryModelCollection.Count > 0 Then
                                            strCategoryCode = udtClaimCategoryModelCollection.Item(0).CategoryCode
                                            strCategoryName = udtClaimCategoryModelCollection.Item(0).CategoryName
                                        End If


                                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                                        ' -----------------------------------------------------------------------------------------
                                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                                        ' -----------------------------------------------------------------------------------------
                                        ' Set Category Code
                                        If Not strCategoryCode = String.Empty Then
                                            udtTransactAdditionfield = New TransactionAdditionalFieldModel
                                            udtTransactAdditionfield.AdditionalFieldID = "CategoryCode"
                                            udtTransactAdditionfield.AdditionalFieldValueCode = strCategoryCode
                                            udtTransactAdditionfield.AdditionalFieldValueDesc = strCategoryName
                                            udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                                            udtTransactAdditionfield.SchemeSeq = udtSubsidize.SchemeSeq
                                            udtTransactAdditionfield.SubsidizeCode = udtSubsidize.SubsidizeCode
                                            udtTransactionAdditionalFieldMList.Add(udtTransactAdditionfield)
                                        End If

                                        ' Set RCHCode to additional field
                                        If Not strRCHCode = String.Empty Then
                                            udtTransactAdditionfield = New TransactionAdditionalFieldModel
                                            udtTransactAdditionfield.AdditionalFieldID = "RHCCode"
                                            udtTransactAdditionfield.AdditionalFieldValueCode = strRCHCode
                                            udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                                            udtTransactAdditionfield.SchemeCode = udtSchemeClaim.SchemeCode
                                            udtTransactAdditionfield.SchemeSeq = udtSubsidize.SchemeSeq
                                            udtTransactAdditionfield.SubsidizeCode = udtSubsidize.SubsidizeCode
                                            udtTransactionAdditionalFieldMList.Add(udtTransactAdditionfield)
                                        End If

                                        If udtTransactionAdditionalFieldMList.Count > 0 Then
                                            udtEHSTransaction.TransactionAdditionFields = udtTransactionAdditionalFieldMList
                                        End If
                                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
                                        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

                                        ' Set selected dose
                                        For Each udtSubsidizeItemDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                                            'TODO: temp hardcode the seq mapping...
                                            Dim strDoseSeq As String = ""
                                            Select Case _udtWSClaimDetaillList.Item(i).WSVaccineDetailList(j).DoseSeq
                                                Case "1ST"
                                                    strDoseSeq = "1STDOSE"
                                                Case "2ND"
                                                    strDoseSeq = "2NDDOSE"
                                                Case "NA"
                                                    strDoseSeq = "ONLYDOSE"
                                            End Select

                                            If udtSubsidizeItemDetail.AvailableItemCode.Trim() = strDoseSeq Then
                                                udtSubsidizeItemDetail.Selected = True
                                            End If
                                        Next
                                    End If
                                Next

                            Next

                            'EHS Transaction Detail Model
                            udtEHSClaimBLL.ConstructEHSTransactionDetails(strSPID, Nothing, udtEHSTransaction, udtEHSAccount, udtEHSClaimVaccine)
                            udtEHSTransactions.Add(udtEHSTransaction)
                        End If
                    Next
                End If

            End If

            Return True

        End Function

#End Region


#Region "Create Model"


        'udtTransaction = New EHSTransactionModel()
        'udtTransaction.ServiceDate = _dtmServiceDate.Value
        'udtTransaction.SchemeCode = _udtWSClaimDetaillList.Item(i).SchemeCode
        'udtTransaction.ServiceType = _udtWSClaimDetaillList.Item(i).WSVoucher.ProfCode

        '' EHS Transaction ---------------------------------

        '''' <summary>
        '''' Construct EHS Transaction Model
        '''' </summary>
        '''' <param name="udtSchemeClaimModel"></param>
        '''' <param name="udtEHSAccount"></param>
        '''' <param name="udtPracticeDisplayModel"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function ConstructNewEHSTransaction(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtPracticeDisplayModel As PracticeDisplayModel) As EHSTransactionModel
        '    Dim udtEHSTran As New EHSTransactionModel()

        '    If Not udtEHSAccount.IsNew() Then
        '        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
        '            udtEHSTran.VoucherAccID = udtEHSAccount.VoucherAccID
        '        Else
        '            udtEHSTran.TempVoucherAccID = udtEHSAccount.VoucherAccID
        '        End If
        '    End If
        '    udtEHSTran.SchemeCode = udtSchemeClaimModel.SchemeCode
        '    udtEHSTran.ServiceDate = DateTime.Now
        '    udtEHSTran.ServiceType = udtPracticeDisplayModel.ServiceCategoryCode
        '    udtEHSTran.ServiceProviderID = udtPracticeDisplayModel.SPID
        '    udtEHSTran.PracticeID = udtPracticeDisplayModel.PracticeID
        '    udtEHSTran.BankAccountID = udtPracticeDisplayModel.BankAcctID
        '    udtEHSTran.BankAccountNo = udtPracticeDisplayModel.BankAccountNo
        '    udtEHSTran.BankAccountOwner = udtPracticeDisplayModel.BankAccHolder
        '    udtEHSTran.ClaimAmount = 0

        '    ' Set External Reference Status (e.g. Vaccination record from CMS)
        '    Dim udtExtRefStatus As EHSTransactionModel.ExtRefStatusClass = (New SessionHandler).ExtRefStatusGetFromSession()
        '    If udtExtRefStatus Is Nothing Then udtExtRefStatus = New EHSTransactionModel.ExtRefStatusClass
        '    udtEHSTran.ExtRefStatus = udtExtRefStatus.Code



        '    If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then

        '        Dim dtmCurrentDate As Date = Me._udtCommonGenFunc.GetSystemDateTime()

        '        udtEHSTran.PerVoucherValue = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeValue
        '        udtEHSTran.VoucherBeforeRedeem = Me._udtEHSTransactionBLL.getAvailableVoucher(dtmCurrentDate, udtEHSAccount.SearchDocCode, _
        '            udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).IdentityNum, _
        '            udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).DOB, _
        '            udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode).ExactDOB, _
        '            udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode)
        '    End If

        '    Return udtEHSTran

        'End Function

        '' Transaction Detail ------------------------------

        '''' <summary>
        '''' Construct the EHS Transaction Detail Model for Vaccination
        '''' use servicedate to retrieve the active scheme and active subsidize
        '''' </summary>
        '''' <param name="udtSP"></param>
        '''' <param name="udtDataEntry"></param>
        '''' <param name="udtEHSTransactionModel"></param>
        '''' <param name="udtEHSAccount"></param>
        '''' <param name="udtEHSClaimVaccineModel"></param>
        '''' <remarks></remarks>
        'Public Sub ConstructEHSTransactionDetails(ByVal udtSP As ServiceProviderModel, _
        '    ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByRef udtEHSClaimVaccineModel As EHSClaimVaccineModel)

        '    Dim dblClaimAmount As Double = 0

        '    ' VoucherTransaction
        '    If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
        '        udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Active
        '        udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
        '    Else
        '        udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
        '        udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
        '    End If

        '    udtEHSTransactionModel.CreateBy = udtSP.SPID
        '    udtEHSTransactionModel.UpdateBy = udtSP.SPID
        '    udtEHSTransactionModel.DataEntryBy = String.Empty


        '    udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
        '    udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

        '    ' TransactionDetail
        '    For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccineModel.SubsidizeList

        '        If udtSubsidize.Selected Then

        '            ' ------------------------------------------------------------------------
        '            ' Construct the Detail usign the Active Scheme & Subsidize By Service date 
        '            ' ------------------------------------------------------------------------
        '            ' RVP->PV 2009Oct19 , RVP->HSIV 2009Dec28 08:00 
        '            ' Service Date: 2009Dec08, use 2009Dec08 23:59 to search
        '            Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSClaimVaccineModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))

        '            If udtSubsidize.SubsidizeDetailList.Count = 1 Then

        '                Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
        '                udtEHSTransactionDetail.SchemeCode = udtEHSClaimVaccineModel.SchemeCode
        '                udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq
        '                'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
        '                udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
        '                udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
        '                udtEHSTransactionDetail.AvailableItemCode = udtSubsidize.SubsidizeDetailList(0).AvailableItemCode
        '                udtEHSTransactionDetail.Unit = 1
        '                udtEHSTransactionDetail.PerUnitValue = udtSubsidize.Amount
        '                udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
        '                udtEHSTransactionDetail.Remark = ""
        '                udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

        '                dblClaimAmount = dblClaimAmount + udtEHSTransactionDetail.TotalAmount.Value

        '            ElseIf udtSubsidize.SubsidizeDetailList.Count > 1 Then
        '                For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
        '                    If udtSubsidizeDetail.Selected Then
        '                        Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()
        '                        udtEHSTransactionDetail.SchemeCode = udtEHSClaimVaccineModel.SchemeCode
        '                        udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtEHSClaimVaccineModel.SchemeCode, udtSubsidize.SubsidizeCode).SchemeSeq
        '                        'udtEHSTransactionDetail.SchemeSeq = udtEHSClaimVaccineModel.SchemeSeq
        '                        udtEHSTransactionDetail.SubsidizeCode = udtSubsidize.SubsidizeCode
        '                        udtEHSTransactionDetail.SubsidizeItemCode = udtSubsidize.SubsidizeItemCode
        '                        udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeDetail.AvailableItemCode
        '                        udtEHSTransactionDetail.Unit = 1
        '                        udtEHSTransactionDetail.PerUnitValue = udtSubsidize.Amount
        '                        udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
        '                        udtEHSTransactionDetail.Remark = ""
        '                        udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

        '                        dblClaimAmount = dblClaimAmount + udtEHSTransactionDetail.TotalAmount.Value
        '                    End If
        '                Next
        '            End If
        '        End If
        '    Next

        '    udtEHSTransactionModel.ClaimAmount = dblClaimAmount
        'End Sub

        '''' <summary>
        '''' Construct EHS Transaction Detail For Voucher
        '''' </summary>
        '''' <param name="udtSP"></param>
        '''' <param name="udtDataEntry"></param>
        '''' <param name="udtEHSTransactionModel"></param>
        '''' <param name="udtEHSAccount"></param>
        '''' <remarks></remarks>
        'Public Sub ConstructEHSTransactionDetails(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel, _
        '    ByRef udtEHSTransactionModel As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel)

        '    ' VoucherTransaction
        '    If udtDataEntry Is Nothing Then
        '        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
        '            udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Active
        '            udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
        '        Else
        '            udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
        '            udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
        '        End If

        '        udtEHSTransactionModel.CreateBy = udtSP.SPID
        '        udtEHSTransactionModel.UpdateBy = udtSP.SPID
        '        udtEHSTransactionModel.DataEntryBy = String.Empty
        '    Else

        '        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
        '            udtEHSTransactionModel.VoucherAccID = udtEHSAccount.VoucherAccID
        '        Else
        '            udtEHSTransactionModel.TempVoucherAccID = udtEHSAccount.VoucherAccID
        '        End If
        '        udtEHSTransactionModel.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Pending
        '        udtEHSTransactionModel.CreateBy = udtSP.SPID
        '        udtEHSTransactionModel.UpdateBy = udtSP.SPID
        '        udtEHSTransactionModel.DataEntryBy = udtDataEntry.DataEntryAccount
        '    End If

        '    udtEHSTransactionModel.DocCode = udtEHSAccount.SearchDocCode
        '    udtEHSTransactionModel.TransactionDetails = New TransactionDetailModelCollection()

        '    Dim udtEHSTransactionDetail As New EHSTransaction.TransactionDetailModel()

        '    ' ------------------------------------------------------------------------
        '    ' Construct the Detail usign the Active Scheme & Subsidize By Service date 
        '    ' ------------------------------------------------------------------------

        '    Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))

        '    'Dim udtSchemeClaimModel As SchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate)
        '    'If udtSchemeClaimModel Is Nothing Then
        '    '    udtSchemeClaimModel = Me._udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtEHSTransactionModel.SchemeCode, udtEHSTransactionModel.ServiceDate.AddDays(1).AddMinutes(-1))
        '    'End If

        '    udtEHSTransactionDetail.SchemeCode = udtSchemeClaimModel.SchemeCode
        '    udtEHSTransactionDetail.SchemeSeq = udtSchemeClaimModel.SchemeSeq
        '    udtEHSTransactionDetail.SubsidizeCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode

        '    udtEHSTransactionDetail.SubsidizeItemCode = udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode

        '    Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeItemCode)
        '    udtEHSTransactionDetail.AvailableItemCode = udtSubsidizeItemDetailList(0).AvailableItemCode
        '    udtEHSTransactionDetail.Unit = udtEHSTransactionModel.VoucherClaim
        '    udtEHSTransactionDetail.PerUnitValue = udtEHSTransactionModel.PerVoucherValue
        '    udtEHSTransactionDetail.TotalAmount = udtEHSTransactionDetail.Unit.Value * udtEHSTransactionDetail.PerUnitValue.Value
        '    udtEHSTransactionDetail.Remark = ""
        '    udtEHSTransactionModel.TransactionDetails.Add(udtEHSTransactionDetail)

        '    udtEHSTransactionModel.VoucherAfterRedeem = udtEHSTransactionModel.VoucherBeforeRedeem - udtEHSTransactionModel.VoucherClaim
        '    udtEHSTransactionModel.ClaimAmount = udtEHSTransactionDetail.TotalAmount


        'End Sub
        'udtTransaction = New EHSTransactionModel()
        'udtTransaction.ServiceDate = _dtmServiceDate.Value
        'udtTransaction.SchemeCode = _udtWSClaimDetaillList.Item(i).SchemeCode
        'udtTransaction.ServiceType = _udtWSClaimDetaillList.Item(i).WSVoucher.ProfCode

        '' ------------------------------------------------------------------
        '' HCVS: Reason for Visit (level 1 + Level 2)
        '' ------------------------------------------------------------------
        'Dim strFieldIDL1 As String = "Reason_for_Visit_L1"
        'Dim strFieldIDL2 As String = "Reason_for_Visit_L2"

        'udtTransaction.TransactionAdditionFields.Add(New TransactionAdditionalFieldModel(udtTransaction.TransactionID, _
        '                                                                                udtTransaction.SchemeCode, _
        '                                                                                2, _
        '                                                                                "EHCVS", _
        '                                                                                strFieldIDL1, _
        '                                                                                _udtWSClaimDetaillList.Item(i).WSVoucher.L1Code, _
        '                                                                                string.Empty)

        'udtTransaction.TransactionAdditionFields.Add(New TransactionAdditionalFieldModel(udtTransaction.TransactionID, _
        '                                                                                udtTransaction.SchemeCode, _
        '                                                                                2, _
        '                                                                                "EHCVS", _
        '                                                                                strFieldIDL2, _
        '                                                                                _udtWSClaimDetaillList.Item(i).WSVoucher.L2Code, _
        '                                                                                string.Empty)
        '' ------------------------------------------------------------------
        '' Transaction Detail
        '' ------------------------------------------------------------------
        'udtTransaction.TransactionDetails.Add(new TransactionDetailModel(udtTransaction.TransactionID, _
        '                                                                udtTransaction.SchemeCode, _
        '                                                                2, _
        '                                                                udtTransaction.SchemeCode, _
        '                                                                "EHCVS", _ 
        '                                                                udtTransaction.

#End Region


    End Class

End Namespace

