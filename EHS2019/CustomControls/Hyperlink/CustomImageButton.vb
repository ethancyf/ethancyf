Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.RedirectParameter
Imports Common.Component.UserRole
Imports System.ComponentModel
Imports System.Web
Imports System.Web.UI.WebControls

Public Class CustomImageButton
    Inherits System.Web.UI.WebControls.CompositeControl

#Region "Methods"

    Public Function Build() As Boolean
        Initialize()

        Controls.Clear()

        Dim ibtn As New ImageButton
        ibtn.ImageUrl = _strImageUrl
        ibtn.AlternateText = _strAlternateText

        If ValidateField() = False Then
            ibtn.Enabled = False
            ibtn.ImageUrl = _strImageUrlDisable

            Controls.Add(ibtn)

            Return False
        End If

        AddHandler ibtn.Click, AddressOf ibtn_Click

        Controls.Add(ibtn)

        If _blnShowRedirectImage Then
            Dim img As New Image
            img.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "Redirect").ToString
            img.Style.Add("padding-left", "4px")

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

    Protected Overridable Sub Initialize()
        Try
            If IsNothing(_udtUserAC) Then _udtUserAC = (New HCVUUserBLL).GetHCVUUser
        Catch ex As Exception
            _udtUserAC = Common.Component.UserAC.UserACBLL.GetUserAC
        End Try

    End Sub

    Private Function ValidateField() As Boolean
        If _strTargetFunctionCode = String.Empty Then Return False
        If _strTargetUrl = String.Empty Then Return False

        If TypeOf _udtUserAC Is HCVUUserModel Then
            If IsNothing(_udtUserAC) Then Return False

            Dim udtUserAC As Common.Component.HCVUUser.HCVUUserModel = DirectCast(_udtUserAC, HCVUUserModel)
            If udtUserAC.AccessRightCollection.Item(_strTargetFunctionCode).Allow = False Then Return False
            If _strSchemeCode <> String.Empty AndAlso CheckUserRoleScheme() = False Then Return False
        End If
        Return True

    End Function

    Protected Overridable Function CheckUserRoleScheme() As Boolean
        If Not TypeOf _udtUserAC Is Common.Component.HCVUUser.HCVUUserModel Then Return False

        Dim udtUserAC As Common.Component.HCVUUser.HCVUUserModel = DirectCast(_udtUserAC, HCVUUserModel)
        For Each udtUserRole As UserRoleModel In (New UserRoleBLL).GetUserRoleCollection(udtUserAC.UserID).Values
            If udtUserRole.SchemeCode.Trim = _strSchemeCode Then Return True
        Next

        Return False

    End Function

#End Region

#Region "Properties"

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Public WriteOnly Property ImageUrl() As String
        Set(ByVal value As String)
            _strImageUrl = value
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Public WriteOnly Property ImageUrlDisable() As String
        Set(ByVal value As String)
            _strImageUrlDisable = value
        End Set
    End Property

    <Bindable(True), Category("Appearance"), DefaultValue(""), Localizable(True)> Public Property AlternateText() As String
        Get
            Return _strAlternateText
        End Get
        Set(ByVal value As String)
            _strAlternateText = value
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
            If TypeOf _udtUserAC Is Common.Component.HCVUUser.HCVUUserModel Then
                Return DirectCast(_udtUserAC, Common.Component.HCVUUser.HCVUUserModel)
            Else
                Return Nothing
            End If
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

    Private _strImageUrl As String = String.Empty
    Private _strImageUrlDisable As String = String.Empty
    Private _strAlternateText As String = String.Empty
    Private _strSourceFunctionCode As String = String.Empty
    Private _strTargetFunctionCode As String = String.Empty
    Private _strTargetUrl As String = String.Empty
    Private _strSchemeCode As String = String.Empty
    Private _blnShowRedirectImage As Boolean = True
    Private _udtUserAC As Object = Nothing
    Private _udtRedirectParameter As RedirectParameterModel = Nothing

#End Region

#Region "Events"

    Public Event Click(ByVal sender As ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs)

    Protected Sub ibtn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent Click(CType(sender, ImageButton), e)
    End Sub

#End Region

End Class
