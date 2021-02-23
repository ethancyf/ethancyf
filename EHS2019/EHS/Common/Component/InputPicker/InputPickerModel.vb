﻿Namespace Component.InputPicker

#Region "Class: InputPickerModel"
    Public Class InputPickerModel
        'Date & Time
        Private _dtmServiceDate As Date

        'String
        Private _strCategoryCode As String
        Private _strRCHCode As String
        Private _strHighRisk As String
        Private _strSchoolCode As String
        Private _strSPID As String

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _strBrand As String
        Private _udtVaccinationRecord As EHSTransaction.TransactionDetailVaccineModelCollection
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        'User Define Type
        Private _udtInputVaccineModelCollection As InputPicker.InputVaccineModelCollection



#Region "Property"

        Public Property ServiceDate() As Date
            Get
                Return _dtmServiceDate
            End Get
            Set(ByVal Value As Date)
                _dtmServiceDate = Value
            End Set
        End Property

        Public Property CategoryCode() As String
            Get
                Return _strCategoryCode
            End Get
            Set(ByVal Value As String)
                _strCategoryCode = Value
            End Set
        End Property

        Public Property RCHCode() As String
            Get
                Return _strRCHCode
            End Get
            Set(ByVal Value As String)
                _strRCHCode = Value
            End Set
        End Property

        Public Property HighRisk() As String
            Get
                Return _strHighRisk
            End Get
            Set(ByVal Value As String)
                _strHighRisk = Value
            End Set
        End Property

        Public Property SchoolCode() As String
            Get
                Return _strSchoolCode
            End Get
            Set(ByVal Value As String)
                _strSchoolCode = Value
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal Value As String)
                _strSPID = Value
            End Set
        End Property

        Public Property EHSClaimVaccine() As InputVaccineModelCollection
            Get
                Return _udtInputVaccineModelCollection
            End Get
            Set(ByVal Value As InputVaccineModelCollection)
                _udtInputVaccineModelCollection = Value
            End Set
        End Property

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property Brand() As String
            Get
                Return _strBrand
            End Get
            Set(ByVal Value As String)
                _strBrand = Value
            End Set
        End Property

        Public Property VaccinationRecord() As EHSTransaction.TransactionDetailVaccineModelCollection
            Get
                Return _udtVaccinationRecord
            End Get
            Set(ByVal Value As EHSTransaction.TransactionDetailVaccineModelCollection)
                _udtVaccinationRecord = Value
            End Set
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]
#End Region

#Region "Constructor"

        Public Sub New()
            _dtmServiceDate = Nothing
            _strCategoryCode = String.Empty
            _strRCHCode = String.Empty
            _strHighRisk = String.Empty
            _strSchoolCode = String.Empty
            _strSPID = String.Empty
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            _strBrand = String.Empty
            _udtVaccinationRecord = Nothing
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            _udtInputVaccineModelCollection = Nothing

        End Sub

#End Region

    End Class

#End Region

End Namespace

