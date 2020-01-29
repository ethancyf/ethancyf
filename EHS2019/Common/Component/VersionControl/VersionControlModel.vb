' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.ComFunction.GeneralFunction

Namespace Component.VersionControl
    <Serializable()> Public Class VersionControlModel

        Private _strLogicalName As String
        Private _strPhysicalName As String
        Private _strDescription As String
        Private _dtmEffectiveDtm As Nullable(Of DateTime)

        Public Const Logical_Name As String = "Logical_Name"
        Public Const Physical_Name As String = "Physical_Name"
        Public Const Description As String = "Description"
        Public Const Effective_Dtm As String = "Effective_Dtm"

        Public Property LogicalName() As String
            Get
                Return _strLogicalName
            End Get
            Set(ByVal value As String)
                _strLogicalName = value
            End Set
        End Property

        Public Property PhysicalName() As String
            Get
                Return _strPhysicalName
            End Get
            Set(ByVal value As String)
                _strPhysicalName = value
            End Set
        End Property

        Public Property Desc() As String
            Get
                Return _strDescription
            End Get
            Set(ByVal value As String)
                _strDescription = value
            End Set
        End Property

        Public Property EffectiveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmEffectiveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEffectiveDtm = value
            End Set
        End Property

        Public Sub New(ByVal strLogicalName As String, ByVal strPhysicalName As String, ByVal strDescription As String, ByVal dtmEffectiveDtm As Nullable(Of DateTime))
            _strLogicalName = strLogicalName
            _strPhysicalName = strPhysicalName
            _strDescription = strDescription
            _dtmEffectiveDtm = dtmEffectiveDtm
        End Sub

        Public Sub New(ByVal udtVersionControlModel As VersionControlModel)
            _strLogicalName = udtVersionControlModel.LogicalName
            _strPhysicalName = udtVersionControlModel.PhysicalName
            _strDescription = udtVersionControlModel.Desc
            _dtmEffectiveDtm = udtVersionControlModel.EffectiveDtm
        End Sub

        Public Function IsEffective() As Boolean
            Return IsWithinPeriod(EffectiveDtm, Nothing)
        End Function

        Public Function IsEffective(ByVal dtmServiceDate As DateTime) As Boolean
            Return IsWithinPeriod(dtmServiceDate, EffectiveDtm, Nothing)
        End Function

    End Class
End Namespace

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

