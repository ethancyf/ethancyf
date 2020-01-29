Imports TestWSforHKMA.Component
Imports TestWSforHKMA.Component.Request
Imports TestWSforHKMA.Cryptography
Imports System.Xml
Imports System.Globalization
Imports System.Data.OleDb

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private blnUATMode As Boolean = False

#Region "View 1"

    Protected Sub btnProcced_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcced.Click, ButtonProceedTop.Click
        Dim strUATMode As String = String.Empty
        strUATMode = System.Configuration.ConfigurationManager.AppSettings("UATMode")

        If strUATMode.Trim.ToUpper = "Y" Then
            blnUATMode = True
        End If

        BtnEncryptAndSign.Enabled = True
        Me.txtResponse.Text = String.Empty

        If rboUploadClaimHL7.Checked Then
            Session("Selected_WS") = "6"
            WSCall_UploadClaim()
            Return
        End If

        If rboUploadClaim.Checked Then
            Session("Selected_WS") = "0"
            WSCall_UploadClaim()
            Return
        End If

        If rboRCHNameQuery.Checked Then
            Session("Selected_WS") = "1"
            WSCall_RCHNameQuery()
            Return
        End If

        If rboGetReasonForVisit.Checked Then
            Session("Selected_WS") = "2"
            WSCall_GetReasonForVisit()
            Return
        End If

        If rboEHSValidatedAccountQuery.Checked Then
            Session("Selected_WS") = "3"
            WSCall_EHSValidatedAccountQuery()
            Return
        End If

        If rboEHSAccountSubsidyQuery.Checked Then
            Session("Selected_WS") = "4"
            WSCall_EHSAccountSubsidyQuery()
            Return
        End If

        If rboSPPracticeValidation.Checked Then
            Session("Selected_WS") = "5"
            WSCall_SPPracticeValidation()
            Return
        End If

    End Sub

    Public Sub WSCall_UploadClaim()

        Dim udtUploadClaimRequest As UploadClaimRequest = New UploadClaimRequest()

        If chkSPInfo.Checked Then
            udtUploadClaimRequest.SPInfo_inXML = True

            'SP ID
            If chkSPID.Checked Then
                udtUploadClaimRequest.SPID_Included = True
                udtUploadClaimRequest.SPID = txtSPID.Text
            End If
            'SP Name
            If chkSPSurname.Checked Then
                udtUploadClaimRequest.SPSurname_Included = True
                udtUploadClaimRequest.SPSurname = txtSPSurname.Text
            End If
            If chkSPGivenname.Checked Then
                udtUploadClaimRequest.SPGivenName_Included = True
                udtUploadClaimRequest.SPGivenName = txtSPGivenName.Text
            End If
            'Practice ID
            If chkPracticeID.Checked Then
                udtUploadClaimRequest.PracticeID_included = True
                udtUploadClaimRequest.PracticeID = txtPracticeID.Text
            End If
            'Practice Name
            If chkPracticeName.Checked Then
                udtUploadClaimRequest.PracticeName_included = True
                udtUploadClaimRequest.PracticeName = txtPracticeName.Text
            End If
        End If

        If chkAccountInfo.Checked Then
            udtUploadClaimRequest.AccountInfo_inXML = True

            'Doc Type
            udtUploadClaimRequest.DocType_Included = True
            'If chkSPID.Checked Then
                'HKIC
                If rboHKIC.Checked Then
                    udtUploadClaimRequest.DocType = "HKIC"
                End If
                'EC
                If rboEC.Checked Then
                    udtUploadClaimRequest.DocType = "EC"
                End If
                'HKBC
                If rboHKBC.Checked Then
                    udtUploadClaimRequest.DocType = "HKBC"
                End If
                'ADOPC
                If rboADOPC.Checked Then
                    udtUploadClaimRequest.DocType = "ADOPC"
                End If
                'DOCI
                If rboDOCI.Checked Then
                    udtUploadClaimRequest.DocType = "Doc/I"
                End If
                'REPMT
                If rboREPMT.Checked Then
                    udtUploadClaimRequest.DocType = "REPMT"
                End If
                'VISA
                If rboVISA.Checked Then
                    udtUploadClaimRequest.DocType = "VISA"
                End If
                'ID235B
                If rboID235B.Checked Then
                    udtUploadClaimRequest.DocType = "ID235B"
                End If
            'End If
            'Entry No
            If chkEntryNo.Checked Then
                udtUploadClaimRequest.EntryNo_Included = True
                udtUploadClaimRequest.EntryNo = txtEntryNo.Text
            End If
            'Document No
            If chkDocumentNo.Checked Then
                udtUploadClaimRequest.DocumentNo_Included = True
                udtUploadClaimRequest.DocumentNo = txtDocumentNo.Text
            End If
            'HKIC
            If chkHKIC.Checked Then
                udtUploadClaimRequest.HKIC_Included = True
                udtUploadClaimRequest.HKIC = txtHKIC.Text
            End If
            'RegNo
            If chkRegNo.Checked Then
                udtUploadClaimRequest.RegNo_Included = True
                udtUploadClaimRequest.RegNo = txtRegNo.Text
            End If
            'BirthEntryNo
            If chkBirthEntryNo.Checked Then
                udtUploadClaimRequest.BirthEntryNo_Included = True
                udtUploadClaimRequest.BirthEntryNo = txtBirthEntryNo.Text
            End If
            'PermitNo
            If chkPermitNo.Checked Then
                udtUploadClaimRequest.PermitNo_Included = True
                udtUploadClaimRequest.PermitNo = txtPermitNo.Text
            End If
            'VisaNo
            If chkVISANo.Checked Then
                udtUploadClaimRequest.VISANo_Included = True
                udtUploadClaimRequest.VISANo = txtVISANo.Text
            End If
            'NameEng
            If chkSurname.Checked Then
                udtUploadClaimRequest.Surname_Included = True
                udtUploadClaimRequest.Surname = txtSurname.Text
            End If
            If chkGivenName.Checked Then
                udtUploadClaimRequest.GivenName_Included = True
                udtUploadClaimRequest.GivenName = txtGivenName.Text
            End If
            'Gender
            If chkGender.Checked Then
                udtUploadClaimRequest.Gender_Included = True
                udtUploadClaimRequest.Gender = txtGender.Text
            End If
            'DOB
            If chkDOB.Checked Then
                udtUploadClaimRequest.DOB_Included = True
                udtUploadClaimRequest.DOB = Me.ConvertDateDFormat(txtDOB.Text)
            End If
            'DOBType
            If chkDOBType.Checked Then
                udtUploadClaimRequest.DOBType_Included = True
                udtUploadClaimRequest.DOBType = txtDOBType.Text
            End If
            'AgeOn
            If chkAgeOn.Checked Then
                udtUploadClaimRequest.AgeOn_Included = True
                udtUploadClaimRequest.AgeOn = txtAgeOn.Text
            End If
            'DOReg
            If chkDOReg.Checked Then
                udtUploadClaimRequest.DOReg_Included = True
                udtUploadClaimRequest.DOReg = Me.ConvertDateDFormat(txtDOReg.Text)
            End If
            'DOBInWord
            If chkDOBInWord.Checked Then
                udtUploadClaimRequest.DOBInWord_Included = True
                udtUploadClaimRequest.DOBInWord = txtDOBInWord.Text
            End If
            'NameChi
            If chkNameChi.Checked Then
                udtUploadClaimRequest.NameChi_Included = True
                udtUploadClaimRequest.NameChi = txtNameChi.Text
            End If
            'DOI
            If chkDOI.Checked Then
                udtUploadClaimRequest.DOI_Included = True
                udtUploadClaimRequest.DOI = Me.ConvertDateDFormat(txtDOI.Text)
            End If
            'SerialNO
            If chkSerialNo.Checked Then
                udtUploadClaimRequest.SerialNo_Included = True
                udtUploadClaimRequest.SerialNo = txtSerialNo.Text
            End If
            'Reference
            If chkReference.Checked Then
                udtUploadClaimRequest.Reference_Included = True
                udtUploadClaimRequest.Reference = txtReference.Text
            End If
            'FreeRef
            If chkFreeReference.Checked Then
                udtUploadClaimRequest.FreeRef_Included = True
                udtUploadClaimRequest.FreeReference = txtFreeReference.Text
            End If
            'RemainUntil
            If chkRemainUntil.Checked Then
                udtUploadClaimRequest.RemainUntil_Included = True
                udtUploadClaimRequest.RemainUntil = Me.ConvertDateDFormat(txtRemainUntil.Text)
            End If
            'PassportNo
            If chkPassportNo.Checked Then
                udtUploadClaimRequest.PassportNo_Included = True
                udtUploadClaimRequest.PassportNo = txtPassportNo.Text
            End If
        End If


        If Me.chkClaimInfo.Checked Then

            udtUploadClaimRequest.ClaimInfo_inXML = True
            udtUploadClaimRequest.WSClaimDetailList = New WSClaimDetailModelCollection()

            'Service Date --------------------------------------------------------------------------------------------------------
            If chkServiceDate.Checked Then
                udtUploadClaimRequest.ServiceDate = Me.ConvertDateDFormat(Me.txtServiceDate.Text)
                udtUploadClaimRequest.ServiceDate_Included = True
            End If

            '------------------------------------------------------------------------------------------------------------------
            '                   Voucher
            '------------------------------------------------------------------------------------------------------------------

            'Voucher 1------------------------------------------------------------------------------------------------------------------
            If Me.chkHCVS.Checked Then

                udtUploadClaimRequest.WSClaimDetailList.Add(New WSClaimDetailModel)

                udtUploadClaimRequest.WSClaimDetailList.Item(0).HCVS_Included = True

                udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher = New WSVoucherModel()

                'Scheme Code
                udtUploadClaimRequest.WSClaimDetailList.Item(0).SchemeCode = "HCVS"
                udtUploadClaimRequest.WSClaimDetailList.Item(0).SchemeCode_Included = True

                'VoucherClaimed
                If chkVoucherClaimed.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.VoucherClaimed = Me.txtVoucherClaimed.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.VoucherClaimed_Included = True
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

                ' -----------------------------------------------------------------------------------------

                'Co-Payment Fee
                If chkCoPaymentFee.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.CoPaymentFee = Me.txtCoPaymentFee.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.CoPaymentFee_Included = True
                End If

                'Primary

                If chkProfCode.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ReasonForVisit_Included = True
                End If

                'ProfCode
                If chkProfCode.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode = Me.txtProfCode.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_Included = True
                End If

                'PriorityCode
                If chkPriorityCode.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode = Me.txtPriorityCode.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode_Included = True
                End If

                'L1Code
                If chkL1Code.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code = Me.txtL1Code.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code_Included = True
                End If

                'L1DescEng
                If chkL1DescEng.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng = Me.txtL1DescEng.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng_Included = True
                End If

                'L2Code
                If chkL2Code.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code = Me.txtL2Code.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code_Included = True
                End If

                'L2DescEng
                If chkL2DescEng.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng = Me.txtL2DescEng.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng_Included = True
                End If

                'S1

                If chkProfCode_S1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ReasonForVisit_S1_Included = True
                End If

                'ProfCode_S1
                If chkProfCode_S1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_S1 = Me.txtProfCode_S1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_S1_Included = True
                End If

                'PriorityCode_S1
                If chkPriorityCode_S1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode_S1 = Me.txtPriorityCode_S1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode_S1_Included = True
                End If

                'L1Code_S1
                If chkL1Code_S1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code_S1 = Me.txtL1Code_S1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code_S1_Included = True
                End If

                'L1DescEng_S1
                If chkL1DescEng_S1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng_S1 = Me.txtL1DescEng_S1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng_S1_Included = True
                End If

                'L2Code_S1
                If chkL2Code_S1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code_S1 = Me.txtL2Code_S1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code_S1_Included = True
                End If

                'L2DescEng_S1
                If chkL2DescEng_S1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng_S1 = Me.txtL2DescEng_S1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng_S1_Included = True
                End If

                'S2

                If chkProfCode_S2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ReasonForVisit_S2_Included = True
                End If

                'ProfCode_S2
                If chkProfCode_S2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_S2 = Me.txtProfCode_S2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_S2_Included = True
                End If

                'PriorityCode_S2
                If chkPriorityCode_S2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode_S2 = Me.txtPriorityCode_S2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode_S2_Included = True
                End If

                'L1Code_S2
                If chkL1Code_S2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code_S2 = Me.txtL1Code_S2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code_S2_Included = True
                End If

                'L1DescEng_S2
                If chkL1DescEng_S2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng_S2 = Me.txtL1DescEng_S2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng_S2_Included = True
                End If

                'L2Code_S2
                If chkL2Code_S2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code_S2 = Me.txtL2Code_S2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code_S2_Included = True
                End If

                'L2DescEng_S2
                If chkL2DescEng_S2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng_S2 = Me.txtL2DescEng_S2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng_S2_Included = True
                End If

                'S3

                If chkProfCode_S3.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ReasonForVisit_S3_Included = True
                End If

                'ProfCode_S3
                If chkProfCode_S3.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_S3 = Me.txtProfCode_S3.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_S3_Included = True
                End If

                'PriorityCode_S3
                If chkPriorityCode_S3.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode_S3 = Me.txtPriorityCode_S3.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.PriorityCode_S3_Included = True
                End If

                'L1Code_S3
                If chkL1Code_S3.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code_S3 = Me.txtL1Code_S3.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1Code_S3_Included = True
                End If

                'L1DescEng_S3
                If chkL1DescEng_S3.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng_S3 = Me.txtL1DescEng_S3.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L1DescEng_S3_Included = True
                End If

                'L2Code_S3
                If chkL2Code_S3.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code_S3 = Me.txtL2Code_S3.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2Code_S3_Included = True
                End If

                'L2DescEng_S3
                If chkL2DescEng_S3.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng_S3 = Me.txtL2DescEng_S3.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.L2DescEng_S3_Included = True
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
            End If

            'PreSchoolInd
            If chkPreSchoolInd.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(0).PreSchoolInd = Me.txtPreSchoolInd.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(0).PreSchoolInd_Included = True
            End If

            'DoseIntervalInd
            If chkDoseIntervalInd.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(0).DoseIntervalInd = Me.txtDoseIntervalInd.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(0).DoseIntervalInd_Included = True
            End If

            'TSWInd
            If chkTSWInd.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(0).TSWInd = Me.txtTSWInd.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(0).TSWInd_Included = True
            End If

            'Voucher 2------------------------------------------------------------------------------------------------------------------
            If Me.chkHCVS.Checked And Me.chkHCVS2.Checked Then

                udtUploadClaimRequest.WSClaimDetailList.Add(New WSClaimDetailModel)

                udtUploadClaimRequest.WSClaimDetailList.Item(1).HCVS_Included = True

                udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher = New WSVoucherModel()

                'Scheme Code
                udtUploadClaimRequest.WSClaimDetailList.Item(1).SchemeCode = "HCVS"
                udtUploadClaimRequest.WSClaimDetailList.Item(1).SchemeCode_Included = True
                'VoucherClaimed
                If chkVoucherClaimed2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.VoucherClaimed = Me.txtVoucherClaimed2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.VoucherClaimed_Included = True
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

                ' -----------------------------------------------------------------------------------------

                'Co-Payment Fee
                If chkCoPaymentFee2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.CoPaymentFee = Me.txtCoPaymentFee2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.CoPaymentFee_Included = True
                End If

                'Primary

                udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ReasonForVisit_Included = True

                'ProfCode
                If chkProfCode2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode = Me.txtProfCode2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode_Included = True
                End If

                'PriorityCode
                If chkPriorityCode2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode = Me.txtPriorityCode2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode_Included = True
                End If

                'L1Code
                If chkL1Code2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code = Me.txtL1Code2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code_Included = True
                End If

                'L1DescEng
                If chkL1DescEng2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng = Me.txtL1DescEng2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng_Included = True
                End If

                'L2Code
                If chkL2Code2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code = Me.txtL2Code2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code_Included = True
                End If

                'L2DescEng
                If chkL2DescEng2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng = Me.txtL2DescEng2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng_Included = True
                End If

                'S1

                udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ReasonForVisit_S1_Included = True

                'ProfCode_S1
                If chkProfCode_S12.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode_S1 = Me.txtProfCode_S12.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode_S1_Included = True
                End If

                'PriorityCode_S1
                If chkPriorityCode_S12.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode_S1 = Me.txtPriorityCode_S12.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode_S1_Included = True
                End If

                'L1Code_S1
                If chkL1Code_S12.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code_S1 = Me.txtL1Code_S12.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code_S1_Included = True
                End If

                'L1DescEng_S1
                If chkL1DescEng_S12.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng_S1 = Me.txtL1DescEng_S12.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng_S1_Included = True
                End If

                'L2Code_S1
                If chkL2Code_S12.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code_S1 = Me.txtL2Code_S12.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code_S1_Included = True
                End If

                'L2DescEng_S1
                If chkL2DescEng_S12.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng_S1 = Me.txtL2DescEng_S12.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng_S1_Included = True
                End If

                'S2

                udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ReasonForVisit_S2_Included = True

                'ProfCode_S2
                If chkProfCode_S22.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode_S2 = Me.txtProfCode_S22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode_S2_Included = True
                End If

                'PriorityCode_S2
                If chkPriorityCode_S22.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode_S2 = Me.txtPriorityCode_S22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode_S2_Included = True
                End If

                'L1Code_S2
                If chkL1Code_S22.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code_S2 = Me.txtL1Code_S22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code_S2_Included = True
                End If

                'L1DescEng_S2
                If chkL1DescEng_S22.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng_S2 = Me.txtL1DescEng_S22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng_S2_Included = True
                End If

                'L2Code_S2
                If chkL2Code_S22.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code_S2 = Me.txtL2Code_S22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code_S2_Included = True
                End If

                'L2DescEng_S2
                If chkL2DescEng_S22.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng_S2 = Me.txtL2DescEng_S22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng_S2_Included = True
                End If

                'S3

                udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ReasonForVisit_S3_Included = True

                'ProfCode_S3
                If chkProfCode_S32.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode_S3 = Me.txtProfCode_S32.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.ProfCode_S3_Included = True
                End If

                'PriorityCode_S3
                If chkPriorityCode_S32.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode_S3 = Me.txtPriorityCode_S32.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.PriorityCode_S3_Included = True
                End If

                'L1Code_S3
                If chkL1Code_S32.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code_S3 = Me.txtL1Code_S32.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1Code_S3_Included = True
                End If

                'L1DescEng_S3
                If chkL1DescEng_S32.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng_S3 = Me.txtL1DescEng_S32.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L1DescEng_S3_Included = True
                End If

                'L2Code_S3
                If chkL2Code_S32.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code_S3 = Me.txtL2Code_S32.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2Code_S3_Included = True
                End If

                'L2DescEng_S3
                If chkL2DescEng_S32.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng_S3 = Me.txtL2DescEng_S32.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(1).WSVoucher.L2DescEng_S3_Included = True
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            End If

            'PreSchoolInd
            If chkPreSchoolInd2.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(1).PreSchoolInd = Me.txtPreSchoolInd2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(1).PreSchoolInd_Included = True
            End If

            'DoseIntervalInd
            If chkDoseIntervalInd2.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(1).DoseIntervalInd = Me.txtDoseIntervalInd2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(1).DoseIntervalInd_Included = True
            End If

            'TSWInd
            If chkTSWInd2.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(1).TSWInd = Me.txtTSWInd2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(1).TSWInd_Included = True
            End If


            '------------------------------------------------------------------------------------------------------------------
            '                   Vaccination
            '------------------------------------------------------------------------------------------------------------------


            'Vaccination 1 ---> Vaccine 1------------------------------------------------------------------------------------------------------------------
            Dim i As Integer = 0

            If chkVaccine1.Checked Then

                udtUploadClaimRequest.WSClaimDetailList.Add(New WSClaimDetailModel)

                i = udtUploadClaimRequest.WSClaimDetailList.Count
                i = i - 1
                If IsNothing(udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList) Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList = New WSVaccineDetailModelCollection()
                End If

                udtUploadClaimRequest.WSClaimDetailList.Item(i).Vaccine_Included = True
                udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Add(New WSVaccineDetailModel())

                'Scheme Code
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode = txtSchemeCode.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode_Included = True

                'Subsidy Code
                If chkSubsidyCode1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).SubsidyCode = Me.txtSubsidyCode1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).SubsidyCode_included = True
                End If

                'Dose Seq
                If chkDoseSeq1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).DoseSeq = Me.txtDoseSeq1.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).DoseSeq_included = True
                End If

                'RCH Code
                If chkRCHCode_Vaccine1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).RCHCode_Included = True
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).RCHCode = txtRCHCode_Vaccine1.Text
                End If
            End If
            'Vaccination 1 ---> Vaccine 2------------------------------------------------------------------------------------------------------------------
            If chkVaccine1.Checked And chkVaccine2.Checked Then

                'Dim x As Integer = 1
                'If Not chkVaccine1.Checked Then
                '    udtUploadClaimRequest.WSClaimDetailList.Add(New WSClaimDetailModel)
                '    x = 0
                'End If
                'Dim j As Integer = udtUploadClaimRequest.WSClaimDetailList.Count
                'j = j - 1
                If IsNothing(udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList) Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList = New WSVaccineDetailModelCollection()
                End If
                udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Add(New WSVaccineDetailModel())

                'Scheme Code
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode = txtSchemeCode.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode_Included = True

                'Subsidy Code
                If chkSubsidyCode1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).SubsidyCode = Me.txtSubsidyCode2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).SubsidyCode_included = True
                End If

                'Dose Seq
                If chkDoseSeq1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).DoseSeq = Me.txtDoseSeq2.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).DoseSeq_included = True
                End If
            End If

            'Vaccine Indicator
            'PreSchoolInd
            If chkPreSchoolInd_vaccine.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(i).PreSchoolInd = Me.txtPreSchoolInd_vaccine.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).PreSchoolInd_Included = True
            End If

            'DoseIntervalInd
            If chkDoseIntervalInd_vaccine.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(i).DoseIntervalInd = Me.txtDoseIntervalInd_vaccine.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).DoseIntervalInd_Included = True
            End If

            'TSWInd
            If chkTSWInd_vaccine.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(i).TSWInd = Me.txtTSWInd_vaccine.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).TSWInd_Included = True
            End If


            'Vaccination 2 ---> Vaccine 1------------------------------------------------------------------------------------------------------------------
            If chkVaccine21.Checked Then

                udtUploadClaimRequest.WSClaimDetailList.Add(New WSClaimDetailModel)

                i = udtUploadClaimRequest.WSClaimDetailList.Count
                i = i - 1
                If IsNothing(udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList) Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList = New WSVaccineDetailModelCollection()
                End If

                udtUploadClaimRequest.WSClaimDetailList.Item(i).Vaccine_Included = True
                udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Add(New WSVaccineDetailModel())

                'Scheme Code
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode = txtSchemeCode2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode_Included = True

                'Subsidy Code
                If chkSubsidyCode1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).SubsidyCode = Me.txtSubsidyCode21.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).SubsidyCode_included = True
                End If

                'Dose Seq
                If chkDoseSeq1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).DoseSeq = Me.txtDoseSeq21.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(0).DoseSeq_included = True
                End If

                'RCH Code
                If chkRCHCode_Vaccine2.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).RCHCode_Included = True
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).RCHCode = txtRCHCode_Vaccine2.Text
                End If
            End If

            'Vaccination 2 ---> Vaccine 2------------------------------------------------------------------------------------------------------------------
            If chkVaccine21.Checked And chkVaccine22.Checked Then

                If IsNothing(udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList) Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList = New WSVaccineDetailModelCollection()
                End If
                udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Add(New WSVaccineDetailModel())

                'Scheme Code
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode = txtSchemeCode2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).SchemeCode_Included = True

                'Subsidy Code
                If chkSubsidyCode1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).SubsidyCode = Me.txtSubsidyCode22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).SubsidyCode_included = True
                End If

                'Dose Seq
                If chkDoseSeq1.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).DoseSeq = Me.txtDoseSeq22.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).WSVaccineDetailList.Item(1).DoseSeq_included = True
                End If
            End If

            'Vaccine Indicator
            'PreSchoolInd
            If chkPreSchoolInd_vaccine2.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(i).PreSchoolInd = Me.txtPreSchoolInd_vaccine2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).PreSchoolInd_Included = True
            End If

            'DoseIntervalInd
            If chkDoseIntervalInd_vaccine2.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(i).DoseIntervalInd = Me.txtDoseIntervalInd_vaccine2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).DoseIntervalInd_Included = True
            End If

            'TSWInd
            If chkTSWInd_vaccine2.Checked Then
                udtUploadClaimRequest.WSClaimDetailList.Item(i).TSWInd = Me.txtTSWInd_vaccine2.Text
                udtUploadClaimRequest.WSClaimDetailList.Item(i).TSWInd_Included = True
            End If



        End If

        Dim xmlDocument As XmlDocument = Nothing
        If Session("Selected_WS") = "0" Then
            If blnUATMode Then
                xmlDocument = udtUploadClaimRequest.GenXMLResult()
                CallWSandShowResult(xmlDocument.InnerXml)
            Else
                Me.mveTest.ActiveViewIndex = 1
                Me.txtResult.Text = xmlDocument.InnerXml
            End If
        Else
            If blnUATMode Then
                xmlDocument = udtUploadClaimRequest.GenXMLResult_HL7()
                CallWSandShowResult(xmlDocument.InnerXml.Replace("<?xml version=""1.0"" encoding=""utf-8""?>", "<?xml version=""1.0"" encoding=""UTF-8""?><Request><Input>") + "</Input></Request>")
            Else
                xmlDocument = udtUploadClaimRequest.GenXMLResult_HL7()
                Me.mveTest.ActiveViewIndex = 1
                Me.txtResult.Text = xmlDocument.InnerXml.Replace("<?xml version=""1.0"" encoding=""utf-8""?>", "<?xml version=""1.0"" encoding=""UTF-8""?><Request><Input>") + "</Input></Request>"
            End If
        End If

    End Sub

    Public Sub WSCall_RCHNameQuery()

        Dim udtRCHNameQueryRequest As RCHNameQueryRequest = New RCHNameQueryRequest()

        If chkSPInfo.Checked Then
            udtRCHNameQueryRequest.SPInfo_inXML = True

            'SP ID
            If chkSPID.Checked Then
                udtRCHNameQueryRequest.SPID_Included = True
                udtRCHNameQueryRequest.SPID = txtSPID.Text
            End If
            'SP Name
            If chkSPSurname.Checked Then
                udtRCHNameQueryRequest.SPSurname_Included = True
                udtRCHNameQueryRequest.SPSurname = txtSPSurname.Text
            End If
            If chkSPGivenname.Checked Then
                udtRCHNameQueryRequest.SPGivenName_Included = True
                udtRCHNameQueryRequest.SPGivenName = txtSPGivenName.Text
            End If
            'Practice ID
            If chkPracticeID.Checked Then
                udtRCHNameQueryRequest.PracticeID_included = True
                udtRCHNameQueryRequest.PracticeID = txtPracticeID.Text
            End If
            'Practice Name
            If chkPracticeName.Checked Then
                udtRCHNameQueryRequest.PracticeName_included = True
                udtRCHNameQueryRequest.PracticeName = txtPracticeName.Text
            End If

            'RCH Code
            If chkRCHCode.Checked Then
                udtRCHNameQueryRequest.RCHCode_Included = True
                udtRCHNameQueryRequest.RCHCode = txtRCHCode.Text
            End If
        End If

        'udtUploadClaimRequest.WSClaimDetailList

        Dim xmlDocument As XmlDocument = Nothing
        xmlDocument = udtRCHNameQueryRequest.GenXMLResult()

        If blnUATMode Then
            CallWSandShowResult(xmlDocument.InnerXml)
        Else
            Me.mveTest.ActiveViewIndex = 1
            Me.txtResult.Text = xmlDocument.InnerXml
            Me.txtResponse.Text = String.Empty
        End If

    End Sub

    Public Sub WSCall_GetReasonForVisit()

        If blnUATMode Then
            CallWSandShowResult("<?xml version=""1.0"" encoding=""utf-8""?><Request><Input></Input></Request>")
        Else
            Me.mveTest.ActiveViewIndex = 1
            Me.txtResult.Text = "<?xml version=""1.0"" encoding=""utf-8""?><Request><Input></Input></Request>"
            Me.txtResponse.Text = String.Empty
        End If
    End Sub

    Public Sub WSCall_EHSAccountSubsidyQuery()

        Dim udteHSAccountSubsidyQueryRequest As eHSAccountSubsidyQueryRequest = New eHSAccountSubsidyQueryRequest()

        If chkSPInfo.Checked Then
            udteHSAccountSubsidyQueryRequest.SPInfo_inXML = True

            'SP ID
            If chkSPID.Checked Then
                udteHSAccountSubsidyQueryRequest.SPID_Included = True
                udteHSAccountSubsidyQueryRequest.SPID = txtSPID.Text
            End If
            'SP Name
            If chkSPSurname.Checked Then
                udteHSAccountSubsidyQueryRequest.SPSurname_Included = True
                udteHSAccountSubsidyQueryRequest.SPSurname = txtSPSurname.Text
            End If
            If chkSPGivenname.Checked Then
                udteHSAccountSubsidyQueryRequest.SPGivenName_Included = True
                udteHSAccountSubsidyQueryRequest.SPGivenName = txtSPGivenName.Text
            End If
            'Practice ID
            If chkPracticeID.Checked Then
                udteHSAccountSubsidyQueryRequest.PracticeID_included = True
                udteHSAccountSubsidyQueryRequest.PracticeID = txtPracticeID.Text
            End If
            'Practice Name
            If chkPracticeName.Checked Then
                udteHSAccountSubsidyQueryRequest.PracticeName_included = True
                udteHSAccountSubsidyQueryRequest.PracticeName = txtPracticeName.Text
            End If

        End If

        If chkAccountInfo.Checked Then
            udteHSAccountSubsidyQueryRequest.AccountInfo_inXML = True

            'Doc Type
            udteHSAccountSubsidyQueryRequest.DocType_Included = True
            If chkSPID.Checked Then
                'HKIC
                If rboHKIC.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "HKIC"
                End If
                'EC
                If rboEC.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "EC"
                End If
                'HKBC
                If rboHKBC.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "HKBC"
                End If
                'ADOPC
                If rboADOPC.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "ADOPC"
                End If
                'DOCI
                If rboDOCI.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "Doc/I"
                End If
                'REPMT
                If rboREPMT.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "REPMT"
                End If
                'VISA
                If rboVISA.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "VISA"
                End If
                'ID235B
                If rboID235B.Checked Then
                    udteHSAccountSubsidyQueryRequest.DocType = "ID235B"
                End If
            End If
            'Entry No
            If chkEntryNo.Checked Then
                udteHSAccountSubsidyQueryRequest.EntryNo_Included = True
                udteHSAccountSubsidyQueryRequest.EntryNo = txtEntryNo.Text
            End If
            'Document No
            If chkDocumentNo.Checked Then
                udteHSAccountSubsidyQueryRequest.DocumentNo_Included = True
                udteHSAccountSubsidyQueryRequest.DocumentNo = txtDocumentNo.Text
            End If
            'HKIC
            If chkHKIC.Checked Then
                udteHSAccountSubsidyQueryRequest.HKIC_Included = True
                udteHSAccountSubsidyQueryRequest.HKIC = txtHKIC.Text
            End If
            'RegNo
            If chkRegNo.Checked Then
                udteHSAccountSubsidyQueryRequest.RegNo_Included = True
                udteHSAccountSubsidyQueryRequest.RegNo = txtRegNo.Text
            End If
            'BirthEntryNo
            If chkBirthEntryNo.Checked Then
                udteHSAccountSubsidyQueryRequest.BirthEntryNo_Included = True
                udteHSAccountSubsidyQueryRequest.BirthEntryNo = txtBirthEntryNo.Text
            End If
            'PermitNo
            If chkPermitNo.Checked Then
                udteHSAccountSubsidyQueryRequest.PermitNo_Included = True
                udteHSAccountSubsidyQueryRequest.PermitNo = txtPermitNo.Text
            End If
            'VisaNo
            If chkVISANo.Checked Then
                udteHSAccountSubsidyQueryRequest.VISANo_Included = True
                udteHSAccountSubsidyQueryRequest.VISANo = txtVISANo.Text
            End If
            'NameEng
            If chkSurname.Checked Then
                udteHSAccountSubsidyQueryRequest.Surname_Included = True
                udteHSAccountSubsidyQueryRequest.Surname = txtSurname.Text
            End If
            If chkGivenName.Checked Then
                udteHSAccountSubsidyQueryRequest.GivenName_Included = True
                udteHSAccountSubsidyQueryRequest.GivenName = txtGivenName.Text
            End If
            'Gender
            If chkGender.Checked Then
                udteHSAccountSubsidyQueryRequest.Gender_Included = True
                udteHSAccountSubsidyQueryRequest.Gender = txtGender.Text
            End If
            'DOB
            If chkDOB.Checked Then
                udteHSAccountSubsidyQueryRequest.DOB_Included = True
                udteHSAccountSubsidyQueryRequest.DOB = txtDOB.Text
            End If
            'DOBType
            If chkDOBType.Checked Then
                udteHSAccountSubsidyQueryRequest.DOBType_Included = True
                udteHSAccountSubsidyQueryRequest.DOBType = txtDOBType.Text
            End If
            'AgeOn
            If chkAgeOn.Checked Then
                udteHSAccountSubsidyQueryRequest.AgeOn_Included = True
                udteHSAccountSubsidyQueryRequest.AgeOn = txtAgeOn.Text
            End If
            'DOReg
            If chkDOReg.Checked Then
                udteHSAccountSubsidyQueryRequest.DOReg_Included = True
                udteHSAccountSubsidyQueryRequest.DOReg = txtDOReg.Text
            End If
            'DOBInWord
            If chkDOBInWord.Checked Then
                udteHSAccountSubsidyQueryRequest.DOBInWord_Included = True
                udteHSAccountSubsidyQueryRequest.DOBInWord = txtDOBInWord.Text
            End If
            'NameChi
            If chkNameChi.Checked Then
                udteHSAccountSubsidyQueryRequest.NameChi_Included = True
                udteHSAccountSubsidyQueryRequest.NameChi = txtNameChi.Text
            End If
            'DOI
            If chkDOI.Checked Then
                udteHSAccountSubsidyQueryRequest.DOI_Included = True
                udteHSAccountSubsidyQueryRequest.DOI = txtDOI.Text
            End If
            'SerialNO
            If chkSerialNo.Checked Then
                udteHSAccountSubsidyQueryRequest.SerialNo_Included = True
                udteHSAccountSubsidyQueryRequest.SerialNo = txtSerialNo.Text
            End If
            'Reference
            If chkReference.Checked Then
                udteHSAccountSubsidyQueryRequest.Reference_Included = True
                udteHSAccountSubsidyQueryRequest.Reference = txtReference.Text
            End If
            'FreeRef
            If chkFreeReference.Checked Then
                udteHSAccountSubsidyQueryRequest.FreeRef_Included = True
                udteHSAccountSubsidyQueryRequest.FreeReference = txtFreeReference.Text
            End If
            'RemainUntil
            If chkRemainUntil.Checked Then
                udteHSAccountSubsidyQueryRequest.RemainUntil_Included = True
                udteHSAccountSubsidyQueryRequest.RemainUntil = txtRemainUntil.Text
            End If
            'PassportNo
            If chkPassportNo.Checked Then
                udteHSAccountSubsidyQueryRequest.PassportNo_Included = True
                udteHSAccountSubsidyQueryRequest.PassportNo = txtPassportNo.Text
            End If
        End If

        Dim xmlDocument As XmlDocument = Nothing
        xmlDocument = udteHSAccountSubsidyQueryRequest.GenXMLResult()

        If blnUATMode Then
            CallWSandShowResult(xmlDocument.InnerXml)
        Else
            Me.mveTest.ActiveViewIndex = 1
            Me.txtResult.Text = xmlDocument.InnerXml
            Me.txtResponse.Text = String.Empty
        End If

    End Sub

    Public Sub WSCall_EHSValidatedAccountQuery()

        Dim udteHSValidatedAccountQueryRequest As eHSValidatedAccountQueryRequest = New eHSValidatedAccountQueryRequest()

        If chkSPInfo.Checked Then
            udteHSValidatedAccountQueryRequest.SPInfo_inXML = True

            'SP ID
            If chkSPID.Checked Then
                udteHSValidatedAccountQueryRequest.SPID_Included = True
                udteHSValidatedAccountQueryRequest.SPID = txtSPID.Text
            End If
            'SP Name
            If chkSPSurname.Checked Then
                udteHSValidatedAccountQueryRequest.SPSurname_Included = True
                udteHSValidatedAccountQueryRequest.SPSurname = txtSPSurname.Text
            End If
            If chkSPGivenname.Checked Then
                udteHSValidatedAccountQueryRequest.SPGivenName_Included = True
                udteHSValidatedAccountQueryRequest.SPGivenName = txtSPGivenName.Text
            End If
            'Practice ID
            If chkPracticeID.Checked Then
                udteHSValidatedAccountQueryRequest.PracticeID_included = True
                udteHSValidatedAccountQueryRequest.PracticeID = txtPracticeID.Text
            End If
            'Practice Name
            If chkPracticeName.Checked Then
                udteHSValidatedAccountQueryRequest.PracticeName_included = True
                udteHSValidatedAccountQueryRequest.PracticeName = txtPracticeName.Text
            End If

        End If

        If chkAccountInfo.Checked Then
            udteHSValidatedAccountQueryRequest.AccountInfo_inXML = True

            'Doc Type
            udteHSValidatedAccountQueryRequest.DocType_Included = True
            If chkSPID.Checked Then
                'HKIC
                If rboHKIC.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "HKIC"
                End If
                'EC
                If rboEC.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "EC"
                End If
                'HKBC
                If rboHKBC.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "HKBC"
                End If
                'ADOPC
                If rboADOPC.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "ADOPC"
                End If
                'DOCI
                If rboDOCI.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "Doc/I"
                End If
                'REPMT
                If rboREPMT.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "REPMT"
                End If
                'VISA
                If rboVISA.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "VISA"
                End If
                'ID235B
                If rboID235B.Checked Then
                    udteHSValidatedAccountQueryRequest.DocType = "ID235B"
                End If
            End If
            'Entry No
            If chkEntryNo.Checked Then
                udteHSValidatedAccountQueryRequest.EntryNo_Included = True
                udteHSValidatedAccountQueryRequest.EntryNo = txtEntryNo.Text
            End If
            'Document No
            If chkDocumentNo.Checked Then
                udteHSValidatedAccountQueryRequest.DocumentNo_Included = True
                udteHSValidatedAccountQueryRequest.DocumentNo = txtDocumentNo.Text
            End If
            'HKIC
            If chkHKIC.Checked Then
                udteHSValidatedAccountQueryRequest.HKIC_Included = True
                udteHSValidatedAccountQueryRequest.HKIC = txtHKIC.Text
            End If
            'RegNo
            If chkRegNo.Checked Then
                udteHSValidatedAccountQueryRequest.RegNo_Included = True
                udteHSValidatedAccountQueryRequest.RegNo = txtRegNo.Text
            End If
            'BirthEntryNo
            If chkBirthEntryNo.Checked Then
                udteHSValidatedAccountQueryRequest.BirthEntryNo_Included = True
                udteHSValidatedAccountQueryRequest.BirthEntryNo = txtBirthEntryNo.Text
            End If
            'PermitNo
            If chkPermitNo.Checked Then
                udteHSValidatedAccountQueryRequest.PermitNo_Included = True
                udteHSValidatedAccountQueryRequest.PermitNo = txtPermitNo.Text
            End If
            'VisaNo
            If chkVISANo.Checked Then
                udteHSValidatedAccountQueryRequest.VISANo_Included = True
                udteHSValidatedAccountQueryRequest.VISANo = txtVISANo.Text
            End If
            'NameEng
            If chkSurname.Checked Then
                udteHSValidatedAccountQueryRequest.Surname_Included = True
                udteHSValidatedAccountQueryRequest.Surname = txtSurname.Text
            End If
            If chkGivenName.Checked Then
                udteHSValidatedAccountQueryRequest.GivenName_Included = True
                udteHSValidatedAccountQueryRequest.GivenName = txtGivenName.Text
            End If
            'Gender
            If chkGender.Checked Then
                udteHSValidatedAccountQueryRequest.Gender_Included = True
                udteHSValidatedAccountQueryRequest.Gender = txtGender.Text
            End If
            'DOB
            If chkDOB.Checked Then
                udteHSValidatedAccountQueryRequest.DOB_Included = True
                udteHSValidatedAccountQueryRequest.DOB = txtDOB.Text
            End If
            'DOBType
            If chkDOBType.Checked Then
                udteHSValidatedAccountQueryRequest.DOBType_Included = True
                udteHSValidatedAccountQueryRequest.DOBType = txtDOBType.Text
            End If
            'AgeOn
            If chkAgeOn.Checked Then
                udteHSValidatedAccountQueryRequest.AgeOn_Included = True
                udteHSValidatedAccountQueryRequest.AgeOn = txtAgeOn.Text
            End If
            'DOReg
            If chkDOReg.Checked Then
                udteHSValidatedAccountQueryRequest.DOReg_Included = True
                udteHSValidatedAccountQueryRequest.DOReg = txtDOReg.Text
            End If
            'DOBInWord
            If chkDOBInWord.Checked Then
                udteHSValidatedAccountQueryRequest.DOBInWord_Included = True
                udteHSValidatedAccountQueryRequest.DOBInWord = txtDOBInWord.Text
            End If
            'NameChi
            If chkNameChi.Checked Then
                udteHSValidatedAccountQueryRequest.NameChi_Included = True
                udteHSValidatedAccountQueryRequest.NameChi = txtNameChi.Text
            End If
            'DOI
            If chkDOI.Checked Then
                udteHSValidatedAccountQueryRequest.DOI_Included = True
                udteHSValidatedAccountQueryRequest.DOI = txtDOI.Text
            End If
            'SerialNO
            If chkSerialNo.Checked Then
                udteHSValidatedAccountQueryRequest.SerialNo_Included = True
                udteHSValidatedAccountQueryRequest.SerialNo = txtSerialNo.Text
            End If
            'Reference
            If chkReference.Checked Then
                udteHSValidatedAccountQueryRequest.Reference_Included = True
                udteHSValidatedAccountQueryRequest.Reference = txtReference.Text
            End If
            'FreeRef
            If chkFreeReference.Checked Then
                udteHSValidatedAccountQueryRequest.FreeRef_Included = True
                udteHSValidatedAccountQueryRequest.FreeReference = txtFreeReference.Text
            End If
            'RemainUntil
            If chkRemainUntil.Checked Then
                udteHSValidatedAccountQueryRequest.RemainUntil_Included = True
                udteHSValidatedAccountQueryRequest.RemainUntil = txtRemainUntil.Text
            End If
            'PassportNo
            If chkPassportNo.Checked Then
                udteHSValidatedAccountQueryRequest.PassportNo_Included = True
                udteHSValidatedAccountQueryRequest.PassportNo = txtPassportNo.Text
            End If
        End If

        Dim xmlDocument As XmlDocument = Nothing
        xmlDocument = udteHSValidatedAccountQueryRequest.GenXMLResult()

        If blnUATMode Then
            CallWSandShowResult(xmlDocument.InnerXml)
        Else
            Me.mveTest.ActiveViewIndex = 1
            Me.txtResult.Text = xmlDocument.InnerXml
            Me.txtResponse.Text = String.Empty
        End If
    End Sub

    Public Sub WSCall_SPPracticeValidation()

        Dim udtSPPracticeValidationRequest As SPPracticeValidationRequest = New SPPracticeValidationRequest()

        If chkSPInfo.Checked Then
            udtSPPracticeValidationRequest.SPInfo_inXML = True

            'SP ID
            If chkSPID.Checked Then
                udtSPPracticeValidationRequest.SPID_Included = True
                udtSPPracticeValidationRequest.SPID = txtSPID.Text
            End If
            'SP Name
            If chkSPSurname.Checked Then
                udtSPPracticeValidationRequest.SPSurname_Included = True
                udtSPPracticeValidationRequest.SPSurname = txtSPSurname.Text
            End If
            If chkSPGivenname.Checked Then
                udtSPPracticeValidationRequest.SPGivenName_Included = True
                udtSPPracticeValidationRequest.SPGivenName = txtSPGivenName.Text
            End If
            'Practice ID
            If chkPracticeID.Checked Then
                udtSPPracticeValidationRequest.PracticeID_included = True
                udtSPPracticeValidationRequest.PracticeID = txtPracticeID.Text
            End If
            'Practice Name
            If chkPracticeName.Checked Then
                udtSPPracticeValidationRequest.PracticeName_included = True
                udtSPPracticeValidationRequest.PracticeName = txtPracticeName.Text
            End If
        End If

        Dim xmlDocument As XmlDocument = Nothing
        xmlDocument = udtSPPracticeValidationRequest.GenXMLResult()

        If blnUATMode Then
            CallWSandShowResult(xmlDocument.InnerXml)
        Else
            Me.mveTest.ActiveViewIndex = 1
            Me.txtResult.Text = xmlDocument.InnerXml
            Me.txtResponse.Text = String.Empty
        End If

    End Sub

#End Region

#Region "View 2"

    Protected Sub BtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Me.mveTest.ActiveViewIndex = 0
        Me.txtResult.Text = String.Empty

        If Not IsNothing(Session("Selected_WS")) Then
            Select Case Session("Selected_WS")
                Case "0"
                    rboUploadClaim.Checked = True
                Case "1"
                    rboRCHNameQuery.Checked = True
                Case "2"
                    rboGetReasonForVisit.Checked = True
                Case "3"
                    rboEHSValidatedAccountQuery.Checked = True
                Case "4"
                    rboEHSAccountSubsidyQuery.Checked = True
                Case "5"
                    rboSPPracticeValidation.Checked = True
                Case "6"
                    rboUploadClaimHL7.Checked = True
            End Select
        Else
            rboUploadClaimHL7.Checked = True
        End If

    End Sub

    Protected Sub Btnrequest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnrequest.Click

        Dim ws As Service1 = New Service1
        ws.Url = System.Configuration.ConfigurationManager.AppSettings("WebServicesURL")
        Dim strSystemName As String = AppConfigMgr.getSystemName()

        Dim sResult As String = String.Empty
        Dim xml As New XmlDocument()

        Dim callback As New System.Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

        If Not IsNothing(Session("Selected_WS")) Then
            Select Case Session("Selected_WS")
                Case "0"
                    xml.LoadXml(txtResult.Text)
                    sResult = ws.UploadClaim(xml.InnerXml.ToString(), strSystemName)
                Case "1"
                    xml.LoadXml(txtResult.Text)
                    sResult = ws.RCHNameQuery(xml.InnerXml.ToString(), strSystemName)
                Case "2"
                    xml.LoadXml(txtResult.Text)
                    sResult = ws.GetReasonForVisitList(xml.InnerXml.ToString(), strSystemName)
                Case "3"
                    xml.LoadXml(txtResult.Text)
                    sResult = ws.eHSValidatedAccountQuery(xml.InnerXml.ToString(), strSystemName)
                Case "4"
                    xml.LoadXml(txtResult.Text)
                    sResult = ws.eHSAccountVoucherQuery(xml.InnerXml.ToString(), strSystemName)
                Case "5"
                    xml.LoadXml(txtResult.Text)
                    sResult = ws.SPPracticeValidation(xml.InnerXml.ToString(), strSystemName)
                Case "6"
                    'xml.LoadXml(txtResult.Text)
                    'sResult = ws.UploadClaim_HL7(xml.InnerXml.ToString(), strSystemName)
                    xml.LoadXml(txtResult.Text)
                    sResult = ws.UploadClaim(xml.InnerXml.ToString(), strSystemName)
            End Select
        Else
            sResult = ws.RCHNameQuery(xml.InnerXml.ToString(), strSystemName)
        End If

        txtResponse.Text = sResult

    End Sub

    Protected Sub BtnEncryptAndSign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEncryptAndSign.Click
        Dim strSystemName As String = AppConfigMgr.getSystemName()
        Dim strSecuredXML As String = CryptographyMgr.CreateSecuredXMLFromPlainXML(txtResult.Text, strSystemName)

        Me.txtResult.Text = strSecuredXML
        BtnEncryptAndSign.Enabled = False
    End Sub

    Protected Sub BtnDeryptAndVerify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDeryptAndVerify.Click

        Dim blnSuccess As Boolean = True
        Dim strPlainXML As String = String.Empty
        Dim strSystemName As String = AppConfigMgr.getSystemName()
        blnSuccess = CryptographyMgr.ExtractContentFromSecuredXML(Me.txtResponse.Text, strPlainXML, strSystemName)

        If blnSuccess Then
            Me.txtResponse.Text = strPlainXML
        Else
            Me.txtResponse.Text = "Failed to decrypt / verify the signature"
        End If

    End Sub

#End Region

#Region "View 3"

    Public Sub CallWSandShowResult(ByVal strXML As String)
        Me.mveTest.ActiveViewIndex = 2
        '-----------------------------------------------------------------
        '   Encrypt and sign
        '-----------------------------------------------------------------
        Dim strSystemName As String = AppConfigMgr.getSystemName()
        Dim strSecuredXML As String = CryptographyMgr.CreateSecuredXMLFromPlainXML(strXML, strSystemName)

        '-----------------------------------------------------------------
        '   WS Request
        '-----------------------------------------------------------------
        Dim ws As Service1 = New Service1
        ws.Url = System.Configuration.ConfigurationManager.AppSettings("WebServicesURL")

        Dim sResult As String = String.Empty

        Dim callback As New System.Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

        If Not IsNothing(Session("Selected_WS")) Then
            Select Case Session("Selected_WS")
                Case "0"
                    sResult = ws.UploadClaim(strSecuredXML, strSystemName)
                Case "1"
                    sResult = ws.RCHNameQuery(strSecuredXML, strSystemName)
                Case "2"
                    sResult = ws.GetReasonForVisitList(strSecuredXML, strSystemName)
                Case "3"
                    sResult = ws.eHSValidatedAccountQuery(strSecuredXML, strSystemName)
                Case "4"
                    sResult = ws.eHSAccountVoucherQuery(strSecuredXML, strSystemName)
                Case "5"
                    sResult = ws.SPPracticeValidation(strSecuredXML, strSystemName)
                Case "6"
                    sResult = ws.UploadClaim(strSecuredXML, strSystemName)
            End Select
        End If

        '-----------------------------------------------------------------
        '   Decrypt and verify 
        '-----------------------------------------------------------------
        Dim blnSuccess As Boolean = True
        Dim strPlainXML As String = String.Empty

        blnSuccess = CryptographyMgr.ExtractContentFromSecuredXML(sResult, strPlainXML, strSystemName)

        If blnSuccess Then
            Me.txtUATResponse.Text = strPlainXML
            '-----------------------------------------------------------------
            '   Extract Result
            '-----------------------------------------------------------------
            txtExtractedResult.Text = String.Empty

            If Not IsNothing(Session("Selected_WS")) Then
                Select Case Session("Selected_WS")
                    Case "0"
                        ExtractResultForUploadClaim(strPlainXML)
                    Case "1"
                        'RCH Name Query
                        ExtractResultForRCHNameQuery(strPlainXML)
                    Case "2"
                        'Get Reason For Visit List
                        ExtractResultForGetReasonForVisit(strPlainXML)
                    Case "3"
                        'Validated Account Query
                        ExtractResultForValidatedAccountQuery(strPlainXML)
                    Case "4"
                        'Voucher Query
                        ExtractResultForVoucherQuery(strPlainXML)
                    Case "5"
                        'SP Practice Validation
                        ExtractResultForSPPracticeValidation(strPlainXML)
                    Case "6"
                        ExtractResultForUploadClaim(strPlainXML)
                End Select
            End If
            '-----------------------------------------------------------------
        Else
            Me.txtUATResponse.Text = "Failed to decrypt / verify the signature"
        End If


    End Sub

    Private Sub btnUATBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUATBack.Click
        Me.mveTest.ActiveViewIndex = 0
        Me.txtUATResponse.Text = String.Empty

        If Not IsNothing(Session("Selected_WS")) Then
            Select Case Session("Selected_WS")
                Case "0"
                    rboUploadClaim.Checked = True
                Case "1"
                    rboRCHNameQuery.Checked = True
                Case "2"
                    rboGetReasonForVisit.Checked = True
                Case "3"
                    rboEHSValidatedAccountQuery.Checked = True
                Case "4"
                    rboEHSAccountSubsidyQuery.Checked = True
                Case "5"
                    rboSPPracticeValidation.Checked = True
                Case "6"
                    rboUploadClaimHL7.Checked = True
            End Select
        Else
            rboUploadClaimHL7.Checked = True
        End If
    End Sub

    Private Sub btnUATBackReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUATBackReset.Click
        Me.mveTest.ActiveViewIndex = 0
        Me.txtUATResponse.Text = String.Empty

        rboUploadClaimHL7.Checked = True
        rboRCHNameQuery.Checked = False
        rboGetReasonForVisit.Checked = False
        rboEHSValidatedAccountQuery.Checked = False
        rboEHSAccountSubsidyQuery.Checked = False
        rboSPPracticeValidation.Checked = False
        rboUploadClaim.Checked = False

        chkSPInfo.Checked = True
        chkAccountInfo.Checked = True
        chkClaimInfo.Checked = True
        chkSPInfo.BackColor = Drawing.Color.Aquamarine
        chkAccountInfo.BackColor = Drawing.Color.Aquamarine
        chkClaimInfo.BackColor = Drawing.Color.Aquamarine

        rboUploadClaimHL7.BackColor = Drawing.Color.Aquamarine
        rboUploadClaim.BackColor = Drawing.Color.Transparent
        rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

        ResetAll()
    End Sub
#End Region

#Region "WS Radio Buttons"

    Private Sub rboUploadClaim_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboUploadClaim.CheckedChanged
        If rboUploadClaim.Checked Then
            rboUploadClaim.BackColor = Drawing.Color.Aquamarine
            rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
            rboRCHNameQuery.BackColor = Drawing.Color.Transparent
            rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
            rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

            chkSPInfo.Checked = True
            chkAccountInfo.Checked = True
            chkClaimInfo.Checked = True

            chkSPInfo.BackColor = Drawing.Color.Aquamarine
            chkAccountInfo.BackColor = Drawing.Color.Aquamarine
            chkClaimInfo.BackColor = Drawing.Color.Aquamarine

            'chkRCHCode.BackColor = Drawing.Color.Azure
        Else
            rboUploadClaim.BackColor = Drawing.Color.Transparent
        End If
    End Sub

    Private Sub rboUploadClaimHL7_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboUploadClaimHL7.CheckedChanged
        If rboUploadClaimHL7.Checked Then
            rboUploadClaimHL7.BackColor = Drawing.Color.Aquamarine
            rboUploadClaim.BackColor = Drawing.Color.Transparent
            rboRCHNameQuery.BackColor = Drawing.Color.Transparent
            rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
            rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

            chkSPInfo.Checked = True
            chkAccountInfo.Checked = True
            chkClaimInfo.Checked = True

            chkSPInfo.BackColor = Drawing.Color.Aquamarine
            chkAccountInfo.BackColor = Drawing.Color.Aquamarine
            chkClaimInfo.BackColor = Drawing.Color.Aquamarine

            'chkRCHCode.BackColor = Drawing.Color.Azure
            '------------
            chkDOReg.Text = "Date of Registration(dd-MM-yyyy)"
            chkDOB.Text = "Date of Birth(dd-MM-yyyy)"
            chkDOI.Text = "Date of Issue(dd-MM-yyyy)"
            '------------
            chkSPID.Font.Underline = True
            chkSPSurname.Font.Underline = False
            chkSPGivenname.Font.Underline = False
            chkPracticeID.Font.Underline = True
            chkPracticeName.Font.Underline = True
            '------------
            pnlSPInfo.Visible = True
            pnlAccountInfo.Visible = True
            pnlClaimInfo.Visible = True
            '------------
            chkRCHCode.Visible = False
            txtRCHCode.Visible = False

            txtTestCaseNo.Text = ""
            txtTestCaseDesc.Text = ""

            lblTip1.Visible = True
            lblTip2.Visible = True
            '-----------
            btnAdd2Vaccine.Visible = True
            btnRemove2Vaccine.Visible = False
            btnAdd2Voucher.Visible = True
            btnRemove2Voucher.Visible = False

            pnl2Vaccine.Visible = False
            chkSubsidyCode21.Checked = False
            chkSubsidyCode22.Checked = False

            pnl2Voucher.Visible = False
            chkHCVS2.Checked = False
        Else
            rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
        End If
    End Sub

    Private Sub rboRCHNameQuery_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboRCHNameQuery.CheckedChanged
        If rboRCHNameQuery.Checked Then
            rboUploadClaim.BackColor = Drawing.Color.Transparent
            rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
            rboRCHNameQuery.BackColor = Drawing.Color.Aquamarine
            rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
            rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

            chkSPInfo.Checked = True
            chkAccountInfo.Checked = False
            chkClaimInfo.Checked = False

            chkSPInfo.BackColor = Drawing.Color.Aquamarine
            chkAccountInfo.BackColor = Drawing.Color.Transparent
            chkClaimInfo.BackColor = Drawing.Color.Transparent

            'chkRCHCode.BackColor = Drawing.Color.Aquamarine
            '------------
            chkDOReg.Text = "Date of Registration(dd-MM-yyyy)"
            chkDOB.Text = "Date of Birth(dd-MM-yyyy)"
            chkDOI.Text = "Date of Issue(dd-MM-yyyy)"
            '-------------
            chkSPID.Font.Underline = True
            chkSPSurname.Font.Underline = False
            chkSPGivenname.Font.Underline = False
            chkPracticeID.Font.Underline = True
            chkPracticeName.Font.Underline = True
            '-------------
            pnlSPInfo.Visible = True
            pnlAccountInfo.Visible = False
            pnlClaimInfo.Visible = False
            '-------------
            chkRCHCode.Visible = True
            txtRCHCode.Visible = True

            txtTestCaseNo.Text = ""
            txtTestCaseDesc.Text = ""

            lblTip1.Visible = False
            lblTip2.Visible = False
        Else
            rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        End If
    End Sub

    Private Sub rboGetReasonForVisit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboGetReasonForVisit.CheckedChanged
        If rboGetReasonForVisit.Checked Then
            rboUploadClaim.BackColor = Drawing.Color.Transparent
            rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
            rboRCHNameQuery.BackColor = Drawing.Color.Transparent
            rboGetReasonForVisit.BackColor = Drawing.Color.Aquamarine
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
            rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

            chkSPInfo.Checked = False
            chkAccountInfo.Checked = False
            chkClaimInfo.Checked = False

            chkSPInfo.BackColor = Drawing.Color.Transparent
            chkAccountInfo.BackColor = Drawing.Color.Transparent
            chkClaimInfo.BackColor = Drawing.Color.Transparent

            'chkRCHCode.BackColor = Drawing.Color.Azure
            '------------
            chkDOReg.Text = "Date of Registration(dd-MM-yyyy)"
            chkDOB.Text = "Date of Birth(dd-MM-yyyy)"
            chkDOI.Text = "Date of Issue(dd-MM-yyyy)"
            '-------------
            chkSPID.Font.Underline = False
            chkSPSurname.Font.Underline = False
            chkSPGivenname.Font.Underline = False
            chkPracticeID.Font.Underline = False
            chkPracticeName.Font.Underline = False
            '-------------
            pnlSPInfo.Visible = False
            pnlAccountInfo.Visible = False
            pnlClaimInfo.Visible = False
            '-------------
            chkRCHCode.Visible = False
            txtRCHCode.Visible = False

            txtTestCaseNo.Text = ""
            txtTestCaseDesc.Text = ""

            lblTip1.Visible = False
            lblTip2.Visible = False
        Else
            rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        End If
    End Sub

    Private Sub rboEHSValidatedAccountQuery_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboEHSValidatedAccountQuery.CheckedChanged
        If rboEHSValidatedAccountQuery.Checked Then
            rboUploadClaim.BackColor = Drawing.Color.Transparent
            rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
            rboRCHNameQuery.BackColor = Drawing.Color.Transparent
            rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Aquamarine
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
            rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

            chkSPInfo.Checked = True
            chkAccountInfo.Checked = True
            chkClaimInfo.Checked = False

            chkSPInfo.BackColor = Drawing.Color.Aquamarine
            chkAccountInfo.BackColor = Drawing.Color.Aquamarine
            chkClaimInfo.BackColor = Drawing.Color.Transparent

            'chkRCHCode.BackColor = Drawing.Color.Azure
            '------------
            chkDOReg.Text = "Date of Registration(dd-MM-yyyy)"
            chkDOB.Text = "Date of Birth(dd-MM-yyyy)"
            chkDOI.Text = "Date of Issue(dd-MM-yyyy)"
            '-------------
            chkSPID.Font.Underline = True
            chkSPSurname.Font.Underline = False
            chkSPGivenname.Font.Underline = False
            chkPracticeID.Font.Underline = True
            chkPracticeName.Font.Underline = True
            '------------
            pnlSPInfo.Visible = True
            pnlAccountInfo.Visible = True
            pnlClaimInfo.Visible = False
            '-------------
            chkRCHCode.Visible = False
            txtRCHCode.Visible = False

            txtTestCaseNo.Text = ""
            txtTestCaseDesc.Text = ""

            lblTip1.Visible = True
            lblTip2.Visible = False
        Else
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        End If
    End Sub

    Private Sub rboEHSAccountSubsidyQuery_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboEHSAccountSubsidyQuery.CheckedChanged
        If rboEHSAccountSubsidyQuery.Checked Then
            rboUploadClaim.BackColor = Drawing.Color.Transparent
            rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
            rboRCHNameQuery.BackColor = Drawing.Color.Transparent
            rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Aquamarine
            rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

            chkSPInfo.Checked = True
            chkAccountInfo.Checked = True
            chkClaimInfo.Checked = False

            chkSPInfo.BackColor = Drawing.Color.Aquamarine
            chkAccountInfo.BackColor = Drawing.Color.Aquamarine
            chkClaimInfo.BackColor = Drawing.Color.Transparent

            'chkRCHCode.BackColor = Drawing.Color.Azure
            '------------
            chkDOReg.Text = "Date of Registration(dd-MM-yyyy)"
            chkDOB.Text = "Date of Birth(dd-MM-yyyy)"
            chkDOI.Text = "Date of Issue(dd-MM-yyyy)"
            '-------------
            chkSPID.Font.Underline = True
            chkSPSurname.Font.Underline = False
            chkSPGivenname.Font.Underline = False
            chkPracticeID.Font.Underline = True
            chkPracticeName.Font.Underline = True
            '------------
            pnlSPInfo.Visible = True
            pnlAccountInfo.Visible = True
            pnlClaimInfo.Visible = False
            '-------------
            chkRCHCode.Visible = False
            txtRCHCode.Visible = False

            txtTestCaseNo.Text = ""
            txtTestCaseDesc.Text = ""

            lblTip1.Visible = True
            lblTip2.Visible = False
        Else
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        End If
    End Sub

    Private Sub rboSPPracticeValidation_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboSPPracticeValidation.CheckedChanged
        If rboSPPracticeValidation.Checked Then
            rboUploadClaim.BackColor = Drawing.Color.Transparent
            rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
            rboRCHNameQuery.BackColor = Drawing.Color.Transparent
            rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
            rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
            rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
            rboSPPracticeValidation.BackColor = Drawing.Color.Aquamarine

            chkSPInfo.Checked = True
            chkAccountInfo.Checked = False
            chkClaimInfo.Checked = False

            chkSPInfo.BackColor = Drawing.Color.Aquamarine
            chkAccountInfo.BackColor = Drawing.Color.Transparent
            chkClaimInfo.BackColor = Drawing.Color.Transparent

            'chkRCHCode.BackColor = Drawing.Color.Azure
            '------------
            chkDOReg.Text = "Date of Registration(dd-MM-yyyy)"
            chkDOB.Text = "Date of Birth(dd-MM-yyyy)"
            chkDOI.Text = "Date of Issue(dd-MM-yyyy)"
            '-------------
            chkSPID.Font.Underline = True
            chkSPSurname.Font.Underline = True
            chkSPGivenname.Font.Underline = True
            chkPracticeID.Font.Underline = True
            chkPracticeName.Font.Underline = True
            '------------
            pnlSPInfo.Visible = True
            pnlAccountInfo.Visible = False
            pnlClaimInfo.Visible = False
            '-------------
            chkRCHCode.Visible = False
            txtRCHCode.Visible = False

            txtTestCaseNo.Text = ""
            txtTestCaseDesc.Text = ""

            lblTip1.Visible = False
            lblTip2.Visible = False
        Else
            rboSPPracticeValidation.BackColor = Drawing.Color.Transparent
        End If
    End Sub

#End Region

#Region "Doc Type Radio Button"
    Private Sub rboHKIC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboHKIC.CheckedChanged
        If rboHKIC.Checked Then
            chkEntryNo.Font.Underline = False
            chkDocumentNo.Font.Underline = False
            chkHKIC.Font.Underline = True
            chkRegNo.Font.Underline = False
            chkBirthEntryNo.Font.Underline = False
            chkPermitNo.Font.Underline = False
            chkVISANo.Font.Underline = False
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = False
            chkAgeOn.Font.Underline = False
            chkDOReg.Font.Underline = False
            chkDOBInWord.Font.Underline = False
            chkNameChi.Font.Underline = False
            chkDOI.Font.Underline = True
            chkSerialNo.Font.Underline = False
            chkReference.Font.Underline = False
            chkFreeReference.Font.Underline = False
            chkRemainUntil.Font.Underline = False
            chkPassportNo.Font.Underline = False
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

    Private Sub rboEC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboEC.CheckedChanged
        If rboEC.Checked Then
            chkEntryNo.Font.Underline = False
            chkDocumentNo.Font.Underline = False
            chkHKIC.Font.Underline = True
            chkRegNo.Font.Underline = False
            chkBirthEntryNo.Font.Underline = False
            chkPermitNo.Font.Underline = False
            chkVISANo.Font.Underline = False
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = True
            chkAgeOn.Font.Underline = True
            chkDOReg.Font.Underline = True
            chkDOBInWord.Font.Underline = False
            chkNameChi.Font.Underline = True
            chkDOI.Font.Underline = True
            chkSerialNo.Font.Underline = True
            chkReference.Font.Underline = True
            chkFreeReference.Font.Underline = True
            chkRemainUntil.Font.Underline = False
            chkPassportNo.Font.Underline = False
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

    Private Sub rboHKBC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboHKBC.CheckedChanged
        If rboHKBC.Checked Then
            chkEntryNo.Font.Underline = False
            chkDocumentNo.Font.Underline = False
            chkHKIC.Font.Underline = False
            chkRegNo.Font.Underline = True
            chkBirthEntryNo.Font.Underline = False
            chkPermitNo.Font.Underline = False
            chkVISANo.Font.Underline = False
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = False
            chkAgeOn.Font.Underline = False
            chkDOReg.Font.Underline = False
            chkDOBInWord.Font.Underline = True
            chkNameChi.Font.Underline = False
            chkDOI.Font.Underline = False
            chkSerialNo.Font.Underline = False
            chkReference.Font.Underline = False
            chkFreeReference.Font.Underline = False
            chkRemainUntil.Font.Underline = False
            chkPassportNo.Font.Underline = False
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

    Private Sub rboADOPC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboADOPC.CheckedChanged
        If rboADOPC.Checked Then
            chkEntryNo.Font.Underline = True
            chkDocumentNo.Font.Underline = False
            chkHKIC.Font.Underline = False
            chkRegNo.Font.Underline = False
            chkBirthEntryNo.Font.Underline = False
            chkPermitNo.Font.Underline = False
            chkVISANo.Font.Underline = False
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = False
            chkAgeOn.Font.Underline = False
            chkDOReg.Font.Underline = False
            chkDOBInWord.Font.Underline = True
            chkNameChi.Font.Underline = False
            chkDOI.Font.Underline = False
            chkSerialNo.Font.Underline = False
            chkReference.Font.Underline = False
            chkFreeReference.Font.Underline = False
            chkRemainUntil.Font.Underline = False
            chkPassportNo.Font.Underline = False
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

    Private Sub rboVISA_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboVISA.CheckedChanged
        If rboVISA.Checked Then
            chkEntryNo.Font.Underline = False
            chkDocumentNo.Font.Underline = False
            chkHKIC.Font.Underline = False
            chkRegNo.Font.Underline = False
            chkBirthEntryNo.Font.Underline = False
            chkPermitNo.Font.Underline = False
            chkVISANo.Font.Underline = True
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = False
            chkAgeOn.Font.Underline = False
            chkDOReg.Font.Underline = False
            chkDOBInWord.Font.Underline = False
            chkNameChi.Font.Underline = False
            chkDOI.Font.Underline = False
            chkSerialNo.Font.Underline = False
            chkReference.Font.Underline = False
            chkFreeReference.Font.Underline = False
            chkRemainUntil.Font.Underline = False
            chkPassportNo.Font.Underline = True
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

    Private Sub rboID235B_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboID235B.CheckedChanged
        If rboID235B.Checked Then
            chkEntryNo.Font.Underline = False
            chkDocumentNo.Font.Underline = False
            chkHKIC.Font.Underline = False
            chkRegNo.Font.Underline = False
            chkBirthEntryNo.Font.Underline = True
            chkPermitNo.Font.Underline = False
            chkVISANo.Font.Underline = False
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = False
            chkAgeOn.Font.Underline = False
            chkDOReg.Font.Underline = False
            chkDOBInWord.Font.Underline = False
            chkNameChi.Font.Underline = False
            chkDOI.Font.Underline = False
            chkSerialNo.Font.Underline = False
            chkReference.Font.Underline = False
            chkFreeReference.Font.Underline = False
            chkRemainUntil.Font.Underline = True
            chkPassportNo.Font.Underline = False
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

    Private Sub rboDOCI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboDOCI.CheckedChanged
        If rboDOCI.Checked Then
            chkEntryNo.Font.Underline = False
            chkDocumentNo.Font.Underline = True
            chkHKIC.Font.Underline = False
            chkRegNo.Font.Underline = False
            chkBirthEntryNo.Font.Underline = False
            chkPermitNo.Font.Underline = False
            chkVISANo.Font.Underline = False
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = False
            chkAgeOn.Font.Underline = False
            chkDOReg.Font.Underline = False
            chkDOBInWord.Font.Underline = False
            chkNameChi.Font.Underline = False
            chkDOI.Font.Underline = True
            chkSerialNo.Font.Underline = False
            chkReference.Font.Underline = False
            chkFreeReference.Font.Underline = False
            chkRemainUntil.Font.Underline = False
            chkPassportNo.Font.Underline = False
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

    Private Sub rboREPMT_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboREPMT.CheckedChanged
        If rboREPMT.Checked Then
            chkEntryNo.Font.Underline = False
            chkDocumentNo.Font.Underline = False
            chkHKIC.Font.Underline = False
            chkRegNo.Font.Underline = False
            chkBirthEntryNo.Font.Underline = False
            chkPermitNo.Font.Underline = True
            chkVISANo.Font.Underline = False
            chkSurname.Font.Underline = True
            chkGivenName.Font.Underline = True
            chkGender.Font.Underline = True
            chkDOB.Font.Underline = True
            chkDOBType.Font.Underline = False
            chkAgeOn.Font.Underline = False
            chkDOReg.Font.Underline = False
            chkDOBInWord.Font.Underline = False
            chkNameChi.Font.Underline = False
            chkDOI.Font.Underline = True
            chkSerialNo.Font.Underline = False
            chkReference.Font.Underline = False
            chkFreeReference.Font.Underline = False
            chkRemainUntil.Font.Underline = False
            chkPassportNo.Font.Underline = False
            'chkRCHCode.Font.Underline = False
        End If
    End Sub

#End Region

#Region "UI Controls"

#Region "SP related"

    Private Sub chkSPID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSPID.CheckedChanged
        If chkSPID.Checked Then
            Me.txtSPID.Text = ""
            Me.txtSPID.Enabled = True
            Me.txtSPID.BackColor = Drawing.Color.White
        Else
            Me.txtSPID.Text = ""
            Me.txtSPID.Enabled = False
            Me.txtSPID.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkSPSurname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSPSurname.CheckedChanged
        If chkSPSurname.Checked Then
            Me.txtSPSurname.Text = ""
            Me.txtSPSurname.Enabled = True
            Me.txtSPSurname.BackColor = Drawing.Color.White
        Else
            Me.txtSPSurname.Text = ""
            Me.txtSPSurname.Enabled = False
            Me.txtSPSurname.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkSPGivenname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSPGivenname.CheckedChanged
        If chkSPGivenname.Checked Then
            Me.txtSPGivenName.Text = ""
            Me.txtSPGivenName.Enabled = True
            Me.txtSPGivenName.BackColor = Drawing.Color.White
        Else
            Me.txtSPGivenName.Text = ""
            Me.txtSPGivenName.Enabled = False
            Me.txtSPGivenName.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkPracticeID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPracticeID.CheckedChanged
        If chkPracticeID.Checked Then
            Me.txtPracticeID.Text = ""
            Me.txtPracticeID.Enabled = True
            Me.txtPracticeID.BackColor = Drawing.Color.White
        Else
            Me.txtPracticeID.Text = ""
            Me.txtPracticeID.Enabled = False
            Me.txtPracticeID.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkPracticeName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPracticeName.CheckedChanged
        If chkPracticeName.Checked Then
            Me.txtPracticeName.Text = ""
            Me.txtPracticeName.Enabled = True
            Me.txtPracticeName.BackColor = Drawing.Color.White
        Else
            Me.txtPracticeName.Text = ""
            Me.txtPracticeName.Enabled = False
            Me.txtPracticeName.BackColor = Drawing.Color.Silver
        End If
    End Sub

#End Region

#Region "Account Related"

    Private Sub chkEntryNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEntryNo.CheckedChanged
        If chkEntryNo.Checked Then
            Me.txtEntryNo.Text = ""
            Me.txtEntryNo.Enabled = True
            Me.txtEntryNo.BackColor = Drawing.Color.White
        Else
            Me.txtEntryNo.Text = ""
            Me.txtEntryNo.Enabled = False
            Me.txtEntryNo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDocumentNo.CheckedChanged
        If chkDocumentNo.Checked Then
            Me.txtDocumentNo.Text = ""
            Me.txtDocumentNo.Enabled = True
            Me.txtDocumentNo.BackColor = Drawing.Color.White
        Else
            Me.txtDocumentNo.Text = ""
            Me.txtDocumentNo.Enabled = False
            Me.txtDocumentNo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkHKIC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHKIC.CheckedChanged
        If chkHKIC.Checked Then
            Me.txtHKIC.Text = ""
            Me.txtHKIC.Enabled = True
            Me.txtHKIC.BackColor = Drawing.Color.White
        Else
            Me.txtHKIC.Text = ""
            Me.txtHKIC.Enabled = False
            Me.txtHKIC.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkRegNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRegNo.CheckedChanged
        If chkRegNo.Checked Then
            Me.txtRegNo.Text = ""
            Me.txtRegNo.Enabled = True
            Me.txtRegNo.BackColor = Drawing.Color.White
        Else
            Me.txtRegNo.Text = ""
            Me.txtRegNo.Enabled = False
            Me.txtRegNo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkBirthEntryNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBirthEntryNo.CheckedChanged
        If chkBirthEntryNo.Checked Then
            Me.txtBirthEntryNo.Text = ""
            Me.txtBirthEntryNo.Enabled = True
            Me.txtBirthEntryNo.BackColor = Drawing.Color.White
        Else
            Me.txtBirthEntryNo.Text = ""
            Me.txtBirthEntryNo.Enabled = False
            Me.txtBirthEntryNo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkPermitNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPermitNo.CheckedChanged
        If chkPermitNo.Checked Then
            Me.txtPermitNo.Text = ""
            Me.txtPermitNo.Enabled = True
            Me.txtPermitNo.BackColor = Drawing.Color.White
        Else
            Me.txtPermitNo.Text = ""
            Me.txtPermitNo.Enabled = False
            Me.txtPermitNo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkVISANo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVISANo.CheckedChanged
        If chkVISANo.Checked Then
            Me.txtVISANo.Text = ""
            Me.txtVISANo.Enabled = True
            Me.txtVISANo.BackColor = Drawing.Color.White
        Else
            Me.txtVISANo.Text = ""
            Me.txtVISANo.Enabled = False
            Me.txtVISANo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkSurname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSurname.CheckedChanged
        If chkSurname.Checked Then
            Me.txtSurname.Text = ""
            Me.txtSurname.Enabled = True
            Me.txtSurname.BackColor = Drawing.Color.White
        Else
            Me.txtSurname.Text = ""
            Me.txtSurname.Enabled = False
            Me.txtSurname.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkGivenName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGivenName.CheckedChanged
        If chkGivenName.Checked Then
            Me.txtGivenName.Text = ""
            Me.txtGivenName.Enabled = True
            Me.txtGivenName.BackColor = Drawing.Color.White
        Else
            Me.txtGivenName.Text = ""
            Me.txtGivenName.Enabled = False
            Me.txtGivenName.BackColor = Drawing.Color.Silver
        End If
    End Sub



    Private Sub chkGender_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGender.CheckedChanged
        If chkGender.Checked Then
            Me.txtGender.Text = ""
            Me.txtGender.Enabled = True
            Me.txtGender.BackColor = Drawing.Color.White
        Else
            Me.txtGender.Text = ""
            Me.txtGender.Enabled = False
            Me.txtGender.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkDOBType_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOBType.CheckedChanged
        If chkDOBType.Checked Then
            Me.txtDOBType.Text = ""
            Me.txtDOBType.Enabled = True
            Me.txtDOBType.BackColor = Drawing.Color.White
        Else
            Me.txtDOBType.Text = ""
            Me.txtDOBType.Enabled = False
            Me.txtDOBType.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkAgeOn_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAgeOn.CheckedChanged
        If chkAgeOn.Checked Then
            Me.txtAgeOn.Text = ""
            Me.txtAgeOn.Enabled = True
            Me.txtAgeOn.BackColor = Drawing.Color.White
        Else
            Me.txtAgeOn.Text = ""
            Me.txtAgeOn.Enabled = False
            Me.txtAgeOn.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkDOReg_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOReg.CheckedChanged
        If chkDOReg.Checked Then
            Me.txtDOReg.Text = ""
            Me.txtDOReg.Enabled = True
            Me.txtDOReg.BackColor = Drawing.Color.White
        Else
            Me.txtDOReg.Text = ""
            Me.txtDOReg.Enabled = False
            Me.txtDOReg.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkDOBInWord_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOBInWord.CheckedChanged
        If chkDOBInWord.Checked Then
            Me.txtDOBInWord.Text = ""
            Me.txtDOBInWord.Enabled = True
            Me.txtDOBInWord.BackColor = Drawing.Color.White
        Else
            Me.txtDOBInWord.Text = ""
            Me.txtDOBInWord.Enabled = False
            Me.txtDOBInWord.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkNameChi_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNameChi.CheckedChanged
        If chkNameChi.Checked Then
            Me.txtNameChi.Text = ""
            Me.txtNameChi.Enabled = True
            Me.txtNameChi.BackColor = Drawing.Color.White
        Else
            Me.txtNameChi.Text = ""
            Me.txtNameChi.Enabled = False
            Me.txtNameChi.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkDOI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOI.CheckedChanged
        If chkDOI.Checked Then
            Me.txtDOI.Text = ""
            Me.txtDOI.Enabled = True
            Me.txtDOI.BackColor = Drawing.Color.White
        Else
            Me.txtDOI.Text = ""
            Me.txtDOI.Enabled = False
            Me.txtDOI.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkSerialNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSerialNo.CheckedChanged
        If chkSerialNo.Checked Then
            Me.txtSerialNo.Text = ""
            Me.txtSerialNo.Enabled = True
            Me.txtSerialNo.BackColor = Drawing.Color.White
        Else
            Me.txtSerialNo.Text = ""
            Me.txtSerialNo.Enabled = False
            Me.txtSerialNo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkReference_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkReference.CheckedChanged
        If chkReference.Checked Then
            Me.txtReference.Text = ""
            Me.txtReference.Enabled = True
            Me.txtReference.BackColor = Drawing.Color.White
        Else
            Me.txtReference.Text = ""
            Me.txtReference.Enabled = False
            Me.txtReference.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkFreeReference_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFreeReference.CheckedChanged
        If chkFreeReference.Checked Then
            Me.txtFreeReference.Text = ""
            Me.txtFreeReference.Enabled = True
            Me.txtFreeReference.BackColor = Drawing.Color.White
        Else
            Me.txtFreeReference.Text = ""
            Me.txtFreeReference.Enabled = False
            Me.txtFreeReference.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkRemainUntil_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRemainUntil.CheckedChanged
        If chkRemainUntil.Checked Then
            Me.txtRemainUntil.Text = ""
            Me.txtRemainUntil.Enabled = True
            Me.txtRemainUntil.BackColor = Drawing.Color.White
        Else
            Me.txtRemainUntil.Text = ""
            Me.txtRemainUntil.Enabled = False
            Me.txtRemainUntil.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkPassportNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPassportNo.CheckedChanged
        If chkPassportNo.Checked Then
            Me.txtPassportNo.Text = ""
            Me.txtPassportNo.Enabled = True
            Me.txtPassportNo.BackColor = Drawing.Color.White
        Else
            Me.txtPassportNo.Text = ""
            Me.txtPassportNo.Enabled = False
            Me.txtPassportNo.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkRCHCode_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRCHCode.CheckedChanged
        If chkRCHCode.Checked Then
            Me.txtRCHCode.Text = ""
            Me.txtRCHCode.Enabled = True
            Me.txtRCHCode.BackColor = Drawing.Color.White
        Else
            Me.txtRCHCode.Text = ""
            Me.txtRCHCode.Enabled = False
            Me.txtRCHCode.BackColor = Drawing.Color.Silver
        End If
    End Sub

    Private Sub chkDOB_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOB.CheckedChanged
        If chkDOB.Checked Then
            Me.txtDOB.Text = ""
            Me.txtDOB.Enabled = True
            Me.txtDOB.BackColor = Drawing.Color.White
        Else
            Me.txtDOB.Text = ""
            Me.txtDOB.Enabled = False
            Me.txtDOB.BackColor = Drawing.Color.Silver
        End If
    End Sub
#End Region


    Protected Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click, btnResetTop.Click
        rboUploadClaimHL7.Checked = True
        rboRCHNameQuery.Checked = False
        rboGetReasonForVisit.Checked = False
        rboEHSValidatedAccountQuery.Checked = False
        rboEHSAccountSubsidyQuery.Checked = False
        rboSPPracticeValidation.Checked = False
        rboUploadClaim.Checked = False

        chkSPInfo.Checked = True
        chkAccountInfo.Checked = True
        chkClaimInfo.Checked = True
        chkSPInfo.BackColor = Drawing.Color.Aquamarine
        chkAccountInfo.BackColor = Drawing.Color.Aquamarine
        chkClaimInfo.BackColor = Drawing.Color.Aquamarine

        rboUploadClaimHL7.BackColor = Drawing.Color.Aquamarine
        rboUploadClaim.BackColor = Drawing.Color.Transparent
        rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

        ResetAll()
        'chkSPID.Checked = False
        'Me.txtSPID.Text = ""
        'Me.txtSPID.Enabled = False
        'Me.txtSPID.BackColor = Drawing.Color.Silver

        'chkSPSurname.Checked = False
        'Me.txtSPSurname.Text = ""
        'Me.txtSPSurname.Enabled = False
        'Me.txtSPSurname.BackColor = Drawing.Color.Silver

        'chkSPGivenname.Checked = False
        'Me.txtSPGivenName.Text = ""
        'Me.txtSPGivenName.Enabled = False
        'Me.txtSPGivenName.BackColor = Drawing.Color.Silver

        'chkPracticeID.Checked = False
        'Me.txtPracticeID.Text = ""
        'Me.txtPracticeID.Enabled = False
        'Me.txtPracticeID.BackColor = Drawing.Color.Silver

        'chkPracticeName.Checked = False
        'Me.txtPracticeName.Text = ""
        'Me.txtPracticeName.Enabled = False
        'Me.txtPracticeName.BackColor = Drawing.Color.Silver

        'chkEntryNo.Checked = False
        'Me.txtEntryNo.Text = ""
        'Me.txtEntryNo.Enabled = False
        'Me.txtEntryNo.BackColor = Drawing.Color.Silver

        'chkDocumentNo.Checked = False
        'Me.txtDocumentNo.Text = ""
        'Me.txtDocumentNo.Enabled = False
        'Me.txtDocumentNo.BackColor = Drawing.Color.Silver

        'chkHKIC.Checked = False
        'Me.txtHKIC.Text = ""
        'Me.txtHKIC.Enabled = False
        'Me.txtHKIC.BackColor = Drawing.Color.Silver

        'chkRegNo.Checked = False
        'Me.txtRegNo.Text = ""
        'Me.txtRegNo.Enabled = False
        'Me.txtRegNo.BackColor = Drawing.Color.Silver

        'chkBirthEntryNo.Checked = False
        'Me.txtBirthEntryNo.Text = ""
        'Me.txtBirthEntryNo.Enabled = False
        'Me.txtBirthEntryNo.BackColor = Drawing.Color.Silver

        'chkPermitNo.Checked = False
        'Me.txtPermitNo.Text = ""
        'Me.txtPermitNo.Enabled = False
        'Me.txtPermitNo.BackColor = Drawing.Color.Silver

        'chkVISANo.Checked = False
        'Me.txtVISANo.Text = ""
        'Me.txtVISANo.Enabled = False
        'Me.txtVISANo.BackColor = Drawing.Color.Silver

        'chkSurname.Checked = False
        'Me.txtSurname.Text = ""
        'Me.txtSurname.Enabled = False
        'Me.txtSurname.BackColor = Drawing.Color.Silver

        'chkGivenName.Checked = False
        'Me.txtGivenName.Text = ""
        'Me.txtGivenName.Enabled = False
        'Me.txtGivenName.BackColor = Drawing.Color.Silver

        'chkGender.Checked = False
        'Me.txtGender.Text = ""
        'Me.txtGender.Enabled = False
        'Me.txtGender.BackColor = Drawing.Color.Silver

        'chkDOBType.Checked = False
        'Me.txtDOBType.Text = ""
        'Me.txtDOBType.Enabled = False
        'Me.txtDOBType.BackColor = Drawing.Color.Silver

        'chkAgeOn.Checked = False
        'Me.txtAgeOn.Text = ""
        'Me.txtAgeOn.Enabled = False
        'Me.txtAgeOn.BackColor = Drawing.Color.Silver

        'chkDOReg.Checked = False
        'Me.txtDOReg.Text = ""
        'Me.txtDOReg.Enabled = False
        'Me.txtDOReg.BackColor = Drawing.Color.Silver

        'chkDOBInWord.Checked = False
        'Me.txtDOBInWord.Text = ""
        'Me.txtDOBInWord.Enabled = False
        'Me.txtDOBInWord.BackColor = Drawing.Color.Silver

        'chkNameChi.Checked = False
        'Me.txtNameChi.Text = ""
        'Me.txtNameChi.Enabled = False
        'Me.txtNameChi.BackColor = Drawing.Color.Silver

        'chkDOI.Checked = False
        'Me.txtDOI.Text = ""
        'Me.txtDOI.Enabled = False
        'Me.txtDOI.BackColor = Drawing.Color.Silver

        'chkSerialNo.Checked = False
        'Me.txtSerialNo.Text = ""
        'Me.txtSerialNo.Enabled = False
        'Me.txtSerialNo.BackColor = Drawing.Color.Silver

        'chkReference.Checked = False
        'Me.txtReference.Text = ""
        'Me.txtReference.Enabled = False
        'Me.txtReference.BackColor = Drawing.Color.Silver

        'chkFreeReference.Checked = False
        'Me.txtFreeReference.Text = ""
        'Me.txtFreeReference.Enabled = False
        'Me.txtFreeReference.BackColor = Drawing.Color.Silver

        'chkRemainUntil.Checked = False
        'Me.txtRemainUntil.Text = ""
        'Me.txtRemainUntil.Enabled = False
        'Me.txtRemainUntil.BackColor = Drawing.Color.Silver

        'chkPassportNo.Checked = False
        'Me.txtPassportNo.Text = ""
        'Me.txtPassportNo.Enabled = False
        'Me.txtPassportNo.BackColor = Drawing.Color.Silver

        'chkRCHCode.Checked = False
        'Me.txtRCHCode.Text = ""
        'Me.txtRCHCode.Enabled = False
        'Me.txtRCHCode.BackColor = Drawing.Color.Silver

        'chkDOB.Checked = False
        'Me.txtDOB.Text = ""
        'Me.txtDOB.Enabled = False
        'Me.txtDOB.BackColor = Drawing.Color.Silver
    End Sub

    Private Sub ResetAll()
        txtTestCaseNo.Text = ""
        txtTestCaseDesc.Text = ""

        btnAdd2Vaccine.Visible = True
        btnRemove2Vaccine.Visible = False
        btnAdd2Voucher.Visible = True
        btnRemove2Voucher.Visible = False

        pnl2Vaccine.Visible = False
        chkSubsidyCode21.Checked = False
        chkSubsidyCode22.Checked = False

        pnl2Voucher.Visible = False
        chkHCVS2.Checked = False

        lblTip1.Visible = True
        lblTip2.Visible = True

        chkSPID.Font.Underline = True
        chkSPSurname.Font.Underline = False
        chkSPGivenname.Font.Underline = False
        chkPracticeID.Font.Underline = True
        chkPracticeName.Font.Underline = True

        chkDOReg.Text = "Date of Registration(dd-MM-yyyy)"
        chkDOB.Text = "Date of Birth(dd-MM-yyyy)"
        chkDOI.Text = "Date of Issue(dd-MM-yyyy)"

        chkServiceDate.Checked = False
        txtServiceDate.Text = ""

        rboHKIC.Checked = True
        rboEC.Checked = False
        rboHKBC.Checked = False
        rboADOPC.Checked = False
        rboVISA.Checked = False
        rboID235B.Checked = False
        rboDOCI.Checked = False
        rboREPMT.Checked = False

        chkSPID.Checked = False
        Me.txtSPID.Text = ""
        Me.txtSPID.Enabled = False
        Me.txtSPID.BackColor = Drawing.Color.Silver

        chkSPSurname.Checked = False
        Me.txtSPSurname.Text = ""
        Me.txtSPSurname.Enabled = False
        Me.txtSPSurname.BackColor = Drawing.Color.Silver

        chkSPGivenname.Checked = False
        Me.txtSPGivenName.Text = ""
        Me.txtSPGivenName.Enabled = False
        Me.txtSPGivenName.BackColor = Drawing.Color.Silver

        chkPracticeID.Checked = False
        Me.txtPracticeID.Text = ""
        Me.txtPracticeID.Enabled = False
        Me.txtPracticeID.BackColor = Drawing.Color.Silver

        chkPracticeName.Checked = False
        Me.txtPracticeName.Text = ""
        Me.txtPracticeName.Enabled = False
        Me.txtPracticeName.BackColor = Drawing.Color.Silver

        chkEntryNo.Checked = False
        Me.txtEntryNo.Text = ""
        Me.txtEntryNo.Enabled = False
        Me.txtEntryNo.BackColor = Drawing.Color.Silver

        chkDocumentNo.Checked = False
        Me.txtDocumentNo.Text = ""
        Me.txtDocumentNo.Enabled = False
        Me.txtDocumentNo.BackColor = Drawing.Color.Silver

        chkHKIC.Checked = False
        Me.txtHKIC.Text = ""
        Me.txtHKIC.Enabled = False
        Me.txtHKIC.BackColor = Drawing.Color.Silver

        chkRegNo.Checked = False
        Me.txtRegNo.Text = ""
        Me.txtRegNo.Enabled = False
        Me.txtRegNo.BackColor = Drawing.Color.Silver

        chkBirthEntryNo.Checked = False
        Me.txtBirthEntryNo.Text = ""
        Me.txtBirthEntryNo.Enabled = False
        Me.txtBirthEntryNo.BackColor = Drawing.Color.Silver

        chkPermitNo.Checked = False
        Me.txtPermitNo.Text = ""
        Me.txtPermitNo.Enabled = False
        Me.txtPermitNo.BackColor = Drawing.Color.Silver

        chkVISANo.Checked = False
        Me.txtVISANo.Text = ""
        Me.txtVISANo.Enabled = False
        Me.txtVISANo.BackColor = Drawing.Color.Silver

        chkSurname.Checked = False
        Me.txtSurname.Text = ""
        Me.txtSurname.Enabled = False
        Me.txtSurname.BackColor = Drawing.Color.Silver

        chkGivenName.Checked = False
        Me.txtGivenName.Text = ""
        Me.txtGivenName.Enabled = False
        Me.txtGivenName.BackColor = Drawing.Color.Silver

        chkGender.Checked = False
        Me.txtGender.Text = ""
        Me.txtGender.Enabled = False
        Me.txtGender.BackColor = Drawing.Color.Silver

        chkDOBType.Checked = False
        Me.txtDOBType.Text = ""
        Me.txtDOBType.Enabled = False
        Me.txtDOBType.BackColor = Drawing.Color.Silver

        chkAgeOn.Checked = False
        Me.txtAgeOn.Text = ""
        Me.txtAgeOn.Enabled = False
        Me.txtAgeOn.BackColor = Drawing.Color.Silver

        chkDOReg.Checked = False
        Me.txtDOReg.Text = ""
        Me.txtDOReg.Enabled = False
        Me.txtDOReg.BackColor = Drawing.Color.Silver

        chkDOBInWord.Checked = False
        Me.txtDOBInWord.Text = ""
        Me.txtDOBInWord.Enabled = False
        Me.txtDOBInWord.BackColor = Drawing.Color.Silver

        chkNameChi.Checked = False
        Me.txtNameChi.Text = ""
        Me.txtNameChi.Enabled = False
        Me.txtNameChi.BackColor = Drawing.Color.Silver

        chkDOI.Checked = False
        Me.txtDOI.Text = ""
        Me.txtDOI.Enabled = False
        Me.txtDOI.BackColor = Drawing.Color.Silver

        chkSerialNo.Checked = False
        Me.txtSerialNo.Text = ""
        Me.txtSerialNo.Enabled = False
        Me.txtSerialNo.BackColor = Drawing.Color.Silver

        chkReference.Checked = False
        Me.txtReference.Text = ""
        Me.txtReference.Enabled = False
        Me.txtReference.BackColor = Drawing.Color.Silver

        chkFreeReference.Checked = False
        Me.txtFreeReference.Text = ""
        Me.txtFreeReference.Enabled = False
        Me.txtFreeReference.BackColor = Drawing.Color.Silver

        chkRemainUntil.Checked = False
        Me.txtRemainUntil.Text = ""
        Me.txtRemainUntil.Enabled = False
        Me.txtRemainUntil.BackColor = Drawing.Color.Silver

        chkPassportNo.Checked = False
        Me.txtPassportNo.Text = ""
        Me.txtPassportNo.Enabled = False
        Me.txtPassportNo.BackColor = Drawing.Color.Silver

        chkRCHCode.Checked = False
        Me.txtRCHCode.Text = ""
        Me.txtRCHCode.Enabled = False
        Me.txtRCHCode.BackColor = Drawing.Color.Silver

        chkDOB.Checked = False
        Me.txtDOB.Text = ""
        Me.txtDOB.Enabled = False
        Me.txtDOB.BackColor = Drawing.Color.Silver

        chkHCVS.Checked = False
        chkVoucherClaimed.Checked = False
        txtVoucherClaimed.Text = ""
        chkProfCode.Checked = False
        txtProfCode.Text = ""
        chkL1Code.Checked = False
        txtL1Code.Text = ""
        chkL1DescEng.Checked = False
        txtL1DescEng.Text = ""
        chkL2Code.Checked = False
        txtL2Code.Text = ""
        chkL2DescEng.Checked = False
        txtL2DescEng.Text = ""
        chkPreSchoolInd.Checked = False
        chkDoseIntervalInd.Checked = False
        chkTSWInd.Checked = False

        chkHCVS2.Checked = False
        chkVoucherClaimed2.Checked = False
        txtVoucherClaimed2.Text = ""
        chkProfCode2.Checked = False
        txtProfCode2.Text = ""
        chkL1Code2.Checked = False
        txtL1Code2.Text = ""
        chkL1DescEng2.Checked = False
        txtL1DescEng2.Text = ""
        chkL2Code2.Checked = False
        txtL2Code2.Text = ""
        chkL2DescEng2.Checked = False
        txtL2DescEng2.Text = ""
        chkPreSchoolInd2.Checked = False
        chkDoseIntervalInd2.Checked = False
        chkTSWInd2.Checked = False

        txtSchemeCode.Text = ""
        chkRCHCode_Vaccine1.Checked = False
        txtRCHCode_Vaccine1.Text = ""
        chkVaccine1.Checked = False
        chkSubsidyCode1.Checked = False
        txtSubsidyCode1.Text = ""
        chkDoseSeq1.Checked = False
        txtDoseSeq1.Text = ""
        chkVaccine2.Checked = False
        chkSubsidyCode2.Checked = False
        txtSubsidyCode2.Text = ""
        chkDoseSeq2.Checked = False
        txtDoseSeq2.Text = ""
        chkPreSchoolInd_vaccine.Checked = False
        chkDoseIntervalInd_vaccine.Checked = False
        chkTSWInd_vaccine.Checked = False
        txtPreSchoolInd_vaccine.Text = ""
        txtDoseIntervalInd_vaccine.Text = ""
        txtTSWInd_vaccine.Text = ""

        txtSchemeCode2.Text = ""
        chkRCHCode_Vaccine2.Checked = False
        txtRCHCode_Vaccine2.Text = ""
        chkVaccine21.Checked = False
        chkSubsidyCode21.Checked = False
        txtSubsidyCode21.Text = ""
        chkDoseSeq21.Checked = False
        txtDoseSeq21.Text = ""
        chkVaccine22.Checked = False
        chkSubsidyCode22.Checked = False
        txtSubsidyCode22.Text = ""
        chkDoseSeq22.Checked = False
        txtDoseSeq22.Text = ""
        chkPreSchoolInd_vaccine2.Checked = False
        chkDoseIntervalInd_vaccine2.Checked = False
        chkTSWInd_vaccine2.Checked = False
        txtPreSchoolInd_vaccine2.Text = ""
        txtDoseIntervalInd_vaccine2.Text = ""
        txtTSWInd_vaccine2.Text = ""

        rboHKIC.Checked = True
        rboEC.Checked = False
        rboHKBC.Checked = False
        rboADOPC.Checked = False
        rboDOCI.Checked = False
        rboID235B.Checked = False
        rboVISA.Checked = False
        rboREPMT.Checked = False
        chkEntryNo.Font.Underline = False
        chkDocumentNo.Font.Underline = False
        chkHKIC.Font.Underline = True
        chkRegNo.Font.Underline = False
        chkBirthEntryNo.Font.Underline = False
        chkPermitNo.Font.Underline = False
        chkVISANo.Font.Underline = False
        chkSurname.Font.Underline = True
        chkGivenName.Font.Underline = True
        chkGender.Font.Underline = True
        chkDOB.Font.Underline = True
        chkDOBType.Font.Underline = False
        chkAgeOn.Font.Underline = False
        chkDOReg.Font.Underline = False
        chkDOBInWord.Font.Underline = False
        chkNameChi.Font.Underline = False
        chkDOI.Font.Underline = True
        chkSerialNo.Font.Underline = False
        chkReference.Font.Underline = False
        chkFreeReference.Font.Underline = False
        chkRemainUntil.Font.Underline = False
        chkPassportNo.Font.Underline = False
        chkRCHCode.Font.Underline = False

        '------------
        pnlSPInfo.Visible = True
        pnlAccountInfo.Visible = True
        pnlClaimInfo.Visible = True
    End Sub

    Private Sub btnShowResultXML_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowResultXML.Click
        If txtUATResponse.Visible = False Then
            txtUATResponse.Visible = True
            btnShowResultXML.Text = "Hide returned XML"
        Else
            txtUATResponse.Visible = False
            btnShowResultXML.Text = "Show returned XML"
        End If
    End Sub

    Private Sub chkSPInfo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSPInfo.CheckedChanged
        If chkSPInfo.Checked Then
            pnlSPInfo.Visible = True
        Else
            pnlSPInfo.Visible = False
        End If
    End Sub

    Private Sub chkAccountInfo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAccountInfo.CheckedChanged
        If chkAccountInfo.Checked Then
            pnlAccountInfo.Visible = True
        Else
            pnlAccountInfo.Visible = False
        End If
    End Sub

    Protected Sub chkClaimInfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkClaimInfo.CheckedChanged
        'If Me.chkClaimInfo.Checked Then
        '    Me.chkServiceDate.Enabled = True
        '    Me.txtServiceDate.Enabled = True
        '    Me.chkHCVS.Enabled = True
        '    Me.chkVoucherClaimed.Enabled = True
        '    Me.txtVoucherClaimed.Enabled = True
        '    Me.chkProfCode.Enabled = True
        '    Me.txtProfCode.Enabled = True
        '    Me.chkL1Code.Enabled = True
        '    Me.txtL1Code.Enabled = True
        '    Me.chkL1DescEng.Enabled = True
        '    Me.txtL1DescEng.Enabled = True
        '    Me.chkL2Code.Enabled = True
        '    Me.txtL2Code.Enabled = True
        '    Me.chkL2DescEng.Enabled = True
        '    Me.txtL2DescEng.Enabled = True
        '    Me.txtSchemeCode.Enabled = True
        '    Me.chkVaccine1.Enabled = True
        '    Me.chkVaccine2.Enabled = True
        '    Me.chkSubsidyCode1.Enabled = True
        '    Me.txtSubsidyCode1.Enabled = True
        '    Me.chkSubsidyCode2.Enabled = True
        '    Me.txtSubsidyCode2.Enabled = True
        '    Me.chkDoseSeq1.Enabled = True
        '    Me.txtDoseSeq1.Enabled = True
        '    Me.chkDoseSeq2.Enabled = True
        '    Me.txtDoseSeq2.Enabled = True

        '    Me.chkServiceDate.Checked = False
        '    Me.txtServiceDate.Text = String.Empty
        '    Me.chkHCVS.Checked = False
        '    Me.chkVoucherClaimed.Checked = False
        '    Me.txtVoucherClaimed.Text = String.Empty
        '    Me.chkProfCode.Checked = False
        '    Me.txtProfCode.Text = String.Empty
        '    Me.chkL1Code.Checked = False
        '    Me.txtL1Code.Text = String.Empty
        '    Me.chkL1DescEng.Checked = False
        '    Me.txtL1DescEng.Text = String.Empty
        '    Me.chkL2Code.Checked = False
        '    Me.txtL2Code.Text = String.Empty
        '    Me.chkL2DescEng.Checked = False
        '    Me.txtL2DescEng.Text = String.Empty
        '    Me.txtSchemeCode.Text = String.Empty
        '    Me.chkSubsidyCode1.Checked = False
        '    Me.txtSubsidyCode1.Text = String.Empty
        '    Me.chkSubsidyCode2.Checked = False
        '    Me.txtSubsidyCode2.Text = String.Empty
        '    Me.chkDoseSeq1.Checked = False
        '    Me.txtDoseSeq1.Text = String.Empty
        '    Me.chkDoseSeq2.Checked = False
        '    Me.txtDoseSeq2.Text = String.Empty
        '    Me.chkVaccine1.Checked = False
        '    Me.chkVaccine2.Checked = False
        'Else
        '    Me.chkServiceDate.Enabled = False
        '    Me.txtServiceDate.Enabled = False
        '    Me.chkHCVS.Enabled = False
        '    Me.chkVoucherClaimed.Enabled = False
        '    Me.txtVoucherClaimed.Enabled = False
        '    Me.chkProfCode.Enabled = False
        '    Me.txtProfCode.Enabled = False
        '    Me.chkL1Code.Enabled = False
        '    Me.txtL1Code.Enabled = False
        '    Me.chkL1DescEng.Enabled = False
        '    Me.txtL1DescEng.Enabled = False
        '    Me.chkL2Code.Enabled = False
        '    Me.txtL2Code.Enabled = False
        '    Me.chkL2DescEng.Enabled = False
        '    Me.txtL2DescEng.Enabled = False
        '    Me.txtSchemeCode.Enabled = False
        '    Me.chkVaccine1.Enabled = False
        '    Me.chkVaccine2.Enabled = False
        '    Me.chkSubsidyCode1.Enabled = False
        '    Me.txtSubsidyCode1.Enabled = False
        '    Me.chkSubsidyCode2.Enabled = False
        '    Me.txtSubsidyCode2.Enabled = False
        '    Me.chkDoseSeq1.Enabled = False
        '    Me.txtDoseSeq1.Enabled = False
        '    Me.chkDoseSeq2.Enabled = False
        '    Me.txtDoseSeq2.Enabled = False

        '    Me.chkServiceDate.Checked = False
        '    Me.txtServiceDate.Text = String.Empty
        '    Me.chkHCVS.Checked = False
        '    Me.chkVoucherClaimed.Checked = False
        '    Me.txtVoucherClaimed.Text = String.Empty
        '    Me.chkProfCode.Checked = False
        '    Me.txtProfCode.Text = String.Empty
        '    Me.chkL1Code.Checked = False
        '    Me.txtL1Code.Text = String.Empty
        '    Me.chkL1DescEng.Checked = False
        '    Me.txtL1DescEng.Text = String.Empty
        '    Me.chkL2Code.Checked = False
        '    Me.txtL2Code.Text = String.Empty
        '    Me.chkL2DescEng.Checked = False
        '    Me.txtL2DescEng.Text = String.Empty
        '    Me.txtSchemeCode.Text = String.Empty
        '    Me.chkSubsidyCode1.Checked = False
        '    Me.txtSubsidyCode1.Text = String.Empty
        '    Me.chkSubsidyCode2.Checked = False
        '    Me.txtSubsidyCode2.Text = String.Empty
        '    Me.chkDoseSeq1.Checked = False
        '    Me.txtDoseSeq1.Text = String.Empty
        '    Me.chkDoseSeq2.Checked = False
        '    Me.txtDoseSeq2.Text = String.Empty
        '    Me.chkVaccine1.Checked = False
        '    Me.chkVaccine2.Checked = False
        'End If
        If chkClaimInfo.Checked Then
            pnlClaimInfo.Visible = True
            lblTip2.Visible = True
        Else
            pnlClaimInfo.Visible = False
            lblTip2.Visible = False
        End If

    End Sub

    Private Sub btnAdd2Vaccine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd2Vaccine.Click
        btnAdd2Vaccine.Visible = False
        btnRemove2Vaccine.Visible = True

        pnl2Vaccine.Visible = True
    End Sub

    Private Sub btnAdd2Voucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd2Voucher.Click
        btnAdd2Voucher.Visible = False
        btnRemove2Voucher.Visible = True

        pnl2Voucher.Visible = True
    End Sub

    Private Sub btnRemove2Vaccine_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove2Vaccine.Click
        btnAdd2Vaccine.Visible = True
        btnRemove2Vaccine.Visible = False

        pnl2Vaccine.Visible = False
        chkSubsidyCode21.Checked = False
        chkSubsidyCode22.Checked = False
    End Sub

    Private Sub btnRemove2Voucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove2Voucher.Click
        btnAdd2Voucher.Visible = True
        btnRemove2Voucher.Visible = False

        pnl2Voucher.Visible = False
        chkHCVS2.Checked = False
    End Sub

#End Region

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

    Protected Sub btnFreeText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFreeText.Click
        'Response.Redirect("~\TestReadXML.aspx")
    End Sub

    Public Function ConvertDateDFormat(ByVal strDate As String) As String

        Dim strRenderedDate As String = String.Empty

        strDate = strDate.Replace("-", "")

        If strDate.Trim.Length = 8 Then
            strRenderedDate = strDate.Substring(4, 4) + strDate.Substring(2, 2) + strDate.Substring(0, 2)
        Else
            If strDate.Trim.Length = 6 Then
                strRenderedDate = strDate.Substring(2, 4) + strDate.Substring(0, 2)
            Else
                strRenderedDate = strDate
            End If
        End If

        Return strRenderedDate
    End Function


#Region "XML Result Reader"

    Private Sub ExtractResultForUploadClaim(ByVal strXML As String)

        Dim strValue As String = String.Empty

        Try
            If ShowErrorInfo(strXML) Then
                Exit Sub
            End If

            Dim Xml As XmlDocument = New XmlDocument
            Xml.LoadXml(strXML)

            txtExtractedResult.Text = "-------------------------------Upload Claim Result--------------------------------" + vbCrLf

            Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            Dim nlTranInfo As XmlNodeList = nlResult.Item(0).SelectNodes("./TranInfo")

            For Each node As XmlNode In nlTranInfo
                strValue = ReadString(node, "TranIndex")
                txtExtractedResult.Text = txtExtractedResult.Text + "TranIndex     : " + strValue + " " + vbCrLf
                strValue = ReadString(node, "TranID")
                txtExtractedResult.Text = txtExtractedResult.Text + "TranID        : " + strValue + " " + vbCrLf
            Next

        Catch ex As Exception
            txtExtractedResult.Text = "Internal error"
        End Try
    End Sub

    Private Sub ExtractResultForRCHNameQuery(ByVal strXML As String)

        Dim strValue As String = String.Empty

        Try
            If ShowErrorInfo(strXML) Then
                Exit Sub
            End If

            Dim Xml As XmlDocument = New XmlDocument
            Xml.LoadXml(strXML)

            txtExtractedResult.Text = "-------------------------------RCH Name Query--------------------------------" + vbCrLf

            Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            strValue = ReadString(nlResult.Item(0), "HomeNameEng")
            txtExtractedResult.Text = txtExtractedResult.Text + "HomeNameEng : " + strValue.Trim + vbCrLf

            strValue = ReadString(nlResult.Item(0), "HomeNameChi")
            txtExtractedResult.Text = txtExtractedResult.Text + "HomeNameChi : " + strValue.Trim + vbCrLf

            strValue = ReadString(nlResult.Item(0), "AddressEng")
            txtExtractedResult.Text = txtExtractedResult.Text + "AddressEng : " + strValue.Trim + vbCrLf

            strValue = ReadString(nlResult.Item(0), "AddressChi")
            txtExtractedResult.Text = txtExtractedResult.Text + "AddressChi : " + strValue.Trim + vbCrLf

        Catch ex As Exception
            txtExtractedResult.Text = "Internal error"
        End Try
    End Sub

    Private Sub ExtractResultForGetReasonForVisit(ByVal strXML As String)

        Dim strValue As String = String.Empty

        Try
            If ShowErrorInfo(strXML) Then
                Exit Sub
            End If

            Dim Xml As XmlDocument = New XmlDocument
            Xml.LoadXml(strXML)

            Dim nlL1 As XmlNodeList = Xml.GetElementsByTagName("ReasonForVisitL1")
            Dim nlL1Item As XmlNodeList = nlL1.Item(0).SelectNodes("./L1Entry")

            If nlL1Item.Count > 0 Then
                txtExtractedResult.Text = "------------------------------------List of Reason for Vist------------------------------------" + vbCrLf + vbCrLf
            End If

            For Each node As XmlNode In nlL1Item
                strValue = ReadString(node, "ProfCode")
                txtExtractedResult.Text = txtExtractedResult.Text + "ProfCode :" + strValue + " " + vbCrLf
                strValue = ReadString(node, "L1Code")
                txtExtractedResult.Text = txtExtractedResult.Text + "L1Code   :" + strValue + " " + vbCrLf
                strValue = ReadString(node, "L1DescEng")
                txtExtractedResult.Text = txtExtractedResult.Text + "L1DescEng:" + strValue + " " + vbCrLf
                strValue = ReadString(node, "L1DescChi")
                txtExtractedResult.Text = txtExtractedResult.Text + "L1DescChi:" + strValue + " " + vbCrLf
                txtExtractedResult.Text = txtExtractedResult.Text + vbCrLf
            Next

            Dim nlL2 As XmlNodeList = Xml.GetElementsByTagName("ReasonForVisitL2")
            Dim nlL2Item As XmlNodeList = nlL2.Item(0).SelectNodes("./L2Entry")
            For Each node As XmlNode In nlL2Item
                strValue = ReadString(node, "ProfCode")
                txtExtractedResult.Text = txtExtractedResult.Text + "ProfCode :" + strValue + " " + vbCrLf
                strValue = ReadString(node, "L1Code")
                txtExtractedResult.Text = txtExtractedResult.Text + "L1Code   :" + strValue + " " + vbCrLf
                strValue = ReadString(node, "L2Code")
                txtExtractedResult.Text = txtExtractedResult.Text + "L2DescEng:" + strValue + " " + vbCrLf
                strValue = ReadString(node, "L2DescEng")
                txtExtractedResult.Text = txtExtractedResult.Text + "L2DescEng:" + strValue + " " + vbCrLf
                strValue = ReadString(node, "L2DescChi")
                txtExtractedResult.Text = txtExtractedResult.Text + "L2DescChi:" + strValue + " " + vbCrLf
                txtExtractedResult.Text = txtExtractedResult.Text + vbCrLf
            Next

        Catch ex As Exception
            txtExtractedResult.Text = "Internal error"
        End Try
    End Sub

    Private Sub ExtractResultForValidatedAccountQuery(ByVal strXML As String)

        Dim strValue As String = String.Empty

        Try
            If ShowErrorInfo(strXML) Then
                Exit Sub
            End If

            Dim Xml As XmlDocument = New XmlDocument
            Dim strValid As String = ""
            Xml.LoadXml(strXML)

            txtExtractedResult.Text = "-------------------------------Validated Account Query--------------------------------" + vbCrLf

            Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            strValid = ReadString(nlResult.Item(0), "AccountMatched")
            If strValid.Trim = "Y" Then
                txtExtractedResult.Text = txtExtractedResult.Text + "AccountMatched : Y"
            ElseIf strValid.Trim = "N" Then
                txtExtractedResult.Text = txtExtractedResult.Text + "AccountMatched : N"
            ElseIf strValid.Trim = "X" Then
                txtExtractedResult.Text = txtExtractedResult.Text + "AccountMatched : X"
            End If

        Catch ex As Exception
            txtExtractedResult.Text = "Internal error"
        End Try
    End Sub

    Private Sub ExtractResultForVoucherQuery(ByVal strXML As String)

        Dim strValue As String = String.Empty

        Try
            If ShowErrorInfo(strXML) Then
                Exit Sub
            End If

            Dim Xml As XmlDocument = New XmlDocument
            Xml.LoadXml(strXML)

            txtExtractedResult.Text = "-------------------------------Voucher Query--------------------------------" + vbCrLf

            Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("SchemeInfo")
            strValue = ReadString(nlResult.Item(0), "SchemeCode")
            txtExtractedResult.Text = txtExtractedResult.Text + "SchemeCode      : " + strValue.Trim + vbCrLf

            strValue = ReadString(nlResult.Item(0), "VoucherRemained")
            txtExtractedResult.Text = txtExtractedResult.Text + "VoucherRemained : " + strValue.Trim + vbCrLf


        Catch ex As Exception
            txtExtractedResult.Text = "Internal error"
        End Try
    End Sub

    Private Sub ExtractResultForSPPracticeValidation(ByVal strXML As String)

        Dim strValue As String = String.Empty

        Try
            If ShowErrorInfo(strXML) Then
                Exit Sub
            End If

            Dim Xml As XmlDocument = New XmlDocument
            Dim strValid As String = ""
            Xml.LoadXml(strXML)

            txtExtractedResult.Text = "---------------------------------Service Provider Validation--------------------------------" + vbCrLf

            Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            strValid = ReadString(nlResult.Item(0), "IsCorrect")
            If strValid.Trim = "Y" Then
                txtExtractedResult.Text = txtExtractedResult.Text + "IsCorrect : Y"
            ElseIf strValid.Trim = "N" Then
                txtExtractedResult.Text = txtExtractedResult.Text + "IsCorrect : N"
            End If

        Catch ex As Exception
            txtExtractedResult.Text = "Internal error"
        End Try
    End Sub

    Private Function ShowErrorInfo(ByVal strXML As String) As Boolean
        Dim Xml As XmlDocument = New XmlDocument
        Xml.LoadXml(strXML)

        Dim strValue As String = String.Empty
        Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("ErrorInfo")

        If nlResult.Count > 0 Then
            txtExtractedResult.Text = "---------------------------------Error--------------------------------" + vbCrLf + vbCrLf
            strValue = ReadString(nlResult.Item(0), "ErrorCode")
            txtExtractedResult.Text = txtExtractedResult.Text + "ErrorCode    : " + strValue + " " + vbCrLf
            strValue = ReadString(nlResult.Item(0), "ErrorMessage")
            txtExtractedResult.Text = txtExtractedResult.Text + "ErrorMessage : " + strValue + " " + vbCrLf
            Return True
        Else
            Return False
        End If

    End Function


    Private Function ReadString(ByVal node As XmlNode, _
                                ByVal sTagName As String)

        Dim nlTemp As XmlNodeList
        nlTemp = node.SelectNodes("./" + sTagName)
        If nlTemp.Count <> 1 Then
            Return String.Empty
        Else
            Return nlTemp(0).InnerText
        End If
    End Function
#End Region




#Region "UAT Test Data"

    Protected Sub btnSearchAndFill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchAndFill.Click

        Dim strUATTestData As String = String.Empty

        strUATTestData = System.Configuration.ConfigurationManager.AppSettings("UATTestData").ToString()

        'Dim ds As DataSet = Nothing

        'ds = GetDataFromExcel(strUATTestData, txtTestCaseNo.Text.Trim)

        'If Not IsNothing(ds) Then
        '    If ds.Tables(0).Rows.Count = 1 Then
        '        FillData2Screen(ds)
        '        'txtTestCaseDesc.Text = "--Success--"
        '    Else
        '        txtTestCaseDesc.Text = "--Not Reocrd Found--"
        '    End If
        'Else
        '    txtTestCaseDesc.Text = "--Not Reocrd Found--"
        'End If

        Dim dt As DataTable = Nothing
        Dim dr As DataRow() = Nothing

        If IsNothing(Session("TempExcel")) Then
            dt = ExtractImportFile(strUATTestData)
        Else
            dt = Session("TempExcel")
        End If

        Session("TempExcel") = dt

        dr = dt.Select("TestCase = '" + txtTestCaseNo.Text.Trim + "'")
        If Not IsNothing(dr) Then
            If dr.Length = 1 Then
                FillData2Screen(dr(0))
            Else
                txtTestCaseDesc.Text = "--Not Reocrd Found--"
            End If
        Else
            txtTestCaseDesc.Text = "--Not Reocrd Found--"
        End If
    End Sub

    Public Function GetDataFromExcel(ByVal FileName As String, ByVal CaseNo As String) As System.Data.DataSet

        Try
            Dim strConn As String = _
                "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                "Data Source=" & FileName & ";Extended Properties=Excel 8.0;"
            Dim objConn _
                As New System.Data.OleDb.OleDbConnection(strConn)
            objConn.Open()

            Dim objCmd As New System.Data.OleDb.OleDbCommand( _
                "select * from [Sheet1$] where TestCase =" & CaseNo.Trim, objConn)
            'Dim objCmd As New System.Data.OleDb.OleDbCommand( _
            '    "select * from [Sheet1$] ", objConn)
            Dim objDA As New System.Data.OleDb.OleDbDataAdapter()
            objDA.SelectCommand = objCmd

            Dim objDS As New System.Data.DataSet()
            objDA.Fill(objDS)
            objConn.Close()
            Return objDS
        Catch

            Return Nothing
        End Try
    End Function

    Private Function ExtractImportFile(ByVal strFileFullPath As String) As DataTable

        Dim dtResult As New DataTable()

        Dim xlsApp As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.ApplicationClass()
        Dim xlsWorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim xlsWorkSheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
        Dim xlsWorkSheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Dim strLastColumn As String = "HM"

        xlsApp.DisplayAlerts = False

        Try

            xlsWorkBook = xlsApp.Workbooks.Open(strFileFullPath, 0, False, 5, "")

            If xlsWorkBook.HasPassword Then
                xlsWorkBook.Close()
                xlsApp.Quit()
                Return Nothing
            End If

            xlsWorkSheet = xlsWorkBook.Worksheets(1)

            ' Read Header
            Dim xlsRange As Microsoft.Office.Interop.Excel.Range = xlsWorkSheet.Range("A1:" + strLastColumn + "1", Type.Missing)

            Dim array As Array = CType(xlsRange.Cells.Value2, Array)

            For Each objValue As Object In array
                dtResult.Columns.Add(New DataColumn(objValue.ToString(), GetType(String)))
            Next

            ' Read Data
            Dim blnReadToEND As Boolean = False

            Dim intCounter As Integer = 2
            While Not blnReadToEND
                xlsRange = xlsWorkSheet.Range("A" + intCounter.ToString() + ":" + strLastColumn + intCounter.ToString(), Type.Missing)

                array = CType(xlsRange.Cells.Value2, Array)

                Dim blnHasValue = False
                For Each objValue As Object In array
                    If Not objValue Is Nothing AndAlso objValue.ToString().Trim() <> "" Then
                        blnHasValue = True
                        Exit For
                    End If
                Next

                If blnHasValue Then
                    Dim j As Integer = 0
                    Dim drRow As DataRow = dtResult.NewRow()
                    For Each objValue As Object In array
                        drRow(j) = objValue
                        j = j + 1
                    Next
                    dtResult.Rows.Add(drRow)
                Else
                    blnReadToEND = True
                End If

                intCounter = intCounter + 1
            End While

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Catch ex As Exception
            Return Nothing
        Finally
            If Not xlsWorkSheet Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkSheet)
                xlsWorkSheet = Nothing
            End If

            If Not xlsWorkBook Is Nothing Then
                xlsWorkBook.Close(True, Type.Missing, Type.Missing)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBook)
                xlsWorkBook = Nothing
            End If

            If Not xlsApp Is Nothing Then
                xlsApp.Workbooks.Close()
                xlsApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp)
                xlsApp = Nothing
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

        Return dtResult

    End Function

    'Public Sub FillData2Screen(ByVal ds As DataSet)
    Public Sub FillData2Screen(ByVal dr As DataRow)
        Dim strTempCaseNo As String = String.Empty
        strTempCaseNo = txtTestCaseNo.Text

        Dim drCase As DataRow = Nothing
        Dim strTestCaseDesc As String = String.Empty
        Dim strFunction As String = String.Empty
        Dim strSPInfo As String = String.Empty
        Dim strAccInfo As String = String.Empty
        Dim strClaimInfo As String = String.Empty

        Dim strcSPID As String = String.Empty
        Dim strvSPID As String = String.Empty
        Dim strcSPSurname As String = String.Empty
        Dim strvSPSurname As String = String.Empty
        Dim strcSPGivenName As String = String.Empty
        Dim strvSPGivenName As String = String.Empty
        Dim strcPracticeID As String = String.Empty
        Dim strvPracticeID As String = String.Empty
        Dim strcPracticeName As String = String.Empty
        Dim strvPracticeName As String = String.Empty
        Dim strcRCHCode As String = String.Empty
        Dim strvRCHCode As String = String.Empty

        Dim strDocType As String = String.Empty
        Dim strcEntryNo As String = String.Empty
        Dim strvEntryNo As String = String.Empty
        Dim strcDocNo As String = String.Empty
        Dim strvDocNo As String = String.Empty
        Dim strcHKIC As String = String.Empty
        Dim strvHKIC As String = String.Empty
        Dim strcRegNo As String = String.Empty
        Dim strvRegNo As String = String.Empty
        Dim strcBirthEntryNo As String = String.Empty
        Dim strvBirthEntryNo As String = String.Empty
        Dim strcPermitNo As String = String.Empty
        Dim strvPermitNo As String = String.Empty
        Dim strcVISANo As String = String.Empty
        Dim strvVISANo As String = String.Empty
        Dim strcSurname As String = String.Empty
        Dim strvSurname As String = String.Empty
        Dim strcGivenName As String = String.Empty
        Dim strvGivenName As String = String.Empty
        Dim strcGender As String = String.Empty
        Dim strvGender As String = String.Empty
        Dim strcDOB As String = String.Empty
        Dim strvDOB As String = String.Empty
        Dim strcDOBType As String = String.Empty
        Dim strvDOBType As String = String.Empty
        Dim strcAgeOn As String = String.Empty
        Dim strvAgeOn As String = String.Empty
        Dim strcDOR As String = String.Empty
        Dim strvDOR As String = String.Empty
        Dim strcDOBInWord As String = String.Empty
        Dim strvDOBInWord As String = String.Empty
        Dim strcChineseName As String = String.Empty
        Dim strvChineseName As String = String.Empty
        Dim strcDOI As String = String.Empty
        Dim strvDOI As String = String.Empty
        Dim strcSerialNo As String = String.Empty
        Dim strvSerialNo As String = String.Empty
        Dim strcRef As String = String.Empty
        Dim strvRef As String = String.Empty
        Dim strcFreeRef As String = String.Empty
        Dim strvFreeRef As String = String.Empty
        Dim strcRemainUntil As String = String.Empty
        Dim strvRemainUntil As String = String.Empty
        Dim strcPassportNo As String = String.Empty
        Dim strvPassportNo As String = String.Empty
        Dim strcServiceDate As String = String.Empty
        Dim strvServiceDate As String = String.Empty

        Dim strcVoucher1 As String = String.Empty
        Dim strcVoucherClaimed1 As String = String.Empty
        Dim strvVoucherClaimed1 As String = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Dim strcCoPaymentFee1 As String = String.Empty
        Dim strvCoPaymentFee1 As String = String.Empty

        Dim strcProfCode1 As String = String.Empty
        Dim strvProfCode1 As String = String.Empty

        Dim strcPriorityCode1 As String = String.Empty
        Dim strvPriorityCode1 As String = String.Empty

        Dim strcL1Code1 As String = String.Empty
        Dim strvL1Code1 As String = String.Empty
        Dim strcL1DescEng1 As String = String.Empty
        Dim strvL1DescEng1 As String = String.Empty
        Dim strcL2Code1 As String = String.Empty
        Dim strvL2Code1 As String = String.Empty
        Dim strcL2DescEng1 As String = String.Empty
        Dim strvL2DescEng1 As String = String.Empty

        'S1
        Dim strcProfCode1_S1 As String = String.Empty
        Dim strvProfCode1_S1 As String = String.Empty

        Dim strcPriorityCode1_S1 As String = String.Empty
        Dim strvPriorityCode1_S1 As String = String.Empty

        Dim strcL1Code1_S1 As String = String.Empty
        Dim strvL1Code1_S1 As String = String.Empty
        Dim strcL1DescEng1_S1 As String = String.Empty
        Dim strvL1DescEng1_S1 As String = String.Empty
        Dim strcL2Code1_S1 As String = String.Empty
        Dim strvL2Code1_S1 As String = String.Empty
        Dim strcL2DescEng1_S1 As String = String.Empty
        Dim strvL2DescEng1_S1 As String = String.Empty

        'S2
        Dim strcProfCode1_S2 As String = String.Empty
        Dim strvProfCode1_S2 As String = String.Empty

        Dim strcPriorityCode1_S2 As String = String.Empty
        Dim strvPriorityCode1_S2 As String = String.Empty

        Dim strcL1Code1_S2 As String = String.Empty
        Dim strvL1Code1_S2 As String = String.Empty
        Dim strcL1DescEng1_S2 As String = String.Empty
        Dim strvL1DescEng1_S2 As String = String.Empty
        Dim strcL2Code1_S2 As String = String.Empty
        Dim strvL2Code1_S2 As String = String.Empty
        Dim strcL2DescEng1_S2 As String = String.Empty
        Dim strvL2DescEng1_S2 As String = String.Empty

        'S3
        Dim strcProfCode1_S3 As String = String.Empty
        Dim strvProfCode1_S3 As String = String.Empty

        Dim strcPriorityCode1_S3 As String = String.Empty
        Dim strvPriorityCode1_S3 As String = String.Empty

        Dim strcL1Code1_S3 As String = String.Empty
        Dim strvL1Code1_S3 As String = String.Empty
        Dim strcL1DescEng1_S3 As String = String.Empty
        Dim strvL1DescEng1_S3 As String = String.Empty
        Dim strcL2Code1_S3 As String = String.Empty
        Dim strvL2Code1_S3 As String = String.Empty
        Dim strcL2DescEng1_S3 As String = String.Empty
        Dim strvL2DescEng1_S3 As String = String.Empty


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Dim strcPreSchoolInd1 As String = String.Empty
        Dim strvPreSchoolInd1 As String = String.Empty
        Dim strcDoseIntervalInd1 As String = String.Empty
        Dim strvDoseIntervalInd1 As String = String.Empty
        Dim strcTSWInd1 As String = String.Empty
        Dim strvTSWInd1 As String = String.Empty

        Dim strcVoucher2 As String = String.Empty
        Dim strcVoucherClaimed2 As String = String.Empty
        Dim strvVoucherClaimed2 As String = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Dim strcCoPaymentFee2 As String = String.Empty
        Dim strvCoPaymentFee2 As String = String.Empty

        Dim strcProfCode2 As String = String.Empty
        Dim strvProfCode2 As String = String.Empty

        Dim strcPriorityCode2 As String = String.Empty
        Dim strvPriorityCode2 As String = String.Empty

        Dim strcL1Code2 As String = String.Empty
        Dim strvL1Code2 As String = String.Empty
        Dim strcL1DescEng2 As String = String.Empty
        Dim strvL1DescEng2 As String = String.Empty
        Dim strcL2Code2 As String = String.Empty
        Dim strvL2Code2 As String = String.Empty
        Dim strcL2DescEng2 As String = String.Empty
        Dim strvL2DescEng2 As String = String.Empty

        'S1
        Dim strcProfCode2_S1 As String = String.Empty
        Dim strvProfCode2_S1 As String = String.Empty

        Dim strcPriorityCode2_S1 As String = String.Empty
        Dim strvPriorityCode2_S1 As String = String.Empty

        Dim strcL1Code2_S1 As String = String.Empty
        Dim strvL1Code2_S1 As String = String.Empty
        Dim strcL1DescEng2_S1 As String = String.Empty
        Dim strvL1DescEng2_S1 As String = String.Empty
        Dim strcL2Code2_S1 As String = String.Empty
        Dim strvL2Code2_S1 As String = String.Empty
        Dim strcL2DescEng2_S1 As String = String.Empty
        Dim strvL2DescEng2_S1 As String = String.Empty

        'S2
        Dim strcProfCode2_S2 As String = String.Empty
        Dim strvProfCode2_S2 As String = String.Empty

        Dim strcPriorityCode2_S2 As String = String.Empty
        Dim strvPriorityCode2_S2 As String = String.Empty

        Dim strcL1Code2_S2 As String = String.Empty
        Dim strvL1Code2_S2 As String = String.Empty
        Dim strcL1DescEng2_S2 As String = String.Empty
        Dim strvL1DescEng2_S2 As String = String.Empty
        Dim strcL2Code2_S2 As String = String.Empty
        Dim strvL2Code2_S2 As String = String.Empty
        Dim strcL2DescEng2_S2 As String = String.Empty
        Dim strvL2DescEng2_S2 As String = String.Empty

        'S3
        Dim strcProfCode2_S3 As String = String.Empty
        Dim strvProfCode2_S3 As String = String.Empty

        Dim strcPriorityCode2_S3 As String = String.Empty
        Dim strvPriorityCode2_S3 As String = String.Empty

        Dim strcL1Code2_S3 As String = String.Empty
        Dim strvL1Code2_S3 As String = String.Empty
        Dim strcL1DescEng2_S3 As String = String.Empty
        Dim strvL1DescEng2_S3 As String = String.Empty
        Dim strcL2Code2_S3 As String = String.Empty
        Dim strvL2Code2_S3 As String = String.Empty
        Dim strcL2DescEng2_S3 As String = String.Empty
        Dim strvL2DescEng2_S3 As String = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Dim strcPreSchoolInd2 As String = String.Empty
        Dim strvPreSchoolInd2 As String = String.Empty
        Dim strcDoseIntervalInd2 As String = String.Empty
        Dim strvDoseIntervalInd2 As String = String.Empty
        Dim strcTSWInd2 As String = String.Empty
        Dim strvTSWInd2 As String = String.Empty

        Dim strSchemeCode1 As String = String.Empty
        Dim strcRCHCode1 As String = String.Empty
        Dim strvRCHCode1 As String = String.Empty
        Dim strcVaccine11 As String = String.Empty
        Dim strcSubsidyCode11 As String = String.Empty
        Dim strvSubsidyCode11 As String = String.Empty
        Dim strcDoseSeq11 As String = String.Empty
        Dim strvDoseSeq11 As String = String.Empty
        Dim strcVaccine12 As String = String.Empty
        Dim strcSubsidyCode12 As String = String.Empty
        Dim strvSubsidyCode12 As String = String.Empty
        Dim strcDoseSeq12 As String = String.Empty
        Dim strvDoseSeq12 As String = String.Empty
        Dim strcPreSchoolIndv1 As String = String.Empty
        Dim strvPreSchoolIndv1 As String = String.Empty
        Dim strcDoseIntervalIndv1 As String = String.Empty
        Dim strvDoseIntervalIndv1 As String = String.Empty
        Dim strcTSWIndv1 As String = String.Empty
        Dim strvTSWIndv1 As String = String.Empty

        Dim strSchemeCode2 As String = String.Empty
        Dim strcRCHCode2 As String = String.Empty
        Dim strvRCHCode2 As String = String.Empty
        Dim strcVaccine21 As String = String.Empty
        Dim strcSubsidyCode21 As String = String.Empty
        Dim strvSubsidyCode21 As String = String.Empty
        Dim strcDoseSeq21 As String = String.Empty
        Dim strvDoseSeq21 As String = String.Empty
        Dim strcVaccine22 As String = String.Empty
        Dim strcSubsidyCode22 As String = String.Empty
        Dim strvSubsidyCode22 As String = String.Empty
        Dim strcDoseSeq22 As String = String.Empty
        Dim strvDoseSeq22 As String = String.Empty
        Dim strcPreSchoolIndv2 As String = String.Empty
        Dim strvPreSchoolIndv2 As String = String.Empty
        Dim strcDoseIntervalIndv2 As String = String.Empty
        Dim strvDoseIntervalIndv2 As String = String.Empty
        Dim strcTSWIndv2 As String = String.Empty
        Dim strvTSWIndv2 As String = String.Empty

        drCase = dr
        'Retrieve Data
        strTestCaseDesc = IIf(IsDBNull(drCase.Item("TestCaseDesc")), String.Empty, drCase.Item("TestCaseDesc"))
        strFunction = IIf(IsDBNull(drCase.Item("Function")), String.Empty, drCase.Item("Function"))
        strSPInfo = IIf(IsDBNull(drCase.Item("SPInfo")), String.Empty, drCase.Item("SPInfo"))
        strAccInfo = IIf(IsDBNull(drCase.Item("AccInfo")), String.Empty, drCase.Item("AccInfo"))
        strClaimInfo = IIf(IsDBNull(drCase.Item("ClaimInfo")), String.Empty, drCase.Item("ClaimInfo"))

        strcSPID = IIf(IsDBNull(drCase.Item("cSPID")), String.Empty, drCase.Item("cSPID"))
        strvSPID = IIf(IsDBNull(drCase.Item("vSPID")), String.Empty, drCase.Item("vSPID"))
        strcSPSurname = IIf(IsDBNull(drCase.Item("cSPSurname")), String.Empty, drCase.Item("cSPSurname"))
        strvSPSurname = IIf(IsDBNull(drCase.Item("vSPSurname")), String.Empty, drCase.Item("vSPSurname"))
        strcSPGivenName = IIf(IsDBNull(drCase.Item("cSPGivenName")), String.Empty, drCase.Item("cSPGivenName"))
        strvSPGivenName = IIf(IsDBNull(drCase.Item("vSPGivenName")), String.Empty, drCase.Item("vSPGivenName"))
        strcPracticeID = IIf(IsDBNull(drCase.Item("cPracticeID")), String.Empty, drCase.Item("cPracticeID"))
        strvPracticeID = IIf(IsDBNull(drCase.Item("vPracticeID")), String.Empty, drCase.Item("vPracticeID"))
        strcPracticeName = IIf(IsDBNull(drCase.Item("cPracticeName")), String.Empty, drCase.Item("cPracticeName"))
        strvPracticeName = IIf(IsDBNull(drCase.Item("vPracticeName")), String.Empty, drCase.Item("vPracticeName"))
        strcRCHCode = IIf(IsDBNull(drCase.Item("cRCHCode")), String.Empty, drCase.Item("cRCHCode"))
        strvRCHCode = IIf(IsDBNull(drCase.Item("vRCHCode")), String.Empty, drCase.Item("vRCHCode"))

        strDocType = IIf(IsDBNull(drCase.Item("DocType")), String.Empty, drCase.Item("DocType"))
        strcEntryNo = IIf(IsDBNull(drCase.Item("cEntryNo")), String.Empty, drCase.Item("cEntryNo"))
        strvEntryNo = IIf(IsDBNull(drCase.Item("vEntryNo")), String.Empty, drCase.Item("vEntryNo"))
        strcDocNo = IIf(IsDBNull(drCase.Item("cDocNo")), String.Empty, drCase.Item("cDocNo"))
        strvDocNo = IIf(IsDBNull(drCase.Item("vDocNo")), String.Empty, drCase.Item("vDocNo"))
        strcHKIC = IIf(IsDBNull(drCase.Item("cHKIC")), String.Empty, drCase.Item("cHKIC"))
        strvHKIC = IIf(IsDBNull(drCase.Item("vHKIC")), String.Empty, drCase.Item("vHKIC"))
        strcRegNo = IIf(IsDBNull(drCase.Item("cRegNo")), String.Empty, drCase.Item("cRegNo"))
        strvRegNo = IIf(IsDBNull(drCase.Item("vRegNo")), String.Empty, drCase.Item("vRegNo"))
        strcBirthEntryNo = IIf(IsDBNull(drCase.Item("cBirthEntryNo")), String.Empty, drCase.Item("cBirthEntryNo"))
        strvBirthEntryNo = IIf(IsDBNull(drCase.Item("vBirthEntryNo")), String.Empty, drCase.Item("vBirthEntryNo"))
        strcPermitNo = IIf(IsDBNull(drCase.Item("cPermitNo")), String.Empty, drCase.Item("cPermitNo"))
        strvPermitNo = IIf(IsDBNull(drCase.Item("vPermitNo")), String.Empty, drCase.Item("vPermitNo"))
        strcVISANo = IIf(IsDBNull(drCase.Item("cVISANo")), String.Empty, drCase.Item("cVISANo"))
        strvVISANo = IIf(IsDBNull(drCase.Item("vVISANo")), String.Empty, drCase.Item("vVISANo"))
        strcSurname = IIf(IsDBNull(drCase.Item("cSurname")), String.Empty, drCase.Item("cSurname"))
        strvSurname = IIf(IsDBNull(drCase.Item("vSurname")), String.Empty, drCase.Item("vSurname"))
        strcGivenName = IIf(IsDBNull(drCase.Item("cGivenName")), String.Empty, drCase.Item("cGivenName"))
        strvGivenName = IIf(IsDBNull(drCase.Item("vGivenName")), String.Empty, drCase.Item("vGivenName"))
        strcGender = IIf(IsDBNull(drCase.Item("cGender")), String.Empty, drCase.Item("cGender"))
        strvGender = IIf(IsDBNull(drCase.Item("vGender")), String.Empty, drCase.Item("vGender"))
        strcDOB = IIf(IsDBNull(drCase.Item("cDOB")), String.Empty, drCase.Item("cDOB"))
        strvDOB = IIf(IsDBNull(drCase.Item("vDOB")), String.Empty, drCase.Item("vDOB"))
        strcDOBType = IIf(IsDBNull(drCase.Item("cDOBType")), String.Empty, drCase.Item("cDOBType"))
        strvDOBType = IIf(IsDBNull(drCase.Item("vDOBType")), String.Empty, drCase.Item("vDOBType"))
        strcAgeOn = IIf(IsDBNull(drCase.Item("cAgeOn")), String.Empty, drCase.Item("cAgeOn"))
        strvAgeOn = IIf(IsDBNull(drCase.Item("vAgeOn")), String.Empty, drCase.Item("vAgeOn"))
        strcDOR = IIf(IsDBNull(drCase.Item("cDOR")), String.Empty, drCase.Item("cDOR"))
        strvDOR = IIf(IsDBNull(drCase.Item("vDOR")), String.Empty, drCase.Item("vDOR"))
        strcDOBInWord = IIf(IsDBNull(drCase.Item("cDOBInWord")), String.Empty, drCase.Item("cDOBInWord"))
        strvDOBInWord = IIf(IsDBNull(drCase.Item("vDOBInWord")), String.Empty, drCase.Item("vDOBInWord"))
        strcChineseName = IIf(IsDBNull(drCase.Item("cChineseName")), String.Empty, drCase.Item("cChineseName"))
        strvChineseName = IIf(IsDBNull(drCase.Item("vChineseName")), String.Empty, drCase.Item("vChineseName"))
        strcDOI = IIf(IsDBNull(drCase.Item("cDOI")), String.Empty, drCase.Item("cDOI"))
        strvDOI = IIf(IsDBNull(drCase.Item("vDOI")), String.Empty, drCase.Item("vDOI"))
        strcSerialNo = IIf(IsDBNull(drCase.Item("cSerialNo")), String.Empty, drCase.Item("cSerialNo"))
        strvSerialNo = IIf(IsDBNull(drCase.Item("vSerialNo")), String.Empty, drCase.Item("vSerialNo"))
        strcRef = IIf(IsDBNull(drCase.Item("cRef")), String.Empty, drCase.Item("cRef"))
        strvRef = IIf(IsDBNull(drCase.Item("vRef")), String.Empty, drCase.Item("vRef"))
        strcFreeRef = IIf(IsDBNull(drCase.Item("cFreeRef")), String.Empty, drCase.Item("cFreeRef"))
        strvFreeRef = IIf(IsDBNull(drCase.Item("vFreeRef")), String.Empty, drCase.Item("vFreeRef"))
        strcRemainUntil = IIf(IsDBNull(drCase.Item("cRemainUntil")), String.Empty, drCase.Item("cRemainUntil"))
        strvRemainUntil = IIf(IsDBNull(drCase.Item("vRemainUntil")), String.Empty, drCase.Item("vRemainUntil"))
        strcPassportNo = IIf(IsDBNull(drCase.Item("cPassportNo")), String.Empty, drCase.Item("cPassportNo"))
        strvPassportNo = IIf(IsDBNull(drCase.Item("vPassportNo")), String.Empty, drCase.Item("vPassportNo"))
        strcServiceDate = IIf(IsDBNull(drCase.Item("cServiceDate")), String.Empty, drCase.Item("cServiceDate"))
        strvServiceDate = IIf(IsDBNull(drCase.Item("vServiceDate")), String.Empty, drCase.Item("vServiceDate"))

        '----
        strcVoucher1 = IIf(IsDBNull(drCase.Item("cVoucher1")), String.Empty, drCase.Item("cVoucher1"))

        strcVoucherClaimed1 = IIf(IsDBNull(drCase.Item("cVoucherClaimed1")), String.Empty, drCase.Item("cVoucherClaimed1"))
        strvVoucherClaimed1 = IIf(IsDBNull(drCase.Item("vVoucherClaimed1")), String.Empty, drCase.Item("vVoucherClaimed1"))

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        strcCoPaymentFee1 = IIf(IsDBNull(drCase.Item("cCoPaymentFee1")), String.Empty, drCase.Item("cCoPaymentFee1"))
        strvCoPaymentFee1 = IIf(IsDBNull(drCase.Item("vCoPaymentFee1")), String.Empty, drCase.Item("vCoPaymentFee1"))

        strcProfCode1 = IIf(IsDBNull(drCase.Item("cProfCode1")), String.Empty, drCase.Item("cProfCode1"))
        strvProfCode1 = IIf(IsDBNull(drCase.Item("vProfCode1")), String.Empty, drCase.Item("vProfCode1"))

        strcPriorityCode1 = IIf(IsDBNull(drCase.Item("cPriorityCode1")), String.Empty, drCase.Item("cPriorityCode1"))
        strvPriorityCode1 = IIf(IsDBNull(drCase.Item("vPriorityCode1")), String.Empty, drCase.Item("vPriorityCode1"))

        strcL1Code1 = IIf(IsDBNull(drCase.Item("cL1Code1")), String.Empty, drCase.Item("cL1Code1"))
        strvL1Code1 = IIf(IsDBNull(drCase.Item("vL1Code1")), String.Empty, drCase.Item("vL1Code1"))
        strcL1DescEng1 = IIf(IsDBNull(drCase.Item("cL1DescEng1")), String.Empty, drCase.Item("cL1DescEng1"))
        strvL1DescEng1 = IIf(IsDBNull(drCase.Item("vL1DescEng1")), String.Empty, drCase.Item("vL1DescEng1"))
        strcL2Code1 = IIf(IsDBNull(drCase.Item("cL2Code1")), String.Empty, drCase.Item("cL2Code1"))
        strvL2Code1 = IIf(IsDBNull(drCase.Item("vL2Code1")), String.Empty, drCase.Item("vL2Code1"))
        strcL2DescEng1 = IIf(IsDBNull(drCase.Item("cL2DescEng1")), String.Empty, drCase.Item("cL2DescEng1"))
        strvL2DescEng1 = IIf(IsDBNull(drCase.Item("vL2DescEng1")), String.Empty, drCase.Item("vL2DescEng1"))

        strcProfCode1_S1 = IIf(IsDBNull(drCase.Item("cProfCode1_S1")), String.Empty, drCase.Item("cProfCode1_S1"))
        strvProfCode1_S1 = IIf(IsDBNull(drCase.Item("vProfCode1_S1")), String.Empty, drCase.Item("vProfCode1_S1"))
        strcPriorityCode1_S1 = IIf(IsDBNull(drCase.Item("cPriorityCode1_S1")), String.Empty, drCase.Item("cPriorityCode1_S1"))
        strvPriorityCode1_S1 = IIf(IsDBNull(drCase.Item("vPriorityCode1_S1")), String.Empty, drCase.Item("vPriorityCode1_S1"))
        strcL1Code1_S1 = IIf(IsDBNull(drCase.Item("cL1Code1_S1")), String.Empty, drCase.Item("cL1Code1_S1"))
        strvL1Code1_S1 = IIf(IsDBNull(drCase.Item("vL1Code1_S1")), String.Empty, drCase.Item("vL1Code1_S1"))
        strcL1DescEng1_S1 = IIf(IsDBNull(drCase.Item("cL1DescEng1_S1")), String.Empty, drCase.Item("cL1DescEng1_S1"))
        strvL1DescEng1_S1 = IIf(IsDBNull(drCase.Item("vL1DescEng1_S1")), String.Empty, drCase.Item("vL1DescEng1_S1"))
        strcL2Code1_S1 = IIf(IsDBNull(drCase.Item("cL2Code1_S1")), String.Empty, drCase.Item("cL2Code1_S1"))
        strvL2Code1_S1 = IIf(IsDBNull(drCase.Item("vL2Code1_S1")), String.Empty, drCase.Item("vL2Code1_S1"))
        strcL2DescEng1_S1 = IIf(IsDBNull(drCase.Item("cL2DescEng1_S1")), String.Empty, drCase.Item("cL2DescEng1_S1"))
        strvL2DescEng1_S1 = IIf(IsDBNull(drCase.Item("vL2DescEng1_S1")), String.Empty, drCase.Item("vL2DescEng1_S1"))

        strcProfCode1_S2 = IIf(IsDBNull(drCase.Item("cProfCode1_S2")), String.Empty, drCase.Item("cProfCode1_S2"))
        strvProfCode1_S2 = IIf(IsDBNull(drCase.Item("vProfCode1_S2")), String.Empty, drCase.Item("vProfCode1_S2"))
        strcPriorityCode1_S2 = IIf(IsDBNull(drCase.Item("cPriorityCode1_S2")), String.Empty, drCase.Item("cPriorityCode1_S2"))
        strvPriorityCode1_S2 = IIf(IsDBNull(drCase.Item("vPriorityCode1_S2")), String.Empty, drCase.Item("vPriorityCode1_S2"))
        strcL1Code1_S2 = IIf(IsDBNull(drCase.Item("cL1Code1_S2")), String.Empty, drCase.Item("cL1Code1_S2"))
        strvL1Code1_S2 = IIf(IsDBNull(drCase.Item("vL1Code1_S2")), String.Empty, drCase.Item("vL1Code1_S2"))
        strcL1DescEng1_S2 = IIf(IsDBNull(drCase.Item("cL1DescEng1_S2")), String.Empty, drCase.Item("cL1DescEng1_S2"))
        strvL1DescEng1_S2 = IIf(IsDBNull(drCase.Item("vL1DescEng1_S2")), String.Empty, drCase.Item("vL1DescEng1_S2"))
        strcL2Code1_S2 = IIf(IsDBNull(drCase.Item("cL2Code1_S2")), String.Empty, drCase.Item("cL2Code1_S2"))
        strvL2Code1_S2 = IIf(IsDBNull(drCase.Item("vL2Code1_S2")), String.Empty, drCase.Item("vL2Code1_S2"))
        strcL2DescEng1_S2 = IIf(IsDBNull(drCase.Item("cL2DescEng1_S2")), String.Empty, drCase.Item("cL2DescEng1_S2"))
        strvL2DescEng1_S2 = IIf(IsDBNull(drCase.Item("vL2DescEng1_S2")), String.Empty, drCase.Item("vL2DescEng1_S2"))

        strcProfCode1_S3 = IIf(IsDBNull(drCase.Item("cProfCode1_S3")), String.Empty, drCase.Item("cProfCode1_S3"))
        strvProfCode1_S3 = IIf(IsDBNull(drCase.Item("vProfCode1_S3")), String.Empty, drCase.Item("vProfCode1_S3"))
        strcPriorityCode1_S3 = IIf(IsDBNull(drCase.Item("cPriorityCode1_S3")), String.Empty, drCase.Item("cPriorityCode1_S3"))
        strvPriorityCode1_S3 = IIf(IsDBNull(drCase.Item("vPriorityCode1_S3")), String.Empty, drCase.Item("vPriorityCode1_S3"))
        strcL1Code1_S3 = IIf(IsDBNull(drCase.Item("cL1Code1_S3")), String.Empty, drCase.Item("cL1Code1_S3"))
        strvL1Code1_S3 = IIf(IsDBNull(drCase.Item("vL1Code1_S3")), String.Empty, drCase.Item("vL1Code1_S3"))
        strcL1DescEng1_S3 = IIf(IsDBNull(drCase.Item("cL1DescEng1_S3")), String.Empty, drCase.Item("cL1DescEng1_S3"))
        strvL1DescEng1_S3 = IIf(IsDBNull(drCase.Item("vL1DescEng1_S3")), String.Empty, drCase.Item("vL1DescEng1_S3"))
        strcL2Code1_S3 = IIf(IsDBNull(drCase.Item("cL2Code1_S3")), String.Empty, drCase.Item("cL2Code1_S3"))
        strvL2Code1_S3 = IIf(IsDBNull(drCase.Item("vL2Code1_S3")), String.Empty, drCase.Item("vL2Code1_S3"))
        strcL2DescEng1_S3 = IIf(IsDBNull(drCase.Item("cL2DescEng1_S3")), String.Empty, drCase.Item("cL2DescEng1_S3"))
        strvL2DescEng1_S3 = IIf(IsDBNull(drCase.Item("vL2DescEng1_S3")), String.Empty, drCase.Item("vL2DescEng1_S3"))

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        strcPreSchoolInd1 = IIf(IsDBNull(drCase.Item("cPreSchoolInd1")), String.Empty, drCase.Item("cPreSchoolInd1"))
        strvPreSchoolInd1 = IIf(IsDBNull(drCase.Item("vPreSchoolInd1")), String.Empty, drCase.Item("vPreSchoolInd1"))
        strcDoseIntervalInd1 = IIf(IsDBNull(drCase.Item("cDoseIntervalInd1")), String.Empty, drCase.Item("cDoseIntervalInd1"))
        strvDoseIntervalInd1 = IIf(IsDBNull(drCase.Item("vDoseIntervalInd1")), String.Empty, drCase.Item("vDoseIntervalInd1"))
        strcTSWInd1 = IIf(IsDBNull(drCase.Item("cTSWInd1")), String.Empty, drCase.Item("cTSWInd1"))
        strvTSWInd1 = IIf(IsDBNull(drCase.Item("vTSWInd1")), String.Empty, drCase.Item("vTSWInd1"))

        strcVoucher2 = IIf(IsDBNull(drCase.Item("cVoucher2")), String.Empty, drCase.Item("cVoucher2"))
        strcVoucherClaimed2 = IIf(IsDBNull(drCase.Item("cVoucherClaimed2")), String.Empty, drCase.Item("cVoucherClaimed2"))
        strvVoucherClaimed2 = IIf(IsDBNull(drCase.Item("vVoucherClaimed2")), String.Empty, drCase.Item("vVoucherClaimed2"))

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        strcCoPaymentFee2 = IIf(IsDBNull(drCase.Item("cCoPaymentFee2")), String.Empty, drCase.Item("cCoPaymentFee2"))
        strvCoPaymentFee2 = IIf(IsDBNull(drCase.Item("vCoPaymentFee2")), String.Empty, drCase.Item("vCoPaymentFee2"))

        strcProfCode2 = IIf(IsDBNull(drCase.Item("cProfCode2")), String.Empty, drCase.Item("cProfCode2"))
        strvProfCode2 = IIf(IsDBNull(drCase.Item("vProfCode2")), String.Empty, drCase.Item("vProfCode2"))
        strcL1Code2 = IIf(IsDBNull(drCase.Item("cL1Code2")), String.Empty, drCase.Item("cL1Code2"))
        strvL1Code2 = IIf(IsDBNull(drCase.Item("vL1Code2")), String.Empty, drCase.Item("vL1Code2"))
        strcL1DescEng2 = IIf(IsDBNull(drCase.Item("cL1DescEng2")), String.Empty, drCase.Item("cL1DescEng2"))
        strvL1DescEng2 = IIf(IsDBNull(drCase.Item("vL1DescEng2")), String.Empty, drCase.Item("vL1DescEng2"))
        strcL2Code2 = IIf(IsDBNull(drCase.Item("cL2Code2")), String.Empty, drCase.Item("cL2Code2"))
        strvL2Code2 = IIf(IsDBNull(drCase.Item("vL2Code2")), String.Empty, drCase.Item("vL2Code2"))
        strcL2DescEng2 = IIf(IsDBNull(drCase.Item("cL2DescEng2")), String.Empty, drCase.Item("cL2DescEng2"))
        strvL2DescEng2 = IIf(IsDBNull(drCase.Item("vL2DescEng2")), String.Empty, drCase.Item("vL2DescEng2"))

        strcProfCode2_S1 = IIf(IsDBNull(drCase.Item("cProfCode2_S1")), String.Empty, drCase.Item("cProfCode2_S1"))
        strvProfCode2_S1 = IIf(IsDBNull(drCase.Item("vProfCode2_S1")), String.Empty, drCase.Item("vProfCode2_S1"))
        strcPriorityCode2_S1 = IIf(IsDBNull(drCase.Item("cPriorityCode2_S1")), String.Empty, drCase.Item("cPriorityCode2_S1"))
        strvPriorityCode2_S1 = IIf(IsDBNull(drCase.Item("vPriorityCode2_S1")), String.Empty, drCase.Item("vPriorityCode2_S1"))
        strcL1Code2_S1 = IIf(IsDBNull(drCase.Item("cL1Code2_S1")), String.Empty, drCase.Item("cL1Code2_S1"))
        strvL1Code2_S1 = IIf(IsDBNull(drCase.Item("vL1Code2_S1")), String.Empty, drCase.Item("vL1Code2_S1"))
        strcL1DescEng2_S1 = IIf(IsDBNull(drCase.Item("cL1DescEng2_S1")), String.Empty, drCase.Item("cL1DescEng2_S1"))
        strvL1DescEng2_S1 = IIf(IsDBNull(drCase.Item("vL1DescEng2_S1")), String.Empty, drCase.Item("vL1DescEng2_S1"))
        strcL2Code2_S1 = IIf(IsDBNull(drCase.Item("cL2Code2_S1")), String.Empty, drCase.Item("cL2Code2_S1"))
        strvL2Code2_S1 = IIf(IsDBNull(drCase.Item("vL2Code2_S1")), String.Empty, drCase.Item("vL2Code2_S1"))
        strcL2DescEng2_S1 = IIf(IsDBNull(drCase.Item("cL2DescEng2_S1")), String.Empty, drCase.Item("cL2DescEng2_S1"))
        strvL2DescEng2_S1 = IIf(IsDBNull(drCase.Item("vL2DescEng2_S1")), String.Empty, drCase.Item("vL2DescEng2_S1"))

        strcProfCode2_S2 = IIf(IsDBNull(drCase.Item("cProfCode2_S2")), String.Empty, drCase.Item("cProfCode2_S2"))
        strvProfCode2_S2 = IIf(IsDBNull(drCase.Item("vProfCode2_S2")), String.Empty, drCase.Item("vProfCode2_S2"))
        strcPriorityCode2_S2 = IIf(IsDBNull(drCase.Item("cPriorityCode2_S2")), String.Empty, drCase.Item("cPriorityCode2_S2"))
        strvPriorityCode2_S2 = IIf(IsDBNull(drCase.Item("vPriorityCode2_S2")), String.Empty, drCase.Item("vPriorityCode2_S2"))
        strcL1Code2_S2 = IIf(IsDBNull(drCase.Item("cL1Code2_S2")), String.Empty, drCase.Item("cL1Code2_S2"))
        strvL1Code2_S2 = IIf(IsDBNull(drCase.Item("vL1Code2_S2")), String.Empty, drCase.Item("vL1Code2_S2"))
        strcL1DescEng2_S2 = IIf(IsDBNull(drCase.Item("cL1DescEng2_S2")), String.Empty, drCase.Item("cL1DescEng2_S2"))
        strvL1DescEng2_S2 = IIf(IsDBNull(drCase.Item("vL1DescEng2_S2")), String.Empty, drCase.Item("vL1DescEng2_S2"))
        strcL2Code2_S2 = IIf(IsDBNull(drCase.Item("cL2Code2_S2")), String.Empty, drCase.Item("cL2Code2_S2"))
        strvL2Code2_S2 = IIf(IsDBNull(drCase.Item("vL2Code2_S2")), String.Empty, drCase.Item("vL2Code2_S2"))
        strcL2DescEng2_S2 = IIf(IsDBNull(drCase.Item("cL2DescEng2_S2")), String.Empty, drCase.Item("cL2DescEng2_S2"))
        strvL2DescEng2_S2 = IIf(IsDBNull(drCase.Item("vL2DescEng2_S2")), String.Empty, drCase.Item("vL2DescEng2_S2"))

        strcProfCode2_S3 = IIf(IsDBNull(drCase.Item("cProfCode2_S3")), String.Empty, drCase.Item("cProfCode2_S3"))
        strvProfCode2_S3 = IIf(IsDBNull(drCase.Item("vProfCode2_S3")), String.Empty, drCase.Item("vProfCode2_S3"))
        strcPriorityCode2_S3 = IIf(IsDBNull(drCase.Item("cPriorityCode2_S3")), String.Empty, drCase.Item("cPriorityCode2_S3"))
        strvPriorityCode2_S3 = IIf(IsDBNull(drCase.Item("vPriorityCode2_S3")), String.Empty, drCase.Item("vPriorityCode2_S3"))
        strcL1Code2_S3 = IIf(IsDBNull(drCase.Item("cL1Code2_S3")), String.Empty, drCase.Item("cL1Code2_S3"))
        strvL1Code2_S3 = IIf(IsDBNull(drCase.Item("vL1Code2_S3")), String.Empty, drCase.Item("vL1Code2_S3"))
        strcL1DescEng2_S3 = IIf(IsDBNull(drCase.Item("cL1DescEng2_S3")), String.Empty, drCase.Item("cL1DescEng2_S3"))
        strvL1DescEng2_S3 = IIf(IsDBNull(drCase.Item("vL1DescEng2_S3")), String.Empty, drCase.Item("vL1DescEng2_S3"))
        strcL2Code2_S3 = IIf(IsDBNull(drCase.Item("cL2Code2_S3")), String.Empty, drCase.Item("cL2Code2_S3"))
        strvL2Code2_S3 = IIf(IsDBNull(drCase.Item("vL2Code2_S3")), String.Empty, drCase.Item("vL2Code2_S3"))
        strcL2DescEng2_S3 = IIf(IsDBNull(drCase.Item("cL2DescEng2_S3")), String.Empty, drCase.Item("cL2DescEng2_S3"))
        strvL2DescEng2_S3 = IIf(IsDBNull(drCase.Item("vL2DescEng2_S3")), String.Empty, drCase.Item("vL2DescEng2_S3"))

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        strcPreSchoolInd2 = IIf(IsDBNull(drCase.Item("cPreSchoolInd2")), String.Empty, drCase.Item("cPreSchoolInd2"))
        strvPreSchoolInd2 = IIf(IsDBNull(drCase.Item("vPreSchoolInd2")), String.Empty, drCase.Item("vPreSchoolInd2"))
        strcDoseIntervalInd2 = IIf(IsDBNull(drCase.Item("cDoseIntervalInd2")), String.Empty, drCase.Item("cDoseIntervalInd2"))
        strvDoseIntervalInd2 = IIf(IsDBNull(drCase.Item("vDoseIntervalInd2")), String.Empty, drCase.Item("vDoseIntervalInd2"))
        strcTSWInd2 = IIf(IsDBNull(drCase.Item("cTSWInd2")), String.Empty, drCase.Item("cTSWInd2"))
        strvTSWInd2 = IIf(IsDBNull(drCase.Item("vTSWInd2")), String.Empty, drCase.Item("vTSWInd2"))

        strSchemeCode1 = IIf(IsDBNull(drCase.Item("SchemeCode1")), String.Empty, drCase.Item("SchemeCode1"))
        strcRCHCode1 = IIf(IsDBNull(drCase.Item("cRCHCode1")), String.Empty, drCase.Item("cRCHCode1"))
        strvRCHCode1 = IIf(IsDBNull(drCase.Item("vRCHCode1")), String.Empty, drCase.Item("vRCHCode1"))
        strcVaccine11 = IIf(IsDBNull(drCase.Item("cVaccine11")), String.Empty, drCase.Item("cVaccine11"))
        strcSubsidyCode11 = IIf(IsDBNull(drCase.Item("cSubsidyCode11")), String.Empty, drCase.Item("cSubsidyCode11"))
        strvSubsidyCode11 = IIf(IsDBNull(drCase.Item("vSubsidyCode11")), String.Empty, drCase.Item("vSubsidyCode11"))
        strcDoseSeq11 = IIf(IsDBNull(drCase.Item("cDoseSeq11")), String.Empty, drCase.Item("cDoseSeq11"))
        strvDoseSeq11 = IIf(IsDBNull(drCase.Item("vDoseSeq11")), String.Empty, drCase.Item("vDoseSeq11"))
        strcVaccine12 = IIf(IsDBNull(drCase.Item("cVaccine12")), String.Empty, drCase.Item("cVaccine12"))
        strcSubsidyCode12 = IIf(IsDBNull(drCase.Item("cSubsidyCode12")), String.Empty, drCase.Item("cSubsidyCode12"))
        strvSubsidyCode12 = IIf(IsDBNull(drCase.Item("vSubsidyCode12")), String.Empty, drCase.Item("vSubsidyCode12"))
        strcDoseSeq12 = IIf(IsDBNull(drCase.Item("cDoseSeq12")), String.Empty, drCase.Item("cDoseSeq12"))
        strvDoseSeq12 = IIf(IsDBNull(drCase.Item("vDoseSeq12")), String.Empty, drCase.Item("vDoseSeq12"))
        strcPreSchoolIndv1 = IIf(IsDBNull(drCase.Item("cPreSchoolIndv1")), String.Empty, drCase.Item("cPreSchoolIndv1"))
        strvPreSchoolIndv1 = IIf(IsDBNull(drCase.Item("vPreSchoolIndv1")), String.Empty, drCase.Item("vPreSchoolIndv1"))
        strcDoseIntervalIndv1 = IIf(IsDBNull(drCase.Item("cDoseIntervalIndv1")), String.Empty, drCase.Item("cDoseIntervalIndv1"))
        strvDoseIntervalIndv1 = IIf(IsDBNull(drCase.Item("vDoseIntervalIndv1")), String.Empty, drCase.Item("vDoseIntervalIndv1"))
        strcTSWIndv1 = IIf(IsDBNull(drCase.Item("cTSWIndv1")), String.Empty, drCase.Item("cTSWIndv1"))
        strvTSWIndv1 = IIf(IsDBNull(drCase.Item("vTSWIndv1")), String.Empty, drCase.Item("vTSWIndv1"))

        strSchemeCode2 = IIf(IsDBNull(drCase.Item("SchemeCode2")), String.Empty, drCase.Item("SchemeCode2"))
        strcRCHCode2 = IIf(IsDBNull(drCase.Item("cRCHCode2")), String.Empty, drCase.Item("cRCHCode2"))
        strvRCHCode2 = IIf(IsDBNull(drCase.Item("vRCHCode2")), String.Empty, drCase.Item("vRCHCode2"))
        strcVaccine21 = IIf(IsDBNull(drCase.Item("cVaccine21")), String.Empty, drCase.Item("cVaccine21"))
        strcSubsidyCode21 = IIf(IsDBNull(drCase.Item("cSubsidyCode21")), String.Empty, drCase.Item("cSubsidyCode21"))
        strvSubsidyCode21 = IIf(IsDBNull(drCase.Item("vSubsidyCode21")), String.Empty, drCase.Item("vSubsidyCode21"))
        strcDoseSeq21 = IIf(IsDBNull(drCase.Item("cDoseSeq21")), String.Empty, drCase.Item("cDoseSeq21"))
        strvDoseSeq21 = IIf(IsDBNull(drCase.Item("vDoseSeq21")), String.Empty, drCase.Item("vDoseSeq21"))
        strcVaccine22 = IIf(IsDBNull(drCase.Item("cVaccine22")), String.Empty, drCase.Item("cVaccine22"))
        strcSubsidyCode22 = IIf(IsDBNull(drCase.Item("cSubsidyCode22")), String.Empty, drCase.Item("cSubsidyCode22"))
        strvSubsidyCode22 = IIf(IsDBNull(drCase.Item("vSubsidyCode22")), String.Empty, drCase.Item("vSubsidyCode22"))
        strcDoseSeq22 = IIf(IsDBNull(drCase.Item("cDoseSeq22")), String.Empty, drCase.Item("cDoseSeq22"))
        strvDoseSeq22 = IIf(IsDBNull(drCase.Item("vDoseSeq22")), String.Empty, drCase.Item("vDoseSeq22"))
        strcPreSchoolIndv2 = IIf(IsDBNull(drCase.Item("cPreSchoolIndv2")), String.Empty, drCase.Item("cPreSchoolIndv2"))
        strvPreSchoolIndv2 = IIf(IsDBNull(drCase.Item("vPreSchoolIndv2")), String.Empty, drCase.Item("vPreSchoolIndv2"))
        strcDoseIntervalIndv2 = IIf(IsDBNull(drCase.Item("cDoseIntervalIndv2")), String.Empty, drCase.Item("cDoseIntervalIndv2"))
        strvDoseIntervalIndv2 = IIf(IsDBNull(drCase.Item("vDoseIntervalIndv2")), String.Empty, drCase.Item("vDoseIntervalIndv2"))
        strcTSWIndv2 = IIf(IsDBNull(drCase.Item("cTSWIndv2")), String.Empty, drCase.Item("cTSWIndv2"))
        strvTSWIndv2 = IIf(IsDBNull(drCase.Item("vTSWIndv2")), String.Empty, drCase.Item("vTSWIndv2"))

        chkSPInfo.BackColor = Drawing.Color.Transparent
        chkAccountInfo.BackColor = Drawing.Color.Transparent
        chkClaimInfo.BackColor = Drawing.Color.Transparent
        If strFunction.Trim <> "" Then
            Select Case strFunction.Trim
                Case "1"
                    rboSPPracticeValidation.Checked = True
                    rboSPPracticeValidation_CheckedChanged(Me, New EventArgs)
                Case "2"
                    rboGetReasonForVisit.Checked = True
                    rboGetReasonForVisit_CheckedChanged(Me, New EventArgs)
                Case "3"
                    rboRCHNameQuery.Checked = True
                    rboRCHNameQuery_CheckedChanged(Me, New EventArgs)
                Case "4"
                    rboEHSValidatedAccountQuery.Checked = True
                    rboEHSValidatedAccountQuery_CheckedChanged(Me, New EventArgs)
                Case "5"
                    rboEHSAccountSubsidyQuery.Checked = True
                    rboEHSAccountSubsidyQuery_CheckedChanged(Me, New EventArgs)
                Case "6"
                    rboUploadClaimHL7.Checked = True
                    rboUploadClaimHL7_CheckedChanged(Me, New EventArgs)
            End Select
        End If
        If strSPInfo.Trim.ToUpper = "Y" Then
            pnlSPInfo.Visible = True
            chkSPInfo.Checked = True
        Else
            pnlSPInfo.Visible = False
            chkSPInfo.Checked = False
        End If
        If strAccInfo.Trim.ToUpper = "Y" Then
            pnlAccountInfo.Visible = True
            chkAccountInfo.Checked = True
            lblTip1.Visible = True
        Else
            pnlAccountInfo.Visible = False
            chkAccountInfo.Checked = False
            lblTip1.Visible = False
        End If
        If strClaimInfo.Trim.ToUpper = "Y" Then
            pnlClaimInfo.Visible = True
            chkClaimInfo.Checked = True
            lblTip2.Visible = True
        Else
            pnlClaimInfo.Visible = False
            chkClaimInfo.Checked = False
            lblTip2.Visible = False
        End If
        'SP ID
        If strcSPID.Trim.ToUpper = "Y" Then
            chkSPID.Checked = True
            txtSPID.Enabled = True
            txtSPID.Text = strvSPID
            txtSPID.BackColor = Drawing.Color.White
        Else
            chkSPID.Checked = False
            txtSPID.Enabled = False
            txtSPID.Text = ""
            txtSPID.BackColor = Drawing.Color.Silver
        End If
        'SP Surname
        If strcSPSurname.Trim.ToUpper = "Y" Then
            chkSPSurname.Checked = True
            txtSPSurname.Enabled = True
            txtSPSurname.Text = strvSPSurname
            txtSPSurname.BackColor = Drawing.Color.White
        Else
            chkSPSurname.Checked = False
            txtSPSurname.Enabled = False
            txtSPSurname.Text = ""
            txtSPSurname.BackColor = Drawing.Color.Silver
        End If
        'SP Given Name
        If strcSPGivenName.Trim.ToUpper = "Y" Then
            chkSPGivenname.Checked = True
            txtSPGivenName.Enabled = True
            txtSPGivenName.Text = strvSPGivenName
            txtSPGivenName.BackColor = Drawing.Color.White
        Else
            chkSPGivenname.Checked = False
            txtSPGivenName.Enabled = False
            txtSPGivenName.Text = ""
            txtSPGivenName.BackColor = Drawing.Color.Silver
        End If
        'Practice ID
        If strcPracticeID.Trim.ToUpper = "Y" Then
            chkPracticeID.Checked = True
            txtPracticeID.Enabled = True
            txtPracticeID.Text = strvPracticeID
            txtPracticeID.BackColor = Drawing.Color.White
        Else
            chkPracticeID.Checked = False
            txtPracticeID.Enabled = False
            txtPracticeID.Text = ""
            txtPracticeID.BackColor = Drawing.Color.Silver
        End If
        'Practice Name
        If strcPracticeName.Trim.ToUpper = "Y" Then
            chkPracticeName.Checked = True
            txtPracticeName.Enabled = True
            txtPracticeName.Text = strvPracticeName
            txtPracticeName.BackColor = Drawing.Color.White
        Else
            chkPracticeName.Checked = False
            txtPracticeName.Enabled = False
            txtPracticeName.Text = ""
            txtPracticeName.BackColor = Drawing.Color.Silver
        End If

        'RCH Code
        If strcRCHCode.Trim.ToUpper = "Y" Then
            chkRCHCode.Visible = True
            chkRCHCode.Checked = True
            txtRCHCode.Enabled = True
            txtRCHCode.Visible = True
            txtRCHCode.Text = strvRCHCode
            txtRCHCode.BackColor = Drawing.Color.White
        Else
            chkRCHCode.Visible = False
            txtRCHCode.Visible = False
            chkRCHCode.Checked = False
            txtRCHCode.Enabled = False
            txtRCHCode.Text = ""
            txtRCHCode.BackColor = Drawing.Color.Silver
        End If
        'Doc Type
        Select Case strDocType.Trim
            Case "HKIC"
                rboHKIC.Checked = True
                rboHKIC_CheckedChanged(Me, New EventArgs)
            Case "EC"
                rboEC.Checked = True
                rboEC_CheckedChanged(Me, New EventArgs)
            Case "HKBC"
                rboHKBC.Checked = True
                rboHKBC_CheckedChanged(Me, New EventArgs)
            Case "ADOPC"
                rboADOPC.Checked = True
                rboADOPC_CheckedChanged(Me, New EventArgs)
            Case "REPMT"
                rboREPMT.Checked = True
                rboREPMT_CheckedChanged(Me, New EventArgs)
            Case "Doc/I", "DOCI"
                rboDOCI.Checked = True
                rboDOCI_CheckedChanged(Me, New EventArgs)
            Case "VISA"
                rboVISA.Checked = True
                rboVISA_CheckedChanged(Me, New EventArgs)
            Case "ID235B"
                rboID235B.Checked = True
                rboID235B_CheckedChanged(Me, New EventArgs)
        End Select
        'Entry No
        If strcEntryNo.Trim.ToUpper = "Y" Then
            chkEntryNo.Checked = True
            txtEntryNo.Enabled = True
            txtEntryNo.Text = strvEntryNo
            txtEntryNo.BackColor = Drawing.Color.White
        Else
            chkEntryNo.Checked = False
            txtEntryNo.Enabled = False
            txtEntryNo.Text = ""
            txtEntryNo.BackColor = Drawing.Color.Silver
        End If
        'Doc No
        If strcDocNo.Trim.ToUpper = "Y" Then
            chkDocumentNo.Checked = True
            txtDocumentNo.Enabled = True
            txtDocumentNo.Text = strvDocNo
            txtDocumentNo.BackColor = Drawing.Color.White
        Else
            chkDocumentNo.Checked = False
            txtDocumentNo.Enabled = False
            txtDocumentNo.Text = ""
            txtDocumentNo.BackColor = Drawing.Color.Silver
        End If
        'HKIC
        If strcHKIC.Trim.ToUpper = "Y" Then
            chkHKIC.Checked = True
            txtHKIC.Enabled = True
            txtHKIC.Text = strvHKIC
            txtHKIC.BackColor = Drawing.Color.White
        Else
            chkHKIC.Checked = False
            txtHKIC.Enabled = False
            txtHKIC.Text = ""
            txtHKIC.BackColor = Drawing.Color.Silver
        End If
        'RegNo
        If strcRegNo.Trim.ToUpper = "Y" Then
            chkRegNo.Checked = True
            txtRegNo.Enabled = True
            txtRegNo.Text = strvRegNo
            txtRegNo.BackColor = Drawing.Color.White
        Else
            chkRegNo.Checked = False
            txtRegNo.Enabled = False
            txtRegNo.Text = ""
            txtRegNo.BackColor = Drawing.Color.Silver
        End If
        'RegNo
        If strcBirthEntryNo.Trim.ToUpper = "Y" Then
            chkBirthEntryNo.Checked = True
            txtBirthEntryNo.Enabled = True
            txtBirthEntryNo.Text = strvBirthEntryNo
            txtBirthEntryNo.BackColor = Drawing.Color.White
        Else
            chkBirthEntryNo.Checked = False
            txtBirthEntryNo.Enabled = False
            txtBirthEntryNo.Text = ""
            txtBirthEntryNo.BackColor = Drawing.Color.Silver
        End If
        'Reg No
        If strcPermitNo.Trim.ToUpper = "Y" Then
            chkPermitNo.Checked = True
            txtPermitNo.Enabled = True
            txtPermitNo.Text = strvPermitNo
            txtPermitNo.BackColor = Drawing.Color.White
        Else
            chkPermitNo.Checked = False
            txtPermitNo.Enabled = False
            txtPermitNo.Text = ""
            txtPermitNo.BackColor = Drawing.Color.Silver
        End If
        'VISA No
        If strcVISANo.Trim.ToUpper = "Y" Then
            chkVISANo.Checked = True
            txtVISANo.Enabled = True
            txtVISANo.Text = strvVISANo
            txtVISANo.BackColor = Drawing.Color.White
        Else
            chkVISANo.Checked = False
            txtVISANo.Enabled = False
            txtVISANo.Text = ""
            txtVISANo.BackColor = Drawing.Color.Silver
        End If
        'Surname
        If strcSurname.Trim.ToUpper = "Y" Then
            chkSurname.Checked = True
            txtSurname.Enabled = True
            txtSurname.Text = strvSurname
            txtSurname.BackColor = Drawing.Color.White
        Else
            chkSurname.Checked = False
            txtSurname.Enabled = False
            txtSurname.Text = ""
            txtSurname.BackColor = Drawing.Color.Silver
        End If
        'Given Name
        If strcGivenName.Trim.ToUpper = "Y" Then
            chkGivenName.Checked = True
            txtGivenName.Enabled = True
            txtGivenName.Text = strvGivenName
            txtGivenName.BackColor = Drawing.Color.White
        Else
            chkGivenName.Checked = False
            txtGivenName.Enabled = False
            txtGivenName.Text = ""
            txtGivenName.BackColor = Drawing.Color.Silver
        End If
        'Gender
        If strcGender.Trim.ToUpper = "Y" Then
            chkGender.Checked = True
            txtGender.Enabled = True
            txtGender.Text = strvGender
            txtGender.BackColor = Drawing.Color.White
        Else
            chkGender.Checked = False
            txtGender.Enabled = False
            txtGender.Text = ""
            txtGender.BackColor = Drawing.Color.Silver
        End If
        'DOB
        If strcDOB.Trim.ToUpper = "Y" Then
            chkDOB.Checked = True
            txtDOB.Enabled = True
            txtDOB.Text = strvDOB
            txtDOB.BackColor = Drawing.Color.White
        Else
            chkDOB.Checked = False
            txtDOB.Enabled = False
            txtDOB.Text = ""
            txtDOB.BackColor = Drawing.Color.Silver
        End If
        'DOB Type
        If strcDOBType.Trim.ToUpper = "Y" Then
            chkDOBType.Checked = True
            txtDOBType.Enabled = True
            txtDOBType.Text = strvDOBType
            txtDOBType.BackColor = Drawing.Color.White
        Else
            chkDOBType.Checked = False
            txtDOBType.Enabled = False
            txtDOBType.Text = ""
            txtDOBType.BackColor = Drawing.Color.Silver
        End If
        'Age On
        If strcAgeOn.Trim.ToUpper = "Y" Then
            chkAgeOn.Checked = True
            txtAgeOn.Enabled = True
            txtAgeOn.Text = strvAgeOn
            txtAgeOn.BackColor = Drawing.Color.White
        Else
            chkAgeOn.Checked = False
            txtAgeOn.Enabled = False
            txtAgeOn.Text = ""
            txtAgeOn.BackColor = Drawing.Color.Silver
        End If
        'DOR
        If strcDOR.Trim.ToUpper = "Y" Then
            chkDOReg.Checked = True
            txtDOReg.Enabled = True
            txtDOReg.Text = strvDOR
            txtDOReg.BackColor = Drawing.Color.White
        Else
            chkDOReg.Checked = False
            txtDOReg.Enabled = False
            txtDOReg.Text = ""
            txtDOReg.BackColor = Drawing.Color.Silver
        End If
        'DOBInWord
        If strcDOBInWord.Trim.ToUpper = "Y" Then
            chkDOBInWord.Checked = True
            txtDOBInWord.Enabled = True
            txtDOBInWord.Text = strvDOBInWord
            txtDOBInWord.BackColor = Drawing.Color.White
        Else
            chkDOBInWord.Checked = False
            txtDOBInWord.Enabled = False
            txtDOBInWord.Text = ""
            txtDOBInWord.BackColor = Drawing.Color.Silver
        End If
        'Chinese Name
        If strcChineseName.Trim.ToUpper = "Y" Then
            chkNameChi.Checked = True
            txtNameChi.Enabled = True
            txtNameChi.Text = strvChineseName
            txtNameChi.BackColor = Drawing.Color.White
        Else
            chkNameChi.Checked = False
            txtNameChi.Enabled = False
            txtNameChi.Text = ""
            txtNameChi.BackColor = Drawing.Color.Silver
        End If
        'DOI
        If strcDOI.Trim.ToUpper = "Y" Then
            chkDOI.Checked = True
            txtDOI.Enabled = True
            txtDOI.Text = strvDOI
            txtDOI.BackColor = Drawing.Color.White
        Else
            chkDOI.Checked = False
            txtDOI.Enabled = False
            txtDOI.Text = ""
            txtDOI.BackColor = Drawing.Color.Silver
        End If
        'Serial No
        If strcSerialNo.Trim.ToUpper = "Y" Then
            chkSerialNo.Checked = True
            txtSerialNo.Enabled = True
            txtSerialNo.Text = strvSerialNo
            txtSerialNo.BackColor = Drawing.Color.White
        Else
            chkSerialNo.Checked = False
            txtSerialNo.Enabled = False
            txtSerialNo.Text = ""
            txtSerialNo.BackColor = Drawing.Color.Silver
        End If
        'Ref
        If strcRef.Trim.ToUpper = "Y" Then
            chkReference.Checked = True
            txtReference.Enabled = True
            txtReference.Text = strvRef
            txtReference.BackColor = Drawing.Color.White
        Else
            chkReference.Checked = False
            txtReference.Enabled = False
            txtReference.Text = ""
            txtReference.BackColor = Drawing.Color.Silver
        End If
        'Free Ref
        If strcFreeRef.Trim.ToUpper = "Y" Then
            chkFreeReference.Checked = True
            txtFreeReference.Enabled = True
            txtFreeReference.Text = strvFreeRef
            txtFreeReference.BackColor = Drawing.Color.White
        Else
            chkFreeReference.Checked = False
            txtFreeReference.Enabled = False
            txtFreeReference.Text = ""
            txtFreeReference.BackColor = Drawing.Color.Silver
        End If
        'Remain Until
        If strcRemainUntil.Trim.ToUpper = "Y" Then
            chkRemainUntil.Checked = True
            txtRemainUntil.Enabled = True
            txtRemainUntil.Text = strvRemainUntil
            txtRemainUntil.BackColor = Drawing.Color.White
        Else
            chkRemainUntil.Checked = False
            txtRemainUntil.Enabled = False
            txtRemainUntil.Text = ""
            txtRemainUntil.BackColor = Drawing.Color.Silver
        End If
        'PassportNo
        If strcPassportNo.Trim.ToUpper = "Y" Then
            chkPassportNo.Checked = True
            txtPassportNo.Enabled = True
            txtPassportNo.Text = strvPassportNo
            txtPassportNo.BackColor = Drawing.Color.White
        Else
            chkPassportNo.Checked = False
            txtPassportNo.Enabled = False
            txtPassportNo.Text = ""
            txtPassportNo.BackColor = Drawing.Color.Silver
        End If

        'Service Date
        If strcServiceDate.Trim.ToUpper = "Y" Then
            chkServiceDate.Checked = True
            txtServiceDate.Text = strvServiceDate
        Else
            chkServiceDate.Checked = False
            txtServiceDate.Text = ""
        End If

        'Voucher 1 -------------------------------------------------
        If strcVoucher1.Trim.ToUpper = "Y" Then
            chkHCVS.Checked = True
        Else
            chkHCVS.Checked = False
        End If
        'Voucher Claimed 1
        If strcVoucherClaimed1.Trim.ToUpper = "Y" Then
            chkVoucherClaimed.Checked = True
            txtVoucherClaimed.Text = strvVoucherClaimed1
        Else
            chkVoucherClaimed.Checked = False
            txtVoucherClaimed.Text = ""
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        'Co-Payment Fee 1
        If strcCoPaymentFee1.Trim.ToUpper = "Y" Then
            chkCoPaymentFee.Checked = True
            txtCoPaymentFee.Text = strvCoPaymentFee1
        Else
            chkCoPaymentFee.Checked = False
            txtCoPaymentFee.Text = ""
        End If

        'Prof Code 1
        If strcProfCode1.Trim.ToUpper = "Y" Then
            chkProfCode.Checked = True
            txtProfCode.Text = strvProfCode1
        Else
            chkProfCode.Checked = False
            txtProfCode.Text = ""
        End If

        'Priority Code 1
        If strcPriorityCode1.Trim.ToUpper = "Y" Then
            chkPriorityCode.Checked = True
            txtPriorityCode.Text = strvPriorityCode1
        Else
            chkPriorityCode.Checked = False
            txtPriorityCode.Text = ""
        End If

        'L1 Code 1
        If strcL1Code1.Trim.ToUpper = "Y" Then
            chkL1Code.Checked = True
            txtL1Code.Text = strvL1Code1
        Else
            chkL1Code.Checked = False
            txtL1Code.Text = ""
        End If
        'L1 Desc Eng 1
        If strcL1DescEng1.Trim.ToUpper = "Y" Then
            chkL1DescEng.Checked = True
            txtL1DescEng.Text = strvL1DescEng1
        Else
            chkL1DescEng.Checked = False
            txtL1DescEng.Text = ""
        End If
        'L2 Code 1
        If strcL2Code1.Trim.ToUpper = "Y" Then
            chkL2Code.Checked = True
            txtL2Code.Text = strvL2Code1
        Else
            chkL2Code.Checked = False
            txtL2Code.Text = ""
        End If
        'L2 Desc Eng 1
        If strcL2DescEng1.Trim.ToUpper = "Y" Then
            chkL2DescEng.Checked = True
            txtL2DescEng.Text = strvL2DescEng1
        Else
            chkL2DescEng.Checked = False
            txtL2DescEng.Text = ""
        End If

        'S1

        'Prof Code 1
        If strcProfCode1_S1.Trim.ToUpper = "Y" Then
            chkProfCode_S1.Checked = True
            txtProfCode_S1.Text = strvProfCode1_S1
        Else
            chkProfCode_S1.Checked = False
            txtProfCode_S1.Text = ""
        End If

        'Priority Code 1
        If strcPriorityCode1_S1.Trim.ToUpper = "Y" Then
            chkPriorityCode_S1.Checked = True
            txtPriorityCode_S1.Text = strvPriorityCode1_S1
        Else
            chkPriorityCode_S1.Checked = False
            txtPriorityCode_S1.Text = ""
        End If

        'L1 Code 1
        If strcL1Code1_S1.Trim.ToUpper = "Y" Then
            chkL1Code_S1.Checked = True
            txtL1Code_S1.Text = strvL1Code1_S1
        Else
            chkL1Code_S1.Checked = False
            txtL1Code_S1.Text = ""
        End If
        'L1 Desc Eng 1
        If strcL1DescEng1_S1.Trim.ToUpper = "Y" Then
            chkL1DescEng_S1.Checked = True
            txtL1DescEng_S1.Text = strvL1DescEng1_S1
        Else
            chkL1DescEng_S1.Checked = False
            txtL1DescEng_S1.Text = ""
        End If
        'L2 Code 1
        If strcL2Code1_S1.Trim.ToUpper = "Y" Then
            chkL2Code_S1.Checked = True
            txtL2Code_S1.Text = strvL2Code1_S1
        Else
            chkL2Code_S1.Checked = False
            txtL2Code_S1.Text = ""
        End If
        'L2 Desc Eng 1
        If strcL2DescEng1_S1.Trim.ToUpper = "Y" Then
            chkL2DescEng_S1.Checked = True
            txtL2DescEng_S1.Text = strvL2DescEng1_S1
        Else
            chkL2DescEng_S1.Checked = False
            txtL2DescEng_S1.Text = ""
        End If

        'S2

        'Prof Code 1
        If strcProfCode1_S2.Trim.ToUpper = "Y" Then
            chkProfCode_S2.Checked = True
            txtProfCode_S2.Text = strvProfCode1_S2
        Else
            chkProfCode_S2.Checked = False
            txtProfCode_S2.Text = ""
        End If

        'Priority Code 1
        If strcPriorityCode1_S2.Trim.ToUpper = "Y" Then
            chkPriorityCode_S2.Checked = True
            txtPriorityCode_S2.Text = strvPriorityCode1_S2
        Else
            chkPriorityCode_S2.Checked = False
            txtPriorityCode_S2.Text = ""
        End If

        'L1 Code 1
        If strcL1Code1_S2.Trim.ToUpper = "Y" Then
            chkL1Code_S2.Checked = True
            txtL1Code_S2.Text = strvL1Code1_S2
        Else
            chkL1Code_S2.Checked = False
            txtL1Code_S2.Text = ""
        End If
        'L1 Desc Eng 1
        If strcL1DescEng1_S2.Trim.ToUpper = "Y" Then
            chkL1DescEng_S2.Checked = True
            txtL1DescEng_S2.Text = strvL1DescEng1_S2
        Else
            chkL1DescEng_S2.Checked = False
            txtL1DescEng_S2.Text = ""
        End If
        'L2 Code 1
        If strcL2Code1_S2.Trim.ToUpper = "Y" Then
            chkL2Code_S2.Checked = True
            txtL2Code_S2.Text = strvL2Code1_S2
        Else
            chkL2Code_S2.Checked = False
            txtL2Code_S2.Text = ""
        End If
        'L2 Desc Eng 1
        If strcL2DescEng1_S2.Trim.ToUpper = "Y" Then
            chkL2DescEng_S2.Checked = True
            txtL2DescEng_S2.Text = strvL2DescEng1_S2
        Else
            chkL2DescEng_S2.Checked = False
            txtL2DescEng_S2.Text = ""
        End If

        'S3

        'Prof Code 1
        If strcProfCode1_S3.Trim.ToUpper = "Y" Then
            chkProfCode_S3.Checked = True
            txtProfCode_S3.Text = strvProfCode1_S3
        Else
            chkProfCode_S3.Checked = False
            txtProfCode_S3.Text = ""
        End If

        'Priority Code 1
        If strcPriorityCode1_S3.Trim.ToUpper = "Y" Then
            chkPriorityCode_S3.Checked = True
            txtPriorityCode_S3.Text = strvPriorityCode1_S3
        Else
            chkPriorityCode_S3.Checked = False
            txtPriorityCode_S3.Text = ""
        End If

        'L1 Code 1
        If strcL1Code1_S3.Trim.ToUpper = "Y" Then
            chkL1Code_S3.Checked = True
            txtL1Code_S3.Text = strvL1Code1_S3
        Else
            chkL1Code_S3.Checked = False
            txtL1Code_S3.Text = ""
        End If
        'L1 Desc Eng 1
        If strcL1DescEng1_S3.Trim.ToUpper = "Y" Then
            chkL1DescEng_S3.Checked = True
            txtL1DescEng_S3.Text = strvL1DescEng1_S3
        Else
            chkL1DescEng_S3.Checked = False
            txtL1DescEng_S3.Text = ""
        End If
        'L2 Code 1
        If strcL2Code1_S3.Trim.ToUpper = "Y" Then
            chkL2Code_S3.Checked = True
            txtL2Code_S3.Text = strvL2Code1_S3
        Else
            chkL2Code_S3.Checked = False
            txtL2Code_S3.Text = ""
        End If
        'L2 Desc Eng 1
        If strcL2DescEng1_S3.Trim.ToUpper = "Y" Then
            chkL2DescEng_S3.Checked = True
            txtL2DescEng_S3.Text = strvL2DescEng1_S3
        Else
            chkL2DescEng_S3.Checked = False
            txtL2DescEng_S3.Text = ""
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        'PreSchoolInd 1
        If strcPreSchoolInd1.Trim.ToUpper = "Y" Then
            chkPreSchoolInd.Checked = True
            txtPreSchoolInd.Text = strvPreSchoolInd1
        Else
            chkPreSchoolInd.Checked = False
            txtPreSchoolInd.Text = ""
        End If
        'Dose Interval Ind 1
        If strcDoseIntervalInd1.Trim.ToUpper = "Y" Then
            chkDoseIntervalInd.Checked = True
            txtDoseIntervalInd.Text = strvDoseIntervalInd1
        Else
            chkDoseIntervalInd.Checked = False
            txtDoseIntervalInd.Text = ""
        End If
        'TSW Ind 1
        If strcTSWInd1.Trim.ToUpper = "Y" Then
            chkTSWInd.Checked = True
            txtTSWInd.Text = strvTSWInd1
        Else
            chkTSWInd.Checked = False
            txtTSWInd.Text = ""
        End If


        'Voucher 2   =-------------------------------------------
        If strcVoucher2.Trim.ToUpper = "Y" Then
            chkHCVS2.Checked = True


        Else
            chkHCVS2.Checked = False
        End If
        'Voucher Claimed 2
        If strcVoucherClaimed2.Trim.ToUpper = "Y" Then
            chkVoucherClaimed2.Checked = True
            txtVoucherClaimed2.Text = strvVoucherClaimed2

            btnAdd2Voucher.Visible = False
            btnRemove2Voucher.Visible = True
            pnl2Voucher.Visible = True
        Else
            chkVoucherClaimed2.Checked = False
            txtVoucherClaimed2.Text = ""
        End If

        'Co-Payment Fee 2
        If strcCoPaymentFee2.Trim.ToUpper = "Y" Then
            chkCoPaymentFee2.Checked = True
            txtCoPaymentFee2.Text = strvCoPaymentFee2
        Else
            chkCoPaymentFee2.Checked = False
            txtCoPaymentFee2.Text = ""
        End If

        'Prof Code 2
        If strcProfCode2.Trim.ToUpper = "Y" Then
            chkProfCode2.Checked = True
            txtProfCode2.Text = strvProfCode2
        Else
            chkProfCode2.Checked = False
            txtProfCode2.Text = ""
        End If

        'Priority Code 2
        If strcPriorityCode2.Trim.ToUpper = "Y" Then
            chkPriorityCode2.Checked = True
            txtPriorityCode2.Text = strvPriorityCode2
        Else
            chkPriorityCode2.Checked = False
            txtPriorityCode2.Text = ""
        End If

        'L1 Code 2
        If strcL1Code2.Trim.ToUpper = "Y" Then
            chkL1Code2.Checked = True
            txtL1Code2.Text = strvL1Code2
        Else
            chkL1Code2.Checked = False
            txtL1Code2.Text = ""
        End If
        'L1 Desc Eng 2
        If strcL1DescEng2.Trim.ToUpper = "Y" Then
            chkL1DescEng2.Checked = True
            txtL1DescEng2.Text = strvL1DescEng2
        Else
            chkL1DescEng2.Checked = False
            txtL1DescEng2.Text = ""
        End If
        'L2 Code 2
        If strcL2Code2.Trim.ToUpper = "Y" Then
            chkL2Code2.Checked = True
            txtL2Code2.Text = strvL2Code2
        Else
            chkL2Code2.Checked = False
            txtL2Code2.Text = ""
        End If
        'L2 Desc Eng 2
        If strcL2DescEng2.Trim.ToUpper = "Y" Then
            chkL2DescEng2.Checked = True
            txtL2DescEng2.Text = strvL2DescEng2
        Else
            chkL2DescEng2.Checked = False
            txtL2DescEng2.Text = ""
        End If

        'S1

        'Prof Code 1
        If strcProfCode1_S1.Trim.ToUpper = "Y" Then
            chkProfCode_S12.Checked = True
            txtProfCode_S12.Text = strvProfCode1_S1
        Else
            chkProfCode_S12.Checked = False
            txtProfCode_S12.Text = ""
        End If

        'Priority Code 1
        If strcPriorityCode1_S1.Trim.ToUpper = "Y" Then
            chkPriorityCode_S12.Checked = True
            txtPriorityCode_S12.Text = strvPriorityCode1_S1
        Else
            chkPriorityCode_S12.Checked = False
            txtPriorityCode_S12.Text = ""
        End If

        'L1 Code 1
        If strcL1Code1_S1.Trim.ToUpper = "Y" Then
            chkL1Code_S12.Checked = True
            txtL1Code_S12.Text = strvL1Code1_S1
        Else
            chkL1Code_S12.Checked = False
            txtL1Code_S12.Text = ""
        End If
        'L1 Desc Eng 1
        If strcL1DescEng1_S1.Trim.ToUpper = "Y" Then
            chkL1DescEng_S12.Checked = True
            txtL1DescEng_S12.Text = strvL1DescEng1_S1
        Else
            chkL1DescEng_S12.Checked = False
            txtL1DescEng_S12.Text = ""
        End If
        'L2 Code 1
        If strcL2Code1_S1.Trim.ToUpper = "Y" Then
            chkL2Code_S12.Checked = True
            txtL2Code_S12.Text = strvL2Code1_S1
        Else
            chkL2Code_S12.Checked = False
            txtL2Code_S12.Text = ""
        End If
        'L2 Desc Eng 1
        If strcL2DescEng1_S1.Trim.ToUpper = "Y" Then
            chkL2DescEng_S12.Checked = True
            txtL2DescEng_S12.Text = strvL2DescEng1_S1
        Else
            chkL2DescEng_S12.Checked = False
            txtL2DescEng_S12.Text = ""
        End If

        'S2

        'Prof Code 1
        If strcProfCode1_S2.Trim.ToUpper = "Y" Then
            chkProfCode_S22.Checked = True
            txtProfCode_S22.Text = strvProfCode1_S2
        Else
            chkProfCode_S22.Checked = False
            txtProfCode_S22.Text = ""
        End If

        'Priority Code 1
        If strcPriorityCode1_S2.Trim.ToUpper = "Y" Then
            chkPriorityCode_S22.Checked = True
            txtPriorityCode_S22.Text = strvPriorityCode1_S2
        Else
            chkPriorityCode_S22.Checked = False
            txtPriorityCode_S22.Text = ""
        End If

        'L1 Code 1
        If strcL1Code1_S2.Trim.ToUpper = "Y" Then
            chkL1Code_S22.Checked = True
            txtL1Code_S22.Text = strvL1Code1_S2
        Else
            chkL1Code_S22.Checked = False
            txtL1Code_S22.Text = ""
        End If
        'L1 Desc Eng 1
        If strcL1DescEng1_S2.Trim.ToUpper = "Y" Then
            chkL1DescEng_S22.Checked = True
            txtL1DescEng_S22.Text = strvL1DescEng1_S2
        Else
            chkL1DescEng_S22.Checked = False
            txtL1DescEng_S22.Text = ""
        End If
        'L2 Code 1
        If strcL2Code1_S2.Trim.ToUpper = "Y" Then
            chkL2Code_S22.Checked = True
            txtL2Code_S22.Text = strvL2Code1_S2
        Else
            chkL2Code_S22.Checked = False
            txtL2Code_S22.Text = ""
        End If
        'L2 Desc Eng 1
        If strcL2DescEng1_S2.Trim.ToUpper = "Y" Then
            chkL2DescEng_S22.Checked = True
            txtL2DescEng_S22.Text = strvL2DescEng1_S2
        Else
            chkL2DescEng_S22.Checked = False
            txtL2DescEng_S22.Text = ""
        End If

        'S3

        'Prof Code 1
        If strcProfCode1_S3.Trim.ToUpper = "Y" Then
            chkProfCode_S32.Checked = True
            txtProfCode_S32.Text = strvProfCode1_S3
        Else
            chkProfCode_S32.Checked = False
            txtProfCode_S32.Text = ""
        End If

        'Priority Code 1
        If strcPriorityCode1_S3.Trim.ToUpper = "Y" Then
            chkPriorityCode_S32.Checked = True
            txtPriorityCode_S32.Text = strvPriorityCode1_S3
        Else
            chkPriorityCode_S32.Checked = False
            txtPriorityCode_S32.Text = ""
        End If

        'L1 Code 1
        If strcL1Code1_S3.Trim.ToUpper = "Y" Then
            chkL1Code_S32.Checked = True
            txtL1Code_S32.Text = strvL1Code1_S3
        Else
            chkL1Code_S32.Checked = False
            txtL1Code_S32.Text = ""
        End If
        'L1 Desc Eng 1
        If strcL1DescEng1_S3.Trim.ToUpper = "Y" Then
            chkL1DescEng_S32.Checked = True
            txtL1DescEng_S32.Text = strvL1DescEng1_S3
        Else
            chkL1DescEng_S32.Checked = False
            txtL1DescEng_S32.Text = ""
        End If
        'L2 Code 1
        If strcL2Code1_S3.Trim.ToUpper = "Y" Then
            chkL2Code_S32.Checked = True
            txtL2Code_S32.Text = strvL2Code1_S3
        Else
            chkL2Code_S32.Checked = False
            txtL2Code_S32.Text = ""
        End If
        'L2 Desc Eng 1
        If strcL2DescEng1_S3.Trim.ToUpper = "Y" Then
            chkL2DescEng_S32.Checked = True
            txtL2DescEng_S32.Text = strvL2DescEng1_S3
        Else
            chkL2DescEng_S32.Checked = False
            txtL2DescEng_S32.Text = ""
        End If

        'PreSchoolInd 2
        If strcPreSchoolInd2.Trim.ToUpper = "Y" Then
            chkPreSchoolInd2.Checked = True
            txtPreSchoolInd2.Text = strvPreSchoolInd2
        Else
            chkPreSchoolInd2.Checked = False
            txtPreSchoolInd2.Text = ""
        End If
        'Dose Interval Ind 2
        If strcDoseIntervalInd2.Trim.ToUpper = "Y" Then
            chkDoseIntervalInd2.Checked = True
            txtDoseIntervalInd2.Text = strvDoseIntervalInd2
        Else
            chkDoseIntervalInd2.Checked = False
            txtDoseIntervalInd2.Text = ""
        End If
        'TSW Ind 2
        If strcTSWInd2.Trim.ToUpper = "Y" Then
            chkTSWInd2.Checked = True
            txtTSWInd2.Text = strvTSWInd2
        Else
            chkTSWInd2.Checked = False
            txtTSWInd2.Text = ""
        End If


        'Scheme Code 1 ----------------------------------
        txtSchemeCode.Text = strSchemeCode1
        'RCH Code 1
        If strcRCHCode1.Trim.ToUpper = "Y" Then
            chkRCHCode_Vaccine1.Checked = True
            txtRCHCode_Vaccine1.Text = strvRCHCode1
        Else
            chkRCHCode_Vaccine1.Checked = False
            txtRCHCode_Vaccine1.Text = ""
        End If
        'Vaccine 1-1 
        If strcVaccine11.Trim.ToUpper = "Y" Then
            chkVaccine1.Checked = True
        Else
            chkVaccine1.Checked = False
        End If
        'Subsidy Code 1-1
        If strcSubsidyCode11.Trim.ToUpper = "Y" Then
            chkSubsidyCode1.Checked = True
            txtSubsidyCode1.Text = strvSubsidyCode11
        Else
            chkSubsidyCode1.Checked = False
            txtSubsidyCode1.Text = ""
        End If
        'Dose Seq 1-1
        If strcDoseSeq11.Trim.ToUpper = "Y" Then
            chkDoseSeq1.Checked = True
            txtDoseSeq1.Text = strvDoseSeq11
        Else
            chkDoseSeq1.Checked = False
            txtDoseSeq1.Text = ""
        End If
        'Vaccine 1-2 
        If strcVaccine12.Trim.ToUpper = "Y" Then
            chkVaccine2.Checked = True
        Else
            chkVaccine2.Checked = False
        End If
        'Subsidy Code 1-2
        If strcSubsidyCode12.Trim.ToUpper = "Y" Then
            chkSubsidyCode2.Checked = True
            txtSubsidyCode2.Text = strvSubsidyCode12
        Else
            chkSubsidyCode2.Checked = False
            txtSubsidyCode2.Text = ""
        End If
        'Dose Seq 1-2
        If strcDoseSeq12.Trim.ToUpper = "Y" Then
            chkDoseSeq2.Checked = True
            txtDoseSeq2.Text = strvDoseSeq12
        Else
            chkDoseSeq2.Checked = False
            txtDoseSeq2.Text = ""
        End If
        'PreSchoolInd v1
        If strcPreSchoolIndv1.Trim.ToUpper = "Y" Then
            chkPreSchoolInd_vaccine.Checked = True
            txtPreSchoolInd_vaccine.Text = strvPreSchoolIndv1
        Else
            chkPreSchoolInd_vaccine.Checked = False
            txtPreSchoolInd_vaccine.Text = ""
        End If
        'Dose Interval Ind v1
        If strcDoseIntervalIndv1.Trim.ToUpper = "Y" Then
            chkDoseIntervalInd_vaccine.Checked = True
            txtDoseIntervalInd_vaccine.Text = strvDoseIntervalIndv1
        Else
            chkDoseIntervalInd_vaccine.Checked = False
            txtDoseIntervalInd_vaccine.Text = ""
        End If
        'TSW Ind v1
        If strcTSWIndv1.Trim.ToUpper = "Y" Then
            chkTSWInd_vaccine.Checked = True
            txtTSWInd_vaccine.Text = strvTSWIndv1
        Else
            chkTSWInd_vaccine.Checked = False
            txtTSWInd_vaccine.Text = ""
        End If



        'Scheme Code 2 ----------------------------------
        txtSchemeCode2.Text = strSchemeCode2
        'RCH Code 1
        If strcRCHCode2.Trim.ToUpper = "Y" Then
            chkRCHCode_Vaccine2.Checked = True
            txtRCHCode_Vaccine2.Text = strvRCHCode2
        Else
            chkRCHCode_Vaccine2.Checked = False
            txtRCHCode_Vaccine2.Text = ""
        End If
        'Vaccine 2-1 
        If strcVaccine21.Trim.ToUpper = "Y" Then
            chkVaccine21.Checked = True

            btnAdd2Vaccine.Visible = False
            btnRemove2Vaccine.Visible = True
            pnl2Vaccine.Visible = True
        Else
            chkVaccine21.Checked = False
        End If
        'Subsidy Code 2-1
        If strcSubsidyCode21.Trim.ToUpper = "Y" Then
            chkSubsidyCode21.Checked = True
            txtSubsidyCode21.Text = strvSubsidyCode21

            btnAdd2Vaccine.Visible = False
            btnRemove2Vaccine.Visible = True
            pnl2Vaccine.Visible = True
        Else
            chkSubsidyCode21.Checked = False
            txtSubsidyCode21.Text = ""
        End If
        'Dose Seq 2-1
        If strcDoseSeq21.Trim.ToUpper = "Y" Then
            chkDoseSeq21.Checked = True
            txtDoseSeq21.Text = strvDoseSeq21
        Else
            chkDoseSeq21.Checked = False
            txtDoseSeq21.Text = ""
        End If
        'Vaccine 2-2 
        If strcVaccine22.Trim.ToUpper = "Y" Then
            chkVaccine22.Checked = True
        Else
            chkVaccine22.Checked = False
        End If
        'Subsidy Code 2-2
        If strcSubsidyCode22.Trim.ToUpper = "Y" Then
            chkSubsidyCode22.Checked = True
            txtSubsidyCode22.Text = strvSubsidyCode22
        Else
            chkSubsidyCode22.Checked = False
            txtSubsidyCode22.Text = ""
        End If
        'Dose Seq 1-2
        If strcDoseSeq22.Trim.ToUpper = "Y" Then
            chkDoseSeq22.Checked = True
            txtDoseSeq22.Text = strvDoseSeq22
        Else
            chkDoseSeq22.Checked = False
            txtDoseSeq22.Text = ""
        End If
        'PreSchoolInd v2
        If strcPreSchoolIndv2.Trim.ToUpper = "Y" Then
            chkPreSchoolInd_vaccine2.Checked = True
            txtPreSchoolInd_vaccine2.Text = strvPreSchoolIndv2
        Else
            chkPreSchoolInd_vaccine2.Checked = False
            txtPreSchoolInd_vaccine2.Text = ""
        End If
        'Dose Interval Ind v2
        If strcDoseIntervalIndv2.Trim.ToUpper = "Y" Then
            chkDoseIntervalInd_vaccine2.Checked = True
            txtDoseIntervalInd_vaccine2.Text = strvDoseIntervalIndv2
        Else
            chkDoseIntervalInd_vaccine2.Checked = False
            txtDoseIntervalInd_vaccine2.Text = ""
        End If
        'TSW Ind v2
        If strcTSWIndv2.Trim.ToUpper = "Y" Then
            chkTSWInd_vaccine2.Checked = True
            txtTSWInd_vaccine2.Text = strvTSWIndv2
        Else
            chkTSWInd_vaccine2.Checked = False
            txtTSWInd_vaccine2.Text = ""
        End If



        txtTestCaseNo.Text = strTempCaseNo
        txtTestCaseDesc.Text = strTestCaseDesc

        If strFunction.Trim <> "" Then
            Select Case strFunction.Trim
                Case "1"
                    rboSPPracticeValidation.Checked = True
                    rboGetReasonForVisit.Checked = False
                    rboRCHNameQuery.Checked = False
                    rboEHSValidatedAccountQuery.Checked = False
                    rboEHSAccountSubsidyQuery.Checked = False
                    rboUploadClaimHL7.Checked = False
                Case "2"
                    rboSPPracticeValidation.Checked = True
                    rboGetReasonForVisit.Checked = True
                    rboRCHNameQuery.Checked = False
                    rboEHSValidatedAccountQuery.Checked = False
                    rboEHSAccountSubsidyQuery.Checked = False
                    rboUploadClaimHL7.Checked = False
                Case "3"
                    rboSPPracticeValidation.Checked = True
                    rboGetReasonForVisit.Checked = False
                    rboRCHNameQuery.Checked = True
                    rboEHSValidatedAccountQuery.Checked = False
                    rboEHSAccountSubsidyQuery.Checked = False
                    rboUploadClaimHL7.Checked = False
                Case "4"
                    rboSPPracticeValidation.Checked = True
                    rboGetReasonForVisit.Checked = False
                    rboRCHNameQuery.Checked = False
                    rboEHSValidatedAccountQuery.Checked = True
                    rboEHSAccountSubsidyQuery.Checked = False
                    rboUploadClaimHL7.Checked = False
                Case "5"
                    rboSPPracticeValidation.Checked = True
                    rboGetReasonForVisit.Checked = False
                    rboRCHNameQuery.Checked = False
                    rboEHSValidatedAccountQuery.Checked = False
                    rboEHSAccountSubsidyQuery.Checked = True
                    rboUploadClaimHL7.Checked = False
                Case "6"
                    rboSPPracticeValidation.Checked = True
                    rboGetReasonForVisit.Checked = False
                    rboRCHNameQuery.Checked = False
                    rboEHSValidatedAccountQuery.Checked = False
                    rboEHSAccountSubsidyQuery.Checked = False
                    rboUploadClaimHL7.Checked = True
            End Select
        End If
        'Doc Type
        Select Case strDocType.Trim
            Case "HKIC"
                rboHKIC.Checked = True
                rboEC.Checked = False
                rboHKBC.Checked = False
                rboADOPC.Checked = False
                rboREPMT.Checked = False
                rboDOCI.Checked = False
                rboVISA.Checked = False
                rboID235B.Checked = False
            Case "EC"
                rboHKIC.Checked = False
                rboEC.Checked = True
                rboHKBC.Checked = False
                rboADOPC.Checked = False
                rboREPMT.Checked = False
                rboDOCI.Checked = False
                rboVISA.Checked = False
                rboID235B.Checked = False
            Case "HKBC"
                rboHKIC.Checked = False
                rboEC.Checked = False
                rboHKBC.Checked = True
                rboADOPC.Checked = False
                rboREPMT.Checked = False
                rboDOCI.Checked = False
                rboVISA.Checked = False
                rboID235B.Checked = False
            Case "ADOPC"
                rboHKIC.Checked = False
                rboEC.Checked = False
                rboHKBC.Checked = False
                rboADOPC.Checked = True
                rboREPMT.Checked = False
                rboDOCI.Checked = False
                rboVISA.Checked = False
                rboID235B.Checked = False
            Case "REPMT"
                rboHKIC.Checked = False
                rboEC.Checked = False
                rboHKBC.Checked = False
                rboADOPC.Checked = False
                rboREPMT.Checked = True
                rboDOCI.Checked = False
                rboVISA.Checked = False
                rboID235B.Checked = False
            Case "Doc/I", "DOCI"
                rboHKIC.Checked = False
                rboEC.Checked = False
                rboHKBC.Checked = False
                rboADOPC.Checked = False
                rboREPMT.Checked = False
                rboDOCI.Checked = True
                rboVISA.Checked = False
                rboID235B.Checked = False
            Case "VISA"
                rboHKIC.Checked = False
                rboEC.Checked = False
                rboHKBC.Checked = False
                rboADOPC.Checked = False
                rboREPMT.Checked = False
                rboDOCI.Checked = False
                rboVISA.Checked = True
                rboID235B.Checked = False
            Case "ID235B"
                rboHKIC.Checked = False
                rboEC.Checked = False
                rboHKBC.Checked = False
                rboADOPC.Checked = False
                rboREPMT.Checked = False
                rboDOCI.Checked = False
                rboVISA.Checked = False
                rboID235B.Checked = True

        End Select
    End Sub


#End Region


End Class