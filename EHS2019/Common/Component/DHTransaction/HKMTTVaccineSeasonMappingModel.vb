Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.DHTransaction
    <Serializable()> _
    Public Class HKMTTVaccineSeasonMappingModel

#Region "Class"

        Public Class SourceSystemClass
            Public Const CIMS As String = "CIMS"
        End Class

        Public Class TargetSystemClass
            Public Const EHS As String = "EHS"
        End Class

        Public Class YesNoClass
            Public Const Y As String = "Y"
            Public Const N As String = "N"
        End Class
#End Region

        Private _strSourceSystem As String
        Private _strTargetSystem As String
        Private _strVaccineTypeSource As String
        Private _dtmInjectionDtmFromSource As DateTime
        Private _dtmInjectionDtmToSource As DateTime
        Private _strVaccineCodeTarget As String
        Private _strVaccineCodeDesc As String
        Private _strVaccineCodeDescChinese As String
        Private _bForBar As String
        Private _bForDisplay As String
        Private _strProvider As String

        Public ReadOnly Property SourceSystem() As String
            Get
                Return _strSourceSystem
            End Get
        End Property

        Public ReadOnly Property TargetSystem() As String
            Get
                Return _strTargetSystem
            End Get
        End Property

        Public ReadOnly Property VaccineTypeSource() As String
            Get
                Return _strVaccineTypeSource
            End Get
        End Property


        Public ReadOnly Property InjectionDtmFromSource() As DateTime
            Get
                Return _dtmInjectionDtmFromSource
            End Get
        End Property

        Public ReadOnly Property InjectionDtmToSource() As DateTime
            Get
                Return _dtmInjectionDtmToSource
            End Get
        End Property

        Public ReadOnly Property VaccineCodeTarget() As String
            Get
                Return _strVaccineCodeTarget
            End Get
        End Property

        Public ReadOnly Property VaccineCodeDesc() As String
            Get
                Return _strVaccineCodeDesc
            End Get
        End Property

        Public ReadOnly Property VaccineCodeDescChinese() As String
            Get
                Return _strVaccineCodeDescChinese
            End Get
        End Property

        Public ReadOnly Property ForBar() As Boolean
            Get
                Return _bForBar
            End Get
        End Property

        Public ReadOnly Property ForDisplay() As Boolean
            Get
                Return _bForDisplay
            End Get
        End Property

        Public ReadOnly Property Provider() As String
            Get
                Return _strProvider
            End Get
        End Property

        Public Sub New(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
                        ByVal strVaccineTypeSource As String, _
                        ByVal dtmInjectionDtmFromSource As DateTime, ByVal dtmInjectionDtmToSource As DateTime, _
                        ByVal strVaccineCodeTarget As String, ByVal strVaccineCodeDesc As String, ByVal strVaccineCodeDescChinese As String, _
                        ByVal strForBar As String, ByVal strForDisplay As String, ByVal strProvider As String)
            _strSourceSystem = strSourceSystem
            _strTargetSystem = strTargetSystem
            _strVaccineTypeSource = strVaccineTypeSource
            _dtmInjectionDtmFromSource = dtmInjectionDtmFromSource
            _dtmInjectionDtmToSource = dtmInjectionDtmToSource
            _strVaccineCodeTarget = strVaccineCodeTarget
            _strVaccineCodeDesc = strVaccineCodeDesc
            _strVaccineCodeDescChinese = strVaccineCodeDescChinese
            _bForBar = IIf(strForBar = YesNoClass.Y, True, False)
            _bForDisplay = IIf(strForDisplay = YesNoClass.Y, True, False)
            _strProvider = strProvider
        End Sub


        ''' <summary>
        ''' Generate unique key of HKMTTVaccineSeasonMappingModel
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GenerateKey() As String
            Return GenerateKey(Me)
        End Function

        ''' <summary>
        ''' Generate unique key of HKMTTVaccineSeasonMappingModel
        ''' </summary>
        ''' <param name="udtHKMTTVaccineSeasonMappingModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GenerateKey(ByVal udtHKMTTVaccineSeasonMappingModel As HKMTTVaccineSeasonMappingModel) As String
            Return GenerateKey(udtHKMTTVaccineSeasonMappingModel.SourceSystem, _
                                udtHKMTTVaccineSeasonMappingModel.TargetSystem, _
                                udtHKMTTVaccineSeasonMappingModel.VaccineTypeSource.Trim, _
                                udtHKMTTVaccineSeasonMappingModel.VaccineCodeTarget.Trim)
        End Function

        ''' <summary>
        ''' Generate unique key of HKMTTVaccineSeasonMappingModel
        ''' </summary>
        ''' <param name="strSourceSystem"></param>
        ''' <param name="strTargetSystem"></param>
        ''' <param name="strVaccineTypeSource"></param>
        ''' <param name="strVaccineCodeTarget"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GenerateKey(ByVal strSourceSystem As String, _
                                     ByVal strTargetSystem As String, _
                                     ByVal strVaccineTypeSource As String, _
                                     ByVal strVaccineCodeTarget As String _
                                     ) As String
            Return String.Format("{0}|{1}|{2}|{3}", _
                                 strSourceSystem.ToUpper(), _
                                 strTargetSystem.ToUpper(), _
                                 strVaccineTypeSource.ToUpper(), _
                                 strVaccineCodeTarget.ToUpper())
        End Function

        Public Function GenerateKeyBySystem() As String
            Return GenerateKeyBySystem(Me.SourceSystem, _
                                Me.TargetSystem,
                                Me.VaccineTypeSource)
        End Function

        Public Shared Function GenerateKeyBySystem(ByVal udtHKMTTVaccineSeasonMappingModel As HKMTTVaccineSeasonMappingModel) As String
            Return GenerateKeyBySystem(udtHKMTTVaccineSeasonMappingModel.SourceSystem, _
                                udtHKMTTVaccineSeasonMappingModel.TargetSystem,
                                udtHKMTTVaccineSeasonMappingModel.VaccineTypeSource)
        End Function

        Public Shared Function GenerateKeyBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String, ByVal strVaccineTypeSource As String) As String
            Return strSourceSystem + "|" + strTargetSystem + "|" + strVaccineTypeSource
        End Function

    End Class
End Namespace