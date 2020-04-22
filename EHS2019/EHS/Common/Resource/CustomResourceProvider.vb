Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.Compilation
Imports System.Resources
Imports System.Globalization
Imports System.Collections
Imports System.Reflection
Imports System.Collections.Specialized
Imports System.Data
Imports Common.ComObject
Imports Common.Component
Imports System.Data.SqlClient
Imports Common.DataAccess

Namespace Resource

    Public Class CustomResourceProviderFactory
        Inherits ResourceProviderFactory
        Public Overrides Function CreateGlobalResourceProvider(ByVal classname As String) As IResourceProvider
            Return New CustomResourceProvider(Nothing, classname)
        End Function
        Public Overrides Function CreateLocalResourceProvider(ByVal virtualPath As String) As IResourceProvider
            Return New CustomResourceProvider(virtualPath, Nothing)
        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        ''' <summary>
        ''' Retrieve global resource manually (Additional way for windows application which not support Global Resource)
        ''' </summary>
        ''' <param name="classKey">e.g. SystemMessage</param>
        ''' <param name="resouceKey">e.g. 990000-E-00123</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetGlobalResourceObject(ByVal classKey As String, ByVal resouceKey As String) As Object
            Dim udtResourceFactory As New Common.Resource.CustomResourceProviderFactory
            Dim udtResourceProvider As Compilation.IResourceProvider = udtResourceFactory.CreateGlobalResourceProvider(classKey)
            Return udtResourceProvider.GetObject(resouceKey, Nothing)
        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        Public Shared Function GetGlobalResourceObject(ByVal classKey As String, ByVal resouceKey As String, ByVal enumLang As EnumLanguage) As Object
            Dim udtResourceFactory As New Common.Resource.CustomResourceProviderFactory
            Dim udtResourceProvider As CustomResourceProvider = udtResourceFactory.CreateGlobalResourceProvider(classKey)
            Return udtResourceProvider.GetObject(resouceKey, enumLang)
        End Function
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
    End Class

    ' Define the resource provider for global and local resources.
    Friend Class CustomResourceProvider
        Implements IResourceProvider
        Dim _virtualPath As String
        Dim _className As String
        Private _resourceCache As IDictionary
        Private CultureNeutralKey As Object = New Object()

        Public Shared LoadedProviders As List(Of CustomResourceProvider)

        Public Sub New(ByVal virtualPath As String, ByVal classname As String)
            _virtualPath = virtualPath
            _className = classname

            'If CustomResourceProvider.LoadedProviders Is Nothing Then
            '    LoadedProviders = New List(Of CustomResourceProvider)
            'End If

            'LoadedProviders.Add(Me)

        End Sub

        Function GetObject(ByVal resourceKey As String, ByVal culture As CultureInfo) As Object Implements IResourceProvider.GetObject
            Dim cultureName As String = String.Empty

            If IsNothing(culture) Then
                cultureName = CultureInfo.CurrentUICulture.Name
            Else
                cultureName = culture.Name
            End If

            Return StringResourcesDALC.GetResources(_virtualPath, _className, cultureName, False, Nothing)(resourceKey)

        End Function

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        Public Function GetObject(ByVal resourceKey As String, ByVal enumLang As EnumLanguage) As Object
            Dim cultureName As String = String.Empty

            Select Case enumLang
                Case EnumLanguage.EN
                    cultureName = CultureLanguage.English
                Case EnumLanguage.TC
                    cultureName = CultureLanguage.TradChinese
                Case EnumLanguage.SC
                    cultureName = CultureLanguage.SimpChinese
            End Select
            Return StringResourcesDALC.GetResources(_virtualPath, _className, cultureName, False, Nothing)(resourceKey)

        End Function
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

        Public Sub ClearResourceCache()
            _resourceCache.Clear()
        End Sub

        ReadOnly Property ResourceReader() As IResourceReader Implements IResourceProvider.ResourceReader
            Get
                Dim cultureName As String
                Dim currentUICulture As CultureInfo
                cultureName = Nothing
                currentUICulture = CultureInfo.CurrentUICulture
                cultureName = CultureInfo.InvariantCulture.Name
                'Dim _StringResourcesDALC As New StringResourcesDALC
                Return New CustomResourceReader(StringResourcesDALC.GetResources(_virtualPath, _className, cultureName, False, Nothing))
            End Get
        End Property

    End Class

    Friend NotInheritable Class CustomResourceReader
        Implements IResourceReader
        Private _resources As IDictionary

        Public Sub New(ByVal resources As IDictionary)
            _resources = resources
        End Sub

        Function GetEnumerator1() As IDictionaryEnumerator Implements IResourceReader.GetEnumerator
            Return _resources.GetEnumerator()
        End Function

        Sub Close() Implements IResourceReader.Close

        End Sub

        Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return _resources.GetEnumerator()
        End Function

        Sub Dispose() Implements IDisposable.Dispose

        End Sub
    End Class

    Public Class StringResourcesDALC

        Public Shared Function GetResources(ByVal virtualPath As String, ByVal className As String, ByVal cultureName As String, ByVal designMode As Boolean, ByVal serviceProvider As IServiceProvider) As IDictionary
            Dim resources As ListDictionary
            Dim resourceType As String = ""
            If Not String.IsNullOrEmpty(virtualPath) Then
                resourceType = virtualPath
            ElseIf Not String.IsNullOrEmpty(className) Then
                resourceType = className
            Else
                'code for unhandle case
            End If
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If HttpRuntime.Cache.Item(IIf(cultureName Is Nothing, "", cultureName.ToLower) & " " & resourceType) Is Nothing Then
                'If HttpContext.Current.Cache.Item(IIf(cultureName Is Nothing, "", cultureName) & " " & resourceType) Is Nothing Then
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                resources = New ListDictionary
                Dim dt As New DataTable
                Dim db As New Database
                Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")
                Dim objDependency As SqlCacheDependency = Nothing
                If resourceType.ToLower.Trim <> "systemmessage" Then
                    If IsNothing(cultureName) Then
                        Throw New Exception(String.Format("StringResourcesDALC.GetResources: cultureName is Nothing (resourceType={0})", resourceType))
                    End If

                    Dim prams() As SqlParameter = {db.MakeInParam("@resourceType", SqlDbType.VarChar, 50, resourceType), _
                                                   db.MakeInParam("@platform", SqlDbType.Char, 2, strPlatform)}

                    Select Case cultureName.ToLower
                        Case CultureLanguage.TradChinese
                            db.RunProc("proc_SystemResource_get_cache_zh_tw", prams, dt)
                        Case CultureLanguage.SimpChinese
                            db.RunProc("proc_SystemResource_get_cache_zh_cn", prams, dt)
                        Case Else
                            ' Default: Consider as English
                            db.RunProc("proc_SystemResource_get_cache", prams, dt)
                    End Select

                Else
                    If IsNothing(cultureName) Then
                        Throw New Exception(String.Format("StringResourcesDALC.GetResources: cultureName is Nothing (resourceType={0})", resourceType))
                    End If

                    Dim prams() As SqlParameter = {db.MakeInParam("@platform", SqlDbType.Char, 2, strPlatform)}

                    Select Case cultureName.ToLower
                        Case CultureLanguage.TradChinese
                            db.RunProc("proc_SystemMessage_get_cache_zh_tw", prams, dt)
                        Case CultureLanguage.SimpChinese
                            db.RunProc("proc_SystemMessage_get_cache_zh_cn", prams, dt)
                        Case Else
                            ' Default: Consider as English
                            db.RunProc("proc_SystemMessage_get_cache", prams, dt)
                    End Select

                End If
                Dim i As Integer
                For i = 0 To dt.Rows.Count - 1
                    resources.Add(dt.Rows(i).Item("resourceKey"), dt.Rows(i).Item("resourceValue"))
                Next
                'HttpContext.Current.Cache.Insert(IIf(cultureName Is Nothing, "", cultureName) & " " & resourceType, resources, objDependency)
                'CacheHandler.InsertCache(IIf(cultureName Is Nothing, "", cultureName) & " " & resourceType, resources, objDependency)
                CacheHandler.InsertCache(IIf(cultureName Is Nothing, "", cultureName.ToLower) & " " & resourceType, resources)
            Else
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                resources = CType(HttpRuntime.Cache.Item(IIf(cultureName Is Nothing, "", cultureName.ToLower) & " " & resourceType), IDictionary)
                'resources = CType(HttpContext.Current.Cache.Item(IIf(cultureName Is Nothing, "", cultureName) & " " & resourceType), IDictionary)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            End If
            Return resources
        End Function

    End Class
End Namespace
