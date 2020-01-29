Option Strict Off
Option Explicit On

Imports Common.ComFunction
Imports Microsoft.Web.Services3.Design

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440"), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Web.Services.WebServiceBindingAttribute(Name:="systemVerificationWebSPortBinding", [Namespace]:="http://hk.gov.ehr.service.tch.ecm.systemVerificationService/")> _
Partial Public Class SystemVerificationWebS
    Inherits Microsoft.Web.Services3.WebServicesClientProtocol

    Private verifySystemOperationCompleted As System.Threading.SendOrPostCallback

    '''<remarks/>
    Public Sub New()
        MyBase.New()
        Me.Url = "http://127.0.0.1/dummy/"
    End Sub

    '''<remarks/>
    Public Event verifySystemCompleted As verifySystemCompletedEventHandler

    '''<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://hk.gov.ehr.service.tch.ecm.systemVerificationService/", ResponseNamespace:="http://hk.gov.ehr.service.tch.ecm.systemVerificationService/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function verifySystem(ByVal inputParam As String) As <System.Xml.Serialization.XmlElementAttribute("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> String
        ' Set the policy to secure the message and remove unnecessary headers
        Dim strThumbprint As String = (New GeneralFunction).getSystemParameter("eHRSS_WS_VerifySystemCertThumbprint")

        Dim p As New Policy
        p.Assertions.Add(New CustomSecurityAssertion(strThumbprint))
        Me.SetPolicy(p)

        ' Call
        Dim results() As Object = Me.Invoke("verifySystem", New Object() {inputParam})
        Return CType(results(0), String)

    End Function

    '''<remarks/>
    Public Function BeginverifySystem(ByVal inputParam As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("verifySystem", New Object() {inputParam}, callback, asyncState)
    End Function

    '''<remarks/>
    Public Function EndverifySystem(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0), String)
    End Function

    '''<remarks/>
    Public Overloads Sub verifySystemAsync(ByVal inputParam As String)
        Me.verifySystemAsync(inputParam, Nothing)
    End Sub

    '''<remarks/>
    Public Overloads Sub verifySystemAsync(ByVal inputParam As String, ByVal userState As Object)
        If (Me.verifySystemOperationCompleted Is Nothing) Then
            Me.verifySystemOperationCompleted = AddressOf Me.OnverifySystemOperationCompleted
        End If
        Me.InvokeAsync("verifySystem", New Object() {inputParam}, Me.verifySystemOperationCompleted, userState)
    End Sub

    Private Sub OnverifySystemOperationCompleted(ByVal arg As Object)
        If (Not (Me.verifySystemCompletedEvent) Is Nothing) Then
            Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
            RaiseEvent verifySystemCompleted(Me, New verifySystemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
        End If
    End Sub

    '''<remarks/>
    Public Shadows Sub CancelAsync(ByVal userState As Object)
        MyBase.CancelAsync(userState)
    End Sub
End Class

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")> _
Public Delegate Sub verifySystemCompletedEventHandler(ByVal sender As Object, ByVal e As verifySystemCompletedEventArgs)

'''<remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440"), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code")> _
Partial Public Class verifySystemCompletedEventArgs
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

