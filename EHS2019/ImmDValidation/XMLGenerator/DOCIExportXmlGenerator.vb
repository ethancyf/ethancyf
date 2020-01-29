Imports System.Data
Imports System.Xml
Imports ImmDValidation.XMLGenerator
Imports Common.ComFunction.Generator
Imports Common.ComFunction

Namespace XMLGenerator

    Public Class DOCIExportXmlGenerator
        Inherits AbstractXmlGenerator

        Private _dsSource As DataSet
        Private _sXsdFilePath As String

        Public Sub New(ByVal dsSource As DataSet, ByVal sXsdFilePath As String)
            _dsSource = dsSource
            _sXsdFilePath = sXsdFilePath
        End Sub

        Public Overrides Function Convert() As System.Xml.XmlDocument
            Try

                ' Create empty dataset by schema
                ' -------------------------------------------------------------------------
                Dim ds As DataSet = DataSetFunction.CreateDataSetByXsd(_sXsdFilePath)

                ' Fill value to dataset
                ' -------------------------------------------------------------------------
                Dim dt As DataTable
                Dim dr As DataRow
                Dim drRecord As DataRow
                Dim strDOI As String = String.Empty

                ' Fill result record
                dt = ds.Tables("HA_Request")
                dr = dt.NewRow
                dr("Doctype") = "DOCI"
                dr("NumberOfRecord") = _dsSource.Tables(0).Rows.Count
                dt.Rows.Add(dr)

                Dim udtCommonFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
                Dim strDI_DOI As String = String.Empty
                udtCommonFunction.getSystemParameter("DI_DOI", strDI_DOI, String.Empty)
                Dim dtmDI_DOI As New Date
                dtmDI_DOI = CDate(strDI_DOI)
                strDI_DOI = dtmDI_DOI.ToString("yyyyMMdd").Trim

                ' Fill vaccination_record
                dt = ds.Tables("tdEntry")
                For Each drRecord In _dsSource.Tables(0).Rows
                    dr = dt.NewRow

                    strDOI = drRecord("dtIcReg")
                    'If strDOI.Trim >= "20030901" Then
                    If strDOI.Trim >= strDI_DOI Then
                        dr("Tno") = drRecord("identifyNo")
                        dr("Name") = ""
                        dr("Sex") = ""
                    Else
                        dr("Tno") = ""
                        dr("Name") = drRecord("nameOnCard")
                        dr("Sex") = drRecord("sexOnCard")
                    End If

                    dr("SeqNo") = drRecord("seqNo")
                    dr("DOB") = drRecord("dobOnCard")
                    dr("DOI") = drRecord("dtIcReg")
                    dr("HA_Request_Id") = 0
                    dt.Rows.Add(dr)
                Next

                ' Convert Dataset to XML
                ' -------------------------------------------------------------------------
                Dim xmlDoc As XmlDocument = XmlFunction.Dataset2Xml_forIMMD(ds, XmlWriteMode.IgnoreSchema)
                Me.ReplaceRootBy2ndRoot(xmlDoc)

                Return xmlDoc
            Catch ex As Exception
                Throw ex
            End Try
        End Function

    End Class

End Namespace


