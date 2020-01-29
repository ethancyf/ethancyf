Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.Component
Imports Common.Component.DocType


Public Class DocumentTypeDropDownList
    Inherits System.Web.UI.WebControls.DropDownList

#Region "Private Memeber"

    Private _strScheme As String

#End Region



#Region "Property"

    Public ReadOnly Property SelectedDocumentCode() As String
        Get
            Return Me.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SelectedDocumentTypeDescription() As String
        Get
            Return IIf(Not Me.SelectedItem Is Nothing, Me.SelectedItem.Text, String.Empty).ToString()
        End Get
    End Property

    Public Property Scheme() As String
        Get
            Return Me._strScheme
        End Get
        Set(ByVal value As String)
            Me._strScheme = value

            If Me._strScheme <> Me.Attributes("Scheme") Then
                Me.Attributes("SelectedValue") = String.Empty
            End If

            Me.Attributes("Scheme") = value
        End Set
    End Property

#End Region



#Region "Populate Drop Down List"

    Public Sub PopulateDropDownList(ByVal strLanguage As String)
        ' Save the SelectedValue
        Dim strSelectedValue As String = Me.SelectedDocumentCode()

        ' Clear the items
        Me.Items.Clear()

        ' Insert Please Select ---
        Me.InsertDefaultItem()

        ' Load Items from Scheme
        If Not String.IsNullOrEmpty(Me._strScheme) Then
            Dim udtDocTypeBLL As New DocTypeBLL()
            Dim udtDocTypeModelList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
            Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())

            PopularDropDownListInner(udtDocTypeModelList, udtSchemeDocTypeList, strLanguage)
        End If

    End Sub

    Private Sub InsertDefaultItem()
        ' Insert Item: Please Select ---
        Me.Items.Add(New ListItem(HttpContext.GetGlobalResourceObject("Text", "EHSClaimPleaseSelect").ToString(), String.Empty))
    End Sub

    Private Sub PopularDropDownListInner(ByVal udtDocTypeModelList As DocType.DocTypeModelCollection, ByVal udtSchemeDocTypeList As DocType.SchemeDocTypeModelCollection, ByVal strLanguage As String)
        Dim strDocumentTypes As Dictionary(Of String, DocumentInfo) = New Dictionary(Of String, DocumentInfo)
        Dim documentInfo As DocumentInfo = Nothing

        ' Init Document Type Status, set all disable
        For Each udtDocTypeModel As DocType.DocTypeModel In udtDocTypeModelList
            documentInfo = New DocumentInfo(udtDocTypeModel)
            strDocumentTypes.Add(documentInfo.DocCode.Trim(), documentInfo)
        Next

        ' Save the Popular Doc Code
        Dim strPopularDoc As String = String.Empty

        ' Enable Document Type Status, according to Scheme Doc Type
        For Each udtSchemeDocTypeModel As DocType.SchemeDocTypeModel In udtSchemeDocTypeList
            documentInfo = strDocumentTypes.Item(udtSchemeDocTypeModel.DocCode.Trim())
            If Not documentInfo Is Nothing Then
                documentInfo.IsEnable = True
                If udtSchemeDocTypeModel.IsMajorDoc Then
                    strPopularDoc = udtSchemeDocTypeModel.DocCode.Trim()
                End If
            End If
        Next

        ' Add item into the Drop Down List
        For Each docType As DocumentInfo In strDocumentTypes.Values
            If docType.IsEnable Then
                Dim strValue As String = docType.DocCode.Trim()
                Dim strDescription As String = IIf(strLanguage = CultureLanguage.English, docType.DocName, docType.DocNameChi).ToString()
                Dim item As ListItem = New ListItem(strDescription, strValue)

                Me.Items.Add(item)
            End If
        Next

        ' Select Default Item
        If Not String.IsNullOrEmpty(strPopularDoc) Then
            Me.SelectedValue = strPopularDoc
        End If

    End Sub

#End Region



End Class
