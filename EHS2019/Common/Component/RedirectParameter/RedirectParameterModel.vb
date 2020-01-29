Namespace Component.RedirectParameter

    <Serializable()> _
    Public Class RedirectParameterModel

        Private _strEHealthAccountID As String
        Private _strEHealthAccountDocCode As String
        Private _strEHealthAccountDocNo As String
        Private _strEHealthAccountReferenceNo As String
        Private _strSPID As String
        Private _strTransactionNo As String
        Private _lstActionList As List(Of EnumRedirectAction)
        Private _strSourceFunctionCode As String
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Private _strTargetFunctionCode As String
        Private _cllnSearchCriteria As SearchCriteriaCollection
        Private _udtReturnParameter As RedirectParameterModel
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        '
        Public Enum EnumRedirectAction
            Search
            ViewDetail
        End Enum

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder
            If _strSourceFunctionCode <> String.Empty Then sb.Append(String.Format("{0}={1}, ", "SourceFunctionCode", _strSourceFunctionCode))
            If _strEHealthAccountID <> String.Empty Then sb.Append(String.Format("{0}={1}, ", "EHealthAccountID", _strEHealthAccountID))
            If _strEHealthAccountDocCode <> String.Empty Then sb.Append(String.Format("{0}={1}, ", "EHealthAccountDocCode", _strEHealthAccountDocCode))
            If _strEHealthAccountReferenceNo <> String.Empty Then sb.Append(String.Format("{0}={1}, ", "EHealthAccountReferenceNo", _strEHealthAccountReferenceNo))
            If _strSPID <> String.Empty Then sb.Append(String.Format("{0}={1}", "SPID", _strSPID))
            If _strTransactionNo <> String.Empty Then sb.Append(String.Format("{0}={1}, ", "TransactionNo", _strTransactionNo))

            Dim strActionList As String = String.Empty
            For Each nActionList As EnumRedirectAction In _lstActionList
                strActionList += nActionList.ToString + "|"
            Next
            If strActionList <> String.Empty Then sb.Append(String.Format("{0}={1}, ", "ActionList", strActionList.Substring(0, strActionList.Length - 1)))

            Dim str As String = sb.ToString

            If str <> String.Empty Then str = str.Substring(0, str.Length - 2)

            Return str

        End Function

        Public Sub New()
            _strEHealthAccountID = String.Empty
            _strEHealthAccountDocCode = String.Empty
            _strEHealthAccountReferenceNo = String.Empty
            _strSPID = String.Empty
            _strTransactionNo = String.Empty
            _lstActionList = New List(Of EnumRedirectAction)
            _strSourceFunctionCode = String.Empty
        End Sub

        '

        Public Property EHealthAccountID() As String
            Get
                Return _strEHealthAccountID
            End Get
            Set(ByVal value As String)
                _strEHealthAccountID = value.Trim
            End Set
        End Property

        Public Property EHealthAccountDocCode() As String
            Get
                Return _strEHealthAccountDocCode
            End Get
            Set(ByVal value As String)
                _strEHealthAccountDocCode = value.Trim
            End Set
        End Property

        Public Property EHealthAccountDocNo() As String
            Get
                Return _strEHealthAccountDocNo
            End Get
            Set(ByVal value As String)
                _strEHealthAccountDocNo = value.Trim
            End Set
        End Property

        Public Property EHealthAccountReferenceNo() As String
            Get
                Return _strEHealthAccountReferenceNo
            End Get
            Set(ByVal value As String)
                _strEHealthAccountReferenceNo = value.Trim
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value.Trim
            End Set
        End Property

        Public Property TransactionNo() As String
            Get
                Return _strTransactionNo
            End Get
            Set(ByVal value As String)
                _strTransactionNo = value.Trim
            End Set
        End Property

        Public Property ActionList() As List(Of EnumRedirectAction)
            Get
                Return _lstActionList
            End Get
            Set(ByVal value As List(Of EnumRedirectAction))
                _lstActionList = value
            End Set
        End Property

        Public Property SourceFunctionCode() As String
            Get
                Return _strSourceFunctionCode
            End Get
            Set(ByVal value As String)
                _strSourceFunctionCode = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Public Property TargetFunctionCode() As String
            Get
                Return _strTargetFunctionCode
            End Get
            Set(ByVal value As String)
                _strTargetFunctionCode = value
            End Set
        End Property

        Public Property SearchCriteria() As SearchCriteriaCollection
            Get
                Return _cllnSearchCriteria
            End Get
            Set(ByVal value As SearchCriteriaCollection)
                _cllnSearchCriteria = value
            End Set
        End Property

        Public Property ReturnParameter() As RedirectParameterModel
            Get
                Return _udtReturnParameter
            End Get
            Set(ByVal value As RedirectParameterModel)
                _udtReturnParameter = value
            End Set
        End Property
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    End Class

End Namespace
