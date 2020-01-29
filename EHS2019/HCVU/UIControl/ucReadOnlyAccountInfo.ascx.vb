Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType.DocTypeModel

Partial Public Class ucReadOnlyAccountInfo
    Inherits System.Web.UI.UserControl

    Private _udtEHSAccount As EHSAccountModel
    Private _intWidth As Integer = 270
    Private _blnShowPublicEnquiry As Boolean = True

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SetupTableTitle(Me._intWidth)
        Me.RenderLanguage()
        Me.Setup()
    End Sub

    Public Sub Setup()
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()

        Dim strAcctType As String
        Dim strAcctStatus As String
        Dim strEnquiryStatus As String

        With _udtEHSAccount
            'Ref No.
            If .AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                Me.lblRefNoText.Text = Me.GetGlobalResourceObject("Text", "AccountID")
                Me.lblRefNo.Text = formatter.formatValidatedEHSAccountNumber(.VoucherAccID)
            ElseIf .AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                Me.lblRefNoText.Text = Me.GetGlobalResourceObject("Text", "SpecialAccountID")
                Me.lblRefNo.Text = formatter.formatSystemNumber(.VoucherAccID)
            Else
                Me.lblRefNoText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
                Me.lblRefNo.Text = formatter.formatSystemNumber(.VoucherAccID)
            End If

            'Account type
            strAcctType = .GetSysAccountSourceDesc(.AccountSourceString)

            'CRE13-006 HCVS Ceiling [Start][Karl]
            If .AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                If .AccountPurpose = EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
                    strAcctType = eHealthAccountStatus.Erased_Desc
                End If
            End If
            'CRE13-006 HCVS Ceiling [End][Karl]

            Me.lblAcctType.Text = strAcctType.Trim

            'Account status 
            strAcctStatus = .GetRecordStatusDescription()
            Me.lblAcctStatus.Text = strAcctStatus.Trim

            'Deceased 
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            If .Deceased() Then
                Me.lblDeceased.Text = Me.GetGlobalResourceObject("Text", "Yes")
                Me.lblDeceased.Visible = True
                Me.lblDeceasedText.Visible = True
            Else
                Me.lblDeceasedText.Visible = False
                Me.lblDeceased.Visible = False
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

            'Enquiry status
            strEnquiryStatus = Me._udtEHSAccount.PublicEnquiryStatus
            If .AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                If .Deceased() Then
                    strEnquiryStatus = Me.GetGlobalResourceObject("Text", "Unavailable")
                Else
                    strEnquiryStatus = getEnquiryStatus(strEnquiryStatus)
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
            Else
                strEnquiryStatus = Me.GetGlobalResourceObject("Text", "N/A")
            End If



            lblEnquiryStatus.Text = strEnquiryStatus.Trim

            ' ===== INT11-0001: Wrongly display the practice number for temporary account created by back office user =====

            'Create By & Create dtm
            If Me._udtEHSAccount.CreateByBO Then
                'Created by back office
                Me.lblCreatedBy.Text = .CreateBy

                Me.lblCreatedDtm.Text = formatter.formatDateTime(.CreateDtm, Session("language"))
            Else
                If IsNothing(.DataEntryBy) Then
                    If .CreateSPPracticeDisplaySeq.ToString().Trim = "0" Then
                        Me.lblCreatedBy.Text = .CreateBy
                    Else
                        Me.lblCreatedBy.Text = .CreateBy + "(" + .CreateSPPracticeDisplaySeq.ToString() + ")"
                    End If
                Else
                    If Not .DataEntryBy.Trim.Equals(String.Empty) Then
                        If .CreateSPPracticeDisplaySeq.ToString().Trim = "0" Then
                            Me.lblCreatedBy.Text = .DataEntryBy + "  " + .CreateBy
                        Else
                            Me.lblCreatedBy.Text = .DataEntryBy + "  " + .CreateBy + "(" + .CreateSPPracticeDisplaySeq.ToString() + ")"
                        End If
                    Else
                        If .CreateSPPracticeDisplaySeq.ToString().Trim = "0" Then
                            Me.lblCreatedBy.Text = .CreateBy
                        Else
                            Me.lblCreatedBy.Text = .CreateBy + "(" + .CreateSPPracticeDisplaySeq.ToString() + ")"
                        End If
                    End If
                End If
                Me.lblCreatedDtm.Text = formatter.formatDateTime(.CreateDtm, Session("language"))
            End If
            ' ===== INT11-0001: End =====

            'Account status remark
            If Not Me._udtEHSAccount.RecordStatus.Trim.Equals("A") Then
                If Not IsNothing(.Remark) Then
                    Me.lblAcctStatusRemark.Text = "  (" + .Remark.Trim + ")"
                Else
                    Me.lblAcctStatusRemark.Text = String.Empty
                End If
            Else
                Me.lblAcctStatusRemark.Text = String.Empty
            End If

            'Public Enquiry
            If _blnShowPublicEnquiry Then
                Me.panEnquiry.Visible = True

                If .AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    If Not Me._udtEHSAccount.PublicEnquiryStatus.Trim.Equals(EHSAccountModel.EnquiryStatusClass.Available) Then
                        If Not .Deceased() Then
                            If Not IsNothing(.PublicEnquiryRemark) Then
                                Me.lblEnquiryStatusRemark.Text = "  (" + .PublicEnquiryRemark.Trim + ")"
                            End If
                        Else
                            Me.lblEnquiryStatusRemark.Text = String.Empty
                        End If
                    Else
                        Me.lblEnquiryStatusRemark.Text = String.Empty
                    End If
                Else
                    Me.lblEnquiryStatusRemark.Text = String.Empty
                End If

            Else
                Me.panEnquiry.Visible = False
            End If

        End With
    End Sub

    Private Sub RenderLanguage()

        If Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
            Me.lblRefNoText.Text = Me.GetGlobalResourceObject("Text", "AccountID")
        Else
            Me.lblRefNoText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
        End If

        Me.lblAcctTypeText.Text = Me.GetGlobalResourceObject("Text", "AccountType")
        Me.lblAcctStatusText.Text = Me.GetGlobalResourceObject("Text", "AccountStatus")
        Me.lblEnquiryStatusText.Text = Me.GetGlobalResourceObject("Text", "EnquiryStatus")
        Me.lblCreatedByText.Text = Me.GetGlobalResourceObject("Text", "CreateBy")
        Me.lblCreatedDtmText.Text = Me.GetGlobalResourceObject("Text", "CreationTime")
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        'Me.lblDeletedByText.Text = Me.GetGlobalResourceObject("Text", "DeletedBy")
        'Me.lblDeletedDtmText.Text = Me.GetGlobalResourceObject("Text", "DeletedDtm")
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
    End Sub

    Private Sub SetupTableTitle(ByVal width As Integer)
        Me.lblRefNoText.Width = width
        Me.lblAcctTypeText.Width = width
        Me.lblAcctStatusText.Width = width
        Me.lblEnquiryStatusText.Width = width
        Me.lblCreatedByText.Width = width
        Me.lblCreatedDtmText.Width = width
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        'Me.lblDeletedByText.Width = width
        'Me.lblDeletedDtmText.Width = width
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
    End Sub

#Region "Property"
    Public Property EHSAccount() As EHSAccountModel
        Get
            Return Me._udtEHSAccount
        End Get
        Set(ByVal value As EHSAccountModel)
            Me._udtEHSAccount = value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return Me._intWidth
        End Get
        Set(ByVal value As Integer)
            _intWidth = value
        End Set
    End Property

    'Whether to show Public Enquiry Status
    Public Property ShowPublicEnquiryStatus() As Boolean
        Get
            Return Me._blnShowPublicEnquiry
        End Get
        Set(ByVal value As Boolean)
            _blnShowPublicEnquiry = value
        End Set
    End Property
#End Region

#Region "suppporting functions"
    Protected Function getEnquiryStatus(ByVal strEnqStatus As String) As String
        Dim strRes As String = String.Empty

        Status.GetDescriptionFromDBCode(VRAcctEnquiryStatus.ClassCode, strEnqStatus, strRes, "")

        If strRes = "" Then
            strRes = "N/A"
        End If


        Return strRes
    End Function

#End Region

End Class