Imports System.Data.SqlClient
Imports Common.Component.StaticData

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

Namespace Component.Professional
    <Serializable()> Public Class ProfessionalModel
        Private _strSPID As String
        Private _strEnrolRefNo As String
        Private _intProfessionalSeq As Integer
        Private _strServiceCategoryCode As String
        Private _strServiceCategoryDesc As String
        Private _strServiceCategoryDescChi As String
        Private _strRegistrationCode As String
        Private _strRecordStatus As String
        Private _dtmCreateDate As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _ObjProfession As ProfessionModel = Nothing

        Public Const ProfessionalSeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const ProfessionalSeqDataSize As Integer = 2

        Public Const ServiceCategoryCodeDataType As SqlDbType = SqlDbType.Char
        Public Const ServiceCategoryCodeDataSize As Integer = 5

        Public Const RegistrationCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const RegistrationCodeDataSize As Integer = 15

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property EnrolRefNo() As String
            Get
                Return _strEnrolRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolRefNo = value
            End Set
        End Property

        Public Property ProfessionalSeq() As Integer
            Get
                Return _intProfessionalSeq
            End Get
            Set(ByVal value As Integer)
                _intProfessionalSeq = value
            End Set
        End Property

        Public Property ServiceCategoryCode() As String
            Get
                Return _strServiceCategoryCode
            End Get
            Set(ByVal value As String)
                _strServiceCategoryCode = value

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------
                _strServiceCategoryDesc = Profession.ServiceCategoryDesc
                _strServiceCategoryDescChi = Profession.ServiceCategoryDescChi

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            End Set
        End Property


        Public ReadOnly Property Profession() As ProfessionModel
            Get
                If _ObjProfession Is Nothing Then
                    _ObjProfession = ProfessionBLL.GetProfessionListByServiceCategoryCode(Me.ServiceCategoryCode)
                End If
                Return _ObjProfession
            End Get
        End Property


        Public Property ServiceCategoryDesc() As String
            Get
                Return _strServiceCategoryDesc
            End Get
            Set(ByVal value As String)
                _strServiceCategoryDesc = value
            End Set
        End Property

        Public Property ServiceCategoryDescChi() As String
            Get
                Return _strServiceCategoryDescChi
            End Get
            Set(ByVal value As String)
                _strServiceCategoryDescChi = value
            End Set
        End Property

        Public Property RegistrationCode() As String
            Get
                Return _strRegistrationCode
            End Get
            Set(ByVal value As String)
                _strRegistrationCode = UCase(value).Trim
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property

        Public Property CreateDate() As Nullable(Of DateTime)
            Get
                Return _dtmCreateDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmCreateDate = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Sub New()

        End Sub


        Public Sub New(ByVal udtProfessionalModel As ProfessionalModel)
            _strSPID = udtProfessionalModel.SPID
            _strEnrolRefNo = udtProfessionalModel.EnrolRefNo
            _intProfessionalSeq = udtProfessionalModel.ProfessionalSeq
            ServiceCategoryCode = udtProfessionalModel.ServiceCategoryCode
            _strRegistrationCode = udtProfessionalModel.RegistrationCode
            _strRecordStatus = udtProfessionalModel.RecordStatus
            _dtmCreateDate = udtProfessionalModel.CreateDate
            _strCreateBy = udtProfessionalModel.CreateBy
        End Sub

        Public Sub New(ByVal strSPID As String, ByVal strEnrolRefNo As String, ByVal intProfessionalSeq As Integer, _
                        ByVal strServiceCategoryCode As String, ByVal strRegistrationCode As String, _
                        ByVal strRecordStatus As String, ByVal dtmCreateDate As Nullable(Of DateTime), ByVal strCreateBy As String)
            _strSPID = strSPID
            _strEnrolRefNo = strEnrolRefNo
            _intProfessionalSeq = intProfessionalSeq
            ServiceCategoryCode = strServiceCategoryCode
            _strRegistrationCode = strRegistrationCode
            _strRecordStatus = strRecordStatus
            _dtmCreateDate = dtmCreateDate
            _strCreateBy = strCreateBy
        End Sub

    End Class

End Namespace

