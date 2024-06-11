'===============================================================================
'   Description  : QRデータ処理(ローム殿特注) V1.18.0.0②
'
'   Copyright(C) : OMRON LASERFRONT INC. 2014
'
'   ReMarks      : ウェルコムデザイン社製２次元コードリーダ OPI-2201用
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports TKY_ALL_SL432HW.My.Resources                                    ' V4.4.0.0-0
Imports LaserFront.Trimmer.DefWin32Fnc                                  ' V6.0.3.0④

Module QR_Data

#Region "【定数/変数の定義】"
    '===========================================================================
    '   定数/変数の定義
    '===========================================================================
    '----- クラスオブジェクト -----
    Private ObjQR_PortInfo As DllSerialIO.PortInformation = Nothing     ' シリアルポート情報オブジェクト(DllSerialIO.dll)
    Private ObjQRDataIO As DllQRDataIO.QRDataIO = Nothing               ' QRデータ入出力オブジェクト(DllQRDataIO.dll)

    Private N As Integer = 5                                            ' V6.0.3.0_21  4 -> 5
    Public gsQRInfo(N) As String                                        ' QR情報表示用ﾊﾞｯﾌｧ
    Public giSTART(N) As Integer                                        ' 開始桁 0:タイプ, 1:許容差, 2:抵抗値(IEC), 3:抵抗値(実抵抗値), 4:Lot No.
    Public giEND(N) As Integer                                          ' 終了桁 0:タイプ, 1:許容差, 2:抵抗値(IEC), 3:抵抗値(実抵抗値), 4:Lot No.
    Public giUse(N) As Integer                                          ' 使用桁 0:タイプ, 1:許容差, 2:抵抗値(IEC), 3:抵抗値(実抵抗値), 4:Lot No.

    Public QR_Data As String                                            ' QRｺｰﾄﾞ受信ﾃﾞｰﾀ(text)
    Public QR_Read_Flg As Integer = 0                                   ' QRｺｰﾄﾞ読み込み判定(0)NG (1)OK
    Public strQRLoadFileFullPath As String                              ' QRｺｰﾄﾞから読み出したﾌｧｲﾙﾊﾟｽ
    Public Const QRDATA_DIR_PATH As String = "C:\TRIMDATA"              ' データファイルフォルダー
    Private Const SYSPARAMPATH As String = "C:\TRIM\tky.ini"            ' システムパラメータパス名 ' V6.0.3.0④

    '----- フラグ -----
    Public QR_Rs_Flag As Integer = 0                                    ' シリアルポートオープンフラグ(0:未ｵｰﾌﾟﾝ, 1:ｵｰﾌﾟﾝ済)

#End Region

#Region "【メソッド定義】"
    '---------------------------------------------------------------------------
    '   シリアル送受信処理
    '---------------------------------------------------------------------------
#Region "RS232Cポートのオープン"
    '''=========================================================================
    '''<summary>RS232Cポートのオープン</summary>
    '''<returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function QR_Rs232c_Open() As Integer

        Dim r As Integer
        Dim strMSG As String
        Dim PortNo As String                                            ' V6.0.3.0④

        Try
            ' 初期処理        
            ' V6.0.3.0⑤ QR_Read_Flg = 0                                ' QRｺｰﾄﾞ読込み判定(0)NG (1)OK 
            r = SerialErrorCode.rRS_OK                                  ' Return値 = 正常
            'If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return (r) '  ローム殿特注でなければNOP V6.1.4.0_22
            ObjQRDataIO = New DllQRDataIO.QRDataIO
            ObjQR_PortInfo = New DllSerialIO.PortInformation            ' シリアルポート情報オブジェクト生成

            ' ポート情報を設定する
            '----- V6.0.3.0④↓ -----
            'ObjQR_PortInfo.PortName = "COM4"'V4.0.0.0-79               ' ポート番号
            'ObjQR_PortInfo.PortName = "COM5"                           ' ポート番号
            PortNo = GetPrivateProfileString_S("QR_CODE", "COM", SYSPARAMPATH, "COM5")
            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                QR_Read_Flg = 0                                         ' QRｺｰﾄﾞ読込み判定(0)NG (1)OK
                PortNo = GetPrivateProfileString_S("QR_CODE", "RS232C_PORT_NO", "C:\TRIM\tky.ini", "COM2")
            End If
            '----- V6.1.4.0_22↑ -----
            ObjQR_PortInfo.PortName = PortNo                            ' ポート番号
            '----- V6.0.3.0④↑ -----
            ObjQR_PortInfo.BaudRate = 9600                              ' Speed
            ObjQR_PortInfo.Parity = 0                                   ' Parity(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
            ObjQR_PortInfo.DataBits = 8                                 ' データ長 = 8 Bit
            ObjQR_PortInfo.StopBits = 1                                 ' Stop Bit = 1 Bit

            ' ポートオープン
            r = ObjQRDataIO.Serial_Open(ObjQR_PortInfo)                 ' ポートオープン
            If (r = SerialErrorCode.rRS_OK) Then                        ' 正常 ? 
                QR_Rs_Flag = 1                                          ' ﾌﾗｸﾞ=1(ｵｰﾌﾟﾝ済)
            End If
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Data.QR_Rs232c_Open() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "RS232Cポートのクローズ"
    '''=========================================================================
    '''<summary>RS232Cポートのクローズ</summary>
    '''<returns>0=正常, 0以外=エラー</returns> 
    '''=========================================================================
    Public Function QR_Rs232c_Close() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ポートクローズ
            r = SerialErrorCode.rRS_OK                                  ' Return値 = 正常
            If (QR_Rs_Flag = 0) Then Exit Function '                    ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ)ならNOP
            r = ObjQRDataIO.Serial_Close()                              ' ポートクローズ

            '----- V6.0.3.0_36↓ -----
            ObjQR_PortInfo = Nothing                                    ' シリアルポート情報オブジェクト解放
            ObjQRDataIO = Nothing                                       ' シリアルＩＯオブジェクト解放
            QR_Rs_Flag = 0                                              ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ)
            '----- V6.0.3.0_36↑ -----

            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Data.QR_Rs232c_Close() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = SerialErrorCode.rRS_Trap
        End Try

        ObjQR_PortInfo = Nothing                                        ' シリアルポート情報オブジェクト解放
        ObjQRDataIO = Nothing                                           ' シリアルＩＯオブジェクト解放
        QR_Rs_Flag = 0                                                  ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ)
        Return (r)
    End Function
#End Region

#Region "受信したQRデータを取得する"
    '''=========================================================================
    ''' <summary>受信したQRデータを取得する</summary>
    ''' <param name="receivedData"></param>
    ''' <returns>cFRS_NORMAL  = QRデータを受信した
    '''          cFRS_ERR_RST = QRデータを受信してない　　　　　　　　　</returns>
    '''=========================================================================
    Public Function QR_GetReceiveData(ByRef receivedData As String) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 初期処理
            '----- V6.0.3.0_35↓ -----
            'If (QR_Rs_Flag = 0) Then Exit Function '                    ' フラグ=0(RS232C未ｵｰﾌﾟﾝ)ならNOP
            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
            'If (QR_Rs_Flag = 0) Then Return (cFRS_ERR_RST) '            ' RS232C未ｵｰﾌﾟﾝならcFRS_ERR_RSTを返す 
            If (QR_Rs_Flag = 0) And (giQrCodeType = QrCodeType.Rome) Then
                Return (cFRS_ERR_RST)
            End If
            '----- V6.1.4.0_22↑ -----
            '----- V6.0.3.0_35↑ -----

            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                If (QR_Rs_Flag = 0) Then                                    ' シリアルポートオープンフラグ(0:未ｵｰﾌﾟﾝ, 1:ｵｰﾌﾟﾝ済)
                    'ポートオープン(QRデータ受信用)
                    r = QR_Rs232c_Open()
                    If (r <> SerialErrorCode.rRS_OK) Then
                        ' "シリアルポートＯＰＥＮエラー"
                        strMSG = MSG_136 + "(" + "QR Code Reader " + GetPrivateProfileString_S("QR_CODE", "RS232C_PORT_NO", "C:\TRIM\tky.ini", "COM2") + ")"
                        Call MsgBox(strMSG, vbOKOnly)
                        'Return (ret)
                    End If
                End If
            End If
            '----- V6.1.4.0_22↑ -----

            ' 受信したQRデータを取得する
            r = ObjQRDataIO.GetReceiveData(receivedData)
            If (r <> SerialErrorCode.rRS_OK) Then                       ' 受信データなし ? 
                Return (cFRS_ERR_RST)
            End If
            QR_Read_Flg = 1                                             ' QRｺｰﾄﾞ読み込み判定(0)NG (1)OK
            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Data.OnReceiveDataEvent() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "QRｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する"
    '''=========================================================================
    ''' <summary>QRｺｰﾄﾞ受信ﾃﾞｰﾀから指定の文字列を編集しﾌｧｲﾙﾊﾟｽを作成する</summary>
    ''' <param name="strCheckData">(INP)QRｺｰﾄﾞ読込みﾃﾞｰﾀ</param>
    ''' <returns>ファイルパス名</returns>
    '''=========================================================================
    Public Function GetQrCodeFileName(ByVal strCheckData As String) As String

        Dim strPosGet As String
        Dim strMakeName(4) As String
        Dim strSetFullPath As String
        Dim s(1) As String
        Dim strMSG As String
        Dim r As Integer        'V4.1.0.0①
        Dim DspMsg As String    'V4.1.0.0①

        Try
            If (True = String.IsNullOrEmpty(strCheckData)) Then Return String.Empty ' V4.0.0.0-66

            ' タイプ取得(17-23桁目)　　　　　　　　　　　               ' フォルダ名 = タイプ + 許容差 (例)"UCR01G")
            strPosGet = strCheckData.Substring(giSTART(0) - 1, giUse(0))
            strMakeName(0) = RTrim(strPosGet)                           ' "UCR01"   後方空白を除く

            ' 許容差取得(46-46桁目)
            strPosGet = strCheckData.Substring(giSTART(1) - 1, giUse(1))
            strMakeName(1) = RTrim(strPosGet)                           ' "G" 

            ' 抵抗値取得(IEC)(48-52桁目)                               ' ファイル名 = タイプ + 許容差 + 抵抗値取得(IEX) (例)"UCR01G1R00.tdc")
            strPosGet = strCheckData.Substring(giSTART(2) - 1, giUse(2))
            strMakeName(2) = RTrim(strPosGet)                           ' "1R00" 

            ' 抵抗値(実抵抗値)取得(53-64桁目)  
            strPosGet = strCheckData.Substring(giSTART(3) - 1, giUse(3))
            strMakeName(3) = RTrim(strPosGet)

            ' ロットNo.(1-16桁目 (ロット番号(1-14桁目)-ロット番号小番(15-16桁目))  
            strPosGet = strCheckData.Substring(giSTART(4) - 1, giUse(4))
            strMakeName(4) = RTrim(strPosGet)

            ' ロットNo.編集(ロット番号-ロット番号小番)
            s(0) = strCheckData.Substring(giSTART(4) - 1, giUse(4) - 2)
            s(0) = RTrim(s(0))
            s(1) = strCheckData.Substring(giSTART(4) + giUse(4) - 2 - 1, 2)
            s(1) = Trim(s(1))
            gsQRInfo(4) = s(0) & "-" & s(1)                             ' ロットNo.

            '----- V6.0.3.0_21 ↓ -----               ↓
            ' ロット投入基板枚数
            If (0 < giSTART(5)) AndAlso (0 <= giUse(5)) Then            ' V6.0.3.0_23
                strPosGet = strCheckData.Substring(giSTART(5) - 1, giUse(5))
            Else
                strPosGet = "0"
            End If
            Dim n As Integer
            If (Integer.TryParse(strPosGet, n)) Then                    ' TryParseは空白を含んでも問題ない
                FormLotEnd.Input = n
                gsQRInfo(5) = n.ToString("0")
            Else
                n = (-1)    ' 数値として使用できない
                gsQRInfo(5) = """" & strPosGet & """"
            End If            
            '----- V6.0.3.0_21 ↑ -----

            ' 表示域に設定
            gsQRInfo(0) = strMakeName(0)                                ' タイプ
            gsQRInfo(1) = strMakeName(1)                                ' 許容差
            gsQRInfo(2) = strMakeName(2)                                ' 抵抗値(IEC)  
            gsQRInfo(3) = strMakeName(3)                                ' 抵抗値(実抵抗値)

            ' ファイルパス名 = フォルダ名+ファイル名
            strSetFullPath = QRDATA_DIR_PATH & "\" & strMakeName(0) & strMakeName(1) & "\" & strMakeName(0) & strMakeName(1) & strMakeName(2) & ".tdc"
            '----- V4.0.0.0⑨↓ -----
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ?
                strSetFullPath = QRDATA_DIR_PATH & "\" & strMakeName(0) & strMakeName(1) & "\" & strMakeName(0) & strMakeName(1) & strMakeName(2) & ".tdcs"
            End If
            '----- V4.0.0.0⑨↑ -----

            ' ファイルが存在するかチェックする
            If (System.IO.File.Exists(strSetFullPath) = False) Then
                strMSG = "File Not Exist " + strSetFullPath
                Call Form1.Z_PRINT(strMSG)
                ' --- V4.1.0.0①↓-----------------------------------------------------
                DspMsg = strSetFullPath
                ' "ＱＲコードに対応したトリミングデータがありません。","ファイルを確認してください。",""
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH48, MSG_SPRASH49, DspMsg, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                ' --- V4.1.0.0①↑-----------------------------------------------------
                strSetFullPath = ""
                '----- V6.0.3.0_21↓ -----         ↓
                FormLotEnd.Input = 0

            ElseIf (n < 0) Then
                FormLotEnd.Input = 0

                ' "ロット投入基板枚数を数値として認識できません。", "0 として処理します。" 'V6.0.3.0_21
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH72, gsQRInfo(5), MSG_SPRASH73, Color.Blue, Color.Blue, Color.Blue)

                gsQRInfo(5) &= " : NaN -> 0"
                Form1.Z_PRINT(gsQRInfo(5))
            Else
                ' DO NOTHING
                '----- V6.0.3.0_21↑ -----            ↑
            End If

            Return (strSetFullPath)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "QR_Data.GetQrCodeFileName() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region

#Region "QRデータで指定のファイルをロードする"
    '''=========================================================================
    ''' <summary>QRデータで指定のファイルをロードする</summary>
    ''' <param name="pPath">(INP)ファイルパス名</param>
    '''=========================================================================
    Public Sub DataLoadQR(ByRef pPath As String)

        Dim r As Integer
        Dim Dt As DateTime = DateTime.Now                               ' 現在の日時を取得
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' 現在の時刻のみを取得 
        Dim strMSG As String

        Try
            ' 初期処理
            Call SetMousePointer(Form1, True)                           ' 砂時計表示(mouse pointer)

            ' ファイルロード
            r = Form1.Sub_FileLoad(pPath)
            If (r <> cFRS_NORMAL) Then                                  ' ファイルロードエラー ?(※エラーメッセージは表示済み) 
                Exit Sub
            End If

            ' 終了処理
            Call Form1.ClearCounter(1)                                  ' 生産管理データクリア
            Call ClrTrimPrnData()                                       ' トリミング結果印刷項目初期化 
            Call QR_Info_Disp(1)                                        ' QRコード情報を表示する

            'Call SetMousePointer(Form1, False)                          ' V6.0.3.0_22 砂時計解除(mouse pointer)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Data.DataLoadQR() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)

        Finally                                                         ' V6.0.3.0_22
            SetMousePointer(Form1, False)                               ' 砂時計解除(mouse pointer)

        End Try
    End Sub
#End Region

#Region "QRデータのオフセット位置をシスパラから取得する"
    '''=========================================================================
    ''' <summary>QRデータのオフセット位置をシスパラから取得する</summary>
    ''' <param name="sPath">(INP)システムパラメータパス名</param>
    '''=========================================================================
    Public Sub GetSysPrm_QR_DataOfs(ByRef sPath As String)

        Dim strMSG As String

        Try
            ' QRデータのオフセット位置をシスパラから取得する
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '  ローム殿特注でなければNOP

            ' QRコード(タイプ)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "TYPE_ST", giSTART(0))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "TYPE_END", giEND(0))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "TYPE_USE", giUse(0))

            ' QRコード(許容差)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "ALLO_ST", giSTART(1))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "ALLO_END", giEND(1))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "ALLO_USE", giUse(1))

            ' QRコード(抵抗値IEC)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RIEC_ST", giSTART(2))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RIEC_END", giEND(2))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RIEC_USE", giUse(2))

            ' QRコード(抵抗値TURE)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RTRU_ST", giSTART(3))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RTRU_END", giEND(3))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RTRU_USE", giUse(3))

            ' QRコード(Lot No.)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "LOTN_ST", giSTART(4))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "LOTN_END", giEND(4))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "LOTN_USE", giUse(4))

            '----- V6.0.3.0_21↓ -----            ↓
            ' ロット投入基板枚数
            Get_SystemParameterShort(sPath, "QR_CODE", "LTIN_ST", giSTART(5))
            Get_SystemParameterShort(sPath, "QR_CODE", "LTIN_END", giEND(5))
            Get_SystemParameterShort(sPath, "QR_CODE", "LTIN_USE", giUse(5))

            ' 許容基板枚数
            Get_SystemParameterShort(sPath, "QR_CODE", "ALB_NUM", FormLotEnd.Allowable)
            '----- V6.0.3.0_21↑ -----          ↑

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Data.GetSysPrm_QR_DataOfs() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "QRコード情報を表示する"
    '''=========================================================================
    ''' <summary>QRコード情報を表示する</summary>
    ''' <param name="Md">(INP)モード(0=初期化, 1=表示)</param>
    '''=========================================================================
    Public Sub QR_Info_Disp(ByVal Md As Integer)

        Dim strMSG As String

        Try
            ' QRデータのオフセット位置をシスパラから取得する
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '  ローム殿特注でなければNOP

            ' 初期化
            If (Md = 0) Then
                gsQRInfo(0) = ""
                gsQRInfo(1) = ""
                gsQRInfo(2) = ""
                gsQRInfo(3) = ""
                gsQRInfo(4) = ""
                gsQRInfo(5) = ""                                    ' V6.0.3.0_21
            End If

            ' QRコード情報を表示する
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    Form1.lblQRData.Text = " タイプ　　　　　　：" & gsQRInfo(0) & vbCrLf & _
            '                            " 許容差　　　　　　：" & gsQRInfo(1) & vbCrLf & _
            '                            " 抵抗値（ＩＥＣ）　：" & gsQRInfo(2) & vbCrLf & _
            '                            " 抵抗値（実抵抗値）：" & gsQRInfo(3) & vbCrLf & _
            '                            " ロットＮｏ．　　　：" & gsQRInfo(4)
            'Else
            '    Form1.lblQRData.Text = " Type               :" & gsQRInfo(0) & vbCrLf & _
            '                             " Allowance          :" & gsQRInfo(1) & vbCrLf & _
            '                             " Res Value(IEC)     :" & gsQRInfo(2) & vbCrLf & _
            '                             " Res Value(True Res):" & gsQRInfo(3) & vbCrLf & _
            '                             " Lot No.            :" & gsQRInfo(4)
            'End If
            ''If (gSysPrm.stTMN.giMsgTyp = 0) Then
            ''    Form1.lblQRData.Text = "[QRコード情報]" & vbCrLf & _
            ''                            " タイプ　　　　　　：" & gsQRInfo(0) & vbCrLf & _
            ''                            " 許容差　　　　　　：" & gsQRInfo(1) & vbCrLf & _
            ''                            " 抵抗値（ＩＥＣ）　：" & gsQRInfo(2) & vbCrLf & _
            ''                            " 抵抗値（実抵抗値）：" & gsQRInfo(3) & vbCrLf & _
            ''                            " ロットＮｏ．　　　：" & gsQRInfo(4) & vbCrLf &
            ''                            " ロット投入基板枚数：" & gsQRInfo(5)     ' V6.0.3.0_21
            ''Else
            ''    Form1.lblQRData.Text = "[QR CODE INFORMATION]" & vbCrLf & _
            ''                             " Type               :" & gsQRInfo(0) & vbCrLf & _
            ''                             " Allowance          :" & gsQRInfo(1) & vbCrLf & _
            ''                             " Res Value(IEC)     :" & gsQRInfo(2) & vbCrLf & _
            ''                             " Res Value(True Res):" & gsQRInfo(3) & vbCrLf & _
            ''                             " Lot No.            :" & gsQRInfo(4) & vbCrLf &
            ''                             " Lot Input          :" & gsQRInfo(5)    ' V6.0.3.0_21
            ''End If
            Form1.lblQRData.Text = QR_Data_001 & gsQRInfo(0) & vbCrLf & _
                                   QR_Data_002 & gsQRInfo(1) & vbCrLf & _
                                   QR_Data_003 & gsQRInfo(2) & vbCrLf & _
                                   QR_Data_004 & gsQRInfo(3) & vbCrLf & _
                                   QR_Data_005 & gsQRInfo(4) & vbCrLf &
                                   QR_Data_006 & gsQRInfo(5)            ' V6.0.3.0_21
            Form1.lblQRData.Refresh()

            ' 印刷項目にタイプとロットNo.を設定する
            stPRT_ROHM.Prot_Typ = gsQRInfo(0)                           ' タイプ
            stPRT_ROHM.Lot_No = gsQRInfo(4)                             ' ロットNo.

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Data.QR_Info_Disp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region
End Module
