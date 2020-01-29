Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.HATransaction
    <Serializable()> _
    Public Class VaccineCodeMappingModel

#Region "Class"

        Public Class SourceSystemClass
            Public Const EHS As String = "EHS"
            Public Const CMS As String = "CMS"
        End Class

        Public Class TargetSystemClass
            Public Const EHS As String = "EHS"
            Public Const CMS As String = "CMS"
        End Class

        Public Class YesNoClass
            Public Const Y As String = "Y"
            Public Const N As String = "N"
        End Class
#End Region

        Private _strSourceSystem As String
        Public ReadOnly Property SourceSystem() As String
            Get
                Return _strSourceSystem
            End Get
        End Property

        Private _strTargetSystem As String
        Public ReadOnly Property TargetSystem() As String
            Get
                Return _strTargetSystem
            End Get
        End Property

        Private _strVaccineCodeSource As String
        Public ReadOnly Property VaccineCodeSource() As String
            Get
                Return _strVaccineCodeSource
            End Get
        End Property

        Private _strVaccineCodeTarget As String
        Public ReadOnly Property VaccineCodeTarget() As String
            Get
                Return _strVaccineCodeTarget
            End Get
        End Property

        Private _strVaccineCodeCommon As String
        Public ReadOnly Property VaccineCodeCommon() As String
            Get
                Return _strVaccineCodeCommon
            End Get
        End Property

        Private _strVaccineCodeDesc As String
        Public ReadOnly Property VaccineCodeDesc() As String
            Get
                Return _strVaccineCodeDesc
            End Get
        End Property

        Private _strVaccineCodeDescChinese As String
        Public ReadOnly Property VaccineCodeDescChinese() As String
            Get
                Return _strVaccineCodeDescChinese
            End Get
        End Property

        Private _bForEnquiry As String
        Public ReadOnly Property ForEnquiry() As Boolean
            Get
                Return _bForEnquiry
            End Get
        End Property

        Private _bForBar As String
        Public ReadOnly Property ForBar() As Boolean
            Get
                Return _bForBar
            End Get
        End Property

        Private _bForDisplay As String
        Public ReadOnly Property ForDisplay() As Boolean
            Get
                Return _bForDisplay
            End Get
        End Property

        Public Sub New(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
                        ByVal strVaccineCodeSource As String, ByVal strVaccineCodeTarget As String, ByVal strVaccineCodeCommon As String, _
                        ByVal strVaccineCodeDesc As String, ByVal strVaccineCodeDescChinese As String, _
                        ByVal strForEnquiry As String, ByVal strForBar As String, ByVal strForDisplay As String)
            _strSourceSystem = strSourceSystem
            _strTargetSystem = strTargetSystem
            _strVaccineCodeSource = strVaccineCodeSource
            _strVaccineCodeTarget = strVaccineCodeTarget
            _strVaccineCodeCommon = strVaccineCodeCommon
            _strVaccineCodeDesc = strVaccineCodeDesc
            _strVaccineCodeDescChinese = strVaccineCodeDescChinese
            _bForEnquiry = IIf(strForEnquiry = YesNoClass.Y, True, False)
            _bForBar = IIf(strForBar = YesNoClass.Y, True, False)
            _bForDisplay = IIf(strForDisplay = YesNoClass.Y, True, False)
        End Sub
    End Class
End Namespace