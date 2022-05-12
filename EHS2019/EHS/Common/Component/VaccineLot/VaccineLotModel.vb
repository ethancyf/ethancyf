
Namespace Component.VaccineLot

    <Serializable()> Public Class VaccineLotModel
        Private _strVaccineLotNo As String
        Private _strVaccineLotID As String
        Private _strServiceType As String
        Private _strSchemeCode As String
        Private _strCentreName As String
        Private _strCentreId As String
        Private _strBrandName As String
        Private _strBrandTradeName As String
        Private _strVaccineExpiryDate As String
        Private _strEffectiveDateFrom As String
        Private _strEffectiveDateTo As String
        Private _strRecordStatus As String
        Private _strNewRecordStatus As String
        Private _strBooth As String
        Private _strBoothId As String
        Private _strNewEffectiveDateFrom As String
        Private _strNewEffectiveDateTo As String


        Private _strCreateBy As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strRequestBy As String
        Private _dtmRequestDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strApproveBy As String
        Private _dtmApproveDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()

        Private _strLotStatus As String
        Private _strRequestType As String
        Private _strUpToExpiryDtm As String


        Public Property VaccineLotID() As String
            Get
                Return _strVaccineLotID
            End Get
            Set(ByVal value As String)
                _strVaccineLotID = value
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

        Public Property CentreName() As String
            Get
                Return _strCentreName
            End Get
            Set(ByVal value As String)
                _strCentreName = value
            End Set
        End Property

        Public Property CentreId() As String
            Get
                Return _strCentreId
            End Get
            Set(ByVal value As String)
                _strCentreId = value
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

        Public Property VaccineExpiryDate() As String
            Get
                Return _strVaccineExpiryDate
            End Get
            Set(ByVal value As String)
                _strVaccineExpiryDate = value
            End Set
        End Property

        Public Property VaccineLotEffectiveDFrom() As String
            Get
                Return _strEffectiveDateFrom
            End Get
            Set(ByVal value As String)
                _strEffectiveDateFrom = value
            End Set
        End Property

        Public Property VaccineLotEffectiveDTo() As String
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

        Public Property UpToExpiryDtm() As String
            Get
                Return _strUpToExpiryDtm
            End Get
            Set(ByVal value As String)
                _strUpToExpiryDtm = value
            End Set
        End Property
        'Public Property NewRecordStatus() As String
        '    Get
        '        Return _strNewRecordStatus
        '    End Get
        '    Set(ByVal value As String)
        '        _strNewRecordStatus = value
        '    End Set
        'End Property

        Public Property Booth() As String
            Get
                Return _strBooth
            End Get
            Set(ByVal value As String)
                _strBooth = value
            End Set
        End Property

        Public Property BoothId() As String
            Get
                Return _strBoothId
            End Get
            Set(ByVal value As String)
                _strBoothId = value
            End Set
        End Property

        Public Property LotStatus() As String
            Get
                Return _strLotStatus
            End Get
            Set(ByVal value As String)
                _strLotStatus = value
            End Set
        End Property

        Public Property NewVaccineLotEffectiveDFrom() As String
            Get
                Return _strNewEffectiveDateFrom
            End Get
            Set(ByVal value As String)
                _strNewEffectiveDateFrom = value
            End Set
        End Property

        Public Property NewVaccineLotEffectiveDTo() As String
            Get
                Return _strNewEffectiveDateTo
            End Get
            Set(ByVal value As String)
                _strNewEffectiveDateTo = value
            End Set
        End Property

        'Public Property NewLotStatus() As String
        '    Get
        '        Return _strNewLotStatus
        '    End Get
        '    Set(ByVal value As String)
        '        _strNewLotStatus = value
        '    End Set
        'End Property

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

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtVaccineLotModel As VaccineLotModel)
            _strVaccineLotID = udtVaccineLotModel.VaccineLotID
            _strVaccineLotNo = udtVaccineLotModel.VaccineLotNo
            _strServiceType = udtVaccineLotModel.ServiceType
            _strSchemeCode = udtVaccineLotModel.SchemeCode
            _strCentreName = udtVaccineLotModel.CentreName
            _strCentreId = udtVaccineLotModel.CentreId
            _strBrandName = udtVaccineLotModel.BrandName
            _strVaccineExpiryDate = udtVaccineLotModel.VaccineExpiryDate
            _strEffectiveDateFrom = udtVaccineLotModel.VaccineLotEffectiveDFrom
            _strEffectiveDateTo = udtVaccineLotModel.VaccineLotEffectiveDTo
            _strRecordStatus = udtVaccineLotModel.RecordStatus
            '_strNewRecordStatus = udtVaccineLotModel.NewRecordStatus
            _strBooth = udtVaccineLotModel.Booth
            _strBoothId = udtVaccineLotModel.BoothId

            _strNewEffectiveDateFrom = udtVaccineLotModel.NewVaccineLotEffectiveDFrom
            _strNewEffectiveDateTo = udtVaccineLotModel.NewVaccineLotEffectiveDFrom


            _strCreateBy = udtVaccineLotModel.CreateBy
            _dtmCreateDtm = udtVaccineLotModel.CreateDtm
            _strRequestBy = udtVaccineLotModel.RequestBy
            _dtmRequestDtm = udtVaccineLotModel.RequestDtm
            _strUpdateBy = udtVaccineLotModel.UpdateBy
            _dtmUpdateDtm = udtVaccineLotModel.UpdateDtm
            _strApproveBy = udtVaccineLotModel.ApproveBy
            _dtmApproveDtm = udtVaccineLotModel.ApproveDtm
            _byteTSMP = udtVaccineLotModel.TSMP

            _strLotStatus = udtVaccineLotModel.LotStatus
            _strRequestType = udtVaccineLotModel.RequestType

            _strUpToExpiryDtm = udtVaccineLotModel.UpToExpiryDtm
        End Sub

        Public Sub New(ByVal strVaccineLotID As String, ByVal strVaccineLotNo As String, ByVal strServiceType As String, ByVal strSchemeCode As String, ByVal strCentreName As String, ByVal strCentreId As String, ByVal strBrandName As String, ByVal strBrandTradeName As String, ByVal strVaccineExpiryDate As String, _
                        ByVal strVaccineLotEffectiveDFrom As String, ByVal strVaccineLotEffectiveDTo As String, ByVal strRecordStatus As String, ByVal strNewRecordStatus As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal strRequestType As String, _
                         ByVal strBooth As String, ByVal strBoothId As String, ByVal strRequestBy As String, ByVal dtmRequestDtm As Nullable(Of DateTime), _
                          ByVal strNewVaccineLotEffectiveDFrom As String, ByVal strNewVaccineLotEffectiveDTo As String, _
                          ByVal strApproveBy As String, ByVal dtmApproveDtm As Nullable(Of DateTime), ByVal strUpToExpiryDtm As String)

            _strVaccineLotID = strVaccineLotID
            _strVaccineLotNo = strVaccineLotNo
            _strServiceType = strServiceType
            _strSchemeCode = strSchemeCode
            _strCentreName = strCentreName
            _strCentreId = strCentreId
            _strBrandName = strBrandName
            _strBrandTradeName = strBrandTradeName
            _strVaccineExpiryDate = strVaccineExpiryDate
            _strEffectiveDateFrom = strVaccineLotEffectiveDFrom
            _strEffectiveDateTo = strVaccineLotEffectiveDTo
            _strRecordStatus = strRecordStatus
            _strNewRecordStatus = strNewRecordStatus
            _strBooth = strBooth
            _strBoothId = strBoothId
            _strNewEffectiveDateFrom = strNewVaccineLotEffectiveDFrom
            _strNewEffectiveDateTo = strNewVaccineLotEffectiveDTo

            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strRequestBy = strRequestBy
            _dtmRequestDtm = dtmRequestDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strApproveBy = strApproveBy
            _dtmApproveDtm = dtmApproveDtm
            _byteTSMP = byteTSMP

            '_strLotStatus = strLotStatus
            _strRequestType = IIf(strRequestType Is DBNull.Value, "", strRequestType)

            _strUpToExpiryDtm = strUpToExpiryDtm
        End Sub

    End Class

End Namespace