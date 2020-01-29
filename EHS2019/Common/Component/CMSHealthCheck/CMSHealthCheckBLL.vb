Imports Common.ComFunction

Namespace Component.CMSHealthCheck

    Public Class CMSHealthCheckBLL

        Public Sub New()
        End Sub

        ' CRE11-002
        Private Function GetUsingEndpointValue() As String
            Dim strUsingMode As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction

            udtGeneralFunction.getSystemParameter("CMS_Get_Vaccine_WS_Endpoint", strUsingMode, String.Empty)

            Return strUsingMode
        End Function

        ' CRE11-002
        Private Function GetUsingEndpoint(ByVal strUsingEndpointValue As String) As EndpointEnum
            Dim strUsingMode As String

            strUsingMode = strUsingEndpointValue

            Return [Enum].Parse(GetType(EndpointEnum), strUsingMode)
        End Function

        ' CRE11-002
        'Public Function GetUsingEndpointURLList() As Collection
        Public Function GetUsingEndpointURLList(Optional ByRef cllnFuncCodeList As Collection = Nothing) As Collection
            Dim strUsingMode As String = String.Empty
            Dim enumUsingEndPoint As EndpointEnum
            Dim strUsingLink As String = String.Empty
            Dim strStandbyLink() As String = Nothing

            Dim intCount As Integer
            Dim intCountFuncCode As Integer
            Dim strHealthCheckFuncCode As String = CMSVaccineHealthCheck.FuncCode


            Dim strWEBLOGICCurrentStandby As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction
            Dim cllnURL As New Collection
            Dim cllnFuncCode As New Collection

            strUsingMode = GetUsingEndpointValue()

            enumUsingEndPoint = GetUsingEndpoint(strUsingMode)


            udtGeneralFunction.getSystemParameter(String.Format("CMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, Nothing)

            udtGeneralFunction.GetInternalSystemParameterByLikeClause(String.Format("CMS_Get_Vaccine_WS_{0}_Url%[0-9]", strUsingMode), Nothing, strStandbyLink, Nothing)

            cllnURL.Add(strUsingLink)

            If Not strStandbyLink Is Nothing AndAlso strStandbyLink.Length > 0 Then
                For intCount = 0 To strStandbyLink.Length - 1
                    If strUsingLink.Trim <> strStandbyLink(intCount).Trim Then
                        cllnURL.Add(strStandbyLink(intCount).Trim)
                    End If
                Next
            End If

            If strStandbyLink Is Nothing OrElse cllnURL.Count > strStandbyLink.Length Then
                For intCountFuncCode = 1 To cllnURL.Count
                    cllnFuncCode.Add(strHealthCheckFuncCode.Replace("%i", intCountFuncCode.ToString))
                Next
            Else
                For intCountFuncCode = 1 To strStandbyLink.Length
                    cllnFuncCode.Add(strHealthCheckFuncCode.Replace("%i", intCountFuncCode.ToString))
                Next
            End If

            cllnFuncCodeList = cllnFuncCode

            Return cllnURL
        End Function


        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Winnie SUEN]
        ' ----------------------------------------------------------
        Private Function GetCIMSUsingEndpointValue() As String
            Dim strUsingMode As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction

            udtGeneralFunction.getSystemParameter("CIMS_Get_Vaccine_WS_Endpoint", strUsingMode, String.Empty)

            Return strUsingMode
        End Function


        Public Function GetCIMSUsingEndpointURLList(Optional ByRef cllnFuncCodeList As Collection = Nothing) As Collection
            Dim strUsingMode As String = String.Empty
            Dim strUsingLink As String = String.Empty
            Dim strStandbyLink() As String = Nothing

            Dim intCount As Integer
            Dim intCountFuncCode As Integer
            Dim strHealthCheckFuncCode As String = CMSVaccineHealthCheck.FuncCode

            Dim udtGeneralFunction As New GeneralFunction
            Dim cllnURL As New Collection
            Dim cllnFuncCode As New Collection

            strUsingMode = GetCIMSUsingEndpointValue()

            udtGeneralFunction.getSystemParameter(String.Format("CIMS_Get_Vaccine_WS_{0}_Url", strUsingMode), strUsingLink, Nothing)

            udtGeneralFunction.GetInternalSystemParameterByLikeClause(String.Format("CIMS_Get_Vaccine_WS_{0}_Url%[0-9]", strUsingMode), Nothing, strStandbyLink, Nothing)

            cllnURL.Add(strUsingLink)

            If Not strStandbyLink Is Nothing AndAlso strStandbyLink.Length > 0 Then
                For intCount = 0 To strStandbyLink.Length - 1
                    If strUsingLink.Trim <> strStandbyLink(intCount).Trim Then
                        cllnURL.Add(strStandbyLink(intCount).Trim)
                    End If
                Next
            End If

            If strStandbyLink Is Nothing OrElse cllnURL.Count > strStandbyLink.Length Then
                For intCountFuncCode = 1 To cllnURL.Count
                    cllnFuncCode.Add(strHealthCheckFuncCode.Replace("%i", intCountFuncCode.ToString))
                Next
            Else
                For intCountFuncCode = 1 To strStandbyLink.Length
                    cllnFuncCode.Add(strHealthCheckFuncCode.Replace("%i", intCountFuncCode.ToString))
                Next
            End If

            cllnFuncCodeList = cllnFuncCode

            Return cllnURL
        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Winnie SUEN]

    End Class
End Namespace
