﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2463
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
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
'This source code was auto-generated by wsdl, Version=1.1.4322.2463.
'

'<remarks/>
<System.Diagnostics.DebuggerStepThroughAttribute(),  _
 System.ComponentModel.DesignerCategoryAttribute("code"),  _
 System.Web.Services.WebServiceBindingAttribute(Name:="Service1Soap", [Namespace]:="http://tempuri.org/")>  _
Public Class Service1
    Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
    
    '<remarks/>
    Public Sub New()
        MyBase.New
        Me.Url = "http://localhost/ExternalInterfaceWS/ExternalInterface.asmx"
    End Sub
    
    '<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RCHNameQuery", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
    Public Function RCHNameQuery(ByVal InputXML As String, ByVal SystemName As String) As String
        Dim results() As Object = Me.Invoke("RCHNameQuery", New Object() {InputXML, SystemName})
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    Public Function BeginRCHNameQuery(ByVal InputXML As String, ByVal SystemName As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("RCHNameQuery", New Object() {InputXML, SystemName}, callback, asyncState)
    End Function
    
    '<remarks/>
    Public Function EndRCHNameQuery(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetReasonForVisitList", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function GetReasonForVisitList(ByVal InputXML As String, ByVal SystemName As String) As String
        Dim results() As Object = Me.Invoke("GetReasonForVisitList", New Object() {InputXML, SystemName})
        Return CType(results(0), String)
    End Function
    
    '<remarks/>
    Public Function BeginGetReasonForVisitList(ByVal InputXML As String, ByVal SystemName As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("GetReasonForVisitList", New Object() {InputXML, SystemName}, callback, asyncState)
    End Function
    
    '<remarks/>
    Public Function EndGetReasonForVisitList(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SPPracticeValidation", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
    Public Function SPPracticeValidation(ByVal InputXML As String, ByVal SystemName As String) As String
        Dim results() As Object = Me.Invoke("SPPracticeValidation", New Object() {InputXML, SystemName})
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    Public Function BeginSPPracticeValidation(ByVal InputXML As String, ByVal SystemName As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("SPPracticeValidation", New Object() {InputXML, SystemName}, callback, asyncState)
    End Function
    
    '<remarks/>
    Public Function EndSPPracticeValidation(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/eHSValidatedAccountQuery", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
    Public Function eHSValidatedAccountQuery(ByVal InputXML As String, ByVal SystemName As String) As String
        Dim results() As Object = Me.Invoke("eHSValidatedAccountQuery", New Object() {InputXML, SystemName})
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    Public Function BegineHSValidatedAccountQuery(ByVal InputXML As String, ByVal SystemName As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("eHSValidatedAccountQuery", New Object() {InputXML, SystemName}, callback, asyncState)
    End Function
    
    '<remarks/>
    Public Function EndeHSValidatedAccountQuery(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/eHSAccountVoucherQuery", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function eHSAccountVoucherQuery(ByVal InputXML As String, ByVal SystemName As String) As String
        Dim results() As Object = Me.Invoke("eHSAccountVoucherQuery", New Object() {InputXML, SystemName})
        Return CType(results(0), String)
    End Function
    
    '<remarks/>
    Public Function BegineHSAccountVoucherQuery(ByVal InputXML As String, ByVal SystemName As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("eHSAccountVoucherQuery", New Object() {InputXML, SystemName}, callback, asyncState)
    End Function
    
    '<remarks/>
    Public Function EndeHSAccountVoucherQuery(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0), String)
    End Function
    
    '<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UploadClaim", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
    Public Function UploadClaim(ByVal InputXML As String, ByVal SystemName As String) As String
        Dim results() As Object = Me.Invoke("UploadClaim", New Object() {InputXML, SystemName})
        Return CType(results(0),String)
    End Function
    
    '<remarks/>
    Public Function BeginUploadClaim(ByVal InputXML As String, ByVal SystemName As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("UploadClaim", New Object() {InputXML, SystemName}, callback, asyncState)
    End Function
    
    '<remarks/>
    Public Function EndUploadClaim(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0),String)
    End Function

    '<remarks/>
    <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/UploadClaim_HL7", RequestNamespace:="http://tempuri.org/", ResponseNamespace:="http://tempuri.org/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function UploadClaim_HL7(ByVal InputXML As String, ByVal SystemName As String) As String
        Dim results() As Object = Me.Invoke("UploadClaim_HL7", New Object() {InputXML, SystemName})
        Return CType(results(0), String)
    End Function

    '<remarks/>
    Public Function BeginUploadClaim_HL7(ByVal InputXML As String, ByVal SystemName As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
        Return Me.BeginInvoke("UploadClaim_HL7", New Object() {InputXML, SystemName}, callback, asyncState)
    End Function

    '<remarks/>
    Public Function EndUploadClaim_HL7(ByVal asyncResult As System.IAsyncResult) As String
        Dim results() As Object = Me.EndInvoke(asyncResult)
        Return CType(results(0), String)
    End Function
End Class
