Imports System.Web.Http
Imports System.Web.Optimization

Public Class WebApiApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        'BundleConfig.RegisterBundles(BundleTable.Bundles)
        CheckAddBinPath()
    End Sub

    Public Shared Sub CheckAddBinPath()
        Dim binPath = System.IO.Path.Combine(New String() {AppDomain.CurrentDomain.BaseDirectory, "bin"})
        Dim path = If(Environment.GetEnvironmentVariable("PATH"), "")

        If Not path.Split(System.IO.Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase) Then
            path = String.Join(System.IO.Path.PathSeparator.ToString(), New String() {path, binPath})
            Environment.SetEnvironmentVariable("PATH", path)
        End If
    End Sub
End Class
