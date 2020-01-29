Imports System.Collections.Generic
Imports Common.DataAccess

'Integration Start

Namespace Component.ProfessionPracticeType

    Public Class ProfessionPracticeTypeBLL

        Private Const CACHE_PROFESSION_PRACTICE_TYPE As String = "ProfessionPracticeType"
        Private Const SP_PROFESSION_PRACTICE_TYPE As String = "proc_ProfessionPracticeType_get_cache"

        Public Shared Function GetByProfession(ByVal objProfession As Profession.ProfessionModel) As ProfessionPracticeTypeModelCollection
            Dim hash As Hashtable = GetProfessionPracticeTypeCache()

            If Not hash.Contains(objProfession.ServiceCategoryCode) Then
                Return Nothing
            End If

            Return hash(objProfession.ServiceCategoryCode)

        End Function

        Private Shared Function GetProfessionPracticeTypeCache() As Hashtable
            Dim clln As ProfessionPracticeTypeModelCollection = Nothing
            Dim objModel As ProfessionPracticeTypeModel = Nothing

            Dim hash As New Hashtable

            If HttpContext.Current.Cache(CACHE_PROFESSION_PRACTICE_TYPE) Is Nothing Then
                Dim dt As New DataTable

                Try
                    GetDatabase.RunProc(SP_PROFESSION_PRACTICE_TYPE, dt)

                    Dim strPreviousServiceCategoryCode As String = String.Empty
                    Dim strCurrentServiceCategoryCode As String = String.Empty
                    Dim strCurrentItemNo As String = String.Empty
                    Dim objProfession As Profession.ProfessionModel

                    For Each row As DataRow In dt.Rows

                        strCurrentServiceCategoryCode = CType(row.Item("Service_Category_Code"), String).Trim
                        strCurrentItemNo = CType(row.Item("Item_No"), String).Trim

                        ' New Practice Type Collection
                        If strCurrentServiceCategoryCode <> strPreviousServiceCategoryCode Then

                            objProfession = Profession.ProfessionBLL.GetProfessionListByServiceCategoryCode(strCurrentServiceCategoryCode)
                            clln = New ProfessionPracticeTypeModelCollection(objProfession)
                            hash.Add(strCurrentServiceCategoryCode, clln)
                            strPreviousServiceCategoryCode = strCurrentServiceCategoryCode
                        End If

                        ' New Practice Type Model
                        objModel = New ProfessionPracticeTypeModel(strCurrentServiceCategoryCode, strCurrentItemNo)
                        clln.Add(objModel)

                    Next

                    Common.ComObject.CacheHandler.InsertCache(CACHE_PROFESSION_PRACTICE_TYPE, hash)
                Catch ex As Exception
                    Throw ex
                End Try
            Else
                hash = CType(HttpContext.Current.Cache(CACHE_PROFESSION_PRACTICE_TYPE), Hashtable)
            End If

            Return hash
        End Function

        Private Shared Function GetDatabase() As Database
            Return New Database
        End Function

    End Class

End Namespace

'Integration End