Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.EHSClaimVaccine
Imports HCVU.BLL
Imports Common.Component.Scheme
Imports Common.Format

Partial Public Class ucReadOnlyCOVID19MEC
    Inherits System.Web.UI.UserControl

    Private _udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL

#Region "Contants"
    Private Class PreExistingReason
        Public Const A2 = "A2"
    End Class
#End Region

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)
        'Render Default Label
        lblPartIForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalExemptionsPartI")
        lblPartIReasonForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalReason")
        lblPartIIForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalExemptionsPartII")
        lblComirnatyForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalExemptionsBioNTech")
        lblComirnatyReasonForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalReason")
        lblCoronaVacForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalExemptionsSinovac")
        lblCoronaVacReasonForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalReason")
        'lblIssueForExemptionText.Text = GetGlobalResourceObject("Text", "MedicalExemptionsDateOfIssue")
        lblValidForExemptionText.Text = GetGlobalResourceObject("Text", "ValidUntil")

        'Model in use
        Dim udtStaticData As New StaticDataBLL

        'Helper Class
        Dim udtFormatter As New Formatter

        Dim strPreExisting As String = udtEHSTransaction.TransactionAdditionFields.PreExisting
        Dim strPreExistingA2Remark As String = udtEHSTransaction.TransactionAdditionFields.PreExistingA2Remark
        Dim strContraindBioNTech As String = udtEHSTransaction.TransactionAdditionFields.ContraindBioNTech
        Dim strContraindSinovac As String = udtEHSTransaction.TransactionAdditionFields.ContraindSinovac
        Dim dtmValidUntil As DateTime = DateTime.Parse(udtFormatter.ConvertToDate(udtEHSTransaction.TransactionAdditionFields.ValidUntil))

        'Split String to Array
        Dim arrPreExisting As Array = Nothing
        Dim arrPreExistingA2Remark As Array = Nothing
        Dim arrContraindBioNTech As Array = Nothing
        Dim arrContraindSinovac As Array = Nothing

        If Not String.IsNullOrEmpty(strPreExisting) Then arrPreExisting = strPreExisting.Split(";")
        'If Not String.IsNullOrEmpty(strPart1ReasonRemark) Then arrPart1ReasonRemark = strPart1ReasonRemark.Split(New String() {"|||"}, StringSplitOptions.None)        
        If Not String.IsNullOrEmpty(strContraindBioNTech) Then arrContraindBioNTech = strContraindBioNTech.Split(";")
        If Not String.IsNullOrEmpty(strContraindSinovac) Then arrContraindSinovac = strContraindSinovac.Split(";")

        If Not String.IsNullOrEmpty(strPreExisting) Then
            'Part I
            If arrPreExisting IsNot Nothing AndAlso arrPreExisting.Length > 0 Then
                lblPartIForExemption.Text = Me.getReasons(arrPreExisting, "COVID19MEC_Pre-existing", strPreExistingA2Remark)
            End If

            'Patt II Hidden
            Me.ShowPart2(False)
        Else
            'Part I
            lblPartIForExemption.Text = "<li>" & Me.GetGlobalResourceObject("Text", "MedicalExemptionsProceedToPartII") & "</li>"

            'Part II - Show
            Me.ShowPart2(True)

            'Comirnaty
            If arrContraindBioNTech IsNot Nothing AndAlso arrContraindBioNTech.Length > 0 Then
                lblComirnatyReasonForExemption.Text = Me.getReasons(arrContraindBioNTech, "COVID19MEC_Contraind_BioNTech")
            End If

            'CoronaVac
            If arrContraindSinovac IsNot Nothing AndAlso arrContraindSinovac.Length > 0 Then
                lblCoronaVacReasonForExemption.Text = Me.getReasons(arrContraindSinovac, "COVID19MEC_Contraind_Sinovac")
            End If

        End If

        'Valid Until
        lblValidForExemption.Text = udtformatter.formatDisplayDate(dtmValidUntil)
        'Select Case _udtSessionHandler.Language
        '    Case CultureLanguage.English
        '        lblValidForExemption.Text = dtmValidUntil.ToString("dd-MMM-yyyy")
        '    Case Else
        '        lblValidForExemption.Text = dtmValidUntil.ToString("YYYY¦~MM¤ëdd¤é")
        'End Select

        'Issue Date
        'lblIssueForExemption.Text = formater.formatDisplayDate(udtEHSTransaction.ServiceDate)

    End Sub

    Private Function getReasons(ByVal arrReason As Array, ByVal strColumnName As String, Optional ByVal strDBRemark As String = "") As String
        Dim udtStaticData As New StaticDataBLL
        Dim strReason As String = String.Empty

        For intCt As Integer = 0 To arrReason.Length - 1
            Dim udtReasonModel As StaticDataModel = udtStaticData.GetStaticDataByColumnNameItemNo(strColumnName, arrReason(intCt), False)
            Dim strRemark = String.Empty

            'If arrRemark IsNot Nothing Then
            '    strRemark = arrRemark(intCt)
            'End If

            If HasRemark(arrReason(intCt)) Then
                strRemark = strDBRemark
            End If

            strReason &= "<li>" & Me.getProperLangReason(udtReasonModel) & strRemark & "</li>"
        Next

        Return strReason
    End Function

    Private Function getProperLangReason(ByVal udtModel As StaticDataModel) As String
        Select Case _udtSessionHandler.Language
            Case CultureLanguage.TradChinese
                Return udtModel.DataValueChi
            Case CultureLanguage.SimpChinese
                Return udtModel.DataValueCN
            Case Else
                Return udtModel.DataValue
        End Select
    End Function

    Private Sub ShowPart2(ByVal blnVisible As Boolean)
        If blnVisible Then
            trPart2Title.Style.Remove("display")
            trPart2Brand1Title.Style.Remove("display")
            trPart2Brand1Reason.Style.Remove("display")
            trPart2Brand2Title.Style.Remove("display")
            trPart2Brand2Reason.Style.Remove("display")
        Else
            trPart2Title.Style("display") = "none"
            trPart2Brand1Title.Style("display") = "none"
            trPart2Brand1Reason.Style("display") = "none"
            trPart2Brand2Title.Style("display") = "none"
            trPart2Brand2Reason.Style("display") = "none"
        End If

    End Sub

    Private Function HasRemark(ByVal strCode As String) As Boolean
        Dim arrWhiteList As New ArrayList

        arrWhiteList.Add(PreExistingReason.A2)

        Return arrWhiteList.Contains(strCode)

    End Function

End Class