


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>醫療院所接種疫苗查詢</title>
    <meta http-equiv="Content-Type" content="text/html; charset=big5" />
    <link href="https://urm.cdc.gov.tw/css/images/css_s.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        .style1
        {
            font-size: medium;
            color: Red;
            height: 30px;
            font-weight: 700;
            vertical-align: middle;
            text-align: center;
        }
        .style3
        {
            font-family: "華康中圓體";
            font-size: 18px;
            color: #993300;
            font-weight: bold;
            width: 83px;
        }
        .style6
        {
            height: 18px;
            width: 810px;
        }
        .style8
        {
            font-size: medium;
            color: Red;
            height: 30px;
            font-weight: 700;
            vertical-align: middle;
            text-align: center;
            width: 810px;
        }
        .style9
        {
            width: 810px;
        }
        .style10
        {
            width: 810px;
            height: 21px;
        }
    </style>
</head>
<script type="text/javascript" language="javascript">
    var installErrorGPKIMsg = "您的電腦無自然人憑證卡元件，請參考卡片元件下載說明。";
    var installErrorHCAMsg = "您的電腦無醫事人員卡及健保卡元件，請參考卡片元件下載說明。";
    var installErrorHCACSMsg = "您的電腦無醫事人員卡，請參考卡片元件下載說明。";
    var authProcessingMsg = "<font color=#434343><strong>認證中，請稍候......</strong></font>";

    function installErrorGPKI() {
        alert(installErrorGPKIMsg);
        //document.getElementById("showmessage").value = installErrorGPKIMsg;
        //document.all.showmessage.innerHTML = installErrorGPKIMsg;
        //document.all.errorDisplay.innerHTML = installErrorGPKIMsg;
    }
    function installErrorHCA() {
        alert(installErrorHCAMsg);
        //document.all.showmessage.innerHTML = installErrorHCAMsg;
        //document.getElementById("showmessage").value = installErrorHCAMsg;
        //document.all.errorDisplay.innerHTML = installErrorHCAMsg;
    }
    function installErrorHCACS() {
        alert(installErrorHCACSMsg);
        //document.all.errorDisplay.innerHTML = installErrorHCACSMsg;
    }

    function checkField(obj) {
        var temp = obj;
        var chksapos = false;
        for (var i = 0; i < temp.length; i++) {
            begin = temp.indexOf("'");
            if (begin != -1) {
                if (chksapos && begin > 0) {
                    return true;
                } else {
                    chksapos = true;
                    temp = temp.substring(begin + 1, temp.length);
                }
            }
        }
    }

    function checkATL() {
        document.all.showmessage.innerHTML = authProcessingMsg;
        //document.all.errorDisplay.innerHTML = authProcessingMsg;

        //Client端自然人憑證卡、及疾管局員工卡元件
        var sGPKICode =
		'<object' +
		' classid="CLSID:3C232DA1-E9AC-4C74-A792-2A686F7315EE"' +
		' codebase="https://urmsso.cdc.gov.tw/changing/FSGPKICryptATL.cab#version=2,1,13,302"' +
        //'codebase="FSGPKICryptATL.cab#version=2,1,13,122"' +
		' width=0' +
		' height=0' +
		' onerror="installErrorGPKI()"' +
		' id="fsgpkicrypt"' +
		'>' +
		'</object>';

        //Client端醫事人員卡元件
        var sHCACode =
		'<object' +
		' classid="clsid:BB76BF14-7D3D-48CA-8824-42CA1A8FB040"' +
		' codebase="https://urmsso.cdc.gov.tw/changing/FSHCAATL.cab#version=2,1,10,1228"' +
        //' codebase="FSHCAATL.cab#version=2,1,10,1228"' +
		' width=0' +
		' height=0' +
		' onerror="installErrorHCA()"' +
		' id="fshca"' +
		'>' +
		'</object>';

        var sHCACSCode =
		'<OBJECT' +
		' classid="clsid:9E031C0E-1474-4A46-8104-A881DFDB0F9A"' +
        ' codebase="https://urmsso.cdc.gov.tw/changing/CGHCACSAPIATL.cab#version=1,1,12,1102"' +
        //' codebase="CGHCACSAPIATL.cab#version=1,1,12,1102"' +
		' width=0' +
		' height=0' +
		' onerror="installErrorHCACS()"' +
		' id="fshcacs"' +
		'>' +
		'</OBJECT>';

        var form = document.getElementById("fm");
        if (form.cardType[1].checked) { //自然人憑證卡
            document.all.installGPKI.innerHTML = sGPKICode;
            //document.all.showmessage.innerHTML = sGPKICode;
        } else if (form.cardType[0].checked) { //醫事人員卡
            document.all.installHCA.innerHTML = sHCACode;
            //document.all.showmessage.innerHTML = sHCACode;
        } else if (form.cardType[2].checked) { //健保讀卡機
            document.all.installHCACS.innerHTML = sHCACSCode;
            //document.all.showmessage.innerHTML = sHCACode;
            if (form.txtPwd2.value == "") {
                alert("請輸入PIN CODE");
                form.txtPwd2.focus();
                form.txtPwd2.select();
                document.all.showmessage.innerHTML = "";
                return;
            }
            else {
                form.pass2.value = form.txtPwd2.value;
            }
        }

        setTimeout('signData()', 10);
    }
</script>
<script type="text/javascript" language="javascript">
    var FS_KU_DIGITAL_SIGNATURE = 0x0080;
    var FS_FLAG_SUBJECT_COMMON_NAME = 0x00010000;
    var iGPKICount = 0; //避免第一次下載元件出現installErrorGPKI()
    var iHCACount = 0; //避免第一次下載元件出現installErrorHCA()
    var iHCACSCount = 0; //避免第一次下載元件出現installErrorHCACS()

    function el(id) {
        if (document.getElementById) {
            return document.getElementById(id);
        }
        return false;
    }

    function transPassword(pass) {
        var temp = "*";
        var pass2 = "";

        for (var i = 0; i < pass.length; i++) {
            pass2 += temp;
        }
        return pass2;
    }

    function signData() {
        document.all.showmessage.innerHTML = "";
        var form = document.getElementById("fm");

        if (form.cardType[1].checked) {
            MOICAsign(form);
        } else if (form.cardType[0].checked) {
            HPCsign(form);
        } else if (form.cardType[2].checked) {
            HCACSsign(form);
        }
    }

    function MOICAsign(form) {
        try {
            var flags = FS_KU_DIGITAL_SIGNATURE;
            var certs = fsgpkicrypt.FSGPKI_EnumCerts(flags);
            if (certs == null) {
                alert(errorMsg(fsgpkicrypt.get_lastError()));
                //alert(fsgpkicrypt.get_lastError());
                return;
            }

            var x509certs = certs.toArray();
            var strX509Cert = x509certs[0];

            //身分證後4碼
            var tailOfCitizenID = fsgpkicrypt.GPKI_GetTailOfCitizenID(strX509Cert, 0);

            if (fsgpkicrypt.get_lastError() != 0) {
                el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + "身分證後4碼取得失敗, " + fsgpkicrypt.get_lastError() + "</font>";
                return;
            }

            form.id.value = tailOfCitizenID;

            el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + transPassword(tailOfCitizenID) + "</font>";
            //姓名
            //憑證取得主旨中的 CN
            var cn = fsgpkicrypt.FSCAPICertGetSubject(strX509Cert, FS_FLAG_SUBJECT_COMMON_NAME);
            if (fsgpkicrypt.get_lastError() != 0) {
                form.name.value = "";
                el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + "姓名取得失敗, " + fsgpkicrypt.get_lastError() + "</font>";
                return;
            } else {
                form.name.value = cn;
                el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + cn + "</font>";
            }

            //簽章
            var pincode = "";
            //用 CG_ALGOR_SHA256 的演算法
            var iHashFlag = 0x04;

            //var SignData = fsgpkicrypt.FSGPKI_SignData( "", form.authData.value , 0 );
            var SignData = fsgpkicrypt.GPKI_SignData("", form.authData.value, 0, iHashFlag);

            if (fsgpkicrypt.get_lastError() == 0) {
                form.signature.value = SignData;
                //alert("MOICA Sign Success=" + SignData);
                form.submit();
            } else {
                form.signature.value = "";
                alert("(" + fsgpkicrypt.get_lastError() + ") " + errorMsg(fsgpkicrypt.get_lastError()));
                return;
            }
        } catch (e) {
            if (iGPKICount > 1) {
                installErrorGPKI();
                iGPKICount = 0;
            } else if (iGPKICount == 0) {
                document.getElementById("showmessage").value = "請在安裝完自然人憑證卡元件之後，重新按下登入鈕";
                iGPKICount++;
            }
        }
    }
    /* 
    * fshca return define
    */

    var FSCARD_RTN_SUCCESS = 0;
    var FSCARD_RTN_CONNECT_FAIL = 3001;
    var FSCARD_RTN_SELECT_APPLET_FAIL = 3002;
    var FSCARD_RTN_ESTABLISH_CONTEXT_FAIL = 3003;
    var FSCARD_RTN_CARD_ABSENT = 3005;
    var FSCARD_RTN_TRANSMIT_ERROR = 3006;
    var FSCARD_RTN_GET_DATA_ERROR = 3007;
    var FSCARD_RTN_LOGIN_FAIL = 3008;
    var FSCARD_RTN_READERS_BUFFER_FAIL = 3009;
    var FSCARD_RTN_GET_READERS_FAIL = 3010;
    var FSCARD_RTN_NO_READER = 3011;
    var FSCARD_RTN_MEMALLOC_ERROR = 3012;
    var FSCARD_RTN_LIST_READERS_ERROR = 3013;
    var FSCARD_RTN_CHAR2WCHAR_ERROR = 3014;
    var FSCARD_RTN_WCHAR2CHAR_ERROR = 3015;
    var FSCARD_RTN_INVALID_PARAM = 3016;
    var FSCARD_RTN_LIB_EXPIRE = 3017;
    var FSCARD_RTN_GEN_PKCS7_FAIL = 3018;
    var FSCARD_RTN_DATA_HASH_ERROR = 3019;
    var FSCARD_RTN_PIN_LOCK = 3021;
    var FSCARD_RTN_UNKNOWN_ERROR = 3999;
    /*
    GPKI return define	
    */
    var FSGPKI_RTN_CONNECT_FAIL = 9056;
    var FSGPKI_RTN_ESTABLISH_CONTEXT_FAIL = 9057;
    var FSGPKI_RTN_LOGIN_CANCEL = 5070;
    var FSGPKI_RTN_PIN_INCORRECT = 9039;
    var FSGPKI_RTN_PIN_LOCK = 9043;

    function errorMsg(code) {
        switch (code) {
            case FSCARD_RTN_SUCCESS:
                return "執行成功.";
                break;
            case FSCARD_RTN_CONNECT_FAIL:
            case FSGPKI_RTN_CONNECT_FAIL:
                return "請確定讀卡機是否安裝與正確插入卡片"; //連結卡片失敗
                break;
            case FSCARD_RTN_SELECT_APPLET_FAIL:
                return "請確定插入的是醫事人員卡"; //非指定之卡片(HC/HPC)，請確定卡片
                break;
            case FSCARD_RTN_ESTABLISH_CONTEXT_FAIL:
                return "連結卡片失敗";
                break;
            case FSCARD_RTN_CARD_ABSENT:
                return "卡片不存在";
                break;
            case FSCARD_RTN_LIB_EXPIRE:
                return "函式庫已過期";
                break;
            case FSCARD_RTN_LOGIN_FAIL:
                return "登入卡片失敗";
                break;
            case FSCARD_RTN_INVALID_PARAM:
                return "無效的參數";
                break;
            case FSGPKI_RTN_ESTABLISH_CONTEXT_FAIL:
                return "請確定插入的是自然人憑證卡";
                break;
            case FSGPKI_RTN_LOGIN_CANCEL:
                return "使用者取消登入卡片";
                break;
            case FSGPKI_RTN_PIN_INCORRECT:
            case FSCARD_RTN_LOGIN_FAIL:
                return "卡片密碼不對";
                break;
            case FSGPKI_RTN_PIN_LOCK:
            case FSCARD_RTN_PIN_LOCK:
                return "卡片鎖卡";
                break;
            default:
                return "";
        }
    }

    function HPCsign(form) {
        try {
            var allReader = fshca.FSHCA_GetReaderNames(0);
            var rtns;

            if (allReader == null) {
                alert('找不到「一般晶片讀卡機」!');
                return;
            } else {
                var readers = allReader.toArray();
                for (var i = 0; i < readers.length; i++) {
                    //alert("readers1[i]="+readers[i]);
                    rtns = fshca.FSHCA_HPCBasicDataByReader(readers[i], 0);
                    if (fshca.FSHCA_GetErrorCode() == FSCARD_RTN_SUCCESS) {
                        //alert("FSHCA_HPCBasicData run success");
                        infos = rtns.toArray();

                        form.id.value = infos[4];
                        el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + transPassword(infos[4]) + "</font>";
                        form.name.value = infos[0];
                        el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + infos[0] + "</font>";
                        //簽章
                        //alert("readers2[i]="+readers[i]);
                        var SignData = fshca.FSHCA_SignByReader(readers[i], "", form.authData.value, 0);
                        if (fshca.FSHCA_GetErrorCode() == FSCARD_RTN_SUCCESS) {
                            form.signature.value = SignData;
                            //alert("HPC Sign Success");
                            //window.open("LoginSuccess.aspx?sPID=" + form.id.value + "&sCardType=2" + "&sName=" + form.name.value);
                            form.submit();
                            break;
                        } else {
                            form.signature.value = "";
                            alert("(" + fshca.FSHCA_GetErrorCode() + ") " + errorMsg(fshca.FSHCA_GetErrorCode()));
                            return;
                        }
                    } else if (fshca.FSHCA_GetErrorCode() != 0 && i + 1 == readers.length) {
                        alert("(" + fshca.FSHCA_GetErrorCode() + ")" + errorMsg(fshca.FSHCA_GetErrorCode()));
                        return;
                    }
                }
            }
        } catch (e) {
            if (iHCACount > 1) {
                installErrorHCA();
                iHCACount = 0;
            } else if (iHCACount == 0) {
                document.all.showmessage.innerHTML = "請在安裝健保卡元件之後，重新按下登入鈕";
                iHCACount++;
            }
        }
    }

    function HCread(form) {
        try {
            var allReader = fshca.FSHCA_GetReaderNames(0);
            var rtn;

            if (allReader == null) {
                alert("找不到「一般晶片讀卡機」!");
                return;
            } else {
                var readers = allReader.toArray();
                for (var i = 0; i < readers.length; i++) {
                    rtns = fshca.FSHCA_GetHCBasicDataByReader(readers[i], 0);
                    if (fshca.FSHCA_GetErrorCode() == FSCARD_RTN_SUCCESS) {
                        //alert("FSHCA_HCBasicData run success");
                        infos = rtns.toArray();
                        form.id.value = infos[2];
                        el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + transPassword(infos[2]) + "</font>";
                        form.name.value = infos[1];
                        el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + infos[1] + "</font>";
                        form.submit();
                        break;
                    } else if (fshca.FSHCA_GetErrorCode() != 0 && i + 1 == readers.length) {
                        alert("(" + fshca.FSHCA_GetErrorCode() + ")" + errorMsg(fshca.FSHCA_GetErrorCode()));
                        return;
                    }
                }
            }
        } catch (e) {
            if (iHCACount > 1) {
                installErrorHCA();
                iHCACount = 0;
            } else if (iHCACount == 0) {
                //document.all.errorDisplay.innerHTML = "請在安裝完醫事人員卡及健保卡元件之後，重新按下登入鈕";
                //document.getElementById("showmessage").value = "請在安裝完醫事人員卡及健保卡元件之後，重新按下登入鈕";
                document.all.showmessage.innerHTML = "請在安裝完醫事人員卡及健保卡元件之後，重新按下登入鈕";
                iHCACount++;
            }
        }
    }

    function HCACSsign(form) {
        try {

            var rtns = fshcacs.CGHCACS_GetHPCBasicData(0, 0);

            if (fshcacs.CGHCACS_GetErrorCode() == FSCARD_RTN_SUCCESS) {
                //alert("CGHCA_HPCBasicData run success");
                infos = rtns.toArray();

                form.id.value = infos[4];
                el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + transPassword(infos[4]) + "</font>";
                form.name.value = infos[0];
                el("showmessage").innerHTML = "<font size=\"4\" color=\"#CFA072\">" + infos[0] + "</font>";
                form.submit();

                //簽章					
                var SignData = fshcacs.CGHCACS_Sign(0, form.pass2.value, form.authData.value, 0, 0x02);
                //var SignData = fshcacs.CGHCACS_Sign(0, "", form.authData.value, 0, 0x02);

                if (fshcacs.CGHCACS_GetErrorCode() == FSCARD_RTN_SUCCESS) {
                    form.signature.value = SignData;
                    //alert("HPC Sign Success");
                    form.submit();
                    //break;
                } else {
                    form.signature.value = "";
                    alert("(簽章驗證錯誤" + fshcacs.CGHCACS_GetErrorCode() + ") " + errorMsg(fshcacs.CGHCACS_GetErrorCode()));
                    return;
                }


            } else {
                alert("(找不到醫事讀卡機或是醫師憑證未插入) " + errorMsg(fshcacs.CGHCACS_GetErrorCode()));
                return;
            }

        } catch (e) {
            alert(e.description);
            alert("未安裝元件或是醫事讀卡機(reader.dll)未正確安裝");

            /*
		
            if(iHCACount > 1) {
            installErrorHCA();
            iHCACount = 0;
            }else if(iHCACount == 0) {
            document.all.errorDisplay.innerHTML = "請在安裝完醫事人員卡及健保卡元件之後，重新按下登入鈕";
            iHCACount++;
            }*/
        }
    }

    function changeCardType(selected) {

        try {
            if (selected == "MOICA") {
                var sGPKICode =
		                        '<object' +
		                        ' classid="CLSID:3C232DA1-E9AC-4C74-A792-2A686F7315EE"' +
		                        ' codebase="https://urmsso.cdc.gov.tw/changing/FSGPKICryptATL.cab#version=2,1,13,302"' +
                //'codebase="FSGPKICryptATL.cab#version=2,1,13,122"' +
		                        ' width=0' +
		                        ' height=0' +
		                        ' onerror="installErrorGPKI()"' +
		                        ' id="fsgpkicrypt"' +
		                        '>' +
		                        '</object>';
                //el('showmessage').innerHTML = "姓　　　名";
                //el('showmessage').innerHTML = "身分證號碼";
                //document.all.imgPwd2.src = "images_N/name.jpg"
                //document.all.installGPKI.innerHTML = sGPKICode;	
                //document.all.txtPwd2.readOnly = true;	
                //document.all.txtPwd2.style.border = '0px';	
                //document.all.txtPwd2.value = "";
                //		
                //document.all.IDOrEmpID.readOnly = true;
                //document.all.IDOrEmpID.style.border = '0px';
                //document.all.IDOrEmpID.value = "";

            } else if (selected == "HPC") {
                var sHCACode =
		                       '<object' +
		                       ' classid="clsid:BB76BF14-7D3D-48CA-8824-42CA1A8FB040"' +
                //' codebase="https://urmsso.cdc.gov.tw/changing/FSHCAATL.cab#version=2,2,12,1202"' +
		                       ' codebase="FSHCAATL.cab#version=2,2,12,1202"' +
		                       ' width=0' +
		                       ' height=0' +
		                       ' onerror="installErrorHCA()"' +
		                       ' id="fshca"' +
		                       '>' +
		                       '</object>';
                //		el('pwdOrName').innerHTML = "姓　　　名";
                //		el('IDOrEmpID').innerHTML = "身分證號碼";
                //		//document.all.imgPwd2.src = "images_N/name.jpg"
                //		document.all.installHCA.innerHTML = sHCACode;	
                //		document.all.txtPwd2.readOnly = true;	
                //		document.all.txtPwd2.style.border = '0px';
                //		document.all.txtPwd2.value = "";
                //		
                //		document.all.IDOrEmpID.readOnly = true;
                //		document.all.IDOrEmpID.style.border = '0px';
                //		document.all.IDOrEmpID.value = "";

            } else if (selected == "HCACS") {
                var sHCACSCode =
		                        '<OBJECT' +
		                        ' classid="clsid:9E031C0E-1474-4A46-8104-A881DFDB0F9A"' +
                //' codebase="https://urmsso.cdc.gov.tw/changing/FSHCAATL.cab#version=2,1,10,1228"' +
		                        ' codebase="CGHCACSAPIATL.cab#version=1,1,12,1102"' +
		                        ' width=0' +
		                        ' height=0' +
		                        ' onerror="installErrorHCACS()"' +
		                        ' id="fshcacs"' +
		                        '>' +
		                        '</OBJECT>';

                document.all.txtPwd2.focus();


                //form.txtPwd2.select();
                //                el('pwdOrName').innerHTML = "PIN CODE";
                //                el('IDOrEmpID').innerHTML = "身分證號碼";
                //                //document.all.imgPwd2.src = "images_N/name.jpg"
                //                document.all.installHCA.innerHTML = sHCACSCode;
                //                //document.all.pwdPassword.readOnly = true;	
                //                //document.all.pwdPassword.style.border = '0px';	
                //                document.all.txtPwd2.readOnly = false;
                //                document.all.txtPwd2.style.border = 'thin solid';
                //                document.all.txtPwd2.value = "";
                //                document.all.txtPwd2.focus();

                //                document.all.IDOrEmpID.readOnly = true;
                //                document.all.IDOrEmpID.style.border = '0px';
                //                document.all.IDOrEmpID.value = "";
            }

        } catch (e) {
            alert("網頁反應不正常，請點選瀏覽器上方：重新整理選項");
        }
    }

</script>
<body style="margin: 0px; background-image: url('IMG/background.png');">
    <form name="fm" method="post" action="Login.aspx" id="fm">
<div>
<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUJNDA4NDM3NzQzZGTq8yz5C99NnDqNJq2bn6qiRpWsOznav1YdjxtexniVSg==" />
</div>

<div>

	<input type="hidden" name="__VIEWSTATEGENERATOR" id="__VIEWSTATEGENERATOR" value="C2EE9ABB" />
</div>
    <table border="0" align="center" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td align="center" colspan="3" style='height: 50px'>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <img src="IMG/CDCLogo.png" alt="CDCLogo" style="width: 200px; height: 200px;" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <img src="IMG/SystemTitle.png" alt="醫療院所預防接種資料查詢系統" style="width: 827px" />
            </td>
        </tr>
        <tr>
            <table border="0" cellspacing="0" cellpadding="0" align="center" class="style13">
                <tr>
                    <td align="right" style="font-family: 微軟正黑體; font-size: large;" dir="ltr" rowspan="2"
                        class="style12">
                        <span>選擇卡片種類：</span>
                    </td>
                    <td align="left" colspan='2' style="font-family: 微軟正黑體; font-size: medium">
                        <input type="radio" name="cardType" id="Radio1" onclick="changeCardType(this.value)"
                            value="HPC" checked="checked" />
                        <span>醫事憑證卡</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input type="radio" name="cardType" id="Radio2" onclick="changeCardType(this.value)"
                            value="MOICA" />
                        <span>自然人憑證</span>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan='2' class="style14" style="font-family: 微軟正黑體; font-size: medium">
                   <input type="radio" name="cardType" id="cardType" onclick="changeCardType(this.value)"
                            value="HCACS" />
                        <span>醫事憑證卡(健保專用雙卡讀卡機),醫事憑證卡PIN碼</span>
                        <input name="txtPwd2" type="password" class="minput" id="Password1" />
                    </td>
                </tr>
                <tr>
                    <td id="pwdOrName" class="style11">
                    </td>
                </tr>
                <tr>
                    <td height="20" colspan="3" align="center">
                        <input type="button" value="登入系統" class="bn_btn" onclick="checkATL();" />&nbsp;&nbsp;
                        <input type="button" value="帳號申請" class="bn_btn" onclick="window.open('UserApply/UserApply.aspx')" />
                        
                    </td>
                </tr>
            </table>
        </tr>
        <tr>
            <td colspan="3" class="style1">
                <table align="center" class="style15">
                    <tr>
                        <td class="style8">
                            <div id="showmessage">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="style6" align="left" style="border-spacing: 1px">
                            <font size="2" color="darkslategray" style="text-align: left; list-style-type: decimal;
                                border-spacing: 1px;">相關連結:&nbsp;&nbsp;&nbsp; <a href="https://urmsso.cdc.gov.tw/Changing/"
                                    target="_new">系統環境偵測網頁(卡片元件下載請至該網頁裡面4.進行下載)</a>、<a href="http://moica.nat.gov.tw/other/index.html"
                                        target="_new">卡片鎖卡解碼及修改PIN碼說明</a>、</br><a href="http://help.changingtec.com/HIS/?m=errdef&a=queryform"
                                            target="_new">卡片登入錯誤代碼查詢</a>、<a href="http://www.cdc.gov.tw/professional/page.aspx?treeid=4c19a0252bbef869&nowtreeid=F40E745FD5AB0F3E"
                                            target="_new">網路ip申請表下載</a></font>
                        </td>
                    </tr>
		   
		     <tr>
                        <td align="left" class="style9">
 				<font size="2" color="green" style="text-align: left;">帳號審核(需工作2~3天)，如沒審核通過:&nbsp; 請洽電話:(02)23959825#3684;
                                服務時間：週一到週五 (早上09:00-12:00)(下午13:30:18:00)</font>
			</td>
		    </tr>
                    <tr>
                        <td align="left" class="style9">
                            <font size="2" color="green" style="text-align: left;">系統操作問題:&nbsp; 請洽 客服電話:(02)2395-9825#3683&nbsp;
                                服務時間：週一到週五 09:00-18:00</font>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style9">
                            <font size="2" color="darkslategray" style="text-align: left;">卡片元件安裝問題: &nbsp;請洽(03)563-0200
                                分機 8 (全景客服) 或參見 <a href="http://helpdesk.changingtec.com/helpdesk/" target="_new">客服中心網站</a>&nbsp;
                                <a href="mailto:E-mail：help@changingtec.com">E-mail:&nbsp;help@changingtec.com</a></font>
                        </td>
                    </tr>

                    <tr>
                        <td class="style9">
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center" class="style9">
                            <font size="2" color="darkslategray">行政院衛生福利部 疾病管制署 Copyright All right reserved. 2013<br />
                                本網站以1024*768 設計,建議用Internet Explorer 6 以上版本瀏覽<br />
                            </font>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="style9">
                             
                            <br/>
                            
                        </td>
                    </tr>
                </table>
            </td>
            <td id="strMessage">
            </td>

        </tr>

    </table>
    <input name="authData" type="hidden" id="authData" value="6647,1578964461823,1266984590" />
    <input type="hidden" name="signature" value="" />
    <input type="hidden" name="id" value="" />
    <input type="hidden" name="empID" value="" />
    <input type="hidden" name="name" value="" />
    <input type="hidden" name="pass2" value="" />
    <input name="op" type="hidden" id="op" value="verify" />
    </form>
</body>
</html>
<div id="installGPKI">
</div>
<div id="installHCA">
</div>
<div id="installHCACS">
</div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
