Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.Specialized

Namespace Component.HATransaction
    <Serializable()> Public Class VaccineCodeBySchemeMappingCollection
        Inherits System.Collections.Specialized.ListDictionary

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtVaccineCodeBySchemeMappingModel As VaccineCodeBySchemeMappingModel)
            MyBase.Add(GenerateKey(udtVaccineCodeBySchemeMappingModel), udtVaccineCodeBySchemeMappingModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtVaccineCodeBySchemeMappingModel As VaccineCodeBySchemeMappingModel)
            MyBase.Remove(GenerateKey(udtVaccineCodeBySchemeMappingModel))
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VaccineCodeBySchemeMappingModel
            Get
                Return CType(MyBase.Item(intIndex), VaccineCodeBySchemeMappingModel)
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSchemeCode">Mapping scheme</param>
        ''' <param name="strVaccineCodeSource">Mapping vaccine code</param>
        ''' <returns>Actual scheme information for bar by different scheme</returns>
        ''' <remarks></remarks>
        Public Function GetMappingByCode(ByVal strSchemeCode As String, ByVal strVaccineCodeSource As String) As VaccineCodeBySchemeMappingModel
            Return CType(MyBase.Item(GenerateKey(strSchemeCode, strVaccineCodeSource)), VaccineCodeBySchemeMappingModel)
        End Function

#Region "Private Function"

        Private Function GenerateKey(ByVal udtVaccineCodeBySchemeMappingModel As VaccineCodeBySchemeMappingModel) As String
            Return GenerateKey(udtVaccineCodeBySchemeMappingModel.SchemeCode, _
                                udtVaccineCodeBySchemeMappingModel.VaccineCodeSource)
        End Function

        Private Function GenerateKey(ByVal strSchemeCode As String, ByVal strVaccineCodeSource As String) As String
            Return strSchemeCode + "|" + strVaccineCodeSource
        End Function

#End Region
    End Class
End Namespace

