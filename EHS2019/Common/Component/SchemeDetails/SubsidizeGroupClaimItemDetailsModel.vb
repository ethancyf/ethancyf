Namespace Component.SchemeDetails
    Public Class SubsidizeGroupClaimItemDetailsModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Private Member"
        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _strSubsidize_Item_Code As String
        Private _strAvailable_Item_Code As String

        Private _strCreate_By As String
        Private _dtmCreate_Dtm As DateTime
        Private _strUpdate_By As String
        Private _dtmUpdate_Dtm As DateTime
        Private _strRecord_Status As String

        ' Addition Field
        Private _intDisplay_Seq As Integer
        Private _strAvailable_Item_Desc As String
        Private _strAvailable_Item_Desc_Chi As String

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private _intAvailable_Item_Num As Integer
        Private _strInternal_Use As String
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]


#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer

            Get
                Return Me._intScheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intScheme_Seq = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Code = value
            End Set
        End Property

        Public Property SubsidizeItemCode() As String
            Get
                Return Me._strSubsidize_Item_Code
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

        Public Property CreateBy() As String
            Get
                Return Me._strCreate_By
            End Get
            Set(ByVal value As String)
                Me._strCreate_By = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreate_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreate_Dtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdate_By
            End Get
            Set(ByVal value As String)
                Me._strUpdate_By = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdate_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdate_Dtm = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecord_Status
            End Get
            Set(ByVal value As String)
                Me._strRecord_Status = value
            End Set
        End Property

        ' Addition

        Public Property DisplaySeq() As Integer
            Get
                Return Me._intDisplay_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplay_Seq = value
            End Set
        End Property

        Public Property AvailableItemDesc() As String
            Get
                Return Me._strAvailable_Item_Desc
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc = value
            End Set
        End Property

        Public Property AvailableItemDescChi() As String
            Get
                Return Me._strAvailable_Item_Desc_Chi
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc_Chi = value
            End Set
        End Property

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Property AvailableItemNum() As Integer
            Get
                Return Me._intAvailable_Item_Num
            End Get
            Set(ByVal value As Integer)
                Me._intAvailable_Item_Num = value
            End Set
        End Property

        Public Property InternalUse() As Boolean
            Get
                If Me._strInternal_Use.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strInternal_Use = strYES
                Else
                    Me._strInternal_Use = strNO
                End If
            End Set
        End Property
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSubsidizeGroupClaimItemDetailsModel As SubsidizeGroupClaimItemDetailsModel)

            Me._strScheme_Code = udtSubsidizeGroupClaimItemDetailsModel._strScheme_Code
            Me._intScheme_Seq = udtSubsidizeGroupClaimItemDetailsModel._intScheme_Seq
            Me._strSubsidize_Code = udtSubsidizeGroupClaimItemDetailsModel._strSubsidize_Code
            Me._strSubsidize_Item_Code = udtSubsidizeGroupClaimItemDetailsModel._strSubsidize_Item_Code
            Me._strAvailable_Item_Code = udtSubsidizeGroupClaimItemDetailsModel._strAvailable_Item_Code

            Me._strCreate_By = udtSubsidizeGroupClaimItemDetailsModel._strCreate_By
            Me._dtmCreate_Dtm = udtSubsidizeGroupClaimItemDetailsModel._dtmCreate_Dtm
            Me._strUpdate_By = udtSubsidizeGroupClaimItemDetailsModel._strUpdate_By
            Me._dtmUpdate_Dtm = udtSubsidizeGroupClaimItemDetailsModel._dtmUpdate_Dtm
            Me._strRecord_Status = udtSubsidizeGroupClaimItemDetailsModel._strRecord_Status

            Me._intDisplay_Seq = udtSubsidizeGroupClaimItemDetailsModel._intDisplay_Seq
            Me._strAvailable_Item_Desc = udtSubsidizeGroupClaimItemDetailsModel._strAvailable_Item_Desc
            Me._strAvailable_Item_Desc_Chi = udtSubsidizeGroupClaimItemDetailsModel._strAvailable_Item_Desc_Chi

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            Me._intAvailable_Item_Num = udtSubsidizeGroupClaimItemDetailsModel._intAvailable_Item_Num
            Me._strInternal_Use = udtSubsidizeGroupClaimItemDetailsModel._strInternal_Use
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        End Sub

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
                        ByVal strSubsidizeItemCode As String, ByVal intDisplaySeq As Integer, _
                        ByVal strAvailableItemCode As String, ByVal strAvailableItemDesc As String, ByVal strAvailableItemDescChi As String, _
                        ByVal intAvailableItemNum As Integer, ByVal strInternalUse As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As String, _
                        ByVal strRecordStatus As String)

            Me._strScheme_Code = strSchemeCode
            Me._intScheme_Seq = intSchemeSeq
            Me._strSubsidize_Code = strSubsidizeCode

            Me._strSubsidize_Item_Code = strSubsidizeItemCode
            Me._intDisplay_Seq = intDisplaySeq
            Me._strAvailable_Item_Code = strAvailableItemCode
            Me._strAvailable_Item_Desc = strAvailableItemDesc
            Me._strAvailable_Item_Desc_Chi = strAvailableItemDescChi

            Me._intAvailable_Item_Num = intAvailableItemNum
            Me._strInternal_Use = strInternalUse

            Me._strCreate_By = strCreateBy
            Me._dtmCreate_Dtm = dtmCreateDtm
            Me._strUpdate_By = strUpdateBy
            Me._dtmUpdate_Dtm = dtmUpdateDtm
            Me._strRecord_Status = strRecordStatus

        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

#End Region
    End Class
End Namespace