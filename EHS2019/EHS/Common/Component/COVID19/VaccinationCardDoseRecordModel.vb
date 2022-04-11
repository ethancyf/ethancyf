Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.RVPHomeList
Imports Common.Component.SchemeDetails

Namespace Component.COVID19

#Region "Class: VaccinationCardDoseRecordModel"
    <Serializable()> Public Class VaccinationCardDoseRecordModel

#Region "Constant"
        Public Class VaccineCode
            Public Const NDI = "NDI" 'No detailed information
        End Class

#End Region

#Region "Private Memeber"
        Private _strAvailableItemCode As String
        Private _strTransaction_ID As String
        Private _strVaccine_Lot_No As String
        Private _strVaccine_Name As String
        Private _strVaccine_Name_Chi As String
        Private _strVaccine_Manufacturer As String
        Private _strVaccine_CodeForQRCode As String
        Private _dtmInjection_Date As Date
        Private _strVaccination_Center As String
        Private _strVaccination_Center_Chi As String
        Private _blnNon_Local_Recovered_History As Boolean

#End Region

#Region "Property"

        Public Property AvailableItemCode() As String
            Get
                Return _strAvailableItemCode
            End Get
            Set(ByVal Value As String)
                _strAvailableItemCode = Value
            End Set
        End Property

        Public Property TransactionID() As String
            Get
                Return _strTransaction_ID
            End Get
            Set(ByVal Value As String)
                _strTransaction_ID = Value
            End Set
        End Property

        Public Property VaccineLotNo() As String
            Get
                Return _strVaccine_Lot_No
            End Get
            Set(ByVal Value As String)
                _strVaccine_Lot_No = Value
            End Set
        End Property

        Public Property VaccineName() As String
            Get
                Return _strVaccine_Name
            End Get
            Set(ByVal Value As String)
                _strVaccine_Name = Value
            End Set
        End Property

        Public Property VaccineNameChi() As String
            Get
                Return _strVaccine_Name_Chi
            End Get
            Set(ByVal Value As String)
                _strVaccine_Name_Chi = Value
            End Set
        End Property

        Public Property VaccineManufacturer() As String
            Get
                Return _strVaccine_Manufacturer
            End Get
            Set(ByVal Value As String)
                _strVaccine_Manufacturer = Value
            End Set
        End Property

        Public Property VaccineCodeForQRCode() As String
            Get
                Return _strVaccine_CodeForQRCode
            End Get
            Set(ByVal Value As String)
                _strVaccine_CodeForQRCode = Value
            End Set
        End Property

        Public Property InjectionDate() As Date
            Get
                Return _dtmInjection_Date
            End Get
            Set(ByVal Value As Date)
                _dtmInjection_Date = Value
            End Set
        End Property

        Public Property VaccinationCentre() As String
            Get
                Return _strVaccination_Center
            End Get
            Set(ByVal Value As String)
                _strVaccination_Center = Value
            End Set
        End Property

        Public Property VaccinationCentreChi() As String
            Get
                Return _strVaccination_Center_Chi
            End Get
            Set(ByVal Value As String)
                _strVaccination_Center_Chi = Value
            End Set
        End Property

        Public Property NonLocalRecoveredHistory() As Boolean
            Get
                Return _blnNon_Local_Recovered_History
            End Get
            Set(ByVal Value As Boolean)
                _blnNon_Local_Recovered_History = Value
            End Set
        End Property

        ' CRE20-023-80 (COVID19 - 4th Dose) [Start][Winnie SUEN]
        ' -------------------------------------------------------------
        Public ReadOnly Property DoseSeq() As Integer
            Get
                Dim intDoseSeq As Integer = 0
                Dim udtSchemeDetailBLL As New SchemeDetails.SchemeDetailBLL
                Dim udtC19SubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = udtSchemeDetailBLL.getSubsidizeItemDetails(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

                'Use [SubsidizeItemDetails].[DisplaySeq] as the DoseSeq
                For Each udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel In udtC19SubsidizeItemDetailList
                    If udtSubsidizeItemDetailsModel.AvailableItemCode.Trim().ToUpper().Equals(Me.AvailableItemCode.Trim().ToUpper()) Then
                        intDoseSeq = udtSubsidizeItemDetailsModel.DisplaySeq
                        Exit For
                    End If
                Next

                Return intDoseSeq
            End Get
        End Property
        ' CRE20-023-80 (COVID19 - 4th Dose) [End][Winnie SUEN]
#End Region

#Region "Constructor"

        Public Sub New(ByVal _udtEHSTransaction As EHSTransactionModel)
            Dim udtCOVID19BLL As New COVID19.COVID19BLL

            Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
            Dim dtVaccine As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

            Me.AvailableItemCode = _udtEHSTransaction.TransactionDetails(0).AvailableItemCode.Trim().ToUpper()
            Me.TransactionID = _udtEHSTransaction.TransactionID

            ' Vaccine
            Me.VaccineLotNo = dtVaccine.Rows(0)("Vaccine_Lot_No")
            Me.VaccineName = dtVaccine.Rows(0)("Brand_Printout_Name")
            Me.VaccineNameChi = dtVaccine.Rows(0)("Brand_Printout_Name_Chi")
            Me.VaccineManufacturer = dtVaccine.Rows(0)("Manufacturer")
            Me.VaccineCodeForQRCode = getVaccineCode(IIf(IsDBNull(dtVaccine.Rows(0)("Brand_Printout_Vaccine_Code")), Nothing, dtVaccine.Rows(0)("Brand_Printout_Vaccine_Code").ToString))

            Me.InjectionDate = _udtEHSTransaction.ServiceDate

            ' Vaccine Centre
            Dim strVaccinationCentre As String = String.Empty
            Dim strVaccinationCentreChi As String = String.Empty

            getVaccinationCentreForEHS(_udtEHSTransaction, strVaccinationCentre, strVaccinationCentreChi)
            Me.VaccinationCentre = strVaccinationCentre
            Me.VaccinationCentreChi = strVaccinationCentreChi

            ' Non local Recovered
            If _udtEHSTransaction IsNot Nothing AndAlso _
                _udtEHSTransaction.TransactionAdditionFields IsNot Nothing AndAlso _
                _udtEHSTransaction.TransactionAdditionFields.NonLocalRecoveredHistory IsNot Nothing AndAlso _
                _udtEHSTransaction.TransactionAdditionFields.NonLocalRecoveredHistory = YesNo.Yes Then

                Me._blnNon_Local_Recovered_History = True
            End If
        End Sub

        Public Sub New(ByVal _udtVaccinationRecordHistory As TransactionDetailVaccineModel)
            Dim udtCOVID19BLL As New COVID19.COVID19BLL

            Me.AvailableItemCode = _udtVaccinationRecordHistory.AvailableItemCode.Trim().ToUpper()
            Me.TransactionID = _udtVaccinationRecordHistory.TransactionID

            ' Vaccine
            Me.VaccineLotNo = _udtVaccinationRecordHistory.VaccineLotNo
            Me.VaccineName = udtCOVID19BLL.GetVaccineBrandPrintoutName(_udtVaccinationRecordHistory.VaccineBrand)
            Me.VaccineNameChi = udtCOVID19BLL.GetVaccineBrandPrintoutNameChi(_udtVaccinationRecordHistory.VaccineBrand)
            Me.VaccineManufacturer = udtCOVID19BLL.GetVaccineManufacturer(_udtVaccinationRecordHistory.VaccineBrand)
            Me.VaccineCodeForQRCode = getVaccineCode(udtCOVID19BLL.GetVaccineCodeForQRCode(_udtVaccinationRecordHistory.VaccineBrand))

            Me.InjectionDate = _udtVaccinationRecordHistory.ServiceReceiveDtm

            ' Vaccine Centre
            If _udtVaccinationRecordHistory.TransactionID IsNot Nothing AndAlso _udtVaccinationRecordHistory.TransactionID <> String.Empty Then
                'EHS Transaction
                Dim udtEHSTransactionBLL As New EHSTransactionBLL
                Dim udtHistoryEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(_udtVaccinationRecordHistory.TransactionID)

                Dim strVaccinationCentre As String = String.Empty
                Dim strVaccinationCentreChi As String = String.Empty
                getVaccinationCentreForEHS(udtHistoryEHSTransaction, strVaccinationCentre, strVaccinationCentreChi)

                Me.VaccinationCentre = strVaccinationCentre
                Me.VaccinationCentreChi = strVaccinationCentreChi

            Else
                'CMS/CMIS Vaccination Record
                Me.VaccinationCentre = _udtVaccinationRecordHistory.PracticeName
                Me.VaccinationCentreChi = _udtVaccinationRecordHistory.PracticeNameChi
            End If

            ' Non local Recovered
            If _udtVaccinationRecordHistory.NonLocalRecovered IsNot Nothing Then
                If _udtVaccinationRecordHistory.NonLocalRecovered = YesNo.Yes Then
                    Me.NonLocalRecoveredHistory = True
                End If
            End If
        End Sub

        Private Sub getVaccinationCentreForEHS(ByVal _udtEHSTransaction As EHSTransactionModel, _
                                               ByRef strVaccinationCentre As String, _
                                               ByRef strVaccinationCentreChi As String)

            Dim udtCOVID19BLL As New COVID19.COVID19BLL

            Select Case _udtEHSTransaction.SchemeCode.Trim.ToUpper()
                Case SchemeClaimModel.COVID19CVC, SchemeClaimModel.COVID19DH,
                    SchemeClaimModel.COVID19SR, SchemeClaimModel.COVID19SB
                    'booth and centre 
                    Dim dtVaccineCenter As DataTable = udtCOVID19BLL.GetCOVID19VaccineCentreBySPIDPracticeDisplaySeq(_udtEHSTransaction.ServiceProviderID, _udtEHSTransaction.PracticeID)
                    strVaccinationCentre = dtVaccineCenter.Rows(0)("Centre_Name")
                    strVaccinationCentreChi = dtVaccineCenter.Rows(0)("Centre_Name_Chi")

                Case SchemeClaimModel.COVID19RVP, SchemeClaimModel.RVP

                    If _udtEHSTransaction.TransactionAdditionFields.OutreachType IsNot Nothing AndAlso _udtEHSTransaction.TransactionAdditionFields.OutreachType = TYPE_OF_OUTREACH.OTHER Then
                        'Outreach
                        Dim strOutreachCode As String = _udtEHSTransaction.TransactionAdditionFields.OutreachCode
                        Dim dtOutreach As DataTable = (New Component.COVID19.OutreachListBLL).GetOutreachListByCode(strOutreachCode)
                        strVaccinationCentre = dtOutreach.Rows(0)("Outreach_Name_Eng").ToString().Trim()
                        strVaccinationCentreChi = dtOutreach.Rows(0)("Outreach_Name_Chi").ToString().Trim()

                    Else
                        'RCH
                        Dim strRCHCode As String = _udtEHSTransaction.TransactionAdditionFields.RCHCode
                        Dim udtRVP As RVPHomeListModel = (New Component.RVPHomeList.RVPHomeListBLL()).GetRVPHomeListModalByCode(strRCHCode)
                        strVaccinationCentre = udtRVP.HomenameEng
                        strVaccinationCentreChi = udtRVP.HomenameChi
                    End If

                Case SchemeClaimModel.COVID19OR
                    'Outreach
                    Dim strOutreachCode As String = _udtEHSTransaction.TransactionAdditionFields.OutreachCode
                    If strOutreachCode IsNot Nothing AndAlso strOutreachCode <> String.Empty Then
                        'Outreach
                        Dim dtOutreach As DataTable = (New Component.COVID19.OutreachListBLL).GetOutreachListByCode(strOutreachCode)
                        strVaccinationCentre = dtOutreach.Rows(0)("Outreach_Name_Eng")
                        strVaccinationCentreChi = dtOutreach.Rows(0)("Outreach_Name_Chi")
                    Else
                        'Practice
                        strVaccinationCentre = _udtEHSTransaction.PracticeName
                        strVaccinationCentreChi = _udtEHSTransaction.PracticeNameChi
                    End If

                Case SchemeClaimModel.VSS

                    Dim strOutreachCode As String = _udtEHSTransaction.TransactionAdditionFields.OutreachCode
                    If strOutreachCode IsNot Nothing AndAlso strOutreachCode <> String.Empty Then
                        'Outreach (Non-Clinic)
                        Dim dtOutreach As DataTable = (New Component.COVID19.OutreachListBLL).GetOutreachListByCode(strOutreachCode)
                        strVaccinationCentre = dtOutreach.Rows(0)("Outreach_Name_Eng")
                        strVaccinationCentreChi = dtOutreach.Rows(0)("Outreach_Name_Chi")
                    Else
                        'Practice
                        strVaccinationCentre = _udtEHSTransaction.PracticeName
                        strVaccinationCentreChi = _udtEHSTransaction.PracticeNameChi
                    End If

                Case Else
                    ' Default Practice
                    strVaccinationCentre = _udtEHSTransaction.PracticeName
                    strVaccinationCentreChi = _udtEHSTransaction.PracticeNameChi
            End Select

        End Sub

        Private Function getVaccineCode(ByVal strVaccineCode As String) As String
            If strVaccineCode Is Nothing OrElse strVaccineCode = String.Empty Then
                strVaccineCode = VaccineCode.NDI

            End If

            Return strVaccineCode

        End Function

#End Region

    End Class

#End Region

End Namespace

