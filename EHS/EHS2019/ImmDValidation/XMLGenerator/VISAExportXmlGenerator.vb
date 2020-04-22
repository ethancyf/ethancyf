Imports System.Data
Imports System.Xml
Imports ImmDValidation.XMLGenerator
Imports Common.Format
Imports Common.ComFunction.Generator
Imports Common.ComFunction


Namespace XMLGenerator

    Public Class VISAExportXmlGenerator
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

                Dim udtFormatter As Formatter = New Formatter

                ' Fill result record
                dt = ds.Tables("HA_Request")
                dr = dt.NewRow
                dr("Doctype") = "VISA"
                dr("NumberOfRecord") = _dsSource.Tables(0).Rows.Count
                dt.Rows.Add(dr)

                ' Fill vaccination_record
                dt = ds.Tables("tdEntry")
                For Each drRecord In _dsSource.Tables(0).Rows
                    dr = dt.NewRow
                    dr("SeqNo") = drRecord("seqNo")
                    'Format the VISA no
                    dr("ARN") = udtFormatter.FormatDocIdentityNoForDisplay("VISA", drRecord("identifyNo").ToString().Trim, False)
                    dr("DOB") = drRecord("dobOnCard")
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

