<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="spProfile.aspx.vb" Inherits="HCVU.spProfile" Title="<%$ Resources:Title, SPDataEnty %>" %>

<%@ Register Src="MOPracticeLists.ascx" TagName="MOPracticeLists" TagPrefix="uc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc3" %>
<%@ Register Src="~/UIControl/PCDIntegration/ucPCDWarningPopUp.ascx" TagName="ucPCDWarningPopUp" TagPrefix="uc4" %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        input[type="text"] {
            border-style: solid;
            border-width: 1px;
            border-color: #666666;
        }

        .chkLabel {
            text-indent: -19px;
            display: block;
            padding-left: 19px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" src="../JS/Common.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        function enableRemarkTextbox(rbo, txtbox) {
            var radioObj = document.getElementById(rbo);
            var radioList = radioObj.getElementsByTagName('input');
            for (var i = 0; i < radioList.length; i++) {
                if (radioList[i].checked) {
                    if (radioList[i].value == 'O') {
                        document.getElementById(txtbox).readOnly = false;
                        document.getElementById(txtbox).style.backgroundColor = '';
                    }
                    else {
                        document.getElementById(txtbox).readOnly = true;
                        document.getElementById(txtbox).value = '';
                        document.getElementById(txtbox).style.backgroundColor = '#f5f5f5';
                    }
                }
            }
        }

        function enableSeviceFeeTextbox(chk, txtbox) {
            var chkObj = document.getElementById(chk);
            if (chkObj.checked) {
                document.getElementById(txtbox).readOnly = true;
                document.getElementById(txtbox).value = '';
                document.getElementById(txtbox).style.backgroundColor = '#f5f5f5';
            }
            else {
                document.getElementById(txtbox).readOnly = false;
                document.getElementById(txtbox).style.backgroundColor = '';
            }
        }

        function chkFreeTextChanged(chkFreeText, tBankAcc, tbBankCode, tbBranchCode, tbAccNo, tbAccNoFreeText, tbAccName, tbAccNameFreeText) {

            if (chkFreeText.checked) {
                document.getElementById(tBankAcc).style.display = 'none';
                document.getElementById(tbBankCode).value = '';
                document.getElementById(tbBranchCode).value = '';
                document.getElementById(tbAccNo).value = '';
                document.getElementById(tbAccNoFreeText).style.display = 'block';
                document.getElementById(tbAccNameFreeText).value = document.getElementById(tbAccName).value;
                document.getElementById(tbAccName).style.display = 'none';
                document.getElementById(tbAccNameFreeText).style.display = 'inline';
            }
            else {
                document.getElementById(tbAccNoFreeText).style.display = 'none';
                document.getElementById(tbAccNoFreeText).value = '';
                document.getElementById(tBankAcc).style.display = 'block';
                document.getElementById(tbAccName).value = '';
                document.getElementById(tbAccNameFreeText).value = '';
                document.getElementById(tbAccName).style.display = 'inline';
                document.getElementById(tbAccNameFreeText).style.display = 'none';

            }
        }

        function selectScheme() {
            $(document).ready(function () {

                $("[id$='chkEditSelect']").click(function () {
                    var scheme = $(this).attr("scheme");
                    var chkScheme = $(this).is(':checked');

                    var hasSubsidize = false;
                    $("[schemedepend=" + scheme + "]").each(function () {

                        var subsidize = $(this).attr("subsidize");

                        if (subsidize != undefined) {
                            hasSubsidize = true;

                            var chk = $(this).find('input:checkbox');

                            if (chkScheme) {

                                if (chk.length) {
                                    chk.parent().removeAttr("disabled");
                                    chk.removeAttr("disabled");
                                    chk.prop("checked", false);

                                    //disable service fee
                                    enableServiceFee(false, subsidize);
                                }
                                else {
                                    //enable service fee
                                    enableServiceFee(true, subsidize);
                                }
                                $(this).removeAttr("disabled");
                            }
                            else {

                                if (chk.length) {
                                    chk.parent().attr("disabled", "disabled");
                                    chk.attr("disabled", "disabled");
                                    chk.prop("checked", false);
                                }
                                //disable service fee
                                enableServiceFee(false, subsidize);

                                $(this).attr("disabled", "disabled");
                            }

                        } else {
                            // Non-clinic checkbox
                            if (chkScheme) {
                                if (!($(this).find("span").attr("alwaysdisable") == "1")) {
                                    $(this).removeAttr("disabled");
                                    $(this).find("span").removeAttr("disabled");
                                    $(this).find("> span > input").removeAttr("disabled");
                                }

                            } else {
                                $(this).attr("disabled", "disabled");
                                $(this).find("span").attr("disabled", "disabled");
                                $(this).find("> span > input").prop("checked", false);
                            }

                        }

                        //handle not display subsidize case
                        if (!hasSubsidize) {
                            if (chkScheme) {
                                $(this).removeAttr("disabled");
                            }
                            else {
                                $(this).attr("disabled", "disabled");
                            }
                        }
                    })

                });

            });

            $("[id$='pnlEditPracticeSchemeSubsidize']").click(function () {

                var chk = $(this).find('input:checkbox');

                if (chk.length) {
                    chk.click();

                    var chkSubsidize = chk.is(':checked');
                    var subsidize = $(this).attr("subsidize");

                    if (chkSubsidize) {
                        //enable service fee
                        enableServiceFee(true, subsidize);
                    }
                    else {
                        //disable service fee
                        enableServiceFee(false, subsidize);
                    }
                }
            });


            function enableServiceFee(enabled, subsidize) {

                var servicefee = $("[subsidizedepend=" + subsidize + "]");
                var chk = servicefee.find('input:checkbox');
                var tb = servicefee.find('input:text');

                if (enabled) {

                    if (chk.length) {
                        chk.parent().removeAttr("disabled");
                        chk.removeAttr("disabled");
                        chk.prop("checked", false);
                    }

                    if (tb.length) {
                        tb.css("background-color", "");
                        tb.removeAttr("readOnly");
                        tb.removeAttr("disabled");

                    }

                    servicefee.removeAttr("disabled");
                }
                else {

                    if (chk.length) {
                        chk.parent().attr("disabled", "disabled");
                        chk.attr("disabled", "disabled");
                        chk.prop("checked", false);
                    }

                    if (tb.length) {
                        tb.css("background-color", "WhiteSmoke");
                        tb.val('');
                        tb.attr("readOnly", "readOnly");
                        tb.attr("disabled", "disabled");
                    }

                    servicefee.attr("disabled", "disabled");
                }
            }
        }
    </script>


    <script type="text/javascript">
        Sys.Application.add_load(selectScheme);
    </script>


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, SPDataEntryBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, SPDataEntryBanner %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panStructureAddress" runat="server" Style="display: none;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblDialogTitle" runat="server" Text="<%$ Resources:Text, StructureAddressSearchTitle %>"></asp:Label></td>
                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <asp:GridView ID="gvStructureAddress" runat="server" AllowPaging="True" AllowSorting="true"
                                Width="100%" BackColor="White" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStructureAddressIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="15px" ForeColor="Black" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="address_eng" HeaderText="<%$ Resources:Text, Address %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddressEng" runat="server" Text='<%# Eval("address_eng") %>'></asp:Label>
                                            <asp:HiddenField ID="hfRecordID" runat="server" Value='<%# Eval("record_id") %>' />
                                            <asp:HiddenField ID="hfDistrictCode" runat="server" Value='<%# Eval("district_code") %>' />
                                            <asp:HiddenField ID="hfAreaCode" runat="server" Value='<%# Eval("area_code") %>' />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" ForeColor="Black" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddressChi" runat="server" Text='<%# Eval("address_chi") %>' CssClass="TextGridChi"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" ForeColor="Black" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, AddressType %>" SortExpression="record_id">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddressType" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="100px" ForeColor="Black" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="record_id" Visible="false" />
                                    <asp:BoundField DataField="address_eng" HeaderText="Text" SortExpression="address_eng"
                                        Visible="false" />
                                    <asp:BoundField DataField="district_code" Visible="false" />
                                    <asp:BoundField DataField="area_code" Visible="false" />
                                </Columns>
                            </asp:GridView>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnDialogSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" OnClick="ibtnDialogSelect_Click" />
                                        <asp:ImageButton ID="ibtnDialogClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnDialogClose_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none;">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnDialogConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDialogCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panExistingSPProfile" runat="server" Style="display: none;">
                <asp:Panel ID="panExistingSPProfileHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblExistingSPProfileTitle" runat="server" Text="<%$ Resources:Text, ShowSPProfileTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panExistingSPProfileContent" ScrollBars="Vertical" Height="500px"
                                runat="server" Width="786px">
                                <uc1:spSummaryView ID="udcExistingSPProfile" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnExistingSPProfileClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnExistingSPProfileClose_Click" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panDeletePractice" runat="server" Style="display: none;">
                <asp:Panel ID="panDeletePracticeHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDeletePracticeText" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgDeletePractice" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblDeletePractice" runat="server" Font-Bold="True"></asp:Label>
                                        <asp:HiddenField ID="hfDeletePractice" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnDeletePracticeConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnDeletePracticeConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDeletePracticeCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDeletePracticeCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panDeleteMO" runat="server" Style="display: none;">
                <asp:Panel ID="panDeleteMOHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDeleteMOText" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgDeleteMO" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblDeleteMO" runat="server" Font-Bold="True"></asp:Label>
                                        <asp:HiddenField ID="hfDeleteMO" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnDeleteMOConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnDeleteMOConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDeleteMOCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDeleteMOCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panDuplicated" runat="server" Style="display: none;">
                <asp:Panel ID="panDuplicatedHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDuplicatedTitle" runat="server" Text="<%$ Resources:Text, Duplicationlist %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panDuplicatedContent" ScrollBars="Vertical" Height="250px" runat="server"
                                Width="97%">
                                <uc2:MOPracticeLists ID="MOPracticeLists1" runat="server"></uc2:MOPracticeLists>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnDuplicatedClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnDuplicatedClose_Click" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupPCDWarning" runat="server" Width="600px">
                <uc4:ucPCDWarningPopup ID="ucPCDWarningPopup" runat="server" />
            </asp:Panel>
            <asp:Panel ID="panNoticePopup" runat="server" Width="550px" Height="100px" Style="display: none">
                <uc3:ucNoticePopUp ID="ucNoticePopup" runat="server" ButtonMode="OK" NoticeMode="Notification" />                
            </asp:Panel>
            <cc2:MessageBox ID="msgBox" runat="server" Width="95%"></cc2:MessageBox>
            <cc2:InfoMessageBox ID="CompleteMsgBox" runat="server" Width="95%" />
            <asp:MultiView ID="MultiViewDataEntry" runat="server" ActiveViewIndex="0">
                <asp:View ID="ViewTabPage" runat="server">
                    <asp:Panel ID="panIDInfo" runat="server" Width="100%">
                        <table width="100%">
                            <tr>
                                <td style="width: 200px" align="left">
                                    <asp:Label ID="lblIDText" runat="server"></asp:Label></td>
                                <td style="width: 200px" align="left">
                                    <asp:Label ID="lblID" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td align="left" style="width: 200px">
                                    <asp:Label ID="lblEnrolDtmText" runat="server" Text="<%$ Resources:Text, EnrolmentTime %>"></asp:Label>
                                </td>
                                <td align="left" style="width: 200px">
                                    <asp:Label ID="lblEnrolDtm" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="ibtnExistingSPProfile" runat="server" OnClick="ibtnExistingSPProfile_Click"
                                        Visible="False" AlternateText="<%$ Resources:AlternateText, OriginalRecordBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, OriginalRecordBtn %>" /></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 200px">
                                    <asp:Label ID="lblERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label>
                                </td>
                                <td align="left" style="width: 200px">
                                    <asp:Label ID="lblERN" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                                <td align="left" style="width: 200px">
                                    <asp:Label ID="lblProcessingDtmText" runat="server" Text="<%$ Resources:Text, DataEntryProcessingTime %>"></asp:Label>
                                </td>
                                <td align="left" style="width: 200px">
                                    <asp:Label ID="lblProcessingDtm" runat="server" CssClass="tableText"></asp:Label></td>
                                <td align="right"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <cc1:TabContainer ID="TabContainer1" runat="server" CssClass="m_ajax__tab_xp" ActiveTabIndex="0">
                        <cc1:TabPanel ID="tabPersonalParticulars" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblPersonalParticulars" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
                                <asp:Image ID="imgPersonalParticulars" runat="server" ImageUrl="~/Images/others/small_tick.png" CssClass="tabIcon"
                                    AlternateText="<%$ Resources:AlternateText, PageCheckedImg %>" ToolTip="<%$ Resources:ToolTip, PageCheckedImg %>" />
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:FormView ID="fvPersonalParticulars" runat="server" Width="100%">
                                    <InsertItemTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="1" style="width: 50px" valign="top">
                                                    <asp:Label ID="lblRegNameText" runat="server" Text="<%$ Resources:Text, Name %>"
                                                        CssClass="tableTitle"></asp:Label></td>
                                                <td style="width: 150px" valign="top" colspan="2">(<asp:Label ID="lblRegEngNameText" runat="server" Text="<%$ Resources:Text, InEnglish %>"
                                                    CssClass="tableTitle"></asp:Label>)
                                                    <asp:Label runat="server" ID="lblEnglishNameInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ID="txtRegSurname" runat="server" Width="100px" onblur="Upper(event,this)"
                                                        MaxLength="40"></asp:TextBox>,
                                                    <asp:TextBox ID="txtRegEname" runat="server" onblur="Upper(event,this);ltrim(this);"
                                                        MaxLength="40"></asp:TextBox>
                                                    <asp:Image ID="imgEnameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="1" style="width: 50px" valign="top"></td>
                                                <td style="width: 150px;" valign="top" colspan="2">(<asp:Label ID="lblRegChineseNameText" runat="server" Text="<%$ Resources:Text, InChinese %>"
                                                    CssClass="tableTitle"></asp:Label>)
                                                    <asp:Label runat="server" ID="lblChineseNameInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ID="txtRegCname" runat="server" MaxLength="6" CssClass="TextBoxChi"></asp:TextBox>
                                                    <asp:Label ID="lblRegCnameOptional" runat="server" ForeColor="#FF8080" Text="<%$ Resources:Text, optionalField %>"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <asp:Label ID="lblRegHKICText" runat="server" Text="<%$ Resources:Text, HKID %>"
                                                        CssClass="tableTitle"></asp:Label></td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ID="txtRegHKID" runat="server" Width="80px" onblur="formatHKID(this)"
                                                        MaxLength="11"></asp:TextBox>
                                                    <asp:Image ID="imgHKIdAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                    <asp:Label ID="lblRegHKIDTip" runat="server" Text="<%$ Resources:Text, HKIDHint %>"></asp:Label><br>
                                                    &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                                    &nbsp;&nbsp; &nbsp;<asp:Label ID="lblRegHKIDEG" runat="server" Text="<%$ Resources:Text, HKIDExample %>"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblRegAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"
                                                                    CssClass="tableTitle"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>(<asp:Label ID="lblRegEAddressText" runat="server" Text="<%$ Resources:Text, InEnglish %>"
                                                                CssClass="tableTitle"></asp:Label>)
                                                                <asp:Label runat="server" ID="lblSpAddressInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top" align="left">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td colspan="6">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblRegRoomText" runat="server" Text="<%$ Resources:Text, Room %>"
                                                                                CssClass="tableTitle"></asp:Label>
                                                                            &nbsp; &nbsp;
                                                                            <asp:TextBox ID="txtRegRoom" runat="server" Width="50px" MaxLength="5"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:Label ID="lblRegFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"
                                                                                CssClass="tableTitle"></asp:Label>
                                                                            &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtRegFloor" runat="server" Width="50px" MaxLength="3"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:Label ID="lblRegBlockText" runat="server" Text="<%$ Resources:Text, Block %>"
                                                                                CssClass="tableTitle"></asp:Label>
                                                                            &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtRegBlock" runat="server" Width="50px" MaxLength="3"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6">
                                                                <asp:TextBox ID="txtRegEAddress" runat="server" Width="600px" ToolTip="Structure Address Search"
                                                                    MaxLength="100"></asp:TextBox>
                                                                <asp:Image ID="imgEAddressAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /><asp:HiddenField
                                                                        ID="hfRegAddressCode" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 80px">
                                                                <asp:Label ID="lblRegDistrictText" runat="server" Text="<%$ Resources:Text, District %>"
                                                                    CssClass="tableTitle"></asp:Label></td>
                                                            <td colspan="5">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlRegDistrict" runat="server" Width="255px" AppendDataBoundItems="true"
                                                                                Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                                                                <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Image ID="imgDistrictAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 80px">
                                                                <asp:Label ID="lblRegAreaText" runat="server" Text="<%$ Resources:Text, Area %>"
                                                                    CssClass="tableTitle"></asp:Label></td>
                                                            <td colspan="5">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rboRegArea" runat="server" RepeatDirection="Horizontal"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="rboArea_SelectedIndexChanged">
                                                                            </asp:RadioButtonList></td>
                                                                        <td>
                                                                            <asp:Image ID="imgAreaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="6">
                                                                <asp:ImageButton ID="ibtnSearchSpAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddressSearchSBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, AddressSearchSBtn %>" OnClick="ibtnSearchAddress_Click" />
                                                                <asp:ImageButton ID="ibtnClearSearchSpAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClearAddressSearchSBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ClearAddressSearchSBtn %>" OnClick="ibtnClearSearchAddress_Click" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <asp:Label ID="lblRegEmailText" runat="server" Text="<%$ Resources:Text, Email %>"
                                                        CssClass="tableTitle"></asp:Label>
                                                    <asp:Label runat="server" ID="lblEmailInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ID="txtRegEmail" runat="server" MaxLength="255" Width="200px"></asp:TextBox>
                                                    <asp:Image ID="imgEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <asp:Label ID="lblRegConfirmEmailText" runat="server" Text="<%$ Resources:Text, ConfirmEmail %>"></asp:Label></td>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtRegConfirmEmail" runat="server" MaxLength="255" Width="200px"
                                                        onkeydown="disableCopyPaste()"></asp:TextBox>
                                                    <asp:Image ID="imgConfirmEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <asp:Label ID="lblRegContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"
                                                        CssClass="tableTitle"></asp:Label>
                                                    <asp:Label runat="server" ID="lblPhoneInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ID="txtRegContactNo" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                                                    <asp:Image ID="imgContactNoAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <asp:Label ID="lblRegFaxNoText" runat="server" Text="<%$ Resources:Text, FaxNo %>"
                                                        CssClass="tableTitle"></asp:Label>
                                                    <asp:Label runat="server" ID="lblFaxInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td valign="top" align="left">
                                                    <asp:TextBox ID="txtRegFaxNo" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                                                    <asp:Label ID="lblRegFaxNoOptional" runat="server" ForeColor="#FF8080" Text="<%$ Resources:Text, optionalField %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="ibtnSPSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSPSave_Click" /></td>
                                            </tr>
                                        </table>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegSurname" runat="server" TargetControlID="txtRegSurname"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegEname" runat="server" TargetControlID="txtRegEname"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredHKID" runat="server" TargetControlID="txtRegHKID"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="()">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredRegRoom" runat="server" TargetControlID="txtRegRoom"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegFloor" runat="server" TargetControlID="txtRegFloor"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegBlock" runat="server" TargetControlID="txtRegBlock"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredSPBuilding" runat="server" TargetControlID="txtRegEAddress"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="'.-(),&/ ">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredRegContactNo" runat="server" TargetControlID="txtRegContactNo"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegFaxNo" runat="server" TargetControlID="txtRegFaxNo"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                    </InsertItemTemplate>
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="width: 50px" valign="top">
                                                    <asp:Label ID="lblSPNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label>
                                                </td>
                                                <td style="width: 150px" valign="top">(<asp:Label ID="lblSPENameText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>
                                                    )
                                                    <asp:Label runat="server" ID="lblEnglishNameInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblSPEName" runat="server" CssClass="tableText" Text='<%# Eval("EnglishName") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50px" valign="top"></td>
                                                <td style="width: 150px" valign="top">(<asp:Label ID="lblSPCNameText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>
                                                    )
                                                    <asp:Label runat="server" ID="lblChineseNameInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblSPCName" runat="server" Text='<%# Eval("ChineseName") %>' CssClass="TextChi"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPHKID" runat="server" CssClass="tableText" Text='<%# Eval("HKID") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblSpAddressInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                                                        <tr>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 25%">
                                                                            <asp:Label ID="lblSPRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                            <asp:Label ID="lblSPRoom" runat="server" CssClass="tableText" Text='<%# Eval("SpAddress.Room") %>'></asp:Label>
                                                                        </td>
                                                                        <td style="width: 25%">
                                                                            <asp:Label ID="lblSPFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                            <asp:Label ID="lblSPFloor" runat="server" CssClass="tableText" Text='<%# Eval("SpAddress.Floor") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblSPBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                            <asp:Label ID="lblSPBlock" runat="server" CssClass="tableText" Text='<%# Eval("SpAddress.Block") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label ID="lblSPBuilding" runat="server" CssClass="tableText" Text='<%# Eval("SpAddress.Building") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <table border="0" cellpadding="0" cellspacing="1">
                                                                    <tr>
                                                                        <td style="width: 60px">
                                                                            <asp:Label ID="lblSPDistrict" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblSPDistrictText" runat="server" CssClass="tableText" Text='<%# Eval("SpAddress.DistrictDesc") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 60px">
                                                                            <asp:Label ID="lblSPAreaText" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblSPArea" runat="server" CssClass="tableText" Text='<%# Eval("SpAddress.AreaDesc") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblEmailInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPEmail" runat="server" CssClass="tableText" Text='<%# Eval("Email") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblPhoneInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPContactNo" runat="server" CssClass="tableText" Text='<%# Eval("Phone") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPFaxNoText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblFaxInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPFaxNo" runat="server" CssClass="tableText" Text='<%# Eval("Fax") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPSubmitByText" runat="server" Text="<%$ Resources:Text, SubmittedVia %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPSubmitBy" runat="server" CssClass="tableText" Text='<%# Eval("SubmitMethod") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPStatusText" runat="server" Text="<%$ Resources:Text, SPStatus %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPStatus" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="ibtnSPEdit" runat="server" AlternateText="<%$ Resources:AlternateText, EditBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, EditBtn %>" OnClick="ibtnSPEdit_Click" />
                                                    <asp:ImageButton ID="ibtnSPPageChecked" runat="server" AlternateText="<%$ Resources:AlternateText, PageCheckedBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, PageCheckedBtn %>" OnClick="ibtnPageChecked_Click" /></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="width: 50px" valign="top">
                                                    <asp:Label ID="lblSPEditNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                                                <td style="width: 200px" valign="top">(<asp:Label ID="lblSPEditENameText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)
                                                    <asp:Label runat="server" ID="lblEnglishNameInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txtSPEditSurname" runat="server" onblur="Upper(event,this)" Width="80px"
                                                        MaxLength="40"></asp:TextBox>
                                                    <asp:Label ID="lblSPEditComma" runat="server" Text=","></asp:Label>
                                                    <asp:TextBox ID="txtSPEditOthername" runat="server" MaxLength="40" onblur="Upper(event,this);ltrim(this);"></asp:TextBox>
                                                    <asp:Image ID="imgSPEditEnameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                    <asp:HiddenField ID="hfSPEditEName" Value='<%# Eval("EnglishName") %>' runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50px" valign="top"></td>
                                                <td style="width: 200px" valign="top">(<asp:Label ID="lblSPEditCNameText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)
                                                    <asp:Label runat="server" ID="lblChineseNameInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txtSPEditCname" runat="server" Text='<%# Eval("ChineseName") %>' CssClass="TextBoxChi"
                                                        MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="lblSPEditCnameOptional" runat="server" ForeColor="#FF8080" Text="<%$ Resources:Text, optionalField %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEditHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"
                                                        CssClass="tableTitle"></asp:Label></td>
                                                <td valign="top" align="left">
                                                    <asp:HiddenField ID="hfSPEditHKID" runat="server" Value='<%# Eval("HKID") %>' />
                                                    <asp:Label ID="lblSPEditHKID" runat="server" Text='<%# Eval("HKID") %>' CssClass="tableText"></asp:Label>
                                                    <asp:TextBox ID="txtSPEditHKID" runat="server" Width="80px" onblur="formatHKID(this)"
                                                        MaxLength="11" Text='<%# Eval("HKID") %>'></asp:TextBox>
                                                    <asp:Image ID="imgSPEditHKIDAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                    <asp:Label ID="lblSPEditHKIDTip" runat="server" Text="<%$ Resources:Text, HKIDHint %>"></asp:Label><br>
                                                    &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
                                                    &nbsp;&nbsp; &nbsp;<asp:Label ID="lblSPEditHKIDEG" runat="server" Text="<%$ Resources:Text, HKIDExample %>"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEditAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblSpAddressInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                                                        <tr>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 25%;">
                                                                            <asp:Label ID="lblSPEditRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>&nbsp;
                                                                            <asp:TextBox ID="txtSPEditRoom" runat="server" Width="50px" Text='<%# Eval("SpAddress.Room") %>'
                                                                                MaxLength="5"></asp:TextBox></td>
                                                                        <td style="width: 25%;">
                                                                            <asp:Label ID="lblSPEditFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>&nbsp;
                                                                            <asp:TextBox ID="txtSPEditFloor" runat="server" Width="50px" Text='<%# Eval("SpAddress.Floor") %>'
                                                                                MaxLength="3"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblSPEditBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>&nbsp;
                                                                            <asp:TextBox ID="txtSPEditBlock" runat="server" Width="50px" Text='<%# Eval("SpAddress.Block") %>'
                                                                                MaxLength="3"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:TextBox ID="txtSPEditBuilding" runat="server" Width="600px" Text='<%# Eval("SpAddress.Building") %>'
                                                                    MaxLength="100"></asp:TextBox>
                                                                <asp:Image ID="imgSPEditBuildingAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /><asp:HiddenField
                                                                        ID="hfSPEditAddressCode" runat="server" Value='<%# Eval("SpAddress.Address_Code") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                                                    <tr>
                                                                        <td style="width: 60px">
                                                                            <asp:Label ID="lblSPEditDistrictText" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlSPEditDistrict" runat="server" AppendDataBoundItems="True"
                                                                                Width="255px" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true">
                                                                                <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hfSPEditDistrict" runat="server" Value='<%# Eval("SpAddress.District") %>' />
                                                                            <asp:Image ID="imgSPEditDistrictAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 60px">
                                                                            <asp:Label ID="lblSPEditAreaText" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:RadioButtonList ID="rboSPEditArea" runat="server" RepeatDirection="Horizontal"
                                                                                            AutoPostBack="true" OnSelectedIndexChanged="rboArea_SelectedIndexChanged">
                                                                                        </asp:RadioButtonList><asp:HiddenField ID="hfSPEditArea" runat="server" Value='<%# Eval("SpAddress.AreaCode") %>' />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Image ID="imgSPEditAreaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:ImageButton ID="ibtnSearchSpEditAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddressSearchSBtn %>"
                                                                                AlternateText="<%$ Resources:AlternateText, AddressSearchSBtn %>" OnClick="ibtnSearchAddress_Click" />&nbsp;
                                                                            <asp:ImageButton ID="ibtnClearSearchSpEditAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClearAddressSearchSBtn %>"
                                                                                AlternateText="<%$ Resources:AlternateText, ClearAddressSearchSBtn %>" OnClick="ibtnClearSearchAddress_Click" /></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEditEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblEmailInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSPEditEmail" runat="server" MaxLength="255" Width="200px" Text='<%# Eval("Email") %>'></asp:TextBox>
                                                    <asp:Image ID="imgSPEditEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEditConfirmEmailText" runat="server" Text="<%$ Resources:Text, ConfirmEmail %>"></asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txtSPEditConfirmEmail" runat="server" MaxLength="255" Width="200px"
                                                        Text='<%# Eval("Email") %>' onkeydown="disableCopyPaste()"></asp:TextBox>
                                                    <asp:Image ID="imgSPEditConfirmEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEditContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblPhoneInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSPEditContactNo" runat="server" Width="80px" Text='<%# Eval("Phone") %>'
                                                        MaxLength="20"></asp:TextBox>
                                                    <asp:Image ID="imgSPEditContactNoAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEditFax" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblFaxInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSPEditFaxNo" runat="server" Width="80px" Text='<%# Eval("Fax") %>'
                                                        MaxLength="20"></asp:TextBox>
                                                    <asp:Label ID="lblSPEditFaxOptional" runat="server" ForeColor="#FF8080" Text="<%$ Resources:Text, optionalField %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" valign="top">
                                                    <asp:Label ID="lblSPEditSubmitByText" runat="server" Text="<%$ Resources:Text, SubmittedVia %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPEditSubmitBy" runat="server" CssClass="tableText" Text='<%# Eval("SubmitMethod") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="ibtnSPEditSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSPEditSave_Click" />
                                                    <asp:ImageButton ID="ibtnSPEditCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnSPEditCancel_Click" /></td>
                                            </tr>
                                        </table>

                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditSurname" runat="server" TargetControlID="txtSPEditSurname"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditOtherName" runat="server" TargetControlID="txtSPEditOtherName"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditRoom" runat="server" TargetControlID="txtSPEditRoom"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditFloor" runat="server" TargetControlID="txtSPEditFloor"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditBlock" runat="server" TargetControlID="txtSPEditBlock"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditBuilding" runat="server" TargetControlID="txtSPEditBuilding"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="'.-(),&/ ">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditContactNo" runat="server" TargetControlID="txtSPEditContactNo"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredSPEditFaxNo" runat="server" TargetControlID="txtSPEditFaxNo"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                    </EditItemTemplate>
                                </asp:FormView>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tablMOInfo" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblMOInfo" runat="server" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"></asp:Label>
                                <asp:Image ID="imgMOInfo" runat="server" ImageUrl="~/Images/others/small_tick.png" CssClass="tabIcon"
                                    AlternateText="<%$ Resources:AlternateText, PageCheckedImg %>" ToolTip="<%$ Resources:ToolTip, PageCheckedImg %>" />
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView ID="gvMOInfo" runat="server" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle VerticalAlign="Top" Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblMODispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEditMODispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 110px; background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                        <td style="width: 100px; background-color: #f7f7de;" valign="top">(<asp:Label ID="lblMONameInEngText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblMOName" runat="server" Text='<%# Eval("MOEngName") %>' CssClass="tableText"></asp:Label>
                                                            <asp:ImageButton ID="ibtnDuplicateMO" runat="server" ImageUrl="~/Images/others/info.png"
                                                                AlternateText="<%$ Resources:Text, DuplicateMO %>" Visible='<%# Eval("IsDuplicated") %>'
                                                                OnClick="ibtnDuplicateMO_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 110px; background-color: #f7f7de" valign="top"></td>
                                                        <td style="width: 100px; background-color: #f7f7de" valign="top">(<asp:Label ID="lblMONameInChiText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblMOChiName" runat="server" Text='<%# Eval("MOChiName") %>' CssClass="TextChi"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblRegMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblRegMOBRCode" runat="server" CssClass="tableText" Text='<%# Eval("BrCode") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblRegMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                                                            <asp:Label ID="lblRegMOEmailInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblRegMOEmail" runat="server" CssClass="tableText" Text='<%# Eval("Email") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblRegMOPhoneText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label>
                                                            <asp:Label ID="lblRegMOPhoneInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblRegMOPhone" runat="server" CssClass="tableText" Text='<%# Eval("PhoneDaytime") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblRegMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                                                            <asp:Label ID="lblRegMOFaxInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblRegMOFax" runat="server" CssClass="tableText" Text='<%# Eval("Fax") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label>
                                                            <asp:Label ID="lblMOAddressInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table style="width: 80%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMORoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblMORoom" runat="server" Width="50px" Text='<%# Eval("MOAddress.Room") %>'
                                                                                        CssClass="tableText"></asp:Label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMOFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblMOFloor" runat="server" Width="50px" Text='<%# Eval("MOAddress.Floor") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMOBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblMOBlock" runat="server" Width="50px" Text='<%# Eval("MOAddress.Block") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Label ID="lblMOBuilding" runat="server" Text='<%# Eval("MOAddress.Building") %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <table border="0" cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblMODistrictText" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMODistrict" runat="server" Text='<%# Eval("MOAddress.DistrictDesc") %>'
                                                                                        CssClass="tableText"></asp:Label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblMOAreaText" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMOArea" runat="server" Text='<%# Eval("MOAddress.AreaDesc") %>'
                                                                                        CssClass="tableText"></asp:Label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblRegMORelationshipText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRegMORelationship" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:Label ID="lblRegMORelationshipRemark" runat="server" Text='<%# Eval("RelationshipRemark") %>'
                                                                CssClass="TextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblMOStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label>
                                                            <asp:Label ID="lblMOStatusInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMOStatus" runat="server" Text='<%# Eval("RecordStatus") %>' CssClass="tableText"></asp:Label>
                                                            <asp:HiddenField ID="hfMOStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100px; background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblEditMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                        <td style="width: 85px; background-color: #f7f7de;" valign="top">(<asp:Label ID="lblEditMONameInEngText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblEditMOName" runat="server" Text='<%# Eval("MOEngName") %>' CssClass="tableText"></asp:Label>
                                                            <asp:TextBox ID="txtEditMOName" runat="server" Text='<%# Eval("MOEngName") %>' Width="310px"
                                                                MaxLength="100"></asp:TextBox>
                                                            <asp:ImageButton ID="ibtnEditDuplicateMO" runat="server" ImageUrl="~/Images/others/info.png"
                                                                AlternateText="<%$ Resources:Text, DuplicateMO %>" Visible='<%# Eval("IsDuplicated") %>'
                                                                OnClick="ibtnDuplicateMO_Click" />
                                                            <asp:Image ID="imgEditMONameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px; background-color: #f7f7de" valign="top"></td>
                                                        <td style="width: 85px; background-color: #f7f7de" valign="top">(<asp:Label ID="lblEditMONameInChiText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblEditMONameChi" runat="server" Text='<%# Eval("MOChiName") %>' CssClass="TextChi"></asp:Label>
                                                            <asp:TextBox ID="txtEditMONameChi" runat="server" Text='<%# Eval("MOChiName") %>' CssClass="TextBoxChi"
                                                                Width="310px" MaxLength="100"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lbEditMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblEditMOBRCode" runat="server" Text='<%# Eval("BrCode") %>' CssClass="tableText"></asp:Label>
                                                            <asp:TextBox ID="txtEditMOBRCode" runat="server" Text='<%# Eval("BrCode") %>'></asp:TextBox><asp:Image
                                                                ID="imgEditMOBRCodeAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblEditMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                                                            <asp:Label ID="lblRegMOEmailInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditMOEmail" runat="server" Text='<%# Eval("Email") %>'></asp:TextBox>
                                                            <asp:Image ID="imgEditMOEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblEditMOPhoneText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label>
                                                            <asp:Label ID="lblRegMOPhoneInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditMOPhone" runat="server" Text='<%# Eval("PhoneDaytime") %>'></asp:TextBox><asp:Image
                                                                ID="imgEditMOPhoneAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblEditMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                                                            <asp:Label ID="lblRegMOFaxInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditMOFax" runat="server" Text='<%# Eval("Fax") %>'></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblEditMOAddressText" runat="server" Text="<%$ Resources: Text, MOAddress %>"></asp:Label>
                                                            <asp:Label ID="lblMOAddressInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table style="width: 95%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblEditMORoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtEditMORoom" runat="server" Text='<%# Eval("MOAddress.Room") %>'
                                                                                        MaxLength="5" Width="50px"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblEditMOFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtEditMOFloor" runat="server" Text='<%# Eval("MOAddress.Floor") %>'
                                                                                        MaxLength="3" Width="50px"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblEditMOBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtEditMOBlock" runat="server" Text='<%# Eval("MOAddress.Block") %>'
                                                                                        MaxLength="3" Width="50px"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtEditMOBuilding" runat="server" Width="500px" Text='<%# Eval("MOAddress.Building") %>'
                                                                            MaxLength="100"></asp:TextBox>
                                                                        <asp:Image ID="imgEditMOBuildingAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /><asp:HiddenField
                                                                                ID="hfEditMOAddressCode" runat="server" Value='<%# Eval("MOAddress.Address_Code") %>' />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblEditMODistrict" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlEditMODistrict" runat="server" Width="255px" AppendDataBoundItems="true"
                                                                                        OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                                    </asp:DropDownList></td>
                                                                                <td>
                                                                                    <asp:Image ID="imgEditMODistrcitAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hfEditMODistrict" runat="server" Value='<%# Eval("MOAddress.District") %>' />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblEditMOArea" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:RadioButtonList ID="rbEditMOArea" runat="server" RepeatDirection="Horizontal"
                                                                                        OnSelectedIndexChanged="rboArea_SelectedIndexChanged" AutoPostBack="true">
                                                                                    </asp:RadioButtonList></td>
                                                                                <td>&nbsp;<asp:Image ID="imgEditMOAreaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hfEditMOArea" runat="server" Value='<%# Eval("MOAddress.AreaCode") %>' />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:ImageButton ID="ibtnEditMOSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, AddressSearchSBtn %>" OnClick="ibtnSearchAddress_Click" />
                                                                        <asp:ImageButton ID="ibtnEditClearMOSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClearAddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, ClearAddressSearchSBtn %>" OnClick="ibtnClearSearchAddress_Click" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblEditMORelationshipText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblEditMORelationship" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:Label ID="lblEditMORelationshipRemark" runat="server" Text='<%# Eval("RelationshipRemark") %>'
                                                                CssClass="TextChi"></asp:Label>
                                                            <asp:Panel ID="pnlEditMORelationship" runat="server">
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <asp:RadioButtonList ID="rboEditMORelation" runat="server" RepeatDirection="Horizontal"
                                                                                RepeatColumns="6" RepeatLayout="Flow">
                                                                            </asp:RadioButtonList><br />
                                                                            <asp:Label ID="lblEditPleaseSpecify" runat="server" Text="<% $ Resources:Text, PleaseSpecify %>"></asp:Label>
                                                                            <asp:TextBox ID="txtEditMORelationRemark" runat="server" Width="300px" MaxLength="255" CssClass="TextBoxChi"
                                                                                Text='<%# Eval("RelationshipRemark") %>'></asp:TextBox><asp:Image ID="imgEditMORelationRemarksAlert"
                                                                                    runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                                            <asp:HiddenField ID="hfEditMORelation" runat="server" Value='<%# Eval("Relationship") %>' />
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:Image ID="imgEditMORelationAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                                            <asp:HiddenField ID="hfEditMOStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditMOBRCode" runat="server" TargetControlID="txtEditMOBRCode"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditMOPhone" runat="server" TargetControlID="txtEditMOPhone"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditMOFax" runat="server" TargetControlID="txtEditMOFax"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditMORoom" runat="server" TargetControlID="txtEditMORoom"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditMOFloor" runat="server" TargetControlID="txtEditMOFloor"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditMOBlock" runat="server" TargetControlID="txtEditMOBlock"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>

                                                <cc1:FilteredTextBoxExtender ID="FilteredMOEditBuilding" runat="server" TargetControlID="txtEditMOBuilding"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="'.-(),&/ ">
                                                </cc1:FilteredTextBoxExtender>

                                            </EditItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                            <FooterTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 100px; background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblAddMOName" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                        <td style="width: 85px; background-color: #f7f7de;" valign="top">(<asp:Label ID="lblAddMONameInEngText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddMOName" runat="server" Width="310px" MaxLength="100"></asp:TextBox>
                                                            <asp:Image ID="imgAddMONameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px; background-color: #f7f7de" valign="top"></td>
                                                        <td style="width: 85px; background-color: #f7f7de" valign="top">(<asp:Label ID="lblAddMONameInChiText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddMONameChi" runat="server" Width="310px" MaxLength="100" CssClass="TextBoxChi"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lbAddMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddMOBRCode" runat="server"></asp:TextBox><asp:Image ID="imgAddMOBRCodeAlert"
                                                                runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblAddMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddMOEmail" runat="server"></asp:TextBox>
                                                            <asp:Image ID="imgAddMOEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblAddMOPhoneText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddMOPhone" runat="server"></asp:TextBox><asp:Image ID="imgAddMOPhoneAlert"
                                                                runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblAddMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddMOFax" runat="server"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblAddMOAddressText" runat="server" Text="<%$ Resources: Text, MOAddress %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table style="width: 95%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblAddMORoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAddMORoom" runat="server" MaxLength="5" Width="50px"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblAddMOFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAddMOFloor" runat="server" MaxLength="3" Width="50px"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblAddMOBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAddMOBlock" runat="server" MaxLength="3" Width="50px"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtAddMOBuilding" runat="server" Width="500px" MaxLength="100"></asp:TextBox>
                                                                        <asp:Image ID="imgAddMOBuildingAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                                        <asp:HiddenField ID="hfAddMOAddressCode" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblAddMODistrict" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlAddMODistrict" runat="server" Width="255px" AppendDataBoundItems="true"
                                                                                        OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                                    </asp:DropDownList></td>
                                                                                <td>&nbsp;<asp:Image ID="imgAddMODistrcitAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblAddMOArea" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:RadioButtonList ID="rbAddMOArea" runat="server" RepeatDirection="Horizontal"
                                                                                        OnSelectedIndexChanged="rboArea_SelectedIndexChanged" AutoPostBack="true">
                                                                                    </asp:RadioButtonList></td>
                                                                                <td>&nbsp;<asp:Image ID="imgAddMOAreaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:ImageButton ID="ibtnAddMOSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, AddressSearchSBtn %>" OnClick="ibtnSearchAddress_Click" />
                                                                        <asp:ImageButton ID="ibtnAddClearMOSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClearAddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, ClearAddressSearchSBtn %>" OnClick="ibtnClearSearchAddress_Click" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblAddMORelationship" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:RadioButtonList ID="rboAddMORelation" runat="server" RepeatDirection="Horizontal"
                                                                            RepeatColumns="6" RepeatLayout="Flow">
                                                                        </asp:RadioButtonList><br />
                                                                        <asp:Label ID="lblPleaseSpecify" runat="server" Text="<% $ Resources:Text, PleaseSpecify %>"></asp:Label>
                                                                        <asp:TextBox ID="txtAddMORelationRemark" runat="server" Width="300px" MaxLength="255" CssClass="TextBoxChi"></asp:TextBox><asp:Image
                                                                            ID="imgAddMORelationRemarksAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:Image ID="imgAddMORelationAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddMOBRCode" runat="server" TargetControlID="txtAddMOBRCode"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddMOPhone" runat="server" TargetControlID="txtAddMOPhone"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddMOFax" runat="server" TargetControlID="txtAddMOFax"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddMORoom" runat="server" TargetControlID="txtAddMORoom"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddMOFloor" runat="server" TargetControlID="txtAddMOFloor"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddMOBlock" runat="server" TargetControlID="txtAddMOBlock"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>

                                                <cc1:FilteredTextBoxExtender ID="FilteredMOAddBuilding" runat="server" TargetControlID="txtAddMOBuilding"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="'.-(),&/ ">
                                                </cc1:FilteredTextBoxExtender>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnMOEdit" runat="server" CausesValidation="False" AlternateText="<%$ Resources:AlternateText, EditSBtn %>"
                                                    CommandName="Edit" ImageUrl="<%$ Resources:ImageUrl, EditSBtn %>" />
                                                <asp:ImageButton ID="ibtnMODelete" runat="server" AlternateText="<% $ Resources:AlternateText, DeleteSBtn%>"
                                                    ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>" OnClick="ibtnMODelete_Click" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="ibtnMOSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveSBtn %>"
                                                    CommandName="Update" ImageUrl="<%$ Resources:ImageUrl, SaveSBtn %>" />
                                                <asp:ImageButton ID="ibtnMOiceCancel" runat="server" CausesValidation="False" AlternateText="<%$ Resources:AlternateText, CancelSBtn %>"
                                                    CommandName="Cancel" ImageUrl="<%$ Resources:ImageUrl, CancelSBtn %>" />
                                            </EditItemTemplate>
                                            <ItemStyle Width="80px" VerticalAlign="Top" />
                                            <FooterTemplate>
                                                <asp:ImageButton ID="ibtnMOAddRecord" runat="server" CommandName="Add" ImageUrl="<%$ Resources:ImageUrl, SaveSBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, SaveSBtn %>" />
                                                <asp:ImageButton ID="ibtnMOCancelAdd" runat="server" CommandName="CancelAdd" AlternateText="<%$ Resources:AlternateText, CancelSBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelSBtn %>" />
                                            </FooterTemplate>
                                            <FooterStyle Width="80px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <asp:ImageButton ID="ibtnMOAdd" runat="server" OnClick="ibtnMOAdd_Click" AlternateText="<%$ Resources:AlternateText, AddSBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, AddSBtn %>" /></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnMOPageChecked" runat="server" AlternateText="<%$ Resources:AlternateText, PageCheckedBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, PageCheckedBtn %>" OnClick="ibtnPageChecked_Click" /></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabPracticeInfo" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblPracticeInfo" runat="server" Text="<%$ Resources:Text, PracticeInfo %>"></asp:Label>
                                <asp:Image ID="imgPracticeInfo" runat="server" ImageUrl="~/Images/others/small_tick.png" CssClass="tabIcon"
                                    AlternateText="<%$ Resources:AlternateText, PageCheckedImg %>" ToolTip="<%$ Resources:ToolTip, PageCheckedImg %>" />
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView ID="gvPracticeInfo" runat="server" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle VerticalAlign="Top" Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPracticeDispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                                <asp:HiddenField ID="hfPracticeBankDisplaySeq" runat="server" Value='<%#  Eval("BankAcct.DisplaySeq") %>' />
                                                <asp:HiddenField ID="hfPracticeStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEditPracticeDispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                                <asp:HiddenField ID="hfEditPracticeStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                                            <ItemTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblPracticeMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                                            <asp:Label ID="lblPracticeMONameTextInd" runat="server" Text="*" ForeColor="Red"
                                                                Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPracticeMOName" runat="server" Text='<%# Eval("MODisplaySeq") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label>
                                                            <asp:Label ID="lblPracticeNameTextInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td style="width: 85px; background-color: #f7f7de;" valign="top">(<asp:Label ID="lblPracticeNameInEngText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:ImageButton ID="ibtnDuplicatePractice" runat="server" ImageUrl="~/Images/others/info.png"
                                                                AlternateText="<%$ Resources:Text, DuplicatePractice %>" Visible='<%# Eval("IsDuplicated") %>'
                                                                OnClick="ibtnDuplicatePractice_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; background-color: #f7f7de" valign="top"></td>
                                                        <td style="width: 85px; background-color: #f7f7de" valign="top">(<asp:Label ID="lblPracticeNameInChiText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblPracticeChiName" runat="server" CssClass="TextChi" Text='<%# Eval("PracticeNameChi") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label>
                                                            <asp:Label ID="lblPracticeAddressInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table style="width: 80%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblPracticeRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                                    <asp:Label ID="lblPracticeRoom" runat="server" Width="50px" Text='<%# Eval("PracticeAddress.Room") %>'
                                                                                        CssClass="tableText"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPracticeFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                    <asp:Label ID="lblPracticeFloor" runat="server" Width="50px" Text='<%# Eval("PracticeAddress.Floor") %>'
                                                                                        CssClass="tableText"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPracticeBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                    <asp:Label ID="lblPracticeBlock" runat="server" Width="50px" Text='<%# Eval("PracticeAddress.Block") %>'
                                                                                        CssClass="tableText"></asp:Label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table style="width: 100%" border="0">
                                                                            <tr>
                                                                                <td style="width: 85px" valign="top">(<asp:Label ID="lblPracticeBuildingText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPracticeBuilding" runat="server" Text='<%# Eval("PracticeAddress.Building") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 85px" valign="top">(<asp:Label ID="lblPracticeChiBuildingText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPracticeChiBuilding" runat="server" Text='<%# Eval("PracticeAddress.ChiBuilding") %>'
                                                                                        CssClass="TextChi"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 85px" valign="top">
                                                                                    <asp:Label ID="lblPracticeDistrictText" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPracticeDistrict" runat="server" Text='<%# Eval("PracticeAddress.DistrictDesc") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 85px" valign="top">
                                                                                    <asp:Label ID="lblPracticeAreaText" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPracticeArea" runat="server" Text='<%# Eval("PracticeAddress.AreaDesc") %>'
                                                                                        CssClass="tableText"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblRegPhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label>
                                                            <asp:Label ID="lblRegPhoneInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblRegPhone" runat="server" CssClass="tableText" Text='<%# Eval("PhoneDaytime") %>'></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRegCode" runat="server" Text='<%# Eval("Professional.RegistrationCode") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblPracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                            <asp:Label runat="server" ID="lblPracticeStatusInd" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPracticeStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblEditPracticeMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                                            <asp:Label ID="lblPracticeMONameTextInd" runat="server" Text="*" ForeColor="Red"
                                                                Font-Size="12pt" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblEditPracticeMOName" runat="server" CssClass="tableText" Text='<%# Eval("MODisplaySeq") %>'></asp:Label>
                                                            <asp:DropDownList ID="ddlEditPracticeMOName" runat="server" AppendDataBoundItems="true">
                                                                <asp:ListItem Text="<%$ Resources:Text, SelectMO %>" Value=""></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hfEditPracticeMOName" runat="server" Value='<%# Eval("MODisplaySeq") %>'></asp:HiddenField>
                                                            <asp:Image ID="imgEditPracticeMONameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblEditPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label>
                                                            <asp:Label ID="lblPracticeNameTextInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td style="width: 85px; background-color: #f7f7de;" valign="top">(<asp:Label ID="lblEditPracticeNameInEngText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblEditPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:TextBox ID="txtEditPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                Width="310px" MaxLength="100"></asp:TextBox>
                                                            <asp:ImageButton ID="ibtnEditDuplicatePractice" runat="server" ImageUrl="~/Images/others/info.png"
                                                                AlternateText="<%$ Resources:Text, DuplicatePractice %>" Visible='<%# Eval("IsDuplicated") %>'
                                                                OnClick="ibtnDuplicatePractice_Click" />
                                                            <asp:Image ID="imgEditPracticeNameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; background-color: #f7f7de" valign="top"></td>
                                                        <td style="width: 85px; background-color: #f7f7de" valign="top">(<asp:Label ID="lblEditPracticeNameInChiText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:Label ID="lblEditPracticeNameChi" runat="server" Text='<%# Eval("PracticeNameChi") %>'
                                                                CssClass="TextChi"></asp:Label>
                                                            <asp:TextBox ID="txtEditPracticeNameChi" runat="server" Text='<%# Eval("PracticeNameChi") %>' CssClass="TextBoxChi"
                                                                Width="310px" MaxLength="100"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblEditPracticeAddressText" runat="server" Text="<%$ Resources: Text, PracticeAddress %>"></asp:Label>
                                                            <asp:Label ID="lblPracticeAddressInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table style="width: 95%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblEditPracticeRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtEditPracticeRoom" runat="server" Text='<%# Eval("PracticeAddress.Room") %>'
                                                                                        MaxLength="5" Width="50px"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblEditPracticeFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtEditPracticeFloor" runat="server" Text='<%# Eval("PracticeAddress.Floor") %>'
                                                                                        MaxLength="3" Width="50px"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblEditPracticeBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtEditPracticeBlock" runat="server" Text='<%# Eval("PracticeAddress.Block") %>'
                                                                                        MaxLength="3" Width="50px"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">(<asp:Label ID="lblEditPracticeBuildingText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEditPracticeBuilding" runat="server" Width="500px" Text='<%# Eval("PracticeAddress.Building") %>'
                                                                            MaxLength="100"></asp:TextBox>
                                                                        <asp:Image ID="imgEditPracticeBuildingAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /><asp:HiddenField
                                                                                ID="hfEditPracticeAddressCode" runat="server" Value='<%# Eval("PracticeAddress.Address_Code") %>' />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">(<asp:Label ID="lblEditPracticeChiBuildingText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEditPracticeBuildingChi" runat="server" Width="500px" Text='<%# Eval("PracticeAddress.ChiBuilding") %>' CssClass="TextBoxChi"
                                                                            MaxLength="100"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblEditPracticeDistrict" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlEditPracticeDistrict" runat="server" Width="255px" AppendDataBoundItems="true"
                                                                                        OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                                    </asp:DropDownList></td>
                                                                                <td>
                                                                                    <asp:Image ID="imgEditPRacticeDistrcitAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hfEditPracticeDistrict" runat="server" Value='<%# Eval("PracticeAddress.District") %>' />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblEditPracticeArea" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:RadioButtonList ID="rbEditPracticeArea" runat="server" RepeatDirection="Horizontal"
                                                                                        OnSelectedIndexChanged="rboArea_SelectedIndexChanged" AutoPostBack="true">
                                                                                    </asp:RadioButtonList></td>
                                                                                <td>&nbsp;<asp:Image ID="imgEditPracticeAreaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:HiddenField ID="hfEditPracticeArea" runat="server" Value='<%# Eval("PracticeAddress.AreaCode") %>' />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:ImageButton ID="ibtnEditPracticeSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, AddressSearchSBtn %>" OnClick="ibtnSearchAddress_Click" />
                                                                        <asp:ImageButton ID="ibtnEditClearPracticeSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClearAddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, ClearAddressSearchSBtn %>" OnClick="ibtnClearSearchAddress_Click" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblEditPhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label>
                                                            <asp:Label ID="lblRegPhoneInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEditPhone" runat="server" Text='<%# Eval("PhoneDaytime")  %>'
                                                                            Width="200px" MaxLength="20"></asp:TextBox></td>
                                                                    <td>&nbsp;<asp:Image ID="imgEditPhoneAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblEditHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblEditHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'
                                                                            CssClass="tableText"></asp:Label>
                                                                        <asp:DropDownList ID="ddlEditHealthProf" runat="server" AppendDataBoundItems="true"
                                                                            Width="315px" AutoPostBack="false" OnSelectedIndexChanged="ddlHealthProf_SelectedIndexChanged">
                                                                            <asp:ListItem Text="<%$ Resources:Text, SelectHealthProf %>" Value=""></asp:ListItem>
                                                                        </asp:DropDownList></td>
                                                                    <td>&nbsp;<asp:Image ID="imgEditHealthProfAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="hfEditHealthProf" runat="server" Value='<%# Eval("Professional.ServiceCategoryCode") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblEditRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblEditRegCode" runat="server" Text='<%# Eval("Professional.RegistrationCode")  %>'
                                                                            CssClass="tableText"></asp:Label>
                                                                        <asp:TextBox ID="txtEditRegCode" runat="server" Text='<%# Eval("Professional.RegistrationCode")  %>'
                                                                            Width="310px" MaxLength="15"></asp:TextBox></td>
                                                                    <td>&nbsp;<asp:Image ID="imgEditRegCodeAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditPracticeRoom" runat="server" TargetControlID="txtEditPracticeRoom"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditPracticeFloor" runat="server" TargetControlID="txtEditPracticeFloor"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditPracticeBlock" runat="server" TargetControlID="txtEditPracticeBlock"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>

                                                <cc1:FilteredTextBoxExtender ID="FilteredPracticeEditBuilding" runat="server" TargetControlID="txtEditPracticeBuilding"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="'.-(),&/ ">
                                                </cc1:FilteredTextBoxExtender>

                                                <cc1:FilteredTextBoxExtender ID="FilteredEditPracticePhone" runat="server" TargetControlID="txtEditPhone"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditRegCode" runat="server" TargetControlID="txtEditRegCode"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                            </EditItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                            <FooterTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td colspan="2" style="background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblAddPracticeMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAddPracticeMOName" runat="server" AppendDataBoundItems="true">
                                                                <asp:ListItem Text="<%$ Resources:Text, SelectMO %>" Value=""></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgAddPracticeMONameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; background-color: #f7f7de" valign="top">
                                                            <asp:Label ID="lblAddPracticeName" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                        <td style="width: 85px; background-color: #f7f7de;" valign="top">(<asp:Label ID="lblAddPracticeNameInEngText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                Width="310px" MaxLength="100"></asp:TextBox>
                                                            <asp:Image ID="imgAddPracticeNameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 125px; background-color: #f7f7de" valign="top"></td>
                                                        <td style="width: 85px; background-color: #f7f7de" valign="top">(<asp:Label ID="lblAddPracticeNameInChiText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddPracticeNameChi" runat="server" Text='<%# Eval("PracticeNameChi") %>' CssClass="TextBoxChi"
                                                                Width="310px" MaxLength="100"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblAddPracticeAddressText" runat="server" Text="<%$ Resources: Text, PracticeAddress %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table style="width: 95%" border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblAddPracticeRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAddPracticeRoom" runat="server" Width="50px" MaxLength="5"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblAddPracticeFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAddPracticeFloor" runat="server" Width="50px" MaxLength="3"></asp:TextBox></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblAddPracticeBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtAddPracticeBlock" runat="server" Width="50px" MaxLength="3"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">(<asp:Label ID="lblAddPracticeBuildingText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAddPracticeBuilding" runat="server" Width="500px" MaxLength="100"></asp:TextBox>
                                                                        <asp:Image ID="imgAddPracticeBuildingAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /><asp:HiddenField
                                                                                ID="hfAddPracticeAddressCode" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">(<asp:Label ID="lblAddPracticeChiBuildingText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAddPracticeBuildingChi" runat="server" Width="500px" MaxLength="100" CssClass="TextBoxChi"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblAddPracticeDistrict" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlAddPracticeDistrict" runat="server" Width="255px" AppendDataBoundItems="true"
                                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                                                                        <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                                    </asp:DropDownList></td>
                                                                                <td>&nbsp;<asp:Image ID="imgAddPRacticeDistrcitAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 85px">
                                                                        <asp:Label ID="lblAddPracticeArea" runat="server" Text="<%$ Resources:Text, Area %>"></asp:Label></td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:RadioButtonList ID="rbAddPracticeArea" runat="server" RepeatDirection="Horizontal"
                                                                                        OnSelectedIndexChanged="rboArea_SelectedIndexChanged" AutoPostBack="true">
                                                                                    </asp:RadioButtonList></td>
                                                                                <td>&nbsp;<asp:Image ID="imgAddPracticeAreaAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:ImageButton ID="ibtnAddPracticeSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, AddressSearchSBtn %>" OnClick="ibtnSearchAddress_Click" />
                                                                        <asp:ImageButton ID="ibtnAddClearPracticeSearchAddress" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClearAddressSearchSBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, ClearAddressSearchSBtn %>" OnClick="ibtnClearSearchAddress_Click" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblAddPhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddPracticePhone" runat="server" Width="200px" MaxLength="15"></asp:TextBox>
                                                            <asp:Image ID="imgAddPracticePhoneAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblAddHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAddHealthProf" runat="server" AppendDataBoundItems="true"
                                                                Width="315px" AutoPostBack="false" OnSelectedIndexChanged="ddlHealthProf_SelectedIndexChanged">
                                                                <asp:ListItem Text="<%$ Resources:Text, SelectHealthProf %>" Value=""></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Image ID="imgAddHealthProfAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de;" valign="top" colspan="2">
                                                            <asp:Label ID="lblAddRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAddRegCode" runat="server" Width="310px" MaxLength="15" onblur="Upper(event,this)"></asp:TextBox>
                                                            <asp:Image ID="imgAddRegCodeAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddPracticeRoom" runat="server" TargetControlID="txtAddPracticeRoom"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddPracticeFloor" runat="server" TargetControlID="txtAddPracticeFloor"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddPracticeBlock" runat="server" TargetControlID="txtAddPracticeBlock"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>

                                                <cc1:FilteredTextBoxExtender ID="FilteredPracticeAddBuilding" runat="server" TargetControlID="txtAddPracticeBuilding"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="'.-(),&/ ">
                                                </cc1:FilteredTextBoxExtender>

                                                <cc1:FilteredTextBoxExtender ID="FilteredAddPracticePhone" runat="server" TargetControlID="txtAddPracticePhone"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredAddRegCode" runat="server" TargetControlID="txtAddRegCode"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnPracticeEdit" runat="server" CausesValidation="False" AlternateText="<%$ Resources:AlternateText, EditSBtn %>"
                                                    CommandName="Edit" ImageUrl="<%$ Resources:ImageUrl, EditSBtn %>" />
                                                <asp:ImageButton ID="ibtnPracticeDelete" runat="server" AlternateText="<% $ Resources:AlternateText, DeleteSBtn%>"
                                                    ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>" OnClick="ibtnPracticeDelete_Click" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="ibtnPracticeSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveSBtn %>"
                                                    CommandName="Update" ImageUrl="<%$ Resources:ImageUrl, SaveSBtn %>" />
                                                <asp:ImageButton ID="ibtnPracticeCancel" runat="server" CausesValidation="False"
                                                    AlternateText="<%$ Resources:AlternateText, CancelSBtn %>" CommandName="Cancel"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelSBtn %>" />
                                            </EditItemTemplate>
                                            <ItemStyle Width="80px" VerticalAlign="Top" />
                                            <FooterTemplate>
                                                <asp:ImageButton ID="ibtnPracticeAddRecord" runat="server" CommandName="Add" ImageUrl="<%$ Resources:ImageUrl, SaveSBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, SaveSBtn %>" />
                                                <asp:ImageButton ID="ibtnPracticeCancelAdd" runat="server" CommandName="CancelAdd"
                                                    AlternateText="<%$ Resources:AlternateText, CancelSBtn %>" ImageUrl="<%$ Resources:ImageUrl, CancelSBtn %>" />
                                            </FooterTemplate>
                                            <FooterStyle Width="80px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <asp:ImageButton ID="ibtnPracticeAdd" runat="server" OnClick="ibtnPracticeAdd_Click"
                                                AlternateText="<%$ Resources:AlternateText, AddSBtn %>" ImageUrl="<%$ Resources:ImageUrl, AddSBtn %>" /></td>
                                    </tr>
                                </table>
                                <asp:Panel ID="panEHRSS" runat="server" BorderColor="#E0E0E0" BorderStyle="Solid"
                                    BorderWidth="1px">
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="headingText">
                                                    <asp:Label ID="lblEHRSSText" runat="server" Text="<%$ Resources:Text, EHRSSVU %>"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblEHRSSStatement" runat="server" Text="<%$ Resources:Text, JoinEHRSSStatement %>"></asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblHadJoinedEHRSSQ" runat="server" Text="<%$ Resources:Text, HadJoinedEHRSSQVU %>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:RadioButtonList ID="rboHadJoinedEHRSS" runat="server" RepeatDirection="Vertical">
                                                                <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                                                                <asp:ListItem Value="N" Text="No"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td valign="top">
                                                            <asp:Image ID="imgHadJoinedEHRSSAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="headingText">
                                                    <asp:Label ID="lblJoinPCDText" runat="server" Text="<%$ Resources:Text, PCD_Short %>"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblJoinPCDStatement" runat="server" Text="<%$ Resources:Text, JoinPCDStatement %>"></asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblJoinPCDQ" runat="server" Text="<%$ Resources:Text, WillJoinPCD %>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:RadioButtonList ID="rboJoinPCD" runat="server" RepeatDirection="Vertical">
                                                                <asp:ListItem Value="Y" Text="<%$ Resources:Text, Yes %>"></asp:ListItem>
                                                                <asp:ListItem Value="E" Text="<%$ Resources:Text, No_JoinedPCD %>"></asp:ListItem>
                                                                <asp:ListItem Value="N" Text="<%$ Resources:Text, No_NotJoinPCD %>"></asp:ListItem>
                                                            </asp:RadioButtonList></td>
                                                        <td valign="top">
                                                            <asp:Image ID="imgJoinPCDAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="right">
                                                <asp:ImageButton ID="ibtnEHRSSSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveSBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, SaveSBtn %>" OnClick="ibtnEHRSSSave_Click"
                                                    Visible="False" />&nbsp;<asp:ImageButton ID="ibtnEHRSSCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelSBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, CancelSBtn %>" OnClick="ibtnEHRSSCancel_Click"
                                                        Visible="False" /><asp:ImageButton ID="ibtnEHRSSEdit" runat="server" AlternateText="<%$ Resources:AlternateText, EditSBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, EditSBtn %>" OnClick="ibtnEHRSSEdit_Click"
                                                            Visible="False" /></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnPracticePageChecked" runat="server" AlternateText="<%$ Resources:AlternateText, PageCheckedBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, PageCheckedBtn %>" OnClick="ibtnPageChecked_Click" /></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tabBankAcctInfo" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblBankInfo" runat="server" Text="<%$ Resources:Text, BankInfo %>"></asp:Label>
                                <asp:Image ID="imgBankInfo" runat="server" ImageUrl="~/Images/others/small_tick.png" CssClass="tabIcon"
                                    AlternateText="<%$ Resources:AlternateText, PageCheckedImg %>" ToolTip="<%$ Resources:ToolTip, PageCheckedImg %>" />
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView ID="gvBankInfo" runat="server" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle VerticalAlign="Top" Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPracticeBankDispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                                <asp:HiddenField ID="hfBankAcctStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblEditPracticeBankDispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                                <asp:HiddenField ID="hfEditBankDisplaySeq" runat="server" Value='<%# Eval("BankAcct.DisplaySeq") %>' />
                                                <asp:HiddenField ID="hfEditBankAcctStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, BankInfo %>">
                                            <ItemTemplate>
                                                <table style="width: 100%;table-layout:fixed">
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankPracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                            <asp:Label runat="server" ID="lblBankStatusInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankPracticeStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankAcct.BankName") %>'
                                                                CssClass="TextChi"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BankAcct.BranchName") %>'
                                                                CssClass="TextChi"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankAcc" runat="server" Text='<%# Eval("BankAcct.BankAcctNo") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblBankOwner" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                                CssClass="tableText" Width="640px" Style="word-wrap:break-word"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditBankPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblEditBankPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditBankPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblEditBankPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditBankPracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                            <asp:Label runat="server" ID="lblBankStatusInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblEditBankPracticeStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditBankName" runat="server" Text='<%# Eval("BankAcct.BankName") %>' CssClass="TextBoxChi"
                                                                Width="260px" MaxLength="100"></asp:TextBox>
                                                            <asp:Image ID="imgEditBankNameAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                                runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtEditBranchName" runat="server" Text='<%# Eval("BankAcct.BranchName") %>' CssClass="TextBoxChi"
                                                                Width="260px" MaxLength="100"></asp:TextBox>
                                                            <asp:Image ID="imgEditBranchNameAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                                runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                        <td>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td runat="server" id="tBankAcc">
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtEditBankCode" runat="server" MaxLength="3" Width="25px"></asp:TextBox></td>
                                                                                <td>-</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtEditBranchCode" runat="server" MaxLength="3" Width="25px"></asp:TextBox></td>
                                                                                <td>-</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtEditBankAcc" runat="server" MaxLength="9" Width="70px"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEditBankAccNoFreeText" runat="server" MaxLength="30" Width="260px"></asp:TextBox></td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkEditIsFreeTextFormat" runat="server" Text='<%$ Resources:Text, FreeFormat %>'
                                                                            Checked='<%# IIF(Eval("BankAcct.IsFreeTextFormat") = "Y", True, False) %>' />
                                                                        <asp:Image ID="imgEditBankAccAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                                            ImageUrl="~/Images/others/icon_caution.gif" Visible="False" /></td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="hfEditBankAccNo" runat="server" Value='<%# Eval("BankAcct.BankAcctNo") %>' />
                                                            <asp:Label ID="lblEditBankAcctTip" runat="server" Text="<%$ Resources:Text, BankAcctNoTip %>"></asp:Label></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtEditBankOwner" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                                Width="520px" Height="80px" MaxLength="300" TextMode="MultiLine" style="overflow:hidden"></asp:TextBox>
                                                            <asp:TextBox ID="txtEditBankOwnerFreeText" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                                Width="520px" Height="80px" MaxLength="300" TextMode="MultiLine" style="overflow:hidden"></asp:TextBox>
                                                            <asp:Image ID="imgEditBankOwnerAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                                runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" Style="position: relative; top: -58px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <cc1:FilteredTextBoxExtender ID="FilteredBankCode" runat="server" TargetControlID="txtEditBankCode"
                                                    FilterType="Custom, Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredBranchCode" runat="server" TargetControlID="txtEditBranchCode"
                                                    FilterType="Custom, Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredBankAcc" runat="server" TargetControlID="txtEditBankAcc"
                                                    FilterType="Custom, Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender ID="FilteredBankAccNoFreeText" runat="server" TargetControlID="txtEditBankAccNoFreeText"
                                                    FilterType="Custom, Numbers" ValidChars="-" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredEditBankOwner" runat="server" TargetControlID="txtEditBankOwner"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                                </cc1:FilteredTextBoxExtender>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnBankEdit" runat="server" CausesValidation="False" AlternateText="<%$ Resources:AlternateText, EditSBtn %>"
                                                    CommandName="Edit" ImageUrl="<%$ Resources:ImageUrl, EditSBtn %>" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="ibtnBankSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveSBtn %>"
                                                    CommandName="Update" ImageUrl="<%$ Resources:ImageUrl, SaveSBtn %>" />
                                                <asp:ImageButton ID="ibtnBankCancel" runat="server" CausesValidation="False" AlternateText="<%$ Resources:AlternateText, CancelSBtn %>"
                                                    CommandName="Cancel" ImageUrl="<%$ Resources:ImageUrl, CancelSBtn %>" />
                                            </EditItemTemplate>
                                            <ItemStyle Width="80px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnBankAcctPageChecked" runat="server" AlternateText="<%$ Resources:AlternateText, PageCheckedBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, PageCheckedBtn %>" OnClick="ibtnPageChecked_Click" /></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tablSchemeInfo" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblSchemeInfo" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
                                <asp:Image ID="imgSchemeInfo" runat="server" ImageUrl="~/Images/others/small_tick.png" CssClass="tabIcon"
                                    AlternateText="<%$ Resources:AlternateText, PageCheckedImg %>" ToolTip="<%$ Resources:ToolTip, PageCheckedImg %>" />
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:GridView ID="gvSI" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvSI_RowDataBound"
                                    OnRowEditing="gvSI_RowEditing" OnRowCancelingEdit="gvSI_RowCancelingEdit" OnRowUpdating="gvSI_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle VerticalAlign="Top" Width="15px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSchemeDispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblSchemeEditDispalySeq" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, SchemeInfo %>">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemePracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblSchemePracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemePracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblSchemePracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemeHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSchemeHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:HiddenField ID="hfSchemeHealthProf" runat="server" Value='<%# Eval("Professional.ServiceCategoryCode") %>' />
                                                            <asp:HiddenField ID="hfHasScheme" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemePracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                            <asp:Label runat="server" ID="lblSchemePracticeStatusInd" Text="*" ForeColor="Red"
                                                                Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblSchemePracticeStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemeInfoText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
                                                            <asp:Label ID="lblSchemeInfoTextInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblSchemeInfoNA" runat="server" Text="<%$ Resources:Text, N/A %>"
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                </table>
                                                <asp:GridView runat="server" ID="gvPracticeSchemeInfo" AutoGenerateColumns="false"
                                                    Width="100%" SkinID="SchemeGridview" OnRowDataBound="gvServiceFee_RowDataBound"
                                                    OnRowCreated="gvServiceFee_RowCreated" OnPreRender="gvServiceFee_PreRender">
                                                    <Columns>
                                                        <asp:BoundField DataField="SchemeCode">
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SubsidizeCode">
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle VerticalAlign="Top" Width="25px" />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" Enabled="false" />
                                                                <asp:HiddenField ID="hfGIsCategoryHeader" runat="server" Value='<%# Eval("IsCategoryHeader") %>' />
                                                                <asp:HiddenField ID="hfGCategoryName" runat="server" Value='<%# Eval("CategoryName") %>' />
                                                                <asp:HiddenField ID="hfGAllNotProvideService" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                            <ItemStyle HorizontalAlign="left" CssClass="tableText" VerticalAlign="Top" Width="110px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGSchemeDisplayCode" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="SubsidizeDisplayCode" HeaderText="<%$ Resources:Text, SubsidyAndServiceFee %>">
                                                            <ItemStyle HorizontalAlign="left" CssClass="tableText" VerticalAlign="Top" Width="90px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" Width="195px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPracticeSchemeServiceFee" runat="server" CssClass="tableText">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>">
                                                            <ItemStyle VerticalAlign="Top" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPracticeSchemeStatus" runat="server" CssClass="tableText">
                                                                </asp:Label>
                                                                <asp:Label ID="lblPracticeSchemeRemark" runat="server" CssClass="tableText">
                                                                </asp:Label>
                                                                <asp:HiddenField ID="hfPracticeSchemeStatus" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveTime %>">
                                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPracticeSchemeEffectiveDtm" runat="server" CssClass="tableText"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingTime %>">
                                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPracticeSchemeDelistDtm" runat="server" CssClass="tableText"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemeEditPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblSchemeEditPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemeEditPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblSchemeEditPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District"), Eval("PracticeAddress.AreaCode")) %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemeEditHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSchemeEditHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:HiddenField ID="hfSchemeEditHealthProf" runat="server" Value='<%# Eval("Professional.ServiceCategoryCode") %>' />
                                                            <asp:HiddenField ID="hfEditHasScheme" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblEditSchemePracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                                            <asp:Label runat="server" ID="lblEditSchemePracticeStatusInd" Text="*" ForeColor="Red"
                                                                Visible="False"></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblEditSchemePracticeStatus" runat="server" Text='<%# Eval("RecordStatus") %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblSchemeEditInfoText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
                                                            <asp:Label ID="lblSchemeEditInfoTextInd" runat="server" Text="*" ForeColor="Red"
                                                                Visible="False"></asp:Label></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                                <asp:GridView runat="server" ID="gvEditPracticeSchemeInfo" AutoGenerateColumns="false"
                                                    Width="100%" SkinID="SchemeGridview" OnRowDataBound="gvServiceFee_RowDataBound"
                                                    OnRowCreated="gvServiceFee_RowCreated" OnPreRender="gvServiceFee_PreRender">
                                                    <Columns>
                                                        <asp:BoundField DataField="SchemeCode">
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SubsidizeCode">
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemStyle VerticalAlign="Top" Width="25px" />
                                                            <HeaderTemplate>
                                                                <asp:Image ID="imgEditSelectAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                                    runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkEditSelect" runat="server" OnCheckedChanged="chkEditSelect_CheckedChanged"
                                                                    AutoPostBack="false" />
                                                                <asp:HiddenField ID="hfGIsCategoryHeader" runat="server" Value='<%# Eval("IsCategoryHeader") %>' />
                                                                <asp:HiddenField ID="hfGCategoryName" runat="server" Value='<%# Eval("CategoryName") %>' />
                                                                <asp:HiddenField ID="hfGAllNotProvideService" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="left" Width="110px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEditPracticeSchemeCode" runat="server" CssClass="tableText" />
                                                                <asp:Image ID="imgEditSelectSubsidizeAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                                    runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" />
                                                                <br />
                                                                <asp:Panel ID="panGNonClinic" runat="server">
                                                                    <asp:CheckBox ID="chkGNonClinic" runat="server" Text="<%$ Resources:Text, NonClinic %>" />
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, SubsidyAndServiceFee %>">
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="left" Width="90px" />
                                                            <ItemTemplate>
                                                                <asp:Panel ID="pnlEditPracticeSchemeSubsidize" runat="server">
                                                                    <table width="100%" border="0">
                                                                        <tr>
                                                                            <td width="20%" valign="top">
                                                                                <asp:CheckBox ID="chkEditSelectSubsidize" runat="server" />
                                                                            </td>
                                                                            <td width="80%">
                                                                                <asp:Label ID="lblEditPracticeSchemeSubsidizeCode" runat="server" CssClass="tableText"
                                                                                    Text="[codebehind]"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="left" Width="195px" />
                                                            <ItemTemplate>
                                                                <asp:Panel ID="pnlEditPracticeSchemeServiceFeeDisplay" runat="server">
                                                                    <table width="100%">
                                                                        <tr valign="top">
                                                                            <td align="left" width="32%">
                                                                                <asp:Label ID="lblEditPracticeSchemeServiceFee" runat="server" CssClass="tableText"
                                                                                    Text="<%$ Resources:Text, N/A %>"></asp:Label>
                                                                                <asp:Panel ID="pnlEditPracticeSchemeServiceFee" runat="server">
                                                                                    <asp:Label ID="lblEditPracticeSchemeServiceFeeDollar" runat="server" CssClass="tableText"
                                                                                        Text="$"></asp:Label>
                                                                                    <asp:TextBox ID="txtEditPracticeSchemeServiceFee" runat="server" Width="40px" MaxLength="4"
                                                                                        onblur="changeInt(this);"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="FilteredServieFee" runat="server" TargetControlID="txtEditPracticeSchemeServiceFee"
                                                                                        FilterType="Custom, Numbers">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </asp:Panel>
                                                                            </td>
                                                                            <asp:Panel ID="pnlEditPracticeSchemeServiceFeeCompulsory" runat="server">
                                                                                <td align="left" width="68%">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:CheckBox ID="chkEditNotProvideServiceFee" runat="server" Text="[CodeBehind]" CssClass="chkLabel" Width="85%" /></td>
                                                                                            <td valign="top">
                                                                                                <asp:Image ID="imgEditServiceFeeAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                                                                    runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </asp:Panel>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>">
                                                            <ItemStyle VerticalAlign="Top" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEditPracticeSchemeStatus" runat="server" CssClass="tableText">
                                                                </asp:Label>
                                                                <asp:Label ID="lblEditPracticeSchemeRemark" runat="server" CssClass="tableText">
                                                                </asp:Label>
                                                                <asp:HiddenField ID="hfEditPracticeSchemeStatus" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveTime %>">
                                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEditPracticeSchemeEffectiveDtm" runat="server" CssClass="tableText"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingTime %>">
                                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEditPracticeSchemeDelistDtm" runat="server" CssClass="tableText"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnSchemeEdit" runat="server" CausesValidation="False" AlternateText="<%$ Resources:AlternateText, EditSBtn %>"
                                                    CommandName="Edit" ImageUrl="<%$ Resources:ImageUrl, EditSBtn %>" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="ibtnSchemeSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveSBtn %>"
                                                    CommandName="Update" ImageUrl="<%$ Resources:ImageUrl, SaveSBtn %>" />
                                                <asp:ImageButton ID="ibtnSchemeCancel" runat="server" CausesValidation="False" AlternateText="<%$ Resources:AlternateText, CancelSBtn %>"
                                                    CommandName="Cancel" ImageUrl="<%$ Resources:ImageUrl, CancelSBtn %>" />
                                            </EditItemTemplate>
                                            <ItemStyle Width="80px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="ibtnSchemePageChecked" runat="server" AlternateText="<%$ Resources:AlternateText, PageCheckedBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, PageCheckedBtn %>" OnClick="ibtnPageChecked_Click" /></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                            <td align="right">
                                <asp:ImageButton ID="ibtnProceedToVetting" runat="server" AlternateText="<%$ Resources:AlternateText, ProceedToVettingBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ProceedToVettingBtn %>" OnClick="ibtnProceedToVetting_Click" />
                                <asp:ImageButton ID="ibtnReject" runat="server" AlternateText="<%$ Resources:AlternateText, AbortBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, AbortBtn %>" OnClick="ibtnReject_Click" /></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewComplete" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="ibtnCompleteBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>&nbsp;
            <asp:HiddenField ID="hfERN" runat="server" />
            <asp:HiddenField ID="hfTableLocation" runat="server" />
            <asp:Button runat="server" ID="btnHiddenShowDialog" Style="display: none" />
            <asp:Button runat="server" ID="btnHiddenShowExistingSPProfile" Style="display: none" />
            <asp:Button runat="server" ID="btnHiddenShowDialogForDelete" Style="display: none" />
            <asp:Button runat="server" ID="btnHiddenShowDeletePractice" Style="display: none" />
            <asp:Button runat="server" ID="btnHiddenShowDeleteMO" Style="display: none" />
            <asp:Button runat="server" ID="btnHiddenShowDialogForMigrate" Style="display: none" />
            <asp:Button runat="server" ID="btnHiddenDuplicated" Style="display: none" />
            <asp:Button runat="server" ID="btnHiddenShowPCDWarning" Style="display: none" />
            <asp:Button runat="server" ID="btnNoticePopupDummy" Style="display: none" />

            <asp:HiddenField ID="hfGridviewIndex" runat="server" />
            <asp:HiddenField ID="hfSelectedAddressEng" runat="server" />
            <asp:HiddenField ID="hfSelectedAddressChi" runat="server" />
            <asp:HiddenField ID="hfSelectedAddressRecordID" runat="server" />
            <asp:HiddenField ID="hfSelectedAddressDistrictCode" runat="server" />
            <asp:HiddenField ID="hfSelectedAddressAreaCode" runat="server" />
            <asp:HiddenField ID="hfPCDWarningPopupType" runat="server" />
            <cc1:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnHiddenShowDialog"
                PopupControlID="panStructureAddress" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmDelete" runat="server" TargetControlID="btnHiddenShowDialogForDelete"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderSPProfile" runat="server" TargetControlID="btnHiddenShowExistingSPProfile"
                PopupControlID="panExistingSPProfile" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panExistingSPProfileHeading">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderDeletePractice" runat="server" TargetControlID="btnHiddenShowDeletePractice"
                PopupControlID="panDeletePractice" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDeletePracticeHeading">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderDeleteMO" runat="server" TargetControlID="btnHiddenShowDeleteMO"
                PopupControlID="panDeleteMO" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDeleteMOHeading">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderDuplicated" runat="server" TargetControlID="btnHiddenDuplicated"
                PopupControlID="panDuplicated" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDuplicatedHeading">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="ModelPopupExtenderPCDWarning" runat="server" TargetControlID="btnHiddenShowPCDWarning"
                PopupControlID="panPopupPCDWarning" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="btnNoticePopupDummy"
                PopupControlID="panNoticePopup" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        //CRE15-004 TIV & QIV [Start][Winnie] Overwrite existing tab function
        Sys.Extended.UI.TabContainer.prototype._app_onload = function (sender, e) {
            if (this._cachedActiveTabIndex != -1) {
                this.set_activeTabIndex(this._cachedActiveTabIndex);
                this._cachedActiveTabIndex = -1;

                var activeTab = this.get_tabs()[this._activeTabIndex];
                if (activeTab) {
                    activeTab._wasLoaded = true;
                    //activeTab._setFocus(activeTab); -- disable focus on active tab in the last TabContainer
                }
            }
            this._loaded = true;
        }
        //CRE15-004 TIV & QIV [End][Winnie]
    </script>
</asp:Content>
