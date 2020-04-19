Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        routes.MapRoute(
           name:="Localization",
           url:="{lang}/{controller}/{action}/{id}",
           constraints:=New With {.lang = "en|tc|sc"},
           defaults:=New With {
                   .Controller = "Home",
                   .Action = "Home",
                   .id = UrlParameter.Optional
               }
        )

        routes.MapRoute(
            name:="Default",
            url:="{controller}/{action}/{id}",
            defaults:=New With {.controller = "Home", .action = "Home", .id = UrlParameter.Optional}
        )
    End Sub
End Module