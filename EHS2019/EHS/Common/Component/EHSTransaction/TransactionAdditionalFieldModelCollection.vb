Namespace Component.EHSTransaction
    <Serializable()> Public Class TransactionAdditionalFieldModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtTransactionAdditionalFieldModel As TransactionAdditionalFieldModel)
            MyBase.Add(udtTransactionAdditionalFieldModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtTransactionAdditionalFieldModel As TransactionAdditionalFieldModel)
            MyBase.Remove(udtTransactionAdditionalFieldModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As TransactionAdditionalFieldModel
            Get
                Return CType(MyBase.Item(intIndex), TransactionAdditionalFieldModel)
            End Get
        End Property

        Public Function Filter(ByVal strSubsidizeCode As String) As TransactionAdditionalFieldModelCollection
            Dim udtResTransactionAdditionalFieldList As New TransactionAdditionalFieldModelCollection()
            For Each udtTransactionAdditionalFieldModel As TransactionAdditionalFieldModel In Me

                If udtTransactionAdditionalFieldModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResTransactionAdditionalFieldList.Add(New TransactionAdditionalFieldModel(udtTransactionAdditionalFieldModel))
                End If
            Next
            Return udtResTransactionAdditionalFieldList
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As TransactionAdditionalFieldModelCollection

            Dim udtResTransactionAdditionalFieldList As New TransactionAdditionalFieldModelCollection()
            For Each udtTransactionAdditionalFieldModel As TransactionAdditionalFieldModel In Me

                If udtTransactionAdditionalFieldModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtTransactionAdditionalFieldModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtTransactionAdditionalFieldModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then

                    udtResTransactionAdditionalFieldList.Add(New TransactionAdditionalFieldModel(udtTransactionAdditionalFieldModel))
                End If
            Next
            Return udtResTransactionAdditionalFieldList
        End Function

        Public Function FilterByAdditionFieldID(ByVal strAdditionFieldID As String) As TransactionAdditionalFieldModel
            For Each udtTransactionAdditionalFieldModel As TransactionAdditionalFieldModel In Me

                If udtTransactionAdditionalFieldModel.AdditionalFieldID.Trim().ToUpper().Equals(strAdditionFieldID.Trim().ToUpper()) Then
                    Return udtTransactionAdditionalFieldModel
                End If
            Next
            Return Nothing
        End Function

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Public ReadOnly Property CoPaymentFee() As Nullable(Of Integer)
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Public ReadOnly Property CoPaymentFeeRMB() As Nullable(Of Decimal)
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFeeRMB)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property

        Public ReadOnly Property PaymentType() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PaymentType)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        'CRE13-019-02 Extend HCVS to China [End][Karl]

        ' CRE15-005 PIDVSS [Start][Winnie]
        Public ReadOnly Property DocumentaryProof() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE15-005 PIDVSS [End][Winnie]

        Public ReadOnly Property PIDInstitutionCode() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PIDInstitutionCode)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public ReadOnly Property SchoolCode() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SchoolCode)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Public ReadOnly Property PlaceVaccination() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property

        Public ReadOnly Property PlaceVaccinationText() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PlaceVaccination)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueDesc
            End Get
        End Property

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property ClaimedPaymentType() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClaimedPaymentType)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property RegistrationFeeRMB() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RegistrationFeeRMB)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property SubSpecialities() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SubSpecialities)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property ConsultAndRegFeeRMB() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ConsultAndRegFeeRMB)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property DrugFeeRMB() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DrugFeeRMB)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property InvestigationFeeRMB() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.InvestigationFeeRMB)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property OtherFeeRMB() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.OtherFeeRMB)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property OtherFeeRMBRemark() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.OtherFeeRMB)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueDesc
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property SubsidyBeforeClaim() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SubsidyBeforeClaim)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property SubsidyAfterClaim() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SubsidyAfterClaim)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property TotalSupportFee() As Decimal
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.TotalSupportFee)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        ' CRE20-015-05 (Special Support Scheme) [Start][Winnie]
        Public ReadOnly Property PaymentTypeMatch() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.PaymentTypeMatch)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-015-05 (Special Support Scheme) [End][Winnie]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property Booth() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.Booth)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property MainCategory() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.MainCategory)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property SubCategory() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.SubCategory)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property VaccineBrand() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.VaccineBrand)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Winnie SUEN]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property VaccineLotNo() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.VaccineLotNo)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property ContactNo() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ContactNo)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property Mobile() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.Mobile)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property Remarks() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.Remarks)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueDesc
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property JoinEHRSS() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.JoinEHRSS)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property RCHCode() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RCHCode)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property OutreachCode() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.OutreachCode)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property RecipientType() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.RecipientType)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property OutreachType() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.OutreachType)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property DischargeResult() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DischargeResult)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Martin Tang]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property ClinicType() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property

        ' CRE20-0023 (Immu record) [End][Martin Tang]

        ' CRE20-015-06 (Special Support Scheme) [Start][Winnie]
        Public ReadOnly Property ExemptRegFee() As Boolean
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ExemptRegFee)
                If udtAdditionalField IsNot Nothing Then
                    If udtAdditionalField.AdditionalFieldValueCode = YesNo.Yes Then
                        Return True
                    Else
                        Return False
                    End If
                End If

                Return False
            End Get
        End Property

        Public ReadOnly Property RegFeeChargedDate() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ExemptRegFee)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueDesc
            End Get
        End Property
        ' CRE20-015-06 (Special Support Scheme) [End][Winnie]

        'CRE20-006 DHC integration [Start][Nichole]
        Public ReadOnly Property DHC_DistrictCode() As String
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.DHCDistrictCode)
                If udtAdditionalField Is Nothing Then Return Nothing
                Return udtAdditionalField.AdditionalFieldValueCode
            End Get
        End Property
        'CRE20-006 DHC integration [End][Nichole]

        Public ReadOnly Property HasReasonForVisit() As Boolean
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel
                For i As Integer = 0 To TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length - 1
                    udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i))

                    If udtAdditionalField IsNot Nothing Then Return True
                Next

                Return False
            End Get
        End Property

        Public ReadOnly Property HasReasonForVisit(ByVal index As Integer) As Boolean
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel
                udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(index))
                If udtAdditionalField IsNot Nothing Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property HasReasonForVisitPrincipal() As Boolean
            Get
                Return HasReasonForVisit(0)
            End Get
        End Property

        Public ReadOnly Property HasReasonForVisitSecondary() As Boolean
            Get
                Return HasReasonForVisit(1) Or HasReasonForVisit(2) Or HasReasonForVisit(3)
            End Get
        End Property

        Public ReadOnly Property ReasonForVisitCount() As Integer
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel
                Dim iCount As Integer = 0
                For i As Integer = 0 To TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length - 1
                    udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i))

                    If udtAdditionalField IsNot Nothing Then iCount += 1
                Next

                'For i As Integer = 0 To TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length - 1
                '    udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(i))

                '    If udtAdditionalField IsNot Nothing Then iCount += 1
                'Next
                Return iCount
            End Get
        End Property

        Public Property ReasonForVisitL1(ByVal index As Integer) As TransactionAdditionalFieldModel
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing
                If TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(index) IsNot Nothing Then
                    udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(index))
                End If

                Return udtAdditionalField
            End Get
            Set(ByVal value As TransactionAdditionalFieldModel)
                If value Is Nothing Then
                    Throw New NullReferenceException("[TransactionAdditionalFieldModelCollection.ReasonForVisitL1] Value cannot be nothing ")
                End If
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing
                If TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(index) IsNot Nothing Then
                    udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(index))
                End If
                If udtAdditionalField Is Nothing Then
                    ' If Not Exist
                    value.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(index)
                    Me.Add(value)
                Else
                    ' If Exist
                    value.AdditionalFieldID = udtAdditionalField.AdditionalFieldID
                    Me.Remove(udtAdditionalField)
                    Me.Add(value)
                End If

            End Set
        End Property

        Public Property ReasonForVisitL2(ByVal index As Integer) As TransactionAdditionalFieldModel
            Get
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing
                If TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(index) IsNot Nothing Then
                    udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(index))
                End If

                Return udtAdditionalField
            End Get
            Set(ByVal value As TransactionAdditionalFieldModel)
                If value Is Nothing Then
                    Throw New NullReferenceException("[TransactionAdditionalFieldModelCollection.ReasonForVisitL2] Value cannot be nothing ")
                End If
                Dim udtAdditionalField As TransactionAdditionalFieldModel = Nothing
                If TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(index) IsNot Nothing Then
                    udtAdditionalField = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(index))
                End If
                If udtAdditionalField Is Nothing Then
                    ' If Not Exist
                    value.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(index)
                    Me.Add(value)
                Else
                    ' If Exist
                    value.AdditionalFieldID = udtAdditionalField.AdditionalFieldID
                    Me.Remove(udtAdditionalField)
                    Me.Add(value)
                End If

            End Set
        End Property

        Public Sub RemoveCoPaymentFee()
            Dim udtAdditionalField As TransactionAdditionalFieldModel = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee)
            If udtAdditionalField IsNot Nothing Then
                MyBase.Remove(udtAdditionalField)
            End If
        End Sub

        Public Sub RemoveReasonForVisit(ByVal index As Integer)
            Dim udtAdditionalField As TransactionAdditionalFieldModel
            udtAdditionalField = ReasonForVisitL1(index)
            If udtAdditionalField IsNot Nothing Then Me.Remove(udtAdditionalField)

            udtAdditionalField = ReasonForVisitL2(index)
            If udtAdditionalField IsNot Nothing Then Me.Remove(udtAdditionalField)

            ShiftReasonForVisitUpwards()

        End Sub

        Public Sub ShiftReasonForVisitUpwards()

            Dim iCount As Integer = 0

            Dim strCurrentL1AdditionalFieldID As String = String.Empty
            Dim strNextL1AdditionalFieldID As String = String.Empty

            Dim strCurrentL2AdditionalFieldID As String = String.Empty
            Dim strNextL2AdditionalFieldID As String = String.Empty

            For i As Integer = 0 To TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length - 1 - 1

                strCurrentL1AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i)
                strNextL1AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i + 1)

                strCurrentL2AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(i)
                strNextL2AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(i + 1)

                If Me.FilterByAdditionFieldID(strCurrentL1AdditionalFieldID) Is Nothing And Me.FilterByAdditionFieldID(strNextL1AdditionalFieldID) IsNot Nothing Then
                    Me.FilterByAdditionFieldID(strNextL1AdditionalFieldID).AdditionalFieldID = strCurrentL1AdditionalFieldID                    
                End If

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If Me.FilterByAdditionFieldID(strCurrentL2AdditionalFieldID) Is Nothing And Me.FilterByAdditionFieldID(strNextL2AdditionalFieldID) IsNot Nothing Then
                    Me.FilterByAdditionFieldID(strNextL2AdditionalFieldID).AdditionalFieldID = strCurrentL2AdditionalFieldID
                End If
                ' CRE19-006 (DHC) [End][Winnie]
            Next
        End Sub

        Public Sub SortReasonForVisit()
            Dim count As Integer = ReasonForVisitCount

            For i As Integer = 1 To count - 2
                For j As Integer = i + 1 To count - 1
                    If Not ReasonForVisitInAscendingOrder(i, j) Then
                        SwapReasonForVisit(i, j)
                    End If
                Next
            Next
        End Sub

        Public Function ReasonForVisitInAscendingOrder(ByVal i As Integer, ByVal j As Integer) As Boolean
            Dim InAscendingOrder As Boolean = False
            Dim udtAdditionalFieldL1i As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL1j As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL2i As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL2j As TransactionAdditionalFieldModel

            udtAdditionalFieldL1i = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i))
            udtAdditionalFieldL1j = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(j))
            udtAdditionalFieldL2i = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(i))
            udtAdditionalFieldL2j = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(j))

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtAdditionalFieldL1i IsNot Nothing AndAlso udtAdditionalFieldL1j IsNot Nothing Then

                If udtAdditionalFieldL2i IsNot Nothing AndAlso udtAdditionalFieldL2j IsNot Nothing Then
                    If udtAdditionalFieldL1i.AdditionalFieldValueCode + udtAdditionalFieldL2i.AdditionalFieldValueCode <= udtAdditionalFieldL1j.AdditionalFieldValueCode + udtAdditionalFieldL2j.AdditionalFieldValueCode Then
                        InAscendingOrder = True
                    End If

                Else
                    If udtAdditionalFieldL1i.AdditionalFieldValueCode <= udtAdditionalFieldL1j.AdditionalFieldValueCode Then
                        InAscendingOrder = True
                    End If
                End If

            End If
            ' CRE19-006 (DHC) [End][Winnie]


            Return InAscendingOrder
        End Function

        Public Sub SwapReasonForVisit(ByVal i As Integer, ByVal j As Integer)
            Dim intAdditionalFieldValueCodeL1 As String
            Dim intAdditionalFieldValueCodeL2 As String

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim udtAdditionalFieldL1i As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL1j As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL2i As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL2j As TransactionAdditionalFieldModel

            udtAdditionalFieldL1i = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i))
            udtAdditionalFieldL1j = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(j))
            udtAdditionalFieldL2i = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(i))
            udtAdditionalFieldL2j = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(j))


            If udtAdditionalFieldL1i IsNot Nothing AndAlso udtAdditionalFieldL1j IsNot Nothing Then

                Dim intAdditionalFieldValueDescL1 As String

                intAdditionalFieldValueCodeL1 = udtAdditionalFieldL1i.AdditionalFieldValueCode
                intAdditionalFieldValueDescL1 = udtAdditionalFieldL1i.AdditionalFieldValueDesc

                udtAdditionalFieldL1i.AdditionalFieldValueCode = udtAdditionalFieldL1j.AdditionalFieldValueCode
                udtAdditionalFieldL1j.AdditionalFieldValueCode = intAdditionalFieldValueCodeL1

                udtAdditionalFieldL1i.AdditionalFieldValueDesc = udtAdditionalFieldL1j.AdditionalFieldValueDesc
                udtAdditionalFieldL1j.AdditionalFieldValueDesc = intAdditionalFieldValueDescL1
            End If

            If udtAdditionalFieldL2i IsNot Nothing AndAlso udtAdditionalFieldL2j IsNot Nothing Then

                Dim intAdditionalFieldValueDescL2 As String

                intAdditionalFieldValueCodeL2 = udtAdditionalFieldL2i.AdditionalFieldValueCode
                intAdditionalFieldValueDescL2 = udtAdditionalFieldL2i.AdditionalFieldValueDesc

                udtAdditionalFieldL2i.AdditionalFieldValueCode = udtAdditionalFieldL2j.AdditionalFieldValueCode
                udtAdditionalFieldL2j.AdditionalFieldValueCode = intAdditionalFieldValueCodeL2

                udtAdditionalFieldL2i.AdditionalFieldValueDesc = udtAdditionalFieldL2j.AdditionalFieldValueDesc
                udtAdditionalFieldL2j.AdditionalFieldValueDesc = intAdditionalFieldValueDescL2
            End If
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        Public Function ReasonForVisitDuplicated() As Integer
            Dim IsDuplicated As Integer = -1
            Dim IsContinue As Boolean = True
            Dim udtAdditionalFieldL1i As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL1j As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL2i As TransactionAdditionalFieldModel
            Dim udtAdditionalFieldL2j As TransactionAdditionalFieldModel

            For i As Integer = 0 To TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length - 1
                For j As Integer = 0 To TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length - 1
                    udtAdditionalFieldL1i = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(i))
                    udtAdditionalFieldL1j = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(j))
                    udtAdditionalFieldL2i = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(i))
                    udtAdditionalFieldL2j = Me.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(j))
                    If IsContinue Then

                        ' CRE19-006 (DHC) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        'If Not (udtAdditionalFieldL1i Is Nothing Or udtAdditionalFieldL1j Is Nothing Or udtAdditionalFieldL2i Is Nothing Or udtAdditionalFieldL2j Is Nothing) Then
                        '    If Not (udtAdditionalFieldL1i.AdditionalFieldID = udtAdditionalFieldL1j.AdditionalFieldID) Then
                        '        If (udtAdditionalFieldL1i.AdditionalFieldValueCode = udtAdditionalFieldL1j.AdditionalFieldValueCode) Then
                        '            If (udtAdditionalFieldL2i.AdditionalFieldValueCode = udtAdditionalFieldL2j.AdditionalFieldValueCode) Then
                        '                IsDuplicated = j
                        '                IsContinue = False
                        '            End If
                        '        End If
                        '    End If
                        'End If

                        If Not (udtAdditionalFieldL1i Is Nothing Or udtAdditionalFieldL1j Is Nothing) Then
                            If Not (udtAdditionalFieldL1i.AdditionalFieldID = udtAdditionalFieldL1j.AdditionalFieldID) Then
                                If (udtAdditionalFieldL1i.AdditionalFieldValueCode = udtAdditionalFieldL1j.AdditionalFieldValueCode) Then

                                    If Not (udtAdditionalFieldL2i Is Nothing Or udtAdditionalFieldL2j Is Nothing) Then
                                        ' With Level 2
                                        If (udtAdditionalFieldL2i.AdditionalFieldValueCode = udtAdditionalFieldL2j.AdditionalFieldValueCode) Then
                                            IsDuplicated = j
                                            IsContinue = False
                                        End If

                                    Else
                                        ' Check Level 1 only without Level 2
                                        IsDuplicated = j
                                        IsContinue = False
                                    End If

                                End If
                            End If
                        End If
                        ' CRE19-006 (DHC) [End][Winnie]
                    End If

                Next
            Next
            Return IsDuplicated
        End Function


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    End Class
End Namespace