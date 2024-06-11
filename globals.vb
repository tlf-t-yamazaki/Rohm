'===============================================================================
'   Description  : グローバル定数の定義
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Runtime.CompilerServices                                 ' V6.0.3.0⑬ For Extension()
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DefWin32Fnc                                  'V4.9.0.0①
Imports LaserFront.Trimmer.DllJog                                       'V6.0.0.0⑧
Imports LaserFront.Trimmer.DllSysPrm.SysParam
Imports LaserFront.Trimmer.TrimData.DataManager                         'V5.0.0.8①
Imports TKY_ALL_SL432HW.My.Resources                                    'V4.4.0.0-0
Imports TrimClassLibrary.IniFile.Tky                                    'V2.0.0.0⑩
Imports TrimControlLibrary                                              '#4.12.2.0①

Module Globals_Renamed

#Region "グローバル定数/変数の定義"
    '===========================================================================
    '   グローバル定数/変数の定義
    '===========================================================================
    '-------------------------------------------------------------------------------
    '   DLL定義
    '-------------------------------------------------------------------------------
    '----- WIN32 API -----
    ' ウィンドウ表示の操作のAPI
    Public Declare Function SetWindowPos Lib "user32" (ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
    Public Const HWND_TOPMOST As Short = -1                 ' ウィンドウを最前面に表示
    Public Const SWP_NOSIZE As Short = &H1S                 ' 現在のサイズを維持
    Public Const SWP_NOMOVE As Short = &H2S                 ' 現在の位置を維持

    '---------------------------------------------------------------------------
    '   アプリケーション名/アプリケーション種別/アプリケーションモード
    '---------------------------------------------------------------------------
    '----- 強制終了用アプリケーション -----
    Public Const APP_FORCEEND As String = "c:\Trim\ForceEndProcess.exe"

    '----- パス　-----
    Public Const OCX_PATH As String = "c:\Trim\ocx\"        '----- OCX登録パス
    Public Const DLL_PATH As String = "c:\Trim\"            '----- DLL登録パス

    Friend Const TKY_INI As String = DLL_PATH & "tky.ini"               'V6.0.0.1②
    Friend Const TKYSYS_INI As String = DLL_PATH & "TKYSYS.INI"         'V6.0.0.1②

    '----- アプリケーション名 -----
    Public Const APP_TKY As String = "TKY"
    Public Const APP_CHIP As String = "TKYCHIP"
    Public Const APP_NET As String = "TKYNET"

    '----- アプリケーション種別 -----
    Public Const KND_TKY As Short = 0
    Public Const KND_CHIP As Short = 1
    Public Const KND_NET As Short = 2

    Public Const MACHINE_TYPE_SL432 As String = "SL432R"                 ' 系名
    Public Const MACHINE_TYPE_SL436 As String = "SL436R"                 ' 系名
    Public Const MACHINE_TYPE_SL436S As String = "SL436S"                ' 系名

    '----- V1.13.0.0⑦↓ -----
    ' 装置タイプ(0=Sl43xR, 1=SL432RW)
    Public Const KEY_TYPE_R As Short = 0
    Public Const KEY_TYPE_RW As Short = 1
    Public Const KEY_TYPE_RS As Short = 2                               'V2.0.0.0⑩
    '----- V1.13.0.0⑦↑ -----

    Public gAppName As String                               ' アプリケーション名
    Public gTkyKnd As Short                                 ' アプリケーション種別

    '----- 画像表示プログラムの表示位置 -----
    'Public Const FORM_X As Integer = 4                     ' コントロール上部左端座標X ###050
    'Public Const FORM_Y As Integer = 20                    ' コントロール上部左端座標Y ###050
    Public Const FORM_X As Integer = 0                      ' コントロール上部左端座標X ###050
    Public Const FORM_Y As Integer = 0                      ' コントロール上部左端座標Y ###050

    'サブフォームの表示位置目印の表示位置オフセット
    'Public Const DISPOFF_SUBFORM_TOP As Integer = 12
    Public Const DISPOFF_SUBFORM_TOP As Integer = 0         'V4.10.0.0③

    '----- シグナルタワー制御種別 -----                     ' ###007
    Public Const SIGTOWR_NORMAL As Short = 0                ' 標準３色制御
    Public Const SIGTOWR_SPCIAL As Short = 1                ' ４色制御(日立ｵｰﾄﾓｰﾃｨﾌﾞ殿特注)

    '----- アプリケーションモード ----- (注)OcxSystem定義と一致させる必要有り
    'V5.0.0.9⑰    Public giAppMode As Short
    Private _giAppMode As Short         'V5.0.0.9⑰  ↓
    Public Property giAppMode As Short
        Get
            Return _giAppMode
        End Get
        Set(value As Short)
            If (Form1.Instance IsNot Nothing) Then
                Select Case (value)
                    Case APP_MODE_IDLE, APP_MODE_LOAD, APP_MODE_SAVE, APP_MODE_EDIT,
                        APP_MODE_LOGGING, APP_MODE_FINEADJ, APP_MODE_LDR_ALRM

                        Form1.Instance.SetLblDoorOpen(True)

                    Case Else
                        Form1.Instance.SetLblDoorOpen(False)
                End Select
            End If
            _giAppMode = value
        End Set
    End Property                        'V5.0.0.9⑰  ↑

    Public Const APP_MODE_IDLE As Short = 0                 ' トリマ装置アイドル中
    Public Const APP_MODE_LOAD As Short = 1                 ' ファイルロード(F1)
    Public Const APP_MODE_SAVE As Short = 2                 ' ファイルセーブ(F2)
    Public Const APP_MODE_EDIT As Short = 3                 ' 編集画面      (F3)
    '                                                       ' 空き
    Public Const APP_MODE_LASER As Short = 5                ' レーザー調整  (F5)
    Public Const APP_MODE_LOTCHG As Short = 6               ' ロット切替    (F6) ※ユーザプロ対応
    Public Const APP_MODE_PROBE As Short = 7                ' プローブ      (F7)
    Public Const APP_MODE_TEACH As Short = 8                ' ティーチング  (F8)
    Public Const APP_MODE_RECOG As Short = 9                ' パターン登録  (F9)
    Public Const APP_MODE_EXIT As Short = 10                ' 終了 　　　　 (F11)
    Public Const APP_MODE_TRIM As Short = 11                ' トリミング中
    Public Const APP_MODE_TRIM_AUTO As Short = 111          ' 自動運転のトリミング中 'V4.7.0.0⑰
    Public Const APP_MODE_CUTPOS As Short = 12              ' ｶｯﾄ位置補正   (S-F8)
    Public Const APP_MODE_PROBE2 As Short = 13              ' プローブ2     (F10) ※ユーザプロ対応
    Public Const APP_MODE_LOGGING As Short = 14             ' ロギング      (F6) 
    '----- V1.13.0.0③↓ -----
    ' TKY系オプション
    Public Const APP_MODE_APROBEREC As Short = 15           ' オートプローブ登録
    Public Const APP_MODE_APROBEEXE As Short = 16           ' オートプローブ実行
    Public Const APP_MODE_IDTEACH As Short = 17             ' ＩＤティーチング
    Public Const APP_MODE_SINSYUKU As Short = 18            ' 伸縮補正(画像登録)
    Public Const APP_MODE_MAP As Short = 19                 ' 伸縮補正(実行ブロック選択)
    '----- V1.13.0.0③↑ -----

    Public Const APP_MODE_MAINTENANCE As Short = 20         ' メンテナンスモード
    Public Const APP_MODE_PROBE_CLEANING As Short = 21      ' プローブクリーニング機能ティーチング 

    ' CHIP,NET系
    Public Const APP_MODE_TTHETA As Short = 40              ' Ｔθ(θ角度補正)ティーチング
    Public Const APP_MODE_TX As Short = 41                  ' TXティーチング
    Public Const APP_MODE_TY As Short = 42                  ' TYティーチング
    Public Const APP_MODE_TY2 As Short = 43                 ' TY2ティーチング
    Public Const APP_MODE_EXCAM_R1TEACH As Short = 44       ' 外部カメラR1ティーチング【外部カメラ】
    Public Const APP_MODE_EXCAM_TEACH As Short = 45         ' 外部カメラティーチング【外部カメラ】
    Public Const APP_MODE_CARIB_REC As Short = 46           ' 画像登録(キャリブレーション補正用)【外部カメラ】
    Public Const APP_MODE_CARIB As Short = 47               ' キャリブレーション【外部カメラ】
    Public Const APP_MODE_CUTREVISE_REC As Short = 48       ' 画像登録(カット位置補正用)【外部カメラ】
    Public Const APP_MODE_CUTREVIDE As Short = 49           ' カット位置補正【外部カメラ】
    Public Const APP_MODE_AUTO As Short = 50                ' 自動運転　　　
    Public Const APP_MODE_LOADERINIT As Short = 51          ' ローダ原点復帰
    Public Const APP_MODE_LDR_ALRM As Short = 52            ' ローダアラーム画面    '###088
    Public Const APP_MODE_FINEADJ As Short = 53             ' 一時停止画面          '###088

    Public Const APP_MODE_INTEGRATED As Short = 54           ' 統合登録調整   'V4.10.0.0③

    ' NET系
    Public Const APP_MODE_CIRCUIT As Short = 60             ' サーキットティーチング

    '---------------------------------------------------------------------------
    '----- 機能選択定義テーブルのｲﾝﾃﾞｯｸｽ定義 -----          '                         TKY CHIP NET
    '                                                       '                (○:標準,△:ｵﾌﾟｼｮﾝ,×:未ｻﾎﾟｰﾄ)
    Public Const F_LOAD As Short = 0                        ' LOADボタン              ○  ○   ○
    Public Const F_SAVE As Short = 1                        ' SAVEボタン              ○  ○   ○
    Public Const F_EDIT As Short = 2                        ' EDITボタン              ○  ○   ○
    Public Const F_LASER As Short = 3                       ' LASERボタン             ○  ○   ○
    Public Const F_LOG As Short = 4                         ' LOGGINGボタン           ○  ○   ○
    Public Const F_PROBE As Short = 5                       ' PROBEボタン             ○  ○   ○
    Public Const F_TEACH As Short = 6                       ' TEACHボタン             ○  ○   ○
    Public Const F_CUTPOS As Short = 7                      ' CUTPOSボタン            △  ×   ×
    Public Const F_RECOG As Short = 8                       ' RECOGボタン             ○  ○   ○
    ' CHIP,NET系
    Public Const F_TTHETA As Short = 9                      ' Tθボタン               ×  △   △
    Public Const F_TX As Short = 10                         ' TXボタン                ×  ○   ○
    Public Const F_TY As Short = 11                         ' TYボタン                ×  ○   ○
    Public Const F_TY2 As Short = 12                        ' TY2ボタン               ×  △   △
    Public Const F_EXR1 As Short = 13                       ' 外部ｶﾒﾗR1ﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ    ×  △   △
    Public Const F_EXTEACH As Short = 14                    ' 外部ｶﾒﾗﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ      ×  △   △
    Public Const F_CARREC As Short = 15                     ' ｷｬﾘﾌﾞﾚｰｼｮﾝ補正登録ﾎﾞﾀﾝ  ×  △   △
    Public Const F_CAR As Short = 16                        ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾎﾞﾀﾝ          ×  △   △
    Public Const F_CUTREC As Short = 17                     ' ｶｯﾄ補正登録ﾎﾞﾀﾝ         ×  △   △
    Public Const F_CUTREV As Short = 18                     ' ｶｯﾄ位置補正ﾎﾞﾀﾝ         ×  △   △
    ' NET系
    Public Const F_CIRCUIT As Short = 19                    ' ｻｰｷｯﾄﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ        ×  ×   ○

    ' SL436R CHIP,NET系 
    Public Const F_AUTO As Short = 20                       ' AUTOボタン              -   ○   ○
    Public Const F_LOADERINI As Short = 21                  ' LOADER INITボタン       -   ○   ○

    '----- V1.13.0.0③↓ -----
    ' TKY系オプション
    Public Const F_APROBEREC As Short = 22                  ' ｵｰﾄプローブ登録ﾎﾞﾀﾝ     △  ×   ×
    Public Const F_APROBEEXE As Short = 23                  ' ｵｰﾄプローブ実行ﾎﾞﾀﾝ     △  ×   ×
    Public Const F_IDTEACH As Short = 24                    ' IDﾃｨｰﾁﾝｸﾞﾎﾞﾀﾝ           △  ×   ×
    Public Const F_SINSYUKU As Short = 25                   ' 伸縮登録ﾎﾞﾀﾝ            △  ×   ×
    Public Const F_MAP As Short = 26                        ' MAP選択                 △  ×   ×
    '----- V1.13.0.0③↑ -----

    ' ↓↓↓ V3.1.0.0③ 2014/12/02
    Public Const F_LOT As Short = 27                        ' ロット番号ボタン
    Public Const F_MAINTENANCE As Short = 28                ' メンテナンスボタン
    Public Const F_PROBE_CLEANING As Short = 29             ' プローブクリーニングボタン
    ' ↑↑↑ V3.1.0.0③ 2014/12/02

    Public Const F_INTEGRATED As Short = 30                 ' 統合登録調整ボタン 'V4.10.0.0③
    Public Const F_RECOG_ROUGH As Short = 31                ' ラフアライメント用画像登録ボタン  'V5.0.0.9④
    Public Const F_FOLDEROPEN As Short = 32                ' フォルダ表示ボタン(生産管理データ) V6.1.4.0⑥

    Public Const MAX_FNCNO As Short = 33                    ' 機能選択定義テーブルのデータ数

    '----- V6.1.4.0③↓(KOA EW殿SL432RD対応) -----
    '' ログ表示域のサイズと位置(標準版)
    'Public Const TXTLOG_SIZEX_ORG As Integer = 614
    'Public Const TXTLOG_SIZEY_ORG As Integer = 642
    'Public Const TXTLOG_LOCATIONX_ORG As Integer = 653
    'Public Const TXTLOG_LOCATIONY_ORG As Integer = 49
    'Public Const GRPMODE_LOCATIONX_ORG As Integer = 653
    'Public Const GRPMODE_LOCATIONY_ORG As Integer = 691
    'Public Const TABCMD_LOCATIONX_ORG As Integer = 610
    'Public Const TABCMD_LOCATIONY_ORG As Integer = 184
    'Public Const CMDEND_LOCATIONX_ORG As Integer = 1114
    'Public Const CMDEND_LOCATIONY_ORG As Integer = 967

    ' ログ表示域のサイズと位置(KOA EW殿)
    Public Const TXTLOG_SIZEX_KOAEW As Integer = 628
    Public Const TXTLOG_SIZEY_KOAEW As Integer = 669
    Public Const TXTLOG_LOCATIONX_KOAEW As Integer = 650
    Public Const TXTLOG_LOCATIONY_KOAEW As Integer = 49
    Public Const GRPMODE_LOCATIONX_KOAEW As Integer = 650
    Public Const GRPMODE_LOCATIONY_KOAEW As Integer = 718
    Public Const TABCMD_LOCATIONX_KOAEW As Integer = 650
    Public Const TABCMD_LOCATIONY_KOAEW As Integer = 791
    Public Const CMDEND_LOCATIONX_KOAEW As Integer = 1110
    Public Const CMDEND_LOCATIONY_KOAEW As Integer = 982
    '----- V6.1.4.0③↑ -----

    '---------------------------------------------------------------------------
    '   最大値/最小値
    '---------------------------------------------------------------------------
    Public Const cMAXOptFlgNUM As Short = 5                 ' OcxSystem用ｺﾝﾊﾟｲﾙｵﾌﾟｼｮﾝの数 (最大5個)

    '----- 各入力項目の範囲 -----
    Public Const gMIN As Short = 0
    Public Const gMAX As Short = 1

    '----- ZZMOVE()の移動指定 -----
    Public Const MOVE_RELATIVE As Short = 0                 ' 相対値指定 
    Public Const MOVE_ABSOLUTE As Short = 1                 ' 絶対値指定

    '----- ZINPSTS()の入力箇所指定  -----
    Public Const GET_CONSOLE_INPUT As Short = 1             ' コンソール
    Public Const GET_INTERLOCK_INPUT As Short = 2           ' インターロック

    '----- 画像登録用パラメータ -----
    Public Const PTN_NUM_MAX As Short = 50                  ' テンプレート番号(1-50)
    Public Const GRP_NUM_MAX As Short = 999                 ' ﾃﾝﾌﾟﾚｰﾄｸﾞﾙｰﾌﾟ番号(1-999)

    Public Const INIT_THRESH_VAL As Double = 0.7            ' 閾値初期値
    Public Const INIT_CONTRAST_VAL As Integer = 216         ' コントラスト初期値
    Public Const INIT_BRIGHTNESS_VAL As Integer = 0         ' 輝度初期値
    Public Const MIN_CONTRAST_VAL As Integer = 0            ' コントラスト最小値
    Public Const MAX_CONTRAST_VAL As Integer = 511          ' コントラスト最大値
    Public Const MIN_BRIGHTNESS_VAL As Integer = -128       ' 輝度最小値
    Public Const MAX_BRIGHTNESS_VAL As Integer = 127        ' 輝度最大値

    '----- ローダ用 ----- 
    Public Const LALARM_COUNT As Integer = 128              ' 最大アラーム数
    Public Const MG_UP As Integer = 1                       ' マガジンＵＰ      2013.01.28  '###182
    Public Const MG_DOWN As Integer = 0                     ' マガジンＤＯＷＮ  2013.01.28  '###182

    '----- マーキング抵抗番号 -----
    Public Const MARKING_RESNO_SET As Integer = 1000        ' 抵抗番号1000番以降はマーキング用の抵抗番号
    Public Const cMAXcMARKINGcSTRLEN As Short = 18          ' マーキング文字列最大長(byte)
    Public Const cResultAry As Integer = 999                ' トリミング結果データの最大数 V1.23.0.0⑥

    '---------------------------------------------------------------------------
    '   システムパラメータ(形式はDllSysprm.dllで定義)
    '---------------------------------------------------------------------------
    Public tkyIni As New TkyIni()                           ' システムパラメータアクセスオブジェクト 'V2.0.0.0⑩　
    Public gKeiTyp As Integer = tkyIni.TMENU.KEITYP.Get(Of Integer)()   'V2.0.0.0⑩　装置タイプ(0=Rタイプ, 1=RWタイプ, 2=RSタイプ) 
#Region "giChipEditEx"  'V4.10.0.0②      ↓
    Private _giChipEditEx As Integer? = Nothing
    ''' <summary>SL436RでCHIPの場合にTrimDataEditorExを使用しない=0,使用する=0でない</summary>
    ''' <value>tky.ini[TMENU]CHIP_EDITEXの値を設定する</value>
    ''' <returns>使用しない=0,使用する=0でない</returns>
    ''' <remarks>'V4.10.0.0②</remarks>
    Public Property giChipEditEx() As Integer
        Get
            If (False = _giChipEditEx.HasValue) Then
                'If (KEY_TYPE_R <> gKeiTyp) OrElse (MACHINE_TYPE_SL436 <> gSysPrm.stTMN.gsKeimei) OrElse (KND_CHIP <> gTkyKnd) Then
                If (KEY_TYPE_R <> gKeiTyp) OrElse (KND_CHIP <> gTkyKnd) Then    'V4.10.0.0⑥
                    ' Rではない、またはCHIPではない
                    _giChipEditEx = 0       ' 使用しない
                End If
            End If
            Return _giChipEditEx
        End Get
        Set(ByVal value As Integer)
            If (KEY_TYPE_R <> gKeiTyp) OrElse (KND_CHIP <> gTkyKnd) Then        'V4.10.0.0⑥
                ' Rではない、またはCHIPではない
                _giChipEditEx = 0       ' 使用しない
            Else
                _giChipEditEx = value
            End If
        End Set
    End Property
#End Region             'V4.10.0.0②      ↑
    Public gMachineType As Integer  'V4.10.0.0⑩
    Public gDllSysprmSysParam_definst As New LaserFront.Trimmer.DllSysPrm.SysParam
    Public gSysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM                  ' システムパラメータ
    Public OptVideoPrm As LaserFront.Trimmer.DllSysPrm.SysParam.OPT_VIDEO_PRM            ' Video.ocx用オプション定義
    Public giTrimExe_NoWork As Short = 0                    ' 手動モード時、載物台に基板なしでトリミング実行する(0)/しない(1)　###240
    Public giTenKey_Btn As Short = 0                        ' 一時停止画面での「Ten Key On/Off」ボタンの初期値(0:ON(既定値), 1:OFF)　###268
    Public giBpAdj_HALT As Short = 0                        ' 一時停止画面での「BPオフセット調整する/しない」(0:調整する(既定値), 1:調整しない)　###269
    Public giMachineKd As Integer = 0                       ' 装置タイプ(0=Sl43xR, 1=SL432RW) V1.13.0.0⑦
    Public giFullCutZpos As Integer = 0                     ' 強制カットモード(x5モード)時のZの位置指定(0=OFF位置, 1=STEP位置, 2=ON位置) V3.0.0.1①
    Public gdblStg2ndOrgX As Double = 0.0                   ' ステージ第二原点位置X V1.13.0.0⑧
    Public gdblStg2ndOrgY As Double = 0.0                   ' ステージ第二原点位置Y V1.13.0.0⑧
    Public gdblStgOrgMoveX As Double = 0.0                  ' 基板交換時とAPP終了時のステージ位置X V1.13.0.0⑩
    Public gdblStgOrgMoveY As Double = 0.0                  ' 基板交換時とAPP終了時のステージ位置Y V1.13.0.0⑩
    Public giAutoPwr As Short = 0                           ' オートパワー調整を(1=する/0=しない) (FL用) 'V1.14.0.0②
    Public giNoDspAttRate As Short = 0                      ' 減衰率を(0=表示する/1=表示しない)(FL以外) 'V1.16.0.0⑭
    Public giAutoModeDataSelect As Short = 0                ' 自動運転時のデータ選択画面にロード済みデータ名を表示(0=しない(標準)/1=する) V1.18.0.0⑧
    Public giAutoModeContinue As Short = 0                  ' 自動運転時のマガジン終了時、マガジン交換して自動運転を(0=続行しない(標準), 1=続行する)) V1.18.0.0⑨
    Public giBtnPwrCtrl As Short = 0                        ' Power Ctrl ON/OFFボタンを1(表示する), 0(表示しない) V1.18.0.4①
    Public gsEDIT_DIGITNUM As String = "0.00000"            ' 小数桁数編集用 V1.16.0.0③
    '----- V4.0.0.0-40↓ -----
    ' ステージYの原点位置
    Public giStageYOrg As Integer = 0                       ' ステージYの原点位置(0=下(標準), 1=上(SL436S時))
    Public giStageYDir As Integer = 1                       ' ステージYの移動方向(CW(1), CCW(-1))

    Public Const STGY_ORG_DW As Integer = 0                 ' 下(標準)
    Public Const STGY_ORG_UP As Integer = 1                 ' 上(SL436S時)

    Public Const STGY_DIR_CW As Integer = 1                 ' CW(標準)
    Public Const STGY_DIR_CCW As Integer = -1               ' CCW(SL436S時)
    '----- V4.0.0.0-40↑ -----

    Public giSummary_Log As Integer = 0                     ' サマリログ出力(0=出力しない, 1=出力する(シナジー)) V1.22.0.0④              
    Public giFLPrm_Ass As Short = 0                         ' 加工条件ファイルの指定方法(0=トリミングデータ毎, 1=1つ固定) V2.0.0.0⑤
    Public gsFLPrmFile As String = ""                       ' 加工条件ファイル名　V2.0.0.0⑤
    Public giDspScreenKeybord As Short = 0                  ' クリーンキーボード表示の有無(0=表示する, 1=表示しない) 'V2.0.0.0⑦(V1.22.0.0⑧)
    Public giBtn_EdtLock As Short = 0                       ' EDIT LOCK/UNLOCKボタンを(0=表示しない, 1=表示する) シンプルトリマ用 V2.0.0.0_25
    Public dblCalibHoseiX As Double = 0.0                   ' キャリブレーション補正率　'V1.20.0.0⑦
    Public gsComPort As String = "COM6"                     ' COMポート番号(バーコードリーダ用) V1.23.0.0①
    Public giProbeCheck As Short = 0                        ' プローブチェック機能の有効/無効(0=無効, 1=有効) V1.23.0.0⑦
    Public giDspCmdName As Short = 0                        ' 画面にコマンド名を表示(0=しない, 1=する) V1.18.0.1①
    Public giDoorLock As Short = 0                          ' 電磁ロック機能(0=なし, 1=あり)V1.18.0.1⑧
    Public giPltCountMode As Short = 0                      ' 基板枚数をカウントするモード (０：通常, １：x3,x5モードでもカウントする) V6.0.3.0①
    Public giAfterDecPoint As Short = 0                     ' トリミング目標値の少数部桁数指定(０：通常(5桁), １：7桁) V6.0.3.0①

    '----- V4.11.0.0①↓ (WALSIN殿SL436S対応) -----
    Public giTargetOfs As Short = 0                         ' 目標値オフセットの有効/無効(0=無効, 1=有効)　
    Public giPwrChkPltNum As Short = 0                      ' オートパワーチェック基板枚数指定の有/無(0=なし, 1=あり) 
    Public giPwrChkTime As Short = 0                        ' オートパワーチェック時間(分)指定の有/無(0=なし, 1=あり) 
    Public giTrimTimeOpt As Short = 0                       ' タクト表示時に一時停止時間を(0=含める(標準), 1=含めない)
    Public giStartBlkAss As Short = 0                       ' トリミング開始ブロック番号指定の有効/無効(0=無効, 1=有効)　
    Public giSubstrateInvBtn As Short = 0                   ' 一時停止画面での「基板投入」ボタンの有効/無効(0=無効, 1=有効)　
    '----- V4.11.0.0①↑ -----

    '----- V6.1.4.0①↓(KOA EW殿SL432RD対応) 【レーザーパワーモニタリング機能】-----
    Public giLotChange As Short = 0                         ' ロット切替え機能の有無 (０：なし, １：あり)
    Public giFileMsgNoDsp As Short = 0                      ' ファイルバージョンチェック等のメッセージ表示の有/無(0:あり(標準), 1:なし)
    Public gbQRCodeReaderUse As Boolean = False             ' ＱＲコードリーダ使用/不使用　V6.1.4.0_22
    Public gbQRCodeReaderUseTKYNET As Boolean = False       ' TKY-NETのＱＲコードリーダ使用/不使用　'V6.1.4.10②
    Public giLaserrPowerMonitoring As Integer               ' レーザーパワーのモニタリングフラグ　0：無し,1：自動運転開始時,2：エントリーロット毎,999隠しコマンド手動でも実行 V6.1.4.0_35
    Public gdFullPowerLimit As Double                       ' レーザーパワーのモニタリングリミット
    Public gdFullPowerQrate As Double                       ' レーザーパワーのモニタリングQレート10KHz
    Public gbLaserPowerMonitoring As Boolean = False        ' レーザーパワーのモニタリング実行有無
    '----- V6.1.4.0①↑ -----
    'V6.1.4.2①↓トリミングカット位置ズレ暫定ソフト[自動キャリブレーション補正実行]
    Public giAutoCalibration As Integer = 0                 ' 自動キャリブレーション補正実行0:無し、>0：実行
    Public giAutoCalibCounter As Integer = 0                ' 自動キャリブレーション補正実行カウンター
    Public giAutoCalibPlateCounter As Integer = 0           ' 自動キャリブレーション用処理基板カウンター
    Public gbAutoCalibration As Boolean = False             ' 自動キャリブレーション補正 実行中:True 実行無し:False
    Public gbAutoCalibrationResult As Boolean = True        ' 自動キャリブレーション補正結果 正常終了:True 異常終了:False
    Public gbAutoCalibrationExecute As Boolean = False      ' 自動キャリブレーション補正結果 実行:True 無し:False
    Public gbAutoCalibrationLog As Boolean = False          ' カット位置ずれログ出力
    'V6.1.4.2①↑
    '----- V6.1.4.0_22↓(KOA EW殿SL432RD対応)【ＱＲコードリード機能】 -----
    Public giQrCodeType As Short = 0                        ' QR_CODEタイプ(0=ローム殿, 1=KOA EW殿)
    Public Enum QrCodeType As Integer
        Rome = 0
        KoaEw = 1
    End Enum
    '----- V6.1.4.0_22↑ -----
    '----- V6.1.4.0③↓ -----
    ' gSysPrm.stLOG.giLoggingType2定義 
    Public Enum LogType2 As Integer                         ' 特注ﾛｸﾞ出力単位(0:抵抗毎,1:ｶｯﾄ毎 )※林電工殿向け特注←現在未使用?
        Reg = 0                                             ' 抵抗毎(標準)
        Cut = 1                                             ' カット毎 
        Reg_KoaEw = 2                                       ' 抵抗毎(KOA EW特注)
    End Enum
    '----- V6.1.4.0③↑ -----

    Public giCpk_Disp_Off As Boolean                        ' ＣＰＫ表示オフ'V5.0.0.4④
    Public gbControllerInterlock As Boolean                 ' 外部機器によるインターロックの有無（真田KOA殿の温度コントローラのインターロックに使用）'V5.0.0.6①
    Public gbLoaderSecondPosition As Boolean                ' ＳＬ４３２Ｒでローダの第２原点を有効にする。'V5.0.0.6②

    Public giTeachpointUse As Short = 0                     ' =0:KOA-EWタイプ(ティーチポイント編集表示タイプ)、=1:（標準）ティーチポイント非表示（ティーチポイントエリアをワークで使用)
    Public giFixAttOnly As Short = 0                        ' ファイバーレーザで固定アッテネータ使用する、しない      V6.0.1.021　
    Public gdblNgStopRate As Double = 0                     ' 基板１枚のNG率の設定値      'V4.5.0.5① 'V4.12.2.0⑨　'V6.0.5.0④
    '----- V6.1.1.0①↓(SL432R時のオプション) -----
    Public giBuzerOn As Short = 0                           ' トリミング終了時のブザー鳴動の有無(0=鳴らさない, 1=鳴らす)　
    '----- V6.1.1.0①↑ -----
    Public giNgCountAss As Short = 0                        ' NG判定基準を%からNG数とする(0=%(標準),1=NG数) V6.1.2.0②
    Public giAlmTimeDsp As Short = 0                        ' ローダアラーム時の時間表示の有無(0=表示なし, 1=表示あり) V6.1.1.0⑬

    Public giAlarmOnOff As Short = 0                        ' アラーム音ON/OFFボタン表示の有/無(1/0) SL436R時有効 V6.1.1.0④
    Public giDispEndTime As Short = 0                       ' 自動運転終了時の時間(表示しない(0)/する(1)) SL436R時有効 V6.1.1.0③

    '----- ONLINE -----
    Public Const TYPE_OFFLINE As Short = 0                  ' OFFLINE
    Public Const TYPE_ONLINE As Short = 1                   ' ONLINE
    Public Const TYPE_MANUAL As Short = 2                   ' SLIDE COVER+XY移動処理

    '----- ProbeType -----
    Public Const TYPE_PROBE_NON As Short = 0                ' NON
    Public Const TYPE_PROBE_STD As Short = 1                ' STANDARD

    '----- XY Table Exist Flag -----
    Public Const TYPE_XYTABLE_NON As Short = 0              ' NON
    Public Const TYPE_XYTABLE_X As Short = 1                ' X Only
    Public Const TYPE_XYTABLE_Y As Short = 2                ' Y Only
    Public Const TYPE_XYTABLE_XY As Short = 3               ' XY

    '----- 吸着ﾘﾄﾗｲ処理 -----
    Public Const VACCUME_ERRRETRY_OFF As Short = 0          ' Not retry
    Public Const VACCUME_ERRRETRY_ON As Short = 1           ' Retry
    Public Const RET_VACCUME_RETRY As Short = 1
    Public Const RET_VACCUME_CANCEL As Short = 2

    '----- ｶｽﾀﾏｲｽﾞ -----
    Public Const customROHM As Short = 1                    ' ﾛｰﾑ殿向け仕様
    Public Const customASAHI As Short = 2                   ' 朝日電子殿向け仕様
    Public Const customSUSUMU As Short = 3                  ' 進殿向け仕様
    Public Const customKOA As Short = 4                     ' KOA(匠の里)殿向け仕様
    Public Const customKOAEW As Short = 5                   ' KOA(EW)殿向け仕様
    Public Const customNORI As Short = 6                    ' ノリタケ殿向け仕様
    Public Const customTAIYO As Short = 7                   ' 太陽社殿向け仕様　　V1.23.0.0①
    Public Const customWALSIN As Short = 8                  ' WALSIN殿向け仕様　　V4.11.0.0②

    '----- パワーメータのデータ取得取得 -----
    Public Const PM_DTTYPE_NONE As Short = 0                ' なし
    Public Const PM_DTTYPE_IO As Short = 1                  ' Ｉ／Ｏ読取り
    Public Const PM_DTTYPE_USB As Short = 2                 ' ＵＳＢ

    '----- V1.22.0.0④↓ -----
    '----- サマリログ出力 -----
    Public Const SUMMARY_NONE As Short = 0                  ' 出力しない
    Public Const SUMMARY_OUT As Short = 1                   ' 出力する(シナジー)
    '----- V1.22.0.0④↑ -----

    '----- V2.0.0.0③↓ -----
    ' 装置タイプ(giMachineKd)
    Public Const MACHINE_KD_R As Short = 0                  ' SL43xR
    Public Const MACHINE_KD_RW As Short = 1                 ' SL432RW
    Public Const MACHINE_KD_RS As Short = 2                 ' SL43xRS
    '----- V8.0.0.16②↑ -----
    ' 装置タイプ(gMachineType) V4.10.0.0⑩↓
    Public Const MACHINE_TYPE_432R As Short = 1             ' SL432R
    Public Const MACHINE_TYPE_436R As Short = 2             ' SL436R
    Public Const MACHINE_TYPE_432RW As Short = 3            ' SL432RW
    Public Const MACHINE_TYPE_436S As Short = 4             ' SL436S
    'V4.10.0.0⑩↑
    'V5.0.0.6①    Public Const TRIM_MODE_STEP_AND_REPEAT As Integer = 6   'V4.1.0.0②
    '---------------------------------------------------------------------------
    '   ステージ動作関係
    '---------------------------------------------------------------------------
    ' ステップ方向
    Public Const STEP_RPT_NON As Short = 0      ' ステップ＆リピート方向（なし）
    Public Const STEP_RPT_X As Short = 1        ' ステップ＆リピート方向（X方向）
    Public Const STEP_RPT_Y As Short = 2        ' ステップ＆リピート方向（Y方向）
    Public Const STEP_RPT_CHIPXSTPY As Short = 3 ' ステップ＆リピート方向（X方向チップ幅ステップ＋Y方向）チップ幅＋Ｘ方向
    Public Const STEP_RPT_CHIPYSTPX As Short = 4 ' ステップ＆リピート方向（Y方向チップ幅ステップ＋X方向）チップ幅＋Ｙ方向

    ' BP基準方向
    Public Const BP_DIR_RIGHTUP As Short = 0    ' BP基準右上（プラス方向）←↓　　　 1 ＿ ＿ 0
    Public Const BP_DIR_LEFTUP As Short = 1     ' BP基準左上（プラス方向）↓→　　　　|＿|＿|
    Public Const BP_DIR_RIGHTDOWN As Short = 2  ' BP基準右下（プラス方向）←↑        |＿|＿|
    Public Const BP_DIR_LEFTDOWN As Short = 3   ' BP基準左下（プラス方向）↑→　　　 3　　　 2

    Public Const BP_DIR_RIGHT As Short = 0      ' BP-X方向基準右
    Public Const BP_DIR_LEFT As Short = 1       ' BP-X方向基準左

    Public Const BLOCK_END As Short = 1         ' ブロック終了 
    Public Const PLATE_BLOCK_END As Short = 2   ' プレート・ブロック終了

    '----- V4.0.0.0-35↓ -----
    ' SL436S時のZ位置のデフォルト値
    Public Const Z_ON_POS_SIMPLE As Double = 1.0
    Public Const Z_STEP_POS_SIMPLE As Double = 1.0
    Public Const Z_OFF_POS_SIMPLE As Double = 0.0
    '----- V4.0.0.0-35↑ -----

    '----- V1.24.0.0①↓ -----
    ' Walsin殿特注Fシータ対応
    Public Const BPSIZE_6060 As Integer = 6060                          ' 60*60(20)
    Public Const BSZ_6060_OFSX As Double = 30.0#
    Public Const BSZ_6060_OFSY As Double = 10.0#
    '----- V1.24.0.0①↑ -----

    '----- V1.24.0.0①↓ -----
    ' KOA-EW殿特注対応
    Public Const BPSIZE_6030 As Integer = 6030                          ' 60*60(30)

    '----- V4.4.0.0①↓ -----
    ' 加工エリア□90mm対応
    Public Const BPSIZE_90 As Integer = 90                          ' Area 90mm
    '----- V4.4.0.0①↑ -----

    '----- その他 -----
    ' FLSET関数のモード
    Public Const FLMD_CNDSET As Integer = 0                 ' 加工条件設定
    Public Const FLMD_BIAS_ON As Integer = 1                ' BIAS ON
    Public Const FLMD_BIAS_OFF As Integer = 2               ' BIAS OFF(LaserOff関数内でBIAS OFFはしている)

    'V1.13.0.0④ 
    ' Z2動作用に追加 
    Public Const Z2OFF As Integer = 0
    Public Const Z2ON As Integer = 1
    Public Const Z2STEP As Integer = 2
    ''V1.13.0.0④

    '---------------------------------------------------------------------------
    '   制御フラグ
    '---------------------------------------------------------------------------
    Public gCmpTrimDataFlg As Short                         ' データ更新フラグ(0=更新なし, 1=更新あり)
    Public giTrimErr As Short                               ' ﾄﾘﾏｰ ｴﾗｰ ﾌﾗｸﾞ ※ｴﾗｰ時はｸﾗﾝﾌﾟｸﾗﾝﾌﾟOFF時ﾄﾘﾏ動作中OFFをﾛｰﾀﾞｰに送信しない
    '                                                       ' B0 : 吸着ｴﾗｰ(EXIT)
    '                                                       ' B1 : その他ｴﾗｰ
    '                                                       ' B2 : 集塵機ｱﾗｰﾑ検出
    '                                                       ' B3 : 軸ﾘﾐｯﾄ､軸ｴﾗｰ､軸ﾀｲﾑｱｳﾄ
    '                                                       ' B4 : 非常停止
    '                                                       ' B5 : ｴｱｰ圧ｴﾗｰ

    Public gLoadDTFlag As Boolean                            ' ﾃﾞｰﾀﾛｰﾄﾞ済ﾌﾗｸﾞ(False:ﾃﾞｰﾀ未ﾛｰﾄﾞ, True:ﾃﾞｰﾀﾛｰﾄﾞ済)
    Public gbInitialized As Boolean                         ' True=原点復帰済, False=原点復帰未
    'Public bFgfrmDistribution As Boolean                    ' 生産ｸﾞﾗﾌ表示ﾌﾗｸﾞ(TRUE:表示 FALSE:非表示)
    Public gLoggingHeader As Boolean                        ' ﾛｸﾞﾍｯﾀﾞｰ書込み指示ﾌﾗｸﾞ(TRUE:出力)
    Public gESLog_flg As Boolean                            ' ESログフラグ(Flase=ログOFF, True=ログON)
    '' '' ''Public giAdjKeybord As Short                             ' トリミング中ADJ機能キーボード矢印(0:入力なし 1:上 2:下 3:右 4:左 )
    Public gPrevInterlockSw As Short

    Public gbCanceled As Boolean ' ←　各画面処理でPrivateで持つ 

    Public giSubExistMsgFlag As Boolean                      ' 基板有無を返すときにメッセージを表示しない 'V4.11.0.0⑧
    Public gDataUpdateFlag As Boolean = False               'V6.1.4.5① カットオフ値、ＥＳポイント値入力画面でデータ更新した時Trueにする。

    '-------------------------------------------------------------------------------
    '   オブジェクト定義
    '-------------------------------------------------------------------------------
    '----- VB6のOCX -----
    'Public ObjSys As Object                                 ' OcxSystem.ocx
    'Public ObjUtl As Object                                 ' OcxUtility.ocx
    'Public ObjHlp As Object                                 ' OcxAbout.ocx
    'Public ObjPas As Object                                 ' OcxPassword.ocx
    'Public ObjMTC As Object                                 ' OcxManualTeach.ocx
    'Public ObjTch As Object                                 ' Teach.ocx
    'Public ObjPrb As Object                                 ' Probe.ocx
    'Public ObjVdo As Object                                 ' Video.ocx
    'Public ObjPrt As Object                                ' OcxPrint.ocx
    Public ObjMON(32) As Object
    Public gparModules As MainModules                                   ' 親側メソッド呼出しオブジェクト(OcxSystem用) '###061
    Public ObjCrossLine As New TrimClassLibrary.TrimCrossLineClass()    ' 補正クロスライン表示用オブジェクト ###232 
    Public TrimClassCommon As New TrimClassLibrary.Common()             ' 共通関数
    Public commandtutorial As New TrimClassLibrary.CommandTutorial()    ' V2.0.0.0⑩コマンド実施状態管理クラス 
    Public frmAutoObj As FormDataSelect2                                ' V6.1.4.0⑩ 自動運転フォームオブジェクト(ロット切替え機能用)
    Public ObjQRCodeReader As New QRCodeReader()                        ' V6.1.4.0_22
    Public Property FormMain As Form1                       ' ﾃﾞﾌｫﾙﾄｲﾝｽﾀﾝｽを使用しないように    V6.1.4.0②

    '---------------------------------------------------------------------------
    ' トリミング動作モード
    '---------------------------------------------------------------------------
    Public Const TRIM_MODE_ITTRFT As Integer = 0    'イニシャルテスト＋トリミング＋ファイナルテスト実行
    Public Const TRIM_MODE_TRFT As Integer = 1      'トリミング＋ファイナルテスト実行
    Public Const TRIM_MODE_FT As Integer = 2        'ファイナルテスト実行（判定）
    Public Const TRIM_MODE_MEAS As Integer = 3      '測定実行
    Public Const TRIM_MODE_POSCHK As Integer = 4    'ポジションチェック
    Public Const TRIM_MODE_CUT As Integer = 5       'カット実行
    Public Const TRIM_MODE_STPRPT As Integer = 6    'ステップ＆リピート実行
    Public Const TRIM_MODE_TRIMCUT As Integer = 7   'トリミングモードでのカット実行


    '-------------------------------------------------------------------------------
    ' トリミング結果
    '-------------------------------------------------------------------------------
    '----- トリミング結果値（INTRIMで設定）
    '//Trim result
    '//0:未実施   1:OK       2:ITNG      3:FTNG     4:SKIP
    '//5:RATIO    6:ITHI NG  7:ITLO NG   8:FTHI NG  9:FTLO NG
    '//10:        11:        12:         13:        14:
    '//15:異形面付けによりSKIP
    Public Const RSLT_NO_JUDGE As Integer = 0
    Public Const RSLT_OK As Integer = 1
    Public Const RSLT_IT_NG As Integer = 2
    Public Const RSLT_FT_NG As Integer = 3
    Public Const RSLT_SKIP As Integer = 4
    Public Const RSLT_RATIO As Integer = 5
    Public Const RSLT_IT_HING As Integer = 6
    Public Const RSLT_IT_LONG As Integer = 7
    Public Const RSLT_FT_HING As Integer = 8
    Public Const RSLT_FT_LONG As Integer = 9
    Public Const RSLT_RANGEOVER As Integer = 10
    Public Const RSLT_OPENCHK_NG As Integer = 20
    Public Const RSLT_SHORTCHK_NG As Integer = 21
    Public Const RSLT_IKEI_SKIP As Integer = 15

    '----- 生産管理グラフフォームオブジェクト
    Public gObjFrmDistribute As Object                      ' frmDistribute

    '----- 生産管理情報用配列 -----
    Public Const MAX_FRAM1_ARY As Integer = 15              ' ラベル配列数
    '                                                       ' 生産管理情報のラベル配列のインデックス 
    Public Const FRAM1_ARY_GO As Integer = 0                ' GO数(サーキット数 or 抵抗数)
    Public Const FRAM1_ARY_NG As Integer = 1                ' NG数(サーキット数 or 抵抗数)
    Public Const FRAM1_ARY_NGPER As Integer = 2             ' NG%
    Public Const FRAM1_ARY_PLTNUM As Integer = 3            ' PLATE数
    Public Const FRAM1_ARY_REGNUM As Integer = 4            ' RESISTOR数
    Public Const FRAM1_ARY_ITHING As Integer = 5            ' IT HI NG数
    Public Const FRAM1_ARY_FTHING As Integer = 6            ' FT HI NG数
    Public Const FRAM1_ARY_ITLONG As Integer = 7            ' IT LO NG数
    Public Const FRAM1_ARY_FTLONG As Integer = 8            ' FT LO NG数
    Public Const FRAM1_ARY_OVER As Integer = 9              ' OVER数
    Public Const FRAM1_ARY_ITHINGP As Integer = 10          ' IT HI NG%
    Public Const FRAM1_ARY_FTHINGP As Integer = 11          ' FT HI NG%
    Public Const FRAM1_ARY_ITLONGP As Integer = 12          ' IT LO NG%
    Public Const FRAM1_ARY_FTLONGP As Integer = 13          ' FT LO NG%
    Public Const FRAM1_ARY_OVERP As Integer = 14            ' OVER NG%

    Public Fram1LblAry(MAX_FRAM1_ARY) As System.Windows.Forms.Label     ' 生産管理情報のラベル配列

    '-------------------------------------------------------------------------------
    '   gMode(OcxSystemのfrmReset()の処理モード)
    '-------------------------------------------------------------------------------
    Public Const cGMODE_ORG As Short = 0                    '  0 : 原点復帰
    Public Const cGMODE_ORG_MOVE As Short = 1               '  1 : 原点位置移動
    Public Const cGMODE_START_RESET As Short = 2            '  2 : 操作確認画面(START/RESET待ち)
    '                                                       '  3 :
    '                                                       '  4 :
    Public Const cGMODE_EMG As Short = 5                    '  5 : 非常停止メッセージ表示
    '                                                       '  6 :
    Public Const cGMODE_SCVR_OPN As Short = 7               '  7 : トリミング中のスライドカバー開メッセージ表示
    Public Const cGMODE_CVR_OPN As Short = 8                '  8 : トリミング中の筐体カバー開メッセージ表示
    Public Const cGMODE_SCVRMSG As Short = 9                '  9 : スライドカバー開メッセージ表示(トリミング中以外)
    Public Const cGMODE_CVRMSG As Short = 10                ' 10 : 筐体カバー開確認メッセージ表示(トリミング中以外)
    Public Const cGMODE_ERR_HW As Short = 11                ' 11 : ハードウェアエラー(カバーが閉じてます)メッセージ表示
    Public Const cGMODE_ERR_HW2 As Short = 12               ' 12 : ハードウェアエラーメッセージ表示
    Public Const cGMODE_CVR_LATCH As Short = 13             ' 13 : カバー開ラッチメッセージ表示
    Public Const cGMODE_CVR_CLOSEWAIT As Short = 14         ' 14 : 筐体カバークローズもしくはインターロック解除待ち
    Public Const cGMODE_ERR_DUST As Short = 20              ' 20 : 集塵機異常検出メッセージ表示
    Public Const cGMODE_ERR_AIR As Short = 21               ' 21 : エアー圧エラー検出メッセージ表示

    Public Const cGMODE_ERR_HING As Short = 40              ' 40 : 連続HI-NGｴﾗｰ(ADVｷｰ押下待ち)
    Public Const cGMODE_SWAP As Short = 41                  ' 41 : 基板交換(STARTｷｰ押下待ち)
    Public Const cGMODE_XYMOVE As Short = 42                ' 42 : 終了時のﾃｰﾌﾞﾙ移動確認(STARTｷｰ押下待ち)
    Public Const cGMODE_ERR_REPROBE As Short = 43           ' 43 : 再プロービング失敗(STARTｷｰ押下待ち) SL436R用
    Public Const cGMODE_LDR_ALARM As Short = 44             ' 44 : ローダアラーム発生   SL436R用
    Public Const cGMODE_LDR_START As Short = 45             ' 45 : 自動運転開始(STARTｷｰ押下待ち)   SL436R用
    Public Const cGMODE_LDR_TMOUT As Short = 46             ' 46 : ローダ通信タイムアウト  SL436R用
    Public Const cGMODE_LDR_END As Short = 47               ' 47 : 自動運転終了(STARTｷｰ押下待ち)   SL436R用
    Public Const cGMODE_LDR_ORG As Short = 48               ' 48 : ローダ原点復帰  SL436R用
    Public Const cGMODE_ERR_CUTOFF_TURNING As Short = 49    ' 49 : 自動カットオフ調整失敗時

    Public Const cGMODE_AUTO_LASER As Short = 50            ' 50 : 自動レーザパワー調整
    Public Const cGMODE_QUEST_NEW_CONTINUE As Short = 51    ' 51 : 自動運転開始時の問い合わせ  ' V6.0.3.0_27

    Public Const cGMODE_LDR_CHK As Short = 60               ' 60 : ローダ状態チェック(起動時ﾛｰﾀﾞ自動ﾓｰﾄﾞ/動作中)
    Public Const cGMODE_LDR_ERR As Short = 61               ' 61 : ローダ状態エラー(ﾛｰﾀﾞ自動でﾛｰﾀﾞ無)
    Public Const cGMODE_LDR_MNL As Short = 62               ' 62 : カバー開後のローダ手動モード処理
    Public Const cGMODE_LDR_WKREMOVE As Short = 63          ' 63 : 残基板取り除きメッセージ  SL436R用
    Public Const cGMODE_LDR_RSTAUTO As Short = 64           ' 64 : 自動運転中止メッセージ  SL436R用 ###124
    Public Const cGMODE_LDR_WKREMOVE2 As Short = 65         ' 65 : 残基板取り除きメッセージ(APP終了)  SL436R用 ###175
    Public Const cGMODE_LDR_STAGE_ORG As Short = 66         ' 66 : ステージ原点移動 SL436R用 ###188
    Public Const cGMODE_LDR_MAGAGINE_EXCHG As Short = 67    ' 67 : マガジン交換メッセージ SL436R用 V1.18.0.0⑨
    Public Const cGMODE_LDR_CHK_AUTO As Short = 67          ' 67 : ローダ状態チェック(自動運転時),ローダが自動に切り替わるまで待つ SL432R用 V6.1.4.0①
    '                                                              ※DllSystemと番号がガッチンコしている←本来ならDllSystemのgModeに合わせるベキ                 
    Public Const cGMODE_ERR_RATE_NG As Short = 68           ' 68 : High,Lowの率が悪くなった場合の停止画面用   'V4.9.0.0①
    Public Const cGMODE_ERR_TOTAL_CLEAR As Short = 69       ' 69 : 集計のトータルをクリアするかの確認         'V4.9.0.0①

    Public Const cGMODE_OPT_START As Short = 70             ' 70 : ﾄﾘﾐﾝｸﾞ開始時のｽﾀｰﾄSW押下待ち
    Public Const cGMODE_OPT_END As Short = 71               ' 71 : ﾄﾘﾐﾝｸﾞ終了時のｽﾗｲﾄﾞｶﾊﾞｰ開待ち

    Public Const cGMODE_MSG_DSP As Short = 90               ' 90 : 指定メッセージ表示(STARTキー押下待ち)

    Public Const cGMODE_STAGE_HOMEMOVE As Short = 91        ' 91 :ステージHome位置移動      'V4.0.0.0-83
    Public Const cGMODE_MSG_DSP3 As Short = 92              ' 92 : 指定メッセージ表示(START,RESETｷｰ押下待ち)　'V6.1.1.0①

    ' リミットセンサー& 軸エラー & タイムアウトメッセージ
    ' ※cGMODE_TO_AXISX As Short = 101以降は TrimErrNo.vbに移動

    '---------------------------------------------------------------------------
    '   補正クロスライン表示用パラメータ
    '---------------------------------------------------------------------------
    'Public gstCLC As CLC_PARAM                              ' 補正クロスライン表示用パラメータ V4.0.0.0-21

    '---------------------------------------------------------------------------
    '   ファイルパス関係
    '---------------------------------------------------------------------------
    Public gStrTrimFileName As String                       ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名

    ''''    lib.bas　でしか使用されていない。
    Public gsDataLogPath As String

    Public gbCutPosTeach As Boolean                         ' CutPosTeach(表示中:True, 非表示:False)

    '---------------------------------------------------------------------------
    '   変数定義
    '---------------------------------------------------------------------------

    '----- パターン認識用 -----
    Public giTempGrpNo As Integer                           ' テンプレートグループ番号(1～999)
    Public giTempNo As Integer                              ' テンプレート番号

    '----- カット位置補正用構造体 -----
    Public Structure CutPosCorrect_Info                     ' パターン登録情報
        Dim intFLG As Short                                 ' カット位置補正フラグ(0:しない, 1:する)
        Dim intGRP As Short                                 ' パターンｸﾞﾙｰﾌﾟ番号(1-999)
        Dim intPTN As Short                                 ' パターン番号(1-50)
        Dim dblPosX As Double                               ' パターン位置X(補正位置ティーチング用)
        Dim dblPosY As Double                               ' パターン位置Y(補正位置ティーチング用)
        Dim intDisp As Short                                ' パターン認識時の検索枠表示(0:なし, 1:あり)
    End Structure

    Public Const MaxRegNum As Short = 256                   ' 抵抗数の最大値
    Public Const MaxCutNum As Short = 30                    ' カットの最大値
    Public Const MaxDataNum As Short = 7681                 ' 抵抗数*カットの最大数+1
    Public stCutPos(MaxRegNum + 1) As CutPosCorrect_Info        ' パターン登録情報

    Public giCutPosRNum As Short                            ' カット位置補正する抵抗数
    'Public giCutPosRSLT(MaxRegNum) As Short                 ' パターン認識結果(0:補正なし, 1:OK, 2:NGｽｷｯﾌﾟ)
    'Public gfCutPosDRX(MaxRegNum) As Double                 ' ズレ量X
    'Public gfCutPosDRY(MaxRegNum) As Double                 ' ズレ量Y
    Public gfCutPosCoef(MaxRegNum) As Double                '  一致度

    '----- θ補正用 -----
    Public gfCorrectPosX As Double                          ' θ補正時のXYﾃｰﾌﾞﾙずれ量X(mm) ※ThetaCorrection()で設定
    Public gfCorrectPosY As Double                          ' θ補正時のXYﾃｰﾌﾞﾙずれ量Y(mm)
    Public gdCorrectTheta As Double                         ' θ補正時の補正角度                     'V5.0.0.9⑨
    Public gbInPattern As Boolean                           ' 位置補正処理中
    Public gbRotCorrectCancel As Short                      ' 0:OK, n < 0: 位置補正をキャンセルした or 位置補正エラー
    Public gbCorrectDone As Boolean = False                 'V4.10.0.0③ オペレータティーチング簡素化 一度ＸＹΘ補正を行ったら後は実施しない為に使用
    Public gbIntegratedMode As Boolean = False              'V4.10.0.0③ 現在一括ティーチングモードの時 True
    Public gbSubstExistChkDone As Boolean = False           'V4.10.0.0③ オペレータティーチング簡素化 一度基板在荷チェックを行ったら後は実施しない為に使用
    '----- V1.13.0.0③↓ -----
    '----- オートプローブ用 -----
    Public gfStgOfsX As Double = 0.0                        ' XYテーブルオフセットX(mm) ※オートプローブ実行コマンド(FrmMatrix())で設定
    Public gfStgOfsY As Double = 0.0                        ' XYテーブルオフセットY(mm)
    '----- V1.13.0.0③↑ -----

    '----- デジタルＳＷ -----
    'Public gDigH As Short                                   ' デジタルＳＷ(Hight)
    'Public gDigL As Short                                   ' デジタルＳＷ(Low)
    'Public gDigSW As Short                                  ' デジタルＳＷ
    Public gPrevTrimMode As Short                           ' デジタルＳＷ値退避域

    '----- GPIB用 -----
    Public giGpibDefAdder As Short = 21                     ' 初期設定(機器ｱﾄﾞﾚｽ)

    '----- その他 -----
    Public giIX2LOG As Short = 0                            ' IX2ログ(0=無効, 1=有効)　###231
    Public giPrint As Short = 0                             ' PRINTボタン(0=無効, 1=有効) V1.18.0.0③
    Public giTablePosUpd As Short = 0                       ' テーブル1,2座標を更新する/しない(VIDEO.OCX用オプション)　###234
    Public giPassWord_Lock As Integer = 0                   ' V1.14.0.0①

    ' ↓↓↓ V3.1.0.0② 2014/12/01
    Public giMeasurement As Short = 0                       ' 測定方法（0=今まで通り、1=イニシャルテスト、2=ファイナルテスト）
    Public gdRESISTOR_MIN As Double                         ' 抵抗最小値
    Public gdRESISTOR_MAX As Double                         ' 抵抗最大値
    ' ↑↑↑ V3.1.0.0② 2014/12/01

    'V4.6.0.0①
    Public giManualSeq As Integer                           ' 手動実行時のクランプ吸着シーケンスをコマンド実行に合わせる。
    'V4.6.0.0①
    'V4.6.0.0②
    Public giTimeLotOnly As Integer                         ' Lot中のみ生産情報の時間カウントを行う
    'V4.6.0.0②

    Public giRateDisp As Integer                                            'V4.8.0.1① NG率の表示か歩留まり表示かの切替え
    Public LOT_COND_FILENAME As String = "C:\TRIM\LotStopCondition.ini"     'V4.8.0.1①
    Public giChangePoint As Integer                         'V4.9.0.0② 切替ポイント、ターンポイント切替機能有効、無効設定
    Public giNgStop As Integer                               ''V4.9.0.0①
    Public giClampSeq As Integer                                            '
    Public giClampLessStage As Integer                      ' クランプレス載物台(0でない場合クランプレス)       'V5.0.0.9③
    Public gdClampLessOffsetX As Double                     ' クランプレス自動搬送時搭載位置基板オフセットX     'V5.0.0.9⑥
    Public gdClampLessOffsetY As Double                     ' クランプレス自動搬送時搭載位置基板オフセットY     'V5.0.0.9⑥
    Public giClampLessRoughCount As Integer                 ' クランプレスラフアライメント回数                 'V5.0.0.9⑥
    Public gdClampLessTheta As Double                       ' クランプレス載物台の自動搬送時基板搭載θ角度    'V5.0.0.9⑨
    Public giClampLessOutPos As Integer                     ' クランプレス時の排出位置指定    'V6.1.2.0④


    ''''    複数個所でFalseに設定しているが、Trueに設定されることはない。
    ''''    フラグとして機能はしていないので、コード確認の上削除。
    'Public OKFlag As Boolean                    'OKボタン押下の有無

    ''''    初期化のみ
    'Public gRegisterExceptMarkingCnt As Short '抵抗数（マーキングを除く数) @@@007
    'Public gsSystemPassword As String
    'Public gLoggingEnd As Boolean

    ' '' '' ''----- 生産管理情報 -----
    '' '' ''Public glCircuitNgTotal As Integer                      ' 不良サーキット数
    '' '' ''Public glCircuitGoodTotal As Integer                    ' 良品サーキット数
    '' '' ''Public glPlateCount As Integer                          ' プレート処理数
    '' '' ''Public glGoodCount As Integer                           ' 良品抵抗数
    '' '' ''Public glNgCount As Integer                             ' 不良抵抗数
    '' '' ''Public glITHINGCount As Integer                         ' IT HI NG数
    '' '' ''Public glITLONGCount As Integer                         ' IT LO NG数
    '' '' ''Public glFTHINGCount As Integer                         ' FT HI NG数
    '' '' ''Public glFTLONGCount As Integer                         ' FT LO NG数
    '' '' ''Public glITOVERCount As Integer                         ' ITｵｰﾊﾞｰﾚﾝｼﾞ数


    Public gfPreviousPrbBpX As Double                       ' BP論理座標上の位置X (BSIZE+BPOFFSET相対)
    Public gfPreviousPrbBpY As Double                       '                   Y

    ''''------------------------------------------------

    ''''---------------------------------------------------
    ''''　090413 minato
    ''''    ProbeTeachで設定し、ResistorGraphで使用しているのみ。
    ''''    内部で出来るように見直す。
    '---------------------------------------------------------------------------
    '   全抵抗測定のグラフ表示用
    '---------------------------------------------------------------------------
    Public giMeasureResistors As Short                      ' 抵抗数
    Public giMeasureResiNum(512) As Double                  ' 抵抗番号
    Public gfMeasureResiOhm(512) As Double                  ' 測定した抵抗値
    Public gfResistorTarget(512) As Double                  ' 目標値
    Public gfMeasureResiPos(2, 512) As Double               ' カットスタートポイント
    Public giMeasureResiRst(512) As Short                   ' トリミング結果

    Public Const cMEASUREcOK As Short = 1                   ' OK
    Public Const cMEASUREcIT As Short = 2                   ' IT ERROR
    Public Const cMEASUREcFT As Short = 3                   ' FT ERROR
    Public Const cMEASUREcNA As Short = 4                   ' 未測定


    '===============================================================================
    Public ExitFlag As Short
    Public gMode As Short 'モード

    'INIファイル取得データ
    ''''(2010/11/16) 動作確認後下記コメントは削除
    'Public gStartX As Double 'プローブ初期値X
    'Public gStartY As Double 'プローブ初期値Y

    ' レーザー調整
    ''''    frmReset、LASER_teaching　で使用
    Public gfLaserContXpos As Double
    Public gfLaserContYpos As Double

    '画像ハンドル
    'Public mlHSKDib As Integer '白黒
    '表示位置
    'Public mtDest As RECT
    'Public mtSrc As RECT
    'Public gVideoStarted As Boolean

    ''----- ｱﾌﾟﾘﾓｰﾄﾞ ----- (注)OcxSystem定義と一致させる必要有り
    'Public giAppMode As Short

    ''データ編集パスワード関連
    'Public gbPassSucceeded As Boolean

    'Public gLoggingHeader As Boolean                    ' ﾍｯﾀﾞｰ書込み指示ﾌﾗｸﾞ(TRUE:出力)
    'Public gbLogHeaderWrite As Boolean ' ログのヘッダ出力フラグ @@@082

    'Public giOpLogFileHandle As Short ' 操作ログファイルのハンドル
    'Public gwTrimmerStatus As Short ' ホスト通信ステータス保持

    '''' ロギングフラグ　09/09/09  SysParamから移行


    Public Const KUGIRI_CHAR As Short = &H9S ' TAB

    'Public gbInPattern As Boolean ' 位置補正処理中
    'Public gbRotCorrectCancel As Short ' 0:OK, n < 0: 位置補正をキャンセルした or 位置補正エラー
    ''Public gfCorrectPosX As Double                          ' トリムポジション補正値X 
    'Public gfCorrectPosY As Double                          ' トリムポジション補正値Y
    'Public gbPreviousPrbPos As Boolean ' プローブ位置合わせのBP/STAGE座標を記憶している
    'Public gsCutTypeName(256) As String ' カットタイプ名テーブル
    'Public gtimerCoverTimeUp As Boolean

    ''BPリニアリティー補正値
    'Public Const cMAXcBPcLINEARITYcNUM As Short = 21


    ''''2009/05/29 minato
    ''''    LoaderAlarm.bas削除により一時移動
    ''''===============================================
    '' ''Public iLoaderAlarmKind As Short ' ｱﾗｰﾑ種類(1:全停止異常 2:ｻｲｸﾙ停止 3:軽故障 0:ｱﾗｰﾑ無し)
    '' ''Public iLoaderAlarmNum As Short ' 発生中のｱﾗｰﾑ数
    '' ''Public strLoaderAlarm() As String ' ｱﾗｰﾑ文字列
    '' ''Public strLoaderAlarmInfo() As String ' ｱﾗｰﾑ情報1
    '' ''Public strLoaderAlarmExec() As String ' ｱﾗｰﾑ情報2(対策)
    ''''===============================================

    'Public gbInitialized As Boolean

    '----- 分布図用 -----
    Public Const MAX_SCALE_NUM As Integer = 999999999           ' ｸﾞﾗﾌ最大値
    Public Const MAX_SCALE_RNUM As Integer = 12                 ' ｸﾞﾗﾌ表示抵抗数

    Public gDistRegNumLblAry(12) As System.Windows.Forms.Label     ' 分布グラフ抵抗数配列
    Public gDistGrpPerLblAry(12) As System.Windows.Forms.Label     ' 分布グラフ%配列
    Public gDistShpGrpLblAry(12) As System.Windows.Forms.Label     ' 分布グラフ配列

    'V4.0.0.0⑫↓
    Public gGoodChip As System.Windows.Forms.Label                  ' 分布グラフGoodChip数
    Public gNgChip As System.Windows.Forms.Label                    ' 分布グラフNGChip数
    Public gMinValue As System.Windows.Forms.Label                  ' 分布グラフ最小値
    Public gMaxValue As System.Windows.Forms.Label                  ' 分布グラフ最大値
    Public gAverageValue As System.Windows.Forms.Label              ' 分布グラフ平均値
    Public gDeviationValue As System.Windows.Forms.Label            ' 分布グラフ標準偏差
    Public gGraphAccumulationTitle As System.Windows.Forms.Label    ' 分布グラフタイトル
    Public gRegistUnit As System.Windows.Forms.Label                ' 分布グラフ抵抗単位

    'Public gSimpleDistRegNumLblAry(12) As System.Windows.Forms.Label     ' 分布グラフ抵抗数配列
    'Public gSimpleDistGrpPerLblAry(12) As System.Windows.Forms.Label     ' 分布グラフ%配列
    'Public gSimpleDistShpGrpLblAry(12) As System.Windows.Forms.Label     ' 分布グラフ配列
    'V4.0.0.0⑫↑

    Public glRegistNum(12) As Integer                            ' 分布グラフ抵抗数
    Public glRegistNumIT(12) As Integer                          ' 分布グラフ抵抗数 ｲﾆｼｬﾙﾃｽﾄ
    Public glRegistNumFT(12) As Integer                          ' 分布グラフ抵抗数 ﾌｧｲﾅﾙﾃｽﾄ

    Public lOkChip As Integer                                   ' OK数
    Public lNgChip As Integer                                   ' NG数
    Public dblMinIT As Double                                   ' 最小値ｲﾆｼｬﾙ
    Public dblMaxIT As Double                                   ' 最大値ｲﾆｼｬﾙ
    Public dblMinFT As Double                                   ' 最小値ﾌｧｲﾅﾙ
    Public dblMaxFT As Double                                   ' 最大値ﾌｧｲﾅﾙ
    '' '' ''Public dblGapIT As Double                                   ' 積算誤差ｲﾆｼｬﾙ
    '' '' ''Public dblGapFT As Double                                   ' 積算誤差ﾌｧｲﾅﾙ

    Public dblAverage As Double                                 ' 平均値
    Public dblDeviationIT As Double                             ' 標準偏差(IT)
    Public dblDeviationFT As Double                             ' 標準偏差(FT)

    Public dblAverageIT As Double                               ' IT平均値
    Public dblAverageFT As Double                               ' FT平均値
    Public HEIHOUIT As Double                                   ' 平方偏差
    Public HEIHOUFT As Double                                   ' 平方偏差

    '    Public Const SIMPLE_PICTURE_SIZEX As Long = 6400 '6400            ' Video画面サイズX(シンプルトリマ時)
    '    Public Const SIMPLE_PICTURE_SIZEY As Long = 6400 '6400            ' Video画面サイズY(シンプルトリマ時)
    Public Const SIMPLE_PICTURE_SIZEX As Long = 426 '6400            ' Video画面サイズX(シンプルトリマ時)
    Public Const SIMPLE_PICTURE_SIZEY As Long = 426 '6400            ' Video画面サイズY(シンプルトリマ時)

    'Public Const NORMAL_PICTURE_SIZEX As Long = 9600            ' Video画面サイズX(ティーチング、ノーマル時)
    'Public Const NORMAL_PICTURE_SIZEY As Long = 7200            ' Video画面サイズY(ティーチング、ノーマル時)
    Public Const NORMAL_PICTURE_SIZEX As Long = 640            ' Video画面サイズX(ティーチング、ノーマル時)
    Public Const NORMAL_PICTURE_SIZEY As Long = 480            ' Video画面サイズY(ティーチング、ノーマル時)

    'V4.0.0.0⑩
    Public Const NORMAL_SIZE As Long = 0                        ' 通常の大きいサイズの画像表示
    Public Const SIMPLE_SIZE As Long = 1                        ' 小さいサイズの画像表示

    Public CROSS_LINEX As Long                                  ' シンプルトリマ画面用クロスライン位置X
    Public CROSS_LINEY As Long                                  ' シンプルトリマ画面用クロスライン位置Y

    Public iGlobalJogMode As Integer                            ' Jog時の動作モード選択
    Public giJogButtonEnable As Integer                         ' Jogボタンの有効無効'V4.0.0.0-78
    Public CleaningPosZ As Double                               ' クリーニング位置Z
    Public gProbeCleaningCounter As Long = 0                    ' プローブクリーニング用カウンタ
    Public gProbeCleaningSpan As Long                           ' プローブクリーニング間隔
    Public Const giClampModeIO As Integer = 0                   ' クランプ制御をIOかメモリか切り替える    'V4.8.0.0①
    Public gfrmAdjustDisp As Integer = 0                        ' FineAdjust画面表示中
    Public giNGCountInPlate As Integer = 0                      ' 1基板内のNG個数     'V6.0.1.0⑮

    '----- V6.0.3.0⑦↓ -----
    Public gAdjustCutoffCount As Long                           ' カットオフ調整ブロック用カウンター 
    Public gAdjustCutoffFunction As Long                        ' カットオフ調整機能有効無効
    Public giCutOffLogOut As Long                               ' カットオフ調整ログ出力有無

    '----- カットオフ調整機能用構造体 -----
    Public Structure CutOffAdjust_Info                          ' カットオフ調整機能用構造体
        Dim dblAdjustCutOff_Exec As Integer                     ' カットオフ調整実行する、しない
        Dim TargetA As Double                                   ' カットオフ調整用目標値
        Dim OrgCutOff As Double                                 ' 開始時のカットオフ値
        Dim dblAdjustCutOff As Double                           ' 調整中カットオフ値
    End Structure
    Public stCutOffAdjust As CutOffAdjust_Info                  ' カットオフ調整機能用構造体

    Public Enum AdjustStat
        ADJUST_DISABLE = 0
        ADJUST_EXEC
        ADJUST_ALREADY
        ADJUST_FINISHED
    End Enum
    Public gVacuumIO As Integer                                 ' V6.0.3.0_37
    '----- V6.0.3.0⑦↑ -----

    Public giReqLotSelect As Integer                            ' マガジンが空でスタートしたときにロット終了か継続かの選択するメッセージを表示するかどうか  V6.0.3.0_38

    ''' <summary>
    ''' ステージ移動時の速度設定 
    ''' </summary>
    Public Enum StageSpeed As Integer
        NormalSpeed = 0             '  0:通常速度
        StepRepeatSpeed             '  1:StepAndRepeat時速度 
    End Enum

#End Region

#Region "グローバル変数の定義"
    '===========================================================================
    '   グローバル変数の定義
    '===========================================================================
    Public gbThetaCorrectionLogOut As Boolean = False               'V4.7.3.2①

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''' 2009/04/13 minato
    ''''    TKYでは使用しているグローバル変数
    ''''    共通化の為、TKY用としては宣言する方向で

    '----- 連続運転用(SL436R用) -----
    Public gbFgAutoOperation As Boolean = False                     ' 自動運転フラグ(True:自動運転中, False:自動運転中でない) 
    Public gsAutoDataFileFullPath() As String                       ' 連続運転登録データファイル名配列
    Public giAutoDataFileNum As Short                               ' 連続運転登録データファイル数
    Public giActMode As Short                                       ' 連続運転動作モード(0:ﾏｶﾞｼﾞﾝﾓｰﾄﾞ 1:ﾛｯﾄﾓｰﾄﾞ 2:ｴﾝﾄﾞﾚｽﾓｰﾄﾞ)
    Public Const MODE_MAGAZINE As Short = 0                         ' マガジンモード
    Public Const MODE_LOT As Short = 1                              ' ロットモード
    Public Const MODE_ENDLESS As Short = 2                          ' エンドレスモード
    '                                                               ' 切替えモード(1=自動モード, 0=手動モード)
    Public Const MODE_MANUAL As Integer = 0                         ' 手動モード
    Public Const MODE_AUTO As Integer = 1                           ' 自動モード
    Public giErrLoader As Short = 0                                 ' ローダアラーム検出(0:未検出 0以外:エラーコード) ###073
    Public gbFgContinue As Boolean = False                          ' 自動運転継続フラグ(True:継続中, False:継続でない) 'V1.18.0.0③(ローム殿特注)

    '                                                               ' 以下はシスパラより設定する
    Public giOPLDTimeOutFlg As Integer                              ' ローダ通信タイムアウト検出(0=検出無し, 1=検出あり)
    Public giOPLDTimeOut As Integer                                 ' ローダ通信タイムアウト時間(msec)
    Public giOPVacFlg As Integer                                    ' 手動モード時の載物台吸着アラーム検出(0=検出無し, 1=検出あり)
    Public giOPVacTimeOut As Integer                                ' 手動モード時の載物台吸着アラームタイムアウト時間(msec)

    Public Const MAXWORK_KND As Integer = 10                        ' プレートデータの基板品種の数
    Public giLoaderSpeed As Integer                                 ' ローダ搬送速度
    Public giLoaderPositionSetting As Integer                       ' ローダ位置設定選択番号(1-10)
    Public gfBordTableOutPosX(0 To MAXWORK_KND - 1) As Double       ' ローダ基板テーブル排出位置X
    Public gfBordTableOutPosY(0 To MAXWORK_KND - 1) As Double       ' ローダ基板テーブル排出位置Y
    Public gfBordTableInPosX(0 To MAXWORK_KND - 1) As Double        ' ローダ基板テーブル供給位置X
    Public gfBordTableInPosY(0 To MAXWORK_KND - 1) As Double        ' ローダ基板テーブル供給位置Y
    '----- V4.0.0.0-26↓ -----
    Public gfTwoSubPickChkPos(0 To MAXWORK_KND - 1) As Double       ' 二枚取りセンサ確認位置座標(mm)
    Public glSubstrateType(0 To MAXWORK_KND - 1) As Long            ' 薄基板対応(0=通常, 1=薄基板(スルーホール))
    '----- V4.0.0.0-26↑ -----

    Public giNgBoxCount(0 To MAXWORK_KND - 1) As Integer            ' NG排出BOXの収納枚数(基板品種分)   ###089
    Public giNgBoxCounter As Integer = 0                            ' NG排出BOXの収納枚数カウンター     ###089

    Public giBreakCounter As Integer = 0                            ' 割れ欠け発生の収納枚数カウンター     ###130 
    Public giTwoTakeCounter As Integer = 0                          ' ２枚取り発生の収納枚数カウンター     ###130 

    Public m_lTrimResult As Integer = cFRS_NORMAL                   ' 基板単位のトリミング結果(SL436R自動運転時のNG排出BOXの収納枚数カウント用) ###089
    '                                                               ' cFRS_NORMAL (正常)
    '                                                               ' cFRS_TRIM_NG(トリミングNG)
    '                                                               ' cFRS_ERR_PTN(パターン認識エラー) ※なし
    Public bFgAutoMode As Boolean = False                           ' ローダ自動モードフラグ
    Public gbCycleStop As Boolean = False                           ' サイクル停止処理有無V5.0.0.4①
    Public bFgCyclStp As Boolean = False                            ' サイクル停止フラグ V4.0.0.0⑲
    Public iExcamCutBlockNo_X As Integer                            ' 外部カメラカット位置画像登録のブロックNoX軸を千鳥対応の為に１に固定する。'V1.25.0.0⑫
    '----- 連続運転用(SL436R用) -----
    Public m_lTrimNgCount As Integer = 0                            ' 連続トリミングＮＧ枚数カウンター(自動運転用 KOA EW殿SL432RD対応) V6.1.4.0⑨

    Public iInverseStepY As Integer                                 ' Y方向のステップを逆転する 'V4.12.0.0①　'V6.1.2.0②

    Public giGazouClrTime As Integer                                ' 画像表示クリア用のタイマ    'V4.12.2.2⑤ 'V6.0.5.0⑧

    ' V6.0.3.0_47
    Public giMoveModeWheelDisable As Integer = 0                    ' メイン画面のMoveModeでMouseWheelを有効とするかの設定
    'Public strPlateDataFileFullPath() As String             ' 連続運転登録ﾘｽﾄﾌﾙﾊﾟｽ文字列配列
    'Public intPlateDataFileNum As Short                     ' 連続運転登録ﾘｽﾄﾌﾙﾊﾟｽ文字列数
    'Public intActMode As Short                              ' 連続運転動作ﾓｰﾄﾞ(0:ﾏｶﾞｼﾞﾝﾓｰﾄﾞ 1:ﾛｯﾄﾓｰﾄﾞ 2:ｴﾝﾄﾞﾚｽﾓｰﾄﾞ)

    'Public INTRTM_Ver As String 'INtime Version
    'Public LMP_No As String 'LMP No

    Public giJogWaitMode As Integer                                 ' Jog動作時のステージ戻り待ち有無 0:あり、1:なし   'V6.1.2.0①


    '' '' ''Public gfX_2IT As Double ' IT標準偏差算出用ワーク
    '' '' ''Public gfX_2FT As Double ' FT標準偏差算出用ワーク

    Public glITTOTAL As Long                                        ' IT計算対象数 ###138
    Public glFTTOTAL As Long                                        ' FT計算対象数 ###138

    'Public gbEditPassword As Short ' データ入力時のパスワード要求(0:無 1:有)
    Public gITNx() As Double                                        'IT 測定誤差(個々)
    Public gFTNx() As Double                                        'FT 測定誤差(個々)

    Public gITNx_cnt As Integer                                     'IT 算出用ﾜｰｸ数
    Public gITNg_cnt As Integer                                     'IT NG数記録
    Public gFTNx_cnt As Integer                                     'FT 算出用ﾜｰｸ数
    Public gFTNg_cnt As Integer                                     'FT NG数記録
    'Public giXmode As Short
    Public gLogMode As Integer                                      'ﾛｷﾞﾝｸﾞﾓｰﾄﾞ(0:しない, 1:INITIAL TEST, 2:FINAL TEST, 3:INITIAL + FINAL) ###150 

    Public StepTab_Mode As Short                                    '(0)Step (1)Group
    Public StepFGMove As Short                                      '(0)なし　(1)ｽﾃｯﾌﾟｸﾞﾘｯﾄﾞ間移動あり[->]  (2)ｽﾃｯﾌﾟｸﾞﾘｯﾄﾞ間移動あり[<-]
    Public StepTitle(2) As Short                                    '(0)入力あり　(1)入力なし

    '--ROHM--
    Public giLoginPass As Boolean                                   '起動時ﾊﾟｽﾜｰﾄﾞ入力(False)NG (True)OK

    Public sIX2LogFilePath As String                                ' IX2 LOGﾌｧｲﾙﾊﾟｽ名
    Public gsESLogFilePath As String                                ' ES LOGﾌｧｲﾙﾊﾟｽ名

    ' frmFineAdjust.vbでのみ使用する変数
    '   フォーム終了後に値の取得が必要なため、
    '   グローバルで変数を設定する。
    Public gCurPlateNo As Integer
    Public gCurBlockNo As Integer
    '#4.12.2.0⑥    Public gFrmEndStatus As Integer
    Friend gCurPlateNoX As Integer
    Friend gCurPlateNoY As Integer
    Friend gCurBlockNoX As Integer
    Friend gCurBlockNoY As Integer
    '#4.12.2.0⑥                         ↑

    '----- ログ画面表示用 -----　                                   '###013
    Public gDspClsCount As Integer                                  ' ログ画面表示クリア基板枚数
    Public gDspCounter As Integer                                   ' ログ画面表示基板枚数カウンタ

    '----- 一時停止画面用 -----
    Public gbExitFlg As Boolean                                     '###014
    Public gbTenKeyFlg As Boolean = True                            ' テンキー入力フラグ ###057
    Public gbChkboxHalt As Boolean = True                           ' ADJボタン状態(ON=ADJ ON, OFF=ADJ OFF) ###009
    Public gbHaltSW As Boolean = False                              ' HALT SW状態退避 ###255
    Public gbChkSubstrateSet As Boolean = False                     ' 基板投入ボタン状態 V4.11.0.0⑥
    'V6.0.0.0⑯    Public gObjADJ As Object = Nothing                              ' 一時停止画面オブジェクト ###053
    Public gObjADJ As frmFineAdjust = Nothing                       ' 一時停止画面オブジェクト      'V6.0.0.0⑯

    'Public gObjMSG As Object = Nothing                              ' 基板投入待ちメッセージ表示用
    Public gObjMSG As FrmWait = Nothing                              ' 基板投入待ちメッセージ表示用

    Public gbLastSubstrateSet As Boolean = False                    ' 最終基板処理　'V4.11.0.0⑯


    '----- EXTOUT LED制御ビット -----                               '###061
    Public glLedBit As Long                                         ' LED制御ビット(EXTOUT) 

    '----- GP-IB制御 -----
    Public bGpib2Flg As Integer = 0                                 ' GP-IB制御(汎用)フラグ(0=制御なし, 1=制御あり) ###229
    'V1.13.0.0⑤
    'Public Const BLOCK_COUNT_MAX As Integer = 256          ' ブロック数最大数   'V5.0.0.8① TrimDataEditorへ

    Public Coordinates(BLOCK_COUNT_MAX, BLOCK_COUNT_MAX) As POINTD
    Public OriginalBlock(BLOCK_COUNT_MAX, BLOCK_COUNT_MAX) As POINTD
    'Public SelectBlock(BLOCK_COUNT_MAX, BLOCK_COUNT_MAX) As Integer            'V5.0.0.8① TrimDataEditorへ
    'V1.13.0.0⑤

    ''' <summary>処理対象ブロック [1ORG] (0:処理しない, 1:処理する)</summary>
    Friend ProcBlock(,) As Integer = Nothing                            '#4.12.2.0②

    Public INFO_MSG18, INFO_MSG20, ERR_TXNUM_E As String                'V4.4.0.0-0

    'V2.0.0.0⑬　↓
    '----- 途中切り検出用構造体 -----　
    Public Structure MidiumCut_Struct                     ' 途中切り検出用構造体 
        Dim DetectFunc As Long                                  ' 途中切り検出フラグ(0:しない, 1:する)
        Dim JudgeCount As Long                                  ' 途中切り検出判定回数 (回)
        Dim dblChangeRate As Double                             ' 途中切り検出変化率 (%)
    End Structure

    Public MidiumCut As MidiumCut_Struct
    'V2.0.0.0⑬　↑

    'V2.0.0.0⑭　↓
    Public glProbeRetryCount As Long
    'V2.0.0.0⑭　↑
#If START_KEY_SOFT Then
    Public gbStartKeySoft As Boolean
#End If
    '----- V2.0.0.0_30↓ -----
    ' 編集画面ができるまでの一時的な保存用変数：本来はトリミングデータ変数から取得する 
    '
    Public globaldblCleaningPosX As Double
    Public globaldblCleaningPosY As Double
    Public globaldblCleaningPosZ As Double
    '----- V2.0.0.0_30↑ -----

    '----- V2.0.0.0_29↓ -----
    '--------------------------------------------------------------------------
    '   軸PCLパラメータ(SL436S用)構造体形式定義
    '--------------------------------------------------------------------------
    Public Structure PCLAXIS_Info
        Dim FL As Integer                                   ' 初速度（FL）
        Dim FH As Integer                                   ' 動作速度（FH）
        Dim DrvRat As Integer                               ' 加速レート
        Dim Magnif As Integer                               ' 倍率
    End Structure
    Public stPclAxisPrm(2, 2) As PCLAXIS_Info               ' X,Y軸PCLパラメータ(軸, FH/STE&PREPEAT) 0 ORG
    '----- V2.0.0.0_29↑ -----

    Public INTERNAL_CAMERA As Integer = 0                   ' 内部ｶﾒﾗ  'V3.0.0.0③
    Public EXTERNAL_CAMERA As Integer = 1                   ' 外部ｶﾒﾗ  'V3.0.0.0③

    '----- V4.0.0.0-58↓ -----
    ' トリマー加工条件構造体(ワーク) 
    Public gwkCND As TrimCondInfo                           ' トリマー加工条件(形式定義はRs232c.vb参照)
    Public gwkCndCount As Integer                           ' 登録数 
    Public gwkPower(MAX_BANK_NUM) As Double
    '----- V4.0.0.0-58↑ -----

    'V4.9.0.0①
    Public Const UNIT_BLOCK As Integer = 0                  ' ブロック単位
    Public Const UNIT_PLATE As Integer = 1                  ' 基板単位
    Public Const UNIT_LOT As Integer = 2                    ' Lot単位

    Public Const UNIT_LO_NG As Integer = 0                  ' Low-NG
    Public Const UNIT_HI_NG As Integer = 1                  ' High-NG
    Public Const UNIT_OPEN_NG As Integer = 2                ' Open-NG

    Public Structure NG_RATE_STOP_Info
        Dim CheckTimmingPlate As Boolean                     '判定のタイミング基板ごと
        Dim CheckTimmingBlock As Boolean                    '判定のタイミングブロックごと
        Dim CheckYeld As Boolean
        Dim CheckOverRange As Boolean
        Dim CheckITHI As Boolean
        Dim CheckITLO As Boolean
        Dim CheckFTHI As Boolean
        Dim CheckFTLO As Boolean

        Dim SelectUnit As Integer                           ' 単位の選択Block/Plate/Lot
        Dim ValYield As Double                              ' 歩留まり判定％
        Dim ValOverRange As Double                          ' OverRange判定％
        Dim ValITHI As Double                               ' ITHI判定％
        Dim ValITLO As Double                               ' ITLO判定％
        Dim ValFTHI As Double                               ' FTHI判定％
        Dim ValFTLO As Double                               ' FTLO判定％
    End Structure
    Public JudgeNgRate As NG_RATE_STOP_Info

    Public NGJudgeResult As Integer
    'V4.9.0.0①

    Public bAllMagazineFinFlag As Boolean                   ' 全マガジン終了状態 'V5.0.0.1②
    Public bEmergencyOccurs As Boolean = False              ' V1.25.0.5②
    Public gbAutoOperating As Boolean = False               ' 自動運転中印刷不可フラグ V6.0.3.0_31

    Public flgLoginPWD As Short                             ' ﾛｸﾞｲﾝﾊﾟｽﾜｰﾄﾞ入力の有無(0:無, 1:有) V6.0.3.0_42

#End Region

    '========================================================================================
    '   ジョグ操作用変数定義(ＴＸ/ＴＹティーチング他共通)
    '========================================================================================
#Region "ジョグ操作用変数定義"
    '-------------------------------------------------------------------------------
    '   ジョグ操作用定義
    '-------------------------------------------------------------------------------
    Public giCurrentNo As Integer                               ' 処理中の行番号(グリッド表示用)

    '----- JOG操作用パラメータ形式定義(OcxJOGを使用しない場合) -----
    Public Structure JOG_PARAM
        Dim Md As Short                                         ' 処理モード(0:XYﾃｰﾌﾞﾙ移動, 1:BP移動, 2:キー入力待ちモード)
        Dim Md2 As Short                                        ' 入力モード(0:画面ﾎﾞﾀﾝ入力, 1:ｺﾝｿｰﾙ入力)
        Dim Opt As UShort                                       ' オプション(キーの有効(1)/無効(0)指定)
        '                                                       '  BIT0:STARTキー
        '                                                       '  BIT1:RESETキー
        '                                                       '  BIT2:Zキー
        '                                                       '  BIT3:
        '                                                       '  BIT4:未使用
        '                                                       '  BIT5:HALTキー
        '                                                       '  BIT6:未使用
        '                                                       '  BIT7-15:未使用
        Dim Flg As Short                                        ' 親画面のOK/Cancelﾎﾞﾀﾝ押下ﾌﾗｸﾞ(cFRS_ERR_ADV, cFRS_ERR_RST)
        Dim PosX As Double                                      ' BP or ﾃｰﾌﾞﾙ X位置
        Dim PosY As Double                                      ' BP or ﾃｰﾌﾞﾙ Y位置
        Dim BpOffX As Double                                    ' BPｵﾌｾｯﾄX 
        Dim BpOffY As Double                                    ' BPｵﾌｾｯﾄY
        Dim BszX As Double                                      ' ﾌﾞﾛｯｸｻｲｽﾞX 
        Dim BszY As Double                                      ' ﾌﾞﾛｯｸｻｲｽﾞY
        'V6.0.0.0⑪        Dim TextX As Object                                     ' BP or ﾃｰﾌﾞﾙ X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
        Dim TextX As Control                                     ' BP or ﾃｰﾌﾞﾙ X位置表示用ﾃｷｽﾄﾎﾞｯｸｽ  'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim TextY As Object                                     ' BP or ﾃｰﾌﾞﾙ Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ
        Dim TextY As Control                                     ' BP or ﾃｰﾌﾞﾙ Y位置表示用ﾃｷｽﾄﾎﾞｯｸｽ  'V6.0.0.0⑪
        Dim cgX As Double                                       ' 移動量X 
        Dim cgY As Double                                       ' 移動量Y
        Dim bZ As Boolean                                       ' Zキー  (True:ON, False:OFF)

        'V6.0.0.0⑪        Dim BtnHI As Object                                     ' HIボタン
        Dim BtnHI As Button                                     ' HIボタン     'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim BtnZ As Object                                      ' Zボタン
        Dim BtnZ As Button                                      ' Zボタン      'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim BtnSTART As Object                                  ' STARTボタン
        Dim BtnSTART As Button                                  ' STARTボタン  'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim BtnHALT As Object                                   ' HALTボタン
        Dim BtnHALT As Button                                   ' HALTボタン   'V6.0.0.0⑪
        'V6.0.0.0⑪        Dim BtnRESET As Object                                  ' RESETボタン
        Dim BtnRESET As Button                                  ' RESETボタン

        Dim TenKey() As Button          'V6.0.0.0-22
        Dim KeyDown As Keys             'V6.0.0.0-22

        Dim CurrentNo As Integer                                ' 処理中の行番号(グリッド表示用)

        Public Sub ResetButtonStyle()   'V6.0.0.0-22
            For Each btn As Button In TenKey
                btn.FlatStyle = FlatStyle.Standard
            Next
            TenKey(0).Parent.Select()

            KeyDown = Keys.None
        End Sub
    End Structure

    '----- ZINPSTS関数(コンソール入力)戻値 -----
    Public Const CONSOLE_SW_START As UShort = &H1           ' bit 0(01)  : START       0/1=未動作/動作
    Public Const CONSOLE_SW_RESET As UShort = &H2           ' bit 1(02)  : RESET       0/1=未動作/動作
    Public Const CONSOLE_SW_ZSW As UShort = &H4             ' bit 2(04)  : Z_ON/OFF_SW 0/1=未動作/動作
    Public Const CONSOLE_SW_ZDOWN As UShort = &H8           ' bit 3(08)  : Z_DOWN      1=状態センス
    Public Const CONSOLE_SW_ZUP As UShort = &H10            ' bit 4(10)  : Z_UP        1=状態センス
    Public Const CONSOLE_SW_HALT As UShort = &H20           ' bit 5(20)  : HALT        0/1=未動作/動作

    '----- コンソールキーSW -----
    'Public Const cBIT_ADV As UShort = &H1US                 ' START(ADV)キー
    'Public Const cBIT_HALT As UShort = &H2US                ' HALTキー
    'Public Const cBIT_RESET As UShort = &H8US               ' RESETキー
    'Public Const cBIT_Z As UShort = &H20US                  ' Zキー
    Public Const cBIT_HI As UShort = &H100US                ' HIキー

    '----- 処理モード定義 -----
    Public Const MODE_STG As Integer = 0                    ' XYテーブルモード
    Public Const MODE_BP As Integer = 1                     ' BPモード
    Public Const MODE_KEY As Integer = 2                    ' キー入力待ちモード

    '----- プローブモード/サブモード定義 -----
    'Public Const MODE_STG      As Integer = 0              ' XYテーブルモード
    'Public Const MODE_BP       As Integer = 1              ' BPモード
    Public Const MODE_Z As Integer = 2                      ' Zﾓｰﾄﾞ
    Public Const MODE_TTA As Integer = 3                    ' θﾓｰﾄﾞ
    Public Const MODE_Z2 As Integer = 4                     ' Z2ﾓｰﾄﾞ

    Public Const MODE_PRB As Integer = 10                   ' 接触位置確認モード
    Public Const MODE_RECOG As Integer = 20                 ' θ補正手動位置合せモード
    ' ※アプリモードは「トリミング中」
    Public Const MODE_POSOFS As Integer = 21                ' 補正ポジションオフセット調整モード
    ' ※アプリモードは「パターン登録(θ補正)」

    '----- 入力モード -----
    Public Const MD2_BUTN As Integer = 0                    ' 画面ボタン入力
    Public Const MD2_CONS As Integer = 1                    ' コンソール入力
    Public Const MD2_BOTH As Integer = 2                    ' 両方

    '----- ピッチ最大値/最小値 -----
    Public Const cPT_LO As Double = 0.001                   ' ﾋﾟｯﾁ最小値(mm)
    Public Const cPT_HI As Double = 0.1                     ' ﾋﾟｯﾁ最大値(mm)
    Public Const cHPT_LO As Double = 0.01                   ' HIGHﾋﾟｯﾁ最小値(mm)
    Public Const cHPT_HI As Double = 5.0#                   ' HIGHﾋﾟｯﾁ最大値(mm)
    Public Const cPAU_LO As Double = 0.05                   ' ポーズ最小値(sec)
    Public Const cPAU_HI As Double = 1.0#                   ' ポーズ最大値(sec)

    '----- 添え字 -----
    Public Const IDX_PIT As Short = 0                       ' ﾋﾟｯﾁ
    Public Const IDX_HPT As Short = 1                       ' HIGHﾋﾟｯﾁ
    Public Const IDX_PAU As Short = 2                       ' ポーズ

    '----- その他 -----
    'Private dblTchMoval(3) As Double                           ' ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time(Sec))
    Private InpKey As UShort                                    ' ｺﾝｿｰﾙｷｰ入力域 
    Private cin As UShort                                       ' ｺﾝｿｰﾙ入力値
    Private bZ As Boolean                                       ' Zキー 退避域 (True:ON, False:OFF)
    Private bHI As Boolean                                      ' HIキー(True:ON, False:OFF)

    Private mPIT As Double                                      ' 移動ﾋﾟｯﾁ
    Private X As Double                                         ' 移動ﾋﾟｯﾁ(X)
    Private Y As Double                                         ' 移動ﾋﾟｯﾁ(Y)
    Private NOWXP As Double                                     ' BP現在値X(ｸﾛｽﾗｲﾝ補正用)
    Private NOWYP As Double                                     ' BP現在値Y(ｸﾛｽﾗｲﾝ補正用)
    Private mvx As Double                                       ' BP/ﾃｰﾌﾞﾙ等の位置X
    Private mvy As Double                                       ' BP/ﾃｰﾌﾞﾙ等の位置Y
    Private mvxBk As Double                                     ' BP/ﾃｰﾌﾞﾙ等の位置X(退避用)
    Private mvyBk As Double                                     ' BP/ﾃｰﾌﾞﾙ等の位置Y(退避用)
#End Region

    '========================================================================================
    '   ＪＯＧ操作画面処理用共通関数
    '========================================================================================
#Region "初期設定処理"
    '''=========================================================================
    '''<summary>初期設定処理</summary>
    '''<param name="stJOG">       (INP)JOG操作用パラメータ</param>
    '''<param name="TBarLowPitch">(I/O)スライダー1(Lowﾋﾟｯﾁ)</param>
    '''<param name="TBarHiPitch"> (I/O)スライダー2(HIGHﾋﾟｯﾁ)</param>
    '''<param name="TBarPause">   (I/O)スライダー3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)目盛1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)目盛2(Lowﾋﾟｯﾁ Label)</param>
    '''<param name="LblTchMoval2">(I/O)目盛3(HIGHﾋﾟｯﾁ Label)</param>
    '''<param name="dblTchMoval"> (I/O)ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time)</param>
    '''=========================================================================
    Public Sub JogEzInit(ByVal stJOG As JOG_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double)

        Dim strMSG As String

        Try
            ' 移動ピッチスライダー初期設定
            If (stJOG.Md = MODE_BP) Then                            ' モード = 1(BP移動) ?
                dblTchMoval(IDX_PIT) = gSysPrm.stSYP.gBpPIT         ' BP用ﾋﾟｯﾁ設定
                dblTchMoval(IDX_HPT) = gSysPrm.stSYP.gBpHighPIT
                dblTchMoval(IDX_PAU) = gSysPrm.stSYP.gPitPause
            Else
                dblTchMoval(IDX_PIT) = gSysPrm.stSYP.gPIT           ' XYテーブル用ﾋﾟｯﾁ設定
                dblTchMoval(IDX_HPT) = gSysPrm.stSYP.gStageHighPIT
                dblTchMoval(IDX_PAU) = gSysPrm.stSYP.gPitPause
            End If
            Call XyzBpMovingPitchInit(TBarLowPitch, TBarHiPitch, TBarPause, _
                                      LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            Call Form1.System1.SetSysParam(gSysPrm)                 ' システムパラメータの設定(OcxSystem用)

            InpKey = 0

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.JogEzInit() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "BP/XYテーブルのJOG操作(Do Loopなし)"
    '''=========================================================================
    '''<summary>BP/XYテーブルのJOG操作 ###047</summary>
    '''<param name="stJOG">       (INP)JOG操作用パラメータ</param>
    '''<param name="TBarLowPitch">(I/O)スライダー1(Lowﾋﾟｯﾁ)</param>
    '''<param name="TBarHiPitch"> (I/O)スライダー2(HIGHﾋﾟｯﾁ)</param>
    '''<param name="TBarPause">   (I/O)スライダー3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)目盛1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)目盛2(Lowﾋﾟｯﾁ Label)</param>
    '''<param name="LblTchMoval2">(I/O)目盛3(HIGHﾋﾟｯﾁ Label)</param>
    '''<param name="dblTchMoval"> (I/O)ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time)</param>
    '''<returns>cFRS_ERR_ADV = OK(STARTｷｰ) 
    '''         cFRS_ERR_RST = Cancel(RESETｷｰ)
    '''         cFRS_ERR_HLT = HALTｷｰ
    '''         -1以下       = エラー</returns>
    ''' <remarks>JogEzInit関数をCall済であること</remarks>
    '''=========================================================================
    Public Function JogEzMove_Ex(ByRef stJOG As JOG_PARAM, ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double) As Integer

        Dim strMSG As String
        Dim r As Short

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
            X = 0.0 : Y = 0.0                                           ' 移動ﾋﾟｯﾁX,Y
            mvx = stJOG.PosX : mvy = stJOG.PosY                         ' BP or ﾃｰﾌﾞﾙ位置X,Y
            mvxBk = stJOG.PosX : mvyBk = stJOG.PosY
            ' キャリブレーション実行/カット位置補正【外部カメラ】時 ※相対座標を表示するためクリアしない
            ' トリミング時の一時停止画面もクリアしない
            If (giAppMode <> APP_MODE_CARIB_REC) And (giAppMode <> APP_MODE_CUTREVIDE) And _
               (giAppMode <> APP_MODE_FINEADJ) And (giAppMode <> APP_MODE_LDR_ALRM) Then   'V1.16.0.0⑩   '###088
                '(giAppMode <> APP_MODE_TRIM) Then                      '###088
                stJOG.cgX = 0.0# : stJOG.cgY = 0.0#                     ' 移動量X,Y
            End If

            'If (giAppMode = APP_MODE_TRIM) Then                        '###088
            If (giAppMode = APP_MODE_FINEADJ) Then                      '###088
                mvx = stJOG.cgX - stJOG.BpOffX : mvy = stJOG.cgY - stJOG.BpOffY
                mvxBk = mvx : mvyBk = mvy
            End If

            Call Init_Proc(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' 現在の位置を表示する(ﾃｷｽﾄﾎﾞｯｸｽの背景色を処理中(黄色)に設定する)
            Call DispPosition(stJOG, 1)
            'Call SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE)
            'Call Me.Focus()                                            ' フォーカスを設定する(テンキー入力のため)
            ''                                                          ' KeyPreviewプロパティをTrueにすると全てのキーイベントをまずフォームが受け取るようになる。
            '---------------------------------------------------------------------------
            '   コンソールボタン又はコンソールキーからのキー入力処理を行う
            '---------------------------------------------------------------------------
            ' システムエラーチェック
            r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
            If (r <> cFRS_NORMAL) Then Return (r)

            ' メッセージポンプ
            System.Windows.Forms.Application.DoEvents()

            '----- ###232↓ -----
            '' 補正クロスライン表示処理(BP移動モードでTeach時)
            'If (stJOG.Md = MODE_BP) Then                                ' モード = 1(BP移動) ?
            '    NOWXP = 0.0 : NOWYP = 0.0
            '    If (gSysPrm.stCRL.giDspFlg = 1) Then                    ' 補正クロスライン表示 ?
            '        If (gSysPrm.stCRL.giDspFlg = 1) And _
            '           (giAppMode = APP_MODE_TEACH) Then                ' 補正クロスライン表示 ?
            '            Call ZGETBPPOS(NOWXP, NOWYP)                    ' BP現在位置取得
            '            gstCLC.x = NOWXP                                ' BP位置X(mm)
            '            gstCLC.y = NOWYP                                ' BP位置Y(mm)
            '            Call CrossLineCorrect(gstCLC)                   ' 補正クロスライン表示
            '        End If
            '    End If
            'End If
            '----- ###232↑ -----

            ' コンソールボタン又はコンソールキーからのキー入力
            Call ReadConsoleSw(stJOG, cin)                              ' キー入力

            '-----------------------------------------------------------------------
            '   入力キーチェック
            '-----------------------------------------------------------------------
            If (cin And CONSOLE_SW_RESET) Then                          ' RESET SW ?
                ' RESET SW押下時
                If (stJOG.Opt And CONSOLE_SW_RESET) Then                ' RESETキー有効 ?
                    Return (cFRS_ERR_RST)                               ' Return値 = Cancel(RESETｷｰ)
                End If

                ' HALT SW押下時
            ElseIf (cin And CONSOLE_SW_HALT) Then                       ' HALT SW ?
                If (stJOG.Opt And CONSOLE_SW_HALT) Then                 ' オプション(0:HALTキー無効, 1:HALTキー有効)
                    r = cFRS_ERR_HALT                                   ' Return値 = HALTｷｰ
                    GoTo STP_END
                End If

                ' START SW押下時
            ElseIf (cin And CONSOLE_SW_START) Then                      ' START SW ?
                ''V4.0.0.0-86
                If (gKeiTyp = KEY_TYPE_RS) Then
                    r = GetLaserOffIO(True) 'V5.0.0.1⑫
                    If r = 1 Then
                        ''V5.0.0.1⑨↓
                        r = cFRS_NORMAL
                        Call ZCONRST()
                        ''V5.0.0.1⑨↑
                    Else
                        If (stJOG.Opt And CONSOLE_SW_START) Then                ' STARTキー有効 ?
                            r = cFRS_ERR_START                                  ' Return値 = OK(STARTｷｰ) 
                            GoTo STP_END
                        End If
                    End If
                Else
                    If (stJOG.Opt And CONSOLE_SW_START) Then                ' STARTキー有効 ?
                        r = cFRS_ERR_START                                  ' Return値 = OK(STARTｷｰ) 
                        GoTo STP_END
                    End If
                End If
                ''V4.0.0.0-86


                ' Z SWがONからOFF(又はOFFからON)に切替わった時
            ElseIf (stJOG.bZ <> bZ) Then
                If (stJOG.Opt And CONSOLE_SW_ZSW) Then                  ' Zキー有効 ?
                    r = cFRS_ERR_Z                                      ' Return値 = ZｷｰON/OFF
                    stJOG.bZ = bZ                                       ' ON/OFF
                    GoTo STP_END
                End If

                ' 矢印SW押下時
            ElseIf cin And &H1E00US Then                                ' 矢印SW
                '「キー入力待ちモード」なら何もしない
                If (stJOG.Md = MODE_KEY) Then

                Else
                    If cin And &H100US Then                             ' HI SW ? 
                        mPIT = dblTchMoval(IDX_HPT)                     ' mPIT = 移動高速ﾋﾟｯﾁ
                    Else
                        mPIT = dblTchMoval(IDX_PIT)                     ' mPIT = 移動通常ﾋﾟｯﾁ
                    End If

                    ' XYテーブル絶対値移動(ソフトリミットチェック有り)
                    r = cFRS_NORMAL
                    If (stJOG.Md = MODE_STG) Then                       ' モード = XYテーブル移動 ?
                        ' XYテーブル絶対値移動
                        r = Sub_XYtableMove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then                      ' ｴﾗｰ ?
                            If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                Return (r)                              ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                            End If
                        End If

                        '  モード = BP移動の場合
                    ElseIf (stJOG.Md = MODE_BP) Then
                        ' BP絶対値移動
                        r = Sub_BPmove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then                      ' BP移動エラー ?
                            If (Form1.System1.IsSoftLimitBP(r) = False) Then
                                Return (r)                              ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                            End If
                        End If
                    End If

                    ' ソフトリミットエラーの場合は HI SW以外はOFFする
                    If (r <> cFRS_NORMAL) Then                          ' ｴﾗｰ ?
                        If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then
                            InpKey = cBIT_HI                            ' HI SW ON
                        Else
                            InpKey = 0                                  ' HI SW以外はOFF
                        End If
                        r = cFRS_NORMAL                                 ' Retuen値 = 正常 ###143 
                        stJOG.ResetButtonStyle()                        'V6.0.0.0-22
                    End If

                    ' 現在の位置を表示する
                    Call DispPosition(stJOG, 1)
                    'Call Form1.System1.WAIT(SysPrm.stSYP.gPitPause)    ' Wait(sec)'###251
                    Call Form1.System1.WAIT(dblTchMoval(IDX_PAU))       ' Wait(sec)'###251
                End If

                InpKey = CType(CtrlJog.MouseClickLocation.Clear(InpKey), UShort)    'V6.0.0.0⑧
                stJOG.KeyDown = Keys.None                                           'V6.0.0.0-23
            End If

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
STP_END:
            'stJOG.PosX = mvx                                            ' 位置X,Y更新
            'stJOG.PosY = mvy
            Return (r)                                                  ' Return値設定 

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.JogEzMove_Ex() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー 
        End Try

    End Function
#End Region

#Region "BP/XYテーブルのJOG操作"
    '''=========================================================================
    '''<summary>BP/XYテーブルのJOG操作</summary>
    '''<param name="stJOG">       (INP)JOG操作用パラメータ</param>
    '''<param name="TBarLowPitch">(I/O)スライダー1(Lowﾋﾟｯﾁ)</param>
    '''<param name="TBarHiPitch"> (I/O)スライダー2(HIGHﾋﾟｯﾁ)</param>
    '''<param name="TBarPause">   (I/O)スライダー3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)目盛1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)目盛2(Lowﾋﾟｯﾁ Label)</param>
    '''<param name="LblTchMoval2">(I/O)目盛3(HIGHﾋﾟｯﾁ Label)</param>
    '''<param name="dblTchMoval"> (I/O)ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time)</param>
    ''' <param name="commonMethods">メインフォームに設定するJOG制御関数</param>
    '''<returns>cFRS_ERR_ADV = OK(STARTｷｰ) 
    '''         cFRS_ERR_RST = Cancel(RESETｷｰ)
    '''         cFRS_ERR_HLT = HALTｷｰ
    '''         -1以下       = エラー</returns>
    ''' <remarks>JogEzInit関数をCall済であること</remarks>
    '''=========================================================================
    Public Function JogEzMove(ByRef stJOG As JOG_PARAM, ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label,
                         ByRef dblTchMoval() As Double,
                         ByVal commonMethods As ICommonMethods) As Integer      'V6.0.0.0⑪  引数 ICommonMethods 追加
        'V6.0.0.0⑪                         ByRef dblTchMoval() As Double) As Integer 

        Dim strMSG As String
        Dim r As Short

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
            X = 0.0 : Y = 0.0                                   ' 移動ﾋﾟｯﾁX,Y
            mvx = stJOG.PosX : mvy = stJOG.PosY                 ' BP or ﾃｰﾌﾞﾙ位置X,Y
            mvxBk = stJOG.PosX : mvyBk = stJOG.PosY
            ' キャリブレーション実行/カット位置補正【外部カメラ】時 ※相対座標を表示するためクリアしない
            ' トリミング時の一時停止画面もクリアしない
            If (giAppMode <> APP_MODE_CARIB_REC) And (giAppMode <> APP_MODE_CUTREVIDE) And _
               (giAppMode <> APP_MODE_FINEADJ) And (giAppMode <> APP_MODE_CARIB) Then             ' V1.14.0.0⑦ ###088
                '(giAppMode <> APP_MODE_TRIM) Then              '###088
                stJOG.cgX = 0.0# : stJOG.cgY = 0.0#             ' 移動量X,Y
            End If
            stJOG.Flg = -1
            InpKey = 0
            Call Init_Proc(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' 現在の位置を表示する(ﾃｷｽﾄﾎﾞｯｸｽの背景色を処理中(黄色)に設定する)
            Call DispPosition(stJOG, 1)
            'Call SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE)
            'Call Me.Focus()                                     ' フォーカスを設定する(テンキー入力のため)
            ''                                                   ' KeyPreviewプロパティをTrueにすると全てのキーイベントをまずフォームが受け取るようになる。

            ' メインフォームにJOG制御関数を設定する      'V6.0.0.0⑪
            Form1.Instance.SetActiveJogMethod(AddressOf commonMethods.JogKeyDown,
                                              AddressOf commonMethods.JogKeyUp,
                                              AddressOf commonMethods.MoveToCenter)
            '---------------------------------------------------------------------------
            '   コンソールボタン又はコンソールキーからのキー入力処理を行う
            '---------------------------------------------------------------------------
            Do
                ' システムエラーチェック
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then GoTo STP_END

                ' メッセージポンプ
                '  →VB.NETはマルチスレッド対応なので、本来はイベントの開放などでなく、
                '    スレッドを生成してコーディングをするのが正しい。
                '    スレッドでなくても、最低でタイマーを利用する。
                System.Windows.Forms.Application.DoEvents()
                System.Threading.Thread.Sleep(10)               ' CPU使用率を下げるためスリープ

                '----- ###232↓ -----
                '' 補正クロスライン表示処理(BP移動モードでTeach時)
                'If (stJOG.Md = MODE_BP) Then                    ' モード = 1(BP移動) ?
                '    NOWXP = 0.0 : NOWYP = 0.0
                '    If (gSysPrm.stCRL.giDspFlg = 1) Then        ' 補正クロスライン表示 ?
                '        If (gSysPrm.stCRL.giDspFlg = 1) And _
                '           (giAppMode = APP_MODE_TEACH) Then    ' 補正クロスライン表示 ?
                '            Call ZGETBPPOS(NOWXP, NOWYP)        ' BP現在位置取得
                '            gstCLC.x = NOWXP                    ' BP位置X(mm)
                '            gstCLC.y = NOWYP                    ' BP位置Y(mm)
                '            Call CrossLineCorrect(gstCLC)       ' 補正クロスライン表示
                '        End If
                '    End If
                'End If
                '----- ###232↑ -----

                ' コンソールボタン又はコンソールキーからのキー入力
                Call ReadConsoleSw(stJOG, cin)                  ' キー入力

                '-----------------------------------------------------------------------
                '   入力キーチェック
                '-----------------------------------------------------------------------
                If (cin And CONSOLE_SW_RESET) Then              ' RESET SW ?
                    ' RESET SW押下時
                    If (stJOG.Opt And CONSOLE_SW_RESET) Then    ' RESETキー有効 ?
                        r = cFRS_ERR_RST                        ' Return値 = Cancel(RESETｷｰ)
                        Exit Do
                    End If

                    ' HALT SW押下時
                ElseIf (cin And CONSOLE_SW_HALT) Then           ' HALT SW ?
                    If (stJOG.Opt And CONSOLE_SW_HALT) Then     ' オプション(0:HALTキー無効, 1:HALTキー有効)
                        r = cFRS_ERR_HALT                       ' Return値 = HALTｷｰ
                        Exit Do
                    End If

                    ' START SW押下時
                    'V6.1.4.2①                ElseIf (cin And CONSOLE_SW_START) Then          ' START SW ?
                ElseIf (cin And CONSOLE_SW_START) Or gbAutoCalibration Then          ' START SW ? 'V6.1.4.2①[自動キャリブレーション補正実行]
                    If (stJOG.Opt And CONSOLE_SW_START) Then    ' STARTキー有効 ?
                        'stJOG.PosX = mvx                       ' 位置X,Y更新
                        'stJOG.PosY = mvy
                        r = cFRS_ERR_START                      ' Return値 = OK(STARTｷｰ) 
                        Exit Do
                    End If

                    ' Z SWがONからOFF(又はOFFからON)に切替わった時
                ElseIf (stJOG.bZ <> bZ) Then
                    If (stJOG.Opt And CONSOLE_SW_ZSW) Then      ' Zキー有効 ?
                        r = cFRS_ERR_Z                          ' Return値 = ZｷｰON/OFF
                        stJOG.bZ = bZ                           ' ON/OFF
                        Exit Do
                    End If

                    ' 矢印SW押下時
                ElseIf cin And &H1E00US Then                    ' 矢印SW
                    '「キー入力待ちモード」なら何もしない
                    If (stJOG.Md = MODE_KEY) Then

                    Else
                        If cin And &H100US Then                     ' HI SW ? 
                            mPIT = dblTchMoval(IDX_HPT)             ' mPIT = 移動高速ﾋﾟｯﾁ
                        Else
                            mPIT = dblTchMoval(IDX_PIT)             ' mPIT = 移動通常ﾋﾟｯﾁ
                        End If

                        ' XYテーブル絶対値移動(ソフトリミットチェック有り)
                        r = cFRS_NORMAL
                        If (stJOG.Md = MODE_STG) Then                ' モード = XYテーブル移動 ?
                            ' XYテーブル絶対値移動
                            r = Sub_XYtableMove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                            If (r <> cFRS_NORMAL) Then              ' ｴﾗｰ ?
                                If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                    GoTo STP_END                    ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                                End If
                            End If

                            '  モード = BP移動の場合
                        ElseIf (stJOG.Md = MODE_BP) Then
                            ' BP絶対値移動
                            r = Sub_BPmove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                            If (r <> cFRS_NORMAL) Then              ' BP移動エラー ?
                                If (Form1.System1.IsSoftLimitBP(r) = False) Then
                                    GoTo STP_END                    ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                                End If
                            End If
                        End If

                        ' ソフトリミットエラーの場合は HI SW以外はOFFする
                        If (r <> cFRS_NORMAL) Then                  ' ｴﾗｰ ?
                            If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then
                                InpKey = cBIT_HI                    ' HI SW ON
                            Else
                                InpKey = 0                          ' HI SW以外はOFF
                            End If
                            stJOG.ResetButtonStyle()                'V6.0.0.0-22
                        End If

                        ' 現在の位置を表示する
                        Call DispPosition(stJOG, 1)
                        'V1.18.0.3⑤                        Call Form1.System1.WAIT(SysPrm.stSYP.gPitPause)    ' Wait(sec)
                        Call Form1.System1.WAIT(dblTchMoval(IDX_PAU))    ' Wait(sec) V1.18.0.3⑤
                    End If

                    InpKey = CType(CtrlJog.MouseClickLocation.Clear(InpKey), UShort)    'V6.0.0.0⑧
                    stJOG.KeyDown = Keys.None                                           'V6.0.0.0-23
                End If

            Loop While (stJOG.Flg = -1)

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
            ' 座標表示用ﾃｷｽﾄﾎﾞｯｸｽの背景色を白色に設定する
            Call DispPosition(stJOG, 0)

            ' 親画面からOK/Cancelﾎﾞﾀﾝ押下 ?
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            ' OK(STARTｷｰ)なら位置X,Y更新
            If (r = cFRS_ERR_START) Then                            ' OK(STARTｷｰ) ?
                stJOG.PosX = mvx                                    ' 位置X,Y更新
                stJOG.PosY = mvy
            End If

STP_END:
            Call ZCONRST()                                          ' ｺﾝｿｰﾙｷｰﾗｯﾁ解除 
            Return (r)                                              ' Return値設定 

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.JogEzMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー 

        Finally
            Form1.Instance.SetActiveJogMethod(Nothing, Nothing, Nothing)    'V6.0.0.0⑪
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
        End Try
    End Function
#End Region

#Region "初期設定処理"
    '''=========================================================================
    '''<summary>初期設定処理</summary>
    '''<param name="stJOG">       (INP)JOG操作用パラメータ</param>
    '''<param name="TBarLowPitch">(I/O)スライダー1(Lowﾋﾟｯﾁ)</param>
    '''<param name="TBarHiPitch"> (I/O)スライダー2(HIGHﾋﾟｯﾁ)</param>
    '''<param name="TBarPause">   (I/O)スライダー3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)目盛1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)目盛2(Lowﾋﾟｯﾁ Label)</param>
    '''<param name="LblTchMoval2">(I/O)目盛3(HIGHﾋﾟｯﾁ Label)</param>
    '''<param name="dblTchMoval"> (I/O)ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time)</param>
    '''=========================================================================
    Private Sub Init_Proc(ByVal stJOG As JOG_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double)

        Dim strMSG As String

        Try

            ' 移動ピッチスライダー設定(前回設定した値)
            Call XyzBpMovingPitchInit(TBarLowPitch, TBarHiPitch, TBarPause, _
                                      LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' ボタン有効/無効設定
            If (stJOG.Opt And CONSOLE_SW_HALT) Then                 ' HALTキー有効/無効
                stJOG.BtnHALT.Enabled = True
            Else
                stJOG.BtnHALT.Enabled = False
            End If
            If (stJOG.Opt And CONSOLE_SW_START) Then                ' STARTキー有効/無効
                stJOG.BtnSTART.Enabled = True
            Else
                stJOG.BtnSTART.Enabled = False
            End If
            If (stJOG.Opt And CONSOLE_SW_RESET) Then                ' RESETキー有効/無効
                stJOG.BtnRESET.Enabled = True
            Else
                stJOG.BtnRESET.Enabled = False
            End If
            If (stJOG.Opt And CONSOLE_SW_ZSW) Then                  ' Zキー有効/無効
                stJOG.BtnZ.Enabled = True
            Else
                stJOG.BtnZ.Enabled = False
            End If

            ' Zキー/HIキー状態等退避
            bZ = stJOG.bZ                                           ' Zキー退避
            If (bZ = False) Then                                    ' Zボタンの背景色を設定
                stJOG.BtnZ.BackColor = System.Drawing.SystemColors.Control ' 背景色 = 灰色
                stJOG.BtnZ.Text = "Z Off"
            Else
                stJOG.BtnZ.BackColor = System.Drawing.Color.Yellow        ' 背景色 = 黄色
                stJOG.BtnZ.Text = "Z On"
            End If

            If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then ' HIキー状態取得
                bHI = True
                InpKey = InpKey Or cBIT_HI                          ' HI SW ON
            Else
                bHI = False
                InpKey = InpKey And Not cBIT_HI                     ' HI SW OFF
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.Init_Proc() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "画面ボタン又はコンソールキーからのキー入力"
    '''=========================================================================
    '''<summary>画面ボタン又はコンソールキーからのキー入力</summary>
    '''<param name="stJOG">(INP)JOG操作用パラメータ</param>
    '''<param name="cin">  (OUT)コンソール入力値</param>
    '''=========================================================================
    Private Sub ReadConsoleSw(ByRef stJOG As JOG_PARAM, ByRef cin As UShort)

        Dim r As Integer
        Dim sw As Long
        Dim strMSG As String

        Try
            ' HALTキー入力チェック
            r = HALT_SWCHECK(sw)
            If (sw <> 0) Then                                           ' HALTキー押下 ?
                If (stJOG.Opt And CONSOLE_SW_HALT) Then                 ' HALTキー有効 ?
                    cin = CONSOLE_SW_HALT
                    Exit Sub
                End If
            End If

            ' Zキー入力チェック
            r = Z_SWCHECK(sw)                                           ' Zスイッチの状態をチェックする
            If (sw <> 0) Then                                           ' Zキー押下 ?
                If (stJOG.Opt And CONSOLE_SW_ZSW) Then                  ' Zキー有効 ?
                    Call SubBtnZ_Click(stJOG)
                    Exit Sub
                End If
            End If

            ' START/RESETキー入力チェック
            r = STARTRESET_SWCHECK(False, sw)                           ' START/RESETキー押下チェック(監視なしモード)

            ' コンソール入力値に変換して設定
            If (sw = cFRS_ERR_START) Then                               ' STARTキー押下 ?
                If (stJOG.Opt And CONSOLE_SW_START) Then                ' STARTキー有効 ?
                    cin = CONSOLE_SW_START
                    Exit Sub
                End If
            ElseIf (sw = cFRS_ERR_RST) Then                             ' RESETキー押下 ?
                If (stJOG.Opt And CONSOLE_SW_RESET) Then                ' RESETキー有効 ?
                    cin = CONSOLE_SW_RESET
                    Exit Sub
                End If
                '    ElseIf (sw = CONSOLE_SW_ZSW) Then                          ' Zキー押下 ?
                '        If (stJOG.opt And CONSOLE_SW_ZSW) Then                  ' Zキー有効 ?
                '            cin = CONSOLE_SW_ZSW
                '        End If
            End If

            ' 「画面ボタン入力」
            cin = InpKey                                                ' 画面ボタン入力

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.ReadConsoleSw() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "座標表示"
    '''=========================================================================
    '''<summary>座標表示</summary>
    '''<param name="stJOG">(INP)JOG操作用パラメータ</param>
    '''<param name="Md">   (INP)0=背景色を白色に設定, 1=背景色を処理中(黄色)に設定</param>
    '''=========================================================================
    Private Sub DispPosition(ByVal stJOG As JOG_PARAM, ByVal MD As Integer)

        Dim xPos As Double = 0.0                    ' ###232
        Dim yPos As Double = 0.0                    ' ###232
        Dim OffPosX As Double                       ' V1.13.0.0⑥
        Dim OffPosY As Double                       ' V1.13.0.0⑥
        Dim strMSG As String

        Try
            '「キー入力待ちモード」ならNOP
            If (stJOG.Md = MODE_KEY) Then Exit Sub

            ' 補正位置ティーチングならグリッドに表示する
            If (giAppMode = APP_MODE_CUTPOS) Then
                'V6.0.0.0⑪                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 2, (stJOG.PosX + stJOG.cgX).ToString("0.0000"))
                'V6.0.0.0⑪                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 3, (stJOG.PosY + stJOG.cgY).ToString("0.0000"))
                Exit Sub

                ' カット位置補正【外部カメラ】ならグリッドに相対座標を表示する
            ElseIf (giAppMode = APP_MODE_CUTREVIDE) Then
                'V4.7.0.0⑩                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 3, (stJOG.cgX).ToString("0.0000"))    ' ずれ量X
                'V4.7.0.0⑩                stJOG.TextX.set_TextMatrix(stJOG.CurrentNo, 4, (stJOG.cgY).ToString("0.0000"))    ' ずれ量Y
                Exit Sub
            End If

            ' テキストボックスに座標を表示する
            If (MD = 0) Then
                ' キャリブレーション実行時は背景色を灰色に設定
                If (giAppMode = APP_MODE_CARIB_REC) Then
                    ' 背景色を灰色に設定
                    stJOG.TextX.BackColor = System.Drawing.SystemColors.Control
                    stJOG.TextY.BackColor = System.Drawing.SystemColors.Control
                Else
                    ' 背景色を白色に設定
                    stJOG.TextX.BackColor = System.Drawing.Color.White
                    stJOG.TextY.BackColor = System.Drawing.Color.White
                End If
            Else
                ' キャリブレーション実行時は相対座標を表示
                'If (giAppMode = APP_MODE_CARIB_REC) Then   ' V1.14.0.0⑦
                If (giAppMode = APP_MODE_CARIB) Then        ' V1.14.0.0⑦
                    stJOG.TextX.Text = stJOG.cgX.ToString("0.0000")
                    stJOG.TextY.Text = stJOG.cgY.ToString("0.0000")
                ElseIf (giAppMode = APP_MODE_IDTEACH) Then
                    '                    Call XYtableMoveOffsetPosition(OffPosX, OffPosY)
                    Dim DirX As Integer = 1, DirY As Integer = 1
                    TrimClassCommon.GetDirectioinByBpDirXy(DirX, DirY)
                    frmIDReaderTeach.ObjOmronIDReader.GetIDReaderPotision(gSysPrm.stDEV.gfTrimX, gSysPrm.stDEV.gfTrimY, typPlateInfo.dblTableOffsetXDir, typPlateInfo.dblTableOffsetYDir, gfCorrectPosX, gfCorrectPosY, OffPosX, OffPosY)
                    stJOG.TextX.Text = ((stJOG.PosX + stJOG.cgX) * DirX + OffPosX).ToString("0.0000")
                    stJOG.TextY.Text = ((OffPosY - (stJOG.PosY + stJOG.cgY) * DirY) * -1).ToString("0.0000")
                    frmIDReaderTeach.ToucFinderUp()
                Else
                    ' その他のモード時は絶対座標を表示
                    stJOG.TextX.Text = (stJOG.PosX + stJOG.cgX).ToString("0.0000")
                    stJOG.TextY.Text = (stJOG.PosY + stJOG.cgY).ToString("0.0000")
                    '----- ###232↓ -----
                    ' トリミング時の一時停止画面表示中なら補正クロスラインを表示する
                    If (giAppMode = APP_MODE_FINEADJ) Or (giAppMode = APP_MODE_TX) Then
                        'xPos = Double.Parse(stJOG.TextX.Text)
                        'yPos = Double.Parse(stJOG.TextY.Text)
                        Call ZGETBPPOS(xPos, yPos)
                        ObjCrossLine.CrossLineDispXY(xPos, yPos)
                    End If
                    '----- ###232↑ -----
                End If
                ' 背景色を処理中(黄色)に設定
                stJOG.TextX.BackColor = System.Drawing.Color.Yellow
                stJOG.TextY.BackColor = System.Drawing.Color.Yellow
            End If

            stJOG.TextX.Refresh()
            stJOG.TextY.Refresh()

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.DispPosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ティーチングＳＷ取得"
    '''=========================================================================
    ''' <summary>ティーチングＳＷ取得</summary>
    ''' <param name="SysPrm">(INP)システムパラメータ</param>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェク</param>
    ''' <returns>0=OFF, 1:ON</returns>
    '''=========================================================================
    Private Function Z_TEACHSTS(ByVal SysPrm As SYSPARAM_PARAM, ByVal ObjSys As Object) As Long

        Dim r As Integer
        Dim strMSG As String

        Try
            ' データ入力 & ONビットチェック
            If (SysPrm.stIOC.giTeachSW = 1) Then                    ' ティーチングSW制御あり ?
                r = ObjSys.Inp_And_Check_Bit(SysPrm.stIOC.glTS_In_Adr, SysPrm.stIOC.glTS_In_ON, SysPrm.stIOC.giTS_In_ON_ST)
                If (r = 1) Then                                     ' TEACH_SW ON ?
                    r = 1                                           ' TEACH_SW ON
                Else
                    r = 0                                           ' TEACH_SW OFF
                End If

            Else
                r = 1                                               ' TEACH_SW ON
            End If
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.Z_TEACHSTS() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (1)
        End Try
    End Function
#End Region

#Region "BP絶対値移動(ソフトリミットチェック有り)"
    '''=========================================================================
    ''' <summary>BP絶対値移動(ソフトリミットチェック有り)</summary>
    ''' <param name="SysPrm">(INP)システムパラメータ</param>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェク</param>
    ''' <param name="ObjUtl">(INP)OcxUtilityオブジェク</param>
    ''' <param name="stJOG"> (I/O)JOG操作用パラメータ</param>
    ''' <returns>0=正常, 0以外:エラー</returns>
    '''=========================================================================
    Private Function Sub_BPmove(ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, ByVal ObjSys As Object, ByVal ObjUtl As Object, ByRef stJOG As JOG_PARAM) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' BP移動量の算出(→X,Y)
            mvxBk = mvx                                             ' 現在の位置退避
            mvyBk = mvy
            If ((cin And CtrlJog.MouseClickLocation.Move) = &H0) Then           'V6.0.0.0⑧
                Call ObjUtl.GetBPmovePitch(cin, X, Y, mPIT, mvx, mvy, SysPrm.stDEV.giBpDirXy)
            Else
                'V6.0.0.0⑧              ↓
                Dim dirX As Double = 0.0
                Dim dirY As Double = 0.0
                Dim tmpX As Double = 0.0
                Dim tmpY As Double = 0.0
                ObjUtl.GetBPmovePitch(cin, dirX, dirY, 1.0, tmpX, tmpY, SysPrm.stDEV.giBpDirXy)   ' 符号を取得

                X = Math.Abs(CtrlJog.MouseClickLocation.DistanceX) * Math.Sign(dirX)
                Y = Math.Abs(CtrlJog.MouseClickLocation.DistanceY) * Math.Sign(dirY)
                mvx -= X
                mvy -= Y
                'V6.0.0.0⑧              ↑
            End If

            ' BP絶対値移動(ソフトリミットチェック有り)
            r = ObjSys.BPMOVE(SysPrm, stJOG.BpOffX, stJOG.BpOffY, stJOG.BszX, stJOG.BszY, mvx, mvy, 1)
            If (r <> cFRS_NORMAL) Then                              ' ｴﾗｰならｴﾗｰﾘﾀｰﾝ(メッセージ表示済み)
                If (ObjSys.IsSoftLimitBP(r) = False) Then
                    GoTo STP_END                                    ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                End If
                mvx = mvxBk                                         ' BPｿﾌﾄﾘﾐｯﾄｴﾗｰ時はBP位置を戻す
                mvy = mvyBk
                GoTo STP_END                                        ' BPｿﾌﾄﾘﾐｯﾄｴﾗｰ
            End If

            stJOG.cgX = stJOG.cgX + (-1 * X)                        ' BP移動量X更新 (※移動量は反転しているので-1を掛ける)
            stJOG.cgY = stJOG.cgY + (-1 * Y)                        ' BP移動量Y更新

STP_END:
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.Sub_BPmove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "XYテーブル絶対値移動(ソフトリミットチェック有り)"
    '''=========================================================================
    ''' <summary>XYテーブル絶対値移動(ソフトリミットチェック有り)</summary>
    ''' <param name="SysPrm">(INP)システムパラメータ</param>
    ''' <param name="ObjSys">(INP)OcxSystemオブジェク</param>
    ''' <param name="ObjUtl">(INP)OcxUtilityオブジェク</param>
    ''' <param name="stJOG"> (I/O)JOG操作用パラメータ</param>
    ''' <returns>0=正常, 0以外:エラー</returns>
    '''=========================================================================
    Private Function Sub_XYtableMove(ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, ByVal ObjSys As Object, ByVal ObjUtl As Object, ByRef stJOG As JOG_PARAM) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' XYテーブル移動量の算出(→X,Y)
            mvxBk = X                                               ' 現在の位置退避
            mvyBk = Y
            'Call ObjUtl.GetXYmovePitch(cin, X, Y, mPIT)
            If ((cin And CtrlJog.MouseClickLocation.Move) = &H0) Then           'V6.0.0.0⑧
                Call TrimClassCommon.GetXYmovePitch(cin, X, Y, mPIT, giStageYDir) ' V4.0.0.0-51
            Else
                'V6.0.0.0⑧              ↓
                Dim dirX As Double = 0.0
                Dim dirY As Double = 0.0
                TrimClassCommon.GetXYmovePitch(cin, dirX, dirY, 1.0, giStageYDir)   ' 符号を取得

                X = -(Math.Abs(CtrlJog.MouseClickLocation.DistanceX) * Math.Sign(dirX)) 'V6.0.0.0-24 -() 追加
                Y = -(Math.Abs(CtrlJog.MouseClickLocation.DistanceY) * Math.Sign(dirY)) 'V6.0.0.0-24 -() 追加
                'V6.0.0.0⑧              ↑
            End If

            ' XYテーブル絶対値移動(ソフトリミットチェック有り)
            'V6.1.2.0①↓
            'r = ObjSys.XYtableMove(SysPrm, mvx + X, mvy + Y)
            r = ObjSys.XYtableMove(SysPrm, mvx + X, mvy + Y, giJogWaitMode)
            'V6.1.2.0①↑
            If (r <> cFRS_NORMAL) Then                              ' ｴﾗｰならｴﾗｰﾘﾀｰﾝ(メッセージ表示済み)
                If (ObjSys.IsSoftLimitXY(r) = False) Then
                    GoTo STP_END                                    ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                End If
                X = mvxBk                                           ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ時はX,Y位置を戻す
                Y = mvyBk
                GoTo STP_END                                        ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ
            End If
            'V4.0.0.0-80    '少数桁精度がおかしくなる対策
            mvx = mvx + 0.00000001
            mvy = mvy + 0.00000001
            mvx = mvx + X
            mvy = mvy + Y
            stJOG.cgX = stJOG.cgX + X                               ' テーブル移動量X,Y更新
            stJOG.cgY = stJOG.cgY + Y
            stJOG.cgX = stJOG.cgX + 0.00000001                               ' テーブル移動量X,Y更新
            stJOG.cgY = stJOG.cgY + 0.00000001
            'stJOG.cgX = stJOG.cgX + X                               ' テーブル移動量X,Y更新
            'stJOG.cgY = stJOG.cgY + Y
            'V4.0.0.0-80

STP_END:
            Return (r)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.Sub_XYtableMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

    '========================================================================================
    '   ボタン押下時処理(ＪＯＧ操作画面)
    '========================================================================================
#Region "HALTボタン押下時処理"
    '''=========================================================================
    '''<summary>HALTボタン押下時処理</summary>
    '''=========================================================================
    Public Sub SubBtnHALT_Click()
        InpKey = CONSOLE_SW_HALT
    End Sub
#End Region

#Region "STARTボタン押下時処理"
    '''=========================================================================
    '''<summary>STARTボタン押下時処理</summary>
    '''=========================================================================
    Public Sub SubBtnSTART_Click()
        InpKey = CONSOLE_SW_START
    End Sub
#End Region

#Region "RESETボタン押下時処理"
    '''=========================================================================
    '''<summary>RESETボタン押下時処理</summary>
    '''=========================================================================
    Public Sub SubBtnRESET_Click()
        InpKey = CONSOLE_SW_RESET
    End Sub
#End Region

#Region "Zボタン押下時処理"
    '''=========================================================================
    '''<summary>RESETボタン押下時処理</summary>
    '''<param name="stJOG">(INP)JOG操作用パラメータ</param>
    '''=========================================================================
    Public Sub SubBtnZ_Click(ByVal stJOG As JOG_PARAM)

        Dim strMSG As String

        Try
            If (stJOG.BtnZ.BackColor = System.Drawing.Color.Yellow) Then    ' Z SW ON ?
                stJOG.BtnZ.BackColor = System.Drawing.SystemColors.Control
                stJOG.BtnZ.Text = "Z Off"
                InpKey = InpKey And Not CONSOLE_SW_ZSW                      ' Z SW OFF
                bZ = False                                                  ' Zキー退避域
            Else
                stJOG.BtnZ.BackColor = System.Drawing.Color.Yellow
                stJOG.BtnZ.Text = "Z On"
                InpKey = InpKey Or CONSOLE_SW_ZSW                           ' Z SW ON
                bZ = True                                                   ' Zキー退避域
            End If

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.SubBtnZ_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "HIボタン押下時処理"
    '''=========================================================================
    '''<summary>HIボタン押下時処理</summary>
    '''<param name="stJOG">(INP)JOG操作用パラメータ</param>
    '''=========================================================================
    Public Sub SubBtnHI_Click(ByVal stJOG As JOG_PARAM)

        ' 背景色を切替える
        If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then   ' 背景色 = 黄色 ?
            ' 背景色をデフォルトにする
            stJOG.BtnHI.BackColor = System.Drawing.SystemColors.Control
            InpKey = InpKey And Not cBIT_HI                             ' HI SW OFF
        Else
            ' 背景色を黄色にする
            stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow
            InpKey = InpKey Or cBIT_HI                                  ' HI SW ON
        End If

    End Sub
#End Region

#Region "InpKeyを取得する"
    '''=========================================================================
    '''<summary>InpKeyを取得する</summary>
    '''<param name="IKey">(OUT)InpKey</param>
    '''=========================================================================
    Public Sub GetInpKey(ByRef IKey As UShort) '###057
        IKey = InpKey
    End Sub
#End Region

#Region "InpKeyを設定する"
    '''=========================================================================
    '''<summary>InpKeyを設定する</summary>
    '''<param name="IKey">(INP)InpKey</param>
    '''=========================================================================
    Public Sub PutInpKey(ByVal IKey As UShort) '###057
        InpKey = IKey
    End Sub
#End Region

#Region "カメラ画像表示PictureBoxクリック位置をJOG経由で画像センターに移動する"
    ''' <summary>カメラ画像表示PictureBoxクリック位置をJOG経由で画像センターに移動する</summary>
    ''' <param name="distanceX"></param>
    ''' <param name="distanceY"></param>
    ''' <param name="stJOG">'V6.0.0.0-23</param>
    ''' <remarks>'V6.0.0.0⑧</remarks>
    Friend Sub MoveToCenter(ByVal distanceX As Decimal, ByVal distanceY As Decimal, ByRef stJOG As JOG_PARAM)
        stJOG.KeyDown = Keys.Execute                                    'V6.0.0.0-23
        InpKey = (InpKey Or CtrlJog.MouseClickLocation.GetInpKey(distanceX, distanceY))
    End Sub
#End Region

#Region "矢印ボタン押下時"
    '''=========================================================================
    '''<summary>矢印ボタン押下時</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SubBtnJOG_0_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22  
        'V6.0.0.0-22    Public Sub SubBtnJOG_0_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H1000US                         ' +Y ON
    End Sub
    Public Sub SubBtnJOG_0_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_0_MouseUp()
        InpKey = InpKey And Not &H1000US                    ' +Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_1_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_1_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H800US                          ' -Y ON
    End Sub
    Public Sub SubBtnJOG_1_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_1_MouseUp()
        InpKey = InpKey And Not &H800US                     ' -Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_2_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_2_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H400US                          ' +X ON
    End Sub
    Public Sub SubBtnJOG_2_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_2_MouseUp()
        InpKey = InpKey And Not &H400US                     ' +X OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_3_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_3_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H200US                          ' -X ON
    End Sub
    Public Sub SubBtnJOG_3_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_3_MouseUp()
        InpKey = InpKey And Not &H200US                     ' -X OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_4_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_4_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &HA00US                          ' -X -Y ON
    End Sub
    Public Sub SubBtnJOG_4_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_4_MouseUp()
        InpKey = InpKey And Not &HA00US                     ' -X -Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_5_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_5_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &HC00US                          ' +X -Y ON
    End Sub
    Public Sub SubBtnJOG_5_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_5_MouseUp()
        InpKey = InpKey And Not &HC00US                     ' +X -Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_6_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_6_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H1400US                         ' +X +Y ON
    End Sub
    Public Sub SubBtnJOG_6_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_6_MouseUp()
        InpKey = InpKey And Not &H1400US                    ' +X +Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub

    Public Sub SubBtnJOG_7_MouseDown(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_7_MouseDown()
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        End If
        InpKey = InpKey Or &H1200US                         ' -X +Y ON
    End Sub
    Public Sub SubBtnJOG_7_MouseUp(ByRef stJOG As JOG_PARAM) 'V6.0.0.0-22
        'V6.0.0.0-22    Public Sub SubBtnJOG_7_MouseUp()
        InpKey = InpKey And Not &H1200US                    ' -X +Y OFF
        If (Keys.None <> stJOG.KeyDown) Then
            Sub_10KeyUp(stJOG.KeyDown, stJOG)
        Else
            stJOG.TenKey(0).Parent.Select()
        End If
    End Sub
#End Region

    '========================================================================================
    '   ＪＯＧ操作画面処理用トラックバー処理
    '========================================================================================
#Region "トラックバーのスライダー画面初期値表示"
    '''=========================================================================
    '''<summary>トラックバーのスライダー画面初期値表示</summary>
    '''<param name="TBarLowPitch">(I/O)スライダー1(Lowﾋﾟｯﾁ)</param>
    '''<param name="TBarHiPitch"> (I/O)スライダー2(HIGHﾋﾟｯﾁ)</param>
    '''<param name="TBarPause">   (I/O)スライダー3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)目盛1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)目盛2(Lowﾋﾟｯﾁ Label)</param>
    '''<param name="LblTchMoval2">(I/O)目盛3(HIGHﾋﾟｯﾁ Label)</param>
    '''<param name="dblTchMoval"> (I/O)ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time)</param>
    '''=========================================================================
    Public Sub XyzBpMovingPitchInit(ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                                    ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                                    ByRef TBarPause As System.Windows.Forms.TrackBar, _
                                    ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                                    ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                                    ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                                    ByRef dblTchMoval() As Double)

        Dim minval As Short

        ' LOWﾋﾟｯﾁがが範囲外なら範囲内に変更する
        If (dblTchMoval(IDX_PIT) < cPT_LO) Then dblTchMoval(IDX_PIT) = cPT_LO
        If (dblTchMoval(IDX_PIT) > cPT_HI) Then dblTchMoval(IDX_PIT) = cPT_HI

        ' LOWﾋﾟｯﾁの目盛を設定する
        If (dblTchMoval(IDX_PIT) < 0.002) Then                          ' 分解能により最小目盛を設定する
            minval = 1                                                  ' 目盛1～
        Else
            minval = 2                                                  ' 目盛2～
        End If

        TBarLowPitch.TickFrequency = dblTchMoval(IDX_PIT) * 1000        ' 0.001mm単位
        TBarLowPitch.Maximum = 100                                      ' 目盛1(or 2)～100(0.001m～0.1mm)
        TBarLowPitch.Minimum = minval
        '###110
        TBarLowPitch.Value = dblTchMoval(IDX_PIT) * 1000        ' 0.001mm単位

        ' HIGHﾋﾟｯﾁがが範囲外なら範囲内に変更する
        If (dblTchMoval(IDX_HPT) < cHPT_LO) Then dblTchMoval(IDX_HPT) = cHPT_LO
        If (dblTchMoval(IDX_HPT) > cHPT_HI) Then dblTchMoval(IDX_HPT) = cHPT_HI

        ' HIGHﾋﾟｯﾁの目盛を設定する
        TBarHiPitch.TickFrequency = dblTchMoval(IDX_HPT) * 100          ' 0.01mm単位
        TBarHiPitch.Maximum = 500                                       ' 目盛1～100(0.01m～5.00mm)
        TBarHiPitch.Minimum = 1
        '###110
        TBarHiPitch.Value = dblTchMoval(IDX_HPT) * 100          ' 0.01mm単位

        ' Pause Timeが範囲外なら範囲内に変更する
        If (dblTchMoval(IDX_PAU) < cPAU_LO) Then dblTchMoval(IDX_PAU) = cPAU_LO
        If (dblTchMoval(IDX_PAU) > cPAU_HI) Then dblTchMoval(IDX_PAU) = cPAU_HI

        ' Pause Timeの目盛を設定する
        TBarPause.TickFrequency = dblTchMoval(IDX_PAU) * 20             ' 0.5秒単位
        TBarPause.Maximum = 20                                          ' 目盛1～20(0.05秒～1.00秒)
        TBarPause.Minimum = 1
        '###110
        TBarPause.Value = dblTchMoval(IDX_PAU) * 20             ' 0.5秒単位

        ' 移動ピッチを表示する
        LblTchMoval0.Text = dblTchMoval(IDX_PIT).ToString("0.0000")
        LblTchMoval1.Text = dblTchMoval(IDX_HPT).ToString("0.0000")
        LblTchMoval2.Text = dblTchMoval(IDX_PAU).ToString("0.0000")

    End Sub
#End Region

#Region "トラックバーのスライダー移動処理"
    '''=========================================================================
    '''<summary>トラックバーのスライダー移動処理</summary>
    '''<param name="Index">       (INP)0=LOWﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause</param>
    '''<param name="TBarLowPitch">(I/O)スライダー1(Lowﾋﾟｯﾁ)</param>
    '''<param name="TBarHiPitch"> (I/O)スライダー2(HIGHﾋﾟｯﾁ)</param>
    '''<param name="TBarPause">   (I/O)スライダー3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)目盛1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)目盛2(Lowﾋﾟｯﾁ Label)</param>
    '''<param name="LblTchMoval2">(I/O)目盛3(HIGHﾋﾟｯﾁ Label)</param>
    '''<param name="dblTchMoval"> (I/O)ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time)</param>
    '''=========================================================================
    Public Sub SetSliderPitch(ByRef Index As Short, _
                              ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                              ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                              ByRef TBarPause As System.Windows.Forms.TrackBar, _
                              ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                              ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                              ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                              ByRef dblTchMoval() As Double)

        Dim lVal As Integer

        ' BPの移動ピッチ等を設定する
        Select Case Index
            Case IDX_PIT    ' LOWﾋﾟｯﾁ
                lVal = TBarLowPitch.Value                       ' ｽﾗｲﾀﾞ目盛値取得
                dblTchMoval(Index) = 0.001 * lVal               ' LOWﾋﾟｯﾁ値変更
                LblTchMoval0.Text = dblTchMoval(Index).ToString("0.0000")
                LblTchMoval0.Refresh()

            Case IDX_HPT    ' HIGHﾋﾟｯﾁ
                lVal = TBarHiPitch.Value                        ' ｽﾗｲﾀﾞ目盛値取得
                dblTchMoval(Index) = 0.01 * lVal                ' HIGHﾋﾟｯﾁ値変更
                LblTchMoval1.Text = dblTchMoval(Index).ToString("0.0000")
                LblTchMoval1.Refresh()

            Case IDX_PAU    ' Pause Time
                lVal = TBarPause.Value                          ' ｽﾗｲﾀﾞ目盛値取得
                dblTchMoval(Index) = 0.05 * lVal                ' 移動ピッチ間のポーズ値変更
                LblTchMoval2.Text = dblTchMoval(Index).ToString("0.0000")
                LblTchMoval2.Refresh()
        End Select

    End Sub
#End Region

    '========================================================================================
    '   ＪＯＧ操作画面処理用テンキー入力処理
    '========================================================================================
#Region "テンキーダウンサブルーチン"
    '''=========================================================================
    '''<summary>テンキーダウンサブルーチン</summary>
    ''' <param name="KeyCode">(INP)キーコード</param>
    '''=========================================================================
    Public Sub Sub_10KeyDown(ByVal KeyCode As Keys, ByRef stJOG As JOG_PARAM)  'V6.0.0.0⑫   'V6.0.0.0-22
        'V6.0.0.0⑪    Public Sub Sub_10KeyDown(ByVal KeyCode As Short)

        Dim strMSG As String
        Try
            With stJOG
                If (Keys.None <> .KeyDown) Then
                    Exit Sub 'V6.0.0.0-22
                Else
                    .KeyDown = KeyCode
                End If

                ' Num Lock版
                Select Case (KeyCode)
                    Case System.Windows.Forms.Keys.NumPad2                      ' ↓  (KeyCode =  98(&H62)
                        .TenKey(0).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &H1000                               ' +Y ON(↓)
                    Case System.Windows.Forms.Keys.NumPad8                      ' ↑  (KeyCode = 104(&H68)
                        .TenKey(1).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &H800                                ' -Y ON(↑)
                    Case System.Windows.Forms.Keys.NumPad4                      ' ←  (KeyCode = 100(&H64)
                        .TenKey(2).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &H400                                ' +X ON(←)
                    Case System.Windows.Forms.Keys.NumPad6                      ' →  (KeyCode = 102(&H66)
                        .TenKey(3).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &H200                                ' -X ON(→)
                    Case System.Windows.Forms.Keys.NumPad9                      ' PgUp(KeyCode = 105(&H69)
                        .TenKey(4).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &HA00                                ' -X -Y ON
                    Case System.Windows.Forms.Keys.NumPad7                      ' Home(KeyCode = 103(&H67))
                        .TenKey(5).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &HC00                                ' +X -Y ON
                    Case System.Windows.Forms.Keys.NumPad1                      ' End(KeyCode =   97(&H61)
                        .TenKey(6).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &H1400                               ' +X +Y ON
                    Case System.Windows.Forms.Keys.NumPad3                      ' PgDn(KeyCode =  99(&H63)
                        .TenKey(7).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        InpKey = InpKey Or &H1200                               ' -X +Y ON
                    Case System.Windows.Forms.Keys.NumPad5                      ' 5ｷｰ (KeyCode = 101(&H65)
                        .TenKey(8).FlatStyle = FlatStyle.Flat               'V6.0.0.0⑫
                        'Call BtnHI_Click(sender, e)                             ' HIボタン ON/OFF
                End Select
            End With

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.Sub_10KeyDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "テンキーアップサブルーチン"
    '''=========================================================================
    '''<summary>テンキーアップサブルーチン</summary>
    ''' <param name="KeyCode">(INP)キーコード</param>
    '''=========================================================================
    Public Sub Sub_10KeyUp(ByVal KeyCode As Keys, ByRef stJOG As JOG_PARAM)    'V6.0.0.0⑫   'V6.0.0.0-22
        'V6.0.0.0⑪    Public Sub Sub_10KeyUp(ByVal KeyCode As Short)

        Dim strMSG As String

        Try
            With stJOG
                'V6.0.0.0-23                If (KeyCode <> .KeyDown) Then
                'V6.0.0.0-23                    Exit Sub 'V6.0.0.0-22
                'V6.0.0.0-23                End If

                ' Num Lock版
                Select Case (KeyCode)
                    Case System.Windows.Forms.Keys.NumPad2                      ' ↓  (KeyCode =  98(&H62)
                        .TenKey(0).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &H1000                          ' +Y OFF
                    Case System.Windows.Forms.Keys.NumPad8                      ' ↑  (KeyCode = 104(&H68)
                        .TenKey(1).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &H800                           ' -Y OFF
                    Case System.Windows.Forms.Keys.NumPad4                      ' ←  (KeyCode = 100(&H64)
                        .TenKey(2).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &H400                           ' +X OFF
                    Case System.Windows.Forms.Keys.NumPad6                      ' →  (KeyCode = 102(&H66)
                        .TenKey(3).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &H200                           ' -X OFF
                    Case System.Windows.Forms.Keys.NumPad9                      ' PgUp(KeyCode = 105(&H69)
                        .TenKey(4).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &HA00                           ' -X -Y OFF
                    Case System.Windows.Forms.Keys.NumPad7                      ' Home(KeyCode = 103(&H67))
                        .TenKey(5).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &HC00                           ' +X -Y OFF
                    Case System.Windows.Forms.Keys.NumPad1                      ' End(KeyCode =   97(&H61)
                        .TenKey(6).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &H1400                          ' +X +Y OFF
                    Case System.Windows.Forms.Keys.NumPad3                      ' PgDn(KeyCode =  99(&H63)
                        .TenKey(7).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        InpKey = InpKey And Not &H1200                          ' -X +Y OFF
                    Case System.Windows.Forms.Keys.NumPad5                      ' 5ｷｰ (KeyCode = 101(&H65)
                        .TenKey(8).FlatStyle = FlatStyle.Standard               'V6.0.0.0⑫
                        'V6.0.1.0③      ↓
                    Case Keys.None
                        InpKey = (InpKey And cBIT_HI)
                        .ResetButtonStyle()
                        'V6.0.1.0③      ↑
                End Select

                .TenKey(0).Parent.Select()  'V6.0.0.0⑫
                .KeyDown = Keys.None        'V6.0.0.0-22

            End With
            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.Sub_10KeyUp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '===========================================================================
    '   グローバルメソッド定義
    '===========================================================================
#Region "機械系のパラメータ設定"
    '''=========================================================================
    '''<summary>機械系のパラメータ設定</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub SetMechanicalParam()

        Dim BpSoftLimitX As Integer
        Dim BpSoftLimitY As Integer

        With gSysPrm.stDEV
            ' 基板種別対応
            If gSysPrm.stCTM.giSPECIAL = customASAHI And typPlateInfo.strDataName = "2" Then
                .gfTrimX = .gfTrimX2                            ' TRIM POSITION X(mm)
                .gfTrimY = .gfTrimY2                            ' TRIM POSITION Y(mm)
                .gfExCmX = .gfExCmX2                            ' Externla Camera Offset X(mm)
                .gfExCmY = .gfExCmY2                            ' Externla Camera Offset Y(mm)
                .gfRot_X1 = .gfRot_X2                           ' 回転中心 X
                .gfRot_Y1 = .gfRot_Y2                           ' 回転中心 Y
                '(2010/11/16)下記処理は不要
                'Else
                '    gSysPrm.stDEV.gfTrimX = gSysPrm.stDEV.gfTrimX   ' TRIM POSITION X(mm)
                '    gSysPrm.stDEV.gfTrimY = gSysPrm.stDEV.gfTrimY   ' TRIM POSITION Y(mm)
                '    gSysPrm.stDEV.gfExCmX = gSysPrm.stDEV.gfExCmX   ' Externla Camera Offset X(mm)
                '    gSysPrm.stDEV.gfExCmY = gSysPrm.stDEV.gfExCmY   ' Externla Camera Offset Y(mm)
                '    gSysPrm.stDEV.gfRot_X1 = gSysPrm.stDEV.gfRot_X1 ' 回転中心 X
                '    gSysPrm.stDEV.gfRot_Y1 = gSysPrm.stDEV.gfRot_Y1 ' 回転中心 Y
            End If
            ''''(2010/11/16) 動作確認後下記コメントは削除
            'gStartX = gSysPrm.stDEV.gfTrimX
            'gStartY = gSysPrm.stDEV.gfTrimY

            'BpSizeからBPのソフトリミット（BPのソフト稼動範囲）を設定
            Select Case (.giBpSize)
                Case 0
                    BpSoftLimitX = 50
                    BpSoftLimitY = 50
                Case 1
                    BpSoftLimitX = 80
                    BpSoftLimitY = 80
                Case 2
                    BpSoftLimitX = 100
                    BpSoftLimitY = 60
                Case 3
                    BpSoftLimitX = 60
                    BpSoftLimitY = 100
                Case BPSIZE_6060 'V1.24.0.0①
                    'BpSoftLimitX = 60
                    'BpSoftLimitY = 60
                    BpSoftLimitX = 67
                    BpSoftLimitY = 67
                Case BPSIZE_90                  'V4.4.0.0①
                    BpSoftLimitX = BPSIZE_90
                    BpSoftLimitY = BPSIZE_90
                Case Else
                    BpSoftLimitX = 80
                    BpSoftLimitY = 80
            End Select

            '''''2009/07/23 minato
            ''''    トリムポジションが変更されているため、
            ''''    INTRTM側のシステムパラメータを更新する必要がある。
            Call ZSYSPARAM2(.giPrbTyp, .gfSminMaxZ2, .giZPTimeOn, .giZPTimeOff, _
                        .giXYtbl, .gfSmaxX, .gfSmaxY, gSysPrm.stIOC.glAbsTime, _
                        .gfTrimX, .gfTrimY, BpSoftLimitX, BpSoftLimitY)

            '----- V1.23.0.0⑪↓ -----
            ' 第二原点位置を再設定する
            Call ZSETPOS2(gSysPrm.stDEV.gfPos2X, gSysPrm.stDEV.gfPos2Y, gSysPrm.stDEV.gfPos2Z)
            '----- V1.23.0.0⑪↑ -----
        End With
    End Sub
#End Region

#Region "Uｶｯﾄ実行結果取得"
    '''=========================================================================
    '''<summary>Uｶｯﾄ実行結果取得</summary>
    '''<param name="rn">(INP) 抵抗番号</param>
    '''<param name="s"> (OUT) 実行結果</param>
    '''<returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function RetrieveUCutResult(ByVal rn As Short, ByRef s As String) As Short

        Dim cn As Short
        Dim n As Short
        Dim f As Double
        Dim r As Integer

        s = ""
        RetrieveUCutResult = 0

        If gSysPrm.stSPF.giUCutKind = 0 Then
            Exit Function
        End If

        On Error GoTo ErrTrap

        For cn = 1 To typResistorInfoArray(rn).intCutCount
            s = typResistorInfoArray(rn).ArrCut(cn).strCutType          ' Cut pattern
            If (s = "H") Or (s = "I") Then                              ' UCUT or UCUT(リトレース) V1.22.0.0①
                s = ""
                '  Uｶｯﾄ実行結果取得
                r = UCUT_RESULT(rn, cn, n, f)
                If (r <> 0) Then
                    MsgBox("Internal error  X001-" & Str(r), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
                    RetrieveUCutResult = 1
                    Exit Function
                End If

                If n = 255 Then                                         ' 255 はUCUT実行していない場合
                    's = Form1.Utility1.sFormat(f, "0.000000", 10 + 7) & " n** "    ' V1.22.0.0①
                    's = " n** " + Form1.Utility1.sFormat(f, "0.000000", 10 + 7)     ' V1.22.0.0①
                    s = " n** " + f.ToString("0.000000").PadLeft(10 + 7)     ' V4.0.0.0⑧
                ElseIf n >= 0 And n <= 19 Then                          ' 正常時(n=0-19) 
                    n = n + 1
                    ' 初期実測値(0.000000) + Uカットテーブル番号
                    's = Form1.Utility1.sFormat(f, "0.000000", 10 + 7) & " " & "n" & n.ToString("00") & " " ' V1.22.0.0①
                    's = "n" & n.ToString("00") & " " + Form1.Utility1.sFormat(f, "0.000000", 10 + 7)        ' V1.22.0.0①
                    s = "n" & n.ToString("00") & " " + f.ToString("0.000000").PadLeft(10 + 7)   ' V4.0.0.0⑧
                ElseIf n = 254 Then                                     ' パラメータテーブルに該当する抵抗番号が無かった場合
                    's = Form1.Utility1.sFormat(f, "0.000000", 10 + 7) & " n** "    ' V1.22.0.0①
                    's = " n** " + Form1.Utility1.sFormat(f, "0.000000", 10 + 7)     ' V1.22.0.0①
                    s = " n** " + f.ToString("0.000000").PadLeft(10 + 7)    ' V4.0.0.0⑧
                Else                                                    ' 変な値
                    RetrieveUCutResult = 2
                    Exit Function
                End If
            Else
                s = ""
            End If
        Next

        Exit Function

ErrTrap:
        Resume ErrTrap1
ErrTrap1:
        Dim er As Integer
        er = Err.Number
        On Error GoTo 0
        MsgBox("Internal error X002-" & Str(er), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
    End Function
#End Region

#Region "ﾍﾞｰｽ抵抗番号よりﾚｼｵﾍﾞｰｽ抵抗番号を取得する"
    '''=========================================================================
    '''<summary>ﾍﾞｰｽ抵抗番号よりﾚｼｵﾍﾞｰｽ抵抗番号を取得する</summary>
    '''<param name="br">(INP) 抵抗番号</param>
    '''<returns>0以上=ﾚｼｵﾍﾞｰｽ抵抗番号, -1=なし</returns>
    '''=========================================================================
    Public Function GetRatio1BaseNum(ByVal br As Short) As Short

        Dim n As Short

        For n = 1 To gRegistorCnt
            ' ベース抵抗 ?
            If typResistorInfoArray(n).intResNo = br Then
                GetRatio1BaseNum = n
                Exit Function
            End If
        Next
        GetRatio1BaseNum = -1

    End Function
#End Region

#Region "グループ数,ブロック数,チップ数(抵抗数),チップサイズを取得する(ＴＸ/ＴＹティーチング用)"
    '''=========================================================================
    ''' <summary>グループ数,ブロック数,チップ数(抵抗数),チップサイズを取得する</summary>
    ''' <param name="AppMode">  (INP)モード</param>
    ''' <param name="Gn">       (OUT)グループ数</param>
    ''' <param name="RnBn">     (OUT)チップ数(ＴＸティーチング時)または
    '''                              ブロック数(ＴＹティーチング時)</param>
    ''' <param name="DblChipSz">(OUT)チップサイズ</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    '''=========================================================================
    Public Function GetChipNumAndSize(ByVal AppMode As Short, ByRef Gn As Short, ByRef RnBn As Short, ByRef DblChipSz As Double) As Short

        Dim ChipNum As Short                                        ' チップ数(抵抗数)
        Dim ChipSzX As Double                                       ' チップサイズX
        Dim ChipSzY As Double                                       ' チップサイズY
        Dim strMSG As String

        Try
            ' 前処理(CHIP/NET共通)
            ChipNum = typPlateInfo.intResistCntInGroup              ' チップ数(抵抗数) = 1グループ内(1サーキット内)抵抗数
            ChipSzX = typPlateInfo.dblChipSizeXDir                  ' チップサイズX,Y
            ChipSzY = typPlateInfo.dblChipSizeYDir

            ' プレートデータからグループ数, ブロック数, チップ数(抵抗数), チップサイズを取得する
            If (AppMode = APP_MODE_TX) Then
                '----- ＴＸティーチング時 -----
                ' チップ数(抵抗数)を返す
                RnBn = ChipNum                                      ' 1グループ内(1サーキット内)抵抗数をセット
                ' グループ数を返す
                Gn = typPlateInfo.intGroupCntInBlockXBp             ' ＢＰグループ数(サーキット数)をセット
                ' チップサイズを返す
                If (typPlateInfo.intResistDir = 0) Then             ' チップ並びはX方向 ?
                    DblChipSz = System.Math.Abs(ChipSzX)
                Else
                    DblChipSz = System.Math.Abs(ChipSzY)
                End If

            Else
                '----- ＴＹティーチング時 -----
                ' グループ数を返す
                Gn = typPlateInfo.intGroupCntInBlockYStage          ' ブロック内Stageグループ数をセット
                ' ブロック数とチップサイズを返す
                If (typPlateInfo.intResistDir = 0) Then             ' チップ並びはX方向 ?
                    RnBn = typPlateInfo.intBlockCntYDir             ' ブロック数Yをセット
                    DblChipSz = System.Math.Abs(ChipSzY)            ' チップサイズYをセット
                Else
                    RnBn = typPlateInfo.intBlockCntXDir             ' ブロック数Xをセット
                    DblChipSz = System.Math.Abs(ChipSzX)            ' チップサイズXをセット
                End If
            End If

            strMSG = "GetChipNumAndSize() Gn=" + Gn.ToString("0") + ", RnBn=" + RnBn.ToString("0") + ", ChipSZ=" + DblChipSz.ToString("0.00000")
            Console.WriteLine(strMSG)
            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.GetChipNumAndSize() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- V1.19.0.0-33↓ -----
#Region "有効な抵抗があるかどうかチェックする"
    '''=========================================================================
    ''' <summary>有効な抵抗数を取得する</summary>
    ''' <param name="AppMode">  (INP)モード</param>
    ''' <returns>cFRS_NORMAL=正常, cFRS_ERR_RST=有効な抵抗がない</returns>
    '''=========================================================================
    Public Function CheckValidRegistance(ByVal AppMode As Short) As Short

        Dim Rn As Short                                                 ' 抵抗数
        Dim strMSG As String

        Try
            ' 有効な抵抗があるかどうかチェックする
            For Rn = 1 To gRegistorCnt                                  ' 抵抗数分処理する
                If (AppMode = APP_MODE_PROBE) Then                      ' プローブコマンド ?
                    If (typResistorInfoArray(Rn).intResNo < 1000) Then  ' 抵抗番号(1～999)があれば
                        Return (cFRS_NORMAL)                            ' Return値 = 正常
                    End If
                Else                                                    ' ティーチングコマンド時
                    If (typResistorInfoArray(Rn).intResNo < 6000) Or _
                       ((typResistorInfoArray(Rn).intResNo >= 8000) And (typResistorInfoArray(Rn).intResNo <= 9999)) Then
                        Return (cFRS_NORMAL)                            ' Return値 = 正常
                    End If

                End If
            Next Rn

            Return (cFRS_ERR_RST)                                       ' Return値 = Cancel(有効な抵抗がない)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.CheckValidRegistance() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region
    '----- V1.19.0.0-33↑ -----
#Region "特注レシオ時のベース抵抗番号(配列の添字)を返す"
    '''=========================================================================
    '''<summary>特注レシオ時のベース抵抗番号(配列の添字)を返す</summary>
    '''<param name="rr">(INP) 抵抗番号</param> 
    '''<param name="br">(OUT) ベース抵抗番号(配列の添字)</param>  
    '''<remarks>日立殿向け特注レシオ機能(TKYより移植)</remarks>
    '''=========================================================================
    Public Sub GetRatio3Br(ByRef rr As Short, ByRef br As Short)

        Dim i As Short
        Dim wRn As Short
        Dim wGn As Short
        Dim wBr As Short
        Dim wBr2 As Short

        ' 特注レシオモード(3～9)でなければ通常のベース抵抗番号を返す
        wRn = typResistorInfoArray(rr).intResNo                         ' 抵抗番号
        wGn = typResistorInfoArray(rr).intTargetValType                 ' 目標値種別（0:絶対値, 1:レシオ、2：計算式, 3～9:ｸﾞﾙｰﾌﾟ番号）
        wBr = GetRatio1BaseNum(typResistorInfoArray(rr).intBaseResNo)   ' ベース抵抗番号(添字)
        wBr2 = -1
        If (wGn < 3) Or (wGn > 9) Then                                  ' 特注レシオモード(3～9)でない ? 
            GoTo STP_END
        End If

        ' 特注レシオなら相手ｸﾞﾙｰﾌﾟ番号を検索する
        For i = 1 To gRegistorCnt                                       ' 抵抗数分繰り返す
            If (wRn <> typResistorInfoArray(i).intResNo) Then           ' 抵抗番号=自分自身はSKIP
                If (wGn = typResistorInfoArray(i).intTargetValType) Then            ' 相手ｸﾞﾙｰﾌﾟ番号 ?
                    wBr2 = GetRatio1BaseNum(typResistorInfoArray(i).intBaseResNo)   ' ベース抵抗番号(添字)
                    Exit For
                End If
            End If
        Next i

        ' ベース抵抗のFT値の大きい方をベース抵抗番号とする
        If (wBr2 < 0) Then GoTo STP_END '                               ' 相手ｸﾞﾙｰﾌﾟ番号が見つからなかった ?
        If (gfFinalTest(wBr2) > gfFinalTest(wBr)) Then                  ' 相手のFT値が大きい ?
            wBr = wBr2
        End If

STP_END:
        'br = wBr                                                       ' ベース抵抗番号を返す
        br = wBr - 1                                                    ' ベース抵抗番号を返す ###244

    End Sub
#End Region

#Region "レシオ(計算式)時のベース抵抗番号(配列の添字)を返す"
    '''=========================================================================
    '''<summary>レシオ(計算式)時のベース抵抗番号から抵抗データの配列の添字を返す###123</summary>
    '''<param name="br">(INP)ベース抵抗番号(配列の添字)</param> 
    '''<param name="rr">(OUT)抵抗データの配列の添字(1 ORG)</param> 
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub GetRatio2Rn(ByVal br As Short, ByRef rr As Short)

        Dim Rn As Short

        ' ベース抵抗番号を検索する
        For Rn = 1 To gRegistorCnt                                      ' 抵抗数分繰り返す
            If (typResistorInfoArray(Rn).intBaseResNo = br) Then
                rr = Rn
                Exit Sub
            End If
        Next Rn

    End Sub
#End Region

#Region "Z/Z2移動(ON/OFF) "
    '''=========================================================================
    '''<summary>Z/Z2移動(ON/OFF) </summary>
    '''<param name="MD">  (INP)ﾓｰﾄﾞ(0 = OFF位置移動, 1 = ON位置移動)</param> 
    '''<param name="Z2ON">(INP)Z2 ON位置(OPTION)</param>  
    '''<remarks>0=正常, 0以外=エラー</remarks>
    '''=========================================================================
    Public Function Sub_Probe_OnOff(ByVal MD As Integer, Optional ByVal Z2ON As Double = 0.0#) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' Ｚプローブをオン位置へ移動
            Sub_Probe_OnOff = cFRS_NORMAL                       ' Return値 = 正常
            If (MD = 1) Then                                    ' ON ?
                r = Form1.System1.EX_PROBON(gSysPrm)                   ' Z ON位置へ移動
                If (r <> cFRS_NORMAL) Then                      ' ｴﾗｰ ?
                    Sub_Probe_OnOff = r                         ' Return値 = 非常停止他(※ﾒｯｾｰｼﾞは表示済)
                    Exit Function
                End If
                If ((gSysPrm.stDEV.giPrbTyp And 2) = 2) Then    ' 下方ﾌﾟﾛｰﾌﾞなしならNOP
                    r = Form1.System1.EX_PROBON2(gSysPrm, Z2ON)        ' Z2 ON位置へ移動
                    If (r <> cFRS_NORMAL) Then                  ' ｴﾗｰ ?
                        Sub_Probe_OnOff = r                     ' Return値 = 非常停止他(※ﾒｯｾｰｼﾞは表示済)
                        Exit Function
                    End If
                End If

                ' Ｚプローブをオフ位置へ移動
            Else
                If ((gSysPrm.stDEV.giPrbTyp And 2) = 2) Then    ' 下方ﾌﾟﾛｰﾌﾞなしならNOP
                    r = Form1.System1.EX_PROBOFF2(gSysPrm)             ' Z2 OFF位置へ移動
                    If (r <> cFRS_NORMAL) Then                  ' ｴﾗｰ ?
                        Sub_Probe_OnOff = r                     ' Return値 = 非常停止他(※ﾒｯｾｰｼﾞは表示済)
                        Exit Function
                    End If
                End If
                r = Form1.System1.EX_PROBOFF(gSysPrm)                  ' Z OFF位置へ移動
                If (r <> cFRS_NORMAL) Then                      ' ｴﾗｰ ?
                    Sub_Probe_OnOff = r                         ' Return値 = 非常停止他(※ﾒｯｾｰｼﾞは表示済)
                    Exit Function
                End If
            End If
            Exit Function

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.Sub_Probe_OnOff() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                  ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "加工条件を入力する(FL時)"
    '''=========================================================================
    '''<summary>加工条件を入力する(FL時)</summary>
    ''' <param name="CondNum">(I/O)加工条件番号</param>
    ''' <param name="dQrate"> (I/O)Qレート(KHz)</param>
    ''' <param name="Owner">  (INP)オーナー</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>キャリブレーション、カット位置補正(外部カメラ)の十字カット用</remarks>
    '''=========================================================================
    Public Function Sub_FlCond(ByRef CondNum As Integer, ByRef dQrate As Double, ByVal Owner As IWin32Window) As Integer

        Dim r As Integer
        'V6.0.0.0⑱        Dim ObjForm As Object = Nothing
        Dim strMSG As String

        Try
            ' 加工条件を入力する(FL時)
            r = cFRS_NORMAL                                             ' Return値 = 正常
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then          ' FLでない時
                CondNum = 0                                             ' 加工条件番号(Dmy)

            Else                                                        ' FL時は加工条件を入力する
                ' 加工条件入力画面表示
                'V6.0.0.0⑱                ObjForm = New FrmFlCond()                               ' オブジェクト生成
                Dim ObjForm As FrmFlCond = New FrmFlCond()              ' オブジェクト生成  'V6.0.0.0⑱
                Call ObjForm.ShowDialog(Owner, CondNum)                 ' 加工条件入力画面表示
                r = ObjForm.GetResult(CondNum, dQrate)                  ' 加工条件取得

                ' オブジェクト開放
                If (ObjForm Is Nothing = False) Then
                    Call ObjForm.Close()                                ' オブジェクト開放
                    Call ObjForm.Dispose()                              ' リソース開放
                End If
            End If

            Return (r)                                                  ' Return値設定

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.Sub_FlCond() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region
    '----- V4.0.0.0-58↓ -----
#Region "加工条件番号を設定しなおす(SL436S時)"
    '''=========================================================================
    ''' <summary>加工条件番号を設定しなおす(SL436S時)</summary>
    ''' <remarks></remarks>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          上記以外 　　= エラー</returns> 
    '''=========================================================================
    Public Function SetCndNum() As Integer

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
            Call Init_wkCND()                                           ' 加工条件構造体(ワーク用)を初期化する

            '------------------------------------------------------------------
            '   カットデータの加工条件番号を設定しなおす
            '------------------------------------------------------------------
            For Rn = 1 To typPlateInfo.intResistCntInBlock              ' １ブロック内抵抗数分チェックする 
                For Cn = 1 To typResistorInfoArray(Rn).intCutCount      ' 抵抗内カット数分チェックする

                    ' カットタイプ取得
                    strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

                    ' 加工条件1を加工条件構造体(ワーク用)に登録して加工条件番号を取得する
                    CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1), _
                                       typResistorInfoArray(Rn).ArrCut(Cn).dblQRate, _
                                       typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1), _
                                       typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1))
                    If (CndNum >= 0) Then
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1) = CndNum
                    End If

                    ' 加工条件2はLカット, 斜めLカット, Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)
                    ' HOOKカット, Uカット時に設定する
                    If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
                       (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
                       (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                        ' 加工条件2
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2) = CndNum
                    End If

                    ' 加工条件3はHOOKカット, Uカット時に設定する(未対応)
                    If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                        ' 加工条件3
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L3), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L3), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L3))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3) = CndNum
                    End If

                    ' 加工条件4は現状は未使用(予備)

                    ' 加工条件5～8はリターン/リトレース用 
                    ' 加工条件5(STカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めSTカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                    If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
                       (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                        ' 加工条件5
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum
                    End If

                    ' 加工条件5,6(Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
                    If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
                       (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                        ' 加工条件5
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                                     typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                                     typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum

                        ' 加工条件6
                        CndNum = Add_stCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2_RET), _
                                 typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4, _
                                 typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2_RET), _
                                 typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2_RET))
                        If (CndNum < 0) Then GoTo STP_ERR
                        typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET) = CndNum
                    End If

                    ' 加工条件7,8は現状は未使用(予備)

                Next Cn
            Next Rn

            Return (cFRS_NORMAL)                                        ' Return値設定 

STP_ERR:
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    strMsg = "加工条件の登録件数が最大件数を超えました。電流値、周波数、STEG本数を確認して下さい。"
            'Else
            '    strMsg = "The Registration Number Of Conditions Exceeded The Maximum. Please Check A Current Value, Frequency, And A Number Of STEG."
            'End If
            strMsg = Globals_Renamed_001
            MsgBox(strMsg)
            Return (cFRS_NORMAL)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "basTrimming.SetCndNum() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try
    End Function
#End Region

#Region "加工条件構造体(ワーク用)を初期化する(SL436S用)"
    '''=========================================================================
    '''<summary>加工条件構造体(ワーク用)を初期化する(SL436S用)</summary>
    '''=========================================================================
    Public Sub Init_wkCND()

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' FLでないまたはSL436SでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return
            If (giMachineKd <> MACHINE_KD_RS) Then Return

            ' 加工条件番号が登録済みかチェックする
            gwkCndCount = 0                                             ' 加工条件登録数 
            For Idx = 0 To (MAX_BANK_NUM - 1)
                gwkCND.Curr(Idx) = 0
                gwkCND.Freq(Idx) = 0
                gwkCND.Steg(Idx) = 0
                gwkPower(Idx) = 0
            Next Idx

            Return

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.Init_wkCND() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "加工条件を加工条件構造体(ワーク用)に登録する(SL436S用)"
    '''=========================================================================
    '''<summary>加工条件を加工条件構造体(ワーク用)に登録する(SL436S用)</summary>
    ''' <param name="Curr"> (INP)電流値(mA)</param>
    ''' <param name="Freq"> (INP)周波数(KHz)</param>
    ''' <param name="Steg"> (INP)STEG本数</param>
    ''' <param name="Power">(INP)目標パワー</param>
    ''' <returns>0-29=加工条件番号, 左記以外=エラー</returns>
    '''=========================================================================
    Public Function Add_stCND(ByVal Curr As Integer, ByVal Freq As Double, ByVal Steg As Integer, ByVal Power As Double) As Integer

        Dim Idx As Integer
        Dim Count As Integer
        Dim strMSG As String

        Try
            ' FLでないまたはSL436SでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)

            ' 加工条件番号が登録済みかチェックする
            Count = gwkCndCount                                         ' 加工条件登録数 
            For Idx = 0 To (Count - 1)
                'V4.9.0.1②↓
                If typPlateInfo.intPowerAdjustMode = 0 Then     ' パワー測定なしの場合
                    ' 電流値, 周波数, STEG本数が等しい加工条件は登録済みか ?
                    If ((gwkCND.Curr(Idx) = Curr) And _
                        (gwkCND.Freq(Idx) = Freq) And _
                        (gwkCND.Steg(Idx) = Steg)) Then
                        Return (Idx)                                        ' 登録済み加工条件番号を返す
                    End If
                Else
                    ' パワー測定ありの場合
                    ' 周波数, STEG本数,目標パワーが等しい加工条件は登録済みか ?
                    If ((gwkCND.Freq(Idx) = Freq) And _
                        (gwkCND.Steg(Idx) = Steg) And _
                        (gwkPower(Idx) = Power)) Then
                        Return (Idx)                                        ' 登録済み加工条件番号を返す
                    End If
                End If
                'V4.9.0.1②↑
            Next Idx

            ' 加工条件登録可能かチェックする(加工条件番号30,31は予約済み)
            If ((gwkCndCount + 1) > (MAX_BANK_NUM - 2)) Then
                Return (-1)
            End If

            ' 加工条件を登録する
            Idx = gwkCndCount
            gwkCND.Curr(Idx) = Curr                                     ' 電流値(mA)
            gwkCND.Freq(Idx) = Freq                                     ' 周波数(KHz)
            gwkCND.Steg(Idx) = Steg                                     ' STEG本数
            gwkPower(Idx) = Power                                       ' 目標パワー

            gwkCndCount = Idx + 1                                       ' 加工条件登録数更新
            Return (Idx)                                                ' 登録した加工条件番号を返す

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.Add_stCND() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "Qレート,電流値,STEG本数から加工条件番号をカットデータに設定する(SL436S時)"
    '''=========================================================================
    ''' <summary>Qレート,電流値,STEG本数から加工条件番号をカットデータに設定する</summary>
    ''' <param name="Rn">(INP)抵抗番号</param>
    ''' <param name="Cn">(INP)カット番号</param>
    ''' <remarks></remarks>
    ''' <returns>cFRS_NORMAL  = 正常
    '''          上記以外 　　= エラー</returns> 
    '''=========================================================================
    Private Function Put_CutCndNum(ByVal Rn As Integer, ByVal Cn As Integer) As Integer

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
            '   トリミングデータの加工条件番号を設定しなおす
            '------------------------------------------------------------------
            ' カットタイプ取得
            strCutType = typResistorInfoArray(Rn).ArrCut(Cn).strCutType.Trim()

            ' 加工条件1は全カット無条件に設定する
            CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1), _
                                         typResistorInfoArray(Rn).ArrCut(Cn).dblQRate, _
                                         typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1), _
                                         typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1))
            If (CndNum >= 0) Then
                typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1) = CndNum
            End If

            ' 加工条件2はLカット, 斜めLカット, Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)
            ' HOOKカット, Uカット時に設定する
            If (strCutType = CNS_CUTP_L) Or (strCutType = CNS_CUTP_NL) Or _
               (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
               (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Or _
               (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                ' 加工条件2
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate2, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2) = CndNum
                End If
            End If

            ' 加工条件3はHOOKカット, Uカット時に設定する(未対応)
            If (strCutType = CNS_CUTP_HK) Or (strCutType = CNS_CUTP_U) Or (strCutType = CNS_CUTP_Ut) Then
                ' 加工条件3
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L3), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L3), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L3))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L3) = CndNum
                End If
            End If

            ' 加工条件4は現状は未使用(予備)

            ' 加工条件5～8はリターン/リトレース用 
            ' 加工条件5(STカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めSTカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
            If (strCutType = CNS_CUTP_STr) Or (strCutType = CNS_CUTP_STt) Or _
               (strCutType = CNS_CUTP_NSTr) Or (strCutType = CNS_CUTP_NSTt) Then
                ' 加工条件5
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum
                End If
            End If

            ' 加工条件5,6(Lカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ), 斜めLカット(ﾘﾀｰﾝ/ﾘﾄﾚｰｽ)時
            If (strCutType = CNS_CUTP_Lr) Or (strCutType = CNS_CUTP_Lt) Or _
               (strCutType = CNS_CUTP_NLr) Or (strCutType = CNS_CUTP_NLt) Then
                ' 加工条件5
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate3, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L1_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L1_RET))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L1_RET) = CndNum
                End If

                ' 加工条件6
                CndNum = Get_CndNumFromWkCND(typResistorInfoArray(Rn).ArrCut(Cn).FLCurrent(CUT_CND_L2_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblQRate4, _
                             typResistorInfoArray(Rn).ArrCut(Cn).FLSteg(CUT_CND_L2_RET), _
                             typResistorInfoArray(Rn).ArrCut(Cn).dblPowerAdjustTarget(CUT_CND_L2_RET))
                If (CndNum >= 0) Then
                    typResistorInfoArray(Rn).ArrCut(Cn).CndNum(CUT_CND_L2_RET) = CndNum
                End If
            End If

            ' 加工条件7,8は現状は未使用(予備)

            Return (cFRS_NORMAL)                                        ' Return値設定 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMsg = "globals.SetAutoPowerCurrData() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cERR_TRAP)                                          ' Return値 = トラップエラー発生
        End Try
    End Function
#End Region

#Region "Qレート,電流値,STEG本数から加工条件番号を取得する(SL436S用)"
    '''=========================================================================
    '''<summary>Qレート,電流値,STEG本数から加工条件番号を取得する(SL436S用)</summary>
    ''' <param name="Curr"> (INP)電流値(mA)</param>
    ''' <param name="Freq"> (INP)周波数(KHz)</param>
    ''' <param name="Steg"> (INP)STEG本数</param>
    ''' <param name="Power">(INP)目標パワー</param>
    ''' <returns>0-29=加工条件番号(※加工条件番号30と31は予約済み), 左記以外=エラー</returns>
    '''=========================================================================
    Public Function Get_CndNumFromWkCND(ByVal Curr As Integer, ByVal Freq As Double, ByVal Steg As Integer, ByVal Power As Double) As Integer

        Dim Idx As Integer
        Dim Count As Integer
        Dim strMSG As String

        Try
            ' FLでないまたはSL436SでなければNOP
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then Return (cFRS_NORMAL)
            If (giMachineKd <> MACHINE_KD_RS) Then Return (cFRS_NORMAL)

            ' 加工条件番号が登録済みかチェックする
            Count = gwkCndCount                                         ' 加工条件登録数 
            For Idx = 0 To (Count - 1)
                ' 電流値, 周波数, STEG本数が等しい加工条件は登録済みか ?
                If ((gwkCND.Curr(Idx) = Curr) And _
                    (gwkCND.Freq(Idx) = Freq) And _
                    (gwkCND.Steg(Idx) = Steg) And _
                    (gwkPower(Idx) = Power)) Then
                    Return (Idx)                                        ' 登録済み加工条件番号を返す
                End If
            Next Idx

            Return (Idx)                                                ' 登録した加工条件番号を返す

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.Get_CndNumFromWkCND() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region
    '----- V4.0.0.0-58↑ -----
#Region "十字カットを行う"
    '''=========================================================================
    '''<summary>十字カットを行う</summary>
    ''' <param name="BPx">         (INP)カット位置X</param>
    ''' <param name="BPy">         (INP)カット位置Y</param>
    ''' <param name="CondNum">     (INP)加工条件番号(FL用)</param>
    ''' <param name="dQrate">      (INP)Qレート(KHz)</param>
    ''' <param name="dblCutLength">(INP)カット長</param>
    ''' <param name="dblCutSpeed"> (INP)カット速度</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>※十字ｶｯﾄの中央にBPを移動しておくこと
    '''            キャリブレーション、カット位置補正(外部カメラ)の十字カット用</remarks>
    '''=========================================================================
    Public Function CrossCutExec(ByVal BPx As Double, ByVal BPy As Double, ByVal CondNum As Integer, _
                                 ByVal dQrate As Double, ByVal dblCutLength As Double, ByVal dblCutSpeed As Double) As Integer

        Dim r As Integer
        Dim intXANG As Integer
        Dim intYANG As Integer
        Dim strMSG As String
        Dim stCutCmnPrm As CUT_COMMON_PRM                               ' カットパラメータ

        Try
            '-------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------
            Call InitCutParam(stCutCmnPrm)                              ' カットパラメータ初期化

            ' カット角度を設定する
            Select Case (gSysPrm.stDEV.giBpDirXy)
                Case 0      ' x←, y↓
                    intXANG = 180
                    intYANG = 270
                Case 1      ' x→, y↓
                    intXANG = 0
                    intYANG = 270
                Case 2      ' x←, y↑
                    intXANG = 180
                    intYANG = 90
                Case 3      ' x→, y↑
                    intXANG = 0
                    intYANG = 90
            End Select

            ' カットパラメータ(カット情報構造体)を設定する
            stCutCmnPrm.CutInfo.srtMoveMode = 2                         ' 動作モード（0:トリミング、1:ティーチング、2:強制カット）
            stCutCmnPrm.CutInfo.srtCutMode = 4                          ' カットモードは「斜め」
            stCutCmnPrm.CutInfo.dblTarget = 1000.0#                     ' 目標値 = 1とする
            stCutCmnPrm.CutInfo.srtSlope = 4                            ' 4:抵抗測定＋スロープ
            stCutCmnPrm.CutInfo.srtMeasType = 0                         ' 測定タイプ(0:高速(3回)、1:高精度(2000回)
            stCutCmnPrm.CutInfo.dblAngle = intXANG                      ' カット角度(X軸)

            ' カットパラメータ(加工設定構造体)を設定する
            stCutCmnPrm.CutCond.CutLen.dblL1 = dblCutLength             ' カット長(Line1用)
            stCutCmnPrm.CutCond.SpdOwd.dblL1 = dblCutSpeed              ' カットスピード（往路）(Line1用)
            stCutCmnPrm.CutCond.QRateOwd.dblL1 = dQrate                 ' カットQレート（往路）(Line1用)
            stCutCmnPrm.CutCond.CondOwd.srtL1 = CondNum                 ' カット条件番号（往路）(Line1用)

            ' Qレート(FL時以外)または加工条件番号(FL時)を設定する
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then          ' FLでない ?
                Call QRATE(dQrate)                                      ' Qレート設定(KHz)
            Else                                                        ' 加工条件番号を設定する(FL時)
                Call QRATE(dQrate)                                      ' Qレート設定(KHz)
                r = FLSET(FLMD_CNDSET, CondNum)                         ' 加工条件番号設定
                If (r <> cFRS_NORMAL) Then GoTo STP_ERR_FL
            End If

            '-------------------------------------------------------------------
            '   十字カットのX軸をカットする
            '-------------------------------------------------------------------
            ' BPをX軸始点へ移動する(絶対値移動)
            r = Form1.System1.EX_MOVE(gSysPrm, BPx - (dblCutLength / 2), BPy, 1)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            Call SendCrossLineMsgToDispGazou()    'V5.0.0.6⑫
            ' 十字カットのX軸をカットする
            r = Sub_CrossCut(stCutCmnPrm)                               ' X軸カット
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            ' BPを中心へ戻す
            r = Form1.System1.EX_MOVE(gSysPrm, BPx, BPy, 1)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            Call System.Threading.Thread.Sleep(500)                     ' Wait(msec)

            '-------------------------------------------------------------------
            '   十字カットのY軸をカットする
            '-------------------------------------------------------------------
            ' BPをY軸始点へ移動する(絶対値移動)
            r = Form1.System1.EX_MOVE(gSysPrm, BPx, BPy - (dblCutLength / 2), 1)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            ' 十字カットのY軸をカットする
            stCutCmnPrm.CutInfo.dblAngle = intYANG                      ' カット角度(Y軸)
            r = Sub_CrossCut(stCutCmnPrm)                               ' Y軸カット
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            ' BPを中心へ戻す
            r = Form1.System1.EX_MOVE(gSysPrm, BPx, BPy, 1)
            If (r <> cFRS_NORMAL) Then                                  ' エラー ?
                Return (r)
            End If
            Call System.Threading.Thread.Sleep(500)                     ' Wait(msec)

            Return (cFRS_NORMAL)

            ' 加工条件番号の設定エラー時(FL時)
STP_ERR_FL:
            strMSG = MSG_151                                            ' "加工条件の設定に失敗しました｡"
            Call Form1.System1.TrmMsgBox(gSysPrm, strMSG, vbOKOnly, gAppName)
            Return (cFRS_ERR_RST)                                       ' Return値 = Cancel

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.CrossCutExec() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "十字カットのX軸またはY軸をカットする"
    '''=========================================================================
    '''<summary>十字カットのX軸またはY軸をカットする</summary>
    ''' <param name="stCutCmnPrm">(INP)カットパラメータ</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>※十字カット位置にBPを移動しておくこと
    '''            キャリブレーション、カット位置補正(外部カメラ)の十字カット用</remarks>
    '''=========================================================================
    Private Function Sub_CrossCut(ByRef stCutCmnPrm As CUT_COMMON_PRM) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 十字カットのX軸またはY軸をカットする
            r = TRIM_ST(stCutCmnPrm)                                    ' STカット
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)
            If (r < cFRS_NORMAL) Then                                   ' エラー ?
                Return (r)
            End If
            Return (cFRS_NORMAL)

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.Sub_CrossCut() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region

#Region "カットパラメータを初期化する"
    '''=========================================================================
    '''<summary>カットパラメータを初期化する</summary>
    ''' <param name="pstCutCmnPrm">(I/O)カットパラメータ</param>
    ''' <remarks>キャリブレーション、カット位置補正(外部カメラ)の十字カット用</remarks>
    '''=========================================================================
    Private Sub InitCutParam(ByRef pstCutCmnPrm As CUT_COMMON_PRM)

        Dim strMSG As String

        Try
            ' カットパラメータを初期化する(カット情報構造体)
            pstCutCmnPrm.CutInfo.srtMoveMode = 1                        ' 動作モード（0:トリミング、1:ティーチング、2:強制カット）
            pstCutCmnPrm.CutInfo.srtCutMode = 0                         ' カットモード(0:ノーマル、1:リターン、2:リトレース、3:斜め）
            pstCutCmnPrm.CutInfo.dblTarget = 0.0#                       ' 目標値
            pstCutCmnPrm.CutInfo.srtSlope = 4                           ' 4:抵抗測定＋スロープ
            pstCutCmnPrm.CutInfo.srtMeasType = 0                        ' 測定タイプ(0:高速(3回)、1:高精度(2000回)
            pstCutCmnPrm.CutInfo.dblAngle = 0.0#                        ' カット角度
            pstCutCmnPrm.CutInfo.dblLTP = 0.0#                          ' Lターンポイント
            pstCutCmnPrm.CutInfo.srtLTDIR = 0                           ' Lターン後の方向
            pstCutCmnPrm.CutInfo.dblRADI = 0.0#                         ' R部回転半径（Uカットで使用）
            '                                                           ' For Hook Or UCut
            pstCutCmnPrm.CutInfo.dblRADI2 = 0.0#                        ' R2部回転半径（Uカットで使用）
            pstCutCmnPrm.CutInfo.srtHkOrUType = 0                       ' HookCut(3)かUカット（3以外）の指定。
            '                                                           ' For Index
            pstCutCmnPrm.CutInfo.srtIdxScnCnt = 0                       ' インデックス/スキャンカット数(1～32767)
            pstCutCmnPrm.CutInfo.srtIdxMeasMode = 0                     ' インデックス測定モード（0:抵抗、1:電圧、2:外部）
            '                                                           ' For EdgeSense
            pstCutCmnPrm.CutInfo.dblEsPoint = 0.0#                      ' エッジセンスポイント
            pstCutCmnPrm.CutInfo.dblRdrJdgVal = 0.0#                    ' ラダー内部判定変化量
            pstCutCmnPrm.CutInfo.dblMinJdgVal = 0.0#                    ' ラダーカット後最低許容変化量
            pstCutCmnPrm.CutInfo.srtEsAftCutCnt = 0                     ' ラダー切抜け後のカット回数（測定回数）
            pstCutCmnPrm.CutInfo.srtMinOvrNgCnt = 0                     ' ラダー抜出し後、最低変化量の連続Over許容数
            pstCutCmnPrm.CutInfo.srtMinOvrNgMode = 0                    ' 連続Over時のNG処理（0:NG判定未実施, 1:NG判定実施。ラダー中切り, 2:NG判定未実施。ラダー切上げ）
            '                                                           ' For Scan
            pstCutCmnPrm.CutInfo.dblStepPitch = 0.0#                    ' ステップ移動ピッチ
            pstCutCmnPrm.CutInfo.srtStepDir = 0                         ' ステップ方向

            ' カットパラメータを初期化する(加工設定構造体)
            pstCutCmnPrm.CutCond.CutLen.dblL1 = 0.0#                    ' カット長(Line1用)
            pstCutCmnPrm.CutCond.CutLen.dblL2 = 0.0#                    ' カット長(Line2用)
            pstCutCmnPrm.CutCond.CutLen.dblL3 = 0.0#                    ' カット長(Line3用)
            pstCutCmnPrm.CutCond.CutLen.dblL4 = 0.0#                    ' カット長(Line4用)

            pstCutCmnPrm.CutCond.SpdOwd.dblL1 = 0.0#                    ' カットスピード（往路）(Line1用)
            pstCutCmnPrm.CutCond.SpdOwd.dblL2 = 0.0#                    ' カットスピード（往路）(Line2用)
            pstCutCmnPrm.CutCond.SpdOwd.dblL3 = 0.0#                    ' カットスピード（往路）(Line3用)
            pstCutCmnPrm.CutCond.SpdOwd.dblL4 = 0.0#                    ' カットスピード（往路）(Line4用)

            pstCutCmnPrm.CutCond.SpdRet.dblL1 = 0.0#                    ' カットスピード（復路）(Line1用)
            pstCutCmnPrm.CutCond.SpdRet.dblL2 = 0.0#                    ' カットスピード（復路）(Line2用)
            pstCutCmnPrm.CutCond.SpdRet.dblL3 = 0.0#                    ' カットスピード（復路）(Line3用)
            pstCutCmnPrm.CutCond.SpdRet.dblL4 = 0.0#                    ' カットスピード（復路）(Line4用)

            pstCutCmnPrm.CutCond.QRateOwd.dblL1 = 0.0#                  ' カットQレート（往路）(Line1用)
            pstCutCmnPrm.CutCond.QRateOwd.dblL2 = 0.0#                  ' カットQレート（往路）(Line2用)
            pstCutCmnPrm.CutCond.QRateOwd.dblL3 = 0.0#                  ' カットQレート（往路）(Line3用)
            pstCutCmnPrm.CutCond.QRateOwd.dblL4 = 0.0#                  ' カットQレート（往路）(Line4用)

            pstCutCmnPrm.CutCond.QRateRet.dblL1 = 0.0#                  ' カットQレート（復路）(Line1用)
            pstCutCmnPrm.CutCond.QRateRet.dblL2 = 0.0#                  ' カットQレート（復路）(Line2用)
            pstCutCmnPrm.CutCond.QRateRet.dblL3 = 0.0#                  ' カットQレート（復路）(Line3用)
            pstCutCmnPrm.CutCond.QRateRet.dblL4 = 0.0#                  ' カットQレート（復路）(Line4用)

            pstCutCmnPrm.CutCond.CondOwd.srtL1 = 0                      ' カット条件番号（往路）(Line1用)
            pstCutCmnPrm.CutCond.CondOwd.srtL2 = 0                      ' カット条件番号（往路）(Line2用)
            pstCutCmnPrm.CutCond.CondOwd.srtL3 = 0                      ' カット条件番号（往路）(Line3用)
            pstCutCmnPrm.CutCond.CondOwd.srtL4 = 0                      ' カット条件番号（往路）(Line4用)

            pstCutCmnPrm.CutCond.CondRet.srtL1 = 0                      ' カット条件番号（復路）(Line1用)
            pstCutCmnPrm.CutCond.CondRet.srtL2 = 0                      ' カット条件番号（復路）(Line2用)
            pstCutCmnPrm.CutCond.CondRet.srtL3 = 0                      ' カット条件番号（復路）(Line3用)
            pstCutCmnPrm.CutCond.CondRet.srtL4 = 0                      ' カット条件番号（復路）(Line4用)

            Exit Sub

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.InitCutParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Exit Sub
    End Sub
#End Region

#Region "パターン認識を実行し、ずれ量を返す"
    '''=========================================================================
    ''' <summary>パターン認識を実行し、ずれ量を返す</summary>
    ''' <param name="GrpNo">    (INP)グループ番号</param>
    ''' <param name="TmpNo">    (INP)パターン番号</param>
    ''' <param name="fCorrectX">(OUT)ずれ量X</param> 
    ''' <param name="fCorrectY">(OUT)ずれ量Y</param>
    ''' <param name="coef">     (OUT)相関係数</param> 
    ''' <returns>cFRS_NORMAL  = 正常
    '''          cFRS_ERR_PTN = パターンパターン認識エラー
    '''          上記以外     = その他エラー</returns>
    ''' <remarks>・パターン認識位置へテーブルは移動済であること
    '''          ・キャリブレーション、カット位置補正(外部カメラ)用
    ''' </remarks>
    '''=========================================================================
    Public Function Sub_PatternMatching(ByRef GrpNo As Short, ByRef TmpNo As Short, ByRef fCorrectX As Double, ByRef fCorrectY As Double, ByRef coef As Double) As Integer

        Dim ret As Short = cFRS_NORMAL
        Dim crx As Double = 0.0                                         ' ずれ量X
        Dim cry As Double = 0.0                                         ' ずれ量Y
        Dim fcoeff As Double = 0.0                                      ' 相関値
        Dim Thresh As Double = 0.0                                      ' 閾値
        Dim r As Integer = cFRS_NORMAL                                  ' 関数値
        Dim strMSG As String = ""

        Try
#If VIDEO_CAPTURE = 1 Then
            fCorrectX = 0.0
            fCorrectY = 0.0
            coef = 0.8
            Return (cFRS_NORMAL)   
#End If
            ' パターンマッチング時のテンプレートグループ番号を設定する(毎回やると遅くなる)
            'V5.0.0.6③            If (giTempGrpNo <> GrpNo) Then                              ' テンプレートグループ番号が変わった ?
            giTempGrpNo = GrpNo                                     ' 現在のテンプレートグループ番号を退避
            Form1.VideoLibrary1.SelectTemplateGroup(GrpNo)          ' テンプレートグループ番号設定
            'V5.0.0.6③            End If

            ' 閾値取得
            Thresh = gDllSysprmSysParam_definst.GetPtnMatchThresh(GrpNo, TmpNo)
            coef = 0.0                                                  ' 一致度

            ' パターンマッチング(外部カメラ)を行う(Video.ocxを使用)
            ret = Form1.VideoLibrary1.PatternMatching_EX(TmpNo, 1, True, crx, cry, fcoeff)
            If (ret = cFRS_NORMAL) Then
                r = cFRS_NORMAL                                         ' Return値 = 正常
                fCorrectX = crx                                         ' ずれ量X
                fCorrectY = cry                                         ' ずれ量Y
                '' マッチしたパターンの測定位置からずれ量を求める
                'fCorrectX = crx / 1000.0#
                'fCorrectY = -cry / 1000.0#
                coef = fcoeff                                           ' 相関係数
                strMSG = Globals_Renamed_002 ' パターン認識成功
                If (fcoeff < Thresh) Then
                    r = cFRS_ERR_PT2                                    ' パターン認識エラー(閾値エラー)
                    strMSG = Globals_Renamed_003 ' パターン認識エラー(閾値エラー)
                End If
                'strMSG = strMSG + " (" & "相関係数" & "=" & fcoeff.ToString("0.000") & " " & "ずれ量X" & "=" & crx.ToString("0.0000") & ", " & "ずれ量Y" & "=" & cry.ToString("0.0000") + ")"
                strMSG = strMSG + " (" & Globals_Renamed_004 & "=" & fcoeff.ToString("0.000") & " " & Globals_Renamed_005 & "=" & crx.ToString("0.0000") & ", " & Globals_Renamed_006 & "=" & cry.ToString("0.0000") + ")"
            Else
                r = cFRS_ERR_PTN                                        ' パターン認識エラー(パターンが見つからなかった)
                strMSG = Globals_Renamed_007 ' パターン認識エラー(パターンが見つからなかった)
            End If

            ' 後処理
            Console.WriteLine(strMSG)
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "globals.Sub_PatternMatching() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Return値 = 例外エラー
        End Try
    End Function
#End Region

#Region "指定ブロックの中央へテーブルを移動する"
    '''=========================================================================
    '''<summary>指定ブロックの中央へテーブルを移動する</summary>
    '''<param name="intCamera">(INP)ｶﾒﾗ種類(0:内部ｶﾒﾗ 1:外部ｶﾒﾗ)</param>
    '''<param name="iXPlate">(INP)XPlateNo</param> 
    '''<param name="iYPlate">(INP)YPlateNo</param>  
    '''<param name="iXBlock">(INP)XBlockNo</param> 
    '''<param name="iYBlock">(INP)YBlockNo</param>   
    '''<remarks>十字ｶｯﾄ位置はﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄからﾌﾟﾚｰﾄﾃﾞｰﾀ
    '''         のPP47の値分ずれたところが中心となる
    '''         現状はﾌﾟﾚｰﾄを指定しても意味なし</remarks>
    '''=========================================================================
    Public Function XYTableMoveBlock(ByRef intCamera As Short, ByRef iXPlate As Short, ByRef iYPlate As Short, ByRef iXBlock As Short, ByRef iYBlock As Short) As Short

        Dim dblX As Double
        Dim dblY As Double
        Dim dblRotX As Double
        Dim dblRotY As Double
        Dim dblPSX As Double
        Dim dblPSY As Double
        Dim dblBsoX As Double
        Dim dblBsoY As Double
        Dim dblBSX As Double
        Dim dblBSY As Double
        Dim intCDir As Short
        Dim dblTrimPosX As Double
        Dim dblTrimPosY As Double
        Dim dblTOffsX As Double
        Dim dblTOffsY As Double
        Dim dblStepInterval As Double
        Dim Del_x As Double
        Dim Del_y As Double
        Dim r As Short
        Dim strMSG As String

        Try
            dblRotX = 0
            dblRotY = 0

            ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                 ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
            dblTrimPosY = gSysPrm.stDEV.gfTrimY
            ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄの取得
            dblTOffsX = typPlateInfo.dblTableOffsetXDir : dblTOffsY = typPlateInfo.dblTableOffsetYDir

            Call CalcBlockSize(dblBSX, dblBSY)                  ' ﾌﾞﾛｯｸｻｲｽﾞ算出

            ' ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ算出　ﾌﾞﾛｯｸｻｲｽﾞ/2 ﾌﾞﾛｯｸの象限はXYともに1 ﾃｰﾌﾞﾙの象限も1
            dblBsoX = (dblBSX / 2.0#) * 1 * 1                   ' Table.BDirX * Table.dir
            dblBsoY = (dblBSY / 2) * 1                          ' Table.BDirY;

            ' θ補正ﾄﾘﾑｵﾌｾｯﾄX,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            ' giBpDirXy 座標系の設定(ｼｽﾃﾑ設定)
            ' 0:XY NOM(右上)  1:X REV(左上)  2:Y REV(右下)  3:XY REV(左下)
            ' ﾄﾘﾐﾝｸﾞ位置座標 (+or-) 回転半径 + ﾃｰﾌﾞﾙｵﾌｾｯﾄ (+or-) ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ + ﾃｰﾌﾞﾙ補正量
            Select Case gSysPrm.stDEV.giBpDirXy

                Case 0 ' x←, y↓
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 1 ' x→, y↓
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY + dblRotY + dblTOffsY + dblBsoY + Del_y

                Case 2 ' x←, y↑
                    dblX = dblTrimPosX + dblRotX + dblTOffsX + dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

                Case 3 ' x→, y↑
                    dblX = dblTrimPosX - dblRotX + dblTOffsX - dblBsoX + Del_x
                    dblY = dblTrimPosY - dblRotY + dblTOffsY - dblBsoY + Del_y

            End Select

            If (1 = intCamera) Then                             ' 外部ｶﾒﾗ位置加算 ?
                dblX = dblX + gSysPrm.stDEV.gfExCmX
                dblY = dblY + gSysPrm.stDEV.gfExCmY
            End If

            'ｽﾃｯﾌﾟ間隔の算出
            intCDir = typPlateInfo.intResistDir                 ' チップ並び方向取得(CHIP-NETのみ)

            If intCDir = 0 Then                                 ' X方向
                dblStepInterval = CalcStepInterval(iYBlock)     ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(Y軸)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 1 Then ' ﾃｰﾌﾞﾙY方向反転なし
                    dblY = dblY + dblStepInterval
                Else                                            ' ﾃｰﾌﾞﾙY方向反転
                    dblY = dblY - dblStepInterval
                End If
            Else                                                ' Y方向
                dblStepInterval = CalcStepInterval(iXBlock)     ' ｽﾃｯﾌﾟｲﾝﾀｰﾊﾞﾙ算出(X軸)
                If gSysPrm.stDEV.giBpDirXy = 0 Or gSysPrm.stDEV.giBpDirXy = 2 Then ' ﾃｰﾌﾞﾙX方向反転なし
                    dblX = dblX + dblStepInterval
                Else                                            ' ﾃｰﾌﾞﾙX方向反転
                    dblX = dblX - dblStepInterval
                End If
            End If

            ' ﾌﾟﾚｰﾄ/ﾌﾞﾛｯｸ位置の相対座標計算
            dblPSX = 0.0 : dblPSY = 0.0                         ' ﾌﾟﾚｰﾄｻｲｽﾞ取得(0固定)
            Select Case gSysPrm.stDEV.giBpDirXy

                Case 0 ' x←, y↓
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

                Case 1 ' x→, y↓
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY + ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

                Case 2 ' x←, y↑
                    dblX = dblX + ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

                Case 3 ' x→, y↑
                    dblX = dblX - ((dblPSX * CInt(iXPlate - 1)) + (dblBSX * CInt(iXBlock - 1)))
                    dblY = dblY - ((dblPSY * CInt(iYPlate - 1)) + (dblBSY * CInt(iYBlock - 1)))

            End Select

            ' 指定ﾌﾟﾚｰﾄ/ﾌﾞﾛｯｸ位置にXYﾃｰﾌﾞﾙ絶対値移動
            r = Form1.System1.XYtableMove(gSysPrm, dblX, dblY)
            Return (r)                                      ' Return

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.XYTableMoveBlock() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                              ' Return値 = 例外エラー 
        End Try

    End Function
#End Region
    'V1.13.0.0⑥ ADD START
#Region "テーブルオフセットを加味したテーブル位置を返す"
    '''=========================================================================
    ''' <summary>
    ''' テーブルオフセットを加味したテーブル位置を返す
    ''' </summary>
    ''' <param name="PosX">(OUT)座標X</param>
    ''' <param name="PosY">(OUT)座標Y</param>
    ''' <returns>0=正常, 0以外=エラー</returns>
    ''' <remarks>'V1.13.0.0⑥</remarks>
    '''=========================================================================
    Public Function XYtableMoveOffsetPosition(ByRef PosX As Double, ByRef PosY As Double) As Integer

        Dim dblTrimPosX As Double                                   ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX
        Dim dblTrimPosY As Double                                   ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝY
        Dim Del_x As Double                                         ' θ補正量X
        Dim Del_y As Double                                         ' θ補正量Y
        Dim dblX As Double                                          ' 移動座標X
        Dim dblY As Double                                          ' 移動座標Y
        Dim mdTbOffx As Double                                      ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX
        Dim mdTbOffy As Double                                      ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄY

        Dim strMSG As String

        Try
            ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
            dblTrimPosX = gSysPrm.stDEV.gfTrimX                     ' ﾄﾘﾑﾎﾟｼﾞｼｮﾝX,Y取得
            dblTrimPosY = gSysPrm.stDEV.gfTrimY
            mdTbOffx = typPlateInfo.dblTableOffsetXDir              ' ﾃｰﾌﾞﾙ位置ｵﾌｾｯﾄX,Yの取得
            mdTbOffy = typPlateInfo.dblTableOffsetYDir

            ' θ補正ﾄﾘﾑｵﾌｾｯﾄX,Y
            Del_x = gfCorrectPosX
            Del_y = gfCorrectPosY

            ' giBpDirXy 座標系の設定(ｼｽﾃﾑ設定)
            ' 0:XY NOM(右上)  1:X REV(左上)  2:Y REV(右下)  3:XY REV(左下)
            ' ﾄﾘﾐﾝｸﾞ位置座標 (+or-) 回転半径 + ﾃｰﾌﾞﾙｵﾌｾｯﾄ (+or-) ﾌﾞﾛｯｸｻｲｽﾞｵﾌｾｯﾄ + ﾃｰﾌﾞﾙ補正量
            Select Case gSysPrm.stDEV.giBpDirXy
                Case 0 ' x←, y↓
                    dblX = dblTrimPosX + mdTbOffx + Del_x
                    dblY = dblTrimPosY + mdTbOffy + Del_y
                Case 1 ' x→, y↓
                    dblX = dblTrimPosX - mdTbOffx - Del_x
                    dblY = dblTrimPosY + mdTbOffy + Del_y
                Case 2 ' x←, y↑
                    dblX = dblTrimPosX + mdTbOffx + Del_x
                    dblY = dblTrimPosY + mdTbOffy - Del_y
                Case 3 ' x→, y↑
                    dblX = dblTrimPosX - mdTbOffx + Del_x
                    dblY = dblTrimPosY + mdTbOffy - Del_y
            End Select

            PosX = dblX
            PosY = dblY
            Return (cFRS_NORMAL)                                    ' Return値 = 正常

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.XYtableMoveOffsetPosition() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー 
        End Try
    End Function
#End Region
    'V1.13.0.0⑥ ADD END

#Region "LOAD・SAVEの共通化によりTrimDataEditorに移動"
#If False Then 'V5.0.0.8①
#Region "GPIBコマンドを設定する"
    '''=========================================================================
    '''<summary>GPIBコマンドを設定する</summary>
    '''<param name="pltInfo">(OUT)プレートデータ</param>
    '''=========================================================================
    Public Sub SetGpibCommand(ByRef pltInfo As PlateInfo)

        Dim strDAT As String
        Dim strMSG As String

        Try
            ' ADEX AX-1152用設定コマンドを設定する
            pltInfo.intGpibDefAdder = giGpibDefAdder                ' GPIBアドレス 
            pltInfo.intGpibDefDelimiter = 0                         ' 初期設定(ﾃﾞﾘﾐﾀ)(固定)
            pltInfo.intGpibDefTimiout = 100                         ' 初期設定(ﾀｲﾑｱｳﾄ)(固定)
            If (pltInfo.intGpibMeasSpeed = 0) Then                  ' 測定速度(0:低速, 1:高速)
                strDAT = "W0"
            Else
                strDAT = "W1"
            End If

            '// 測定モードで切り替え
            If (pltInfo.intGpibMeasMode = 0) Then                   ' 測定モード(0:絶対, 1:偏差)
                strDAT = strDAT + "FR"                              ' 測定モード=絶対
                strDAT = strDAT + "LL00000" + "LH15000"             ' 下限/上限リミットの設定
            Else

                strDAT = strDAT + "FD"                              ' 測定モード=偏差
                strDAT = strDAT + "DL-5000" + "DH+5000"             ' 下限/上限リミットの設定
            End If

            pltInfo.strGpibInitCmnd1 = strDAT                       ' 初期化ｺﾏﾝﾄﾞ1
            pltInfo.strGpibInitCmnd2 = ""                           ' 初期化ｺﾏﾝﾄﾞ2
            pltInfo.strGpibTriggerCmnd = "E"                        ' ﾄﾘｶﾞｺﾏﾝﾄﾞ

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "globals.SetGpibCommand() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
#End If
#End Region
    '----- ###211↓ -----
#Region "START/RESETキー押下待ちサブルーチン"
    '''=========================================================================
    ''' <summary>START/RESETキー押下待ちサブルーチン</summary>
    ''' <param name="Md">(INP)cFRS_ERR_START                = STARTキー押下待ち
    '''                       cFRS_ERR_RST                  = RESETキー押下待ち
    '''                       cFRS_ERR_START + cFRS_ERR_RST = START/RESETキー押下待ち
    ''' </param>
    ''' <param name="bZ">(INP)True=Zキー押下チェックする, False=しない ###220</param>
    ''' <returns>cFRS_ERR_START = STARTキー押下
    '''          cFRS_ERR_RST   = RESETキー押下
    '''          cFRS_ERR_Z     = Zキー押下
    '''          上記以外=エラー
    ''' </returns>
    '''=========================================================================
    Public Function WaitStartRestKey(ByVal Md As Integer, ByVal bZ As Boolean) As Integer

        Dim sts As Long = 0
        Dim r As Long = 0
        Dim ExitFlag As Integer
        Dim strMSG As String

        Try
            ' パラメータチェック
            If (Md = 0) Then
                Return (-1 * ERR_CMD_PRM)                               ' パラメータエラー
            End If

#If cOFFLINEcDEBUG Then                                                 ' OffLineﾃﾞﾊﾞｯｸﾞON ?(↓FormResetが最前面表示なので下記のようにしないとMsgBoxが最前面表示されない)
            Dim Dr As System.Windows.Forms.DialogResult
            Dr = MessageBox.Show("START SW CHECK", "Debug", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            If (Dr = System.Windows.Forms.DialogResult.OK) Then
                ExitFlag = cFRS_ERR_START                               ' Return値 = STARTキー押下
            Else
                ExitFlag = cFRS_ERR_RST                                 ' Return値 = RESETキー押下
            End If
            Return (ExitFlag)
#End If
            ' START/RESETキー押下待ち
            Call ZCONRST()                                              ' コンソールキーラッチ解除
            ExitFlag = -1
            Call Form1.System1.SetSysParam(gSysPrm)                     ' システムパラメータの設定(OcxSystem用)

            ' START/RESETキー押下待ち
            Do
                r = STARTRESET_SWCHECK(False, sts)                      ' START/RESET SW押下チェック
                If (sts = cFRS_ERR_RST) And ((Md = cFRS_ERR_RST) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    ExitFlag = cFRS_ERR_RST                             ' ExitFlag = Cancel(RESETキー)
                ElseIf (sts = cFRS_ERR_START) And ((Md = cFRS_ERR_START) Or (Md = cFRS_ERR_START + cFRS_ERR_RST)) Then
                    ExitFlag = cFRS_ERR_START                           ' ExitFlag = OK(STARTキー)
                End If
                '----- ###220↓ -----
                If (bZ = True) Then
                    r = Z_SWCHECK(sts)                                  ' Z SW押下チェック
                    If (sts <> 0) Then
                        ExitFlag = cFRS_ERR_Z                           ' ExitFlag = Zキー押下
                    End If
                End If
                '----- ###220↑ -----
                System.Windows.Forms.Application.DoEvents()             ' メッセージポンプ
                Call System.Threading.Thread.Sleep(100)                 ' Wait(msec)

                ' システムエラーチェック
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then                              ' 非常停止等(メッセージは表示済) ?
                    ExitFlag = r
                    Exit Do
                End If
            Loop While (ExitFlag = -1)

            Call ZCONRST()                                              ' コンソールキーラッチ解除
            Return (ExitFlag)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "Globals.WaitRestKey() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                          ' Retuen値 = ﾄﾗｯﾌﾟｴﾗｰ発生
        End Try
    End Function
#End Region
    '----- ###211↑ -----
#Region "ユーザカスタマイズ画面表示"
    '''=========================================================================
    ''' <summary>ユーザカスタマイズ画面表示 V1.14.0.0①</summary>
    ''' <param name="CTM_NO">ユーザ番号</param>
    '''=========================================================================
    Public Function Set_UserSpecialCtrl(ByVal CTM_NO As Integer) As Integer

        Dim ret As Integer
        Dim strMsg As String

        Try
            ret = True

            With Form1
                Select Case (CTM_NO)
                    Case customSUSUMU
                        .CmdCnd.Visible = True

                        '　V1.14.0.0① ↓
                        If gSysPrm.stDEV.sEditLock_flg = True Then
                            ' UNLOCK->LOCK
                            gSysPrm.stDEV.sEditLock_flg = False
                            .CmdCnd.Text = "EDIT LOCK"
                            .CmdCnd.BackColor = System.Drawing.SystemColors.Control
                            giPassWord_Lock = 1
                        Else
                            ' LOCK->UNLOCK
                            gSysPrm.stDEV.sEditLock_flg = True
                            .CmdCnd.Text = "EDIT UNLOCK"
                            .CmdCnd.BackColor = System.Drawing.Color.Lime
                            giPassWord_Lock = 0
                        End If

                        '----- V1.18.0.0②↓ -----
                        ' ローム殿仕様
                    Case customROHM
                        ' QRｺｰﾄﾞ情報(初期化)を表示
                        Call QR_Info_Disp(0)                            ' QRｺｰﾄﾞ情報の表示初期化
                        .GrpQrCode.Visible = True                       ' QRｺｰﾄﾞ情報表示グループボックス表示

                        ' 収納マガジンに収納した基板枚数表示グループボックス表示 'V1.18.0.0⑫
                        '.GrpStrageBox.Visible = True
                        ' V6.0.3.0⑤
                        QR_Read_Flg = 0                                 ' QRｺｰﾄﾞ読込み判定(0)NG (1)OK

                        'ポートオープン(QRデータ受信用)
                        ret = QR_Rs232c_Open()
                        If (ret <> SerialErrorCode.rRS_OK) Then
                            ' "シリアルポートＯＰＥＮエラー"
                            ' strMsg = MSG_136 + "(" + "COM4" + ")" 'V4.0.0.0-79
                            strMsg = MSG_136 + "(" + "QR Code Reader COM5" + ")" 'V4.0.0.0-79
                            Call MsgBox(strMsg, vbOKOnly)
                            'Return (ret)
                        End If

                        ' 印刷初期処理
                        ret = Print_Init()
                        If (ret <> cFRS_NORMAL) Then
                            ' "印刷初期処理に失敗しました。(r=xxxx)"
                            strMsg = MSG_152 + "(r = " + ret.ToString("0") + ")"
                            Call MsgBox(strMsg, vbOKOnly)
                            Return (ret)
                        End If
                        '----- V1.18.0.0②↑ -----

                        '----- V6.0.3.0⑤↓ -----
                        ' QRLimitボタン初期化
                        Form1.btnQRLmit.Visible = True
                        Form1.btnQRLmit.BackColor = Color.Lime
                        '----- V6.0.3.0⑤↑ -----

                        '----- V1.23.0.0①↓ -----
                        ' 太陽社殿仕様
                    Case customTAIYO
#If False Then                          'V5.0.0.9⑮ Form1.SetBarcodeMode() でおこなう

                        ' バーコード情報(初期化)を表示
                        Call BC_Info_Disp(0)                            ' バーコード情報の表示初期化
                        .GrpQrCode.Visible = True                       ' バーコード情報(QRｺｰﾄﾞ情報域を使用)表示グループボックス表示

                        ' ポートオープン(バーコードデータ受信用)
                        ret = BC_Rs232c_Open()
                        If (ret <> SerialErrorCode.rRS_OK) Then
                            ' "シリアルポートＯＰＥＮエラー"
                            strMsg = MSG_136 + "(" + gsComPort + ")"
                            Call MsgBox(strMsg, vbOKOnly)
                            'Return (ret)
                        End If
                        '----- V1.23.0.0①↑ -----
#End If
                        '----- V4.11.0.0②↓ (WALSIN殿SL436S対応) -----
                    Case customWALSIN
#If False Then                          'V5.0.0.9⑮ Form1.SetBarcodeMode() でおこなう
                        ' バーコード情報(初期化)を表示
                        Call BC_Info_Disp(0)                            ' バーコード情報の表示初期化
                        .GrpQrCode.Visible = True                       ' バーコード情報(QRｺｰﾄﾞ情報域を使用)表示グループボックス表示

                        ' ポートオープン(バーコードデータ受信用)
                        ret = BC_Rs232c_Open()
                        If (ret <> SerialErrorCode.rRS_OK) Then
                            ' "シリアルポートＯＰＥＮエラー"
                            strMsg = MSG_136 + "(" + gsComPort + ")"
                            Call MsgBox(strMsg, vbOKOnly)
                            'Return (ret)
                        End If

                        ' SizeCode.iniを構造体を設定する
                        ret = IniSizeCodeData(StSizeCode)
                        '----- V4.11.0.0②↑ -----
#End If
                    Case Else
                        ret = True
                End Select
            End With

            Return (ret)
        Catch ex As Exception
            strMsg = "Globals.Set_UserSpecialCtrl() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (False)
        End Try
    End Function
#End Region
    '----- V1.18.0.0①↓ -----
#Region "データ編集用パスワード種別取得"
    '''=========================================================================
    ''' <summary>データ編集用パスワード種別取得</summary>
    ''' <param name="CTM_NO">ユーザ番号</param>
    ''' <returns>パスワード機能(0=なし, 1=あり(進殿仕様), 2あり(ローム殿仕様))</returns>
    '''=========================================================================
    Public Function Get_DataEdit_PasswordKind(ByVal CTM_NO As Integer) As Integer

        Dim ret As Integer = 0
        Dim strMsg As String

        Try
            Select Case (CTM_NO)
                Case customSUSUMU                                       ' 進殿仕様 ?
                    ret = giPassWord_Lock                               ' ret = 0(パスワード機能なし)/ 1 (パスワード機能あり(進殿仕様))

                Case customROHM                                         ' ローム殿仕様 ?
                    ' 「Administrator Mode」又は「ログインパスワードなし」?
                    'If (Form1.lblLoginResult.Visible = True) Or (Form1.flgLoginPWD = 0) Then ' V6.0.3.0_42
                    If (Form1.lblLoginResult.Visible = True) Or (flgLoginPWD = 0) Then        ' V6.0.3.0_42
                        ret = 0                                         ' ret = 0(パスワード機能なし) 
                    Else
                        ret = 2                                         ' ret = 2(パスワード機能あり(ローム殿仕様)) 
                    End If

                Case Else                                               ' その他 
                    ret = 0                                             ' ret = 0(パスワード機能なし) 
            End Select

            Return (ret)

        Catch ex As Exception
            strMsg = "Globals.Get_DataEdit_PasswordKind() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (0)
        End Try
    End Function
#End Region
    '----- V1.18.0.0①↑ -----
    '----- V1.14.0.0⑤↓ -----
#Region "ＬＥＤバックライト制御"
    '''=========================================================================
    ''' <summary>ＬＥＤバックライト制御</summary>
    ''' <param name="MD">   (INP)0:初期設定モード,1:実行モード, 2:終了モード</param>
    ''' <param name="OnOff">(INP)On/Off指定 (1:ＯＮ,0:ＯＦＦ)
    '''                     ※実行モードでローダ自動時で画像認識時のみ有効</param>
    ''' <remarks>stSPF.giLedOnOff = 0(LED制御なし)
    ''' 　　　　　　　　　　　　　= 1(LED制御ありで常時ON)
    '''                           = 2(LED制御ありで常時OFF)
    '''                           = 3(LED制御ありで画像認識時のみON)</remarks>
    '''=========================================================================
    Public Sub Set_Led(ByVal MD As Integer, ByVal OnOff As Integer)

        Dim strMsg As String

        Try
            ' ＬＥＤバックライト制御なしならNOP
            If (gSysPrm.stSPF.giLedOnOff = 0) Then Exit Sub

            ' 終了モード時
            If (MD = 2) Then                                            ' 処理モード = 終了モード ?
                Call EXTOUT1(0, glLedBit)                               ' バックライト照明ＯＦＦ
                Exit Sub
            End If

            ' 初期設定モード時
            If (MD = 0) Then                                            ' 処理モード = 初期設定モード ?
                If (gSysPrm.stSPF.giLedOnOff = 2) Then                  ' 常時ＯＮ ?
                    Call EXTOUT1(glLedBit, 0)                           ' バックライト照明ＯＮ(※0x216aのxBIT BIT状態は保持)

                Else                                                    ' 常時ＯＦＦ/画像認識時 ?
                    Call EXTOUT1(0, glLedBit)                           ' バックライト照明ＯＦＦ
                End If

                ' 実行モード時
            Else
                ' オートローダ自動時以外はNOP
                'If (giHostMode <> cHOSTcMODEcAUTO) Then Exit Sub                                   'V1.20.0.0⑩
                If (giHostMode = cHOSTcMODEcAUTO) Or (gSysPrm.stCTM.giSPECIAL = customSUSUMU) Then  'V1.20.0.0⑩
                    If (gSysPrm.stSPF.giLedOnOff = 3) Then                  ' バックライト照明 = 画像認識時のみ ?
                        If (OnOff = 1) Then                                 ' バックライト照明ＯＮ ?
                            Call EXTOUT1(glLedBit, 0)                       ' バックライト照明ＯＮ
                            Call System.Threading.Thread.Sleep(200)         ' Wait(ms) 
                        Else
                            Call EXTOUT1(0, glLedBit)                       ' バックライト照明ＯＦＦ
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            strMsg = "Globals.Set_Led() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V1.14.0.0⑤↑ -----
    '----- V1.18.0.0③↓ -----
#Region "ファイルパス名からファイル名だけを取り出して返す"
    '''=========================================================================
    ''' <summary>ファイルパス名からファイル名だけを取り出して返す</summary>
    ''' <param name="FPath">(INP)ファイルパス名</param>
    ''' <param name="FNam"> (OUT)ファイル名</param>
    '''=========================================================================
    Public Sub Sub_GetFileName(ByRef FPath As String, ByRef FNam As String)

        Dim Idx As Integer
        Dim rDATA() As String
        Dim strMsg As String

        Try
            rDATA = FPath.Split("\")
            Idx = rDATA.Length
            FNam = rDATA(Idx - 1)

        Catch ex As Exception
            strMsg = "Globals.Sub_GetFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region

#Region "ファイルパス名からドライブ名とフォルダ名を取り出して返す"
    '''=========================================================================
    ''' <summary>ファイルパス名からドライブ名とフォルダ名を取り出して返す</summary>
    ''' <param name="FPath">(INP)ファイルパス名</param>
    ''' <param name="sDrv"> (OUT)ドライブ名</param>
    ''' <param name="sFold">(OUT)フォルダ名</param>
    '''=========================================================================
    Public Sub Sub_GetFileFolder(ByRef FPath As String, ByRef sDrv As String, ByRef sFold As String)

        Dim Idx As Integer
        Dim rDATA() As String
        Dim strMsg As String

        Try
            rDATA = FPath.Split("\")
            sFold = ""
            sDrv = rDATA(0)
            For Idx = 0 To (rDATA.Length - 2)
                If (Idx = (rDATA.Length - 2)) Then
                    sFold = sFold + rDATA(Idx)
                Else
                    sFold = sFold + rDATA(Idx) + "\"
                End If
            Next Idx

        Catch ex As Exception
            strMsg = "Globals.Sub_GetFileFolder() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0③↑ -----
    '----- V1.22.0.0④↓ -----
#Region "拡張子を抜かしたファイル名を取り出して返す"
    '''=========================================================================
    ''' <summary>拡張子を抜かしたファイル名を取り出して返す</summary>
    ''' <param name="strFilePath">(INP)ファイルパス名</param>
    '''=========================================================================
    Public Function GetFileNameNonExtension(ByRef strFilePath As String) As String

        Dim intPos As Integer
        Dim strFileName As String
        Dim strMsg As String

        Try
            ' 最終の\記号の位置を取得
            intPos = InStrRev(strFilePath, "\")
            If (intPos > 0) Then
                ' \記号以降の文字を取得(ファイル名のみを取得)
                strFileName = Right$(strFilePath, Len(strFilePath) - intPos)
            Else
                strFileName = strFilePath
            End If

            ' 最終の.記号を取得
            intPos = InStrRev(strFileName, ".")
            If (intPos > 0) Then
                ' .記号前の文字を取得(拡張子を抜いた形で取得)
                strFileName = Left$(strFileName, intPos - 1)
            End If

            Return (strFileName)

        Catch ex As Exception
            strMsg = "Globals.GetFileNameNonExtension() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return ("")
        End Try
    End Function
#End Region

#Region "ファイル名から拡張子を取り出して返す"
    '''=========================================================================
    ''' <summary>ファイル名から拡張子を取り出して返す V4.0.0.0⑨</summary>
    ''' <param name="strFilePath">(INP)ファイルパス名</param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function GetFileNameExtension(ByRef strFilePath As String) As String

        Dim Pos As Integer
        Dim Len As Integer
        Dim strEXT As String
        Dim strMsg As String

        Try
            ' ファイル名から拡張子を取り出して返す
            Len = strFilePath.Length
            Pos = strFilePath.LastIndexOf(".")                             'V1.16.0.0⑮
            strEXT = strFilePath.Substring(Pos, Len - Pos)

            Return (strEXT)

        Catch ex As Exception
            strMsg = "Globals.GetFileNameExtension() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return ("")
        End Try
    End Function
#End Region

#Region "サマリーロギングファイルパス名作成"
    '''=========================================================================
    ''' <summary>サマリーロギングファイルパス名作成</summary>
    ''' <returns>サマリーロギングファイルパス名</returns>
    '''=========================================================================
    Public Function MakeSummaryFileName() As String

        Dim strFileName As String
        Dim strFilePath As String
        Dim strMsg As String

        Try
            ' サマリーロギングファイル名の作成 = "加工データ名_SUMMARY.TXT"
            strFileName = GetFileNameNonExtension(gStrTrimFileName) ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ名の拡張子を抜かしたファイル名を取り出して返す
            strFilePath = gSysPrm.stLOG.gsLoggingDir + strFileName + "_SUMMARY.TXT"

            Return (strFilePath)

        Catch ex As Exception
            strMsg = "Globals.MakeSummaryFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return ("")
        End Try
    End Function
#End Region
    '----- V1.22.0.0④↑ -----
    '----- V3.0.0.0④(V1.22.0.0③) -----
#Region "トリミングデータがロードされていることを確認"
    '''=========================================================================
    ''' <summary>トリミングデータがロードされていることを確認</summary>
    ''' <returns></returns>
    '''=========================================================================
    Public Function ChkTrimDataLoaded() As Integer

        ChkTrimDataLoaded = cFRS_NORMAL

        If (gLoadDTFlag = False) Then                                   ' ﾄﾘﾐﾝｸﾞﾃﾞｰﾀﾌｧｲﾙ未ﾛｰﾄﾞ ?
            ' "トリミングパラメータデータをロードするか新規作成してください"
            Call Form1.System1.TrmMsgBox(gSysPrm, MSG_14, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
            ChkTrimDataLoaded = cFRS_ERR_RST
        End If

    End Function
#End Region
    '----- V1.22.0.0③↑ -----
    '----- V4.0.0.0-40↓ -----
#Region "ステージYの原点位置(下・上)を考慮してY座標を変換して返す"
    '''=========================================================================
    ''' <summary>ステージYの原点位置(下・上)を考慮してY座標を変換して返す</summary>
    ''' <param name="StgY">(I/O)</param>
    '''=========================================================================
    Public Sub Sub_GetStageYPosistion(ByRef StgY As Double)

        Dim strMsg As String

        Try
            ' SL36S時でステージYの原点位置が上の場合、ブロックサイズの1/2は加算しない
            If (giMachineKd = MACHINE_KD_RS) Then
                ''V4.3.0.0②
                'If (giStageYOrg = STGY_ORG_UP) Then
                StgY = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY
                'Else
                '    StgY = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + gfStgOfsY + (typPlateInfo.dblBlockSizeYDir / 2)
                'End If
                ''V4.3.0.0②
            End If

            Return

        Catch ex As Exception
            strMsg = "Globals.Sub_GetStagePosistion() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
        End Try
    End Sub
#End Region
    '----- V4.0.0.0-40↑ -----
#Region "ブロック番号からブロック位置Ｘ・Ｙを取得する"
    ''' <summary>ブロック番号からブロック位置Ｘ・Ｙを取得する</summary>
    ''' <param name="blockNo">ブロック番号</param>
    ''' <param name="blockPosX">ブロック位置Ｘ(1ORG)</param>
    ''' <param name="blockPosY">ブロック位置Ｙ(1ORG)</param>
    ''' <remarks>'V4.12.2.0①</remarks>
    Friend Sub GetBlockPosition(ByVal blockNo As Integer, ByRef blockPosX As Integer, ByRef blockPosY As Integer)
        If (1 <= blockNo) Then
            With typPlateInfo
                Dim x As Integer = .intBlockCntXDir
                Dim y As Integer = .intBlockCntYDir

                blockNo -= 1
                Select Case .intDirStepRepeat       ' ステップ&(リピート方向)
                    Case STEP_RPT_Y, STEP_RPT_CHIPXSTPY, STEP_RPT_NON
                        blockPosX = Math.Truncate(blockNo / y) + 1

                        If (0 = (blockPosX Mod 2)) Then
                            blockPosY = y - (blockNo Mod y)
                        Else
                            blockPosY = y - (y - (blockNo Mod y)) + 1
                        End If

                    Case STEP_RPT_X, STEP_RPT_CHIPYSTPX
                        blockPosY = Math.Truncate(blockNo / x) + 1

                        If (0 = (blockPosY Mod 2)) Then
                            blockPosX = x - (blockNo Mod x)
                        Else
                            blockPosX = x - (x - (blockNo Mod x)) + 1
                        End If

                    Case Else ' 通らない
                        blockPosX = 1
                        blockPosY = 1
                End Select
            End With
        Else
            blockPosX = 1
            blockPosY = 1
        End If

    End Sub
#End Region

#Region "何番目に処理するブロックであるかを取得する"
    ''' <summary>何番目に処理するブロックであるかを取得する</summary>
    ''' <param name="blockPosX">ブロック位置Ｘ(1ORG)</param>
    ''' <param name="blockPosY">ブロック位置Ｙ(1ORG)</param>
    ''' <returns>処理する順番</returns>
    ''' <remarks>'V4.12.2.0②</remarks>
    Friend Function GetBlockNumber(ByVal blockPosX As Integer, ByVal blockPosY As Integer) As Integer
        Dim ret As Integer

        ' ステップ&(リピート方向)
        With typPlateInfo
            Dim x As Integer = .intBlockCntXDir
            Dim y As Integer = .intBlockCntYDir

            Select Case .intDirStepRepeat
                Case STEP_RPT_Y, STEP_RPT_CHIPXSTPY, STEP_RPT_NON
                    If (0 = (blockPosX Mod 2)) Then
                        ret = (y * (blockPosX - 1)) + (y - blockPosY + 1)
                    Else
                        ret = (y * blockPosX) - (y - blockPosY)
                    End If

                Case STEP_RPT_X, STEP_RPT_CHIPYSTPX
                    If (0 = (blockPosY Mod 2)) Then
                        ret = (x * (blockPosY - 1)) + (x - blockPosX + 1)
                    Else
                        ret = (x * blockPosY) - (x - blockPosX)
                    End If

                Case Else ' 通らない
                    ret = 1
            End Select
        End With

        Return ret

    End Function
#End Region

#Region "次の処理対象ブロック番号を取得する"
    ''' <summary>次の処理対象ブロック番号を取得する</summary>
    ''' <param name="currentBlockNo">現在の処理ブロック番号</param>
    ''' <returns>次の処理対象ブロック番号、処理対象ブロックなしの場合はブロック総数+1</returns>
    ''' <remarks>'V4.12.2.0②</remarks>
    Friend Function GetNextProcBlock(ByVal currentBlockNo As Integer) As Integer
        If (ProcBlock Is Nothing) Then
            Return currentBlockNo
        End If

        Dim x As Integer = typPlateInfo.intBlockCntXDir
        Dim y As Integer = typPlateInfo.intBlockCntYDir
        Dim allBlock As Integer = x * y
        Dim ret As Integer = allBlock + 1

        ' 処理対象の次ブロックを探す
        For i As Integer = currentBlockNo To allBlock Step 1
            x = 0
            y = 0
            Globals_Renamed.GetBlockPosition(i, x, y)

            If (TrimControlLibrary.SelectedState.IsSelected = ProcBlock(x, y)) Then
                ret = i
                Exit For
            End If
        Next i

        Return ret

    End Function
#End Region

#Region "前の処理対象ブロックを取得する"
    ''' <summary>前の処理対象ブロックを取得する</summary>
    ''' <param name="currentBlockNo">現在の処理ブロック番号</param>
    ''' <returns>前の処理対象ブロック番号、処理対象ブロックなしの場合は0</returns>
    ''' <remarks>'V4.12.2.0③</remarks>
    Friend Function GetPrevProcBlock(ByVal currentBlockNo As Integer) As Integer
        If (ProcBlock Is Nothing) Then
            Return currentBlockNo
        End If

        Dim ret As Integer = 0

        ' 処理対象の前ブロックを探す
        For i As Integer = currentBlockNo To 1 Step (-1)
            Dim x, y As Integer
            Globals_Renamed.GetBlockPosition(i, x, y)

            If (TrimControlLibrary.SelectedState.IsSelected = ProcBlock(x, y)) Then
                ret = i
                Exit For
            End If
        Next i

        Return ret

    End Function
#End Region

#Region "処理対象ブロック数を取得する"
    ''' <summary>処理対象ブロック数を取得する</summary>
    ''' <returns>処理対象ブロック数</returns>
    ''' <remarks>'V4.12.2.0②</remarks>
    Friend Function GetProcBlockCount() As Integer
        If (ProcBlock Is Nothing) Then
            Return 0
        End If

        Dim ret As Integer = 0

        For x As Integer = 0 To ProcBlock.GetLength(0) - 1 Step 1
            For y As Integer = 0 To ProcBlock.GetLength(1) - 1 Step 1
                If (TrimControlLibrary.SelectedState.IsSelected = ProcBlock(x, y)) Then
                    ret += 1
                End If
            Next y
        Next x

        Return ret

    End Function
#End Region
    '----- V6.0.3.0⑬↓ -----
#Region "Double 拡張メソッド"
    '''=========================================================================
    '''<summary>Double 拡張メソッド</summary>
    ''' <param name="value">Double.ToStringF(n)</param>
    ''' <param name="n">1～15</param>
    ''' <returns>valueを変換した文字列</returns>
    ''' <remarks></remarks>
    '''=========================================================================
    <Extension()>
    Public Function ToStringF(ByVal value As Double, ByVal n As Integer) As String
        If (n < 1) OrElse (15 < n) Then
            Throw New ArgumentOutOfRangeException()
        End If
        Return Math.Truncate(value).ToString("0") & "." & (value Mod 1).ToString("F" & n).Substring(2, n)
    End Function

#End Region
    '----- V6.0.3.0⑬↑ -----
    '----- V6.1.1.0①↓ -----
#Region "ブザー鳴動"
    '''=========================================================================
    ''' <summary>ブザー鳴動</summary>
    ''' <param name="iRtn">(INP)トリミング結果</param>
    ''' <returns></returns>
    '''=========================================================================
    Public Function Sub_Buzzer_On(ByVal iRtn As Integer) As Integer

        Dim r As Integer
        Dim strMsg As String = ""
        Dim strMsg1 As String = ""
        Dim strMsg2 As String = ""
        Dim strMsg3 As String = ""
        'V6.1.1.0⑫↓
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        'V6.1.1.0⑫↑

        Try
            ' メッセージ設定
            If (giBuzerOn = 0) Then Return (cFRS_NORMAL) '                                  ' トリミング終了時のブザー鳴らさないならNOP
            If (giHostMode = cHOSTcMODEcAUTO) Then Return (cFRS_NORMAL) '                   ' 自動運転中ならNOP
            strMsg1 = ""
            strMsg3 = MSG_frmLimit_07                                                       ' "STARTキーを押すか、OKボタンを押して下さい。"

            ' シグナルタワー点滅＋ブザー
            If (iRtn = cFRS_ERR_PTN) Then
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALARM)           ' 赤点滅＋ブザー１ＯＮ
                strMsg2 = MSG_127                                                           ' "パターン認識エラー"
            Else
                Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_OPERATION_END)   ' 緑点滅＋ブザー１ＯＮ
                'V6.1.1.0⑫↓
                Call Form1.GetMoveMode(digL, digH, digSW)
                If (digL < TRIM_MODE_FT) Then
                    strMsg2 = MSG_SPRASH77                                                      ' "トリミング終了"
                Else
                    strMsg2 = MSG_SPRASH78                                                      ' "動作終了"
                End If
                'V6.1.1.0⑫↑
            End If

            ' "", "トリミング終了 or パターン認識エラー","STARTキーを押すか、OKボタンを押して下さい。"(ユーザプロと合わせる為Form_MsgDispEx()を使用)
            Call Form1.System1.EX_SLIDECOVERCHK(SLIDECOVER_CHECK_OFF)                       ' スライドカバーチェックなし
            r = Form1.System1.Form_MsgDispEx(cFRS_ERR_START, True, strMsg1, strMsg2, strMsg3, System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black), System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue))
            Call ZCONRST()                                                                  ' コンソールキーラッチ解除
            Call Form1.System1.EX_SLIDECOVERCHK(SLIDECOVER_CHECK_ON)                        ' スライドカバーチェックあり

            ' シグナルタワー消灯＋ブザー停止
            Call Form1.System1.SetSignalTowerCtrl(Form1.System1.SIGNAL_ALL_OFF)

            Return (r)

        Catch ex As Exception
            strMsg = "Globals.Sub_Buzzer_On() TRAP ERROR = " + ex.Message
            MsgBox(strMsg)
            Return (cFRS_NORMAL)
        End Try
    End Function
#End Region
    '----- V6.1.1.0①↑ -----

    '===========================================================================
    '   汎用タイマー
    '===========================================================================
    Private bTmTimeOut As Boolean                                       ' タイムアウトフラグ

#Region "汎用タイマー生成"
    '''=========================================================================
    ''' <summary>汎用タイマー生成</summary>
    ''' <param name="TimerTM">(I/O)タイマー</param>
    ''' <param name="TimeVal">(INP)タイムアウト値(msec)</param>
    ''' <remarks>タイマー生成した場合はTimerTM_DisposeをCallしてタイマーを破棄する事</remarks>
    '''=========================================================================
    Public Sub TimerTM_Create(ByRef TimerTM As System.Threading.Timer, ByVal TimeVal As Integer)

        Dim strMSG As String

        Try
            ' タイムアウトチェック用タイマーオブジェクトの作成(TimerTM_TickをTimeVal msec間隔で実行する)
            bTmTimeOut = False                                          ' タイムアウトフラグOFF
            '----- V1.18.0.0②↓ -----
            If (TimeVal = 0) Then
                TimerTM = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerTM_Tick), Nothing, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            Else
                TimerTM = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerTM_Tick), Nothing, TimeVal, TimeVal)
            End If
            '----- V1.18.0.0②↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Create() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "汎用タイマー開始"
    '''=========================================================================
    ''' <summary>汎用タイマー開始</summary>
    ''' <param name="TimerTM">(INP)タイマー</param>
    '''=========================================================================
    Public Sub TimerTM_Start(ByRef TimerTM As System.Threading.Timer, ByVal TimeVal As Integer)

        Dim strMSG As String

        Try
            If (TimerTM Is Nothing) Then Return
            bTmTimeOut = False                                          ' タイムアウトフラグOFF V1.18.0.1⑧
            TimerTM.Change(TimeVal, TimeVal)
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Start() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "汎用タイマー停止(コールバックメソッド(TimerTM_Tick)の呼出しを停止する)"
    '''=========================================================================
    ''' <summary>汎用タイマー停止(コールバックメソッド(TimerTM_Tick)の呼出しを停止する)</summary>
    ''' <param name="TimerTM">(INP)タイマー</param>
    '''=========================================================================
    Public Sub TimerTM_Stop(ByRef TimerTM As System.Threading.Timer)

        Dim strMSG As String

        Try
            ' コールバックメソッドの呼出しを停止する
            If (TimerTM Is Nothing) Then Return
            TimerTM.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Stop() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "汎用タイマーを破棄する"
    '''=========================================================================
    ''' <summary>汎用タイマーを破棄する</summary>
    ''' <param name="TimerTM">(I/O)タイマー</param>
    '''=========================================================================
    Public Sub TimerTM_Dispose(ByRef TimerTM As System.Threading.Timer)

        Dim strMSG As String

        Try
            ' コールバックメソッドの呼出しを停止する
            If (TimerTM Is Nothing) Then Return
            TimerTM.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            TimerTM.Dispose()                                           ' タイマーを破棄する
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Dispose() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "タイムアウトフラグを返す"
    '''=========================================================================
    ''' <summary>タイムアウトフラグを返す</summary>
    ''' <returns>Trur=タイムアウト, False=タイムアウトでない</returns>
    '''=========================================================================
    Public Function TimerTM_Sts() As Boolean

        Dim strMSG As String

        Try
            ' タイムアウトフラグを返す
            Return (bTmTimeOut)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Sts() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (bTmTimeOut)
        End Try
    End Function
#End Region

#Region "タイマーイベント(指定タイマ間隔が経過した時に発生)"
    '''=========================================================================
    ''' <summary>タイマーイベント(指定タイマ間隔が経過した時に発生)</summary>
    ''' <param name="Sts">(INP)</param>
    '''=========================================================================
    Private Sub TimerTM_Tick(ByVal Sts As Object)

        Dim strMSG As String

        Try
            bTmTimeOut = True                                           ' タイムアウトフラグON
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "globals.TimerTM_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "入力キーコードのクリア"
    '''=========================================================================
    ''' <summary>
    ''' 入力キーコードのクリア
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub ClearInpKey()

        Try
            InpKey = 0

            ' トラップエラー発生時
        Catch ex As Exception
            MsgBox("globals.ClearInpKey() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "分布図表示サブ"
    '''=========================================================================
    '''<summary>分布図表示サブ</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub picGraphAccumulationDrawSubLine()
        'Dim i As Short

        '      'UPGRADE_ISSUE: PictureBox メソッド picGraphAccumulation.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
        'picGraphAccumulation.Line (56, 16) - (56, 112), RGB(0, 255, 0)
        '      'UPGRADE_ISSUE: PictureBox メソッド picGraphAccumulation.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
        'picGraphAccumulation.Line (56, 112) - (288, 112), RGB(0, 255, 0)
        '      For i = 0 To 10
        '          'UPGRADE_ISSUE: PictureBox メソッド picGraphAccumulation.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
        '	picGraphAccumulation.Line (56, 24 + (i * 8)) - (288, 24 + (i * 8)), RGB(0, 0, 128)
        '      Next
        '      'UPGRADE_ISSUE: PictureBox メソッド picGraphAccumulation.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
        'picGraphAccumulation.Line (172, 112) - (172, 116), RGB(0, 255, 0)

    End Sub
#End Region

#Region "XYテーブル/Z軸のJOG操作"
    '''=========================================================================
    '''<summary>Z軸/XYテーブルのJOG操作</summary>
    '''<param name="stJOG">       (INP)JOG操作用パラメータ</param>
    '''<param name="TBarLowPitch">(I/O)スライダー1(Lowﾋﾟｯﾁ)</param>
    '''<param name="TBarHiPitch"> (I/O)スライダー2(HIGHﾋﾟｯﾁ)</param>
    '''<param name="TBarPause">   (I/O)スライダー3(Pause Time)</param>
    '''<param name="LblTchMoval0">(I/O)目盛1(Low Pich Label)</param>
    '''<param name="LblTchMoval1">(I/O)目盛2(Lowﾋﾟｯﾁ Label)</param>
    '''<param name="LblTchMoval2">(I/O)目盛3(HIGHﾋﾟｯﾁ Label)</param>
    '''<param name="dblTchMoval"> (I/O)ﾋﾟｯﾁ退避域(0=ﾋﾟｯﾁ, 1=HIGHﾋﾟｯﾁ, 2=Pause Time)</param>
    '''<returns>cFRS_ERR_ADV = OK(STARTｷｰ) 
    '''         cFRS_ERR_RST = Cancel(RESETｷｰ)
    '''         cFRS_ERR_HLT = HALTｷｰ
    '''         -1以下       = エラー</returns>
    ''' <remarks>JogEzInit関数をCall済であること</remarks>
    '''=========================================================================
    Public Function JogEzMoveWithZ(ByRef stJOG As JOG_PARAM, ByVal SysPrm As LaserFront.Trimmer.DllSysPrm.SysParam.SYSPARAM_PARAM, _
                         ByRef TBarLowPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarHiPitch As System.Windows.Forms.TrackBar, _
                         ByRef TBarPause As System.Windows.Forms.TrackBar, _
                         ByRef LblTchMoval0 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval1 As System.Windows.Forms.Label, _
                         ByRef LblTchMoval2 As System.Windows.Forms.Label, _
                         ByRef dblTchMoval() As Double, _
                         ByVal DispProbePos As Action(Of Double)) As Integer    'V6.0.0.0⑪ 引数変更
        'ByRef stJogTextZ As Object) As Integer

        Dim strMSG As String
        Dim r As Short
        Dim z As Double

        Try
            '---------------------------------------------------------------------------
            '   初期処理
            '---------------------------------------------------------------------------
            X = 0.0 : Y = 0.0                                   ' 移動ﾋﾟｯﾁX,Y
            mvx = stJOG.PosX : mvy = stJOG.PosY                 ' BP or ﾃｰﾌﾞﾙ位置X,Y
            mvxBk = stJOG.PosX : mvyBk = stJOG.PosY
            ' キャリブレーション実行/カット位置補正【外部カメラ】時 ※相対座標を表示するためクリアしない
            ' トリミング時の一時停止画面もクリアしない
            If (giAppMode <> APP_MODE_CARIB_REC) And (giAppMode <> APP_MODE_CUTREVIDE) And _
               (giAppMode <> APP_MODE_FINEADJ) And (giAppMode <> APP_MODE_CARIB) Then             ' V1.14.0.0⑦ ###088
                '(giAppMode <> APP_MODE_TRIM) Then              '###088
                stJOG.cgX = 0.0# : stJOG.cgY = 0.0#             ' 移動量X,Y
            End If

            stJOG.Flg = -1
            InpKey = 0
            stJOG.Md = MODE_STG
            stJOG.Md = iGlobalJogMode

            Call Init_Proc(stJOG, TBarLowPitch, TBarHiPitch, TBarPause, LblTchMoval0, LblTchMoval1, LblTchMoval2, dblTchMoval)

            ' 現在の位置を表示する(ﾃｷｽﾄﾎﾞｯｸｽの背景色を処理中(黄色)に設定する)
            Call DispPosition(stJOG, 1)
            'Call SetWindowPos(Me.Handle.ToInt32, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE Or SWP_NOMOVE)
            'Call Me.Focus()                                     ' フォーカスを設定する(テンキー入力のため)
            ''                                                   ' KeyPreviewプロパティをTrueにすると全てのキーイベントをまずフォームが受け取るようになる。
            '---------------------------------------------------------------------------
            '   コンソールボタン又はコンソールキーからのキー入力処理を行う
            '---------------------------------------------------------------------------
            Do
                ' システムエラーチェック
                r = Form1.System1.SysErrChk_ForVBNET(giAppMode)
                If (r <> cFRS_NORMAL) Then GoTo STP_END

                stJOG.Md = iGlobalJogMode

                ' メッセージポンプ
                '  →VB.NETはマルチスレッド対応なので、本来はイベントの開放などでなく、
                '    スレッドを生成してコーディングをするのが正しい。
                '    スレッドでなくても、最低でタイマーを利用する。
                System.Windows.Forms.Application.DoEvents()
                System.Threading.Thread.Sleep(10)               ' CPU使用率を下げるためスリープ

                '----- ###232↓ -----

                '----- ###232↑ -----

                ' コンソールボタン又はコンソールキーからのキー入力
                Call ReadConsoleSw(stJOG, cin)                  ' キー入力

                '-----------------------------------------------------------------------
                '   入力キーチェック
                '-----------------------------------------------------------------------
                If (cin And CONSOLE_SW_RESET) Then              ' RESET SW ?
                    ' RESET SW押下時
                    If (stJOG.Opt And CONSOLE_SW_RESET) Then    ' RESETキー有効 ?
                        r = cFRS_ERR_RST                        ' Return値 = Cancel(RESETｷｰ)
                        Exit Do
                    End If

                    ' HALT SW押下時
                ElseIf (cin And CONSOLE_SW_HALT) Then           ' HALT SW ?
                    If (stJOG.Opt And CONSOLE_SW_HALT) Then     ' オプション(0:HALTキー無効, 1:HALTキー有効)
                        r = cFRS_ERR_HALT                       ' Return値 = HALTｷｰ
                        Exit Do
                    End If

                    ' START SW押下時
                ElseIf (cin And CONSOLE_SW_START) Then          ' START SW ?
                    If (stJOG.Opt And CONSOLE_SW_START) Then    ' STARTキー有効 ?
                        'stJOG.PosX = mvx                       ' 位置X,Y更新
                        'stJOG.PosY = mvy
                        r = cFRS_ERR_START                      ' Return値 = OK(STARTｷｰ) 
                        Exit Do
                    End If

                    ' Z SWがONからOFF(又はOFFからON)に切替わった時
                ElseIf (stJOG.bZ <> bZ) Then
                    If (stJOG.Opt And CONSOLE_SW_ZSW) Then      ' Zキー有効 ?
                        r = cFRS_ERR_Z                          ' Return値 = ZｷｰON/OFF
                        stJOG.bZ = bZ                           ' ON/OFF
                        Exit Do
                    End If
                    ' 矢印SW押下時
                ElseIf cin And &H1E00US Then                    ' 矢印SW
                    If cin And &H100US Then                     ' HI SW ? 
                        mPIT = dblTchMoval(IDX_HPT)             ' mPIT = 移動高速ﾋﾟｯﾁ
                    Else
                        mPIT = dblTchMoval(IDX_PIT)             ' mPIT = 移動通常ﾋﾟｯﾁ
                    End If

                    ' XYテーブル絶対値移動(ソフトリミットチェック有り)
                    r = cFRS_NORMAL
                    'If (stJOG.Md = MODE_STG) Then                ' モード = XYテーブル移動 ?
                    If (iGlobalJogMode = MODE_STG) Then                ' モード = XYテーブル移動 ?
                        ' XYテーブル絶対値移動
                        r = Sub_XYtableMove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then              ' ｴﾗｰ ?
                            If (Form1.System1.IsSoftLimitXY(r) = False) Then
                                GoTo STP_END                    ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                            End If
                        End If
                    ElseIf (iGlobalJogMode = MODE_Z) Then             ' Z絶対値移動
                        '                        If (cin And &H200) Or (cin And &H400)) Then    ' →ｷｰ/←ｷｰ 又は「ZボタンがOFF」時は無効
                        'V6.0.0.0⑧                        If (cin And &H200) Or (cin And &H400) Or (giJogButtonEnable = 0) Then    ' →ｷｰ/←ｷｰ 又は「ZボタンがOFF」時は無効
                        If ((cin And &H200) <> &H0) OrElse ((cin And &H400) <> &H0) OrElse
                            ((cin And CtrlJog.MouseClickLocation.Move) <> &H0) OrElse (giJogButtonEnable = 0) Then
                            ' →ｷｰ/←ｷｰ 又は「ZボタンがOFF」時は無効      'V6.0.0.0⑧ マウスクリック時も無効
                        Else
                            If cin And &H800 Then                               ' ↑ｷｰ ?
                                z = -1 * mPIT
                            ElseIf cin And &H1000 Then                          ' ↓ｷｰ ?
                                z = mPIT
                            End If

                            ' ソフトリミットチェック
                            If ((CleaningPosZ + z) >= SysPrm.stDEV.gfSminMaxZ2) Then
                                'If (SysPrm.stTMN.giMsgTyp = 0) Then
                                '    strMSG = "Z軸ソフトリミット "
                                'Else
                                '    strMSG = "Z AXIS Software LIMIT "
                                'End If
                                strMSG = Globals_Renamed_008
                                Call Form1.System1.TrmMsgBox(SysPrm, strMSG, vbOKOnly, "Z Low Limit")
                                Call ZCONRST()                                    ' ｺﾝｿｰﾙｷｰﾗｯﾁ解除
                                JogEzMoveWithZ = ERR_SOFTLIMIT_Z                    ' Return値 =Z軸ソフトリミット
                                Exit Function
                            End If

                            ' 0以上　
                            If ((CleaningPosZ + z) <= 0) Then
                                'If (SysPrm.stTMN.giMsgTyp = 0) Then
                                '    strMSG = "Z軸 ソフトリミット "
                                'Else
                                '    strMSG = "Z AXIS Software LIMIT "
                                'End If
                                strMSG = Globals_Renamed_008
                                Call Form1.System1.TrmMsgBox(SysPrm, strMSG, vbOKOnly, "Z High Limit")
                                Call ZCONRST()                                    ' ｺﾝｿｰﾙｷｰﾗｯﾁ解除
                                JogEzMoveWithZ = ERR_SOFTLIMIT_Z                    ' Return値 =Z軸ソフトリミット
                                Exit Function
                            End If

                            CleaningPosZ = CleaningPosZ + z
                            r = EX_ZMOVE(CleaningPosZ, MOVE_ABSOLUTE)

                            If (r = cFRS_NORMAL) Then                           ' 正常 ?
                            End If
                            If (r <> cFRS_NORMAL) Then                          ' エラー ?
                                Exit Function
                            End If
                            'V6.0.0.0⑪                            Call frmProbeCleaning.DispProbePos(CleaningPosZ)                 ' 位置表示(Z)
                            DispProbePos.Invoke(CleaningPosZ)                   ' 位置表示(Z)     'V6.0.0.0⑪
                            'V6.0.0.0⑪                            stJogTextZ.Text = Format(CleaningPosZ, "0.000")
                        End If

                        '  モード = BP移動の場合
                    ElseIf (iGlobalJogMode = MODE_BP) Then
                        ' BP絶対値移動
                        r = Sub_BPmove(SysPrm, Form1.System1, Form1.Utility1, stJOG)
                        If (r <> cFRS_NORMAL) Then              ' BP移動エラー ?
                            If (Form1.System1.IsSoftLimitBP(r) = False) Then
                                GoTo STP_END                    ' ｿﾌﾄﾘﾐｯﾄｴﾗｰ以外はｴﾗｰﾘﾀｰﾝ
                            End If
                        End If
                    End If

                    ' ソフトリミットエラーの場合は HI SW以外はOFFする
                    If (r <> cFRS_NORMAL) Then                  ' ｴﾗｰ ?
                        If (stJOG.BtnHI.BackColor = System.Drawing.Color.Yellow) Then
                            InpKey = cBIT_HI                    ' HI SW ON
                        Else
                            InpKey = 0                          ' HI SW以外はOFF
                        End If
                        stJOG.ResetButtonStyle()                'V6.0.0.0-22
                    End If

                    ' 現在の位置を表示する
                    Call DispPosition(stJOG, 1)
                    Call Form1.System1.WAIT(SysPrm.stSYP.gPitPause)    ' Wait(sec)

                    InpKey = CType(CtrlJog.MouseClickLocation.Clear(InpKey), UShort)    'V6.0.0.0⑧
                    stJOG.KeyDown = Keys.None                                           'V6.0.0.0-23
                End If

            Loop While (stJOG.Flg = -1)

            '---------------------------------------------------------------------------
            '   終了処理
            '---------------------------------------------------------------------------
            ' 座標表示用ﾃｷｽﾄﾎﾞｯｸｽの背景色を白色に設定する
            Call DispPosition(stJOG, 0)

            ' 親画面からOK/Cancelﾎﾞﾀﾝ押下 ?
            If (stJOG.Flg <> -1) Then
                r = stJOG.Flg
            End If

            ' OK(STARTｷｰ)なら位置X,Y更新
            If (r = cFRS_ERR_START) Then                            ' OK(STARTｷｰ) ?
                stJOG.PosX = mvx                                    ' 位置X,Y更新
                stJOG.PosY = mvy
            End If

STP_END:
            Call ZCONRST()                                          ' ｺﾝｿｰﾙｷｰﾗｯﾁ解除 
            Return (r)                                              ' Return値設定 

            ' トラップエラー発生時
        Catch ex As Exception
            strMSG = "Globals.JogEzMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                      ' Return値 = 例外エラー 

        Finally
            stJOG.ResetButtonStyle()                'V6.0.0.0-22
        End Try
    End Function

#End Region

    ''' <summary>
    ''' ロット停止の条件を読み込む
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadLotStopData() As Integer

        'V4.9.0.0①
        If giNgStop = 0 Then
            Return 0
        End If

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "TIMMING_PLATE", JudgeNgRate.CheckTimmingPlate)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "TIMMING_BLOCK", JudgeNgRate.CheckTimmingBlock)

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_YIELD", JudgeNgRate.CheckYeld)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_OVERRANGE", JudgeNgRate.CheckOverRange)

        JudgeNgRate.ValYield = Val(GetPrivateProfileString_S("JUDGE", "TEXT_YIELD", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValOverRange = Val(GetPrivateProfileString_S("JUDGE", "TEXT_OVERRANGE", LOT_COND_FILENAME, "0.0"))

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "JUDGE_UNIT", JudgeNgRate.SelectUnit)

        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_ITHI", JudgeNgRate.CheckITHI)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_ITLO", JudgeNgRate.CheckITLO)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_FTHI", JudgeNgRate.CheckFTHI)
        Call Get_SystemParameterShort(LOT_COND_FILENAME, "JUDGE", "CHECK_FTLO", JudgeNgRate.CheckFTLO)

        JudgeNgRate.ValITHI = Val(GetPrivateProfileString_S("JUDGE", "TEXT_ITHI", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValITLO = Val(GetPrivateProfileString_S("JUDGE", "TEXT_ITLO", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValFTHI = Val(GetPrivateProfileString_S("JUDGE", "TEXT_FTHI", LOT_COND_FILENAME, "0.0"))
        JudgeNgRate.ValFTLO = Val(GetPrivateProfileString_S("JUDGE", "TEXT_FTLO", LOT_COND_FILENAME, "0.0"))

    End Function

    ''' <summary>
    ''' XYステージの速度を切り替える 
    ''' </summary>mode:0:通常速度、1:ステップ＆リピート時速度
    ''' <returns></returns>
    Public Function SetXYStageSpeed(mode As Integer) As Integer
        Dim Axis As Integer
        Dim strSECT As String
        Dim strKEY As String
        Dim SpeedMode As Integer
        Dim r2 As Integer

        SpeedMode = mode

        ' X,Y軸PCLパラメータをシスパラより設定する→速度切り替えを標準とする 
        'X軸パラメータの取得
        strSECT = "PCL_PRM_0"
        Axis = 0
        strKEY = "FL_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FL = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "400"))
        strKEY = "FH_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FH = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "3000"))
        strKEY = "DRVRAT_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).DrvRat = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "65"))
        strKEY = "MAGNIF_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).Magnif = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "5"))

        'Y軸パラメータの取得
        strSECT = "PCL_PRM_1"
        Axis = 1
        strKEY = "FL_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FL = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "800"))
        strKEY = "FH_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).FH = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "15000"))
        strKEY = "DRVRAT_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).DrvRat = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "4"))
        strKEY = "MAGNIF_" + (SpeedMode + 1).ToString("000")
        stPclAxisPrm(Axis, SpeedMode).Magnif = Val(GetPrivateProfileString_S(strSECT, strKEY, "C:\TRIM\TKYSYS.INI", "29"))

        ' X,Y軸速度を通常速度に切り替える
        r2 = SETAXISPRM2(AXIS_X, stPclAxisPrm(AXIS_X, SpeedMode).FL, stPclAxisPrm(AXIS_X, SpeedMode).FH, stPclAxisPrm(AXIS_X, SpeedMode).DrvRat, stPclAxisPrm(AXIS_X, SpeedMode).Magnif)
        r2 = SETAXISPRM2(AXIS_Y, stPclAxisPrm(AXIS_Y, SpeedMode).FL, stPclAxisPrm(AXIS_Y, SpeedMode).FH, stPclAxisPrm(AXIS_Y, SpeedMode).DrvRat, stPclAxisPrm(AXIS_Y, SpeedMode).Magnif)
        r2 = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r2, 0)   ' エラーならアプリ強制終了(メッセージ表示済み)

        Return r2

    End Function


End Module