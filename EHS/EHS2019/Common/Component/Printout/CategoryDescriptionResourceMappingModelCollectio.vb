Namespace Component.Printout
    <Serializable()> Public Class CategoryDescriptionResourceMappingModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtCategoryDescriptionResourceMapping As CategoryDescriptionResourceMappingModel)
            MyBase.Add(udtCategoryDescriptionResourceMapping)
        End Sub

        Public Overloads Sub Remove(ByVal udtCategoryDescriptionResourceMapping As CategoryDescriptionResourceMappingModel)
            MyBase.Remove(udtCategoryDescriptionResourceMapping)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As CategoryDescriptionResourceMappingModel
            Get
                Return CType(MyBase.Item(intIndex), CategoryDescriptionResourceMappingModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strCategoryCode As String) As CategoryDescriptionResourceMappingModelCollection

            Dim udtCollection As CategoryDescriptionResourceMappingModelCollection = New CategoryDescriptionResourceMappingModelCollection()

            For Each udtCategoryDescriptionResourceMappingModel As CategoryDescriptionResourceMappingModel In Me
                If udtCategoryDescriptionResourceMappingModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtCategoryDescriptionResourceMappingModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtCategoryDescriptionResourceMappingModel.CategoryCode.Trim().ToUpper().Equals(strCategoryCode.Trim().ToUpper()) Then

                    udtCollection.Add(New CategoryDescriptionResourceMappingModel(udtCategoryDescriptionResourceMappingModel))
                End If
            Next

            Return udtCollection

        End Function


        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strCategoryCode As String, ByVal strRecordStatus As String) As CategoryDescriptionResourceMappingModelCollection

            Dim udtCollection As CategoryDescriptionResourceMappingModelCollection = New CategoryDescriptionResourceMappingModelCollection()

            For Each udtCategoryDescriptionResourceMappingModel As CategoryDescriptionResourceMappingModel In Me
                If udtCategoryDescriptionResourceMappingModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtCategoryDescriptionResourceMappingModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtCategoryDescriptionResourceMappingModel.CategoryCode.Trim().ToUpper().Equals(strCategoryCode.Trim().ToUpper()) AndAlso _
                    udtCategoryDescriptionResourceMappingModel.RecordStatus.Trim().ToUpper().Equals(strRecordStatus.Trim().ToUpper()) Then

                    udtCollection.Add(New CategoryDescriptionResourceMappingModel(udtCategoryDescriptionResourceMappingModel))
                End If
            Next

            Return udtCollection

        End Function

    End Class
End Namespace