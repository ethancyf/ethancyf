Option Strict Off
Option Explicit On

Imports Common.ComFunction
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by wsdl, Version=2.0.50727.42.
'

Namespace WebService

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Web.Services.WebServiceBindingAttribute(Name:="PPI_RSA_Replacement_WSSoap", [Namespace]:="https://ppi.ha.org.hk")> _
    Partial Public Class TokenReplacementWS
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol

        Private IsCommonUserOperationCompleted As System.Threading.SendOrPostCallback

        Private ReplaceTokenOperationCompleted As System.Threading.SendOrPostCallback

        '''<remarks/>
        Public Sub New()
            MyBase.New()
            Dim udtGeneralFunction As New GeneralFunction
            Dim strValue As String = String.Empty

            udtGeneralFunction.getSystemParameter("TokenReplacementWS_EHSToPPI_Url", strValue, String.Empty)

            Me.Url = strValue

            udtGeneralFunction.getSystemParameter("TokenReplacementWS_EHSToPPI_Timeout", strValue, String.Empty)

            Me.Timeout = CInt(strValue) * 1000
        End Sub

        '''<remarks/>
        Public Event IsCommonUserCompleted As IsCommonUserCompletedEventHandler

        '''<remarks/>
        Public Event ReplaceTokenCompleted As ReplaceTokenCompletedEventHandler

        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ppi.ha.org.hk/IsCommonUser", RequestNamespace:="https://ppi.ha.org.hk", ResponseNamespace:="https://ppi.ha.org.hk", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function IsCommonUser(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String) As String()
            Dim results() As Object = Me.Invoke("IsCommonUser", New Object() {strAuthenCode, strMessageID, strHKID})
            Return CType(results(0), String())
        End Function

        '''<remarks/>
        Public Function BeginIsCommonUser(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("IsCommonUser", New Object() {strAuthenCode, strMessageID, strHKID}, callback, asyncState)
        End Function

        '''<remarks/>
        Public Function EndIsCommonUser(ByVal asyncResult As System.IAsyncResult) As String()
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0), String())
        End Function

        '''<remarks/>
        Public Overloads Sub IsCommonUserAsync(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String)
            Me.IsCommonUserAsync(strAuthenCode, strMessageID, strHKID, Nothing)
        End Sub

        '''<remarks/>
        Public Overloads Sub IsCommonUserAsync(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String, ByVal userState As Object)
            If (Me.IsCommonUserOperationCompleted Is Nothing) Then
                Me.IsCommonUserOperationCompleted = AddressOf Me.OnIsCommonUserOperationCompleted
            End If
            Me.InvokeAsync("IsCommonUser", New Object() {strAuthenCode, strMessageID, strHKID}, Me.IsCommonUserOperationCompleted, userState)
        End Sub

        Private Sub OnIsCommonUserOperationCompleted(ByVal arg As Object)
            If (Not (Me.IsCommonUserCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent IsCommonUserCompleted(Me, New IsCommonUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub

        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ppi.ha.org.hk/ReplaceToken", RequestNamespace:="https://ppi.ha.org.hk", ResponseNamespace:="https://ppi.ha.org.hk", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function ReplaceToken(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String, ByVal strOrgTokenSerialNo As String, ByVal strReplaceTokenSerialNo As String) As String()
            Dim results() As Object = Me.Invoke("ReplaceToken", New Object() {strAuthenCode, strMessageID, strHKID, strOrgTokenSerialNo, strReplaceTokenSerialNo})
            Return CType(results(0), String())
        End Function

        '''<remarks/>
        Public Function BeginReplaceToken(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String, ByVal strOrgTokenSerialNo As String, ByVal strReplaceTokenSerialNo As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("ReplaceToken", New Object() {strAuthenCode, strMessageID, strHKID, strOrgTokenSerialNo, strReplaceTokenSerialNo}, callback, asyncState)
        End Function

        '''<remarks/>
        Public Function EndReplaceToken(ByVal asyncResult As System.IAsyncResult) As String()
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0), String())
        End Function

        '''<remarks/>
        Public Overloads Sub ReplaceTokenAsync(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String, ByVal strOrgTokenSerialNo As String, ByVal strReplaceTokenSerialNo As String)
            Me.ReplaceTokenAsync(strAuthenCode, strMessageID, strHKID, strOrgTokenSerialNo, strReplaceTokenSerialNo, Nothing)
        End Sub

        '''<remarks/>
        Public Overloads Sub ReplaceTokenAsync(ByVal strAuthenCode As String, ByVal strMessageID As String, ByVal strHKID As String, ByVal strOrgTokenSerialNo As String, ByVal strReplaceTokenSerialNo As String, ByVal userState As Object)
            If (Me.ReplaceTokenOperationCompleted Is Nothing) Then
                Me.ReplaceTokenOperationCompleted = AddressOf Me.OnReplaceTokenOperationCompleted
            End If
            Me.InvokeAsync("ReplaceToken", New Object() {strAuthenCode, strMessageID, strHKID, strOrgTokenSerialNo, strReplaceTokenSerialNo}, Me.ReplaceTokenOperationCompleted, userState)
        End Sub

        Private Sub OnReplaceTokenOperationCompleted(ByVal arg As Object)
            If (Not (Me.ReplaceTokenCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent ReplaceTokenCompleted(Me, New ReplaceTokenCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub

        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")> _
    Public Delegate Sub IsCommonUserCompletedEventHandler(ByVal sender As Object, ByVal e As IsCommonUserCompletedEventArgs)

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code")> _
    Partial Public Class IsCommonUserCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs

        Private results() As Object

        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub

        '''<remarks/>
        Public ReadOnly Property Result() As String()
            Get
                Me.RaiseExceptionIfNecessary()
                Return CType(Me.results(0), String())
            End Get
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")> _
    Public Delegate Sub ReplaceTokenCompletedEventHandler(ByVal sender As Object, ByVal e As ReplaceTokenCompletedEventArgs)

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code")> _
    Partial Public Class ReplaceTokenCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs

        Private results() As Object

        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub

        '''<remarks/>
        Public ReadOnly Property Result() As String()
            Get
                Me.RaiseExceptionIfNecessary()
                Return CType(Me.results(0), String())
            End Get
        End Property
    End Class

End Namespace
