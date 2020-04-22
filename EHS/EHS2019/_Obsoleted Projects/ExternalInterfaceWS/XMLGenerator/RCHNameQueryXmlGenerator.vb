Imports System.Data
Imports System.Xml
Imports Common.ComFunction.Generator
Imports Common.ComFunction


Namespace XMLGenerator

    Public Class RCHNameQueryXmlGenerator
        Inherits AbstractXmlGenerator

        Private _dtSource As DataTable
        Private _sXsdFilePath As String

        Public Sub New(ByVal dtSource As DataTable, ByVal sXsdFilePath As String)
            _dtSource = dtSource
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

                ' Fill result record
                dt = ds.Tables("Output")
                dr = dt.NewRow
                dr("HomeNameEng") = _dtSource.Rows(0).Item("Homename_Eng")
                dr("HomeNameChi") = _dtSource.Rows(0).Item("Homename_Chi")
                dt.Rows.Add(dr)

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

