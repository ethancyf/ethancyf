function GetDataFromXML(xmlFile, lang, type)
{
    var xmlDoc=null;
    
    if (window.ActiveXObject)
    {// code for IE
        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        }
        else // code for IE5 and IE6
        {
            xhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
    }
    else if (document.implementation.createDocument)
    {// code for Mozilla, Firefox, Opera, etc.
        xhttp = new XMLHttpRequest();
    }
    else
    {
        alert('Your browser cannot handle this script');
        return;
    }

    // Load xml file
    if (xhttp != null) {
        xhttp.open("GET", xmlFile, false);
        xhttp.send();
        xmlDoc = xhttp.responseXML;
    }

    if (xmlDoc!=null) 
    {
        var y=xmlDoc.getElementsByTagName("Schedule");
              
        var current_dtm = new Date();
        
        var blnU = true;
        var blnR = true;
        var array_final = new Array;
        
        var index = 0;
        
        if (type == 'R')
        {
            //Regular System Maint
            for (var i=0;i<y.length;i++)
            {
                var schedule_dtm = new Date(y[i].getElementsByTagName("expiry_dtm")[0].childNodes[0].nodeValue);

                //if (blnR)
                //{
                    if (current_dtm < schedule_dtm)
                    {                    
                        //blnR = false;
                        if (lang == "ZH")
                        {
                            var array_r = new Array(2);
                            array_r[2] = y[i].getElementsByTagName("ZH_Date")[0].childNodes[0].nodeValue;
                            array_r[1] = y[i].getElementsByTagName("ZH_Time")[0].childNodes[0].nodeValue;
                            array_r[0] = y[i].getElementsByTagName("s_id")[0].childNodes[0].nodeValue;
                        }
                        else
                        {
                            var array_r = new Array(2);
                            array_r[2] = y[i].getElementsByTagName("EN_Date")[0].childNodes[0].nodeValue;
                            array_r[1] = y[i].getElementsByTagName("EN_Time")[0].childNodes[0].nodeValue;
                            array_r[0] = y[i].getElementsByTagName("s_id")[0].childNodes[0].nodeValue;
                        }
                        
                        array_final[index] = array_r;     
                        index = index + 1;                         
                    }  
                //}                   
                
            }
            
            for (var j=0; j < array_final.length;j ++)
            {
               
                document.write("<tr>");
                document.write("<td style='background-color: #ffffff;width:250px' align='center'>");
                document.write(array_final[j][2]);
                document.write("</td>");
                document.write("<td style='background-color: #ffffff' align='center'>");
                document.write(array_final[j][1]);
                document.write("</td>");
                document.write("</tr>");
            }
                
        }
        else
        {
                //Ungert System Maint
            for (var i=0;i<y.length;i++)
            {
                var schedule_dtm = new Date(y[i].getElementsByTagName("expiry_dtm")[0].childNodes[0].nodeValue);

                if (blnU)
                {
                    if (current_dtm < schedule_dtm)
                    {
                        //blnU = false;
                        if (lang == "ZH")
                        {
                            var array_u = new Array(2);
                            array_u[2] = y[i].getElementsByTagName("ZH_Date")[0].childNodes[0].nodeValue;
                            array_u[1] = y[i].getElementsByTagName("ZH_Time")[0].childNodes[0].nodeValue;
                            array_u[0] = y[i].getElementsByTagName("s_id")[0].childNodes[0].nodeValue;
                        }
                        else
                        {
                            var array_u = new Array(2);
                            array_u[2] = y[i].getElementsByTagName("EN_Date")[0].childNodes[0].nodeValue;
                            array_u[1] = y[i].getElementsByTagName("EN_Time")[0].childNodes[0].nodeValue;
                            array_u[0] = y[i].getElementsByTagName("s_id")[0].childNodes[0].nodeValue;
                        }
                        
                        array_final[index] = array_u;
                        index = index + 1;                    
                    }  
                }                   
                
            }
            array_final.sort();        
            if (array_final.length > 0)
            {
                if (lang == "ZH")
                {
                    document.write("<div class='headingText'>緊急系統維護</div>");           
                    document.write("<table style='background-color: #87ceeb' border='0' cellpadding='1' cellspacing='1'width='60%'>");
                    document.write("<tr><td style='font-weight: bold; color: #ffffff;width:250px' align='center'>日期</td><td style='font-weight: bold; color: #ffffff' align='center'>時間</td></tr>");
                }
                else
                {
                    document.write("<div class='headingText'>Ad hoc System Maintenance Schedule</div>");           
                    document.write("<table style='background-color: #87ceeb' border='0' cellpadding='1' cellspacing='1'width='60%'>");
                    document.write("<tr><td style='font-weight: bold; color: #ffffff;width:250px' align='center'>Date</td><td style='font-weight: bold; color: #ffffff' align='center'>Time</td></tr>");
                }
                    
                for (var j=0; j < array_final.length;j ++)
                {
                   
                    document.write("<tr>");
                    document.write("<td style='background-color: #ffffff' align='center'>");
                    document.write(array_final[j][2]);
                    document.write("</td>");
                    document.write("<td style='background-color: #ffffff' align='center'>");
                    document.write(array_final[j][1]);
                    document.write("</td>");
                    document.write("</tr>");
                }
                
                document.write("</table><br/>")

            }
        }
        
    }

}