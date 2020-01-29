Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeFeeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeFeeModel As SubsidizeFeeModel)
            MyBase.Add(udtSubsidizeFeeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeFeeModel As SubsidizeFeeModel)
            MyBase.Remove(udtSubsidizeFeeModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeFeeModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeFeeModel)
            End Get
        End Property

        ''' <summary>
        ''' Filter by Current Date, return the SubsidizeGroupClaim with valid claim period
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal dtmCurrentDate As DateTime) As SubsidizeFeeModelCollection
            Dim udtResSubsidizeFeeModelCollection As New SubsidizeFeeModelCollection()
            Dim udtResSubsidizeFeeModel As SubsidizeFeeModel = Nothing
            For Each udtSubsidizeFeeModel As SubsidizeFeeModel In Me
                If udtSubsidizeFeeModel.EffectiveDtm <= dtmCurrentDate AndAlso dtmCurrentDate <= udtSubsidizeFeeModel.ExpiryDtm Then
                    udtResSubsidizeFeeModel = New SubsidizeFeeModel(udtSubsidizeFeeModel)
                    udtResSubsidizeFeeModelCollection.Add(udtResSubsidizeFeeModel)
                End If
            Next
            Return udtResSubsidizeFeeModelCollection
        End Function

        ''' <summary>
        ''' Filter by SchemeCode + SubsidizeCode, return the Last matched SubsidizeFeeModelCollection
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FilterBySchemeCodeAndSubsidizeCode(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String) As SubsidizeFeeModelCollection
            Dim udtResSubsidizeFeeModelCollection As New SubsidizeFeeModelCollection()
            Dim udtResSubsidizeFeeModel As SubsidizeFeeModel = Nothing
            For Each udtSubsidizeFeeModel As SubsidizeFeeModel In Me
                If udtSubsidizeFeeModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                   udtSubsidizeFeeModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResSubsidizeFeeModel = New SubsidizeFeeModel(udtSubsidizeFeeModel)
                    udtResSubsidizeFeeModelCollection.Add(udtResSubsidizeFeeModel)
                End If
            Next
            Return udtResSubsidizeFeeModelCollection
        End Function

        ''' <summary>
        ''' Filter by SubsidizeFeeType + Current Date, return the Last matched SubsidizeFee
        ''' </summary>
        ''' <param name="strSubsidizeFeeType"></param>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSubsidizeFeeType As String, ByVal dtmCurrentDate As DateTime) As SubsidizeFeeModel
            ' INT14-0010 - Filter SubsidizeFee by current date excluding time [Start][Lawrence]
            dtmCurrentDate = dtmCurrentDate.Date
            ' INT14-0010 - Filter SubsidizeFee by current date excluding time [End][Lawrence]

            Dim udtResSubsidizeFeeModel As SubsidizeFeeModel = Nothing
            For Each udtSubsidizeFeeModel As SubsidizeFeeModel In Me
                If udtSubsidizeFeeModel.SubsidizeFeeType.Trim().ToUpper().Equals(strSubsidizeFeeType.Trim().ToUpper()) AndAlso _
                    udtSubsidizeFeeModel.EffectiveDtm <= dtmCurrentDate AndAlso dtmCurrentDate <= udtSubsidizeFeeModel.ExpiryDtm Then
                    udtResSubsidizeFeeModel = udtSubsidizeFeeModel
                End If
            Next
            Return udtResSubsidizeFeeModel
        End Function

        ''' <summary>
        ''' Filter by SubsidizeFeeType, return the Last matched SubsidizeFee
        ''' </summary>
        ''' <param name="strSubsidizeFeeType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSubsidizeFeeType As String) As SubsidizeFeeModel
            Dim udtResSubsidizeFeeModel As SubsidizeFeeModel = Nothing
            For Each udtSubsidizeFeeModel As SubsidizeFeeModel In Me
                If udtSubsidizeFeeModel.SubsidizeFeeType.Trim().ToUpper().Equals(strSubsidizeFeeType.Trim().ToUpper()) Then
                    udtResSubsidizeFeeModel = udtSubsidizeFeeModel
                End If
            Next
            Return udtResSubsidizeFeeModel
        End Function

        ''' <summary>
        ''' Filter by SchemeCode + SubsidizeSeq + SubsidizeCode + Current Date, return the Last matched SubsidizeFee
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSubsidizeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String, ByVal intSubsidizeSeq As Integer, ByVal strSubsidizeCode As String, ByVal dtmCurrentDate As DateTime) As SubsidizeFeeModel
            Dim udtResSubsidizeFeeModel As SubsidizeFeeModel = Nothing
            For Each udtSubsidizeFeeModel As SubsidizeFeeModel In Me
                If udtSubsidizeFeeModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeFeeModel.SubsidizeSeq = intSubsidizeSeq AndAlso _
                    udtSubsidizeFeeModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeFeeModel.EffectiveDtm <= dtmCurrentDate AndAlso dtmCurrentDate <= udtSubsidizeFeeModel.ExpiryDtm Then
                    udtResSubsidizeFeeModel = udtSubsidizeFeeModel
                End If
            Next
            Return udtResSubsidizeFeeModel
        End Function

        ''' <summary>
        ''' Filter by SchemeCode + SubsidizeSeq + SubsidizeCode + SubsidizeFeeType + Current Date, return the Last matched SubsidizeFee
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSubsidizeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="strSubsidizeFeeType"></param>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String, ByVal intSubsidizeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strSubsidizeFeeType As String, ByVal dtmCurrentDate As DateTime) As SubsidizeFeeModel
            Dim udtResSubsidizeFeeModel As SubsidizeFeeModel = Nothing
            For Each udtSubsidizeFeeModel As SubsidizeFeeModel In Me
                If udtSubsidizeFeeModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeFeeModel.SubsidizeSeq = intSubsidizeSeq AndAlso _
                    udtSubsidizeFeeModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeFeeModel.SubsidizeFeeType.Trim().ToUpper().Equals(strSubsidizeFeeType.Trim().ToUpper()) AndAlso _
                    udtSubsidizeFeeModel.EffectiveDtm <= dtmCurrentDate AndAlso dtmCurrentDate <= udtSubsidizeFeeModel.ExpiryDtm Then
                    udtResSubsidizeFeeModel = udtSubsidizeFeeModel
                End If
            Next
            Return udtResSubsidizeFeeModel
        End Function

        ''' <summary>
        ''' Filter by SchemeCode + SubsidizeSeq + SubsidizeCode, return the related SubsidizeFee List
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSubsidizeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String, ByVal intSubsidizeSeq As Integer, ByVal strSubsidizeCode As String) As SubsidizeFeeModelCollection
            Dim udtResSubsidizeFeeModelCollection As New SubsidizeFeeModelCollection()
            Dim udtResSubsidizeFeeModel As SubsidizeFeeModel = Nothing
            For Each udtSubsidizeFeeModel As SubsidizeFeeModel In Me
                If udtSubsidizeFeeModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeFeeModel.SubsidizeSeq = intSubsidizeSeq AndAlso _
                    udtSubsidizeFeeModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResSubsidizeFeeModel = New SubsidizeFeeModel(udtSubsidizeFeeModel)
                    udtResSubsidizeFeeModelCollection.Add(udtResSubsidizeFeeModel)
                End If
            Next
            Return udtResSubsidizeFeeModelCollection
        End Function

    End Class

End Namespace
