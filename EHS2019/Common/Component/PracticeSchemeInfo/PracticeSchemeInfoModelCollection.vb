Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel

Namespace Component.PracticeSchemeInfo
    <Serializable()> Public Class PracticeSchemeInfoModelCollection
        Inherits System.Collections.SortedList
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Add SchemeCode to key
        Public Overloads Sub Add(ByVal udtPracticeSchemeInfoModel As PracticeSchemeInfoModel)
            MyBase.Add(udtPracticeSchemeInfoModel.PracticeDisplaySeq.ToString.Trim + "-" + CStr(udtPracticeSchemeInfoModel.SchemeDisplaySeq).PadLeft(5, "0") + "-" + CStr(udtPracticeSchemeInfoModel.SubsidizeDisplaySeq).PadLeft(5, "0") + "-" + udtPracticeSchemeInfoModel.SubsidizeCode.Trim + "-" + udtPracticeSchemeInfoModel.SchemeCode.Trim, udtPracticeSchemeInfoModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtPracticeSchemeInfoModel As PracticeSchemeInfoModel)
            'MyBase.Remove(udtPracticeModel)
            MyBase.Remove(udtPracticeSchemeInfoModel.PracticeDisplaySeq.ToString.Trim + "-" + CStr(udtPracticeSchemeInfoModel.SchemeDisplaySeq).PadLeft(5, "0") + "-" + CStr(udtPracticeSchemeInfoModel.SubsidizeDisplaySeq).PadLeft(5, "0") + "-" + udtPracticeSchemeInfoModel.SubsidizeCode.Trim + "-" + udtPracticeSchemeInfoModel.SchemeCode.Trim)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intPracticeDisplaySeq As Integer, ByVal strSubsidizeCode As String, ByVal intSchemeDisplaySeq As Integer, ByVal intSubsidizeDisplaySeq As Integer, ByVal strSchemeCode As String) As PracticeSchemeInfoModel
            Get
                'Dim udtSchemeBLL As New Scheme.SchemeBLL

                'Dim intSubSeqNo As Integer
                'Dim strMasterSchemeCode As String = Nothing
                'For Each udtSubScheme As Scheme.SubSchemeModel In udtSchemeBLL.getAllSubScheme()
                '    If udtSubScheme.SchemeCode.Trim = strSchemeCode Then
                '        intSubSeqNo = udtSubScheme.SequenceNum
                '        strMasterSchemeCode = udtSubScheme.MSchemeCode.Trim
                '        Exit For
                '    End If
                'Next

                'Dim intMasterSeqNo As Integer
                'For Each udtMasterScheme As Scheme.MasterSchemeModel In udtSchemeBLL.getAllMasterSchemeWithSubScheme()
                '    If udtMasterScheme.MSchemeCode.Trim = strMasterSchemeCode Then
                '        intMasterSeqNo = udtMasterScheme.SequenceNum
                '        Exit For
                '    End If
                'Next

                Return CType(MyBase.Item(intPracticeDisplaySeq.ToString.Trim + "-" + CStr(intSchemeDisplaySeq).PadLeft(5, "0") + "-" + CStr(intSubsidizeDisplaySeq).PadLeft(5, "0") + "-" + strSubsidizeCode.Trim + "-" + strSchemeCode), PracticeSchemeInfoModel)
            End Get
        End Property
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Sub New()
        End Sub

        Public Function Filter(ByVal strSchemeCode As String) As PracticeSchemeInfoModelCollection
            Dim udtResPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In Me.Values
                If udtPracticeSchemeInfo.SchemeCode.Trim.Equals(strSchemeCode.Trim) Then
                    udtResPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                End If
            Next

            If udtResPracticeSchemeInfoList.Count = 0 Then
                udtResPracticeSchemeInfoList = Nothing
            End If

            Return udtResPracticeSchemeInfoList
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal strSubsidzeCode As String) As PracticeSchemeInfoModel

            Dim udtPracticeResSchemeInfo As PracticeSchemeInfoModel = Nothing

            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In Me.Values
                If udtPracticeSchemeInfo.SchemeCode.Trim.Equals(strSchemeCode.Trim) AndAlso udtPracticeSchemeInfo.SubsidizeCode.Trim.Equals(strSubsidzeCode.Trim) Then
                    udtPracticeResSchemeInfo = New PracticeSchemeInfoModel
                    udtPracticeResSchemeInfo = udtPracticeSchemeInfo

                    Exit For
                End If
            Next

            Return udtPracticeResSchemeInfo

        End Function

        'CRE15-004 TIV & QIV [Start][Winnie]
        Public Function FilterByPracticeScheme(ByVal intPracticeDisplaySeq As Integer, ByVal strSchemeCode As String) As PracticeSchemeInfoModelCollection
            Dim udtResPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In Me.Values
                If udtPracticeSchemeInfo.SchemeCode.Trim.Equals(strSchemeCode.Trim) AndAlso udtPracticeSchemeInfo.PracticeDisplaySeq.Equals(intPracticeDisplaySeq) Then
                    udtResPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                End If
            Next

            If udtResPracticeSchemeInfoList.Count = 0 Then
                udtResPracticeSchemeInfoList = Nothing
            End If

            Return udtResPracticeSchemeInfoList
        End Function
        'CRE15-004 TIV & QIV [End][Winnie]

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Function FilterByHCSPSubPlatform(ByVal enumSubPlatform As EnumHCSPSubPlatform) As PracticeSchemeInfoModelCollection
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtPracticeSchemeList As New PracticeSchemeInfoModelCollection
            Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim

            For Each udtPracticeScheme As PracticeSchemeInfoModel In Me.Values
                Dim strSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtPracticeScheme.SchemeCode)
                Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimList.Filter(strSchemeCode)

                If IsNothing(udtSchemeClaim) Then Continue For

                If udtSchemeClaim.AvailableHCSPSubPlatform = EnumAvailableHCSPSubPlatform.ALL _
                        OrElse udtSchemeClaim.AvailableHCSPSubPlatform.ToString = enumSubPlatform.ToString Then
                    udtPracticeSchemeList.Add(New PracticeSchemeInfoModel(udtPracticeScheme))
                End If

            Next

            Return udtPracticeSchemeList

        End Function
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function IsNonClinic() As Boolean

            For Each udtPracticeScheme As PracticeSchemeInfoModel In Me.Values
                If udtPracticeScheme.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                    Return True
                End If
            Next

            Return False

        End Function
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
        Public Function FilterByPracticeDisplaySeq(ByVal intPracticeDisplaySeq As Integer) As PracticeSchemeInfoModelCollection
            Dim udtResPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In Me.Values
                If udtPracticeSchemeInfo.PracticeDisplaySeq.Equals(intPracticeDisplaySeq) Then
                    udtResPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                End If
            Next

            ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'If udtResPracticeSchemeInfoList.Count = 0 Then
            '    udtResPracticeSchemeInfoList = Nothing
            'End If
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

            Return udtResPracticeSchemeInfoList
        End Function
        ' CRE16-021 Transfer VSS category to PCD [End][Winnie]
    End Class
End Namespace

