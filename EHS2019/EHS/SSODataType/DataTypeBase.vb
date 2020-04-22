' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : 
'
' Type              : User Define DataType
' Detail            : Base Class for User Define DataType
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------
<Serializable()> _
Public MustInherit Class DataTypeBase

    Public Enum SysSource
        Database
        NewAdd
    End Enum

    Protected _strSource As SysSource = SysSource.NewAdd

    ReadOnly Property IsNewAdded() As Boolean
        Get
            Return Me._strSource = SysSource.NewAdd
        End Get
    End Property

    Sub New()
        Me._strSource = SysSource.NewAdd
    End Sub

    Sub New(ByVal drRow As DataRow)
        Me._strSource = SysSource.Database
        Me.FillByDataRow(drRow)
    End Sub

    Protected Overridable Sub FillByDataRow(ByVal drRow As DataRow)
    End Sub

End Class