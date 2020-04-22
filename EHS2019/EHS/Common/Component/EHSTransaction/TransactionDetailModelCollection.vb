Namespace Component.EHSTransaction
    <Serializable()> Public Class TransactionDetailModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtTranDetailModel As TransactionDetailModel)
            MyBase.Add(udtTranDetailModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtTranDetailModel As TransactionDetailModel)
            MyBase.Remove(udtTranDetailModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As TransactionDetailModel
            Get
                Return CType(MyBase.Item(intIndex), TransactionDetailModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As TransactionDetailModelCollection
            Dim udtResTransactionDetailList As New TransactionDetailModelCollection()
            For Each udtTransactionDetailModel As TransactionDetailModel In Me

                If udtTransactionDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtTransactionDetailModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtTransactionDetailModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then

                    udtResTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetailModel))
                End If
            Next
            Return udtResTransactionDetailList
        End Function

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterBySchemeSeq(ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As TransactionDetailModelCollection
            Dim udtResTransactionDetailList As New TransactionDetailModelCollection()
            For Each udtTransactionDetailModel As TransactionDetailModel In Me

                If udtTransactionDetailModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtTransactionDetailModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then

                    udtResTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetailModel))
                End If
            Next
            Return udtResTransactionDetailList
        End Function

        Public Function FilterByServiceDate(ByVal dtmDate As Date) As TransactionDetailModelCollection
            Dim udtResTransactionDetailList As New TransactionDetailModelCollection()
            For Each udtTransactionDetailModel As TransactionDetailModel In Me

                If udtTransactionDetailModel.ServiceReceiveDtm <= dtmDate Then
                    udtResTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetailModel))
                End If
            Next
            Return udtResTransactionDetailList
        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Filter TransactionDetailModel by Scheme + Subsidize + Service date + Profession
        ''' </summary>
        ''' <remarks></remarks>
        Public Function FilterByPeriodProfession(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                                 ByVal dtmStartDate As DateTime, dtmEndDate As DateTime, _
                                                 ByVal strProfCode As String) As TransactionDetailModelCollection

            Dim udtResTransactionDetailList As New TransactionDetailModelCollection()
            For Each udtTransactionDetailModel As TransactionDetailModel In Me

                If udtTransactionDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtTransactionDetailModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtTransactionDetailModel.ServiceReceiveDtm >= dtmStartDate AndAlso _
                    udtTransactionDetailModel.ServiceReceiveDtm < dtmEndDate AndAlso _
                    udtTransactionDetailModel.ServiceType.Trim().ToUpper().Equals(strProfCode.Trim().ToUpper()) Then

                    udtResTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetailModel))
                End If
            Next

            Return udtResTransactionDetailList
        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

        Public Function RemoveByTransactionID(ByVal strTransactionID As String) As TransactionDetailModelCollection
            Dim udtResTransactionDetailList As New TransactionDetailModelCollection()
            For Each udtTransactionDetailModel As TransactionDetailModel In Me
                If udtTransactionDetailModel.TransactionID.Trim().ToUpper() <> strTransactionID.Trim().ToUpper() Then
                    udtResTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetailModel))
                End If
            Next
            Return udtResTransactionDetailList
        End Function

        Public Function FilterBySubsidize(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As TransactionDetailModelCollection
            Dim udtResTranDetailModelList As New TransactionDetailModelCollection()
            For Each udtTranDetailModel As TransactionDetailModel In Me
                If udtTranDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.SchemeSeq = intSchemeSeq Then
                    udtResTranDetailModelList.Add(New TransactionDetailModel(udtTranDetailModel))
                End If
            Next
            Return udtResTranDetailModelList
        End Function

        Public Function FilterBySubsidizeItemDetail(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strSubsidizeitemCode As String, ByVal strAvailableItemCode As String) As TransactionDetailModelCollection
            Dim udtResTranDetailModelList As New TransactionDetailModelCollection()
            For Each udtTranDetailModel As TransactionDetailModel In Me
                If udtTranDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtTranDetailModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeitemCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.AvailableItemCode.Trim().ToUpper().Equals(strAvailableItemCode.Trim().ToUpper()) Then
                    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                    ' ----------------------------------------------------------
                    udtResTranDetailModelList.Add(udtTranDetailModel)
                    'udtResTranDetailModelList.Add(New TransactionDetailModel(udtTranDetailModel))
                    ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
                End If
            Next
            Return udtResTranDetailModelList
        End Function

        Public Function FilterBySubsidizeItemDetail(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeItemCode As String) As TransactionDetailModelCollection
            Dim udtResTranDetailModelList As New TransactionDetailModelCollection()
            For Each udtTranDetailModel As TransactionDetailModel In Me
                If udtTranDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtTranDetailModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                    ' ----------------------------------------------------------
                    udtResTranDetailModelList.Add(udtTranDetailModel)
                    'udtResTranDetailModelList.Add(New TransactionDetailModel(udtTranDetailModel))
                    ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
                End If
            Next

            Return udtResTranDetailModelList
        End Function

        Public Function FilterBySubsidizeItemDetail(ByVal strSubsidizeItemCode As String) As TransactionDetailModelCollection
            Dim udtResTranDetailModelList As New TransactionDetailModelCollection()

            For Each udtTranDetailModel As TransactionDetailModel In Me
                If udtTranDetailModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    udtResTranDetailModelList.Add(New TransactionDetailModel(udtTranDetailModel))
                End If
            Next

            Return udtResTranDetailModelList
        End Function

        Public Function FilterBySubsidizeItemDetail(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, _
             ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String) As TransactionDetailModelCollection
            Dim udtResTranDetailModelList As New TransactionDetailModelCollection()
            For Each udtTranDetailModel As TransactionDetailModel In Me
                If udtTranDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtTranDetailModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) AndAlso _
                    udtTranDetailModel.AvailableItemCode.Trim().ToUpper().Equals(strAvailableItemCode.Trim().ToUpper()) Then
                    udtResTranDetailModelList.Add(New TransactionDetailModel(udtTranDetailModel))
                End If
            Next
            Return udtResTranDetailModelList
        End Function

        Public Function FilterByAvailableCode(ByVal strAvailableItemCode As String) As TransactionDetailModel
            For Each udtTranDetailModel As TransactionDetailModel In Me
                If udtTranDetailModel.AvailableItemCode.Trim().ToUpper().Equals(strAvailableItemCode.Trim().ToUpper()) Then
                    Return udtTranDetailModel
                End If
            Next
            Return Nothing
        End Function

        Public Overloads Function Contains(ByVal udtTranDetail As TransactionDetailModel) As Boolean
            If Me.FilterBySubsidizeItemDetail(udtTranDetail.SchemeCode, udtTranDetail.SchemeSeq, udtTranDetail.SubsidizeCode, _
                                              udtTranDetail.SubsidizeItemCode, udtTranDetail.AvailableItemCode).Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function ContainAvailableCode(ByVal strAvailableItemCode As String) As Boolean

            For Each udtTranDetailModel As TransactionDetailModel In Me
                If udtTranDetailModel.AvailableItemCode.Trim().ToUpper().Equals(strAvailableItemCode.Trim().ToUpper()) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Enum enumSortBy
            ServiceDate
        End Enum

        Public Overloads Sub Sort(ByVal eSortBy As enumSortBy, ByVal oSortDirection As SortDirection)
            Select Case eSortBy
                Case enumSortBy.ServiceDate
                    MyBase.Sort(New ServiceDateComparer(oSortDirection))
            End Select
        End Sub

        ''' <summary>
        ''' Sort TransactionDetailModel by Service date + english subsidize name
        ''' </summary>
        ''' <remarks></remarks>
        Private Class ServiceDateComparer
            Implements System.Collections.IComparer

            Private _oSortDirection As SortDirection

            Public Sub New(ByVal oSortDirection As SortDirection)
                _oSortDirection = oSortDirection
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim iResult As Integer = 0

                If x.GetType Is GetType(TransactionDetailModel) AndAlso y.GetType Is GetType(TransactionDetailModel) Then
                    iResult = CType(x, TransactionDetailModel).ServiceReceiveDtm.CompareTo(CType(y, TransactionDetailModel).ServiceReceiveDtm)
                    If iResult = 0 Then
                        ' if same service date then sort by subsidize name 
                        ' Subsidize name always sort ascending
                        Return CType(x, TransactionDetailModel).SubsidizeItemCode.CompareTo(CType(y, TransactionDetailModel).SubsidizeItemCode)
                    End If
                Else
                    If x.GetType Is GetType(TransactionDetailModel) Then
                        iResult = -1
                    End If
                    If y.GetType Is GetType(TransactionDetailModel) Then
                        iResult = 1
                    End If
                End If

                Select Case _oSortDirection
                    Case SortDirection.Ascending
                        Return iResult
                    Case SortDirection.Descending
                        Return iResult * -1
                End Select
            End Function
        End Class
    End Class
End Namespace
