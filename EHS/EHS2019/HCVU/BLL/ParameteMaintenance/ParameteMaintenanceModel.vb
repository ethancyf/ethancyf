<Serializable()> _
Public Class ParameteMaintenanceModel

#Region "Constructors"

    Public Sub New()

    End Sub

#End Region

#Region "Fields and Properties"

    Private _strCategory As String
    Private _strParameterID As String
    Private _strParameterDescription As String
    Private _strParameterValue1 As String
    Private _strApplyLimit As String
    Private _strExternalUse As String
    Private _TSMP As Byte()
    Private _strUpperLimit As Integer
    Private _strLowerLimit As Integer

    Public Property Category() As String
        Get
            Return Me._strCategory
        End Get
        Set(ByVal value As String)
            Me._strCategory = value
        End Set
    End Property

    Public Property ParameterID() As String
        Get
            Return Me._strParameterID
        End Get
        Set(ByVal value As String)
            Me._strParameterID = value
        End Set
    End Property

    Public Property ParameterDescription() As String
        Get
            Return Me._strParameterDescription
        End Get
        Set(ByVal value As String)
            Me._strParameterDescription = value
        End Set
    End Property

    Public Property ParameterValue1() As String
        Get
            Return Me._strParameterValue1
        End Get
        Set(ByVal value As String)
            Me._strParameterValue1 = value
        End Set
    End Property

    Public Property UpperLimit() As Integer
        Get
            Return Me._strUpperLimit
        End Get
        Set(ByVal value As Integer)
            Me._strUpperLimit = value
        End Set
    End Property

    Public Property LowerLimit() As Integer
        Get
            Return Me._strLowerLimit
        End Get
        Set(ByVal value As Integer)
            Me._strLowerLimit = value
        End Set
    End Property

    Public Property ApplyLimit() As String
        Get
            Return Me._strApplyLimit
        End Get
        Set(ByVal value As String)
            Me._strApplyLimit = value
        End Set
    End Property

    ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
    Public Property ExternalUse() As String
        Get
            Return Me._strExternalUse
        End Get
        Set(ByVal value As String)
            Me._strExternalUse = value
        End Set
    End Property
    ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

    Public Property TSMP() As Byte()
        Get
            Return _TSMP
        End Get
        Set(ByVal Value As Byte())
            _TSMP = Value
        End Set
    End Property

#End Region

#Region "Functions"

    Public Function Clone() As ParameteMaintenanceModel
        Dim udtParameterMaintenanceOut As New ParameteMaintenanceModel

        With udtParameterMaintenanceOut
            .Category = Me.Category
            .ParameterID = Me.ParameterID
            .ParameterDescription = Me.ParameterDescription
            .ParameterValue1 = Me.ParameterValue1
            .UpperLimit = Me.UpperLimit
            .LowerLimit = Me.LowerLimit
            .ApplyLimit = Me.ApplyLimit
            .ExternalUse = Me.ExternalUse
            .TSMP = Me.TSMP
        End With

        Return udtParameterMaintenanceOut

    End Function

#End Region

End Class
