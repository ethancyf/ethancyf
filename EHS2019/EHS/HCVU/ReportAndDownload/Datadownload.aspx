<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="Datadownload.aspx.vb" Inherits="HCVU.Datadownload"
    Title="<%$ Resources:Title, DownloadReportDataDownload %>" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" src="../JS/Common.js" type="text/javascript"></script>
    <script type="text/javascript">

        function SelectAll(id) {
            var frm = document.forms[0];
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox") {
                    frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
        }

        function CheckOCXExists(winwasher) {
            winwasher = winwasher.replace(/\?/g, "\\\\");
            setTimeout('CheckOCXExistsContent ("' + winwasher + '")', 1)
            return false;
        }

        function CheckOCXExistsContent(winwasherFolder) {
            var objDownload = document.getElementById("DataDownload");
            var objHiddenFieldResult = document.getElementById("ctl00_ContentPlaceHolder1_HiddenField1");
            objHiddenFieldResult.value = "";
            try {
                //alert('Checking OCX');  
                var bFolderExists = objDownload.getFolderExists(winwasherFolder);
                if (!bFolderExists) {
                    //alert('Folder not exists');   
                    objHiddenFieldResult.value = "00002";
                    return;
                }
            }
            catch (err) {
                //alert('CheckOCXExists Exception');
                objHiddenFieldResult.value = "00001";
                return;
            }
        }

        function DownloadFile(url, file, winwasher) {

            //alert(url);

            url = url.replace(/\|/g, "\\\\");
            //url = url.replace(/\|/g, "\\\\\\\\");
            winwasher = winwasher.replace(/\?/g, "\\\\");
            setTimeout('FileDownload("' + url + '", "' + file + '","' + winwasher + '")', 1)

            return false;
        }

        function FileDownload(url, file, winwasher) {
            var objDownload = document.getElementById("DataDownload");
            var objHiddenFieldResult = document.getElementById("ctl00_ContentPlaceHolder1_HiddenField1");
            //objHiddenFieldResult.value = "";

            try {
                var size = objDownload.DownloadtoWindowWasher(url, file, winwasher);
                if (size < 0) {
                    //alert('size < 0');
                    if (size == -1) {
                        objHiddenFieldResult.value = "00002";
                    }
                    else if (size == -4) {
                        objHiddenFieldResult.value = "00004";
                    }
                    else {
                        objHiddenFieldResult.value = "00003";
                    }

                    var objHiddenFieldResultBtn = document.getElementById("ctl00_ContentPlaceHolder1_HiddenSuccessBtn");
                    objHiddenFieldResultBtn.click();
                    return;
                }
                else {
                    //alert('normal');   
                    objHiddenFieldResult.value = "";
                    var objHiddenFieldResultBtn = document.getElementById("ctl00_ContentPlaceHolder1_HiddenSuccessBtn");
                    objHiddenFieldResultBtn.click();
                    return;
                }
            }
            catch (err2) {
                //alert('exception again');   
                objHiddenFieldResult.value = "00003";
                return;
            }


            /*		    
            var filesize;
            var filepath = d + ":\\" + "CMIS" + "\\" + file
            
            var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            
            alert(xmlhttp);
                //xmlhttp.open("POST", "searchherbs.aspx?herbs_type=" + herbs_type + "&external_use=" + ExternalUse + "&herbs_name="+ escape(herbs_name), true);
            xmlhttp.open("POST", "datadownloadsize.aspx", false);
            
            xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");		
            xmlhttp.onreadystatechange = function() {
                if ( xmlhttp.readyState == 4 ){
                    filesize = xmlhttp.responseText;
                    alert("filesize" + filesize)
                }
                    
            }
            xmlhttp.send(null);
                                            
            if (size > 0 && filesize > 0){
                if (filesize == size){
                    
                    document.all.SpanMsg.innerHTML = "<span style='position: relative; top: 5;'><FONT size='4' >&nbsp;資料成功下載到:</Font>" + "<br><br><span style='position: relative; top: -10'>&nbsp;&nbsp;<span style='font-size: 12pt; font-weight: bold'>" + filepath + "</span></span></span>"
                    //document.all.SpanMsg.innerHTML = "<FONT size='4' >&nbsp;資料成功下載到:</Font>" + "<br><br><span style='position: relative; top: -10'>&nbsp;&nbsp;<FONT size='3'><b>" + d + ":\\" + "CMIS" + "\\" + "CMIS0001D_2007-02-05 14_29_34.zip" + "</b></FONT></span>"
                    if(document.all.btnReturn != null){
                        document.all.btnReturn.disabled = false;
                        document.all.btnReturn.src = "../Images/DataDownload/ConfirmS.jpg"
                    }
                    //setTimeout('window.close()', 2400);
                    
                    alert('Download OK');
                }
                else{
                    
                    document.all.SpanMsg.innerHTML = "<FONT size='4' style='position: relative; top: 18;'>&nbsp;系統發生錯誤, 下載資料經已損壞</Font>" + "<br><br><span style='position: relative; top: -10'>&nbsp;&nbsp;<FONT size='4' ></FONT></span>"
                    document.all.SpanMsg.innerHTML += "<span style='font-size:10pt'>- 下載資料不存在</span>"
                    if(document.all.btnReturn != null){
                        document.all.btnReturn.disabled = false;
                        document.all.btnReturn.src = "../Images/DataDownload/ConfirmS.jpg"
                    }
                    
                    alert('Download Fail!!');
                }					
            }
            else{
                
                //alert('Error')
                document.all.SpanMsg.innerHTML = "<FONT size='4' style='position: relative; top: 18;'>&nbsp;系統發生錯誤, 資料下載失敗</Font>" + "<br><br><span style='position: relative; top: -10'>&nbsp;&nbsp;<FONT size='4' ></FONT></span>"
                if(size == -1){
                    document.all.SpanMsg.innerHTML += "<span style='font-size:10pt'>- 下載目標磁碟不存在</span>"
                }
                else if (size == -2){
                    document.all.SpanMsg.innerHTML += "<span style='font-size:10pt'>- 檔案下載失敗</span>"
                }				
                if(document.all.btnReturn != null){
                    document.all.btnReturn.disabled = false;
                    document.all.btnReturn.src = "../Images/DataDownload/ConfirmS.jpg"
                }
                
                alert('Download Fail!!!!!');
            }
            */
        }

        function btnReturn_Click() {
            //setTimeout('window.close()', 1)
            window.close();
            return false;
        }

    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Image ID="img_header" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReportDataDownloadBanner %>"
                ImageAlign="AbsMiddle" AlternateText="<%$ Resources:AlternateText, ReportDataDownloadBanner %>">
            </asp:Image>
            <br />

            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <cc2:MessageBox ID="udcErrorMessage" runat="server" Width="90%" />
                    <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="90%" />
                    <br />
                    <asp:Label ID="lbl_inboxSelect" runat="server" Text="<%$ Resources:Text, SelectFolder %>"></asp:Label>&nbsp;<asp:RadioButton
                        ID="rbMyFolder" runat="server" AutoPostBack="True" GroupName="msgType" Text="<%$ Resources:Text, MyFolder %>"
                        OnCheckedChanged="rbMyFolder_CheckedChanged" />&nbsp;<asp:RadioButton
                            ID="rbRecycleBin" runat="server" AutoPostBack="True" GroupName="msgType" Text="<%$ Resources:Text, RecycleBin %>"
                            OnCheckedChanged="rbRecycleBin_CheckedChanged" /><br />
                    <asp:TextBox ID="HiddenField1" runat="server" Style="display: none"></asp:TextBox>                    
                    <asp:Button ID="HiddenSuccessBtn" runat="server" CausesValidation="false" Enabled="true"
                        Style="display: none"></asp:Button>
                    <asp:TextBox ID="hfSelectedIndex" runat="server" Style="display: none"></asp:TextBox>
                    <asp:Panel runat="server" ID="panel_Folder">
                        <asp:GridView ID="gvDataDownloadFolder" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            Width="90%" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="gvDataDownloadFolder_PageIndexChanging"
                            OnPreRender="gvDataDownloadFolder_PreRender" OnSorting="gvDataDownloadFolder_Sorting"
                            AllowSorting="True" OnRowCommand="GridView1_RowCommand">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("lineNum") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblGenerationID" runat="server" Text='<%# Eval("GenerationID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" VerticalAlign="Top" />
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="HeaderLevelCheckBox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_selected" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <itemstyle width="80px" horizontalalign="Center" />
                                        <asp:ImageButton ID="lbtn_ReadyDownload" runat="server" CausesValidation="false"
                                            CommandName="DataDownload" Enabled="true" AlternateText="<%$ Resources:AlternateText, ReadyDownloadBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ReadyDownloadBtn %>" Visible="false"></asp:ImageButton>
                                        <asp:ImageButton ID="lbtn_Processing" runat="server" CausesValidation="false" Enabled="false"
                                            AlternateText="<%$ Resources:AlternateText, ProcessingBtn %>" ImageUrl="<%$ Resources:ImageUrl, ProcessingBtn %>"
                                            Visible="false"></asp:ImageButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="DisplayCode" HeaderText="<%$ Resources:Text, ReportFileNumber %>"
                                    SortExpression="DisplayCode">
                                    <ItemStyle Width="100px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FileDescription" HeaderText="<%$ Resources:Text, ReportFileName %>"
                                    SortExpression="FileDescription">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:TemplateField SortExpression="submissionDate" HeaderText="<%$ Resources:Text, SubmissionDtmTime %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubmissionDtm" runat="server" Text='<%# Eval("submissionDate") %> '></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="130px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="submittedBy" HeaderText="<%$ Resources:Text, SubmittedBy %>"
                                    SortExpression="submittedBy">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False" Visible="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lbtn_FilePassword" runat="server" CausesValidation="false" CommandName=""
                                            Enabled="false" AlternateText="<%$ Resources:AlternateText, FilePasswordBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, FilePasswordBtn %>" Visible="false"></asp:ImageButton>
                                        <asp:ImageButton ID="lbtn_FilePasswordDisabled" runat="server" CausesValidation="false"
                                            Enabled="false" AlternateText="<%$ Resources:AlternateText, FilePasswordBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, FilePasswordDisabledBtn %>" Visible="false">
                                        </asp:ImageButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="180px" VerticalAlign="Top" />
                                    <HeaderStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:ImageButton ID="ibtn_delete" runat="server" AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>" OnClick="ibtn_delete_Click" />
                        <asp:ImageButton ID="btn_delete_disabled" runat="server" AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, DeleteSDisableBtn %>" Enabled="False" />
                        <asp:ImageButton ID="ibtn_undelete" runat="server" AlternateText="<%$ Resources:AlternateText, UndeleteSBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, UndeleteSBtn %>" OnClick="ibtn_undelete_Click" />
                        <asp:Label ID="lbl_KeepFilePeriodNote" runat="server" CssClass="tableText" Text="<%$ Resources:Text, DownloadFileKeepPeriodText %>"></asp:Label>
                        <asp:Label ID="lbl_RecycleBinNote" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RecycleBinNote %>"></asp:Label><br />
                    </asp:Panel>
                    <asp:Panel ID="panel_searchCriteria" runat="server" Visible="false">
                        <asp:Label ID="lbl_filePasswordLabel" runat="server" CssClass="tableText" Text="<%$ Resources:Text, FilePassword %>"></asp:Label>
                        <asp:Label ID="lbl_SelectedReportName" runat="server" CssClass="tableText"></asp:Label>
                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td style="width: 200px; height: 36px" valign="top">
                                    <asp:Label ID="lbl_accAct_newPW" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Password %>"></asp:Label>
                                </td>
                                <td valign="top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txt_accAct_newPW" runat="server" MaxLength="15" TextMode="Password"
                                                    Width="200px"></asp:TextBox>
                                                <asp:Image ID="img_err_webNewPW" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                            </td>
                                            <td valign="top">
                                                <table cellpadding="0" style="width: 290px">
                                                    <tr>
                                                        <td colspan="5">
                                                            <div id="progressBar" style="border-right: white 1px solid; border-top: white 1px solid;
                                                                font-size: 1px; border-left: white 1px solid; width: 290px; border-bottom: white 1px solid;
                                                                height: 10px">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 290px">
                                                        <td align="center" style="width: 30%">
                                                            <span id="strength1"></span>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <span id="direction1"></span>
                                                        </td>
                                                        <td align="center" style="width: 30%">
                                                            <span id="strength2"></span>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <span id="direction2"></span>
                                                        </td>
                                                        <td align="center" style="width: 30%">
                                                            <span id="strength3"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lbl_accAct_confirmPW" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmPW %>"
                                        Visible="False"></asp:Label></td>
                                <td valign="top">
                                    <asp:TextBox ID="txt_accAct_confirmPW" runat="server" MaxLength="15" TextMode="Password"
                                        Width="200px" Visible="False"></asp:TextBox><asp:Image ID="img_err_webConfirmPW"
                                            runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top"></td>
                                <td valign="top">
                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Text, FileDownloadPasswordTips %>"></asp:Label>
                                    <br />
                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Text, WebPasswordTips1-3Rule %>"></asp:Label><br />
                                    &nbsp; &nbsp;
                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label><br />
                                    &nbsp; &nbsp;
                    <asp:Label ID="Label74" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label><br />
                                    &nbsp; &nbsp;
                    <asp:Label ID="Label75" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label><br />
                                    &nbsp; &nbsp;
                    <asp:Label ID="Label76" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                    <asp:Label ID="Label77" runat="server" Text="<%$ Resources:Text, FilePasswordTips2 %>"></asp:Label><br />
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" valign="top">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 60px">
                                                <asp:ImageButton ID="btn_back" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btn_back_Click" />
                                            </td>
                                            <td align="center">
                                                <asp:ImageButton ID="btn_Download" runat="server" AlternateText="<%$ Resources:AlternateText, DownloadBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, DownloadBtn %>" OnClick="btn_Download_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Button ID="btnHiddenShowDialog" runat="server" Style="display: none" />
                    <cc1:ModalPopupExtender
                        ID="ModalPopupExtenderConfirmDelete" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" PopupControlID="panConfirmMsg" PopupDragHandleControlID="panConfirmMsgHeading"
                        RepositionMode="None" TargetControlID="btnHiddenShowDialog">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none">
                        <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                    </td>
                                    <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                        color: #ffffff; background-repeat: repeat-x; height: 35px">
                                        <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                                </td>
                                <td style="background-color: #ffffff">
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="left" style="width: 10px; height: 42px" valign="middle">
                                                <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" />
                                            </td>
                                            <td align="center" style="height: 42px; width: 100%;">
                                                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmDeleteFile %>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="btn_confirm_Click" />
                                                <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDialogCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/dialog/bottom-left.png); width: 9px; height: 7px">
                                </td>
                                <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                                    height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                                    height: 7px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    &nbsp;<table width="100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="btn_Return" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                    OnClick="btn_Return_Click" /></td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <asp:Label ID="lblScript" runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
