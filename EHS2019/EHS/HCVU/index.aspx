<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="index.aspx.vb" Inherits="HCVU.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <base id="basetag" runat="server" />
    <script type="text/javascript" src="JS/Common.js"></script>
    <script type="text/javascript">
    
	    function goNewWin() {
			
			var win;
			var tmp;
			var w = screen.availWidth||screen.width;
			var h = screen.availHeight||screen.height;
			
			var wi = screen.width;
            var he = screen.height;
			
			w = 0;
			h = 0;
			
			var opts;
            													    
			opts = 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0,width=' + wi + ',height=' + he*0.88;
			win = window.open('login.aspx', '_blank', opts);			    
				
			while (!win.open) {}
		
			window.self.opener = window.self;
			window.self.close();
		}
			
    </script>
</head>
<body onload="goNewWin()">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
