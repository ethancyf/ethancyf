Imports Common.Component.ProfessionVoucherQuota

Namespace Component.ProfessionVoucherQuota
    <Serializable()> Public Class ProfessionVoucherQuotaModelCollection
        Inherits System.Collections.ArrayList

        Public Overloads Sub add(ByVal udtProfessionVoucherQuotaModel As ProfessionVoucherQuotaModel)
            MyBase.Add(udtProfessionVoucherQuotaModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strServiceCategoryCode As String) As ProfessionVoucherQuotaModel
            Get
                Dim intIdx As Integer
                Dim udtProfessionVoucherQuotaModel As ProfessionVoucherQuotaModel

                For intIdx = 0 To MyBase.Count - 1
                    udtProfessionVoucherQuotaModel = CType(MyBase.Item(intIdx), ProfessionVoucherQuotaModel)
                    If udtProfessionVoucherQuotaModel.ServiceCategoryCode = strServiceCategoryCode Then
                        Return udtProfessionVoucherQuotaModel
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub remove(ByVal udtProfessionVoucherQuotaModel As ProfessionVoucherQuotaModel)
            MyBase.Remove(udtProfessionVoucherQuotaModel)
        End Sub

        Public Function Filter(ByVal strServiceCategoryCode As String, ByVal dtmServiceDate As DateTime) As ProfessionVoucherQuotaModel

            Dim udtProfessionVoucherQuotaModel As ProfessionVoucherQuotaModel

            For Each udtProfessionVoucherQuotaModel In Me

                If udtProfessionVoucherQuotaModel.ServiceCategoryCode.Trim().ToUpper() = strServiceCategoryCode.Trim().ToUpper() Then

                    If udtProfessionVoucherQuotaModel.EffectiveDate <= dtmServiceDate Then
                        If udtProfessionVoucherQuotaModel.ExpiryDate.HasValue Then

                            If udtProfessionVoucherQuotaModel.ExpiryDate >= dtmServiceDate Then
                                Return udtProfessionVoucherQuotaModel
                            End If
                        Else
                            Return udtProfessionVoucherQuotaModel
                        End If
                    End If
                End If
            Next

            Return Nothing
        End Function
    End Class
End Namespace

