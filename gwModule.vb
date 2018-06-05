'===============================================================================
'   Description  : INtime側インターフェース定義
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.IO                       'V4.4.0.0-0
Imports System.Runtime.InteropServices
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module gwModule
#Region "定数/変数定義"
    '===========================================================================
    '   定数/変数定義
    '===========================================================================
    Public gmhTky As System.Threading.Mutex = New System.Threading.Mutex(False, "iTKY")

    '-------------------------------------------------------------------------------
    '   ﾗﾝﾌﾟ ON/OFF制御用ﾗﾝﾌﾟ番号
    '-------------------------------------------------------------------------------
    'SL43xR向け
    Public Const LAMP_START As Short = 0
    Public Const LAMP_RESET As Short = 1
    Public Const LAMP_Z As Short = 2
    Public Const LAMP_HALT As Short = 5
    'Public Const LAMP_READY As Short = 0
    'Public Const LAMP_START As Short = 1
    'Public Const LAMP_HALT As Short = 2
    'Public Const LAMP_RESET As Short = 3
    'Public Const LAMP_Z As Short = 5

    '-------------------------------------------------------------------------------
    '   Interlock switch bits (ADR. 0x21E8)
    '-------------------------------------------------------------------------------
    Public Const BIT_SLIDECOVER_CLOSE As Short = &H100S     ' B8 : ｽﾗｲﾄﾞｶﾊﾞｰ閉
    Public Const BIT_SLIDECOVER_OPEN As Short = &H200S      ' B9 : ｽﾗｲﾄﾞｶﾊﾞｰ開
    Public Const BIT_EMERGENCY_SW As Short = &H400S         ' B10: ｴﾏｰｼﾞｪﾝｼｰSW
    Public Const BIT_EMERGENCY_RESET As Short = &H800S      ' B11: ｴﾏｰｼﾞｪﾝｼｰﾘｾｯﾄ
    Public Const BIT_INTERLOCK_DISABLE As Short = &H1000S   ' B12: ｲﾝﾀｰﾛｯｸ解除SW
    Public Const BIT_SERVO_ALARM As Short = &H2000S         ' B13: ｻｰﾎﾞｱﾗｰﾑ
    Public Const BIT_COVER_CLOSE As Short = &H4000S         ' B14: ｶﾊﾞｰ閉
    '                                                       ' B15: ｶﾊﾞｰ&ｽﾗｲﾄﾞｶﾊﾞｰ閉

    Public Const INTERLOCK_STS_DISABLE_NO As Short = (0)    ' インターロック状態(解除なし)
    Public Const INTERLOCK_STS_DISABLE_PART As Short = (1)  ' インターロック一部解除（ステージ可動可能）
    Public Const INTERLOCK_STS_DISABLE_FULL As Short = 2    ' インターロック全解除
    Public Const SLIDECOVER_OPEN As Short = (1)             ' Bit0 : スライドカバー：オープン
    Public Const SLIDECOVER_CLOSE As Short = (2)            ' Bit1 : スライドカバー：クローズ
    Public Const SLIDECOVER_MOVING As Short = (4)           ' Bit2 : スライドカバー：動作中

    '-------------------------------------------------------------------------------
    '   シグナルタワー３色制御(標準)SL432R/SL436R共通       ' ###007
    '   ①手動運転中 ･････････････ 無点灯(原点復帰完了, レディ(手動))
    '   ②インターロック解除中････ 黄点滅(H/Wで制御)
    '   ③ティーチング中･･････････ 黄点灯(但し②優先)
    '   ④原点復帰時 ･････････････ 緑点滅
    '   ⑤非常停止中 ･････････････ 赤点灯＋ブザーＯＮ ← H/Wが落ちる為なし
    '   ⑥自動運転中 ･････････････
    '     ａ)正常運転時　　　：緑点灯
    '     ｂ)全マガジン終了時：赤点滅＋ブザーＯＮ
    '     ｃ)アラーム時　　　：赤点滅＋ブザーＯＮ（但し、⑤項優先）
    '-------------------------------------------------------------------------------
    '----- OUTPUT -----                                     ' ON時の意味
    '                                                       ' B0 : 未使用
    '                                                       ' :
    '                                                       ' B7 : 未使用
    Public Const SIGOUT_GRN_ON As UShort = &H100            ' B8 : 緑点灯  (自動運転中)
    Public Const SIGOUT_YLW_ON As UShort = &H200            ' B9 : 黄点灯  (ティーチング中)(インターロック解除中は黄点滅)　'###024
    Public Const SIGOUT_RED_ON As UShort = &H400            ' B10: 赤点灯  (非常停止) ※未使用(H/Wで制御)
    Public Const SIGOUT_GRN_BLK As UShort = &H800           ' B11: 緑点滅  (原点復帰時)
    Public Const SIGOUT_YLW_BLK As UShort = &H1000          ' B12: 黄点滅  (インターロック解除中)　'###024
    Public Const SIGOUT_RED_BLK As UShort = &H2000          ' B13: 赤点滅  (異常/全マガジン終了) ※+ブザー１
    Public Const SIGOUT_BZ1_ON As UShort = &H4000           ' B14: ブザー１(異常) ※+赤点滅
    '                                                       ' B15: 未使用

    '-------------------------------------------------------------------------------
    '   拡張ＥＸＴＢＩＴ(上位16ビット ADR. 213A)
    '   シグナルタワー４色制御用ビット ※日立ｵｰﾄﾓｰﾃｨﾌﾞ殿特注
    '-------------------------------------------------------------------------------
    '----- OUTPUT -----                                     ' ON時の意味
    '                                                       ' B0 (B16): 未使用
    '                                                       ' :
    '                                                       ' B3 (B19): 未使用
    '                                                       ' B4 (B20): 未使用
    '                                                       ' :
    '                                                       ' B7 (B23): 未使用

    Public Const EXTOUT_RED_ON As UShort = &H100            ' B8 (B24): 赤点灯  (非常停止) ※未使用(H/Wで制御)
    Public Const EXTOUT_RED_BLK As UShort = &H200           ' B9 (B25): 赤点滅  (異常) ※+ブザー１
    Public Const EXTOUT_YLW_ON As UShort = &H400            ' B10(B26): 黄色点灯(原点復帰完了, レディ(手動))
    Public Const EXTOUT_YLW_BLK As UShort = &H800           ' B11(B27): 黄色点滅(原点復帰中)

    Public Const EXTOUT_GRN_ON As UShort = &H1000           ' B12(B28): 緑点灯  (自動運転中)
    Public Const EXTOUT_GRN_BLK As UShort = &H2000          ' B13(B29): 緑点滅  (-) ※未使用
    Public Const EXTOUT_BZ1_ON As UShort = &H4000           ' B14(B30): ブザー１(異常) ※+赤点滅
    '                                                       ' B15(B31): 未使用
    '----- V1.18.0.1⑦↓ -----
    '-------------------------------------------------------------------------------
    '   ＥＸＴＢＩＴ(216A BIT4-7) ※ローム殿特注(SL436R/SL436S)
    '-------------------------------------------------------------------------------
    '----- OUTPUT -----                                     ' ON時の意味(SL436Rの場合)
    Public EXTOUT_EX_YLW_ON As UShort = &H10                ' B4 : 黄点灯(レーザ照射中)
    Public EXTOUT_EX_LOK_ON As UShort = &H20                ' B5 : 電磁ロック(観音扉右側ロック)

    '----- INPUT -----                                      ' ON時の意味(SL436Rの場合)
    Public EXTINP_EX_LOK_ON As UShort = &H10                ' B4 : 電磁ロック(観音扉右側ロック)

    '----- その他 -----
    Public EX_LOK_STS As Integer = &H216A                   ' 電磁ロックステータスアドレス(SL436Rの場合)
    Public EX_LOK_TOUT As Integer = (10 * 1000)             ' 電磁ロックステータスタイムアウト値(msec)

    '----- EL_Lock_OnOff関数のモード -----
    Public EX_LOK_MD_ON As Integer = 1                      ' 電磁ロックON
    Public EX_LOK_MD_OFF As Integer = 0                     ' 電磁ロックOFF

    '----- V1.18.0.1⑦↑ -----
    'V5.0.0.6①↓拡張I/O汎用定義
    Public Const EXT_IN0 As UShort = &H10                   ' B04: ユーザ割付１
    Public Const EXT_IN1 As UShort = &H20                   ' B05: ユーザ割付２
    Public Const EXT_IN2 As UShort = &H40                   ' B06: ユーザ割付３
    Public Const EXT_IN3 As UShort = &H80                   ' B07: ユーザ割付４
    'V5.0.0.6①↑

    '-------------------------------------------------------------------------------
    '   SL43xR向け
    '-------------------------------------------------------------------------------
    ' Switch状態
    Public Const cSTS_NOSW_ON As Integer = 0                ' スイッチの入力なしもしくは、ON後にキャンセル
    Public Const cSTS_STARTSW_ON As Integer = 1             ' STARTスイッチON
    Public Const cSTS_RESETSW_ON As Integer = 3             ' RESETスイッチON
    Public Const cSTS_HALTSW_ON As Integer = 4              ' HALTスイッチON

    'スライドカバー/固定カバーチェック状態
    Public Const SLIDECOVER_CHECK_OFF As Integer = 1        ' スライドカバーチェックを行わない。→SL436R用設定
    Public Const SLIDECOVER_CHECK_ON As Integer = 0         ' スライドカバーチェックを行う。
    Public Const COVER_CHECK_OFF As Integer = 1             ' 固定カバーチェックを行わない  ###088
    Public Const COVER_CHECK_ON As Integer = 0              ' 固定カバーチェックを行う      ###088

    '---------------------------
    ' ロギング関係
    '---------------------------
    'セクション名
    Public Const cTRIMLOGcSECTNAME As String = "TRIMMODE_FILELOG_DATA_SET"
    Public Const cMEASLOGcSECTNAME As String = "MEASMODE_FILELOG_DATA_SET"
    Public Const cTRIMDISP1xcSECTNAME As String = "TRIMMODE1x_DISPLOG_DATA_SET"
    Public Const cTRIMDISPx0cSECTNAME As String = "TRIMMODEx0_DISPLOG_DATA_SET"
    Public Const cTRIMDISPx1cSECTNAME As String = "TRIMMODEx1_DISPLOG_DATA_SET"
    Public Const cMEASDISPcSECTNAME As String = "MEASMODE_DISPLOG_DATA_SET"
    'キー名
    Public Const cDATACOUNTcKEYNAME As String = "DATACOUNT"

    Public m_TrimlogFileFormat(20) As Integer           ' トリミング時-ログファイルへの出力対象＆順番設定変数
    Public m_MeaslogFileFormat(20) As Integer           ' 測定時-ログファイルへの出力対象＆順番設定変数
    Public m_TrimDisp1xFormat(20) As Integer            ' トリミング1x時-ログ表示出力対象＆順番設定変数
    Public m_TrimDispx0Format(20) As Integer            ' トリミングx0時-ログ表示出力対象＆順番設定変数
    Public m_TrimDispx1Format(20) As Integer            ' トリミングx1時-ログ表示出力対象＆順番設定変数
    Public m_MeasDispFormat(20) As Integer              ' 測定時-ログファイルへの出力対象＆順番設定変数
#If False Then
    'ログターゲット定数
    Public Const LOGTAR_NOSET As Short = 0
    Public Const LOGTAR_DATE As Short = 1
    Public Const LOGTAR_MODE As Short = 2
    Public Const LOGTAR_LOTNO As Short = 3
    Public Const LOGTAR_CIRCUIT As Short = 4
    Public Const LOGTAR_RESISTOR As Short = 5
    Public Const LOGTAR_JUDGE As Short = 6
    Public Const LOGTAR_TARGET As Short = 7
    Public Const LOGTAR_INITIAL As Short = 8
    Public Const LOGTAR_FINAL As Short = 9
    Public Const LOGTAR_DEVIATION As Short = 10
    Public Const LOGTAR_UCUTPRMNO As Short = 11
    ''V4.1.0.0⑦   Public Const LOGTAR_END As Short = 99
    Public Const LOGTAR_END As Short = 12 ' 現状使用しているループ回数にする
#Else
    'ログターゲット列挙体
    Public Enum LOGTAR As Integer       '#4.12.2.0⑥
        NOSET
        [DATE]
        MODE
        LOTNO
        BLOCKX
        BLOCKY
        CIRCUIT
        RESISTOR
        JUDGE
        TARGET
        INITIAL
        FINAL
        DEVIATION
        UCUTPRMNO
        [END]
    End Enum
#End If

    'ログタイプ定数
    Public Const LOGTYPE_DISP As Short = 1
    Public Const LOGTYPE_FILE As Short = 2

    '-------------------------------------------------------------------------------
    '   ローダーＩ／Ｏビット(SL432R用 ADR. 219A) '###035
    '-------------------------------------------------------------------------------
    '----- トリマー  → ローダー -----
    ' ※Bit0～Bit4,Bit7が標準版
    Public Const COM_STS_TRM_STATE As UShort = &H1US        ' B0 : トリマ停止(0:停止,1:動作中)
    Public Const COM_STS_TRM_NG As UShort = &H2US           ' B1 : トリミングＮＧ(0:正常, 1:NG)
    Public Const COM_STS_PTN_NG As UShort = &H4US           ' B2 : パターン認識エラー(0:正常, 1:エラー)
    Public Const COM_STS_TRM_ERR As UShort = &H8US          ' B3 : トリマエラー(0:正常, 1:エラー)
    Public Const COM_STS_TRM_READY As UShort = &H10US       ' B4 : トリマレディ(0:ﾉｯﾄﾚﾃﾞｨ, 1:ﾚﾃﾞｨ)
    Public Const COM_STS_LOT_END As UShort = &H20US         ' B5 : ﾛｯﾄ終了信号(※ROHM特注)
    '                                                       ' B5 : 未使用
    Public Const COM_STS_TRM_COMP_SUPPLY As Short = &H40S   ' B6 : 供給位置移動完了'V5.0.0.6②
    Public Const COM_STS_ABS_ON As UShort = &H80US          ' B7 : 載物台ｸﾗﾝﾌﾟ開閉(0:閉, 1:開)

    '----- ローダー  → トリマー -----
    ' ※Bit0～Bit3までが標準版
    Public Const cHSTcRDY As UShort = &H1US                 ' B0 : ｵｰﾄﾛｰﾀﾞｰ有無(0:無, 1:有)
    Public Const cHSTcAUTO As UShort = &H2US                ' B1 : ｵｰﾄﾛｰﾀﾞｰﾓｰﾄﾞ(1=自動ﾓｰﾄﾞ, 0=手動ﾓｰﾄﾞ)
    Public Const cHSTcSTATE As UShort = &H4US               ' B2 : ｵｰﾄﾛｰﾀﾞｰ動作中(0=動作中, 1=停止)
    Public Const cHSTcTRMCMD As UShort = &H8US              ' B3 : ﾄﾘﾏｰｽﾀｰﾄ(1=ﾄﾘﾏｰｽﾀｰﾄ) ※ﾗｯﾁ

    '----- ローダ入出力関連(SL432R系用ｵﾌﾟｼｮﾝ) -----
    Public giHostMode As Short                              ' ﾛｰﾀﾞﾓｰﾄﾞ(0:手動ﾓｰﾄﾞ, 1:自動ﾓｰﾄﾞ)
    Public giHostMode_tmp As Short                          ' ﾛｰﾀﾞ運転ﾓｰﾄﾞ(ｱﾗｰﾑ停止ﾄｰﾀﾙ時間用)
    Public Const cHOSTcMODEcAUTO As Short = 1               '  1:自動ﾓｰﾄﾞ
    Public Const cHOSTcMODEcMANUAL As Short = 0             '  0:手動ﾓｰﾄﾞ
    Public gbHostConnected As Boolean                       ' ホスト接続状態(True=接続(ﾛｰﾀﾞ有), False=未接続(ﾛｰﾀﾞ無))
    Public giHostRun As Short                               ' ﾛｰﾀﾞ動作中(0:停止, 1:動作中)
    Public gdwATLDDATA As UInteger                          ' ローダ出力データ
    Public gDebugHostCmd As UInteger                        ' ローダ入力データ(ﾃﾞﾊﾞｯｸﾞ用)
    Public gwPrevHcmd As UInteger                           ' ローダ入力データ退避域

    Public Const cLoggingIT As Short = 1
    Public Const cLoggingFT As Short = 2

    '----- 最大値/最小値 -----
    '' '' ''Public Const cMAXcMARKINGcSTRLEN As Integer = 18        ' マーキング文字列最大長(byte)
    '' '' ''Public Const cMAXcSENDcPRMCNT As Integer = 32           ' VB→INTRIMの送信コマンドパラメータ最大数
    '' '' ''Public Const cResultMax As Integer = 256                ' トリミング結果データの最大配列数
    '' '' ''Public Const cResultAry As Integer = 999                ' トリミング結果データの最大数
    '' '' ''Public Const cAxisMAX As Integer = 4                    ' 最大軸数
    '' '' ''Public Const cRsultTky As Integer = 4                   ' TKY戻り値
    '' '' ''Public Const cRetAxisBpPos As Integer = 5               ' 各軸、BPの現在値

    ' OCSLLN  CONSOLE OUT PORT
    Public Const OCSLLN_SLIDECOVER_OPEN As Short = 8 'b8
    Public Const OCSLLN_SLIDECOVER_CLOSE As Short = 9 'b9
    Public Const OCSLLN_ABS_VACCUME As Short = 10 'b10

    ' INPUT OUTPUT
    Public Const INP_MAX As Short = 5
    Public Const INP_ICSLSS As Short = 0 ' [0]:コンソールSWセンス
    Public Const INP_IITLKS As Short = 1 ' [1]:インターロック関係SWセンス
    Public Const INP_AUTLODL As Short = 2 ' [2]:オートローダLO
    Public Const INP_AUTLODH As Short = 3 ' [3]:オートローダHI
    Public Const INP_ATTNATE As Short = 4 ' [4]:固定アッテネータ

    'Global Const OUT_MAX = 5
    Public Const OUT_MAX As Short = 4
    Public Const OUT_OCSLLN As Short = 0 ' [0]:コンソール制御
    Public Const OUT_OSYSCTL As Short = 1 ' [1]:サーボパワー
    Public Const OUT_AUTLODL As Short = 2 ' [2]:オートローダLO
    Public Const OUT_AUTLODH As Short = 3 ' [3]:オートローダHI
    Public Const OUT_SIGNALT As Short = 4 ' [4]:シグナルタワー

    Public Const OUT_CONSLOE As Short = 0

    ' ERROR STATUS
    Public Const ERR_SUCCESS As Short = 0 ' エラーなし
    'Public Const ERR_EMGSWCH As Short = 1 ' 非常停止
    'Public Const ERR_SRV_ALM As Short = 2 ' サーボアラーム

    ' 原点位置象限
    Public Const DIMENSION_1 As Short = 0 ' 第1象現(x>0,y>0)    2 | 1
    Public Const DIMENSION_2 As Short = 1 ' 第2象現(x<0,y>0) -----+------
    Public Const DIMENSION_3 As Short = 2 ' 第3象現(x>0,y<0)    4 | 3
    Public Const DIMENSION_4 As Short = 3 ' 第4象現(x<0,y<0)

    ' パワー測定(NET用)
    Private Const READ_MAX As Integer = 20              ' パワー読み取り時の値を得るための測定回数
    Private Const READ_DEL As Integer = 5               ' パワー読み取り時の値を得るためにＭＡＸ，ＭＩＮから除く個数
    Private Const READ_OK_WID As Integer = 333 * 2      ' パワー読み取り時のばらつき許容範囲［モニタ入力ビット値］

    Public Const FULL_POWER_LIMIT As Double = 0.5       ' フルパワー減衰許容値(±0.5W)

    ' 次ﾌﾞﾛｯｸへの移動位置取得用の構造体形式定義
    Public Structure TRIM_GETNEXTXY
        Dim intCDir As Short                            ' ﾁｯﾌﾟ方向
        Dim intStepR As Short                           ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ
        Dim dblstgx As Double                           ' ｽﾃｰｼﾞX位置
        Dim dblstgy As Double                           ' ｽﾃｰｼﾞY位置
        Dim dblCSx As Double                            ' ﾁｯﾌﾟｻｲｽﾞX
        Dim dblCSy As Double                            ' ﾁｯﾌﾟｻｲｽﾞY
        Dim intxy1 As Short                             ' ﾌﾞﾛｯｸ位置
        Dim intxy2 As Short                             ' ﾌﾞﾛｯｸ位置
        Dim dblgspacex As Double                        ' ｸﾞﾙｰﾌﾟ間隔X
        Dim dblgspacey As Double                        ' ｸﾞﾙｰﾌﾟ間隔Y
        Dim intpxy1 As Short                            ' ﾌﾟﾚｰﾄ位置
        Dim intpxy2 As Short                            ' ﾌﾟﾚｰﾄ位置
        Dim dblblockx As Double                         ' ﾌﾞﾛｯｸX
        Dim dblblocky As Double                         ' ﾌﾞﾛｯｸY
        Dim dblStepOffX As Double                       ' ｽﾃｯﾌﾟｵﾌｾｯﾄX
        Dim dblStepOffY As Double                       ' ｽﾃｯﾌﾟｵﾌｾｯﾄY
        Dim dblStrp As Double                           ' ステップインターバル
        Dim dblblockIntervalx As Double                 ' ﾌﾞﾛｯｸ間隔X
        Dim dblblockIntervaly As Double                 ' ﾌﾞﾛｯｸ間隔Y
        Dim intBlockCntXDir As Short                    ' ﾌﾞﾛｯｸ数Ｘ
        Dim intBlockCntYDir As Short                    ' ﾌﾞﾛｯｸ数Ｙ
        '----- <CHIPのみ> -----
        Dim intArrayCntX As Short                       ' 配列数
        Dim intArrayCntY As Short                       ' 配列数
        '----- <NETのみ> -----
        Dim dblADDSZX As Double
        Dim dblADDSZY As Double
        Dim intptxy1 As Short                           ' ﾌﾟﾚｰﾄ位置
        Dim intptxy2 As Short                           ' ﾌﾟﾚｰﾄ位置
        Dim dblptspacex As Double                       ' ｸﾞﾙｰﾌﾟ間隔X
        Dim dblptspacey As Double                       ' ｸﾞﾙｰﾌﾟ間隔Y
    End Structure

    ' オートローダ関連データ項目
    Public Structure ATLD_DATA
        Dim iLED As Short                               ' バックライト照明(0:常時ＯＮ,1:常時ＯＦＦ,2:画像認識時)
        Dim iLOT As Short                               ' ロット切換(0:OFF,1:ON)
    End Structure
    Public stATLD As ATLD_DATA                          ' オートローダ関連データ

    '-------------------------------------------------------------------------------
    '   ファイバーレーザ用定義
    '-------------------------------------------------------------------------------
    '----- 発振器種別 -----
    Public Const OSCILLATOR_FL As Integer = 3           ' FL(ﾌｧｲﾊﾞｰﾚｰｻﾞ)

    '----- トリマー加工条件用定義 -----
    'V5.0.0.8①    Public Const cCNDNUM As Integer = 8                 ' 1ｶｯﾄの最大加工条件数
    '#5.0.0.8①    '                                                   ' 加工条件構造体のインデックス
    '#5.0.0.8①    Public Const CUT_CND_L1 As Integer = 0              ' L1加工条件設定
    '#5.0.0.8①    Public Const CUT_CND_L2 As Integer = 1              ' L2加工条件設定
    '#5.0.0.8①    Public Const CUT_CND_L3 As Integer = 2              ' L3加工条件設定
    '#5.0.0.8①    Public Const CUT_CND_L4 As Integer = 3              ' L4加工条件設定
    '#5.0.0.8①    Public Const CUT_CND_L1_RET As Integer = 4          ' L1_Return/Retrace加工条件設定
    '#5.0.0.8①    Public Const CUT_CND_L2_RET As Integer = 5          ' L2_Return/Retrace加工条件設定
    '#5.0.0.8①    Public Const CUT_CND_L3_RET As Integer = 6          ' L3_Return/Retrace加工条件設定
    '#5.0.0.8①    Public Const CUT_CND_L4_RET As Integer = 7          ' L4_Return/Retrace加工条件設定

    Public stCND As TrimCondInfo                        ' トリマー加工条件(形式定義はRs232c.vb参照)

    '----- 加工条件番号31(フルパワー測定用)退避域 ----- ' ###072
    Public FLCnd31Curr As Integer                       ' 電流値(mA)
    Public FLCnd31Freq As Double                        ' 周波数(KHz)
    Public FLCnd31Steg As Integer                       ' STEG波形

    '----- RS232Cポート情報定義
    Public stCOM As ComInfo                             ' ポート情報(形式定義はRs232c.vb参照)
    Public Const cTIMEOUT As Long = 10000               ' 応答待タイマ値(ms)

    '----- デフォルト -----
    'V6.0.0.1⑥    Public Const POWERADJUST_TARGET As Double = 1.0     ' 目標パワー(W)            LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_TAERGET を使用する
    'V6.0.0.1⑥    Public Const POWERADJUST_LEVEL As Double = 0.5      ' 許容範囲(±W)             LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_LEVELを 使用する
    'V6.0.0.1⑥    Public Const POWERADJUST_CURRENT As Short = 1200    ' 電流値(mA) V2.0.0.0_23   LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_CURRENT を使用する
    'V6.0.0.1⑥    Public Const POWERADJUST_STEG As Short = 10         ' STEG1～8   V2.0.0.0_23   LaserFront.Trimmer.TrimData.DataManager.DEFAULT_ADJUST_STEG を使用する

    '----- FL用パワー調整情報 ###066 -----
    Public Structure POWER_ADJUST_INFO                  ' FL用パワー調整情報形式定義
        Dim CndNumAry() As Integer                      ' 加工条件番号配列(0=無効, 1=有効) 
        Dim AdjustTargetAry() As Double                 ' 調整目標パワー配列 
        Dim AdjustLevelAry() As Double                  ' パワー調整許容範囲配列
        Dim AttInfoAry() As Integer                     ' 固定ATT情報(0:固定ATT Off, 1:固定ATT On)配列 V1.14.0.0②

        ' この構造体を初期化するには、"Initialize" を呼び出さなければなりません。 
        Public Sub Initialize()
            ReDim CndNumAry(MAX_BANK_NUM - 1)
            ReDim AdjustTargetAry(MAX_BANK_NUM - 1)
            ReDim AdjustLevelAry(MAX_BANK_NUM - 1)
            ReDim AttInfoAry(MAX_BANK_NUM - 1)          ' V1.14.0.0②
        End Sub
    End Structure
    Public stPWR As POWER_ADJUST_INFO                   ' FL用パワー調整情報 ###192 
    Public stPWR_LSR As POWER_ADJUST_INFO               ' FL用パワー調整情報(レーザコマンド用) V1.14.0.0②

    '----- その他 -----
    Public Const MINCUR_CND_NUM As Integer = 30         ' 加工条件番号(最小電流値設定用)
    Public Const ADJ_CND_NUM As Integer = 0             ' 加工条件番号(一時停止画面用) ###237

    '-------------------------------------------------------------------------------
    '   その他
    '-------------------------------------------------------------------------------
    Public gManualThetaCorrection As Boolean            ' シータ補正実行フラグ(True=シータ補正を実行する, False=シータ補正を実行した)
    '                                                   ' 補正モード=1(手動)で補正方法=1(1回のみ)の判定に使用

    '-------------------------------------------------------------------------------
    '   ロギング向け結果取得定数
    '-------------------------------------------------------------------------------
    Public Const RSLTTYP_TRIMJUDGE As Integer = 0           ' トリミング判定結果
    Public Const RSLTTYP_INTIAL_TEST As Integer = 1         ' イニシャルテスト結果
    Public Const RSLTTYP_FINAL_TEST As Integer = 2          ' ファイナルテスト結果
    Public Const RSLTTYP_RATIO_TARGET As Integer = 3        ' レシオ結果
    '----- V1.18.0.0⑥↓ -----
    Public Const RSLTTYP_CIRCUIT As Integer = 4             ' サーキット結果
    Public Const RSLTTYP_IX2_CUTCOUNT As Integer = 5        ' カット回数(IX2)
    Public Const RSLTTYP_IX2_MEAS As Integer = 6            ' 測定値(IX2)
    Public Const RSLTTYP_ES2_MEASCOUNT As Integer = 7       ' 測定値数(ES2)
    Public Const RSLTTYP_ES2_MEAS As Integer = 8            ' 測定値  (ES2)
    '----- V1.18.0.0⑥↑ -----

#End Region

#Region "トリミングデータ定義"
    '===========================================================================
    '   トリミング要求/応答データ定義
    '===========================================================================
    '----- 要求/応答データ(VB←→INtime) ----- ※以下は未使用のため不要
    Public AppInfo As S_CMD_DAT                         ' 要求データ(コマンド)(VB→INtime)
    Public ResInfo As S_RES_DAT                         ' 応答データ(コマンド)(VB←INtime)

    '----- トリミング要求データ(VB→INtime) -----
    '(2011/2/2)
    '   動的配列が構造体メンバにいる場合、初期化処理の呼び出しが必要になる。
    '   ローカルメンバで宣言後初期化実行しても、警告が消えないので、
    '   パブリックメンバとして構造体を宣言する。（詳細調査後可能ならローカルへ修正）
    'Public stTPLT As TRIM_PLATE_DATA                     ' プレートデータ
    'Public stTGPI As TRIM_DAT_GPIB                      ' GPIB設定データ
    Public stTGPI As TRIM_PLATE_GPIB                     ' GPIB設定データ ###002
    Public stTGPI2 As TRIM_PLATE_GPIB2                   ' GPIB設定データ ###229
    Public stTGPI3 As TRIM_PLATE_GPIB3                   ' 外部リレー切替機GPIB設定データ V6.1.1.0⑤

    'Public stTREG As PRM_RESISTOR                       ' 抵抗データ
    Public stTCUT As TRIM_CUT_DATA                       ' カットデータ
    'Public stTREG As TRIM_DAT_RESISTOR                  ' 抵抗データ
    'Public stTCUT As TRIM_DAT_CUT                       ' カットデータ
    '                                                   ' カットパラメータ 
    'Public stCutST As PRM_CUT_ST                        ' ST cutパラメータ
    'Public stCutL As PRM_CUT_L                          ' L cutパラメータ
    'Public stCutHK As PRM_CUT_HOOK                      ' HOOK cutパラメータ
    'Public stCutIX As PRM_CUT_INDEX                     ' INDEX cutパラメータ
    'Public stCutIX2 As PRM_CUT_INDEX                    ' INDEX2 cutパラメータ
    'Public stCutSC As PRM_CUT_SCAN                      ' SCAN cutパラメータ
    Public stCutMK As PRM_CUT_MARKING                   ' Letter Markingパラメータ
    ''Public stCutC As TRIM_DAT_CUT_C                     ' C cutパラメータ
    'Public stCutES As PRM_CUT_ES                        ' ES cutパラメータ
    ''Public stCutE2 As TRIM_DAT_CUT_ES2                  ' ES2 cutパラメータ
    ''Public stCutZ As TRIM_DAT_CUT_Z                     ' Z cut(NOP)パラメータ

    'Public stCutST As TRIM_DAT_CUT_ST                   ' ST cutパラメータ
    'Public stCutL As TRIM_DAT_CUT_L                     ' L cutパラメータ
    'Public stCutHK As TRIM_DAT_CUT_HOOK                 ' HOOK cutパラメータ
    'Public stCutIX As TRIM_DAT_CUT_INDEX                ' INDEX cutパラメータ
    'Public stCutIX2 As TRIM_DAT_CUT_INDEX               ' INDEX2 cutパラメータ
    'Public stCutSC As TRIM_DAT_CUT_SCAN                 ' SCAN cutパラメータ
    'Public stCutMK As TRIM_DAT_CUT_MARKING              ' Letter Markingパラメータ
    '' ''Public stCutC As TRIM_DAT_CUT_C                     ' C cutパラメータ
    'Public stCutES As TRIM_DAT_CUT_ES                   ' ES cutパラメータ
    '' ''Public stCutE2 As TRIM_DAT_CUT_ES2                  ' ES2 cutパラメータ
    ''Public stCutZ As TRIM_DAT_CUT_Z                     ' Z cut(NOP)パラメータ

    Public Const RMax As Integer = 512
    Public stUCutPrm(RMax) As S_UCUTPARAM               ' U Cut パラメータ

    Public gStCutPosCorrData As CUTPOS_CORRECT_DATA

    '----- 応答データ(トリミング結果データ)(VB←INtime) -----

    '' ''Public gwTrimResult(MaxCntResist) As UShort                  ' OK/NG結果(0:未実施, 1:OK, 2:ITNG, 3:FTNG, 4:SKIP, 5:RATIO 6:NON他)
    '' ''Public gfInitialTest(MaxCntResist) As Double                 ' IT 抵抗値
    '' ''Public gfFinalTest(MaxCntResist) As Double                   ' FT 抵抗値
    '' ''Public gfTargetVal(MaxCntResist) As Double                   ' レシオ目標値

    '---------------------------------------------------------------------------
    '   基板ディレイ(ディレイトリム２)用データ(CHIP用) V1.23.0.0⑥
    '---------------------------------------------------------------------------
    ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時のNGﾁｪｯｸ用構造体形式定義
    Public Structure DELAYTRIM2_NGCNT_RESISTOR              ' ブロック数×抵抗数分 
        'Dim intNGCnt As Short                              ' 未使用のためコメント化 V1.23.0.0⑥
        Dim intITHiNgCnt As Short                           ' IT HI NG数(1ブロック1カットなので最大でも1がセットされる 以下同様)
        Dim intITLoNgCnt As Short                           ' IT LO NG数
        Dim intFTHiNgCnt As Short                           ' FT HI NG数
        Dim intFTLoNgCnt As Short                           ' FT LO NG数
        Dim intOverNgCnt As Short                           ' ｵｰﾊﾞｰﾚﾝｼﾞ数
        Dim intTotalNGCnt As Short                          ' NG数
        Dim intTotalOkCnt As Short                          ' OK数
        Dim dblInitialTest As Double                        ' IT測定値 
    End Structure

    ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時のNGﾁｪｯｸ用構造体形式定義(抵抗数分(0 ORG)) V1.23.0.0⑥
    Public Structure DELAYTRIM2_NGCNT
        Dim intNgFlag As Short                              ' NGフラグ(0=OK, 1=IT/FT NG)  
        Dim tNgCheck() As DELAYTRIM2_NGCNT_RESISTOR         ' NGチェック用配列(抵抗数分)

        ' 構造体の初期化
        Public Sub Initialize(ByVal RegCount As Integer)
            ReDim tNgCheck(RegCount - 1)                    ' 0-255
        End Sub
    End Structure

    ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時のNGﾁｪｯｸ用構造体形式定義(ブロック数X,Y分) V1.23.0.0⑥
    Public Structure DELAYTRIM2_INFO
        Dim NgAry(,) As DELAYTRIM2_NGCNT                    ' ブロック数X,Y分(0 ORG)

        ' 構造体の初期化
        Public Sub Initialize(ByVal BlkXCount As Integer, ByVal BlkYCount As Integer)
            ReDim NgAry(BlkXCount - 1, BlkYCount - 1)       ' 0-255
        End Sub
    End Structure

    'V4.9.0.0①↓
    Public CutPointTable As LaserFront.Trimmer.DefTrimFnc.CUTPOINTCALC
    Public DEFAULT_CUTFILE As String = "C:\TRIM\CutPoint.cut"
    'V4.9.0.0①↑
    ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時のNGﾁｪｯｸ用構造体 V1.23.0.0⑥
    Public stDelay2 As DELAYTRIM2_INFO = Nothing

    '---------------------------------------------------------------------------
    '   トリミング処理用
    '---------------------------------------------------------------------------
    Public gRegistorCnt As Short                            ' 抵抗数（マーキングを含む)
    Public gwCircuitCount As Short                          ' サーキット数 
    'Public NowCntResist As Integer                         ' 実際の抵抗数

    Public gfITest_Par(999) As Double                       ' IT 誤差
    Public gfITest_ParMag(999) As Double                    ' IT 誤差
    Public gfFTest_Par(999) As Double                       ' FT 誤差

    Public Const MaxCntCut As Short = 30                    ' 最大ｶｯﾄ数
    'Public MaxCutNum As Integer                            ' 最大ｶｯﾄ数 20 or 30

    'Public gfIndex2TestNum(MaxCntCut) As Short             ' Index2抵抗値(回数) V1.18.0.0⑥
    Public gfIndex2TestNum(MaxCntCut) As UShort             ' Index2抵抗値(回数) V1.18.0.0⑥ UShortでないと正しく設定されない
    Public gfIndex2ResultNum(128, MaxCntCut) As Short       ' Index2抵抗値(回数)
    Public gfIndex2Test(MaxCntCut) As Double                ' Index2抵抗値(測定値)
    Public gfIndex2Result(128, MaxCntCut) As Double         ' Index2抵抗値(測定値)

    Public gEsCutFileType As Short                          ' ESｶｯﾄをES2で読み込むかの判断ﾌﾗｸﾞ (0)ES (1)ES2

    '@@@@@(2011/02/04)@@@@
    '   動作検証が必要。
    '   VB.NETのマネージドコードから、DLLのアンマネージドコードを呼び出す際
    '   StructLayoutでマーシャルする必要がある。
    '   下記は動的配列になっているため、動作検証の上処理を修正する
    Public Structure EsTestResultMeas
        Dim dblMeas() As Double                             ' 測定値
    End Structure

    Public Structure EsTestResult
        Dim bEsExsit As Boolean                             ' ESｶｯﾄの存在ﾌﾗｸﾞ(True:1件以上ある, False:1件もない)
        Dim iCutNo() As EsTestResultMeas                    ' ｶｯﾄ番号
    End Structure

    'UPGRADE_WARNING: 配列 gtyESTestResult の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
    Public gtyESTestResult(999) As EsTestResult             ' ES測定値結果格納
    Public giTrimResult0x(999) As Short                     ' 1:High 2:Low 3:イコール
    'Public gflgResetStart As Boolean                        ' 初期化フラグ
    'Public gLoggingStart As Boolean



    '----- フラグ -----
    Public g_blnTxTyExec As Boolean                         ' TX/TY実行フラグ
    Public g_blnTx2Ty2Exec As Boolean                       ' TX2/TY2実行フラグ

#End Region

#Region "ブロック単位のトリミング処理を行う"
    '''=========================================================================
    '''<summary>ブロック単位のトリミング処理を行う</summary>
    '''<param name="xmode">      (INP)トリムモード(x0 - x5)</param>
    '''<param name="iPowerCycle">(INP)周波数(Hz)</param>
    '''<param name="intCutIndex">(INP)ｶｯﾄﾃﾞｰﾀｲﾝﾃﾞｯｸｽ(1～)(ﾃﾞｨﾚｲﾄﾘﾑ2時有効)</param>
    '''<param name="intNgFlag">  (INP)ﾌﾞﾛｯｸ内NG抵抗の有無(0:無し,1:有り)(ﾃﾞｨﾚｲﾄﾘﾑ2時有効)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function TrimBlockExe(ByVal xmode As Short, ByVal iPowerCycle As Short, ByRef intCutIndex As Short, ByRef intNgFlag As Short) As Integer

        Dim iCutIndex As Short
        Dim iNgFlag As Short
        Dim iResIndex As Short = 1                                  ' V1.23.0.0⑥
        Dim r As Integer
        Dim strMSG As String
        Dim bFlg As Boolean

        Try
            ResInfo.Initialize()

            iCutIndex = 0
            iNgFlag = 0

            ' ディレイトリム2用のデータを設定する(CHIP時)
            If (gTkyKnd = KND_CHIP) Then                            ' CHIP ? 
                If m_blnDelayCheck Then                             ' ﾃﾞｨﾚｲﾄﾘﾑ2 ?
                    If xmode = 0 Then                               ' ×0モードの時だけ前回ﾌﾗｸﾞを設定する。
                        ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時は、ﾌﾞﾛｯｸNo.毎にNGﾌﾗｸﾞを設定する。(前ｶｯﾄまでにNGがある場合は1が設定されている。)
                        iNgFlag = intNgFlag                         ' ﾌﾞﾛｯｸ内NG抵抗の有無(0:無し,1:有り)(ﾃﾞｨﾚｲﾄﾘﾑ2時有効)
                    Else
                        iNgFlag = 0
                    End If
                    iCutIndex = intCutIndex                         ' ｶｯﾄﾃﾞｰﾀｲﾝﾃﾞｯｸｽ(1～)(ﾃﾞｨﾚｲﾄﾘﾑ2時有効)
                End If
            End If

            '-------------------------------------------------------------------
            '   ブロック単位のトリミング処理
            '-------------------------------------------------------------------
            '----- V1.18.0.1⑦↓ -----
            ' レーザ照射中のシグナルタワー(黄色)点灯(ﾄﾘﾐﾝｸﾞﾓｰﾄﾞ = x0,x1,x5時)(ローム殿特注)
            If (gSysPrm.stCTM.giSPECIAL = customROHM) And ((xmode < 2) Or (xmode = 5)) Then
                Call EXTOUT1(EXTOUT_EX_YLW_ON, 0)
            End If
            ' ブロック単位のトリミング処理
            r = TRIMBLOCK(xmode, iPowerCycle, iResIndex, iCutIndex, iNgFlag)
            ' レーザ照射中のシグナルタワー(黄色)消灯(ﾄﾘﾐﾝｸﾞﾓｰﾄﾞ = x0,x1,x5)(ローム殿特注)
            If (gSysPrm.stCTM.giSPECIAL = customROHM) And ((xmode < 2) Or (xmode = 5)) Then
                Call EXTOUT1(0, EXTOUT_EX_YLW_ON)
            End If
            '----- V1.18.0.1⑦↑ -----
            '----- V1.13.0.0⑪↓ -----
            'r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)      ' エラーならエラーリターン(メッセージ表示済み)'###032
            ' 測定ばらつき検出/オーバロード検出チェック
            bFlg = IS_CV_OverLoadErrorCode(r)
            If (bFlg = False) Then                                  ' 測定ばらつき検出/オーバロード検出ならメッセージ表示しない 
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならエラーリターン(メッセージ表示済み)'###032
            End If
            '----- V1.13.0.0⑪↑ -----

            TrimBlockExe = r

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.TrimBlockExe() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            TrimBlockExe = cERR_TRAP
        End Try

    End Function

#End Region
    '----- V1.13.0.0⑪↓ -----
#Region "戻り値が測定ばらつき検出/オーバロード検出かチェックする"
    '''=========================================================================
    ''' <summary>戻り値が測定ばらつき検出/オーバロード検出かチェックする</summary>
    ''' <param name="RtnCode">(INP)TrimBlockExe()の戻り値</param>
    ''' <returns>True  = 測定ばらつき検出/オーバロード検出
    '''          False = 上記以外
    ''' </returns>
    '''=========================================================================
    Public Function IS_CV_OverLoadErrorCode(ByVal RtnCode As Integer) As Boolean 'V1.23.0.0⑦

        Dim r As Boolean
        Dim strMSG As String

        Try
            Select Case (RtnCode)
                Case ERR_MEAS_CV, ERR_MEAS_OVERLOAD, ERR_MEAS_REPROBING
                    r = True
                Case Else
                    r = False
            End Select
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "gwModule.IS_CV_OverLoadErrorCode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (False)
        End Try
    End Function
#End Region
    '----- V1.13.0.0⑪↑ -----
#Region "ログ出力対象の取得"
    '''=========================================================================
    '''<summary>ログ出力対象を設定ファイルから取得しパラメータを設定する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TrimLogging_SetLogTarget(ByRef logFileFormat() As Integer, ByVal sectName As String, ByVal ObjUtil As Object)
        Dim cnt As Integer
        Dim strTargetName As String
        Dim filePath As String

        Try
            filePath = "c:\TRIM\LogTargetFile.ini"
            logFileFormat(0) = CInt(GetPrivateProfileString_S(sectName, cDATACOUNTcKEYNAME, filePath, "1"))

            For cnt = 1 To logFileFormat.Length() - 1
                '設定ファイルの呼び出し
                strTargetName = GetPrivateProfileString_S(sectName, (cnt).ToString(), filePath, "")

                '取得文字列を比較
                Select Case strTargetName
                    Case "DATE"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_DATE
                        logFileFormat(cnt) = LOGTAR.DATE             '#4.12.2.0⑥
                    Case "MODE"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_MODE
                        logFileFormat(cnt) = LOGTAR.MODE             '#4.12.2.0⑥
                    Case "LOTNO"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_LOTNO
                        logFileFormat(cnt) = LOGTAR.LOTNO            '#4.12.2.0⑥

                    Case "BLOCKX"                   '#4.12.2.0⑥
                        logFileFormat(cnt) = LOGTAR.BLOCKX
                    Case "BLOCKY"                   '#4.12.2.0⑥ 
                        logFileFormat(cnt) = LOGTAR.BLOCKY

                    Case "CIRCUIT"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_CIRCUIT
                        logFileFormat(cnt) = LOGTAR.CIRCUIT          '#4.12.2.0⑥
                    Case "RESISTOR"
                        '#4.12.2.0⑥                       logFileFormat(cnt) = LOGTAR_RESISTOR
                        logFileFormat(cnt) = LOGTAR.RESISTOR         '#4.12.2.0⑥
                    Case "JUDGE"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_JUDGE
                        logFileFormat(cnt) = LOGTAR.JUDGE            '#4.12.2.0⑥
                    Case "TARGET"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_TARGET
                        logFileFormat(cnt) = LOGTAR.TARGET           '#4.12.2.0⑥
                    Case "INITIAL"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_INITIAL
                        logFileFormat(cnt) = LOGTAR.INITIAL          '#4.12.2.0⑥
                    Case "FINAL"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_FINAL
                        logFileFormat(cnt) = LOGTAR.FINAL            '#4.12.2.0⑥
                    Case "DEVIATION"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_DEVIATION
                        logFileFormat(cnt) = LOGTAR.DEVIATION        '#4.12.2.0⑥
                    Case "UCUTPRMNO"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_UCUTPRMNO
                        logFileFormat(cnt) = LOGTAR.UCUTPRMNO        '#4.12.2.0⑥
                    Case "END"
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_END
                        logFileFormat(cnt) = LOGTAR.END              '#4.12.2.0⑥
                    Case ""
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_NOSET
                        logFileFormat(cnt) = LOGTAR.NOSET            '#4.12.2.0⑥
                    Case Else
                        '#4.12.2.0⑥                        logFileFormat(cnt) = LOGTAR_END
                        logFileFormat(cnt) = LOGTAR.END              '#4.12.2.0⑥
                End Select

                'if 
            Next
        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)

        End Try
    End Sub
#End Region

#Region "プレートデータをINtime側に送信する"
    '''=========================================================================
    '''<summary>プレートデータをINtime側に送信する</summary>
    '''<param name="intTkyType">(INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="wreg">      (INP)抵抗数</param>
    '''<param name="bpofx">     (OUT)BP位置ｵﾌｾｯﾄX</param>
    '''<param name="bpofy">     (OUT)BP位置ｵﾌｾｯﾄY</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataPlate(ByRef intTkyType As Short, ByRef wreg As Short, ByRef bpofx As Double, ByRef bpofy As Double) As Short

        Dim intNgUnit As Short
        Dim intNgStd As Short
        Dim intCDir As Short
        Dim r As Integer
        Dim strMSG As String
        Dim stTPLT As TRIM_PLATE_DATA                                   ' プレートデータ

        Try
            ' プレートデータを設定する
            SendTrimDataPlate = cFRS_NORMAL                             ' Return値 = 正常
            With stTPLT
                ' ｱｼﾞｬｽﾄﾎﾟｲﾝﾄ位置ｵﾌｾｯﾄX,Y設定
                .fAdjustOffsetX = typPlateInfo.dblAdjOffSetXDir
                .fAdjustOffsetY = typPlateInfo.dblAdjOffSetYDir
                ' BP位置ｵﾌｾｯﾄX,Y設定
                .fBPOffsetX = typPlateInfo.dblBpOffSetXDir
                .fBPOffsetY = typPlateInfo.dblBpOffSetYDir
                bpofx = .fBPOffsetX
                bpofy = .fBPOffsetY

                ' ﾃﾞｨﾚｲﾄﾘﾑ=2の場合(CHIPのみ)には、ｶｯﾄ数のﾁｪｯｸ/ﾚｼｵﾓｰﾄﾞの有無ﾁｪｯｸを行う
                .wDelayTrim = typPlateInfo.intDelayTrim                 ' ﾃﾞｨﾚｲﾄﾘﾑ取得				
                If .wDelayTrim = 2 Then
                    If Not m_blnDelayCheck Then
                        ' ﾃﾞｨﾚｲﾁｪｯｸでｶｯﾄ数が同一でない場合、又はﾚｼｵﾓｰﾄﾞの抵抗が存在する場合には、wDelayTrim=0として実行する。
                        .wDelayTrim = 0
                    End If
                End If

                intNgUnit = typPlateInfo.intNgJudgeUnit                 ' NG判定単位取得(0:BLOCK, 1:PLATE)
                If intNgUnit = 0 Then                                   ' ブロック単位判定か？
                    .wCircuitCnt = gwCircuitCount
                Else
                    .wCircuitCnt = 1
                End If
                intNgStd = typPlateInfo.intNgJudgeLevel                 ' NG JUDGEMENT RATE 0-100%
                .fNgCriterion = CDbl(intNgStd)                          ' NG判定率

                'If (giMarkingMode = 1) Then                             ' NGマーキングあり ?　###012
                '    .wRegistCnt = wreg + 1                              ' 1000番分を設定
                'Else
                '    .wRegistCnt = wreg
                'End If
                .wRegistCnt = wreg                                      ' ###012

                '@@@@(2011/02/04)一時的に処理を追加
                '   サーキット数が1の場合は、' サーキット内抵抗数抵抗数は全抵抗数とする
                If (.wCircuitCnt <= 1) Then
                    .wResCntInCrt = wreg                                ' サーキット内抵抗数
                Else                                                    ' ###164
                    .wResCntInCrt = typPlateInfo.intResistCntInGroup    ' ###164 サーキット内抵抗数
                End If                                                  ' ###164

                .fZStepPos = typPlateInfo.dblZStepUpDist                ' Z軸ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置
                .fZTrimPos = typPlateInfo.dblZOffSet                    ' Z軸ｺﾝﾀｸﾄ位置(ｵﾌｾｯﾄ位置)

                intCDir = typPlateInfo.intResistDir                     ' チップ並び方向取得(CHIP-NETのみ)
                If (0 = intCDir) Then
                    .fReProbingX = 0
                    .fReProbingY = typPlateInfo.dblRetryProbeDistance   ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞY移動量
                Else
                    .fReProbingY = 0
                    .fReProbingX = typPlateInfo.dblRetryProbeDistance   ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞX移動量
                End If
                .wReProbingCnt = typPlateInfo.intRetryProbeCount        ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ回数

                .wInitialOK = typPlateInfo.intInitialOkTestDo           ' ｲﾆｼｬﾙOKﾃｽﾄ有無
                .wNGMark = typPlateInfo.intNGMark                       ' NGﾏｰｷﾝｸﾞする/しない
                .w4Terminal = typPlateInfo.intOpenCheck                 ' 4端子ｵｰﾌﾟﾝﾁｪｯｸする/しない

                ' ﾛｷﾞﾝｸﾞﾓｰﾄﾞ(0:しない, 1:INITIAL TEST, 2:FINAL TEST, 3:INITIAL + FINAL)	
                If (gSysPrm.stLOG.giLoggingMode = 0) Then
                    .wLogMode = gSysPrm.stLOG.giLoggingMode
                Else
                    .wLogMode = gSysPrm.stLOG.giLoggingDataKind
                End If

                gLogMode = .wLogMode                                    ' ﾛｷﾞﾝｸﾞﾓｰﾄﾞ退避 ###150
                .bTrimCutEnd = True                                     ' カットオフ目標最大値に到達したらカットを終了する（TRUE）/しない（FALSE）
                '----- V1.13.0.0②↓ -----
                .intNgJudgeStop = typPlateInfo.intNgJudgeStop           ' NG判定時停止(OVRERLOAD/CVエラー用)
                .dblReprobeVar = typPlateInfo.dblReprobeVar             ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞばらつき量
                .dblReprobePitch = typPlateInfo.dblReprobePitch         ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞﾋﾟｯﾁ
                .dblLwPrbStpDwDist = typPlateInfo.dblLwPrbStpDwDist     ' 下方プローブステップ下降距離
                .dblLwPrbStpUpDist = typPlateInfo.dblLwPrbStpUpDist     ' 下方プローブステップ上昇距離
                '----- V1.13.0.0②↑ -----
            End With

            ' プレートデータをINtime側に送信する
            r = TRIMDATA_PLATE(stTPLT, intTkyType)
            If (r <> cFRS_NORMAL) Then                                  ' 送信失敗 ?
                strMSG = "Ptate Data Send Error(r=" + r.ToString + ")"
                Call Form1.System1.TrmMsgBox(gSysPrm, "Ptate Data Send Error 1    ", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataPlate = 1
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataPlate() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataPlate = cERR_TRAP
        End Try

    End Function
#End Region

#Region "プレートデータ(GP-IB設定用ﾃﾞｰﾀ)をINtime側に送信する"
    '''=========================================================================
    ''' <summary>プレートデータ(GP-IB設定用ﾃﾞｰﾀ)をINtime側に送信する</summary>
    ''' <param name="intTkyType">(INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    ''' <param name="wreg">      (I/O)抵抗数</param>
    ''' <param name="Type">      (INP)タイプ(0=GPIB, 1=GBIB2) ###229</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataPlate2(ByRef intTkyType As Short, ByRef wreg As Short, ByVal Type As Integer) As Short ' ###229
        '   Public Function SendTrimDataPlate2(ByRef intTkyType As Short, ByRef wreg As Short) As Short

        Dim s As String
        Dim n As Short
        Dim nn As Short
        Dim r As Integer
        ''Dim stTGPI As TRIM_DAT_GPIB                                    ' GPIB設定データ
        'stTGPI.prmGPIB.Initialize()
        Dim strMSG As String

        Try
            ' プレートデータ(GP-IB設定用ﾃﾞｰﾀ)を設定する
            SendTrimDataPlate2 = cFRS_NORMAL                                ' Return値 = 正常
            If (Type = 0) Then                                              ' ###229

                With stTGPI
                    .wGPIBmode = typPlateInfo.intGpibCtrl                   ' GP-IB制御(0:しない 1:する(ADEX用))
                    '----- ###229↓ -----
                    ' シスパラがGP-IB制御なし又はGP-IB制御あり(2:汎用)ならGP-IB制御(ADEX)なしとする 
                    If (gSysPrm.stCTM.giGP_IB_flg = 0) Or (gSysPrm.stCTM.giGP_IB_flg = 2) Then
                        .wGPIBmode = 0                                      ' GP-IB制御なしとする 
                    End If
                    '----- ###229↑ -----
                    .wDelim = typPlateInfo.intGpibDefDelimiter              ' ﾃﾞﾘﾐﾀ(0:CR+LF 1:CR 2:LF 3:NONE)
                    .wTimeout = typPlateInfo.intGpibDefTimiout              ' ﾀｲﾑｱｳﾄ(0～1000)
                    .wAddress = typPlateInfo.intGpibDefAdder                ' 機器ｱﾄﾞﾚｽ(0～30)
                    .wMeasurementMode = typPlateInfo.intGpibMeasMode        ' 測定ﾓｰﾄﾞ(0:絶対、1:偏差)

                    ' 予備(MAX6byte)(念の為、初期化しておく)
                    s = ""
                    For n = 0 To (6 - 1)
                        .strI(n) = 0 : Next
                    nn = Len(s)
                    If nn <= 6 Then
                        s = s & Space(6 - nn)
                    End If
                    For n = 1 To 6
                        .wReserve(n - 1) = Asc(Mid(s, n, 1))
                    Next

                    ' 初期化ｺﾏﾝﾄﾞ(MAX40byte)
                    s = typPlateInfo.strGpibInitCmnd1 & typPlateInfo.strGpibInitCmnd2
                    For n = 0 To (40 - 1)
                        .strI(n) = 0 : Next
                    nn = Len(s)
                    If nn <= 40 Then
                        s = s & Space(40 - nn)
                    End If
                    For n = 1 To 40
                        .strI(n - 1) = Asc(Mid(s, n, 1))
                    Next

                    ' ﾄﾘｶﾞｺﾏﾝﾄﾞ(MAX10byte)
                    s = typPlateInfo.strGpibTriggerCmnd
                    For n = 0 To (10 - 1)
                        .strT(n) = 0 : Next
                    nn = Len(s)
                    If nn <= 10 Then
                        s = s & Space(10 - nn)
                    End If
                    For n = 1 To 10
                        .strT(n - 1) = Asc(Mid(s, n, 1))
                    Next

                End With

                ' プレートデータ(GP-IB設定用ﾃﾞｰﾀ)をINtime側に送信する ###002
                r = TRIMDATA_GPIB(stTGPI, intTkyType)
                If (r <> cFRS_NORMAL) Then                                  ' 送信失敗 ?
                    Call Form1.System1.TrmMsgBox(gSysPrm, "GP-IB Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataPlate2 = 8
                    Exit Function
                End If

                '----- ###229↓ -----
            Else
                ' プレートデータ(GP-IB設定用ﾃﾞｰﾀ)を設定する
                With stTGPI2
                    .wGPIBflg = bGpib2Flg                                  ' GP-IB制御(0:しない 1:する)
                    'V5.0.0.6⑱ .wGPIBdlm = typGpibInfo.wDelim                            ' ﾃﾞﾘﾐﾀ(0:CR+LF 1:CR 2:LF 3:NONE)
                    'V5.0.0.6⑱ INTIME以降、IDELIM、GpibSetDelimは、ﾃﾞﾘﾐﾀ(0:NONE 1:CR+LF 2:CR 3:LF )の順番なのでここで修正する。
                    .wGPIBdlm = typGpibInfo.wDelim + 1                      'V5.0.0.6⑱
                    If .wGPIBdlm = 4 Then                                   'V5.0.0.6⑱
                        .wGPIBdlm = 0                                       'V5.0.0.6⑱
                    End If                                                  'V5.0.0.6⑱
                    .wGPIBtout = typGpibInfo.wTimeout                        ' ﾀｲﾑｱｳﾄ(1～32767)
                    .wGPIBdev = typGpibInfo.wAddress                        ' 機器ｱﾄﾞﾚｽ(0～30)
                    .wEOI = typGpibInfo.wEOI                                ' EOI(0:使用しない, 1:使用する)
                    .wPause1 = typGpibInfo.wPause1                          ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
                    .wPause2 = typGpibInfo.wPause2                          ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
                    .wPause3 = typGpibInfo.wPause3                          ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
                    .wPauseT = typGpibInfo.wPauseT                          ' 設定ｺﾏﾝﾄﾞ1送信後ポーズ時間(1～32767msec)
                    .wRsv = typGpibInfo.wRev                                ' 予備
                    ' GPIB文字列パラメータを設定する
                    SetGpibByteParam(typGpibInfo.strI, .cGPIBinit, PLT_GPIB2_INI_SZ)         ' 初期化コマンド文字列1
                    SetGpibByteParam(typGpibInfo.strI2, .cGPIBini2, PLT_GPIB2_INI_SZ)       ' 初期化コマンド文字列2
                    SetGpibByteParam(typGpibInfo.strI3, .cGPIBini3, PLT_GPIB2_INI_SZ)       ' 初期化コマンド文字列3
                    SetGpibByteParam(typGpibInfo.strT, .cGPIBtriger, PLT_GPIB2_TRG_SZ)         ' トリガー文字列
                    SetGpibByteParam(typGpibInfo.strName, .cGPIBName, PLT_GPIB2_NAM_SZ)   ' 機器名文字列
                    SetGpibByteParam(typGpibInfo.wReserve, .cRsv, PLT_GPIB2_RSV_SZ) ' 予備文字列
                End With

                ' プレートデータ(GP-IB設定用(汎用)ﾃﾞｰﾀ)をINtime側に送信する
                r = TRIMDATA_GPIB2(stTGPI2, intTkyType)
                If (r <> cFRS_NORMAL) Then                                  ' 送信失敗 ?
                    Call Form1.System1.TrmMsgBox(gSysPrm, "GP-IB2 Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataPlate2 = 9
                    Exit Function
                End If
            End If
            '----- ###229↑ -----

            '----- V6.1.1.0⑤↓ -----
            ' 外部リレー切替機用(GP-IB設定用データ)をINtime側に送信する
            r = SendTrimDataGpib3(intTkyType)
            If (r <> cFRS_NORMAL) Then
                SendTrimDataPlate2 = r
                Exit Function
            End If
            '----- V6.1.1.0⑤↑ -----

            'If giMarkingMode = 1 Then                                   ' NGマーキングあり ?　###012
            '    wreg = wreg + 1                                         ' マーキング情報分増やす
            'End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataPlate2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataPlate2 = cERR_TRAP
        End Try

    End Function
#End Region

    '----- V6.1.1.0⑤↓ -----
#Region "外部リレー切替機用(GP-IB設定用データ)をINtime側に送信する"
    '''=========================================================================
    ''' <summary>外部リレー切替機用(GP-IB設定用データ)をINtime側に送信する</summary>
    ''' <param name="intTkyType">(INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataGpib3(ByRef intTkyType As Short) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 外部リレー切替機用(GP-IB設定用データ)を設定する
            SendTrimDataGpib3 = cFRS_NORMAL                             ' Return値 = 正常

            ' シスパラがGP-IB制御なしの時送信しない：GP-IB制御あり(1:ADEX,2:汎用)
            If gSysPrm.stCTM.giGP_IB_flg <> 2 Then
                Exit Function
            End If
            With stTGPI3
                .wScannerUse = typGpibInfo.wScannerUse                  ' スキャナ使用有無(0:使用しない, 1:使用する)
                .wScannerAddress = typGpibInfo.wScannerAddress          ' 機器アドレス(0～30)
                .wScannerTimeout = typGpibInfo.wScannerTimeout          ' タイムアウト値(100ms単位)
                .wScannerPauseT = typGpibInfo.wScannerPauseT            ' トリガコマンド送信後ポーズ時間(1～32767msec)
            End With

            ' プレートデータ(GP-IB設定用(汎用)ﾃﾞｰﾀ)をINtime側に送信する
            r = TRIMDATA_GPIB3(stTGPI3, intTkyType)
            If (r <> cFRS_NORMAL) Then                                  ' 送信失敗 ?
                'Call Form1.System1.TrmMsgBox(gSysPrm, "スキャナーの初期化がエラー終了しました。", MsgBoxStyle.OkOnly, TITLE_4)
                Call Form1.System1.TrmMsgBox(gSysPrm, "GP-IB3 Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataGpib3 = 10
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataGpib3() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataGpib3 = cERR_TRAP
        End Try

    End Function
#End Region
    '----- V6.1.1.0⑤↑ -----
    '----- ###229↓ -----
#Region "GPIB文字列パラメータを設定する"
    '''=========================================================================
    '''<summary>GPIB文字列パラメータを設定する</summary>
    '''<param name="InpData"> (INP)入力データ</param>
    ''' <param name="OutData">(OUT)出力データ</param>
    ''' <param name="OutSz">  (OUT)出力データサイズ</param>
    '''=========================================================================
    Private Sub SetGpibByteParam(ByRef InpData As String, ByRef OutData() As Byte, ByVal OutSz As Integer)

        Dim Idx As Integer
        Dim Sz As Integer
        Dim strDat As String
        Dim strMSG As String

        Try
            ' 出力データ域nullクリア
            strDat = InpData
            For Idx = 0 To OutSz - 1
                OutData(Idx) = 0
            Next Idx

            ' 入力データの後方に空白を設定する 
            Sz = Len(InpData)
            If (Sz <= OutSz) Then
                strDat = strDat + Space(OutSz - Sz)
            End If

            ' 出力データへ1文字づつ設定する
            For Idx = 1 To OutSz
                OutData(Idx - 1) = Asc(Mid(strDat, Idx, 1))
            Next Idx

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SetGpibByteParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###229↑ ----
#Region "抵抗データをINtime側に送信する"
    '''=========================================================================
    '''<summary>抵抗データをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="i">          (INP)抵抗データ・インデックス</param>
    '''<param name="intRegIndex">(OUT)抵抗データ・インデックス</param> 
    '''<param name="wreg">       (INP)抵抗数</param>  
    '''<param name="RNO">        (OUT)抵抗番号</param>  
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataRegistor(ByRef intTkyType As Short, ByRef i As Short, _
                                         ByRef intRegIndex As Short, ByRef wreg As Short, ByRef RNO As Short) As Short
        'Public Function SendTrimDataRegistor(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                     ByRef intRegIndex As Short, ByRef wreg As Short, ByRef RNO As Short) As Short


        Dim r As Integer
        Dim stTRES As New TRIM_RESISTOR_DATA()                         ' 抵抗データ
        Dim strMSG As String

        Try
            '-----------------------------------------------------------------------
            '   抵抗データを設定する
            '-----------------------------------------------------------------------
            SendTrimDataRegistor = cFRS_NORMAL                  ' Return値 = 正常
            stTRES.Initialize()

            '' CHIP,NETのみ下記は存在
            'If giMarkingMode = 1 And i = wreg Then              ' マーキングありで、ｲﾝﾃﾞｯｸｽが最後の場合　###012
            '    intRegIndex = 1000                              ' 配列はマーキングを指すようにする
            'Else
            '    intRegIndex = i
            'End If
            intRegIndex = i                                     ' ###012

            With stTRES
                'V4.7.0.0-22                If (gTkyKnd = KND_TKY) Then
                If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                    '-----------------------------------------------------------
                    '   TKY時
                    '-----------------------------------------------------------
                    Dim isBaseExist As Boolean
                    Dim n As Integer

                    ' レシオトリミング ?
                    .wRatioMode = typResistorInfoArray(intRegIndex).intTargetValType    ' 目標値指定（0:絶対値,1:レシオ,2:計算式）
                    .wBaseReg = typResistorInfoArray(intRegIndex).intBaseResNo          ' ベース抵抗番号
                    If (typResistorInfoArray(intRegIndex).intTargetValType = 1) Or _
                       ((typResistorInfoArray(intRegIndex).intTargetValType >= 3) And (typResistorInfoArray(intRegIndex).intTargetValType <= 9)) Then
                        ' 目標値指定がレシオの場合、 
                        ' ベース抵抗がレシオ指定でないかチェックする
                        isBaseExist = False
                        For n = 1 To wreg                       ' 抵抗数分繰り返す 
                            If typResistorInfoArray(n).intResNo = typResistorInfoArray(intRegIndex).intBaseResNo Then   'ベース抵抗?
                                If typResistorInfoArray(n).intTargetValType Then
                                    Call Form1.System1.TrmMsgBox(gSysPrm, "Base resistor is also ratio mode. Base reg =" + Str(n), vbExclamation + vbOKOnly, gAppName)
                                    SendTrimDataRegistor = 2    ' Return値 = 2(ベース抵抗がレシオ指定) 
                                    Exit Function
                                End If
                                isBaseExist = True
                                Exit For
                            End If
                        Next
                        If isBaseExist = False Then             ' ベース抵抗なし ?
                            Call Form1.System1.TrmMsgBox(gSysPrm, "Ratio trimming base resistor could not be found." + _
                                                           "PR1=" + Str(typResistorInfoArray(intRegIndex).intResNo) + _
                                                           ",PR8=" + Str(typResistorInfoArray(intRegIndex).intBaseResNo), _
                                                  vbExclamation + vbOKOnly, gAppName)
                            SendTrimDataRegistor = 3            ' Return値 = 3(レシオ指定時ベース抵抗なし) 
                            Exit Function
                        End If
                        .fTargetVal = typResistorInfoArray(intRegIndex).dblTrimTargetVal

                    ElseIf typResistorInfoArray(intRegIndex).intTargetValType = 2 Then
                        ' 目標値指定が2(計算式)の場合、レシオモード２計算式をINtime側に送信する
                        Dim sRatio2 As S_RATIO2EXP
                        Dim strExp As String

                        sRatio2.RNO = typResistorInfoArray(intRegIndex).intResNo
                        strExp = typResistorInfoArray(intRegIndex).strRatioTrimTargetVal
                        strExp = UCase(strExp)
                        If Len(strExp) > 100 Then
                            MsgBox("check Expression length", vbOKOnly, "DEBUG")
                            strExp = Left(strExp, 100)
                        End If
                        ' レシオモード２計算式をINtime側に送信する
                        sRatio2.strExp = strExp
                        Call RATIO2EXP(sRatio2.RNO, sRatio2.strExp)
                        .fTargetVal = 0.0#                                ' トリミング目標値
                    Else
                        ' 目標値指定が絶対値によるトリミングの場合、トリミング目標値を設定する
                        .fTargetVal = typResistorInfoArray(intRegIndex).dblTrimTargetVal
                    End If

                    ' カット位置補正機能有効 ?
                    If (Form1.stFNC(F_CUTPOS).iDEF = 1) Then
                        .wCorrectFlg = typResistorInfoArray(i).intCutReviseMode    ' カット位置補正フラグ(0:補正しない, 1:補正する)
                    Else
                        .wCorrectFlg = 0                                           ' カット位置補正しない
                    End If

                ElseIf (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then
                    '-----------------------------------------------------------
                    '   CHIP/NET時
                    '-----------------------------------------------------------
                    .fTargetVal = CDbl(typResistorInfoArray(intRegIndex).dblTrimTargetVal)            ' トリミング目標値(ohm)
                    .fCutMag = CDbl(typResistorInfoArray(intRegIndex).dblCutOffRatio)           ' 切上げ倍
                End If

                '---------------------------------------------------------------
                '   TKY/CHIP/NET共通
                '---------------------------------------------------------------
                .wResNo = typResistorInfoArray(intRegIndex).intResNo                ' 抵抗番号(1-999=トリミング, 1000-9999=マーキング)
                .wMeasMode = typResistorInfoArray(intRegIndex).intResMeasMode       ' 測定モード(0：抵抗、1：電圧)
                .wMeasType = typResistorInfoArray(intRegIndex).intResMeasType       ' 測定タイプ(0:高速 ,1:高精度、２：外部)
                '----- ###229↓ -----
                ' シスパラがGP-IB制御あり(2:汎用)で測定タイプが外部ならGP-IB制御(汎用)フラグを制御ありとする
                If (gSysPrm.stCTM.giGP_IB_flg = 2) And (.wMeasType = 2) Then
                    bGpib2Flg = 1                                                   ' GP-IB制御(汎用)フラグ(0=制御なし, 1=制御あり)
                End If
                '----- ###229↑ -----
                .wCircuit = typResistorInfoArray(intRegIndex).intCircuitGrp         ' サーキット(抵抗が属するサーキット番号)
                .wHiProbNo = typResistorInfoArray(intRegIndex).intProbHiNo          ' ハイ側プローブ番号
                .wLoProbNo = typResistorInfoArray(intRegIndex).intProbLoNo          ' ロー側プローブ番号
                .w1stAG = typResistorInfoArray(intRegIndex).intProbAGNo1            ' 第1アクティブガード番号
                .w2ndAG = typResistorInfoArray(intRegIndex).intProbAGNo2            ' 第2アクティブガード番号
                .w3rdAG = typResistorInfoArray(intRegIndex).intProbAGNo3            ' 第3アクティブガード番号
                .w4thAG = typResistorInfoArray(intRegIndex).intProbAGNo4            ' 第4アクティブガード番号
                .w5thAG = typResistorInfoArray(intRegIndex).intProbAGNo5            ' 第5アクティブガード番号
                '----- ###239↓-----
                '.dwExBits = CLng(typResistorInfoArray(intRegIndex).strExternalBits)' External Bits(これだと"11110000"→0x11110000となる(正しくは0x00f0))
                strMSG = typResistorInfoArray(intRegIndex).strExternalBits.Trim()   ' 前後の空白を削除
                .dwExBits = Convert.ToUInt32(strMSG, 2)                             ' External Bits(2進数文字列を数値に変換)
                '----- ###239↑-----
                .wPauseTime = typResistorInfoArray(intRegIndex).intPauseTime        ' External Bits 出力後ポーズタイム
                .wSlope = typResistorInfoArray(intRegIndex).intSlope                ' スロープ(1:電圧+スロープ, 2:電圧-スロープ, 4:抵抗+スロープ, 5:抵抗マイナススロープ) 
                ' ITﾘﾐｯﾄﾊｲ/ﾛｰ(%)
                .fITLimitH = CDbl(typResistorInfoArray(intRegIndex).dblInitTest_HighLimit)
                .fITLimitL = CDbl(typResistorInfoArray(intRegIndex).dblInitTest_LowLimit)
                ' FTﾘﾐｯﾄﾊｲ/ﾛｰ(%)
                .fFTLimitH = CDbl(typResistorInfoArray(intRegIndex).dblFinalTest_HighLimit)
                .fFTLimitL = CDbl(typResistorInfoArray(intRegIndex).dblFinalTest_LowLimit)
                .wInitialOK = typResistorInfoArray(intRegIndex).intInitialOkTestDo   '初期ＯＫ判定(0:しない,1:する) V5.0.0.6⑨
                .wCutCnt = typResistorInfoArray(intRegIndex).intCutCount            ' カット数
                '----- V1.13.0.0②↓ -----
                .intCvMeasNum = typResistorInfoArray(intRegIndex).intCvMeasNum      ' CV 最大測定回数
                .intCvMeasTime = typResistorInfoArray(intRegIndex).intCvMeasTime    ' CV 最大測定時間(ms) 
                .dblCvValue = typResistorInfoArray(intRegIndex).dblCvValue          ' CV CV値 
                .intOverloadNum = typResistorInfoArray(intRegIndex).intOverloadNum  ' ｵｰﾊﾞｰﾛｰﾄﾞ 回数
                .dblOverloadMin = typResistorInfoArray(intRegIndex).dblOverloadMin  ' ｵｰﾊﾞｰﾛｰﾄﾞ 下限値
                .dblOverloadMax = typResistorInfoArray(intRegIndex).dblOverloadMax  ' CV ｵｰﾊﾞｰﾛｰﾄﾞ 上限値
                '----- V1.13.0.0②↑ -----
                'V4.0.0.0-39　↓
                .wPauseTimeFT = typResistorInfoArray(intRegIndex).wPauseTimeFT      'FT前のポーズタイム 
                'V4.0.0.0-39　↑
                ''V4.8.0.0②
                .dblInsideEndChgRate = typResistorInfoArray(intRegIndex).dblInsideEndChgRate
                .wInsideEndChkCount = typResistorInfoArray(intRegIndex).intInsideEndChkCount
                ''V4.8.0.0②

                'V6.0.0.2①↓
                .wModeSelect = typResistorInfoArray(intRegIndex).intULowResOutputType                 '///< 電流モード：０：
                .wCurSel = typResistorInfoArray(intRegIndex).intULowResCurrentVal                     '///< 測定電流：0:標準、1:１A、2:2A、3:3A
                'V6.0.0.2①↑

                RNO = .wResNo                                                       ' 抵抗番号を返す
            End With

            '-----------------------------------------------------------------------
            '   抵抗データをINtime側に送信する
            '-----------------------------------------------------------------------
            r = TRIMDATA_RESISTOR(stTRES, i)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "Resistor Data Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataRegistor = 5
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataRegistor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataRegistor = cERR_TRAP
        End Try

    End Function
#End Region

#Region "加工条件の設定"
    '''=========================================================================
    '''<summary>加工条件を設定する</summary>
    '''<param name="stCutCon">(OUT)加工条件設定構造体</param>
    '''<param name="resNo">   (INP)抵抗データ・インデックス</param>
    '''<param name="cutNo">   (INP)カットデータ・インデックス</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function SetCutConditionData(ByRef stCutCon() As S_CUT_CONDITION, ByVal resNo As Integer, ByVal cutNo As Integer) As Integer

        Dim CndNum As Integer
        Dim Idx As Integer
        Dim strCutType As String
        Dim strMSG As String

        Try
            ' 初期処理
            SetCutConditionData = cFRS_NORMAL
            strCutType = typResistorInfoArray(resNo).ArrCut(cutNo).strCutType.Trim()

            ' 加工条件設定構造体を初期化する
            'For Idx = 0 To (cCNDNUM - 1)
            For Idx = 0 To (MaxCndNum - 1)  'V5.0.0.8①
                stCutCon(Idx).cutSetNo = 0                          ' 加工条件番号
                'stCutCon(Idx).cutSpd = 0.0                          ' カットスピード
                'stCutCon(Idx).cutQRate = 0.0                        ' カットQレート
                stCutCon(Idx).cutSpd = 1.0                          ' カットスピード ※###004 固体レーザ時INTime側でパラメータエラーとなるため
                stCutCon(Idx).cutQRate = 1.0                        ' カットQレート  ※###004 
                stCutCon(Idx).bUse = False                          ' INTRIM側でのみ使用
            Next Idx

            '   '-------------------------------------------------------------------
            '   '   加工条件構造体を設定する(FL時)
            '   '-------------------------------------------------------------------
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                ' FL時は加工条件番号テーブルから加工条件番号とQレートを設定する(カットスピードはデータから設定)
                ' 加工条件1は全カット無条件に設定する
                CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1)
                stCutCon(CUT_CND_L1).cutSetNo = CndNum
                stCutCon(CUT_CND_L1).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                stCutCon(CUT_CND_L1).cutQRate = stCND.Freq(CndNum)

                ' 加工条件2はLカット, 斜めLカット, Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)
                ' HOOKカット, Uカット時に設定する
                If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                   (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                   (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0①
                    ' 加工条件2
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L2)
                    stCutCon(CUT_CND_L2).cutSetNo = CndNum
                    stCutCon(CUT_CND_L2).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L2).cutQRate = stCND.Freq(CndNum)
                End If

                ' 加工条件3はHOOKカット, Uカット時に設定する
                If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0①
                    ' 加工条件3
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L3)
                    stCutCon(CUT_CND_L3).cutSetNo = CndNum
                    stCutCon(CUT_CND_L3).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed3
                    stCutCon(CUT_CND_L3).cutQRate = stCND.Freq(CndNum)
                    '----- V1.22.0.0①↓ -----
                    ' Uカット/Uカット(リトレース)時は速度3を設定する(データ編集V1.19.0.0以降で対応)
                    ''----- ###194↓ -----
                    '' Uカット時は速度3に速度1を設定する
                    'If (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0①
                    '    stCutCon(CUT_CND_L3).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                    'End If
                    ''----- ###194↑ -----
                    '----- V1.22.0.0①↑ -----
                End If

                ' 加工条件4は現状は未使用(予備)

                ' 加工条件5～8はリターン/リトレース用 
                ' 加工条件5(STカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めSTカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                   (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                    ' 加工条件5の条件番号をカットデータの加工条件2より設定する
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(1)               ' V4.2.0.0②
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1_RET)   ' V4.2.0.0②
                    stCutCon(CUT_CND_L1_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L1_RET).cutQRate = stCND.Freq(CndNum)
                End If

                ' 加工条件5,6(Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                    ' 加工条件5の条件番号をカットデータの加工条件3より設定する
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(2)               ' V4.2.0.0②
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1_RET)   ' V4.2.0.0②
                    stCutCon(CUT_CND_L1_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed3
                    stCutCon(CUT_CND_L1_RET).cutQRate = stCND.Freq(CndNum)
                    ' 加工条件6の条件番号をカットデータの加工条件4より設定する
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(3)               ' V4.2.0.0②
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L2_RET)   ' V4.2.0.0②
                    stCutCon(CUT_CND_L2_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed4
                    stCutCon(CUT_CND_L2_RET).cutQRate = stCND.Freq(CndNum)
                End If

                '----- V1.22.0.0①↓ -----
                ' 加工条件5,6,7はUカット(リトレース)時に設定する
                If (strCutType = CNS_CUTP_Ut) Then
                    ' 加工条件5の条件番号をカットデータの加工条件4より設定する
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(3)               ' V4.2.0.0②
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L1_RET)   ' V4.2.0.0②
                    stCutCon(CUT_CND_L1_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed4
                    stCutCon(CUT_CND_L1_RET).cutQRate = stCND.Freq(CndNum)
                    ' 加工条件6の条件番号をカットデータの加工条件5より設定する
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(4)               ' V4.2.0.0②
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L2_RET)   ' V4.2.0.0②
                    stCutCon(CUT_CND_L2_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed5
                    stCutCon(CUT_CND_L2_RET).cutQRate = stCND.Freq(CndNum)
                    ' 加工条件7の条件番号をカットデータの加工条件6より設定する
                    'CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(5)               ' V4.2.0.0②
                    CndNum = typResistorInfoArray(resNo).ArrCut(cutNo).CndNum(CUT_CND_L3_RET)   ' V4.2.0.0②
                    stCutCon(CUT_CND_L3_RET).cutSetNo = CndNum
                    stCutCon(CUT_CND_L3_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed6
                    stCutCon(CUT_CND_L3_RET).cutQRate = stCND.Freq(CndNum)
                End If
                '----- V1.22.0.0①↑ -----

                ' 加工条件8は現状は未使用(予備)

                '-------------------------------------------------------------------
                '   加工条件構造体を設定する(FL以外)
                '   FLでない時はカットデータからカットスピード/Qレートを設定する
                '-------------------------------------------------------------------
            Else
                ' 通常カット用(加工条件1～4)
                ' 加工条件1は全カット無条件に設定する
                stCutCon(CUT_CND_L1).cutSetNo = 0                                           ' 加工条件1
                stCutCon(CUT_CND_L1).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                stCutCon(CUT_CND_L1).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate

                ' 加工条件2はLカット, 斜めLカット, Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)
                ' HOOKカット, Uカット時に設定する '###023 
                If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                   (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                   (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0①
                    ' 加工条件2
                    stCutCon(CUT_CND_L2).cutSetNo = 0
                    stCutCon(CUT_CND_L2).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                    stCutCon(CUT_CND_L2).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate
                End If

                ' 加工条件3はHOOKカット, Uカット時に設定する '###023 
                If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0①
                    ' 加工条件3
                    stCutCon(CUT_CND_L3).cutSetNo = 0
                    stCutCon(CUT_CND_L3).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed
                    stCutCon(CUT_CND_L3).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate
                End If

                ' 加工条件4は現状は未使用(予備)

                ' リターン/リトレース用(加工条件5～6) '###023
                ' 加工条件5は(STカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めSTカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時に設定する
                If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                   (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Or _
                   (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                   (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                    ' 加工条件5をカットデータより設定する
                    stCutCon(CUT_CND_L1_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L1_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    ' 加工条件6をカットデータより設定する
                    stCutCon(CUT_CND_L2_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L2_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                End If

                '----- V1.22.0.0①↓ -----
                ' 加工条件5,6,7は(Uカット(ﾘﾄﾚｰｽ)時に設定する
                If (strCutType = CNS_CUTP_Ut) Then
                    ' 加工条件5をカットデータより設定する(速度2とQレート2を設定する)
                    stCutCon(CUT_CND_L1_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L1_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L1_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    ' 加工条件6をカットデータより設定する(速度2とQレート2を設定する)
                    stCutCon(CUT_CND_L2_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L2_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L2_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    ' 加工条件7をカットデータより設定する(速度2とQレート2を設定する)
                    stCutCon(CUT_CND_L3_RET).cutSetNo = 0
                    stCutCon(CUT_CND_L3_RET).cutSpd = typResistorInfoArray(resNo).ArrCut(cutNo).dblCutSpeed2
                    stCutCon(CUT_CND_L3_RET).cutQRate = typResistorInfoArray(resNo).ArrCut(cutNo).dblQRate2
                    '----- V1.22.0.0①↑ -----
                End If

                ' 加工条件8は現状は未使用(予備)

            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SetCutConditionData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SetCutConditionData = cERR_TRAP
        End Try
    End Function

#End Region

    '----- V2.0.0.0_26↓ -----
#Region "加工条件番号からカットデータのQレート,電流値,STEG本数を設定する"
    '''=========================================================================
    ''' <summary>加工条件番号からカットデータのQレート,電流値,STEG本数を設定する</summary>
    ''' <param name="FileVer">(INP)データファイルバージョン</param>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          上記以外 　　= エラー</returns> 
    '''=========================================================================
    Public Function SetCutDataCndInfFromCndNum(ByVal FileVer As Double) As Integer

        Dim Rn As Integer
        Dim Cn As Integer
        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   初期処理
            '------------------------------------------------------------------
            ' FLでない又はSL436SでなければNOP
            'If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Or (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)
            ' FLでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL) ' V4.0.0.0-28

            ' データファイルバージョンがVer10.10以上ならNOP
            If (FileVer >= FileIO.FILE_VER_10_10) Then Return (cFRS_NORMAL)

            '------------------------------------------------------------------
            '   加工条件番号からカットデータのQレート,電流値,STEG本数を設定する
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' １ブロック内抵抗数分設定する 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' カット数分設定する
                    ' カットタイプ取得
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

                    ' Qレート1,電流値1,STEG本数1は無条件に設定する
                    ' 加工条件1
                    CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1)
                    typResistorInfoArray(Rn).ArrCut(Cn).dblQRate = stCND.Freq(CndNum)
                    typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(0) = stCND.Steg(CndNum)
                    typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(0) = stCND.Curr(CndNum)

                    ' Qレート2,電流値2,STEG本数2はLカット, 斜めLカット, Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)
                    ' HOOKカット, Uカット時に設定する
                    If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                       (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                       (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Then
                        ' 加工条件2
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(1) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(1) = stCND.Curr(CndNum)
                    End If

                    ' Qレート3,電流値3,STEG本数3はHOOKカット, Uカット時に設定する
                    If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Then
                        ' 加工条件3
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(2) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(2) = stCND.Curr(CndNum)
                    End If

                    ' 加工条件4は現状は未使用(予備)

                    ' 加工条件5～8はリターン/リトレース用 
                    ' Qレート5,電流値5,STEG本数5は(STカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めSTカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時に設定する
                    If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                       (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                        ' 加工条件5
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate5 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(4) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(4) = stCND.Curr(CndNum)
                    End If

                    ' Qレート6,電流値6,STEG本数6はLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時に設定する
                    If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                        ' 加工条件6
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).dblQRate6 = stCND.Freq(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(5) = stCND.Steg(CndNum)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(5) = stCND.Curr(CndNum)
                    End If

                    ' 加工条件7,8は現状は未使用(予備)

                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return値設定 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "gwModule.SetCutDataCndInfFromCndNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try
    End Function
#End Region
    '----- V2.0.0.0_26↑ -----

#Region "カットデータをINtime側に送信する"
    '''=========================================================================
    '''<summary>カットデータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="i">          (INP)抵抗データ・インデックス</param>
    '''<param name="j">          (INP)カットデータ・インデックス</param>
    '''<param name="intRegIndex">(INP)抵抗番号</param>
    '''<param name="startpx">    (OUT)カットスタート座標X</param> 
    '''<param name="startpy">    (OUT)カットスタート座標Y</param> 
    '''<param name="m">          (OUT)抵抗データ・インデックス</param> 
    '''<param name="mm">         (OUT)カットデータ・インデックス</param>  
    '''<param name="cut_type">   (OUT)カット形状(1:st, 2:L, 3:HK, 4:IX 他)</param>   
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCut(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
                                    ByRef intRegIndex As Short, ByRef startpx As Double, ByRef startpy As Double, _
                                    ByRef m As Short, ByRef mm As Short, ByRef cut_type As Short) As Short

        Dim r As Integer
        Dim s As String
        Dim strMSG As String

        Try
            SendTrimDataCut = cFRS_NORMAL                                       ' Return値 = 正常
            'stTCUT.Initialize()                                                ' カットデータ構造体初期化

            ' マーキングデータの判定
            If (intRegIndex = 1000) Then
                m = intRegIndex
                'mm = j
            Else
                '' 指定抵抗番号、ｶｯﾄ番号mとｶｯﾄﾃﾞｰﾀ配列番号mmを返す
                'Call GetResistorCutAddress(intRegIndex, j, m, mm)
                m = i
            End If
            mm = j

            ' 画面で設定したｶｯﾄ形状をINtimeに渡す値に変更
            s = typResistorInfoArray(m).ArrCut(mm).strCutType                   ' ｶｯﾄ形状
            cut_type = Form1.Utility1.GetCutTypeNum(s.Trim())                   ' ｶｯﾄ形状をｶｯﾄ種別に変換(前後の空白を削除)

            ' カットデータを設定する
            With stTCUT
                .wCutNo = j                                                     ' カット番号
                .wDelayTime = typResistorInfoArray(m).ArrCut(mm).intDelayTime   ' 定電流印加後測定遅延時間
                .wCutType = CnvCutTypeNum(cut_type)                             ' カット形状(1:st, 2:L, 3:HK, 4:IX 他)
                '----- V1.22.0.0①↓ -----
                .wMoveMode = typResistorInfoArray(m).ArrCut(mm).intMoveMode     ' 動作モード(0:通常モード,1:ティーチ, 2:強制カットモード)
                '----- V1.22.0.0①↑ -----
                .fCutStartX = typResistorInfoArray(m).ArrCut(mm).dblStartPointX ' カットスタート座標X
                .fCutStartY = typResistorInfoArray(m).ArrCut(mm).dblStartPointY ' カットスタート座標Y
                'V5.0.0.6⑩↓外部カメラカット位置補正量の加算
                If giTeachpointUse = 1 Then
                    If (Not GetCutStartPointAddTeachPoint(m, mm, .fCutStartX, .fCutStartY)) Then
                        .fCutStartX = typResistorInfoArray(m).ArrCut(mm).dblStartPointX ' カットスタート座標X
                        .fCutStartY = typResistorInfoArray(m).ArrCut(mm).dblStartPointY ' カットスタート座標Y
                        Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + m.ToString() + "] CUT=[" + mm.ToString() + "]")
                    End If
                End If
                'V5.0.0.6⑩↑
                startpx = .fCutStartX                                           ' カットスタート座標Xを返す
                startpy = .fCutStartY                                           ' カットスタート座標Yを返す
                '                                                               ' カットオフ %
                If (gSysPrm.stCTM.giSPECIAL = customKOA) Then
                    .fCutOff = typResistorInfoArray(m).ArrCut(mm).dblCutOff + typResistorInfoArray(m).ArrCut(mm).dblCutOffOffset + 100.0#
                Else
                    .fCutOff = typResistorInfoArray(m).ArrCut(mm).dblCutOff + 100.0#
                End If

                'V6.0.0.2③↓
                .fAveDataRate = typResistorInfoArray(m).ArrCut(mm).dblJudgeLevel
                ''----- ###176↓ -----
                '' 切替ポイント(%)を設定する(CHIP用)
                'If (gTkyKnd = KND_CHIP) And (j = 1) Then
                '    .fAveDataRate = typResistorInfoArray(m).ArrCut(mm).dblJudgeLevel
                'Else
                '    .fAveDataRate = 0.0
                'End If
                ''If (gTkyKnd = KND_TKY) Then
                ''    .fAveDataRate = typResistorInfoArray(m).ArrCut(mm).dblJudgeLevel ' 平均化率(0.0～100.0, 0%)(未使用)
                ''End If
                ''----- ###176↑ -----
                'V6.0.0.2③↑

                'ポジショニングなしカットパラメータ送付 '###005
                .wDoPosition = typResistorInfoArray(m).ArrCut(mm).intDoPosition

                ' カット後ポーズタイム（ｍｓ）V1.18.0.3①
                .wCutAftPause = typResistorInfoArray(m).ArrCut(mm).intCutAftPause

                ' 加工条件の設定
                SetCutConditionData(.CutCnd, m, mm)

            End With

            ' カットデータをINtime側に送信する
            r = TRIMDATA_CUTDATA(stTCUT, i, j)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "Cut Data Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCut = 6
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCut = cERR_TRAP
        End Try
    End Function

#End Region

#Region "カット番号を変換する"
    '''=========================================================================
    '''<summary>カット番号を変換する</summary>
    '''<param name="cut_type">(INP)カット番号</param>
    '''<returns>変換後のカット番号</returns>
    '''=========================================================================
    Private Function CnvCutTypeNum(ByVal cut_type As Short) As UShort

        Dim r As UShort

        Select Case cut_type
            Case 1, 6, 8, 10, 12, 14, 28                        ' ST, RETURN/RETRACE + NANAME, ST2
                r = 1
            Case 2, 7, 9, 11, 13, 15                            ' L, (RETURN or RETRACE) + NANAME
                r = 2
            Case 18                                             ' Uカット(リトレース) 'V1.22.0.0① 
                r = 17                                          ' Uカットとする       'V1.22.0.0①
            Case Else
                r = cut_type
        End Select
        Return (r)
    End Function

#End Region

#Region "ST CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>ST CutパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="cut_type">   (INP)カット形状(1:st, 2:L, 3:HK, 4:IX 他)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<param name="lenxy">      (OUT)カッティング長</param>
    '''<param name="dirxy">      (OUT)カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutST(ByRef intTkyType As Short, _
                                      ByRef cut_type As Short, ByRef m As Short, ByRef mm As Short, _
                                      ByRef lenxy As Double, ByRef dirxy As Short, _
                                      ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutST(ByRef intTkyType As Short, _
        '                                  ByRef cut_type As Short, ByRef m As Short, ByRef mm As Short, _
        '                                  ByRef mCut1 As Short, ByRef lenxy As Double, ByRef dirxy As Short, _
        '                                  ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutST(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                  ByRef cut_type As Short, ByRef m As Short, ByRef mm As Short, _
        '                                  ByRef mCut1 As Short, ByRef lenxy As Double, ByRef dirxy As Short, _
        '                                  ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutST As PRM_CUT_ST                                                   ' ST cutパラメータ
        Dim strMSG As String

        Try
            ' ST Cutパラメータを設定する
            SendTrimDataCutST = cFRS_NORMAL                                         ' Return値 = 正常
            With stCutST
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' カッティング長
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle             ' カット角度
                '.angle = 0.0#                                                       ' カット角度(0～359)
                .RtOfs = typResistorInfoArray(m).ArrCut(mm).dblReturnPos            ' RTオフセット(mm) V1.16.0.0①
                lenxy = .Length                                                     ' カッティング長を返す
                lenxy2 = 0
                dirxy2 = 0
                '.DIR = mCut1                                                        ' カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)
                'dirxy = .DIR                                                        ' カット方向を返す

                Select Case cut_type
                    Case 1
                        .MODE = 0                                                   ' 動作モード = 通常
                    Case 6
                        .MODE = 1                                                   ' 動作モード = リターン
                    Case 8
                        .MODE = 2                                                   ' 動作モード = リトレース
                    Case 10
                        .MODE = 4                                                   ' 動作モード = 斜めカット
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' カット角度
                    Case 12
                        .MODE = 5                                                   ' 動作モード = 斜め + リターン
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' カット角度
                    Case 14
                        .MODE = 6                                                   ' 動作モード = 斜め + リトレース
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' カット角度
                    Case 28
                        .MODE = 0                                                   ' 従来のSTと同様
                End Select
            End With

            ' ST CutパラメータをINtime側に送信する
            r = TRIMDATA_CutST(stCutST, m, mm)
            If (r <> cFRS_NORMAL) Then                                              ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "ST Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutST = 7
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutST() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutST = cERR_TRAP
        End Try
    End Function

#End Region

#Region "L CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>L CutパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="cut_type">   (INP)カット形状(1:st, 2:L, 3:HK, 4:IX 他)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<param name="lenxy">      (OUT)カッティング長</param>
    '''<param name="dirxy">      (OUT)カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)カッティング長2</param>
    '''<param name="dirxy2">     (OUT)Lターン方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param>         
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutL(ByRef intTkyType As Short, ByRef cut_type As Short, _
                                      ByRef m As Short, ByRef mm As Short, _
                                      ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutL(ByRef intTkyType As Short, ByRef cut_type As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, ByRef mCut2 As Short, _
        '                                  ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutL(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, ByRef cut_type As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, ByRef mCut2 As Short, _
        '                                  ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutL As PRM_CUT_L                                                     ' L Cutパラメータ
        Dim strMSG As String

        Try
            ' L Cutパラメータを設定する
            SendTrimDataCutL = cFRS_NORMAL                                          ' Return値 = 正常
            With stCutL
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength            ' L1最大カッティング長
                .r = typResistorInfoArray(m).ArrCut(mm).dblR1                       ' ターンの円弧半径
                .turn = typResistorInfoArray(m).ArrCut(mm).dblLTurnPoint            ' Lターンポイント(0.0～100.0(%))
                .L2 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthL           ' L2最大カッティング長
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle             ' カット角度
                .tdir = typResistorInfoArray(m).ArrCut(mm).intLTurnDir              ' Lﾀｰﾝ方向(1:CW, 2:CCW) ※変更
                .RtOfs = typResistorInfoArray(m).ArrCut(mm).dblReturnPos            ' RTオフセット(mm) V1.16.0.0①
                lenxy = .L1                                                         ' L1最大カッティング長を返す
                lenxy2 = .L2                                                        ' L2最大カッティング長を返す

                Select Case cut_type
                    Case 2
                        .MODE = 0                                                   ' 動作モード = 通常
                    Case 7
                        .MODE = 1                                                   ' 動作モード = リターン
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' カット角度
                    Case 9
                        .MODE = 2                                                   ' 動作モード = リトレース
                    Case 11
                        .MODE = 4                                                   ' 動作モード = 斜め
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' カット角度
                    Case 13
                        .MODE = 5                                                   ' 動作モード = 斜め + リターン
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' カット角度
                    Case 15
                        .MODE = 6                                                   ' 動作モード = 斜め + リトレース
                        '.angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle     ' カット角度
                End Select
            End With

            ' L CutパラメータをINtime側に送信する
            r = TRIMDATA_CutL(stCutL, m, mm)
            If (r <> cFRS_NORMAL) Then                                              ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "L Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutL = 8
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutL() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutL = cERR_TRAP
        End Try
    End Function
#End Region

#Region "HOOK CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>HOOK CutパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="cut_type">   (INP)カット形状(1:st, 2:L, 3:HK, 4:IX 他)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<param name="lenxy">      (OUT)カッティング長</param>
    '''<param name="dirxy">      (OUT)カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)カッティング長2</param>
    '''<param name="dirxy2">     (OUT)Lターン方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param>         
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutHOOK(ByRef intTkyType As Short, ByRef cut_type As Short, _
                                         ByRef m As Short, ByRef mm As Short, _
                                         ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutHK As PRM_CUT_HOOK                                             ' HOOK Cutパラメータ
        Dim strMSG As String

        Try
            ' HOOK Cutパラメータを設定する
            SendTrimDataCutHOOK = cFRS_NORMAL                                   ' Return値 = 正常
            With stCutHK
                .MODE = typResistorInfoArray(m).ArrCut(mm).intMoveMode          ' 動作モード(0:NOM, 1:リターン, 2:リトレース, 3:斜め)
                '----- V1.22.0.0①↓ -----
                If (cut_type = 18) Then                                         ' U CUT(RETRACE) ? 
                    .MODE = 2                                                   ' 動作モード = 2(リトレース)
                End If
                '----- V1.22.0.0①↑ -----
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' 斜めカット角度(0～359)
                .tdir = typResistorInfoArray(m).ArrCut(mm).intLTurnDir          ' Lﾀｰﾝ方向(1:CW, 2:CCW) ※変更
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' L1 最大カッティング長
                .r1 = typResistorInfoArray(m).ArrCut(mm).dblR1                  ' ターン1の円弧半径
                .turn = typResistorInfoArray(m).ArrCut(mm).dblLTurnPoint        ' Lターンポイント
                .L2 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthL       ' L2 最大カッティング長
                .r2 = typResistorInfoArray(m).ArrCut(mm).dblR2                  ' ターン2の円弧半径
                .L3 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthHook    ' L3 最大カッティング長
                lenxy = .L1                                                     ' L1 最大カッティング長を返す
                lenxy2 = .L2                                                    ' L2 最大カッティング長を返す
            End With

            ' HOOK CutパラメータをINtime側に送信する
            r = TRIMDATA_CutHK(stCutHK, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                If cut_type = 3 Then                                            ' HOOK Cut ? 
                    Call Form1.System1.TrmMsgBox(gSysPrm, "HOOK Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataCutHOOK = 9
                ElseIf cut_type = 17 Then                                       ' U Cut ? 
                    Call Form1.System1.TrmMsgBox(gSysPrm, "U Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                    SendTrimDataCutHOOK = 14
                End If
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutHOOK() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutHOOK = cERR_TRAP
        End Try

    End Function
#End Region

#Region "INDEX CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>INDEX CutパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  param> 
    '''<param name="lenxy">      (OUT)インデックス長</param>
    '''<param name="dirxy">      (OUT)カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)0を返す</param>
    '''<param name="dirxy2">     (OUT)0を返す</param>         
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutINDEX(ByRef intTkyType As Short, _
                                          ByRef m As Short, ByRef mm As Short, _
                                          ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutINDEX(ByRef intTkyType As Short, _
        '                                      ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                      ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutINDEX(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                      ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                      ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutIX As PRM_CUT_INDEX                                            ' INDEX Cutパラメータ
        Dim strMSG As String

        Try
            ' INDEX Cutパラメータを設定する
            SendTrimDataCutINDEX = cFRS_NORMAL                                  ' Return値 = 正常
            With stCutIX
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' 斜めカット角度(0～359)
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength    ' インデックス長
                .maxindex = typResistorInfoArray(m).ArrCut(mm).intIndexCnt      ' インデックス数
                .measMode = typResistorInfoArray(m).ArrCut(mm).intMeasMode      ' 測定モード(0:抵抗, 1:電圧)
                .measType = typResistorInfoArray(m).ArrCut(mm).intMeasType      ' 測定判定タイプ 0:高速, 1:高精度, 2:外部)
                .LimitLen = typResistorInfoArray(m).ArrCut(mm).dblLimitLen      ' IXカットのリミット長 'V1.18.0.0④
                '----- V6.1.1.0⑥↓ (インデックスＷカット用) -----
                .lines = typResistorInfoArray(m).ArrCut(mm).intCutCnt           ' シフト回数(1～n)               (SCANカットの本数域使用)
                .pitch = typResistorInfoArray(m).ArrCut(mm).dblPitch            ' シフト量(±0.0001～20.0000(mm))(SCANカットのピッチ域使用)
                '----- V6.1.1.0⑥↑ -----

                '----- ###229↓ -----
                ' シスパラがGP-IB制御あり(2:汎用)で測定タイプが外部ならGP-IB制御(汎用)フラグを制御ありとする
                If (gSysPrm.stCTM.giGP_IB_flg = 2) And (.measType = 2) Then
                    bGpib2Flg = 1                                               ' GP-IB制御(汎用)フラグ(0=制御なし, 1=制御あり)
                End If
                '----- ###229↑ -----
                lenxy = .Length                                                 ' インデックス長を返す
                lenxy2 = 0
                dirxy2 = 0
                '.DIR = mCut1                                                    ' カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)
                'dirxy = .DIR                                                    ' カット方向を返す
            End With

            ' INDEX CutパラメータをINtime側に送信する
            r = TRIMDATA_CutIX(stCutIX, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "IX Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutINDEX = 10
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutINDEX() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutINDEX = cERR_TRAP
        End Try

    End Function
#End Region

#Region "INDEX2 CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>INDEX2 CutパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<param name="lenxy">      (OUT)インデックス長</param>
    '''<param name="dirxy">      (OUT)カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param>  
    '''<param name="lenxy2">     (OUT)0を返す</param>
    '''<param name="dirxy2">     (OUT)0を返す</param>         
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutINDEX2(ByRef intTkyType As Short, _
                                           ByRef m As Short, ByRef mm As Short, _
                                           ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        'Public Function SendTrimDataCutINDEX2(ByRef intTkyType As Short, _
        '                                       ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                       ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short
        '    'Public Function SendTrimDataCutINDEX2(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                       ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short, _
        '                                       ByRef lenxy As Double, ByRef dirxy As Short, ByRef lenxy2 As Double, ByRef dirxy2 As Short) As Short

        Dim r As Integer
        Dim stCutIX2 As PRM_CUT_INDEX                                           ' INDEX2 cutパラメータ
        Dim strMSG As String

        Try
            ' INDEX2 Cut(ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しｲﾝﾃﾞｯｸｽｶｯﾄ)パラメータを設定する
            SendTrimDataCutINDEX2 = cFRS_NORMAL                                 ' Return値 = 正常
            With stCutIX2
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength    ' インデックス長
                .maxindex = typResistorInfoArray(m).ArrCut(mm).intIndexCnt      ' インデックス数
                .measMode = typResistorInfoArray(m).ArrCut(mm).intMeasMode      ' 測定モード(0:抵抗, 1:電圧, 3:外部)
                .measType = typResistorInfoArray(m).ArrCut(mm).intMeasType      ' 測定タイプ 0:高速, 1:高精度, 3:)
                .LimitLen = typResistorInfoArray(m).ArrCut(mm).dblLimitLen      ' IXカットのリミット長 'V1.18.0.0④
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' 斜めカット角度(0～359)
                lenxy = .Length                                                 ' インデックス長を返す
                lenxy2 = 0
                dirxy2 = 0
                '.DIR = mCut1                                                    ' カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)
                'dirxy = .DIR                                                    ' カット方向を返す
            End With

            ' INDEX2 CutパラメータをINtime側に送信する
            r = TRIMDATA_CutIX(stCutIX2, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "IX2 Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutINDEX2 = 10
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutINDEX2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutINDEX2 = cERR_TRAP
        End Try

    End Function

#End Region

#Region "SCAN CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>SCAN CutパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutSCAN(ByRef intTkyType As Short, _
                                         ByRef m As Short, ByRef mm As Short) As Short
        'Public Function SendTrimDataCutSCAN(ByRef intTkyType As Short, _
        '                                     ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short
        'Public Function SendTrimDataCutSCAN(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                     ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short

        Dim r As Integer
        'Dim Cdir As Short
        Dim stCutSC As PRM_CUT_SCAN                                             ' SCAN Cutパラメータ
        Dim strMSG As String

        Try
            ' SCAN Cut(ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しｲﾝﾃﾞｯｸｽｶｯﾄ)パラメータを設定する
            SendTrimDataCutSCAN = cFRS_NORMAL                                   ' Return値 = 正常
            With stCutSC
                .Length = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength    ' カッティング長
                .pitch = typResistorInfoArray(m).ArrCut(mm).dblPitch            ' ピッチ
                .lines = typResistorInfoArray(m).ArrCut(mm).intCutCnt           ' 本数
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' 斜めカット角度(0～359)
                .sdir = typResistorInfoArray(m).ArrCut(mm).intStepDir            ' ステップ方向(1:→(+X), 2:←(-X), 3:↑(+Y), 4:↓(-Y))
                'Cdir = typResistorInfoArray(m).ArrCut(mm).intStepDir            ' ステップ方向(1:→(+X), 2:←(-X), 3:↑(+Y), 4:↓(-Y))
                '.sdir = Form1.Utility1.ConvertVbdir2Rtdir(Cdir)                         ' ｽﾃｯﾌﾟ方向変換(SCANカット用)(1:+X,2:-Y,3:-X,4:+Y)→(1:+X,4:-Y,2:-X,3:+Y)
                ''.DIR = mCut1                                                    
            End With

            ' SCAN CutパラメータをINtime側に送信する
            r = TRIMDATA_CutSC(stCutSC, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "SCAN Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutSCAN = 11
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutSCAN() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutSCAN = cERR_TRAP
        End Try

    End Function
#End Region

#Region "文字マーキングパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>文字マーキングパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<param name="mCut1">      (INP)カット方向(1:+X, 2:-X, 3:+Y, 4:-Y)</param> 
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutMarking(ByRef intTkyType As Short, _
                                            ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short
        'Public Function SendTrimDataCutMarking(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                        ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short

        Dim r As Integer
        Dim s As String
        Dim n As Short
        Dim nn As Short
        Dim markDir(4) As Short
        'Dim stCutMK As PRM_CUT_MARKING                                         ' Letter Markingパラメータ
        Dim strMSG As String

        Try
            ' 文字ﾏｰｷﾝｸﾞパラメータを設定する
            SendTrimDataCutMarking = cFRS_NORMAL                                ' Return値 = 正常
            'stCutMK.Initialize()


            ' 文字ﾏｰｷﾝｸﾞ方向
            'markDir(0) = 1                                                      ' 0
            'markDir(1) = 1                                                      ' +X 0
            'markDir(2) = 4                                                      ' -Y 270
            'markDir(3) = 3                                                      ' -X 180
            'markDir(4) = 2                                                      ' +Y 90

            ' 文字マーキングパラメータを設定する
            With stCutMK
                '.DIR = markDir(mCut1)
                .DIR = mCut1 / 90
                .magnify = Val(typResistorInfoArray(m).ArrCut(mm).dblZoom)      ' 倍率
                s = typResistorInfoArray(m).ArrCut(mm).strChar                  ' マーキング文字列
                For n = 0 To (cMAXcMARKINGcSTRLEN - 1)
                    .str(n) = 0 : Next
                nn = Len(s)
                If nn > cMAXcMARKINGcSTRLEN Then nn = cMAXcMARKINGcSTRLEN
                For n = 1 To nn
                    .str(n - 1) = Asc(Mid(s, n, 1))
                Next
            End With

            ' 文字マーキングパラメータをINtime側に送信する
            r = TRIMDATA_CutMK(stCutMK, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "Marking Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutMarking = 12
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutMarking() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutMarking = cERR_TRAP
        End Try

    End Function
#End Region

#Region "ES CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>ES CutパラメータをINtime側に送信する</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutES(ByRef intTkyType As Short, _
                                      ByRef m As Short, ByRef mm As Short) As Short
        'Public Function SendTrimDataCutES(ByRef intTkyType As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short
        '    'Public Function SendTrimDataCutES(ByRef intTkyType As Short, ByRef i As Short, ByRef j As Short, _
        '                                  ByRef m As Short, ByRef mm As Short, ByRef mCut1 As Short) As Short

        Dim r As Integer
        Dim stCutES As PRM_CUT_ES                                               ' ES cutパラメータ
        Dim strMSG As String

        Try
            ' ES Cutパラメータを設定する
            SendTrimDataCutES = cFRS_NORMAL                                     ' Return値 = 正常
            With stCutES
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' L1 最大カッティング長
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' 斜めカット角度(0～359)
                '.DIR = mCut1
                .EsPoint = typResistorInfoArray(m).ArrCut(mm).dblESPoint        ' ESポイント
                .ESWide = typResistorInfoArray(m).ArrCut(mm).dblESJudgeLevel    ' ES判定変化率
                .ESWide2 = typResistorInfoArray(m).ArrCut(mm).dblESChangeRatio  ' ES後変化率(0.0～100.0%)
                .EScount = typResistorInfoArray(m).ArrCut(mm).intESConfirmCnt   ' ES後確認回数(0～20)
                .CTcount = typResistorInfoArray(m).ArrCut(mm).intCTcount        ' ｴｯｼﾞｾﾝｽ後連続NG確認回数　
                .wJudgeNg = typResistorInfoArray(m).ArrCut(mm).intJudgeNg       ' NG判定する/しない（0:TRUE/1:FALSE）
            End With

            ' ES CutパラメータをINtime側に送信する
            r = TRIMDATA_CutES(stCutES, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "ES Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutES = 20
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutES() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutES = cERR_TRAP
        End Try

    End Function
#End Region

#Region "ES CutパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>ES CutパラメータをINtime側に送信する V1.14.0.0①</summary>
    '''<param name="intTkyType"> (INP)TKY種別(0:TKY, 1:CHIP, 2:NET)</param>
    '''<param name="m">          (INP)抵抗データ・インデックス</param> 
    '''<param name="mm">         (INP)カットデータ・インデックス</param>  
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutES0(ByRef intTkyType As Short, _
                                      ByRef m As Short, ByRef mm As Short) As Short

        Dim r As Integer
        Dim stCutES0 As PRM_CUT_ES0                                               ' ES cutパラメータ
        Dim strMSG As String

        Try
            ' ES Cutパラメータを設定する
            SendTrimDataCutES0 = cFRS_NORMAL                                     ' Return値 = 正常
            With stCutES0
                .L1 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLength        ' L1 最大カッティング長
                .angle = typResistorInfoArray(m).ArrCut(mm).intCutAngle         ' 斜めカット角度(0～359)
                '.DIR = mCut1
                .EsPoint = typResistorInfoArray(m).ArrCut(mm).dblESPoint        ' ESポイント
                .ESWide = typResistorInfoArray(m).ArrCut(mm).dblESJudgeLevel    ' ES判定変化率
                .L2 = typResistorInfoArray(m).ArrCut(mm).dblMaxCutLengthES       ' L2 ＥＳ後カット長(0.0001～20.0000(mm))
                .wJudgeNg = typResistorInfoArray(m).ArrCut(mm).intJudgeNg       ' NG判定する/しない（0:TRUE/1:FALSE）
            End With

            ' ES CutパラメータをINtime側に送信する
            r = TRIMDATA_CutES0(stCutES0, m, mm)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "ES0 Cut Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutES0 = 20
                Exit Function
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutES0() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutES0 = cERR_TRAP
        End Try

    End Function
#End Region

#Region "UCutパラメータの送信"
    '''=========================================================================
    '''<summary>UCutパラメータの送信</summary>
    '''<param name="sParamFn">(INP)Uカットデータ名</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''<remarks>TKYより移植：TKY専用とするかは後に検討</remarks>
    '''=========================================================================
    Public Function SendUCutParam(ByVal sParamFn As String) As Short

        Dim sPath As String
        Dim RNO As Short
        Dim RATIO As Double
        Dim NOM As Double
        Dim LTP As Double
        Dim LTP2 As Double
        Dim L1 As Double
        Dim L2 As Double
        Dim r As Double
        Dim V As Double
        Dim n As Short
        'Dim fn As Short
        Dim indx As Short
        Dim EL As Short
        Dim maxcnt As Short
        Dim prevrno As Short
        Dim s As String
        Dim nlines As Integer

        ' Ｕカットデータ名無し ?
        If (sParamFn = "") Then
            SendUCutParam = 0
            Exit Function
        End If

        'sParamFn = Trim(sParamFn)
        sParamFn = sParamFn.Trim()
        If (sParamFn = "") Then
            MsgBox("UCUT DATA FILE NAME ERROR", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT DATA ERROR")
            SendUCutParam = 1
            Exit Function
        End If

        ' R番号、初期値％、LTP、L1、L2、RADIUS、V
        '----- V1.19.0.0⑤↓ -----
        'sPath = gStrTrimFileName                                        ' sPath = トリミングデータ名 
        'n = InStrRev(sPath, "\")
        'If n = 0 Then
        '    sPath = My.Application.Info.DirectoryPath
        'Else
        '    sPath = Left(sPath, n)
        'End If
        'If Right(sPath, 1) <> "\" Then                                  ' sPath = トリミングデータパス名+ "\" 
        '    sPath = sPath & "\"
        'End If
        'sPath = sPath & sParamFn & ".csv"

        sPath = sParamFn                                                ' sPath = Uカットデータ名
        '----- V1.19.0.0⑤↑ -----
        'If Dir(sPath) = "" Then
        If (False = IO.File.Exists(sPath)) Then
            SendUCutParam = 2
            MsgBox("UCUT DATA ERROR", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT DATA FILE READ ERROR")
            Exit Function
        End If

        'On Error GoTo ErrTrap
        Try                                                             'V4.4.0.0-0
            'fn = FreeFile()
            'FileOpen(fn, sPath, OpenMode.Input)
            Using sr As New StreamReader(sPath, Encoding.GetEncoding("Shift_JIS")) ' 全角なし互換性維持  V4.4.0.0-0
                indx = -1
                nlines = 0
                prevrno = -1
                'Do While Not EOF(fn)
                Do While (False = sr.EndOfStream)                       'V4.4.0.0-0
                    's = LineInput(fn)
                    s = sr.ReadLine()                                   'V4.4.0.0-0
                    If n < 1 And s = "" Then Exit Do ' データ最後がNULLの場合は強制的に終了させる。
                    nlines = nlines + 1
                    'If Left(s, 1) <> ";" Then GoTo ErrFile
                    If (False = s.StartsWith(";")) Then GoTo ErrFile '   V4.4.0.0-0
                    'If EOF(fn) Then Exit Do
                    If (True = sr.EndOfStream) Then Exit Do '            V4.4.0.0-0
                    's = LineInput(fn)
                    s = sr.ReadLine()                                   'V4.4.0.0-0
                    nlines = nlines + 1
                    'If Left(s, 1) <> ";" Then GoTo ErrFile
                    If (False = s.StartsWith(";")) Then GoTo ErrFile '   V4.4.0.0-0
                    'If EOF(fn) Then Exit Do
                    If (True = sr.EndOfStream) Then Exit Do '            V4.4.0.0-0
                    's = LineInput(fn)
                    s = sr.ReadLine()                                   'V4.4.0.0-0
                    nlines = nlines + 1
                    'If Left(s, 1) <> ";" Then GoTo ErrFile
                    If (False = s.StartsWith(";")) Then GoTo ErrFile '   V4.4.0.0-0

                    'Do While Not EOF(fn)
                    Do While (False = sr.EndOfStream)                   'V4.4.0.0-0

                        s = sr.ReadLine()                               'V4.4.0.0-0
                        Dim splt() As String
                        Try
                            splt = s.Replace(" ", String.Empty).Split(","c)
                        Catch ex As Exception
                            GoTo ErrData
                        End Try

                        Select Case gSysPrm.stSPF.giUCutKind
                            Case 1 ' 電子技研
                                If (9 <> splt.Length) Then GoTo ErrData 'V4.4.0.0-0
                                'Input(fn, RNO)
                                If (False = Short.TryParse(splt(0), RNO)) Then GoTo ErrData
                                'Input(fn, n)
                                If (False = Short.TryParse(splt(1), n)) Then GoTo ErrData
                                'Input(fn, RATIO)
                                If (False = Double.TryParse(splt(2), RATIO)) Then GoTo ErrData
                                'Input(fn, NOM)
                                If (False = Double.TryParse(splt(3), NOM)) Then GoTo ErrData
                                'Input(fn, LTP)
                                If (False = Double.TryParse(splt(4), LTP)) Then GoTo ErrData
                                'Input(fn, L1)
                                If (False = Double.TryParse(splt(5), L1)) Then GoTo ErrData
                                'Input(fn, L2)
                                If (False = Double.TryParse(splt(6), L2)) Then GoTo ErrData
                                'Input(fn, r)
                                If (False = Double.TryParse(splt(7), r)) Then GoTo ErrData
                                'Input(fn, V)
                                If (False = Double.TryParse(splt(8), V)) Then GoTo ErrData
                                nlines = nlines + 1

                                If n < 1 Then Exit Do

                                If LTP < 0.0# OrElse LTP > 100.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= LTP <= 100.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If NOM < 0.0001 OrElse NOM > 1000000000 Then '0.1mΩ～１ＧΩ
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0001 <= NOM <= 1000000000", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L1 < 0.0# OrElse L1 > 20.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= L1 <= 20.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L2 < 0.0# OrElse L2 > 20.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= L2 <= 20.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If r < 0.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " r >= 0.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If V < 0.1 OrElse V > 409.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " V >= 0.1", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L1 < r Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " L1 < r" & vbCrLf & "L1 length must be greater than r or equal.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If L2 < (r * 2) Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " L2 < (r*2)" & vbCrLf & "L2 length must be greater than (r*2) or equal.", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If RNO <> prevrno Then
                                    indx = indx + 1
                                End If
                                prevrno = RNO

                                EL = n - 1

                                stUCutPrm(indx).EL(EL).RNO = RNO
                                stUCutPrm(indx).EL(EL).NOM = NOM
                                stUCutPrm(indx).EL(EL).PRM_UNIT.RATIO = RATIO
                                stUCutPrm(indx).EL(EL).PRM_UNIT.LTP = LTP
                                stUCutPrm(indx).EL(EL).PRM_UNIT.L1 = L1
                                stUCutPrm(indx).EL(EL).PRM_UNIT.L2 = L2
                                stUCutPrm(indx).EL(EL).PRM_UNIT.r = r
                                stUCutPrm(indx).EL(EL).PRM_UNIT.V = V
                                stUCutPrm(indx).EL(EL).PRM_UNIT.Flg = 1

                                ' 種別＝日立の場合
                            Case 2 '日立
                                If (5 <> splt.Length) Then GoTo ErrData 'V4.4.0.0-0
                                'Input(fn, RNO)
                                If (False = Short.TryParse(splt(0), RNO)) Then GoTo ErrData
                                'Input(fn, n)
                                If (False = Short.TryParse(splt(1), n)) Then GoTo ErrData
                                'Input(fn, RATIO)
                                If (False = Double.TryParse(splt(2), RATIO)) Then GoTo ErrData
                                'Input(fn, LTP)
                                If (False = Double.TryParse(splt(3), LTP)) Then GoTo ErrData
                                'Input(fn, LTP2)
                                If (False = Double.TryParse(splt(4), LTP2)) Then GoTo ErrData
                                nlines = nlines + 1
                                If n < 1 Then Exit Do

                                If LTP < 0.0# OrElse LTP > 100.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= LTP <= 100.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If LTP2 < 0.0# OrElse LTP2 > 100.0# Then
                                    MsgBox("UCUT PARAMETER ERROR : line#" & nlines.ToString("0") & " 0.0 <= LTP2 <= 100.0", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT ERROR")
                                    GoTo ErrData
                                End If

                                If RNO <> prevrno Then
                                    indx = indx + 1
                                End If
                                prevrno = RNO

                                EL = n - 1
                                stUCutPrm(indx).EL(EL).RNO = RNO
                                stUCutPrm(indx).EL(EL).PRM_UNIT.Flg = 1
                                stUCutPrm(indx).EL(EL).PRM_UNIT.RATIO = RATIO
                                stUCutPrm(indx).EL(EL).PRM_UNIT.LTP = LTP
                                stUCutPrm(indx).EL(EL).PRM_UNIT.LTP2 = LTP2
                        End Select
                    Loop

                Loop

                'FileClose(fn)
                'On Error GoTo 0
            End Using

            maxcnt = indx + 1

            ' UCUTパラメータ設定(モード(0=メモリアロケート))
            'Call UCUT_PARAMSET(0, maxcnt, 0, 0, 0, stUCutPrm(0).EL(0).PRM_UNIT)
            Call UCUT_PARAMSET(0, 0, maxcnt, 0, 0, stUCutPrm(0).EL(0).PRM_UNIT) ' V1.19.0.0⑤

            For indx = 0 To maxcnt - 1
                For EL = 0 To 19
                    If stUCutPrm(indx).EL(EL).PRM_UNIT.Flg Then
                        ' 特注Ｕカット種別により切り分け
                        Select Case gSysPrm.stSPF.giUCutKind
                            Case 1 ' 電子技研
                                Call UCUT_PARAMSET(1, 1, stUCutPrm(indx).EL(EL).RNO, indx, EL, stUCutPrm(indx).EL(EL).PRM_UNIT)
                            Case 2 '日立
                                Call UCUT_PARAMSET(1, 2, stUCutPrm(indx).EL(EL).RNO, indx, EL, stUCutPrm(indx).EL(EL).PRM_UNIT)
                        End Select
                    End If
                Next
            Next

            Exit Function

        Catch ex As Exception
            'ErrTrap:
            '            Resume ErrTrap1
            'ErrTrap1:
            '            On Error GoTo 0
            MsgBox("UCUT DATA ERROR", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT DATA FILE READ ERROR")
            SendUCutParam = 2
            Exit Function
        End Try

ErrFile:
        'FileClose(fn)
        'On Error GoTo 0
        MsgBox("File format error", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "UCUT PARAMETER FILE")
        SendUCutParam = 3
        Exit Function

ErrData:
        'FileClose(fn)
        'On Error GoTo 0
        SendUCutParam = 4
        Exit Function

    End Function

#End Region

#Region "ｶｯﾄを行う(CUT2)"
    '''=========================================================================
    '''<summary>ｶｯﾄを行う(CUT2)</summary>
    '''<param name="dblLength">(INP)移動量</param>
    '''<param name="intDir">   (INP)方向</param> 
    '''<param name="dblSpeed"> (INP)速度</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function cut(ByVal dblLength As Double, ByVal intDir As Short, ByVal dblSpeed As Double) As Integer

        Dim strMSG As String

        Try
            ' 将来ｶｯﾄ方向が1度単位になったとき使用
            If (1 = intDir) Then
                intDir = 180
            ElseIf (2 = intDir) Then
                intDir = 90
            ElseIf (3 = intDir) Then
                intDir = 270
            End If

#If cOFFLINEcDEBUG = 1 Then
        cut = 0
#Else
            cut = CUT2(dblLength, dblSpeed, intDir)
#End If
            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.cut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            cut = cERR_TRAP
        End Try

    End Function

#End Region

#Region "原点復帰処理(ｽﾀｰﾄSW押下待ちｵﾌﾟｼｮﾝ対応)"
    '''=========================================================================
    '''<summary>原点復帰処理(ｽﾀｰﾄSW押下待ちｵﾌﾟｼｮﾝ対応)</summary>
    '''<returns>0:正常, 0以外:エラー</returns> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Function sResetStart() As Short

        Dim strMSG As String
        Dim r As Short
        Dim coverSts As Long                                            ' ###215

        Try
            ' 原点復帰
            sResetStart = cFRS_NORMAL                                   ' Return値 = 正常
            '----- ###215↓----- 
            r = Form1.DispInterLockSts()
            ' インターロック全解除/一部解除で、カバー閉は異常とする(SL436R時) 
            If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                'V4.0.0.0⑭If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gKeiTyp <> KEY_TYPE_RS) Then  'V4.0.0.0⑭

                r = COVER_CHECK(coverSts)                               ' 固定カバー状態取得(0=固定カバー開, 1=固定カバー閉))
                If (coverSts = 1) Then                                  ' 固定カバー閉 ?
                    ' ハードウェアエラー(カバースイッチオン)メッセージ表示
                    Call Form1.System1.Form_Reset(cGMODE_ERR_HW, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                    Return (cFRS_ERR_HW)                                ' アプリ強制終了
                End If
            End If
            '----- ###215↑ -----

            Call COVERCHK_ONOFF(COVER_CHECK_ON)                         ' 固定カバーチェックあり ###088
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                ' SL436系の場合
                Call SLIDECOVERCHK(SLIDECOVER_CHECK_OFF)                ' スライドカバーチェックなし

                '----- ###185↓ ----- 
                ' 載物台に基板がある場合は、取り除かれるまで待つ(ローダが原点復帰できないため)
                r = SubstrateCheck(Form1.System1, APP_MODE_LOADERINIT)
                If (r < cFRS_NORMAL) Then
                    sResetStart = r                                     ' Return値設定
                    Exit Function
                End If
                '----- ###185↑ ----- 
                '----- ###197↓ ----- 
                ' NG排出BOXが満杯の場合は、取り除かれるまで待つ(ローダが原点復帰できないため)
                r = NgBoxCheck(Form1.System1, APP_MODE_LOADERINIT)
                If (r < cFRS_NORMAL) Then
                    sResetStart = r                                     ' Return値設定
                    Exit Function
                End If
                '----- ###197↑ ----- 
                Call W_RESET()                                          ' アラームリセット送出 ###073
                Call W_START()                                          ' スタート信号送出 ###073
                r = Loader_OrgBack(Form1.System1)                       ' 原点復帰処理(オートローダ/トリマ)
                ' ↓↓↓V3.1.0.0① 2014/11/28 原点復帰時に第二原点位置に移動する。
#If START_KEY_SOFT Then
                If gbStartKeySoft Then
                    Call ZCONRST()
                End If
#End If
                '----- V2.0.0.0⑧↓ -----
                If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then        ' エラー ?
                    sResetStart = r                                     ' Return値設定
                    Exit Function
                End If
                If (r = cFRS_ERR_RST) Then
                    sResetStart = r                                     ' Return値設定 'V2.0.0.0⑭
                    Exit Function
                End If

                ' 第二原点位置にステージを移動する
                If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                    'Home位置移動
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_STAGE_HOMEMOVE)
                    'SubHomeMove()
                    'r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                    'If (r < cFRS_NORMAL) Then                           ' エラー ?(※エラーメッセージは表示済み) 
                    '    sResetStart = r                                 ' Return値設定
                    '    Exit Function
                    'End If
                End If
                '----- V2.0.0.0⑧↑ -----
                ' ↑↑↑V3.1.0.0① 2014/11/28 原点復帰時に第二原点位置に移動する。
            Else
                ' SL432系の場合
                Call SLIDECOVERCHK(SLIDECOVER_CHECK_ON)                 ' スライドカバーチェックあり ###088
                r = Form1.System1.Form_Reset(cGMODE_ORG, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                '----- V1.13.0.0⑧↓ -----
                If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then        ' エラー ?
                    sResetStart = r                                     ' Return値設定
                    Exit Function
                End If
                ' 第二原点位置にステージを移動する
                'V5.0.0.6②                If (r = cFRS_NORMAL) And ((gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0)) Then
                'V5.0.0.6②                r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                If (r = cFRS_NORMAL) And ((GetLoaderBordTableOutPosX() <> 0) Or (GetLoaderBordTableOutPosY() <> 0)) Then
                    r = Form1.System1.EX_SMOVE2(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                End If
                '----- V1.13.0.0⑧↑ -----
            End If
            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then            ' エラー ?
                sResetStart = r                                         ' Return値設定
                Exit Function
            End If
            sResetStart = r

            ' ｽﾀｰﾄSW押下待ち(ｵﾌﾟｼｮﾝ)時はｽﾗｲﾄﾞｶﾊﾞｰ自動ｵｰﾌﾟﾝしないので
            ' ﾒｯｾｰｼﾞ表示後ｽﾗｲﾄﾞｶﾊﾞｰ開待ち
            If ((Form1.System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0) And _
                ((gSysPrm.stSPF.giWithStartSw = 1) And gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then ' ｲﾝﾀｰﾛｯｸ時でｽﾀｰﾄSW押下待ち(ｵﾌﾟｼｮﾝ) ?
                r = Form1.System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then        ' エラー ?
                    sResetStart = r                                     ' Return値設定
                    Exit Function
                End If
            End If

            ' クランプ/吸着OFF
            If (bFgAutoMode = False) Then                               ' 自動運転時はクランプ及びバキュームOFFしない ###107
                r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)
            End If

            'V5.0.0.9⑭
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_IDLE)
            'V5.0.0.9⑭

            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                sResetStart = r                                         ' Return値設定
                Exit Function
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "gwModule.sResetStart() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            sResetStart = cERR_TRAP                                     ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "原点位置移動(絶対移動)"
    '''=========================================================================
    '''<summary>原点位置移動(絶対移動)</summary>
    '''<returns>0:正常, 0以外:エラー</returns> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Function sResetTrim() As Short

        Dim strMSG As String
        Dim r As Short

        Try
            ' 原点位置移動
            sResetTrim = cFRS_NORMAL                        ' Return値 = 正常
            r = Form1.System1.Form_Reset(cGMODE_ORG_MOVE, gSysPrm, giAppMode, False, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            If (r <> cFRS_NORMAL) Then                      ' エラー ? 
                If (r <= cFRS_ERR_EMG) Then                 ' 非常停止等 ? 
                    Call Form1.AppEndDataSave()
                    Call Form1.AplicationForcedEnding()     ' 強制終了
                End If
                sResetTrim = r                              ' Return値設定
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "gwModule.sResetTrim() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "Z移動(相対/絶対)"
    '''=========================================================================
    '''<summary>Z移動(相対/絶対)</summary>
    '''<param name="z"> (INP)移動量</param> 
    '''<param name="MD">(INP)0=相対移動, 1=絶対移動</param> 
    '''<returns>0=正常, 0以外:エラー</returns>
    '''=========================================================================
    Public Function EX_ZMOVE(ByVal z As Double, ByVal MD As Integer) As Integer

        Dim r As Integer
        Dim opt As Integer                                  ' Z軸原点復帰ﾁｪｯｸﾌﾗｸﾞ(0:無, 1:Z軸 2:Z軸)
        Dim strMSG As String

        Try
            ' ZなしならNOP
            If (gSysPrm.stDEV.giPrbTyp = 0) Then
                EX_ZMOVE = cFRS_NORMAL
                Exit Function
            End If

            ' Z軸原点復帰ﾁｪｯｸﾌﾗｸﾞを設定する
            If (MD = 1) And (z = 0.0#) Then                 ' 原点移動 ?
                opt = 1                                     ' ﾌﾗｸﾞ = Z軸原点復帰ﾁｪｯｸ有り
            Else
                opt = 0                                     ' ﾌﾗｸﾞ = Z軸原点復帰ﾁｪｯｸ無し
            End If

            ' Z移動
            r = ZZMOVE(z, MD)
            EX_ZMOVE = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)

            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "gwModule.EX_ZMOVE() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            EX_ZMOVE = cERR_TRAP                            ' Return値 = 例外エラー
        End Try

    End Function

#End Region

#Region "Z2移動(相対/絶対)"
    '''=========================================================================
    '''<summary>Z2移動(相対/絶対)</summary>
    '''<param name="z"> (INP)移動量</param> 
    '''<param name="MD">(INP)0=相対移動, 1=絶対移動</param> 
    '''<returns>0=正常, 0以外:エラー</returns>
    '''=========================================================================
    Public Function EX_ZMOVE2(ByVal z As Double, ByVal MD As Integer) As Integer

        Dim r As Integer
        Dim opt As Integer                                  ' Z軸原点復帰ﾁｪｯｸﾌﾗｸﾞ(0:無, 1:Z軸 2:Z軸)
        Dim strMSG As String

        Try
            ' 下方ﾌﾟﾛｰﾌﾞなしならNOP
            If ((gSysPrm.stDEV.giPrbTyp And 2) = 0) Then
                EX_ZMOVE2 = cFRS_NORMAL
                Exit Function
            End If

            ' Z軸原点復帰ﾁｪｯｸﾌﾗｸﾞを設定する
            If (MD = 1) And (z = 0.0#) Then                 ' 原点移動 ?
                opt = 1                                     ' ﾌﾗｸﾞ = Z軸原点復帰ﾁｪｯｸ有り
            Else
                opt = 0                                     ' ﾌﾗｸﾞ = Z軸原点復帰ﾁｪｯｸ無し
            End If

            ' Z2移動
            r = ZZMOVE2(z, MD)
            EX_ZMOVE2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)

            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "gwModule.EX_ZMOVE2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            EX_ZMOVE2 = cERR_TRAP                           ' Return値 = 例外エラー
        End Try

    End Function

#End Region

#Region "ｽﾃｯﾌﾟX,Yの値を再計算する"
    '''=========================================================================
    '''<summary>ｽﾃｯﾌﾟX,Yの値を再計算する</summary>
    '''<param name="intFirstCnt">(INP)</param>
    '''<param name="blnUpdate">  (INP)</param> 
    '''<returns></returns>
    '''=========================================================================
    Public Function GetTy2StepPos(ByRef intFirstCnt As Short, ByRef blnUpdate As Boolean) As Short

        Dim intForCnt As Short
        Dim fTblOffsetX As Double
        Dim fTblOffsetY As Double
        Dim blockx As Double
        Dim blocky As Double
        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim gspacex As Double
        Dim gspacey As Double
        Dim dblStepOffX As Double
        Dim dblStepOffY As Double
        Dim blockIntervalx As Double
        Dim blockIntervaly As Double
        Dim iCDir As Short
        Dim iStepRepeat As Short
        Dim BNX As Short
        Dim BNY As Short
        Dim GNx As Short
        Dim GNy As Short
        Dim BlkNum As Short
        Dim tSetNextXY As TRIM_GETNEXTXY                ' 次ﾌﾞﾛｯｸへの移動位置取得用の構造体
        Dim dblX As Double
        Dim dblY As Double
        Dim intGrpCnt As Short
        Dim intCntBlockNum As Short
        Dim GrpNum As Short

        intGrpCnt = 1
        intCntBlockNum = 0

        ' チップ並び方向取得(CHIP-NETのみ)
        iCDir = typPlateInfo.intResistDir
        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの取得(0:なし、1:X, 2:Y)
        iStepRepeat = typPlateInfo.intDirStepRepeat
        ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄの取得
        fTblOffsetX = typPlateInfo.dblTableOffsetXDir : fTblOffsetY = typPlateInfo.dblTableOffsetYDir

        ' ﾌﾞﾛｯｸｻｲｽﾞの取得
        Call CalcBlockSize(blockx, blocky)
        ' ﾁｯﾌﾟｻｲｽﾞX,Y
        dblCSx = typPlateInfo.dblChipSizeXDir
        dblCSy = typPlateInfo.dblChipSizeYDir
        ' ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙX,Y
        gspacex = typPlateInfo.dblBpGrpItv
        gspacey = typPlateInfo.dblStgGrpItvY
        'gspacex = typPlateInfo.dblGroupItvXDir
        'gspacey = typPlateInfo.dblGroupItvYDir
        ' ｽﾃｯﾌﾟｵﾌｾｯﾄX,Y
        dblStepOffX = typPlateInfo.dblStepOffsetXDir
        dblStepOffY = typPlateInfo.dblStepOffsetYDir

        ' ﾌﾞﾛｯｸ数の取得
        BNX = typPlateInfo.intBlockCntXDir
        BNY = typPlateInfo.intBlockCntYDir

        ' グループ数X,Y
        GNx = typPlateInfo.intGroupCntInBlockXBp
        GNy = typPlateInfo.intGroupCntInBlockYStage
        ' ﾌﾞﾛｯｸ間隔X,Y
        blockIntervalx = typPlateInfo.dblBlockItvXDir
        blockIntervaly = typPlateInfo.dblBlockItvYDir

        If iCDir = 1 Then
            BlkNum = BNX
        Else
            BlkNum = BNY
        End If

        If iCDir = 1 Then
            GrpNum = GNx
        Else
            GrpNum = GNy
        End If

        MaxTy2 = BlkNum
        intCntBlockNum = intFirstCnt - 1

        For intForCnt = intFirstCnt To BlkNum - 1

            With tSetNextXY
                .intCDir = iCDir
                .intStepR = iStepRepeat
                .dblblockx = blockx
                .dblblocky = blocky
                .dblCSx = dblCSx
                .dblCSy = dblCSy
                .dblgspacex = gspacex
                .dblgspacey = gspacey
                .dblStepOffX = dblStepOffX
                .dblStepOffY = dblStepOffY
                .intpxy1 = 0
                .intpxy2 = 0
                If iCDir = 1 Then
                    .intxy1 = 1
                    .intxy2 = 0
                Else
                    .intxy1 = 0
                    .intxy2 = 1
                End If

                .dblStrp = 0
                If GrpNum > 1 Then

                    If typStepInfoArray(1).intSP2 > 0 Then

                        If typStepInfoArray(intGrpCnt).intSP2 - 1 = intCntBlockNum Then

                            If intGrpCnt = 1 Then
                                .dblStrp = typStepInfoArray(intGrpCnt).dblSP3
                                If iCDir = 1 Then
                                    .intpxy1 = typStepInfoArray(intGrpCnt).intSP1
                                Else
                                    .intpxy2 = typStepInfoArray(intGrpCnt).intSP1
                                End If
                            Else

                                If iCDir = 1 Then
                                    .dblStrp = typStepInfoArray(intGrpCnt).dblSP3
                                    .intpxy1 = typStepInfoArray(intGrpCnt).intSP1
                                Else
                                    .dblStrp = typStepInfoArray(intGrpCnt).dblSP3
                                    .intpxy2 = typStepInfoArray(intGrpCnt).intSP1
                                End If
                            End If

                            intGrpCnt = intGrpCnt + 1

                            intCntBlockNum = -1

                        End If
                    End If

                End If

                .dblblockIntervalx = blockIntervalx
                .dblblockIntervaly = blockIntervaly

            End With

            ' 次ﾌﾞﾛｯｸ移動位置(X,Y)を取得する。
            Call GetNextBlockXYdata(tSetNextXY, dblX, dblY)
            typTy2InfoArray(intForCnt).intTy21 = intForCnt  ' ブロック番号 
            If iCDir = 0 Then                               ' ステップ距離 
                typTy2InfoArray(intForCnt).dblTy22 = dblY.ToString("0.0000")
            Else
                typTy2InfoArray(intForCnt).dblTy22 = dblX.ToString("0.0000")
            End If

            If blnUpdate Then
                ' 下記は検討
                '' ''If frmDataEdit.FGridTy2.Rows > MaxTy2 And frmDataEdit.FGridTy2.Cols > 2 Then
                '' ''    ' X座標変更時
                '' ''    frmDataEdit.FGridTy2.set_TextMatrix(intForCnt, 1, typTy2InfoArray(intForCnt).dblTy22.ToString("0.0000"))
                '' ''End If
            End If
            intCntBlockNum = intCntBlockNum + 1
        Next

        typTy2InfoArray(BlkNum).dblTy22 = "0.0000"
    End Function

#End Region

#Region "次のﾌﾞﾛｯｸ位置X,Yの値算出"
    '''=========================================================================
    '''<summary>次のﾌﾞﾛｯｸ位置X,Yの値算出</summary>
    '''<param name="tSetNextXY"></param>
    '''<param name="X"></param>
    '''<param name="y"></param>
    '''<remarks>ﾄﾘﾐﾝｸﾞ処理中のXYの算出処理を行なう</remarks>
    '''=========================================================================
    Private Sub GetNextBlockXYdata(ByRef tSetNextXY As TRIM_GETNEXTXY, ByRef X As Double, ByRef y As Double)

        On Error GoTo ErrExit

        With tSetNextXY
            ' 次のブロックへXY移動
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x←, y↓
                    ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then              ' X方向
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp

                    Else                                ' Y方向
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If

                    End If

                Case 1 ' x→, y↓

                    ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then              ' X方向
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp
                    Else                                ' Y方向
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If

                    End If

                Case 2 ' x←, y↑
                    ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then              ' X方向
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp
                    Else                                ' Y方向
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If

                    End If

                Case 3 ' x→, y↑
                    'ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then              ' X方向
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            X = ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If
                        y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + .dblStrp
                    Else                                ' Y方向
                        X = ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + .dblStrp
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If

                    End If

            End Select

        End With

        Exit Sub

        '''''   2009/07/02 minato
        ''''        下記はNETの処理。
        ''''        NETとCHIPで処理が異なるため、後ほどコードをマージ
        ''''    With tSetNextXY
        ''''
        ''''        ' 次のブロックへXY移動
        ''''        Select Case giBpDirXy
        ''''
        ''''            Case 0:     ' x←, y↓
        ''''
        ''''                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
        ''''                    If (0 = .intCDir) Then         ' X方向
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (3 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        End If
        ''''                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
        ''''                    Else  'Y方向
        ''''                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblStrp + .dblADDSZX
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (4 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        End If
        ''''
        ''''                    End If
        ''''            Case 1:     ' x→, y↓
        ''''
        ''''                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
        ''''                    If (0 = .intCDir) Then         ' X方向
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (3 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        End If
        ''''                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
        ''''                    Else  'Y方向
        ''''                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (4 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
        ''''                        End If
        ''''                    End If
        ''''
        ''''            Case 2:     ' x←, y↑
        ''''                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
        ''''                    If (0 = .intCDir) Then         ' X方向
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (3 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
        ''''                        End If
        ''''
        ''''                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
        ''''                    Else  'Y方向
        ''''                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblADDSZX
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (4 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        End If
        ''''                    End If
        ''''
        ''''            Case 3:     ' x→, y↑
        ''''
        ''''                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
        ''''                    If (0 = .intCDir) Then         ' X方向
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (3 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        Else
        ''''                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
        ''''                        End If
        ''''                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
        ''''                    Else  'Y方向
        ''''                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
        ''''
        ''''                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
        ''''                        If (4 = .intStepR) Then         ' 1抵抗ずらし
        ''''                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        Else
        ''''                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
        ''''                        End If
        ''''
        ''''                    End If
        ''''
        ''''        End Select
        ''''
        ''''    End With
        ''''    Exit Sub

ErrExit:
        MsgBox("GetNextBlockXYdata" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub

#End Region

#Region "ﾊﾟﾄﾗｲﾄ制御(進仕様)"
    '''=========================================================================
    '''<summary>ﾊﾟﾄﾗｲﾄ制御(進仕様)</summary>
    '''<param name="MODE">(INP) True=ON, False=OFF</param>
    '''=========================================================================
    Public Sub PATO_IO_OUT(ByRef MODE As Boolean)

        Dim strDATA As String

        ' 進仕様
        If gSysPrm.stCTM.giSPECIAL = customSUSUMU Then
            If MODE = True Then
                strDATA = "&H1"                         ' ON
            Else
                strDATA = "&H0"                         ' OFF
            End If

#If NOTINTIME Then
#Else
#If cOFFLINEcDEBUG = 1 Then
#Else
            Call OUT16(Val("&H21f8"), Val(strDATA))
#End If
#End If
        End If
    End Sub

#End Region

#Region "GP-IB初期化ｺﾏﾝﾄﾞ作成処理"
    '''=========================================================================
    '''<summary>GP-IB初期化ｺﾏﾝﾄﾞ作成処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub MakeGPIBinit()

        Dim strIniData As String

        With typPlateInfo
            strIniData = ""

            ' 測定速度の設定
            If .intGpibMeasSpeed = 0 Then                       ' 設定速度=低速
                strIniData = strIniData & "W0"
            Else ' 設定速度=高速
                strIniData = strIniData & "W1"
            End If

            ' 測定モードで切り替え
            If .intGpibMeasMode = 0 Then
                strIniData = strIniData & "FR"                  ' 測定モード=絶対
                strIniData = strIniData & "LL00000" & "LH15000" ' 下限/上限リミットの設定
            Else
                strIniData = strIniData & "FD"                  ' 測定モード=偏差
                strIniData = strIniData & "DL-5000" & "DH+5000" ' 下限/上限リミットの設定
            End If

            .strGpibInitCmnd1 = strIniData
            .strGpibInitCmnd2 = ""

        End With

    End Sub

#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090605 minato
    ''''    各Private関数から共通関数として定義。
    ''''    OcxUtilityへ実装するかは今後検討
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

#Region "四捨五入を行う"
    '''=========================================================================
    '''<summary>四捨五入を行う</summary>
    '''<param name="dblSrc">元ﾃﾞｰﾀ</param>
    '''<returns>処理ﾃﾞｰﾀ</returns>
    '''=========================================================================
    Public Function RoundOff(ByRef dblSrc As Double) As Double
        Dim str_Renamed As String

        str_Renamed = dblSrc.ToString("0.0000")
        RoundOff = CDbl(str_Renamed)

    End Function
#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090605 minato
    ''''    各Private関数から共通関数として定義。
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    '==========================================================
#Region "基準座標の中央にﾃｰﾌﾞﾙ移動"
    '''=========================================================================
    '''<summary>基準座標の中央にﾃｰﾌﾞﾙ移動</summary>
    '''<param name="dblStd1X">(INP)基準座標1のX</param>
    '''<param name="dblStd1Y">(INP)基準座標1のY</param>
    '''<param name="dblStd2X">(INP)基準座標2のX</param>
    '''<param name="dblStd2Y">(INP)基準座標2のY</param>  
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function XYTableMoveCenter(ByRef dblStd1X As Double, ByRef dblStd1Y As Double, ByRef dblStd2X As Double, ByRef dblStd2Y As Double) As Short

        Dim r As Integer
        Dim dblX As Double
        Dim dblY As Double
        Dim dblRotX As Double
        Dim dblRotY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim dblBPX As Double
        Dim dblBPY As Double
        Dim intCDir As Short
        Dim intTableType As Short
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim dblTOffsX As Double
        Dim dblTOffsY As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim strMSG As String

        Try

            r = cFRS_NORMAL
            dblRotX = 0
            dblRotY = 0

            ' データ取得
            ''''(2010/11/16) 動作確認後下記コメントは削除
            'dblTrimPosX = gStartX                           ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
            'dblTrimPosY = gStartY
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                 ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
            dblTrimPosY = gSysPrm.stDEV.gfTrimY
            dblTOffsX = typPlateInfo.dblCaribTableOffsetXDir    ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄ
            dblTOffsY = typPlateInfo.dblCaribTableOffsetYDir
            Call CalcBlockSize(dblBSX, dblBSY)                  ' ﾌﾞﾛｯｸｻｲｽﾞ算出
            intTableType = gSysPrm.stDEV.giXYtbl
            intCDir = typPlateInfo.intResistDir                 ' チップ並び方向取得(CHIP-NETのみ)

            ' θ補正ﾄﾘﾑｵﾌｾｯﾄX,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            If ((dblStd2X >= dblStd1X) And (dblStd2Y >= dblStd1Y)) Then

                dblBPX = dblBSX / 2
                dblBPY = dblBSY / 2
                ' giBpDirXy 座標系の設定(ｼｽﾃﾑ設定)
                ' 0:XY NOM(右上)  1:X REV(左上)  2:Y REV(右下)  3:XY REV(左下)
                ' ﾄﾘﾐﾝｸﾞ位置座標 + ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄ + 補正位置 (+or-) 回転半径 (+or-) ﾃｰﾌﾞﾙ補正量
                Select Case gSysPrm.stDEV.giBpDirXy
                    Case 0 ' x←, y↓
                        dblX = dblTrimPosX + dblTOffsX + Del_x + dblRotX + dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y + dblRotY + dblBPY

                    Case 1 ' x→, y↓
                        dblX = dblTrimPosX + dblTOffsX + Del_x - dblRotX - dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y + dblRotY + dblBPY

                    Case 2 ' x←, y↑
                        dblX = dblTrimPosX + dblTOffsX + Del_x + dblRotX + dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y - dblRotY - dblBPY

                    Case 3 ' x→, y↑
                        dblX = dblTrimPosX + dblTOffsX + Del_x - dblRotX - dblBPX
                        dblY = dblTrimPosY + dblTOffsY + Del_y - dblRotY - dblBPY
                End Select

                ' テーブル絶対値移動
                r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)

            End If
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "gwModule.XYTableMoveCenter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '''''=========================================================================
    '''''【機　能】先頭ﾌﾞﾛｯｸの中央へ移動
    '''''【引　数】---
    '''''【戻り値】(True)OK (False)NG
    '''''【注　記】
    '''''=========================================================================
    ''''Public Function XYTableMoveTopBlock(x_tblOff As Double, y_tblOff As Double) As Boolean
    ''''
    ''''    Dim bRetc As Boolean        '戻り値
    ''''    Dim dblTrimPosX As Double   'ﾄﾘﾑﾎﾟｼﾞｼｮﾝX
    ''''    Dim dblTrimPosY As Double   'ﾄﾘﾑﾎﾟｼﾞｼｮﾝY
    ''''    Dim dblBSX As Double        'ﾌﾞﾛｯｸｻｲｽﾞX
    ''''    Dim dblBSY As Double        'ﾌﾞﾛｯｸｻｲｽﾞY
    ''''    Dim dblBsoX As Double       'ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄX
    ''''    Dim dblBsoY As Double       'ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄY
    ''''    Dim Del_x As Double         'θ補正量X
    ''''    Dim Del_y As Double         'θ補正量Y
    ''''    Dim dblRotX As Double       '回転半径X
    ''''    Dim dblRotY As Double       '回転半径Y
    ''''    Dim dblX As Double          '移動座標X
    ''''    Dim dblY As Double          '移動座標Y
    ''''
    ''''    bRetc = False
    ''''    dblRotX = 0
    ''''    dblRotY = 0
    ''   ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
    ''       dblTrimPosX = gStartX
    ''       dblTrimPosY = gStartY
    ''''''    Call CalcBlockSize(dblBSX, dblBSY)              'ﾌﾞﾛｯｸｻｲｽﾞ算出
    '''''
    ''''    'ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ算出(ﾌﾞﾛｯｸｻｲｽﾞ/2 ﾌﾞﾛｯｸの象限はXYともに1 ﾃｰﾌﾞﾙの象限も1)
    ''''    dblBsoX = (dblBSX / 2#) * 1 * 1                 'Table.BDirX * Table.dir
    ''''    dblBsoY = (dblBSY / 2) * 1                      'Table.BDirY;
    '' '' θ補正ﾄﾘﾑｵﾌｾｯﾄX,Y
    ' ''    Del_x = gfCorrectTrimPosX
    ' ''    Del_y = gfCorrectTrimPosY
    ''''    'giBpDirXy 座標系の設定(ｼｽﾃﾑ設定)
    ''''    '0:XY NOM(右上)  1:X REV(左上)  2:Y REV(右下)  3:XY REV(左下)
    ''''    'ﾄﾘﾐﾝｸﾞ位置座標 (+or-) 回転半径 + ﾃｰﾌﾞﾙｵﾌｾｯﾄ (+or-) ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ + ﾃｰﾌﾞﾙ補正量
    ''''    Select Case giBpDirXy
    ''''        Case 0          ' x←, y↓
    ''''            dblX = dblTrimPosX + dblRotX + x_tblOff + dblBsoX + Del_x
    ''''            dblY = dblTrimPosY + dblRotY + y_tblOff + dblBsoY + Del_y
    ''''        Case 1          ' x→, y↓
    ''''            dblX = dblTrimPosX - dblRotX + x_tblOff - dblBsoX + Del_x
    ''''            dblY = dblTrimPosY + dblRotY + y_tblOff + dblBsoY + Del_y
    ''''        Case 2          ' x←, y↑
    ''''            dblX = dblTrimPosX + dblRotX + x_tblOff + dblBsoX + Del_x
    ''''            dblY = dblTrimPosY - dblRotY + y_tblOff - dblBsoY + Del_y
    ''''        Case 3          ' x→, y↑
    ''''            dblX = dblTrimPosX - dblRotX + x_tblOff - dblBsoX + Del_x
    ''''            dblY = dblTrimPosY - dblRotY + y_tblOff - dblBsoY + Del_y
    ''''    End Select
    ''''
    ''''    KJPosX = dblX
    ''''    KJPosY = dblY
    ''''    Call XYtableMove(dblX, dblY)
    ''''    bRetc = True
    ''''    XYTableMoveTopBlock = bRetc
    ''''End Function

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090608 minato
    ''''    モジュール関数へ変更するために移動
    ''''    GlbChip.basより
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

#Region "文字列編集"
    '''=========================================================================
    '''<summary>文字列編集</summary>
    '''<param name="sD1">(INP)文字列1</param>
    '''<param name="sD2">(INP)文字列2</param>
    '''<param name="sD3">(INP)文字列3</param>
    '''<param name="sD4">(INP)文字列4</param>  
    '''<returns>編集後の文字列</returns>
    '''=========================================================================
    Public Function StrMSGReplace(ByRef sD1 As String, ByRef sD2 As Object, ByRef sD3 As Object, ByRef sD4 As String) As String

        Dim sBuff As String

        'Select Case gSysPrm.stTMN.giMsgTyp
        '    Case 0                                      ' Japanese
        '        sBuff = sD1 & sD2 & " ～ " & sD3 & sD4
        '    Case Else                                   ' US/EU
        '        sBuff = sD1 & sD2 & " and " & sD3 & sD4
        'End Select
        sBuff = sD1 & sD2 & gwModule_001 & sD3 & sD4
        StrMSGReplace = sBuff

    End Function
#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090415 minato
    ''''    モジュール関数へ変更するために移動
    ''''    tky.basより
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/



    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090417 minato
    ''''    basTheta.basより移動。basThetaは削除する
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

    '-------------------------------------------------------------------------------
    '   θ軸の補正を行う
    '-------------------------------------------------------------------------------
    Public Function CorrectTheta() As Short

        '----------- Video.OCXを使用するように修正

        '        Dim reviseMode As Short
        '        Dim manualReviseType As Short
        '        Dim r As Short

        '        reviseMode = typPlateInfo.intReviseMode ' 補正モード
        '        manualReviseType = typPlateInfo.intManualReviseType ' 手動補正タイプ
        '        gbRotCorrectCancel = 0

        '        Dim mCorrect As System.Windows.Forms.Form
        '        If reviseMode = 1 And manualReviseType = 0 Then

        '        Else
        '            ' ADVボタン押下待ち、Z,BP,XY 初期位置移動
        '            '        gMode = 7
        '            '        frmReset.Show vbModal
        '            r = Form1.System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, LoadDTFlag)
        '            If (r <= cFRS_ERR_EMG) Then
        '                '強制終了
        '                Call Form1.AppEndDataSave()
        '                Call Form1.AplicationForcedEnding()
        '            ElseIf (r = cFRS_ERR_RST) Then
        '                ' リセット　or ソフト上の例外エラー発生
        '                CorrectTheta = r
        '                Exit Function
        '            End If

        '#If VIDEO_CAPTURE_OVERLAY Then
        '			If gVideoStarted Then
        '			' オーバーレイOFF
        '			Call Trim_OverLayOff
        '			Form1.Refresh
        '			End If
        '#End If

        '            mCorrect = New CorrectPos
        '            If gbRotCorrectCancel = 0 Then
        '                mCorrect.ShowDialog()
        '            End If
        '            'UPGRADE_NOTE: オブジェクト mCorrect をガベージ コレクトするまでこのオブジェクトを破棄することはできません。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"' をクリックしてください。
        '            mCorrect = Nothing

        '            Form1.Refresh()

        '#If VIDEO_CAPTURE_OVERLAY Then
        '			If gVideoStarted Then
        '			Call Trim_OverLayOn
        '			End If
        '#End If
        '        End If

        '        If gbRotCorrectCancel = 0 Then
        '            ' 正常終了
        '        Else
        '            ' パターンマッチNG or ユーザーキャンセル
        '        End If

    End Function

#Region "BPをﾌﾞﾛｯｸの右上に移動する"
    '''=========================================================================
    '''<summary>BPをﾌﾞﾛｯｸの右上に移動する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub BpMoveOrigin_Ex()

        Dim dblBpOffsX As Double
        Dim dblBpOffsY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double

        ' ﾌﾞﾛｯｸｻｲｽﾞ取得
        Call CalcBlockSize(dblBSX, dblBSY)
        ' BP位置ｵﾌｾｯﾄX,Y設定
        dblBpOffsX = typPlateInfo.dblBpOffSetXDir
        dblBpOffsY = typPlateInfo.dblBpOffSetYDir
        ' BPをﾌﾞﾛｯｸの右上に移動する(BSIZE()後BPOFF()実行)
        Call Form1.System1.BpMoveOrigin(gSysPrm, dblBSX, dblBSY, dblBpOffsX, dblBpOffsY)

    End Sub
#End Region

    '----- 以下、未使用のため削除

    '#Region "HaltSwで一時停止後のコンソール入力取得処理"
    '    '==========================================================
    '    '(2011/06/13)
    '    '   frmPtnCalibrationなど様々所で同じ処理がある。
    '    '   メッセージ表示部分に共通性を持たせ、共通のモジュールとして
    '    '   使用できるように変更後、gwModuleへ移動させる
    '    '
    '    '   FUNC    :   HaltSw 押下時のコンソール入力待ち
    '    '   PARAM   :
    '    '   RET     :   
    '    '               cSTS_STARTSW_ON As Integer = 1  ' STARTスイッチON
    '    '               cSTS_RESETSW_ON As Integer = 3  ' RESETスイッチON
    '    '   COMMENT :
    '    '==========================================================
    '    Public Function HaltSwMoveWait(ByVal objCtrl As Object, ByVal lblMsg As String) As Integer

    '        Dim sts As Integer
    '        Dim ret As Integer
    '        HaltSwMoveWait = 0

    '        Try
    '            'If Form1.System1.AdjReqSw() Then
    '            ret = HALT_SWCHECK(sts)
    '            If sts = cSTS_HALTSW_ON Then
    '                objCtrl.Text = lblMsg
    '                'Me.lblInfo.Text = lblMsg
    '                sts = cSTS_STARTSW_ON
    '#If cOFFLINEcDEBUG = 1 Then
    '                If (MsgBoxResult.Ok = MsgBox("OK=START CANCEL=RESET", MsgBoxStyle.OkCancel)) Then
    '                    sts = cSTS_STARTSW_ON
    '                Else
    '                    sts = cSTS_RESETSW_ON 
    '                End If
    '#Else
    'RETRY_SWWAIT:
    '                ret = STARTRESET_SWWAIT(sts)
    '                If (sts = cSTS_STARTSW_ON) Then
    '                    'そのまま実行
    '                ElseIf (sts = cSTS_RESETSW_ON) Then
    '                    Call LAMP_CTRL(LAMP_RESET, True)
    '                    If Form1.System1.TrmMsgBox(gSysPrm, "OK:[START] CANCEL:[HALT]", _
    '                            MsgBoxStyle.OkCancel, FRM_CALIBRATION_TITLE) Then
    '                        sts = cSTS_NOSW_ON
    '                    Else
    '                        '再度待ち状態に入る。
    '                        GoTo RETRY_SWWAIT
    '                    End If
    '                    Call LAMP_CTRL(LAMP_RESET, False)
    '                End If
    '#End If
    '                'Call Form1.System1.sLampOnOff(LAMP_HALT, False)
    '                Call LAMP_CTRL(LAMP_HALT, False)
    '            End If

    '            HaltSwMoveWait = sts

    '        Catch ex As Exception
    '            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
    '        End Try
    '    End Function
    '#End Region


#Region "ターン、切替ポイントのパラメータをINtime側に送信する"
    '''=========================================================================
    '''<summary>ターン、切替ポイントパラメータをINtime側に送信する</summary>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function SendTrimDataCutPoint() As Short

        Dim r As Integer
        Dim strMSG As String
        Dim strName As String
        Dim strDirName As String
        Dim CutPointFilePath As String

        Try

            If giChangePoint = 0 Then
                Return cFRS_NORMAL
            End If

            strName = ""
            CutPointTable.Initialize()
            'データファイルの内容を読み込んで、構造体テーブルにセット
            strName = System.IO.Path.GetFileNameWithoutExtension(gStrTrimFileName)
            strDirName = System.IO.Path.GetDirectoryName(gStrTrimFileName)
            CutPointFilePath = strDirName + "\" + strName + ".cut"
            ReadCutDataFile(CutPointFilePath, CutPointTable)

            ' パラメータをINtime側に送信する
            r = LaserFront.Trimmer.DefTrimFnc.CUTPOINTPARAM(CutPointTable)
            If (r <> cFRS_NORMAL) Then                                          ' 送信失敗 ?
                Call Form1.System1.TrmMsgBox(gSysPrm, "SendTrimDataCutPoint Paratemer Send Error", MsgBoxStyle.OkOnly, TITLE_4)
                SendTrimDataCutPoint = 1
            End If

            Exit Function


            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.SendTrimDataCutPoint() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            SendTrimDataCutPoint = cERR_TRAP
        End Try

    End Function

    ''' <summary>
    ''' 指定のファイルから切替ポイント、ターンポイントを読み込む
    ''' </summary>
    ''' <param name="CutPointFilePath"></param>
    ''' <param name="CutPointTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadCutDataFile(ByVal CutPointFilePath As String, ByRef CutPointTable As LaserFront.Trimmer.DefTrimFnc.CUTPOINTCALC) As Integer
        Dim intFileNo As Integer
        Dim strDAT As String
        Dim loopcnt As Integer
        Dim stArraData As String()
        Dim strMSG As String
        Dim tmpStr As String


        Try
            loopcnt = 0
            ' テキストファイルをオープンする
            intFileNo = FreeFile()                                  ' 使用可能なファイルナンバーを取得
            FileOpen(intFileNo, CutPointFilePath, OpenMode.Input)

            ' ファイルの終端までループを繰り返します。
            Do While Not EOF(intFileNo)
                strDAT = LineInput(intFileNo)                       ' 1行読み込み
                tmpStr = strDAT.Substring(0, 1)
                If (tmpStr = ";") Then
                    Continue Do
                End If
                stArraData = strDAT.Split(",")
                CutPointTable.dblCutInitParam(loopcnt) = stArraData(1)
                CutPointTable.dblCutParam(loopcnt) = stArraData(2)
                loopcnt = loopcnt + 1
                If loopcnt >= 10 Then
                    Exit Do
                End If
            Loop
            CutPointTable.func = giChangePoint

            FileClose(intFileNo)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.ReadCutDataFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            ReadCutDataFile = cERR_TRAP
        End Try

    End Function

    ''' <summary>
    ''' ターンポイント、切替ポイントファイル名作成
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MakeutPointFileData(ByVal gStrTrimFileName As String) As String
        Dim strName As String
        Dim strDirName As String

        MakeutPointFileData = ""

        'データファイルの内容を読み込んで、構造体テーブルにセット
        strName = System.IO.Path.GetFileNameWithoutExtension(gStrTrimFileName)
        strDirName = System.IO.Path.GetDirectoryName(gStrTrimFileName)
        MakeutPointFileData = strDirName + "\" + strName + ".cut"

        Return MakeutPointFileData

    End Function

#End Region

End Module