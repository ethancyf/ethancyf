Imports Common.Component.CCCode
Imports Common.ComObject

Partial Public Class ChooseCCCode
    Inherits System.Web.UI.UserControl

    Public Enum DisplayStyle
        DoubleRow
        SingalRow
    End Enum

    Private _strCCCode1 As String
    Private _strCCCode2 As String
    Private _strCCCode3 As String
    Private _strCCCode4 As String
    Private _strCCCode5 As String
    Private _strCCCode6 As String
    Private _enumDisplayStyle As DisplayStyle

    'Dim udtVRAcctMaintBLL As BLL.VoucherAccountMaintenanceBLL = New BLL.VoucherAccountMaintenanceBLL()
    'Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Dim udteHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL
    'Events
    Public Event Confirm(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event Cancel(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Public Const SESS_CCCode1 As String = "SESS_CHOOSECCCODE_CCCODE1"
    Public Const SESS_CCCode2 As String = "SESS_CHOOSECCCODE_CCCODE2"
    Public Const SESS_CCCode3 As String = "SESS_CHOOSECCCODE_CCCODE3"
    Public Const SESS_CCCode4 As String = "SESS_CHOOSECCCODE_CCCODE4"
    Public Const SESS_CCCode5 As String = "SESS_CHOOSECCCODE_CCCODE5"
    Public Const SESS_CCCode6 As String = "SESS_CHOOSECCCODE_CCCODE6"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim generalFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction()
        Dim strCCCLinkCHI As String = String.Empty
        Dim strCCCLinkENG As String = String.Empty

        Me.lnkHere.Text = Me.GetGlobalResourceObject("Text", "here")
        If LCase(Session("language")) = Common.Component.CultureLanguage.TradChinese Then
            generalFunction.getSystemParameter("CheckCCCodeLink_HCVU_ZH", strCCCLinkCHI, String.Empty)
            Me.lnkHere.OnClientClick = String.Format("javascript:openNewWin('{0}'); return false;", strCCCLinkCHI)
        Else
            generalFunction.getSystemParameter("CheckCCCodeLink_HCVU_EN", strCCCLinkENG, String.Empty)
            Me.lnkHere.OnClientClick = String.Format("javascript:openNewWin('{0}'); return false;", strCCCLinkENG)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Me.SetRowDisplayStyle()
    End Sub

    Protected Sub ibtnCCCodeConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCCCodeConfirm.Click
        RaiseEvent Confirm(sender, e)
    End Sub

    Protected Sub ibtnCCCodeCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCCCodeCancel.Click
        RaiseEvent Cancel(sender, e)
    End Sub

    Public Function GetChineseName(ByVal strFunctionCode As String, ByVal blnSaveCCCodeToSession As Boolean) As String
        Dim strCName As String = String.Empty
        strCName += Me.GetChineseWord(Me._strCCCode1, Me.ddlCCCode1, blnSaveCCCodeToSession, strFunctionCode, SESS_CCCode1)
        strCName += Me.GetChineseWord(Me._strCCCode2, Me.ddlCCCode2, blnSaveCCCodeToSession, strFunctionCode, SESS_CCCode2)
        strCName += Me.GetChineseWord(Me._strCCCode3, Me.ddlCCCode3, blnSaveCCCodeToSession, strFunctionCode, SESS_CCCode3)
        strCName += Me.GetChineseWord(Me._strCCCode4, Me.ddlCCCode4, blnSaveCCCodeToSession, strFunctionCode, SESS_CCCode4)
        strCName += Me.GetChineseWord(Me._strCCCode5, Me.ddlCCCode5, blnSaveCCCodeToSession, strFunctionCode, SESS_CCCode5)
        strCName += Me.GetChineseWord(Me._strCCCode6, Me.ddlCCCode6, blnSaveCCCodeToSession, strFunctionCode, SESS_CCCode6)

        Return strCName
    End Function

    Private Function GetChineseWord(ByVal strCCCode As String, ByVal ddlCCCode As DropDownList, ByVal blnSaveCCCodeToSession As Boolean, ByVal strfunctionCode As String, ByVal sessionName As String) As String
        Dim strChiWord As String = String.Empty
        Session.Remove(sessionName)

        If strCCCode Is Nothing OrElse strCCCode.Equals(String.Empty) Then
            ddlCCCode.Items.Clear()
            ddlCCCode.Enabled = False
            ' INT20-0047 (Fix throw error for invalid CCCode) [Start][Winnie]
            Me.udteHSAccountMaintBLL.CCCodeRemoveFromSession(strfunctionCode, sessionName)
            ' INT20-0047 (Fix throw error for invalid CCCode) [End][Winnie]
        Else
            If IsNothing(ddlCCCode.SelectedItem) Then
                If blnSaveCCCodeToSession Then
                    Me.udteHSAccountMaintBLL.CCCodeRemoveFromSession(strfunctionCode, sessionName)
                End If
            Else

                If blnSaveCCCodeToSession Then
                    If strCCCode.Length = 4 Then
                        Me.udteHSAccountMaintBLL.CCCodeSaveToSession(strfunctionCode, sessionName, String.Format("{0}{1}", strCCCode, ddlCCCode.SelectedValue.Trim()))
                    ElseIf strCCCode.Length = 5 Then
                        Me.udteHSAccountMaintBLL.CCCodeSaveToSession(strfunctionCode, sessionName, strCCCode)
                    End If

                End If
                strChiWord = ddlCCCode.SelectedItem.Text
            End If
        End If

        Return strChiWord
    End Function

    Public Function CCCodeDiff(ByVal strCurrentCCCode As String, ByVal strFunctionCode As String, ByVal intCCCodeIndex As Integer) As Boolean
        Dim isDiff As Boolean
        Select Case intCCCodeIndex

            Case 1
                isDiff = Me.CCCodeDiff(strCurrentCCCode, strFunctionCode, SESS_CCCode1, Me.ddlCCCode1)
            Case 2
                isDiff = Me.CCCodeDiff(strCurrentCCCode, strFunctionCode, SESS_CCCode2, Me.ddlCCCode2)
            Case 3
                isDiff = Me.CCCodeDiff(strCurrentCCCode, strFunctionCode, SESS_CCCode3, Me.ddlCCCode3)
            Case 4
                isDiff = Me.CCCodeDiff(strCurrentCCCode, strFunctionCode, SESS_CCCode4, Me.ddlCCCode4)
            Case 5
                isDiff = Me.CCCodeDiff(strCurrentCCCode, strFunctionCode, SESS_CCCode5, Me.ddlCCCode5)
            Case 6
                isDiff = Me.CCCodeDiff(strCurrentCCCode, strFunctionCode, SESS_CCCode6, Me.ddlCCCode6)
        End Select

        Return isDiff
    End Function

    Private Function CCCodeDiff(ByVal strCurrentCCCode As String, ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String, ByVal ddlCCCode As DropDownList) As Boolean
        Dim strSessionCCCode As String = String.Empty

        Dim isDiff As String = False

        If Not strCurrentCCCode.Equals(String.Empty) Then

            'Chineses name is selected
            If Not IsNothing(ddlCCCode.SelectedItem) Then
                'Get the CCCode with tail                
                strCurrentCCCode = String.Format("{0}{1}", strCurrentCCCode, ddlCCCode.SelectedValue.Trim())
            End If
        End If

        'Start the checking if the Session CCCode is not empty 
        'if Session CCCode is empty, then no need to check
        strSessionCCCode = Me.udteHSAccountMaintBLL.CCCodeGetFormSession(strFunctionCode, strCCCodeSessionName)
        If strSessionCCCode Is Nothing Then
            strSessionCCCode = String.Empty
        Else
            strSessionCCCode = strSessionCCCode.Trim()
        End If

        'Start the checking if the passed CCCode is not empty 
        If strCurrentCCCode.Equals(strSessionCCCode) Then
            isDiff = False
        Else            

            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 2 Change ddl if tail not match but head match
            If strCurrentCCCode.Equals(String.Empty) OrElse strSessionCCCode.Equals(String.Empty) Then
                isDiff = True
            Else
                If strCurrentCCCode.Substring(0, 4) = strSessionCCCode.Substring(0, 4) Then
                    Me.BindCCCode(strSessionCCCode, ddlCCCode)
                    isDiff = False
                Else
                    isDiff = True
                End If
            End If
            ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 2 Change ddl if tail not match but head match
        End If

        Return isDiff

    End Function

    Public Function BindCCCode() As Common.ComObject.SystemMessage
        Dim systemMessage As SystemMessage = Nothing
        Dim isValid As Boolean = True

        isValid = Me.BindCCCode(Me._strCCCode1, Me.ddlCCCode1)

        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 3
        'If isValid Then
        '    isValid = Me.BindCCCode(Me._strCCCode2, Me.ddlCCCode2)
        'End If

        If Not isValid Then
            systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
            isValid = False
        End If
        ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 3

        If isValid Then
            If Me._strCCCode1.Equals(String.Empty) AndAlso Not Me._strCCCode2.Equals(String.Empty) Then
                systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                isValid = False
            Else
                If Not Me.BindCCCode(Me._strCCCode2, Me.ddlCCCode2) Then
                    systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                    isValid = False
                End If
            End If
        End If

        If isValid Then
            If Me._strCCCode2.Equals(String.Empty) AndAlso Not Me._strCCCode3.Equals(String.Empty) Then
                systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                isValid = False
            Else
                If Not Me.BindCCCode(Me._strCCCode3, Me.ddlCCCode3) Then
                    systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                    isValid = False
                End If
            End If
        End If

        If isValid Then
            If Me._strCCCode3.Equals(String.Empty) AndAlso Not Me._strCCCode4.Equals(String.Empty) Then
                systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                isValid = False
            Else
                If Not Me.BindCCCode(Me._strCCCode4, Me.ddlCCCode4) Then
                    systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                    isValid = False
                End If
            End If
        End If

        If isValid Then
            If Me._strCCCode4.Equals(String.Empty) AndAlso Not Me._strCCCode5.Equals(String.Empty) Then
                systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                isValid = False
            Else
                If Not Me.BindCCCode(Me._strCCCode5, Me.ddlCCCode5) Then
                    systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                    isValid = False
                End If
            End If
        End If

        If isValid Then
            If Me._strCCCode5.Equals(String.Empty) AndAlso Not Me._strCCCode6.Equals(String.Empty) Then
                systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                isValid = False
            Else
                If Not Me.BindCCCode(Me._strCCCode6, Me.ddlCCCode6) Then
                    systemMessage = New Common.ComObject.SystemMessage("990000", "E", "00039")
                    isValid = False
                End If
            End If
        End If

        Return systemMessage
    End Function

    Private Function BindCCCode(ByVal strCCCode As String, ByVal ddlCCCode As DropDownList) As Boolean
        Dim isValid As Boolean = False
        Dim dtCCCode As DataTable
        Dim udteHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL

        If strCCCode Is Nothing OrElse strCCCode.Equals(String.Empty) Then
            isValid = True

            ddlCCCode.Items.Clear()
            ddlCCCode.Enabled = False
        Else

            If strCCCode.Length >= 4 Then
                Dim strTail As String = String.Empty
                Dim strCode As String = strCCCode.Substring(0, 4)

                dtCCCode = udteHSAccountMaintBLL.getCCCTail(strCode)
                If dtCCCode.Rows.Count > 0 Then
                    ddlCCCode.DataSource = dtCCCode
                    ddlCCCode.DataTextField = "Big5"
                    ddlCCCode.DataValueField = "CCC_Tail"
                    ddlCCCode.DataBind()
                    ddlCCCode.Enabled = True
                    isValid = True
                End If

                If strCCCode.Length = 5 Then
                    strTail = strCCCode.Substring(4, 1)
                    ' INT20-0047 (Fix throw error for invalid CCCode) [Start][Winnie]
                    'ddlCCCode.SelectedValue = strTail
                    If Not ddlCCCode.Items.FindByValue(strTail) Is Nothing Then
                        ddlCCCode.SelectedValue = strTail
                    End If
                    ' INT20-0047 (Fix throw error for invalid CCCode) [End][Winnie]
                End If

            End If
        End If

        Return isValid
    End Function

    Public Sub CleanSession(ByVal strFunctionCode As String)
        Me.udteHSAccountMaintBLL.CCCodeRemoveFromSession(strFunctionCode)
    End Sub

    Public Sub Clean()
        Me.ddlCCCode1.Items.Clear()
        Me.ddlCCCode2.Items.Clear()
        Me.ddlCCCode3.Items.Clear()
        Me.ddlCCCode4.Items.Clear()
        Me.ddlCCCode5.Items.Clear()
        Me.ddlCCCode6.Items.Clear()
    End Sub

    Public Property CCCode1() As String
        Get
            Return Me._strCCCode1
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                Me._strCCCode1 = value.Trim()
            Else
                Me._strCCCode1 = Nothing
            End If
        End Set
    End Property

    Public Property CCCode2() As String
        Get
            Return Me._strCCCode2
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                Me._strCCCode2 = value.Trim()
            Else
                Me._strCCCode2 = Nothing
            End If
        End Set
    End Property

    Public Property CCCode3() As String
        Get
            Return Me._strCCCode3
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                Me._strCCCode3 = value.Trim()
            Else
                Me._strCCCode3 = Nothing
            End If
        End Set
    End Property

    Public Property CCCode4() As String
        Get
            Return Me._strCCCode4
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                Me._strCCCode4 = value.Trim()
            Else
                Me._strCCCode4 = Nothing
            End If
        End Set
    End Property

    Public Property CCCode5() As String
        Get
            Return Me._strCCCode5
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                Me._strCCCode5 = value.Trim()
            Else
                Me._strCCCode5 = Nothing
            End If
        End Set
    End Property

    Public Property CCCode6() As String
        Get
            Return Me._strCCCode6
        End Get
        Set(ByVal value As String)
            If Not value Is Nothing Then
                Me._strCCCode6 = value.Trim()
            Else
                Me._strCCCode6 = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property SelectedCCCodeTail1() As String
        Get
            Return Me.ddlCCCode1.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCCCodeTail2() As String
        Get
            Return Me.ddlCCCode2.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCCCodeTail3() As String
        Get
            Return Me.ddlCCCode3.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCCCodeTail4() As String
        Get
            Return Me.ddlCCCode4.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCCCodeTail5() As String
        Get
            Return Me.ddlCCCode5.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedCCCodeTail6() As String
        Get
            Return Me.ddlCCCode6.SelectedValue
        End Get
    End Property

    Public Property RowDisplayStyle() As DisplayStyle
        Get
            Return Me._enumDisplayStyle
        End Get
        Set(ByVal value As DisplayStyle)
            Me._enumDisplayStyle = value
        End Set
    End Property

    Private Sub SetRowDisplayStyle()
        If Not IsNothing(Me._enumDisplayStyle) Then
            If Me._enumDisplayStyle = DisplayStyle.DoubleRow Then
                Me.tbCCCode.Width = "200px"
            Else
                Me.tbCCCode.Width = "330px"
            End If
        End If
    End Sub

    Private Function getCCCTail(ByVal strcccode As String, ByRef strDisplay As String) As String
        Dim strRes As String
        Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
        strRes = String.Empty
        strRes = udtCCCodeBLL.GetCCCodeDesc(strcccode, strDisplay)
        Return strRes
    End Function

    Private Function getCCCTail(ByVal strcccode As String) As DataTable
        Dim dtRes As DataTable
        Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
        dtRes = udtCCCodeBLL.GetCCCodeDesc(strcccode)
        Return dtRes
    End Function


    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [Start][Winnie] Step 1
    Public Function getCCCodeFromSession(ByVal intCCCodeIndex As Integer, ByVal strFunctionCode As String) As String
        Dim CCCode As String = String.Empty

        Select Case intCCCodeIndex
            Case 1
                CCCode = Me.udteHSAccountMaintBLL.CCCodeGetFormSession(strFunctionCode, SESS_CCCode1)
            Case 2
                CCCode = Me.udteHSAccountMaintBLL.CCCodeGetFormSession(strFunctionCode, SESS_CCCode2)
            Case 3
                CCCode = Me.udteHSAccountMaintBLL.CCCodeGetFormSession(strFunctionCode, SESS_CCCode3)
            Case 4
                CCCode = Me.udteHSAccountMaintBLL.CCCodeGetFormSession(strFunctionCode, SESS_CCCode4)
            Case 5
                CCCode = Me.udteHSAccountMaintBLL.CCCodeGetFormSession(strFunctionCode, SESS_CCCode5)
            Case 6
                CCCode = Me.udteHSAccountMaintBLL.CCCodeGetFormSession(strFunctionCode, SESS_CCCode6)
        End Select

        If Not CCCode Is Nothing Then
            Return CCCode
        End If

        Return String.Empty
    End Function
    ' CRE15-014 HA_MingLiu UTF32 - Fix CCCode Session Handling Issue [End][Winnie] Step 1
End Class