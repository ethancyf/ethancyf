<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="FGSTester._Default" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eHealth System Consent Form Generation Service - Testing Page</title>
    <link rel="Stylesheet" href="core.css" />
    <base target="_blank" />

    <script type="text/javascript">
    function Set1() 
    {
        // ----- Set FormType -----
        if (document.getElementById('rblFormType_0').checked) {
            // HCVS
            document.getElementById('FormType').value = document.getElementById('rblFormType_0').value;
            
        } else if (document.getElementById('rblFormType_1').checked) {
            // CIVSS
            document.getElementById('FormType').value = document.getElementById('rblFormType_1').value;
            
        } else if (document.getElementById('rblFormType_2').checked) {
            // EVSS
            document.getElementById('FormType').value = document.getElementById('rblFormType_2').value;
          
        } else if (document.getElementById('rblFormType_3').checked) {
            document.getElementById('FormType').value = document.getElementById('txtFormTypeOther').value;
        
        }
        
        // ----- Set Platform -----
        if (document.getElementById('rblPlatform_0').checked) {
            document.getElementById('Platform').value = document.getElementById('rblPlatform_0').value;
            
        } else if (document.getElementById('rblPlatform_1').checked) {
            document.getElementById('Platform').value = document.getElementById('rblPlatform_1').value;
            
        } else if (document.getElementById('rblPlatform_2').checked) {
            document.getElementById('Platform').value = document.getElementById('txtPlatformOther').value;
        
        }
        
        // ----- Set Language -----
        if (document.getElementById('rblLanguage_0').checked) {
            document.getElementById('Language').value = document.getElementById('rblLanguage_0').value;
            
        } else if (document.getElementById('rblLanguage_1').checked) {
            document.getElementById('Language').value = document.getElementById('rblLanguage_1').value;
            
        } else if (document.getElementById('rblLanguage_2').checked) {
            document.getElementById('Language').value = document.getElementById('txtLanguageOther').value;
        
        }
        
        // ----- Set FormStyle -----
        if (document.getElementById('rblFormStyle_0').checked) {
            document.getElementById('FormStyle').value = document.getElementById('rblFormStyle_0').value;
            
        } else if (document.getElementById('rblFormStyle_1').checked) {
            document.getElementById('FormStyle').value = document.getElementById('rblFormStyle_1').value;
            
        } else if (document.getElementById('rblFormStyle_2').checked) {
            document.getElementById('FormStyle').value = document.getElementById('txtFormStyleOther').value;
        
        }
        
        // ----- Set NeedPassword -----
        if (document.getElementById('rblNeedPassword_0').checked) {
            document.getElementById('NeedPassword').value = document.getElementById('rblNeedPassword_0').value;
            
        } else if (document.getElementById('rblNeedPassword_1').checked) {
            document.getElementById('NeedPassword').value = document.getElementById('rblNeedPassword_1').value;
        
        } else if (document.getElementById('rblNeedPassword_2').checked) {
            document.getElementById('NeedPassword').value = document.getElementById('txtNeedPasswordOther').value;
            
        }
        
        // ----- Set Document Type -----
        if (document.getElementById('rblFormType_0').checked || document.getElementById('rblFormType_2').checked) {
            if (document.getElementById('rblDocType_0').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_0').value;
                
            } else if (document.getElementById('rblDocType_1').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_1').value;
            
            } else if (document.getElementById('rblDocType_2').checked) {
                document.getElementById('DocType').value = document.getElementById('txtDocTypeOther').value;
                
            }
        
        } else if (document.getElementById('rblFormType_1').checked) {
            if (document.getElementById('rblDocType_0').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_0').value;
                
            } else if (document.getElementById('rblDocType_1').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_1').value;
            
            } else if (document.getElementById('rblDocType_2').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_2').value;
            
            } else if (document.getElementById('rblDocType_3').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_3').value;
            
            } else if (document.getElementById('rblDocType_4').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_4').value;
            
            } else if (document.getElementById('rblDocType_5').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_5').value;
            
            } else if (document.getElementById('rblDocType_6').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_6').value;
            
            } else if (document.getElementById('rblDocType_7').checked) {
                document.getElementById('DocType').value = document.getElementById('txtDocTypeOther').value;
                
            }
            
        } else if (document.getElementById('rblFormType_3').checked) {
            if (document.getElementById('rblDocType_0').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_0').value;
                
            } else if (document.getElementById('rblDocType_1').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_1').value;
            
            } else if (document.getElementById('rblDocType_2').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_2').value;
            
            } else if (document.getElementById('rblDocType_3').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_3').value;
            
            } else if (document.getElementById('rblDocType_4').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_4').value;
            
            } else if (document.getElementById('rblDocType_5').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_5').value;
            
            } else if (document.getElementById('rblDocType_6').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_6').value;
            
            } else if (document.getElementById('rblDocType_7').checked) {
                document.getElementById('DocType').value = document.getElementById('rblDocType_7').value;
            
            } else if (document.getElementById('rblDocType_8').checked) {
                document.getElementById('DocType').value = document.getElementById('txtDocTypeOther').value;
                
            }
            
        }
        
        // ----- Set Gender -----
        if (document.getElementById('rblRecpGender_0') != null) {
            if (document.getElementById('rblRecpGender_0').checked) {
                document.getElementById('RecpGender').value = document.getElementById('rblRecpGender_0').value;
                
            } else if (document.getElementById('rblRecpGender_1').checked) {
                document.getElementById('RecpGender').value = document.getElementById('rblRecpGender_1').value;
            
            } else if (document.getElementById('rblRecpGender_2').checked) {
                document.getElementById('RecpGender').value = document.getElementById('rblRecpGender_2').value;
            
            } else if (document.getElementById('rblRecpGender_3').checked) {
                document.getElementById('RecpGender').value = document.getElementById('txtRecpGenderOther').value;
                
            }
        }
        
        // ----- Set UseSmartIC -----
        if (document.getElementById('rblUseSmartIC_0') != null) {
            if (document.getElementById('rblUseSmartIC_0').checked) {
                document.getElementById('UseSmartIC').value = document.getElementById('rblUseSmartIC_0').value;
                
            } else if (document.getElementById('rblUseSmartIC_1').checked) {
                document.getElementById('UseSmartIC').value = document.getElementById('rblUseSmartIC_1').value;
            
            } else if (document.getElementById('rblUseSmartIC_2').checked) {
                document.getElementById('UseSmartIC').value = document.getElementById('rblUseSmartIC_2').value;
            
            } else if (document.getElementById('rblUseSmartIC_3').checked) {
                document.getElementById('UseSmartIC').value = document.getElementById('txtUseSmartICOther').value;
                
            }
        }
        
        // ----- Set SubsidyCode -----
        switch (document.getElementById('FormType').value) {
            case 'HCVS':
                break;
                
            case 'CIVSS':
                if (document.getElementById('rblCIVSSSubsidyCode_0').checked) {
                    document.getElementById('SubsidyCode').value = 'CSIV-1ST';
                } else if (document.getElementById('rblCIVSSSubsidyCode_1').checked) {
                    document.getElementById('SubsidyCode').value = 'CSIV-2ND';
                } else if (document.getElementById('rblCIVSSSubsidyCode_2').checked) {
                    document.getElementById('SubsidyCode').value = 'CSIV';
                } else if (document.getElementById('rblCIVSSSubsidyCode_3').checked) {
                    document.getElementById('SubsidyCode').value = '';
                } else if (document.getElementById('rblCIVSSSubsidyCode_4').checked) {
                    document.getElementById('SubsidyCode').value = document.getElementById('txtCIVSSSubsidyCodeOther').value;
                }
                
                // Preschool
                if ((document.getElementById('rblCIVSSSubsidyCode_0').checked) ||
                        (document.getElementById('rblCIVSSSubsidyCode_2').checked) ||
                        (document.getElementById('rblCIVSSSubsidyCode_4').checked)) {
                    if (document.getElementById('rblCIVSSPreschool_0').checked) {
                        document.getElementById('CIVSSPreSchool').value = document.getElementById('rblCIVSSPreschool_0').value;
                    } else if (document.getElementById('rblCIVSSPreschool_1').checked) {
                        document.getElementById('CIVSSPreSchool').value = document.getElementById('rblCIVSSPreschool_1').value;
                    } else if (document.getElementById('rblCIVSSPreschool_2').checked) {
                        document.getElementById('CIVSSPreSchool').value = document.getElementById('rblCIVSSPreschool_2').value;
                    } else if (document.getElementById('rblCIVSSPreschool_3').checked) {
                        document.getElementById('CIVSSPreSchool').value = document.getElementById('txtCIVSSPreschoolOther').value;
                    }
                } else {
                    //document.getElementById('CIVSSPreSchool').value = '';
                }
                
                break;
                
            case 'EVSS':
                var strSubsidyCode = '';
                
                if (document.getElementById('cblEVSSSubsidyCode_0').checked) {
                    strSubsidyCode += ',23vPPV';
                } 
                if (document.getElementById('cblEVSSSubsidyCode_1').checked) {
                    strSubsidyCode += ',ESIV';
                }
                if (document.getElementById('cblEVSSSubsidyCode_2').checked) {
                    strSubsidyCode += ',' + document.getElementById('txtEVSSSubsidyCodeOther').value;
                }
                
                document.getElementById('SubsidyCode').value = strSubsidyCode.substring(1);
                
                break;
            
            default:
                break;
        }

        // ----- Remove the aspx post data -----
        var v = document.getElementById('__VIEWSTATE');
        v.parentNode.removeChild(v);
        
        v = document.getElementById('__EVENTTARGET');
        v.parentNode.removeChild(v);

        // ----- Prepare new window -----
        
        var width = 1020;
        var height = 500;
        
        var left = parseInt((screen.availWidth / 2) - (width / 2));
        var top = parseInt((screen.availHeight / 2) - (height / 2));
    
        document.form1.target = '_self';
        document.form1.action = 'Default.aspx';
        
        w = window.open('blank.htm', 'NewWindow', 'scrollbars=no,menubar=no,height='+height+',width='+width+',left='+left+',top='+top+',resizable=yes,toolbar=no,status=no');
    
        w.resizeTo(width, height);
        w.moveTo(left, top);
        
        document.form1.target = 'NewWindow';
        document.form1.action = 'http://localhost/FGS/gencsf.aspx';
    }
    
    function Set2() 
    {
        document.form1.target = '_self';
        document.form1.action = 'Default.aspx';
    }
    
    </script>

</head>
<body>
    <h1>
        eHealth System Consent Form Generation Service - Testing Page</h1>
    <h5>
        v2.5 - 15 October 2010</h5>
    <hr />
    <form id="form1" runat="server" method="post">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 95%" cellpadding="3" cellspacing="1">
                    <tr>
                        <td style="background-color: #ffffff; text-align: center">
                            <table style="width: 95%; background-color: #cccccc" cellpadding="3" cellspacing="1">
                                <tr>
                                    <td style="background-color: #ffffff; text-align: left">
                                        <table style="width: 100%" cellpadding="2">
                                            <tr>
                                                <td style="height: 10px; background-color: #ffffff" class="sectionheader">
                                                    Control Information:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 3px; background-color: #0099ff">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 20px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="lblPlatform" runat="server" Text="Platform" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblPlatform" runat="server" RepeatDirection="Horizontal"
                                                                    AutoPostBack="True" CssClass="inputvalue">
                                                                    <asp:ListItem Text="FGS" Value="FGS" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="HCSP" Value="HCSP"></asp:ListItem>
                                                                    <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlatformOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                <asp:HiddenField ID="Platform" runat="server" />
                                                            </td>
                                                        </tr>
                                                        </table>
                                                    <table cellpadding="0" cellspacing="0">                                                        
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="RequestByText" runat="server" Text="Request By" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="RequestBy" runat="server" Width="100px" Text="Demo" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="FormTypeText" runat="server" Text="Form Type" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblFormType" runat="server" RepeatDirection="Horizontal"
                                                                                AutoPostBack="True" onclick="Set2()" CssClass="inputvalue">
                                                                                <asp:ListItem Text="HCVS" Value="HCVS" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="CIVSS" Value="CIVSS"></asp:ListItem>
                                                                                <asp:ListItem Text="EVSS" Value="EVSS"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFormTypeOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:HiddenField ID="FormType" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="LanguageText" runat="server" Text="Language" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblLanguage" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="Chinese" Value="Chinese" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="English" Value="English"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLanguageOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:HiddenField ID="Language" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="FormStyleText" runat="server" Text="Form Style" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblFormStyle" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="Full" Value="Full" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="Condensed" Value="Condensed"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtFormStyleOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:HiddenField ID="FormStyle" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="NeedPasswordText" runat="server" Text="Need Password" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblNeedPassword" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtNeedPasswordOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:HiddenField ID="NeedPassword" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table style="width: 95%; background-color: #cccccc" cellpadding="3" cellspacing="1">
                                <tr>
                                    <td style="background-color: #ffffff; text-align: left">
                                        <table style="width: 100%" cellpadding="2">
                                            <tr>
                                                <td style="height: 10px; background-color: #ffffff" class="sectionheader">
                                                    Service Provider and Recipient Information:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 3px; background-color: #009933">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 20px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="DocumentTypeText" runat="server" Text="Document Type" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblDocType" runat="server" RepeatDirection="Horizontal"
                                                                                AutoPostBack="True" onclick="Set2()">
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDocTypeOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:HiddenField ID="DocType" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="SPNameText" runat="server" Text="SP Name" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="SPName" runat="server" Width="300px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="RecpNameText" runat="server" Text="Recipient English Name" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="RecpName" runat="server" Width="300px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="RecpNameChiText" runat="server" Text="Recipient Chinese Name" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="RecpNameChi" runat="server" Width="120px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="RecpDOBStrText" runat="server" Text="Date of Birth" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="RecpDOBStr" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="RecpGenderText" runat="server" Text="Gender" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblRecpGender" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                                                                <asp:ListItem Text="F" Value="F"></asp:ListItem>
                                                                                <asp:ListItem Text="Not Provided" Value="" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRecpGenderOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:TextBox ID="RecpGender" runat="server" Style="display: none"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ECSerialNoText" runat="server" Text="Serial No." CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="ECSerialNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ECRefNoText" runat="server" Text="Reference No." CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="ECRefNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="HKBCRegNoText" runat="server" Text="Reg. No." CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="HKICNoText" runat="server" Text="HKIC No." CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="REPMTPermitNoText" runat="server" Text="Permit No." CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="DocIDocNoText" runat="server" Text="Document No." CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="ID235BBirthEntryNoText" runat="server" Text="Entry No." CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="PassportNoText" runat="server" Text="Passport No." CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="ADOPCEntryNoText" runat="server" Text="Entry No." CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="HKBCRegNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="HKICNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="REPMTPermitNo" runat="server" Width="100px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="DocIDocNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="ID235BBirthEntryNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="PassportNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="ADOPCEntryNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="DocDOIStrText" runat="server" Text="Date of Issue" CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="ID235BRemainUntilStrText" runat="server" Text="Permitted Remain Until"
                                                                    CssClass="inputheadertext"></asp:Label>
                                                                <asp:Label ID="VisaNoText" runat="server" Text="Visa No." CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="DocDOIStr" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="ID235BRemainUntilStr" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                                <asp:TextBox ID="VisaNo" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table style="width: 95%; background-color: #cccccc" cellpadding="3" cellspacing="1">
                                <tr>
                                    <td style="background-color: #ffffff; text-align: left">
                                        <table style="width: 100%" cellpadding="2">
                                            <tr>
                                                <td style="height: 10px; background-color: #ffffff" class="sectionheader">
                                                    Subsidy Information:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 3px; background-color: #dd3333">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 20px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="ServiceDateStrText" runat="server" Text=" Service Date" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="ServiceDateStr" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="SignDateStrText" runat="server" Text="Sign Date" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="SignDateStr" runat="server" Width="160px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="UseSmartICText" runat="server" Text="Read Smart ID Card" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblUseSmartIC" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                                <asp:ListItem Text="Not Provided" Value="" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtUseSmartICOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:TextBox ID="UseSmartIC" runat="server" Style="display: none"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="VoucherClaimedText" runat="server" Text="No. of Vouchers Claimed"
                                                                    CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="VoucherClaimed" runat="server" Width="40px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CoPaymentFeeText" runat="server" Text="Co-payment Fee" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="CoPaymentFee" runat="server" Width="40px" CssClass="plain"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CIVSSSubsidyCodeText" runat="server" Text="Subsidy Information" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblCIVSSSubsidyCode" runat="server" RepeatDirection="Horizontal"
                                                                                AutoPostBack="True" onclick="Set2()">
                                                                                <asp:ListItem Text="1st Dose" Value="1STDOSE"></asp:ListItem>
                                                                                <asp:ListItem Text="2nd Dose" Value="2NDDOSE"></asp:ListItem>
                                                                                <asp:ListItem Text="Only Dose" Value="ONLYDOSE"></asp:ListItem>
                                                                                <asp:ListItem Text="Not Provided" Value="" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCIVSSSubsidyCodeOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CIVSSPreschoolText" runat="server" Text="Preschool" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rblCIVSSPreschool" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="Preschool" Value="PreSchool"></asp:ListItem>
                                                                                <asp:ListItem Text="Non-preschool" Value="NonPreSchool"></asp:ListItem>
                                                                                <asp:ListItem Text="Not Provided" Value="" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtCIVSSPreschoolOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                            <asp:TextBox ID="CIVSSPreSchool" runat="server" Style="display: none"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="EVSSSubsidyCodeText" runat="server" Text="Subsidy Information" CssClass="inputheadertext"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="cblEVSSSubsidyCode" runat="server" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="23vPPV" Value="23vPPV"></asp:ListItem>
                                                                                <asp:ListItem Text="ESIV" Value="ESIV"></asp:ListItem>
                                                                                <asp:ListItem Text="" Value="O"></asp:ListItem>
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEVSSSubsidyCodeOther" runat="server" Width="50px" CssClass="plain"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:TextBox ID="SubsidyCode" runat="server" Style="display: none"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="180px" UseSubmitBehavior="True"
                                            OnClientClick="Set1()" />
                                        &nbsp;
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" Width="70px" OnClientClick="Set2()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="height: 1px; background-color: #999999">
                        </td>
                    </tr>
                    <tr>
                        <td class="amscopyright">
                            Developed by HAITS CS5 Team
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
