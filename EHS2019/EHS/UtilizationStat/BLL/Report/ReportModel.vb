Namespace Report
    <Serializable()> Public Class ReportModel
        Private _strReportID As String
        Private _strReportName As String
        Private _strReportDesc As String
        Private _strExecSP As String
        Private _strTemplateName As String
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Private _strReportFileExt As String
        Private _intStartRow As List(Of Integer)
        'Private _intStartRow() As Integer
        'CRE13-016 Upgrade to excel 2007 [End][Karl]
        Private _strFileNameFormat As String
        Private _intMinusDateForFileName As Integer
        Private _blnAuto As Boolean
        Private _strDBFlag As String

        Public Property ReportID() As String
            Get
                Return _strReportID
            End Get
            Set(ByVal value As String)
                _strReportID = value
            End Set
        End Property

        Public Property ReportName() As String
            Get
                Return _strReportName
            End Get
            Set(ByVal value As String)
                _strReportName = value
            End Set
        End Property

        Public Property ReportDesc() As String
            Get
                Return _strReportDesc
            End Get
            Set(ByVal value As String)
                _strReportDesc = value
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

        Public Property TemplateName() As String
            Get
                Return _strTemplateName
            End Get
            Set(ByVal value As String)
                _strTemplateName = value
            End Set
        End Property
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Public Property ReportFileExt() As String
            Get
                Return _strReportFileExt
            End Get
            Set(ByVal value As String)
                _strReportFileExt = value
            End Set
        End Property

        'Public Property StartRowNo() As Integer()
        '    Get
        '        Return _intStartRow
        '    End Get
        '    Set(ByVal value As Integer())
        '        _intStartRow = value
        '    End Set
        'End Property

        Public Property StartRowNo() As List(Of Integer)
            Get
                Return _intStartRow
            End Get
            Set(ByVal value As List(Of Integer))
                _intStartRow = value
            End Set
        End Property
        'CRE13-016 Upgrade to excel 2007 [End][Karl]
        Public Property FileNameFormat() As String
            Get
                Return _strFileNameFormat
            End Get
            Set(ByVal value As String)
                _strFileNameFormat = value
            End Set
        End Property

        Public Property MinusDateForFileName() As Integer
            Get
                Return _intMinusDateForFileName
            End Get
            Set(ByVal value As Integer)
                _intMinusDateForFileName = value
            End Set
        End Property

        Public Property AutoGenerate() As Boolean
            Get
                Return _blnAuto
            End Get
            Set(ByVal value As Boolean)
                _blnAuto = value
            End Set
        End Property

        Public Property DBFlag() As String
            Get
                Return _strDBFlag
            End Get
            Set(ByVal value As String)
                _strDBFlag = value
            End Set
        End Property

        Public Sub New()

        End Sub
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Public Sub New(ByVal strReportID As String, ByVal strReportName As String, ByVal strReportDesc As String, ByVal strExecSP As String, ByVal strTemplateName As String, ByVal strReportFileExt As String, ByVal intStartRow As List(Of Integer), ByVal strFileNameFormat As String, ByVal intMinusDateForFileName As Integer, ByVal blnAuto As Boolean, ByVal strDBFlag As String)
            'Public Sub New(ByVal strReportID As String, ByVal strReportName As String, ByVal strReportDesc As String, ByVal strExecSP As String, ByVal strTemplateName As String, ByVal intStartRow() As Integer, ByVal strFileNameFormat As String, ByVal intMinusDateForFileName As Integer, ByVal blnAuto As Boolean, ByVal strDBFlag As String)
            'CRE13-016 Upgrade to excel 2007 [End][Karl]
            _strReportID = strReportID
            _strReportName = strReportName
            _strReportDesc = strReportDesc
            _strExecSP = strExecSP
            _strTemplateName = strTemplateName
            'CRE13-016 Upgrade to excel 2007 [Start][Karl]
            _strReportFileExt = strReportFileExt
            'CRE13-016 Upgrade to excel 2007 [End][Karl]
            _intStartRow = intStartRow
            _strFileNameFormat = strFileNameFormat
            _intMinusDateForFileName = intMinusDateForFileName
            _blnAuto = blnAuto
            _strDBFlag = strDBFlag

        End Sub

        Public Sub New(ByVal udtReportModel As ReportModel)
            _strReportID = udtReportModel.ReportID
            _strReportName = udtReportModel.ReportName
            _strReportDesc = udtReportModel.ReportDesc
            _strExecSP = udtReportModel.ExecSP
            _strTemplateName = udtReportModel.TemplateName
            'CRE13-016 Upgrade to excel 2007 [Start][Karl]
            _strReportFileExt = udtReportModel.ReportFileExt
            'CRE13-016 Upgrade to excel 2007 [End][Karl]
            _intStartRow = udtReportModel.StartRowNo
            _strFileNameFormat = udtReportModel.FileNameFormat
            _intMinusDateForFileName = udtReportModel.MinusDateForFileName
            _blnAuto = udtReportModel.AutoGenerate
            _strDBFlag = udtReportModel.DBFlag
        End Sub

    End Class
End Namespace

