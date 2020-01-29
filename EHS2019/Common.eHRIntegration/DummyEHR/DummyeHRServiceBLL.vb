Imports System.Configuration
Imports System.IO
Imports System.Xml
Imports Common.ComFunction
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports Common.Format

Namespace Dummy

    Public Class DummyeHRServiceBLL

        Public Function VerifySystem(strRequestXml As String) As String
            Dim udtOutXml As New InVerifySystemXmlModel

            Dim dtmNow As DateTime = DateTime.Now
            Dim dtmExpiry As DateTime = dtmNow.AddHours(4)

            Dim dt As DataTable = ReadDummySPList()

            If dt.Rows.Count > 0 AndAlso dt.Columns.Contains("VPResultCode") Then
                Dim dr As DataRow = dt.Rows(0)

                If Not IsDBNull(dr("VPResultCode")) AndAlso dr("VPResultCode") <> String.Empty Then
                    udtOutXml.Status = dr("VPResultCode")

                    Return XmlFunction.SerializeXml(udtOutXml)

                End If

            End If

            udtOutXml.Status = "10000"
            udtOutXml.StatusDescription = "Success"

            udtOutXml.VerificationPass = String.Empty
            Dim r As New Random

            For i As Integer = 1 To 32
                udtOutXml.VerificationPass += "0123456789abcdefghijklmnopqrstuvwxyz".Chars(r.Next(0, 36))
            Next

            udtOutXml.VerificationPassExpiry = String.Format("{0}{1}", _
                                                             dtmExpiry.ToString("yyyy-MM-dd HH:mm:ss.fff"), _
                                                             dtmExpiry.ToString("zzzz").Replace(":", String.Empty))

            udtOutXml.VerificationPassIssueTime = String.Format("{0}{1}", _
                                                                dtmNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), _
                                                                dtmNow.ToString("zzzz").Replace(":", String.Empty))

            Return XmlFunction.SerializeXml(udtOutXml)

        End Function

        Public Function GetEhrWebS(strRequestXml As String) As String
            Dim udtInXml As New OutGeteHRWebSXmlModel

            ' Deserialize
            XmlFunction.DeserializeXml(strRequestXml, udtInXml)

            ' Convert the data part to Xml
            Dim xml As New XmlDocument

            Try
                xml.LoadXml(udtInXml.data)

            Catch exXE As XmlException
                Throw

            End Try

            ' Check the root element to determine the function
            Dim udtOutFunctionXml As Object = Nothing

            Select Case xml.DocumentElement.Name
                Case "geteHRSSTokenInfo"
                    ' Deserialize the input
                    Dim udtInFunctionXml As New OutGeteHRSSTokenInfoXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXml)

                    ' Process data
                    udtOutFunctionXml = GeteHRSSTokenInfo(udtInFunctionXml)

                Case "seteHRSSTokenShared"
                    ' Deserialize the input
                    Dim udtInFunctionXml As New OutSeteHRSSTokenSharedXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXml)

                    ' Process data
                    udtOutFunctionXml = SeteHRSSTokenShared(udtInFunctionXml)

                Case "replaceeHRSSToken"
                    ' Deserialize the input
                    Dim udtInFunctionXml As New OutReplaceeHRSSTokenXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXml)

                    ' Process data
                    udtOutFunctionXml = ReplaceeHRSSToken(udtInFunctionXml)

                Case "notifyeHRSSTokenDeactivated"
                    ' Deserialize the input
                    Dim udtInFunctionXml As New OutNotifyeHRSSTokenDeactivatedXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXml)

                    ' Process data
                    udtOutFunctionXml = NotifyeHRSSTokenDeactivated(udtInFunctionXml)

                Case "geteHRSSLoginAlias"
                    ' Deserialize the input
                    Dim udtInFunctionXml As New OutGeteHRSSLoginAliasXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXml)

                    ' Process data
                    udtOutFunctionXml = GeteHRSSLoginAlias(udtInFunctionXml)

                Case "healthCheckeHRSS"
                    ' Deserialize the input
                    Dim udtInFunctionXml As New OutHealthCheckeHRSSXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXml)

                    ' Process data
                    udtOutFunctionXml = HealthCheckeHRSS(udtInFunctionXml)

                Case Else
                    Throw New NotImplementedException

            End Select

            ' Serialize the output
            Dim udtOutXml As New InGeteHRWebSXmlModel
            udtOutXml.Status = "70000"
            udtOutXml.StatusDescription = "Success"
            udtOutXml.data = XmlFunction.SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

            Dim strResponseXml As String = XmlFunction.SerializeXml(udtOutXml)

            Return strResponseXml

        End Function

        Private Function GeteHRSSTokenInfo(udtInFunctionXml As OutGeteHRSSTokenInfoXmlModel) As InGeteHRTokenInfoXmlModel
            ' Format
            udtInFunctionXml.HKID = (New Formatter).formatHKIDInternal(udtInFunctionXml.HKID)

            Dim udtOutXml As InGeteHRTokenInfoXmlModel = Nothing

            Dim dt As DataTable = ReadDummySPList()
            Dim drs As DataRow() = dt.Select(String.Format("HKID = '{0}'", udtInFunctionXml.HKID), String.Empty)

            Select Case drs.Count
                Case 0
                    udtOutXml = New InGeteHRTokenInfoXmlModel(eHRResultCode.R9002_UserNotFound, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                Case 1
                    Dim dr As DataRow = drs(0)

                    If dt.Columns.Contains("WSResultCode") AndAlso Not IsDBNull(dr("WSResultCode")) AndAlso dr("WSResultCode") <> String.Empty Then
                        Dim eResultStatus As eHRResultCode = Nothing

                        For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                            If e.ToString.Contains(dr("WSResultCode")) Then
                                eResultStatus = e
                            End If
                        Next

                        udtOutXml = New InGeteHRTokenInfoXmlModel(eResultStatus, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    Else
                        If IsDBNull(dr("ExistingTokenID")) OrElse dr("ExistingTokenID").ToString = String.Empty Then
                            udtOutXml = New InGeteHRTokenInfoXmlModel(eHRResultCode.R1001_NoTokenAssigned, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                            udtOutXml.IsCommonUser = "Y"

                        Else
                            udtOutXml = New InGeteHRTokenInfoXmlModel(eHRResultCode.R1000_Success, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                            udtOutXml.IsCommonUser = "Y"
                            udtOutXml.ExistingTokenID = dr("ExistingTokenID")
                            udtOutXml.ExistingTokenIssuer = dr("ExistingTokenIssuer")
                            udtOutXml.IsExistingTokenShared = dr("IsExistingTokenShared")

                            If Not IsDBNull(dr("NewTokenID")) AndAlso dr("NewTokenID").ToString <> String.Empty Then
                                udtOutXml.NewTokenID = dr("NewTokenID")
                                udtOutXml.NewTokenIssuer = dr("NewTokenIssuer")
                                udtOutXml.IsNewTokenShared = dr("IsNewTokenShared")
                            End If

                        End If

                    End If

                Case Else
                    Throw New Exception(String.Format("DummyeHRServiceBLL.GeteHRSSTokenInfo: Unexpected value (drs.Count={0},HKID={1})", drs.Count, udtInFunctionXml.HKID))

            End Select

            Return udtOutXml

        End Function

        Private Function SeteHRSSTokenShared(udtInFunctionXml As OutSeteHRSSTokenSharedXmlModel) As InSeteHRSSTokenSharedXmlModel
            ' Format
            udtInFunctionXml.HKID = (New Formatter).formatHKIDInternal(udtInFunctionXml.HKID)

            Dim udtOutXml As InSeteHRSSTokenSharedXmlModel = Nothing

            Dim dt As DataTable = ReadDummySPList()
            Dim drs As DataRow() = dt.Select(String.Format("HKID = '{0}'", udtInFunctionXml.HKID), String.Empty)

            Select Case drs.Count
                Case 0
                    udtOutXml = New InSeteHRSSTokenSharedXmlModel(eHRResultCode.R9002_UserNotFound, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                Case 1
                    Dim dr As DataRow = drs(0)

                    If dt.Columns.Contains("WSResultCode") AndAlso Not IsDBNull(dr("WSResultCode")) AndAlso dr("WSResultCode") <> String.Empty Then
                        Dim eResultStatus As eHRResultCode = Nothing

                        For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                            If e.ToString.Contains(dr("WSResultCode")) Then
                                eResultStatus = e
                            End If
                        Next

                        udtOutXml = New InSeteHRSSTokenSharedXmlModel(eResultStatus, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    Else
                        udtOutXml = New InSeteHRSSTokenSharedXmlModel(eHRResultCode.R1000_Success, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    End If

                Case Else
                    Throw New Exception(String.Format("DummyeHRServiceBLL.SeteHRSSTokenShared: Unexpected value (drs.Count={0},HKID={1})", drs.Count, udtInFunctionXml.HKID))

            End Select

            Return udtOutXml

        End Function

        Private Function ReplaceeHRSSToken(udtInFunctionXml As OutReplaceeHRSSTokenXmlModel) As InReplaceeHRSSTokenXmlModel
            ' Format
            udtInFunctionXml.HKID = (New Formatter).formatHKIDInternal(udtInFunctionXml.HKID)

            Dim udtOutXml As InReplaceeHRSSTokenXmlModel = Nothing

            Dim dt As DataTable = ReadDummySPList()
            Dim drs As DataRow() = dt.Select(String.Format("HKID = '{0}'", udtInFunctionXml.HKID), String.Empty)

            Select Case drs.Count
                Case 0
                    udtOutXml = New InReplaceeHRSSTokenXmlModel(eHRResultCode.R9002_UserNotFound, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                Case 1
                    Dim dr As DataRow = drs(0)

                    If dt.Columns.Contains("WSResultCode") AndAlso Not IsDBNull(dr("WSResultCode")) AndAlso dr("WSResultCode") <> String.Empty Then
                        Dim eResultStatus As eHRResultCode = Nothing

                        For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                            If e.ToString.Contains(dr("WSResultCode")) Then
                                eResultStatus = e
                            End If
                        Next

                        udtOutXml = New InReplaceeHRSSTokenXmlModel(eResultStatus, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    Else
                        udtOutXml = New InReplaceeHRSSTokenXmlModel(eHRResultCode.R1000_Success, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    End If

                Case Else
                    Throw New Exception(String.Format("DummyeHRServiceBLL.ReplaceeHRSSToken: Unexpected value (drs.Count={0},HKID={1})", drs.Count, udtInFunctionXml.HKID))

            End Select

            Return udtOutXml

        End Function

        Private Function NotifyeHRSSTokenDeactivated(udtInFunctionXml As OutNotifyeHRSSTokenDeactivatedXmlModel) As InNotifyeHRSSTokenDeactivatedXmlModel
            ' Format
            udtInFunctionXml.HKID = (New Formatter).formatHKIDInternal(udtInFunctionXml.HKID)

            Dim udtOutXml As InNotifyeHRSSTokenDeactivatedXmlModel = Nothing

            Dim dt As DataTable = ReadDummySPList()
            Dim drs As DataRow() = dt.Select(String.Format("HKID = '{0}'", udtInFunctionXml.HKID), String.Empty)

            Select Case drs.Count
                Case 0
                    udtOutXml = New InNotifyeHRSSTokenDeactivatedXmlModel(eHRResultCode.R9002_UserNotFound, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                Case 1
                    Dim dr As DataRow = drs(0)

                    If dt.Columns.Contains("WSResultCode") AndAlso Not IsDBNull(dr("WSResultCode")) AndAlso dr("WSResultCode") <> String.Empty Then
                        Dim eResultStatus As eHRResultCode = Nothing

                        For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                            If e.ToString.Contains(dr("WSResultCode")) Then
                                eResultStatus = e
                            End If
                        Next

                        udtOutXml = New InNotifyeHRSSTokenDeactivatedXmlModel(eResultStatus, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    Else
                        udtOutXml = New InNotifyeHRSSTokenDeactivatedXmlModel(eHRResultCode.R1000_Success, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    End If

                Case Else
                    Throw New Exception(String.Format("DummyeHRServiceBLL.NotifyeHRSSTokenDeactivated: Unexpected value (drs.Count={0},HKID={1})", drs.Count, udtInFunctionXml.HKID))

            End Select

            Return udtOutXml

        End Function

        Private Function GeteHRSSLoginAlias(udtInFunctionXml As OutGeteHRSSLoginAliasXmlModel) As InGeteHRSSLoginAliasXmlModel
            ' Format
            udtInFunctionXml.HKID = (New Formatter).formatHKIDInternal(udtInFunctionXml.HKID)

            Dim udtOutXml As InGeteHRSSLoginAliasXmlModel = Nothing

            Dim dt As DataTable = ReadDummySPList()
            Dim drs As DataRow() = dt.Select(String.Format("HKID = '{0}'", udtInFunctionXml.HKID), String.Empty)

            Select Case drs.Count
                Case 0
                    udtOutXml = New InGeteHRSSLoginAliasXmlModel(eHRResultCode.R9002_UserNotFound, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                Case 1
                    Dim dr As DataRow = drs(0)

                    If dt.Columns.Contains("WSResultCode") AndAlso Not IsDBNull(dr("WSResultCode")) AndAlso dr("WSResultCode") <> String.Empty Then
                        Dim eResultStatus As eHRResultCode = Nothing

                        For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                            If e.ToString.Contains(dr("WSResultCode")) Then
                                eResultStatus = e
                            End If
                        Next

                        udtOutXml = New InGeteHRSSLoginAliasXmlModel(eResultStatus, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                    Else
                        If Not IsDBNull(dr("LoginAlias")) AndAlso dr("LoginAlias").ToString <> String.Empty Then
                            udtOutXml = New InGeteHRSSLoginAliasXmlModel(eHRResultCode.R1000_Success, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)
                            udtOutXml.LoginAlias = dr("LoginAlias")

                        Else
                            udtOutXml = New InGeteHRSSLoginAliasXmlModel(eHRResultCode.R2001_LoginAliasNotSet, udtInFunctionXml.HKID, udtInFunctionXml.Timestamp)

                        End If

                    End If

                Case Else
                    Throw New Exception(String.Format("DummyeHRServiceBLL.GeteHRSSLoginAlias: Unexpected value (drs.Count={0},HKID={1})", drs.Count, udtInFunctionXml.HKID))

            End Select

            Return udtOutXml

        End Function

        Private Function HealthCheckeHRSS(udtInFunctionXml As OutHealthCheckeHRSSXmlModel) As InHealthCheckeHRSSXmlModel
            Dim dt As DataTable = ReadDummySPList()

            If dt.Rows.Count > 0 AndAlso dt.Columns.Contains("WSResultCode") Then
                Dim dr As DataRow = dt.Rows(0)

                If Not IsDBNull(dr("WSResultCode")) AndAlso dr("WSResultCode") <> String.Empty Then
                    Dim eResultStatus As eHRResultCode = Nothing

                    For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                        If e.ToString.Contains(dr("WSResultCode")) Then
                            eResultStatus = e
                        End If
                    Next

                    Return New InHealthCheckeHRSSXmlModel(eResultStatus, udtInFunctionXml.Timestamp)

                End If

            End If

            Return New InHealthCheckeHRSSXmlModel(eHRResultCode.R1000_Success, udtInFunctionXml.Timestamp)

        End Function

        '

        Public Function ReadDummySPList() As DataTable
            Dim strPath As String = (New GeneralFunction).getSystemParameter("eHRSS_EMULATE_eHRDummySPXmlPath")

            If IsNothing(strPath) OrElse File.Exists(strPath) = False Then
                Throw New Exception(String.Format("DummyeHRServiceBLL.ReadDummySPList: You are using EMULATE mode but system is unable to locate " + _
                                                  "the dummy xml file, please check your SystemParameters eHRSS_EMULATE_eHRDummySPXmlPath"))
            End If

            Dim xr As XmlReader = XmlReader.Create(strPath, New XmlReaderSettings)
            Dim ds As New DataSet
            ds.ReadXml(xr)
            xr.Close()

            Return ds.Tables(0)

        End Function

    End Class

End Namespace
