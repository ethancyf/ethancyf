Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.XML

<System.Web.Services.WebService(Namespace:="https://ppi.ha.org.hk")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PPI_EVS_WS
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function getPPIRSATokenSerialNoByHKIDDummy(ByVal hkid As String) As String
        Return "Hello World"
    End Function

    <WebMethod()> _
Public Function getPPIeHSRSATokenSerialNoByHKID(ByVal strHKID As String, ByVal strPassCode As String) As String

        'Result format returned from PPI-ePR web service
        '<TokenInfo>
        '    <TokenSN>XXX</TokenSN>
        '    <UserID>XXX</UserID>
        '    <ProjectCode>XXX</ProjectCode>
        '</TokenInfo>
        Dim strRes As String
        Dim xmlDoc As New XmlDocument()
        xmlDoc.Load(GetWSXmlData())

        Dim ds As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlDoc.InnerXml)
        Dim dtToken As DataTable = ds.Tables("token")
        Dim drToken() As DataRow = dtToken.Select("HKID='" + strHKID.Trim + "'")

        Dim xml As New XmlDocument()

        If drToken.Length > 0 Then
            If drToken(0).Item("HKID").ToString = strHKID.ToString.Trim Then

                Dim nodeRoot As XmlElement
                nodeRoot = xml.CreateElement("Root")
                xml.AppendChild(nodeRoot)

                Dim nodeTokenInfo As XmlElement
                nodeTokenInfo = xml.CreateElement("TokenInfo")
                nodeRoot.AppendChild(nodeTokenInfo)

                Dim nodeTokenSN As XmlElement
                nodeTokenSN = xml.CreateElement("TokenSN")
                nodeTokenInfo.AppendChild(nodeTokenSN)

                Dim nodeUserID As XmlElement
                nodeUserID = xml.CreateElement("UserID")
                nodeTokenInfo.AppendChild(nodeUserID)

                Dim nodeProjectCode As XmlElement
                nodeProjectCode = xml.CreateElement("ProjectCode")
                nodeTokenInfo.AppendChild(nodeProjectCode)

                nodeTokenSN.InnerText = drToken(0).Item(2).ToString
                nodeUserID.InnerText = drToken(0).Item(1).ToString
                nodeProjectCode.InnerText = drToken(0).Item(3).ToString
            End If
            strRes = xml.InnerXml
        Else
            strRes = String.Empty
        End If

        Return strRes
    End Function

#Region "Get System Parameter"

    Private Const SYS_PARAM_XML_DATA As String = "Get_Token_WS_XML_Data"

    Private Shared Function GetWSXmlData() As String

        Return System.Web.Configuration.WebConfigurationManager.AppSettings(SYS_PARAM_XML_DATA)
    End Function

#End Region


End Class