
Imports System.Collections

Namespace Component.FunctionInformation

    <Serializable()> Public Class FunctionFeatureModelCollection
        Inherits Hashtable

        Public Sub New(ByVal dtFunctionFeature As DataTable)
            For Each dr As DataRow In dtFunctionFeature.Rows
                Add(New FunctionFeatureModel(dr))
            Next
        End Sub

        Public Overloads Sub Add(ByVal udtFunctionFeatureModel As FunctionFeatureModel)
            MyBase.Add(GenKey(udtFunctionFeatureModel), udtFunctionFeatureModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtFunctionFeatureModel As FunctionFeatureModel)
            MyBase.Remove(GenKey(udtFunctionFeatureModel))
        End Sub

        Public Overloads Function ContainsKey(ByVal strFunctionCode As String, ByVal enumFeatureCode As FunctionFeatureModel.EnumFeatureCode) As Boolean
            Return MyBase.ContainsKey(GenKey(strFunctionCode, enumFeatureCode))
        End Function

        Default Public Overloads ReadOnly Property Item(ByVal strFunctionCode As String, ByVal enumFeatureCode As FunctionFeatureModel.EnumFeatureCode) As FunctionFeatureModel
            Get
                Return CType(MyBase.Item(GenKey(strFunctionCode, enumFeatureCode)), FunctionFeatureModel)
            End Get
        End Property

        Protected Function GenKey(ByVal udtFunctionFeatureModel As FunctionFeatureModel)
            Return GenKey(udtFunctionFeatureModel.FunctionCode, udtFunctionFeatureModel.FeatureCode)
        End Function

        Protected Function GenKey(ByVal strFunctionCode As String, ByVal enumFeatureCode As FunctionFeatureModel.EnumFeatureCode)
            Return strFunctionCode + enumFeatureCode.ToString
        End Function
    End Class

End Namespace
