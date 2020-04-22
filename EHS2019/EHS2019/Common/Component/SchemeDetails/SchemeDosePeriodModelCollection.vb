Namespace Component.SchemeDetails

    <Serializable()> Public Class SchemeDosePeriodModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeDosePeriodModel As SchemeDosePeriodModel)
            MyBase.Add(udtSchemeDosePeriodModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeDosePeriodModel As SchemeDosePeriodModel)
            MyBase.Remove(udtSchemeDosePeriodModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeDosePeriodModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeDosePeriodModel)
            End Get
        End Property

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function Filter(ByVal strSchemeCode As String) As SchemeDosePeriodModelCollection

            Dim udtSchemeDosePeriodList As New SchemeDosePeriodModelCollection()

            For Each udtSchemeDosePeriodModel As SchemeDosePeriodModel In Me

                If udtSchemeDosePeriodModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    udtSchemeDosePeriodList.Add(New SchemeDosePeriodModel(udtSchemeDosePeriodModel))
                End If
            Next
            Return udtSchemeDosePeriodList
        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer) As SchemeDosePeriodModelCollection

            Dim udtSchemeDosePeriodList As New SchemeDosePeriodModelCollection()

            For Each udtSchemeDosePeriodModel As SchemeDosePeriodModel In Me

                If udtSchemeDosePeriodModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso udtSchemeDosePeriodModel.SchemeSeq = intSchemeSeq Then
                    udtSchemeDosePeriodList.Add(New SchemeDosePeriodModel(udtSchemeDosePeriodModel))
                End If
            Next
            Return udtSchemeDosePeriodList
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As SchemeDosePeriodModelCollection
            Dim udtSchemeDosePeriodList As New SchemeDosePeriodModelCollection()

            For Each udtSchemeDosePeriodModel As SchemeDosePeriodModel In Me

                If udtSchemeDosePeriodModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSchemeDosePeriodModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtSchemeDosePeriodModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtSchemeDosePeriodList.Add(New SchemeDosePeriodModel(udtSchemeDosePeriodModel))
                End If
            Next
            Return udtSchemeDosePeriodList
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strDoseName As String) As SchemeDosePeriodModel

            Dim udtReturnSchemeDosePeriodModel As SchemeDosePeriodModel = Nothing
            For Each udtSchemeDosePeriodModel As SchemeDosePeriodModel In Me

                If udtSchemeDosePeriodModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSchemeDosePeriodModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtSchemeDosePeriodModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtSchemeDosePeriodModel.DoseName.Trim().ToUpper().Equals(strDoseName.Trim().ToUpper()) Then
                    udtReturnSchemeDosePeriodModel = udtSchemeDosePeriodModel
                End If
            Next
            Return udtReturnSchemeDosePeriodModel
        End Function

    End Class
End Namespace

