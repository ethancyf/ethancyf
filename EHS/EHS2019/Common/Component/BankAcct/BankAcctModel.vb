
' ----- Model Structure Change 07 May 2009 -----
' 1  Remove Delist Dtm
' 2  Remove Effective Dtm
' 3  Remove BR Code  
' ----- End 06 May 2009 ------------------------

'-------------- 2009-07-14 --------------'
' ------ Br_Code is reserved ------'
' 
' BrCode will be removed after Complete Data Mirgration
' The Br_Code will be used for MyProfile version 1
' The Br_Code will be searched from BankAccount.BrCode Or MO.BrCode
'----------------------------------------'

Namespace Component.BankAcct

    <Serializable()> Public Class BankAcctModel

        '-------------- 2009-07-14 --------------'
        ' ------ Br_Code is reserved ------'
        ' 
        ' BrCode will be removed after Complete Data Mirgration
        ' The Br_Code will be used for MyProfile version 1
        ' The Br_Code will be searched from BankAccount.BrCode Or MO.BrCode
        '----------------------------------------'

        Private _strBrCode As String

        Public Property BrCode() As String
            Get
                Return _strBrCode
            End Get
            Set(ByVal value As String)
                _strBrCode = value
            End Set
        End Property

        ' END ------------ 2009-07-14 -----------'
        ' ------ Br_Code is reserved ------'
        '----------------------------------------'


        Private _strEnrolRefNo As String
        Private _intDisplaySeq As Nullable(Of Integer)
        Private _intSpPracticeDisplaySeq As Nullable(Of Integer)
        Private _strSPID As String
        'Private _strBrCode As String
        Private _strBankName As String
        Private _strBranchName As String
        Private _strBankAcctOwner As String
        Private _strBankAcctNo As String
        Private _strRecordStatus As String
        Private _strRemark As String
        Private _strUnderModification As String
        Private _strSubmitMethod As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _byteTSMP As Byte()

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Private _strIsFreeTextFormat As String
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Const DisplaySeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const DisplaySeqDataSize As Integer = 2

        Public Const SpPracticeDisplaySeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const SpPracticeDisplaySeqDataSize As Integer = 2

        'Public Const BrCodeDataType As SqlDbType = SqlDbType.VarChar
        'Public Const BrCodeDataSize As Integer = 50

        Public Const BankNameDataType As SqlDbType = SqlDbType.NVarChar
        Public Const BankNameDataSize As Integer = 100

        Public Const BranchNameDataType As SqlDbType = SqlDbType.NVarChar
        Public Const BranchNameDataSize As Integer = 100

        Public Const BankAcctNoDataType As SqlDbType = SqlDbType.VarChar
        Public Const BankAcctNoDataSize As Integer = 30

        Public Const BankAcctOwnerDataType As SqlDbType = SqlDbType.NVarChar
        Public Const BankAcctOwnerDataSize As Integer = 300

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const RemarkDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarkDataSize As Integer = 255

        Public Const SubmissionMethodDataType As SqlDbType = SqlDbType.Char
        Public Const SubmissionMethodDataSize As Integer = 1

        Public Const UnderModificationDataType As SqlDbType = SqlDbType.Char
        Public Const UnderModificationDataSize As Integer = 1

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Const IsFreeTextFormatDataType As SqlDbType = SqlDbType.Char
        Public Const IsFreeTextFormatDataSize As Integer = 1
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

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

        Public Property DisplaySeq() As Nullable(Of Integer)
            Get
                Return _intDisplaySeq
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intDisplaySeq = value
            End Set
        End Property

        Public Property SpPracticeDisplaySeq() As Nullable(Of Integer)
            Get
                Return _intSpPracticeDisplaySeq
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intSpPracticeDisplaySeq = value
            End Set
        End Property

        'Public Property BrCode() As String
        '    Get
        '        Return _strBrCode
        '    End Get
        '    Set(ByVal value As String)
        '        _strBrCode = value
        '    End Set
        'End Property

        Public Property BankName() As String
            Get
                Return _strBankName
            End Get
            Set(ByVal value As String)
                _strBankName = value
            End Set
        End Property

        Public Property BranchName() As String
            Get
                Return _strBranchName
            End Get
            Set(ByVal value As String)
                _strBranchName = value
            End Set
        End Property

        Public Property BankAcctOwner() As String
            Get
                Return _strBankAcctOwner
            End Get
            Set(ByVal value As String)
                _strBankAcctOwner = value
            End Set
        End Property

        Public Property BankAcctNo() As String
            Get
                Return _strBankAcctNo
            End Get
            Set(ByVal value As String)
                _strBankAcctNo = value
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

        Public Property SubmitMethod() As String
            Get
                Return _strSubmitMethod
            End Get
            Set(ByVal value As String)
                _strSubmitMethod = value
            End Set
        End Property

        Public Property Remark() As String
            Get
                Return _strRemark
            End Get
            Set(ByVal value As String)
                _strRemark = value
            End Set
        End Property

        Public Property UnderModification() As String
            Get
                Return _strUnderModification
            End Get
            Set(ByVal value As String)
                _strUnderModification = value
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

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Property IsFreeTextFormat() As String
            Get
                Return _strIsFreeTextFormat
            End Get
            Set(ByVal value As String)
                _strIsFreeTextFormat = value
            End Set
        End Property
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtBankAcctModel As BankAcctModel)
            _strSPID = udtBankAcctModel.SPID
            _strEnrolRefNo = udtBankAcctModel.EnrolRefNo
            _intDisplaySeq = udtBankAcctModel.DisplaySeq
            _intSpPracticeDisplaySeq = udtBankAcctModel.SpPracticeDisplaySeq
            '_strBrCode = udtBankAcctModel.BrCode
            _strBankName = udtBankAcctModel.BankName
            _strBranchName = udtBankAcctModel.BranchName
            _strBankAcctOwner = udtBankAcctModel.BankAcctOwner
            _strBankAcctNo = udtBankAcctModel.BankAcctNo
            _strRecordStatus = udtBankAcctModel.RecordStatus
            _strSubmitMethod = udtBankAcctModel.SubmitMethod
            _strRemark = udtBankAcctModel.Remark
            '_strUnderModification = udtBankAcctModel.UnderModification
            _dtmCreateDtm = udtBankAcctModel.CreateDtm
            _strCreateBy = udtBankAcctModel.CreateBy
            _dtmUpdateDtm = udtBankAcctModel.UpdateDtm
            _strUpdateBy = udtBankAcctModel.UpdateBy
            _byteTSMP = udtBankAcctModel.TSMP
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            _strIsFreeTextFormat = udtBankAcctModel.IsFreeTextFormat
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End Sub

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Sub New(ByVal strSPID As String, ByVal strEnrolRefNo As String, ByVal intDisplaySeq As Nullable(Of Integer), _
                        ByVal intSpPracticeDisplaySeq As Nullable(Of Integer), ByVal strBankName As String, _
                        ByVal strBranchName As String, ByVal strBankAcctOwner As String, ByVal strBankAcctNo As String, _
                        ByVal strRecordStatus As String, ByVal strSubmitMethod As String, ByVal strRemark As String, _
                        ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
                        ByVal strUpdateBy As String, ByVal byteTSMP As Byte(), ByVal strIsFreeTextFormat As String)

            _strSPID = strSPID
            _strEnrolRefNo = strEnrolRefNo
            _intDisplaySeq = intDisplaySeq
            _intSpPracticeDisplaySeq = intSpPracticeDisplaySeq
            '_strBrCode = strBrCode
            _strBankName = strBankName
            _strBranchName = strBranchName
            _strBankAcctOwner = strBankAcctOwner
            _strBankAcctNo = strBankAcctNo
            _strRecordStatus = strRecordStatus
            _strSubmitMethod = strSubmitMethod
            _strRemark = strRemark
            '_strUnderModification = strUnderModification
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _byteTSMP = byteTSMP
            _strIsFreeTextFormat = strIsFreeTextFormat
        End Sub
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

    End Class

End Namespace
