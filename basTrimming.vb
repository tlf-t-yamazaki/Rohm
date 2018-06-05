'===============================================================================
'   Description  : トリミング処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Globalization                        'V4.0.0.0-65
Imports System.IO                                   'V4.4.0.0-0
Imports System.Reflection                           'V4.0.0.0-65
Imports System.Runtime.InteropServices
'V6.0.0.0②  Imports System.Runtime.Remoting                     'V3.0.0.0⑤
'V6.0.0.0②  Imports System.Runtime.Remoting.Channels            'V3.0.0.0⑤
'V6.0.0.0②  Imports System.Runtime.Remoting.Channels.Ipc        'V3.0.0.0⑤
Imports System.Text                                 'V4.0.0.0-65
Imports System.Threading                            'V4.0.0.0-65
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources                'V4.4.0.0-0
Imports TrimClassLibrary    'V3.0.0.0⑤      'V3.0.0.0⑤  
Imports TrimControlLibrary              '#4.12.2.0①

Module basTrimming
#Region "定数/変数定義"
    '===========================================================================
    '   定数/変数定義
    '===========================================================================
    '----- トリミング結果 -----
    Public Const TRIM_RESULT_NOTDO As Short = 0                         ' 未実施
    Public Const TRIM_RESULT_OK As Short = 1                            ' OK
    Public Const TRIM_RESULT_IT_NG As Short = 2                         ' ITNG(ワーク ※VB側へは返らない)
    Public Const TRIM_RESULT_FT_NG As Short = 3                         ' FTNG(ワーク ※VB側へは返らない)
    Public Const TRIM_RESULT_SKIP As Short = 4                          ' SKIP
    Public Const TRIM_RESULT_RATIO As Short = 5                         ' RATIO 
    Public Const TRIM_RESULT_IT_HING As Short = 6                       ' ITHI NG
    Public Const TRIM_RESULT_IT_LONG As Short = 7                       ' ITLO NG
    Public Const TRIM_RESULT_FT_HING As Short = 8                       ' FTHI NG 
    Public Const TRIM_RESULT_FT_LONG As Short = 9                       ' FTLO NG
    Public Const TRIM_RESULT_OVERRANGE As Short = 10                    ' レンジオーバ
    '                                                                   ' 4端子ｵｰﾌﾟﾝ
    Public Const TRIM_RESULT_IT_OK As Short = 12                        ' ITテストOK
    Public Const TRIM_RESULT_PATTERNNG As Short = 13                    ' カット位置補正パターン認識NGのためSKIP
    Public Const TRIM_RESULT_TRIM_OK As Short = 14                      ' TRIM OK
    Public Const TRIM_RESULT_IKEI_SKIP As Short = 15                    ' 異形面付けによりSKIP 
    '----- V1.13.0.0⑪↓ -----
    Public Const TRIM_RESULT_CVERR As Short = 16                        ' 測定ばらつき検出
    Public Const TRIM_RESULT_OVERLOAD As Short = 17                     ' オーバロード検出
    Public Const TRIM_RESULT_REPROBING As Short = 18                    ' 再プロービングばらつき検出(双信殿)
    '----- V1.13.0.0⑪↑ -----
    Public Const TRIM_RESULT_ES2ERR As Short = 19                       ' ES2 エラー V1.14.0.1①
    Public Const TRIM_RESULT_OPENCHK_NG As Short = 20                   ' オープンチェックエラー
    Public Const TRIM_RESULT_SHORTCHK_NG As Short = 21                  ' ショートチェックエラー

    'V1.20.0.1①
    Public Const TRIM_RESULT_MIDIUM_CUT_NG As Short = 22                ' 途中切り検出エラー 
    'V1.20.0.1①
    '----- NG Judge -----
    Public Const CONTINUES_NG_HI As Integer = 1                         ' 連続NG-HIGHｴﾗｰ発生
    Public Const CONTINUES_NG_LO As Integer = 2

    '----- 画像表示プログラムの起動用 -----
    'V6.0.0.0⑤    Public Const DISPGAZOU_SMALL_PATH As String = "C:\TRIM\DispGazouSmall.exe"     ' 画像表示プログラム名
    'Public Const DISPGAZOU_PATH As String = "C:\TRIM\DispGazou.exe"     ' 画像表示プログラム名 V4.3.0.0③
    'V6.0.0.0⑤    Public Const DISPGAZOU_PATH As String = "C:\TRIM\DispGazouSmall.exe" ' V4.3.0.0③
    'V6.0.0.0⑤    Public Const DISPGAZOU_WRK As String = "C:\\TRIM"                   ' 作業フォルダ

    'Private ObjGazou As Process = Nothing                              ' Processオブジェクト ###156
    'V6.0.0.0⑤    Public ObjGazou As Process = Nothing                                ' Processオブジェクト ###156

    '----- FL向けデフォルト設定ファイル ----
    Public Const DEF_FLPRM_SETFILEPATH As String = "c:\TRIM\"           'FL向けデフォルト設定ファイル
    Public Const DEF_FLPRM_SETFILENAME As String = "c:\TRIM\defaultFlParamSet.xml"    'FL向けデフォルト設定ファイル

    Private Const NEXT_BLOCK As Integer = -1                            ' ADJ処理で使用。次のステップへ移動する場合。
    Private Const PREV_BLOCK As Integer = -2                            ' ADJ処理で使用。一つ前のステップへ移動する場合。

    'Private gstrResult(20) As String                                   ' ﾄﾘﾐﾝｸﾞ結果 ###248
    Private gstrResult(MAX_RESULT_NUM) As String                        ' ﾄﾘﾐﾝｸﾞ結果 ###248

    'Public glTchBPTeachMoval(2) As Double                              ' スライダーバーの値保存領域

    Public globalLogFileName As String = "tmpLogFile.Log"                  'V1.13.0.0⑫
    '----- 定数定義 -----
    Public giMarkingMode As Short                                       ' 1:NGマーキングあり

    '----- プローブチェック機能用 -----
    Private m_PlateCounter As Integer = 0                               ' 基板枚数カウンタ V1.23.0.0⑦
    Private m_ChkCounter As Integer = 0                                 ' 基板チェックカウンタ V1.23.0.0⑦
    '----- V4.11.0.0③↓ (WALSIN殿SL436S対応) -----
    ' オートパワー実行機能用
    Private m_PwrChkCounter As Integer = 0                              ' 基板チェックカウンタ
    Private m_TimeAss As TimeSpan = New TimeSpan(0, 0, 0)               ' 指定時間(分)
    Private m_TimeSpan As TimeSpan = New TimeSpan(0, 0, 0)              ' 経過時間(分)
    Private m_TimeSTart As DateTime                                     ' 開始時間
    Private m_TimeNow As DateTime                                       ' 現在時間

    '----- AutoLaserPowerADJ()の処理モード
    Private Const MD_INI As Integer = 0                                 ' 初期モード(stPWR(加工条件番号配列)を初期化する)
    Private Const MD_ADJ As Integer = 1                                 ' 調整モード(stPWR(加工条件番号配列)を初期化しない)
    '----- V4.11.0.0③↑ -----

    '----- 基板ディレイ(ディレイトリム２)用 -----
    Public intGetCutCnt As Integer = 1                                  ' カット数(先頭ブロック～最終ブロックまでブロック移動するためのループ数)
    Public m_blnDelayCheck As Boolean = False                           ' ディレイトリム２を行う(True), 行わない(False)
    Private m_blnDelayFirstCut As Boolean                               ' 第1カットフラグ
    Private m_blnDelayLastCut As Boolean                                ' 最終カットフラグ
    Private m_intDelayBlockIndex As Short                               ' 現在のブロック番号(カット番号)

    Private m_intNgCount As Short                                       ' NG数 
    Private m_lngRetcTrim As Integer

    'Private pfPP36x As Double                                          ' ピクセルあたりのBP距離
    'Private pfPP36y As Double
    'Private piPP37_3 As Short
    'Private pfPosition(1, 512) As Double                               ' カット補正ポジションXY
    'Private piCutCorPtn(512) As Short                                  ' カット補正のパターン番号
    'Private pbCutCorDisp(512) As Boolean                               ' カット補正　パターンマッチ表示

    ' ロギング関係
    Private strLogFileNameDate As String                                ' ﾛｸﾞﾌｧｲﾙのﾌｧｲﾙ名日付部分
    'Private Const cTRIMLOGcSECTNAME As String = "TRIMMODE_FILELOG_DATA_SET"
    'Private Const cMEASLOGcSECTNAME As String = "MEASMODE_FILELOG_DATA_SET"
    'Private m_TrimlogFileFormat(20) As Integer                         ' トリミング時-ログファイルへの出力対象＆順番設定変数
    'Private m_MeaslogFileFormat(20) As Integer                         ' 測定時-ログファイルへの出力対象＆順番設定変数

    ''----- ログ対象定数 -----
    'Private Const LOGTAR_DATE As Short = 1
    'Private Const LOGTAR_LOTNO As Short = 2
    'Private Const LOGTAR_CIRCUIT As Short = 3
    'Private Const LOGTAR_RESISTOR As Short = 4
    'Private Const LOGTAR_JUDGE As Short = 5
    'Private Const LOGTAR_TARGET As Short = 6
    'Private Const LOGTAR_INITIAL As Short = 7
    'Private Const LOGTAR_FINAL As Short = 8
    'Private Const LOGTAR_DEVIATION As Short = 9
    'Private Const LOGTAR_UCUTPRMNO As Short = 10
    'Private Const LOGTAR_END As Short = 99

    '----- 生産管理情報 -----
    Public m_lCircuitNgTotal As Integer                                 ' 不良サーキット数
    Public m_lCircuitGoodTotal As Integer                               ' 良品サーキット数
    Public m_lPlateCount As Integer                                     ' プレート処理数
    Public m_lGoodCount As Integer                                      ' 良品抵抗数
    Public m_lNgCount As Integer                                        ' 不良抵抗数
    Public m_lITHINGCount As Integer                                    ' IT HI NG数
    Public m_lITLONGCount As Integer                                    ' IT LO NG数
    Public m_lFTHINGCount As Integer                                    ' FT HI NG数
    Public m_lFTLONGCount As Integer                                    ' FT LO NG数
    Public m_lITOVERCount As Integer                                    ' ITｵｰﾊﾞｰﾚﾝｼﾞ数

    ' NG判定(0-100%)用NG抵抗数(ブロック単位/プレート単位) ###142 
    Public m_NG_RES_Count As Integer                                    ' NG抵抗数

    Public m_NgCountInPlate As Integer                                    ' NG抵抗数カウント用 'V4.5.0.5① 'V6.0.5.0④
    Public TotalAverageFT As Double                                       ' ###154
    Public TotalDeviationFT As Double                                     ' ###154
    Public TotalAverageIT As Double                                       ' ###154
    Public TotalDeviationIT As Double                                     ' ###154
    Public TotalAverageDebug As Double                                    ' ###154
    Public TotalDeviationDebug As Double                                  ' ###154
    Public TotalSum2FT As Double                                          ' ###154
    Public TotalSum2IT As Double                                          ' ###154

    '----- V6.0.3.0_26↓ -----
    Public TotalAverageFTValue As Double
    Public TotalFTValue As Double
    Public TotalCntTrimming As Long
    '----- V6.0.3.0_26↑ -----

    'UPGRADE_WARNING: 配列 gwCircuitNgCount の下限が 1 から 0 に変更されました。
    Private gwCircuitNgCount(48) As Short                               '  
    Private gfCircuitX(48) As Double                                    ' サーキット座標 X 
    Private gfCircuitY(48) As Double                                    ' サーキット座標 Y 

    '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) -----
    ' 一時停止時間集計用構造体
    Public Structure PauseTime_Data_Info
        Dim StartTime As DateTime                                       ' 一時停止開始時間(YYYY/MM/DD MM:DD:SS)
        Dim EndTime As DateTime                                         ' 一時停止終了時間
        Dim PauseTime As TimeSpan                                       ' 一時停止時間
        Dim TotalTime As TimeSpan                                       ' 一時停止トータル時間
    End Structure
    Public StPauseTime As PauseTime_Data_Info
    Public m_blnElapsedTime As Boolean = False                          ' 経過時間を表示する(True), しない(False)
    '----- V4.11.0.0④↑ -----

    '----- V1.22.0.0④↓ -----
    '----- サマリーロギング用(シナジー殿対応) -----
    ' サマリーロギングの抵抗詳細項目形式定義
    Public Structure SummaryLog_SubData
        Dim lngItLow As Long                                            ' IT LO NG抵抗数
        Dim lngItHigh As Long                                           ' IT HI NG抵抗数
        Dim lngItTotal As Long                                          ' IT NG抵抗数 =  
        Dim lngFtLow As Long                                            ' FT LO NG抵抗数
        Dim lngFtHight As Long                                          ' FT HI NG抵抗数
        Dim lngFtTotal As Long                                          ' IT NG抵抗数 
        Dim lngOpen As Long                                             ' オープンチェックエラー抵抗数
    End Structure

    ' サマリーロギング情報形式定義
    Public Structure SummaryLog
        Dim strStartTime As String                                      ' サマリー開始時間
        Dim strEndTime As String                                        ' サマリー終了時間
        Dim stTotalSubData As SummaryLog_SubData                        ' サマリーロギングの詳細項目(トータル)
        <VBFixedArray(1000)> Dim RegAry() As SummaryLog_SubData         ' サマリーロギングの詳細項目(抵抗毎1-999)

        ' 構造体の初期化
        Public Sub Initialize()
            ReDim RegAry(1000)
        End Sub
    End Structure
    Public stSummaryLog As SummaryLog                                   ' サマリーロギング出力データ
    '----- V1.22.0.0④↑ -----

    '----- V6.1.1.0③↓ (SL436R用オプション)-----
    ' 自動運転開始/終了時間表示用
    Public cStartTime As DateTime
    Public cEndTime As DateTime
    '----- V6.1.1.0③↑ -----

    '----- その他 -----
    Private bIniFlg As Integer = 0                                      ' 初期フラグ(0=初回, 1= トリミング中, 2=終了) ###156
    Private bIniPwrFlg As Boolean = False                               ' V4.11.0.0③
    Public IDReadName As String                                         ' ID読み込み結果        'V1.13.0.0⑫
    Public LogFileSaveName As String                                    ' Logファイル名         'V1.13.0.0⑫

    Public Const WM_COPYDATA As Int32 = &H4A
    Public Const WM_USER As Int32 = &H400
    Public Const WM_APP As Int32 = &H8000

    'COPYDATASTRUCT構造体 
    Public Structure COPYDATASTRUCT
        Public dwData As Int32   '送信する32ビット値
        Public cbData As Int32        'lpDataのバイト数
        Public lpData As String     '送信するデータへのポインタ(0も可能)
    End Structure

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function FindWindow( _
         ByVal lpClassName As String, _
         ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function SendMessage( _
                           ByVal hWnd As IntPtr, _
                           ByVal wMsg As Int32, _
                           ByVal wParam As Int32, _
                           ByVal lParam As Int32) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Function SendNotifyMessage( _
                           ByVal hWnd As IntPtr, _
                           ByVal wMsg As Int32, _
                           ByVal wParam As Int32, _
                           ByVal lParam As Int32) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Public Function SendMessage(
                            ByVal hWnd As IntPtr,
                            ByVal wMsg As Int32,
                            ByVal wParam As Int32,
                            ByRef lParam As COPYDATASTRUCT) As Integer
    End Function
    '#4.12.2.0④            ↓
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="SendMessage")>
    Public Function SendMessageString(ByVal hWnd As IntPtr,
                                      ByVal wMsg As UInt32,
                                      ByVal wParam As Int32,
                                      <[In], MarshalAs(UnmanagedType.LPWStr)>
                                      lParam As String) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode, EntryPoint:="SendMessage")>
    Public Function SendMessageStringBuilder(ByVal hWnd As IntPtr,
                                             ByVal wMsg As UInt32,
                                             ByVal wParam As Int32,
                                             <[In], MarshalAs(UnmanagedType.LPWStr)>
                                             ByVal lParam As StringBuilder) As Integer
    End Function
    '#4.12.2.0④            ↑
#End Region

#Region "トリミング結果保存領域初期化"
    Public Sub ClearTrimResult()
        m_lPlateCount = 0                                               ' プレート処理数
        m_lGoodCount = 0                                                ' 良品抵抗数
        m_lNgCount = 0                                                  ' 不良抵抗数
        m_lCircuitNgTotal = 0                                           ' 不良サーキット数
        m_lCircuitGoodTotal = 0                                         ' 良品サーキット数

        m_lITHINGCount = 0
        m_lITLONGCount = 0
        m_lFTHINGCount = 0
        m_lFTLONGCount = 0
        m_lITOVERCount = 0
    End Sub
#End Region

#Region "自動レーザパワー調整処理"
    '''=========================================================================
    '''<summary>自動レーザパワー調整処理</summary>
    '''<param name="Mode">(INP)処理モード V4.11.0.0③
    '''                        MD_INI = 初期モード(stPWR(加工条件番号配列)を初期化する)
    '''                        MD_ADJ = 調整モード(stPWR(加工条件番号配列)を初期化しない)
    '''                      </param>
    '''<remarks>自動レーザパワーの調整処理実行。</remarks>
    '''<returns>cFRS_NORMAL  = 正常
    '''         cFRS_ERR_RST = Cancel(RESETｷｰ)
    '''         上記以外 　　= 非常停止検出等のエラー</returns> 
    '''=========================================================================
    Public Function AutoLaserPowerADJ(ByVal Mode As Integer) As Short
        'Public Function AutoLaserPowerADJ() As Short

        Dim strMsg As String
        Dim r As Integer
        Dim iCurr As Long
        Dim iCurrOfs As Long
        Dim dMeasPower As Double
        Dim dFullPower As Double
        Dim AdjustTarget As Double                                      ' ###066
        Dim AdjustLevel As Double                                       ' ###066
        Dim CndNum As Integer                                           ' ###066
        Dim bIniFlg As Boolean = True                                   ' V4.0.0.0-87

        Try
            With typPlateInfo
                stPRT_ROHM.LaserPower = ""                              ' V1.18.0.0③
                strMsg = "" 'V4.0.0.0-72
                ' パワーメータのデータ取得タイプが「Ｉ／Ｏ読取り」/「ＵＳＢ」でなければNOP(そのまま抜ける)
                If (gSysPrm.stIOC.giPM_DataTp = PM_DTTYPE_NONE) Then
                    Return (cFRS_NORMAL)
                End If

                '----- V1.18.0.4①↓ -----
                ' Power Ctrl ON/OFFボタンを表示する(オプション)なら「Power Ctrl ON/OFF」ボタンを優先する(ローム殿特注)
                If (giBtnPwrCtrl = 1) Then                              '「Power Ctrl ON/OFF」ボタン有効 ?
                    If (Form1.BtnPowerOnOff.Text = "Power Ctrl ON") Then
                        GoTo STP_EXEC                                   ' 「Power Ctrl ON」ならパワー調整を実行する
                    Else
                        GoTo STP_NO_EXEC                                ' 「Power Ctrl OFF」なら パワー調整を実行しない
                    End If
                    'Else                                               'V1.18.0.2①
                    '    GoTo STP_NO_EXEC                               'V1.18.0.2①
                End If
                '----- V1.18.0.4①↑ -----

                ' パワー調整を実行しない場合はそのまま抜ける
                If (.intPowerAdjustMode <> 1) Then                      ' パワー調整実行フラグ = 実行しない ?
                    '----- V1.18.0.0③↓ -----
STP_NO_EXEC:        ' V1.18.0.4①
                    ' 印刷用加工面出力パワー設定(ローム殿特注)
                    If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                        If (stPRT_ROHM.LaserPower = "") Then
                            stPRT_ROHM.LaserPower = dMeasPower.ToString("-.--") + "W"
                        End If
                    End If
                    '----- V1.18.0.0③↑ -----
                    ' パワー調整を実行しない場合はそのまま抜ける
                    Return (cFRS_NORMAL)
                End If

STP_EXEC:       ' V1.18.0.4①
                ' Zを原点へ移動
                r = EX_ZMOVE(0, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                    Return (r)                                          ' Return値設定 
                End If

                '---------------------------------------------------------------
                '   自動パワー調整実行
                '---------------------------------------------------------------
                '----- V1.18.0.1⑦↓ -----
                ' レーザ照射中のシグナルタワー(黄色)点灯(ローム殿特注)
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ローム殿特注 ? 
                    Call EXTOUT1(EXTOUT_EX_YLW_ON, 0)
                End If
                '----- V1.18.0.1⑦↑ -----

                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                    '-----------------------------------------------------------
                    '   FL時 ###066
                    '-----------------------------------------------------------
                    'r = Form1.System1.Form_FLAutoLaser(gSysPrm, .intPowerAdjustCondNo, .dblPowerAdjustTarget, .dblPowerAdjustToleLevel, _
                    '                                   iCurr, iCurrOfs, dMeasPower, dFullPower)
                    '----- V4.11.0.0③↓ (WALSIN殿SL436S対応) -----
                    ' パワー調整する加工条件番号配列に有効/無効を設定する
                    'r = SetAutoPowerCndNumAry(stPWR)
                    If (Mode = MD_INI) Then                                 ' 初期モード時に設定する 
                        r = SetAutoPowerCndNumAry(stPWR)
                    End If
                    '----- V4.11.0.0③↑ -----

                    ' カットに使用する加工条件番号のパワー調整を行う 
                    For CndNum = 0 To (MAX_BANK_NUM - 1)
                        If (stPWR.CndNumAry(CndNum) = 1) Then               ' 加工条件は有効 ?
                            AdjustTarget = stPWR.AdjustTargetAry(CndNum)    ' 目標パワー値(W)
                            AdjustLevel = stPWR.AdjustLevelAry(CndNum)      ' 調整許容範囲(±W)

                            ' メッセージ表示("パワー調整開始"+ " 加工条件番号xx")
                            strMsg = MSG_AUTOPOWER_01 + " " + MSG_AUTOPOWER_02 + CndNum.ToString("00")
                            Call Form1.Z_PRINT(strMsg)

                            ' パワー調整を行う
                            r = Form1.System1.Form_FLAutoLaser(gSysPrm, CndNum, AdjustTarget, AdjustLevel, iCurr, iCurrOfs, dMeasPower, dFullPower)
                            '----- ###177↓ -----
                            If (r < cFRS_NORMAL) Then
                                ' エラーメッセージ表示
                                r = Form1.System1.Form_AxisErrMsgDisp(System.Math.Abs(r))
                                Return (r)
                            End If
                            '----- ###177↑ -----

                            ' 調整結果をメイン画面に表示する
                            If (r = cFRS_NORMAL) Then                   ' 正常終了 ? 
                                ' メッセージ表示("レーザパワー設定値"+" = xx.xxW, " + "電流値=" + "xxxmA")
                                strMsg = MSG_AUTOPOWER_03 + "= " + dMeasPower.ToString("0.00") + "W, "
                                strMsg = strMsg + MSG_AUTOPOWER_04 + "= " + iCurr.ToString("0") + "mA"
                                Call Form1.Z_PRINT(strMsg)

                                '----- V4.0.0.0-58↓ -----
                                ' オートパワー調整後の電流値を「加工条件構造体」に反映する(SL436S時)
                                If (giMachineKd = MACHINE_KD_RS) Then
                                    stCND.Curr(CndNum) = iCurr          ' トリマー加工条件構造体の電流値更新
                                End If
                                '----- V4.0.0.0-58↑ -----

                                '----- V1.18.0.0③↓ -----
                                ' 印刷用加工面出力パワー設定(ローム殿特注)
                                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                                    If (stPRT_ROHM.LaserPower = "") Or (stPRT_ROHM.LaserPower = "        ---") Then
                                        stPRT_ROHM.LaserPower = dMeasPower.ToString("0.00") + "W"
                                    End If
                                End If
                                '----- V1.18.0.0③↑ -----
                                '----- V4.0.0.0-87↓ -----
                                If (giMachineKd = MACHINE_KD_RS) And (bIniFlg = True) Then
                                    TrimData.SetLaserPower(dMeasPower)
                                End If
                                bIniFlg = False
                                '----- V4.0.0.0-87↑ -----
                            Else
                                ' メッセージ表示("パワー調整未完了")
                                strMsg = MSG_AUTOPOWER_05
                                Call Form1.Z_PRINT(strMsg)
                                '----- V4.0.0.0-87↓ -----
                                If (giMachineKd = MACHINE_KD_RS) Then
                                    TrimData.SetLaserPower(0.0)
                                End If
                                '----- V4.0.0.0-87↑ -----
                                Exit For                                ' 処理終了
                            End If
                        End If
                    Next CndNum

                    '----- V4.0.0.0-58↓ -----
                    If strMsg <> MSG_AUTOPOWER_05 Then  'V4.0.0.0-72
                        ' オートパワー調整後の電流値をトリミングデータに反映する(SL436S時)
                        If (giMachineKd = MACHINE_KD_RS) Then
                            r = SetAutoPowerCurrData(stCND)
                            gCmpTrimDataFlg = 1                            ' データ更新フラグ = 1(更新あり)
                        End If
                        '----- V4.0.0.0-58↑ -----
                    End If                              'V4.0.0.0-72

                Else
                    '-----------------------------------------------------------
                    '   FL以外の場合
                    '-----------------------------------------------------------
                    If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then                                  'V5.0.0.6⑧ RMCTRL2対応 ?
                        Call ATTRESET()                                                     'V1.25.0.0⑭
                    End If                                                                  'V5.0.0.6⑧
                    r = Form1.System1.Form_AutoLaser(gSysPrm, .dblPowerAdjustQRate,
                                        .dblPowerAdjustTarget, .dblPowerAdjustToleLevel)
                    '----- V1.16.0.0⑧↓ -----
                    ' 調整結果をメイン画面に表示する
                    If (r = cFRS_NORMAL) Then                           ' 正常終了 ? 
                        ' メッセージ表示("パワー調整正常終了")
                        strMsg = MSG_AUTOPOWER_06
                        Call Form1.Z_PRINT(strMsg)
                        'V4.4.0.0⑤
                        '---------------------------------------------------------------------------
                        '   レーザー調整後処理
                        '---------------------------------------------------------------------------
                        ' RMCTRL2 >=2 の場合、 減衰率をシスパラより再表示("減衰率 = 99.9%")  '###026
                        Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)          '###029
                        If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then
                            strMsg = LBL_ATT + "  " + CDbl(gSysPrm.stRAT.gfAttRate).ToString("##0.0") + " %"
                            Form1.LblRotAtt.Text = strMsg
                        End If
                        'V4.4.0.0⑤
                    Else
                        ' メッセージ表示("パワー調整未完了")
                        strMsg = MSG_AUTOPOWER_05
                        Call Form1.Z_PRINT(strMsg)
                    End If
                    '----- V1.16.0.0⑧↑ -----
                End If

                System.Windows.Forms.Application.DoEvents()
            End With

            '----- V1.18.0.1⑦↓ -----
            ' レーザ照射中のシグナルタワー(黄色)消灯(ローム殿特注)
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ローム殿特注 ? 
                Call EXTOUT1(0, EXTOUT_EX_YLW_ON)
            End If
            '----- V1.18.0.1⑦↑ -----

            Return (r)                                                  ' Return値設定 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "basTrimming.AutoLaserPowerADJ() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try
    End Function
#End Region
    '----- V4.11.0.0③↓ (WALSIN殿SL436S対応) -----
#Region "オートパワー調整実行(基板枚数指定/時間(分)指定)FL用(自動運転時有効)"
    '''=========================================================================
    ''' <summary>オートパワー調整実行(基板枚数指定/時間(分)指定)</summary>
    ''' <param name="digL">       (INP)Dig-SW Low</param>
    ''' <param name="ChkCounter"> (I/O)チェックカウンタ</param>
    ''' <param name="StartTime">  (I/O)開始時間</param>
    ''' <param name="NowTimeSpan">(I/O)経過時間</param>
    ''' <param name="AssTimeSpan">(INP)指定時間(分)</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>基板単位で実行する。例えばブロックの途中で指定時間が来てもその基板
    '''          が終わった後に実行する</remarks>
    '''=========================================================================
    Public Function AutoPowerExeByOption(ByVal digL As Integer, ByRef ChkCounter As Integer, ByRef StartTime As DateTime, ByRef NowTimeSpan As TimeSpan, ByRef AssTimeSpan As TimeSpan) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim rtn As Integer = cFRS_NORMAL
        Dim iCurr As Long
        Dim CndNum As Integer
        Dim dblMeasure As Double
        Dim AdjustTarget As Double
        Dim AdjustLevel As Double
        Dim dblHi As Double
        Dim dblLo As Double
        Dim bPowerADJ As Boolean = False
        Dim strMSG As String
        Dim ts As TimeSpan
        Dim Flg As Integer = 0

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 自動運転でなければNOP
            If (bFgAutoMode = False) Then Return (cFRS_NORMAL)

            ' FLでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)

            ' パワー調整実行フラグ = 実行しないならNOP
            If (typPlateInfo.intPowerAdjustMode <> 1) Then Return (cFRS_NORMAL)

            ' パワーメータのデータ取得タイプが「Ｉ／Ｏ読取り」/「ＵＳＢ」でなければNOP
            If (gSysPrm.stIOC.giPM_DataTp = PM_DTTYPE_NONE) Then Return (cFRS_NORMAL)

            ' オートパワー「基板枚数指定」機能なし又は「時間(分)指定」機能なしならNOP
            If (giPwrChkPltNum = 0) And (giPwrChkTime = 0) Then Return (cFRS_NORMAL)

            '-------------------------------------------------------------------
            '   基板枚数指定時
            '-------------------------------------------------------------------
            If (giPwrChkPltNum = 1) Then                                '「基板枚数指定」機能あり ?
                If (typPlateInfo.intPwrChkPltNum = 0) Then              ' 基板枚数指定なし ?
                    If (giPwrChkTime = 0) Then                          '「時間(分)指定」機能なし ?
                        Return (cFRS_NORMAL)
                    End If
                Else
                    ' トリミングモードがx0,x1,x5以外なら
                    ' チェックカウンタに最大値を入れて抜ける(x0,x1,x5モードに変わった時に実行するため)
                    If (digL <> 0) And (digL <> 1) And (digL <> 5) Then
                        ChkCounter = typPlateInfo.intPwrChkPltNum + 1
                        Return (cFRS_NORMAL)
                    End If
                    ' チェックカウンタ > チェック枚数 ?
                    '                    If (ChkCounter <= typPlateInfo.intPwrChkPltNum) Then
                    If (ChkCounter < typPlateInfo.intPwrChkPltNum) Then
                        Return (cFRS_NORMAL)
                    End If
                    Flg = &H1                                           ' Flg = 基板枚数指定 
                End If
            End If

            '-------------------------------------------------------------------
            '   時間(分)指定時
            '-------------------------------------------------------------------
            '「時間(分)指定」機能なし/「時間(分)指定なし」ならNOP
            If (giPwrChkTime = 0) And ((Flg And &H1) = 0) Then Return (cFRS_NORMAL)
            If (typPlateInfo.intPwrChkTime = 0) And ((Flg And &H1) = 0) Then Return (cFRS_NORMAL)

            ' トリミングモードがx0,x1,x5以外なら
            ' 経過時間(分)に最大値を入れて抜ける(x0,x1,x5モードに変わった時に実行するため)
            If (digL <> 0) And (digL <> 1) And (digL <> 5) Then
                ts = New TimeSpan(0, typPlateInfo.intPwrChkTime + 1, 0)
                NowTimeSpan = ts
                Return (cFRS_NORMAL)
            End If

            ' 経過時間(分) > 指定時間(分) ?
            If (NowTimeSpan <= AssTimeSpan) Then
                If ((Flg And &H1) = 0) Then
                    Return (cFRS_NORMAL)
                End If
            Else
                Flg = Flg Or &H2                                        ' Flg = 時間(分)指定 
            End If

            '-------------------------------------------------------------------
            '   現在のレーザパワーを測定する
            '-------------------------------------------------------------------
STP_CHK:
            ' Zを原点へ移動
            r = EX_ZMOVE(0, MOVE_ABSOLUTE)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(メッセージは表示済み) 
                Return (r)                                              ' Return値設定 
            End If

            ' パワー調整する加工条件番号配列に有効/無効を設定する
            r = SetAutoPowerCndNumAry(stPWR)

            ' カットに使用する加工条件番号のパワー調整を行う 
            For CndNum = 0 To (MAX_BANK_NUM - 1)                        ' 最大加工条件数分繰り返す
                If (stPWR.CndNumAry(CndNum) = 1) Then                   ' 加工条件は有効 ?

                    ' 許容範囲[W](Hight/Low)を設定する
                    AdjustTarget = stPWR.AdjustTargetAry(CndNum)        ' 目標パワー値(W)
                    AdjustLevel = stPWR.AdjustLevelAry(CndNum)          ' 調整許容範囲(±W)
                    dblHi = AdjustTarget + AdjustLevel                  ' 許容範囲[W](Hight)
                    dblLo = AdjustTarget - AdjustLevel                  ' 許容範囲[W](Low)

                    ' パワー測定を行う
                    r = Form1.System1.Form_GetLaserPowerFL(gSysPrm, CndNum, iCurr, dblMeasure)
                    If (r < cFRS_NORMAL) Then                           ' エラー ? 
                        rtn = r                                         ' 非常停止等のエラーならリターン(エラーメッセージは表示済)
                        GoTo STP_END
                    End If
                    If (r = cFRS_ERR_RST) Then                          ' Cancel(RESETｷｰ) ?
                        rtn = r                                         ' ならリターン
                        GoTo STP_END
                    End If

                    ' 測定結果をメイン画面に表示する
                    ' メッセージ表示("レーザパワー測定値 = "+" 加工条件番号xx"+" = xx.xxW, " + "電流値=" + "xxxmA")
                    strMSG = MSG_AUTOPOWER_03 + "= " + MSG_AUTOPOWER_02 + CndNum.ToString("00")
                    strMSG = strMSG + ", " + dblMeasure.ToString("0.00") + "W, "
                    strMSG = strMSG + MSG_AUTOPOWER_04 + "= " + iCurr.ToString("0") + "mA"
                    Call Form1.Z_PRINT(strMSG)

                    ' 測定値は許容範囲内 ?
                    If (dblMeasure <= dblHi) And (dblMeasure >= dblLo) Then
                        stPWR.CndNumAry(CndNum) = 0                     ' 許容範囲内ならパワー調整しないように「加工条件を無効」とする
                        Continue For
                    Else
                        bPowerADJ = True                                ' 許容範囲外なら「パワー調整実行フラグ」をONにする
                    End If
                End If
            Next CndNum

            '-------------------------------------------------------------------
            '   レーザパワー調整を行う(測定パワーが範囲外のもののみ)
            '-------------------------------------------------------------------
            If (bPowerADJ = True) Then                                      '「パワー調整実行フラグ」ON ?
                r = AutoLaserPowerADJ(MD_ADJ)                               ' レーザパワー調整実行(調整モード)
                If (r = cFRS_ERR_RST) Then                                  ' Cancel(RESETｷｰ) ?
                    rtn = cFRS_ERR_RST                                      ' Return値 = Cancel(RESETｷｰ)
                ElseIf (r <> cFRS_NORMAL) Then                              ' エラー ?(※エラーメッセージは表示済み) 
                    Return (r)                                              ' 非常停止等のエラーならアプリ強制終了へ
                End If
            End If

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
STP_END:
            ' 基板枚数指定ならチェックカウンタ初期化を初期化する
            If ((Flg And &H1) = &H1) Then
                ChkCounter = 0
            End If

            ' 時間(分)指定なら開始時間/経過時間を初期化する
            If ((Flg And &H2) = &H2) Then
                StartTime = DateTime.Now
                NowTimeSpan = New TimeSpan(0, 0, 0)
            End If

            ' ブロックサイズを再設定する(Form_GetLaserPowerFL()で変更される為)
            r = Form1.System1.EX_BSIZE(gSysPrm, typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)                                              ' アプリ強制終了へ
            End If
            ' BPオフセット設定(※ブロックサイズを設定するとBPオフセットはINtime側で初期化される為)
            r = Form1.System1.EX_BPOFF(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)                                              ' アプリ強制終了へ
            End If

            Return (rtn)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.AutoPowerExeCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V4.11.0.0③↑ -----
    '----- V4.0.0.0-58↓ -----
#Region "オートパワー調整後の電流値をトリミングデータに反映する(SL436S時)"
    '''=========================================================================
    ''' <summary>オートパワー調整後の電流値をトリミングデータに反映する</summary>
    ''' <param name="stCND">(INP)FL用トリマー加工条件情報 ※配列は0オリジン</param>
    ''' <remarks></remarks>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          上記以外 　　= エラー</returns> 
    '''=========================================================================
    Private Function SetAutoPowerCurrData(ByRef stCND As TrimCondInfo) As Integer

        Dim Rn As Integer
        Dim Cn As Integer
        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   初期処理
            '------------------------------------------------------------------
            ' FLでないまたはSL436SでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)

            '------------------------------------------------------------------
            '   トリミングデータの電流値を更新する
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' １ブロック内抵抗数分チェックする 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' 抵抗内カット数分チェックする
                    ' カットタイプ取得
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

                    ' 加工条件1は全カット無条件にオートパワー調整後の電流値を設定する
                    CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1)
                    typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1) = stCND.Curr(CndNum)

                    ' 加工条件2はLカット, 斜めLカット, Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)
                    ' HOOKカット, Uカット時に設定する
                    If (strCutType = DataManager.CNS_CUTP_L) Or (strCutType = DataManager.CNS_CUTP_NL) Or
                       (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Or
                       (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then
                        ' 加工条件2
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2) = stCND.Curr(CndNum)
                    End If

                    ' 加工条件3はHOOKカット, Uカット時に設定する
                    If (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then
                        ' 加工条件3
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L3) = stCND.Curr(CndNum)
                    End If

                    ' 加工条件4は現状は未使用(予備)

                    ' 加工条件5～8はリターン/リトレース用 
                    ' 加工条件5(STカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めSTカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                    If (strCutType = DataManager.CNS_CUTP_STr) Or (strCutType = DataManager.CNS_CUTP_STt) Or
                       (strCutType = DataManager.CNS_CUTP_NSTr) Or (strCutType = DataManager.CNS_CUTP_NSTt) Then
                        ' 加工条件5の条件番号をカットデータより設定する
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET) = stCND.Curr(CndNum)
                    End If

                    ' 加工条件5,6(Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                    If (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Then
                        ' 加工条件5の条件番号をカットデータより設定する
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET) = stCND.Curr(CndNum)

                        ' 加工条件6の条件番号をカットデータの加工条件4より設定する
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET)
                        typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2_RET) = stCND.Curr(CndNum)
                    End If

                    ' 加工条件7,8は現状は未使用(予備)

                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return値設定 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "basTrimming.SetAutoPowerCurrData() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try
    End Function
#End Region
    '----- V4.0.0.0-58↑ -----
#Region "パワー調整する加工条件番号配列に有効/無効を設定する"
    '''=========================================================================
    ''' <summary>パワー調整する加工条件番号配列に有効/無効を設定する ###066</summary>
    ''' <param name="stPWR">(OUT)FL用パワー調整情報
    '''                              ※配列は0オリジン</param>
    ''' <remarks>自動レーザパワーの調整処理実行用</remarks>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          上記以外 　　= エラー</returns> 
    '''=========================================================================
    Private Function SetAutoPowerCndNumAry(ByRef stPWR As POWER_ADJUST_INFO) As Short

        Dim Rn As Integer
        Dim Cn As Integer
        Dim CndNum As Integer
        Dim strCutType As String
        Dim strMsg As String

        Try
            '------------------------------------------------------------------
            '   初期処理
            '------------------------------------------------------------------
            ' FLでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)

            ' トリマー加工条件構造体(FL用)初期化 
            stPWR.Initialize()

            '------------------------------------------------------------------
            '   加工条件番号配列を設定する
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' １ブロック内抵抗数分チェックする 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' 抵抗内カット数分チェックする
                    ' カットタイプ取得
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()
#If False Then
                    ' 加工条件1は全カット無条件に設定する
                    CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1)
                    If (stPWR.CndNumAry(CndNum) = 0) Then               ' 無効 ? 
                        stPWR.CndNumAry(CndNum) = 1                     ' 有効に設定
                        ' 目標パワー値(W)と調整許容範囲(±W)を設定する
                        stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1)
                        stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L1)
                    End If

                    ' 加工条件2はLカット, 斜めLカット, Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)
                    ' HOOKカット, Uカット時に設定する
                    If (strCutType = DataManager.CNS_CUTP_L) Or (strCutType = DataManager.CNS_CUTP_NL) Or _
                       (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or _
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Or _
                       (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then ' V1.22.0.0①
                        ' 加工条件2
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2)
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' 無効 ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' 有効に設定
                            ' 目標パワー値(W)と調整許容範囲(±W)を設定する
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2)
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L2)
                        End If
                    End If

                    ' 加工条件3はHOOKカット, Uカット時に設定する
                    If (strCutType = DataManager.CNS_CUTP_HK) Or (strCutType = DataManager.CNS_CUTP_U) Or (strCutType = DataManager.CNS_CUTP_Ut) Then ' V1.22.0.0①
                        ' 加工条件3
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3)
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' 無効 ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' 有効に設定
                            ' 目標パワー値(W)と調整許容範囲(±W)を設定する
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L3)
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L3)
                        End If
                    End If

                    ' 加工条件4は現状は未使用(予備)

                    ' 加工条件5～8はリターン/リトレース用 
                    ' 加工条件5(STカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めSTカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                    If (strCutType = DataManager.CNS_CUTP_STr) Or (strCutType = DataManager.CNS_CUTP_STt) Or _
                       (strCutType = DataManager.CNS_CUTP_NSTr) Or (strCutType = DataManager.CNS_CUTP_NSTt) Then
                        ' 加工条件5の条件番号をカットデータの加工条件2より設定する
                        'CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(1)                 'V4.0.0.0-58
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)     'V4.0.0.0-58
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' 無効 ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' 有効に設定
                            ' 目標パワー値(W)と調整許容範囲(±W)を設定する
                            'stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(1)'V4.0.0.0-58
                            'stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(1)'V4.0.0.0-58
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET) 'V4.0.0.0-58
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L1_RET) 'V4.0.0.0-58
                        End If
                    End If

                    ' 加工条件5,6(Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                    If (strCutType = DataManager.CNS_CUTP_Lr) Or (strCutType = DataManager.CNS_CUTP_Lt) Or _
                       (strCutType = DataManager.CNS_CUTP_NLr) Or (strCutType = DataManager.CNS_CUTP_NLt) Then
                        ' 加工条件5の条件番号をカットデータの加工条件3より設定する
                        'CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(2)                 'V4.0.0.0-58
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET)     'V4.0.0.0-58
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' 無効 ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' 有効に設定
                            ' 目標パワー値(W)と調整許容範囲(±W)を設定する
                            'stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(2)                'V4.0.0.0-58
                            'stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(2)              'V4.0.0.0-58
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET)    'V4.0.0.0-58
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L1_RET)  'V4.0.0.0-58
                        End If
                        ' 加工条件6の条件番号をカットデータの加工条件4より設定する
                        'CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(3)             'V4.0.0.0-58
                        CndNum = typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET) 'V4.0.0.0-58
                        If (stPWR.CndNumAry(CndNum) = 0) Then           ' 無効 ? 
                            stPWR.CndNumAry(CndNum) = 1                 ' 有効に設定
                            ' 目標パワー値(W)と調整許容範囲(±W)を設定する
                            'stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(3)                'V4.0.0.0-58
                            'stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(3)              'V4.0.0.0-58
                            stPWR.AdjustTargetAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2_RET)    'V4.0.0.0-58
                            stPWR.AdjustLevelAry(CndNum) = typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustToleLevel(CUT_CND_L2_RET)  'V4.0.0.0-58
                        End If
                    End If

                    ' 加工条件7,8は現状は未使用(予備)
#Else
                    For cndIdx As Integer = 0 To (DataManager.MaxCndNum - 1) Step 1                 '#5.0.0.8①
                        Dim arrIdx As Integer = DataManager.GetArrCutFLIndex(cndIdx, strCutType)    '#5.0.0.8①
                        If (0 <= arrIdx) Then
                            With typResistorInfoArray(Rn).ArrCut(Cn)
                                CndNum = .CndNum(arrIdx)                ' 加工条件番号
                                If (0 = stPWR.CndNumAry(CndNum)) Then   ' 設定されていない ?
                                    stPWR.CndNumAry(CndNum) = 1         ' 設定済みとする
                                    ' 指定加工条件番号の目標パワー値(W)と調整許容範囲(±W)を設定する
                                    stPWR.AdjustTargetAry(CndNum) = .dblPowerAdjustTarget(arrIdx)
                                    stPWR.AdjustLevelAry(CndNum) = .dblPowerAdjustToleLevel(arrIdx)
                                End If
                            End With
                        End If
                    Next cndIdx
#End If
                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return値設定 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "basTrimming.SetAutoPowerCndNumAry() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try
    End Function
#End Region

#Region "Trimming実行中-HALT押下時動作"
    '''=========================================================================
    ''' <summary>トリミング実行中のHALTスイッチ動作（一時停止動作）</summary>
    ''' <param name="curPltNo"></param>
    ''' <param name="curBlkNo"></param>
    ''' <param name="curPltNoX"></param>
    ''' <param name="curPltNoY"></param>
    ''' <param name="curBlkNoX"></param>
    ''' <param name="curBlkNoY"></param>
    ''' <param name="bFgAutoMode"></param>
    ''' <remarks>HALTスイッチ押下時は一時処理を停止し、
    '''          BPオフセットを調整できるようにする。
    '''          TKYCHIPでは、抵抗やカットの条件も変更できるようにする。</remarks>
    ''' <returns>-1:次のステップへ移動</returns> 
    '''=========================================================================
    Public Function HaltSwOnMove(ByRef curPltNo As Integer, ByRef curBlkNo As Integer,
                                 ByRef curPltNoX As Integer, ByRef curPltNoY As Integer,
                                 ByRef curBlkNoX As Integer, ByRef curBlkNoY As Integer,
                                 ByVal bFgAutoMode As Boolean) As Integer                   '#4.12.2.0⑥
        '#4.12.2.0⑥    Public Function HaltSwOnMove(ByRef curPltNo As Integer, ByRef curBlkNo As Integer, ByVal bFgAutoMode As Boolean) As Integer

        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        'Dim objForm As Object                                                  '###053
        Dim r As Integer                                                        '###054
        Dim swSts As Integer
        Dim RtnCode As Integer = cFRS_NORMAL                                    '###073 
        Dim AlarmCount As Integer                                               '###073 
        Dim strLoaderAlarm(LALARM_COUNT) As String
        Dim strLoaderAlarmInfo(LALARM_COUNT) As String
        Dim strLoaderAlarmExec(LALARM_COUNT) As String
        Dim coverSts As Long                                                    '###213
        Dim rtn As Integer
        Dim xPos As Double                                                      'V4.8.0.1③
        Dim yPos As Double                                                      'V4.8.0.1③

        Try

            ''V4.12.2.2⑥↓'V6.0.5.0⑨
            GraphDispSet()
            ''V4.12.2.2⑥↑'V6.0.5.0⑨

            ' HALT SW読み込み
            Call HALT_SWCHECK(swSts)
            'If (swSts = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Then          '###255 ###009
            If (swSts = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Or (gbHaltSW = True) Then           '###255
                gbHaltSW = False                                                ' ###255
                '----- V4.0.0.0⑲↓ -----
                ' SL436S時は自動運転時は「ADJ ON」以外は一時停止画面を表示しない(HALTはサイクル停止)(ローム殿特注)
                'V5.0.0.4①     If (bFgAutoMode = True) And (gbChkboxHalt <> True) And (giMachineKd = MACHINE_KD_RS) And (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                If (bFgAutoMode = True) And (gbChkboxHalt <> True) And (gbCycleStop Or ((giMachineKd = MACHINE_KD_RS) And (gSysPrm.stCTM.giSPECIAL = customROHM))) Then
                    Call LAMP_CTRL(LAMP_HALT, True)                             ' HALTランプON
                    bFgCyclStp = True
                    '----- V6.0.3.0⑧↓ -----
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_CYCLE_STOP)
                    '----- V6.0.3.0⑧↑ -----
                    '----- V6.0.3.0_37↓ -----
                    If gVacuumIO = 1 Then
                        'V4.11.0.0⑬ クランプ及びバキュームOFF
                        If gKeiTyp = KEY_TYPE_RS Then
                            r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                        End If
                        'V4.11.0.0⑬
                    End If
                    '----- V6.0.3.0_37↑ -----
                    Form1.btnCycleStop.Text = "CYCLE STOP ON"                   ' V5.0.0.4①
                    Form1.btnCycleStop.BackColor = System.Drawing.Color.Yellow  ' V5.0.0.4①
                    Return (cFRS_NORMAL)
                End If
                '----- V4.0.0.0⑲↑ -----
                '----- ###214↓ ----
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)                 ' システムエラーチェック
                If (r <> cFRS_NORMAL) Then
                    Return (r)
                End If
                '----- ###214↑ ----

                ' ローダを停止する 
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R系 ? 
                    ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))(SL436R用)
                    If (bFgAutoMode = True) Then                                ' ローダ自動モード? ###001(下記を出力するとクランプ・吸着OFFする)
                        'Call SetLoaderIO(&H0, LOUT_SUPLY)                       ' ローダは現サイクル終了時点(収納終わり)で停止する
                        Call SetLoaderIO(LOUT_STOP, &H0)                        ' ローダ出力(ON=トリマ停止中, OFF=なし) ###073
                    End If
                    'V4.8.0.1③↓
                    ' 補正クロスラインを表示する
                    If gMachineType = MACHINE_TYPE_436S Then
                        Call ZGETBPPOS(xPos, yPos)
                        ObjCrossLine.CrossLineDispXY(xPos, yPos)
                    End If
                    'V4.8.0.1③↑
                Else
                    ' ローダ出力(ON=動作中, OFF=なし)                           ' 432R系
                    Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                    ' トリマ部一時停止状態とする '###035
                End If

                ' アプリモードを「一時停止画面」にする ###088
                giAppMode = APP_MODE_FINEADJ
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436系の場合は
                    Call COVERCHK_ONOFF(COVER_CHECK_OFF)                        ' 「固定カバー開チェックなし」にする
                    '----- V1.18.0.1⑧↓ -----
                    ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
                    RtnCode = EL_Lock_OnOff(EX_LOK_MD_OFF)
                    If (RtnCode = cFRS_TO_EXLOCK) Then '                        ' 「前面扉ロックタイムアウト」なら戻り値を「RESET」にする
                        RtnCode = cFRS_ERR_RST
                        GoTo STP_END                                            ' 処理終了へ 
                    End If
                    If (r < cFRS_NORMAL) Then                                   ' 異常終了レベルのエラー ? 
                        Return (r)                                              ' アプリケーション強制終了
                    End If
                    RtnCode = cFRS_NORMAL
                    '----- V1.18.0.1⑧↑ -----
                End If

                ' CMOSﾒﾓﾘよりWindows側時間を設定する
                'V4.3.0.0⑥　Call GETSETTIME()
                ' ランプON
                Call LAMP_CTRL(LAMP_START, True)  ' STARTランプON 
                Call LAMP_CTRL(LAMP_RESET, True)  ' RESETランプON 

                'デジタルスイッチの値取得
                Call Form1.GetMoveMode(digL, digH, digSW)

                'Versionボタン等の無効化
                Form1.mnuHelpAbout.Enabled = False

                ' マガジン上下動作　無効
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R系 ? ###182
                    'V4.0.0.0⑰↓
                    If gKeiTyp <> KEY_TYPE_RS Then
                        Form1.GroupBox1.Visible = True
                    End If
                    'V4.0.0.0⑰↑
                End If

                'ログ表示エリアの移動
                Dim orgSize As System.Drawing.Size
                Dim orgLocation As System.Drawing.Point
                Dim setSize As System.Drawing.Size

                ' Distributeボタンを非表示へ
                Form1.chkDistributeOnOff.Visible = False
                Form1.GrpNgBox.Visible = False                                '###149

                ' V4.11.0.0⑯↓
                '                Form1.BtnSubstrateSet.Enabled = False 'V4.11.0.0⑪
                If gbLastSubstrateSet = False And (bFgAutoMode = True) Then
                    Form1.BtnSubstrateSet.Enabled = True
                Else
                    Form1.BtnSubstrateSet.Enabled = False
                End If
                ' V4.11.0.0⑯↑

                ''V5.0.0.1④↓
                If giNgStop = 1 Then
                    btnJudgeEnable(True)
                End If
                ''V5.0.0.1④↑

                '元の領域を保存
                orgSize = Form1.txtLog.Size
                orgLocation = Form1.txtLog.Location

                If gKeiTyp <> KEY_TYPE_RS Then
                    'V6.0.0.0⑥                  ↓
                    Form1.Instance.frmHistoryData.Visible = False
                    If (Form1.Instance.VideoLibrary1.SetTrackBarVisible(True)) Then
                        Form1.Instance.txtLog.Location =
                            Point.Add(Form1.Instance.HistoryDataLocation, New Size(0, 30))
                        setSize.Height = 420
                    Else    'V6.0.0.0⑥                  ↑
                        Form1.txtLog.Location = Form1.Instance.HistoryDataLocation
                        setSize.Height = 450
                    End If
                    setSize.Width = Form1.Instance.HistoryDataSize.Width
                    Form1.txtLog.Size = setSize
                    Form1.SetTrimMapVisible(Form1.MapOn)                'V6.0.1.0⑪
                End If

                '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
                ' WALSIN殿は非表示にしない
                'Form1.GrpQrCode.Visible = False                                ' QRｺｰﾄﾞ情報を非表示 V1.18.0.0②
                ' ローム殿特注 ? 
                'V5.0.0.9⑲                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                'V5.0.0.9⑳                If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.Taiyo = BarCode_Data.Type) Then 'V5.0.0.9⑲
                If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.Walsin <> BarCode_Data.Type) Then 'V5.0.0.9⑲
                    Form1.GrpQrCode.Visible = False                             ' QRｺｰﾄﾞ情報を非表示 V1.18.0.0②
                End If
                '----- V4.11.0.0②↑ -----

                'メイン画面のスクロールバーは無効になるため、
                '表示位置を最下層へ設定し消す
                'Form1.txtLog.ScrollBars = ScrollBars.None                      ' スクロールバーは表示する ###014
                '#4.12.2.0④                Form1.txtLog.ScrollToCaret()                                    ' 現在のカレット位置までスクロールする(一番下まで表示する) 
                Form1.TxtLogScrollToCaret() '#4.12.2.0④

                '----- V1.18.0.7①↓ -----
                ' SL436R時で自動運転中の一時停止時は、シグナルタワーの緑点灯(自動運転中)は消す
                If ((bFgAutoMode = True) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436)) Then
                    ' シグナルタワー制御(On=なし, Off=自動運転中(緑点灯))
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_GRN_ON)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_IDLE)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧ 

                End If
                '----- V1.18.0.7①↑ -----

                '--------------------------------------------------------------
                '   一時停止用微調整フォームの表示
                '--------------------------------------------------------------
                gbExitFlg = False                                               '###014
                'objForm = New frmFineAdjust()                                  '###053
                gObjADJ = New frmFineAdjust()                                   '###053

                'frmFineAdjust.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo)'###014
                'Call frmFineAdjust.Focus() ' ###014
                'Call frmFineAdjust.ShowDialog()' ###014

                '----- ###053(変更)ここから -----
                'Call objForm.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo)
                'Call objForm.Focus()                                           ' ###014
                'Call objForm.Show()                                            ' ###014

                If gKeiTyp = KEY_TYPE_RS Then
                    ' 画像表示プログラムを終了する
                    'V6.0.0.0⑤                    r = End_SmallGazouProc(ObjGazou)
                    GroupBoxEnableChange(True)
                    Form1.CbDigSwH.Enabled = True                  ' DisplayModeコンボボックスリスト
                    Form1.CbDigSwL.Enabled = True                  ' MoveModeコンボボックスリスト
                    'V4.0.0.0②
                    'スタートスイッチラッチクリア   
                    Call ZCONRST()
                    'V4.0.0.0②
                    'V4.11.0.0⑨
                    GrpStartBlkPartEnable()
                    'V4.11.0.0⑨

                    'V6.0.0.0⑤                Else
                    ' 画像表示プログラムを終了する '###054
                    'V6.0.0.0⑤                    Call End_GazouProc(ObjGazou)
                End If
                Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(False)   'V6.0.0.0-26

                '---------------------------------------------------------------
                '   一時停止用微調整フォームを表示する
                '---------------------------------------------------------------

                '#4.12.2.0⑥                Call gObjADJ.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo)
                Call gObjADJ.SetInitialData(gSysPrm, digL, digH, curPltNo, curBlkNo,
                                            curPltNoX, curPltNoY, curBlkNoX, curBlkNoY)     '#4.12.2.0⑥
                If gKeiTyp <> KEY_TYPE_RS Then

                    Form1.SetMapOnOffButtonEnabled(True)                        ' MAP ON/OFF ボタンを有効にする  'V4.12.2.0①

                    ' SL432R/SL436Rの場合
                    Call gObjADJ.Focus()
                    Call gObjADJ.Show()
                    '----- ###053(変更)ここまで -----
                Else
                    ' SL436Sの場合
                    BPAdjustButton.Visible = True
                    '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) ----
                    ' 一時停止開始時間を設定する(オプション)
                    m_blnElapsedTime = True                             ' 経過時間を表示する
                    Call Set_PauseStartTime(StPauseTime)
                    '----- V4.11.0.0④↑ -----
                    Form1.TimerAdjust.Enabled = True
                    'V4.0.0.0-82        ' 
                    BlockNextButton.Enabled = True
                    BlockRvsButton.Enabled = True
                    BlockMainButton.Enabled = True
                    'V4.0.0.0-82        ' 
                    DataEditButton.Visible = True 'V4.0.0.0-84 
                    '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
                    ' トリミング開始ブロック番号指定(手動モード時(オプション))活性化
                    'V5.0.0.9⑯                    If (giStartBlkAss = 1) And (bFgAutoMode = False) Then
                    If (giStartBlkAss <> 0) AndAlso (bFgAutoMode = False) Then  'V5.0.0.9⑯
                        Call Form1.Set_StartBlkNum_Enabled(True)
                    End If
                    '----- V4.11.0.0⑤↑ -----
                End If

                ' 一時停止用微調整フォームの終了を待つ ###014
                Do
                    System.Threading.Thread.Sleep(100)                          ' Wait(ms)
                    System.Windows.Forms.Application.DoEvents()

                    'V4.8.0.1③↓
                    ' 補正クロスラインを表示する
                    If gMachineType = MACHINE_TYPE_436S Then
                        Call ZGETBPPOS(xPos, yPos)
                        ObjCrossLine.CrossLineDispXY(xPos, yPos)
                    End If
                    'V4.8.0.1③↑
                    ' インターロック状態の表示およびローダへ通知(SL436R) ###162
                    r = Form1.DispInterLockSts()
                    '----- ###213↓ -----
                    ' インターロック全解除/一部解除で、カバー閉は異常とする(SL436R時) 
                    If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                        'V4.0.0.0⑭    If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gKeiTyp <> KEY_TYPE_RS) Then  'V4.0.0.0⑭

                        r = COVER_CHECK(coverSts)                           ' 固定カバー状態取得(0=固定カバー開, 1=固定カバー閉))
                        If (coverSts = 1) Then                              ' 固定カバー閉 ?
                            ' ハードウェアエラー(カバースイッチオン)メッセージ表示
                            Call Form1.System1.Form_Reset(cGMODE_ERR_HW, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                            RtnCode = cFRS_ERR_HW                           ' Return値 = ハードウェアエラー検出
                            Return (RtnCode)                                ' アプリ強制終了
                        End If
                    End If
                    '----- ###213↑ -----

                    ' ローダアラームチェック(ローダ自動モード時)(SL436R) ###073
                    If (bFgAutoMode = True) And (giErrLoader <> cFRS_ERR_RST) Then
                        If (gfrmAdjustDisp = 1) Then
                            gObjADJ.Sub_StopTimer()
                        End If
                        'V6.0.0.0⑱                        r = Loader_AlarmCheck(gSysPrm, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
                        r = Loader_AlarmCheck(Nothing, True, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec) 'V6.0.0.0⑱
                        If (r <> cFRS_NORMAL) Then
                            'V5.0.0.1⑬↓
                            If gKeiTyp = KEY_TYPE_RS Then
                                Form1.BtnSubstrateSet.Enabled = False                       ' 基板投入ボタン非活性化
                            End If
                            'V5.0.0.1⑬↑
                            giErrLoader = r                                     ' ローダアラーム検出ON
                            RtnCode = r                                         ' Return値設定 
                            Call W_RESET()                                      ' アラームリセット送出 
                            'V5.0.0.1⑯
                            If (r <> cFRS_ERR_RST) Then
                                Call W_START()                                      ' スタート信号送出 
                            End If
                            'V5.0.0.1⑯
                        End If
                        If (gfrmAdjustDisp = 1) Then
                            gObjADJ.Sub_StartTimer()
                        End If

                    End If
                    ''V5.0.0.1-28
                    If (bFgAutoMode = False) Then
                        '----- ###209↓ -----
                        ' カバー閉を確認する(SL436R時で手動モード時)
                        If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (bFgAutoMode = False) Then
                            Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア
                            r = FrmReset.Sub_CoverCheck()
                            If (r < cFRS_NORMAL) Then                               ' 非常停止等検出 ?
                                RtnCode = r                                     ' Return値 = ハードウェアエラー検出
                                Return (RtnCode)                                ' アプリ強制終了
                            End If
                        End If
                        '----- ###209↑ -----
                    End If
                    ''V5.0.0.1-28

                    'V4.0.0.0①　↓
                    ' 非常停止等チェック(トリマ装置アイドル中)
                    r = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
                    If (r <> cFRS_NORMAL) Then                          ' 非常停止等検出 ?
                        RtnCode = r                                     ' Return値 = ハードウェアエラー検出
                        Return (RtnCode)                                ' アプリ強制終了
                    End If
                    'V4.0.0.0①　↑

                Loop While (gbExitFlg = False)

                '----- ###232↓(表示はDllTrimClassLiblaryで表示される) -----
                ' 補正後クロスラインX,Y非表示
                'V6.0.0.0④                Form1.CrosLineX.Visible = False
                'V6.0.0.0④                Form1.CrosLineY.Visible = False
                Form1.VideoLibrary1.SetCorrCrossVisible(False)          'V6.0.0.0④
                '----- ###232↑ -----
                If gKeiTyp = KEY_TYPE_RS Then
                    BPAdjustButton.Visible = False
                    Form1.TimerAdjust.Enabled = False
                    '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) ----
                    ' 一時停止終了時間を設定し一時停止時間を集計する(オプション)
                    Call Set_PauseTotalTime(StPauseTime)
                    ' DllTrimClassLibraryに一時停止時間を渡す(オプション)
                    TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
                    '----- V4.11.0.0④↑ -----
                    SetSimpleVideoSize()
                    GroupBoxEnableChange(False)
                    ' データ表示を非表示にする
                    Call Form1.SetDataDisplayOn()                           'V4.0.0.0⑪
                    GroupBoxVisibleChange(True)                             'V4.0.0.0⑪
                    'V4.0.0.0-82        ' 
                    BlockNextButton.Enabled = False
                    BlockRvsButton.Enabled = False
                    BlockMainButton.Enabled = False
                    'V4.0.0.0-82        ' 
                    DataEditButton.Visible = False 'V4.0.0.0-84 
                    '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
                    ' トリミング開始ブロック番号指定(手動モード時(オプション))非活性化
                    'V5.0.0.9⑯                    If (giStartBlkAss = 1) And (bFgAutoMode = False) Then
                    If (giStartBlkAss <> 0) AndAlso (bFgAutoMode = False) Then  'V5.0.0.9⑯
                        Call Form1.Set_StartBlkNum_Enabled(False)
                    End If
                    '----- V4.11.0.0⑤↑ -----
                    'V4.11.0.0⑨
                    Form1.btnNEXT.Enabled = False
                    Form1.btnPREV.Enabled = False
                    'V4.11.0.0⑨
                    'V5.0.0.1⑭↓
                    If (giSubstrateInvBtn = 1) Then                         ' 一時停止画面での「基板投入」ボタンの有効 ?　
                        If giErrLoader <> cFRS_NORMAL Then
                            RtnCode = giErrLoader                                         ' Return値設定 
                        End If
                    End If
                    'V5.0.0.1⑭↑
                End If

                ''V5.0.0.1④↓
                If giNgStop = 1 Then
                    btnJudgeEnable(False)
                End If
                ''V5.0.0.1④↑
                '----- ###141↓ -----
                ' ローダアラーム検出(軽故障)で続行指定ならリターン値に正常を設定する
                'If (RtnCode = cFRS_ERR_LDR3) Then                              ' ###196 ローダアラーム検出(軽故障) ?
                If (RtnCode = cFRS_ERR_LDR3) Or (RtnCode = cFRS_ERR_LDR2) Then  ' ###196 ローダアラーム検出(軽故障, サイクル停止) ?
                    giErrLoader = cFRS_NORMAL
                    RtnCode = cFRS_NORMAL
                End If
                '----- ###141↑ -----

                '----- V1.18.0.6①↓ -----
                ' SL436R時で自動運転中はシグナルタワー制御(自動運転中(緑点灯))を行う
                If ((bFgAutoMode = True) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436)) Then
                    ' シグナルタワー制御(On=自動運転中(緑点灯),Off=全ﾋﾞｯﾄ)
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                    'Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧
                End If
                '----- V1.18.0.6①↑ -----
                '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
                ' トリミング開始ブロック番号を表示する
                'V5.0.0.9⑯                If (giStartBlkAss = 1) Then                             ' トリミング開始ブロック番号指定の有効/無効(0=無効, 1=有効)　
                If (giStartBlkAss <> 0) AndAlso (gKeiTyp = KEY_TYPE_RS) Then    ' トリミング開始ブロック番号指定の有効/無効(0=無効, 1=有効)     'V5.0.0.9⑯
                    Form1.GrpStartBlk.Visible = True
                    'V5.0.0.1①↓
                    SetNowBlockDspNum(gCurBlockNo)
                    'V5.0.0.1①↑
                End If
                '----- V4.11.0.0⑤↑ -----

                ' ランプOFF
                Call LAMP_CTRL(LAMP_START, False)                               ' STARTランプOFF 
                Call LAMP_CTRL(LAMP_RESET, False)                               ' RESETランプOFF 

                '----- ###150↓ -----
                ' 統計表示時は統計表示上のボタンを無効にする(CHIP/NET時)
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then             ' ###157 (CHIP/NET時)
                    gObjFrmDistribute.cmdGraphSave.Enabled = False
                    gObjFrmDistribute.cmdInitial.Enabled = False
                    gObjFrmDistribute.cmdFinal.Enabled = False
                End If
                '----- ###150↑ -----

                ' マガジン上下動作　無効
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R系 ? ###182
                    Form1.GroupBox1.Visible = False
                End If

                'V4.11.0.0⑪↓
                If bFgAutoMode = True Then
                    'V4.11.0.0⑯↓
                    '                    Form1.BtnSubstrateSet.Enabled = True
                    Form1.BtnSubstrateSet.Enabled = False
                    'V4.11.0.0⑯↑
                End If
                'V4.11.0.0⑪↑
#If False Then                          'V6.0.0.0⑤
                ' 画像表示プログラムを起動する '###054
                If gKeiTyp = KEY_TYPE_RS Then
                    ' ↓↓↓ V3.1.0.0② 2014/12/01
                    'r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                    r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 1)
                    ' ↑↑↑ V3.1.0.0② 2014/12/01
                    '                    r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                Else
                    If (Form1.chkDistributeOnOff.Checked = False) Then              ' 統計画面非表示時に起動する ###116
                        ' ↓↓↓ V3.1.0.0② 2014/12/01
                        'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                        r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                        ' ↑↑↑ V3.1.0.0② 2014/12/01
                    End If

                End If
#End If
                ' ログ表示エリアを元に戻す
                '元の領域を保存
                Form1.txtLog.Location = orgLocation
                Form1.txtLog.Size = orgSize
                'Form1.txtLog.ScrollBars = ScrollBars.Vertical
                Form1.txtLog.ScrollBars = ScrollBars.Both                       ' ###010 
                '#4.12.2.0④                Form1.txtLog.ScrollToCaret()
                Form1.TxtLogScrollToCaret()     '#4.12.2.0④

                'V6.0.0.0⑥                  ↓
                If gKeiTyp <> KEY_TYPE_RS Then
                    Form1.Instance.VideoLibrary1.SetTrackBarVisible(False)
                    Form1.Instance.frmHistoryData.Visible = True
                    Form1.MoveHistoryDataLocation(True)                     'V6.0.1.0⑪

                    Form1.SetMapOnOffButtonEnabled(False)                        ' MAP ON/OFF ボタンを有効にする  'V4.12.2.0①
                    Form1.SetTrimMapVisible(True)                       'V6.0.1.0⑪
                End If
                'V6.0.0.0⑥                  ↑

                '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
                '----- V1.18.0.0②↓ -----
                ' QRｺｰﾄﾞ情報/バーコード情報(太陽社)/バーコード情報(WALSIN)を表示 V1.23.0.0①
                'V5.0.0.9⑮                If (gSysPrm.stCTM.giSPECIAL = customROHM) Or (gSysPrm.stCTM.giSPECIAL = customTAIYO) Or (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
                'If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse
                '    (BarcodeType.Taiyo = BarCode_Data.Type) OrElse
                '    (BarcodeType.Walsin = BarCode_Data.Type) Then ' V5.0.0.9⑮
                If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.None <> BarCode_Data.Type) Then 'V5.0.0.9⑳
                    Form1.GrpQrCode.Visible = True
                End If
                '----- V1.18.0.0②↑ -----
                '----- V4.11.0.0②↑ -----

                ' データ取得
                'objForm.GetStagePosInfo(curPltNo, curBlkNo)                        '###053
                'HaltSwOnMove = objForm.GetReturnVal()                              '###053
                '#4.12.2.0⑥                gObjADJ.GetStagePosInfo(curPltNo, curBlkNo)                         '###053
                gObjADJ.GetStagePosInfo(curPltNo, curBlkNo, curPltNoX, curPltNoY, curBlkNoX, curBlkNoY) '#4.12.2.0⑥
                r = gObjADJ.GetReturnVal()                                          '###053

                ' メイン側のタイマーでRESETボタンを検出した場合の処理
                'V4.10.0.0⑩                If gKeiTyp = KEY_TYPE_RS Then
                If (gMachineType = MACHINE_TYPE_436S) Then
                    'V4.9.0.0①↓
                    ' 初期値、切り替えポイント、ターンポイントのテーブルを転送する。 
                    SendTrimDataCutPoint()
                    'V4.9.0.0①↑

                    rtn = Form1.GetResultAdjust()
                    If (rtn = cFRS_ERR_RST) Then
                        r = rtn
                    End If
                End If
                If (RtnCode = cFRS_NORMAL) Then                                     '###073 
                    RtnCode = r
                End If

                'V4.1.0.0⑮↓
                'If (gbChkboxHalt = True) Then                                       '###009
                '    Form1.BtnADJ.Text = "ADJ ON"                                    '###009
                '    Form1.BtnADJ.BackColor = System.Drawing.Color.Yellow            '###009
                'Else                                                                '###009
                '    Form1.BtnADJ.Text = "ADJ OFF"                                   '###009
                '    Form1.BtnADJ.BackColor = System.Drawing.SystemColors.Control    '###009
                'End If                                                              '###009
                SetADJButton()
                'V4.1.0.0⑮↑

                ' Versionボタン等の有効化
                Form1.mnuHelpAbout.Enabled = True
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then               ' ###149
                    'Form1.GrpNgBox.Visible = True
                    Form1.GrpNgBox.Visible = False                                  '###195
                End If

                ' 微調整画面オブジェクト開放
                '----- ###053(変更)ここから -----
                ''Call frmFineAdjust.Close()                                    ' ###014
                'If (objForm Is Nothing = False) Then
                '    Call objForm.Close()                                       ' オブジェクト開放
                '    Call objForm.Dispose()                                     ' リソース開放
                'End If
                If (gObjADJ Is Nothing = False) Then
                    Call gObjADJ.Sub_StopTimer()                                ' ###260
                    Call gObjADJ.Close()                                        ' オブジェクト開放
                    Call gObjADJ.Dispose()                                      ' リソース開放
                    gObjADJ = Nothing
                End If
                '----- ###053(変更)ここまで -----
                Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(True)    'V6.0.0.0-26

                ' 表示の更新
                Form1.Refresh()

                '----- ###088↓ -----
STP_END:        ' V1.18.0.1⑧
                ' SL436R時は筐体カバー閉を確認する
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R系 ?
                    Do
                        System.Threading.Thread.Sleep(100)                      ' Wait(ms)
                        System.Windows.Forms.Application.DoEvents()

                        ' 筐体カバー閉を確認する
                        r = FrmReset.CoverCheck(0, False)                       ' 筐体カバー閉チェック(RESETキー無効指定, 原点復帰処理中以外)
                        If (r = cFRS_NORMAL) Then Exit Do
                        If (r <> ERR_OPN_CVR) Then Return (r) '                 ' 非常停止等のエラー

                        ' "筐体カバーを閉じて","","STARTキーを押すか、OKボタンを押して下さい。"
                        r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                                MSG_SPRASH36, "", MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        ' 非常停止等のエラーならアプリ強制終了へ(エラーメッセージは表示済み) 
                        'If (r < cFRS_NORMAL) Then Return (RtnCode)             ' ###193
                        If (r < cFRS_NORMAL) Then Return (r) '                  ' ###193
                        Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア

                    Loop While (1)
                End If
                '----- ###088↑ -----
                '----- ###193↓ -----
                ' RESETキー/エラーならReturn(###088の前から後に移動)
                If (RtnCode = cFRS_ERR_RST) Or (RtnCode <> cFRS_NORMAL) Then    ' ###073
                    If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436系の場合は「固定カバー開チェックあり」にする###133
                        Call COVERLATCH_CLEAR()                                 ' カバー開ラッチのクリア
                        Call COVERCHK_ONOFF(COVER_CHECK_ON)                     ' 「固定カバー開チェックあり」
                    End If
                    Return (RtnCode)
                End If
                '----- ###193↑ -----

                ' ローダを再開する
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R系 ? 
                    ' ローダ出力(ON=基板要求(連続運転開始), OFF=なし)(SL436R用)
                    If (bFgAutoMode = True) Then                                ' ローダ自動モード? ###001(下記を出力するとクランプ・吸着OFFする)
                        'Call SetLoaderIO(LOUT_SUPLY, &H0)                       ' 連続運転再開
                        Call SetLoaderIO(&H0, LOUT_STOP)                        ' ローダ出力(ON=なし, OFF=トリマ停止中) ###07
                    End If
                Else
                    ' ローダ出力(ON=運転中, OFF=なし)                           ' 432R系
                    Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                    ' トリマ部運転中とする ###035
                End If

                ' アプリモードを「トリミング中」に戻す ###088
                giAppMode = APP_MODE_TRIM
                Call COVERCHK_ONOFF(COVER_CHECK_ON)                             ' 「固定カバー開チェックあり」にする(SL436R時)
                Call COVERLATCH_CLEAR()                                         ' カバー開ラッチのクリア

                '----- V1.18.0.1⑧↓ -----
                ' 電磁ロック(観音扉右側ロック)する(ローム殿特注)
                If (bFgAutoMode = True) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    RtnCode = EL_Lock_OnOff(EX_LOK_MD_ON)
                    If (RtnCode = cFRS_TO_EXLOCK) Then RtnCode = cFRS_ERR_RST ' ' 前面扉ロックタイムアウトなら戻り値を「RESET」にする
                End If
                '----- V1.18.0.1⑧↑ -----
            End If

            ''V4.12.2.2⑥↓'V6.0.5.0⑨
            GraphDispSet()
            ''V4.12.2.2⑥↑'V6.0.5.0⑨
            Return (RtnCode)                                                    '###073 

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.HaltSwOnMove() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)

        Finally                         'V6.0.0.0⑩
            Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)
        End Try

    End Function
#End Region
    '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) -----
#Region "一時停止開始時間を設定する"
    '''=========================================================================
    ''' <summary>一時停止開始時間を設定する</summary>
    ''' <param name="StPauseTime">(OUT)一時停止時間集計用構造体</param>
    '''=========================================================================
    Public Sub Set_PauseStartTime(ByRef StPauseTime As PauseTime_Data_Info)

        Dim strMsg As String

        Try
            StPauseTime.StartTime = DateTime.Now
            StPauseTime.EndTime = StPauseTime.StartTime

        Catch ex As Exception
            strMsg = "basTrimming.Set_PauseStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "一時停止終了時間を設定し一時停止時間を集計する"
    '''=========================================================================
    ''' <summary>一時停止終了時間を設定し一時停止時間を集計する</summary>
    ''' <param name="StPauseTime">(OUT)一時停止時間集計用構造体</param>
    '''=========================================================================
    Public Sub Set_PauseTotalTime(ByRef StPauseTime As PauseTime_Data_Info)

        Dim strMsg As String

        Try
            StPauseTime.EndTime = DateTime.Now                                      ' 一時停止終了時間
            StPauseTime.PauseTime = StPauseTime.EndTime - StPauseTime.StartTime     ' 一時停止時間
            StPauseTime.TotalTime = StPauseTime.TotalTime + StPauseTime.PauseTime   ' 一時停止トータル時間

        Catch ex As Exception
            strMsg = "basTrimming.Set_PauseTotalTime() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0④↑ -----
#Region "分布図表示処理"
    '''=========================================================================
    ''' <summary>分布図の表示処理</summary>
    ''' <remarks>分布図表示ボタン押下なら分布図を表示する</remarks>
    '''=========================================================================
    Public Sub DisplayDistribute()
        Dim strMsg As String

        Try
            ' ｸﾞﾗﾌ更新
            'If (bFgfrmDistribution) Then    ' 生産ｸﾞﾗﾌ表示ﾌﾗｸﾞON ?
            If (Form1.chkDistributeOnOff.Checked = True) Then    ' 生産ｸﾞﾗﾌ表示ﾌﾗｸﾞON ?
                gObjFrmDistribute.RedrawGraph()
            End If
            System.Windows.Forms.Application.DoEvents()

        Catch ex As Exception
            strMsg = "basTrimming.DisplayDistribute() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub

#End Region

#Region "開始ログ表示処理"
    '''=========================================================================
    ''' <summary>開始ログ表示</summary>
    ''' <param name="plateNoX"></param>
    ''' <param name="plateNoY"></param>
    ''' <param name="StageGrpNoX"></param>
    ''' <param name="StageGrpNoY"></param>
    ''' <param name="blockNoX"></param>
    ''' <param name="blockNoY"></param>
    ''' <remarks>処理開始時の実行箇所の情報を表示する。</remarks>
    '''=========================================================================
    Public Sub DisplayStartLog(ByVal plateNoX As Integer, ByVal plateNoY As Integer,
                ByVal StageGrpNoX As Integer, ByVal StageGrpNoY As Integer,
                ByVal blockNoX As Integer, ByVal blockNoY As Integer)
        Dim bDispLogWrite As Boolean
        Dim strLOG As String
        Dim strMsg As String
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        'Dim TotalBlk As Integer                                         ' V4.0.0.0⑲


        Try
            SimpleTrimmer.TrimData.SetBlockXYNumber(blockNoX, blockNoY)       'V2.0.0.0⑩
            bDispLogWrite = False

            ' ﾃﾞｼﾞSWの2桁目をﾁｪｯｸする。
            Call Form1.GetMoveMode(digL, digH, digSW)
            Select Case digH
                'Select Case gDigH
                'Case 0, 1
                Case 0                                          '###217
                    'Case 0, 1
                    '    ' ﾃﾞｼﾞSWの1桁目をﾁｪｯｸする。
                    '    Select Case digL
                    '        'Select Case gDigL
                    '        Case 0, 1, 2
                    '            ' 表示しない
                    '            Exit Sub
                    '        Case Else
                    '            ' ログ画面に文字列を表示する
                    '            bDispLogWrite = True
                    '    End Select
                Case Else
                    ' ログ画面に文字列を表示する
                    bDispLogWrite = True
            End Select

            ' ログ出力文字列の構築と出力
            If (bDispLogWrite = True) Then
                ''V4.4.0.0②↓
                ''----- V4.0.0.0⑲↓ -----
                '' 自動運転時の最終ブロック時は" ■ Last Block ■"を表示する(サイクル停止のため)
                '' ※ローム殿特注　SL436RのみでSL436Sはやらない
                'If (gSysPrm.stCTM.giSPECIAL = customROHM) And (giMachineKd <> MACHINE_KD_RS) Then
                '    TotalBlk = typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir
                '    If (bFgAutoMode = True) And (TotalBlk <= (blockNoX * blockNoY)) Then
                '        strLOG = "■■■ Last Block ■■■"
                '        Call Form1.Z_PRINT(strLOG)
                '    End If
                'End If
                ''----- V4.0.0.0⑲↑ -----
                ''V4.4.0.0②↑
                ''V4.4.0.0②↓
                'V4.0.0.0-63 ↓
                If (giMachineKd <> MACHINE_KD_RS) Then
                    strLOG = "--- Plate X=" + plateNoX.ToString("000") + " Y=" + plateNoY.ToString("000")
                    strLOG = strLOG + "--- StageGroup X=" & StageGrpNoX.ToString("000") + " Y=" & StageGrpNoY.ToString("000")

                    '　'V4.5.0.0② ステップ＆リピートなしの場合には、ブロック番号は常に１として表示する 
                    If (typPlateInfo.intDirStepRepeat = STEP_RPT_NON) Then
                        strLOG = strLOG + " Block X=" + "001" + " Y=" + "001" + " CntX=" + blockNoX.ToString("000") + " CntY=" + blockNoY.ToString("000")
                    Else
                        strLOG = strLOG + " Block X=" + blockNoX.ToString("000") + " Y=" + blockNoY.ToString("000")
                    End If
                    '　'V4.5.0.0② ステップ＆リピートなしの場合には、ブロック番号は常に１として表示する 

                    ''''処理の検討が必要。
                    'If StepDir = 1 Then
                    '    strLOG = strLOG & " Block X=" & (LogBlkData + XY(1) + 1).ToString("000") & " Y=" & (XY(2) + 1).ToString("000") '(TXT仕様変更)
                    'Else
                    '    strLOG = strLOG & " Block X=" & (XY(1) + 1).ToString("000") & " Y=" & (LogBlkData + XY(2) + 1).ToString("000") '(TXT仕様変更)
                    'End If
                    'strLOG = strLOG & vbCrLf
                    '#4.12.2.0④                    strLOG = strLOG

                    ' ログ画面に文字列を表示する
                    Call Form1.Z_PRINT(strLOG)
                End If
                'V4.0.0.0-63 ↑
                ''V4.4.0.0②↑
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "basTrimming.DisplayStartLog() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "デバッグ用チェック関数"
    '''=========================================================================
    '''<summary>デバッグ時に実行する関数。何を確認したいかは不明。</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub DebugCheck()
#If cOFFLINEcDEBUG Then
        Dim i As Integer
        Dim strMsg As Integer

        Try
            For i = 1 To MaxCntResist - 1                       ' 1-999
                gwTrimResult(i) = 1
                ' (抵抗ﾃﾞｰﾀ)目標値指定 = 絶対値であるかﾁｪｯｸする。
                If typResistorInfoArray(i).intTargetValType = TARGET_TYPE_ABSOLUTE Then
                    ' IT 抵抗値を設定する。
                    gfInitialTest(i) = typResistorInfoArray(i).dblTrimTargetVal * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                    ' FT抵抗値を設定する。
                    gfFinalTest(i) = typResistorInfoArray(i).dblTrimTargetVal * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                Else
                    gfInitialTest(i) = 1000.0# * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                    gfFinalTest(i) = 1000.0# * (1.0# + (Rnd() - 0.5) * 0.001 / 100.0#)
                End If
            Next
        Catch ex As Exception
            strMsg = "basTrimming.DebugCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
#End If
    End Sub
#End Region

#Region "NG判定閾値の算出"
    '''=========================================================================
    ''' <summary>NG判定閾値の算出</summary>
    ''' <param name="ngJudgeUnit">       (OUT)NG判定単位(0:BLOCK, 1:PLATE)</param>
    ''' <param name="ngJudgeResCntInBlk">(OUT)NG判定を実施する抵抗数(ブロック単位でのNG判定)</param>
    ''' <param name="ngJudgeCntInPlt">   (OUT)NG判定を実施する抵抗数(プレート単位でのNG判定)</param>
    '''=========================================================================
    Public Sub CalcNgJudegeResCnt(ByRef ngJudgeUnit As Integer, ByRef ngJudgeResCntInBlk As Integer, ByRef ngJudgeCntInPlt As Integer)

        Dim iNgJudgeRate As Integer
        Dim iResistor As Integer
        Dim i As Integer

        '-----------------------------------------------------------
        '   NG判定閾値の算出
        '-----------------------------------------------------------
        With typPlateInfo
            ngJudgeUnit = .intNgJudgeUnit           ' NG判定単位取得(0:BLOCK, 1:PLATE)
            'ngJudgeUnit = ngJudgeUnit <> 0         '←比較演算の意味が不明。。。 ###142
            iNgJudgeRate = .intNgJudgeLevel         ' NG JUDGEMENT RATE 0-100%

            ' １ブロックの抵抗数をカウント
            iResistor = 0

            'V5.0.0.6⑤↓
            'If (gTkyKnd = KND_TKY)  Then                                   'V6.0.1.0⑮
            If (gTkyKnd = KND_TKY) AndAlso giNGCountInPlate = 0 Then        'V6.0.1.0⑮
                iResistor = .intCircuitCntInBlock
            ElseIf (gTkyKnd = KND_NET) Then
                iResistor = .intGroupCntInBlockXBp
            Else
                'V5.0.0.6⑤↑
                ' 抵抗数分処理を行なう。(TKY_CHIP)
                For i = 1 To gRegistorCnt
                    ' 抵抗番号<1000であるかチェック
                    If typResistorInfoArray(i).intResNo < 1000 Then
                        ' １ブロックの抵抗数のカウント
                        iResistor = iResistor + 1
                    End If
                Next
            End If
            'V5.0.0.6⑤↑

            ' ブロック単位でのNG判定閾値
            ' 「ブロック内の抵抗数xNG判定率」よりNG判定を実施する抵抗数を算出
            ngJudgeResCntInBlk = iResistor * (iNgJudgeRate / 100.0#)

            ' プレート単位でのNG判定閾値
            ' 「１プレートの総抵抗数（１ブロックの抵抗数 × 総ブロック数）ｘNG判定率」より
            ' NG判定実施する抵抗数を算出()
            ' lResistorPlateTotalCount = iResistor * bxy(1) * bxy(2) * (iNgJudgeRate / 100.0#)
            '#4.12.2.0⑦ ngJudgeCntInPlt = iResistor * .intBlockCntXDir * .intBlockCntYDir * (iNgJudgeRate / 100.0#)
            ngJudgeCntInPlt = iResistor * CInt(.intBlockCntXDir) * CInt(.intBlockCntYDir) * (iNgJudgeRate / 100.0#) '#4.12.2.0⑦

            'V6.0.1.0⑮↓
            If giNGCountInPlate <> 0 Then
                ngJudgeResCntInBlk = iNgJudgeRate
                ngJudgeCntInPlt = iNgJudgeRate
            End If
            'V6.0.1.0⑮↑

            '----- 60.1.1.0⑨↓ -----
            ' NG判定基準が%でなくNG数の場合(オプション)
            If (giNgCountAss = 1) Then
                ' NG判定単位がブロック単位の場合  抵抗数 = NG抵抗数
                ngJudgeResCntInBlk = iNgJudgeRate
                '  NG判定単位がプレート単位の場合 抵抗数 = NG抵抗数
                ngJudgeCntInPlt = iNgJudgeRate
            End If
            '----- 60.1.1.0⑨↑ -----
        End With
        '-----------------------------------------------------------
    End Sub
#End Region

#Region "エラーコードにより継続動作可能か判定する"
    '''=========================================================================
    ''' <summary>エラーコードにより継続動作可能か判定する</summary>
    ''' <param name="errCode"></param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function IsAliveErrorCode(ByVal errCode As Integer) As Boolean
        Dim absCode As Integer
        Dim ret As Boolean

        Try
            ret = True
            absCode = System.Math.Abs(errCode)

            Select Case (absCode)
                Case ERR_CMD_NOTSPT, ERR_CMD_PRM, ERR_CMD_LIM_L,
                    ERR_CMD_LIM_U, ERR_RT2WIN_SEND, ERR_RT2WIN_RECV,
                    ERR_WIN2RT_SEND, ERR_WIN2RT_RECV, ERR_SYS_BADPOINTER,
                    ERR_SYS_FREE_MEMORY, ERR_SYS_ALLOC_MEMORY, ERR_CALC_OVERFLOW,
                    ERR_INTIME_NOTMOVE, ERR_BP_MOVE_TIMEOUT, ERR_BP_GRV_ALARM_X, ERR_BP_GRV_ALARM_Y,
                    ERR_BP_GRVMOVE_HARDERR, ERR_LSR_STATUS_STANBY, ERR_LSR_STATUS_OSCERR,
                    ERR_OPN_CVR, ERR_OPN_SCVR, ERR_OPN_CVRLTC
                    ret = False
                Case Else
                    ret = True
            End Select

            Return (ret)
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.IsAliveErrorCode() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (False)
        End Try

    End Function
#End Region

#Region "トリミングブロックのループ処理"
    '''=========================================================================
    ''' <summary>トリミングブロックのループ処理</summary>
    ''' <param name="stgx"></param>
    ''' <param name="stgy"></param>
    ''' <param name="zStepPos"></param>
    ''' <param name="zWaitPos"></param>
    ''' <param name="zOnPos"></param>
    ''' <param name="bpOffX"></param>
    ''' <param name="bpOffY"></param>
    ''' <param name="blkSizeX"></param>
    ''' <param name="blkSizeY"></param>
    ''' <param name="digH"></param>
    ''' <param name="digL"></param>
    ''' <param name="bLoaderNg"></param>
    ''' <param name="strLogDataBuffer"></param>
    ''' <param name="bFgAutoMode"></param>
    ''' <remarks>ブロック単位のトリミングのループ処理
    '''          プレート数・ブロック数分この関数内で処理がループされる。
    ''' </remarks>
    ''' <returns></returns> 
    '''=========================================================================
    Public Function TrimmingBlockLoop(ByVal stgx As Double, ByVal stgy As Double,
                                      ByVal zStepPos As Double, ByVal zWaitPos As Double, ByVal zOnPos As Double,
                                      ByVal bpOffX As Double, ByVal bpOffY As Double, ByVal blkSizeX As Double, ByVal blkSizeY As Double,
                                      ByVal digH As Integer, ByVal digL As Integer,
                                      ByRef bLoaderNg As Boolean, ByVal strLogDataBuffer As StringBuilder, ByVal bFgAutoMode As Boolean) As Integer
        '#4.12.2.0④    Public Function TrimmingBlockLoop(ByVal stgx As Double, ByVal stgy As Double, _
        '#4.12.2.0④                                      ByVal zStepPos As Double, ByVal zWaitPos As Double, ByVal zOnPos As Double, _
        '#4.12.2.0④                                      ByVal bpOffX As Double, ByVal bpOffY As Double, ByVal blkSizeX As Double, ByVal blkSizeY As Double, _
        '#4.12.2.0④                                      ByVal digH As Integer, ByVal digL As Integer, _
        '#4.12.2.0④                                      ByRef bLoaderNg As Boolean, ByRef strLogDataBuffer As String, ByVal bFgAutoMode As Boolean) As Integer

        Dim currentPlateNo As Integer
        Dim currentBlockNo As Integer

        Dim retBlkNoX As Integer
        Dim retBlkNoY As Integer
        Dim dispCurPltNoX As Integer
        Dim dispCurPltNoY As Integer
        Dim dispCurStgGrpNoX As Integer
        Dim dispCurStgGrpNoY As Integer
        Dim dispCurBlkNoX As Integer
        Dim dispCurBlkNoY As Integer
        Dim nextStagePosX As Double
        Dim nextStagePosY As Double
        Dim r As Integer
        Dim ngJudgeUnit As Integer
        Dim ngJudgeResCntInBlk As Integer
        Dim ngJudgeResCntInPlt As Integer
        Dim ngCount As Integer
        Dim iDummy As Integer
        'Dim curMode As Integer
        'Dim blnCheckPlate As Boolean                                   ' 値が何も設定されない。必要性検討  ###093
        Dim bFlgInit As Boolean = True                                  ' ###045
        Dim FlErrCode As Integer
        Dim strMsg As String
        Dim contNgCountError As Integer = 0                             ' 連続HI-NG発生フラグ初期化 ###129
        Dim Sw As Long                                                  ' ###255
        Dim i As Long                                                   ' リトライ回数カウント用 'V2.0.0.0⑭ 
        Dim strLOG As String = ""                                       ' V1.23.0.0⑦ 
        Dim ret1 As Integer    'V6.0.5.0④
        '@@@888 test 時間計測用
        '        Dim StopWatch As New System.Diagnostics.Stopwatch
        '       Dim sw_save(100) As TimeSpan
        'Dim strStopwatch As String '@@@888


        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            r = cFRS_NORMAL
            ngCount = 0
            currentPlateNo = 1
            'V5.0.0.9⑯            currentBlockNo = 1    ↓ GetCurrentBlockNo() で設定する

            dispCurPltNoX = 1
            dispCurPltNoY = 1
            dispCurStgGrpNoX = 1
            dispCurStgGrpNoY = 1
            dispCurBlkNoX = 1
            dispCurBlkNoY = 1
            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号XYを取得する(手動運転時のみ有効)
            Call Form1.Get_StartBlkNum(dispCurBlkNoX, dispCurBlkNoY)
            'V5.0.0.9⑯            currentBlockNo = dispCurBlkNoX * dispCurBlkNoY
            currentBlockNo = GetProcessingOrder(dispCurBlkNoX, dispCurBlkNoY)    'V5.0.0.9⑯
            '----- V4.11.0.0⑤↑ -----

            '----- V1.23.0.0⑥↓ -----
            ' 基板ディレイ(ディレイトリム２)時の最終ブロック番号を設定する
            If (m_blnDelayCheck) Then                                   ' 基板ディレイ ? 
                m_intDelayBlockIndex = 1                                ' 現在のブロック番号(カット番号)初期化
                m_blnDelayLastCut = False                               ' 最終カットフラグOFF
            End If
            If (digL > 2) Then                                          ' 基板ディレイ(ディレイトリム２)は
                m_blnDelayCheck = False                                 ' x0,x1時のみ有効とする
            End If
            '----- V1.23.0.0⑥↑ -----
            TrimmingBlockLoop = cFRS_NORMAL

            '----- ###255↓-----
            ' HALT SW読み込み
            gbHaltSW = False
            bFgCyclStp = False                                          ' サイクル停止フラグOFF V4.0.0.0⑲
            Form1.btnCycleStop.Text = "CYCLE STOP OFF"                          'V5.0.0.4①
            Form1.btnCycleStop.BackColor = System.Drawing.SystemColors.Control  'V5.0.0.4①
            Call HALT_SWCHECK(Sw)
            If (Sw = cSTS_HALTSW_ON) Then
                gbHaltSW = True                                         ' HALT SW状態退避
            End If
            '----- ###255↑-----
            Call ZCONRST()                                              ' コンソールキーラッチ解除 ###042

            '=====================================
            ' NG判定閾値の抵抗数を取得
            '=====================================
            Call CalcNgJudegeResCnt(ngJudgeUnit, ngJudgeResCntInBlk, ngJudgeResCntInPlt)

            ' V4.5.0.5① NG数カウント用  'V4.12.2.0⑨　'V6.0.5.0④
            m_NgCountInPlate = 0

            '-------------------------------------------------------------------
            '   1基板分(ブロック数分)以下を繰り返す(Loop①)
            '-------------------------------------------------------------------
            Do
                '@@@888 StopWatch.Restart()
                'Erase tDelayTrimNgCnt                                   ' ﾃﾞｨﾚｲﾄﾘﾑ2用NGﾁｪｯｸ用配列 V1.23.0.0⑥
                '----- ###142↓ -----
                ' ブロック単位でのNG判定実行時ならNG判定用NG抵抗数を初期化する
                If (ngJudgeUnit = 0) Then                               ' NG判定基準 = ブロック単位 ?
                    '----- V1.23.0.0⑥↓ -----
                    ' 基板ディレイ(ディレイトリム２)時は最終カット時に初期化する
                    If ((m_blnDelayCheck = False) Or ((m_blnDelayCheck = True) And (m_blnDelayLastCut = True))) Then
                        'V6.0.1.0⑮↓
                        'NG数での判定でブロック単位が選択されている場合
                        If giNGCountInPlate = 1 And ngJudgeUnit = 0 Then
                        Else
                            m_NG_RES_Count = 0                              ' NG判定(0-100%)用NG抵抗数(ブロック単位/プレート単位)初期化 
                        End If
                        'V6.0.1.0⑮↑
                    End If
                    'm_NG_RES_Count = 0                                 ' NG判定(0-100%)用NG抵抗数(ブロック単位/プレート単位)初期化 
                    '----- V1.23.0.0⑥↑ -----
                End If
                '----- ###142↑ -----

                '---------------------------------------------------------------
                ' BPオフセット位置へ移動 
                ' ###102(EX_START()の後に移動,BP移動距離が50mmで約6msecかかる為
                '        ステージの移動後のポーズと相殺する)
                '---------------------------------------------------------------
                'r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, 0, 0, 1)
                'If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                '    Return (r)
                'End If

#If cOFFLINEcDEBUG = 0 Then
                ' 非常停止等チェック
                r = Form1.System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' 非常停止等検出 ?
                    TrimmingBlockLoop = r
                    Exit Function
                End If
#End If
                'V1.23.0.0⑥(削除)

                ' エアー圧エラー検出 ?
                If Form1.System1.Air_Valve_Check(gSysPrm) = False Then
                    GoTo STP_ERR_AIR
                End If

                '---------------------------------------------------------------------
                '　対象ブロックのステージ位置を取得
                '---------------------------------------------------------------------
                r = GetTargetStagePos(currentPlateNo, currentBlockNo, nextStagePosX, nextStagePosY,
                                    dispCurPltNoX, dispCurPltNoY, retBlkNoX, retBlkNoY)
                ' 全ブロック終了 ?
                If r = BLOCK_END Then                                   ' ブロック終了 ?
                    '----- V1.23.0.0⑥↓ -----
                    ' 基板ディレイ(ディレイトリム２)時
                    If (m_blnDelayCheck) Then                           ' 基板ディレイ ? 
                        m_intDelayBlockIndex = m_intDelayBlockIndex + 1 ' 現在のブロック番号(カット番号)更新
                        If (m_intDelayBlockIndex >= intGetCutCnt) Then  ' 最終カットなら
                            m_blnDelayLastCut = True                    ' 最終カットフラグON
                        End If
                        If (m_intDelayBlockIndex <= intGetCutCnt) Then  ' カット数分ブロック移動 ? 
                            currentBlockNo = 1                          ' してないなら先頭ブロック 
                            Continue Do                                 ' から再度実行する
                        Else
                            ' 基板ディレイの全ブロック終了時
                            m_blnDelayLastCut = True                    ' 最終カットフラグON
                            m_intDelayBlockIndex = 1                    ' 現在のブロック番号(カット番号)初期化  
                            currentBlockNo = 0                          ' ブロック番号初期化
                            currentPlateNo = currentPlateNo + 1         ' プレート番号更新
                        End If
                    End If
                    '----- V1.23.0.0⑥↑ -----

                    ' プレート単位のNG判定の場合ここで判定を実行する。
                    If (ngJudgeUnit <> 0) Then                          ' NG判定基準 = プレート単位 ?
                        ' NG数 > NG判定を実施する抵抗数(プレート単位でのNG判定)
                        'If (ngCount > ngJudgeResCntInPlt) Then         ' ###142
                        '                        If (m_NG_RES_Count > ngJudgeResCntInPlt) Then   ' ###142
                        ''V6.1.1.0⑪↓                        If ((m_NG_RES_Count > ngJudgeResCntInPlt) And ngJudgeResCntInPlt <> 0) Or
                        If ((m_NG_RES_Count >= ngJudgeResCntInPlt) And ngJudgeResCntInPlt <> 0) Or
                             (((m_NG_RES_Count >= ngJudgeResCntInPlt) And ngJudgeResCntInPlt <> 0) And (giNGCountInPlate = 1)) Then
                            'V6.1.1.0⑪↑
                            bLoaderNg = True                            ' トリミングNGフラグON(ブロック単位/プレート単位でのNG数がNG抵抗数を超えた)
                            '----- V1.18.0.0③↓削除(ここでのカウントはNGブロック数となる) -----
                            '''' CHIPのみ：（090702：minato)
                            'If (gTkyKnd = KND_CHIP) Then
                            '   If gSysPrm.stDEV.rPrnOut_flg = True Then    ' 印刷する ? (ROHM殿仕様)
                            '       stPRT_ROHM.Tol_NG_Sheet = stPRT_ROHM.Tol_NG_Sheet + 1 
                            '   End If
                            'End If
                            '----- V1.18.0.0③↑ -----
                            'V6.0.1.0⑮↓
                            If giNGCountInPlate = 1 Then
                                JudgePlateNGCount(ngJudgeResCntInPlt, m_NG_RES_Count)
                            End If
                            'V6.0.1.0⑮↑

                        End If
                        m_NG_RES_Count = 0                              ' NG判定(0-100%)用NG抵抗数(ブロック単位/プレート単位)初期化 ###142 
                    End If

                    ' 全ブロック終了ならブロック番号を初期化し、次のプレートを読み出し
                    currentBlockNo = 1
                    currentPlateNo = currentPlateNo + 1
                    Continue Do

                    ' プレートもブロックも終了 ?
                ElseIf r = PLATE_BLOCK_END Then
                    'V4.5.0.5①↓'V4.12.2.0⑨　'V6.0.5.0④↓
                    ''V4.12.2.2⑦↓                    ret1 = JudgePlateNGRate()
                    ret1 = JudgePlateNGRate(bFgAutoMode)
                    ''V4.12.2.2⑦↑
                    If (ret1 = cFRS_ERR_START) Then
                        ' ロット続行
                        ' 正常:NG排出してロットは続行 
                        bLoaderNg = True                                ' トリミングNGフラグON(ブロック単位/プレート単位でのNG数がNG抵抗数を超えた)
                    ElseIf (ret1 = cFRS_ERR_RST) Then
                        ' ロット中断RESET処理
                        r = cFRS_ERR_RST
                        ' 異常
                        TrimmingBlockLoop = r
                        Exit Function
                    Else
                        'チェックなしで何もしない場合 

                    End If
                    'V4.5.0.5①↑'V4.12.2.0⑨　'V6.0.5.0④↑
                    ' プレートもブロックも終了した場合は次へ。
                    Exit Do

                ElseIf r <> cFRS_NORMAL Then
                    ' パラメータエラーで返す。
                    TrimmingBlockLoop = r
                    Exit Function
                End If

                ' 伸縮補正用パラメータの設定
                GetShinsyukuData(retBlkNoX, retBlkNoY, nextStagePosX, nextStagePosY)

                '---------------------------------------------------------------------
                ' 表示用各ポジションの番号を設定（プレート/ステージグループ/ブロック）
                '---------------------------------------------------------------------
                Dim bRet As Boolean
                bRet = GetDisplayPosInfo(retBlkNoX, retBlkNoY,
                                dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)

                '===============================================================
                '   ログ表示文字列の設定(見出しの表示)
                '   --- Plate X=xxx Y=xxx--- StageGroup X= xxx Y= xxx Block X=xxx Y=xxx
                '===============================================================
                '----- V1.23.0.0⑥↓ -----
                ' 基板ディレイ(ディレイトリム２)時は最終カット時に表示する
                If ((m_blnDelayCheck = False) Or ((m_blnDelayCheck = True) And (m_blnDelayLastCut = True))) Then
                    Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY,
                                    dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
                Else
                    ' 基板ディレイ(ディレイトリム２)の最終カット以外時はブロック番号とカット番号を表示する

                    If (digH <> 0) Then                                         ' DigSW-H = 表示なし以外 ? 
                        strMsg = "- Block X=" + dispCurBlkNoX.ToString("000") + " Y=" + dispCurBlkNoY.ToString("000") + " Cut =" + m_intDelayBlockIndex.ToString("0")
                        Call Form1.Z_PRINT(strMsg)                              ' ログ画面に表示する 
                    End If

                End If

                System.Windows.Forms.Application.DoEvents()             '###009

                'Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY, _
                '                dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
                '----- V1.23.0.0⑥↑ -----

                '---------------------------------------------------------------------
                ' ステージの移動
                '   EX_SMOVE2ではなく、STARTにて動作させる。
                '   stgx,stgyはステージのオフセットと補正値の加算
                '---------------------------------------------------------------------
                ' ZをSTEP位置へ移動する(START()の中でやっているので初回のみ実行) '###045
                If (bFlgInit = True) Then
                    'bFlgInit = False                                   ' V2.0.0.0_29
                    SETZOFFPOS(-1) 'V6.0.2.0①
                    'r = PROBOFF_EX(zStepPos)                           ' EX_STARTのZOFF位置をzStepPosとする
                    r = PROBOFF_EX(zWaitPos)                            ' EX_STARTのZOFF位置をzWaitPosとする ###058
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then                          ' エラーならエラーリターン(メッセージ表示済み)
                        Return (r)
                    End If

                    ''V6.0.1.022↓ Step&Repeatの速度を転送する。 
                    SetXYStageSpeed(StageSpeed.StepRepeatSpeed)
                    ''V6.0.1.022↑

                End If

                '@@@888 test 時間計測用
                'StopWatch.Restart()

                ' ステージの移動
                r = Form1.System1.EX_START(gSysPrm, stgx + nextStagePosX, stgy + nextStagePosY, 0)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み)
                    Return (r)
                End If

                '@@@888 test 時間計測用
                'StopWatch.Stop()
                'sw_save(currentBlockNo) = StopWatch.Elapsed()

                '----- V2.0.0.0_29↓ -----
                ' ステップ＆リピート時のX,Y軸速度を切り替える(SL436S時)
                'V4.4.0.0⑥
                'If (giMachineKd = MACHINE_KD_RS) And (bFlgInit = True) Then
                If (bFlgInit = True) Then
                    bFlgInit = False

                    If (giMachineKd = MACHINE_KD_RS) Then
                        'V4.1.0.0② ---
                        ' トリミングモードがStep&Repeatのときには、Z軸を動作させない
                        'V5.0.0.6①  If digL <> TRIM_MODE_STEP_AND_REPEAT Then
                        If digL <> TRIM_MODE_STPRPT Then
                            ' ZをSTEP位置へ移動(SL436S時は２段階(STEP位置→ON位置)でONする)
                            r = EX_ZMOVE(zStepPos, MOVE_ABSOLUTE)               ' ZをSTEP位置へ移動 V2.0.0.0⑯
                            If (r <> cFRS_NORMAL) Then                          ' エラー ?(メッセージは表示済み)
                                Return (r)
                            End If
                            Call System.Threading.Thread.Sleep(100)             ' Wait(msec)
                        End If
                        'V4.1.0.0② ---

                        ''V6.0.1.022↓標準にするためSL436Sの条件から出す
                        '' ステップ＆リピート時のX,Y軸速度を切り替える
                        'r = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, 1).FL, stPclAxisPrm(AXIS_X, 1).FH, stPclAxisPrm(AXIS_X, 1).DrvRat, stPclAxisPrm(AXIS_X, 1).Magnif)
                        ''V4.1.0.0⑬ 通信エラー以外おきないのに２回も個別に見る必要はない                   r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならエラーリターン(メッセージ表示済み)
                        ''V4.1.0.0⑬                   If (r <> cFRS_NORMAL) Then Return (r)
                        'r = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, 1).FL, stPclAxisPrm(AXIS_Y, 1).FH, stPclAxisPrm(AXIS_Y, 1).DrvRat, stPclAxisPrm(AXIS_Y, 1).Magnif)
                        'r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならエラーリターン(メッセージ表示済み)
                        'If (r <> cFRS_NORMAL) Then Return (r)
                    End If
                    '----- V2.0.0.0_29↑ -----
                End If

                '---------------------------------------------------------------
                '   プローブチェック処理(x0,x1,x2モード) ※オプション機能 V1.23.0.0⑦
                '---------------------------------------------------------------
                r = gparModules.ProbeCheck(digL, m_PlateCounter, m_ChkCounter)
                If (r <> cFRS_NORMAL) Then                              ' 非常停止等のエラーならエラー戻り(エラーメッセージは表示済み)
                    Return (r)                                          ' プローブチェックエラーならReturn値 = Cancel(RESETキー押下)で戻る 
                End If

                '---------------------------------------------------------------
                '   BPオフセット位置へ移動 ###102
                '---------------------------------------------------------------
                r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, 0, 0, 1)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                    Return (r)
                End If

                '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
                ' トリミング開始ブロック番号XYを更新する
                Call Form1.Set_StartBlkNum(dispCurBlkNoX, dispCurBlkNoY)
                '----- V4.11.0.0⑤↑ -----

                ''----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
                '' 基板投入ボタン押下チェック(自動運転時)
                'If (gbChkSubstrateSet = True) And (bFgAutoMode = True) And (gMachineType = MACHINE_TYPE_436S) Then
                '    ' 基板投入処理
                '    r = SubstrateSet_Proc(Form1.System1)
                '    If (r <> cFRS_NORMAL) Then                          ' エラー ?(メッセージは表示済み)
                '        Return (r)
                '    End If
                'End If
                'If (gbChkSubstrateSet = True) Then
                '    ' 基板投入ボタンの背景色を灰色にする
                '    gbChkSubstrateSet = False                           ' 基板投入フラグOFF
                '    Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
                'End If
                '----- V4.11.0.0⑥↑ -----

                '=======================================================
                ' HALT処理へ変更
                '=======================================================
                '#4.12.2.0⑥                r = HaltSwOnMove(currentPlateNo, currentBlockNo, bFgAutoMode)
                r = HaltSwOnMove(currentPlateNo, currentBlockNo, dispCurPltNoX,
                                 dispCurPltNoY, dispCurBlkNoX, dispCurBlkNoY, bFgAutoMode)  '#4.12.2.0⑥
                ' 正常/STARTキー/RESETキー押下以外はエラーリターンする '###032
                If (r <> cFRS_NORMAL) And (r <> cFRS_ERR_START) Then
                    If (r = cFRS_ERR_RST) Then
                        TrimmingBlockLoop = r
                        Exit Do
                    Else
                        Return (r)
                    End If
                End If

                '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
                ' トリミング開始ブロック番号XYを取得する(手動運転時のみ有効(オプション))
                Dim wkBlkX As Integer = dispCurBlkNoX
                Dim wkBlkY As Integer = dispCurBlkNoY
                Call Form1.Get_StartBlkNum(wkBlkX, wkBlkY)
                ' ブロック番号が変わったかチェックする
                If (wkBlkX <> dispCurBlkNoX) Or (wkBlkY <> dispCurBlkNoY) Then
                    dispCurBlkNoX = wkBlkX
                    dispCurBlkNoY = wkBlkY
                    'V5.0.0.9⑯                    currentBlockNo = dispCurBlkNoX * dispCurBlkNoY
                    currentBlockNo = GetProcessingOrder(dispCurBlkNoX, dispCurBlkNoY)   'V5.0.0.9⑯
                    '　指定ブロックのステージ位置を取得
                    r = GetTargetStagePos(currentPlateNo, currentBlockNo, nextStagePosX, nextStagePosY,
                                        dispCurPltNoX, dispCurPltNoY, retBlkNoX, retBlkNoY)
                    ' ステージの移動
                    r = Form1.System1.EX_START(gSysPrm, stgx + nextStagePosX, stgy + nextStagePosY, 0)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ?(メッセージは表示済み)
                        Return (r)
                    End If
                End If
                '----- V4.11.0.0⑤↑ -----

                '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
                '' 基板投入ボタン押下チェック(自動運転時)
                'If (gbChkSubstrateSet = True) And (bFgAutoMode = True) And (gMachineType = MACHINE_TYPE_436S) Then
                '    ' 基板投入処理
                '    r = SubstrateSet_Proc(Form1.System1)
                '    If (r <> cFRS_NORMAL) Then                          ' エラー ?(メッセージは表示済み)
                '        Return (r)
                '    End If
                'End If
                'If (gbChkSubstrateSet = True) Then
                '    ' 基板投入ボタンの背景色を灰色にする
                '    gbChkSubstrateSet = False                           ' 基板投入フラグOFF
                '    Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
                'End If
                '----- V4.11.0.0⑥↑ -----

                ' デジスイッチの読取り
                Call Form1.GetMoveMode(digL, digH, 0)
                '下でやっているのでここではやらない'V4.1.0.0⑬↓
                'If gKeiTyp = KEY_TYPE_RS Then                                   'V3.1.0.0⑤ シンプルトリマの場合
                '    'タイミング変更
                '    SimpleTrimmer.TrimData.SetBlockNumber(currentBlockNo)    'V2.0.0.0⑩ 現在のブロック番号の保存
                '    ''V4.0.0.0-82        ' 
                '    'BlockNextButton.Enabled = False
                '    'BlockRvsButton.Enabled = False
                '    'BlockMainButton.Enabled = False
                'End If
                '下でやっているのでここではやらない'V4.1.0.0⑬↑
                'V4.0.0.0-82    
                'If r = cFRS_ERR_RST Then'###032
                '    TrimmingBlockLoop = r
                '    Exit Do
                'ElseIf (r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Or (r = cFRS_ERR_LATCH) Or (r = cFRS_ERR_EMG) Then
                '    Return (r)
                'Else
                '    ' デジスイッチの読取り
                '    Call Form1.GetMoveMode(digL, digH, 0)
                'End If

                'V4.12.2.0①             ↓
                If (digL < 3) Then
                    'V6.0.1.3①                    Form1.SetMapColor(currentBlockNo, TrimMap.ColorStart)       ' 加工中ブロックの背景色設定
                    Form1.SetMapColor(currentPlateNo, currentBlockNo, TrimMap.ColorStart)   ' プレート対応    'V6.0.1.3①
                Else
                    'V6.0.1.3①                    Form1.SetMapBorder(currentBlockNo, Color.Black)
                    Form1.SetMapBorder(currentPlateNo, currentBlockNo, Color.Black)         ' プレート対応    'V6.0.1.3①
                End If
                'V4.12.2.0①             ↑

                '=======================================================
                '   ZプローブをON位置に移動
                '=======================================================
                ' ﾃﾞｼﾞSWの1桁目が5以下であるかﾁｪｯｸする。(x0,x1,x2,x3,x4,x5)
                'If (gDigL <= 5) Then
                If (digL <= 3) Then
                    'この呼び方をするとPROP_SETで設定したプローブのON/OFF位置がクリアされる。
                    '原因不明だが、いったんOcxSystemのIF使用を止めて回避させる。
                    'r = Form1.System1.EX_PROBON(gSysPrm)
                    'r = PROBON()
                    'r = EX_ZMOVE(typPlateInfo.dblZOffSet, MOVE_ABSOLUTE)

                    ' バイアスON(FLでx0,x1モード時)　###043 ###017
                    'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And (digL <= 1) Then
                    ' バイアスON(CHIP時でFLでx0,x1モード時)　###043
                    If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And (gTkyKnd = KND_CHIP) And (digL <= 1) Then
                        '----- ###192↓ -----
                        Call QRATE(stCND.Freq(MINCUR_CND_NUM))          ' Qレート設定(KHz)
                        r = FLSET(FLMD_CNDSET, MINCUR_CND_NUM)          ' 加工条件番号設定(最小電流値)
                        If (r <> cFRS_NORMAL) Then                      ' エラー ? (メッセージは表示済み)
                            Return (r)
                        End If
                        '----- ###192↑ -----
                        r = FLSET(FLMD_BIAS_ON, 0)                      ' バイアスON
                        If (r <> cFRS_NORMAL) Then                      ' エラー ? (メッセージは表示済み)
                            Return (r)
                        End If
                    End If

                    ' V2.0.0.0⑭↓
                    glProbeRetryCount = typPlateInfo.intPrbRetryCount   ' プローブリトライ回数(0=リトライなし)
                    For i = 0 To (glProbeRetryCount)

                        '###018 r = EX_ZMOVE(zOnPos, MOVE_ABSOLUTE)
                        r = PROBON()                                        ' ###018
                        r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' ###140

                        ' 'V1.13.0.0④  ↓
                        ' Z2軸を移動する。
                        If IsUnderProbe() Then
                            r = Z2move(Z2ON)                                ' Z2軸動作指令
                            If (r <> cFRS_NORMAL) Then                      ' エラー ? (メッセージは表示済み)
                                Return (r)
                            End If
                            Call ZSTOPSTS2()                                ' Z2軸動作停止待ち
                        End If
                        ' ''V1.13.0.0④  ↑
                        If i < (glProbeRetryCount) Then ' 再プロービングするときには一度上昇させる 
                            ' 'V1.13.0.0④  ↓
                            ' Z2をDOWN位置に移動する
                            If IsUnderProbe() Then                          ' 下方プローブ有りの場合
                                r = Z2move(Z2STEP)                          ' Ｚ２をステップ移動時は下方プローブステップ下降距離だけ下降する。
                                If (r <> cFRS_NORMAL) Then                  ' エラー ? (メッセージは表示済み)
                                    TrimmingBlockLoop = r
                                    Exit Do
                                End If
                                Call ZSTOPSTS2()
                            End If
                            ' 'V1.13.0.0④  ↑

                            ' Zをｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置に移動(上昇位置)
                            r = PROBOFF_EX(zStepPos)                        ' EX_STARTのZOFF位置をzWaitPosとする ###058
                            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                            If (r <> cFRS_NORMAL) Then                      ' エラー ? (メッセージは表示済み)
                                TrimmingBlockLoop = r
                                Exit Do
                            Else
                                'Call LAMP_CTRL(LAMP_Z, False)               ' ZランプOFF
                            End If
                        End If

                    Next i
                    'V2.0.0.0⑭↑

                Else
                    '----- V3.0.0.1①↓ -----
                    ' x5モードの場合はZを指定位置に移動する(0=OFF位置, 1=STEP位置, 2=ON位置)　※オプション
                    If ((digL = 5) And (giFullCutZpos <> 0)) Then
                        If (giFullCutZpos = 1) Then                             ' Z位置指定=STEP位置指定
                            ' ZをSTEP位置へ移動(Z位置指定=STEP位置の場合)
                            r = EX_ZMOVE(zStepPos, MOVE_ABSOLUTE)               ' エラーならメッセージ表示済み
                        Else                                                    ' Z位置指定=ON位置指定
                            ' ZをON位置へ移動(Z位置指定=ON位置の場合)
                            r = PROBON()
                            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならメッセージを表示する
                        End If
                    Else
                        '----- 'V4.5.0.0④↓ -----
                        ' Zを待機位置へ移動する
                        r = EX_ZMOVE(zWaitPos, MOVE_ABSOLUTE)
                        If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                            Return (r)                                              ' Return値設定
                        End If
                        '----- 'V4.5.0.0④↑ -----
                        ' Zを待機位置へ移動(x5モード以外またはx5モードでZ位置指定なしの場合)
                        r = PROBOFF_EX(zWaitPos)                                ' EX_STARTのZOFF位置をzWaitPosとする
                        r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)       ' エラーならメッセージを表示する
                    End If

                    '' x4以上の場合はZを待機位置へ移動する
                    ''r = EX_ZMOVE(typPlateInfo.dblZWaitOffset, MOVE_ABSOLUTE)
                    'r = PROBOFF_EX(zWaitPos)                            ' EX_STARTのZOFF位置をzWaitPosとする ###171
                    'r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならメッセージを表示する ###212
                    '----- V3.0.0.1①↑ -----
                End If
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                End If
                If (digL <= 3) Then
                    Call LAMP_CTRL(LAMP_Z, True)                        ' ZランプON 
                End If

                ''@@@888
                'StopWatch.Stop()
                'sw_save(currentBlockNo) = StopWatch.Elapsed()

                'V4.2.0.0①
                If gKeiTyp = KEY_TYPE_RS Then                                   ' シンプルトリマの場合
                    SetNowBlockDspNum(currentBlockNo + 1)
                End If
                'V4.2.0.0①

                ' V5.0.0.6⑮ ↓
                ' ブロック毎にトリマ停止中を一度ONしてOFFする。ローダでの工程間タイムアウトを避けるための一時的な処理 
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R系 ? 
                    ' ローダ出力(ON=基板要求(連続運転開始), OFF=なし)(SL436R用)
                    If (bFgAutoMode = True) Then                                ' ローダ自動モード? ###001(下記を出力するとクランプ・吸着OFFする)
                        '                            Call SetLoaderIO(LOUT_STOP, &H0)                        ' ローダ出力(ON=トリマ停止中) 
                        '                            Sleep(20)
                        Call SetLoaderIO(&H0, LOUT_STOP)                        ' ローダ出力(OFF=トリマ動作中) 
                    End If
                End If
                ' V5.0.0.6⑮ ↑

                ' ﾃﾞｼﾞSWの1桁目が5以下であるかﾁｪｯｸする。(x0,x1,x2,x3,x4,x5)
                If (digL <= 5) Then
                    ' トリミング結果取得エリアを初期化する。
                    Init_TrimResultData()

                    If IsCutPosCorrect() Then       'V3.0.0.0③ ADD カット位置補正指定が無い場合は処理しない。
                        ' TKYにおいては、カット位置補正を実施する
#If False Then                          'V6.0.0.0⑤
                        If (bIniFlg <> 0) Then
                            End_GazouProc(ObjGazou)
                        End If
#End If
                        Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(False)   'V6.0.0.0-26
                        r = DoCutPosCorrect(digL, gRegistorCnt, stCutPos)
                        Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(True)    'V6.0.0.0-26
#If False Then                          'V6.0.0.0⑤
                        '---------------------------------------------------------------------------
                        '   画像表示プログラムを起動する
                        '---------------------------------------------------------------------------
                        If (bIniFlg <> 0) And (Form1.chkDistributeOnOff.Checked = False) Then
                            ' ↓↓↓ V3.1.0.0② 2014/12/01
                            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                            r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                            ' ↑↑↑ V3.1.0.0② 2014/12/01
                        End If
#End If
                        ''V6.0.1.0⑳↓    
                    Else
                        DoCutPosCorrectClr(gRegistorCnt)
                        ''V6.0.1.0⑳↑
                    End If

                    '----- ###211↓ -----
                    '-----------------------------------------------------------
                    ' ブロック単位のトリミング処理又はポジションチェック処理
                    '-----------------------------------------------------------
                    If gKeiTyp = KEY_TYPE_RS Then                                   'V3.1.0.0⑤ シンプルトリマの場合
                        SimpleTrimmer.BlockStart()                                  'V3.1.0.0⑤ ブロックスタート時処理
                        SimpleTrimmer.TrimData.SetBlockNumber(currentBlockNo)       'V2.0.0.0⑩ 現在のブロック番号の保存
                        BlockNoLabel.Text = currentBlockNo                          'V4.0.0.0⑬
                        'SetNowBlockDspNum(currentBlockNo + 1)                           'V4.1.0.0⑱'V4.2.0.0①
                    End If                                                          'V3.1.0.0⑤

                    If (digL <> 4) Then
                        ' ブロック単位のトリミング処理(x0,x1,x2,x3,x5モード時)
                        r = TrimBlockExe(digL, gSysPrm.stDEV.giPower_Cyc, m_intDelayBlockIndex, 0) ' V1.23.0.0⑥
                    Else
                        ' ポジションチェック処理(x4モード時)
                        r = TrimPositionCheck(digH, digL, bpOffX, bpOffY, blkSizeX, blkSizeY)
                        '----- ###232↓(表示はDllTrimClassLiblaryで表示される) -----
                        ' 補正後クロスラインX,Y非表示
                        'V6.0.0.0④                        Form1.CrosLineX.Visible = False
                        'V6.0.0.0④                        Form1.CrosLineY.Visible = False
                        Form1.VideoLibrary1.SetCorrCrossVisible(False)          'V6.0.0.0④
                        '----- ###232↑ -----
                        If (r >= cFRS_NORMAL) Then                      ' RESETキー押下ならReturn値を正常とする
                            r = cFRS_NORMAL
                        Else
                            Return (r)
                        End If
                    End If

                    '----- V4.0.0.0⑲↓ -----
                    ' 自動運転時の最終ブロックでHALT SWを押下された場合は「サイクル停止」処理を
                    ' 行うため「サイクル停止フラグ」をONする
                    'V5.0.0.4①         If (gSysPrm.stCTM.giSPECIAL = customROHM) Then      ' ローム殿特注(SL436R/SL436S) ?
                    If gbCycleStop Or (gSysPrm.stCTM.giSPECIAL = customROHM) Then      ' ローム殿特注(SL436R/SL436S) ?
                        '#4.12.2.0⑦                        If (bFgAutoMode = True) And (currentBlockNo >= (typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir)) Then
                        If (bFgAutoMode = True) AndAlso
                            (currentBlockNo >= (CInt(typPlateInfo.intBlockCntXDir) * CInt(typPlateInfo.intBlockCntYDir))) Then '#4.12.2.0⑦ 
                            Call HALT_SWCHECK(Sw)
                            If (Sw = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Or (gbHaltSW = True) Then
                                bFgCyclStp = True                       ' サイクル停止フラグON
                                Call LAMP_CTRL(LAMP_HALT, True)             ' HALTランプON
                            End If
                            gbHaltSW = False
                            Call ZCONRST()                              ' コンソールキーラッチ解除
                        End If
                    End If
                End If
                '----- V4.0.0.0⑲↑ -----

                '' ブロック単位のトリミング処理(x0,x1,x2,x3,x5)
                'r = TrimBlockExe(digL, gSysPrm.stDEV.giPower_Cyc, 0, 0)
                '----- ###211↑ -----
                If r <> cFRS_NORMAL Then
                    If r = cFRS_ERR_EMG Then            'V1.25.0.5②
                        bEmergencyOccurs = True         'V1.25.0.5②
                    End If                              'V1.25.0.5②
                    If r = 1 Then
                        TrimmingBlockLoop = cGMODE_EMG
                        Exit Do
                    ElseIf r = 3 Then
                        Call LAMP_CTRL(LAMP_HALT, False)            ' HALTランプOFF
                        Call LAMP_CTRL(LAMP_RESET, True)            ' RESETランプON
                        Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMRES, "")
                        Call Form1.Z_PRINT("RESET SW")
                        TrimmingBlockLoop = cFRS_ERR_RST
                        Exit Do
                        '(2011/06/15)
                        '   下記のエラーコードは返ってこない()
                        '   必要ならばINTRIM側を修正
                        'ElseIf r = 4 Then
                        '    Call Form1.Z_PRINT("HOST ERR")
                        '    GoTo EXIT_LOOP
                    Else
                        ' FLエラーならエラー情報をINtime側に送信する(ログ出力用)
                        If ((Math.Abs(r) = ERR_LSR_STATUS_OSCERR) Or (Math.Abs(r) = ERR_LSR_STATUS_OSCERR)) Then
                            FlErrCode = 0
                            Call ReceiveErrInfo(FlErrCode)          ' FL側からエラー情報を受信する
                            If (FlErrCode <> 0) Then
                                Call SET_FL_ERRLOG(FlErrCode)       ' エラー情報をINtime側に送信する
                            End If
                        End If
                        '----- V1.13.0.0⑪↓ -----
                        ' 測定ばらつき検出/オーバロード検出チェック
                        r = CV_OverLoadErrorCheck(r)
                        If (r = cFRS_ERR_RST) Then                  ' 測定ばらつき検出/オーバロード検出で処理打切指定
                            TrimmingBlockLoop = cFRS_ERR_RST
                            Exit Do
                        ElseIf (r = cFRS_ERR_START) Then            ' 測定ばらつき検出/オーバロード検出で処理続行指定
                            GoTo STP_NEXT
                        End If
                        '----- V1.13.0.0⑪↑ -----

                        TrimmingBlockLoop = Math.Abs(r) * -1        ' Return値を－変換 
                        Exit Do
                    End If
                End If

STP_NEXT:       '                                                   ' V1.13.0.0⑪
                '---------------------------------------------------------------
                '   Zをステップ&リピート位置に移動する
                '---------------------------------------------------------------
                ' プレートのチェック・表示更新
                'blnCheckPlate = CheckPlate(blnCheckPlate, digL)    ' ###093

                '----- V3.0.0.1①↓ -----
                'If (digL <= 3) Then
                ' x0,x1,x2,x3モードおよびx5モードのZの位置指定有り(オプション)の場合
                If ((digL <= 3) Or ((digL = 5) And (giFullCutZpos <> 0))) Then
                    '----- V3.0.0.1①↑ -----
                    ' 'V1.13.0.0④  ↓
                    ' Z2をDOWN位置に移動する
                    If IsUnderProbe() Then                          ' 下方プローブ有りの場合
                        r = Z2move(Z2STEP)                          ' Ｚ２をステップ移動時は下方プローブステップ下降距離だけ下降する。
                        If (r <> cFRS_NORMAL) Then                  ' エラー ? (メッセージは表示済み)
                            TrimmingBlockLoop = r
                            Exit Do
                        End If
                        Call ZSTOPSTS2()
                    End If
                    ' 'V1.13.0.0④  ↑

                    ' Zをｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置に移動(上昇位置)
                    r = PROBOFF_EX(zStepPos)                        ' EX_STARTのZOFF位置をzWaitPosとする ###058
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    'r = EX_ZMOVE(zStepPos, MOVE_ABSOLUTE)          ' ###058
                    If (r <> cFRS_NORMAL) Then                      ' エラー ? (メッセージは表示済み)
                        TrimmingBlockLoop = r
                        Exit Do
                    Else
                        Call LAMP_CTRL(LAMP_Z, False)               ' ZランプOFF
                    End If
                End If

                ' 非常停止ﾎﾞﾀﾝの確認
                If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

                '---------------------------------------------------------------
                '   トリミング結果の表示/ログファイルの出力を行う
                '---------------------------------------------------------------
                '----- V1.23.0.0⑥↓ -----
STP_LOG_OUT:
                ' 基板ディレイ(ディレイトリム２)時
                If (m_blnDelayCheck) Then                               ' 基板ディレイ ?
                    If (digL < 2) Then                                  ' x0,x1モード時 
                        ' トリミング結果を取得する →gwTrimResult()
                        Call TrimLoggingResult_Get(RSLTTYP_TRIMJUDGE, 0)

                        ' イニシャル測定値を取得する(第１カット時)
                        If (m_intDelayBlockIndex = 1) Then              ' 第１カット ?
                            ' NGﾁｪｯｸ用構造体の初期化
                            Call SetDefaultData(dispCurBlkNoX, dispCurBlkNoY)
                            ' イニシャル測定値を取得する→gfInitialTest()
                            Call TrimLoggingResult_Get(RSLTTYP_INTIAL_TEST, 0)
                            ' イニシャル測定値と結果をstDelay2.NgAry()に保持しておく
                            Call TrimLogging_NgCont_CHIP(digL, dispCurBlkNoX, dispCurBlkNoY)
                        End If

                        ' 最終ブロック以外はトリミング結果の表示/ログファイルの出力は行わない
                        If (m_blnDelayLastCut = False) Then             ' 最終ブロック以外なら
                            GoTo STP_NEXT_BLOCK                         ' トリミング結果の表示/ログファイル出力しないで次ブロックへ
                        End If
                    Else
                        m_blnDelayCheck = False                         ' x3以上なら基板ディレイは止める
                    End If
                End If
                '----- V1.23.0.0⑥↑ -----

                m_intNgCount = ngCount
                ' 表示ログの更新/出力ログのデータ保存を行なう。
                Call TrimLogging_Main(digH, digL, r, dispCurPltNoX, dispCurPltNoY, dispCurBlkNoX, dispCurBlkNoY,
                                   strLogDataBuffer, contNgCountError)


                'V2.0.0.0⑩ ADD START 抵抗データの表示
                If gKeiTyp = KEY_TYPE_RS Then
                    m_blnElapsedTime = True                             ' 経過時間を表示する V4.11.0.0④
                    Call SimpleTrimmer.ResistorDataDisp(True, 0, 1)
                    SimpleTrimmer.ElapsedTimeUpdate()       'V2.0.0.0⑩ 経過時間の更新(１ブロック終了時)
                End If
                'V2.0.0.0⑩ ADD END 抵抗データの表示

                '----- V6.0.3.0⑦↓ (カットオフ調整機能) -----
                ' 調整ブロック数が０でないときに実行
                If gAdjustCutoffFunction = 1 And (giMachineKd = MACHINE_KD_RS) Then
                    If digL <= TRIM_MODE_TRFT Then
                        If gAdjustCutoffCount = 0 Then
                            '最初の１ブロック目は目標値の保存のみ
                            InitCalCutoff()
                        End If
                        If (stCutOffAdjust.dblAdjustCutOff_Exec <> AdjustStat.ADJUST_ALREADY) And (stCutOffAdjust.dblAdjustCutOff_Exec <> AdjustStat.ADJUST_FINISHED) Then
                            If 0 < typPlateInfo.intAdjustBlockCnt Then
                                '調整カウンタが調整ブロック以下の場合実行
                                If gAdjustCutoffCount < typPlateInfo.intAdjustBlockCnt Then
                                    If stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC Then
                                        ' カットオフの計算
                                        CalcCutOffVal()
                                    End If

                                    gAdjustCutoffCount = gAdjustCutoffCount + 1
                                Else
                                    '----- V6.0.3.0⑳↓ -----
                                    ' 調整回数をオーバーしたが、調整ができていない場合
                                    If (stCutOffAdjust.dblAdjustCutOff_Exec <> AdjustStat.ADJUST_ALREADY) Then
                                        ' 画面表示して再度実行か、ロット終了かを選択させる。
                                        r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_CUTOFF_TURNING) ' START/RESETキー押下待ち画面表示
                                        If r = cFRS_ERR_START Then
                                            ' 再度カットオフ調整を繰り返す 
                                            gAdjustCutoffCount = 0
                                        Else
                                            ' ロット終了 
                                            TrimmingBlockLoop = r
                                            Exit Do
                                        End If
                                    End If
                                    '----- V6.0.3.0⑳↑ -----
                                End If
                            End If
                        End If
                    End If
                End If
                '----- V6.0.3.0⑦↑ -----

                '----- V1.22.0.0④↓ -----
                ' サマリーロギング用データ設定処理(ファイル出力はここではしない)
                Call SummaryLoggingDataSet(stSummaryLog, digH, digL)
                '----- V1.22.0.0④↑ -----

                'V1.14.0.0①
                If (gESLog_flg = True) Then     'V1.14.0.0①
                    Call TrimLoggingResult_ES()
                    Call LoggingWrite_ES()
                End If

                '----- V1.18.0.0⑥↓ -----
                ' IX2ログを出力する(オプション)
                If (digL <= 1) Then                                     ' x0.x1モード ? 
                    Call TrimLoggingResult_Index2()                     ' 測定結果の取得 
                    Call LoggingWrite_Index2()                          ' IX2ログ出力
                End If
                '----- V1.18.0.0⑥↑ -----

                ''表示テキストボックスを最下層に移動
                'Form1.txtLog.ScrollToCaret()

                '-------------------------------------------------------
                '   連続NG-HIGHｴﾗｰのﾁｪｯｸ処理(CHIP/NET時)
                '-------------------------------------------------------
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then
                    If (contNgCountError = CONTINUES_NG_HI) Then        ' 連続NG-HIGHｴﾗｰ発生 ?
                        ' 連続HI-NGエラーメッセージ表示(SL432R/SL436R共通)
                        r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_HING) ' STARTキー押下待ち画面表示

                        ' 連続HI-NGﾁｪｯｸﾊﾞｯﾌｧ初期化 ###181
                        Call ClearNgHiCount()
                        contNgCountError = 0

                        If (r) Then r = cFRS_ERR_RST '                  ' Return値 = 連続NG-HIGHｴエラーならCancel(RESETｷｰ)で返す ###129
                        TrimmingBlockLoop = r                           ' Return値 = 非常停止他  
                        Exit Do
                    End If
                End If

                'V4.9.0.0①↓ 'High,Lowの率が悪くなった場合の停止画面用
                'V4.10.0.0⑩                If gKeiTyp = KEY_TYPE_RS Then                                   ' シンプルトリマの場合
                If (gMachineType = MACHINE_TYPE_436S) Then                      ' シンプルトリマの場合
                    'V4.9.0.0①
                    If giNgStop = 1 Then
                        If (bFgAutoMode = True) And (JudgeNgRate.CheckTimmingBlock = True) Then
                            r = sub_JudgeLotStop()
                            If r = cFRS_ERR_RST Then
                                ' ここは画面で中断がおされたときのみ
                                r = cFRS_NORMAL '                  ' Return値 = 連続NG-HIGHｴエラーならCancel(RESETｷｰ)で返す ###129
                                TrimmingBlockLoop = r                           ' Return値 = 非常停止他  
                                bLoaderNg = True
                                Exit Do
                                'V4.11.0.0⑧↓
                            ElseIf r = cFRS_ERR_LDRTO Then
                                TrimmingBlockLoop = r                           ' Return値 = 非常停止他  
                                Exit Do
                            ElseIf r = cFRS_ERR_LDR Then
                                TrimmingBlockLoop = r                           ' Return値 = 非常停止他  
                                Exit Do
                                'V4.11.0.0⑧↑
                            End If
                        End If
                    End If
                End If
                'V4.9.0.0①↑

                ngCount = m_intNgCount

                'Call Form1.System1.D_DREAD2_EX(iDummy, gDigH, iDummy, gDigL, gDigH)
                Call Form1.GetMoveMode(iDummy, digH, iDummy)
                lOkChip = m_lGoodCount                                   ' OK数
                lNgChip = m_lNgCount                                     ' NG数

                '-------------------------------------------------------
                '   分布図表示処理(CHIP/NET時)
                '-------------------------------------------------------
                'If (gTkyKnd = KND_CHIP) Then
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then     ' ###139
                    'V4.0.0.0⑫↓
                    If gKeiTyp <> KEY_TYPE_RS Then
                        Call DisplayDistribute()
                    Else
                        gObjFrmDistribute.RedrawGraph()
                    End If
                End If

                ' ブロック単位でのNG判定実行
                If (ngJudgeUnit = 0) Then                               ' NG判定基準 = ブロック単位 ?
                    ' NG数 > NG判定を実施する抵抗数(ブロック単位でのNG判定)
                    'If (ngCount > ngJudgeResCntInBlk) Then             ' ###142
                    'イコールがないと１００％のとき入らない If (m_NG_RES_Count > ngJudgeResCntInBlk) Then       ' ###142
                    ''V4.1.0.0⑯                    If (m_NG_RES_Count >= ngJudgeResCntInBlk) Then       ' ###142　'V4.0.0.0-64
                    If ((m_NG_RES_Count >= ngJudgeResCntInBlk) And (ngJudgeResCntInBlk <> 0)) Then  'V4.1.0.0⑯     ' ###142　'V4.0.0.0-64
                        bLoaderNg = True                                ' トリミングNGフラグON(ブロック単位/プレート単位でのNG数がNG抵抗数を超えた)
                        'V6.0.1.0⑮↓
                        If giNGCountInPlate = 1 Then
                            r = JudgePlateNGCount(ngJudgeResCntInBlk, m_NG_RES_Count)
                            r = cFRS_ERR_RST '                             ' Return値 = NG数による停止での中断処理へ
                            TrimmingBlockLoop = r                           ' Return値 = 非常停止他  
                            Exit Do
                        End If
                        'V6.0.1.0⑮↑

                        ' ロット中の不良基板数カウント(CHIPのみ)
                        '----- V1.18.0.0③↓ 削除 -----
                        'If (gTkyKnd = KND_CHIP) Then
                        '   If gSysPrm.stDEV.rPrnOut_flg = True Then    ' 印刷する ? (ROHM殿仕様)
                        '       stPRT_ROHM.Tol_NG_Sheet = stPRT_ROHM.Tol_NG_Sheet + 1 
                        '   End If
                        'End If
                        '----- V1.18.0.0③↑ -----
                    End If
                End If

STP_NEXT_BLOCK:  '                                                       ' V1.23.0.0⑥

                ' V5.0.0.6⑮ ↓
                ' ブロック毎にトリマ停止中を一度ONしてOFFする。ローダでの工程間タイムアウトを避けるための一時的な処理 
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then           ' SL436R系 ? 
                    ' ローダ出力(ON=基板要求(連続運転開始), OFF=なし)(SL436R用)
                    If (bFgAutoMode = True) Then                                ' ローダ自動モード? ###001(下記を出力するとクランプ・吸着OFFする)
                        Call SetLoaderIO(LOUT_STOP, &H0)                        ' ローダ出力(ON=トリマ停止中) 
                        '                        Sleep(20)
                        '                        Call SetLoaderIO(&H0, LOUT_STOP)                        ' ローダ出力(OFF=トリマ動作中) 
                    End If
                End If
                ' V5.0.0.6⑮ ↑

                'StopWatch.Stop()
                'sw_save(currentBlockNo) = StopWatch.Elapsed()

                ' 次ブロックの実行
                currentBlockNo = currentBlockNo + 1
                System.Windows.Forms.Application.DoEvents()             '###009

            Loop

            '時間計測用 @@@888
            'For i = 1 To typPlateInfo.intBlockCntXDir
            '    strStopwatch = CStr(sw_save(i).Milliseconds) + vbCrLf
            '    MakeTmpLogFile("C:\TRIMDATA\LOG\timeCheck.log", strStopwatch)
            'Next

            ''V1.13.0.0④  ↓
            ' Z2を待機位置に移動
            If IsUnderProbe() Then                                      ' 下方プローブ有りの場合
                r = Z2move(Z2OFF)                                       ' Z2をDOWN位置に移動
                If (r <> cFRS_NORMAL) Then                              ' エラーならエラーリターン(メッセージ表示済み)
                    Return (r)
                End If
                Call ZSTOPSTS2()
            End If
            ''V1.13.0.0④  ↑

            '---- ###261↓ -----
            ' Zを待機位置に移動
            r = PROBOFF_EX(zWaitPos)                                    ' EX_STARTのZOFF位置をzWaitPosとする 
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            Else
                ' Zランプの消灯
                Call LAMP_CTRL(LAMP_Z, False)
            End If

            ''If (r <> cFRS_NORMAL) Then
            '' Zを待機位置に移動
            ''r = EX_ZMOVE(typPlateInfo.dblZWaitOffset, MOVE_ABSOLUTE)
            'r = EX_ZMOVE(zWaitPos, MOVE_ABSOLUTE)
            'If (r <> cFRS_NORMAL) Then                                  ' エラー ? (メッセージは表示済み)
            '    Return (r)
            'Else
            '    ' Zランプの消灯
            '    Call LAMP_CTRL(LAMP_Z, False)
            'End If
            '---- ###261↑----

            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' 全ブロック終了はトリミング開始ブロック番号XYを1,1とする
            'V5.0.0.9⑯            If (dispCurBlkNoX >= typPlateInfo.intBlockCntXDir) And (dispCurBlkNoY >= typPlateInfo.intBlockCntYDir) Then
            If (1 = currentBlockNo) Then 'V5.0.0.9⑯
                Call Form1.Set_StartBlkComb1St()
            End If
            '----- V4.11.0.0⑤↑ -----

            Exit Function
            'End If

            '---------------------------------
            ' BPオフセット位置へ移動
            '---------------------------------
            'r = Form1.System1.BPMOVE(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
            '                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
            '                0, 0, 1)
            r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, 0, 0, 1)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? (メッセージは表示済み)
                Return (r)
            End If

            ' ﾃﾞｼﾞSWの1桁目が3以下であるかﾁｪｯｸする。(x0,x1,x2,x3)
            'blnCheckPlate = CheckPlate(blnCheckPlate, gDigL) ###093

            Exit Function

            '---------------------------------------------------------------------
            '　エラー検出時
            '---------------------------------------------------------------------
            ' 非常停止検出時 
STP_EMERGENCY:
            r = Sub_CallFrmRset(Form1.System1, cGMODE_EMG)              ' 非常停止メッセージ表示
            Return (cFRS_ERR_EMG)

            ' エアー圧エラー検出時 
STP_ERR_AIR:
            r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_AIR)          ' エアー圧エラー検出メッセージ表示
            Return (r)                                                  ' Return値 = エアー圧エラー検出他

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "basTrimming.TrimmingBlockLoop() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "何番目に加工するブロックであるかを取得する"
    ''' <summary>何番目に加工するブロックであるかを取得する</summary>
    ''' <param name="blockPosX">ブロック位置Ｘ</param>
    ''' <param name="blockPosY">ブロック位置Ｙ</param>
    ''' <returns>加工する順番</returns>
    ''' <remarks>'V5.0.0.9⑯</remarks>
    Friend Function GetProcessingOrder(ByVal blockPosX As Integer, ByVal blockPosY As Integer) As Integer
        Dim ret As Integer

        Select Case (giStartBlkAss)
            Case 1                      ' 既存Walsin仕様
                ret = blockPosX * blockPosY

            Case 2                      ' 2016研究開発 再開ブロック位置指定
                ' ステップ&(リピート方向)
                With typPlateInfo
                    Select Case .intDirStepRepeat
                        Case STEP_RPT_Y, STEP_RPT_CHIPXSTPY, STEP_RPT_NON
                            If (0 = (blockPosX Mod 2)) Then
                                ret = (.intBlockCntYDir * (blockPosX - 1)) + (.intBlockCntYDir - blockPosY + 1)
                            Else
                                ret = (.intBlockCntYDir * blockPosX) - (.intBlockCntYDir - blockPosY)
                            End If

                        Case STEP_RPT_X, STEP_RPT_CHIPYSTPX
                            If (0 = (blockPosY Mod 2)) Then
                                ret = (.intBlockCntXDir * (blockPosY - 1)) + (.intBlockCntXDir - blockPosX + 1)
                            Else
                                ret = (.intBlockCntXDir * blockPosY) - (.intBlockCntXDir - blockPosX)
                            End If

                        Case Else ' 通らない
                            ret = 1
                    End Select
                End With

            Case Else                   ' 再開ブロック位置指定機能なし、先頭ブロックから開始する
                ret = 1
        End Select

        Return ret

    End Function
#End Region

    '----- ###211↓ -----
#Region "ポジションチェック処理(x4)"
    '''=========================================================================
    ''' <summary>ポジションチェック処理(x4)</summary>
    ''' <param name="digH">    (INP)DigSW High</param>
    ''' <param name="digL">    (INP)DigSW Low</param>
    ''' <param name="bpOffX">  (INP)BPオフセットX</param>
    ''' <param name="bpOffY">  (INP)BPオフセットY</param>
    ''' <param name="blkSizeX">(INP)ブロックサイズX</param>
    ''' <param name="blkSizeY">(INP)ブロックサイズY</param>
    ''' <returns>cFRS_NORMAL    = 正常
    '''          cFRS_ERR_RST   = RESETキー押下
    '''          上記以外=エラー
    ''' </returns>
    ''' <remarks>エラー発生時のメッセージは表示済み</remarks>
    '''=========================================================================
    Public Function TrimPositionCheck(ByVal digH As Integer, ByVal digL As Integer, ByVal bpOffX As Double, ByVal bpOffY As Double, ByVal blkSizeX As Double, ByVal blkSizeY As Double) As Integer

        Dim Rn As Integer = 0
        Dim Cn As Integer = 1                                           ' 第一カット 
        Dim Rtn As Integer = cFRS_NORMAL
        Dim r As Integer
        Dim PosX As Double = 0.0
        Dim PosY As Double = 0.0
        Dim strMSG As String
        Dim bZ As Boolean = False                                       ' ###220 
        Dim xPos As Double = 0.0                                        ' ###232
        Dim yPos As Double = 0.0                                        ' ###232

        Try
            ' 初期処理
            Call LAMP_CTRL(LAMP_START, True)                            ' STARTランプON 
            Call LAMP_CTRL(LAMP_RESET, True)                            ' RESETランプON 
            Call LAMP_CTRL(LAMP_Z, False)                               ' ###220 

            ' 画像表示プログラムを終了する '###232
            'V6.0.0.0⑤            Call End_GazouProc(ObjGazou)

            ' BPのポジションチェックを行う
            For Rn = 1 To gRegistorCnt                                  ' 抵抗数分繰り返す 
                ' 抵抗の第一カット位置XYを取得する
                PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                'V5.0.0.6⑩↓外部カメラカット位置補正量の加算
                If giTeachpointUse = 1 Then
                    If (Not GetCutStartPointAddTeachPoint(Rn, Cn, PosX, PosY)) Then
                        PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                        PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                        Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + Rn.ToString() + "] CUT=[" + Cn.ToString() + "]")
                    End If
                End If
                'V5.0.0.6⑩↑
                '----- ###245 -----
                ' パターン認識結果がOKならずれ量を加算する
                If (gStCutPosCorrData.corrResult(Rn - 1) = 1) Then
                    PosX = PosX + gStCutPosCorrData.corrPosX(Rn - 1)
                    PosY = PosY + gStCutPosCorrData.corrPosY(Rn - 1)
                End If
                '----- ###245 -----

                ' BPを抵抗の第一カット位置に移動する(絶対値移動)
                r = Form1.System1.BPMOVE(gSysPrm, bpOffX, bpOffY, blkSizeX, blkSizeY, PosX, PosY, 1)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                    Rtn = r
                    Exit For
                End If

                '----- ###232↓ -----
                ' 補正クロスラインを表示する
                Call ZGETBPPOS(xPos, yPos)
                ObjCrossLine.CrossLineDispXY(xPos, yPos)
                '----- ###232↑ -----

                ' ログエリアにメッセージ表示
                If (digH = 2) Then                                      ' DigSW High = 全て表示 ? 
                    strMSG = "[Position Check Rn=" + typResistorInfoArray(Rn).intResNo.ToString("0") + "] START SW:NEXT, RESET SW:CANCEL"
                    Call Form1.Z_PRINT(strMSG)
                End If

STP_KEY_WAIT:   '                                                       ' ###220 
                ' START/RESETキー押下待ち(Zキー押下チェックあり)
                r = WaitStartRestKey(cFRS_ERR_START + cFRS_ERR_RST, True)
                If (r = cFRS_ERR_RST) Then                              ' RESETキー押下ならReturn
                    Rtn = cFRS_ERR_RST
                    Exit For
                End If
                If (r < cFRS_NORMAL) Then                               ' エラー ?(メッセージは表示済み) 
                    Rtn = r
                    Exit For
                End If
                '----- ###220↓ -----
                If (r = cFRS_ERR_Z) Then                                ' Zキー押下 ?
                    If (bZ = False) Then                                ' Z OFFならZ ON
                        r = PROBON()
                    Else                                                ' Z ONなら Z OFF
                        r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)     ' EX_STARTのZOFF位置をzWaitPosとする
                    End If
                    ' エラーならメッセージを表示してエラーリターン
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)   ' エラーならメッセージを表示する
                    If (r <> cFRS_NORMAL) Then
                        Return (r)                                      ' エラーリターン 
                    End If

                    ' Zランプの点灯/消灯
                    If (bZ = False) Then
                        Call LAMP_CTRL(LAMP_Z, True)
                        bZ = True
                    Else
                        Call LAMP_CTRL(LAMP_Z, False)
                        bZ = False
                    End If
                    GoTo STP_KEY_WAIT                                   ' START/RESETキー押下待ちへ 
                End If
                '----- ###220↑ -----
            Next Rn                                                     ' STARTキー押下なら次抵抗へ 
#If False Then                          'V6.0.0.0⑤
            '----- ###232↓ -----
            ' 画像表示プログラムを起動する
            If gKeiTyp = KEY_TYPE_RS Then
                ' ↓↓↓ V3.1.0.0② 2014/12/01
                'r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 1)
                ' ↑↑↑ V3.1.0.0② 2014/12/01
            Else

                If (Form1.chkDistributeOnOff.Checked = False) Then          ' 統計画面非表示時に起動する
                    ' ↓↓↓ V3.1.0.0② 2014/12/01
                    'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                    r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                    ' ↑↑↑ V3.1.0.0② 2014/12/01
                End If
                '----- ###232↑ -----
            End If
#End If
            Return (Rtn)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.TrimPositionCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###211↑ -----
#Region "トリミング処理"
    '''=========================================================================
    '''<summary>トリミング処理</summary>
    '''<remarks>ブロック単位の処理はINTRTM側で処理される。
    '''         VB系のループでは、
    '''         プレート、グループ、ブロックのループ処理が実行される。
    '''         実行されるループは、NET,CHIP,TKYで異なる。  </remarks>
    '''<returns>cFRS_NORMAL = 正常, 左記以外 = エラー </returns> 
    '''=========================================================================
    Public Function Trimming() As Short
        '#4.12.2.0④        Dim strLogDataBuffer As String
        Dim strLogDataBuffer As StringBuilder               '#4.12.2.0④
        Dim stgx As Double
        Dim stgy As Double
        Dim r As Integer
        Dim lRet As Long                                                  ' 'V2.0.0.0⑩ Return値
        Dim rtnCode As Short = cFRS_NORMAL                              ' Return値 
        Dim zStepPos As Double                                          ' Z軸ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置
        Dim strLOG As String                                            ' ログ表示文字列
        Dim bLoaderNG As Boolean                                        ' トリミングNGフラグ(ブロック単位/プレート単位でのNG数がNG抵抗数を超えた)

        '----- CHIPで使用 -----
        Dim blnRotTheta As Boolean                                      ' ﾛｯﾄ毎のθ補正有無(True=あり, False=なし)

        ' 共通で名前を再設定
        Dim plate(2) As Short                                           ' プレート
        Dim block(2) As Short                                           ' ブロック

        '----- 自動運転用(SL436R用) -----
        'Dim bFgAutoMode As Boolean = False                              ' ローダ自動モードフラグ ###107
        Dim bFgAutoMode_BK As Boolean                                   ' ローダ自動モードフラグ(退避)
        'Dim bIniFlg As Integer = 0                                     ' 初期フラグ(0=初回, 1= トリミング中, 2=終了) ###156
        Dim bFgLot As Boolean = False                                   ' ロット切替え要求フラグ
        Dim bFgMagagin As Boolean = False                               ' マガジン終了フラグﾞ
        Dim bFgAllMagagin As Boolean = False                            ' 全マガジン終了フラグﾞ
        Dim FileNum As Integer                                          ' ファイルカウンタ 
        Dim strFNAM As String = ""                                      ' 処理中のファイル名
        Dim SBLoader As Short                                           ' 処理中の基板品種番号(-1した値) ###089

        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        Dim strMSG As String
        Dim InterlockSts As Integer                                     ' ###006
        Dim Sw As Long                                                  ' ###006
        Dim blnCheckPlate As Boolean = False                            ' ###093
        Dim tmpLogFileName As String = "tmpLogFilename.Log"
        '----- V2.0.0.0_29↓ -----
        Dim i As Integer
        Dim Axis As Integer
        Dim strSECT As String
        Dim strKEY As String
        '----- V2.0.0.0_29↑ -----
        Dim TPInit As TimeSpan = New TimeSpan(0, 0, 0, 0)               ' V4.11.0.0④
        Dim WkBlkX As Integer = 1                                       ' V4.11.0.0⑤
        Dim WkBlkY As Integer = 1                                       ' V4.11.0.0⑤

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
            bFgAutoMode = False                                         ' V1.24.0.0③
            '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) -----
            StPauseTime.PauseTime = TPInit                              ' 一時停止時間初期化 
            StPauseTime.TotalTime = TPInit
            ' DllTrimClassLibraryに一時停止時間を渡す(オプション)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0④↑ -----
            Call SetTrimStartTime()                                     ' V1.18.0.0③ トリミング開始時間を設定する(ローム殿特注)
            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号を非活性化する
            Call Form1.Set_StartBlkNum_Enabled(False)
            '----- V4.11.0.0⑤↑ -----

            '----- V2.0.0.0_29↓ -----
            'V4.10.0.0⑩            If (gKeiTyp = KEY_TYPE_RS) Then
            'V6.0.1.022            If (gMachineType = MACHINE_TYPE_436S) Then
            ' X,Y軸PCLパラメータ(SL436S用)をシスパラより設定する→速度切り替えを標準とする 
            For Axis = 0 To 1
                strSECT = "PCL_PRM_" + Axis.ToString("0")
                For i = 0 To 1
                    strKEY = "FL_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).FL = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                    strKEY = "FH_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).FH = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                    strKEY = "DRVRAT_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).DrvRat = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                    strKEY = "MAGNIF_" + (i + 1).ToString("000")
                    stPclAxisPrm(Axis, i).Magnif = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "0"))
                Next i
            Next Axis

            '----- 'V4.9.0.0③ V2.0.0.0_29↓ -----
            ' X,Y軸速度を通常速度に切り替える
            Dim r2 As Integer
            r2 = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, 0).FL, stPclAxisPrm(AXIS_X, 0).FH, stPclAxisPrm(AXIS_X, 0).DrvRat, stPclAxisPrm(AXIS_X, 0).Magnif)
            'V4.1.0.0⑬ 通信エラー以外おきないのに２回も個別に見る必要はない          r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' エラーならアプリ強制終了(メッセージ表示済み)
            'V4.1.0.0⑬If (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            r2 = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, 0).FL, stPclAxisPrm(AXIS_Y, 0).FH, stPclAxisPrm(AXIS_Y, 0).DrvRat, stPclAxisPrm(AXIS_Y, 0).Magnif)
            r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' エラーならアプリ強制終了(メッセージ表示済み)
            If (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            '----- 'V4.9.0.0③ V2.0.0.0_29↑ -----
            'V6.0.1.022
            If (gMachineType = MACHINE_TYPE_436S) Then
                SimpleTrimmer.BlockNoBtnVisible(False)           'V4.1.0.0⑱
                '----- 'V4.9.0.0③ V2.0.0.0_29↑ -----

                'V4.6.0.0②↓
                If giTimeLotOnly = 1 Then
                    TrimData.SetLotChange()
                End If
                'V4.6.0.0②↑
                Clear113Bit() 'V4.11.0.0⑧

            End If
            '----- V2.0.0.0_29↑ -----

STP_CONTINUE:  '                                                        ' V1.18.0.0③ 自動運転継続時のエントリポイント
            ' ﾌﾗｸﾞ等初期化
            giTrimErr = 0                                               ' ﾄﾘﾏｰ ｴﾗｰ ﾌﾗｸﾞ初期化
            strLOG = ""
            '#4.12.2.0④            strLogDataBuffer = ""                                       ' ロギングバッファー初期化 
            strLogDataBuffer = New StringBuilder(64)                    ' ロギングバッファー初期化            '#4.12.2.0④
            giErrLoader = 0                                             ' ローダアラーム検出(0:未検出 0以外:エラーコード) ###073
            bIniFlg = 0                                                 ' 初期フラグ(0=初回) ###156
            bFgCyclStp = False                                          ' サイクル停止フラグ V4.0.0.0⑲
            Form1.btnCycleStop.Text = "CYCLE STOP OFF"                          'V5.0.0.4①
            Form1.btnCycleStop.BackColor = System.Drawing.SystemColors.Control  'V5.0.0.4①
            m_PlateCounter = 0                                          ' プローブチェック機能用基板枚数カウンタ V1.23.0.0⑦
            m_ChkCounter = 1                                            ' プローブチェック機能用基板チェックカウンタ V1.23.0.0⑦
            '----- V4.11.0.0③↓ (WALSIN殿SL436S対応) -----
            m_PwrChkCounter = 0                                         ' オートパワーチェック用基板チェックカウンタ
            m_TimeSpan = New TimeSpan(0, 0, 0)                          ' 経過時間(分)
            m_TimeAss = New TimeSpan(0, 0, 0)                           ' 指定時間(分)
            '----- V4.11.0.0③↑ -----
            '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) -----
            StPauseTime.PauseTime = New TimeSpan(0, 0, 0)
            StPauseTime.TotalTime = New TimeSpan(0, 0, 0)
            '----- V4.11.0.0④↑ -----
            bFgMagagin = False                                          'V4.0.0.0-74
            ' NG判定時処理用
            '###130 giNgBoxCounter = 0                                  ' NG排出BOXの収納枚数カウンター初期化           ###089
            m_NG_RES_Count = 0                                          ' ###142 NG判定(0-100%)用NG抵抗数(ブロック単位/プレート単位)初期化
            Call ClearNGTrayCount()
            If gMachineType = MACHINE_TYPE_436R Or gMachineType = MACHINE_TYPE_436S Then    'V4.10.0.0⑨条件文追加
                gProbeCleaningCounter = 0
            End If

            '----- V6.0.3.0⑳↓ (カットオフ調整機能用) -----
            gAdjustCutoffCount = 0                                      ' カットオフ調整用カウンター     
            stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_DISABLE
            Form1.lblCutOff.Visible = False
            '----- V6.0.3.0⑳↑ -----

            m_lTrimResult = cFRS_NORMAL                                 ' 基板単位のトリミング結果(SL436R用) = 正常     ###089
            bLoaderNG = False                                           ' トリミングNGフラグOFF(ブロック単位/プレート単位でのNG数がNG抵抗数を超えた)
            Form1.txtLog.ShortcutsEnabled = False                       ' ###083 右クリックメニューを表示しない 

            gbLastSubstrateSet = False                                  ' V4.11.0.0⑯

            '----- V4.3.0.0②↓ -----
            ' 下記はSL436S時のみ行う
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ?
                'V4.0.0.0-82        ' 
                BlockNextButton.Enabled = False
                BlockRvsButton.Enabled = False
                BlockMainButton.Enabled = False
                'V4.0.0.0-82        ' 
            End If
            '----- V4.3.0.0②↑ -----

            ''V5.0.0.1④↓
            If giNgStop = 1 Then
                btnJudgeEnable(False)
            End If
            ''V5.0.0.1④↑
            ' 自動運転開始前にローダ側に前回送信時の不要ビットをOFFする(SL436Rで自動運転時のみ送信(クランプ及びバキュームOFFする為)) ###118
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gbFgAutoOperation = True) Then
                ' ローダー出力(ON=なし, OFF=基板要求+トリミングＮＧ+基板回収要求+ＮＧ基板排出要求)
                '###148                Call SetLoaderIO(&H0, LOUT_SUPLY + LOUT_TRM_NG + LOUT_REQ_COLECT + LOUT_NG_DISCHRAGE)
                Call SetLoaderIO(&H0, LOUT_SUPLY + LOUT_TRM_NG + LOUT_REQ_COLECT + LOUT_NG_DISCHRAGE + LOUT_PROC_CONTINUE)

            End If
            ' 'V1.13.0.0⑫　↓
            If typPlateInfo.intIDReaderUse = 1 Then
                ' IDリーダ有なら書き込み用のファイルをクリア
                globalLogFileName = gSysPrm.stLOG.gsLoggingDir + tmpLogFileName
                Call ClearTmpLogFile(globalLogFileName)
            End If
            ' 'V1.13.0.0⑫　↑

            '----- V1.14.0.0②↓ -----
            ' INtime側へ固定ATT情報(レーザコマンドで調整した値)を送信する
            lRet = SetFixAttInfo(stPWR_LSR.AttInfoAry(0))
            '            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, lRet, 0)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? (メッセージは表示済み)
                GoTo STP_ERR_EXIT                                       ' アプリ強制終了へ
            End If
            '----- V1.14.0.0②↑ -----

            ' インターロック状態の表示/非表示 ###108
            Call Form1.DispInterLockSts()

            '---------------------------------------------------------------------------
            '   シグナルタワー制御(自動運転中)およびローダ自動運転切替え(SL436R時)
            '---------------------------------------------------------------------------
            Call ClearBefAlarm()                                        ' ローダアラーム情報退避域クリア(SL436R 自動運転用)
            r = Loader_ChangeMode(Form1.System1, bFgAutoMode, MODE_AUTO)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?(※エラーメッセージは表示済み) 
                rtnCode = r                                             ' Return値設定 
                If (r = cFRS_ERR_RST) Then                              ' RESETキー(SL436R 自動運転時) ?
                    GoTo STP_TRIM_END                                   ' 終了処理へ
                ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                    giErrLoader = r                                     ' ローダアラーム検出 ###073
                    GoTo STP_TRIM_END                                   ' 終了処理へ
                ElseIf (r = cFRS_ERR_LDRTO) Then                        ' ローダ通信タイムアウト  ?
                    giErrLoader = r                                     ' ローダアラーム検出 ###073
                    GoTo STP_TRIM_END                                   ' 終了処理へ
                Else                                                    ' その他の異常終了レベルのエラー 
                    GoTo STP_ERR_EXIT                                   ' アプリ強制終了へ
                End If
            End If

            ' ローダ自動モード移行成功かチェックする(SL436R 自動運転モード時)
            bFgAutoMode_BK = bFgAutoMode                                ' ローダ手動/自動モード退避
            If (gbFgAutoOperation) And (bFgAutoMode = False) Then       ' ローダ自動モード移行成功 ？
                ' "ローダー自動運転の起動ができませんでした。" & vbCrLf & "処理を続行しますか？"
                If (vbNo = MsgBox(MSG_TRIM_02 & vbCrLf & MSG_TRIM_03, vbYesNo)) Then
                    rtnCode = cFRS_ERR_TRIM                             ' Return値 = トリマエラー
                    GoTo STP_EXIT                                       ' 終了処理へ
                End If
            End If
            '----- V6.1.1.0③↓(IAM殿オプション) -----
            ' 自動運転開始時間を取得する(SL436R時)
            If ((gMachineType = MACHINE_TYPE_436R) And (giDispEndTime = 1)) Then
                If (bFgAutoMode = True) Then                            ' 自動運転 ? 
                    Call DispTrimStartTime(cStartTime)
                End If
            End If
            '----- V6.1.1.0③↑ -----

            '----- ###178↓ -----
            '-----------------------------------------------------------------------
            '   インターロックステータス監視タイマー開始(自動運転時 SL436R)
            '-----------------------------------------------------------------------
            If (bFgAutoMode = True) Then                                ' 自動運転 ? 
                Form1.TimerInterLockSts.Interval = 300                  ' 監視タイマー値(msec)
                Form1.TimerInterLockSts.Enabled = True                  ' 監視タイマー開始
            End If
            '----- ###178↑ -----

            '---------------------------------------------------------------------------
            '   トリミングを実行する
            '   自動運転に対応するため連続運転登録データファイル数分ループする
            '---------------------------------------------------------------------------
            giAppMode = APP_MODE_TRIM                                   ' アプリモード = トリミング中
            Call LAMP_CTRL(LAMP_START, True)                            ' STARTランプON

            ' 手動運転時はループ回数に1を設定する
            If (gbFgAutoOperation = False) Then                         ' 手動運転ならばファイル数1でループする
                giAutoDataFileNum = 1
            Else
                stPRT_ROHM.bAutoMode = True                             '　自動運転設定(ローム殿特注)　V1.18.0.0③
            End If

            ' 自動運転に対応するため連続運転登録データファイル数分ループする
            For FileNum = 0 To giAutoDataFileNum - 1
                '------------------------------------------------------------------------
                '   ファイルロード処理(SL436R 自動運転時(自動運転以外はロード済み))
                '------------------------------------------------------------------------
                If (gbFgAutoOperation = True) Then                      ' 自動運転モードならファイルロードする
                    Call Form1.Z_CLS()                                  ' データロードでログ画面クリア ###013
                    gDspCounter = 0                                     ' ログ画面表示基板枚数カウンタクリア ###013
                    m_lTrimResult = cFRS_NORMAL                         ' 基板単位のトリミング結果(SL436R用) = 正常 ###089
                    If (strFNAM = gsAutoDataFileFullPath(FileNum)) Then ' 処理中のファイル名が前回と同じならロードしない 
                        GoTo STP_SKIP_FILE_LOAD
                    End If
                    FormLotEnd.Processed = 0                            ' 処理基板枚数クリア             ' V6.0.3.0_21
                    r = Form1.Sub_FileLoad(gsAutoDataFileFullPath(FileNum))
                    If (r <> cFRS_NORMAL) Then                          ' ファイルロードエラー ?(※エラーメッセージは表示済み) 
                        rtnCode = r                                     ' Return値設定 
                        GoTo STP_TRIM_ERR                               ' トリマエラー信号送信へ
                    End If
                    strFNAM = gsAutoDataFileFullPath(FileNum)           ' 処理中のファイル名退避
                    gCmpTrimDataFlg = 0                                 ' データ更新フラグ = 0(更新なし) ###223
                    stPRT_ROHM.bAutoMode = True                         ' 自動運転設定(ローム殿特注)　V1.18.0.0③

                    ' 基板品種番号を退避する ###089
                    If (FileNum <> 0) Then                              ' 最初のデータファイル以外 ? 
                        ' 基板品種番号が変わった場合でNG排出BOXの収納枚数カウンターが0以上の時は
                        ' メッセージを表示する
                        If (SBLoader <> (typPlateInfo.intWorkSetByLoader - 1)) Then
                            If (giNgBoxCounter > 0) Then                ' NG排出BOXの収納枚数カウンター > 0 ?
                                giAppMode = APP_MODE_LDR_ALRM           ' アプリモード = ローダアラーム画面(カバー開のエラーとしない為)
                                Call COVERCHK_ONOFF(COVER_CHECK_OFF)    ' 「固定カバー開チェックなし」にする ###088

                                '----- V1.18.0.1⑧↓ -----
                                ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
                                r = EL_Lock_OnOff(EX_LOK_MD_OFF)
                                If (r = cFRS_TO_EXLOCK) Then            ' 「前面扉ロック解除タイムアウト」なら戻り値を「RESET」にする
                                    rtnCode = cFRS_ERR_RST
                                    GoTo STP_TRIM_END                   ' 終了処理へ
                                End If
                                If (r < cFRS_NORMAL) Then               ' 異常終了レベルのエラー ? 
                                    rtnCode = r
                                    GoTo STP_ERR_EXIT                   ' アプリケーション強制終了
                                End If
                                '----- V1.18.0.1⑧↑ -----

                                ''"基板品種が変わりました","ＮＧ排出ボックスからＮＧ基板を取り除いてから","STARTキーを押すか、OKボタンを押して下さい。" V6.1.1.0⑩
                                ' "基板品種が変わりました","ＮＧ排出ボックスからＮＧ基板を取り除いて","STARTキーを押すか、OKボタンを押して下さい。"     V6.1.1.0⑩
                                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                                        MSG_LOADER_19, MSG_LOADER_20, MSG_frmLimit_07, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                                If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' 非常停止等のエラーならアプリ強制終了へ(エラーメッセージは表示済み) 

                                giAppMode = APP_MODE_TRIM               ' アプリモード = トリミング中
                                Call COVERLATCH_CLEAR()                 ' カバー開ラッチのクリア ###088
                                Call COVERCHK_ONOFF(COVER_CHECK_ON)     ' 「固定カバー開チェックあり」にする ###088
                                '----- V1.18.0.1⑧↓ -----
                                ' 電磁ロック(観音扉右側ロック)する(ローム殿特注)
                                r = EL_Lock_OnOff(EX_LOK_MD_ON)
                                If (r = cFRS_TO_EXLOCK) Then            ' 「前面扉ロックタイムアウト」なら戻り値を「RESET」にする
                                    rtnCode = cFRS_ERR_RST
                                    GoTo STP_TRIM_END                   ' 終了処理へ
                                End If
                                If (r < cFRS_NORMAL) Then               ' 異常終了レベルのエラー ? 
                                    rtnCode = r
                                    GoTo STP_ERR_EXIT                   ' アプリケーション強制終了
                                End If
                                '----- V1.18.0.1⑧↑ -----
                            End If
                            ' 基板品種番号が変わったら「NG排出BOXの収納枚数カウンター」を初期化する
                            '###130 giNgBoxCounter = 0                  ' 「NG排出BOXの収納枚数カウンター」を初期化
                            Call ClearNGTrayCount()
                        End If
                    End If
                    SBLoader = typPlateInfo.intWorkSetByLoader - 1      ' 基板品種番号退避(-1した値)
                End If

                '---------------------------------------------------------------------------
                '   トリミング実行前チェック
                '---------------------------------------------------------------------------
                ' データロードチェック
                If (gLoadDTFlag = False) Then                            ' データ未ロード ?
                    ' "トリミングパラメータデータをロードするか新規作成してください"
                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    rtnCode = cFRS_ERR_TRIM                             ' Return値 = トリマエラー
                    GoTo STP_TRIM_ERR                                   ' トリマエラー信号送信へ
                End If

                ' ＦＬ側に加工条件が設定されているかチェックする(ファイバーレーザ時) 
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then       ' FL(ﾌｧｲﾊﾞｰﾚｰｻﾞ) ?
                    '' シリアルポートからトリマー加工条件を個別に受信する
                    'r = RsReceiveBankChkALL(cTIMEOUT)
                    'If (r <> SerialErrorCode.rRS_OK) Then               ' エラー ? 
                    '    If (r = SerialErrorCode.rRS_FLCND_NONE) Then
                    '        strMSG = MSG_140                            '"ＦＬ側の加工条件の設定がありません。再度データをロードするか、編集画面から加工条件の設定を行ってください。"
                    '    Else
                    '        strMSG = MSG_141                            '"ＦＬ側加工条件のリードに失敗しました。"
                    '    End If
                    '    Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    '    rtnCode = cFRS_ERR_TRIM                         ' Return値 = トリマエラー
                    '    GoTo STP_TRIM_ERR                               ' トリマエラー信号送信へ
                    'End If

                    'r = TrimCondInfoRcv(stCND)                          ' FL側から現在の加工条件を受信する
                    'If (r <> SerialErrorCode.rRS_OK) Then               ' エラー ? 
                    '    If (r = SerialErrorCode.rRS_FLCND_NONE) Then
                    '        strMSG = MSG_140                            '"ＦＬ側の加工条件の設定がありません。再度データをロードするか、編集画面から加工条件の設定を行ってください。"
                    '    Else
                    '        strMSG = MSG_141                            '"ＦＬ側加工条件のリードに失敗しました。"
                    '    End If
                    '    Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    '    rtnCode = cFRS_ERR_TRIM                         ' Return値 = トリマエラー
                    '    GoTo STP_TRIM_ERR                               ' トリマエラー信号送信へ
                    'End If
                End If

STP_SKIP_FILE_LOAD:
                ' デジスイッチの読取り
                Call Form1.GetMoveMode(digL, digH, digSW)
                If (digL > 6) Then
                    ' "動作モード（デジスイッチ）の設定を x0～x6 にしてください"
                    Call Form1.System1.TrmMsgBox(gSysPrm, MSG_25, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    rtnCode = cFRS_ERR_TRIM                             ' Return値 = トリマエラー
                    GoTo STP_TRIM_ERR                                   ' トリマエラー信号送信へ
                End If

                '---------------------------------------------------------------------------
                '   各種パラメータ取得
                '---------------------------------------------------------------------------
                With typPlateInfo
                    ' システム変数設定(プローブON/OFF位置)
                    Call PROP_SET(.dblZOffSet, .dblZWaitOffset,
                            gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, gSysPrm.stDEV.gfSmaxX, gSysPrm.stDEV.gfSmaxY)

                    'この呼び方をすると上で設定したプローブのON/OFF位置がクリアさせる。
                    '原因不明だが、いったんOcxSystemのIF使用を止めて回避させる。
                    'r = Form1.System1.EX_PROBON(gSysPrm)

                    ' NETのみ：2009/07/24 なぜココでセットしているのか時間があったら調査
                    gwCircuitCount = .intCircuitCntInBlock              ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数(TKY) 
                    If (gTkyKnd = KND_NET) Then                         ' ###164 
                        gwCircuitCount = .intGroupCntInBlockXBp         ' ###164 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数(NET) 
                    End If                                              ' ###164 

                    '----- V1.14.0.0⑤↓ -----
                    '' LEDの設定(オプション)
                    'stATLD.iLED = .intLedCtrl                           ' LED制御(TKY-CHIPのみ)
                    'If gSysPrm.stSPF.giLedOnOff = 1 Then
                    '    Call Form1.System1.Set_Led(0, stATLD.iLED, 0)   ' バックライト照明ＯＮ／ＯＦＦ(LED制御)
                    'End If
                    '----- V1.14.0.0⑤↑ -----

                End With ' typPlateInfo

                '----- V1.23.0.0⑥↓ -----
                ' 下記を復活
                '*****************************************************************************
                '(2010/11/09)　DelayTrim2は一時的にコメントアウト
                '*****************************************************************************
                '==================================
                ' CHIPのみ：（090414：minato)
                '==================================
                If (gTkyKnd = KND_CHIP) Then
                    ' ﾃﾞｨﾚｲﾄﾘﾑﾁｪｯｸﾌﾗｸﾞを初期化する。
                    m_blnDelayCheck = False
                    m_blnDelayLastCut = False
                    m_blnDelayFirstCut = False
                    intGetCutCnt = 1

                    ' ﾃﾞｨﾚｲﾄﾘﾑ2実行であるかﾁｪｯｸする。
                    If (typPlateInfo.intDelayTrim = 2) Then
                        ' ｶｯﾄ数が全て同じであるか/ﾚｼｵﾓｰﾄﾞが存在しないかﾁｪｯｸする。
                        m_blnDelayCheck = DelayTrimCheck(intGetCutCnt)
                        ' ﾃﾞｨﾚｲﾄﾘﾑ2実行ﾁｪｯｸNGの場合にはｶｯﾄ数を1に設定する。
                        If Not m_blnDelayCheck Then
                            intGetCutCnt = 1
                        End If
                    Else
                        m_blnDelayLastCut = True
                    End If

                    ' ﾃﾞｨﾚｲﾄﾘﾑ2用構造体の初期化
                    Init_stDelay2(typPlateInfo.intBlockCntXDir, typPlateInfo.intBlockCntYDir, gRegistorCnt)
                End If
                '----- V1.23.0.0⑥↑ -----

COVER_CLOSE_RETRY:  'V4.7.0.0⑰
                '-----------------------------------------------------------------------
                '   スライドカバーをクローズする(手動/自動) (SL432R時)
                '-----------------------------------------------------------------------
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then   ' SL432R系 ? 
                    If (gSysPrm.stSPF.giWithStartSw = 1) And (giHostMode <> cHOSTcMODEcAUTO) Then
                        ' スライドカバー自動クローズしない (ｽﾀｰﾄSW押下待ち(オプション) でローダ自動運転中でない場合)
                        r = Form1.System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, True)
                        Form1.Refresh()                 'V1.13.0.0⑬
                        If (r = cFRS_ERR_START) Then r = cFRS_NORMAL ' START SW押下ならReturn値 = 正常

                    Else
                        'V4.7.0.0⑰ 吸着エラーとクランプエラーを上位で処理する為giAppModeからiAppModeへ変更↓
                        Dim iAppMode As Short
                        If giHostMode = cHOSTcMODEcAUTO And giAppMode = APP_MODE_TRIM Then
                            iAppMode = APP_MODE_TRIM_AUTO
                        Else
                            iAppMode = giAppMode
                        End If
                        'V4.7.0.0⑰ 吸着エラーとクランプエラーを上位で処理する為↑
                        ' スライドカバーを自動クローズする
                        ' XY_SLIDE同時動作 ? (0:OFFLINE, 1:ONLINE, 2:SLIDE COVER+XY移動)
                        If gSysPrm.stTMN.giOnline = TYPE_MANUAL Then        ' XY_SLIDE同時動作 
                            r = Form1.System1.Z_CCLOSE(gSysPrm, giAppMode, 0, True, gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY) '###134
                        End If
                        If gSysPrm.stTMN.giOnline = TYPE_ONLINE Then        ' XY_SLIDE通常動作
                            r = Form1.System1.Z_CCLOSE(gSysPrm, giAppMode, 0, False, stgx, stgy)
                        End If
                        'V4.7.0.0⑰ 吸着エラーとクランプエラーを上位で処理する為giAppModeからiAppModeへ変更↓
                        If (r = cFRS_TO_CLAMP_ON) Or (r = cFRS_VACCUME_ERROR) Then
                            'V5.0.0.9⑭ ↓
                            ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF) ' 標準((赤点滅+ブザー１))
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                            'V5.0.0.9⑭ ↑

                            strMSG = "エラー"
                            If (r = cFRS_TO_CLAMP_ON) Then
                                strMSG = "クランプを確認してください"        'クランプエラー
                            ElseIf (r = cFRS_VACCUME_ERROR) Then
                                strMSG = "吸着を確認してください"          ' 吸着エラー
                            End If
                            r = Form1.System1.Form_MsgDispStartReset(strMSG, "RESETキーで終了します。", &HFF, &HFF0000)
                            Call ZCONRST()                                                  ' ラッチ解除

                            'V5.0.0.9⑭ ↓
                            ' Call Form1.System1.SetSignalTower(0, &HFFFF)   ' シグナルタワー制御初期化(On=0, Off=全ﾋﾞｯﾄｵﾌ)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                            'V5.0.0.9⑭ ↑

                            If (r = cFRS_ERR_START) Then
                                GoTo COVER_CLOSE_RETRY
                            ElseIf (r = cFRS_ERR_RST) Then
                                'frmAutoObj.SetAutoOpeCancel()
                                GoTo STP_TRIM_END
                            Else
                                GoTo STP_ERR_EXIT                               ' 非常停止等のエラーならアプリ強制終了へ
                            End If
                        End If
                        If giHostMode = cHOSTcMODEcAUTO Then
                            'V5.0.0.9⑭ ↓
                            ' Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, &HFFFF)    ' シグナルタワー制御(On=自動運転中(緑点灯),Off=全ﾋﾞｯﾄ)
                            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                            'V5.0.0.9⑭ ↑

                        End If
                        'V4.7.0.0⑰ 吸着エラーとクランプエラーを上位で処理する為↑

                    End If
                Else
                    ' クランプ及びバキュームON(SL436R時) '###001
                    If (bFgAutoMode = False) Then                   ' 自動運転時はクランプ及びバキュームONしない ###107
                        ' 追加したけど、これやると遅く感じるので元に戻す   System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)     'V5.0.0.8⑥
                        r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 1, giAppMode, giTrimErr)
                    End If
                End If

                If (r = cFRS_ERR_RST) Then                          ' Cancel(RESETｷｰ) ?
                    rtnCode = cFRS_ERR_RST                          ' Return値 = Cancel(RESETｷｰ)
                    GoTo STP_TRIM_END                               ' 終了処理へ
                ElseIf ((r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Or (r = cFRS_ERR_LATCH)) Then
                    GoTo STP_TRIM_END                               ' カバー開エラーなら終了処理へ
                ElseIf (r <> cFRS_NORMAL) Then                      ' エラー ?(※エラーメッセージは表示済み) 
                    GoTo STP_ERR_EXIT                               ' 非常停止等のエラーならアプリ強制終了へ
                End If

                'V5.0.0.6①↓
                r = basTrimming.CheckControllerInterlock()       ' 外部コントローラチェック（内部でシスパラ機能有無確認）
                If (r = (cFRS_ERR_EMG)) Then
                    GoTo STP_ERR_EXIT                               ' 非常停止等のエラーならアプリ強制終了へ
                ElseIf (r = cFRS_ERR_RST) Then
                    rtnCode = cFRS_ERR_RST                          ' Return値 = Cancel(RESETｷｰ)
                    GoTo STP_TRIM_END                               ' 終了処理へ
                End If
                'V5.0.0.6①↑

                ' ログ画面表示クリア ###013
                gDspCounter = gDspCounter + 1                       ' ログ画面表示基板枚数カウンタ更新
                '#4.12.2.0④                If (gDspCounter > gDspClsCount) Then                ' カウンタ > ログ画面表示クリア基板枚数
                If (gDspCounter > gDspClsCount) OrElse (Form1.TxtLogLengthLimit()) Then '#4.12.2.0④
                    ' カウンタ > ログ画面表示クリア基板枚数 OrElse 制限文字数以上
                    Call Form1.Z_CLS()                              ' ログ画面クリア
                    gDspCounter = 1                                 ' ログ画面表示基板枚数カウンタ再設定
                End If
                Form1.ShowSelectedMap(False)                        ' 選択済み加工対象ブロックを変更不可で表示する 'V4.12.2.0②
                'V6.0.1.0⑪
                Form1.SetMapOnOffButtonEnabled(False)               ' MAP ON/OFF ボタンを無効にする              'V4.12.2.0①

                'V6.0.1.0⑰↓
                ' ＭＡＰボタンの選択状態によって、表示を切り替える 
                '                Form1.SetTrimMapVisible(True)                           'V6.0.1.0⑪
                Form1.SetTrimMapVisible(Form1.MapOn)                'V6.0.1.0⑪
                'V6.0.1.0⑰↑
                Form1.MoveHistoryDataLocation(True)                     'V6.0.1.0⑪
                '---------------------------------------------------------------------------
                '   自動レーザパワー調整実行
                '---------------------------------------------------------------------------
                'V5.0.0.4⑦         If (digL < 2) Or (digL = 5) Then                        ' ﾄﾘﾐﾝｸﾞﾓｰﾄﾞ = x0,x1,x5 ?
                If (digL < 2) Or (digL = 5) Or (digL = 2 And typPlateInfo.intNGMark <> 0) Then     ' ﾄﾘﾐﾝｸﾞﾓｰﾄﾞ = x0,x1,x5 ?
                    bIniPwrFlg = True                                   ' V4.11.0.0③
                    r = AutoLaserPowerADJ(MD_INI)                       ' レーザパワー調整実行　V4.11.0.0③
                    If (r = cFRS_ERR_RST) Then                          ' Cancel(RESETｷｰ) ?
                        rtnCode = cFRS_ERR_RST                          ' Return値 = Cancel(RESETｷｰ)
                        GoTo STP_TRIM_END                               ' 終了処理へ 
                    ElseIf (r <> cFRS_NORMAL) Then                      ' エラー ?(※エラーメッセージは表示済み) 
                        GoTo STP_ERR_EXIT                               ' 非常停止等のエラーならアプリ強制終了へ
                    End If
                End If
                '----- V4.11.0.0③↓ (WALSIN殿SL436S対応) -----
                ' 「時間(分)指定」機能ありで時間(分)指定」あり ?
                If (giPwrChkTime = 1) And (typPlateInfo.intPwrChkTime <> 0) Then
                    m_TimeSTart = DateTime.Now
                    m_TimeAss = New TimeSpan(0, typPlateInfo.intPwrChkTime, 0)
                End If
                '----- V4.11.0.0③↑ -----

                '---------------------------------------------------------------------------
                '   トリミング前処理 (SL436R系時)
                '    品種データ送信(STB)
                '    (チップサイズXY, 連続トリミングＮＧ枚数上限,トリミングＮＧカウンタ(上限)
                '    割欠け排出カウンタ(上限)ローダに送信する※現状処理なし)
                '---------------------------------------------------------------------------
                r = Loader_TrimPreStart(Form1.System1, bFgAutoMode, typPlateInfo.dblChipSizeXDir, typPlateInfo.dblChipSizeYDir,
                                             typPlateInfo.intTrimNgCount, typPlateInfo.intMaxTrimNgCount, typPlateInfo.intMaxBreakDischargeCount)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(※エラーメッセージは表示済み) 
                    GoTo STP_ERR_EXIT                                   ' 非常停止等のエラーならアプリ強制終了へ
                End If

                '----- V6.0.3.0_31↓ -----
                If (gbFgAutoOperation = True) Then
                    gbAutoOperating = True                              ' 自動運転中印刷不可フラグ設定　
                End If
                '----- V6.0.3.0_31↑ -----

                '---------------------------------------------------------------
                '   ローダへ基板要求(基板投入/交換要求信号)を送信し、
                '   ローダからのトリミングスタート信号を待つ(SL436R系時)
                '---------------------------------------------------------------
                Do
                    '----- V2.0.0.0⑫↓ -----
                    If gKeiTyp = KEY_TYPE_RS Then
                        ' θを原点に戻す(SL436R/SL436S時) ※補正モード=1(手動),補正方法=1(1回のみ)時は前回の位置へ回転させる必要あり(未実装)
                        If (bFgAutoMode = True) Then
                            If (gSysPrm.stDEV.giTheta <> 0) Then        ' θあり ?
                                Call ROUND4(0.0)
                            End If
                        End If
                    End If
                    '----- V2.0.0.0⑫↑ -----

                    ' ローダへ基板要求(基板投入/交換要求信号)を送信し、トリミングスタート信号を待つ(SL436R系時)
                    r = Loader_WaitTrimStart(Form1.System1, bFgAutoMode, m_lTrimResult, bFgMagagin, bFgAllMagagin, bFgLot, bIniFlg)
                    'V4.9.0.0①↓
                    If (gMachineType = MACHINE_TYPE_436S) Then
                        ' W113.02をOFFする
                        SetLotStopBit(0)
                    End If
                    'V4.9.0.0①↑
                    Call Form1.System1.AutoLoaderFlgReset()                     ' オートローダ　フラグリセット ###099
                    If (r <> cFRS_NORMAL) Then                                  ' エラー ?(※エラーメッセージは表示済み) 
                        rtnCode = r                                             ' Return値設定 
                        'V5.0.0.9⑭ ↓ V6.0.3.0⑧ 停止移行状態（自動運転でアラームメッセージでOK押してから停止状態に移行するまでの間)
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_WAIT_IDLE)
                        'V5.0.0.9⑭ ↑ V6.0.3.0⑧
                        If (r = cFRS_ERR_RST) Then                              ' RESETキー(SL436R 自動運転時) ?
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                            GoTo STP_TRIM_END                                   ' 終了処理へ
                        ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                            '                                                   ' ローダアラーム検出(SL436R 自動運転時) ? ###073
                            giErrLoader = r                                     ' ローダアラーム検出 ###073
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                            Call W_RESET()                                      ' アラームリセット 
                            GoTo STP_TRIM_END                                   ' 終了処理へ
                        ElseIf (r = cFRS_ERR_LDRTO) Then                        ' ローダ通信タイムアウト  ?
                            giErrLoader = r                                     ' ローダアラーム検出 ###073
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                            GoTo STP_TRIM_END                                   ' 終了処理へ
                        ElseIf (r = cFRS_ERR_LOTEND) Then                       ' LotEnd 
                            '----- V4.0.0.0⑲↓ -----
                            'V2.0.0.0_21　↓
                            ' giErrLoader = r                                   ' ローダアラーム検出 V1.16.0.0⑨
                            giErrLoader = cFRS_NORMAL                           ' ロット終了は正常とする V1.16.0.0⑨
                            rtnCode = cFRS_NORMAL
                            '----- V4.0.0.0⑲↑ -----
                            bFgAllMagagin = True
                            'V2.0.0.0_21　↑
                            Call SetLoaderIO(&H0, LOUT_SUPLY)                   ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                            GoTo STP_TRIM_END                                   ' 終了処理へ
                        Else                                                    ' その他の異常終了レベルのエラー 
                            GoTo STP_ERR_EXIT                                   ' アプリ強制終了へ
                        End If
                    End If

                    '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
                    bAllMagazineFinFlag = bFgMagagin
                    ' 最終基板時は「基板投入ボタン」を押せないようにする(自動運転時)
                    If (gKeiTyp = KEY_TYPE_RS) And (bFgAutoMode = True) Then
                        Clear113Bit() 'V4.11.0.0⑧

                        If (bFgMagagin) Then
                            Form1.BtnSubstrateSet.Enabled = False       '「基板投入ボタン」非活性化
                            gbChkSubstrateSet = False                   ' 基板投入フラグOFF
                            Call Form1.Set_SubstrateSetBtn(gbChkSubstrateSet)
                            gbLastSubstrateSet = True                                   ' V4.11.0.0⑯

                        Else
                            'V4.11.0.0⑯ Form1.BtnSubstrateSet.Enabled = True     'V4.11.0.0⑯   '「基板投入ボタン」活性化
                            Form1.BtnSubstrateSet.Enabled = False     'V4.11.0.0⑯   '「基板投入ボタン」活性化
                        End If
                    End If
                    '----- V4.11.0.0⑥↑ -----

                    ' プローブクリーニングカウンタ更新
                    'V6.0.0.1④                    If (typPlateInfo.intPrbCleanAutoSubCount <> 0) Then
                    If (typPlateInfo.intPrbCleanAutoSubCount <> 0) AndAlso (1 <= Form1.Instance.stFNC(F_PROBE_CLEANING).iDEF) Then 'V6.0.0.1④
                        If (digL < 4) Then
                            gProbeCleaningCounter = gProbeCleaningCounter + 1
                            If typPlateInfo.intPrbCleanAutoSubCount <= gProbeCleaningCounter Then
                                'V4.3.0.0③
                                globaldblCleaningPosX = typPlateInfo.dblPrbCleanPosX
                                globaldblCleaningPosY = typPlateInfo.dblPrbCleanPosY
                                globaldblCleaningPosZ = typPlateInfo.dblPrbCleanPosZ
                                frmProbeCleaning.ProbeCleaningCnt = typPlateInfo.intPrbCleanUpDwCount
                                'V4.3.0.0③
                                frmProbeCleaning.ProbeCleaningStart()
                                gProbeCleaningCounter = 0
                            End If
                        End If
                    End If

                    '----- V4.11.0.0③↓ (WALSIN殿SL436S対応) -----
                    '-----------------------------------------------------------------
                    ' オートパワー調整実行(基板枚数指定/時間(分)指定)FL用(自動運転時)
                    '-----------------------------------------------------------------
                    Call Form1.GetMoveMode(digL, digH, digSW)           ' デジスイッチの読取り
                    ' オートパワー「基板枚数指定」機能あり又は「時間(分)指定」機能ありの場合
                    If (giPwrChkPltNum = 1) Or (giPwrChkTime = 1) Then
                        m_PwrChkCounter = m_PwrChkCounter + 1           ' チェックカウンタ更新
                        m_TimeNow = DateTime.Now
                        m_TimeSpan = m_TimeNow - m_TimeSTart            ' 経過時間更新 

                        If (bIniPwrFlg = True) Then                     ' 初回は上記で実施しているのでやらない
                            bIniPwrFlg = False
                        Else
                            ' オートパワー調整実行(基板枚数指定/時間(分)指定)FL用
                            r = AutoPowerExeByOption(digL, m_PwrChkCounter, m_TimeSTart, m_TimeSpan, m_TimeAss)
                            If (r <> cFRS_NORMAL) Then                  ' エラー ?(※エラーメッセージは表示済み) 
                                rtnCode = r                             ' Return値設定 
                                If (r = cFRS_ERR_RST) Then              ' RESETキー(SL436R 自動運転時) ?
                                    Call SetLoaderIO(&H0, LOUT_SUPLY)   ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                                    GoTo STP_TRIM_END                   ' 終了処理へ
                                Else                                    ' その他の異常終了レベルのエラー 
                                    GoTo STP_ERR_EXIT                   ' アプリ強制終了へ
                                End If
                            End If
                        End If
                    End If
                    '----- V4.11.0.0③↑ -----

                    '---------------------------------------------------------------------------
                    '   θ補正処理
                    '---------------------------------------------------------------------------
                    '----- ###172↓ -----
                    ' 前回は自動モードでパターン認識エラーなら処理終了/続行メッセージを表示する
                    ' (HALT SW押下 又は ADJボタンON時)
                    If (bFgAutoMode = True) And (m_lTrimResult = cFRS_ERR_PTN) Then
                        ' HALT SW読み込み
                        Call HALT_SWCHECK(Sw)
                        ' HALT SW押下 又は ADJボタンON ?
                        If (Sw = cSTS_HALTSW_ON) Or (gbChkboxHalt = True) Then
                            Call ZCONRST()                              ' ラッチ解除

                            ' "自動運転停止中","","STARTキー：処理続行，RESETキー：処理終了"
                            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                                    MSG_LOADER_30, "", MSG_SPRASH35, System.Drawing.Color.Black, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                            If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' 非常停止等のエラーならアプリ強制終了へ(エラーメッセージは表示済み) 
                            If (r = cFRS_ERR_RST) Then                  ' RESETキー押下なら処理終了へ 
                                rtnCode = cFRS_ERR_RST                  ' Return = Cancel(RESETキー) 
                                Call W_RESET()                          ' アラームリセット 
                                Call SetLoaderIO(&H0, LOUT_AUTO)        ' ローダ手動モード切替え(ローダ出力(ON=なし, OFF=自動))
                                'r = DspNGTrayChk()                      ' NGトレイへの排出完了信号のOFFを待つ
                                GoTo STP_TRIM_END
                            End If
                        End If
                    End If
                    '----- ###172↑ -----

                    blnRotTheta = False                                 ' ﾛｯﾄ毎のθ補正 = なし
                    gfCorrectPosX = 0.0                                 ' 補正値初期化 
                    gfCorrectPosY = 0.0
                    gdCorrectTheta = 0.0                                'V5.0.0.9⑨
                    If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then           ' SL432R系 ? 
                        If (gTkyKnd = KND_CHIP) Then                    ' CHIPのみ
                            ' ﾛｯﾄ毎のθ補正あり(シスパラ)で補正モード=0(自動)の時に行う
                            If (gSysPrm.stSPF.giRotTheta = 1) And (typPlateInfo.intReviseMode = 0) Then
                                '' ﾛｰﾀﾞｰI/O入力Bit4のﾁｪｯｸを行う。
                                'Call GetLoaderIO(lWork)                 ' ﾛｰﾀﾞｰIO入力
                                'If (lWork And &H10S) Then               ' ﾛｯﾄ切替θ補正あり？←　このBITはなし
                                '    blnRotTheta = True                  ' ﾛｯﾄ毎のθ補正 = あり
                                'End If
                            End If
                        End If
                    End If

                    ' ラフアライメントを実行するか？   'V5.0.0.9⑩
                    Dim doRough As Boolean = (True = bFgAutoMode) AndAlso
                        (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) AndAlso
                        (0 <> typPlateInfo.intReviseExecRgh) AndAlso (0 < giClampLessRoughCount)

                    Form1.Instance.txtLog.Visible = False   'V6.0.0.0-27

                    ' ﾛｯﾄ毎のθ補正ありであるかﾁｪｯｸする。
                    r = cFRS_NORMAL
                    If (blnRotTheta) Then                               ' ﾛｯﾄ毎のθ補正あり ?(CHIPのみ使用するフラグ)
                        ' θ補正の実行
                        'r = DoCorrectPos(typPlateInfo)
                        r = DoCorrectPos(typPlateInfo, True, doRough)   'V5.0.0.9⑩
                    Else
                        ' 補正ﾓｰﾄﾞ=1(手動)で補正方法=0(補正なし)であるかﾁｪｯｸする。
                        'If (typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) Then
                        If (typPlateInfo.intReviseMode = 1) AndAlso (typPlateInfo.intManualReviseType = 0) Then
                            If (gSysPrm.stDEV.giTheta <> 0) Then        ' θあり ?
                                If (False = doRough) Then               'V5.0.0.9⑩
                                    '----- V1.19.0.0-32   ↓ -----
                                    gfCorrectPosX = 0.0                 ' 補正値初期化 
                                    gfCorrectPosY = 0.0
                                    '----- V1.19.0.0-32   ↑ -----
                                    gdCorrectTheta = 0.0                'V5.0.0.9⑨
                                Else
                                    ' ラフアライメント実行、θ補正は実行しない
                                    r = DoCorrectPos(typPlateInfo, False, doRough) 'V5.0.0.9⑩
                                End If

                                ' θを指定角度回転させる
                                'Call ROUND4(typPlateInfo.dblRotateTheta) ' θ載物台の絶対角度指定回転'###037
                                Call ROUND4(typPlateInfo.dblRotateTheta + gdCorrectTheta) ' θ載物台の絶対角度指定回転 'V5.0.0.9⑩
                            End If

                            'V5.0.0.9⑩              ↓
                            '    ' ﾛｯﾄ毎のθ補正なしで、補正モード=1(手動),補正方法=1(1回のみ),シータ補正実行フラグ(実行した)以外
                            'ElseIf (gSysPrm.stSPF.giRotTheta = 0) And Not (typPlateInfo.intReviseMode = 1 And typPlateInfo.intManualReviseType = 1 And gManualThetaCorrection = False) Then
                            '    ' θ補正の実行
                            '    r = DoCorrectPos(typPlateInfo)
                        Else
                            ' ﾛｯﾄ毎のθ補正なしで、補正モード=1(手動),補正方法=1(1回のみ),シータ補正実行フラグ(実行した)以外
                            Dim doAlign As Boolean = (gSysPrm.stSPF.giRotTheta = 0) AndAlso
                                 Not ((typPlateInfo.intReviseMode = 1) AndAlso
                                 (typPlateInfo.intManualReviseType = 1) AndAlso
                                 (gManualThetaCorrection = False))

                            ' θ補正の実行
                            r = DoCorrectPos(typPlateInfo, doAlign, doRough)
                        End If
                        'V5.0.0.9⑩                  ↑

                    End If
                    Form1.Instance.txtLog.Visible = True    'V6.0.0.0-27

                    ' θ補正結果をチェックする
                    If (r = cFRS_NORMAL) Then                           ' 正常 ?

                    ElseIf (r = cFRS_ERR_RST) Then                      ' Cancel(RESETｷｰ) ?
                        GoTo STP_TRIM_END                               ' 終了処理へ 
                    ElseIf (r = cFRS_ERR_PTN) Then                      ' パターン認識エラー ?(ログ画面にメッセージを表示済み)
                        '----- V1.18.0.0③↓ .Edg_Failは未使用のため削除 -----
                        'If (gTkyKnd = KND_CHIP) Then                    ' CHIPのみ(ﾄﾘﾐﾝｸﾞ結果印刷)
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Edg_Fail = stPRT_ROHM.Edg_Fail + 1 ' ﾛｯﾄ中の認識不良基板枚数
                        '    End If
                        'End If                                          ' ###028
                        '----- V1.18.0.0③↑ -----

                        ' ローダ出力(ON=パターン認識ＮＧ, OFF=なし) SL432R時
                        If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then   ' SL432R系 ? 
                            ' ローダー出力(ON=ﾊﾟﾀｰﾝ認識NG,OFF=なし)
                            Call SetLoaderIO(COM_STS_PTN_NG, &H0)       ' ローダ出力(ON=パターン認識ＮＧ, OFF=なし) '###035
                            rtnCode = cFRS_ERR_PTN                      ' V6.1.1.0①
                            GoTo STP_TRIM_END                           ' 終了処理へ 
                        End If
                        m_lTrimResult = cFRS_ERR_PTN                    ' 基板単位のトリミング結果(SL436R用) = パターン認識エラー
                        '----- ###146↓ -----
                        'GoTo STP_NEXT_FILE                             ' 次のファイルのロード処理へ(自動運転時)
                        If (bIniFlg = 0) Then
                            bIniFlg = 1                                 ' フラグ = 1(トリミング中)
                        End If
                        GoTo STP_PTN_NG
                        '----- ###146↑ -----
                    Else
                        GoTo STP_ERR_EXIT                               ' 非常停止等のエラーならアプリ強制終了へ
                    End If
                    gManualThetaCorrection = False                      ' シータ補正実行フラグ = False(シータ補正を実行した)

                    ' トリミング実行時は取得画像データサイズを制限する(基準倍率が1倍以上の場合のみ)
                    Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(True)    'V6.0.0.0-26

                    '　伸縮補正     V1.13.0.0⑤↓
                    'ブロックのスタートポジション→TY2の補正値はここで足しこむ方向で検討
                    Call CalcBlockXYStartPos()
                    r = ShinsyukuHoseiMain()
                    If (r = cFRS_NORMAL) Then                           ' 正常 ?

                    ElseIf (r = cFRS_ERR_RST) Then                      ' Cancel(RESETｷｰ) ?
                        GoTo STP_TRIM_END                               ' 終了処理へ 
                    ElseIf (r = -1) Then
                        m_lTrimResult = cFRS_ERR_PTN                    ' 基板単位のトリミング結果(SL436R用) = パターン認識エラー
                        GoTo STP_PTN_NG
                    ElseIf (r = cFRS_ERR_PTN) Then                      ' パターン認識エラー ?(ログ画面にメッセージを表示済み)
                        '----- V1.18.0.0③↓ .Edg_Failは未使用のため削除 -----
                        'If (gTkyKnd = KND_CHIP) Then                    ' CHIPのみ(ﾄﾘﾐﾝｸﾞ結果印刷)
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Edg_Fail = stPRT_ROHM.Edg_Fail + 1           ' ﾛｯﾄ中の認識不良基板枚数 V1.18.0.0③
                        '    End If
                        'End If                                          ' ###028
                        '----- V1.18.0.0③↑ -----

                        ' ローダ出力(ON=パターン認識ＮＧ, OFF=なし) SL432R時
                        If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then   ' SL432R系 ? 
                            ' ローダー出力(ON=ﾊﾟﾀｰﾝ認識NG,OFF=なし)
                            Call SetLoaderIO(COM_STS_PTN_NG, &H0)       ' ローダ出力(ON=パターン認識ＮＧ, OFF=なし) '###035
                            GoTo STP_TRIM_END                           ' 終了処理へ 
                        End If
                        m_lTrimResult = cFRS_ERR_PTN                    ' 基板単位のトリミング結果(SL436R用) = パターン認識エラー
                        '----- ###146↓ -----
                        'GoTo STP_NEXT_FILE                             ' 次のファイルのロード処理へ(自動運転時)
                        If (bIniFlg = 0) Then
                            bIniFlg = 1                                 ' フラグ = 1(トリミング中)
                        End If
                        GoTo STP_PTN_NG
                        '----- ###146↑ -----
                    Else
                        Call Form1.Z_PRINT("Thresh hold length over")
                        GoTo STP_TRIM_END                               ' 非常停止等のエラーならアプリ強制終了へ
                    End If
                    'V1.13.0.0⑤↑

                    '---------------------------------------------------------------------------
                    '   画像表示プログラムを起動する
                    '---------------------------------------------------------------------------
                    If (bIniFlg = 0) Then                               ' 初回のみ起動する
                        bIniFlg = 1
                        ' 統計画面非表示時は起動
                        If gKeiTyp = KEY_TYPE_RS Then
                            ' 画像表示プログラムを起動する
                            '                            r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                            ' ↓↓↓ V3.1.0.0② 2014/12/01
                            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                            'V6.0.0.0⑤                            r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 1)
                            ' ↑↑↑ V3.1.0.0② 2014/12/01
                        ElseIf (Form1.chkDistributeOnOff.Checked = False) Then
                            ' ↓↓↓ V3.1.0.0② 2014/12/01
                            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
                            'V6.0.0.0⑤                            r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                            ' ↑↑↑ V3.1.0.0② 2014/12/01
                        Else
                            ' 統計表示時は統計表示上のボタンを無効にする
                            gObjFrmDistribute.cmdGraphSave.Enabled = False
                            gObjFrmDistribute.cmdInitial.Enabled = False
                            gObjFrmDistribute.cmdFinal.Enabled = False
                        End If
                    End If

                    '---------------------------------------------------------------------------
                    '   トリミングデータをINtime側に送信する
                    '---------------------------------------------------------------------------
                    r = SendTrimData()
                    If (r <> cFRS_NORMAL) Then
                        ' "トリミングデータの設定に失敗しました。" & vbCrLf & "トリミングデータに問題がないか確認してください。"
                        Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                        rtnCode = cFRS_ERR_TRIM                         ' Return値 = トリマエラー
                        'GoTo STP_TRIM_ERR                              ' トリマエラー信号送信へ V1.24.0.0②
                        GoTo STP_TRIM_END                               ' ' 終了処理へ           V1.24.0.0②
                    End If

                    '---------------------------------------------------------------------------
                    '   カット位置補正用パターン登録情報テーブルを設定する【TKY用】
                    '   ※TKY時のみ有効だがCHIP/NET時もテーブル設定(初期化)はする
                    '---------------------------------------------------------------------------
                    giCutPosRNum = CutPosCorrectInit(gRegistorCnt, stCutPos)

                    '' 連続HI-NGﾁｪｯｸﾊﾞｯﾌｧ初期化 ###129
                    'For r = 1 To gRegistorCnt
                    '    iNgHiCount(r) = 0
                    'Next r
                    ' 連続HI-NGﾁｪｯｸﾊﾞｯﾌｧ初期化 ###181
                    Call ClearNgHiCount()

                    '---------------------------------------------------------------------------
                    '   ログ画面に開始日付時刻を表示する
                    '---------------------------------------------------------------------------
                    '----- V1.22.0.0④↓ -----
                    'strMSG = CStr(Today) & " " & CStr(TimeOfDay)
                    strMSG = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")       'V4.4.0.0-0
                    If (stSummaryLog.strStartTime = "") Then
                        stSummaryLog.strStartTime = strMSG              ' サマリログ開始日時 
                    End If
                    Call Form1.Z_PRINT("START " & strMSG)
                    'Call Form1.Z_PRINT("START " & CStr(Today) & " " & CStr(TimeOfDay))
                    '----- V1.22.0.0④↑ -----

                    ' ﾛｸﾞ表示開始ﾌﾗｸﾞをﾁｪｯｸする。
                    If gSysPrm.stLOG.giLoggingMode = 1 Then
                        '' '' '''スレッドの処理として速度向上を検討する。ただし、ログデータの保存方法の検討必要
                        '' '' ''Dim t As New System.Threading.Thread( _
                        '' '' ''        New System.Threading.ThreadStart( _
                        '' '' ''         AddressOf TrimLogging_Start2))
                        '' '' ''t.Start()
                        Call TrimLogging_Start2()                       ' ロギング開始
                    End If

                    '(2011/06/21)
                    '   ここではテーブルを動作させず、TrimmingBlockLoopでステージを動作させる。
                    '   この場所では、ステージオフセットと補正値の加算のみを行う。
                    '   デバッグ/テストにて必要性を検討し、必要であればここでの動作とする。
                    ' X開始位置＋オフセット＋補正値
                    'stgx = typPlateInfo.dblTableOffsetXDir + gfCorrectPosX             'V1.13.0.0③
                    stgx = typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX  'V1.13.0.0③
                    ' Y開始位置＋オフセット＋補正値
                    'stgy = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY             'V1.13.0.0③
                    stgy = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY  'V1.13.0.0③

                    '----- V4.0.0.0-40↓ -----
                    ' SL36S時でステージYの原点位置が上の場合、ブロックサイズの1/2は加算しない
                    Sub_GetStageYPosistion(stgy)

                    ''----- V2.0.0.0⑨↓ -----
                    ''If (giMachineKd = MACHINE_KD_RS) Then
                    ''    stgy = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2) ' V4.0.0.0-40
                    ''End If
                    ''----- V2.0.0.0⑨↑ -----
                    '----- V4.0.0.0-40↑ -----

                    ' 非常停止ﾎﾞﾀﾝの確認
                    If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

                    '---------------------------------------------------------------------------
                    '   BP位置の設定
                    '---------------------------------------------------------------------------
                    ' ブロックサイズ、BP位置ｵﾌｾｯﾄX,Y設定
                    Call BSIZE(typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir)
                    Call BPOFF(typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)

                    ' 非常停止ﾎﾞﾀﾝの確認
                    If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

                    Call DebugCheck()

                    '---------------------------------------------------------------------------
                    '   ステージ位置の算出
                    '---------------------------------------------------------------------------
                    'プレートのスタートポジション
                    Call CalcPlateXYStartPos()
                    'ブロックのスタートポジション→TY2の補正値はここで足しこむ方向で検討
                    Call CalcBlockXYStartPos()

                    '---------------------------------------------------------------------------
                    '   Z待機位置を算出する
                    '---------------------------------------------------------------------------
                    ' Z軸ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置 = ZON位置 - ｽﾃｯﾌﾟ上昇距離 
                    zStepPos = typPlateInfo.dblZOffSet - typPlateInfo.dblZStepUpDist
                    ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置が待機位置より小さい場合は、待機位置をｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄ位置とする
                    If (zStepPos < typPlateInfo.dblZWaitOffset) Then zStepPos = typPlateInfo.dblZWaitOffset

                    '=======================================================
                    '   トリミングブロックループ(1基板分のトリミング処理を行う)
                    '   ※テーブル移動とZ ON/OFFはこの中で行っている
                    '=======================================================
                    If gKeiTyp = KEY_TYPE_RS Then                       'V2.0.0.0⑩ シンプルトリマの場合
                        SimpleTrimmer.TrimmingStart()                   'V2.0.0.0⑩ スタート時処理
                        SimpleTrimmer.BlockNoBtnVisible(True)           'V4.1.0.0⑱
                        '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
                        'SetNowBlockDspNum(1)                            'V4.1.0.0⑱
                        ' トリミング開始ブロック番号XYを取得する(手動運転時のみ有効)
                        Call Form1.Get_StartBlkNum(WkBlkX, WkBlkY)
                        WkBlkX = WkBlkX * WkBlkY
                        SetNowBlockDspNum(WkBlkX)                       ' ブロック番号表示
                        '----- V4.11.0.0⑤↑ -----
                    End If                                              'V2.0.0.0⑩

                    m_lTrimResult = cFRS_NORMAL                         ' 基板単位のトリミング結果(SL436用) = 正常 '###089
                    m_PlateCounter = m_PlateCounter + 1                 ' プローブチェック機能用基板枚数カウンタ更新 V1.23.0.0⑦
                    r = TrimmingBlockLoop(stgx, stgy,
                                              zStepPos, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet,
                                              typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir,
                                              typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir,
                                              digH, digL, bLoaderNG, strLogDataBuffer, bFgAutoMode)
                    '-----  ###173 -----
                    ''V6.0.1.022↓通常の速度を転送する。 
                    '----- V2.0.0.0_29↓ -----
                    ' X,Y軸速度を通常速度に切り替える(SL436S時)
                    'If (giMachineKd = MACHINE_KD_RS) Then
                    'Dim r2 As Integer
                    'r2 = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, 0).FL, stPclAxisPrm(AXIS_X, 0).FH, stPclAxisPrm(AXIS_X, 0).DrvRat, stPclAxisPrm(AXIS_X, 0).Magnif)
                    ''V4.1.0.0⑬ 通信エラー以外おきないのに２回も個別に見る必要はない          r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' エラーならアプリ強制終了(メッセージ表示済み)
                    ''V4.1.0.0⑬If (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
                    'r2 = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, 0).FL, stPclAxisPrm(AXIS_Y, 0).FH, stPclAxisPrm(AXIS_Y, 0).DrvRat, stPclAxisPrm(AXIS_Y, 0).Magnif)
                    'r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' エラーならアプリ強制終了(メッセージ表示済み)
                    'If (r2 <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT
                    '    SimpleTrimmer.BlockNoBtnVisible(False)           'V4.1.0.0⑱
                    'End If
                    ''V6.0.1.022↓通常の速度を転送する。 
                    SetXYStageSpeed(StageSpeed.NormalSpeed)
                    ''V6.0.1.022↑

                    '----- V2.0.0.0_29↑ -----
                    If (r = cFRS_NORMAL) Then
                        Call DspNGTrayChk()                             ' NGトレイへの排出完了信号のOFFを待つ
                        'Call SimpleTrimmer.SetPlateEnd()                ' V2.0.0.0⑩ 基板終了状態の設定
                        'If gKeiTyp = KEY_TYPE_RS Then                   ' V2.0.0.0⑩ シンプルトリマの場合
                        '    SimpleTrimmer.PlateTrimmingEnd()            ' V2.0.0.0⑩ １基板処理後の処理
                        'End If                                          ' V2.0.0.0⑩
                    End If
                    'V4.3.0.0⑦タイミング変更
                    ''正常終了でないときもブロックデータを表示したい 
                    If gKeiTyp = KEY_TYPE_RS Then                       ' V2.0.0.0⑩ シンプルトリマの場合
                        SimpleTrimmer.BlockNoBtnVisible(False)          ' V4.1.0.0⑱
                        Call SimpleTrimmer.SetPlateEnd()                ' V2.0.0.0⑩ 基板終了状態の設定
                        SimpleTrimmer.PlateTrimmingEnd()                ' V2.0.0.0⑩ １基板処理後の処理
                    End If                                              ' V2.0.0.0⑩
                    ''----- ###173↑ -----
                    'V4.3.0.0⑦タイミング変更
                    '----- ###142↓ -----
                    ' NG抵抗数がNG判定基準(0-100%)を超えたら、基板単位のトリミング結果にトリミングNGを設定する(SL436R用)
                    If (bLoaderNG = True) Then                          ' NG抵抗数がNG判定基準(0-100%)を超えた ?
                        m_lTrimResult = cFRS_TRIM_NG                    ' 基板単位のトリミング結果 = トリミングNG
                    End If
                    '----- ###142↑ -----

                    '----- V1.22.0.0④↓ -----
                    ' サマリーログファイル出力(x0,x1,x2モード時)
                    'strMSG = CStr(Today) & " " & CStr(TimeOfDay)
                    strMSG = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")       'V4.4.0.0-0
                    stSummaryLog.strEndTime = strMSG
                    SummaryLoggingWrite(digH, digL)
                    '----- V1.22.0.0④↑ -----

                    '----- V6.0.3.0⑦↓(カットオフ調整機能) -----
                    If gAdjustCutoffFunction = 1 And (giMachineKd = MACHINE_KD_RS) And (bFgAutoMode = True) Then ' V6.0.3.0⑳
                        ' 調整中、または調整済み、ブロック内処理が正常の場合はファイル保存する
                        If stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC OrElse stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_ALREADY Then
                            '-----  V6.0.3.0_24 -----
                            Dim rt2 As Integer
                            stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_FINISHED
                            rt2 = File_Save(typPlateInfo.strDataName)    ' トリムデータ書込み
                            If rt2 <> cFRS_NORMAL Then
                                '----- V6.0.3.0_24↑ -----
                                MsgBox("ファイルの保存に失敗しました。" + typPlateInfo.strDataName)
                            End If
                        End If
                    End If
                    '----- V6.0.3.0⑦↑ -----

                    ' ロギングスタートフラグをチェックする。
                    If gSysPrm.stLOG.giLoggingMode = 1 Then             ' ログ出力する ?
                        '#4.12.2.0④                        Call TrimLogging_Write(strLogDataBuffer)        ' ログ出力 
                        TrimLogging_Write(strLogDataBuffer.ToString())  ' ログ出力      '#4.12.2.0④ 
                        '#4.12.2.0④                        strLogDataBuffer = ""                           ' ロギングバッファー初期化 
                        strLogDataBuffer.Length = 0                     ' ロギングバッファー初期化'#4.12.2.0④ 
                    End If

                    ' トリミング結果をチェックする 
                    If (r = cFRS_ERR_EMG) Then
                        GoTo STP_ERR_EXIT                               ' アプリ強制終了
                    ElseIf (r = cFRS_ERR_AIR) Then
                        GoTo STP_ERR_EXIT                               ' アプリ強制終了
                    ElseIf (r = cFRS_NORMAL) Or (r = cFRS_ERR_START) Then
                        ' 正常終了- ログ画面に終了日付時刻を表示する
                        'Call Form1.Z_PRINT("END " & CStr(Today) & " " & CStr(TimeOfDay) & vbCrLf)  ' V1.22.0.0④
                        Call Form1.Z_PRINT("END " & strMSG & vbCrLf)                             ' V1.22.0.0④
                    ElseIf (r = cFRS_ERR_RST) Then
                        'Call Form1.Z_PRINT("RESET END " & CStr(Today) & " " & CStr(TimeOfDay) & vbCrLf)' V1.22.0.0④
                        Call Form1.Z_PRINT("RESET END " & strMSG & vbCrLf)                           ' V1.22.0.0④
                        rtnCode = cFRS_ERR_RST                          ' Return値 = Cancel(RESETｷｰ)
                        GoTo STP_TRIM_END                               ' 終了処理へ
                        '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
                        'ElseIf (r = cFRS_ERR_LDR3) Then                     ' ローダアラーム(軽故障)(SL436R時)なら処理続行 ###073
                        '    '----- ###196↓ -----
                        '    'ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDRTO) Then
                        '    '    GoTo STP_TRIM_END                          ' ローダアラーム(軽故障以外)は終了処理へ(SL436R時) ###073
                        'ElseIf (r = cFRS_ERR_LDR2) Then                     ' ローダアラーム(サイクル停止)(SL436R時)なら処理続行

                        'ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDRTO) Then
                        '    GoTo STP_TRIM_END                               ' ローダアラーム(軽故障,サイクル停止以外)は終了処理へ(SL436R時) ###073
                        '    '----- ###196↑ -----
                    ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                        '                                               ' ローダアラーム検出(SL436R 自動運転時) ? 
                        giErrLoader = r                                 ' ローダアラーム検出
                        Call SetLoaderIO(&H0, LOUT_SUPLY)               ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                        Call W_RESET()                                  ' アラームリセット 
                        GoTo STP_TRIM_END                               ' 終了処理へ
                    ElseIf (r = cFRS_ERR_LDRTO) Then                    ' ローダ通信タイムアウト  ?
                        giErrLoader = r                                 ' ローダアラーム検出
                        Call SetLoaderIO(&H0, LOUT_SUPLY)               ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                        GoTo STP_TRIM_END                               ' 終了処理へ
                    ElseIf (r = cFRS_ERR_LOTEND) Then                   ' LotEnd 
                        giErrLoader = cFRS_NORMAL                       ' ロット終了は正常とする
                        rtnCode = cFRS_NORMAL
                        bFgAllMagagin = True
                        Call SetLoaderIO(&H0, LOUT_SUPLY)               ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始))
                        GoTo STP_TRIM_END                               ' 終了処理へ
                        '----- V4.11.0.0⑥↑ -----
                        '----- ###229↓ -----
                        ' GPIB系エラーならアプリ強制終了しないで終了処理へ
                    ElseIf (System.Math.Abs(r) >= ERR_GPIB_PARAM) And (System.Math.Abs(r) <= ERR_GPIB_EXEC) Then
                        Call Form1.Z_PRINT("GPIB ERROR " & vbCrLf)
                        GoTo STP_TRIM_END                               ' 終了処理へ
                        '----- ###229↑ -----
                    Else
                        GoTo STP_ERR_EXIT                               ' アプリ強制終了'###032
                    End If

STP_PTN_NG:         '                                                   ' ###146
                    ' デジスイッチの読取り V6.0.3.0⑥
                    Call Form1.GetMoveMode(digL, digH, digSW)

                    ' 基板枚数の表示 ###093
                    blnCheckPlate = CheckPlate(False, digL)             ' ###145 (blnCheckPlateがFalseでないと基板枚数がカウントされない) 

                    ' 全マガジン終了チェック(SL436R時)
                    If (bFgAllMagagin = True) Then                      ' 全マガジン終了？
                        ' ローダへ最終基板の取出要求を送信する
                        bIniFlg = 3
                        r = Loader_WaitTrimStart(Form1.System1, bFgAutoMode, m_lTrimResult, bFgMagagin, bFgAllMagagin, bFgLot, bIniFlg)
                        '----- V4.0.0.0⑲↓ -----
                        ' LotEndならReturn値=正常とする 
                        If (r = cFRS_ERR_LOTEND) Then
                            r = cFRS_NORMAL
                        End If
                        '----- V4.0.0.0⑲↑ -----
                        Call Form1.System1.AutoLoaderFlgReset()         ' オートローダ　フラグリセット ###099
                        If (r <> cFRS_NORMAL) Then                      ' エラー ?(※エラーメッセージは表示済み) 
                            rtnCode = r                                 ' Return値設定 
                            If (r = cFRS_ERR_RST) Then                  ' RESETキー(SL436R 自動運転時) ?
                                GoTo STP_TRIM_END                       ' 終了処理へ
                            ElseIf (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Then
                                '                                       ' ローダアラーム検出(SL436R 自動運転時) ? ###073
                                giErrLoader = r                         ' ローダアラーム検出 ###073
                                Call W_RESET()                          ' アラームリセット 
                                GoTo STP_TRIM_END                       ' 終了処理へ
                            ElseIf (r = cFRS_ERR_LDRTO) Then            ' ローダ通信タイムアウト  ?
                                giErrLoader = r                         ' ローダアラーム検出 V1.16.0.0⑨
                                GoTo STP_TRIM_END                       ' 終了処理へ
                            Else                                        ' その他の異常終了レベルのエラー 
                                GoTo STP_ERR_EXIT                       ' アプリ強制終了へ
                            End If
                        End If

                        '----- V1.23.0.0⑤↓ -----
                        ' マガジンが下がるまで待つ
                        r = Sub_MagazineDownCheck(Form1.System1)
                        'Call System.Threading.Thread.Sleep(10 * 1000)  ' Wait(msec)※正しくは全マガジンが下になるまで待つ V1.23.0.0⑤ 
                        '----- V1.23.0.0⑤↑ -----
                        Exit For                                        ' 終了処理へ(ファイルロードのループを抜ける)
                    End If

                    ' マガジン終了チェック(SL436R時) ※エンドレスモード時は1ファイルで全マガジンを処理する為、無視
                    If (giActMode = MODE_MAGAZINE) And (bFgMagagin) Then ' マガジンモードで マガジン終了？
                        Exit Do                                         ' トリミングのループを抜けて次ファイルのロード処理へ
                    End If

                    ' ロット切替えチェック(SL436R時)
                    If (giActMode = MODE_LOT) And (bFgLot) Then         ' ロットモードで ロット切替え要求あり？
                        Exit Do                                         ' トリミングのループを抜けて次ファイルのロード処理へ
                    End If

                    ' トリミングNG ?
                    If (bFgAutoMode) And (bLoaderNG = True) Then        ' トリミングNG ?
                        bLoaderNG = False                               ' トリミングNGフラグOFF
                    End If

                    '----- ###153↓ -----
                    ' ログ画面表示クリア(自動運転時) 
                    If (bFgAutoMode) Then                               ' 自動運転 ? 
                        gDspCounter = gDspCounter + 1                   ' ログ画面表示基板枚数カウンタ更新
                        FormLotEnd.Processed += 1                       ' 処理基板枚数                ' V6.0.3.0_21
                        If (gDspCounter > gDspClsCount) Then            ' カウンタ > ログ画面表示クリア基板枚数
                            Call Form1.Z_CLS()                          ' ログ画面クリア
                            gDspCounter = 1                             ' ログ画面表示基板枚数カウンタ再設定
                        End If
                    End If
                    '----- ###153↑ -----

                Loop While (bFgAutoMode)                                ' 自動運転中は回る(ローダへ基板要求を送信し、ローダからのトリミングスタート信号待ちへ)


                ' INtime内のワークメモリ解放
                Call TRIMEND()

                ' 時間を再設定
                'V4.3.0.0⑥　Call GETSETTIME()                                       ' CMOSﾒﾓﾘよりWindows側時間を設定する

                ' 配列を初期化する(CHIPのみ)
                'Erase tDelayTrimNgCnt                                  'V1.23.0.0⑥
STP_NEXT_FILE:
            Next FileNum                                                ' 次のファイルのロード処理へ(自動運転時)

STP_TRIM_END:
            ' INtime内のワークメモリ解放
            Call TRIMEND()

            If gKeiTyp = KEY_TYPE_RS Then
                GroupBoxEnableChange(True)
                Clear113Bit() 'V4.11.0.0⑧
                'V4.11.0.0⑪
                If IsNothing(gObjMSG) <> True Then
                    gObjMSG.MsgClose()
                    gObjMSG = Nothing
                End If
                'V4.11.0.0⑪

            End If

            '--------------------------------------------------------------------------
            '   Zを待機位置へ移動する
            '--------------------------------------------------------------------------
            r = Form1.System1.EX_PROBOFF(gSysPrm)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '              ' エラーならアプリ強制終了へ(※エラーメッセージは表示済み) 

            ' 非常停止ﾎﾞﾀﾝの確認
            If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY

            ' 'V1.13.0.0⑫　↓
            '            If typPlateInfo.intIDReaderUse = 1 Then
            If (typPlateInfo.intIDReaderUse = 1) And (gSysPrm.stLOG.giLoggingMode = 1) Then  ''V1.13.0.1③
                Dim DirX As Integer = 1, DirY As Integer = 1
                Dim OffPosX As Double, OffPosY As Double
                Dim IdData As String = ""
                TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)
                frmIDReaderTeach.ObjOmronIDReader.GetIDReaderPotision(gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, typPlateInfo.dblTableOffsetXDir, typPlateInfo.dblTableOffsetYDir, gfCorrectPosX, gfCorrectPosY, OffPosX, OffPosY)
                OffPosX = typPlateInfo.dblIDReadPos1X * DirX + OffPosX
                OffPosY = typPlateInfo.dblIDReadPos1Y * DirY + OffPosY
                r = Form1.System1.EX_SMOVE2(gSysPrm, OffPosX, OffPosY)
                If (r < cFRS_NORMAL) Then                               ' エラー ?(※エラーメッセージは表示済み) 
                    GoTo STP_ERR_EXIT                                   ' アプリ強制終了へ
                End If
                If frmIDReaderTeach.ObjOmronIDReader.IDRead(1, IdData) Then
                    IDReadName = IdData
                    r = IDReadName.Length - 1
                    strMSG = IDReadName.Substring(0, r)
                    strMSG = gSysPrm.stLOG.gsLoggingDir + strMSG + ".log"
                    FileCopy(globalLogFileName, strMSG)
                Else
                    ''V1.13.0.1②
                    strMSG = Today.ToString("yyyyMMdd") & "_" & TimeOfDay.ToString("HHmmss")
                    strMSG = gSysPrm.stLOG.gsLoggingDir + strMSG + ".log"
                    FileCopy(globalLogFileName, strMSG)
                    ''V1.13.0.1②
                    IDReadName = ""
                End If
            End If
            ' 'V1.13.0.0⑫　↑

            '###161 ステージ原点移動の後に移動
            '' 残基板取り除きメッセージ表示(自動運転でCancel(RESETｷｰ)押下時)又はローダアラーム検出時 ###124
            'If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
            '    If (rtnCode = cFRS_ERR_RST) Then
            '        ' 自動運転中止メッセージ表示
            '        r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_RSTAUTO)  ' STARTキー押下待ち画面表示
            '    Else
            '        ' 残基板取り除きメッセージ表示
            '        r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_WKREMOVE) ' STARTキー押下待ち画面表示
            '    End If
            '    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            'End If

            '----- ###222↓ -----
            '--------------------------------------------------------------------------
            '   ステージ原点移動時ハンドが下がっている場合ハンドを上昇させる(SL436R自動運転でアラーム発生時)
            '--------------------------------------------------------------------------
            If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
                W_HAND1_UP()                                            ' 供給ハンド
                W_HAND2_UP()                                            ' 収納ハンド
            End If
            '----- ###222↑ -----

            '--------------------------------------------------------------------------
            '   ステージ原点移動
            '--------------------------------------------------------------------------
            Dim bInitStage As Boolean
            bInitStage = False

            ' XY_SLIDE同時動作指定時
            If gSysPrm.stTMN.giOnline = 2 Then
                '----- V1.22.0.0⑩↓ -----
                r = INTERLOCK_CHECK(InterlockSts, Sw)                   ' インターロック状態取得
                ' 自動運転でインタロック解除時または手動運転時は原点移動FLGを立てる
                If ((giHostMode = cHOSTcMODEcAUTO) And (InterlockSts <> INTERLOCK_STS_DISABLE_NO)) Or (giHostMode = cHOSTcMODEcMANUAL) Then
                    ' ステージ原点移動
                    bInitStage = True
                End If

                '' XY_SLIDE同時動作  ' InterLockSwRead()未サポートのため修正必要
                'If giHostMode = cHOSTcMODEcAUTO And (Form1.System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) <> 0 Or giHostMode = cHOSTcMODEcMANUAL Then
                '    ' ステージ原点移動
                '    bInitStage = True
                'End If
                '----- V1.22.0.0⑩↑ -----
            ElseIf gSysPrm.stTMN.giOnline = 1 Then                      ' XY_SLIDE同時動作
                ' ステージ原点移動
                bInitStage = True
            End If

            ' この処理もいまいちだが、2箇所記述よりは良い？
            If True = bInitStage Then
                If r <> cGMODE_LDR_MNL Then
                    '----- V1.18.0.0⑨↓ -----
                    ' SL436R自動運転時で自動運転続行指示待ちの場合はここではXYステージの原点移動しない
                    ' 'V2.0.0.0⑳                   If ((gSysPrm.stCTM.giSPECIAL = customROHM) And (bFgAutoMode = True) And (giAutoModeContinue = 1) And _
                    If ((bFgAutoMode = True) And (giAutoModeContinue = 1) And (giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Then  'V2.0.0.0⑳

                    Else
                        ' XYステージの原点移動(第二原点位置にステージを移動する)
                        '----- V2.0.0.0⑧↓ -----
                        r = Form1.System1.XYtableMove(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                        'V5.0.0.6②                        r = Form1.System1.XYtableMove(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                        'r = Form1.System1.XYtableMove(gSysPrm, 0.0#, 0.0#)
                        '----- V2.0.0.0⑧↑ ----
                        If (r <> cFRS_NORMAL) Then                          ' エラー ?(メッセージは表示済み)
                            GoTo STP_ERR_EXIT                               ' アプリ強制終了へ
                        End If
                    End If

                    '' XYステージの原点移動
                    'r = Form1.System1.XYtableMove(gSysPrm, 0.0#, 0.0#)
                    'If (r <> cFRS_NORMAL) Then                          ' エラー ?(メッセージは表示済み)
                    '    GoTo STP_ERR_EXIT                               ' アプリ強制終了へ
                    'End If
                    '----- V1.18.0.0⑨↑ -----
                End If
            End If

            '--------------------------------------------------------------------------
            '       THETAPARAM=（0:θを原点に戻さない（標準）,1:θを原点に戻す）
            '       THETAPARAMは「位置補正モード=1(手動)で補正方法=0(補正)なし」時有効
            '       ・位置補正モード=0(自動),2(自動+微調)の時は無条件にθを原点に戻す
            '       ・位置補正モード=1(手動)で, 補正方法=1(1回のみ)は原点に戻さない
            '                                            2(毎回)は原点に戻す ###233
            '--------------------------------------------------------------------------
            If (gSysPrm.stDEV.giTheta <> 0) Then                        ' θ有り ?
                If (typPlateInfo.intReviseMode = 0) Or (typPlateInfo.intReviseMode = 2) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 2))) Then
                    '###233  ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1)) Then
                    Call ROUND4(0.0#)                                   ' θを原点に戻す
                    '----- V1.19.0.0-32   ↓ -----
                    gfCorrectPosX = 0.0                                 ' 補正値初期化 
                    gfCorrectPosY = 0.0
                    '----- V1.19.0.0-32   ↑ -----
                End If
            End If

            '----- ###161↓ -----
            ' 残基板取り除きメッセージ表示(自動運転でCancel(RESETｷｰ)押下時)又はローダアラーム検出時 ###124
            If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
                If (rtnCode = cFRS_ERR_RST) Then
                    ' 自動運転中止メッセージ表示
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_RSTAUTO)  ' STARTキー押下待ち画面表示
                Else
                    ' 残基板取り除きメッセージ表示
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_WKREMOVE) ' STARTキー押下待ち画面表示
                End If
                If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            End If
            '----- ###161↑ -----

            ' カバー開後ローダを手動モードにした場合、原点復帰に切替える
            If r = cGMODE_LDR_MNL Then
                r = Form1.System1.Form_Reset(cGMODE_ORG, gSysPrm, giAppMode, False, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r <= cFRS_ERR_EMG) Then
                    ' 強制終了
                    Call Form1.AppEndDataSave()
                    Call Form1.AplicationForcedEnding()
                End If

                ' 内部カメラへ切り替え
                Call Form1.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)

                ' インターロック状態の表示/非表示
                Call Form1.DispInterLockSts()
            End If

            ' 非常停止ﾎﾞﾀﾝの確認
            If Form1.System1.EmergencySwCheck() Then GoTo STP_EMERGENCY
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETランプOFF
            GoTo STP_EXIT                                               ' 終了処理へ

            '---------------------------------------------------------------------------
            '   エラー発生時
            '---------------------------------------------------------------------------
            ' 非常停止検出時 
STP_EMERGENCY:
            r = Sub_CallFrmRset(Form1.System1, cGMODE_EMG)              ' 非常停止メッセージ表示
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALTランプOFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETランプOFF

            'If (r <= cFRS_ERR_EMG) Then                                ' 非常停止等のエラーならアプリ強制終了
            If (r = cFRS_ERR_EMG) Then                                  ' 非常停止等のエラーならアプリ強制終了
                GoTo STP_ERR_EXIT                                       ' アプリ強制終了へ
            End If
            GoTo STP_EXIT                                               ' 終了処理へ 

            ' トリマエラー信号をローダへ送信する(SL432R系用) 
STP_TRIM_ERR:
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ? 
                ' ローダー出力(ON=トリミングＮＧ,OFF=なし)
                Call SetLoaderIO(COM_STS_TRM_NG, &H0)                   ' ローダ出力(ON=トリミングＮＧ, OFF=なし) ###035
            End If

            '' シグナルタワー制御(On=異常+ﾌﾞｻﾞｰ1, Off=全ﾋﾞｯﾄ) ###007
            'Select Case (gSysPrm.stIOC.giSignalTower)
            '    Case SIGTOWR_NORMAL                                 ' 標準(赤点滅+ブザー１)
            '        r = Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            '    Case SIGTOWR_SPCIAL                                 ' 特注(赤点滅+ブザー１)
            '        r = Form1.System1.SetSignalTower(EXTOUT_RED_BLK Or EXTOUT_BZ1_ON, &HFFFF)
            'End Select

            ' エラー発生のログ画面出力("トリミング実行中にエラーが発生しました。")
            Call Form1.Z_PRINT(MSG_SPRASH23)

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
STP_EXIT:
            '----- V1.14.0.0⑤↓ -----
            '' CHIPのみ：（090703：minato)
            'If gSysPrm.stSPF.giLedOnOff = 1 Then
            '    Call Form1.System1.Set_Led(0, 1, 0)                    ' バックライト照明ＯＮ／ＯＦＦ(LED制御)
            'End If
            '----- V1.14.0.0⑤↑ -----

            If gKeiTyp = KEY_TYPE_RS Then
                ''V4.3.0.0⑦
                ''V4.1.0.0⑬タイミング変更
                ' ''正常終了でないときもブロックデータを表示したい 
                'If gKeiTyp = KEY_TYPE_RS Then                   ' V2.0.0.0⑩ シンプルトリマの場合
                '    Call SimpleTrimmer.SetPlateEnd()                ' V2.0.0.0⑩ 基板終了状態の設定
                '    SimpleTrimmer.PlateTrimmingEnd()            ' V2.0.0.0⑩ １基板処理後の処理
                'End If                                          ' V2.0.0.0⑩
                ' ''----- ###173↑ -----
                ''V4.1.0.0⑬タイミング変更
                'V4.3.0.0⑦

                'V4.1.0.0⑫
                r = Form1.System1.EX_BSIZE(gSysPrm, 0, 0)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_EXIT '              ' エラーならアプリ強制終了へ(※エラーメッセージは表示済み) 
                r = BPMaintMove()
                'V4.1.0.0⑫
                GroupBoxEnableChange(True)
            End If
            '---------------------------------------------------------------------------
            '   スライドカバーのオープン (SL432R系時)
            '---------------------------------------------------------------------------
            ' スライドカバーをオープンする(手動/自動)
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then       ' SL432R系 ? 
                '----- V1.13.0.0⑧↓ -----
                ' 第二原点位置にステージを移動する
                'V5.0.0.6②                If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                'V5.0.0.6②                r = Form1.System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                If (GetLoaderBordTableOutPosX() <> 0) Or (GetLoaderBordTableOutPosY() <> 0) Then
                    r = Form1.System1.EX_SMOVE2(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                    If (r < cFRS_NORMAL) Then                           ' エラー ?(※エラーメッセージは表示済み) 
                        GoTo STP_ERR_EXIT                               ' アプリ強制終了へ
                    End If
                End If
                '----- V1.13.0.0⑧↑ -----

                Call Form1.System1.ReadHostCommand(gSysPrm, giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag) 'V5.0.0.6⑦ 

                If (gSysPrm.stSPF.giWithStartSw = 1) And (giHostMode <> cHOSTcMODEcAUTO) Then
                    ' スライドカバーを自動オープンしない (ｽﾀｰﾄSW押下待ち(オプション) でローダ自動運転中でない場合)
                    r = Form1.System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, True)
                Else
                    ' スライドカバーを自動オープンする
                    ' XY_SLIDE同時動作 ? (0:OFFLINE, 1:ONLINE, 2:SLIDE COVER+XY移動)
                    If gSysPrm.stTMN.giOnline = TYPE_ONLINE Then        ' XY_SLIDE通常動作
                        r = Form1.System1.Z_COPEN(gSysPrm, giAppMode, giTrimErr, False)
                    End If
                    If gSysPrm.stTMN.giOnline = TYPE_MANUAL Then        '  XY_SLIDE同時動作
                        r = Form1.System1.Z_COPEN(gSysPrm, giAppMode, giTrimErr, True)
                    End If
                End If
            Else
                ' クランプ及びバキュームOFF(SL436R時) '###001
                If (bFgAutoMode = False) Then                           ' 自動運転時はクランプ及びバキュームOFFしない ###107
                    r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                End If
                '----- V6.0.3.0_37↓ -----
                If gVacuumIO = 1 Then
                    'V4.11.0.0⑬
                    If gKeiTyp = KEY_TYPE_RS Then
                        r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                    End If
                    'V4.11.0.0⑬
                End If
                '----- V6.0.3.0_37↑ -----
            End If

            If (r <= cFRS_ERR_EMG) Then                                 ' エラー ?(※エラーメッセージは表示済み) 
                GoTo STP_ERR_EXIT                                       ' アプリ強制終了へ
            End If

            ' 画像表示プログラムを終了する
            If gKeiTyp = KEY_TYPE_RS Then
                'V6.0.0.0⑤                End_SmallGazouProc(ObjGazou)
            Else
                If (Form1.chkDistributeOnOff.Checked = False) Then
                    ' 統計画面非表示時は起動
                    '' '' ''Form1.VideoLibrary1.Refresh()
                    'V6.0.0.0⑤                    End_GazouProc(ObjGazou)
                Else
                    ' 統計画面表示時はボタンを有効に戻す
                    gObjFrmDistribute.cmdGraphSave.Enabled = True
                    gObjFrmDistribute.cmdInitial.Enabled = True
                    gObjFrmDistribute.cmdFinal.Enabled = True
                End If
            End If

            VacuumeStateCheck() 'V4.11.0.0⑧

            '---------------------------------------------------------------------------
            '   ローダ手動運転切替え(SL436R時)
            '---------------------------------------------------------------------------
            r = Loader_ChangeMode(Form1.System1, bFgAutoMode, MODE_MANUAL)
            If (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDRTO) Then
                '                                                       ' ローダアラーム検出(SL436R 自動運転時) ? ###073
                giErrLoader = r                                         ' ローダアラーム検出 ###073
                Call W_RESET()                                          ' アラームリセット信号送出 
                Call W_START()                                          ' スタート信号送出 
                bFgAutoMode = False                                     ' V1.24.0.0③

            ElseIf (r <= cFRS_ERR_EMG) Then                             ' エラー ?(※エラーメッセージは表示済み) 
                GoTo STP_ERR_EXIT                                       ' アプリ強制終了へ
            End If

            ' ###159 自動運転終了メッセージ表示はローダ原点復帰処理後に移動
            'r = Loader_EndAutoDrive(Form1.System1, bFgAutoMode_BK)      ' ※bFgAutoModeは上記のローダ手動運転切替えで手動になっている
            'If (r < cFRS_NORMAL) Then                                   ' エラー ?(※エラーメッセージは表示済み) 
            '    GoTo STP_ERR_EXIT                                       ' アプリ強制終了へ
            'End If

            '---------------------------------------------------------------------------
            '   ローダ原点復帰処理(SL436x時)
            '---------------------------------------------------------------------------
STP_LDR_INIT:  ' ###137
            If (gbFgAutoOperation = True) Then                          ' ローダ自動 ? ###137

                ''V5.0.0.1-31↓
                'If gKeiTyp = KEY_TYPE_RS Then
                '    If giErrLoader = cFRS_ERR_LDR1 Then
                '        GoTo STP_ERR_EXIT                                   ' アプリ強制終了へ
                '    End If
                'End If
                ''V5.0.0.1-31↑

                '----- ###197↓ ----- 
                ' NG排出BOXが満杯の場合は、取り除かれるまで待つ(ローダが原点復帰できないため)
                'r = NgBoxCheck(Form1.System1, APP_MODE_LOADERINIT)     ' V1.18.0.5①
                r = NgBoxCheck(Form1.System1, APP_MODE_TRIM)            ' V1.18.0.5①
                If (r < cFRS_NORMAL) Then
                    GoTo STP_ERR_EXIT                                   ' アプリ強制終了へ
                End If
                '----- ###197↑ ----- 
                '----- V1.16.0.0⑨↓ ----- 
                ' 全マガジン終了(正常終了)時は、全マガジンを下げるのみで「ローダ原点復帰処理」は行わない
                'If (bFgAllMagagin = True) And (giErrLoader = cFRS_NORMAL) Then                                 ' V1.23.0.0⑦
                If (bFgAllMagagin = True) And (giErrLoader = cFRS_NORMAL) And (rtnCode <> cFRS_ERR_RST) Then    ' V1.23.0.0⑦

                Else
                    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_ORG)  ' ローダ原点復帰(全マガジンが下にないと原点復帰異常となる)
                    If r = cFRS_ERR_LDR1 Then
                        GoTo STP_ERR_EXIT
                    End If
                End If
                'r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_ORG)     ' ローダ原点復帰(全マガジンが下にないと原点復帰異常となる)
                '----- V1.16.0.0⑨↑ ----- 
                Call SetLoaderIO(&H0, LOUT_SUPLY + LOUT_PROC_CONTINUE)  ' ローダ出力(ON=なし, OFF=基板要求(連続運転開始)) ###111
                '###180
                Call W_RESET()                                          ' アラームリセット信号送出 
                Call W_START()                                          ' スタート信号送出 
                '###180

                If (r < cFRS_NORMAL) Then                               ' エラー ?(※エラーメッセージは表示済み) 
                    '----- ###137↓-----
                    Call W_RESET()                                      ' アラームリセット信号送出 
                    Call W_START()                                      ' スタート信号送出 

                    '----- ###179↓-----
                    '' "ローダ原点復帰未完了","","STARTキー：処理続行，RESETキー：処理終了"
                    'r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                    '        MSG_SPRASH37, "", MSG_SPRASH35, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)

                    ' "ローダ原点復帰未完了","","STARTキー又はOKボタン押下で原点復帰します。"
                    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                            MSG_SPRASH37, "", MSG_LOADER_22, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT '       ' アプリ強制終了へ 
                    '----- ###179↑-----

                    ' STARTキー押下なら再度原点復帰へ
                    If (r = cFRS_ERR_START) Then GoTo STP_LDR_INIT
                    '----- ###137↑-----
                    'GoTo STP_ERR_EXIT                                  ' アプリ強制終了へ '###073
                End If
            End If

            '' 残基板取り除きメッセージ表示(自動運転でCancel(RESETｷｰ)押下時) ###124(ローダ原点復帰処理の前に移動)
            '' 又はローダアラーム検出時 ###073
            'If ((bFgAutoMode_BK) And (rtnCode = cFRS_ERR_RST)) Or (giErrLoader <> 0) Then
            '    r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_WKREMOVE) ' STARTキー押下待ち画面表示
            '    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT
            'End If

            '----- V1.18.0.0⑨↓ -----
            ' マガジン交換メッセージ表示して自動運転続行か終了かの指定を待つ(SL436R自動運転時 ローム殿特注)
            'V2.0.0.0⑳ If ((gSysPrm.stCTM.giSPECIAL = customROHM) And (bFgAutoMode_BK = True) And (giAutoModeContinue = 1) And _
            'If ((bFgAutoMode_BK = True) And (giAutoModeContinue = 1) And (giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Then  'V2.0.0.0⑳ V6.0.3.0_38
            If ((bFgAutoMode_BK = True) And (giAutoModeContinue = 1) And (giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Or
                ((giAutoModeContinue = 1) And (giReqLotSelect = 1)) Then  ' V6.0.3.0_38
                giReqLotSelect = 0                                      ' マガジンが空でスタートしたときにロット終了か継続かの選択するメッセージを表示するかどうか　V6.0.3.0_38
                r = Sub_CallFrmRset(Form1.System1, cGMODE_LDR_MAGAGINE_EXCHG)
                If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT '           ' エラーならアプリ強制終了へ 
                If (r = cFRS_ERR_START) Then                            ' 続行指定(OKボタン(STARTｷｰ)押下)なら自動運転続行へ
                    gbFgContinue = True                                 ' 自動運転継続フラグON   
                    GoTo STP_CONTINUE
                End If

                ' フラグ等のクリア
                FormLotEnd.Processed = 0                                ' 処理基板枚数クリア             V6.0.3.0_21
                QR_Read_Flg = 0                                         ' QRコード読み込みフラグのクリア V6.0.3.0_33     
                gbAutoOperating = False                                 ' 自動運転中印刷不可フラグクリア V6.0.3.0_31

                ' XYステージの原点移動(第二原点位置にステージを移動する)
                '----- V2.0.0.0⑧↓ -----
                'r = Form1.System1.XYtableMove(gSysPrm, 0.0#, 0.0#)
                'V5.0.0.6②                r = Form1.System1.XYtableMove(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                r = Form1.System1.XYtableMove(gSysPrm, GetLoaderBordTableOutPosX(), GetLoaderBordTableOutPosY())
                '----- V2.0.0.0⑧↑ ----
                If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み)
                    GoTo STP_ERR_EXIT                                   ' アプリ強制終了へ
                End If

                '----- V6.0.3.0⑨↓ -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    TrimData.SetLotChange()                             ' ロット情報クリア
                End If
                '----- V6.0.3.0⑨↑ -----
            End If
            '----- V1.18.0.0⑨↑ -----

            'V4.6.0.0②↓ 'V4.10.0.0⑩ gKeiTypからgMachineTypeに変更　AndからAndAlsoに変更
            If gMachineType = MACHINE_TYPE_436S AndAlso giTimeLotOnly = 1 Then
                SimpleTrimmer.SetLotEnd()
            End If
            'V4.6.0.0②↑

            '----- V6.1.1.0③↓(IAM殿オプション) -----
            ' 自動運転終了時間を表示する(SL436R時)
            If ((gMachineType = MACHINE_TYPE_436R) And (giDispEndTime = 1)) Then
                If (bFgAutoMode_BK = True) Then                         ' 自動運転 ? 
                    Call DispTrimEndTime(cStartTime, cEndTime)          ' 自動運転終了時間表示
                End If
            End If
            '----- V6.1.1.0③↑ -----

            '----- ###159 -----
            '---------------------------------------------------------------------------
            '   自動運転終了メッセージ表示(SL436R時)および
            '   シグナルタワー制御(全マガジン終了,自動運転OFF)
            '---------------------------------------------------------------------------
            Call SetTrimEndTime()                                       ' トリミング終了時間を設定する(ローム殿特注) 'V1.18.0.0③
            r = Loader_EndAutoDrive(Form1.System1, bFgAutoMode_BK)      ' 自動運転終了メッセージ表示およびシグナルタワー制御 (※bFgAutoModeは上記のローダ手動運転切替えで手動になっている)
            If (r < cFRS_NORMAL) Then                                   ' エラー ?(※エラーメッセージは表示済み) 
                GoTo STP_ERR_EXIT                                       ' アプリ強制終了へ
            End If
            '----- V1.18.0.0③↓ -----
            ' 自動運転時(SL436R時)はトリミング結果を印刷する(ローム殿特注)
            '----- V1.18.0.3④↓ -----
            ' ローダアラーム時及び一時停止画面でRESET時(軽故障時でRESET時)は印刷しない 
            'If (bFgAutoMode_BK = True) Then                            ' 自動運転 ? 
            If (bFgAutoMode_BK = True) And ((giErrLoader = 0) And (rtnCode <> cFRS_ERR_RST)) Then
                Call PrnTrimResult(0)                                   ' トリミング結果印刷
            End If
            '----- V1.18.0.3④↑ -----
            '----- V1.18.0.0③↑ -----

            'V6.0.1.0⑫      ↓
            ' トリミング結果表示マップ画像を印刷する
            If (digL < 3) AndAlso (rtnCode <> cFRS_ERR_RST) Then
                Form1.PrintTrimMap()
            End If
            'V6.0.1.0⑫      ↑
            '----- ###159 -----

            '----- V6.0.3.0_37↓ クランプ及びバキュームOFF  -----
            If gVacuumIO = 1 Then
                'V4.11.0.0⑬
                If gKeiTyp = KEY_TYPE_RS Then
                    r = Form1.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)
                End If
                'V4.11.0.0⑬
            End If
            '----- V6.0.3.0_37↑ -----

            ' ローダー出力(SL432Rの場合) ###035
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ?
                ' ローダー出力(ON=トリミングＮＧ,OFF=なし)(SL432R時)
                If bLoaderNG Then
                    Call SetLoaderIO(COM_STS_TRM_NG, &H0)               ' ローダ出力(ON=トリミングＮＧ, OFF=なし) ###035
                End If
            Else
                ' ローダー出力(ON=トリマ部停止中,OFF=なし)(SL436R時)
                Call SetLoaderIO(LOUT_STOP, &H0)                        ' ローダ出力(ON=トリマ部停止中, OFF=なし)
            End If
            Call Form1.System1.AutoLoaderFlgReset()

            ' ランプ状態の変更
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALTランプOFF
            Call LAMP_CTRL(LAMP_START, False)                           ' STARTランプOFF

            ' XYZ移動速度切替え
            '###006            If (Form1.System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0 Then ' InterLockSwRead()未サポートのため修正必要
            r = INTERLOCK_CHECK(InterlockSts, Sw)                       ' インターロック状態取得
            If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then          ' インターロック状態(解除なし)でない ? ###108
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed
                gPrevInterlockSw = 1
            Else
                Call ZSELXYZSPD(0)                                      ' XYZ Normal speed
                gPrevInterlockSw = 0
            End If

            'V4.7.0.0-21↓
            Dim coverSts As Integer
            If (InterlockSts = INTERLOCK_STS_DISABLE_NO) Then           ' インターロック中
                r = COVER_CHECK(coverSts)
                If (r = cFRS_NORMAL And coverSts = 0) Then              ' 筐体カバー開
                    Call Form1.System1.Form_AxisErrMsgDisp(ERR_OPN_CVR)
                    GoTo STP_ERR_EXIT
                End If
            End If
            'V4.7.0.0-21↑

            '----- V6.1.1.0①↓ -----
            ' トリミング終了時にブザーを鳴らす(SL432R時のオプション)
            If (gMachineType = MACHINE_TYPE_432R) Then
                If (rtnCode <> cFRS_ERR_RST) Then                       ' Cancel(RESETｷｰ)以外 ?
                    r = Sub_Buzzer_On(rtnCode)
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT         ' 非常停止等のエラーならアプリ強制終了へ(エラーメッセージは表示済み) 
                End If
            End If
            '----- V6.1.1.0①↑ -----

            ' IDLEモード
            giAppMode = APP_MODE_IDLE                                   ' ｱﾌﾟﾘﾓｰﾄﾞ = トリマ装置アイドル中
            If (gLoadDTFlag = True) Then
                Call LAMP_CTRL(LAMP_START, True)                        ' STARTランプON
            End If

            Form1.txtLog.ShortcutsEnabled = True                        ' ###083 右クリックメニューを表示する 
            Form1.SetMapOnOffButtonEnabled(True)                        ' MAP ON/OFF ボタンを有効にする  'V4.12.2.0①
            Form1.MoveHistoryDataLocation(False)                        'V6.0.1.0⑪
            Form1.MoveTrimMapLocation(False)                            'V6.0.1.0⑪
            Form1.SetTrimMapVisible(Form1.MapOn)                        'V6.0.1.0⑪
            Form1.TimerInterLockSts.Enabled = False                     '  インターロックステータス監視タイマー停止　###178
            Console.WriteLine("Trimming() Return code=" + rtnCode.ToString)
            Call SetTrimStartTime()                                     ' V1.18.0.0③ トリミング開始時間を設定する(ローム殿特注)
            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号を活性化する
            Call Form1.Set_StartBlkNum_Enabled(True)
            '----- V4.11.0.0⑤↑ -----
            ''V5.0.0.1④↓
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1④↑

            Return (rtnCode)                                            ' Return値設定 

STP_ERR_EXIT:
            ' アプリ強制終了
            Form1.TimerInterLockSts.Enabled = False                     ' インターロックステータス監視タイマー停止　###178
            'V6.0.0.0⑤           Call End_GazouProc(ObjGazou)                                ' 画像表示プログラムを終了する
            Call Form1.AppEndDataSave()
            Call Form1.AplicationForcedEnding()                         ' アプリ強制終了

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.Trimming() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生 

        Finally                                         'V6.0.0.0-26
            Form1.Instance.VideoLibrary1.SetAcquisitionSizeForTrimming(False)
        End Try
    End Function
#End Region

#Region "トリミングデータをINtime側へ送信する"
    '''=========================================================================
    '''<summary>トリミングデータをINtime側へ送信する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Function SendTrimData() As Short

        Dim i As Short
        Dim j As Short
        Dim wreg As Short
        Dim cut_type As Short
        'Dim mCut1 As Short
        'Dim mCut2 As Short
        Dim r As Short
        Dim RNO As Short
        Dim bpofx As Double
        Dim bpofy As Double
        Dim startpx As Double
        Dim startpy As Double
        Dim lenxy As Double
        Dim dirxy As Short
        Dim lenxy2 As Double
        Dim dirxy2 As Short
        Dim m As Short
        Dim mm As Short
        'Dim Cdir As Short
        Dim intRegIndex As Short
        Dim intTkyType As Short
        Dim strUCutData As String
        Dim strMSG As String

        Try
            ' 変数初期化
            intTkyType = gTkyKnd                                ' TKY種別の設定(0:TKY, 1:CHIP, 2:NET)                                                
            lenxy = 0                                           ' BP範囲チェック変数の初期化
            dirxy = 0
            lenxy2 = 0
            dirxy2 = 0
            giMarkingMode = typPlateInfo.intNGMark              ' マーキングモード(0:しない,1:NG,2:OK,3:強制)  
            strUCutData = ""                                    ' Uカットデータ名

            ' サーキット数の取得
            If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                'gwCircuitCount = typPlateInfo.intCurcuitCnt        ' ###012
                gwCircuitCount = typPlateInfo.intCircuitCntInBlock  ' 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数　'###012
                If (gTkyKnd = KND_NET) Then                             '###164
                    gwCircuitCount = typPlateInfo.intGroupCntInBlockXBp '###164 1ﾌﾞﾛｯｸ内ｻｰｷｯﾄ数
                End If                                                  '###164
            ElseIf (gTkyKnd = KND_CHIP) Then
                gwCircuitCount = 1
            End If
            'Call GetChipNum(wreg)                               ' 抵抗数
            wreg = typPlateInfo.intResistCntInBlock             ' １ブロック内抵抗数
            'gRegisterExceptMarkingCnt = 0

            If (gTkyKnd = KND_TKY) Then
                ' サーキットデータテーブル
                If giMarkingMode <> 0 Then
                    For i = 1 To gwCircuitCount
                        gfCircuitX(i) = typCircuitInfoArray(i).dblIP2X
                        gfCircuitY(i) = typCircuitInfoArray(i).dblIP2Y
                    Next
                End If

                'For i = 1 To wreg
                '    If typResistorInfoArray(i).intResNo < 1000 Then
                '        gRegisterExceptMarkingCnt = gRegisterExceptMarkingCnt + 1
                '    End If
                'Next
            End If

            ' プレートデータを渡す
            r = SendTrimDataPlate(intTkyType, wreg, bpofx, bpofy)
            If r <> 0 Then
                SendTrimData = r
                Exit Function
            End If

            '----- V1.18.0.0⑦↓ -----
            ' GPIB(ADEX用)はCHIP以外も送信する
            '' CHIPのみ：プレートデータの設定
            'If (gTkyKnd = KND_CHIP) Then
            'r = SendTrimDataPlate2(intTkyType, wreg)   ' ###229
            r = SendTrimDataPlate2(intTkyType, wreg, 0) ' ###229
            If r <> 0 Then
                SendTrimData = r
                Exit Function
            End If
            'End If
            '----- V1.18.0.0⑦↑ -----

            '----- V6.0.3.0⑦↓ (カットオフの保存) -----
            If gAdjustCutoffFunction = 1 And (giMachineKd = MACHINE_KD_RS) Then
                stCutOffAdjust.dblAdjustCutOff = typResistorInfoArray(1).ArrCut(typResistorInfoArray(1).intCutCount).dblCutOff
            End If
            '----- V6.0.3.0⑦↑ -----

            ' 抵抗データを渡す
            bGpib2Flg = 0                                               ' GP-IB制御(汎用)フラグ初期化(0=制御なし, 1=制御あり) ###229
            For i = 1 To wreg
                ' 抵抗データをINtime側へ送信する
                r = SendTrimDataRegistor(intTkyType, i, intRegIndex, wreg, RNO)
                If r <> 0 Then
                    SendTrimData = r
                    Exit Function
                End If

                ' カットデータ（各パターン共通)をINtime側へ送信する
                For j = 1 To typResistorInfoArray(intRegIndex).intCutCount
                    ' 各カットパターン共通のパラメータ送信
                    r = SendTrimDataCut(intTkyType, i, j, intRegIndex, startpx, startpy, m, mm, cut_type)
                    If r <> 0 Then
                        SendTrimData = r
                        Exit Function
                    End If

                    ' カットパターン別データ
                    Select Case cut_type
                        Case 1, 6, 8, 10, 12, 14, 28 ' ST, RETURN/RETRACE + NANAME, ST2
                            r = SendTrimDataCutST(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 2, 7, 9, 11, 13, 15 ' L, (RETURN or RETRACE) + NANAME
                            r = SendTrimDataCutL(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 3 ' HOOK
                            r = SendTrimDataCutHOOK(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 4 ' INDEX
                            r = SendTrimDataCutINDEX(intTkyType, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                        Case 5 ' SCAN CUT
                            r = SendTrimDataCutSCAN(intTkyType, m, mm)
                        Case 17, 18 ' U CUT, U CUT(RETRACE) V1.22.0.0①
                            r = SendTrimDataCutHOOK(intTkyType, cut_type, m, mm, lenxy, dirxy, lenxy2, dirxy2)
                            If (strUCutData = "") Then          ' Uカットデータ名を退避 
                                strUCutData = typResistorInfoArray(m).ArrCut(mm).strDataName
                            End If
                            '----- V1.14.0.0①↓ -----
                        Case 20 ' ES2 CUT(CHIP/NET用)
                            'r = SendTrimDataCutES(intTkyType, m, mm)
                            r = SendTrimDataCutES0(intTkyType, m, mm)
                        Case 21 'ES CUT(CHIP/NET用)
                            r = SendTrimDataCutES(intTkyType, m, mm)
                            '----- V1.14.0.0①↑ -----
                        Case 22 ' "M"       SL436Kでも必要
                            r = SendTrimDataCutMarking(intTkyType, m, mm, typResistorInfoArray(m).ArrCut(mm).intCutAngle)
                        Case 33 ' INDEX2(ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しｲﾝﾃﾞｯｸｽｶｯﾄ)(CHIP/NET用)
                            r = SendTrimDataCutINDEX2(intTkyType, m, mm, lenxy, dirxy, lenxy2, dirxy2)

                            'Case 16 ' C CUT ← 削除
                            '    r = SendTrimDataCutC(intTkyType, i, j, m, mm, mCut1)
                            'Case 21 'ES2 CUT(CHIP/NET用) ← 削除
                            '    r = SendTrimDataCutES2(intTkyType, i, j, m, mm, mCut1)
                            'Case 35 ' Z ← 削除
                            '    r = SendTrimDataCutZ(intTkyType, i, j)
                        Case Else
                            Debug.Print("Error cut_type=" & cut_type)
                            Stop
                    End Select

                    If r <> 0 Then
                        SendTrimData = r
                        Exit Function
                    End If

                    ' 加工範囲のチェック(OcxSystemを使用)
                    '----- V2.0.0.0④↓ -----
                    'r = Form1.System1.BpLimitCheck2(gSysPrm, bpofx, bpofy, _
                    '            typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    '            startpx, startpy)
                    '----- V5.0.0.4③↓ -----
                    'r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
                    '            typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    '            typResistorInfoArray(RNO).ArrCut(j).dblStartPointX, typResistorInfoArray(RNO).ArrCut(j).dblStartPointY)
                    'V5.0.0.6⑩                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
                    'V5.0.0.6⑩                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    'V5.0.0.6⑩                                typResistorInfoArray(i).ArrCut(j).dblStartPointX, typResistorInfoArray(i).ArrCut(j).dblStartPointY)
                    '----- V5.0.0.4③↑ -----
                    '----- V2.0.0.0④↑ -----
                    'V5.0.0.6⑩↓外部カメラカット位置補正量の加算
                    Dim PosX As Double, PosY As Double
                    PosX = typResistorInfoArray(i).ArrCut(j).dblStartPointX
                    PosY = typResistorInfoArray(i).ArrCut(j).dblStartPointY
                    If giTeachpointUse = 1 Then
                        If (Not GetCutStartPointAddTeachPoint(intRegIndex, j, PosX, PosY)) Then
                            PosX = typResistorInfoArray(i).ArrCut(j).dblStartPointX
                            PosY = typResistorInfoArray(i).ArrCut(j).dblStartPointY
                            Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + i.ToString() + "] CUT=[" + j.ToString() + "]")
                        End If
                    End If
                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir,
                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir,
                                PosX, PosY)
                    'V5.0.0.6⑩↑
                    If r <> 0 Then
                        '----- V2.0.0.0④↓ -----
                        '' "'Rxxは加工範囲外です。"
                        'strMSG = "R" & RNO & "=" & MSG_TRIM_01 'V2.0.0.0④
                        ' "'Rxxは加工範囲外です。" "データを確認してください!"
                        strMSG = "R" & typResistorInfoArray(i).intResNo.ToString() + MSG_TRIM_01 + ERR_BP_LIMITOVER.ToString("0")
                        '----- V2.0.0.0④↑ -----
                        Call Form1.Z_PRINT(strMSG)
                        SendTrimData = 16
                        Exit Function
                    End If

                Next j
            Next i

            ' Uカットパラメータを送信する(TKYより移植)
            If (gSysPrm.stSPF.giUCutKind <> 0) Then
                r = SendUCutParam(strUCutData)
                If r Then
                    SendTrimData = 17
                    Exit Function
                End If
            End If

            '----- V1.18.0.0⑦↓ -----
            ' 汎用GPIBデータはデータロード時/データ編集終了時に送信する
            ''----- ###229↓ -----
            '' GPIB2データを渡す
            'r = SendTrimDataPlate2(intTkyType, wreg, 1) ' ###229
            'If r <> 0 Then
            '    SendTrimData = r
            '    Exit Function
            'End If
            ''----- ###229↑ -----
            '----- V1.18.0.0⑦↑ -----
            'V4.9.0.0②↓
            ' 初期値、切り替えポイント、ターンポイントのテーブルを転送する。 
            SendTrimDataCutPoint()
            'V4.9.0.0②↑

            SendTrimData = 0

        Catch ex As Exception
            strMSG = "i-TKY.SendTrimData() TRAP ERROR = " + ex.Message
            SendTrimData = 18   '2011/02/4　エラー番号の設定がないため一時的に設定
            MsgBox(strMSG)
        End Try
    End Function
#End Region
    '----- V2.0.0.0④↓ -----
#Region "加工範囲のチェック"
    '''=========================================================================
    ''' <summary>加工範囲のチェック</summary>
    ''' <param name="DspMd">(INP)ログ表示域にメッセージ表示する(True). しない(False)</param>
    ''' <param name="sMSG"> (OUT)メッセージ設定域</param>
    ''' <returns>cFRS_NORMAL  = 正常 
    '''          cFRS_ERR_RST = 加工範囲エラー(Cancel(RESETｷｰ)で返す)</returns>
    '''=========================================================================
    Public Function Bp_Limit_Check(ByVal DspMd As Boolean, ByRef sMSG As String) As Integer

        Dim r As Integer
        Dim Rn As Integer
        Dim Cn As Integer
        Dim strMSG As String

        Try
            ' 全抵抗チェックする
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' １ブロック内抵抗数
                'オープン/ショートチェック用の抵抗はSKIP 
                If (typResistorInfoArray(Rn).intResNo >= 6000) And (typResistorInfoArray(Rn).intResNo <= 7999) Then
                    GoTo NEXT_REG
                End If

                ' カット数分チェックする
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount

                    ' 加工範囲のチェック(OcxSystemを使用)
                    'V5.0.0.6⑩                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir, _
                    'V5.0.0.6⑩                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir, _
                    'V5.0.0.6⑩                                typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX, typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY)
                    'V5.0.0.6⑩↓外部カメラカット位置補正量の加算
                    Dim PosX As Double, PosY As Double
                    PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                    PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                    If giTeachpointUse = 1 Then
                        If (Not GetCutStartPointAddTeachPoint(Rn, Cn, PosX, PosY)) Then
                            PosX = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointX
                            PosY = typResistorInfoArray(Rn).ArrCut(Cn).dblStartPointY
                            Call Form1.Z_PRINT("GetCutStartPointAddTeachPoint ERROR REG=[" + Rn.ToString() + "] CUT=[" + Cn.ToString() + "]")
                        End If
                    End If
                    r = Form1.System1.BpLimitCheck2(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir,
                                typPlateInfo.dblBlockSizeXDir, typPlateInfo.dblBlockSizeYDir,
                                PosX, PosY)
                    'V5.0.0.6⑩↑
                    If (r <> 0) Then
                        ' "'Rxxは加工範囲外です。" "データを確認してください!"
                        strMSG = "R" & typResistorInfoArray(Rn).intResNo.ToString() & MSG_TRIM_01 & vbCrLf & ERR_BP_LIMITOVER
                        sMSG = strMSG
                        If (DspMd = True) Then                          ' ログ表示域にメッセージ表示する ?
                            Call Form1.Z_PRINT(strMSG)
                        End If
                        Return (cFRS_ERR_RST)                           ' Return値 = Cancel(RESETｷｰ) 
                    End If
                Next Cn
NEXT_REG:
            Next Rn

            Return (cFRS_NORMAL)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.Bp_Limit_Check() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cFRS_ERR_RST)                                       ' Return値 = Cancel(RESETｷｰ) 
        End Try
    End Function
#End Region
    '----- V2.0.0.0④↑ -----
#Region "ディレイトリムの設定チェック"
    '''=========================================================================
    '''<summary>ディレイトリムの設定チェック</summary>
    '''<param name="intCutCnt">(INP) 対象のカット番号</param>
    '''<returns>ディレイ ON：True, OFF：False</returns>
    '''=========================================================================
    Private Function DelayTrimCheck(ByRef intCutCnt As Short) As Boolean

        On Error GoTo ErrExit

        Dim intResCnt As Short
        Dim intForCnt As Short
        Dim blnCheckOk As Boolean
        Dim intSaveCutCnt As Short

        'Call GetChipNum(intResCnt)
        intResCnt = typPlateInfo.intResistCntInBlock                    ' １ブロック内抵抗数
        blnCheckOk = True

        ' INIファイルの設定情報をﾁｪｯｸする。
        If gSysPrm.stSPF.giDelayTrim2 = 1 Then

            ' 抵抗データを渡す
            For intForCnt = 1 To intResCnt
                ' ﾚｼｵﾓｰﾄﾞであるかﾁｪｯｸする。
                If typResistorInfoArray(intForCnt).intTargetValType = 1 Then
                    ' ﾚｼｵﾓｰﾄﾞの場合はﾁｪｯｸNGとする。
                    blnCheckOk = blnCheckOk And False
                Else
                    ' ﾚｼｵﾓｰﾄﾞでない場合はﾁｪｯｸOKとする。
                    blnCheckOk = blnCheckOk And True
                End If

                ' 保持ｶｯﾄ数が0の場合は、最新のｶｯﾄ数を取得する。
                If intSaveCutCnt = 0 Then
                    intSaveCutCnt = typResistorInfoArray(intForCnt).intCutCount
                Else
                    ' 保持ｶｯﾄ数が0より大きい場合には、次のｶｯﾄ数と同じであるかﾁｪｯｸする。
                    If intSaveCutCnt = typResistorInfoArray(intForCnt).intCutCount Then
                        ' 同じ場合にはﾁｪｯｸOKとする。
                        blnCheckOk = blnCheckOk And True
                    Else
                        ' 違う場合にはﾁｪｯｸNGとする。
                        blnCheckOk = blnCheckOk And False
                    End If
                End If
            Next intForCnt

            ' 戻り値を設定する。
            DelayTrimCheck = blnCheckOk
            intCutCnt = intSaveCutCnt
            '----- V1.23.0.0⑥↓ -----
            ' カット数が１の場合はﾃﾞｨﾚｲﾄﾘﾑ2制御無しとする
            If (blnCheckOk = True) And (intCutCnt = 1) Then
                DelayTrimCheck = False
            End If
            '----- V1.23.0.0⑥↑ -----
        Else
            ' ﾃﾞｨﾚｲﾄﾘﾑ2制御無しの場合には、ﾁｪｯｸFalseとする。(ﾃﾞｨﾚｲﾄﾘﾑ2の処理は行なわない。)
            DelayTrimCheck = False
            intCutCnt = 1

        End If

        Exit Function

ErrExit:
        MsgBox("DelayTrimCheck" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Function
#End Region
    '----- V1.18.0.0⑦↓ -----
#Region "汎用GP-IB制御の有無を返す"
    '''=========================================================================
    ''' <summary>汎用GP-IB制御の有無を返す</summary>
    ''' <param name="Gpib2Flg">(OUT)GP-IB制御(汎用)フラグ(0=制御なし, 1=制御あり)</param>
    ''' <param name="wreg">    (OUT)抵抗数</param>
    ''' <param name="Type">    (OUT)タイプ(0=GPIB, 1=GBIB2)</param>
    '''=========================================================================
    Public Sub Gpib2FlgCheck(ByRef Gpib2Flg As Integer, ByRef wreg As Short, ByRef Type As Integer)

        Dim Rn As Integer
        Dim Cn As Integer
        Dim strMSG As String

        Try
            ' シスパラがGP-IB制御あり(2:汎用)でなければGP-IB制御(汎用)制御なしとする
            Gpib2Flg = 0                                                ' GP-IB制御(汎用)フラグ = 0(制御なし)
            Type = 0                                                    ' タイプ = 0(GPIB) 
            wreg = typPlateInfo.intResistCntInBlock                     ' １ブロック内抵抗数

            ' シスパラがGP-IB制御あり(2:汎用)でなければGP-IB制御(汎用)制御なしとする
            If (gSysPrm.stCTM.giGP_IB_flg <> 2) Then Return
            Type = 1                                                    ' タイプ = 1(GPIB2) 

            ' 全抵抗チェックする
            For Rn = 1 To wreg
                If (typResistorInfoArray(Rn).intResNo >= 1000) Then GoTo NEXT_REG
                If (typResistorInfoArray(Rn).intResMeasType = 2) Then   ' 測定タイプ(0:高速 ,1:高精度、２：外部)
                    Gpib2Flg = 1                                        ' GP-IB制御(汎用)フラグ=1(制御あり)
                    Return
                End If

                ' カット数分チェックする
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount
                    ' IX, IX2カットの時にチャックする
                    strMSG = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()
                    If (strMSG <> DataManager.CNS_CUTP_IX) And (strMSG <> DataManager.CNS_CUTP_IX2) Then
                        GoTo NEXT_CUT
                    End If

                    ' 測定判定タイプ 0:高速, 1:高精度, 2:外部)
                    If (typResistorInfoArray(Rn).ArrCut(Cn).intMeasType = 2) Then
                        Gpib2Flg = 1                                    ' GP-IB制御(汎用)フラグ=1(制御あり)
                        'Return                                         ' V6.1.1.0⑧
                        GoTo STP_END                                    ' V6.1.1.0⑧
                    End If
NEXT_CUT:
                Next Cn
NEXT_REG:
            Next Rn

            '----- V6.1.1.0⑧↓ -----
STP_END:
            ' 設定コマンドがない時はGP-IB制御なしとする
            If (typGpibInfo.strI.Trim(" ") = "") And (typGpibInfo.strI2.Trim(" ") = "") And (typGpibInfo.strI3.Trim(" ") = "") And (typGpibInfo.strT.Trim(" ") = "") Then
                Gpib2Flg = 0                                            ' GP-IB制御(汎用)フラグ = 0(制御なし
            End If
            '----- V6.1.1.0⑧↑ -----

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.Gpib2FlgCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0⑦↑ -----
#Region "ロギングメイン処理"
    '''=========================================================================
    ''' <summary>ロギングメイン処理</summary>
    ''' <param name="digH">            (INP)デジタルスイッチ上位設定</param>
    ''' <param name="digL">            (INP)デジタルスイッチ下位設定</param>
    ''' <param name="trimResult">      (INP)トリミング実行結果</param>
    ''' <param name="pltNoX">          (INP)プレート番号X</param>
    ''' <param name="pltNoY">          (INP)プレート番号Y</param>
    ''' <param name="blkNoX">          (INP)ブロック番号X</param>
    ''' <param name="blkNoY">          (INP)ブロック番号Y</param>
    ''' <param name="strLogMsgBuf">    (OUT)ログデータ</param> 
    ''' <param name="contNgCountError">(OUT)</param>
    '''=========================================================================
    Private Function TrimLogging_Main(ByVal digH As Integer, ByVal digL As Integer, ByVal trimResult As Integer,
                                      ByVal pltNoX As Integer, ByVal pltNoY As Integer,
                                      ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                      ByVal strLogMsgBuf As StringBuilder, ByRef contNgCountError As Integer) As Integer
        '#4.12.2.0④    Private Function TrimLogging_Main(ByVal digH As Integer, ByVal digL As Integer, ByVal trimResult As Integer,
        '#4.12.2.0④                        ByRef pltNoX As Integer, ByRef pltNoY As Integer,
        '#4.12.2.0④                        ByRef blkNoX As Integer, ByRef blkNoY As Integer, ByRef strLogMsgBuf As String, ByRef contNgCountError As Integer) As Integer

        '#4.12.2.0④        Dim tmp_m_intxmode10 As Short
        '#4.12.2.0④        Dim strFileLogMsg As String
        Static strFileLogMsg As New StringBuilder(4096)     '#4.12.2.0④
        Dim strLogMsg1 As String
        '#4.12.2.0④        Dim strDispLogMsg As String
        Static strDispLogMsg As New StringBuilder(8192)     '#4.12.2.0④
        Dim r As Integer = cFRS_NORMAL                                  ' V1.19.0.0-21

        '#4.12.2.0④        On Error GoTo ErrExit
        Try                                                 '#4.12.2.0④
            '#4.12.2.0④        strFileLogMsg = ""
            strFileLogMsg.Length = 0                        '#4.12.2.0④
            strLogMsg1 = ""
            '#4.12.2.0④        strDispLogMsg = ""
            strDispLogMsg.Length = 0                        '#4.12.2.0④

            'V6.0.1.0⑪                  ↓
            If (3 <= digL) Then
                Form1.ClearMapBorder()
            End If
            'V6.0.1.0⑪                  ↑

            ' ﾃﾞｼﾞSWの1桁目のﾁｪｯｸを行なう。
            Select Case digL
            ' ×0：INITIAL TEST+TRIM+FINAL TEST
            ' ×1：TRIM+FINAL TEST
            ' ×2：FINAL TEST
                Case 0, 1, 2, 3
                    '===============================================
                    '   判定結果の取得 →gwTrimResult()
                    '===============================================
                    Call TrimLoggingResult_Get(RSLTTYP_TRIMJUDGE, trimResult)

                    If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                        'サーキットOK/NGの取得
                        '(2011/06/25) 取得が必要が検討して再度
                    End If

                    ' x3 モード以外(x0,x1,x2)
                    If (digL <> 3) Then
                        ' NG数集計
                        If (gTkyKnd = KND_CHIP) Then
                            'Call TrimLogging_NgCont_CHIP(digL)                 ' V1.23.0.0⑥
                            'V6.0.1.0⑧                            Call TrimLogging_NgCont_CHIP(digL, blkNoX, blkNoY)  ' V1.23.0.0⑥
                            Dim ret As Integer = TrimLogging_NgCont_CHIP(digL, blkNoX, blkNoY)       'V6.0.1.0⑧
                            ''V6.0.1.0⑧      ↓  MARUWA殿仕様
                            'V6.0.1.3①                            Form1.SetMapColor(blkNoX, blkNoY, Form1.GetResultColor(ret))
                            Form1.SetMapColor(pltNoX, pltNoY, blkNoX, blkNoY, Form1.GetResultColor(ret))    ' プレート対応    'V6.0.1.3①
                            Form1.CountTrimmingResults(ret)
                            ''V6.0.1.0⑧      ↑
                        ElseIf (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                            Dim ret As Integer = TrimLogging_NgCont_Net()       'V6.0.1.0⑧
                            ''V6.0.1.0⑧      ↓  MARUWA殿仕様
                            'V6.0.1.3①                            Form1.SetMapColor(blkNoX, blkNoY, Form1.GetResultColor(ret))
                            Form1.SetMapColor(pltNoX, pltNoY, blkNoX, blkNoY, Form1.GetResultColor(ret))    ' プレート対応    'V6.0.1.3①
                            Form1.CountTrimmingResults(ret)
                        End If

                        '----------------------------------------
                        ' イニシャルテスト結果保存
                        'KND-CHIP or KND-NET
                        ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時は最終ｶｯﾄの時だけﾛｸﾞ出力、ﾛｸﾞ表示を行なう。
                        ' ﾃﾞｨﾚｲﾄﾘﾑ2でない場合には、毎回ﾛｸﾞ出力、ﾛｸﾞ表示を行なう。
                        If (gTkyKnd = KND_NET) Or
                           ((gTkyKnd = KND_CHIP) And ((m_blnDelayCheck And m_blnDelayLastCut) Or (Not m_blnDelayCheck))) Then

                            ' 連続NG-HIGHｴﾗｰﾁｪｯｸ処理
                            ' (x0モードで連続NG-HIGH抵抗ﾌﾞﾛｯｸ数が0以上の時にチェックする ###129)
                            If (digL = 0) And (typPlateInfo.intContHiNgBlockCnt > 0) Then
                                Call TrimLogging_NgHiCountCheck(contNgCountError)

                                ' 連続NG-HIGHｴﾗｰの場合には、処理を抜けて、ｴﾗｰﾀﾞｲｱﾛｸﾞ表示の処理を行なう。
                                If contNgCountError = CONTINUES_NG_HI Then
                                    Exit Function
                                End If
                            End If                                          ' ###129
                        End If

                        '-----------------------------------------------------------
                        ' イニシャルテスト判定結果(測定値)の取得→gfInitialTest()
                        '-----------------------------------------------------------
                        If (gTkyKnd = KND_CHIP) Then
                            If (Not (m_blnDelayCheck And m_blnDelayLastCut)) Then
                                Call TrimLoggingResult_Get(RSLTTYP_INTIAL_TEST, 0)
                            End If

                        Else
                            Call TrimLoggingResult_Get(RSLTTYP_INTIAL_TEST, 0)
                        End If

                        ''V1.14.0.0①
                        'If (gESLog_flg = True) And (digL <= 1) Then     'V1.14.0.0①
                        '    Call TrimLoggingResult_ES()
                        '    Call LoggingWrite_ES()
                        'End If
                        '(2011/06/25) 下記の処理は不要 '###012 復活
                        ' サーキット毎のOK/NGカウントとOK/NGマーキングを行う(TKY/NET時)
                        If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                            Dim ng As Integer = m_lCircuitNgTotal           'V4.12.2.0①
                            'Call TrimLoggingCircuit_OKNG(strLogMsg1)       '###012
                            'Call TrimLoggingCircuit_OKNG(digL, strLogMsg1) 'V1.23.0.0④ ###012
                            r = TrimLoggingCircuit_OKNG(digL, strLogMsg1)   'V1.23.0.0④
                            If (r <> cFRS_NORMAL) Then                      'V1.19.0.0-36 ADD END
                                Return (r)                                  'V1.19.0.0-36 ADD END
                            End If                                          'V1.19.0.0-36 ADD END
#If False Then  ' 京セラ殿仕様
                            'V4.12.2.0①             ↓
                            If (digL < 3) Then
                                If (ng = m_lCircuitNgTotal) Then            ' 加工終了ブロックの背景色設定
                                    Form1.SetMapColor(blkNoX, blkNoY, TrimMap.ColorOK)
                                    'giMapColorOK = 0        'V4.12.2.0⑰ 
                                Else
                                    Form1.SetMapColor(blkNoX, blkNoY, TrimMap.ColorNG)
                                    'giMapColorOK = 1        'V4.12.2.0⑰ 
                                End If
                            End If
                            'V4.12.2.0①             ↑
#End If
                        End If
                    End If

                    '-----------------------------------------------------------
                    ' ファイナルテスト判定結果(測定値)の取得→gfFinalTest()
                    '-----------------------------------------------------------
                    Call TrimLoggingResult_Get(RSLTTYP_FINAL_TEST, 0)

                    '-----------------------------------------------------------
                    ' レシオ目標値判定結果の取得→gfTargetVal()
                    '-----------------------------------------------------------
                    Call TrimLoggingResult_Get(RSLTTYP_RATIO_TARGET, 0)

                    '===============================================
                    ' 強制マーキング(マーキングモード=3の場合)
                    '===============================================
                    r = TrimLoggingMarkingMode_3(digL)
                    If (r <> cFRS_NORMAL) Then                      'V1.19.0.0-36 ADD END
                        Return (r)                                  'V1.19.0.0-36 ADD END
                    End If                                          'V1.19.0.0-36 ADD END

                    '===============================================
                    ' イニシャルテスト/ファイナルテスト分布図処理
                    '===============================================
                    'If (Form1.chkDistributeOnOff.Checked = True) Then      
                    If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then     ' ###139 一時停止画面からのグラフ表示の為に統計は無条件に取る(CHIP/NET時)
                        '----- ###150↓ -----
                        ' x0モード又はx1モードでロギングモード=3(INITIAL + FINAL)の場合、イニシャルテスト結果集計を行う
                        If (digL = 0) Or ((digL = 1) And (gLogMode = 3)) Then
                            ' イニシャルテスト結果集計
                            Call TrimLoggingGraph_RegistNumIT()
                        End If

                        ' ファイナルテスト結果集計
                        Call TrimLoggingGraph_RegistNumFT()

                        'If (gObjFrmDistribute.DisplayInitialMode = True) Then   ' 表示ｸﾞﾗﾌ種別(TRUE:IT FALSE:FT)
                        '    'イニシャルテスト結果表示時
                        '    Call TrimLoggingGraph_RegistNumIT()
                        'Else
                        '    'ファイナルテスト結果表示時
                        '    Call TrimLoggingGraph_RegistNumFT()
                        'End If
                        '----- ###150↑ -----
                    End If

                    '===============================================
                    '   画面/ファイルログ表示メイン処理の呼び出し
                    '===============================================
                    '#4.12.2.0⑥                    Call TrimLogging_LoggingDataMain(digH, digL, strFileLogMsg, strDispLogMsg)
                    TrimLogging_LoggingDataMain(digH, digL, blkNoX, blkNoY, strFileLogMsg, strDispLogMsg)   '#4.12.2.0⑥

                    '===============================================
                    '   ロギング処理(ログ表示画面と生産管理画面)
                    '===============================================
                    If gKeiTyp = KEY_TYPE_RS Then           'V2.0.0.0⑩ シンプルトリマ
                        Call SimpleTrim_LoggingStart()      'V2.0.0.0⑩
                    End If
                    ' V1.23.0.0⑥ 最終ブロック時にCallする
                    Call TrimLogging_LoggingStart(digH, digL, pltNoX, pltNoY, blkNoX, blkNoY,
                                        strLogMsgBuf, strFileLogMsg, strDispLogMsg)

                ' ×4：ﾎﾟｼﾞｼｮﾆﾝｸﾞﾁｪｯｸ
                Case 4

                ' ×5：ｶｯﾃｨﾝｸﾞﾁｪｯｸ
                Case 5
                    If (gTkyKnd = KND_CHIP) Then
                        ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時は最終ｶｯﾄの時だけﾛｸﾞ出力、ﾛｸﾞ表示を行なう。
                        ' ﾃﾞｨﾚｲﾄﾘﾑ2でない場合には、毎回ﾛｸﾞ出力、ﾛｸﾞ表示を行なう。
                        If (m_blnDelayCheck And m_blnDelayLastCut) Or (Not m_blnDelayCheck) Then
                            ' 強制マーキング  (マーキングモード=3の場合)
                            r = TrimLoggingMarkingMode_3(digL)
                            If (r <> cFRS_NORMAL) Then                      'V1.19.0.0-36 ADD END
                                Return (r)                                  'V1.19.0.0-36 ADD END
                            End If                                          'V1.19.0.0-36 ADD END
                        End If
                    Else
                        ' 強制マーキング  (マーキングモード=3の場合)
                        'Call TrimLoggingMarkingMode_3(digL)                'V1.23.0.0④
                        r = TrimLoggingMarkingMode_3(digL)                  'V1.23.0.0④
                        If (r <> cFRS_NORMAL) Then                          'V1.19.0.0-36 ADD END
                            Return (r)                                      'V1.19.0.0-36 ADD END
                        End If                                              'V1.19.0.0-36 ADD END
                    End If

                ' ×6：ＸＹﾃｰﾌﾞﾙﾁｪｯｸ
                Case 6

            End Select

            '#4.12.2.0④            Return (cFRS_NORMAL)                                            'V1.19.0.0-36
        Catch ex As Exception
            '@@@20170810ErrExit:
            MsgBox("TrimLogging_Main" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

        Return (cFRS_NORMAL)                                            'V1.19.0.0-36

    End Function

#End Region

#Region "NG数集計【CHIP用】"
    '''=========================================================================
    ''' <summary>NG数集計</summary>
    ''' <param name="digL">(INP)Dig-SW</param>
    ''' <param name="iBlkX">(INP)ブロック番号X(1-n) V1.23.0.0⑥</param>
    ''' <param name="iBlkY">(INP)ブロック番号Y(1-n) V1.23.0.0⑥</param>
    ''' <returns>マップ用ブロックOK/NG判定値 'V6.0.1.0⑧</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function TrimLogging_NgCont_CHIP(ByVal digL As Integer, ByVal iBlkX As Integer, ByVal iBlkY As Integer) As Integer  ' V1.23.0.0⑥
        'V6.0.1.0⑧    Private Sub TrimLogging_NgCont_CHIP(ByVal digL As Integer, ByVal iBlkX As Integer, ByVal iBlkY As Integer) ' V1.23.0.0⑥

        'V6.0.1.0⑧        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim BlkX As Integer                                             ' V1.23.0.0⑥
        Dim BlkY As Integer                                             ' V1.23.0.0⑥

        BlkX = iBlkX - 1                                                ' V1.23.0.0⑥
        BlkY = iBlkY - 1                                                ' V1.23.0.0⑥

        'V6.0.1.0⑧      ↓
        Dim ItHi As Integer = m_lITHINGCount
        Dim ItLo As Integer = m_lITLONGCount
        Dim FtHi As Integer = m_lFTHINGCount
        Dim FtLo As Integer = m_lFTLONGCount
        Dim OvRg As Integer = m_lITOVERCount
        Dim TmOk As Integer = 0
        Dim ret As Integer = TRIM_RESULT_NOTDO
        'V6.0.1.0⑧      ↑

        Try                             'V6.0.1.0⑧
            For intForCnt = 1 To gRegistorCnt
                '---------------------------------------------------------------
                '   NG数を集計する(ディレイトリム２用 最終ブロック以外)
                '---------------------------------------------------------------
                If (m_blnDelayCheck And Not m_blnDelayLastCut) Then
                    If (TRIM_RESULT_IT_HING = gwTrimResult(intForCnt - 1)) Then ' ###202
                        ' V1.23.0.0⑥ tDelayTrimNgCnt() → stDelay2.NgAry()に変更
                        ' ｲﾆｼｬﾙﾃｽﾄHIGHNGﾌﾗｸﾞをONにする。
                        'stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intNGCnt = 1 ' 未使用のためコメント化 V1.23.0.0⑥
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITHiNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_IT_LONG = gwTrimResult(intForCnt - 1)) Then
                        ' ｲﾆｼｬﾙﾃｽﾄLOWNGﾌﾗｸﾞをONにする。
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITLoNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_FT_HING = gwTrimResult(intForCnt - 1)) Then
                        ' ﾌｧｲﾅﾙﾃｽﾄHIGHNGﾌﾗｸﾞをONにする。
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intFTHiNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_FT_LONG = gwTrimResult(intForCnt - 1)) Then
                        ' ﾌｧｲﾅﾙﾃｽﾄLOWNGﾌﾗｸﾞをONにする。
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intFTLoNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    If (TRIM_RESULT_OVERRANGE = gwTrimResult(intForCnt - 1)) Then
                        ' ｵｰﾊﾞｰﾚﾝｼﾞNGﾌﾗｸﾞをONにする。
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intOverNgCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).intNgFlag = 1
                    End If

                    ' ﾄﾘﾐﾝｸﾞ結果が1(正常)であるかﾁｪｯｸする。
                    If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_OK Then
                        ' 既にNGがない場合のみOKﾌﾗｸﾞをONにする。
                        If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalNGCnt = 0 Then
                            stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalOkCnt = 1
                        End If
                    ElseIf gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_NG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE _
                            Or gwTrimResult(intForCnt - 1) = 11 _
                            Or gwTrimResult(intForCnt - 1) = 13 _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_CVERR _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERLOAD _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_ES2ERR _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_MIDIUM_CUT_NG Then     ' 'V1.20.0.1① ' V1.13.0.0⑪
                        ' NGがある場合にはOKﾌﾗｸﾞを0に戻す。
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalNGCnt = 1
                        stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intTotalOkCnt = 0
                    End If

                    ' ｲﾆｼｬﾙﾃｽﾄの結果(測定値)を保持しておく V1.23.0.0⑥
                    stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).dblInitialTest = gfInitialTest(intForCnt - 1)

                Else
                    '---------------------------------------------------------------
                    '   NG数を集計する(ディレイトリム２用 最終ブロック時)
                    '---------------------------------------------------------------
                    ' ﾃﾞｨﾚｲﾄﾘﾑ2実行時で最終ｶｯﾄの場合のみ、ｲﾆｼｬﾙﾃｽﾄの結果ﾁｪｯｸ、設定を行なう
                    If (m_blnDelayCheck And m_blnDelayLastCut) Then
                        ' 保持しておいたイニシャル結果(測定値)を再設定する。
                        gfInitialTest(intForCnt - 1) = stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).dblInitialTest

                        If digL = 0 Then
                            ' ｲﾆｼｬﾙﾃｽﾄHighNGが発生していたかﾁｪｯｸする。
                            If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITHiNgCnt = 1 Then
                                gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING
                            End If
                            ' ｲﾆｼｬﾙﾃｽﾄLOWNGが発生していたかﾁｪｯｸする。
                            If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intITLoNgCnt = 1 Then
                                gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG
                            End If
                            '----- V1.23.0.0⑥↓ -----
                            ' ｵｰﾊﾞｰﾚﾝｼﾞが発生していたかﾁｪｯｸする。
                            If stDelay2.NgAry(BlkX, BlkY).tNgCheck(intForCnt - 1).intOverNgCnt = 1 Then
                                gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG
                            End If
                            '----- V1.23.0.0⑥↑ -----
                        End If
                    End If

                    '---------------------------------------------------------------
                    '   NG数を集計する
                    '---------------------------------------------------------------
#If False Then
                    If (TRIM_RESULT_IT_HING = gwTrimResult(intForCnt - 1)) Then
                        m_intNgCount = m_intNgCount + 1
                        m_lITHINGCount = m_lITHINGCount + 1
                    End If

                    If (TRIM_RESULT_IT_LONG = gwTrimResult(intForCnt - 1)) Then
                        m_lITLONGCount = m_lITLONGCount + 1
                    End If

                    If (TRIM_RESULT_FT_HING = gwTrimResult(intForCnt - 1)) Then
                        m_lFTHINGCount = m_lFTHINGCount + 1
                    End If

                    If (TRIM_RESULT_FT_LONG = gwTrimResult(intForCnt - 1)) Then
                        m_lFTLONGCount = m_lFTLONGCount + 1
                    End If
                    If (TRIM_RESULT_OVERRANGE = gwTrimResult(intForCnt - 1)) Then
                        m_lITOVERCount = m_lITOVERCount + 1
                    End If
#Else
                    Select Case (gwTrimResult(intForCnt - 1))
                        Case TRIM_RESULT_IT_HING
                            m_intNgCount = m_intNgCount + 1
                            m_lITHINGCount = m_lITHINGCount + 1

                        Case TRIM_RESULT_IT_LONG
                            m_lITLONGCount = m_lITLONGCount + 1

                        Case TRIM_RESULT_FT_HING
                            m_lFTHINGCount = m_lFTHINGCount + 1

                        Case TRIM_RESULT_FT_LONG
                            m_lFTLONGCount = m_lFTLONGCount + 1

                        Case TRIM_RESULT_OVERRANGE
                            m_lITOVERCount = m_lITOVERCount + 1

                        Case TRIM_RESULT_OK
                            TmOk += 1

                        Case Else
                            ' DO NOTHING
                    End Select
#End If
                End If
            Next

            'V6.0.1.0⑧      ↓ MARUWA殿仕様
            ' retがForm1._trimmingResultのKeyにあること
            If (TmOk = gRegistorCnt) Then
                ret = TRIM_RESULT_OK
            ElseIf (FtHi < m_lFTHINGCount) Then
                ret = TRIM_RESULT_FT_HING
            ElseIf (ItHi < m_lITHINGCount) Then
                ret = TRIM_RESULT_FT_HING
            ElseIf (FtLo < m_lFTLONGCount) Then
                ret = TRIM_RESULT_FT_LONG
            ElseIf (ItLo < m_lITLONGCount) Then
                ret = TRIM_RESULT_FT_LONG
            Else
                ret = TRIM_RESULT_NOTDO
            End If
            'V6.0.1.0⑧      ↑            Exit Sub

        Catch ex As Exception
            'V6.0.1.0⑧ErrExit:
            MsgBox("TrimLogging_NgCont" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

        Return ret                      'V6.0.1.0⑧

    End Function
#End Region

#Region "NG数集計【TKY/NET用】"
    '''=========================================================================
    ''' <summary>NG数集計</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    'Private Sub TrimLogging_NgCont_Net()
    Private Function TrimLogging_NgCont_Net() As Integer        'V6.0.1.0⑧      ↓

        On Error GoTo ErrExit

        Dim intForCnt As Short

        'V6.0.1.0⑧      ↓
        Dim m_lPTNNGCount As Integer = 0
        Dim ItHi As Integer = m_lITHINGCount
        Dim ItLo As Integer = m_lITLONGCount
        Dim FtHi As Integer = m_lFTHINGCount
        Dim FtLo As Integer = m_lFTLONGCount
        Dim OvRg As Integer = m_lITOVERCount
        Dim PtnNg As Integer = m_lPTNNGCount
        Dim TmOk As Integer = 0
        Dim ret As Integer = TRIM_RESULT_NOTDO
        Dim NormalRegCnt As Integer = 0
        'V6.0.1.0⑧      ↑

        For intForCnt = 1 To gRegistorCnt
            ' NGﾏｰｷﾝｸﾞ処理でないことを確認する。
            If typResistorInfoArray(intForCnt).intResNo < 1000 Then
                NormalRegCnt = NormalRegCnt + 1
            End If
            'If (TRIM_RESULT_IT_HING = gwTrimResult(intForCnt - 1)) Then
            '    m_intNgCount = m_intNgCount + 1
            '    m_lITHINGCount = m_lITHINGCount + 1                     ' IT HI NG数
            'End If

            'If (TRIM_RESULT_IT_LONG = gwTrimResult(intForCnt - 1)) Then
            '    m_lITLONGCount = m_lITLONGCount + 1                     ' IT LO NG数
            'End If

            'If (TRIM_RESULT_FT_HING = gwTrimResult(intForCnt - 1)) Then
            '    m_lFTHINGCount = m_lFTHINGCount + 1                     ' FT HI NG数
            'End If

            'If (TRIM_RESULT_FT_LONG = gwTrimResult(intForCnt - 1)) Then
            '    m_lFTLONGCount = m_lFTLONGCount + 1                     ' FT LO NG数
            'End If

            'If (TRIM_RESULT_OVERRANGE = gwTrimResult(intForCnt - 1)) Then
            '    m_lITOVERCount = m_lITOVERCount + 1                     ' ITｵｰﾊﾞｰﾚﾝｼﾞ数
            'End If

            Select Case (gwTrimResult(intForCnt - 1))
                Case TRIM_RESULT_IT_HING
                    m_intNgCount = m_intNgCount + 1
                    m_lITHINGCount = m_lITHINGCount + 1

                Case TRIM_RESULT_IT_LONG
                    m_lITLONGCount = m_lITLONGCount + 1

                Case TRIM_RESULT_FT_HING
                    m_lFTHINGCount = m_lFTHINGCount + 1

                Case TRIM_RESULT_FT_LONG
                    m_lFTLONGCount = m_lFTLONGCount + 1

                Case TRIM_RESULT_OVERRANGE
                    m_lITOVERCount = m_lITOVERCount + 1

                Case TRIM_RESULT_OK
                    TmOk += 1

                Case Else
                    ' DO NOTHING
            End Select

            '''''2009/07/29 minato
            ''''    下記は436Kのコード。432のINTRTMでは、[12][14]がセットされる事はない。
            '        If (TRIM_RESULT_IT_HING <= gwTrimResult(intForCnt - 1) _
            ''            And 12 <> gwTrimResult(intForCnt - 1)) Then  ' NG抵抗数
            '            m_intNgCount = m_intNgCount + 1
            '        End If
            '
            '        If (14 = gwTrimResult(intForCnt - 1)) Then
            '            glITOVERCount = glITOVERCount + 1
            '        End If
        Next


        'V6.0.1.0⑧      ↓ MARUWA殿仕様
        ' retがForm1._trimmingResultのKeyにあること
        If (TmOk = NormalRegCnt) Then
            ret = TRIM_RESULT_OK
        ElseIf (OvRg < m_lITOVERCount) Then
            ret = TRIM_RESULT_OVERRANGE
        ElseIf (FtHi < m_lFTHINGCount) Then
            ret = TRIM_RESULT_FT_HING
        ElseIf (ItHi < m_lITHINGCount) Then
            ret = TRIM_RESULT_FT_HING
        ElseIf (FtLo < m_lFTLONGCount) Then
            ret = TRIM_RESULT_FT_LONG
        ElseIf (ItLo < m_lITLONGCount) Then
            ret = TRIM_RESULT_FT_LONG
        Else
            ret = TRIM_RESULT_NOTDO
        End If
        'V6.0.1.0⑧      ↑            Exit Sub

        Return ret

        Exit Function

ErrExit:
        MsgBox("TrimLogging_NgCont" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Function
#End Region

#Region "サーキット毎のNGカウント数　OK/NGマーキング(TKY/NET用)"
    '''=========================================================================
    ''' <summary>サーキット毎のNGカウント数　OK/NGマーキング</summary>
    ''' <param name="digL"></param>
    ''' <param name="strLogMsg1"></param>
    ''' <returns></returns>
    ''' <remarks>TKYとNETで使用される</remarks>
    '''=========================================================================
    Private Function TrimLoggingCircuit_OKNG(ByVal digL As Integer, ByRef strLogMsg1 As String) As Integer

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim intGetCircuit As Short
        Dim intNgJudgeUnit As Short
        Dim r As Integer                                    'V1.19.0.0-36

        ' サーキット毎のNG数カウントをクリア
        For intForCnt = 1 To gwCircuitCount
            gwCircuitNgCount(intForCnt) = 0
        Next

        ' NG判定単位を取得する
        intNgJudgeUnit = typPlateInfo.intNgJudgeUnit                                            ' NG判定単位取得(0:BLOCK, 1:PLATE)		
        If intNgJudgeUnit = 0 Then
            m_intNgCount = 0
        End If

        ' サーキット毎のNG数をカウント
        For intForCnt = 1 To gRegistorCnt
            ' NGﾏｰｷﾝｸﾞ処理でないことを確認する。
            If typResistorInfoArray(intForCnt).intResNo < 1000 Then
                intGetCircuit = typResistorInfoArray(intForCnt).intCircuitGrp                   ' サーキット番号
                'If gwTrimResult(intForCnt - 1) = 2 Or gwTrimResult(intForCnt - 1) = 3 Then     ' ###012    
                If gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE _
                        Or gwTrimResult(intForCnt - 1) = 11 _
                        Or gwTrimResult(intForCnt - 1) = 13 _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_CVERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERLOAD _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_ES2ERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_MIDIUM_CUT_NG Then     ' 'V1.20.0.1① ' トリミング結果がNG ? V1.13.0.0⑪ ###012  
                    gwCircuitNgCount(intGetCircuit) = gwCircuitNgCount(intGetCircuit) + 1
                    m_intNgCount = m_intNgCount + 1
                End If
            End If
        Next

        ' ｻｰｷｯﾄ数分処理を行なう。
        For intForCnt = 1 To gwCircuitCount
            ' ｻｰｷｯﾄNGが存在するかﾁｪｯｸする。
            If gwCircuitNgCount(intForCnt) Then                                                 ' NG抵抗が一つでもあるか?
                ' ｻｰｷｯﾄNGｶｳﾝﾀをｲﾝｸﾘﾒﾝﾄする。
                m_lCircuitNgTotal = m_lCircuitNgTotal + 1
                '----- ###167↓ -----
                m_lNgCount = m_lNgCount + 1                                                     ' NGカウント更新
                m_NG_RES_Count = m_NG_RES_Count + 1                                             ' NG判定(0-100%)用NG抵抗数(ブロック単位/プレート単位)
                '----- ###167↑ -----
                strLogMsg1 = strLogMsg1 & "Circuit=" & intForCnt.ToString("00") & " NG"

                ' マーキングモード(1:NGマーキング)で、ﾃﾞｼﾞSWの1桁目が0,1,2であるかﾁｪｯｸする。
                If giMarkingMode = 1 And digL <= 2 Then
                    ' NGマーキングを行う
                    'Call TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)  '###012
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)   '###012
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"

                ElseIf giMarkingMode = 4 And digL <= 2 Then
                    ' マーキングモード(4:NG/OKマーキング)で、ﾃﾞｼﾞSWの1桁目が0,1,2であるかﾁｪｯｸする
                    ' NGマーキングを行う(1000～4999を使用して)
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"
                End If
                strLogMsg1 = strLogMsg1 & Chr(13) & Chr(10)

            Else
                ' ｻｰｷｯﾄOKｶｳﾝﾀをｲﾝｸﾘﾒﾝﾄする。
                m_lCircuitGoodTotal = m_lCircuitGoodTotal + 1
                '----- ###167↓ -----
                m_lGoodCount = m_lGoodCount + 1                                                 ' OKカウント更新
                '----- ###167↑ -----
                strLogMsg1 = strLogMsg1 & "Circuit=" & intForCnt.ToString("00") ' +

                ' マーキングモード(2:OKマーキング)で、、ﾃﾞｼﾞSWの1桁目が0,1,2であるかﾁｪｯｸする。
                If giMarkingMode = 2 And digL <= 2 Then
                    ' OKマーキングを行う(5000～5999番を使用してマーキング)
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"

                ElseIf giMarkingMode = 4 And digL <= 2 Then
                    ' マーキングモード(4:NG/OKマーキング)で、ﾃﾞｼﾞSWの1桁目が0,1,2であるかﾁｪｯｸする
                    ' OKマーキングを行う(5000～5999番を使用してマーキング)
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    strLogMsg1 = strLogMsg1 & ", Marking"
                End If
                strLogMsg1 = strLogMsg1 & vbCrLf
            End If
        Next

        Return (cFRS_NORMAL)

ErrExit:
        MsgBox("TrimLoggingCircuit_OKNG" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
        Return (cFRS_NORMAL)
    End Function
#End Region

#Region "強制マーキング  (マーキングモード=3の場合)"
    '''=========================================================================
    '''<summary>強制マーキング  (マーキングモード=3の場合)</summary>
    ''' <param name="digL"></param>
    ''' <returns></returns>
    '''=========================================================================
    Private Function TrimLoggingMarkingMode_3(ByVal digL As Integer) As Integer

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim r As Integer 'V1.19.0.0-36

        ' マーキングモード(3:強制) でﾃﾞｼﾞSWの1桁目が0,1,2,5であるかﾁｪｯｸする。
        If giMarkingMode = 3 And (digL <= 2 Or digL = 5) Then
            ' ｻｰｷｯﾄ数分処理を行なう。
            For intForCnt = 1 To gwCircuitCount
                ' 強制マーキングを行う(1000～5999番を使用してマーキング)
                r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)
                'V1.19.0.0-36 ADD START
                r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then
                    Return (r)
                End If
                'V1.19.0.0-36 ADD END
            Next

        ElseIf giMarkingMode <> 0 And (digL = 5) Then
            '(x0,1,2は↑の方で書いているのでここではx5だけ)
            For intForCnt = 1 To gwCircuitCount
                If giMarkingMode = 1 Then
                    ' NGマーキングを行う(1000～5999番を使用してマーキング)
                    'Call TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)  '###012
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)   '###012
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END

                ElseIf giMarkingMode = 2 Then
                    ' OKマーキングを行う(5000～5999番を使用してマーキング)
                    'Call TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 0)  '###012
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)   '###012
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END

                ElseIf giMarkingMode = 4 Then
                    ' NGマーキング(1000～4999番)/OKマーキング(5000～5999番)を行う
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 1)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                    r = TRIM_NGMARK(gfCircuitX(intForCnt), gfCircuitY(intForCnt), 0, intForCnt, digL, 2)
                    'V1.19.0.0-36 ADD START
                    r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then
                        Return (r)
                    End If
                    'V1.19.0.0-36 ADD END
                End If
            Next
        End If

        Return (cFRS_NORMAL)        'V1.19.0.0-36

ErrExit:
        MsgBox("TrimLoggingMarkingMode_3" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
        Return (cFRS_NORMAL)        'V1.19.0.0-36
    End Function
#End Region

#Region "判定結果の取得"
    '''=========================================================================
    '''<summary>判定結果の取得</summary>
    ''' <param name="resultType">結果取得タイプ</param>
    ''' <param name="trimRes">トリミング結果（戻り値）</param>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TrimLoggingResult_Get(ByVal resultType As Integer, ByVal trimRes As Integer) 'V1.23.0.0⑦

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim intSetData As Short
        Dim intForCnt As Short

        ' 抵抗数/15の整数部を取得する。
        intCntMax = Int(gRegistorCnt / 15)

        ' 最大値まで処理を行なう。
        For intForCnt = 0 To intCntMax
            ' ｶｳﾝﾀが最大値であるかﾁｪｯｸする。
            If intForCnt < intCntMax Then
                ' 最大値の場合は、15で処理を行なう。
                intSetData = 15
            Else
                ' 最大値以外の場合は、15で除算した余りを取得する。
                intSetData = gRegistorCnt Mod 15
            End If

            ' 取得した値が0の場合には、処理を抜ける。
            If intSetData = 0 Then Exit For

#If cOFFLINEcDEBUG Then
#Else
            Select Case resultType
                Case RSLTTYP_TRIMJUDGE
                    ' 判定結果の取得
                    Call TRIM_RESULT_WORD(resultType, intForCnt * 15, intSetData, 0, 0, gwTrimResult(intForCnt * 15))
                Case RSLTTYP_INTIAL_TEST
                    ' イニシャルテスト結果の取得
                    Call TRIM_RESULT_Double(resultType, intForCnt * 15, intSetData, 0, 0, gfInitialTest(intForCnt * 15))
                Case RSLTTYP_FINAL_TEST
                    ' ファイナルテスト結果の取得
                    Call TRIM_RESULT_Double(resultType, intForCnt * 15, intSetData, 0, 0, gfFinalTest(intForCnt * 15))
                Case RSLTTYP_RATIO_TARGET
                    ' レシオモード目標値の取得
                    Call TRIM_RESULT_Double(resultType, intForCnt * 15, intSetData, 0, 0, gfTargetVal(intForCnt * 15))
            End Select
#End If
        Next

        ' 再ﾌﾟﾛｰﾋﾞﾝｸﾞ失敗
        If (resultType = RSLTTYP_TRIMJUDGE And trimRes = 5) Then
            For intForCnt = 1 To gRegistorCnt
                gwTrimResult(intForCnt - 1) = 13
            Next
        End If

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_Get" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "レシオ目標値判定結果の取得"
    '''=========================================================================
    '''<summary>レシオ目標値判定結果の取得</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingResult_TargetVal()

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim intSetData As Short
        Dim intForCnt As Short

        ' 抵抗数/15の整数部を取得する。
        intCntMax = Int(gRegistorCnt / 15)

        ' 最大値まで処理を行なう。
        For intForCnt = 0 To intCntMax
            ' ｶｳﾝﾀが最大値であるかﾁｪｯｸする。
            If intForCnt < intCntMax Then
                ' 最大値の場合は、15で処理を行なう。
                intSetData = 15
            Else
                ' 最大値以外の場合は、15で除算した余りを取得する。
                intSetData = gRegistorCnt Mod 15
            End If

            ' 取得した値が0の場合には、処理を抜ける。
            If intSetData = 0 Then Exit For

            ' レシオ目標値取得
#If cOFFLINEcDEBUG Then
#Else
            Call TRIM_RESULT_Double(3, intForCnt * 15, intSetData, 0, 0, gfTargetVal(intForCnt * 15))
#End If
        Next
        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_TargetVal" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "連続NG-HIGHｴﾗｰﾁｪｯｸ処理"
    '''=========================================================================
    ''' <summary>連続NG-HIGHｴﾗｰﾁｪｯｸ処理</summary>
    ''' <param name="contNgCountError">(OUT)1=連続NG-HIGHｴﾗｰ発生</param>
    ''' <remarks>下記の処理はオプションとする</remarks>
    '''=========================================================================
    Private Sub TrimLogging_NgHiCountCheck(ByRef contNgCountError As Integer)

        On Error GoTo ErrExit
        Dim intForCnt As Short
        Dim intHighNgCnt As Short

        '----- ###129 ↓ -----
        intHighNgCnt = 0                                                ' FINAL HI NG ｶｳﾝﾀ = 0 
        For intForCnt = 1 To gRegistorCnt                               ' 抵抗数分繰り返す 
            ' INITIAL HI NG
            '----- ###202↓ -----
            ' ITHI NGにオーバレンジを含める
            'If (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) Then ' INITIAL HI NG ? 
            If (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) Or (gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE) Then
                iNgHiCount(intForCnt - 1) = iNgHiCount(intForCnt - 1) + 1
            Else
                iNgHiCount(intForCnt - 1) = 0
            End If
            '----- ###202↑ -----

            ' 連続NG-HIGH抵抗ﾌﾞﾛｯｸ数以上ならエラーとする
            If (iNgHiCount(intForCnt - 1) >= typPlateInfo.intContHiNgBlockCnt) Then
                contNgCountError = CONTINUES_NG_HI                      ' 連続NG-HIGHｴﾗｰ発生
                Exit For
            End If
        Next

        'intHighNgCnt = 0                                    ' FINAL HI NG ｶｳﾝﾀ = 0 
        'For intForCnt = 1 To gRegistorCnt                   ' 抵抗数分繰り返す 
        '    ' FINAL HI NG
        '    If (8 = gwTrimResult(intForCnt - 1)) Then       ' FINAL HI NG ? 
        '        ''''            iNgHiCount(intForCnt - 1) = iNgHiCount(intForCnt - 1) + 1
        '        intHighNgCnt = intHighNgCnt + 1
        '        'Exit For                                    ' ←ここで抜けて良いの ?
        '    Else
        '        ''''            iNgHiCount(intForCnt - 1) = 0
        '    End If
        'Next

        '' 連続High-NGﾌﾞﾛｯｸ数の設定が0より大きい場合には、連続High-NGチェックを行う。
        'If typPlateInfo.intContHiNgBlockCnt > 0 Then
        '    ' 連続NG-HIGH抵抗ﾌﾞﾛｯｸ数の値を超えた場合にはｴﾗｰとする。
        '    If (typPlateInfo.intContHiNgBlockCnt <= intHighNgCnt) Then
        '        contNgCountError = contNgCountError                 ' 連続NG-HIGHｴﾗｰFlg = True
        '    End If
        'End If
        '----- ###129 ↑ -----

        Exit Sub

ErrExit:
        MsgBox("TrimLogging_NgHiCountCheck" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "イニシャルテスト分布図"
    '''=========================================================================
    '''<summary>イニシャルテスト分布図</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingGraph_RegistNumIT()

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim dblGraphDiv As Double                                       ' グラフ範囲刻み値
        Dim dblGraphTop As Double                                       ' グラフ最上段値
        Dim dblGap As Double
        Dim dblGapIT As Double                                          ' 積算誤差ｲﾆｼｬﾙ
        Dim dblX_2IT As Double                                          ' IT標準偏差算出用ワーク
        Dim Average As Double                                           ' ###154 
        'Dim lITTOTAL As Integer                                        ' IT計算対象数 ###138
        dblX_2IT = 0
        'lITTOTAL = 0                                                   ' ###138 
        '----- V6.0.3.0_26↓ -----
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        '----- V6.0.3.0_26↑ -----

        ' ｲﾆｼｬﾙﾃｽﾄ(LOWﾘﾐｯﾄ)とｲﾆｼｬﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)の値ﾁｪｯｸを行う。
        If ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
            ' ｲﾆｼｬﾙﾃｽﾄ(LOWﾘﾐｯﾄ)が0以下でｲﾆｼｬﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)が0以上の場合
            dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5

        ElseIf ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 > typResistorInfoArray(1).dblInitTest_HighLimit)) Then
            ' ｲﾆｼｬﾙﾃｽﾄ(LOWﾘﾐｯﾄ)が0以上でｲﾆｼｬﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)が0より小さい場合
            dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit / 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit / 1.5

        ElseIf ((0 < typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
            ' ｲﾆｼｬﾙﾃｽﾄ(LOWﾘﾐｯﾄ)が0より大きくてｲﾆｼｬﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)が0以上の場合
            dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit / 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5

        Else
            ' 上記条件以外の場合
            dblGraphDiv = 0.3
            dblGraphTop = 1.5
        End If

        '----- V6.0.3.0_26↓ -----
        ' デジスイッチの読取り
        Call Form1.GetMoveMode(digL, digH, digSW)
        '----- V6.0.3.0_26↑ -----

        ' 1ｸﾞﾙｰﾌﾟ内抵抗数分処理を行なう。
        For intForCnt = 1 To gRegistorCnt
            ' ﾄﾘﾐﾝｸﾞ結果がRSLT_OK(1),RSLT_IT_NG(2),RSLT_IT_HING(6),RSLT_IT_LONG(7)で,
            ' RSLT_OPENCHK_NG(20),RSLT_SHORTCHK_NG(21)以外の場合、データを追加する
            'If ((gwTrimResult(intForCnt - 1) = RSLT_OK) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_NG) Or _
            '    (gwTrimResult(intForCnt - 1) = RSLT_IT_HING) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_LONG)) _
            '    And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then
            'V4.0.0.0-48↓
            If ((gwTrimResult(intForCnt - 1) = RSLT_OK) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_NG) Or
                (gwTrimResult(intForCnt - 1) = RSLT_IT_HING) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_LONG) _
                    Or (gwTrimResult(intForCnt - 1) = RSLT_FT_LONG) Or (gwTrimResult(intForCnt - 1) = RSLT_FT_HING)) _
                And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then
                'V4.0.0.0-48↑

                'If ((gwTrimResult(intForCnt - 1) = RSLT_OK) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_NG) Or _
                '    (gwTrimResult(intForCnt - 1) = RSLT_IT_HING) Or (gwTrimResult(intForCnt - 1) = RSLT_IT_LONG)) _
                '    And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then

                ' '' '' '' ﾄﾘﾐﾝｸﾞ結果が0(未実施), 11(未使用), 13(未使用)以外であることを確認する。
                '' '' ''If (gwTrimResult(intForCnt - 1) <> 0) And (gwTrimResult(intForCnt - 1) <> 11) And (gwTrimResult(intForCnt - 1) <> 13) Then
                ' 差を算出する。　ｲﾆｼｬﾙﾃｽﾄ結果/ﾄﾘﾐﾝｸﾞ目標値*100-100
                dblGap = (gfInitialTest(intForCnt - 1) / typResistorInfoArray(intForCnt).dblTrimTargetVal) * 100.0# - 100.0#
                If ((dblGraphTop - (dblGraphDiv * 0)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*0)　<　差の場合
                    glRegistNumIT(0) = glRegistNumIT(0) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 1)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*1)　<　差の場合
                    glRegistNumIT(1) = glRegistNumIT(1) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 2)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*2)　<　差の場合
                    glRegistNumIT(2) = glRegistNumIT(2) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 3)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*3)　<　差の場合
                    glRegistNumIT(3) = glRegistNumIT(3) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 4)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*4)　<　差の場合
                    glRegistNumIT(4) = glRegistNumIT(4) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 5)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*5)　<　差の場合
                    glRegistNumIT(5) = glRegistNumIT(5) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 6)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*6)　<　差の場合
                    glRegistNumIT(6) = glRegistNumIT(6) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 7)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*7)　<　差の場合
                    glRegistNumIT(7) = glRegistNumIT(7) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 8)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*8)　<　差の場合
                    glRegistNumIT(8) = glRegistNumIT(8) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 9)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*9)　<　差の場合
                    glRegistNumIT(9) = glRegistNumIT(9) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 10)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*10)　<　差の場合
                    glRegistNumIT(10) = glRegistNumIT(10) + 1
                Else
                    ' 上記条件以外の場合
                    glRegistNumIT(11) = glRegistNumIT(11) + 1
                End If

                ' ﾄﾘﾐﾝｸﾞ結果が1(OK)または12(IT OKTEST OK(SL436K))であるかﾁｪｯｸする。
                ''V4.2.0.0④ ↓
                'If (gwTrimResult(intForCnt - 1) = 1) Or (gwTrimResult(intForCnt - 1) = 12) Then
                If (gwTrimResult(intForCnt - 1) = 1) Or (gwTrimResult(intForCnt - 1) = 12) _
                    Or (gwTrimResult(intForCnt - 1) = RSLT_FT_LONG) Or (gwTrimResult(intForCnt - 1) = RSLT_FT_HING) Then
                    ''V4.2.0.0④ ↑
                    glITTOTAL = glITTOTAL + 1                           ' IT計算対象数 ###138
                    dblGapIT = dblGapIT + dblGap                        ' 積算誤差
                    dblX_2IT = dblX_2IT + (dblGap * dblGap)

                    '----- V6.0.3.0_26↓ -----
                    If digL = 0 Or digL = 1 Then
                        '正常にトリミングできた抵抗の抵抗値取得 
                        TotalCntTrimming = TotalCntTrimming + 1
                        SetAverageFTValue(gfFinalTest(intForCnt - 1), TotalCntTrimming)
                    End If
                    '----- V6.0.3.0_26↑ -----

                    gITNx_cnt = gITNx_cnt + 1
                    'ReDim Preserve gITNx(gITNx_cnt)                     ' 配列の作成                  '###154
                    ''(標準偏差算出式修正)
                    'gITNx(gITNx_cnt) = dblGap                           ' 測定誤差ｾｯﾄ                 '###154
                    Average = GetAverageIT(dblGap, gITNx_cnt + 1)                                      '###154
                    dblDeviationIT = GetDeviationIT(dblGap, gITNx_cnt + 1, Average)                    '###154
                    dblAverageIT = Average                                                             '###154
                    If (1 = glITTOTAL) Then                             ' ###138
                        dblMinIT = dblGap
                        dblMaxIT = dblGap
                    End If
                    If (dblMinIT > dblGap) Then
                        dblMinIT = dblGap
                    End If
                    If (dblMaxIT < dblGap) Then
                        dblMaxIT = dblGap
                    End If
                Else
                    'NGカウント数を記録
                    gITNg_cnt = gITNg_cnt + 1
                End If
            End If
        Next

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingGraph_RegistNumIT" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "ファイナルテストテスト分布図"
    '''=========================================================================
    '''<summary>ファイナルテストテスト分布図</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingGraph_RegistNumFT()

        On Error GoTo ErrExit

        Dim intForCnt As Short
        Dim dblGraphDiv As Double                                       ' グラフ範囲刻み値
        Dim dblGraphTop As Double                                       ' グラフ最上段値
        Dim dblGap As Double
        Dim dblGapFT As Double                                          ' 積算誤差ﾌｧｲﾅﾙ
        Dim dblX_2FT As Double                                          ' FT標準偏差算出用ワーク
        Dim Average As Double                                           ' 平均用　###154
        'Dim lFTTOTAL As Integer                                        ' FT計算対象数 ###138
        dblX_2FT = 0
        'lFTTOTAL = 0                                                   ' ###138

        ' ﾌｧｲﾅﾙﾃｽﾄ(LOWﾘﾐｯﾄ)とﾌｧｲﾅﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)の値をﾁｪｯｸする。
        If ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
            ' ﾌｧｲﾅﾙﾃｽﾄ(LOWﾘﾐｯﾄ)が0以下でﾌｧｲﾅﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)が0以上の場合
            dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5

        ElseIf ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 > typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
            ' ﾌｧｲﾅﾙﾃｽﾄ(LOWﾘﾐｯﾄ)が0以下でﾌｧｲﾅﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)がより小さい場合
            dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit / 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5

        ElseIf ((0 < typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
            ' ﾌｧｲﾅﾙﾃｽﾄ(LOWﾘﾐｯﾄ)が0より大きくてﾌｧｲﾅﾙﾃｽﾄ(HIGHﾘﾐｯﾄ)が0以上の場合
            dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit / 1.5) / 10
            dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
        Else
            ' 上記条件以外の場合
            dblGraphDiv = 0.3
            dblGraphTop = 1.5
        End If

        ' 1ｸﾞﾙｰﾌﾟ内抵抗数分処理を行なう。
        For intForCnt = 1 To gRegistorCnt
            ' ﾄﾘﾐﾝｸﾞ結果が1(OK), 8(FT HI NG), 9(FT LO NG)のいずれかでRSLT_OPENCHK_NG(20),RSLT_SHORTCHK_NG(21)以外の場合
            If (gwTrimResult(intForCnt - 1) = RSLT_OK Or gwTrimResult(intForCnt - 1) = RSLT_FT_HING Or gwTrimResult(intForCnt - 1) = RSLT_FT_LONG) _
                And ((gwTrimResult(intForCnt - 1) <> RSLT_OPENCHK_NG) Or (gwTrimResult(intForCnt - 1) <> RSLT_SHORTCHK_NG)) Then
                ' 差を算出する。　ﾌｧｲﾅﾙﾃｽﾄ結果/ﾄﾘﾐﾝｸﾞ目標値*100　-　100
                dblGap = (gfFinalTest(intForCnt - 1) / typResistorInfoArray(intForCnt).dblTrimTargetVal) * 100.0# - 100.0#

                If ((dblGraphTop - (dblGraphDiv * 0)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*0)　<　差の場合
                    glRegistNumFT(0) = glRegistNumFT(0) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 1)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*1)　<　差の場合
                    glRegistNumFT(1) = glRegistNumFT(1) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 2)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*2)　<　差の場合
                    glRegistNumFT(2) = glRegistNumFT(2) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 3)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*3)　<　差の場合
                    glRegistNumFT(3) = glRegistNumFT(3) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 4)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*4)　<　差の場合
                    glRegistNumFT(4) = glRegistNumFT(4) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 5)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*5)　<　差の場合
                    glRegistNumFT(5) = glRegistNumFT(5) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 6)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*6)　<　差の場合
                    glRegistNumFT(6) = glRegistNumFT(6) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 7)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*7)　<　差の場合
                    glRegistNumFT(7) = glRegistNumFT(7) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 8)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*8)　<　差の場合
                    glRegistNumFT(8) = glRegistNumFT(8) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 9)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*9)　<　差の場合
                    glRegistNumFT(9) = glRegistNumFT(9) + 1
                ElseIf ((dblGraphTop - (dblGraphDiv * 10)) < dblGap) Then
                    ' ｸﾞﾗﾌ最上段値-(ｸﾞﾗﾌ範囲刻み位置*10)　<　差の場合
                    glRegistNumFT(10) = glRegistNumFT(10) + 1
                Else
                    ' 上記条件以外の場合
                    glRegistNumFT(11) = glRegistNumFT(11) + 1
                End If

                ' ﾄﾘﾐﾝｸﾞ結果が1(OK)であるかﾁｪｯｸする。
                If (gwTrimResult(intForCnt - 1) = 1) Then
                    glFTTOTAL = glFTTOTAL + 1                           ' FT計算対象数 ###138
                    dblGapFT = dblGapFT + dblGap                        ' 積算誤差
                    dblX_2FT = dblX_2FT + (dblGap * dblGap)

                    gFTNx_cnt = gFTNx_cnt + 1
                    'ReDim Preserve gFTNx(gFTNx_cnt)                     ' 配列の作成      '###154
                    'gFTNx(gFTNx_cnt) = dblGap                           ' 測定誤差ｾｯﾄ     '###154
                    'GetDeviationFTOrg(gFTNx, gFTNx_cnt + 1, Average)                      '###154
                    Average = GetAverageFT(dblGap, gFTNx_cnt + 1)                          '###154
                    dblDeviationFT = GetDeviationFT(dblGap, gFTNx_cnt + 1, Average)        '###154
                    dblAverageFT = Average                                                 '###154
                    '(標準偏差算出式修正)
                    If (1 = glFTTOTAL) Then                             ' ###138
                        dblMinFT = dblGap
                        dblMaxFT = dblGap
                    End If
                    If (dblMinFT > dblGap) Then
                        dblMinFT = dblGap
                    End If
                    If (dblMaxFT < dblGap) Then
                        dblMaxFT = dblGap
                    End If
                Else
                    'NGカウント数を記録
                    gFTNg_cnt = gFTNg_cnt + 1
                End If
            End If
        Next

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingGraph_RegistNumFT" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "トリミング結果を出力する"
    '''=========================================================================
    ''' <summary>トリミング結果を出力する</summary>
    ''' <param name="digH">         (INP)</param>
    ''' <param name="digL">         (INP)</param>
    ''' <param name="strFileLogMsg">(OUT)</param>
    ''' <param name="strDispLogMsg">(OUT)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_LoggingDataMain(ByVal digH As Integer, ByVal digL As Integer,
                                            ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                            ByVal strFileLogMsg As StringBuilder, ByVal strDispLogMsg As StringBuilder)
        '#4.12.2.0⑥ blkNoX,blkNoY追加
        '#4.12.2.0④    Private Sub TrimLogging_LoggingDataMain(ByVal digH As Integer, ByVal digL As Integer,
        '#4.12.2.0④                ByRef strFileLogMsg As String, ByRef strDispLogMsg As String)

        '#4.12.2.0①        On Error GoTo ErrExit
        Try                             '#4.12.2.0①
            Dim intForCnt As Short
            Dim strchKugiri As String
            Dim strchFloatPoint As String
            Dim calcWork As Integer

            ' ﾒｯｾｰｼﾞ言語が欧州以外であるかﾁｪｯｸする。
            If gSysPrm.stTMN.giMsgTyp <> 2 Then
                strchKugiri = ","
                strchFloatPoint = "."
            Else
                strchKugiri = Chr(KUGIRI_CHAR) ' TAB
                strchFloatPoint = ","
            End If

            If gKeiTyp = KEY_TYPE_RS Then              'V2.0.0.0⑩ シンプルトリマ 目標値、規格値の設定
                SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)    'V2.0.0.0⑩
            End If                                      'V2.0.0.0⑩
            '
            ' 抵抗数分、初期化処理を行なう。
            '' ''For intForCnt = 1 To gRegistorCnt
            '' ''giTrimResult0x(intForCnt - 1) = 0
            'V4.1.0.0⑤
            '' ''If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE Then     ' ｵｰﾊﾞｰﾚﾝｼﾞ
            '' ''    gfInitialTest(intForCnt - 1) = 9999999999.9
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''ElseIf gwTrimResult(intForCnt - 1) = 11 Then                    ' 4端子ｵｰﾌﾟﾝ
            '' ''    gfInitialTest(intForCnt - 1) = 0
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''ElseIf gwTrimResult(intForCnt - 1) = 12 Then                    ' ｲﾆｼｬﾙOKﾃｽﾄ
            '' ''    gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
            '' ''ElseIf gwTrimResult(intForCnt - 1) = 13 Then                    ' 再ﾌﾟﾛーﾋﾞﾝｸﾞ
            '' ''    gfInitialTest(intForCnt - 1) = 9999999999.9
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''    'ElseIf (m_intxmode <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
            '' ''ElseIf (digL <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
            '' ''                                Or (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG)) Then  ' ｲﾆｼｬﾙNG
            '' ''    gfFinalTest(intForCnt - 1) = 0
            '' ''    'ElseIf m_intxmode = 2 Then                                 ' 上記ｴﾗｰ以外のx2
            '' ''ElseIf digL = 2 Then                                            ' 上記ｴﾗｰ以外のx2
            '' ''    gfInitialTest(intForCnt - 1) = gfFinalTest(intForCnt - 1)
            '' ''    'ElseIf m_intxmode = 3 Then                                 ' 上記ｴﾗｰ以外のx3
            '' ''ElseIf digL = 3 Then                                            ' 上記ｴﾗｰ以外のx3
            '' ''    '(2011/06/25) 下記の値は取得していたとしても同じ値のはず。
            '' ''    '   また、x3モードではファイナルの値だけを取得するようにするので,
            '' ''    '   下記の処理は不要（値がクリアされてしまう可能性がある。
            '' ''    'gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
            '' ''End If
            '' ''Next intForCnt
            ''V4.1.0.0⑤
            ' ﾒｯｾｰｼﾞ用変数の初期化処理を行なう。
            '#4.12.2.0④        strFileLogMsg = ""
            strFileLogMsg.Length = 0                '#4.12.2.0④ 
            '#4.12.2.0④        strDispLogMsg = ""
            strDispLogMsg.Length = 0                '#4.12.2.0④

            ' 抵抗数分処理を行なう。
            For intForCnt = 1 To gRegistorCnt
                ''V4.1.0.0⑤
                ' 同じループで実行してみる
                giTrimResult0x(intForCnt - 1) = 0

                If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE Then     ' ｵｰﾊﾞｰﾚﾝｼﾞ
                    gfInitialTest(intForCnt - 1) = 9999999999.9
                    gfFinalTest(intForCnt - 1) = 0
                ElseIf gwTrimResult(intForCnt - 1) = 11 Then                    ' 4端子ｵｰﾌﾟﾝ
                    gfInitialTest(intForCnt - 1) = 0
                    gfFinalTest(intForCnt - 1) = 0
                ElseIf gwTrimResult(intForCnt - 1) = 12 Then                    ' ｲﾆｼｬﾙOKﾃｽﾄ
                    gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
                ElseIf gwTrimResult(intForCnt - 1) = 13 Then                    ' 再ﾌﾟﾛーﾋﾞﾝｸﾞ
                    gfInitialTest(intForCnt - 1) = 9999999999.9
                    gfFinalTest(intForCnt - 1) = 0
                    'ElseIf (m_intxmode <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
                ElseIf (digL <> 3) And ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING) _
                                            Or (gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG)) Then  ' ｲﾆｼｬﾙNG
                    gfFinalTest(intForCnt - 1) = 0
                    'ElseIf m_intxmode = 2 Then                                 ' 上記ｴﾗｰ以外のx2
                ElseIf digL = 2 Then                                            ' 上記ｴﾗｰ以外のx2
                    gfInitialTest(intForCnt - 1) = gfFinalTest(intForCnt - 1)
                    'ElseIf m_intxmode = 3 Then                                 ' 上記ｴﾗｰ以外のx3
                ElseIf digL = 3 Then                                            ' 上記ｴﾗｰ以外のx3
                    '(2011/06/25) 下記の値は取得していたとしても同じ値のはず。
                    '   また、x3モードではファイナルの値だけを取得するようにするので,
                    '   下記の処理は不要（値がクリアされてしまう可能性がある。
                    'gfFinalTest(intForCnt - 1) = gfInitialTest(intForCnt - 1)
                End If

                ''V4.1.0.0⑤

                'V5.0.0.4⑥ If gKeiTyp = KEY_TYPE_RS Then              'V2.0.0.0⑩ シンプルトリマ
                If gKeiTyp = KEY_TYPE_RS And (Val(CStr(typResistorInfoArray(intForCnt).intResNo)) < 1000) Then              'V2.0.0.0⑩ シンプルトリマ
                    'SimpleTrimmer.TrimData.SetResData(intForCnt, gfInitialTest(intForCnt - 1), gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1))   'V2.0.0.0⑩
                    '                SimpleTrimmer.TrimData.SetResData(intForCnt, gfInitialTest(intForCnt - 1), gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1), digL)       'V4.0.0.0-23
                    If digL = 2 Then                                            ' 上記ｴﾗｰ以外のx2
                        SimpleTrimmer.TrimData.SetResData(intForCnt, 0, gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1), digL)       'V4.0.0.0-23
                    Else
                        SimpleTrimmer.TrimData.SetResData(intForCnt, gfInitialTest(intForCnt - 1), gfFinalTest(intForCnt - 1), gwTrimResult(intForCnt - 1), digL)       'V4.0.0.0-23

                    End If

                End If                                      'V2.0.0.0⑩
                ' 抵抗数ｶｳﾝﾀ=0の時だけ処理を行なう。
                '==========================================
                ' 画面表示用のデータ文字列を生成
                '==========================================

                If (digH = 0) Then
                    '表示なしなら何も表示しない
                Else
                    '---------------------------------------
                    '表示モード（デジタルの上位設定)
                    '   digH=0:表示しない。
                    '   digH=1-digL=0～2:NG抵抗のみ表示する。
                    '---------------------------------------
                    If intForCnt = 1 Then
                        ' 画面ログのヘッダー部分の取得
                        Call TrimLogging_MakeLogDisplayHeader(digH, digL, strDispLogMsg)
                    End If

                    If (digH = 1) And (digL <= 2) Then
                        calcWork = intForCnt Mod m_TrimDisp1xFormat(0)
                        If calcWork <> 0 Then
                            Call TrimLogging_MakeLogStringData(m_TrimDisp1xFormat, digH, digL, blkNoX, blkNoY,
                                        intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)
                        Else
                            Call TrimLogging_MakeLogStringData(m_TrimDisp1xFormat, digH, digL, blkNoX, blkNoY,
                                        intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)
                        End If
                    Else
                        ' 標準表示(3×、4×)/0×、1×表示
                        Select Case digL
                            Case 0
                                calcWork = intForCnt Mod m_TrimDispx0Format(0)
                                If calcWork <> 0 Then
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)   '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, False)      '###238
                                Else
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)    '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx0Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, True)       '###238
                                End If
                            Case 1, 2
                                calcWork = intForCnt Mod m_TrimDispx1Format(0)
                                If calcWork <> 0 Then
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)   '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, False)      '###238
                                Else
                                    'Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)    '###238
                                    Call TrimLogging_MakeLogStringData(m_TrimDispx1Format, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, True)       '###238
                                End If
                            Case 3
                                calcWork = intForCnt Mod m_MeasDispFormat(0)
                                If calcWork <> 0 Then
                                    'Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, False)   '###238
                                    Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, False)      '###238
                                Else
                                    'Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, _
                                    '                intForCnt, vbTab, strchFloatPoint, strDispLogMsg, True)    '###238
                                    Call TrimLogging_MakeLogStringData(m_MeasDispFormat, digH, digL, blkNoX, blkNoY,
                                                intForCnt, " ", strchFloatPoint, strDispLogMsg, True)       '###238
                                End If
                        End Select
                    End If
                End If

                '==========================================================
                '   OK/NGカウントの算出(x0,x1,x2モード時)
                '==========================================================
                If digL <= 2 Then
                    '----- ###167↓ -----
                    ' NETとTKYはTrimLoggingCircuit_OKNG()でサーキット単位でOK/NGカウントを求めているのでSKIP
                    If (gTkyKnd = KND_NET) Or (gTkyKnd = KND_TKY) Then
                        GoTo STP_MAKELOG
                    End If
                    '----- ###167↑ -----
                    'V6.1.1.0⑮↓
                    ' 抵抗数の中にNGマーキングの数も含まれていたので、1000以降のときはカウントしない
                    If (Val(CStr(typResistorInfoArray(intForCnt).intResNo)) >= 1000) Then
                        Continue For
                    End If
                    'V6.1.1.0⑮↑

                    ' ﾄﾘﾐﾝｸﾞ結果がOKであるかﾁｪｯｸする。
                    If gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK Or gwTrimResult(intForCnt - 1) = 12 Then
                        m_lGoodCount = m_lGoodCount + 1                         ' OKカウント更新
                        '----- V1.18.0.0③↓ .Trim_OKは未使用のため削除 -----
                        'If (gTkyKnd = KND_CHIP) Then
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Trim_OK = stPRT_ROHM.Trim_OK + 1    ' 良品ﾁｯﾌﾟ数
                        '    End If
                        'End If
                        '----- V1.18.0.0③↑ -----
                        ' ﾄﾘﾐﾝｸﾞ結果がNGであるかﾁｪｯｸする。
                    ElseIf gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_RATIO _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_IT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERRANGE _
                        Or gwTrimResult(intForCnt - 1) = 11 _
                        Or gwTrimResult(intForCnt - 1) = 13 _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_CVERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_OVERLOAD _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_ES2ERR _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO _
                        Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_MIDIUM_CUT_NG Then     ' 'V1.20.0.1① ' V1.13.0.0⑪
                        m_lNgCount = m_lNgCount + 1                             ' NGカウント更新
                        'm_lTrimResult = cFRS_TRIM_NG                           ' 基板単位のトリミング結果(SL436R用) = トリミングNG ###142 ###089
                        m_NG_RES_Count = m_NG_RES_Count + 1                     ' NG判定(0-100%)用NG抵抗数(ブロック単位/プレート単位) ###142 
                        '----- V1.18.0.0③↓ .Trim_NGは未使用のため削除 -----
                        'If (gTkyKnd = KND_CHIP) Then
                        '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                        '        stPRT_ROHM.Trim_NG = stPRT_ROHM.Trim_NG + 1     ' 不良品ﾁｯﾌﾟ数
                        '    End If
                        'End If
                        '----- V1.18.0.0③↑ -----

                        m_NgCountInPlate = m_NgCountInPlate + 1     'V4.5.0.5① １基板内NG数のカウント 'V4.12.2.0⑨　'V6.0.5.0④

                    End If
                End If

STP_MAKELOG:  ' ###167
                '==========================================
                ' ログファイル出力用のデータ文字列を生成
                '==========================================
                If Not (Val(CStr(typResistorInfoArray(intForCnt).intResNo)) >= 1000) Then

                    'ファイルへの出力はフォーマットをあわせるため、TrimModeのみとする。
                    calcWork = intForCnt Mod m_TrimlogFileFormat(0)
                    If calcWork <> 0 Then
                        Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, digH, digL, blkNoX, blkNoY,
                                    intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, False)
                    Else
                        Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, digH, digL, blkNoX, blkNoY,
                                   intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, True)
                    End If
                    '' '' ''Select Case digL
                    '' '' ''    Case 3
                    '' '' ''        calcWork = intForCnt Mod m_MeasDispFormat(0)
                    '' '' ''        ' ファイルログ(×3モードの場合)
                    '' '' ''        If calcWork <> 0 Then
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_MeaslogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, False)
                    '' '' ''        Else
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_MeaslogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, True)
                    '' '' ''        End If
                    '' '' ''    Case Else
                    '' '' ''        ' ファイルログ(×0、×1、×2モードの場合)
                    '' '' ''        If calcWork <> 0 Then
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, False)
                    '' '' ''        Else
                    '' '' ''            Call TrimLogging_MakeLogStringData(m_TrimlogFileFormat, _
                    '' '' ''                            intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg, True)
                    '' '' ''        End If
                    '' '' ''End Select
                    'Call TrimLogging_MakeLogStringData(digL, m_logFileFormat, intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg)

                    ' ﾃﾞｼﾞSWの1桁目をﾁｪｯｸする。
                    'Select Case m_intxmode
                    'Select Case digL
                    '    Case 3
                    '        ' ファイルログ(×3モードの場合)
                    '        Call TrimLogging_FileLoggingData_mode3(intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg)
                    '    Case Else
                    '        ' ファイルログ(×0、×1、×2モードの場合)
                    '        Call TrimLogging_FileLoggingData(intForCnt, strchKugiri, strchFloatPoint, strFileLogMsg)
                    'End Select
                    'End If                                                     '###012 

                    ' 電圧測定値がシスパラの最大電圧値を超えたらIT測定値を9999999999.9にする
                    If (gTkyKnd = KND_TKY) Or (gTkyKnd = KND_NET) Then
                        'If typPlateInfo.intMeasType = 1 Then                           ' ###166
                        If (typResistorInfoArray(intForCnt).intResMeasMode = 1) Then    ' ###166 測定モード = 1(電圧) ?
                            If gfInitialTest(intForCnt - 1) > gSysPrm.stMES.gdVOLTAGE_MAX Then
                                gfInitialTest(intForCnt - 1) = 9999999999.9
                            End If
                            If gfFinalTest(intForCnt - 1) > gSysPrm.stMES.gdVOLTAGE_MAX Then
                                gfFinalTest(intForCnt - 1) = 9999999999.9
                            End If
                            If gfInitialTest(intForCnt - 1) < gSysPrm.stMES.gdVOLTAGE_MIN Then
                                gfInitialTest(intForCnt - 1) = -9999999999.9
                            End If
                            If gfFinalTest(intForCnt - 1) < gSysPrm.stMES.gdVOLTAGE_MIN Then
                                gfFinalTest(intForCnt - 1) = -9999999999.9
                            End If
                        End If
                    End If
                End If                                                          '###012 

            Next

            ' '' '' ''表示用ログの最後の改行を削除
            ' '' '' ''strDispLogMsg = strDispLogMsg & vbCrLf
            '' '' ''Dim lastVbCrlfPos As Integer
            '' '' ''Dim length As Integer
            '' '' ''lastVbCrlfPos = strDispLogMsg.LastIndexOf("%")
            '' '' ''length = strDispLogMsg.Length
            '' '' ''strDispLogMsg.Remove(lastVbCrlfPos - 1, length - lastVbCrlfPos + 1)
            '' '' '' '' '' ''lastKugiriPos = InStrRev(strLogMsg, strchKugiri)
            '' '' '' '' '' ''Replace(strDispLogMsg, vbCrLf, "", lastVbCrlfPos)

            '#4.12.2.0①            Exit Sub
        Catch ex As Exception           '#4.12.2.0①
            '#4.12.2.0①ErrExit:
            MsgBox("TrimLogging_LoggingDataMain" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

    End Sub
#End Region

#Region "トリミング結果ヘッダー出力文字列を構築する。"
    '''=========================================================================
    ''' <summary>トリミング結果ヘッダー出力の文字列を構築する。
    ''' 設定に従い、ヘッダ部分の詳細なフォーマットを作成する。</summary>
    ''' <param name="logTarget"></param>
    ''' <param name="logType"></param>
    ''' <param name="separator"></param>
    ''' <param name="strLogMsg"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogHeader(ByVal logTarget() As Integer, ByVal logType As Integer,
                                          ByVal separator As String, ByVal strLogMsg As StringBuilder)
        '#4.12.2.0④    Private Sub TrimLogging_MakeLogHeader(ByVal logTarget() As Integer, ByVal logType As Integer,
        '#4.12.2.0④                                        ByVal separator As String, ByRef strLogMsg As String)

        '#4.12.2.0④        On Error GoTo ErrExit

        '#4.12.2.0④        Dim strMakeMsg As String
        Dim cnt As Integer
        Dim logResCnt As Integer

        'strLogMsg = strLogMsg & vbCrLf
        Try                             '#4.12.2.0④
            For logResCnt = 1 To logTarget(0)
                For cnt = 1 To LOGTAR.END
                    ' 標準形式のファイルログ
                    Select Case logTarget(cnt)
                        Case LOGTAR.DATE
                            If logType = LOGTYPE_DISP Then
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "DATE" & separator & separator & separator
                                strLogMsg.Append("DATE" & separator & separator & separator)            '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "DATE" & separator
                                strLogMsg.Append("DATE" & separator)                                    '#4.12.2.0④
                            End If
                        Case LOGTAR.MODE
                            '#4.12.2.0④                        strLogMsg = strLogMsg & "MODE" & separator
                            strLogMsg.Append("MODE" & separator)                                        '#4.12.2.0④
                        Case LOGTAR.LOTNO
                            '#4.12.2.0④                        strLogMsg = strLogMsg & "LOT-NO" & separator
                            strLogMsg.Append("LOT-NO" & separator)                                      '#4.12.2.0④

                        Case LOGTAR.BLOCKX                              '#4.12.2.0④
                            strLogMsg.Append("BLOCK-X" & separator)
                        Case LOGTAR.BLOCKY                              '#4.12.2.0④
                            strLogMsg.Append("BLOCK-Y" & separator)

                        Case LOGTAR.CIRCUIT
                            'strLogMsg = strLogMsg & "CIR-NO" & separator           '###238
                            If logType = LOGTYPE_DISP Then
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "CNo " & separator          '###238 4桁左詰め
                                strLogMsg.Append("CNo " & separator)        '###238 4桁左詰め           '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "CIR-NO" & separator
                                strLogMsg.Append("CIR-NO" & separator)                                  '#4.12.2.0④
                            End If
                        Case LOGTAR.RESISTOR
                            'strLogMsg = strLogMsg & "RES-NO" & separator           '###238
                            If logType = LOGTYPE_DISP Then
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "RNo " & separator          '###238 4桁左詰め
                                strLogMsg.Append("RNo " & separator)        '###238 4桁左詰め           '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "RES-NO" & separator
                                strLogMsg.Append("RES-NO" & separator)                                  '#4.12.2.0④
                            End If
                        Case LOGTAR.JUDGE
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "RESULT  " & separator     '###238
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "RESULT    " & separator    '###238 10文字左詰め
                                strLogMsg.Append("RESULT    " & separator)  '###238 10文字左詰め         '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "RESULT  " & separator
                                strLogMsg.Append("RESULT  " & separator)                                '#4.12.2.0④
                            End If
                        Case LOGTAR.TARGET
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "TARGET" & separator & separator '###238 
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "   TARGGET      " & separator    '###238 16文字
                                strLogMsg.Append("   TARGET       " & separator)    '###238 16文字      '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "TARGET" & separator
                                strLogMsg.Append("TARGET" & separator)                                  '#4.12.2.0④
                            End If
                        Case LOGTAR.INITIAL
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "INIVAL" & separator & separator '###238 
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "    INIVAL      " & separator    '###238 16文字
                                strLogMsg.Append("    INIVAL      " & separator)    '###238 16文字      '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "INIVAL" & separator
                                strLogMsg.Append("INIVAL" & separator)                                  '#4.12.2.0④
                            End If
                        Case LOGTAR.FINAL
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "FINVAL" & separator & separator '###238 
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "    FINVAL      " & separator    '###238 16文字
                                strLogMsg.Append("    FINVAL      " & separator)    '###238 16文字      '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "FINVAL" & separator
                                strLogMsg.Append("FINVAL" & separator)                                  '#4.12.2.0④
                            End If
                        Case LOGTAR.DEVIATION
                            If logType = LOGTYPE_DISP Then
                                'strLogMsg = strLogMsg & "DEVIAT" & separator & separator   '###238 
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "  DEVIAT" & separator              '###238 
                                strLogMsg.Append("  DEVIAT" & separator)            '###238             '#4.12.2.0④
                            Else
                                '#4.12.2.0④                            strLogMsg = strLogMsg & "DEVIAT" & separator
                                strLogMsg.Append("DEVIAT" & separator)                                  '#4.12.2.0④
                            End If
                        Case LOGTAR.UCUTPRMNO
                            '#4.12.2.0④                        strLogMsg = strLogMsg & "UcutPrmNo" & separator
                            strLogMsg.Append("UcutPrmNo" & separator)                                   '#4.12.2.0④
                        Case LOGTAR.NOSET
                        '何もしない
                        Case LOGTAR.END
                            Exit For
                    End Select
                Next
            Next
            'ヘッダーの最後尾には改行コードを追加する
            '#4.12.2.0④        strLogMsg = strLogMsg & vbCrLf
            strLogMsg.AppendLine()          '#4.12.2.0④

            '#4.12.2.0④            Exit Sub

        Catch ex As Exception
            '@@@20170810ErrExit:
            MsgBox("TrimLogging_MakeLogHeader" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        End Try

    End Sub
#End Region

#Region "ログ出力用のヘッダ文字列を構築する"
    '''=========================================================================
    '''<summary>ログ出力用のヘッダ文字列を構築する。
    ''' strLogMs2に測定結果表示のヘッダー部分の設定を行う。</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogDisplayHeader(ByVal digH As Integer, ByVal digL As Integer, ByVal strLogHeader As StringBuilder)
        '#4.12.2.0④    Private Sub TrimLogging_MakeLogDisplayHeader(ByVal digH As Integer, ByVal digL As Integer, ByRef strLogHeader As String)

        Dim strCircuit As String
        Dim addRetFlg As Integer

        On Error GoTo ErrExit

        If (gTkyKnd = KND_TKY Or gTkyKnd = KND_NET) Then
            strCircuit = "CIRCUIT" & vbTab
        Else
            strCircuit = ""
        End If

        'If (digH = 1) And (digL <= 2) Then
        If (digH = 1) And (digL <= 3) Then                                                                  '###217

            ' ﾃﾞｼﾞSWの1桁目のﾁｪｯｸを行う
            Select Case digL
                Case 0, 1, 2                                                                                '###217
                    '1x用に設定を変更してヘッダーを構築
                    Call TrimLogging_MakeLogHeader(m_TrimDisp1xFormat, LOGTYPE_DISP, vbTab, strLogHeader)
            End Select                                                                                      '###217

        Else
            ' ﾃﾞｼﾞSWの1桁目のﾁｪｯｸを行う
            Select Case digL
                Case 0
                    'x0用に設定を変更してヘッダーを構築
                    'Call TrimLogging_MakeLogHeader(m_TrimDispx0Format, LOGTYPE_DISP, vbTab, strLogHeader)  '###238
                    Call TrimLogging_MakeLogHeader(m_TrimDispx0Format, LOGTYPE_DISP, " ", strLogHeader)     '###238
                Case 1, 2
                    'x1用に設定を変更してヘッダーを構築
                    'Call TrimLogging_MakeLogHeader(m_TrimDispx1Format, LOGTYPE_DISP, vbTab, strLogHeader)  '###238
                    Call TrimLogging_MakeLogHeader(m_TrimDispx1Format, LOGTYPE_DISP, " ", strLogHeader)     '###238
                Case 3
                    'x3用に設定を変更してヘッダーを構築
                    'Call TrimLogging_MakeLogHeader(m_MeasDispFormat, LOGTYPE_DISP, vbTab, strLogHeader)    '###238
                    Call TrimLogging_MakeLogHeader(m_MeasDispFormat, LOGTYPE_DISP, " ", strLogHeader)       '###238
            End Select
        End If

        Exit Sub

ErrExit:
        MsgBox("TrimLogging_MakeLogDisplayHeader" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "トリミング結果出力文字列を構築する"
    '''=========================================================================
    ''' <summary>トリミング結果出力文字列を構築する</summary>
    ''' <param name="logTarget">      </param>
    ''' <param name="digH">           </param>
    ''' <param name="digL">           </param>
    ''' <param name="blkNoX"></param>
    ''' <param name="blkNoY"></param>
    ''' <param name="intForCnt">      </param>
    ''' <param name="strchKugiri">    </param>
    ''' <param name="strchFloatPoint"></param>
    ''' <param name="strLogMsg">      </param>
    ''' <param name="addRetFlg">      </param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogStringData(ByVal logTarget() As Integer,
                                              ByVal digH As Integer, ByVal digL As Integer,
                                              ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                              ByVal intForCnt As Short, ByVal strchKugiri As String, ByVal strchFloatPoint As String,
                                              ByVal strLogMsg As StringBuilder, ByVal addRetFlg As Boolean)
        '#4.12.2.0⑥ blkNoX,blkNoY追加
        '#4.12.2.0④    Private Sub TrimLogging_MakeLogStringData(ByVal logTarget() As Integer, ByVal digH As Integer, ByVal digL As Integer,
        '#4.12.2.0④                                    ByVal intForCnt As Short, ByVal strchKugiri As String, ByVal strchFloatPoint As String,
        '#4.12.2.0④                                    ByRef strLogMsg As String, ByVal addRetFlg As Boolean)

        On Error GoTo ErrExit

        Dim strMakeMsg As String = ""
        Dim dblDiff As Double
        Dim strBuf As String
        Dim cnt As Integer
        Dim Rn As Short
        'Dim curTime As DateTime = DateTime.Now
        Dim curTime As String
        Dim NowBlockNo As Integer 'V4.3.0.0①


        curTime = Today.ToString("yyMMdd") & " " & TimeOfDay.ToString("HHmmss")

        ' マーキング抵抗はSkip @@@007
        If (typResistorInfoArray(intForCnt).intResNo >= 1000) Then
            Exit Sub
        End If

        ' NGのみ表示の場合
        If (digH = 1) Then
            'OKまたは未実施の場合はログ文字列を追加しない
            If ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_NOTDO) Or
                 (gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK) Or
                 (gwTrimResult(intForCnt - 1) = TRIM_RESULT_SKIP)) Then
                Exit Sub
            End If
        End If

        '----- V6.0.3.0⑬↓ -----
        Dim DecPoint As Integer

        ' 少数点以下の桁数を求める (gsEDIT_DIGITNUM("0.00000")'
        DecPoint = GetDecPointSize(gsEDIT_DIGITNUM)
        '----- V6.0.3.0⑬↑ -----

        For cnt = 1 To LOGTAR.END
            ' 標準形式のファイルログ
            Select Case logTarget(cnt)
                Case LOGTAR.DATE
                    '日付データの追加
                    '#4.12.2.0④                    strLogMsg = strLogMsg & curTime & strchKugiri
                    strLogMsg.Append(curTime & strchKugiri)                         '#4.12.2.0④

                Case LOGTAR.MODE
                    'トリミングモードの追加
                    If (digL <> 3) Then
                        '#4.12.2.0④                        strLogMsg = strLogMsg & "TRIMx" & digL & strchKugiri
                        strLogMsg.Append("TRIMx" & digL & strchKugiri)              '#4.12.2.0④
                    Else
                        '#4.12.2.0④                        strLogMsg = strLogMsg & "MEAS" & strchKugiri
                        strLogMsg.Append("MEAS" & strchKugiri)                      '#4.12.2.0④
                    End If

                Case LOGTAR.LOTNO
                    'ロットデータの追加
                    '#4.12.2.0④                    strLogMsg = strLogMsg & gSysPrm.stLOG.giLoggingLotNo.ToString() & strchKugiri
                    strLogMsg.Append(gSysPrm.stLOG.giLoggingLotNo & strchKugiri)    '#4.12.2.0④

                Case LOGTAR.BLOCKX                                      '#4.12.2.0⑥
                    ' ブロック番号Ｘの追加
                    strLogMsg.Append(blkNoX & strchKugiri)
                Case LOGTAR.BLOCKY                                      '#4.12.2.0⑥
                    ' ブロック番号Ｙの追加
                    strLogMsg.Append(blkNoY & strchKugiri)

                Case LOGTAR.CIRCUIT
                    '----- ###238↓ -----
                    'サーキットデータの追加
                    'strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intCircuitGrp.ToString() & strchKugiri
                    If (strchKugiri = ",") Then
                        'V4.3.0.0①↓
                        If gKeiTyp = KEY_TYPE_RS Then
                            'SimpleTrimmer .SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit)    'V2.0.0.0⑩
                            SimpleTrimmer.GetBlockDisplayNumber(NowBlockNo)
                            ' 区切り文字が","ならログファイル出力用データと判断し4桁左詰めとしない
                            '#4.12.2.0④                            strLogMsg = strLogMsg & NowBlockNo.ToString() & strchKugiri
                            strLogMsg.Append(NowBlockNo & strchKugiri)              '#4.12.2.0④
                        Else
                            ' 区切り文字が","ならログファイル出力用データと判断し4桁左詰めとしない
                            '#4.12.2.0④                            strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intCircuitGrp.ToString() & strchKugiri
                            strLogMsg.Append(typResistorInfoArray(intForCnt).intCircuitGrp & strchKugiri)   '#4.12.2.0④
                        End If
                        'V4.3.0.0①↑
                    Else
                        'V4.3.0.0①↓
                        If gKeiTyp = KEY_TYPE_RS Then
                            SimpleTrimmer.GetBlockDisplayNumber(NowBlockNo)
                            ' サーキット番号を4桁左詰めで設定する
                            '#4.12.2.0④                            strLogMsg = strLogMsg & String.Format("{0, -4}", NowBlockNo) & strchKugiri
                            strLogMsg.Append(String.Format("{0, -4}", NowBlockNo) & strchKugiri)    '#4.12.2.0④
                        Else
                            ' サーキット番号を4桁左詰めで設定する
                            '#4.12.2.0④                            strLogMsg = strLogMsg & String.Format("{0, -4}", typResistorInfoArray(intForCnt).intCircuitGrp) & strchKugiri
                            strLogMsg.Append(String.Format("{0, -4}", typResistorInfoArray(intForCnt).intCircuitGrp) & strchKugiri) '#4.12.2.0④
                        End If
                        'V4.3.0.0①↑
                    End If
                    '----- ###238↑ -----

                Case LOGTAR.RESISTOR
                    '----- ###238↓ -----
                    '抵抗データの追加
                    'strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intResNo.ToString() & strchKugiri
                    If (strchKugiri = ",") Then
                        ' 区切り文字が","ならログファイル出力用データと判断し4桁左詰めとしない
                        '#4.12.2.0④                        strLogMsg = strLogMsg & typResistorInfoArray(intForCnt).intResNo.ToString() & strchKugiri
                        strLogMsg.Append(typResistorInfoArray(intForCnt).intResNo & strchKugiri)    '#4.12.2.0④
                    Else
                        ' サーキット番号を4桁左詰めで設定する
                        '#4.12.2.0④                        strLogMsg = strLogMsg & String.Format("{0, -4}", typResistorInfoArray(intForCnt).intResNo) & strchKugiri
                        strLogMsg.Append(String.Format("{0, -4}", typResistorInfoArray(intForCnt).intResNo) & strchKugiri)  '#4.12.2.0④
                    End If
                    '----- ###238↑ -----

                Case LOGTAR.JUDGE
                    '===============================================
                    ' 判定結果の追加
                    '===============================================
                    ' ﾄﾘﾐﾝｸﾞ結果が12以下であるかﾁｪｯｸする。
                    '   →2009/07/30 432HWのINTRTMでは、11以降の結果は設定されないが、
                    '   　436Kとの互換を考慮しコードとしてはそのまま
                    '----- ###238↓ -----
                    If (strchKugiri = ",") Then
                        ' 区切り文字が","ならログファイル出力用データと判断し10文字左詰めとしない
                        'If gwTrimResult(intForCnt - 1) <= 12 Then              '###248
                        If gwTrimResult(intForCnt - 1) <= MAX_RESULT_NUM Then   '###248
                            ' ﾒｯｾｰｼﾞを作成する。
                            '#4.12.2.0④                            strLogMsg = strLogMsg & gstrResult(gwTrimResult(intForCnt - 1))
                            strLogMsg.Append(gstrResult(gwTrimResult(intForCnt - 1)))               '#4.12.2.0④
                        Else
                            ' ｴﾗｰﾒｯｾｰｼﾞを作成する。
                            '#4.12.2.0④                            strLogMsg = strLogMsg & "ERR=" & CStr(gwTrimResult(intForCnt - 1))
                            strLogMsg.Append("ERR=" & gwTrimResult(intForCnt - 1))                  '#4.12.2.0④
                        End If
                    Else
                        'If gwTrimResult(intForCnt - 1) <= 12 Then              '###248
                        If gwTrimResult(intForCnt - 1) <= MAX_RESULT_NUM Then   '###248
                            ' ﾒｯｾｰｼﾞを作成する。(10文字左詰め)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & gstrResult(gwTrimResult(intForCnt - 1))
                            strLogMsg.Append(gstrResult(gwTrimResult(intForCnt - 1)))               '#4.12.2.0④
                        Else
                            ' ｴﾗｰﾒｯｾｰｼﾞを作成する。(10文字左詰め)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & "ERR=" & String.Format("{0, -6}", gwTrimResult(intForCnt - 1))
                            strLogMsg.Append("ERR=" & String.Format("{0, -6}", gwTrimResult(intForCnt - 1)))    '#4.12.2.0④
                        End If
                    End If
                    'If gwTrimResult(intForCnt - 1) <= 12 Then
                    '    ' ﾒｯｾｰｼﾞを作成する。
                    '    strLogMsg = strLogMsg & gstrResult(gwTrimResult(intForCnt - 1))
                    'Else
                    '    ' ｴﾗｰﾒｯｾｰｼﾞを作成する。
                    '    strLogMsg = strLogMsg & "ERR=" & CStr(gwTrimResult(intForCnt - 1))
                    'End If
                    '----- ###238↑ -----
                    '#4.12.2.0④                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0④

                Case LOGTAR.TARGET
                    '目標値データの追加(###238 16文字左詰め) 
                    '----- ###238↓ -----
                    If (strchKugiri = ",") Then
                        ' 区切り文字が","ならログファイル出力用データと判断し16文字左詰めとしない
                        If (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_RATIO) Then
                            strMakeMsg = "BaseR" + typResistorInfoArray(intForCnt).intBaseResNo.ToString + "*"
                            strMakeMsg = strMakeMsg + typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00")
                        ElseIf (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_CALC) Then
                            '----- ###123 ↓レシオ計算式の目標値 -----
                            'strMakeMsg = String.Format("{0,9}", gfTargetVal(typResistorInfoArray(intForCnt).intBaseResNo).ToString("0.00000")) ' ###003
                            Call GetRatio2Rn(typResistorInfoArray(intForCnt).intBaseResNo, Rn)
                            strMakeMsg = String.Format("{0,9}", gfTargetVal(Rn - 1).ToString("0.00000"))
                            '----- ###123 ↑ -----
                        Else
                            strMakeMsg = String.Format("{0,9}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000")) ' ###003
                        End If
                    Else
                        If (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_RATIO) Then
                            ' レシオ時("BaseR999*9.99")
                            strMakeMsg = "BaseR" + typResistorInfoArray(intForCnt).intBaseResNo.ToString + "*"
                            strMakeMsg = String.Format("{0,16}", strMakeMsg + typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00"))
                        ElseIf (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_CALC) Then
                            '----- V6.0.3.0⑬↓ -----
                            ' レシオ計算式の目標値
                            Call GetRatio2Rn(typResistorInfoArray(intForCnt).intBaseResNo, Rn)
                            'strMakeMsg = String.Format("{0,16}", gfTargetVal(intForCnt - 1).ToString("0.00000"))
                            If (DecPoint = 5) Then                      ' トリミング目標値の少数部桁数指定(０：通常(5桁), １：7桁)
                                strMakeMsg = String.Format("{0,16}", gfTargetVal(intForCnt - 1).ToString("0.00000"))
                            Else
                                strMakeMsg = String.Format("{0,18}", gfTargetVal(intForCnt - 1).ToStringF(7))
                            End If
                        Else
                            ' 絶対値
                            'strMakeMsg = String.Format("{0,16}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000"))
                            If (DecPoint = 5) Then                      ' トリミング目標値の少数部桁数指定(０：通常(5桁), １：7桁)
                                strMakeMsg = String.Format("{0,16}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000"))
                            Else
                                strMakeMsg = String.Format("{0,18}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToStringF(7))
                                'strMakeMsg = String.Format("{0,18}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("F7"))
                            End If
                            '----- V6.0.3.0⑬↑ -----
                        End If
                    End If

                    'If (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_RATIO) Then
                    '    strMakeMsg = "BaseR(" + typResistorInfoArray(intForCnt).intBaseResNo.ToString + ")*"
                    '    strMakeMsg = strMakeMsg + typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00")
                    'ElseIf (typResistorInfoArray(intForCnt).intTargetValType = TARGET_TYPE_CALC) Then
                    '    '----- ###123 ↓レシオ計算式の目標値 -----
                    '    'strMakeMsg = String.Format("{0,9}", gfTargetVal(typResistorInfoArray(intForCnt).intBaseResNo).ToString("0.00000")) ' ###003
                    '    Call GetRatio2Rn(typResistorInfoArray(intForCnt).intBaseResNo, Rn)
                    '    strMakeMsg = String.Format("{0,9}", gfTargetVal(Rn - 1).ToString("0.00000"))
                    '    '----- ###123 ↑ -----
                    'Else
                    '    strMakeMsg = String.Format("{0,9}", typResistorInfoArray(intForCnt).dblTrimTargetVal.ToString("0.00000")) ' ###003
                    'End If
                    '----- ###238↑ -----
                    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '#4.12.2.0④ strLogMsg = strLogMsg & strMakeMsg & strchKugiri
                    strLogMsg.Append(strMakeMsg & strchKugiri)                                      '#4.12.2.0④

                Case LOGTAR.INITIAL
                    '===============================================
                    ' イニシャルテストの追加
                    '===============================================
                    '----- ###238↓ -----
                    If (strchKugiri = ",") Then
                        ' 区切り文字が","ならログファイル出力用データと判断し16文字左詰めとしない
                        ' SKIPでなく、イニシャル出力ありの場合　
                        If ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                            ((gSysPrm.stLOG.giLoggingDataKind And cLoggingIT) Or (digL = 0))) Then ' ###183
                            ' イニシャル結果をフォーマット変換し文字列を構築(###238 16文字左詰め)
                            'strMakeMsg = String.Format("{0,9}", gfInitialTest(intForCnt - 1).ToString("0.00000"))      ' V1.16.0.0③ ###003
                            strMakeMsg = String.Format("{0,9}", gfInitialTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM)) ' V1.16.0.0③
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0④
                        Else
                            'SKIPやログデータ種別の判定=イニシャル出力なしの場合は「0.000000」
                            'strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000")      ' V1.16.0.0③ ###003
                            '#4.12.2.0④                            strLogMsg = strLogMsg & String.Format("{0,9}", gsEDIT_DIGITNUM) ' V1.16.0.0③
                            strLogMsg.Append(String.Format("{0,9}", gsEDIT_DIGITNUM)) ' V1.16.0.0③  '#4.12.2.0④
                        End If
                    Else
                        ' SKIPでなく、イニシャル出力ありの場合　
                        If ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                            ((gSysPrm.stLOG.giLoggingDataKind And cLoggingIT) Or (digL = 0))) Then
                            ' イニシャル結果をフォーマット変換し文字列を構築(16文字左詰め)
                            'strMakeMsg = String.Format("{0,16}", gfInitialTest(intForCnt - 1).ToString("0.00000"))         ' V1.16.0.0③
                            strMakeMsg = String.Format("{0,16}", gfInitialTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))    ' V1.16.0.0③
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0④
                        Else
                            ' SKIPやログデータ種別の判定=イニシャル出力なしの場合は「0.000000(16文字左詰め)」
                            'strLogMsg = strLogMsg & String.Format("{0,16}", "0.00000")         ' V1.16.0.0③
                            '#4.12.2.0④                            strLogMsg = strLogMsg & String.Format("{0,16}", gsEDIT_DIGITNUM)    ' V1.16.0.0③
                            strLogMsg.Append(String.Format("{0,16}", gsEDIT_DIGITNUM)) ' V1.16.0.0③ '#4.12.2.0④
                        End If
                    End If

                    '' SKIPでなく、イニシャル出力ありの場合　
                    'If ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                    '        And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                    '        And gwTrimResult(intForCnt - 1) <> 11 _
                    '        And gwTrimResult(intForCnt - 1) <> 13) And _
                    '    ((gSysPrm.stLOG.giLoggingDataKind And cLoggingIT) Or (digL = 0))) Then ' ###183
                    '    'イニシャル結果をフォーマット変換し文字列を構築(###238 16文字左詰め)
                    '    strMakeMsg = String.Format("{0,9}", gfInitialTest(intForCnt - 1).ToString("0.00000"))  ' ###003
                    '    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '    strLogMsg = strLogMsg & strMakeMsg
                    'Else
                    '    'SKIPやログデータ種別の判定=イニシャル出力なしの場合は「0.000000」
                    '    strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000") ' ###003
                    'End If
                    '----- ###238↑ -----
                    '#4.12.2.0④                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0④
                Case LOGTAR.FINAL
                    '===============================================
                    ' ファイナルテストの追加
                    '===============================================
                    '----- ###238↓ -----
                    If (strchKugiri = ",") Then
                        ' 区切り文字が","ならログファイル出力用データと判断し16文字左詰めとしない
                        If (digL = 3) Then '測定モードでは判定していないため、無条件に出力
                            ' ファイナル結果をフォーマット変換し文字列を構築
                            'strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000"))        ' V1.16.0.0③ ###003
                            strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))   ' V1.16.0.0③
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0④
                        ElseIf ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_IT_NG _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                                ((gSysPrm.stLOG.giLoggingDataKind And cLoggingFT) Or (digL <= 2))) Then ' ###183
                            'ファイナル結果をフォーマット変換し文字列を構築(###238 16文字左詰め)
                            'strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000"))        ' V1.16.0.0③ ###003
                            strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))   ' V1.16.0.0③
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0④
                        Else
                            'IT ERRやログデータ種別の判定=ファイナル出力なしの場合、
                            '「0.000000」にデータを設定し判定結果をクリアする
                            'strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000")      ' V1.16.0.0③ ###003
                            '#4.12.2.0④                            strLogMsg = strLogMsg & String.Format("{0,9}", gsEDIT_DIGITNUM) ' V1.16.0.0③
                            strLogMsg.Append(String.Format("{0,9}", gsEDIT_DIGITNUM)) ' V1.16.0.0③  '#4.12.2.0④
                        End If
                    Else
                        If (digL = 3) Then '測定モードでは判定していないため、無条件に出力
                            'ファイナル結果をフォーマット変換し文字列を構築(16文字左詰め)
                            'strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString("0.00000"))       ' V1.16.0.0③
                            strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))  ' V1.16.0.0③
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0④
                        ElseIf ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_IT_NG _
                                And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                                And gwTrimResult(intForCnt - 1) <> 11 _
                                And gwTrimResult(intForCnt - 1) <> 13) And
                                ((gSysPrm.stLOG.giLoggingDataKind And cLoggingFT) Or (digL <= 2))) Then
                            'ファイナル結果をフォーマット変換し文字列を構築(16文字左詰め)
                            'strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString("0.00000"))       ' V1.16.0.0③
                            strMakeMsg = String.Format("{0,16}", gfFinalTest(intForCnt - 1).ToString(gsEDIT_DIGITNUM))  ' V1.16.0.0③
                            strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                            '#4.12.2.0④                            strLogMsg = strLogMsg & strMakeMsg
                            strLogMsg.Append(strMakeMsg)                                            '#4.12.2.0④
                        Else
                            'IT ERRやログデータ種別の判定=ファイナル出力なしの場合、
                            '「0.000000」にデータを設定し判定結果をクリアする(16文字左詰め)
                            'strLogMsg = strLogMsg & String.Format("{0,16}", "0.00000")         ' V1.16.0.0③
                            '#4.12.2.0④                            strLogMsg = strLogMsg & String.Format("{0,16}", gsEDIT_DIGITNUM)    ' V1.16.0.0③
                            strLogMsg.Append(String.Format("{0,16}", gsEDIT_DIGITNUM)) ' V1.16.0.0③ '#4.12.2.0④
                        End If
                    End If

                    'If (digL = 3) Then '測定モードでは判定していないため、無条件に出力
                    '    'ファイナル結果をフォーマット変換し文字列を構築
                    '    strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000")) ' ###003
                    '    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '    strLogMsg = strLogMsg & strMakeMsg

                    'ElseIf ((gwTrimResult(intForCnt - 1) <> TRIM_RESULT_NOTDO _
                    '        And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_IT_NG _
                    '        And gwTrimResult(intForCnt - 1) <> TRIM_RESULT_SKIP _
                    '        And gwTrimResult(intForCnt - 1) <> 11 _
                    '        And gwTrimResult(intForCnt - 1) <> 13) And _
                    '        ((gSysPrm.stLOG.giLoggingDataKind And cLoggingFT) Or (digL <= 2))) Then ' ###183
                    '    'ファイナル結果をフォーマット変換し文字列を構築(###238 16文字左詰め)
                    '    strMakeMsg = String.Format("{0,9}", gfFinalTest(intForCnt - 1).ToString("0.00000")) ' ###003
                    '    strMakeMsg = CnvFloatPointChar(strMakeMsg, strchFloatPoint)
                    '    strLogMsg = strLogMsg & strMakeMsg
                    'Else
                    '    'IT ERRやログデータ種別の判定=ファイナル出力なしの場合、
                    '    '「0.000000」にデータを設定し判定結果をクリアする
                    '    'strLogMsg = strLogMsg & String.Format("{0,9}", "0.00000") ' ###003
                    'End If
                    '----- ###238↑ -----
                    '#4.12.2.0④                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0④

                Case LOGTAR.DEVIATION
                    '===============================================
                    ' 差分結果の追加
                    '===============================================
                    '----- ###238↓ -----
                    'Call TrimLogging_MakeLogDivResult(typResistorInfoArray(intForCnt).intTargetValType, intForCnt, strLogMsg)
                    strMakeMsg = ""
                    Call TrimLogging_MakeLogDivResult(typResistorInfoArray(intForCnt).intTargetValType, intForCnt, strMakeMsg)
                    If (strchKugiri = ",") Then
                        ' 区切り文字が","ならログファイル出力用データと判断し10文字左詰めとしない
                        '#4.12.2.0④                        strLogMsg = strLogMsg & strMakeMsg
                        strLogMsg.Append(strMakeMsg)                                                '#4.12.2.0④
                    Else
                        '#4.12.2.0④                        strLogMsg = strLogMsg & String.Format("{0,10}", strMakeMsg)
                        strLogMsg.Append(String.Format("{0,10}", strMakeMsg))                       '#4.12.2.0④
                    End If
                    '----- ###238↑ -----
                    '#4.12.2.0④                    strLogMsg = strLogMsg & strchKugiri
                    strLogMsg.Append(strchKugiri)                                                   '#4.12.2.0④

                Case LOGTAR.UCUTPRMNO
                    ' (ﾃﾞｼﾞSWの1桁目が0かつ(ﾄﾘﾐﾝｸﾞ結果が1または、3))または、ﾃﾞｼﾞSWの1桁目が1であるかﾁｪｯｸする。
                    If ((gwTrimResult(intForCnt - 1) = TRIM_RESULT_OK _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_NG _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_HING _
                            Or gwTrimResult(intForCnt - 1) = TRIM_RESULT_FT_LONG _
                            Or gwTrimResult(intForCnt - 1) = 12)) Then
                        Dim strWork As String
                        strWork = ""

                        ' Uｶｯﾄ実行結果
                        Call RetrieveUCutResult(intForCnt, strWork)

                        If Len(strWork) Then
                            '#4.12.2.0④                            strLogMsg = strLogMsg & ", " & strWork & Chr(13) & Chr(10)
                            strLogMsg.Append(", " & strWork & Chr(13) & Chr(10))                    '#4.12.2.0④
                        Else
                            '#4.12.2.0④                            strLogMsg = strLogMsg & Chr(13) & Chr(10)
                            strLogMsg.Append(Chr(13) & Chr(10))                                     '#4.12.2.0④
                        End If
                    Else
                        '#4.12.2.0④                        strLogMsg = strLogMsg & Chr(13) & Chr(10)
                        strLogMsg.Append(Chr(13) & Chr(10))                                         '#4.12.2.0④
                    End If
                Case LOGTAR.NOSET
                    ' 何もしない
                Case LOGTAR.END
                    Exit For
            End Select
        Next

        ' 1データ完了につき区切り文字を挿入
        If (addRetFlg = True) Then
            '#4.12.2.0④            strLogMsg = strLogMsg & vbCrLf
            strLogMsg.AppendLine()                                                                  '#4.12.2.0④
        Else
            '#4.12.2.0④            strLogMsg = strLogMsg & strchKugiri
            strLogMsg.Append(strchKugiri)                                                           '#4.12.2.0④
        End If

        Exit Sub

ErrExit:
        MsgBox("TrimLogging_MakeLogStringData" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region
    'V4.0.0.0-65        ↓↓↓↓
#Region "カット条件ログ出力用の日時文字列を取得する"
    '''=========================================================================
    ''' <summary>カット条件ログ出力用の日時文字列を取得する</summary>
    ''' <returns>カット条件ログ出力用の日時文字列</returns>
    ''' <remarks>V4.0.0.0-65</remarks>
    '''=========================================================================
    Private Function TrimLogging_GetDateFormatString() As String
        ' ﾒｯｾｰｼﾞ言語が欧州以外であるかﾁｪｯｸする。
        Dim s As String
        If (2 <> gSysPrm.stTMN.giMsgTyp) Then
            s = ","
        Else
            s = vbTab
        End If

        Return String.Format("{0:yyMMdd} {1:HHmmss}" & s & " ",
                             DateTime.Today, DateTime.Now)
    End Function
#End Region

#Region "カット条件をログ出力データとして構築する。"
    '''=========================================================================
    ''' <summary>カット条件をログ出力データとして構築する。</summary>
    ''' <param name="procMsg">File Load, Edit Start, Edit End</param>
    ''' <param name="strLogMsg">ﾛｸﾞ出力する文字列を返す</param>
    ''' <param name="flgOut">&#38;H01: ｶｯﾄ番号, 
    '''                      &#38;H02: ﾃﾞｨﾚｲ, 
    '''                      &#38;H04: ｽﾀｰﾄPX, 
    '''                      &#38;H08: ｽﾀｰﾄPY, 
    '''                      &#38;H10: ｶｯﾄ後ﾎﾟｰｽﾞ, 
    '''                      &#38;H20: 切替ﾎﾟｲﾝﾄ, 
    '''                      &#38;H40: RTｵﾌｾｯﾄ</param>
    ''' <remarks>V4.0.0.0-65</remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogCutCondition(ByRef procMsg As String,
                                                ByRef strLogMsg As String,
                                               Optional ByVal flgOut As Integer = &H1)
        '#4.12.2.0④    Private Sub TrimLogging_MakeLogCutCondition(ByVal procMsg As String,
        '#4.12.2.0④                                               ByRef strLogMsg As String,
        '#4.12.2.0④                                               Optional ByVal flgOut As Integer = &H1)
        ' &H01: ｶｯﾄ番号
        ' &H02: ﾃﾞｨﾚｲ
        ' &H04: ｽﾀｰﾄPX
        ' &H08: ｽﾀｰﾄPY
        ' &H10: ｶｯﾄ後ﾎﾟｰｽﾞ
        ' &H20: 切替ﾎﾟｲﾝﾄ
        ' &H40: RTｵﾌｾｯﾄ

        Dim ci As CultureInfo = Thread.CurrentThread.CurrentCulture
        Try
            ' ﾒｯｾｰｼﾞ言語が欧州以外であるかﾁｪｯｸする。
            Dim S As String
            If (2 <> gSysPrm.stTMN.giMsgTyp) Then
                S = ","
            Else
                S = vbTab
                ' 出力文字列の小数点をｶﾝﾏに設定する
                Dim nfi As New NumberFormatInfo() With {.NumberDecimalSeparator = ","}
                Thread.CurrentThread.CurrentCulture =
                    New CultureInfo("ja-JP") With {.NumberFormat = nfi}
            End If

            Dim sb As New StringBuilder(1024)

            sb.AppendFormat(TrimLogging_GetDateFormatString())      ' 日時
            sb.AppendLine(procMsg)      ' File Load, Edit Start, Edit End

            If ((&H1 And flgOut) = &H1) Then sb.Append("CUT-NO" & S) ' ｶｯﾄ番号
            If ((&H2 And flgOut) = &H2) Then sb.Append("DELAY" & S) ' ﾃﾞｨﾚｲ
            If ((&H4 And flgOut) = &H4) Then sb.Append("ST-PX" & S) ' ｽﾀｰﾄPX
            If ((&H8 And flgOut) = &H8) Then sb.Append("ST-PY" & S) ' ｽﾀｰﾄPY
            sb.Append("SPEED1" & S)     ' 速度1
            sb.Append("QRATE1" & S)     ' Qrate1
            sb.Append("STEG1" & S)      ' STEG1
            sb.Append("CURRENT1" & S)   ' 電流値1
            sb.Append("TARGET1" & S)    ' 目標ﾊﾟﾜｰ1
            sb.Append("RANGE1" & S)     ' 範囲1
            sb.Append("CUTOFF" & S)     ' ｶｯﾄｵﾌ
            If ((&H10 And flgOut) = &H10) Then sb.Append("PAUSE-AFTER-CUT" & S) ' ｶｯﾄ後ﾎﾟｰｽﾞ
            If ((&H20 And flgOut) = &H20) Then sb.Append("SWITCHING-POINT" & S) ' 切替ﾎﾟｲﾝﾄ
            If ((&H40 And flgOut) = &H40) Then sb.Append("RT-OFFSET" & S) ' RTｵﾌｾｯﾄ

            sb.Append("CUT-TYPE" & S)   ' ｶｯﾄ種別
            sb.Append("DIR" & S)        ' 角度
            sb.Append("CUT-LEN1" & S)   ' ｶｯﾄ長1

            sb.Append("TURN-DIR" & S)   ' ﾀｰﾝ方向
            sb.Append("R1" & S)         ' R1
            sb.Append("LTURN-P" & S)    ' LﾀｰﾝP
            sb.Append("CUT-LEN2" & S)   ' ｶｯﾄ長2
            sb.Append("SPEED2" & S)     ' 速度2
            sb.Append("QRATE2" & S)     ' Qﾚｰﾄ2
            sb.Append("STEG2" & S)      ' STEG2
            sb.Append("CURRENT2" & S)   ' 電流値2
            sb.Append("TARGET2" & S)    ' 目標ﾊﾟﾜｰ2
            sb.Append("RANGE2" & S)     ' 範囲2

            sb.Append("SPEED3" & S)     ' ｶｯﾄ速度3
            sb.Append("QRATE3" & S)     ' Qﾚｰﾄ3
            sb.Append("STEG3" & S)      ' STEG3
            sb.Append("CURRENT3" & S)   ' 電流値3
            sb.Append("TARGET3" & S)    ' 目標ﾊﾟﾜｰ3
            sb.Append("RANGE3" & S)     ' 範囲3

            sb.Append("SPEED4" & S)     ' ｶｯﾄ速度4
            sb.Append("QRATE4" & S)     ' Qﾚｰﾄ4
            sb.Append("STEG4" & S)      ' STEG4
            sb.Append("CURRENT4" & S)   ' 電流値4
            sb.Append("TARGET4" & S)    ' 目標ﾊﾟﾜｰ4
            sb.Append("RANGE4")         ' 範囲4

            sb.AppendLine()             ' 改行

            For cn As Integer = 1 To typResistorInfoArray(1).intCutCount Step 1
                With typResistorInfoArray(1).ArrCut(cn)
                    If ((&H1 And flgOut) = &H1) Then sb.AppendFormat("{0,2}" & S, .intCutNo) ' ｶｯﾄ番号
                    If ((&H2 And flgOut) = &H2) Then sb.AppendFormat("{0,5}" & S, .intDelayTime) ' ﾃﾞｨﾚｲ
                    If ((&H4 And flgOut) = &H4) Then sb.AppendFormat("{0,9:F5}" & S, .dblStartPointX) ' ｽﾀｰﾄPX
                    If ((&H8 And flgOut) = &H8) Then sb.AppendFormat("{0,9:F5}" & S, .dblStartPointY) ' ｽﾀｰﾄPY
                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed)                   ' 速度1
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate)                      ' Qrate1
                    sb.AppendFormat("{0,2}" & S, .FLSteg(0))                        ' STEG1
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(0))                     ' 電流値1
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(0))       ' 目標ﾊﾟﾜｰ1
                    sb.AppendFormat("{0,5:F2}" & S, .dblPowerAdjustToleLevel(0))    ' 範囲1
                    sb.AppendFormat("{0,6:F2}" & S, .dblCutOff)                     ' ｶｯﾄｵﾌ
                    If ((&H10 And flgOut) = &H10) Then sb.AppendFormat("{0,5}" & S, .intCutAftPause) ' ｶｯﾄ後ﾎﾟｰｽﾞ
                    If ((&H20 And flgOut) = &H20) Then sb.AppendFormat("{0,5:F1}" & S, .dblJudgeLevel) ' 切替ﾎﾟｲﾝﾄ
                    If ((&H40 And flgOut) = &H40) Then
                        If ((DataManager.CNS_CUTP_NSTr = .strCutType) OrElse
                            (DataManager.CNS_CUTP_NLr = .strCutType)) Then
                            sb.AppendFormat("{0,7:F4}" & S, .dblReturnPos)          ' RTｵﾌｾｯﾄ
                        Else
                            sb.Append("       " & S)
                        End If
                    End If
                    Dim cutType As String
                    Select Case (.strCutType)
                        Case DataManager.CNS_CUTP_NST : cutType = "StCUT"
                        Case DataManager.CNS_CUTP_NL : cutType = "L-CUT"
                        Case DataManager.CNS_CUTP_NSTr : cutType = "StRTN"
                        Case DataManager.CNS_CUTP_NLr : cutType = "L-RTN"
                        Case DataManager.CNS_CUTP_NSTt : cutType = "StTRC"
                        Case DataManager.CNS_CUTP_NLt : cutType = "L-TRC"
                        Case Else : cutType = String.Empty
                    End Select
                    sb.AppendFormat("{0,5}" & S, cutType)                           ' ｶｯﾄ種別
                    sb.AppendFormat("{0,3}" & S, .intCutAngle)                      ' 角度
                    If (DataManager.CNS_CUTP_NST = .strCutType) Then
                        sb.AppendFormat("{0,8:F5}", .dblMaxCutLength)               ' ｶｯﾄ長1
                        sb.AppendLine()                                             ' 改行
                        Continue For                                                ' STｶｯﾄの場合
                    Else
                        sb.AppendFormat("{0,8:F5}" & S, .dblMaxCutLength)           ' ｶｯﾄ長1
                    End If

                    If (DataManager.CNS_CUTP_NL = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NLr = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NLt = .strCutType) Then
                        Dim LTurnDir As String
                        If (1 = .intLTurnDir) Then                                  ' CW=1,CCW=2に変更
                            LTurnDir = "CW"
                        ElseIf (2 = .intLTurnDir) Then
                            LTurnDir = "CCW"
                        Else
                            LTurnDir = "??"
                        End If
                        sb.AppendFormat("{0,3}" & S, LTurnDir)                      ' ﾀｰﾝ方向
                        sb.AppendFormat("{0,8:F5}" & S, .dblR1)                     ' R1
                        sb.AppendFormat("{0,5:F1}" & S, .dblLTurnPoint)             ' LﾀｰﾝP
                    Else
                        sb.Append("   " & S & "        " & S & "     " & S)
                    End If
                    sb.AppendFormat("{0,8:F5}" & S, .dblMaxCutLengthL)              ' ｶｯﾄ長2
                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed2)                  ' 速度2
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate2)                     ' Qﾚｰﾄ2
                    Dim idx As Integer
                    If (DataManager.CNS_CUTP_NSTr = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NSTt = .strCutType) Then
                        ' STｶｯﾄRT、STｶｯﾄTRの場合
                        idx = 4
                    Else
                        ' Lｶｯﾄ、LｶｯﾄRT、LｶｯﾄTRの場合
                        idx = 1
                    End If
                    sb.AppendFormat("{0,2}" & S, .FLSteg(idx))                     ' STEG2
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(idx))                  ' 電流値2
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(idx))    ' 目標ﾊﾟﾜｰ2
                    If (DataManager.CNS_CUTP_NL = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NSTr = .strCutType) OrElse
                        (DataManager.CNS_CUTP_NSTt = .strCutType) Then
                        sb.AppendFormat("{0,5:F2}", .dblPowerAdjustToleLevel(idx))  ' 範囲2
                        sb.AppendLine()                                             ' 改行
                        Continue For                                                ' Lｶｯﾄ,STｶｯﾄTR,STｶｯﾄTRの場合
                    Else
                        sb.AppendFormat("{0,5:F2}" & S, .dblPowerAdjustToleLevel(idx)) ' 範囲2
                    End If

                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed3)                  ' ｶｯﾄ速度3
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate3)                     ' Qﾚｰﾄ3
                    sb.AppendFormat("{0,2}" & S, .FLSteg(4))                        ' STEG3
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(4))                     ' 電流値3
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(4))       ' 目標ﾊﾟﾜｰ3
                    sb.AppendFormat("{0,5:F2}" & S, .dblPowerAdjustToleLevel(4))    ' 範囲3

                    sb.AppendFormat("{0,5:F1}" & S, .dblCutSpeed4)                  ' ｶｯﾄ速度4
                    sb.AppendFormat("{0,4:F1}" & S, .dblQRate4)                     ' Qﾚｰﾄ4
                    sb.AppendFormat("{0,2}" & S, .FLSteg(5))                        ' STEG4
                    sb.AppendFormat("{0,4}" & S, .FLCurrent(5))                     ' 電流値4
                    sb.AppendFormat("{0,4:F2}" & S, .dblPowerAdjustTarget(5))       ' 目標ﾊﾟﾜｰ4
                    sb.AppendFormat("{0,5:F2}", .dblPowerAdjustToleLevel(5))        ' 範囲4

                    sb.AppendLine()                                                 ' 改行
                End With
            Next cn

            Debug.Print(MethodBase.GetCurrentMethod().Name &
                        "() : sb.Length = " & sb.Length & ", sb.Capacity = " & sb.Capacity &
                        Environment.NewLine & sb.ToString())

            If (strLogMsg Is Nothing) Then strLogMsg = String.Empty
            strLogMsg &= sb.ToString()

        Catch ex As Exception
            MsgBox(MethodBase.GetCurrentMethod().Name & ":" &
                   ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, gAppName)
            Err.Clear()
        Finally
            Thread.CurrentThread.CurrentCulture = ci
        End Try

    End Sub
#End Region
    'V4.0.0.0-65        ↑↑↑↑
#Region "差分演算結果のログ出力データを構築する。"
    '''=========================================================================
    ''' <summary>差分演算結果の出力データを構築する。</summary>
    ''' <param name="tarValType">(INP)目標値指定(0:絶対値,1:レシオ,2:計算式, 3～9:特注レシオ)</param>
    ''' <param name="resCnt">    (INP)抵抗データのインデックス(1～)</param>
    ''' <param name="strDivMsg"> (OUT)編集後データ</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogDivResult(ByVal tarValType As Integer, ByVal resCnt As Integer, ByRef strDivMsg As String)

        Dim strMakeMsg As String
        Dim dblDiffdata As Double
        Dim targetVal As Double
        Dim rationBaseRNo As Short

        Try

            strMakeMsg = ""
            dblDiffdata = 0
            rationBaseRNo = 0
            If (tarValType = TARGET_TYPE_CALC) Then
                targetVal = gfTargetVal(resCnt - 1)
            Else
                targetVal = typResistorInfoArray(resCnt).dblTrimTargetVal
            End If

            ' (抵抗ﾃﾞｰﾀ）目標値指定のﾁｪｯｸを行う。
            Select Case tarValType
                ' 絶対値/計算式の場合
                Case TARGET_TYPE_ABSOLUTE, TARGET_TYPE_CALC
                    If targetVal <> 0.0# Then

                        ' (ﾌｧｲﾅﾙﾃｽﾄの結果/ﾄﾘﾐﾝｸﾞ目標値*100-100)の値を取得する。
                        dblDiffdata = (gfFinalTest(resCnt - 1) / targetVal) * 100.0# - 100.0#

                        ' 上の計算結果を表示ﾛｸﾞ用にﾌｫｰﾏｯﾄする。　" xxxxxx%(計算結果"
                        strDivMsg = strDivMsg & " " & dblDiffdata.ToString("0.000").PadLeft(3 + 4) & "%"

                        ' ﾌｧｲﾅﾙ測定値誤差(ﾌｧｲﾙﾛｸﾞ用)
                        gfFTest_Par(resCnt - 1) = dblDiffdata
                    Else
                        '----- ###227↓-----
                        '' ﾚｼｵ目標値が0の場合には、ｽｷｯﾌﾟ用の文字列を作成する。
                        'strDivMsg = strDivMsg & " " & "    ---" & "%"

                        ' 電圧トリミングで目標値が0の場合
                        If (typResistorInfoArray(resCnt).intResMeasMode = MEASMODE_VOL) Then
                            ' (ﾌｧｲﾅﾙﾃｽﾄの結果*100-100)の値を取得する。
                            dblDiffdata = gfFinalTest(resCnt - 1) * 100.0#
                            If (typResistorInfoArray(resCnt).intSlope = 1) Then     ' 電圧+スロープ
                                dblDiffdata = dblDiffdata
                            Else                                                    ' 電圧-スロープ
                                dblDiffdata = dblDiffdata
                            End If

                            ' 上の計算結果を表示ﾛｸﾞ用にﾌｫｰﾏｯﾄする。　" xxxxxx%(計算結果"
                            strDivMsg = strDivMsg & " " & dblDiffdata.ToString("0.000").PadLeft(3 + 4) & "%"

                            ' ﾌｧｲﾅﾙ測定値誤差(ﾌｧｲﾙﾛｸﾞ用)
                            gfFTest_Par(resCnt - 1) = dblDiffdata
                        Else
                            ' ﾚｼｵ目標値が0の場合には、ｽｷｯﾌﾟ用の文字列を作成する。
                            strDivMsg = strDivMsg & " " & "    ---" & "%"
                        End If
                        '----- ###227↑-----
                    End If

                    ' レシオ
                Case TARGET_TYPE_RATIO, TARGET_TYPE_CUSRTO_3, TARGET_TYPE_CUSRTO_4,
                        TARGET_TYPE_CUSRTO_5, TARGET_TYPE_CUSRTO_6, TARGET_TYPE_CUSRTO_7,
                        TARGET_TYPE_CUSRTO_8, TARGET_TYPE_CUSRTO_9

                    ' ﾚｼｵﾍﾞｰｽNo.を取得する。
                    Call GetRatio3Br(resCnt, rationBaseRNo)   ' ﾍﾞｰｽ抵抗番号取得
                    If rationBaseRNo < 0 Then
                        ' ｴﾗｰﾒｯｾｰｼﾞを出力する。
                        MsgBox("Internal error X003", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                        rationBaseRNo = 0
                    End If

                    ' 目標値(最終目標値)=ベース抵抗実測値＊レシオ＝を取得する。
                    dblDiffdata = gfFinalTest(rationBaseRNo) * targetVal
                    ' 取得した目標値が0以外であることを確認する。
                    If (gTkyKnd = KND_CHIP) Then
                        If (targetVal <> 0.0#) Then
                            dblDiffdata = targetVal
                        Else
                            dblDiffdata = 0
                        End If
                    End If

                    If dblDiffdata <> 0.0# Then
                        ' (ﾌｧｲﾅﾙﾃｽﾄの結果/ﾄﾘﾐﾝｸﾞ目標値*100-100)の値を取得する。
                        dblDiffdata = (gfFinalTest(resCnt - 1) / dblDiffdata) * 100.0# - 100.0#

                        ' 取得した計算結果を表示ﾛｸﾞ用にﾌｫｰﾏｯﾄする。
                        strDivMsg = strDivMsg & " " & dblDiffdata.ToString("0.000").PadLeft(3 + 4) & "%"
                        ' ﾌｧｲﾅﾙ測定値誤差(ﾌｧｲﾙﾛｸﾞ用)
                        gfFTest_Par(resCnt - 1) = dblDiffdata
                    Else
                        ' 取得した目標値が0の場合には、ｽｷｯﾌﾟ用の文字列を作成する。
                        strDivMsg = strDivMsg & " " & "    ---" & "%"
                    End If

                    '' '' ''    ' 計算式
                    '' '' ''Case TARGET_TYPE_CALC
                    '' '' ''    ' ﾚｼｵ目標値が0以外であることを確認する。
                    '' '' ''    If gfTargetVal(resCnt - 1) <> 0.0# Then
                    '' '' ''        ''計算値取得変数を初期化する。
                    '' '' ''        dblDiffdata = 0

                    '' '' ''        ' (ﾌｧｲﾅﾙﾃｽﾄ結果/ﾚｼｵ目標値*100-100)の計算結果を取得する。
                    '' '' ''        dblDiffdata = (gfFinalTest(resCnt - 1) / gfTargetVal(resCnt - 1)) * 100.0# - 100.0#

                    '' '' ''        ' 計算結果を表示ﾛｸﾞ用にﾌｫｰﾏｯﾄする。
                    '' '' ''        strDivMsg = strDivMsg & " " & Form1.Utility1.sFormat(dblDiffdata, "0.000", 3 + 4) & "%"
                    '' '' ''    Else
                    '' '' ''        ' ﾚｼｵ目標値が0の場合には、ｽｷｯﾌﾟ用の文字列を作成する。
                    '' '' ''        strDivMsg = strDivMsg & " " & "    ---" & "%"
                    '' '' ''    End If

                    '' '' ''    ' ﾒｯｾｰｼﾞにﾚｼｵ目標値をﾌﾟﾗｽする。(表示ﾛｸﾞ用にﾌｫｰﾏｯﾄする。)
                    '' '' ''    strDivMsg = strDivMsg & " " & Form1.Utility1.sFormat(gfTargetVal(resCnt - 1), "0.000000", 10 + 7)
            End Select

        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub
#End Region

#Region "ファイナル判定結果のログ出力データを構築する。"
    '''=========================================================================
    '''<summary>ファイナル判定結果の出力データを構築する。
    ''' 差分計算は別途実施</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLogging_MakeLogFinalResult(ByVal tarValType As Integer, ByVal resCnt As Integer, ByRef strFinalMsg As String)

        Dim strMakeMsg As String
        Dim dblDiffdata As Double
        Dim rationBaseRNo As Short
        strMakeMsg = ""
        dblDiffdata = 0
        rationBaseRNo = 0


        Try
            ' (抵抗ﾃﾞｰﾀ）ﾄﾘﾑﾓｰﾄﾞのﾁｪｯｸを行う。
            Select Case tarValType

                ' 絶対値, 計算式
                Case TARGET_TYPE_ABSOLUTE, TARGET_TYPE_CALC

                    ' ﾌｧｲﾅﾙﾃｽﾄ結果を表示ﾛｸﾞ用にﾌｫｰﾏｯﾄする。
                    strMakeMsg = gfFinalTest(resCnt - 1).ToString("0.000000").PadLeft(10 + 7)
                    strFinalMsg = strFinalMsg & " FT=" & strMakeMsg
                    ' レシオ
                Case TARGET_TYPE_RATIO, TARGET_TYPE_CUSRTO_3, TARGET_TYPE_CUSRTO_4,
                        TARGET_TYPE_CUSRTO_5, TARGET_TYPE_CUSRTO_6, TARGET_TYPE_CUSRTO_7,
                        TARGET_TYPE_CUSRTO_8, TARGET_TYPE_CUSRTO_9

                    ' ﾚｼｵﾍﾞｰｽNo.を取得する。
                    Call GetRatio3Br(resCnt, rationBaseRNo)   ' ﾍﾞｰｽ抵抗番号取得
                    If rationBaseRNo < 0 Then
                        ' ｴﾗｰﾒｯｾｰｼﾞを出力する。
                        MsgBox("Internal error X003", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                        rationBaseRNo = 0
                    End If

                    ' ﾌｧｲﾅﾙﾃｽﾄ結果が0以外であるかﾁｪｯｸする。
                    If gfFinalTest(rationBaseRNo) <> 0 Then
                        ' 計算値取得変数を初期化する。
                        dblDiffdata = 0

                        ' 比率を取得する。(ﾌｧｲﾅﾙﾃｽﾄ結果値/ﾍﾞｰｽﾌｧｲﾅﾙﾃｽﾄ結果値)
                        If (gfFinalTest(rationBaseRNo) <> 0) Then
                            dblDiffdata = gfFinalTest(resCnt - 1) / gfFinalTest(rationBaseRNo)

                            ' 表示ﾛｸﾞ用に計算結果をﾌｫｰﾏｯﾄする。
                            strMakeMsg = dblDiffdata.ToString("0.000000").PadLeft(10 + 7)
                        Else
                            ' ﾌｧｲﾅﾙﾃｽﾄ結果が0の場合はｽｷｯﾌﾟ用の文字列を作成する。
                            strMakeMsg = Space(14) & "---"
                        End If
                    End If
                    ' 作成したﾒｯｾｰｼﾞを設定する。　　" FT=" + strMakeMsg
                    strFinalMsg = strFinalMsg & " FT=" & strMakeMsg
            End Select

            If (gESLog_flg = True) Then     'V1.14.0.0①
                Call TrimLoggingResult_ES()
                Call LoggingWrite_ES()
            End If
        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub
#End Region

#Region "ロギング処理(ログ表示画面/生産管理画面)"
    '''=========================================================================
    ''' <summary>ロギング処理</summary>
    ''' <param name="digH">         (INP) 表示モード（デジタルスイッチ上位１桁）</param>
    ''' <param name="digL">         (INP) トリミングモード（デジタルスイッチ下位１桁）</param>
    ''' <param name="pltNoX">       (INP) プレート番号X</param>
    ''' <param name="pltNoY">       (INP) プレート番号Y</param>
    ''' <param name="blkNoX">       (INP) ブロック番号X</param>
    ''' <param name="blkNoY">       (INP) ブロック番号Y</param>
    ''' <param name="strLogMsgBuf"> (INP) </param>
    ''' <param name="strFileLogMsg">(INP) ログファイル出力用ログデータ</param>   
    ''' <param name="strDispLogMsg">(INP) 画面表示用ログデータ</param>  
    '''=========================================================================
    Private Sub TrimLogging_LoggingStart(ByVal digH As Integer, ByVal digL As Integer,
                                         ByVal pltNoX As Integer, ByVal pltNoY As Integer,
                                         ByVal blkNoX As Integer, ByVal blkNoY As Integer,
                                         ByVal strLogMsgBuf As StringBuilder,
                                         ByVal strFileLogMsg As StringBuilder,
                                         ByVal strDispLogMsg As StringBuilder)
        '#4.12.2.0④    Private Sub TrimLogging_LoggingStart(ByVal digH As Integer, ByVal digL As Integer, ByRef pltNoX As Integer, ByRef pltNoY As Integer,
        '#4.12.2.0④                                        ByRef blkNoX As Integer, ByRef blkNoY As Integer,
        '#4.12.2.0④                                        ByRef strLogMsgBuf As String, ByRef strFileLogMsg As String,
        '#4.12.2.0④                                        ByRef strDispLogMsg As String)
        '''''''スレッドを生成し表示処理の向上を検討する。問題はロギングに使用しているデータ
        '' '' ''Dim m_digH, m_digL As Integer
        '' '' ''Dim m_strLogMsgBuf, m_strFileLogMsg, m_strDispLogMsg As String

        '' '' ''Private Sub TrimLogging_LoggingStart()
        Dim dblTotalCnt As Double
        'Dim i As Integer
        Dim strMSG As String

        Try
            ' ﾛｷﾞﾝｸﾞｽﾀｰﾄﾌﾗｸﾞをﾁｪｯｸする。
            If gSysPrm.stLOG.giLoggingMode = 1 Then
                '   ココでは、ロギングデータの設定を行うようにして
                '   書込みは行わないように変更する。
                '#4.12.2.0④                strLogMsgBuf = strLogMsgBuf + strFileLogMsg
                strLogMsgBuf.Append(strFileLogMsg.ToString())   '#4.12.2.0④
                '        Call TrimLogging_Write(strLogMsg, False)   ' gsDataLogStr = gsDataLogStr + m_strMsg
                'm_strFileLogMsg = ""
                '#4.12.2.0④                strFileLogMsg = ""
                strFileLogMsg.Length = 0                        '#4.12.2.0④
            End If

            ' 表示モードのチェック
            '　表示なし（digH=0)の場合は何も表示しない。
            If (digH = 0) Then
                'ログ表示なし
            Else
                ''V4.0.0.0-63
                If gKeiTyp = KEY_TYPE_RS Then
                Else
                    '非表示モード以外はログエリアにメッセージ表示
                    Call Form1.Z_PRINT(strDispLogMsg)
                End If
            End If

            '画面のリフレッシュ                                         ' 再描画すると時間がかかるため削除 ###097
            'Call Form1.Refresh()

            '-----------------------------------------------------------------------
            '   Frame1情報(生産管理情報)表示
            '-----------------------------------------------------------------------
            ' 良不良合計数、不良率表示
            Fram1LblAry(FRAM1_ARY_GO).Text = m_lGoodCount.ToString("0")                         ' GO数(サーキット数)
            Fram1LblAry(FRAM1_ARY_NG).Text = m_lNgCount.ToString("0")                           ' NG数(サーキット数)
            Fram1LblAry(FRAM1_ARY_REGNUM).Text = (m_lGoodCount + m_lNgCount).ToString("0")      ' RESISTOR数
            If (m_lGoodCount + m_lNgCount) > 0 Then
                dblTotalCnt = CDbl(m_lNgCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_NGPER).Text = dblTotalCnt.ToString("0.00") + "%"          ' NG%
            Else
                Fram1LblAry(FRAM1_ARY_NGPER).Text = " - %"                                      ' NG%
            End If

            ' NG数を表示ラベルに設定する
            Fram1LblAry(FRAM1_ARY_ITHING).Text = m_lITHINGCount.ToString("0")        ' IT HI NG数
            Fram1LblAry(FRAM1_ARY_FTHING).Text = m_lFTHINGCount.ToString("0")        ' FT HI NG数
            Fram1LblAry(FRAM1_ARY_ITLONG).Text = m_lITLONGCount.ToString("0")        ' IT LO NG数
            Fram1LblAry(FRAM1_ARY_FTLONG).Text = m_lFTLONGCount.ToString("0")        ' FT LO NG数
            Fram1LblAry(FRAM1_ARY_OVER).Text = m_lITOVERCount.ToString("0")          ' OVER数

            ' NG 率を表示する
            If (m_lGoodCount + m_lNgCount) > 0 Then
                dblTotalCnt = CDbl(m_lITHINGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_ITHINGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lITLONGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_ITLONGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lFTHINGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_FTHINGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lFTLONGCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_FTLONGP).Text = dblTotalCnt.ToString("0.00")
                dblTotalCnt = CDbl(m_lITOVERCount) / CDbl(m_lGoodCount + m_lNgCount) * 100.0#
                Fram1LblAry(FRAM1_ARY_OVERP).Text = dblTotalCnt.ToString("0.00")
            Else
                Fram1LblAry(FRAM1_ARY_ITHINGP).Text = " -.-- "                  ' IT HI NG%
                Fram1LblAry(FRAM1_ARY_ITLONGP).Text = " -.-- "                  ' IT LO NG%
                Fram1LblAry(FRAM1_ARY_FTHINGP).Text = " -.-- "                  ' FT HI NG%
                Fram1LblAry(FRAM1_ARY_FTLONGP).Text = " -.-- "                  ' FT LO NG%
                Fram1LblAry(FRAM1_ARY_OVERP).Text = " -.-- "                    ' OVER NG%
            End If

            ' 表示ラベル再描画                                          ' 再描画すると時間がかかるため削除 ###097
            'For i = 0 To (MAX_FRAM1_ARY - 1)
            '    Fram1LblAry(i).Refresh()
            'Next i

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.ClearCounter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Err.Clear()
        End Try

    End Sub

#End Region

#Region "Index2 測定結果の取得(ROHM)"
    '''=========================================================================
    '''<summary>Index2 測定結果の取得(ROHM)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingResult_Index2()

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim intSetData As Short
        Dim intForCnt As Short
        Dim i As Short
        Dim ii As Short

        '----- V1.18.0.0⑥↓ -----
        ' IX2ログ出力なしならNOP
        If (giIX2LOG = 0) Or (gSysPrm.stDEV.rIX2Log_flg = False) Then
            Exit Sub
        End If
        '----- V1.18.0.0⑥↑ -----

        ' 抵抗数分ﾙｰﾌﾟ
        For i = 0 To gRegistorCnt - 1
            ' ｶｯﾄ数/15の整数部を取得する。
            intCntMax = Int(typResistorInfoArray(i + 1).intCutCount / 15)
            ' 最大値まで処理を行なう。
            For intForCnt = 0 To intCntMax
                ' ｶｳﾝﾀが最大値であるかﾁｪｯｸする。
                If intForCnt < intCntMax Then
                    ' 最大値の場合は、15で処理を行なう。
                    intSetData = 15
                Else
                    ' 最大値以外の場合は、15で除算した余りを取得する。
                    intSetData = typResistorInfoArray(i + 1).intCutCount Mod 15
                End If
                ' 取得した値が0の場合には、処理を抜ける。
                If intSetData = 0 Then Exit For

#If cOFFLINEcDEBUG Then
#Else
                ' INDEX2カット回数取得 V1.18.0.0⑥
                Call TRIM_RESULT_WORD(RSLTTYP_IX2_CUTCOUNT, i, intSetData, intForCnt * 15, 0, gfIndex2TestNum(intForCnt * 15))
                ' INDEX2測定値取得
                Call TRIM_RESULT_Double(RSLTTYP_IX2_MEAS, i, intSetData, intForCnt * 15, 0, gfIndex2Test(intForCnt * 15))
#End If
            Next

            ' ﾊﾞｯﾌｧに格納
            For ii = 0 To typResistorInfoArray(i + 1).intCutCount - 1
                gfIndex2ResultNum(i, ii) = gfIndex2TestNum(ii)  ' INDEX2カット回数
                gfIndex2Result(i, ii) = gfIndex2Test(ii)        ' INDEX2測定値
            Next ii
        Next i

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_Index2" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()

    End Sub
#End Region

#Region "Index2 ﾛｸﾞ出力(ROHM)"
    '''=========================================================================
    '''<summary>Index2 ﾛｸﾞ出力(ROHM)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub LoggingWrite_Index2()

        On Error GoTo ErrExit

        Dim intCntMax As Short
        Dim i As Short
        Dim c As Short
        Dim CutNum As Short
        Dim strDATA As String
        'Dim ff As Integer

        '----- V1.18.0.0⑥↓ -----
        ' IX2ログ出力なしならNOP
        If (giIX2LOG = 0) Or (gSysPrm.stDEV.rIX2Log_flg = False) Then
            Exit Sub
        End If
        '----- V1.18.0.0⑥↑ -----

        '抵抗数を取得
        intCntMax = gRegistorCnt
        strDATA = ""

        For i = 1 To intCntMax
            CutNum = typResistorInfoArray(i).intCutCount 'ｶｯﾄ数取得
            For c = 1 To CutNum
                ' 抵抗番号
                strDATA = strDATA & "R" & typResistorInfoArray(i).intResNo.ToString("000") & ","
                ' ｶｯﾄ番号
                strDATA = strDATA & "C" & Form1.ToString("00") & ","
                ' 回数
                strDATA = strDATA & gfIndex2ResultNum(i - 1, c - 1).ToString("0") & ","
                ' 測定値
                strDATA = strDATA & gfIndex2Result(i - 1, c - 1).ToString("0.00000")
                ' CRLF
                If c <> CutNum Then strDATA = strDATA & vbCrLf
            Next c
            If i <> intCntMax Then strDATA = strDATA & vbCrLf
        Next

        'ff = FreeFile()
        'FileOpen(ff, sIX2LogFilePath, OpenMode.Append)
        'PrintLine(ff, strDATA)
        'FileClose(ff)
        Using sw As New StreamWriter(sIX2LogFilePath, True, Encoding.UTF8)      ' 追記 UTF8 BOM有 notepad.exe 向け   V4.4.0.0-0
            sw.WriteLine(strDATA)
        End Using

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_Index2" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "ロギング開始(標準)"
    '''=========================================================================
    '''<summary>ロギング開始(標準)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub TrimLogging_Start2()

        Dim s As String
        Dim chKugiri As String
        Dim chFloatPoint As String

        Try
            ' 既にヘッダ出力済みなら抜ける
            ''V1.13.0.1①           '            If gLoggingHeader = False Then Exit Sub
            If (gLoggingHeader = False) And (typPlateInfo.intIDReaderUse = 0) Then Exit Sub

            '#4.12.2.0④            s = ""
            's = typPlateInfo.strDataName & vbCrLf          'V4.0.0.0-65

            If gSysPrm.stTMN.giMsgTyp <> 2 Then
                chKugiri = ","
                chFloatPoint = "."
            Else
                chKugiri = Chr(KUGIRI_CHAR) ' TAB
                chFloatPoint = ","
            End If

            Dim sb As New StringBuilder(64)                                             '#4.12.2.0④
            'x0用に設定を変更してヘッダーを構築
            '#4.12.2.0④            Call TrimLogging_MakeLogHeader(m_TrimlogFileFormat, LOGTYPE_FILE, chKugiri, s)
            TrimLogging_MakeLogHeader(m_TrimlogFileFormat, LOGTYPE_FILE, chKugiri, sb)  '#4.12.2.0④
            s = sb.ToString()                                                           '#4.12.2.0④

            ''V1.13.0.1①
            If (gLoggingHeader = False) Then
            Else
                Call TrimLogging_Write(s)
            End If
            ' IDRead用のLog書き込み 'V1.13.0.0⑥
            If (typPlateInfo.intIDReaderUse = 1) And (gLoggingHeader = False) Then
                s = typPlateInfo.strDataName & vbCrLf & s       'V4.0.0.0-65
                MakeTmpLogFile(globalLogFileName, s)
            End If
            ''V1.13.0.1①

            gLoggingHeader = False

            Exit Sub

        Catch ex As Exception
        End Try
    End Sub

#End Region
    'V4.0.0.0-65        ↓↓↓↓
#Region "ログファイル新規作成または追記"
    '''=========================================================================
    ''' <summary>ログファイル新規作成または追記</summary>
    ''' <param name="procMsg">File Load, Edit Start, Edit End, Logging ON, Logging OFF など</param>
    ''' <param name="dataFileName">ﾛｸﾞﾌｧｲﾙ先頭に書き込むﾃﾞｰﾀﾌｧｲﾙ名(指定すると新規ﾛｸﾞﾌｧｲﾙ作成)</param>
    ''' <param name="procMsgOnly">True:ｶｯﾄ条件を含まないﾛｸﾞ出力(Falseでもｼｽﾊﾟﾗの設定が0の場合は出力しない)</param>
    ''' <remarks>V4.0.0.0-65</remarks>
    '''=========================================================================
    Public Sub TrimLogging_CreateOrAppend(ByRef procMsg As String,
                                          Optional ByVal dataFileName As String = "",
                                          Optional ByVal procMsgOnly As Boolean = False)
        '#4.12.2.0④    Public Sub TrimLogging_CreateOrAppend(ByVal procMsg As String,
        '#4.12.2.0④                                          Optional ByVal dataFileName As String = "",
        '#4.12.2.0④                                          Optional ByVal procMsgOnly As Boolean = False)
        Dim dataName As String
        If (String.Empty <> dataFileName) Then
            ' ﾛｸﾞﾌｧｲﾙ新規作成
            dataName = dataFileName & Environment.NewLine               ' ﾃﾞｰﾀﾌｧｲﾙ名を書き込む
            TrimLogging_Write(dataName, True)                           ' 通常のﾛｸﾞﾌｧｲﾙを新規作成する
            Globals_Renamed.gLoggingHeader = True                       ' ﾛｸﾞﾍｯﾀﾞｰ書込み指示ﾌﾗｸﾞ(TRUE:出力)
        Else
            ' 既存ﾛｸﾞﾌｧｲﾙを使用
            dataName = String.Empty                                     ' ﾃﾞｰﾀﾌｧｲﾙ名は書き込まない
        End If

        If (True = procMsgOnly) Then
            ' 日時を付加した procMsg のみ出力する
            Dim proc As String = TrimLogging_GetDateFormatString() & procMsg & Environment.NewLine

            Select Case gSysPrm.stLOG.giLogPutCutCond
                Case 1  ' 同じﾛｸﾞﾌｧｲﾙに書き込む
                    TrimLogging_Write(proc)                             ' 通常のﾛｸﾞﾌｧｲﾙに出力する

                Case 2  ' ｶｯﾄ条件ﾛｸﾞﾌｧｲﾙにのみ書き込む
                    TrimLogging_Write(dataName & proc, outFile:=1)      ' ｶｯﾄ条件ﾛｸﾞﾌｧｲﾙに出力する

                Case 3  ' 同じﾛｸﾞﾌｧｲﾙとｶｯﾄ条件ﾛｸﾞﾌｧｲﾙに書き込む
                    TrimLogging_Write(dataName & proc, outFile:=1)      ' ｶｯﾄ条件ﾛｸﾞﾌｧｲﾙに出力する
                    TrimLogging_Write(proc)                             ' 通常のﾛｸﾞﾌｧｲﾙに出力する

                Case Else ' 処理は書き込まない
                    ' DO NOTHING
            End Select

        Else
            ' ｶｯﾄ条件も出力する
            Dim cut As String = String.Empty
            If (String.Empty <> dataName) Then dataName &= Environment.NewLine

            Select Case gSysPrm.stLOG.giLogPutCutCond
                Case 1  ' 同じﾛｸﾞﾌｧｲﾙにｶｯﾄ条件も書き込む
                    TrimLogging_MakeLogCutCondition(procMsg, cut)
                    TrimLogging_Write(cut)                              ' 通常のﾛｸﾞﾌｧｲﾙに出力する

                Case 2  ' ｶｯﾄ条件ﾛｸﾞﾌｧｲﾙにのみ書き込む
                    TrimLogging_MakeLogCutCondition(procMsg, cut)
                    TrimLogging_Write(dataName & cut, outFile:=1)       ' ｶｯﾄ条件ﾛｸﾞﾌｧｲﾙに出力する

                Case 3  ' 同じﾛｸﾞﾌｧｲﾙとｶｯﾄ条件ﾛｸﾞﾌｧｲﾙに書き込む
                    TrimLogging_MakeLogCutCondition(procMsg, cut)
                    TrimLogging_Write(dataName & cut, outFile:=1)       ' ｶｯﾄ条件ﾛｸﾞﾌｧｲﾙに出力する
                    TrimLogging_Write(cut)                              ' 通常のﾛｸﾞﾌｧｲﾙに出力する

                Case Else ' ｶｯﾄ条件は書き込まない
                    ' DO NOTHING
            End Select
        End If

    End Sub
#End Region
    'V4.0.0.0-65        ↑↑↑↑
#Region "ロギング 書込み"
    '''=========================================================================
    '''<summary>ロギング 書込み</summary>
    ''' <param name="createFile">True:新規作成, False:作成しない  V4.0.0.0-65</param>
    ''' <param name="outFile">出力先ﾌｧｲﾙ指定 0:通常ﾛｸﾞﾌｧｲﾙ, 1:ｶｯﾄ条件ﾌｧｲﾙ  V4.0.0.0-65</param>
    '''<remarks>LoggingWriteはトリミング終了時に実施する</remarks>
    '''=========================================================================
    Private Function TrimLogging_Write(ByRef s As String,
                                       Optional ByVal createFile As Boolean = False,
                                       Optional ByVal outFile As Integer = 0) As Short
        Dim n As Short
        'Dim ff As Short
        Dim sPath As String
        Dim strFile As String
        Dim strExt As String

        '    gsDataLogStr = gsDataLogStr + s

        ' ログファイル書き込み
        '    If giLoggingMode Then
        'If (gLoggingHeader) Then ' ﾍｯﾀﾞｰ書き込み指示なのでﾌｧｲﾙ名作成(西暦から秒まで)
        If (True = createFile) Then ' ﾌｧｲﾙ新規作成              'V4.0.0.0-65
            strLogFileNameDate = Today.ToString("yyyyMMdd") & TimeOfDay.ToString("HHmmss")
        End If

        ' データファイル名の抽出
        n = InStrRev(gSysPrm.stLOG.gsLoggingFile, ".", , CompareMethod.Text)
        If n > 0 Then
            strFile = Left(gSysPrm.stLOG.gsLoggingFile, n - 1)
            strExt = Right(gSysPrm.stLOG.gsLoggingFile, Len(gSysPrm.stLOG.gsLoggingFile) - (n - 1))
        Else
            strFile = gSysPrm.stLOG.gsLoggingFile
            strExt = ""
        End If
        'V4.0.0.0-65                    ↓↓↓↓
        'sPath = gSysPrm.stLOG.gsLoggingDir & strFile & strLogFileNameDate & strExt
        Select Case (outFile)
            Case 1
                sPath = gSysPrm.stLOG.gsLoggingDir & strFile & "_" & strLogFileNameDate & "_Cut" & strExt
            Case Else
                sPath = gSysPrm.stLOG.gsLoggingDir & strFile & "_" & strLogFileNameDate & strExt
                LogFileSaveName = sPath                     'V1.13.0.0⑫
        End Select
        'V4.0.0.0-65                    ↑↑↑↑

        On Error GoTo ERROR01
        'ff = FreeFile()
        Dim append As Boolean
        If (0 <> gSysPrm.stLOG.giLoggingAppend) Then               ' ロギングはアペンドモード ?
            'FileOpen(ff, sPath, OpenMode.Append)
            append = True                                               ' 追記    V4.4.0.0①
        Else
            'FileOpen(ff, sPath, OpenMode.Output)
            append = False                                              ' 上書    V4.4.0.0①
        End If

        Using sw As New StreamWriter(sPath, append, Encoding.UTF8)      ' UTF8 BOM有 notepad.exe 向け   V4.4.0.0-0
            sw.WriteLine(s)
        End Using

        'PrintLine(ff, s)
        'FileClose(ff)
        ' IDRead用のLog書き込み 'V1.13.0.0⑥
        'If typPlateInfo.intIDReaderUse = 1 Then                        ' ここでは何を出力したい？ V4.0.0.0-65
        If typPlateInfo.intIDReaderUse = 1 AndAlso (0 = outFile) Then   ' V4.0.0.0-65
            MakeTmpLogFile(globalLogFileName, s)
        End If
        On Error GoTo 0

        '    End If

        '    gsDataLogStr = ""

        Exit Function

ERROR01:
        Resume ERROR02
ERROR02:
        On Error GoTo 0
        ' "ログファイルの書込みでエラーが発生しました"
        Call Form1.System1.TrmMsgBox(gSysPrm, MSG_LOGERROR, MsgBoxStyle.OkOnly, TITLE_4)
        Call Form1.System1.OperationLogging(gSysPrm, MSG_LOGERROR, gsDataLogPath)

    End Function
#End Region
    '----- V1.22.0.0④↓ -----
    '=========================================================================
    '   サマリーロギング関係処理(オプション)
    '=========================================================================
#Region "サマリーロギング用データを初期化する"
    '''=========================================================================
    ''' <summary>サマリーロギング用データを初期化する</summary>
    ''' <param name="stSummaryLog">(OUT)サマリーロギング用データ</param>
    '''=========================================================================
    Public Sub SummaryLoggingDataInit(ByRef stSummaryLog As SummaryLog)

        Dim Rn As Integer
        Dim strMSG As String

        Try
            ' サマリーログ出力なしならNOP
            If (giSummary_Log = SUMMARY_NONE) Then Return

            ' サマリーロギング用データ設定
            With stSummaryLog
                ' サマリーロギング詳細項目(トータル)
                .stTotalSubData.lngItLow = 0                            ' IT LO NG抵抗数
                .stTotalSubData.lngItHigh = 0                           ' IT HI NG抵抗数
                .stTotalSubData.lngFtLow = 0                            ' FT LO NG抵抗数
                .stTotalSubData.lngFtHight = 0                          ' FT HI NG抵抗数
                .stTotalSubData.lngOpen = 0                             ' オープンチェックエラー抵抗数
                .stTotalSubData.lngItTotal = 0                          ' IT NG抵抗数
                .stTotalSubData.lngFtTotal = 0                          ' FT NG抵抗数

                ' サマリーロギング情報
                .strStartTime = ""                                      ' サマリー開始時間
                .strEndTime = ""                                        ' サマリー終了時間

                ' サマリーロギング情報(抵抗毎1-999)
                For Rn = 1 To 999                                      ' 最大抵抗数分繰り返す 
                    .RegAry(Rn).lngItLow = 0
                    .RegAry(Rn).lngItHigh = 0
                    .RegAry(Rn).lngFtLow = 0
                    .RegAry(Rn).lngItHigh = 0
                    .RegAry(Rn).lngFtHight = 0
                    .RegAry(Rn).lngOpen = 0
                    .RegAry(Rn).lngItTotal = 0
                    .RegAry(Rn).lngFtTotal = 0
                Next

            End With

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.SummaryLoggingDataInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "サマリーロギング用データ設定処理"
    '''=========================================================================
    ''' <summary>サマリーロギング用データ設定処理</summary>
    ''' <param name="stSummaryLog">(OUT)サマリーロギング用データ</param>
    '''=========================================================================
    Private Sub SummaryLoggingDataSet(ByRef stSummaryLog As SummaryLog, ByVal digH As Integer, ByVal digL As Integer)

        Dim Idx As Integer
        Dim Rn As Integer
        Dim strMSG As String

        Try
            ' サマリーログ出力なしならNOP
            If (giSummary_Log = SUMMARY_NONE) Then Return

            ' トリミングモードがx0,x1,x2モード以外ならNOP
            If (digL > 2) Then Return

            ' サマリーロギング用データ設定
            With stSummaryLog
                ' サマリーロギング情報(抵抗毎1-999)
                For Idx = 1 To gRegistorCnt                             ' 抵抗数分繰り返す 
                    If (typResistorInfoArray(Idx).intResNo < 1000) Then
                        Rn = typResistorInfoArray(Idx).intResNo         ' Rn = 抵抗番号(1-999) 

                        Select Case (gwTrimResult(Idx - 1))
                            Case TRIM_RESULT_IT_LONG
                                .RegAry(Rn).lngItLow = .RegAry(Rn).lngItLow + 1
                            Case TRIM_RESULT_IT_HING
                                .RegAry(Rn).lngItHigh = .RegAry(Rn).lngItHigh + 1
                            Case TRIM_RESULT_FT_LONG
                                .RegAry(Rn).lngFtLow = .RegAry(Rn).lngFtLow + 1
                            Case TRIM_RESULT_FT_HING
                                .RegAry(Rn).lngFtHight = .RegAry(Rn).lngFtHight + 1
                            Case TRIM_RESULT_OVERRANGE
                                .RegAry(Rn).lngOpen = .RegAry(Rn).lngOpen + 1
                        End Select
                    End If
                Next

            End With

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.SummaryLoggingDataSet() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "サマリーログデータファイルを出力する"
    '''=========================================================================
    '''<summary>サマリーログデータファイルを出力する</summary>
    '''<remarks>LoggingWriteはトリミング終了時に実施する</remarks>
    '''=========================================================================
    Public Function SummaryLoggingWrite(ByVal digH As Integer, ByVal digL As Integer) As Integer

        'Dim writer As System.IO.StreamWriter = Nothing
        'Dim bFlg As Boolean = False
        Dim blnRdataTitle As Boolean = False
        Dim RtnCode As Integer = cFRS_NORMAL
        Dim strFilePath As String = ""
        Dim strMSG As String
        Dim strOutText As String                                        ' 出力内容
        Dim intStrCnt As Integer                                        ' 文字桁数
        Dim strDAT(2) As String                                         ' 出力項目(配列0:タイトル、1:値、2:%データ)
        Dim intArrayCnt As Integer
        Dim intRno As Integer                                           ' 抵抗番号
        Dim TotalCnt As Long
        Dim ItTotalCnt As Long
        Dim FtTotalCnt As Long
        Dim OpenCnt As Long

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' サマリーログ出力なしならNOP
            If (giSummary_Log = SUMMARY_NONE) Then Return (cFRS_NORMAL)

            ' トリミングモードがx0,x1,x2モード以外ならNOP
            If (digL > 2) Then Return (cFRS_NORMAL)

            ' サマリーロギングファイル名の作成 = "C:\TRIMDATA\LOG\加工データ名 + "_SUMMARY.TXT"
            strFilePath = MakeSummaryFileName()
            If (strFilePath = "") Then Return (cFRS_NORMAL)

            '' サマリログファイルオープン(false = 上書き(true = 追加))
            'writer = New System.IO.StreamWriter(strFilePath, False, System.Text.Encoding.GetEncoding("Shift_JIS"))
            'bFlg = True

            '-------------------------------------------------------------------
            '   サマリーロギング出力
            '-------------------------------------------------------------------
            ' トリミングデータ名、開始時間、終了時間を出力する
            strOutText = ""
            Sub_GetFileName(strFilePath, strDAT(0))                     ' パス名からサマリーロギングファイル名を取り出す
            strOutText = strOutText & "FILE: " & strDAT(0) & vbCrLf     ' FILE :トリミングデータ名  
            strOutText = strOutText & "START TIME: " & stSummaryLog.strStartTime & vbCrLf
            strOutText = strOutText & "END TIME  : " & stSummaryLog.strEndTime & vbCrLf
            strOutText = strOutText & vbCrLf

            ' Total Countを出力する
            intStrCnt = 16                                              ' 文字桁数 = 16文字(左側に空白パディング)
            strDAT(0) = "Total Count".PadLeft(intStrCnt)
            strDAT(1) = ((m_lGoodCount + m_lNgCount).ToString("0")).PadLeft(intStrCnt)
            strOutText = strOutText & vbCrLf & strDAT(0) & vbCrLf & strDAT(1) & vbCrLf & vbCrLf

            ' Go Count, Ng Count(見出し)を出力する
            intStrCnt = 13                                              ' 文字桁数 = 13文字(左側に空白パディング)
            strDAT(0) = "Go Count".PadLeft(intStrCnt) & ","
            strDAT(1) = "Ng Count".PadLeft(intStrCnt)
            strOutText = strOutText & vbCrLf & strDAT(0) & strDAT(1) & vbCrLf
            ' OK, NG数を出力する
            intStrCnt = 13                                              ' 文字桁数 = 13文字(左側に空白パディング)
            strDAT(0) = m_lGoodCount.ToString("0").PadLeft(intStrCnt) & ","
            strDAT(1) = m_lNgCount.ToString("0").PadLeft(intStrCnt)
            strOutText = strOutText & strDAT(0) & strDAT(1)
            ' OK, NG比率(%)を出力する
            intStrCnt = 13                                              ' 文字桁数 = 13文字(左側に空白パディング)
            TotalCnt = m_lGoodCount + m_lNgCount
            If (TotalCnt = 0) Then
                strDAT(0) = "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt)
            Else
                strDAT(0) = (((m_lGoodCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," & (((m_lNgCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt)
            End If
            strOutText = strOutText & vbCrLf & strDAT(0) & vbCrLf & vbCrLf

            ' IT.Low～Openを出力する
            ItTotalCnt = m_lITLONGCount + m_lITHINGCount
            FtTotalCnt = m_lFTLONGCount + m_lFTHINGCount
            OpenCnt = m_lITOVERCount
            intStrCnt = 13                                              ' 文字桁数 = 16文字(左側に空白パディング)
            strDAT(0) = "     IT.Low,      IT.High,     IT.Total,       FT.Low,      FT.High,     FT.Total,         Open"
            If (TotalCnt = 0) Then
                strDAT(1) = "0".PadLeft(11) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt) & "," & "0".PadLeft(intStrCnt)
                strDAT(2) = "0.000%".PadLeft(11) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt) & "," & "0.000%".PadLeft(intStrCnt)
            Else
                strDAT(1) = m_lITLONGCount.ToString("0").PadLeft(11) & "," & m_lITHINGCount.ToString("0").PadLeft(intStrCnt) & "," &
                            ItTotalCnt.ToString("0").PadLeft(intStrCnt) & "," & m_lFTLONGCount.ToString("0").PadLeft(intStrCnt) & "," &
                            m_lFTHINGCount.ToString("0").PadLeft(intStrCnt) & "," & FtTotalCnt.ToString("0").PadLeft(intStrCnt) & "," &
                            OpenCnt.ToString("0").PadLeft(intStrCnt)
                strDAT(2) = (((m_lITLONGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(11) & "," &
                            (((m_lITHINGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((ItTotalCnt / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((m_lFTLONGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((m_lFTHINGCount / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((FtTotalCnt / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt) & "," &
                            (((OpenCnt / TotalCnt) * 100).ToString("0.000") & "%").PadLeft(intStrCnt)
            End If
            strOutText = strOutText & vbCrLf & strDAT(0) & vbCrLf & strDAT(1) & vbCrLf & strDAT(2) & vbCrLf

            ' 抵抗毎のデータを出力する
            intStrCnt = 13                                              ' 文字桁数 = 13文字(左側に空白パディング) 
            strDAT(0) = "     R-No,         Open,       IT.Low,      IT.High,       FT.Low,      FT.Higt,        Total"
            For intArrayCnt = 0 To (gRegistorCnt - 1)
                intRno = typResistorInfoArray(intArrayCnt + 1).intResNo ' 抵抗番号
                ' 抵抗データの場合
                If (intRno < 1000) Then

                    ' 対応する抵抗番号の情報を出力
                    With stSummaryLog.RegAry(intRno)
                        TotalCnt = .lngItLow + .lngItHigh + .lngFtLow + .lngFtHight + .lngOpen
                        strDAT(1) = intRno.ToString("0").PadLeft(9) & "," & .lngOpen.ToString("0").PadLeft(intStrCnt) & "," &
                                    .lngItLow.ToString("0").PadLeft(intStrCnt) & "," & .lngItHigh.ToString("0").PadLeft(intStrCnt) & "," &
                                    .lngFtLow.ToString("0").PadLeft(intStrCnt) & "," & .lngFtHight.ToString("0").PadLeft(intStrCnt) & "," &
                                    TotalCnt.ToString("0").PadLeft(intStrCnt)

                        ' タイトル行をまだ出力していない ?
                        If (blnRdataTitle = False) Then
                            blnRdataTitle = True
                            strOutText = strOutText & vbCrLf & vbCrLf & strDAT(0)
                        End If

                        ' 出力内容の設定
                        strOutText = strOutText & vbCrLf & strDAT(1)
                    End With
                End If
            Next intArrayCnt

            ' ファイルへ書き込み
            ' サマリログファイルオープン(false = 上書き(true = 追加))
            Using writer As New StreamWriter(strFilePath, False, Encoding.UTF8) 'V4.4.0.0-0
                writer.WriteLine(strOutText)
            End Using

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
STP_END:
            'writer.Close()
            Return (RtnCode)                                            ' Return値設定

            ' トラップエラー発生時 
        Catch ex As Exception
            ' "ログファイルの書込みでエラーが発生しました"
            strMSG = MSG_LOGERROR + " File = " + strFilePath + " Error = " + ex.Message
            Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, TITLE_4)
            Call Form1.System1.OperationLogging(gSysPrm, strMSG, "")
            RtnCode = cERR_TRAP                                         ' Return値 = トラップエラー発生
            GoTo STP_END
        End Try
    End Function
#End Region
    '----- V1.22.0.0④↑ -----

    '=========================================================================
    '   基板ディレイ(ディレイトリム２)ログ関係処理(オプション)
    '=========================================================================
#Region "ﾃﾞｨﾚｲﾄﾘﾑ2用構造体の初期化"
    '''=========================================================================
    '''<summary>ﾃﾞｨﾚｲﾄﾘﾑ2用構造体の初期化 V1.23.0.0⑥</summary>
    ''' <param name="BlkXCount">(INP)ブロック数X</param>
    ''' <param name="BlkYCount">(INP)ブロック数Y</param>
    ''' <param name="RegCount"> (INP)抵抗数</param>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Init_stDelay2(ByVal BlkXCount As Integer, ByVal BlkYCount As Integer, ByVal RegCount As Integer)

        Dim BXn As Integer
        Dim BYn As Integer
        Dim strMSG As String

        Try
            ' ﾃﾞｨﾚｲﾄﾘﾑ2用構造体の初期化
            stDelay2.Initialize(BlkXCount, BlkYCount)

            For BXn = 0 To (BlkXCount - 1)
                For BYn = 0 To (BlkYCount - 1)
                    stDelay2.NgAry(BXn, BYn).Initialize(RegCount)
                Next BYn
            Next BXn

            Exit Sub

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.Init_stDelay2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ﾃﾞｨﾚｲﾄﾘﾑ2実行時のNGﾁｪｯｸ用構造体の初期化"
    '''=========================================================================
    '''<summary>ﾃﾞｨﾚｲﾄﾘﾑ2実行時のNGﾁｪｯｸ用構造体の初期化</summary>
    ''' <param name="BlkX">(INP)ブロック番号X(1-n) V1.23.0.0⑥</param>
    ''' <param name="BlkY">(INP)ブロック番号Y(1-n) V1.23.0.0⑥</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetDefaultData(ByVal BlkX As Integer, ByVal BlkY As Integer)

        Dim intForCnt As Integer
        Dim strMSG As String

        Try
            ' 配列のNG情報をクリアする
            If (m_blnDelayCheck = False) Then Return '              ' ディレイトリム２でなければNOP V1.23.0.0⑥
            For intForCnt = 0 To (gRegistorCnt - 1)                 ' V1.23.0.0⑥
                stDelay2.NgAry(BlkX - 1, BlkY - 1).intNgFlag = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intITHiNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intITLoNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intFTHiNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intFTLoNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intOverNgCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intTotalNGCnt = 0
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).intTotalOkCnt = 0
            Next intForCnt

            Exit Sub

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.SetDefaultData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ﾃﾞｨﾚｲﾄﾘﾑ2 ｲﾆｼｬﾙﾃｽﾄの結果(測定値)を保持しておく"
    '''=========================================================================
    '''<summary>ﾃﾞｨﾚｲﾄﾘﾑ2 ｲﾆｼｬﾙﾃｽﾄの結果(測定値)を保持しておく</summary>
    ''' <param name="BlkX">(INP)ブロック番号X V1.23.0.0⑥</param>
    ''' <param name="BlkY">(INP)ブロック番号Y V1.23.0.0⑥</param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetInitialTestResultData(ByVal BlkX As Integer, ByVal BlkY As Integer)

        Dim intForCnt As Short
        Dim strMSG As String

        Try
            ' ｲﾆｼｬﾙﾃｽﾄの結果(測定値)を抵抗数分保持しておく
            For intForCnt = 0 To (gRegistorCnt - 1)                     ' V1.23.0.0⑥
                stDelay2.NgAry(BlkX - 1, BlkY - 1).tNgCheck(intForCnt).dblInitialTest = gfInitialTest(intForCnt)
            Next

            Exit Sub

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.SetInitialTestResultData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '=========================================================================
    '   ＥＳログ関係処理(オプション)
    '=========================================================================
#Region "ES 判定結果の取得"
    '''=========================================================================
    '''<summary>ES 判定結果の取得</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TrimLoggingResult_ES()

        On Error GoTo ErrExit

        Dim intRegNum As Short                          ' 抵抗ﾙｰﾌﾟｶｳﾝﾀ(抵抗番号)
        Dim intEsRegCnt As Short
        Dim blnRet As Boolean

        ' 初期化
        System.Array.Clear(gtyESTestResult, 0, gtyESTestResult.Length)

        intEsRegCnt = 0

        ' 抵抗数分ﾙｰﾌﾟ
        For intRegNum = 1 To gRegistorCnt

            ' 抵抗毎のES判定結果の測定
            blnRet = TrimLoggingResult_Es_Reg(intRegNum, intEsRegCnt)

            ' ESｶｯﾄのある抵抗だった
            If blnRet = True Then
                ' ESｶｯﾄを持つ抵抗数のｶｳﾝﾀをｱｯﾌﾟ
                intEsRegCnt = intEsRegCnt + 1
            End If

            If intEsRegCnt >= 10 Then
                ' 最大１０個取得で終了
                Exit For
            End If
        Next

        Exit Sub

ErrExit:
        MsgBox("TrimLoggingResult_ES" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "ES 判定結果の取得(1抵抗分)"
    '''=========================================================================
    '''<summary>ES 判定結果の取得(1抵抗分)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function TrimLoggingResult_Es_Reg(ByRef intRegNum As Short, ByRef intEsRegCnt As Short) As Boolean

        Dim intCutMax As Short                              ' ｶｯﾄ数
        Dim intCutNum As Short                              ' ｶｯﾄ数ﾙｰﾌﾟｶｳﾝﾀ(ｶｯﾄ番号)
        Dim intCutIndex As Short                            ' ｶｯﾄｲﾝﾃﾞｯｸｽ(ESｶｯﾄだけで数えて何個目か)
        Dim intRetMeasMax(2) As UShort                      ' 測定値数　※配列番号1～2はごみ情報
        Dim intLoopMax As Short                             ' ﾙｰﾌﾟ最大値
        Dim intLoopCnt As Short                             ' ﾙｰﾌﾟｶｳﾝﾀ
        Dim dGetMeas(15) As Double                          ' 結果格納(128ﾊﾞｲﾄ分) ※配列番号15はごみ情報
        Dim intArrayMeas As Short                           ' 測定配列ｲﾝﾃﾞｯｸｽ
        Dim intTempMax As Short                             ' 1回で取得する測定値数の最大値
        Dim intTempCnt As Short                             ' 1回で取得する測定値数のｶｳﾝﾀ
        Dim blnINtimeSend As Boolean                        ' INtimeに送信する必要があるか判別ﾌﾗｸﾞ

        ' ｶｯﾄ数を取得する。
        intCutMax = typResistorInfoArray(intRegNum).intCutCount

        ' ｶｯﾄ情報の配列作成
        'UPGRADE_WARNING: 配列 gtyESTestResult(intRegNum).iCutNo の下限が 1 から 0 に変更されました。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"' をクリックしてください。
        ReDim Preserve gtyESTestResult(intRegNum).iCutNo(intCutMax)

        ' 初期化
        intCutIndex = -1

        ' ｶｯﾄ数分ﾙｰﾌﾟ
        For intCutNum = 1 To intCutMax

            Select Case UCase(typResistorInfoArray(intRegNum).ArrCut(intCutNum).strCutType)

                Case "K", "S" ' ｶｯﾄ種別がエッジセンス
                    ' ESｶｯﾄの存在ﾌﾗｸﾞをON
                    gtyESTestResult(intRegNum).bEsExsit = True
                    ' 何個目のESか取得
                    intCutIndex = intCutIndex + 1
                    blnINtimeSend = True                    ' ESなのでINtimeに送信
                Case Else
                    blnINtimeSend = False                   ' ESでないので何もしない
            End Select

            If intCutIndex > 3 Then
                blnINtimeSend = False
            End If

            If blnINtimeSend = True Then

                ' 測定値数を取得
                '            AppInfo.cmdNo = CMD_TRIM_RESULT
                '            AppInfo.dwPara(0) = 7               '測定値数取得ｺﾏﾝﾄﾞ(ES)
                '            AppInfo.dwPara(1) = intEsRegCnt     '抵抗番号(0～) ※ESを持つ抵抗だけで数えて何個目かを渡す
                '            AppInfo.dwPara(3) = intCutIndex     'ｶｯﾄ数(0～) ※ESだけで数えて何個目かを渡す
                '
#If cOFFLINEcDEBUG Then
#Else
                ' 測定値数(ES2)の取得
                Call TRIM_RESULT_WORD(7, intEsRegCnt, 0, intCutIndex, 0, intRetMeasMax(0))
#End If

                ' 測定値数の個数分で配列作成
                ReDim Preserve gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intRetMeasMax(0))

                ' 1回で取得可能な個数は128ﾊﾞｲﾄまでなので、何回取る必要があるか算出
                ' (取得はDouble型なので8ﾊﾞｲﾄ。但し、ごみの情報が8ﾊﾞｲﾄ必ず取れるので1回で取れる個数は15個まで)
                intLoopMax = intRetMeasMax(0) \ 15

                ' 割り切れる？
                If (intRetMeasMax(0) Mod 15) > 0 Then
                    intLoopMax = intLoopMax + 1             ' 1回多くとる
                End If

                intArrayMeas = -1
                For intLoopCnt = 1 To intLoopMax
                    System.Array.Clear(dGetMeas, 0, dGetMeas.Length)
                    ' 最終の場合
                    If intLoopCnt = intLoopMax Then
                        ' 余りを取得
                        intTempMax = intRetMeasMax(0) Mod 15
                        ' 余り無しの場合
                        If intTempMax = 0 Then
                            intTempMax = 15
                        End If
                    Else
                        intTempMax = 15
                    End If

                    '測定値取得
                    '                AppInfo.cmdNo = CMD_TRIM_RESULT
                    '                AppInfo.dwPara(0) = 8               '測定値取得ｺﾏﾝﾄﾞ(ES)
                    '                AppInfo.dwPara(1) = intEsRegCnt     '抵抗番号(0～) ※ESを持つ抵抗だけで数えて何個目かを渡す
                    '                AppInfo.dwPara(2) = intTempMax      '取得個数
                    '                AppInfo.dwPara(3) = intCutIndex             'ｶｯﾄ番号(0～)   ※ESだけで数えて何個目かを渡す
                    '                AppInfo.dwPara(4) = (intLoopCnt - 1) * 15  'ﾃﾞｰﾀ番号(0～)
                    '
#If cOFFLINEcDEBUG Then
#Else
                    Call TRIM_RESULT_Double(8, intEsRegCnt, intTempMax, intCutIndex, (intLoopCnt - 1) * 15, dGetMeas(0))
#End If

                    For intTempCnt = 1 To intTempMax
                        ' 測定値を格納
                        intArrayMeas = intArrayMeas + 1
                        ReDim Preserve gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intArrayMeas)
                        gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intArrayMeas) = dGetMeas(intTempCnt - 1)

                    Next

                Next  '測定値の取得分ﾙｰﾌﾟ

            End If

        Next  ' ｶｯﾄ数分ﾙｰﾌﾟ

        If gtyESTestResult(intRegNum).bEsExsit = True Then
            ' ESカットが１件でもあった場合
            TrimLoggingResult_Es_Reg = True
        Else
            ' ESカットが１件もなかった場合
            TrimLoggingResult_Es_Reg = False
        End If

    End Function
#End Region

#Region "ES ﾛｸﾞ出力"
    '''=========================================================================
    '''<summary>ES ﾛｸﾞ出力</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub LoggingWrite_ES()

        On Error GoTo ErrExit

        Dim intRegNum As Short
        'Dim intFileNo As Short
        Dim strRet As String
        Dim strDATA As String
        Dim strFileName As String
        Dim intPos As Short

        ' 抵抗数分ﾙｰﾌﾟ
        strDATA = ""
        For intRegNum = 1 To gRegistorCnt
            ' ESｶｯﾄが存在する抵抗？
            If gtyESTestResult(intRegNum).bEsExsit = True Then
                strRet = LoggingWrite_ES_GetData(intRegNum)
                strDATA = strDATA & strRet
            End If
        Next

        ' 出力する内容がある
        If Len(strDATA) > 0 Then
            intPos = InStrRev(gsESLogFilePath, "\")
            strFileName = Mid(gsESLogFilePath, intPos + 1)  ' ファイル名取得（拡張子含む）

            'ファイルが存在しない場合は、ファイル名を出力する
            'UPGRADE_WARNING: Dir に新しい動作が指定されています。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"' をクリックしてください。
            If Len(Dir(gsESLogFilePath)) = 0 Then
                strDATA = "File name:" & strFileName & vbCrLf & vbCrLf & strDATA
            End If

            ' ﾌｧｲﾙへ書き込み
            'intFileNo = FreeFile()
            'FileOpen(intFileNo, gsESLogFilePath, OpenMode.Append)
            'PrintLine(intFileNo, strDATA)
            'FileClose(intFileNo)
            Using sw As New StreamWriter(gsESLogFilePath, True, Encoding.UTF8)  ' 追記 UTF8 BOM有 notepad.exe 向け   V4.4.0.0-0
                sw.WriteLine(strDATA)
            End Using

        End If

        Exit Sub

ErrExit:
        MsgBox("LoggingWrite_ES" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub

#End Region

#Region "ES ﾛｸﾞ内容の作成"
    '''=========================================================================
    '''<summary>ES ﾛｸﾞ内容の作成</summary>
    '''<param name="intRegNum">(INP) 対象のカット番号</param>
    '''<returns>ES ﾛｸﾞ</returns>
    '''=========================================================================
    Private Function LoggingWrite_ES_GetData(ByRef intRegNum As Short) As String
        On Error GoTo ErrExit

        Dim intCutMax As Short
        Dim intCutNum As Short
        Dim intMeasCnt As Short
        Dim strDATA As String
        Dim strDATAMeas As String

        ' ｶｯﾄ数を取得する
        strDATA = ""
        intCutMax = typResistorInfoArray(intRegNum).intCutCount

        ' ｶｯﾄ数分ﾙｰﾌﾟ
        For intCutNum = 1 To intCutMax
            strDATAMeas = ""
            Select Case UCase(typResistorInfoArray(intRegNum).ArrCut(intCutNum).strCutType)

                Case "K", "S"                           ' ｶｯﾄ種別がエッジセンス
                    strDATA = strDATA & "R" & typResistorInfoArray(intRegNum).intResNo.ToString("000")
                    strDATA = strDATA & ","
                    strDATA = strDATA & "C" & intCutNum.ToString("00")

                    'For intMeasCnt = 1 To UBound(gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas)
                    For intMeasCnt = 1 To (UBound(gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas) + 1) ' V1.14.0.0①
                        If Len(strDATAMeas) > 0 Then
                            strDATAMeas = strDATAMeas & ","
                        End If

                        ' 測定値
                        'strDATAMeas = strDATAMeas & gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intMeasCnt).ToString("0.00000")
                        strDATAMeas = strDATAMeas & gtyESTestResult(intRegNum).iCutNo(intCutNum).dblMeas(intMeasCnt - 1).ToString("0.00000") ' V1.14.0.0①
                    Next
                    strDATA = strDATA & "," & strDATAMeas & vbCrLf

                Case Else

            End Select

        Next

        LoggingWrite_ES_GetData = strDATA

        Exit Function

ErrExit:
        LoggingWrite_ES_GetData = ""
        Err.Clear()
    End Function

#End Region

#Region "SetHighLow()←未使用"
    '''=========================================================================
    '''<summary>SetHighLow()</summary>
    '''<param name="intForCnt">(INP)</param> 
    '''<param name="fTest">    (INP) </param>  
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetHighLow(ByRef intForCnt As Short, ByRef fTest() As Double, ByRef fTarget() As Double,
                           ByRef hl As Double, ByRef ll As Double)

        Dim dblr_diff As Double
        Dim intbr As Short

        Try
            With typResistorInfoArray(intForCnt)
                ' ﾄﾘﾑﾓｰﾄﾞのﾁｪｯｸを行う。
                Select Case .intTargetValType

                    ' 絶対値
                    Case TARGET_TYPE_ABSOLUTE

                        ' (ﾃｽﾄ結果/ﾄﾘﾐﾝｸﾞ目標値*100-100)の計算結果を取得する。
                        dblr_diff = (fTest(intForCnt - 1) / .dblTrimTargetVal) * 100.0# - 100.0#

                        ' レシオ
                    Case TARGET_TYPE_RATIO
                        ' ﾍﾞｰｽ抵抗No.より、ﾚｼｵﾍﾞｰｽNo.を取得する。
                        intbr = GetRatio1BaseNum(.intBaseResNo)
                        ' 取得したﾍﾞｰｽNo.が0より小さくないかﾁｪｯｸする。
                        If intbr < 0 Then
                            ' 0より小さい場合には、ｴﾗｰﾒｯｾｰｼﾞを出力する。
                            MsgBox("Internal error X003", MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                            intbr = 0
                        End If

                        ' (ベース抵抗実測値＊レシオ)より目標値を取得する。
                        dblr_diff = fTest(intbr) * .dblTrimTargetVal
                        ' 取得した目標値が0以外であることを確認する。
                        If dblr_diff <> 0.0# Then

                            ' 0以外の場合は、(ﾃｽﾄ結果/取得目標値*100-100)の計算結果を取得する。
                            dblr_diff = (fTest(intForCnt - 1) / dblr_diff) * 100.0# - 100.0#
                        Else
                            ' 目標値が0の場合には、計算結果も0とする。
                            dblr_diff = 0
                        End If

                        ' 計算式
                    Case TARGET_TYPE_CALC
                        ' ﾚｼｵ目標値が0以外であることを確認する。
                        If fTarget(intForCnt - 1) <> 0.0# Then
                            dblr_diff = 0

                            ' (ﾃｽﾄ結果/ﾚｼｵ目標値*100-100)の計算結果を取得する。
                            dblr_diff = (fTest(intForCnt - 1) / fTarget(intForCnt)) * 100.0# - 100.0#
                        Else
                            ' ﾚｼｵ目標値が0の場合には、計算結果も0とする。
                            dblr_diff = 0
                        End If
                End Select

                If dblr_diff > hl Then
                    giTrimResult0x(intForCnt - 1) = 1
                ElseIf dblr_diff < ll Then
                    giTrimResult0x(intForCnt - 1) = 2
                ElseIf dblr_diff = 0 Then
                    giTrimResult0x(intForCnt - 1) = 3
                Else
                    giTrimResult0x(intForCnt - 1) = 4
                End If
            End With

        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub
#End Region

    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    ''''  090731　ロギング関係処理
    ''''_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
#Region "ﾄﾘﾐﾝｸﾞ結果ﾛｸﾞ共通表示部分の初期化"
    '''=========================================================================
    '''<summary>ﾄﾘﾐﾝｸﾞ結果ﾛｸﾞ共通表示部分の初期化</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetTrimResultCmnStr()
        ' 通常ログ
        '----- ###238↓ -----
        ' 10文字に揃える
        'gstrResult(0) = "0.0000  "
        'gstrResult(1) = "OK      "
        'gstrResult(2) = "IT NG   "
        'gstrResult(3) = "FT NG   "
        'gstrResult(4) = "SKIP    "
        'gstrResult(5) = "BASE NG "
        'gstrResult(6) = "IT HI NG"
        'gstrResult(7) = "IT LO NG"
        'gstrResult(8) = "FT HI NG"
        'gstrResult(9) = "FT LO NG"
        'gstrResult(10) = "OVER RNG"
        'gstrResult(11) = "OPEN"
        'gstrResult(12) = "IT OK TEST OK"
        gstrResult(0) = "0.0000    "
        gstrResult(1) = "OK        "
        gstrResult(2) = "IT NG     "
        gstrResult(3) = "FT NG     "
        gstrResult(4) = "SKIP      "
        gstrResult(5) = "BASE NG   "
        gstrResult(6) = "IT HI NG  "
        gstrResult(7) = "IT LO NG  "
        gstrResult(8) = "FT HI NG  "
        gstrResult(9) = "FT LO NG  "
        gstrResult(10) = "OVER RNG  "
        gstrResult(11) = "OPEN      "   '←INtime側からは帰ってこない(現状未使用)
        gstrResult(12) = "IT OK     "   '
        '----- ###248↓ -----
        gstrResult(13) = "PATTERN NG"
        gstrResult(14) = "CUT OK    "   ' 
        gstrResult(15) = "IKEI SKIP "   '←INtime側からは帰ってこない(現状未使用)
        '----- 'V1.13.0.0⑪↓ -----
        gstrResult(16) = "CV ERROR  "   ' 測定ばらつき検出
        gstrResult(17) = "OVER LOAD "   ' オーバロード検出
        gstrResult(18) = "RE PROB NG"   ' 再プロービングエラー
        '----- 'V1.13.0.0⑪↑ -----
        gstrResult(19) = "ES2 ERROR "   ' ES2エラー V1.14.0.1①
        gstrResult(20) = "OPEN NG   "
        gstrResult(21) = "SHORT NG  "
        '----- ###248↑ -----
        '----- ###238↑ -----
        'V1.20.0.1① ↓
        gstrResult(22) = "MID CUT NG"
        'V1.20.0.1① ↑

        '特注向けログ表示選択
        If gSysPrm.stSPF.giMeasOKNG <> 0 Then
            'OK/NG表示なしモード
            gstrResult(1) = ""
            gstrResult(6) = "H"
            gstrResult(7) = "L"
            gstrResult(8) = "H"
            gstrResult(9) = "L"
        End If

        'NETのみ→432HWのINTRTMでは、11以降の結果が設定されることはない。
        '   13,14は436Kトリミング結果の為、コメントアウトする。
        '    gstrResult(13) = "err13"
        '    gstrResult(14) = "ORL  "
        '===============================================

    End Sub
#End Region

#Region "θ補正の実行"
    '''=========================================================================
    '''<summary>θ補正の実行</summary>
    '''<param name="pltInfo">(INP) プレートデータ</param>
    '''<returns>cFRS_NORMAL   = 正常
    '''         cFRS_ERR_RST  = Cancel(RESETｷｰ)
    '''         cFRS_ERR_PTN  = パターン認識エラー
    '''         上記以外      = 非常停止等その他エラー
    '''</returns>
    '''=========================================================================
    Private Function DoCorrectPos(ByRef pltInfo As PlateInfo, ByVal doAlign As Boolean, ByVal doRough As Boolean) As Integer 'V5.0.0.9⑩
        'Private Function DoCorrectPos(ByRef pltInfo As PlateInfo) As Integer

        Dim strMSG As String = ""
        Dim r As Short
        Dim rtn As Integer = cFRS_NORMAL                                ' ###170

        Try
            ' ありの場合には、θ補正処理を行なう。
            'If (gSysPrm.stDEV.giTheta = 0) Then                         ' θなし ? 
            If (gSysPrm.stDEV.giTheta = 0) OrElse
                ((False = doAlign) AndAlso (False = doRough)) Then      ' θなし または θ補正もラフアライメントも実行しない     'V5.0.0.9⑩

                gfCorrectPosX = 0.0                                     ' 補正値初期化 
                gfCorrectPosY = 0.0
                gdCorrectTheta = 0.0   'V5.0.0.9⑩
                Return (cFRS_NORMAL)
            End If

            ' 画像表示プログラムを終了する ###156
            'V6.0.0.0⑤            Call End_GazouProc(ObjGazou)

            ' ''V5.0.1.0③↓ 'V6.0.5.0⑧
            If giGazouClrTime <> 0 Then
                Call Form1.VideoLibrary1.VideoStart()
                Sleep(giGazouClrTime)
                Call Form1.VideoLibrary1.VideoStop()
            End If
            ' ''V5.0.1.0③↑ 'V6.0.5.0⑧

            '----- V1.14.0.0⑤↓ -----
            ' LEDバックライト照明ON((オプション) ローダ自動モード時有効(432R))
            Call Set_Led(1, 1)                                          ' バックライト照明ON 

            '' バックライト照明初期設定(CHIP用)
            'If gSysPrm.stSPF.giLedOnOff = 1 Then                        ' LED制御あり ? 
            '    Call Form1.System1.Set_Led(1, stATLD.iLED, 1)           ' バックライト照明ＯＮ(LED制御)
            'End If
            '----- V1.14.0.0⑤↑ -----

            'θ補正処理
            '----- V1.15.0.0③↓ -----
            ' Zを待機位置へ移動する
            r = EX_ZMOVE(pltInfo.dblZWaitOffset, MOVE_ABSOLUTE)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)                                              ' Return値設定
            End If
            r = PROBOFF_EX(pltInfo.dblZWaitOffset)                      ' ROUND4のZOFF位置をzWaitPosとする
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            '----- V1.15.0.0③↑ -----

            strMSG = ""
            'r = Form1.SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG)
            r = Form1.SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG, doAlign, doRough) 'V5.0.0.9⑩
            If (r <> cFRS_NORMAL) Then                                  ' ERROR ?
                rtn = r                                                 ' Return値 ###170
                If (r <= cFRS_VIDEO_PTN) Then                           ' パターン認識エラー ?
                    Call Beep()                                         ' Beep音
                    If (strMSG = "") Then                               '###038
                        ' ログ画面に文字列を表示する(マッチングエラー, パターン番号エラー)
                        strMSG = MSG_LOADER_07 + " (" + gbRotCorrectCancel.ToString + ")"
                    Else
                        'If (pltInfo.intRecogDispMode = 0) Then          ' 結果表示しない ?
                        If (pltInfo.intRecogDispMode = 0) AndAlso
                            (0 = pltInfo.intRecogDispModeRgh) Then      ' 結果表示しない ? 'V5.0.0.9⑤
                            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                            '    ' パターンマッチングエラー(閾値)
                            '    strMSG = MSG_LOADER_07 + "(閾値)"
                            'Else
                            '    strMSG = MSG_LOADER_07 + "(Thresh)"
                            'End If
                            strMSG = MSG_LOADER_07 & Form1_008
                        End If
                    End If
                    ' メッセージ表示
                    Call Form1.Z_PRINT(strMSG)
                    '----- ###172↓ -----
                    If (bFgAutoMode = True) Then                        ' 自動運転時(SL436R) ? 
                        System.Threading.Thread.Sleep(500)              ' Wait(ms)
                    End If
                    '----- ###172↑ -----
                    rtn = cFRS_ERR_PTN                                  ' Return値 = パターン認識エラー ###170
                End If
            End If

            'θ補正情報表示
            If (r = cFRS_NORMAL) Then
                'If (pltInfo.intRecogDispMode = 1) Then                  ' 結果表示する ? '###038
                If (pltInfo.intRecogDispMode = 1) OrElse
                    (0 <> pltInfo.intRecogDispModeRgh) Then             ' 結果表示する ? 'V5.0.0.9⑤
                    Call Form1.Z_PRINT(strMSG)
                End If
            End If

            '----- V1.14.0.0⑤↓ -----
            ' LEDバックライト照明OFF(オプション)
            Call Set_Led(1, 0)                                          ' バックライト照明OFF 

            '' バックライト照明初期設定(CHIP用)
            'If gSysPrm.stSPF.giLedOnOff = 1 Then                        ' LED制御あり ? 
            '    Call Form1.System1.Set_Led(1, stATLD.iLED, 0)           ' バックライト照明ＯＦＦ(LED制御)
            'End If
            '----- V1.14.0.0⑤↑ -----
#If False Then                          'V6.0.0.0⑤
            ' 画像表示プログラムを起動する ###156
            If gKeiTyp = KEY_TYPE_RS Then
                '                r = Exec_SmallGazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)
                ' ↓↓↓ V3.1.0.0② 2014/12/01
                r = Exec_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0, 0)
                ' ↑↑↑ V3.1.0.0② 2014/12/01
            Else

                If (bIniFlg <> 0) And (Form1.chkDistributeOnOff.Checked = False) Then
                    ' 初回以外で統計画面非表示時に起動する
                    ' ↓↓↓ V3.1.0.0② 2014/12/01
                    r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0, 0)
                    ' ↑↑↑ V3.1.0.0② 2014/12/01
                End If
            End If
#End If
            Return (rtn)                                                ' ###170

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.DoCorrectPos() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                           ' 戻値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function

#End Region

#Region "プレートのチェック・表示更新"
    '''=========================================================================
    '''<summary>プレートのチェック・表示更新</summary>
    '''<param name="Flg"> (INP) </param> 
    '''<param name="mode">(INP) </param>  
    '''<returns>True= , False= </returns>
    '''=========================================================================
    Private Function CheckPlate(ByRef Flg As Boolean, ByRef mode As Short) As Boolean

        Dim strMSG As String

        Try

            If Not Flg Then
                '----- V6.0.3.0⑥↓ -----
                'If mode < 3 Then
                If (mode < 3) Or ((giPltCountMode = 1) And ((mode = 3) Or (mode = 5))) Then
                    '----- V6.0.3.0⑥↑ -----
                    m_lPlateCount = m_lPlateCount + 1                           ' PLATE数 += 1
                    '----- V1.18.0.0③↓ -----
                    ' stPRT_ROHM.Tol_Sheetはm_lPlateCountを使用するため下記は削除
                    'If (gTkyKnd = KND_CHIP) Then
                    '    ' (ﾄﾘﾐﾝｸﾞ結果印刷)
                    '    If gSysPrm.stDEV.rPrnOut_flg = True Then
                    '        stPRT_ROHM.Pdt_Sheet = stPRT_ROHM.Pdt_Sheet + 1    ' ﾄﾘﾐﾝｸﾞｽﾃｰｼﾞで処理した基板枚数
                    '    End If
                    'End If
                    '----- V1.18.0.0③↑ -----
                End If

                ' Frame1情報(生産管理情報)表示(PLATE数)
                Fram1LblAry(FRAM1_ARY_PLTNUM).Text = m_lPlateCount.ToString("0")
                Fram1LblAry(FRAM1_ARY_PLTNUM).Refresh()
                CheckPlate = True
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.CheckPlate() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function
#End Region

#Region "カット位置補正の実施の有無判定"   'V3.0.0.0③ ADD START
    Private IsCutPosCorrectExecute As Boolean = False

    Public Function IsCutPosCorrect() As Boolean
        Return (IsCutPosCorrectExecute)
    End Function
#End Region                                'V3.0.0.0③ END

#Region "カット位置補正用パターン登録情報テーブルを設定する"
    '''=========================================================================
    '''<summary>カット位置補正用パターン登録情報テーブルを設定する</summary>
    '''<param name="registorCnt">(INP)抵抗数</param>
    '''<param name="pstCutPos">  (OUT)パターン登録情報</param> 
    '''<returns>カット位置補正する抵抗数</returns>
    '''=========================================================================
    Public Function CutPosCorrectInit(ByVal registorCnt As Integer, ByRef pstCutPos() As CutPosCorrect_Info) As Short

        Dim rn As Integer
        Dim nm As Integer
        Dim Ix As Integer                                                                   '###059
        Dim strMSG As String

        IsCutPosCorrectExecute = False                                                      'V3.0.0.0③ ADD

        Try
            ' カット位置補正用パターン登録情報テーブルを初期化する
            nm = 0                                                                          ' カット位置補正する抵抗数
            For rn = 1 To registorCnt                                                       ' 全抵抗数分設定する 
                If (typResistorInfoArray(rn).intResNo < 1) Then Exit For
                ' パターン認識結果を初期化する
                Ix = rn - 1                                                                 ' カット位置補正構造体は0オリジン ###059  
                gStCutPosCorrData.corrResult(Ix) = 0                                        ' パターン認識結果 =0(補正なし)
                gStCutPosCorrData.corrPosX(Ix) = 0.0                                        ' ズレ量X
                gStCutPosCorrData.corrPosY(Ix) = 0.0                                        ' ズレ量Y

                'giCutPosRSLT(rn) = 0                                                        ' パターン認識結果 =0(補正なし)
                'gfCutPosDRX(rn) = 0.0                                                       ' ズレ量X
                'gfCutPosDRY(rn) = 0.0                                                       ' ズレ量Y
                gfCutPosCoef(rn) = 0.0                                                      ' 一致度
                ' カット位置補正用パターン登録情報テーブルを初期化する
                If (typResistorInfoArray(rn).intCutReviseMode = 0) Then                     ' ｶｯﾄ位置補正なし ?
                    ' ｶｯﾄ位置補正なしの場合はパターン登録情報構造体を初期化する        
                    pstCutPos(rn).intFLG = 0                                                ' カット位置補正フラグ = 0(しない)
                    pstCutPos(rn).intGRP = 0                                                ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟ番号
                    pstCutPos(rn).intPTN = 0                                                ' テンプレート番号
                    pstCutPos(rn).dblPosX = 0.0                                             ' パターン位置X
                    pstCutPos(rn).dblPosY = 0.0                                             ' パターン位置Y
                    pstCutPos(rn).intDisp = 0                                               ' パターン認識時の検索枠表示(0:なし, 1:あり)
                Else
                    ' ｶｯﾄ位置補正ありの場合は、抵抗データからパターン登録情報構造体を設定する
                    nm = nm + 1                                                             ' カット位置補正する抵抗数
                    pstCutPos(rn).intFLG = 1                                                ' カット位置補正フラグ = 1(補正あり)
                    pstCutPos(rn).intGRP = typResistorInfoArray(rn).intCutReviseGrpNo       ' ﾊﾟﾀｰﾝｸﾞﾙｰﾌﾟ番号
                    pstCutPos(rn).intPTN = typResistorInfoArray(rn).intCutRevisePtnNo       ' テンプレート番号
                    pstCutPos(rn).dblPosX = typResistorInfoArray(rn).dblCutRevisePosX       ' パターン位置X
                    pstCutPos(rn).dblPosY = typResistorInfoArray(rn).dblCutRevisePosY       ' パターン位置Y
                    pstCutPos(rn).intDisp = typResistorInfoArray(rn).intCutReviseDispMode   ' パターン認識時の検索枠表示(0:なし, 1:あり)
                    IsCutPosCorrectExecute = True                                           'V3.0.0.0③ ADD
                End If
            Next rn

            CutPosCorrectInit = nm                                                          ' カット位置補正する抵抗数を返す

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BasTrimming.CutPosCorrectInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CutPosCorrectInit = 0
        End Try

    End Function
#End Region

#Region "１ブロック分の抵抗のカット位置補正値を求める"
    '''=========================================================================
    '''<summary>１ブロック分の抵抗のカット位置補正値を求める</summary>
    '''<param name="registorCnt">     (INP)抵抗数</param> 
    '''<param name="pstCutPos">       (INP)パターン登録情報</param> 
    '''<param name="cutPosCorDatSend">(OUT)True=1つでも補正が正しく実行された, False=パターン認識エラー</param> 
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function CutPosCorrection(ByVal registorCnt As Integer, ByRef pstCutPos() As CutPosCorrect_Info, ByRef cutPosCorDatSend As Boolean) As Integer

        Dim rn As Integer
        Dim x As Double
        Dim y As Double
        Dim ret As Integer
        Dim r As Integer
        Dim strMSG As String
        Dim swSts As Integer
        Dim Ix As Integer                                                   '###059

        Try
            r = cFRS_NORMAL                                                 ' Return値 = 正常
            cutPosCorDatSend = False
            giTempGrpNo = -1                                                ' 現在のテンプレートグループ番号初期化(1～999)
            With gStCutPosCorrData
                For rn = 1 To registorCnt                                   ' 全抵抗数分設定する
                    Ix = rn - 1                                             ' カット位置補正構造体は0オリジン ###059  
                    If (pstCutPos(rn).intFLG <> 1) Then                     ' カット位置補正なし ?
                        .corrResult(Ix) = 0                                 ' パターン認識結果 = 0(補正なし)
                        .corrPosX(Ix) = 0.0                                 ' ズレ量X
                        .corrPosY(Ix) = 0.0                                 ' ズレ量Y
                        'giCutPosRSLT(rn) = 0                               ' パターン認識結果 = 0(補正なし)
                        'gfCutPosDRX(rn) = 0.0                              ' ズレ量X
                        'gfCutPosDRY(rn) = 0.0                              ' ズレ量Y
                    Else
                        ' パターン認識を行う
                        ret = CutPosPatternMatching(rn, x, y)
                        If (ret = 0) Then
                            ' カットオフ補正データ送信フラグ
                            If (cutPosCorDatSend <> True) Then
                                cutPosCorDatSend = True
                            End If
                            .corrResult(Ix) = 1                             ' パターン認識結果 = 1(OK)
                            .corrPosX(Ix) = x                               ' ズレ量X
                            .corrPosY(Ix) = y                               ' ズレ量Y
                            'giCutPosRSLT(rn) = 1                           ' パターン認識結果 = 1(OK)
                            'gfCutPosDRX(rn) = x                            ' ズレ量X
                            'gfCutPosDRY(rn) = y                            ' ズレ量Y
                        Else
                            .corrResult(Ix) = 2                             ' パターン認識結果 = 2(NG)
                            .corrPosX(Ix) = 0.0                             ' ズレ量X
                            .corrPosY(Ix) = 0.0                             ' ズレ量Y
                            '    giCutPosRSLT(rn) = 2                       ' パターン認識結果 = 2(NG)
                            '    gfCutPosDRX(rn) = 0.0                      ' ズレ量X
                            '    gfCutPosDRY(rn) = 0.0                      ' ズレ量Y
                        End If

                        '' ADJキー押下チェック
                        'If Form1.System1.AdjReqSw() Then
                        ' HALTキー押下チェック
                        Call HALT_SWCHECK(swSts)
                        If swSts = cSTS_HALTSW_ON Then
                            ' HALTﾗﾝﾌﾟON
                            Call LAMP_CTRL(LAMP_HALT, True)                 ' HALTランプON

                            ' HALTキー押下時はSTART/RESETキー押下待ち
                            Call STARTRESET_SWWAIT(swSts)
                            If (swSts = cSTS_STARTSW_ON) Then
                                'そのまま抜ける
                            ElseIf (swSts = cSTS_RESETSW_ON) Then
                                'ResetSw 押下時
                                Call LAMP_CTRL(LAMP_HALT, False)            ' HALTランプOFF
                                Call LAMP_CTRL(LAMP_RESET, True)            ' RESETランプON
                                ' "トリミング中RESET SW押下により停止"
                                Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMRES, "")
                            End If
                            Call LAMP_CTRL(LAMP_HALT, False)                ' HALTランプOFF
                        End If
                    End If
                Next rn
            End With

            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BasTrimming.CutPosCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                  ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "パターンマッチングにより１抵抗分の位置補正を求める"
    '''=========================================================================
    '''<summary>パターンマッチングにより１抵抗分の位置補正を求める</summary>
    '''<param name="iResistorNum">(INP)補正する抵抗のテーブル番号</param>
    '''<param name="fCorrectX">   (OUT)ずれ量X</param> 
    '''<param name="fCorrectY">   (OUT)ずれ量Y</param> 
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function CutPosPatternMatching(ByVal iResistorNum As Integer, ByRef fCorrectX As Double, ByRef fCorrectY As Double) As Integer

        Dim ret As Short = cFRS_NORMAL
        Dim crx As Double = 0.0                                         ' ずれ量X
        Dim cry As Double = 0.0                                         ' ずれ量Y
        Dim fcoeff As Double = 0.0                                      ' 相関値
        Dim Thresh As Double = 0.0                                      ' 閾値
        Dim r As Integer = cFRS_NORMAL                                  ' 関数値
        Dim strMSG As String

        Try
#If VIDEO_CAPTURE = 1 Then
        fCorrectX = 0.0
        fCorrectY = 0.0
        Return (cFRS_NORMAL)   
#Else

            ' パターンマッチング時のテンプレートグループ番号を設定する(毎回やると遅くなる)
            'V5.0.0.6③            If (giTempGrpNo <> stCutPos(iResistorNum).intGRP) Then  ' テンプレートグループ番号が変わった ?
            giTempGrpNo = stCutPos(iResistorNum).intGRP         ' 現在のテンプレートグループ番号を退避
            Form1.VideoLibrary1.SelectTemplateGroup(giTempGrpNo)             ' テンプレートグループ番号設定
            'V5.0.0.6③            End If

            ' 閾値取得
            Thresh = gDllSysprmSysParam_definst.GetPtnMatchThresh(giTempGrpNo, stCutPos(iResistorNum).intPTN)

            ' パーターン位置XYへBP移動(絶対値)
            r = Form1.System1.EX_MOVE(gSysPrm, stCutPos(iResistorNum).dblPosX, stCutPos(iResistorNum).dblPosY, 1)
            If (r <> cFRS_NORMAL) Then                              ' エラー ?
                Return (r)
            End If
            Form1.System1.WAIT(0.1)                                        ' Wait(Sec)
            Form1.VideoLibrary1.Refresh()                           'V3.0.0.0③ ADD 画像表示させる為

            ' パターンマッチング時の検索範囲枠表示/非表示を設定する 
            If (stCutPos(iResistorNum).intDisp = 0) Then            ' パターン認識時の検索枠表示(0:なし, 1:あり)
                Call Form1.VideoLibrary1.PatternDisp(False)                      ' 検索範囲枠非表示 
            Else
                Call Form1.VideoLibrary1.PatternDisp(True)                       ' 検索範囲枠表示 
            End If

            ' パターンマッチングを行う(Video.ocxを使用)
            'ret = Form1.VideoLibrary1.PatternMatching(iResistorNum, crx, cry, fcoeff)
            ret = Form1.VideoLibrary1.PatternMatching_EX(stCutPos(iResistorNum).intPTN, 0, True, crx, cry, fcoeff) ' ###059
            If (ret <> cFRS_NORMAL) Then
                r = cFRS_ERR_PTN                                    ' RETURN値 = パターンマッチィングエラー
            ElseIf (fcoeff < Thresh) Then
                gfCutPosCoef(iResistorNum) = fcoeff                 ' 一致度
                r = cFRS_ERR_PTN                                    ' RETURN値 = パターンマッチィングエラー
            Else
                ' マッチしたパターンの測定位置からずれ量を求める
                'fCorrectX = crx / 1000.0#                          ' ###059
                'fCorrectY = -cry / 1000.0#
                fCorrectX = crx
                fCorrectY = cry
                gfCutPosCoef(iResistorNum) = fcoeff                 ' 一致度
                r = cFRS_NORMAL                                     ' Return値 = 正常
            End If

Exit1:
            ' 後処理
            Debug.Print("X=" & fCorrectX.ToString("0.0000") & " Y=" & fCorrectY.ToString("0.0000"))
            Call Form1.VideoLibrary1.PatternDisp(False)                          ' 検索範囲枠非表示 
            Return (r)
#End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BasTrimming.CutPosPatternMatching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "カット位置補正を実施する"
    '''=========================================================================
    ''' <summary>カット位置補正を実施する</summary>
    ''' <param name="mode">       (INP)ﾄﾘﾐﾝｸﾞﾓｰﾄﾞ(ﾃﾞｼﾞSW下1桁)</param>
    ''' <param name="registorCnt">(INP)抵抗数</param> 
    ''' <param name="pstCutPos">  (INP)パターン登録情報</param>
    ''' <returns>ディレイ ON：True, OFF：False</returns>
    '''=========================================================================
    Private Function DoCutPosCorrect(ByVal mode As Integer, ByVal registorCnt As Integer, ByRef pstCutPos() As CutPosCorrect_Info) As Integer

        Dim rn As Integer
        Dim r As Integer
        Dim bCpNg As Boolean
        Dim strMSG As String
        Dim bSendCutPosCorData As Boolean
        Dim Ix As Integer                                               '###059

        Try
            ' ｶｯﾄ位置補正を実施する ?  
            '###247            If (Form1.stFNC(F_CUTPOS).iDEF = 1) And (mode <> 3) And (registorCnt) Then
            If (Form1.stFNC(F_CUTPOS).iDEF = 1) And (mode <> 2) And (mode <> 3) And (mode <> 6) And (registorCnt) Then
                '------------------------------------------------------------------------
                '   パターン認識を行いカット位置の補正量を求める
                '------------------------------------------------------------------------
                r = CutPosCorrection(registorCnt, pstCutPos, bSendCutPosCorData)
                If (r <> cFRS_NORMAL) Then                                              ' エラー ? 
                    Return (r)                                                          ' Return値設定
                End If

                ' カット補正結果による、NGスキップと、ログ表示
                bCpNg = False                                                           ' NG SKIPﾌﾗｸﾞOFF
                For rn = 1 To registorCnt                                               ' 全抵抗数分設定する 
                    Ix = rn - 1                                                         ' カット位置補正構造体は0オリジン ###059  
                    If (pstCutPos(rn).intFLG = 1) Then                                  ' カット位置補正する ?
                        If gStCutPosCorrData.corrResult(Ix) = 1 Then                    ' パターン認識結果 = 1(OK) ?
                            ' 結果表示ありの場合のみ表示する '###039
                            If (typResistorInfoArray(rn).intCutReviseDispMode = 1) Then
                                ' パターン認識結果 = 1(OK)時
                                strMSG = "Cut Pos Correct: R" + typResistorInfoArray(rn).intResNo.ToString("000") + " " +
                                         gStCutPosCorrData.corrPosX(Ix).ToString("0.0000") + "," + gStCutPosCorrData.corrPosY(Ix).ToString("0.0000") +
                                         "(" + gfCutPosCoef(rn).ToString("##0.0000") + ")"  '###019 
                                Call Form1.Z_PRINT(strMSG)
                            End If

                            '----- ###248↓ -----
                            '' NG SKIPﾌﾗｸﾞONなら以降もNG SKIPとする
                            'If bCpNg Then
                            '    gStCutPosCorrData.corrResult(Ix) = 2                       ' パターン認識結果 = 2(NG SKIP)とする
                            'End If
                            '----- ###248↑ -----
                        Else
                            ' パターン認識結果NG時
                            If (gfCutPosCoef(rn) = 0.0) Then
                                strMSG = "Cut Pos Correct: R" + typResistorInfoArray(rn).intResNo.ToString("000") + " Pattern Matching NG"
                            Else
                                strMSG = "Cut Pos Correct: R" + typResistorInfoArray(rn).intResNo.ToString("000") + " Pattern Matching NG" +
                                         "(" + gfCutPosCoef(rn).ToString("##0.0000") + ")" '###019 
                            End If
                            Call Form1.Z_PRINT(strMSG)

                            '----- ###248↓ -----
                            '' 画像認識NG判定有り ?
                            'If (typResistorInfoArray(rn).intIsNG = 0) Then
                            '    bCpNg = True                                                ' NG SKIPﾌﾗｸﾞON
                            'End If

                            ' 画像認識NG判定有り ?
                            If (typResistorInfoArray(rn).intIsNG = 0) Then                  ' 画像認識NG判定(0:あり, 1:なし, 手動(現在は未実装))
                                gStCutPosCorrData.corrResult(Ix) = 2                        ' パターン認識結果 = 2(NG SKIP)とする
                            Else
                                gStCutPosCorrData.corrResult(Ix) = 0                        ' パターン認識結果 = 0(補正なし)とする
                            End If
                            '----- ###248↑ -----
                        End If
                    End If
                Next
            End If

            '----------------------------------------------------------------------------
            '   ｶｯﾄ位置補正ﾃﾞｰﾀをINtime側に送信する(最大256抵抗分一括で送信)
            '----------------------------------------------------------------------------
            ' TKYの場合は無条件に送信する(送信しないとINtime側に残っている前回の補正データで補正される) ###092
            '###246 If (gTkyKnd = KND_TKY) Then
            Call CUTPOSCOR_ALL(registorCnt, gStCutPosCorrData)
            '###246 End If
            'If (bSendCutPosCorData = True) Then                         ' 1つでも補正が正しく実行された場合
            '    Call CUTPOSCOR_ALL(registorCnt, gStCutPosCorrData)
            'End If

            ' '' '' '' ｶｯﾄ位置補正ﾃﾞｰﾀをINtime側に送信する
            '' '' ''Call CUTPOSCOR(gRegistorCnt, gfCutPosDRX, gfCutPosDRY, giCutPosRSLT)

            '' '' ''ReDim giCutPosRSLT(MaxRegNum)                       ' CUTPOSCOR()をCallするとなぜか
            '' '' ''ReDim gfCutPosDRX(MaxRegNum)                        ' 配列の要素数が抵抗数に変更される
            '' '' ''ReDim gfCutPosDRY(MaxRegNum)                        ' ので再定義する(内容は初期化される)
            Return (cFRS_NORMAL)                                        ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BasTrimming.DoCutPosCorrect() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "カット位置補正を実施する"
    '''=========================================================================
    ''' <summary>カット位置補正の情報を最低限クリアする</summary>
    ''' <param name="mode">       (INP)ﾄﾘﾐﾝｸﾞﾓｰﾄﾞ(ﾃﾞｼﾞSW下1桁)</param>
    ''' <param name="registorCnt">(INP)抵抗数</param> 
    ''' <param name="pstCutPos">  (INP)パターン登録情報</param>
    ''' <returns>ディレイ ON：True, OFF：False</returns>
    '''=========================================================================
    Private Function DoCutPosCorrectClr(ByVal registorCnt As Integer) As Integer

        Dim rn As Integer
        Dim strMSG As String

        Try

            For rn = 1 To registorCnt                                               ' 全抵抗数分設定する 
                gStCutPosCorrData.corrResult(rn - 1) = 0                            ' パターン認識結果 = 0(補正なし)とする
                gStCutPosCorrData.corrPosX(rn - 1) = 0.0                            ' ズレ量X
                gStCutPosCorrData.corrPosY(rn - 1) = 0.0                            ' ズレ量Y
            Next

            '----------------------------------------------------------------------------
            '   ｶｯﾄ位置補正ﾃﾞｰﾀをINtime側に送信する(最大256抵抗分一括で送信)
            '----------------------------------------------------------------------------
            ' TKYの場合は無条件に送信する(送信しないとINtime側に残っている前回の補正データで補正される) ###092
            Call CUTPOSCOR_ALL(registorCnt, gStCutPosCorrData)

            Return (cFRS_NORMAL)                                        ' Return値 = 正常

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "BasTrimming.DoCutPosCorrect() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try

    End Function
#End Region


#Region "次ブロックへXYテーブルを移動する【TKY時】(未使用)"
    '''=========================================================================
    '''<summary>次ブロックへXYテーブルを移動する</summary>
    '''<param name="stgx">         (INP)</param>
    '''<param name="stgy">          (INP)</param>
    '''<param name="blockx">(OUT)ログデータ</param> 
    '''<param name="blocky">(OUT)ログデータ</param> 
    '''<param name="XY">(OUT)ログデータ</param> 
    '''<param name="pxy">(OUT)ログデータ</param>  
    '''<param name="dblADDSZX">(OUT)ログデータ</param> 
    '''<param name="dblADDSZY">(OUT)ログデータ</param>       
    '''=========================================================================
    Private Function MoveNextBlockTkyMode(ByVal stgx As Double, ByVal stgy As Double,
                                          ByVal blockx As Double, ByVal blocky As Double,
                                          ByVal XY() As Short, ByVal pxy() As Short,
                                          ByVal dblADDSZX As Double, ByVal dblADDSZY As Double) As Integer

        Dim x As Double
        Dim y As Double
        Dim pspacex As Double
        Dim pspacey As Double
        Dim r As Integer

        MoveNextBlockTkyMode = cFRS_NORMAL              ' Return値 = 正常

        pspacex = typPlateInfo.dblPlateItvXDir          ' プレートスペース（X) mm
        pspacey = typPlateInfo.dblPlateItvYDir          ' プレートスペース（Y) mm

        ' 次のブロックへXY移動
        Select Case gSysPrm.stDEV.giBpDirXy
            Case 0 ' x←, y↓
                x = stgx + blockx * XY(1) + pspacex * pxy(1) + dblADDSZX

                y = stgy + blocky * XY(2) + pspacey * pxy(2) + dblADDSZY

            Case 1 ' x→, y↓
                x = stgx - (blockx * XY(1) + pspacex * pxy(1)) - dblADDSZX
                y = stgy + blocky * XY(2) + pspacey * pxy(2) + dblADDSZY

            Case 2 ' x←, y↑
                x = stgx + blockx * XY(1) + pspacex * pxy(1) + dblADDSZX
                y = stgy - (blocky * XY(2) + pspacey * pxy(2)) - dblADDSZY

            Case 3 ' x→, y↑
                x = stgx - (blockx * XY(1) + pspacex * pxy(1)) - dblADDSZX
                y = stgy - (blocky * XY(2) + pspacey * pxy(2)) - dblADDSZY
        End Select

        ' Z移動 停止確認
        Call ZSTOPSTS()

        ' XYテーブル移動(絶対値指定)
        r = Form1.System1.XYtableMove(gSysPrm, x, y)
        MoveNextBlockTkyMode = r                        ' Retuen値設定  

    End Function

#End Region

#Region "次ﾌﾞﾛｯｸへの移動位置取得用の構造体へﾃﾞｰﾀを設定する【CHIP/NET時】(未使用)"
    '''=========================================================================
    '''<summary>次ﾌﾞﾛｯｸへの移動位置取得用の構造体へﾃﾞｰﾀを設定する</summary>
    '''<param name="tSetNextXY"> (OUT)次ﾌﾞﾛｯｸへの移動位置取得用の構造体</param>
    '''<param name="blockx">(OUT)ﾌﾞﾛｯｸｻｲｽﾞX</param> 
    '''<param name="blocky">(OUT)ﾌﾞﾛｯｸｻｲｽﾞY</param>
    '''<param name="stgx">(OUT)ﾌﾞﾛｯｸｻｲｽﾞX</param>   
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetBlockDataToStruct(ByRef tSetNextXY As TRIM_GETNEXTXY, ByRef blockx As Double, ByRef blocky As Double,
                                    ByRef stgx As Double, ByRef stgy As Double, ByRef pxy() As Short, ByRef XY() As Short,
                                    ByRef dblADDSZX As Double, ByRef dblADDSZY As Double, ByRef ptxy() As Short,
                                    ByRef gfStrp As Double, ByRef s As Short)

        Dim iCDir As Short
        Dim iStepRepeat As Short
        Dim bxy(2) As Short
        Dim dblCSx As Double
        Dim dblCSy As Double
        Dim gspacex As Double
        Dim gspacey As Double
        Dim dblStepOffX As Double
        Dim dblStepOffY As Double
        Dim blockIntervalx As Double
        Dim blockIntervaly As Double
        Dim gbxy(2) As Short
        Dim ptspacex As Double
        Dim ptspacey As Double

        '-----------------------------------------------------------------------
        '   データの取得
        '-----------------------------------------------------------------------
        ' チップ並び方向取得(CHIP-NETのみ)
        iCDir = typPlateInfo.intResistDir
        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの取得(0:なし,1:X,2:Y)
        iStepRepeat = typPlateInfo.intDirStepRepeat
        ' ﾌﾞﾛｯｸｻｲｽﾞの取得
        Call CalcBlockSize(blockx, blocky)
        ' ﾁｯﾌﾟｻｲｽﾞX,Y
        dblCSx = typPlateInfo.dblChipSizeXDir
        dblCSy = typPlateInfo.dblChipSizeYDir
        ' ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙX,Y
        gspacex = typPlateInfo.dblBpGrpItv      'BPグループ間隔
        gspacey = typPlateInfo.dblStgGrpItvY     'Stageグループ間隔
        'gspacex = typPlateInfo.dblGroupItvXDir
        'gspacey = typPlateInfo.dblGroupItvYDir

        ' ｽﾃｯﾌﾟｵﾌｾｯﾄX,Y
        dblStepOffX = typPlateInfo.dblStepOffsetXDir
        dblStepOffY = typPlateInfo.dblStepOffsetYDir
        ' ﾌﾞﾛｯｸ間隔X,Y
        blockIntervalx = typPlateInfo.dblBlockItvXDir
        blockIntervaly = typPlateInfo.dblBlockItvYDir
        ' グループ数X,Y
        gbxy(1) = typPlateInfo.intGroupCntInBlockXBp
        gbxy(2) = typPlateInfo.intGroupCntInBlockYStage
        ' ﾌﾞﾛｯｸ数の取得
        bxy(1) = typPlateInfo.intBlockCntXDir
        bxy(2) = typPlateInfo.intBlockCntYDir
        ' ﾌﾟﾚｰﾄ間隔X,Y(NETのみ)
        ptspacex = typPlateInfo.dblPlateItvXDir
        ptspacey = typPlateInfo.dblPlateItvYDir

        '-----------------------------------------------------------------------
        '   データを「次ﾌﾞﾛｯｸへの移動位置取得用の構造体」に設定する
        '-----------------------------------------------------------------------
        With tSetNextXY                                 ' 次ﾌﾞﾛｯｸへの移動位置取得用の構造体
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
            .dblstgx = stgx
            .dblstgy = stgy
            .intpxy1 = pxy(1)
            .intpxy2 = pxy(2)
            .intxy1 = XY(1)
            .intxy2 = XY(2)
            .dblblockIntervalx = blockIntervalx
            .dblblockIntervaly = blockIntervaly
            .intBlockCntXDir = bxy(1)
            .intBlockCntYDir = bxy(2)

            '----- NETのみ -----
            .dblStrp = gfStrp
            .dblADDSZX = dblADDSZX
            .dblADDSZY = dblADDSZY
            .intptxy1 = ptxy(1)
            .intptxy2 = ptxy(1)
            .dblptspacex = ptspacex
            .dblptspacey = ptspacey

            '----- CHIPのみ -----
            If gbxy(s) <> 1 Then
                If s = 1 Then
                    .intArrayCntX = XY(1) + (bxy(s) * pxy(1))
                    .intArrayCntY = XY(2)
                Else
                    .intArrayCntX = XY(1)
                    .intArrayCntY = XY(2) + (bxy(s) * pxy(2))
                End If
            Else
                .intArrayCntX = XY(1)
                .intArrayCntY = XY(2)
            End If
            .dblStrp = gfStrp
        End With

    End Sub

#End Region

#Region "次ブロックへXYテーブルを移動する【CHIP/NET時】(未使用?)"
    '''=========================================================================
    '''<summary>次ﾌﾞﾛｯｸへの移動処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Function MoveNextBlockChipNetMode(ByRef blockx As Double, ByRef blocky As Double,
                                             ByRef stgx As Double, ByRef stgy As Double, ByRef pxy() As Short, ByRef XY() As Short,
                                             ByRef dblADDSZX As Double, ByRef dblADDSZY As Double,
                                             ByRef ptxy() As Short, ByRef gfStrp As Double, ByRef s As Short) As Short

        Dim tSetNextXY As TRIM_GETNEXTXY                    ' 次ﾌﾞﾛｯｸへの移動位置取得用の構造体
        Dim x As Double
        Dim y As Double

        MoveNextBlockChipNetMode = cFRS_NORMAL              ' Return値 = 正常 

        ' 次ﾌﾞﾛｯｸへの移動位置取得用構造体を設定する
        Call SetBlockDataToStruct(tSetNextXY, blockx, blocky, stgx, stgy, pxy, XY, dblADDSZX, dblADDSZY, ptxy, gfStrp, s)
        ' 
        Call GetTrimmingNextBlockXYdata(tSetNextXY, x, y)

        ' Z移動 停止確認
        Call ZSTOPSTS()

        ' XY移動(絶対位置指定)
        If Form1.System1.XYtableMove(gSysPrm, x, y) Then
            MoveNextBlockChipNetMode = -1
        End If

    End Function
#End Region

#Region "次のﾌﾞﾛｯｸ位置X,Yの値算出(ﾄﾘﾐﾝｸﾞ処理)【CHIP時】(未使用?)"
    '''=========================================================================
    '''<summary>次のﾌﾞﾛｯｸ位置X,Yの値算出(ﾄﾘﾐﾝｸﾞ処理)</summary>
    '''<remarks>ﾘﾐﾝｸﾞ処理中のXYの算出処理を行なう</remarks>
    '''=========================================================================
    Private Sub GetTrimmingNextBlockXYdata(ByRef tSetNextXY As TRIM_GETNEXTXY, ByRef x As Double, ByRef y As Double)

        On Error GoTo ErrExit

        Dim dblInterval As Double
        Dim intForCnt As Short

        With tSetNextXY
            ' 次のブロックへXY移動
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x←, y↓
                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then ' X方向
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then ' 1抵抗ずらし
                            x = .dblstgx + ((.dblCSx / .intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy + dblInterval
                    Else ' Y方向
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx + dblInterval

                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then ' 1抵抗ずらし
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If
                    End If
                Case 1 ' x→, y↓
                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then ' X方向
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then ' 1抵抗ずらし
                            x = .dblstgx - ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy + dblInterval
                    Else 'Y方向
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx - dblInterval

                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then ' 1抵抗ずらし
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) + (.dblStepOffX * .intxy1)
                        End If
                    End If
                Case 2 ' x←, y↑
                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then ' X方向
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then ' 1抵抗ずらし
                            x = .dblstgx + ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) + (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy - dblInterval
                    Else 'Y方向
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx + dblInterval

                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then ' 1抵抗ずらし
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If
                    End If
                Case 3 ' x→, y↑
                    ' ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then                  ' X方向
                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then             ' 1抵抗ずらし
                            x = .dblstgx - ((.dblCSx / typPlateInfo.intBlockCntXDir) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1) - (.dblStepOffX * .intxy2)
                        End If

                        For intForCnt = 1 To .intArrayCntY
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        y = .dblstgy - dblInterval
                    Else 'Y方向
                        For intForCnt = 1 To .intArrayCntX
                            dblInterval = dblInterval + typTy2InfoArray(intForCnt).dblTy22
                        Next
                        x = .dblstgx - dblInterval

                        ' ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then             ' 1抵抗ずらし
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2) - (.dblStepOffX * .intxy1)
                        End If
                    End If
            End Select
        End With

        Exit Sub
ErrExit:

        MsgBox("GetTrimmingNextBlockXYdata" & ":" & Err.Description, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, gAppName)
        Err.Clear()
    End Sub
#End Region

#Region "次のﾌﾞﾛｯｸ位置X,Yの値算出(ﾄﾘﾐﾝｸﾞ処理)【NET時】(未使用?)"
    '''=========================================================================
    '''<summary>次のﾌﾞﾛｯｸ位置X,Yの値算出(ﾄﾘﾐﾝｸﾞ処理)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetTrimmingNextBlockXYdataNET(ByVal tSetNextXY As TRIM_GETNEXTXY, ByVal x As Double, ByVal y As Double)

        On Error GoTo ErrExit

        With tSetNextXY

            ' 次のブロックへXY移動
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x←, y↓
                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then         ' X方向
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        End If
                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
                    Else  'Y方向
                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblStrp + .dblADDSZX
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        End If
                    End If
                Case 1 ' x→, y↓
                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then         ' X方向
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        End If
                        y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + .dblStrp + .dblADDSZY
                    Else  'Y方向
                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = .dblstgy + (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        Else
                            y = .dblstgy + ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) + (.dblStepOffX * .intxy1) + .dblADDSZY
                        End If
                    End If
                Case 2 ' x←, y↑
                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then         ' X方向
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            x = .dblstgx + (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        Else
                            x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + (.dblStepOffX * .intxy2) + .dblADDSZX
                        End If
                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
                    Else  'Y方向
                        x = .dblstgx + ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) + .dblADDSZX
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        End If
                    End If
                Case 3 ' x→, y↑
                    ''ﾁｯﾌﾟ方向のﾁｪｯｸを行う。
                    If (0 = .intCDir) Then         ' X方向
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (3 = .intStepR) Then         ' 1抵抗ずらし
                            x = .dblstgx - (.dblCSx * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        Else
                            x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - (.dblStepOffX * .intxy2) - .dblADDSZX
                        End If
                        y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - .dblStrp - .dblADDSZY
                    Else  'Y方向
                        x = .dblstgx - ((.dblblockx + .dblblockIntervalx) * .intxy1 + .dblgspacex * .intpxy1 + .dblptspacex * .intptxy1) - .dblStrp - .dblADDSZX
                        ''ｽﾃｯﾌﾟ&ﾘﾋﾟｰﾄの確認を行なう。
                        If (4 = .intStepR) Then         ' 1抵抗ずらし
                            y = .dblstgy - (.dblCSy * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        Else
                            y = .dblstgy - ((.dblblocky + .dblblockIntervaly) * .intxy2 + .dblgspacey * .intpxy2 + .dblptspacey * .intptxy2) - (.dblStepOffX * .intxy1) - .dblADDSZY
                        End If
                    End If
            End Select
        End With

        Exit Sub

ErrExit:
        Err.Clear()
    End Sub
#End Region

#Region "小数点変換"
    '''=========================================================================
    '''<summary>小数点変換</summary>
    '''<param name="s"> (INP)変換対象文字列</param>
    '''<param name="ch">(INP)小数点置き換え文字</param> 
    '''<returns>変換済み文字列</returns>
    '''=========================================================================
    Public Function CnvFloatPointChar(ByRef s As String, ByRef ch As String) As String
        Dim i As Short
        Dim ln As Short

        ln = Len(s)
        For i = 1 To ln
            If Mid(s, i, 1) = "." Then
                Mid(s, i, 1) = ch
            End If
        Next
        CnvFloatPointChar = s
    End Function
#End Region

#Region "DispGazou.exe処理"
#If False Then                          'V6.0.0.0⑤
    '=========================================================================
    '   画像表示プログラムの起動処理
    '=========================================================================
    'V3.0.0.0⑤↓
    Private ipcChnl As New System.Runtime.Remoting.Channels.Ipc.IpcClientChannel
    Private IpcObj As Object = Activator.GetObject(GetType(IPCServiceClass), "ipc://TRIM_DISP_GAZOU_IPC_PORT_NM/" + GetType(IPCServiceClass).Name)
    Private refObj As IPCServiceClass = CType(IpcObj, IPCServiceClass)

#Region "画像表示プログラムを起動する"
    '''=========================================================================
    ''' <summary>画像表示プログラムを起動する</summary>
    ''' <param name="ObjProc"> (OUT)Processｵﾌﾞｼﾞｪｸﾄ</param>
    ''' <param name="strFName">(INP)起動プログラム名</param>
    ''' <param name="Camera">  (INP)カメラ番号(0-3)</param> 
    ''' <returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function Execute_GazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer) As Integer

        Dim strARG As String                                        ' 引数() 

        Dim dispXPos As Integer
        Dim dispYPos As Integer
        Dim Cnt As Integer = 0

        Try
            If gKeiTyp = KEY_TYPE_RS Then
                TrimClassCommon.ForceEndProcess(DISPGAZOU_SMALL_PATH)       ' プロセスを強制終了する。
            Else
                TrimClassCommon.ForceEndProcess(DISPGAZOU_PATH)       ' プロセスを強制終了する。
            End If
            ' 表示位置設定
            dispXPos = FORM_X + Form1.VideoLibrary1.Location.X
            dispYPos = FORM_Y + Form1.VideoLibrary1.Location.Y

            ' ｺﾏﾝﾄﾞﾗｲﾝ引数設定
            strARG = Camera.ToString("0") + " "                     ' args[0] :カメラ番号(0-3)
            'strARG = "0 "                                           ' args[0] :カメラ番号(0-3)"
            strARG = strARG + "1 "                                  ' args[1] :(0=ボタン表示する, 1=ボタン表示しない)
            strARG = strARG + dispXPos.ToString("0") + " "          ' args[2] :フォームの表示位置X
            strARG = strARG + dispYPos.ToString("0")                ' args[3] :フォームの表示位置Y
            strARG = strARG + " 1"                                  ' args[4] :(0=メッセージ制御無し, 1=メッセージ制御有り)
            'V5.0.0.8④ strARG = strARG + " 1"                                  ' args[5] :(0=シンプルトリマ用サイズ小画面, 1=通常画面サイズ) 'V5.0.0.6⑯

            ' プロセスの起動
            ObjProc = New Process                                   ' Processｵﾌﾞｼﾞｪｸﾄを生成する 
            ObjProc.StartInfo.FileName = strFName                   ' プロセス名 
            ObjProc.StartInfo.Arguments = strARG                    ' ｺﾏﾝﾄﾞﾗｲﾝ引数設定
            ObjProc.StartInfo.WorkingDirectory = strWrk             ' 作業フォルダ
            ObjProc.Start()                                         ' プロセス起動

            ' チャネルを登録
            'ChannelServices.RegisterChannel(ipcChnl, False)
IPC_RETRY_START:  ' サーバ（DispGazou)側を停止して直ぐに起動した後だとポートに書き込めないエラーになる。対処方法が判らないので再試行する。
            Try
                'refObj.CallServer("STOP")
                Sleep(2000)
                SendMsgToDispGazou(ObjProc, 2)       'STOP 'V4.0.0.0-87
            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou強制終了
                        If gKeiTyp = KEY_TYPE_RS Then
                            Execute_GazouProc(ObjProc, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    '再起動
                        Else
                            Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '再起動
                        End If
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.Execute_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try


            ' トラップエラー発生時 
        Catch ex As Exception
            MsgBox("basTrimming.Execute_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "画像表示プログラムを強制終了する"
    '''=========================================================================
    '''<summary>画像表示プログラムを強制終了する</summary>
    '''<param name="ObjProc"> (OUT)Processｵﾌﾞｼﾞｪｸﾄ</param>
    '''<returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function FinalEnd_GazouProc(ByRef ObjProc As Process) As Integer
        Dim ExecName As String

        Try
            ' 画像表示プログラム未起動ならNOP
            'If Not ObjProc Is Nothing Then
            '    ' プロセス終了メッセージを送信する(ｳｨﾝﾄﾞｳのあるｱﾌﾟﾘの場合) 
            '    If (ObjProc.CloseMainWindow() = False) Then             ' ﾒｲﾝｳｨﾝﾄﾞｳにｸﾛｰｽﾞﾒｯｾｰｼﾞを送信する
            '        ObjProc.Kill()                                      ' 終了しなかった場合は強制終了する
            '    End If

            '    ' プロセスの終了を待つ
            '    Do
            '        Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
            '    Loop While (ObjProc.HasExited <> True)                  ' プロセスが終了している場合のみTrueが返る
            'End If

            ''refObj.CallServer("END")

            ''ChannelServices.UnregisterChannel(ipcChnl)

            '' 後処理 
            'ObjProc.Dispose()                                       ' リソース開放 
            'ObjProc = Nothing

            '            TrimClassCommon.ForceEndProcess(DISPGAZOU_PATH)       ' ダメ押しでプロセスを強制終了する。
            TrimClassCommon.ForceEndProcess(DISPGAZOU_SMALL_PATH)       ' ダメ押しでプロセスを強制終了する。

            ' 画像表示プロセスを強制終了する(上記でプロセスが終了しない場合があるため) 
            'V4.1.0.0③
            If gKeiTyp = KEY_TYPE_RS Then
                ExecName = "DispGazouSmall"
            Else
                ExecName = "DispGazou"
            End If
            '            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("DispGazou")
            Dim ps As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName(ExecName)
            'V4.1.0.0③
            For Each p As System.Diagnostics.Process In ps
                p.Kill()
            Next


            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            '            MsgBox("basTrimming.FinalEnd_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    'V3.0.0.0⑤↑
#Region "画像表示プログラムを起動する"
    '''=========================================================================
    ''' <summary>画像表示プログラムを起動する</summary>
    ''' <param name="ObjProc"> (OUT)Processｵﾌﾞｼﾞｪｸﾄ</param>
    ''' <param name="strFName">(INP)起動プログラム名</param>
    ''' <param name="Camera">  (INP)カメラ番号(0-3)</param> 
    ''' <param name="nCommand">(INP)プロセスに送信するコマンド(0:START,1:START_SMALL)</param>
    ''' <returns>0 = 正常, 0以外 = エラー</returns>
    ''' <remarks>V3.1.0.0② 2014/12/01 画像表示プログラムのモジュールを統合</remarks>
    '''=========================================================================
    Public Function Exec_GazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer, ByVal nCommand As Integer) As Integer

        Dim Cnt As Integer = 0
        ' Dim result As Integer

        Try
            ' VideoOcx表示を停止
            Call Form1.VideoLibrary1.VideoStop()

IPC_RETRY_START:  ' サーバ（DispGazou)側を停止して直ぐに起動した後だとポートに書き込めないエラーになる。対処方法が判らないので再試行する。
            Try
                ' ↓↓↓ V3.1.0.0② 2014/12/01
                If nCommand = 0 Then
                    'V4.0.0.0-87refObj.CallServer("START")'V4.0.0.0-87
                    'V4.4.0.0③　↓
                    'SendMsgToDispGazou(1)                               ' START 'V4.0.0.0-87
                    '----- V4.3.0.0④↓ -----
                    If (gKeiTyp <> KEY_TYPE_RS) Then                    ' SL43xR ? 
                        SendMsgToDispGazou(ObjProc, 5)                           ' START_NORMAL
                    Else
                        SendMsgToDispGazou(ObjProc, 1)                               ' START 'V4.0.0.0-87
                    End If
                    'V4.4.0.0③　↑
                    '----- V4.3.0.0④↑ -----
                Else
                    'V4.0.0.0-87refObj.CallServer("START_SMALL")'V4.0.0.0-87
                    SendMsgToDispGazou(ObjProc, 4)       'START_SMALL'V4.0.0.0-87
                End If
                ' ↑↑↑ V3.1.0.0② 2014/12/01
            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou強制終了
                        System.Threading.Thread.Sleep(100)
                        ' ↓↓↓ V3.1.0.0② 2014/12/01
                        'Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '再起動
                        Execute_GazouProc(ObjProc, strFName, strWrk, Camera)    '再起動
                        ' ↑↑↑ V3.1.0.0② 2014/12/01
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try

            ' トラップエラー発生時 
        Catch ex As Exception
            MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
    'Public Function Exec_GazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer) As Integer

    '    Dim strARG As String                                        ' 引数() 
    '    Dim strMSG As String
    '    Dim dispXPos As Integer
    '    Dim dispYPos As Integer
    '    Try
    '        ' 表示位置設定
    '        dispXPos = FORM_X + Form1.VideoLibrary1.Location.X
    '        dispYPos = FORM_Y + Form1.VideoLibrary1.Location.Y

    '        ' ｺﾏﾝﾄﾞﾗｲﾝ引数設定
    '        strARG = Camera.ToString("0") + " "                     ' args[0] :カメラ番号(0-3)"
    '        'strARG = "0 "                                           ' args[0] :カメラ番号(0-3)"
    '        strARG = strARG + "1 "                                  ' args[1] :(0=ボタン表示する, 1=ボタン表示しない)
    '        strARG = strARG + dispXPos.ToString("0") + " "          ' args[2] :フォームの表示位置X
    '        strARG = strARG + dispYPos.ToString("0")                ' args[3] :フォームの表示位置Y

    '        ' VideoOcx表示を停止
    '        Call Form1.VideoLibrary1.VideoStop()

    '        ' プロセスの起動
    '        ObjProc = New Process                                   ' Processｵﾌﾞｼﾞｪｸﾄを生成する 
    '        ObjProc.StartInfo.FileName = strFName                   ' プロセス名 
    '        ObjProc.StartInfo.Arguments = strARG                    ' ｺﾏﾝﾄﾞﾗｲﾝ引数設定
    '        ObjProc.StartInfo.WorkingDirectory = strWrk             ' 作業フォルダ
    '        ObjProc.Start()                                         ' プロセス起動

    '        ' トラップエラー発生時 
    '    Catch ex As Exception
    '        strMSG = "basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message
    '        MsgBox(strMSG)
    '        Return (cERR_TRAP)
    '    End Try
    'End Function
#End Region

#End Region

#Region "画像表示プログラムを強制終了する"
    '''=========================================================================
    '''<summary>画像表示プログラムを強制終了する</summary>
    '''<param name="ObjProc"> (OUT)Processｵﾌﾞｼﾞｪｸﾄ</param>
    '''<returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function End_GazouProc(ByRef ObjProc As Process) As Integer

        Dim Cnt As Integer = 0
        '        Dim result As Integer 

        Try
            ' 画像表示プログラム未起動ならNOP
            'If (ObjProc Is Nothing) Then
            '    Return (cFRS_NORMAL)
            'End If

IPC_RETRY_START:  ' サーバ（DispGazou)側を停止して直ぐに起動した後だとポートに書き込めないエラーになる。対処方法が判らないので再試行する。
            Try

                'V4.0.0.0-87refObj.CallServer("STOP")
                SendMsgToDispGazou(ObjProc, 2)       'STOP 
                '                result = SendMessage(hWnd, WM_USER, 0, 2)
                'V4.0.0.0-87

            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou強制終了
                        Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '再起動
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try

            Call Form1.VideoLibrary1.VideoStart()

            ' 画面を更新
            Call Form1.Refresh()

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try
    End Function
    'Public Function End_GazouProc(ByRef ObjProc As Process) As Integer

    '    Dim strMSG As String

    '    Try
    '        ' 画像表示プログラム未起動ならNOP
    '        If (ObjProc Is Nothing) Then
    '            Return (cFRS_NORMAL)
    '        End If

    '        ' プロセス終了メッセージを送信する(ｳｨﾝﾄﾞｳのあるｱﾌﾟﾘの場合) 
    '        If (ObjProc.CloseMainWindow() = False) Then             ' ﾒｲﾝｳｨﾝﾄﾞｳにｸﾛｰｽﾞﾒｯｾｰｼﾞを送信する
    '            ObjProc.Kill()                                      ' 終了しなかった場合は強制終了する
    '        End If

    '        ' プロセスの終了を待つ
    '        Do
    '            Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
    '        Loop While (ObjProc.HasExited <> True)                  ' プロセスが終了している場合のみTrueが返る

    '        ' VideoOcx表示の再開
    '        Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
    '        Call Form1.VideoLibrary1.VideoStart()

    '        ' 画面を更新
    '        Call Form1.Refresh()

    '        ' 後処理 
    '        ObjProc.Dispose()                                       ' リソース開放 
    '        ObjProc = Nothing
    '        Return (cFRS_NORMAL)

    '        ' トラップエラー発生時 
    '    Catch ex As Exception
    '        strMSG = "basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message
    '        MsgBox(strMSG)
    '        Return (cERR_TRAP)
    '    End Try
    'End Function
#End If                                 'V6.0.0.0⑤
#End Region

#Region "FT平均値(Double)の算出"
    ' ###154
    '''=========================================================================
    '''<summary>FT平均値(Double)の算出</summary> 
    '''<param name="dblNx"> (IN)今回の値ｵﾌﾞｼﾞｪｸﾄ</param>
    '''<returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function GetAverageFT(ByVal dblNx As Double, ByVal lngNxMax As Long) As Double

        Dim strMSG As String

        Try

            TotalAverageFT = TotalAverageFT + dblNx
            '' 数分ﾙｰﾌﾟ
            'For lngCnt = 0 To (lngNxMax - 1)
            '    ' 誤差合計
            '    dblSub = dblSub + dblNx(lngCnt)
            'Next
            '' 平均値
            'GetAverage = dblSub / lngNxMax
            GetAverageFT = TotalAverageFT / lngNxMax
            TotalAverageDebug = GetAverageFT

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.GetAverageFT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "IT平均値(Double)の算出"
    '''=========================================================================
    '''<summary>IT平均値(Double)の算出</summary>###154 
    '''<param name="dblNx"> (IN)今回の測定結果</param>
    '''<returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function GetAverageIT(ByVal dblNx As Double, ByVal lngNxMax As Long) As Double
        Dim strMSG As String

        Try
            TotalAverageIT = TotalAverageIT + dblNx
            '' 数分ﾙｰﾌﾟ
            'For lngCnt = 0 To (lngNxMax - 1)
            '    ' 誤差合計
            '    dblSub = dblSub + dblNx(lngCnt)
            'Next
            '' 平均値
            'GetAverage = dblSub / lngNxMax
            GetAverageIT = TotalAverageIT / lngNxMax
            TotalAverageDebug = GetAverageIT

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.TotalAverageIT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "FT標準偏差(Double)の算出"
    '''=========================================================================
    ''' <summary>FT標準偏差(Double)の算出</summary>###154 
    ''' <param name="dblNx"> (INP)値(Doubleの配列)  (添字は0ORG)</param>
    ''' <param name="lngNxMax">(INP) : 数</param>
    ''' <param name="dblAverage">(INP) : 平均値</param>
    ''' <returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function GetDeviationFT(ByVal dblNx As Double, ByVal lngNxMax As Long, ByVal dblAverage As Double) As Double

        Dim strMSG As String

        Try
            TotalSum2FT = TotalSum2FT + dblNx * dblNx
            TotalDeviationFT = Math.Sqrt((TotalSum2FT / lngNxMax) - (dblAverage * dblAverage))
            GetDeviationFT = TotalDeviationFT
            '' CHG 
            'TotalDeviationFT = TotalDeviationFT + (dblNx - dblAverage) * (dblNx - dblAverage)
            ' '' 平方偏差
            'GetDeviationFT = Math.Sqrt(TotalDeviationFT / lngNxMax)
            'TotalDeviationDebug = TotalDeviationFT
            '' ORG
            '' 数分ﾙｰﾌﾟ
            'For lngCnt = 0 To (lngNxMax - 1)

            '    ' 平均誤差を引いた値を2乗
            '    dblXi = dblXi + ((dblNx(lngCnt) - dblAverage) * (dblNx(lngCnt) - dblAverage))

            'Next

            '' 平方偏差
            'GetDeviation = Sqr(dblXi / lngNxMax)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.GetDeviationFT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "FT標準偏差(Double)の算出ORG "
    '===============================================================================
    '【機　能】標準偏差(Double)の算出
    '【引　数】dblNx()    (INP) : 値(Doubleの配列)  (添字は0ORG)
    '          lngNxMax   (INP) : 数
    '          dblAverage (INP) : 平均値
    '【戻り値】標準偏差
    '===============================================================================
    '''=========================================================================
    '''<summary>FT標準偏差(Double)の算出</summary>###154 
    '''<param name="dblNx"> (IN)今回の測定結果ｵﾌﾞｼﾞｪｸﾄ</param>
    '''<returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function GetDeviationFTOrg(ByVal dblNx() As Double, ByVal lngNxMax As Long, ByVal dblAverage As Double) As Double

        Dim strMSG As String
        Dim lngCnt As Long
        Dim dblXi As Double

        Try
            'TotalDeviationFT = TotalDeviationFT + (dblNx - dblAverage) * (dblNx - dblAverage)
            ' '' 平方偏差
            'GetDeviationFTOrg = Math.Sqrt(TotalDeviationFT / lngNxMax)
            'TotalDeviationDebug = GetDeviationFTOrg

            ' 数分ﾙｰﾌﾟ
            For lngCnt = 0 To (lngNxMax - 1)

                ' 平均誤差を引いた値を2乗
                dblXi = dblXi + ((dblNx(lngCnt) - dblAverage) * (dblNx(lngCnt) - dblAverage))

            Next

            ' 平方偏差
            GetDeviationFTOrg = Math.Sqrt(dblXi / lngNxMax)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.GetDeviationFT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "IT標準偏差(Double)の算出"
    '===============================================================================
    '【機　能】標準偏差(Double)の算出
    '【引　数】dblNx()    (INP) : 値(Doubleの配列)  (添字は0ORG)
    '          lngNxMax   (INP) : 数
    '          dblAverage (INP) : 平均値
    '【戻り値】標準偏差
    '===============================================================================
    '''=========================================================================
    '''<summary>IT標準偏差(Double)の算出</summary>###154 
    '''<param name="dblNx"> (IN)今回の測定結果ｵﾌﾞｼﾞｪｸﾄ</param>
    '''<returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function GetDeviationIT(ByVal dblNx As Double, ByVal lngNxMax As Long, ByVal dblAverage As Double) As Double

        Dim strMSG As String

        Try
            TotalSum2IT = TotalSum2IT + dblNx * dblNx
            TotalDeviationIT = Math.Sqrt((TotalSum2IT / lngNxMax) - (dblAverage * dblAverage))
            GetDeviationIT = TotalDeviationIT
            'TotalDeviationIT = TotalDeviationFT + (dblNx - dblAverage) * (dblNx - dblAverage)
            ' '' 平方偏差
            'GetDeviationIT = Math.Sqrt(TotalDeviationFT / lngNxMax)
            '            TotalDeviationDebug = GetDeviationIT
            '' 数分ﾙｰﾌﾟ
            'For lngCnt = 0 To (lngNxMax - 1)

            '    ' 平均誤差を引いた値を2乗
            '    dblXi = dblXi + ((dblNx(lngCnt) - dblAverage) * (dblNx(lngCnt) - dblAverage))

            'Next

            '' 平方偏差
            'GetDeviation = Sqr(dblXi / lngNxMax)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.GetDeviationIT() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
        Exit Function

    End Function
#End Region

#Region "計算値のクリア"
    '''=========================================================================
    '''<summary>計算値のクリア</summary>###154 
    '''=========================================================================
    Public Sub ClearAvgDevCount()

        Try
            TotalDeviationIT = 0
            TotalAverageIT = 0
            TotalDeviationFT = 0
            TotalAverageFT = 0
            TotalSum2FT = 0
            TotalSum2IT = 0
            '----- V6.0.3.0_26↓ -----
            TotalFTValue = 0
            TotalAverageFTValue = 0
            TotalCntTrimming = 0
            '----- V6.0.3.0_26↑ -----

        Catch ex As Exception
        End Try
        Exit Sub

    End Sub
#End Region

#Region "連続HI-NGﾁｪｯｸﾊﾞｯﾌｧ初期化"
    '''=========================================================================
    '''<summary>連続HI-NGﾁｪｯｸﾊﾞｯﾌｧ初期化</summary> ###181 2013.01.25 
    '''=========================================================================
    Public Sub ClearNgHiCount()
        Dim r As Integer

        ' 連続HI-NGﾁｪｯｸﾊﾞｯﾌｧ初期化 ###181
        For r = 0 To gRegistorCnt
            iNgHiCount(r) = 0
        Next r

    End Sub

#End Region
    '----- V1.13.0.0⑪↓ -----
#Region "測定ばらつき検出/オーバロード検出チェック"
    '''=========================================================================
    ''' <summary>測定ばらつき検出/オーバロード検出チェック</summary>
    ''' <param name="RtnCode">(INP)TrimBlockExe()の戻り値</param>
    ''' <returns>cFRS_ERR_START = 測定ばらつき検出/オーバロード検出で処理続行指定
    '''          cFRS_ERR_RST   = 測定ばらつき検出/オーバロード検出で処理打切指定
    '''          上記以外       = エラー
    ''' </returns>
    '''=========================================================================
    Private Function CV_OverLoadErrorCheck(ByVal RtnCode As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 測定ばらつき検出/オーバロード検出
            If (RtnCode = ERR_MEAS_CV) Then                             ' 測定ばらつき検出 ? 
                strMSG = MSG_SPRASH40                                   ' "測定のばらつきを検出しました"  
            ElseIf (RtnCode = ERR_MEAS_OVERLOAD) Then                   ' オーバロード検出 ?
                strMSG = MSG_SPRASH41                                   ' "測定時オーバロードを検出しました"  
            ElseIf (RtnCode = ERR_MEAS_REPROBING) Then                  ' 再プロービングエラー ?
                strMSG = MSG_SPRASH42                                   ' "再プロービングエラー"
                Call Form1.Z_PRINT(strMSG)                              ' ※ログ画面にも表示する 
            Else                                                        ' 上記以外 
                Return (RtnCode)                                        ' Return値 = TrimBlockExe()の戻り値を返す
            End If

            ' ローダが自動モード時はログ表示域にメッセージ表示する
            If (giHostMode = cHOSTcMODEcAUTO) Then
                If (RtnCode <> ERR_MEAS_REPROBING) Then
                    Call Form1.Z_PRINT(strMSG)
                End If
                Return (cFRS_ERR_RST)                                   ' Return値 = 測定ばらつき検出/オーバロード検出で処理打切指定
            End If

            ' メッセージ表示(STARTキー/RESETキー押下待ち)
            Call LAMP_CTRL(LAMP_START, True)                            ' STARTランプON
            Call LAMP_CTRL(LAMP_RESET, True)                            ' RESETランプON
            Call ZCONRST()                                              ' コンソールキーラッチ解除

            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                    strMSG, MSG_SPRASH35, "", System.Drawing.Color.Blue, System.Drawing.Color.Black, System.Drawing.Color.Black)

            Call LAMP_CTRL(LAMP_START, False)                           ' STARTランプOFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETランプOFF
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "basTrimming.CV_OverLoadErrorCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (RtnCode)                                            ' Return値 = TrimBlockExe()の戻り値を返す
        End Try
    End Function
#End Region
    '----- V1.13.0.0⑪↑ -----
    '----- V1.13.0.0⑪↓ -----
#Region "Logファイル用ファイルのクリア"
    '''=========================================================================
    ''' <summary>Log書き込み用ファイルの削除</summary>
    ''' <param name="tmpLogFileName">(INP)削除したいファイル名</param>
    ''' <returns>OK  
    ''' </returns>
    '''=========================================================================
    Private Function ClearTmpLogFile(ByVal tmpLogFileName As String) As Integer

        System.IO.File.Delete(tmpLogFileName)

    End Function

#End Region
    '----- V1.13.0.0⑪↓ -----
#Region "Logファイル用ファイルの作成"
    '''=========================================================================
    ''' <summary>Log書き込み用ファイルの作成</summary>
    ''' <param name="tmpLogFileName">(INP)作成したいファイル名</param>
    ''' <returns>OK  
    ''' </returns>
    '''=========================================================================
    Private Function MakeTmpLogFile(ByVal tmpLogFileName As String, ByRef writestr As String) As Integer
        '#4.12.2.0④    Private Function MakeTmpLogFile(ByVal tmpLogFileName As String, ByVal writestr As String) As Integer
        Dim strMSG As String
        'Dim RtnCode As Integer

        Try
            'RtnCode = 0
            'Dim sw As New System.IO.StreamWriter(tmpLogFileName, True, System.Text.Encoding.GetEncoding("shift_jis"))
            Using sw As New StreamWriter(tmpLogFileName, True, Encoding.UTF8)   'V4.4.0.0-0
                'RtnCode = 1
                sw.Write(writestr)
                'sw.Close()
            End Using

        Catch ex As Exception
            strMSG = "basTrimming.MakeTmpLogFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function

#End Region

    '=========================================================================
    '   画像表示プログラムの起動処理
    '=========================================================================
#If False Then
#Region "画像表示プログラムを起動する"
    ' ↓↓↓ V3.1.0.0② 2014/12/01 コメント
    ' '''=========================================================================
    ' ''' <summary>画像表示プログラムを起動する</summary>
    ' ''' <param name="ObjProc"> (OUT)Processｵﾌﾞｼﾞｪｸﾄ</param>
    ' ''' <param name="strFName">(INP)起動プログラム名</param>
    ' ''' <param name="Camera">  (INP)カメラ番号(0-3)</param> 
    ' ''' <returns>0 = 正常, 0以外 = エラー</returns>
    ' '''=========================================================================
    '    Public Function Exec_SmallGazouProc(ByRef ObjProc As Process, ByRef strFName As String, ByRef strWrk As String, ByVal Camera As Integer) As Integer

    '        Dim Cnt As Integer = 0

    '        Try
    '            ' VideoOcx表示を停止
    '            Call Form1.VideoLibrary1.VideoStop()

    'IPC_RETRY_START:  ' サーバ（DispGazou)側を停止して直ぐに起動した後だとポートに書き込めないエラーになる。対処方法が判らないので再試行する。
    '            Try
    '                refObj.CallServer("START_SMALL")
    '            Catch ex As Exception
    '                Cnt = Cnt + 1
    '                If Cnt < 100 Then
    '                    If (Cnt Mod 10) = 0 Then
    '                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou強制終了
    '                        System.Threading.Thread.Sleep(100)
    '                        Execute_GazouProc(ObjProc, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    '再起動
    '                    End If
    '                    System.Threading.Thread.Sleep(10)
    '                    GoTo IPC_RETRY_START
    '                Else
    '                    MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
    '                End If
    '            End Try

    '            ' トラップエラー発生時 
    '        Catch ex As Exception
    '            MsgBox("basTrimming.Exec_GazouProc() TRAP ERROR = " + ex.Message)
    '            Return (cERR_TRAP)
    '        End Try
    '    End Function
    ' ↑↑↑ V3.1.0.0② 2014/12/01 コメント
#End Region

#Region "画像表示プログラムを強制終了する"
    '''=========================================================================
    '''<summary>画像表示プログラムを強制終了する</summary>
    '''<param name="ObjProc"> (OUT)Processｵﾌﾞｼﾞｪｸﾄ</param>
    '''<returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Public Function End_SmallGazouProc(ByRef ObjProc As Process) As Integer

        Dim Cnt As Integer = 0
        '    Dim result As Integer 

        Try
            ' 画像表示プログラム未起動ならNOP
            'If (ObjProc Is Nothing) Then
            '    Return (cFRS_NORMAL)
            'End If

IPC_RETRY_START:  ' サーバ（DispGazou)側を停止して直ぐに起動した後だとポートに書き込めないエラーになる。対処方法が判らないので再試行する。
            Try
                'V4.0.0.0-87refObj.CallServer("STOP")
                'V4.0.0.0-87 refObj.CallServer("STOP")
                SendMsgToDispGazou(ObjProc, 2)       'STOP 

            Catch ex As Exception
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou強制終了
                        Execute_GazouProc(ObjProc, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    '再起動
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo IPC_RETRY_START
                Else
                    MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
                End If
            End Try

            Call Form1.VideoLibrary1.VideoStart()

            ' 画面を更新
            Call Form1.Refresh()

            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            MsgBox("basTrimming.End_GazouProc() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)
        End Try

        'Dim strMSG As String

        'Try
        '    ' 画像表示プログラム未起動ならNOP
        '    If (ObjProc Is Nothing) Then
        '        Return (cFRS_NORMAL)
        '    End If

        '    ' プロセス終了メッセージを送信する(ｳｨﾝﾄﾞｳのあるｱﾌﾟﾘの場合) 
        '    If (ObjProc.CloseMainWindow() = False) Then             ' ﾒｲﾝｳｨﾝﾄﾞｳにｸﾛｰｽﾞﾒｯｾｰｼﾞを送信する
        '        ObjProc.Kill()                                      ' 終了しなかった場合は強制終了する
        '    End If

        '    ' プロセスの終了を待つ
        '    Do
        '        Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
        '    Loop While (ObjProc.HasExited <> True)                  ' プロセスが終了している場合のみTrueが返る

        '    ' VideoOcx表示の再開
        '    Form1.System1.WAIT(0.1)                             ' Wait(Sec) 
        '    Call Form1.VideoLibrary1.VideoStart()

        '    ' 画面を更新
        '    Call Form1.Refresh()

        '    ' 後処理 
        '    ObjProc.Dispose()                                       ' リソース開放 
        '    ObjProc = Nothing
        '    Return (cFRS_NORMAL)

        '    ' トラップエラー発生時 
        'Catch ex As Exception
        '    strMSG = "SimpleTrimmer.End_SmallGazouProc() TRAP ERROR = " + ex.Message
        '    MsgBox(strMSG)
        '    Return (cERR_TRAP)
        'End Try
    End Function
#End Region

#Region "DispgazouにWindowメッセージを送信する"
    '''=========================================================================
    ''' <summary>
    ''' DispgazouにWindowメッセージを送信する
    ''' </summary>
    ''' <param name="ObjProc">'V5.0.0.6⑯ ADD</param>
    ''' <param name="No">(INP)メッセージ番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function SendMsgToDispGazou(ByRef ObjProc As Process, ByVal No As Integer) As Integer

        Dim result As Integer = cFRS_NORMAL
        Dim Cnt As Integer = 0
        Dim hWnd As Int32
        Try
SND_MSG_RETRY_START:
            '相手のウィンドウハンドルを取得します
            'Dim hWnd As Int32 = FindWindow(Nothing, "DispGazou(Camera1) V4.0.0.0")'V4.3.0.0③
            hWnd = FindWindow(Nothing, "DispGazou") 'V4.3.0.0③
            If hWnd = 0 Then
                'ハンドルが取得できなかった
                'V5.0.0.6⑯ ADD START↓
                Cnt = Cnt + 1
                If Cnt < 100 Then
                    If (Cnt Mod 10) = 0 Then
                        Call FinalEnd_GazouProc(ObjProc)                                'DispGazou強制終了
                        Execute_GazouProc(ObjProc, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    '再起動
                    End If
                    System.Threading.Thread.Sleep(10)
                    GoTo SND_MSG_RETRY_START
                Else                'V5.0.0.6⑯ ADD END↑
                    MessageBox.Show("相手Windowのハンドルが取得できません")
                End If
            End If

            '//'V4.0.0.0-89        result = SendMessage(hWnd, WM_APP, 0, No)
            result = SendNotifyMessage(hWnd, WM_APP, 0, No)

            Return result
        Catch ex As Exception
            MsgBox("basTrimming.SendMsgToDispGazou() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try

    End Function
#End Region

    'V4.3.0.0①↓
#Region "DispgazouにWindowメッセージを送信する"
    Private bDasouDispCrossLine As Boolean = False
    Public Sub DasouDispCrossLineOn()
        bDasouDispCrossLine = True
    End Sub
    Public Sub DasouDispCrossLineOff()
        bDasouDispCrossLine = False
    End Sub
    Public Function GetDasouDispCrossLine() As Boolean
        Return (bDasouDispCrossLine)
    End Function
#End Region
#End If
    '''=========================================================================
    ''' <summary>
    ''' DispgazouにWindowメッセージを送信する 
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SendCrossLineMsgToDispGazou()
        'V6.0.0.0⑤        Dim result As Integer
        'V6.0.0.0⑤        Dim int1 As Int32 = 0
        'V6.0.0.0⑤        Dim int2 As Int32 = 0
        Try
            'V6.0.0.0⑤            Dim bpx, bpy, xpos, ypos, zpos As Double
            If gSysPrm.stCRL.giDspFlg = 1 Then      ' 収差補正有り
#If False Then
                '相手のウィンドウハンドルを取得します
                Dim hWnd As Int32 = FindWindow(Nothing, "DispGazou")
                LaserFront.Trimmer.DefTrimFnc.GET_STATUS(1, xpos, ypos, zpos, bpx, bpy)
                If hWnd = 0 Then
                    'ハンドルが取得できなかった
                    MessageBox.Show("SendCrossLineMsgToDispGazou() 相手Windowのハンドルが取得できません")
                End If
                bpx = bpx * 1000
                bpy = bpy * 1000
                ' Int型なのでこちらで1000倍して、受け側で1000で割って使用する 
                '数値に正しく変換出来るか？
                int1 = CType(bpx, Int32)
                int2 = CType(bpy, Int32)
                result = SendMessage(hWnd, (WM_APP + 2), int1, int2)
#Else
                ObjCrossLine.CrossLineDispXY(0.0#, 0.0#)                'V6.0.0.0⑤
#End If
            End If
        Catch ex As Exception
            MsgBox("SendCrossLineMsgToDispGazou() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    'V4.3.0.0①↑

    ''' <summary>'V4.1.0.0⑫
    ''' BPの寿命を延ばすために、BPを最大に動作させる
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BPMaintMove() As Integer
        Dim r As Integer
        Dim strMsg As String

        Try
            r = Form1.System1.BPMOVE(gSysPrm, 0, 0, 0, 0, -40, -40, 1)
            If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                Return (r)
            End If
            r = Form1.System1.BPMOVE(gSysPrm, 0, 0, 0, 0, 40, 40, 1)
            If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                Return (r)
            End If
            r = Form1.System1.BPMOVE(gSysPrm, 0, 0, 0, 0, 0, 0, 1)
            If (r <> cFRS_NORMAL) Then                              ' エラー ?(メッセージは表示済み) 
                Return (r)
            End If

            Return r

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "BPMaintMove() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try

    End Function


    ''' <summary>'V4.1.0.0⑮
    ''' Form1のADJボタンの状態設定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetADJButton() As Integer

        If (gbChkboxHalt = True) Then
            Form1.BtnADJ.Text = "ADJ ON"
            Form1.BtnADJ.BackColor = System.Drawing.Color.Yellow
        Else
            Form1.BtnADJ.Text = "ADJ OFF"
            Form1.BtnADJ.BackColor = System.Drawing.SystemColors.Control
        End If

    End Function

    'V4.9.0.0①↓ 'High,Lowの率が悪くなった場合の停止画面用
    ''' <summary>
    ''' NG率の判定を行い停止する判定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function sub_JudgeLotStop() As Integer
        Dim r As Integer
        Dim rtnCode As Integer
        Dim strMSG As String
        Dim LaserPower As Double 'V5.0.0.1-32

        sub_JudgeLotStop = cFRS_NORMAL

        r = JudgeLotStop()
        If r <> cFRS_NORMAL Then
            NGJudgeResult = r

            ' 'V4.11.0.0⑧↓
            r = StageMveExchangePos()
            If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
                rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' メッセージ表示
                sub_JudgeLotStop = r
                Exit Function
            End If

            SetLotStopRequestBit(1)
            r = WaitLotStopReady()
            If r <> cFRS_NORMAL Then
                sub_JudgeLotStop = r
                Exit Function
            End If
            'クランプOFF、吸着OFF
            ClampAndVacuum(0)
            ' 'V4.11.0.0⑧↑

            '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) ----
            ' 一時停止開始時間を設定する(オプション)
            m_blnElapsedTime = True                             ' 経過時間を表示する
            Call Set_PauseStartTime(StPauseTime)
            '----- V4.11.0.0④↑ -----

            ' NG率が設定よりも高くなった場合のエラーメッセージ表示(SL432R/SL436R共通)
            r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_RATE_NG) ' キー押下待ち画面表示
            If r = cFRS_ERR_RST Then

LOT_CANCEL:
                sub_JudgeLotStop = cFRS_ERR_RST

                ' ローダー出力(ON=トリマ部停止中,OFF=なし)(SL436R時)
                Call SetLoaderIO(LOUT_STOP, &H0)                        ' ローダ出力(ON=トリマ部停止中, OFF=なし)
                'V4.9.0.0①↓
                SetSubExistBit(1)
                ' W113.02をONする
                SetLotStopBit(1)
                'V4.9.0.0①↑

            ElseIf r = cFRS_ERR_OTHER Then
                'クリーニングの実行
            Else
                '集計をクリアするかの確認
                r = Sub_CallFrmRset(Form1.System1, cGMODE_ERR_TOTAL_CLEAR) ' キー押下待ち画面表示
                If r = cFRS_ERR_OTHER Then
                    '何もしないで続行
                Else
                    'V5.0.0.1-32↓
                    LaserPower = TrimData.GetLaserPower()
                    '集計内容のクリア
                    ClearTotalCount()
                    TrimData.SetLaserPower(LaserPower)
                    SimpleTrimmer.DspLaserPower()
                    'V5.0.0.1-32↑
                End If

PLATE_CHECK_RETRY:
                r = SetSubExistBit(1)
                If r = cFRS_NORMAL Then
                    ' 基板あり
                Else
                    ' 基板なし
                    strMSG = MSG_SPRASH58
                    'r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True, _
                    '        "", strMSG, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_BTN_START + cFRS_ERR_RST, True, _
                            "", strMSG, "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                    If r = cFRS_ERR_RST Then
                        GoTo LOT_CANCEL
                    ElseIf r = cFRS_ERR_START Then
                        GoTo PLATE_CHECK_RETRY
                    End If
                End If
            End If
            ' 'V4.11.0.0⑧↓
            SetLotStopRequestBit(0)
            ' 'V4.11.0.0⑧↑

            '----- V4.11.0.0④↓ (WALSIN殿SL436S対応) ----
            ' 一時停止終了時間を設定し一時停止時間を集計する(オプション)
            Call Set_PauseTotalTime(StPauseTime)
            ' DllTrimClassLibraryに一時停止時間を渡す(オプション)
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0④↑ -----


        End If
    End Function

    '----- V6.0.3.0⑦↓ -----
#Region "前回の測定結果と目標値の差分からカットオフ値を計算する"
    '''=========================================================================
    ''' <summary>前回の測定結果と目標値の差分からカットオフ値を計算する</summary>
    ''' <returns>
    ''' </returns>
    '''=========================================================================
    Public Function CalcCutOffVal() As Integer

        Dim i As Integer
        Dim HighLimitTarget As Double
        Dim LowLimitTarget As Double
        Dim turnning As Boolean = False
        Dim FinalCutOff As Double
        Dim SumFinalTest As Double = 0.0
        Dim FinalTestOKCnt As Long = 0
        Dim AvgFinal As Double = 0.0
        Dim sMsg As String


        ' 目標値Ａに対する調整Ｈｉｇｈ側オフセット
        HighLimitTarget = stCutOffAdjust.TargetA * (1 + typResistorInfoArray(i + 1).dblAdjustCutOff_HighLimit / 100)    ' カットオフ調整HI(%)      ' V6.0.3.0⑫
        ' 目標値Ａに対する調整Ｌｏｗ側オフセット
        LowLimitTarget = stCutOffAdjust.TargetA * (1 + typResistorInfoArray(i + 1).dblAdjustCutOff_LowLimit / 100)      ' カットオフ調整LO(%)      ' V6.0.3.0⑫

        For i = 0 To typPlateInfo.intResistCntInBlock - 1

            If gwTrimResult(i) = TRIM_RESULT_OK Then
                SumFinalTest = SumFinalTest + gfFinalTest(i)    ' 測定が正常な抵抗の平均値
                FinalTestOKCnt = FinalTestOKCnt + 1             ' 測定が正常な抵抗の個数
            End If

            sMsg = "Judge=," + gwTrimResult(i).ToString("0") + " , 測定値," + gfFinalTest(i).ToString("0.0000000")
            DbgLogDsp(sMsg)

        Next

        If FinalTestOKCnt > 0 Then                              ' 正常に測定できた抵抗があるときのみ
            AvgFinal = TrimData.CalcAverage(SumFinalTest, FinalTestOKCnt)

            If (AvgFinal < LowLimitTarget) OrElse (HighLimitTarget < AvgFinal) Then
                stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC

                '次回調整用カットオフ値
                FinalCutOff = TrimData.ConvValToPercent(stCutOffAdjust.TargetA, AvgFinal)
                stCutOffAdjust.dblAdjustCutOff = stCutOffAdjust.dblAdjustCutOff - FinalCutOff

                stCutOffAdjust.dblAdjustCutOff = Math.Round(stCutOffAdjust.dblAdjustCutOff, 2, MidpointRounding.AwayFromZero)

                sMsg = "Cnt=" + gAdjustCutoffCount.ToString("0") + " : 測定平均=" + AvgFinal.ToString("0.0000000") + "," + "調整後カットオフ＝" + stCutOffAdjust.dblAdjustCutOff.ToString("0.00")
                Call Form1.Z_PRINT(sMsg)
                DbgLogDsp(sMsg)

                For i = 0 To typPlateInfo.intResistCntInBlock - 1
                    typResistorInfoArray(i + 1).ArrCut(typResistorInfoArray(i + 1).intCutCount).dblCutOff = stCutOffAdjust.dblAdjustCutOff
                Next
                ' INtimeへデータ転送
                SendTrimData()
            Else
                ' 調整範囲内に入っている
                stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_ALREADY
                sMsg = "調整完了：Cnt=" + gAdjustCutoffCount.ToString("0") + " : 測定平均＝" + AvgFinal.ToString("0.0000000")
                ' V6.0.3.0⑳↓ 画面上にカットオフ値を表示
                Form1.lblCutOff.Visible = True
                Form1.lblCutOff.Location = New System.Drawing.Point(615, 25)
                Form1.lblCutOff.Text = "CutOFF=" + stCutOffAdjust.dblAdjustCutOff.ToString("0.00")
                ' V6.0.3.0⑳↑

                Call Form1.Z_PRINT(sMsg)
                DbgLogDsp(sMsg)
            End If
        Else
            sMsg = "正常測定データなし"
            Call Form1.Z_PRINT(sMsg)
        End If

    End Function
#End Region

#Region "カットオフ計算用の領域の初期化"
    '''=========================================================================
    ''' <summary>カットオフ計算用の領域の初期化</summary>
    ''' <remarks>
    ''' </remarks>
    '''=========================================================================
    Public Sub InitCalCutoff()

        Dim sMsg As String


        '調整前のカットオフ値
        stCutOffAdjust.OrgCutOff = typResistorInfoArray(1).ArrCut(typResistorInfoArray(1).intCutCount).dblCutOff
        ' 目標値Ａ
        ' stCutOffAdjust.TargetA = typResistorInfoArray(1).dblTrimTargetVal * (1 + (typResistorInfoArray(1).ArrCut(typResistorInfoArray(1).intCutCount).dblCutOff) / 100)
        stCutOffAdjust.TargetA = typResistorInfoArray(1).dblTrimTargetVal

        'カットオフ初期値設定
        stCutOffAdjust.dblAdjustCutOff = stCutOffAdjust.OrgCutOff

        '調整中に設定
        stCutOffAdjust.dblAdjustCutOff_Exec = AdjustStat.ADJUST_EXEC

        sMsg = "目標A=" + stCutOffAdjust.TargetA.ToString("0.0000000") + "," + "元カットオフ＝" + stCutOffAdjust.OrgCutOff.ToString("0.00")
        Call Form1.Z_PRINT(sMsg)
        DbgLogDsp(sMsg)

    End Sub
#End Region

#Region "デバッグログ出力"
    '''=========================================================================
    ''' <summary>デバッグログ出力</summary>
    ''' <remarks>
    ''' </remarks>
    '''=========================================================================
    Public Sub DbgLogDsp(ByVal DspString As String)

        ' デバッグログ出力しないならNOP
        If (0 = giCutOffLogOut) Then Exit Sub

        Dim fileDate As String = (DateTime.Today).ToString("yyyyMMdd") 'yyyyMMdd
        Dim fileName As String =
            "C:\TRIMDATA\LOG\CutOfflog" & fileDate & ".log" ' VideoDbglogyyyyMMdd.log
        Try
            Using fs As New FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)
                Using sw As New StreamWriter(fs)
                    ' "yyyy/MM/dd HH:mm:ss:DspString"
                    Dim strDateTime As String = (DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss")
                    sw.WriteLine(strDateTime & " : " & DspString)
                End Using
            End Using

        Catch ex As Exception
            ' DO NOTHING
        End Try
    End Sub
#End Region
    '----- V6.0.3.0⑦↑ -----

    ' 'V4.11.0.0⑧↓
    ''' <summary>
    ''' 基板交換位置へ移動して、クランプ開、吸着OFFする
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function StageMveExchangePos() As Integer
        Dim Idx As Integer
        Dim r As Integer
        Dim rtnCode As Integer

        StageMveExchangePos = cFRS_NORMAL

        '基板取り出し位置への移動
        Idx = typPlateInfo.intWorkSetByLoader - 1               ' Idx = 基板品種番号 - 1
        r = SMOVE2(gfBordTableInPosX(Idx), gfBordTableInPosY(Idx))
        If (r <> cFRS_NORMAL) Then                                  ' エラー ? 
            rtnCode = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0) ' メッセージ表示
        End If

        StageMveExchangePos = r

        Exit Function

    End Function
    'V4.9.0.0①↑

    '========================================================================================
    '   共通関数
    '========================================================================================
#Region "ステージ移動処理"
    '''=========================================================================
    ''' <summary>ステージ移動処理</summary>
    ''' <param name="pltNo"></param>
    ''' <param name="blkNo"></param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function MoveTargetStagePos(ByVal pltNo As Integer, ByVal blkNo As Integer) As Integer

        Dim intRet As Integer
        Dim nextStgX As Double
        Dim nextStgY As Double
        Dim dispPltX As Integer
        Dim dispPltY As Integer
        Dim dispBlkX As Integer
        Dim dispBlkY As Integer
        'Dim retBlkNoX As Integer
        'Dim retBlkNoY As Integer
        Dim dispCurStgGrpNoX As Integer
        Dim dispCurStgGrpNoY As Integer
        Dim dispCurBlkNoX As Integer
        Dim dispCurBlkNoY As Integer
        Dim dispCurPltNoX As Integer
        Dim dispCurPltNoY As Integer
        Dim StgX As Double = 0.0 ' V4.0.0.0-40
        Dim StgY As Double = 0.0 ' V4.0.0.0-40

        Try
            MoveTargetStagePos = frmFineAdjust.MOVE_NEXT
            intRet = GetTargetStagePos(pltNo, blkNo, nextStgX, nextStgY, dispPltX, dispPltY, dispBlkX, dispBlkY)
            If intRet = BLOCK_END Then
                ' 何もしないで終了
                MoveTargetStagePos = frmFineAdjust.MOVE_NOT
                Exit Function
            ElseIf intRet = PLATE_BLOCK_END Then
                ' 何もしないで終了
                MoveTargetStagePos = frmFineAdjust.MOVE_NOT
                Exit Function
            End If

            '---------------------------------------------------------------------
            '   表示用各ポジションの番号を設定（プレート/ステージグループ/ブロック）
            '---------------------------------------------------------------------
            Dim bRet As Boolean
            bRet = GetDisplayPosInfo(dispBlkX, dispBlkY, _
                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)
            '#4.12.2.0⑥                 ↓
            Globals_Renamed.gCurPlateNoX = dispPltX
            Globals_Renamed.gCurPlateNoY = dispPltY
            Globals_Renamed.gCurBlockNoX = dispCurBlkNoX
            Globals_Renamed.gCurBlockNoY = dispCurBlkNoY
            '#4.12.2.0⑥                 ↑

            '---------------------------------------------------------------------
            '   ログ表示文字列の設定
            '---------------------------------------------------------------------
            dispCurPltNoX = dispPltX : dispCurPltNoY = dispPltY         '###056
            Call DisplayStartLog(dispCurPltNoX, dispCurPltNoY, _
                            dispCurStgGrpNoX, dispCurStgGrpNoY, dispCurBlkNoX, dispCurBlkNoY)

            'V5.0.0.9⑯                  ↓
            If (2 = giStartBlkAss) Then
                Form1.Set_StartBlkNum(dispCurBlkNoX, dispCurBlkNoY)
            End If
            'V5.0.0.9⑯                  ↑

            ' ステージの動作
            '----- V1.13.0.0③↓ -----
            ' 伸縮補正用パラメータの設定
            GetShinsyukuData(dispBlkX, dispBlkY, nextStgX, nextStgY)
            '----- V2.0.0.0⑨↓ -----
            If (giMachineKd = MACHINE_KD_RS) Then
                '----- V4.0.0.0-40↓ -----
                ' SL36S時でステージYの原点位置が上の場合、ブロックサイズの1/2は加算しない
                '↓
                'V4.6.0.0④　If (giStageYOrg = STGY_ORG_UP) Then
                StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
                'V4.6.0.0④　Else
                'V4.6.0.0④　StgX = nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                'V4.6.0.0④　StgY = nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY ' + (typPlateInfo.dblBlockSizeYDir / 2)
                'V4.6.0.0④　End If
                'V4.6.0.0④　↑
                intRet = Form1.System1.EX_START(gSysPrm, StgX, StgY, 0)

                'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
                '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2), 0)
                '----- V4.0.0.0-40↑ -----
            Else
                intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
                                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
            End If
            'intRet = Form1.System1.EX_START(gSysPrm, nextStgX + typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, _
            '                        nextStgY + typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
            '----- V2.0.0.0⑨↑ ----
            '----- V1.13.0.0③↑ -----
            '#4.12.1.0⑩↓
            If (intRet < (-1 * ERR_STG_STATUS)) Then
                ' 強制終了
                Call Form1.AppEndDataSave()
                Call Form1.AplicationForcedEnding()
            End If
            '#4.12.1.0⑩↑

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "frmFineAdjust.btnTrimming_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Function
#End Region

#Region "GrpStartBlkフレーム内のPREV/NEXTだけ有効にする"
    ''' <summary>
    ''' GrpStartBlkフレーム内のPREV/NEXTだけ有効にする
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GrpStartBlkPartEnable()

        Form1.GrpStartBlk.Enabled = True
        Form1.CbStartBlkX.Enabled = False
        Form1.CbStartBlkY.Enabled = False

        Form1.btnNEXT.Enabled = True
        Form1.btnPREV.Enabled = True

    End Sub
#End Region
    ''V5.0.0.1④↓
#Region "Judgeボタンの有効無効切替え"
    ''' <summary>
    ''' Judgeボタンの有効無効切替え
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <remarks></remarks>
    Public Sub btnJudgeEnable(ByVal flg As Boolean)

        Form1.btnJudge.Enabled = flg

    End Sub
#End Region
    'V5.0.0.1④↑
    'V5.0.0.6①↓
#Region "外部コントローラインターロック処理"
    ''' <summary>
    ''' 外部コントローラインターロック処理
    ''' </summary>
    ''' <returns>正常：cFRS_NORMAL,キャンセル：cFRS_ERR_RST,非常停止:cFRS_ERR_EMG</returns>
    ''' <remarks></remarks>
    Public Function CheckControllerInterlock() As Integer
        Try
            Dim digL As Integer
            Dim digH As Integer
            Dim digSW As Integer
            Dim iBitData As Integer
            Dim RtnStatus As Short = cFRS_NORMAL
            Dim BuzzerFirst As Boolean = True
            Dim sMessage As String = "温度コントローラ"

            Call Form1.GetMoveMode(digL, digH, digSW)

            If Not gbControllerInterlock Then        ' コントローラインターロック有りの時
                Return (cFRS_NORMAL)
            End If

            If typPlateInfo.intControllerInterlock = 0 Then
                Return (cFRS_NORMAL)
            End If

            If digL <> TRIM_MODE_ITTRFT And digL <> TRIM_MODE_TRFT And digL <> TRIM_MODE_FT And digL <> TRIM_MODE_MEAS Then
                Return (cFRS_NORMAL)
            End If



            Do
                Call EXTIN(iBitData)
                If (iBitData And EXT_IN3) Then  ' コントローラアラーム
                    sMessage = "温度コントローラ・アラーム発生です。"
                    RtnStatus = cFRS_ERR_START
                ElseIf (iBitData And EXT_IN2) Then  ' コントローラ適正温度
                    sMessage = "温度コントローラ・設定温度範囲外です。"
                    RtnStatus = cFRS_ERR_START
                Else
                    If (RtnStatus = cFRS_ERR_START) Then
                        'V5.0.0.9⑭ ↓
                        '                        Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)            ' OFF
                        'Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)            ' OFF
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                        'V5.0.0.9⑭ ↑

                    End If
                    RtnStatus = cFRS_NORMAL
                End If

                If RtnStatus <> cFRS_NORMAL And BuzzerFirst Then

                    'V5.0.0.9⑭ ↓
                    ' Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF) ' 標準(赤点滅+ブザー１)
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)
                    'V5.0.0.9⑭ ↑

                    Call System.Threading.Thread.Sleep(1000)                                    ' ３秒
                    'V5.0.0.9⑭ ↓
                    ' Call Form1.System1.SetSignalTower(0, SIGOUT_BZ1_ON)                         ' ブザー１OFF
                    Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_BUZZER_OFF)
                    'V5.0.0.9⑭ ↑

                    BuzzerFirst = False
                    Call SetLoaderIO(COM_STS_TRM_ERR, 0)    ' ローダー出力(ON=トリマエラー)
                End If
                'Call Form1.System1.SetSignalTower(0, &HFFFF)            ' ｼｸﾞﾅﾙﾀﾜｰ制御(On=0, Off=全ﾋﾞｯﾄ)

                If RtnStatus <> cFRS_NORMAL Then
                    Call SetLoaderIO(COM_STS_TRM_ERR, 0)    ' ローダー出力(ON=トリマエラー)
                    RtnStatus = Form1.System1.Form_MsgDispStartReset("STARTボタンで再試行します。", sMessage, "RESETボタンで中断します。", System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black))
                    Call ZCONRST()
                    If (RtnStatus = cFRS_ERR_START) Then
                    ElseIf (RtnStatus = cFRS_ERR_RST) Then
                        'V5.0.0.9⑭ ↓
                        ' Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)            ' OFF
                        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)
                        'V5.0.0.9⑭ ↑

                        Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ローダー出力(OFF=トリマエラー)
                        Return (cFRS_ERR_RST)   ' リセット押下
                    Else
                        Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ローダー出力(OFF=トリマエラー)
                        Return (cFRS_ERR_EMG)   ' 非常停止
                    End If
                End If
            Loop While (RtnStatus = cFRS_ERR_START)

            If BuzzerFirst = False Then
                Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ローダー出力(OFF=トリマエラー)
            End If

            Return (RtnStatus)

        Catch ex As Exception
            MsgBox("CheckControllerInterlock() TRAP ERROR = " + ex.Message)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try

    End Function
#End Region
    'V5.0.0.6①↑
    'V5.0.0.6②↓
#Region "第２原点処理・アクセサー(accessor)"
    ' ローダー供給、排出時のテーブル位置
    Private LoaderBordTableInPosX As Double = 0.0
    Private LoaderBordTableInPosY As Double = 0.0
    Private LoaderBordTableOutPosX As Double = 0.0
    Private LoaderBordTableOutPosY As Double = 0.0
    ''' <summary>
    ''' 第２原点座標の設定
    ''' </summary>
    ''' <param name="InPosX"></param>
    ''' <param name="InPosY"></param>
    ''' <param name="OutPosX"></param>
    ''' <param name="OutPosY"></param>
    ''' <remarks></remarks>
    Private Sub SetLoaderBordTablePos(ByVal InPosX As Double, ByVal InPosY As Double, ByVal OutPosX As Double, ByVal OutPosY As Double)
        Try
            If gbLoaderSecondPosition Then
                LoaderBordTableInPosX = InPosX
                LoaderBordTableInPosY = InPosY
                LoaderBordTableOutPosX = OutPosX
                LoaderBordTableOutPosY = OutPosY
                'TKYは、Form1.System1.XYtableMoveを使用しているのでSBACKへ原点座標変更設定は行わない。
                'Dim Rtn As Integer = SETLOADPOS(LoaderBordTableOutPosX, LoaderBordTableOutPosY)
                'If Rtn <> cFRS_NORMAL Then
                '    Call Form1.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH63, &HFF, &HFF0000)
                '    '"基板収納位置変更が異常終了しました。(SETLOADPOS)"
                'End If
            End If
        Catch ex As Exception
            MsgBox("SetLoaderBordTablePos() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    Public Function GetLoaderBordTableInPosX() As Double
        Return (LoaderBordTableInPosX)
    End Function
    Public Function GetLoaderBordTableInPosY() As Double
        Return (LoaderBordTableInPosY)
    End Function
    Public Function GetLoaderBordTableOutPosX() As Double
        If gbLoaderSecondPosition Then
            Return (LoaderBordTableOutPosX)
        Else
            Return (gdblStg2ndOrgX)
        End If
    End Function
    Public Function GetLoaderBordTableOutPosY() As Double
        If gbLoaderSecondPosition Then
            Return (LoaderBordTableOutPosY)
        Else
            Return (gdblStg2ndOrgY)
        End If
    End Function
#End Region

#Region "トリミングデータの製品種別変更時の処理"
    ''' <summary>
    ''' トリミングデータの製品種別変更時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChangeByTrimmingData()
        Try
            If gbLoaderSecondPosition Then
                Dim InPosX As Double, InPosY As Double, OutPosX As Double, OutPosY As Double
                If 1 <= typPlateInfo.intWorkSetByLoader And typPlateInfo.intWorkSetByLoader <= 10 Then
                    InPosX = Double.Parse(GetPrivateProfileString_S("SYSTEM", "INSERTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "X", Form1.LOADER_PARAMPATH, "0.0"))
                    InPosY = Double.Parse(GetPrivateProfileString_S("SYSTEM", "INSERTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "Y", Form1.LOADER_PARAMPATH, "0.0"))
                    OutPosX = Double.Parse(GetPrivateProfileString_S("SYSTEM", "EJECTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "X", Form1.LOADER_PARAMPATH, "0.0"))
                    OutPosY = Double.Parse(GetPrivateProfileString_S("SYSTEM", "EJECTPOS" & typPlateInfo.intWorkSetByLoader.ToString("0") + "Y", Form1.LOADER_PARAMPATH, "0.0"))
                    SetLoaderBordTablePos(InPosX, InPosY, OutPosX, OutPosY)
                Else
                    SetLoaderBordTablePos(0.0, 0.0, 0.0, 0.0)
                End If
            End If
        Catch ex As Exception
            MsgBox("ChangeByTrimmingData() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region
    'V5.0.0.6②↑
    'V6.0.1.0⑮↓
#Region "基板１枚のNG個数の判定"
    '''=========================================================================
    ''' <summary>基板１枚のNG個数の判定</summary>
    ''' <returns>0:正常、1:NG個数超えた</returns>
    '''=========================================================================
    Public Function JudgePlateNGCount(ByVal SetNgCount As Integer, ByVal NgCount As Integer) As Integer

        Dim r As Integer = 0
        Dim strMsg As String
        Dim strMsg1 As String


        ' シグナルタワーを赤点滅+ブザーONする
        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)

        strMsg = MSG_SPRASH66 + SetNgCount.ToString("0")
        strMsg1 = MSG_SPRASH67 + NgCount.ToString("0")

        r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                strMsg, strMsg1, MSG_frmLimit_07, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)

        ' 標準
        Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)

        Return r
        '-----------------------------------------------------------

    End Function
#End Region
    'V6.0.1.0⑮↑
    '----- V6.0.3.0_26↓ -----
#Region "FT平均値(Double)の算出"
    '''=========================================================================
    '''<summary>FT平均値(Double)の設定</summary> 
    '''=========================================================================
    Public Sub SetAverageFTValue(ByVal dblMesVal As Double, ByVal lngNxMax As Long)

        Dim strMSG As String

        Try

            TotalFTValue = TotalFTValue + dblMesVal

            TotalAverageFTValue = TotalFTValue / lngNxMax

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.SetAverageFTValue() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Exit Sub

    End Sub
#End Region

#Region "FT平均値の取得"
    '''=========================================================================
    '''<summary>FT平均値の取得</summary> 
    '''=========================================================================
    Public Function GetAverageFTValue() As Double

        Return TotalAverageFTValue

    End Function
#End Region
    '----- V6.0.3.0_26↑ -----
    '----- V6.0.3.0⑬↓ -----
#Region "少数点以下の桁数を求める"
    '''=========================================================================
    '''<summary>少数点以下の桁数を求める</summary> 
    ''' <param name="strDigitNum">(INP)gsEDIT_DIGITNUM("0.00000"</param>
    ''' <returns>少数点以下の桁数(5桁,7桁)</returns>
    '''=========================================================================
    Public Function GetDecPointSize(ByVal strDigitNum As String) As Integer

        Dim DecPoint As Integer
        Dim strMSG As String

        Try
            strDigitNum = strDigitNum.Trim()
            DecPoint = strDigitNum.IndexOf(".")                         ' 少数点位置を検索 (strDigitNum("0.00000")'
            If (DecPoint < 0) Then DecPoint = 1 '                       ' 少数桁なし
            DecPoint = strDigitNum.Length - (DecPoint + 1)              ' 少数点以下の桁数

            Return (DecPoint)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "GetDecPointSize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (5)                                                  ' Return値 = 標準値を返す
        End Try
    End Function
#End Region
    '----- V6.0.3.0⑬↑ -----
    '----- V6.1.1.0③↓ -----
#Region "自動運転開始時間を取得する(SL436R時オプション)"
    '''=========================================================================
    ''' <summary>自動運転開始時間を取得する</summary>
    ''' <param name="cStartTime">(OUT)開始時間</param>
    ''' <remarks>IAM殿オプション</remarks>
    '''=========================================================================
    Public Sub DispTrimStartTime(ByRef cStartTime As DateTime)

        Dim strMSG As String

        Try
            ' 自動運転開始時間を取得する
            cStartTime = DateTime.Now
            cStartTime = TruncMillSecond(cStartTime)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.DispTrimStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "自動運転終了時間を表示する(SL436R時オプション)"
    '''=========================================================================
    ''' <summary>自動運転終了時間を表示する</summary>
    ''' <param name="cStartTime">(INP)開始時間</param>
    ''' <param name="cEndTime">  (OUT)終了時間</param>
    ''' <remarks>IAM殿オプション</remarks>
    '''=========================================================================
    Public Sub DispTrimEndTime(ByRef cStartTime As DateTime, ByRef cEndTime As DateTime)

        Dim ts As TimeSpan
        Dim strDat As String
        Dim strMSG As String

        Try
            ' 終了時間を求める
            cEndTime = DateTime.Now
            cEndTime = TruncMillSecond(cEndTime)

            ' 経過時間を求める
            ts = cEndTime - cStartTime
            strDat = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00")

            ' 自動運転開始時間を表示する
            strMSG = "AUTOMATIC DRIVING START = " + cStartTime.ToString("H:mm:ss")
            Call Form1.Z_PRINT(strMSG)

            ' 自動運転終了時間を表示する
            strMSG = "AUTOMATIC DRIVING END   = " + cEndTime.ToString("H:mm:ss") + " (" + strDat + ")"
            Call Form1.Z_PRINT(strMSG)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "basTrimming.DispTrimEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
#Region "ミリ秒を切り捨てて返す"
    '''=========================================================================
    ''' <summary>ミリ秒を切り捨てて返す</summary>
    ''' <param name="source">(INP)時間</param>
    ''' <returns>時間</returns>
    '''=========================================================================
    Public Function TruncMillSecond(source As DateTime) As DateTime
        Dim d = New DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second, 0)
        Return d
    End Function
#End Region
    '----- V6.1.1.0③↑ -----
    '----- V6.1.1.0⑬↓ -----
#Region "現在の時刻を返す"
    '''=========================================================================
    ''' <summary>現在の時刻を返す</summary>
    ''' <param name="sDate">(OUT)時間</param>
    '''=========================================================================
    Public Sub Get_NowYYMMDDHHMMSS(ByRef sDate As String)

        Dim NowTime As DateTime

        NowTime = DateTime.Now
        sDate = NowTime.ToString("yyyy/MM/dd H:mm:ss")

    End Sub
#End Region
    '----- V6.1.1.0⑬↑ -----
#Region "基板１枚のNG率の判定" 'V4.5.0.5① 'V4.12.2.0⑨　'V6.0.5.0④
    '''=========================================================================
    ''' <summary>基板１枚のNG率の判定</summary>
    ''' <returns>0:正常、1:NG率超えた</returns>
    '''=========================================================================
    Public Function JudgePlateNGRate(ByVal automode As Boolean) As Integer

        Dim iResistor As Integer
        Dim i As Integer
        Dim PlateresCnt As Integer
        Dim dJudgeRate As Double
        Dim r As Integer = 0
        Dim strMsg As String
        Dim strMsg1 As String

        '-----------------------------------------------------------
        '   NG判定閾値の算出
        '-----------------------------------------------------------

        ' １ブロックの抵抗数をカウント
        iResistor = 0

        ' 抵抗数分処理を行なう。
        For i = 1 To gRegistorCnt
            ' 抵抗番号<1000であるかチェック
            If typResistorInfoArray(i).intResNo < 1000 Then
                ' １ブロックの抵抗数のカウント
                iResistor = iResistor + 1
            End If
        Next

        ' １基板の抵抗数＝１ブロック内抵抗数×ブロック数X×ブロック数Y×プレート数X×プレート数Y
        PlateresCnt = iResistor * typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir * typPlateInfo.intPlateCntXDir * typPlateInfo.intPlateCntYDir

        ' 抵抗数がない場合には、そのまま正常で戻す
        If (PlateresCnt = 0) Then
            Return r
        End If

        ' 全体に対するNG率 = NG数 / １基板内抵抗数
        dJudgeRate = (m_NgCountInPlate / PlateresCnt) * 100.0#

        r = 0
        If (dJudgeRate >= gdblNgStopRate) And (gdblNgStopRate <> 0) Then

            ' シグナルタワーを赤点滅+ブザーONする
            '            Call Form1.System1.SetSignalTower(SIGOUT_RED_BLK Or SIGOUT_BZ1_ON, &HFFFF)
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)

            strMsg = MSG_SPRASH79 + gdblNgStopRate.ToString("0.0") + " (%)"
            strMsg1 = MSG_SPRASH80 + dJudgeRate.ToString("0.0") + " (%) : " + m_NgCountInPlate.ToString + MSG_SPRASH81 + " / " + PlateresCnt.ToString + MSG_SPRASH81

            r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                    strMsg, strMsg1, MSG_SPRASH82, System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
            If (r = cFRS_ERR_START) Then
                ' ロット続行
                r = cFRS_ERR_START
            Else
                ' ロット中断RESET処理
                r = cFRS_ERR_RST
            End If

            ' 標準(赤点滅+ブザー１)
            'V4.12.2.2⑦↓
            If automode = True Then
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION)
                Call Form1.System1.SetSignalTower(SIGOUT_GRN_ON, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
            Else
                Call Form1.System1.SetSignalTower(0, SIGOUT_RED_BLK Or SIGOUT_BZ1_ON)
            End If
            'V4.12.2.2⑦↑

            Return (r)
        End If
        Return r
        '-----------------------------------------------------------

    End Function
#End Region

    ''' <summary>
    ''' グラフ表示フォームの再設定 'V6.0.2.0④
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GraphDispSet()

        Try

            If (Form1.chkDistributeOnOff.Checked = True) Then              ' 統計画面表示時に起動する 

                'Form1.changefrmDistStatus(1)                            ' グラフ表示 
                'gObjFrmDistribute.Visible = True
                'gObjFrmDistribute.Show()
                gObjFrmDistribute.ShowGraph()
            End If
        Catch ex As Exception

        End Try

    End Sub

End Module
