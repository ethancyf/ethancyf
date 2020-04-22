Imports Common.Component.Address
Imports Common.Component.StaticData

' ----- Model Structure Change 06 May 2009 -----
' 1  Remove SP Practice Display Seq
' 2  Add SPID
' 3  Add Relationship
' 4  Add Relationship Remark
' 5  Add Record Status
' 6  Add TSMP
' ----- End 06 May 2009 ------------------------

Namespace Component.MedicalOrganizationT
    <Serializable()> Public Class MedicalOrganizationModel
        Private _strEnrolRefNo As String
        Private _strSPID As String
        Private _intDisplaySeq As Nullable(Of Integer)
        'Private _intSpPracticeDisplaySeq As Nullable(Of Integer)
        Private _strMOEngName As String
        Private _strMOChiName As String
        Private _Address As AddressModel
        Private _strBrCode As String
        Private _strPhoneDaytime As String
        Private _strEmail As String
        Private _strFax As String
        Private _strRelationship As String
        Private _strRelationshipDesc As String
        Private _strRelationshipChiDesc As String
        Private _strRelationshipRemark As String
        Private _strRecordStatus As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _byteTSMP As Byte()

        Private _strDisplaySeqMOName As String


        Public Const DisplaySeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const DisplaySeqDataSize As Integer = 2

        'Public Const SpPracticeDisplaySeqDataType As SqlDbType = SqlDbType.SmallInt
        'Public Const SpPracticeDisplaySeqDataSize As Integer = 2

        Public Const MOEngNameDataType As SqlDbType = SqlDbType.VarChar
        Public Const MOEngNameDataSize As Integer = 100

        Public Const MOChiNameDataType As SqlDbType = SqlDbType.NVarChar
        Public Const MOChiNameDataSize As Integer = 100

        Public Const BrCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const BrCodeDataSize As Integer = 50

        Public Const PhoneDaytimeDataType As SqlDbType = SqlDbType.VarChar
        Public Const PhoneDaytimeDataSize As Integer = 20

        Public Const EmailDataType As SqlDbType = SqlDbType.VarChar
        Public Const EmailDataSize As Integer = 255

        Public Const FaxDataType As SqlDbType = SqlDbType.VarChar
        Public Const FaxDataSize As Integer = 20

        Public Const RelationshipDataType As SqlDbType = SqlDbType.Char
        Public Const RelationshipDataSize As Integer = 5

        Public Const RelationshipRemarkDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RelationshipRemarkDataSize As Integer = 255

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        Public Property EnrolRefNo() As String
            Get
                Return _strEnrolRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolRefNo = value
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property DisplaySeq() As Nullable(Of Integer)
            Get
                Return _intDisplaySeq
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intDisplaySeq = value
            End Set
        End Property

        'Public Property SpPracticeDisplaySeq() As Nullable(Of Integer)
        '    Get
        '        Return _intSpPracticeDisplaySeq
        '    End Get
        '    Set(ByVal value As Nullable(Of Integer))
        '        _intSpPracticeDisplaySeq = value
        '    End Set
        'End Property

        Public Property Relationship() As String
            Get
                Return _strRelationship
            End Get
            Set(ByVal value As String)
                _strRelationship = value

                Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
                Dim udtStaticDataModel As StaticDataModel
                udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", _strRelationship)
                _strRelationshipDesc = udtStaticDataModel.DataValue
                _strRelationshipChiDesc = udtStaticDataModel.DataValueChi
            End Set
        End Property

        Public Property RelationshipDesc() As String
            Get
                Return _strRelationshipDesc
            End Get
            Set(ByVal value As String)
                _strRelationshipDesc = value
            End Set
        End Property

        Public Property RelationshipChiDesc() As String
            Get
                Return _strRelationshipChiDesc
            End Get
            Set(ByVal value As String)
                _strRelationshipChiDesc = value
            End Set
        End Property

        Public Property RelationshipRemark() As String
            Get
                Return _strRelationshipRemark
            End Get
            Set(ByVal value As String)
                _strRelationshipRemark = value
            End Set
        End Property

        Public Property MOEngName() As String
            Get
                Return _strMOEngName
            End Get
            Set(ByVal value As String)
                _strMOEngName = value

                _strDisplaySeqMOName = DisplaySeq.ToString + ". " + value
            End Set
        End Property

        Public Property MOChiName() As String
            Get
                Return _strMOChiName
            End Get
            Set(ByVal value As String)
                _strMOChiName = value
            End Set
        End Property

        Public Property MOAddress() As AddressModel
            Get
                Return _Address
            End Get
            Set(ByVal value As AddressModel)
                _Address = value
            End Set
        End Property

        Public Property BrCode() As String
            Get
                Return _strBrCode
            End Get
            Set(ByVal value As String)
                _strBrCode = value
            End Set
        End Property

        Public Property PhoneDaytime() As String
            Get
                Return _strPhoneDaytime
            End Get
            Set(ByVal value As String)
                _strPhoneDaytime = value
            End Set
        End Property

        Public Property Fax() As String
            Get
                Return _strFax
            End Get
            Set(ByVal value As String)
                _strFax = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _strEmail
            End Get
            Set(ByVal value As String)
                _strEmail = value
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

        Public Property CreateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmCreateDtm = value
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

        Public Property UpdateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmUpdateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Property DisplaySeqMOName() As String
            Get
                Return _strDisplaySeqMOName
            End Get
            Set(ByVal value As String)
                _strDisplaySeqMOName = value
            End Set
        End Property


        Public Sub New(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel)
            _strEnrolRefNo = udtMedicalOrganizationModel.EnrolRefNo
            _strSPID = udtMedicalOrganizationModel.SPID
            _intDisplaySeq = udtMedicalOrganizationModel.DisplaySeq
            '_intSpPracticeDisplaySeq = udtMedicalOrganizationModel.SpPracticeDisplaySeq
            '_strMOEngName = udtMedicalOrganizationModel.MOEngName
            MOEngName = udtMedicalOrganizationModel.MOEngName
            _strMOChiName = udtMedicalOrganizationModel.MOChiName
            _Address = udtMedicalOrganizationModel.MOAddress
            _strBrCode = udtMedicalOrganizationModel.BrCode
            _strPhoneDaytime = udtMedicalOrganizationModel.PhoneDaytime
            _strEmail = udtMedicalOrganizationModel.Email
            _strFax = udtMedicalOrganizationModel.Fax
            _strRelationship = udtMedicalOrganizationModel.Relationship
            _strRelationshipRemark = udtMedicalOrganizationModel.RelationshipRemark
            _strRecordStatus = udtMedicalOrganizationModel.RecordStatus
            _dtmCreateDtm = udtMedicalOrganizationModel.CreateDtm
            _strCreateBy = udtMedicalOrganizationModel.CreateBy
            _dtmUpdateDtm = udtMedicalOrganizationModel.UpdateDtm
            _strUpdateBy = udtMedicalOrganizationModel.UpdateBy
            _byteTSMP = udtMedicalOrganizationModel.TSMP
        End Sub

        Public Sub New(ByVal strEnrolRefNo As String, ByVal strSPID As String, ByVal intDisplaySeq As Nullable(Of Integer), _
                        ByVal strMOEngName As String, ByVal strMOChiName As String, _
                        ByVal MOAddress As AddressModel, ByVal strBrCode As String, _
                        ByVal strPhoneDaytime As String, ByVal strEmail As String, ByVal strFax As String, _
                        ByVal strRelationship As String, ByVal strRelationshipRemark As String, _
                        ByVal strRecordStatus As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                        ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal byteTSMP As Byte())

            _strEnrolRefNo = strEnrolRefNo
            _strSPID = strSPID
            _intDisplaySeq = intDisplaySeq
            '_strMOEngName = strMOEngName
            MOEngName = strMOEngName
            _strMOChiName = strMOChiName
            _Address = MOAddress
            _strBrCode = strBrCode
            _strPhoneDaytime = strPhoneDaytime
            _strEmail = strEmail
            _strFax = strFax
            _strRelationship = strRelationship
            _strRelationshipRemark = strRelationshipRemark
            _strRecordStatus = strRecordStatus
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _byteTSMP = byteTSMP

        End Sub

        'Public Sub New(ByVal strEnrolRefNo As String, ByVal intDisplaySeq As Nullable(Of Integer), _
        '                ByVal intSpPracticeDisplaySeq As Nullable(Of Integer), ByVal strMOEngName As String, _
        '                ByVal strMOChiName As String, ByVal MOAddress As AddressModel, ByVal strBrCode As String, _
        '                ByVal strPhoneDaytime As String, ByVal strEmail As String, ByVal strFax As String)

        '    _strEnrolRefNo = strEnrolRefNo
        '    _intDisplaySeq = intDisplaySeq
        '    _intSpPracticeDisplaySeq = intSpPracticeDisplaySeq
        '    _strMOEngName = strMOEngName
        '    _strMOChiName = strMOChiName
        '    _Address = MOAddress
        '    _strBrCode = strBrCode
        '    _strPhoneDaytime = strPhoneDaytime
        '    _strEmail = strEmail
        '    _strFax = strFax

        'End Sub
    End Class
End Namespace
