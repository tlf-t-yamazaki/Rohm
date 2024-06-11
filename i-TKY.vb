'===============================================================================
'   Description  : トリミングプログラムメイン処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Collections.Generic      'V4.10.0.0③
Imports System.Globalization            'V4.4.0.0-0
Imports System.Linq                     'V5.0.0.9⑤
Imports System.Text                     'V4.4.0.0-0
Imports System.Threading                'V4.4.0.0-0
Imports DllPrintString                  'V6.0.1.0⑫
Imports LaserFront.Trimmer
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc
Imports LaserFront.Trimmer.DllTeach.Teaching
Imports LaserFront.Trimmer.DllVideo     'V5.0.0.9⑤
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①
Imports Microsoft.Win32
Imports TKY_ALL_SL432HW.My.Resources
Imports TrimClassLibrary                '#4.12.2.0①
Imports TrimControlLibrary              'V6.0.1.0⑫
Imports TrimDataEditor                  'V4.10.0.0①

Friend Class Form1
    Inherits System.Windows.Forms.Form

    Friend Shared Instance As Form1     'V5.0.0.9⑰

    Const WM_SYSCOMMAND As Integer = &H112
    Private Const SC_MOVE As Integer = &HF010

    Private Const WM_ACTIVATEAPP As Integer = &H1C                              'V6.0.1.0④
    Private ReadOnly _keysNone As KeyEventArgs = New KeyEventArgs(Keys.None)    'V6.0.1.0④
    '===============================================================================
    '   定数定義
    '===============================================================================
    '----- API定義 -----
    'Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long 'V1.14.0.0②

#Region "メインフォームのマウスでの位置移動を無効にする"
    <System.Security.Permissions.SecurityPermission( _
        System.Security.Permissions.SecurityAction.LinkDemand, _
        Flags:=System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode), _
        System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub WndProc(ByRef m As Message)
        'V6.0.1.0④        If m.Msg = WM_SYSCOMMAND AndAlso
        'V6.0.1.0④            (m.WParam.ToInt32() And &HFFF0) = SC_MOVE Then
        'V6.0.1.0④            m.Result = IntPtr.Zero
        'V6.0.1.0④            Return
        'V6.0.1.0④        End If

        Select Case (m.Msg)
            Case WM_SYSCOMMAND
                ' メインフォームのマウスでの位置移動を無効にする
                If ((m.WParam.ToInt32() And &HFFF0) = SC_MOVE) Then
                    m.Result = IntPtr.Zero
                    Return
                End If

            Case WM_ACTIVATEAPP         'V6.0.1.0④
                ' アプリケーションが非アクティブになる時に念のためJogKeyUpする
                If (IntPtr.Zero = m.WParam) Then
                    If (Me._jogKeyUp IsNot Nothing) Then
                        Me._jogKeyUp.Invoke(_keysNone)
                        Debug.Print("WM_ACTIVATEAPP - JogKeyUp")
                    End If
                End If
        End Select

        MyBase.WndProc(m)
    End Sub
#End Region

#Region "定数/変数定義"
    '===========================================================================
    '   定数/変数定義
    '===========================================================================
    '----- WIN32 API -----
    Public Structure SECURITY_ATTRIBUTES
        Dim nLength As Integer              '構造体のバイト数
        Dim lpSecurityDescriptor As Integer 'セキュリティデスクリプタ(Win95,98では無効)
        Dim bInheritHandle As Integer       '1のとき、属性を継承する
    End Structure

    '-------------------------------------------------------------------------------
    '   LogClient.dll内の関数定義
    '-------------------------------------------------------------------------------
    Public Declare Function CreateEvent Lib "kernel32" Alias "CreateEventA" (ByRef lpEventAttributes As SECURITY_ATTRIBUTES, ByVal ManualReset As Integer, ByVal bInitialState As Integer, ByVal lpName As String) As Integer
    Public Declare Function WaitForSingleObject Lib "KERNEL32.DLL" (ByVal hHandle As Integer, ByVal dwMilliseconds As Integer) As Integer
    Public Declare Function SetEvent Lib "kernel32" (ByVal hEvent As Integer) As Integer
    Public Declare Function ResetEvent Lib "kernel32" (ByVal hEvent As Integer) As Integer
    'Private Declare Function GetPrivateProfileInt Lib "kernel32" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer

    '----- 定数定義 -----
    Private Const CUR_CRS_LINEX As Short = 45               ' ｸﾛｽﾗｲﾝX表示位置の補正値
    Private Const CUR_CRS_LINEY As Short = 8                ' ｸﾛｽﾗｲﾝY表示位置の補正値
    Private Const cLOGcBUFFERcSIZE As Double = 8192 * 3.5   ' ﾛｸﾞ画面表示の最大ｻｲｽﾞ

    Private Const cThetaAnglMin As Double = -5.0            ' θ回転最小角度
    Private Const cThetaAnglMax As Double = 5.0             ' θ回転最大角度

    '----- データ編集プログラム終了コード定義 -----
    Private Const PRC_EXIT_CAN As Integer = 0               ' 正常終了(Cancel指定) 
    Private Const PRC_EXIT_NML As Integer = 1               ' 正常終了(データセーブ要求なし) 
    Private Const PRC_EXIT_NML2 As Integer = 2              ' 正常終了(データセーブ要求あり) 
    Private Const PRC_EXIT_MLTEXE As Integer = 900          ' 多重起動エラー 
    Private Const PRC_EXIT_ENDREQ As Integer = 901          ' 終了メッセージ受信
    Private Const PRC_EXIT_ERRPRM As Integer = 902          ' 不正引数
    Private Const PRC_EXIT_ERRFIO As Integer = 903          ' ファイルＩＯエラー
    Private Const PRC_EXIT_ERRCNDIO As Integer = 904        ' 加工条件送受信エラー
    Private Const PRC_EXIT_TRAP As Integer = 999            ' トラップエラー 

    '----- ファイルパス名 -----                                         
    Private Const EditWorkFilePath As String = "C:\TRIMDATA\DATA\tmpTrimData"   ' データ編集用中間ファイル名
    Private Const cTEMPLATPATH As String = "C:\TRIM\VIDEO"                      ' Video.OCX用ﾃﾝﾌﾟﾚｰﾄﾌｧｲﾙの保存場所
    Private Const WORK_DIR_PATH As String = "C:\TRIM"                           ' 作業用ﾌｫﾙﾀﾞｰ
    Private Const DATA_DIR_PATH As String = "C:\TRIMDATA\DATA"                  ' ﾃﾞｰﾀﾌｧｲﾙﾌｫﾙﾀﾞｰ 
    Private Const SYSPARAMPATH As String = "C:\TRIM\tky.ini"                    ' システムパラメータパス名
    Public Const TKYSYSPARAMPATH As String = "C:\TRIM\TKYSYS.INI"              ' システムパラメータパス名 V2.0.0.0_29 'V6.1.4.0_39 Public化
    Public Const LOADER_PARAMPATH As String = "C:\TRIM\LOADER.ini"             ' システムパラメータパス名(ローダメンテナンス用)  ###069 'V5.0.0.6② Private→Public変更
    Public Const ENTRY_PATH As String = "C:\TRIMDATA\ENTRYLOT\"                 'V6.1.4.0_50
    '----- 変数定義 -----
    'V6.0.0.0    Public ObjIOMon As Object = Nothing                    ' ＩＯモニタ表示用
    Public ObjIOMon As FrmIOMon = Nothing                   ' ＩＯモニタ表示用      'V6.0.0.0

    '----- ログ画面の拡大用 -----
    Private TxtLog_NmlLocat As System.Drawing.Point         ' 通常ログ画面の位置(X,Y)
    Private TxtLog_NmlSize As System.Drawing.Size           ' 通常ログ画面のｻｲｽﾞ(X,Y)
    Private TxtLog_ExpLocat As System.Drawing.Point         ' 拡大ログ画面の位置(X,Y)
    Private TxtLog_ExpSize As System.Drawing.Size           ' 拡大ログ画面のｻｲｽﾞ(X,Y)

    '----- フラグ -----
    Private pbVideoInit As Boolean                          ' ビデオInitフラグ
    Private pbVideoCapture As Boolean                       ' ビデオキャプチャー開始フラグ
    Private gflgResetStart As Boolean                       ' 初期設定フラグ(True=初期設定済み, False=初期設定済みでない)
    Private gflgCmpEndProcess As Boolean                    ' 終了処理完了フラグ（True=終了処理実行済み、False=終了処理実行済みでない）

    '----- 内部状態保持変数 -----
    '' '' ''Private m_keepHaltSwSts As Boolean                      ' timer処理時のHALTスイッチ設定保存変数
    Private m_bStopRunning As Boolean                       ' 連続稼動テスト実施フラグ
    Private m_bDispDistributeSts As Boolean                 ' 統計処理表示状態保存変数

    '-------------------------------------------------------------------------------
    '   機能選択定義テーブル
    '-------------------------------------------------------------------------------
    '----- SL432R系用 ----- 
    Private Const cDEF_FUNCNAME_TKY As String = "C:\TRIM\DefFunc_Tky.INI"
    Private Const cDEF_FUNCNAME_CHIP As String = "C:\TRIM\DefFunc_TkyChip.INI"
    Private Const cDEF_FUNCNAME_NET As String = "C:\TRIM\DefFunc_TkyNet.INI"
    '----- SL436R系用 ----- 
    Private Const cDEF_FUNCNAME_CHIP_436 As String = "C:\TRIM\DefFunc_TkyChip_436.INI"
    Private Const cDEF_FUNCNAME_NET_436 As String = "C:\TRIM\DefFunc_TkyNet_436.INI"

    '----- 機能選択定義テーブル形式定義 (定義ファイル(TKY_DEFFUNC.INI)より設定する) -----
    Public Structure FNC_DEF
        Dim iDEF As Short                                   ' 機能選択定義(-1:非表示, 0:選択不可, 1:選択可)
        Dim iPAS As Short                                   ' パスワード指定(0:パスワードなし, 1:パスワードあり)
        Dim sCMD As String                                  ' ｺﾏﾝﾄﾞ(キー名)
    End Structure
    Public stFNC(MAX_FNCNO) As FNC_DEF                      ' 機能選択定義テーブル
    'Public flgLoginPWD As Short                             ' ﾛｸﾞｲﾝﾊﾟｽﾜｰﾄﾞ入力の有無(0:無, 1:有) V6.0.3.0_42

    Public strBTN(MAX_FNCNO + 1) As String                  ' ボタン名の配列(0 ORG) V1.18.0.1①

    'V3.0.0.0⑤    Private ObjGazou As Process = Nothing                   ' Processオブジェクト

    '----- V6.1.4.0⑦↓(KOA EW殿SL432RD対応) -----
#Region "LblDataFileNameのText変更時に第１抵抗のｶｯﾄﾃﾞｰﾀ表示処理をおこなう"
    '''=========================================================================
    ''' <summary>LblDataFileNameのText変更時に第１抵抗のｶｯﾄﾃﾞｰﾀ表示処理をおこなう</summary>
    ''' <value>ﾛｰﾄﾞしたﾃﾞｰﾀﾌｧｲﾙ名</value>
    ''' <returns>LblDataFileName.Text</returns>
    ''' <remarks>V6.1.4.0⑦</remarks>
    '''=========================================================================
    Public Property LblDataFileNameText() As String
        Get
            Return LblDataFileName.Text
        End Get
        Set(ByVal value As String)
            ' 第１抵抗のｶｯﾄﾃﾞｰﾀ表示を変更する
            If (String.Empty = value) Then
                SetFirstResData(True)
            Else
                SetFirstResData()
            End If
            LblDataFileName.Text = value
        End Set
    End Property
#End Region
    '----- V6.1.4.0⑦↑ -----

    Private _jogKeyDown As Action(Of KeyEventArgs) = Nothing
    Private _jogKeyUp As Action(Of KeyEventArgs) = Nothing

#Region "表示中のJOGを制御するKeyDown,KeyUp時の処理をメインフォームに"
    '''=========================================================================
    ''' <summary><para>表示中のJOGを制御するKeyDown,KeyUp時の処理をメインフォームに、</para>
    ''' <para>カメラ画像MouseClick時の処理をDllVideoに設定する</para></summary>
    ''' <param name="keyDown"></param>
    ''' <param name="keyUp"></param>
    ''' <param name="moveToCenter">カメラ画像クリック位置を画像センターに移動する処理</param>
    ''' <remarks>'V6.0.0.0⑩</remarks>
    '''=========================================================================
    Friend Sub SetActiveJogMethod(ByVal keyDown As Action(Of KeyEventArgs),
                                  ByVal keyUp As Action(Of KeyEventArgs),
                                  ByVal moveToCenter As Action(Of Decimal, Decimal))
        _jogKeyDown = keyDown
        _jogKeyUp = keyUp

        'カメラ画像表示PictureBoxクリック位置をJOG経由で画像センターに移動する
        VideoLibrary1.MoveToCenter = moveToCenter
    End Sub
#End Region

    Private mExit_flg As Integer                            ' ボタン押した結果格納用
    Private _readFileVer As Double = FileIO.FILE_VER_10_10  ' ロードしたファイルのバージョンを保持する   V4.0.0.0-28
    Private _editNewData As Boolean = False                 'V4.12.4.0②
    Private _mapDisp As Boolean = False                     '#4.12.2.0①
    Private _mapPdfDir As String                            'V6.0.1.0⑫
    Private _PrintDisp As Boolean = False                   'V6.1.1.0②
#End Region

#Region "シャットダウン処理-強制終了"
    '''=========================================================================
    ''' <summary>シャットダウン処理-強制終了</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub SystemEvents_SessionEnding(
            ByVal sender As Object,
            ByVal e As SessionEndingEventArgs)
        If e.Reason = SessionEndReasons.SystemShutdown Then
            Call AplicationForcedEnding()
        End If
    End Sub
#End Region

    '=========================================================================
    '   フォームの初期化/終了処理
    '=========================================================================
#Region "表示言語設定"
    '''=========================================================================
    ''' <summary>表示言語設定</summary>
    ''' <remarks>InitializeComponent() よりも前に呼び出すこと  'V4.4.0.0-0</remarks>
    '''=========================================================================
    Private Sub SetCurrentUICulture()
        ' 構造体の初期化
        Call Init_Struct()
        Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)     ' ｼｽﾊﾟﾗ取得

        Dim culture As CultureInfo
        Select Case gSysPrm.stTMN.giMsgTyp
            Case 0      ' 日本語(既定値)
                culture = New CultureInfo("ja")
            Case 1      ' 英語
                culture = New CultureInfo("en")
            Case 2      ' ﾖｰﾛｯﾊﾟ
                culture = New CultureInfo("en")
            Case 3      ' 中国語(簡体字)
                culture = New CultureInfo("zh-Hans")
            Case Else   ' 既定値(日本語)
                culture = New CultureInfo("ja")
        End Select

        Thread.CurrentThread.CurrentCulture = culture           ' 不要 ?
        Thread.CurrentThread.CurrentUICulture = culture

    End Sub

#End Region
    '----- V6.1.1.0⑦↓ -----
#Region "画面が表示された時の処理"
    '''=========================================================================
    ''' <summary>画面が表示された時の処理</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub Form1_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        Dim strMSG As String

        Try

            PanelMap.Enabled = _mapDisp
            PanelMap.Visible = _mapDisp                                 ' MAP ON/OFFボタンを設定する
            CmdPrintMap.Visible = Not _PrintDisp                        ' PRINT ON/OFFボタン

            If (_mapDisp) Then
                PanelMap.Location = New Point(txtLog.Left, txtLog.Top - PanelMap.Height)
                CmdMapOnOff.Enabled = True
            Else
                CmdPrintMap.Enabled = False
                CmdMapOnOff.Enabled = False
            End If

            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
            ' 生産管理情報の伝票Ｎｏ.と抵抗値を表示する(【ＱＲコードリード機能】特注)
            If (giQrCodeType = QrCodeType.KoaEw) Then
                Me.LblcLOTNUMBER.Visible = True                                     ' 生産管理情報(伝票Ｎｏ．) 
                Me.LblcRESVALUE.Visible = True                                      ' 生産管理情報(抵抗値)
                If (gTkyKnd = KND_CHIP) Then                                        'V6.1.4.9④
                    Me.pnlFirstResData.Location = New System.Drawing.Point(75, 757)     'V6.1.4.4①                                                                  'V6.1.4.14①移動
                    Me.chkDistributeOnOff.Location = New System.Drawing.Point(12, 743)  ' 第１抵抗カットデータ表示域に被るので生産グラフ表示ボタンの位置を上げる     'V6.1.4.14①移動
                    Me.pnlFirstResData.Visible = True                                   ' 第１抵抗カットデータ表示域
                    Me.pnlFirstResData.Enabled = True                                   'V6.1.4.14①
                End If                                                              'V6.1.4.9④
                'V6.1.4.4①                Me.pnlFirstResData.Location = New System.Drawing.Point(75, 783)
                'V6.1.4.14①                Me.pnlFirstResData.Location = New System.Drawing.Point(75, 757)     'V6.1.4.4①
                'V6.1.4.14①                Me.chkDistributeOnOff.Location = New System.Drawing.Point(12, 743)  ' 第１抵抗カットデータ表示域に被るので生産グラフ表示ボタンの位置を上げる
                'V6.1.4.14①↓
                If (gTkyKnd = KND_NET) Then
                    Me.pnlFirstResDataNET.Location = New System.Drawing.Point(75, 750)
                    Me.chkDistributeOnOff.Location = New System.Drawing.Point(12, 728)  ' 表示ボタンの位置
                    Me.pnlFirstResDataNET.Visible = True                            ' ＮＥＴ時は抵抗４のカットオフ表示
                    Me.pnlFirstResDataNET.Enabled = True
                End If
                'V6.1.4.14①↑
            Else
                Me.LblcLOTNUMBER.Visible = False
                Me.LblcRESVALUE.Visible = False
                Me.pnlFirstResData.Visible = False
            End If
            '----- V6.1.4.0_22↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Form1_Shown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V6.1.1.0⑦↑ -----
#Region "メインフォームの初期化処理"
    '''=========================================================================
    '''<summary>メインフォームの初期化処理</summary>
    '''<remarks>Form_Initialize は Form_Initialize_Renamed にアップグレードされました。</remarks>
    '''=========================================================================
    Private Sub Form_Initialize_Renamed()

        '----- V1.16.0.2①↓ -----
        Dim strSECT As String
        Dim strKEY As String
        Dim sPath As String
        Dim i As Integer
        '----- V1.16.0.2①↑ -----
        Dim strMSG As String
        Dim r As Short

        Try
            '-----------------------------------------------------------------------
            '   多重起動防止Mutexハンドル
            '-----------------------------------------------------------------------
            If gmhTky.WaitOne(0, False) = False Then
                '' すでに起動されている場合
                '   →メッセージボックスがＳＴＡＲＴボタン入力待ちなどの状態で、後ろに回ることがあるので、表示はやめる。
                'MessageBox.Show("Cannot run TKY's family.(Another Process of TKY's family is already running.", "Trimmer Program", _
                '                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, _
                '                MessageBoxOptions.ServiceNotification, False)
                End
            End If

            'シャットダウンイベント処理関数
            AddHandler SystemEvents.SessionEnding, AddressOf SystemEvents_SessionEnding

            '-----------------------------------------------------------------------
            '   初期処理
            '-----------------------------------------------------------------------
            ChDir(WORK_DIR_PATH)                        ' 作業用ﾌｫﾙﾀﾞｰ(C:\TRIM)
            Timer1.Enabled = False                      ' 監視タイマー停止
            ' INtime稼動チェック (※#If cOFFLINEcDEBUGを使用するとINtimeGWInitializeの定義なしとなる)
#If 0 = cOFFLINEcDEBUG Then
            r = ISALIVE_INTIME()
            If (r = ERR_INTIME_NOTMOVE) Then
                'エラーメッセージの表示。(System1.TrmMsgBoxはここでは使用できない為、標準メッセージボックス)
                MessageBox.Show("Real-time control module has not loaded.", "Trimmer Program", MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly, False)
                'Call MsgBox("Real-time control module has not loaded.", vbOKOnly Or vbCritical, gAppName)
                End                                             ' アプリ終了 
            End If
#End If

            ' コマンドライン引数(アプリケーション種別)の取得
            Dim strARG() As String
            strARG = Environment.GetCommandLineArgs()       ' コマンドライン引数の取得
            If (strARG.Length <= 1) Then                    ' 引数なし ?
                strMSG = Process.GetCurrentProcess().ProcessName
                Console.WriteLine("Process Name = " + strMSG)
                If (strMSG = "TKYCHIP") Then
                    gTkyKnd = KND_CHIP                      ' アプリケーション種別 = TKYCHIP
                ElseIf (strMSG = "TKYNET") Then
                    gTkyKnd = KND_NET                       ' アプリケーション種別 = TKYNET
                Else
                    gTkyKnd = KND_TKY                       ' アプリケーション種別 = TKY
                End If
            Else                                            ' 引数あり 
                gTkyKnd = strARG(1)                         ' アプリケーション種別(0:TKY, 1=CHP, 2:NET)
                If (gTkyKnd <> KND_TKY) And (gTkyKnd <> KND_CHIP) And (gTkyKnd <> KND_NET) Then
                    MsgBox("ArgV Error.", vbOKOnly Or vbCritical, gAppName)
                    End                                     ' アプリ終了 
                End If
            End If

            DataManager.Initialize(DirectCast(CType(gTkyKnd, Integer), TkyKind))        'V5.0.0.8①

            ' アプリケーション種別の設定
            If (gTkyKnd = KND_CHIP) Then
                gAppName = APP_CHIP

                'INFO_MSG18 = "第1グループ、第1抵抗基準位置のティーチング"
                Globals_Renamed.INFO_MSG18 = INFO_MSG18_CHIP
                'INFO_MSG20 = "グループ、最終抵抗基準位置のティーチング"
                Globals_Renamed.INFO_MSG20 = INFO_MSG20_CHIP
                'ERR_TXNUM_E = "抵抗数が１のためこのコマンドは実行できません！"
                Globals_Renamed.ERR_TXNUM_E = ERR_TXNUM_E_CHIP

            ElseIf (gTkyKnd = KND_NET) Then
                gAppName = APP_NET

                'INFO_MSG18 = "第1グループ、第1サーキット位置のティーチング"
                Globals_Renamed.INFO_MSG18 = INFO_MSG18_NET
                'INFO_MSG20 = "グループ、最終サーキット基準位置のティーチング"
                Globals_Renamed.INFO_MSG20 = INFO_MSG20_NET
                'ERR_TXNUM_E = "サーキット数が１のためこのコマンドは実行できません！"
                Globals_Renamed.ERR_TXNUM_E = ERR_TXNUM_E_NET

            Else
                gAppName = APP_TKY
            End If

            'V6.5.5.0①↓
            Dim CpuNo As Integer = Integer.Parse(GetPrivateProfileString_S("DEVICE_CONST", "PROCESS_AFFINITY", SYSPARAMPATH, "0"))
            If (1 <= CpuNo) AndAlso (CpuNo <= &H7F) Then 'CPU指定はBIT割り当てでCPU0～CPU6までを有効としてその場合のみ設定する
                Dim tmpProcess As Process = Process.GetCurrentProcess()
                Dim vHandle As IntPtr
                vHandle = tmpProcess.ProcessorAffinity
                tmpProcess.ProcessorAffinity = CpuNo
            End If
            'V6.5.5.0①↑

            ' フラグ等初期化
            gbInitialized = False
            pbVideoInit = False
            pbVideoCapture = False                          ' ビデオキャプチャー開始フラグOFF
            gflgResetStart = False                          ' 初期設定フラグOFF
            'bFgfrmDistribution = False                      ' 生産ｸﾞﾗﾌ表示ﾌﾗｸﾞOFF
            gLoadDTFlag = False                              ' ﾃﾞｰﾀﾛｰﾄﾞ済ﾌﾗｸﾞ(False:ﾃﾞｰﾀ未ﾛｰﾄﾞ, True:ﾃﾞｰﾀﾛｰﾄﾞ済)
            gCmpTrimDataFlg = 0                             ' データ更新フラグ(0=更新なし, 1=更新あり)
            giTrimErr = 0                                   ' ﾄﾘﾏｰ ｴﾗｰ ﾌﾗｸﾞ初期化
            giAppMode = APP_MODE_IDLE                       ' ｱﾌﾟﾘﾓｰﾄﾞ = トリマ装置アイドル中
            giTempGrpNo = 1                                 ' テンプレートグループ番号(1～999)
            gPrevTrimMode = -1                              ' デジタルＳＷ値初期化
            gESLog_flg = False                              ' ESログOFF
            'giAdjKeybord = 0                                 ' トリミング中ADJ機能キーボード矢印(0:入力なし)
            gLoggingHeader = False                          ' ﾌｧｲﾙﾛｸﾞﾍｯﾀﾞｰ出力ﾌﾗｸﾞ(TRUE:出力)
            gwPrevHcmd = 0                                  ' ローダ入力データ退避域
            gManualThetaCorrection = True                   ' シータ補正実行フラグ = True(シータ補正を実行する)
            gflgCmpEndProcess = False                       ' 終了処理完了フラグ
            gbFgAutoOperation = False                       ' 自動運転フラグ(True:自動運転中, False:自動運転中でない) 
            giAutoDataFileNum = 0                           ' 連続運転登録データファイル数 
            giSubExistMsgFlag = True                        ' 基板有無チェック時ない場合にメッセージを表示しない V4.11.0.0⑧
            'm_keepHaltSwSts = False                         ' HALTキー状態(True=ON, False=OFF) 

            'V4.0.0.0-69                ↓↓↓↓
            '' 構造体の初期化                          'V4.4.0.0-0  SetCurrentUICulture()に移動
            'Call Init_Struct()
            'Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)         ' ｼｽﾊﾟﾗ取得

            Dim dir As String = String.Empty
            Try
                ' 指定ﾌｫﾙﾀﾞが存在しない場合に作成する
                '----- V6.1.4.0⑬↓ -----
                'dir = DATA_DIR_PATH                             'V5.0.0.8② C:\TRIMDATA\DATA
                'If (False = IO.Directory.Exists(dir)) Then IO.Directory.CreateDirectory(dir)
                'dir = gSysPrm.stLOG.gsLoggingDir
                'If (False = IO.Directory.Exists(dir)) Then IO.Directory.CreateDirectory(dir)

                Dim dirs() As String = New String() {
                    DATA_DIR_PATH,
                    gSysPrm.stLOG.gsLoggingDir,
                    gSysPrm.stDIR.gsTrimFilePath
                }

                For Each dir In dirs
                    If (False = IO.Directory.Exists(dir)) Then IO.Directory.CreateDirectory(dir)
                Next
                '----- V6.1.4.0⑬↑ -----

            Catch ex As Exception
                ' 権限がない場合やﾄﾞﾗｲﾌﾞが存在しない場合など
                strMSG = Environment.NewLine &
                    "System.IO.CreateDirectory(""" & dir & """)" & Environment.NewLine & ex.Message
                Throw New Exception(strMSG)
            End Try
            'V4.0.0.0-69                ↑↑↑↑

            'V4.1.0.0③
            'V6.0.0.0⑤            FinalEnd_GazouProc(ObjGazou)

#If START_KEY_SOFT Then
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "START_KEY_SOFT", r)
            If r = 1 Then
                gbStartKeySoft = True
            Else
                gbStartKeySoft = False
            End If
#End If
            ''V5.0.0.9③      ↓
            'If (KEY_TYPE_R = gSysPrm.stTMN.giKeiTyp) Then
            '    ' クランプレス載物台(0でない場合クランプレス)
            '    giClampLessStage = Integer.Parse(GetPrivateProfileString_S(
            '                                     "DEVICE_CONST", "CLAMP_LESS_STAGE", SYSPARAMPATH, "0"))

            '    If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then
            '        ' クランプレス載物台の場合、クランプなしとする
            '        gSysPrm.stIOC.giClamp = 0S

            '        'V5.0.0.9⑥          ↓
            '        gdClampLessOffsetX = Double.Parse(GetPrivateProfileString_S(
            '                                          "DEVICE_CONST", "CLAMP_LESS_OFFSET_X", SYSPARAMPATH, "0.0"))

            '        gdClampLessOffsetY = Double.Parse(GetPrivateProfileString_S(
            '                                          "DEVICE_CONST", "CLAMP_LESS_OFFSET_Y", SYSPARAMPATH, "0.0"))

            '        giClampLessRoughCount = Integer.Parse(GetPrivateProfileString_S(
            '                                              "DEVICE_CONST", "CLAMP_LESS_ROUGH_COUNT", SYSPARAMPATH, "0"))
            '        ''V6.0.2.0⑥↓
            '        'gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
            '        '                                "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))   'V5.0.0.9⑨
            '        ''V6.0.2.0⑥↑
            '    Else
            '        gdClampLessOffsetX = 0.0
            '        gdClampLessOffsetY = 0.0
            '        giClampLessRoughCount = 0
            '        gdClampLessTheta = 0.0      'V5.0.0.9⑨
            '        'V5.0.0.9⑥          ↑
            '    End If

            '    'V4.12.2.0⑥         ↓'V6.0.4.1①
            '    If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then
            '        If (0 <> giClampLessStage) Then
            '            W_Write(LOFS_W192, LPC_CLAMP_EXEC)
            '        Else
            '            W_Write(LOFS_W192, 0)
            '        End If
            '    End If
            '    'V4.12.2.0⑥         ↑'V6.0.4.1①

            '    'V6.0.2.0⑥↓
            '    gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
            '                                        "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))
            '    'V6.0.2.0⑥↑

            'End If
            ''V5.0.0.9③                  ↑

            ' ローダ初期化
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then               ' SL436R系 ?
                '###148 Call SetLoaderIO(LOUT_NO_ALM, Not (LOUT_NO_ALM))        ' ローダ初期化(ON=トリマ部正常, OFF=左記以外) 
                Call SetLoaderIO(0, 0)                                          ' ローダ初期化(ON=トリマ部正常, OFF=左記以外) 
            Else
                Call SetLoaderIO(COM_STS_TRM_STATE, Not (COM_STS_TRM_STATE))    ' ローダー出力(ON=トリマ動作中, OFF=左記以外)
            End If

            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
            If (giDoorLock = 1) Then                        ' 電磁ロック有効 ?
                Call EXTOUT1(0, EXTOUT_EX_LOK_ON)
            End If
            '----- V1.18.0.1⑧↑ -----

            'V4.11.0.0⑭
            ' パワーモニタシャッターON/OFF(0=シャッタ閉、1=シャッタ開)
            Call PMON_SHUTCTRL(0)
            'V4.11.0.0⑭

            '' 構造体の初期化
            'Call Init_Struct()         'V4.0.0.0-69    上に移動

            ' 配列の初期化
            Call Init_Arrey()

            '-----------------------------------------------------------------------
            '   使用するＯＣＸの初期設定を行う
            '-----------------------------------------------------------------------
            Call Ocx_Initialize()
            Call InitFunction()
            Call PROP_SET(0.0, 0.0, 0.0, 0.0, 0.0, 0.0)     ' システム変数初期設定(プローブON/OFF位置他)

            '-----------------------------------------------------------------------
            '   システム設定ファイルリード
            '   ※システムパラメータの送信はOcxSystemのSetOptionFlg()で行う
            '-----------------------------------------------------------------------
            'Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)        'V4.0.0.0-69    上に移動
            Call Me.System1.SetMessageConst(gSysPrm)        ' OcxSystemのﾒｯｾｰｼﾞを設定する(ｼｽﾃﾑﾊﾟﾗﾒｰﾀのﾘｰﾄﾞ後で行う)
            Call GetFncDefParameter(gTkyKnd)                ' 機能選択定義テーブル設定
            '                                               ' Video.ocx用オプション定義取得
            Call gDllSysprmSysParam_definst.GetSysprmOptVideo(OptVideoPrm)
            Call Me.System1.SetSysParam(gSysPrm)            ' OcxSystem用のシステムパラメータを設定する

            '' メッセージ初期設定処理
            'Call PrepareMessages(gSysPrm.stTMN.giMsgTyp)
            'Call PrepareMessages_N(gSysPrm.stTMN.giMsgTyp)
            'Call PrepareMessagesCutPosCorrect_Calibration(gSysPrm.stTMN.giMsgTyp) 'V1.20.0.0⑧

            '----- V1.16.0.2①↓ -----
            ' 電流値が0.5Aの場合はレンジ1-9の補正値をリードし直す
            If (gSysPrm.stSPF.giProcPower2 = 3) Then             ' 電流値は0.5A ?
                sPath = "C:\TRIM\TKYSYS.ini"
                strSECT = "DRM_KG500MA"
                For i = 1 To 9
                    strKEY = "DRM_KGL_" + Format(i, "000")
                    ' ※gfDrm_kgL().NETは0 ORGだがVB6(OcxSYStem)は1 ORGなので注意 
                    ''V4.0.0.0-70                    gSysPrm.stDRM.gfDrm_kgL(i - 1) = Val(GetPrivateProfileString_S(strSECT, strKEY, sPath, "1.0"))
                    gSysPrm.stDRM.gfDrm_kgL(i) = Val(GetPrivateProfileString_S(strSECT, strKEY, sPath, "1.0"))
                    'V4.0.0.0-70
                Next i
            End If
            '----- V1.16.0.2①↑ -----

            ' その他のシスパラを取得する
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "GPIB_ADR", giGpibDefAdder)                    ' GPIBアドレス ###002
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "FULLCUT_ZPOS", giFullCutZpos)                 ' 強制カットモード(x5モード)時のZの位置指定(0=OFF位置, 1=STEP位置, 2=ON位置) V3.0.0.1①
            giBuzerOn = Val(GetPrivateProfileString_S("CUSTOMIZE", "TRIMEND_BUZER_ON", SYSPARAMPATH, "0"))          ' V6.1.1.0①
            giAlmTimeDsp = Val(GetPrivateProfileString_S("CUSTOMIZE", "ALARM_TIME_DISP", SYSPARAMPATH, "0"))        ' V6.1.1.0⑬
            giNgCountAss = Val(GetPrivateProfileString_S("CUSTOMIZE", "NGBOX_COUNT_ASS", SYSPARAMPATH, "0"))        ' V6.1.1.0⑨
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "DISP_ENDTIME", giDispEndTime)                 ' V6.1.1.0③
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "ALARM_ONOFF_BTN", giAlarmOnOff)               ' V6.1.1.0④
            Call Get_SystemParameterShort(SYSPARAMPATH, "LOGGING", "IX2LOG", giIX2LOG)                              ' IX2LOG ###231
            Call Get_SystemParameterShort(SYSPARAMPATH, "LOGGING", "SUMMARY_LOG", giSummary_Log)                    ' V1.22.0.0④
            Call Get_SystemParameterShort(SYSPARAMPATH, "OPT_VIDEO", "TABLE_POS_UPDATE", giTablePosUpd)             ' ###234
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "TRIM_EXEC_NOWORK", giTrimExe_NoWork)    ' ###240
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "TENKEY_BTN", giTenKey_Btn)              ' ###268
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "BPADJ_HALTMENU", giBpAdj_HALT)          ' ###269
            giProbeCheck = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "PROBE_CHECK", SYSPARAMPATH, "0"))      ' V1.23.0.0⑦
            giPltCountMode = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "PLATE_COUNT_MODE", SYSPARAMPATH, "0")) 'V6.0.3.0①
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "CUTOFF_ADJUST", gAdjustCutoffFunction)  ' V6.0.3.0⑦
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", "CUTOFF_ADJUST_LOGOUT", giCutOffLogOut)  ' V6.0.3.0⑦
            Call Get_SystemParameterShort(SYSPARAMPATH, "TMENU", "KEITYP", giMachineKd)                             ' V1.13.0.0⑦
            Call Get_SystemParameterShort(SYSPARAMPATH, "RMCTRL", "AUTOPOWER_LASER_COMMAND", giAutoPwr)             ' V1.14.0.0②
            Call Get_SystemParameterShort(SYSPARAMPATH, "RMCTRL", "NODSP_ATT_RATE", giNoDspAttRate)                 ' V1.16.0.0⑭
            dblCalibHoseiX = Val(GetPrivateProfileString_S("CUSTOMIZE", "CALIBHOSEIX", SYSPARAMPATH, "1.0"))        ' V1.20.0.0⑦

            ''V6.0.1.021　↓
            ' ファイバーレーザで固定アッテネータON/OFFのみ行う    
            Call Get_SystemParameterShort(SYSPARAMPATH, "OPT_LASER_TEACH", "FL_FIX_ATT_USE", giFixAttOnly)
            ''V6.0.1.021　↑

            'V5.0.0.9⑮                  ↓
            Dim bt As BarcodeType
            If ([Enum].TryParse(Of BarcodeType)(
                GetPrivateProfileString_S("CUSTOMIZE", "BARCODE_TYPE", SYSPARAMPATH, "0"), bt)) Then
                ' 0(なし), 1(Walsin), 2(太陽社), 3(標準オプション)
                BarCode_Data.Type = bt
            Else
                BarCode_Data.Type = BarcodeType.None
            End If

            If (BarcodeType.None <> BarCode_Data.Type) Then
                For idx As Integer = 1 To 2 Step 1
                    Dim str As String = GetPrivateProfileString_S(
                        "CUSTOMIZE", "BARCODE_SUBSTR" & idx, SYSPARAMPATH, "")
                    If ("" = str) Then Continue For

                    Dim sp() As String = str.Split("-"c)
                    If (2 <> sp.Length) Then Continue For

                    Dim p1, p2 As Integer
                    If (Integer.TryParse(sp(0), p1) AndAlso (0 < p1) AndAlso
                        Integer.TryParse(sp(1), p2) AndAlso (0 < p2)) Then
                        p1 -= 1             ' string.Substring(p1, p2), p1は0ｵﾘｼﾞﾝ
                        BarCode_Data.SubStr.Add(Tuple.Create(p1, p2))
                    End If
                Next idx
            End If
            'V5.0.0.9⑮                  ↑
            gsComPort = GetPrivateProfileString_S("CUSTOMIZE", "COM", SYSPARAMPATH, "COM6")                         ' V1.23.0.0①
            strBCConvFileFullPath = GetPrivateProfileString_S("CUSTOMIZE", "BARCODE_FILENAME", SYSPARAMPATH, "C:\TRIMDATA\436R立上げ用ファイル.CSV") ' V1.23.0.0①
            gdblStg2ndOrgX = Val(GetPrivateProfileString_S("CUSTOMIZE", "STAGE_2NDPOSX", SYSPARAMPATH, "0.0"))      ' V1.13.0.0⑧
            gdblStg2ndOrgY = Val(GetPrivateProfileString_S("CUSTOMIZE", "STAGE_2NDPOSY", SYSPARAMPATH, "0.0"))      ' V1.13.0.0⑧
            gdblStgOrgMoveX = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "STAGE_ORG_MOVE_POSX", SYSPARAMPATH, "0.0"))    ' V1.13.0.0⑩
            gdblStgOrgMoveY = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "STAGE_ORG_MOVE_POSY", SYSPARAMPATH, "0.0"))    ' V1.13.0.0⑩
            gsEDIT_DIGITNUM = GetPrivateProfileString_S("CUSTOMIZE", "EDIT_DIGITNUM", SYSPARAMPATH, "0.00000")      ' V1.16.0.0③
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "AUTOMODE_DATA_SELECT", giAutoModeDataSelect)  ' V1.18.0.0⑧
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "AUTOMODE_CONTINUE", giAutoModeContinue)       ' V1.18.0.0⑨
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "BTN_AUTOPOWER", giBtnPwrCtrl)                 ' V1.18.0.4①
            giFLPrm_Ass = Val(GetPrivateProfileString_S("CUSTOMIZE", "FL_PARAM_ASSIGN", SYSPARAMPATH, "0"))         ' V2.0.0.0⑤
            gsFLPrmFile = GetPrivateProfileString_S("CUSTOMIZE", "FL_PARAM_NAME", SYSPARAMPATH, "C:\TRIMDATA\DATA\FLParamFile.xml") ' V2.0.0.0⑤
            giDspScreenKeybord = Val(GetPrivateProfileString_S("CUSTOMIZE", "DSP_SCREEN_KEYBORD", SYSPARAMPATH, "1")) ' V2.0.0.0⑦(V1.22.0.0⑧)
            giBtn_EdtLock = Val(GetPrivateProfileString_S("CUSTOMIZE", "BTN_EDITLOCK", SYSPARAMPATH, "0"))          ' V2.0.0.0_25
            giDspCmdName = Val(GetPrivateProfileString_S("CUSTOMIZE", "DSP_CMD_NAME", SYSPARAMPATH, "0"))           ' V1.18.0.1①
            giDoorLock = Val(GetPrivateProfileString_S("CUSTOMIZE", "DOOR_LOCK", SYSPARAMPATH, "0"))                ' V1.18.0.1⑧
            ' ↓↓↓ V3.1.0.0② 2014/12/01
            Call Get_SystemParameterShort(SYSPARAMPATH, "MEASUREMENT", "MEASUREMENT", giMeasurement)                ' 測定方法
            gdRESISTOR_MIN = Val(GetPrivateProfileString_S("MEASUREMENT", "RESISTOR_MIN", SYSPARAMPATH, "0.0"))     ' 抵抗最小値
            gdRESISTOR_MAX = Val(GetPrivateProfileString_S("MEASUREMENT", "RESISTOR_MAX", SYSPARAMPATH, "0.0"))     ' 抵抗最大値
            ' ↑↑↑ V3.1.0.0② 2014/12/01
            'V5.0.0.9⑱↓
            Dim rngDiv As Double = Double.Parse(GetPrivateProfileString_S("MEASUREMENT", "POWER_RESOLUTION", SYSPARAMPATH, "0.0"))
            If rngDiv > 0.0 Then
                Call SETPOWERRESOLUTION(rngDiv)
            End If
            'V5.0.0.9⑱↑
            giTeachpointUse = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CUTREVIDE_TEACHPOINT_USE", SYSPARAMPATH, "0"))      'V5.0.0.6⑩
            'V5.0.0.6①↓
            If Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CONTROLLER_INTERLOCK", SYSPARAMPATH, "0")) = 1 Then
                gbControllerInterlock = True
            Else
                gbControllerInterlock = False
            End If
            'V5.0.0.6①↑
            'V5.0.0.6②↓
            If gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432 And Val(GetPrivateProfileString_S("SPECIALFUNCTION", "LOADER_SECOND_POS", SYSPARAMPATH, "0")) = 1 Then
                gbLoaderSecondPosition = True
            Else
                gbLoaderSecondPosition = False
            End If
            'V5.0.0.6②↑

            If gKeiTyp = KEY_TYPE_RS Then
                Call Get_SystemParameterShort(SYSPARAMPATH, "DEVICE_VAR", "SIMPLE_CROSSLINEX", CROSS_LINEX)
                Call Get_SystemParameterShort(SYSPARAMPATH, "DEVICE_VAR", "SIMPLE_CROSSLINEY", CROSS_LINEY)
                'V4.8.0.1①↓ 歩留まり表示かNG率表示にするかの設定読み込み
                Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "RATE_DISP", giRateDisp)
                'V4.8.0.1①↑
                Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "CHANGE_CUTPOINT", giChangePoint) 'V4.9.0.0②
                ''V4.9.0.0①↓
                Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "NG_STOP", giNgStop)
                If giNgStop <> 0 Then
                    ReadLotStopData()
                End If
                ''V4.9.0.0①↑
            End If

            ' 'V4.5.0.1①
            giClampSeq = Val(GetPrivateProfileString_S("CUSTOMIZE", "CLAMP_SEQ", SYSPARAMPATH, "0"))     ' 抵抗最大値

            'V4.6.0.0①
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "MANUAL_CMD_START_SEQ", giManualSeq)
            Call Get_SystemParameterShort(SYSPARAMPATH, "CUSTOMIZE", "TIME_LOT_ONLY", giTimeLotOnly)
            'V4.6.0.0①

            ' 扉開OK 表示・非表示   'V5.0.0.9⑰ 
            Me.lblDoorOpen.Visible = ("0" <> GetPrivateProfileString_S("CUSTOMIZE", "DOOR_OPEN", SYSPARAMPATH, "0"))

            ' MAP ON/OFFボタン使用する・しない     '#4.12.2.0①
            _mapDisp = ("0" <> GetPrivateProfileString_S("DEVICE_CONST", "MAP_DISP", SYSPARAMPATH, "0"))
            '----- V6.1.1.0②↓ -----
            ' PRINT ON/OFFボタンを有効にする(0)/無効にする(1)　
            _PrintDisp = ("0" <> GetPrivateProfileString_S("DEVICE_CONST", "PRINT_NOTDISP", SYSPARAMPATH, "0"))
            '----- V6.1.1.0②↑ -----
            ' プリンタオフライン時の MAP.pdf 保存フォルダ    'V6.0.1.0⑫
            _mapPdfDir = GetPrivateProfileString_S("MAP", "PDF_DIR", SYSPARAMPATH, "C:\TRIMDATA\LOG\")

            ' トリミングの判定結果と色・合計数を管理する         'V6.0.1.0⑫
            _trimmingResults = New Dictionary(Of Integer, TrimmingResult) From
            {
                {TRIM_RESULT_OK, New TrimmingResult(TrimMap.ColorOK, "OK")},
                {TRIM_RESULT_FT_HING, New TrimmingResult(TrimMap.ColorNG_FtHigh, "NG-HI")},
                {TRIM_RESULT_FT_LONG, New TrimmingResult(TrimMap.ColorNG_FtLow, "NG-LO")},
                {TRIM_RESULT_OVERRANGE, New TrimmingResult(TrimMap.ColorNG, "OVER-RANGE")},
                {TRIM_RESULT_NOTDO, New TrimmingResult(TrimMap.ColorSelected, "PTN-NG")}
            }
            '{TRIM_RESULT_NOTDO, New TrimmingResult(TrimMap.ColorSelected, "NG")},

            HistoryDataLocation = frmHistoryData.Location               'V6.0.1.0⑪
            HistoryDataSize = frmHistoryData.Size                       'V6.0.1.0⑪

            '----- V4.0.0.0-40↓ -----
            ' ステージYの原点位置をシスパラからリードする
            strMSG = GetPrivateProfileString_S("DEVICE_CONST", "STAGE_DIRY", SYSPARAMPATH, "0")
            giStageYOrg = Integer.Parse(strMSG)
            If (giStageYOrg = STGY_ORG_DW) Then
                giStageYDir = STGY_DIR_CW
            Else
                giStageYDir = STGY_DIR_CCW
            End If
            '----- V4.0.0.0-40↑ -----
            '----- V4.11.0.0①↓ (WALSIN殿SL436S対応) -----
            giTargetOfs = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "TARGET_OFFSET", SYSPARAMPATH, "0"))
            giPwrChkPltNum = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "POWER_CHECK_PLATE_NUM", SYSPARAMPATH, "0"))
            giPwrChkTime = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "POWER_CHECK_TIME", SYSPARAMPATH, "0"))
            giTrimTimeOpt = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "TRIM_TIME_OPT", SYSPARAMPATH, "0"))
            giStartBlkAss = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "START_BLOCK_ASS", SYSPARAMPATH, "0"))
            'V5.0.0.9⑯                  ↓
            If (0 < giStartBlkAss) AndAlso (KEY_TYPE_R = gKeiTyp) Then
                giStartBlkAss = 2       ' Rは 2
            End If
            'V5.0.0.9⑯                  ↑
            giSubstrateInvBtn = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "SUBSTRATE_INVEST_BTN", SYSPARAMPATH, "0"))
            '----- V4.11.0.0①↑ -----
            '----- V6.1.4.0①↓(KOA EW殿SL432RD対応) -----
            giLotChange = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "LOT_CHANGE", SYSPARAMPATH, "0"))
            giFileMsgNoDsp = Val(GetPrivateProfileString_S("CUSTOMIZE", "FILE_MSG_NODSP", SYSPARAMPATH, "0"))
            'V6.1.4.0_22↓
            ' ＱＲコードリード機能はTKY-CHIPのみ有効
            If (gTkyKnd = KND_CHIP) And Integer.Parse(GetPrivateProfileString_S("QR_CODE", "QR_CODE_READER_USE", SYSPARAMPATH, "0")) = 1 Then
                gbQRCodeReaderUse = True
            Else
                gbQRCodeReaderUse = False
            End If
            'V6.1.4.10②↓
            ' ＱＲコードリード機能はTKY-CHIPのみ有効
            If (gTkyKnd = KND_NET) And Integer.Parse(GetPrivateProfileString_S("QR_CODE", "QR_CODE_READER_USE_TKYNET", SYSPARAMPATH, "0")) = 1 Then
                gbQRCodeReaderUseTKYNET = True
            Else
                gbQRCodeReaderUseTKYNET = False
            End If
            'V6.1.4.10②↑
            giQrCodeType = Val(GetPrivateProfileString_S("QR_CODE", "QR_CODE_TYPE", SYSPARAMPATH, "0"))
            Me.LblcLOTNUMBER.Text = ""                                  ' 生産管理情報(伝票Ｎｏ．) 特注
            Me.LblcRESVALUE.Text = ""                                   ' 生産管理情報(抵抗値) 　　特注
            'V6.1.4.0_22↑
            'V6.1.4.0_35↓レーザーパワーのモニタリング
            giLaserrPowerMonitoring = Integer.Parse(GetPrivateProfileString_S("ROT_ATT", "FULL_POWER_MONITORING", TKYSYSPARAMPATH, "0"))    '0：無し,1：自動運転開始時,2：エントリーロット毎
            gdFullPowerLimit = Double.Parse(GetPrivateProfileString_S("ROT_ATT", "FULL_POWER_LIMIT", TKYSYSPARAMPATH, "0.1"))
            gdFullPowerQrate = Double.Parse(GetPrivateProfileString_S("ROT_ATT", "FULL_POWER_QRATE", TKYSYSPARAMPATH, "10.000"))
            'V6.1.4.0_35↑
            '----- V6.1.4.0①↑ -----
            'V6.1.4.2①↓
            giAutoCalibration = Integer.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CALIBRATION", SYSPARAMPATH, "0"))          ' 自動キャリブレーション補正実行0:無し、>0：実行
            gbAutoCalibrationLog = Integer.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CALIBRATION_LOG", SYSPARAMPATH, "0"))   ' カット位置ずれログ出力　0:無し、1：出力
            'V6.1.4.2①↑
            If Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CPK_DISP_OFF", SYSPARAMPATH, "0")) = 1 Then  'V5.0.0.4④
                giCpk_Disp_Off = True
            Else
                giCpk_Disp_Off = False
            End If
            '----- V6.0.3.0_37↓ -----
            Call Get_SystemParameterShort(SYSPARAMPATH, "DEVICE_CONST", "VACUM_IO", gVacuumIO)
            strMSG = GetPrivateProfileString_S("DEVICE_CONST", "VACUM_IO", SYSPARAMPATH, "0")
            '----- V6.0.3.0_37↑ -----

            ' V4.12.0.0①↓'V6.1.2.0②
            iInverseStepY = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "INVERSE_STEPY", SYSPARAMPATH, "0")) ' 外部カメラカット位置画像登録のブロックNoX軸を千鳥対応の為に１に固定する。'V1.25.0.0⑫'V4.8.0.0②
            ' V4.12.0.0①↑'V6.1.2.0②

            ' ロット継続か終了かの選択メッセージを表示するかどうか
            giReqLotSelect = 0                                          ' V6.0.3.0_38

            ' クリーニング間隔設定(トリミングデータから読み込んだ設定で上書きする)
            gProbeCleaningSpan = 0

            'V6.0.1.0⑮↓
            '1基板内のNG個数によって停止する機能の有効／無効
            giNGCountInPlate = Val(GetPrivateProfileString_S("CUSTOMIZE", "NG_COUNT_IN_PLATE", SYSPARAMPATH, "0"))
            'V6.0.1.0⑮↑

            ' V4.5.0.5① 'V4.12.2.0⑨　'V6.0.5.0④
            gdblNgStopRate = Val(GetPrivateProfileString_S("CUSTOMIZE", "NG_RATE_IN_PLATE", SYSPARAMPATH, "0.0"))      ' V1.13.0.0⑧

            'V4.12.2.2⑤↓'V6.0.5.0⑧
            giGazouClrTime = Val(GetPrivateProfileString_S("CUSTOMIZE", "GAZOU_CLR_TIME", SYSPARAMPATH, "0"))
            'V4.12.2.2⑤↑'V6.0.5.0⑧
            '----- V6.0.3.0_47↓ -----
            ' MoveModeをマウスのホイールで動作させるのを有効/無効
            giMoveModeWheelDisable = Val(GetPrivateProfileString_S("CUSTOMIZE", "MOVEMODE_MOUSEWHEELDISABLE", SYSPARAMPATH, "0"))
            '----- V6.0.3.0_47↑ -----

            'V6.1.2.0①↓
            'ステージ動作の完了待ち実行有無のパラメータ設定[0:完了待ちする、1:動作指示のみ]
            giJogWaitMode = Convert.ToInt16(
                GetPrivateProfileString_S("CUSTOMIZE", "STAGEJOG_WAITMODE", SYSPARAMPATH, "0"))
            'V6.1.2.0①↑

            ' クリーニング間隔設定(トリミングデータから読み込んだ設定で上書きする)
            gProbeCleaningSpan = 0

            '----- V1.18.0.0②↓ -----
            ' QRデータのオフセット位置をシスパラから取得する(ローム殿特注)
            Call GetSysPrm_QR_DataOfs(SYSPARAMPATH)
            '----- V1.18.0.0②↑ -----
            '----- V1.18.0.0③↓ -----
            If (gSysPrm.stDEV.rPrnOut_flg = True) Then                  ' Printボタン有効 ? 
                giPrint = 1
            Else
                giPrint = 0
            End If
            '----- V1.18.0.0③↑ -----

            'V4.10.0.0⑩↓
            'gSysPrmは、SetCurrentUICultureの中で、 giMachineKdは、ここの上でセットされている。
            ' 装置種別の設定
            Select Case (gSysPrm.stTMN.gsKeimei)
                Case MACHINE_TYPE_SL432
                    gMachineType = MACHINE_TYPE_432R
                Case MACHINE_TYPE_SL436
                    gMachineType = MACHINE_TYPE_436R
                Case MACHINE_TYPE_SL436S
                    gMachineType = MACHINE_TYPE_436S
                Case Else
                    gMachineType = MACHINE_TYPE_432R
            End Select

            Select Case (giMachineKd)
                Case MACHINE_KD_R
                Case MACHINE_KD_RW
                    gMachineType = MACHINE_TYPE_432RW
                Case MACHINE_KD_RS
                    gMachineType = MACHINE_TYPE_436S
                Case Else
            End Select
            'V4.10.0.0⑩↑

            '----- V6.0.3.0⑳↓ -----
            lblCutOff.Visible = False
            '----- V6.0.3.0⑳↑ -----

            '' --- V4.1.0.0①↓-----------------------------------------------------
            'Dim DspMsg As String
            'Dim strSetFullPath As String

            'strSetFullPath = "C:\TRIMDATA\DATA\"
            'DspMsg = strSetFullPath
            '' "ＱＲコードに対応したトリミングデータがありません。","ファイルを確認してください。","パス表示"
            'r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
            '        MSG_SPRASH48, MSG_SPRASH49, DspMsg, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
            '' --- V4.1.0.0①↑-----------------------------------------------------

            ' ログ画面表示クリア基板枚数をシスパラより取得する ###013
            If (gAppName = APP_CHIP) Then
                strMSG = "DISP_CLS_CHIP"
            ElseIf (gAppName = APP_NET) Then
                strMSG = "DISP_CLS_NET"
            Else
                strMSG = "DISP_CLS_TKY"
            End If
            Call Get_SystemParameterShort(SYSPARAMPATH, "SPECIALFUNCTION", strMSG, gDspClsCount)    ' ログ画面表示クリア基板枚数
            If (gDspClsCount <= 0) Then gDspClsCount = 1
            gDspCounter = 0                                                                         ' ログ画面表示基板枚数カウンタ

            ' Shiftキー押下しながらの編集画面実行で初期値のデータを編集(新規作成)する           'V4.12.4.0②
            _editNewData = ("0" <> (GetPrivateProfileString_S("TMENU", "EDIT_NEW", SYSPARAMPATH, "0")))

            ' EXTOUT LED制御ビット(BIT4-7)をシスパラより設定する '###061
            glLedBit = Val(GetPrivateProfileString_S("IO_CONTROL", "ILUM_BIT", SYSPARAMPATH, "16"))
#If SANADA_KOA Then
            Call EXTOUT1(glLedBit, 0)                       ' バックライト照明ＯＮ・落射照明
#End If
            ' SL436RでCHIPの場合にTrimDataEditorExを使用しない=0,使用する=0でない 'V4.10.0.0②
            Globals_Renamed.giChipEditEx = Convert.ToInt32(GetPrivateProfileString_S("TMENU", "CHIP_EDITEX", SYSPARAMPATH, "0"))

            ' カット位置補正用パターン登録情報テーブル初期化(INtime側) ###092
            ' (TKYでカット位置補正実行後、CHIP/NETを実行するとカット位置補正が実行される場合がある)
            Call CUTPOSCOR_ALL(cMAXcRESISTORS, gStCutPosCorrData)

            '-----------------------------------------------------------------------
            '   ローダ通信用初期設定処理(SL436R用)
            '-----------------------------------------------------------------------
            GrpNgBox.Visible = False                                '###149
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                ' SL436R用シスパラをリードする
                Call GetSL436RSysparam()

                ' PLC通信部を初期化する
                Call Init_PlcIF()

                ' シスパラよりローダ用各種設定値を設定する  ※処理を追加する
                r = Loader_PutParameterFromSysparam()

                ' シスパラよりローダタイマ値を設定する　　　※処理を追加する
                r = Loader_PutTimerValFromSysparam()

                'GrpNgBox.Visible = True                    '###195 ###149
                GrpNgBox.Visible = False                    '###195
            End If

            '----------------------------------------------------------------------------
            '   ログインパスワードチェック(オプション)
            '----------------------------------------------------------------------------
            If (flgLoginPWD = 1) Then                       ' ﾛｸﾞｲﾝﾊﾟｽﾜｰﾄﾞ入力有 ? 
                r = Password1.ShowDialog((gSysPrm.stTMN.giMsgTyp), (gSysPrm.stSYP.gsLoginPassword))
                '----- V1.18.0.0①↓ -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then  ' ローム殿仕様 ? 
                    If (r = cFRS_ERR_RST) Then                  ' Cancel ? 

                    Else
                        lblLoginResult.Visible = True           ' ログインパスワードOKなら「Administrator Mode」と表示 
                    End If
                Else
                    If (r = cFRS_ERR_RST) Then                  ' Cancel ? 
                        End                                     ' アプリ終了
                    End If
                End If
                '----- V1.18.0.0①↑ -----
            End If
            Call Me.System1.OperationLogDelete(gSysPrm)        ' 古い操作ログファイルを削除する
            Call Me.System1.OperationLogging(gSysPrm, MSG_OPLOG_WAKEUP, gAppName)

            ' 変数初期化
            gSysPrm.stLOG.giLoggingAppend = 1                   ' ロギングは常にアペンドモード

            ' シグナルタワー制御初期化(On=0, Off=全ﾋﾞｯﾄｵﾌ)
            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
            ' Call Me.System1.SetSignalTower(0, &HFFFF)
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_ALL_OFF)
            'V5.0.0.9⑭ ↑ V6.0.3.0⑧

            ' BPキャリブレーションの初期値設定
            Call BP_CALIBRATION(1.0#, 1.0#, 0.0#, 0.0#)

            ' 補正後クロスラインX,Y非表示
            'V6.0.0.0④            Me.CrosLineX.Visible = False
            'V6.0.0.0④            Me.CrosLineY.Visible = False

            '----- ###232↓ ----
            ' クロスライン補正の初期化
            'V6.0.0.0③            ObjCrossLine.CrossLineParamINitial(Me.Picture2, Me.Picture1, Me.CrosLineX, Me.CrosLineY, 0.0, 0.0)
            ObjCrossLine.CrossLineParamINitial(AddressOf VideoLibrary1.GetCrossLineCenter,
                                               AddressOf VideoLibrary1.SetCorrCrossVisible,
                                               AddressOf VideoLibrary1.SetCorrCrossCenter,
                                               0.0, 0.0)                                'V6.0.0.0③
            '' 補正後クロスライン表示用パラメータ初期設定
            'gstCLC.k = 0                                    ' 種別(0:標準, 1:ﾒﾝﾃﾅﾝｽ)
            'gstCLC.x = 0                                    ' X方向のカット開始位置(mm)
            'gstCLC.y = 0                                    ' Y方向のカット開始位置(mm)
            'gstCLC.bpx = 0                                  ' Beem Position X OFFSET(mm)
            'gstCLC.bpy = 0                                  ' Beem Position Y OFFSET(mm)
            'gstCLC.Chk = gSysPrm.stCRL.giDspFlg             ' 補正後のクロスラインを表示(0:しない, 1:する)
            'gstCLC.LineX = Picture2                         ' クロスラインＸ(縦線を指定する)
            'gstCLC.LineY = Picture1                         ' クロスラインＹ(横線を指定する)
            'gstCLC.LineX2 = CrosLineX                       ' 補正後クロスラインＸ(補正後の表示用縦線を指定する)
            'gstCLC.LineY2 = CrosLineY                       ' 補正後クロスラインＹ(補正後の表示用横線を指定する)
            ''                                               ' クロスライン補正値テーブル
            ''                                               ' ※SysPrmﾘｰﾄﾞ時二次元配列は一次元配列になるので一次元配列で処理する
            'gstCLC.CorrectTBL = New Double(2 * 17 - 1) { _
            '    gSysPrm.stCRL.gfCrossLineCorrect(0), gSysPrm.stCRL.gfCrossLineCorrect(2), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(4), gSysPrm.stCRL.gfCrossLineCorrect(6), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(8), gSysPrm.stCRL.gfCrossLineCorrect(10), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(12), gSysPrm.stCRL.gfCrossLineCorrect(14), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(16), gSysPrm.stCRL.gfCrossLineCorrect(18), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(20), gSysPrm.stCRL.gfCrossLineCorrect(22), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(24), gSysPrm.stCRL.gfCrossLineCorrect(26), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(28), gSysPrm.stCRL.gfCrossLineCorrect(30), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(32), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(1), gSysPrm.stCRL.gfCrossLineCorrect(3), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(5), gSysPrm.stCRL.gfCrossLineCorrect(7), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(9), gSysPrm.stCRL.gfCrossLineCorrect(11), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(13), gSysPrm.stCRL.gfCrossLineCorrect(15), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(17), gSysPrm.stCRL.gfCrossLineCorrect(19), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(21), gSysPrm.stCRL.gfCrossLineCorrect(23), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(25), gSysPrm.stCRL.gfCrossLineCorrect(27), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(29), gSysPrm.stCRL.gfCrossLineCorrect(31), _
            '    gSysPrm.stCRL.gfCrossLineCorrect(33)}
            '----- ###232 ----

            '----------------------------------------------------------------------------
            '   画面表示項目を設定する
            '----------------------------------------------------------------------------
            Me.Text = gAppName                              ' ｱﾌﾟﾘｹｰｼｮﾝ名表示 
            '----- V6.1.4.0⑦↓(KOA EW殿SL432RD対応) -----
            ' トリミングデータ名表示域クリア
            ' LblDataFileName.Text = ""                      ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名初期化 
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = ""                    ' トリミングデータ名表示域と第１抵抗データ表示域クリア  
            Else
                LblDataFileName.Text = ""                   ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名初期化 
            End If
            '----- V6.1.4.0⑦↑ -----
            Call Z_CLS()                                    ' ログ画面クリア 
            '                                               ' ログ画面のﾌｫﾝﾄｻｲｽﾞをｼｽﾊﾟﾗより設定
            txtLog.Font = New Font(txtLog.Font.FontFamily, CSng(gSysPrm.stLOG.gdLogTextFontSize))
            Call Form1LanguageSet()                         ' フォームのラベルを設定する
            'picGraphAccumulation.Visible = False            ' 分布図グラフ非表示

            ' クロスライン表示
            'V6.0.0.0④            Call SetCrossLine() Form1_Load()内へ移動

            ' ﾛｷﾞﾝｸﾞ状態(Logging ON/OFF)表示
            Call LoggingModeDisp()

            ' レーザパワー設定値表示 (RMCTRL2 >=3 の場合に表示する) ###029
            ' ※ﾌﾟﾛｸﾞﾗﾑ起動時はﾚｰｻﾞｰﾊﾟﾜｰ設定値は「-----」表示とする
            If (gSysPrm.stRMC.giRmCtrl2 >= 3) And (gSysPrm.stRMC.giPMonHi = 1) Then
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    strMSG = "レーザパワー設定値　---- W"
                'Else
                '    strMSG = "Laser Power ---- W"
                'End If
                LblMes.Text = Form1_001                     ' 測定パワー[W]の表示
                LblMes.Visible = True                       ' 設定値表示
            Else
                LblMes.Visible = False                      ' 設定値非表示
            End If

            ' レーザパワー設定値表示(FL時)
            ' ※FLでパワーメータのデータ取得タイプが「Ｉ／Ｏ読取り」/「ＵＳＢ」で「パワー測定値表示あり」の場合に表示する
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And
               (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    strMSG = "レーザパワー設定値　---- W"
                'Else
                '    strMSG = "Laser Power ---- W"
                'End If
                LblMes.Text = Form1_001                     ' 測定パワー[W]の表示
                'LblMes.Visible = True                      ' 設定値表示
                LblMes.Visible = False                      ' 設定値非表示 ###066
            Else
                LblMes.Visible = False                      ' 設定値非表示
            End If

            ' 生産管理情報初期化
            Call SetTrimResultCmnStr()                      ' ﾄﾘﾐﾝｸﾞ結果ﾛｸﾞ共通表示部分の初期化
            Call ClearCounter(0)                            ' 生産管理データのクリア(画面表示なし)

            '----- ###243↓ -----
            ' 生産管理画面のRESISTOR表示をTKY/NET時はCIRCUITに変更
            If (gTkyKnd = KND_CHIP) Then
                LblcREGNUM.Text = "RESISTOR="
            Else
                LblcREGNUM.Text = "CIRCUIT="
            End If
            '----- ###243↑ -----

            ' インターロック状態の表示/非表示
            Call DispInterLockSts()

            ' Digi-SW 表示(無条件に表示する)
            '#If cOFFLINEcDEBUG = 1 Then
            LblDIGSW_HI.Visible = True
            LblDIGSW.Visible = True                         ' "DSW="　表示
            CbDigSwH.Visible = True
            CbDigSwL.Visible = True
            CbDigSwH.SelectedIndex = 2                      ' Digi-SW = 33
            CbDigSwL.SelectedIndex = 3
            '#End If

            BtnADJ.Text = "ADJ ON"                          '###009
            BtnADJ.BackColor = System.Drawing.Color.Yellow  '###009
            gbChkboxHalt = True                             '###009

            '-----------------------------------------------------------------------
            '   オプションボタンの表示/非表示
            '-----------------------------------------------------------------------
            ' 「ESﾛｸﾞ」ﾎﾞﾀﾝ表示/非表示
            If gSysPrm.stLOG.gEsLogUse = 1 Then             ' ESログ使用可 ?
                cmdEsLog.Visible = True                     ' ESﾛｸﾞﾎﾞﾀﾝ表示
            Else
                cmdEsLog.Visible = False                    ' ESﾛｸﾞﾎﾞﾀﾝ非表示
            End If
            cmdEsLog.Text = "ESLog OFF"
            cmdEsLog.BackColor = System.Drawing.SystemColors.Control

            ' 「IX2Log ON」/「IX2Log OFF」ﾎﾞﾀﾝ表示/非表示
            'If gSysPrm.stCTM.giGP_IB_flg <> 0 Then          ' GP-IB制御機能あり ?  '###231
            ' GP-IB制御機能ありでIX2有効 ?                                          '###231
            'If (gSysPrm.stCTM.giGP_IB_flg <> 0) And (giIX2LOG = 1) Then             V1.18.0.0⑥
            If (giIX2LOG = 1) Then                          ' IX2ログが有効ならGPIBに関係なく有効とする V1.18.0.0⑥
                CMdIX2Log.Visible = True                    '「IX2Log On/OFF」ﾎﾞﾀﾝ表示
                If gSysPrm.stDEV.rIX2Log_flg = False Then   ' IX2LOG OFF ?
                    CMdIX2Log.Text = "IX2Log OFF"           '「IX2Log OFF」ﾎﾞﾀﾝ表示 
                    CMdIX2Log.BackColor = System.Drawing.SystemColors.Control
                Else                                        ' IX2LOG ON
                    CMdIX2Log.Text = "IX2Log ON"            '「IX2Log ON」ﾎﾞﾀﾝ表示 
                    CMdIX2Log.BackColor = System.Drawing.Color.Lime
                End If
            End If

            '----- V1.18.0.4①↓ -----
            ' Power Ctrl ON/OFFボタンを1(表示する), 0(表示しない) ローム殿特注
            If (giBtnPwrCtrl = 0) Then
                BtnPowerOnOff.Visible = False
            Else
                BtnPowerOnOff.Visible = True
            End If
            '----- V1.18.0.4①↑ -----

            '----- V6.1.1.0④↓ -----
            ' Alarm ON/OFFボタンを1(表示する), 0(表示しない) IAM殿特注(SL436R時)
            Me.System1.giAlarmBuzzer = 1                    ' アラーム音を鳴らす(既定値を設定する)
            If (giAlarmOnOff = 0) Then
                BtnAlarmOnOff.Visible = False
            Else
                BtnAlarmOnOff.Visible = True
            End If
            If (gMachineType <> MACHINE_TYPE_436R) Then     ' SL436R ?
                BtnAlarmOnOff.Visible = False
            End If
            '----- V6.1.1.0④↑ -----

            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号コンボボックスを初期化する
            Call Init_StartBlkComb()
            GrpStartBlk.Enabled = False                     ' データロード済でないので非活性化 
            ' 表示位置を設定する
            '            GrpStartBlk.Location = New System.Drawing.Point(BtnPowerOnOff.Location.X + 132, BtnPowerOnOff.Location.Y + BtnPowerOnOff.Height + 18)
            'V5.0.0.9⑯            GrpStartBlk.Location = New System.Drawing.Point(BtnPowerOnOff.Location.X + 120, BtnPowerOnOff.Location.Y + BtnPowerOnOff.Height + 18)
            ' トリミング開始ブロック番号の表示/非表示を設定する
            If (giStartBlkAss = 0) Then                     ' トリミング開始ブロック番号指定の有効/無効(0=無効, 1=有効)　
                GrpStartBlk.Visible = False
            Else
                GrpStartBlk.Visible = True

                'V5.0.0.9⑯              ↓
                If (1 < giStartBlkAss) Then
                    chkContinue.Visible = True              ' 継続チェックボックス表示状態設定
                    lblStartBlkX.Visible = True             ' X
                    lblStartBlkY.Visible = True             ' Y
                    CbStartBlkY.Visible = True              ' コンボボックスY

                    If (KEY_TYPE_RS = gKeiTyp) Then
                        btnPREV.UseVisualStyleBackColor = False
                        btnPREV.BackColor = Color.FromArgb(192, 255, 255)
                        btnPREV.Text = "Prev"

                        btnNEXT.UseVisualStyleBackColor = False
                        btnNEXT.BackColor = Color.FromArgb(255, 255, 128)
                        btnNEXT.Text = "Next"
                    Else
                        btnPREV.Enabled = False
                        btnPREV.Visible = False
                        btnNEXT.Enabled = False
                        btnNEXT.Visible = False
                    End If

                    With GrpStartBlk
                        .Location = New Point(
                            (LblCur.Left - .Margin.Right - (.Width - btnPREV.Width - btnNEXT.Width)),
                            0)
                    End With
                End If
                'V5.0.0.9⑯              ↑
            End If
            '----- V4.11.0.0⑤↑ -----

            PanelMap.Enabled = _mapDisp                                 ' V6.0.1.0⑫
            PanelMap.Visible = _mapDisp                                 ' MAP ON/OFFボタンを設定する    '#4.12.2.0①
            CmdPrintMap.Visible = Not _PrintDisp                        ' PRINT ON/OFFボタン V6.1.1.0②
            'V6.0.1.0⑫          ↓
            If (_mapDisp) Then
                PanelMap.Location = New Point(txtLog.Left, txtLog.Top - PanelMap.Height)
            Else
                CmdPrintMap.Enabled = False
                CmdMapOnOff.Enabled = False
            End If
            'V6.0.1.0⑫          ↑
            SetBtnUserLogon()           ' ログオンユーザー表示・切替ボタンを設定する     V4.10.0.0①

            '---------------------------------------------------------------------------
            '   起動後の最初の検出がﾛｰﾀﾞ自動ﾓｰﾄﾞ/動作中の場合は、停止に切替えるよう確認する(SL432R系)
            '---------------------------------------------------------------------------
            ''----- V6.1.4.0_45↓(KOA EW殿SL432RD対応)【ロット切替え機能】-----
            'If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ?
            '    If (giLotChange = 1) Then                               ' ロット切替え機能有効 ?
            '        Call Me.System1.Z_ATLDSET(0, &HFFFF)                ' 全てOFFとする
            '    End If
            'End If
            ''----- V6.1.4.0_45↑ -----

            ' ローダ入力
            giHostMode = cHOSTcMODEcMANUAL                  ' ﾛｰﾀﾞﾓｰﾄﾞ = 手動ﾓｰﾄﾞ
            gbHostConnected = False                         ' ホスト接続状態 = 未接続(ﾛｰﾀﾞ無)
            giHostRun = 0                                   ' ﾛｰﾀﾞ停止中
            Call Me.System1.ReadHostCommand(gSysPrm, giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag)

            ' 起動時ﾛｰﾀﾞ自動ﾓｰﾄﾞ/動作中ﾁｪｯｸ(SL432R系)
            r = Me.System1.Form_Reset(cGMODE_LDR_CHK, gSysPrm, giAppMode, gbInitialized, 0, 0, gLoadDTFlag)
            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then                             ' エラー
                strMSG = "i-TKY.Form_Initialize()::Form_Reset(cGMODE_LDR_CHK) error." + vbCrLf _
                        + " Err Code = " + r.ToString
                MsgBox(strMSG)
                End                                         ' アプリ終了
            ElseIf (r = cFRS_ERR_RST) Then
                'アプリケーション終了
                End
            End If

            'ロギング出力対象データの取得
            '   ログファイル出力
            Call TrimLogging_SetLogTarget(m_TrimlogFileFormat, cTRIMLOGcSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_MeaslogFileFormat, cMEASLOGcSECTNAME, Me.Utility1)
            '   ログ表示対象
            Call TrimLogging_SetLogTarget(m_TrimDisp1xFormat, cTRIMDISP1xcSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_TrimDispx0Format, cTRIMDISPx0cSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_TrimDispx1Format, cTRIMDISPx1cSECTNAME, Me.Utility1)
            Call TrimLogging_SetLogTarget(m_MeasDispFormat, cMEASDISPcSECTNAME, Me.Utility1)
            Call ClearAvgDevCount()                     ' ###154 

            ' 画面表示ログ文字数制限       '#4.12.2.0④
            If (False = Integer.TryParse(GetPrivateProfileString_S(
                                         "SPECIALFUNCTION", "DISP_CLS_LEN", TKY_INI, "5900000"),
                                         _txtLogClearLength)) Then
                _txtLogClearLength = 5900000                            ' .NET内部文字コードUTF16で11.25MB程度
            End If
            '----- V6.1.4.0③↓(KOA EW殿SL432RD対応) -----
            ' ログ表示域のサイズと位置を変更
            If (gSysPrm.stLOG.giLoggingType2 = LogType2.Reg_KoaEw) Then
                Me.txtLog.Size = New System.Drawing.Size(TXTLOG_SIZEX_KOAEW, TXTLOG_SIZEY_KOAEW)
                Me.txtLog.Location = New Point(TXTLOG_LOCATIONX_KOAEW, TXTLOG_LOCATIONY_KOAEW)
                Me.GrpMode.Location = New Point(GRPMODE_LOCATIONX_KOAEW, GRPMODE_LOCATIONY_KOAEW)
                Me.tabCmd.Location = New Point(TABCMD_LOCATIONX_KOAEW, TABCMD_LOCATIONY_KOAEW)
                Me.CmdEnd.Location = New Point(CMDEND_LOCATIONX_KOAEW, CMDEND_LOCATIONY_KOAEW)
            End If
            '----- V6.1.4.0③↑ -----
            'V6.1.4.0_50↓
            ' ENTRYLOT ﾌｫﾙﾀﾞ内のﾌｧｲﾙをすべて削除する
            If System.IO.Directory.Exists(ENTRY_PATH) Then
                For Each tmpFile As String In (System.IO.Directory.GetFiles(ENTRY_PATH))
                    IO.File.Delete(tmpFile)
                Next
            End If
            'V6.1.4.0_50↑
            ''---------------------------------------------------------------------------
            ''装置初期化処理()
            ''---------------------------------------------------------------------------
            'Call Me.Initialize_VideoLib()
            'Call Me.Initialize_TrimMachine()
            'V5.0.0.9③      ↓
            If (KEY_TYPE_R = gSysPrm.stTMN.giKeiTyp) Then
                ' クランプレス載物台(0でない場合クランプレス)
                giClampLessStage = Integer.Parse(GetPrivateProfileString_S(
                                                 "DEVICE_CONST", "CLAMP_LESS_STAGE", SYSPARAMPATH, "0"))

                giClampLessOutPos = Integer.Parse(GetPrivateProfileString_S(
                                                 "DEVICE_CONST", "CLAMP_LESS_OUTPOS", SYSPARAMPATH, "0"))


                If (0 <> gSysPrm.stDEV.giTheta) AndAlso (0 <> giClampLessStage) Then
                    ' クランプレス載物台の場合、クランプなしとする
                    'V6.0.5.0③  gSysPrm.stIOC.giClamp = 0S

                    'V5.0.0.9⑥          ↓
                    gdClampLessOffsetX = Double.Parse(GetPrivateProfileString_S(
                                                      "DEVICE_CONST", "CLAMP_LESS_OFFSET_X", SYSPARAMPATH, "0.0"))

                    gdClampLessOffsetY = Double.Parse(GetPrivateProfileString_S(
                                                      "DEVICE_CONST", "CLAMP_LESS_OFFSET_Y", SYSPARAMPATH, "0.0"))

                    giClampLessRoughCount = Integer.Parse(GetPrivateProfileString_S(
                                                          "DEVICE_CONST", "CLAMP_LESS_ROUGH_COUNT", SYSPARAMPATH, "0"))
                    ''V6.0.2.0⑥↓
                    'gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
                    '                                "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))   'V5.0.0.9⑨
                    ''V6.0.2.0⑥↑
                Else
                    gdClampLessOffsetX = 0.0
                    gdClampLessOffsetY = 0.0
                    giClampLessRoughCount = 0
                    gdClampLessTheta = 0.0      'V5.0.0.9⑨
                    'V5.0.0.9⑥          ↑
                End If

                'V4.12.2.0⑥         ↓'V6.0.4.1①
                If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then
                    If (0 <> giClampLessStage) Then
                        W_Write(LOFS_W192, LPC_CLAMP_EXEC)
                    Else
                        W_Write(LOFS_W192, 0)
                    End If
                End If
                'V4.12.2.0⑥         ↑'V6.0.4.1①

                'V6.0.2.0⑥↓
                gdClampLessTheta = Double.Parse(GetPrivateProfileString_S(
                                                    "DEVICE_CONST", "CLAMP_LESS_THETA", SYSPARAMPATH, "0.0"))
                'V6.0.2.0⑥↑

            End If
            'V5.0.0.9③                  ↑



            '' V3.0.0.0③ MV10のボードのビデオ端子を変更可能とする。↓               'V6.0.0.0⑨ カメラ初期化の後に移動
            'Dim TkyIni As New TkyIni()
            'INTERNAL_CAMERA = TkyIni.OPT_VIDEO.INTERNAL_CAMERA_PORT.Get(Of Integer)()
            'EXTERNAL_CAMERA = TkyIni.OPT_VIDEO.EXTERNAL_CAMERA_PORT.Get(Of Integer)()
            'If EXTERNAL_CAMERA = 0 Then
            '    INTERNAL_CAMERA = 0
            '    EXTERNAL_CAMERA = 1
            'End If
            '' V3.0.0.0③ MV10のボードのビデオ端子を変更可能とする。↑

            ' V6.1.4.0⑩ ロット切替え用自動運転フォームオブジェクト生成(File_Read()で使用するので標準版でも生成する)
            frmAutoObj = New FormDataSelect2(Me)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Form_Initialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "SL436R用のシステムパラメータをリードする"
    '''=========================================================================
    '''<summary>SL436R用のシステムパラメータをリードする</summary>
    '''<remarks>配列を使用している構造体のインスタンスを初期化するには"Initialize"を呼び出さなければならない</remarks>
    '''=========================================================================
    Private Sub GetSL436RSysparam()

        Dim i As Integer
        Dim strKEY As String
        Dim strSEC As String
        Dim strMSG As String

        Try
            ' SL436R用シスパラをリードする
            giActMode = Val(GetPrivateProfileString_S("DEVICE_CONST", "AUTOMODE", SYSPARAMPATH, "2"))                     ' 連続運転動作モード(0:ﾏｶﾞｼﾞﾝﾓｰﾄﾞ 1:ﾛｯﾄﾓｰﾄﾞ 2:ｴﾝﾄﾞﾚｽﾓｰﾄﾞ)
            giOPLDTimeOutFlg = Val(GetPrivateProfileString_S("DEVICE_CONST", "LOADER_TIMEOUT_CHECK", SYSPARAMPATH, "1"))  ' ローダ通信タイムアウト検出(0=検出無し, 1=検出あり)
            giOPLDTimeOut = Val(GetPrivateProfileString_S("DEVICE_CONST", "LOADER_TIMEOUT", SYSPARAMPATH, "180000"))      ' ローダ通信タイムアウト時間(msec)
            giOPVacFlg = Val(GetPrivateProfileString_S("DEVICE_CONST", "VACUME_CHECK", SYSPARAMPATH, "0"))                ' 手動モード時の載物台吸着アラーム検出(0=検出無し, 1=検出あり)
            giOPVacTimeOut = Val(GetPrivateProfileString_S("DEVICE_CONST", "VACUME_TIMEOUT", SYSPARAMPATH, "3000"))       ' 手動モード時の載物台吸着アラームタイムアウト時間(msec)

            strKEY = "LOADER_SPEED"
            giLoaderSpeed = Val(GetPrivateProfileString_S("LOADER", strKEY, SYSPARAMPATH, "0"))                           ' ローダ搬送速度
            strKEY = "LOADER_SETTING_NO"
            giLoaderPositionSetting = Val(GetPrivateProfileString_S("LOADER", strKEY, SYSPARAMPATH, "0"))                 ' ローダ位置設定選択番号

            File.ConvertFileEncoding(LOADER_PARAMPATH)  ' LOADER.INIの文字ｺｰﾄﾞをShift_JISからUnicode(UTF16LE BOM有)に変換する 'V4.4.0.0-1
            'V5.0.0.4①↓
            If Val(GetPrivateProfileString_S("SPECIALFUNCTION", "CYCLE_STOP", SYSPARAMPATH, "0")) = 1 Then
                gbCycleStop = True
                Me.btnCycleStop.Enabled = True
                Me.btnCycleStop.Visible = True
            Else
                gbCycleStop = False
                Me.btnCycleStop.Enabled = False
                Me.btnCycleStop.Visible = False
            End If
            'V5.0.0.4①↑

            ' ローダ基板テーブル排出位置XY/供給位置XYをTKT.iniでなくLOADER.iniから設定する ###069
            strSEC = "SYSTEM"
            For i = 0 To (MAXWORK_KND - 1)
                strKEY = "EJECTPOS" + (i + 1).ToString("0") + "X"
                gfBordTableOutPosX(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))          ' ローダ基板テーブル排出位置X
                strKEY = "EJECTPOS" + (i + 1).ToString("0") + "Y"
                gfBordTableOutPosY(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))          ' ローダ基板テーブル排出位置Y

                strKEY = "INSERTPOS" + (i + 1).ToString("0") + "X"
                gfBordTableInPosX(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))           ' ローダ基板テーブル供給位置X
                strKEY = "INSERTPOS" + (i + 1).ToString("0") + "Y"
                gfBordTableInPosY(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))           ' ローダ基板テーブル供給位置Y
                '----- V4.0.0.0-26↓ -----
                strKEY = "S_THICKNESS_" + (i + 1).ToString("00") 'V4.0.0.0-59
                gfTwoSubPickChkPos(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0.0000"))          ' 二枚取りセンサ確認位置座標(pls)
                strKEY = "S_THINFILM_" + (i + 1).ToString("00")
                glSubstrateType(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "0"))                  ' 薄基板対応(0=通常, 1=薄基板(スルーホール))
                '----- V4.0.0.0-26↑ -----
            Next

            '  NG排出BOXの収納枚数(基板品種分)をLOADER.iniから設定する ###089
            strSEC = "SYSTEM"
            For i = 0 To (MAXWORK_KND - 1)
                strKEY = "NGBOX_COUNT_" + (i + 1).ToString("00")
                giNgBoxCount(i) = Val(GetPrivateProfileString_S(strSEC, strKEY, LOADER_PARAMPATH, "10"))
            Next

            '----- V4.0.0.0⑥↓ -----
            '-------------------------------------------------------------------------------
            '   SL436RとSL436Sの電磁ロック等のIOアドレスの統合(ローム殿対応)
            '   ＥＸＴＢＩＴ(216A BIT4-7) ※ローム殿特注(SL436R/SL436S)
            '-------------------------------------------------------------------------------
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ? 
                '-----【SL436Sの場合】※V4.0.0.0-52(SL436Rと同じ)-----
                ' 出力ビット
                EXTOUT_EX_YLW_ON = &H10                                 ' B4 : 黄点灯(レーザ照射中)
                EXTOUT_EX_LOK_ON = &H20                                 ' B5 : 電磁ロック(観音扉右側ロック)

                ' 入力ビット
                EXTINP_EX_LOK_ON = &H10                                 ' B4 : 電磁ロック(観音扉右側ロック)

                ' その他
                EX_LOK_STS = &H216A                                     ' 電磁ロックステータスアドレス(SL436Rの場合)

            Else
                '-----【SL436Rの場合】-----
                ' 出力ビット
                EXTOUT_EX_YLW_ON = &H10                                 ' B4 : 黄点灯(レーザ照射中)
                EXTOUT_EX_LOK_ON = &H20                                 ' B5 : 電磁ロック(観音扉右側ロック)

                ' 入力ビット
                EXTINP_EX_LOK_ON = &H10                                 ' B4 : 電磁ロック(観音扉右側ロック)

                ' その他
                EX_LOK_STS = &H216A                                     ' 電磁ロックステータスアドレス(SL436Rの場合)
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.GetSL436RSysparam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

#End Region
    '----- V1.14.0.0②↓ -----
#Region "FL用パワー調整情報ファイルをリードする"
    '''=========================================================================
    ''' <summary>FL用パワー調整情報ファイルをリードする</summary>
    ''' <param name="sPath">(INP)ファイルパス名</param>
    ''' <param name="stPWR">(OUT)FL用パワー調整情報</param>
    ''' <param name="Md">   (INP)0=初期化, 0以外=リード</param>
    '''=========================================================================
    Private Sub GetFlAttInfoData(ByRef sPath As String, ByRef stPWR As POWER_ADJUST_INFO, ByVal Md As Integer)

        Dim Idx As Integer
        Dim strKEY As String
        Dim strSEC As String
        Dim strMSG As String

        Try
            ' FL用パワー調整情報を初期化または設定する
            If (Md = 0) Then

                ' FL用パワー調整情報を初期化する
                For Idx = 0 To MAX_BANK_NUM - 1
                    stPWR.AttInfoAry(Idx) = 0                           ' 固定ATT情報配列を初期化する(0(固定ATT Off))
                    stPWR.CndNumAry(Idx) = 0                            ' 加工条件番号配列を初期化する(0=無効, 1=有効) 
                    stPWR.AdjustTargetAry(Idx) = 0.0                    ' 目標パワー配列を初期化する(0.000W)
                    stPWR.AdjustLevelAry(Idx) = 0.0                     ' 許容範囲配列を初期化する(0.000W)
                Next Idx

            Else
                ' 固定ATT情報(0:固定ATT Off, 1:固定ATT On)をリードする
                strSEC = "FIXATT"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.AttInfoAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0"))
                Next Idx

                ' 加工条件番号配列(0=無効, 1=有効)をリードする
                strSEC = "COND_NUM"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.CndNumAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0"))
                Next Idx

                ' 目標パワー配列(W)をリードする
                strSEC = "TARGET"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.AdjustTargetAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0.000"))
                Next Idx

                ' 許容範囲配列(＋－W)をリードする
                strSEC = "HILO"
                For Idx = 0 To MAX_BANK_NUM - 1
                    strKEY = "NUM_" + Idx.ToString("000")
                    stPWR.AdjustLevelAry(Idx) = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S(strSEC, strKEY, sPath, "0.000"))
                Next Idx

            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.GetFlAttInfoData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "FL用パワー調整情報ファイルをライトする"
    '''=========================================================================
    ''' <summary>FL用パワー調整情報ファイルをライトする</summary>
    ''' <param name="sPath">(INP)ファイルパス名</param>
    ''' <param name="stPWR">(OUT)パワー調整情報</param>
    '''=========================================================================
    Private Sub PutFlAttInfoData(ByRef sPath As String, ByRef stPWR As POWER_ADJUST_INFO)

        Dim Idx As Integer
        Dim strKEY As String
        Dim strSEC As String
        Dim strDAT As String
        Dim strMSG As String

        Try
            ' 固定ATT情報(0:固定ATT Off, 1:固定ATT On)をライトする
            strDAT = sPath
            strSEC = "FIXATT"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.AttInfoAry(Idx).ToString("0")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' 加工条件番号配列(0=無効, 1=有効)をライトする
            strSEC = "COND_NUM"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.CndNumAry(Idx).ToString("0")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' 目標パワー配列(W)をライトする
            strSEC = "TARGET"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.AdjustTargetAry(Idx).ToString("0.000")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' 許容範囲配列(＋－W)をライトする
            strSEC = "HILO"
            For Idx = 0 To MAX_BANK_NUM - 1
                strKEY = "NUM_" + Idx.ToString("000")
                strMSG = stPWR.AdjustLevelAry(Idx).ToString("0.000")
                Call WritePrivateProfileString(strSEC, strKEY, strMSG, sPath)
                sPath = strDAT
            Next Idx

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.PutFlAttInfoData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.14.0.0②↑ -----
#Region "構造体の初期化"
    '''=========================================================================
    '''<summary>構造体の初期化</summary>
    '''<remarks>配列を使用している構造体のインスタンスを初期化するには"Initialize"を呼び出さなければならない</remarks>
    '''=========================================================================
    Private Sub Init_Struct()

        'V5.0.0.8①        Dim i As Integer
        Dim strMSG As String

        Try
            'V5.0.0.8①                  ↓
            '' トリミングデータ構造体の初期化
            'For i = 0 To MaxCntResist
            '    typResistorInfoArray(i).Initialize()    ' ｶｯﾄﾃﾞｰﾀ
            '    markResistorInfoArray(i).Initialize()   ' ｶｯﾄﾃﾞｰﾀ(NG Marking) 
            'Next i
            'V5.0.0.8①                  ↑

            ' トリミングデータの最小値/最大値チェック用構造体の初期化
            typSPInputArea.Initialize()                 ' STEP  (0)Min/(1)Max
            typGPInputArea.Initialize()                 ' GROUP (0)Min/(1)Max
            typTy2InputArea.Initialize()                ' Ty2   (0)Min/(1)Max
            typRPInputArea.Initialize()                 ' RES.  (0)Min/(1)Max
            typCPInputArea.Initialize()                 ' CUT   (0)Min/(1)Max

            ' パワー調整情報構造体(FL用)初期化 
            stCND.Initialize()
            stPWR_LSR.Initialize()                      ' FL用パワー調整情報(レーザコマンド用) V1.14.0.0② 

            ' トリマー加工条件構造体(FL用)初期化 ###066
            stPWR.Initialize()

            ' カット位置補正用構造体初期化
            gStCutPosCorrData.Initialize()

            '----- V1.22.0.0④↓ -----
            ' サマリーロギング用データの初期化
            stSummaryLog.Initialize()
            '----- V1.22.0.0④↑ -----

            ' カットデータ送受信用データ
            stTGPI.Initialize()                         ' ###002 
            stTGPI2.Initialize()                        ' ###229
            stTCUT.Initialize()                         ' カットデータ構造体初期化
            stCutMK.Initialize()

            gSysPrm.Initialize()

            '----- ###188↓ -----
            ' 軸サブステータスアドレス設定 -----
            AxisSubStsAry(AXIS_X) = ADRSUB_STS_X
            AxisSubStsAry(AXIS_Y) = ADRSUB_STS_Y
            AxisSubStsAry(AXIS_Z) = ADRSUB_STS_Z
            AxisSubStsAry(AXIS_T) = ADRSUB_STS_T
            '----- ###188↑ -----

            '----- V4.0.0.0-58↓ -----
            ' トリマー加工条件構造体(ワーク) 
            gwkCND = Nothing
            gwkCND.Initialize()
            '----- V4.0.0.0-58↑ -----
            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Init_Struct() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "配列の初期化"
    '''=========================================================================
    '''<summary>配列の初期化</summary>
    '''<remarks>Vs2010からVB6.xxxAryは使用できない</remarks>
    '''=========================================================================
    Private Sub Init_Arrey()

        Dim strMSG As String

        Try
            ' 生産管理情報用ラベル配列の初期化
            Fram1LblAry(FRAM1_ARY_GO) = Me.LblGO
            Fram1LblAry(FRAM1_ARY_NG) = Me.LblTNG
            Fram1LblAry(FRAM1_ARY_NGPER) = Me.LblNGPER
            Fram1LblAry(FRAM1_ARY_PLTNUM) = Me.LblPLTNUM
            Fram1LblAry(FRAM1_ARY_REGNUM) = Me.LblREGNUM
            Fram1LblAry(FRAM1_ARY_ITHING) = Me.LblITHING
            Fram1LblAry(FRAM1_ARY_FTHING) = Me.LblFTHING
            Fram1LblAry(FRAM1_ARY_ITLONG) = Me.LblITLONG
            Fram1LblAry(FRAM1_ARY_FTLONG) = Me.LblFTLONG
            Fram1LblAry(FRAM1_ARY_OVER) = Me.LblOVER
            Fram1LblAry(FRAM1_ARY_ITHINGP) = Me.LblITHINGP
            Fram1LblAry(FRAM1_ARY_FTHINGP) = Me.LblFTHINGP
            Fram1LblAry(FRAM1_ARY_ITLONGP) = Me.LblITLONGP
            Fram1LblAry(FRAM1_ARY_FTLONGP) = Me.LblFTLONGP
            Fram1LblAry(FRAM1_ARY_OVERP) = Me.LblOVERP


            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Init_Arrey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "ＯＣＸの初期設定"
    '''=========================================================================
    '''<summary>使用するＯＣＸの初期設定を行う</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Ocx_Initialize()

        Dim strMSG As String

        Try
            Dim r As Short
            Dim OptTbl(cMAXOptFlgNUM) As Short              ' ｺﾝﾊﾟｲﾙｵﾌﾟｼｮﾝ(最大数)

            '-------------------------------------------------------------------
            '   OCX(VB6)用オブジェクトを設定する
            '-------------------------------------------------------------------
            'ObjSys = System1                                ' OcxSystem.ocx
            'ObjUtl = Utility1                               ' OcxUtility.ocx
            '' '' ''ObjHlp = HelpVersion1                           ' (未使用)なぜ？OcxAbout.ocx
            'ObjPas = Password1                              ' OcxPassword.ocx
            '' '' ''ObjMTC = ManualTeach1                           ' (未使用)OcxManualTeach.ocx
            'ObjTch = Teaching1                              ' Teach.ocx
            'ObjPrb = Probe1                                 ' Probe.ocx
            'ObjVdo = VideoLibrary1                          ' Video.ocx
            'ObjPrt = Print1                                ' OcxPrint.ocx

            '-------------------------------------------------------------------
            '   OcxSystem.ocx用の初期設定処理を行う
            '-------------------------------------------------------------------
            ' OcxSystem用のオブジェクトを設定する
            Call Me.System1.SetOcxUtilityObject(Utility1)   ' OcxUtility1.ocx
            Call Me.System1.SetMainObject_EX()              ' Mainｵﾌﾞｼﾞｪｸﾄ(Dummy)
            Call Me.System1.SetSystemObject(System1)        ' System.ocx

            ' 親モジュールのメソッドを設定する(OcxSystem用) ' ###061
            gparModules = New MainModules()                 ' 親側メソッド呼出しオブジェクト
            Call System1.SetMainObject(gparModules)
            'V6.0.1.0⑥            System1.SetActiveJogMethod = AddressOf Me.SetActiveJogMethod        'V6.0.0.0⑭

            ' ｺﾝﾊﾟｲﾙｵﾌﾟｼｮﾝを設定する
#If cOFFLINEcDEBUG = 0 Then                                 ' ﾃﾞﾊﾞｯｸﾞﾓｰﾄﾞでない ?
            OptTbl(0) = 0                                       ' ﾃﾞﾊﾞｯｸﾞﾌﾗｸﾞOFF
            Call DebugMode(0, 0)                                ' DllTrimFunc.dllﾊﾞｯｸﾞﾌﾗｸﾞOFF
#Else
            OptTbl(0) = 1                                   ' ﾃﾞﾊﾞｯｸﾞﾌﾗｸﾞON
            Call DebugMode(1, 0)                            ' DllTrimFunc.dllﾊﾞｯｸﾞﾌﾗｸﾞON
#End If

#If cIOcMONITORcENABLED = 0 Then                            ' OcxSystem用 
            OptTbl(1) = 0                                   ' IOﾓﾆﾀ表示(0=表示しない, 1=表示する)
#Else
        OptTbl(1) = 1
#End If

            ' ｺﾝﾊﾟｲﾙｵﾌﾟｼｮﾝを設定しシステムパラメータをINtime側へ送信する
            r = Me.System1.SetOptionFlg(cMAXOptFlgNUM, OptTbl)
            If (r <> cFRS_NORMAL) Then
                strMSG = "System1.SetOptionFlg Error (r = " & r.ToString("0") & ")"
                Call MsgBox(strMSG, MsgBoxStyle.OkOnly)
                End
            End If

            '-------------------------------------------------------------------
            '   OcxAbout.ocx用の初期設定処理を行う
            '-------------------------------------------------------------------
            Call Me.HelpVersion1.SetOcxUtilityObject(Utility1) ' OcxUtility.ocx

            '-------------------------------------------------------------------
            '   OcxPassword.ocx用の初期設定処理を行う
            '-------------------------------------------------------------------
            Call Me.Password1.SetOcxUtilityObject(Utility1)    ' OcxUtility.ocx

            '-------------------------------------------------------------------
            '   OcxManualTeach.ocx用の初期設定処理を行う
            '-------------------------------------------------------------------
            Call Me.ManualTeach1.SetOcxUtilityObject(Utility1) ' OcxUtility1.ocx
            Call Me.ManualTeach1.SetSystemObject(System1)      ' System.ocx

            '-------------------------------------------------------------------
            '   DllSysprm.dll用の初期設定処理を行う   
            '-------------------------------------------------------------------
            Call gDllSysprmSysParam_definst.SetOcxUtilityObjectForSysprm(Utility1)

            '-------------------------------------------------------------------
            '   Teach.ocx用の初期設定処理を行う
            '-------------------------------------------------------------------
            Call Me.Teaching1.SetOcxUtilityObject(Utility1)     ' OcxUtility1.ocx
            Call Me.Teaching1.SetSystemObject(System1)          ' System.ocx
            'Call Me.Teaching1.SetCrossLineObject(gparModules)   ' クロスライン表示用 ###232

            '-------------------------------------------------------------------
            '   Probe.ocx用の初期設定処理を行う
            '-------------------------------------------------------------------
            Call Me.Probe1.SetOcxUtilityObject(Utility1)       ' OcxUtility1.ocx
            Call Me.Probe1.SetSystemObject(System1)            ' System.ocx

            '-------------------------------------------------------------------
            '   Video.ocx用の初期設定処理を行う
            '-------------------------------------------------------------------
            Call Me.VideoLibrary1.SetOcxUtilityObject(Utility1) ' OcxUtility1.ocx
            Call Me.VideoLibrary1.SetSystemObject(System1)      ' System.ocx

            '----- V4.3.0.0②↓ -----
            '-------------------------------------------------------------------
            '   クラスライブラリ等のオブジェクトを設定する
            '-------------------------------------------------------------------
            TrimData = New TrimClassLibrary.TrimData()

            '----- V4.3.0.0②↑ -----

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Ocx_Initialize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "機能選択定義テーブル設定"
    '''=========================================================================
    ''' <summary>機能選択定義テーブル設定</summary>
    ''' <param name="Knd">(INP)種別(0=TKY, 1=CHIP, 2=NET)</param>
    '''=========================================================================
    Private Sub GetFncDefParameter(ByVal Knd As Short)

        Dim strMSG As String

        Try
            Dim i As Short                                  ' Counter
            Dim sPath As String                             ' ﾌｧｲﾙ名
            Dim sSect As String                             ' ｾｸｼｮﾝ名
            Dim sSec2 As String                             ' ｾｸｼｮﾝ名

            ' ｷｰ名を設定する
            stFNC(F_LOAD).sCMD = "LOAD"                     ' LOADボタン
            stFNC(F_SAVE).sCMD = "SAVE"                     ' SAVEボタン
            stFNC(F_EDIT).sCMD = "EDIT"                     ' EDITボタン
            stFNC(F_LASER).sCMD = "LASER"                   ' LASERボタン
            stFNC(F_LOG).sCMD = "LOG"                       ' LOGGINGボタン
            stFNC(F_PROBE).sCMD = "PROBE"                   ' PROBEボタン
            stFNC(F_TEACH).sCMD = "TEACH"                   ' TEACHボタン
            stFNC(F_CUTPOS).sCMD = "CUTPOS"                 ' CUTPOS(ｶｯﾄ位置補正)ボタン
            stFNC(F_RECOG).sCMD = "RECOG"                   ' RECOG(画像登録)ボタン
            ' CHIP,NET系
            stFNC(F_TTHETA).sCMD = "TTHETA"                 ' Tθボタン 
            stFNC(F_TX).sCMD = "TX"                         ' TXボタン 
            stFNC(F_TY).sCMD = "TY"                         ' TYボタン 
            stFNC(F_TY2).sCMD = "TY2"                       ' TY2ボタン
            stFNC(F_EXR1).sCMD = "EXR1"                     ' 外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ 
            stFNC(F_EXTEACH).sCMD = "EXTEACH"               ' 外部ｶﾒﾗﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
            stFNC(F_CARREC).sCMD = "CARIBREC"               ' ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録ﾎﾞﾀﾝ
            stFNC(F_CAR).sCMD = "CARIB"                     ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾎﾞﾀﾝ 
            stFNC(F_CUTREC).sCMD = "CUTREC"                 ' ｶｯﾄ補正登録ﾎﾞﾀﾝ
            stFNC(F_CUTREV).sCMD = "CUTREV"                 ' ｶｯﾄ位置補正ﾎﾞﾀﾝ
            ' NET系
            stFNC(F_CIRCUIT).sCMD = "CIRCUIT"               ' ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
            ' SL436R CHIP,NET系
            stFNC(F_AUTO).sCMD = "AUTO"                     ' AUTOボタン 
            stFNC(F_LOADERINI).sCMD = "LOADERINI"           ' LOADER INITボタン 
            '----- V1.13.0.0③↓ -----
            ' TKY系オプション
            stFNC(F_APROBEREC).sCMD = "APROBEREC"           ' ｵｰﾄプローブ登録ﾎﾞﾀﾝ 
            stFNC(F_APROBEEXE).sCMD = "APROBEEXE"           ' ｵｰﾄプローブ実行ﾎﾞﾀﾝ
            stFNC(F_IDTEACH).sCMD = "IDTEACH"               ' IDﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
            stFNC(F_SINSYUKU).sCMD = "SINSYUKU"             ' 伸縮登録ﾎﾞﾀﾝ 
            stFNC(F_MAP).sCMD = "MAP"                       ' MAPボタン 
            '----- V1.13.0.0③↑ -----
            'V4.1.0.0⑤
            stFNC(F_PROBE_CLEANING).sCMD = "PROBECLEAN"      ' プローブクリーニングボタン
            'V4.1.0.0⑤
            'V4.1.0.0⑥
            stFNC(F_MAINTENANCE).sCMD = "IOMAINT"           ' IO確認ボタン
            'V4.1.0.0⑥

            stFNC(F_INTEGRATED).sCMD = "INTEGRATED"         ' 統合登録調整ボタン 'V4.10.0.0③
            stFNC(F_RECOG_ROUGH).sCMD = "RECOG_ROUGH"       ' ラフアライメント用画像登録ボタン  'V5.0.0.9④
            stFNC(F_FOLDEROPEN).sCMD = "FOLDEROPEN"         ' フォルダ表示ボタン(生産管理データ) V6.1.4.0⑥

            ' ファイル名/セクション名を設定する
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then        ' SL436R ? 
                If (Knd = KND_NET) Then
                    sPath = cDEF_FUNCNAME_NET_436           ' 定義ファイル名
                Else
                    sPath = cDEF_FUNCNAME_CHIP_436          ' 定義ファイル名
                End If
            Else
                If (Knd = KND_TKY) Then
                    sPath = cDEF_FUNCNAME_TKY               ' 定義ファイル名
                ElseIf (Knd = KND_CHIP) Then
                    sPath = cDEF_FUNCNAME_CHIP              ' 定義ファイル名
                Else
                    sPath = cDEF_FUNCNAME_NET               ' 定義ファイル名
                End If
            End If

            sSect = "FUNCDEF"
            sSec2 = "PASSWORD"

            ' 機能選択定義テーブルを設定する(-1:非表示, 0:選択不可, 1:選択可)
            For i = 0 To (MAX_FNCNO - 1)                    ' 定義数分繰返す
                stFNC(i).iDEF = GetPrivateProfileInt(sSect, stFNC(i).sCMD, -1, sPath)
                stFNC(i).iPAS = GetPrivateProfileInt(sSec2, stFNC(i).sCMD, 0, sPath)
            Next i

            ' ﾛｸﾞｲﾝﾊﾟｽﾜｰﾄﾞ入力の有無(0:無, 1:有) 
            flgLoginPWD = GetPrivateProfileInt("COMMON", "LOGINPWD", 0, sPath)

        Catch ex As Exception
            strMSG = "i-TKY.GetFncDefParameter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "生産管理データのクリア"
    '''=========================================================================
    '''<summary>生産管理データのクリア</summary>
    '''<param name="flag">(INP)0=画面表示なし, 1=画面表示あり</param> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub ClearCounter(ByVal flag As Short)

        Dim i As Short
        Dim strMSG As String

        Try

            ' 生産管理情報初期化
            Call ClearTrimResult()
            '' '' ''glPlateCount = 0                                        ' プレート処理数
            '' '' ''glGoodCount = 0                                         ' 良品抵抗数
            '' '' ''glNgCount = 0                                           ' 不良抵抗数
            '' '' ''glCircuitNgTotal = 0                                    ' 不良サーキット数
            '' '' ''glCircuitGoodTotal = 0                                  ' 良品サーキット数

            '----- V1.22.0.0④↓ -----
            ' サマリーロギング用データを初期化する
            Call SummaryLoggingDataInit(stSummaryLog)
            '----- V1.22.0.0④↑ -----

            If flag Then
                Fram1LblAry(FRAM1_ARY_GO).Text = "0"                    ' GO数(サーキット数 or 抵抗数)
                Fram1LblAry(FRAM1_ARY_NG).Text = "0"                    ' NG数(サーキット数 or 抵抗数)
                Fram1LblAry(FRAM1_ARY_NGPER).Text = " - "               ' NG%
                Fram1LblAry(FRAM1_ARY_PLTNUM).Text = "0"                ' PLATE数
                Fram1LblAry(FRAM1_ARY_REGNUM).Text = "0"                ' RESISTOR数
                Fram1LblAry(FRAM1_ARY_ITHING).Text = "0"                ' IT HI NG数
                Fram1LblAry(FRAM1_ARY_FTHING).Text = "0"                ' FT HI NG数
                Fram1LblAry(FRAM1_ARY_ITLONG).Text = "0"                ' IT LO NG数
                Fram1LblAry(FRAM1_ARY_FTLONG).Text = "0"                ' FT LO NG数
                Fram1LblAry(FRAM1_ARY_OVER).Text = "0"                  ' OVER数　'V3.0.0.0⑧(V1.22.0.0⑪)
                Fram1LblAry(FRAM1_ARY_ITHINGP).Text = " - "             ' IT HI NG%
                Fram1LblAry(FRAM1_ARY_FTHINGP).Text = " - "             ' FT HI NG%
                Fram1LblAry(FRAM1_ARY_ITLONGP).Text = " - "             ' IT LO NG%
                Fram1LblAry(FRAM1_ARY_FTLONGP).Text = " - "             ' FT LO NG%
                Fram1LblAry(FRAM1_ARY_OVERP).Text = " - "               ' OVER NG%
            End If

            For i = 0 To 11
                glRegistNum(i) = 0                                      ' 分布グラフ抵抗数
                glRegistNumIT(i) = 0                                    ' 分布グラフ抵抗数
                glRegistNumFT(i) = 0                                    ' 分布グラフ抵抗数
            Next

            lOkChip = 0                                                 ' OK数
            lNgChip = 0                                                 ' NG数
            dblMinIT = 0                                                ' 最小値
            dblMaxIT = 0                                                ' 最大値
            dblMinFT = 0                                                ' 最小値
            dblMaxFT = 0                                                ' 最大値
            dblAverage = 0                                              ' 平均値
            dblAverageIT = 0                                            ' 平均値   
            dblAverageFT = 0                                            ' 平均値
            HEIHOUIT = 0                                                '  ..
            HEIHOUFT = 0                                                '  ..
            dblDeviationIT = 0                                          ' 標準偏差
            dblDeviationFT = 0                                          ' 標準偏差
            '' '' ''glITHINGCount = 0
            '' '' ''glITLONGCount = 0
            '' '' ''glFTHINGCount = 0
            '' '' ''glFTLONGCount = 0
            '' '' ''glITOVERCount = 0
            '' '' ''dblGapIT = 0.0#
            '' '' ''dblGapFT = 0.0#
            '' '' ''gfX_2IT = 0.0#
            '' '' ''gfX_2FT = 0.0#
            glITTOTAL = 0                                               ' IT計算対象数 ###138
            glFTTOTAL = 0                                               ' FT計算対象数 ###138
            Call ClearAvgDevCount()                                     ' ###198

            gITNx_cnt = -1
            gITNg_cnt = -1
            gFTNx_cnt = -1
            gFTNg_cnt = -1
            Erase gITNx                                                 ' IT 測定誤差(個々)
            Erase gFTNx                                                 ' FT 測定誤差(個々)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.ClearCounter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form上デジタルスイッチの数値取得"
    '''=========================================================================
    '''<summary>デジタルスイッチの数値取得</summary>
    '''<param name="digL">(OUT)デジタルスイッチ動作モードの設定取得（下位一桁）</param> 
    '''<param name="digH">(OUT)デジタルスイッチ表示モードの設定取得（上位一桁）</param> 
    '''<param name="digSW">(OUT)デジタルスイッチの設定取得（上位下位）</param> 
    '''<remarks>デジタルスイッチの現在の設定を取得する</remarks>
    '''=========================================================================
    Public Sub GetMoveMode(ByRef digL As Short, ByRef digH As Short, ByRef digSW As Short)


        Try
            '現在値の設定
            digL = Me.CbDigSwL.SelectedIndex
            digH = Me.CbDigSwH.SelectedIndex
            digSW = digH * 10 + digL

            ' トラップエラー発生時 
        Catch ex As Exception
            Dim strMSG As String

            strMSG = "i-TKY.GetMoveMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form上デジタルスイッチの数値設定"
    '''=========================================================================
    '''<summary>デジタルスイッチの数値設定</summary>
    '''<param name="digL">(IN)デジタルスイッチ動作モードの設定取得（下位一桁）</param> 
    '''<param name="digH">(IN)デジタルスイッチ表示モードの設定取得（上位一桁）</param> 
    '''<remarks>デジタルスイッチの現在値を設定する</remarks>
    '''=========================================================================
    Public Sub SetMoveMode(ByVal digL As Short, ByVal digH As Short)


        Try
            '現在値の設定
            Me.CbDigSwL.SelectedIndex = digL
            Me.CbDigSwH.SelectedIndex = digH

            ' トラップエラー発生時 
        Catch ex As Exception
            Dim strMSG As String

            strMSG = "i-TKY.SetMoveMode() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "MV10 ビデオライブラリ初期化処理"
    '''=========================================================================
    '''<summary>MV10 ビデオライブラリ初期化処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Initialize_VideoLib()
        Dim strMSG As String
        Try

            Dim lRet As Integer
            'V6.0.0.0②            Dim r As Short
            Dim s As String
            '---------------------------------------------------------------------------
            '   ビデオライブラリを初期化する
            '---------------------------------------------------------------------------
            If (pbVideoCapture = False) Then                ' ビデオキャプチャー開始フラグOFF ?
                pbVideoCapture = True                       ' ビデオキャプチャー開始フラグON
                ChDir(WORK_DIR_PATH)                        ' MvcPt2.iniのあるﾌｫﾙﾀﾞｰを作業用ﾌｫﾙﾀﾞｰとする
                If (gSysPrm.stDEV.giEXCAM = 0) Then         ' 内部カメラ?
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfPixelX       ' ピクセル値X(um)
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfPixelY       ' ピクセル値Y(um)
                Else
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfEXCAM_PixelX ' 外部ｶﾒﾗﾋﾟｸｾﾙ値X(um)
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfEXCAM_PixelY ' 外部ｶﾒﾗﾋﾟｸｾﾙ値Y(um)
                End If

                VideoLibrary1.OverLay = True                ' ←↑不要 ?
                lRet = VideoLibrary1.Init_Library()         ' ビデオライブラリ初期化
                If (lRet <> 0) Then                         ' Video.OCXエラー ?
                    Select Case lRet
                        Case cFRS_VIDEO_INI
                            s = "VIDEOLIB: Already initialized."
                        Case cFRS_VIDEO_PRP
                            s = "VIDEOLIB: Invalid property value."
                        Case cFRS_MVC_UTL
                            s = "VIDEOLIB: Error in MvcUtil"
                        Case cFRS_MVC_PT2
                            s = "VIDEOLIB: Error in MvcPt2"
                        Case cFRS_MVC_10
                            s = "VIDEOLIB: Error in Mvc10"
                        Case Else
                            s = "VIDEOLIB: Unexpected error 2"
                    End Select
                    Call System1.TrmMsgBox(gSysPrm, s, MsgBoxStyle.OkOnly, My.Application.Info.Title)
                Else
                    ' "ライブラリ初期化完了"
                    pbVideoInit = True

                    ' V3.0.0.0③ MV10のボードのビデオ端子を変更可能とする。↓    'V6.0.0.0⑨ カメラ初期化の後に移動
                    ' デバッグ実行時は bin\Debug\TKY.exe が実行されるため、C:\TRIM\*.dll ではなく bin\Debug\*.dll が使用される
                    ' Baslerバージョン・MV10バージョンの入れ替えをした場合は、bin\Debug\DefMv10Fnc.dll も手動で入れ替えるか
                    ' TKYをリビルドして、入れ替えた C:\TRIM\DefMv10Fnc.dll を bin\Debug にコピーさせる必要がある
                    INTERNAL_CAMERA = VideoLibrary1.InternalCameraPort
                    EXTERNAL_CAMERA = VideoLibrary1.ExternalCameraPort
                    ' V3.0.0.0③ MV10のボードのビデオ端子を変更可能とする。↑

                    'V6.0.0.0-28                    'Videoを停止する
                    'V6.0.0.0-28                    VideoLibrary1.VideoStop()

                    ' OcxTeach用クロスライン補正用パラメータ設定
                    'V6.0.0.0②                r = Teaching1.SetCrossLineCorrectParam(VideoLibrary1.gPicture2, VideoLibrary1.gPicture1, _
                    'V6.0.0.0②                                                VideoLibrary1.gCrosLineXY2, VideoLibrary1.gCrosLineXY1)
                    'V6.0.0.0②                If (r <> cFRS_NORMAL) Then                          ' エラー ?
                    'V6.0.0.0②                    strMSG = "i-TKY.Form1_Activated() SetCrossLineCorrectParam ERROR = "
                    'V6.0.0.0②                    MsgBox(strMSG)
                    'V6.0.0.0②                End If

                    ' クロスライン表示
                    Call SetCrossLine()                                     ' Form_Initialize_Renamed()からここへ移動  'V6.0.0.0④  

                    'If (Debugger.IsAttached) Then
                    VideoLibrary1.SetCameraOptionContextMenu(True, True)    ' fps 表示コンテキストメニュー
                    'End If

                End If
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Form1_Activated() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "装置初期化処理-ビデオ取り込み開始、原点復帰"
    '''=========================================================================
    '''<summary>装置初期化処理-ビデオ取り込み開始、原点復帰</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Initialize_TrimMachine()

        Dim strMSG As String = ""

        Try

            Dim lRet As Long                                            'V1.18.0.0⑤
            Dim r As Short

            '---------------------------------------------------------------------------
            '   ビデオスタートと原点復帰処理とFLへの初期化ファイル送付
            '---------------------------------------------------------------------------
            If (gflgResetStart = False) Then                ' 初期設定済みでない ?

                ' ﾃﾝﾌﾟﾚｰﾄﾌｧｲﾙの保存場所を"C:\TRIM"に設定する(VideoStart()後に指定する)
                ' (注)管理ﾌｧｲﾙ「Pt2Template.xxx」は起動ﾌｫﾙﾀﾞに作成される。
                r = Me.VideoLibrary1.SetTemplatePass(cTEMPLATPATH)

#If cOFFLINEcDEBUG = 0 Then
                ' 原点復帰処理()
                r = sResetStart()
                gflgResetStart = True                       ' 初期設定フラグON
                ''V5.0.0.1-23                If (r <= cFRS_ERR_EMG Or r = cFRS_ERR_RST) Then ' RESETは終了する
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then      ' SL432R系 ? ###035
                    If (r <= cFRS_ERR_EMG Or r = cFRS_ERR_RST Or r = cFRS_ERR_CVR) Then ' RESETは終了する
                        ' 強制終了
                        Call AppEndDataSave()
                        Call AplicationForcedEnding()
                        Exit Sub
                    End If
                Else
                    If (r <= cFRS_ERR_EMG Or r = cFRS_ERR_RST) Then ' RESETは終了する
                        ' 強制終了
                        Call AppEndDataSave()
                        Call AplicationForcedEnding()
                        Exit Sub
                    End If
                End If
                'V5.0.0.1-23
                '----- V1.14.0.0⑤↓ -----
                ' LEDバックライトON/OFF(オプション)
                Call Set_Led(0, 0)                          ' バックライト照明ON又はOFF 

                ' LEDバックライトON(EXTOUT)                 '###061
                'Call EXTOUT1(glLedBit, 0)                   ' LED ON 
                '----- V1.14.0.0⑤↑ -----
#End If

                ''V6.0.1.022↓通常の速度を転送する。 
                SetXYStageSpeed(StageSpeed.NormalSpeed)
                ''V6.0.1.022↑

                '-----------------------------------------------------------------------
                '   FL側へ加工条件を送信する(FL時で加工条件ファイルがある場合)
                '-----------------------------------------------------------------------
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                    ' '' データ送信中のメッセージ表示
                    ''strMSG = MSG_148
                    ''Call Z_PRINT(strMSG)                                ' ﾒｯｾｰｼﾞ表示(ログ画面)
                    Dim strSetFileName As String = ""

                    ' FL用加工条件ファイルをリードしてFL側へ加工条件を送信する
                    r = SendTrimCondInfToFL(stCND, DEF_FLPRM_SETFILENAME, strSetFileName)
                    If (r <> SerialErrorCode.rRS_OK) Then
                        '----- V2.0.0.0⑤↓ -----
                        If (r <= SerialErrorCode.rRS_FLCND_XMLREADERR) Then
                            '"加工条件ファイルリードエラー。""
                            strMSG = MSG_158 + "(File = " + strSetFileName + ")"
                        Else
                            '"ＦＬ通信異常。ＦＬとの通信に失敗しました。" + vbCrLf + "ＦＬと正しく接続できているか確認してください。"
                            strMSG = MSG_150
                        End If
                        'strMSG = MSG_150                    '"ＦＬ通信異常。ＦＬとの通信に失敗しました。" + vbCrLf + "ＦＬと正しく接続できているか確認してください。"
                        Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
                        '----- V2.0.0.0⑤↑ -----
                    End If
                End If
                'Call System1.sLampOnOff(LAMP_Z, False)     ' HALTﾗﾝﾌﾟON 
                '----- V1.14.0.0②↓ -----
                ' FL用パワー調整情報初期化
                Call GetFlAttInfoData(strMSG, stPWR_LSR, 0)
                ' INtime側へ固定ATT情報を送信する(初期化)
                lRet = SetFixAttInfo(stPWR_LSR.AttInfoAry(0)) 'V1.18.0.0⑤
                '----- V1.14.0.0②↑ -----

                '-----------------------------------------------------------------------
                ' コマンドボタンを有効にする
                '-----------------------------------------------------------------------
                Call Form1Button(2)                         ' ボタン等の表示/非表示
                Call Form1Button(1)                         ' ボタン活性化/非活性化
                frmHistoryData.Visible = True
                '----- V1.18.0.0③↓ -----
                If (giPrint = 1) Then                       ' Printボタン有効 ? 
                    gSysPrm.stDEV.rPrnOut_flg = True
                    BtnPrintOnOff.Text = "Print ON"
                    BtnPrintOnOff.BackColor = Color.Lime
                Else
                    giPrint = 0
                End If
                '----- V1.18.0.0③↑ -----

                '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
                ' 「基板投入ボタン」
                BtnSubstrateSet.Text = LBL_BTN_SUBSTRAT                 ' "基板投入"
                BtnSubstrateSet.Visible = False                         '「基板投入ボタン」非表示 
                If (giSubstrateInvBtn = 1) Then                         ' 一時停止画面での「基板投入」ボタンの有効 ?　
                    BtnSubstrateSet.Visible = True                      '「基板投入ボタン」表示
                    BtnSubstrateSet.Enabled = False                     '「基板投入ボタン」非活性化
                End If
                '----- V4.11.0.0⑥↑ -----

                '-----------------------------------------------------------------------
                '   設定値表示(オプション)
                '-----------------------------------------------------------------------
                ' 減衰率をシスパラより表示("減衰率 = 99.9%")
                ' ※ﾛｰﾀﾘｱｯﾃﾈｰﾀの設定はOcxSystemの原点復帰処理で行われる
                If (gSysPrm.stRMC.giRmCtrl2 >= 2 And
                    gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then          ' ﾛｰﾀﾘｱｯﾃﾈｰﾀ制御有(RMCTRL2対応時有効) ? ###029
                    'If (gSysPrm.stRMC.giRmCtrl2 >= 1) Then          ' ﾛｰﾀﾘｱｯﾃﾈｰﾀ制御有(RMCTRL2対応時有効) ?
                    strMSG = LBL_ATT + "  " + CDbl(gSysPrm.stRAT.gfAttRate).ToString("##0.0") + " %"
                    Me.LblRotAtt.Text = strMSG
                    '----- V1.16.0.0⑭↓ -----
                    ' 減衰率の表示/非表示をシスパラより設定する
                    'Me.LblRotAtt.Visible = True
                    If (giNoDspAttRate = 1) Then
                        Me.LblRotAtt.Visible = False
                    Else
                        Me.LblRotAtt.Visible = True
                    End If
                    '----- V1.16.0.0⑭↑ -----
                End If

                '' FL時のオートパワー調整用電流値表示(FL時は減衰率の代わりに電流値を表示する) ###066
                '' ※FLでパワーメータのデータ取得タイプが「Ｉ／Ｏ読取り」/「ＵＳＢ」で「パワー測定値表示あり」の場合に表示する
                'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And _
                '   (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                '    strMSG = LBL_FLCUR + " ----mA(Off:---)"
                '    Me.LblRotAtt.Text = strMSG
                '    Me.LblRotAtt.Visible = True
                'End If

                ' 定電流値表示
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    strMSG = "定電流値 "
                'Else
                '    strMSG = "CurrentVal "
                'End If
                Select Case (gSysPrm.stSPF.giProcPower2)
                    Case 0                                      ' 指定なし(標準)
                        LblCur.Text = Form1_002 & "0.25A"          '  定電流値 0.25A
                        'Case 1
                        '    LblCur.Text = strMSG & "1.00A"          '  定電流値 1.00A
                        'Case 2
                        '    LblCur.Text = strMSG & "0.75A"          '  定電流値 0.75A
                    Case 3
                        LblCur.Text = Form1_002 & "0.50A"          '  定電流値 0.50A
                End Select

                ' 加工電力表示/非表示設定
                'If (gSysPrm.stSPF.giProcPower = 4) And (gSysPrm.stSPF.giProcPower2 <> 0) Then  '###250
                If (gSysPrm.stSPF.giProcPower2 <> 0) Then                                       '###250
                    LblCur.Visible = True
                Else
                    LblCur.Visible = False
                End If

                '-----------------------------------------------------------------------
                ' コンソールキーのラッチ解除
                '-----------------------------------------------------------------------
                Call ZCONRST()

                ' 'V4.1.0.0⑦
                CheckPLCLowBatteryAlarm()

                '-----------------------------------------------------------------------
                ' 監視タイマー開始
                '-----------------------------------------------------------------------
                ''V6.0.1.0⑬ ↓ Timer1.Interval = 10                        ' 監視タイマー値(msec)
                Timer1.Interval = 100                                   ' 監視タイマー値(msec)
                ''V6.0.1.0⑬↑
                Timer1.Enabled = True                                   ' 監視タイマー開始
                '----- V1.18.0.0②↓ -----
                ' QRコード受信チェックタイマー開始(ローム殿特注)
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    TimerQR.Interval = 1000
                    TimerQR.Enabled = True
                End If
                '----- V1.18.0.0②↑ -----
                '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応)【ＱＲコードリード機能】 -----
                If (giQrCodeType = QrCodeType.KoaEw) Then
                    If (gbQRCodeReaderUse = True OrElse gbQRCodeReaderUseTKYNET = True) Then    ' ＱＲコードリード機能はTKY-CHIPのみ有効 'V6.1.4.10② TKY-NET（gbQRCodeReaderUseTKYNET）追加
                        TimerQR.Interval = 1000                         ' 監視タイマー値(msec)
                        TimerQR.Enabled = True                          ' 監視タイマー開始
                    End If
                End If
                '----- V6.1.4.0_22↑ -----
#If False Then                          'V5.0.0.9⑮
                '----- V1.23.0.0①↓ -----
                ' バーコード受信チェックタイマー開始(太陽社殿特注)
                If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then
                    TimerBC.Interval = 1000
                    TimerBC.Enabled = True
                End If
                '----- V1.23.0.0①↑ -----

                '----- V1.23.0.0①↓ -----
                ' バーコード受信チェックタイマー開始(太陽社殿特注)/(WALSIN殿SL436S対応) V4.11.0.0②
                'If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then   V4.11.0.0②
                If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Or (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then ' V4.11.0.0②
                    TimerBC.Interval = 1000
                    TimerBC.Enabled = True
                End If
                '----- V1.23.0.0①↑ -----
#Else                                   'V5.0.0.9⑮
                ' バーコード受信チェックタイマー開始
                If (BarcodeType.None <> BarCode_Data.Type) Then
                    'V5.0.0.9⑲          ↓
                    BarCode_Data.BC_ReadCount = 0
                    BarCode_Data.BC_ReadDataFirst = ""                  ' バーコード１回目で読込んだデータ保存用
                    BarCode_Data.BC_ReadDataSecound = ""                ' バーコード２回目で読込んだデータ保存用
                    'V5.0.0.9⑲          ↑

                    TimerBC.Interval = 1000
                    TimerBC.Enabled = True
                End If
#End If
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "Initialize_TrimMachine TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ソフト終了時のステージ移動処理(オプション)"
    '''=========================================================================
    ''' <summary>ソフト終了時のステージ移動処理(オプション) V1.13.0.0⑩</summary>
    ''' <param name="PosX">(INP)ステージ位置X</param>
    ''' <param name="PosY">(INP)ステージ位置Y</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function AppEndStageMove(ByVal PosX As Double, ByVal PosY As Double) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 基板交換時とAPP終了時のステージ位置指定なしならNOP
            If (PosX = 0) And (PosY = 0) Then Return (cFRS_NORMAL)

            ' "XYテーブルを移動します","スライドカバーを閉じてください"表示後スライドカバー閉待ち
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                r = System1.ClampVacume_Ctrl(gSysPrm, 1, giAppMode, 1)  ' クランプ/吸着 ON
                If (r < cFRS_NORMAL) Then                               ' エラーならエラー戻り(エラーメッセージは表示済み)               
                    Return (r)
                End If
                r = System1.Form_Reset(cGMODE_XYMOVE, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r < cFRS_NORMAL) Then                               ' エラーならエラー戻り(エラーメッセージは表示済み)               
                    Return (r)
                End If
            End If
            Me.Refresh()                                                ' メッセージを消すため

            ' 基板交換時とAPP終了時のステージ位置へ移動
            r = System1.EX_SMOVE2(gSysPrm, PosX, PosY)
            If (r < cFRS_NORMAL) Then                                   ' エラー ?(※エラーメッセージは表示済み) 
                Return (r)                                              ' アプリ強制終了へ
            End If

            ' "スライドカバーを開けてください"表示後スライドカバー開待ち
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                r = System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                If (r < cFRS_NORMAL) Then                               ' エラーならエラー戻り(エラーメッセージは表示済み)               
                    Return (r)
                End If
                ' クランプ/吸着OFF
                r = System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)
                If (r < cFRS_NORMAL) Then                               ' エラーならエラー戻り(エラーメッセージは表示済み)               
                    Return (r)
                End If
            End If

            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.AppEndStageMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認"
    '''=========================================================================
    '''<summary>ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub AppEndDataSave()

        'Dim ret As Short
        Dim ret As MsgBoxResult         'V4.10.0.0⑤

        '----- V6.1.4.0⑩↓(KOA EW殿SL432RD対応)【ロット切替え機能】-----
        If (giLotChange = 1) Then                                   ' ロット切替え機能有効 ?
            ' ローダー出力(ON=トリマ動作中 or トリミング不良信号を連続自動運転中止通知に使用, OFF=トリマレディ)
            Call SetLoaderIO(COM_STS_TRM_STATE Or COM_STS_TRM_NG, COM_STS_TRM_READY)
        End If
        '----- V6.1.4.0⑩↑ -----

        ' 編集中のデータあり ?
        If gCmpTrimDataFlg = 1 Then
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    ret = MsgBox("アプリケーションを終了します。" & vbCrLf & "トリミングデータを保存しますか？", MsgBoxStyle.OkCancel, "")
            'Else
            '    ret = MsgBox("Quits the program." & vbCrLf & "Do you store trimming data?", MsgBoxStyle.OkCancel, "")
            'End If
            'ret = MsgBox(Form1_003 & vbCrLf & Form1_004, MsgBoxStyle.OkCancel, "")
            ret = MsgBox(Form1_003 & vbCrLf & Form1_004, MsgBoxStyle.YesNo, "") 'V4.10.0.0⑤
            'If ret = MsgBoxResult.Ok Then
            If (ret = MsgBoxResult.Yes) Then                                    'V4.10.0.0⑤
                ' データ保存
                Call Me.CmdSave_Click(Me.CmdSave, New System.EventArgs())
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    ret = MsgBox("データの保存が完了しました。" & vbCrLf & "アプリケーションを終了します。", MsgBoxStyle.OkOnly, "")
                'Else
                '    ret = MsgBox("A save of data was completed." & vbCrLf & "Quits the program.", MsgBoxStyle.OkOnly, "")
                'End If
                'V4.10.0.0⑤              ↓
                Dim msg As String
                If (0 = gCmpTrimDataFlg) Then
                    ' 保存した
                    msg = (Form1_005 & vbCrLf & Form1_003)
                Else
                    ' 保存をｷｬﾝｾﾙした
                    msg = Form1_003
                End If
                'ret = MsgBox(Form1_005 & vbCrLf & Form1_003, MsgBoxStyle.OkOnly, "")
                ret = MsgBox(msg, MsgBoxStyle.OkOnly, "")
                'V4.10.0.0⑤              ↑
            Else
                ' データ保存なし
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    ret = MsgBox("アプリケーションを終了します。", MsgBoxStyle.OkOnly, "")
                'Else
                '    ret = MsgBox("Quits the program.", MsgBoxStyle.OkOnly, "")
                'End If
                ret = MsgBox(Form1_003, MsgBoxStyle.OkOnly, "")
            End If
        Else
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    ret = MsgBox("アプリケーションを終了します。", MsgBoxStyle.OkOnly, "")
            'Else
            '    ret = MsgBox("Quits the program.", MsgBoxStyle.OkOnly, "")
            'End If
            ret = MsgBox(Form1_003, MsgBoxStyle.OkOnly, "")
        End If

    End Sub

#End Region

#Region "ｿﾌﾄ強制終了処理"
    '''=========================================================================
    '''<summary>ｿﾌﾄ強制終了処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub AplicationForcedEnding()

        Dim lRet As Integer
        Dim hProcInf As New System.Diagnostics.ProcessStartInfo()
        'Dim ret As Short

        Try
            'V6.0.0.0⑤            Call FinalEnd_GazouProc(ObjGazou)  'V3.0.0.0⑤DispGazou終了

            '----- ###175↓-----
            'V1.25.0.5②If (bFgAutoMode) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
            If (Not bEmergencyOccurs) And (bFgAutoMode) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                ' 残基板取り除きメッセージ表示(STARTキー押下待ち画面表示)
                lRet = Sub_CallFrmRset(System1, cGMODE_LDR_WKREMOVE2)
            End If
            '----- ###175↑ -----
            '----- V1.18.0.0③↓ -----
            ' トリミング結果印刷処理(ローム殿特注)
            Call PrnTrimResult(2)

            ' 印刷終了処理(ローム殿特注)
            Call Print_End()
            '----- V1.18.0.0③↑ -----
            '----- V1.18.0.0②↓ -----
            ' ポートクローズ(QRコード受信用 ローム殿特注)
            Call QR_Rs232c_Close()
            '----- V1.18.0.0②↑ -----
            '----- V1.23.0.0①↓ -----
            ' ポートクローズ(バーコード受信用 太陽社殿特注)
            Call BC_Rs232c_Close()
            '----- V1.23.0.0①↑ -----

            ' オートローダ終了処理
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ?
                '----- V6.1.4.0⑩↓(KOA EW殿SL432RD対応)【ロット切替え機能】-----
                'Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_READY) ' ローダー出力(ON=トリマ動作中, OFF=トリマレディ) ###035
                If (giLotChange = 1) Then                               ' ロット切替え機能有効 ?
                    ' トリミング不良信号を連続自動運転中止通知に使用
                    Call SetLoaderIO(COM_STS_TRM_STATE Or COM_STS_TRM_NG, COM_STS_TRM_READY)
                Else
                    ' ローダー出力(ON=トリマ動作中, OFF=トリマレディ)
                    Call SetLoaderIO(COM_STS_TRM_STATE, COM_STS_TRM_READY)
                End If
                '----- V6.1.4.0⑩↑ -----
            Else
                Call Loader_Term()                                      ' ローダー出力(ON=なし,OFF=トリマ部レディ)
            End If

            ' シグナルタワー制御初期化(On=0, Off=全ﾋﾞｯﾄ)
            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
            ' Call System1.SetSignalTower(0, &HFFFF)                      ' ###007
            Call System1.SetSignalTowerCtrl(System1.SIGNAL_ALL_OFF)
            'V5.0.0.9⑭ ↑ V6.0.3.0⑧

            ' スライドカバーオープン/クローズバルブOFF
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then       ' SL432R系 ? 
                Call ZSLCOVERCLOSE(0)                                   ' スライドカバークローズバルブOFF
                Call ZSLCOVEROPEN(0)                                    ' スライドカバーオープンバルブOFF
            End If

            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
            If (giDoorLock = 1) Then                                    ' 電磁ロック有効 ?
                Call EXTOUT1(0, EXTOUT_EX_LOK_ON)
            End If
            '----- V1.18.0.1⑧↑ -----

            '----- V2.0.0.0⑬↓ -----
            ' サーボアラームならアラームクリアする
            Call SrvAlm_Check()

            '' サーボアラームクリア
            'Call CLEAR_SERVO_ALARM(1, 1)                                ' X/Y,Z/THETA '###031
            ''----- V1.13.0.0①↓ -----
            'If (gSysPrm.stDEV.giPrbTyp = 3) Then                        ' Z2あり ? 
            '    Call CLEAR_SERVO_ALARM_Z2(1)                            ' Z2/予備
            'End If
            ''----- V1.13.0.0①↑ -----
            '----- V2.0.0.0⑬↑ -----

            ' ビデオライブラリ終了処理
            If (pbVideoInit = True) Then
                lRet = VideoLibrary1.Close_Library
                If (lRet <> 0) Then
                    Select Case lRet
                        Case cFRS_VIDEO_INI
                            'Call System1.TrmMsgBox(gSysPrm, "Video library: Not initialized.", MsgBoxStyle.OkOnly, My.Application.Info.Title)
                            Call MsgBox("Video library: Not initialized.", MsgBoxStyle.OkOnly, My.Application.Info.Title) ' 2011.09.01
                        Case Else
                            ' "予期せぬエラー"
                            'Call System1.TrmMsgBox(gSysPrm, "Video library: Unexpected error.", MsgBoxStyle.OkOnly, My.Application.Info.Title)
                            Call MsgBox("Video library: Unexpected error.", MsgBoxStyle.OkOnly, My.Application.Info.Title) ' 2011.09.01
                    End Select
                End If
            End If

            ' 操作ログ出力("トリマ装置停止")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC10, "")
            gflgCmpEndProcess = True

            ' クランプ及びバキュームOFF '###001
            Call Me.System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, giTrimErr)

            ' ランプOFF
            Call LAMP_CTRL(LAMP_START, False)   ' STARTランプOFF 
            Call LAMP_CTRL(LAMP_RESET, False)   ' RESETランプOFF 
            Call LAMP_CTRL(LAMP_Z, False)       ' ZランプOFF 

            '----- V1.14.0.0⑤↓ -----
            ' LEDバックライトOFF(オプション)
            Call Set_Led(2, 0)
            '----- V1.14.0.0⑤↑ -----

            '-----------------------------------------------------------------------
            '   Mutexの解放
            '-----------------------------------------------------------------------
            gmhTky.ReleaseMutex()

            '-----------------------------------------------------------------------
            '   イベントの解放
            '-----------------------------------------------------------------------
            RemoveHandler SystemEvents.SessionEnding, AddressOf SystemEvents_SessionEnding

            '-----------------------------------------------------------------------
            '終了時Videolib関係でエラーが発生するため強制的に外部からアプリを終了させる。
            '-----------------------------------------------------------------------
            hProcInf.FileName = APP_FORCEEND
            'hProcInf.Arguments = gAppName
            hProcInf.Arguments = System.Diagnostics.Process.GetCurrentProcess.ProcessName
            Call System.Diagnostics.Process.Start(hProcInf)

            Call System.Threading.Thread.Sleep(2000)

        Catch ex As Exception
            ' 操作ログ出力("トリマ装置停止")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC10, "")
            gflgCmpEndProcess = True

            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub

#End Region
    '----- V2.0.0.0⑬↓ -----
#Region "サーボアラームチェック処理"
    '''=========================================================================
    '''<summary>サーボアラームチェック処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SrvAlm_Check()

        Dim SrvSts As Long = 0
        Dim Bit_Clr_XY As Integer = 0
        Dim Bit_Clr_ZT As Integer = 0
        Dim strMSG As String

        Try

            ' XY軸サーボアラームチェック
            If (gSysPrm.stDEV.giXYtbl <> 0) Then                                    ' XYあり ? 
                Call INP16(SRVALM_XY_STS, SrvSts)
                If ((SrvSts And (SRVALM_X_BIT + SRVALM_Y_BIT)) <> 0) Then           ' XY軸アラーム ?
                    Bit_Clr_XY = 1                                                  ' XY軸サーボアラームクリア
                End If
            End If

            ' Z/THETA軸サーボアラームチェック
            If (gSysPrm.stDEV.giPrbTyp <> 0) Or (gSysPrm.stDEV.giTheta <> 0) Then   ' Z/THETA軸あり ? 
                Call INP16(SRVALM_ZT_STS, SrvSts)
                If ((SrvSts And (SRVALM_Z_BIT + SRVALM_T_BIT)) <> 0) Then           ' Z/THETA軸アラーム ?
                    Bit_Clr_ZT = 1                                                  ' Z/THETA軸サーボアラームクリア
                End If
            End If

            ' X/Y,Z/THETA軸のどれかがサーボアラームならサーボアラームクリアする
            If (Bit_Clr_XY <> 0) Or (Bit_Clr_ZT <> 0) Then
                Call SERVO_POWER(0, 0, 0, 0)                                        ' サーボOFF(X,Y,Z,T) 
                Call CLEAR_SERVO_ALARM(Bit_Clr_XY, Bit_Clr_ZT)                      ' サーボアラームクリア
            End If

            ' Z2軸サーボアラームチェック
            If (gSysPrm.stDEV.giPrbTyp = 3) Then                                    ' Z2あり ? 
                Call INP16(SRVALM_Z2_STS, SrvSts)
                If (SrvSts And SRVALM_Z2_BIT) Then                                  ' アラームなら
                    Call SERVO_POWER_Z2(0, 0)                                       ' サーボOFF(Z2,α) 
                    Call CLEAR_SERVO_ALARM_Z2(1)                                    ' サーボアラームクリア
                End If
            End If

        Catch ex As Exception
            strMSG = "SrvAlm_Check() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V2.0.0.0⑬↑ -----
#Region "シスパラよりクロスライン位置を設定する"
    '''=========================================================================
    '''<summary>ｸﾛｽﾗｲﾝ補正</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetCrossLine()

        ' シスパラよりクロスライン位置を設定する 
        If gKeiTyp = KEY_TYPE_RS Then
            'Me.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
            'V6.0.0.0④            Me.Picture2.Left = CROSS_LINEX + VideoLibrary1.Location.X
            'V6.0.0.0④            Me.Picture1.Top = CROSS_LINEY + VideoLibrary1.Location.Y
            VideoLibrary1.SetCrossLineCenter(CROSS_LINEX, CROSS_LINEY)                              'V6.0.0.0④
        Else
            'V6.0.0.0④            Me.Picture1.Top = gSysPrm.stDVR.giCrossLineX + VideoLibrary1.Location.Y
            'V6.0.0.0④            Me.Picture2.Left = gSysPrm.stDVR.giCrossLineY + VideoLibrary1.Location.X
            VideoLibrary1.SetCrossLineCenter(gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX) 'V6.0.0.0④
        End If

        VideoLibrary1.SetCrossLineVisible(True) 'V6.0.0.0④
    End Sub
#End Region

#Region "インターロック状態の表示/非表示"
    '''=========================================================================
    '''<summary>インターロック状態の表示/非表示</summary>
    ''' <returns>インターロック状態
    '''          INTERLOCK_STS_DISABLE_FULL = インターロック全解除
    '''          INTERLOCK_STS_DISABLE_PART = インターロック一部解除（ステージ動作可能）
    '''          INTERLOCK_STS_DISABLE_NO   = インターロック状態（解除なし）
    ''' </returns>
    '''<remarks></remarks>
    '''=========================================================================
    Public Function DispInterLockSts() As Integer

        Dim r As Integer
        Dim InterlockSts As Integer
        Dim SwitchSts As Long
        Dim strMSG As String

        Try
            ' インターロック状態によりステータス表示を変更
            r = INTERLOCK_CHECK(InterlockSts, SwitchSts)
#If cOFFLINEcDEBUG Then
            InterlockSts = INTERLOCK_STS_DISABLE_FULL
#End If
            ' インターロック全解除の場合
            If (InterlockSts = INTERLOCK_STS_DISABLE_FULL) Then         ' インターロック全解除 ?
                Me.lblInterLockMSG.Text = MSG_SPRASH25                  '「インターロック全解除中」表示
                Me.lblInterLockMSG.Visible = True
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R時はインターロック状態を出力する ###070 
                    '###131 Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)         ' ローダー出力(ON=インターロック解除中,OFF=なし)
                    ' インターロック状態送信(SL436R時) '###114
                    Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)      ' ローダー出力(ON=インターロック解除中,OFF=なし)
                End If
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed ###108

                ' インターロック一部解除の場合
            ElseIf (InterlockSts = INTERLOCK_STS_DISABLE_PART) Then     ' インターロック一部解除（ステージ動作可能） ?
                Me.lblInterLockMSG.Text = MSG_SPRASH26                  '「インターロック一部解除中」表示
                Me.lblInterLockMSG.Visible = True
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R時はインターロック状態を出力する ###070   
                    '###131 'Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)         ' ローダー出力(ON=インターロック解除中,OFF=なし)
                    ' インターロック状態送信(SL436R時) '###114
                    Call SetLoaderIO(LOUT_INTLOK_DISABLE, &H0)      ' ローダー出力(ON=インターロック解除中,OFF=なし)
                End If
                Call ZSELXYZSPD(1)                                      ' XYZ Slow speed ###108

                ' インターロック中の場合
            Else                                                        '「インターロック中」
                Me.lblInterLockMSG.Visible = False
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R時はインターロック状態を出力する ###070  
                    '###131 'Call SetLoaderIO(&H0, LOUT_INTLOK_DISABLE)         ' ローダー出力(ON=なし,OFF=インターロック解除中)
                    ' インターロック状態送信(SL436R時) '###114
                    Call SetLoaderIO(&H0, LOUT_INTLOK_DISABLE)      ' ローダー出力(ON=なし,OFF=インターロック解除中)
                End If
                Call ZSELXYZSPD(0)                                      ' XYZ Normal speed ###108
            End If

            Return (InterlockSts)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.DispInterLockSts() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "フォームのラベルを設定する"
    '''=========================================================================
    '''<summary>フォームのラベルを設定する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form1LanguageSet()

        ' 生産管理情報ラベルを設定する
        If (gTkyKnd = KND_CHIP) Then
            ' CHIPの場合
            LblUnit.Text = MSG_TOTAL_REGISTOR           ' "抵抗単位"
        Else
            ' TKY-NETの場合
            LblUnit.Text = MSG_TOTAL_CIRCUIT            ' "サーキット単位"
        End If

        'LblPlate.Text = MSG_MAIN_LABEL01                    ' "基板枚数="
        'LblcNGPER.Text = MSG_MAIN_LABEL02                   ' "NG率="
        'LblcITHINGP.Text = MSG_MAIN_LABEL03                 ' "IT H NG率="
        'LblcITLONGP.Text = MSG_MAIN_LABEL04                 ' "IT L NG率="
        'LblcFTHINGP.Text = MSG_MAIN_LABEL05                 ' "FT H NG率="
        'LblcFTLONGP.Text = MSG_MAIN_LABEL06                 ' "FT L NG率="
        'LblcOVERP.Text = MSG_MAIN_LABEL07                   ' "OVER NG率="

        'checkAutoTeach.Text = MSG_TRIM_06      '自動ポジション
        'cmdDataClear.Text = BTN_TRIM_01        'ｸﾞﾗﾌﾃﾞｰﾀ ｸﾘｱ
        'cmdReEdit.Text = BTN_TRIM_02           'ﾌﾟﾚｰﾄﾃﾞｰﾀ編集
        'cmdGrpInitialTest.Text = BTN_TRIM_03   'ｲﾆｼｬﾙﾃｽﾄ分布表示
        'cmdGrpFinalTest.Text = BTN_TRIM_04     'ﾌｧｲﾅﾙﾃｽﾄ分布表示

        ' ｲﾆｼｬﾙﾃｽﾄ/ﾌｧｲﾅﾙﾃｽﾄ分布図ラベルを設定する
        'lblRegistTitle.Text = PIC_TRIM_09                   ' "抵抗数"
        'lblGoodTitle.Text = PIC_TRIM_03                     ' "良品"
        'lblNgTitle.Text = PIC_TRIM_04                       ' "不良品"
        'lblMinTitle.Text = PIC_TRIM_05                      ' "最小%"
        'lblMaxTitle.Text = PIC_TRIM_06                      ' "最大%"
        'lblAverage.Text = PIC_TRIM_07                       ' "平均%"
        'lblDeviation.Text = PIC_TRIM_08                     ' "標準偏差"
    End Sub
#End Region
    '----- ###266↓ -----
#Region "フォームのボタン名の設定(日本語/英語)"
    '''=========================================================================
    '''<summary>フォームのボタン名の設定(日本語/英語)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub SetButtonImage()

        Dim strMSG As String
        'Dim ObjTkyMsg As DllTkyMsgGet.TkyMsgGet                        ' TKY用メッセージファイルリードオブジェクト(DllTkyMsgGet.dll) 
        'Dim strBTN(MAX_FNCNO + 1) As String                            ' ボタン名の配列(0 ORG) V1.18.0.1① Publicに変更
        'Dim Count As Integer

        Try
            '-------------------------------------------------------------------
            '   フォームのボタン名をTky_Msg.iniより設定する(日本語/英語)
            '-------------------------------------------------------------------
            'ObjTkyMsg = New DllTkyMsgGet.TkyMsgGet                      ' TKY用メッセージファイルリードオブジェクト生成 
            'Count = ObjTkyMsg.Get_Button_Name(gSysPrm.stTMN.giMsgTyp, strBTN)

            strBTN(F_LOAD) = CmdLoad.Text                               ' "ロード"
            strBTN(F_SAVE) = CmdSave.Text                               ' "セーブ"
            strBTN(F_EDIT) = CmdEdit.Text                               ' "編集"
            strBTN(F_LASER) = CmdLaser.Text                             ' "レーザ"
            strBTN(F_LOG) = CmdLoging.Text                              ' "ロギング"
            strBTN(F_PROBE) = CmdProbe.Text                             ' "プローブ"
            strBTN(F_TEACH) = CmdTeach.Text                             ' "ティーチング"
            strBTN(F_CUTPOS) = CmdCutPos.Text                           ' "カット位置補正"
            strBTN(F_RECOG) = CmdPattern.Text                           ' "画像登録"
            strBTN(MAX_FNCNO) = CmdEnd.Text                             ' "終了"

            ' CHIP,NET共通ボタン
            strBTN(F_TX) = CmdTx.Text                                   ' "BP位置調整"
            strBTN(F_TY) = CmdTy.Text                                   ' "ステージ位置調整"
            strBTN(F_EXR1) = CmdExCam.Text                              ' 外部カメラティーチング "1抵抗"
            strBTN(F_EXTEACH) = CmdExCam1.Text                          ' 外部カメラティーチング "全抵抗"
            strBTN(F_CARREC) = CmdPtnCalibration.Text                   ' キャリブレーション "画像登録"
            strBTN(F_CAR) = CmdCalibration.Text                         ' キャリブレーション "補正実行"
            strBTN(F_CUTREC) = CmdPtnCutPosCorrect.Text                 ' 外部カメラカット位置 "画像登録"
            strBTN(F_CUTREV) = CmdCutPosCorrect.Text                    ' 外部カメラカット位置 "補正実行"

            ' CHIPボタン
            strBTN(F_TY2) = CmdTy2.Text                                 ' "ＴＹ２ティーチング"
            strBTN(F_TTHETA) = CmdT_Theta.Text                          ' "Tθ"

            ' SL436Rボタン
            strBTN(F_AUTO) = CmdAutoOperation.Text                      ' "自動運転"
            strBTN(F_LOADERINI) = CmdLoaderInit.Text                    ' "ローダ原点復帰"

            '----- V1.13.0.0③↓ -----
            ' TKY系オプション
            strBTN(F_APROBEREC) = CmdAotoProbePtn.Text                  ' "画像登録"
            strBTN(F_APROBEEXE) = CmdAotoProbeCorrect.Text              ' "実行" 
            strBTN(F_IDTEACH) = CmdIDTeach.Text                         ' "ＩＤティーチング"
            strBTN(F_SINSYUKU) = CmdSinsyukuPtn.Text                    ' "画像登録"
            strBTN(F_MAP) = CmdMap.Text                                 ' "MAP"
            '----- V1.13.0.0③↑ -----

            'V4.10.0.0③                  ↓
            strBTN(F_INTEGRATED) = CmdIntegrated.Text                   ' 統合登録調整
            grpIntegrated.Text = strBTN(F_INTEGRATED)                   ' 統合登録調整
            lblIntegRecog.Text = strBTN(F_RECOG)                        ' 画像登録
            lblIntegProbe.Text = strBTN(F_PROBE)                        ' プローブ
            lblIntegTX.Text = strBTN(F_TX)                              ' BP位置調整
            lblIntegTeach.Text = strBTN(F_TEACH)                        ' ティーチング
            lblIntegTY.Text = strBTN(F_TY)                              ' ステージ位置調整
            'V4.10.0.0③                  ↑
            strBTN(F_PROBE_CLEANING) = CmdT_ProbeCleaning.Text          ' "プローブクリーニング"
            strBTN(F_RECOG_ROUGH) = CmdRecogRough.Text                  ' ラフ画像登録        'V5.0.0.9④
            strBTN(F_FOLDEROPEN) = CmdFolderOpen.Text                   ' ﾌｫﾙﾀﾞ表示 V6.1.4.0⑥

            ''-------------------------------------------------------------------
            ''   DIG-SWHを設定する(日本語/英語) ※将来的にはTky_Msg.iniより設定する
            ''-------------------------------------------------------------------
            'CbDigSwH.Items(0) = LBL_FINEADJ_003                         ' "０：表示なし"
            'CbDigSwH.Items(1) = LBL_FINEADJ_004                         ' "１：ＮＧのみ表示"
            'CbDigSwH.Items(2) = LBL_FINEADJ_005                         ' "２：全て表示"

            ''-------------------------------------------------------------------
            ''   ラベル名を設定する(日本語/英語) ※将来的にはTky_Msg.iniより設定する
            ''-------------------------------------------------------------------
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    lblExCamera.Text = "外部カメラティーチング"
            '    lblCalibration.Text = "キャリブレーション"
            '    lblCutPos.Text = "外部カメラカット位置"
            '    chkDistributeOnOff.Text = "生産グラフ　表示"
            '    lblAutoProbe.Text = "オートプローブ"                    'V1.13.0.0③
            '    lblSinsyuku.Text = "伸縮補正"                           'V1.13.0.0③
            'Else
            '    lblExCamera.Text = "ExCameraTeaching"
            '    lblCalibration.Text = "Calibration"
            '    lblCutPos.Text = "ExCamCutPosition"
            '    chkDistributeOnOff.Text = "Distribute ON"
            '    lblAutoProbe.Text = "Auto Probe"                        'V1.13.0.0③
            '    lblSinsyuku.Text = "Elastic Compensation"               'V1.13.0.0③
            'End If

            '' 後処理
            'ObjTkyMsg = Nothing                                         ' オブジェクト開放

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetButtonImage() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- ###266↑ -----
#Region "フォームのコマンドボタンを有効・無効にする"
    '''=========================================================================
    '''<summary>フォームのコマンドボタンを有効・無効にする</summary>
    '''<param name="Flg">(INP) 0=ボタン非活性化
    '''                        1=ボタン活性化
    '''                        2=ボタン等の表示/非表示
    ''' 　　　　　　　　　　　 3=「SAVE」ﾎﾞﾀﾝと「END」ﾎﾞﾀﾝのみ有効とする
    ''' 　　　　　　　　　　　 4=「LOAD」/「ﾛｷﾞﾝｸﾞ」/「ﾛｰﾀﾞ原点復帰」/「END」ﾎﾞﾀﾝのみ有効とする
    ''' </param>
    '''=========================================================================
    Public Sub Form1Button(ByVal Flg As Short)

        Dim strMSG As String
        Dim isDispOptGrp As Boolean
        Dim isDispOptGrp2 As Boolean                                    ' V1.13.0.0③
        Dim isDispOptGrp3 As Boolean = False                            'V4.10.0.0⑨

        Try

            If SimpleTrimmer.IsBlockDataDisp() Then                     'V2.0.0.0⑩ ブロックデータ表示中は、表示させない為
                Exit Sub
            End If
            ' オプションコントロールボタンの非表示数
            isDispOptGrp = False
            isDispOptGrp2 = False                                       ' V1.13.0.0③

            ' 「TRIMMING」ボタン表示/非表示(デバッグ用)
            '#If cOFFLINEcDEBUG Then
            'Me.btnTrimming.Visible = True  '###036
            Me.btnTrimming.Visible = False  '###036
            '#Else
            'Me.btnTrimming.Visible = False
            '#End If
            If (Flg = 1) Then
                '---------------------------------------------------------------------------
                '   ボタンを活性化する
                '---------------------------------------------------------------------------
                Me.btnCounterClear.Enabled = True           ' CLRﾎﾞﾀﾝ
                Me.btnTrimming.Enabled = True               ' GOボタン
                Me.CbDigSwH.Enabled = True                  ' DisplayModeコンボボックスリスト
                Me.CbDigSwL.Enabled = True                  ' MoveModeコンボボックスリスト
                '----- コマンドボタンの活性化 -----
                Me.CmdEnd.Enabled = True                    ' ENDﾎﾞﾀﾝは無条件に活性化する 
                If (stFNC(F_LOAD).iDEF = 1) Then            ' LOADﾎﾞﾀﾝ(1:選択可)
                    Me.CmdLoad.Enabled = True
                Else
                    Me.CmdLoad.Enabled = False
                End If
                If (stFNC(F_SAVE).iDEF = 1) Then            ' SAVEﾎﾞﾀﾝ
                    Me.CmdSave.Enabled = True
                Else
                    Me.CmdSave.Enabled = False
                End If
                If (stFNC(F_EDIT).iDEF = 1) Then            ' EDITﾎﾞﾀﾝ
                    Me.CmdEdit.Enabled = True
                Else
                    Me.CmdEdit.Enabled = False
                End If
                If (stFNC(F_LASER).iDEF = 1) Then           ' LASERﾎﾞﾀﾝ
                    Me.CmdLaser.Enabled = True
                Else
                    Me.CmdLaser.Enabled = False
                End If
                If (stFNC(F_LOG).iDEF = 1) Then             ' Logingﾎﾞﾀﾝ
                    Me.CmdLoging.Enabled = True
                Else
                    Me.CmdLoging.Enabled = False
                End If
                If (stFNC(F_PROBE).iDEF = 1) Then           ' Probeﾎﾞﾀﾝ
                    Me.CmdProbe.Enabled = True
                Else
                    Me.CmdProbe.Enabled = False
                End If
                If (stFNC(F_TEACH).iDEF = 1) Then           ' TEACHﾎﾞﾀﾝ
                    Me.CmdTeach.Enabled = True
                Else
                    Me.CmdTeach.Enabled = False
                End If
                If (stFNC(F_RECOG).iDEF = 1) Then           ' RECOGﾎﾞﾀﾝ
                    Me.CmdPattern.Enabled = True
                Else
                    Me.CmdPattern.Enabled = False
                End If

                ' TKY用
                ' '###246 If (gTkyKnd = KND_TKY) Then
                If (stFNC(F_CUTPOS).iDEF = 1) Then          ' CUTPOSﾎﾞﾀﾝ
                    Me.CmdCutPos.Enabled = True
                Else
                    Me.CmdCutPos.Enabled = False
                End If
                ' '###246           End If

                '----- V6.1.4.0⑩↓(KOA EW殿SL432RD対応)【ロット切替え機能】-----
                If (stFNC(F_AUTO).iDEF = 1) Then            ' AUTOボタン (SL432Rでも有効)
                    Me.CmdAutoOperation.Enabled = True
                Else
                    Me.CmdAutoOperation.Enabled = False
                End If
                '----- V6.1.4.0⑩↑ -----

                ' CHIP,NET用
                If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                    If gKeiTyp <> KEY_TYPE_RS Then              'V2.0.0.0⑩ シンプルトリマ以外
                        Me.chkDistributeOnOff.Enabled = True
                        Me.chkDistributeOnOff.Visible = True
                    End If                                      'V2.0.0.0⑩

                    If (stFNC(F_TX).iDEF = 1) Then              ' TXﾎﾞﾀﾝ
                        Me.CmdTx.Enabled = True
                    Else
                        Me.CmdTx.Enabled = False
                    End If
                    If (stFNC(F_TY).iDEF = 1) Then              ' TYﾎﾞﾀﾝ
                        Me.CmdTy.Enabled = True
                    Else
                        Me.CmdTy.Enabled = False
                    End If

                    Me.CmdIntegrated.Enabled = (1 <= stFNC(F_INTEGRATED).iDEF)  ' 統合登録調整ボタン 'V4.10.0.0③

                    'Option functions
                    If (stFNC(F_TY2).iDEF = 1) Then             ' TY2ﾎﾞﾀﾝ
                        Me.CmdTy2.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdTy2.Enabled = False
                    End If
                    If (stFNC(F_EXR1).iDEF = 1) Then         ' 外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ  
                        Me.CmdExCam.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdExCam.Enabled = False
                    End If
                    If (stFNC(F_EXTEACH).iDEF = 1) Then         ' 外部ｶﾒﾗﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ  
                        Me.CmdExCam1.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdExCam1.Enabled = False
                    End If
                    If (Me.CmdExCam1.Enabled = False And Me.CmdExCam.Enabled = False) Then
                        'ラベル表示も無効にする
                        lblExCamera.Visible = False
                    Else
                        lblExCamera.Visible = True              ' ###103
                    End If
                    If (stFNC(F_CARREC).iDEF = 1) Then          ' ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録ﾎﾞﾀﾝ  
                        Me.CmdPtnCalibration.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdPtnCalibration.Enabled = False
                    End If
                    If (stFNC(F_CAR).iDEF = 1) Then             ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾎﾞﾀﾝ
                        Me.CmdCalibration.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdCalibration.Enabled = False
                    End If
                    If (Me.CmdPtnCalibration.Enabled = False And Me.CmdCalibration.Enabled = False) Then
                        'ラベル表示も無効にする
                        lblCalibration.Visible = False
                    Else
                        lblCalibration.Visible = True           ' ###103
                    End If
                    If (stFNC(F_CUTREC).iDEF = 1) Then          ' ｶｯﾄ補正登録ﾎﾞﾀﾝ
                        Me.CmdPtnCutPosCorrect.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdPtnCutPosCorrect.Enabled = False
                    End If
                    If (stFNC(F_CUTREV).iDEF = 1) Then          ' ｶｯﾄ位置補正ﾎﾞﾀﾝ
                        Me.CmdCutPosCorrect.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdCutPosCorrect.Enabled = False
                    End If
                    If (Me.CmdPtnCutPosCorrect.Enabled = False And Me.CmdCutPosCorrect.Enabled = False) Then
                        'ラベル表示も無効にする
                        lblCutPos.Visible = False
                    Else
                        lblCutPos.Visible = True                ' ###103
                    End If
                    If (stFNC(F_TTHETA).iDEF = 1) Then          ' Tθﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
                        Me.CmdT_Theta.Enabled = True
                        isDispOptGrp = True
                    Else
                        Me.CmdT_Theta.Enabled = False
                    End If
                End If
                '#4.12.3.0④↓
                'TKYでもSL436タイプで自動運転する場合があるのでこの条件はCHIP、NETの種別から外して全部共通とする
                ' SL436R専用のコマンド
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    If (stFNC(F_AUTO).iDEF = 1) Then            ' AUTOボタン
                        Me.CmdAutoOperation.Enabled = True
                    Else
                        Me.CmdAutoOperation.Enabled = False
                    End If
                    If (stFNC(F_LOADERINI).iDEF = 1) Then       ' LOADER INIボタン
                        Me.CmdLoaderInit.Enabled = True
                    Else
                        Me.CmdLoaderInit.Enabled = False
                    End If
                End If
                '#4.12.3.0④↑

                '----- V1.13.0.0③↓ -----
                ' TKY系オプション
                If (stFNC(F_APROBEREC).iDEF = 1) Then                   ' ｵｰﾄプローブ登録ﾎﾞﾀﾝ 
                    Me.CmdAotoProbePtn.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbePtn.Enabled = False
                End If
                If (stFNC(F_APROBEEXE).iDEF = 1) Then                   ' ｵｰﾄプローブ実行ﾎﾞﾀﾝ
                    Me.CmdAotoProbeCorrect.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbeCorrect.Enabled = False
                End If
                If (Me.CmdAotoProbePtn.Enabled = False And Me.CmdAotoProbeCorrect.Enabled = False) Then
                    'ラベル表示も無効にする
                    lblAutoProbe.Visible = False
                Else
                    lblAutoProbe.Visible = True
                End If
                If (stFNC(F_IDTEACH).iDEF = 1) Then                     ' IDﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
                    Me.CmdIDTeach.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdIDTeach.Enabled = False
                End If
                If (Me.CmdIDTeach.Enabled = False) Then
                    'ラベル表示も無効にする
                    lblSinsyuku.Visible = False
                Else
                    lblSinsyuku.Visible = True
                End If
                If (stFNC(F_SINSYUKU).iDEF = 1) Then                    ' 伸縮登録ﾎﾞﾀﾝ 
                    Me.CmdSinsyukuPtn.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdSinsyukuPtn.Enabled = False
                End If
                If (stFNC(F_SINSYUKU).iDEF = 1) Then                    ' MAPﾎﾞﾀﾝ 
                    Me.CmdSinsyukuPtn.Enabled = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdMap.Enabled = False
                End If
                '----- V1.13.0.0③↑ -----

                'V4.1.0.0⑤
                If stFNC(F_PROBE_CLEANING).iDEF >= 1 Then
                    ProbeBtnEnableSet(True)
                    Me.CmdT_ProbeCleaning.Enabled = True                'V4.10.0.0⑨
                    'V5.0.0.9⑫↓
                    If (gMachineType = MACHINE_TYPE_436S) Then
                    Else
                        isDispOptGrp3 = True                                'V4.10.0.0⑨
                    End If
                    'V5.0.0.9⑫↑
                Else
                    ProbeBtnEnableSet(False)
                End If
                'V4.1.0.0⑤
                'V4.1.0.0⑥
                If stFNC(F_MAINTENANCE).iDEF >= 1 Then
                    MaintBtnEnableSet(True)
                Else
                    MaintBtnEnableSet(False)
                End If
                'V4.1.0.0⑥

                'V6.1.4.0⑥         ↓
                If (1S <= stFNC(F_FOLDEROPEN).iDEF) Then                ' ﾌｫﾙﾀﾞ表示ﾎﾞﾀﾝ
                    Me.CmdFolderOpen.Enabled = True
                    isDispOptGrp = True
                Else
                    Me.CmdFolderOpen.Enabled = False
                End If
                'V6.1.4.0⑥         ↑

                ' 活性化時は表示する
                mnuHelpAbout.Visible = True
                btnGoClipboard.Visible = False                          '###036
                Me.tabCmd.Visible = True
                Me.CmdEnd.Visible = True
                '----- V1.18.0.0③↓ -----
                'If gSysPrm.stCTM.giGP_IB_flg <> 0 Then                  ' GP-IB制御機能あり ? '###006
                '    If (giIX2LOG = 1) Then                              '###231
                '        CMdIX2Log.Visible = True                        '「IX2Log On/OFF」ﾎﾞﾀﾝ表示
                '    End If
                'End If
                If (giIX2LOG = 1) Then                                  ' IX2Logボタン有効 ?
                    Me.CMdIX2Log.Enabled = True
                Else
                    Me.CMdIX2Log.Enabled = False
                End If
                If (giPrint = 1) Then                                   ' Printボタン有効 ?
                    Me.BtnPrint.Enabled = True
                    Me.BtnPrintOnOff.Enabled = True                     ' Print On/Offボタン
                Else
                    Me.BtnPrint.Enabled = False
                    Me.BtnPrintOnOff.Enabled = False
                End If
                '----- V1.18.0.0③↑ -----
                If gSysPrm.stLOG.gEsLogUse = 1 Then                     ' ESログ使用可 ? '###006
                    cmdEsLog.Visible = True                             ' ESﾛｸﾞﾎﾞﾀﾝ表示
                End If

                ' マガジン上下動作　有効
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then   ' SL436R系 ? ###182
                    GroupBox1.Visible = True
                End If

                'V4.7.3.5①↓
                If (gTkyKnd = KND_CHIP) Then
                    CutOffEsEditButton.Visible = True
                    CutOffEsEditButton.Enabled = True
                Else
                    CutOffEsEditButton.Visible = False
                    CutOffEsEditButton.Enabled = False
                End If
                'V4.7.3.5①↑
                'V6.1.4.14①↓
                If (gTkyKnd = KND_NET) Then
                    CutOffEsEditButtonNET.Visible = True
                    CutOffEsEditButtonNET.Enabled = True
                Else
                    CutOffEsEditButtonNET.Visible = False
                    CutOffEsEditButtonNET.Enabled = False
                End If
                'V6.1.4.14①↑

            ElseIf (Flg = 0) Then
                '---------------------------------------------------------------------------
                '   ボタンを非活性化する
                '---------------------------------------------------------------------------
                Me.btnCounterClear.Enabled = False          ' CLRﾎﾞﾀﾝ
                Me.btnTrimming.Enabled = False              ' GOボタン
                Me.CbDigSwH.Enabled = False                 ' DisplayModeコンボボックスリスト
                Me.CbDigSwL.Enabled = False                 ' MoveModeコンボボックスリスト
                '----- コマンドボタンの非活性化 -----
                Me.CmdEnd.Enabled = False
                Me.CmdLoad.Enabled = False
                Me.CmdSave.Enabled = False
                Me.CmdEdit.Enabled = False
                Me.CmdLaser.Enabled = False
                Me.CmdLoging.Enabled = False
                Me.CmdProbe.Enabled = False
                Me.CmdTeach.Enabled = False
                Me.CmdPattern.Enabled = False
                Me.CmdCutPos.Enabled = False
                ' CHIP,NET用
                Me.chkDistributeOnOff.Enabled = False
                Me.chkDistributeOnOff.Visible = False
                Me.CmdTx.Enabled = False
                Me.CmdTy.Enabled = False
                Me.CmdExCam.Enabled = False
                Me.CmdExCam1.Enabled = False
                Me.CmdPtnCalibration.Enabled = False
                Me.CmdCalibration.Enabled = False
                Me.CmdPtnCutPosCorrect.Enabled = False
                Me.CmdCutPosCorrect.Enabled = False
                Me.CmdT_Theta.Enabled = False
                Me.CmdTy2.Enabled = False
                Me.CmdAutoOperation.Enabled = False         ' AUTOボタン 
                Me.CmdLoaderInit.Enabled = False            ' LOADER INIボタン
                Me.CmdIntegrated.Enabled = False            ' 統合登録調整ボタン 'V4.10.0.0③
                '----- V1.13.0.0③↓ -----
                ' TKY系オプション
                Me.CmdAotoProbePtn.Enabled = False                      ' ｵｰﾄプローブ登録ﾎﾞﾀﾝ
                Me.CmdAotoProbeCorrect.Enabled = False                  ' ｵｰﾄプローブ実行ﾎﾞﾀﾝ
                Me.CmdIDTeach.Enabled = False                           ' IDﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
                Me.CmdSinsyukuPtn.Enabled = False                       ' 伸縮登録ﾎﾞﾀﾝ 
                '----- V1.13.0.0③↑ -----
                '----- V1.18.0.0③↓ -----
                ' ローム殿対応
                Me.CMdIX2Log.Enabled = False                            ' IX2Logボタン
                Me.BtnPrint.Enabled = False                             ' Printボタン
                Me.BtnPrintOnOff.Enabled = False                        ' Print On/Offボタン
                '----- V1.18.0.0③↑ -----

                '無効時は表示も消す
                mnuHelpAbout.Visible = False
                btnGoClipboard.Visible = False
                'CMdIX2Log.Visible = False                   ' V1.13.0.0③ ###006
                cmdEsLog.Visible = False                    ' ###006
                Me.tabCmd.Visible = False
                Me.CmdEnd.Visible = False

                ' マガジン上下動作　無効
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R系 ? ###182
                    GroupBox1.Visible = False
                End If

                ' ログオンユーザー表示・切替ボタン
                SetBtnUserLogon(False)

                Me.CmdT_ProbeCleaning.Enabled = False       'V4.10.0.0⑨

                '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応)【ＱＲコードリード機能】 -----
                If (giQrCodeType = QrCodeType.KoaEw) Then
                    ' ＱＲ読み込み状態の初期化
                    ObjQRCodeReader.ResetQRReadFlag()                   ' ＱＲコード未読み込み状態にリセット
                End If
                '----- V6.1.4.0_22↑ -----

                'V4.7.3.5①↓
                If (gTkyKnd = KND_CHIP) Then
                    CutOffEsEditButton.Enabled = False
                Else
                    CutOffEsEditButton.Visible = False
                    CutOffEsEditButton.Enabled = False
                End If
                'V4.7.3.5①↑
                'V6.1.4.14①↓
                If (gTkyKnd = KND_NET) Then
                    CutOffEsEditButtonNET.Enabled = False
                Else
                    CutOffEsEditButtonNET.Visible = False
                    CutOffEsEditButtonNET.Enabled = False
                End If
                'V6.1.4.14①↑

            ElseIf (Flg = 2) Then
                '---------------------------------------------------------------------------
                '   ボタンの表示/非表示を設定する(ctlのbit1 ON時)
                '---------------------------------------------------------------------------
                Me.CmdEnd.Visible = True                    ' ENDﾎﾞﾀﾝは無条件に表示する 
                Me.btnTrimming.Visible = False              ' GOボタン
                If (stFNC(F_LOAD).iDEF >= 0) Then           ' LOADﾎﾞﾀﾝ
                    Me.CmdLoad.Visible = True
                Else
                    Me.CmdLoad.Visible = False
                End If
                If (stFNC(F_SAVE).iDEF >= 0) Then           ' SAVEﾎﾞﾀﾝ
                    Me.CmdSave.Visible = True
                Else
                    Me.CmdSave.Visible = False
                End If
                If (stFNC(F_EDIT).iDEF >= 0) Then           ' EDITﾎﾞﾀﾝ
                    Me.CmdEdit.Visible = True
                Else
                    Me.CmdEdit.Visible = False
                End If
                If (stFNC(F_LASER).iDEF >= 0) Then          ' LASERﾎﾞﾀﾝ
                    Me.CmdLaser.Visible = True
                Else
                    Me.CmdLaser.Visible = False
                End If
                If (stFNC(F_LOG).iDEF >= 0) Then            ' Logingﾎﾞﾀﾝ
                    Me.CmdLoging.Visible = True
                Else
                    Me.CmdLoging.Visible = False
                End If
                If (stFNC(F_PROBE).iDEF >= 0) Then          ' Probeﾎﾞﾀﾝ
                    Me.CmdProbe.Visible = True
                Else
                    Me.CmdProbe.Visible = False
                    Me.lblIntegProbe.Visible = False        'V4.10.0.0③
                End If
                If (stFNC(F_TEACH).iDEF >= 0) Then          ' TEACHﾎﾞﾀﾝ
                    Me.CmdTeach.Visible = True
                Else
                    Me.CmdTeach.Visible = False
                    Me.lblIntegTeach.Visible = False        'V4.10.0.0③
                End If
                If (stFNC(F_RECOG).iDEF >= 0) Then          ' RECOGﾎﾞﾀﾝ
                    If (gSysPrm.stDEV.giTheta = 0) Then     ' θなし ? 
                        Me.CmdPattern.Visible = False
                        Me.lblIntegRecog.Visible = False    'V4.10.0.0③
                    Else
                        Me.CmdPattern.Visible = True
                    End If
                Else
                    Me.CmdPattern.Visible = False
                    Me.lblIntegRecog.Visible = False        'V4.10.0.0③
                End If
                ' TKY用
                ''###246 If (gTkyKnd = KND_TKY) Then
                If (stFNC(F_CUTPOS).iDEF >= 0) Then         ' CUTPOSﾎﾞﾀﾝ
                    Me.CmdCutPos.Visible = True
                Else
                    Me.CmdCutPos.Visible = False
                End If
                ''###246  End If

                '----- V1.13.0.0③↓ -----
                ' TKY系オプション
                If (stFNC(F_APROBEREC).iDEF >= 0) Then              ' ｵｰﾄプローブ登録ﾎﾞﾀﾝ 
                    Me.CmdAotoProbePtn.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbePtn.Visible = False
                End If
                If (stFNC(F_APROBEEXE).iDEF >= 0) Then              ' ｵｰﾄプローブ実行ﾎﾞﾀﾝ
                    Me.CmdAotoProbeCorrect.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdAotoProbeCorrect.Visible = False
                End If
                If (stFNC(F_IDTEACH).iDEF >= 0) Then                ' IDﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
                    Me.CmdIDTeach.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdIDTeach.Visible = False
                End If
                If (stFNC(F_SINSYUKU).iDEF >= 0) Then               ' 伸縮登録ﾎﾞﾀﾝ 
                    Me.CmdSinsyukuPtn.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdSinsyukuPtn.Visible = False
                End If
                If (stFNC(F_MAP).iDEF >= 0) Then                    ' MAPﾎﾞﾀﾝ 
                    Me.CmdMap.Visible = True
                    isDispOptGrp2 = True
                Else
                    Me.CmdMap.Visible = False
                End If
                '----- V1.13.0.0③↑ -----
                '----- V6.1.4.0⑩↓(KOA EW殿SL432RD対応)【ロット切替え機能】-----
                If (stFNC(F_AUTO).iDEF >= 0) Then               ' AUTOボタン (SL432Rでも有効)
                    Me.CmdAutoOperation.Visible = True
                Else
                    Me.CmdAutoOperation.Visible = False
                End If
                '----- V6.1.4.0⑩↑ -----

                ' CHIP,NET用
                If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                    If gKeiTyp <> KEY_TYPE_RS Then              'V2.0.0.0⑩ シンプルトリマ以外
                        Me.chkDistributeOnOff.Visible = True
                    End If                                      'V2.0.0.0⑩

                    If (stFNC(F_TX).iDEF >= 0) Then             ' TXﾎﾞﾀﾝ
                        Me.CmdTx.Visible = True
                    Else
                        Me.CmdTx.Visible = False
                        Me.lblIntegTX.Visible = False           'V4.10.0.0③
                    End If
                    If (stFNC(F_TY).iDEF >= 0) Then             ' TYﾎﾞﾀﾝ
                        Me.CmdTy.Visible = True
                    Else
                        Me.CmdTy.Visible = False
                        Me.lblIntegTY.Visible = False           'V4.10.0.0③
                    End If
                    If (stFNC(F_TY2).iDEF >= 0) Then            ' TY2ﾎﾞﾀﾝ
                        Me.CmdTy2.Visible = True
                        isDispOptGrp = True
                    Else
                        Me.CmdTy2.Visible = False
                    End If

                    ' 統合登録調整ﾎﾞﾀﾝ 'V4.10.0.0③     ↓
                    If (0 <= stFNC(F_INTEGRATED).iDEF) AndAlso ((0 <= stFNC(F_PROBE).iDEF) OrElse
                        (0 <= stFNC(F_TEACH).iDEF) OrElse (0 <= stFNC(F_TX).iDEF) OrElse
                        (0 <= stFNC(F_TY).iDEF) OrElse (0 <= stFNC(F_RECOG).iDEF)) Then

                        Me.CmdIntegrated.Visible = True
                    Else
                        Me.CmdIntegrated.Visible = False
                    End If
                    ' 統合登録調整ﾎﾞﾀﾝ 'V4.10.0.0③     ↑

                    ' 外部カメラ有無により表示／非表示(CHIP/NET時)を設定する
                    If (gSysPrm.stDEV.giEXCAM = 0) Then
                        ' 外部カメラがない場合,下記のボタンは非表示とする
                        Me.CmdExCam.Visible = False             ' 外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ   
                        Me.CmdExCam1.Visible = False            ' 外部ｶﾒﾗﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
                        Me.CmdPtnCalibration.Visible = False    ' ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録ﾎﾞﾀﾝ
                        Me.CmdCalibration.Visible = False       ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾎﾞﾀﾝ
                        Me.CmdPtnCutPosCorrect.Visible = False  ' ｶｯﾄ補正登録ﾎﾞﾀﾝ 
                        Me.CmdCutPosCorrect.Visible = False     ' ｶｯﾄ位置補正ﾎﾞﾀﾝ 
                        Me.CmdT_Theta.Visible = False           ' Tθﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ 
                    Else
                        ' 外部カメラがある場合,設定ファイルの指定により表示／非表示
                        If (stFNC(F_EXR1).iDEF >= 0) Then    ' 外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ  
                            Me.CmdExCam.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdExCam.Visible = False
                        End If
                        If (stFNC(F_EXTEACH).iDEF >= 0) Then    ' 外部ｶﾒﾗﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ  
                            Me.CmdExCam1.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdExCam1.Visible = False
                        End If
                        If (stFNC(F_CARREC).iDEF >= 0) Then     ' ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録ﾎﾞﾀﾝ  
                            Me.CmdPtnCalibration.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdPtnCalibration.Visible = False
                        End If
                        If (stFNC(F_CAR).iDEF >= 0) Then        ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾎﾞﾀﾝ
                            Me.CmdCalibration.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdCalibration.Visible = False
                        End If
                        If (stFNC(F_CUTREC).iDEF >= 0) Then     ' ｶｯﾄ補正登録ﾎﾞﾀﾝ
                            Me.CmdPtnCutPosCorrect.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdPtnCutPosCorrect.Visible = False
                        End If
                        If (stFNC(F_CUTREV).iDEF >= 0) Then     ' ｶｯﾄ位置補正ﾎﾞﾀﾝ
                            Me.CmdCutPosCorrect.Visible = True
                            isDispOptGrp = True
                        Else
                            Me.CmdCutPosCorrect.Visible = False
                        End If
                        If (stFNC(F_TTHETA).iDEF >= 0) Then     ' Tθﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
                            If (gSysPrm.stDEV.giTheta = 0) Then ' θなし ? 
                                Me.CmdT_Theta.Visible = False
                            Else
                                Me.CmdT_Theta.Visible = True
                                isDispOptGrp = True
                            End If
                        Else
                            Me.CmdT_Theta.Visible = False
                        End If
                        If ((stFNC(F_RECOG_ROUGH).iDEF) >= 0) Then  ' ﾗﾌｱﾗﾒﾝﾄ用画像登録ﾎﾞﾀﾝ  'V5.0.0.9④  ↓
                            If (0 <> giClampLessStage) AndAlso (0 <> gSysPrm.stDEV.giTheta) Then ' クランプレス載物台 ? 
                                Me.CmdRecogRough.Visible = True
                                ' isDispOptGrp = True
                            Else
                                Me.CmdRecogRough.Visible = False
                            End If
                        End If                                                              'V5.0.0.9④  ↑
                    End If
                End If

                '#4.12.3.0④↓
                'TKYでもSL436タイプで自動運転する場合があるのでこの条件はCHIP、NETの種別から外して全部共通とする
                ' SL436R専用のコマンド
                If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
                    If (stFNC(F_AUTO).iDEF >= 0) Then            ' AUTOボタン 
                        Me.CmdAutoOperation.Visible = True
                    Else
                        Me.CmdAutoOperation.Visible = False
                    End If
                    If (stFNC(F_LOADERINI).iDEF >= 0) Then      ' LOADER INIボタン
                        Me.CmdLoaderInit.Visible = True
                    Else
                        Me.CmdLoaderInit.Visible = False
                    End If
                End If
                '#4.12.3.0④↑

                '----- V1.18.0.0③↓ -----
                ' ローム殿対応
                If (giIX2LOG = 1) Then                                  ' IX2Logボタン有効 ?
                    Me.CMdIX2Log.Visible = True
                Else
                    Me.CMdIX2Log.Visible = False
                End If
                If (giPrint = 1) Then                                   ' Printボタン有効 ?
                    Me.BtnPrint.Visible = True
                    Me.BtnPrintOnOff.Visible = True                     ' Print On/Offボタン
                Else
                    Me.BtnPrint.Visible = False
                    Me.BtnPrintOnOff.Visible = False
                End If
                '----- V1.18.0.0③↑ -----

                'V4.1.0.0⑤
                If stFNC(F_PROBE_CLEANING).iDEF >= 1 Then
                    ProbeBtnVisibleSet(True)
                    Me.CmdT_ProbeCleaning.Enabled = True                'V4.10.0.0⑨
                    Me.CmdT_ProbeCleaning.Visible = True                'V4.10.0.0⑨
                    'V5.0.0.9⑫↓
                    If (gMachineType = MACHINE_TYPE_436S) Then
                    Else
                        isDispOptGrp3 = True                                'V4.10.0.0⑨
                    End If
                    'V5.0.0.9⑫↑
                Else
                    ProbeBtnVisibleSet(False)
                End If
                'V4.1.0.0⑤
                'V4.1.0.0⑥
                If stFNC(F_MAINTENANCE).iDEF >= 1 Then
                    MaintBtnVisibleSet(True)
                Else
                    MaintBtnVisibleSet(False)
                End If
                'V4.1.0.0⑥

                'V6.1.4.0⑥         ↓
                If (1S <= stFNC(F_FOLDEROPEN).iDEF) Then                ' フォルダ表示ボタン
                    Me.CmdFolderOpen.Visible = True
                    Me.lblProductionData.Visible = True
                    isDispOptGrp = True
                Else
                    Me.CmdFolderOpen.Visible = False
                    Me.lblProductionData.Visible = False
                End If
                'V6.1.4.0⑥         ↑

                '---------------------------------------------------------------------------
                '   オプショングループの表示/非表示設定
                '---------------------------------------------------------------------------
                If (isDispOptGrp <> True) Then
                    Me.tabCmd.TabPages.Remove(Me.tabOptCmnds)
                End If
                '----- V1.13.0.0③↓ -----
                ' TKY系オプション
                If (isDispOptGrp2 <> True) Then
                    Me.tabCmd.TabPages.Remove(Me.tabOptCmnd2)
                End If
                '----- V1.13.0.0③↑ -----
                'V4.10.0.0⑨↓
                'V5.0.0.4⑧                If (isDispOptGrp3 <> True) Then
                'V6.0.0.1③                If (isDispOptGrp3 <> True) AndAlso (gMachineType = MACHINE_TYPE_436S) Then  'V5.0.0.9⑫
                If (isDispOptGrp3 <> True) OrElse (gMachineType = MACHINE_TYPE_436S) Then      'V6.0.0.1③
                    Me.tabCmd.TabPages.Remove(Me.tabOptCmnd3)
                End If
                'V5.0.0.4⑧            End If
                'V4.10.0.0⑨↑

                'V4.7.3.5①↓
                If (gTkyKnd = KND_CHIP) Then
                    CutOffEsEditButton.Visible = True
                    CutOffEsEditButton.Enabled = True
                Else
                    CutOffEsEditButton.Visible = False
                    CutOffEsEditButton.Enabled = False
                End If
                'V4.7.3.5①↑
                'V6.1.4.14①↓
                If (gTkyKnd = KND_NET) Then
                    CutOffEsEditButtonNET.Visible = True
                    CutOffEsEditButtonNET.Enabled = True
                Else
                    CutOffEsEditButtonNET.Visible = False
                    CutOffEsEditButtonNET.Enabled = False
                End If
                'V6.1.4.14①↑

            ElseIf (Flg = 3) Then
                '---------------------------------------------------------------------------
                '   「SAVE」ﾎﾞﾀﾝと「END」ﾎﾞﾀﾝのみ有効とする
                '---------------------------------------------------------------------------
                Me.CmdSave.Enabled = True                   '「SAVE」ﾎﾞﾀﾝ
                Me.CmdSave.Visible = True
                Me.CmdEnd.Enabled = True                    '「END」ﾎﾞﾀﾝ
                Me.CmdEnd.Visible = True
                Me.btnCounterClear.Enabled = True           '「CLR」ﾎﾞﾀﾝ
                Me.btnCounterClear.Visible = True

            ElseIf (Flg = 4) Then
                '---------------------------------------------------------------------------
                '   「LOAD」/「ﾛｷﾞﾝｸﾞ」/「ﾛｰﾀﾞ原点復帰」/「END」ﾎﾞﾀﾝのみ有効とする
                '---------------------------------------------------------------------------
                Me.btnTrimming.Visible = False                          ' GOボタン
                Me.CmdSave.Visible = False
                Me.CmdEdit.Visible = False
                Me.CmdLaser.Visible = False
                Me.CmdProbe.Visible = False
                Me.CmdTeach.Visible = False
                Me.CmdPattern.Visible = False
                Me.CmdCutPos.Visible = False
                ' CHIP,NET用
                Me.CmdTx.Visible = False
                Me.CmdTy.Visible = False
                Me.CmdExCam.Visible = False
                Me.CmdExCam1.Visible = False
                Me.CmdPtnCalibration.Visible = False
                Me.CmdCalibration.Visible = False
                Me.CmdPtnCutPosCorrect.Visible = False
                Me.CmdCutPosCorrect.Visible = False
                Me.CmdT_Theta.Visible = False
                Me.CmdTy2.Visible = False
                Me.CmdAutoOperation.Visible = False                     ' AUTOボタン 
                '----- V1.13.0.0③↓ -----
                ' TKY系オプション
                Me.CmdAotoProbePtn.Visible = False                      ' ｵｰﾄプローブ登録ﾎﾞﾀﾝ
                Me.CmdAotoProbeCorrect.Visible = False                  ' ｵｰﾄプローブ実行ﾎﾞﾀﾝ
                Me.CmdIDTeach.Visible = False                           ' IDﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ
                Me.CmdSinsyukuPtn.Visible = False                       ' 伸縮登録ﾎﾞﾀﾝ 
                '----- V1.13.0.0③↑ -----
                '----- V1.18.0.0③↓ -----
                ' ローム殿対応
                Me.CMdIX2Log.Visible = False                            ' IX2Logボタン
                Me.BtnPrint.Visible = False                             ' Printボタン
                Me.BtnPrintOnOff.Visible = False                        ' Print On/Offボタン
                '----- V1.18.0.0③↑ -----
                Me.CmdT_ProbeCleaning.Visible = False                   'V4.10.0.0⑨

                '「LOAD」/「ﾛｷﾞﾝｸﾞ」/「ﾛｰﾀﾞ原点復帰」/「END」ﾎﾞﾀﾝのみ有効とする
                Me.CmdLoad.Enabled = True
                Me.CmdLoad.Visible = True
                Me.CmdEnd.Enabled = True                    '「END」ﾎﾞﾀﾝ
                Me.CmdEnd.Visible = True
                Me.CmdLoging.Enabled = True                 '「Loging」ﾎﾞﾀﾝ
                Me.CmdLoging.Visible = True
                Me.CmdLoaderInit.Enabled = True             '「LOADER INI」ﾎﾞﾀﾝ
                Me.CmdLoaderInit.Visible = True
            End If
            'V2.0.0.0⑩ ADD START
            If Flg <> 0 Then
                Call SimpleTrimmer.ResistorDataDisp(False, 0, 0)
            End If
            'V2.0.0.0⑩ ADD END

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Form1Button() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "画像登録用にメインフォームのコントロールを表示/非表示に設定する"
    '''=========================================================================
    '''<summary>画像登録用にメインフォームのコントロールを表示/非表示に設定する</summary>
    '''<param name="Flg">(INP)True=表示, False=非表示</param>
    '''<remarks>Video.OCXは最背面にいる為、メインフォームのコントロールの下
    ''' 　　　　に登録画面が表示されるため</remarks>
    '''=========================================================================
    Public Sub SetVisiblePropForVideo(ByVal Flg As Boolean)

        Dim strMSG As String

        Try
            ' 画像登録用にメインフォームのコントロールを表示/非表示に設定する
            txtLog.Visible = Flg
            '' '' ''grpCmd.Visible = Flg
            GrpMode.Visible = Flg
            'btnTrimming.Visible = Flg '###036

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetVisiblePropForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "レーザパワー調整関連項目の表示/非表示設定"
    '''=========================================================================
    '''<summary>レーザパワー調整関連項目の表示/非表示設定 ###029</summary>
    ''' <param name="Md">(INP)0=表示しない, 1=表示する</param>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetLaserItemsVisible(ByVal Md As Integer)

        Dim strMSG As String

        Try
            ' 減衰率をシスパラより表示する("減衰率 = 99.9%")
            Me.LblRotAtt.Visible = False                                ' 減衰率非表示
            If (Md = 1) Then                                            ' 表示する ?
                If (gSysPrm.stRMC.giRmCtrl2 >= 2 And
                    gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then       ' ﾛｰﾀﾘｱｯﾃﾈｰﾀ制御有でFLでない ?
                    '----- V1.16.0.0⑭↓ -----
                    'Me.LblRotAtt.Visible = True
                    ' 減衰率の表示/非表示をシスパラより設定する
                    If (giNoDspAttRate = 1) Then
                        Me.LblRotAtt.Visible = False
                    Else
                        Me.LblRotAtt.Visible = True
                    End If
                    '----- V1.16.0.0⑭↑ -----
                End If

                '' FL時のオートパワー調整用電流値表示(FL時は減衰率の代わりに電流値を表示する) ###066
                '' ※FLでパワーメータのデータ取得タイプが「Ｉ／Ｏ読取り」/「ＵＳＢ」で「パワー測定値表示あり」の場合に表示する
                'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And _
                '   (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                '    Me.LblRotAtt.Visible = True
                'End If
            End If

            ' 測定値をシスパラより表示する
            Me.LblMes.Visible = False                                   ' 測定値非表示
            If (Md = 1) Then                                            ' 表示する ?
                ' RMCTRL2 >=3 で 測定値表示 ?
                If (gSysPrm.stRMC.giRmCtrl2 >= 3) And (gSysPrm.stRMC.giPMonHi = 1) Then
                    LblMes.Visible = True                              ' 測定値表示
                End If

                '' レーザパワー設定値表示(FL時) ###066
                '' ※FLでパワーメータのデータ取得タイプが「Ｉ／Ｏ読取り」/「ＵＳＢ」で「パワー測定値表示あり」の場合に表示する
                'If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) And _
                '   (gSysPrm.stIOC.giPM_DataTp <> PM_DTTYPE_NONE) And (gSysPrm.stRMC.giPMonHi = 1) Then
                '    LblMes.Visible = True                               ' 設定値表示
                'End If
            End If

            ' 定電流値を表示する
            LblCur.Visible = False                                      ' 定電流値非表示
            If (Md = 1) Then                                            ' 表示する ?
                ' 加工電力設定 = 4(定電流1A)の時に表示
                'If (gSysPrm.stSPF.giProcPower = 4) And (gSysPrm.stSPF.giProcPower2 <> 0) Then  '###250
                If (gSysPrm.stSPF.giProcPower2 <> 0) Then                                       '###250
                    LblCur.Visible = True
                End If
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "SetLaserItemsVisible() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "パスワードチェック"
    '''=========================================================================
    '''<summary>パスワードチェック</summary>
    '''<param name="IntIndexNo">(INP)機能選択定義テーブルのｲﾝﾃﾞｯｸｽ</param>
    '''<returns>True=正常, False=エラー</returns>
    '''=========================================================================
    Public Function Func_Password(ByRef IntIndexNo As Short) As Short

        Dim r As Short

        Func_Password = True
        If (stFNC(IntIndexNo).iPAS = 1) Then
            'V6.1.4.9⑧分布図が表示されていたら非表示にする↓
            Call RedrawDisplayDistribution(True)    '統計表示処理の状態変更'V6.1.4.9⑧
            r = Password1.ShowDialog((gSysPrm.stTMN.giMsgTyp), (gSysPrm.stSYP.gstrPassword))
            If (r <> 1) Then
                Func_Password = False                   ' ﾊﾟｽﾜｰﾄﾞ入力ｴﾗｰならEXIT
                'V4.7.3.5①↓
                If r <> cFRS_NORMAL And r <> cFRS_ERR_RST Then
                    Call Me.AppEndDataSave()
                    Call Me.AplicationForcedEnding()                         ' アプリ強制終了
                End If
                'V4.7.3.5①↑
            End If
            Call RedrawDisplayDistribution(False)   'V6.1.4.9⑧
        End If

    End Function
#End Region

#Region "ソフトウェアキーボードの起動処理"
    '#If cSOFTKYBOARDcUSE = 1 Then
    '''=========================================================================
    '''<summary>ソフトウェアキーボードの起動処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub StartSoftwareKeyBoard(ByRef ps As Process)
        Try
            If (giDspScreenKeybord = 1) Then Return ' クリーンキーボード表示しないならNOP V2.0.0.0⑦(V1.22.0.0⑧)
            ps.StartInfo.FileName = "osk.exe"
            ps.Start()
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "iTKY.StartSoftwareKeyBoard() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "ソフトウェアキーボードの終了処理"
    '''=========================================================================
    '''<summary>ソフトウェアキーボードの終了処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub EndSoftwareKeyBoard(ByRef ps As Process)
        Try
            If (giDspScreenKeybord = 1) Then Return ' クリーンキーボード表示しないならNOP V2.0.0.0⑦(V1.22.0.0⑧)
            '----- ###259↓-----
            ' プロセス終了メッセージ送信(ｳｨﾝﾄﾞｳのあるｱﾌﾟﾘの場合) 
            If (ps.CloseMainWindow() = False) Then      ' ﾒｲﾝｳｨﾝﾄﾞｳにｸﾛｰｽﾞﾒｯｾｰｼﾞを送信する
                ps.Kill()                               ' 終了しなかった場合は強制終了する
            End If

            ' プロセスの終了を待つ
            Do
                System1.WAIT(0.1)                       ' Wait(Sec) 
            Loop While (ps.HasExited <> True)           ' プロセスが終了している場合のみTrueが返る

            ' オブジェクト開放
            ps.Close()                                  ' オブジェクト開放
            ps.Dispose()                                ' リソース開放

            'If (ps.HasExited <> True) Then
            '    ps.Kill()
            'End If
            '----- ###259↑-----

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "iTKY.EndSoftwareKeyBoard() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
    '#End If
#End Region

    '========================================================================================
    '   コマンドボタン押下時の処理
    '========================================================================================
#Region "ﾌｧｲﾙﾛｰﾄﾞ(F1)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ﾌｧｲﾙﾛｰﾄﾞ(F1)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdLoad_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdLoad.Click

        Dim sPath As String
        Dim r As Integer
        Dim strMSG As String
        Dim result As System.Windows.Forms.DialogResult
        Dim NewFileName As String = ""       'V4.9.0.0②

        Try
            '-----------------------------------------------------------------------
            '   初期処理
            '-----------------------------------------------------------------------
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(True)

            ' トリマ装置状態を動作中に設定する
            sPath = ""
            r = TrimStateOn(F_LOAD, APP_MODE_LOAD, "", "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            '-----------------------------------------------------------------------
            '   読込ファイルパラメータの拡張子設定
            '-----------------------------------------------------------------------
            '----- V1.14.0.0⑥↓ -----
            ' DOS版の拡張子は表示しない
            If (gTkyKnd = KND_TKY) Then                                 ' TKYの場合
                comDlgOpen.Filter = "*.tdt|*.tdt|*.WDT|*.WDT"
            ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIPの場合
                'comDlgOpen.Filter = "*.tdc|*.tdc|*.WTC|*.WTC|*.WTC2|*.WTC2"            'V1.23.0.0⑧
                comDlgOpen.Filter = "*.tdc|*.tdc|*.WTC|*.WTC|*.WTC2|*.WTC2|*.wdc|*.wdc" 'V1.23.0.0⑧
            Else                                                        ' NETの場合
                comDlgOpen.Filter = "*.tdn|*.tdn|*.WTN|*.WTN"
            End If
            'If (gTkyKnd = KND_TKY) Then                                 ' TKYの場合
            '    comDlgOpen.Filter = "*.tdt|*.tdt|*.WDT|*.WDT|*.DAT|*.DAT"
            'ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIPの場合
            '    comDlgOpen.Filter = "*.tdc|*.tdc|*.WTC|*.WTC|*.WTC2|*.WTC2|*.DC|*.DC"
            'Else                                                        ' NETの場合
            '    comDlgOpen.Filter = "*.tdn|*.tdn|*.WTN|*.WTN|*.DAT|*.DAT"
            'End If
            '----- V1.14.0.0⑥↑ -----
            '----- V4.0.0.0③↓ -----
            If (giMachineKd = MACHINE_KD_RS) Then                           ' SL436S ? 
                ' SL436S用の拡張子を表示する(SL43xRのデータもリード可)
                If (gTkyKnd = KND_TKY) Then                                 ' TKYの場合
                    comDlgOpen.Filter = "*.tdts|*.tdts|*.tdt|*.tdt"
                ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIPの場合
                    comDlgOpen.Filter = "*.tdcs|*.tdcs|*.tdc|*.tdc"
                Else                                                        ' NETの場合
                    comDlgOpen.Filter = "*.tdns|*.tdns|*.tdn|*.tdn"
                End If
            End If
            '----- V4.0.0.0③↑ -----
            '#If cSOFTKYBOARDcUSE = 1 Then
            '-----------------------------------------------------------------------
            ' ソフトウェアキーボードを起動する
            '-----------------------------------------------------------------------
            Dim procHandle As Process
            procHandle = New Process
            Call StartSoftwareKeyBoard(procHandle)
            '#End If

            '-----------------------------------------------------------------------
            '   【ﾌｧｲﾙを開く】ﾀﾞｲｱﾛｸﾞを表示する
            '-----------------------------------------------------------------------
            comDlgOpen.ShowReadOnly = False
            comDlgOpen.CheckFileExists = True
            comDlgOpen.CheckPathExists = True
            comDlgOpen.InitialDirectory = gSysPrm.stDIR.gsTrimFilePath  ' トリムデータファイル格納位置
            'V6.1.4.14②↓' ＮＥＴの時のトリムデータファイル格納位置
            If gTkyKnd = KND_NET Then
                comDlgOpen.InitialDirectory = GetPrivateProfileString_S("QR_CODE", "TKYNET_TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA_NET\")
            End If
            'V6.1.4.14②↑

            'V6.0.0.0⑤            Call End_GazouProc(ObjGazou)    'V5.0.0.6⑭
            ' 【ﾌｧｲﾙを開く】ﾀﾞｲｱﾛｸﾞを表示する
            result = comDlgOpen.ShowDialog()

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' ソフトウェアキーボードを終了する
            Call EndSoftwareKeyBoard(procHandle)
            '#End If

            ' OK以外の場合
            If (result <> Windows.Forms.DialogResult.OK) Then
                GoTo STP_END                                            ' Cansel指定なら終了
            End If

            '-----------------------------------------------------------------------
            '   トリミングデータファイルを読込む
            '-----------------------------------------------------------------------
            ' ファイルバージョン名取得
            Call SetMousePointer(Me, True)                              ' 砂時計表示(ﾏｳｽﾎﾟｲﾝﾀ)
            sPath = comDlgOpen.FileName                                 ' トリミングデータファイル名
            r = Sub_FileLoad(sPath)                                     ' トリミングデータファイルを読込む 
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' Cancel ならReturn V4.0.0.0-34
            '----- V1.18.0.0②↓ -----
            ' データを手動でロードしたらQR情報を初期化する(ローム殿特注)
            If (r = cFRS_NORMAL) Then
                Call QR_Info_Disp(0)                                    ' QR情報を初期化する(ローム殿特注)
                Call BC_Info_Disp(0)                                    ' バーコード情報を初期化する(太陽社殿特注) V1.23.0.0①
                '----- V4.0.0.0⑳↓ -----
                '目標値、上限下限の表示
                SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)
                '----- V4.0.0.0⑳↑ -----
            End If
            '----- V1.18.0.0②↑ -----

            ' 'V4.9.0.0②↓
            If giChangePoint = 1 Then
                NewFileName = MakeutPointFileData(gStrTrimFileName)
                If System.IO.File.Exists(NewFileName) Then
                Else
                    ' ファイルがなかった場合には、デフォルトのファイルをコピーする
                    My.Computer.FileSystem.CopyFile(DEFAULT_CUTFILE, NewFileName)
                End If
            End If
            ' 'V4.9.0.0②↑

            '-----------------------------------------------------------------------
            '   終了処理
            '-----------------------------------------------------------------------
STP_END:
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)
            Call basTrimming.ChangeByTrimmingData()                     ' V5.0.0.6②　トリミングデータ更新時の処理
            Call Form1Button(2)                                         ' コマンドボタンを有効にする
            gManualThetaCorrection = True                               ' シータ補正実行フラグ = True(シータ補正を実行する)
            Call TrimStateOff()                                         ' トリマ装置状態をアイドル中に設定する

            '----- V6.0.3.0⑨↓ -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TrimData.SetLotChange()                                 ' ロット情報クリア
            End If
            '----- V6.0.3.0⑨↑ -----

            ' ガベージコレクションに強制的にメモリを開放させる
            '   デバッグ時のみ使用。高付加のため明示的には実行しない。
#If DEBUG Then
            GC.Collect()
#End If
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLoad_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try
    End Sub
#End Region

#Region "ファイルロード処理"
    '''=========================================================================
    '''<summary>ファイルロード処理</summary>
    '''<param name="sPath">(INP)ロードするファイル名</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function Sub_FileLoad(ByVal sPath As String) As Integer

        Dim r As Integer
        'Dim dblVer As Double                                           ' V4.0.0.0-28 _readFileVer, Field変数に変更
        Dim strXmlFName As String
        Dim strLDR As String
        Dim strMSG As String = ""
        Dim strPath As String = ""                                      ' V4.0.0.0-34
        Dim wreg As Short                                               ' V1.18.0.0⑦
        Dim GType As Integer                                            ' V1.18.0.0⑦
        Dim doSave As Boolean                                           ' V4.0.0.0-28

        Try
            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応)【ＱＲコードリード機能】 -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                Me.LblcLOTNUMBER.Text = ""                              ' 生産管理情報(伝票Ｎｏ．)
                Me.LblcRESVALUE.Text = ""                               ' 生産管理情報(抵抗値) 
            End If
            '----- V6.1.4.0_22↑ -----
            '----- V4.0.0.0-34↓ -----
            '-----------------------------------------------------------------------
            '   SL436Sで拡張子が".tdc"なら".tdcs"に変換後のファイルが存在しない事を確認する
            '-----------------------------------------------------------------------
            Call GetSaveFileName(sPath, strPath)                        ' ".tdc → .tdsd" 

            ' SL436Sで拡張子が".tdc"なら".tdcs"でファイルをセーブする
            strMSG = GetFileNameExtension(sPath)                        ' ファイル名から拡張子を取り出す 
            If ((giMachineKd = MACHINE_KD_RS) And (strMSG.Length = 4)) Then
                ' データファイルの存在チェック
                If (System.IO.File.Exists(strPath) = True) Then
                    strMSG = strPath + "" + MSG_159
                    ' "ファイルが存在します。上書きしても宜しいですか？"
                    r = MsgBox(strMSG, MsgBoxStyle.OkCancel, "")
                    If (r <> MsgBoxResult.Ok) Then
                        Return (cFRS_ERR_RST)
                    End If
                End If
                doSave = True                                           ' セーブはFLへの転送後におこなう V4.0.0.0-28
            End If
            '----- V4.0.0.0-34↑    -----

            '-----------------------------------------------------------------------
            '   トリミングデータファイルを読込む
            '-----------------------------------------------------------------------
            ' ログ出力用
            If (gbFgAutoOperation = True) Then                          ' 自動運転フラグ(True:自動運転中, False:自動運転中でない) 
                strLDR = "' AUTO"
            Else
                strLDR = "' MANUAL"
            End If

            ' ファイルバージョン名取得
            r = File_Read_Ver(sPath, _readFileVer)
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            ' トリミングデータファイルを読込む
            If (_readFileVer >= FileIO.FILE_VER_10) Then
                ' 新バージョン以降の場合 
                r = File_Read(sPath)
            Else
                ' 旧バージョンの場合(TKY時)
                If (gTkyKnd = KND_TKY) Then
                    'V5.0.0.8①                    r = DatConv_TKY(sPath)                              ' DATファイル変換
                    'V5.0.0.8①                    If (r <> 0) Then GoTo STP_ERR
                    r = File_Read_Tky(sPath)                            ' トリムデータ読込み(TKY)
                End If
                ' 旧バージョンの場合(CHIP/NET時)
                If (gTkyKnd = KND_CHIP) Or (gTkyKnd = KND_NET) Then
                    'V5.0.0.8①                    r = DatConv_CHIPNET(sPath)                          ' DATファイル変換
                    'V5.0.0.8①                    If (r <> 0) Then GoTo STP_ERR
                    r = FileLoadExe(sPath)                              ' トリムデータ読込み(CHIP/NET)
                End If
                typPlateInfo.strDataName = sPath                        ' トリミングデータ名設定 
            End If

            ' データファイルの読込み失敗時
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            '----- V4.0.0.0-34↓ -----
            'typPlateInfo.strDataName = sPath                           ' トリミングデータ名設定 V1.16.0.2③
            typPlateInfo.strDataName = strPath                           ' トリミングデータ名設定
            sPath = typPlateInfo.strDataName
            '----- V4.0.0.0-34↑    -----

            ' GPIBコマンドを設定する ###002
            'V5.0.0.8①            Call SetGpibCommand(typPlateInfo)
            DataManager.SetGpibCommand()                                'V5.0.0.8①

            ' 読み込んだデータに同一FL加工条件番号で異なる目標パワー・許容範囲が保存されている場合があるため
            ' 同一加工条件番号の場合は自動パワー調整で使用される値に変更する
            DataManager.SetNormalizedArrCutFLData()                     '#5.0.0.8①

            '-----------------------------------------------------------------------
            '   操作ログ等を出力する
            '-----------------------------------------------------------------------
            ' データファイルの読込み成功時
            If (gTkyKnd = KND_CHIP) Then
                Call SetEsType(sPath)                                   ' ESのタイプを設定
                Call ChkCutTypeEs()                                     ' ESの置き換え
            End If

            '----- V6.1.4.0⑦↓(KOA EW殿SL432RD対応) -----
            ' ファイルパス名の表示
            'LblDataFileName.Text = Form1_006 & sPath
            'gStrTrimFileName = sPath                                    ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名をｸﾞﾛｰﾊﾞﾙ変数に設定する
            'LblDataFileName.Text = sPath
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = sPath                             ' トリミングデータ名と第１抵抗データ表示域を設定する 
            Else
                LblDataFileName.Text = sPath                            ' トリミングデータ名表示
            End If
            gStrTrimFileName = sPath                                    ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名をｸﾞﾛｰﾊﾞﾙ変数に設定する
            '----- V6.1.4.0⑦↑ -----
            gLoadDTFlag = True                                          ' ﾃﾞｰﾀﾛｰﾄﾞ済ﾌﾗｸﾞ(False:ﾃﾞｰﾀ未ﾛｰﾄﾞ, True:ﾃﾞｰﾀﾛｰﾄﾞ済)
            commandtutorial.SetDataLoad()                               'V2.0.0.0⑩ データロード状態設定

            '----- オートプローブ用オフセット初期化 -----
            gfStgOfsX = 0.0                                             ' XYテーブルオフセットX(mm) ※オートプローブ実行コマンド(FrmMatrix())で設定
            gfStgOfsY = 0.0                                             ' XYテーブルオフセットY(mm)
            '----- V1.13.0.0③↑ -----

            ' システム変数設定(プローブON/OFF位置他) 
            Call PROP_SET(typPlateInfo.dblZOffSet, typPlateInfo.dblZWaitOffset, gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, gSysPrm.stDEV.gfSmaxX, gSysPrm.stDEV.gfSmaxY)
            '----- V1.15.0.0③↓ -----
            ' INtime側のZOFF位置を設定する
            'r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)                 ' INtime側のZOFF位置をzWaitPosとする 'V1.15.0.0④
            r = SETZOFFPOS(typPlateInfo.dblZWaitOffset)                  ' INtime側のZOFF位置をzWaitPosとする 'V1.15.0.0④
            r = System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r <> cFRS_NORMAL) Then                                  ' エラーならエラーリターン(メッセージ表示済み)
                Return (r)
            End If
            '----- V1.15.0.0③↑ -----

            '----- V4.0.0.0-28 ↓ -----  FL側への加工条件転送後におこなう
            ''----- V2.0.0.0_26↓ -----
            ''-----------------------------------------------------------------------
            ''   加工条件番号からカットデータのQレート,電流値,STEG本数を設定する
            ''   (シンプルトリマ用(FL時で加工条件ファイルがある場合))
            ''-----------------------------------------------------------------------
            'r = SetCutDataCndInfFromCndNum(dblVer)
            ''----- V2.0.0.0_26↑ -----
            '----- V4.0.0.0-28 ↑ -----

            ' ログ出力("データロード")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC01, "File='" & sPath & strLDR)
            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応)【ＱＲコードリード機能】 -----
            'Call Z_CLS()                                                ' データロードでログ画面クリア
            If (giQrCodeType = QrCodeType.KoaEw) Then
                If (QR_Read_Flg = 0) Then                               ' ＱＲコード表示中はクリアしない
                    Call Z_CLS()                                        ' データロードでログ画面クリア
                End If
                ' 生産管理情報の伝票Ｎｏ.と抵抗値を表示する
                If (typQRDATAInfo.bStatus = True) Then
                    ObjQRCodeReader.LotNumberDisp()
                End If
            Else
                Call Z_CLS()                                            ' データロードでログ画面クリア(標準版)
            End If
            '----- V6.1.4.0_22↑ -----
            gDspCounter = 0                                             ' ログ画面表示基板枚数カウンタクリア ###013
            ' "データをロードしました"
            strMSG = MSG_143 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                        ' ﾒｯｾｰｼﾞ表示(ログ画面)
            'V4.0.0.0-65                ↓↓↓↓
            'gLoggingHeader = True                                       ' ﾌｧｲﾙﾛｸﾞﾍｯﾀﾞｰ出力ﾌﾗｸﾞ(TRUE:出力)
            ' ﾛｸﾞ表示開始ﾌﾗｸﾞをﾁｪｯｸする。
            If (1 = gSysPrm.stLOG.giLoggingMode) Then
                basTrimming.TrimLogging_CreateOrAppend("Main - Load", typPlateInfo.strDataName)
            End If
            'V4.0.0.0-65                ↑↑↑↑
            stPRT_ROHM.Lot_No = " "                                     ' QRデータロット番号クリア(ローム殿用)

            'V4.0.0.0-61
            If gKeiTyp = KEY_TYPE_RS Then
                TrimData.SetTrimmingData(sPath)
            End If
            'V4.0.0.0-61

            '----- V1.22.0.0④↓ -----
            ' サマリログ出力(シナジー)ならデータロードに生産管理データをクリアする(TKY)
            If (giSummary_Log = SUMMARY_OUT) Then
                Call ClearCounter(1)                                    ' 生産管理データクリア
            End If
            '----- V1.22.0.0④↑ -----

            ' TKY_CHIP ?
            If (gTkyKnd = KND_CHIP) Then
                '----- V1.18.0.0③↓ -----
                'If gSysPrm.stDEV.rPrnOut_flg = True Then               ' 「Print ON/OFF」ボタンが「Print ON」 ?
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ローム殿特注 ? 
                    '----- V1.18.0.1④↓ -----
                    ' データロード時はクリアしない
                    'If (giAppMode <> APP_MODE_LOAD) Then                ' ロードコマンド(手動)の場合は クリアしない
                    '    Call ClearCounter(1)                            ' 生産管理データクリア
                    '    Call ClrTrimPrnData()                           ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする(ローム殿特注) 
                    'End If
                    '----- V1.18.0.1④↑ -----
                End If
                '----- V1.18.0.0③↑ -----

                ' 基板種別対応(ﾄﾘﾑﾎﾟｼﾞｼｮﾝ、外部ｶﾒﾗｵﾌｾｯﾄ、回転中心再設定)
                Call SetMechanicalParam()
            End If

            '-----------------------------------------------------------------------
            '   FL側へ加工条件を送信する(FL時で加工条件ファイルがある場合)
            '-----------------------------------------------------------------------
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                '----- V4.0.0.0-58↓ -----
                ' SL436S時は加工条件番号を設定しなおす
                Call SetCndNum()
                '----- V4.0.0.0-58↑ -----
                ' 加工条件ファイルが存在するかチェック
                strXmlFName = ""
                r = GetFLCndFileName(sPath, strXmlFName, True)
                If (r = SerialErrorCode.rRS_OK) Then                    ' 加工条件ファイルが存在する ?
                    ' データ送信中のメッセージ表示
                    strMSG = MSG_148
                    Call Z_PRINT(strMSG)                                ' ﾒｯｾｰｼﾞ表示(ログ画面)

                    ' FL用加工条件ファイルをリードしてFL側へ加工条件を送信する
                    r = SendTrimCondInfToFL(stCND, sPath, strXmlFName)
                    If (r = SerialErrorCode.rRS_OK) Then

                        ' '#4.12.3.0⑥ ↓
                        'FL条件一元管理の場合には、ログ画面にFL条件ファイル名を表示しない 
                        If (giFLPrm_Ass = 1) Then
                            ' "FLへ加工条件を送信しました。"
                            strMSG = MSG_147 & vbCrLf
                        Else
                            ' "FLへ加工条件を送信しました。"
                            strMSG = MSG_147 & vbCrLf & " (SendDdata File Name = " & strXmlFName & ")"
                        End If
                        Call Z_PRINT(strMSG)                            ' ﾒｯｾｰｼﾞ表示(ログ画面)
                        ' '#4.12.3.0⑥ ↑

                    Else
                        strMSG = MSG_132                                ' "加工条件の送信に失敗しました。再度データをロードするか、編集画面から加工条件の設定を行ってください。"
                        Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
                        Call Z_PRINT(strMSG)                            ' ﾒｯｾｰｼﾞ表示(ログ画面)
                        Return (cFRS_FIOERR_INP)                        ' Return値 = ファイル入力エラー
                    End If
                    '----- V1.14.0.0②↓ -----
                    ' FL用パワー調整情報ファイルをリードする
                    Call GetFlAttInfoData(strMSG, stPWR_LSR, 0)         ' FL用パワー調整情報初期化
                    'V6.0.1.021　↓
                    '                    If (giAutoPwr = 1) Then                             ' オートパワー調整を(1=する/0=しない) 
                    If (giAutoPwr = 1) Or (giFixAttOnly = 1) Then                             ' オートパワー調整を(1=する/0=しない) 
                        'V6.0.1.021　↑
                        r = GetFLAttFileName(sPath, strMSG, True)       ' FL用パワー調整情報ファイル名を取得する
                        If (r = SerialErrorCode.rRS_OK) Then            ' FL用パワー調整情報ファイルが存在する ?
                            Call GetFlAttInfoData(strMSG, stPWR_LSR, 1) ' FL用パワー調整情報リード
                        End If
                    End If

                    ' INtime側へ固定ATT情報を送信する
                    r = SetFixAttInfo(stPWR_LSR.AttInfoAry(0))
                    r = Me.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? (メッセージは表示済み)
                        Return (r)
                    End If
                    '----- V1.14.0.0②↑ -----

                End If

                '----- V4.0.0.0-28 ↓ -----  加工条件ファイルがない場合は
                ' Form_Load() Initialize_TrimMachine()でFLへ転送されたFLParam.xmlの値が設定される
                '-----------------------------------------------------------------------
                '   加工条件番号からカットデータのQレート,電流値,STEG本数を設定する
                '-----------------------------------------------------------------------
                r = SetCutDataCndInfFromCndNum(_readFileVer)
                '----- V4.0.0.0-28 ↑ -----
            End If

            '----- V4.0.0.0-28 ↓ -----
            If (True = doSave) Then
                ' ファイルをセーブする
                r = File_Save(sPath)
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR2
            End If
            '----- V4.0.0.0-28 ↑ -----

            '----- V1.18.0.0⑦↓ -----
            ' 汎用GP-IB制御の有無を取得する
            Call Gpib2FlgCheck(bGpib2Flg, wreg, GType)
            ' 汎用GPIBデータをINtime側に送信する
            If (GType = 1) Then                                         ' 汎用GPIB ?
                r = SendTrimDataPlate2(gTkyKnd, wreg, GType)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                End If
            End If
            '----- V1.18.0.0⑦↑ -----

            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応)【ＱＲコードリード機能】 -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                If (gbQRCodeReaderUse OrElse gbQRCodeReaderUseTKYNET) And (typQRDATAInfo.bStatus) Then ' ＱＲデータ有効 ? 'V6.1.4.14②gbQRCodeReaderUseTKYNETも追加
                    ' ＱＲデータの減衰率(%)からロータリーアッテネータを設定する
                    'r = ObjQRCodeReader.SetAttenuater(typQRDATAInfo.dAttenuaterValue)      'V6.1.4.0_22
                    r = ObjQRCodeReader.SetAttenuater(CDbl(typQRDATAInfo.dAttenuaterValue)) 'V6.1.4.0_22
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? (メッセージは表示済み)
                        ' "アッテネータ減衰率=[x.xx(%)]設定異常終了しました。=[xxx]"
                        'strMSG = MSG_167 + "=[" & typQRDATAInfo.dAttenuaterValue.ToString("0.00") & "(%)]" + MSG_168 + "=[" & r.ToString & "]"         'V6.1.4.0_22
                        strMSG = MSG_167 + "=[" & CDbl(typQRDATAInfo.dAttenuaterValue).ToString("0.00") & "(%)]" + MSG_168 + "=[" & r.ToString & "]"    'V6.1.4.0_22
                        Call Me.System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    End If
                End If
            End If
            '----- V6.1.4.0_22↑ -----

            '-----------------------------------------------------------------------
            '   終了処理
            '-----------------------------------------------------------------------
            gCmpTrimDataFlg = 0                                         ' データ更新フラグ = 0(更新なし)    'V4.0.0.0-32
            Return (cFRS_NORMAL)                                        ' Return値 = 正常

STP_ERR:
            ' データファイルの読込み失敗時("データロードNG File=")
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC01, "NG File='" & sPath & strLDR)
            ' "データロードＮＧ"
            strMSG = MSG_144 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                        ' ﾒｯｾｰｼﾞ表示(ログ画面)
            Return (cFRS_FIOERR_INP)                                    ' Return値 = ファイル入力エラー

            '----- V4.0.0.0⑨↓ -----
STP_ERR2:
            ' データファイルの書込み失敗時
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC01, "NG File='" & sPath & strLDR)
            ' "データセーブＮＧ"
            strMSG = MSG_146 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                        ' ﾒｯｾｰｼﾞ表示(ログ画面)
            Return (cFRS_FIOERR_OUT)                                    ' Return値 = ファイル出力エラー
            '----- V4.0.0.0⑨↑    -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Sub_FileLoad() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー

            'V4.12.2.0①                 ↓
        Finally
            ' データファイル読み込み成功=True
            Dim doEnabled As Boolean = (cFRS_NORMAL = Sub_FileLoad)
            MapInitialize(doEnabled)                                    ' マップ再描画
        End Try
        'V4.12.2.0①                     ↑
    End Function
#End Region

#Region "ESｶｯﾄの読み込み判別設定処理"
    '''=========================================================================
    '''<summary>ESｶｯﾄの読み込み判別設定処理</summary>
    '''<param name="sPath">(INP) ﾌｧｲﾙﾊﾟｽ名</param>
    '''=========================================================================
    Private Sub SetEsType(ByRef sPath As String)
        On Error GoTo ERR_PROC

        Dim iPos As Short
        Dim sExt As String

        ' 拡張子までの文字数を取得
        iPos = InStrRev(sPath, ".")

        ' 拡張子を取得
        sExt = Mid(sPath, iPos + 1)

        Select Case UCase(sExt)

            Case "WTC"
                ' ESは通常のES
                gEsCutFileType = 0
            Case "WTC2"
                ' ESはES2で読み込む
                gEsCutFileType = 1
            Case Else
                ' ESは通常のES
                gEsCutFileType = 0
        End Select

        Exit Sub
ERR_PROC:
        System.Diagnostics.Debug.Assert((0), "")
        Err.Clear()
    End Sub
#End Region

#Region "ESｶｯﾄの置換え処理"
    '''=========================================================================
    '''<summary>ESｶｯﾄの置換え処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub ChkCutTypeEs()
        On Error GoTo ERR_PROC

        Dim iTotalChipNum As Short
        Dim iTotalCutNum As Short
        Dim iRegCnt As Short
        Dim iRegNo As Short
        Dim iCutCnt As Short

        ' 開発時にはESのみだけだったが、ES ES2をカットタイプ設ける為処理を実施しない    'V1.14.0.0①
        Exit Sub
        ' ﾃﾞｰﾀ取得
        'Call GetChipNum(iTotalChipNum)                      ' 抵抗数取得
        iTotalChipNum = typPlateInfo.intResistCntInBlock    ' ブロック内抵抗数(マーキング抵抗含む)
        For iRegCnt = 1 To iTotalChipNum
            ' ﾃﾞｰﾀ取得
            iRegNo = typResistorInfoArray(iRegCnt).intResNo ' 抵抗番号取得
            Call GetRegCutNum(iRegNo, iTotalCutNum)         ' ｶｯﾄ数取得

            'With typResistorInfoArray(iRegNo)              ' ###012
            With typResistorInfoArray(iRegCnt)              ' ###012
                ' 1抵抗のｶｯﾄ数分ﾙｰﾌﾟ
                For iCutCnt = 1 To iTotalCutNum
                    ' ｶｯﾄﾀｲﾌﾟESの読み込みを"ES"で行う場合
                    If gEsCutFileType = 0 Then                      ' ＳＬ４３２ＨＷのデータ　V1.14.0.0①
                        ' ES2の情報があれば
                        If .ArrCut(iCutCnt).strCutType = "S" Then
                            ' ESに変更
                            .ArrCut(iCutCnt).strCutType = "K"
                        End If
                        ' ｶｯﾄﾀｲﾌﾟESの読み込みを"ES2"で行う場合
                    ElseIf gEsCutFileType = 1 Then                  ' ＳＬ４３２ＨＷのデータ　V1.14.0.0①
                        ' ESの情報があれば
                        If .ArrCut(iCutCnt).strCutType = "K" Then
                            'ES2に変更
                            .ArrCut(iCutCnt).strCutType = "S"
                        End If
                    Else                                            ' ＳＬ４３２Ｒのデータ 　V1.14.0.0①
                    End If
                Next
            End With
        Next

        '' NGマーキングありの場合                           ' ###012
        'If typPlateInfo.intNGMark = 1 Then
        '    iRegNo = 1000
        '    Call GetRegCutNum(iRegNo, iTotalCutNum)         ' ｶｯﾄ数取得
        '    With typResistorInfoArray(iRegNo)
        '        ' 1抵抗のｶｯﾄ数分ﾙｰﾌﾟ
        '        For iCutCnt = 1 To iTotalCutNum
        '            ' ｶｯﾄﾀｲﾌﾟESの読み込みを"ES"で行う場合
        '            If gEsCutFileType = 0 Then
        '                ' ES2の情報があれば
        '                If .ArrCut(iCutCnt).strCutType = "S" Then
        '                    ' ESに変更
        '                    .ArrCut(iCutCnt).strCutType = "K"
        '                End If
        '                ' ｶｯﾄﾀｲﾌﾟESの読み込みを"ES2"で行う場合
        '            Else
        '                ' ESの情報があれば
        '                If .ArrCut(iCutCnt).strCutType = "K" Then
        '                    'ES2に変更
        '                    .ArrCut(iCutCnt).strCutType = "S"
        '                End If
        '            End If
        '        Next
        '    End With
        'End If

        '        Exit Sub

ERR_PROC:
        System.Diagnostics.Debug.Assert((0), "")
        Err.Clear()
    End Sub
#End Region

#Region "ﾌｧｲﾙｾｰﾌﾞ(F2)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ﾌｧｲﾙｾｰﾌﾞ(F2)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub CmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdSave.Click

        Dim r As Integer
        Dim sPath As String
        Dim strXmlFName As String
        Dim strMSG As String = ""
        Dim result As System.Windows.Forms.DialogResult
        Dim NowFileName As String = ""       'V4.9.0.0②
        Dim NewFileName As String = ""       'V4.9.0.0②

        Try
            '-----------------------------------------------------------------------
            '   初期処理
            '-----------------------------------------------------------------------
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(True)

            If (giAppMode <> APP_MODE_EDIT) Then                ' ﾃﾞｰﾀ編集中なら下記はSKIP
                ' トリマ装置状態を動作中に設定する
                r = TrimStateOn(F_SAVE, APP_MODE_SAVE, "", "")
                If (r <> cFRS_NORMAL) Then GoTo STP_END
            End If

            '-----------------------------------------------------------------------
            '   書込みファイルの拡張子設定
            '-----------------------------------------------------------------------
            If (gTkyKnd = KND_TKY) Then                         ' TKYの場合
                comDlgSave.Filter = "*.tdt|*.tdt"
            End If
            If (gTkyKnd = KND_CHIP) Then                        ' CHIPの場合
                comDlgSave.Filter = "*.tdc|*.tdc"
            End If
            If (gTkyKnd = KND_NET) Then                         ' NETの場合
                comDlgSave.Filter = "*.tdn|*.tdn"
            End If
            '----- V4.0.0.0③↓ -----
            If (giMachineKd = MACHINE_KD_RS) Then                           ' SL436S ? 
                ' SL436S用の拡張子を表示する
                If (gTkyKnd = KND_TKY) Then                                 ' TKYの場合
                    comDlgSave.Filter = "*.tdts|*.tdts"
                ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIPの場合
                    comDlgSave.Filter = "*.tdcs|*.tdcs"
                Else                                                        ' NETの場合
                    comDlgSave.Filter = "*.tdns|*.tdns"
                End If
            End If
            '----- V4.0.0.0③↑ -----
            '#If cSOFTKYBOARDcUSE = 1 Then
            '-----------------------------------------------------------------------
            ' ソフトウェアキーボードを起動する
            '-----------------------------------------------------------------------
            Dim procHandle As Process
            procHandle = New Process
            Call StartSoftwareKeyBoard(procHandle)
            '#End If

            '-----------------------------------------------------------------------
            '   【名前を付けて保存】ﾀﾞｲｱﾛｸﾞを表示する
            '-----------------------------------------------------------------------
            ' トリムデータファイル格納位置
            'V6.1.4.14②↓
            comDlgSave.InitialDirectory = System.IO.Path.GetDirectoryName(typPlateInfo.strDataName) 'V6.1.4.14②
            If System.IO.Directory.Exists(comDlgSave.InitialDirectory) = False Then
                'V6.1.4.14②↑
                comDlgSave.InitialDirectory = gSysPrm.stDIR.gsTrimFilePath
                'V6.1.4.14②↓' ＮＥＴの時のトリムデータファイル格納位置
                If gTkyKnd = KND_NET Then
                    comDlgSave.InitialDirectory = GetPrivateProfileString_S("QR_CODE", "TKYNET_TRIMMING_DATA_FOLDER", "C:\TRIM\tky.ini", "C:\TRIMDATA\DATA_NET\")
                End If
            End If
            'V6.1.4.14②↑
            '----- V1.14.0.0⑥↓ -----
            ' 例)旧データ(xxxx.WTC)の場合、拡張子がxxx.tdcでなく.WTCのままセーブされるので　拡張子をtdcに変更する
            'comDlgSave.FileName = typPlateInfo.strDataName
            Call GetSaveFileName(typPlateInfo.strDataName, strMSG)
            comDlgSave.FileName = strMSG
            '----- V1.14.0.0⑥↑ -----
            comDlgSave.OverwritePrompt = True                   ' 存在する場合は確認ﾒｯｾｰｼﾞを表示
            'V6.0.0.0⑤            Call End_GazouProc(ObjGazou)    'V5.0.0.6⑭
            '【名前を付けて保存】ﾀﾞｲｱﾛｸﾞを表示する
            ' ※ﾌｧｲﾙ名指定なしでは戻ってこない、拡張子付で戻ってくる
            result = comDlgSave.ShowDialog()

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' ソフトウェアキーボードを終了する
            Call EndSoftwareKeyBoard(procHandle)
            '#End If

            ' OK以外なら終了
            If (result <> Windows.Forms.DialogResult.OK) Then
                GoTo STP_END                                    ' Cansel指定なら終了
            End If

            'V4.0.0.0-65                ↓↓↓↓
            ' ﾛｸﾞ表示開始ﾌﾗｸﾞをﾁｪｯｸする。
            If (1 = gSysPrm.stLOG.giLoggingMode) Then
                If (comDlgSave.FileName <> typPlateInfo.strDataName) Then
                    ' 別名保存された場合
                    basTrimming.TrimLogging_CreateOrAppend(
                        "Main - Save (" & comDlgSave.FileName & ")", procMsgOnly:=True)             ' 既存ﾌｧｲﾙに保存

                    basTrimming.TrimLogging_CreateOrAppend("Main - Save", comDlgSave.FileName)      ' 新規ﾌｧｲﾙに保存
                Else
                    basTrimming.TrimLogging_CreateOrAppend("Main - Save", procMsgOnly:=True)
                End If
            End If
            'V4.0.0.0-65                ↑↑↑↑

            '-----------------------------------------------------------------------
            '   トリミングデータファイルを書込む
            '-----------------------------------------------------------------------
            ' トリミングデータファイルを書込む
            Call SetMousePointer(Me, True)                      ' 砂時計表示(ﾏｳｽﾎﾟｲﾝﾀ)
            sPath = comDlgSave.FileName                         ' トリミングデータファイル名
            typPlateInfo.strDataName = sPath                    ' プレートデータのトリミングデータファイル名更新
            r = File_Save(sPath)                                ' トリムデータ書込み
            ' データファイルの書込み失敗時
            If (r <> cFRS_NORMAL) Then GoTo STP_ERR

            '-----------------------------------------------------------------------
            '   ファイルパス名を表示する
            '-----------------------------------------------------------------------
            '----- V6.1.4.0⑦↓(KOA EW殿SL432RD対応) -----
            ''V4.0.0.0⑮
            'LblDataFileName.Text = sPath
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = sPath                     ' トリミングデータ名と第１抵抗データ表示域を設定する 
            Else
                LblDataFileName.Text = sPath                    ' トリミングデータ名表示
            End If
            '----- V6.1.4.0⑦↑ -----

            '-----------------------------------------------------------------------
            '   FL側から現在の加工条件を受信してFL用加工条件ファイルをライトする(FL時)
            '-----------------------------------------------------------------------
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then   ' FL(ﾌｧｲﾊﾞｰﾚｰｻﾞ) ? 
                strXmlFName = ""
                r = RcvTrimCondInfToFL(stCND, sPath, strXmlFName)
                If (r = SerialErrorCode.rRS_OK) Then
                    ' "加工条件ファイルを作成しました。"
                    ' '#4.12.3.0⑥ ↓
                    '                    strMSG = MSG_142 + vbCrLf + " (File Name = " + strXmlFName + ")"
                    'FL条件一元管理の場合には、ログ画面にFL条件ファイル名を表示しない 
                    If (giFLPrm_Ass = 1) Then
                        ' "FLへ加工条件を送信しました。"
                        strMSG = MSG_142 & vbCrLf
                    Else
                        ' "FLへ加工条件を送信しました。"
                        strMSG = MSG_142 & vbCrLf & " (SendDdata File Name = " & strXmlFName & ")"
                    End If
                    Call Z_PRINT(strMSG)                        ' ﾒｯｾｰｼﾞ表示(ログ画面)
                    ' '#4.12.3.0⑥ ↑
                End If
                '----- V1.14.0.0②↓-----
                ' FL用パワー調整情報ファイル(データファイル名+.att)をライトする
                'V6.0.1.021　↓
                '                    If (giAutoPwr = 1) Then                             ' オートパワー調整を(1=する/0=しない) 
                If (giAutoPwr = 1) Or (giFixAttOnly = 1) Then                             ' オートパワー調整を(1=する/0=しない) 
                    'V6.0.1.021　↑
                    r = GetFLAttFileName(sPath, strMSG, False)  ' 固定ATT情報ファイル名を取得する
                    Call PutFlAttInfoData(strMSG, stPWR_LSR)    ' FL用パワー調整情報ファイルをライトする
                End If
                '----- V1.14.0.0②↑-----
            End If

            'V4.9.0.0②↓
            If giChangePoint = 1 Then
                If gKeiTyp = KEY_TYPE_RS Then
                    NowFileName = MakeutPointFileData(gStrTrimFileName)
                End If
            End If
            'V4.9.0.0②↑
            gStrTrimFileName = sPath                            ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名をｸﾞﾛｰﾊﾞﾙ変数に設定する
            'V4.2.0.0③
            If gKeiTyp = KEY_TYPE_RS Then
                TrimData.SetTrimmingData(sPath)
                'V4.9.0.0②↓
                If giChangePoint = 1 Then
                    If System.IO.File.Exists(NowFileName) Then
                        NewFileName = MakeutPointFileData(gStrTrimFileName)
                        If NowFileName <> NewFileName Then
                            My.Computer.FileSystem.CopyFile(NowFileName, NewFileName)
                        End If
                    Else
                        ' ファイルがなかった場合には、デフォルトのファイルをコピーする
                        My.Computer.FileSystem.CopyFile(DEFAULT_CUTFILE, NewFileName)
                    End If
                End If
                'V4.9.0.0②↑
            End If
            'V4.2.0.0③
            gCmpTrimDataFlg = 0                                 ' データ更新フラグ = 0(更新なし)

            '-----------------------------------------------------------------------
            '   操作ログを出力する
            '-----------------------------------------------------------------------
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC02, "File='" & sPath & "' MANUAL") 'OK
            ' "データをセーブしました"
            strMSG = MSG_145 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                ' ﾒｯｾｰｼﾞ表示(ログ画面)

STP_END:
            '-----------------------------------------------------------------------
            '   終了処理
            '-----------------------------------------------------------------------
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            If (giAppMode <> APP_MODE_EDIT) Then                ' ﾃﾞｰﾀ編集中なら下記はSKIP
                Call TrimStateOff()                             ' トリマ装置状態を動作中に設定する
            End If
            Exit Sub

STP_ERR:
            ' データファイルの書込み失敗時
            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_FUNC02, "File='" & sPath & "' MANUAL LOADERR")
            ' "データセーブＮＧ"
            strMSG = MSG_146 & vbCrLf & " (File Name = " & sPath & ")"
            Call Z_PRINT(strMSG)                                ' ﾒｯｾｰｼﾞ表示(ログ画面)
            GoTo STP_END

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdSave_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try
    End Sub
#End Region

#Region "ﾃﾞｰﾀ編集(F3)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ﾃﾞｰﾀ編集(F3)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdEdit.Click

        Dim piPP31 As Short
        Dim r As Integer
        Dim strMSG As String
        Dim shiftKeyDown As Boolean = (_editNewData AndAlso (Control.ModifierKeys = Keys.Shift))    'V4.12.4.0②

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' 統計表示処理の状態変更
            Call RedrawDisplayDistribution(True)                'V4.7.3.5① パスワード画面表示前（TrimStateOnの中）に統計画面を閉じないとパスワード画面が隠れてしまうのでTrimStateOnの前に移動
            ' トリマ装置状態を動作中に設定する
            '#4.12.2.0⑤            r = TrimStateOn(F_EDIT, APP_MODE_EDIT, MSG_OPLOG_FUNC03, "")
            r = TrimStateOn(F_EDIT, APP_MODE_EDIT, MSG_OPLOG_FUNC03, "", shiftKeyDown)      '#4.12.2.0⑤
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' 統計表示処理の状態変更
            'V4.7.3.5①            Call RedrawDisplayDistribution(True)

            ' コンソールボタンのランプ状態を設定する
            Call LAMP_CTRL(LAMP_START, False)                       ' STARTﾗﾝﾌﾟ消灯 
            Call LAMP_CTRL(LAMP_RESET, False)                       ' RESETﾗﾝﾌﾟ消灯 

            piPP31 = typPlateInfo.intManualReviseType               ' 補正方法(0:補正なし, 1:1回のみ, 2:毎回)を保存しておく
            Call Form1Button(0)                                     ' コマンドボタンを無効にする
            Call SetVisiblePropForVideo(False)

            '----- V6.0.3.0③↓ -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TimerQR.Enabled = False                             ' 監視タイマー停止
                Call QR_Rs232c_Close()                              ' ポートクローズ(QRコード受信用 ローム殿特注)
            End If
            '----- V6.0.3.0③↑ -----

            '-------------------------------------------------------------------
            '   データ編集プログラム起動(通常モード)処理 ###063
            '-------------------------------------------------------------------
            '#4.12.2.0⑤            r = ExecEditProgram(0)                                  ' データ編集プログラム起動
            r = ExecEditProgram(0, shiftKeyDown)                    ' データ編集プログラム起動  '#4.12.2.0⑤
            If (r <> cFRS_NORMAL) And (r <> cFRS_FIOERR_INP) And (r <> cFRS_FIOERR_OUT) Then
                If (r <= cFRS_ERR_EMG) Then                         ' 非常停止等検出 ?
                    Call Me.AppEndDataSave()
                    Call Me.AplicationForcedEnding()                ' 強制終了
                End If
                GoTo STP_END
            End If

            '----- V4.0.0.0⑳↓ -----
            '目標値、上限下限の表示
            SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)
            '----- V4.0.0.0⑳↑ -----

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
STP_END:
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            '----- V6.0.3.0③↓ -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                Call QR_Rs232c_Open()                               ' ポートオープン(QRコード受信用 ローム殿特注)
                TimerQR.Enabled = True                              ' 監視タイマー開始
            End If
            '----- V6.0.3.0③↑ -----

            'V5.0.0.6⑲ ADD START↓
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                Dim swStatus As Integer
                Dim interlockStatus As Integer
                r = INTERLOCK_CHECK(interlockStatus, swStatus)
                If r = cFRS_NORMAL And interlockStatus = INTERLOCK_STS_DISABLE_NO Then
                    ' スライドカバーの状態取得（INTRIMではIO取得のみの為、エラーが返る事はない）
                    r = SLIDECOVER_GETSTS(swStatus)
                    If r = cFRS_NORMAL And swStatus = SLIDECOVER_CLOSE Then
                        ' スライドカバー閉
                        r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH62, &HFF, &HFF0000)
                    End If
                End If
            End If
            'V5.0.0.6⑲ ADD END↑

            Call basTrimming.ChangeByTrimmingData()                     ' V5.0.0.6②　トリミングデータ更新時の処理

            ' Formを元に戻す
            'Me.WindowState = FormWindowState.Normal
            ' トリマ装置状態をアイドル中に設定する
            Call TrimStateOff()
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdEdit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try
    End Sub
#End Region

#Region "新規モード用のデータ名を設定する"
    '''=========================================================================
    '''<summary>新規モード用のデータ名を設定する</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Replace_Data_Extension(ByRef FName As String)

        Dim strDAT As String
        Dim strMSG As String

        Try
            ' 新規モードの場合は、新規モード用のデータ名を設定する
            strDAT = DateTime.Today.Year.ToString("0000") + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00")
            If (gTkyKnd = KND_TKY) Then                         ' TKYの場合
                strDAT = "TkyData" + strDAT + ".tdt"            ' "TkyDatayyyymmddhhmmss.tdt"
            ElseIf (gTkyKnd = KND_CHIP) Then                    ' CHIPの場合
                If (gKeiTyp = MACHINE_KD_RS) Then   'V4.0.0.0-38
                    strDAT = "ChipData" + strDAT + ".tdcs"      ' "ChipDatayyyymmddhhmmss.tdcs"
                Else
                    strDAT = "ChipData" + strDAT + ".tdc"       ' "ChipDatayyyymmddhhmmss.tdc"
                End If
            Else
                strDAT = "NetData" + strDAT + ".tdn"            ' "NetDatayyyymmddhhmmss.tdn"
            End If
            FName = strDAT

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Replace_Data_Extension() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "Laser(F5)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>Laser(F5)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdLaser_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdLaser.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_LASER, APP_MODE_LASER, MSG_OPLOG_FUNC05, "")

            '----- V4.0.0.0-28 ↓ -----
            '-----------------------------------------------------------------------
            '   加工条件番号からカットデータのQレート,電流値,STEG本数を設定する
            '   (シンプルトリマ用(FL時で加工条件ファイルがある場合))
            '-----------------------------------------------------------------------
            r = SetCutDataCndInfFromCndNum(_readFileVer)
            '----- V4.0.0.0-28 ↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLaser_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "Laser調整処理"
    '''=========================================================================
    '''<summary>Laser調整処理</summary>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function LaserProc(ByRef pltInfo As PlateInfo) As Short

        Dim fQrate As Double                                            ' Qレート[KHz] 
        Dim SPower As Double                                            ' 設定パワー[W](ｵﾌﾟｼｮﾝ)
        Dim r As Short
        Dim strMSG As String = ""
        Dim stPWR As LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach.POWER_ADJUST_INFO                      ' FL用パワー調整情報パラメータ ※Laser.OCXで定義'V1.14.0.0②
        ReDim stPWR.CndNumAry(MAX_BANK_NUM - 1)                         ' 構造体の初期化 
        ReDim stPWR.AdjustTargetAry(MAX_BANK_NUM - 1)
        ReDim stPWR.AdjustLevelAry(MAX_BANK_NUM - 1)
        ReDim stPWR.AttInfoAry(MAX_BANK_NUM - 1)
        Dim parModules As MainModules
        parModules = New MainModules
        Dim StgX As Double = 0.0 ' V4.0.0.0-40
        Dim StgY As Double = 0.0 ' V4.0.0.0-40

        Try
            With pltInfo
                '-------------------------------------------------------------------
                '   レーザー調整前処理
                '-------------------------------------------------------------------
                LaserProc = cFRS_NORMAL                                 ' Return値 = 正常

                '----- V3.0.0.2①↓ -----
                ' LASERコマンド時は、Zの位置を0とする
                r = EX_ZMOVE(0.0, MOVE_ABSOLUTE)                        ' エラーならメッセージ表示済み
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                End If
                '----- V3.0.0.2①↑ -----

                If (gSysPrm.stIOC.giPM_Tp = 1) Then                     ' パワーメータが「ステージ設置」タイプ ? ###132
                    'If (gSysPrm.stRMC.giRmCtrl2 >= 2) And (gSysPrm.stIOC.giPM_Tp = 1) Then
                    '---------------------------------------------------------------
                    '   XYテーブルをシスパラのﾊﾟﾜｰ調整位置へ移動する
                    '   パワー測定値表示ならﾊﾟﾜｰ調整位置へ移動する (ﾊﾟﾜｰﾒｰﾀ付き載物台の場合)
                    '---------------------------------------------------------------
                    ' 設定パワー/Qレート設定
                    fQrate = .dblPowerAdjustQRate                       ' Qレート[KHz]
                    SPower = .dblPowerAdjustQRate                       ' 設定パワー[W]

                    ' XYテーブルをﾊﾟﾜｰ調整位置へ移動する
                    r = System1.EX_SMOVE2(gSysPrm, gSysPrm.stRA2.gfATTTableOffsetX, gSysPrm.stRA2.gfATTTableOffsetY)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ?
                        LaserProc = r                                   ' (注)軸ﾘﾐｯﾄ/ﾀｲﾑｱｳﾄｴﾗｰﾒｯｾｰｼﾞは表示済み
                        Exit Function
                    End If
                    ' ブロックサイズ設定しfθセンタへ
                    'r = System1.EX_BSIZE(gSysPrm, 0, 0)                ' ﾌﾞﾛｯｸｻｲｽﾞ/BPｵﾌｾｯﾄ(ﾊﾟﾜｰ調整位置)設定
                    '----- V1.24.0.0①↓ -----
                    If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                        r = System1.EX_BSIZE(gSysPrm, 60.0, 20.0)
                        'V6.0.4.0①↓
                    ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                        r = System1.EX_BSIZE(gSysPrm, 60.0, 30.0)
                        'V6.0.4.0①↑
                    ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then        'V4.4.0.0①
                        r = System1.EX_BSIZE(gSysPrm, 90.0, 90.0)
                    Else
                        r = System1.EX_BSIZE(gSysPrm, 80.0, 80.0)
                    End If
                    'r = System1.EX_BSIZE(gSysPrm, 80.0, 80.0)           ' ﾌﾞﾛｯｸｻｲｽﾞ/BPｵﾌｾｯﾄ(ﾊﾟﾜｰ調整位置)設定 ###132
                    '----- V1.24.0.0①↑ -----
                    If (r <> cFRS_NORMAL) Then                          ' ｴﾗｰ ?
                        LaserProc = r                                   ' (注)軸ﾘﾐｯﾄ/ﾀｲﾑｱｳﾄｴﾗｰﾒｯｾｰｼﾞは表示済み
                        Exit Function
                    End If

                    ' BP移動(ﾊﾟﾜｰ調整BP位置) ###132
                    'r = System1.EX_BPOFF(gSysPrm, gSysPrm.stRA2.gfATTBpOffsetX, gSysPrm.stRA2.gfATTBpOffsetY)
                    'If (r <> cFRS_NORMAL) Then                          ' ｴﾗｰ ?
                    '    LaserProc = r                                   ' (注)軸ﾘﾐｯﾄ/ﾀｲﾑｱｳﾄｴﾗｰﾒｯｾｰｼﾞは表示済み
                    '    Exit Function
                    'End If
                    r = System1.EX_MOVE(gSysPrm, gSysPrm.stRA2.gfATTBpOffsetX, gSysPrm.stRA2.gfATTBpOffsetY, 1)
                    If (r <> cFRS_NORMAL) Then                          ' ｴﾗｰ ?
                        LaserProc = r                                   ' (注)軸ﾘﾐｯﾄ/ﾀｲﾑｱｳﾄｴﾗｰﾒｯｾｰｼﾞは表示済み
                        Exit Function
                    End If

                Else
                    '--------------------------------------------------------------------------
                    '   θ補正(ｵﾌﾟｼｮﾝ) & XYテーブルをトリム位置へ移動する
                    '--------------------------------------------------------------------------
                    ' Qレート初期値設定
                    fQrate = gSysPrm.stDEV.gfLaserQrate                 ' Qレート[KHz]
                    SPower = 0.0                                        ' 設定パワー[W](ｵﾌﾟｼｮﾝ)

                    ' ブロックサイズ(0,0)へ設定しfθセンタへ
                    r = System1.EX_BSIZE(gSysPrm, 0, 0)                 ' ﾌﾞﾛｯｸｻｲｽﾞ/BPｵﾌｾｯﾄ(ﾊﾟﾜｰ調整位置)設定
                    If (r <> cFRS_NORMAL) Then                          ' ｴﾗｰ ?
                        LaserProc = r                                   ' (注)軸ﾘﾐｯﾄ/ﾀｲﾑｱｳﾄｴﾗｰﾒｯｾｰｼﾞは表示済み
                        Exit Function
                    End If

                    ' XYテーブルをトリム位置へ移動(トリム位置＋オフセット＋補正値)
                    '----- V1.13.0.0③↓ -----
                    'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX, .dblTableOffsetYDir + gfCorrectPosY, 0)

                    '----- V2.0.0.0⑨↓ -----
                    If (giMachineKd = MACHINE_KD_RS) Then
                        '----- V4.0.0.0-40↓ -----
                        ' SL36S時でステージYの原点位置が上の場合、ブロックサイズの1/2は加算しない
                        If (giStageYOrg = STGY_ORG_UP) Then
                            StgX = .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                            StgY = .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
                        Else
                            StgX = .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX
                            StgY = .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2)
                        End If
                        r = System1.EX_START(gSysPrm, StgX, StgY, 0)
                        'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2), 0)
                        '----- V4.0.0.0-40↑ -----
                    Else
                        r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
                    End If
                    'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + gfStgOfsX, .dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY, 0)
                    '----- V2.0.0.0⑨↑ -----
                    '----- V1.13.0.0③↑ -----
                    If (r <> cFRS_NORMAL) Then                          ' エラー ?
                        LaserProc = r                                   ' Return値設定
                        Exit Function
                    End If
                End If

                '--------------------------------------------------------------------------
                '   レーザＯＣＸにデータを渡す
                '--------------------------------------------------------------------------
                ' 初期設定(OcxSystemｵﾌﾞｼﾞｪｸﾄ, OcxUtilityｵﾌﾞｼﾞｪｸﾄ, Qレート[KHz], 設定パワー[W](ｵﾌﾟｼｮﾝ), 処理ﾓｰﾄﾞ(0=標準), 実行ﾓｰﾄﾞ(0:手動))
                r = Me.Ctl_LaserTeach2.SetUp(System1, Utility1, gSysPrm.stDEV.gfLaserQrate, 0.1, 0, 0)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    LaserProc = r                                       ' Return値設定
                    Exit Function
                End If

                '----- V1.14.0.0②↓ -----
                ' FL用パワー調整情報を設定する
                Call SetFlParamForLaserCmd(stPWR_LSR, stPWR)
                '----- V1.14.0.0②↑ -----
                '---------------------------------------------------------------------------
                '   レーザー調整処理
                '---------------------------------------------------------------------------
                'V6.0.1.0①                r = Me.Ctl_LaserTeach2.LaserProc()
                r = Me.Ctl_LaserTeach2.LaserProc(Text4.Left, Text4.Top + DISPOFF_SUBFORM_TOP) 'V6.0.1.0①
                If (r <> cFRS_NORMAL) Then                              ' ｴﾗｰ ?
                    LaserProc = r                                       ' Return値設定
                    Exit Function
                End If
                Me.Refresh()

                '----- V1.14.0.0⑧↓ -----
                ' FL側から現在の加工条件を受信する
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then
                    r = TrimCondInfoRcv(stCND)
                    If (r <> SerialErrorCode.rRS_OK) Then
                        ' "ＦＬ側加工条件のリードに失敗しました。"
                        Call System1.TrmMsgBox(gSysPrm, MSG_141, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                        LaserProc = r                                       ' Return値設定
                        Exit Function
                    End If
                End If
                '----- V1.14.0.0⑧↑ -----
                '----- V1.14.0.0②↓ -----
                ' FL用パワー調整情報を取得する
                Call GetFlParamForLaserCmd(stPWR_LSR, stPWR)
                '----- V1.14.0.0②↑ -----

                '---------------------------------------------------------------------------
                '   レーザー調整後処理
                '---------------------------------------------------------------------------
                ' RMCTRL2 >=2 の場合、 減衰率をシスパラより再表示("減衰率 = 99.9%")  '###026
                Call gDllSysprmSysParam_definst.GetSystemParameter(gSysPrm)          '###029
                If (gSysPrm.stRMC.giRmCtrl2 >= 2) Then
                    strMSG = LBL_ATT + "  " + CDbl(gSysPrm.stRAT.gfAttRate).ToString("##0.0") + " %"
                    Me.LblRotAtt.Text = strMSG
                End If

                ' 測定値表示(RMCTRL2 >=3 で測定値表示指定の場合)
                If (gSysPrm.stRMC.giRmCtrl2 >= 3) And (gSysPrm.stRMC.giPMonHi = 1) Then '###026
                    ' 測定値取得
                    r = Me.Ctl_LaserTeach2.GetMesPower((gSysPrm.stRAT.gfMesPower))
                    'Call gDllSysprmSysParam_definst.GetSysPrm_ROT_ATT((gSysPrm.stRAT)) '###029
                    ' 測定パワー[W]の表示
                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    '    strMSG = "レーザーパワー設定値　"
                    '    strMSG = strMSG & gSysPrm.stRAT.gfMesPower.ToString("##0.00") & "W"
                    'Else
                    '    strMSG = "Laser Power "
                    '    strMSG = strMSG & gSysPrm.stRAT.gfMesPower.ToString("##0.00") & "W"
                    'End If
                    strMSG = Form1_007 & gSysPrm.stRAT.gfMesPower.ToString("##0.00") & "W"
                    Me.LblMes.Text = strMSG                             ' 測定パワー[W]の表示
                End If

                ' FL(ﾌｧｲﾊﾞｰﾚｰｻﾞ)時はデータ更新フラグを更新ありとする(加工条件が変更されている可能性ありの為)
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then       ' V1.14.0.0②
                    gCmpTrimDataFlg = 1                                 ' データ更新フラグ = 1(更新あり)
                End If
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.LaserProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            LaserProc = cERR_TRAP                                       ' Return値 = 例外エラー
        End Try

    End Function
#End Region
    '----- V1.14.0.0②↓ -----
#Region "FL用パワー調整パラメータを設定する"
    '''=========================================================================
    ''' <summary>FL用パワー調整パラメータを設定する</summary>
    ''' <param name="stPWR_LSR"></param>
    ''' <param name="stPWR"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub SetFlParamForLaserCmd(ByRef stPWR_LSR As POWER_ADJUST_INFO, ByRef stPWR As LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach.POWER_ADJUST_INFO)

        Dim strMSG As String
        Dim IDX As Integer

        Try
            ' FLでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return

            ' FL用パワー調整情報をレーザコマンド用パラメータに設定する
            For IDX = 0 To (MAX_BANK_NUM - 1)
                stPWR.CndNumAry(IDX) = stPWR_LSR.CndNumAry(IDX)                     ' 加工条件番号配列(0-31)(0=無効, 1=有効)
                stPWR.AdjustTargetAry(IDX) = stPWR_LSR.AdjustTargetAry(IDX)         ' 調整目標パワー配列
                stPWR.AdjustLevelAry(IDX) = stPWR_LSR.AdjustLevelAry(IDX)           ' パワー調整許容範囲配列
                stPWR.AttInfoAry(IDX) = stPWR_LSR.AttInfoAry(IDX)                   ' 固定ATT情報(0:固定ATT Off, 1:固定ATT On)配列
            Next IDX

            ' FL用パワー調整情報をLaserOCXに渡す
            Call Me.Ctl_LaserTeach2.SetFLPowerInfo(stPWR)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetFlParamForLaserCmd() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "FL用パワー調整パラメータを取得する"
    '''=========================================================================
    ''' <summary>FL用パワー調整パラメータを取得する</summary>
    ''' <param name="stPWR_LSR"></param>
    ''' <param name="stPWR"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub GetFlParamForLaserCmd(ByRef stPWR_LSR As POWER_ADJUST_INFO, ByRef stPWR As LaserFront.Trimmer.DllLaserTeach.ctl_LaserTeach.POWER_ADJUST_INFO)

        Dim strMSG As String
        Dim IDX As Integer
        Dim r As Integer

        Try
            ' FLでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return

            ' FL用パワー調整結果を取得する
            r = Me.Ctl_LaserTeach2.GetFLPowerResult(stPWR)
            If (r <> cFRS_NORMAL) Then Exit Sub

            ' FL用パワー調整情報をレーザコマンド用パラメータに設定する
            For IDX = 0 To (MAX_BANK_NUM - 1)
                stPWR_LSR.CndNumAry(IDX) = stPWR.CndNumAry(IDX)                     ' 加工条件番号配列(0-31)(0=無効, 1=有効)
                stPWR_LSR.AdjustTargetAry(IDX) = stPWR.AdjustTargetAry(IDX)         ' 調整目標パワー配列
                stPWR_LSR.AdjustLevelAry(IDX) = stPWR.AdjustLevelAry(IDX)           ' パワー調整許容範囲配列
                stPWR_LSR.AttInfoAry(IDX) = stPWR.AttInfoAry(IDX)                   ' 固定ATT情報(0:固定ATT Off, 1:固定ATT On)配列
            Next IDX

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetFlParamForLaserCmd() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.14.0.0②↑ -----
#Region "ﾛｷﾞﾝｸﾞ(F6)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ﾛｷﾞﾝｸﾞ(F6)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdLoging_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdLoging.Click

        Dim strMSG As String
        Dim objForm As Logging

        Try
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(True)

            ' トリマ装置状態を動作中に設定する
            Call TrimStateOn(F_LOG, APP_MODE_LOGGING, MSG_OPLOG_FUNC06, "MANUAL")

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' ソフトウェアキーボードを起動する
            Dim procHandle As Process
            procHandle = New Process
            Call StartSoftwareKeyBoard(procHandle)
            '#End If

            ' ロギング設定画面の表示
            objForm = New Logging()                         ' オブジェクト生成 
            ' ダイアログの表示
            Call objForm.ShowDialog()                       ' ロギング設定画面の表示
            Call objForm.Close()                            ' オブジェクト開放
            Call objForm.Dispose()                          ' リソース開放
            Call LoggingModeDisp()                          ' ﾛｷﾞﾝｸﾞ状態(Logging ON/OFF)表示"

            '#If cSOFTKYBOARDcUSE = 1 Then
            ' ソフトウェアキーボードを終了する
            Call EndSoftwareKeyBoard(procHandle)
            '#End If

            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLoging_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        ' 終了処理
        Call TrimStateOff()                                 ' トリマ装置状態をアイドル中に設定する

    End Sub
#End Region

#Region "ﾛｷﾞﾝｸﾞ状態(Logging ON/OFF)表示"
    '''=========================================================================
    '''<summary>ﾛｷﾞﾝｸﾞ状態(Logging ON/OFF)表示</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub LoggingModeDisp()

        If gSysPrm.stLOG.giLoggingMode Then
            lblLogging.Text = "Logging ON"
        Else
            lblLogging.Text = "Logging OFF"
        End If

    End Sub
#End Region

#Region "ﾌﾟﾛｰﾌﾞﾃｨｰﾁﾝｸﾞ(F7)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ﾌﾟﾛｰﾌﾞﾃｨｰﾁﾝｸﾞ(F7)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdProbe_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdProbe.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_PROBE, APP_MODE_PROBE, MSG_OPLOG_FUNC07, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdProbe_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "ﾌﾟﾛｰﾌﾞﾃｨｰﾁﾝｸﾞ処理"
    '''=========================================================================
    '''<summary>ﾌﾟﾛｰﾌﾞﾃｨｰﾁﾝｸﾞ処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function ProbeTeaching(ByRef pltInfo As PlateInfo) As Short
        'Private Function ProbeTeaching(ByVal pltInfo As PlateInfo) As Short

        Dim r As Short
        Dim strMSG As String
        Dim strDAT As String
        Dim rn As Short
        Dim ManualThetaFlg As Short
        Dim W_bpox As Double                                ' Beem Position X OFFSET(mm)
        Dim W_bpoy As Double                                ' Beem Position Y OFFSET(mm)
        Dim W_Xoff As Double                                ' Trim Position Offset X(mm)
        Dim W_Yoff As Double                                ' Trim Position Offset Y(mm)
        'Dim W_XCor As Double                                ' ずれ量X X(mm)
        'Dim W_YCor As Double                                ' ずれ量Y Y(mm)
        '                                                   ' プローブ接触位置確認用ﾃｰﾌﾞﾙ
        Dim PrbHiTBL(MaxRegNum, 2) As Double                '  Hの座標（左側の配列=抵抗番号,右側の配列=1:X座標、2:Y座標)
        Dim PrbLoTBL(MaxRegNum, 2) As Double                '  Lの座標（左側の配列=抵抗番号,右側の配列=1:X座標、2:Y座標)
        Dim RnoTbl(MaxRegNum) As Short                      ' 抵抗番号テーブル
        Dim PrbTbl(MaxRegNum, 7) As Short                   ' プローブ番号テーブル
        Dim NomTbl(MaxRegNum) As Double                     ' 目標値テーブル
        Dim SlpTbl(MaxRegNum) As Short                      ' 電圧変化スロープテーブル
        Dim StPosXTbl(MaxRegNum) As Double                  ' カット開始位置Xテーブル
        Dim StPosYTbl(MaxRegNum) As Double                  ' カット開始位置Yテーブル
        Dim FtTbl(MaxRegNum, 2) As Double                   ' ﾌｧｲﾅﾙﾃｽﾄ上限値/下限値(未使用だけどﾊﾟﾗﾒｰﾀにあるので)
        Dim parModules As MainModules
        parModules = New MainModules
        Dim gCntRegData As Integer                          ' プローブコマンドに必要な情報を渡す(抵抗数)     V6.0.1.023

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   θ補正(ｵﾌﾟｼｮﾝ) & XYテーブルをトリム位置へ移動する
                '--------------------------------------------------------------------------
                ProbeTeaching = cFRS_NORMAL                     ' Return値 = 正常
                W_bpox = .dblBpOffSetXDir                       ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄX
                W_bpoy = .dblBpOffSetYDir                       ' ﾋﾞｰﾑ位置ｵﾌｾｯﾄY
                W_Xoff = .dblTableOffsetXDir                    ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
                W_Yoff = .dblTableOffsetYDir                    ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY
                'Call BSIZE(.dblBlockSizeXDir, .dblBlockSizeYDir) 
                r = Move_Trimposition(pltInfo, 0.0, 0.0)        ' θ補正(ｵﾌﾟｼｮﾝ) & XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動
                SetTeachVideoSize()                             ' V6.0.3.0_49
                If (r <> cFRS_NORMAL) Then                      ' エラー ?
                    ProbeTeaching = r                           ' Return値設定
                    Exit Function
                End If

                ' BPｵﾌｾｯﾄ設定
                'Call System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then GoTo STP_END
                gCntRegData = 0  'V6.0.1.023
                '--------------------------------------------------------------------------
                '   プローブＯＣＸに渡すテーブルを作成する
                '--------------------------------------------------------------------------
                For rn = 1 To gRegistorCnt                              ' 抵抗数分設定する
                    If (typResistorInfoArray(rn).intResNo < 1000) Then  ' マーキング用抵抗以外 ? ###055
                        ' 抵抗番号
                        'V5.0.0.6⑰                        RnoTbl(rn) = rn
                        RnoTbl(rn) = typResistorInfoArray(rn).intResNo  'V5.0.0.6⑰
                        ' プローブ番号(H,L,G1～G5)
                        PrbTbl(rn, 1) = typResistorInfoArray(rn).intProbHiNo
                        PrbTbl(rn, 2) = typResistorInfoArray(rn).intProbLoNo
                        PrbTbl(rn, 3) = typResistorInfoArray(rn).intProbAGNo1
                        PrbTbl(rn, 4) = typResistorInfoArray(rn).intProbAGNo2
                        PrbTbl(rn, 5) = typResistorInfoArray(rn).intProbAGNo3
                        PrbTbl(rn, 6) = typResistorInfoArray(rn).intProbAGNo4
                        PrbTbl(rn, 7) = typResistorInfoArray(rn).intProbAGNo5
                        ' 目標値
                        NomTbl(rn) = typResistorInfoArray(rn).dblTrimTargetVal
                        ' 電圧変化スロープ(1:+スロープ, 2:-スロープ, 4:抵抗)
                        SlpTbl(rn) = typResistorInfoArray(rn).intSlope
                        'If (.intMeasType = 0) Then '###011
                        If (typResistorInfoArray(rn).intResMeasMode = 0) Then ' 測定モード(0:抵抗 ,1:電圧 ,2:外部) '###011
                            SlpTbl(rn) = 4
                        End If
                        ' カット開始位置X,Yテーブル
                        StPosXTbl(rn) = typResistorInfoArray(rn).ArrCut(1).dblStartPointX
                        StPosYTbl(rn) = typResistorInfoArray(rn).ArrCut(1).dblStartPointY

                        ' ↓↓↓ V3.1.0.0② 2014/12/01
                        ' 測定範囲
                        If giMeasurement = 1 Then           ' イニシャルテスト
                            ' HIGH
                            FtTbl(rn, 1) = typResistorInfoArray(rn).dblInitTest_HighLimit
                            ' LOW
                            FtTbl(rn, 2) = typResistorInfoArray(rn).dblInitTest_LowLimit
                        ElseIf giMeasurement = 2 Then       ' ファイナルテスト
                            ' HIGH
                            FtTbl(rn, 1) = typResistorInfoArray(rn).dblFinalTest_HighLimit
                            ' LOW
                            FtTbl(rn, 2) = typResistorInfoArray(rn).dblFinalTest_LowLimit
                        Else                                ' 今まで通り
                            ' HIGH
                            FtTbl(rn, 1) = gdRESISTOR_MAX
                            ' LOW
                            FtTbl(rn, 2) = gdRESISTOR_MIN
                        End If
                        ' ↑↑↑ V3.1.0.0② 2014/12/01

                        gCntRegData = gCntRegData + 1  'V6.0.1.023

                    End If
                Next rn

                ' 'V6.0.1.023　プローブに必要な情報を渡す 
                Probe1.SetTrimmingDataName(gStrTrimFileName, gCntRegData)
                Form1.Instance.VideoLibrary1.SetCorrCrossVisible(False)                'V6.0.4.0②

                '--------------------------------------------------------------------------
                '   プローブＯＣＸにデータを渡す
                '--------------------------------------------------------------------------
                Probe1.SetMainObject(parModules)                ' 親モジュールのメソッドを設定する
                r = Probe1.Setup(RnoTbl, W_bpox, W_bpoy, .dblBlockSizeXDir, .dblBlockSizeYDir,
                                 .dblTableOffsetXDir, .dblTableOffsetYDir,
                                 .dblZOffSet, .dblZWaitOffset,
                                 PrbTbl, SlpTbl, NomTbl, FtTbl, 0, StPosXTbl, StPosYTbl, 7500, 105,
                                 gfCorrectPosX, gfCorrectPosY,
                                 .intBlockCntXDir, .intBlockCntYDir,
                                 .dblLwPrbStpUpDist, 0.0)

                If (r <> cFRS_NORMAL) Then                      ' エラー ?
                    ProbeTeaching = r                           ' Return値設定
                    strDAT = "Setup() "
                    GoTo STP_ERR
                End If

                '--------------------------------------------------------------------------
                '   θマニュアル調整初期化(手動補正モードで、補正なしの時有効)
                '   注）Setup()の後にCallする
                '--------------------------------------------------------------------------
                ' 補正モードが手動で補正なし?
                If (.intReviseMode = 1) And (.intManualReviseType = 0) Then
                    ManualThetaFlg = 1                          ' θﾏﾆｭｱﾙ調整を有効とする
                Else
                    ManualThetaFlg = 0                          ' θﾏﾆｭｱﾙ調整を無効とする
                End If

                ' θマニュアル調整初期化
                r = Probe1.SetupTheta(ManualThetaFlg, .intReviseMode, .intManualReviseType,
                                      cThetaAnglMin, cThetaAnglMax, .dblRotateTheta)
                If (r <> cFRS_NORMAL) Then                      ' エラー ?
                    ProbeTeaching = r                           ' Return値設定
                    strDAT = "SetupTheta() "
                    GoTo STP_ERR
                End If

                '--------------------------------------------------------------------------
                '   プローブ接触位置確認機能(ｵﾌﾟｼｮﾝ)前処理
                '--------------------------------------------------------------------------
                ' プローブ接触位置確認用ﾃｰﾌﾞﾙを設定する
                For rn = 1 To gRegistorCnt                      ' 抵抗数分設定する
                    ' プローブ確認位置HI X,Y座標 
                    PrbHiTBL(rn, 1) = typResistorInfoArray(rn).dblProbCfmPoint_Hi_X
                    PrbHiTBL(rn, 2) = typResistorInfoArray(rn).dblProbCfmPoint_Hi_Y
                    PrbLoTBL(rn, 1) = typResistorInfoArray(rn).dblProbCfmPoint_Lo_X
                    PrbLoTBL(rn, 2) = typResistorInfoArray(rn).dblProbCfmPoint_Lo_Y
                Next rn

                ' プローブ接触位置確認機能初期設定(ｵﾌﾟｼｮﾝ)
                r = Probe1.SetupPrbPosChk(gRegistorCnt, .dblAdjOffSetXDir, .dblAdjOffSetYDir,
                                          65, 650, PrbHiTBL, PrbLoTBL)

                If (r <> cFRS_NORMAL) Then                      ' エラー ?
                    ProbeTeaching = r                           ' Return値設定
                    strDAT = "SetupPrbPosChk() "
                    GoTo STP_ERR
                End If

                '----- ###262↓ -----
                r = SendTrimData()
                If (r <> cFRS_NORMAL) Then
                    ' "トリミングデータの設定に失敗しました。" & vbCrLf & "トリミングデータに問題がないか確認してください。"
                    Call Me.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    ProbeTeaching = cFRS_ERR_RST                ' Return値 = RESET
                    GoTo STP_ERR
                End If
                '----- ###262↑ -----

                '--------------------------------------------------------------------------
                '   プローブティーチング画面表示
                '--------------------------------------------------------------------------
                Probe1.Visible = True
                Probe1.BringToFront()                           ' 最前面へ表示
                r = Probe1.START()                              ' プローブ調整
                Probe1.Visible = False
                If (r <> cFRS_NORMAL) Then                      ' エラー ?
                    ProbeTeaching = r
                    strDAT = "START() "
                    GoTo STP_ERR
                End If

                '--------------------------------------------------------------------------
                '   プローブ調整結果を取得する
                '--------------------------------------------------------------------------
                ' プローブ調整結果取得(ﾄﾘﾑ位置ｵﾌｾｯﾄX,Y/ZON位置,Z2ON位置更新)
                ''V1.13.0.0④ r = Probe1.GetResult(.dblTableOffsetXDir, .dblTableOffsetYDir, _
                '                     .dblZOffSet, WK_Z2ON)
                r = Probe1.GetResult(.dblTableOffsetXDir, .dblTableOffsetYDir,
                                     .dblZOffSet, .dblLwPrbStpUpDist)

                If (r = cFRS_NORMAL) Then                       ' 正常(Cancelでない) ?

                    '' PROBE2ｺﾏﾝﾄﾞならXY移動分BPもずらす(Teachｺﾏﾝﾄﾞがない為)
                    'If (giAppMode = APP_MODE_PROBE2) Then       ' PROBE2ｺﾏﾝﾄﾞ ?
                    '    W_XCor = W_Xoff - (stPLT.z_xoff - gfCorrectPosX) ' W_Xoff = XY移動分 X
                    '    W_YCor = W_Yoff - (stPLT.z_yoff - gfCorrectPosY) ' W_Yoff = XY移動分 Y
                    '    stPLT.BPOX = W_bpox - W_XCor            ' BP Offset X(mm)±ずれ量X
                    '    stPLT.BPOY = W_bpoy + W_YCor            ' BP Offset Y(mm)±ずれ量Y
                    'End If

                    ' ﾄﾘﾑ位置ｵﾌｾｯﾄX,Yを更新する(XYﾃｰﾌﾞﾙ補正分を引く)
                    .dblTableOffsetXDir = .dblTableOffsetXDir - gfCorrectPosX
                    .dblTableOffsetYDir = .dblTableOffsetYDir - gfCorrectPosY
                    'V4.7.0.0⑮↓
                    '----- V6.1.4.0⑯↓(KOA EW殿SL432RD対応) -----
                    ' BP_INPUT=1(テーブルオフセット値の逆数をBPオフセットに設定) ?
                    If (gSysPrm.stCTM.giBPOffsetInput <> 0) Then
                        .dblBpOffSetXDir = -1.0 * .dblTableOffsetXDir
                        .dblBpOffSetYDir = -1.0 * .dblTableOffsetYDir
                    End If
                    '----- V6.1.4.0⑯↑ -----
                    ' システム変数設定(プローブON/OFF位置他)
                    Call PROP_SET(.dblZOffSet, .dblZWaitOffset,
                                  gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, gSysPrm.stDEV.gfSmaxX, gSysPrm.stDEV.gfSmaxY)
                    gCmpTrimDataFlg = 1                         ' データ更新フラグ = 1(更新あり)
                    commandtutorial.SetProbeExecute()           ' V2.0.0.0⑩
                End If

                ' θマニュアル調整結果取得(θ回転角度更新)
                If (ManualThetaFlg = 1) Then                    ' ###258θﾏﾆｭｱﾙ調整が有効の時に取得する
                    r = Probe1.GetResultTheta(.dblRotateTheta)
                    If (r = cFRS_NORMAL) Then                   ' 正常 ?
                        gCmpTrimDataFlg = 1                     ' データ更新フラグ = 1(更新あり)
                    End If
                End If                                          ' ###258

                ' プローブティーチング結果(プローブ接触位置)を取得する(ｵﾌﾟｼｮﾝ)
                r = Probe1.GetResultPrbPosChk(PrbHiTBL, PrbLoTBL)
                If (r = cFRS_NORMAL) Then                       ' 正常 ?
                    For rn = 1 To gRegistorCnt                  ' 抵抗数分設定する
                        ' プローブ確認位置HI X,Y座標更新 
                        typResistorInfoArray(rn).dblProbCfmPoint_Hi_X = PrbHiTBL(rn, 1)
                        typResistorInfoArray(rn).dblProbCfmPoint_Hi_Y = PrbHiTBL(rn, 2)
                        typResistorInfoArray(rn).dblProbCfmPoint_Lo_X = PrbLoTBL(rn, 1)
                        typResistorInfoArray(rn).dblProbCfmPoint_Lo_Y = PrbLoTBL(rn, 2)
                        gCmpTrimDataFlg = 1                     ' データ更新フラグ = 1(更新あり)
                    Next rn
                End If

                Call TRIMEND()                                  ' メモリ開放 ###262
                ProbeTeaching = r                               'V4.10.0.0③
                Exit Function

STP_ERR:
                ' プローブＯＣＸエラー時
                'strMSG = "i-TKY.ProbeTeaching() PROBE.OCX ERROR = " + strDAT + r.ToString
                'MsgBox(strMSG)
                'ProbeTeaching = cERR_TRAP                       ' Return値 = 例外エラー
                Call TRIMEND()                                  ' メモリ開放 ###262
                Exit Function
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.ProbeTeaching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            ProbeTeaching = cERR_TRAP                           ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "ﾃｨｰﾁﾝｸﾞ(F8)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ﾃｨｰﾁﾝｸﾞ(F8)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTeach_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTeach.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' 補正クロスライン表示
            ' ※補正クロスライン表示はVB6と.NETでPictueBoxの形式が異なるためVideo.OCXのクロスラインを使用する
            'VideoLibrary1.gPicture1.Visible = True              ' Video.OCXのクロスラインを表示する
            'VideoLibrary1.gPicture2.Visible = True
            'Picture1.Visible = True                            ' Form1のクロスラインは非表示とする
            'Picture2.Visible = True
            'CrosLineX.Visible = True
            'CrosLineY.Visible = True

            ' コマンド実行
            r = CmdExec_Proc(F_TEACH, APP_MODE_TEACH, MSG_OPLOG_FUNC08, "")

            '' 補正クロスライン非表示
            'Picture1.Visible = True                             ' Form1のクロスラインを表示する
            'Picture2.Visible = True
            'VideoLibrary1.gPicture1.Visible = False             ' Video.OCXのクロスラインを非表示とする
            'VideoLibrary1.gPicture2.Visible = False
            'VideoLibrary1.gCrosLineXY1.Visible = False
            'VideoLibrary1.gCrosLineXY2.Visible = False

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTeach_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "ティーチング処理"
    '''=========================================================================
    '''<summary>ティーチング処理</summary>
    '''<param name="iAppMode">(INP)アプリモード(giAppMode参照)
    '''                            APP_MODE_TEACH         = ティーチング
    '''                            APP_MODE_EXCAM_R1TEACH = 外部カメラR1ティーチング
    '''                            APP_MODE_EXCAM_TEACH   = 外部カメラティーチング</param> 
    '''<param name="pltInfo"> (I/O)プレートデータ</param> 
    '''<returns>0=正常, 0以外=エラー</returns>
    '''<remarks>Teach.OCXを使用</remarks>
    '''=========================================================================
    Private Function Teaching(ByVal iAppMode As Short, ByRef pltInfo As PlateInfo) As Short

        Dim strMSG As String
        Dim strDAT As String
        Dim r As Short
        Dim idx As Short
        Dim rn As Short
        Dim cn As Short
        Dim dirL1 As Short                                              ' カット方向(1:-X←,2:+Y↑, 3:+X:→, 4:-Y↓)
        Dim dirL2 As Short
        Dim dblTmpBpX As Double
        Dim dblTmpBpY As Double
        Dim sRName() As String                                          ' 抵抗名(抵抗番号)テーブル
        Dim RnCnTbl(,) As Short                                         ' 抵抗番号,カット番号テーブル
        Dim StartPosTbl(,) As Double                                    ' カット開始位置テーブル
        Dim TblOfsX As Double
        Dim TblOfsY As Double
        Dim dStartPointX As Double, dStartPointY As Double              ' V6.1.4.0⑮

        ' 外部カメラティーチング用パラメータ
        Dim stPLT As TEACH_PLATE_INFO                                   ' プレート情報 (※Teach.OCXで定義)
        Dim stSTP(MaxCntStep) As TEACH_STEP_INFO                        ' ステップ情報
        Dim stGRP(MaxCntStep) As TEACH_GROP_INFO                        ' グループ情報
        Dim stTY2(MaxCntTy2) As TEACH_TY2_INFO                          ' ＴＹ２情報

        Dim dX As Double, dY As Double                                  'V5.0.0.6⑫
        Dim arrayCnt As Integer
        Dim parModules As MainModules
        parModules = New MainModules

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   ティーチングＯＣＸに渡すカット開始位置テーブルを作成する
                '--------------------------------------------------------------------------
                idx = 0                                                 ' テーブルインデックス

                'カット開始位置保存テーブルを動的に確保
                '   全抵抗のカット数を計算する。
                For rn = 1 To gRegistorCnt
                    arrayCnt = arrayCnt + typResistorInfoArray(rn).intCutCount
                Next
                ReDim sRName(arrayCnt)
                ReDim RnCnTbl(arrayCnt, 2)
                ReDim StartPosTbl(2, arrayCnt)

                For rn = 1 To gRegistorCnt                              ' 抵抗数分設定する 
                    If (typResistorInfoArray(rn).intResNo < 1) Then Exit For
                    ' 抵抗内カット数分設定する
                    For cn = 1 To typResistorInfoArray(rn).intCutCount
                        idx = idx + 1                                   ' テーブルインデックス + 1
                        ' 抵抗名(抵抗番号)
                        sRName(idx) = typResistorInfoArray(rn).intResNo.ToString()
                        ' 抵抗番号,カット番号テーブル
                        RnCnTbl(idx, 0) = rn                            ' 抵抗番号
                        RnCnTbl(idx, 1) = cn                            ' カット番号
                        ' カット開始位置X,Yテーブル(StartPosTbl)を設定する
                        If (iAppMode = APP_MODE_TEACH) Then
                            ' ティーチングの場合
                            StartPosTbl(1, idx) = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX
                            StartPosTbl(2, idx) = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY
                        Else
                            ' 外部カメラR1ティーチング/外部カメラティーチングの場合
                            '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                            'If gSysPrm.stCTM.giTEACH_P <> 0 Then      ' ティーチングポイント使用の時
                            If (gSysPrm.stCTM.giTEACH_P = 1) Then      ' ティーチングポイント/スタートポイントの両方変更?
                                ' dStartPointX,YにティーチングポイントX,Yを退避する
                                dStartPointX = typResistorInfoArray(rn).ArrCut(cn).dblTeachPointX
                                dStartPointY = typResistorInfoArray(rn).ArrCut(cn).dblTeachPointY
                            Else
                                ' dStartPointX,YにスタートポイントX,Yを退避する
                                dStartPointX = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX
                                dStartPointY = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY
                            End If
                            '----- V6.1.4.0⑮↑ -----
                            '----- V1.14.0.0④↓ -----
                            ' 外部カメラの時
                            '----- V6.1.4.0⑮↓ -----
                            ' dX,dY=スタートポイントX,Y又はティーチングポイントX,Y 
                            'dX = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX 'V5.0.0.6⑫
                            'dY = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY 'V5.0.0.6⑫
                            dX = dStartPointX
                            dY = dStartPointY
                            '----- V6.1.4.0⑮↑ -----
                            UpdateByCrossLineToExt(dX, dY)                          'V5.0.0.6⑫

                            ' ティーチング画面のティーチング位置X,Yを設定
                            Select Case gSysPrm.stDEV.giBpDirXy
                                Case 0 ' x←, y↓
                                    StartPosTbl(1, idx) = dX
                                    StartPosTbl(2, idx) = dY
                                Case 1 ' x→, y↓(X反転)
                                    StartPosTbl(1, idx) = dX * -1
                                    StartPosTbl(2, idx) = dY
                                Case 2 ' x←, y↑(Y反転)
                                    StartPosTbl(1, idx) = dX
                                    StartPosTbl(2, idx) = dY * -1
                                Case 3 ' x→, y↑(XY反転)
                                    StartPosTbl(1, idx) = dX * -1
                                    StartPosTbl(2, idx) = dY * -1
                            End Select
                            '----- V1.14.0.0④↑ -----
                        End If
                    Next cn
                Next rn

                '--------------------------------------------------------------------------
                '   ティーチングＯＣＸにデータを渡す
                '--------------------------------------------------------------------------
                ' クロスライン表示用  ###232
                r = Teaching1.SetCrossLineObject(gparModules)
                If (r <> cFRS_NORMAL) Then
                    MsgBox("i-TKY.teaching() SetCrossLineObject ERROR")
                End If

                ' ティーチングＯＣＸに画像表示プログラムの表示位置を渡す ###052
                Teaching1.dispXPos = VideoLibrary1.Location.X
                Teaching1.dispYPos = VideoLibrary1.Location.Y

                ' ティーチングＯＣＸにデータを渡す
                r = Teaching1.Setup(RnCnTbl, .dblBpOffSetXDir, .dblBpOffSetYDir,
                                  .dblBlockSizeXDir, .dblBlockSizeYDir, sRName, gSysPrm.stDEV.giBpDirXy, iAppMode)  ' iAppMode追加 V6.1.4.0⑮
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Teaching = r                                        ' Return値設定
                    strDAT = "Setup() "
                    GoTo STP_ERR
                End If
                ''V6.0.1.0⑭↓
                ' ティーチングＯＣＸにデータを渡す
                'r = Teaching1.SetQrate(stCND.Freq(ADJ_CND_NUM))
                ''V6.0.1.0⑭↑

                '----- V1.13.0.0①↓ -----
                r = Teaching1.SetZ2Pos(.dblLwPrbStpUpDist, 0.0)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Teaching = r                                        ' Return値設定
                    GoTo STP_ERR
                End If
                '----- V1.13.0.0①↑ -----

                ' カット毎のカット位置XYデータをコントロールに渡す
                idx = 0                                                 ' テーブルインデックス
                For rn = 1 To gRegistorCnt                              ' 抵抗数分設定する 
                    ' 抵抗内カット数分繰返す
                    For cn = 1 To typResistorInfoArray(rn).intCutCount
                        idx = idx + 1                                   ' テーブルインデックス + 1
                        ' カットトレースのためのカット方向を設定する
                        Call Cnv_Cut_Dir(rn, cn, dirL1, dirL2)
                        ' 各カットトレースのためセットアップ処理
                        Call Sub_Cut_Setup(rn, cn, dirL1, dirL2, RnCnTbl, StartPosTbl, idx)
                    Next cn
                Next rn

                ' 外部カメラティーチング時は外部カメラに切替える
                If (iAppMode = APP_MODE_TEACH) Then
                    TblOfsX = 0.0#
                    TblOfsY = 0.0#
                Else
                    ' XYテーブルを第一抵抗の第一カット位置へ移動する(外部カメラティーチング時)
                    '----- ###249↓ -----
                    'TblOfsX = gSysPrm.stDEV.gfExCmX                     ' 外部カメラオフセットX設定 ###075
                    'TblOfsY = gSysPrm.stDEV.gfExCmY                     ' 外部カメラオフセットY設定 ###075
                    ' ブロックサイズ/2を減算しＢＰオフセット分加算する ###075
                    'TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                    'TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir

                    '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                    'If gSysPrm.stCTM.giTEACH_P <> 0 Then               ' ティーチングポイント使用の時
                    If (gSysPrm.stCTM.giTEACH_P = 1) Then               ' ティーチングポイント/スタートポイントの両方変更?
                        ' dStartPointX,Yに第1抵抗の第1カットのティーチングポイントX,Yを退避する
                        dStartPointX = typResistorInfoArray(1).ArrCut(1).dblTeachPointX
                        dStartPointY = typResistorInfoArray(1).ArrCut(1).dblTeachPointY
                    Else
                        ' dStartPointX,Yに第1抵抗の第1カットのスタートポイントX,Yを退避する
                        dStartPointX = typResistorInfoArray(1).ArrCut(1).dblStartPointX
                        dStartPointY = typResistorInfoArray(1).ArrCut(1).dblStartPointY
                    End If
                    '----- V6.1.4.0⑮↑ -----
                    '----- V6.1.4.0⑮↓ -----
                    ' BP基準コーナーを考慮(INtime側Cmd_Start()参照)
                    Select Case gSysPrm.stDEV.giBpDirXy
                        Case 0 ' 右上(x←, y↓)
                            TblOfsX = gSysPrm.stDEV.gfExCmX + dStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                            TblOfsY = gSysPrm.stDEV.gfExCmY + dStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                            '----- V1.24.0.0①↓ -----
                            If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                                TblOfsX = gSysPrm.stDEV.gfExCmX + dStartPointX + .dblBpOffSetXDir
                                TblOfsY = gSysPrm.stDEV.gfExCmY + dStartPointY + .dblBpOffSetYDir
                            End If
                            '----- V1.24.0.0①↑ -----
                        Case 1 ' 左上(x→, y↓)
                            TblOfsX = (gSysPrm.stDEV.gfExCmX - dStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                            TblOfsY = gSysPrm.stDEV.gfExCmY + dStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                        Case 2 ' 右下(x←, y↑)
                            TblOfsX = gSysPrm.stDEV.gfExCmX + dStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                            TblOfsY = (gSysPrm.stDEV.gfExCmY + dStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                        Case 3 ' 左下(x→, y↑)
                            TblOfsX = (gSysPrm.stDEV.gfExCmX + dStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                            TblOfsY = (gSysPrm.stDEV.gfExCmY + dStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                    End Select

                    'Select Case gSysPrm.stDEV.giBpDirXy
                    '    Case 0 ' 右上(x←, y↓)
                    '        TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                    '        TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                    '        '----- V1.24.0.0①↓ -----
                    '        If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                    '            TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX + .dblBpOffSetXDir
                    '            TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY + .dblBpOffSetYDir
                    '        End If
                    '        '----- V1.24.0.0①↑ -----
                    '    Case 1 ' 左上(x→, y↓)
                    '        TblOfsX = (gSysPrm.stDEV.gfExCmX - typResistorInfoArray(1).ArrCut(1).dblStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                    '        TblOfsY = gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY - (.dblBlockSizeYDir / 2) + .dblBpOffSetYDir
                    '    Case 2 ' 右下(x←, y↑)
                    '        TblOfsX = gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX - (.dblBlockSizeXDir / 2) + .dblBpOffSetXDir
                    '        TblOfsY = (gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                    '    Case 3 ' 左下(x→, y↑)
                    '        TblOfsX = (gSysPrm.stDEV.gfExCmX + typResistorInfoArray(1).ArrCut(1).dblStartPointX + (.dblBlockSizeXDir / 2) - .dblBpOffSetXDir) * -1
                    '        TblOfsY = (gSysPrm.stDEV.gfExCmY + typResistorInfoArray(1).ArrCut(1).dblStartPointY + (.dblBlockSizeYDir / 2) - .dblBpOffSetYDir) * -1
                    'End Select
                    '----- ###249↑ -----
                    '----- V6.1.4.0⑮↑ -----

                    ' 外部カメラに切替える
                    Call VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)
                End If

                '--------------------------------------------------------------------------
                '   θ補正(ｵﾌﾟｼｮﾝ) & XYテーブル & Zをトリム位置へ移動する
                '--------------------------------------------------------------------------
                Teaching = cFRS_NORMAL                                  ' Return値 = 正常
                'Call BSIZE(stPLT.zsx, stPLT.zsy)                       ' ブロックサイズ設定
                r = Move_Trimposition(pltInfo, TblOfsX, TblOfsY)        ' θ補正(ｵﾌﾟｼｮﾝ) & XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動
                SetTeachVideoSize()                                     ' V6.0.3.0_49
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Teaching = r                                        ' Return値設定
                    Exit Function
                End If

                ' BPｵﾌｾｯﾄ設定
                'Call System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then GoTo STP_END

                ' 外部カメラティーチング用パラメータ設定
                If (iAppMode = APP_MODE_EXCAM_R1TEACH) Or (iAppMode = APP_MODE_EXCAM_TEACH) Then
                    Call VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)                  ' 外部カメラに切替える'V1.14.0.0①
                    'V6.1.4.9①                    Call SetExCamTeachParam(iAppMode, pltInfo, stPLT, stSTP, stGRP, stTY2)
                    Call SetExCamTeachParam(iAppMode, pltInfo, stPLT, stSTP, stGRP, stTY2, gTkyKnd)  'V6.1.4.9① gTkyKnd追加
                    Call Teaching1.SetupExcamTeach(iAppMode, stPLT, stSTP, stGRP, stTY2)
                End If

                '--------------------------------------------------------------------------
                '   ティーチング画面表示
                '--------------------------------------------------------------------------
                Teaching1.ZOFF = .dblZWaitOffset                        ' Z PROBE OFF OFFSET(mm)
                Teaching1.ZON = .dblZOffSet                             ' Z PROBE ON OFFSET(mm)
                ' マーキング位置の表示のため、ビデオライブラリの描画オブジェクトを渡す
                'Teaching1.set_formObject(VideoLibrary1.shpObject)
                ' 親モジュールのメソッドを設定する。
                Teaching1.SetMainObject(parModules)

                ''ZOff位置へ移動
                'r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)

                SetTeachVideoSize()                                     'V2.0.0.0⑱

                ' ティーチングのコントロールを表示する
                Teaching1.Visible = True
                Teaching1.BringToFront()                                ' 最前面へ表示

                ''V1.16.0.1②
                ' 加工条件番号を設定する(FL時)
                If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then       ' FL ?
                    Call QRATE(stCND.Freq(ADJ_CND_NUM))                 ' Qレート設定(KHz)
                    r = FLSET(FLMD_CNDSET, ADJ_CND_NUM)                 ' 加工条件番号設定(一時停止画面用)
                Else
                    Call QRATE(gSysPrm.stDEV.gfLaserQrate)              ' Qレート設定(KHz) ※レーザ調整用Qレートを設定
                    ''V6.0.1.0⑭↓
                    ' ティーチングＯＣＸにデータを渡す
                    r = Teaching1.SetQrate(gSysPrm.stDEV.gfLaserQrate)
                    ''V6.0.1.0⑭↑

                End If
                ''V1.16.0.1②

                ' ティーチング処理を実行する
                Select Case (iAppMode)
                    Case APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH
                        r = Teaching1.StartExcamTeach(iAppMode)         ' 外部カメラティーチング
                    Case Else
                        r = Teaching1.START()                           ' ティーチング
                End Select

                ' ティーチングのコントロールを非表示にする
                Teaching1.Visible = False

                '--------------------------------------------------------------------------
                '   ティーチング結果取得
                '--------------------------------------------------------------------------
                If (r = cFRS_NORMAL) Then                               ' ティーチング処理正常終了 ?
                    If (Teaching1.Getresult(dblTmpBpX, dblTmpBpY, StartPosTbl) = 0) Then
                        ' ビームポジションオフセット値更新
                        If (iAppMode = APP_MODE_TEACH) Then
                            .dblBpOffSetXDir = dblTmpBpX
                            .dblBpOffSetYDir = dblTmpBpY
                        End If

                        If (iAppMode = APP_MODE_TEACH Or iAppMode = APP_MODE_EXCAM_TEACH) Then
                            ' 抵抗数分カット位置XYを設定する
                            idx = 0                                         ' テーブルインデックス
                            For rn = 1 To gRegistorCnt                      ' 抵抗数分設定する 
                                ' 抵抗内カット数分設定する
                                For cn = 1 To typResistorInfoArray(rn).intCutCount
                                    idx = idx + 1                           ' テーブルインデックス + 1
                                    '                                       ' カット位置XY更新
                                    If (iAppMode = APP_MODE_TEACH) Then
                                        typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx)
                                        typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx)
                                    Else
                                        '----- V1.14.0.0④↓ -----
                                        ' 外部カメラの時
                                        UpdateByCrossLineToIN(StartPosTbl(1, idx), StartPosTbl(2, idx)) 'V5.0.0.6⑫
                                        '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                                        ' dStartPointX,Y = スタートポイントX,YまたはティーチングポイントX,Y
                                        Select Case gSysPrm.stDEV.giBpDirXy
                                            Case 0 ' x←, y↓
                                                dStartPointX = StartPosTbl(1, idx)
                                                dStartPointY = StartPosTbl(2, idx)
                                            Case 1 ' x→, y↓(X反転)
                                                dStartPointX = StartPosTbl(1, idx) * -1
                                                dStartPointY = StartPosTbl(2, idx)
                                            Case 2 ' x←, y↑(Y反転)
                                                dStartPointX = StartPosTbl(1, idx)
                                                typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx) * -1
                                            Case 3 ' x→, y↑(XY反転)
                                                dStartPointX = StartPosTbl(1, idx) * -1
                                                dStartPointY = StartPosTbl(2, idx) * -1

                                                'Case 0 ' x←, y↓
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx)
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx)
                                                'Case 1 ' x→, y↓(X反転)
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx) * -1
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx)
                                                'Case 2 ' x←, y↑(Y反転)
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx)
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx) * -1
                                                'Case 3 ' x→, y↑(XY反転)
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = StartPosTbl(1, idx) * -1
                                                '    typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = StartPosTbl(2, idx) * -1
                                        End Select
                                        '----- V1.14.0.0④↑ -----

                                        'If gSysPrm.stCTM.giTEACH_P <> 0 Then      ' ティーチングポイント使用の時
                                        If (gSysPrm.stCTM.giTEACH_P = 1) Then      ' ティーチングポイント/スタートポイントの両方変更?
                                            ' スタートポイントX,Yを、ティーチングポイントとの差分を加算して更新
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = typResistorInfoArray(rn).ArrCut(cn).dblStartPointX + (dStartPointX - typResistorInfoArray(rn).ArrCut(cn).dblTeachPointX)
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = typResistorInfoArray(rn).ArrCut(cn).dblStartPointY + (dStartPointY - typResistorInfoArray(rn).ArrCut(cn).dblTeachPointY)
                                            ' ティーチングポイントX,Y更新
                                            typResistorInfoArray(rn).ArrCut(cn).dblTeachPointX = dStartPointX
                                            typResistorInfoArray(rn).ArrCut(cn).dblTeachPointY = dStartPointY
                                        Else
                                            ' スタートポイントX,Y更新
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointX = dStartPointX
                                            typResistorInfoArray(rn).ArrCut(cn).dblStartPointY = dStartPointY
                                        End If
                                        '----- V6.1.4.0⑮↑ -----
                                    End If
                                Next cn
                            Next rn
                        ElseIf (iAppMode = APP_MODE_EXCAM_R1TEACH) Then
                            'R1ティーチングの場合、先頭抵抗のデータを全抵抗へ展開
                            Call SetR1Data2AllResistor(StartPosTbl)
                        End If
                        gCmpTrimDataFlg = 1                             ' データ更新フラグ = 1(更新あり)
                        commandtutorial.SetTeachExecute()               ' V2.0.0.0⑩
                    End If
                Else
                    Teaching = r                                        ' Return値 =cFRS_ERR_RST(キャンセル)　それ以外= エラー
                End If

                ' 外部カメラティーチング時は内部カメラに切替える
                If (iAppMode <> APP_MODE_TEACH) Then
                    Call VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)                  ' 内部カメラに切替える
                End If

                'SetSimpleVideoSize()           'V2.0.0.0⑱  'V4.10.0.0⑫ CmdExec_Proc()でおこなっているためここではNOP

                'V6.0.0.0④                Me.CrosLineX.Visible = False                            ' 補正クロスライン非表示
                'V6.0.0.0④                Me.CrosLineY.Visible = False
                VideoLibrary1.SetCorrCrossVisible(False)                'V6.0.0.0④
                Exit Function

STP_ERR:
                ' ティーチングＯＣＸエラー時
                'strMSG = "i-TKY.Teaching() TEACH.OCX ERROR = " + strDAT + r.ToString
                'MsgBox(strMSG)
                'Teaching = cERR_TRAP                                    ' Return値 = 例外エラー
                Exit Function
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Teaching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Teaching = cERR_TRAP                                        ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region ""
    'V5.0.0.6⑫ 外部カメラキャリブレーションにクロスライン補正を加味する。↓
    Private Sub UpdateByCrossLineToExt(ByRef X As Double, ByRef Y As Double)
        Try
            If gSysPrm.stCRL.giDspFlg = 0 Then      ' 収差補正無し
                Exit Sub
            End If
            Dim dX As Double, dY As Double, dPixX As Double, dPixY As Double
            dX = X + typPlateInfo.dblBpOffSetXDir + (82.0 - typPlateInfo.dblBlockSizeXDir) / 2.0
            dY = Y + typPlateInfo.dblBpOffSetYDir + (82.0 - typPlateInfo.dblBlockSizeYDir) / 2.0
            ObjCrossLine.GetCorrectPixelXY(dX, dY, dPixX, dPixY)
            X = X - (dPixX * gSysPrm.stGRV.gfPixelX / 1000.0)
            Y = Y - (dPixY * gSysPrm.stGRV.gfPixelY / 1000.0)
            ' INTRIMに設定されている現状のキャリブレーション補整値を取得
            'r = BP_GET_CALIBDATA(dblCurGain(0), dblCurGain(1), dblCurCalibOff(0), dblCurCalibOff(1))

        Catch ex As Exception
            MsgBox("i-UpdateByCrossLineToExt() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub UpdateByCrossLineToIN(ByRef X As Double, ByRef Y As Double)
        Try
            Dim dX As Double, dY As Double, dPixX As Double, dPixY As Double
            If gSysPrm.stCRL.giDspFlg = 0 Then      ' 収差補正無し
                Exit Sub
            End If
            dX = X + typPlateInfo.dblBpOffSetXDir + (82.0 - typPlateInfo.dblBlockSizeXDir) / 2.0
            dY = Y + typPlateInfo.dblBpOffSetYDir + (82.0 - typPlateInfo.dblBlockSizeYDir) / 2.0
            ObjCrossLine.GetCorrectPixelXY(dX, dY, dPixX, dPixY)
            X = X + (dPixX * gSysPrm.stGRV.gfPixelX / 1000.0)
            Y = Y + (dPixY * gSysPrm.stGRV.gfPixelY / 1000.0)
            ' INTRIMに設定されている現状のキャリブレーション補整値を取得
            'r = BP_GET_CALIBDATA(dblCurGain(0), dblCurGain(1), dblCurCalibOff(0), dblCurCalibOff(1))
        Catch ex As Exception
            MsgBox("i-TKY.UpdateByCrossLineToIN() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    'V5.0.0.6⑫ ↑
#End Region

#Region "外部R1ティーチングの1カット目のパラメータを全抵抗に展開する"
    '''=========================================================================
    ''' <summary>外部R1ティーチングの1カット目のパラメータを全抵抗に展開する</summary>
    '''=========================================================================
    Private Sub SetR1Data2AllResistor(ByRef startPosTbl(,) As Double)

        Dim strMsg As String
        Dim deltaX(), deltaY() As Double
        Dim maxCutCnt As Integer
        Dim cutCnt As Integer
        Dim rno As Integer
        Dim dStartPointX As Double, dStartPointY As Double              ' V6.1.4.0⑮
        Dim iCircuitCnt As Integer                                      'V6.1.4.9①　typResistorInfoArray(1)をtypResistorInfoArray(iCircuitCnt)に置き換え
        Dim iMaxCircuitCnt As Integer                                   'V6.1.4.9①　サーキット内抵抗数
        Dim iTotalCutCnt As Integer = 0                                 'V6.1.4.9①　積算カット数（DllTeach内のテーブル配列)
        Dim iNgMarkNo As Integer = 0                                    'V6.1.4.9①  ＮＧマークは、第１抵抗第１カットで更新する。

        Try
            'V6.1.4.9①↓
            If gTkyKnd = KND_CHIP Then
                iMaxCircuitCnt = 1
            Else
                iMaxCircuitCnt = typPlateInfo.intResistCntInGroup
            End If
            For iCircuitCnt = 1 To iMaxCircuitCnt
                'V6.1.4.9①↑

                '現在の先頭データと変更値から差分を換算し、全抵抗に展開する。
                maxCutCnt = typResistorInfoArray(iCircuitCnt).intCutCount
                ReDim Preserve deltaX(maxCutCnt)                            ' 差分格納域X
                ReDim Preserve deltaY(maxCutCnt)                            ' 差分格納域Y

                ' 先頭抵抗のカット数分差分を求める
                For cutCnt = 1 To maxCutCnt                                 ' 先頭抵抗のカット数分繰り返す  
                    iTotalCutCnt = iTotalCutCnt + 1                         ' 以下startPosTblテーブルのcutCntはiTotalCutCntへ変更 'V6.1.4.9①
                    '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                    'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' ティーチングポイント使用の時
                    If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' ティーチングポイント/スタートポイントの両方変更?
                        ' dStartPointX,YにティーチングポイントX,Yを退避する
                        dStartPointX = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointX
                        dStartPointY = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointY
                    Else
                        ' dStartPointX,YにスタートポイントX,Yを退避する
                        dStartPointX = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX
                        dStartPointY = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY
                    End If
                    '----- V6.1.4.0⑮↑ -----
                    ' ----- V1.14.0.0④↓ -----
                    'deltaX(cutCnt) = startPosTbl(1, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                    'deltaY(cutCnt) = startPosTbl(2, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                    UpdateByCrossLineToIN(startPosTbl(1, iTotalCutCnt), startPosTbl(2, iTotalCutCnt)) 'V5.0.0.6⑫

                    ' 差分格納域deltaX,YにスタートポイントX,Y(又はティーチングポイントX,Y)の差分を設定する
                    Select Case gSysPrm.stDEV.giBpDirXy
                    '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                        Case 0 ' x←, y↓
                            deltaX(cutCnt) = startPosTbl(1, iTotalCutCnt) - dStartPointX
                            deltaY(cutCnt) = startPosTbl(2, iTotalCutCnt) - dStartPointY
                        Case 1 ' x→, y↓(X反転)
                            deltaX(cutCnt) = (startPosTbl(1, iTotalCutCnt) * -1) - dStartPointX
                            deltaY(cutCnt) = startPosTbl(2, iTotalCutCnt) - dStartPointY
                        Case 2 ' x←, y↑(Y反転)
                            deltaX(cutCnt) = startPosTbl(1, iTotalCutCnt) - dStartPointX
                            deltaY(cutCnt) = (startPosTbl(2, iTotalCutCnt) * -1) - dStartPointY
                        Case 3 ' x→, y↑(XY反転)
                            deltaX(cutCnt) = (startPosTbl(1, iTotalCutCnt) * -1) - dStartPointX
                            deltaY(cutCnt) = (startPosTbl(2, iTotalCutCnt) * -1) - dStartPointY

                            'Case 0 ' x←, y↓
                            '    deltaX(cutCnt) = startPosTbl(1, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = startPosTbl(2, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            'Case 1 ' x→, y↓(X反転)
                            '    deltaX(cutCnt) = (startPosTbl(1, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = startPosTbl(2, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            'Case 2 ' x←, y↑(Y反転)
                            '    deltaX(cutCnt) = startPosTbl(1, cutCnt) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = (startPosTbl(2, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            'Case 3 ' x→, y↑(XY反転)
                            '    deltaX(cutCnt) = (startPosTbl(1, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX
                            '    deltaY(cutCnt) = (startPosTbl(2, cutCnt) * -1) - typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY
                            '----- V6.1.4.0⑮↑ -----
                    End Select
                    '----- V1.14.0.0④↑ -----

                    ' 先頭のデータを更新
                    ' ----- V1.14.0.0④↓ -----
                    'typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt)
                    'typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt)

                    ' dStartPointX,Y = ティーチング画面のティーチング位置X,Y
                    Select Case gSysPrm.stDEV.giBpDirXy
                    '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                        Case 0 ' x←, y↓
                            dStartPointX = startPosTbl(1, iTotalCutCnt)
                            dStartPointY = startPosTbl(2, iTotalCutCnt)
                        Case 1 ' x→, y↓(X反転)
                            dStartPointX = startPosTbl(1, iTotalCutCnt) * -1
                            dStartPointY = startPosTbl(2, iTotalCutCnt)
                        Case 2 ' x←, y↑(Y反転)
                            dStartPointX = startPosTbl(1, iTotalCutCnt)
                            dStartPointY = startPosTbl(2, iTotalCutCnt) * -1
                        Case 3 ' x→, y↑(XY反転)
                            dStartPointX = startPosTbl(1, iTotalCutCnt) * -1
                            dStartPointY = startPosTbl(2, iTotalCutCnt) * -1

                            'Case 0 ' x←, y↓
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt)
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt)
                            'Case 1 ' x→, y↓(X反転)
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt) * -1
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt)
                            'Case 2 ' x←, y↑(Y反転)
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt)
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt) * -1
                            'Case 3 ' x→, y↑(XY反転)
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointX = startPosTbl(1, cutCnt) * -1
                            '    typResistorInfoArray(1).ArrCut(cutCnt).dblStartPointY = startPosTbl(2, cutCnt) * -1
                            '----- V6.1.4.0⑮↑ -----
                    End Select
                    '----- V1.14.0.0④↑ -----
                    '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                    'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' ティーチングポイント使用の時
                    If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' ティーチングポイント/スタートポイントの両方変更?
                        ' ティーチングポイントX,Y更新
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointX = dStartPointX
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblTeachPointY = dStartPointY
                        ' スタートポイントX,Yを差分を加算して更新
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX + deltaX(cutCnt)
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY = typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY + deltaY(cutCnt)
                    Else
                        'スタートポイントX,Y更新
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointX = dStartPointX
                        typResistorInfoArray(iCircuitCnt).ArrCut(cutCnt).dblStartPointY = dStartPointY
                    End If
                    '----- V6.1.4.0⑮↑ -----
                Next

                ' 抵抗数分2番目以降のカットに対して、差分を加算していく。
                'V6.1.4.9①                For rno = 2 To gRegistorCnt
                For rno = iCircuitCnt + iMaxCircuitCnt To gRegistorCnt Step iMaxCircuitCnt           'V6.1.4.9①
                    If typResistorInfoArray(rno).intResNo > 1000 Then
                        iNgMarkNo = rno
                        Exit For
                    End If
                    For cutCnt = 1 To typResistorInfoArray(rno).intCutCount
                        ' カット数が先頭より多い場合は抜ける
                        If (cutCnt > UBound(deltaX)) Then
                            Exit For
                        End If

                        ' X座標
                        typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointX =
                                    typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointX + deltaX(cutCnt)
                        ' Y座標
                        typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointY =
                                    typResistorInfoArray(rno).ArrCut(cutCnt).dblStartPointY + deltaY(cutCnt)

                        '----- V6.1.4.0⑮↓(KOA EW殿SL432RD対応) -----
                        ' ティーチングポイントを差分を加算して更新
                        'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' ティーチングポイント使用の時
                        If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' ティーチングポイント/スタートポイントの両方変更?
                            typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointX = typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointX + deltaX(cutCnt)
                            typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointY = typResistorInfoArray(rno).ArrCut(cutCnt).dblTeachPointY + deltaY(cutCnt)
                        End If
                        '----- V6.1.4.0⑮↑ -----
                    Next
                Next
                'V6.1.4.9①↓NGマーク番号有りの時は、マーキング位置に第１抵抗第１カットの情報を反映する。
                If iNgMarkNo > 0 And iCircuitCnt = 1 Then
                    'マーキングは、更新しない。
                    'For rno = iNgMarkNo To gRegistorCnt
                    '    For cutCnt = 1 To typResistorInfoArray(rno).intCutCount
                    '        ' X座標
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointX =
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointX + deltaX(1)
                    '        ' Y座標
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointY =
                    '        typResistorInfoArray(rno).ArrCut(1).dblStartPointY + deltaY(1)

                    '        ' ティーチングポイントを差分を加算して更新
                    '        'If gSysPrm.stCTM.giTEACH_P <> 0 Then                   ' ティーチングポイント使用の時
                    '        If (gSysPrm.stCTM.giTEACH_P = 1) Then                   ' ティーチングポイント/スタートポイントの両方変更?
                    '            typResistorInfoArray(rno).ArrCut(1).dblTeachPointX = typResistorInfoArray(rno).ArrCut(1).dblTeachPointX + deltaX(1)
                    '            typResistorInfoArray(rno).ArrCut(1).dblTeachPointY = typResistorInfoArray(rno).ArrCut(1).dblTeachPointY + deltaY(1)
                    '        End If
                    '    Next
                    'Next
                End If
                'V6.1.4.9①↑
            Next                                                        'V6.1.4.9① iCircuitCnt
            Exit Sub

        Catch ex As Exception
            strMsg = "i-TKY.SetR1Data2AllResistor() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub
#End Region

#Region "外部カメラティーチング用パラメータを設定する"
    '''=========================================================================
    ''' <summary>外部カメラティーチング用パラメータを設定する</summary>
    ''' <param name="iAppMode">(INP)アプリモード(giAppMode参照)
    '''                            　APP_MODE_TEACH          = ティーチング
    '''                            　APP_MODEi_EXCAM_R1TEACH = 外部カメラR1ティーチング
    '''                            　APP_MODE_EXCAM_TEACH    = 外部カメラティーチング</param> 
    ''' <param name="pltInfo"> (INP)プレートデータ</param>
    ''' <param name="stPLT">   (INP)プレート情報</param>
    ''' <param name="stSTP">　 (INP)ステップ情報</param>
    ''' <param name="stGRP">　 (INP)グループ情報</param>
    ''' <param name="stTY2">　 (INP)ＴＹ２情報</param>
    '''=========================================================================
    Private Sub SetExCamTeachParam(ByVal iAppMode As Integer, ByRef pltInfo As PlateInfo, ByRef stPLT As TEACH_PLATE_INFO, ByRef stSTP() As TEACH_STEP_INFO,
                                   ByRef stGRP() As TEACH_GROP_INFO, ByRef stTY2() As TEACH_TY2_INFO, Optional ByVal iTkyKnd As Short = 1)  'V6.1.4.9① アプリケーション種別iTkyKnd追加

        Dim IDX As Integer
        Dim strMSG As String                                                    ' メッセージ編集域

        Try
            With pltInfo
                stPLT.iTkyKnd = iTkyKnd                                         'アプリケーション種別 V6.1.4.9①
                ' プレート情報
                stPLT.intResistDir = .intResistDir                              ' 抵抗並び方向
                'V6.1.4.9①                stPLT.intResistCntInGroup = .intResistCntInBlock                ' ブロック内抵抗数
                stPLT.intResistCntInGroup = .intResistCntInGroup                '  1グループ内(1サーキット内)抵抗数
                stPLT.intBlockCntXDir = .intBlockCntXDir                        ' ブロック数Ｘ
                stPLT.intBlockCntYDir = .intBlockCntYDir                        ' ブロック数Ｙ
                stPLT.intGroupCntInBlockXBp = .intGroupCntInBlockXBp            ' ブロック内BPグループ数(X方向）
                stPLT.intGroupCntInBlockYStage = .intGroupCntInBlockYStage      ' ブロック内Stageグループ数(Y方向）
                stPLT.dblBlockSizeXDir = .dblBlockSizeXDir                      ' ブロックサイズＸ
                stPLT.dblBlockSizeYDir = .dblBlockSizeYDir                      ' ブロックサイズＹ
                stPLT.dblTableOffsetXDir = .dblTableOffsetXDir                  ' テーブル位置オフセットX
                stPLT.dblTableOffsetYDir = .dblTableOffsetYDir                  ' テーブル位置オフセットY
                stPLT.dblBpOffSetXDir = .dblBpOffSetXDir                        ' ＢＰオフセットX
                stPLT.dblBpOffSetYDir = .dblBpOffSetYDir                        ' ＢＰオフセットY
                stPLT.dblChipSizeXDir = .dblChipSizeXDir                        ' チップサイズX
                stPLT.dblChipSizeYDir = .dblChipSizeYDir                        ' チップサイズY
                stPLT.dblBpGrpItv = .dblBpGrpItv                                ' BPグループ間隔
                stPLT.dblblStgGrpItv = .dblStgGrpItvY                           ' Stageグループ間隔
                stPLT.dblRev1 = 0.0#                                            ' 予備
                stPLT.dblRev2 = 0.0#                                            ' 予備
                stPLT.CorrectTrimPosX = gfCorrectPosX                           ' トリムポジション補正値X
                stPLT.CorrectTrimPosX = gfCorrectPosY                           ' トリムポジション補正値Y

                ' ステップ情報
                For IDX = 1 To MaxStep
                    stSTP(IDX).intSP1 = typStepInfoArray(IDX).intSP1            ' ステップ番号
                    stSTP(IDX).intSP2 = typStepInfoArray(IDX).intSP2            ' ブロック数
                    stSTP(IDX).DblSP3 = typStepInfoArray(IDX).dblSP3            ' ステップ間インターバル
                Next IDX

                ' グループ情報
                For IDX = 1 To MaxGrp
                    stGRP(IDX).intGP1 = typGrpInfoArray(IDX).intGP1             ' グループ番号
                    stGRP(IDX).intGP2 = typGrpInfoArray(IDX).intGP2             ' 抵抗数
                    stGRP(IDX).dblGP3 = typGrpInfoArray(IDX).dblGP3             ' グループ間インターバル
                    stGRP(IDX).dblStgPosX = typGrpInfoArray(IDX).dblStgPosX     ' ステージXポジション
                    stGRP(IDX).dblStgPosY = typGrpInfoArray(IDX).dblStgPosY     ' ステージYポジション
                Next IDX

                ' ＴＹ２情報
                For IDX = 1 To MaxTy2
                    stTY2(IDX).intTy21 = typTy2InfoArray(IDX).intTy21           ' ブロック番号
                    stTY2(IDX).dblTy22 = typTy2InfoArray(IDX).dblTy22           ' 各ブロック間のステップ距離
                Next IDX
            End With

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetExCamTeachParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "カットトレースのためのカット方向を設定する"
    '''=========================================================================
    '''<summary>カットトレースのためのカット方向を設定する</summary>
    '''<param name="rn">   (INP)抵抗番号</param>
    '''<param name="cn">   (INP)カット番号</param>
    '''<param name="dirL1">(I/O)カット方向1</param>
    '''<param name="dirL2">(I/O)カット方向2</param>
    '''<remarks>・STｶｯﾄ/IDXｶｯﾄ時(dirL2は返さない)
    '''           入力        = カット角度(0°, 90°, 180°, 270°)
    '''           出力(dirL1) = カット方向(3:+X(0°), 2:+Y(90°), 1:-X(180°), 4:-Y(270°))
    '''         ・L ｶｯﾄ/ HOOK ｶｯﾄ時(dirL2は返さない)
    '''           入力        = カット角度(0°, 90°, 180°, 270°)
    '''                       = Lﾀｰﾝ方向(1:CW, 2:CCW) 
    '''           出力(dirL1) = カット方向 3:+X+Y(→↑), 4:-Y+X(↓→), 1:-X-Y(↓←) ,2:+Y-X(←↑),
    '''                                    7:+X-Y(→↓), 8:-Y-X(←↓), 5:-X+Y(↑←) ,6:+Y+X(↑→))
    '''         ・スキャンカット時
    '''           入力        = カット角度(0°, 90°, 180°, 270°)
    '''                       = ｽﾃｯﾌﾟ方向(0:0°, 1:90°, 2:180°, 3:270) 
    '''           出力(dirL1) = カット方向(1:-X, 2:+X, 3:-Y, 4:+Y)
    '''           出力(dirL2) = ｽﾃｯﾌﾟ方向 (1:+X, 2:-X, 3:+Y, 4:-Y)
    ''' </remarks>
    '''=========================================================================
    Private Sub Cnv_Cut_Dir(ByRef rn As Short, ByRef cn As Short, ByRef dirL1 As Short, ByRef dirL2 As Short)

        Dim strMSG As String                                ' メッセージ編集域

        Try
            ' STｶｯﾄ/IDXｶｯﾄ時
            ' 文字マーキング時
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_ST) Or
                (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_M) Or
                (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_ES) Or
                (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_ES2) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_IX) Then
                Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
                    Case 0
                        dirL1 = 3                                   ' 0°(+X→) 
                    Case 90
                        dirL1 = 2                                   ' 90°(+Y↑)
                    Case 180
                        dirL1 = 1                                   ' 180°(-X←) 
                    Case Else
                        dirL1 = 4                                   ' 270°(+Y↓)
                End Select
            End If

            '' 文字マーキング時
            'If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_M) Then
            '    Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
            '        Case 0
            '            dirL1 =                                    ' 0°(+X→) 
            '        Case 90
            '            dirL1 = 4                                   ' 90°(+Y↑)
            '        Case 180
            '            dirL1 = 1                                   ' 180°(-X←) 
            '        Case Else
            '            dirL1 = 3                                   ' 270°(+Y↓)
            '    End Select
            'End If

            ' Lｶｯﾄ/HOOKｶｯﾄ/Uｶｯﾄ時
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_L) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_HK) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_U) Or
               (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_Ut) Then ' V1.22.0.0①

                Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
                    Case 0                                          ' カット方向(1:+X(0°)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 7                               ' +X-Y(→↓)
                        Else
                            dirL1 = 3                               ' +X+Y(→↑)
                        End If
                    Case 90                                         ' カット方向(2:+Y(90°)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 6                               ' +Y+X(↑→))
                        Else
                            dirL1 = 2                               ' +Y-X(↑←)
                        End If
                    Case 180                                        ' カット方向(3:-X(180°)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 5                               ' -X+Y(←↑)
                        Else
                            dirL1 = 1                               ' -X-Y(←↓) 
                        End If
                    Case Else                                       ' カット方向(4:-Y(270°)) ?
                        If (typResistorInfoArray(rn).ArrCut(cn).intLTurnDir = 1) Then
                            dirL1 = 8                               ' -Y-X(↓←)
                        Else
                            dirL1 = 4                               ' -Y+X(↓→)
                        End If
                End Select
            End If

            ' スキャンカット時
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_SC) Then
                ' カット方向 
                Select Case (typResistorInfoArray(rn).ArrCut(cn).intCutAngle)
                    Case 0                                          ' +X
                        dirL1 = 2                                   ' 0°(+X→) 
                    Case 90                                         ' +Y
                        dirL1 = 4                                   ' 90°(+Y↑)
                    Case 180                                        ' -X
                        dirL1 = 1                                   ' 180°(-X←) 
                    Case Else                                       ' -Y
                        dirL1 = 3                                   ' 270°(-Y↓)

                End Select

                ' ステップ方向 
                Select Case (typResistorInfoArray(rn).ArrCut(cn).intStepDir)
                    Case 0                                          ' +X
                        dirL2 = 1                                   '   0°(+X→) 
                    Case 1                                          ' +Y
                        dirL2 = 3                                   '  90°(+Y↑)
                    Case 2                                          ' -X
                        dirL2 = 2                                   ' 180°(-X←) 
                    Case Else                                       ' -Y
                        dirL2 = 4                                   ' 270°(-Y↓)
                End Select
            End If

            '----- V6.1.1.0⑥↓ -----
            ' インデックスＷカット時はステップ方向(0:0°, 1:90°, 2:180°, 3:270)を返す
            If (typResistorInfoArray(rn).ArrCut(cn).strCutType = CNS_CUTP_IX) And (typResistorInfoArray(rn).ArrCut(cn).intCutCnt > 0) Then
                ' 基準コーナー
                Select Case (gSysPrm.stDEV.giBpDirXy)
                    Case 0 ' 右上(x←, y↓)
                        '    ' カット方向 = 0°と180°の場合
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 3                                               ' ｽﾃｯﾌﾟ方向(270°) 
                            Else
                                dirL2 = 1                                               ' ｽﾃｯﾌﾟ方向(90°) 
                            End If
                            ' カット方向 = 90°と270°の場合
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 2                                               ' ｽﾃｯﾌﾟ方向(180°) 
                            Else
                                dirL2 = 0                                               ' ｽﾃｯﾌﾟ方向(0°) 
                            End If
                        End If

                    Case 1 ' 左上( x→, y↓)
                        '    ' カット方向 = 0°と180°の場合
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 3                                               ' ｽﾃｯﾌﾟ方向(270°) 
                            Else
                                dirL2 = 1                                               ' ｽﾃｯﾌﾟ方向(90°) 
                            End If
                            ' カット方向 = 90°と270°の場合
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 0                                               ' ｽﾃｯﾌﾟ方向(0°) 
                            Else
                                dirL2 = 2                                               ' ｽﾃｯﾌﾟ方向(180°) 
                            End If
                        End If

                    Case 2 ' 右下(x←, y↑)
                        '    ' カット方向 = 0°と180°の場合
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 1                                               ' ｽﾃｯﾌﾟ方向(90°) 
                            Else
                                dirL2 = 3                                               ' ｽﾃｯﾌﾟ方向(270°) 
                            End If
                            ' カット方向 = 90°と270°の場合
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 2                                               ' ｽﾃｯﾌﾟ方向(180°) 
                            Else
                                dirL2 = 0                                               ' ｽﾃｯﾌﾟ方向(0°) 
                            End If
                        End If

                    Case 3 ' 左下(x→, y↑)
                        '    ' カット方向 = 0°と180°の場合
                        If (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 0) Or (typResistorInfoArray(rn).ArrCut(cn).intCutAngle = 180) Then
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 1                                               ' ｽﾃｯﾌﾟ方向(90°) 
                            Else
                                dirL2 = 3                                               ' ｽﾃｯﾌﾟ方向(270°) 
                            End If
                            ' カット方向 = 90°と270°の場合
                        Else
                            If (typResistorInfoArray(rn).ArrCut(cn).dblPitch > 0) Then  ' ステップは+方向 ?
                                dirL2 = 2                                               ' ｽﾃｯﾌﾟ方向(180°) 
                            Else
                                dirL2 = 0                                               ' ｽﾃｯﾌﾟ方向(0°) 
                            End If
                        End If
                End Select
            End If
            '----- V6.1.1.0⑥↑ -----

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Cnv_Cut_Dir() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "各カットトレースのためセットアップ処理"
    '''=========================================================================
    '''<summary>各カットトレースのためセットアップ処理</summary>
    '''<param name="rn">         (INP)抵抗番号</param>
    '''<param name="cn">         (INP)カット番号</param>
    '''<param name="dirL1">      (INP)カット方向1</param>
    '''<param name="dirL2">      (INP)カット方向2</param>
    '''<param name="RnCnTbl">    (INP)抵抗番号,カット番号テーブル</param> 
    '''<param name="StartPosTbl">(INP)カット開始位置テーブル</param> 
    '''<param name="idx">        (INP)テーブルインデックス</param>   
    '''=========================================================================
    Private Sub Sub_Cut_Setup(ByRef rn As Short, ByRef cn As Short, ByRef dirL1 As Short, ByRef dirL2 As Short,
                              ByRef RnCnTbl(,) As Short, ByRef StartPosTbl(,) As Double, ByRef idx As Short)

        Dim strMSG As String                                ' メッセージ編集域
        Dim iWK As Short

        Try
            With typResistorInfoArray(rn).ArrCut(cn)
                ' カット形状毎のセットアップ処理を行う
                Select Case (typResistorInfoArray(rn).ArrCut(cn).strCutType)
                    '   ' STカット(通常/リターン/リトレース) ※未使用(斜めSTカットを使用する)
                    Case CNS_CUTP_ST, CNS_CUTP_STr, CNS_CUTP_STt
                        ' ストレートカット(カット方向 1:-X←, 2:+Y↑, 3:+X→ ,4:-Y↓)
                        ' SetupCutST(抵抗番号, ｶｯﾄ番号, ｶｯﾄ方向, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1)
                        Call Teaching1.SetupCutST(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength)

                        ' 斜めSTカット(通常/リターン/リトレース) 
                    Case CNS_CUTP_NST, CNS_CUTP_NSTr, CNS_CUTP_NSTt, CNS_CUTP_ES, CNS_CUTP_ES2         'V1.14.0.0①
                        ' SetupCutST(抵抗番号, ｶｯﾄ番号, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, 角度)
                        Call Teaching1.SetupCutSST(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intCutAngle, typResistorInfoArray(rn).ArrCut(cn).strCutType)    'V3.0.0.0③ 最後の引数にカット種別を追加

                        'Ｌカット(通常/リターン/リトレース) ※未使用(斜めLカットを使用する)
                    Case CNS_CUTP_L, CNS_CUTP_Lr, CNS_CUTP_Lt
                        'Ｌカット(カット方向 1:-X-Y, 2:+Y-X, 3:+X+Y ,4:-Y+X, 5:-X+Y, 6:+Y+X, 7:+X-Y ,8:-Y-X)
                        ' SetupCutL(抵抗番号, ｶｯﾄ番号, ｶｯﾄ方向, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, R1, ﾀｰﾝﾎﾟｲﾝﾄ, ｶｯﾄ長2)
                        Call Teaching1.SetupCutL(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblLTurnPoint, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL)

                        ' 斜めＬカット(通常/リターン/リトレース) 
                    Case CNS_CUTP_NL, CNS_CUTP_NLr, CNS_CUTP_NLt
                        ' SetupCutSL(抵抗番号, ｶｯﾄ番号, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, R1(未使用), ｶｯﾄ長2, 角度, ﾀｰﾝ方向)
                        Call Teaching1.SetupCutSL(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, typResistorInfoArray(rn).ArrCut(cn).intCutAngle, typResistorInfoArray(rn).ArrCut(cn).intLTurnDir, typResistorInfoArray(rn).ArrCut(cn).strCutType)    'V3.0.0.0③ 最後の引数にカット種別を追加)

                    Case CNS_CUTP_HK    ' フックカット(カット方向 1:-X-Y, 2:+Y-X, 3:+X+Y ,4:-Y+X, 5:-X+Y, 6:+Y+X, 7:+X-Y ,8:-Y-X)
                        ' SetupCutHK(抵抗番号,ｶｯﾄ番号,ｶｯﾄ方向,ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y,ｶｯﾄ長1,R1半径(未使用), ﾀｰﾝﾎﾟｲﾝﾄ(未使用), ｶｯﾄ長2, r2(-1固定), ﾌｯｸｶｯﾄ移動量)
                        '' ''Call Teaching1.SetupCutHK(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), _
                        '' ''                StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblR1, 100.0#, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblR2, _
                        '' ''                typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthHook)
                        Call Teaching1.SetupCutHK(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx),
                                        StartPosTbl(2, idx), .dblMaxCutLength, .dblR1, 100.0#,
                                        .dblMaxCutLengthL, .dblR2, .dblMaxCutLengthHook)

                    Case CNS_CUTP_IX    ' インデックスカット(カット方向 1:-X←, 2:+Y↑, 3:+X→ ,4:-Y↓)
                        '----- V6.1.1.0⑥↓ -----
                        '  インデックスＷカット対応
                        '' SetupCutIX(抵抗番号, ｶｯﾄ番号, ｶｯﾄ方向, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, IDX回数, 測定ﾓｰﾄﾞ(未使用))
                        'Call Teaching1.SetupCutIX(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intIndexCnt, typResistorInfoArray(rn).ArrCut(cn).intMeasMode)

                        If (.intCutCnt > 0) Then                                           ' シフト回数(1～n)
                            'SetupCutIX(抵抗番号, ｶｯﾄ番号, ｶｯﾄ方向, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, IDX回数, 測定ﾓｰﾄﾞ(シフト回数), ｽﾃｯﾌﾟ方向, 本数)
                            Call Teaching1.SetupCutIX(RnCnTbl(idx, 0), RnCnTbl(idx, 1), .intCutAngle, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intIndexCnt, .intCutCnt, dirL2, System.Math.Abs(.dblPitch))
                        Else
                            'SetupCutIX(抵抗番号, ｶｯﾄ番号, ｶｯﾄ方向, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, IDX回数, 測定ﾓｰﾄﾞ(未使用), ｽﾃｯﾌﾟ方向, 本数)
                            Call Teaching1.SetupCutIX(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).intIndexCnt, .intCutCnt, 0, 0.0)
                        End If
                        '----- V6.1.1.0⑥↑ -----

                    Case CNS_CUTP_SC    ' スキャンカット(ｶｯﾄ方向 1:-X, 2:+X, 3:-Y, 4:+Y)/ｽﾃｯﾌﾟ方向(1:+X, 2:-X, 3:+Y, 4:-Y)
                        ' SetupCutSC(抵抗番号, ｶｯﾄ番号, ｶｯﾄ方向, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, ﾋﾟｯﾁ, ｽﾃｯﾌﾟ方向, 本数)
                        Call Teaching1.SetupCutSC(RnCnTbl(idx, 0), RnCnTbl(idx, 1), .intCutAngle,
                                                StartPosTbl(1, idx), StartPosTbl(2, idx), .dblMaxCutLength,
                                                .dblPitch, .intStepDir, .intCutCnt)
                        'Call Teaching1.SetupCutSC(RnCnTbl(idx, 0), RnCnTbl(idx, 1), typResistorInfoArray(rn).ArrCut(cn).intCutAngle, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).dblPitch, typResistorInfoArray(rn).ArrCut(cn).intStepDir, typResistorInfoArray(rn).ArrCut(cn).intCutCnt)
                        'Call Teaching1.SetupCutSC(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, typResistorInfoArray(rn).ArrCut(cn).dblPitch, dirL2, typResistorInfoArray(rn).ArrCut(cn).intCutCnt)

                    Case CNS_CUTP_U, CNS_CUTP_Ut     ' Uカット(カット方向 1:-X-Y, 2:+X+Y, 3:-Y+X ,4:+Y-X) V1.22.0.0①
                        ' SetupCutU(抵抗番号, ｶｯﾄ番号, ｶｯﾄ方向, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, ｶｯﾄ長1, R1半径, ｶｯﾄ長2, Lﾀｰﾝ後移動方向(未使用))
                        Call Teaching1.SetupCutU(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, 0)
                        'Call Teaching1.SetupCutU(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLength, 0.0#, typResistorInfoArray(rn).ArrCut(cn).dblMaxCutLengthL, 0)

                    Case CNS_CUTP_NOP   ' Z(NO CUT)
                        ' SetupCutZ(抵抗番号, ｶｯﾄ番号, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y)
                        Call Teaching1.SetupCutZ(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx))

                    Case CNS_CUTP_M     ' 文字マーキング 
                        ' SetupCutM(抵抗番号, ｶｯﾄ番号, 方向方向(1:-X, 2:+X, 3:-Y, 4:+Y), ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y, 倍率, 文字列長)
                        iWK = typResistorInfoArray(rn).ArrCut(cn).dblZoom
                        '----- V6.0.3.0_51↓ -----
                        'Call Teaching1.SetupCutM(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), iWK, Len(typResistorInfoArray(rn).ArrCut(cn).strChar))
                        'Call Teaching1.SetupCutMStr(RnCnTbl(idx, 0), RnCnTbl(idx, 1), dirL1, StartPosTbl(1, idx), StartPosTbl(2, idx), iWK, Len(typResistorInfoArray(rn).ArrCut(cn).strChar), typResistorInfoArray(rn).ArrCut(cn).strChar)
                        Call Teaching1.SetupCutMStr(RnCnTbl(idx, 0), RnCnTbl(idx, 1), typResistorInfoArray(rn).ArrCut(cn).intCutAngle, StartPosTbl(1, idx), StartPosTbl(2, idx), iWK, Len(typResistorInfoArray(rn).ArrCut(cn).strChar), typResistorInfoArray(rn).ArrCut(cn).strChar)
                        '----- V6.0.3.0_51↓ -----

                    Case CNS_CUTP_C     ' Cカット(円弧カット)
                        ' SetupCutCir(抵抗番号, ｶｯﾄ番号, ｽﾀｰﾄ位置X, ｽﾀｰﾄ位置Y,円弧部の半径,円弧の角度, 始めの移動角度)
                        ' Call Teaching1.SetupCutCir(RnCnTbl(t, 0), RnCnTbl(t, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), 1, 45, -270)
                        Call Teaching1.SetupCutCir(RnCnTbl(idx, 0), RnCnTbl(idx, 1), StartPosTbl(1, idx), StartPosTbl(2, idx), typResistorInfoArray(rn).ArrCut(cn).dblR1, typResistorInfoArray(rn).ArrCut(cn).dblR2, typResistorInfoArray(rn).ArrCut(cn).intCutAngle)

                    Case Else
                        Call System1.TrmMsgBox(gSysPrm, "Cut Type Error Type = " & typResistorInfoArray(rn).ArrCut(cn).strCutType, MsgBoxStyle.OkOnly, My.Application.Info.Title)
                End Select
            End With
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Sub_Cut_Setup() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "θ補正とXYﾃｰﾌﾞﾙﾄﾘﾑ位置移動とZ待機位置移動処理"
    '''=========================================================================
    ''' <summary>θ補正とXYﾃｰﾌﾞﾙﾄﾘﾑ位置移動とZ待機位置移動処理</summary>
    ''' <param name="pltInfo">(INP)プレートデータ</param>
    ''' <param name="TblOfsX">(INP)テーブルオフセットX</param>
    ''' <param name="TblOfsY">(INP)テーブルオフセットY</param>
    ''' <returns>cFRS_NORMAL   = 正常
    '''         cFRS_ERR_RST  = Cancel(RESETｷｰ)
    '''         cFRS_ERR_PTN  = パターン認識エラー
    '''         上記以外      = 非常停止等その他エラー
    ''' </returns>
    '''=========================================================================
    Private Function Move_Trimposition(ByVal pltInfo As PlateInfo, ByVal TblOfsX As Double, ByVal TblOfsY As Double) As Short

        Dim strMSG As String = ""
        Dim r As Short
        Dim StgOfsX As Double = 0.0                                     ' XYテーブルオフセットX 'V1.13.0.0③
        Dim StgOfsY As Double = 0.0                                     ' XYテーブルオフセットY 'V1.13.0.0③

        Try
            With pltInfo
                ' 初期処理
                Move_Trimposition = cFRS_NORMAL                         ' Return値 = 正常
                'V4.10.0.0③↓
                If gbCorrectDone Then                                   ' 一括ティーチング時は、一度補正を実施した後は行わない。
                    GoTo STP_START
                End If
                'V4.10.0.0③↑
                'V1.19.0.0-27　↓
                ' θ補正手動で、１回のみの場合はフラグをチェックして実行済みなら何もしない
                If (.intReviseMode = 1) And (.intManualReviseType = 1) Then
                    If (gManualThetaCorrection = True) Then
                        gfCorrectPosX = 0.0                             ' 補正値初期化 
                        gfCorrectPosY = 0.0
                    End If
                Else
                    gfCorrectPosX = 0.0                                 ' 補正値初期化 
                    gfCorrectPosY = 0.0
                End If
                'V1.19.0.0-27　↑
                '----- V1.15.0.0③↓ -----
                ' Zを待機位置へ移動する
                r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Move_Trimposition = r                               ' Return値設定
                    Exit Function
                End If
                '----- V1.15.0.0③↑ -----

                '--------------------------------------------------------------------------
                '  θ補正処理
                '--------------------------------------------------------------------------
                ' θ無しならθ補正しないでXYﾃｰﾌﾞﾙをﾄﾘﾑ位置に移動
                If (gSysPrm.stDEV.giTheta = 0) Then GoTo STP_START
                ' θ有りで「補正モード=1(手動)」で補正方法=0(補正なし)の場合はθ回転を行う
                If ((.intReviseMode = 1) And (.intManualReviseType = 0)) Then
                    Call ROUND4(.dblRotateTheta)                        'θ載物台の絶対角度指定回転 '###037
                    GoTo STP_START
                End If
                'V1.19.0.0-27　↓
                ' θ補正手動で、１回のみの場合はフラグをチェックして実行済みなら何もしない
                If (.intReviseMode = 1) And (.intManualReviseType = 1) Then
                    If (gManualThetaCorrection = True) Then
                    Else
                        GoTo STP_START
                    End If
                End If
                'V1.19.0.0-27　↑

                'θ補正処理
                'r = SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG)
                r = SUb_ThetaCorrection(pltInfo, gfCorrectPosX, gfCorrectPosY, strMSG, True, False) 'V5.0.0.9⑩
                If (r <> cFRS_NORMAL) Then                              ' ERROR ?
                    If (r <= cFRS_VIDEO_PTN) Then                       ' パターン認識エラー ?
                        Call Beep()                                     ' Beep音
                        If (strMSG = "") Then                           '###038
                            ' ログ画面に文字列を表示する(マッチングエラー, パターン番号エラー)
                            strMSG = MSG_LOADER_07 + " (" + gbRotCorrectCancel.ToString + ")"
                        Else
                            'If (pltInfo.intRecogDispMode = 0) Then      ' 結果表示しない ?
                            If (pltInfo.intRecogDispMode = 0) AndAlso
                                (0 = pltInfo.intRecogDispModeRgh) Then  ' 結果表示しない ? 'V5.0.0.9⑤
                                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                                '    ' パターンマッチングエラー(閾値)
                                '    strMSG = MSG_LOADER_07 + "(閾値)"
                                'Else
                                '    strMSG = MSG_LOADER_07 + "(Thresh)"
                                'End If
                                strMSG = MSG_LOADER_07 & Form1_008
                            End If
                        End If
                        Call Z_PRINT(strMSG & vbCrLf)
                        Move_Trimposition = cFRS_ERR_PTN                ' Return値 = パターン認識エラー
                    Else
                        Move_Trimposition = r
                    End If
                    Exit Function
                End If

                'V1.19.0.0-27　↓
                gManualThetaCorrection = False
                'V1.19.0.0-27　↑

                ' θ補正情報表示
                'If (pltInfo.intRecogDispMode = 1) Then                  ' 結果表示する ? '###038
                If (pltInfo.intRecogDispMode = 1) OrElse
                    (0 <> pltInfo.intRecogDispModeRgh) Then             ' 結果表示する ?  'V5.0.0.9⑤
                    Call Z_PRINT(strMSG)
                End If

                'V4.10.0.0③↓
                If gbIntegratedMode Then                                ' 現在一括ティーチングモードの時 True
                    gbCorrectDone = True                                ' オペレータティーチング簡素化 一度ＸＹΘ補正を行ったら後は実施しない為に使用
                End If
                'V4.10.0.0③↑
STP_START:
                '--------------------------------------------------------------------------
                '  XYテーブルをトリム位置へ移動する
                '--------------------------------------------------------------------------
                '----- V1.13.0.0③↓ -----
                ' オートプローブ登録/実行ならθ補正のみ実行し補正値(gfCorrectPosX, gfCorrectPosY)を求める
                If (giAppMode = APP_MODE_APROBEREC) Or (giAppMode = APP_MODE_APROBEEXE) Then
                    ' XYテーブルを原点へ移動する
                    r = System1.EX_SMOVE2(gSysPrm, 0.0, 0.0)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ?
                        Move_Trimposition = r                           ' Return値設定
                        Exit Function
                    End If
                    Exit Function
                End If
                '----- V1.13.0.0③↑ -----
                '----- V3.0.0.0⑥(V1.22.0.0⑦)↓ -----
                ''----- V1.14.0.0⑦↓ -----
                ' キャリブレーション時はブロックサイズを80角とする
                If (giAppMode = APP_MODE_CARIB_REC) Or (giAppMode = APP_MODE_CARIB) Then
                    ' ブロックサイズ設定
                    If (typPlateInfo.intResistDir = 0) Then             ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                        '----- V1.24.0.0①↓ -----
                        If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                            r = System1.EX_BSIZE(gSysPrm, 60.0, 20.0)
                            'V6.0.4.0①↓
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                            r = System1.EX_BSIZE(gSysPrm, 60.0, 30.0)
                            'V6.0.4.0①↑
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then    'V4.4.0.0①
                            r = System1.EX_BSIZE(gSysPrm, 90.0, 0.0)
                        Else
                            r = System1.EX_BSIZE(gSysPrm, 80.0, 0.0)
                        End If
                        'r = System1.EX_BSIZE(gSysPrm, 80.0, 0.0)
                        '----- V1.24.0.0①↑ -----
                    Else
                        '----- V1.24.0.0①↓ -----
                        If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                            r = System1.EX_BSIZE(gSysPrm, 20.0, 60.0)
                            'V6.0.4.0①↓
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_6030) Then
                            r = System1.EX_BSIZE(gSysPrm, 30.0, 60.0)
                            'V6.0.4.0①↑
                        ElseIf (gSysPrm.stDEV.giBpSize = BPSIZE_90) Then    'V4.4.0.0①
                            r = System1.EX_BSIZE(gSysPrm, 0.0, 90.0)
                        Else
                            r = System1.EX_BSIZE(gSysPrm, 0.0, 80.0)
                        End If
                        'r = System1.EX_BSIZE(gSysPrm, 0.0, 80.0)
                        '----- V1.24.0.0①↑ -----
                    End If

                    '' キャリブレーション時はブロックサイズを80角とする
                    'If (giAppMode = APP_MODE_CARIB_REC) Or (giAppMode = APP_MODE_CARIB) Then
                    '    ' ブロックサイズ設定
                    '    '----- V1.20.0.1②↓ -----
                    '    ' ブロックサイズはそのままとする
                    '    'r = System1.EX_BSIZE(gSysPrm, 80.0, 80.0)                          
                    '    r = System1.EX_BSIZE(gSysPrm, .dblBlockSizeXDir, .dblBlockSizeYDir)
                    '    '----- V1.20.0.1②↑ -----
                    '----- V3.0.0.0⑥(V1.22.0.0⑦)↑ -----
                    If (r <> cFRS_NORMAL) Then                              ' エラー ?
                        Move_Trimposition = r                               ' Return値設定
                        Exit Function
                    End If
                    ' BPオフセット設定(※ブロックサイズを設定するとBPオフセットはINtime側で初期化される)
                    r = System1.EX_BPOFF(gSysPrm, 0.0, 0.0)
                    If (r <> cFRS_NORMAL) Then                              ' エラー ?
                        Move_Trimposition = r                               ' Return値設定
                        Exit Function
                    End If
                Else
                    ' ブロックサイズ設定
                    r = System1.EX_BSIZE(gSysPrm, .dblBlockSizeXDir, .dblBlockSizeYDir)
                    If (r <> cFRS_NORMAL) Then                              ' エラー ?
                        Move_Trimposition = r                               ' Return値設定
                        Exit Function
                    End If
                    ' BPオフセット設定(※ブロックサイズを設定するとBPオフセットはINtime側で初期化される)
                    r = System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                    If (r <> cFRS_NORMAL) Then                              ' エラー ?
                        Move_Trimposition = r                               ' Return値設定
                        Exit Function
                    End If
                End If
                '' ブロックサイズ設定
                'r = System1.EX_BSIZE(gSysPrm, .dblBlockSizeXDir, .dblBlockSizeYDir)
                'If (r <> cFRS_NORMAL) Then                              ' エラー ?
                '    Move_Trimposition = r                               ' Return値設定
                '    Exit Function
                'End If
                '' BPオフセット設定(※ブロックサイズを設定するとBPオフセットはINtime側で初期化される)
                'r = System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then                              ' エラー ?
                '    Move_Trimposition = r                               ' Return値設定
                '    Exit Function
                'End If
                '----- V1.14.0.0⑦↑ -----

                'V6.0.0.0-28                ' ステージ移動前にビデオの更新処理を一旦停止
                'V6.0.0.0-28                Call Me.VideoLibrary1.VideoStop()

                '----- V1.13.0.0③↓ -----
                ' XYテーブルオフセットを設定する(ステージオフセット＋θ補正値+オフセット+オートプローブオフセット)
                '----- V1.14.0.0⑦↓ -----
                ' キャリブレーション時はステージオフセットを加味しない
                If (giAppMode = APP_MODE_CARIB_REC) Or (giAppMode = APP_MODE_CARIB) Then
                    '----- V1.22.0.0⑦↓ -----
                    ' ブロックサイズを0にしたので、オフセットに基準座標+オフセットを加算する
                    If (typPlateInfo.intResistDir = 0) Then             ' 抵抗(ﾁｯﾌﾟ)並び方向(0:X, 1:Y)
                        'V4.7.1.0①                        StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX
                        StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX + .dblCaribTableOffsetXDir  'V4.7.1.0①
                        StgOfsY = gfCorrectPosY + TblOfsY + gfStgOfsY + .dblCaribBaseCordnt1YDir + .dblCaribTableOffsetYDir
                    Else
                        StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX + .dblCaribBaseCordnt1XDir + .dblCaribTableOffsetXDir
                        StgOfsY = gfCorrectPosY + TblOfsY + gfStgOfsY
                    End If
                    'StgOfsX = gfCorrectPosX + TblOfsX + gfStgOfsX
                    'StgOfsY = gfCorrectPosY + TblOfsY + gfStgOfsY
                    '----- V1.22.0.0⑦↑ -----
                    '----- V1.24.0.0①↓ -----
                    ' ステージオフセットを加味
                    If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                        StgOfsX = StgOfsX + .dblTableOffsetXDir
                        StgOfsY = StgOfsY + .dblTableOffsetYDir
                    End If
                    '----- V1.24.0.0①↑ -----
                Else
                    StgOfsX = .dblTableOffsetXDir + gfCorrectPosX + TblOfsX + gfStgOfsX
                    StgOfsY = .dblTableOffsetYDir + gfCorrectPosY + TblOfsY + gfStgOfsY
                End If
                'StgOfsX = .dblTableOffsetXDir + gfCorrectPosX + TblOfsX + gfStgOfsX
                'StgOfsY = .dblTableOffsetYDir + gfCorrectPosY + TblOfsY + gfStgOfsY
                '----- V1.14.0.0⑦↑ -----
                '----- V1.13.0.0③↑ -----

                '----- V2.0.0.0⑨↓ -----
                If (giMachineKd = MACHINE_KD_RS) Then
                    '----- V4.0.0.0-40↓ -----
                    ' SL36S時でステージYの原点位置が上の場合、ブロックサイズの1/2は加算しない
                    If (giStageYOrg = STGY_ORG_UP) Then
                        StgOfsY = StgOfsY
                    Else
                        StgOfsY = StgOfsY
                    End If
                    'StgOfsY = StgOfsY + (typPlateInfo.dblBlockSizeYDir / 2)
                    '----- V4.0.0.0-40↑ -----
                End If
                '----- V2.0.0.0⑨↑ -----

                '----- V1.24.0.0①↓ -----
                ' 特注Fシータ時で外部カメラ位置へ移動する場合(キャリブレーションを除く)
                If (giAppMode = APP_MODE_EXCAM_R1TEACH) Or (giAppMode = APP_MODE_EXCAM_TEACH) Or
                   (giAppMode = APP_MODE_CUTREVISE_REC) Or (giAppMode = APP_MODE_CUTREVIDE) Then
                    If (gSysPrm.stDEV.giBpSize = BPSIZE_6060) Then
                        StgOfsX = StgOfsX - BSZ_6060_OFSX
                        StgOfsY = StgOfsY - BSZ_6060_OFSY
                    End If
                End If
                '----- V1.24.0.0①↑ -----

                ' XYテーブルをトリム位置へ移動(トリム位置＋ステージオフセット＋補正値+オフセット)
                'r = System1.EX_START(gSysPrm, .dblTableOffsetXDir + gfCorrectPosX + TblOfsX, .dblTableOffsetYDir + gfCorrectPosY + TblOfsY, 0)' V1.13.0.0③
                r = System1.EX_START(gSysPrm, StgOfsX, StgOfsY, 0)      ' V1.13.0.0③
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Move_Trimposition = r                               ' Return値設定
                    Exit Function
                End If

                'V6.0.0.0-28                ' ステージ移動後にビデオの更新処理を再実施
                'V6.0.0.0-28                Call Me.VideoLibrary1.VideoStart()
                VideoLibrary1.SetCrossLineObject(gparModules)   'V5.0.0.6⑫
                gparModules.DispCrossLine(0, 0)                 'V5.0.0.6⑫
                '--------------------------------------------------------------------------
                '   ZOff位置へ移動
                '--------------------------------------------------------------------------
                r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Move_Trimposition = r                               ' Return値設定
                    Exit Function
                End If
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Move_Trimposition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Move_Trimposition = cERR_TRAP                               ' 戻値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "θ補正のための画像登録(F9)ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>θ補正のための画像登録(F9)ﾎﾞﾀﾝ</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdPattern_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdPattern.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_RECOG, APP_MODE_RECOG, MSG_OPLOG_FUNC09, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdPattern_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "クランプレス載物台・ラフアライメント用画像登録処理"
    '''=========================================================================
    ''' <summary>クランプレス載物台・ラフアライメント用画像登録処理</summary>
    ''' <remarks>'V5.0.0.9④</remarks>
    '''=========================================================================
    Private Sub CmdRecogRough_Click(sender As Object, e As EventArgs) Handles CmdRecogRough.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_RECOG_ROUGH, APP_MODE_RECOG, MSG_OPLOG_FUNC09, "", True)   ' TODO: MSG_OPLOG_FUNC09から変更するか

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdRecogRough_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "θ補正のためのパターン登録初期設定処理"
    '''=========================================================================
    '''<summary>θ補正のためのパターン登録初期設定処理</summary>
    '''=========================================================================
    Private Function InitThetaCorrection(ByVal isRough As Boolean,
                                         Optional ByVal clampLessOffsetX As Double = 0.0, Optional ByVal clampLessOffsetY As Double = 0.0,
                                         Optional ByVal correctTrimPosX As Double = 0.0, Optional ByVal correctTrimPosY As Double = 0.0) As Integer   'V5.0.0.9③ 'V5.0.0.9⑤
        'Private Function InitThetaCorrection() As Integer

        Dim strMSG As String                                            ' メッセージ編集域

        Try
            ' Video.OCXのプロパティを設定する
            VideoLibrary1.pfTrim_x = gSysPrm.stDEV.gfTrimX              ' トリミングポジションX(mm)
            VideoLibrary1.pfTrim_y = gSysPrm.stDEV.gfTrimY              ' トリミングポジションY(mm)
            VideoLibrary1.pfBlock_x = typPlateInfo.dblBlockSizeXDir     ' Block Size X
            VideoLibrary1.pfBlock_y = typPlateInfo.dblBlockSizeYDir     ' Block Size Y
            VideoLibrary1.zwaitpos = typPlateInfo.dblZWaitOffset        ' Z PROBE OFF OFFSET
            'VideoLibrary1.ThetaRCenterX = gSysPrm.stDEV.gfRot_X1        ' 回転中心座標 X mm
            'V6.1.1.0⑰            VideoLibrary1.ThetaRCenterX = gSysPrm.stDEV.gfRot_X1 + correctTrimPosX + clampLessOffsetX   ' 回転中心座標 X mm  'V5.0.0.9③
            VideoLibrary1.ThetaRCenterX = gSysPrm.stDEV.gfRot_X1   ' 回転中心座標 X mm  'V5.0.0.9③
            'VideoLibrary1.ThetaRCenterY = gSysPrm.stDEV.gfRot_Y1        ' 回転中心座標 Y mm
            ''V6.1.1.0⑰            VideoLibrary1.ThetaRCenterY = gSysPrm.stDEV.gfRot_Y1 + correctTrimPosY + clampLessOffsetY   ' 回転中心座標 Y mm  'V5.0.0.9③
            VideoLibrary1.ThetaRCenterY = gSysPrm.stDEV.gfRot_Y1   ' 回転中心座標 Y mm  'V5.0.0.9③
            VideoLibrary1.PP18 = 0                                      ' Z待機位置
            VideoLibrary1.pfStgOffX = typPlateInfo.dblTableOffsetXDir   ' Trim Position Offset Y(mm) 'V1.13.0.0③
            VideoLibrary1.pfStgOffY = typPlateInfo.dblTableOffsetYDir   ' Trim Position Offset Y(mm) 'V1.13.0.0③

            If (True = isRough) Then
                ' クランプレス載物台・ラフアライメント画像登録        'V5.0.0.9③  ↓
                VideoLibrary1.PP30 = 0                                  ' 補正モード(0:自動,1:手動, 2:自動+微調)
                VideoLibrary1.PP31 = 2                                  ' 補正方法(0:なし,1:1回のみ,2:毎回）※PP30=0のときは無効
                VideoLibrary1.pp32_x = typPlateInfo.dblReviseCordnt1XDirRgh + correctTrimPosX + clampLessOffsetX ' パターン1座標x
                VideoLibrary1.pp32_y = typPlateInfo.dblReviseCordnt1YDirRgh + correctTrimPosY + clampLessOffsetY ' パターン1座標y
                VideoLibrary1.PP33X = typPlateInfo.dblReviseCordnt2XDirRgh + correctTrimPosX + clampLessOffsetX ' パターン2座標x
                VideoLibrary1.PP33Y = typPlateInfo.dblReviseCordnt2YDirRgh + correctTrimPosY + clampLessOffsetY ' パターン2座標y
                VideoLibrary1.pp34_x = typPlateInfo.dblReviseOffsetXDirRgh      ' 補正ポジションオフセットx
                VideoLibrary1.pp34_y = typPlateInfo.dblReviseOffsetYDirRgh      ' 補正ポジションオフセットy
                ' 内部カメラでおこなう
                VideoLibrary1.pp36_x = gSysPrm.stGRV.gfPixelX                   ' Xピクセル分解能 um
                VideoLibrary1.pp36_y = gSysPrm.stGRV.gfPixelY                   ' Yピクセル分解能 um
                VideoLibrary1.PP37_1 = typPlateInfo.intRevisePtnNo1Rgh          ' パターン1 テンプレート番号
                VideoLibrary1.PP37_2 = typPlateInfo.intRevisePtnNo2Rgh          ' パターン2 テンプレート番号
                VideoLibrary1.PP52 = typPlateInfo.intRevisePtnNo1GroupNoRgh     ' パターン1 グループ番号
                VideoLibrary1.PP52_1 = typPlateInfo.intRevisePtnNo2GroupNoRgh   ' パターン2 グループ番号
                VideoLibrary1.PP53 = typPlateInfo.dblRotateTheta                ' θ軸角度
            Else                                           'V5.0.0.9③  ↑
                ' θ補整
                VideoLibrary1.PP30 = typPlateInfo.intReviseMode         ' 補正モード(0:自動,1:手動, 2:自動+微調)
                VideoLibrary1.PP31 = typPlateInfo.intManualReviseType   ' 補正方法(0:なし,1:1回のみ,2:毎回）※PP30=0のときは無効
                ' 手動補正モードで補正あり ?
                If (typPlateInfo.intReviseMode = 1) Then
                    VideoLibrary1.PP31 = 2                                  ' 手動補正時の動作 = 毎回
                End If
                VideoLibrary1.pp32_x = typPlateInfo.dblReviseCordnt1XDir + correctTrimPosX + clampLessOffsetX   ' パターン1座標x  'V5.0.0.9⑤
                VideoLibrary1.pp32_y = typPlateInfo.dblReviseCordnt1YDir + correctTrimPosY + clampLessOffsetY   ' パターン1座標y  'V5.0.0.9⑤
                VideoLibrary1.PP33X = typPlateInfo.dblReviseCordnt2XDir + correctTrimPosX + clampLessOffsetX    ' パターン2座標x  'V5.0.0.9⑤
                VideoLibrary1.PP33Y = typPlateInfo.dblReviseCordnt2YDir + correctTrimPosY + clampLessOffsetY    ' パターン2座標y  'V5.0.0.9⑤
                VideoLibrary1.pp34_x = typPlateInfo.dblReviseOffsetXDir     ' 補正ポジションオフセットx 'V5.0.0.9⑤
                VideoLibrary1.pp34_y = typPlateInfo.dblReviseOffsetYDir     ' 補正ポジションオフセットy 'V5.0.0.9⑤
                If (gSysPrm.stDEV.giEXCAM = 1) Then                         ' 外部カメラ ?
                    'VideoLibrary1.pp34_x = 0.0#                             ' 手動で補正あり時 pp34_x,y分　###100
                    'VideoLibrary1.pp34_y = 0.0#                             ' ずれるので0にする            ###100
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfEXCAM_PixelX     ' Xピクセル分解能 um
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfEXCAM_PixelY     ' Yピクセル分解能 um
                Else
                    VideoLibrary1.pp36_x = gSysPrm.stGRV.gfPixelX           ' Xピクセル分解能 um
                    VideoLibrary1.pp36_y = gSysPrm.stGRV.gfPixelY           ' Yピクセル分解能 um
                End If
                VideoLibrary1.PP37_1 = typPlateInfo.intRevisePtnNo1         ' パターン1 テンプレート番号
                VideoLibrary1.PP37_2 = typPlateInfo.intRevisePtnNo2         ' パターン2 テンプレート番号
                VideoLibrary1.PP52 = typPlateInfo.intRevisePtnNo1GroupNo    ' パターン1 グループ番号
                ' 'V4.0.0.0-45
                VideoLibrary1.PP52_1 = typPlateInfo.intRevisePtnNo2GroupNo    ' パターン2 グループ番号
                VideoLibrary1.PP53 = typPlateInfo.dblRotateTheta            ' θ軸角度
            End If

            'VideoLibrary1.PP35 = 1                                     ' Debug用(1:on,0:off)
            VideoLibrary1.PP35 = 0                                      ' Debug用(1:on,0:off)
            VideoLibrary1.RNASTMPNUM = False
            VideoLibrary1.frmLeft = Me.Text4.Location.Y                 ' 表示位置(Left)
            VideoLibrary1.frmTop = Me.Text4.Location.X                  ' 表示位置(Top)

            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.InitThetaCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            InitThetaCorrection = cERR_TRAP                             ' 戻値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

    End Function
#End Region

#Region "θ補正実行サブルーチン"
    '''=========================================================================
    '''<summary>θ補正実行サブルーチン</summary>
    '''<param name="pltInfo">    (INP) プレートデータ</param>
    '''<param name="dblCorrectX">(OUT) XYテーブル補正値X</param>
    '''<param name="dblCorrectY">(OUT) XYテーブル補正値Y</param>
    '''<param name="strDAT">     (OUT) θ補正結果編集域</param>
    ''' <param name="doAlign">(INP)θ補正実行する・しない</param>
    ''' <param name="doRough">(INP)ラフアライメント実行する・しない</param>
    '''<returns>cFRS_NORMAL   = 正常
    '''         cFRS_ERR_RST  = Cancel(RESETｷｰ)
    '''         cFRS_ERR_PTN  = パターン認識エラー
    '''         上記以外      = 非常停止等その他エラー
    '''</returns>
    '''=========================================================================
    Public Function SUb_ThetaCorrection(ByVal pltInfo As PlateInfo, ByRef dblCorrectX As Double, ByRef dblCorrectY As Double,
                                        ByRef strDAT As String, ByVal doAlign As Boolean, ByVal doRough As Boolean) As Integer
        'Public Function SUb_ThetaCorrection(ByVal pltInfo As PlateInfo, ByRef dblCorrectX As Double, ByRef dblCorrectY As Double, _
        '                                     ByRef strDAT As String) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String                                            ' メッセージ編集域
        'Dim stResult As LaserFront.Trimmer.DllVideo.VideoLibrary.Theta_Cor_Info                          ' θ補正結果 ※｢Theta_Cor_Info｣はVideo.OCXで定義
        Dim results As New List(Of PatternRecog)                        ' θ補正結果 'V5.0.0.9⑤
        'Dim Thresh1 As Double
        'Dim Thresh2 As Double

        Try
            ' θ補正を実行する
            r = ThetaCorrection(gfCorrectPosX, gfCorrectPosY, results, doAlign, doRough)  'θ補正実行
            If (r <> cFRS_NORMAL) AndAlso (cFRS_ERR_PT2 <> r) Then      ' ERROR ?       ' 閾値ｴﾗｰではない？ 'V5.0.0.9⑤
                ' パターン検出できなかった場合
                'V6.1.4.16①↓
                If (r <= ((-1) * ERR_TIMEOUT_BASE)) Then                            ' INtime側でステージ敬エラーが発生
                    'If (r <= cFRS_VIDEO_PTN) Then                           ' パターン認識エラー ?
                ElseIf (r <= cFRS_VIDEO_PTN) Then                           ' パターン認識エラー ?
                    'V6.1.4.16①↑
                    r = cFRS_ERR_PTN                                    ' Return値 = パターン認識エラー
                End If
                Return (r)
            End If

#If True Then   'V5.0.0.9⑤
                If (0 < results.Count) Then

                Dim sb As New StringBuilder(256)

                With pltInfo
                    Const HL As Integer = 80

                    ' ラフアライメント実行時に結果表示する・しない
                    If (True = doRough) AndAlso (0 <> .intRecogDispModeRgh) Then

                        Dim line As String = New String("-"c, HL)
                        ' ラフアライメントの要素を取り出す
                        For Each ptr As PatternRecog In results.Where(Function(pr)
                                                                          Return pr.IsRough
                                                                      End Function)
                            sb.AppendLine(line)
                            r = GetRecogResult(0, 0.0, ptr, sb)

                            If (cFRS_NORMAL <> r) Then
                                Exit For
                            End If
                        Next

                        If (cFRS_NORMAL <> r) OrElse (False = doAlign) OrElse (0 = .intRecogDispMode) Then
                            ' ラフアライメントの結果のみ表示する場合
                            sb.AppendLine(line)
                        End If
                    End If

                    ' ラフアライメント正常（または未実行）時にθ補正結果表示する・しない
                    If (cFRS_NORMAL = r) AndAlso (True = doAlign) AndAlso (0 <> .intRecogDispMode) Then
                        Dim result As PatternRecog = results(results.Count - 1)
                        If (False = result.IsRough) Then
                            Dim line As String = New String("="c, HL)
                            sb.AppendLine(line)
                            r = GetRecogResult(.intReviseMode, .dblRotateTheta, result, sb)
                            sb.AppendLine(line)
                        End If
                        'V4.12.2.3②↓'V6.0.1.0⑲
                    Else
                        Dim result As PatternRecog = results(results.Count - 1)
                        If (False = result.IsRough) Then
                            If (False = result.IsMatch) Then
                                r = (cFRS_ERR_PT2)                    ' Return値 = パターン認識エラー(閾値エラー)
                            End If
                        End If
                    End If
                    'V4.12.2.3②↑ 'V6.0.1.0⑲
                End With

                Debug.Print(sb.ToString())
                If (cERR_TRAP <> r) Then
                    strDAT = sb.ToString()
                End If
            End If
#Else
            ' 閾値取得 '###038
            Thresh1 = gDllSysprmSysParam_definst.GetPtnMatchThresh(typPlateInfo.intRevisePtnNo1GroupNo, typPlateInfo.intRevisePtnNo1)
            Thresh2 = gDllSysprmSysParam_definst.GetPtnMatchThresh(typPlateInfo.intRevisePtnNo1GroupNo, typPlateInfo.intRevisePtnNo2)

            ' θ補正結果取得
            Call VideoLibrary1.GetThetaResult(stResult)
            With pltInfo
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    ' θ補正表示情報設定(日本語)
                '    strDAT = "θ角度 = " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "°" & vbCrLf
                '    If (.intReviseMode = 2) Then                        ' 自動+微調の場合
                '        strDAT = "θ角度 = " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "°"
                '        strDAT = strDAT & "+ " & .dblRotateTheta.ToString("0.0000").PadLeft(7) & "°" & vbCrLf
                '    End If
                '    strDAT = strDAT & "  トリム位置X,Y=" & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPosy.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " ずれ量X,Y    =" & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCory.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  補正位置1X,Y =" & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos1y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " ずれ量1X,Y   =" & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor1y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  補正位置2X,Y =" & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos2y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " ずれ量2X,Y   =" & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor2y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    If (.intReviseMode <> 1) Then                       ' 自動補正モード ? '###038
                '        strDAT = strDAT & "  一致度POS1   =" & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ","
                '        strDAT = strDAT & " 一致度POS2   =" & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                '    End If
                'Else
                '    ' θ補正表示情報設定(英語)
                '    strDAT = "Theta = " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "degree" & vbCrLf
                '    If (.intReviseMode = 2) Then                        ' 自動+微調の場合
                '        strDAT = "Theta= " & stResult.fTheta.ToString("0.0000").PadLeft(7) & "degree"
                '        strDAT = strDAT & " + " & .dblRotateTheta.ToString("0.0000").PadLeft(7) & "degree" & vbCrLf
                '    End If
                '    strDAT = strDAT & "  Trim PositionXY=" & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPosy.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " Distance=" & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCory.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  Correct position1=" & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos1y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " Distance1=" & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor1y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    strDAT = strDAT & "  Correct position2=" & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fPos2y.ToString("0.0000").PadLeft(9) & "  "
                '    strDAT = strDAT & " Distance2=" & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                '    strDAT = strDAT & stResult.fCor2y.ToString("0.0000").PadLeft(9) & vbCrLf
                '    If (.intReviseMode <> 1) Then                       ' 自動補正モード ? '###038
                '        strDAT = strDAT & "  Correlation coefficient1=" & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ","
                '        strDAT = strDAT & " Correlation coefficient2=" & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                '    End If
                'End If

                ' θ補正表示情報設定
                strDAT = Form1_009 & stResult.fTheta.ToString("0.0000").PadLeft(7) & Form1_018 & vbCrLf
                If (.intReviseMode = 2) Then                        ' 自動+微調の場合
                    strDAT = Form1_009 & stResult.fTheta.ToString("0.0000").PadLeft(7) & Form1_018
                    strDAT = strDAT & "+ " & .dblRotateTheta.ToString("0.0000").PadLeft(7) & Form1_018 & vbCrLf
                End If

                Const lftLen As Integer = 18
                Const rhtLen As Integer = 11
                Dim sp As Integer
                '    strDAT = strDAT & "  トリム位置X,Y=" & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_010)
                strDAT = strDAT & "  " & Form1_010 & New String(" ", sp) & stResult.fPosx.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fPosy.ToString("0.0000").PadLeft(9) & "  "

                '    strDAT = strDAT & " ずれ量X,Y    =" & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_011)
                strDAT = strDAT & "  " & Form1_011 & New String(" ", sp) & stResult.fCorx.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fCory.ToString("0.0000").PadLeft(9) & vbCrLf

                '    strDAT = strDAT & "  補正位置1X,Y =" & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_012)
                strDAT = strDAT & "  " & Form1_012 & New String(" ", sp) & stResult.fPos1x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fPos1y.ToString("0.0000").PadLeft(9) & "  "

                '    strDAT = strDAT & " ずれ量1X,Y   =" & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_013)
                strDAT = strDAT & "  " & Form1_013 & New String(" ", sp) & stResult.fCor1x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fCor1y.ToString("0.0000").PadLeft(9) & vbCrLf

                '    strDAT = strDAT & "  補正位置2X,Y =" & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_014)
                strDAT = strDAT & "  " & Form1_014 & New String(" ", sp) & stResult.fPos2x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fPos2y.ToString("0.0000").PadLeft(9) & "  "

                '    strDAT = strDAT & " ずれ量2X,Y   =" & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_015)
                strDAT = strDAT & "  " & Form1_015 & New String(" ", sp) & stResult.fCor2x.ToString("0.0000").PadLeft(9) & ","
                strDAT = strDAT & stResult.fCor2y.ToString("0.0000").PadLeft(9) & vbCrLf
                If (.intReviseMode <> 1) Then                       ' 自動補正モード ? '###038
                    '        strDAT = strDAT & "  一致度POS1   =" & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ","
                    strDAT = strDAT & "  " & Form1_016 & stResult.fCorV1.ToString("0.0000").PadLeft(9) & ", "
                    '        strDAT = strDAT & " 一致度POS2   =" & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                    strDAT = strDAT & Form1_017 & stResult.fCorV2.ToString("0.0000").PadLeft(9) & vbCrLf
                End If
            End With

            ' 閾値判定 '###038
            If (pltInfo.intReviseMode <> 1) Then                        ' 自動補正モード ? 
                If (Thresh1 > stResult.fCorV1) Or (Thresh2 > stResult.fCorV2) Then
                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    '    ' パターンマッチングエラー(閾値)
                    '    strDAT = strDAT + vbCrLf + MSG_LOADER_07 + "(閾値)"
                    'Else
                    '    strDAT = strDAT + vbCrLf + MSG_LOADER_07 + "(Thresh)"
                    'End If
                    strDAT = strDAT + vbCrLf + MSG_LOADER_07 + Form1_008
                    Return (cFRS_ERR_PTN)                               ' Return値 = パターン認識エラー
                End If
            End If
#End If
            'Return (cFRS_NORMAL)
            Return r   'V5.0.0.9⑤

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SUb_ThetaCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' 戻値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

    End Function
#End Region

#Region "θ補正実行"
    '''=========================================================================
    '''<summary>θ補正実行</summary>
    '''<param name="dblCorrectX">(OUT) XYテーブル補正値X</param>
    '''<param name="dblCorrectY">(OUT) XYテーブル補正値Y</param>
    ''' <param name="results">θ補正結果リスト</param>
    '''<returns>cFRS_NORMAL   = 正常
    '''         cFRS_ERR_PTN  = パターン認識エラー
    '''         上記以外      = その他エラー
    '''</returns>
    '''=========================================================================
    Private Function ThetaCorrection(ByRef dblCorrectX As Double, ByRef dblCorrectY As Double, ByVal results As List(Of PatternRecog),
                                     ByVal doAlign As Boolean, ByVal doRough As Boolean) As Integer 'V5.0.0.9⑩
        'Private Function ThetaCorrection(ByRef dblCorrectX As Double, ByRef dblCorrectY As Double) As Integer

        Dim r As Integer = cFRS_NORMAL
        Dim strMSG As String                                ' メッセージ編集域

        Try
            ' 初期処理
            ThetaCorrection = cFRS_NORMAL                   ' Return値 = 正常
#If False Then
            Call InitThetaCorrection()                      ' パターン登録初期値設定

            ' カメラ切替
            If (gSysPrm.stDEV.giEXCAM = 1) Then             ' 外部カメラを使用？
                Call VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA)          ' カメラ切替(外部ｶﾒﾗ)
            End If

            ' θ補正処理
            VideoLibrary1.frmTop = Me.Text4.Location.Y      ' ###027
            VideoLibrary1.frmLeft = Me.Text4.Location.X     ' ###027

            Call VideoLibrary1.PatternDisp(True)            ' パターンマッチング時の検索範囲枠表示 '###155 
            r = VideoLibrary1.CorrectTheta(giAppMode)       ' θ補正
            Call VideoLibrary1.PatternDisp(False)           ' パターンマッチング時の検索範囲枠非表示 '###155

            ' XYテーブル補正値(θ補正時のXYﾃｰﾌﾞﾙずれ量)取得
            If (r = 0) Then
                dblCorrectX = VideoLibrary1.CorrectTrimPosX
                dblCorrectY = VideoLibrary1.CorrectTrimPosY
            Else
                dblCorrectX = 0
                dblCorrectY = 0
            End If
            '###252
            '----- V1.13.0.0⑨(Video.OCX V8.0.13対応) -----
            ''###252
            'Select Case gSysPrm.stDEV.giBpDirXy
            '    Case 0 ' 右上(x←, y↓)
            '    Case 1
            '        dblCorrectX = -1 * dblCorrectX
            '    Case 2
            '    Case 3
            'End Select
            '----- V1.13.0.0⑨ -----
#Else
            'V5.0.0.9⑥                  ↓
            Dim threshPt1 As Double
            Dim threshPt2 As Double
            Dim correctTrimPosX As Double = 0.0
            Dim correctTrimPosY As Double = 0.0
            Dim clampLessOffsetX As Double
            Dim clampLessOffsetY As Double
            Dim correctTheta As Double = 0.0
            Dim result As PatternRecog

            '----- V6.0.3.0_49↓ -----
            ' パターンマッチングは大きい画面で行う(SL436S時)
            If (gKeiTyp = MACHINE_KD_RS) Then
                SetTeachVideoSize()
                Form1.Instance.VideoLibrary1.BringToFront()
            End If
            '----- V6.0.3.0_49↑ -----

            If (True = bFgAutoMode) AndAlso (0 <> giClampLessStage) Then
                ' 自動搬送時のクランプレス載物台基板搭載位置オフセット
                clampLessOffsetX = gdClampLessOffsetX
                clampLessOffsetY = gdClampLessOffsetY

                If (True = doRough) Then
                    With typPlateInfo
                        ' ラフアライメント閾値取得
                        threshPt1 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo1GroupNoRgh, .intRevisePtnNo1Rgh)
                        threshPt2 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo2GroupNoRgh, .intRevisePtnNo2Rgh)
                    End With

                    For i As Integer = 1 To giClampLessRoughCount Step 1
                        ' ラフアライメント実行
                        result = New PatternRecog(True, threshPt1, threshPt2)
                        r = SubThetaCorrection(True, clampLessOffsetX, clampLessOffsetY,
                                               correctTrimPosX, correctTrimPosY, correctTheta, result)
                        If (cFRS_NORMAL = r) Then
                            results.Add(result)
                        ElseIf (cFRS_ERR_PT2 = r) Then
                            results.Add(result)
                            Exit For
                        Else
                            Exit For
                        End If
                    Next i
                End If
            Else
                ' 手置きまたはクランプレス載物台ではない
                clampLessOffsetX = 0.0
                clampLessOffsetY = 0.0
            End If

            If (cFRS_NORMAL = r) AndAlso (True = doAlign) Then
                ' θ補正実行
                With typPlateInfo
                    ' θ補整閾値取得
                    threshPt1 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo1GroupNo, .intRevisePtnNo1)
                    threshPt2 = gDllSysprmSysParam_definst.GetPtnMatchThresh(.intRevisePtnNo2GroupNo, .intRevisePtnNo2)
                End With

                result = New PatternRecog(False, threshPt1, threshPt2)
                r = SubThetaCorrection(False, clampLessOffsetX, clampLessOffsetY,
                                       correctTrimPosX, correctTrimPosY, correctTheta, result)
                If (cFRS_NORMAL = r) Then
                    results.Add(result)
                End If
            End If

            ' XYテーブル補正値(θ補正時のXYﾃｰﾌﾞﾙずれ量)取得
            If (cFRS_NORMAL = r) AndAlso (True = doAlign OrElse True = doRough) Then
                dblCorrectX = correctTrimPosX + clampLessOffsetX
                dblCorrectY = correctTrimPosY + clampLessOffsetY
                gdCorrectTheta = correctTheta
            Else
                dblCorrectX = 0.0
                dblCorrectY = 0.0
                gdCorrectTheta = 0.0
            End If
            'V5.0.0.9⑥                  ↑
#End If
            ' 後処理
            If (gSysPrm.stDEV.giEXCAM = 1) Then             ' 外部カメラを使用？
                Call VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)          ' カメラ切替(内部ｶﾒﾗ)
            End If

            '----- V6.0.3.0_49↓ -----
            If (gKeiTyp = MACHINE_KD_RS) Then
                SetSimpleVideoSize()
                Form1.Instance.VideoLibrary1.SendToBack()
            End If
            '----- V6.0.3.0_49↑ -----

            ThetaCorrection = r
            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.ThetaCorrection() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            ThetaCorrection = cERR_TRAP                     ' 戻値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

    End Function
#End Region

#Region "VideoLibrary.CorrectTheta を実行する"
    ''' <summary>
    ''' VideoLibrary.CorrectTheta を実行する
    ''' </summary>
    ''' <param name="isRough">True:ラフアライメント</param>
    ''' <param name="clampLessOffsetX">基板搭載位置オフセットX</param>
    ''' <param name="clampLessOffsetY">基板搭載位置オフセットY</param>
    ''' <param name="correctTrimPosX">補正位置Xのずれ量を指定・その値を加算した実行結果のずれ量を返す</param>
    ''' <param name="correctTrimPosY">補正位置Yのずれ量を指定・その値を加算した実行結果のずれ量を返す</param>
    ''' <param name="correctTheta">補正後のθ角度を指定・その値が加算された補正後のθ角度を返す</param>
    ''' <param name="result">VideoLibrary.GetThetaResult(result)で取得した構造体</param>
    ''' <returns>VideoLibrary.CorrectTheta(AppMode)の戻り値またはcERR_TRAP</returns>
    ''' <remarks>'V5.0.0.9⑥</remarks>
    Private Function SubThetaCorrection(ByVal isRough As Boolean,
                                        ByVal clampLessOffsetX As Double, ByVal clampLessOffsetY As Double,
                                        ByRef correctTrimPosX As Double, ByRef correctTrimPosY As Double,
                                        ByRef correctTheta As Double,
                                        ByVal result As PatternRecog) As Integer

        Dim ret As Integer = cFRS_NORMAL                        ' Return値 = 正常
        Try
            InitThetaCorrection(isRough, clampLessOffsetX, clampLessOffsetY, correctTrimPosX, correctTrimPosY)  ' パターン登録初期値設定

            If (False = isRough) Then
                ' カメラ切替
                If (gSysPrm.stDEV.giEXCAM = 1) Then             ' 外部カメラを使用？
                    VideoLibrary1.ChangeCamera(EXTERNAL_CAMERA) ' カメラ切替(外部ｶﾒﾗ)
                End If
            End If

            ' θ補正処理
            VideoLibrary1.frmTop = Me.Text4.Location.Y      ' ###027
            VideoLibrary1.frmLeft = Me.Text4.Location.X     ' ###027

            'V6.0.0.0-25            VideoLibrary1.PatternDisp(True)            ' パターンマッチング時の検索範囲枠表示 '###155 
            'ret = VideoLibrary1.CorrectTheta(giAppMode)       ' θ補正
            ret = VideoLibrary1.CorrectTheta(giAppMode, isRough, correctTheta)  ' θ補正           'V5.0.0.9⑦
            VideoLibrary1.PatternDisp(False)           ' パターンマッチング時の検索範囲枠非表示 '###155

            ' 補正結果取得
            If (cFRS_NORMAL = ret) Then
                VideoLibrary1.GetThetaResult(result.ThetaCorInfo)

                If (False = isRough) OrElse (result.IsMatch) Then
                    ' XYテーブル補正値(θ補正時のXYﾃｰﾌﾞﾙずれ量)取得
                    correctTrimPosX += VideoLibrary1.CorrectTrimPosX    ' Xのずれ量(mm)
                    correctTrimPosY += VideoLibrary1.CorrectTrimPosY    ' Yのずれ量(mm)
                    correctTheta = result.ThetaCorInfo.fTheta           ' 補正後のθ角度 VideoLibrary内で累計
                Else
                    ' ラフアライメントの場合、閾値超過でマッチングエラーとする
                    ret = cFRS_ERR_PT2  ' パターン認識エラー(閾値エラー)
                End If
            End If

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.SubThetaCorrection() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            ret = cERR_TRAP                     ' 戻値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

        Return ret

    End Function
#End Region

#Region "アライメント結果文字列を生成する"
    ''' <summary>
    ''' アライメント結果文字列を生成する
    ''' </summary>
    ''' <param name="reviseMode"></param>
    ''' <param name="rotateTheta"></param>
    ''' <param name="result"></param>
    ''' <param name="sb"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRecogResult(ByVal reviseMode As Integer, ByVal rotateTheta As Double,
                                    ByVal result As PatternRecog, ByVal sb As StringBuilder) As Integer

        Dim ret As Integer = (cFRS_NORMAL)
        Dim tci As VideoLibrary.Theta_Cor_Info = result.ThetaCorInfo
        Try
            Const lftLen As Integer = 18
            Const rhtLen As Integer = 11
            Const F4 As String = "F4"

            '#Const CLAMPLESS_TEST = True            'V5.0.0.9⑪

#If CLAMPLESS_TEST Then
            WriteClamplessTestLog((82 = sb.Length), reviseMode, result.IsRough, result.IsMatch, tci) 'V5.0.0.9⑪
#End If
            ' θ補正表示情報設定
            If (False = result.IsRough) Then    'V5.0.0.9⑩
                sb.Append(Form1_023)            ' アライメント: 
            Else
                sb.Append(Form1_024)            ' ラフアライメント: 
            End If
            sb.AppendLine(Form1_009 & tci.fTheta.ToString(F4).PadLeft(7) & Form1_018)
            If (2 = reviseMode) Then                        ' 自動+微調の場合
                sb.Append(Form1_009 & tci.fTheta.ToString(F4).PadLeft(7) & Form1_018)
                sb.AppendLine("+ " & rotateTheta.ToString(F4).PadLeft(7) & Form1_018)
            End If

            Dim sp As Integer
            '    strDAT = strDAT & "  トリム位置X,Y=" & result.fPosx.ToString(F4).PadLeft(9) & ","
            sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_010)
            sb.Append("  " & Form1_010 & New String(" "c, sp) & tci.fPosx.ToString(F4).PadLeft(9) & ",")
            sb.Append(tci.fPosy.ToString(F4).PadLeft(9) & "  ")

            '    strDAT = strDAT & " ずれ量X,Y    =" & result.fCorx.ToString(F4).PadLeft(9) & ","
            sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_011)
            sb.Append("  " & Form1_011 & New String(" "c, sp) & tci.fCorx.ToString(F4).PadLeft(9) & ",")
            sb.AppendLine(tci.fCory.ToString(F4).PadLeft(9))

            '    strDAT = strDAT & "  補正位置1X,Y =" & result.fPos1x.ToString(F4).PadLeft(9) & ","
            sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_012)
            sb.Append("  " & Form1_012 & New String(" "c, sp) & tci.fPos1x.ToString(F4).PadLeft(9) & ",")
            sb.Append(tci.fPos1y.ToString(F4).PadLeft(9) & "  ")

            '    strDAT = strDAT & " ずれ量1X,Y   =" & result.fCor1x.ToString(F4).PadLeft(9) & ","
            sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_013)
            sb.Append("  " & Form1_013 & New String(" "c, sp) & tci.fCor1x.ToString(F4).PadLeft(9) & ",")
            sb.AppendLine(tci.fCor1y.ToString(F4).PadLeft(9))

            '    strDAT = strDAT & "  補正位置2X,Y =" & result.fPos2x.ToString(F4).PadLeft(9) & ","
            sp = lftLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_014)
            sb.Append("  " & Form1_014 & New String(" "c, sp) & tci.fPos2x.ToString(F4).PadLeft(9) & ",")
            sb.Append(tci.fPos2y.ToString(F4).PadLeft(9) & "  ")

            '    strDAT = strDAT & " ずれ量2X,Y   =" & result.fCor2x.ToString(F4).PadLeft(9) & ","
            sp = rhtLen - Encoding.GetEncoding(Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage).GetByteCount(Form1_015)
            sb.Append("  " & Form1_015 & New String(" "c, sp) & tci.fCor2x.ToString(F4).PadLeft(9) & ",")
            sb.AppendLine(tci.fCor2y.ToString(F4).PadLeft(9))

            If (1 <> reviseMode) Then                       ' 自動補正モード ? '###038
                '        strDAT = strDAT & "  一致度POS1   =" & result.fCorV1.ToString(F4).PadLeft(9) & ","
                sb.Append("  " & Form1_016 & tci.fCorV1.ToString(F4).PadLeft(9) & ", ")
                '        strDAT = strDAT & " 一致度POS2   =" & result.fCorV2.ToString(F4).PadLeft(9) & vbCrLf
                sb.AppendLine(Form1_017 & tci.fCorV2.ToString(F4).PadLeft(9))

                'If (result.fCorV1 < th1) OrElse (result.fCorV2 < th2) Then
                If (False = result.IsMatch) Then
                    'strDAT = strDAT + vbCrLf + MSG_LOADER_07 + Form1_008
                    sb.AppendLine()
                    sb.AppendLine(MSG_LOADER_07 & Form1_008)
                    ret = (cFRS_ERR_PT2)                    ' Return値 = パターン認識エラー(閾値エラー)
                End If
            End If

            'V4.7.3.2①カット位置ずれ解析用↓
            If gbThetaCorrectionLogOut Then
                Dim strLogMes As String = ""
                ' θ補正表示情報設定(日本語)
                strLogMes = "θ角度=," & tci.fTheta.ToString("0.0000") & ","
                If (reviseMode = 2) Then                        ' 自動+微調の場合
                    strLogMes = "θ角度=," & tci.fTheta.ToString("0.0000") & ","
                    strLogMes = strLogMes & "+," & rotateTheta.ToString("0.0000") & ","
                End If
                strLogMes = strLogMes & "トリム位置XY=," & tci.fPosx.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fPosy.ToString("0.0000") & ","
                strLogMes = strLogMes & "ずれ量XY=," & tci.fCorx.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fCory.ToString("0.0000") & ","
                strLogMes = strLogMes & "補正位置1XY=," & tci.fPos1x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fPos1y.ToString("0.0000") & ","
                strLogMes = strLogMes & "ずれ量1XY=," & tci.fCor1x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fCor1y.ToString("0.0000") & ","
                strLogMes = strLogMes & "補正位置2XY=," & tci.fPos2x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fPos2y.ToString("0.0000") & ","
                strLogMes = strLogMes & "ずれ量2XY=," & tci.fCor2x.ToString("0.0000") & ","
                strLogMes = strLogMes & tci.fCor2y.ToString("0.0000")
                If (reviseMode <> 1) Then                       ' 自動補正モード ? '###038
                    strLogMes = strLogMes & ",一致度POS1=," & tci.fCorV1.ToString("0.0000") & ","
                    strLogMes = strLogMes & "一致度POS2=," & tci.fCorV2.ToString("0.0000")
                End If
                Call System1.OperationLogging(gSysPrm, strLogMes, "ANALYSIS")
            End If
            'V4.7.3.2①↑

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.GetRecogResult() TRAP ERROR = " & ex.Message
            MsgBox(strMSG)
            ret = (cERR_TRAP)                               ' 戻値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

        Return ret

    End Function
#End Region

#Region "クランプレス載物台補正結果ログファイル書き込み"
#If CLAMPLESS_TEST Then
    ''' <summary>
    ''' クランプレス載物台補正結果ログファイル書き込み
    ''' </summary>
    ''' <param name="headerWrite"></param>
    ''' <param name="reviseMode"></param>
    ''' <param name="isRough"></param>
    ''' <param name="isMatch"></param>
    ''' <param name="tci"></param>
    ''' <remarks>'V5.0.0.9⑪</remarks>
    Private Sub WriteClamplessTestLog(ByVal headerWrite As Boolean, ByVal reviseMode As Integer,
                                      ByVal isRough As Boolean, ByVal isMatch As Boolean,
                                      ByVal tci As VideoLibrary.Theta_Cor_Info)
        Const F4 As String = "F4"

        Dim head As New List(Of String)
        If (headerWrite) Then
            head.Add("処理")
            head.Add("θ角度")
            head.Add("トリム位置X")
            head.Add("トリム位置Y")
            head.Add("ずれ量X")
            head.Add("ずれ量Y")
            head.Add("補正位置1X")
            head.Add("補正位置1Y")
            head.Add("ずれ量1X")
            head.Add("ずれ量1Y")
            head.Add("補正位置2X")
            head.Add("補正位置2Y")
            head.Add("ずれ量2X")
            head.Add("ずれ量2Y")
            If (1 <> reviseMode) Then
                head.Add("一致度POS1")
                head.Add("一致度POS2")
                head.Add("結果")
            End If
        End If

        Dim data As New List(Of String)
        If (False = isRough) Then
            data.Add("ｱﾗﾒﾝﾄ")
        Else
            data.Add("ラフ")
        End If
        data.Add(tci.fTheta.ToString(F4))       ' θ角度
        data.Add(tci.fPosx.ToString(F4))        ' トリム位置X
        data.Add(tci.fPosy.ToString(F4))        ' トリム位置Y
        data.Add(tci.fCorx.ToString(F4))        ' ずれ量X
        data.Add(tci.fCory.ToString(F4))        ' ずれ量Y
        data.Add(tci.fPos1x.ToString(F4))       ' 補正位置1X
        data.Add(tci.fPos1y.ToString(F4))       ' 補正位置1Y
        data.Add(tci.fCor1x.ToString(F4))       ' ずれ量1X
        data.Add(tci.fCor1y.ToString(F4))       ' ずれ量1Y
        data.Add(tci.fPos2x.ToString(F4))       ' 補正位置2X
        data.Add(tci.fPos2y.ToString(F4))       ' 補正位置2Y
        data.Add(tci.fCor2x.ToString(F4))       ' ずれ量2X
        data.Add(tci.fCor2y.ToString(F4))       ' ずれ量2Y
        If (1 <> reviseMode) Then
            data.Add(tci.fCorV1.ToString(F4))   ' 一致度POS1
            data.Add(tci.fCorV2.ToString(F4))   ' 一致度POS2

            If (False = isMatch) Then
                data.Add(MSG_LOADER_07 & Form1_008)
            Else
                data.Add("OK")
            End If
        End If

        Using fs As New System.IO.FileStream("C:\TRIMDATA\LOG\ClamplessTest.csv",
                                             System.IO.FileMode.Append,
                                             System.IO.FileAccess.Write,
                                             System.IO.FileShare.Read)
            Using sw As New System.IO.StreamWriter(fs, Encoding.UTF8)

                If (headerWrite) Then
                    Dim h As String = String.Join(",", head)
                    sw.WriteLine()
                    sw.WriteLine(h)
                End If

                Dim d As String = String.Join(",", data)
                sw.WriteLine(d)
            End Using
        End Using

    End Sub
#End If
#End Region

#Region "θ補正等のためのパターン登録処理"
    '''=========================================================================
    ''' <summary>θ補正等のためのパターン登録処理</summary>
    ''' <param name="AppMode">(INP)アプリモード
    '''                        APP_MODE_RECOG         = θ補正の為のパターン登録モード(TKY用)
    '''                        APP_MODE_CARIB_REC     = キャリブレーション補正(外部カメラ)の為のパターン登録モード(CHIP/NET用)
    '''                        APP_MODE_CUTREVISE_REC = カット補正登録(外部カメラ)の為のパターン登録モード(CHIP/NET用)
    '''                        APP_MODE_APROBEREC     = オートプローブの為のパターン登録モード(TKY用) V1.13.0.0③
    ''' </param>
    ''' <param name="pltInfo"> (I/O)プレートデータ</param>
    ''' <returns>0 = 正常, 0以外 = エラー</returns>
    '''=========================================================================
    Private Function PatternTeach(ByVal AppMode As Short, ByRef pltInfo As PlateInfo,
                                  Optional ByVal isRough As Boolean = False) As Short   'V5.0.0.9③

        Dim r As Short
        Dim stPLTI As LaserFront.Trimmer.DllVideo.VideoLibrary.PLATE_Info           ' プレート情報 ※Video.OCXで定義
        Dim stPosiRevis As LaserFront.Trimmer.DllVideo.VideoLibrary.CutPosiRevis_Info                   ' カット補正登録パラメータ ※Video.OCXで定義
        Dim stCalib As LaserFront.Trimmer.DllVideo.VideoLibrary.Calib_Info                              ' キャリブレーション補正パラメータ ※Video.OCXで定義
        Dim strMSG As String
        Dim TblOfsX As Double
        Dim TblOfsY As Double
        Dim parModules As MainModules
        parModules = New MainModules
        Dim videoStcnd As New LaserFront.Trimmer.DllVideo.VideoLibrary.TrimCondInfo()
        Dim i As Integer

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   ビデオＯＣＸ用(θ補正のためのパターン登録処理)パラメータを設定する
                '--------------------------------------------------------------------------
                PatternTeach = cFRS_NORMAL                              ' Return値 = 正常
                Call BSIZE(.dblBlockSizeXDir, .dblBlockSizeYDir)
                'Call InitThetaCorrection()                              ' パターン登録(RECOG)コントロール初期値設定
                Call InitThetaCorrection(isRough)                         ' パターン登録(RECOG)コントロール初期値設定  'V5.0.0.9③
                '----- V1.15.0.0③↓ -----
                ' Zを待機位置へ移動する
                r = EX_ZMOVE(.dblZWaitOffset, MOVE_ABSOLUTE)
                If (r <> cFRS_NORMAL) Then                              ' エラー ?
                    Return (r)
                End If
                '----- V1.15.0.0③↑ -----

                '--------------------------------------------------------------------------
                '   θ補正(ｵﾌﾟｼｮﾝ) & XYテーブル & Zをトリム位置へ移動する
                '--------------------------------------------------------------------------
                ' キャリブレーション時は基準座標1とテーブルオフセット値を加算して移動する 
                If (AppMode = APP_MODE_CARIB_REC) Then
                    '----- V1.20.0.1②↓ -----
                    'TblOfsX = pltInfo.dblCaribBaseCordnt1XDir + pltInfo.dblCaribTableOffsetXDir
                    'TblOfsY = pltInfo.dblCaribBaseCordnt1YDir + pltInfo.dblCaribTableOffsetYDir
                    TblOfsX = 0.0
                    TblOfsY = 0.0
                    '----- V1.20.0.1②↑ -----
                    '----- V1.13.0.0⑤↓ -----
                ElseIf (AppMode = APP_MODE_SINSYUKU) Then
                    TblOfsX = pltInfo.dblContExpPosX
                    TblOfsY = pltInfo.dblContExpPosY
                    '----- V1.13.0.0⑤↑ -----
                Else
                    TblOfsX = 0.0
                    TblOfsY = 0.0
                End If

                ' θ補正(ｵﾌﾟｼｮﾝ) & XYテーブル & Zをトリム位置へ移動する
                '----- V1.13.0.0⑤↓ -----
                If (AppMode <> APP_MODE_RECOG) Then                     ' θ補正の為のパターン登録モード以外 ?
                    'r = Move_Trimposition(pltInfo, 0.0, 0.0)            ' θ補正(ｵﾌﾟｼｮﾝ) & XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動
                    r = Move_Trimposition(pltInfo, TblOfsX, TblOfsY)    ' θ補正(ｵﾌﾟｼｮﾝ) & XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動
                    If (r <> cFRS_NORMAL) Then                          ' エラー ?
                        Return (r)
                    End If
                End If
                '----- V1.13.0.0⑤↑ -----

                '--------------------------------------------------------------------------
                '   パターン登録処理
                '--------------------------------------------------------------------------
                If (KEY_TYPE_RS <> gKeiTyp) Then        'V4.10.0.0⑫ 条件を追加
                    SetVisiblePropForVideo(False)                           ' 画像登録用にメインフォームのコントロールを非表示に設定する
                End If
                VideoLibrary1.SetMainObject(parModules)                 ' 親モジュールのメソッドを設定する。

                Select Case (AppMode)
                    Case APP_MODE_CUTREVISE_REC                         ' カット補正登録(外部カメラ)の為のパターン登録モード
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' プレート情報パラメータ設定
                        Call SetPosiRevisForVideo(pltInfo, stPosiRevis) ' カット補正登録パラメータ設定
                        videoStcnd.Initialize()
                        For i = 0 To MAX_BANK_NUM - 1
                            videoStcnd.Curr(i) = stCND.Curr(i)
                            videoStcnd.Freq(i) = stCND.Freq(i)
                            videoStcnd.Steg(i) = stCND.Steg(i)
                        Next i
                        VideoLibrary1.SetCrossLineObject(gparModules) 'V5.0.0.6⑫
                        r = VideoLibrary1.SetFLCond(videoStcnd)

                        r = VideoLibrary1.PatternRegist_CutPosiRevis(stPLTI, stPosiRevis)
                        'V5.0.0.6⑬↓
                        pltInfo.dblCutPosiReviseOffsetXDir = stPosiRevis.dblCutPosiReviseOffsetXDir        ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
                        pltInfo.dblCutPosiReviseOffsetYDir = stPosiRevis.dblCutPosiReviseOffsetYDir        ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
                        Call gparModules.CrossLineDispOff()
                        'V5.0.0.6⑬↑
                    Case APP_MODE_CARIB_REC                             ' キャリブレーション補正(外部カメラ)の為のパターン登録モード
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' プレート情報パラメータ設定
                        Call SetCalibInfoForVideo(pltInfo, stCalib)     ' キャリブレーション補正パラメータ設定
                        videoStcnd.Initialize()
                        For i = 0 To MAX_BANK_NUM - 1
                            videoStcnd.Curr(i) = stCND.Curr(i)
                            videoStcnd.Freq(i) = stCND.Freq(i)
                            videoStcnd.Steg(i) = stCND.Steg(i)
                        Next i
                        VideoLibrary1.SetCrossLineObject(gparModules) 'V5.0.0.6⑫
                        r = VideoLibrary1.SetFLCond(videoStcnd)
                        r = VideoLibrary1.PatternRegist_Calibration(stPLTI, stCalib)
                        Call gparModules.CrossLineDispOff()

                        '----- V1.13.0.0③↓ -----
                    Case APP_MODE_APROBEREC                             ' オートプローブの為のパターン登録モード
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' プレート情報パラメータ設定
                        r = VideoLibrary1.PatternRegist_AutoProb(stPLTI)
                        '----- V1.13.0.0③↑ -----

                        '----- V1.13.0.0⑤↓ -----
                    Case APP_MODE_SINSYUKU                              ' 伸縮補正用登録モード 
                        Call SetPlateInfoForVideo(pltInfo, stPLTI)      ' プレート情報パラメータ設定
                        r = VideoLibrary1.PatternRegist_ShinSyukuHosei(stPLTI)
                        '----- V1.13.0.0⑤↑ -----

                    Case Else                                           ' θ補正の為のパターン登録モード
                        'r = VideoLibrary1.PatternRegist()               ' パターン登録(XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動も行う)
                        r = VideoLibrary1.PatternRegist(isRough)          ' パターン登録(XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動も行う)     'V5.0.0.9⑦

                End Select
                If (KEY_TYPE_RS <> gKeiTyp) Then        'V4.10.0.0⑫ 条件を追加
                    SetVisiblePropForVideo(True)                            ' 画像登録用にメインフォームのコントロールを表示に設定する
                End If

                ' Video.OCXからの戻り値を判定する
                If (r >= cFRS_NORMAL) Then                              ' 正常終了 ?
                    If (r <> cFRS_ERR_RST) Then                         ' Cancel以外 ?
                        ' トリミングデータ(プレートデータ)を更新する
                        Select Case (AppMode)
                            Case APP_MODE_CUTREVISE_REC                 ' カット補正登録(外部カメラ)の為のパターン登録モード
                                r = VideoLibrary1.GetCutPosiRevisResult(stPosiRevis)                    ' 結果を取得する
                                .dblCutPosiReviseOffsetXDir = stPosiRevis.dblCutPosiReviseOffsetXDir    ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
                                .dblCutPosiReviseOffsetYDir = stPosiRevis.dblCutPosiReviseOffsetYDir    ' ｶｯﾄ点補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
                                .intCutPosiReviseGroupNo = stPosiRevis.intCutPosiReviseGroupNo          ' ｸﾞﾙｰﾌﾟNo
                                .intCutPosiRevisePtnNo = stPosiRevis.intCutPosiRevisePtnNo              ' ｶｯﾄ点補正ﾊﾟﾀｰﾝ登録No
                                '.xxx = stPosiRevis.intCondNo                                           ' 加工条件番号(FL時)

                            Case APP_MODE_CARIB_REC                     ' キャリブレーション補正(外部カメラ)の為のパターン登録モード
                                r = VideoLibrary1.GetCalibrationResult(stCalib)                         ' 結果を取得する
                                '----- V1.20.0.1②↓ -----
                                '----- V1.14.0.0⑦↓ -----
                                'Select Case gSysPrm.stDEV.giBpDirXy
                                '    Case 0 ' x←, y↓
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                                '    Case 1 ' x→, y↓(X反転)
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                                '    Case 2 ' x←, y↑(Y反転)
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                                '    Case 3 ' x→, y↑(XY反転)
                                '        .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                                '        .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                                'End Select
                                .dblCaribTableOffsetXDir = stCalib.dblCaribTableOffsetXDir              ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                                .dblCaribTableOffsetYDir = stCalib.dblCaribTableOffsetYDir              ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                                '----- V1.14.0.0⑦↑ -----
                                '----- V1.20.0.1②↑ -----
                                .intCaribPtnNo1GroupNo = stCalib.intCaribPtnNo1GroupNo                  ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1グループNo
                                .intCaribPtnNo2GroupNo = stCalib.intCaribPtnNo2GroupNo                  ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2グループNo
                                .intCaribPtnNo1 = stCalib.intCaribPtnNo1                                ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1
                                .intCaribPtnNo2 = stCalib.intCaribPtnNo2                                ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2
                                '.xxx = stCalib.intCondNo                                               ' 加工条件番号(FL時)

                                '----- V1.13.0.0③↓ -----
                            Case APP_MODE_APROBEREC                     ' オートプローブの為のパターン登録モード
                                r = VideoLibrary1.GetAutoProbResult(stPLTI)                             ' 結果を取得する
                                .dblStepMeasBpPosX = stPLTI.dblAProbeBpPosX                             ' パターン1(下方)用BP位置X
                                .dblStepMeasBpPosY = stPLTI.dblAProbeBpPosY                             ' パターン1(下方)用BP位置Y
                                .dblStepMeasTblOstX = stPLTI.dblAProbeStgPosX                           ' パターン2(上方)用ステージ位置X
                                .dblStepMeasTblOstY = stPLTI.dblAProbeStgPosY                           ' パターン2(上方)用ステージ位置Y
                                '----- V1.13.0.0③↑ -----

                                '----- V1.13.0.0⑤↓ -----
                            Case APP_MODE_SINSYUKU                      ' 伸縮補正用登録モード 
                                r = VideoLibrary1.GetShinsyukuResult(stPLTI)
                                .dblContExpPosX = stPLTI.dblShinsyukuPosX
                                .dblContExpPosY = stPLTI.dblShinsyukuPosY

                                '----- V1.13.0.0⑤↑ -----

                            Case Else                                   ' θ補正の為のパターン登録モード
                                If (False = isRough) Then    'V5.0.0.9④
                                    .dblReviseOffsetXDir = VideoLibrary1.pp34_x                             ' 補正ポジションオフセットX更新
                                    .dblReviseOffsetYDir = VideoLibrary1.pp34_y                             ' 補正ポジションオフセットY更新
                                    '----- ###234↓ -----
                                    ' シスパラ(TKY.iniのOPT_VIDEOのTABLE_POS_UPDATE)で「テーブル1,2座標を更新する(VIDEO.OCX用オプション)」
                                    '　指定時は「トリミングデータ(パターン登録データ)のパターン座標1,2を更新する
                                    If (giTablePosUpd = 1) Then
                                        .dblReviseCordnt1XDir = VideoLibrary1.pp32_x                        ' パターン1座標x
                                        .dblReviseCordnt1YDir = VideoLibrary1.pp32_y                        ' パターン1座標y
                                        .dblReviseCordnt2XDir = VideoLibrary1.PP33X                         ' パターン2座標x
                                        .dblReviseCordnt2YDir = VideoLibrary1.PP33Y                         ' パターン2座標y
                                    End If
                                    '----- ###234↑ -----
                                Else
                                    ' クランプレス載物台・ラフアライメント用画像登録   'V5.0.0.9④      ↓
                                    .dblReviseOffsetXDirRgh = VideoLibrary1.pp34_x              ' 補正ポジションオフセットX更新
                                    .dblReviseOffsetYDirRgh = VideoLibrary1.pp34_y              ' 補正ポジションオフセットY更新
                                    ' シスパラ(TKY.iniのOPT_VIDEOのTABLE_POS_UPDATE)で「テーブル1,2座標を更新する(VIDEO.OCX用オプション)」
                                    '　指定時は「トリミングデータ(パターン登録データ)のパターン座標1,2を更新する
                                    If (giTablePosUpd = 1) Then
                                        .dblReviseCordnt1XDirRgh = VideoLibrary1.pp32_x         ' パターン1座標x
                                        .dblReviseCordnt1YDirRgh = VideoLibrary1.pp32_y         ' パターン1座標y
                                        .dblReviseCordnt2XDirRgh = VideoLibrary1.PP33X          ' パターン2座標x
                                        .dblReviseCordnt2YDirRgh = VideoLibrary1.PP33Y          ' パターン2座標y
                                    End If
                                    '                                               V5.0.0.9④       ↑
                                End If
                        End Select
                        gCmpTrimDataFlg = 1                             ' データ更新フラグ = 1(更新あり)
                    End If

                    ' Video.OCXエラー ?
                ElseIf (r >= cFRS_MVC_10) And (r <= cFRS_VIDEO_INI) Then
                    Select Case r
                        Case cFRS_VIDEO_INI
                            strMSG = "VIDEOLIB: Not initialized."       ' "期化が行われていません。
                        Case cFRS_VIDEO_FRM
                            strMSG = "VIDEOLIB: Form Display Now"       ' フォームの表示中です。
                        Case cFRS_VIDEO_PRP
                            strMSG = "VIDEOLIB: Invalid property value." ' プロパティ値が不正です
                        Case cFRS_VIDEO_UXP
                            strMSG = "VIDEOLIB: Unexpected error"       ' 予期せぬエラー
                        Case Else
                            strMSG = "VIDEOLIB: Unexpected error 2"
                    End Select
                    Call System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, gAppName)

                Else                                                    ' 非常停止等エラー
                    PatternTeach = r                                    ' Return値設定
                    Exit Function
                End If
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.PatternTeach() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            PatternTeach = cERR_TRAP                                    ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "プレート情報パラメータ設定処理(パターン登録用)"
    '''=========================================================================
    ''' <summary>プレート情報パラメータ設定処理(パターン登録用)</summary>
    ''' <param name="pltInfo">(INP)プレートデータ</param>
    ''' <param name="stPLTI"> (OUT)プレート情報</param>
    '''=========================================================================
    Private Sub SetPlateInfoForVideo(ByRef pltInfo As PlateInfo, ByRef stPLTI As LaserFront.Trimmer.DllVideo.VideoLibrary.PLATE_Info)

        Dim strMSG As String                                            ' メッセージ編集域

        Try
            With pltInfo
                stPLTI.intResistDir = .intResistDir                             ' 抵抗並び方向
                stPLTI.intResistCntInGroup = .intResistCntInBlock               ' ブロック内抵抗数
                stPLTI.intBlockCntXDir = .intBlockCntXDir                       ' ブロック数Ｘ
                stPLTI.intBlockCntYDir = .intBlockCntYDir                       ' ブロック数Ｙ
                stPLTI.intGroupCntInBlockXBp = .intGroupCntInBlockXBp           ' ブロック内BPグループ数(X方向）
                stPLTI.intGroupCntInBlockYStage = .intGroupCntInBlockYStage     ' ブロック内Stageグループ数(Y方向）
                stPLTI.dblChipSizeXDir = .dblChipSizeXDir                       ' チップサイズX
                stPLTI.dblChipSizeYDir = .dblChipSizeYDir                       ' チップサイズY
                stPLTI.dblBpGrpItv = .dblBpGrpItv                               ' BPグループ間隔
                stPLTI.dblblStgGrpItv = .dblStgGrpItvY                          ' Stageグループ間隔
                stPLTI.dbldispXPos = FORM_X + VideoLibrary1.Location.X          ' 画像表示プログラムの表示位置X
                stPLTI.dbldispYPos = FORM_Y + VideoLibrary1.Location.Y          ' 画像表示プログラムの表示位置Y

                stPLTI.dblBlockSizeXDir = .dblBlockSizeXDir                     ' ブロックサイズX                   '###120
                stPLTI.dblBlockSizeYDir = .dblBlockSizeYDir                     ' ブロックサイズY                   '###120
                stPLTI.dblbpoffX = .dblBpOffSetXDir                             ' Bp-OffsetX                        '###120
                stPLTI.dblbpoffY = .dblBpOffSetYDir                             ' Bp-OffsetyY                       '###120
                '----- V1.13.0.0③↓ -----
                ' オートプローブ用
                stPLTI.CorrectTrimPosX = gfCorrectPosX                          ' トリムポジション補正値X
                stPLTI.CorrectTrimPosY = gfCorrectPosY                          ' トリムポジション補正値Y
                stPLTI.intAProbeGroupNo1 = .intStepMeasLwGrpNo                  ' パターン1(下方)用グループ番号
                stPLTI.intAProbePtnNo1 = .intStepMeasLwPtnNo                    ' パターン1(下方)用パターン番号
                stPLTI.intAProbeGroupNo2 = .intStepMeasUpGrpNo                  ' パターン2(上方)用グループ番号
                stPLTI.intAProbePtnNo2 = .intStepMeasUpPtnNo                    ' パターン2(上方)用パターン番号
                stPLTI.dblAProbeBpPosX = .dblStepMeasBpPosX                     ' パターン1(下方)用BP位置X
                stPLTI.dblAProbeBpPosY = .dblStepMeasBpPosY                     ' パターン1(下方)用BP位置Y
                stPLTI.dblAProbeStgPosX = .dblStepMeasTblOstX                   ' パターン2(上方)用ステージ位置X
                stPLTI.dblAProbeStgPosY = .dblStepMeasTblOstY                   ' パターン2(上方)用ステージ位置Y
                stPLTI.intAProbeStepCount = .intStepMeasCnt                     ' ステップ実行用ステップ回数
                stPLTI.intAProbeStepCount2 = .intStepMeasReptCnt                ' ステップ実行用繰返しステップ回数
                stPLTI.dblAProbePitch = .dblStepMeasPitch                       ' ステップ実行用ステップピッチ
                stPLTI.dblAProbePitch2 = .dblStepMeasReptPitch                  ' ステップ実行用繰返しステップピッチ
                stPLTI.dblZ2OnPos = .dblLwPrbStpUpDist                          ' Z2 On位置
                stPLTI.dblZ2OffPos = .dblLwPrbStpDwDist                         ' Z2 Off位置
                '----- V1.13.0.0③ -----
                'V1.13.0.0⑤
                stPLTI.dblShinsyukuPosX = .dblContExpPosX
                stPLTI.dblShinsyukuPosY = .dblContExpPosY
                'V1.13.0.0⑤

            End With
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetPlateInfoForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "カット補正登録(外部カメラ)の為のパターン登録パラメータ設定処理(パターン登録用)"
    '''=========================================================================
    ''' <summary>カット補正登録(外部カメラ)の為のパターン登録パラメータ設定処理(パターン登録用)</summary>
    ''' <param name="pltInfo">    (INP)プレートデータ</param>
    ''' <param name="stPosiRevis">(OUT)カット補正登録(外部カメラ)の為のパターン登録パラメータ</param>
    '''=========================================================================
    Private Sub SetPosiRevisForVideo(ByRef pltInfo As PlateInfo, ByRef stPosiRevis As LaserFront.Trimmer.DllVideo.VideoLibrary.CutPosiRevis_Info)

        Dim strMSG As String                                            ' メッセージ編集域

        Try
            With pltInfo
                stPosiRevis.dblCutPosiReviseOffsetXDir = .dblCutPosiReviseOffsetXDir        ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄX
                stPosiRevis.dblCutPosiReviseOffsetYDir = .dblCutPosiReviseOffsetYDir        ' ｶｯﾄ位置補正ﾃｰﾌﾞﾙｵﾌｾｯﾄY
                stPosiRevis.dblCutPosiReviseCutLength = .dblCutPosiReviseCutLength          ' ｶｯﾄ位置補正ｶｯﾄ長
                stPosiRevis.dblCutPosiReviseCutSpeed = .dblCutPosiReviseCutSpeed            ' ｶｯﾄ位置補正ｶｯﾄ速度
                stPosiRevis.dblCutPosiReviseCutQRate = .dblCutPosiReviseCutQRate            ' ｶｯﾄ位置補正ﾚｰｻﾞQﾚｰﾄ
                stPosiRevis.dblCutBpPosX = typResistorInfoArray(1).ArrCut(1).dblStartPointX ' 最初のカット位置X
                stPosiRevis.dblCutBpPosY = typResistorInfoArray(1).ArrCut(1).dblStartPointY ' 最初のカット位置Y
                stPosiRevis.intCutPosiReviseGroupNo = .intCutPosiReviseGroupNo              ' ｶｯﾄ位置補正ｸﾞﾙｰﾌﾟNo
                stPosiRevis.intCutPosiRevisePtnNo = .intCutPosiRevisePtnNo                  ' ｶｯﾄ位置補正ﾊﾟﾀｰﾝ登録No
                stPosiRevis.intCondNo = .intCutPosiReviseCondNo                             ' ｶｯﾄ位置補正加工条件番号(FL時)
            End With
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetPosiRevisForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "キャリブレーション補正(外部カメラ)の為のパターン登録パラメータ設定処理(パターン登録用)"
    '''=========================================================================
    ''' <summary>キャリブレーション補正(外部カメラ)の為のパターン登録パラメータ設定処理(パターン登録用)</summary>
    ''' <param name="pltInfo">(INP)プレートデータ</param>
    ''' <param name="stCalib">(OUT)キャリブレーション補正(外部カメラ)の為のパターン登録パラメータ</param>
    '''=========================================================================
    Private Sub SetCalibInfoForVideo(ByRef pltInfo As PlateInfo, ByRef stCalib As LaserFront.Trimmer.DllVideo.VideoLibrary.Calib_Info)

        Dim strMSG As String                                            ' メッセージ編集域

        Try
            With pltInfo
                stCalib.dblCaribBaseCordnt1XDir = .dblCaribBaseCordnt1XDir  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
                stCalib.dblCaribBaseCordnt1YDir = .dblCaribBaseCordnt1YDir  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
                stCalib.dblCaribBaseCordnt2XDir = .dblCaribBaseCordnt2XDir  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
                stCalib.dblCaribBaseCordnt2YDir = .dblCaribBaseCordnt2YDir  ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
                '----- V1.20.0.1②↓ -----
                ' V1.14.0.0⑦の修正は削除
                '----- V1.14.0.0⑦↓ -----
                'Select Case gSysPrm.stDEV.giBpDirXy
                '    Case 0 ' x←, y↓
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                '    Case 1 ' x→, y↓(X反転)
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                '    Case 2 ' x←, y↑(Y反転)
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                '    Case 3 ' x→, y↑(XY反転)
                '        stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                '        stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir * -1 ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                'End Select
                stCalib.dblCaribTableOffsetXDir = .dblCaribTableOffsetXDir  ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
                stCalib.dblCaribTableOffsetYDir = .dblCaribTableOffsetYDir  ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
                '----- V1.14.0.0⑦↑ -----
                '----- V1.20.0.1②↑ -----
                stCalib.dblCaribCutLength = .dblCaribCutLength              ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ長
                stCalib.dblCaribCutSpeed = .dblCaribCutSpeed                ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ速度
                stCalib.dblCaribCutQRate = .dblCaribCutQRate                ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾚｰｻﾞQﾚｰﾄ
                stCalib.intCaribPtnNo1GroupNo = .intCaribPtnNo1GroupNo      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1グループNo
                stCalib.intCaribPtnNo2GroupNo = .intCaribPtnNo2GroupNo      ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2グループNo
                stCalib.intCaribPtnNo1 = .intCaribPtnNo1                    ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No1
                stCalib.intCaribPtnNo2 = .intCaribPtnNo2                    ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾊﾟﾀｰﾝ登録No2
                stCalib.intCondNo = .intCaribCutCondNo                      ' ｷｬﾘﾌﾞﾚｰｼｮﾝ加工条件番号(FL時)
            End With
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetCalibInfoForVideo() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "カット位置補正(F10)ﾎﾞﾀﾝ押下時処理【TKY用】"
    '''=========================================================================
    '''<summary>カット位置補正(F10)ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks>カット位置補正のためのパターン登録処理を行う</remarks>
    '''=========================================================================
    Private Sub CmdCutPos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdCutPos.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_CUTPOS, APP_MODE_CUTPOS, MSG_OPLOG_FUNC08S, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCutPos_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

STP_END:
    End Sub
#End Region

#Region "カット位置補正のためのパターン登録処理【TKY用】"
    '''=========================================================================
    '''<summary>カット位置補正のためのパターン登録処理【TKY用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Function CutPosTeach(ByRef pltInfo As PlateInfo) As Short
        'Private Function CutPosTeach(ByVal pltInfo As PlateInfo) As Short

        Dim r As Short
        Dim strMSG As String

        Dim rn As Short
        Dim idx As Short
        Dim sRName(MaxRegNum) As String                         ' 抵抗名(抵抗番号)テーブル
        Dim StartPosTbl(2, MaxRegNum) As Double                 ' パターン位置X,Yテーブル
        Dim PtnGrpTbl(MaxRegNum) As Short                       ' グループ番号テーブル
        Dim PtnNumTbl(MaxRegNum) As Short                       ' パターン番号テーブル

        Try
            With pltInfo
                '--------------------------------------------------------------------------
                '   θ補正(ｵﾌﾟｼｮﾝ) & XYテーブルをトリム位置へ移動する
                '--------------------------------------------------------------------------
                CutPosTeach = cFRS_NORMAL                           ' Return値 = 正常
                ChDir(My.Application.Info.DirectoryPath)            ' exeのあるﾌｫﾙﾀﾞｰにMV10のDLLもないとダメ ?
                '                                                   ' (デバッグ時は\i-TKY_VS2005\binにMV10のDLLもないとダメ?)
                'Call BSIZE(.dblBlockSizeXDir, .dblBlockSizeYDir) 
                r = Move_Trimposition(pltInfo, 0.0, 0.0)            ' θ補正(ｵﾌﾟｼｮﾝ) & XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動
                SetTeachVideoSize()                                 ' V6.0.3.0_49
                If (r <> cFRS_NORMAL) Then                          ' エラー ?
                    CutPosTeach = r                                 ' Return値設定
                    Exit Function
                End If

                ' BPｵﾌｾｯﾄ設定
                'Call System1.EX_BPOFF(gSysPrm, .dblBpOffSetXDir, .dblBpOffSetYDir)
                'If (r <> cFRS_NORMAL) Then GoTo STP_END

                '--------------------------------------------------------------------------
                '   ビデオＯＣＸ(カット位置補正処理)に渡すテーブルを作成する
                '---------------------------------------------------------------------------
                idx = 0                                             ' テーブルインデックス
                For rn = 1 To gRegistorCnt                          ' 抵抗数分設定する
                    ' カット位置補正する抵抗 ?
                    If (typResistorInfoArray(rn).intCutReviseMode = 1) Then
                        idx = idx + 1                               ' テーブルインデックス + 1
                        ' 抵抗名(抵抗番号)テーブル
                        sRName(idx) = "R" + typResistorInfoArray(rn).intResNo.ToString()
                        ' パターン位置X,Yテーブル
                        StartPosTbl(1, idx) = typResistorInfoArray(rn).dblCutRevisePosX
                        StartPosTbl(2, idx) = typResistorInfoArray(rn).dblCutRevisePosY
                        ' グループ番号テーブル
                        PtnGrpTbl(idx) = typResistorInfoArray(rn).intCutReviseGrpNo
                        ' パターン番号テーブル
                        PtnNumTbl(idx) = typResistorInfoArray(rn).intCutRevisePtnNo
                    End If
                Next rn

                If (idx <= 0) Then
                    ' "カット位置補正対象の抵抗がありません"
                    Call System1.TrmMsgBox(gSysPrm, MSG_39, vbExclamation Or vbOKOnly, gAppName)
                    CutPosTeach = cFRS_ERR_RST                      ' Return値 = Cancel
                    Exit Function
                End If
                ReDim Preserve sRName(idx)                          ' (注)抵抗数分で再定義しないとVideo.OCXのグリッドに配列数分処理されてしまう

                '--------------------------------------------------------------------------
                '   ビデオＯＣＸ用パラメータを設定する
                '--------------------------------------------------------------------------
                VideoLibrary1.pp32_x = 0.0#                         ' CorStgPos1X
                VideoLibrary1.pp32_y = 0.0#                         ' CorStgPos1Y
                VideoLibrary1.pp34_x = .dblBpOffSetXDir             ' Bp Offset X
                VideoLibrary1.pp34_y = .dblBpOffSetYDir             ' Bp Offset Y
                VideoLibrary1.frmLeft = Me.Text4.Location.X         ' 表示位置(Left)
                VideoLibrary1.frmTop = Me.Text4.Location.Y          ' 表示位置(Top)
                VideoLibrary1.pfTrim_x = gSysPrm.stDEV.gfTrimX      ' トリム位置x
                VideoLibrary1.pfTrim_y = gSysPrm.stDEV.gfTrimY      ' トリム位置y
                VideoLibrary1.pfStgOffX = .dblTableOffsetXDir       ' Trim Position Offset Y(mm)
                VideoLibrary1.pfStgOffY = .dblTableOffsetYDir       ' Trim Position Offset Y(mm)
                VideoLibrary1.pfBlock_x = .dblBlockSizeXDir         ' Block Size x(mm)
                VideoLibrary1.pfBlock_y = .dblBlockSizeYDir         ' Block Size y(mm)
                VideoLibrary1.zwaitpos = .dblZWaitOffset            ' Z PROBE OFF OFFSET(mm)
                VideoLibrary1.RNASTMPNUM = True                     ' カット位置補正モード 
                VideoLibrary1.ZON = .dblZOffSet                     ' Z ON位置 
                VideoLibrary1.ZOFF = .dblZWaitOffset                ' Z OFF位置 

                ' テンプレートグループ選択/テンプレート番号設定
                Call VideoLibrary1.SelectTemplateGroup(PtnGrpTbl(1))       ' 最初のグループ番号を設定
                r = VideoLibrary1.SetTemplateNum_EX(idx, PtnNumTbl, PtnGrpTbl)

                '--------------------------------------------------------------------------
                '   カット位置補正画面表示
                '--------------------------------------------------------------------------
                SetVisiblePropForVideo(False)                       ' 画像登録用にメインフォームのコントロールを非表示に設定する
                r = VideoLibrary1.CutPosTeach(sRName, StartPosTbl, .dblBlockSizeXDir, .dblBlockSizeYDir,
                                       .dblBpOffSetXDir, .dblBpOffSetYDir)
                SetVisiblePropForVideo(True)                        ' 画像登録用にメインフォームのコントロールを表示に設定する                'V3.0.0.0③ キャンセル時及びその他のエラー時もデータ更新している不具合修正の為ＩＦ文全体を書き直し↓
                CutPosTeach = r                                             ' Return値設定 
                If (r < cFRS_NORMAL) Or (r = cFRS_ERR_RST) Then
                    If (r <= cFRS_VIDEO_INI) Then                               ' Video.OCXエラー ?
                        Select Case r
                            Case cFRS_VIDEO_INI
                                strMSG = "VIDEOLIB: Not initialized."           ' 初期化が行われていません。
                            Case cFRS_VIDEO_FRM
                                strMSG = "VIDEOLIB: Form Display Now"           ' フォームの表示中です。
                            Case cFRS_VIDEO_PRP
                                strMSG = "VIDEOLIB: Invalid property value."    ' プロパティ値が不正です
                            Case cFRS_VIDEO_UXP
                                strMSG = "VIDEOLIB: Unexpected error"           ' 予期せぬエラー
                            Case Else
                                strMSG = "VIDEOLIB: Unexpected error 2"
                        End Select
                        Call System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, gAppName)
                    End If
                    Exit Function
                Else
                    gCmpTrimDataFlg = 1                         ' データ更新フラグ = 1(更新あり)
                End If
                'V3.0.0.0③ キャンセル時及びその他のエラー時もデータ更新している不具合修正の為IF文書き直し↑

                '--------------------------------------------------------------------------
                '   パターン位置XYを取得する
                '--------------------------------------------------------------------------
                r = VideoLibrary1.GetResult(StartPosTbl)            ' 結果取得
                If (r = cFRS_NORMAL) Then                           ' 正常終了 ?
                    idx = 0                                         ' テーブルインデックス
                    For rn = 1 To gRegistorCnt                      ' 抵抗数分設定する
                        ' カット位置補正する抵抗 ?
                        If (typResistorInfoArray(rn).intCutReviseMode = 1) Then
                            idx = idx + 1                           ' テーブルインデックス + 1
                            ' パターン位置X,Yテーブル
                            typResistorInfoArray(rn).dblCutRevisePosX = StartPosTbl(1, idx)
                            typResistorInfoArray(rn).dblCutRevisePosY = StartPosTbl(2, idx)
                        End If
                    Next rn
                    gCmpTrimDataFlg = 1                             ' データ更新フラグ = 1(更新あり)
                End If
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CutPosTeach() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CutPosTeach = cERR_TRAP                                 ' Return値 = 例外エラー
        End Try

    End Function
#End Region

#Region "TXﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理【CHIP/NET用】"
    '''=========================================================================
    '''<summary>TXﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理【CHIP/NET用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTx.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' フラグ初期設定
            g_blnTx2Ty2Exec = False                         ' TX2実行フラグをFALSEにしておく
            g_blnTxTyExec = True                            ' TX実行フラグをTRUEにする

            ' コマンド実行
            r = CmdExec_Proc(F_TX, APP_MODE_TX, MSG_OPLOG_TX_START, "")
            'If (r = cFRS_NORMAL) Then
            '    ' TX2実行フラグがTRUEの場合、ﾃｨｰﾁﾝｸﾞ処理を呼び出す。
            '    If (g_blnTx2Ty2Exec) Then                   ' TX画面終了時に「TX2ﾎﾞﾀﾝ」を選択された ? 
            '        r = Teaching()
            '        If (r <= cFRS_ERR_EMG) Then
            '            Call AppEndDataSave()
            '            Call AplicationForcedEnding()       ' 強制終了
            '        End If
            '        g_blnTx2Ty2Exec = False                 ' TX2実行フラグをFALSEにする
            '    End If
            '    g_blnTxTyExec = False                       ' TX実行フラグをFALSEにする
            'End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTx_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "TYﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理【CHIP/NET用】"
    '''=========================================================================
    '''<summary>TYﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理【CHIP/NET用】</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTy.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_TY, APP_MODE_TY, MSG_OPLOG_TY_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTy_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "TY2ﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>TY2ﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdTy2_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdTy2.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_TY2, APP_MODE_TY2, MSG_OPLOG_TY2_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdTy2_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "Tθﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>Tθﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdT_Theta_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdT_Theta.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_TTHETA, APP_MODE_TTHETA, MSG_OPLOG_T_THETA_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdT_Theta_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

#End Region

#Region "外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】"
    '''=========================================================================
    '''<summary>外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】</summary>
    '''<remarks>第１抵抗のみ
    '''         外部カメラのある場合のみ有効</remarks>
    '''=========================================================================
    Private Sub CmdExCam_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdExCam.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_EXR1, APP_MODE_EXCAM_R1TEACH, MSG_OPLOG_ExCam_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExCam_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "外部ｶﾒﾗﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】"
    '''=========================================================================
    '''<summary>外部ｶﾒﾗﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】</summary>
    '''<remarks>全抵抗
    '''         外部カメラのある場合のみ有効</remarks>
    '''=========================================================================
    Private Sub CmdExCam1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdExCam1.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_EXTEACH, APP_MODE_EXCAM_TEACH, MSG_OPLOG_ExCam1_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExCam1_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録ﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】"
    '''=========================================================================
    '''<summary>ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録ﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】</summary>
    '''<remarks>外部カメラのある場合のみ有効</remarks>
    '''=========================================================================
    Private Sub CmdPtnCalibration_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdPtnCalibration.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_CARREC, APP_MODE_CARIB_REC, MSG_OPLOG_CALIBRATION_RECOG_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdPtnCalibration_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try


        '        Dim mFrmObj As System.Windows.Forms.Form
        '        Dim r As Short
        '        Dim strMSG As String

        '        Try

        '            ' ローダー出力(ON=トリマ動作中,OFF=なし)
        '            Call ZATLDSET(COM_STS_TRM_STATE, 0)
        '            ' モードのセット
        '            giAppMode = APP_MODE_CARIB_REC

        '            ' コマンド実行前初期処理
        '            'r = CmdExec_Init(giAppMode, MSG_OPLOG_CALIBRATION_RECOG_START)
        '            If (r <> cFRS_NORMAL) Then
        '                GoTo RETURNMENU
        '            End If

        '            ' キャリブレーション補正登録実行
        '            gbInPattern = True
        '            mFrmObj = New frmPtnCalibration
        '            mFrmObj.Show()

        '            'Do
        '            '    System.Windows.Forms.Application.DoEvents()
        '            'Loop While gbInPattern

        '            ' トラップエラー発生時 
        '        Catch ex As Exception
        '            strMSG = "cmdPtnCalibration_Click() TRAP ERROR = " + ex.Message
        '            MsgBox(strMSG)
        '        End Try

        'RETURNMENU:
        '        ' コマンド実行後処理
        '        'Call CmdExec_Term()

        '        ' IDLEモード
        '        Call ZCONRST()                                  ' ラッチ解除
        '        Timer1.Enabled = True
        '        giAppMode = APP_MODE_IDLE                       ' ｱﾌﾟﾘﾓｰﾄﾞ = トリマ装置アイドル中

        '        ' ローダー出力(ON=なし,OFF=トリマ動作中)
        '        Call ZATLDSET(0, COM_STS_TRM_STATE)
    End Sub
#End Region

#Region "ｷｬﾘﾌﾞﾚｰｼｮﾝﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】"
    '''=========================================================================
    '''<summary>ｷｬﾘﾌﾞﾚｰｼｮﾝﾎﾞﾀﾝ押下時処理(外部カメラ)【CHIP/NET用】</summary>
    '''<remarks>外部カメラのある場合のみ有効</remarks>
    '''=========================================================================
    Private Sub CmdCalibration_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCalibration.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_CAR, APP_MODE_CARIB, MSG_OPLOG_CALIBRATION_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCalibration_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "カット補正登録ボタン押下時処理(外部カメラ)【CHIP/NET用】"
    '''=========================================================================
    '''<summary>カット補正登録ボタン押下時処理(外部カメラ)【CHIP/NET用】</summary>
    '''<remarks>外部カメラのある場合のみ有効</remarks>
    '''=========================================================================
    Private Sub CmdPtnCutPosCorrect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdPtnCutPosCorrect.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_CUTREC, APP_MODE_CUTREVISE_REC, MSG_OPLOG_CUT_POS_CORRECT_RECOG_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdPtnCutPosCorrect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "カット補正ボタン押下時処理(外部カメラ)【CHIP/NET用】"
    '''=========================================================================
    '''<summary>カット位置補正ボタン押下時処理(外部カメラ)【CHIP/NET用】</summary>
    '''<remarks>外部カメラのある場合のみ有効</remarks>
    '''=========================================================================
    Private Sub CmdCutPosCorrect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCutPosCorrect.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_CUTREV, APP_MODE_CUTREVIDE, MSG_OPLOG_CUT_POS_CORRECT_START, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCutPosCorrect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ENDﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ENDﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdEnd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdEnd.Click

        Dim strMSG As String
        Dim ret As Integer

        Try
            ' ローダへトリマ動作中信号を送信する
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ? ###035
                Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ローダ出力(ON=トリマ動作中, OFF=なし)
            Else
                Call SetLoaderIO(&H0, LOUT_STOP)                        ' ローダ出力(ON=なし, OFF=トリマ停止中)
            End If
            giAppMode = APP_MODE_EXIT                                   ' ｱﾌﾟﾘﾓｰﾄﾞ = 終了
            Timer1.Enabled = False                                      ' 監視タイマー停止
            Call ZCONRST()                                              ' ラッチ解除

            ' 終了確認メッセージ表示
            If (gCmpTrimDataFlg = 1) Then
                '  "編集中のデータがあります。" "アプリケーションを終了してよろしいですか？"
                ret = MsgBox(MSG_117 + vbCrLf + MSG_102,
                        MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground, gAppName)
            Else
                ' "アプリケーションを終了してよろしいですか？"
                ret = MsgBox(MSG_102,
                        MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground, gAppName)
            End If

            '----- V6.0.3.0_46↓ -----
            ' 統計表示オブジェクト
            If (IsNothing(gObjFrmDistribute) = False) Then
                GraphDispSet()
            End If
            '----- V6.0.3.0_46↓ -----

            ' 終了キャンセル処理
            If (ret = MsgBoxResult.No) Then                             ' 終了キャンセル ? 
                'e.Cancel = True                                        ' UserClosingイベントキャンセル
                giAppMode = APP_MODE_IDLE                               ' ｱﾌﾟﾘﾓｰﾄﾞ = トリマ装置アイドル中

                ' ローダへトリマ停止中信号を送信する
                If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then  ' SL432R系 ? ###035
                    Call SetLoaderIO(0, COM_STS_TRM_STATE)              ' ローダー出力(ON=なし,OFF=トリマ動作中)
                Else
                    Call SetLoaderIO(LOUT_STOP, &H0)                    ' ローダ出力(ON=トリマ停止中, OFF=なし)
                End If

                Timer1.Enabled = True                                   ' 監視タイマー開始
                Exit Sub
            End If

            ' ソフト終了時のステージ移動処理(オプション) V1.13.0.0⑩
            ret = AppEndStageMove(gdblStgOrgMoveX, gdblStgOrgMoveY)

            ' ソフト強制終了処理
            Call AplicationForcedEnding()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdEnd_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            End
        End Try

    End Sub
#End Region

#Region "ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理【NETのみ】"
    '''=========================================================================
    '''<summary>ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub CmdCircuitTeach_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCircuitTeach.Click

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_CIRCUIT, APP_MODE_CIRCUIT, MSG_OPLOG_FUNC30, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCircuitTeach_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        '        Dim r As Integer
        '        Dim mFrmObj As System.Windows.Forms.Form

        '        ' ローダー出力(ON=トリマ動作中,OFF=なし)
        '        Call ZATLDSET(COM_STS_TRM_STATE, 0)
        '        ' サーキットティーチングモードのセット
        '        giAppMode = APP_MODE_CIRCUIT

        '        ' コマンド実行前初期処理
        '        'r = CmdExec_Init(giAppMode, MSG_OPLOG_FUNC30)
        '        If (r <> cFRS_NORMAL) Then
        '            GoTo RETURNMENU
        '        End If

        '        ' ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞ処理
        '        mFrmObj = New frmCircuitTeach
        '        mFrmObj.ShowDialog()

        '        'Me.SetFocus()

        'RETURNMENU:
        '        ' コマンド実行後処理
        '        'Call CmdExec_Term()

        '        '　IDLEモード
        '        Call ZCONRST()                                      ' ラッチ解除
        '        Timer1.Enabled = True
        '        giAppMode = APP_MODE_IDLE                           ' ｱﾌﾟﾘﾓｰﾄﾞ = トリマ装置アイドル中

        '        ' ローダー出力(ON=なし,OFF=トリマ動作中)
        '        Call ZATLDSET(0, COM_STS_TRM_STATE)

    End Sub
#End Region

#Region "自動運転ボタン押下時処理"
    '''=========================================================================
    ''' <summary>自動運転ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SL436R/SL436S用
    '''          SL432R(特注)--ロット切替え機能有効時
    ''' </remarks>
    '''=========================================================================
    Private Sub CmdAutoOperation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAutoOperation.Click

        Dim r As Integer
        'V6.0.0.0⑪        Dim objForm As Object = Nothing
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   自動運転前処理
            '-------------------------------------------------------------------
            '----- V6.1.4.0①↓(KOA EW殿SL432RD対応) -----
            ' 自動運転実行(ロット切替え機能)
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then   ' SL432R系 ? 
                If (giLotChange = 1) Then                           ' ロット切替え機能有効 ?
                    LotChangeProc()                                 ' 自動運転実行(ロット切替え機能)
                End If
                Exit Sub
            End If
            '----- V6.1.4.0①↑ -----

            '統計表示処理の状態変更
            '###112　Call RedrawDisplayDistribution(True)

            ''V4.0.0.0-86
            r = GetLaserOffIO(True) 'V5.0.0.1⑫
            If r = 1 Then
                Exit Sub
            End If
            ''V4.0.0.0-86

            '----- V6.0.3.0_27↓ -----
            '----- V6.0.3.0_28↓ -----
            '' メッセージ表示(新規：STARTボタン、継続：RESETボタン押下待ち)
            'r = Sub_CallFrmMsgDisp(System1, cGMODE_QUEST_NEW_CONTINUE, cFRS_ERR_START, True, _
            '        "", "", "", System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)

            'If r = cFRS_ERR_START Then
            '    ' 新規を選択した場合 
            '' V6.0.3.0_30↓
            'If CbDigSwL.SelectedIndex <= TRIM_MODE_FT Then
            '    If QR_Read_Flg = 0 Then
            '        If btnQRLmit.BackColor = Color.Lime Then
            '            If gSysPrm.stTMN.giMsgTyp = 0 Then
            '                MsgBox("QRコードを読み込んでいないので自動運転を開始できません。")
            '            Else
            '                MsgBox("Cannot Auto Operation , Because not read Qr-Code. ")
            '            End If
            '            Return
            '        End If
            '    End If
            'End If

            '----- V6.0.3.0⑤↓(ローム殿特注) -----
            ' QRコードを読み込んだかチェック 
            r = ChkQRLimitStatus()
            If r <> cFRS_NORMAL Then
                Exit Sub
            End If
            ' V6.0.3.0_30↑
            '----- V6.0.3.0⑤↑ -----
            'Else
            '    ' 継続を選択した場合
            '    ' データロードチェック
            '    If (gLoadDTFlag = False) Then                            ' データ未ロード ?
            '        ' "トリミングパラメータデータをロードするか新規作成してください"
            '        Call System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
            '        Return                                    ' トリマエラー信号送信へ
            '    End If

            'End If
            '----- V6.0.3.0_28↑ -----
            '----- V6.0.3.0_27↑ -----

            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' 自動運転時はトリミング開始ブロック番号XYを1,1とする
            Call Set_StartBlkComb1St()
            Call Set_StartBlkNum_Enabled(False)
            '----- V4.11.0.0⑤↑ -----
            '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
            ' 基板投入ボタンの背景色を灰色にする
            gbChkSubstrateSet = False                                   ' 基板投入フラグOFF
            BtnSubstrateSet.Enabled = False                             '「基板投入ボタン」非活性化
            Call Set_SubstrateSetBtn(gbChkSubstrateSet)
            '----- V4.11.0.0⑥↑ -----

            ' トリマ装置状態を動作中に設定する
            Call Form1Button(0)                                         ' コマンドボタンを無効にする ###236
            r = TrimStateOn(F_AUTO, APP_MODE_AUTO, MSG_OPLOG_AUTO, "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' 編集中のデータを保存する
            If (gCmpTrimDataFlg = 1) Then                               ' データ更新フラグ = 1(更新あり)
                '"編集中のデータを保存しますか？"
                r = MsgBox(MSG_AUTO_19, MsgBoxStyle.OkCancel)
                If (r = MsgBoxResult.Ok) Then
                    ' データ保存
                    Call Me.CmdSave_Click(Me.CmdSave, New System.EventArgs())
                    giAppMode = APP_MODE_AUTO                           ' APP Modeを自動運転モードに戻す(Saveから帰ると0になるため) ###201
                    gCmpTrimDataFlg = 0                                 ' データ更新フラグ = 0(更新なし)
                End If
            End If

            '----- ###184↓ -----
            ' 載物台に基板がない事をチェックする
            ' ※この修正をSTARTキー押下待ち(Sub_CallFrmRset())の後に入れると何故かローダが
            '   供給マガジンの上昇を待たずに移動する(デバッグのためにBREAKしても同じ)
            r = Sub_SubstrateCheck(System1, APP_MODE_AUTO)
            If (r <= cFRS_ERR_EMG) Then                                 ' 異常終了レベルのエラー ? 
                GoTo STP_ERR                                            ' アプリケーション強制終了
            ElseIf (r = cFRS_ERR_RST) Then                              ' 基板有(RESETキー押下)ならReturn
                GoTo STP_END                                            ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            End If

            ' マガジン基板検出センサがOFFである事をチェックする
            r = Sub_SubstrateSensorOffCheck(System1)
            If (r <= cFRS_ERR_EMG) Then                                 ' 異常終了レベルのエラー ? 
                GoTo STP_ERR                                            ' アプリケーション強制終了
            ElseIf (r = cFRS_ERR_RST) Then                              ' センサらON(RESETキー押下)ならReturn
                GoTo STP_END                                            ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            End If
            '----- ###184↑ -----

            '----- ###197↓ ----- 
            ' NG排出BOXが満杯でない事をチェックする
            r = Sub_NgBoxCheck(System1, APP_MODE_AUTO)
            If (r <= cFRS_ERR_EMG) Then                                 ' 異常終了レベルのエラー ? 
                GoTo STP_ERR                                            ' アプリケーション強制終了
            ElseIf (r = cFRS_ERR_RST) Then                              ' 基板有(RESETキー押下)ならReturn
                GoTo STP_END                                            ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            End If
            '----- ###197↑ -----

            '-------------------------------------------------------------------
            '   自動運転のためのデータ選択画面を表示する
            '-------------------------------------------------------------------
            If (gsQRInfo(0) = "") Then                                  ' V1.18.0.0② 
                'V6.0.0.0⑪                objForm = New FormDataSelect()                          ' オブジェクト生成
                Dim objForm As New FormDataSelect()                     ' オブジェクト生成
                Call objForm.ShowDialog(Me)                             ' データ選択画面表示 'V6.0.0.0⑪
                r = objForm.sGetReturn()                                ' Return値取得(連続運転動作ﾓｰﾄﾞ(0:ﾏｶﾞｼﾞﾝﾓｰﾄﾞ 1:ﾛｯﾄﾓｰﾄﾞ 2:ｴﾝﾄﾞﾚｽﾓｰﾄﾞ))
                ' オブジェクト開放
                If (objForm Is Nothing = False) Then
                    Call objForm.Close()                                ' オブジェクト開放
                    Call objForm.Dispose()                              ' リソース開放
                End If
                Me.Refresh()                                            ' 再描画しないと次のSTARTキー押下待ちでも画面表示が残る 
                If (r = cFRS_ERR_RST) Then GoTo STP_END '               ' データ選択画面からCancelボタン押下ならトリマ装置状態をアイドル中に設定する(監視タイマー開始)

                '----- V1.18.0.0②↓ -----
            Else
                giAutoDataFileNum = 1
                ReDim gsAutoDataFileFullPath(giAutoDataFileNum - 1)
                gsAutoDataFileFullPath(0) = gStrTrimFileName
                giActMode = MODE_ENDLESS                                ' エンドレスモードを設定 V4.0.0.0-88
                '----- V1.18.0.1④↓ -----
                ' 生産管理データはクリアしない(ローム殿特注)
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ローム殿特注 ? 

                Else
                    Call ClearCounter(1)                                ' 生産管理データのクリア
                    Call ClrTrimPrnData()                               ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする
                End If
                'Call ClearCounter(1)                                    ' 生産管理データのクリア
                'Call ClrTrimPrnData()                                   ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする
                '----- V1.18.0.1④↑ -----
            End If
            '----- V1.18.0.0②↑ -----

            '-------------------------------------------------------------------
            '   自動運転開始のためのSTARTキー押下待ち
            '-------------------------------------------------------------------
            Call LAMP_CTRL(LAMP_START, True)                            ' STARTランプON
            Call LAMP_CTRL(LAMP_RESET, True)                            ' RESETランプON

            ''V5.0.0.1-23↓
            ' 各コマンド処理実行時のｽﾗｲﾄﾞｶﾊﾞｰ自動ｸﾛｰｽﾞ又はSTART/RESETキー押下待ち
            r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            Me.Refresh()
            If (r <= cFRS_ERR_EMG) Then                     ' ｴﾗｰ(非常停止等)ならｿﾌﾄ強制終了
            ElseIf (r = cFRS_ERR_RST) Then                              ' RESETキー押下ならReturn
                GoTo STP_END                                            ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            End If
            'r = Sub_CallFrmRset(System1, cGMODE_LDR_START)              ' STARTキー押下待ち画面表示
            'If (r <= cFRS_ERR_EMG) Then                                 ' 異常終了レベルのエラー ? 
            '    GoTo STP_ERR                                            ' アプリケーション強制終了
            'ElseIf (r = cFRS_ERR_RST) Then                              ' RESETキー押下ならReturn
            '    GoTo STP_END                                            ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            'End If
            ''V5.0.0.1-23↑

            '----- V4.0.0.0⑲↓ -----
            ' SL436S サイクル停止用(ローム殿特注)
            Call LAMP_CTRL(LAMP_HALT, False)                            ' HALTランプOFF
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            '----- V4.0.0.0⑲↑ -----
            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)する(ローム殿特注)
            r = EL_Lock_OnOff(EX_LOK_MD_ON)
            If (r = cFRS_TO_EXLOCK) Then r = cFRS_ERR_RST '             ' 前面扉ロックタイムアウトなら戻り値を「RESET」にする
            If (r <= cFRS_ERR_EMG) Then                                 ' 異常終了レベルのエラー ? 
                GoTo STP_ERR                                            ' アプリケーション強制終了
            ElseIf (r = cFRS_ERR_RST) Then                              ' RESETキー押下ならReturn
                GoTo STP_END                                            ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            End If
            '----- V1.18.0.1⑧↑ -----

            ' マガジン基板検出センサがOFFである事をチェックする
            r = Sub_SubstrateSensorOffCheck(System1)
            If (r <= cFRS_ERR_EMG) Then                                 ' 異常終了レベルのエラー ? 
                GoTo STP_ERR                                            ' アプリケーション強制終了
            ElseIf (r = cFRS_ERR_RST) Then                              ' センサらON(RESETキー押下)ならReturn
                GoTo STP_END                                            ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            End If

            '-------------------------------------------------------------------
            '   自動運転開始
            '-------------------------------------------------------------------
            gbFgAutoOperation = True                                    ' 自動運転フラグ(True:自動運転中)
            'Call SetSignalTower(0, 0)                                  ' シグナルタワー制御(手動運転OFF)
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETランプOFF '###160
            Timer1.Enabled = True                                       ' 監視タイマー開始
            stPRT_ROHM.bAutoMode = True                                 ' トリミング結果印刷データに自動運転を設定(ローム殿特注) V1.18.0.0③
            '----- V6.0.3.0_32↓(ローム殿特注) -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TimerQR.Enabled = False                                 ' 監視タイマー停止
            End If
            '----- V6.0.3.0_32↑ -----

            r = Start_Trimming()                                        ' 自動運転開始

            '----- V6.0.3.0_32↓(ローム殿特注) -----
            If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                TimerQR.Enabled = True                                  ' 監視タイマー開始
            End If
            '----- V6.0.3.0_32↑ -----
STP_END:
            '----- V1.18.0.1⑧↓ -----
            ' 電磁ロック(観音扉右側ロック)を解除する(ローム殿特注)
            r = EL_Lock_OnOff(EX_LOK_MD_OFF)
            If (r = cFRS_TO_EXLOCK) Then r = cFRS_ERR_RST '             ' 前面扉ロックタイムアウトなら戻り値を「RESET」にする
            If (r <= cFRS_ERR_EMG) Then                                 ' 異常終了レベルのエラー ? 
                GoTo STP_ERR                                            ' アプリケーション強制終了
            End If
            '----- V1.18.0.1⑧↑ -----
            '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
            ' 基板投入ボタンの背景色を灰色にする
            gbChkSubstrateSet = False                                   ' 基板投入フラグOFF
            BtnSubstrateSet.Enabled = False                             '「基板投入ボタン」非活性化
            Call Set_SubstrateSetBtn(gbChkSubstrateSet)
            '----- V4.11.0.0⑥↑ -----

            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
            'V5.0.0.9⑭ ↑ V6.0.3.0⑧

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
            giAppMode = APP_MODE_AUTO                                   ' アプリケーションモード再設定 
            Call TrimStateOff()                                         ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            'Call Form1Button(4)                                         ' 「LOAD」/「ﾛｷﾞﾝｸﾞ」/「ﾛｰﾀﾞ原点復帰」/「END」ﾎﾞﾀﾝのみ有効とする
            gbFgAutoOperation = False                                   ' 自動運転フラグOFF
            Call Form1Button(1)                                         ' コマンドボタンを有効にする ###236
            Exit Sub

STP_ERR:
            ' アプリケーション強制終了
            Call AppEndDataSave()
            Call AplicationForcedEnding()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdAutoOperation_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            gbFgAutoOperation = False                                   ' 自動運転フラグOFF
        End Try
    End Sub
#End Region
    '----- V6.1.4.0①↓(KOA EW殿SL432RD対応) -----
#Region "自動運転(ロット切替え機能)処理(特注)"
    '''=========================================================================
    ''' <summary>自動運転(ロット切替え機能)処理</summary>
    ''' <remarks>KOA EW特注版ではCmdAutoOperation_Click()の中でやっていたが
    '''          本来はSL436系用なので、SL432R用にCmdAutoOperation_Click()から分離した
    ''' </remarks>
    '''=========================================================================
    Private Sub LotChangeProc()

        Dim r As Integer
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   自動運転前処理
            '-------------------------------------------------------------------
            ' トリマ装置状態を動作中に設定する
            Call Form1Button(0)                                         ' コマンドボタンを無効にする
            r = TrimStateOn(F_AUTO, APP_MODE_AUTO, MSG_OPLOG_AUTO, "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' 編集中のデータを保存する
            If (gCmpTrimDataFlg = 1) Then                               ' データ更新フラグ = 1(更新あり)
                '"編集中のデータを保存しますか？"
                r = MsgBox(MSG_AUTO_19, MsgBoxStyle.OkCancel)
                If (r = MsgBoxResult.Ok) Then
                    ' データ保存
                    Call Me.CmdSave_Click(Me.CmdSave, New System.EventArgs())
                    giAppMode = APP_MODE_AUTO                           ' APP Modeを自動運転モードに戻す(Saveから帰ると0になるため)
                    gCmpTrimDataFlg = 0                                 ' データ更新フラグ = 0(更新なし)
                End If
            End If

            '-------------------------------------------------------------------
            '   自動運転のためのデータ選択画面を表示する
            '-------------------------------------------------------------------
            Call frmAutoObj.ShowDialog()                                ' データ選択画面表示
            r = frmAutoObj.sGetReturn()                                 ' Return値取得(連続運転動作ﾓｰﾄﾞ(0:ﾏｶﾞｼﾞﾝﾓｰﾄﾞ 1:ﾛｯﾄﾓｰﾄﾞ 2:ｴﾝﾄﾞﾚｽﾓｰﾄﾞ))
            Me.Refresh()                                                ' 再描画しないと次のSTARTキー押下待ちでも画面表示が残る 
            If (r = cFRS_ERR_RST) Then GoTo STP_END '                   ' データ選択画面からCancelボタン押下ならトリマ装置状態をアイドル中に設定する(監視タイマー開始)

            ' ローダ状態チェック(自動運転時),ローダが自動に切り替わるまで待つ
            ' "ローダ信号が手動です", "ローダを自動に切り替えてください", "Cancelボタン押下で処理を終了します"
            r = Me.System1.Form_Reset(cGMODE_LDR_CHK_AUTO, gSysPrm, giAppMode, gbInitialized, 0, 0, gLoadDTFlag)
            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then            ' エラー
                '----- V6.1.4.0①↓(Dllsystemでメッセージは表示済) -----
                'MsgBox("i-TKY::Form_Reset(cGMODE_LDR_MAGAGINE_EXCHG) error." + vbCrLf + " Err Code = " + r.ToString)
                If (r <= cFRS_ERR_EMG) Then                             ' 異常終了レベルのエラー ? 
                    GoTo STP_ERR                                        ' アプリケーション強制終了
                End If
            ElseIf (r = cFRS_ERR_RST) Then                              ' RESET(Cancelボタン)押下 ? 
                frmAutoObj.SetAutoOpeCancel()                           ' ローダ出力(ON=トリミングＮＧ, OFF=なし)トリミング不良信号を連続自動運転中止通知に使用
                frmAutoObj.gbFgAutoOperation432 = False                 ' 自動運転フラグ = 自動運転中でない
                frmAutoObj.AutoOperationEnd()                           ' 連続自動運転終了処理
                GoTo STP_END
            End If
            m_lTrimNgCount = 0                                          ' 連続トリミングＮＧ枚数カウンター V6.1.4.0⑨

            '-------------------------------------------------------------------
            '   自動運転開始(ローダからのSTART信号を待つ)
            '-------------------------------------------------------------------
            Call LAMP_CTRL(LAMP_START, False)                           ' STARTランプOFF
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETランプOFF
            Timer1.Enabled = True                                       ' 監視タイマー開始(TrimStateOff()↓でやってるけど元のソースに合わせた)
STP_END:
            ' 統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            '-------------------------------------------------------------------
            '   後処理
            '-------------------------------------------------------------------
            giAppMode = APP_MODE_AUTO                                   ' アプリケーションモード再設定 
            Call TrimStateOff()                                         ' トリマ装置状態をアイドル中に設定する(監視タイマー開始)
            Call Form1Button(1)                                         ' コマンドボタンを有効にする(TrimStateOff()↑でやってるけど)
            Exit Sub

STP_ERR:
            ' アプリケーション強制終了
            Call AppEndDataSave()
            Call AplicationForcedEnding()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.LotChangeProc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            gbFgAutoOperation = False                                   ' 自動運転フラグOFF
        End Try
    End Sub
#End Region
    '----- V6.1.4.0①↑ -----
#Region "ローダ原点復帰ボタン押下時処理"
    '''=========================================================================
    ''' <summary>ローダ原点復帰ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub CmdLoaderInit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdLoaderInit.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            'V4.11.0.0⑩
            gbInitialized = False                           ' 原点復帰未
            ' 各コマンド処理実行時のｽﾗｲﾄﾞｶﾊﾞｰ自動ｸﾛｰｽﾞ又はSTART/RESETキー押下待ち
            giAppMode = APP_MODE_LOADERINIT
            r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            Me.Refresh()
            If (r <= cFRS_ERR_EMG) Then                     ' ｴﾗｰ(非常停止等)ならｿﾌﾄ強制終了
                ''V5.0.0.1-23↓
                'GoTo STP_END
                GoTo STP_ERR
                ''V5.0.0.1-23↑
            ElseIf r = cFRS_ERR_RST Then
                GoTo STP_END
            End If
            'V4.11.0.0⑩



            '統計表示処理の状態変更
            Call Form1Button(0)                                     ' コマンドボタンを無効にする ###236
            Call RedrawDisplayDistribution(True)

            ' 初期処理
            r = TrimStateOn(F_LOADERINI, APP_MODE_LOADERINIT, MSG_OPLOG_LOADERINIT, "MANUAL")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            '----- ###185↓ ----- 
            ' 載物台に基板がある場合は、取り除かれるまで待つ
            r = SubstrateCheck(System1, APP_MODE_LOADERINIT)
            If (r <> cFRS_NORMAL) Then GoTo STP_END
            '----- ###185↑ ----- 

            '----- ###197↓ ----- 
            ' NG排出BOXが満杯の場合は、取り除かれるまで待つ(ローダが原点復帰できないため)
            r = NgBoxCheck(System1, APP_MODE_LOADERINIT)
            If (r < cFRS_NORMAL) Then
                If (r <> cFRS_NORMAL) Then GoTo STP_END
            End If
            '----- ###197↑ ----- 

            ' ローダ原点復帰処理(エラー発生時のメッセージは表示済)
            Call W_RESET()                                              ' アラームリセット信号送出 ###073
            Call W_START()                                              ' スタート信号送出 ###073


            r = Sub_CallFrmRset(System1, cGMODE_LDR_ORG)
            'V5.0.0.1-21↓
            If (r <= cFRS_ERR_EMG) Then                     ' ｴﾗｰ(非常停止等)ならｿﾌﾄ強制終了
                GoTo STP_ERR
            End If
            'V5.0.0.1-21↑
            ' コマンド実行後処理(異常終了レベルのエラー時はCmdExec_Term()でアプリ終了)
            Call CmdExec_Term(APP_MODE_LOADERINIT, r)

            ' 終了処理 
STP_END:

            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            Call TrimStateOff()                                         ' トリマ装置状態をアイドル中に設定する
            Call Form1Button(1)                                         ' コマンドボタンを有効にする ###236
            giAppMode = APP_MODE_IDLE            'V4.11.0.0⑩

            Exit Sub

STP_ERR:
            ' アプリケーション強制終了
            Call AppEndDataSave()
            Call AplicationForcedEnding()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdLoaderInit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "統合登録調整ボタン押下時処理"
    '''=========================================================================
    ''' <summary>統合登録調整ボタン押下時処理</summary>
    ''' <remarks>'V4.10.0.0③</remarks>
    '''=========================================================================
    Private Sub CmdIntegrated_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdIntegrated.Click
        Try
            gbIntegratedMode = True     'V4.10.0.0③ 一括ティーチング時はTrueになる変数
            gbCorrectDone = False       'V4.10.0.0③ 一括ティーチング時は、一度補正を実施した後は行わない。
            gbSubstExistChkDone = False 'V4.10.0.0③ オペレータティーチング簡素化 一度基板在荷チェックを行ったら後は実施しない為に使用
            ' コマンド実行
            Dim r As Short = CmdExec_Proc(F_INTEGRATED, APP_MODE_INTEGRATED, MSG_OPLOG_INTEGRATED, "") ' UNDONE: MSG_OPLOG_INTEGRATED ﾘｿｰｽ対応要

            gbCorrectDone = False       'V4.10.0.0③ 一括ティーチング時は、一度補正を実施した後は行わない。
            gbIntegratedMode = False    'V4.10.0.0③ 一括ティーチング時はTrueになる変数
            gbSubstExistChkDone = False 'V4.10.0.0③ オペレータティーチング簡素化 一度基板在荷チェックを行ったら後は実施しない為に使用

            ' トラップエラー発生時
        Catch ex As Exception
            Dim msg As String = "i-TKY.CmdIntegrated_Click() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, msg)
        Finally
            grpIntegrated.Visible = False
        End Try
    End Sub
#End Region
    '----- V1.13.0.0③↓ -----
#Region "オートプローブ登録ボタン押下時処理"
    '''=========================================================================
    ''' <summary>オートプローブ登録ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdAotoProbePtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_APROBEREC, APP_MODE_APROBEREC, MSG_OPLOG_APROBEREC, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdAotoProbePtn_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "オートプローブ実行ボタン押下時処理"
    '''=========================================================================
    ''' <summary>オートプローブ実行ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdAotoProbeCorrect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_APROBEEXE, APP_MODE_APROBEEXE, MSG_OPLOG_APROBEEXE, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdAotoProbeCorrect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "オートプローブ実行処理"
    '''=========================================================================
    ''' <summary>オートプローブ実行処理</summary>
    ''' <param name="pltInfo"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function AutoProbing(ByRef pltInfo As PlateInfo) As Short

        Dim r As Short
        Dim strMSG As String
        Dim parModules As MainModules

        Try
            parModules = New MainModules
            With pltInfo
                '--------------------------------------------------------------------------
                '   θ補正(ｵﾌﾟｼｮﾝ)実行(θ補正のみ実行し補正値(gfCorrectPosX, gfCorrectPosY)を求める)
                '--------------------------------------------------------------------------
                r = Move_Trimposition(pltInfo, 0.0, 0.0)        ' θ補正(ｵﾌﾟｼｮﾝ) & XYﾃｰﾌﾞﾙﾄﾘﾑ位置へは移動しない
                If (r <> cFRS_NORMAL) Then                      ' エラー ?
                    Return (r)                                  ' Return値設定
                End If

                '--------------------------------------------------------------------------
                '   オートプローブ実行
                '--------------------------------------------------------------------------
                Call SetAProbParam(pltInfo, parModules)         ' パラメータ設定 
                r = parModules.Sub_CallFrmMatrix()              ' プローブ調整
                If (r <> cFRS_NORMAL) Then                      ' エラー ?
                    Return (r)                                  ' Return値設定
                End If

                Return (cFRS_NORMAL)
            End With

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.AutoProbing() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                  ' Return値 = トラップエラー
        End Try

    End Function
#End Region

#Region "オートプローブ実行用パラメータ設定処理"
    '''=========================================================================
    ''' <summary>オートプローブ実行用パラメータ設定処理</summary>
    ''' <param name="pltInfo">   (INP)プレートデータ</param>
    ''' <param name="parModules">(INP)parModules</param>
    '''=========================================================================
    Private Sub SetAProbParam(ByRef pltInfo As PlateInfo, ByRef parModules As MainModules)

        Dim strMSG As String                                                ' メッセージ編集域

        Try
            With pltInfo
                parModules.stAPRB.intAProbeGroupNo1 = .intStepMeasLwGrpNo   ' パターン1(下方)用グループ番号
                parModules.stAPRB.intAProbePtnNo1 = .intStepMeasLwPtnNo     ' パターン1(下方)用パターン番号
                parModules.stAPRB.intAProbeGroupNo2 = .intStepMeasUpGrpNo   ' パターン2(上方)用グループ番号
                parModules.stAPRB.intAProbePtnNo2 = .intStepMeasUpPtnNo     ' パターン2(上方)用パターン番号
                parModules.stAPRB.dblAProbeBpPosX = .dblStepMeasBpPosX      ' パターン1(下方)用BP位置X
                parModules.stAPRB.dblAProbeBpPosY = .dblStepMeasBpPosY      ' パターン1(下方)用BP位置Y
                parModules.stAPRB.dblAProbeStgPosX = .dblStepMeasTblOstX    ' パターン2(上方)用ステージオフセット位置X
                parModules.stAPRB.dblAProbeStgPosY = .dblStepMeasTblOstY    ' パターン1(下方)用ステージオフセット位置Y
                parModules.stAPRB.intAProbeStepCount = .intStepMeasCnt      ' ステップ実行用ステップ回数
                parModules.stAPRB.intAProbeStepCount2 = .intStepMeasReptCnt ' ステップ実行用繰返しステップ回数
                parModules.stAPRB.dblAProbePitch = .dblStepMeasPitch        ' ステップ実行用ステップピッチ
                parModules.stAPRB.dblAProbePitch2 = .dblStepMeasReptPitch   ' ステップ実行用繰返しステップピッチ
            End With
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SetAProbParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ＩＤティーチングボタン押下時処理"
    '''=========================================================================
    ''' <summary>ＩＤティーチングボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdIDTeach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_IDTEACH, APP_MODE_IDTEACH, MSG_OPLOG_IDTEACH, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdIDTeach_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "伸縮補正(画像登録)ボタン押下時処理"
    '''=========================================================================
    ''' <summary>伸縮補正(画像登録)ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdSinsyukuPtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_SINSYUKU, APP_MODE_SINSYUKU, MSG_OPLOG_SINSYUKU, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdSinsyukuPtn_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.13.0.0③↑ -----
#Region "マップボタン押下時処理"
    '''=========================================================================
    ''' <summary>マップボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub CmdMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim r As Integer
        Dim strMSG As String

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            ' トリマ装置状態を動作中に設定する
            r = TrimStateOn(F_MAP, APP_MODE_MAP, MSG_OPLOG_MAP, "")
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' コンソールボタンのランプ状態を設定する
            Call LAMP_CTRL(LAMP_START, False)                       ' STARTﾗﾝﾌﾟ消灯 
            Call LAMP_CTRL(LAMP_RESET, False)                       ' RESETﾗﾝﾌﾟ消灯 

            Call Form1Button(0)                                     ' コマンドボタンを無効にする
            Call SetVisiblePropForVideo(False)

            'V6.0.0.0④            FormMapSelect.Show()
            Dim frm As New FormMapSelect    'V6.0.0.0④
            frm.Show()                      'V6.0.0.0④

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
STP_END:
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            ' Formを元に戻す
            'Me.WindowState = FormWindowState.Normal
            ' トリマ装置状態をアイドル中に設定する
            Call TrimStateOff()
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdEdit_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            GoTo STP_END
        End Try

    End Sub
#End Region
    'V4.10.0.0⑨↓
#Region "プローブクリーニング実行ボタン押下時処理"
    Private Sub CmdT_ProbeCleaning_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdT_ProbeCleaning.Click
        Dim strMSG As String
        Dim r As Short

        Try
            ' コマンド実行
            r = CmdExec_Proc(F_PROBE_CLEANING, APP_MODE_PROBE_CLEANING, MSG_OPLOG_MAINT, "")

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdT_ProbeCleaning_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    'V4.10.0.0⑨↑
    '----- V6.1.4.0⑥↓(KOA EW殿SL432RD対応) -----
#Region "ﾌｫﾙﾀﾞ表示ボタン押下時処理"
    '''=========================================================================
    '''<summary>ﾌｫﾙﾀﾞ表示ボタン押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub btnHistoryData_Click(sender As System.Object, e As System.EventArgs) Handles CmdFolderOpen.Click

        Try
            ' 生産管理ﾃﾞｰﾀﾛｸﾞ保存ﾌｫﾙﾀﾞをｴｸｽﾌﾟﾛｰﾗで開く
            Call frmAutoObj.ProductControlFolderOpen()

        Catch ex As Exception
            Dim strMsg As String = "i-TKY.btnHistoryData_Click() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMsg)
        End Try
    End Sub
#End Region
    '----- V6.1.4.0⑥↑ -----

    '========================================================================================
    '   各コマンド実行処理
    '========================================================================================
#Region "各コマンド実行制御処理"
    '''=========================================================================
    '''<summary>各コマンド実行制御処理</summary>
    '''<param name="IdxFnc">  (INP)機能選択定義テーブルのｲﾝﾃﾞｯｸｽ</param>
    '''<param name="iAppMode">(INP)ｱﾌﾟﾘﾓｰﾄﾞ(giAppMode参照)</param> 
    '''<param name="strLOG">  (INP)操作ﾛｸﾞﾒｯｾｰｼﾞ</param>
    '''<param name="strNote"> (INP)操作ﾛｸﾞｺﾒﾝﾄ</param>
    '''<returns>0=正常 ,0以外=エラー</returns>
    '''<remarks>非常停止/集塵機異常時は当関数内でｿﾌﾄ強制終了する</remarks>
    '''=========================================================================
    Public Function CmdExec_Proc(ByVal IdxFnc As Short, ByRef iAppMode As Short, ByRef strLOG As String, ByRef strNote As String,
                                 Optional ByVal isRough As Boolean = False) As Short                'V5.0.0.9④
        'Public Function CmdExec_Proc(ByVal IdxFnc As Short, ByRef iAppMode As Short, ByRef strLOG As String, ByRef strNote As String) As Short

        Dim strMSG As String
        Dim r As Short
        Dim InterlockSts As Integer                                     ' ###062
        Dim SwitchSts As Long                                           ' ###062
        'V6.0.0.0⑪        Dim objForm As Object
        Dim objForm As Form                         'V6.0.0.0⑪
        'Dim ObjGazou As Process = Nothing

        Try
            '-----------------------------------------------------------------------------
            '   初期処理
            '-----------------------------------------------------------------------------
            ' コマンド実行前のチェック
            r = CmdExec_Check(iAppMode)
            If (r <> cFRS_NORMAL) Then                                  ' チェックエラー ?
                '// 'V4.0.0.0-90 
                CommandEnableSet(True)

                Return (r)                                              ' Return値 = チェックエラー(Cancel(RESETｷｰ)を返す)
            End If

            ' 統計表示処理の状態変更
            Call RedrawDisplayDistribution(True)

            ''V5.0.0.1④↓
            If giNgStop = 1 Then
                btnJudgeEnable(False)
            End If
            ''V5.0.0.1④↑
            ' トリマ装置状態を動作中に設定する
            objForm = Nothing
            r = TrimStateOn(IdxFnc, iAppMode, strLOG, strNote)
            If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' >>> V3.1.0.0① 2014/11/28
            ' データ表示を消すか判断する
            If IsDataDisplayCheck(iAppMode) = True Then
                ' データ表示を非表示にする
                Call SetDataDisplayOff()
            End If
            ' <<< V3.1.0.0① 2014/11/28

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call SimpleTrimmer.Sub_StartResetButtonDispOff()
            End If
#End If
            ' コマンド実行前処理
            r = CmdExec_Init(iAppMode)
            If (r = cFRS_ERR_RST) Then                                  ' Cancel(RESETｷｰ) ?
                If (gSysPrm.stSPF.giWithStartSw = 0) Then               ' ｽﾀｰﾄSW押下待ち(ｵﾌﾟｼｮﾝ)でない ?
                    GoTo STP_END
                Else                                                    ' ｽﾀｰﾄSW押下待ち(ｵﾌﾟｼｮﾝ)なら
                    GoTo STP_TRM                                        ' ｽﾗｲﾄﾞｶﾊﾞｰｸﾛｽﾞしてREADY状態へ
                End If
            End If

            ' エラーチェック
            If (r < cFRS_NORMAL) Then                                   ' エラー ?
                ' カバー開検出(ｽﾀｰﾄSW押下待ち(ｵﾌﾟｼｮﾝ)時発生する)
                If (r = cFRS_ERR_CVR) Or (r = cFRS_ERR_SCVR) Or (r = cFRS_ERR_LATCH) Then
                    r = cFRS_NORMAL
                    GoTo STP_TRM                                        ' カバー開検出ならｽﾗｲﾄﾞｶﾊﾞｰｸﾛｽﾞしてREADY状態へ
                Else
                    GoTo STP_END
                End If
            End If

            '-----------------------------------------------------------------------------
            '   画像表示プログラムを起動する
            '-----------------------------------------------------------------------------
            ' ティーチングコマンドの場合(カットトレース用)
            'V3.0.0.0③            If (iAppMode = APP_MODE_TEACH) Then
            'r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0) '###052
            'V3.0.0.0③            Else
            ''Videoの表示を無効にする。
            'Call Me.VideoLibrary1.VideoStop()
            'Me.VideoLibrary1.Visible = False
            'V3.0.0.0③            End If

            '' ティーチングコマンド他の場合
            'If (iAppMode = APP_MODE_TEACH) Or (iAppMode = APP_MODE_LASER) _
            '    Or (iAppMode = APP_MODE_PROBE) Or (iAppMode = APP_MODE_TX) _
            '    Or (iAppMode = APP_MODE_TY) Or (iAppMode = APP_MODE_TY2) _
            '    Or (iAppMode = APP_MODE_CIRCUIT) Then
            '    '' ''r = Exec_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)
            'Else
            '    ''Videoの表示を無効にする。
            '    'Call Me.VideoLibrary1.VideoStop()
            '    'Me.VideoLibrary1.Visible = False
            'End If

            '-----------------------------------------------------------------------------
            '   ランプ点灯
            '-----------------------------------------------------------------------------
            If ((iAppMode = APP_MODE_TTHETA) Or (iAppMode = APP_MODE_TX) Or
                (iAppMode = APP_MODE_TY) Or (iAppMode = APP_MODE_TY2) Or
                (iAppMode = APP_MODE_CARIB) Or (iAppMode = APP_MODE_CUTREVIDE)) Then
                'START/RESETランプを点灯
                Call LAMP_CTRL(LAMP_START, True)
                Call LAMP_CTRL(LAMP_RESET, True)
            End If

            ' シグナルタワー黄点灯(ティーチング中) ###062
            ' ※但しインターロック解除中(黄点滅)優先
            r = INTERLOCK_CHECK(InterlockSts, SwitchSts)
            If (InterlockSts = INTERLOCK_STS_DISABLE_NO) Then           ' インターロック中なら黄点灯
                ' シグナルタワー制御(On=ティーチング中,Off=全ﾋﾞｯﾄ) ###007
                Select Case (gSysPrm.stIOC.giSignalTower)
                    Case SIGTOWR_NORMAL                                 ' 標準(黄点灯(但し黄点滅優先)) ###024 
                        'V5.0.0.9⑭ ↓ V6.0.3.0⑧(ローム殿仕様は黄点灯)
                        ' Call Me.System1.SetSignalTower(SIGOUT_YLW_ON, &HFFFF)
                        Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_TEACHING)
                        'V5.0.0.9⑭ ↑ V6.0.3.0⑧
                    Case SIGTOWR_SPCIAL                                 ' 特注(なし)
                End Select
            End If

            '' ''スタートスイッチラッチクリア   20121001 M.KAMI ADD
            ' ''Call ZCONRST()
            If gKeiTyp = KEY_TYPE_RS Then
                If ChkVideoChangeMode(iAppMode) Then
                    SetTeachVideoSize() 'V2.0.0.0⑱
                End If
                GroupBoxEnableChange(False)
            End If

            '-----------------------------------------------------------------------------
            '   各コマンド実行処理
            '-----------------------------------------------------------------------------
            ' ========================================================= 'V4.10.0.0③  ↓
            Dim prmAppMode As Short = iAppMode
            Dim commands As New List(Of Short)
            Dim lblList As New List(Of Label)
            If (APP_MODE_INTEGRATED = iAppMode) Then
                ' 実行中ｺﾏﾝﾄﾞﾘｽﾄを表示する
                For Each lbl As Label In flpIntegrated.Controls
                    lbl.BackColor = SystemColors.Control
                Next
                grpIntegrated.BringToFront()
                grpIntegrated.Visible = True
                grpIntegrated.Refresh()

                If (0 <= stFNC(F_RECOG).iDEF) AndAlso (0 <> gSysPrm.stDEV.giTheta) Then
                    commands.Add(APP_MODE_RECOG) ' 画像登録
                    lblList.Add(Me.lblIntegRecog)
                End If
                If (0 <= stFNC(F_PROBE).iDEF) Then
                    commands.Add(APP_MODE_PROBE) ' ﾌﾟﾛｰﾌﾞ
                    lblList.Add(Me.lblIntegProbe)
                End If
                If (0 <= stFNC(F_TX).iDEF) Then
                    commands.Add(APP_MODE_TX)   ' BP位置調整
                    lblList.Add(Me.lblIntegTX)
                End If
                If (0 <= stFNC(F_TEACH).iDEF) Then
                    commands.Add(APP_MODE_TEACH) ' ﾃｨｰﾁﾝｸﾞ
                    lblList.Add(Me.lblIntegTeach)
                End If
                If (0 <= stFNC(F_TY).iDEF) Then
                    commands.Add(APP_MODE_TY)   ' ｽﾃｰｼﾞ位置調整
                    lblList.Add(Me.lblIntegTY)
                End If
            Else
                commands.Add(iAppMode)
            End If

            For i As Integer = 0 To (commands.Count - 1) Step 1         'V4.10.0.0③
                iAppMode = commands(i)

                If (APP_MODE_INTEGRATED = prmAppMode) Then
                    ' 実行中ｺﾏﾝﾄﾞの背景色
                    lblList(i).BackColor = Color.FromArgb(255, 255, 225)

                    If (1 <= i) Then
                        ' 実行済みｺﾏﾝﾄﾞの背景色
                        lblList(i - 1).BackColor = SystemColors.Control

                        ' オブジェクト開放
                        If (objForm IsNot Nothing) Then
                            objForm.Close()                             ' オブジェクト開放
                            objForm.Dispose()                           ' リソース開放
                        End If

                        ' lbl.Textを実行します
                        If (cFRS_ERR_START <> System1.TrmMsgBox(gSysPrm,
                            String.Format(iTKY_001, lblList(i).Text) & New String(" "c, 20), vbOKCancel, gAppName)) Then
                            ' ｷｬﾝｾﾙの場合ﾙｰﾌﾟ終了
                            Exit For
                        End If

                        ' コマンド実行前のチェック
                        r = CmdExec_Check(iAppMode)
                        If (cFRS_NORMAL <> r) Then                      ' チェックエラー ?
                            If (cERR_TRAP = r) Then
                                CommandEnableSet(True)
                                Return (r)                              ' 例外発生時は終了
                            Else
                                If (APP_MODE_TX = iAppMode) OrElse (APP_MODE_TY = iAppMode) Then
                                    ' ブロック数が１のためこのコマンドは実行できません！
                                    ' 抵抗数が１のためこのコマンドは実行できません！
                                    ' TODO: 終了か？
                                End If
                                Continue For ' TODO: 次の処理に進む？
                            End If
                        End If

                    End If
                End If
                ' ===================================================== 'V4.10.0.0③  ↑

                Select Case (iAppMode)
                    Case APP_MODE_LASER                                     ' レーザー調整画面処理(Laser.OCX)
                        If (gSysPrm.stRMC.giRmCtrl2 >= 2 And gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then     'V5.0.0.6⑧ RMCTRL2対応 ?
                            Call ATTRESET()                                                     'V1.25.0.0⑭
                            Call LATTSET(gSysPrm.stRAT.giAttFix, gSysPrm.stRAT.giAttRot)        'V1.25.0.0⑭
                        End If                                                                                  'V5.0.0.6⑧
                        SetLaserItemsVisible(0)                             ' レーザパワー調整関連項目を非表示とする ###029 
                        r = LaserProc(typPlateInfo)
                        SetLaserItemsVisible(1)                             ' レーザパワー調整関連項目を表示とする ###029 

                    Case APP_MODE_PROBE                                     ' プローブティーチング画面処理(Probe.OCX)
                        r = ProbeTeaching(typPlateInfo)

                    Case APP_MODE_TEACH                                     ' ティーチング画面処理(Teach.OCX)
                        r = Teaching(APP_MODE_TEACH, typPlateInfo)

                    Case APP_MODE_RECOG                                     ' θ補正用パターン登録画面処理(Video.OCX)
                        'r = PatternTeach(iAppMode, typPlateInfo)
                        r = PatternTeach(iAppMode, typPlateInfo, isRough)  'V5.0.0.9④

                    Case APP_MODE_CUTPOS                                    ' ｶｯﾄ位置補正用パターン登録画面処理(Video.OCX)
                        r = CutPosTeach(typPlateInfo)

                    Case APP_MODE_TTHETA                                    ' Ｔθ(θ角度補正)ティーチング
                        objForm = New frmTThetaTeach()
                        r = CmdCall_Proc(APP_MODE_TTHETA, objForm, 0, typPlateInfo)

                    Case APP_MODE_TX                                        ' TXティーチング
                        objForm = New frmTxTyTeach()
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)
                        'V5.0.0.6⑩↓キャンセルでなければ外部カメラのカット位置補正値を初期化する。
                        If giTeachpointUse = 1 And (r <> cFRS_ERR_RST) Then
                            DataAccess.ResetCutTeachPoint()             ' 外部カメラのカット位置補正値を初期化する。
                        End If
                        'V5.0.0.6⑩↑
                        'If (r = cFRS_TxTy) Then                             ' TX2押下 ?
                        If (r = cFRS_TxTy) AndAlso (APP_MODE_INTEGRATED <> prmAppMode) Then ' TX2押下 ?   'V4.10.0.0③
                            r = Teaching(APP_MODE_TEACH, typPlateInfo)
                        End If

                    Case APP_MODE_TY                                        ' TYティーチング
                        objForm = New frmTxTyTeach()
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)
                        If (r = cFRS_TxTy) Then                             ' TY2押下 ?
                            Call objForm.Close()                            ' オブジェクト開放
                            Call objForm.Dispose()                          ' リソース開放
                            objForm = New frmTy2Teach()                     ' TY2ティーチング実行
                            r = CmdCall_Proc(APP_MODE_TY2, objForm, 0, typPlateInfo)
                        End If

                    Case APP_MODE_TY2                                       ' TY2ティーチング
                        objForm = New frmTy2Teach()
                        r = CmdCall_Proc(APP_MODE_TY2, objForm, 0, typPlateInfo)

                    Case APP_MODE_EXCAM_R1TEACH                             ' 外部カメラR1ティーチング【外部カメラ】(Teach.OCX)
                        Call Set_Led(1, 1)                                  ' バックライト照明ON V1.20.0.0⑩
                        r = Teaching(APP_MODE_EXCAM_R1TEACH, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' バックライト照明OFF V1.20.0.0⑩

                    Case APP_MODE_EXCAM_TEACH                               ' 外部カメラティーチング【外部カメラ】(Teach.OCX)
                        Call Set_Led(1, 1)                                  ' バックライト照明ON V1.20.0.0⑩
                        r = Teaching(APP_MODE_EXCAM_TEACH, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' バックライト照明OFF V1.20.0.0⑩

                    Case APP_MODE_CARIB_REC                                 ' 画像登録(キャリブレーション補正用)【外部カメラ】(Video.OCX)
                        Call Set_Led(1, 1)                                  ' バックライト照明ON V1.20.0.0⑩
                        r = PatternTeach(iAppMode, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' バックライト照明OFF V1.20.0.0⑩

                    Case APP_MODE_CARIB                                     ' キャリブレーション実行【外部カメラ】
                        Call Set_Led(1, 1)                                  ' バックライト照明ON V1.20.0.0⑩
                        objForm = New FrmCalibration()
                        r = CmdCall_Proc(APP_MODE_CARIB, objForm, 0, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' バックライト照明OFF V1.20.0.0⑩

                    Case APP_MODE_CUTREVISE_REC                             ' 画像登録(カット位置補正用)【外部カメラ】(Video.OCX)
                        Call Set_Led(1, 1)                                  ' バックライト照明ON V1.20.0.0⑩
                        r = PatternTeach(iAppMode, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' バックライト照明OFF V1.20.0.0⑩

                    Case APP_MODE_CUTREVIDE                                 ' カット位置補正【外部カメラ】
                        Call Set_Led(1, 1)                                  ' バックライト照明ON V1.20.0.0⑩
                        objForm = New FrmCutPosCorrect()
                        r = CmdCall_Proc(APP_MODE_CUTREVIDE, objForm, 0, typPlateInfo)
                        Call Set_Led(1, 0)                                  ' バックライト照明OFF V1.20.0.0⑩

                    Case APP_MODE_CIRCUIT                                   ' サーキットティーチング
                        objForm = New frmCircuitTeach()
                        r = CmdCall_Proc(APP_MODE_CIRCUIT, objForm, 0, typPlateInfo)

                        '----- V1.13.0.0③↓ -----
                    Case APP_MODE_APROBEREC                                 ' 画像登録(オートプローブ用)【内部カメラ】(Video.OCX)
                        r = PatternTeach(iAppMode, typPlateInfo)

                    Case APP_MODE_APROBEEXE                                 ' オートプローブ実行
                        r = AutoProbing(typPlateInfo)

                    Case APP_MODE_SINSYUKU
                        r = PatternTeach(iAppMode, typPlateInfo)

                        '----- V1.13.0.0③↑ -----
                    Case APP_MODE_IDTEACH                                   'V1.13.0.0⑥ ＩＤリーダー実行
                        objForm = New frmIDReaderTeach()
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)

                    Case APP_MODE_MAINTENANCE                               ' メンテナンス
                        objForm = New frmMaintenance()
                        'V6.0.0.0⑪                        objForm.gdblCleaningPosX = 4  frmMaintenance にメンバなし
                        'V6.0.0.0⑪                        objForm.gdblCleaningPosY = 1  frmMaintenance にメンバなし
                        r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)

                    Case APP_MODE_PROBE_CLEANING                            ' プローブクリーニング
                        'objForm = New frmProbeCleaning()
                        'objForm.sSetInitVal()          'V6.0.0.0⑪ コンストラクタの引数で設定する
                        objForm = New frmProbeCleaning(
                            typPlateInfo.dblPrbCleanPosX, typPlateInfo.dblPrbCleanPosY) 'V6.0.0.0⑪

                        'V6.0.0.0⑪                        Call objForm.ShowDialog()
                        objForm.Show(Me)           'V6.0.0.0⑪
                        'V6.0.0.0⑪                        r = objForm.sGetReturn()                         ' Return値 = コマンド終了結果
                        r = (TryCast(objForm, ICommonMethods)).Execute()        ' Return値 = コマンド終了結果    'V6.0.0.0⑪

                        '                    r = CmdCall_Proc(iAppMode, objForm, 0, typPlateInfo)

                    Case Else
                        strMSG = "CmdExec_Proc() AppMode ERROR = " + iAppMode
                        MsgBox(strMSG)
                        r = -1 * ERR_CMD_PRM                                ' Return値 = パラメータエラー 
                        GoTo STP_TRM
                End Select

                ' ===================================================== 'V4.10.0.0③↓
                If (r < cFRS_NORMAL) Then Exit For ' ﾊｰﾄﾞｴﾗｰ
            Next i

            If (APP_MODE_INTEGRATED = prmAppMode) Then
                iAppMode = prmAppMode   ' TODO: 以降の終了処理をおこなうために別のﾓｰﾄﾞにする必要があるかも？
                giAppMode = prmAppMode
                grpIntegrated.Visible = False
            Else
                iAppMode = prmAppMode
            End If
            ' ========================================================= 'V4.10.0.0③↑

            '-----------------------------------------------------------------------------
            '   後処理
            '-----------------------------------------------------------------------------
            ' オブジェクト開放
            If (objForm Is Nothing = False) Then
                Call objForm.Close()                                ' オブジェクト開放
                Call objForm.Dispose()                              ' リソース開放
                objForm = Nothing                                       'V4.10.0.0③
                ' ガベージコレクションによる明示的な開放
                Call GC.Collect()
            End If

            If gKeiTyp = KEY_TYPE_RS Then
                If ChkVideoChangeMode(iAppMode) Then
                    SetSimpleVideoSize() 'V2.0.0.0⑱
                End If
                GroupBoxEnableChange(True)
            End If

            '-----------------------------------------------------------------------------
            '   ランプ消灯
            '-----------------------------------------------------------------------------
            If ((iAppMode = APP_MODE_TTHETA) Or (iAppMode = APP_MODE_TX) Or
                (iAppMode = APP_MODE_TY) Or (iAppMode = APP_MODE_TY2) Or
                (iAppMode = APP_MODE_CARIB) Or (iAppMode = APP_MODE_CUTREVIDE)) Then
                'START/RESETランプを点灯
                Call LAMP_CTRL(LAMP_START, False)
                Call LAMP_CTRL(LAMP_RESET, False)
            End If
            Call gparModules.CrossLineDispOff()                         'V5.0.0.6⑫クロスライン非表示
#If False Then                          'V6.0.0.0⑤
            ' 画像表示プログラムを終了する
            If (iAppMode = APP_MODE_TEACH) Then
                End_GazouProc(ObjGazou)                                 ' 画像表示プログラムを終了する
            Else
                'Videoの表示を有効にする。
                VideoLibrary1.Visible = True
                Call Me.VideoLibrary1.VideoStart()
            End If
#End If
            ' シグナルタワー制御(On=レディ, Off=全ﾋﾞｯﾄ) ###007
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(無点灯)
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                    ' Call Me.System1.SetSignalTower(0, &HEFFF)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧

                Case SIGTOWR_SPCIAL                                     ' 特注(なし)

            End Select

STP_TRM:
            ' コマンド実行後処理
            Call CmdExec_Term(iAppMode, r)

STP_END:
            ' >>> V3.1.0.0① 2014/11/28
            ' データ表示を表示にする
            Call SetDataDisplayOn()
            ' <<< V3.1.0.0① 2014/11/28

            '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
            ' バーコード情報(WALSIN)を表示
            'V5.0.0.9⑮            If (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
            If (BarcodeType.Walsin = BarCode_Data.Type) Then            'V5.0.0.9⑮
                Me.GrpQrCode.Visible = True
            End If
            '----- V4.11.0.0②↑ -----

            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            ' 終了処理
            Call TrimStateOff()                                         ' トリマ装置状態を動作中に設定する

            'V4.10.0.0③↓念の為ここでも設定
            gbCorrectDone = False                                       ' オペレータティーチング簡素化 一度ＸＹΘ補正を行ったら後は実施しない為に使用
            gbIntegratedMode = False                                    ' 現在一括ティーチングモードの時 True
            gbSubstExistChkDone = False                                 ' オペレータティーチング簡素化 一度基板在荷チェックを行ったら後は実施しない為に使用
            'V4.10.0.0③↑

            ''V5.0.0.1④↓
            If giNgStop = 1 Then
                btnJudgeEnable(True)
            End If
            ''V5.0.0.1④↑

            ' シグナルタワー制御(On=レディ, Off=全ﾋﾞｯﾄ) ###007
            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
            Select Case (gSysPrm.stIOC.giSignalTower)
                Case SIGTOWR_NORMAL                                     ' 標準(無点灯)
                    ' Call Me.System1.SetSignalTower(0, &HEFFF)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                Case SIGTOWR_SPCIAL                                     ' 特注(なし)

            End Select
            'V5.0.0.9⑭ ↑ V6.0.3.0⑧

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call SimpleTrimmer.Sub_StartResetButtonDispOn()
            End If
#End If
            Return (r)                                                  ' Return値設定 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)

        Finally
            SetActiveJogMethod(Nothing, Nothing, Nothing)               'V6.0.0.0⑪  通常は不要
            Me.txtLog.ScrollToCaret()
        End Try

    End Function
#End Region

#Region "各コマンド実行処理"
    ''' =========================================================================
    ''' <summary>各コマンド実行処理</summary>
    ''' <param name="iAppMode">(INP)ｱﾌﾟﾘﾓｰﾄﾞ(giAppMode参照)</param>
    ''' <param name="objForm"> (INP)Formオブジェクト</param>
    ''' <param name="prm">     (INP)引数(外部カメラティーチング用)</param>
    ''' <param name="pltInfo"> (I/O)プレートデータ</param>
    ''' <remarks></remarks>
    ''' =========================================================================
    Private Function CmdCall_Proc(ByVal iAppMode As Short, ByVal objForm As ICommonMethods, ByVal prm As Integer, ByRef pltInfo As PlateInfo) As Short
        'V6.0.0.0⑪    Private Function CmdCall_Proc(ByRef iAppMode As Short, ByRef objForm As Object, ByRef prm As Integer, ByVal pltInfo As PlateInfo) As Short

        Dim strMSG As String
        Dim r As Short

        Try
            '--------------------------------------------------------------------------
            '   θ補正(ｵﾌﾟｼｮﾝ) & XYテーブルをトリム位置へ移動する
            '--------------------------------------------------------------------------
            CmdCall_Proc = cFRS_NORMAL                                  ' Return値 = 正常
            'Call BSIZE(stPLT.zsx, stPLT.zsy)                           ' ブロックサイズ設定
            r = Move_Trimposition(pltInfo, 0.0, 0.0)                    ' θ補正(ｵﾌﾟｼｮﾝ) & XYﾃｰﾌﾞﾙﾄﾘﾑ位置移動
            SetTeachVideoSize()                                         ' V6.0.3.0_49
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                CmdCall_Proc = r                                        ' Return値設定
                Exit Function
            End If

            ' BPｵﾌｾｯﾄ設定
            'Call System1.EX_BPOFF(gSysPrm, typPlateInfo.dblBpOffSetXDir, typPlateInfo.dblBpOffSetYDir)
            'If (r <> cFRS_NORMAL) Then GoTo STP_END

            ' スタートスイッチラッチクリア   ###082
            Call ZCONRST()

            '--------------------------------------------------------------------------
            '   各コマンド処理を実行する
            '--------------------------------------------------------------------------
            'V6.0.0.0⑦      APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH がくることはない
            ' 外部カメラR1ティーチング/外部カメラティーチング時は種別を引数として渡す
            'V6.0.0.0⑦            If (iAppMode = APP_MODE_EXCAM_R1TEACH) Or (iAppMode = APP_MODE_EXCAM_TEACH) Then
            'V6.0.0.0⑦Call objForm.ShowDialog(Me, prm)
            'V6.0.0.0⑦            Else
            'V6.0.0.0⑦            Call objForm.ShowDialog()
            'V6.0.0.0⑪            objForm.Show(Me)            'V6.0.0.0⑦
            TryCast(objForm, Form).Show(Me)                             'V6.0.0.0⑪
            'V6.0.0.0⑦            End If

            'V6.0.0.0⑪            CmdCall_Proc = objForm.sGetReturn()                         ' Return値 = コマンド終了結果
            'V6.0.0.0⑬            CmdCall_Proc = objForm.sGetReturn                           ' Return値 = コマンド終了結果    'V6.0.0.0⑪
            CmdCall_Proc = objForm.Execute()                            'V6.0.0.0⑬

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdCall_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            CmdCall_Proc = cERR_TRAP                                    ' Return値 = 例外エラー

        Finally                         'V6.0.0.0⑪
            SetActiveJogMethod(Nothing, Nothing, Nothing)               ' 通常は不要
        End Try

    End Function
#End Region

#Region "各コマンド実行前処理"
    '''=========================================================================
    '''<summary>各コマンド実行前処理</summary>
    '''<param name="iAppMode">(INP) ｱﾌﾟﾘﾓｰﾄﾞ(giAppMode参照)</param>
    ''' <returns>  0  = 正常
    '''            3  = Reset SW押下
    '''            上記以外のエラー
    ''' </returns>
    '''<remarks>非常停止/集塵機異常時は当関数内でｿﾌﾄ強制終了する</remarks>
    '''=========================================================================
    Private Function CmdExec_Init(ByRef iAppMode As Short) As Short

        Dim strMSG As String
        Dim r As Short

        Try
            '' コマンド実行前のチェック CmdExec_Proc()に移動
            'r = CmdExec_Check(iAppMode)
            'If (r <> cFRS_NORMAL) Then                      ' チェックエラー ?
            '    Return (r)                                  ' Return値 = チェックエラー(Cancel(RESETｷｰ)を返す)
            'End If

            gbInitialized = False                           ' 原点復帰未
            'V6.0.0.0⑩            Me.KeyPreview = False                           ' ﾌｧﾝｸｼｮﾝｷｰ無効
            Call Form1Button(0)                             ' コマンドボタンを無効にする
            Call SetVisiblePropForVideo(False)
            Me.GrpQrCode.Visible = False                    ' QRコード情報表示域非表示 
            Me.GrpStartBlk.Visible = False                  'トリミング開始ブロック番号非表示 V4.11.0.0⑤

            ' 集塵機異常チェック
            r = System1.CheckDustVaccumeAlarm(gSysPrm)
            If (r <> cFRS_NORMAL) Then                      ' エラーなら集塵機異常検出メッセージ表示
                Call System1.Form_Reset(cGMODE_ERR_DUST, gSysPrm, iAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                GoTo STP_ERR_EXIT                           ' ｿﾌﾄ強制終了
            End If

            If gbAutoCalibration Then                                   'V6.1.4.2①[自動キャリブレーション補正実行]
                r = cFRS_NORMAL                                         'V6.1.4.2①
            Else                                                        'V6.1.4.2①
                ' 各コマンド処理実行時のｽﾗｲﾄﾞｶﾊﾞｰ自動ｸﾛｰｽﾞ又はSTART/RESETキー押下待ち
                r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            End If
            Me.Refresh()
            If (r <= cFRS_ERR_EMG) Then                     ' ｴﾗｰ(非常停止等)ならｿﾌﾄ強制終了
                GoTo STP_ERR_EXIT
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Init() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = cERR_TRAP                                   ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

        Return (r)
        Exit Function

        ' ｿﾌﾄ強制終了処理
STP_ERR_EXIT:
        Call AppEndDataSave()                               ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
        Call AplicationForcedEnding()                       ' ｿﾌﾄ強制終了処理
        Return (r)
    End Function
#End Region

#Region "各コマンド実行前のチェック処理"
    '''=========================================================================
    '''<summary>各コマンド実行前のチェック処理</summary>
    '''<param name="iAppMode">(INP) ｱﾌﾟﾘﾓｰﾄﾞ(giAppMode参照)</param>
    ''' <returns>  0  = 正常
    '''            3  = チェックエラー(Cancel(RESETｷｰ)を返す)
    '''            上記以外のエラー
    ''' </returns>
    '''=========================================================================
    Private Function CmdExec_Check(ByRef iAppMode As Short) As Short

        Dim bFlg As Boolean
        Dim Rn As Integer
        Dim r As Short
        Dim Gn As Short
        Dim RnBn As Short
        Dim DblChipSz As Double
        Dim strMSG As String = ""

        Try
            '// 'V4.0.0.0-90
            CommandEnableSet(False)

            '-------------------------------------------------------------------
            '   コマンド実行前のチェックを行う
            '-------------------------------------------------------------------
            '----- V1.13.0.0③↓ -----
            ' 実行前のチェックでエラーになるとクランプONのままとなるので最後に移動

            ''----- ###240↓-----
            '' 載物台に基板がある事をチェックする(手動モード時(OPTION))
            'If (iAppMode <> APP_MODE_LASER) Then                    ' レーザコマンドはNO CHECK 
            '    giAppMode = iAppMode                                ' ###256  
            '    r = SubstrateExistCheck(System1)
            '    If (r <> cFRS_NORMAL) Then                          ' エラー ? 
            '        If (r = cFRS_ERR_RST) Then                      ' 基板無し(Cancel(RESETｷｰ)　?
            '            giAppMode = APP_MODE_IDLE
            '            Timer1.Enabled = True                       ' ###256 監視タイマー開始
            '            Return (cFRS_ERR_RST)                       ' Return値 = チェックエラー(Cancel(RESETｷｰ)を返す)
            '        End If
            '        Call AppEndDataSave()                           ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
            '        Call AplicationForcedEnding()                   ' ｿﾌﾄ強制終了処理
            '        Return (r)
            '    End If
            'End If
            ''----- ###240↑-----
            '----- VV1.13.0.0③↑ -----
            '----- V3.0.0.0④(V1.22.0.0③)↓ -----
            ' データロード済みチェック
            If (iAppMode <> APP_MODE_LASER) Then                        ' レーザコマンドはNO CHECK 
                If ChkTrimDataLoaded() <> cFRS_NORMAL Then
                    Return (cFRS_ERR_RST)                               ' Return値 = チェックエラー(Cancel(RESETｷｰ)を返す)
                End If
            End If
            '----- V3.0.0.0④(V1.22.0.0③)↑ -----
            '----- V1.19.0.0-33↓ -----
            ' プローブコマンド時
            If (iAppMode = APP_MODE_PROBE) Then
                ' 有効な抵抗番号(1-999)があるかチェックする
                r = CheckValidRegistance(APP_MODE_PROBE)                ' 有効な抵抗番号(1-999)なし ? 
                If (r <> cFRS_NORMAL) Then
                    strMSG = ERR_PROB_EXE                               ' "有効な抵抗(1-999)データがないためこのコマンドは実行できません！"
                    GoTo STP_ERR_EXIT                                   ' メッセージ表示後エラー戻り
                End If
            End If

            ' ティーチングコマンド時
            If (iAppMode = APP_MODE_TEACH) Then
                ' 有効な抵抗番号(1-999),又はマーキング抵抗があるかチェックする
                r = CheckValidRegistance(APP_MODE_TEACH)
                If (r <> cFRS_NORMAL) Then                              ' 有効な抵抗番号(1-999),又はマーキング抵抗なし ? 
                    strMSG = ERR_TEACH_EXE                              ' "有効な抵抗(1-999)又はマーキング抵抗データがないためこのコマンドは実行できません！"
                    GoTo STP_ERR_EXIT                                   ' メッセージ表示後エラー戻り
                End If
            End If
            '----- V1.19.0.0-33↑ -----
            '----- V2.0.0.0④↓ -----
            ' 加工範囲のチェックを行う
            If (iAppMode = APP_MODE_PROBE) Or (iAppMode = APP_MODE_TEACH) Then
                ' 加工範囲のチェック
                r = Bp_Limit_Check(False, strMSG)
                If (r <> cFRS_NORMAL) Then
                    GoTo STP_ERR_EXIT                                   ' メッセージ表示後エラー戻り
                End If
            End If
            '----- V2.0.0.0④↑ -----

            ' カット位置補正(TKY用)コマンド時
            If (iAppMode = APP_MODE_CUTPOS) Then
                ' カット位置補正対象の抵抗があるかチェックする
                bFlg = False
                For Rn = 1 To gRegistorCnt                          ' 抵抗数分処理する
                    ' カット位置補正する抵抗 ?
                    If (typResistorInfoArray(Rn).intCutReviseMode = 1) Then
                        bFlg = True
                        Exit For
                    End If
                Next Rn

                ' カット位置補正対象の抵抗がない場合は処理しない
                If (bFlg = False) Then
                    strMSG = MSG_39                                 ' "カット位置補正対象の抵抗がありません"
                    GoTo STP_ERR_EXIT                               ' メッセージ表示後エラー戻り
                End If
            End If

            ' Tθコマンド時
            If (iAppMode = APP_MODE_TTHETA) Then
                ' θ補正なしの場合は処理しない
                If typPlateInfo.intReviseMode = 1 And typPlateInfo.intManualReviseType = 0 Then
                    strMSG = MSG_118                                ' "補正モードが無しに設定されている場合は、実行できません。"
                    GoTo STP_ERR_EXIT                               ' メッセージ表示後エラー戻り
                End If
            End If

            ' ＴＸ/ＴＹコマンド時
            If (iAppMode = APP_MODE_TX) Or (iAppMode = APP_MODE_TY) Then
                ' 抵抗数またはブロック数を取得する
                r = GetChipNumAndSize(iAppMode, Gn, RnBn, DblChipSz)

                ' ＴＸコマンドで抵抗数が｢1｣の場合は処理できない為、エラーとする
                If (iAppMode = APP_MODE_TX) And (RnBn <= 1) Then
                    strMSG = ERR_TXNUM_E                            ' "抵抗数が１のためこのコマンドは実行できません！"
                    GoTo STP_ERR_EXIT                               ' メッセージ表示後エラー戻り
                End If

                ' ＴＹコマンドでブロック数が｢1｣の場合は処理できない為、エラーとする
                If (iAppMode = APP_MODE_TY) And (RnBn <= 1) Then
                    strMSG = ERR_TXNUM_B                            ' "ブロック数が１のためこのコマンドは実行できません！"
                    GoTo STP_ERR_EXIT                               ' メッセージ表示後エラー戻り
                End If
            End If

            ' サーキットティーチコマンド時(NET用)
            If (iAppMode = APP_MODE_CIRCUIT) Then
                ' サーキット数 > 1であるかチェックする
                If (typPlateInfo.intCircuitCntInBlock < 2) Then
                    strMSG = ERR_TXNUM_C                            ' "サーキット数が１のためこのコマンドは実行できません！"
                    GoTo STP_ERR_EXIT                               ' メッセージ表示後エラー戻り
                End If
            End If

            '----- V1.13.0.0③↓ -----
            ' オートプローブ実行/画像登録コマンド時
            If (iAppMode = APP_MODE_APROBEEXE) Or (iAppMode = APP_MODE_APROBEREC) Then
                ' ステップ測定回数 > 1であるかチェックする
                If (typPlateInfo.intStepMeasCnt < 1) Then
                    strMSG = ERR_TXNUM_S                            ' "ステップ測定回数が0のためこのコマンドは実行できません！"
                    GoTo STP_ERR_EXIT                               ' メッセージ表示後エラー戻り
                End If
            End If
            '----- V1.13.0.0③↑ -----

            '----- V1.13.0.0③↓ -----
            ' 載物台に基板がある事をチェックする(手動モード時(OPTION))
            '            If (iAppMode <> APP_MODE_LASER) Then                    ' レーザコマンドはNO CHECK 
            If ((iAppMode <> APP_MODE_LASER) And (iAppMode <> APP_MODE_MAINTENANCE)) Then                    ' レーザコマンドはNO CHECK (IOメンテナンスもチェックしない)
                giAppMode = iAppMode                                ' ###256  
                'V6.1.2.0③ Return (cFRS_NORMAL)                                'V4.10.0.0③
                If Not gbSubstExistChkDone Then                     'V4.10.0.0③

                    r = SubstrateExistCheck(System1)
                    If (r <> cFRS_NORMAL) Then                          ' エラー ? 
                        If (r = cFRS_ERR_RST) Then                      ' 基板無し(Cancel(RESETｷｰ)　?
                            giAppMode = APP_MODE_IDLE
                            Timer1.Enabled = True                       ' ###256 監視タイマー開始
                            Return (cFRS_ERR_RST)                       ' Return値 = チェックエラー(Cancel(RESETｷｰ)を返す)
                        End If
                        Call AppEndDataSave()                           ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                        Call AplicationForcedEnding()                   ' ｿﾌﾄ強制終了処理
                        Return (r)
                    End If
                    'V4.10.0.0③↓
                    If gbIntegratedMode Then                                ' 現在一括ティーチングモードの時 True
                        gbSubstExistChkDone = True                                ' オペレータティーチング簡素化 一度ＸＹΘ補正を行ったら後は実施しない為に使用
                    End If
                    'V4.10.0.0③↑
                End If                              'V4.10.0.0③
            End If
            '----- VV1.13.0.0③↑ -----

            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            '-------------------------------------------------------------------
            '   メッセージ表示後エラー戻り
            '-------------------------------------------------------------------
STP_ERR_EXIT:
            '----- ###235↓ -----
            'MsgBox(strMSG, MsgBoxStyle.Exclamation)
            giAppMode = APP_MODE_LOAD                               ' アイドルモードだとSTARTキーでトリミング実行するのでLOADモードとする 
            Call System1.TrmMsgBox(gSysPrm, strMSG, MsgBoxStyle.OkOnly, TITLE_4)
            Call ZCONRST()                                          ' ラッチ解除
            giAppMode = APP_MODE_IDLE
            Return (cFRS_ERR_RST)                                   ' Return値 = チェックエラー(Cancel(RESETｷｰ)を返す)
            '----- ###235↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Check() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "各コマンド実行後処理"
    '''=========================================================================
    '''<summary>各コマンド実行後処理</summary>
    '''<param name="iAppMode">(INP) ｱﾌﾟﾘﾓｰﾄﾞ(giAppMode参照)</param>
    '''<param name="sts">     (INP) コマンド実行ステータス(エラー番号)</param> 
    '''<remarks>STSが非常停止/集塵機異常時は当関数内でｿﾌﾄ強制終了する</remarks>
    '''=========================================================================
    Private Sub CmdExec_Term(ByRef iAppMode As Short, ByRef sts As Short)

        Dim strMSG As String
        Dim r As Short

        Try
            ' 各コマンドの実行ステータスをチェックする
            'frmInfo.Visible = True                          ' 結果表示域表示
            If (sts < cFRS_NORMAL) Then                     ' コマンド実行エラー ?
                If (sts = cFRS_ERR_PTN) Then                ' パターン認識エラー ?

                ElseIf (sts < cFRS_NORMAL) Then             ' ｿﾌﾄ強制終了ﾚﾍﾞﾙのｴﾗｰ ?
                    ' クランプ/吸着OFF
                    If (iAppMode = APP_MODE_LOADERINIT) Then GoTo STP_ERR_EXIT
                    r = System1.ClampVacume_Ctrl(gSysPrm, 0, iAppMode, giTrimErr)
                    If (r <> cFRS_ERR_EMG) Then
                        strMSG = "CmdExec_Term::ClampError occured" + vbCrLf + "Error code=" + r.ToString
                    End If
                    GoTo STP_ERR_EXIT                       ' ｿﾌﾄ強制終了
                End If
            End If

            ' テーブル原点移動
            If (iAppMode = APP_MODE_LOADERINIT) Then Exit Sub
            r = System1.Form_Reset(cGMODE_ORG_MOVE, gSysPrm, iAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
            If (r <= cFRS_ERR_EMG) Then
                strMSG = "CmdExec_Term::System1.Form_Reset" + vbCrLf + "Error code=" + r.ToString
                GoTo STP_ERR_EXIT ' エラーならﾌﾄ強制終了
            End If

            ' θを原点に戻す(giThetaParamは「位置補正モード=1(手動)で補正方法=0(補正)なし」時有効)
            '   giThetaParam = (0=θを原点に戻さない(標準), 1=θを原点に戻す)
            '   ・位置補正モード=0(自動),2(自動+微調)の時は無条件にθを原点に戻す
            '   ・位置補正モード=1(手動)で, 補正方法=1(1回のみ)は原点に戻さない
            '                                        2(毎回)は原点に戻す ###233
            If (gSysPrm.stDEV.giTheta <> 0) Then            ' θ有り ?
                If (typPlateInfo.intReviseMode = 0) Or (typPlateInfo.intReviseMode = 2) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1) Or
                   ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 2))) Then
                    '###233 ((typPlateInfo.intReviseMode = 1) And (typPlateInfo.intManualReviseType = 0) And (gSysPrm.stSPF.giThetaParam = 1)) Then
                    Call ROUND4(0.0#)                       ' θを原点に戻す
                    '----- V1.19.0.0-32   ↓ -----
                    gfCorrectPosX = 0.0                     ' 補正値初期化 
                    gfCorrectPosY = 0.0
                    '----- V1.19.0.0-32   ↑ -----
                End If
            End If

            ' SL432系の場合の処理
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) Then
                ''----- V1.13.0.0⑧↓ -----  V2.0.0.0⑧(OcxSystemで吸収)
                '' 第二原点位置にステージを移動する
                'If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                '    r = System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                '    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' エラーならソフト強制終了
                'End If
                ''----- V1.13.0.0⑧↑ -----

                If gbAutoCalibration Then                                   'V6.1.4.2①[自動キャリブレーション補正実行]
                Else                                                        'V6.1.4.2①
                    'スライドカバー自動オープン
                    If (gSysPrm.stSPF.giWithStartSw = 0) Then       ' スタートSW押下待ち（オプション）でない ?
                        r = System1.Z_COPEN(gSysPrm, iAppMode, giTrimErr, False)
                        If (r <= cFRS_ERR_EMG) Then GoTo STP_ERR_EXIT ' エラーならソフト強制終了
                    Else
                        ' インターロック時ならスライドカバー開待ち
                        If (System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0 Then
                            r = System1.Form_Reset(cGMODE_OPT_END, gSysPrm, iAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                            If (r <= cFRS_ERR_EMG) Then GoTo STP_ERR_EXIT ' エラーならﾌﾄ強制終了
                        End If
                    End If
                End If                                                      'V6.1.4.2①

                '----- V2.0.0.0⑧↓ -----
            Else
                ' SL436S/SL436Rの場合も第二原点位置にステージを移動する
                If (gdblStg2ndOrgX <> 0) Or (gdblStg2ndOrgY <> 0) Then
                    r = System1.EX_SMOVE2(gSysPrm, gdblStg2ndOrgX, gdblStg2ndOrgY)
                    If (r < cFRS_NORMAL) Then GoTo STP_ERR_EXIT ' エラーならソフト強制終了
                End If
                '----- V2.0.0.0⑧↑ -----
            End If

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.CmdExec_Term() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Exit Sub

        ' ｿﾌﾄ強制終了処理
STP_ERR_EXIT:
        '----- ###163↓ -----
        ' ローダアラームはｿﾌﾄ強制終了しない
        If (sts = cFRS_ERR_LDR) Or (sts = cFRS_ERR_LDR1) Or (sts = cFRS_ERR_LDR2) Or (sts = cFRS_ERR_LDR3) Or (sts = cFRS_ERR_LDRTO) Then
            Exit Sub
        End If
        '----- ###163↑ -----
        Call AppEndDataSave()                           ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
        Call AplicationForcedEnding()                   ' ｿﾌﾄ強制終了処理
    End Sub
#End Region

#Region "トリマ装置状態を動作中に設定する"
    '''=========================================================================
    '''<summary>トリマ装置状態を動作中に設定する</summary>
    '''<param name="IdxFnc">  (INP)機能選択定義テーブルのｲﾝﾃﾞｯｸｽ</param>
    '''<param name="iAppMode">(INP)ｱﾌﾟﾘﾓｰﾄﾞ(giAppMode参照)</param> 
    '''<param name="strLOG">  (INP)操作ﾛｸﾞﾒｯｾｰｼﾞ</param>
    '''<param name="strNote"> (INP)操作ﾛｸﾞｺﾒﾝﾄ</param>
    '''<returns>0=正常 ,0以外=エラー</returns>
    '''=========================================================================
    Public Function TrimStateOn(ByVal IdxFnc As Short, ByRef iAppMode As Short,
                                ByRef strLOG As String, ByRef strNote As String,
                                Optional ByVal shiftKeyDown As Boolean = False) As Short    '#4.12.2.0⑤
        '#4.12.2.0⑤    Public Function TrimStateOn(ByVal IdxFnc As Short, ByRef iAppMode As Short, ByRef strLOG As String, ByRef strNote As String) As Short

        Dim r As Short
        Dim rslt As Short
        Dim strMSG As String
        Dim stPos As System.Drawing.Point

        Try
            '// 'V4.0.0.0-90
            CommandEnableSet(False)

            ' ローダへトリマ動作中信号を送信する
            TrimStateOn = cFRS_NORMAL                                   ' Return値 = 正常
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ? ###035
                Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ローダ出力(ON=トリマ動作中, OFF=なし)
            Else
                Call SetLoaderIO(&H0, LOUT_STOP)                        ' ローダ出力(ON=なし, OFF=トリマ停止中)
            End If
            giAppMode = iAppMode                                        ' ｱﾌﾟﾘﾓｰﾄﾞをｸﾞﾛｰﾊﾞﾙ変数に設定

            ' データ編集中またはロード済チェック
            If (giAppMode = APP_MODE_LOGGING) OrElse
                (giAppMode = APP_MODE_LASER) OrElse
                (giAppMode = APP_MODE_AUTO) OrElse
                (giAppMode = APP_MODE_LOADERINIT) Then                  ' ロギング, レーザ, 自動運転, ローダ原点復帰コマンドの場合はチェックなし
                '#4.12.2.0⑤    (giAppMode = APP_MODE_EDIT) Then

            ElseIf (giAppMode = APP_MODE_MAINTENANCE) Then              ' V3.1.0.0① 2014/11/28 メンテナンスでトリミングデータロードしないようにする。

            ElseIf (giAppMode = APP_MODE_LOAD) Then                     ' ロードコマンドの場合 
                If gCmpTrimDataFlg = 1 Then                             ' 現在のデータが更新されているかチェックする
                    If (gLoadDTFlag = True) Then
                        ' "編集中のデータがあります。ロードしますか？"　　　　　
                        r = MsgBox(MSG_115, MsgBoxStyle.OkCancel, MSG_116)
                    Else
                        ' "ファイバレーザの加工条件を変更している可能性があります。ロードしますか？"　　　　　
                        r = MsgBox(MSG_149, MsgBoxStyle.OkCancel, MSG_116)
                    End If
                    If (r = MsgBoxResult.Cancel) Then                   ' Cansel指定 ?
                        TrimStateOn = cFRS_ERR_RST
                        GoTo STP_END
                    End If
                End If
                '#4.12.2.0⑤             ↓
            ElseIf (APP_MODE_EDIT = giAppMode) Then                     ' 編集画面
                If (shiftKeyDown) Then
                    ' shiftキー押下による新規作成編集画面起動の場合
                    If gCmpTrimDataFlg = 1 Then                         ' 現在のデータが更新されているかチェックする
                        If (gLoadDTFlag = True) Then
                            ' "編集中のデータがあります。新規にデータを作成しますか？"
                            r = MsgBox(MSG_110, MsgBoxStyle.OkCancel, MSG_109)
                        Else
                            ' "ファイバレーザの加工条件を変更している可能性があります。新規にデータを作成しますか？"
                            r = MsgBox(MSG_111, MsgBoxStyle.OkCancel, MSG_109)
                        End If
                        If (r = MsgBoxResult.Cancel) Then               ' Cansel指定 ?
                            TrimStateOn = cFRS_ERR_RST
                            GoTo STP_END
                        End If
                    End If
                End If
                '#4.12.2.0⑤             ↑
            Else                                                        ' その他のコマンドの場合
                If (gLoadDTFlag = False) Then                           ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ未ﾛｰﾄﾞ ?
                    ' "トリミングパラメータデータをロードするか新規作成してください"
                    Call System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    TrimStateOn = cFRS_ERR_RST
                    GoTo STP_END
                End If
            End If

            ' パスワード入力(オプション)
            rslt = Func_Password(IdxFnc)
            If (rslt <> True) Then
                '// 'V4.1.0.0⑧
                CommandEnableSet(True)
                TrimStateOn = cFRS_ERR_RST
                GoTo STP_END                                            ' ﾊﾟｽﾜｰﾄﾞ入力ｴﾗｰならEXIT
            End If

            SetMapOnOffButtonEnabled(False)                             'V4.12.2.0①
            SetTrimMapVisible(False)                                    'V4.12.2.0①

            ' クランプ/吸着ON(ﾛｰﾄﾞ/ｾｰﾌﾞｺﾏﾝﾄﾞ等以外) '###001
            ' V3.1.0.0① 2014/11/28 メンテナンスの時はClampVacume_Ctrl関数をコールしない。（条件追加）
            If (giAppMode <> APP_MODE_LOAD) And (giAppMode <> APP_MODE_SAVE) And (giAppMode <> APP_MODE_EDIT) And (giAppMode <> APP_MODE_LOGGING) And
               (giAppMode <> APP_MODE_AUTO) And (giAppMode <> APP_MODE_LOADERINIT) And (giAppMode <> APP_MODE_MAP) And
               (giAppMode <> APP_MODE_MAINTENANCE) Then     ' 'V1.13.0.0⑤

                'V4.5.0.1①↓
                '薄基板対応の場合には、クランプシーケンス変更する 
                'V4.5.0.1②↓
                '                If (giClampSeq = 1) And (giAppMode = APP_MODE_LASER) Then
                If (giClampSeq = 1) And (giAppMode <> APP_MODE_LASER) Then
                    'V4.5.0.1②↑

                    Dim lData As Long = 0
                    Dim lBit As Long = 0
                    Dim rtn As Integer = cFRS_NORMAL
                    Dim strMS2 As String = ""
                    Dim strMS3 As String = ""
                    Dim bFlg As Boolean = True
RETRY:
                    ' 載物台に基板がある事をチェックする
                    'クランプON
                    ''----- V1.16.0.0⑫↓ -----
                    'If (gSysPrm.stIOC.giClamp = 1) Then
                    '    Call W_CLMP_ONOFF(1)                                    ' クランプON
                    '    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                    '    Call W_CLMP_ONOFF(0)                                    ' クランプOFF
                    'End If
                    '----- V1.16.0.0⑫↑ -----
                    '吸着ON
                    Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
                    System.Threading.Thread.Sleep(500)                          ' Wait(ms) ※200msだとワーク有が検出されない場合がある
                    If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0⑥
                        ' SL436S時
                        r = W_Read(LOFS_W42S, lData)                            ' 物理入力状態取得(W42.00-W42.15)
                        lBit = LDSTS_VACUME
                    Else
                        ' SL436R時
                        r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
                        lBit = LDST_VACUME
                    End If
                    ' ワーク無しならメッセージ表示
                    If (lData And lBit) Then                                    ' 載物台に基板有 ? V2.0.0.0⑥
                        r = 0
                    Else
                        ' 「固定カバー開１チェックなし」にする
                        Call COVERCHK_ONOFF(COVER_CHECK_OFF)
                        'クランプ開、吸着OFF
                        r = System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)

                        ' 表示メッセージを設定する
                        strMSG = MSG_LDALARM_06                                 ' "載物台吸着ミス"
                        strMS2 = MSG_SPRASH52                                   ' "基板を置いて再度実行して下さい"
                        strMS3 = MSG_SPRASH53                                             ' ""

                        'r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        '        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        ' メッセージ表示(STARTキー押下待ち)
                        r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                                strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        If r = 1 Then ' STARTはRetry
                            If (gSysPrm.stIOC.giClamp = 1) Then
                                Call W_CLMP_ONOFF(1)                                    ' クランプON
                                System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                                Call W_CLMP_ONOFF(0)                                    ' クランプOFF
                            End If

                            GoTo RETRY
                        ElseIf r = 3 Then
                            ' Cancelが押されたら終了
                            TrimStateOn = cFRS_ERR_RST
                            GoTo STP_END
                        End If
                    End If
                Else
                    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)     'V5.0.0.8⑥
                    r = System1.ClampVacume_Ctrl(gSysPrm, 1, giAppMode, 1)
                    TrimStateOn = r
                End If
                'V4.5.0.1①↑

                'V6.0.0.0⑥                  ↓
                If (VideoLibrary1.SetTrackBarVisible(True)) Then
                    Dim sz As New Size(0, 30)
                    'V6.0.1.0⑪                    frmHistoryData.Location = Point.Add(frmHistoryData.Location, sz)
                    frmHistoryData.Location = Point.Add(HistoryDataLocation, sz)    'V6.0.1.0⑪
                    grpIntegrated.Location = Point.Add(grpIntegrated.Location, sz)
                End If
                'V6.0.0.0⑥                  ↑

            End If

            ' 操作ログ出力 V1.13.0.0③
            If gbAutoCalibration = False Then                          'V6.1.4.2①[自動キャリブレーション補正実行]でない時
                Call System1.OperationLogging(gSysPrm, strLOG, "")
            End If                                                      'V6.1.4.2①

            '----- V1.18.0.1①↓ -----
            ' 画面にコマンド名を表示する
            If (giDspCmdName = 1) Then
                Select Case (IdxFnc)
                    ' オプションコマンドの場合
                    Case F_EXR1                                             ' 外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞ
                        LblComandName.Text = MSG_F_EXR1
                    Case F_EXTEACH                                          ' 外部ｶﾒﾗﾃｨｰﾁﾝｸﾞ
                        LblComandName.Text = MSG_F_EXTEACH
                    Case F_CARREC                                           ' ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録
                        LblComandName.Text = MSG_F_CARREC
                    Case F_CAR                                              ' ｷｬﾘﾌﾞﾚｰｼｮﾝ
                        LblComandName.Text = MSG_F_CAR
                    Case F_CUTREC                                           ' 外部ｶﾒﾗｶｯﾄ位置補正登録
                        LblComandName.Text = MSG_F_CUTREC
                    Case F_CUTREV                                           ' 外部ｶﾒﾗｶｯﾄ位置補正
                        LblComandName.Text = MSG_F_CUTREV

                        ' 上記以外はコマンドのボタン名を表示
                    Case Else
                        LblComandName.Text = strBTN(IdxFnc) + MSG_OPLOG_CMD ' 例) "プローブ" + "コマンド"
                End Select
                stPos.X = 800
                stPos.Y = 0
                LblComandName.Location = stPos
                LblComandName.Visible = True
                LblComandName.BringToFront()
            Else
                LblComandName.Text = ""
                LblComandName.Visible = False
            End If
            '----- V1.18.0.1①↑ -----

            Exit Function

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.TrimStateOn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            TrimStateOn = cERR_TRAP                         ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

STP_END:
    End Function

#End Region

#Region "トリマ装置状態をアイドル中に設定する"
    '''=========================================================================
    '''<summary>トリマ装置状態をアイドル中に設定する</summary>
    '''<returns>0=正常 ,0以外=エラー</returns>
    '''=========================================================================
    Public Function TrimStateOff() As Short

        Dim strMSG As String
        Dim SndDat As UShort
        Dim r As Short

        Try
            'V6.1.4.2①[自動キャリブレーション補正実行]↓
            If gbAutoCalibration Then
                Call SetVisiblePropForVideo(True)
                'V6.1.4.14④↓
                If (VideoLibrary1.SetTrackBarVisible(False)) Then
                    Dim sz As New Size(0, 30)
                    frmHistoryData.Location = Point.Subtract(frmHistoryData.Location, sz)
                    grpIntegrated.Location = Point.Subtract(grpIntegrated.Location, sz)
                End If
                'V6.1.4.14④↑
                Return (cFRS_NORMAL)
            End If
            'V6.1.4.2①↑

            '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応)【ＱＲコードリード機能】 -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                ' ＱＲ読み込み状態の初期化
                ObjQRCodeReader.ResetQRReadFlag()                       ' ＱＲコード未読み込み状態にリセット
            End If
            '----- V6.1.4.0_22↑ -----

            '//'V4.0.0.0-90
            CommandEnableSet(True)

            ' 初期処理
            TrimStateOff = cFRS_NORMAL                                  ' Return値 = 正常
            If (giAppMode = APP_MODE_LOAD) Or (giAppMode = APP_MODE_SAVE) Then
                '                                                       ' ﾛｰﾄﾞ/ｾｰﾌﾞｺﾏﾝﾄﾞ 
                Call SetMousePointer(Me, False)                         ' 砂時計解除(ﾏｳｽﾎﾟｲﾝﾀ)
            ElseIf (giAppMode = APP_MODE_EDIT) Then                     ' EDIT ｺﾏﾝﾄﾞ ?

            ElseIf (giAppMode = APP_MODE_AUTO) Then                     ' 自動運転 ｺﾏﾝﾄﾞ ?

            ElseIf (giAppMode = APP_MODE_LOADERINIT) Then               ' ローダ原点復帰 ｺﾏﾝﾄﾞ ? 

                'ElseIf (giAppMode <> APP_MODE_MAP) Then                ' MAP画面 V1.16.0.0④ V1.13.0.0⑤
            ElseIf (giAppMode = APP_MODE_MAP) Then                      ' MAP画面 V1.16.0.0④

            Else                                                        ' その他のｺﾏﾝﾄﾞ ?
                ' クランプ/吸着OFF(SL436R用)
                r = System1.ClampVacume_Ctrl(gSysPrm, 0, giAppMode, 1)
                '----- V1.16.0.0④↓ -----
                If (r < cFRS_NORMAL) Then                               ' ｿﾌﾄ強制終了ﾚﾍﾞﾙのｴﾗｰ ?
                    Call AppEndDataSave()                               ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                    Call AplicationForcedEnding()                       ' ｿﾌﾄ強制終了処理
                    Return (r)
                End If
                '----- V1.16.0.0④↑ -----
            End If

            ' コンソールボタンのランプ状態を設定する
            Call LAMP_CTRL(LAMP_START, False)                           ' STARTﾗﾝﾌﾟ消灯 
            Call LAMP_CTRL(LAMP_RESET, False)                           ' RESETﾗﾝﾌﾟ消灯 
            Call LAMP_CTRL(LAMP_Z, False)                               ' PRBﾗﾝﾌﾟOFF (INTRTMのプローブコマンドで制御する?)

            '----- V1.18.0.1①↓ -----
            ' 画面のコマンド名表示を消す
            LblComandName.Visible = False
            '----- V1.18.0.1①↑ -----

            ' メイン画面 ボタン表示状態の変更
            Call SetVisiblePropForVideo(True)
            If giHostMode = cHOSTcMODEcMANUAL Then
                Call Form1Button(1)                                     ' コマンドボタンを有効にする
            Else
                Call Form1Button(3)                                     ' ﾃｰﾀｾｰﾌﾞﾎﾞﾀﾝ(F2), ENDﾎﾞﾀﾝ(F10)のみ有効にする
            End If

            ' QRｺｰﾄﾞ情報/バーコード情報(太陽社)を表示
            'V5.0.0.9⑮            If (gSysPrm.stCTM.giSPECIAL = customROHM) Or (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then
            'V5.0.0.9⑳            If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.Taiyo = BarCode_Data.Type) Then   'V5.0.0.9⑮
            If (gSysPrm.stCTM.giSPECIAL = customROHM) OrElse (BarcodeType.None <> BarCode_Data.Type) Then   'V5.0.0.9⑳
                Me.GrpQrCode.Visible = True
            End If

            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' トリミング開始ブロック番号を表示する
            'V5.0.0.9⑯            If (giStartBlkAss = 1) Then                                 ' トリミング開始ブロック番号指定の有効 ?
            If (giStartBlkAss <> 0) Then                                ' トリミング開始ブロック番号指定の有効 ?  'V5.0.0.9⑯
                GrpStartBlk.Visible = True
            End If
            '----- V4.11.0.0⑤↑ -----

            'マトリックス表示画面を非表示とする
            Me.GrpMatrix.Visible = False

            ' トリマ装置アイドル中に設定する
            '' '' ''Call ExpansionOnOff(True)                           ' ログ画面拡大ﾎﾞﾀﾝ有効
            Call ZCONRST()                                              ' ｺﾝｿｰﾙｷｰ ﾗｯﾁ解除

            ' ローダへトリマ停止中信号を送信する
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ? ###035
                Call SetLoaderIO(&H0, COM_STS_TRM_STATE)                ' ローダ出力(ON=なし, OFF=トリマ動作中)

            Else                                                        ' SL436R
                If (gLoadDTFlag = True) Then                            ' データロード済ならトリマ部レディ信号ON 
                    'SndDat = LOUT_STOP + LOUT_REDY                     ' ローダ出力データ = トリマ停止中 + トリマ部レディ '###098
                    SndDat = LOUT_STOP                                  ' ローダ出力データ = トリマ停止中 + トリマ部レディ '###098
                Else
                    SndDat = LOUT_STOP                                  ' ローダ出力データ = トリマ停止中
                End If
                Call SetLoaderIO(SndDat, &H0)                           ' ローダ出力(ON=トリマ停止中(+ トリマ部レディ), OFF=なし)
            End If

            ' 監視タイマー開始(下記のコマンド以外)
            If (giAppMode <> APP_MODE_IDLE) And
               (giAppMode <> APP_MODE_LOAD) And
               (giAppMode <> APP_MODE_SAVE) And
               (giAppMode <> APP_MODE_LOGGING) Then
                Timer1.Enabled = True                                   ' 監視タイマー開始
            End If

            giAppMode = APP_MODE_IDLE                                   ' ｱﾌﾟﾘﾓｰﾄﾞ = トリマ装置アイドル中
            If (gLoadDTFlag = True) Then
                Call LAMP_CTRL(LAMP_START, True)                        ' STARTﾗﾝﾌﾟON 
            End If

            'V5.0.0.9⑭ ↓ V6.0.3.0⑧
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
            'V5.0.0.9⑭ ↑ V6.0.3.0⑧

            'V6.0.0.0⑥                  ↓
            If (VideoLibrary1.SetTrackBarVisible(False)) Then
                Dim sz As New Size(0, 30)
                frmHistoryData.Location = Point.Subtract(frmHistoryData.Location, sz)
                grpIntegrated.Location = Point.Subtract(grpIntegrated.Location, sz)
            End If
            'V6.0.0.0⑥                  ↑

            SetMapOnOffButtonEnabled(True)                              'V4.12.2.0①
            SetTrimMapVisible(_mapOn)                                   'V4.12.2.0①

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.TrimStateOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            TrimStateOff = cERR_TRAP                                    ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try

    End Function
#End Region

    '========================================================================================
    '   その他のボタン押下時の処理
    '========================================================================================
#Region "TRIMMINGボタン押下時処理"
    '''=========================================================================
    '''<summary>TRIMMINGボタン押下時処理</summary>
    '''<remarks>画面上よりTRIMMINGボタンの入力を監視し、Trimming実行する</remarks>
    '''=========================================================================
    Private Sub btnTrimming_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Try
            Dim r As Short

            'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "MANUAL")'###030

            ' メインの表示状態変更
            Call Form1Button(0)                             ' コマンドボタンを非活性化にする
            ' タイマー停止
            Timer1.Enabled = False

            ' SL432RでｽﾀｰﾄSW押下待ちしない場合はメッセージ表示する
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                '-----V3.0.0.0⑦(V1.22.0.0⑤) -----
                ' "注意！！！　スライドカバーが自動で閉じます。"(Red,Blue)
                'r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH32, &HFF, &HFF0000)

                ' メッセージ表示(STARTキー押下待ち)
                r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                        MSG_SPRASH31, MSG_SPRASH32, "", System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                If (r < cFRS_NORMAL) Then
                    Call AppEndDataSave()                               ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                    Call AplicationForcedEnding()                       ' ｿﾌﾄ強制終了処理
                    End                                                 ' アプリ強制終了
                End If
                '----- V3.0.0.0⑦(V1.22.0.0⑤) -----
            End If

            Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "START BUTTON")

            ' トリミング
            r = Start_Trimming()

            ' タイマー開始
            Timer1.Enabled = True

            'Call ZCONRST()
        Catch ex As Exception
            Dim strMSG As String
            strMSG = "i-TKY.btnTrimming_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub
#End Region

#Region "Versionボタン押下時処理"
    '''=========================================================================
    '''<summary>Versionボタン押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub mnuHelpAbout_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuHelpAbout.Click

        Dim r As Short
        Dim pstPRM As DllAbout.HelpVersion.HelpVer_PARAM                            ' バージョン情報表示関数用パラメータ(OCXで定義)
        Dim strVER(3) As String
        Dim strMSG As String
        Dim EqType As String 'V4.0.0.0-77

        Try
            '----- ###068(追加)ここから -----
            ' トリマ装置状態を動作中に設定する
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then      ' SL432R系 ?
                Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ローダ出力(ON=トリマ動作中, OFF=なし)
            Else
                Call SetLoaderIO(&H0, LOUT_STOP)                        ' ローダ出力(ON=なし, OFF=トリマ停止中)
            End If
            giAppMode = APP_MODE_LOGGING                                ' ｱﾌﾟﾘﾓｰﾄﾞを「Version表示」に設定(※ロギングモードを使用)

            ' ボタン等を非活性化する 
            GrpMode.Enabled = False                                     ' コンソールスイッチグループボックス非活性化
            tabCmd.Enabled = False                                      ' コマンドボタングループボックス非活性化
            mnuHelpAbout.Enabled = False                                ' Aboutボタン非活性化 
            CmdCnd.Enabled = False                                      ' EDITLOCKボタン非活性化
            cmdEsLog.Enabled = False                                    ' ESLogボタン非活性化
            CMdIX2Log.Enabled = False                                   ' IX2Logボタン非活性化
            BtnPrintOnOff.Enabled = False                               ' Print ON/OFFボタン非活性化  V1.18.0.0③
            BtnPrint.Enabled = False                                    ' Printボタン非活性化        V1.18.0.0③
            '----- ###068(追加)ここまで -----

            ' 構造体pstPRMの配列の初期化 ※配列の要素数はDllAbout.dllで定義と同じにする必要あり
            pstPRM.strTtl = New String(4) {}
            pstPRM.strModule = New String(20) {}
            pstPRM.strVer = New String(20) {}

            ' バージョン情報表示関数用パラメータを設定する
            pstPRM.iTtlNum = 3                                          ' タイトル文字列の数
            pstPRM.strTtl(0) = gAppName                                 ' アプリ名 

            ''V6.0.2.0⑦↓
            Dim strVersion = GetPrivateProfileString_S("TMENU", "VERSION_NAME", SYSPARAMPATH, "")
            If strVersion.ToString().Trim <> "" Then
                EqType = strVersion
            Else
                'V4.0.0.0-77↓
                If (giMachineKd = MACHINE_KD_RS) Then
                    EqType = MACHINE_TYPE_SL436S
                Else
                    EqType = gSysPrm.stTMN.gsKeimei
                End If
            End If
            ''V6.0.2.0⑦↑

            '            pstPRM.strTtl(1) = "LMP-" + gSysPrm.stTMN.gsKeimei + gSysPrm.stDEV.gsDevice_No + "-000 " + _
            pstPRM.strTtl(1) = "LMP-" + EqType + gSysPrm.stDEV.gsDevice_No + "-000 " +
                               My.Application.Info.Version.Major.ToString("0") & "." &
                               My.Application.Info.Version.Minor.ToString("0") & "." &
                               My.Application.Info.Version.Build.ToString("0") & "." &
                               My.Application.Info.Version.Revision.ToString("0")
            'V4.0.0.0-77↑
            pstPRM.strTtl(2) = "(c) TOWA LASERFRONT CORP."

            '----- V3.0.0.0②↓(Ocx→Dllに名称変更) -----
            pstPRM.iVerNum = 19                                         ' バージョン情報の数 ###266
            pstPRM.strModule(0) = "RT MODULE"                           ' 1."RT MODULE"
            pstPRM.strVer(0) = DLL_PATH + "INTRIM_SL432.rta"

            pstPRM.strModule(1) = "DllTrimFnc.dll"                      ' 2."DllTrimFnc.dll"
            pstPRM.strVer(1) = DLL_PATH + pstPRM.strModule(1)

            pstPRM.strModule(2) = "DllSysprm.dll"                       ' 3."DllSysprm.dll"
            pstPRM.strVer(2) = DLL_PATH + pstPRM.strModule(2)

            pstPRM.strModule(3) = "DllSystem.dll"                       ' 4."DllSystem.dll"
            pstPRM.strVer(3) = DLL_PATH + pstPRM.strModule(3)

            pstPRM.strModule(4) = "DllAbout.dll"                        ' 5."DllAbout.dll"
            pstPRM.strVer(4) = DLL_PATH + pstPRM.strModule(4)

            pstPRM.strModule(5) = "DllUtility.dll"                      ' 6."DllUtility.dll"
            pstPRM.strVer(5) = DLL_PATH + pstPRM.strModule(5)

            pstPRM.strModule(6) = "DllLaserTeach.dll"                ' 7."DllLaserTeach.dll"
            pstPRM.strVer(6) = DLL_PATH + pstPRM.strModule(6)

            pstPRM.strModule(7) = "DllManualTeach.dll"                  ' 8."DllManualTeach.dll"
            pstPRM.strVer(7) = DLL_PATH + pstPRM.strModule(7)

            pstPRM.strModule(8) = "DllPassword.dll"                     ' 9.DllPassword.dll"
            pstPRM.strVer(8) = DLL_PATH + pstPRM.strModule(8)

            pstPRM.strModule(9) = "DllProbeTeach.dll"                '10."DllProbeTeach.dll"
            pstPRM.strVer(9) = DLL_PATH + pstPRM.strModule(9)

            pstPRM.strModule(10) = "DllTeach.dll"                       '11."DllTeach.dll"
            pstPRM.strVer(10) = DLL_PATH + pstPRM.strModule(10)

            pstPRM.strModule(11) = "DllVideo.dll"                       '12."DllVideo.dll"
            pstPRM.strVer(11) = DLL_PATH + pstPRM.strModule(11)

            pstPRM.strModule(12) = "DllJog.dll"                         '13."DllJog.dll" ###067
            pstPRM.strVer(12) = DLL_PATH + pstPRM.strModule(12)

            'Call Rs232c_GetVersion(strVER)                             ' 新Dll(C#で作成) 
            pstPRM.strModule(13) = "DllSerialIO.dll"                    '14."DllSerialIO.dll"
            pstPRM.strVer(13) = DLL_PATH + pstPRM.strModule(13)

            pstPRM.strModule(14) = "DllCndXMLIO.dll"                    '15."DllCndXMLIO.dll"
            pstPRM.strVer(14) = DLL_PATH + pstPRM.strModule(14)

            pstPRM.strModule(15) = "DllFLCom.dll"                       '16."DllFLCom.dll"
            pstPRM.strVer(15) = DLL_PATH + pstPRM.strModule(15)
            '----- V2.0.0.0_27↓ -----
            If (giMachineKd = MACHINE_KD_RS) Then
                pstPRM.strModule(16) = "TrimDataEditorEx"               '17."TrimDataEditorEx.exe"
                pstPRM.strVer(16) = DLL_PATH + "TrimDataEditorEx.exe"
            Else
                pstPRM.strModule(16) = "TrimDataEditor"                 '17."TrimDataEditor.exe" ###067
                pstPRM.strVer(16) = DLL_PATH + "TrimDataEditor.exe"
            End If
            '----- V2.0.0.0_27↑ -----
            pstPRM.strModule(17) = "FLSetup"                            '18."FLSetup.exe" ###067
            pstPRM.strVer(17) = DLL_PATH + "FLSetup.exe"

            '----- ###266↓ -----
            pstPRM.strModule(18) = "DllTkyMsgGet.dll"                   '19."DllTkyMsgGet.dll"
            pstPRM.strVer(18) = DLL_PATH + pstPRM.strModule(18)
            '----- ###266↑ -----
            '----- V3.0.0.0②↑ -----

            ' バージョン情報表示位置を設定する
            HelpVersion1.Left = Text4.Location.X                        ' Left = Text4位置 
            HelpVersion1.Top = mnuHelpAbout.Location.Y                  ' Top  = Versionﾎﾞﾀﾝ位置 

            ' バージョン情報表示
            HelpVersion1.Visible = True
            HelpVersion1.BringToFront()                                 ' 最前面へ表示

            r = HelpVersion1.Version_Disp(pstPRM)
            HelpVersion1.Visible = False

            '----- ###068(追加)ここから -----
            ' 終了処理
            Call TrimStateOff()                                         ' トリマ装置状態をアイドル中に設定する

            ' ボタン等を活性化する 
            GrpMode.Enabled = True                                      ' コンソールスイッチグループボックス活性化
            tabCmd.Enabled = True                                       ' コマンドボタングループボックス活性化
            mnuHelpAbout.Enabled = True                                 ' Aboutボタン活性化 
            CmdCnd.Enabled = True                                       ' EDITLOCKボタン活性化
            cmdEsLog.Enabled = True                                     ' ESLogボタン活性化
            CMdIX2Log.Enabled = True                                    ' IX2Logボタン活性化
            BtnPrintOnOff.Enabled = True                               ' Print ON/OFFボタン活性化  V1.18.0.0③
            BtnPrint.Enabled = True                                    ' Printボタン活性化        V1.18.0.0③
            '----- ###068(追加)ここまで -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.mnuHelpAbout_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "CLRボタン押下時処理"
    '''=========================================================================
    '''<summary>CLRボタン押下時処理</summary>
    '''<remarks>生産履歴ﾘｾｯﾄ</remarks>
    '''=========================================================================
    Private Sub btnCounterClear_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnCounterClear.Click

        Dim r As Short
        Try

            r = MsgBox(MSG_108, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)          ' 累計をクリアしてもよろしいですか？
            If (r = MsgBoxResult.Yes) Then
                Call System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
                Call ClearCounter(1)                        ' 生産管理データのクリア
                Call ClrTrimPrnData()                       ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする

                '統計表示がONの場合、表示を更新する
                If chkDistributeOnOff.Checked = True Then
                    gObjFrmDistribute.RedrawGraph()
                End If
                'Call ClearAvgDevCount()                    ' ###198 ###154 
            End If
            ''V4.12.2.2⑥↓'V6.0.2.0④
            GraphDispSet()
            ''V4.12.2.2⑥↑'V6.0.2.0④

        Catch ex As Exception

        End Try

    End Sub
#End Region

#Region "ES Logﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ES Logﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdEsLog_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEsLog.Click

        If gESLog_flg = True Then                       ' ON->OFF
            gESLog_flg = False                          ' ESログOFF 
            cmdEsLog.Text = "ESLog OFF"
            cmdEsLog.BackColor = System.Drawing.SystemColors.Control
        Else                                            ' OFF->ON
            gESLog_flg = True                           ' ESログON
            cmdEsLog.Text = "ESLog ON"
            cmdEsLog.BackColor = System.Drawing.Color.Lime
        End If

    End Sub

#End Region

#Region "IX2 Log Logﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>IX2 Logﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks>IX2 Logﾎﾞﾀﾝを「IX2Log ON」又は「IX2Log OFF」に切替える</remarks>
    '''=========================================================================
    Private Sub CMdIX2Log_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        If gSysPrm.stDEV.rIX2Log_flg = True Then
            ' ON->OFF
            gSysPrm.stDEV.rIX2Log_flg = False                           ' IX2Log OFF 
            CMdIX2Log.Text = "IX2Log OFF"
            CMdIX2Log.BackColor = System.Drawing.ColorTranslator.FromOle(&H8000000F)
        Else
            ' OFF->ON
            gSysPrm.stDEV.rIX2Log_flg = True                            ' IX2Log ON
            CMdIX2Log.Text = "IX2Log ON"
            CMdIX2Log.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF00)
        End If
    End Sub
#End Region

#Region "EDIT LOCK/UNLOCKﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>EDIT LOCK/UNLOCKﾎﾞﾀﾝ押下時処理</summary>
    '''<remarks>オプション機能</remarks>
    '''=========================================================================
    Private Sub cmdEditLock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdCnd.Click

        Dim r As Short
        Dim strMSG As String = ""                                       ' V2.0.0.0_25

        '　V1.14.0.0① Add ↓
        If gSysPrm.stDEV.sEditLock_flg = True Then
            ' UNLOCK->LOCK
            gSysPrm.stDEV.sEditLock_flg = False
            CmdCnd.Text = "EDIT LOCK"
            CmdCnd.BackColor = System.Drawing.SystemColors.Control
            giPassWord_Lock = 1
        Else
            ' LOCK->UNLOCK
            ' データ編集パスワードチェック
            r = Password1.ShowDialog((gSysPrm.stTMN.giMsgTyp), (gSysPrm.stSYP.gstrPassword))
            If (r = cFRS_ERR_START) Then                  ' OK 
                gSysPrm.stDEV.sEditLock_flg = True
                CmdCnd.Text = "EDIT UNLOCK"
                CmdCnd.BackColor = System.Drawing.Color.Lime
                giPassWord_Lock = 0
            End If
        End If
        '　V1.14.0.0① ↑

        '----- V2.0.0.0_25↓ -----
        ' SL436Sの場合はデータ編集と「EDIT LOCK/UNLOCKﾎﾞﾀﾝ」をリンクさせる為シスパラ(EDIT LOCK状態)を更新する
        If (giMachineKd = MACHINE_KD_RS) Then
            If (gSysPrm.stDEV.sEditLock_flg = False) Then               ' sEditLock_flg(編集ロック(0=する, 1=しない))
                strMSG = "0"                                            ' 「EDIT LOCK」
            Else
                strMSG = "1"                                            ' 「EDIT UNLOCK」
            End If
            Call WritePrivateProfileString("DEVICE_CONST", "EDITLOCK", strMSG, SYSPARAMPATH)
        End If
        '----- V2.0.0.0_25↑ -----

    End Sub
#End Region

#Region "ADJﾎﾞﾀﾝ押下時処理"
    '''=========================================================================
    '''<summary>ADJﾎﾞﾀﾝ押下時処理 ###009</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub BtnADJ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnADJ.Click

        Dim strMSG As String

        Try
            If (BtnADJ.Text = "ADJ OFF") Then
                gbChkboxHalt = True
                BtnADJ.Text = "ADJ ON"
                BtnADJ.BackColor = System.Drawing.Color.Yellow
                'V5.0.0.1②↓
                If (gKeiTyp = KEY_TYPE_RS) Then
                    If (gbFgAutoOperation = True) Then
                        ''V5.0.0.1-22                        If giAppMode = APP_MODE_FINEADJ And bAllMagazineFinFlag = False  Then
                        If giAppMode = APP_MODE_FINEADJ And bAllMagazineFinFlag = False And gbChkSubstrateSet = False Then 'V5.0.0.1-22
                            BtnSubstrateSet.Enabled = True
                        End If
                    End If
                End If
                'V5.0.0.1②↑
            Else
                gbChkboxHalt = False
                BtnADJ.Text = "ADJ OFF"
                BtnADJ.BackColor = System.Drawing.SystemColors.Control
                'V5.0.0.1②↓
                If (gKeiTyp = KEY_TYPE_RS) Then
                    If (gbFgAutoOperation = True) Then
                        BtnSubstrateSet.Enabled = False
                    End If
                End If
                'V5.0.0.1②↑
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.BtnADJ_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    'V5.0.0.4①↓
#Region "SYCLEﾎﾞﾀﾝ押下時処理"
    Private Sub btnCycleStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCycleStop.Click
        Try
            If bFgCyclStp Then
                'bFgCyclStp = False
                'btnCycleStop.Text = "CYCLE STOP OFF"
                'btnCycleStop.BackColor = System.Drawing.SystemColors.Control
            Else
                bFgCyclStp = True                                          ' サイクル停止フラグ V4.0.0.0⑲
                btnCycleStop.Text = "CYCLE STOP ON"
                btnCycleStop.BackColor = System.Drawing.Color.Yellow
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            MsgBox("i-TKY.btnCycleStop_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region
    'V5.0.0.4①↑
    '----- V1.18.0.0③↓ -----
#Region "Print On/Offﾎﾞﾀﾝ押下時処理(ローム殿特注)"
    '''=========================================================================
    ''' <summary>Print On/Offﾎﾞﾀﾝ押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnPrintOnOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrintOnOff.Click

        Dim strMSG As String

        Try
            '----- V6.0.3.0_25↓ -----
            'If (gSysPrm.stDEV.rPrnOut_flg = True) Then
            If (BtnPrintOnOff.BackColor = Color.Lime) Then
                '-----V6.0.3.0_25↑ -----
                ' ON->OFF
                gSysPrm.stDEV.rPrnOut_flg = False
                BtnPrintOnOff.Text = "Print OFF"
                BtnPrintOnOff.BackColor = System.Drawing.SystemColors.Control
            Else
                ' OFF->ON
                gSysPrm.stDEV.rPrnOut_flg = True
                BtnPrintOnOff.Text = "Print ON"
                BtnPrintOnOff.BackColor = Color.Lime
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.BtnPrintOnOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Printﾎﾞﾀﾝ押下時処理(ローム殿特注)"
    '''=========================================================================
    ''' <summary>Printﾎﾞﾀﾝ押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrint.Click

        Dim strMSG As String

        Try
            '----- V6.0.3.0_39↓ -----
            Timer1.Enabled = False                                      ' 監視タイマー停止
            '----- V6.0.3.0_39↑ -----

            '----- V6.0.3.0_31↓ -----
            If gbAutoOperating = True Then                              ' 自動運転中なら印刷不可とする
                If (gSysPrm.stTMN.giMsgTyp = 0) Then
                    strMSG = "自動運転中は手動で印刷できません。自動運転を終了してから実行して下さい。 "
                Else
                    strMSG = "Cannot printout while auto operation mode. Please finish auto operation. "
                End If
                MsgBox(strMSG)
                GoTo STP_EXIT                                           ' V6.0.3.0_39
                'Exit Sub                                               ' V6.0.3.0_39
            End If
            '----- V6.0.3.0_31↑ -----

            ' トリミング結果印刷(Printボタン押下)
            Call PrnTrimResult(1)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.BtnPrint_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        '----- V6.0.3.0_39↓ -----
STP_EXIT:
        Call ZCONRST()                                              ' コンソールキーラッチ解除
        Timer1.Enabled = True                                       ' 監視タイマー開始
        '----- V6.0.3.0_39↑ -----
    End Sub
#End Region
    '----- V1.18.0.0③↑ -----
    '----- V1.18.0.0⑫↓ -----
#Region "収納マガジンに収納した基板枚数のクリアボタン押下時処理(ローム殿特注)"
    '''=========================================================================
    ''' <summary>収納マガジンに収納した基板枚数のクリアボタン押下時処理(ローム殿特注)</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnStrageClr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnStrageClr.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' "基板枚数をクリアしてもよろしいですか？"
            r = Me.System1.TrmMsgBox(gSysPrm, MSG_155, MsgBoxStyle.OkCancel, gAppName)
            If (r <> cFRS_ERR_START) Then Return '                      ' OK押下以外はReturn



            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.BtnStrageClr_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0⑫↑ -----
    '----- V1.18.0.4①↓ -----
#Region "Power Ctrl ON/OFFﾎﾞﾀﾝ押下時処理(ローム殿特注)"
    '''=========================================================================
    ''' <summary>Power Ctrl ON/OFFﾎﾞﾀﾝ押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BtnPowerOnOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPowerOnOff.Click

        Dim strMSG As String

        Try
            If (BtnPowerOnOff.Text = "Power Ctrl ON") Then
                ' ON->OFF
                BtnPowerOnOff.Text = "Power Ctrl OFF"
                BtnPowerOnOff.BackColor = System.Drawing.SystemColors.Control
            Else
                ' OFF->ON
                BtnPowerOnOff.Text = "Power Ctrl ON"
                BtnPowerOnOff.BackColor = Color.Lime
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.BtnPowerOnOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.4①↑ -----
    'V4.10.0.0①          ↓
#Region "ログオンユーザー表示・切替ボタン押下時処理"
    Private Sub btnUserLogon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserLogon.Click
        Try
            GrpMode.Select()
            'Using frm As New FormLogon()
            '    If (DialogResult.OK = frm.ShowDialog(Me)) Then
            '        SetBtnUserLogon()
            '    End If
            'End Using

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.btnUserLogon_Click() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMSG)
        End Try

    End Sub
    ''' <summary>ログオンユーザー表示更新</summary>
    ''' <param name="visible">False=非表示にする</param>
    ''' <remarks>'V4.10.0.0①</remarks>
    Private Sub SetBtnUserLogon(Optional ByVal visible As Boolean = True)
        With btnUserLogon
#If False Then  'V4.10.0.0⑦
            If (UserInfo.EnableUserLevel) Then
                .BackColor = UserInfo.LogonUser.GetBackColor()
                .ForeColor = UserInfo.LogonUser.GetForeColor()
                .Text = UserInfo.LogonUser.GetDisplayName()
                .Visible = visible
            Else
                .Visible = False
            End If
#Else
            'V4.10.0.0⑦ 研究開発ﾊﾞｰｼﾞｮﾝでは使用しない
            .Enabled = False
            .Visible = False
#End If
        End With

    End Sub
#End Region
    'V4.10.0.0①          ↑
    '----- V4.11.0.0⑥↓ (WALSIN殿SL436S対応) -----
#Region "基板投入ボタン押下時処理"
    '''=========================================================================
    '''<summary>基板投入ボタン押下時処理</summary>
    '''<param name="sender"></param>
    '''<param name="e"></param>
    '''=========================================================================
    Private Sub BtnSubstrateSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSubstrateSet.Click

        Dim strMSG As String
        Dim r As Integer

        Try

            If (BtnSubstrateSet.BackColor = System.Drawing.SystemColors.Control) Then
                gbChkSubstrateSet = True                                ' 基板投入フラグON 
                BtnSubstrateSet.BackColor = System.Drawing.Color.Pink
                'V5.0.0.1⑧
                'If IsNothing(gObjMSG) = True Then
                '    gObjMSG = New FrmWait()
                '    Call gObjMSG.Show(Me)
                'End If
                'V5.0.0.1⑧
                TimerAdjust.Enabled = False 'V4.11.0.0⑯
                GroupBoxEnableChange(False)
                ' 基板投入処理
                r = SubstrateSet_Proc(System1)
                'V5.0.0.1⑩
                ''V5.0.0.1⑭                 If r = cFRS_ERR_RST Or r = cFRS_ERR_LDR1 Then
                If r = cFRS_ERR_RST Then 'V5.0.0.1⑭ 
#If START_KEY_SOFT Then
                    If gbStartKeySoft Then
                        Call RESET_SWITCH_ON()
                    End If
#End If
                ElseIf r = cFRS_ERR_LDR1 Then
                    giErrLoader = cFRS_ERR_LDR1
                End If
                'V5.0.0.1⑩
                GroupBoxEnableChange(True)
                TimerAdjust.Enabled = True 'V4.11.0.0⑯
            Else
                gbChkSubstrateSet = False                               ' 基板投入フラグOFF
                BtnSubstrateSet.BackColor = System.Drawing.SystemColors.Control
                'V4.11.0.0⑪
                If IsNothing(gObjMSG) <> True Then
                    gObjMSG.MsgClose()
                    gObjMSG = Nothing
                End If
                'V4.11.0.0⑪
            End If


            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.BtnSubstrateSet_MouseDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "基板投入ボタンの背景色を設定する"
    '''=========================================================================
    ''' <summary>基板投入ボタンの背景色を設定する</summary>
    ''' <param name="bFlg">(INP)True:背景色=ピンク, False:背景色=灰色</param>
    '''=========================================================================
    Public Sub Set_SubstrateSetBtn(ByVal bFlg As Boolean)

        Dim strMSG As String

        Try
            If (bFlg = True) Then
                BtnSubstrateSet.BackColor = System.Drawing.Color.Pink
            Else
                BtnSubstrateSet.BackColor = System.Drawing.SystemColors.Control
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "i-TKY.Set_SubstrateSetBtn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0⑥↑ -----
    '----- V6.1.1.0④↓ -----
#Region "アラーム音のON/OFFボタン押下時処理(オプション)"
    '''=========================================================================
    ''' <summary>アラーム音のON/OFFボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>IAM殿特注</remarks>
    '''=========================================================================
    Private Sub BtnAlarmOnOff_Click(sender As System.Object, e As System.EventArgs) Handles BtnAlarmOnOff.Click

        Dim strMSG As String

        Try
            If (BtnAlarmOnOff.Text = "Buzzer On") Then
                ' ON->OFF
                Me.System1.giAlarmBuzzer = 0                            ' アラーム音を鳴らさない
                BtnAlarmOnOff.Text = "Buzzer Off"
                BtnAlarmOnOff.BackColor = System.Drawing.SystemColors.Control
            Else
                ' OFF->ON
                Me.System1.giAlarmBuzzer = 1                            ' アラーム音を鳴らす
                BtnAlarmOnOff.Text = "Buzzer On"
                BtnAlarmOnOff.BackColor = Color.Lime
            End If


            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.BtnAlarmOnOff_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V6.1.1.0④↑ -----

    '========================================================================================
    '   共通関数
    '========================================================================================
#Region "ログ画面に文字列を表示する"
    '''=========================================================================
    '''<summary>ログ画面に文字列を表示する</summary>
    '''<remarks>ﾛ</remarks>
    '''=========================================================================
    Public Function Z_PRINT(ByRef s As String) As Integer

        '#4.12.2.0④        Z_PRINT = LogPrint(s)
        Debug.Print("CON:" & s)

        '#4.12.2.0④                    ↓
        Static hWnd As IntPtr = txtLog.Handle
        Const WM_GETTEXTLENGTH As Integer = &HE
        Const EM_SETSEL As Integer = &HB1
        Const EM_REPLACESEL As Integer = &HC2

        Try
            Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)      ' 文字数取得
            SendMessage(hWnd, EM_SETSEL, len, len)                              ' カーソルを末尾へ
            SendMessageString(hWnd, EM_REPLACESEL, 0, s & Environment.NewLine)  ' テキストに文字列を追加する

            Z_PRINT = cFRS_NORMAL

            ' トラップエラー発生時 
        Catch ex As Exception
            Dim strMSG As String = "i-TKY.LogPrint() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMSG)
            Z_PRINT = cERR_TRAP
        End Try
        '#4.12.2.0④                    ↑
    End Function

    ''' <summary>ログ画面に文字列を表示する</summary>
    ''' <param name="s"></param>
    ''' <remarks>'#4.12.2.0④</remarks>
    Public Sub Z_PRINT(ByVal s As StringBuilder)

        Static hWnd As IntPtr = txtLog.Handle
        Const WM_SETREDRAW As Integer = &HB
        Const WM_GETTEXTLENGTH As Integer = &HE
        Const EM_SETSEL As Integer = &HB1
        Const EM_REPLACESEL As Integer = &HC2

        Try
            s.AppendLine()              ' 改行

            SendMessage(hWnd, WM_SETREDRAW, 0, 0)                           ' 描画停止
            Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)  ' 文字数取得
            SendMessage(hWnd, EM_SETSEL, len, len)                          ' カーソルを末尾へ
            SendMessageStringBuilder(hWnd, EM_REPLACESEL, 0, s)             ' テキストに文字列を追加する

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.Z_PRINT() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMSG)

        Finally
            SendMessage(hWnd, WM_SETREDRAW, 1, 0)                           ' 描画再開
        End Try

    End Sub
#End Region

#Region "ログ画面クリア"
    '''=========================================================================
    '''<summary>ログ画面クリア</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub Z_CLS()
        'Private Sub Z_CLS() '###013

        '#4.12.2.0④                    ↓
        '#4.12.2.0④        txtLog.Text = ""
        Static hWnd As IntPtr = txtLog.Handle
        Const WM_SETTEXT As Integer = &HC

        Try
            SendMessageString(hWnd, WM_SETTEXT, 0, "")                  ' 削除

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.Z_CLS() TRAP ERROR = " & ex.Message
            MessageBox.Show(Me, strMSG)
        End Try
        '#4.12.2.0④                    ↑
    End Sub
#End Region

#Region "ログ画面への文字列一行削除"
    '''=========================================================================
    '''<summary>ログ画面への文字列一行削除</summary>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Private Function Z_DEL() As Integer

        Dim stemp As String

        stemp = txtLog.Text
        txtLog.Text = Mid(stemp, 1, InStrRev(stemp, vbCrLf, Len(stemp) - 2) + 1)
        txtLog.Refresh()

        Z_DEL = cFRS_NORMAL

    End Function
#End Region

#Region "ログ画面への文字列表示"
#If False Then                          '#4.12.2.0④
    '''=========================================================================
    '''<summary>ログ画面への文字列表示</summary>
    '''<param name="s">(INP) 表示する文字列</param>
    '''<returns>0=成功, 0以外=失敗</returns>
    '''=========================================================================
    Private Function LogPrint(ByRef s As String) As Integer

        Dim strMSG As String

        Try
            Me.txtLog.AppendText(s + vbCrLf)                            ' テキストに文字列を追加する 
            '----- ###097 これ以下はなくても表示結果はかわらないため削除 -----
            'Me.txtLog.SelectionStart = Len(txtLog.Text)                 ' テキストの開始点を設定
            'Me.txtLog.SelectionLength = 0                               ' 選択文字列 = 0 
            'Me.txtLog.Refresh()                                         ' 再描画
            'Me.txtLog.ScrollToCaret()                                   ' 現在のカレット位置までスクロールする
            '----- ###097  -----

            LogPrint = cFRS_NORMAL

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.LogPrint() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            LogPrint = cERR_TRAP
        End Try

    End Function
#End If
#End Region

#Region "現在の日時ﾌｧｲﾙ名を作成(Index2専用)"
    '''=========================================================================
    '''<summary>現在の日時ﾌｧｲﾙ名を作成(Index2専用)</summary>
    '''<param name="sPath">(INP) ﾌｧｲﾙを作成するﾊﾟｽ名</param>
    '''<param name="sFrm"> (INP) 拡張子(.LOG/.TXT/.CSV)</param>
    '''<param name="sHead">(INP) ﾌｧｲﾙ名の先頭文字列</param>
    '''<returns>日時ﾌｧｲﾙ名(Full Path)</returns>
    '''=========================================================================
    Private Function MakeDateFileName(ByRef sPath As String, ByRef sFrm As String, ByRef sHead As String) As String

        Dim pDate As String
        Dim pTime As String
        Dim pFName As String

        ' 日時取得
        pDate = Today.ToString("yyyyMMdd") ' V1.18.0.0⑥
        pTime = TimeOfDay.ToString("hhmmss")

        ' ﾌｧｲﾙ名
        pFName = sPath & "\" & sHead & pDate & pTime & sFrm
        MakeDateFileName = pFName

        Debug.Print("MakeDateFileName = " & pFName)
    End Function

#End Region

#Region "トリミング開始処理"
    '''=========================================================================
    '''<summary>トリミング開始処理</summary>
    '''<remarks>トリミング実行前の各種チェック、設定実行処理。
    ''' GUI上のスタートボタンとコンソール上のSTART SW押下の
    ''' 2系統から入力を受け付ける。</remarks>
    '''=========================================================================
    Private Function Start_Trimming() As Integer

        Dim intRet As Integer = cFRS_NORMAL
        Dim intPos1 As Integer
        Dim intPos2 As Integer
        Dim strHead As String
        Dim strMSG As String

        Try
            intPos1 = 0
            intPos2 = 0
            strHead = ""

            ' 集塵機異常チェック
            intRet = System1.CheckDustVaccumeAlarm(gSysPrm)
            If (intRet <> cFRS_NORMAL) Then
                intRet = System1.Form_Reset(cGMODE_ERR_DUST, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                giAppMode = APP_MODE_EXIT
                If (intRet <= cFRS_ERR_EMG) Then
                    ' 強制終了
                    Call AppEndDataSave()
                    Call AplicationForcedEnding()
                    Return (intRet)
                End If
            End If

            ' 'V4.1.0.0⑦
            CheckPLCLowBatteryAlarm()

            ' IX2ログ出力パス取得 V1.18.0.0⑥
            sIX2LogFilePath = MakeDateFileName((gSysPrm.stLOG.gsLoggingDir), ".txt", "IX2_")

            ' EdgeSenseのログ出力パス取得
            intPos1 = InStrRev(gStrTrimFileName, "\")
            intPos2 = InStrRev(gStrTrimFileName, ".")

            If intPos1 <> 0 And intPos2 <> 0 Then
                strHead = Mid(gStrTrimFileName, intPos1 + 1, intPos2 - (intPos1 + 1)) 'データファイル名を取得（拡張子無し）
            End If
            If gEsCutFileType = 0 Then
                gsESLogFilePath = MakeDateFileName((gSysPrm.stLOG.gsLoggingDir), ".txt", "ES_" & strHead & "_") 'ﾌｧｲﾙﾊﾟｽ取得
            Else
                gsESLogFilePath = MakeDateFileName((gSysPrm.stLOG.gsLoggingDir), ".txt", "ES2_" & strHead & "_") 'ﾌｧｲﾙﾊﾟｽ取得
            End If

            ' メインの表示状態変更
            Call Form1Button(0)                             ' コマンドボタンを非活性化にする

            'V2.0.0.0⑬　↓　
            'V4.10.0.0⑬            If gKeiTyp = KEY_TYPE_RS Then
            MidiumCut.DetectFunc = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "MIDIUM_CUT_FUNC", SYSPARAMPATH, "0")) ' V2.0.0.0⑦
            MidiumCut.JudgeCount = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "MIDIUM_CUT_JUDGE_COUNT", SYSPARAMPATH, "0")) ' V2.0.0.0⑦
            MidiumCut.dblChangeRate = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "MIDIUM_CUT_CHANGE_RATE", SYSPARAMPATH, "0")) ' V2.0.0.0⑦
            Dim longbuf(2) As Long
            Dim doublebuf(2) As Double
            longbuf(0) = MidiumCut.DetectFunc
            longbuf(1) = MidiumCut.JudgeCount
            doublebuf(0) = MidiumCut.dblChangeRate
            intRet = MIDIUMCUTPARAM(doublebuf(0), longbuf(0), longbuf(1))
            'V2.0.0.0⑬　↑

            'V2.0.0.0⑭　↓
            'V4.10.0.0⑬プレートデータから取得しているのでコメントアウト            glProbeRetryCount = Val(GetPrivateProfileString_S("SPECIALFUNCTION", "PROBE_RETRY_COUNT", SYSPARAMPATH, "0")) ' V2.0.0.0⑦
            'V2.0.0.0⑭　↑
            'V4.10.0.0⑬            End If

            '-----------------------------------------------------------------------
            '   トリミング開始
            '-----------------------------------------------------------------------
            intRet = Trimming()

            ' ###073
            If (intRet = cFRS_NORMAL) Or (intRet = cFRS_ERR_RST) Then

                ' ローダアラームは終了しない
            ElseIf (intRet = cFRS_ERR_LDR) Or (intRet = cFRS_ERR_LDR1) Or (intRet = cFRS_ERR_LDR2) Or (intRet = cFRS_ERR_LDR3) Or (intRet = cFRS_ERR_LDRTO) Then

            ElseIf (intRet = cFRS_ERR_EMG) Then
                ' 強制終了
                Call AppEndDataSave()
                Call AplicationForcedEnding()
                Return (intRet)
            End If

            'メインの表示状態変更
            If giHostMode <> cHOSTcMODEcAUTO Then
                Call Form1Button(1)                         ' コマンドボタンを有効にする
            Else
                Call Form1Button(3)                         ' ﾃｰﾀｾｰﾌﾞﾎﾞﾀﾝ(F2), ENDﾎﾞﾀﾝ(F10)のみ有効にする
            End If

            If giHostMode = cHOSTcMODEcMANUAL Then
                Call ZCONRST()
            End If

            ' 'V4.1.0.0⑦
            CheckPLCLowBatteryAlarm()

            Return (intRet)

        Catch ex As Exception
            strMSG = "i-TKY.Start_Trimming() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "データ編集プログラム起動処理"
    '''=========================================================================
    ''' <summary>データ編集プログラム起動処理 ###063</summary>
    ''' <param name="Mode">(INP)モード2(0=通常モード, 1=一時停止画面モード)</param>
    ''' <param name="shiftKeyDown">shiftキー押下</param>
    ''' <remarks>一時停止画面モード時は「抵抗」と「カット」データのみ更新可能とする</remarks>
    '''=========================================================================
    Public Function ExecEditProgram(ByVal Mode As Integer, Optional ByVal shiftKeyDown As Boolean = False) As Integer '#4.12.2.0⑤ 
        '#4.12.2.0⑤    Public Function ExecEditProgram(ByVal Mode As Integer) As Integer

        Dim piPP31 As Short
        Dim br As Boolean
        Dim r As Integer
        Dim iPW As Integer = 0                                          ' V1.18.0.0①
        Dim intRet As Integer
        Dim iRtn As Integer = cFRS_NORMAL                               ' Return値
        Dim ExitCode As Integer
        Dim strFilePath As String
        Dim strLdFlf As String
        Dim Objproc As Process = Nothing                                ' プロセス起動用
        Dim strMSG As String
        Dim wreg As Short                                               ' V1.18.0.0⑦
        Dim GType As Integer                                            ' V1.18.0.0⑦
        Dim nAutoMode As Integer                                        ' V1.19.0.0-27
        Dim dblRotateTheta As Double                                    ' θ回転角度 V1.19.0.0-27
        Dim dblReviseCordnt1XDir As Double                              ' 補正位置座標1X V1.19.0.0-27
        Dim dblReviseCordnt1YDir As Double                              ' 補正位置座標1Y V1.19.0.0-27
        Dim dblReviseCordnt2XDir As Double                              ' 補正位置座標2X V1.19.0.0-27
        Dim dblReviseCordnt2YDir As Double                              ' 補正位置座標2Y V1.19.0.0-27
        Dim dblReviseOffsetXDir As Double                               ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX V1.19.0.0-27
        Dim dblReviseOffsetYDir As Double                               ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY V1.19.0.0-27
        '                                       'V5.0.0.9④  ↓
        Dim dblReviseCordnt1XDirRgh As Double                           ' 補正位置座標1X
        Dim dblReviseCordnt1YDirRgh As Double                           ' 補正位置座標1Y
        Dim dblReviseCordnt2XDirRgh As Double                           ' 補正位置座標2X
        Dim dblReviseCordnt2YDirRgh As Double                           ' 補正位置座標2Y
        Dim dblReviseOffsetXDirRgh As Double                            ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
        Dim dblReviseOffsetYDirRgh As Double                            ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
        '                                       'V5.0.0.9④  ↑
        'V4.0.0.0-65            ↓↓↓↓
        Dim adjust As String = String.Empty
        ' 一時停止画面からの呼び出し時
        If (1 = Mode) Then adjust = "Adjust : "
        'V4.0.0.0-65            ↑↑↑↑
        '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
        Dim BlkX As Integer = 1
        Dim BlkY As Integer = 1
        '----- V4.11.0.0⑤↑ -----

        Try
            '-------------------------------------------------------------------
            '   トリミングデータを中間ファイルに書込む(ファイル上書)
            '-------------------------------------------------------------------
            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' 現在のブロック番号を退避しておく
            'V5.0.0.9⑯            If (giStartBlkAss = 1) Then                             ' トリミング開始ブロック番号指定有効 ?
            If (giStartBlkAss <> 0) Then                                ' トリミング開始ブロック番号指定有効 ?   'V5.0.0.9⑯
                Get_StartBlkNum(BlkX, BlkY)
            End If
            '----- V4.11.0.0⑤↑ -----

            ' 中間ファイル名を設定する
            If (gTkyKnd = KND_TKY) Then
                strFilePath = EditWorkFilePath + ".tdtw"                ' TKYの場合
            ElseIf (gTkyKnd = KND_CHIP) Then                            ' CHIPの場合
                strFilePath = EditWorkFilePath + ".tdcw"
            Else                                                        ' NETの場合
                strFilePath = EditWorkFilePath + ".tdnw"
            End If

            ' 新規モード時はデータファイル名 = (例 TKY時)"TkyDatayyyymmddhhmmss.tdt"とする
            '#4.12.2.0⑤ If (gLoadDTFlag = False) Then                  ' 新規モード ?
            If (gLoadDTFlag = False) OrElse (shiftKeyDown) Then         ' 新規モード     '#4.12.2.0⑤ shiftキー押下時の新規作成追加
                If (gLoadDTFlag) Then
                    ' shiftキー押下時の新規作成
                    Temporary.Backup()                                  ' トリミングデータ構造体をバックアップする
                End If
                Call Init_AllTrmmingData()                              ' ﾄﾘﾐﾝｸﾞﾊﾟﾗﾒｰﾀの初期化
                'V5.0.0.8① Call Init_FileVer_Sub()                     ' ﾌｧｲﾙﾊﾞｰｼﾞｮﾝ設定(CHIP/NET用)
                Replace_Data_Extension(gStrTrimFileName)                ' データファイル名 = (例 TKY時)"TkyDatayyyymmddhhmmss.tdt"
                '                                                       ' プレートデータにデータファイル名を設定する
                gStrTrimFileName = DATA_DIR_PATH + "\" + gStrTrimFileName
                typPlateInfo.strDataName = gStrTrimFileName
                strLdFlf = "0"                                          ' モード = 0(新規)
            Else
                strLdFlf = "1"                                          ' モード = 1(更新)
                'V4.0.0.0-65            ↓↓↓↓
                ' ﾛｸﾞ表示開始ﾌﾗｸﾞをﾁｪｯｸする。
                If (1 = gSysPrm.stLOG.giLoggingMode) Then
                    basTrimming.TrimLogging_CreateOrAppend(adjust & "Edit - Start")
                End If
                'V4.0.0.0-65            ↑↑↑↑
            End If

            'V1.19.0.0-27
            ' 現在の設定値を保存しておく。
            nAutoMode = typPlateInfo.intReviseMode
            piPP31 = typPlateInfo.intManualReviseType
            dblRotateTheta = typPlateInfo.dblRotateTheta
            dblReviseCordnt1XDir = typPlateInfo.dblReviseCordnt1XDir
            dblReviseCordnt1YDir = typPlateInfo.dblReviseCordnt1YDir
            dblReviseCordnt2XDir = typPlateInfo.dblReviseCordnt2XDir
            dblReviseCordnt2YDir = typPlateInfo.dblReviseCordnt2YDir
            dblReviseOffsetXDir = typPlateInfo.dblReviseOffsetXDir      'V5.0.0.9④  抜け？
            dblReviseOffsetYDir = typPlateInfo.dblReviseOffsetYDir      'V5.0.0.9④  抜け？
            'V1.19.0.0-27
            '                                       'V5.0.0.9④  ↓
            dblReviseCordnt1XDirRgh = typPlateInfo.dblReviseCordnt1XDirRgh      ' 補正位置座標1X
            dblReviseCordnt1YDirRgh = typPlateInfo.dblReviseCordnt1YDirRgh      ' 補正位置座標1Y
            dblReviseCordnt2XDirRgh = typPlateInfo.dblReviseCordnt2XDirRgh      ' 補正位置座標2X
            dblReviseCordnt2YDirRgh = typPlateInfo.dblReviseCordnt2YDirRgh      ' 補正位置座標2Y
            dblReviseOffsetXDirRgh = typPlateInfo.dblReviseOffsetXDirRgh        ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄX
            dblReviseOffsetYDirRgh = typPlateInfo.dblReviseOffsetYDirRgh        ' 補正ﾎﾟｼﾞｼｮﾝｵﾌｾｯﾄY
            '                                       'V5.0.0.9④  ↑
            ' トリミングデータを中間ファイルに書込む
            r = File_Save(strFilePath)
            If (r <> cFRS_NORMAL) Then GoTo STP_WRITE_ERR

            ''V4.5.0.0⑤ ↓
            ' 統計表示処理の状態変更
            Call RedrawDisplayDistribution(True)
            ''V4.5.0.0⑤ ↑

            '-------------------------------------------------------------------
            '   データ編集プログラムを起動する
            '-------------------------------------------------------------------
            ' プロセスの起動
            Objproc = New Process                                       ' Processオブジェクトを生成する 

            ' コマンドライン引数設定
            'If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ? 
            If (giMachineKd = MACHINE_KD_RS) OrElse (0 <> Globals_Renamed.giChipEditEx) Then 'V4.10.0.0②
                '----- V2.0.0.0_25↓ -----
                ' コマンドライン引数設定
                Objproc.StartInfo.FileName = "C:\TRIM\TrimDataEditorEx.exe"
                '                                                           ' コマンドライン引数を作成する
                strMSG = gTkyKnd.ToString("0") + " "                        ' args[0]:データ種別("0"=TKY, "1"=TKY-CHIP, "2"=TKY-NET
                strMSG = strMSG + """" + gStrTrimFileName + """" + " "      ' args[1]:データファイル名
                strMSG = strMSG + strFilePath + " "                         ' args[2]:中間ファイル名
                strMSG = strMSG + strLdFlf + " "                            ' args[3]:モード(0=新規, 1=更新)
                strMSG = strMSG + Mode.ToString("0") + " "                  ' args[4]:モード2(0=通常モード, 1=一時停止画面モード) 
                iPW = Get_DataEdit_PasswordKind(gSysPrm.stCTM.giSPECIAL)    '         パスワード機能取得                                    
                strMSG = strMSG + iPW.ToString("0")                         ' args[5]:パスワード機能(0=なし, 1=あり(進殿仕様), 2あり(ローム殿仕様, 3=シンプルトリマ)
                strMSG &= (" " & UserInfo.LogonUser.GetName())              ' args[6]:ログオンユーザー  'V4.10.0.0① 'V5.0.0.8① ｱﾝｺﾒﾝﾄ
                Objproc.StartInfo.Arguments = strMSG                        ' コマンドライン引数設定
                '----- V2.0.0.0_25↑ -----
            Else
                ' コマンドライン引数設定
                Objproc.StartInfo.FileName = "C:\TRIM\TrimDataEditor.exe"
                '                                                           ' コマンドライン引数を作成する
                strMSG = gTkyKnd.ToString("0") + " "                        ' args[0]:データ種別("0"=TKY, "1"=TKY-CHIP, "2"=TKY-NET
                strMSG = strMSG + """" + gStrTrimFileName + """" + " "      ' args[1]:データファイル名 '###034
                strMSG = strMSG + strFilePath + " "                         ' args[2]:中間ファイル名
                strMSG = strMSG + strLdFlf + " "                            ' args[3]:モード(0=新規, 1=更新)
                strMSG = strMSG + Mode.ToString("0") + " "                  ' args[4]:モード2(0=通常モード, 1=一時停止画面モード) V1.14.0.0①
                'strMSG = strMSG + giPassWord_Lock.ToString("0")            ' args[5]:パスワード機能(0=なし, 1=あり)                V1.14.0.0①
                iPW = Get_DataEdit_PasswordKind(gSysPrm.stCTM.giSPECIAL)    '         パスワード機能取得                                          V1.18.0.0①
                strMSG = strMSG + iPW.ToString("0")                         ' args[5]:パスワード機能(0=なし, 1=あり(進殿仕様), 2あり(ローム殿仕様)  V1.18.0.0①
                strMSG &= (" " & UserInfo.LogonUser.GetName())              ' args[6]:ログオンユーザー  'V4.10.0.0① 'V5.0.0.8① ｱﾝｺﾒﾝﾄ
                Objproc.StartInfo.Arguments = strMSG                        ' コマンドライン引数設定
            End If

            VideoLibrary1.VideoStop()                                   'V6.0.0.0-29

            Objproc.StartInfo.WorkingDirectory = "C:\\TRIM"             ' 作業フォルダ
            Objproc.Start()                                             ' プロセス起動

            ' Formを最小化する
            'Me.WindowState = FormWindowState.Minimized
            If (ObjIOMon Is Nothing = False) Then                       ' ＩＯモニタ非表示 ?
                ObjIOMon.Hide()
            End If

            ' プロセスの終了チェック
            br = False
            Do
                ' 非常停止等チェック
                intRet = System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
                If (intRet <> cFRS_NORMAL) Then                         ' エラー検出 ?

                    ' プロセス終了メッセージ送信(ｳｨﾝﾄﾞｳのあるｱﾌﾟﾘの場合) 
                    If (Objproc.CloseMainWindow() = False) Then         ' ﾒｲﾝｳｨﾝﾄﾞｳにｸﾛｰｽﾞﾒｯｾｰｼﾞを送信する
                        Objproc.Kill()                                  ' 終了しなかった場合は強制終了する
                    End If

                    ' プロセスの終了を待つ
                    Do
                        System1.WAIT(0.1)                               ' Wait(Sec) 
                    Loop While (Objproc.HasExited <> True)              ' プロセスが終了している場合のみTrueが返る
                    Exit Do
                End If

                ' プロセスの終了チェック
                br = Objproc.WaitForExit(300)                           ' プロセスの終了待ち(ms) ※時間指定なしは終了まで返ってこない 
            Loop While (br = False)

            ' データ編集プログラムの終了コードを取得する 
            'V4.10.0.0①              ↓
            'ExitCode = Objproc.ExitCode                                 ' 終了コード取得(HasExited=Trueでないとﾄﾗｯﾌﾟが発生する)
            ExitCode = UserInfo.GetExitCode(Objproc.ExitCode)           ' 終了コード取得(HasExited=Trueでないとﾄﾗｯﾌﾟが発生する)'V5.0.0.8①
#If False Then  'V4.10.0.0⑧ 研究開発ﾊﾞｰｼﾞｮﾝでは編集画面のﾕｰｻﾞを引き継がない
            UserInfo.LogonUser = UserInfo.GetLogonUser(Objproc.ExitCode) ' ログオンユーザー取得
            UserInfo.ReadUserSettings()                                 ' 表示名・パスワードを再読み込みする
#End If
            SetBtnUserLogon()                                           ' ログオンユーザー表示更新
            'V4.10.0.0①              ↑
            Objproc.Close()                                             ' オブジェクト開放
            Objproc.Dispose()                                           ' リソース開放
            Objproc = Nothing                                           'V4.10.0.0①

            ' Formを元に戻す
            'Me.WindowState = FormWindowState.Normal
            If (ObjIOMon Is Nothing = False) Then                       ' ＩＯモニタ表示 ?
                ObjIOMon.Show()
            End If

            ' コマンドボタンを有効にする(通常モード時)
            If (Mode = 0) Then
                Call Form1Button(1)                                     ' コマンドボタンを有効にする
                Call SetVisiblePropForVideo(True)
                Me.Refresh()
            End If

            '-------------------------------------------------------------------
            '   非常停止等のエラーを検出した場合
            '-------------------------------------------------------------------
            If (intRet <> cFRS_NORMAL) Then                             ' エラー検出 ?
                If (intRet <= cFRS_ERR_EMG) Then                        ' 非常停止等検出 ?
                    Call Me.AppEndDataSave()
                    Call Me.AplicationForcedEnding()                    ' 強制終了
                End If
                iRtn = intRet                                           ' Return値設定
                GoTo STP_END
            End If

            '-------------------------------------------------------------------
            '   中間ファイルを読込む
            '-------------------------------------------------------------------
            ' データ編集プログラムの終了コードが正常かチェックする
            If (ExitCode = PRC_EXIT_CAN) Or (ExitCode = PRC_EXIT_NML) Or (ExitCode = PRC_EXIT_NML2) Then
                ' 正常時はNOP
            Else
                ' "データ編集エラー。データを再ロードしてください。(ExitCode = xxx)"
                strMSG = MSG_135 + "(ExitCode = " + ExitCode.ToString("0") + ")"
                iRtn = cFRS_FIOERR_INP                                  ' Return値 = ファイル入力エラー
                GoTo STP_ERR_DSP
            End If

            ' ↓↓↓ V3.1.0.0② 2014/12/01
            ' データ編集のキャンセル時はデータを読み込まないようにする
            If ExitCode = PRC_EXIT_CAN Then
#If False Then  '#4.12.2.0⑤             ↓
                'V4.0.0.0-65            ↓↓↓↓
                If (True = gLoadDTFlag) AndAlso (1 = gSysPrm.stLOG.giLoggingMode) Then
                    basTrimming.TrimLogging_CreateOrAppend( _
                        adjust & "Edit - Cancel", procMsgOnly:=True)
                End If
                'V4.0.0.0-65            ↑↑↑↑
#End If
                If (gLoadDTFlag) Then
                    If (shiftKeyDown) Then
                        ' shiftキー押下時の新規作成でキャンセル
                        Temporary.Restore()                             ' トリミングデータ構造体を復元する
                        gStrTrimFileName = typPlateInfo.strDataName
                    Else
                        If (1 = gSysPrm.stLOG.giLoggingMode) Then
                            basTrimming.TrimLogging_CreateOrAppend(
                                adjust & "Edit - Cancel", procMsgOnly:=True)
                        End If
                    End If
                End If
                '#4.12.2.0⑤             ↑
                GoTo STP_END
            End If
            ' ↑↑↑ V3.1.0.0② 2014/12/01

            ' 中間ファイルを読込む
            r = File_Read(strFilePath)
            If (r <> cFRS_NORMAL) Then GoTo STP_READ_ERR

            'V4.0.0.0-65            ↓↓↓↓
            ' ﾛｸﾞ表示開始ﾌﾗｸﾞをﾁｪｯｸする。
            If (1 = gSysPrm.stLOG.giLoggingMode) Then
                '#4.12.2.0⑤    If (False = gLoadDTFlag) Then
                If (False = gLoadDTFlag) OrElse (shiftKeyDown) Then     '#4.12.2.0⑤ shiftキー押下新規作成追加
                    If (PRC_EXIT_NML2 = ExitCode) Then
                        ' ﾃﾞｰﾀﾌｧｲﾙ未ﾛｰﾄﾞ時に編集画面からSaveで戻った場合
                        basTrimming.TrimLogging_CreateOrAppend("Edit - Save", typPlateInfo.strDataName)
                    Else
                        ' ﾃﾞｰﾀﾌｧｲﾙ未ﾛｰﾄﾞ時に編集画面からOKで戻った場合
                        basTrimming.TrimLogging_CreateOrAppend("Edit - New", typPlateInfo.strDataName)
                    End If
                Else
                    If (gStrTrimFileName <> typPlateInfo.strDataName) Then
                        ' 編集画面で別名保存された場合
                        If (0 = Mode) Then
                            ' 編集画面を通常起動した場合
                            basTrimming.TrimLogging_CreateOrAppend("Edit - Save (" & typPlateInfo.strDataName _
                                                                   & ")", procMsgOnly:=True)        ' 既存ﾌｧｲﾙに保存

                            basTrimming.TrimLogging_CreateOrAppend("Edit - Save",
                                                                   typPlateInfo.strDataName)        ' 新規ﾌｧｲﾙに保存 
                        Else
                            ' 一時停止画面から起動した場合
                            basTrimming.TrimLogging_CreateOrAppend(
                                adjust & "Edit - Save (" & typPlateInfo.strDataName & ")")          ' 既存ﾌｧｲﾙに保存
                        End If
                    Else
                        ' OKで終了または同名保存で終了した場合
                        basTrimming.TrimLogging_CreateOrAppend(adjust & "Edit - End")
                    End If
                End If
            End If
            'V4.0.0.0-65            ↑↑↑↑

            ' データファイル名更新
            gStrTrimFileName = typPlateInfo.strDataName
            '----- V6.1.4.0⑦↓(KOA EW殿SL432RD対応) -----
            'LblDataFileName.Text = typPlateInfo.strDataName
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = typPlateInfo.strDataName          ' トリミングデータ名と第１抵抗データ表示域を設定する 
            Else
                LblDataFileName.Text = typPlateInfo.strDataName
            End If
            '----- V6.1.4.0⑦↑ -----
            'V4.2.0.0③
            If gKeiTyp = KEY_TYPE_RS Then
                TrimData.SetTrimmingData(gStrTrimFileName)
            End If
            'V4.2.0.0③

            gLoadDTFlag = True                                          ' gLoadDTFlag = データロード済
            commandtutorial.SetDataLoad()                               'V2.0.0.0⑩ データロード状態設定
            If (ExitCode = PRC_EXIT_NML) Or (ExitCode = PRC_EXIT_NML2) Then
                gCmpTrimDataFlg = 1                                     ' データ更新フラグ = 1(更新あり)
            End If

            If (ExitCode = PRC_EXIT_NML2) Then                          ' 正常終了(データセーブ要求あり) ?
                ' "データをセーブしました"
                strMSG = MSG_145 & vbCrLf & " (File Name = " + gStrTrimFileName + ")"
                Call Z_PRINT(strMSG)                                    ' メッセージ表示(ログ画面)
            End If

            '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
            ' 現在のブロック番号を再設定する
            'V5.0.0.9⑯            If (giStartBlkAss = 1) Then                             ' トリミング開始ブロック番号指定有効 ?
            If (giStartBlkAss <> 0) Then                                ' トリミング開始ブロック番号指定有効 ?   'V5.0.0.9⑯
                Set_StartBlkNum(BlkX, BlkY)
            End If
            '----- V4.11.0.0⑤↑ -----

            '----- V1.14.0.0⑧↓ -----
            ' FL側から現在の加工条件を受信する
            If (gSysPrm.stRAT.giOsc_Res = OSCILLATOR_FL) Then

                '----- V6.0.3.0②↓ -----
                ' SL436S時は加工条件番号を設定しなおす
                Call SetCndNum()
                '----- V6.0.3.0②↑ -----

                r = TrimCondInfoRcv(stCND)
                If (r <> SerialErrorCode.rRS_OK) Then
                    ' "ＦＬ側加工条件のリードに失敗しました。"
                    Call System1.TrmMsgBox(gSysPrm, MSG_141, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                    iRtn = r                                         ' Return値設定
                    GoTo STP_END
                End If
                '----- V4.0.0.0-28 ↓ -----
                '-----------------------------------------------------------------------
                '   加工条件番号からカットデータのQレート,電流値,STEG本数を設定する
                '   (シンプルトリマ用(FL時で加工条件ファイルがある場合))
                '-----------------------------------------------------------------------
                r = SetCutDataCndInfFromCndNum(_readFileVer)
                '----- V4.0.0.0-28 ↑ -----

                '----- V3.0.0.0⑩↓-----
                ' FL用パワー調整情報ファイル(データファイル名+.att)をライトする
                'V6.0.1.021　↓
                '                    If (giAutoPwr = 1) Then                             ' オートパワー調整を(1=する/0=しない) 
                If (giAutoPwr = 1) Or (giFixAttOnly = 1) Then                             ' オートパワー調整を(1=する/0=しない) 
                    'V6.0.1.021　↑
                    r = GetFLAttFileName(gStrTrimFileName, strMSG, False)   ' 固定ATT情報ファイル名を取得する
                    Call PutFlAttInfoData(strMSG, stPWR_LSR)                ' FL用パワー調整情報ファイルをライトする
                End If
                '----- V3.0.0.0⑩↑-----

            End If
            '----- V1.14.0.0⑧↑ -----
            '----- V1.18.0.0⑦↓ -----
            ' 汎用GP-IB制御の有無を取得する
            Call Gpib2FlgCheck(bGpib2Flg, wreg, GType)
            ' GPIBデータをINtime側に送信する
            If (GType = 1) Then                                         ' 汎用GPIB ?
                r = SendTrimDataPlate2(gTkyKnd, wreg, GType)
                If (r <> cFRS_NORMAL) Then                              ' エラー ? (メッセージは表示済み)
                    Return (r)
                End If
            End If
            '----- V1.18.0.0⑦↑ -----

            ' OKボタンが押下された場合
            If (gCmpTrimDataFlg = 1) Then                               ' データ更新フラグ(0=更新なし, 1=更新あり)
                ' 位置補正モード=1(手動)であるかチェックする
                If (typPlateInfo.intReviseMode = 1) Then
                    ' 位置補正方法を一回のみに変更した ?
                    'V5.0.0.9④              ↓
                    'If typPlateInfo.intManualReviseType = 1 And (piPP31 <> 1 Or nAutoMode <> 1 Or _
                    '                        dblRotateTheta <> typPlateInfo.dblRotateTheta Or _
                    '                        dblReviseCordnt1XDir <> typPlateInfo.dblReviseCordnt1XDir Or _
                    '                        dblReviseCordnt1YDir <> typPlateInfo.dblReviseCordnt1YDir Or _
                    '                        dblReviseCordnt2XDir <> typPlateInfo.dblReviseCordnt2XDir Or _
                    '                        dblReviseCordnt2YDir <> typPlateInfo.dblReviseCordnt2YDir Or _
                    '                        dblReviseOffsetXDir <> typPlateInfo.dblReviseOffsetXDir Or _
                    '                        dblReviseOffsetYDir <> typPlateInfo.dblReviseOffsetYDir) Then ' V1.19.0.0-27
                    If (typPlateInfo.intManualReviseType = 1) AndAlso
                        ((piPP31 <> 1) OrElse
                         (nAutoMode <> 1) OrElse
                         (dblRotateTheta <> typPlateInfo.dblRotateTheta) OrElse
                         (dblReviseCordnt1XDir <> typPlateInfo.dblReviseCordnt1XDir) OrElse
                         (dblReviseCordnt1YDir <> typPlateInfo.dblReviseCordnt1YDir) OrElse
                         (dblReviseCordnt2XDir <> typPlateInfo.dblReviseCordnt2XDir) OrElse
                         (dblReviseCordnt2YDir <> typPlateInfo.dblReviseCordnt2YDir) OrElse
                         (dblReviseOffsetXDir <> typPlateInfo.dblReviseOffsetXDir) OrElse
                         (dblReviseOffsetYDir <> typPlateInfo.dblReviseOffsetYDir) OrElse
                         (dblReviseCordnt1XDirRgh <> typPlateInfo.dblReviseCordnt1XDirRgh) OrElse
                         (dblReviseCordnt1YDirRgh <> typPlateInfo.dblReviseCordnt1YDirRgh) OrElse
                         (dblReviseCordnt2XDirRgh <> typPlateInfo.dblReviseCordnt2XDirRgh) OrElse
                         (dblReviseCordnt2YDirRgh <> typPlateInfo.dblReviseCordnt2YDirRgh) OrElse
                         (dblReviseOffsetXDirRgh <> typPlateInfo.dblReviseOffsetXDirRgh) OrElse
                         (dblReviseOffsetYDirRgh <> typPlateInfo.dblReviseOffsetYDirRgh)) Then
                        'V5.0.0.9④          ↑
                        gManualThetaCorrection = True                   ' シータ補正実行フラグ = True(シータ補正を実行する)
                    End If
                End If
                '----- V1.15.0.0③↓ -----
                ' INtime側のZOFF位置を設定する
                'r = PROBOFF_EX(typPlateInfo.dblZWaitOffset)            ' INtime側のZOFF位置をzWaitPosとする 'V1.15.0.0④
                r = SETZOFFPOS(typPlateInfo.dblZWaitOffset)             ' INtime側のZOFF位置をzWaitPosとする 'V1.15.0.0④
                r = System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
                If (r <> cFRS_NORMAL) Then                              ' エラーならエラーリターン(メッセージ表示済み)
                    Return (r)
                End If
                '----- V1.15.0.0③↑ -----
            End If

            '----- V2.0.0.0_25↓ -----
            ' SL436Sの場合はデータ編集と「EDIT LOCK/UNLOCKﾎﾞﾀﾝ」をリンクさせる為シスパラをリードする(EDIT LOCK/UNLOCKボタン表示ありの場合)
            If ((giMachineKd = MACHINE_KD_RS) And (giBtn_EdtLock = 1)) Then
                r = Val(GetPrivateProfileString_S("DEVICE_CONST", "EDITLOCK", SYSPARAMPATH, "0"))
                If (r = 0) Then
                    gSysPrm.stDEV.sEditLock_flg = False                 ' 「EDIT LOCK」
                Else
                    gSysPrm.stDEV.sEditLock_flg = True                  ' 「EDIT UNLOCK」
                End If

                '「EDIT LOCK/UNLOCKﾎﾞﾀﾝ」表示
                If (gSysPrm.stDEV.sEditLock_flg = False) Then
                    ' LOCK
                    CmdCnd.Text = "EDIT LOCK"
                    CmdCnd.BackColor = System.Drawing.SystemColors.Control
                    giPassWord_Lock = 1
                Else
                    ' UNLOCK
                    CmdCnd.Text = "EDIT UNLOCK"
                    CmdCnd.BackColor = System.Drawing.Color.Lime
                    giPassWord_Lock = 0
                End If
            End If
            '----- V2.0.0.0_25↑ -----

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
STP_END:
            ''V4.5.0.0⑤ ↓
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)
            ''V4.5.0.0⑤ ↑
            Return (iRtn)

STP_READ_ERR:
            ' "中間ファイルの読込みに失敗しました。"
            Call MsgBox(MSG_133, MsgBoxStyle.OkOnly, "")
            strMSG = MSG_135                                            ' "データ編集エラー。データを再ロードしてください。"
            iRtn = cFRS_FIOERR_INP                                      ' Return値 = ファイル入力エラー
            GoTo STP_ERR_DSP

STP_WRITE_ERR:
            ' "中間ファイルの書き込みに失敗しました。"
            Call MsgBox(MSG_134, MsgBoxStyle.OkOnly, "")
            strMSG = MSG_135                                            ' "データ編集エラー。データを再ロードしてください。"
            iRtn = cFRS_FIOERR_OUT                                      ' Return値 = ファイル出力エラー
            GoTo STP_ERR_DSP

STP_ERR_DSP:
            ' Formを元に戻す
            Me.WindowState = FormWindowState.Normal
            ' "データ編集エラー。データを再ロードしてください。"
            Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
            Call Z_PRINT(strMSG)                                        ' メッセージ表示(ログ画面)
            gStrTrimFileName = String.Empty                             ' V4.0.0.0-41
            '----- V6.1.4.0⑦↓(KOA EW殿SL432RD対応) -----
            ' トリミングデータ名表示域クリア
            'LblDataFileName.Text = String.Empty                         ' V4.0.0.0-41
            If (giQrCodeType = QrCodeType.KoaEw) Then
                LblDataFileNameText = String.Empty                      ' トリミングデータ名表示域と第１抵抗データ表示域クリア  
            Else
                LblDataFileName.Text = String.Empty
            End If
            '----- V6.1.4.0⑦↑ -----
            gLoadDTFlag = False                                         ' gLoadDTFlag = データロード済フラグOFF
            GoTo STP_END

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.ExecEditProgram() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            iRtn = cERR_TRAP                                            ' Return値 = トラップエラー発生
            GoTo STP_END

        Finally                                                         'V6.0.0.0-29
            'V4.12.2.0①                 ↓
            If (0 = Mode) AndAlso ((ExitCode = PRC_EXIT_NML) OrElse (ExitCode = PRC_EXIT_NML2)) Then
                ' 一時停止画面からの呼び出しではない場合
                ' 編集画面キャンセルではない正常終了
                Dim doEnabled As Boolean = (cFRS_NORMAL = iRtn)
                MapInitialize(doEnabled)                                ' マップ再描画
            End If
            'V4.12.2.0①                 ↑

            VideoLibrary1.VideoStart()
        End Try

    End Function
#End Region
    '----- ###240↓-----
#Region "載物台に基板がある事をチェックする(手動モード時)"
    '''=========================================================================
    ''' <summary>載物台に基板がある事をチェックする(手動モード時)</summary>
    ''' <param name="ObjSys"> (INP)OcxSystemオブジェクト</param>
    ''' <returns>0=正常
    '''          3=基板無し(Cancel(RESETｷｰ)　
    '''          上記以外=エラー</returns> 
    '''=========================================================================
    Public Function SubstrateExistCheck(ByVal ObjSys As Object) As Integer

        Dim strMSG As String
        Dim r As Integer

        Try
            ' 載物台に基板がある事をチェックする(手動モード時)
            If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                r = Sub_SubstrateExistCheck_432R(ObjSys)                ' SL432R用 
            Else
                r = Sub_SubstrateExistCheck(ObjSys)                     ' SL436R用 
            End If
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.SubstrateExistCheck() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- ###240↑-----
    '----- V6.0.3.0⑤↓ -----
#Region "QRコード読み込みの制限設定用(ローム殿特注)"
    '''=========================================================================
    ''' <summary>QRコード読み込みの制限設定用(ローム殿特注)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub btnQRLmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQRLmit.Click

        Dim ret As Long

        If btnQRLmit.BackColor = Color.Lime Then
            ' アドミニストレータレベルでログオンしていないと変更できない
            If lblLoginResult.Visible = True Then
                ret = Password1.ShowDialog(gSysPrm.stTMN.giMsgTyp, gSysPrm.stSYP.gsLoginPassword)
                If ret = 1 Then
                Else
                    Return
                End If
            Else
                If gSysPrm.stTMN.giMsgTyp = 0 Then
                    ret = MsgBox("起動時パスワードを入力しないと変更できません。", vbOKOnly)
                Else
                    ret = MsgBox("Cannot change , because not input initial password. ", vbOKOnly)
                End If

                Return
            End If

            btnQRLmit.BackColor = System.Drawing.SystemColors.Control
        Else
            ' 
            If gSysPrm.stTMN.giMsgTyp = 0 Then
                ret = MsgBox("QR制限をして良いですか？", vbYesNo)
            Else
                ret = MsgBox("Do you set limit for QR-Read?", vbYesNo)
            End If
            If ret = vbNo Then
                Return
            End If

            btnQRLmit.BackColor = Color.Lime
        End If

    End Sub
#End Region
    '----- V6.0.3.0⑤↑ -----
    '----- V6.0.3.0_30↓ -----
#Region "QR制限の条件を満たしているかの判定を行う(ローム殿特注)"
    '''=========================================================================
    ''' <summary>QR制限の条件を満たしているかの判定を行う(ローム殿特注)</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function ChkQRLimitStatus() As Integer

        ChkQRLimitStatus = cFRS_NORMAL

        If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return (cFRS_NORMAL) ' ローム殿特注でなければNOP

        If CbDigSwL.SelectedIndex <= TRIM_MODE_FT Then                       ' x0,x1,x2モード時チェック
            If QR_Read_Flg = 0 Then

                If btnQRLmit.BackColor = Color.Lime Then
                    If gSysPrm.stTMN.giMsgTyp = 0 Then
                        MsgBox("QRコードを読み込んでいないので自動運転を開始できません。")
                    Else
                        MsgBox("Cannot Auto Operation , Because not read Qr-Code. ")
                    End If
                    ChkQRLimitStatus = cFRS_ERR_RST

                End If

            End If
        End If

    End Function
#End Region
    '----- V6.0.3.0_30↑ -----
    '----- V6.0.3.0④↓ -----
#Region "バックアップ用ファイルの作成(ローム殿特注)"
    '''=========================================================================
    ''' <summary>バックアップ用ファイルの作成(ローム殿特注)</summary>
    ''' <param name="OrgFullPath">(INP)ファイル名(Full Path)</param>
    '''<returns>バックアップ用ファイル名(Full Path)</returns>
    '''=========================================================================
    Private Function MakeBackupFile(ByVal OrgFullPath As String) As String

        Dim DateStr As String
        Dim RetStr As String
        Dim OrgFileName As String
        Dim OrgFolderName As String
        Dim FLCondFile As String

        Try

            MakeBackupFile = ""

            OrgFileName = System.IO.Path.GetFileName(OrgFullPath)
            OrgFolderName = System.IO.Path.GetDirectoryName(OrgFullPath)
            DateStr = Now.ToString("yyyyMMdd") + "_" + Now.ToString("HHmmss") + OrgFileName
            RetStr = DATA_DIR_PATH + "\" + Now.ToString("yyyyMMdd") + "\"
            If System.IO.Directory.Exists(RetStr) = False Then
                MkDir(RetStr)
            End If
            MakeBackupFile = RetStr + DateStr
            FileSystem.FileCopy(OrgFullPath, MakeBackupFile)            ' ファイルがあっても上書きされる

            OrgFileName = System.IO.Path.ChangeExtension(OrgFullPath, "xml")
            FLCondFile = System.IO.Path.ChangeExtension(MakeBackupFile, "xml")

            FileSystem.FileCopy(OrgFileName, FLCondFile)                ' ファイルがあっても上書きされる

        Catch ex As Exception
            MakeBackupFile = ""

            Dim strMSG As String
            strMSG = "MakeBackupFile() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

        Exit Function

    End Function
#End Region
    '----- V6.0.3.0④↑ -----

    '========================================================================================
    '   タイマーイベント処理
    '========================================================================================
#Region "周期起動タイマー処理"
    '''=========================================================================
    '''<summary>周期起動タイマー処理</summary>
    '''<remarks>ホストコマンドの取得と処理
    '''         START SW押下・デジスイッチの取得と処理
    '''         非常停止等のチェック</remarks>
    '''=========================================================================
    Private Sub Timer1_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Timer1.Tick

        Dim r As Integer
        Dim fStartTrim As Boolean
        Dim fClamp As Boolean
        Dim strHead As String
        Dim intRet As Short
        Dim swStatus As Integer
        Dim interlockStatus As Integer
        Dim sldCvrSts As Integer
        fClamp = False
        Dim InterlockSts As Integer     ' ###006
        Dim Sw As Long                  ' ###006
        Dim coverSts As Long            ' ###109
        Dim iRtn As Integer             'V5.0.0.6②

        '---------------------------------------------------------------------------
        '   初期処理
        '---------------------------------------------------------------------------
        Timer1.Enabled = False                              ' 監視タイマー停止
        strHead = ""

        Try

            ' ｱｲﾄﾞﾙ/LOAD/SAVE/EDIT/LOTCHGｺﾏﾝﾄﾞ以外の場合(OCX使用ｺﾏﾝﾄﾞ)はﾀｲﾏｰ停止してそのまま抜けて
            '' OCXから返ってきたら監視ﾀｲﾏｰを開始する
            'If (giAppMode <> APP_MODE_IDLE) And _
            '   (giAppMode <> APP_MODE_LOAD) And _
            '   (giAppMode <> APP_MODE_SAVE) And _
            '   (giAppMode <> APP_MODE_LOGGING) And _
            '   (giAppMode <> APP_MODE_EDIT) Then
            '    Call ZCONRST()                                  ' コンソールキーラッチ解除
            '    Exit Sub
            'End If
            '###135 APP_MODE_AUTO追加
            If (giAppMode <> APP_MODE_IDLE) And
               (giAppMode <> APP_MODE_LOAD) And
               (giAppMode <> APP_MODE_SAVE) And
               (giAppMode <> APP_MODE_AUTO) And
               (giAppMode <> APP_MODE_LOGGING) Then
                Call ZCONRST()                                  ' コンソールキーラッチ解除
                Exit Sub
            End If

            '---------------------------------------------------------------------------
            '   監視処理開始
            '---------------------------------------------------------------------------
            fStartTrim = False                                  ' スタートTRIMフラグ=OFF

            ' 非常停止等チェック(トリマ装置アイドル中)
            r = System1.Sys_Err_Chk_EX(gSysPrm, giAppMode)
            If (r <> cFRS_NORMAL) Then                          ' 非常停止等検出 ?
                GoTo TimerErr                                   ' アプリ強制終了
            End If
            '---------------------------------------------------------------------------
            '   HALTスイッチ制御
            '   →(2012/01/27)
            '   　TRIMMING実施中などにHALTスイッチを押下された場合、ソフトで状態変化を
            '   　監視することは不可能。よって、ADJと同様の動作となるようにハード変更が必要。
            '   　ハード変更を実施した場合、下記の処理は不要となるためコメント化する。
            '---------------------------------------------------------------------------
            '' '' ''r = HALT_SWCHECK(swStatus)
            '' '' ''If (r <> cFRS_NORMAL) Then
            '' '' ''    GoTo TimerErr
            '' '' ''End If
            '' '' ''If (swStatus = cSTS_HALTSW_ON) Then
            '' '' ''    If (m_keepHaltSwSts = False) Then
            '' '' ''        'HALT LAMPをオンする。  
            '' '' ''        r = LAMP_CTRL(LAMP_HALT, True)
            '' '' ''        m_keepHaltSwSts = True
            '' '' ''    Else
            '' '' ''        'HALT LAMPをオフする。  
            '' '' ''        r = LAMP_CTRL(LAMP_HALT, False)
            '' '' ''        m_keepHaltSwSts = False
            '' '' ''    End If
            '' '' ''End If
            '' '' ''If (r <> cFRS_NORMAL) Then
            '' '' ''    GoTo TimerErr
            '' '' ''End If

            '---------------------------------------------------------------------------
            '   インターロック状態取得
            '---------------------------------------------------------------------------
            r = DispInterLockSts()                                  ' インターロック状態の表示/非表示および取得
            ' インターロック全解除/一部解除で、カバー閉は異常とする(SL436R時) ###109
            If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then 'V4.0.0.0⑭
                'If (r <> INTERLOCK_STS_DISABLE_NO) And (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) And (gKeiTyp <> KEY_TYPE_RS) Then
                r = COVER_CHECK(coverSts)                           ' 固定カバー状態取得(0=固定カバー開, 1=固定カバー閉))
                If (coverSts = 1) Then                              ' 固定カバー閉 ?
                    ' ハードウェアエラー(カバースイッチオン)メッセージ表示
                    Call System1.Form_Reset(cGMODE_ERR_HW, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                    GoTo TimerErr                                   ' アプリ強制終了
                End If
            End If

            If gKeiTyp = KEY_TYPE_RS Then
                SimpleTrimmer.CommandButtonTutorial()               ' V2.0.0.0⑩ コマンドボタンのチュートリアル
            End If
            '-----------------------------------------------------------------------
            '   ローダ自動ならローダからデータを入力する（コマンド受信）(※SL432R系時)
            '-----------------------------------------------------------------------
            Call GetLoaderIO(gLdRDate)                              ' ローダからデータを入力する(モニタ用)
            ' SL432系のみ下記を行う
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (giAppMode = APP_MODE_IDLE) Then
                ' ローダ自動でローダ有りからローダ無しの変化はエラーとする
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = False) Then
                    ' ローダが手動&停止に切り替わるまで待つ
                    intRet = System1.Form_Reset(cGMODE_LDR_ERR, gSysPrm, giAppMode, gbInitialized,
                                        typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                    If (intRet <= cFRS_ERR_EMG) Then GoTo TimerErr ' 非常停止等のエラーならアプリ強制終了
                End If

                ' ローダからデータを入力する
                r = System1.ReadHostCommand(gSysPrm, giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag)
                'r = System1.ReadHostCommand_ForVBNET(giHostMode, gbHostConnected, giHostRun, giAppMode, gLoadDTFlag)

                ' ローダ自動時で動作中はボタン非活性化 ###035
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = True) Then
                    Call Form1Button(0)                             ' コマンドボタンを非活性化にする
                    '----- V6.1.4.0_40↓(KOA EW殿SL432RD対応)【ロット切替え機能】-----
                    If (giLotChange = 1) Then                       ' ロット切替え機能有効 ?
                        If frmAutoObj.GetAutoOpeCancelStatus() Then ' 自動運転中止 ?
                            '' 「トリマ動作中(BIT0)」をOFFしてからでないとcmd_ZAtldGet()でローダ自動(BIT1)のままとなる V6.1.4.0_45
                            'Call SetLoaderIO(0, COM_STS_TRM_STATE)  ' ローダ出力(ON=なし, OFF=トリマ動作中) V6.1.4.0_45
                            ' "ローダ信号が自動です", "ローダを手動に切り替えてください", "Cancelボタン押下でプログラムを終了します"
                            r = Me.System1.Form_Reset(cGMODE_LDR_CHK, gSysPrm, giAppMode, gbInitialized, 0, 0, gLoadDTFlag)
                            If (r <> cFRS_NORMAL And r <> cFRS_ERR_RST) Then ' 非常停止等のエラー ?
                                'MsgBox("i-TKY.Form_Initialize()::Form_Reset(cGMODE_LDR_CHK) error." + vbCrLf + " Err Code = " + r.ToString)
                                'End                                ' アプリ終了
                                GoTo TimerErr                       ' エラーならアプリ強制終了へ
                            ElseIf (r = cFRS_ERR_RST) Then          ' RESET(Cancelボタン)押下 ? 
                                If frmAutoObj.gbFgAutoOperation432 = True Then
                                    ' 連続自動運転終了処理(自動運転終了強制終了時に生産管理データが出力されなかったので追加)
                                    Call frmAutoObj.AutoOperationEnd()
                                End If
                                GoTo TimerErr                       ' アプリ強制終了へ
                            End If
                            ' 正常時
                            Call SetLoaderIO(0, COM_STS_TRM_STATE)  ' ローダ出力(ON=なし, OFF=トリマ動作中)
                        End If
                    End If
                    '----- V6.1.4.0_40↑ -----
                Else
                    Call Form1Button(1)                             ' コマンドボタンを有効にする
                    '----- V6.1.4.0⑩↓(KOA EW殿SL432RD対応)【ロット切替え機能】-----
                    If (giLotChange = 1) Then                       ' ロット切替え機能有効 ?
                        If (frmAutoObj.gbFgAutoOperation432 = True) Then    ' 自動運転中 ?
                            Call frmAutoObj.AutoOperationEnd()              ' 連続自動運転終了処理
                            ' ローダ出力(ON=なし, OFF=トリマ動作中, NG, パターンNG)
                            Call SetLoaderIO(0, COM_STS_TRM_STATE Or COM_STS_TRM_NG Or COM_STS_PTN_NG)
                        End If
                    End If
                    '----- V6.1.4.0⑩↑ -----
                End If
                'SimpleTrimmer.CommandButtonTutorial()               ' V2.0.0.0⑩ コマンドボタンのチュートリアル

                '-----------------------------------------------------------------------
                '   ローダから受信したデータをチェックする
                '-----------------------------------------------------------------------
                If gbHostConnected = True And r >= 0 Then           ' ホスト接続でデータ受信 ?
                    If giHostMode = cHOSTcMODEcAUTO Then            ' ﾛｰﾀﾞ = 自動ﾓｰﾄﾞ ?
                        Select Case r
                            ' ﾄﾘﾏｰｽﾀｰﾄ信号受信時
                            'Case LINP_TRM_START                            'V6.1.4.0⑩
                            ' トリマースタート信号またはロット切替え信号(特注)受信時
                            Case LINP_TRM_START, LINP_TRM_LOTCHANGE_START   'V6.1.4.0⑩

                                ' @@@888 ↓
                                If (frmAutoObj.gbFgAutoOperation432 = False) Then    ' 自動運転中でないのに、ローダからSTART/Lot切り替え信号を受信した

                                    Exit Select
                                End If
                                ' @@@888 ↑


                                fStartTrim = True                   ' スタートTRIMフラグ=ON
                                '----- V6.1.4.0⑩↓(KOA EW殿SL432RD対応【ロット切替え機能】) -----
                                If (giLotChange = 1) Then           ' ロット切替え機能有効 ?
                                    If (r = LINP_TRM_LOTCHANGE_START) Then                  ' 「ロット切替え信号」受信 ?
                                        If (frmAutoObj.gbFgAutoOperation432 = True) Then    ' 自動運転中 ?
                                            If frmAutoObj.LotChangeExecute() Then           ' ロット切替え処理 
                                                fStartTrim = True                           ' スタートTRIMフラグ=ON
                                                ' V6.1.4.0_35↓
                                                ' レーザーパワーのモニタリングフラグ設定(0：無し,1：自動運転開始時,2：エントリーロット毎)
                                                If (giLaserrPowerMonitoring = 2) Then       ' エントリーロット毎にモニタリング ?
                                                    gbLaserPowerMonitoring = True           ' モニタリングフラグ ON
                                                Else
                                                    gbLaserPowerMonitoring = False          ' モニタリングフラグ OFF
                                                End If
                                                ' V6.1.4.0_35↑
                                                'V6.1.4.2①↓
                                                If Integer.Parse(GetPrivateProfileString_S("SPECIALFUNCTION", "AUTO_CALIBRATION_LOTCHANGE_EXEC", "C:\TRIM\tky.ini", "0")) = 1 Then
                                                    gbAutoCalibrationExecute = True
                                                End If
                                                'V6.1.4.2①↑
                                            Else
                                                fStartTrim = False                          ' スタートTRIMフラグ=OFF
                                                frmAutoObj.SetAutoOpeCancel()               ' ローダ出力(ON=トリミングＮＧ, OFF=なし)トリミングＮＧ信号を連続自動運転中止通知に使用
                                                ' "ロット切り替え信号を受けましたが、次のロットはエントリーされていません。"
                                                Call Z_PRINT(MSG_166 & vbCrLf)
                                            End If
                                        End If
                                    End If
                                End If
                                '----- V6.1.4.0⑩↑ -----

                                ' 操作ログ出力
                                Call System1.OperationLogging(gSysPrm, MSG_OPLOG_HCMD_TRMCMD, "HOSTCMD")
                                '###006 If (System1.InterLockSwRead() And BIT_INTERLOCK_DISABLE) = 0 Then
                                r = INTERLOCK_CHECK(InterlockSts, Sw)               ' インターロック状態取得
                                If (InterlockSts <> INTERLOCK_STS_DISABLE_NO) Then  ' インターロック状態(解除なし)でない ? ###108
                                    Call ZSELXYZSPD(1)              ' XYZ Slow speed
                                    gPrevInterlockSw = 1
                                Else
                                    Call ZSELXYZSPD(0)              ' XYZ Normal speed
                                    gPrevInterlockSw = 0
                                End If

                                'V5.0.0.6②↓
                            Case LINP_HST_MOVESUPPLY                ' コマンドが供給位置移動指示なら
                                If gbLoaderSecondPosition And giAppMode = APP_MODE_IDLE Then
                                    iRtn = Me.System1.EX_SLIDECOVERCHK(1)   ' スライドカバーチェックしない
                                    If (iRtn = cFRS_NORMAL) Then
                                        iRtn = Me.System1.EX_SMOVE2(gSysPrm, basTrimming.GetLoaderBordTableInPosX(), basTrimming.GetLoaderBordTableInPosY())
                                    End If
                                    If (iRtn = cFRS_NORMAL) Then
                                        Call SetLoaderIO(COM_STS_TRM_COMP_SUPPLY, 0)    ' ローダー出力(ON=供給位置移動完了)
                                        ' Call Sub_ATLDSET(COM_STS_TRM_COMP_SUPPLY, COM_STS_TRM_STATE)    ' ローダー出力(ON=供給位置移動完了,OFF=トリマ動作中)
                                    Else
                                        Call SetLoaderIO(COM_STS_TRM_ERR, 0)    ' ローダー出力(ON=トリマエラー)
                                        Call Me.System1.TrmMsgBox(gSysPrm, MSG_SPRASH64, MsgBoxStyle.OkOnly, My.Application.Info.Title)    ' "ステージ供給位置移動エラーが発生しました。"
                                        Call SetLoaderIO(0, COM_STS_TRM_ERR)    ' ローダー出力(OFF=トリマエラー)
                                    End If
                                    iRtn = Me.System1.EX_SLIDECOVERCHK(0)   ' スライドカバーチェックする。
                                    If (iRtn <> cFRS_NORMAL) Then
                                        Call Me.System1.TrmMsgBox(gSysPrm, MSG_SPRASH65, MsgBoxStyle.OkOnly, My.Application.Info.Title)     ' "スライドカバーチェック（EX_SLIDECOVERCHK(0)）設定エラーが発生しました。"
                                    End If
                                End If
                                'V5.0.0.6②↑
                        End Select
                    End If                                          ' トリマ非常停止中
                End If
            End If

            '-----------------------------------------------------------------------
            '   ローダの状態を確認する(※SL436R系時)
            '-----------------------------------------------------------------------
            ' ###073 削除
            ''If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then
            ''    Dim AlarmCount As Integer
            ''    Dim strLoaderAlarm(LALARM_COUNT) As String
            ''    Dim strLoaderAlarmInfo(LALARM_COUNT) As String
            ''    Dim strLoaderAlarmExec(LALARM_COUNT) As String

            ''    ' ローダアラームを取得
            ''    ' ローダアラームメッセージ作成 & ローダアラーム画面表示
            ''    r = Loader_AlarmCheck(gSysPrm, False, AlarmCount, strLoaderAlarm, strLoaderAlarmInfo, strLoaderAlarmExec)
            ''    If (r <> cFRS_NORMAL) Then
            ''        Dim objForm As Object = Nothing

            ''        ' frmResetを使用してエラーメッセージの表示を行う。
            ''        objForm = New FrmReset()
            ''        Call objForm.ShowDialog(Nothing, cGMODE_LDR_ALARM, Me.System1)
            ''        r = objForm.sGetReturn()                                    ' Return値取得

            ''        ' オブジェクト開放
            ''        If (objForm Is Nothing = False) Then
            ''            Call objForm.Close()                                    ' オブジェクト開放
            ''            Call objForm.Dispose()                                  ' リソース開放
            ''        End If

            ''        ' ローダアラーム検出はアプリ強制終了しない 
            ''        If (r <> cFRS_NORMAL) Then
            ''            If (r = cFRS_ERR_RST) Or (r = cFRS_ERR_LDR) Or (r = cFRS_ERR_LDR1) Or (r = cFRS_ERR_LDR2) Or (r = cFRS_ERR_LDR3) Or (r = cFRS_ERR_LDRTO) Then
            ''                Call W_RESET()                                      ' アラームリセット
            ''            Else
            ''                GoTo TimerErr                                       ' PLCステータス異常等はアプリ強制終了へ
            ''            End If
            ''        End If

            ''    End If
            ''End If

            ' '' '' '' 分布図表示(TKY/CHIPのみ)
            '' '' ''If (Me.chkDistributeOnOff.Checked = True) Then    ' 生産ｸﾞﾗﾌ表示ﾌﾗｸﾞON ?
            '' '' ''    frmDistribution.RedrawGraph()           ' ｸﾞﾗﾌ更新
            '' '' ''End If

            '---------------------------------------------------------------------------
            '   ローダーなしの場合
            '---------------------------------------------------------------------------
            If (giHostMode = cHOSTcMODEcMANUAL) Then
                'swStatus = Me.System1.InterLockSwRead()
                r = INTERLOCK_CHECK(interlockStatus, swStatus)
                If (r <> ERR_CLEAR) Then                                    ' ※メッセージ表示を追加する 
                    '筐体カバー開の場合、インターロック解除かカバー閉を監視する
                    If (r = ERR_OPN_CVR) Then
                        '----- V1.22.0.0⑨↓ -----
                        intRet = System1.Sub_CoverCheck(gSysPrm, 0, False)
                        'intRet = System1.Form_Reset(cGMODE_CVR_CLOSEWAIT, gSysPrm, giAppMode, False, _
                        '                   typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                        '----- V1.22.0.0⑨↑ -----
                        If (intRet <= cFRS_ERR_EMG) Then GoTo TimerErr ' 非常停止等のエラーならアプリ強制終了
                        'スタートスイッチラッチクリア
                        Call ZCONRST()
                        'If (intRet = ERR_OPN_CVRLTC) Then COVERLATCH_CLEAR() ' ラッチクリア
                    ElseIf (r = ERR_OPN_SCVR Or r = ERR_OPN_CVRLTC) Then
                        ' SL432Rの場合はカバー開ラッチは無視する。
                        If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL432) Then
                            '----- 'V4.0.0.0⑯↓ -----
                            intRet = System1.Sub_CoverCheck(gSysPrm, 0, False)
                            If (intRet <= cFRS_ERR_EMG) Then GoTo TimerErr ' 非常停止等のエラーならアプリ強制終了
                            '                            GoTo TimerErr
                            '----- 'V4.0.0.0⑯↑ -----
                        End If
                    Else
                        GoTo TimerErr
                    End If
                End If

                '---------------------------------------------------------------------------
                '   ＳＴＡＲＴ　ＳＷの押下チェック(SL432R/SL436R共通)
                '---------------------------------------------------------------------------
                If (giAppMode = APP_MODE_IDLE) Then         ' アイドルモード時にチェックする
                    r = START_SWCHECK(0, swStatus)          ' トリマー START SW 押下チェック
                    If (swStatus = cSTS_STARTSW_ON) Then
                        '----- V3.0.0.0④(V1.22.0.0③)↓-----
                        ' データロード済みチェック
                        If ChkTrimDataLoaded() <> cFRS_NORMAL Then
                            Timer1.Enabled = True                               ' タイマー再起動
#If START_KEY_SOFT Then
                            If gbStartKeySoft Then
                                Call ZCONRST()                                          ' ラッチ解除
                            End If
#End If
                            Exit Sub
                        End If
                        '----- V3.0.0.0④(V1.22.0.0③)↑-----
                        ''V4.0.0.0-86
                        r = GetLaserOffIO(True) 'V5.0.0.1⑫
                        If r = 1 Then
                            Call ZCONRST()                                      ' コンソールキーラッチ解除'V5.0.0.1⑪
                            Timer1.Enabled = True                               ' タイマー再起動
                            Exit Sub
                        End If
                        ''V4.0.0.0-86

                        '----- V6.0.3.0_30↓ -----
                        ' QRコードを読み込んだかチェック 
                        r = ChkQRLimitStatus()
                        If r <> cFRS_NORMAL Then
                            Timer1.Enabled = True                               ' タイマー再起動
                            Exit Sub
                        End If
                        '----- V6.0.3.0_30↑ -----

                        '----- ###240↓-----
                        ' 載物台に基板がある事をチェックする(手動モード時(OPTION))
                        r = SubstrateExistCheck(System1)
                        If (r <> cFRS_NORMAL) Then                              ' エラー ? 
                            If (r = cFRS_ERR_RST) Then                          ' 基板無し(Cancel(RESETｷｰ)　?
                                Timer1.Enabled = True                           ' タイマー再起動
                                Exit Sub
                            End If
                            GoTo TimerErr                                       ' その他のエラーならアプリ強制終了
                        End If
                        '----- ###240↑-----

                        Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "START SW ON(VAC/CLAMP ON)")
RETRY_VACUME:           ' V1.16.0.2②
                        ' クランプ/吸着ON

                        'V4.5.0.1①↓
                        '薄基板対応の場合には、クランプシーケンス変更する 
                        If (giClampSeq = 1) Then

                            Dim lData As Long = 0
                            Dim lBit As Long = 0
                            Dim rtn As Integer = cFRS_NORMAL
                            Dim strMSG As String = ""
                            Dim strMS2 As String = ""
                            Dim strMS3 As String = ""
                            Dim bFlg As Boolean = True

                            ' 載物台に基板がある事をチェックする
                            'クランプON
                            ''----- V1.16.0.0⑫↓ -----
                            'If (gSysPrm.stIOC.giClamp = 1) Then
                            '    Call W_CLMP_ONOFF(1)                                    ' クランプON
                            '    System.Threading.Thread.Sleep(gSysPrm.stIOC.glClampWait) ' Wait(ms)
                            '    Call W_CLMP_ONOFF(0)                                    ' クランプOFF
                            'End If
                            '----- V1.16.0.0⑫↑ -----
                            '吸着ON
                            Call ZABSVACCUME(1)                                         ' バキュームの制御(吸着ON)
                            System.Threading.Thread.Sleep(500)                          ' Wait(ms) ※200msだとワーク有が検出されない場合がある
                            If (giMachineKd = MACHINE_KD_RS) Then                       ' V2.0.0.0⑥
                                ' SL436S時
                                r = W_Read(LOFS_W42S, lData)                            ' 物理入力状態取得(W42.00-W42.15)
                                lBit = LDSTS_VACUME
                            Else
                                ' SL436R時
                                r = W_Read(LOFS_W44, lData)                             ' 物理入力状態取得(W44.00-W44.15)
                                lBit = LDST_VACUME
                            End If
                            ' ワーク無しならメッセージ表示
                            If (lData And lBit) Then                                    ' 載物台に基板有 ? V2.0.0.0⑥
                                r = 0
                            Else
                                ' 「固定カバー開１チェックなし」にする
                                Call COVERCHK_ONOFF(COVER_CHECK_OFF)
                                ' 表示メッセージを設定する
                                strMSG = MSG_LDALARM_06                                 ' "載物台吸着ミス"
                                strMS2 = MSG_SPRASH52                                   ' "基板を置いて再度実行して下さい"
                                strMS3 = MSG_SPRASH53                                             ' ""

                                'r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                                '        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                                ' メッセージ表示(STARTキー押下待ち)
                                r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START + cFRS_ERR_RST, True,
                                        strMSG, strMS2, strMS3, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                                If r = 1 Then ' STARTはRetry
                                ElseIf r = 3 Then
                                    ' Cancelが押されたら終了
                                    r = 2
                                End If
                            End If

                        Else
                            'Call Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)       '###060
                            r = Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)         '###060
                            If (r <> cFRS_NORMAL) Then
                                Call AppEndDataSave()                               ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                                Call AplicationForcedEnding()                       ' ｿﾌﾄ強制終了処理
                                End                                                 ' アプリ強制終了
                            End If

                            '----- V1.16.0.0⑯↓ -----
                            'r = Me.System1.AbsVaccume(gSysPrm, 1, giAppMode, giTrimErr)        ' V1.19.0.0-32
                            r = Me.System1.AbsVaccume(gSysPrm, 1, APP_MODE_TRIM, giTrimErr)     ' V1.19.0.0-32
                            'Call Me.System1.Adsorption(gSysPrm, 1)
                        End If
                        'V4.5.0.1①↑

                        Select Case r
                            Case 1                                              ' 吸着エラーでリトライ指定
                                GoTo RETRY_VACUME
                            Case 2                                              ' 吸着エラーで中止指定
                                GoTo TimerExit
                        End Select
                        '----- V1.16.0.0⑯↑ -----

                        ' スタートSWを離すまで待つ→INTRIMにて監視の無限ループ
                        Call START_SWCHECK(1, swStatus)
                        'V5.0.0.6⑲ ADD START↓
                        ' ｽﾀｰﾄSW押下待ちしない場合はメッセージ表示する
                        Call ZCONRST()                                                          ' ラッチ解除
                        If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                            r = INTERLOCK_CHECK(interlockStatus, swStatus)
                            If interlockStatus = INTERLOCK_STS_DISABLE_NO Then
                                ' "注意！！！　スライドカバーが自動で閉じます。"(Red,Blue)
                                r = Me.System1.Form_MsgDispStartReset(MSG_SPRASH31, MSG_SPRASH32, &HFF, &HFF0000)
                                Call ZCONRST()                                                  ' ラッチ解除
                                If (r = cFRS_ERR_START) Then
                                    r = Me.System1.Z_CCLOSE(gSysPrm, giAppMode, 0, False, 0.0, 0.0)
                                    If r = cFRS_NORMAL Then
                                    ElseIf r = cFRS_TO_SCVR_CL Then 'タイムアウト(スライドカバー閉待ち)
                                        GoTo TimerErr
                                    Else
                                        Timer1.Enabled = True                                   ' タイマー再起動
                                        Exit Sub
                                    End If
                                ElseIf (r = cFRS_ERR_RST) Then
                                    ' RESET SW押下なら
                                    'V6.1.4.0_42↓
                                    ' "スライドカバーを開けてください"表示後スライドカバー開待ち
                                    r = Me.System1.Form_Reset(cGMODE_OPT_END, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                                    If r <= cFRS_ERR_EMG Then
                                        GoTo TimerErr
                                    End If
                                    'V6.1.4.0_42↑ 
                                    Timer1.Enabled = True                                       ' タイマー再起動
                                    Exit Sub
                                Else
                                    GoTo TimerErr                                               ' 非常停止等
                                End If
                            End If
                        End If
                        'V5.0.0.6⑲ ADD END↑

                        'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "START SW RELEASED(XY START)")'###030
                        'V5.0.0.6⑲コメントSTART↓
                        ' SL432RでｽﾀｰﾄSW押下待ちしない場合はメッセージ表示する
                        'If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                        '    '----- V3.0.0.0⑦(V1.22.0.0⑤) ↓ -----
                        '    r = INTERLOCK_CHECK(interlockStatus, swStatus)
                        '    If (interlockStatus = INTERLOCK_STS_DISABLE_NO) Then         ' インターロック状態(解除なし) ?
                        '        ' スライドカバー自動クローズ又はSTART/RESETキー押下待ち
                        '        r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                        '        Me.Refresh()
                        '        If (r < cFRS_NORMAL) Then
                        '            Call AppEndDataSave()                       ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                        '            Call AplicationForcedEnding()               ' ｿﾌﾄ強制終了処理
                        '            End                                         ' アプリ強制終了
                        '        End If
                        '    End If
                        '    '    ' "注意！！！　スライドカバーが自動で閉じます。"(Red,Blue)
                        '    '    'r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH32, &HFF, &HFF0000)
                        '    '    ' メッセージ表示(STARTキー押下待ち)
                        '    '    r = Sub_CallFrmMsgDisp(System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        '    '            MSG_SPRASH31, MSG_SPRASH32, "", System.Drawing.Color.Red, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                        '    '    If (r < cFRS_NORMAL) Then
                        '    '        Call AppEndDataSave()                       ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                        '    '        Call AplicationForcedEnding()               ' ｿﾌﾄ強制終了処理
                        '    '        End                                         ' アプリ強制終了
                        '    '    End If

                        '    '    ' スライドカバーを自動クローズする
                        '    '    Call ZSLCOVEROPEN(0)                            ' ｽﾗｲﾄﾞｶﾊﾞｰｵｰﾌﾟﾝﾊﾞﾙﾌﾞOFF
                        '    '    Call ZSLCOVERCLOSE(1)                           ' ｽﾗｲﾄﾞｶﾊﾞｰｸﾛｰｽﾞﾊﾞﾙﾌﾞON
                        '    '----- V3.0.0.0⑦(V1.22.0.0⑤)↑ -----
                        'End If
                        'V5.0.0.6⑲コメントEND↑

                        'V4.6.0.0①↓
                        'V4.10.0.0⑩                        If (giMachineKd = MACHINE_KD_RS) And (giManualSeq = 1) Then
                        If (gMachineType = MACHINE_TYPE_436S) AndAlso (giManualSeq = 1) Then
                            '// ここStartキー待ち
                            ' 各コマンド処理実行時のｽﾗｲﾄﾞｶﾊﾞｰ自動ｸﾛｰｽﾞ又はSTART/RESETキー押下待ち
                            r = System1.Form_Reset(cGMODE_START_RESET, gSysPrm, giAppMode, gbInitialized, typPlateInfo.dblZWaitOffset, typPlateInfo.dblZOffSet, gLoadDTFlag)
                            Me.Refresh()
                            'スタートスイッチラッチクリア
                            Call ZCONRST()
                            'V5.0.0.1⑰↓
                            If (r <= cFRS_ERR_EMG) Then
                                Call W_CLMP_ONOFF(0)                                    ' クランプOFF
                                Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着OFF)
                                '    GoTo TimerExit
                                GoTo TimerErr                                       ' その他のエラーならアプリ強制終了
                            ElseIf (r = cFRS_ERR_RST) Then                     ' ｴﾗｰ(非常停止等)ならｿﾌﾄ強制終了
                                Call W_CLMP_ONOFF(0)                                    ' クランプOFF
                                Call ZABSVACCUME(0)                                         ' バキュームの制御(吸着OFF)
                                GoTo TimerExit
                            End If
                            'V5.0.0.1⑰↑
                        End If
                        'V4.6.0.0①↑

                        fStartTrim = True                                       ' スタートTRIMフラグON
                        stPRT_ROHM.bAutoMode = False                            ' トリミング結果印刷データに手動運転を設定(ローム殿特注) V1.18.0.0③
                        gbLaserPowerMonitoring = False                          ' 手動時はフルパワー測定を行わない　V6.1.4.6①
                    Else
                        '---------------------------------------------------------------------------
                        '   スライドカバー状態のチェック(SL432R時)
                        '---------------------------------------------------------------------------
                        If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (interlockStatus = INTERLOCK_STS_DISABLE_NO) Then
                            ' スライドカバーの状態取得（INTRIMではIO取得のみの為、エラーが返る事はない）
                            r = SLIDECOVER_GETSTS(sldCvrSts)

                            ' スライドカバー状態のチェック
                            If (sldCvrSts = SLIDECOVER_MOVING) And (fClamp = False) Then
                                '----- ###240↓-----
                                ' 載物台に基板がある事をチェックする(手動モード時(OPTION))
                                r = SubstrateExistCheck(System1)
                                If (r <> cFRS_NORMAL) Then                              ' エラー ? 
                                    If (r = cFRS_ERR_RST) Then                          ' 基板無し(Cancel(RESETｷｰ)　?
                                        Timer1.Enabled = True                           ' タイマー再起動
                                        Exit Sub
                                    End If
                                    GoTo TimerErr                                       ' その他のエラーならアプリ強制終了
                                End If
                                '----- ###240↑-----

                                ' スライドカバー中間、クランプOFFの場合：クランプをONする。
                                'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "SLIDE COVER CLOSING(CLAMP/VAC ON)")'###030
                                ' クランプ/吸着ON
                                'Call Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)       '###060
                                r = Me.System1.ClampCtrl(gSysPrm, 1, giTrimErr)         '###060
                                If (r <> cFRS_NORMAL) Then
                                    Call AppEndDataSave()                               ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                                    Call AplicationForcedEnding()                       ' ｿﾌﾄ強制終了処理
                                    End                                                 ' アプリ強制終了
                                End If
                                Call Me.System1.AbsVaccume(gSysPrm, 1, giAppMode, giTrimErr)
                                Call Me.System1.Adsorption(gSysPrm, 1)
                                fClamp = True
                                'ElseIf (sldCvrSts = SLIDECOVER_OPEN) And (fClamp = True) Then  '###020 
                            ElseIf (sldCvrSts = SLIDECOVER_OPEN) Then                           '###020 
                                ' スライドカバーがオープン状態で、クランプONの場合：クランプをOFFする。
                                'Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "SLIDE COVER reOPEN(CLAMP/VAC OFF)")'###030
                                fClamp = False
                                ' クランプ/吸着OFF
                                'Call Me.System1.ClampCtrl(gSysPrm, 0, giTrimErr)       '###060
                                r = Me.System1.ClampCtrl(gSysPrm, 0, giTrimErr)         '###060
                                If (r <> cFRS_NORMAL) Then
                                    Call AppEndDataSave()                               ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
                                    Call AplicationForcedEnding()                       ' ｿﾌﾄ強制終了処理
                                    End                                                 ' アプリ強制終了
                                End If
                                Call Me.System1.AbsVaccume(gSysPrm, 0, giAppMode, giTrimErr)
                                Call Me.System1.Adsorption(gSysPrm, 0)
                            ElseIf (sldCvrSts = SLIDECOVER_CLOSE) Then
                                ' スライドカバー閉
                                fClamp = False
                                Call System1.OperationLogging(gSysPrm, MSG_OPLOG_TRIMST, "SLIDE COVER CLOSED")
                                fStartTrim = True           ' スタートTRIMフラグON
                                gbLaserPowerMonitoring = False                          ' 手動時はフルパワー測定を行わない　V6.1.4.6①
                            End If
                        End If
                    End If
                End If
            End If
#If 0 = cOFFLINEcDEBUG Then     'V4.0.0.0-38
            If gKeiTyp = KEY_TYPE_RS Then               'V2.0.0.0⑩ シンプルトリマ
                SimpleTrimmer.ElapsedTimeUpdate()       'V2.0.0.0⑩ 経過時間の更新
            End If                                      'V2.0.0.0⑩
#End If
            '--------------------------------------------------------------------------
            '   トリミングを実行する(スタートTRIMフラグがONの場合)
            '--------------------------------------------------------------------------
            If fStartTrim = True Then                                           ' スタートTRIMフラグON ?
#If START_KEY_SOFT Then
                If gbStartKeySoft Then
                    Call ZCONRST()                                              ' ラッチ解除
                End If
#End If
                '----- V6.0.3.0③↓ -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    TimerQR.Enabled = False                                     ' 監視タイマー停止
                    ' ポートクローズ(QRコード受信用 ローム殿特注)
                    Call QR_Rs232c_Close()
                End If
                '----- V6.0.3.0③↑ -----

                ' 'V6.1.4.15①↓
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = True) Then
                Else
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_MANUAL)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧
                End If
                ' 'V6.1.4.15①↑
                'V5.0.0.9⑯              ↓
                If (1 < giStartBlkAss) Then
                    If (False = chkContinue.Checked) Then
                        CbStartBlkX.SelectedIndex = 0
                        CbStartBlkY.SelectedIndex = 0
                    End If
                    chkContinue.Enabled = False

                    If (KEY_TYPE_RS <> gKeiTyp) Then
                        GrpStartBlk.Visible = False
                    End If
                End If
                'V5.0.0.9⑯              ↑

                ' トリミングを実行する
                r = Start_Trimming()

                'V5.0.0.9⑯↓
                If (1 < giStartBlkAss) Then
                    chkContinue.Enabled = True
                    GrpStartBlk.Visible = True
                End If
                'V5.0.0.9⑯↑

                ' 'V6.1.4.15①↓
                If (giHostMode = cHOSTcMODEcAUTO) And (gbHostConnected = True) Then
                Else
                    'V5.0.0.9⑭ ↓ V6.0.3.0⑧
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                    'V5.0.0.9⑭ ↑ V6.0.3.0⑧
                End If
                ' 'V6.1.4.15①↑

                '----- V6.0.3.0③↓ -----
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                    ' V6.0.3.0_32 TrimData.SetLotChange()
                    Call QR_Rs232c_Open()
                    TimerQR.Enabled = True                                  ' 監視タイマー開始
                End If
                '----- V6.0.3.0③↑ -----

                ' '' '' ''HALTランプ状態保持変数を初期化
                '' '' ''m_keepHaltSwSts = False
                If (r = cFRS_ERR_EMG) Then
                    GoTo TimerErr
                End If

                '----- V6.1.4.0⑨↓(KOA EW殿SL432RD対応) -----
                ' 連続トリミングＮＧ枚数のチェック(自動運転中)
                If frmAutoObj.gbFgAutoOperation432 = True And typPlateInfo.intTrimNgCount > 0 And m_lTrimNgCount >= typPlateInfo.intTrimNgCount Then
                    ' シグナルタワー制御(赤点滅+ブザー１)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_ALARM)
                    Call SetLoaderIO(COM_STS_TRM_STATE, &H0)            ' ローダ出力(ON=トリマ動作中, OFF=なし)
                    ' メッセージ表示してSTARTキー押下待ち "ＮＧ品が指定された枚数連続して発生","",""
                    r = Sub_CallFrmMsgDisp(Me.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True,
                            MSG_LOADER_54, "", "", System.Drawing.Color.Red, System.Drawing.Color.Red, System.Drawing.Color.Red)
                    If (r < cFRS_NORMAL) Then
                        GoTo TimerErr
                    End If
                    ' シグナルタワー制御初期化(全ﾋﾞｯﾄｵﾌ)
                    Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_ALL_OFF)
                    frmAutoObj.SetAutoOpeCancel()                       ' ローダ出力(連続自動運転中断)
                    Call Me.Refresh()
                    GoTo TimerExit
                End If
                '----- V6.1.4.0⑨↑ -----

                ' ローダ出力(トリマ停止中)(SL432R時) ###035
                If (gSysPrm.stTMN.gsKeimei <> MACHINE_TYPE_SL436) Then
                    Call SetLoaderIO(0, COM_STS_TRM_STATE)                  ' ローダ出力(ON=なし, OFF=トリマ動作中)
                End If

                '画面表示の更新
                Call Me.Refresh()
            Else
                '--------------------------------------------------------------------------
                '   スタートTRIMフラグがOFFなら、以下の処理を行う
                '--------------------------------------------------------------------------
                ' マニュアルローダでRESET SW押下時は「原点復帰処理」を行う(アイドルモード時にチェックする)
                If (giAppMode = APP_MODE_IDLE) Then
                    STARTRESET_SWCHECK(1, swStatus)
                    If giHostMode = cHOSTcMODEcMANUAL And swStatus = cSTS_RESETSW_ON Then
                        gbInitialized = False
                        'Call System1.sLampOnOff(LAMP_RESET, True)   ' RESETランプON
                        Call LAMP_CTRL(LAMP_RESET, True)   ' RESETランプON
                        ' 原点復帰
                        intRet = sResetStart()
                        If (intRet <= cFRS_ERR_EMG) Then            ' 非常停止等ならアプリ強制終了
                            Call AppEndDataSave()
                            Call AplicationForcedEnding()           ' 強制終了
                        End If

                        ' V6.0.3.0⑧(ローム殿版ではここに追加されている)
                        Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
                        ' V6.0.3.0⑧

                        ' 内部カメラへ切り替え
                        Call VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)

                        ' インターロック状態の表示/非表示
                        Call DispInterLockSts()

                        gManualThetaCorrection = True               ' シータ補正実行フラグ = True(シータ補正を実行する)
                    End If
                End If
            End If

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
TimerExit:
            '強制的にメモリを開放
            '   デバッグ時のみ使用。高付加のため明示的には実行しない。
#If DEBUG Then
            GC.Collect()
#End If

            ' タイマー再起動
            Timer1.Enabled = True                               ' 監視タイマー開始
            Exit Sub

            '---------------------------------------------------------------------------
            '   アプリ強制終了
            '---------------------------------------------------------------------------
TimerErr:
            ' エラーコードの表示
            '   INTRIMのエラーコードがそのまま返ってきている場合（DllTrimFuncのIFを直接コールした場合、
            '   エラーメッセー時の表示がされていないため、メッセージをココで表示
            If (r >= ERR_INTIME_BASE) Then  'INTRIMからのエラーコードは100番以降となる
                Call System1.Form_AxisErrMsgDisp(r)
            End If
            Call AppEndDataSave()                               ' ｿﾌﾄ強制終了時のﾃﾞｰﾀ保存確認
            Call AplicationForcedEnding()                       ' ｿﾌﾄ強制終了処理
            End                                                 ' アプリ強制終了

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "Timer1.Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub

#End Region

#Region "インターロックステータス監視タイマー処理(自動運転時 SL436R)"
    '''=========================================================================
    '''<summary>インターロックステータス監視タイマー処理(自動運転時 SL436R) ###178</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub TimerInterLockSts_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TimerInterLockSts.Tick

        Dim r As Integer

        TimerInterLockSts.Enabled = False                               ' 監視タイマー停止
        r = DispInterLockSts()                                          ' インターロック状態の表示/非表示および取得
        TimerInterLockSts.Enabled = True                                ' 監視タイマー開始

    End Sub
#End Region

#Region "QRコード受信チェックタイマー処理(ローム殿特注)"
    '''=========================================================================
    ''' <summary>QRコード受信チェックタイマー処理(ローム殿特注)</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>V1.18.0.0②</remarks>
    '''=========================================================================
    Private Sub TimerQR_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerQR.Tick

        Dim r As Integer
        Dim strMSG As String = ""
        Dim QR_Data As String = ""
        Dim BackupFile As String                                        ' V6.0.3.0④

        Try
            ' QRデータ受信チェック
            TimerQR.Enabled = False                                     ' 監視タイマー停止
            r = QR_GetReceiveData(QR_Data)
            If (r <> cFRS_NORMAL) Then
                '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
                ' シリアルポートオープンエラーならタイマー停止のまま抜ける(ｵｰﾌﾟﾝｴﾗｰﾒｯｾｰｼﾞが表示されつづける)
                If (giQrCodeType = QrCodeType.KoaEw) And (QR_Rs_Flag = 0) Then
                    Exit Sub
                End If
                '----- V6.1.4.0_22↑ -----
            Else
                ' トリマ装置アイドル中でないならNOP(受信データは捨てる)
                If (giAppMode <> APP_MODE_IDLE) Then
                    QR_Read_Flg = 0
                    TimerQR.Enabled = True                              ' 監視タイマー開始
                    Exit Sub
                End If

                ' 受信データをログ画面に表示
                If (QR_Data <> "") Then
                    strMSG = "QR Code Receive=" + QR_Data
                    '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
                    Debug.WriteLine(strMSG)
                    If (giQrCodeType = QrCodeType.KoaEw) Then
                        Call Me.System1.OperationLogging(gSysPrm, strMSG, "QRCODE")
                    Else
                        Call Z_PRINT(strMSG)
                    End If
                    '----- V6.1.4.0_22↑ -----
                End If

                '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
                If (giQrCodeType = QrCodeType.KoaEw) AndAlso (gTkyKnd = KND_CHIP) Then               'V6.1.4.10②　(gTkyKnd = KND_CHIP)　判定追加
                    ' ＱＲコードリーダから受信したデータを処理する
                    Timer1.Stop()                                      'V6.1.4.14④
                    Call ObjQRCodeReader.QRCodeDataExecute(QR_Data)
                    TimerQR.Enabled = True                              ' 監視タイマー開始
                    Call ZCONRST()                                      'V6.1.4.14④スタートスイッチラッチクリア
                    Timer1.Start()                                      'V6.1.4.14④
                    Exit Sub
                End If
                '----- V6.1.4.0_22↑ -----

                'V6.1.4.10②
                If (giQrCodeType = QrCodeType.KoaEw) AndAlso (gTkyKnd = KND_NET) Then
                    ' ＱＲコードリーダから受信したデータを処理する
                    Timer1.Stop()                                      'V6.1.4.14④
                    Call ObjQRCodeReader.QRCodeDataExecuteForTKYNET(QR_Data)
                    TimerQR.Enabled = True                              ' 監視タイマー開始
                    Call ZCONRST()                                      'V6.1.4.14④スタートスイッチラッチクリア
                    Timer1.Start()                                      'V6.1.4.14④
                    Exit Sub
                End If
                'V6.1.4.10②

                ' QRコード受信データから指定の文字列を編集しファイルパスを作成する
                strQRLoadFileFullPath = GetQrCodeFileName(QR_Data)
                If (strQRLoadFileFullPath = "") Then                    ' ファイルが存在しない ? 
                    QR_Read_Flg = 0
                Else
                    ' V6.0.3.0④ トリミングデータバックアップ保存 
                    BackupFile = MakeBackupFile(strQRLoadFileFullPath)
                    ' 指定のファイルをロードする(読み込むのはマスターのファイルとする) V6.0.3.0_34
                    Call DataLoadQR(strQRLoadFileFullPath)
                    '----- V6.0.3.0④↓ -----
                    typPlateInfo.strDataName = strQRLoadFileFullPath    ' プレートデータのトリミングデータファイル名更新
                    ' データファイルの書込み失敗時
                    If (r <> cFRS_NORMAL) Then
                        strMSG = "Fle Save Error =" + QR_Data
                        Call Z_PRINT(strMSG)
                    End If
                    '----- V6.0.3.0④↑ -----
                    '----- V6.0.3.0⑨↓ -----
                    If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
                        ClearDispData()                                 ' V6.0.3.0_32
                        TrimData.SetLotChange()                         ' ロット情報クリア
                    End If
                    '----- V6.0.3.0⑨↑ -----
                    QR_Read_Flg = 1                                     ' QRｺｰﾄﾞ読み込み判定(0)NG (1)OK
                End If
            End If

            TimerQR.Enabled = True                                      ' 監視タイマー開始
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "QR_Data.TimerQR_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "バーコード受信チェックタイマー処理"
    '''=========================================================================
    ''' <summary>バーコード受信チェックタイマー処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>V1.23.0.0①</remarks>
    '''=========================================================================
    Private Sub TimerBC_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerBC.Tick

        Dim r As Integer
        Dim strMSG As String = ""

        Try
            ' バーコードデータ受信チェック
            TimerBC.Enabled = False                                     ' 監視タイマー停止
            r = BC_GetReceiveData(BC_Data)
            If (r <> cFRS_NORMAL) Then

            Else
                ' トリマ装置アイドル中でないならNOP(受信データは捨てる)
                If (giAppMode <> APP_MODE_IDLE) Then
                    BC_Read_Flg = 0
                    TimerBC.Enabled = True                              ' 監視タイマー開始
                    Exit Sub
                End If

                ' 受信データをログ画面に表示
                If (BC_Data <> "") Then                                 ' バーコードデータ受信 ? 
                    'V5.0.0.9⑲                    strMSG = "Bar Code Receive=" + BC_Data
                    strMSG = String.Format("Barcode{0} Receive={1}",
                        If((BarcodeType.Taiyo = BarCode_Data.Type), (BarCode_Data.BC_ReadCount + 1), ""),
                        BarCode_Data.BC_Data)                           'V5.0.0.9⑲

                    Call Z_PRINT(strMSG)
                Else
                    GoTo STP_END
                End If

                'V5.0.0.9⑲                ' 編集中のデータがあるかチェックする
                'V5.0.0.9⑲                If (gCmpTrimDataFlg = 1) Then                           ' データ更新フラグ = 1(更新あり))
                'V5.0.0.9⑲                    r = MsgBox(MSG_115, MsgBoxStyle.OkCancel, MSG_116)  ' "編集中のデータがあります。ロードしますか？"
                'V5.0.0.9⑲                    If (r = MsgBoxResult.Cancel) Then                   ' Cansel指定 ?
                'V5.0.0.9⑲                        GoTo STP_END
                'V5.0.0.9⑲                    End If
                'V5.0.0.9⑲                    gCmpTrimDataFlg = 0                                 ' データ更新フラグ = 0(更新なし))
                'V5.0.0.9⑲                End If

                ' バーコード受信データから指定の文字列を編集しファイルパスを作成する
                strBCLoadFileFullPath = GetBarCodeFileName(BC_Data)
                'V5.0.0.9⑲              ↓
                If (strBCLoadFileFullPath Is Nothing) Then
                    ' 太陽社１度目の受信時
                    TimerBC.Enabled = True                              ' 監視タイマー開始
                    Exit Sub
                End If
                'V5.0.0.9⑲              ↑
                If (strBCLoadFileFullPath = "") Then                    ' ファイルが存在しない ? 
                    BC_Read_Flg = 0
                    ' トリミングデータを未ロードとする(自動運転を前のデータで実行させないため)
                    gStrTrimFileName = ""
                    LblDataFileName.Text = ""
                    gLoadDTFlag = False                                 ' gLoadDTFlag = データ未ロード
                    SetMapOnOffButtonEnabled(False)                     'V4.12.2.0①
                    SetTrimMapVisible(False)                            'V4.12.2.0①
                    Call BC_Info_Disp(0)                                ' バーコード情報表示をクリアする
                    BarCode_Data.BC_Data = ""                           'V5.0.0.9⑮
                Else
                    'V5.0.0.9⑲          ↓
                    ' 編集中のデータがあるかチェックする
                    If (gCmpTrimDataFlg = 1) Then                           ' データ更新フラグ = 1(更新あり))
                        r = MsgBox(MSG_115, MsgBoxStyle.OkCancel, MSG_116)  ' "編集中のデータがあります。ロードしますか？"
                        If (r = MsgBoxResult.Cancel) Then                   ' Cansel指定 ?
                            GoTo STP_END
                        End If
                        gCmpTrimDataFlg = 0                                 ' データ更新フラグ = 0(更新なし))
                    End If
                    'V5.0.0.9⑲          ↑

                    ' 指定のファイルをロードする
                    r = DataLoadBC(strBCLoadFileFullPath)
                    If (r = cFRS_NORMAL) Then
                        BC_Read_Flg = 1
                    Else
                        BC_Read_Flg = 0
                    End If
                End If
            End If
STP_END:
            TimerBC.Enabled = True                                      ' 監視タイマー開始
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.TimerBC_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

    ''' <summary>バーコード関連コントロール表示・接続をおこなう</summary>
    ''' <remarks>'V5.0.0.9⑮</remarks>
    Private Sub SetBarcodeMode()
        Dim ret As Integer
        Dim strMsg As String

        With Me
            Select Case (BarCode_Data.Type)

                '----- V1.23.0.0①↓ -----
                ' 太陽社殿仕様
                Case BarcodeType.Taiyo
                    ' バーコード情報(初期化)を表示
                    Call BC_Info_Disp(0)                            ' バーコード情報の表示初期化
                    .GrpQrCode.Visible = True                       ' バーコード情報(QRｺｰﾄﾞ情報域を使用)表示グループボックス表示
                    .btnRest.Enabled = True                         'V5.0.0.9⑲
                    .btnRest.Visible = True                         'V5.0.0.9⑲

                    ' ポートオープン(バーコードデータ受信用)
                    ret = BC_Rs232c_Open()
                    If (ret <> SerialErrorCode.rRS_OK) Then
                        ' "シリアルポートＯＰＥＮエラー"
                        strMsg = MSG_136 & "(" + gsComPort + ")"
                        Call MsgBox(strMsg, vbOKOnly)
                        'Return (ret)
                    End If
                    '----- V1.23.0.0①↑ -----

                    '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
                Case BarcodeType.Walsin
                    ' バーコード情報(初期化)を表示
                    Call BC_Info_Disp(0)                            ' バーコード情報の表示初期化
                    .GrpQrCode.Visible = True                       ' バーコード情報(QRｺｰﾄﾞ情報域を使用)表示グループボックス表示

                    ' ポートオープン(バーコードデータ受信用)
                    ret = BC_Rs232c_Open()
                    If (ret <> SerialErrorCode.rRS_OK) Then
                        ' "シリアルポートＯＰＥＮエラー"
                        strMsg = MSG_136 & "(" + gsComPort + ")"
                        Call MsgBox(strMsg, vbOKOnly)
                    End If

                    ' SizeCode.iniを構造体を設定する
                    ret = IniSizeCodeData(StSizeCode)
                    '----- V4.11.0.0②↑ -----

                Case BarcodeType.Standard
                    ' バーコード情報(初期化)を表示
                    Call BC_Info_Disp(0)                            ' バーコード情報の表示初期化
                    .GrpQrCode.Visible = True                       ' バーコード情報(QRｺｰﾄﾞ情報域を使用)表示グループボックス表示 'V5.0.0.9⑳
                    .btnRest.Enabled = True                         'V5.0.0.9⑳
                    .btnRest.Visible = True                         'V5.0.0.9⑳

                    ' ポートオープン(バーコードデータ受信用)
                    ret = BC_Rs232c_Open()
                    If (ret <> SerialErrorCode.rRS_OK) Then
                        ' "シリアルポートＯＰＥＮエラー"
                        strMsg = MSG_136 & "(" + gsComPort + ")"
                        Call MsgBox(strMsg, vbOKOnly)
                    End If

                Case Else
                    ' DO NOTHING
            End Select
        End With

    End Sub
#End Region

    '========================================================================================
    '   マウスアップダウン等のイベント処理
    '========================================================================================
#Region "キーダウン時処理"
    '''=========================================================================
    '''<summary>キーダウン時処理</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form1_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Short = eventArgs.KeyCode
        'V6.0.0.0⑩        Dim Shift As Short = eventArgs.KeyData \ &H10000
        Dim pbFuncShift As Boolean = False                  ' SHIFT KEY 押下状態
        Dim pbFuncCtrl As Boolean = False
        Dim pbFuncAlt As Boolean = False

        If (Control.ModifierKeys And Keys.Shift) = Keys.Shift Then
            pbFuncShift = True  ' SHIFT KEY
        End If
        If (Control.ModifierKeys And Keys.Control) = Keys.Control Then
            pbFuncCtrl = True   ' CTRL KEY
        End If
        If (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pbFuncAlt = True    ' ALT KEY
        End If

        If (KeyCode = System.Windows.Forms.Keys.M) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            Me.grpDbg.Visible = True
        End If
        If (KeyCode = System.Windows.Forms.Keys.N) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            Me.grpDbg.Visible = False
        End If
        If (KeyCode = System.Windows.Forms.Keys.S) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            '            Call ZSELXYZSPD(0)          ' XYZ Slow speed ###108
        End If
        If (KeyCode = System.Windows.Forms.Keys.F) And
            pbFuncShift = True And pbFuncCtrl = True And pbFuncAlt = True Then
            '           Call ZSELXYZSPD(1)          ' XYZ Normal speed ###108
        End If

        If (_jogKeyDown IsNot Nothing) Then         'V6.0.0.0⑩
            _jogKeyDown.Invoke(eventArgs)
        End If
    End Sub
#End Region

#Region "キーアップ時処理"
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.0.0⑩</remarks>
    Private Sub Form1_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp
        If (_jogKeyUp IsNot Nothing) Then
            _jogKeyUp.Invoke(e)
        End If
    End Sub
#End Region

#Region "マウス移動時処理(picGraphView)"
    '''=========================================================================
    '''<summary>マウス移動時処理(picGraphView)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub picGraphView_MouseMove(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs)

        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        'Call ResistorGraphMouseMove(picGraphView, Button, Shift, X, y)

    End Sub
#End Region

#Region "マウスアップ時処理(picGraphView)"
    '''=========================================================================
    '''<summary>マウスアップ時処理(picGraphView)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub picGraphView_MouseUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.MouseEventArgs)
        Dim Button As Short = eventArgs.Button \ &H100000
        Dim Shift As Short = System.Windows.Forms.Control.ModifierKeys \ &H10000
        Dim X As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        'Call ResistorGraphClick(picGraphView, Button, Shift, X, y)

    End Sub
#End Region

    '========================================================================================
    '   各コマンド(ユーザコントロールにフォームがあるOCXの場合)がフォーカスを失った場合の処理
    '========================================================================================
#Region "ビデオ画像をクリックしてフォーカスを失った場合の処理"
#If False Then                          'V6.0.0.0⑩
    '''=========================================================================
    ''' <summary>ビデオ画像をクリックしてフォーカスを失った場合の処理 ###051</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>・OcxTeachはDispGazou.EXE実行中のためEnterイベントは入ってこない
    '''          　DispGazou.EXEはOcxTeachで起動する###052
    '''          ・EnterイベントはFormのACTIVEコントロールになった時に発生
    '''            一時停止画面時は何故か一度しか入ってこない</remarks>
    '''=========================================================================
    Private Sub VideoLibrary1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strMSG As String

        Try
            Select Case (giAppMode)
                Case APP_MODE_PROBE
                    ' プローブコマンド実行中 ?
                    Probe1.Focus()                                      ' OcxProbeにフォーカスをセットする 

                Case APP_MODE_TEACH
                    ' ティーチコマンド実行中 ?
                    Teaching1.Focus()                                   ' OcxTeachにフォーカスをセットする 
                    Teaching1.JogSetFocus()                             ' ←Probeと違い何故かこれがないとテンキーが効かない

                    '----- ###122↓ -----
                Case APP_MODE_CUTPOS, APP_MODE_RECOG, APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH, _
                     APP_MODE_CARIB_REC, APP_MODE_CARIB, APP_MODE_CUTREVISE_REC, APP_MODE_CUTREVIDE
                    VideoLibrary1.JogSetFocus()
                    '----- ###122↑ -----

            End Select

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.VideoLibrary1_Enter() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End If
#End Region

#Region "メインFormのコントロールをクリックしてフォーカスを失った場合の処理(一時停止画面)"
#If False Then                          'V6.0.0.0⑩
    '''=========================================================================
    ''' <summary>メインFormのコントロールをクリックしてフォーカスを失った場合の処理(一時停止画面) ###053</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub Form1_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        ' VideoLibrary1_Enterイベントは一時停止画面時は何故か一度しか入ってこないので
        ' Activatedイベントで処理
        Call ADJSetFocu()                               ' 一時停止画面にフォーカスを設定する
    End Sub

    Private Sub Form1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Click
        Call ADJSetFocu()                               ' 一時停止画面にフォーカスを設定する
    End Sub
    ' ログ表示域 
    Private Sub txtLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLog.Click
        Call ADJSetFocu()
    End Sub

    Private Sub lblInterLockMSG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblInterLockMSG.Click
        Call ADJSetFocu()
    End Sub
    Private Sub LblDataFileName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblDataFileName.Click
        Call ADJSetFocu()
    End Sub

    Private Sub lblLoginResult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblLoginResult.Click
        Call ADJSetFocu()
    End Sub

    Private Sub LblCur_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblCur.Click
        Call ADJSetFocu()
    End Sub

    Private Sub LblMes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblMes.Click
        Call ADJSetFocu()
    End Sub

    Private Sub LblRotAtt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblRotAtt.Click

    End Sub

    Private Sub lblLogging_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblLogging.Click
        Call ADJSetFocu()
    End Sub
#End If
#End Region

#Region "一時停止画面にフォーカスを設定する"
#If False Then                          'V6.0.0.0⑩
    '''=========================================================================
    ''' <summary> 一時停止画面にフォーカスを設定する ###053</summary>
    '''=========================================================================
    Private Sub ADJSetFocu()

        Dim strMSG As String

        Try
            ' 一時停止画面表示中の場合
            If (gObjADJ Is Nothing = False) Then
                If (gbTenKeyFlg = True) Then                                '###057
                    gObjADJ.Activate()                                      ' 一時停止画面にフォーカスを設定する
                End If
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.ADJSetFocu() TRAP ERROR = " + ex.Message
            'MsgBox(strMSG)
        End Try
    End Sub
#End If
#End Region

    '========================================================================================
    '   以下デバッグ用他
    '========================================================================================
    Private Sub btnDbgForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDbgForm.Click
        Dim blkNo As Integer
        Dim pltNo As Integer
        Dim pltNoX, pltNoY, blkNoX, blkNoY As Integer                       '#4.12.2.0⑥
        pltNo = 1
        blkNo = 1
        pltNoX = 1 : pltNoY = 1 : blkNoX = 1 : blkNoY = 1                   '#4.12.2.0⑥
        '#4.12.2.0⑥        Call HaltSwOnMove(pltNo, blkNo, False)
        HaltSwOnMove(pltNo, blkNo, pltNoX, pltNoY, blkNoX, blkNoY, False)   '#4.12.2.0⑥
    End Sub

    Private Sub btnGoClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGoClipboard.Click
        Try
            Call Clipboard.Clear()
            Call Clipboard.SetText(txtLog.Text)

        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try


    End Sub

    Public Sub changefrmDistStatus(ByVal DispOnOff As Integer)
        Try

            If (DispOnOff = 1) Then
                '統計表示のON
                gObjFrmDistribute.Show()
                gObjFrmDistribute.RedrawGraph()  '###218 
                'ボタン表示の変更
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    chkDistributeOnOff.Text = "生産グラフ　非表示"
                'Else
                '    chkDistributeOnOff.Text = "Distribute OFF"
                'End If
                chkDistributeOnOff.Text = Form1_019
            Else
                '統計表示のOFF
                gObjFrmDistribute.hide()

                'ボタン表示の変更
                'If (gSysPrm.stTMN.giMsgTyp = 0) Then
                '    chkDistributeOnOff.Text = "生産グラフ　表示"
                'Else
                '    chkDistributeOnOff.Text = "Distribute ON"
                'End If
                chkDistributeOnOff.Text = Form1_020
            End If

            Exit Sub

        Catch ex As Exception
            MsgBox("changefrmDistStatus() Execption error." & vbCrLf & " error msg = " & ex.Message)
        End Try
    End Sub

#Region "グラフ表示/非表示ボタン押下時処理"
    '''=========================================================================
    ''' <summary>グラフ表示/非表示ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub chkDistributeOnOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDistributeOnOff.CheckedChanged
        Try
            If chkDistributeOnOff.Checked = True Then
                ''統計表示のON
                '----- ###218↓ -----
                ' 統計画面ボタンを有効する
                gObjFrmDistribute.cmdGraphSave.Enabled = True
                gObjFrmDistribute.cmdInitial.Enabled = True
                gObjFrmDistribute.cmdFinal.Enabled = True
                '----- ###218↑ -----
                changefrmDistStatus(1)
            Else
                ''統計表示のOFF
                changefrmDistStatus(0)
            End If
        Catch ex As Exception
            MsgBox("chkDistributeOnOff_CheckedChanged() Execption error." & vbCrLf & " error msg = " & ex.Message)

        End Try
    End Sub
#End Region

    Private Sub cmdTestRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTestRun.Click
        Try
            '連続稼動
            m_bStopRunning = False

            '表示状態の変更
            Me.cmdTestRun.BackColor = Color.Yellow
            Me.cmdStop.BackColor = Color.Silver
            Me.txtCount.Text = "0"

            Do
                If (m_bStopRunning = True) Then
                    '表示状態の変更
                    Me.cmdTestRun.BackColor = Color.Silver
                    Me.cmdStop.BackColor = Color.Yellow
                    Exit Do
                Else
                    Me.txtCount.Text = (Long.Parse(Me.txtCount.Text) + 1).ToString
                    Me.btnTrimming.PerformClick()
                End If
            Loop
        Catch ex As Exception
            Dim strMsg As String
            strMsg = "basTrimming.HaltSwOnMove() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub

    Private Sub cmdStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStop.Click
        '連続稼動停止
        m_bStopRunning = True
    End Sub

#Region "統計画面表示状態変更処理"
    Private Sub RedrawDisplayDistribution(ByVal bEntry As Boolean)

        If gKeiTyp = KEY_TYPE_RS Then               'V2.0.0.0⑩ シンプルトリマ
            Return
        End If

        ' TKYCHIPorNETの場合は統計表示処理画面の状態取得
        If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
            If (bEntry = True) Then
                m_bDispDistributeSts = chkDistributeOnOff.Checked
                If (m_bDispDistributeSts = True) Then
                    '統計表示のOFF
                    changefrmDistStatus(0)
                    'frmDistribution.Close()

                    ''ボタン表示の変更
                    'chkDistributeOnOff.Text = "Distribute ON"

                    ' コントロールの状態変更
                    ''###112                    Me.chkDistributeOnOff.Checked = False
                End If
            Else
                If (m_bDispDistributeSts = True) Then
                    '統計表示のON
                    changefrmDistStatus(1)
                    'frmDistribution.Show()

                    ''ボタン表示の変更
                    'chkDistributeOnOff.Text = "Distribute OFF"

                    ' コントロールの状態変更
                    Me.chkDistributeOnOff.Checked = True
                End If
            End If
        End If

    End Sub
#End Region

    Private Sub btnEndTrace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'CmdEnd.PerformClick()
    End Sub

    '###182
    Private Sub Mg_Up_MouseDownButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseDown
        Dim CmpData As Integer

        CmpData = 0

        If (CheckBox1.Checked = True) Then
            CmpData = CmpData Or &H1
        End If

        If (CheckBox2.Checked = True) Then
            CmpData = CmpData Or &H2
        End If

        If (CheckBox3.Checked = True) Then
            CmpData = CmpData Or &H4
        End If

        If (CheckBox4.Checked = True) Then
            CmpData = CmpData Or &H8
        End If

        Call MGMoveJog(CmpData, MG_UP)

    End Sub
    '###182
    Private Sub Mg_Up_MouseUpButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseUp
        Call MGStopJog()
    End Sub


    '###182
    Private Sub Mg_Down_MouseUpButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button2.MouseUp
        Call MGStopJog()
    End Sub
    '###182
    Private Sub Mg_Down_MouseDownButton(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button2.MouseDown
        Dim CmpData As Integer

        CmpData = 0

        If (CheckBox1.Checked = True) Then
            CmpData = CmpData Or &H1
        End If

        If (CheckBox2.Checked = True) Then
            CmpData = CmpData Or &H2
        End If

        If (CheckBox3.Checked = True) Then
            CmpData = CmpData Or &H4
        End If

        If (CheckBox4.Checked = True) Then
            CmpData = CmpData Or &H8
        End If

        Call MGMoveJog(CmpData, MG_DOWN)

    End Sub

#Region "フォームロード時処理"
    '''=========================================================================
    '''<summary>フォームロード時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strMSG As String                                            ' ﾒｯｾｰｼﾞ編集域
        Dim dispSize As System.Drawing.Size
        Dim dispPos As System.Drawing.Point
        Dim r As Integer
        Dim lSts As Long = 0                                            ' ###207

        Try
            '---------------------------------------------------------------------------
            '   装置初期化処理
            '---------------------------------------------------------------------------
            r = Loader_Init()                                           ' オートローダ初期処理(SL436用)
            If (r < cFRS_NORMAL) Then
                ' 強制終了
                Call AppEndDataSave()
                Call AplicationForcedEnding()
                Exit Sub
            End If

            Call Me.Initialize_VideoLib()
            'V6.0.0.0-28            Call Me.VideoLibrary1.VideoStop()                           ' 原点復帰処理で表示がフリーズ可能性があるため一旦停止
            Call Me.Initialize_TrimMachine()

            ' '###241 
            'V6.0.0.0⑩            AddHandler VideoLibrary1.Enter, AddressOf VideoLibrary1_Enter

            ' 画面表示位置の設定
            dispPos.X = 0
            dispPos.Y = 0
            Me.Location = dispPos

            ' 画面サイズの設定
            dispSize.Height = 1024
            dispSize.Width = 1280
            Me.Size = dispSize

            Call SetButtonImage()                                       ' フォームのボタン名の設定(日本語/英語)

            ' Ocx(VB6)ﾃﾞﾊﾞｯｸﾞﾓｰﾄﾞ設定
#If cOFFLINEcDEBUG Then
            VideoLibrary1.cOFFLINEcDEBUG = &H3141S
            Teaching1.cOFFLINEcDEBUG = &H3141S
            Probe1.cOFFLINEcDEBUG = &H3141S
            Ctl_LaserTeach2.cOFFLINEcDEBUG = &H3141S
#End If
            ' Video.ocxのDbgOn/Offﾎﾞﾀﾝの有効/無効指定(デバッグ用)
            '　デバッグ時変数内容を表示させるため
#If cDBGRdraw Then                                                      ' Video.ocxのDbgOn/Offﾎﾞﾀﾝ有効とする ?
            VideoLibrary1.cDBGRdraw = &H3142
#End If
            ' コントロールを非表示にする
            Probe1.Visible = False
            Teaching1.Visible = False
            HelpVersion1.Visible = False

            ' V1.14.0.0①
            'ユーザカスタマイズ画面表示
            r = Set_UserSpecialCtrl(gSysPrm.stCTM.giSPECIAL)

            ' バーコード関連コントロール表示・接続をおこなう
            SetBarcodeMode()            'V5.0.0.9⑮

            ' プローブ位置合わせのコントロールの表示位置を指定する
            Probe1.Left = Text4.Location.X
            Probe1.Top = Text4.Location.Y

            ' ティーチングのコントロールの表示位置を指定する
            Teaching1.Left = Text4.Location.X
            Teaching1.Top = Text4.Location.Y

            ' TKYCHIPorNETの場合、統計表示オブジェクトを生成する
            If (gTkyKnd = KND_CHIP Or gTkyKnd = KND_NET) Then
                gObjFrmDistribute = New frmDistribution
            End If

            ' ローダへトリマ停止中信号を送信する
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R系 ? ###035
                Call SetLoaderIO(LOUT_STOP, &H0)                        ' ローダ出力(ON=トリマ停止中, OFF=なし)
            Else
                Call SetLoaderIO(0, COM_STS_TRM_STATE)                  ' ローダ出力(ON=なし,OFF=トリマ動作中)
            End If

            ' マガジンの上下動作
            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL436) Then       ' SL436R系 ? ###035
                GroupBox1.Visible = True
                '----- ###207↓ -----
                r = H_Read(LOFS_H00, lSts)                              ' 初期設定状態取得(H0.00-H0.15)
                If (lSts And LHST_MAGAZINE) Then                        ' 4マガジン ?
                    CheckBox3.Visible = True                            ' MG3表示 
                    CheckBox4.Visible = True                            ' MG4表示 
                Else
                    CheckBox3.Visible = False                           ' MG3非表示 
                    CheckBox4.Visible = False                           ' MG4非表示 
                End If
                '----- ###207↑ -----
            Else
                GroupBox1.Visible = False
            End If

            ' ﾄﾘﾐﾝｸﾞ結果印刷項目のﾃﾞｰﾀをｸﾘｱする(ローム殿特注) 'V1.18.0.0③
            Call ClrTrimPrnData()
#If False Then                          'V6.0.0.0⑤
            ' 画像表示プログラムを起動する
            If gKeiTyp = KEY_TYPE_RS Then
                Execute_GazouProc(ObjGazou, DISPGAZOU_SMALL_PATH, DISPGAZOU_WRK, 0)    'V3.0.0.0⑤
            Else
                Execute_GazouProc(ObjGazou, DISPGAZOU_PATH, DISPGAZOU_WRK, 0)    'V3.0.0.0⑤
            End If
#End If
            ' Initialize_VideoLib()内のVideoLibrary1.Init_Library()で実行済み  'V6.0.0.0-28
            'V6.0.0.0-28            ' 内部カメラに切り替える 
            'V6.0.0.0-28            Call Me.VideoLibrary1.ChangeCamera(INTERNAL_CAMERA)
            'V6.0.0.0-28            Call Me.VideoLibrary1.VideoStart()

            '-------------------------------------------------------------------
            '   シンプルトリマ画面初期化処理(SL436S時)
            '-------------------------------------------------------------------
            Call SimpleTrimmer.SimpleTrimmerInit()              'V2.0.0.0⑩ ADD

            '----- V4.0.0.0-87↓ -----
            If (giMachineKd = MACHINE_KD_RS) Then
                SimpleTrimmer.DspLaserPower()
            End If
            '----- V4.0.0.0-87↑ -----

            'V4.8.0.1①↓
            If giRateDisp = 1 Then
                CheckNGRate.Checked = True
            End If
            'V4.8.0.1①↑
            'V4.9.0.0①
            If giNgStop = 1 Then
                btnJudge.Visible = True
            Else
                btnJudge.Visible = False
            End If
            'V4.9.0.0①
            'V4.11.0.0⑨
            btnNEXT.Enabled = False
            btnPREV.Enabled = False
            'V4.11.0.0⑨

            'V5.0.0.9⑭ V6.0.3.0⑧
            Call Me.System1.SetSignalTowerCtrl(Me.System1.SIGNAL_IDLE)
            'V5.0.0.9⑭ V6.0.3.0⑧

            ' 表示を更新
            Me.Refresh()

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "i-TKY.Form1_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try


    End Sub

#End Region

    '''' <summary>
    '''' シンプルトリマ用のVideoサイズに変更する 
    '''' </summary>
    '''' <remarks></remarks>
    'Public Sub SetSimpleVideoSize()
    '    If gKeiTyp <> KEY_TYPE_RS Then
    '        Return
    '    End If
    '    VideoLibrary1.SetVideoSizeAndCross(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)

    'End Sub

    '''' <summary>
    '''' ティーチング用に従来のVideoサイズに変更する 
    '''' </summary>
    '''' <remarks></remarks>
    'Public Sub SetTeachVideoSize()
    '    If gKeiTyp <> KEY_TYPE_RS Then
    '        Return
    '    End If

    '    VideoLibrary1.SetVideoSizeAndCross(NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)

    'End Sub

    ''' <summary>
    ''' 画像サイズを変えるモードか判定する
    ''' </summary>
    ''' <param name="iAppMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChkVideoChangeMode(ByVal iAppMode As Integer) As Integer
        ChkVideoChangeMode = 0
        Select Case (iAppMode)
            Case APP_MODE_LASER, APP_MODE_PROBE, APP_MODE_TEACH, APP_MODE_RECOG, APP_MODE_CUTPOS, APP_MODE_TTHETA, APP_MODE_TX, APP_MODE_TY,
                APP_MODE_EXCAM_R1TEACH, APP_MODE_EXCAM_TEACH, APP_MODE_CARIB_REC, APP_MODE_CARIB, APP_MODE_CUTREVISE_REC, APP_MODE_CUTREVIDE,
                APP_MODE_CIRCUIT, APP_MODE_APROBEREC, APP_MODE_APROBEEXE, APP_MODE_IDTEACH, APP_MODE_PROBE_CLEANING, APP_MODE_INTEGRATED 'V4.10.0.0⑫
                ChkVideoChangeMode = 1
            Case Else
                ChkVideoChangeMode = 0
        End Select

    End Function

    ''' <summary>
    ''' 一時停止中の監視用タイマー
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TimerAdjust_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerAdjust.Tick

        Dim Ret As Integer
        Dim sw As Long
        Dim swStatus As Integer
        Dim r As Integer 'V4.0.0.0-86

        mExit_flg = cFRS_NORMAL
        TimerAdjust.Enabled = False

        ' 'V4.0.0.0-43
        If gKeiTyp = KEY_TYPE_RS Then               'V2.0.0.0⑩ シンプルトリマ
            SimpleTrimmer.ElapsedTimeUpdate()       'V2.0.0.0⑩ 経過時間の更新(一時停止中)
        Else
            Return
        End If                                      'V2.0.0.0⑩
        ' 'V4.0.0.0-43

        ' START/RESETキー入力チェック
        Ret = STARTRESET_SWCHECK(False, sw)                           ' START/RESETキー押下チェック(監視なしモード)
        If (sw = cFRS_ERR_START) Then
            ''V4.0.0.0-86
            r = GetLaserOffIO(True) 'V5.0.0.1⑫
            If r = 1 Then
                TimerAdjust.Enabled = True      'V4.11.0.0⑰
                Call ZCONRST()     'V4.11.0.0⑰
                Exit Sub
            End If
            ''V4.0.0.0-86
            gbExitFlg = True
            TimerAdjust.Enabled = False
            Call START_SWCHECK(1, swStatus)
            Return
        ElseIf (sw = cFRS_ERR_RST) Then
            mExit_flg = cFRS_ERR_RST                                        ' Return値 = Cancel(RESETｷｰ)  
            gbExitFlg = True                                                ' 終了フラグON
            TimerAdjust.Enabled = False
            Call STARTRESET_SWCHECK(1, swStatus)
            Return
        End If


        TimerAdjust.Enabled = True

    End Sub

    ''' <summary>
    ''' ボタンを押した結果を格納する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetResultAdjust() As Integer

        GetResultAdjust = mExit_flg

    End Function

    ' >>> V3.1.0.0① 2014/11/28
#Region "パネル表示ON"
    '''=========================================================================
    ''' <summary>
    ''' パネル表示ON
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetDataDisplayOn()
        If (giMachineKd <> MACHINE_KD_RS) Then Return ' SL436SでなければNOP V4.0.0.0-36
        Me.pnlDataDisplay.Visible = True
    End Sub
#End Region

#Region "パネル表示OFF"
    '''=========================================================================
    ''' <summary>
    ''' パネル表示OFF
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetDataDisplayOff()
        Me.pnlDataDisplay.Visible = False
    End Sub
#End Region

#Region "データ表示を消すか判断する"
    '''=========================================================================
    ''' <summary>
    ''' データ表示を消すか判断する
    ''' </summary>
    ''' <param name="iAppMode">アプリケーションモード</param>
    ''' <returns>True:該当、False:非該当</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Function IsDataDisplayCheck(ByVal iAppMode As Integer) As Boolean
        ' プローブ、ティーチング、画像登録、BP位置調整、ステージ位置調整
        'Dim arMode As Integer() = New Integer() {APP_MODE_PROBE, APP_MODE_TEACH, APP_MODE_RECOG, APP_MODE_TX, APP_MODE_TY, APP_MODE_LASER, APP_MODE_PROBE_CLEANING}
        Dim arMode As Integer() = New Integer() {APP_MODE_PROBE, APP_MODE_TEACH, APP_MODE_RECOG, APP_MODE_TX,
                                                 APP_MODE_TY, APP_MODE_LASER, APP_MODE_PROBE_CLEANING, APP_MODE_INTEGRATED} 'V4.10.0.0⑫
        Dim i As Integer

        IsDataDisplayCheck = False

        For i = 0 To UBound(arMode) Step 1
            If arMode(i) = iAppMode Then
                IsDataDisplayCheck = True
                Exit For
            End If
        Next i

    End Function
#End Region
    ' <<< V3.1.0.0① 2014/11/28

    Private Sub cmdGraphSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGraphSave.Click

        SavePanelImage()

    End Sub
    'V4.0.0.0⑫
    '''=========================================================================
    ''' <summary>
    ''' イニシャル表示を押した処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdInitial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInitial.Click
        gObjFrmDistribute.SetDispGrp(True)
        Call gObjFrmDistribute.RedrawGraph()

    End Sub
    'V4.0.0.0⑫
    '''=========================================================================
    ''' <summary>
    ''' ファイナル表示を押した処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub cmdFinal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFinal.Click
        gObjFrmDistribute.SetDispGrp(False)
        Call gObjFrmDistribute.RedrawGraph()                                              ' 分布図表示処理

    End Sub

#Region "統計表示パネルのON/OFF"
    ' V4.0.0.0⑫
    '''=========================================================================
    ''' <summary>
    ''' 統計表示パネルのON/OFF
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub PanelGraphOnOff(ByVal flg As Boolean)
        PanelGraph.Visible = flg
    End Sub

#End Region
    'V4.0.0.0⑫
#Region "メイン画面上の分布図保存処理"
    '''=========================================================================
    '''<summary>分布図保存ボタン押下時処理</summary>
    '''<remarks>メイン画面上の分布図をBmpファイルに保存する</remarks>
    '''=========================================================================
    Private Sub SavePanelImage()

        Dim fileName As String
        Dim bFileSave As Boolean
        Dim bitMap As New Bitmap(PanelGraph.Width, PanelGraph.Height)

        Try

            bFileSave = False
            fileName = ""

            'ファイル名称の作成
            If gObjFrmDistribute.GetDispGrp() = True Then
                fileName = gSysPrm.stLOG.gsLoggingDir & "IT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            Else
                fileName = gSysPrm.stLOG.gsLoggingDir & "FT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            End If

            ' PanelGraphの内容を保存
            PanelGraph.DrawToBitmap(bitMap, New Rectangle(0, 0, PanelGraph.Width, PanelGraph.Height))
            bitMap.Save(fileName)

            '結果の表示
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    MsgBox("保存完了！" & vbCrLf & " (" & fileName & ")")
            'Else
            '    MsgBox("Save completion." & vbCrLf & " (" & fileName & ")")
            'End If
            MsgBox(Form1_021 & vbCrLf & " (" & fileName & ")")
            bitMap.Dispose()

            Exit Sub

        Catch ex As Exception

            bitMap.Dispose()
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    MsgBox("保存できませんでした。")
            'Else
            '    MsgBox("I was not able to save it.")
            'End If
            MsgBox(Form1_022)

        End Try

    End Sub
#End Region

    ''' <summary> 'V4.1.0.0⑤
    ''' ボタンの表示非表示を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChkButtonDisp()

        If stFNC(F_PROBE_CLEANING).iDEF >= 1 Then
            ProbeBtnVisibleSet(True)
        Else
            ProbeBtnVisibleSet(False)
        End If
        If stFNC(F_MAINTENANCE).iDEF >= 1 Then
            MaintBtnVisibleSet(True)
        Else
            MaintBtnVisibleSet(False)
        End If

    End Sub
    '----- V4.11.0.0⑤↓ (WALSIN殿SL436S対応) -----
    '========================================================================================
    '   トリミング開始ブロック番号コンボボックス処理
    '========================================================================================
#Region "トリミング開始ブロック番号コンボボックスを初期化する"
    '''=========================================================================
    '''<summary>トリミング開始ブロック番号コンボボックスを初期化する</summary>
    '''=========================================================================
    Public Sub Init_StartBlkComb()

        Dim strMsg As String

        Try
            ' トリミング開始ブロック番号指定が無効ならNOP
            If (giStartBlkAss = 0) Then Return

            ' DropDownStyleプロパティをDropDownにする(テキスト部分を直接編集できる)
            '                          DropDownListにするとテキスト部分を直接編集できない
            Me.CbStartBlkX.DropDownStyle = ComboBoxStyle.DropDownList
            Me.CbStartBlkY.DropDownStyle = ComboBoxStyle.DropDownList

            ' トリミング開始ブロック番号コンボボックスを初期化する
            Me.CbStartBlkX.Items.Clear()
            Me.CbStartBlkY.Items.Clear()
            Me.CbStartBlkX.Items.Add("  1")
            Me.CbStartBlkY.Items.Add("  1")
            Me.CbStartBlkX.SelectedIndex = 0
            Me.CbStartBlkY.SelectedIndex = 0

            SetGrpStartBlkText(1)       'V5.0.0.9⑯

        Catch ex As Exception
            strMsg = "iTKY.Init_StartBlkComb() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "トリミング開始ブロック番号XYを1,1とする"
    '''=========================================================================
    '''<summary>トリミング開始ブロック番号XYを1,1とする</summary>
    '''=========================================================================
    Public Sub Set_StartBlkComb1St()

        Dim strMsg As String

        Try
            ' トリミング開始ブロック番号指定が無効ならNOP
            If (giStartBlkAss = 0) Then Return

            ' トリミング開始ブロック番号XYを1,1とする
            Me.CbStartBlkX.SelectedIndex = 0
            Me.CbStartBlkY.SelectedIndex = 0

            SetGrpStartBlkText(1)       'V5.0.0.9⑯

        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkComb1St() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "トリミング開始ブロック番号コンボボックスの設定"
    '''=========================================================================
    '''<summary>トリミング開始ブロック番号コンボボックスの設定</summary>
    '''=========================================================================
    Public Sub Set_StartBlkComb()

        Dim Idx As Integer
        Dim strMsg As String

        Try
            ' トリミング開始ブロック番号指定が無効ならNOP
            If (giStartBlkAss = 0) Then Return

            ' トリミング開始ブロック番号Xコンボボックスの設定
            Me.CbStartBlkX.Items.Clear()
            For Idx = 1 To typPlateInfo.intBlockCntXDir
                Me.CbStartBlkX.Items.Add(Idx.ToString(0).PadLeft(3))
            Next Idx
            Me.CbStartBlkX.SelectedIndex = 0

            ' トリミング開始ブロック番号Yコンボボックスの設定
            Me.CbStartBlkY.Items.Clear()
            For Idx = 1 To typPlateInfo.intBlockCntYDir
                Me.CbStartBlkY.Items.Add(Idx.ToString(0).PadLeft(3))
            Next Idx
            Me.CbStartBlkY.SelectedIndex = 0

            ' 開始ブロック番号活性化/非活性化
            If (bFgAutoMode = False) Then                           ' 手動運転 ?
                GrpStartBlk.Enabled = True                          ' 活性化 
            Else
                GrpStartBlk.Enabled = False                         ' 活性化 
            End If

            SetGrpStartBlkText(1)       'V5.0.0.9⑯

        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkComb() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "トリミング開始ブロック番号XYを取得する"
    '''=========================================================================
    '''<summary>トリミング開始ブロック番号XYを取得する</summary>
    ''' <param name="BlkX">(OUT)ブロック番号X</param>
    ''' <param name="BlkY">(OUT)ブロック番号Y</param>
    '''=========================================================================
    Public Sub Get_StartBlkNum(ByRef BlkX As Integer, ByRef BlkY As Integer)

        Dim strMsg As String

        Try
            ' トリミング開始ブロック番号指定が無効ならNOP
            If (giStartBlkAss = 0) Then Return

            ' トリミング開始ブロック番号XYを取得する
            BlkX = Me.CbStartBlkX.SelectedIndex + 1
            BlkY = Me.CbStartBlkY.SelectedIndex + 1

        Catch ex As Exception
            strMsg = "iTKY.Get_StartBlkNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "トリミング開始ブロック番号XYを設定する"
    '''=========================================================================
    '''<summary>トリミング開始ブロック番号XYを設定する</summary>
    ''' <param name="BlkX">(INP)ブロック番号X</param>
    ''' <param name="BlkY">(INP)ブロック番号Y</param>
    '''=========================================================================
    Public Sub Set_StartBlkNum(ByVal BlkX As Integer, ByVal BlkY As Integer)

        Dim strMsg As String

        Try
            ' トリミング開始ブロック番号指定が無効ならNOP
            If (giStartBlkAss = 0) Then Return

            If Me.CbStartBlkX.Items.Count >= BlkX Then
                ' トリミング開始ブロック番号Xを設定する
                Me.CbStartBlkX.SelectedIndex = BlkX - 1
            Else
                Me.CbStartBlkX.SelectedIndex = 0
            End If
            If Me.CbStartBlkY.Items.Count >= BlkY Then
                ' トリミング開始ブロック番号Yを設定する
                Me.CbStartBlkY.SelectedIndex = BlkY - 1
            Else
                Me.CbStartBlkY.SelectedIndex = 0
            End If

            'V5.0.0.9⑯                  ↓
            If (2 = giStartBlkAss) Then
                Dim blockNo As Integer = basTrimming.GetProcessingOrder(BlkX, BlkY)
                SetGrpStartBlkText(blockNo)
            End If
            'V5.0.0.9⑯                  ↑
        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "トリミング開始ブロック番号コンボボックスを活性化/非活性化する"
    '''=========================================================================
    ''' <summary>トリミング開始ブロック番号コンボボックスを活性化/非活性化する</summary>
    ''' <param name="bFlg">(INP)TRUE=活性化, FALSE=非活性化</param>
    '''=========================================================================
    Public Sub Set_StartBlkNum_Enabled(ByVal bFlg As Boolean)

        Dim strMsg As String

        Try
            ' トリミング開始ブロック番号指定が無効ならNOP
            If (giStartBlkAss = 0) Then Return

            ' トリミング開始ブロック番号コンボボックスを活性化/非活性化する
            Me.GrpStartBlk.Enabled = bFlg
            Me.CbStartBlkX.Enabled = bFlg
            Me.CbStartBlkY.Enabled = bFlg

        Catch ex As Exception
            strMsg = "iTKY.Set_StartBlkNum_Enabled() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V4.11.0.0⑤↑ -----

    Private Sub btnJudge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJudge.Click
        Dim objform As New frmStopCond

        objform.Owner = Me
        objform.ShowDialog()

    End Sub

    ''' <summary>
    ''' 画面上のNEXTボタンを押した処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNEXT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNEXT.Click
        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer
        Dim plateCnt As Integer
        Dim blockCnt As Integer

        Try
            plateCnt = GetPlateCnt()
            blockCnt = GetBlockCnt()

            If (gCurPlateNo >= plateCnt) And (gCurBlockNo >= blockCnt) Then
                ' 最終プレート、最終ブロックであれば移動しない
                GoTo STP_END
            ElseIf (gCurBlockNo >= blockCnt) Then
                ' 最終ブロックであれば、次のプレートの先頭へ
                workPlateNo = gCurPlateNo + 1
                workBlockNo = 1
            Else
                ' 次のブロックへ移動する
                workPlateNo = gCurPlateNo
                workBlockNo = gCurBlockNo + 1
            End If

            ' ステージ移動
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = frmFineAdjust.MOVE_NEXT) Then
                ' 移動後のプレート番号、ブロック番号を保存する
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                '                lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0⑤
                'V5.0.0.9⑯                Call Set_StartBlkNum(workBlockNo, 1)  ↓へ

                'V4.0.0.0⑬↓
                If gKeiTyp = KEY_TYPE_RS Then
                    'V5.0.0.9⑯                  ↓
                    Select Case (giStartBlkAss)
                        Case 1          ' 既存
                            Set_StartBlkNum(workBlockNo, 1)
                            SetBlockDisplayNumber(gCurBlockNo)

                        Case 2          'V5.0.0.9⑯
                            SetBlockDisplayNumber(workBlockNo, True)
                    End Select
                    'V5.0.0.9⑯                  ↑
                    SetNowBlockDspNum(gCurBlockNo)                      'V4.1.0.0⑱
                End If
                'V4.0.0.0⑬↑
            End If

STP_END:

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "Form1.btnNEXT_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub

    ''' <summary>
    ''' 画面上のPREVボタンを押した処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPREV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPREV.Click

        Dim intRet As Integer
        Dim workBlockNo As Integer
        Dim workPlateNo As Integer

        Try
            ' 先頭プレート、先頭ブロックであれば移動しない
            If (gCurPlateNo <= 1) And (gCurBlockNo <= 1) Then
                GoTo STP_END
            ElseIf (gCurBlockNo <= 1) Then
                ' 先頭ブロックであれば一つ前のプレートの最終ブロックへ
                workPlateNo = gCurPlateNo - 1
                workBlockNo = GetBlockCnt()
            Else
                ' ブロックを1つ前に移動する
                workBlockNo = gCurBlockNo - 1
                workPlateNo = gCurPlateNo
            End If

            ' ステージ移動
            intRet = MoveTargetStagePos(workPlateNo, workBlockNo)
            If (intRet = frmFineAdjust.MOVE_NEXT) Then
                ' 移動後のプレート番号、ブロック番号を保存する
                gCurBlockNo = workBlockNo
                gCurPlateNo = workPlateNo
                ' lblBlockNo.Text = "BlockNo=" + gCurBlockNo.ToString("0")    'V4.9.0.0⑤
                'V4.1.0.0⑱
                If gKeiTyp = KEY_TYPE_RS Then
                    'V5.0.0.9⑯                  ↓
                    Select Case (giStartBlkAss)
                        Case 1          ' 既存
                            Set_StartBlkNum(workBlockNo, 1)

                        Case 2          'V5.0.0.9⑯
                            SetBlockDisplayNumber(workBlockNo, True)
                    End Select
                    'V5.0.0.9⑯                  ↑
                    SetNowBlockDspNum(gCurBlockNo)
                End If
                'V4.1.0.0⑱

            End If
STP_END:

        Catch ex As Exception
            Dim strMsg As String
            strMsg = "Form1.btnPREV_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try

    End Sub

#Region "ステージを指定ブロック位置に移動する"
    ''' <summary>ステージを指定ブロック位置に移動する</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V5.0.0.9⑯</remarks>
    Private Sub CbStartBlkXY_SelectionChangeCommitted(ByVal sender As Object, ByVal e As EventArgs) Handles CbStartBlkY.SelectionChangeCommitted, CbStartBlkX.SelectionChangeCommitted
        If (1 < giStartBlkAss) Then
            Try
                Dim x, y As Integer : Get_StartBlkNum(x, y)
                Dim workBlockNo As Integer = basTrimming.GetProcessingOrder(x, y)
                SetGrpStartBlkText(workBlockNo)

                If (Globals_Renamed.gKeiTyp = Globals_Renamed.KEY_TYPE_RS) Then
                    SimpleTrimmer.SetBlockDisplayNumber(workBlockNo, True)
                End If

                If (btnPREV.Enabled OrElse btnNEXT.Enabled) Then
                    ' ステージ移動
                    Dim intRet As Integer = basTrimming.MoveTargetStagePos(Globals_Renamed.gCurPlateNo, workBlockNo)
                    If (intRet = frmFineAdjust.MOVE_NEXT) Then
                        ' 移動後のブロック番号を保存する
                        Globals_Renamed.gCurBlockNo = workBlockNo
                    End If
                End If

            Catch ex As Exception
                Dim strMsg As String
                strMsg = "Form1.CbStartBlkXY_SelectionChangeCommitted() TRAP ERROR = " & ex.Message
                MessageBox.Show(Me, strMsg)
            End Try
        End If

    End Sub
#End Region

#Region "ブロック位置指定コンボボックスＸ・Ｙに該当するブロック番号を表示する"
    ''' <summary>ブロック位置コンボボックスＸ・Ｙに該当するブロック番号を表示する</summary>
    ''' <param name="blockNo">加工対象ブロック番号</param>
    ''' <remarks>'V5.0.0.9⑯</remarks>
    Private Sub SetGrpStartBlkText(ByVal blockNo As Integer)
        Static text As String = Me.GrpStartBlk.Text & " "
        If (2 = giStartBlkAss) Then
            Me.GrpStartBlk.Text = text & blockNo
        End If
    End Sub
#End Region

#Region "継続チェックボックスの描画をおこなう"
    ''' <summary>継続チェックボックスの描画をおこなう</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V5.0.0.9⑯</remarks>
    Private Sub chkContinue_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles chkContinue.Paint
        Dim chk As CheckBox = DirectCast(sender, CheckBox)
        Dim rect As Rectangle = chk.DisplayRectangle
        If (rect.IsEmpty) Then Return

        Dim state As ButtonState
        Dim bgColor As Color
        If (chk.Checked) Then
            state = ButtonState.Checked
            bgColor = Color.Yellow
        Else
            state = ButtonState.Normal
            bgColor = SystemColors.Control
        End If
        If (False = chk.Enabled) Then
            state = ButtonState.Flat
        End If
        ' ボタンとして描画
        ControlPaint.DrawButton(e.Graphics, rect, state)

        rect.Inflate(-2, -2)    ' 描画領域を縮小
        Using b As Brush = New SolidBrush(bgColor)
            ' 背景色としてボタン領域内を塗りつぶし
            e.Graphics.FillRectangle(b, rect)
        End Using

        ' 文字描画
        TextRenderer.DrawText(
            e.Graphics,
            chk.Text,
            chk.Font,
            rect,
            If(chk.Enabled, chk.ForeColor, SystemColors.GrayText),
            TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)

    End Sub
#End Region
    'V4.12.2.0①                         ↓
#Region "マップをプレートデータのブロック数X・Yで初期化する"
    ''' <summary>マップをプレートデータのブロック数X・Yで初期化する</summary>
    ''' <param name="doEnabled">マップ有効:true, 無効:false (データファイル読み込み結果)</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Public Shared Sub MapInitialize(ByVal doEnabled As Boolean)
        With Form1.Instance
            If (._mapDisp) Then         'tky.ini[DEVICE_CONST]MAP_DISP
                If (doEnabled) Then
                    Dim cntX As Integer = typPlateInfo.intBlockCntXDir
                    Dim cntY As Integer = typPlateInfo.intBlockCntYDir
#If False Then  'V6.0.1.3①
                    .TrimMap1.TrimMapInitialize(.VideoLibrary1.Top, .VideoLibrary1.Left,
                                                .VideoLibrary1.Width, .VideoLibrary1.Height,
                                                cntX, cntY, gSysPrm.stDEV.giBpDirXy,
                                                True, True, (cntX <= 45), (cntY <= 50),
                                                False, True)

                    Globals_Renamed.ProcBlock = New Integer(cntX, cntY) {}      'V4.12.2.0②
#Else
                    ' プレート対応    'V6.0.1.3①
                    Dim area As Size = .VideoLibrary1.CameraArea
                    Dim Width As Integer = area.Width
                    Dim Height As Integer = area.Height

                    Dim pcx As Integer = typPlateInfo.intPlateCntXDir
                    Dim pcy As Integer = typPlateInfo.intPlateCntYDir

                    Dim col, row As Integer
                    If (1 < pcx) AndAlso (1 < pcy) Then
                        col = pcx
                        row = pcy
                    Else
                        col = cntX
                        row = cntY
                    End If

                    Dim args As New TrimMapPlate.InitializeArgs()
                    args.Top = .VideoLibrary1.Top
                    args.Left = .VideoLibrary1.Left
                    args.Width = Width
                    args.Height = Height
                    args.PlateCountX = pcx
                    args.PlateCountY = pcy
                    args.BlockCountX = cntX
                    args.BlockCountY = cntY
                    args.DirStepRepeat = typPlateInfo.intDirStepRepeat
                    args.ColumnHeaderVisible = True
                    args.RowHeaderVisible = True
                    args.DrawColumnNumber = (col <= 45)
                    args.DrawRowNumber = (row <= 50)
                    args.DrawBlockNumber = False
                    args.DrawBlockNumberStyle = DISPCELL_TYPE.CELL_XY   ' TODO: tky.ini[MAP]CELLNO_TYPE を使用するか
                    args.ToolTipVisible = True
                    args.KeepBlockSelect = False
                    args.CanBlockSelect = False
                    args.DisableFourCorners = False

                    .TrimMap1.Initialize(args)
#End If
                Else
                    .TrimMap1.Visible = doEnabled       ' マップ表示
                End If
                .CmdMapOnOff.Enabled = doEnabled        ' マップオンオフボタン
            End If
        End With
    End Sub
#End Region

#Region "選択済み加工対象ブロックを変更不可で表示する"
    ''' <summary>選択済み加工対象ブロックを変更不可で表示する</summary>
    ''' <param name="onCameraArea">True:カメラ画像に重ねる,False:カメラ画像の下</param>
    ''' <remarks>'V4.12.2.0②</remarks>
    Public Shared Sub ShowSelectedMap(ByVal onCameraArea As Boolean)    'V6.0.1.0⑪
        'V6.0.1.0⑪    Public Shared Sub ShowSelectedMap()
        With Form1.Instance
            If (._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
                Dim plateX As Integer = typPlateInfo.intPlateCntXDir    'V6.0.1.3①
                Dim plateY As Integer = typPlateInfo.intPlateCntYDir    'V6.0.1.3①
                Dim blockX As Integer = typPlateInfo.intBlockCntXDir    'V6.0.1.3① cntX -> blockX
                Dim blockY As Integer = typPlateInfo.intBlockCntYDir    'V6.0.1.3① cntY -> blockY

                Dim top, left, width, height As Integer
                Dim area As Size = .VideoLibrary1.CameraArea            'V6.0.1.0⑨
                width = area.Width
                height = area.Height

                If (onCameraArea) Then
                    top = .VideoLibrary1.Top
                    left = .VideoLibrary1.Left
                Else
                    'V6.0.1.0⑪          ↓
                    top = .VideoLibrary1.Top + height + 1
                    left = .VideoLibrary1.Left
                    'V6.0.1.0⑪          ↑
                End If
#If False Then  'V6.0.1.3①
                .TrimMap1.InitializeNotSelectable(top, left, width, height,
                                                  cntX, cntY, gSysPrm.stDEV.giBpDirXy,
                                                  True, True, (cntX <= 45), (cntY <= 50),
                                                  False, True, Globals_Renamed.ProcBlock, 1)
#Else
                ' プレート対応    'V6.0.1.3①
                Dim col, row As Integer
                If (1 = plateX) AndAlso (1 = plateY) Then
                    col = blockX
                    row = blockY
                Else
                    col = plateX
                    row = plateY
                End If

                Dim args As New TrimMapPlate.InitializeArgs()
                args.Top = top
                args.Left = left
                args.Width = width
                args.Height = height
                args.PlateCountX = plateX
                args.PlateCountY = plateY
                args.BlockCountX = blockX
                args.BlockCountY = blockY
                args.DirStepRepeat = typPlateInfo.intDirStepRepeat
                args.ColumnHeaderVisible = True
                args.RowHeaderVisible = True
                args.DrawColumnNumber = (col <= 45)
                args.DrawRowNumber = (row <= 50)
                args.DrawBlockNumber = False
                args.DrawBlockNumberStyle = DISPCELL_TYPE.CELL_XY       ' TODO: tky.ini[MAP]CELLNO_TYPE を使用するか
                args.ToolTipVisible = True
                args.KeepBlockSelect = False
                args.CanBlockSelect = False
                args.DisableFourCorners = False

                .TrimMap1.Initialize(args)
#End If
                .TrimMap1.BringToFront()        'V6.0.1.0⑪
            End If
        End With

        ClearTrimmingResult()           'V6.0.1.0⑫
    End Sub
#End Region

#Region "マップ表示位置をカメラ画像上・下に移動する"
    ''' <summary>マップ表示位置をカメラ画像上・下に移動する</summary>
    ''' <param name="startTrimming">True:トリミング開始時,False:トリミング終了時</param>
    ''' <remarks>'V6.0.1.0⑪</remarks>
    Friend Shared Sub MoveTrimMapLocation(ByVal startTrimming As Boolean)
        With Form1.Instance
            If (._mapDisp) Then
                Dim location As Point
                If (startTrimming) Then
                    location = New Point(.VideoLibrary1.Left,
                                         .VideoLibrary1.Top + .VideoLibrary1.CameraArea.Height + 1)
                Else
                    location = .VideoLibrary1.Location
                End If
                .TrimMap1.Location = location
            End If
        End With
    End Sub
#End Region

#Region "frmHistoryDataの位置・サイズを設定する"
    ''' <summary>frmHistoryDataの位置・サイズを設定する</summary>
    ''' <param name="startTrimming">True:トリミング開始時,False:トリミング終了時</param>
    ''' <remarks>'V6.0.1.0⑪</remarks>
    Friend Shared Sub MoveHistoryDataLocation(ByVal startTrimming As Boolean)
        With Form1.Instance
            ''V6.0.1.0⑰↓            If (._mapDisp) Then
            ' MapボタンがＯＮのときには、表示位置を切り替える 
            If (Form1.MapOn) Then
                If (startTrimming) Then
                    .frmHistoryData.Location = .tabCmd.Location
                    .frmHistoryData.Size = .tabCmd.Size
                Else
                    .frmHistoryData.SetBounds(.HistoryDataLocation.X,
                                              .HistoryDataLocation.Y,
                                              .HistoryDataSize.Width,
                                              .HistoryDataSize.Height)
                End If
                .btnCounterClear.Visible = Not startTrimming
            Else
                .frmHistoryData.SetBounds(.HistoryDataLocation.X,
                                          .HistoryDataLocation.Y,
                                          .HistoryDataSize.Width,
                                          .HistoryDataSize.Height)
                .btnCounterClear.Visible = Not startTrimming
            End If
            ''V6.0.1.0⑰↑
        End With
    End Sub
    Friend HistoryDataLocation As Point 'V6.0.1.0⑪
    Friend HistoryDataSize As Size      'V6.0.1.0⑪
#End Region

#Region "トリミングの判定結果を数える"
    ''' <summary>トリミングの判定結果を数える</summary>
    ''' <param name="result">判定結果</param>
    ''' <returns>合計数</returns>
    ''' <remarks>'V6.0.1.0⑫</remarks>
    Public Shared Function CountTrimmingResults(ByVal result As Integer) As Integer
        Dim ret As Integer = (-1)

        Dim tr As TrimmingResult = Nothing
        If (Form1.Instance._trimmingResults.TryGetValue(result, tr)) Then
            tr.Count += 1
            ret = tr.Count
        End If

        Return ret
    End Function

    ''' <summary>トリミングの判定結果をクリアする</summary>
    ''' <remarks>'V6.0.1.0⑫</remarks>
    Public Shared Sub ClearTrimmingResult()
        With Form1.Instance
            For Each key As Integer In ._trimmingResults.Keys
                ._trimmingResults(key).Count = 0
            Next
        End With
    End Sub

    ''' <summary>トリミングの判定結果で使用する色を取得する</summary>
    ''' <param name="result">判定結果</param>
    ''' <returns>判定結果で使用する色</returns>
    ''' <remarks>'V6.0.1.0⑫</remarks>
    Public Shared Function GetResultColor(ByVal result As Integer) As Brush
        Dim ret As Brush = Nothing

        Dim tr As TrimmingResult = Nothing
        If (Form1.Instance._trimmingResults.TryGetValue(result, tr)) Then
            ret = tr.Color
        End If

        Return ret
    End Function

    ''' <summary>トリミングの判定結果と色・合計数を管理する</summary>
    ''' <remarks>'V6.0.1.0⑫</remarks>
    Private _trimmingResults As New Dictionary(Of Integer, TrimmingResult)

    ''' <summary>トリミング判定結果色・数</summary>
    ''' <remarks>'V6.0.1.0⑫</remarks>
    Private Class TrimmingResult
        Private _color As Brush
        Friend ReadOnly Property Color() As Brush
            Get
                Return _color
            End Get
        End Property

        Private _colorName As String
        Friend ReadOnly Property ColorName() As String
            Get
                If (String.IsNullOrEmpty(_colorName)) Then
                    _colorName = DirectCast(_color, SolidBrush).Color.Name
                End If
                Return _colorName
            End Get
        End Property

        Private _keyname As String
        Friend ReadOnly Property KeyName() As String
            Get
                Return _keyname
            End Get
        End Property

        Friend Count As Integer

        Public Sub New(ByVal brush As Brush, ByVal keyName As String)
            _color = brush
            _keyname = keyName
            Count = 0
        End Sub

    End Class
#End Region

#Region "トリミング結果表示マップ画像を印刷する"
    ''' <summary>トリミング結果表示マップ画像を印刷する</summary>
    ''' <remarks>'V6.0.1.0⑫</remarks>
    Friend Shared Sub PrintTrimMap()
        With Form1.Instance
            If (._mapDisp AndAlso ._printMap) Then
                Using bmp As Bitmap = .TrimMap1.GetMapImage()
                    If (bmp IsNot Nothing) Then
                        Dim sb As New StringBuilder()
                        For Each tr As TrimmingResult In ._trimmingResults.Values
                            sb.Append(tr.ColorName)
                            sb.Append(",")
                            sb.Append(tr.KeyName)
                            sb.Append(",")
                            sb.Append(tr.Count.ToString())
                            sb.Append("_")
                        Next

                        If (PrintBase.DefaultPrinterIsOffline) Then
                            Using pp As New PrintPdf(gStrTrimFileName)
                                pp.SaveFilePath =
                                        ._mapPdfDir &
                                        IO.Path.GetFileNameWithoutExtension(gStrTrimFileName) &
                                        "_" & DateTime.Now.ToString("yyyyMMdd-HHmmss") & ".pdf"
                                pp.PrintMap(bmp, sb)

                                .Z_PRINT(Form1_025)                                 ' 「通常使うプリンター」がオフラインです。
                                .Z_PRINT(String.Format(Form1_026, pp.SaveFilePath)) ' {0} を作成しました。
                            End Using
                        Else
                            Using pi As New PrintImage(gStrTrimFileName)
                                pi.PrintMap(bmp, sb)
                            End Using
                        End If
                    End If
                End Using
            End If

        End With
    End Sub
#End Region

#Region "マップ画像印刷処理のオンオフを設定する"
    Private _printMap As Boolean = False
    ''' <summary>マップ画像印刷処理のオンオフを設定する</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V6.0.1.0⑫</remarks>
    Private Sub CmdPrintMap_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CmdPrintMap.Click
        _printMap = Not _printMap
        With CmdPrintMap
            If (_printMap) Then
                .BackColor = Color.Lime
                .Text = "Print ON"
            Else
                .BackColor = SystemColors.Control
                .Text = "Print OFF"
            End If
        End With
        LblDataFileName.Select()
    End Sub
#End Region

#Region "指定ブロックの背景色を設定する"
#If False Then  'V6.0.1.3①
    ''' <summary>指定ブロック番号(加工順)の背景色を設定する</summary>
    ''' <param name="blockNo">ブロック番号</param>
    ''' <param name="color">背景色</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Public Shared Sub SetMapColor(ByVal blockNo As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Dim x, y As Integer
            Globals_Renamed.GetBlockPosition(blockNo, x, y)
            Form1.Instance.TrimMap1.TrimMapColorSet(x, y, color)
        End If
    End Sub

    ''' <summary>指定位置(X,Y)ブロックの背景色を設定する</summary>
    ''' <param name="blockPosX">ブロック位置Ｘ(1ORG)</param>
    ''' <param name="blockPosY">ブロック位置Ｙ(1ORG)</param>
    ''' <param name="color">背景色</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Public Shared Sub SetMapColor(ByVal blockPosX As Integer, ByVal blockPosY As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapColorSet(blockPosX, blockPosY, color)
        End If
    End Sub
#Else
    ''' <summary>指定ブロック番号(加工順)の背景色を設定する</summary>
    ''' <param name="plateNo">プレート処理順番号(1ORG)</param>
    ''' <param name="blockNo">ブロック処理順番号(1ORG)</param>
    ''' <param name="color">背景色</param>
    ''' <remarks>プレート対応 'V6.0.1.3①</remarks>
    Public Shared Sub SetMapColor(ByVal plateNo As Integer, ByVal blockNo As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then
            Form1.Instance.TrimMap1.TrimMapColorSet(plateNo, blockNo, color)
        End If
    End Sub

    ''' <summary>指定位置(X,Y)ブロックの背景色を設定する</summary>
    ''' <param name="platePosX">プレート位置Ｘ(1ORG)</param>
    ''' <param name="platePosY">プレート位置Ｙ(1ORG)</param>
    ''' <param name="blockPosX">ブロック位置Ｘ(1ORG)</param>
    ''' <param name="blockPosY">ブロック位置Ｙ(1ORG)</param>
    ''' <param name="color">背景色</param>
    ''' <remarks>プレート対応 'V6.0.1.3①</remarks>
    Public Shared Sub SetMapColor(ByVal platePosX As Integer, ByVal platePosY As Integer,
                                  ByVal blockPosX As Integer, ByVal blockPosY As Integer, ByVal color As Brush)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapColorSet(platePosX, platePosY, blockPosX, blockPosY, color)
        End If
    End Sub
#End If
#End Region

#Region "指定ブロックに枠線を表示する"
#If False Then  'V6.0.1.3①
    ''' <summary>指定ブロック番号(加工順)の枠線を表示する</summary>
    ''' <param name="blockNo">ブロック番号</param>
    ''' <param name="color">枠線の色</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Public Shared Sub SetMapBorder(ByVal blockNo As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Dim x, y As Integer
            Globals_Renamed.GetBlockPosition(blockNo, x, y)
            Form1.Instance.TrimMap1.TrimMapBorderSet(x, y, color)

        End If
    End Sub

    ''' <summary>指定位置(X,Y)ブロックの枠線を表示する</summary>
    ''' <param name="blockPosX">ブロック位置Ｘ(1ORG)</param>
    ''' <param name="blockPosY">ブロック位置Ｙ(1ORG)</param>
    ''' <param name="color">枠線の色</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Public Shared Sub SetMapBorder(ByVal blockPosX As Integer, ByVal blockPosY As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapBorderSet(blockPosX, blockPosY, color)
        End If
    End Sub
#Else
    ''' <summary>指定ブロック番号(加工順)の枠線を表示する</summary>
    ''' <param name="plateNo">プレート処理順番号(1ORG)</param>
    ''' <param name="blockNo">ブロック処理順番号(1ORG)</param>
    ''' <param name="color">枠線の色</param>
    ''' <remarks>プレート対応 'V6.0.1.3①</remarks>
    Public Shared Sub SetMapBorder(ByVal plateNo As Integer, ByVal blockNo As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapBorderSet(plateNo, blockNo, color)
        End If
    End Sub

    ''' <summary>指定位置(X,Y)ブロックの枠線を表示する</summary>
    ''' <param name="plateX">プレート位置Ｘ(1ORG)</param>
    ''' <param name="plateY">プレート位置Ｙ(1ORG)</param>
    ''' <param name="blockX">ブロック位置Ｘ(1ORG)</param>
    ''' <param name="blockY">ブロック位置Ｙ(1ORG)</param>
    ''' <param name="color">枠線の色</param>
    ''' <remarks>プレート対応 'V6.0.1.3①</remarks>
    Public Shared Sub SetMapBorder(ByVal plateX As Integer, ByVal plateY As Integer,
                                   ByVal blockX As Integer, ByVal blockY As Integer, ByVal color As Color)
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapBorderSet(plateX, plateY, blockX, blockY, color)
        End If
    End Sub
#End If
    ''' <summary>最後に表示したブロックの枠線を消す</summary>
    ''' <remarks>'V4.12.2.0①</remarks>
    Public Shared Sub ClearMapBorder()
        If (Form1.Instance._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            Form1.Instance.TrimMap1.TrimMapClearMapBorder()
        End If
    End Sub
#End Region

#Region "マップの表示状態変更時にMAP ON/OFFボタンの表示を設定する"
    ''' <summary>マップの表示状態変更時にMAP ON/OFFボタンの表示を設定する</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Private Sub TrimMap1_VisibleChanged(sender As Object, e As EventArgs) Handles TrimMap1.VisibleChanged
        With CmdMapOnOff
            If (TrimMap1.Visible) Then
                .BackColor = Color.Lime
                .Text = "MAP ON"
            Else
                .BackColor = SystemColors.Control
                .Text = "MAP OFF"
            End If
        End With
    End Sub
#End Region

#Region "MAP ON/OFFボタンのクリックによりマップの表示状態を切り替える"
    ''' <summary>MAP ON/OFFボタンのクリックによりマップの表示状態を切り替える</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Private Sub CmdMapOnOff_Click(sender As Object, e As EventArgs) Handles CmdMapOnOff.Click
        Me.LblDataFileName.Select()
        Select Case (giAppMode)
            Case APP_MODE_FINEADJ, APP_MODE_IDLE, APP_MODE_TRIM, APP_MODE_TRIM_AUTO
                ' 一時停止時, アイドル時
                _mapOn = Not TrimMap1.Visible
                SetTrimMapVisible(_mapOn)
            Case Else
                ' DO NOTHING
        End Select
    End Sub
    Private Shared _mapOn As Boolean = False  ' MAP ON/OFF ボタンによる設定を保持する
    ''' <summary>MAP ONであるか</summary>
    ''' <returns>Ture:ON,False:OFF</returns>
    ''' <remarks>'V4.12.2.0①</remarks>
    Friend Shared ReadOnly Property MapOn As Boolean
        Get
            Return _mapOn
        End Get
    End Property

    ''' <summary>MAP ON/OFF ボタンのEnabledを設定する</summary>
    ''' <param name="enabled">Enabled</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Friend Shared Sub SetMapOnOffButtonEnabled(ByVal enabled As Boolean)
        With Form1.Instance
            If (._mapDisp) Then         'tky.ini[DEVICE_CONST]MAP_DISP
                .CmdMapOnOff.Enabled = (enabled) AndAlso (gLoadDTFlag)
            End If
        End With
    End Sub
#End Region

#Region "マップの表示状態を設定する"
    ''' <summary>マップの表示状態を設定する</summary>
    ''' <param name="visible">True:表示,False:非表示</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Friend Shared Sub SetTrimMapVisible(ByVal visible As Boolean)
        'V6.0.1.0⑩    Private Sub SetTrimMapVisible(ByVal visible As Boolean)
        With Form1.Instance
            'V6.0.1.0⑰↓
            '            If (._mapDisp) Then       'tky.ini[DEVICE_CONST]MAP_DISP
            If (Form1.MapOn) Then
                .TrimMap1.Visible = visible
                If (visible) Then
                    .TrimMap1.BringToFront()
                Else
                    .TrimMap1.SendToBack()
                End If
            Else
                .TrimMap1.Visible = False
                .TrimMap1.SendToBack()
            End If
            'V6.0.1.0⑰↑
        End With
    End Sub
#End Region
    'V4.12.2.0①                         ↑
#Region "マップ上のブロックが選択されているかを取得する"
    ''' <summary>マップ上のブロックが選択されているかを取得する</summary>
    ''' <returns>True:選択されている, False:選択されていない</returns>
    ''' <remarks>プレート対応 'V6.0.1.3①</remarks>
    Friend Shared ReadOnly Property TrimMapSelected() As Boolean

        Get
            Return (0 < Form1.Instance.TrimMap1.GetSelectBlockCount())
        End Get
    End Property
#End Region

#Region "次の処理対象プレート番号・ブロック番号を取得する"
    ''' <summary>次の処理対象プレート番号・ブロック番号を取得する</summary>
    ''' <param name="currentPlate">現在の処理順プレート番号(1ORG)</param>
    ''' <param name="currentBlock">現在の処理順ブロック番号(1ORG)</param>
    ''' <param name="nextPlate">次の処理順プレート番号(1ORG)</param>
    ''' <param name="nextBlock">次の処理順ブロック番号(1ORG)</param>
    ''' <returns>True:処理対象ブロックあり, False:処理対象ブロックなし</returns>
    ''' <remarks>プレート対応 'V6.0.1.3①</remarks>
    Friend Shared Function GetNextSelectBlock(ByVal currentPlate As Integer, ByVal currentBlock As Integer,
                                              ByRef nextPlate As Integer, ByRef nextBlock As Integer) As Boolean
        Return Form1.Instance.TrimMap1.GetNextSelectBlock(currentPlate, currentBlock, nextPlate, nextBlock)
    End Function
#End Region

#Region "前の処理対象プレート番号・ブロック番号を取得する"
    ''' <summary>前の処理対象プレート番号・ブロック番号を取得する</summary>
    ''' <param name="currentPlate">現在の処理順プレート番号(1ORG)</param>
    ''' <param name="currentBlock">現在の処理順ブロック番号(1ORG)</param>
    ''' <param name="prevPlate">前の処理順プレート番号(1ORG)</param>
    ''' <param name="prevBlock">前の処理順ブロック番号(1ORG)</param>
    ''' <returns>True:処理対象ブロックあり, False:処理対象ブロックなし</returns>
    ''' <remarks>プレート対応 'V6.0.1.3①</remarks>
    Friend Shared Function GetPrevSelectBlock(ByVal currentPlate As Integer, ByVal currentBlock As Integer,
                                              ByRef prevPlate As Integer, ByRef prevBlock As Integer) As Boolean
        Return Form1.Instance.TrimMap1.GetPrevSelectBlock(currentPlate, currentBlock, prevPlate, prevBlock)
    End Function
#End Region

#Region "ログ表示テキストボックスを最終行までスクロールする"
    ''' <summary>ログ表示テキストボックスを最終行までスクロールする</summary>
    ''' <remarks>'#4.12.2.0④</remarks>
    Friend Shared Sub TxtLogScrollToCaret()

        Static hWnd As IntPtr = Form1.Instance.txtLog.Handle
        Const WM_SETREDRAW As Integer = &HB
        Const EM_GETLINECOUNT As Integer = &HBA
        Const EM_LINESCROLL As Integer = &HB6
        Const WM_GETTEXTLENGTH As Integer = &HE
        Const EM_SETSEL As Integer = &HB1

        Try
            SendMessage(hWnd, WM_SETREDRAW, 0, 0)                           ' 描画停止
            Dim lines As Integer = SendMessage(hWnd, EM_GETLINECOUNT, 0, 0)
            SendMessage(hWnd, EM_LINESCROLL, 0, lines)                      ' 最終行を表示

            Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)  ' 文字数取得
            SendMessage(hWnd, EM_SETSEL, len, len)                          ' カーソルを末尾へ

        Catch ex As Exception
            Dim strMSG As String = "i-TKY.TxtLogScrollToCaret() TRAP ERROR = " & ex.Message
            MessageBox.Show(Form1.Instance, strMSG)

        Finally
            SendMessage(hWnd, WM_SETREDRAW, 1, 0)                           ' 描画再開
        End Try

    End Sub
#End Region

#Region "ログ表示中文字数が制限値以上であるかを取得する"
    ''' <summary>ログ表示中文字数が制限値以上であるかを取得する</summary>
    ''' <returns>True:制限以上,False:制限未満</returns>
    ''' <remarks>'#4.12.2.0④</remarks>
    Friend Shared Function TxtLogLengthLimit() As Boolean
        Static hWnd As IntPtr = Form1.Instance.txtLog.Handle
        Const WM_GETTEXTLENGTH As Integer = &HE
        Dim len As Integer = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0)      ' 文字数取得
        Return (_txtLogClearLength <= len)
    End Function
    Private Shared _txtLogClearLength As Integer
#End Region

#Region "アライメント・θ補正結果管理クラス"
    ''' <summary>
    ''' アライメント・θ補正結果管理クラス
    ''' </summary>
    ''' <remarks>'V5.0.0.9⑤</remarks>
    Private Class PatternRecog
        ''' <summary>
        ''' パターン１・２ともに閾値以内でマッチしたかどうか
        ''' </summary>
        ''' <returns>True:マッチした,False:マッチしない</returns>
        Public ReadOnly Property IsMatch As Boolean
            Get
                Return (_th1 <= ThetaCorInfo.fCorV1) AndAlso (_th2 <= ThetaCorInfo.fCorV2)
            End Get
        End Property

        Private _isRough As Boolean
        ''' <summary>
        ''' ラフアライメントの実行結果であるかどうか
        ''' </summary>
        ''' <returns>True:ラフアライメント,False:ラフアライメントではない</returns>
        Public ReadOnly Property IsRough As Boolean
            Get
                Return _isRough
            End Get
        End Property

        Private _th1 As Double
        Private _th2 As Double

        ''' <summary>
        ''' VideoLibrary.CorrectTheta()の実行結果
        ''' </summary>
        Public ThetaCorInfo As VideoLibrary.Theta_Cor_Info

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="isRough">ラフアライメントの場合に True</param>
        ''' <param name="threshPt1">パターン１の閾値</param>
        ''' <param name="threshPt2">パターン２の閾値</param>
        Public Sub New(ByVal isRough As Boolean, ByVal threshPt1 As Double, ByVal threshPt2 As Double)
            _isRough = isRough
            _th1 = threshPt1
            _th2 = threshPt2

            ThetaCorInfo = New VideoLibrary.Theta_Cor_Info()
        End Sub
    End Class
#End Region

#Region "扉を開けてもアプリケーション終了しない状態を表示する"
    ''' <summary>扉を開けてもアプリケーション終了しない状態を表示する</summary>
    ''' <param name="isOK">扉開(True:OK, False:NG)</param>
    ''' <remarks>'V5.0.0.9⑰</remarks>
    Friend Sub SetLblDoorOpen(ByVal isOK As Boolean)
        Static text As String = lblDoorOpen.Text

        With lblDoorOpen
            If (isOK) Then
                .BackColor = Color.Lime
                .Text = text & "OK"
            Else
                .BackColor = Color.Yellow
                .Text = text & "NG"
            End If
        End With
    End Sub
#End Region

#Region "バーコード読み込み内容のリセット"
    ''' <summary>バーコード読み込み内容のリセット</summary>
    ''' <remarks>'V5.0.0.9⑲</remarks>
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRest.Click
        Dim r As MsgBoxResult

        ' バーコード情報をクリアしてもよろしいですか？
        r = MsgBox(MSG_163, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)
        If (r = MsgBoxResult.Yes) Then
            BC_Info_Disp(0)
            BarCode_Data.BC_ReadCount = 0
            BarCode_Data.BC_ReadDataFirst = ""                          ' バーコード１回目で読込んだデータ保存用
            BarCode_Data.BC_ReadDataSecound = ""                        ' バーコード２回目で読込んだデータ保存用
        End If
    End Sub
#End Region

    Private Sub CbDigSwL_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CbDigSwL.MouseWheel

        '----- V6.0.3.0_47↓ -----
        If giMoveModeWheelDisable = 0 Then
            Return
        End If
        '----- V6.0.3.0_47↑ -----

        Dim eventArgs As HandledMouseEventArgs = DirectCast(e, HandledMouseEventArgs)
        eventArgs.Handled = True

    End Sub

    '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応) -----
#Region "現在読み込まれているデータの第１抵抗のデータを表示"
    '''=========================================================================
    ''' <summary>現在読み込まれているデータの第１抵抗のみ以下のデータを表示します。
    '''          目標値、カットオフ、エッジセンスポイント、（カット６まで）のデータを表示します。
    ''' </summary>
    ''' <param name="init">true=表示を初期化</param>
    ''' <remarks>V6.1.4.0⑦</remarks>
    '''=========================================================================
    Public Sub SetFirstResData(Optional ByVal init As Boolean = False) ' V6.1.4.0_22

        Const BLANK As String = "----"

        Try
            If gTkyKnd = KND_CHIP Then  'V6.1.4.14①　CHIPの場合は従来通り
                Me.tlpFirstResData.SuspendLayout()

                ' 表示を初期化する
                lblFrdNomVal.Text = BLANK
                With Me.tlpFirstResData
                    For i As Integer = 1 To 6 Step 1
                        .Controls("lblFrdC" & i).Text = BLANK
                        .Controls("lblFrdE" & i).Text = BLANK
                    Next i
                End With

                If (False = init) Then
                    If (typResistorInfoArray Is Nothing) Then Return

                    Dim tmpRes As ResistorInfo = typResistorInfoArray(1)
                    lblFrdNomVal.Text = String.Format("{0:F5}", tmpRes.dblTrimTargetVal)

                    Dim tmpCut() As CutList = tmpRes.ArrCut
                    With Me.tlpFirstResData
                        If (tmpCut IsNot Nothing) AndAlso (tmpRes.intCutCount <= tmpCut.Length) Then
                            Dim min As Integer = Math.Min(tmpRes.intCutCount, tmpCut.Length)
                            min = Math.Min(min, 6)
                            ' 最大6ｶｯﾄまで第1抵抗のｶｯﾄﾃﾞｰﾀを表示する
                            For i As Integer = 1 To min Step 1
                                'V6.1.4.0_51                            .Controls("lblFrdC" & i).Text = String.Format("{0:F2}", tmpCut(i).dblCutOff)
                                .Controls("lblFrdC" & i).Text = String.Format("{0:F3}", tmpCut(i).dblCutOff)        'V6.1.4.0_51
                                If tmpCut(i).strCutType = CNS_CUTP_ES Or tmpCut(i).strCutType = CNS_CUTP_ES2 Then
                                    .Controls("lblFrdE" & i).Text = String.Format("{0:F2}", tmpCut(i).dblESPoint)
                                End If
                            Next i
                        End If
                    End With
                End If
                'V6.1.4.14①↓NETの場合を追加
            Else
                Me.pnlFirstResDataNET.SuspendLayout()

                ' 表示を初期化する
                lblNETNomVal.Text = BLANK
                With Me.tlpFirstResDataNET
                    For i As Integer = 1 To 4 Step 1
                        For j As Integer = 1 To 10 Step 1
                            .Controls("Res" & i & "Cut" & j).Text = BLANK
                        Next j
                    Next i
                End With

                If (False = init) Then
                    If (typResistorInfoArray Is Nothing) Then Return

                    lblNETNomVal.Text = String.Format("{0:F5}", typResistorInfoArray(1).dblTrimTargetVal)

                    With Me.tlpFirstResDataNET
                        For i As Integer = 1 To Math.Min(typPlateInfo.intResistCntInGroup, 4) Step 1
                            For j As Integer = 1 To Math.Min(typResistorInfoArray(i).intCutCount, 10) Step 1
                                .Controls("Res" & i & "Cut" & j).Text = String.Format("{0:F3}", typResistorInfoArray(i).ArrCut(j).dblCutOff)
                            Next j
                        Next i
                    End With
                End If
            End If
            'V6.1.4.14①↑

        Catch ex As Exception
            Dim strMsg As String = "i-TKY.SetFirstResData() TRAP ERROR = " & ex.Message
            MessageBox.Show(strMsg)
        Finally
            Me.tlpFirstResData.ResumeLayout()
        End Try
    End Sub
#End Region
    '----- V6.1.4.0_22↑ -----

    'V4.7.3.5①↓
    Private Sub CutOffEsEditButton_Click(sender As Object, e As EventArgs) Handles CutOffEsEditButton.Click
        Try



            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            Call RedrawDisplayDistribution(True)
            ' トリマ装置状態を動作中に設定する

            ' ローダへトリマ動作中信号を送信する
            Call SetLoaderIO(COM_STS_TRM_STATE, &H0)                ' ローダ出力(ON=トリマ動作中, OFF=なし)
            giAppMode = APP_MODE_EDIT

            If (gLoadDTFlag = False) Then                           ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ未ﾛｰﾄﾞ ?
                ' "トリミングパラメータデータをロードするか新規作成してください"
                Call System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                GoTo STP_END
            End If

            Call System1.OperationLogging(gSysPrm, "カットオフ値編集", "")

            ' コンソールボタンのランプ状態を設定する
            Call LAMP_CTRL(LAMP_START, False)                       ' STARTﾗﾝﾌﾟ消灯 
            Call LAMP_CTRL(LAMP_RESET, False)                       ' RESETﾗﾝﾌﾟ消灯 

            Call Form1Button(0)                                     ' コマンドボタンを無効にする

            If (gTkyKnd = KND_CHIP) Then     'V6.1.4.14①
                'V6.1.4.14①↓
                Dim objFormCutOffEsPointEnter As New FormCutOffEsPointEnter()
                objFormCutOffEsPointEnter.ShowDialog()
                objFormCutOffEsPointEnter.Dispose()
            Else
                Dim objFormCutOffEnter As New FormCutOffEnter()                 'V6.1.4.14①
                objFormCutOffEnter.ShowDialog()
                objFormCutOffEnter.Dispose()
            End If
            'V6.1.4.14①↑

            '-------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------
STP_END:
            '統計表示処理の状態変更
            Call RedrawDisplayDistribution(False)

            If (gSysPrm.stTMN.gsKeimei = MACHINE_TYPE_SL432) And (gSysPrm.stSPF.giWithStartSw = 0) Then
                Dim swStatus As Integer
                Dim interlockStatus As Integer
                Dim r As Short
                r = INTERLOCK_CHECK(interlockStatus, swStatus)
                If r = cFRS_NORMAL And interlockStatus = INTERLOCK_STS_DISABLE_NO Then
                    ' スライドカバーの状態取得（INTRIMではIO取得のみの為、エラーが返る事はない）
                    r = SLIDECOVER_GETSTS(swStatus)
                    If r = cFRS_NORMAL And swStatus = SLIDECOVER_CLOSE Then
                        ' スライドカバー閉
                        r = Me.System1.Form_MsgDisp(MSG_SPRASH31, MSG_SPRASH52, &HFF, &HFF0000)
                    End If
                End If
            End If

            ' トリマ装置状態をアイドル中に設定する
            Call TrimStateOff()
            Exit Sub

        Catch ex As Exception
            MessageBox.Show("i-TKY.CutOffEsEditButton_Click() TRAP ERROR = " & ex.Message)
        End Try

    End Sub
    'V4.7.3.5①↑

    'V6.1.4.14①↓
    Private Sub CutOffEsEditButtonNET_Click(sender As Object, e As EventArgs) Handles CutOffEsEditButtonNET.Click
        Try
            Call CutOffEsEditButton_Click(sender, e)
        Catch ex As Exception
            MessageBox.Show("i-TKY.CutOffEsEditButtonNET_Click() TRAP ERROR = " & ex.Message)
        End Try
    End Sub
    'V6.1.4.14①↑

End Class

#Region "各コマンド実行サブフォーム用共通インターフェース"
''' <summary>各コマンド実行サブフォーム用共通インターフェース</summary>
''' <remarks>'V6.0.0.0⑪</remarks>
Public Interface ICommonMethods
    ''' <summary>サブフォーム処理実行</summary>
    ''' <returns>実行結果 sGetReturn</returns>
    ''' <remarks>'V6.0.0.0⑬</remarks>
    Function Execute() As Integer

    ''' <summary>サブフォームKeyDown時の処理</summary>
    ''' <param name="e"></param>
    Sub JogKeyDown(ByVal e As KeyEventArgs)

    ''' <summary>サブフォームKeyUp時の処理</summary>
    ''' <param name="e"></param>
    Sub JogKeyUp(ByVal e As KeyEventArgs)

    ''' <summary>カメラ画像クリック位置を画像センターに移動する処理</summary>
    ''' <param name="distanceX">画像センターからの距離X</param>
    ''' <param name="distanceY">画像センターからの距離Y</param>
    Sub MoveToCenter(ByVal distanceX As Decimal, ByVal distanceY As Decimal)
End Interface
#End Region

'=============================== END OF FILE ===============================