Namespace Model.DoctorList

#Region "Class Vaccine"
    <Serializable()> Public Class vaccine

#Region "Private Members"
        Private _strVaccineCode As String
        Private _strVaccineServiceFeeProvided As String
        Private _strVaccineServiceFee As String
        Private _strVaccineServiceFeeRemarkEN As String
        Private _strVaccineServiceFeeRemarkTC As String

#End Region

#Region "Properties"
        ''' <summary>
        ''' Vaccine Code
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_code() As String
            Get
                Return _strVaccineCode
            End Get
            Set(value As String)
                _strVaccineCode = value
            End Set
        End Property

        ''' <summary>
        ''' Service Fee Provided
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_service_fee_provided() As String
            Get
                Return _strVaccineServiceFeeProvided
            End Get
            Set(value As String)
                _strVaccineServiceFeeProvided = value
            End Set
        End Property

        ''' <summary>
        ''' Service Fee
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_service_fee() As String
            Get
                Return _strVaccineServiceFee
            End Get
            Set(value As String)
                _strVaccineServiceFee = value
            End Set
        End Property

        ''' <summary>
        ''' English Service Fee Remark
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_service_fee_remark_en() As String
            Get
                Return _strVaccineServiceFeeRemarkEN
            End Get
            Set(value As String)
                _strVaccineServiceFeeRemarkEN = value
            End Set
        End Property

        ''' <summary>
        ''' Traditional Chinese Service Fee Remark
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property vaccine_service_fee_remark_tc() As String
            Get
                Return _strVaccineServiceFeeRemarkTC
            End Get
            Set(value As String)
                _strVaccineServiceFeeRemarkTC = value
            End Set
        End Property

#End Region

#Region "Supported Functions"
        Public Function Copy() As vaccine
            Dim udtVaccine As vaccine = New vaccine
            udtVaccine.vaccine_code = Me.vaccine_code
            udtVaccine.vaccine_service_fee_provided = Me.vaccine_service_fee_provided
            udtVaccine.vaccine_service_fee = Me.vaccine_service_fee
            udtVaccine.vaccine_service_fee_remark_en = Me.vaccine_service_fee_remark_en
            udtVaccine.vaccine_service_fee_remark_tc = Me.vaccine_service_fee_remark_tc

            Return udtVaccine

        End Function
#End Region

    End Class
#End Region

#Region "Class VaccineModelCollection"
    <Serializable()> Public Class VaccineModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtAdd As vaccine)
            MyBase.Add(udtAdd)
        End Sub

        Public Overloads Sub Remove(ByVal udtRemove As vaccine)
            MyBase.Remove(udtRemove)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As vaccine
            Get
                Return CType(MyBase.Item(intIndex), vaccine)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New VaccineModelCollection
            For Each udtReturn As vaccine In Me
                udtReturnCollection.Add(udtReturn.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
#End Region

End Namespace