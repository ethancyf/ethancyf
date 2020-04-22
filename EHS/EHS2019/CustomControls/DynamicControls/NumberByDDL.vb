' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace DynamicControls

    ' A single [DropDownList] for Numeric Input
    ' No. of Argument is 2
    ' 1st Argument is Min. Value for Selection
    ' 2nd Argument is Max. Value for Selection
    Public Class NumberByDDL
        Inherits DynamicControlBase

        Private Const NO_OF_ARGS As Integer = 2
        Private Const NO_SELECTION As String = "-1"

        Private _ddlNumber As DropDownList = New DropDownList()

#Region "Abstract Method for the Abstract Class - [DynamicControlBase]"
        Public Overrides Sub Build(ByVal strArg As String)
            If Not strArg Is Nothing Then

                Dim strArgs() As String = strArg.Split(New String() {ARG_DELIMITER}, StringSplitOptions.RemoveEmptyEntries)

                If strArgs.Length = NO_OF_ARGS Then

                    Dim itmNumber As ListItem
                    Dim i As Integer

                    _ddlNumber.ID = Me.ID & "_ddlNumber"

                    itmNumber = New ListItem("----", NO_SELECTION)
                    _ddlNumber.Items.Add(itmNumber)

                    Try

                        For i = CInt(strArgs(0)) To CInt(strArgs(1))

                            itmNumber = New ListItem(i.ToString(), i.ToString())
                            _ddlNumber.Items.Add(itmNumber)

                        Next

                    Catch ex As Exception

                        Throw New Exception("Error: Class = [CustomControls.DynamicControls.NumberByDDL], Method = [Build], Message = Inappropriate argument passed into this method. Min. and Max. number must be a number.")

                    End Try

                    Controls.Add(_ddlNumber)

                Else

                    Throw New Exception("Error: Class = [CustomControls.DynamicControls.NumberByDDL], Method = [Build], Message = Inappropriate argument passed into this method. It must have both Min. and Max. number that were delimited by the symbol - """ & ARG_DELIMITER & """.")

                End If

            Else

                Throw New Exception("Error: Class = [CustomControls.DynamicControls.NumberByDDL], Method = [Build], Message = Missing argument passed into this method")

            End If
        End Sub

        Public Overrides Function GetText() As String

            Return _ddlNumber.SelectedItem.Text

        End Function

        Public Overrides Function HasInputValue() As Boolean
            If _ddlNumber.SelectedValue = NO_SELECTION Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Overrides Function IsValid() As Boolean
            Return True
        End Function
#End Region

    End Class

End Namespace
