'Integration Start

Imports System.Data.SqlClient
Imports Common.Component.PracticeType_PCD
Imports Common.Component.StaticData

Namespace Component.PracticeType_PCD
    Public Class PracticeType_PCDBLL

        Public Const strColumnName As String = "PRACTICETYPE_PCD"

        Public Shared Function GetPracticeTypeList() As PracticeType_PCDModelCollection

            Dim udtStaticDataBLL As New StaticDataBLL
            Return New PracticeType_PCDModelCollection(udtStaticDataBLL.GetStaticDataListByColumnName(strColumnName))

        End Function

        Public Shared Function GetPracticeTypeByCode(ByVal strItemNo As String) As PracticeType_PCDModel

            Dim udtStaticDataBLL As New StaticDataBLL
            Return New PracticeType_PCDModel(udtStaticDataBLL.GetStaticDataByColumnNameItemNo(strColumnName, strItemNo))

        End Function

    End Class
End Namespace

'Integration End