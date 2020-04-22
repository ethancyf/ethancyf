Public Partial Class ucCollapsibleSearchCriteriaReview
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property HeaderText() As String
        Get
            Return lblSearchCriteriaReviewHeader.Text
        End Get
        Set(ByVal value As String)
            lblSearchCriteriaReviewHeader.Text = value
        End Set
    End Property

    Public WriteOnly Property ShowHeaderText() As Boolean
        Set(ByVal value As Boolean)
            tdHeaderText.Visible = value
        End Set
    End Property

    Public Property AutoCollapse() As Boolean
        Get
            Return collapsiblePanelExtender.AutoCollapse
        End Get
        Set(ByVal value As Boolean)
            collapsiblePanelExtender.AutoCollapse = value
        End Set
    End Property

    Public Property AutoExpand() As Boolean
        Get
            Return collapsiblePanelExtender.AutoExpand
        End Get
        Set(ByVal value As Boolean)
            collapsiblePanelExtender.AutoExpand = value
        End Set
    End Property

    Public Property ClientState() As String
        Get
            Return collapsiblePanelExtender.ClientState
        End Get
        Set(ByVal value As String)
            collapsiblePanelExtender.ClientState = value
        End Set
    End Property

    Public Property Collapsed() As Boolean
        Get
            Return collapsiblePanelExtender.Collapsed
        End Get
        Set(ByVal value As Boolean)
            collapsiblePanelExtender.Collapsed = value
        End Set
    End Property

    Public Property CollapsedImage() As String
        Get
            Return collapsiblePanelExtender.CollapsedImage
        End Get
        Set(ByVal value As String)
            collapsiblePanelExtender.CollapsedImage = value
        End Set
    End Property

    Public Property CollapsedSize() As Integer
        Get
            Return collapsiblePanelExtender.CollapsedSize
        End Get
        Set(ByVal value As Integer)
            collapsiblePanelExtender.CollapsedSize = value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return collapsiblePanelExtender.Enabled
        End Get
        Set(ByVal value As Boolean)
            collapsiblePanelExtender.Enabled = value
        End Set
    End Property

    Public Property ExpandDirection() As AjaxControlToolkit.CollapsiblePanelExpandDirection
        Get
            Return collapsiblePanelExtender.ExpandDirection
        End Get
        Set(ByVal value As AjaxControlToolkit.CollapsiblePanelExpandDirection)
            collapsiblePanelExtender.ExpandDirection = value
        End Set
    End Property

    Public Property ExpandedImage() As String
        Get
            Return collapsiblePanelExtender.ExpandedImage
        End Get
        Set(ByVal value As String)
            collapsiblePanelExtender.ExpandedImage = value
        End Set
    End Property

    Public Property ExpandedSize() As Integer
        Get
            Return collapsiblePanelExtender.ExpandedSize
        End Get
        Set(ByVal value As Integer)
            collapsiblePanelExtender.ExpandedSize = value
        End Set
    End Property

    Public Property ScrollContents() As Boolean
        Get
            Return collapsiblePanelExtender.ScrollContents
        End Get
        Set(ByVal value As Boolean)
            collapsiblePanelExtender.ScrollContents = value
        End Set
    End Property

    Public Property TargetControlID() As String
        Get
            Return collapsiblePanelExtender.TargetControlID
        End Get
        Set(ByVal value As String)
            collapsiblePanelExtender.TargetControlID = value
        End Set
    End Property
End Class
