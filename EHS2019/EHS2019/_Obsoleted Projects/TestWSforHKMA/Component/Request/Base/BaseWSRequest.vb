Imports Microsoft.VisualBasic
Imports System.Xml
Imports TestWSforHKMA.Component

Namespace Component.Request.Base

    Public MustInherit Class BaseWSRequest

#Region "Private Constant"

        'Private Const ERR_TAG_NOT_FOUND As String = "{0} tag not found"
        'Private Const ERR_TAG_DUPLICATE As String = "Duplicate {0} tag found"
        'Private Const ERR_TAG_INVALID_VALUE As String = "Invalid {0} tag value"
        'Private Const ERR_ITEM_NOT_MATCH_COUNT As String = "Number of {0} is not match {1}"

#End Region

#Region "Protected Constant"

        Protected Const TAG_INPUT As String = "Input"

#End Region

#Region "Properties"

        Protected _bIsValid As Boolean = False
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return _bIsValid
            End Get
        End Property

        'Protected _strErrorCode As String = ErrorCodes.Null
        'Public ReadOnly Property ErrorCode() As String
        '    Get
        '        Return _strErrorCode
        '    End Get
        '    'Set(ByVal value As String)
        '    '    _strErrorCode = value
        '    'End Set
        'End Property

#End Region


    End Class

End Namespace


