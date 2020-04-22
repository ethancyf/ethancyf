Imports System.Text.RegularExpressions
Imports System.Web
Imports Common.Web
Imports Common.Web.SystemStatus

Public Class PCDFormatter
    Inherits Common.Web.Formatter

#Region "DateTime"
    Public Const DefaultYearFormat As String = "yyyy"
    Public Const DefaultDateFormat As String = "dd/MM/yyyy"
    Public Const DefaultTimeFormat As String = "HH:mm"
    Public Const DefaultDateTimeFormat As String = "dd/MM/yyyy HH:mm"
    Public Const DefaultDateTimeSecondFormat As String = "ddMMyyyyHHmmss"
    Public Const DefaultDBDateFormat As String = "yyyy/MM/dd"
    Public Const DefaultTimeOptionFormat As String = "HHmm"
    Public Const DefaultLongDateFormat As String = "dd MMMM yyyy"
    Public Const DefaultSystemDateTimeSecondFormat As String = "dd/MM/yyyy HH:mm:ss"
#End Region

#Region "Address"

    Public Function Address(ByVal strCorrespondenceAddressLine1 As String, _
                            ByVal DistrictID As String, ByVal lang As SystemStatus.EnumLanguage) As String
        Dim strAddress As String = String.Empty

        Dim udtAreaBLL As AreaSetupBLL = New AreaSetupBLL
        Dim udtDistrictBoardBLL As DistrictBoardSetupBLL = New DistrictBoardSetupBLL
        Dim udtDistrictBLL As DistrictSetupBLL = New DistrictSetupBLL

        Dim udtArea As AreaSetup
        Dim udtDistrictBoard As DistrictBoardSetup
        Dim udtDistrict As DistrictSetup

        udtDistrict = udtDistrictBLL.GetDistrictSetupList.Item(DistrictID)
        udtDistrictBoard = udtDistrictBoardBLL.GetDistrictBoardSetupList.Item(udtDistrict.DistrictBoardID)
        udtArea = udtAreaBLL.GetAreaSetupList.Item(udtDistrictBoard.AreaID)


        Select Case lang
            Case EnumLanguage.EN
                strAddress = IIf(strCorrespondenceAddressLine1.Length > 0, strCorrespondenceAddressLine1 + ", ", String.Empty) + udtDistrict.Description(lang) + ", " + udtArea.Description(lang)

            Case EnumLanguage.SC
                strAddress = udtArea.Description(lang) + udtDistrict.Description(lang) + strCorrespondenceAddressLine1
            Case EnumLanguage.TC
                strAddress = udtArea.Description(lang) + udtDistrict.Description(lang) + strCorrespondenceAddressLine1
            Case Else
                Throw New Exception(String.Format("Formatter: Unhandled language ({0})", lang))
        End Select

        Return strAddress

    End Function

#End Region

#Region "HKID"

    ''' <summary>
    ''' Format HKID for display
    ''' </summary>
    ''' <param name="strHKICNoChar"></param>
    ''' <param name="strHKICNo"></param>
    ''' <param name="strHKICCheckDigit"></param>
    ''' <param name="blnMask"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormatHKID(ByVal strHKICNoChar As String, ByVal strHKICNo As String, ByVal strHKICCheckDigit As String, Optional ByVal blnMask As Boolean = False) As String
        Return FormatHKID(String.Format("{0}{1}{2}", strHKICNoChar.Trim, strHKICNo.Trim, strHKICCheckDigit.Trim), blnMask)
    End Function

    ''' <summary>
    ''' Format HKID for display
    ''' </summary>
    ''' <param name="strHKIC"></param>
    ''' <param name="blnMask"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormatHKID(ByVal strHKIC As String, Optional ByVal blnMask As Boolean = False) As String
        Dim strOut As String = strHKIC.Replace(" ", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty)

        Select Case strOut.Length
            Case 8, 9
                If blnMask Then
                    Return String.Format("{0}{1}(X)", strOut.Substring(0, 4), New String("X"c, strOut.Length - 5))
                Else
                    Return String.Format("{0}({1})", strOut.Substring(0, strOut.Length - 1), strOut.Substring(strOut.Length - 1))
                End If

            Case Else
                ' Unexpected length, return original
                Return strHKIC

        End Select

        Return Nothing

    End Function

    ''' <summary>
    ''' Convert the HKID to contain alphanumeric only
    ''' </summary>
    ''' <param name="strHKID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertHKID(ByVal strHKID As String) As String
        If strHKID = String.Empty Then Return String.Empty

        Return strHKID.Replace(" ", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty).PadRight(9, " ")
    End Function

#End Region

#Region "Name"

    Public Function FormatEnglishName(ByVal strSurname As String, ByVal strFirstName As String) As String
        Dim udtResultCode_Surname As Validator.General.GeneralResultCode
        Dim udtResultCode_firstName As Validator.General.GeneralResultCode
        Dim strRes As String = String.Empty

        udtResultCode_Surname = Validator.General.IsEmpty(strSurname)
        udtResultCode_firstName = Validator.General.IsEmpty(strFirstName)

        If udtResultCode_Surname = Validator.General.GeneralResultCode.Empty AndAlso _
            udtResultCode_firstName = Validator.General.GeneralResultCode.Empty Then
            strRes = String.Empty
        Else
            If udtResultCode_firstName = Validator.General.GeneralResultCode.Empty Then
                strRes = strSurname
            Else
                If udtResultCode_Surname = Validator.General.GeneralResultCode.Empty Then
                    strRes = strFirstName
                Else
                    strRes = String.Format("{0}, {1}", strSurname.Trim, strFirstName.Trim)
                End If

            End If
        End If

        Return strRes
    End Function

    Public Sub ConvertEnglishName(ByVal strName As String, ByRef strSurname As String, ByRef strFirstName As String)
        If strName.Contains(",") Then
            strSurname = strName.Split(",")(0).Trim
            strFirstName = strName.Split(",")(1).Trim

        Else
            strSurname = strName.Trim
            strFirstName = String.Empty

        End If

    End Sub

    ''' <summary>
    ''' Remove the multiple spaces (2 or more) and trim
    ''' </summary>
    ''' <param name="strName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertEnglishName(ByVal strName As String) As String
        Return Regex.Replace(strName, "\s+", " ").Trim
    End Function

    ''' <summary>
    ''' Remove all the spaces
    ''' </summary>
    ''' <param name="strName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertChineseName(ByVal strName As String) As String
        Return Regex.Replace(strName, " ", String.Empty)
    End Function

#End Region

#Region "Gender"

    Public Function FormatGender(ByVal strGenderCode As String) As String
        Dim udtStaticData As StaticDataModel = (New StaticDataBLL).GetStaticDataList(StaticDataModel.StaticDataTypeClass.Gender).Filter(strGenderCode)

        If Not IsNothing(udtStaticData) Then
            Return udtStaticData.Description
        Else
            Return String.Empty
        End If
    End Function

#End Region

#Region "Pager"

    Public Function FormatPager(ByVal strPagerNo As String, ByVal strPage As String) As String
        If strPagerNo = String.Empty OrElse strPage = String.Empty Then
            Return strPagerNo
        Else
            Return String.Format("{0} {1} {2}", strPagerNo, HttpContext.GetGlobalResourceObject("Text", "Page"), strPage)
        End If
    End Function

#End Region

#Region "System Number"

    Public Function FormatSystemNumber(ByVal strValue As String) As String
        If strValue.Length < 9 Then Return strValue

        If Validator.General.IsAlpha(strValue.Substring(0, 5)) = Validator.General.GeneralResultCode.Valid Then
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

    Public Function ConvertSystemNumber(ByVal strValue As String) As String
        If strValue = String.Empty OrElse Not strValue.Contains("-") Then Return strValue

        Dim aryValue As String() = strValue.Split("-")
        If Validator.General.IsAlpha(strValue.Substring(0, 5)) = Validator.General.GeneralResultCode.Valid Then
            Return String.Format("{0}{1}{2}", aryValue(0).Trim, aryValue(1).Trim.PadLeft(7, "0"), aryValue(2).Trim)
        Else
            Return String.Format("{0}{1}{2}", aryValue(0).Trim, aryValue(1).Trim.PadLeft(8, "0"), aryValue(2).Trim)
        End If

    End Function

#End Region

#Region "Process Note"

    Public Function FormatProcessNote(ByVal strNote As String) As String
        Dim strResult As String = String.Empty

        For Each strValue As String In GeneralFunction.StringTOList(strNote)
            strResult += strValue.Trim + Environment.NewLine + Environment.NewLine
        Next

        Return strResult

    End Function

    Public Function FormatLatestProcessNote(ByVal strNote As String) As String
        Return GeneralFunction.StringTOList(strNote).First.Trim
    End Function

#End Region

#Region "Service Provision"
    Public Shared Function FormatFeePrice(ByVal intFeeFrom As Nullable(Of Integer), ByVal intFeeTo As Nullable(Of Integer), ByVal strSetupResourceID As String, ByVal udtFeeType As String) As String
        Return FormatFeePrice(GeneralFunction.NullableInt2String(intFeeFrom, False), GeneralFunction.NullableInt2String(intFeeTo, False), strSetupResourceID, udtFeeType)
    End Function
    Public Shared Function FormatFeePrice(ByVal strFeeFrom As String, ByVal strFeeTo As String, ByVal strSetupResourceID As String, ByVal udtFeeType As String) As String
        Dim strPricedDisplay As String = String.Empty
        If strFeeFrom.Length = 0 AndAlso strFeeTo.Length = 0 Then
            strPricedDisplay = HttpContext.GetGlobalResourceObject("Text", "PriceOnRequestNotation") '*
        Else
            If udtFeeType = ServiceProvisionSetup.ServiceFeeTypeClass.UNIT Then
                strPricedDisplay = HttpContext.GetGlobalResourceObject("SetupResource", "ServiceFeeUnitPrefix") & " " & HttpContext.GetGlobalResourceObject("SetupResource", strSetupResourceID)

            ElseIf udtFeeType = ServiceProvisionSetup.ServiceFeeTypeClass.RANGE Then
                If strFeeTo Is Nothing OrElse strFeeTo.Length = 0 Then
                    strPricedDisplay = HttpContext.GetGlobalResourceObject("SetupResource", "SERVICEFEETYPE_RANGE_FROM_ONLY")
                Else
                    strPricedDisplay = HttpContext.GetGlobalResourceObject("SetupResource", "SERVICEFEETYPE_RANGE")
                End If
            Else
                Throw New Exception("Formatter: Unhandled Fee Type ")
            End If
        End If
        strPricedDisplay = String.Format(strPricedDisplay, New String() {strFeeFrom, strFeeTo})

        Return strPricedDisplay
    End Function
#End Region

End Class
