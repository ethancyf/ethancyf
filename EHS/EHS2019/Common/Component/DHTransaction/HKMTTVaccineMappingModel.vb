Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.DHTransaction
    <Serializable()> _
    Public Class HKMTTVaccineMappingModel

#Region "Class"

        Public Class SourceSystemClass
            Public Const CIMS As String = "CIMS"
        End Class

        Public Class TargetSystemClass
            Public Const EHS As String = "EHS"
        End Class

        Public Enum EnumVaccineType
            INFLUENZA
            PNEUMOCOCCAL
            MEASLES
        End Enum

#End Region

        Private _strSourceSystem As String
        Private _strTargetSystem As String
        Private _strVaccineType As String
        Private _strVaccineIdentifierType As String
        Private _strL3VaccineHKReqNoSource As String
        Private _strL3VaccineProductNameSource As String
        Private _strL2VaccineDescSource As String
        Private _strVaccineTypeTarget As String

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

        ''' <summary>
        ''' DH vaccine type
        ''' </summary>
        ''' <value>INFLUENZA or PNEUMOCOCCAL or MEASLES</value>
        ''' <returns>INFLUENZA or PNEUMOCOCCAL or MEASLES</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VaccineType() As EnumVaccineType
            Get
                Select Case _strVaccineType
                    Case EnumVaccineType.INFLUENZA.ToString
                        Return EnumVaccineType.INFLUENZA
                    Case EnumVaccineType.PNEUMOCOCCAL.ToString
                        Return EnumVaccineType.PNEUMOCOCCAL
                    Case EnumVaccineType.MEASLES.ToString
                        Return EnumVaccineType.MEASLES
                    Case Else
                        Throw New Exception(String.Format("Unknown HKMTTVaccineMappingModel.VaccineType ({0})", _strVaccineType))
                End Select
            End Get
        End Property

        ''' <summary>
        ''' DH vaccine identifier type according to HKMTT
        ''' </summary>
        ''' <value>L2 or L3</value>
        ''' <returns>L2 or L3</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VaccineIdentifierType() As String
            Get
                Return _strVaccineIdentifierType
            End Get
        End Property

        ''' <summary>
        ''' HKMTT L3 registration number
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property L3VaccineHKReqNoSource() As String
            Get
                Return _strL3VaccineHKReqNoSource
            End Get
        End Property

        ''' <summary>
        ''' HKMTT L3 product name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property L3VaccineProductNameSource() As String
            Get
                Return _strL3VaccineProductNameSource
            End Get
        End Property

        ''' <summary>
        ''' DH L2 vaccine description
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property L2VaccineDescSource() As String
            Get
                Return _strL2VaccineDescSource
            End Get
        End Property

        Public ReadOnly Property VaccineTypeTarget() As String
            Get
                Return _strVaccineTypeTarget
            End Get
        End Property

        Public ReadOnly Property IsUnknownVaccine() As Boolean
            Get
                Return _strVaccineIdentifierType = String.Empty
            End Get
        End Property

        Public Sub New(ByVal strSourceSystem As String, ByVal strTargetSystem As String, _
                        ByVal strVaccineType As String, ByVal strVaccineIdentifierType As String, _
                        ByVal strL3VaccineHKReqNoSource As String, ByVal strL3VaccineProductNameSource As String, ByVal strL2VaccineDescSource As String, _
                        ByVal strVaccineTypeTarget As String)
            _strSourceSystem = strSourceSystem
            _strTargetSystem = strTargetSystem
            _strVaccineType = strVaccineType
            _strVaccineIdentifierType = strVaccineIdentifierType
            _strL3VaccineHKReqNoSource = strL3VaccineHKReqNoSource
            _strL3VaccineProductNameSource = strL3VaccineProductNameSource
            _strL2VaccineDescSource = strL2VaccineDescSource
            _strVaccineTypeTarget = strVaccineTypeTarget
        End Sub

        Public Shared Function GenerateKeyUnknownVaccine(ByVal udtDHVaccineModel As DHVaccineModel) As String
            Return GenerateKey(SourceSystemClass.CIMS, _
                                TargetSystemClass.EHS, _
                                 udtDHVaccineModel.VaccineType, _
                                 String.Empty, _
                                 String.Empty, _
                                 String.Empty)
        End Function

        ''' <summary>
        ''' Generate unique key of HKMTTVaccineMappingModel
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GenerateKey() As String
            Return GenerateKey(Me)
        End Function

        ''' <summary>
        ''' Generate unique key of HKMTTVaccineMappingModel
        ''' </summary>
        ''' <param name="udtHKMTTVaccineMappingModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GenerateKey(ByVal udtHKMTTVaccineMappingModel As HKMTTVaccineMappingModel) As String
            Return GenerateKey(udtHKMTTVaccineMappingModel.SourceSystem, _
                                udtHKMTTVaccineMappingModel.TargetSystem, _
                                udtHKMTTVaccineMappingModel.VaccineType.ToString, _
                                udtHKMTTVaccineMappingModel.VaccineIdentifierType, _
                                udtHKMTTVaccineMappingModel.L3VaccineHKReqNoSource, _
                                udtHKMTTVaccineMappingModel.L2VaccineDescSource)
        End Function

        ''' <summary>
        ''' Generate unique key of HKMTTVaccineMappingModel
        ''' </summary>
        ''' <param name="strSourceSystem"></param>
        ''' <param name="strTargetSystem"></param>
        ''' <param name="strVaccineType"></param>
        ''' <param name="strVaccineIdentifierType"></param>
        ''' <param name="strL3VaccineHKReqNoSource"></param>
        ''' <param name="strL2VaccineDescSource"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GenerateKey(ByVal strSourceSystem As String, _
                                     ByVal strTargetSystem As String, _
                                     ByVal strVaccineType As String, _
                                     ByVal strVaccineIdentifierType As String, _
                                     ByVal strL3VaccineHKReqNoSource As String, _
                                     ByVal strL2VaccineDescSource As String _
                                     ) As String
            Return String.Format("{0}|{1}|{2}|{3}|{4}|{5}", _
                                 strSourceSystem.ToUpper(), _
                                 strTargetSystem.ToUpper(), _
                                 strVaccineType.ToUpper(), _
                                 strVaccineIdentifierType.ToUpper(), _
                                 strL3VaccineHKReqNoSource.ToUpper(), _
                                 strL2VaccineDescSource.ToUpper())
        End Function

        Public Shared Function GenerateKey(ByVal udtDHVaccineModel As DHVaccineModel) As String
            Select Case udtDHVaccineModel.VaccineIdenType.ToUpper()
                Case DHVaccineModel.VaccineIdenifierType.L3
                    Return GenerateKey(SourceSystemClass.CIMS, _
                                TargetSystemClass.EHS, _
                                udtDHVaccineModel.VaccineType, _
                                udtDHVaccineModel.VaccineIdenType, _
                                udtDHVaccineModel.VaccineL3Iden.HkRegNum, _
                                String.Empty)
                Case DHVaccineModel.VaccineIdenifierType.L2
                    Return GenerateKey(SourceSystemClass.CIMS, _
                               TargetSystemClass.EHS, _
                               udtDHVaccineModel.VaccineType, _
                               udtDHVaccineModel.VaccineIdenType, _
                               String.Empty, _
                               udtDHVaccineModel.VaccineL2Iden.VaccineDesc)
                Case Else
                    Throw New Exception(String.Format("Unknown VaccineIdenType {0}", udtDHVaccineModel.VaccineIdenType))
            End Select
        End Function

        Public Function GenerateKeyBySystem() As String
            Return GenerateKeyBySystem(Me.SourceSystem, _
                                Me.TargetSystem)
        End Function

        Public Shared Function GenerateKeyBySystem(ByVal udtHKMTTVaccineMappingModel As HKMTTVaccineMappingModel) As String
            Return GenerateKeyBySystem(udtHKMTTVaccineMappingModel.SourceSystem, _
                                udtHKMTTVaccineMappingModel.TargetSystem)
        End Function

        Public Shared Function GenerateKeyBySystem(ByVal strSourceSystem As String, ByVal strTargetSystem As String) As String
            Return strSourceSystem + "|" + strTargetSystem
        End Function
    End Class
End Namespace