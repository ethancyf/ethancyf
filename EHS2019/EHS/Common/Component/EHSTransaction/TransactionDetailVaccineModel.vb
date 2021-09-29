Imports Common.Component.StaticData
Imports Common.Component.EHSAccount.EHSAccountModel

Namespace Component.EHSTransaction
    <Serializable()> Public Class TransactionDetailVaccineModel
        Inherits TransactionDetailModel

        Private _strProvider As String ' Define the source of transaction, e.g. HA or (RVP or eHS)
        Private _dtmTransaction_Dtm As Nullable(Of DateTime)

        Private _strSubsidize_Desc As String
        Private _strSubsidize_Desc_Chi As String
        Private _strRecordType As String = RecordTypeClass.OnSite
        Private _bForBar As Boolean = True
        Private _bIsUnknownVaccine As Boolean = False

        Private _strPracticeName As String = String.Empty
        Private _strPracticeNameChi As String = String.Empty
        Private _strExtRefStatus As String = String.Empty
        Private _strHighRisk As String = String.Empty
        Private _udtPersonalInformationDemographic As EHSPersonalInformationDemographicModel

        Private _strVaccineBrand As String = String.Empty
        Private _strVaccineLotNo As String = String.Empty
        Private _strVaccineTradeName As String = String.Empty
        Private _strVaccineTradeNameChi As String = String.Empty
        Private _strClinicType As String = String.Empty

        Private _strJoinEHRSS As String = String.Empty
        Private _strContactNo As String = String.Empty
        Private _strNonLocalRecovered As String = String.Empty

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _strMainCategory As String = String.Empty
        Private _strSubCategory As String = String.Empty
        ' CRE20-0023 (Immu record) [End][Chris YIM]

#Region "Status"

        Public Class ProviderClass
            Public Const [Private] = "P"
            Public Const HA = "HA"
            Public Const DH = "DH"
            Public Const RVP = Common.Component.Scheme.SchemeClaimModel.RVP
            Public Const ClassCode = "VaccinationRecordProvider"
        End Class

        Public Class RecordTypeClass
            Public Const OnSite = "O"
            Public Const History = "H"
            Public Const DemographicNotMatch = "D"
            Public Const ClassCode = "VaccinationRecordRecordType"
        End Class

#End Region

#Region "Property"

        Public Overloads Property SchemeCode() As String
            Get
                Return MyBase.SchemeCode
            End Get
            Set(ByVal value As String)
                MyBase.SchemeCode = value
            End Set
        End Property
        ''' <summary>
        ''' Define the source of transaction, e.g. eHS or HA
        ''' </summary>
        ''' <value>Private or HA (ProviderClass)</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Provider() As String
            Get
                Return _strProvider
            End Get
            Set(ByVal value As String)
                _strProvider = value
            End Set
        End Property

        Public Property TransactionDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmTransaction_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmTransaction_Dtm = value
            End Set
        End Property

        Public Property SubsidizeDesc() As String
            Get
                Return _strSubsidize_Desc
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Desc = value
            End Set
        End Property

        Public Property SubsidizeDescChi() As String
            Get
                Return _strSubsidize_Desc_Chi
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Desc_Chi = value
            End Set
        End Property

        Public Property RecordType() As String
            Get
                Return _strRecordType
            End Get
            Set(ByVal value As String)
                Me._strRecordType = value
            End Set
        End Property

        Public Property ForBar() As Boolean
            Get
                Return _bForBar
            End Get
            Set(ByVal value As Boolean)
                _bForBar = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        Public Property IsUnknownVaccine() As Boolean
            Get
                Return _bIsUnknownVaccine
            End Get
            Set(value As Boolean)
                _bIsUnknownVaccine = value
            End Set
        End Property
        ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]

        Public Property PracticeName() As String
            Get
                Return _strPracticeName
            End Get
            Set(ByVal value As String)
                _strPracticeName = value
            End Set
        End Property

        Public Property PracticeNameChi() As String
            Get
                Return _strPracticeNameChi
            End Get
            Set(ByVal value As String)
                _strPracticeNameChi = value
            End Set
        End Property

        ''' <summary>
        ''' External source reference from table (VoucherTransaction), 
        ''' Value can be convert to object (EHSTransactionModel.ExtRefStatusClass)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExtRefStatus() As String
            Get
                Return _strExtRefStatus
            End Get
            Set(ByVal value As String)
                _strExtRefStatus = value
            End Set
        End Property

        Public Property HighRisk() As String
            Get
                Return _strHighRisk
            End Get
            Set(ByVal value As String)
                _strHighRisk = value
            End Set
        End Property

        Public Property PersonalInformationDemographic() As EHSPersonalInformationDemographicModel
            Get
                Return _udtPersonalInformationDemographic
            End Get
            Set(ByVal value As EHSPersonalInformationDemographicModel)
                _udtPersonalInformationDemographic = value
            End Set
        End Property

        Public Property VaccineBrand() As String
            Get
                Return _strVaccineBrand
            End Get
            Set(ByVal value As String)
                _strVaccineBrand = value
            End Set
        End Property

        Public Property VaccineLotNo() As String
            Get
                Return _strVaccineLotNo
            End Get
            Set(ByVal value As String)
                _strVaccineLotNo = value
            End Set
        End Property

        Public Property VaccineTradeName() As String
            Get
                Return _strVaccineTradeName
            End Get
            Set(ByVal value As String)
                _strVaccineTradeName = value
            End Set
        End Property

        Public Property VaccineTradeNameChi() As String
            Get
                Return _strVaccineTradeNameChi
            End Get
            Set(ByVal value As String)
                _strVaccineTradeNameChi = value
            End Set
        End Property

        Public Property ClinicType() As PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum
            Get
                Select Case _strClinicType
                    Case "C"
                        Return PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.Clinic
                    Case "N"
                        Return PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic
                    Case Else
                        Return PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.NA
                End Select
            End Get
            Set(ByVal value As PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum)
                Select Case value
                    Case PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.Clinic
                        _strClinicType = "C"
                    Case PracticeSchemeInfo.PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic
                        _strClinicType = "N"
                    Case Else
                        _strClinicType = String.Empty
                End Select
            End Set
        End Property

        Public Property JoinEHRSS() As String
            Get
                Return _strJoinEHRSS
            End Get
            Set(ByVal value As String)
                _strJoinEHRSS = value
            End Set
        End Property

        Public Property ContactNo() As String
            Get
                Return _strContactNo
            End Get
            Set(ByVal value As String)
                _strContactNo = value
            End Set
        End Property

        Public Property NonLocalRecovered() As String
            Get
                Return _strNonLocalRecovered
            End Get
            Set(ByVal value As String)
                _strNonLocalRecovered = value
            End Set
        End Property

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property MainCategory() As String
            Get
                Return _strMainCategory
            End Get
            Set(ByVal value As String)
                _strMainCategory = value
            End Set
        End Property
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property SubCategory() As String
            Get
                Return _strSubCategory
            End Get
            Set(ByVal value As String)
                _strSubCategory = value
            End Set
        End Property
        ' CRE20-0023 (Immu record) [End][Chris YIM]

#End Region

#Region "Constructor"

        Public Sub New(ByVal udtTransactionDetailVaccine As TransactionDetailVaccineModel)
            Me.TransactionID = udtTransactionDetailVaccine.TransactionID
            Me.SchemeCode = udtTransactionDetailVaccine.SchemeCode
            Me.SubsidizeCode = udtTransactionDetailVaccine.SubsidizeCode
            Me.SchemeSeq = udtTransactionDetailVaccine.SchemeSeq
            Me.SubsidizeItemCode = udtTransactionDetailVaccine.SubsidizeItemCode

            Me.AvailableItemCode = udtTransactionDetailVaccine.AvailableItemCode
            Me.Unit = udtTransactionDetailVaccine.Unit
            Me.PerUnitValue = udtTransactionDetailVaccine.PerUnitValue
            Me.TotalAmount = udtTransactionDetailVaccine.TotalAmount
            Me.Remark = udtTransactionDetailVaccine.Remark
            Me.Provider = udtTransactionDetailVaccine.Provider

            ' Addition Memeber
            Me.AvailableItemDesc = udtTransactionDetailVaccine.AvailableItemDesc
            Me.AvailableItemDescChi = udtTransactionDetailVaccine.AvailableItemDescChi

            Me.TransactionDtm = udtTransactionDetailVaccine.TransactionDtm
            Me.VaccineBrand = udtTransactionDetailVaccine.VaccineBrand
            Me.VaccineLotNo = udtTransactionDetailVaccine.VaccineLotNo
            Me.VaccineTradeName = udtTransactionDetailVaccine.VaccineTradeName
            Me.VaccineTradeNameChi = udtTransactionDetailVaccine.VaccineTradeNameChi
            Me.ClinicType = udtTransactionDetailVaccine.ClinicType

            Me.JoinEHRSS = udtTransactionDetailVaccine.JoinEHRSS
            Me.ContactNo = udtTransactionDetailVaccine.ContactNo
            Me.NonLocalRecovered = udtTransactionDetailVaccine.NonLocalRecovered
            Me.MainCategory = udtTransactionDetailVaccine.MainCategory
            Me.SubCategory = udtTransactionDetailVaccine.SubCategory

        End Sub

        Public Sub New(ByVal strTransactionID As String, ByVal strSchemeCode As String, ByVal intScheme_Seq As Integer, ByVal strSubsidize_Code As String, _
                        ByVal strSubsidize_Item_Code As String, ByVal strAvailable_Item_Code As String, ByVal intUnit As Nullable(Of Integer), _
                        ByVal dblPer_Unit_Value As Nullable(Of Double), ByVal dblTotal_Amount As Nullable(Of Double), ByVal strRemark As String, _
                        ByVal dtmTransactionDtm As Nullable(Of DateTime), _
                        Optional ByVal strAvailable_Item_Desc As String = "", Optional ByVal strAvailable_Item_Desc_Chi As String = "")

            Constructor(strTransactionID, strSchemeCode, intScheme_Seq, strSubsidize_Code, _
                         strSubsidize_Item_Code, strAvailable_Item_Code, intUnit, _
                         dblPer_Unit_Value, dblTotal_Amount, strRemark, _
                         strAvailable_Item_Desc, strAvailable_Item_Desc_Chi)

            Me.TransactionDtm = dtmTransactionDtm


        End Sub

        Private Sub Constructor(ByVal strTransactionID As String, ByVal strSchemeCode As String, ByVal intScheme_Seq As Integer, ByVal strSubsidize_Code As String, _
                        ByVal strSubsidize_Item_Code As String, ByVal strAvailable_Item_Code As String, ByVal intUnit As Nullable(Of Integer), _
                        ByVal dblPer_Unit_Value As Nullable(Of Double), ByVal dblTotal_Amount As Nullable(Of Double), ByVal strRemark As String, _
                        Optional ByVal strAvailable_Item_Desc As String = "", Optional ByVal strAvailable_Item_Desc_Chi As String = "")

            Me.TransactionID = strTransactionID
            Me.SchemeCode = strSchemeCode
            Me.SubsidizeCode = strSubsidize_Code
            Me.SchemeSeq = intScheme_Seq
            Me.SubsidizeItemCode = strSubsidize_Item_Code

            Me.AvailableItemCode = strAvailable_Item_Code
            Me.Unit = intUnit
            Me.PerUnitValue = dblPer_Unit_Value
            Me.TotalAmount = dblTotal_Amount
            Me.Remark = strRemark
            Me.Provider = ProviderClass.Private

            ' Addition Memeber
            Me.AvailableItemDesc = strAvailable_Item_Desc
            Me.AvailableItemDescChi = strAvailable_Item_Desc_Chi

            ' Handle changes trigger by some properties
            'Me.HandleSchemeCodeChange() ' Triggered when change SchemeSeq Property
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="udtHAVaccineModel"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal udtPersonalInfo As EHSPersonalInformationModel, ByVal udtHAVaccineModel As Component.HATransaction.HAVaccineModel)
            With udtHAVaccineModel

                Me.AvailableItemCode = .DoseSeqCode
                Me._strProvider = .Provider

                ' Addition Memeber
                Me.AvailableItemDesc = .DoseSeqDesc
                Me.AvailableItemDescChi = .DoseSeqDescChinese
                Me._strSubsidize_Desc = .VaccineDesc
                Me._strSubsidize_Desc_Chi = .VaccineDescChinese

                Me._strPracticeName = .Location
                Me._strPracticeNameChi = .LocationChinese

                If .OnSite = Component.HATransaction.HAVaccineModel.OnSiteClass.Y Then
                    Me._strRecordType = RecordTypeClass.OnSite
                ElseIf .OnSite = Component.HATransaction.HAVaccineModel.OnSiteClass.N Then
                    Me._strRecordType = RecordTypeClass.History
                Else
                    Throw New Exception(String.Format("TransactionDetailModel: Unhandled OnSite value ({0})", .OnSite))
                End If

                Me.ServiceReceiveDtm = .InjectionDtm
                Me._dtmTransaction_Dtm = .CreateDtm

                Me.DOB = udtPersonalInfo.DOB
                Me.ExactDOB = udtPersonalInfo.ExactDOB
                Me.PersonalInformationDemographic = New EHSPersonalInformationDemographicModel(udtPersonalInfo.DOB, _
                                                                                                udtPersonalInfo.ExactDOB, _
                                                                                                udtPersonalInfo.Gender, _
                                                                                                udtPersonalInfo.IdentityNum, _
                                                                                                udtPersonalInfo.DocCode, _
                                                                                                udtPersonalInfo.EName)
                Me.HighRisk = String.Empty
                Me.VaccineBrand = .VaccineBrand
                Me.VaccineLotNo = .VaccineLotNo
                Me.VaccineTradeName = (New COVID19.COVID19BLL).GetVaccineTradeName(.VaccineBrand)
                Me.VaccineTradeNameChi = (New COVID19.COVID19BLL).GetVaccineTradeNameChi(.VaccineBrand)

                Me.JoinEHRSS = String.Empty
                Me.ContactNo = String.Empty
                Me.NonLocalRecovered = String.Empty
                Me.MainCategory = String.Empty
                Me.SubCategory = String.Empty

            End With
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="udtDHVaccineModel"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal udtPersonalInfo As EHSPersonalInformationModel, ByVal udtDHVaccineModel As Component.DHTransaction.DHVaccineModel)
            With udtDHVaccineModel

                Me.AvailableItemCode = .DoseSeq
                Me._strProvider = ProviderClass.DH

                ' Addition Memeber
                Me.AvailableItemDesc = .DoseSeqDescEng
                Me.AvailableItemDescChi = .DoseSeqDescChi
                Me._strPracticeName = .VaccineProviderEng
                Me._strPracticeNameChi = .VaccineProviderChi

                Me._strRecordType = RecordTypeClass.OnSite

                Me.ServiceReceiveDtm = .AdmDate
                Me._dtmTransaction_Dtm = .AdmDate

                Me.DOB = udtPersonalInfo.DOB
                Me.ExactDOB = udtPersonalInfo.ExactDOB
                Me.PersonalInformationDemographic = New EHSPersonalInformationDemographicModel(udtPersonalInfo.DOB, _
                                                                                                udtPersonalInfo.ExactDOB, _
                                                                                                udtPersonalInfo.Gender, _
                                                                                                udtPersonalInfo.IdentityNum, _
                                                                                                udtPersonalInfo.DocCode, _
                                                                                                udtPersonalInfo.EName)
                Me.HighRisk = String.Empty
                Me.VaccineBrand = String.Empty
                Me.VaccineLotNo = String.Empty
                Me.VaccineTradeName = String.Empty
                Me.VaccineTradeNameChi = String.Empty

                Me.JoinEHRSS = String.Empty
                Me.ContactNo = String.Empty
                Me.NonLocalRecovered = String.Empty
                Me.MainCategory = String.Empty
                Me.SubCategory = String.Empty

            End With
        End Sub

#End Region

    End Class
End Namespace