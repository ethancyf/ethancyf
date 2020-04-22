' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace DynamicControls

    ' Abstract Class of all Dynamic Control
    <DefaultProperty("Text"), ToolboxData("<{0}:DynamicControlBase runat=server></{0}:DynamicControlBase>")> _
    Public MustInherit Class DynamicControlBase
        Inherits CompositeControl

        ' Delimiter of the Argument
        Protected Const ARG_DELIMITER As String = "|||"

        ' Build all component of this Dynamic Control
        Public MustOverride Sub Build(ByVal strArg As String)

        ' Get the display text of this Dynamic Control
        Public MustOverride Function GetText() As String

        ' Check if the value of this Dynamic Control is input
        Public MustOverride Function HasInputValue() As Boolean

        ' Check if the input value of this Dynamic Control is valid
        Public MustOverride Function IsValid() As Boolean

        ' Set the Control ID of this Dynamic Control
        Public Sub SetID(ByVal strID As String)
            Me.ID = strID
        End Sub

    End Class

End Namespace
