Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData
Imports Common.Component.Scheme
Imports Common.Format

Partial Public Class ucReadOnlyCOVID19MEC
    Inherits ucReadOnlyEHSClaimBase

#Region "Contants"
    Private Class PreExistingReason
        Public Const A2 = "A2"
    End Class
#End Region

#Region "Properties"

#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        ' Text Field
        lblPartIForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalExemptionsPartI")
        lblPartIReasonForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalReason")
        lblPartIIForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalExemptionsPartII")
        lblComirnatyForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalExemptionsBioNTech")
        lblComirnatyReasonForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalReason")
        lblCoronaVacForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalExemptionsSinovac")
        lblCoronaVacReasonForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalReason")
        'lblIssueForExemptionText.Text = Me.GetGlobalResourceObject("Text", "MedicalExemptionsDateOfIssue")
        lblValidForExemptionText.Text = Me.GetGlobalResourceObject("Text", "ValidUntil")
        'lblJoinForExemptionText.Text = Me.GetGlobalResourceObject("Text", "JoinEHRSS")
        'lblContactForExemptionText.Text = Me.GetGlobalResourceObject("Text", "ContactNo2")

    End Sub

    Protected Overrides Sub Setup()

        'Model in use

        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction

        'Helper Class
        Dim udtFormatter As New Formatter

        'Value
        Dim strPreExisting As String = udtEHSTransaction.TransactionAdditionFields.PreExisting
        Dim strPreExistingA2Remark As String = udtEHSTransaction.TransactionAdditionFields.PreExistingA2Remark
        Dim strContraindBioNTech As String = udtEHSTransaction.TransactionAdditionFields.ContraindBioNTech
        Dim strContraindSinovac As String = udtEHSTransaction.TransactionAdditionFields.ContraindSinovac
        Dim dtmValidUntil As DateTime = DateTime.Parse(udtFormatter.ConvertToDate(udtEHSTransaction.TransactionAdditionFields.ValidUntil))
        Dim strContactNo As String = udtEHSTransaction.TransactionAdditionFields.ContactNo
        Dim strJoinEHealth As String = udtEHSTransaction.TransactionAdditionFields.JoinEHRSS

        'Split String to Array
        Dim arrPreExisting As Array = Nothing
        Dim arrPreExistingA2Remark As Array = Nothing
        Dim arrContraindBioNTech As Array = Nothing
        Dim arrContraindSinovac As Array = Nothing

        If Not String.IsNullOrEmpty(strPreExisting) Then arrPreExisting = strPreExisting.Split(";")
        'If Not String.IsNullOrEmpty(strPreExistingRemark) Then arrPreExistingA2Remark = strPreExistingRemark.Split(New String() {"|||"}, StringSplitOptions.None)
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
        lblValidForExemption.Text = udtFormatter.formatDisplayDate(dtmValidUntil)
        'Select Case MyBase.SessionHandler.Language
        '    Case CultureLanguage.English
        '        lblValidForExemption.Text = dtmValidUntil.ToString("dd-MMM-yyyy")
        '    Case Else
        '        lblValidForExemption.Text = dtmValidUntil.ToString("yyyy¦~MM¤ëdd¤é")
        'End Select

        ''Issue Date
        'lblIssueForExemption.Text = udtFormater.formatDisplayDate(udtEHSTransaction.ServiceDate)

        ''Join eHealth
        'If udtJoinEHealthString = "Y" Then
        '    lblJoinForExemption.Text = Me.GetGlobalResourceObject("Text", "Yes")
        'Else
        '    lblJoinForExemption.Text = Me.GetGlobalResourceObject("Text", "No")
        'End If

        ''Contact No.
        'lblContactForExemption.Text = udtContactNoString

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

            strReason &= "<li>" & Me.GetProperLangReason(udtReasonModel) & strRemark & "</li>"
        Next

        Return strReason

    End Function

    Private Function GetProperLangReason(ByVal udtModel As StaticDataModel) As String
        Select Case MyBase.SessionHandler.Language
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

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            tdCategoryForCovid19.Width = width
        Else
            tdCategoryForCovid19.Width = 200
        End If

    End Sub

    Private Function HasRemark(ByVal strCode As String) As Boolean
        Dim arrWhiteList As New ArrayList

        arrWhiteList.Add(PreExistingReason.A2)

        Return arrWhiteList.Contains(strCode)

    End Function

#End Region

End Class