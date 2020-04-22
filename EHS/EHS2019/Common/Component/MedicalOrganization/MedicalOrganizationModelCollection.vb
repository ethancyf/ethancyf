Imports Common.Component.MedicalOrganization

Namespace Component.MedicalOrganization
    <Serializable()> Public Class MedicalOrganizationModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel)
            MyBase.Add(udtMedicalOrganizationModel.DisplaySeq, udtMedicalOrganizationModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel)
            MyBase.Remove(udtMedicalOrganizationModel.DisplaySeq)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intDisplaySeq As Integer) As MedicalOrganizationModel
            Get
                Return CType(MyBase.Item(intDisplaySeq), MedicalOrganizationModel)
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Function GetDisplaySeq() As Integer
            Dim intLastValue As Integer = 0
            For Each udtMO As MedicalOrganizationModel In MyBase.Values
                intLastValue = udtMO.DisplaySeq
            Next

            Return intLastValue + 1
        End Function

        Public Sub DuplicateMOEName()

            ' Dim udtMOList As New MedicalOrganizationModelCollection


            Dim strMOEName As New List(Of String)
            Dim strDMOEname As String = String.Empty

            Dim intMOIndex As New List(Of Integer)


            If Not IsNothing(MyBase.Values) Then
                If MyBase.Count > 0 Then
                    For Each udtMO As MedicalOrganizationModel In MyBase.Values
                        If Not udtMO.RecordStatus.Trim.Equals("D") Then
                            strMOEName.Add(udtMO.MOEngName.Trim)
                        End If

                    Next

                    Dim strDistinctMOEname As New List(Of String)
                    Dim strDuplicationMOEname As New List(Of String)

                    For Each item As String In strMOEName
                        If Not strDistinctMOEname.Contains(item) Then
                            strDistinctMOEname.Add(item)
                        Else
                            If Not strDuplicationMOEname.Contains(item) Then
                                strDuplicationMOEname.Add(item)
                            End If
                        End If
                    Next

                    For Each udtMO As MedicalOrganizationModel In MyBase.Values
                        udtMO.IsDuplicated = False
                        For Each item As String In strDuplicationMOEname
                            If udtMO.MOEngName.Trim.Equals(item.Trim) Then
                                If Not udtMO.RecordStatus.Trim.Equals("D") Then
                                    udtMO.IsDuplicated = True
                                End If
                                'udtMOList.Add(udtMO)
                            End If
                        Next
                    Next
                End If
            End If

            'Return udtMOList

        End Sub

        Public Function GetActiveMO(ByVal strTableLocation As String) As MedicalOrganizationModelCollection
            Dim strActiveStatus As String = String.Empty
            Dim udtMOList As New MedicalOrganizationModelCollection


            Select Case strTableLocation
                Case TableLocation.Enrolment
                    udtMOList = Me
                Case TableLocation.Staging

                    For Each udtMO As MedicalOrganizationModel In MyBase.Values
                        If udtMO.RecordStatus.Trim.Equals(MedicalOrganizationStagingStatus.Active) OrElse _
                            udtMO.RecordStatus.Trim.Equals(MedicalOrganizationStagingStatus.Existing) OrElse _
                            udtMO.RecordStatus.Trim.Equals(MedicalOrganizationStagingStatus.Update) Then
                            udtMOList.Add(udtMO)
                        End If
                    Next

                Case TableLocation.Permanent

                    For Each udtMO As MedicalOrganizationModel In MyBase.Values
                        If udtMO.RecordStatus.Trim.Equals(MedicalOrganizationStatus.Active) Then
                            udtMOList.Add(udtMO)
                        End If
                    Next

            End Select

           

            Return udtMOList
        End Function

    End Class
End Namespace
