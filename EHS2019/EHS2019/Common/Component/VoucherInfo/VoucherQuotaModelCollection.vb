Namespace Component.VoucherInfo
    <Serializable()> Public Class VoucherQuotaModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtVoucherQuotaModel As VoucherQuotaModel)
            MyBase.Add(udtVoucherQuotaModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtVoucherQuotaModel As VoucherQuotaModel)
            MyBase.Remove(udtVoucherQuotaModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VoucherQuotaModel
            Get
                Return CType(MyBase.Item(intIndex), VoucherQuotaModel)
            End Get
        End Property

        Public Function Filter(ByVal strProfCode As String) As VoucherQuotaModel
            Dim udtResVoucherQuota As VoucherQuotaModel = Nothing
            Dim dtmCurrentDate As Date = (New Common.ComFunction.GeneralFunction).GetSystemDateTime.Date

            For Each udtVoucherQuotaModel As VoucherQuotaModel In Me
                If udtVoucherQuotaModel.ProfCode.Trim().ToUpper() = strProfCode.Trim().ToUpper() AndAlso _
                    udtVoucherQuotaModel.PeriodStartDtm <= dtmCurrentDate And dtmCurrentDate < udtVoucherQuotaModel.PeriodEndDtm Then

                    udtResVoucherQuota = New VoucherQuotaModel(udtVoucherQuotaModel)
                    Exit For
                End If
            Next

            Return udtResVoucherQuota

        End Function

        Public Function FilterByEffectiveDtm(ByVal dtmEffectiveDate As Date) As VoucherQuotaModelCollection
            Dim udtResVoucherQuotaList As New VoucherQuotaModelCollection

            For Each udtVoucherQuotaModel As VoucherQuotaModel In Me
                If udtVoucherQuotaModel.PeriodStartDtm <= dtmEffectiveDate And dtmEffectiveDate < udtVoucherQuotaModel.PeriodEndDtm Then

                    udtResVoucherQuotaList.Add(udtVoucherQuotaModel)
                End If
            Next

            Return udtResVoucherQuotaList

        End Function

        Public Function FilterByProfCodeEffectiveDtm(ByVal strProfCode As String, ByVal dtmEffectiveDate As Date) As VoucherQuotaModel
            Dim udtResVoucherQuota As VoucherQuotaModel = Nothing

            For Each udtVoucherQuotaModel As VoucherQuotaModel In Me
                If udtVoucherQuotaModel.ProfCode.Trim().ToUpper() = strProfCode.Trim().ToUpper() AndAlso _
                    udtVoucherQuotaModel.PeriodStartDtm <= dtmEffectiveDate And dtmEffectiveDate <= udtVoucherQuotaModel.PeriodEndDtm Then

                    udtResVoucherQuota = New VoucherQuotaModel(udtVoucherQuotaModel)
                    Exit For
                End If
            Next

            Return udtResVoucherQuota

        End Function

        Public Function Copy() As VoucherQuotaModelCollection
            Dim udtResVoucherQuotaList As New VoucherQuotaModelCollection

            For Each udtVoucherQuota As VoucherQuotaModel In Me
                udtResVoucherQuotaList.Add(New VoucherQuotaModel(udtVoucherQuota))
            Next

            Return udtResVoucherQuotaList

        End Function

    End Class

End Namespace