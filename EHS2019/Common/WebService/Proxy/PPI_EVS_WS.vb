﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.5472
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

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
     System.Web.Services.WebServiceBindingAttribute(Name:="PPI_EVS_WSSoap", [Namespace]:="https://ppi.ha.org.hk")> _
    Partial Public Class PPI_EVS_WS
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol

        Private getPPIRSATokenSerialNoByHKIDDummyOperationCompleted As System.Threading.SendOrPostCallback

        Private getPPIRSATokenSerialNoByHKIDOperationCompleted As System.Threading.SendOrPostCallback

        Private getTSWPatientListOperationCompleted As System.Threading.SendOrPostCallback

        Private getPPIeHSRSATokenSerialNoByHKIDOperationCompleted As System.Threading.SendOrPostCallback

        '''<remarks/>
        Public Sub New()

            MyBase.New()
            Dim udtGeneralFunction As New GeneralFunction
            Dim strValue As String = String.Empty

            udtGeneralFunction.getSystemParameter("PPIePRWSLink", strValue, String.Empty)

            Me.Url = strValue

        End Sub

        '''<remarks/>
        Public Event getPPIRSATokenSerialNoByHKIDDummyCompleted As getPPIRSATokenSerialNoByHKIDDummyCompletedEventHandler

        '''<remarks/>
        Public Event getPPIRSATokenSerialNoByHKIDCompleted As getPPIRSATokenSerialNoByHKIDCompletedEventHandler

        '''<remarks/>
        Public Event getTSWPatientListCompleted As getTSWPatientListCompletedEventHandler

        '''<remarks/>
        Public Event getPPIeHSRSATokenSerialNoByHKIDCompleted As getPPIeHSRSATokenSerialNoByHKIDCompletedEventHandler

        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ppi.ha.org.hk/getPPIRSATokenSerialNoByHKIDDummy", RequestNamespace:="https://ppi.ha.org.hk", ResponseNamespace:="https://ppi.ha.org.hk", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function getPPIRSATokenSerialNoByHKIDDummy(ByVal hkid As String) As String
            Dim results() As Object = Me.Invoke("getPPIRSATokenSerialNoByHKIDDummy", New Object() {hkid})
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Function BegingetPPIRSATokenSerialNoByHKIDDummy(ByVal hkid As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("getPPIRSATokenSerialNoByHKIDDummy", New Object() {hkid}, callback, asyncState)
        End Function

        '''<remarks/>
        Public Function EndgetPPIRSATokenSerialNoByHKIDDummy(ByVal asyncResult As System.IAsyncResult) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Overloads Sub getPPIRSATokenSerialNoByHKIDDummyAsync(ByVal hkid As String)
            Me.getPPIRSATokenSerialNoByHKIDDummyAsync(hkid, Nothing)
        End Sub

        '''<remarks/>
        Public Overloads Sub getPPIRSATokenSerialNoByHKIDDummyAsync(ByVal hkid As String, ByVal userState As Object)
            If (Me.getPPIRSATokenSerialNoByHKIDDummyOperationCompleted Is Nothing) Then
                Me.getPPIRSATokenSerialNoByHKIDDummyOperationCompleted = AddressOf Me.OngetPPIRSATokenSerialNoByHKIDDummyOperationCompleted
            End If
            Me.InvokeAsync("getPPIRSATokenSerialNoByHKIDDummy", New Object() {hkid}, Me.getPPIRSATokenSerialNoByHKIDDummyOperationCompleted, userState)
        End Sub

        Private Sub OngetPPIRSATokenSerialNoByHKIDDummyOperationCompleted(ByVal arg As Object)
            If (Not (Me.getPPIRSATokenSerialNoByHKIDDummyCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getPPIRSATokenSerialNoByHKIDDummyCompleted(Me, New getPPIRSATokenSerialNoByHKIDDummyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub

        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ppi.ha.org.hk/getPPIRSATokenSerialNoByHKID", RequestNamespace:="https://ppi.ha.org.hk", ResponseNamespace:="https://ppi.ha.org.hk", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function getPPIRSATokenSerialNoByHKID(ByVal strHKID As String, ByVal strPassCode As String) As String
            Dim results() As Object = Me.Invoke("getPPIRSATokenSerialNoByHKID", New Object() {strHKID, strPassCode})
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Function BegingetPPIRSATokenSerialNoByHKID(ByVal strHKID As String, ByVal strPassCode As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("getPPIRSATokenSerialNoByHKID", New Object() {strHKID, strPassCode}, callback, asyncState)
        End Function

        '''<remarks/>
        Public Function EndgetPPIRSATokenSerialNoByHKID(ByVal asyncResult As System.IAsyncResult) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Overloads Sub getPPIRSATokenSerialNoByHKIDAsync(ByVal strHKID As String, ByVal strPassCode As String)
            Me.getPPIRSATokenSerialNoByHKIDAsync(strHKID, strPassCode, Nothing)
        End Sub

        '''<remarks/>
        Public Overloads Sub getPPIRSATokenSerialNoByHKIDAsync(ByVal strHKID As String, ByVal strPassCode As String, ByVal userState As Object)
            If (Me.getPPIRSATokenSerialNoByHKIDOperationCompleted Is Nothing) Then
                Me.getPPIRSATokenSerialNoByHKIDOperationCompleted = AddressOf Me.OngetPPIRSATokenSerialNoByHKIDOperationCompleted
            End If
            Me.InvokeAsync("getPPIRSATokenSerialNoByHKID", New Object() {strHKID, strPassCode}, Me.getPPIRSATokenSerialNoByHKIDOperationCompleted, userState)
        End Sub

        Private Sub OngetPPIRSATokenSerialNoByHKIDOperationCompleted(ByVal arg As Object)
            If (Not (Me.getPPIRSATokenSerialNoByHKIDCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getPPIRSATokenSerialNoByHKIDCompleted(Me, New getPPIRSATokenSerialNoByHKIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub

        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ppi.ha.org.hk/getTSWPatientList", RequestNamespace:="https://ppi.ha.org.hk", ResponseNamespace:="https://ppi.ha.org.hk", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function getTSWPatientList(ByVal strPassCode As String) As String
            Dim results() As Object = Me.Invoke("getTSWPatientList", New Object() {strPassCode})
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Function BegingetTSWPatientList(ByVal strPassCode As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("getTSWPatientList", New Object() {strPassCode}, callback, asyncState)
        End Function

        '''<remarks/>
        Public Function EndgetTSWPatientList(ByVal asyncResult As System.IAsyncResult) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Overloads Sub getTSWPatientListAsync(ByVal strPassCode As String)
            Me.getTSWPatientListAsync(strPassCode, Nothing)
        End Sub

        '''<remarks/>
        Public Overloads Sub getTSWPatientListAsync(ByVal strPassCode As String, ByVal userState As Object)
            If (Me.getTSWPatientListOperationCompleted Is Nothing) Then
                Me.getTSWPatientListOperationCompleted = AddressOf Me.OngetTSWPatientListOperationCompleted
            End If
            Me.InvokeAsync("getTSWPatientList", New Object() {strPassCode}, Me.getTSWPatientListOperationCompleted, userState)
        End Sub

        Private Sub OngetTSWPatientListOperationCompleted(ByVal arg As Object)
            If (Not (Me.getTSWPatientListCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getTSWPatientListCompleted(Me, New getTSWPatientListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub

        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://ppi.ha.org.hk/getPPIeHSRSATokenSerialNoByHKID", RequestNamespace:="https://ppi.ha.org.hk", ResponseNamespace:="https://ppi.ha.org.hk", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
        Public Function getPPIeHSRSATokenSerialNoByHKID(ByVal strHKID As String, ByVal strPassCode As String) As String
            Dim results() As Object = Me.Invoke("getPPIeHSRSATokenSerialNoByHKID", New Object() {strHKID, strPassCode})
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Function BegingetPPIeHSRSATokenSerialNoByHKID(ByVal strHKID As String, ByVal strPassCode As String, ByVal callback As System.AsyncCallback, ByVal asyncState As Object) As System.IAsyncResult
            Return Me.BeginInvoke("getPPIeHSRSATokenSerialNoByHKID", New Object() {strHKID, strPassCode}, callback, asyncState)
        End Function

        '''<remarks/>
        Public Function EndgetPPIeHSRSATokenSerialNoByHKID(ByVal asyncResult As System.IAsyncResult) As String
            Dim results() As Object = Me.EndInvoke(asyncResult)
            Return CType(results(0), String)
        End Function

        '''<remarks/>
        Public Overloads Sub getPPIeHSRSATokenSerialNoByHKIDAsync(ByVal strHKID As String, ByVal strPassCode As String)
            Me.getPPIeHSRSATokenSerialNoByHKIDAsync(strHKID, strPassCode, Nothing)
        End Sub

        '''<remarks/>
        Public Overloads Sub getPPIeHSRSATokenSerialNoByHKIDAsync(ByVal strHKID As String, ByVal strPassCode As String, ByVal userState As Object)
            If (Me.getPPIeHSRSATokenSerialNoByHKIDOperationCompleted Is Nothing) Then
                Me.getPPIeHSRSATokenSerialNoByHKIDOperationCompleted = AddressOf Me.OngetPPIeHSRSATokenSerialNoByHKIDOperationCompleted
            End If
            Me.InvokeAsync("getPPIeHSRSATokenSerialNoByHKID", New Object() {strHKID, strPassCode}, Me.getPPIeHSRSATokenSerialNoByHKIDOperationCompleted, userState)
        End Sub

        Private Sub OngetPPIeHSRSATokenSerialNoByHKIDOperationCompleted(ByVal arg As Object)
            If (Not (Me.getPPIeHSRSATokenSerialNoByHKIDCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg, System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getPPIeHSRSATokenSerialNoByHKIDCompleted(Me, New getPPIeHSRSATokenSerialNoByHKIDCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub

        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")> _
    Public Delegate Sub getPPIRSATokenSerialNoByHKIDDummyCompletedEventHandler(ByVal sender As Object, ByVal e As getPPIRSATokenSerialNoByHKIDDummyCompletedEventArgs)

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code")> _
    Partial Public Class getPPIRSATokenSerialNoByHKIDDummyCompletedEventArgs
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")> _
    Public Delegate Sub getPPIRSATokenSerialNoByHKIDCompletedEventHandler(ByVal sender As Object, ByVal e As getPPIRSATokenSerialNoByHKIDCompletedEventArgs)

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code")> _
    Partial Public Class getPPIRSATokenSerialNoByHKIDCompletedEventArgs
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")> _
    Public Delegate Sub getTSWPatientListCompletedEventHandler(ByVal sender As Object, ByVal e As getTSWPatientListCompletedEventArgs)

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code")> _
    Partial Public Class getTSWPatientListCompletedEventArgs
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")> _
    Public Delegate Sub getPPIeHSRSATokenSerialNoByHKIDCompletedEventHandler(ByVal sender As Object, ByVal e As getPPIeHSRSATokenSerialNoByHKIDCompletedEventArgs)

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42"), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code")> _
    Partial Public Class getPPIeHSRSATokenSerialNoByHKIDCompletedEventArgs
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

End Namespace