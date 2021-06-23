Imports Common.Component.EHSTransaction
Imports Common.Component.ReasonForVisit
Imports Common.ComFunction
Imports Common.Component.Scheme         ' CRE11-024-02 HCVS Pilot Extension Part 2
Imports Common.Component                ' CRE11-024-02 HCVS Pilot Extension Part 2
Imports Common.Format

Partial Public Class ucReadOnlyHCVS
    Inherits System.Web.UI.UserControl


    'CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    Private Reason_for_Visit_L1 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(0)
    Private Reason_for_Visit_L1_S1 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(1)
    Private Reason_for_Visit_L1_S2 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(2)
    Private Reason_for_Visit_L1_S3 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(3)
    Private Reason_for_Visit_L2 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(0)
    Private Reason_for_Visit_L2_S1 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(1)
    Private Reason_for_Visit_L2_S2 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(2)
    Private Reason_for_Visit_L2_S3 As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(3)
    Private CoPaymentFee As String = Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee
    'CRE11-024-02 HCVS Pilot Extension Part 2 [End]
    Private udtDistrictBoardBLL As Common.Component.DistrictBoard.DistrictBoardBLL = New Common.Component.DistrictBoard.DistrictBoardBLL 'nichole
#Region "Field"

    Private udtGeneralFunction As New GeneralFunction

#End Region

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel, ByVal intWidth As Integer)
        ' Reason for Visit
        Dim udtReasonForVisitBLL As New ReasonForVisitBLL
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim udtFormatter As New Formatter
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' 1.) Retrieve Values for Principal Reason for Visit and CoPaymentFee
        'Dim intVisitL1 As Integer = CInt(udtEHSTransaction.TransactionAdditionFields(0).AdditionalFieldValueCode)
        'Dim intVisitL2 As Integer = CInt(udtEHSTransaction.TransactionAdditionFields(1).AdditionalFieldValueCode)
        Dim intVisitL1 As Integer = 0
        Dim intVisitL2 As Integer = 0
        Dim strVisitL1 As String = String.Empty
        Dim strVisitL2 As String = String.Empty
        Dim intCoPaymentFee As Integer
        Dim strCoPaymentFee As String = String.Empty
        Dim blnCoPaymentFee As Boolean = False              ' check for validation
        Dim blnCoPaymentFeeStarted As Boolean = False       ' check for CoPaymentFee start date

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L1, strVisitL1)
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L2, strVisitL2)

            Try
                If strVisitL1 <> String.Empty And strVisitL1 <> "0" Then
                    intVisitL1 = Integer.Parse(strVisitL1)
                End If
                If strVisitL2 <> String.Empty And strVisitL2 <> "0" Then
                    intVisitL2 = Integer.Parse(strVisitL2)
                End If
            Catch ex As Exception
                lblReasonVisitToBeProvided.Visible = True
                'lblReasonVisitToBeProvided.Text = "Invalid Reason For Visit"
                tblReasonForVisit.Visible = False
                tblReasonForVisit.Style.Value = "Display: none"
                trPrincipal.Visible = False
                trPrincipal.Style.Value = "Display: none"
                lblPrimary.Visible = False
                lblPrimary.Style.Value = "Display: none"
                divReasonForVisit.Style.Value = "Display: none"
                Exit Sub
            End Try
            
            blnCoPaymentFee = Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, CoPaymentFee, strCoPaymentFee)
            If blnCoPaymentFee Then
                Try
                    intCoPaymentFee = CInt(strCoPaymentFee)
                Catch ex As Exception
                    lblReasonVisitToBeProvided.Visible = True
                    'lblReasonVisitToBeProvided.Text = "Invalid CoPayment Fee"
                    tblReasonForVisit.Visible = False
                    tblReasonForVisit.Style.Value = "Display: none"
                    trPrincipal.Visible = False
                    trPrincipal.Style.Value = "Display: none"
                    lblPrimary.Visible = False
                    lblPrimary.Style.Value = "Display: none"
                    divReasonForVisit.Style.Value = "Display: none"
                    Exit Sub
                End Try
            End If
        End If

        blnCoPaymentFeeStarted = udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate)
        If strVisitL1 <> String.Empty Then
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Dim dtResL1 As DataTable = udtReasonForVisitBLL.getReasonForVisitL1(udtEHSTransaction.ServiceType, intVisitL1)
            lblReasonVisitL1.Text = dtResL1.Rows(0)("Reason_L1")

        Else
            lblReasonVisitToBeProvided.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")    ' To be provided
            divReasonForVisit.Style.Value = "Display: none"
        End If

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If strVisitL2 <> String.Empty Then
            Dim dtResL2 As DataTable = udtReasonForVisitBLL.getReasonForVisitL2(udtEHSTransaction.ServiceType, intVisitL1, intVisitL2)
            lblReasonVisitL2.Text = String.Format("- {0}", dtResL2.Rows(0)("Reason_L2"))
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        '2.) Determine display values for Copayment Fee
        'Dim d As DateTime
        'Dim presult As Boolean = DateTime.TryParseExact(Session("ClaimDate"), "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None, d)

        If Not udtEHSTransaction Is Nothing Then
            If blnCoPaymentFeeStarted Then
                If blnCoPaymentFee Then
                    lblCoPaymentFee.Visible = True
                    Me.Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Me.CoPaymentFee, lblCoPaymentFee.Text)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'lblCoPaymentFee.Text = "$" + lblCoPaymentFee.Text
                    lblCoPaymentFee.Text = udtFormatter.formatMoney(lblCoPaymentFee.Text, True)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                Else
                    lblCoPaymentFee.Visible = True
                    lblCoPaymentFee.Text = Me.GetGlobalResourceObject("Text", "ToBeProvided")       ' To be provided
                End If
            Else
                lblCoPaymentFee.Visible = True
                lblCoPaymentFee.Text = Me.GetGlobalResourceObject("Text", "NotApplicable")          ' Not Applicable
            End If
        End If

        '3.) Retrieve values for Secondary Reason For Visit
        Me.trSecondary.Visible = False

        Dim strVisitL1_2 As String = String.Empty
        Dim strVisitL2_2 As String = String.Empty
        Dim strVisitL1_3 As String = String.Empty
        Dim strVisitL2_3 As String = String.Empty
        Dim strVisitL1_4 As String = String.Empty
        Dim strVisitL2_4 As String = String.Empty

        If Not udtEHSTransaction Is Nothing AndAlso Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L1_S1, strVisitL1_2)
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L2_S1, strVisitL2_2)
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L1_S2, strVisitL1_3)
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L2_S2, strVisitL2_3)
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L1_S3, strVisitL1_4)
            Transaction_AdditionFields_GetFieldValueCode(udtEHSTransaction.TransactionAdditionFields, Reason_for_Visit_L2_S3, strVisitL2_4)
        End If

        '4.) Work on Display the Secondary Reason For Visit
        Me.PlaceHolder1.Controls.Add(New LiteralControl("<table cellpadding=""1"" cellspacing=""0"" border=""0"">"))
        Me.PlaceHolder1.Controls.Add(New LiteralControl("   <tr>"))
        Me.PlaceHolder1.Controls.Add(New LiteralControl("       <td>"))

        If strVisitL1_2 <> String.Empty Then
            Me.trSecondary.Visible = True
            Dim lblReasonVisitL1_2 As New Label()            
            Dim dtResL1_2 As DataTable = udtReasonForVisitBLL.getReasonForVisitL1(udtEHSTransaction.ServiceType, CInt(strVisitL1_2))

            lblReasonVisitL1_2.Text = dtResL1_2.Rows(0)("Reason_L1")            
            lblReasonVisitL1_2.CssClass = "tableText"

            Me.PlaceHolder1.Controls.Add(New LiteralControl("           <table cellpadding=""1"" cellspacing=""1"" border=""0"">"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("               <tr>"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("                   <td>"))
            Me.PlaceHolder1.Controls.Add(lblReasonVisitL1_2)
            Me.PlaceHolder1.Controls.Add(New LiteralControl("                   </td>"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("               </tr>"))

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If strVisitL2_2 <> String.Empty Then
                Dim lblReasonVisitL2_2 As New Label()
                Dim dtResL2_2 As DataTable = udtReasonForVisitBLL.getReasonForVisitL2(udtEHSTransaction.ServiceType, CInt(strVisitL1_2), CInt(strVisitL2_2))

                lblReasonVisitL2_2.Text = String.Format("- {0}", dtResL2_2.Rows(0)("Reason_L2"))
                lblReasonVisitL2_2.CssClass = "tableText"

                Me.PlaceHolder1.Controls.Add(New LiteralControl("               <tr>"))
                Me.PlaceHolder1.Controls.Add(New LiteralControl("                   <td style=""padding-left:15px"">"))
                Me.PlaceHolder1.Controls.Add(lblReasonVisitL2_2)
                Me.PlaceHolder1.Controls.Add(New LiteralControl("                   </td>"))
                Me.PlaceHolder1.Controls.Add(New LiteralControl("               </tr>"))
            End If
            ' CRE19-006 (DHC) [End][Winnie]
            Me.PlaceHolder1.Controls.Add(New LiteralControl("           </table>"))

        End If

        If strVisitL1_3 <> String.Empty Then
            Me.trSecondary.Visible = True
            Dim lblReasonVisitL1_3 As New Label()            
            Dim dtResL1_3 As DataTable = udtReasonForVisitBLL.getReasonForVisitL1(udtEHSTransaction.ServiceType, CInt(strVisitL1_3))

            lblReasonVisitL1_3.Text = dtResL1_3.Rows(0)("Reason_L1")            
            lblReasonVisitL1_3.CssClass = "tableText"

            Me.PlaceHolder1.Controls.Add(New LiteralControl("           <table cellpadding=""1"" cellspacing=""1"" border=""0"">"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("               <tr>"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("                   <td>"))
            Me.PlaceHolder1.Controls.Add(lblReasonVisitL1_3)
            Me.PlaceHolder1.Controls.Add(New LiteralControl("                   </td>"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("               </tr>"))

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If strVisitL2_3 <> String.Empty Then
                Dim lblReasonVisitL2_3 As New Label()
                Dim dtResL2_3 As DataTable = udtReasonForVisitBLL.getReasonForVisitL2(udtEHSTransaction.ServiceType, CInt(strVisitL1_3), CInt(strVisitL2_3))

                lblReasonVisitL2_3.Text = String.Format("- {0}", dtResL2_3.Rows(0)("Reason_L2"))
                lblReasonVisitL2_3.CssClass = "tableText"

                Me.PlaceHolder1.Controls.Add(New LiteralControl("               <tr>"))
                Me.PlaceHolder1.Controls.Add(New LiteralControl("                   <td style=""padding-left:15px"">"))
                Me.PlaceHolder1.Controls.Add(lblReasonVisitL2_3)
                Me.PlaceHolder1.Controls.Add(New LiteralControl("                   </td>"))
                Me.PlaceHolder1.Controls.Add(New LiteralControl("               </tr>"))
            End If
            ' CRE19-006 (DHC) [End][Winnie]
            Me.PlaceHolder1.Controls.Add(New LiteralControl("           </table>"))
            'Me.PlaceHolder1.Controls.Add(New LiteralControl("<br />"))
        End If

        If strVisitL1_4 <> String.Empty Then
            Me.trSecondary.Visible = True
            Dim lblReasonVisitL1_4 As New Label()            
            Dim dtResL1_4 As DataTable = udtReasonForVisitBLL.getReasonForVisitL1(udtEHSTransaction.ServiceType, CInt(strVisitL1_4))

            lblReasonVisitL1_4.Text = dtResL1_4.Rows(0)("Reason_L1")            
            lblReasonVisitL1_4.CssClass = "tableText"

            Me.PlaceHolder1.Controls.Add(New LiteralControl("           <table cellpadding=""1"" cellspacing=""1"" border=""0"">"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("               <tr>"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("                   <td>"))
            Me.PlaceHolder1.Controls.Add(lblReasonVisitL1_4)
            Me.PlaceHolder1.Controls.Add(New LiteralControl("                   </td>"))
            Me.PlaceHolder1.Controls.Add(New LiteralControl("               </tr>"))

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If strVisitL2_4 <> String.Empty Then
                Dim lblReasonVisitL2_4 As New Label()
                Dim dtResL2_4 As DataTable = udtReasonForVisitBLL.getReasonForVisitL2(udtEHSTransaction.ServiceType, CInt(strVisitL1_4), CInt(strVisitL2_4))

                lblReasonVisitL2_4.Text = String.Format("- {0}", dtResL2_4.Rows(0)("Reason_L2"))
                lblReasonVisitL2_4.CssClass = "tableText"

                Me.PlaceHolder1.Controls.Add(New LiteralControl("               <tr>"))
                Me.PlaceHolder1.Controls.Add(New LiteralControl("                   <td style=""padding-left:15px"">"))
                Me.PlaceHolder1.Controls.Add(lblReasonVisitL2_4)
                Me.PlaceHolder1.Controls.Add(New LiteralControl("                   </td>"))
                Me.PlaceHolder1.Controls.Add(New LiteralControl("               </tr>"))
            End If
            ' CRE19-006 (DHC) [End][Winnie]
            Me.PlaceHolder1.Controls.Add(New LiteralControl("           </table>"))
            'Me.PlaceHolder1.Controls.Add(New LiteralControl("<br />"))
        End If

        Me.PlaceHolder1.Controls.Add(New LiteralControl("       </td>"))
        Me.PlaceHolder1.Controls.Add(New LiteralControl("   </tr>"))
        Me.PlaceHolder1.Controls.Add(New LiteralControl("</table>"))

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
        ' No. of Unit Redeemed
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'lblNoOfUnitRedeem.Text = udtEHSTransaction.TransactionDetails(0).Unit.ToString
        'lblTotalAmount.Text = String.Format("(${0})", udtEHSTransaction.TransactionDetails(0).TotalAmount.ToString)
        lblRedeemAmount.Text = udtFormatter.formatMoney(CStr(CInt(udtEHSTransaction.TransactionDetails(0).TotalAmount.Value)), True)
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Display DHC-related Services        
        Dim strDHCService As String = String.Empty
        trDHCRelatedService.Visible = False

        If udtEHSTransaction.SchemeCode.Trim = SchemeClaimModel.HCVS Then
            If Not udtEHSTransaction.DHCService Is Nothing Then
                strDHCService = udtEHSTransaction.DHCService
            End If

            If strDHCService <> String.Empty Then
                Select Case strDHCService
                    Case Common.Component.YesNo.Yes
                        lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "Yes")
                        trDHCRelatedService.Visible = True
                        'CRE20-006 DHC Integration [Nichole]
                        lblDHCDistrictCode.Text = " (" + udtDistrictBoardBLL.GetDistrictNameByDistrictCode(udtEHSTransaction.TransactionAdditionFields.DHC_DistrictCode).DistrictBoard + ")"
                    Case Common.Component.YesNo.No
                        lblDHCRelatedService.Text = Me.GetGlobalResourceObject("Text", "No")
                        trDHCRelatedService.Visible = True

                End Select
            End If
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        ' Control the width of the first column
        tblHCVS.Rows(0).Cells(0).Width = intWidth

    End Sub

    'CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' Helper Functions to handle Transaction Addition Fields
    Public Function Transaction_AdditionFields_GetFieldValueCode(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection, ByVal FieldID As String, Optional ByRef ValueCode As String = Nothing) As Boolean
        ' returns the first found AdditionalFieldValueCode
        Dim blnFound As Boolean = False
        Dim ctr As Integer = 0

        While (Not AdditionalFieldModelCollection Is Nothing AndAlso AdditionalFieldModelCollection.Count > ctr)
            If Not AdditionalFieldModelCollection(ctr) Is Nothing AndAlso AdditionalFieldModelCollection(ctr).AdditionalFieldID = FieldID Then
                blnFound = True
                If Not ValueCode Is Nothing Then
                    ValueCode = AdditionalFieldModelCollection(ctr).AdditionalFieldValueCode
                End If
                Exit While
            End If
            ctr = ctr + 1
        End While
        Return blnFound
    End Function

    Public Function Transaction_AdditionFields_AddField(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection, ByVal FieldID As String, ByVal FieldValueCode As String, ByVal FieldDesc As String) As Integer
        ' add the addition field item and returns the position
        ' if AdditionalFieldModelCollection is nothing, then do nothing and return -1
        Dim Additionfield As TransactionAdditionalFieldModel
        Dim intPos As Integer = 0

        If Not AdditionalFieldModelCollection Is Nothing Then
            Additionfield = New TransactionAdditionalFieldModel()
            Additionfield.AdditionalFieldID = FieldID
            Additionfield.AdditionalFieldValueCode = FieldValueCode
            Additionfield.AdditionalFieldValueDesc = FieldDesc
            intPos = AdditionalFieldModelCollection.Count
            AdditionalFieldModelCollection.Add(Additionfield)
            Return intPos
        Else
            Return -1
        End If
    End Function

    Public Function Transaction_AdditionFields_AddorSetValue(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection, ByVal FieldID As String, ByVal FieldValueCode As String, ByVal FieldDesc As String) As Integer
        ' locate the addition field item in the addition field model collection;
        ' if found, set the value and description to the existing found field model, and return the found position;
        ' if not found, then create a new field model
        Dim Additionfield As TransactionAdditionalFieldModel
        Dim blnFound As Boolean = False
        Dim ctr As Integer = 0

        While (Not AdditionalFieldModelCollection Is Nothing AndAlso AdditionalFieldModelCollection.Count > ctr)
            If Not AdditionalFieldModelCollection(ctr) Is Nothing AndAlso AdditionalFieldModelCollection(ctr).AdditionalFieldID = FieldID Then
                blnFound = True
                AdditionalFieldModelCollection(ctr).AdditionalFieldValueCode = FieldValueCode
                AdditionalFieldModelCollection(ctr).AdditionalFieldValueDesc = FieldDesc
                Return ctr
            End If
            ctr = ctr + 1
        End While

        Additionfield = New TransactionAdditionalFieldModel()
        Additionfield.AdditionalFieldID = FieldID
        Additionfield.AdditionalFieldValueCode = FieldValueCode
        Additionfield.AdditionalFieldValueDesc = FieldDesc
        AdditionalFieldModelCollection.Add(Additionfield)
        Return ctr

    End Function

    Public Function Transaction_AdditionFields_AddorSetValueFull(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection, ByVal FieldID As String, ByVal FieldValueCode As String, ByVal FieldDesc As String, ByVal SchemeCode As String, ByVal SchemeSeq As Integer, ByVal SubsidizeCode As String) As Integer
        ' locate the addition field item in the addition field model collection;
        ' if found, set the value, description, scheme code, scheme sequence and subsidize code to the existing found field model, and return the found position;
        ' if not found, then create a new field model
        Dim Additionfield As TransactionAdditionalFieldModel
        Dim blnFound As Boolean = False
        Dim ctr As Integer = 0

        While (Not AdditionalFieldModelCollection Is Nothing AndAlso AdditionalFieldModelCollection.Count > ctr)
            If Not AdditionalFieldModelCollection(ctr) Is Nothing AndAlso AdditionalFieldModelCollection(ctr).AdditionalFieldID = FieldID Then
                blnFound = True
                AdditionalFieldModelCollection(ctr).AdditionalFieldValueCode = FieldValueCode
                AdditionalFieldModelCollection(ctr).AdditionalFieldValueDesc = FieldDesc
                AdditionalFieldModelCollection(ctr).SchemeCode = SchemeCode
                AdditionalFieldModelCollection(ctr).SchemeSeq = SchemeSeq
                AdditionalFieldModelCollection(ctr).SubsidizeCode = SubsidizeCode
                Return ctr
            End If
            ctr = ctr + 1
        End While

        Additionfield = New TransactionAdditionalFieldModel()
        Additionfield.AdditionalFieldID = FieldID
        Additionfield.AdditionalFieldValueCode = FieldValueCode
        Additionfield.AdditionalFieldValueDesc = FieldDesc
        Additionfield.SchemeCode = SchemeCode
        Additionfield.SchemeSeq = SchemeSeq
        Additionfield.SubsidizeCode = SubsidizeCode
        AdditionalFieldModelCollection.Add(Additionfield)
        Return ctr

    End Function

    Public Function Transaction_AdditionFields_SetValue(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection, ByVal FieldID As String, ByVal FieldValueCode As String, ByVal FieldDesc As String) As Boolean
        ' set FieldValueCode and FieldDesc found in the addition field model collection
        Dim blnFound As Boolean = False
        Dim ctr As Integer = 0

        While (Not AdditionalFieldModelCollection Is Nothing AndAlso AdditionalFieldModelCollection.Count > ctr)
            If Not AdditionalFieldModelCollection(ctr) Is Nothing AndAlso AdditionalFieldModelCollection(ctr).AdditionalFieldID = FieldID Then
                blnFound = True
                AdditionalFieldModelCollection(ctr).AdditionalFieldValueCode = FieldValueCode
                AdditionalFieldModelCollection(ctr).AdditionalFieldValueDesc = FieldDesc
                Exit While
            End If
            ctr = ctr + 1
        End While
        Return blnFound
    End Function


    Public Function Transaction_AdditionFields_RemoveField(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection, ByVal FieldID As String) As Boolean
        ' removes all additional fields with the FieldID
        Dim blnFound As Boolean = False
        Dim ctr As Integer = 0

        While (Not AdditionalFieldModelCollection Is Nothing AndAlso AdditionalFieldModelCollection.Count > ctr)
            If Not AdditionalFieldModelCollection(ctr) Is Nothing AndAlso AdditionalFieldModelCollection(ctr).AdditionalFieldID = FieldID Then
                blnFound = True
                AdditionalFieldModelCollection.RemoveAt(ctr)
            End If
            ctr = ctr + 1
        End While
        Return blnFound
    End Function

    Public Function Transaction_AdditionFields_RemoveAllReasonForVisits(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection) As Boolean
        ' removes all additional fields with the FieldID the same as any of the Reason For Visit field ID defined
        ' returns true if any of reason for visit is found and removed; false otherwise
        Dim blnFound As Boolean = False
        Dim arrReasonForVisitL1 As String() = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1()
        Dim arrReasonForVisitL2 As String() = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2()
        Dim strControlName As String

        For Each strControlName In arrReasonForVisitL1
            If Transaction_AdditionFields_RemoveField(AdditionalFieldModelCollection, strControlName) Then
                blnFound = True
            End If
        Next

        For Each strControlName In arrReasonForVisitL2
            If Transaction_AdditionFields_RemoveField(AdditionalFieldModelCollection, strControlName) Then
                blnFound = True
            End If
        Next
        Return blnFound
    End Function

    Public Function Transaction_AdditionFields_GetPos(ByRef AdditionalFieldModelCollection As EHSTransaction.TransactionAdditionalFieldModelCollection, ByVal FieldID As String) As Integer
        ' returns the position of the FieldID matched; returns -1 if not found
        Dim ctr As Integer = 0

        While (Not AdditionalFieldModelCollection Is Nothing AndAlso AdditionalFieldModelCollection.Count > ctr)
            If Not AdditionalFieldModelCollection(ctr) Is Nothing AndAlso AdditionalFieldModelCollection(ctr).AdditionalFieldID = FieldID Then
                Exit While
            End If
            ctr = ctr + 1
        End While
        If ctr = AdditionalFieldModelCollection.Count Then
            Return -1
        End If
        Return ctr
    End Function

    'CRE11-024-02 HCVS Pilot Extension Part 2 [End]

End Class
