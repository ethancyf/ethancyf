Public Class FunctionActionListModel

    Private Const VALUE_YES As String = "Y"
    Private Const VALUE_NO As String = "N"

    Private _strFunctionCode As String
    Public ReadOnly Property FunctionCode() As String
        Get
            Return _strFunctionCode
        End Get
    End Property

    Private _strLogID As String
    Public ReadOnly Property LogID() As String
        Get
            Return _strLogID
        End Get
    End Property

    Private _strIsLogEHAInfo As String
    Public ReadOnly Property IsLogEHAInfo() As Boolean
        Get
            Return _strIsLogEHAInfo = VALUE_YES
        End Get
    End Property

    Private _strIsLogEHADocInfo As String
    Public ReadOnly Property IsLogEHADocInfo() As Boolean
        Get
            Return _strIsLogEHADocInfo = VALUE_YES
        End Get
    End Property

    Private _strIsLogSPID As String
    Public ReadOnly Property IsLogSPID() As Boolean
        Get
            Return _strIsLogSPID = VALUE_YES
        End Get
    End Property

    Private _strIsLogSPHKIC As String
    Public ReadOnly Property IsLogSPHKIC() As Boolean
        Get
            Return _strIsLogSPHKIC = VALUE_YES
        End Get
    End Property

    ''' <summary>
    ''' Check is it all IsLog property is false
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NoLogRequire() As Boolean
        Get
            Return Not IsLogEHAInfo And _
                    Not IsLogEHADocInfo And _
                    Not IsLogSPID And _
                    Not IsLogSPHKIC
        End Get
    End Property

    Public Sub New(ByVal strFunctionCode As String, ByVal strLogID As String)
        _strFunctionCode = strFunctionCode
        _strLogID = strLogID
        _strIsLogEHAInfo = VALUE_NO
        _strIsLogEHADocInfo = VALUE_NO
        _strIsLogSPID = VALUE_NO
        _strIsLogSPHKIC = VALUE_NO
    End Sub

    Public Sub New(ByVal strFunctionCode As String, ByVal strLogID As String, _
                    ByVal strIsLogEHAInfo As String, ByVal strIsLogEHADocInfo As String, _
                    ByVal strIsLogSPID As String, ByVal strIsLogSPHKIC As String)
        _strFunctionCode = strFunctionCode
        _strLogID = strLogID
        _strIsLogEHAInfo = strIsLogEHAInfo
        _strIsLogEHADocInfo = strIsLogEHADocInfo
        _strIsLogSPID = strIsLogSPID
        _strIsLogSPHKIC = strIsLogSPHKIC
    End Sub
End Class
