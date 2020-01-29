Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports Common.Component.ReasonForVisit
Imports Common.ComFunction.Generator
Imports ExternalInterfaceWS.XMLGenerator
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject
Imports Common.Component

Namespace Component.Response

    Public Class GetReasonForVisitListResponse

#Region "Private Constant"

        Private Const TAG_EHS_RESPONSE As String = "Response"
        Private Const TAG_OUTPUT As String = "Output"
        Private Const TAG_ERROR_INFO As String = "ErrorInfo"
        Private Const TAG_ERROR_CODE As String = "ErrorCode"
        Private Const TAG_ERROR_MESSAGE As String = "ErrorMessage"
        Private Const TAG_REASON_FOR_VISIT_L1 As String = "ReasonForVisitL1"
        Private Const TAG_L1_ENTRY As String = "L1Entry"
        Private Const TAG_REASON_FOR_VISIT_L2 As String = "ReasonForVisitL2"
        Private Const TAG_L2_ENTRY As String = "L2Entry"
        Private Const TAG_PROF_CODE As String = "ProfCode"
        Private Const TAG_L1_CODE As String = "L1Code"
        Private Const TAG_L1_DESC_ENG As String = "L1DescEng"
        Private Const TAG_L1_DESC_CHI As String = "L1DescChi"
        Private Const TAG_L2_CODE As String = "L2Code"
        Private Const TAG_L2_DESC_ENG As String = "L2DescEng"
        Private Const TAG_L2_DESC_CHI As String = "L2DescChi"

#End Region

#Region "Properties"

        Private _udtReturnErrorCodes As ErrorInfoModelCollection = New ErrorInfoModelCollection
        Public ReadOnly Property ReturnErrorCodes() As ErrorInfoModelCollection
            Get
                Return _udtReturnErrorCodes
            End Get
        End Property

        Private _dtL1Result As DataTable = Nothing
        Public ReadOnly Property ReasonForVisitL1() As DataTable
            Get
                Return _dtL1Result
            End Get
        End Property

        Private _dtL2Result As DataTable = Nothing
        Public ReadOnly Property ReasonForVisitL2() As DataTable
            Get
                Return _dtL2Result
            End Get
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            'Do Nothing
        End Sub

#End Region

        Public Function ProcessRequest(ByRef udtAuditLog As ExtAuditLogEntry) As Boolean
            Try

                Dim udtReasonForVisitBLL As ReasonForVisitBLL = New ReasonForVisitBLL()

                _dtL1Result = udtReasonForVisitBLL.getActiveReasonForVisitL1()
                _dtL2Result = udtReasonForVisitBLL.getActiveReasonForVisitL2()

                udtAuditLog.AddDescripton("L1RecordCount", _dtL1Result.Rows.Count.ToString())
                udtAuditLog.AddDescripton("L2RecordCount", _dtL2Result.Rows.Count.ToString())
                udtAuditLog.WriteLog_Ext(LogID.LOG00053)

                Return True
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.GetReasonForVisitList).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                _udtReturnErrorCodes.Add(ErrorCodeList.I99999)
                Return False
            End Try


        End Function

#Region "Generate XML Result"

        Public Function GenXMLResult() As XmlDocument
            Dim xml As New XmlDocument()

            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)

            Dim nodeResponse As XmlElement
            nodeResponse = xml.CreateElement(TAG_EHS_RESPONSE)
            xml.AppendChild(nodeResponse)

            Dim nodeResult As XmlElement
            nodeResult = xml.CreateElement(TAG_OUTPUT)
            nodeResponse.AppendChild(nodeResult)

            If _udtReturnErrorCodes.Count = 0 Then

                Dim nodeL1Data As XmlElement
                nodeL1Data = xml.CreateElement(TAG_REASON_FOR_VISIT_L1)
                nodeResult.AppendChild(nodeL1Data)

                For Each drL1Result As DataRow In _dtL1Result.Rows
                    GenL1Reason(xml, nodeL1Data, drL1Result)
                Next

                Dim nodeL2Data As XmlElement
                nodeL2Data = xml.CreateElement(TAG_REASON_FOR_VISIT_L2)
                nodeResult.AppendChild(nodeL2Data)

                For Each drL2Result As DataRow In _dtL2Result.Rows
                    GenL2Reason(xml, nodeL2Data, drL2Result)
                Next

                'Dim objXmlGenerator As AbstractXmlGenerator = Nothing
                'Dim objXmlDocument As XmlDocument = Nothing
                'Dim strXmlSchemaFilePath As String = ""
                'Dim dsResult As DataSet = New DataSet

                'strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("GetReasonForVisitListOutput_XmlSchema_FilePath").ToString()

                'dsResult.Tables.Add(_dtL1Result)
                'dsResult.Tables.Add(_dtL2Result)

                'objXmlGenerator = New GetReasonForVisitListXmlGenerator(dsResult, strXmlSchemaFilePath)

                'objXmlDocument = objXmlGenerator.Convert()

                ''Avoid XML sharthand closing tag
                'For Each el As XmlElement In objXmlDocument.SelectNodes("descendant::*[not(node())]")
                '    el.IsEmpty = False
                'Next

                'xml = objXmlDocument
            Else
                'Error exists
                GenReturnCode(xml, nodeResult)
            End If

            Return xml
        End Function

        Private Sub GenReturnCode(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeInfo As XmlElement
            nodeInfo = xml.CreateElement(TAG_ERROR_INFO)
            nodeResult.AppendChild(nodeInfo)

            Dim nodeTmp As XmlElement

            '----------------------------------------------
            'Retrieve External Code and Message
            '----------------------------------------------
            Dim strErrorCode As String = String.Empty
            Dim strErrorMessage As String = String.Empty
            _udtReturnErrorCodes.RetrieveExternalCodeAndMessageToOutside(strErrorCode, strErrorMessage)
            '----------------------------------------------

            'Error Code
            nodeTmp = xml.CreateElement(TAG_ERROR_CODE)
            'nodeTmp.InnerText = udtErrorInfo.ErrorCode
            nodeTmp.InnerText = strErrorCode
            nodeInfo.AppendChild(nodeTmp)

            'Error Message
            nodeTmp = xml.CreateElement(TAG_ERROR_MESSAGE)
            'nodeTmp.InnerText = udtErrorInfo.ErrorMessage
            nodeTmp.InnerText = strErrorMessage
            nodeInfo.AppendChild(nodeTmp)

        End Sub


        Private Sub GenL1Reason(ByVal xml As XmlDocument, ByVal nodeReturnData As XmlElement, ByVal drL1Record As DataRow)
            Dim nodeL1 As XmlElement
            nodeL1 = xml.CreateElement(TAG_L1_ENTRY)
            nodeReturnData.AppendChild(nodeL1)

            Dim nodeTmp As XmlElement

            ' Professional code
            nodeTmp = xml.CreateElement(TAG_PROF_CODE)
            nodeTmp.InnerText = drL1Record("Professional_Code")
            nodeL1.AppendChild(nodeTmp)
            ' L1 code
            nodeTmp = xml.CreateElement(TAG_L1_CODE)
            nodeTmp.InnerText = drL1Record("Reason_L1_Code")
            nodeL1.AppendChild(nodeTmp)
            ' L1 Desc Eng
            nodeTmp = xml.CreateElement(TAG_L1_DESC_ENG)
            nodeTmp.InnerText = drL1Record("Reason_L1")
            nodeL1.AppendChild(nodeTmp)
            ' L1 Desc Chi
            nodeTmp = xml.CreateElement(TAG_L1_DESC_CHI)
            nodeTmp.InnerText = drL1Record("Reason_L1_Chi")
            nodeL1.AppendChild(nodeTmp)

        End Sub

        Private Sub GenL2Reason(ByVal xml As XmlDocument, ByVal nodeReturnData As XmlElement, ByVal drL2Record As DataRow)
            Dim nodeL2 As XmlElement
            nodeL2 = xml.CreateElement(TAG_L2_ENTRY)
            nodeReturnData.AppendChild(nodeL2)

            Dim nodeTmp As XmlElement

            ' Professional code
            nodeTmp = xml.CreateElement(TAG_PROF_CODE)
            nodeTmp.InnerText = drL2Record("Professional_Code")
            nodeL2.AppendChild(nodeTmp)
            ' L1 code
            nodeTmp = xml.CreateElement(TAG_L1_CODE)
            nodeTmp.InnerText = drL2Record("Reason_L1_Code")
            nodeL2.AppendChild(nodeTmp)
            ' L2 code
            nodeTmp = xml.CreateElement(TAG_L2_CODE)
            nodeTmp.InnerText = drL2Record("Reason_L2_Code")
            nodeL2.AppendChild(nodeTmp)
            ' L2 Desc Eng
            nodeTmp = xml.CreateElement(TAG_L2_DESC_ENG)
            nodeTmp.InnerText = drL2Record("Reason_L2")
            nodeL2.AppendChild(nodeTmp)
            ' L2 Desc Chi
            nodeTmp = xml.CreateElement(TAG_L2_DESC_CHI)
            nodeTmp.InnerText = drL2Record("Reason_L2_Chi")
            nodeL2.AppendChild(nodeTmp)

        End Sub

#End Region

    End Class

End Namespace


