' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

Namespace Component.Profession
    <Serializable()> Public Class ProfessionModelCollection
        'Inherits System.Collections.SortedList
        Inherits System.Collections.ArrayList


        Public Enum EnumPeriodType
            Enrollment
            Claim
            ServiceDirectory
        End Enum

        Public Overloads Sub add(ByVal udtProfessionModel As ProfessionModel)
            MyBase.Add(udtProfessionModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strServiceCategoryCode As String) As ProfessionModel
            Get
                Dim intIdx As Integer
                Dim udtProfession As ProfessionModel

                For intIdx = 0 To MyBase.Count - 1
                    udtProfession = CType(MyBase.Item(intIdx), ProfessionModel)
                    If udtProfession.ServiceCategoryCode = strServiceCategoryCode Then
                        Return udtProfession
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub remove(ByVal udtProfessionModel As ProfessionModel)
            MyBase.Remove(udtProfessionModel)
        End Sub

        Public Function Filter(ByVal strServiceCategoryCode As String) As ProfessionModelCollection
            Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
            Dim udtProfessionModel As ProfessionModel
            For Each udtProfessionModel In Me
                If udtProfessionModel.ServiceCategoryCode = strServiceCategoryCode Then
                    udtProfessionModelCollection.add(udtProfessionModel)
                End If
            Next

            Return udtProfessionModelCollection
        End Function

        Public Function FilterByPeriod(ByVal enumPeriodType As EnumPeriodType) As ProfessionModelCollection

            Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
            Dim udtProfessionModel As ProfessionModel
            Dim tblProfession As New Hashtable
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Dim dtmSystem As DateTime = (New Common.ComFunction.GeneralFunction).GetSystemDateTime()
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Koala]

            For Each udtProfessionModel In Me
                If enumPeriodType = enumPeriodType.Enrollment Then
                    If udtProfessionModel.IsEnrolPeriod(dtmSystem) Then
                        udtProfessionModelCollection.add(udtProfessionModel)
                    End If
                End If

                If enumPeriodType = enumPeriodType.ServiceDirectory Then
                    If Not tblProfession.Contains(udtProfessionModel.ServiceCategoryCodeSD) Then
                        If udtProfessionModel.IsSDPeriod(dtmSystem) Then
                            udtProfessionModelCollection.add(udtProfessionModel)
                            tblProfession.Add(udtProfessionModel.ServiceCategoryCodeSD, udtProfessionModel.ServiceCategoryCodeSD)
                        End If
                    End If
                End If

                If enumPeriodType = enumPeriodType.Claim Then
                    If udtProfessionModel.IsClaimPeriod(dtmSystem) Then
                        udtProfessionModelCollection.add(udtProfessionModel)
                    End If
                End If
            Next

            Return udtProfessionModelCollection
        End Function

        Public Enum enumSortBy
            ServiceCategoryCode
            ServiceCategoryDesc
            SDDisplaySeq
        End Enum

        Public Overloads Sub Sort(ByVal eSortBy As enumSortBy)
            Select Case eSortBy
                Case enumSortBy.ServiceCategoryCode
                    MyBase.Sort(New SortingComparer(eSortBy))
                Case enumSortBy.ServiceCategoryDesc
                    MyBase.Sort(New SortingComparer(eSortBy))
                Case enumSortBy.SDDisplaySeq
                    MyBase.Sort(New SortingComparer(eSortBy))
            End Select
        End Sub

        ''' <summary>
        ''' Sort TransactionDetailModel by Service date + english subsidize name
        ''' </summary>
        ''' <remarks></remarks>
        Private Class SortingComparer
            Implements System.Collections.IComparer

            Private _eSortBy As enumSortBy

            Public Sub New(ByVal eSortBy As enumSortBy)
                _eSortBy = eSortBy
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim iResult As Integer = 0

                If x.GetType Is GetType(ProfessionModel) AndAlso y.GetType Is GetType(ProfessionModel) Then
                    If _eSortBy = enumSortBy.ServiceCategoryCode Then
                        iResult = CType(x, ProfessionModel).ServiceCategoryCode.CompareTo(CType(y, ProfessionModel).ServiceCategoryCode)
                    End If

                    If _eSortBy = enumSortBy.ServiceCategoryDesc Then
                        iResult = CType(x, ProfessionModel).ServiceCategoryDesc.CompareTo(CType(y, ProfessionModel).ServiceCategoryDesc)
                    End If

                    If _eSortBy = enumSortBy.SDDisplaySeq Then
                        iResult = CType(x, ProfessionModel).SDDisplaySeq.CompareTo(CType(y, ProfessionModel).SDDisplaySeq)
                    End If
                Else
                    If x.GetType Is GetType(ProfessionModel) Then
                        iResult = -1
                    End If
                    If y.GetType Is GetType(ProfessionModel) Then
                        iResult = 1
                    End If
                End If
                Return iResult
            End Function
        End Class

    End Class
End Namespace

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]
