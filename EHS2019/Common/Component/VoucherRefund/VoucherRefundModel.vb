Imports Common.Component
Imports Common.Format

Namespace Component.VoucherRefund

    <Serializable()> Public Class VoucherRefundModel

#Region "DB Table Schema"
        'DB Table [VoucherRefund]"
        '--------------------------------------------------
        '[Refund_ID] [varchar](10) NOT NULL,
        '[Voucher_Acc_ID] [char](15) NOT NULL,
        '[Doc_Code] [char](20) NOT NULL,
        '[Encrypt_Field1] [varbinary](100) NOT NULL,
        '[Refund_Amt] [money],
        '[Refund_Dtm] [datetime] NOT NULL,
        '[Scheme_Seq] [int] NOT NULL,
        '[Record_Status] [char](1) NOT NULL,
        '[Create_Dtm] [datetime] NOT NULL,
        '[Create_By] [varchar](20) NOT NULL,
        '[Update_Dtm] [datetime] NOT NULL,
        '[Update_By] [varchar](20) NOT NULL,
        '[TSMP] [timestamp] NOT NULL

        'PRIMARY KEY ([VoucherRefund_ID])
        '==================================================

#End Region

#Region "DB Data Value of the Field"
        'Record_Status
        Public Const VR_RECORD_STATUS_P As Char = "P"     'Pending Approval
        Public Const VR_RECORD_STATUS_A As Char = "A"     'Approved
        Public Const VR_RECORD_STATUS_D As Char = "D"     'Deleted
        Public Const VR_RECORD_STATUS_R As Char = "R"     'Refunded
#End Region

#Region "Private Member"
        Private _strRefundID As String
        Private _strVoucher_Acc_ID As String
        Private _strDoc_Code As String
        Private _strDoc_No As String
        Private _intRefund_Amt As Integer
        Private _dtmRefundDtm As DateTime
        Private _intSchemeSeq As Integer
        Private _strRecord_Status As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _byteTSMP As Byte()
#End Region

#Region "Property"
        Public Property RefundID() As String
            Get
                Return _strRefundID
            End Get
            Set(ByVal value As String)
                _strRefundID = value
            End Set
        End Property

        Public Property VoucherAccID() As String
            Get
                Return Me._strVoucher_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strVoucher_Acc_ID = value
            End Set
        End Property

        Public Property DocCode() As String
            Get
                Return Me._strDoc_Code
            End Get
            Set(ByVal value As String)
                Me._strDoc_Code = value
            End Set
        End Property

        Public Property DocNo() As String
            Get
                Return Me._strDoc_No
            End Get
            Set(ByVal value As String)
                Me._strDoc_No = value
            End Set
        End Property

        Public Property RefundAmt() As Integer
            Get
                Return _intRefund_Amt
            End Get
            Set(ByVal value As Integer)
                _intRefund_Amt = value
            End Set
        End Property

        Public Property RefundDtm() As Nullable(Of DateTime)
            Get
                Return _dtmRefundDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmRefundDtm = value
            End Set
        End Property

        Public Property SchemeSeq() As Nullable(Of Integer)
            Get
                Return _intSchemeSeq
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intSchemeSeq = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecord_Status
            End Get
            Set(ByVal value As String)
                _strRecord_Status = value
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

        Public Property CreateDtm() As DateTime
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmCreateDtm = value
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

        Public Property UpdateDtm() As DateTime
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmUpdateDtm = value
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
#End Region

#Region "Constructor"
        ' For [VoucherRefund] Retrieval
        Public Sub New(ByVal strRefundID As String, _
                       ByVal strVoucher_Acc_ID As String, _
                       ByVal strDoc_Code As String, _
                       ByVal strDoc_No As String, _
                       ByVal intRefund_Amt As Integer, _
                       ByVal dtmRefundDtm As Nullable(Of DateTime), _
                       ByVal intSchemeSeq As Nullable(Of Integer), _
                       ByVal strRecord_Status As String, _
                       ByVal strCreateBy As String, _
                       ByVal dtmCreateDtm As DateTime, _
                       ByVal strUpdateBy As String, _
                       ByVal dtmUpdateDtm As DateTime, _
                       ByVal byteTSMP As Byte())

            _strRefundID = strRefundID
            _strVoucher_Acc_ID = strVoucher_Acc_ID
            _strDoc_Code = strDoc_Code
            _strDoc_No = strDoc_No
            _intRefund_Amt = intRefund_Amt
            _dtmRefundDtm = dtmRefundDtm
            _intSchemeSeq = intSchemeSeq
            _strRecord_Status = strRecord_Status
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _byteTSMP = byteTSMP
        End Sub

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Sub New(ByVal udtVoucherRefund As VoucherRefundModel)
            _strRefundID = udtVoucherRefund.RefundID
            _strVoucher_Acc_ID = udtVoucherRefund.VoucherAccID
            _strDoc_Code = udtVoucherRefund.DocCode
            _strDoc_No = udtVoucherRefund.DocNo
            _intRefund_Amt = udtVoucherRefund.RefundAmt
            _dtmRefundDtm = udtVoucherRefund.RefundDtm
            _intSchemeSeq = udtVoucherRefund.SchemeSeq
            _strRecord_Status = udtVoucherRefund.RecordStatus
            _strCreateBy = udtVoucherRefund.CreateBy
            _dtmCreateDtm = udtVoucherRefund.CreateDtm
            _strUpdateBy = udtVoucherRefund.UpdateBy
            _dtmUpdateDtm = udtVoucherRefund.UpdateDtm
            _byteTSMP = udtVoucherRefund.TSMP
        End Sub
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

#End Region

    End Class

End Namespace
