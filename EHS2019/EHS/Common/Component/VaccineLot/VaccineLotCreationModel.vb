
Namespace Component.VaccineLot

    <Serializable()> Public Class VaccineLotCreationModel
        Private _strVaccineLotNo As String
        Private _strBrandName As String
        Private _strBrandId As String
        Private _strVaccineExpiryDate As String
        Private _strRecordStatus As String
        Private _strNewRecordStatus As String

        Private _strNewExpiryDate As String


        Private _strCreateBy As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strRequestBy As String
        Private _dtmRequestDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strApproveBy As String
        Private _dtmApproveDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()
        Private _strRequestType As String
        Private _strBrandTradeName As String
        Private _strLotAssignStatus As String
        Private _strNewLotAssignStatus As String



        Public Property VaccineLotNo() As String
            Get
                Return _strVaccineLotNo
            End Get
            Set(ByVal value As String)
                _strVaccineLotNo = value
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


        Public Property BrandId() As String
            Get
                Return _strBrandId
            End Get
            Set(ByVal value As String)
                _strBrandId = value
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



        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property


        Public Property NewRecordStatus() As String
            Get
                Return _strNewRecordStatus
            End Get
            Set(ByVal value As String)
                _strNewRecordStatus = value
            End Set
        End Property



        Public Property NewExpiryDate() As String
            Get
                Return _strNewExpiryDate
            End Get
            Set(ByVal value As String)
                _strNewExpiryDate = value
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


        Public Property BrandTradeName() As String
            Get
                Return _strBrandTradeName
            End Get
            Set(ByVal value As String)
                _strBrandTradeName = value
            End Set
        End Property

        Public Property NewVaccineLotAssignStatus() As String
            Get
                Return _strNewLotAssignStatus
            End Get
            Set(ByVal value As String)
                _strNewLotAssignStatus = value
            End Set
        End Property

        Public Property VaccineLotAssignStatus() As String
            Get
                Return _strLotAssignStatus
            End Get
            Set(ByVal value As String)
                _strLotAssignStatus = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtVaccineLotModel As VaccineLotCreationModel)
            _strVaccineLotNo = udtVaccineLotModel.VaccineLotNo
            _strBrandName = udtVaccineLotModel.BrandName
            _strVaccineExpiryDate = udtVaccineLotModel.VaccineExpiryDate
            _strRecordStatus = udtVaccineLotModel.RecordStatus
            _strNewRecordStatus = udtVaccineLotModel.NewRecordStatus
            _strNewExpiryDate = udtVaccineLotModel.NewExpiryDate
            _strCreateBy = udtVaccineLotModel.CreateBy
            _dtmCreateDtm = udtVaccineLotModel.CreateDtm
            _strRequestBy = udtVaccineLotModel.RequestBy
            _dtmRequestDtm = udtVaccineLotModel.RequestDtm
            _strUpdateBy = udtVaccineLotModel.UpdateBy
            _dtmUpdateDtm = udtVaccineLotModel.UpdateDtm
            _strApproveBy = udtVaccineLotModel.ApproveBy
            _dtmApproveDtm = udtVaccineLotModel.ApproveDtm
            _byteTSMP = udtVaccineLotModel.TSMP
            _strRequestType = udtVaccineLotModel.RequestType
            _strBrandTradeName = udtVaccineLotModel.BrandTradeName
            _strLotAssignStatus = udtVaccineLotModel.VaccineLotAssignStatus
            _strNewLotAssignStatus = udtVaccineLotModel.NewVaccineLotAssignStatus
        End Sub

        Public Sub New(ByVal strVaccineLotNo As String, ByVal strBrandName As String, ByVal strBrandId As String, ByVal strVaccineExpiryDate As String, _
                        ByVal strRecordStatus As String, ByVal strNewRecordStatus As String, ByVal strCreateBy As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal strRequestType As String, ByVal strRequestBy As String, _
                        ByVal dtmRequestDtm As Nullable(Of DateTime), ByVal strNewVaccineLotExpiryDate As String, ByVal strApproveBy As String, _
                        ByVal dtmApproveDtm As Nullable(Of DateTime), ByVal strBrandTradeName As String, ByVal strLotAssignStatus As String, ByVal strNewLotAssignStatus As String)

            _strVaccineLotNo = strVaccineLotNo
            _strBrandId = strBrandId
            _strBrandName = strBrandName
            _strVaccineExpiryDate = strVaccineExpiryDate
            _strRecordStatus = strRecordStatus
            _strNewRecordStatus = strNewRecordStatus
            _strNewExpiryDate = strNewVaccineLotExpiryDate

            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strRequestBy = strRequestBy
            _dtmRequestDtm = dtmRequestDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strApproveBy = strApproveBy
            _dtmApproveDtm = dtmApproveDtm
            _byteTSMP = byteTSMP


            _strRequestType = IIf(strRequestType Is DBNull.Value, "", strRequestType)
            _strBrandTradeName = strBrandTradeName
            _strLotAssignStatus = strLotAssignStatus
            _strNewLotAssignStatus = strNewLotAssignStatus
        End Sub

    End Class

End Namespace