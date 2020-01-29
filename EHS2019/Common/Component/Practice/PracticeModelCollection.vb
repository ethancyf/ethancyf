Imports Common.Component.PracticeSchemeInfo

Namespace Component.Practice
    <Serializable()> Public Class PracticeModelCollection
        Inherits System.Collections.SortedList
        Public Overloads Sub Add(ByVal udtPracticeModel As PracticeModel)
            MyBase.Add(udtPracticeModel.DisplaySeq, udtPracticeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtPracticeModel As PracticeModel)
            'MyBase.Remove(udtPracticeModel)
            MyBase.Remove(udtPracticeModel.DisplaySeq)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intDisplaySeq As Integer) As PracticeModel
            Get
                Return CType(MyBase.Item(intDisplaySeq), PracticeModel)
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Function GetPracticeSeq() As Integer
            Dim intLastValue As Integer = 0
            For Each udtPracticeModel As PracticeModel In MyBase.Values
                intLastValue = udtPracticeModel.DisplaySeq
            Next

            Return intLastValue + 1
        End Function

        Public Function FilterByMO(ByVal intMODisplaySeq As Integer) As PracticeModelCollection
            Dim udtPracticeList As PracticeModelCollection = New PracticeModelCollection
            Dim udtPractice As PracticeModel
            For Each udtPractice In Me.Values
                If udtPractice.MODisplaySeq = intMODisplaySeq Then
                    udtPracticeList.Add(udtPractice)
                End If
            Next

            Return udtPracticeList
        End Function

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Public Function FilterByPCD() As PracticeModelCollection
        '    Dim udtPracticeList As PracticeModelCollection = New PracticeModelCollection
        '    Dim udtPractice As PracticeModel
        '    For Each udtPractice In Me.Values
        '        If udtPractice.IsAllowJoinPCD() Then
        '            udtPracticeList.Add(udtPractice)
        '        End If
        '    Next
        '    Return udtPracticeList
        'End Function

        ''' <summary>
        ''' Filter the practice and practice scheme by PCD
        ''' Only the Practice with Active (or New) Practice Scheme can be sent to PCD        
        ''' </summary>
        ''' <param name="strTableLocation"></param>
        ''' <param name="udtSPPermanent"></param>
        ''' <returns>Return the Practice list with Practice Scheme which can be sent to PCD</returns>
        ''' <remarks></remarks>
        Public Function FilterByPCD(ByVal strTableLocation As String, _
                                    Optional ByVal udtSPPermanent As ServiceProvider.ServiceProviderModel = Nothing) As PracticeModelCollection

            ' ----------------------------------------------------------------------------------------
            ' Enrolment:    Enrolment Copy / eForm
            '   No filtering on the Practice and Practice scheme status as it must be new
            '
            ' Staging:      VU - Token Scheme Management
            '   Filtered by New (in Staging) and Active (in Perm) Practice Scheme
            '
            ' Permanent:    SP My Profile / VU - SP Maintenance
            '   Filtered by Active Practice and Practice Scheme   
            ' ----------------------------------------------------------------------------------------

            Dim udtPracticeList As PracticeModelCollection = New PracticeModelCollection
            Dim udtPractice As PracticeModel

            For Each udtPractice In Me.Values

                Dim blnIsAllowJoinPCD As Boolean = False
                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

                If udtPractice.IsAllowJoinPCD() Then

                    Select Case strTableLocation
                        Case TableLocation.Enrolment
                            blnIsAllowJoinPCD = True
                            udtPracticeSchemeInfoList = udtPractice.PracticeSchemeInfoList

                        Case TableLocation.Staging

                            ' Check Practice Scheme Status
                            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values

                                Select Case udtPracticeSchemeInfo.RecordStatus
                                    Case PracticeSchemeInfoStagingStatus.Active
                                        ' New Practice Scheme
                                        blnIsAllowJoinPCD = True
                                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)

                                    Case PracticeSchemeInfoStagingStatus.Existing, _
                                        PracticeSchemeInfoStagingStatus.Update

                                        ' Existing Practice Scheme, Check Perm Status whether is 'Active'
                                        If Not IsNothing(udtSPPermanent) AndAlso udtSPPermanent.PracticeList.Count > 0 Then
                                            Dim udtPracticePermanent As PracticeModel = udtSPPermanent.PracticeList.Item(udtPractice.DisplaySeq)

                                            Dim udtPracticeSchemeInfoPerm As PracticeSchemeInfoModel = udtPracticePermanent.PracticeSchemeInfoList.Filter(udtPracticeSchemeInfo.SchemeCode, udtPracticeSchemeInfo.SubsidizeCode)

                                            If udtPracticeSchemeInfoPerm.RecordStatus = PracticeSchemeInfoStatus.Active Then
                                                blnIsAllowJoinPCD = True
                                                udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                            End If
                                        End If

                                End Select
                            Next ' Practice Scheme


                        Case TableLocation.Permanent

                            ' Practice 'Active'
                            If udtPractice.RecordStatus = PracticeStatus.Active Then

                                ' Practice Scheme 'Active' (Exclude Active pending Suspended/Delisted record)
                                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                                    If udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Active Then
                                        blnIsAllowJoinPCD = True
                                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                    End If
                                Next

                            End If

                    End Select

                    If blnIsAllowJoinPCD Then
                        udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoList
                        udtPracticeList.Add(udtPractice)
                    End If

                End If
            Next

            Return udtPracticeList
        End Function
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Public Sub DuplicatePracticeEName()

            ' Dim udtMOList As New MedicalOrganizationModelCollection


            Dim strPracticeEName As New List(Of String)
            Dim strDPracticeEname As String = String.Empty

            Dim intPracticeIndex As New List(Of Integer)


            If Not IsNothing(MyBase.Values) Then
                If MyBase.Count > 0 Then
                    For Each udtPractice As PracticeModel In MyBase.Values
                        If Not udtPractice.RecordStatus.Trim.Equals("D") Then
                            strPracticeEName.Add(udtPractice.PracticeName.Trim)
                        End If
                    Next

                    Dim strDistinctPracticeEname As New List(Of String)
                    Dim strDuplicationPracticeEname As New List(Of String)

                    For Each item As String In strPracticeEName
                        If Not strDistinctPracticeEname.Contains(item) Then
                            strDistinctPracticeEname.Add(item)
                        Else
                            If Not strDuplicationPracticeEname.Contains(item) Then
                                strDuplicationPracticeEname.Add(item)
                            End If
                        End If
                    Next

                    For Each udtPractice As PracticeModel In MyBase.Values
                        udtPractice.IsDuplicated = False
                        For Each item As String In strDuplicationPracticeEname
                            If udtPractice.PracticeName.Trim.Equals(item.Trim) Then
                                If Not udtPractice.RecordStatus.Trim.Equals("D") Then
                                    udtPractice.IsDuplicated = True
                                End If
                                'udtMOList.Add(udtMO)
                            End If
                        Next
                    Next
                End If
            End If

            'Return udtMOList

        End Sub

    End Class
End Namespace

