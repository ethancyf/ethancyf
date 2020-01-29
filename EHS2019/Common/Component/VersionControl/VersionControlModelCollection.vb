' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.VersionControl

Namespace Component.VersionControl
    <Serializable()> Public Class VersionControlModelCollection
        'Inherits System.Collections.SortedList
        Inherits System.Collections.ArrayList

        Public Overloads Sub add(ByVal udtVersionControlModel As VersionControlModel)
            MyBase.Add(udtVersionControlModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strLogicalName As String) As VersionControlModel
            Get
                Dim intIdx As Integer
                Dim udtVersionControl As VersionControlModel

                For intIdx = 0 To MyBase.Count - 1
                    udtVersionControl = CType(MyBase.Item(intIdx), VersionControlModel)
                    If udtVersionControl.LogicalName.ToString = strLogicalName Then
                        Return udtVersionControl
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub remove(ByVal udtVersionControlModel As VersionControlModel)
            MyBase.Remove(udtVersionControlModel)
        End Sub

        Public Function Filter(ByVal strLogicalName As String) As VersionControlModelCollection
            Dim udtVersionControlModelCollection As VersionControlModelCollection = New VersionControlModelCollection
            Dim udtVersionControlModel As VersionControlModel
            For Each udtVersionControlModel In Me
                If udtVersionControlModel.LogicalName = strLogicalName Then
                    udtVersionControlModelCollection.add(udtVersionControlModel)
                End If
            Next

            Return udtVersionControlModelCollection
        End Function
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
        'Fix for getting too many DB datetime
        Public Function FilterByPeriod(ByVal strLogicalName As String, ByVal dtmDateTime As DateTime) As VersionControlModelCollection
            'Public Function FilterByPeriod(ByVal strLogicalName As String) As VersionControlModelCollection
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

            Dim udtVersionControlModelCollection As VersionControlModelCollection = New VersionControlModelCollection
            Dim udtVersionControlModel As VersionControlModel

            For Each udtVersionControlModel In Me
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
                'Fix for getting too many DB datetime
                If udtVersionControlModel.LogicalName = strLogicalName AndAlso udtVersionControlModel.IsEffective(dtmDateTime) Then
                    'If udtVersionControlModel.IsEffective  And  udtVersionControlModel.LogicalName = strLogicalName Then
                    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
                    udtVersionControlModelCollection.add(udtVersionControlModel)
                End If

            Next

            Return udtVersionControlModelCollection
        End Function

    End Class
End Namespace

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
