Imports Common.Component.ServiceProvider
Imports Common.Component.Profession
Imports Common.Component.UserAC

Partial Public Class ucPCDEnrolledPopup
    Inherits System.Web.UI.UserControl

    Public Enum enumButtonClick
        Cancel
        AddPractice
        LoginPCD
    End Enum

    Public Event ButtonClick(ByVal e As enumButtonClick)

    ''' <summary>
    ''' Use for handle mouse click and move popup numpad (ModalPopupExtender)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Header() As Control
        Get
            Return Me.panHeader
        End Get
    End Property

    Public ReadOnly Property ButtonCancel() As ImageButton
        Get
            Return Me.ibtnCancel
        End Get
    End Property

    Public Sub Reset()
    End Sub

#Region "Event"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Setup()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        RaiseEvent ButtonClick(enumButtonClick.Cancel)
    End Sub

    Private Sub ibtnAddPractice_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnAddPractice.Click
        RaiseEvent ButtonClick(enumButtonClick.AddPractice)
    End Sub

    Private Sub ibtnLoginPCD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLoginPCD.Click
        RaiseEvent ButtonClick(enumButtonClick.LoginPCD)
    End Sub
#End Region

    Public Sub Setup()

    End Sub

End Class