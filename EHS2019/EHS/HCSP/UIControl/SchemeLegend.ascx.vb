Imports Common.Component
Imports Common.Component.Scheme


Partial Public Class SchemeLegend
    Inherits System.Web.UI.UserControl

#Region "Private Member"
    Private udtSchemeBackOfficeBLL As SchemeBackOfficeBLL = New SchemeBackOfficeBLL
    Private udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL
    Private udtGeneralFn As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Private _blnShowFilteredSubsidy As Boolean
    Private _enumSchemeLegendSubPlatform As [Enum]
    Private _blnShowScheme As Boolean = True
    Private _blnShowSubsidy As Boolean = True

#End Region

    Public Event DataChanged()

    Public Const ViewState_ShowSeasonalDisplayCode As String = "ShowSeasonalDisplayCode"

    Private Class SESS
        Public Const IsSchemeLegendFilterSubsidy As String = "IsSchemeLegendFilterSubsidy"
        Public Const ShowScheme As String = "ShowScheme"
        Public Const ShowSubsidy As String = "ShowSubsidy"
        Public Const SchemeLegendSubPlatform As String = "SchemeLegendSubPlatform"
    End Class

#Region "Property"

    Public Property ShowSeasonalDisplayCode() As Boolean
        Get
            If ViewState(ViewState_ShowSeasonalDisplayCode) Is Nothing Then
                ViewState(ViewState_ShowSeasonalDisplayCode) = True
            End If
            Return ViewState(ViewState_ShowSeasonalDisplayCode)
        End Get
        Set(ByVal value As Boolean)
            ViewState(ViewState_ShowSeasonalDisplayCode) = value
        End Set
    End Property

    Public Property ShowFilteredSubsidy() As Boolean
        Get
            Return Me._blnShowFilteredSubsidy
        End Get
        Set(ByVal value As Boolean)
            Me._blnShowFilteredSubsidy = value
        End Set
    End Property

    Public Property SchemeLegendSubPlatform() As [Enum]
        Get
            Return Me._enumSchemeLegendSubPlatform
        End Get
        Set(ByVal value As [Enum])
            Me._enumSchemeLegendSubPlatform = value
        End Set
    End Property

    Public Property ShowScheme() As Boolean
        Get
            Return _blnShowScheme
        End Get
        Set(ByVal value As Boolean)
            _blnShowScheme = value
        End Set
    End Property

    Public Property ShowSubsidy() As Boolean
        Get
            Return _blnShowSubsidy
        End Get
        Set(ByVal value As Boolean)
            _blnShowSubsidy = value
        End Set
    End Property

#End Region

#Region "Constructor"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
#End Region

    Public Sub BindSchemeClaim(ByVal strLang As String)
        BindSchemeClaim(strLang, False)
    End Sub

    Public Sub BindSchemeClaim(ByVal strLang As String, ByVal enumSubPlatform As EnumHCSPSubPlatform)
        BindSchemeClaim(strLang, False)
    End Sub

    Public Sub BindSchemeClaim(ByVal strLang As String, ByVal blnDisplayInactive As Boolean)
        If Me.chkDisplayInactive.Checked <> blnDisplayInactive Then
            Me.chkDisplayInactive.Checked = blnDisplayInactive
        End If

        ' Bind scheme
        Dim dt As New DataTable

        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Abbreviation"))
        dt.Columns.Add(Me.GetGlobalResourceObject("Text", "Description"))
        dt.Columns.Add("SortingField")

        'Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllEffectiveSchemeClaim_WithSubsidizeGroup
        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtResSchemeClaimModelCollection As SchemeClaimModelCollection

        ' INT15-0014 Hide scheme of another subplatform in HCSP scheme and subsidy legend [Start][Lawrence]
        ShowFilteredSubsidy = True
        SchemeLegendSubPlatform = DirectCast(Me.Page, BasePage).SubPlatform
        ' INT15-0014 Hide scheme of another subplatform in HCSP scheme and subsidy legend [End][Lawrence]

        If ShowFilteredSubsidy Then
            udtResSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterByHCSPSubPlatform(SchemeLegendSubPlatform)

            For Each udtResSchemeClaimModel As SchemeClaimModel In udtResSchemeClaimModelCollection
                For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
                    If udtResSchemeClaimModel.SchemeCode.Equals(udtSchemeClaimModel.SchemeCode) Then
                        udtResSchemeClaimModel.SubsidizeGroupClaimList = udtSchemeClaimModel.SubsidizeGroupClaimList
                    End If
                Next
            Next
        Else
            udtResSchemeClaimModelCollection = udtSchemeClaimModelCollection
        End If
        Session(SESS.IsSchemeLegendFilterSubsidy) = ShowFilteredSubsidy
        Session(SESS.ShowScheme) = ShowScheme
        Session(SESS.ShowSubsidy) = ShowSubsidy
        Session(SESS.SchemeLegendSubPlatform) = SchemeLegendSubPlatform
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup()
        Dim udtSubsidizeGroupClaimModelCollection As SubsidizeGroupClaimModelCollection = New SubsidizeGroupClaimModelCollection

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Dim dtmNow As DateTime = udtGeneralFn.GetSystemDateTime.Date

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
        For Each udtSchemeClaimModel As SchemeClaimModel In udtResSchemeClaimModelCollection
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Dim dr As DataRow = Nothing

            If ShowScheme Then
                If blnDisplayInactive Or (udtSchemeClaimModel.ClaimPeriodFrom <= dtmNow And dtmNow < udtSchemeClaimModel.ClaimPeriodTo) Then
                    dr = dt.NewRow()
                    dr.Item(0) = udtSchemeClaimModel.DisplayCode.Trim
                    dr.Item(2) = udtSchemeClaimModel.DisplayCode.Trim
                    If strLang = CultureLanguage.English Then
                        dr.Item(1) = udtSchemeClaimModel.SchemeDesc.Trim
                    ElseIf strLang = CultureLanguage.SimpChinese Then
                        dr.Item(1) = udtSchemeClaimModel.SchemeDescCN.Trim
                    Else
                        dr.Item(1) = udtSchemeClaimModel.SchemeDescChi.Trim
                    End If

                    dt.Rows.Add(dr)
                End If
            End If

            If ShowSubsidy Then
                If Not udtSchemeClaimModel.SubsidizeGroupClaimList Is Nothing Then
                    For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
                        If blnDisplayInactive Or udtSubsidizeGroupClaimModel.ClaimPeriodFrom <= dtmNow And dtmNow <= udtSubsidizeGroupClaimModel.LastServiceDtm Then
                            If udtSubsidizeGroupClaimModel.SubsidizeType = "VACCINE" Then
                                udtSubsidizeGroupClaimModelCollection.Add(udtSubsidizeGroupClaimModel)
                            End If
                        End If
                    Next
                End If
            End If

        Next
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        'filter duplicate 
        Dim dtResult As DataTable = dt.Clone
        Dim aryScheme As New ArrayList
        Dim strAbbreviation As String = Me.GetGlobalResourceObject("Text", "Abbreviation")
        Dim strDescription As String = Me.GetGlobalResourceObject("Text", "Description")

        For Each dr As DataRow In dt.Rows
            If aryScheme.Contains(dr(strAbbreviation).ToString.Trim) Then Continue For
            dtResult.ImportRow(dr)
            aryScheme.Add(dr(strAbbreviation).ToString.Trim)
        Next


        ' Bind vaccination ----------------------------------------------------------------------------------------
        Dim dtSubsidize As New DataTable
        dtSubsidize = New DataTable

        dtSubsidize.Columns.Add(Me.GetGlobalResourceObject("Text", "Abbreviation"))
        dtSubsidize.Columns.Add(Me.GetGlobalResourceObject("Text", "Description"))
        dtSubsidize.Columns.Add("SortingField")

        For Each udtSubsidize As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimModelCollection
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Dim dr As DataRow = dtSubsidize.NewRow()

            If ShowSeasonalDisplayCode Then
                dr.Item(0) = udtSubsidize.DisplayCodeForClaim.Trim
                dr.Item(2) = udtSubsidize.DisplayCodeForClaim.Trim
                If strLang = CultureLanguage.English Then
                    dr.Item(1) = udtSubsidize.LegendDescForClaim.Trim
                ElseIf strLang = CultureLanguage.SimpChinese Then
                    dr.Item(1) = udtSubsidize.LegendDescForClaimCN.Trim
                Else
                    dr.Item(1) = udtSubsidize.LegendDescForClaimChi.Trim
                End If
            Else
                dr.Item(0) = udtSubsidize.SubsidizeDisplayCode.Trim
                dr.Item(2) = udtSubsidize.SubsidizeDisplayCode.Trim
                If strLang = CultureLanguage.English Then
                    dr.Item(1) = udtSubsidize.SubsidizeLegendDesc.Trim
                ElseIf strLang = CultureLanguage.SimpChinese Then
                    dr.Item(1) = udtSubsidize.SubsidizeLegendDescCN.Trim
                Else
                    dr.Item(1) = udtSubsidize.SubsidizeLegendDescChi.Trim
                End If
            End If

            dtSubsidize.Rows.Add(dr)
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
        Next

        'filter duplicate 
        Dim dtfiltered As DataTable = dt.Clone
        Dim arySubsidize As New ArrayList

        For Each dr As DataRow In dtSubsidize.Rows
            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
            Dim strSubsidizeDesc As String = String.Format("{0}|||{1}", dr(strAbbreviation), dr(strDescription))
            If arySubsidize.Contains(strSubsidizeDesc) Then Continue For

            dtfiltered.ImportRow(dr)
            arySubsidize.Add(strSubsidizeDesc)
            ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]
        Next

        'Merge
        For Each dr As DataRow In dtfiltered.Rows
            dtResult.ImportRow(dr)
        Next

        ' Perform sorting on Scheme Name column
        dtResult.DefaultView.Sort = "SortingField Asc"
        dtResult.Columns.RemoveAt(2)


        gvSchemeNameHelpScheme.DataSource = dtResult
        gvSchemeNameHelpScheme.DataBind()

        If gvSchemeNameHelpScheme.Rows.Count > 0 Then
            gvSchemeNameHelpScheme.Rows(0).Cells(0).Width = 160
            For Each r As GridViewRow In gvSchemeNameHelpScheme.Rows
                r.Cells(0).VerticalAlign = VerticalAlign.Top
            Next
        End If
    End Sub

    Private Sub chkDisplayInactive_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDisplayInactive.CheckedChanged
        Me.ShowFilteredSubsidy = Session(SESS.IsSchemeLegendFilterSubsidy)
        Me.ShowScheme = Session(SESS.ShowScheme)
        Me.ShowSubsidy = Session(SESS.ShowSubsidy)
        Me.SchemeLegendSubPlatform = Session(SESS.SchemeLegendSubPlatform)
        Me.BindSchemeClaim(Session("language"), Me.chkDisplayInactive.Checked)

        RaiseEvent DataChanged()
    End Sub
End Class