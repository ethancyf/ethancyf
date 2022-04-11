Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.RVPHomeList

Namespace Component.COVID19


#Region "Class: VaccinationCardRecordModel"
    <Serializable()> Public Class VaccinationCardRecordModel

        Private _udtDoseList As VaccinationCardDoseRecordSortedList

        Public Enum LastDoseSeq
            FirstLastDose = 1
            SecondLastDose = 2
            ThirdLastDose = 3
        End Enum

#Region "Property"

        Public Property DoseList() As VaccinationCardDoseRecordSortedList
            Get
                Return _udtDoseList
            End Get
            Set(ByVal Value As VaccinationCardDoseRecordSortedList)
                _udtDoseList = Value
            End Set
        End Property

        Public ReadOnly Property MaxDoseSeq() As Integer
            Get
                Dim intMaxDoseSeq As Integer = 0

                For Each intCurrentDoseSeq As Integer In Me.DoseList.Keys
                    If intMaxDoseSeq < intCurrentDoseSeq Then
                        intMaxDoseSeq = intCurrentDoseSeq
                    End If
                Next

                Return intMaxDoseSeq
            End Get
        End Property

        Public ReadOnly Property MaxDoseOrder() As Integer
            Get
                Return Me.DoseList.Count
            End Get
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            _udtDoseList = New VaccinationCardDoseRecordSortedList
        End Sub

#End Region

#Region "Function"

        ''' <summary>
        ''' Return Dose Record by particular Dose Seq, 
        ''' If doseseq is not exist in list, return Nothing
        ''' </summary>
        ''' <param name="intDoseSeq">1=First Dose, 2=Second Dose, 3=Third Dose etc...</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDoseRecordByDose(ByVal intDoseSeq As Integer) As VaccinationCardDoseRecordModel
            Return Me.DoseList.FilterByDoseSeq(intDoseSeq)
        End Function

        ''' <summary>
        ''' Get Latest Dose, If doseseq is not exist in list, return Nothing
        ''' </summary>
        ''' <param name="enumLastDoseSeq">1=Last Dose, 2=Second Last Dose, 3=Third Last Dose etc...</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getLastDoseRecordForQRCode(ByVal enumLastDoseSeq As LastDoseSeq) As VaccinationCardDoseRecordModel
            Dim udtResult As VaccinationCardDoseRecordModel = Nothing

            'If Me.MaxDoseSeq = 1 Then
            '    'Special handle for case with 1st Dose Only
            '    'Last Dose = Nothing; Second Last Dose = 1st Dose; Third Last Dose = Nothing
            '    If enumLastDoseSeq = LastDoseSeq.SecondLastDose Then
            '        udtResult = Me.DoseList.FilterByDoseSeq(1)
            '    Else
            '        udtResult = Nothing
            '    End If

            'Else
            '    'Normal Case  e.g. Injected 1st and 3rd Dose, MaxDoseSeq = 3
            '    'Last Dose = 3rd Dose; Second Last Dose = Nothing (2nd Dose); Third Last Dose = 1st Dose
            '    udtResult = Me.DoseList.FilterByDoseSeq(MaxDoseSeq - enumLastDoseSeq + 1)
            'End If

            'Normal Case  e.g. Injected 1st and 3rd Dose, MaxDoseSeq = 3
            'Last Dose = 3rd Dose; Second Last Dose = 1st Dose; Third Last Dose = Nothing;
            If MaxDoseOrder() >= enumLastDoseSeq Then
                udtResult = CType(Me.DoseList.GetByIndex(MaxDoseOrder() - enumLastDoseSeq), VaccinationCardDoseRecordModelCollection).FilterFindNearestRecord()
            End If

            Return udtResult

        End Function

        Public Sub AddDoseRecord(ByVal udtVaccinationRecordList As TransactionDetailVaccineModelCollection)

            For Each udtVaccinationRecord As TransactionDetailVaccineModel In udtVaccinationRecordList
                Me.DoseList.Add(New VaccinationCardDoseRecordModel(udtVaccinationRecord))
            Next

        End Sub

        Public Sub AddDoseRecord(ByVal udtEHSTransaction As EHSTransactionModel)
            Me.DoseList.Add(New VaccinationCardDoseRecordModel(udtEHSTransaction))
        End Sub

#End Region

    End Class

#End Region

#Region "Class: VaccinationCardDoseRecordSortedList"
    <Serializable()> Public Class VaccinationCardDoseRecordSortedList
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtVaccinationCardDoseRecordModel As VaccinationCardDoseRecordModel)
            
            If Me.ContainsKey(udtVaccinationCardDoseRecordModel.DoseSeq) Then
                Me.Item(udtVaccinationCardDoseRecordModel.DoseSeq).Add(udtVaccinationCardDoseRecordModel)
            Else
                Dim udtVaccinationCardDoseRecordModelCollection As New VaccinationCardDoseRecordModelCollection
                udtVaccinationCardDoseRecordModelCollection.Add(udtVaccinationCardDoseRecordModel)
                Me.Add(udtVaccinationCardDoseRecordModel.DoseSeq, udtVaccinationCardDoseRecordModelCollection)
            End If

        End Sub

        Public Overloads Sub Remove(ByVal udtVaccinationCardDoseRecordModel As VaccinationCardDoseRecordModel)
            MyBase.Remove(udtVaccinationCardDoseRecordModel.DoseSeq)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VaccinationCardDoseRecordModelCollection
            Get
                Return CType(MyBase.Item(intIndex), VaccinationCardDoseRecordModelCollection)
            End Get
        End Property

        ''' <summary>
        ''' Return DoseRecordModel by particular DoseSeq, if DoseSeq not exist, return nothing; 
        ''' if 2 Dose Record with same DoseSeq, return the record with latest Injection Date
        ''' </summary>
        ''' <param name="intDoseSeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public  Function FilterByDoseSeq(ByVal intDoseSeq As Integer) As VaccinationCardDoseRecordModel
            Dim udtResult As VaccinationCardDoseRecordModel = Nothing

            If Me.ContainsKey(intDoseSeq) Then
                udtResult = Me.Item(intDoseSeq).FilterFindNearestRecord()
            End If

            Return udtResult
        End Function

    End Class

#End Region

End Namespace

