Imports Common.Component

<Serializable()> Public Class StatisticsModel

#Region "Private member"
    Private _strStatisticID As String
    Private _strDesc As String
    Private _strExecSP As String
    Private _dtmCreateDtm As DateTime
    Private _strCreateBy As String
    Private _dtmUpdateDtm As DateTime
    Private _strUpdateBy As String
    Private _strCriteriaSetup As String
    Private _strResultSetup As String
    Private _strExportSetup As String
    Private _chrRecordStatus As Char
    Private _chrScheme As String
    Private _strRemark As String
#End Region

#Region "Property"
    Public Property StatisticID() As String
        Get
            Return _strStatisticID
        End Get
        Set(ByVal value As String)
            _strStatisticID = value
        End Set
    End Property

    Public Property Desc() As String
        Get
            Return _strDesc
        End Get
        Set(ByVal value As String)
            _strdesc = value
        End Set
    End Property

    Public Property ExecSP() As String
        Get
            Return _strExecSP
        End Get
        Set(ByVal value As String)
            _strExecSP = value
        End Set
    End Property

    Public Property CreateDtm() As DateTime
        Get
            Return _dtmCreateDtm
        End Get
        Set(ByVal value As DateTime)
            _dtmCreateDtm = value
        End Set
    End Property

    Public Property CreateBy() As String
        Get
            Return _strCreateBy
        End Get
        Set(ByVal value As String)
            _strCreateBy = value
        End Set
    End Property

    Public Property UpdateDtm() As DateTime
        Get
            Return _dtmUpdateDtm
        End Get
        Set(ByVal value As DateTime)
            _dtmUpdateDtm = value
        End Set
    End Property

    Public Property UpdateBy() As String
        Get
            Return _strUpdateBy
        End Get
        Set(ByVal value As String)
            _strUpdateBy = value
        End Set
    End Property

    Public Property CriteriaSetup() As String
        Get
            Return _strCriteriaSetup
        End Get
        Set(ByVal value As String)
            _strCriteriaSetup = value
        End Set
    End Property

    Public Property ResultSetup() As String
        Get
            Return _strResultSetup
        End Get
        Set(ByVal value As String)
            _strResultSetup = value
        End Set
    End Property

    Public Property ExportSetup() As String
        Get
            Return _strExportSetup
        End Get
        Set(ByVal value As String)
            _strExportSetup = value
        End Set
    End Property

    Public Property RecordStatus() As Char
        Get
            Return _chrRecordStatus
        End Get
        Set(ByVal value As Char)
            _chrRecordStatus = value
        End Set
    End Property

    Public Property Scheme() As String
        Get
            Return _chrScheme
        End Get
        Set(ByVal value As String)
            _chrScheme = value
        End Set
    End Property

    Public Property Remark() As String
        Get
            Return _strRemark
        End Get
        Set(ByVal value As String)
            _strRemark = value
        End Set
    End Property


#End Region

#Region "Constructor"

    Public Sub New(ByVal strStatisticID As String, _
                   ByVal strDesc As String, _
                   ByVal strExecSP As String, _
                   ByVal dtmCreateDtm As DateTime, _
                   ByVal strCreateBy As String, _
                   ByVal dtmUpdateDtm As DateTime, _
                   ByVal strUpdateBy As String, _
                   ByVal strCriteriaSetup As String, _
                   ByVal strResultSetup As String, _
                   ByVal strExportSetup As String, _
                   ByVal chrRecordStatus As Char, _
                   ByVal chrScheme As String, _
                   ByVal strRemark As String)

        _strStatisticID = strStatisticID
        _strDesc = strDesc
        _strExecSP = strExecSP
        _dtmCreateDtm = dtmCreateDtm
        _strCreateBy = strCreateBy
        _dtmUpdateDtm = dtmUpdateDtm
        _strUpdateBy = strUpdateBy
        _strCriteriaSetup = strCriteriaSetup
        _strResultSetup = strResultSetup
        _strExportSetup = strExportSetup
        _chrRecordStatus = chrRecordStatus
        _chrScheme = chrScheme
        _strRemark = strRemark
    End Sub

#End Region

End Class
