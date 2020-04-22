Imports System.Data
Imports System.Xml
Imports ImmDValidation.XMLGenerator
Imports Common.ComFunction.Generator
Imports Common.ComFunction

Namespace XMLGenerator

    Public Class HKIC_ECExportXmlGenerator
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
                Dim dtIC As DataTable
                Dim dtEC As DataTable

                Dim dr As DataRow
                Dim drRecord As DataRow

                'dt = ds.Tables("HA_HealthCareVoucher")

                dtIC = ds.Tables("icList")
                dr = dtIC.NewRow
                dtIC.Rows.Add(dr)

                dtEC = ds.Tables("ecList")
                dr = dtEC.NewRow
                dtEC.Rows.Add(dr)

                For Each drRecord In _dsSource.Tables(0).Rows
                    If drRecord("Doc_Code").ToString().Trim = "HKIC" Then
                        dt = ds.Tables("icEntry")
                        dr = dt.NewRow
                        dr("seqNo") = drRecord("seqNo")
                        'HKIC --> icPrf  (A space character is expected for cases without IC prefix)
                        If drRecord("icPrf").ToString().Trim = "" Then
                            dr("icPrf") = " "
                        Else
                            dr("icPrf") = drRecord("icPrf")
                        End If
                        dr("icNo") = drRecord("icNo")
                        dr("dtIcReg") = drRecord("dtIcReg")
                        dr("dobOnCard") = drRecord("dobOnCard")
                        dr("icList_Id") = 0
                        dt.Rows.Add(dr)
                    ElseIf drRecord("Doc_Code").ToString().Trim = "EC" Then
                        dt = ds.Tables("ecEntry")
                        dr = dt.NewRow
                        dr("seqNo") = drRecord("seqNo")
                        dr(1) = drRecord("applnRefNo")
                        'dr("appInRefNo") = drRecord("applnRefNo")
                        dr("audNo") = drRecord("audNo")
                        dr("dobFlag") = drRecord("dobFlag")
                        dr("appltDOB") = drRecord("appltDOB")
                        dr("appltYOB") = drRecord("appltYOB")
                        dr("appltDtIcReg") = drRecord("appltDtIcReg")
                        dr("ageOnDt") = drRecord("ageOnDt")
                        dr("ecList_Id") = 0
                        dt.Rows.Add(dr)
                    End If
                Next

                ' Convert Dataset to XML
                ' -------------------------------------------------------------------------
                Dim xmlDoc As XmlDocument = XmlFunction.Dataset2Xml_forIMMD(ds, XmlWriteMode.IgnoreSchema)

                'Remove (space = "preserve") attribute
                For Each xNode As XmlNode In xmlDoc.ChildNodes
                    RemoveAttr(xNode)
                Next

                'Me.ReplaceRootBy2ndRoot(xmlDoc)

                Return xmlDoc
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Private Sub RemoveAttr(ByVal xN As XmlNode)
            If Not xN.Attributes Is Nothing Then
                xN.Attributes.RemoveNamedItem("xml:space")
                'xN.Attributes.RemoveAll()
            End If
            For Each xCNode As XmlNode In xN.Childnodes
                'Recursive call
                RemoveAttr(xCNode)
            Next
        End Sub
    End Class

End Namespace


