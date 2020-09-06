Imports Common.Component
Imports Common.Component.EHSAccount.EHSAccountModel

Namespace BLL

    Public Class StudentModel
        ' StudentFileHeader Information
        Private _strStudentFileID As String
        Private _dtmServiceReceviceDate As DateTime
        Private _strSchoolCode As String

        Private _strSP_ID As String
        Private _intPractice_Display_Seq As String
        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strDose As String
        Private _strClaimUploadBy As String

        ' StudentFileEntry Information
        Private _intStudentSeq As Integer
        Private _strClassName As String
        Private _strAccProcessStage As String
        Private _strVaccinationProcessStage As String
        Private _blnEntitleOnlyDose As Boolean
        Private _blnEntitle1stDose As Boolean
        Private _blnEntitle2ndDose As Boolean
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _blnEntitle3rdDose As Boolean
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]
        Private _blnEntitleInject As Boolean
        Private _blnInjected As Boolean
        Private _strHAVaccineRefStatus As String
        Private _strDHVaccineRefStatus As String
        Private _enumAccountSource As SysAccountSource
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _strHKICSymbol As String
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        ' Account Information
        Private _strAccRecordStatus As String
        Private _udtPersonalInformation As EHSPersonalInformationModel

        Public ReadOnly Property StudentFileID As String
            Get
                Return _strStudentFileID
            End Get
        End Property

        Public ReadOnly Property SchoolCode As String
            Get
                Return _strSchoolCode
            End Get
        End Property

        Public ReadOnly Property ServiceProviderID() As String
            Get
                Return Me._strSP_ID
            End Get
        End Property

        Public ReadOnly Property PracticeID() As Integer
            Get
                Return Me._intPractice_Display_Seq
            End Get
        End Property

        Public ReadOnly Property ServiceReceviceDate() As DateTime
            Get
                Return Me._dtmServiceReceviceDate
            End Get
        End Property

        Public ReadOnly Property SchemeCode As String
            Get
                Return _strSchemeCode
            End Get
        End Property

        Public ReadOnly Property SchemeSeq As Integer
            Get
                Return _intSchemeSeq
            End Get
        End Property

        Public ReadOnly Property Dose As String
            Get
                Return _strDose
            End Get
        End Property

        Public ReadOnly Property ClaimUploadBy As String
            Get
                Return _strClaimUploadBy
            End Get
        End Property

        Public ReadOnly Property StudentSeq As Integer
            Get
                Return _intStudentSeq
            End Get
        End Property

        Public ReadOnly Property ClassName As String
            Get
                Return _strClassName
            End Get
        End Property

        Public ReadOnly Property AccProcessStage As String
            Get
                Return _strAccProcessStage
            End Get
        End Property

        Public ReadOnly Property VaccinationProcessStage As String
            Get
                Return _strVaccinationProcessStage
            End Get
        End Property

        Public ReadOnly Property EntitleOnlyDose As Boolean
            Get
                Return _blnEntitleOnlyDose
            End Get
        End Property

        Public ReadOnly Property Entitle1stDose As Boolean
            Get
                Return _blnEntitle1stDose
            End Get
        End Property

        Public ReadOnly Property Entitle2ndDose As Boolean
            Get
                Return _blnEntitle2ndDose
            End Get
        End Property

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property Entitle3rdDose As Boolean
            Get
                Return _blnEntitle3rdDose
            End Get
        End Property
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        Public ReadOnly Property HAVaccineRefStatus As String
            Get
                Return _strHAVaccineRefStatus
            End Get
        End Property

        Public ReadOnly Property EntitleInject As Boolean
            Get
                Return _blnEntitleInject
            End Get
        End Property

        Public ReadOnly Property Injected As Boolean
            Get
                Return _blnInjected
            End Get
        End Property

        Public ReadOnly Property DHVaccineRefStatus As String
            Get
                Return _strDHVaccineRefStatus
            End Get
        End Property

        Public ReadOnly Property AccountSource As SysAccountSource
            Get
                Return _enumAccountSource
            End Get
        End Property

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public ReadOnly Property HKICSymbol As String
            Get
                Return _strHKICSymbol
            End Get
        End Property
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        Public ReadOnly Property AccRecordStatus As String
            Get
                Return _strAccRecordStatus
            End Get
        End Property

        Public ReadOnly Property PersonalInformation As EHSPersonalInformationModel
            Get
                Return _udtPersonalInformation
            End Get
        End Property

        Public Sub New(ByVal strStudentFileID As String, _
                        ByVal strSchoolCode As String, _
                        ByVal strSP_ID As String, _
                        ByVal intPractice_Display_Seq As Integer, _
                        ByVal dtmServiceReceviceDate As DateTime, _
                        ByVal strSchemeCode As String, _
                        ByVal intSchemeSeq As Integer, _
                        ByVal strDose As String, _
                        ByVal strClaimUploadBy As String, _
                        ByVal intStudentSeq As Integer, _
                        ByVal strClassName As String, _
                        ByVal strAccProcessStage As String, _
                        ByVal strVaccinationProcessStage As String, _
                        ByVal strEntitleOnlyDose As String, _
                        ByVal strEntitle1stDose As String, _
                        ByVal strEntitle2ndDose As String, _
                        ByVal strEntitle3rdDose As String, _
                        ByVal strEntitleInject As String, _
                        ByVal strInjected As String, _
                        ByVal strHAVaccineRefStatus As String, _
                        ByVal strDHVaccineRefStatus As String, _
                        ByVal strAccountSource As String, _
                        ByVal strHKICSymbol As String, _
                        ByVal strAccRecordStatus As String, _
                        ByVal udtPersonalInformation As EHSPersonalInformationModel)
            _strStudentFileID = strStudentFileID
            _strSchoolCode = strSchoolCode
            _dtmServiceReceviceDate = dtmServiceReceviceDate
            _strSP_ID = strSP_ID
            _intPractice_Display_Seq = intPractice_Display_Seq
            _strSchemeCode = strSchemeCode
            _intSchemeSeq = intSchemeSeq
            _strDose = strDose
            _strClaimUploadBy = strClaimUploadBy
            _intStudentSeq = intStudentSeq
            _strClassName = strClassName
            _strAccProcessStage = strAccProcessStage
            _strVaccinationProcessStage = strVaccinationProcessStage

            _blnEntitleOnlyDose = (strEntitleOnlyDose = YesNo.Yes)
            _blnEntitle1stDose = (strEntitle1stDose = YesNo.Yes)
            _blnEntitle2ndDose = (strEntitle2ndDose = YesNo.Yes)
            _blnEntitle3rdDose = (strEntitle3rdDose = YesNo.Yes)
            _blnEntitleInject = (strEntitleInject = YesNo.Yes)
            _blnInjected = (strInjected = YesNo.Yes)

            _strHAVaccineRefStatus = strHAVaccineRefStatus
            _strDHVaccineRefStatus = strDHVaccineRefStatus

            Select Case strAccountSource
                Case SysAccountSourceClass.ValidateAccount
                    _enumAccountSource = SysAccountSource.ValidateAccount
                Case SysAccountSourceClass.TemporaryAccount
                    _enumAccountSource = SysAccountSource.TemporaryAccount
                Case Else
                    _enumAccountSource = SysAccountSource.InvalidAccount
                    'Throw New Exception(String.Format("Invalid [StudentFileEntryStaging].[Acc_Type]({0})", strAccountSource))
            End Select

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            _strHKICSymbol = strHKICSymbol
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            _strAccRecordStatus = strAccRecordStatus
            _udtPersonalInformation = udtPersonalInformation
        End Sub

    End Class
End Namespace
