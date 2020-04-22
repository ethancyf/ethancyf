Imports Common.ComFunction
Imports Common.Component.Mapping
Imports Common.Validation


Public Class ComFunction

    ''' <summary>
    ''' Map EHS code to PCD code
    ''' </summary>
    ''' <param name="eCodeType">Mapping Type</param>
    ''' <param name="strSourceCode">EHS code</param>
    ''' <returns>Return PCD code</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMappingForPCD(ByVal eCodeType As EnumConstant.EnumMappingCodeType, ByVal strSourceCode As String) As CodeMappingModel
        Dim udtCodeMapList As CodeMappingCollection
        Dim udtCodeMap As CodeMappingModel

        udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.EHS, CodeMappingModel.EnumTargetSystem.PCD, eCodeType.ToString, strSourceCode)

        Return udtCodeMap
    End Function

    ''' <summary>
    ''' Map PCD code to EHS code
    ''' </summary>
    ''' <param name="eCodeType">Mapping Type</param>
    ''' <param name="strSourceCode">PCD code</param>
    ''' <returns>Return EHS code</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMappingForEHS(ByVal eCodeType As EnumConstant.EnumMappingCodeType, ByVal strSourceCode As String) As CodeMappingModel
        Dim udtCodeMapList As CodeMappingCollection
        Dim udtCodeMap As CodeMappingModel

        udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, eCodeType.ToString, strSourceCode)

        Return udtCodeMap
    End Function


#Region "PCD Enrolment Number"
    Public Shared Function GeneratePCDEnrolRefNo() As String
        Dim genFunct As GeneralFunction = New GeneralFunction()
        Dim strPrefix As String = String.Empty
        Dim strTempNum As String = String.Empty

        Dim dtToday As DateTime = genFunct.GetSystemDateTime()
        Dim intNextNum As Integer = 0

        Dim strRes As String = String.Empty
        Dim strPrefix_y, strPrefix_m, strPrefix_d As String

        intNextNum = genFunct.getSeqNo_Prefix("PCDAE", "ALL", strPrefix)

        strRes = strPrefix
        strPrefix_y = dtToday.Year.ToString
        strPrefix_y = Right(strPrefix_y, 2)
        strPrefix_m = dtToday.Month.ToString
        strPrefix_d = dtToday.Day.ToString
        If strPrefix_d.Length = 1 Then
            strPrefix_d = "0" + strPrefix_d
        End If
        Select Case strPrefix_m
            Case "10"
                strPrefix_m = "A"
            Case "11"
                strPrefix_m = "B"
            Case "12"
                strPrefix_m = "C"
        End Select

        strRes = strRes + strPrefix_y + strPrefix_m + strPrefix_d
        strRes = strRes + String.Format("{0:d7}", intNextNum)

        strRes = strRes + genFunct.generateChkDgt(strRes)

        Return strRes

    End Function

    Public Shared Function FormatPCDEnrolRefNo(ByVal strValue As String) As String
        Dim eValid As Validator = New Validator()

        If strValue.Length < 9 Then Return strValue

        If eValid.IsAlpha(strValue.Substring(0, 5)) Then
            Return String.Format("{0}-{1}-{2}", _
                            strValue.Substring(0, strValue.Length - 8), _
                            CInt(strValue.Substring(strValue.Length - 8, 7)).ToString, _
                            strValue.Substring(strValue.Length - 1, 1))
        Else
            Return String.Format("{0}-{1}-{2}", _
                            strValue.Substring(0, strValue.Length - 9), _
                            CInt(strValue.Substring(strValue.Length - 9, 8)).ToString, _
                            strValue.Substring(strValue.Length - 1, 1))
        End If
    End Function


#End Region

End Class
