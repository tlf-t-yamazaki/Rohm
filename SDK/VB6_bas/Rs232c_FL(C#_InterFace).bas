Attribute VB_Name = "Rs232c_FL"
'==============================================================================
'
'   DESCRIPTION:    トリマー加工条件をFL側よりRS232Cで送受信する
'                   (C#で作成された｢DllFLCom.dll｣を使用)
'                   ※対象機器　FL用FPGA設定用
'                     通信方式       : 調歩同期式半二重通信
'                     ボーレート     : 38,400BPS
'                     キャラクター長 : 8 Bit
'                     パリティ       : なし
'                     ストップ       : 1 BIT
'                     デリミタコード : CR
'
'                   送受信データ形式
'                   1. 送信データ形式(PC　←→ FL)
'                   -------------------------------------------
'                   |コマンド名(2) | データ(4)        | CR(1) |
'                   |A～PのASCII   |0～9,a～fのASCII  |       |
'                   -------------------------------------------
'                   2. 応答要求データ形式(PC　→ FL)
'                   -----------------------------------
'                   |コマンド名(2) | ﾘｰﾄﾞ(1) |　CR(1) |
'                   |A～PのASCII   |  r      |　      |
'                   -----------------------------------
'
'==============================================================================
Option Explicit
'==============================================================================
'   変数定義
'==============================================================================
'----- ポート情報 -----
Public Type ComInfo
    PortName    As String                               ' ＲＳ２３２Ｃポート番号
    BaudRate    As Long                                 ' 伝送スピード
    Parity      As Integer                              ' パリティ(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
    DataBits    As Integer                              ' データー長
    StopBits    As Integer                              ' ストップビット長
End Type

'----- クラスオブジェクト -----
Private ObjPortInfo     As DllSerialIO.PortInformation  ' シリアルポート情報オブジェク(DllSerialIO.dll)
Private ObjFLCom        As DllFLCom.FLComIO             ' トリマー加工条件送受信オブジェクト(DllFLCom.dll)
                                                        
                                                        ' バージョン情報クラス
Private ObjPortCVer     As DllSerialIO.VersionInformation   ' DllSerialIO.dll
Private ObjFLXMLVer     As DllCndXMLIO.VersionInformation   ' DllCndXMLIO.dll
Private ObjFLComVer     As DllFLCom.VersionInformation      ' DllFLCom.dll

Private Rs_Flag         As Integer                      ' ﾌﾗｸﾞ(0:未ｵｰﾌﾟﾝ, 1:ｵｰﾌﾟﾝ済)

'----- トリマー加工条件 -----
Public Const MAX_BANK_NUM   As Integer = 32             ' 最大加工条件数(0-31)
Public Const MAX_STEG_NUM   As Integer = 20             ' STEG波形最大値(1-20)
Public Const MAX_CURR_VAL   As Integer = 8500           ' 最大電流値(mA)
Public Const MIN_CURR_VAL   As Integer = 1              ' 最小電流値(mA)
'Public Const MAX_FREQ_VAL   As Integer = 50            ' 最大周波数(KHz)
Public Const MAX_FREQ_VAL   As Integer = 100            ' 最大周波数(KHz)
Public Const MIN_FREQ_VAL   As Integer = 1              ' 最小周波数(KHz)

Public Type TrimCondInfo                                ' トリマー加工条件形式定義
    Curr(MAX_BANK_NUM)      As Long                     ' 電流値(mA)
    Freq(MAX_BANK_NUM)      As Double                   ' 周波数(KHz)
    Steg(MAX_BANK_NUM)      As Long                     ' STEG波形
End Type

'^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'   エラーコード(C#で作成されたdllが返しているもの)
'^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Public Enum SerialErrorCode
'----- 1-18はDllSerialIOで使用 -----
    rRS_OK = 0                                          '  0:正常
    rRS_ReadTimeout                                     '  1:リードタイムアウト
    rRS_WriteTimeout                                    '  2:ライトタイムアウト
    rRS_RespomseTimeout                                 '  3:応答タイムアウト
    rRS_FailOpen                                        '  4:ｼﾘｱﾙﾎﾟｰﾄｵｰﾌﾟﾝ失敗
    rRS_FailClose                                       '  5:ｼﾘｱﾙﾎﾟｰﾄｸﾛｰｽﾞ失敗
    rRS_FailInit                                        '  6:ｼﾘｱﾙﾎﾟｰﾄ初期化失敗
    rRS_SerialErrorFrame                                '  7:H/Wでﾌﾚｰﾑｴﾗｰ検出
    rRS_SerialErrorOverrun                              '  8:文字ﾊﾞｯﾌｧのｵｰﾊﾞｰﾗﾝ発生
    rRS_SerialErrorRXOver                               '  9:入力ﾊﾞｯﾌｧのｵｰﾊﾞｰﾌﾛｰ発生
    rRS_SerialErrorRXParity                             ' 10:H/Wでﾊﾟﾘﾃｨｴﾗｰ発生
    rRS_SerialErrorTXFull                               ' 11:ｱﾌﾟﾘｹｰｼｮﾝは文字を送信しようとしたが出力ﾊﾞｯﾌｧが一杯
    rRS_InvalidSerialProtInfo                           ' 12:シリアルポート情報不正
    rRS_InvalidValue                                    ' 13:無効なデータ
    rRS_FailSerialRead                                  ' 14:ｼﾘｱﾙﾎﾟｰﾄからの読込失敗
    rRS_FailSerialWrite                                 ' 15:ｼﾘｱﾙﾎﾟｰﾄへの書込失敗
    rRS_NotOpen                                         ' 16:ｼﾘｱﾙﾎﾟｰﾄがｵｰﾌﾟﾝしていない
    rRS_Exception                                       ' 17:例外
    
    '----- 以降は当関数で使用 -----
    rRS_FLCND_NONE = 101                                ' 101:加工条件の設定なし
    rRS_FLCND_XMLNONE = 102                             ' 102:加工条件ファイルが存在しない
    rRS_FLCND_XMLREADERR = 103                          ' 103:加工条件ファイルリードエラー
    rRS_FLCND_XMLWRITERR = 104                          ' 104:加工条件ファイルライトエラー
    rRS_FLCND_SNDERR = 105                              ' 105:加工条件送信エラー
    rRS_FLCND_RCVERR = 106                              ' 106:加工条件受信エラー

'----- 以降はDllFLComで使用 -----
    rRS_CndNum = 900                                    '900:加工条件番号エラー
    rRS_Trap = 999                                      '999:トラップエラー発生
    
End Enum

'==============================================================================
'  機能: RS232Cポートのオープン
'　引数: pstCom (INP): ポート情報
'　戻値: 0     = 正常
'        0以外 = エラー
'==============================================================================
Public Function Rs232cFL_Open(pstCom As ComInfo) As Long

    On Error GoTo STP_ERR
    Dim r           As Long

    ' 初期処理
    Rs232cFL_Open = rRS_OK                              ' Return値 = 正常
    Set ObjPortInfo = New DllSerialIO.PortInformation   ' ﾎﾟｰﾄ情報ｵﾌﾞｼﾞｪｸﾄ生成
    Set ObjFLCom = New DllFLCom.FLComIO                 ' トリマー加工条件送受信ｵﾌﾞｼﾞｪｸﾄ生成
            
    ' ポート情報を設定する
    ObjPortInfo.PortName = pstCom.PortName              ' ポート番号
    ObjPortInfo.BaudRate = pstCom.BaudRate              ' Speed
    ObjPortInfo.Parity = pstCom.Parity                  ' Parity(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
    ObjPortInfo.DataBits = pstCom.DataBits              ' Char Data
    ObjPortInfo.StopBits = pstCom.StopBits              ' Stop Bit
    
    ' ポートオープン
    r = ObjFLCom.Serial_Open(ObjPortInfo)               ' ポートオープン
    If (r <> rRS_OK) Then
        Rs232cFL_Open = r                               ' Return値設定
    End If
    Rs_Flag = 1                                         ' ﾌﾗｸﾞ=1(ｵｰﾌﾟﾝ済)
    Exit Function
    
    ' トラップエラー発生時
STP_ERR:
    mFncName = "Rs232c_FL.Rs232cFL_Open()"
    Call gObjUtl.SystemErrLog(mModName, mFncName, err.Number, err.Description)
    err.Clear
    Rs232cFL_Open = rRS_Trap                            ' Return値 = トラップエラー発生

End Function

'==============================================================================
'  機能: RS232Cポートのクローズ
'　引数: なし
'　戻値: 0     = 正常
'        0以外 = エラー
'==============================================================================
Public Function Rs232cFL_Close() As Long

    On Error GoTo STP_ERR
    Dim r           As Long
     
    ' ポートクローズ
    Rs232cFL_Close = rRS_OK                             ' Return値 = 正常
    If (Rs_Flag = 0) Then Exit Function                 ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ)ならNOP
    r = ObjFLCom.Serial_Close()                         ' ポートクローズ
    Rs232cFL_Close = r                                  ' Return値設定
    Rs_Flag = 0                                         ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ)

STP_END:
    Set ObjPortInfo = Nothing                           ' ﾎﾟｰﾄ情報ｵﾌﾞｼﾞｪｸﾄ解放
    Set ObjFLCom = Nothing                              ' トリマー加工条件送受信ｵﾌﾞｼﾞｪｸﾄ解放
    Exit Function

STP_ERR:
    Rs232cFL_Close = rRS_Trap                           ' Return値 = トラップエラー発生
    GoTo STP_END
    
End Function

'==============================================================================
'  機能: シリアルポートへトリマー加工条件を個別に送信する
'  引数: CndNum (INP): 条件番号
'        Curr   (INP): 電流値
'        Freq   (INP): 周波数
'        Steg   (INP): STEG波形
'　戻値: 0     = 正常
'        0以外 = エラー
'==============================================================================
Public Function RsSendBankInfo(CndNum As Long, Curr As Long, Freq As Double, Steg As Long) As Long

    Dim r           As Long
    Dim wkFreq      As Long
    Dim dblWK       As Long

    ' 初期処理
    On Error GoTo STP_ERR
    RsSendBankInfo = rRS_OK                             ' Return値 = 正常
    If (Rs_Flag = 0) Then                               ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ) ?
        RsSendBankInfo = rRS_NotOpen                    ' Return値 = ｼﾘｱﾙﾎﾟｰﾄがｵｰﾌﾟﾝしていない
        Exit Function
    End If
        
    ' 周波数(KHz)を繰返し間隔に変換する
    ' 繰返し間隔N = 繰返し時間/200ns 例)10KHzの場合は、500(0x01f4)を送信する。
    If (Freq <= 0) Then
        wkFreq = 0
    Else
        dblWK = 1000000 / 200 / Freq                    '###803①(OcxSystem)
        wkFreq = dblWK                                  '###803①(OcxSystem)
'       wkFreq = 1000000 / 200 / Freq                   '###803①(OcxSystem)
    End If
    
    ' トリマー加工条件送信
    r = ObjFLCom.Serial_SendBankInfo(CndNum, Curr, wkFreq, Steg)
    If (r <> rRS_OK) Then                               ' 送信エラー ?
        RsSendBankInfo = r                              ' Return値設定
        Exit Function
    End If
    Exit Function

STP_ERR:
    RsSendBankInfo = rRS_Trap                           ' Return値 = トラップエラー発生

End Function

'==============================================================================
'  機能: シリアルポートへトリマー加工条件を一括で送信する
'  引数: Curr() (INP): 電流値
'        Freq() (INP): 周波数
'        Steg() (INP): STEG波形
'　戻値: 0     = 正常
'        0以外 = エラー
'==============================================================================
Public Function RsSendBankALL(Curr() As Long, Freq() As Double, Steg() As Long) As Long

    Dim r           As Long
    Dim i           As Long

    ' 初期処理
    On Error GoTo STP_ERR
    RsSendBankALL = rRS_OK                              ' Return値 = 正常
    If (Rs_Flag = 0) Then                               ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ) ?
        RsSendBankALL = rRS_NotOpen                     ' Return値 = ｼﾘｱﾙﾎﾟｰﾄがｵｰﾌﾟﾝしていない
        Exit Function
    End If
        
    ' シリアルポートへトリマー加工条件を送信する
    For i = 0 To (MAX_BANK_NUM - 1)                     ' 最大加工条件数分送信する
        r = RsSendBankInfo(i, Curr(i), Freq(i), Steg(i))
        If (r <> rRS_OK) Then                           ' 送信エラー ?
            RsSendBankALL = r                           ' Return値設定
            Exit Function
        End If
    Next i
    Exit Function

STP_ERR:
    RsSendBankALL = rRS_Trap                            ' Return値 = トラップエラー発生

End Function

'==============================================================================
'  機能: シリアルポートからトリマー加工条件を個別に受信する
'  引数: CndNum (INP): 条件番号
'        Curr   (OUT): 電流値(mA)
'        Freq   (OUT): 周波数(KHz)
'        Steg   (OUT): STEG波形(0-15)
'        TimeOut(INP): 応答待タイマ値(ms)
'　戻値: 0     = 正常
'        0以外 = エラー
'==============================================================================
Public Function RsReceiveBankInfo(CndNum As Long, Curr As Long, Freq As Double, Steg As Long, timeout As Long) As Long

    Dim r           As Long
    Dim wkFreq      As Long
    Dim dblWK       As Double
    Dim dblWK2      As Double
    Dim strMSG      As String

    ' 初期処理
    On Error GoTo STP_ERR
    RsReceiveBankInfo = rRS_OK                          ' Return値 = 正常
    If (Rs_Flag = 0) Then                               ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ) ?
        RsReceiveBankInfo = rRS_NotOpen                 ' Return値 = ｼﾘｱﾙﾎﾟｰﾄがｵｰﾌﾟﾝしていない
        Exit Function
    End If
        
    ' トリマー加工条件受信
    r = ObjFLCom.Serial_ReceiveBankInfo(CndNum, Curr, wkFreq, Steg, timeout)
    If (r <> rRS_OK) Then                               ' 送信エラー ?
        RsReceiveBankInfo = r                           ' Return値設定
        Exit Function
    End If
    
    ' 繰返し間隔を周波数(KHz)に変換する
    If (wkFreq <= 0) Then
        Freq = 0
    Else
        '###803①(OcxSystem)↓
        dblWK = wkFreq * 200
        dblWK2 = 1000000
        Freq = dblWK2 / dblWK
        strMSG = Format((Freq * 10), "0.0")             ' 小数点２位以下を切り捨てる
        Freq = CDbl(strMSG)
        Freq = Freq / 10
        '###803①(OcxSystem)↑
    
'        '###801④(OcxLaser) ↓       Freq = 1000000 / (wkFreq * 200)
'        Freq = 1000000 / (wkFreq * 200)
'        wkFreq = Fix(10 * Freq)
'        Freq = wkFreq / 10
'        '###801④(OcxLaser) ↑
        
    End If
    Exit Function

STP_ERR:
    RsReceiveBankInfo = rRS_Trap                        ' Return値 = トラップエラー発生

End Function

'==============================================================================
'  機能: シリアルポートからトリマー加工条件を一括で受信する
'  引数: Curr   (OUT): 電流値(mA)
'        Freq   (OUT): 周波数(KHz)
'        Steg   (OUT): STEG波形(0-15)
'        TimeOut(INP): 応答待タイマ値(ms)
'　戻値: 0     = 正常
'        0以外 = エラー
'==============================================================================
Public Function RsReceiveBankALL(Curr() As Long, Freq() As Double, Steg() As Long, timeout As Long) As Long

    Dim r           As Long
    Dim i           As Long

    ' 初期処理
    On Error GoTo STP_ERR
    RsReceiveBankALL = rRS_OK                           ' Return値 = 正常
    If (Rs_Flag = 0) Then                               ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ) ?
        RsReceiveBankALL = rRS_NotOpen                  ' Return値 = ｼﾘｱﾙﾎﾟｰﾄがｵｰﾌﾟﾝしていない
        Exit Function
    End If
    
    ' シリアルポートからトリマー加工条件を一括で受信する
    For i = 0 To (MAX_BANK_NUM - 1)                     ' 最大加工条件数分受信する
        r = RsReceiveBankInfo(i, Curr(i), Freq(i), Steg(i), timeout)
        If (r <> rRS_OK) Then                           ' エラー ?
            RsReceiveBankALL = r                        ' Return値設定
            Exit Function
        End If
    Next i
    
     ' FL側の設定があるか確認する
    For i = 0 To (MAX_BANK_NUM - 1)                     ' 最大加工条件数分受信する
        If (Curr(i) <> 0) Then                          ' 電流値設定0以外が見つかったらFL側の設定があると判断する
            RsReceiveBankALL = rRS_OK                   ' Return値 = 正常
            Exit Function
        End If
    Next i
    RsReceiveBankALL = rRS_FLCND_NONE                   ' Return値 = 加工条件の設定なし
   
    Exit Function

STP_ERR:
    RsReceiveBankALL = rRS_Trap                         ' Return値 = トラップエラー発生

End Function

'==============================================================================
'  機能: シリアルポートからエラー情報を受信する
'  引数: ErrInf (OUT): エラー情報
'        TimeOut(INP): 応答待タイマ値(ms)
'　戻値: 0     = 正常
'        0以外 = エラー
'==============================================================================
Public Function Rs232cFL_ReceiveErrInfo(ErrInf As Long, timeout As Long) As Long

    Dim r           As Long
    Dim wkFreq      As Long

    ' 初期処理
    On Error GoTo STP_ERR
    Rs232cFL_ReceiveErrInfo = rRS_OK                    ' Return値 = 正常
    If (Rs_Flag = 0) Then                               ' ﾌﾗｸﾞ=0(未ｵｰﾌﾟﾝ) ?
        Rs232cFL_ReceiveErrInfo = rRS_NotOpen           ' Return値 = ｼﾘｱﾙﾎﾟｰﾄがｵｰﾌﾟﾝしていない
        Exit Function
    End If
        
    ' エラー情報受信
    r = ObjFLCom.Serial_ReceiveErrInfo(ErrInf, timeout)
    If (r <> rRS_OK) Then                               ' 受信エラー ?
        Rs232cFL_ReceiveErrInfo = r                     ' Return値設定
        Exit Function
    End If
    
    Exit Function

STP_ERR:
    Rs232cFL_ReceiveErrInfo = rRS_Trap                  ' Return値 = トラップエラー発生

End Function

'==============================================================================
'  機能: バージョンの取得
'　引数: strVER (OUT): strVER (0) = DllSerialIO.dllのバージョン
'　　　　　　　　　　: strVER (1) = DllCndXMLIO.dllのバージョン
'　　　　　　　　　　: strVER (2) = DllFLCom.dllのバージョン
'　戻値: なし
'==============================================================================
Public Sub Rs232cFL_GetVersion(strVER() As String)

    On Error GoTo STP_ERR
    Dim iMajor      As Long                             ' Major Version
    Dim iMinor      As Long                             ' Minor Version
    Dim iBNum       As Long                             ' Build Number
    Dim iRev        As Long                             ' Revision
    
    ' バージョン情報クラスオブジェクト生成
    Set ObjPortCVer = New DllSerialIO.VersionInformation
    Set ObjFLXMLVer = New DllCndXMLIO.VersionInformation
    Set ObjFLComVer = New DllFLCom.VersionInformation
    
    ' バージョンの取得("Vx.x.x.x"の形式で返す)
    Call ObjPortCVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    strVER(0) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")
    
    Call ObjFLXMLVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    strVER(1) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")
    
    Call ObjFLComVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    strVER(2) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")
    
    ' バージョン情報クラスオブジェクト開放
    Set ObjPortCVer = Nothing
    Set ObjFLXMLVer = Nothing
    Set ObjFLComVer = Nothing

STP_ERR:

End Sub

