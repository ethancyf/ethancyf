Imports Common.Component.ClaimCategory

Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeGroupBackOfficeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel)
            MyBase.Add(udtSubsidizeGroupBackOfficeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel)
            MyBase.Remove(udtSubsidizeGroupBackOfficeModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeGroupBackOfficeModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeGroupBackOfficeModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String) As SubsidizeGroupBackOfficeModel
            Dim udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel = Nothing
            For Each udtSubsidizeGroupBackOfficeM As SubsidizeGroupBackOfficeModel In Me
                If udtSubsidizeGroupBackOfficeM.SchemeCode.Trim.Equals(strSchemeCode.Trim) AndAlso udtSubsidizeGroupBackOfficeM.SubsidizeCode.Trim.Equals(strSubsidizeCode.Trim) Then
                    udtSubsidizeGroupBackOfficeModel = udtSubsidizeGroupBackOfficeM
                End If
            Next

            Return udtSubsidizeGroupBackOfficeModel
        End Function

#Region "Functions"

        Public Function ToSPProfileDataTable()
            Dim dt As New DataTable
            dt.Columns.Add("SchemeCode", GetType(String))
            dt.Columns.Add("SubsidizeCode", GetType(String))
            dt.Columns.Add("SchemeDisplayCode", GetType(String))
            dt.Columns.Add("SubsidizeDisplayCode", GetType(String))
            dt.Columns.Add("IsCategoryHeader", GetType(String))
            dt.Columns.Add("CategoryName", GetType(String))
            dt.Columns.Add("CategoryNameChi", GetType(String))
            dt.Columns.Add("CategoryNameCN", GetType(String))

            Dim strPreviousSchemeCode As String = String.Empty
            Dim strPreviousCategory As String = String.Empty
            Dim udtClaimCategoryList As ClaimCategoryModelCollection = (New ClaimCategoryBLL).GetClaimCategoryCache

            For Each udtSubGpBO As SubsidizeGroupBackOfficeModel In Me
                If udtSubGpBO.SchemeCode <> strPreviousSchemeCode Then
                    strPreviousCategory = String.Empty
                End If

                If udtSubGpBO.CategoryCode <> String.Empty AndAlso udtSubGpBO.CategoryCode <> strPreviousCategory Then
                    Dim drHeader As DataRow = dt.NewRow

                    drHeader("SchemeCode") = udtSubGpBO.SchemeCode
                    drHeader("SubsidizeCode") = udtSubGpBO.SubsidizeCode
                    drHeader("SchemeDisplayCode") = udtSubGpBO.SchemeDisplayCode
                    drHeader("SubsidizeDisplayCode") = udtSubGpBO.SubsidizeDisplayCode
                    drHeader("IsCategoryHeader") = "Y"
                    drHeader("CategoryName") = udtClaimCategoryList.Filter(udtSubGpBO.CategoryCode).CategoryName
                    drHeader("CategoryNameChi") = udtClaimCategoryList.Filter(udtSubGpBO.CategoryCode).CategoryNameChi
                    drHeader("CategoryNameCN") = udtClaimCategoryList.Filter(udtSubGpBO.CategoryCode).CategoryNameCN

                    dt.Rows.Add(drHeader)

                End If

                Dim drData As DataRow = dt.NewRow

                drData("SchemeCode") = udtSubGpBO.SchemeCode
                drData("SubsidizeCode") = udtSubGpBO.SubsidizeCode
                drData("SchemeDisplayCode") = udtSubGpBO.SchemeDisplayCode
                drData("SubsidizeDisplayCode") = udtSubGpBO.SubsidizeDisplayCode
                drData("IsCategoryHeader") = "N"

                If udtSubGpBO.CategoryCode = String.Empty Then
                    drData("CategoryName") = String.Empty
                    drData("CategoryNameChi") = String.Empty
                    drData("CategoryNameCN") = String.Empty
                Else
                    drData("CategoryName") = udtClaimCategoryList.Filter(udtSubGpBO.CategoryCode).CategoryName
                    drData("CategoryNameChi") = udtClaimCategoryList.Filter(udtSubGpBO.CategoryCode).CategoryNameChi
                    drData("CategoryNameCN") = udtClaimCategoryList.Filter(udtSubGpBO.CategoryCode).CategoryNameCN
                End If

                dt.Rows.Add(drData)

                strPreviousSchemeCode = udtSubGpBO.SchemeCode
                strPreviousCategory = udtSubGpBO.CategoryCode

            Next

            Return dt

        End Function

#End Region

    End Class

End Namespace
