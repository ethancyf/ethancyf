Imports TestWSforHKMA.Component
Imports TestWSforHKMA.Component.Request
Imports TestWSforHKMA.Cryptography
Imports System.Xml
Imports System.Globalization


Partial Public Class _Default1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private blnUATMode As Boolean = False
#Region "View 1"

    Protected Sub btnProcced_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcced.Click
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

        udtUploadClaimRequest.WSClaimDetailList = New WSClaimDetailModelCollection()
        If Me.chkClaimInfo.Checked Then

            udtUploadClaimRequest.ClaimInfo_inXML = True
            'udtUploadClaimRequest.WSClaimDetailList = New WSClaimDetailModelCollection()

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

                udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ReasonForVisit_Included = True
                'ProfCode
                If chkProfCode.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode = Me.txtProfCode.Text
                    udtUploadClaimRequest.WSClaimDetailList.Item(0).WSVoucher.ProfCode_Included = True
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
                If chkRCHCode.Checked Then
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).RCHCode_Included = True
                    udtUploadClaimRequest.WSClaimDetailList.Item(i).RCHCode = txtRCHCode.Text
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
        'ws.Url = "http://localhost/ExternalInterfaceWS/ExternalInterface.asmx"
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

        btnReset_Click(Me, New System.EventArgs)
    End Sub
#End Region

#Region "WS Radio Buttons"

    Private Sub rboUploadClaim_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboUploadClaim.CheckedChanged
        If rboUploadClaim.Checked Then
            'rboUploadClaim.BackColor = Drawing.Color.Aquamarine
            'rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
            'rboRCHNameQuery.BackColor = Drawing.Color.Transparent
            'rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
            'rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
            'rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
            'rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

            'chkSPInfo.Checked = True
            'chkAccountInfo.Checked = True
            'chkClaimInfo.Checked = True

            'chkSPInfo.BackColor = Drawing.Color.Aquamarine
            'chkAccountInfo.BackColor = Drawing.Color.Aquamarine
            'chkClaimInfo.BackColor = Drawing.Color.Aquamarine
        Else
            'rboUploadClaim.BackColor = Drawing.Color.Transparent
        End If
    End Sub

    Private Sub rboUploadClaimHL7_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboUploadClaimHL7.CheckedChanged
        'If rboUploadClaimHL7.Checked Then
        '    rboUploadClaimHL7.BackColor = Drawing.Color.Aquamarine
        '    rboUploadClaim.BackColor = Drawing.Color.Transparent
        '    rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        '    rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        '    rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        '    rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        '    rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

        '    chkSPInfo.Checked = True
        '    chkAccountInfo.Checked = True
        '    chkClaimInfo.Checked = True

        '    chkSPInfo.BackColor = Drawing.Color.Aquamarine
        '    chkAccountInfo.BackColor = Drawing.Color.Aquamarine
        '    chkClaimInfo.BackColor = Drawing.Color.Aquamarine
        'Else
        '    rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
        'End If
    End Sub

    Private Sub rboRCHNameQuery_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboRCHNameQuery.CheckedChanged
        'If rboRCHNameQuery.Checked Then
        '    rboUploadClaim.BackColor = Drawing.Color.Transparent
        '    rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
        '    rboRCHNameQuery.BackColor = Drawing.Color.Aquamarine
        '    rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        '    rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        '    rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        '    rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

        '    chkSPInfo.Checked = True
        '    chkAccountInfo.Checked = True
        '    chkClaimInfo.Checked = False

        '    chkSPInfo.BackColor = Drawing.Color.Aquamarine
        '    chkAccountInfo.BackColor = Drawing.Color.Aquamarine
        '    chkClaimInfo.BackColor = Drawing.Color.Transparent
        'Else
        '    rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        'End If
    End Sub

    Private Sub rboGetReasonForVisit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboGetReasonForVisit.CheckedChanged
        'If rboGetReasonForVisit.Checked Then
        '    rboUploadClaim.BackColor = Drawing.Color.Transparent
        '    rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
        '    rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        '    rboGetReasonForVisit.BackColor = Drawing.Color.Aquamarine
        '    rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        '    rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        '    rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

        '    chkSPInfo.Checked = False
        '    chkAccountInfo.Checked = False
        '    chkClaimInfo.Checked = False

        '    chkSPInfo.BackColor = Drawing.Color.Transparent
        '    chkAccountInfo.BackColor = Drawing.Color.Transparent
        '    chkClaimInfo.BackColor = Drawing.Color.Transparent
        'Else
        '    rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        'End If
    End Sub

    Private Sub rboEHSValidatedAccountQuery_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboEHSValidatedAccountQuery.CheckedChanged
        'If rboEHSValidatedAccountQuery.Checked Then
        '    rboUploadClaim.BackColor = Drawing.Color.Transparent
        '    rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
        '    rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        '    rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        '    rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Aquamarine
        '    rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        '    rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

        '    chkSPInfo.Checked = True
        '    chkAccountInfo.Checked = True
        '    chkClaimInfo.Checked = False

        '    chkSPInfo.BackColor = Drawing.Color.Aquamarine
        '    chkAccountInfo.BackColor = Drawing.Color.Aquamarine
        '    chkClaimInfo.BackColor = Drawing.Color.Transparent
        'Else
        '    rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        'End If
    End Sub

    Private Sub rboEHSAccountSubsidyQuery_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboEHSAccountSubsidyQuery.CheckedChanged
        'If rboEHSAccountSubsidyQuery.Checked Then
        '    rboUploadClaim.BackColor = Drawing.Color.Transparent
        '    rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
        '    rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        '    rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        '    rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        '    rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Aquamarine
        '    rboSPPracticeValidation.BackColor = Drawing.Color.Transparent

        '    chkSPInfo.Checked = True
        '    chkAccountInfo.Checked = True
        '    chkClaimInfo.Checked = False

        '    chkSPInfo.BackColor = Drawing.Color.Aquamarine
        '    chkAccountInfo.BackColor = Drawing.Color.Aquamarine
        '    chkClaimInfo.BackColor = Drawing.Color.Transparent
        'Else
        '    rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        'End If
    End Sub

    Private Sub rboSPPracticeValidation_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rboSPPracticeValidation.CheckedChanged
        'If rboSPPracticeValidation.Checked Then
        '    rboUploadClaim.BackColor = Drawing.Color.Transparent
        '    rboUploadClaimHL7.BackColor = Drawing.Color.Transparent
        '    rboRCHNameQuery.BackColor = Drawing.Color.Transparent
        '    rboGetReasonForVisit.BackColor = Drawing.Color.Transparent
        '    rboEHSValidatedAccountQuery.BackColor = Drawing.Color.Transparent
        '    rboEHSAccountSubsidyQuery.BackColor = Drawing.Color.Transparent
        '    rboSPPracticeValidation.BackColor = Drawing.Color.Aquamarine

        '    chkSPInfo.Checked = True
        '    chkAccountInfo.Checked = False
        '    chkClaimInfo.Checked = False

        '    chkSPInfo.BackColor = Drawing.Color.Aquamarine
        '    chkAccountInfo.BackColor = Drawing.Color.Transparent
        '    chkClaimInfo.BackColor = Drawing.Color.Transparent
        'Else
        '    rboSPPracticeValidation.BackColor = Drawing.Color.Transparent
        'End If
    End Sub

#End Region

#Region "UI Controls"

#Region "SP related"

    Private Sub chkSPID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSPID.CheckedChanged
        'If chkSPID.Checked Then
        '    Me.txtSPID.Text = ""
        '    Me.txtSPID.Enabled = True
        '    Me.txtSPID.BackColor = Drawing.Color.White
        'Else
        '    Me.txtSPID.Text = ""
        '    Me.txtSPID.Enabled = False
        '    Me.txtSPID.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkSPSurname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSPSurname.CheckedChanged
        'If chkSPSurname.Checked Then
        '    Me.txtSPSurname.Text = ""
        '    Me.txtSPSurname.Enabled = True
        '    Me.txtSPSurname.BackColor = Drawing.Color.White
        'Else
        '    Me.txtSPSurname.Text = ""
        '    Me.txtSPSurname.Enabled = False
        '    Me.txtSPSurname.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkSPGivenname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSPGivenname.CheckedChanged
        'If chkSPGivenname.Checked Then
        '    Me.txtSPGivenName.Text = ""
        '    Me.txtSPGivenName.Enabled = True
        '    Me.txtSPGivenName.BackColor = Drawing.Color.White
        'Else
        '    Me.txtSPGivenName.Text = ""
        '    Me.txtSPGivenName.Enabled = False
        '    Me.txtSPGivenName.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkPracticeID_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPracticeID.CheckedChanged
        'If chkPracticeID.Checked Then
        '    Me.txtPracticeID.Text = ""
        '    Me.txtPracticeID.Enabled = True
        '    Me.txtPracticeID.BackColor = Drawing.Color.White
        'Else
        '    Me.txtPracticeID.Text = ""
        '    Me.txtPracticeID.Enabled = False
        '    Me.txtPracticeID.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkPracticeName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPracticeName.CheckedChanged
        'If chkPracticeName.Checked Then
        '    Me.txtPracticeName.Text = ""
        '    Me.txtPracticeName.Enabled = True
        '    Me.txtPracticeName.BackColor = Drawing.Color.White
        'Else
        '    Me.txtPracticeName.Text = ""
        '    Me.txtPracticeName.Enabled = False
        '    Me.txtPracticeName.BackColor = Drawing.Color.Silver
        'End If
    End Sub

#End Region

#Region "Account Related"

    Private Sub chkEntryNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEntryNo.CheckedChanged
        'If chkEntryNo.Checked Then
        '    Me.txtEntryNo.Text = ""
        '    Me.txtEntryNo.Enabled = True
        '    Me.txtEntryNo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtEntryNo.Text = ""
        '    Me.txtEntryNo.Enabled = False
        '    Me.txtEntryNo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDocumentNo.CheckedChanged
        'If chkDocumentNo.Checked Then
        '    Me.txtDocumentNo.Text = ""
        '    Me.txtDocumentNo.Enabled = True
        '    Me.txtDocumentNo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtDocumentNo.Text = ""
        '    Me.txtDocumentNo.Enabled = False
        '    Me.txtDocumentNo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkHKIC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHKIC.CheckedChanged
        'If chkHKIC.Checked Then
        '    Me.txtHKIC.Text = ""
        '    Me.txtHKIC.Enabled = True
        '    Me.txtHKIC.BackColor = Drawing.Color.White
        'Else
        '    Me.txtHKIC.Text = ""
        '    Me.txtHKIC.Enabled = False
        '    Me.txtHKIC.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkRegNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRegNo.CheckedChanged
        'If chkRegNo.Checked Then
        '    Me.txtRegNo.Text = ""
        '    Me.txtRegNo.Enabled = True
        '    Me.txtRegNo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtRegNo.Text = ""
        '    Me.txtRegNo.Enabled = False
        '    Me.txtRegNo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkBirthEntryNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBirthEntryNo.CheckedChanged
        'If chkBirthEntryNo.Checked Then
        '    Me.txtBirthEntryNo.Text = ""
        '    Me.txtBirthEntryNo.Enabled = True
        '    Me.txtBirthEntryNo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtBirthEntryNo.Text = ""
        '    Me.txtBirthEntryNo.Enabled = False
        '    Me.txtBirthEntryNo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkPermitNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPermitNo.CheckedChanged
        'If chkPermitNo.Checked Then
        '    Me.txtPermitNo.Text = ""
        '    Me.txtPermitNo.Enabled = True
        '    Me.txtPermitNo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtPermitNo.Text = ""
        '    Me.txtPermitNo.Enabled = False
        '    Me.txtPermitNo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkVISANo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVISANo.CheckedChanged
        'If chkVISANo.Checked Then
        '    Me.txtVISANo.Text = ""
        '    Me.txtVISANo.Enabled = True
        '    Me.txtVISANo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtVISANo.Text = ""
        '    Me.txtVISANo.Enabled = False
        '    Me.txtVISANo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkSurname_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSurname.CheckedChanged
        'If chkSurname.Checked Then
        '    Me.txtSurname.Text = ""
        '    Me.txtSurname.Enabled = True
        '    Me.txtSurname.BackColor = Drawing.Color.White
        'Else
        '    Me.txtSurname.Text = ""
        '    Me.txtSurname.Enabled = False
        '    Me.txtSurname.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkGivenName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGivenName.CheckedChanged
        'If chkGivenName.Checked Then
        '    Me.txtGivenName.Text = ""
        '    Me.txtGivenName.Enabled = True
        '    Me.txtGivenName.BackColor = Drawing.Color.White
        'Else
        '    Me.txtGivenName.Text = ""
        '    Me.txtGivenName.Enabled = False
        '    Me.txtGivenName.BackColor = Drawing.Color.Silver
        'End If
    End Sub



    Private Sub chkGender_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGender.CheckedChanged
        'If chkGender.Checked Then
        '    Me.txtGender.Text = ""
        '    Me.txtGender.Enabled = True
        '    Me.txtGender.BackColor = Drawing.Color.White
        'Else
        '    Me.txtGender.Text = ""
        '    Me.txtGender.Enabled = False
        '    Me.txtGender.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkDOBType_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOBType.CheckedChanged
        'If chkDOBType.Checked Then
        '    Me.txtDOBType.Text = ""
        '    Me.txtDOBType.Enabled = True
        '    Me.txtDOBType.BackColor = Drawing.Color.White
        'Else
        '    Me.txtDOBType.Text = ""
        '    Me.txtDOBType.Enabled = False
        '    Me.txtDOBType.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkAgeOn_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAgeOn.CheckedChanged
        'If chkAgeOn.Checked Then
        '    Me.txtAgeOn.Text = ""
        '    Me.txtAgeOn.Enabled = True
        '    Me.txtAgeOn.BackColor = Drawing.Color.White
        'Else
        '    Me.txtAgeOn.Text = ""
        '    Me.txtAgeOn.Enabled = False
        '    Me.txtAgeOn.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkDOReg_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOReg.CheckedChanged
        'If chkDOReg.Checked Then
        '    Me.txtDOReg.Text = ""
        '    Me.txtDOReg.Enabled = True
        '    Me.txtDOReg.BackColor = Drawing.Color.White
        'Else
        '    Me.txtDOReg.Text = ""
        '    Me.txtDOReg.Enabled = False
        '    Me.txtDOReg.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkDOBInWord_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOBInWord.CheckedChanged
        'If chkDOBInWord.Checked Then
        '    Me.txtDOBInWord.Text = ""
        '    Me.txtDOBInWord.Enabled = True
        '    Me.txtDOBInWord.BackColor = Drawing.Color.White
        'Else
        '    Me.txtDOBInWord.Text = ""
        '    Me.txtDOBInWord.Enabled = False
        '    Me.txtDOBInWord.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkNameChi_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNameChi.CheckedChanged
        'If chkNameChi.Checked Then
        '    Me.txtNameChi.Text = ""
        '    Me.txtNameChi.Enabled = True
        '    Me.txtNameChi.BackColor = Drawing.Color.White
        'Else
        '    Me.txtNameChi.Text = ""
        '    Me.txtNameChi.Enabled = False
        '    Me.txtNameChi.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkDOI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOI.CheckedChanged
        'If chkDOI.Checked Then
        '    Me.txtDOI.Text = ""
        '    Me.txtDOI.Enabled = True
        '    Me.txtDOI.BackColor = Drawing.Color.White
        'Else
        '    Me.txtDOI.Text = ""
        '    Me.txtDOI.Enabled = False
        '    Me.txtDOI.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkSerialNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSerialNo.CheckedChanged
        'If chkSerialNo.Checked Then
        '    Me.txtSerialNo.Text = ""
        '    Me.txtSerialNo.Enabled = True
        '    Me.txtSerialNo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtSerialNo.Text = ""
        '    Me.txtSerialNo.Enabled = False
        '    Me.txtSerialNo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkReference_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkReference.CheckedChanged
        'If chkReference.Checked Then
        '    Me.txtReference.Text = ""
        '    Me.txtReference.Enabled = True
        '    Me.txtReference.BackColor = Drawing.Color.White
        'Else
        '    Me.txtReference.Text = ""
        '    Me.txtReference.Enabled = False
        '    Me.txtReference.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkFreeReference_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFreeReference.CheckedChanged
        'If chkFreeReference.Checked Then
        '    Me.txtFreeReference.Text = ""
        '    Me.txtFreeReference.Enabled = True
        '    Me.txtFreeReference.BackColor = Drawing.Color.White
        'Else
        '    Me.txtFreeReference.Text = ""
        '    Me.txtFreeReference.Enabled = False
        '    Me.txtFreeReference.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkRemainUntil_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRemainUntil.CheckedChanged
        'If chkRemainUntil.Checked Then
        '    Me.txtRemainUntil.Text = ""
        '    Me.txtRemainUntil.Enabled = True
        '    Me.txtRemainUntil.BackColor = Drawing.Color.White
        'Else
        '    Me.txtRemainUntil.Text = ""
        '    Me.txtRemainUntil.Enabled = False
        '    Me.txtRemainUntil.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkPassportNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPassportNo.CheckedChanged
        'If chkPassportNo.Checked Then
        '    Me.txtPassportNo.Text = ""
        '    Me.txtPassportNo.Enabled = True
        '    Me.txtPassportNo.BackColor = Drawing.Color.White
        'Else
        '    Me.txtPassportNo.Text = ""
        '    Me.txtPassportNo.Enabled = False
        '    Me.txtPassportNo.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkRCHCode_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRCHCode.CheckedChanged
        'If chkRCHCode.Checked Then
        '    Me.txtRCHCode.Text = ""
        '    Me.txtRCHCode.Enabled = True
        '    Me.txtRCHCode.BackColor = Drawing.Color.White
        'Else
        '    Me.txtRCHCode.Text = ""
        '    Me.txtRCHCode.Enabled = False
        '    Me.txtRCHCode.BackColor = Drawing.Color.Silver
        'End If
    End Sub

    Private Sub chkDOB_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOB.CheckedChanged
        'If chkDOB.Checked Then
        '    Me.txtDOB.Text = ""
        '    Me.txtDOB.Enabled = True
        '    Me.txtDOB.BackColor = Drawing.Color.White
        'Else
        '    Me.txtDOB.Text = ""
        '    Me.txtDOB.Enabled = False
        '    Me.txtDOB.BackColor = Drawing.Color.Silver
        'End If
    End Sub
#End Region

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
    End Sub

    Protected Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        chkSPID.Checked = False
        Me.txtSPID.Text = ""
        'Me.txtSPID.Enabled = False
        'Me.txtSPID.BackColor = Drawing.Color.Silver

        chkSPSurname.Checked = False
        Me.txtSPSurname.Text = ""
        'Me.txtSPSurname.Enabled = False
        'Me.txtSPSurname.BackColor = Drawing.Color.Silver

        chkSPGivenname.Checked = False
        Me.txtSPGivenName.Text = ""
        'Me.txtSPGivenName.Enabled = False
        'Me.txtSPGivenName.BackColor = Drawing.Color.Silver

        chkPracticeID.Checked = False
        Me.txtPracticeID.Text = ""
        'Me.txtPracticeID.Enabled = False
        'Me.txtPracticeID.BackColor = Drawing.Color.Silver

        chkPracticeName.Checked = False
        Me.txtPracticeName.Text = ""
        'Me.txtPracticeName.Enabled = False
        'Me.txtPracticeName.BackColor = Drawing.Color.Silver

        chkEntryNo.Checked = False
        Me.txtEntryNo.Text = ""
        'Me.txtEntryNo.Enabled = False
        'Me.txtEntryNo.BackColor = Drawing.Color.Silver

        chkDocumentNo.Checked = False
        Me.txtDocumentNo.Text = ""
        'Me.txtDocumentNo.Enabled = False
        'Me.txtDocumentNo.BackColor = Drawing.Color.Silver

        chkHKIC.Checked = False
        Me.txtHKIC.Text = ""
        'Me.txtHKIC.Enabled = False
        'Me.txtHKIC.BackColor = Drawing.Color.Silver

        chkRegNo.Checked = False
        Me.txtRegNo.Text = ""
        'Me.txtRegNo.Enabled = False
        'Me.txtRegNo.BackColor = Drawing.Color.Silver

        chkBirthEntryNo.Checked = False
        Me.txtBirthEntryNo.Text = ""
        'Me.txtBirthEntryNo.Enabled = False
        'Me.txtBirthEntryNo.BackColor = Drawing.Color.Silver

        chkPermitNo.Checked = False
        Me.txtPermitNo.Text = ""
        'Me.txtPermitNo.Enabled = False
        'Me.txtPermitNo.BackColor = Drawing.Color.Silver

        chkVISANo.Checked = False
        Me.txtVISANo.Text = ""
        'Me.txtVISANo.Enabled = False
        'Me.txtVISANo.BackColor = Drawing.Color.Silver

        chkSurname.Checked = False
        Me.txtSurname.Text = ""
        'Me.txtSurname.Enabled = False
        'Me.txtSurname.BackColor = Drawing.Color.Silver

        chkGivenName.Checked = False
        Me.txtGivenName.Text = ""
        'Me.txtGivenName.Enabled = False
        'Me.txtGivenName.BackColor = Drawing.Color.Silver

        chkGender.Checked = False
        Me.txtGender.Text = ""
        'Me.txtGender.Enabled = False
        'Me.txtGender.BackColor = Drawing.Color.Silver

        chkDOBType.Checked = False
        Me.txtDOBType.Text = ""
        'Me.txtDOBType.Enabled = False
        'Me.txtDOBType.BackColor = Drawing.Color.Silver

        chkAgeOn.Checked = False
        Me.txtAgeOn.Text = ""
        'Me.txtAgeOn.Enabled = False
        'Me.txtAgeOn.BackColor = Drawing.Color.Silver

        chkDOReg.Checked = False
        Me.txtDOReg.Text = ""
        'Me.txtDOReg.Enabled = False
        'Me.txtDOReg.BackColor = Drawing.Color.Silver

        chkDOBInWord.Checked = False
        Me.txtDOBInWord.Text = ""
        'Me.txtDOBInWord.Enabled = False
        'Me.txtDOBInWord.BackColor = Drawing.Color.Silver

        chkNameChi.Checked = False
        Me.txtNameChi.Text = ""
        'Me.txtNameChi.Enabled = False
        'Me.txtNameChi.BackColor = Drawing.Color.Silver

        chkDOI.Checked = False
        Me.txtDOI.Text = ""
        'Me.txtDOI.Enabled = False
        'Me.txtDOI.BackColor = Drawing.Color.Silver

        chkSerialNo.Checked = False
        Me.txtSerialNo.Text = ""
        'Me.txtSerialNo.Enabled = False
        'Me.txtSerialNo.BackColor = Drawing.Color.Silver

        chkReference.Checked = False
        Me.txtReference.Text = ""
        'Me.txtReference.Enabled = False
        'Me.txtReference.BackColor = Drawing.Color.Silver

        chkFreeReference.Checked = False
        Me.txtFreeReference.Text = ""
        'Me.txtFreeReference.Enabled = False
        'Me.txtFreeReference.BackColor = Drawing.Color.Silver

        chkRemainUntil.Checked = False
        Me.txtRemainUntil.Text = ""
        'Me.txtRemainUntil.Enabled = False
        'Me.txtRemainUntil.BackColor = Drawing.Color.Silver

        chkPassportNo.Checked = False
        Me.txtPassportNo.Text = ""
        'Me.txtPassportNo.Enabled = False
        'Me.txtPassportNo.BackColor = Drawing.Color.Silver

        chkRCHCode.Checked = False
        Me.txtRCHCode.Text = ""
        'Me.txtRCHCode.Enabled = False
        'Me.txtRCHCode.BackColor = Drawing.Color.Silver

        chkDOB.Checked = False
        Me.txtDOB.Text = ""
        'Me.txtDOB.Enabled = False
        'Me.txtDOB.BackColor = Drawing.Color.Silver
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

            txtExtractedResult.Text = "Success"
            'Dim Xml As XmlDocument = New XmlDocument
            'Xml.LoadXml(strXML)

            'txtExtractedResult.Text = "-------------------------------Upload Claim Result--------------------------------" + vbCrLf

            'Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            'Dim nlTranInfo As XmlNodeList = nlResult.Item(0).SelectNodes("./TranInfo")

            'For Each node As XmlNode In nlTranInfo
            '    strValue = ReadString(node, "TranIndex")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "TranIndex     : " + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "TranID")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "TranID        : " + strValue + " " + vbCrLf
            'Next

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

            txtExtractedResult.Text = "Success"
            'Dim Xml As XmlDocument = New XmlDocument
            'Xml.LoadXml(strXML)

            'txtExtractedResult.Text = "-------------------------------RCH Name Query--------------------------------" + vbCrLf

            'Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            'strValue = ReadString(nlResult.Item(0), "HomeNameEng")
            'txtExtractedResult.Text = txtExtractedResult.Text + "HomeNameEng : " + strValue.Trim + vbCrLf

            'strValue = ReadString(nlResult.Item(0), "HomeNameChi")
            'txtExtractedResult.Text = txtExtractedResult.Text + "HomeNameChi : " + strValue.Trim + vbCrLf

            'strValue = ReadString(nlResult.Item(0), "AddressEng")
            'txtExtractedResult.Text = txtExtractedResult.Text + "AddressEng : " + strValue.Trim + vbCrLf

            'strValue = ReadString(nlResult.Item(0), "AddressChi")
            'txtExtractedResult.Text = txtExtractedResult.Text + "AddressChi : " + strValue.Trim + vbCrLf

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

            txtExtractedResult.Text = "Success"
            'Dim Xml As XmlDocument = New XmlDocument
            'Xml.LoadXml(strXML)

            'Dim nlL1 As XmlNodeList = Xml.GetElementsByTagName("ReasonForVisitL1")
            'Dim nlL1Item As XmlNodeList = nlL1.Item(0).SelectNodes("./L1Entry")

            'If nlL1Item.Count > 0 Then
            '    txtExtractedResult.Text = "------------------------------------List of Reason for Vist------------------------------------" + vbCrLf + vbCrLf
            'End If

            'For Each node As XmlNode In nlL1Item
            '    strValue = ReadString(node, "ProfCode")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "ProfCode :" + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "L1Code")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "L1Code   :" + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "L1DescEng")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "L1DescEng:" + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "L1DescChi")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "L1DescChi:" + strValue + " " + vbCrLf
            '    txtExtractedResult.Text = txtExtractedResult.Text + vbCrLf
            'Next

            'Dim nlL2 As XmlNodeList = Xml.GetElementsByTagName("ReasonForVisitL2")
            'Dim nlL2Item As XmlNodeList = nlL2.Item(0).SelectNodes("./L2Entry")
            'For Each node As XmlNode In nlL2Item
            '    strValue = ReadString(node, "ProfCode")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "ProfCode :" + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "L1Code")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "L1Code   :" + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "L2Code")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "L2DescEng:" + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "L2DescEng")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "L2DescEng:" + strValue + " " + vbCrLf
            '    strValue = ReadString(node, "L2DescChi")
            '    txtExtractedResult.Text = txtExtractedResult.Text + "L2DescChi:" + strValue + " " + vbCrLf
            '    txtExtractedResult.Text = txtExtractedResult.Text + vbCrLf
            'Next

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

            txtExtractedResult.Text = "Success"
            'Dim Xml As XmlDocument = New XmlDocument
            'Dim strValid As String = ""
            'Xml.LoadXml(strXML)

            'txtExtractedResult.Text = "-------------------------------Validated Account Query--------------------------------" + vbCrLf

            'Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            'strValid = ReadString(nlResult.Item(0), "AccountMatched")
            'If strValid.Trim = "Y" Then
            '    txtExtractedResult.Text = txtExtractedResult.Text + "AccountMatched : Y"
            'ElseIf strValid.Trim = "N" Then
            '    txtExtractedResult.Text = txtExtractedResult.Text + "AccountMatched : N"
            'ElseIf strValid.Trim = "X" Then
            '    txtExtractedResult.Text = txtExtractedResult.Text + "AccountMatched : X"
            'End If

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

            txtExtractedResult.Text = "Success"
            'Dim Xml As XmlDocument = New XmlDocument
            'Xml.LoadXml(strXML)

            'txtExtractedResult.Text = "-------------------------------Voucher Query--------------------------------" + vbCrLf

            'Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("SchemeInfo")
            'strValue = ReadString(nlResult.Item(0), "SchemeCode")
            'txtExtractedResult.Text = txtExtractedResult.Text + "SchemeCode      : " + strValue.Trim + vbCrLf

            'strValue = ReadString(nlResult.Item(0), "VoucherRemained")
            'txtExtractedResult.Text = txtExtractedResult.Text + "VoucherRemained : " + strValue.Trim + vbCrLf


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

            txtExtractedResult.Text = "Success"
            'Dim Xml As XmlDocument = New XmlDocument
            'Dim strValid As String = ""
            'Xml.LoadXml(strXML)

            'txtExtractedResult.Text = "---------------------------------Service Provider Validation--------------------------------" + vbCrLf

            'Dim nlResult As XmlNodeList = Xml.GetElementsByTagName("Output")
            'strValid = ReadString(nlResult.Item(0), "IsCorrect")
            'If strValid.Trim = "Y" Then
            '    txtExtractedResult.Text = txtExtractedResult.Text + "IsCorrect : Y"
            'ElseIf strValid.Trim = "N" Then
            '    txtExtractedResult.Text = txtExtractedResult.Text + "IsCorrect : N"
            'End If

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
            'txtExtractedResult.Text = "---------------------------------Error--------------------------------" + vbCrLf + vbCrLf
            'strValue = ReadString(nlResult.Item(0), "ErrorCode")
            'txtExtractedResult.Text = txtExtractedResult.Text + "ErrorCode    : " + strValue + " " + vbCrLf
            'strValue = ReadString(nlResult.Item(0), "ErrorMessage")
            'txtExtractedResult.Text = txtExtractedResult.Text + "ErrorMessage : " + strValue + " " + vbCrLf
            txtExtractedResult.Text = "Failed"
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

End Class