Imports Common.Component.Scheme.SchemeClaimModel
Imports System.Globalization

Namespace Component.Scheme
    <Serializable()> Public Class SchemeClaimModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeClaimModel As SchemeClaimModel)
            MyBase.Add(udtSchemeClaimModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeClaimModel As SchemeClaimModel)
            MyBase.Remove(udtSchemeClaimModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeClaimModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeClaimModel)
            End Get
        End Property

        ''' <summary>
        ''' Filter by current datetime, return SchemeClaim List with valid claim period
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FilterByClaimPeriod(ByVal dtmCurrentDate As DateTime) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelCollection As SchemeClaimModelCollection = New SchemeClaimModelCollection()
            Dim udtResSchemeClaimModel As SchemeClaimModel = Nothing

            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.ClaimPeriodFrom <= dtmCurrentDate AndAlso dtmCurrentDate < udtSchemeClaimModel.ClaimPeriodTo Then
                    udtResSchemeClaimModel = New SchemeClaimModel(udtSchemeClaimModel)
                    udtResSchemeClaimModelCollection.Add(udtResSchemeClaimModel)
                End If
            Next
            Return udtResSchemeClaimModelCollection
        End Function

        ''' <summary>
        ''' Filter by current datetime, return SchemeClaim List with valid claim period
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FilterByEffectivePeriod(ByVal dtmCurrentDate As DateTime) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelCollection As SchemeClaimModelCollection = New SchemeClaimModelCollection()
            Dim udtResSchemeClaimModel As SchemeClaimModel = Nothing

            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.EffectiveDtm <= dtmCurrentDate AndAlso dtmCurrentDate < udtSchemeClaimModel.ExpiryDtm Then
                    udtResSchemeClaimModel = New SchemeClaimModel(udtSchemeClaimModel)
                    udtResSchemeClaimModelCollection.Add(udtResSchemeClaimModel)
                End If
            Next
            Return udtResSchemeClaimModelCollection
        End Function

        ''' <summary>
        ''' Filter by Scheme Code, Return the Last match
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String) As SchemeClaimModel
            Dim udtResSchemeClaimModel As SchemeClaimModel = Nothing
            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    udtResSchemeClaimModel = udtSchemeClaimModel
                End If
            Next
            Return udtResSchemeClaimModel
        End Function

        ''' <summary>
        ''' Filter By Key
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FilterKey(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer) As SchemeClaimModel
            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso udtSchemeClaimModel.SchemeSeq = intSchemeSeq Then
                    Return udtSchemeClaimModel
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Filter By Scheme Code and return List
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FilterList(ByVal strSchemeCode As String) As SchemeClaimModelCollection

            Dim udtResSchemeClaimModelCollection As SchemeClaimModelCollection = New SchemeClaimModelCollection()
            Dim udtResSchemeClaimModel As SchemeClaimModel = Nothing

            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    udtResSchemeClaimModel = New SchemeClaimModel(udtSchemeClaimModel)
                    udtResSchemeClaimModelCollection.Add(udtResSchemeClaimModel)
                End If
            Next
            Return udtResSchemeClaimModelCollection
        End Function

        Public Function FilterLastEffective(ByVal strSchemeCode As String, ByVal dtmServiceDate As Date) As SchemeClaimModel
            Dim udtResSchemeClaimModel As SchemeClaimModel = Nothing

            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    If udtSchemeClaimModel.EffectiveDtm <= dtmServiceDate OrElse udtSchemeClaimModel.ExpiryDtm < dtmServiceDate Then
                        If udtResSchemeClaimModel Is Nothing Then
                            udtResSchemeClaimModel = udtSchemeClaimModel
                        Else
                            If udtResSchemeClaimModel.ExpiryDtm < udtSchemeClaimModel.ExpiryDtm Then
                                udtResSchemeClaimModel = udtSchemeClaimModel
                            End If
                        End If
                    End If
                End If
            Next
            Return udtResSchemeClaimModel
        End Function

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        ''' <summary>
        ''' Filter by the Field - "TextOnly_Available", return SchemeClaim List
        ''' </summary>
        ''' <param name="blnTextOnlyAvailable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FilterByTextOnlyAvailable(ByVal blnTextOnlyAvailable As Boolean) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelCollection As SchemeClaimModelCollection = New SchemeClaimModelCollection()

            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.TextOnlyAvailable = blnTextOnlyAvailable Then
                    udtResSchemeClaimModelCollection.Add(udtSchemeClaimModel)
                End If
            Next
            Return udtResSchemeClaimModelCollection
        End Function
        ' CRE13-001 - EHAPP [End][Tommy L]

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Function FilterReimbursementAvailableScheme() As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelCollection As SchemeClaimModelCollection = New SchemeClaimModelCollection()
            Dim udtResSchemeClaimModel As SchemeClaimModel = Nothing

            For Each udtSchemeClaimModel As SchemeClaimModel In Me
                If udtSchemeClaimModel.ReimbursementMode = SchemeClaimModel.EnumReimbursementMode.All _
                        Or udtSchemeClaimModel.ReimbursementMode = SchemeClaimModel.EnumReimbursementMode.FirstAuthAndSecondAuth _
                        Or udtSchemeClaimModel.ReimbursementMode = SchemeClaimModel.EnumReimbursementMode.HAFinance Then 'CRE20-015 (Special Support Scheme) [Martin]


                    udtResSchemeClaimModel = New SchemeClaimModel(udtSchemeClaimModel)
                    udtResSchemeClaimModelCollection.Add(udtResSchemeClaimModel)
                End If
            Next
            Return udtResSchemeClaimModelCollection
        End Function

        Public Function FilterByHCSPSubPlatform(ByVal enumSubPlatform As EnumHCSPSubPlatform) As SchemeClaimModelCollection
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeClaimList As New SchemeClaimModelCollection

            For Each udtSchemeClaim As SchemeClaimModel In Me
                If udtSchemeClaim.AvailableHCSPSubPlatform = EnumAvailableHCSPSubPlatform.ALL _
                        OrElse udtSchemeClaim.AvailableHCSPSubPlatform.ToString = enumSubPlatform.ToString Then
                    udtSchemeClaimList.Add(New SchemeClaimModel(udtSchemeClaim))
                End If

            Next

            Return udtSchemeClaimList

        End Function
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function FilterWithoutReadonly() As SchemeClaimModelCollection
            Dim udtSchemeClaimList As New SchemeClaimModelCollection

            For Each udtSchemeClaim As SchemeClaimModel In Me
                If Not udtSchemeClaim.ReadonlyHCSP Then
                    udtSchemeClaimList.Add(New SchemeClaimModel(udtSchemeClaim))
                End If

            Next

            Return udtSchemeClaimList

        End Function
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function FilterByDateBackClaimMinDate(ByVal dtmServiceDate As DateTime) As SchemeClaimModelCollection
            Dim udtSchemeClaimList As New SchemeClaimModelCollection
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim udtValidator As New Common.Validation.Validator

            Dim dtmMinDate As DateTime
            Dim strMinDate As String
            Dim udtSystemMessage As Common.ComObject.SystemMessage
            Dim intMaxDateBackDate As Integer

            intMaxDateBackDate = DateDiff(DateInterval.Day, dtmServiceDate, udtGeneralFunction.GetSystemDateTime) + 1  'Back office platform do not restrict max date back day but display warning during claim

            For Each udtSchemeClaim As SchemeClaimModel In Me
                If udtSchemeClaim.SubsidizeGroupClaimList.Count > 0 Then

                    'To filter out scheme beyond min date back day
                    dtmMinDate = Nothing
                    strMinDate = Nothing
                    udtSystemMessage = Nothing

                    udtGeneralFunction.getSystemParameter("DateBackClaimMinDate", strMinDate, String.Empty, udtSchemeClaim.SchemeCode)
                    dtmMinDate = Convert.ToDateTime(strMinDate)

                    udtSystemMessage = udtValidator.chkDateBackClaimServiceDate(dtmServiceDate.ToString("dd-MM-yyyy", New System.Globalization.CultureInfo("en-us")), _
                                                                                intMaxDateBackDate, _
                                                                                dtmMinDate)

                    If udtSystemMessage Is Nothing Then
                        udtSchemeClaimList.Add(udtSchemeClaim)
                    End If

                End If

            Next

            Return udtSchemeClaimList

        End Function
        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

    End Class
End Namespace
