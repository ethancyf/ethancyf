Imports Common.Component

Namespace ComObject
    <Serializable()> Public Class SystemMessage
        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        Public Const MSG_DELIMITER As String = "-"
        ' CRE13-001 - EHAPP [End][Tommy L]

        Private _strFunctionCode As String
        Private _strSeverityCode As String
        Private _strMessageCode As String
        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _dtReplaceMessage As DataTable

        Public Const TradChinese As String = "zh-tw"
        Public Const SimpChinese As String = "zh-cn"
        Public Const English As String = "en-us"
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        Public Sub New(ByVal strFunctionCode As String, ByVal strSeverityCode As String, ByVal strMessageCode As String)
            _strFunctionCode = strFunctionCode
            _strSeverityCode = strSeverityCode
            _strMessageCode = strMessageCode
            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _dtReplaceMessage = CreateDataTableForReplaceMessage()
            'CRE15-004 (TIV and QIV) [End][Chris YIM]
        End Sub

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        Public Sub New(ByVal strResourceCode As String)
            Dim strSystemMessage() As String = strResourceCode.Split(New String() {MSG_DELIMITER}, StringSplitOptions.RemoveEmptyEntries)

            If strSystemMessage.Length = 3 Then
                _strFunctionCode = strSystemMessage(0)
                _strSeverityCode = strSystemMessage(1)
                _strMessageCode = strSystemMessage(2)
                'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                _dtReplaceMessage = CreateDataTableForReplaceMessage()
                'CRE15-004 (TIV and QIV) [End][Chris YIM]
            Else
                Throw New Exception("Error: Class = [Common.ComObject.SystemMessage], Method = [Constructor], Message = The format of Param - [strResourceCode] is wrong")
            End If
        End Sub
        ' CRE13-001 - EHAPP [End][Tommy L]

        Public Property FunctionCode() As String
            Get
                Return _strFunctionCode
            End Get
            Set(ByVal value As String)
                _strFunctionCode = value
            End Set
        End Property

        Public Property SeverityCode() As String
            Get
                Return _strSeverityCode
            End Get
            Set(ByVal value As String)
                _strSeverityCode = value
            End Set
        End Property

        Public Property MessageCode() As String
            Get
                Return _strMessageCode
            End Get
            Set(ByVal value As String)
                _strMessageCode = value
            End Set
        End Property

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public ReadOnly Property dtReplaceMessage() As DataTable
            Get
                Return _dtReplaceMessage
            End Get
        End Property
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        Public Function ConvertToResourceCode() As String
            Dim strCode As String
            strCode = Me.FunctionCode & "-" & Me.SeverityCode & "-" & Me.MessageCode
            Return strCode
        End Function

        Public Function GetMessage() As String
            Dim strMsg As String
            strMsg = HttpContext.GetGlobalResourceObject("SystemMessage", Me.ConvertToResourceCode)

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            ' Since windows application cannot use function "GetGlobalResourceObject", so need get resource manually
            If strMsg Is Nothing And HttpRuntime.AppDomainAppId Is Nothing Then
                strMsg = Common.Resource.CustomResourceProviderFactory.GetGlobalResourceObject("SystemMessage", Me.ConvertToResourceCode)
            End If
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Return strMsg
        End Function

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        Public Function GetMessage(ByVal enumLang As enumlanguage) As String
            Dim strMsg As String
            strMsg = Common.Resource.CustomResourceProviderFactory.GetGlobalResourceObject("SystemMessage", Me.ConvertToResourceCode, enumLang)
            Return strMsg
        End Function
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Function CreateDataTableForReplaceMessage() As DataTable
            Dim dt As DataTable
            Dim dc As DataColumn

            dt = New DataTable("TableReplaceMessage")

            dc = New DataColumn()
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "Key"
            dt.Columns.Add(dc)

            dc = New DataColumn()
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "ReplaceMessage"
            dt.Columns.Add(dc)

            Return dt

        End Function
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Sub GetReplaceMessage(ByVal strKey As String, ByRef lstStrIdx As List(Of String), ByRef lstStrReplaceMessage As List(Of String))
            Dim drResReplaceMessage() As DataRow = Nothing
            Dim dtResReplaceMessage As DataTable = Nothing
            Dim strRes As String = String.Empty

            If strKey.Equals(String.Empty) Then
                drResReplaceMessage = _dtReplaceMessage.Select()
            Else
                drResReplaceMessage = _dtReplaceMessage.Select("Key = '" + strKey + "'")
            End If

            If drResReplaceMessage.Length > 0 Then
                lstStrIdx = New List(Of String)
                lstStrReplaceMessage = New List(Of String)

                For Each dr As DataRow In drResReplaceMessage
                    lstStrIdx.Add(dr.Item("Key").ToString.Trim)
                    lstStrReplaceMessage.Add(dr.Item("ReplaceMessage").ToString.Trim)
                Next
            End If

        End Sub
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Sub AddReplaceMessage(ByVal strkey As String, ByVal strReplaceMessage As String)
            Dim dr As DataRow = _dtReplaceMessage.NewRow

            dr("Key") = strkey
            dr("ReplaceMessage") = strReplaceMessage

            _dtReplaceMessage.Rows.Add(dr)
        End Sub
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

    End Class
End Namespace