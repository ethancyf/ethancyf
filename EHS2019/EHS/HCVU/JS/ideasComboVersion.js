function getIDEASComboVersion(){
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function() {
		if (this.readyState == 4){
			if(this.status == 200){eHSSuccessCallbackFunc(this.responseText.substring(this.responseText.lastIndexOf("Version:")+9).split("<")[0].trim());}
			else if(this.status == 404){eHSFailCallbackFunc();}
		}//else{}
	};
	xhttp.open("GET", "https://127.0.0.1:44928/GetInfo", true);
	xhttp.send();
}//getIDEASComboVersion();