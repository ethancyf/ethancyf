Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.Mapping
    <Serializable()> _
    Public Class CodeMappingModel

#Region "Class"

        Public Enum EnumSourceSystem
            EHS
            PCD
            EHRSS
        End Enum

        Public Enum EnumTargetSystem
            EHS
            PCD
            EHRSS
        End Enum

        Public Enum EnumCodeType
            Service_Category_Code
        End Enum
#End Region

        Private _enumSourceSystem As String
        Public ReadOnly Property SourceSystem() As EnumSourceSystem
            Get
                Return _enumSourceSystem
            End Get
        End Property

        Private _enumTargetSystem As String
        Public ReadOnly Property TargetSystem() As EnumTargetSystem
            Get
                Return _enumTargetSystem
            End Get
        End Property

        Private _strCodeType As String
        Public ReadOnly Property CodeType() As String
            Get
                Return _strCodeType
            End Get
        End Property

        Private _strCodeSource As String
        Public ReadOnly Property CodeSource() As String
            Get
                Return _strCodeSource
            End Get
        End Property

        Private _strCodeTarget As String
        Public ReadOnly Property CodeTarget() As String
            Get
                Return _strCodeTarget
            End Get
        End Property

        Public Sub New(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
                        ByVal strCodeType As String, ByVal strCodeSource As String, ByVal strCodeTarget As String)
            _enumSourceSystem = DirectCast([Enum].Parse(GetType(EnumSourceSystem), strSourceSystem), EnumSourceSystem)
            _enumTargetSystem = DirectCast([Enum].Parse(GetType(EnumTargetSystem), strTargetSystem), EnumTargetSystem)
            _strCodeType = strCodeType
            _strCodeSource = strCodeSource
            _strCodeTarget = strCodeTarget
        End Sub
    End Class
End Namespace