'------------------------------------------------------------------------------
' <auto-generated>
'     這段程式碼是由工具產生的。
'     執行階段版本:2.0.50727.1873
'
'     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
'     變更將會遺失。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by wsdl, Version=2.0.50727.42.
'

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Web.Services.WebServiceBindingAttribute(Name:="MessageDynListenerWebServiceSoapHttp", [Namespace]:="http://receiver.common.eai.ha.org.hk/")> _
Partial Public Class ImmuEnquiryWebS_EndPoint
    Inherits Microsoft.Web.Services3.WebServicesClientProtocol
    'Inherits System.Web.Services.Protocols.SoapHttpClientProtocol

    Private submitTextMessageOperationCompleted As System.Threading.SendOrPostCallback

    '''<remarks/>
    Public Sub New()
        MyBase.New()
        Me.Url = "http://dc4zwingw13x/eai_common_receiver_weblistener/ha/cms/messagetype/receiver"
    End Sub

    '''<remarks/>
    Public Event submitTextMessageCompleted As submitTextMessageCompletedEventHandler

    '''<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://receiver.common.eai.ha.org.hk//submitTextMessage", RequestElementName:="submitTextMessageElement", RequestNamespace:="http://receiver.common.eai.ha.org.hk/types/", ResponseElementName:="submitTextMessageResponseElement", ResponseNamespace:="http://receiver.common.eai.ha.org.hk/types/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function submitTextMessage(<System.Xml.Serialization.XmlElementAttribute(IsNullable:=True)> ByVal message As String) As <System.Xml.Serialization.XmlElementAttribute("result", IsNullable:=True)> String
        Dim results() As Object = Me.Invoke("submitTextMessage", New Object() {message})
        Return CType(results(0), String)
    End Function

    '''<remarks/>
    Public Function BeginsubmitTextMessage(ByVal message As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("submitTextMessage", New Object() {message}, callback, asyncState)
    End Function

    '''<remarks/>
    Public Function EndsubmitTextMessage(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0), String)
    End Function

    '''<remarks/>
    Public Overloads Sub submitTextMessageAsync(ByVal message As String)
        Me.submitTextMessageAsync(message, Nothing)
    End Sub

    '''<remarks/>
    Public Overloads Sub submitTextMessageAsync(ByVal message As String, ByVal userState As Object)
        If (Me.submitTextMessageOperationCompleted Is Nothing) Then
            Me.submitTextMessageOperationCompleted = AddressOf Me.OnsubmitTextMessageOperationCompleted
        End If
        Me.InvokeAsync("submitTextMessage", New Object() {message}, Me.submitTextMessageOperationCompleted, userState)
    End Sub

    Private Sub OnsubmitTextMessageOperationCompleted(ByVal arg As Object)
        If (Not (Me.submitTextMessageCompletedEvent) Is Nothing) Then
            Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
            RaiseEvent submitTextMessageCompleted(Me, New submitTextMessageCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
        End If
    End Sub

    '''<remarks/>
    Public Shadows Sub CancelAsync(ByVal userState As Object)
        MyBase.CancelAsync(userState)
    End Sub
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")>  _
Public Delegate Sub submitTextMessageCompletedEventHandler(ByVal sender As Object, ByVal e As submitTextMessageCompletedEventArgs)

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code")> _
Partial Public Class submitTextMessageCompletedEventArgs
    Inherits System.ComponentModel.AsyncCompletedEventArgs

    Private results() As Object

    Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
        MyBase.New(exception, cancelled, userState)
        Me.results = results
    End Sub

    '''<remarks/>
    Public ReadOnly Property Result() As String
        Get
            Me.RaiseExceptionIfNecessary()
            Return CType(Me.results(0), String)
        End Get
    End Property
End Class
