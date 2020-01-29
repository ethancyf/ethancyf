Namespace Component.Scheme
    <Serializable()> Public Class SchemeBackOfficeModelCollection
        Inherits System.Collections.ArrayList

        Private udtGF As ComFunction.GeneralFunction = New ComFunction.GeneralFunction

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeBackOfficeModel As SchemeBackOfficeModel)
            MyBase.Add(udtSchemeBackOfficeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeBackOfficeModel As SchemeBackOfficeModel)
            MyBase.Remove(udtSchemeBackOfficeModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeBackOfficeModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeBackOfficeModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String) As SchemeBackOfficeModel
            Dim udtSchemeBackOfficeModel As SchemeBackOfficeModel = Nothing
            Dim dtmDate As DateTime

            dtmDate = udtGF.GetSystemDateTime()

            For Each udtSchemeBackOfficeM As SchemeBackOfficeModel In Me
                If udtSchemeBackOfficeM.SchemeCode.Trim.Equals(strSchemeCode.Trim) AndAlso DateTime.Compare(dtmDate, udtSchemeBackOfficeM.EffectiveDtm) >= 0 AndAlso DateTime.Compare(dtmDate, udtSchemeBackOfficeM.ExpiryDtm) < 0 Then
                    udtSchemeBackOfficeModel = udtSchemeBackOfficeM
                End If
            Next

            Return udtSchemeBackOfficeModel
        End Function

        Public Function FilterByEffectiveExpiryDate(ByVal dtmDate As DateTime) As SchemeBackOfficeModelCollection
            Dim udtSchemeBackOfficeModelList As New SchemeBackOfficeModelCollection

            For Each udtSchemeBackOfficeM As SchemeBackOfficeModel In Me
                If DateTime.Compare(dtmDate, udtSchemeBackOfficeM.EffectiveDtm) >= 0 AndAlso DateTime.Compare(dtmDate, udtSchemeBackOfficeM.ExpiryDtm) < 0 Then
                    udtSchemeBackOfficeModelList.Add(udtSchemeBackOfficeM)
                End If
            Next

            If udtSchemeBackOfficeModelList.Count = 0 Then
                udtSchemeBackOfficeModelList = Nothing
            End If

            Return udtSchemeBackOfficeModelList
        End Function

        Public Function FilterByProfCode(ByVal strProfCode As String) As SchemeBackOfficeModelCollection
            Dim udtSchemeBackOfficeModelList As New SchemeBackOfficeModelCollection

            For Each udtSchemeBackOfficeM As SchemeBackOfficeModel In Me
                ' CRE13-001 - EHAPP [Start][Koala]
                ' -------------------------------------------------------------------------------------
                If udtSchemeBackOfficeM.EligibleProfesional(strProfCode) Then
                    udtSchemeBackOfficeModelList.Add(udtSchemeBackOfficeM)
                End If
                'If udtSchemeBackOfficeM.EligibleProfesionalString.Trim.Equals(ServiceCategoryCode.ALL) Or _
                '    udtSchemeBackOfficeM.EligibleProfesionalString.Trim.Equals(strProfCode) Then
                '    udtSchemeBackOfficeModelList.Add(udtSchemeBackOfficeM)
                'End If
                ' CRE13-001 - EHAPP [End][Koala]
            Next

            If udtSchemeBackOfficeModelList.Count = 0 Then
                udtSchemeBackOfficeModelList = Nothing
            End If

            Return udtSchemeBackOfficeModelList
        End Function

        Public Function FilterLastEffective(ByVal strSchemeCode As String, ByVal dtmServiceDate As Date) As SchemeBackOfficeModel
            Dim udtResSchemeBackOfficeModel As SchemeBackOfficeModel = Nothing

            For Each udtSchemeBackOfficeModel As SchemeBackOfficeModel In Me
                If udtSchemeBackOfficeModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    If udtSchemeBackOfficeModel.EffectiveDtm <= dtmServiceDate OrElse udtSchemeBackOfficeModel.ExpiryDtm < dtmServiceDate Then
                        If udtResSchemeBackOfficeModel Is Nothing Then
                            udtResSchemeBackOfficeModel = udtSchemeBackOfficeModel
                        Else
                            If udtResSchemeBackOfficeModel.ExpiryDtm < udtSchemeBackOfficeModel.ExpiryDtm Then
                                udtResSchemeBackOfficeModel = udtSchemeBackOfficeModel
                            End If
                        End If
                    End If
                End If
            Next
            Return udtResSchemeBackOfficeModel
        End Function
    End Class
End Namespace


