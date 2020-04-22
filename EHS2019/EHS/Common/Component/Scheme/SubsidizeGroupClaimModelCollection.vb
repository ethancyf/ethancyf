Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeGroupClaimModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel)
            MyBase.Add(udtSubsidizeGroupClaimModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel)
            MyBase.Remove(udtSubsidizeGroupClaimModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeGroupClaimModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeGroupClaimModel)
            End Get
        End Property

        ''' <summary>
        ''' Filter by Current Date, return the SubsidizeGroupClaim with valid claim period
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal dtmCurrentDate As DateTime) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If udtSubsidizeGroupClaimModel.ClaimPeriodFrom <= dtmCurrentDate AndAlso dtmCurrentDate < udtSubsidizeGroupClaimModel.ClaimPeriodTo Then
                    udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                End If
            Next
            Return udtResSubsidizeGroupClaimModelCollection
        End Function

        'CRE13-006 HCVS Ceiling [Start][Karl]
        Public Function FilterbyRange(ByVal dtmStartDate As DateTime, ByVal dtmEndDate As DateTime) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing

            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If (udtSubsidizeGroupClaimModel.ClaimPeriodFrom >= dtmStartDate AndAlso udtSubsidizeGroupClaimModel.ClaimPeriodFrom <= dtmEndDate) Or _
               udtSubsidizeGroupClaimModel.ClaimPeriodFrom <= dtmStartDate AndAlso dtmStartDate < udtSubsidizeGroupClaimModel.ClaimPeriodTo Then
                    udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                End If
            Next
            Return udtResSubsidizeGroupClaimModelCollection
        End Function

        Public Function OrderBySchemeSeqASC() As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtRunningSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            Dim intCount As Integer = 0
            Dim intMin As Integer = 0
            Dim intPos As Integer = 0
            Dim intTotalcount As Integer = 0
            Dim intMaxLoop As Integer = 999 'prevent infinite loop
            Dim intLoopCount As Integer = 0

            udtRunningSubsidizeGroupClaimModelCollection = Me
            intTotalcount = udtRunningSubsidizeGroupClaimModelCollection.Count

            If intTotalcount > 0 Then
                Do While (udtRunningSubsidizeGroupClaimModelCollection.Count > 0)
                    intPos = 0
                    intMin = udtRunningSubsidizeGroupClaimModelCollection(intPos).SchemeSeq

                    For intCount = 0 To udtRunningSubsidizeGroupClaimModelCollection.Count - 1
                        'CRE13-026 Repository [Start][Karl]
                        'If Me(intCount).SchemeSeq < intMin Then
                        If udtRunningSubsidizeGroupClaimModelCollection(intCount).SchemeSeq < intMin Then
                            'CRE13-026 Repository [End][Karl]
                            intPos = intCount
                            'CRE13-026 Repository [Start][Karl]
                            'Fix ordering 
                            intMin = udtRunningSubsidizeGroupClaimModelCollection(intPos).SchemeSeq
                            'CRE13-026 Repository [End][Karl]
                        End If
                    Next

                    udtResSubsidizeGroupClaimModel = udtRunningSubsidizeGroupClaimModelCollection(intPos)
                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                    udtRunningSubsidizeGroupClaimModelCollection.RemoveAt(intPos)

                    'prevent infinite loop [start]
                    If intLoopCount >= intMaxLoop Then
                        Throw New Exception("Infinite Loop Appeared at [SubsidizeGroupClaimModelCollection.OrderBySchemeSeq]")
                        Exit Do
                    End If
                    intLoopCount += 1
                    'prevent infinite loop [end]
                Loop
            End If

            Return udtResSubsidizeGroupClaimModelCollection
        End Function
        'CRE13-006 HCVS Ceiling [End][Karl]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ''' <summary>
        ''' Filter by SchemeCode + SubsidizeCode, return the Last matched SubsidizeGroupClaimModelCollection
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FilterBySchemeCodeAndSubsidizeCode(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                   udtSubsidizeGroupClaimModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                End If
            Next
            Return udtResSubsidizeGroupClaimModelCollection
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Filter by SchemeCode + SubsidizeCode, return the Last matched SubsidizeGroupClaim
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String) As SubsidizeGroupClaimModel
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                   udtSubsidizeGroupClaimModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResSubsidizeGroupClaimModel = udtSubsidizeGroupClaimModel
                End If
            Next
            Return udtResSubsidizeGroupClaimModel
        End Function

        ''' <summary>
        ''' Filter by SchemeCode + SchemeSeq + SubsidizeCode (Unique)
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As SubsidizeGroupClaimModel
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeGroupClaimModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtSubsidizeGroupClaimModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResSubsidizeGroupClaimModel = udtSubsidizeGroupClaimModel
                End If
            Next
            Return udtResSubsidizeGroupClaimModel
        End Function

        ''' <summary>
        ''' Filter by SchemeCode + SchemeSeq, return the related SubsidizeGroupClaim List
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                   udtSubsidizeGroupClaimModel.SchemeSeq = intSchemeSeq Then
                    udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                End If
            Next
            Return udtResSubsidizeGroupClaimModelCollection
        End Function

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Filter by SchemeCode, return the related SubsidizeGroupClaim List
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                End If
            Next
            Return udtResSubsidizeGroupClaimModelCollection
        End Function

      

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Function FilterLatestClaimPeriodGroupBySubsidizeCode(ByVal strSchemeCode As String, ByVal dtmCurrentDate As DateTime) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            Dim htSubisidzeCode As New Hashtable
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                If udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeGroupClaimModel.ClaimPeriodFrom <= dtmCurrentDate AndAlso dtmCurrentDate < udtSubsidizeGroupClaimModel.ClaimPeriodTo Then
                    udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)

                    If htSubisidzeCode.ContainsKey(udtResSubsidizeGroupClaimModel.SubsidizeCode) Then
                        udtResSubsidizeGroupClaimModelCollection.Remove(CType(htSubisidzeCode(udtResSubsidizeGroupClaimModel.SubsidizeCode), SubsidizeGroupClaimModel))
                        htSubisidzeCode.Remove(udtResSubsidizeGroupClaimModel.SubsidizeCode)
                    End If

                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                    htSubisidzeCode.Add(udtResSubsidizeGroupClaimModel.SubsidizeCode, udtResSubsidizeGroupClaimModel)
                End If

            Next
            Return udtResSubsidizeGroupClaimModelCollection
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function FilterLastServiceDtm(ByVal strSchemeCode As String, ByVal dtmCurrentDate As DateTime) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                ' Ignore time checking

                'INT13-0033 Fix for HCVR unable retrieve scheme at last day of the service date [Start][Karl]
                If udtSubsidizeGroupClaimModel.ClaimPeriodFrom.Date <= dtmCurrentDate.Date AndAlso _
                dtmCurrentDate.Date <= udtSubsidizeGroupClaimModel.LastServiceDtm.Date AndAlso _
                   udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    'If udtSubsidizeGroupClaimModel.ClaimPeriodFrom.Date <= dtmCurrentDate AndAlso dtmCurrentDate <= udtSubsidizeGroupClaimModel.LastServiceDtm AndAlso _
                    '   udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    'INT13-0033 Fix for HCVR unable retrieve scheme at last day of the service date [End][Karl]

                    udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
                    udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                End If
            Next
            Return udtResSubsidizeGroupClaimModelCollection
        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Obsolete function, not necessary to filter by SchemeSeq
        'Public Function FilterLastServiceDtm(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal dtmCurrentDate As DateTime) As SubsidizeGroupClaimModelCollection
        '    Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
        '    Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
        '    For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
        '        ' Ignore time checking
        '        If udtSubsidizeGroupClaimModel.ClaimPeriodFrom.Date <= dtmCurrentDate AndAlso dtmCurrentDate <= udtSubsidizeGroupClaimModel.LastServiceDtm AndAlso _
        '           udtSubsidizeGroupClaimModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
        '           udtSubsidizeGroupClaimModel.SchemeSeq = intSchemeSeq Then

        '            udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
        '            udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
        '        End If
        '    Next
        '    Return udtResSubsidizeGroupClaimModelCollection
        'End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        'INT15-0011 (Fix empty print option in HCSP Account) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function FilterLatestSchemeSeqWithoutFutureDate(ByVal dtmCurrentDate As DateTime) As SubsidizeGroupClaimModelCollection
            Dim udtResSubsidizeGroupClaimModelCollection As New SubsidizeGroupClaimModelCollection()
            Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            Dim udtHashTableSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing
            Dim htSubisidzeCode As New Hashtable

            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In Me
                udtResSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)

                If htSubisidzeCode.ContainsKey(udtResSubsidizeGroupClaimModel.SubsidizeCode) Then
                    udtHashTableSubsidizeGroupClaimModel = CType(htSubisidzeCode(udtResSubsidizeGroupClaimModel.SubsidizeCode), SubsidizeGroupClaimModel)

                    If udtResSubsidizeGroupClaimModel.SchemeSeq > udtHashTableSubsidizeGroupClaimModel.SchemeSeq AndAlso _
                        udtResSubsidizeGroupClaimModel.ClaimPeriodFrom < dtmCurrentDate Then

                        udtResSubsidizeGroupClaimModelCollection.Remove(udtHashTableSubsidizeGroupClaimModel)
                        htSubisidzeCode.Remove(udtResSubsidizeGroupClaimModel.SubsidizeCode)

                        udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                        htSubisidzeCode.Add(udtResSubsidizeGroupClaimModel.SubsidizeCode, udtResSubsidizeGroupClaimModel)
                    End If
                Else
                    If udtResSubsidizeGroupClaimModel.ClaimPeriodFrom < dtmCurrentDate Then
                        udtResSubsidizeGroupClaimModelCollection.Add(udtResSubsidizeGroupClaimModel)
                        htSubisidzeCode.Add(udtResSubsidizeGroupClaimModel.SubsidizeCode, udtResSubsidizeGroupClaimModel)
                    End If
                End If

            Next

            Return udtResSubsidizeGroupClaimModelCollection
        End Function
        'INT15-0011 (Fix empty print option in HCSP Account) [End][Chris YIM]
    End Class

End Namespace
