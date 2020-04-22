Imports Common.Component.EHSTransaction
Namespace BLL

    Public Class VaccineEntitleModel
        Private _blnEntitleOnlyDose As Boolean = False
        Private _blnEntitle1stDose As Boolean = False
        Private _blnEntitle2ndDose As Boolean = False
        Private _blnEntitleInject As Boolean = False
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        Private _strRemarkOnlyDose As String = String.Empty
        Private _strRemark1stDose As String = String.Empty
        Private _strRemark2ndDose As String = String.Empty
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
        Private _strEntitleInjectFailReason As String = String.Empty
        Private _udtEHSTransaction As EHSTransactionModel = Nothing

        Public Property EntitleOnlyDose As Boolean
            Get
                Return _blnEntitleOnlyDose
            End Get
            Set(value As Boolean)
                _blnEntitleOnlyDose = value
            End Set
        End Property

        Public Property Entitle1stDose As Boolean
            Get
                Return _blnEntitle1stDose
            End Get
            Set(value As Boolean)
                _blnEntitle1stDose = value
            End Set
        End Property

        Public Property Entitle2ndDose As Boolean
            Get
                Return _blnEntitle2ndDose
            End Get
            Set(value As Boolean)
                _blnEntitle2ndDose = value
            End Set
        End Property

        Public Property EntitleInject As Boolean
            Get
                Return _blnEntitleInject
            End Get
            Set(value As Boolean)
                _blnEntitleInject = value
            End Set
        End Property

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]

        Public Property RemarkOnlyDose As String
            Get
                Return _strRemarkOnlyDose
            End Get
            Set(value As String)
                _strRemarkOnlyDose = value
            End Set
        End Property

        Public Property Remark1stDose As String
            Get
                Return _strRemark1stDose
            End Get
            Set(value As String)
                _strRemark1stDose = value
            End Set
        End Property

        Public Property Remark2ndDose As String
            Get
                Return _strRemark2ndDose
            End Get
            Set(value As String)
                _strRemark2ndDose = value
            End Set
        End Property

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

        Public Property EntitleInjectFailReason As String
            Get
                Return _strEntitleInjectFailReason
            End Get
            Set(value As String)
                _strEntitleInjectFailReason = value
            End Set
        End Property

        Public Property EHSTransaction As EHSTransactionModel
            Get
                Return _udtEHSTransaction
            End Get
            Set(value As EHSTransactionModel)
                _udtEHSTransaction = value
            End Set
        End Property

        Public Sub New()
        End Sub

    End Class
End Namespace
