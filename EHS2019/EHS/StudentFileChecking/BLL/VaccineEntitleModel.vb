Imports Common.Component.EHSTransaction
Namespace BLL

    Public Class VaccineEntitleModel
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _blnEntitleOnlyDose As Boolean = False
        Private _blnEntitle1stDose As Boolean = False
        Private _blnEntitle2ndDose As Boolean = False
        Private _blnEntitle3rdDose As Nullable(Of Boolean) = False
        Private _blnEntitleInject As Boolean = False

        Private _strRemarkOnlyDose As String = String.Empty
        Private _strRemark1stDose As String = String.Empty
        Private _strRemark2ndDose As String = String.Empty
        Private _strRemark3rdDose As String = String.Empty
        Private _strEntitleInjectFailReason As String = String.Empty
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property Entitle3rdDose As Nullable(Of Boolean)
            Get
                Return _blnEntitle3rdDose
            End Get
            Set(value As Nullable(Of Boolean))
                _blnEntitle3rdDose = value
            End Set
        End Property
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property Remark3rdDose As String
            Get
                Return _strRemark3rdDose
            End Get
            Set(value As String)
                _strRemark3rdDose = value
            End Set
        End Property
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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
