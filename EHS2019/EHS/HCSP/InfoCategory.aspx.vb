Imports Common
Imports Common.Component
Imports Common.ComFunction
Imports Common.Format

Public Class InfoCategory
    Inherits System.Web.UI.Page

    Private udtFormatter As New Formatter
    Private _udtCommfunct As GeneralFunction = New GeneralFunction

    Public Const ZH As String = "ZH"
    Public Const EN As String = "EN"

    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strLang As String = Request.QueryString("Language")
        Dim strLanguage As String = English

        Select Case strLang.ToUpper
            Case ZH
                strLanguage = TradChinese
            Case EN
                strLanguage = English
            Case Else
                strLanguage = English
        End Select

        'Dim strLanguage As String = Request.QueryString("Language")
        'If strLanguage Is Nothing OrElse (strLanguage <> TradChinese And strLanguage <> SimpChinese And strLanguage <> English) Then
        '    strLanguage = English
        'End If

        'Table Title
        tcCategoryTitle.Text = HttpContext.GetGlobalResourceObject("Text", "Category", New System.Globalization.CultureInfo(strLanguage))
        tcSubCategoryTitle.Text = HttpContext.GetGlobalResourceObject("Text", "SubCategory", New System.Globalization.CultureInfo(strLanguage))

        Dim dtMainCategory As DataTable = Status.GetDescriptionListFromDBEnumCode("VSSC19MainCategory")
        Dim dtSubCategory As DataTable = Status.GetDescriptionListFromDBEnumCode("VSSC19SubCategory")
        'Build a dynamic Table
        For mainCatRow As Integer = 0 To dtMainCategory.Rows.Count - 1
            Dim lblMainCat As Label = New Label()


            Select Case strLanguage
                Case English
                    lblMainCat.Text = dtMainCategory.Rows(mainCatRow).Item("Status_Description")
                Case TradChinese
                    lblMainCat.Text = dtMainCategory.Rows(mainCatRow).Item("Status_Description_Chi")
                Case SimpChinese
                    lblMainCat.Text = dtMainCategory.Rows(mainCatRow).Item("Status_Description_CN")
                Case Else
                    lblMainCat.Text = dtMainCategory.Rows(mainCatRow).Item("Status_Description")
            End Select




            Dim dvSubCategory As DataView = New DataView(dtSubCategory)
            'Filter with the specific Category
            dvSubCategory.RowFilter = "[Column_Name] = '" + dtMainCategory.Rows(mainCatRow).Item("Column_Name") + "'"
            dvSubCategory.Sort = "Display_Order asc"




            Dim tr As TableRow = New TableRow()
            Dim tcMain As TableCell = New TableCell() 'first column cell
            Dim tcSub As TableCell = New TableCell() 'second column cell
            tcMain.Style.Add("vertical-align", "top")
            tcMain.Style.Add("padding", "5px 5px 5px 5px;")
            tcSub.Style.Add("vertical-align", "top")
            Dim blSubCategory As BulletedList = New BulletedList()
            Dim lblSubCategoryNA As Label = New Label()


            If dvSubCategory.Count = 0 Then
                lblSubCategoryNA.Text = "(" & HttpContext.GetGlobalResourceObject("Text", "NA", New System.Globalization.CultureInfo(strLanguage)) & ")"
                tcSub.Controls.Add(lblSubCategoryNA)
                tcSub.Style.Add("padding", "5px 15px 5px 20px;")
            Else
                Select Case strLanguage
                    Case English
                        blSubCategory.DataTextField = "Status_Description"
                    Case TradChinese
                        blSubCategory.DataTextField = "Status_Description_Chi"
                    Case SimpChinese
                        blSubCategory.DataTextField = "Status_Description_CN"
                    Case Else
                        blSubCategory.DataTextField = "Status_Description"
                End Select
                blSubCategory.DataSource = dvSubCategory
                blSubCategory.DataBind()
                blSubCategory.BulletStyle = BulletStyle.Disc
                tcSub.Controls.Add(blSubCategory)
                tcSub.Style.Add("padding", "5px 15px 5px 0px;")
                blSubCategory.Style.Add("padding", "0px 0px 0px 20px;")
            End If
            tcMain.Controls.Add(lblMainCat)

            tr.Cells.Add(tcMain)
            tr.Cells.Add(tcSub)

            tbCategory.Rows.Add(tr)
        Next


        'Effective Date
        Dim strCategoryEffectiveDate As String = String.Empty
        _udtCommfunct.getSystemParameter("CategoryEffectiveDate", strCategoryEffectiveDate, String.Empty, "ALL")
        lblEffectiveDate.Text = udtFormatter.formatECDORegistration(strLanguage, strCategoryEffectiveDate)
        lblEffectiveDateText.Text = HttpContext.GetGlobalResourceObject("Text", "EffectiveDate", New System.Globalization.CultureInfo(strLanguage)) + ":"
        'Remark
        lblRemart.Text = HttpContext.GetGlobalResourceObject("Text", "PopUpCategoryRemark", New System.Globalization.CultureInfo(strLanguage))
        'close button
        ibtnClose.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "CloseBtn", New System.Globalization.CultureInfo(strLanguage))
        ibtnClose.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "CloseBtn", New System.Globalization.CultureInfo(strLanguage))



    End Sub

End Class