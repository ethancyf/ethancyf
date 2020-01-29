Option Strict Off
Option Explicit On

Imports Common.ComFunction
Imports Microsoft.Web.Services3.Design

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440"), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Web.Services.WebServiceBindingAttribute(Name:="ExternalCallinPortBinding", [Namespace]:="http://hk.gov.ehr.service.app.ehrws.proxy/ehrws-proxy/ExternalCallinWebS")> _
Partial Public Class ExternalCallinWebS
    Inherits Microsoft.Web.Services3.WebServicesClientProtocol

    Private getEhrWebSOperationCompleted As System.Threading.SendOrPostCallback

    '''<remarks/>
    Public Sub New()
        MyBase.New()
        Me.Url = "http://127.0.0.1/dummy/"
    End Sub

    '''<remarks/>
    Public Event getEhrWebSCompleted As getEhrWebSCompletedEventHandler

    '''<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://hk.gov.ehr.service.app.ehrws.proxy/ehrws-proxy/ExternalCallinWebS", ResponseNamespace:="http://hk.gov.ehr.service.app.ehrws.proxy/ehrws-proxy/ExternalCallinWebS", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function getEhrWebS(ByVal inputParam As String) As <System.Xml.Serialization.XmlElementAttribute("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> String
        ' Set the policy to secure the message and remove unnecessary headers
        Dim strThumbprint As String = (New GeneralFunction).getSystemParameter("eHRSS_WS_GetEhrWebSCertThumbprint")

        Dim p As New Policy
        p.Assertions.Add(New CustomSecurityAssertion(strThumbprint))
        Me.SetPolicy(p)

        ' Call
        Dim results() As Object = Me.Invoke("getEhrWebS", New Object() {inputParam})
        Return CType(results(0), String)

    End Function

    '''<remarks/>
    Public Function BegingetEhrWebS(ByVal inputParam As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("getEhrWebS", New Object() {inputParam}, callback, asyncState)
    End Function

    '''<remarks/>
    Public Function EndgetEhrWebS(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0), String)
    End Function

    '''<remarks/>
    Public Overloads Sub getEhrWebSAsync(ByVal inputParam As String)
        Me.getEhrWebSAsync(inputParam, Nothing)
    End Sub

    '''<remarks/>
    Public Overloads Sub getEhrWebSAsync(ByVal inputParam As String, ByVal userState As Object)
        If (Me.getEhrWebSOperationCompleted Is Nothing) Then
            Me.getEhrWebSOperationCompleted = AddressOf Me.OngetEhrWebSOperationCompleted
        End If
        Me.InvokeAsync("getEhrWebS", New Object() {inputParam}, Me.getEhrWebSOperationCompleted, userState)
    End Sub

    Private Sub OngetEhrWebSOperationCompleted(ByVal arg As Object)
        If (Not (Me.getEhrWebSCompletedEvent) Is Nothing) Then
            Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
            RaiseEvent getEhrWebSCompleted(Me, New getEhrWebSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
        End If
    End Sub

    '''<remarks/>
    Public Shadows Sub CancelAsync(ByVal userState As Object)
        MyBase.CancelAsync(userState)
    End Sub
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")> _
Public Delegate Sub getEhrWebSCompletedEventHandler(ByVal sender As Object, ByVal e As getEhrWebSCompletedEventArgs)

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440"), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code")> _
Partial Public Class getEhrWebSCompletedEventArgs
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
