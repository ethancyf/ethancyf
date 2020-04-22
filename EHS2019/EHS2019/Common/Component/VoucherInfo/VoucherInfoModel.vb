Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.ProfessionVoucherQuota
Imports Common.Component.Scheme
Imports Common.DataAccess

Namespace Component.VoucherInfo

    <Serializable()> Public Class VoucherInfoModel

#Region "Private Member"
        Private _strFunctionCode As String = String.Empty
        Private _blnEnableVoucherDetail As Boolean = False
        Private _blnEnableVoucherQuota As Boolean = False
        Private _dtmServiceDate As Nullable(Of Date) = Nothing
        Private _dtmCurrentDate As Nullable(Of Date) = Nothing

        Private _udtVoucherDetailList As VoucherDetailModelCollection = Nothing
        Private _udtVoucherQuotaList As VoucherQuotaModelCollection = Nothing

        Private _intNextDepositAmount As Nullable(Of Integer) = Nothing
        Private _intNextCappingAmount As Nullable(Of Integer) = Nothing
        Private _dtmNextForfeitDate As Nullable(Of Date) = Nothing
        Private _intNextForfeitAmount As Nullable(Of Integer) = Nothing

#End Region

#Region "Constant"
        Public Enum AvailableVoucher
            None = 0
            Include = 1
        End Enum

        Public Enum AvailableQuota
            None = 0
            Include = 1
        End Enum

#End Region

#Region "Property"

        Public Property FunctionCode() As String
            Get
                Return Me._strFunctionCode
            End Get
            Set(ByVal value As String)
                Me._strFunctionCode = value
            End Set
        End Property

        Public Property EnableVoucherDetail() As Boolean
            Get
                Return Me._blnEnableVoucherDetail
            End Get
            Set(ByVal value As Boolean)
                Me._blnEnableVoucherDetail = value
            End Set
        End Property

        Public Property EnableVoucherQuota() As Boolean
            Get
                Return Me._blnEnableVoucherQuota
            End Get
            Set(ByVal value As Boolean)
                Me._blnEnableVoucherQuota = value
            End Set
        End Property

        Public Property ServiceDate() As Nullable(Of Date)
            Get
                Return Me._dtmServiceDate
            End Get
            Set(value As Nullable(Of Date))
                Me._dtmServiceDate = value
            End Set
        End Property

        Public Property CurrentDate() As Nullable(Of Date)
            Get
                Return Me._dtmCurrentDate
            End Get
            Set(value As Nullable(Of Date))
                Me._dtmCurrentDate = value
            End Set
        End Property

        Public Property VoucherDetailList() As VoucherDetailModelCollection
            Get
                Return Me._udtVoucherDetailList
            End Get
            Set(ByVal value As VoucherDetailModelCollection)
                Me._udtVoucherDetailList = value
            End Set
        End Property

        Public ReadOnly Property VoucherDetail(ByVal intSchemeSeq As Integer) As VoucherDetailModel
            Get
                Return Me._udtVoucherDetailList.Find(intSchemeSeq)
            End Get
        End Property

        Public Property VoucherQuotalist() As VoucherQuotaModelCollection
            Get
                Return Me._udtVoucherQuotaList
            End Get
            Set(ByVal value As VoucherQuotaModelCollection)
                Me._udtVoucherQuotaList = value
            End Set
        End Property

        Private ReadOnly Property VoucherQuota(ByVal strProfCode As String, ByVal dtmServiceDtm As Date) As VoucherQuotaModel
            Get
                Return Me._udtVoucherQuotaList.FilterByProfCodeEffectiveDtm(strProfCode, dtmServiceDtm)
            End Get
        End Property

        Public ReadOnly Property GetMaxUsableBalance(ByVal strProfCode As String) As Nullable(Of Integer)
            Get
                Dim intRes As Nullable(Of Integer) = Nothing

                Dim udtVoucherQuota As VoucherQuotaModel = Me._udtVoucherQuotaList.FilterByProfCodeEffectiveDtm(strProfCode, Me._dtmServiceDate)

                If Not udtVoucherQuota Is Nothing Then
                    Dim intAvailablevoucher As Integer = GetAvailableVoucher()

                    If intAvailablevoucher > udtVoucherQuota.AvailableQuota Then
                        intRes = udtVoucherQuota.AvailableQuota
                    Else
                        intRes = intAvailablevoucher
                    End If

                End If

                Return intRes
            End Get
        End Property

        Private ReadOnly Property GetEntitlement(ByVal intSchemeSeq As Integer) As Integer
            Get
                Return Me._udtVoucherDetailList.Find(intSchemeSeq).Entitlement
            End Get
        End Property

        Private ReadOnly Property GetUsed(ByVal intSchemeSeq As Integer) As Integer
            Get
                Return Me._udtVoucherDetailList.Find(intSchemeSeq).Used()
            End Get
        End Property

        Private ReadOnly Property GetUsed(ByVal intSchemeSeq As Integer, ByVal strSchemeCode As String) As Integer
            Get
                Return Me._udtVoucherDetailList.Find(intSchemeSeq).Used(strSchemeCode)
            End Get
        End Property

        Private ReadOnly Property GetRefund(ByVal intSchemeSeq As Integer) As Integer
            Get
                Return Me._udtVoucherDetailList.Find(intSchemeSeq).Refund
            End Get
        End Property

        Private ReadOnly Property GetRefund(ByVal intSchemeSeq As Integer, ByVal dtmServiceDate As Date) As Integer
            Get
                Dim intTotalRefund As Integer = 0

                For Each udtVoucherRefund As VoucherRefund.VoucherRefundModel In Me._udtVoucherDetailList.Find(intSchemeSeq).VoucherRefundList.FilterByRefundDtm(dtmServiceDate)
                    intTotalRefund = intTotalRefund + udtVoucherRefund.RefundAmt
                Next

                Return intTotalRefund
            End Get
        End Property

        Private ReadOnly Property GetWriteOff(ByVal intSchemeSeq As Integer) As Integer
            Get
                Return Me._udtVoucherDetailList.Find(intSchemeSeq).WriteOff
            End Get
        End Property

        Public ReadOnly Property GetTotalEntitlement() As Integer
            Get
                Return Me._udtVoucherDetailList.GetTotalEntitlement
            End Get
        End Property

        Public ReadOnly Property GetTotalUsed() As Integer
            Get
                Return Me._udtVoucherDetailList.GetTotalUsed()
            End Get
        End Property

        Public ReadOnly Property GetTotalUsed(ByVal strSchemeCode As String) As Integer
            Get
                Return Me._udtVoucherDetailList.GetTotalUsed(strSchemeCode)
            End Get
        End Property

        Public ReadOnly Property GetTotalRefund() As Integer
            Get
                Return Me._udtVoucherDetailList.GetTotalRefund
            End Get
        End Property

        Public ReadOnly Property GetTotalWriteOff() As Integer
            Get
                Return Me._udtVoucherDetailList.GetTotalWriteOff
            End Get
        End Property

        Public ReadOnly Property GetPeriodEndBalance() As Integer
            Get
                Return GetTotalEntitlement - GetTotalUsed - GetTotalWriteOff + GetTotalRefund
            End Get
        End Property

        Public ReadOnly Property GetPeriodEndBalance(ByVal intSchemeSeq As Integer) As Integer
            Get
                Dim intAvailableVoucher As Integer = 0

                For intCurrentSchemeSeq As Integer = 1 To intSchemeSeq
                    intAvailableVoucher += GetEntitlement(intCurrentSchemeSeq) - GetUsed(intCurrentSchemeSeq) - GetWriteOff(intCurrentSchemeSeq) + GetRefund(intCurrentSchemeSeq)
                Next

                Return intAvailableVoucher
            End Get
        End Property

        Private ReadOnly Property GetAvailableVoucherAsAtServiceDate(ByVal intSchemeSeq As Integer, ByVal dtmServiceDate As Date) As Integer
            Get
                Dim intAvailableVoucher As Integer = 0

                For intCurrentSchemeSeq As Integer = 1 To intSchemeSeq
                    intAvailableVoucher += GetEntitlement(intCurrentSchemeSeq) - GetUsed(intCurrentSchemeSeq) - GetWriteOff(intCurrentSchemeSeq) + GetRefund(intCurrentSchemeSeq, dtmServiceDate)
                Next

                Return intAvailableVoucher
            End Get
        End Property

        Public ReadOnly Property GetAvailableVoucher() As Integer
            Get
                If Me.ServiceDate Is Nothing OrElse CDate(Me.ServiceDate).Date = CDate(Me.CurrentDate).Date Then
                    'Current available voucher will be returned. Or the servcie date is not set.
                    Return GetTotalEntitlement - GetTotalUsed - GetTotalWriteOff + GetTotalRefund
                Else
                    'Back-date available voucher will be returned
                    Return CalculateAvailableVoucher(CDate(Me.ServiceDate).Date)
                End If

            End Get
        End Property

        'Public ReadOnly Property GetAvailableVoucher(ByVal intSchemeSeq As Integer) As Integer
        '    Get
        '        Return CalculateAvailableVoucher(Me._udtVoucherDetailList.Find(intSchemeSeq).SchemeSeq)
        '    End Get
        'End Property

        Public ReadOnly Property GetAvailableVoucher(ByVal dtmServiceDate As Date) As Integer
            Get
                Return CalculateAvailableVoucher(dtmServiceDate)
            End Get
        End Property

        Public ReadOnly Property GetNextDepositAmount() As Integer
            Get
                If _intNextDepositAmount Is Nothing Then
                    Me.GetNextForfeitVoucher()
                End If

                Return _intNextDepositAmount
            End Get
        End Property

        Public ReadOnly Property GetNextCappingAmount() As Integer
            Get
                If _intNextCappingAmount Is Nothing Then
                    Me.GetNextForfeitVoucher()
                End If

                Return _intNextCappingAmount
            End Get
        End Property

        Public ReadOnly Property GetNextForfeitDate() As Date
            Get
                If _dtmNextForfeitDate Is Nothing Then
                    Me.GetNextForfeitVoucher()
                End If

                Return _dtmNextForfeitDate
            End Get
        End Property

        Public ReadOnly Property GetNextForfeitAmount() As Integer
            Get
                If _intNextForfeitAmount Is Nothing Then
                    Me.GetNextForfeitVoucher()
                End If

                Return _intNextForfeitAmount
            End Get
        End Property

        Public ReadOnly Property GetAllTransaction() As TransactionDetailModelCollection
            Get
                Return Me._udtVoucherDetailList.GetAllTransaction
            End Get
        End Property
#End Region

#Region "Constructor"

        Public Sub New()
            _udtVoucherDetailList = New VoucherDetailModelCollection
            _udtVoucherQuotaList = New VoucherQuotaModelCollection

        End Sub

        Public Sub New(ByVal enumAvailableVoucher As AvailableVoucher, ByVal enumAvailableQuota As AvailableQuota)
            _blnEnableVoucherDetail = CBool(enumAvailableVoucher)
            _blnEnableVoucherQuota = CBool(enumAvailableQuota)

            _udtVoucherDetailList = New VoucherDetailModelCollection
            _udtVoucherQuotaList = New VoucherQuotaModelCollection

        End Sub

        Public Sub New(ByVal udtVoucherInfo As VoucherInfoModel)
            _blnEnableVoucherDetail = udtVoucherInfo.EnableVoucherDetail
            _blnEnableVoucherQuota = udtVoucherInfo.EnableVoucherQuota

            _udtVoucherDetailList = udtVoucherInfo.VoucherDetailList.Copy
            _udtVoucherQuotaList = udtVoucherInfo.VoucherQuotalist.Copy

        End Sub

#End Region

#Region "Method"
        Private Function CalculateAvailableVoucher(ByVal dtmServiceDate As Date) As Integer
            'If current date is not exist, get the current date
            If Me.CurrentDate Is Nothing Then
                Me.CurrentDate = (New GeneralFunction).GetSystemDateTime.Date
            End If

            Dim intTargetSchemeSeq As Integer = Me._udtVoucherDetailList.FilterByServiceDate(dtmServiceDate.Date)(0).SchemeSeq()
            Dim intCurrentSchemeSeq As Integer = Me._udtVoucherDetailList.FilterByServiceDate(CDate(Me.CurrentDate).Date)(0).SchemeSeq()

            Dim intTargetAvailableVoucher As Integer = 0
            Dim intMinValue As Nullable(Of Integer) = Nothing


            If intTargetSchemeSeq = intCurrentSchemeSeq Then
                'Current Season

                ' -------------------------------------------------------------------------------------------------------------
                ' 1. Get the target available voucher
                ' -------------------------------------------------------------------------------------------------------------
                ' e.g. Target Scheme Seq. = 13, Current Scheme Seq. = 13
                intTargetAvailableVoucher = Me.GetAvailableVoucherAsAtServiceDate(intTargetSchemeSeq, dtmServiceDate.Date)

            Else
                'Previous Season

                ' e.g. Sample data
                ' -------------------------------------------------------------------------------------------------------------
                ' Scheme Seq    | Write Off      | Available Voucher
                ' ------------------------------------------------------
                ' 10            | 0              | 3500   
                ' 11            | 1500           | 2700   
                ' 12            | 0              | 3700   
                ' 13            | 700            | 1000   
                ' -------------------------------------------------------------------------------------------------------------

                ' -------------------------------------------------------------------------------------------------------------
                ' 1. Get the target available voucher
                ' -------------------------------------------------------------------------------------------------------------
                ' e.g. Target Scheme Seq. = 10, Current Scheme Seq. = 13
                intTargetAvailableVoucher = Me.GetPeriodEndBalance(intTargetSchemeSeq)

                ' -------------------------------------------------------------------------------------------------------------
                ' 2. Get the list of value (available voucher + write off) after the target scheme seq. for comparison
                ' -------------------------------------------------------------------------------------------------------------
                ' e.g. The list of available voucher includes the scheme seq. 11, 12 and 13
                For Each udtFilteredVoucherDetail As VoucherDetailModel In Me.VoucherDetailList.FilterListAfterSchemeSeq(intTargetSchemeSeq + 1)
                    '(a) Available voucher at that scheme seq.
                    ' e.g. Scheme Seq = 13, Available voucher = 1000
                    Dim dtmCutOff As Date = IIf(udtFilteredVoucherDetail.PeriodEnd >= Me.CurrentDate, Me.CurrentDate, udtFilteredVoucherDetail.PeriodEnd)
                    Dim intSchemeSeqAvailableVoucher As Integer = GetAvailableVoucherAsAtServiceDate(udtFilteredVoucherDetail.SchemeSeq, dtmCutOff)

                    '(b) Sum of write off after the target scheme seq. and before the next scheme seq.
                    ' e.g. Scheme Seq = 13, Sum of write off(Seq. 11, 12 & 13) = 1500 + 0 + 700 = 2200
                    Dim intSubTotalWriteOff As Integer = 0

                    For Each udtVoucherDetailForWriteOff As VoucherDetailModel In Me.VoucherDetailList.FilterListAfterSchemeSeq(intTargetSchemeSeq + 1)
                        If udtVoucherDetailForWriteOff.SchemeSeq <= udtFilteredVoucherDetail.SchemeSeq Then
                            intSubTotalWriteOff = intSubTotalWriteOff + udtVoucherDetailForWriteOff.WriteOff
                        End If
                    Next

                    '(c) Logical maximum voucher amount for use at that scheme seq., = (a) + (b)
                    ' e.g. 1000 + 2200
                    Dim intLogicalAvailableVoucher As Integer = intSchemeSeqAvailableVoucher + intSubTotalWriteOff

                    'Find the minimum voucher amount
                    ' e.g. 3500 > 3200
                    If intTargetAvailableVoucher > intLogicalAvailableVoucher Then
                        'Set initial value
                        If intMinValue Is Nothing Then
                            intMinValue = intLogicalAvailableVoucher
                        End If

                        'Compare the values, choose the minimum one to set
                        If Not intMinValue Is Nothing AndAlso intMinValue > intLogicalAvailableVoucher Then
                            intMinValue = intLogicalAvailableVoucher
                        End If

                    End If

                Next

                'If has the minimum voucher amount, the target available voucher will be overrided.
                'e.g. Mininium voucher amount = 3200
                If Not intMinValue Is Nothing Then
                    intTargetAvailableVoucher = intMinValue
                End If

            End If

            Return intTargetAvailableVoucher

        End Function

        Private Sub GetNextForfeitVoucher()
            Dim dtmCurrent As Date = (New GeneralFunction).GetSystemDateTime
            Dim intCeiling As Integer = 0
            Dim intAllotment As Integer = 0
            Dim dtmNextForfeitDate As Date = New Date(dtmCurrent.Year + 1, 1, 1)
            Dim dtmCurrentForfeitDate As Date = New Date(dtmCurrent.Year, 1, 1)

            Dim udtSchemeClaimList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllSchemeClaim_WithSubsidizeGroup
            Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimList.Filter(SchemeClaimModel.HCVS)
            Dim udtFutureSubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection = udtSchemeClaim.SubsidizeGroupClaimList.Filter(dtmNextForfeitDate)

            If udtFutureSubsidizeGroupClaimList.Count > 0 Then
                ' -----------------------------------------------
                ' With next year voucher season settings
                ' -----------------------------------------------
                Dim udtFutureSubsidizeGroupClaim As SubsidizeGroupClaimModel = udtFutureSubsidizeGroupClaimList(0)
                intCeiling = udtFutureSubsidizeGroupClaim.NumSubsidizeCeiling
                intAllotment = udtFutureSubsidizeGroupClaim.NumSubsidize
            Else
                ' -----------------------------------------------------------------------------
                ' Without next year voucher season settings, use current settings to determine
                ' 1. Voucher allotment amount
                ' 2. Ceiling
                ' -----------------------------------------------------------------------------

                ' Find voucher allotment amount from claim period on 1 January at that current year
                Dim udtJanuary1SubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection = udtSchemeClaim.SubsidizeGroupClaimList.Filter(dtmCurrentForfeitDate)
                Dim udtJanuary1SubsidizeGroupClaim As SubsidizeGroupClaimModel = Nothing

                If udtJanuary1SubsidizeGroupClaimList.Count > 0 Then
                    udtJanuary1SubsidizeGroupClaim = udtJanuary1SubsidizeGroupClaimList(0)
                Else
                    ErrorHandler.CriticalError(Me.FunctionCode, String.Format("No available SubsidizeGropuClaim settings on date({0}).", dtmCurrentForfeitDate.ToShortDateString))
                End If

                ' Find ceiling amount from current claim period
                Dim udtCurrentSubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection = udtSchemeClaim.SubsidizeGroupClaimList.Filter(dtmCurrent)
                Dim udtCurrentSubsidizeGroupClaim As SubsidizeGroupClaimModel = Nothing

                If udtCurrentSubsidizeGroupClaimList.Count > 0 Then
                    udtCurrentSubsidizeGroupClaim = udtCurrentSubsidizeGroupClaimList(0)
                Else
                    ErrorHandler.CriticalError(Me.FunctionCode, String.Format("No available SubsidizeGropuClaim settings on date({0}).", dtmCurrent.ToShortDateString))
                End If

                intCeiling = udtCurrentSubsidizeGroupClaim.NumSubsidizeCeiling
                intAllotment = udtJanuary1SubsidizeGroupClaim.NumSubsidize

                ' Write system log
                ErrorHandler.CriticalError(Me.FunctionCode, String.Format("No next year voucher season settings on date({0}) at DB tables(e.g., SubsidizeGropuClaim).", dtmNextForfeitDate.ToShortDateString))

            End If

            _intNextDepositAmount = intAllotment
            _intNextCappingAmount = intCeiling
            _dtmNextForfeitDate = dtmNextForfeitDate

            Dim intForfeitAmount As Integer = Me.GetAvailableVoucher() + _intNextDepositAmount - _intNextCappingAmount

            If intForfeitAmount > 0 Then
                _intNextForfeitAmount = intForfeitAmount
            Else
                _intNextForfeitAmount = 0
            End If

        End Sub

        'Voucher Quota
        ' Retrieve quota on specific date
        Public Function GetVoucherQuota(ByVal dtmServiceDate As Date, _
                                        ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                        ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, _
                                        ByVal strProfessionCode As String, _
                                        Optional ByVal dtmClaimPeriodTo As Date? = Nothing, _
                                        Optional ByVal udtDB As Database = Nothing) As VoucherQuotaModel

            Dim udtEHSTransaction As New EHSTransactionBLL
            Dim udtTransactionDetailList As TransactionDetailModelCollection = Nothing
            Dim udtVoucherQuota As VoucherQuotaModel = Nothing

            ' Use the retrieved transaction to calculate quota
            If _udtVoucherDetailList.Count > 0 Then
                udtTransactionDetailList = Me.GetAllTransaction()
            End If

            udtVoucherQuota = udtEHSTransaction.getVoucherQuota(dtmServiceDate, udtEHSPersonalInformation, udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeCode, strProfessionCode, dtmClaimPeriodTo, udtTransactionDetailList, udtDB)

            If Not udtVoucherQuota Is Nothing AndAlso Me._udtVoucherQuotaList.FilterByProfCodeEffectiveDtm(strProfessionCode, dtmServiceDate) Is Nothing Then
                Me._udtVoucherQuotaList.Add(udtVoucherQuota)
            End If

            Return udtVoucherQuota
        End Function

        ' Retrieve quota based on voucher info's service date
        Public Function GetVoucherQuota(ByVal udtSchemeClaimModel As SchemeClaimModel,
                                        ByVal udtEHSPersonalInformation As EHSPersonalInformationModel,
                                        ByVal strProfessionCode As String, _
                                        Optional ByVal dtmClaimPeriodTo As Date? = Nothing, _
                                        Optional ByVal udtDB As Database = Nothing) As VoucherQuotaModel

            Dim dtmServiceDate As Date

            If Me.ServiceDate.HasValue Then
                dtmServiceDate = Me.ServiceDate.Value
            Else
                Dim udtGenFunct As New GeneralFunction()
                dtmServiceDate = udtGenFunct.GetSystemDateTime().Date
            End If

            Return Me.GetVoucherQuota(dtmServiceDate, udtSchemeClaimModel, udtEHSPersonalInformation, strProfessionCode, dtmClaimPeriodTo, udtDB)
        End Function

        Public Function GetVoucherQuotaList(ByVal udtSchemeClaimModel As SchemeClaimModel, ByVal udtEHSPersonalInformation As EHSPersonalInformationModel) As VoucherQuotaModelCollection

            Dim udtVoucherQuotaModelCollection As VoucherQuotaModelCollection = New VoucherQuotaModelCollection

            Dim udtProfessionModelCollection As Profession.ProfessionModelCollection = Profession.ProfessionBLL.GetProfessionList

            For Each udtProfessionModel As Profession.ProfessionModel In udtProfessionModelCollection
                Dim udtVoucherQuotaModel As New VoucherQuotaModel()

                udtVoucherQuotaModel = Me.GetVoucherQuota(udtSchemeClaimModel, udtEHSPersonalInformation, udtProfessionModel.ServiceCategoryCode)

                If Not udtVoucherQuotaModel Is Nothing Then
                    udtVoucherQuotaModelCollection.Add(udtVoucherQuotaModel)
                End If
            Next

            Return udtVoucherQuotaModelCollection
        End Function

        Public Function GetInfo(ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                ByVal udtEHSPersonalInformation As EHSPersonalInformationModel,
                                Optional ByVal strProfessionCode As String = "") As VoucherInfoModel

            Return Me.GetInfo((New GeneralFunction).GetSystemDateTime().Date, udtSchemeClaimModel, udtEHSPersonalInformation, strProfessionCode)

        End Function

        Public Function GetInfo(ByVal dtmServiceDate As Date, _
                                ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                ByVal udtEHSPersonalInformation As EHSPersonalInformationModel, _
                                Optional ByVal strProfessionCode As String = "") As VoucherInfoModel

            If Me.EnableVoucherDetail Then
                Me.GetVoucherDetails(dtmServiceDate, udtSchemeClaimModel, udtEHSPersonalInformation)
            End If

            If Me.EnableVoucherQuota Then
                If strProfessionCode <> String.Empty Then
                    Me.GetVoucherQuota(udtSchemeClaimModel, udtEHSPersonalInformation, strProfessionCode)
                Else
                    Me.GetVoucherQuotaList(udtSchemeClaimModel, udtEHSPersonalInformation)
                End If

            End If

            Return Me

        End Function

        Public Sub GetVoucherDetails(ByVal dtmServiceDate As Date, _
                                          ByVal udtSchemeClaimModel As SchemeClaimModel, _
                                          ByVal udtEHSPersonalInformation As EHSPersonalInformationModel)

            Dim udtEHSTransactionBLL As New EHSTransactionBLL
            Dim udtVoucherInfo As VoucherInfoModel = udtEHSTransactionBLL.getAvailableVoucher(dtmServiceDate, udtSchemeClaimModel, udtEHSPersonalInformation)

            Me.ServiceDate = udtVoucherInfo.ServiceDate
            Me.CurrentDate = udtVoucherInfo.CurrentDate
            Me.VoucherDetailList() = udtVoucherInfo.VoucherDetailList()

        End Sub

#End Region


    End Class

End Namespace