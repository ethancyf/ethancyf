Imports Common.Component.EHSAccount
Imports Common.Component.ReasonForVisit
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.ComObject

Namespace UIControl.EHCClaimText
    Partial Public Class ucReasonForVisit
        Inherits ucInputEHSClaimBase

        Private Const ConfirmedValueAttribute As String = "confirmedvalue"

        Private Class DataRowName
            Public Const Reason_L1_Code As String = "Reason_L1_Code"
            Public Const Reason_L1 As String = "Reason_L1"
            Public Const Reason_L1_Chi As String = "Reason_L1_Chi"
            Public Const Reason_L2_Code As String = "Reason_L2_Code"
            Public Const Reason_L2 As String = "Reason_L2"
            Public Const Reason_L2_Chi As String = "Reason_L2_Chi"
        End Class

        Private Enum ActiveViewIndex As Short

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            SelectReasonForVisitGroup = 0
            SelectReasonForVisitL1 = 1
            SelectReasonForVisitL2 = 2
            SelectReasonForVisitGroupDelete = 3

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        End Enum

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Public Enum EnumMode
            BeforeCopaymentFeeEnabled
            AfterCopaymentFeeEnabled
        End Enum

        'Public Const FunctCode As String = Common.Component.FunctCode.FUNT020202

        Private FunctCode As String
        Public Property FunctionCode() As String
            Get
                Return FunctCode
            End Get
            Set(ByVal value As String)
                FunctCode = value
            End Set
        End Property

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Public Event ReturnButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Public Event ConfirmButtonClick(ByVal sender As Object, ByVal e As EventArgs)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Public Event ReturnButtonClickForCTM(ByVal sender As Object, ByVal e As EventArgs)
        Public Event ConfirmButtonClickForCTM(ByVal sender As Object, ByVal e As EventArgs)

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Public Sub Build()
            Me.Setup()
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Public Property ActionMode() As EnumMode
            Get
                Return Me.ViewState("ActionMode")
            End Get
            Set(ByVal value As EnumMode)
                Me.ViewState("ActionMode") = value
            End Set
        End Property

        Public Sub Mode(ByVal enumMode As EnumMode)
            Me.ActionMode = enumMode
            ClearReasonForVisitGroup()
            ClearErrorMessage()
            Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitGroup
        End Sub

        Protected Overrides Sub Setup()

            Me.lblRFVPrincipalText.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")
            Me.lblRFVSecondaryText.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")

            Dim ReasonForVisitGroupSelectedIndex As Integer = 0

            If MyBase.CurrentPractice Is Nothing And Me.FunctionCode = Common.Component.FunctCode.FUNT020202 Then
                If Common.Component.FunctCode.FUNT020303 And Me.SessionHandler.EHSTransactionGetFromSession(FunctCode) IsNot Nothing Then
                    Return
                End If
            End If

            Select Case Me.mvHCVS.ActiveViewIndex
                Case ActiveViewIndex.SelectReasonForVisitGroup
                    Me.BindReasonForVisitGroup()
                Case ActiveViewIndex.SelectReasonForVisitL1
                    Me.BindReasonForVisitFirst()
                Case ActiveViewIndex.SelectReasonForVisitL2
                    Me.BindReasonForVisitSecond()
                Case ActiveViewIndex.SelectReasonForVisitGroupDelete
                    Me.BindReasonForVisitGroupDelete()
            End Select
        End Sub

        Private Sub ClearErrorMessage()
            Me.udcMsgBoxErr.Clear()
            Me.lblRFVPrincipalContentError.Visible = False
            Me.lblRFVSecondary1ContentError.Visible = False
            Me.lblRFVSecondary2ContentError.Visible = False
            Me.lblRFVSecondary3ContentError.Visible = False
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Public Property ReasonForVisitFirst() As String
            Get
                Return Me.rbSelectReasonForVisitL1.SelectedValue
            End Get
            Set(ByVal value As String)
                Me.rbSelectReasonForVisitL1.SelectedValue = value
                Me.rbSelectReasonForVisitL1.Attributes(ConfirmedValueAttribute) = value
                HandleReasonForVisitFirstChanged(True)
            End Set
        End Property

        Public Property ReasonForVisitSecond() As String
            Get
                Return Me.rbSelectReasonForVisitL2.SelectedValue
            End Get
            Set(ByVal value As String)
                Me.rbSelectReasonForVisitL2.SelectedValue = value
                Me.rbSelectReasonForVisitL2.Attributes(ConfirmedValueAttribute) = value
            End Set
        End Property

        ' Event
        Protected Sub mvHCVS_ActiveViewChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mvHCVS.ActiveViewChanged
            Me.udcMsgBoxErr.Clear()
        End Sub

        ' Overrides
        Public Overrides Sub Clear()
            'MyBase.Clear()

            Me.rbSelectReasonForVisitL1.ClearSelection()
            Me.rbSelectReasonForVisitL1.Attributes(ConfirmedValueAttribute) = String.Empty
            Me.rbSelectReasonForVisitL2.ClearSelection()
            Me.rbSelectReasonForVisitL2.Attributes(ConfirmedValueAttribute) = String.Empty
        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

#Region "Step of vSelectReasonForVisitGroup"
        ' Event

        Protected Sub btnSelectReasonForVisitGroupBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitGroupBack.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00001, "Select Reason For Visit Group Back Click")

            ClearErrorMessage()
            Me.RollbackUpdate()

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Dim no As Integer = SessionHandler.EHSTransactionGetFromSession(FunctionCode).TransactionAdditionFields.ReasonForVisitDuplicated

            Dim isValid As Boolean = True
            If no >= 0 Then
                isValid = False
                Me.udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00312"))
                Me.udcMsgBoxErr.BuildMessageBox()

                If no = 0 Then
                    Me.lblRFVPrincipalContentError.Visible = True
                End If

                If no = 1 Then
                    Me.lblRFVSecondary1ContentError.Visible = True
                End If

                If no = 2 Then
                    Me.lblRFVSecondary2ContentError.Visible = True
                End If

                If no = 3 Then
                    Me.lblRFVSecondary3ContentError.Visible = True
                End If

            Else
                SessionHandler.EHSTransactionGetFromSession(FunctionCode).TransactionAdditionFields.SortReasonForVisit()
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            If isValid Then
                If FunctionCode = Common.Component.FunctCode.FUNT020303 Then
                    RaiseEvent ReturnButtonClickForCTM(sender, e)
                Else
                    RaiseEvent ReturnButtonClick(sender, e)
                End If
            End If

        End Sub

        Protected Sub btnSelectReasonForVisitGroupAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitGroupAdd.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00002, "Select Reason For Visit Group Add Click")

            If Me.AllowReasonForVisitAdd Then
                Me.ClearReasonForVisitGroup()
                Me.BindReasonForVisitFirst()
                Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitL1
            Else

            End If
        End Sub

        Protected Sub btnSelectReasonForVisitGroupEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitGroupEdit.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00003, "Select Reason For Visit Group Edit Click")

            If Me.AllowReasonForVisitEdit Then
                Me.BindReasonForVisitFirst()
                LoadReasonForVisitL1FromSession(Me.RFVSelectedIndex)
                Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitL1
            Else

            End If
        End Sub

        Protected Sub btnSelectReasonForVisitGroupDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitGroupDelete.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00004, "Select Reason For Visit Group Delete Click")

            If Me.AllowReasonForVisitDelete Then
                BindReasonForVisitGroupDelete()
                Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitGroupDelete
            Else

            End If
        End Sub

        Public Sub LoadReasonForVisitL1FromSession(ByVal index As Integer)
            If index >= 0 Then
                If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode) IsNot Nothing Then
                    If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount > 0 Then
                        Me.rbSelectReasonForVisitL1.SelectedValue = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(index).AdditionalFieldValueCode
                    End If
                End If
            End If
        End Sub

        Public Sub LoadReasonForVisitL2FromSession(ByVal index As Integer)
            If index >= 0 Then
                If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode) IsNot Nothing Then
                    If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount > 0 Then
                        If Me.rbSelectReasonForVisitL1.SelectedValue = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(index).AdditionalFieldValueCode Then
                            Me.rbSelectReasonForVisitL2.SelectedValue = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(index).AdditionalFieldValueCode
                        End If
                    End If
                End If
            End If
        End Sub

#End Region

        Function AllowReasonForVisitAdd() As Boolean
            ClearErrorMessage()
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim AllowAdd As Boolean = True
            Dim ReasonForVisitUpperLimit As Integer = 0
            If Me.ViewState("ActionMode") = EnumMode.BeforeCopaymentFeeEnabled Then
                ReasonForVisitUpperLimit = 1
            End If
            If Me.ViewState("ActionMode") = EnumMode.AfterCopaymentFeeEnabled Then
                ReasonForVisitUpperLimit = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length
            End If
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount = ReasonForVisitUpperLimit Then
                AllowAdd = False
                'Error Message (TBC)
                Dim udtSystemMessage As Common.ComObject.SystemMessage = New Common.ComObject.SystemMessage("020202", "E", "00014")
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If
            Return AllowAdd
        End Function

        Function AllowReasonForVisitEdit() As Boolean
            ClearErrorMessage()
            Dim AllowEdit As Boolean = True
            If Not Me.RFVSelected Then
                AllowEdit = False
                'Error Message (TBC)
                Dim udtSystemMessage As Common.ComObject.SystemMessage = New Common.ComObject.SystemMessage("020202", "E", "00022")
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If
            Return AllowEdit
        End Function

        Function AllowReasonForVisitDelete() As Boolean
            ClearErrorMessage()
            Dim AllowDelete As Boolean = True
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount > 1 And Me.rdoRFVPrincipal.Checked Then
                AllowDelete = False
                'Error Message (TBC)
                Dim udtSystemMessage As Common.ComObject.SystemMessage = New Common.ComObject.SystemMessage("020202", "E", "00020")
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If
            If Not Me.RFVSelected Then
                AllowDelete = False
                'Error Message (TBC)
                Dim udtSystemMessage As Common.ComObject.SystemMessage = New Common.ComObject.SystemMessage("020202", "E", "00021")
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If
            Return AllowDelete
        End Function

#Region "Step of vSelectReasonForVisitGroupDelete"
        ' Event

        Protected Sub btnSelectReasonForVisitGroupDeleteCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitGroupDeleteCancel.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00009, "Select Reason For Visit Group Delete Cancel Click")

            Me.ReturnToRFVGroup()
        End Sub

        Protected Sub btnSelectReasonForVisitGroupDeleteConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitGroupDeleteConfirm.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00010, "Select Reason For Visit Group Delete Confirm Click")

            Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.RemoveReasonForVisit(Me.RFVSelectedIndex)
            Me.ReturnToRFVGroup()
        End Sub

        Private Sub HandleReturn(ByVal sender As System.Object, ByVal e As System.EventArgs)
            'If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount > 0 Then
            '    Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitGroup
            'Else
            If FunctionCode = Common.Component.FunctCode.FUNT020303 Then
                'RaiseEvent ReturnButtonClickForCTM(sender, e)
                Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitGroup
            Else
                RaiseEvent ReturnButtonClick(sender, e)
            End If
            'End If
        End Sub

#End Region

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#Region "Step of vSelectReasonForVisitL1"

        ' Event
        Protected Sub btnSelectReasonForVisitL1Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitL1Back.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00005, "Select Reason For Visit L1 Back Click")

            Me.RollbackUpdate()

            'If FunctionCode = Common.Component.FunctCode.FUNT020303 Then
            '    RaiseEvent ReturnButtonClickForCTM(sender, e)
            'Else
            '    RaiseEvent ReturnButtonClick(sender, e)
            'End If

            Me.ReturnToRFVGroup()

        End Sub


        Protected Sub btnSelectReasonForVisitL1Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitL1Next.Click
            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00006, "Select Reason For Visit L1 Confirm Click")

            Dim intL1Code As Integer = 0
            Dim intL2Code As Integer = 0
            Dim udtReasonForVisitL2 As TransactionAdditionalFieldModel = Nothing

            Dim udtEHSTransaction As EHSTransactionModel = Nothing
            Dim intReasonForVisitGroupSelectedIndex As Integer
            intReasonForVisitGroupSelectedIndex = Me.RFVSelectedIndex

            udtEHSTransaction = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode)

            If intReasonForVisitGroupSelectedIndex >= 0 Then
                intL1Code = udtEHSTransaction.TransactionAdditionFields.ReasonForVisitL1(intReasonForVisitGroupSelectedIndex).AdditionalFieldValueCode

                udtReasonForVisitL2 = udtEHSTransaction.TransactionAdditionFields.ReasonForVisitL2(intReasonForVisitGroupSelectedIndex)
                If udtReasonForVisitL2 IsNot Nothing Then
                    intL2Code = udtReasonForVisitL2.AdditionalFieldValueCode
                End If
            End If

            If ValidateReasonForVisitL1() Then
                ' Bind Level 2
                If Me.rbSelectReasonForVisitL1.SelectedItem IsNot Nothing Then
                    If Me.rbSelectReasonForVisitL1.SelectedValue = intL1Code Then
                        If intL2Code >= 0 Then
                            BindReasonForVisitSecond()
                            Me.rbSelectReasonForVisitL2.SelectedValue = intL2Code
                        End If
                        HandleReasonForVisitFirstChanged(False)
                    Else
                        HandleReasonForVisitFirstChanged(True)
                    End If
                End If

                Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitL2

            End If

        End Sub

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Protected Sub btnSelectReasonForVisitL1Confirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitL1Confirm.Click
            ' Without Level 2
            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00006, "Select Reason For Visit L1 Confirm Click")

            If ValidateReasonForVisitL1() Then
                Me.CommitUpdate()
                Me.ReturnToRFVGroup()
            End If

        End Sub
        ' CRE19-006 (DHC) [End][Winnie]


        Protected Sub rbSelectReasonForVisitL1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSelectReasonForVisitL1.SelectedIndexChanged
            HandleReasonForVisitFirstChanged(True)
        End Sub

        Private Sub HandleReasonForVisitFirstChanged(ByVal blnClearSelection As Boolean)
            ' Clear Reason for Visit L2
            If blnClearSelection Then
                Me.rbSelectReasonForVisitL2.ClearSelection()
                Me.rbSelectReasonForVisitL2.Items.Clear()
            End If

            Me.BindReasonForVisitSecond()
        End Sub

        ' Function
        Private Function ValidateReasonForVisitL1() As Boolean
            Dim isValid As Boolean = True

            If String.IsNullOrEmpty(Me.rbSelectReasonForVisitL1.SelectedValue) Then
                isValid = False
                Dim udtSystemMessage As Common.ComObject.SystemMessage = New Common.ComObject.SystemMessage("020202", "E", "00014")
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If

            Return isValid

        End Function

        Private Function ValidateReasonForVisitGroup() As Boolean
            Dim isValid As Boolean = True

            If Not Me.RFVSelected Then
                isValid = False
                Dim udtSystemMessage As Common.ComObject.SystemMessage = New Common.ComObject.SystemMessage("020202", "E", "00014")
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If

            Return isValid

        End Function

#End Region


#Region "Step of vSelectReasonForVisitL2"

        ' Event
        Protected Sub btnSelectReasonForVisitL2Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitL2Back.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00007, "Select Reason For Visit L2 Back Click")

            Me.BindReasonForVisitFirst()
            Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitL1
        End Sub

        Protected Sub btnSelectReasonForVisitL2Confirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectReasonForVisitL2Confirm.Click

            AuditLogSelectReasonForVisitButton(Common.Component.FunctCode.FUNT020304, Common.Component.LogID.LOG00008, "Select Reason For Visit L2 Confirm Click")

            If ValidateReasonForVisitL2() Then
                Me.CommitUpdate()

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------

                Me.ReturnToRFVGroup()

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            End If

        End Sub

        ' Function
        Private Function ValidateReasonForVisitL2() As Boolean
            Dim isValid As Boolean = True

            If String.IsNullOrEmpty(Me.rbSelectReasonForVisitL2.SelectedValue) Then
                isValid = False
                Dim udtSystemMessage As Common.ComObject.SystemMessage = New Common.ComObject.SystemMessage("020202", "E", "00014")
                Me.udcMsgBoxErr.AddMessage(udtSystemMessage)
                Me.udcMsgBoxErr.BuildMessageBox()
            End If

            Return isValid

        End Function

        Sub ReturnToRFVGroup()
            Me.ClearReasonForVisitGroup()
            Me.BindReasonForVisitGroup()
            ShowRFVTable()
            Me.mvHCVS.ActiveViewIndex = ActiveViewIndex.SelectReasonForVisitGroup
        End Sub

        Function IsFromCTM() As Boolean
            Dim IsCTM As Boolean = False
            If FunctCode = Common.Component.FunctCode.FUNT020303 Then
                IsCTM = True
            End If
            Return IsCTM
        End Function

#End Region

#Region "Reason for visit setup"

        'Commit Changes
        Private Sub CommitUpdate()

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            'If IsEdit Then
            '    Me.rbSelectReasonForVisitL1.Attributes(ConfirmedValueAttribute) = Me.rbSelectReasonForVisitL1.SelectedValue
            '    Me.rbSelectReasonForVisitL2.Attributes(ConfirmedValueAttribute) = Me.rbSelectReasonForVisitL2.SelectedValue
            'End If

            Dim udtSchemeClaim As SchemeClaimModel = Me.SessionHandler.SchemeSelectedGetFromSession(FunctCode)
            Dim udtEHSTransaction As EHSTransactionModel = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode)

            Dim strSchemeCode As String = String.Empty
            Dim strSchemeSeq As String = String.Empty
            Dim strSubsidizeCode As String = String.Empty

            If Me.FunctionCode = Common.Component.FunctCode.FUNT020202 Then
                strSchemeCode = udtSchemeClaim.SchemeCode.Trim
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'strSchemeSeq = udtSchemeClaim.SchemeSeq
                strSchemeSeq = udtSchemeClaim.SubsidizeGroupClaimList(0).SchemeSeq
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                strSubsidizeCode = udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode.Trim
            Else
                If udtEHSTransaction.TransactionAdditionFields Is Nothing Then udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
                Dim udtDetail As TransactionDetailModel = Nothing
                For Each udtTemp As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                    If udtTemp.SchemeCode.Trim = udtEHSTransaction.SchemeCode.Trim Then
                        udtDetail = udtTemp
                        Exit For
                    End If
                Next
                strSchemeCode = udtDetail.SchemeCode.Trim
                strSchemeSeq = udtDetail.SchemeSeq
                strSubsidizeCode = udtDetail.SubsidizeCode.Trim
            End If

            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel

            Dim intCount As Integer
            intCount = udtEHSTransaction.TransactionAdditionFields.ReasonForVisitCount

            If intCount <= Common.Component.EHSTransaction.TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length Then

                If Me.RFVSelected Then
                    Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(Me.RFVSelectedIndex).AdditionalFieldValueCode = Me.rbSelectReasonForVisitL1.SelectedValue

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If rbSelectReasonForVisitL2.SelectedItem IsNot Nothing Then
                        Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(Me.RFVSelectedIndex).AdditionalFieldValueCode = Me.rbSelectReasonForVisitL2.SelectedValue
                    End If
                    ' CRE19-006 (DHC) [End][Winnie]
                Else
                    ' Reason For Visit Level1
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1(intCount)
                    udtTransactAdditionfield.AdditionalFieldValueCode = Me.rbSelectReasonForVisitL1.SelectedValue
                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                    udtTransactAdditionfield.SchemeCode = strSchemeCode
                    udtTransactAdditionfield.SchemeSeq = strSchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = strSubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    ' Reason For Visit Level2
                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If rbSelectReasonForVisitL2.SelectedItem IsNot Nothing Then
                        udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                        udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL2(intCount)
                        udtTransactAdditionfield.AdditionalFieldValueCode = Me.rbSelectReasonForVisitL2.SelectedValue
                        udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                        udtTransactAdditionfield.SchemeCode = strSchemeCode
                        udtTransactAdditionfield.SchemeSeq = strSchemeSeq
                        udtTransactAdditionfield.SubsidizeCode = strSubsidizeCode

                        udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                    End If
                    ' CRE19-006 (DHC) [End][Winnie]
                End If
            End If

            Me.Clear()

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        End Sub

        'Cancel all the update
        Private Sub RollbackUpdate()
            If String.IsNullOrEmpty(Me.rbSelectReasonForVisitL1.Attributes(ConfirmedValueAttribute)) Then
                Me.rbSelectReasonForVisitL1.ClearSelection()
            Else
                Me.rbSelectReasonForVisitL1.SelectedValue = Me.rbSelectReasonForVisitL1.Attributes(ConfirmedValueAttribute)
            End If

            BindReasonForVisitSecond()

            If String.IsNullOrEmpty(Me.rbSelectReasonForVisitL2.Attributes(ConfirmedValueAttribute)) Then
                Me.rbSelectReasonForVisitL2.ClearSelection()
            Else
                Me.rbSelectReasonForVisitL2.SelectedValue = Me.rbSelectReasonForVisitL2.Attributes(ConfirmedValueAttribute)
            End If

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Public Sub ClearReasonForVisitGroup()
            Me.ResetRFV()
        End Sub

        Private Sub BindReasonForVisitGroupDelete()

            Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
            Dim dtL1 As DataTable = Nothing
            Dim dtL2 As DataTable = Nothing
            Dim dt As DataTable = Nothing
            Dim dr As DataRow = Nothing

            Dim strL1Code As String = String.Empty
            Dim strL2Code As String = String.Empty

            Dim i As Integer = Me.RFVSelectedIndex

            If i >= 0 Then
                If Not Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(i) Is Nothing Then
                    strL1Code = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(i).AdditionalFieldValueCode
                End If

                If Not Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(i) Is Nothing Then
                    strL2Code = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(i).AdditionalFieldValueCode
                End If

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If MyBase.CurrentPractice IsNot Nothing Then
                    dtL1 = udtReasonforVisitBLL.getReasonForVisitL1(MyBase.CurrentPractice.ServiceCategoryCode, strL1Code)
                    dtL2 = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, strL1Code, IIf(strL2Code = String.Empty, 0, strL2Code))
                Else
                    dtL1 = udtReasonforVisitBLL.getReasonForVisitL1(Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType, strL1Code)
                    dtL2 = udtReasonforVisitBLL.getReasonForVisitL2(Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType, strL1Code, IIf(strL2Code = String.Empty, 0, strL2Code))
                End If
                ' CRE19-006 (DHC) [End][Winnie]

                If i = 0 Then
                    lblRFVDeleteText.Text = Me.GetGlobalResourceObject("Text", "PrincipalReasonForVisit")
                Else
                    lblRFVDeleteText.Text = Me.GetGlobalResourceObject("Text", "SecondaryReasonForVisit")
                End If

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                    Me.lblRFVDeleteL1.Text = dtL1.Rows(0)(DataRowName.Reason_L1_Chi)
                    If dtL2.Rows.Count > 0 Then Me.lblRFVDeleteL2.Text = "- ".ToString + dtL2.Rows(0)(DataRowName.Reason_L2_Chi)
                Else
                    Me.lblRFVDeleteL1.Text = dtL1.Rows(0)(DataRowName.Reason_L1)
                    If dtL2.Rows.Count > 0 Then Me.lblRFVDeleteL2.Text = "- ".ToString + dtL2.Rows(0)(DataRowName.Reason_L2)
                End If
                ' CRE19-006 (DHC) [End][Winnie]
            End If
        End Sub

        Private Sub BindReasonForVisitGroup()

            Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
            Dim dtL1 As DataTable = Nothing
            Dim dtL2 As DataTable = Nothing
            Dim dt As DataTable = Nothing
            Dim dr As DataRow = Nothing

            Dim strL1Code As String = String.Empty
            Dim strL2Code As String = String.Empty

            Dim L1Text As String = String.Empty
            Dim L2Text As String = String.Empty

            If Not Me.SessionHandler.EHSTransactionGetFromSession(FunctCode) Is Nothing Then
                If Not Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields Is Nothing Then

                    'Me.rbSelectReasonForVisitGroup.Items.Clear()

                    If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount = 0 Then
                        Me.lblNoReasonForVisit.Visible = True
                        Me.tblRFV.Visible = False
                    Else
                        Me.lblNoReasonForVisit.Visible = False
                        Me.tblRFV.Visible = True
                        Me.trRFVPrincipal.Visible = False
                        Me.trRFVSecondary.Visible = False
                        Me.trRFVPrincipalContent.Visible = False
                        Me.trRFVSecondary1Content.Visible = False
                        Me.trRFVSecondary2Content.Visible = False
                        Me.trRFVSecondary3Content.Visible = False

                        For i As Integer = 0 To Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount - 1

                            If Not Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(i) Is Nothing Then
                                strL1Code = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL1(i).AdditionalFieldValueCode
                            End If

                            If Not Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(i) Is Nothing Then
                                strL2Code = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitL2(i).AdditionalFieldValueCode
                            End If

                            ' CRE19-006 (DHC) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            If MyBase.CurrentPractice IsNot Nothing Then
                                dtL1 = udtReasonforVisitBLL.getReasonForVisitL1(MyBase.CurrentPractice.ServiceCategoryCode, strL1Code)
                                dtL2 = udtReasonforVisitBLL.getReasonForVisitL2(MyBase.CurrentPractice.ServiceCategoryCode, strL1Code, IIf(strL2Code = String.Empty, 0, strL2Code))
                            Else
                                dtL1 = udtReasonforVisitBLL.getReasonForVisitL1(Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType, strL1Code)
                                dtL2 = udtReasonforVisitBLL.getReasonForVisitL2(Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType, strL1Code, IIf(strL2Code = String.Empty, 0, strL2Code))
                            End If

                            If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                                L1Text = dtL1.Rows(0)(DataRowName.Reason_L1_Chi)
                            Else
                                L1Text = dtL1.Rows(0)(DataRowName.Reason_L1)
                            End If

                            If dtL2.Rows.Count > 0 Then
                                If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                                    L2Text = dtL2.Rows(0)(DataRowName.Reason_L2_Chi)
                                Else
                                    L2Text = dtL2.Rows(0)(DataRowName.Reason_L2)
                                End If

                                L2Text = FormatRFVL2(L2Text)
                            End If
                            ' CRE19-006 (DHC) [End][Winnie]

                            If i = 0 Then
                                Me.trRFVPrincipalContent.Visible = True
                                Me.trRFVPrincipal.Visible = True
                                Me.rdoRFVPrincipal.Text = L1Text + L2Text
                            End If
                            If i = 1 Then
                                Me.trRFVSecondary1Content.Visible = True
                                Me.trRFVSecondary.Visible = True
                                Me.rdoRFVSecondary1.Text = L1Text + L2Text
                            End If
                            If i = 2 Then
                                Me.trRFVSecondary2Content.Visible = True
                                Me.trRFVSecondary.Visible = True
                                Me.rdoRFVSecondary2.Text = L1Text + L2Text
                            End If
                            If i = 3 Then
                                Me.trRFVSecondary3Content.Visible = True
                                Me.trRFVSecondary.Visible = True
                                Me.rdoRFVSecondary3.Text = L1Text + L2Text
                            End If

                        Next
                        ShowRFVTable()
                    End If
                End If
            End If

        End Sub

        Private Function FormatRFVL2(ByVal L2 As String) As String
            Return "<br/>- ".ToString + L2
        End Function

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]


        'First Reason for Visit
        Private Sub BindReasonForVisitFirst()
            Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL
            Dim dtRes As DataTable = Nothing

            Dim strSelectedValue As String
            strSelectedValue = Me.rbSelectReasonForVisitL1.SelectedValue


            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim strServiceType As String = String.Empty

            If MyBase.CurrentPractice IsNot Nothing Then
                strServiceType = MyBase.CurrentPractice.ServiceCategoryCode
            Else
                strServiceType = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType
            End If

            dtRes = udtReasonforVisitBLL.getReasonForVisitL1(strServiceType)
            ' CRE19-006 (DHC) [End][Winnie]

            If dtRes IsNot Nothing Then
                Me.rbSelectReasonForVisitL1.DataSource = dtRes
                If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                    Me.rbSelectReasonForVisitL1.DataTextField = DataRowName.Reason_L1_Chi
                Else
                    Me.rbSelectReasonForVisitL1.DataTextField = DataRowName.Reason_L1
                End If
                Me.rbSelectReasonForVisitL1.DataValueField = DataRowName.Reason_L1_Code
                Me.rbSelectReasonForVisitL1.DataBind()
                'Me.rbSelectReasonForVisitL1.Enabled = IIf(MyBase.AvaliableForClaim, True, False)
            End If


            If Not String.IsNullOrEmpty(strSelectedValue) Then
                Me.rbSelectReasonForVisitL1.SelectedValue = strSelectedValue
            Else
                If Me.RFVSelected Then
                    Me.LoadReasonForVisitL1FromSession(Me.RFVSelectedIndex)
                End If
            End If

            lblSelectReasonForVisitL1Header.Text = Me.RFVL1Text
            lblSelectReasonForVisitL2Header.Text = Me.RFVL2Text


            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Check level 2 exist
            Dim dtSecondReasonForVisit As DataTable = udtReasonforVisitBLL.getReasonForVisitL2(strServiceType)

            If dtSecondReasonForVisit.Rows.Count > 0 Then
                ' with level 2
                btnSelectReasonForVisitL1Next.Visible = True
                btnSelectReasonForVisitL1Confirm.Visible = False
            Else
                btnSelectReasonForVisitL1Next.Visible = False
                btnSelectReasonForVisitL1Confirm.Visible = True
            End If
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        Function RFVL1Text() As String

            Dim strRFVL1Text As String = String.Empty

            If Me.RFVSelected Then

                'Edit Reason For Visit

                'Primary Reason For Visit
                If Me.RFVSelectedIndex = 0 Then
                    strRFVL1Text = Me.GetGlobalResourceObject("Text", "PrincipalRFV")
                End If

                'Secondary Reason For Visit
                If Me.RFVSelectedIndex > 0 Then
                    strRFVL1Text = Me.GetGlobalResourceObject("Text", "SecondaryRFV")
                End If

            Else

                'Add Reason For Visit
                If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount = 0 Then
                    'Primary Reason For Visit
                    strRFVL1Text = Me.GetGlobalResourceObject("Text", "PrincipalRFV")
                Else
                    'Secondary Reason For Visit
                    strRFVL1Text = Me.GetGlobalResourceObject("Text", "SecondaryRFV")
                End If

            End If

            Return strRFVL1Text

        End Function

        Function RFVL2Text() As String
            Return Me.RFVL1Text
        End Function


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        'Second Reason for Visit
        Private Sub BindReasonForVisitSecond()

            Dim strServiceCategoryCode As String
            If MyBase.CurrentPractice IsNot Nothing Then
                strServiceCategoryCode = MyBase.CurrentPractice.ServiceCategoryCode
            Else
                strServiceCategoryCode = Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceType
            End If


            Dim strSelectedValue As String
            strSelectedValue = Me.rbSelectReasonForVisitL2.SelectedValue

            Dim udtReasonforVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL

            Dim dtReasonForVisitL1 As DataTable
            Dim dtReasonForVisitL2 As DataTable

            Dim intReasonForVisitL1 As Integer = -1

            lblSelectReasonForVisitL1Value.Text = String.Empty

            If Not String.IsNullOrEmpty(Me.rbSelectReasonForVisitL1.SelectedValue) Then
                intReasonForVisitL1 = CInt(Me.rbSelectReasonForVisitL1.SelectedValue)
                dtReasonForVisitL1 = udtReasonforVisitBLL.getReasonForVisitL1(strServiceCategoryCode, intReasonForVisitL1)

                If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                    lblSelectReasonForVisitL1Value.Text = dtReasonForVisitL1.Rows(0)(DataRowName.Reason_L1_Chi)
                Else
                    lblSelectReasonForVisitL1Value.Text = dtReasonForVisitL1.Rows(0)(DataRowName.Reason_L1)
                End If
            End If

            If intReasonForVisitL1 > 0 Then
                If MyBase.AvaliableForClaim Or FunctCode = Common.Component.FunctCode.FUNT020303 Then
                    dtReasonForVisitL2 = udtReasonforVisitBLL.getReasonForVisitL2(strServiceCategoryCode, intReasonForVisitL1)
                    Me.rbSelectReasonForVisitL2.DataSource = dtReasonForVisitL2
                    If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                        Me.rbSelectReasonForVisitL2.DataTextField = DataRowName.Reason_L2_Chi
                    Else
                        Me.rbSelectReasonForVisitL2.DataTextField = DataRowName.Reason_L2
                    End If
                    Me.rbSelectReasonForVisitL2.Enabled = True
                    Me.rbSelectReasonForVisitL2.DataValueField = DataRowName.Reason_L2_Code
                    Me.rbSelectReasonForVisitL2.DataBind()
                    Me.rbSelectReasonForVisitL2.ClearSelection()

                    If Me.RFVSelected Then
                        Me.LoadReasonForVisitL2FromSession(Me.RFVSelectedIndex)
                    End If

                    ' Retain Selection
                    If Not String.IsNullOrEmpty(strSelectedValue) AndAlso udtReasonforVisitBLL.getReasonForVisitL2(strServiceCategoryCode, intReasonForVisitL1, Convert.ToInt32(strSelectedValue)).Rows.Count > 0 Then
                        Me.rbSelectReasonForVisitL2.SelectedValue = strSelectedValue
                    End If
                End If
            Else
                'Me.rbSelectReasonForVisitL2.Enabled = False
            End If

            lblSelectReasonForVisitL1Header.Text = Me.RFVL1Text
            lblSelectReasonForVisitL2Header.Text = Me.RFVL2Text

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Private Sub ShowRFVTable()
            'Show Add Button
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim AllowAdd As Boolean = True
            Dim ReasonForVisitUpperLimit As Integer = 0
            If Me.ViewState("ActionMode") = EnumMode.BeforeCopaymentFeeEnabled Then
                ReasonForVisitUpperLimit = 1
            End If
            If Me.ViewState("ActionMode") = EnumMode.AfterCopaymentFeeEnabled Then
                ReasonForVisitUpperLimit = TransactionAdditionalFieldModel.AdditionalFieldType.ReasonForVisitL1.Length
            End If
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount = ReasonForVisitUpperLimit Then
                Me.btnSelectReasonForVisitGroupAdd.Visible = False
            Else
                Me.btnSelectReasonForVisitGroupAdd.Visible = True
            End If

            'Show Edit and Delete Button
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.HasReasonForVisit Then
                Me.btnSelectReasonForVisitGroupEdit.Visible = True
                Me.btnSelectReasonForVisitGroupDelete.Visible = True
            Else
                Me.btnSelectReasonForVisitGroupEdit.Visible = False
                Me.btnSelectReasonForVisitGroupDelete.Visible = False
            End If

            'Secondary Reason For Visit Separation Line
            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount >= 3 Then
                Me.trRFVSecondary1ContentOption.Style.Item("border-bottom") = "dashed 1px gray;"
                Me.trRFVSecondary1ContentError.Style.Item("border-bottom") = "dashed 1px gray;"
            Else
                Me.trRFVSecondary1ContentOption.Style.Item("border-bottom") = ""
                Me.trRFVSecondary1ContentError.Style.Item("border-bottom") = ""
            End If

            If Me.SessionHandler.EHSTransactionGetFromSession(FunctCode).TransactionAdditionFields.ReasonForVisitCount >= 4 Then
                Me.trRFVSecondary2ContentOption.Style.Item("border-bottom") = "dashed 1px gray;"
                Me.trRFVSecondary2ContentError.Style.Item("border-bottom") = "dashed 1px gray;"
            Else
                Me.trRFVSecondary2ContentOption.Style.Item("border-bottom") = ""
                Me.trRFVSecondary2ContentError.Style.Item("border-bottom") = ""
            End If

        End Sub

        Private Function RFVSelected() As Boolean
            Return Me.rdoRFVPrincipal.Checked Or Me.rdoRFVSecondary1.Checked Or Me.rdoRFVSecondary2.Checked Or Me.rdoRFVSecondary3.Checked
        End Function

        Private Function RFVSelectedIndex() As Integer
            Dim index As Integer = -1
            If Me.rdoRFVPrincipal.Checked Then
                index = 0
            End If
            If Me.rdoRFVSecondary1.Checked Then
                index = 1
            End If
            If Me.rdoRFVSecondary2.Checked Then
                index = 2
            End If
            If Me.rdoRFVSecondary3.Checked Then
                index = 3
            End If
            Return index
        End Function

        Private Sub ResetRFV()
            Me.rdoRFVPrincipal.Checked = False
            Me.rdoRFVSecondary1.Checked = False
            Me.rdoRFVSecondary2.Checked = False
            Me.rdoRFVSecondary3.Checked = False
            Me.btnSelectReasonForVisitGroupEdit.Visible = False
            Me.btnSelectReasonForVisitGroupDelete.Visible = False
        End Sub

        Private Sub AuditLogSelectReasonForVisitButton(ByVal strFunctionCode As String, ByVal strLogID As String, ByVal strDescription As String)

            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(strFunctionCode, Me.Page)
            udtAuditLogEntry.WriteLog(strLogID, strDescription)

        End Sub

#End Region

    End Class
End Namespace