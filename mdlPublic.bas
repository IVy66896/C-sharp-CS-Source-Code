Attribute VB_Name = "mdlPublic"
Option Explicit

Public Const VERSION = "V3.1.18b版" '版本
Public Const RELEASE_DATE = "101/11/20" '發行日期

Public Const ISDEBUG = True ' 設定debug
Public Const CLIENT_DEBUG = True ' 設定debug

Public Const SignExpireTime = 300 ' 簽到退從開始到結束可以多久，in秒，預設是 5分鐘 = 300(秒)。
Public Const WSTimeOut = 900000 ' Web Service Timeout

Public Const LOCAL_DB_PASSWORD = "20n6JY81xY9QUvs" ' local access database password

Public Const AESKEY = "GIAgVDmYkSQ3ACm" ' key

Public Const g_SgnSignLevel = SL_LOW ' 2011_08_XX 簽名檔檢查level

Public g_MDB_PATH As String   ' 資料mdb路徑，因應clickonce..

Public g_LOCAL_DB_CONNECTION_STRING As String   ' local db connection string

Public g_isLegalSystem As Boolean
Public g_isLegalUser As Boolean

Public g_isUpLoadingSgnFiles As Boolean
Public g_isCheckSgnSignLevel As Boolean ' 2011_08_XX 新增是否檢查簽名檔level
Public g_SgnDirPath As String ' 2011_08_XX 簽名檔dir path


Public g_intDelayTime As Integer '停筆延遲時間

Public g_strAbaNo As String '屠宰場代號
Public g_strAbaName As String '屠宰場名稱

Public g_strSysSerialNo As String '系統驗証序號

Public g_strUpdateTime As String '使用者清單更新日期

Public g_strUserID As String '登入的OA使用者ID
Public g_strEncryptedUserID As String '登入的OA使用者ID, 加密過了
Public g_strDoctorNo As String '登入的獸醫師證號
Public g_strDoctorName As String '登入的獸醫師名字


Public g_WSurl As String 'server web service
Public g_ThisNTPTime As String    ' 這次校時後的時間，沒有的話代表沒校時到    2010_03_09
Public g_NTPSrvIPBeforAdjust As String    ' 這次校時前的Host    2010_07_01
Public g_NTPSrvIPAfterAdjust As String    ' 這次校時後的Host    2010_07_01

'''''''''''''''''''''''''''''''''''''''''''''
'   2010_07_01 新增，為了看這一次寫了什麼進資料庫, 除了這一筆，其他的都視為離線。
'''''''''''''''''''''''''''''''''''''''''''''
Public g_ThisSaveSignDate As String
Public g_ThisSaveSignTime As String
Public g_ThisSaveLogType As String
Public g_ThisSaveSignFile As String
Public g_ThisSavePhotoFile As String
Public g_ThisSaveVerifiedNo As String

'''''''''''''''''''''''''''''''''''''''''''''
'   2010_07_23 新增 持續監控校時時間
'''''''''''''''''''''''''''''''''''''''''''''
Public g_MonitorSystemTime As Date              ' 紀錄系統時間，理論上不會後退的。
Public g_MonitorSysTimeDesc As String           ' 如果有問題的點，會寫成一個字串，跟著一筆資料加密後上傳，再清掉。
Public g_MonitorSysTimeDescForClient As String  ' 紀錄有問題的點，給client參考用的。



Public g_DBUpdate As Boolean 'Access裡的Doctor schema是否有變動

Public g_OffLineMode As Boolean ' 因應要寫入是不是使用離線模式  2010_03_09

Public Sub Delay(sglPauseTime As Single)
    '目的：造成延遲時間
    '輸入：延遲時間(秒)
    
    Dim sglStart As Single
    Dim sglEnd As Single
    
    sglStart = Timer
    sglEnd = Timer
    Do While sglEnd < sglStart + sglPauseTime
        DoEvents
        sglEnd = Timer
        If sglEnd < sglStart Then sglEnd = sglEnd + 24! * 60! * 60!
    Loop
End Sub

Public Function getVerifiedNo() As String
    '目的：產生簽名檔驗証號碼
    '輸出：0001~9999
    
    Randomize
    getVerifiedNo = Format(Int(9999 * Rnd + 1), "0000")
End Function

Public Function CDate2Date(strDate As String) As Date
    '目的：將民國日期(YY/MM/DD)轉換為西元日期
    '輸入：民國日期(YY/MM/DD)
    '輸出：西元日期
    
    Dim strTemp() As String
    
    strTemp = Split(strDate, "/")
    
    CDate2Date = DateSerial(Val(strTemp(0)) + 1911, Val(strTemp(1)), Val(strTemp(2)))
End Function

Public Function C2CDate(datDate As Date) As String
    '目的：將西元日期轉換為民國日期(YY/MM/DD)
    '輸入：西元日期
    '輸出：民國日期(YY/MM/DD)
    
    Dim strCDate As String
    
    strCDate = ""
    strCDate = strCDate & Format(Year(datDate) - 1911, "00") & "/"
    strCDate = strCDate & Format(Month(datDate), "00") & "/"
    strCDate = strCDate & Format(Day(datDate), "00")
    
    C2CDate = strCDate
End Function

Public Function getSignPhotoSerialNo(strDate As String, strLogType As String, strTime As String, strDoctorNo As String) As String
    '目的：依簽到退日期、簽到退別、簽到退時間及檢查人員代號產生簽名檔及照片檔編號
    '輸入：簽到退日期(YY/MM/DD)、簽到退別、簽到退時間(HH/MM/SS)及檢查人員代號
    '輸出：簽名檔及照片檔編號
    
    Dim strSignPhotoSerialNo As String
    
    strSignPhotoSerialNo = ""
    strSignPhotoSerialNo = strSignPhotoSerialNo & Trim(Replace(strDate, "/", ""))
    strSignPhotoSerialNo = strSignPhotoSerialNo & Trim(strLogType)
    strSignPhotoSerialNo = strSignPhotoSerialNo & Trim(Replace(strTime, ":", ""))
    strSignPhotoSerialNo = strSignPhotoSerialNo & Trim(strDoctorNo)
        
    getSignPhotoSerialNo = strSignPhotoSerialNo
End Function

Public Function getSysSerialNo(strGeneralSerialNo As String) As String
    '目的：依硬碟physical序號或網卡MAC Address產生系統序號
    '輸入：硬碟physical序號或網卡MAC Address
    '輸出：系統序號
    
    Dim dblSysSerialNo As Double
    Dim strSysSerialNo As String
    Dim i As Integer
    
    strGeneralSerialNo = Trim(strGeneralSerialNo)
    dblSysSerialNo = 11 * 13
    For i = 1 To Len(strGeneralSerialNo)
        dblSysSerialNo = dblSysSerialNo * 11 + Asc(Mid(strGeneralSerialNo, i, 1)) ^ 2 * 13
    Next
    
    strSysSerialNo = Format(dblSysSerialNo, "0")
    getSysSerialNo = IIf(Len(strSysSerialNo) > 8, Right(strSysSerialNo, 8), strSysSerialNo)
End Function

Public Sub Main()
    Dim strSmartvsdPath As String 'Smartvsd.vxd完整路徑
    Dim strSysSerialNo As String '系統序號
    Dim objMyDB As MyDB
    Dim objRS As ADODB.Recordset
    Dim strSQL As String
    
    On Error GoTo ErrorHandler
    
    g_MDB_PATH = ""
    
    g_MonitorSystemTime = Now
    g_MonitorSysTimeDesc = ""
    g_MonitorSysTimeDescForClient = ""
    
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ╔程式開始執行。")
    
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╔開始讀取ini檔。")
    ' 讀取ini檔、設定webService 和決定是否建立捷徑
    Call ReadINI
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╚結束讀取ini檔。")

    If g_MDB_PATH = "" Then g_MDB_PATH = App.Path
    
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ╔程式開始執行。")

    g_LOCAL_DB_CONNECTION_STRING = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=" & LOCAL_DB_PASSWORD & ";Data Source=" & g_MDB_PATH & "\屠檢人員簽到退作業系統.mdb"  ' self explanatory

    'If ISDEBUG Then MsgBox "Connection string: " & g_LOCAL_DB_CONNECTION_STRING

    If App.PrevInstance Then
        End
        Exit Sub
    End If
    
    Select Case getOSVersion
        Case OS_VER_WIN_95, OS_VER_WIN_98, OS_VER_WIN_ME
            strSmartvsdPath = getSystemPath & "\Iosubsys\Smartvsd.vxd"
            If Dir(strSmartvsdPath) = "" Then
                FileCopy App.Path & "\Smartvsd.vxd", strSmartvsdPath
                MsgBox "請重新啟動電腦", vbOKOnly + vbInformation, "訊息提示"
                Exit Sub
            End If
    End Select

    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╔開始MDB。")
    Call modifyDB '修改DataBase
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╚結束MDB。")
    
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╔開始GSN。")
    strSysSerialNo = getHddSerialNo() '讀取Primary Master IDE硬碟序號
    If strSysSerialNo = "" Then
        strSysSerialNo = GetMacAddress() '若無法讀取硬碟序號改讀取網卡MAC Address
    End If
    If strSysSerialNo <> "" Then strSysSerialNo = getSysSerialNo(strSysSerialNo) '不為空才要計算系統序號
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╚結束GSN。")
    
    Set objMyDB = New MyDB
    objMyDB.OpenConnection
    
    strSQL = "select * from SysPara"
    Set objRS = objMyDB.RetrieveData(strSQL)
    
    g_strAbaNo = Trim("" & objRS("AbaNo"))
    g_strAbaName = Trim("" & objRS("AbaName"))
    
    g_intDelayTime = Val("" & objRS("DelayTime"))
    g_strUpdateTime = Trim("" & objRS("UpdateUserList"))
    
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╔開始CNN。")
    If strSysSerialNo = "" Then ' 沒連接網路線在網卡的話會有這個問題，用另一種方法把全部的physical adapter 的 mac 抓回來比對
        If (ISDEBUG) Then MsgBox "應該是網卡沒插…"
        Dim i As Integer
        Dim macs() As String
        macs = GetMACAddresses()
        For i = LBound(macs) To UBound(macs)
            If (ISDEBUG) Then MsgBox "Mac #" & i & "=「" & macs(i) & "」"
            If getSysSerialNo(macs(i)) = Trim("" & objRS("SysSerialNo")) Then
                g_isLegalSystem = True
            End If
        Next
    ElseIf strSysSerialNo = Trim("" & objRS("SysSerialNo")) Then
        g_isLegalSystem = True
        g_strSysSerialNo = Trim("" & objRS("SysSerialNo"))
    Else
        g_isLegalSystem = False
    End If
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╚結束CNN。")
    
    If Not g_isLegalSystem Then frmSysSerialNo.Show vbModal
    If Not g_isLegalSystem Then Exit Sub
    
    g_isLegalUser = False
        
    Set objRS = Nothing
    Set objMyDB = Nothing
        
    'frmNTP.isAuto = True
    'frmNTP.Show vbModal
    'DoEvents
    'MsgBox ("g_ThisNTPTime = 「" & g_ThisNTPTime & "」，有的話代表有校時到。")
    'Exit Sub

        
    If Not g_isLegalUser Then frmLogin.Show vbModal
    If Not g_isLegalUser Then Exit Sub
        
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╔開始MKD。")
    If Dir(g_MDB_PATH & "\Bmp", vbDirectory) = "" Then MkDir g_MDB_PATH & "\Bmp"
    If Dir(g_MDB_PATH & "\Ftp", vbDirectory) = "" Then MkDir g_MDB_PATH & "\Ftp"
    If Dir(g_MDB_PATH & "\Pic", vbDirectory) = "" Then MkDir g_MDB_PATH & "\Pic"
    If Dir(g_MDB_PATH & "\SignFeature", vbDirectory) = "" Then MkDir g_MDB_PATH & "\SignFeature"
    If Dir(g_MDB_PATH & "\srdata", vbDirectory) = "" Then MkDir g_MDB_PATH & "\srdata"
    If Dir(g_MDB_PATH & "\clientOfflineSrdata", vbDirectory) = "" Then MkDir g_MDB_PATH & "\clientOfflineSrdata"
    
    SRVB_GetDataDirectory AddressOf StringCallBack
    g_SgnDirPath = g_String
    If Dir(g_SgnDirPath, vbDirectory) = "" Then MkDir g_SgnDirPath
    
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╚結束MKD。")
    
    If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ╚OK。")

    frmMain.Show
    
    Exit Sub
ErrorHandler:
    If Not objMyDB Is Nothing Then Set objMyDB = Nothing
    If Not objRS Is Nothing Then Set objRS = Nothing
    MsgBox "Error Number:" & Err.Number & vbCrLf & "Error Description:" & Err.Description, vbOKOnly + vbInformation, "訊息提示"
End Sub

Public Sub modifyDB()
    Dim objMyDB As MyDB
    Dim objRS As ADODB.Recordset
    Dim strSQL As String
    Dim i As Integer
    
    Dim tblDefines As TableDefs
    
    Set objMyDB = New MyDB
    objMyDB.OpenConnection

    strSQL = "select * from LogTrn where 1=2"
    Set objRS = objMyDB.RetrieveData(strSQL)
    
    '新增UTIMES欄位
    For i = 0 To objRS.Fields.Count - 1
        If UCase(objRS(i).Name) = "UTIMES" Then Exit For
    Next
    If i = objRS.Fields.Count Then
        strSQL = "alter table LogTrn add column UTimes int default 0"
        objMyDB.Execute strSQL
        DoEvents
    End If
    
    '新增UTIMES欄位
    For i = 0 To objRS.Fields.Count - 1
        If UCase(objRS(i).Name) = "UTIMES" Then Exit For
    Next
    If i = objRS.Fields.Count Then
        strSQL = "alter table LogTrn add column UTimes int default 0"
        objMyDB.Execute strSQL
        DoEvents
    End If
    
    '新增Chk欄位，存放的是這筆資料加密過的一些簽到時資訊
    For i = 0 To objRS.Fields.Count - 1
        If UCase(objRS(i).Name) = "CHK" Then Exit For
    Next
    If i = objRS.Fields.Count Then
        strSQL = "alter table LogTrn add column Chk memo"
        objMyDB.Execute strSQL
        DoEvents
    End If
    
    strSQL = "select * from SysPara where 1=2"
    Set objRS = objMyDB.RetrieveData(strSQL)
    For i = 0 To objRS.Fields.Count - 1
        If UCase(objRS(i).Name) = "DELAYTIME" Then Exit For
    Next
    If i = objRS.Fields.Count Then
        strSQL = "alter table SysPara add column DelayTime int "
        objMyDB.Execute strSQL
        DoEvents
        strSQL = "update SysPara set DelayTime=2"
        objMyDB.Execute strSQL
        DoEvents
    End If
    
    '新增UserID欄位
    strSQL = "select * from Doctor where 1=2"
    Set objRS = objMyDB.RetrieveData(strSQL)
    For i = 0 To objRS.Fields.Count - 1
        If UCase(objRS(i).Name) = "USERID" Then Exit For
    Next
    If i = objRS.Fields.Count Then
        strSQL = "alter table Doctor add column UserID Text(50)"
        objMyDB.Execute strSQL
        g_DBUpdate = True
        DoEvents
    End If
    
    '新增Pwd欄位
    For i = 0 To objRS.Fields.Count - 1
        If UCase(objRS(i).Name) = "PWD" Then Exit For
    Next
    If i = objRS.Fields.Count Then
        strSQL = "alter table Doctor add column Pwd Text(50)"
        objMyDB.Execute strSQL
        g_DBUpdate = True
        DoEvents
    End If
    
    Set objRS = Nothing
    Set objMyDB = Nothing
End Sub

'''''''''''''''''''''''''''''''''''''''''''''''
' 更新 v3 Fadis, 從OA撈取使用者資料           '
'''''''''''''''''''''''''''''''''''''''''''''''
Public Sub UpdateUserList()
    
    Dim errMessage As String

    On Error GoTo ErrorHandler:
    
    Dim objMyDB As MyDB
    Dim strSQL As String
    Dim i As Integer
    Dim objRS As ADODB.Recordset
    Set objMyDB = New MyDB
    objMyDB.OpenConnection
    
    Dim strNow As String

    If g_DBUpdate Then
        strSQL = "delete from Doctor"
    Else
        strSQL = "update Doctor set Online='N'"
    End If
    objMyDB.Execute (strSQL)
    
    '設定Web Service
    errMessage = "連接伺服器過程發生錯誤，請稍候再試。"
    Dim mySOAP As New SoapClient30
    mySOAP.MSSoapInit (g_WSurl & "?WSDL")
    mySOAP.ConnectorProperty("Timeout") = WSTimeOut
    Dim strUserList As String
    
    errMessage = "從伺服器取得資料過程發生錯誤，請稍候再試。"
    strUserList = mySOAP.GetUserList(g_strAbaNo)
    Dim strArray() As String
    strArray = Split(strUserList, "#")

    For i = 0 To UBound(strArray)
        Dim strDoctor() As String
        strDoctor = Split(strArray(i), ",")
        If ISDEBUG Then MsgBox "UserID: " & Trim(strDoctor(0)) & ", DoctorName: " & Trim(strDoctor(2)) & ", DoctorNo: " & Trim(strDoctor(1)) & ", Password: " & Trim(strDoctor(3)) & ", 在線上嗎? " & Trim(strDoctor(4))
        
        ' 2010_06_23    刪掉UserID不一樣、但是DoctorNo卻一樣的資料
        '               目的在於刪掉OA號碼被換過的。
        objMyDB.Execute "DELETE FROM Doctor WHERE UserID <> '" & encrypt(Trim(strDoctor(0))) & "' AND DoctorNo = '" & Trim(strDoctor(1)) & "'"
        
        '判斷此帳號是否已存在
        strSQL = "Select * from Doctor where UserID='" & encrypt(Trim(strDoctor(0))) & "'"
        Set objRS = objMyDB.RetrieveData(strSQL)
        If objRS.RecordCount = 0 Then
            '不存在，新增資料
            strSQL = "insert into Doctor (UserID,DoctorNo,DoctorName,Pwd,Online) values ('" & encrypt(Trim(strDoctor(0))) & "','" & Trim(strDoctor(1)) & "','" & Trim(strDoctor(2)) & "','" & encrypt(Trim(strDoctor(3))) & "','" & Trim(strDoctor(4)) & "')"
        Else
            '存在，更新資料
            strSQL = "update Doctor set DoctorNo='" & Trim(strDoctor(1)) & "',DoctorName='" & Trim(strDoctor(2)) & "',Pwd='" & encrypt(Trim(strDoctor(3))) & "',Online='" & Trim(strDoctor(4)) & "' where UserID='" & encrypt(Trim(strDoctor(0))) & "'"
        End If
        Set objRS = Nothing
        objMyDB.Execute strSQL
    Next i
    
    strNow = Now
    strSQL = "update SysPara set UpdateUserList='" & strNow & "'"
    objMyDB.Execute strSQL
    g_strUpdateTime = strNow
    
    Set objMyDB = Nothing
    
    Exit Sub
    
ErrorHandler:

    If ISDEBUG Then MsgBox "UpdateUserList() 發生錯誤: Error(" & str$(Err.Number) & ")，Source:「" & Err.Source & "」，說明: 「" & Err.Description & "」occured"
    Set objMyDB = Nothing
    MsgBox (errMessage)
    
End Sub
'''''''''''''''''''''''''''''''''''''''''''''''
' 連接ws抓取這個使用者最新的註冊檔  '
'''''''''''''''''''''''''''''''''''''''''''''''
Public Sub GetLatestDocSgn()
    
    Dim errMessage As String
    '讀取檔案到二進位陣列
    Dim sgnFile() As Byte
        
    On Error GoTo ErrorHandler:
    g_isCheckSgnSignLevel = False
    
    Dim oFile As New Scripting.FileSystemObject
    'If oFile.FileExists(g_MDB_PATH & "\srdata\" & g_strDoctorNo & ".sgn") Then
    '    Kill g_MDB_PATH & "\srdata\" & g_strDoctorNo & ".sgn"
    'End If
    If Dir(g_MDB_PATH & "\srdata\") <> "" Then
        Kill g_MDB_PATH & "\srdata\*.*"
    End If
        
    '設定Web Service
    errMessage = "連接伺服器過程發生錯誤，請稍候再試。"
    Dim mySOAP As New SoapClient30
    mySOAP.MSSoapInit (g_WSurl & "?WSDL")
    mySOAP.ConnectorProperty("Timeout") = WSTimeOut
    errMessage = "從伺服器取得註冊檔過程發生錯誤，請稍候再試。"
    sgnFile = mySOAP.DocSgnLastUploadedPath(g_strDoctorNo)
    Call WriteByteArray(g_SgnDirPath & "\" & g_strDoctorNo & ".sgn", sgnFile)
    If oFile.FileExists(g_MDB_PATH & "\clientOfflineSrdata\" & g_strDoctorNo & ".sgn") Then
        Kill g_MDB_PATH & "\clientOfflineSrdata\" & g_strDoctorNo & ".sgn"
    End If
    FileCopy g_SgnDirPath & "\" & g_strDoctorNo & ".sgn", g_MDB_PATH & "\clientOfflineSrdata\" & g_strDoctorNo & ".sgn"
    
    g_isCheckSgnSignLevel = True
    Exit Sub
ErrorHandler:
    If oFile.FileExists(g_MDB_PATH & "\clientOfflineSrdata\" & g_strDoctorNo & ".sgn") Then
        FileCopy g_MDB_PATH & "\clientOfflineSrdata\" & g_strDoctorNo & ".sgn", g_SgnDirPath & "\" & g_strDoctorNo & ".sgn"
        g_isCheckSgnSignLevel = True
    End If
    If ISDEBUG Then MsgBox "GetLatestDocSgn() 發生錯誤: Error(" & str$(Err.Number) & ")，Source:「" & Err.Source & "」，說明: 「" & Err.Description & "」occured"
    'MsgBox (errMessage)
    
End Sub

Public Sub WriteByteArray(ByVal strPath As String, ByRef arrData() As Byte)

    Dim lngFile As Long
   
    ' open the file
    lngFile = FreeFile()
    Open strPath For Binary Access Write As lngFile
   
    ' write blob
    Put lngFile, , arrData
   
    ' close file
    Close lngFile

End Sub
    
'''''''''''''''''''''''''''''''''''''''''''''''
' 連接ws抓取這個使用者需不需要重新上傳註冊檔  '
'''''''''''''''''''''''''''''''''''''''''''''''
Public Function ReUploadSgnFileNeeded() As Boolean
    
    Dim errMessage As String
    ReUploadSgnFileNeeded = False
    
    On Error GoTo ErrorHandler:
    
    Dim result As String
    
    '設定Web Service
    errMessage = "連接伺服器過程發生錯誤，請稍候再試。"
    Dim mySOAP As New SoapClient30
    mySOAP.MSSoapInit (g_WSurl & "?WSDL")
    mySOAP.ConnectorProperty("Timeout") = WSTimeOut
    Dim strUserList As String
    
    errMessage = "從伺服器取得是否重新上傳註冊檔過程發生錯誤，請稍候再試。"
    result = mySOAP.ReUploadSgnNeeded(g_strDoctorNo)
    If result <> "0" Then
        ReUploadSgnFileNeeded = True
    End If
    Exit Function
ErrorHandler:

    If ISDEBUG Then MsgBox "ReUploadSgnFileNeeded() 發生錯誤: Error(" & str$(Err.Number) & ")，Source:「" & Err.Source & "」，說明: 「" & Err.Description & "」occured"
    'MsgBox (errMessage)
    
End Function
'''''''''''''''''''''''''''''''''''''''''''
' V3.0 加密
'''''''''''''''''''''''''''''''''''''''''''
Public Function encrypt(strTemp As String) As String
    encrypt = MD5_string(strTemp)
End Function

''''''''''''''''''''''''''''''''''''''
' 讀取ini檔設定
''''''''''''''''''''''''''''''''''''''
Sub ReadINI()

    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠開始讀取設定檔")
    Dim objTS As TextStream
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠建立TextStream")
    Dim objFS As FileSystemObject
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠建立FileSystemObject")
    Dim strFilename As String
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠建立字串")
    Dim ServerWS As String
    Dim strTemp As String
    Dim strArray() As String
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠建立bln")
    Dim createdShortCut As Boolean ' 是不是創過了桌面捷徑
    
    createdShortCut = False
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠New FileSystemObject")
    Set objFS = New FileSystemObject
    strFilename = Replace(App.Path & "\config.ini", "\\", "\")
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠讀取設定檔「" & strFilename & "」 ")
    Set objTS = objFS.OpenTextFile(strFilename, ForReading, True)
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠FileSystemObject.OpenTextFile")
    
    
    Do While Not objTS.AtEndOfStream
        DoEvents
        strTemp = Trim(objTS.ReadLine)
        If strTemp <> "" Then
            strArray = Split(strTemp, "=")
            If UBound(strArray) = 1 Then
                Select Case UCase(Trim(strArray(0)))
                    Case UCase("WebService")
                        g_WSurl = Trim(strArray(1))
                        'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠WS=「" & g_WSurl & "」 ")
                    Case UCase("CreatedShortCut")
                        If UCase(Trim(strArray(1))) = "Y" Then createdShortCut = True
                        'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠SHCT=「" & createdShortCut & "」 ")
                    Case UCase("MDBPath")
                        g_MDB_PATH = Trim(strArray(1))
                        'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║╠MDPH=「" & g_MDB_PATH & "」 ")
                End Select
            End If
        End If
    Loop
    
    objTS.Close
    Set objTS = Nothing
    
    ' 沒建立過桌面捷徑才去建
    If Not createdShortCut Then
        'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║║╔開始CrSrt")
        Call DoCreateShortCut("家畜屠宰衛生檢查系統(" & VERSION & ")", App.Path & "\" & App.EXEName & ".exe", "")
        'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ║║╚結束CrSrt")
    End If
    
End Sub

''''''''''''''''''''''''''''''''''''''
' 創桌面捷徑，並寫config.ini 「CreatedShortCut=Y」
''''''''''''''''''''''''''''''''''''''
Sub DoCreateShortCut(lnkName As String, exePath As String, destDir As String)

    Dim WshShell            As New WshShell
    Dim oShellLink          As WshShortcut
    
    If destDir = "" Then destDir = WshShell.SpecialFolders("AllUsersDesktop")
    
    Set oShellLink = WshShell.CreateShortcut(Replace(destDir & "\" & lnkName & ".lnk", "\\", "\"))
    Set WshShell = Nothing
    With oShellLink
    .TargetPath = Replace(exePath, "\\", "\")
    .WindowStyle = 1
    '.IconLocation = xxxx
    .Arguments = ""
    End With
    oShellLink.Save
    
    ' 存完再去寫Config 檔
     Call writeToFiles(App.Path & "\config.ini", vbCrLf & "CreatedShortCut=Y")
End Sub

'''''''''''''''''''''''''''''''''''''''''''
' 亂數數字。
'''''''''''''''''''''''''''''''''''''''''''
Public Function Random(Lowerbound As Long, Upperbound As Long)
    Randomize
    Random = Int((Upperbound - Lowerbound) * Rnd + Lowerbound)
End Function

'''''''''''''''''''''''''''''''''''''''''''
' 把字串加一些料，不要讓每個加密完的字串看起來都一樣。
'''''''''''''''''''''''''''''''''''''''''''
Public Function Distract(strRaw As String) As String
    Distract = Format(Now, "YYYY/MM/DD hh:mm:ss") & "!@,@!" & strRaw & "!@,@!" & Random(1000, 9999)
End Function

'''''''''''''''''''''''''''''''''''''''''''
' 監控時間的函數。
'''''''''''''''''''''''''''''''''''''''''''
Public Sub MonitorSystemTime()
    Dim datetimeTemp As Date
    datetimeTemp = Now
    Dim timeDiff As Long
    timeDiff = DateDiff("s", g_MonitorSystemTime, datetimeTemp)
    If (timeDiff < 0) Then
        If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ╠★★★系統時間變更!! 系統時間於「" & Format(g_MonitorSystemTime, "YYYY/MM/DD hh:mm:ss") & "」的10秒鐘內變成了「" & Format(datetimeTemp, "YYYY/MM/DD hh:mm:ss") & "」。")
        g_MonitorSysTimeDesc = g_MonitorSysTimeDesc & "Last Monitor SystemTime is " & Format(g_MonitorSystemTime, "YYYY/MM/DD hh:mm:ss") & " and its now " & Format(datetimeTemp, "YYYY/MM/DD hh:mm:ss") & " that time flows backwards.. not reasonable."
        g_MonitorSysTimeDescForClient = "您的系統時間於「" & Format(g_MonitorSystemTime, "YYYY/MM/DD hh:mm:ss") & "」的10秒鐘內變成了「" & Format(datetimeTemp, "YYYY/MM/DD hh:mm:ss") & "」，請確認您的系統時間是否正常。"
    End If
        
    g_MonitorSystemTime = datetimeTemp
        
    'If CLIENT_DEBUG Then Call writeToFiles(g_MDB_PATH & "\log.txt", Format(Now, "YYYY/MM/DD hh:mm:ss") & "     ╠又過了10秒。")
End Sub

'''''''''''''''''''''''''''''''''''''''''''
' 如果系統時間有變更的話通知一下使用者。
'''''''''''''''''''''''''''''''''''''''''''
Public Sub TellClientIfSystemTimeChanged()
    If g_MonitorSysTimeDescForClient <> "" Then
        MsgBox g_MonitorSysTimeDescForClient
        g_MonitorSysTimeDescForClient = ""
    End If
End Sub
