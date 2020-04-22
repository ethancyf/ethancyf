Imports Common.Component.HCVUUser
Imports Common.Component.RedirectParameter
Imports Common.Component.UserRole
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI.WebControls

Public Class CustomLinkButton
    Inherits System.Web.UI.WebControls.CompositeControl

#Region "Methods"

    Public Function Build() As Boolean
        Initialize()

        Controls.Clear()

        If ValidateField() = False Then
            Dim lbl As New Label
            lbl.Text = _strText
            lbl.CssClass = _strCssClass
            Controls.Add(lbl)

            Return False
        End If

        Dim lbtn As New LinkButton
        lbtn.Text = _strText
        lbtn.CssClass = _strCssClass

        AddHandler lbtn.Click, AddressOf lbtn_Click

        Controls.Add(lbtn)

        If _blnShowRedirectImage Then
            Dim img As New Image
            img.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "Redirect").ToString
            img.Style.Add("padding-left", "4px")
            img.Style.Add("position", "relative")
            img.Style.Add("top", "3px")

            Controls.Add(img)
        End If

        Return True

    End Function

    '

    Public Sub ConstructNewRedirectParameter()
        _udtRedirectParameter = New RedirectParameterModel
        _udtRedirectParameter.SourceFunctionCode = _strSourceFunctionCode
    End Sub

    '

    Public Sub SaveRedirectParameterToSession()
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        udtRedirectParameterBLL.SaveToSession(_udtRedirectParameter)
    End Sub

    Public Function GetRedirectParameterFromSession() As RedirectParameterModel
        Return (New RedirectParameterBLL).GetFromSession()
    End Function

    Public Sub RemoveRedirectParameterFromSession()
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        udtRedirectParameterBLL.RemoveFromSession()
    End Sub

    Public Sub Redirect()
        SaveRedirectParameterToSession()
        HttpContext.Current.Response.Redirect(_strTargetUrl)
    End Sub

#End Region

#Region "Supporting Methods"

    Private Sub Initialize()
        If IsNothing(_udtUserAC) Then _udtUserAC = (New HCVUUserBLL).GetHCVUUser
    End Sub

    Private Function ValidateField() As Boolean
        If _strTargetFunctionCode = String.Empty Then Return False
        If _strTargetUrl = String.Empty Then Return False
        If IsNothing(_udtUserAC) Then Return False

        If _udtUserAC.AccessRightCollection.Item(_strTargetFunctionCode).Allow = False Then Return False
        If _strSchemeCode <> String.Empty AndAlso CheckUserRoleScheme() = False Then Return False

        Return True

    End Function

    Private Function CheckUserRoleScheme() As Boolean
        For Each udtUserRole As UserRoleModel In (New UserRoleBLL).GetUserRoleCollection(_udtUserAC.UserID).Values
            If udtUserRole.SchemeCode.Trim = _strSchemeCode Then Return True
        Next

        Return False

    End Function

#End Region

#Region "Properties"

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Public Property Text() As String
        Get
            Return _strText
        End Get
        Set(ByVal value As String)
            _strText = value
        End Set
    End Property

    Public Property StyleClass() As String
        Get
            Return _strCssClass
        End Get
        Set(ByVal value As String)
            _strCssClass = value
        End Set
    End Property

    Public Property SourceFunctionCode() As String
        Get
            Return _strSourceFunctionCode
        End Get
        Set(ByVal value As String)
            _strSourceFunctionCode = value.Trim
        End Set
    End Property

    Public Property TargetFunctionCode() As String
        Get
            Return _strTargetFunctionCode
        End Get
        Set(ByVal value As String)
            _strTargetFunctionCode = value.Trim
        End Set
    End Property

    Public Property TargetUrl() As String
        Get
            Return _strTargetUrl
        End Get
        Set(ByVal value As String)
            _strTargetUrl = value.Trim
        End Set
    End Property

    Public Property SchemeCode() As String
        Get
            Return _strSchemeCode
        End Get
        Set(ByVal value As String)
            _strSchemeCode = value.Trim
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Public Property ShowRedirectImage() As Boolean
        Get
            Return _blnShowRedirectImage
        End Get
        Set(ByVal value As Boolean)
            _blnShowRedirectImage = value
        End Set
    End Property

    Public ReadOnly Property UserAC() As HCVUUserModel
        Get
            Return _udtUserAC
        End Get
    End Property

    Public Property RedirectParameter() As RedirectParameterModel
        Get
            Return _udtRedirectParameter
        End Get
        Set(ByVal value As RedirectParameterModel)
            _udtRedirectParameter = value
        End Set
    End Property

#End Region

#Region "Fields"

    Private _strText As String = String.Empty
    Private _strCssClass As String = String.Empty
    Private _strSourceFunctionCode As String = String.Empty
    Private _strTargetFunctionCode As String = String.Empty
    Private _strTargetUrl As String = String.Empty
    Private _strSchemeCode As String = String.Empty
    Private _blnShowRedirectImage As Boolean = True
    Private _udtUserAC As HCVUUserModel = Nothing
    Private _udtRedirectParameter As RedirectParameterModel = Nothing

#End Region

#Region "Events"

    Public Event Click(ByVal sender As LinkButton, ByVal e As System.EventArgs)

    Protected Sub lbtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent Click(CType(sender, LinkButton), e)
    End Sub

#End Region

End Class
