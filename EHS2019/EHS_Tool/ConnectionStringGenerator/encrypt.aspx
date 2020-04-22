<%@ Page Language="vb" Trace="false" AutoEventWireup="false" Inherits="ConnectionStringGenerator.encrypt" Codebehind="encrypt.aspx.vb" %>
<HTML>
	<body>
		<center>
			<h4>數據庫設定加密程式
			</h4>
			<form id="form1" runat="server">
				<div id="div1" style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 15px; BORDER-TOP: black 1px solid; PADDING-LEFT: 15px; PADDING-BOTTOM: 15px; BORDER-LEFT: black 1px solid; WIDTH: 400px; PADDING-TOP: 15px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: beige"
					align="left">用戶名稱:
					<br>
					<asp:textbox id="txtUID" runat="server" Width="500px" Text=""></asp:textbox><br>
					用戶密碼:
					<br>
					<asp:textbox id="txtPWD" runat="server" Width="500px" Text="" TextMode="Password"></asp:textbox><br>
					數據庫伺服器:
					<br>
					<asp:textbox id="txtSvrName" runat="server" Width="500px" Text=""></asp:textbox><br>
					數據庫名稱:
					<br>
					<asp:textbox id="txtDBName" runat="server" Width="500px" Text=""></asp:textbox><br>
					數據庫共用連接數:
					<br>
					<asp:textbox id="txtConnSize" runat="server" Width="500px" Text=""></asp:textbox><br>
					加密鑰匙:
					<br>
					<asp:textbox id="txtKey" runat="server" Width="500px" Text=""></asp:textbox><br>
					<p><asp:button id="btnEncrypt" runat="server" Width="72px" Text="加密"></asp:button>&nbsp;<asp:button id="btnReset" runat="server" Width="72px" Text="取消"></asp:button>
					<p>加密結果:
						<asp:textbox id="txtResult" runat="server" Width="500px" Text="" TextMode="MultiLine"></asp:textbox></p>
					<P>鑰匙加密結果:
						<BR>
						<asp:textbox id="txtEncryptKey" runat="server" Text="" Width="500px" TextMode="MultiLine"></asp:textbox></P>
				</div>
			</form>
		</center>
	</body>
</HTML>
