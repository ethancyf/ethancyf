' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection

Namespace DynamicControls

    Public Class DynamicControlCreator

        ' Create Dynamic Control by the Type (The Class of Dynamic Control would be identified during RunTime)
        Public Shared Function CreateControl(ByVal strControlID As String, ByVal strControlType As String, ByVal strArg As String) As Control
            'Dim assemblyCustomControls As Assembly
            Dim typeDynamicControl As Type
            Dim constructorDynamicControl As ConstructorInfo
            Dim udcDynamicControlBase As DynamicControlBase

            Try

                'assemblyCustomControls = Assembly.LoadFrom(HttpRuntime.AppDomainAppPath() & "bin\CustomControls.dll")

                typeDynamicControl = Type.GetType("CustomControls.DynamicControls." & strControlType, False)

                If typeDynamicControl Is Nothing Then

                    Throw New Exception("Error: Class = [CustomControls.DynamicControls.DynamicControlCreator], Method = [CreateControl], Message = The Type of [DynamicControl] passed into this method is mismatched with the Class.")

                Else

                    constructorDynamicControl = typeDynamicControl.GetConstructor(System.Type.EmptyTypes)
                    udcDynamicControlBase = CType(constructorDynamicControl.Invoke(Nothing), DynamicControlBase)

                    udcDynamicControlBase.SetID(strControlID)
                    udcDynamicControlBase.Build(strArg)

                    Return udcDynamicControlBase

                End If

            Catch ex As Exception

                Throw ex

            End Try

            'Select Case strControlType

            'Case "CustomDateByDDL"
            'Return CreateControl_CustomDateByDDL(strControlID, strArg)

            'Case "NumberByDDL"
            'Return CreateControl_NumberByDDL(strControlID, strArg)

            'Case Else
            'Throw New Exception("Error: Class = [CustomControls.DynamicControls.DynamicControlCreator], Method = [CreateControl], Message = Type mismatch within this method")

            'End Select
        End Function

    End Class

End Namespace
