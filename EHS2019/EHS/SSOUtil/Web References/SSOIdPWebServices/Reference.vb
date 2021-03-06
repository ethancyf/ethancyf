'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.34209
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34209.
'
Namespace SSOIdPWebServices
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="SSOIdPWebServicesSoap", [Namespace]:="http://ppi.ha.org.hk/")>  _
    Partial Public Class SSOIdPWebServices
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private getSSOAssertionByArtifactOperationCompleted As System.Threading.SendOrPostCallback
        
        Private getSSOAppInfoOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.SSOUtil.My.MySettings.Default.SSOUtil_SSOIdPWebServices_SSOIdPWebServices
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event getSSOAssertionByArtifactCompleted As getSSOAssertionByArtifactCompletedEventHandler
        
        '''<remarks/>
        Public Event getSSOAppInfoCompleted As getSSOAppInfoCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ppi.ha.org.hk/getSSOAssertionByArtifact", RequestNamespace:="http://ppi.ha.org.hk/", ResponseNamespace:="http://ppi.ha.org.hk/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function getSSOAssertionByArtifact(ByVal strSecuredArtifactResolveRequest As String, ByVal strArtifactResolveRequestIssuerSSOAppIdP As String, ByVal strSSOTxnId As String, ByVal strSSOArtifact As String) As String
            Dim results() As Object = Me.Invoke("getSSOAssertionByArtifact", New Object() {strSecuredArtifactResolveRequest, strArtifactResolveRequestIssuerSSOAppIdP, strSSOTxnId, strSSOArtifact})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getSSOAssertionByArtifactAsync(ByVal strSecuredArtifactResolveRequest As String, ByVal strArtifactResolveRequestIssuerSSOAppIdP As String, ByVal strSSOTxnId As String, ByVal strSSOArtifact As String)
            Me.getSSOAssertionByArtifactAsync(strSecuredArtifactResolveRequest, strArtifactResolveRequestIssuerSSOAppIdP, strSSOTxnId, strSSOArtifact, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getSSOAssertionByArtifactAsync(ByVal strSecuredArtifactResolveRequest As String, ByVal strArtifactResolveRequestIssuerSSOAppIdP As String, ByVal strSSOTxnId As String, ByVal strSSOArtifact As String, ByVal userState As Object)
            If (Me.getSSOAssertionByArtifactOperationCompleted Is Nothing) Then
                Me.getSSOAssertionByArtifactOperationCompleted = AddressOf Me.OngetSSOAssertionByArtifactOperationCompleted
            End If
            Me.InvokeAsync("getSSOAssertionByArtifact", New Object() {strSecuredArtifactResolveRequest, strArtifactResolveRequestIssuerSSOAppIdP, strSSOTxnId, strSSOArtifact}, Me.getSSOAssertionByArtifactOperationCompleted, userState)
        End Sub
        
        Private Sub OngetSSOAssertionByArtifactOperationCompleted(ByVal arg As Object)
            If (Not (Me.getSSOAssertionByArtifactCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getSSOAssertionByArtifactCompleted(Me, New getSSOAssertionByArtifactCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://ppi.ha.org.hk/getSSOAppInfo", RequestNamespace:="http://ppi.ha.org.hk/", ResponseNamespace:="http://ppi.ha.org.hk/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function getSSOAppInfo() As System.Data.DataSet
            Dim results() As Object = Me.Invoke("getSSOAppInfo", New Object(-1) {})
            Return CType(results(0),System.Data.DataSet)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getSSOAppInfoAsync()
            Me.getSSOAppInfoAsync(Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getSSOAppInfoAsync(ByVal userState As Object)
            If (Me.getSSOAppInfoOperationCompleted Is Nothing) Then
                Me.getSSOAppInfoOperationCompleted = AddressOf Me.OngetSSOAppInfoOperationCompleted
            End If
            Me.InvokeAsync("getSSOAppInfo", New Object(-1) {}, Me.getSSOAppInfoOperationCompleted, userState)
        End Sub
        
        Private Sub OngetSSOAppInfoOperationCompleted(ByVal arg As Object)
            If (Not (Me.getSSOAppInfoCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getSSOAppInfoCompleted(Me, New getSSOAppInfoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")>  _
    Public Delegate Sub getSSOAssertionByArtifactCompletedEventHandler(ByVal sender As Object, ByVal e As getSSOAssertionByArtifactCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getSSOAssertionByArtifactCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")>  _
    Public Delegate Sub getSSOAppInfoCompletedEventHandler(ByVal sender As Object, ByVal e As getSSOAppInfoCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getSSOAppInfoCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As System.Data.DataSet
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),System.Data.DataSet)
            End Get
        End Property
    End Class
End Namespace
