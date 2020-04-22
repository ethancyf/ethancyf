Imports System.Data
Imports System.Xml
Imports Common.ComFunction.Generator
Imports Common.ComFunction

Public Class GetReasonForVisitListXmlGenerator
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
            Dim dtL1 As DataTable
            Dim dtL2 As DataTable
            Dim dr As DataRow
            Dim drRecord As DataRow

            ' Fill result record
            dtL1 = ds.Tables("ReasonForVisitL1")
            dr = dtL1.NewRow
            dr("ReasonForVisitL1_Id") = 0
            dtL1.Rows.Add(dr)

            dtL2 = ds.Tables("ReasonForVisitL2")
            dr = dtL2.NewRow
            dr("ReasonForVisitL2_Id") = 0
            dtL2.Rows.Add(dr)

            For Each drRecord In _dsSource.Tables(0).Rows
                dt = ds.Tables("L1Entry")
                dr = dt.NewRow
                dr("ProfCode") = drRecord("Professional_Code")
                dr("L1Code") = drRecord("Reason_L1_Code")
                dr("L1DescEng") = drRecord("Reason_L1")
                dr("L1DescChi") = drRecord("Reason_L1_Chi")
                dr("ReasonForVisitL1_Id") = 0
                dt.Rows.Add(dr)
            Next

            For Each drRecord In _dsSource.Tables(1).Rows
                dt = ds.Tables("L2Entry")
                dr = dt.NewRow
                dr("ProfCode") = drRecord("Professional_Code")
                dr("L1Code") = drRecord("Reason_L1_Code")
                dr("L2Code") = drRecord("Reason_L2_Code")
                dr("L2DescEng") = drRecord("Reason_L2")
                dr("L2DescChi") = drRecord("Reason_L2_Chi")
                dr("ReasonForVisitL2_Id") = 0
                dt.Rows.Add(dr)
            Next

            ' Convert Dataset to XML
            ' -------------------------------------------------------------------------
            Dim xmlDoc As XmlDocument = XmlFunction.Dataset2Xml_forIMMD(ds, XmlWriteMode.IgnoreSchema)
            'Me.ReplaceRootBy2ndRoot(xmlDoc)

            Return xmlDoc
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
