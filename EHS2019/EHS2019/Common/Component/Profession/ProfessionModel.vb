Imports Common.Component.ProfessionPracticeType

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.ComFunction.GeneralFunction

Namespace Component.Profession
    <Serializable()> Public Class ProfessionModel

        Private _strServiceCategoryCode As String
        Private _strServiceCategoryDesc As String
        Private _strServiceCategoryDescChi As String
        Private _strServiceCategoryDescCN As String
        Private _dtmClaimPeriodFrom As Nullable(Of DateTime)
        Private _dtmClaimPeriodTo As Nullable(Of DateTime)
        Private _dtmEnrolPeriodFrom As Nullable(Of DateTime)
        Private _dtmEnrolPeriodTo As Nullable(Of DateTime)
        Private _strServiceCategoryCodeSD As String
        Private _strServiceCategoryDescSD As String
        Private _strServiceCategoryDescSDChi As String
        Private _intSDDisplaySeq As Nullable(Of Integer)
        Private _dtmSDPeriodFrom As Nullable(Of DateTime)
        Private _dtmSDPeriodTo As Nullable(Of DateTime)
        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strEFormAvail As String
        ' CRE19-006 (DHC) [End][Winnie]

        Public Const Service_Category_Code As String = "Service_Category_Code"
        Public Const Service_Category_Desc As String = "Service_Category_Desc"
        Public Const Service_Category_Desc_Chi As String = "Service_Category_Desc_Chi"
        Public Const Service_Category_Desc_CN As String = "Service_Category_Desc_CN"
        Public Const Claim_Period_From As String = "Claim_Period_From"
        Public Const Claim_Period_To As String = "Claim_Period_To"
        Public Const Enrol_Period_From As String = "Enrol_Period_From"
        Public Const Enrol_Period_To As String = "Enrol_Period_To"
        Public Const Service_Category_Code_SD As String = "Service_Category_Code_SD"
        Public Const Service_Category_Desc_SD As String = "Service_Category_Desc_SD"
        Public Const Service_Category_Desc_SD_Chi As String = "Service_Category_Desc_SD_Chi"
        Public Const SD_Display_Seq As String = "SD_Display_Seq"
        Public Const SD_Period_From As String = "SD_Period_From"
        Public Const SD_Period_To As String = "SD_Period_To"

        
        Public Property ServiceCategoryCode() As String
            Get
                Return _strServiceCategoryCode
            End Get
            Set(ByVal value As String)
                _strServiceCategoryCode = value
            End Set
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

        Public Property ServiceCategoryDescCN() As String
            Get
                Return _strServiceCategoryDescCN
            End Get
            Set(ByVal value As String)
                _strServiceCategoryDescCN = value
            End Set
        End Property

        Public Property ClaimPeriodFrom() As Nullable(Of DateTime)
            Get
                Return _dtmClaimPeriodFrom
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmClaimPeriodFrom = value
            End Set
        End Property

        Public Property ClaimPeriodTo() As Nullable(Of DateTime)
            Get
                Return _dtmClaimPeriodTo
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmClaimPeriodTo = value
            End Set
        End Property

        Public Property EnrolPeriodFrom() As Nullable(Of DateTime)
            Get
                Return _dtmEnrolPeriodFrom
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEnrolPeriodFrom = value
            End Set
        End Property

        Public Property EnrolPeriodTo() As Nullable(Of DateTime)
            Get
                Return _dtmEnrolPeriodTo
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEnrolPeriodTo = value
            End Set
        End Property

        Public Property ServiceCategoryCodeSD() As String
            Get
                Return _strServiceCategoryCodeSD
            End Get
            Set(ByVal value As String)
                _strServiceCategoryCodeSD = value
            End Set
        End Property

        Public Property ServiceCategoryDescSD() As String
            Get
                Return _strServiceCategoryDescSD
            End Get
            Set(ByVal value As String)
                _strServiceCategoryDescSD = value
            End Set
        End Property

        Public Property ServiceCategoryDescSDChi() As String
            Get
                Return _strServiceCategoryDescSDChi
            End Get
            Set(ByVal value As String)
                _strServiceCategoryDescSDChi = value
            End Set
        End Property

        Public Property SDDisplaySeq() As Integer
            Get
                Return _intSDDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intSDDisplaySeq = value
            End Set
        End Property

        Public Property SDPeriodFrom() As Nullable(Of DateTime)
            Get
                Return _dtmSDPeriodFrom
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmSDPeriodFrom = value
            End Set
        End Property

        Public Property SDPeriodTo() As Nullable(Of DateTime)
            Get
                Return _dtmSDPeriodTo
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmSDPeriodTo = value
            End Set
        End Property

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Property EFormAvail() As String
            Get
                Return _strEFormAvail
            End Get
            Set(ByVal value As String)
                _strEFormAvail = value
            End Set
        End Property
        ' CRE19-006 (DHC) [End][Winnie]

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------

        ''' <summary>
        ''' Check this profession is allow to join PCD or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AllowJoinPCD() As Boolean
            Get
                Return PracticeTypePCD IsNot Nothing
            End Get
        End Property

        ''' <summary>
        ''' Get the available Type of Practice to join PCD for this profession
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PracticeTypePCD() As ProfessionPracticeTypeModelCollection
            Get
                Return ProfessionPracticeTypeBLL.GetByProfession(Me)
            End Get
        End Property

        ''' <summary>
        ''' Check this profession has sub profession or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HasSubProfession() As Boolean
            Get
                Return SubProfession IsNot Nothing
            End Get
        End Property

        ''' <summary>
        ''' Get the sub profession of this profession
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SubProfession() As SubProfessionModel
            Get
                Return ProfessionBLL.GetSubProfessionByProfession(Me)
            End Get
        End Property

        ' CRE12-001 eHS and PCD integration [End][Koala]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Add column [EForm_Avail]
        Public Sub New(ByVal strServiceCategoryCode As String, ByVal strServiceCategoryDesc As String, ByVal strServiceCategoryDescChi As String, ByVal strServiceCategoryDescCN As String, ByVal dtmEnrolPeriodFrom As Nullable(Of DateTime), ByVal dtmEnrolPeriodTo As Nullable(Of DateTime), ByVal dtmClaimPeriodFrom As Nullable(Of DateTime), ByVal dtmClaimPeriodTo As Nullable(Of DateTime), ByVal strServiceCategoryCodeSD As String, ByVal strServiceCategoryDescSD As String, ByVal strServiceCategoryDescSDChi As String, ByVal intSDDisplaySeq As Nullable(Of Integer), ByVal dtmSDPeriodFrom As Nullable(Of DateTime), ByVal dtmSDPeriodTo As Nullable(Of DateTime), ByVal strEFormAvail As String)
            ' CRE19-006 (DHC) [End][Winnie]
            _strServiceCategoryCode = strServiceCategoryCode
            _strServiceCategoryDesc = strServiceCategoryDesc
            _strServiceCategoryDescChi = strServiceCategoryDescChi
            _strServiceCategoryDescCN = strServiceCategoryDescCN
            _dtmEnrolPeriodFrom = dtmEnrolPeriodFrom
            _dtmEnrolPeriodTo = dtmEnrolPeriodTo
            _dtmClaimPeriodFrom = dtmClaimPeriodFrom
            _dtmClaimPeriodTo = dtmClaimPeriodTo
            _strServiceCategoryCodeSD = strServiceCategoryCodeSD
            _strServiceCategoryDescSD = strServiceCategoryDescSD
            _strServiceCategoryDescSDChi = strServiceCategoryDescSDChi
            _intSDDisplaySeq = intSDDisplaySeq
            _dtmSDPeriodFrom = dtmSDPeriodFrom
            _dtmSDPeriodTo = dtmSDPeriodTo
            _strEFormAvail = strEFormAvail
        End Sub

        Public Sub New(ByVal udtProfessionModel As ProfessionModel)
            _strServiceCategoryCode = udtProfessionModel.ServiceCategoryCode
            _strServiceCategoryDesc = udtProfessionModel.ServiceCategoryDesc
            _strServiceCategoryDescChi = udtProfessionModel.ServiceCategoryDescChi
            _strServiceCategoryDescCN = udtProfessionModel.ServiceCategoryDescCN
            _dtmEnrolPeriodFrom = udtProfessionModel.EnrolPeriodFrom
            _dtmEnrolPeriodTo = udtProfessionModel.EnrolPeriodTo
            _dtmClaimPeriodFrom = udtProfessionModel.ClaimPeriodFrom
            _dtmClaimPeriodTo = udtProfessionModel.ClaimPeriodTo
            _strServiceCategoryCodeSD = udtProfessionModel.ServiceCategoryCodeSD
            _strServiceCategoryDescSD = udtProfessionModel.ServiceCategoryDescSD
            _strServiceCategoryDescSDChi = udtProfessionModel.ServiceCategoryDescSDChi
            _intSDDisplaySeq = udtProfessionModel.SDDisplaySeq
            _dtmSDPeriodFrom = udtProfessionModel.SDPeriodFrom
            _dtmSDPeriodTo = udtProfessionModel.SDPeriodTo
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            _strEFormAvail = udtProfessionModel.EFormAvail
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        Public Function IsEnrolPeriod() As Boolean
            Return IsWithinPeriod(EnrolPeriodFrom, EnrolPeriodTo)
        End Function

        Public Function IsEnrolPeriod(ByVal dtmServiceDate As DateTime) As Boolean
            Return IsWithinPeriod(dtmServiceDate, EnrolPeriodFrom, EnrolPeriodTo)
        End Function

        Public Function IsSDPeriod() As Boolean
            Return IsWithinPeriod(SDPeriodFrom, SDPeriodTo)
        End Function

        Public Function IsSDPeriod(ByVal dtmServiceDate As DateTime) As Boolean
            Return IsWithinPeriod(dtmServiceDate, SDPeriodFrom, SDPeriodTo)
        End Function

        Public Function IsClaimPeriod() As Boolean
            Return IsWithinPeriod(ClaimPeriodFrom, ClaimPeriodTo)
        End Function

        Public Function IsClaimPeriod(ByVal dtmServiceDate As DateTime) As Boolean
            Return IsWithinPeriod(dtmServiceDate, ClaimPeriodFrom, ClaimPeriodTo)
        End Function

    End Class
End Namespace

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

