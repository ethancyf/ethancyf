Namespace Component.DocType
    <Serializable()> Public Class DocTypeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtDocTypeModel As DocTypeModel)
            MyBase.Add(udtDocTypeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtDocTypeModel As DocTypeModel)
            MyBase.Remove(udtDocTypeModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As DocTypeModel
            Get
                Return CType(MyBase.Item(intIndex), DocTypeModel)
            End Get
        End Property

        Public Function Filter(ByVal strDocCode As String) As DocTypeModel
            Dim udtResDocTypeModel As DocTypeModel = Nothing
            For Each udtDocTypeModel As DocTypeModel In Me
                If udtDocTypeModel.DocCode.Trim.ToUpper().Equals(strDocCode.Trim().ToUpper()) Then
                    udtResDocTypeModel = udtDocTypeModel
                End If
            Next
            Return udtResDocTypeModel
        End Function

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Function FilterByHCSPSubPlatform(ByVal enumSubPlatform As EnumHCSPSubPlatform) As DocTypeModelCollection
            Dim udtDocTypeList As New DocTypeModelCollection

            For Each udtDocType As DocTypeModel In Me
                If udtDocType.AvailableHCSPSubPlatform = DocTypeModel.EnumAvailableHCSPSubPlatform.ALL _
                        OrElse udtDocType.AvailableHCSPSubPlatform.ToString = enumSubPlatform.ToString Then
                    udtDocTypeList.Add(New DocTypeModel(udtDocType))
                End If

            Next

            Return udtDocTypeList

        End Function
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        Public Function FilterForVaccinationRecordEnquriySearch() As DocTypeModelCollection
            Dim udtDocTypeList As New DocTypeModelCollection

            For Each udtDocType As DocTypeModel In Me
                Select Case udtDocType.DocCode
                    Case DocTypeModel.DocTypeCode.HKIC, _
                        DocTypeModel.DocTypeCode.EC, _
                        DocTypeModel.DocTypeCode.DI, _
                        DocTypeModel.DocTypeCode.HKBC, _
                        DocTypeModel.DocTypeCode.REPMT, _
                        DocTypeModel.DocTypeCode.ID235B, _
                        DocTypeModel.DocTypeCode.VISA, _
                        DocTypeModel.DocTypeCode.ADOPC

                        udtDocTypeList.Add(New DocTypeModel(udtDocType))

                End Select
            Next

            Return udtDocTypeList

        End Function

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterBySchemeDocTypeList(ByVal udtSchemeDocTypeList As SchemeDocTypeModelCollection) As DocTypeModelCollection
            Dim udtDocTypeList As New DocTypeModelCollection

            For Each udtSchemeDocType As SchemeDocTypeModel In udtSchemeDocTypeList
                Dim udtDocType As DocTypeModel = Me.Filter(udtSchemeDocType.DocCode)

                If Not udtDocType Is Nothing Then
                    udtDocTypeList.Add(New DocTypeModel(udtDocType))
                End If

            Next

            Return udtDocTypeList

        End Function
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function SortByDisplaySeq() As DocTypeModelCollection
            Dim udtDocTypeList As New DocTypeModelCollection
            Dim slstDocTypeList As New SortedList(Of Integer, DocTypeModel)

            For Each udtDocType As DocTypeModel In Me
                slstDocTypeList.Add(udtDocType.DisplaySeq, udtDocType)
            Next

            For i As Integer = 0 To slstDocTypeList.Count - 1
                udtDocTypeList.Add(New DocTypeModel(slstDocTypeList.Values.Item(i)))
            Next

            Return udtDocTypeList

        End Function
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
    End Class
End Namespace
