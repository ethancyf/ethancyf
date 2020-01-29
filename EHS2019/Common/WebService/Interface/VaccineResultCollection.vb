Imports Common.Component.EHSTransaction

Namespace WebService.Interface
    <Serializable()> Public Class VaccineResultCollection

        Private _udtHAVaccineResult As HAVaccineResult
        Private _udtDHVaccineResult As DHVaccineResult
        Private _enumHAReturnStatus As VaccinationBLL.EnumVaccinationRecordReturnStatus
        Private _enumDHReturnStatus As VaccinationBLL.EnumVaccinationRecordReturnStatus

        Property HAVaccineResult() As HAVaccineResult
            Get
                Return _udtHAVaccineResult
            End Get
            Set(value As HAVaccineResult)
                _udtHAVaccineResult = value
            End Set
        End Property

        Property DHVaccineResult() As DHVaccineResult
            Get
                Return _udtDHVaccineResult
            End Get
            Set(value As DHVaccineResult)
                _udtDHVaccineResult = value
            End Set
        End Property

        Property HAReturnStatus() As VaccinationBLL.EnumVaccinationRecordReturnStatus
            Get
                Return _enumHAReturnStatus
            End Get
            Set(value As VaccinationBLL.EnumVaccinationRecordReturnStatus)
                _enumHAReturnStatus = value
            End Set
        End Property

        Property DHReturnStatus() As VaccinationBLL.EnumVaccinationRecordReturnStatus
            Get
                Return _enumDHReturnStatus
            End Get
            Set(value As VaccinationBLL.EnumVaccinationRecordReturnStatus)
                _enumDHReturnStatus = value
            End Set
        End Property

        Public Sub New()
            _udtHAVaccineResult = Nothing
            _udtDHVaccineResult = Nothing
        End Sub

    End Class
End Namespace
