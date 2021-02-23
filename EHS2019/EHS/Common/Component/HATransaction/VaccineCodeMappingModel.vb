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

        Private _blnForEnquiry As Boolean
        Public ReadOnly Property ForEnquiry() As Boolean
            Get
                Return _blnForEnquiry
            End Get
        End Property

        Private _blnForBar As Boolean
        Public ReadOnly Property ForBar() As Boolean
            Get
                Return _blnForBar
            End Get
        End Property

        Private _blnForDisplay As Boolean
        Public ReadOnly Property ForDisplay() As Boolean
            Get
                Return _blnForDisplay
            End Get
        End Property

        Private _strVaccineBrandIDSource As String
        Public ReadOnly Property VaccineBrandIDSource() As String
            Get
                Return _strVaccineBrandIDSource
            End Get
        End Property

        Private _strVaccineBrandIDTarget As String
        Public ReadOnly Property VaccineBrandIDTarget() As String
            Get
                Return _strVaccineBrandIDTarget
            End Get
        End Property

        Public Sub New(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
                        ByVal strVaccineCodeSource As String, ByVal strVaccineCodeTarget As String, ByVal strVaccineCodeCommon As String, _
                        ByVal strVaccineCodeDesc As String, ByVal strVaccineCodeDescChinese As String, _
                        ByVal strForEnquiry As String, ByVal strForBar As String, ByVal strForDisplay As String, _
                        ByVal strVaccineBrandIDSource As String, ByVal strVaccineBrandIDTarget As String)
            _strSourceSystem = strSourceSystem
            _strTargetSystem = strTargetSystem
            _strVaccineCodeSource = strVaccineCodeSource
            _strVaccineCodeTarget = strVaccineCodeTarget
            _strVaccineCodeCommon = strVaccineCodeCommon
            _strVaccineCodeDesc = strVaccineCodeDesc
            _strVaccineCodeDescChinese = strVaccineCodeDescChinese
            _blnForEnquiry = IIf(strForEnquiry = YesNoClass.Y, True, False)
            _blnForBar = IIf(strForBar = YesNoClass.Y, True, False)
            _blnForDisplay = IIf(strForDisplay = YesNoClass.Y, True, False)
            _strVaccineBrandIDSource = strVaccineBrandIDSource
            _strVaccineBrandIDTarget = strVaccineBrandIDTarget
        End Sub
    End Class
End Namespace