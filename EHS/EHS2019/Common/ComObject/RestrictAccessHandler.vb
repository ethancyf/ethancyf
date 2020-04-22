Imports Common.ComInterface
Imports Common.Component
Imports Common.ComFunction
Imports System.Net

Namespace ComObject

    Public Class RestrictAccessHandler

        Public Const HCVU_CallCentre_RestrictIP_White_List As String = "HCVU_CallCentre_RestrictIPWhiteList"

        Public Sub New()

        End Sub

        Public Shared Function CheckValidIPAddress() As Boolean

            Dim IsValidIpAddress As Boolean = False

            Dim lstRestrictIPWhiteList As List(Of String) = Nothing
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim strRestrictIPWhiteList As String = String.Empty

            Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")
            Dim enumSubPlatform As EnumHCVUSubPlatform = EnumHCVUSubPlatform.BO

            If Not IsNothing(strSubPlatform) Then
                enumSubPlatform = [Enum].Parse(GetType(EnumHCVUSubPlatform), strSubPlatform)
            End If

            If enumSubPlatform = EnumHCVUSubPlatform.CC Then
                strRestrictIPWhiteList = (New Common.ComFunction.GeneralFunction).getSystemParameter(HCVU_CallCentre_RestrictIP_White_List)
            End If


            'Load restrict IP White List from DB SystemParameters
            If strRestrictIPWhiteList = String.Empty Then
                IsValidIpAddress = True

            Else
                lstRestrictIPWhiteList = New List(Of String)
                lstRestrictIPWhiteList.AddRange(Split(strRestrictIPWhiteList.Trim.ToUpper, ";"))

                Dim ipAddress As String = RestrictAccessHandler.GetClientIPAddress()

                If lstRestrictIPWhiteList.Contains(ipAddress.Trim.ToUpper) Then
                    IsValidIpAddress = True
                End If

            End If

            Return IsValidIpAddress

        End Function

        Private Shared Function GetClientIPAddress() As String
            
            Dim strClientIP As String = HttpContext.Current.Request.UserHostAddress

            '' IPv4
            'Dim IP As IPAddress = IPAddress.Parse(strClientIP)
            'If IP.AddressFamily = Sockets.AddressFamily.InterNetwork Then
            '    strIPAddress = IP.ToString()
            'End If

            Return strClientIP

        End Function
    End Class

End Namespace

