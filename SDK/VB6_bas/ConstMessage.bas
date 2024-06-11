Attribute VB_Name = "ConstMessage"
'==============================================================================
'   Description  : ＯＣＸ用メッセージ設定(日本語/英語)
'
'   Copyright(C) : OMRON Laser Front 2011
'
'   備考         : メッセージ定義ファイル「C:\TRIM\SYSMSG.INI」
'
'-------------------------------------------------------------------------------

Option Explicit
'===============================================================================
'   WIN32 API定義
'===============================================================================
Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Long, ByVal lpFileName As String) As Long

'===============================================================================
'   メッセージ変数定義
'===============================================================================
Public MSG_0 As String
Public MSG_1 As String
Public MSG_2 As String
Public MSG_3 As String
Public MSG_4 As String
Public MSG_5 As String
Public MSG_6 As String
Public MSG_7 As String
Public MSG_8 As String
Public MSG_9 As String
Public MSG_10 As String
Public MSG_02 As String
Public MSG_03 As String
Public MSG_04 As String
Public MSG_COM As String
Public MSG_PP19_1 As String
Public MSG_PP19_2 As String
Public MSG_11 As String
Public MSG_12 As String
Public MSG_14 As String
Public MSG_15 As String
Public MSG_16 As String
Public MSG_17 As String
Public MSG_18 As String
Public MSG_19 As String
Public MSG_20 As String
Public MSG_21 As String
Public MSG_22 As String
Public MSG_23 As String
Public MSG_24 As String
Public MSG_30 As String
Public MSG_31 As String
Public MSG_32 As String
Public MSG_33 As String
Public MSG_34 As String
Public MSG_35 As String
Public MSG_36 As String
Public MSG_37 As String
Public MSG_38 As String
Public MSG_39 As String
Public MSG_40 As String
Public MSG_41 As String
Public MSG_42 As String
Public MSG_43 As String
Public MSG_44 As String
Public MSG_45 As String
Public MSG_46 As String
Public MSG_47 As String
Public MSG_48 As String
Public MSG_49 As String
Public MSG_50 As String
Public MSG_51 As String

Public MSG_PP23 As String
Public MSG_PP24_1 As String
Public MSG_PP24_2 As String
Public MSG_25 As String
Public MSG_TOTAL_CIRCUIT As String
Public MSG_TOTAL_REGISTOR As String
Public MSG_PR_COM1 As String
Public MSG_PR04_1 As String
Public MSG_PR04_2 As String
Public MSG_PR04_3 As String
Public MSG_PR04_4 As String
Public MSG_PR07_1 As String
Public MSG_PR07_2 As String
Public MSG_PR08_1 As String
Public MSG_PR08_2 As String
Public MSG_PR08_3 As String
Public MSG_PR08_COM1 As String
Public MSG_100 As String
Public MSG_101 As String
Public MSG_102 As String
Public MSG_103 As String
Public MSG_104 As String
Public MSG_105 As String
Public MSG_106 As String
Public MSG_107 As String
Public MSG_108 As String
Public MSG_109 As String
Public MSG_110 As String
Public MSG_111 As String
Public MSG_112 As String
Public MSG_113 As String
Public MSG_114 As String
Public MSG_115 As String

' TITLE
Public TITLE_1 As String
Public TITLE_2 As String
Public TITLE_3 As String
Public TITLE_4 As String
Public TITLE_5 As String
Public TITLE_6 As String
Public TITLE_7 As String
Public TITLE_8 As String
Public TITLE_LASER As String
Public TITLE_LOGGING As String
' ラベル
Public LBL_PLATE_1 As String
Public LBL_PLATE_2 As String
Public LBL_PLATE_3 As String
Public LBL_PLATE_4 As String
Public LBL_PLATE_5 As String
Public LBL_PLATE_6 As String
Public LBL_PLATE_7 As String
Public LBL_PLATE_8 As String
Public LBL_PLATE_9 As String
Public LBL_PLATE_10 As String
Public LBL_PLATE_11 As String
Public LBL_PLATE_12 As String
Public LBL_PLATE_13 As String
Public LBL_PLATE_14 As String
Public LBL_PLATE_15 As String
Public LBL_PLATE_16 As String
Public LBL_PLATE_17 As String
Public LBL_PLATE_18 As String
Public LBL_PLATE_19 As String
Public LBL_PLATE_20 As String
'add by yabe Y01start
Public LBL_PLATE_21 As String
Public LBL_PLATE_22 As String
Public LBL_PLATE_23 As String
Public LBL_PLATE_24 As String
Public LBL_PLATE_25 As String
Public LBL_PLATE_26 As String
Public LBL_PLATE_27 As String
Public LBL_PLATE_28 As String
Public LBL_PLATE_29 As String
'
Public LBL_TEACHING_001 As String
Public LBL_TEACHING_002 As String
Public LBL_TEACHING_003 As String
Public LBL_TEACHING_004 As String
Public LBL_TEACHING_005 As String
Public LBL_TEACHING_006 As String
Public LBL_TEACHING_007 As String
Public LBL_TEACHING_008 As String
Public LBL_TEACHING_009 As String
Public LBL_TEACHING_010 As String
Public LBL_TEACHING_011 As String

'cut pos teaching
Public LBL_CUTPOSTEACH_001 As String
Public LBL_CUTPOSTEACH_002 As String
Public LBL_CUTPOSTEACH_003 As String
Public LBL_CUTPOSTEACH_004 As String
Public LBL_CUTPOSTEACH_005 As String

'recog
Public LBL_RECOG_001 As String
Public LBL_RECOG_002 As String
Public LBL_RECOG_003 As String
Public LBL_RECOG_004 As String
Public LBL_RECOG_005 As String
Public LBL_RECOG_006 As String
Public LBL_RECOG_007 As String
Public LBL_RECOG_008 As String
Public LBL_RECOG_009 As String
Public LBL_RECOG_010 As String
Public LBL_RECOG_011 As String
Public LBL_RECOG_012 As String
Public LBL_RECOG_013 As String
Public LBL_RECOG_014 As String
Public LBL_RECOG_015 As String
Public LBL_RECOG_016 As String
Public LBL_RECOG_017 As String
Public LBL_RECOG_018_1 As String
Public LBL_RECOG_018_2 As String
Public LBL_RECOG_019 As String
Public LBL_RECOG_020 As String
Public LBL_RECOG_021 As String
Public LBL_RECOG_022 As String
Public LBL_RECOG_023 As String
Public LBL_RECOG_024 As String
Public LBL_RECOG_025 As String
Public LBL_RECOG_026 As String
Public LBL_RECOG_027 As String
Public LBL_RECOG_028 As String
Public LBL_RECOG_029 As String
Public LBL_RECOG_030 As String
Public LBL_RECOG_031 As String
Public LBL_RECOG_032 As String
Public LBL_RECOG_033 As String
Public LBL_RECOG_034 As String
Public LBL_RECOG_035 As String
Public LBL_RECOG_036 As String
Public LBL_RECOG_037 As String
Public LBL_RECOG_038 As String
Public LBL_RECOG_039 As String
Public LBL_RECOG_040 As String
Public LBL_RECOG_041 As String
Public LBL_RECOG_042 As String
'----- ###813①(VIDEO)↓ -----
Public LBL_RECOG_043 As String
Public LBL_RECOG_044 As String
Public LBL_RECOG_045 As String
'----- ###813①↑ -----
'----- ###813②↓ -----
Public LBL_RECOG_046 As String
Public LBL_RECOG_047 As String
'----- ###813②↑ -----

Public LBL_RECOG_100 As String
Public LBL_RECOG_101 As String
Public LBL_RECOG_102 As String
Public LBL_RECOG_103 As String

' スプラッシュメッセージ(For OcxSystem)
Public MSG_SPRASH0 As String
Public MSG_SPRASH1 As String
Public MSG_SPRASH2 As String
Public MSG_SPRASH3 As String
Public MSG_SPRASH4 As String
Public MSG_SPRASH5 As String
Public MSG_SPRASH6 As String
Public MSG_SPRASH7 As String
Public MSG_SPRASH8 As String
Public MSG_SPRASH9 As String
Public MSG_SPRASH10 As String
Public MSG_SPRASH11 As String
Public MSG_SPRASH12 As String
Public MSG_SPRASH13 As String
Public MSG_SPRASH14 As String
Public MSG_SPRASH15 As String
Public MSG_SPRASH16 As String
Public MSG_SPRASH17 As String
Public MSG_SPRASH18 As String
Public MSG_SPRASH19 As String
Public MSG_SPRASH20 As String
Public MSG_SPRASH21 As String
Public MSG_SPRASH22 As String
Public MSG_SPRASH23 As String
Public MSG_SPRASH24 As String
Public MSG_SPRASH25 As String
Public MSG_SPRASH26 As String
Public MSG_SPRASH27 As String
Public MSG_SPRASH28 As String
Public MSG_SPRASH29 As String                                   ' ###800⑩
Public MSG_SPRASH30 As String                                   ' ###801⑤

'----- INtime側エラーメッセージ(For OcxSystem) -----
Public MSG_CLAMP_MOVE_TIMEOUT As String                         ' クランプ タイムアウト V8.0.0.15③
Public MSG_SRV_ALM As String                                    ' サーボアラーム
Public MSG_AXS_LIM_X As String                                  ' X軸リミット検出
Public MSG_AXS_LIM_Y  As String                                 ' Y軸リミット検出
Public MSG_AXS_LIM_Z  As String                                 ' Z軸リミット検出
Public MSG_AXS_LIM_T  As String                                 ' θ軸リミット検出
Public MSG_AXS_LIM_ATT As String                                ' ATT軸リミット検出
Public MSG_AXS_LIM_Z2 As String                                 ' Z2軸リミット検出
Public MSG_OPN_CVR As String                                    ' 筐体カバー開検出
Public MSG_OPN_SCVR As String                                   ' スライドカバー開検出
Public MSG_OPN_CVRLTC As String                                 ' カバー開ラッチ検出
Public MSG_AXIS_X_SERVO_ALM As String                           ' X軸サーボアラーム
Public MSG_AXIS_Y_SERVO_ALM As String                           ' Y軸サーボアラーム
Public MSG_AXIS_Z_SERVO_ALM As String                           ' Z軸サーボアラーム
Public MSG_AXIS_T_SERVO_ALM As String                           ' θ軸サーボアラーム

' 軸系エラー(タイムアウト)
Public MSG_TIMEOUT_AXIS_X As String                             ' X軸タイムアウトエラー
Public MSG_TIMEOUT_AXIS_Y As String                             ' Y軸タイムアウトエラー
Public MSG_TIMEOUT_AXIS_Z As String                             ' Z軸タイムアウトエラー
Public MSG_TIMEOUT_AXIS_T As String                             ' θ軸タイムアウトエラー
Public MSG_TIMEOUT_AXIS_Z2 As String                            ' Z2軸タイムアウトエラー
Public MSG_TIMEOUT_ATT As String                                ' ロータリアッテネータタイムアウトエラー
Public MSG_TIMEOUT_AXIS_XY As String                            ' XY軸タイムアウトエラー

' 軸系エラー(プラスリミットオーバー)
Public MSG_STG_SOFTLMT_PLUS_AXIS_X As String                    ' X軸プラスリミットオーバー
Public MSG_STG_SOFTLMT_PLUS_AXIS_Y As String                    ' Y軸プラスリミットオーバー
Public MSG_STG_SOFTLMT_PLUS_AXIS_Z As String                    ' Z軸プラスリミットオーバー
Public MSG_STG_SOFTLMT_PLUS_AXIS_T As String                    ' θ軸プラスリミットオーバー
Public MSG_STG_SOFTLMT_PLUS_AXIS_Z2 As String                   ' Z2軸プラスリミットオーバー
Public MSG_STG_SOFTLMT_PLUS As String                           ' 軸プラスリミットオーバー

' 軸系エラー(マイナスリミットオーバー)
Public MSG_STG_SOFTLMT_MINUS_AXIS_X As String                   ' X軸マイナスリミットオーバー
Public MSG_STG_SOFTLMT_MINUS_AXIS_Y As String                   ' Y軸マイナスリミットオーバー
Public MSG_STG_SOFTLMT_MINUS_AXIS_Z As String                   ' Z軸マイナスリミットオーバー
Public MSG_STG_SOFTLMT_MINUS_AXIS_T As String                   ' θ軸マイナスリミットオーバー
Public MSG_STG_SOFTLMT_MINUS_AXIS_Z2 As String                  ' Z2軸マイナスリミットオーバー
Public MSG_STG_SOFTLMT_MINUS As String                          ' 軸マイナスリミットオーバー

' BP系エラー
Public MSG_BP_MOVE_TIMEOUT As String                            ' BP タイムアウト
Public MSG_BP_GRV_ALARM_X As String                             ' ガルバノアラームX
Public MSG_BP_GRV_ALARM_Y As String                             ' ガルバノアラームY
Public MSG_BP_HARD_LIMITOVER_LO As String                       ' BPリミットオーバー（最小可動範囲以下）
Public MSG_BP_HARD_LIMITOVER_HI As String                       ' BPリミットオーバー（最大可動範囲以上）
Public MSG_BP_LIMITOVER As String                               ' BP移動距離設定リミットオーバー
Public MSG_BP_SOFT_LIMITOVER As String                          ' BPソフト可動範囲オーバー
Public MSG_BP_BSIZE_OVER As String                              ' ブロックサイズ設定オーバー（ソフト可動範囲オーバー）

'----- ###806②↓ -----
' GPIB系エラー
Public MSG_GPIB_PARAM As String                                 ' GPIBパラメータエラー
Public MSG_GPIB_TCPSOCKET As String                             ' GPIB-TCP/IP送信エラー
Public MSG_GPIB_EXEC As String                                  ' GPIBコマンド実行エラー
'----- ###806②↑ -----

Public MSG_INTIME_ERROR As String                               ' INtime側エラー

'----- 自動ﾚｰｻﾞﾊﾟﾜｰ調整用(For OcxSystem) -----
Public MSG_AUTOLASER_01 As String
Public MSG_AUTOLASER_02 As String
Public MSG_AUTOLASER_03 As String
Public MSG_AUTOLASER_04 As String
Public MSG_AUTOLASER_05 As String
Public MSG_AUTOLASER_06 As String
Public MSG_AUTOLASER_07 As String
Public MSG_AUTOLASER_08 As String                               ' フルパワー測定
Public MSG_AUTOLASER_09 As String                               ' ＦＬパワー異常
Public MSG_AUTOLASER_10 As String                               ' STARTキーを押すと、パワー調整を開始します
Public MSG_AUTOLASER_11 As String                               ' 測定パワー異常

'----- ローダー関連(For OcxSystem) -----
Public MSG_LOADER_01 As String
Public MSG_LOADER_02 As String
Public MSG_LOADER_03 As String
Public MSG_LOADER_04 As String
Public MSG_LOADER_05 As String
Public MSG_LOADER_06 As String
Public MSG_LOADER_07 As String                                  ' ###805②
Public MSG_LOADER_08 As String                                  ' ###805②

'----- その他(For OcxSystem) -----
Public MSG_ERROR_01 As String
Public MSG_ERROR_02 As String

'----- limit.frm用(For OcxSystem) -----
Public MSG_frmLimit_01 As String
Public MSG_frmLimit_02 As String
Public MSG_frmLimit_03 As String
Public MSG_frmLimit_04 As String
Public MSG_frmLimit_05 As String
Public MSG_frmLimit_06 As String
Public MSG_frmLimit_07 As String
Public MSG_frmLimit_08 As String                                '###801①

'----- Logging()用(For OcxSystem) -----
Public MSG_OpLog_TTL As String
Public MSG_OpLog_01 As String

' ステータスバー
Public CONST_PP01 As String
Public CONST_PP02 As String
Public CONST_PP03 As String
Public CONST_PP04X As String
Public CONST_PP04Y As String
Public CONST_PP05X As String
Public CONST_PP05Y As String
Public CONST_PP06X As String
Public CONST_PP06Y As String
Public CONST_PP07X As String
Public CONST_PP07Y As String
Public CONST_PP08X As String
Public CONST_PP08Y As String
Public CONST_PP09X As String
Public CONST_PP09Y As String
Public CONST_PP10X As String
Public CONST_PP10Y As String
Public CONST_PP11 As String
Public CONST_PP12 As String
Public CONST_PP13 As String
Public CONST_PP14 As String
Public CONST_PP15 As String
Public CONST_PP16 As String
Public CONST_PP17 As String
Public CONST_PP18 As String
Public CONST_PP30 As String
Public CONST_PP31 As String
Public CONST_PP32X As String
Public CONST_PP32Y As String
Public CONST_PP33X As String
Public CONST_PP33Y As String
Public CONST_PP34X As String
Public CONST_PP34Y As String
Public CONST_PP35 As String
Public CONST_PP36X As String
Public CONST_PP36Y As String
Public CONST_PP371 As String
Public CONST_PP372 As String
Public CONST_PP381 As String
Public CONST_P41 As String
Public CONST_P42 As String

' メッセージが未定です
Public CONST_PP60 As String
Public CONST_PP70 As String
Public CONST_IPX As String
Public CONST_IPY As String
Public CONST_PR1 As String
Public CONST_PR2 As String
Public CONST_PR3 As String
Public CONST_PR4H As String
Public CONST_PR4L As String
Public CONST_PR4G As String
Public CONST_PR5 As String
Public CONST_PR6 As String
Public CONST_PR7 As String
Public CONST_PR8 As String
Public CONST_PR9 As String
Public CONST_PR10 As String
Public CONST_PR11H As String
Public CONST_PR11L As String
Public CONST_PR12H As String
Public CONST_PR12L As String
Public CONST_PR13 As String
Public CONST_PR14 As String
Public CONST_PR15 As String
Public CONST_PR16 As String
Public CONST_PR17X As String
Public CONST_PR17Y As String
Public CONST_PR18 As String
Public CONST_CP2 As String
Public CONST_CP4X As String
Public CONST_CP4Y As String
Public CONST_CP5 As String
Public CONST_CP5_2 As String
Public CONST_CP6 As String
Public CONST_CP6_2 As String
Public CONST_CP7 As String
Public CONST_CP7_1 As String
Public CONST_CP9 As String
Public CONST_CP11 As String
Public CONST_CP12 As String
Public CONST_CP13 As String
Public CONST_CP14 As String
Public CONST_CP15 As String
Public CONST_CP17 As String
Public CONST_CP18 As String
Public CONST_CP19 As String
Public CONST_CP30 As String
Public CONST_CP31 As String
Public CONST_CP20 As String
Public CONST_CP21 As String
Public CONST_CP22 As String
Public CONST_CP23 As String
Public CONST_CP24 As String
Public CONST_CPR1 As String
Public CONST_CPR2 As String
Public STS_COM01 As String
Public STS_0 As String
Public STS_1 As String
Public STS_2 As String
Public STS_3 As String
Public STS_4 As String
Public STS_5 As String
Public STS_6 As String
Public STS_7 As String
Public STS_8 As String
Public STS_9 As String
Public STS_10 As String
Public STS_11 As String
Public STS_12 As String
Public STS_13 As String
Public STS_14 As String
Public STS_15 As String
Public STS_16 As String
Public STS_17 As String
Public STS_19 As String
Public STS_20 As String
Public STS_21 As String
Public STS_22 As String
Public STS_23 As String
Public STS_24 As String
Public STS_25 As String
Public STS_26 As String
Public STS_41 As String
Public STS_42 As String

' データ読み込みおよび編集終了時のエラー
Public CONST_LOAD01 As String
Public CONST_LOAD06 As String
Public CONST_LOAD08 As String
Public CONST_LOAD99 As String

' 項目
Public CONST_PR4G_M As String
Public CONST_PR4H_M As String
Public CONST_PR4L_M As String

' ボタン押下時メッセージ
Public MSG_BTN_CLEAR As String
Public MSG_BTN_CANCEL As String

' プローブ位置合わせメッセージ
Public MSG_PRB_XYMODE As String
Public MSG_PRB_BPMODE As String
Public MSG_PRB_ZTMODE As String
Public MSG_PRB_Z2TMODE As String
Public MSG_PRB_THETA As String
Public MSG_PRB_Z_MSG As String

' システムエラーメッセージ
Public MSG_ERR_ZNOTORG As String

' レーザー調整画面説明文
Public MSG_LASER_LASERON As String
Public MSG_LASER_LASEROFF As String
Public MSG_LASEROFF As String
Public MSG_LASERON As String
Public MSG_ERRQRATE As String
Public MSG_LOGERROR As String
Public MSG_SPECPOWER As String
Public MSG_MEASPOWER As String
Public MSG_POWERPROCESS As String
Public MSG_FULLPOWER As String
Public MSG_ATTRATE As String
'----- ###806①↓ -----
Public MSG_AP_POWER As String   ' パワー調整
Public MSG_AP_TARGET As String  ' 目標パワー
Public MSG_AP_HILO As String    ' 許容範囲
Public MSG_AP_HILOW As String   ' (±W)
'----- ###806①↑ -----

' 操作ログ　メッセージ
Public MSG_OPLOG_WAKEUP As String
Public MSG_OPLOG_FUNC01 As String
Public MSG_OPLOG_FUNC02 As String
Public MSG_OPLOG_FUNC03 As String
Public MSG_OPLOG_FUNC04 As String
Public MSG_OPLOG_FUNC05 As String
Public MSG_OPLOG_FUNC06 As String
Public MSG_OPLOG_FUNC07 As String
Public MSG_OPLOG_FUNC08 As String
Public MSG_OPLOG_FUNC09 As String
Public MSG_OPLOG_FUNC10 As String
Public MSG_OPLOG_CLRTOTAL As String
Public MSG_OPLOG_TRIMST As String
Public MSG_OPLOG_TRIMRES As String
Public MSG_OPLOG_HCMD_ERRRST As String
Public MSG_OPLOG_HCMD_PATCMD As String
Public MSG_OPLOG_HCMD_LASCMD As String
Public MSG_OPLOG_HCMD_MARKCMD As String
Public MSG_OPLOG_HCMD_LODCMD As String
Public MSG_OPLOG_HCMD_TECCMD As String
Public MSG_OPLOG_HCMD_TRMCMD As String
Public MSG_OPLOG_HCMD_LSTCMD As String
Public MSG_OPLOG_HCMD_LENCMD As String
Public MSG_OPLOG_HCMD_MDAUTO As String
Public MSG_OPLOG_HCMD_MDMANU As String
Public MSG_OPLOG_HCMD_CPCMD As String
Public MSG_POPUP_MESSAGE As String
Public MSG_OPLOG_FUNC08S As String

' メイン画面ラベル
Public MSG_MAIN_LABEL01 As String
Public MSG_MAIN_LABEL02 As String

' [CIRCUIT]
Public MSG_CIRCUIT_LABEL01 As String
Public MSG_CIRCUIT_LABEL02 As String

' [registor]
Public MSG_REGISTER_LABEL01 As String
Public MSG_REGISTER_LABEL02 As String
Public MSG_REGISTER_LABEL03 As String
Public MSG_REGISTER_LABEL04 As String
Public MSG_REGISTER_LABEL05 As String
Public MSG_REGISTER_LABEL06 As String
Public MSG_REGISTER_LABEL07 As String
Public MSG_REGISTER_LABEL08 As String
Public MSG_REGISTER_LABEL09 As String
Public MSG_REGISTER_LABEL10 As String
Public MSG_REGISTER_LABEL11 As String
Public MSG_REGISTER_LABEL12 As String
Public MSG_REGISTER_LABEL13 As String
Public MSG_REGISTER_LABEL14 As String
Public MSG_REGISTER_LABEL15 As String
Public MSG_REGISTER_LABEL16 As String
Public MSG_REGISTER_LABEL17 As String
Public MSG_REGISTER_LABEL18 As String
Public MSG_REGISTER_LABEL19 As String
Public MSG_REGISTER_LABEL20 As String
Public MSG_REGISTER_LABEL21 As String
Public MSG_REGISTER_LABEL22 As String
Public MSG_REGISTER_LABEL23 As String
Public MSG_REGISTER_LABEL24 As String
Public MSG_REGISTER_LABEL25 As String

' 編集　[cut]
Public MSG_CUT_LABEL01 As String
Public MSG_CUT_LABEL02 As String
Public MSG_CUT_LABEL03 As String
Public MSG_CUT_LABEL04 As String
Public MSG_CUT_LABEL05 As String
Public MSG_CUT_LABEL06 As String
Public MSG_CUT_LABEL07 As String
Public MSG_CUT_LABEL08 As String
Public MSG_CUT_LABEL09 As String
Public MSG_CUT_LABEL10 As String
Public MSG_CUT_LABEL11 As String
Public MSG_CUT_LABEL12 As String
Public MSG_CUT_LABEL13 As String
Public MSG_CUT_LABEL14 As String
Public MSG_CUT_LABEL15 As String

' ■ティーチング
Public MSG_TEACH_LABEL01 As String
Public MSG_TEACH_LABEL02 As String
Public MSG_TEACH_LABEL03 As String
Public MSG_TEACH_LABEL04 As String
Public MSG_TEACH_LABEL05 As String

' ■プローブ位置合わせ
Public MSG_PROBE_LABEL01 As String
Public MSG_PROBE_LABEL02 As String
Public MSG_PROBE_LABEL03 As String
Public MSG_PROBE_LABEL04 As String
Public MSG_PROBE_LABEL05 As String
Public MSG_PROBE_LABEL06 As String
Public MSG_PROBE_LABEL07 As String
Public MSG_PROBE_LABEL08 As String
Public MSG_PROBE_LABEL09 As String
Public MSG_PROBE_LABEL10 As String
Public MSG_PROBE_LABEL11 As String
Public MSG_PROBE_LABEL12 As String
Public MSG_PROBE_LABEL13 As String
Public MSG_PROBE_LABEL14 As String                      ' ###708
Public MSG_PROBE_LABEL15 As String                      ' ###708

Public MSG_PROBE_MSG01 As String
Public MSG_PROBE_MSG02 As String
Public MSG_PROBE_MSG03 As String
Public MSG_PROBE_MSG04 As String

Public MSG_PROBE_ERR01 As String

' ■frmMsgBox 画面終了確認
Public MSG_CLOSE_LABEL01 As String
Public MSG_CLOSE_LABEL02 As String
Public MSG_CLOSE_LABEL03 As String

' ■frmReset 原点復帰画面など
Public MSG_FRMRESET_LABEL01 As String

' ■LASER_teaching
Public MSG_LASER_LABEL01 As String
Public MSG_LASER_LABEL02 As String
Public MSG_LASER_LABEL03 As String                  ' ###800 加工条件番号
Public MSG_LASER_LABEL04 As String                  ' ###800 加工条件
Public MSG_LASER_LABEL05 As String                  ' ###800 Q SWITCH RATE (KHz)
Public MSG_LASER_LABEL06 As String                  ' ###800 STEG本数
Public MSG_LASER_LABEL07 As String                  ' ###800 電流値(mA)
Public MSG_LASER_LABEL08 As String                  ' ###800 加工条件番号を指定して下さい。

' 編集画面 カット
Public LBL_CUT_PLATE_J1 As String
Public LBL_CUT_PLATE_J2 As String
Public LBL_CUT_PLATE_J3 As String
Public LBL_CUT_PLATE_J4 As String
Public LBL_CUT_PLATE_J5 As String
Public LBL_CUT_PLATE_J6 As String
Public LBL_CUT_PLATE_J7 As String
Public LBL_CUT_PLATE_J8 As String
Public LBL_CUT_PLATE_J9 As String
Public LBL_CUT_PLATE_J10 As String
Public LBL_CUT_PLATE_J11 As String
Public LBL_CUT_PLATE_J12 As String
Public LBL_CUT_PLATE_J13 As String
Public LBL_CUT_PLATE_J14 As String
Public LBL_CUT_PLATE_J15 As String
Public LBL_CUT_PLATE_J16 As String
Public LBL_CUT_PLATE_J17 As String
Public LBL_CUT_PLATE_J18 As String
Public LBL_CUT_PLATE_J19 As String
Public LBL_CUT_PLATE_J21 As String
Public LBL_CUT_PLATE_J22 As String
Public LBL_CUT_PLATE_J23 As String
Public LBL_CUT_PLATE_J24 As String
Public LBL_CUT_PLATE_J25 As String
Public LBL_CUT_PLATE_J26 As String
Public LBL_CUT_PLATE_J27 As String
Public MSG_CUT_1 As String
Public LBL_CUT_COPYLINE As String
Public LBL_CUT_COPYCOLUMN As String

' ■CorrectPos
Public MSG_CORPOS_LABEL01 As String                     ' 補正位置１
Public MSG_CORPOS_LABEL02 As String                     ' 指定座標
Public MSG_CORPOS_LABEL03 As String                     ' ずれ量
Public MSG_CORPOS_LABEL04 As String                     ' 補正位置２
Public MSG_CORPOS_LABEL05 As String                     ' 補正値
Public MSG_CORPOS_LABEL06 As String                     ' 回転補正角度
Public MSG_CORPOS_LABEL07 As String                     ' 補正後
Public MSG_CORPOS_LABEL08 As String                     ' トリム位置
Public MSG_CORPOS_MSG01 As String                       ' 補正位置１　移動中
Public MSG_CORPOS_MSG02 As String                       ' Ｚ移動中
Public MSG_CORPOS_MSG03 As String                       ' 補正位置２　移動中
Public MSG_CORPOS_MSG04 As String                       ' 補正位置１を合わせてからADVキーを押下してください
Public MSG_CORPOS_MSG05 As String                       ' 補正位置２を合わせてからADVキーを押下してください
Public MSG_CORPOS_MSG06 As String                       ' 補正値を確認してADVキーを押下してください
Public MSG_CORPOS_MSG07 As String                       ' パターン認識エラーです。手動モードで実行しますか？
Public MSG_CORPOS_MSG08 As String                       ' RS232C通信エラー(下方カメラ)

'===============================================================================
'【機　能】 メッセージ設定(日本語/英語)
'【引　数】 language(INP) : 0=Japanese, 1=English
'【戻り値】 なし
'===============================================================================
Public Sub PrepareMessages(ByVal language As Integer)
    
    Dim sPath As String
    
    sPath = "C:\TRIM\SYSMSG.INI"
    
    Select Case language
    Case 0
        Call PrepareMessagesJapanese(sPath)
    Case 1
        Call PrepareMessagesEnglish(sPath)
    Case Else
        Call PrepareMessagesEnglish(sPath)
    End Select

End Sub

'===============================================================================
'【機　能】 メッセージ設定(日本語/英語)
'【引　数】 sPath(INP) : 設定ファイル名
'【戻り値】 なし
'===============================================================================
Private Sub PrepareMessagesJapanese(sPath As String)

    ' エラーメッセージ
    MSG_0 = GetProfileString_S("JAPANESE", "MSG_0", sPath, "*")
    MSG_1 = GetProfileString_S("JAPANESE", "MSG_1", sPath, "*")
    MSG_2 = GetProfileString_S("JAPANESE", "MSG_2", sPath, "*")
    MSG_3 = GetProfileString_S("JAPANESE", "MSG_3", sPath, "*")
    MSG_4 = GetProfileString_S("JAPANESE", "MSG_4", sPath, "*")
    MSG_5 = GetProfileString_S("JAPANESE", "MSG_5", sPath, "*")
    MSG_6 = GetProfileString_S("JAPANESE", "MSG_6", sPath, "*")
    MSG_7 = GetProfileString_S("JAPANESE", "MSG_7", sPath, "*")
    MSG_8 = GetProfileString_S("JAPANESE", "MSG_8", sPath, "*")
    MSG_9 = GetProfileString_S("JAPANESE", "MSG_9", sPath, "*")
    MSG_10 = GetProfileString_S("JAPANESE", "MSG_10", sPath, "*")
    MSG_02 = GetProfileString_S("JAPANESE", "MSG_02", sPath, "*")
    MSG_03 = GetProfileString_S("JAPANESE", "MSG_03", sPath, "*")
    MSG_04 = GetProfileString_S("JAPANESE", "MSG_04", sPath, "*")
    MSG_COM = GetProfileString_S("JAPANESE", "MSG_COM", sPath, "*")
    MSG_PP19_1 = GetProfileString_S("JAPANESE", "MSG_PP19_1", sPath, "*")
    MSG_PP19_2 = GetProfileString_S("JAPANESE", "MSG_PP19_2", sPath, "*")
    MSG_11 = GetProfileString_S("JAPANESE", "MSG_11", sPath, "*")
    MSG_12 = GetProfileString_S("JAPANESE", "MSG_12", sPath, "*")
    MSG_14 = GetProfileString_S("JAPANESE", "MSG_14", sPath, "*")
    MSG_15 = GetProfileString_S("JAPANESE", "MSG_15", sPath, "*")
    MSG_16 = GetProfileString_S("JAPANESE", "MSG_16", sPath, "*")
    MSG_17 = GetProfileString_S("JAPANESE", "MSG_17", sPath, "*")
    MSG_18 = GetProfileString_S("JAPANESE", "MSG_18", sPath, "*")
    MSG_19 = GetProfileString_S("JAPANESE", "MSG_19", sPath, "*")
    MSG_20 = GetProfileString_S("JAPANESE", "MSG_20", sPath, "*")
    MSG_21 = GetProfileString_S("JAPANESE", "MSG_21", sPath, "*")
    MSG_22 = GetProfileString_S("JAPANESE", "MSG_22", sPath, "*")
    MSG_23 = GetProfileString_S("JAPANESE", "MSG_23", sPath, "*")
    MSG_24 = GetProfileString_S("JAPANESE", "MSG_24", sPath, "*")
    MSG_30 = GetProfileString_S("JAPANESE", "MSG_30", sPath, "*")
    MSG_31 = GetProfileString_S("JAPANESE", "MSG_31", sPath, "*")
    MSG_32 = GetProfileString_S("JAPANESE", "MSG_32", sPath, "*")
    MSG_33 = GetProfileString_S("JAPANESE", "MSG_33", sPath, "*")
    MSG_34 = GetProfileString_S("JAPANESE", "MSG_34", sPath, "*")
    MSG_35 = GetProfileString_S("JAPANESE", "MSG_35", sPath, "*")
    MSG_36 = GetProfileString_S("JAPANESE", "MSG_36", sPath, "*")
    MSG_37 = GetProfileString_S("JAPANESE", "MSG_37", sPath, "*")
    MSG_38 = GetProfileString_S("JAPANESE", "MSG_38", sPath, "*")
    MSG_39 = GetProfileString_S("JAPANESE", "MSG_39", sPath, "*")
    MSG_40 = GetProfileString_S("JAPANESE", "MSG_40", sPath, "*")
    MSG_41 = GetProfileString_S("JAPANESE", "MSG_41", sPath, "*")
    MSG_42 = GetProfileString_S("JAPANESE", "MSG_42", sPath, "*")
    MSG_43 = GetProfileString_S("JAPANESE", "MSG_43", sPath, "*")
    MSG_44 = GetProfileString_S("JAPANESE", "MSG_44", sPath, "*")
    MSG_45 = GetProfileString_S("JAPANESE", "MSG_45", sPath, "*")
    MSG_46 = GetProfileString_S("JAPANESE", "MSG_46", sPath, "*")
    MSG_47 = GetProfileString_S("JAPANESE", "MSG_47", sPath, "*")
    MSG_48 = GetProfileString_S("JAPANESE", "MSG_48", sPath, "*")
    MSG_49 = GetProfileString_S("JAPANESE", "MSG_49", sPath, "*")
    MSG_50 = GetProfileString_S("JAPANESE", "MSG_50", sPath, "*")
    MSG_51 = GetProfileString_S("JAPANESE", "MSG_51", sPath, "*")
    
    MSG_PP23 = GetProfileString_S("JAPANESE", "MSG_PP23", sPath, "*")
    MSG_PP24_1 = GetProfileString_S("JAPANESE", "MSG_PP24_1", sPath, "*")
    MSG_PP24_2 = GetProfileString_S("JAPANESE", "MSG_PP24_2", sPath, "*")
    MSG_25 = GetProfileString_S("JAPANESE", "MSG_25", sPath, "*")
    MSG_TOTAL_CIRCUIT = GetProfileString_S("JAPANESE", "MSG_TOTAL_CIRCUIT", sPath, "*")
    MSG_TOTAL_REGISTOR = GetProfileString_S("JAPANESE", "MSG_TOTAL_REGISTOR", sPath, "*")
    MSG_PR_COM1 = GetProfileString_S("JAPANESE", "MSG_PR_COM1", sPath, "*")
    MSG_PR04_1 = GetProfileString_S("JAPANESE", "MSG_PR04_1", sPath, "*")
    MSG_PR04_2 = GetProfileString_S("JAPANESE", "MSG_PR04_2", sPath, "*")
    MSG_PR04_3 = GetProfileString_S("JAPANESE", "MSG_PR04_3", sPath, "*")
    MSG_PR04_4 = GetProfileString_S("JAPANESE", "MSG_PR04_4", sPath, "*")
    MSG_PR07_1 = GetProfileString_S("JAPANESE", "MSG_PR07_1", sPath, "*")
    MSG_PR07_2 = GetProfileString_S("JAPANESE", "MSG_PR07_2", sPath, "*")
    MSG_PR08_1 = GetProfileString_S("JAPANESE", "MSG_PR08_1", sPath, "*")
    MSG_PR08_2 = GetProfileString_S("JAPANESE", "MSG_PR08_2", sPath, "*")
    MSG_PR08_3 = GetProfileString_S("JAPANESE", "MSG_PR08_3", sPath, "*")
    MSG_PR08_COM1 = GetProfileString_S("JAPANESE", "MSG_PR08_COM1", sPath, "*")
    MSG_100 = GetProfileString_S("JAPANESE", "MSG_100", sPath, "*")
    MSG_101 = GetProfileString_S("JAPANESE", "MSG_101", sPath, "*")
    MSG_102 = GetProfileString_S("JAPANESE", "MSG_102", sPath, "*")
    MSG_103 = GetProfileString_S("JAPANESE", "MSG_103", sPath, "*")
    MSG_104 = GetProfileString_S("JAPANESE", "MSG_104", sPath, "*")
    MSG_105 = GetProfileString_S("JAPANESE", "MSG_105", sPath, "*")
    MSG_106 = GetProfileString_S("JAPANESE", "MSG_106", sPath, "*")
    MSG_107 = GetProfileString_S("JAPANESE", "MSG_107", sPath, "*")
    MSG_108 = GetProfileString_S("JAPANESE", "MSG_108", sPath, "*")
    MSG_109 = GetProfileString_S("JAPANESE", "MSG_109", sPath, "*")
    MSG_110 = GetProfileString_S("JAPANESE", "MSG_110", sPath, "*")
    MSG_111 = GetProfileString_S("JAPANESE", "MSG_111", sPath, "*")
    MSG_112 = GetProfileString_S("JAPANESE", "MSG_112", sPath, "*")
    MSG_113 = GetProfileString_S("JAPANESE", "MSG_113", sPath, "*")
    MSG_114 = GetProfileString_S("JAPANESE", "MSG_114", sPath, "*")
    MSG_115 = GetProfileString_S("JAPANESE", "MSG_115", sPath, "*")

    ' TITLE
    TITLE_1 = GetProfileString_S("JAPANESE", "TITLE_1", sPath, "*")
    TITLE_2 = GetProfileString_S("JAPANESE", "TITLE_2", sPath, "*")
    TITLE_3 = GetProfileString_S("JAPANESE", "TITLE_3", sPath, "*")
    TITLE_4 = GetProfileString_S("JAPANESE", "TITLE_4", sPath, "*")
    TITLE_5 = GetProfileString_S("JAPANESE", "TITLE_5", sPath, "*")
    TITLE_6 = GetProfileString_S("JAPANESE", "TITLE_6", sPath, "*")
    TITLE_7 = GetProfileString_S("JAPANESE", "TITLE_7", sPath, "*")
    TITLE_8 = GetProfileString_S("JAPANESE", "TITLE_8", sPath, "*")
    TITLE_LASER = GetProfileString_S("JAPANESE", "TITLE_LASER", sPath, "*")
    TITLE_LOGGING = GetProfileString_S("JAPANESE", "TITLE_LOGGING", sPath, "*")
    
    ' ラベル
    LBL_PLATE_1 = GetProfileString_S("JAPANESE", "LBL_PLATE_1", sPath, "*")
    LBL_PLATE_2 = GetProfileString_S("JAPANESE", "LBL_PLATE_2", sPath, "*")
    LBL_PLATE_3 = GetProfileString_S("JAPANESE", "LBL_PLATE_3", sPath, "*")
    LBL_PLATE_4 = GetProfileString_S("JAPANESE", "LBL_PLATE_4", sPath, "*")
    LBL_PLATE_5 = GetProfileString_S("JAPANESE", "LBL_PLATE_5", sPath, "*")
    LBL_PLATE_6 = GetProfileString_S("JAPANESE", "LBL_PLATE_6", sPath, "*")
    LBL_PLATE_7 = GetProfileString_S("JAPANESE", "LBL_PLATE_7", sPath, "*")
    LBL_PLATE_8 = GetProfileString_S("JAPANESE", "LBL_PLATE_8", sPath, "*")
    LBL_PLATE_9 = GetProfileString_S("JAPANESE", "LBL_PLATE_9", sPath, "*")
    LBL_PLATE_10 = GetProfileString_S("JAPANESE", "LBL_PLATE_10", sPath, "*")
    LBL_PLATE_11 = GetProfileString_S("JAPANESE", "LBL_PLATE_11", sPath, "*")
    LBL_PLATE_12 = GetProfileString_S("JAPANESE", "LBL_PLATE_12", sPath, "*")
    LBL_PLATE_13 = GetProfileString_S("JAPANESE", "LBL_PLATE_13", sPath, "*")
    LBL_PLATE_14 = GetProfileString_S("JAPANESE", "LBL_PLATE_14", sPath, "*")
    LBL_PLATE_15 = GetProfileString_S("JAPANESE", "LBL_PLATE_15", sPath, "*")
    LBL_PLATE_16 = GetProfileString_S("JAPANESE", "LBL_PLATE_16", sPath, "*")
    LBL_PLATE_17 = GetProfileString_S("JAPANESE", "LBL_PLATE_17", sPath, "*")
    LBL_PLATE_18 = GetProfileString_S("JAPANESE", "LBL_PLATE_18", sPath, "*")
    LBL_PLATE_19 = GetProfileString_S("JAPANESE", "LBL_PLATE_19", sPath, "*")
    LBL_PLATE_20 = GetProfileString_S("JAPANESE", "LBL_PLATE_20", sPath, "*")
    LBL_PLATE_21 = GetProfileString_S("JAPANESE", "LBL_PLATE_21", sPath, "*")
    LBL_PLATE_22 = GetProfileString_S("JAPANESE", "LBL_PLATE_22", sPath, "*")
    LBL_PLATE_23 = GetProfileString_S("JAPANESE", "LBL_PLATE_23", sPath, "*")
    LBL_PLATE_24 = GetProfileString_S("JAPANESE", "LBL_PLATE_24", sPath, "*")
    LBL_PLATE_25 = GetProfileString_S("JAPANESE", "LBL_PLATE_25", sPath, "*")
    LBL_PLATE_26 = GetProfileString_S("JAPANESE", "LBL_PLATE_26", sPath, "*")
    LBL_PLATE_27 = GetProfileString_S("JAPANESE", "LBL_PLATE_27", sPath, "*")
    LBL_PLATE_28 = GetProfileString_S("JAPANESE", "LBL_PLATE_28", sPath, "*")
    LBL_PLATE_29 = GetProfileString_S("JAPANESE", "LBL_PLATE_29", sPath, "*")
    
    'teaching
    LBL_TEACHING_001 = GetProfileString_S("JAPANESE", "LBL_TEACHING_001", sPath, "*")
    LBL_TEACHING_002 = GetProfileString_S("JAPANESE", "LBL_TEACHING_002", sPath, "*")
    LBL_TEACHING_003 = GetProfileString_S("JAPANESE", "LBL_TEACHING_003", sPath, "*")
    LBL_TEACHING_004 = GetProfileString_S("JAPANESE", "LBL_TEACHING_004", sPath, "*")
    LBL_TEACHING_005 = GetProfileString_S("JAPANESE", "LBL_TEACHING_005", sPath, "*")
    LBL_TEACHING_006 = GetProfileString_S("JAPANESE", "LBL_TEACHING_006", sPath, "*")
    LBL_TEACHING_007 = GetProfileString_S("JAPANESE", "LBL_TEACHING_007", sPath, "*")
    LBL_TEACHING_008 = GetProfileString_S("JAPANESE", "LBL_TEACHING_008", sPath, "*")
    LBL_TEACHING_009 = GetProfileString_S("JAPANESE", "LBL_TEACHING_009", sPath, "*")
    LBL_TEACHING_010 = GetProfileString_S("JAPANESE", "LBL_TEACHING_010", sPath, "*")
    LBL_TEACHING_011 = GetProfileString_S("JAPANESE", "LBL_TEACHING_011", sPath, "*")
    
    'cut pos teaching
    LBL_CUTPOSTEACH_001 = GetProfileString_S("JAPANESE", "LBL_CUTPOSTEACH_001", sPath, "*")
    LBL_CUTPOSTEACH_002 = GetProfileString_S("JAPANESE", "LBL_CUTPOSTEACH_002", sPath, "*")
    LBL_CUTPOSTEACH_003 = GetProfileString_S("JAPANESE", "LBL_CUTPOSTEACH_003", sPath, "*")
    LBL_CUTPOSTEACH_004 = GetProfileString_S("JAPANESE", "LBL_CUTPOSTEACH_004", sPath, "*")
    LBL_CUTPOSTEACH_005 = GetProfileString_S("JAPANESE", "LBL_CUTPOSTEACH_005", sPath, "*")
 
    'recog
    LBL_RECOG_001 = GetProfileString_S("JAPANESE", "LBL_RECOG_001", sPath, "*")
    LBL_RECOG_002 = GetProfileString_S("JAPANESE", "LBL_RECOG_002", sPath, "*")
    LBL_RECOG_003 = GetProfileString_S("JAPANESE", "LBL_RECOG_003", sPath, "*")
    LBL_RECOG_004 = GetProfileString_S("JAPANESE", "LBL_RECOG_004", sPath, "*")
    LBL_RECOG_005 = GetProfileString_S("JAPANESE", "LBL_RECOG_005", sPath, "*")
    LBL_RECOG_006 = GetProfileString_S("JAPANESE", "LBL_RECOG_006", sPath, "*")
    LBL_RECOG_007 = GetProfileString_S("JAPANESE", "LBL_RECOG_007", sPath, "*")
    LBL_RECOG_008 = GetProfileString_S("JAPANESE", "LBL_RECOG_008", sPath, "*")
    LBL_RECOG_009 = GetProfileString_S("JAPANESE", "LBL_RECOG_009", sPath, "*")
    LBL_RECOG_010 = GetProfileString_S("JAPANESE", "LBL_RECOG_010", sPath, "*")
    LBL_RECOG_011 = GetProfileString_S("JAPANESE", "LBL_RECOG_011", sPath, "*")
    LBL_RECOG_012 = GetProfileString_S("JAPANESE", "LBL_RECOG_012", sPath, "*")
    LBL_RECOG_013 = GetProfileString_S("JAPANESE", "LBL_RECOG_013", sPath, "*")
    LBL_RECOG_014 = GetProfileString_S("JAPANESE", "LBL_RECOG_014", sPath, "*")
    LBL_RECOG_015 = GetProfileString_S("JAPANESE", "LBL_RECOG_015", sPath, "*")
    LBL_RECOG_016 = GetProfileString_S("JAPANESE", "LBL_RECOG_016", sPath, "*")
    LBL_RECOG_017 = GetProfileString_S("JAPANESE", "LBL_RECOG_017", sPath, "*")
    LBL_RECOG_018_1 = GetProfileString_S("JAPANESE", "LBL_RECOG_018_1", sPath, "*")
    LBL_RECOG_018_2 = GetProfileString_S("JAPANESE", "LBL_RECOG_018_2", sPath, "*")
    LBL_RECOG_019 = GetProfileString_S("JAPANESE", "LBL_RECOG_019", sPath, "*")
    LBL_RECOG_020 = GetProfileString_S("JAPANESE", "LBL_RECOG_020", sPath, "*")
    LBL_RECOG_021 = GetProfileString_S("JAPANESE", "LBL_RECOG_021", sPath, "*")
    LBL_RECOG_022 = GetProfileString_S("JAPANESE", "LBL_RECOG_022", sPath, "*")
    LBL_RECOG_023 = GetProfileString_S("JAPANESE", "LBL_RECOG_023", sPath, "*")
    LBL_RECOG_024 = GetProfileString_S("JAPANESE", "LBL_RECOG_024", sPath, "*")
    LBL_RECOG_025 = GetProfileString_S("JAPANESE", "LBL_RECOG_025", sPath, "*")
    LBL_RECOG_026 = GetProfileString_S("JAPANESE", "LBL_RECOG_026", sPath, "*")
    LBL_RECOG_027 = GetProfileString_S("JAPANESE", "LBL_RECOG_027", sPath, "*")
    LBL_RECOG_028 = GetProfileString_S("JAPANESE", "LBL_RECOG_028", sPath, "*")
    LBL_RECOG_029 = GetProfileString_S("JAPANESE", "LBL_RECOG_029", sPath, "*")
    LBL_RECOG_030 = GetProfileString_S("JAPANESE", "LBL_RECOG_030", sPath, "*")
    LBL_RECOG_031 = GetProfileString_S("JAPANESE", "LBL_RECOG_031", sPath, "*")
    LBL_RECOG_032 = GetProfileString_S("JAPANESE", "LBL_RECOG_032", sPath, "*")
    LBL_RECOG_033 = GetProfileString_S("JAPANESE", "LBL_RECOG_033", sPath, "*")
    LBL_RECOG_034 = GetProfileString_S("JAPANESE", "LBL_RECOG_034", sPath, "*")
    LBL_RECOG_035 = GetProfileString_S("JAPANESE", "LBL_RECOG_035", sPath, "*")
    LBL_RECOG_036 = GetProfileString_S("JAPANESE", "LBL_RECOG_036", sPath, "*")
    LBL_RECOG_037 = GetProfileString_S("JAPANESE", "LBL_RECOG_037", sPath, "*")
    LBL_RECOG_038 = GetProfileString_S("JAPANESE", "LBL_RECOG_038", sPath, "*")
    LBL_RECOG_039 = GetProfileString_S("JAPANESE", "LBL_RECOG_039", sPath, "*")
    LBL_RECOG_040 = GetProfileString_S("JAPANESE", "LBL_RECOG_040", sPath, "*")
    LBL_RECOG_041 = GetProfileString_S("JAPANESE", "LBL_RECOG_041", sPath, "*")
    LBL_RECOG_042 = GetProfileString_S("JAPANESE", "LBL_RECOG_042", sPath, "*")
    LBL_RECOG_043 = GetProfileString_S("JAPANESE", "LBL_RECOG_043", sPath, "*")
    LBL_RECOG_044 = GetProfileString_S("JAPANESE", "LBL_RECOG_044", sPath, "*")
    LBL_RECOG_045 = GetProfileString_S("JAPANESE", "LBL_RECOG_045", sPath, "*")
    ' '###813②
    LBL_RECOG_046 = GetProfileString_S("JAPANESE", "LBL_RECOG_046", sPath, "*")
    LBL_RECOG_047 = GetProfileString_S("JAPANESE", "LBL_RECOG_047", sPath, "*")
    ' '###813②
    
    LBL_RECOG_100 = GetProfileString_S("JAPANESE", "LBL_RECOG_100", sPath, "*")
    LBL_RECOG_101 = GetProfileString_S("JAPANESE", "LBL_RECOG_101", sPath, "*")
    LBL_RECOG_102 = GetProfileString_S("JAPANESE", "LBL_RECOG_102", sPath, "*")
    LBL_RECOG_103 = GetProfileString_S("JAPANESE", "LBL_RECOG_103", sPath, "*")
 
    ' スプラッシュメッセージ
    MSG_SPRASH0 = GetProfileString_S("JAPANESE", "MSG_SPRASH0", sPath, "*")
    MSG_SPRASH1 = GetProfileString_S("JAPANESE", "MSG_SPRASH1", sPath, "*")
    MSG_SPRASH2 = GetProfileString_S("JAPANESE", "MSG_SPRASH2", sPath, "*")
    MSG_SPRASH3 = GetProfileString_S("JAPANESE", "MSG_SPRASH3", sPath, "*")
    MSG_SPRASH4 = GetProfileString_S("JAPANESE", "MSG_SPRASH4", sPath, "*")
    MSG_SPRASH5 = GetProfileString_S("JAPANESE", "MSG_SPRASH5", sPath, "*")
    MSG_SPRASH6 = GetProfileString_S("JAPANESE", "MSG_SPRASH6", sPath, "*")
    MSG_SPRASH7 = GetProfileString_S("JAPANESE", "MSG_SPRASH7", sPath, "*")
    MSG_SPRASH8 = GetProfileString_S("JAPANESE", "MSG_SPRASH8", sPath, "*")
    MSG_SPRASH9 = GetProfileString_S("JAPANESE", "MSG_SPRASH9", sPath, "*")
    MSG_SPRASH10 = GetProfileString_S("JAPANESE", "MSG_SPRASH10", sPath, "*")
    MSG_SPRASH11 = GetProfileString_S("JAPANESE", "MSG_SPRASH11", sPath, "*")
    MSG_SPRASH12 = GetProfileString_S("JAPANESE", "MSG_SPRASH12", sPath, "*")
    MSG_SPRASH13 = GetProfileString_S("JAPANESE", "MSG_SPRASH13", sPath, "*")
    MSG_SPRASH14 = GetProfileString_S("JAPANESE", "MSG_SPRASH14", sPath, "*")
    MSG_SPRASH15 = GetProfileString_S("JAPANESE", "MSG_SPRASH15", sPath, "*")
    MSG_SPRASH16 = GetProfileString_S("JAPANESE", "MSG_SPRASH16", sPath, "*")
    MSG_SPRASH17 = GetProfileString_S("JAPANESE", "MSG_SPRASH17", sPath, "*")
    MSG_SPRASH18 = GetProfileString_S("JAPANESE", "MSG_SPRASH18", sPath, "*")
    MSG_SPRASH19 = GetProfileString_S("JAPANESE", "MSG_SPRASH19", sPath, "*")
    MSG_SPRASH20 = GetProfileString_S("JAPANESE", "MSG_SPRASH20", sPath, "*")
    MSG_SPRASH21 = GetProfileString_S("JAPANESE", "MSG_SPRASH21", sPath, "*")
    MSG_SPRASH22 = GetProfileString_S("JAPANESE", "MSG_SPRASH22", sPath, "*")
    MSG_SPRASH23 = GetProfileString_S("JAPANESE", "MSG_SPRASH23", sPath, "*")
    MSG_SPRASH24 = GetProfileString_S("JAPANESE", "MSG_SPRASH24", sPath, "*")
    MSG_SPRASH25 = GetProfileString_S("JAPANESE", "MSG_SPRASH25", sPath, "*")
    MSG_SPRASH26 = GetProfileString_S("JAPANESE", "MSG_SPRASH26", sPath, "*")
    MSG_SPRASH27 = GetProfileString_S("JAPANESE", "MSG_SPRASH27", sPath, "*")
    MSG_SPRASH28 = GetProfileString_S("JAPANESE", "MSG_SPRASH28", sPath, "*")
    MSG_SPRASH29 = GetProfileString_S("JAPANESE", "MSG_SPRASH29", sPath, "*")
    MSG_SPRASH30 = GetProfileString_S("JAPANESE", "MSG_SPRASH30", sPath, "*")

    '----- INtime側エラーメッセージ(For OcxSystem) -----
    '----- V8.0.0.15③↓ -----
    MSG_CLAMP_MOVE_TIMEOUT = GetProfileString_S("JAPANESE", "MSG_CLAMP_MOVE_TIMEOUT", sPath, "*")
    '----- V8.0.0.15③↑ -----
    MSG_SRV_ALM = GetProfileString_S("JAPANESE", "MSG_SRV_ALM", sPath, "*")
    MSG_AXS_LIM_X = GetProfileString_S("JAPANESE", "MSG_AXS_LIM_X", sPath, "*")
    MSG_AXS_LIM_Y = GetProfileString_S("JAPANESE", "MSG_AXS_LIM_Y", sPath, "*")
    MSG_AXS_LIM_Z = GetProfileString_S("JAPANESE", "MSG_AXS_LIM_Z", sPath, "*")
    MSG_AXS_LIM_T = GetProfileString_S("JAPANESE", "MSG_AXS_LIM_T", sPath, "*")
    MSG_AXS_LIM_ATT = GetProfileString_S("JAPANESE", "MSG_AXS_LIM_ATT", sPath, "*")
    MSG_AXS_LIM_Z2 = GetProfileString_S("JAPANESE", "MSG_AXS_LIM_Z2", sPath, "*")
    MSG_OPN_CVR = GetProfileString_S("JAPANESE", "MSG_OPN_CVR", sPath, "*")
    MSG_OPN_SCVR = GetProfileString_S("JAPANESE", "MSG_OPN_SCVR", sPath, "*")
    MSG_OPN_CVRLTC = GetProfileString_S("JAPANESE", "MSG_OPN_CVRLTC", sPath, "*")
    MSG_AXIS_X_SERVO_ALM = GetProfileString_S("JAPANESE", "MSG_AXIS_X_SERVO_ALM", sPath, "*")
    MSG_AXIS_Y_SERVO_ALM = GetProfileString_S("JAPANESE", "MSG_AXIS_Y_SERVO_ALM", sPath, "*")
    MSG_AXIS_Z_SERVO_ALM = GetProfileString_S("JAPANESE", "MSG_AXIS_Z_SERVO_ALM", sPath, "*")
    MSG_AXIS_T_SERVO_ALM = GetProfileString_S("JAPANESE", "MSG_AXIS_T_SERVO_ALM", sPath, "*")
    
    ' 軸系エラー(タイムアウト)
    MSG_TIMEOUT_AXIS_X = GetProfileString_S("JAPANESE", "MSG_TIMEOUT_AXIS_X", sPath, "*")
    MSG_TIMEOUT_AXIS_Y = GetProfileString_S("JAPANESE", "MSG_TIMEOUT_AXIS_Y", sPath, "*")
    MSG_TIMEOUT_AXIS_Z = GetProfileString_S("JAPANESE", "MSG_TIMEOUT_AXIS_Z", sPath, "*")
    MSG_TIMEOUT_AXIS_T = GetProfileString_S("JAPANESE", "MSG_TIMEOUT_AXIS_T", sPath, "*")
    MSG_TIMEOUT_AXIS_Z2 = GetProfileString_S("JAPANESE", "MSG_TIMEOUT_AXIS_Z2", sPath, "*")
    MSG_TIMEOUT_ATT = GetProfileString_S("JAPANESE", "MSG_TIMEOUT_ATT", sPath, "*")
    MSG_TIMEOUT_AXIS_XY = GetProfileString_S("JAPANESE", "MSG_TIMEOUT_AXIS_XY", sPath, "*")
    
    ' 軸系エラー(プラスリミットオーバー)
    MSG_STG_SOFTLMT_PLUS_AXIS_X = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_PLUS_AXIS_X", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_Y = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_PLUS_AXIS_Y", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_Z = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_Z", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_T = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_T", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_Z2 = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_Z2", sPath, "*")
    MSG_STG_SOFTLMT_PLUS = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_PLUS", sPath, "*")

    ' 軸系エラー(マイナスリミットオーバー)
    MSG_STG_SOFTLMT_MINUS_AXIS_X = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_X", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_Y = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_Y", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_Z = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_Z", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_T = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_T", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_Z2 = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS_AXIS_Z2", sPath, "*")
    MSG_STG_SOFTLMT_MINUS = GetProfileString_S("JAPANESE", "MSG_STG_SOFTLMT_MINUS", sPath, "*")
    
    ' BP系エラー
    MSG_BP_MOVE_TIMEOUT = GetProfileString_S("JAPANESE", "MSG_BP_MOVE_TIMEOUT", sPath, "*")
    MSG_BP_GRV_ALARM_X = GetProfileString_S("JAPANESE", "MSG_BP_GRV_ALARM_X", sPath, "*")
    MSG_BP_GRV_ALARM_Y = GetProfileString_S("JAPANESE", "MSG_BP_GRV_ALARM_Y", sPath, "*")
    MSG_BP_HARD_LIMITOVER_LO = GetProfileString_S("JAPANESE", "MSG_BP_HARD_LIMITOVER_LO", sPath, "*")
    MSG_BP_HARD_LIMITOVER_HI = GetProfileString_S("JAPANESE", "MSG_BP_HARD_LIMITOVER_HI", sPath, "*")
    MSG_BP_LIMITOVER = GetProfileString_S("JAPANESE", "MSG_BP_LIMITOVER", sPath, "*")
    MSG_BP_SOFT_LIMITOVER = GetProfileString_S("JAPANESE", "MSG_BP_SOFT_LIMITOVER", sPath, "*")
    MSG_BP_BSIZE_OVER = GetProfileString_S("JAPANESE", "MSG_BP_BSIZE_OVER", sPath, "*")
    
    '----- ###806②↓ -----
    ' GPIB系エラー
    MSG_GPIB_PARAM = GetProfileString_S("JAPANESE", "MSG_GPIB_PARAM", sPath, "*")
    MSG_GPIB_TCPSOCKET = GetProfileString_S("JAPANESE", "MSG_GPIB_TCPSOCKET", sPath, "*")
    MSG_GPIB_EXEC = GetProfileString_S("JAPANESE", "MSG_GPIB_EXEC", sPath, "*")
    '----- ###806②↑ -----
    
    MSG_INTIME_ERROR = GetProfileString_S("JAPANESE", "MSG_INTIME_ERROR", sPath, "*")

    '----- 自動ﾚｰｻﾞﾊﾟﾜｰ調整用 -----
    MSG_AUTOLASER_01 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_01", sPath, "*")
    MSG_AUTOLASER_02 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_02", sPath, "*")
    MSG_AUTOLASER_03 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_03", sPath, "*")
    MSG_AUTOLASER_04 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_04", sPath, "*")
    MSG_AUTOLASER_05 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_05", sPath, "*")
    MSG_AUTOLASER_06 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_06", sPath, "*")
    MSG_AUTOLASER_07 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_07", sPath, "*")
    MSG_AUTOLASER_08 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_08", sPath, "*")
    MSG_AUTOLASER_09 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_09", sPath, "*")
    MSG_AUTOLASER_10 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_10", sPath, "*")
    MSG_AUTOLASER_11 = GetProfileString_S("JAPANESE", "MSG_AUTOLASER_11", sPath, "*")

    '----- ローダー関連 -----
    MSG_LOADER_01 = GetProfileString_S("JAPANESE", "MSG_LOADER_01", sPath, "*")
    MSG_LOADER_02 = GetProfileString_S("JAPANESE", "MSG_LOADER_02", sPath, "*")
    MSG_LOADER_03 = GetProfileString_S("JAPANESE", "MSG_LOADER_03", sPath, "*")
    MSG_LOADER_04 = GetProfileString_S("JAPANESE", "MSG_LOADER_04", sPath, "*")
    MSG_LOADER_05 = GetProfileString_S("JAPANESE", "MSG_LOADER_05", sPath, "*")
    MSG_LOADER_06 = GetProfileString_S("JAPANESE", "MSG_LOADER_06", sPath, "*")
    MSG_LOADER_07 = GetProfileString_S("JAPANESE", "MSG_LOADER_07", sPath, "*")         ' ###805②
    MSG_LOADER_08 = GetProfileString_S("JAPANESE", "MSG_LOADER_08", sPath, "*")         ' ###805②

    '----- その他 -----
    MSG_ERROR_01 = GetProfileString_S("JAPANESE", "MSG_ERROR_01", sPath, "*")
    MSG_ERROR_02 = GetProfileString_S("JAPANESE", "MSG_ERROR_02", sPath, "*")

    '----- limit.frm用 -----
    MSG_frmLimit_01 = GetProfileString_S("JAPANESE", "MSG_frmLimit_01", sPath, "*")
    MSG_frmLimit_02 = GetProfileString_S("JAPANESE", "MSG_frmLimit_02", sPath, "*")
    MSG_frmLimit_03 = GetProfileString_S("JAPANESE", "MSG_frmLimit_03", sPath, "*")
    MSG_frmLimit_04 = GetProfileString_S("JAPANESE", "MSG_frmLimit_04", sPath, "*")
    MSG_frmLimit_05 = GetProfileString_S("JAPANESE", "MSG_frmLimit_05", sPath, "*")
    MSG_frmLimit_06 = GetProfileString_S("JAPANESE", "MSG_frmLimit_06", sPath, "*")
    MSG_frmLimit_07 = GetProfileString_S("JAPANESE", "MSG_frmLimit_07", sPath, "*")
    MSG_frmLimit_08 = GetProfileString_S("JAPANESE", "MSG_frmLimit_08", sPath, "*")     '###801①

    '----- Logging()用 -----
    MSG_OpLog_TTL = GetProfileString_S("JAPANESE", "MSG_OpLog_TTL", sPath, "*")
    MSG_OpLog_01 = GetProfileString_S("JAPANESE", "MSG_OpLog_01", sPath, "*")

    ' ステータスバー
    CONST_PP01 = GetProfileString_S("JAPANESE", "CONST_PP01", sPath, "*")
    CONST_PP02 = GetProfileString_S("JAPANESE", "CONST_PP02", sPath, "*")
    CONST_PP03 = GetProfileString_S("JAPANESE", "CONST_PP03", sPath, "*")
    CONST_PP04X = GetProfileString_S("JAPANESE", "CONST_PP04X", sPath, "*")
    CONST_PP04Y = GetProfileString_S("JAPANESE", "CONST_PP04Y", sPath, "*")
    CONST_PP05X = GetProfileString_S("JAPANESE", "CONST_PP05X", sPath, "*")
    CONST_PP05Y = GetProfileString_S("JAPANESE", "CONST_PP05Y", sPath, "*")
    CONST_PP06X = GetProfileString_S("JAPANESE", "CONST_PP06X", sPath, "*")
    CONST_PP06Y = GetProfileString_S("JAPANESE", "CONST_PP06Y", sPath, "*")
    CONST_PP07X = GetProfileString_S("JAPANESE", "CONST_PP07X", sPath, "*")
    CONST_PP07Y = GetProfileString_S("JAPANESE", "CONST_PP07Y", sPath, "*")
    CONST_PP08X = GetProfileString_S("JAPANESE", "CONST_PP08X", sPath, "*")
    CONST_PP08Y = GetProfileString_S("JAPANESE", "CONST_PP08Y", sPath, "*")
    CONST_PP09X = GetProfileString_S("JAPANESE", "CONST_PP09X", sPath, "*")
    CONST_PP09Y = GetProfileString_S("JAPANESE", "CONST_PP09Y", sPath, "*")
    CONST_PP10X = GetProfileString_S("JAPANESE", "CONST_PP10X", sPath, "*")
    CONST_PP10Y = GetProfileString_S("JAPANESE", "CONST_PP10Y", sPath, "*")
    CONST_PP11 = GetProfileString_S("JAPANESE", "CONST_PP11", sPath, "*")
    CONST_PP12 = GetProfileString_S("JAPANESE", "CONST_PP12", sPath, "*")
    CONST_PP13 = GetProfileString_S("JAPANESE", "CONST_PP13", sPath, "*")
    CONST_PP14 = GetProfileString_S("JAPANESE", "CONST_PP14", sPath, "*")
    CONST_PP15 = GetProfileString_S("JAPANESE", "CONST_PP15", sPath, "*")
    CONST_PP16 = GetProfileString_S("JAPANESE", "CONST_PP16", sPath, "*")
    CONST_PP17 = GetProfileString_S("JAPANESE", "CONST_PP17", sPath, "*")
    CONST_PP18 = GetProfileString_S("JAPANESE", "CONST_PP18", sPath, "*")
    CONST_PP30 = GetProfileString_S("JAPANESE", "CONST_PP30", sPath, "*")
    CONST_PP31 = GetProfileString_S("JAPANESE", "CONST_PP31", sPath, "*")
    CONST_PP32X = GetProfileString_S("JAPANESE", "CONST_PP32X", sPath, "*")
    CONST_PP32Y = GetProfileString_S("JAPANESE", "CONST_PP32Y", sPath, "*")
    CONST_PP33X = GetProfileString_S("JAPANESE", "CONST_PP33X", sPath, "*")
    CONST_PP33Y = GetProfileString_S("JAPANESE", "CONST_PP33Y", sPath, "*")
    CONST_PP34X = GetProfileString_S("JAPANESE", "CONST_PP34X", sPath, "*")
    CONST_PP34Y = GetProfileString_S("JAPANESE", "CONST_PP34Y", sPath, "*")
    CONST_PP35 = GetProfileString_S("JAPANESE", "CONST_PP35", sPath, "*")
    CONST_PP36X = GetProfileString_S("JAPANESE", "CONST_PP36X", sPath, "*")
    CONST_PP36Y = GetProfileString_S("JAPANESE", "CONST_PP36Y", sPath, "*")
    CONST_PP371 = GetProfileString_S("JAPANESE", "CONST_PP371", sPath, "*")
    CONST_PP372 = GetProfileString_S("JAPANESE", "CONST_PP372", sPath, "*")
    CONST_PP381 = GetProfileString_S("JAPANESE", "CONST_PP381", sPath, "*")
    CONST_P41 = GetProfileString_S("JAPANESE", "CONST_P41", sPath, "*")
    CONST_P42 = GetProfileString_S("JAPANESE", "CONST_P42", sPath, "*")

    CONST_PP60 = GetProfileString_S("JAPANESE", "CONST_PP60", sPath, "*")
    CONST_PP70 = GetProfileString_S("JAPANESE", "CONST_PP70", sPath, "*")
    CONST_IPX = GetProfileString_S("JAPANESE", "CONST_IPX", sPath, "*")
    CONST_IPY = GetProfileString_S("JAPANESE", "CONST_IPY", sPath, "*")
    CONST_PR1 = GetProfileString_S("JAPANESE", "CONST_PR1", sPath, "*")
    CONST_PR2 = GetProfileString_S("JAPANESE", "CONST_PR2", sPath, "*")
    CONST_PR3 = GetProfileString_S("JAPANESE", "CONST_PR3", sPath, "*")
    CONST_PR4H = GetProfileString_S("JAPANESE", "CONST_PR4H", sPath, "*")
    CONST_PR4L = GetProfileString_S("JAPANESE", "CONST_PR4L", sPath, "*")
    CONST_PR4G = GetProfileString_S("JAPANESE", "CONST_PR4G", sPath, "*")
    CONST_PR5 = GetProfileString_S("JAPANESE", "CONST_PR5", sPath, "*")
    CONST_PR6 = GetProfileString_S("JAPANESE", "CONST_PR6", sPath, "*")
    CONST_PR7 = GetProfileString_S("JAPANESE", "CONST_PR7", sPath, "*")
    CONST_PR8 = GetProfileString_S("JAPANESE", "CONST_PR8", sPath, "*")
    CONST_PR9 = GetProfileString_S("JAPANESE", "CONST_PR9", sPath, "*")
    CONST_PR10 = GetProfileString_S("JAPANESE", "CONST_PR10", sPath, "*")
    CONST_PR11H = GetProfileString_S("JAPANESE", "CONST_PR11H", sPath, "*")
    CONST_PR11L = GetProfileString_S("JAPANESE", "CONST_PR11L", sPath, "*")
    CONST_PR12H = GetProfileString_S("JAPANESE", "CONST_PR12H", sPath, "*")
    CONST_PR12L = GetProfileString_S("JAPANESE", "CONST_PR12L", sPath, "*")
    CONST_PR13 = GetProfileString_S("JAPANESE", "CONST_PR13", sPath, "*")
    CONST_PR14 = GetProfileString_S("JAPANESE", "CONST_PR14", sPath, "*")
    CONST_PR15 = GetProfileString_S("JAPANESE", "CONST_PR15", sPath, "*")
    CONST_PR16 = GetProfileString_S("JAPANESE", "CONST_PR16", sPath, "*")
    CONST_PR17X = GetProfileString_S("JAPANESE", "CONST_PR17X", sPath, "*")
    CONST_PR17Y = GetProfileString_S("JAPANESE", "CONST_PR17Y", sPath, "*")
    CONST_PR18 = GetProfileString_S("JAPANESE", "CONST_PR18", sPath, "*")
    CONST_CP2 = GetProfileString_S("JAPANESE", "CONST_CP2", sPath, "*")
    CONST_CP4X = GetProfileString_S("JAPANESE", "CONST_CP4X", sPath, "*")
    CONST_CP4Y = GetProfileString_S("JAPANESE", "CONST_CP4Y", sPath, "*")
    CONST_CP5 = GetProfileString_S("JAPANESE", "CONST_CP5", sPath, "*")
    CONST_CP5_2 = GetProfileString_S("JAPANESE", "CONST_CP5_2", sPath, "*")
    CONST_CP6 = GetProfileString_S("JAPANESE", "CONST_CP6", sPath, "*")
    CONST_CP6_2 = GetProfileString_S("JAPANESE", "CONST_CP6_2", sPath, "*")
    CONST_CP7 = GetProfileString_S("JAPANESE", "CONST_CP7", sPath, "*")
    CONST_CP7_1 = GetProfileString_S("JAPANESE", "CONST_CP7_1", sPath, "*")
    CONST_CP9 = GetProfileString_S("JAPANESE", "CONST_CP9", sPath, "*")
    CONST_CP11 = GetProfileString_S("JAPANESE", "CONST_CP11", sPath, "*")
    CONST_CP12 = GetProfileString_S("JAPANESE", "CONST_CP12", sPath, "*")
    CONST_CP13 = GetProfileString_S("JAPANESE", "CONST_CP13", sPath, "*")
    CONST_CP14 = GetProfileString_S("JAPANESE", "CONST_CP14", sPath, "*")
    CONST_CP15 = GetProfileString_S("JAPANESE", "CONST_CP15", sPath, "*")
    CONST_CP17 = GetProfileString_S("JAPANESE", "CONST_CP17", sPath, "*")
    CONST_CP18 = GetProfileString_S("JAPANESE", "CONST_CP18", sPath, "*")
    CONST_CP19 = GetProfileString_S("JAPANESE", "CONST_CP19", sPath, "*")
    CONST_CP30 = GetProfileString_S("JAPANESE", "CONST_CP30", sPath, "*")
    CONST_CP31 = GetProfileString_S("JAPANESE", "CONST_CP31", sPath, "*")
    CONST_CP20 = GetProfileString_S("JAPANESE", "CONST_CP20", sPath, "*")
    CONST_CP21 = GetProfileString_S("JAPANESE", "CONST_CP21", sPath, "*")
    CONST_CP22 = GetProfileString_S("JAPANESE", "CONST_CP22", sPath, "*")
    CONST_CP23 = GetProfileString_S("JAPANESE", "CONST_CP23", sPath, "*")
    CONST_CP24 = GetProfileString_S("JAPANESE", "CONST_CP24", sPath, "*")
    CONST_CPR1 = GetProfileString_S("JAPANESE", "CONST_CPR1", sPath, "*")
    CONST_CPR2 = GetProfileString_S("JAPANESE", "CONST_CPR2", sPath, "*")
    STS_COM01 = GetProfileString_S("JAPANESE", "STS_COM01", sPath, "*")
    STS_0 = GetProfileString_S("JAPANESE", "STS_0", sPath, "*")
    STS_1 = GetProfileString_S("JAPANESE", "STS_1", sPath, "*")
    STS_2 = GetProfileString_S("JAPANESE", "STS_2", sPath, "*")
    STS_3 = GetProfileString_S("JAPANESE", "STS_3", sPath, "*")
    STS_4 = GetProfileString_S("JAPANESE", "STS_4", sPath, "*")
    STS_5 = GetProfileString_S("JAPANESE", "STS_5", sPath, "*")
    STS_6 = GetProfileString_S("JAPANESE", "STS_6", sPath, "*")
    STS_7 = GetProfileString_S("JAPANESE", "STS_7", sPath, "*")
    STS_8 = GetProfileString_S("JAPANESE", "STS_8", sPath, "*")
    STS_9 = GetProfileString_S("JAPANESE", "STS_9", sPath, "*")
    STS_10 = GetProfileString_S("JAPANESE", "STS_10", sPath, "*")
    STS_11 = GetProfileString_S("JAPANESE", "STS_11", sPath, "*")
    STS_12 = GetProfileString_S("JAPANESE", "STS_12", sPath, "*")
    STS_13 = GetProfileString_S("JAPANESE", "STS_13", sPath, "*")
    STS_14 = GetProfileString_S("JAPANESE", "STS_14", sPath, "*")
    STS_15 = GetProfileString_S("JAPANESE", "STS_15", sPath, "*")
    STS_16 = GetProfileString_S("JAPANESE", "STS_16", sPath, "*")
    STS_17 = GetProfileString_S("JAPANESE", "STS_17", sPath, "*")
    STS_19 = GetProfileString_S("JAPANESE", "STS_19", sPath, "*")
    STS_20 = GetProfileString_S("JAPANESE", "STS_20", sPath, "*")
    STS_21 = GetProfileString_S("JAPANESE", "STS_21", sPath, "*")
    STS_22 = GetProfileString_S("JAPANESE", "STS_22", sPath, "*")
    STS_23 = GetProfileString_S("JAPANESE", "STS_23", sPath, "*")
    STS_24 = GetProfileString_S("JAPANESE", "STS_24", sPath, "*")
    STS_25 = GetProfileString_S("JAPANESE", "STS_25", sPath, "*")
    STS_26 = GetProfileString_S("JAPANESE", "STS_26", sPath, "*")
    STS_41 = GetProfileString_S("JAPANESE", "STS_41", sPath, "*")
    STS_42 = GetProfileString_S("JAPANESE", "STS_42", sPath, "*")

    ' データ読み込みおよび編集終了時のエラー
    CONST_LOAD01 = GetProfileString_S("JAPANESE", "CONST_LOAD01", sPath, "*")
    CONST_LOAD06 = GetProfileString_S("JAPANESE", "CONST_LOAD06", sPath, "*")
    CONST_LOAD08 = GetProfileString_S("JAPANESE", "CONST_LOAD08", sPath, "*")
    CONST_LOAD99 = GetProfileString_S("JAPANESE", "CONST_LOAD99", sPath, "*")
    
    ' 項目
    CONST_PR4G_M = GetProfileString_S("JAPANESE", "CONST_PR4G_M", sPath, "*")
    CONST_PR4H_M = GetProfileString_S("JAPANESE", "CONST_PR4H_M", sPath, "*")
    CONST_PR4L_M = GetProfileString_S("JAPANESE", "CONST_PR4L_M", sPath, "*")
    
    ' ボタン押下時メッセージ
    MSG_BTN_CLEAR = GetProfileString_S("JAPANESE", "MSG_BTN_CLEAR", sPath, "*")
    MSG_BTN_CANCEL = GetProfileString_S("JAPANESE", "MSG_BTN_CANCEL", sPath, "*")
    
    ' プローブ位置合わせメッセージ
    MSG_PRB_XYMODE = GetProfileString_S("JAPANESE", "MSG_PRB_XYMODE", sPath, "*")
    MSG_PRB_BPMODE = GetProfileString_S("JAPANESE", "MSG_PRB_BPMODE", sPath, "*")
    MSG_PRB_ZTMODE = GetProfileString_S("JAPANESE", "MSG_PRB_ZTMODE", sPath, "*")
    MSG_PRB_Z2TMODE = GetProfileString_S("JAPANESE", "MSG_PRB_Z2TMODE", sPath, "*")
    MSG_PRB_THETA = GetProfileString_S("JAPANESE", "MSG_PRB_THETA", sPath, "*")
    MSG_PRB_Z_MSG = GetProfileString_S("JAPANESE", "MSG_PRB_Z_MSG", sPath, "*")
    
    ' システムエラーメッセージ
    MSG_ERR_ZNOTORG = GetProfileString_S("JAPANESE", "MSG_ERR_ZNOTORG", sPath, "*")
    
    ' レーザー調整画面説明文
    MSG_LASER_LASERON = GetProfileString_S("JAPANESE", "MSG_LASER_LASERON", sPath, "*")
    MSG_LASER_LASEROFF = GetProfileString_S("JAPANESE", "MSG_LASER_LASEROFF", sPath, "*")
    MSG_LASEROFF = GetProfileString_S("JAPANESE", "MSG_LASEROFF", sPath, "*")
    MSG_LASERON = GetProfileString_S("JAPANESE", "MSG_LASERON", sPath, "*")
    MSG_ERRQRATE = GetProfileString_S("JAPANESE", "MSG_ERRQRATE", sPath, "*")
    MSG_LOGERROR = GetProfileString_S("JAPANESE", "MSG_LOGERROR", sPath, "*")
    MSG_SPECPOWER = GetProfileString_S("JAPANESE", "MSG_SPECPOWER", sPath, "*")
    MSG_MEASPOWER = GetProfileString_S("JAPANESE", "MSG_MEASPOWER", sPath, "*")
    MSG_FULLPOWER = GetProfileString_S("JAPANESE", "MSG_FULLPOWER", sPath, "*")
    MSG_POWERPROCESS = GetProfileString_S("JAPANESE", "MSG_POWERPROCESS", sPath, "*")
    MSG_ATTRATE = GetProfileString_S("JAPANESE", "MSG_ATTRATE", sPath, "*")
    '----- ###806①↓ -----
    MSG_AP_POWER = GetProfileString_S("JAPANESE", "MSG_AP_POWER", sPath, "*")
    MSG_AP_TARGET = GetProfileString_S("JAPANESE", "MSG_AP_TARGET", sPath, "*")
    MSG_AP_HILO = GetProfileString_S("JAPANESE", "MSG_AP_HILO", sPath, "*")
    MSG_AP_HILOW = GetProfileString_S("JAPANESE", "MSG_AP_HILOW", sPath, "*")
    '----- ###806①↑ -----
    
    ' 操作ログ　メッセージ
    MSG_OPLOG_WAKEUP = GetProfileString_S("JAPANESE", "MSG_OPLOG_WAKEUP", sPath, "*")
    MSG_OPLOG_FUNC01 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC01", sPath, "*")
    MSG_OPLOG_FUNC02 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC02", sPath, "*")
    MSG_OPLOG_FUNC03 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC03", sPath, "*")
    MSG_OPLOG_FUNC04 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC04", sPath, "*")
    MSG_OPLOG_FUNC05 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC05", sPath, "*")
    MSG_OPLOG_FUNC06 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC06", sPath, "*")
    MSG_OPLOG_FUNC07 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC07", sPath, "*")
    MSG_OPLOG_FUNC08 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC08", sPath, "*")
    MSG_OPLOG_FUNC09 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC09", sPath, "*")
    MSG_OPLOG_FUNC10 = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC10", sPath, "*")
    MSG_OPLOG_CLRTOTAL = GetProfileString_S("JAPANESE", "MSG_OPLOG_CLRTOTAL", sPath, "*")
    MSG_OPLOG_TRIMST = GetProfileString_S("JAPANESE", "MSG_OPLOG_TRIMST", sPath, "*")
    MSG_OPLOG_TRIMRES = GetProfileString_S("JAPANESE", "MSG_OPLOG_TRIMRES", sPath, "*")
    MSG_OPLOG_HCMD_ERRRST = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_ERRRST", sPath, "*")
    MSG_OPLOG_HCMD_PATCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_PATCMD", sPath, "*")
    MSG_OPLOG_HCMD_LASCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_LASCMD", sPath, "*")
    MSG_OPLOG_HCMD_MARKCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_MARKCMD", sPath, "*")
    MSG_OPLOG_HCMD_LODCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_LODCMD", sPath, "*")
    MSG_OPLOG_HCMD_TECCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_TECCMD", sPath, "*")
    MSG_OPLOG_HCMD_TRMCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_TRMCMD", sPath, "*")
    MSG_OPLOG_HCMD_LSTCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_LSTCMD", sPath, "*")
    MSG_OPLOG_HCMD_LENCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_LENCMD", sPath, "*")
    MSG_OPLOG_HCMD_MDAUTO = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_MDAUTO", sPath, "*")
    MSG_OPLOG_HCMD_MDMANU = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_MDMANU", sPath, "*")
    MSG_OPLOG_HCMD_CPCMD = GetProfileString_S("JAPANESE", "MSG_OPLOG_HCMD_CPCMD", sPath, "*")
    MSG_POPUP_MESSAGE = GetProfileString_S("JAPANESE", "MSG_POPUP_MESSAGE", sPath, "*")
    MSG_OPLOG_FUNC08S = GetProfileString_S("JAPANESE", "MSG_OPLOG_FUNC08S", sPath, "*")
    
    ' メイン画面ラベル
    MSG_MAIN_LABEL01 = GetProfileString_S("JAPANESE", "MSG_MAIN_LABEL01", sPath, "*")
    MSG_MAIN_LABEL02 = GetProfileString_S("JAPANESE", "MSG_MAIN_LABEL02", sPath, "*")
    
    ' [CIRCUIT]
    MSG_CIRCUIT_LABEL01 = GetProfileString_S("JAPANESE", "MSG_CIRCUIT_LABEL01", sPath, "*")
    MSG_CIRCUIT_LABEL02 = GetProfileString_S("JAPANESE", "MSG_CIRCUIT_LABEL02", sPath, "*")
    
    ' [registor]
    MSG_REGISTER_LABEL01 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL01", sPath, "*")
    MSG_REGISTER_LABEL02 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL02", sPath, "*")
    MSG_REGISTER_LABEL03 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL03", sPath, "*")
    MSG_REGISTER_LABEL04 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL04", sPath, "*")
    MSG_REGISTER_LABEL05 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL05", sPath, "*")
    MSG_REGISTER_LABEL06 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL06", sPath, "*")
    MSG_REGISTER_LABEL07 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL07", sPath, "*")
    MSG_REGISTER_LABEL08 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL08", sPath, "*")
    MSG_REGISTER_LABEL09 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL09", sPath, "*")
    MSG_REGISTER_LABEL10 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL10", sPath, "*")
    MSG_REGISTER_LABEL11 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL11", sPath, "*")
    MSG_REGISTER_LABEL12 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL12", sPath, "*")
    MSG_REGISTER_LABEL13 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL13", sPath, "*")
    MSG_REGISTER_LABEL14 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL14", sPath, "*")
    MSG_REGISTER_LABEL15 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL15", sPath, "*")
    MSG_REGISTER_LABEL16 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL16", sPath, "*")
    MSG_REGISTER_LABEL17 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL17", sPath, "*")
    MSG_REGISTER_LABEL18 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL18", sPath, "*")
    MSG_REGISTER_LABEL19 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL19", sPath, "*")
    MSG_REGISTER_LABEL20 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL20", sPath, "*")
    MSG_REGISTER_LABEL21 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL21", sPath, "*")
    MSG_REGISTER_LABEL22 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL22", sPath, "*")
    MSG_REGISTER_LABEL23 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL23", sPath, "*")
    MSG_REGISTER_LABEL24 = GetProfileString_S("JAPANESE", "MSG_REGISTER_LABEL24", sPath, "*")
    
    ' 編集　[cut]
    MSG_CUT_LABEL01 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL01", sPath, "*")
    MSG_CUT_LABEL02 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL02", sPath, "*")
    MSG_CUT_LABEL03 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL03", sPath, "*")
    MSG_CUT_LABEL04 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL04", sPath, "*")
    MSG_CUT_LABEL05 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL05", sPath, "*")
    MSG_CUT_LABEL06 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL06", sPath, "*")
    MSG_CUT_LABEL07 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL07", sPath, "*")
    MSG_CUT_LABEL08 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL08", sPath, "*")
    MSG_CUT_LABEL09 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL09", sPath, "*")
    MSG_CUT_LABEL10 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL10", sPath, "*")
    MSG_CUT_LABEL11 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL11", sPath, "*")
    MSG_CUT_LABEL12 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL12", sPath, "*")
    MSG_CUT_LABEL13 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL13", sPath, "*")
    MSG_CUT_LABEL14 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL14", sPath, "*")
    MSG_CUT_LABEL15 = GetProfileString_S("JAPANESE", "MSG_CUT_LABEL15", sPath, "*")
    
    ' ■ティーチング
    MSG_TEACH_LABEL01 = GetProfileString_S("JAPANESE", "MSG_TEACH_LABEL01", sPath, "*")
    MSG_TEACH_LABEL02 = GetProfileString_S("JAPANESE", "MSG_TEACH_LABEL02", sPath, "*")
    MSG_TEACH_LABEL03 = GetProfileString_S("JAPANESE", "MSG_TEACH_LABEL03", sPath, "*")
    MSG_TEACH_LABEL04 = GetProfileString_S("JAPANESE", "MSG_TEACH_LABEL04", sPath, "*")
    MSG_TEACH_LABEL05 = GetProfileString_S("JAPANESE", "MSG_TEACH_LABEL05", sPath, "*")

    ' ■プローブ位置合わせ
    MSG_PROBE_LABEL01 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL01", sPath, "*")
    MSG_PROBE_LABEL02 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL02", sPath, "*")
    MSG_PROBE_LABEL03 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL03", sPath, "*")
    MSG_PROBE_LABEL04 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL04", sPath, "*")
    MSG_PROBE_LABEL05 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL05", sPath, "*")
    MSG_PROBE_LABEL06 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL06", sPath, "*")
    MSG_PROBE_LABEL07 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL07", sPath, "*")
    MSG_PROBE_LABEL08 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL08", sPath, "*")
    MSG_PROBE_LABEL09 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL09", sPath, "*")
    MSG_PROBE_LABEL10 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL10", sPath, "*")
    MSG_PROBE_LABEL11 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL11", sPath, "*")
    MSG_PROBE_LABEL12 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL12", sPath, "*")
    MSG_PROBE_LABEL13 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL13", sPath, "*")
    MSG_PROBE_LABEL14 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL14", sPath, "*")
    MSG_PROBE_LABEL15 = GetProfileString_S("JAPANESE", "MSG_PROBE_LABEL15", sPath, "*")
  
    MSG_PROBE_MSG01 = GetProfileString_S("JAPANESE", "MSG_PROBE_MSG01", sPath, "*")
    MSG_PROBE_MSG02 = GetProfileString_S("JAPANESE", "MSG_PROBE_MSG02", sPath, "*")
    MSG_PROBE_MSG03 = GetProfileString_S("JAPANESE", "MSG_PROBE_MSG03", sPath, "*")
    MSG_PROBE_MSG04 = GetProfileString_S("JAPANESE", "MSG_PROBE_MSG04", sPath, "*")
    
    MSG_PROBE_ERR01 = GetProfileString_S("JAPANESE", "MSG_PROBE_ERR01", sPath, "*")
    
    ' ■frmMsgBox 画面終了確認
    MSG_CLOSE_LABEL01 = GetProfileString_S("JAPANESE", "MSG_CLOSE_LABEL01", sPath, "*")
    MSG_CLOSE_LABEL02 = GetProfileString_S("JAPANESE", "MSG_CLOSE_LABEL02", sPath, "*")
    MSG_CLOSE_LABEL03 = GetProfileString_S("JAPANESE", "MSG_CLOSE_LABEL03", sPath, "*")
    
    ' ■frmReset 原点復帰画面など
    MSG_FRMRESET_LABEL01 = GetProfileString_S("JAPANESE", "MSG_FRMRESET_LABEL01", sPath, "*")
    
    ' ■LASER_teaching
    MSG_LASER_LABEL01 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL01", sPath, "*")
    MSG_LASER_LABEL02 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL02", sPath, "*")
    MSG_LASER_LABEL03 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL03", sPath, "*")
    MSG_LASER_LABEL04 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL04", sPath, "*")
    MSG_LASER_LABEL05 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL05", sPath, "*")
    MSG_LASER_LABEL06 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL06", sPath, "*")
    MSG_LASER_LABEL07 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL07", sPath, "*")
    MSG_LASER_LABEL08 = GetProfileString_S("JAPANESE", "MSG_LASER_LABEL08", sPath, "*")

    ' 編集画面 カット
    LBL_CUT_PLATE_J1 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J1", sPath, "*")
    LBL_CUT_PLATE_J2 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J2", sPath, "*")
    LBL_CUT_PLATE_J3 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J3", sPath, "*")
    LBL_CUT_PLATE_J4 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J4", sPath, "*")
    LBL_CUT_PLATE_J5 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J5", sPath, "*")
    LBL_CUT_PLATE_J6 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J6", sPath, "*")
    LBL_CUT_PLATE_J7 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J7", sPath, "*")
    LBL_CUT_PLATE_J8 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J8", sPath, "*")
    LBL_CUT_PLATE_J9 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J9", sPath, "*")
    LBL_CUT_PLATE_J10 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J10", sPath, "*")
    LBL_CUT_PLATE_J11 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J11", sPath, "*")
    LBL_CUT_PLATE_J12 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J12", sPath, "*")
    LBL_CUT_PLATE_J13 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J13", sPath, "*")
    LBL_CUT_PLATE_J14 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J14", sPath, "*")
    LBL_CUT_PLATE_J15 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J15", sPath, "*")
    LBL_CUT_PLATE_J16 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J16", sPath, "*")
    LBL_CUT_PLATE_J17 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J17", sPath, "*")
    LBL_CUT_PLATE_J18 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J18", sPath, "*")
    LBL_CUT_PLATE_J19 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J19", sPath, "*")
    LBL_CUT_PLATE_J21 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J21", sPath, "*")
    LBL_CUT_PLATE_J22 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J22", sPath, "*")
    LBL_CUT_PLATE_J23 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J23", sPath, "*")
    LBL_CUT_PLATE_J24 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J24", sPath, "*")
    LBL_CUT_PLATE_J25 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J25", sPath, "*")
    LBL_CUT_PLATE_J26 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J26", sPath, "*")
    LBL_CUT_PLATE_J27 = GetProfileString_S("JAPANESE", "LBL_CUT_PLATE_J27", sPath, "*")
    MSG_CUT_1 = GetProfileString_S("JAPANESE", "MSG_CUT_1", sPath, "*")
    LBL_CUT_COPYLINE = GetProfileString_S("JAPANESE", "LBL_CUT_COPYLINE", sPath, "*")
    LBL_CUT_COPYCOLUMN = GetProfileString_S("JAPANESE", "LBL_CUT_COPYCOLUMN", sPath, "*")

    ' ■CorrectPos
    MSG_CORPOS_LABEL01 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL01", sPath, "*")
    MSG_CORPOS_LABEL02 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL02", sPath, "*")
    MSG_CORPOS_LABEL03 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL03", sPath, "*")
    MSG_CORPOS_LABEL04 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL04", sPath, "*")
    MSG_CORPOS_LABEL05 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL05", sPath, "*")
    MSG_CORPOS_LABEL06 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL06", sPath, "*")
    MSG_CORPOS_LABEL07 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL07", sPath, "*")
    MSG_CORPOS_LABEL08 = GetProfileString_S("JAPANESE", "MSG_CORPOS_LABEL08", sPath, "*")
    MSG_CORPOS_MSG01 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG01", sPath, "*")
    MSG_CORPOS_MSG02 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG02", sPath, "*")
    MSG_CORPOS_MSG03 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG03", sPath, "*")
    MSG_CORPOS_MSG04 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG04", sPath, "*")
    MSG_CORPOS_MSG05 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG05", sPath, "*")
    MSG_CORPOS_MSG06 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG06", sPath, "*")
    MSG_CORPOS_MSG07 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG07", sPath, "*")
    MSG_CORPOS_MSG08 = GetProfileString_S("JAPANESE", "MSG_CORPOS_MSG08", sPath, "*")

End Sub

'===============================================================================
'【機　能】 メッセージ設定(英語)
'【引　数】 sPath(INP) : 設定ファイル名
'【戻り値】 なし
'===============================================================================
Private Sub PrepareMessagesEnglish(sPath As String)

    ' エラーメッセージ
    MSG_0 = GetProfileString_S("ENGLISH", "MSG_0", sPath, "*")
    MSG_1 = GetProfileString_S("ENGLISH", "MSG_1", sPath, "*")
    MSG_2 = GetProfileString_S("ENGLISH", "MSG_2", sPath, "*")
    MSG_3 = GetProfileString_S("ENGLISH", "MSG_3", sPath, "*")
    MSG_4 = GetProfileString_S("ENGLISH", "MSG_4", sPath, "*")
    MSG_5 = GetProfileString_S("ENGLISH", "MSG_5", sPath, "*")
    MSG_6 = GetProfileString_S("ENGLISH", "MSG_6", sPath, "*")
    MSG_7 = GetProfileString_S("ENGLISH", "MSG_7", sPath, "*")
    MSG_8 = GetProfileString_S("ENGLISH", "MSG_8", sPath, "*")
    MSG_9 = GetProfileString_S("ENGLISH", "MSG_9", sPath, "*")
    MSG_10 = GetProfileString_S("ENGLISH", "MSG_10", sPath, "*")
    MSG_02 = GetProfileString_S("ENGLISH", "MSG_02", sPath, "*")
    MSG_03 = GetProfileString_S("ENGLISH", "MSG_03", sPath, "*")
    MSG_04 = GetProfileString_S("ENGLISH", "MSG_04", sPath, "*")
    MSG_COM = GetProfileString_S("ENGLISH", "MSG_COM", sPath, "*")
    MSG_PP19_1 = GetProfileString_S("ENGLISH", "MSG_PP19_1", sPath, "*")
    MSG_PP19_2 = GetProfileString_S("ENGLISH", "MSG_PP19_2", sPath, "*")
    MSG_11 = GetProfileString_S("ENGLISH", "MSG_11", sPath, "*")
    MSG_12 = GetProfileString_S("ENGLISH", "MSG_12", sPath, "*")
    MSG_14 = GetProfileString_S("ENGLISH", "MSG_14", sPath, "*")
    MSG_15 = GetProfileString_S("ENGLISH", "MSG_15", sPath, "*")
    MSG_16 = GetProfileString_S("ENGLISH", "MSG_16", sPath, "*")
    MSG_17 = GetProfileString_S("ENGLISH", "MSG_17", sPath, "*")
    MSG_18 = GetProfileString_S("ENGLISH", "MSG_18", sPath, "*")
    MSG_19 = GetProfileString_S("ENGLISH", "MSG_19", sPath, "*")
    MSG_20 = GetProfileString_S("ENGLISH", "MSG_20", sPath, "*")
    MSG_21 = GetProfileString_S("ENGLISH", "MSG_21", sPath, "*")
    MSG_22 = GetProfileString_S("ENGLISH", "MSG_22", sPath, "*")
    MSG_23 = GetProfileString_S("ENGLISH", "MSG_23", sPath, "*")
    MSG_24 = GetProfileString_S("ENGLISH", "MSG_24", sPath, "*")
    MSG_30 = GetProfileString_S("ENGLISH", "MSG_30", sPath, "*")
    MSG_31 = GetProfileString_S("ENGLISH", "MSG_31", sPath, "*")
    MSG_32 = GetProfileString_S("ENGLISH", "MSG_32", sPath, "*")
    MSG_33 = GetProfileString_S("ENGLISH", "MSG_33", sPath, "*")
    MSG_34 = GetProfileString_S("ENGLISH", "MSG_34", sPath, "*")
    MSG_35 = GetProfileString_S("ENGLISH", "MSG_35", sPath, "*")
    MSG_36 = GetProfileString_S("ENGLISH", "MSG_36", sPath, "*")
    MSG_37 = GetProfileString_S("ENGLISH", "MSG_37", sPath, "*")
    MSG_38 = GetProfileString_S("ENGLISH", "MSG_38", sPath, "*")
    MSG_39 = GetProfileString_S("ENGLISH", "MSG_39", sPath, "*")
    MSG_40 = GetProfileString_S("ENGLISH", "MSG_40", sPath, "*")
    MSG_41 = GetProfileString_S("ENGLISH", "MSG_41", sPath, "*")
    MSG_42 = GetProfileString_S("ENGLISH", "MSG_42", sPath, "*")
    MSG_43 = GetProfileString_S("ENGLISH", "MSG_43", sPath, "*")
    MSG_44 = GetProfileString_S("ENGLISH", "MSG_44", sPath, "*")
    MSG_45 = GetProfileString_S("ENGLISH", "MSG_45", sPath, "*")
    MSG_46 = GetProfileString_S("ENGLISH", "MSG_46", sPath, "*")
    MSG_47 = GetProfileString_S("ENGLISH", "MSG_47", sPath, "*")
    MSG_48 = GetProfileString_S("ENGLISH", "MSG_48", sPath, "*")
    MSG_49 = GetProfileString_S("ENGLISH", "MSG_49", sPath, "*")
    MSG_50 = GetProfileString_S("ENGLISH", "MSG_50", sPath, "*")
    MSG_51 = GetProfileString_S("ENGLISH", "MSG_51", sPath, "*")
    
    MSG_PP23 = GetProfileString_S("ENGLISH", "MSG_PP23", sPath, "*")
    MSG_PP24_1 = GetProfileString_S("ENGLISH", "MSG_PP24_1", sPath, "*")
    MSG_PP24_2 = GetProfileString_S("ENGLISH", "MSG_PP24_2", sPath, "*")
    MSG_25 = GetProfileString_S("ENGLISH", "MSG_25", sPath, "*")
    MSG_TOTAL_CIRCUIT = GetProfileString_S("ENGLISH", "MSG_TOTAL_CIRCUIT", sPath, "*")
    MSG_TOTAL_REGISTOR = GetProfileString_S("ENGLISH", "MSG_TOTAL_REGISTOR", sPath, "*")
    MSG_PR_COM1 = GetProfileString_S("ENGLISH", "MSG_PR_COM1", sPath, "*")
    MSG_PR04_1 = GetProfileString_S("ENGLISH", "MSG_PR04_1", sPath, "*")
    MSG_PR04_2 = GetProfileString_S("ENGLISH", "MSG_PR04_2", sPath, "*")
    MSG_PR04_3 = GetProfileString_S("ENGLISH", "MSG_PR04_3", sPath, "*")
    MSG_PR04_4 = GetProfileString_S("ENGLISH", "MSG_PR04_4", sPath, "*")
    MSG_PR07_1 = GetProfileString_S("ENGLISH", "MSG_PR07_1", sPath, "*")
    MSG_PR07_2 = GetProfileString_S("ENGLISH", "MSG_PR07_2", sPath, "*")
    MSG_PR08_1 = GetProfileString_S("ENGLISH", "MSG_PR08_1", sPath, "*")
    MSG_PR08_2 = GetProfileString_S("ENGLISH", "MSG_PR08_2", sPath, "*")
    MSG_PR08_3 = GetProfileString_S("ENGLISH", "MSG_PR08_3", sPath, "*")
    MSG_PR08_COM1 = GetProfileString_S("ENGLISH", "MSG_PR08_COM1", sPath, "*")
    MSG_100 = GetProfileString_S("ENGLISH", "MSG_100", sPath, "*")
    MSG_101 = GetProfileString_S("ENGLISH", "MSG_101", sPath, "*")
    MSG_102 = GetProfileString_S("ENGLISH", "MSG_102", sPath, "*")
    MSG_103 = GetProfileString_S("ENGLISH", "MSG_103", sPath, "*")
    MSG_104 = GetProfileString_S("ENGLISH", "MSG_104", sPath, "*")
    MSG_105 = GetProfileString_S("ENGLISH", "MSG_105", sPath, "*")
    MSG_106 = GetProfileString_S("ENGLISH", "MSG_106", sPath, "*")
    MSG_107 = GetProfileString_S("ENGLISH", "MSG_107", sPath, "*")
    MSG_108 = GetProfileString_S("ENGLISH", "MSG_108", sPath, "*")
    MSG_109 = GetProfileString_S("ENGLISH", "MSG_109", sPath, "*")
    MSG_110 = GetProfileString_S("ENGLISH", "MSG_110", sPath, "*")
    MSG_111 = GetProfileString_S("ENGLISH", "MSG_111", sPath, "*")
    MSG_112 = GetProfileString_S("ENGLISH", "MSG_112", sPath, "*")
    MSG_113 = GetProfileString_S("ENGLISH", "MSG_113", sPath, "*")
    MSG_114 = GetProfileString_S("ENGLISH", "MSG_114", sPath, "*")
    MSG_115 = GetProfileString_S("ENGLISH", "MSG_115", sPath, "*")

    ' TITLE
    TITLE_1 = GetProfileString_S("ENGLISH", "TITLE_1", sPath, "*")
    TITLE_2 = GetProfileString_S("ENGLISH", "TITLE_2", sPath, "*")
    TITLE_3 = GetProfileString_S("ENGLISH", "TITLE_3", sPath, "*")
    TITLE_4 = GetProfileString_S("ENGLISH", "TITLE_4", sPath, "*")
    TITLE_5 = GetProfileString_S("ENGLISH", "TITLE_5", sPath, "*")
    TITLE_6 = GetProfileString_S("ENGLISH", "TITLE_6", sPath, "*")
    TITLE_7 = GetProfileString_S("ENGLISH", "TITLE_7", sPath, "*")
    TITLE_8 = GetProfileString_S("ENGLISH", "TITLE_8", sPath, "*")
    TITLE_LASER = GetProfileString_S("ENGLISH", "TITLE_LASER", sPath, "*")
    TITLE_LOGGING = GetProfileString_S("ENGLISH", "TITLE_LOGGING", sPath, "*")
    
    ' ラベル
    LBL_PLATE_1 = GetProfileString_S("ENGLISH", "LBL_PLATE_1", sPath, "*")
    LBL_PLATE_2 = GetProfileString_S("ENGLISH", "LBL_PLATE_2", sPath, "*")
    LBL_PLATE_3 = GetProfileString_S("ENGLISH", "LBL_PLATE_3", sPath, "*")
    LBL_PLATE_4 = GetProfileString_S("ENGLISH", "LBL_PLATE_4", sPath, "*")
    LBL_PLATE_5 = GetProfileString_S("ENGLISH", "LBL_PLATE_5", sPath, "*")
    LBL_PLATE_6 = GetProfileString_S("ENGLISH", "LBL_PLATE_6", sPath, "*")
    LBL_PLATE_7 = GetProfileString_S("ENGLISH", "LBL_PLATE_7", sPath, "*")
    LBL_PLATE_8 = GetProfileString_S("ENGLISH", "LBL_PLATE_8", sPath, "*")
    LBL_PLATE_9 = GetProfileString_S("ENGLISH", "LBL_PLATE_9", sPath, "*")
    LBL_PLATE_10 = GetProfileString_S("ENGLISH", "LBL_PLATE_10", sPath, "*")
    LBL_PLATE_11 = GetProfileString_S("ENGLISH", "LBL_PLATE_11", sPath, "*")
    LBL_PLATE_12 = GetProfileString_S("ENGLISH", "LBL_PLATE_12", sPath, "*")
    LBL_PLATE_13 = GetProfileString_S("ENGLISH", "LBL_PLATE_13", sPath, "*")
    LBL_PLATE_14 = GetProfileString_S("ENGLISH", "LBL_PLATE_14", sPath, "*")
    LBL_PLATE_15 = GetProfileString_S("ENGLISH", "LBL_PLATE_15", sPath, "*")
    LBL_PLATE_16 = GetProfileString_S("ENGLISH", "LBL_PLATE_16", sPath, "*")
    LBL_PLATE_17 = GetProfileString_S("ENGLISH", "LBL_PLATE_17", sPath, "*")
    LBL_PLATE_18 = GetProfileString_S("ENGLISH", "LBL_PLATE_18", sPath, "*")
    LBL_PLATE_19 = GetProfileString_S("ENGLISH", "LBL_PLATE_19", sPath, "*")
    LBL_PLATE_20 = GetProfileString_S("ENGLISH", "LBL_PLATE_20", sPath, "*")
    LBL_PLATE_21 = GetProfileString_S("ENGLISH", "LBL_PLATE_21", sPath, "*")
    LBL_PLATE_22 = GetProfileString_S("ENGLISH", "LBL_PLATE_22", sPath, "*")
    LBL_PLATE_23 = GetProfileString_S("ENGLISH", "LBL_PLATE_23", sPath, "*")
    LBL_PLATE_24 = GetProfileString_S("ENGLISH", "LBL_PLATE_24", sPath, "*")
    LBL_PLATE_25 = GetProfileString_S("ENGLISH", "LBL_PLATE_25", sPath, "*")
    LBL_PLATE_26 = GetProfileString_S("ENGLISH", "LBL_PLATE_26", sPath, "*")
    LBL_PLATE_27 = GetProfileString_S("ENGLISH", "LBL_PLATE_27", sPath, "*")
    LBL_PLATE_28 = GetProfileString_S("ENGLISH", "LBL_PLATE_28", sPath, "*")
    LBL_PLATE_29 = GetProfileString_S("ENGLISH", "LBL_PLATE_29", sPath, "*")

    'teaching
    LBL_TEACHING_001 = GetProfileString_S("ENGLISH", "LBL_TEACHING_001", sPath, "*")
    LBL_TEACHING_002 = GetProfileString_S("ENGLISH", "LBL_TEACHING_002", sPath, "*")
    LBL_TEACHING_003 = GetProfileString_S("ENGLISH", "LBL_TEACHING_003", sPath, "*")
    LBL_TEACHING_004 = GetProfileString_S("ENGLISH", "LBL_TEACHING_004", sPath, "*")
    LBL_TEACHING_005 = GetProfileString_S("ENGLISH", "LBL_TEACHING_005", sPath, "*")
    LBL_TEACHING_006 = GetProfileString_S("ENGLISH", "LBL_TEACHING_006", sPath, "*")
    LBL_TEACHING_007 = GetProfileString_S("ENGLISH", "LBL_TEACHING_007", sPath, "*")
    LBL_TEACHING_008 = GetProfileString_S("ENGLISH", "LBL_TEACHING_008", sPath, "*")
    LBL_TEACHING_009 = GetProfileString_S("ENGLISH", "LBL_TEACHING_009", sPath, "*")
    LBL_TEACHING_010 = GetProfileString_S("ENGLISH", "LBL_TEACHING_010", sPath, "*")
    LBL_TEACHING_011 = GetProfileString_S("ENGLISH", "LBL_TEACHING_011", sPath, "*")
    
    'cut pos teaching
    LBL_CUTPOSTEACH_001 = GetProfileString_S("ENGLISH", "LBL_CUTPOSTEACH_001", sPath, "*")
    LBL_CUTPOSTEACH_002 = GetProfileString_S("ENGLISH", "LBL_CUTPOSTEACH_002", sPath, "*")
    LBL_CUTPOSTEACH_003 = GetProfileString_S("ENGLISH", "LBL_CUTPOSTEACH_003", sPath, "*")
    LBL_CUTPOSTEACH_004 = GetProfileString_S("ENGLISH", "LBL_CUTPOSTEACH_004", sPath, "*")
    LBL_CUTPOSTEACH_005 = GetProfileString_S("ENGLISH", "LBL_CUTPOSTEACH_005", sPath, "*")
   
    'recog
    LBL_RECOG_001 = GetProfileString_S("ENGLISH", "LBL_RECOG_001", sPath, "*")
    LBL_RECOG_002 = GetProfileString_S("ENGLISH", "LBL_RECOG_002", sPath, "*")
    LBL_RECOG_003 = GetProfileString_S("ENGLISH", "LBL_RECOG_003", sPath, "*")
    LBL_RECOG_004 = GetProfileString_S("ENGLISH", "LBL_RECOG_004", sPath, "*")
    LBL_RECOG_005 = GetProfileString_S("ENGLISH", "LBL_RECOG_005", sPath, "*")
    LBL_RECOG_006 = GetProfileString_S("ENGLISH", "LBL_RECOG_006", sPath, "*")
    LBL_RECOG_007 = GetProfileString_S("ENGLISH", "LBL_RECOG_007", sPath, "*")
    LBL_RECOG_008 = GetProfileString_S("ENGLISH", "LBL_RECOG_008", sPath, "*")
    LBL_RECOG_009 = GetProfileString_S("ENGLISH", "LBL_RECOG_009", sPath, "*")
    LBL_RECOG_010 = GetProfileString_S("ENGLISH", "LBL_RECOG_010", sPath, "*")
    LBL_RECOG_011 = GetProfileString_S("ENGLISH", "LBL_RECOG_011", sPath, "*")
    LBL_RECOG_012 = GetProfileString_S("ENGLISH", "LBL_RECOG_012", sPath, "*")
    LBL_RECOG_013 = GetProfileString_S("ENGLISH", "LBL_RECOG_013", sPath, "*")
    LBL_RECOG_014 = GetProfileString_S("ENGLISH", "LBL_RECOG_014", sPath, "*")
    LBL_RECOG_015 = GetProfileString_S("ENGLISH", "LBL_RECOG_015", sPath, "*")
    LBL_RECOG_016 = GetProfileString_S("ENGLISH", "LBL_RECOG_016", sPath, "*")
    LBL_RECOG_017 = GetProfileString_S("ENGLISH", "LBL_RECOG_017", sPath, "*")
    LBL_RECOG_018_1 = GetProfileString_S("ENGLISH", "LBL_RECOG_018_1", sPath, "*")
    LBL_RECOG_018_2 = GetProfileString_S("ENGLISH", "LBL_RECOG_018_2", sPath, "*")
    LBL_RECOG_019 = GetProfileString_S("ENGLISH", "LBL_RECOG_019", sPath, "*")
    LBL_RECOG_020 = GetProfileString_S("ENGLISH", "LBL_RECOG_020", sPath, "*")
    LBL_RECOG_021 = GetProfileString_S("ENGLISH", "LBL_RECOG_021", sPath, "*")
    LBL_RECOG_022 = GetProfileString_S("ENGLISH", "LBL_RECOG_022", sPath, "*")
    LBL_RECOG_023 = GetProfileString_S("ENGLISH", "LBL_RECOG_023", sPath, "*")
    LBL_RECOG_024 = GetProfileString_S("ENGLISH", "LBL_RECOG_024", sPath, "*")
    LBL_RECOG_025 = GetProfileString_S("ENGLISH", "LBL_RECOG_025", sPath, "*")
    LBL_RECOG_026 = GetProfileString_S("ENGLISH", "LBL_RECOG_026", sPath, "*")
    LBL_RECOG_027 = GetProfileString_S("ENGLISH", "LBL_RECOG_027", sPath, "*")
    LBL_RECOG_028 = GetProfileString_S("ENGLISH", "LBL_RECOG_028", sPath, "*")
    LBL_RECOG_029 = GetProfileString_S("ENGLISH", "LBL_RECOG_029", sPath, "*")
    LBL_RECOG_030 = GetProfileString_S("ENGLISH", "LBL_RECOG_030", sPath, "*")
    LBL_RECOG_031 = GetProfileString_S("ENGLISH", "LBL_RECOG_031", sPath, "*")
    LBL_RECOG_032 = GetProfileString_S("ENGLISH", "LBL_RECOG_032", sPath, "*")
    LBL_RECOG_033 = GetProfileString_S("ENGLISH", "LBL_RECOG_033", sPath, "*")
    LBL_RECOG_034 = GetProfileString_S("ENGLISH", "LBL_RECOG_034", sPath, "*")
    LBL_RECOG_035 = GetProfileString_S("ENGLISH", "LBL_RECOG_035", sPath, "*")
    LBL_RECOG_036 = GetProfileString_S("ENGLISH", "LBL_RECOG_036", sPath, "*")
    LBL_RECOG_037 = GetProfileString_S("ENGLISH", "LBL_RECOG_037", sPath, "*")
    LBL_RECOG_038 = GetProfileString_S("ENGLISH", "LBL_RECOG_038", sPath, "*")
    LBL_RECOG_039 = GetProfileString_S("ENGLISH", "LBL_RECOG_039", sPath, "*")
    LBL_RECOG_040 = GetProfileString_S("ENGLISH", "LBL_RECOG_040", sPath, "*")
    LBL_RECOG_041 = GetProfileString_S("ENGLISH", "LBL_RECOG_041", sPath, "*")
    LBL_RECOG_042 = GetProfileString_S("ENGLISH", "LBL_RECOG_042", sPath, "*")
    LBL_RECOG_043 = GetProfileString_S("ENGLISH", "LBL_RECOG_043", sPath, "*")
    LBL_RECOG_044 = GetProfileString_S("ENGLISH", "LBL_RECOG_044", sPath, "*")
    LBL_RECOG_045 = GetProfileString_S("ENGLISH", "LBL_RECOG_045", sPath, "*")
    
    LBL_RECOG_100 = GetProfileString_S("ENGLISH", "LBL_RECOG_100", sPath, "*")
    LBL_RECOG_101 = GetProfileString_S("ENGLISH", "LBL_RECOG_101", sPath, "*")
    LBL_RECOG_102 = GetProfileString_S("ENGLISH", "LBL_RECOG_102", sPath, "*")
    LBL_RECOG_103 = GetProfileString_S("ENGLISH", "LBL_RECOG_103", sPath, "*")
   
    ' スプラッシュメッセージ
    MSG_SPRASH0 = GetProfileString_S("ENGLISH", "MSG_SPRASH0", sPath, "*")
    MSG_SPRASH1 = GetProfileString_S("ENGLISH", "MSG_SPRASH1", sPath, "*")
    MSG_SPRASH2 = GetProfileString_S("ENGLISH", "MSG_SPRASH2", sPath, "*")
    MSG_SPRASH3 = GetProfileString_S("ENGLISH", "MSG_SPRASH3", sPath, "*")
    MSG_SPRASH4 = GetProfileString_S("ENGLISH", "MSG_SPRASH4", sPath, "*")
    MSG_SPRASH5 = GetProfileString_S("ENGLISH", "MSG_SPRASH5", sPath, "*")
    MSG_SPRASH6 = GetProfileString_S("ENGLISH", "MSG_SPRASH6", sPath, "*")
    MSG_SPRASH7 = GetProfileString_S("ENGLISH", "MSG_SPRASH7", sPath, "*")
    MSG_SPRASH8 = GetProfileString_S("ENGLISH", "MSG_SPRASH8", sPath, "*")
    MSG_SPRASH9 = GetProfileString_S("ENGLISH", "MSG_SPRASH9", sPath, "*")
    MSG_SPRASH10 = GetProfileString_S("ENGLISH", "MSG_SPRASH10", sPath, "*")
    MSG_SPRASH11 = GetProfileString_S("ENGLISH", "MSG_SPRASH11", sPath, "*")
    MSG_SPRASH12 = GetProfileString_S("ENGLISH", "MSG_SPRASH12", sPath, "*")
    MSG_SPRASH13 = GetProfileString_S("ENGLISH", "MSG_SPRASH13", sPath, "*")
    MSG_SPRASH14 = GetProfileString_S("ENGLISH", "MSG_SPRASH14", sPath, "*")
    MSG_SPRASH15 = GetProfileString_S("ENGLISH", "MSG_SPRASH15", sPath, "*")
    MSG_SPRASH16 = GetProfileString_S("ENGLISH", "MSG_SPRASH16", sPath, "*")
    MSG_SPRASH17 = GetProfileString_S("ENGLISH", "MSG_SPRASH17", sPath, "*")
    MSG_SPRASH18 = GetProfileString_S("ENGLISH", "MSG_SPRASH18", sPath, "*")
    MSG_SPRASH19 = GetProfileString_S("ENGLISH", "MSG_SPRASH19", sPath, "*")
    MSG_SPRASH20 = GetProfileString_S("ENGLISH", "MSG_SPRASH20", sPath, "*")
    MSG_SPRASH21 = GetProfileString_S("ENGLISH", "MSG_SPRASH21", sPath, "*")
    MSG_SPRASH22 = GetProfileString_S("ENGLISH", "MSG_SPRASH22", sPath, "*")
    MSG_SPRASH23 = GetProfileString_S("ENGLISH", "MSG_SPRASH23", sPath, "*")
    MSG_SPRASH24 = GetProfileString_S("ENGLISH", "MSG_SPRASH24", sPath, "*")
    MSG_SPRASH25 = GetProfileString_S("ENGLISH", "MSG_SPRASH25", sPath, "*")
    MSG_SPRASH26 = GetProfileString_S("ENGLISH", "MSG_SPRASH26", sPath, "*")
    MSG_SPRASH27 = GetProfileString_S("ENGLISH", "MSG_SPRASH27", sPath, "*")
    MSG_SPRASH28 = GetProfileString_S("ENGLISH", "MSG_SPRASH28", sPath, "*")
    MSG_SPRASH29 = GetProfileString_S("ENGLISH", "MSG_SPRASH29", sPath, "*")
    MSG_SPRASH30 = GetProfileString_S("ENGLISH", "MSG_SPRASH30", sPath, "*")

    '----- INtime側エラーメッセージ(For OcxSystem) -----
      '----- V8.0.0.15③↓ -----
    MSG_CLAMP_MOVE_TIMEOUT = GetProfileString_S("ENGLISH", "MSG_CLAMP_MOVE_TIMEOUT", sPath, "*")
    '----- V8.0.0.15③↑ -----
    MSG_SRV_ALM = GetProfileString_S("ENGLISH", "MSG_SRV_ALM", sPath, "*")
    MSG_AXS_LIM_X = GetProfileString_S("ENGLISH", "MSG_AXS_LIM_X", sPath, "*")
    MSG_AXS_LIM_Y = GetProfileString_S("ENGLISH", "MSG_AXS_LIM_Y", sPath, "*")
    MSG_AXS_LIM_Z = GetProfileString_S("ENGLISH", "MSG_AXS_LIM_Z", sPath, "*")
    MSG_AXS_LIM_T = GetProfileString_S("ENGLISH", "MSG_AXS_LIM_T", sPath, "*")
    MSG_AXS_LIM_ATT = GetProfileString_S("ENGLISH", "MSG_AXS_LIM_ATT", sPath, "*")
    MSG_AXS_LIM_Z2 = GetProfileString_S("ENGLISH", "MSG_AXS_LIM_Z2", sPath, "*")
    MSG_OPN_CVR = GetProfileString_S("ENGLISH", "MSG_OPN_CVR", sPath, "*")
    MSG_OPN_SCVR = GetProfileString_S("ENGLISH", "MSG_OPN_SCVR", sPath, "*")
    MSG_OPN_CVRLTC = GetProfileString_S("ENGLISH", "MSG_OPN_CVRLTC", sPath, "*")
    MSG_AXIS_X_SERVO_ALM = GetProfileString_S("ENGLISH", "MSG_AXIS_X_SERVO_ALM", sPath, "*")
    MSG_AXIS_Y_SERVO_ALM = GetProfileString_S("ENGLISH", "MSG_AXIS_Y_SERVO_ALM", sPath, "*")
    MSG_AXIS_Z_SERVO_ALM = GetProfileString_S("ENGLISH", "MSG_AXIS_Z_SERVO_ALM", sPath, "*")
    MSG_AXIS_T_SERVO_ALM = GetProfileString_S("ENGLISH", "MSG_AXIS_T_SERVO_ALM", sPath, "*")
    
    ' 軸系エラー(タイムアウト)
    MSG_TIMEOUT_AXIS_X = GetProfileString_S("ENGLISH", "MSG_TIMEOUT_AXIS_X", sPath, "*")
    MSG_TIMEOUT_AXIS_Y = GetProfileString_S("ENGLISH", "MSG_TIMEOUT_AXIS_Y", sPath, "*")
    MSG_TIMEOUT_AXIS_Z = GetProfileString_S("ENGLISH", "MSG_TIMEOUT_AXIS_Z", sPath, "*")
    MSG_TIMEOUT_AXIS_T = GetProfileString_S("ENGLISH", "MSG_TIMEOUT_AXIS_T", sPath, "*")
    MSG_TIMEOUT_AXIS_Z2 = GetProfileString_S("ENGLISH", "MSG_TIMEOUT_AXIS_Z2", sPath, "*")
    MSG_TIMEOUT_ATT = GetProfileString_S("ENGLISH", "MSG_TIMEOUT_ATT", sPath, "*")
    MSG_TIMEOUT_AXIS_XY = GetProfileString_S("ENGLISH", "MSG_TIMEOUT_AXIS_XY", sPath, "*")
    
    ' 軸系エラー(プラスリミットオーバー)
    MSG_STG_SOFTLMT_PLUS_AXIS_X = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_PLUS_AXIS_X", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_Y = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_PLUS_AXIS_Y", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_Z = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_PLUS_AXIS_Z", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_T = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_PLUS_AXIS_T", sPath, "*")
    MSG_STG_SOFTLMT_PLUS_AXIS_Z2 = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_PLUS_AXIS_Z2", sPath, "*")
    MSG_STG_SOFTLMT_PLUS = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_PLUS", sPath, "*")

    ' 軸系エラー(マイナスリミットオーバー)
    MSG_STG_SOFTLMT_MINUS_AXIS_X = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_MINUS_AXIS_X", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_Y = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_MINUS_AXIS_Y", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_Z = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_MINUS_AXIS_Z", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_T = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_MINUS_AXIS_T", sPath, "*")
    MSG_STG_SOFTLMT_MINUS_AXIS_Z2 = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_MINUS_AXIS_Z2", sPath, "*")
    MSG_STG_SOFTLMT_MINUS = GetProfileString_S("ENGLISH", "MSG_STG_SOFTLMT_MINUS", sPath, "*")
    
    ' BP系エラー
    MSG_BP_MOVE_TIMEOUT = GetProfileString_S("ENGLISH", "MSG_BP_MOVE_TIMEOUT", sPath, "*")
    MSG_BP_GRV_ALARM_X = GetProfileString_S("ENGLISH", "MSG_BP_GRV_ALARM_X", sPath, "*")
    MSG_BP_GRV_ALARM_Y = GetProfileString_S("ENGLISH", "MSG_BP_GRV_ALARM_Y", sPath, "*")
    MSG_BP_HARD_LIMITOVER_LO = GetProfileString_S("ENGLISH", "MSG_BP_HARD_LIMITOVER_LO", sPath, "*")
    MSG_BP_HARD_LIMITOVER_HI = GetProfileString_S("ENGLISH", "MSG_BP_HARD_LIMITOVER_HI", sPath, "*")
    MSG_BP_LIMITOVER = GetProfileString_S("ENGLISH", "MSG_BP_LIMITOVER", sPath, "*")
    MSG_BP_SOFT_LIMITOVER = GetProfileString_S("ENGLISH", "MSG_BP_SOFT_LIMITOVER", sPath, "*")
    MSG_BP_BSIZE_OVER = GetProfileString_S("ENGLISH", "MSG_BP_BSIZE_OVER", sPath, "*")
    
    '----- ###806②↓ -----
    ' GPIB系エラー
    MSG_GPIB_PARAM = GetProfileString_S("ENGLISH", "MSG_GPIB_PARAM", sPath, "*")
    MSG_GPIB_TCPSOCKET = GetProfileString_S("ENGLISH", "MSG_GPIB_TCPSOCKET", sPath, "*")
    MSG_GPIB_EXEC = GetProfileString_S("ENGLISH", "MSG_GPIB_EXEC", sPath, "*")
    '----- ###806②↑ -----
   
    MSG_INTIME_ERROR = GetProfileString_S("ENGLISH", "MSG_INTIME_ERROR", sPath, "*")


    '----- 自動ﾚｰｻﾞﾊﾟﾜｰ調整用 -----
    MSG_AUTOLASER_01 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_01", sPath, "*")
    MSG_AUTOLASER_02 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_02", sPath, "*")
    MSG_AUTOLASER_03 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_03", sPath, "*")
    MSG_AUTOLASER_04 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_04", sPath, "*")
    MSG_AUTOLASER_05 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_05", sPath, "*")
    MSG_AUTOLASER_06 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_06", sPath, "*")
    MSG_AUTOLASER_07 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_07", sPath, "*")
    MSG_AUTOLASER_08 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_08", sPath, "*")
    MSG_AUTOLASER_09 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_09", sPath, "*")
    MSG_AUTOLASER_10 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_10", sPath, "*")
    MSG_AUTOLASER_11 = GetProfileString_S("ENGLISH", "MSG_AUTOLASER_11", sPath, "*")

    '----- ローダー関連 -----
    MSG_LOADER_01 = GetProfileString_S("ENGLISH", "MSG_LOADER_01", sPath, "*")
    MSG_LOADER_02 = GetProfileString_S("ENGLISH", "MSG_LOADER_02", sPath, "*")
    MSG_LOADER_03 = GetProfileString_S("ENGLISH", "MSG_LOADER_03", sPath, "*")
    MSG_LOADER_04 = GetProfileString_S("ENGLISH", "MSG_LOADER_04", sPath, "*")
    MSG_LOADER_05 = GetProfileString_S("ENGLISH", "MSG_LOADER_05", sPath, "*")
    MSG_LOADER_06 = GetProfileString_S("ENGLISH", "MSG_LOADER_06", sPath, "*")
    MSG_LOADER_07 = GetProfileString_S("ENGLISH", "MSG_LOADER_07", sPath, "*")      ' ###805②
    MSG_LOADER_08 = GetProfileString_S("ENGLISH", "MSG_LOADER_08", sPath, "*")      ' ###805②
    
    '----- その他 -----
    MSG_ERROR_01 = GetProfileString_S("ENGLISH", "MSG_ERROR_01", sPath, "*")
    MSG_ERROR_02 = GetProfileString_S("ENGLISH", "MSG_ERROR_02", sPath, "*")

    '----- limit.frm用 -----
    MSG_frmLimit_01 = GetProfileString_S("ENGLISH", "MSG_frmLimit_01", sPath, "*")
    MSG_frmLimit_02 = GetProfileString_S("ENGLISH", "MSG_frmLimit_02", sPath, "*")
    MSG_frmLimit_03 = GetProfileString_S("ENGLISH", "MSG_frmLimit_03", sPath, "*")
    MSG_frmLimit_04 = GetProfileString_S("ENGLISH", "MSG_frmLimit_04", sPath, "*")
    MSG_frmLimit_05 = GetProfileString_S("ENGLISH", "MSG_frmLimit_05", sPath, "*")
    MSG_frmLimit_06 = GetProfileString_S("ENGLISH", "MSG_frmLimit_06", sPath, "*")
    MSG_frmLimit_07 = GetProfileString_S("ENGLISH", "MSG_frmLimit_07", sPath, "*")
    MSG_frmLimit_08 = GetProfileString_S("ENGLISH", "MSG_frmLimit_08", sPath, "*")  '###801①

    '----- Logging()用 -----
    MSG_OpLog_TTL = GetProfileString_S("ENGLISH", "MSG_OpLog_TTL", sPath, "*")
    MSG_OpLog_01 = GetProfileString_S("ENGLISH", "MSG_OpLog_01", sPath, "*")

    ' ステータスバー
    CONST_PP01 = GetProfileString_S("ENGLISH", "CONST_PP01", sPath, "*")
    CONST_PP02 = GetProfileString_S("ENGLISH", "CONST_PP02", sPath, "*")
    CONST_PP03 = GetProfileString_S("ENGLISH", "CONST_PP03", sPath, "*")
    CONST_PP04X = GetProfileString_S("ENGLISH", "CONST_PP04X", sPath, "*")
    CONST_PP04Y = GetProfileString_S("ENGLISH", "CONST_PP04Y", sPath, "*")
    CONST_PP05X = GetProfileString_S("ENGLISH", "CONST_PP05X", sPath, "*")
    CONST_PP05Y = GetProfileString_S("ENGLISH", "CONST_PP05Y", sPath, "*")
    CONST_PP06X = GetProfileString_S("ENGLISH", "CONST_PP06X", sPath, "*")
    CONST_PP06Y = GetProfileString_S("ENGLISH", "CONST_PP06Y", sPath, "*")
    CONST_PP07X = GetProfileString_S("ENGLISH", "CONST_PP07X", sPath, "*")
    CONST_PP07Y = GetProfileString_S("ENGLISH", "CONST_PP07Y", sPath, "*")
    CONST_PP08X = GetProfileString_S("ENGLISH", "CONST_PP08X", sPath, "*")
    CONST_PP08Y = GetProfileString_S("ENGLISH", "CONST_PP08Y", sPath, "*")
    CONST_PP09X = GetProfileString_S("ENGLISH", "CONST_PP09X", sPath, "*")
    CONST_PP09Y = GetProfileString_S("ENGLISH", "CONST_PP09Y", sPath, "*")
    CONST_PP10X = GetProfileString_S("ENGLISH", "CONST_PP10X", sPath, "*")
    CONST_PP10Y = GetProfileString_S("ENGLISH", "CONST_PP10Y", sPath, "*")
    CONST_PP11 = GetProfileString_S("ENGLISH", "CONST_PP11", sPath, "*")
    CONST_PP12 = GetProfileString_S("ENGLISH", "CONST_PP12", sPath, "*")
    CONST_PP13 = GetProfileString_S("ENGLISH", "CONST_PP13", sPath, "*")
    CONST_PP14 = GetProfileString_S("ENGLISH", "CONST_PP14", sPath, "*")
    CONST_PP15 = GetProfileString_S("ENGLISH", "CONST_PP15", sPath, "*")
    CONST_PP16 = GetProfileString_S("ENGLISH", "CONST_PP16", sPath, "*")
    CONST_PP17 = GetProfileString_S("ENGLISH", "CONST_PP17", sPath, "*")
    CONST_PP18 = GetProfileString_S("ENGLISH", "CONST_PP18", sPath, "*")
    CONST_PP30 = GetProfileString_S("ENGLISH", "CONST_PP30", sPath, "*")
    CONST_PP31 = GetProfileString_S("ENGLISH", "CONST_PP31", sPath, "*")
    CONST_PP32X = GetProfileString_S("ENGLISH", "CONST_PP32X", sPath, "*")
    CONST_PP32Y = GetProfileString_S("ENGLISH", "CONST_PP32Y", sPath, "*")
    CONST_PP33X = GetProfileString_S("ENGLISH", "CONST_PP33X", sPath, "*")
    CONST_PP33Y = GetProfileString_S("ENGLISH", "CONST_PP33Y", sPath, "*")
    CONST_PP34X = GetProfileString_S("ENGLISH", "CONST_PP34X", sPath, "*")
    CONST_PP34Y = GetProfileString_S("ENGLISH", "CONST_PP34Y", sPath, "*")
    CONST_PP35 = GetProfileString_S("ENGLISH", "CONST_PP35", sPath, "*")
    CONST_PP36X = GetProfileString_S("ENGLISH", "CONST_PP36X", sPath, "*")
    CONST_PP36Y = GetProfileString_S("ENGLISH", "CONST_PP36Y", sPath, "*")
    CONST_PP371 = GetProfileString_S("ENGLISH", "CONST_PP371", sPath, "*")
    CONST_PP372 = GetProfileString_S("ENGLISH", "CONST_PP372", sPath, "*")
    CONST_PP381 = GetProfileString_S("ENGLISH", "CONST_PP381", sPath, "*")
    CONST_P41 = GetProfileString_S("ENGLISH", "CONST_P41", sPath, "*")
    CONST_P42 = GetProfileString_S("ENGLISH", "CONST_P42", sPath, "*")

    CONST_PP60 = GetProfileString_S("ENGLISH", "CONST_PP60", sPath, "*")
    CONST_PP70 = GetProfileString_S("ENGLISH", "CONST_PP70", sPath, "*")
    CONST_IPX = GetProfileString_S("ENGLISH", "CONST_IPX", sPath, "*")
    CONST_IPY = GetProfileString_S("ENGLISH", "CONST_IPY", sPath, "*")
    CONST_PR1 = GetProfileString_S("ENGLISH", "CONST_PR1", sPath, "*")
    CONST_PR2 = GetProfileString_S("ENGLISH", "CONST_PR2", sPath, "*")
    CONST_PR3 = GetProfileString_S("ENGLISH", "CONST_PR3", sPath, "*")
    CONST_PR4H = GetProfileString_S("ENGLISH", "CONST_PR4H", sPath, "*")
    CONST_PR4L = GetProfileString_S("ENGLISH", "CONST_PR4L", sPath, "*")
    CONST_PR4G = GetProfileString_S("ENGLISH", "CONST_PR4G", sPath, "*")
    CONST_PR5 = GetProfileString_S("ENGLISH", "CONST_PR5", sPath, "*")
    CONST_PR6 = GetProfileString_S("ENGLISH", "CONST_PR6", sPath, "*")
    CONST_PR7 = GetProfileString_S("ENGLISH", "CONST_PR7", sPath, "*")
    CONST_PR8 = GetProfileString_S("ENGLISH", "CONST_PR8", sPath, "*")
    CONST_PR9 = GetProfileString_S("ENGLISH", "CONST_PR9", sPath, "*")
    CONST_PR10 = GetProfileString_S("ENGLISH", "CONST_PR10", sPath, "*")
    CONST_PR11H = GetProfileString_S("ENGLISH", "CONST_PR11H", sPath, "*")
    CONST_PR11L = GetProfileString_S("ENGLISH", "CONST_PR11L", sPath, "*")
    CONST_PR12H = GetProfileString_S("ENGLISH", "CONST_PR12H", sPath, "*")
    CONST_PR12L = GetProfileString_S("ENGLISH", "CONST_PR12L", sPath, "*")
    CONST_PR13 = GetProfileString_S("ENGLISH", "CONST_PR13", sPath, "*")
    CONST_PR14 = GetProfileString_S("ENGLISH", "CONST_PR14", sPath, "*")
    CONST_PR15 = GetProfileString_S("ENGLISH", "CONST_PR15", sPath, "*")
    CONST_PR16 = GetProfileString_S("ENGLISH", "CONST_PR16", sPath, "*")
    CONST_PR17X = GetProfileString_S("ENGLISH", "CONST_PR17X", sPath, "*")
    CONST_PR17Y = GetProfileString_S("ENGLISH", "CONST_PR17Y", sPath, "*")
    CONST_PR18 = GetProfileString_S("ENGLISH", "CONST_PR18", sPath, "*")
    CONST_CP2 = GetProfileString_S("ENGLISH", "CONST_CP2", sPath, "*")
    CONST_CP4X = GetProfileString_S("ENGLISH", "CONST_CP4X", sPath, "*")
    CONST_CP4Y = GetProfileString_S("ENGLISH", "CONST_CP4Y", sPath, "*")
    CONST_CP5 = GetProfileString_S("ENGLISH", "CONST_CP5", sPath, "*")
    CONST_CP5_2 = GetProfileString_S("ENGLISH", "CONST_CP5_2", sPath, "*")
    CONST_CP6 = GetProfileString_S("ENGLISH", "CONST_CP6", sPath, "*")
    CONST_CP6_2 = GetProfileString_S("ENGLISH", "CONST_CP6_2", sPath, "*")
    CONST_CP7 = GetProfileString_S("ENGLISH", "CONST_CP7", sPath, "*")
    CONST_CP7_1 = GetProfileString_S("ENGLISH", "CONST_CP7_1", sPath, "*")
    CONST_CP9 = GetProfileString_S("ENGLISH", "CONST_CP9", sPath, "*")
    CONST_CP11 = GetProfileString_S("ENGLISH", "CONST_CP11", sPath, "*")
    CONST_CP12 = GetProfileString_S("ENGLISH", "CONST_CP12", sPath, "*")
    CONST_CP13 = GetProfileString_S("ENGLISH", "CONST_CP13", sPath, "*")
    CONST_CP14 = GetProfileString_S("ENGLISH", "CONST_CP14", sPath, "*")
    CONST_CP15 = GetProfileString_S("ENGLISH", "CONST_CP15", sPath, "*")
    CONST_CP17 = GetProfileString_S("ENGLISH", "CONST_CP17", sPath, "*")
    CONST_CP18 = GetProfileString_S("ENGLISH", "CONST_CP18", sPath, "*")
    CONST_CP19 = GetProfileString_S("ENGLISH", "CONST_CP19", sPath, "*")
    CONST_CP30 = GetProfileString_S("ENGLISH", "CONST_CP30", sPath, "*")
    CONST_CP31 = GetProfileString_S("ENGLISH", "CONST_CP31", sPath, "*")
    CONST_CP20 = GetProfileString_S("ENGLISH", "CONST_CP20", sPath, "*")
    CONST_CP21 = GetProfileString_S("ENGLISH", "CONST_CP21", sPath, "*")
    CONST_CP22 = GetProfileString_S("ENGLISH", "CONST_CP22", sPath, "*")
    CONST_CP23 = GetProfileString_S("ENGLISH", "CONST_CP23", sPath, "*")
    CONST_CP24 = GetProfileString_S("ENGLISH", "CONST_CP24", sPath, "*")
    CONST_CPR1 = GetProfileString_S("ENGLISH", "CONST_CPR1", sPath, "*")
    CONST_CPR2 = GetProfileString_S("ENGLISH", "CONST_CPR2", sPath, "*")
    
    STS_COM01 = GetProfileString_S("ENGLISH", "STS_COM01", sPath, "*")
    STS_0 = GetProfileString_S("ENGLISH", "STS_0", sPath, "*")
    STS_1 = GetProfileString_S("ENGLISH", "STS_1", sPath, "*")
    STS_2 = GetProfileString_S("ENGLISH", "STS_2", sPath, "*")
    STS_3 = GetProfileString_S("ENGLISH", "STS_3", sPath, "*")
    STS_4 = GetProfileString_S("ENGLISH", "STS_4", sPath, "*")
    STS_5 = GetProfileString_S("ENGLISH", "STS_5", sPath, "*")
    STS_6 = GetProfileString_S("ENGLISH", "STS_6", sPath, "*")
    STS_7 = GetProfileString_S("ENGLISH", "STS_7", sPath, "*")
    STS_8 = GetProfileString_S("ENGLISH", "STS_8", sPath, "*")
    STS_9 = GetProfileString_S("ENGLISH", "STS_9", sPath, "*")
    STS_10 = GetProfileString_S("ENGLISH", "STS_10", sPath, "*")
    STS_11 = GetProfileString_S("ENGLISH", "STS_11", sPath, "*")
    STS_12 = GetProfileString_S("ENGLISH", "STS_12", sPath, "*")
    STS_13 = GetProfileString_S("ENGLISH", "STS_13", sPath, "*")
    STS_14 = GetProfileString_S("ENGLISH", "STS_14", sPath, "*")
    STS_15 = GetProfileString_S("ENGLISH", "STS_15", sPath, "*")
    STS_16 = GetProfileString_S("ENGLISH", "STS_16", sPath, "*")
    STS_17 = GetProfileString_S("ENGLISH", "STS_17", sPath, "*")
    STS_19 = GetProfileString_S("ENGLISH", "STS_19", sPath, "*")
    STS_20 = GetProfileString_S("ENGLISH", "STS_20", sPath, "*")
    STS_21 = GetProfileString_S("ENGLISH", "STS_21", sPath, "*")
    STS_22 = GetProfileString_S("ENGLISH", "STS_22", sPath, "*")
    STS_23 = GetProfileString_S("ENGLISH", "STS_23", sPath, "*")
    STS_24 = GetProfileString_S("ENGLISH", "STS_24", sPath, "*")
    STS_25 = GetProfileString_S("ENGLISH", "STS_25", sPath, "*")
    STS_26 = GetProfileString_S("ENGLISH", "STS_26", sPath, "*")
    STS_41 = GetProfileString_S("ENGLISH", "STS_41", sPath, "*")
    STS_42 = GetProfileString_S("ENGLISH", "STS_42", sPath, "*")

    ' データ読み込みおよび編集終了時のエラー
    CONST_LOAD01 = GetProfileString_S("ENGLISH", "CONST_LOAD01", sPath, "*")
    CONST_LOAD06 = GetProfileString_S("ENGLISH", "CONST_LOAD06", sPath, "*")
    CONST_LOAD08 = GetProfileString_S("ENGLISH", "CONST_LOAD08", sPath, "*")
    CONST_LOAD99 = GetProfileString_S("ENGLISH", "CONST_LOAD99", sPath, "*")
    
    ' 項目
    CONST_PR4G_M = GetProfileString_S("ENGLISH", "CONST_PR4G_M", sPath, "*")
    CONST_PR4H_M = GetProfileString_S("ENGLISH", "CONST_PR4H_M", sPath, "*")
    CONST_PR4L_M = GetProfileString_S("ENGLISH", "CONST_PR4L_M", sPath, "*")
    
    ' ボタン押下時メッセージ
    MSG_BTN_CLEAR = GetProfileString_S("ENGLISH", "MSG_BTN_CLEAR", sPath, "*")
    MSG_BTN_CANCEL = GetProfileString_S("ENGLISH", "MSG_BTN_CANCEL", sPath, "*")
    
    ' プローブ位置合わせメッセージ
    MSG_PRB_XYMODE = GetProfileString_S("ENGLISH", "MSG_PRB_XYMODE", sPath, "*")
    MSG_PRB_BPMODE = GetProfileString_S("ENGLISH", "MSG_PRB_BPMODE", sPath, "*")
    MSG_PRB_ZTMODE = GetProfileString_S("ENGLISH", "MSG_PRB_ZTMODE", sPath, "*")
    MSG_PRB_Z2TMODE = GetProfileString_S("ENGLISH", "MSG_PRB_Z2TMODE", sPath, "*")
    MSG_PRB_THETA = GetProfileString_S("ENGLISH", "MSG_PRB_THETA", sPath, "*")
    MSG_PRB_Z_MSG = GetProfileString_S("ENGLISH", "MSG_PRB_Z_MSG", sPath, "*")
    
    ' システムエラーメッセージ
    MSG_ERR_ZNOTORG = GetProfileString_S("ENGLISH", "MSG_ERR_ZNOTORG", sPath, "*")
    
    ' レーザー調整画面説明文
    MSG_LASER_LASERON = GetProfileString_S("ENGLISH", "MSG_LASER_LASERON", sPath, "*")
    MSG_LASER_LASEROFF = GetProfileString_S("ENGLISH", "MSG_LASER_LASEROFF", sPath, "*")
    MSG_LASEROFF = GetProfileString_S("ENGLISH", "MSG_LASEROFF", sPath, "*")
    MSG_LASERON = GetProfileString_S("ENGLISH", "MSG_LASERON", sPath, "*")
    MSG_ERRQRATE = GetProfileString_S("ENGLISH", "MSG_ERRQRATE", sPath, "*")
    MSG_LOGERROR = GetProfileString_S("ENGLISH", "MSG_LOGERROR", sPath, "*")
    MSG_SPECPOWER = GetProfileString_S("ENGLISH", "MSG_SPECPOWER", sPath, "*")
    MSG_MEASPOWER = GetProfileString_S("ENGLISH", "MSG_MEASPOWER", sPath, "*")
    MSG_FULLPOWER = GetProfileString_S("ENGLISH", "MSG_FULLPOWER", sPath, "*")
    MSG_POWERPROCESS = GetProfileString_S("ENGLISH", "MSG_POWERPROCESS", sPath, "*")
    MSG_ATTRATE = GetProfileString_S("ENGLISH", "MSG_ATTRATE", sPath, "*")
    '----- ###806①↓ -----
    MSG_AP_POWER = GetProfileString_S("ENGLISH", "MSG_AP_POWER", sPath, "*")
    MSG_AP_TARGET = GetProfileString_S("ENGLISH", "MSG_AP_TARGET", sPath, "*")
    MSG_AP_HILO = GetProfileString_S("ENGLISH", "MSG_AP_HILO", sPath, "*")
    MSG_AP_HILOW = GetProfileString_S("ENGLISH", "MSG_AP_HILOW", sPath, "*")
    '----- ###806①↑ -----

    ' 操作ログ　メッセージ
    MSG_OPLOG_WAKEUP = GetProfileString_S("ENGLISH", "MSG_OPLOG_WAKEUP", sPath, "*")
    MSG_OPLOG_FUNC01 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC01", sPath, "*")
    MSG_OPLOG_FUNC02 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC02", sPath, "*")
    MSG_OPLOG_FUNC03 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC03", sPath, "*")
    MSG_OPLOG_FUNC04 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC04", sPath, "*")
    MSG_OPLOG_FUNC05 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC05", sPath, "*")
    MSG_OPLOG_FUNC06 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC06", sPath, "*")
    MSG_OPLOG_FUNC07 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC07", sPath, "*")
    MSG_OPLOG_FUNC08 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC08", sPath, "*")
    MSG_OPLOG_FUNC09 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC09", sPath, "*")
    MSG_OPLOG_FUNC10 = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC10", sPath, "*")
    MSG_OPLOG_CLRTOTAL = GetProfileString_S("ENGLISH", "MSG_OPLOG_CLRTOTAL", sPath, "*")
    MSG_OPLOG_TRIMST = GetProfileString_S("ENGLISH", "MSG_OPLOG_TRIMST", sPath, "*")
    MSG_OPLOG_TRIMRES = GetProfileString_S("ENGLISH", "MSG_OPLOG_TRIMRES", sPath, "*")
    MSG_OPLOG_HCMD_ERRRST = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_ERRRST", sPath, "*")
    MSG_OPLOG_HCMD_PATCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_PATCMD", sPath, "*")
    MSG_OPLOG_HCMD_LASCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_LASCMD", sPath, "*")
    MSG_OPLOG_HCMD_MARKCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_MARKCMD", sPath, "*")
    MSG_OPLOG_HCMD_LODCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_LODCMD", sPath, "*")
    MSG_OPLOG_HCMD_TECCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_TECCMD", sPath, "*")
    MSG_OPLOG_HCMD_TRMCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_TRMCMD", sPath, "*")
    MSG_OPLOG_HCMD_LSTCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_LSTCMD", sPath, "*")
    MSG_OPLOG_HCMD_LENCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_LENCMD", sPath, "*")
    MSG_OPLOG_HCMD_MDAUTO = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_MDAUTO", sPath, "*")
    MSG_OPLOG_HCMD_MDMANU = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_MDMANU", sPath, "*")
    MSG_OPLOG_HCMD_CPCMD = GetProfileString_S("ENGLISH", "MSG_OPLOG_HCMD_CPCMD", sPath, "*")
    MSG_POPUP_MESSAGE = GetProfileString_S("ENGLISH", "MSG_POPUP_MESSAGE", sPath, "*")
    MSG_OPLOG_FUNC08S = GetProfileString_S("ENGLISH", "MSG_OPLOG_FUNC08S", sPath, "*")
    
    ' メイン画面ラベル
    MSG_MAIN_LABEL01 = GetProfileString_S("ENGLISH", "MSG_MAIN_LABEL01", sPath, "*")
    MSG_MAIN_LABEL02 = GetProfileString_S("ENGLISH", "MSG_MAIN_LABEL02", sPath, "*")
    
    ' [CIRCUIT]
    MSG_CIRCUIT_LABEL01 = GetProfileString_S("ENGLISH", "MSG_CIRCUIT_LABEL01", sPath, "*")
    MSG_CIRCUIT_LABEL02 = GetProfileString_S("ENGLISH", "MSG_CIRCUIT_LABEL02", sPath, "*")
    
    ' [registor]
    MSG_REGISTER_LABEL01 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL01", sPath, "*")
    MSG_REGISTER_LABEL02 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL02", sPath, "*")
    MSG_REGISTER_LABEL03 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL03", sPath, "*")
    MSG_REGISTER_LABEL04 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL04", sPath, "*")
    MSG_REGISTER_LABEL05 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL05", sPath, "*")
    MSG_REGISTER_LABEL06 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL06", sPath, "*")
    MSG_REGISTER_LABEL07 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL07", sPath, "*")
    MSG_REGISTER_LABEL08 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL08", sPath, "*")
    MSG_REGISTER_LABEL09 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL09", sPath, "*")
    MSG_REGISTER_LABEL10 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL10", sPath, "*")
    MSG_REGISTER_LABEL11 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL11", sPath, "*")
    MSG_REGISTER_LABEL12 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL12", sPath, "*")
    MSG_REGISTER_LABEL13 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL13", sPath, "*")
    MSG_REGISTER_LABEL14 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL14", sPath, "*")
    MSG_REGISTER_LABEL15 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL15", sPath, "*")
    MSG_REGISTER_LABEL16 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL16", sPath, "*")
    MSG_REGISTER_LABEL17 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL17", sPath, "*")
    MSG_REGISTER_LABEL18 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL18", sPath, "*")
    MSG_REGISTER_LABEL19 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL19", sPath, "*")
    MSG_REGISTER_LABEL20 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL20", sPath, "*")
    MSG_REGISTER_LABEL21 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL21", sPath, "*")
    MSG_REGISTER_LABEL22 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL22", sPath, "*")
    MSG_REGISTER_LABEL23 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL23", sPath, "*")
    MSG_REGISTER_LABEL24 = GetProfileString_S("ENGLISH", "MSG_REGISTER_LABEL24", sPath, "*")
    
    ' 編集　[cut]
    MSG_CUT_LABEL01 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL01", sPath, "*")
    MSG_CUT_LABEL02 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL02", sPath, "*")
    MSG_CUT_LABEL03 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL03", sPath, "*")
    MSG_CUT_LABEL04 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL04", sPath, "*")
    MSG_CUT_LABEL05 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL05", sPath, "*")
    MSG_CUT_LABEL06 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL06", sPath, "*")
    MSG_CUT_LABEL07 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL07", sPath, "*")
    MSG_CUT_LABEL08 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL08", sPath, "*")
    MSG_CUT_LABEL09 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL09", sPath, "*")
    MSG_CUT_LABEL10 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL10", sPath, "*")
    MSG_CUT_LABEL11 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL11", sPath, "*")
    MSG_CUT_LABEL12 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL12", sPath, "*")
    MSG_CUT_LABEL13 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL13", sPath, "*")
    MSG_CUT_LABEL14 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL14", sPath, "*")
    MSG_CUT_LABEL15 = GetProfileString_S("ENGLISH", "MSG_CUT_LABEL15", sPath, "*")
    
    ' ■ティーチング
    MSG_TEACH_LABEL01 = GetProfileString_S("ENGLISH", "MSG_TEACH_LABEL01", sPath, "*")
    MSG_TEACH_LABEL02 = GetProfileString_S("ENGLISH", "MSG_TEACH_LABEL02", sPath, "*")
    MSG_TEACH_LABEL03 = GetProfileString_S("ENGLISH", "MSG_TEACH_LABEL03", sPath, "*")
    MSG_TEACH_LABEL04 = GetProfileString_S("ENGLISH", "MSG_TEACH_LABEL04", sPath, "*")
    MSG_TEACH_LABEL05 = GetProfileString_S("ENGLISH", "MSG_TEACH_LABEL05", sPath, "*")
    
    ' ■プローブ位置合わせ
    MSG_PROBE_LABEL01 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL01", sPath, "*")
    MSG_PROBE_LABEL02 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL02", sPath, "*")
    MSG_PROBE_LABEL03 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL03", sPath, "*")
    MSG_PROBE_LABEL04 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL04", sPath, "*")
    MSG_PROBE_LABEL05 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL05", sPath, "*")
    MSG_PROBE_LABEL06 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL06", sPath, "*")
    MSG_PROBE_LABEL07 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL07", sPath, "*")
    MSG_PROBE_LABEL08 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL08", sPath, "*")
    MSG_PROBE_LABEL09 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL09", sPath, "*")
    MSG_PROBE_LABEL10 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL10", sPath, "*")
    MSG_PROBE_LABEL11 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL11", sPath, "*")
    MSG_PROBE_LABEL12 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL12", sPath, "*")
    MSG_PROBE_LABEL13 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL13", sPath, "*")
    MSG_PROBE_LABEL14 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL14", sPath, "*")
    MSG_PROBE_LABEL15 = GetProfileString_S("ENGLISH", "MSG_PROBE_LABEL15", sPath, "*")
    
    MSG_PROBE_MSG01 = GetProfileString_S("ENGLISH", "MSG_PROBE_MSG01", sPath, "*")
    MSG_PROBE_MSG02 = GetProfileString_S("ENGLISH", "MSG_PROBE_MSG02", sPath, "*")
    MSG_PROBE_MSG03 = GetProfileString_S("ENGLISH", "MSG_PROBE_MSG03", sPath, "*")
    MSG_PROBE_MSG04 = GetProfileString_S("ENGLISH", "MSG_PROBE_MSG04", sPath, "*")
    
    MSG_PROBE_ERR01 = GetProfileString_S("ENGLISH", "MSG_PROBE_ERR01", sPath, "*")
    
    ' ■frmMsgBox 画面終了確認
    MSG_CLOSE_LABEL01 = GetProfileString_S("ENGLISH", "MSG_CLOSE_LABEL01", sPath, "*")
    MSG_CLOSE_LABEL02 = GetProfileString_S("ENGLISH", "MSG_CLOSE_LABEL02", sPath, "*")
    MSG_CLOSE_LABEL03 = GetProfileString_S("ENGLISH", "MSG_CLOSE_LABEL03", sPath, "*")
    
    ' ■frmReset 原点復帰画面など
    MSG_FRMRESET_LABEL01 = GetProfileString_S("ENGLISH", "MSG_FRMRESET_LABEL01", sPath, "*")
    
    ' ■LASER_teaching
    MSG_LASER_LABEL01 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL01", sPath, "*")
    MSG_LASER_LABEL02 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL02", sPath, "*")
    MSG_LASER_LABEL03 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL03", sPath, "*")
    MSG_LASER_LABEL04 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL04", sPath, "*")
    MSG_LASER_LABEL05 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL05", sPath, "*")
    MSG_LASER_LABEL06 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL06", sPath, "*")
    MSG_LASER_LABEL07 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL07", sPath, "*")
    MSG_LASER_LABEL08 = GetProfileString_S("ENGLISH", "MSG_LASER_LABEL08", sPath, "*")
 
    ' 編集画面 カット
    LBL_CUT_PLATE_J1 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J1", sPath, "*")
    LBL_CUT_PLATE_J2 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J2", sPath, "*")
    LBL_CUT_PLATE_J3 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J3", sPath, "*")
    LBL_CUT_PLATE_J4 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J4", sPath, "*")
    LBL_CUT_PLATE_J5 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J5", sPath, "*")
    LBL_CUT_PLATE_J6 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J6", sPath, "*")
    LBL_CUT_PLATE_J7 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J7", sPath, "*")
    LBL_CUT_PLATE_J8 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J8", sPath, "*")
    LBL_CUT_PLATE_J9 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J9", sPath, "*")
    LBL_CUT_PLATE_J10 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J10", sPath, "*")
    LBL_CUT_PLATE_J11 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J11", sPath, "*")
    LBL_CUT_PLATE_J12 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J12", sPath, "*")
    LBL_CUT_PLATE_J13 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J13", sPath, "*")
    LBL_CUT_PLATE_J14 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J14", sPath, "*")
    LBL_CUT_PLATE_J15 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J15", sPath, "*")
    LBL_CUT_PLATE_J16 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J16", sPath, "*")
    LBL_CUT_PLATE_J17 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J17", sPath, "*")
    LBL_CUT_PLATE_J18 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J18", sPath, "*")
    LBL_CUT_PLATE_J19 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J19", sPath, "*")
    LBL_CUT_PLATE_J21 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J21", sPath, "*")
    LBL_CUT_PLATE_J22 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J22", sPath, "*")
    LBL_CUT_PLATE_J23 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J23", sPath, "*")
    LBL_CUT_PLATE_J24 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J24", sPath, "*")
    LBL_CUT_PLATE_J25 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J25", sPath, "*")
    LBL_CUT_PLATE_J26 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J26", sPath, "*")
    LBL_CUT_PLATE_J27 = GetProfileString_S("ENGLISH", "LBL_CUT_PLATE_J27", sPath, "*")
    MSG_CUT_1 = GetProfileString_S("ENGLISH", "MSG_CUT_1", sPath, "*")
    LBL_CUT_COPYLINE = GetProfileString_S("ENGLISH", "LBL_CUT_COPYLINE", sPath, "*")
    LBL_CUT_COPYCOLUMN = GetProfileString_S("ENGLISH", "LBL_CUT_COPYCOLUMN", sPath, "*")

    ' ■CorrectPos
    MSG_CORPOS_LABEL01 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL01", sPath, "*")
    MSG_CORPOS_LABEL02 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL02", sPath, "*")
    MSG_CORPOS_LABEL03 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL03", sPath, "*")
    MSG_CORPOS_LABEL04 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL04", sPath, "*")
    MSG_CORPOS_LABEL05 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL05", sPath, "*")
    MSG_CORPOS_LABEL06 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL06", sPath, "*")
    MSG_CORPOS_LABEL07 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL07", sPath, "*")
    MSG_CORPOS_LABEL08 = GetProfileString_S("ENGLISH", "MSG_CORPOS_LABEL08", sPath, "*")
    MSG_CORPOS_MSG01 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG01", sPath, "*")
    MSG_CORPOS_MSG02 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG02", sPath, "*")
    MSG_CORPOS_MSG03 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG03", sPath, "*")
    MSG_CORPOS_MSG04 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG04", sPath, "*")
    MSG_CORPOS_MSG05 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG05", sPath, "*")
    MSG_CORPOS_MSG06 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG06", sPath, "*")
    MSG_CORPOS_MSG07 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG07", sPath, "*")
    MSG_CORPOS_MSG08 = GetProfileString_S("ENGLISH", "MSG_CORPOS_MSG08", sPath, "*")

End Sub

'===============================================================================
'【機　能】 INIファイル形式のファイルからデータを取得する
'【引　数】 p_Sect     (INP) : セクション名
'           p_Key      (INP) : キー名
'           p_Filename (INP) : INIファイル名（フルパス：Private、ファイル名のみ：Windwosディレクトリ）
'           pSstr      (INP) : INIファイルに存在しないときの初期値
'【戻り値】 取得データ（String)
'===============================================================================
Private Function GetProfileString_S(p_Sect As String, p_Key As String, p_Filename As String, pSstr As String) As String
    Dim m_Str As String
    m_Str = Space(256)

    GetPrivateProfileString p_Sect, p_Key, pSstr, m_Str, Len(m_Str), p_Filename
    If Trim(m_Str) = "" Then
        m_Str = ""
    Else
        m_Str = Mid(m_Str, 1, InStr(m_Str, Chr(0)) - 1)
    End If

    GetProfileString_S = m_Str
    
End Function

'###812①
Public Function GetLaserButton() As Long

    GetLaserButton = GetProfileString_S("OPT_TEACH", "BTN_LASER", "C:\TRIM\TKY.INI", "0")
    
End Function
    
    
    
