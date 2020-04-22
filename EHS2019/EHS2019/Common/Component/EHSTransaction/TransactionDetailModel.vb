Imports Common.Component.StaticData

Namespace Component.EHSTransaction
    <Serializable()> Public Class TransactionDetailModel

#Region "Status"

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

        Public Enum SysSource
            Database
            NewAdd
        End Enum

        Public Class AppSourceClass
            Public Const WEB_FULL = "WEB-FULL"
            Public Const WEB_TEXT = "WEB-TEXT"
            Public Const IVRS = "IVRS"
        End Class

        Public Enum AppSourceEnum
            WEB_FULL
            WEB_TEXT
            IVRS
        End Enum
#End Region

#Region "Schema"
        'Transaction_ID	char(20)	Unchecked
        'Scheme_Code	char(10)	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Item_Code	char(10)	Checked
        'Available_Item_Code    char(20)    Checked
        'Unit	int		Checked
        'Per_Unit_Value	money	Checked
        'Total_Amount	money	Checked
        'Remark	nvarchar(255)	Checked
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        'ExchangeRate_Value decimal(9,3) null
        'Total_Amount_RMB	money null
        'CRE13-019-02 Extend HCVS to China [End][Karl]
#End Region

#Region "Private Member"

        Private _strTransaction_ID As String
        Private _strScheme_Code As String
        Private _strSubsidize_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Item_Code As String
        Private _strAvailable_Item_Code As String
        Private _intUnit As Nullable(Of Integer)
        Private _dblPer_Unit_Value As Nullable(Of Double)
        Private _dblTotal_Amount As Nullable(Of Double)
        Private _strRemark As String
        Private _dblExchangeRate_Value As Nullable(Of Double)
        Private _dblTotal_Amount_RMB As Nullable(Of Double)

#End Region

#Region "Property"

        Public Property TransactionID() As String
            Get
                Return Me._strTransaction_ID
            End Get
            Set(ByVal value As String)
                Me._strTransaction_ID = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return _strSubsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Code = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return _intScheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intScheme_Seq = value
            End Set
        End Property

        Public Property SubsidizeItemCode() As String
            Get
                Return _strSubsidize_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Item_Code = value
            End Set
        End Property

        Public Property AvailableItemCode() As String
            Get
                Return Me._strAvailable_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Code = value
            End Set
        End Property

        Public Property Unit() As Nullable(Of Integer)
            Get
                Return Me._intUnit
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intUnit = value
            End Set
        End Property

        Public Property PerUnitValue() As Nullable(Of Double)
            Get
                Return Me._dblPer_Unit_Value
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblPer_Unit_Value = value
            End Set
        End Property

        Public Property TotalAmount() As Nullable(Of Double)
            Get
                Return Me._dblTotal_Amount
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblTotal_Amount = value
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
        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Public Property ExchangeRate_Value() As Nullable(Of Double)
            Get
                Return Me._dblExchangeRate_Value
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblExchangeRate_Value = value
            End Set
        End Property

        Public Property TotalAmountRMB() As Nullable(Of Double)
            Get
                Return Me._dblTotal_Amount_RMB
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblTotal_Amount_RMB = value
            End Set
        End Property
        'CRE13-019-02 Extend HCVS to China [End][Karl]
#End Region

#Region "Addition Member"

        Private _strAvailable_Item_Desc As String
        Private _strAvailable_Item_Desc_Chi As String
        Private _strAvailable_Item_Desc_CN As String

        Private _dtmService_Receive_Dtm As Date
        Private _dtmDOB As Date
        Private _strExact_DOB As String

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private _strServiceType As String
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

        Property AvailableItemDesc() As String
            Get
                Return Me._strAvailable_Item_Desc
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc = value
            End Set
        End Property

        Property AvailableItemDescChi() As String
            Get
                Return Me._strAvailable_Item_Desc_Chi
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc_Chi = value
            End Set
        End Property

        Property AvailableItemDescCN() As String
            Get
                Return Me._strAvailable_Item_Desc_CN
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc_CN = value
            End Set
        End Property

        Property ServiceReceiveDtm() As Date
            Get
                Return Me._dtmService_Receive_Dtm
            End Get
            Set(ByVal value As Date)
                Me._dtmService_Receive_Dtm = value
            End Set
        End Property

        Property DOB() As Date
            Get
                Return Me._dtmDOB
            End Get
            Set(ByVal value As Date)
                Me._dtmDOB = value
            End Set
        End Property

        Property ExactDOB() As String
            Get
                Return Me._strExact_DOB
            End Get
            Set(ByVal value As String)
                Me._strExact_DOB = value
            End Set
        End Property

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Property ServiceType() As String
            Get
                Return Me._strServiceType
            End Get
            Set(ByVal value As String)
                Me._strServiceType = value
            End Set
        End Property
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
#End Region

#Region "Constructor"

        Public Sub New()
            Me._strTransaction_ID = String.Empty
            Me._strScheme_Code = String.Empty
            Me._strSubsidize_Code = String.Empty
            Me._intScheme_Seq = 0
            Me._strSubsidize_Item_Code = String.Empty

            Me._strAvailable_Item_Code = String.Empty
            'Me._intUnit = 0
            'Me._dblPer_Unit_Value = 0.0
            'Me._dblTotal_Amount = 0.0
            Me._strRemark = String.Empty

            ' Addition Memeber
            Me._strAvailable_Item_Desc = String.Empty
            Me._strAvailable_Item_Desc_Chi = String.Empty
            Me._strAvailable_Item_Desc_CN = String.Empty

            Me._dtmDOB = Nothing
            Me._strExact_DOB = Nothing

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Me._strServiceType = String.Empty
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
        End Sub

        Public Sub New(ByVal udtTranDetailModel As TransactionDetailModel)
            With udtTranDetailModel
                Me._strTransaction_ID = ._strTransaction_ID
                Me._strScheme_Code = ._strScheme_Code
                Me._strSubsidize_Code = ._strSubsidize_Code
                Me._intScheme_Seq = ._intScheme_Seq
                Me._strSubsidize_Item_Code = ._strSubsidize_Item_Code

                Me._strAvailable_Item_Code = ._strAvailable_Item_Code
                Me._intUnit = ._intUnit
                Me._dblPer_Unit_Value = ._dblPer_Unit_Value
                Me._dblTotal_Amount = ._dblTotal_Amount
                Me._strRemark = ._strRemark
                'CRE13-019-02 Extend HCVS to China [Start][Karl]
                Me._dblExchangeRate_Value = ._dblExchangeRate_Value
                Me._dblTotal_Amount_RMB = ._dblTotal_Amount_RMB
                'CRE13-019-02 Extend HCVS to China [End][Karl]

                ' Addition Memeber
                Me._strAvailable_Item_Desc = ._strAvailable_Item_Desc
                Me._strAvailable_Item_Desc_Chi = ._strAvailable_Item_Desc_Chi
                Me._strAvailable_Item_Desc_CN = ._strAvailable_Item_Desc_CN
                Me._dtmService_Receive_Dtm = ._dtmService_Receive_Dtm

                Me._dtmDOB = ._dtmDOB
                Me._strExact_DOB = ._strExact_DOB

                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Me._strServiceType = ._strServiceType
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
            End With
        End Sub

        'CRE13-019-02 Extend HCVS to China [Start][Karl]
        Public Sub New(ByVal strTrsansactionID As String, ByVal strSchemeCode As String, ByVal intScheme_Seq As Integer, ByVal strSubsidize_Code As String, _
                        ByVal strSubsidize_Item_Code As String, ByVal strAvailable_Item_Code As String, ByVal intUnit As Nullable(Of Integer), _
                        ByVal dblPer_Unit_Value As Nullable(Of Double), ByVal dblTotal_Amount As Nullable(Of Double), ByVal strRemark As String, _
                        ByVal dblExchangeRate_Value As Nullable(Of Double), ByVal dblTotal_Amount_RMB As Nullable(Of Double), _
                        Optional ByVal strAvailable_Item_Desc As String = "", Optional ByVal strAvailable_Item_Desc_Chi As String = "", Optional ByVal strAvailable_Item_Desc_CN As String = "")
            'CRE13-019-02 Extend HCVS to China [End][Karl]
            Me._strTransaction_ID = strTrsansactionID
            Me._strScheme_Code = strSchemeCode
            Me._strSubsidize_Code = strSubsidize_Code
            Me._intScheme_Seq = intScheme_Seq
            Me._strSubsidize_Item_Code = strSubsidize_Item_Code

            Me._strAvailable_Item_Code = strAvailable_Item_Code
            Me._intUnit = intUnit
            Me._dblPer_Unit_Value = dblPer_Unit_Value
            Me._dblTotal_Amount = dblTotal_Amount
            Me._strRemark = strRemark
            'CRE13-019-02 Extend HCVS to China [Start][Karl]
            Me._dblExchangeRate_Value = dblExchangeRate_Value
            Me._dblTotal_Amount_RMB = dblTotal_Amount_RMB
            'CRE13-019-02 Extend HCVS to China [End][Karl]

            ' Addition Memeber
            Me._strAvailable_Item_Desc = strAvailable_Item_Desc
            Me._strAvailable_Item_Desc_Chi = strAvailable_Item_Desc_Chi
            Me._strAvailable_Item_Desc_CN = strAvailable_Item_Desc_CN
        End Sub

#End Region

    End Class
End Namespace