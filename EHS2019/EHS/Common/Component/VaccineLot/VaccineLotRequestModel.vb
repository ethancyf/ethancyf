
Namespace Component.VaccineLot

    <Serializable()> Public Class VaccineLotRequestModel
        Private _strRequestID As String
        Private _strVaccineLotNo As String
        Private _strCentreName As String
        Private _strBooth As String
        Private _strBrandName As String
        Private _strBrandTradeName As String

        Private _strServiceType As String
        Private _strSchemeCode As String

        Private _strExpiryDate As String
        Private _strEffectiveDateFrom As String
        Private _strEffectiveDateTo As String

        Private _strRequestType As String
        Private _strRecordStatus As String

        Private _strCreateBy As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strRequestBy As String
        Private _dtmRequestDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strApproveBy As String
        Private _dtmApproveDtm As Nullable(Of DateTime)

        Private _strRejectedBy As String
        Private _dtmRejectedDtm As Nullable(Of DateTime)

        Private _byteTSMP As Byte()
        Private _strUpToExpiryDtm As String



        Public Property RequestID() As String
            Get
                Return _strRequestID
            End Get
            Set(ByVal value As String)
                _strRequestID = value
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

        Public Property CentreName() As String
            Get
                Return _strCentreName
            End Get
            Set(ByVal value As String)
                _strCentreName = value
            End Set
        End Property

        Public Property Booth() As String
            Get
                Return _strBooth
            End Get
            Set(ByVal value As String)
                _strBooth = value
            End Set
        End Property

        Public Property BrandName() As String
            Get
                Return _strBrandName
            End Get
            Set(ByVal value As String)
                _strBrandName = value
            End Set
        End Property

        Public Property BrandTradeName() As String
            Get
                Return _strBrandTradeName
            End Get
            Set(ByVal value As String)
                _strBrandTradeName = value
            End Set
        End Property

        Public Property ServiceType() As String
            Get
                Return _strServiceType
            End Get
            Set(ByVal value As String)
                _strServiceType = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public Property ExpiryDate() As String
            Get
                Return _strExpiryDate
            End Get
            Set(ByVal value As String)
                _strExpiryDate = value
            End Set
        End Property

        Public Property EffectiveFrom() As String
            Get
                Return _strEffectiveDateFrom
            End Get
            Set(ByVal value As String)
                _strEffectiveDateFrom = value
            End Set
        End Property

        Public Property EffectiveTo() As String
            Get
                Return _strEffectiveDateTo
            End Get
            Set(ByVal value As String)
                _strEffectiveDateTo = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property

        Public Property CreateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmCreateDtm = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Property RequestDtm() As Nullable(Of DateTime)
            Get
                Return _dtmRequestDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmRequestDtm = value
            End Set
        End Property

        Public Property RequestBy() As String
            Get
                Return _strRequestBy
            End Get
            Set(ByVal value As String)
                _strRequestBy = value
            End Set
        End Property

        Public Property UpdateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmUpdateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property ApproveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmApproveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmApproveDtm = value
            End Set
        End Property

        Public Property ApproveBy() As String
            Get
                Return _strApproveBy
            End Get
            Set(ByVal value As String)
                _strApproveBy = value
            End Set
        End Property

        Public Property RejectedDtm() As Nullable(Of DateTime)
            Get
                Return _dtmRejectedDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmRejectedDtm = value
            End Set
        End Property

        Public Property RejectedBy() As String
            Get
                Return _strRejectedBy
            End Get
            Set(ByVal value As String)
                _strRejectedBy = value
            End Set
        End Property
        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property

        Public Property RequestType() As String
            Get
                Return _strRequestType
            End Get
            Set(ByVal value As String)
                _strRequestType = value
            End Set
        End Property

        Public Property UpToExpiryDtm() As String
            Get
                Return _strUpToExpiryDtm
            End Get
            Set(ByVal value As String)
                _strUpToExpiryDtm = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtVaccineLotRequestModel As VaccineLotRequestModel)
            _strRequestID = udtVaccineLotRequestModel.RequestID
            _strVaccineLotNo = udtVaccineLotRequestModel.VaccineLotNo
            _strCentreName = udtVaccineLotRequestModel.CentreName
            _strBooth = udtVaccineLotRequestModel.Booth
            _strBrandName = udtVaccineLotRequestModel.BrandName
            _strServiceType = udtVaccineLotRequestModel.ServiceType
            _strSchemeCode = udtVaccineLotRequestModel.SchemeCode
            _strExpiryDate = udtVaccineLotRequestModel.ExpiryDate
            _strRecordStatus = udtVaccineLotRequestModel.RecordStatus
            _strEffectiveDateFrom = udtVaccineLotRequestModel.EffectiveFrom
            _strEffectiveDateTo = udtVaccineLotRequestModel.EffectiveTo
            _strCreateBy = udtVaccineLotRequestModel.CreateBy
            _dtmCreateDtm = udtVaccineLotRequestModel.CreateDtm
            _strRequestBy = udtVaccineLotRequestModel.RequestBy
            _dtmRequestDtm = udtVaccineLotRequestModel.RequestDtm
            _strUpdateBy = udtVaccineLotRequestModel.UpdateBy
            _dtmUpdateDtm = udtVaccineLotRequestModel.UpdateDtm
            _strApproveBy = udtVaccineLotRequestModel.ApproveBy
            _dtmApproveDtm = udtVaccineLotRequestModel.ApproveDtm
            _strRejectedBy = udtVaccineLotRequestModel.RejectedBy
            _dtmRejectedDtm = udtVaccineLotRequestModel.RejectedDtm
            _byteTSMP = udtVaccineLotRequestModel.TSMP
            _strRequestType = udtVaccineLotRequestModel.RequestType
            _strUpToExpiryDtm = udtVaccineLotRequestModel.UpToExpiryDtm
            _strBrandTradeName = udtVaccineLotRequestModel.BrandTradeName
        End Sub

        Public Sub New(ByVal strRequestID As String, ByVal strVaccineLotNo As String, ByVal strServiceType As String, ByVal strSchemeCode As String, ByVal strCentreName As String, ByVal strBooth As String, ByVal strBrandName As String, ByVal strExpiryDate As String, _
                        ByVal strEffectiveFrom As String, ByVal strEffectiveTo As String, ByVal strRecordStatus As String, ByVal strRequestType As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal strRequestBy As String, _
                        ByVal dtmRequestDtm As Nullable(Of DateTime), ByVal strApproveBy As String, ByVal dtmApproveDtm As Nullable(Of DateTime), _
                        ByVal strRejectedBy As String, ByVal dtmRejectedDtm As Nullable(Of DateTime), ByVal strUpToExpiryDtm As String, ByVal strBrandTradeName As String)

            _strRequestID = strRequestID
            _strVaccineLotNo = strVaccineLotNo
            _strServiceType = strServiceType
            _strSchemeCode = strSchemeCode
            _strCentreName = strCentreName
            _strBooth = strBooth
            _strBrandName = strBrandName
            _strExpiryDate = strExpiryDate
            _strRecordStatus = strRecordStatus
            _strEffectiveDateFrom = strEffectiveFrom
            _strEffectiveDateTo = strEffectiveTo

            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strRequestBy = strRequestBy
            _dtmRequestDtm = dtmRequestDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strApproveBy = strApproveBy
            _dtmApproveDtm = dtmApproveDtm
            _strRejectedBy = strRejectedBy
            _dtmRejectedDtm = dtmRejectedDtm
            _byteTSMP = byteTSMP
            _strUpToExpiryDtm = strUpToExpiryDtm

            _strRequestType = IIf(strRequestType Is DBNull.Value, "", strRequestType)

            _strBrandTradeName = strBrandTradeName
        End Sub

    End Class

End Namespace