Imports Microsoft.VisualBasic
Imports System.Globalization
Imports Common.WebService.Interface.DHVaccineResult

Namespace Component.DHTransaction
    <Serializable()> _
    Public Class DHClientModel

#Region "Constants"
        Public Enum ReturnCode
            ''' <summary>
            ''' Success
            ''' </summary>
            ''' <remarks></remarks>
            Success = 20000

            ''' <summary>
            ''' Client not found for the Document Number and Type.
            ''' </summary>
            ''' <remarks></remarks>
            ClientNotFound = 20001

            ''' <summary>
            ''' Client found for the Document Number and Type but demographics not matched.
            ''' </summary>
            ''' <remarks></remarks>
            ClientFoundDemographicNotMatch = 20002

            ''' <summary>
            ''' Incomplete client core fields
            ''' </summary>
            ''' <remarks></remarks>
            IncompleteClientFields = 20003

            ''' <summary>
            ''' Invalid Sex, {}
            ''' </summary>
            ''' <remarks></remarks>
            InvalidSex = 20004

            ''' <summary>
            ''' Invalid DOB indicator, {}
            ''' </summary>
            ''' <remarks></remarks>
            InvalidDOBInd = 20005

            ''' <summary>
            ''' Invalid Identity Document Type, {}
            ''' </summary>
            ''' <remarks></remarks>
            InvalidDocType = 20006

            ''' <summary>
            ''' Invalid Checksum for the Document number of ID/BC Document Type.
            ''' </summary>
            ''' <remarks></remarks>
            InvalidChecksum = 20007

            ''' <summary>
            ''' Unmatched DOB format with DOB indicator
            ''' </summary>
            ''' <remarks></remarks>
            UnmatchedDOBFormatDOBInd = 20008

            ''' <summary>
            ''' [EHSS Error Code] Unexpected Error
            ''' </summary>
            ''' <remarks></remarks>
            UnexpectedError = 99999

        End Enum

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Enum ReturnCIMSCode
            ''' <summary>
            ''' All submitted demographic matched - Full record returned
            ''' </summary>
            ''' <remarks></remarks>
            AllDemographicMatch_FullRecord = 30100

            ''' <summary>
            ''' All submitted demographic matched - Partial record returned
            ''' </summary>
            ''' <remarks></remarks>
            AllDemographicMatch_PartialRecord = 30101

            ''' <summary>
            ''' All submitted demographic matched - No record returned
            ''' </summary>
            ''' <remarks></remarks>
            AllDemographicMatch_NoRecord = 30102

            ''' <summary>
            ''' Client not found for the Document Number and Type
            ''' </summary>
            ''' <remarks></remarks>
            ClientNotFound = 30103

            ''' <summary>
            ''' Client found for the Document Number and Type but demographics not matched
            ''' </summary>
            ''' <remarks></remarks>
            ClientFoundDemographicNotMatch = 30104

        End Enum

        Public Enum ReturnEHSSCode
            ''' <summary>
            ''' All submitted demographic matched - Full record returned
            ''' </summary>
            ''' <remarks></remarks>
            AllDemographicMatch_FullRecord = 30200

            ''' <summary>
            ''' All submitted demographic matched - Partial record returned
            ''' </summary>
            ''' <remarks></remarks>
            AllDemographicMatch_PartialRecord = 30201

            ''' <summary>
            ''' All submitted demographic matched - No record returned
            ''' </summary>
            ''' <remarks></remarks>
            AllDemographicMatch_NoRecord = 30202

            ''' <summary>
            ''' Patient not found
            ''' </summary>
            ''' <remarks></remarks>
            PatientNotFound = 30203

            ''' <summary>
            ''' Patient found but demographic does not match
            ''' </summary>
            ''' <remarks></remarks>
            PatientFoundDemographicNotMatch = 30204

            ''' <summary>
            ''' Document Type Not Supported
            ''' </summary>
            ''' <remarks></remarks>
            DocumentTypeNotSupported = 30205

            ''' <summary>
            ''' Service Unavailable
            ''' </summary>
            ''' <remarks></remarks>
            ServiceUnavailable = 30206

        End Enum

        Public Enum ReturnHACMSCode
            ''' <summary>
            ''' "document_no" found, and all demographic matched - Full record returned
            ''' </summary>
            ''' <remarks></remarks>
            DocumentNoFound_AllDemographicMatch_FullRecord = 30300

            '' ''' <summary>
            '' ''' "document_no" found, and all demographic matched - Partial record returned
            '' ''' </summary>
            '' ''' <remarks></remarks>
            ''DocumentNoFound_AllDemographicMatch_PartialRecord = 30301

            ''' <summary>
            ''' "document_no" found, and all demographic matched - No record returned
            ''' </summary>
            ''' <remarks></remarks>
            DocumentNoFound_AllDemographicMatch_NoRecord = 30302

            ''' <summary>
            ''' "document_no" not found
            ''' </summary>
            ''' <remarks></remarks>
            DocumentNoNotFound = 30303

            ''' <summary>
            ''' Patient found but demographic does not match
            ''' </summary>
            ''' <remarks></remarks>
            DocumentNoFoundDemographicNotMatch = 30304

            ''' <summary>
            ''' Document Type Not Supported
            ''' </summary>
            ''' <remarks></remarks>
            DocumentTypeNotSupported = 30305
            ''' <summary>
            ''' Service Unavailable
            ''' </summary>
            ''' <remarks></remarks>
            ServiceUnavailable = 30306
        End Enum
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

#End Region

#Region "Private Members"
        Private _strEngName As String
        Private _strDOB As String
        Private _strDOBInd As String
        Private _strSex As String
        Private _strDocType As String
        Private _strDocNum As String
        Private _enumClientReturnCode As ReturnCode
        Private _strReturnCodeDesc As String
        Private _enumClientReturnCIMSCode As Nullable(Of ReturnCIMSCode)
        Private _enumClientReturnEHSSCode As Nullable(Of ReturnEHSSCode)
        Private _enumClientReturnHACMSCode As Nullable(Of ReturnHACMSCode)
        Private _intReturnRecordCnt As Integer
        Private _udtVaccineList As New DHVaccineModelCollection

#End Region

#Region "Properties"
        ''' <summary>
        ''' EngName retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property EngName()
            Get
                Return _strEngName
            End Get
        End Property

        ''' <summary>
        ''' Dob retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DOB()
            Get
                Return _strDob
            End Get
        End Property

        ''' <summary>
        ''' Dob Indicator retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DobInd()
            Get
                Return _strDOBInd
            End Get
        End Property

        ''' <summary>
        ''' Sex retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Sex()
            Get
                Return _strSex
            End Get
        End Property

        ''' <summary>
        ''' Document Type retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DocType()
            Get
                Return _strDOBInd
            End Get
        End Property

        ''' <summary>
        ''' Document Number retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DocNum()
            Get
                Return _strDocNum
            End Get
        End Property

        ''' <summary>
        ''' Return Code retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnClientCode() As ReturnCode
            Get
                Return _enumClientReturnCode
            End Get
        End Property

        ''' <summary>
        ''' Return Code Desc retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnCodeDesc()
            Get
                Return _strReturnCodeDesc
            End Get
        End Property

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Return CIMS Code retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnClientCIMSCode() As Nullable(Of ReturnCIMSCode)
            Get
                Return _enumClientReturnCIMSCode
            End Get
        End Property
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Return EHSS Code retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnClientEHSSCode() As Nullable(Of ReturnEHSSCode)
            Get
                Return _enumClientReturnEHSSCode
            End Get
        End Property
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Return HACMS Code retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnClientHACMSCode() As Nullable(Of ReturnHACMSCode)
            Get
                Return _enumClientReturnHACMSCode
            End Get
        End Property
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        ''' <summary>
        ''' Vaccine Record Count retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnRecordCnt() As Integer
            Get
                Return _intReturnRecordCnt
            End Get
        End Property

        ''' <summary>
        ''' Vaccination records retrieve from CIMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VaccineRecordList() As Component.DHTransaction.DHVaccineModelCollection
            Get
                Return _udtVaccineList
            End Get
            Set(ByVal value As Component.DHTransaction.DHVaccineModelCollection)
                _udtVaccineList = value
            End Set
        End Property

#End Region

#Region "Constructor"
        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Sub New(ByVal strEngName As String, _
                        ByVal strDob As String, _
                        ByVal strDobInd As String, _
                        ByVal strSex As String, _
                        ByVal strDocType As String, _
                        ByVal strDocNum As String, _
                        ByVal enumClientReturnCode As ReturnCode, _
                        ByVal strReturnCodeDesc As String, _
                        ByVal enumClientReturnCIMSCode As Nullable(Of ReturnCIMSCode), _
                        ByVal enumClientReturnEHSSCode As Nullable(Of ReturnEHSSCode), _
                        ByVal enumClientReturnHACMSCode As Nullable(Of ReturnHACMSCode), _
                        ByVal intReturnRecordCnt As Integer)

            _strEngName = strEngName
            _strDOB = strDob
            _strDOBInd = strDobInd
            _strSex = strSex
            _strDocType = strDocType
            _strDocNum = strDocNum
            _enumClientReturnCode = enumClientReturnCode

            If String.IsNullOrEmpty(strReturnCodeDesc) Then
                _strReturnCodeDesc = _enumClientReturnCode.ToString
            Else
                _strReturnCodeDesc = strReturnCodeDesc
            End If

            _enumClientReturnCIMSCode = enumClientReturnCIMSCode
            _enumClientReturnEHSSCode = enumClientReturnEHSSCode
            _enumClientReturnHACMSCode = enumClientReturnHACMSCode

            _intReturnRecordCnt = intReturnRecordCnt
        End Sub
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

        Public Sub New(ByVal enumClientReturnCode As ReturnCode, ByVal intReturnRecordCnt As Integer)
            _enumClientReturnCode = enumClientReturnCode
            _strReturnCodeDesc = _enumClientReturnCode.ToString
            _intReturnRecordCnt = intReturnRecordCnt
        End Sub

#End Region

#Region "Supported Functions"
        ' CRE19-007 (DH CIMS Sub return code) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function Copy() As DHClientModel
            Dim udtClient As DHClientModel = New DHClientModel(Me.EngName, _
                                                               Me.DOB, _
                                                               Me.DobInd, _
                                                               Me.Sex, _
                                                               Me.DocType, _
                                                               Me.DocNum, _
                                                               Me.ReturnClientCode, _
                                                               Me.ReturnCodeDesc, _
                                                               Me.ReturnClientCIMSCode, _
                                                               Me.ReturnClientEHSSCode, _
                                                               Me.ReturnClientHACMSCode, _
                                                               Me.ReturnRecordCnt)

            Dim udtDHVaccineRecordList As New DHVaccineModelCollection

            For Each udtVaccineRecord As DHVaccineModel In Me.VaccineRecordList
                udtDHVaccineRecordList.Add(udtVaccineRecord.Copy())
            Next

            udtClient.VaccineRecordList = udtDHVaccineRecordList

            Return udtClient

        End Function
        ' CRE19-007 (DH CIMS Sub return code) [End][Chris YIM]

#End Region

    End Class

End Namespace