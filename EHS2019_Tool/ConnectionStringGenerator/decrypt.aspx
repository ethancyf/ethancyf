<%@ Page Language="vb" Trace="false" AutoEventWireup="false" Inherits="ConnectionStringGenerator.decrypt" Codebehind="decrypt.aspx.vb" %>
<HTML>
	<body>
		<center>
			<h4>數據庫設定解密程式
			</h4>
			<form id="form1" runat="server">
				<div id="div1" style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 15px; BORDER-TOP: black 1px solid; PADDING-LEFT: 15px; PADDING-BOTTOM: 15px; BORDER-LEFT: black 1px solid; WIDTH: 400px; PADDING-TOP: 15px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: beige"
					align="left">
					加密字串:
					<br>
					<asp:textbox id="txtEncryptStr" runat="server" Width="500px" Text=""></asp:textbox><br>
					加密鑰匙:
					<br>
					<asp:textbox id="txtKey" runat="server" Width="500px" Text=""></asp:textbox><br>
					<p><asp:button id="btnDecrypt" runat="server" Width="72px" Text="解密"></asp:button>&nbsp;<asp:button id="btnReset" runat="server" Width="72px" Text="取消"></asp:button>
					<p>解密結果:
						<asp:textbox id="txtResult" runat="server" Width="500px" Text="" TextMode="MultiLine"></asp:textbox></p>
				</div>
			</form>
		</center>
	</body>
</HTML>
