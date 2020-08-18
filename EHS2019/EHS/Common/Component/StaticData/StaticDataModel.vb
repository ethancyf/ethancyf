Namespace Component.StaticData
    <Serializable()> Public Class StaticDataModel
        Private _strColumnName As String
        Private _strItemNo As String
        Private _strDataValue As String
        Private _strDataValueChi As String
        Private _strDataValueCN As String
        Private _strDisplayOrder As String

        Public Const Column_Name As String = "Column_Name"
        Public Const Item_No As String = "Item_No"
        Public Const Data_Value As String = "Data_Value"
        Public Const Data_Value_Chi As String = "Data_Value_Chi"
        Public Const Data_Value_CN As String = "Data_Value_CN"
        Public Const Display_Order As String = "Display_Order"

        Public Property ColumnName()
            Get
                Return _strColumnName
            End Get
            Set(ByVal value)
                _strColumnName = value
            End Set
        End Property

        Public Property ItemNo()
            Get
                Return _strItemNo
            End Get
            Set(ByVal value)
                _strItemNo = value
            End Set
        End Property

        Public Property DataValue()
            Get
                Return _strDataValue
            End Get
            Set(ByVal value)
                _strDataValue = value
            End Set
        End Property

        Public Property DataValueChi()
            Get
                Return _strDataValueChi
            End Get
            Set(ByVal value)
                _strDataValueChi = value
            End Set
        End Property

        Public Property DataValueCN() As String
            Get
                Return _strDataValueCN
            End Get
            Set(ByVal value As String)
                _strDataValueCN = value
            End Set
        End Property

        Public Property DisplayOrder() As String
            Get
                Return _strDisplayOrder
            End Get
            Set(value As String)
                _strDisplayOrder = value
            End Set
        End Property

        Public Sub New(ByVal strColumnName As String, ByVal strItemNo As String, ByVal strDataValue As String, ByVal strDataValueChi As String, ByVal strDataValueCN As String, Optional ByVal strDisplayOrder As String = "")
            _strColumnName = strColumnName
            _strItemNo = strItemNo
            _strDataValue = strDataValue
            _strDataValueChi = strDataValueChi
            _strDataValueCN = strDataValueCN
            _strDisplayOrder = strDisplayOrder
        End Sub

        Public Sub New(ByVal udtStaticDataModel As StaticDataModel)
            _strColumnName = udtStaticDataModel.ColumnName
            _strItemNo = udtStaticDataModel.ItemNo
            _strDataValue = udtStaticDataModel.DataValue
            _strDataValueChi = udtStaticDataModel.DataValueChi
            _strDataValueCN = udtStaticDataModel.DataValueCN
            _strDisplayOrder = udtStaticDataModel.DisplayOrder
        End Sub

    End Class
End Namespace

