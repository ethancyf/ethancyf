'Integration Start

Namespace Component.PracticeType_PCD
    <Serializable()> Public Class PracticeType_PCDModel
        Inherits Common.Component.StaticData.StaticDataModel

        Public Sub New(ByVal strColumnName As String, ByVal strItemNo As String, ByVal strDataValue As String, ByVal strDataValueChi As String, ByVal strDataValueCN As String)
            MyBase.New(strColumnName, strItemNo, strDataValue, strDataValueChi, strDataValueCN)
        End Sub

        Public Sub New(ByVal udtPracticeTypeModel As PracticeType_PCDModel)
            MyBase.New(udtPracticeTypeModel.ColumnName, udtPracticeTypeModel.ItemNo, udtPracticeTypeModel.DataValue, udtPracticeTypeModel.DataValueChi, udtPracticeTypeModel.DataValueCN)
        End Sub

        Public Sub New(ByVal udtStaticDataModel As Common.Component.StaticData.StaticDataModel)
            MyBase.New(udtStaticDataModel.ColumnName, udtStaticDataModel.ItemNo, udtStaticDataModel.DataValue, udtStaticDataModel.DataValueChi, udtStaticDataModel.DataValueCN)
        End Sub
    End Class

End Namespace

'Integration End

