<%@ Page Language="vb" Trace="false" AutoEventWireup="false" Inherits="ConnectionStringGenerator.encrypt" Codebehind="encrypt.aspx.vb" %>
<HTML>
	<body>
		<center>
			<h4>�ƾڮw�]�w�[�K�{��
			</h4>
			<form id="form1" runat="server">
				<div id="div1" style="BORDER-RIGHT: black 1px solid; PADDING-RIGHT: 15px; BORDER-TOP: black 1px solid; PADDING-LEFT: 15px; PADDING-BOTTOM: 15px; BORDER-LEFT: black 1px solid; WIDTH: 400px; PADDING-TOP: 15px; BORDER-BOTTOM: black 1px solid; BACKGROUND-COLOR: beige"
					align="left">�Τ�W��:
					<br>
					<asp:textbox id="txtUID" runat="server" Width="500px" Text=""></asp:textbox><br>
					�Τ�K�X:
					<br>
					<asp:textbox id="txtPWD" runat="server" Width="500px" Text="" TextMode="Password"></asp:textbox><br>
					�ƾڮw���A��:
					<br>
					<asp:textbox id="txtSvrName" runat="server" Width="500px" Text=""></asp:textbox><br>
					�ƾڮw�W��:
					<br>
					<asp:textbox id="txtDBName" runat="server" Width="500px" Text=""></asp:textbox><br>
					�ƾڮw�@�γs����:
					<br>
					<asp:textbox id="txtConnSize" runat="server" Width="500px" Text=""></asp:textbox><br>
					�[�K�_��:
					<br>
					<asp:textbox id="txtKey" runat="server" Width="500px" Text=""></asp:textbox><br>
					<p><asp:button id="btnEncrypt" runat="server" Width="72px" Text="�[�K"></asp:button>&nbsp;<asp:button id="btnReset" runat="server" Width="72px" Text="����"></asp:button>
					<p>�[�K���G:
						<asp:textbox id="txtResult" runat="server" Width="500px" Text="" TextMode="MultiLine"></asp:textbox></p>
					<P>�_�ͥ[�K���G:
						<BR>
						<asp:textbox id="txtEncryptKey" runat="server" Text="" Width="500px" TextMode="MultiLine"></asp:textbox></P>
				</div>
			</form>
		</center>
	</body>
</HTML>
