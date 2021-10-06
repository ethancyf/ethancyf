Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimCategory
Imports Common.Component.RVPHomeList
Imports Common.Component.StaticData
Imports Common.Component
Imports Common.Component.EHSClaimVaccine
Imports HCVU.BLL
Imports Common.Component.Scheme

Partial Public Class ucReadOnlyVSS
    Inherits System.Web.UI.UserControl

    Public Class HighRiskOption
        Public Const HIGHRISK As String = "HIGHRISK"
        Public Const NOHIGHRISK As String = "NOHIGHRISK"
    End Class

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer, ByVal blnShowContactNoNotAbleToSMS As Boolean)
        trDocumentaryProof.Visible = False
        trPIDInstitutionCode.Visible = False
        trPIDInstitutionName.Visible = False
        trPlaceOfVaccination.Visible = False
        trRecipientCondition.Visible = False

        ' Category
        lblCategory.Text = (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(udtEHSTransaction.CategoryCode).GetCategoryName

        ' Type of Documentary Proof
        Dim strDocumentaryProof As String = udtEHSTransaction.TransactionAdditionFields.DocumentaryProof

        If Not IsNothing(strDocumentaryProof) Then
            trDocumentaryProof.Visible = True

            Dim udtStaticData As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo(String.Format("{0}_DOCUMENTARYPROOF", udtEHSTransaction.CategoryCode), strDocumentaryProof)
            lblDocumentaryProof.Text = udtStaticData.DataValue

            'CRE20-009 VSS Da with CSSA -  visible the content of checkboxes [Start][Nichole]
            If udtStaticData.ItemNo = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Or udtStaticData.ItemNo = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Then
                panVSSDAConfirm.Visible = True
            Else
                panVSSDAConfirm.Visible = False
            End If
            'CRE20-009 VSS Da with CSSA - visible the content of checkboxes [End][Nichole]
        End If



        ' PID Institution Code/Name
        Dim strPIDInstitutionCode As String = udtEHSTransaction.TransactionAdditionFields.PIDInstitutionCode

        If Not IsNothing(strPIDInstitutionCode) Then
            trPIDInstitutionCode.Visible = True
            trPIDInstitutionName.Visible = True

            lblPIDInstitutionCode.Text = strPIDInstitutionCode

            Dim dtRVPhomeList = (New RVPHomeListBLL).getRVPHomeListByCode(strPIDInstitutionCode)

            If dtRVPhomeList.Rows.Count > 0 Then
                Dim dr As DataRow = dtRVPhomeList.Rows(0)
                lblPIDInstitutionName.Text = dr("Homename_Eng").ToString.Trim

            End If

        End If

        ' Place of Vaccination (+- Others)
        Dim strPlaceVaccination As String = udtEHSTransaction.TransactionAdditionFields.PlaceVaccination

        If Not IsNothing(strPlaceVaccination) Then
            trPlaceOfVaccination.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_PLACEOFVACCINATION", strPlaceVaccination)
            lblPlaceOfVaccination.Text = udtStaticDataModel.DataValue

            ' Others
            Dim strPlaceVaccinationOthers As String = udtEHSTransaction.TransactionAdditionFields.PlaceVaccinationText

            If Not IsNothing(strPlaceVaccinationOthers) Then
                lblPlaceOfVaccination.Text += String.Format(" - {0}", strPlaceVaccinationOthers)
            End If

        End If

        udcReadOnlyVaccine.Build(udtEHSTransaction)

        ' Recipient Condition
        Dim strRecipientCondition As String = String.Empty

        If Not udtEHSTransaction.HighRisk Is Nothing Then
            Select Case udtEHSTransaction.HighRisk
                Case YesNo.Yes
                    strRecipientCondition = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strRecipientCondition = HighRiskOption.NOHIGHRISK
            End Select
        End If

        'Display Recipient Condition
        If strRecipientCondition <> String.Empty Then

            trRecipientCondition.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_RECIPIENTCONDITION", strRecipientCondition)
            lblRecipientCondition.Text = udtStaticDataModel.DataValue

        End If

        'ContactNo
        Dim strContactNo As String = udtEHSTransaction.TransactionAdditionFields.ContactNo
        If strContactNo IsNot Nothing AndAlso strContactNo <> String.Empty Then
            trContactNo.Visible = True
            lblContactNo.Text = strContactNo

            If blnShowContactNoNotAbleToSMS = True Then
                Select Case Left(strContactNo, 1)
                    Case "2", "3"
                        lblContactNoNotAbleSMS.Visible = True
                    Case Else
                        lblContactNoNotAbleSMS.Visible = False
                End Select
            Else
                lblContactNoNotAbleSMS.Visible = False
            End If

        Else
            trContactNo.Visible = False
        End If

        'Remarks
        Dim strRemarks As String = udtEHSTransaction.TransactionAdditionFields.Remarks
        If strRemarks Is Nothing Then
            trRemarks.Visible = True
            lblRemarks.Text = GetGlobalResourceObject("Text", "NA")
        Else
            trRemarks.Visible = True
            If strRemarks <> String.Empty Then
                lblRemarks.Text = strRemarks
            ElseIf strRemarks = String.Empty Then
                lblRemarks.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If
        End If

        ' Control the width of the first column
        tdCategory.Width = intWidth

    End Sub

End Class