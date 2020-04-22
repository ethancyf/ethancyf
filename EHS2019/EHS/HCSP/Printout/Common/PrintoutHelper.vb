
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount.EHSAccountModel

Namespace PrintOut.Common

    Public Class PrintoutHelper

        Public Enum PrintoutVersion
            FullChinese
            FullEnglish

            CondensedChinese
            CondensedEnglish

            FullChineseSmartID
            FullEnglishSmartID

            CondensedChineseSmartID
            CondensedEnglishSmartID

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            FullSimpChinese
            FullSimpChineseSmartID
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End Enum

        Public Const SystemParameterCategory As String = "Printout"
        Public Const FullChineseVersionParameter As String = "VersionCodeFull_CHI"
        Public Const FullEnglishVersionParameter As String = "VersionCodeFull_ENG"
        Public Const CondensedChineseVersionParameter As String = "VersionCodeCondensed_CHI"
        Public Const CondensedEnglishVersionParameter As String = "VersionCodeCondensed_ENG"

        Public Const FullChineseVersionSmartIDParameter As String = "VersionCodeFullSmartID_CHI"
        Public Const FullEnglishVersionSmartIDParameter As String = "VersionCodeFullSmartID_ENG"
        Public Const CondensedChineseVersionSmartIDParameter As String = "VersionCodeCondensedSmartID_CHI"
        Public Const CondensedEnglishVersionSmartIDParameter As String = "VersionCodeCondensedSmartID_ENG"

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Const FullSimpChineseVersionParameter As String = "VersionCodeFull_CN"
        Public Const FullSimpChineseVersionSmartIDParameter As String = "VersionCodeFullSmartID_CN"
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Private _udtGeneralFunction As GeneralFunction = New GeneralFunction()

        ' Printout Version Code
        Private Function GetVersionCodeSystemParameter(ByVal udtPrintoutVersion As PrintoutVersion) As String
            Select Case udtPrintoutVersion
                Case PrintoutVersion.FullChinese
                    Return FullChineseVersionParameter

                Case PrintoutVersion.FullEnglish
                    Return FullEnglishVersionParameter

                Case PrintoutVersion.CondensedChinese
                    Return CondensedChineseVersionParameter

                Case PrintoutVersion.CondensedEnglish
                    Return CondensedEnglishVersionParameter

                    ' Smart ID Forms --------------------------------------------------------------
                Case PrintoutVersion.FullChineseSmartID
                    Return FullChineseVersionSmartIDParameter

                Case PrintoutVersion.FullEnglishSmartID
                    Return FullEnglishVersionSmartIDParameter

                Case PrintoutVersion.CondensedChineseSmartID
                    Return CondensedChineseVersionSmartIDParameter

                Case PrintoutVersion.CondensedEnglishSmartID
                    Return CondensedEnglishVersionSmartIDParameter

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Case PrintoutVersion.FullSimpChinese
                    Return FullSimpChineseVersionParameter

                Case PrintoutVersion.FullSimpChineseSmartID
                    Return FullSimpChineseVersionSmartIDParameter
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

            End Select

            Return String.Empty

        End Function

        ' Printout Version Code
        Public Function GetPrintoutVersionCode(ByVal strSchemeCode As String, ByVal udtPrintoutVersion As PrintoutVersion) As String

            Dim strValue1 As String = String.Empty
            Dim strValue2 As String = String.Empty
            Dim strParameterName As String = GetVersionCodeSystemParameter(udtPrintoutVersion)

            If Not String.IsNullOrEmpty(strParameterName) AndAlso _udtGeneralFunction.getSytemParameterByParameterNameSchemeCode(strParameterName, strValue1, strValue2, strSchemeCode) Then
                Return strValue1
            End If

            Return strValue1

        End Function

        ' Measure String Length for placing Textbox Control
        Public Function MeasureStringSize(ByVal strMessage As String, ByVal sdtFont As System.Drawing.Font) As System.Drawing.SizeF
            Dim sdtTextBitmap As System.Drawing.Bitmap = New System.Drawing.Bitmap(1, 1)
            Dim sdtDrawGraphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(sdtTextBitmap)

            sdtDrawGraphics.PageUnit = Drawing.GraphicsUnit.Inch
            sdtDrawGraphics.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.None

            Dim sdtMessageSizePixel As System.Drawing.SizeF = sdtDrawGraphics.MeasureString(strMessage, sdtFont)
            Dim sngWidth = System.Math.Round(sdtMessageSizePixel.Width, 4)
            Dim sngHeight = System.Math.Round(sdtMessageSizePixel.Height, 4)

            Return New System.Drawing.SizeF(sngWidth, sngHeight)
        End Function

        ' Get Dose Description for Consent form
        Public Function GetDoseDescription(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String, ByVal strLanguage As String) As String
            Dim strResourceObjectType As String = "PrintoutText"
            Dim strResourceObjectName As String = String.Empty
            Dim objResource As Object = Nothing

            'CRE15-004 TIV & QIV [Start][Philip]
            'For i As Integer = intSchemeSeq To 1 Step -1
            '    strResourceObjectName = String.Format("{0}_{1}_{2}_{3}", strSchemeCode, i.ToString(), strSubsidizeItemCode, strAvailableItemCode)
            '    objResource = HttpContext.GetGlobalResourceObject(strResourceObjectType, strResourceObjectName, New System.Globalization.CultureInfo(strLanguage))
            '    If Not objResource Is Nothing Then
            '        Exit For
            '    End If
            'Next

            strResourceObjectName = String.Format("{0}_{1}_{2}_{3}", strSchemeCode, intSchemeSeq.ToString(), strSubsidizeItemCode, strAvailableItemCode)
            objResource = HttpContext.GetGlobalResourceObject(strResourceObjectType, strResourceObjectName, New System.Globalization.CultureInfo(strLanguage))
            'CRE15-004 TIV & QIV [End][Philip]

            If objResource Is Nothing Then
                Throw New ArgumentException(String.Format("Missing SystemResource:{0}_{1}_{2}_{3}", strSchemeCode, intSchemeSeq.ToString(), strSubsidizeItemCode, strAvailableItemCode))
            End If

            Return objResource.ToString()

        End Function

    End Class



    ' HSIVSS Helper
    Public Class HSIVSSPrintoutHelper

        Private _udtGeneralFunction As GeneralFunction = New GeneralFunction()

        Public Function IsShowDoseInformation(ByVal dtmServiceDate As Date, ByVal strSchemeCode As String, ByVal udtEHSPersonalInformation As EHSPersonalInformationModel) As Boolean
            Dim strParmValue1 As String = String.Empty
            Dim strParmValue2 As String = String.Empty

            Dim strOperation As String = String.Empty
            Dim strCompareValue As String = String.Empty
            Dim strCompareUnit As String = String.Empty
            Dim strCheckingMethod As String = String.Empty

            If _udtGeneralFunction.getSytemParameterByParameterName("HSIVSS_DisplayDoseAgeCriteria_Operator", strParmValue1, strParmValue2) Then
                strOperation = strParmValue1
            Else
                Return False
            End If

            If _udtGeneralFunction.getSytemParameterByParameterName("HSIVSS_DisplayDoseAgeCriteria_CompareValue", strParmValue1, strParmValue2) Then
                strCompareValue = strParmValue1
            Else
                Return False
            End If

            If _udtGeneralFunction.getSytemParameterByParameterName("HSIVSS_DisplayDoseAgeCriteria_CompareUnit", strParmValue1, strParmValue2) Then
                strCompareUnit = strParmValue1
            Else
                Return False
            End If

            If _udtGeneralFunction.getSytemParameterByParameterName("HSIVSS_DisplayDoseAgeCriteria_CheckingMethod", strParmValue1, strParmValue2) Then
                strCheckingMethod = strParmValue1
            Else
                Return False
            End If

            Dim udtClaimRuleBLL As ClaimRulesBLL = New ClaimRulesBLL()

            Return udtClaimRuleBLL.CheckIVSSAge(dtmServiceDate, strSchemeCode, udtEHSPersonalInformation, Convert.ToInt32(strCompareValue), strCompareUnit, strOperation, strCheckingMethod)

        End Function

    End Class

End Namespace