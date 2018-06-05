''===============================================================================
''   Description  : メッセージ定義(日本語/英語)
''
''   Copyright(C) : OMRON LASERFRONT INC. 2010
''
''===============================================================================
'Option Strict Off
'Option Explicit On
'Module ConstMessage
'#Region "メッセージ定義"
'    '===========================================================================
'    '   メッセージ定義
'    '===========================================================================
'    ' エラーメッセージ
'    Public MSG_0 As String
'    Public MSG_7 As String
'    Public MSG_COM As String
'    Public MSG_11 As String
'    Public MSG_14 As String
'    Public MSG_15 As String
'    Public MSG_16 As String
'    Public MSG_19 As String
'    Public MSG_PP19_1 As String
'    Public MSG_PP19_2 As String
'    Public MSG_PP0 As String
'    Public MSG_CP0 As String
'    Public MSG_CP_STEP As String
'    Public MSG_20 As String
'    Public MSG_21 As String
'    Public MSG_22 As String
'    Public MSG_30 As String
'    Public MSG_35 As String
'    Public MSG_36 As String
'    Public MSG_37 As String
'    Public MSG_38 As String
'    Public MSG_39 As String
'    Public MSG_40 As String
'    Public MSG_41 As String
'    Public MSG_42 As String
'    Public MSG_43 As String
'    Public MSG_44 As String
'    Public MSG_45 As String
'    Public MSG_46 As String
'    Public MSG_PP11_1 As String
'    Public MSG_PP11_2 As String
'    Public MSG_71 As String
'    Public MSG_72 As String
'    Public MSG_73 As String
'    Public MSG_74 As String
'    Public MSG_75 As String

'    Public MSG_PP23 As String
'    Public MSG_PP24_1 As String
'    Public MSG_PP24_2 As String
'    Public MSG_25 As String
'    Public MSG_TOTAL_CIRCUIT As String
'    Public MSG_TOTAL_REGISTOR As String
'    Public MSG_PR_COM1 As String
'    Public MSG_PR01_1 As String
'    Public MSG_PR01_2 As String
'    Public MSG_BP As String
'    Public MSG_PR04_1 As String
'    Public MSG_PR04_2 As String
'    Public MSG_PR04_3 As String
'    Public MSG_PR04_4 As String
'    Public MSG_PR05 As String
'    Public MSG_PR07_1 As String
'    Public MSG_PR07_2 As String
'    Public MSG_PR08_1 As String
'    Public MSG_PR08_2 As String
'    Public MSG_PR08_3 As String
'    Public MSG_PR08_4 As String
'    Public MSG_PR08_4a As String
'    Public MSG_PR08_5 As String
'    Public MSG_PR08_5a As String
'    Public MSG_PR08_COM1 As String
'    Public MSG_101 As String
'    Public MSG_102 As String
'    Public MSG_103 As String
'    Public MSG_104 As String
'    Public MSG_105 As String
'    Public MSG_106 As String
'    Public MSG_107 As String
'    Public MSG_108 As String
'    Public MSG_109 As String
'    Public MSG_110 As String
'    Public MSG_112 As String
'    Public MSG_114 As String
'    Public MSG_115 As String
'    Public MSG_116 As String
'    Public MSG_117 As String
'    Public MSG_118 As String
'    Public MSG_119 As String
'    Public MSG_120 As String
'    Public MSG_121 As String
'    Public MSG_122 As String
'    Public MSG_123 As String
'    Public MSG_124 As String
'    Public MSG_125 As String
'    Public MSG_126 As String
'    Public MSG_127 As String    'V1.13.0.0③

'    Public MSG_130 As String
'    Public MSG_131 As String
'    Public MSG_132 As String
'    Public MSG_133 As String
'    Public MSG_134 As String
'    Public MSG_135 As String
'    Public MSG_136 As String
'    Public MSG_137 As String
'    Public MSG_138 As String
'    Public MSG_139 As String
'    Public MSG_140 As String
'    Public MSG_141 As String
'    Public MSG_142 As String
'    Public MSG_143 As String
'    Public MSG_144 As String
'    Public MSG_145 As String
'    Public MSG_146 As String
'    Public MSG_147 As String
'    Public MSG_148 As String
'    Public MSG_149 As String
'    Public MSG_150 As String
'    Public MSG_151 As String
'    '----- V1.18.0.0③↓ -----
'    Public MSG_152 As String
'    Public MSG_153 As String
'    Public MSG_154 As String
'    '----- V1.18.0.0③↑ -----
'    Public MSG_155 As String 'V1.18.0.0⑫
'    Public MSG_156 As String 'V1.23.0.0⑦
'    Public MSG_157 As String 'V1.23.0.0①
'    Public MSG_158 As String 'V2.0.0.0⑤
'    Public MSG_159 As String 'V4.0.0.0⑨
'    '----- V4.0.0.0-30↓ -----
'    Public MSG_160 As String
'    Public MSG_161 As String
'    Public MSG_162 As String
'    '----- V4.0.0.0-30↑ -----

'    Public LBL_cmdresi As String
'    Public LBL_cmdvol As String

'    Public MSGERR_COVER_CLOSE As String
'    Public MSGERR_SEND_TRIMDATA As String

'    ' TITLE
'    Public TITLE_1 As String
'    Public TITLE_2 As String
'    Public TITLE_3 As String
'    Public TITLE_4 As String
'    Public TITLE_5 As String
'    Public TITLE_6 As String
'    Public TITLE_7 As String
'    Public TITLE_8 As String

'    Public TITLE_LASER As String
'    Public TITLE_LOGGING As String
'    Public TITLE_TRIMMODE As String
'    Public TITLE_RATIOINPUT As String

'    ' ラベル
'    Public LBL_PP_1 As String
'    Public LBL_PP_2 As String
'    Public LBL_PP_3 As String
'    Public LBL_PP_4 As String
'    Public LBL_PP_5 As String
'    Public LBL_PP_6 As String
'    Public LBL_PP_8 As String
'    Public LBL_PP_9 As String
'    Public LBL_PP_10 As String
'    Public LBL_PP_12 As String
'    Public LBL_PP_13 As String
'    Public LBL_PP_14 As String
'    Public LBL_PP_15 As String
'    Public LBL_PP_16 As String
'    Public LBL_PP_17 As String
'    Public LBL_PP_18 As String
'    Public LBL_PP_19 As String
'    Public LBL_PP_20 As String
'    Public LBL_PP_21 As String
'    Public LBL_PP_22 As String
'    Public LBL_PP_23 As String
'    Public LBL_PP_24 As String
'    Public LBL_PP_25 As String
'    Public LBL_PP_26 As String
'    Public LBL_PP_REVISE As String
'    Public LBL_PP_30 As String
'    Public LBL_PP_31 As String
'    Public LBL_PP_32 As String
'    Public LBL_PP_33 As String
'    Public LBL_PP_34 As String
'    Public LBL_PP_35 As String
'    Public LBL_PP_36 As String
'    Public LBL_PP_37 As String
'    Public LBL_PP_38 As String
'    Public LBL_PP_39 As String
'    Public LBL_PP_CARIB As String
'    Public LBL_PP_40 As String
'    Public LBL_PP_41 As String
'    Public LBL_PP_42 As String
'    Public LBL_PP_43 As String
'    Public LBL_PP_CARIBGRPNO As String
'    Public LBL_PP_44 As String
'    Public LBL_PP_45 As String
'    Public LBL_PP_46 As String
'    Public LBL_PP_47 As String
'    Public LBL_PP_48 As String
'    Public LBL_PP_49 As String
'    Public LBL_PP_50 As String
'    Public LBL_PP_51 As String
'    Public LBL_PP_52 As String
'    Public LBL_PP_53 As String
'    Public LBL_PP_54 As String
'    Public LBL_PP_55 As String
'    Public LBL_PP_115 As String
'    Public LBL_PP_116 As String
'    Public LBL_PP_117 As String
'    Public LBL_PP_118 As String
'    Public LBL_PP_119 As String
'    Public LBL_PP_120 As String
'    Public LBL_PP_121 As String
'    Public LBL_PP_122 As String
'    Public LBL_PP_123 As String
'    Public LBL_PP_124 As String
'    Public LBL_PP_125 As String
'    Public LBL_PP_126 As String
'    Public LBL_PP_127 As String
'    Public LBL_PP_128 As String

'    ' 一時停止画面用 ###014
'    Public LBL_FINEADJ_001 As String
'    '----- ###204↓ ----- 
'    Public LBL_FINEADJ_002 As String

'    Public LBL_FINEADJ_003 As String
'    Public LBL_FINEADJ_004 As String
'    Public LBL_FINEADJ_005 As String
'    Public LBL_FINEADJ_006 As String ' V4.11.0.0⑥
'    '----- ###204↑ ----- 

'    'GP-IB制御
'    Public LBL_PP_130 As String
'    Public LBL_PP_131 As String
'    Public LBL_PP_132 As String
'    Public LBL_PP_133 As String
'    Public LBL_PP_134 As String
'    Public LBL_PP_135 As String

'    'GP -IB制御(パターン2)
'    Public LBL_PP_136 As String
'    Public LBL_PP_137 As String
'    Public LBL_PP_138 As String
'    Public LBL_PP_139 As String
'    Public LBL_PP_140 As String
'    Public LBL_PP_141 As String
'    Public LBL_PP_142 As String
'    Public LBL_S_1 As String
'    Public LBL_S_2 As String
'    Public LBL_S_3 As String
'    Public LBL_S_4 As String
'    Public LBL_S_5 As String
'    Public LBL_S_6 As String
'    Public LBL_S_7 As String
'    Public LBL_S_8 As String
'    Public LBL_S_9 As String

'    '(TXTY仕様変更)
'    Public LBL_G_1 As String
'    Public LBL_G_2 As String
'    Public LBL_G_3 As String
'    Public LBL_TY2_1 As String
'    Public LBL_TY2_2 As String
'    Public LBL_TEACHING_001 As String
'    Public LBL_TEACHING_002 As String
'    Public LBL_TEACHING_003 As String
'    Public LBL_TEACHING_004 As String

'    'cut pos teaching
'    Public LBL_CUTPOSTEACH_001 As String
'    Public LBL_CUTPOSTEACH_002 As String
'    Public LBL_CUTPOSTEACH_003 As String
'    Public LBL_CUTPOSTEACH_004 As String
'    Public LBL_RECOG_001 As String
'    Public LBL_RECOG_002 As String
'    Public LBL_RECOG_003 As String
'    Public LBL_RECOG_004 As String
'    Public LBL_RECOG_005 As String
'    Public LBL_RECOG_006 As String
'    Public LBL_RECOG_007 As String
'    Public LBL_RECOG_008 As String
'    Public LBL_RECOG_009 As String
'    Public LBL_RECOG_010 As String
'    Public LBL_RECOG_011 As String
'    Public LBL_RECOG_012 As String
'    Public LBL_RECOG_013 As String
'    Public LBL_RECOG_014 As String
'    Public LBL_RECOG_015 As String
'    Public LBL_RECOG_016 As String
'    Public LBL_RECOG_017 As String
'    Public LBL_RECOG_018 As String
'    Public LBL_RECOG_019 As String
'    Public LBL_RECOG_020 As String

'    Public MSG_RECOG_001 As String
'    Public MSG_RECOG_002 As String

'    ' スプラッシュメッセージ
'    Public MSG_SPRASH0 As String
'    Public MSG_SPRASH1 As String
'    Public MSG_SPRASH2 As String
'    Public MSG_SPRASH3 As String
'    Public MSG_SPRASH4 As String
'    Public MSG_SPRASH5 As String
'    Public MSG_SPRASH6 As String
'    Public MSG_SPRASH7 As String
'    Public MSG_SPRASH8 As String
'    Public MSG_SPRASH9 As String
'    Public MSG_SPRASH10 As String
'    Public MSG_SPRASH11 As String
'    Public MSG_SPRASH12 As String
'    Public MSG_SPRASH13 As String
'    Public MSG_SPRASH14 As String
'    Public MSG_SPRASH15 As String
'    Public MSG_SPRASH16 As String
'    Public MSG_SPRASH17 As String
'    Public MSG_SPRASH18 As String
'    Public MSG_SPRASH19 As String
'    Public MSG_SPRASH20 As String
'    Public MSG_SPRASH21 As String
'    Public MSG_SPRASH22 As String
'    Public MSG_SPRASH23 As String
'    Public MSG_SPRASH24 As String
'    Public MSG_SPRASH25 As String
'    Public MSG_SPRASH26 As String
'    Public MSG_SPRASH27 As String
'    Public MSG_SPRASH28 As String
'    Public MSG_SPRASH29 As String
'    Public MSG_SPRASH30 As String
'    Public MSG_SPRASH31 As String
'    Public MSG_SPRASH32 As String
'    Public MSG_SPRASH33 As String
'    Public MSG_SPRASH34 As String
'    Public MSG_SPRASH35 As String                                   ' ###073
'    Public MSG_SPRASH36 As String                                   ' ###088
'    Public MSG_SPRASH37 As String                                   ' ###137 
'    Public MSG_SPRASH38 As String                                   ' ###188 
'    Public MSG_SPRASH39 As String                                   ' V1.13.0.0③
'    Public MSG_SPRASH40 As String                                   ' V1.13.0.0⑩
'    Public MSG_SPRASH41 As String                                   ' V1.13.0.0⑩
'    Public MSG_SPRASH42 As String                                   ' V1.13.0.0⑩
'    Public MSG_SPRASH43 As String                                   ' V1.18.0.1⑧
'    Public MSG_SPRASH44 As String                                   ' V1.18.0.1⑧
'    Public MSG_SPRASH45 As String                                   ' V4.0.0.0-71
'    Public MSG_SPRASH46 As String                                   ' V4.0.0.0-83
'    Public MSG_SPRASH47 As String                                   ' V4.0.0.0-83
'    Public MSG_SPRASH48 As String                                   ' V4.1.0.0①
'    Public MSG_SPRASH49 As String                                   ' V4.1.0.0①
'    Public MSG_SPRASH50 As String                                   ' V4.1.0.0⑦
'    Public MSG_SPRASH51 As String                                   ' V4.1.0.0⑦

'    '----- limit.frm用 -----
'    Public MSG_frmLimit_01 As String
'    Public MSG_frmLimit_02 As String
'    Public MSG_frmLimit_03 As String
'    Public MSG_frmLimit_04 As String
'    Public MSG_frmLimit_05 As String
'    Public MSG_frmLimit_06 As String
'    Public MSG_frmLimit_07 As String

'    '----- INtime側エラーメッセージ -----
'    Public MSG_SRV_ALM As String                                    ' サーボアラーム
'    Public MSG_AXIS_X_SERVO_ALM As String                           ' X軸サーボアラーム
'    Public MSG_AXIS_Y_SERVO_ALM As String                           ' Y軸サーボアラーム
'    Public MSG_AXIS_Z_SERVO_ALM As String                           ' Z軸サーボアラーム
'    Public MSG_AXIS_T_SERVO_ALM As String                           ' θ軸サーボアラーム

'    ' 軸系エラー(タイムアウト)
'    Public MSG_TIMEOUT_AXIS_X As String                             ' X軸タイムアウトエラー
'    Public MSG_TIMEOUT_AXIS_Y As String                             ' Y軸タイムアウトエラー
'    Public MSG_TIMEOUT_AXIS_Z As String                             ' Z軸タイムアウトエラー
'    Public MSG_TIMEOUT_AXIS_T As String                             ' θ軸タイムアウトエラー
'    Public MSG_TIMEOUT_AXIS_Z2 As String                            ' Z2軸タイムアウトエラー
'    Public MSG_TIMEOUT_ATT As String                                ' ロータリアッテネータタイムアウトエラー
'    Public MSG_TIMEOUT_AXIS_XY As String                            ' XY軸タイムアウトエラー

'    ' 軸系エラー(プラスリミットオーバー)
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_X As String                    ' X軸プラスリミットオーバー
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_Y As String                    ' Y軸プラスリミットオーバー
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_Z As String                    ' Z軸プラスリミットオーバー
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_T As String                    ' θ軸プラスリミットオーバー
'    Public MSG_STG_SOFTLMT_PLUS_AXIS_Z2 As String                   ' Z2軸プラスリミットオーバー
'    Public MSG_STG_SOFTLMT_PLUS As String                           ' 軸プラスリミットオーバー

'    ' 軸系エラー(マイナスリミットオーバー)
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_X As String                   ' X軸マイナスリミットオーバー
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_Y As String                   ' Y軸マイナスリミットオーバー
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_Z As String                   ' Z軸マイナスリミットオーバー
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_T As String                   ' θ軸マイナスリミットオーバー
'    Public MSG_STG_SOFTLMT_MINUS_AXIS_Z2 As String                  ' Z2軸マイナスリミットオーバー
'    Public MSG_STG_SOFTLMT_MINUS As String                          ' 軸マイナスリミットオーバー

'    ' 軸系エラー(リミット検出)
'    Public MSG_AXS_LIM_X As String                                  ' X軸リミット検出
'    Public MSG_AXS_LIM_Y As String                                  ' Y軸リミット検出
'    Public MSG_AXS_LIM_Z As String                                  ' Z軸リミット検出
'    Public MSG_AXS_LIM_T As String                                  ' θ軸リミット検出
'    Public MSG_AXS_LIM_Z2 As String                                 ' Z2軸リミット検出
'    Public MSG_AXS_LIM_ATT As String                                ' ATT軸リミット検出

'    ' BP系エラー
'    Public MSG_BP_MOVE_TIMEOUT As String                            ' BP タイムアウト
'    Public MSG_BP_GRV_ALARM_X As String                             ' ガルバノアラームX
'    Public MSG_BP_GRV_ALARM_Y As String                             ' ガルバノアラームY
'    Public MSG_BP_HARD_LIMITOVER_LO As String                       ' BPリミットオーバー（最小可動範囲以下）
'    Public MSG_BP_HARD_LIMITOVER_HI As String                       ' BPリミットオーバー（最大可動範囲以上）
'    Public MSG_BP_LIMITOVER As String                               ' BP移動距離設定リミットオーバー
'    Public MSG_BP_SOFT_LIMITOVER As String                          ' BPソフト可動範囲オーバー
'    Public MSG_BP_BSIZE_OVER As String                              ' ブロックサイズ設定オーバー（ソフト可動範囲オーバー）

'    ' カバー開検出
'    Public MSG_OPN_CVR As String                                    ' "筐体カバー開検出"
'    Public MSG_OPN_SCVR As String                                   ' "スライドカバー開検出"
'    Public MSG_OPN_CVRLTC As String                                 ' "カバー開ラッチ検出"

'    Public MSG_INTIME_ERROR As String                               ' INtime側エラー

'    '(上記のCONST_PPxxは削除予定)
'    Public CONST_PP(256) As String 'データ編集画面のステータスバー表示用バッファ

'    '(上記のCONST_SPxは削除予定)
'    Public CONST_SP(5) As String 'データ編集画面のステータスバー表示用バッファ
'    Public CONST_CA(5) As String 'データ編集画面のステータスバー表示用バッファ
'    Public CONST_CI(5) As String 'データ編集画面のステータスバー表示用バッファ
'    Public CONST_GP(5) As String 'データ編集画面のステータスバー表示用バッファ
'    Public CONST_TY2(5) As String 'データ編集画面のステータスバー表示用バッファ

'    ' メッセージが未定です
'    Public CONST_PR1 As String
'    Public CONST_PR2 As String
'    Public CONST_PR3 As String
'    Public CONST_PR4H As String
'    Public CONST_PR4L As String
'    Public CONST_PR4G As String
'    Public CONST_PR5 As String
'    Public CONST_PR6 As String
'    Public CONST_PR7 As String
'    Public CONST_PR8 As String
'    Public CONST_PR9 As String
'    Public CONST_PR9_1 As String
'    Public CONST_PR10 As String
'    Public CONST_PR11H As String
'    Public CONST_PR11L As String
'    Public CONST_PR12H As String
'    Public CONST_PR12L As String
'    Public CONST_PR13 As String
'    Public CONST_PR14H As String
'    Public CONST_PR14L As String
'    Public CONST_PR15 As String
'    Public CONST_CP2 As String
'    Public CONST_CP99X As String
'    Public CONST_CP99Y As String
'    Public CONST_CP4X As String
'    Public CONST_CP4Y As String
'    Public CONST_CP5 As String
'    Public CONST_CP5_2 As String
'    Public CONST_CP6 As String
'    Public CONST_CP6_2 As String
'    Public CONST_CP7 As String
'    Public CONST_CP7_1 As String
'    Public CONST_CP7_2 As String
'    Public CONST_CP9 As String
'    Public CONST_CP11 As String
'    Public CONST_CP12 As String
'    Public CONST_CP13 As String
'    Public CONST_CP14 As String
'    Public CONST_CP15 As String
'    Public CONST_CP17 As String
'    Public CONST_CP18 As String
'    Public CONST_CP19 As String
'    Public CONST_CP19_A As String
'    Public CONST_CP30 As String
'    Public CONST_CP31 As String
'    Public CONST_CP20 As String
'    Public CONST_CP21 As String
'    Public CONST_CP22 As String
'    Public CONST_CP23 As String
'    Public CONST_CP24 As String
'    Public CONST_CP38 As String
'    Public CONST_CP39 As String
'    Public CONST_CP40 As String
'    Public CONST_CP50 As String
'    Public CONST_CP51 As String
'    Public CONST_CP52 As String
'    Public CONST_CP53 As String
'    Public CONST_CP54 As String
'    Public CONST_CP55 As String
'    Public CONST_CP56 As String
'    Public CONST_CP57 As String

'    Public CONST_CPR1 As String
'    Public CONST_CPR2 As String
'    Public STS_21 As String
'    Public STS_22 As String
'    Public STS_23 As String
'    Public STS_24 As String
'    Public STS_26 As String
'    Public STS_27 As String
'    Public STS_28 As String
'    Public STS_29 As String
'    Public STS_69 As String

'    ' データ読み込みおよび編集終了時のエラー
'    Public CONST_LOAD01 As String '削除予定
'    Public CONST_LOAD06 As String '削除予定
'    Public CONST_LOAD08 As String '削除予定
'    Public CONST_LOAD99 As String '削除予定
'    Public LOAD_MSG01 As String '"指定したファイルが見つかりません。"

'    ' 項目
'    ' ボタン押下時メッセージ
'    Public MSG_BTN_CANCEL As String
'    ' プローブ位置合わせメッセージ
'    Public MSG_PRB_XYMODE As String
'    Public MSG_PRB_BPMODE As String
'    Public MSG_PRB_ZTMODE As String
'    Public MSG_PRB_THETA As String
'    ' システムエラーメッセージ
'    Public MSG_ERR_ZNOTORG As String
'    ' レーザー調整画面説明文
'    Public MSG_LASER_LASERON As String
'    Public MSG_LASER_LASEROFF As String
'    Public MSG_LASEROFF As String
'    Public MSG_LASERON As String
'    Public MSG_ATTRATE As String
'    Public MSG_ERRQRATE As String
'    Public MSG_LOGERROR As String
'    ' 画像登録画面説明文
'    Public MSG_PATERN_MSG01 As String

'    ' 操作ログ　メッセージ
'    Public MSG_OPLOG_WAKEUP As String
'    Public MSG_OPLOG_FUNC01 As String
'    Public MSG_OPLOG_FUNC02 As String
'    Public MSG_OPLOG_FUNC03 As String
'    Public MSG_OPLOG_FUNC04 As String
'    Public MSG_OPLOG_FUNC05 As String
'    Public MSG_OPLOG_FUNC06 As String
'    Public MSG_OPLOG_FUNC07 As String
'    Public MSG_OPLOG_FUNC08 As String
'    Public MSG_OPLOG_FUNC09 As String
'    Public MSG_OPLOG_FUNC20 As String
'    Public MSG_OPLOG_FUNC10 As String
'    Public MSG_OPLOG_FUNC30 As String 'サーキットティーチング
'    Public MSG_OPLOG_AUTO As String
'    Public MSG_OPLOG_LOADERINIT As String
'    Public MSG_OPLOG_CLRTOTAL As String
'    Public MSG_OPLOG_TRIMST As String
'    Public MSG_OPLOG_TRIMRES As String
'    Public MSG_OPLOG_HCMD_ERRRST As String
'    Public MSG_OPLOG_HCMD_PATCMD As String
'    Public MSG_OPLOG_HCMD_LASCMD As String
'    Public MSG_OPLOG_HCMD_MARKCMD As String
'    Public MSG_OPLOG_HCMD_LODCMD As String
'    Public MSG_OPLOG_HCMD_TECCMD As String
'    Public MSG_OPLOG_HCMD_TRMCMD As String
'    Public MSG_OPLOG_HCMD_LSTCMD As String
'    Public MSG_OPLOG_HCMD_LENCMD As String
'    Public MSG_OPLOG_HCMD_MDAUTO As String
'    Public MSG_OPLOG_HCMD_MDMANU As String
'    Public MSG_OPLOG_HCMD_CPCMD As String
'    Public MSG_POPUP_MESSAGE As String
'    Public MSG_OPLOG_FUNC08S As String
'    '----- V1.13.0.0③↓ -----
'    ' TKY系オプション
'    Public MSG_OPLOG_APROBEREC As String        ' オートプローブ登録
'    Public MSG_OPLOG_APROBEEXE As String        ' オートプローブ実行
'    Public MSG_OPLOG_IDTEACH As String          ' ＩＤティーチング
'    Public MSG_OPLOG_SINSYUKU As String         ' 伸縮補正(画像登録)
'    Public MSG_OPLOG_MAP As String              ' MAP
'    '----- V1.13.0.0③↑ -----
'    Public MSG_OPLOG_MAINT As String
'    Public MSG_OPLOG_PRBCLEAN As String

'    '----- V1.18.0.1①↓ -----
'    Public MSG_F_EXR1 As String
'    Public MSG_F_EXTEACH As String
'    Public MSG_F_CARREC As String
'    Public MSG_F_CAR As String
'    Public MSG_F_CUTREC As String
'    Public MSG_F_CUTREV As String
'    Public MSG_OPLOG_CMD As String
'    '----- V1.18.0.1①↑ -----

'    'BLICK MOVE処理用メッセージ
'    Public MSG_txtLog_BLOCKMOVE As String
'    Public MSG_ERRTRIMVAL As String
'    Public MSG_ERRCHKMEASM As String

'    ' メイン画面ラベル
'    Public MSG_MAIN_LABEL01 As String
'    Public MSG_MAIN_LABEL02 As String
'    Public MSG_MAIN_LABEL03 As String
'    Public MSG_MAIN_LABEL04 As String
'    Public MSG_MAIN_LABEL05 As String
'    Public MSG_MAIN_LABEL06 As String
'    Public MSG_MAIN_LABEL07 As String

'    ' [CIRCUIT]

'    ' [registor]
'    Public MSG_REGISTER_LABEL01 As String
'    Public MSG_REGISTER_LABEL02 As String
'    Public MSG_REGISTER_LABEL03 As String
'    Public MSG_REGISTER_LABEL04 As String
'    Public MSG_REGISTER_LABEL05 As String
'    Public MSG_REGISTER_LABEL06 As String
'    Public MSG_REGISTER_LABEL07 As String
'    Public MSG_REGISTER_LABEL08 As String
'    Public MSG_REGISTER_LABEL09 As String
'    Public MSG_REGISTER_LABEL10 As String
'    Public MSG_REGISTER_LABEL11 As String
'    Public MSG_REGISTER_LABEL12 As String
'    Public MSG_REGISTER_LABEL13 As String
'    Public MSG_REGISTER_LABEL14 As String
'    Public MSG_REGISTER_LABEL15 As String
'    Public MSG_REGISTER_LABEL16 As String
'    Public MSG_REGISTER_LABEL17 As String
'    Public MSG_REGISTER_LABEL18 As String
'    Public MSG_REGISTER_LABEL19 As String
'    Public MSG_REGISTER_LABEL20 As String
'    Public MSG_REGISTER_LABEL21 As String
'    Public MSG_REGISTER_LABEL22 As String
'    Public MSG_REGISTER_LABEL25 As String
'    Public MSG_REGISTER_LABEL26 As String
'    Public MSG_REGISTER_LABEL27 As String
'    Public MSG_REGISTER_LABEL33 As String
'    Public MSG_REGISTER_LABEL34 As String
'    Public MSG_REGISTER_LABEL35 As String
'    Public MSG_REGISTER_LABEL36 As String
'    Public MSG_REGISTER_LABEL37 As String

'    ' 編集　[cut]
'    Public MSG_CUT_LABEL01 As String
'    Public MSG_CUT_LABEL02 As String
'    Public MSG_CUT_LABEL03 As String
'    Public MSG_CUT_LABEL04 As String
'    Public MSG_CUT_LABEL05 As String
'    Public MSG_CUT_LABEL06 As String
'    Public MSG_CUT_LABEL07 As String
'    Public MSG_CUT_LABEL08 As String
'    Public MSG_CUT_LABEL09 As String
'    Public MSG_CUT_LABEL10 As String
'    Public MSG_CUT_LABEL11 As String
'    Public MSG_CUT_LABEL12 As String
'    Public MSG_CUT_LABEL13 As String
'    Public MSG_CUT_LABEL14 As String
'    Public MSG_CUT_LABEL15 As String

'    Public MSG_CUT_LABEL16 As String
'    Public MSG_CUT_LABEL17 As String
'    Public MSG_CUT_LABEL18 As String
'    Public MSG_CUT_LABEL19 As String
'    Public MSG_CUT_LABEL20 As String
'    Public MSG_CUT_LABEL21 As String
'    Public MSG_CUT_LABEL22 As String
'    Public MSG_CUT_LABEL23 As String
'    Public MSG_CUT_LABEL24 As String
'    Public MSG_CUT_LABEL25 As String
'    Public MSG_CUT_LABEL26 As String
'    Public MSG_CUT_LABEL27 As String
'    Public MSG_CUT_LABEL28 As String
'    Public MSG_CUT_LABEL29 As String
'    Public MSG_CUT_LABEL30 As String
'    Public MSG_CUT_LABEL31 As String
'    Public MSG_CUT_LABEL32 As String
'    Public MSG_CUT_LABEL33 As String
'    Public MSG_CUT_LABEL34 As String
'    Public MSG_CUT_LABEL35 As String
'    Public MSG_CUT_LABEL36 As String
'    Public MSG_CUT_LABEL37 As String
'    Public MSG_CUT_LABEL38 As String
'    Public MSG_CUT_LABEL39 As String
'    Public MSG_CUT_LABEL40 As String

'    ' ■ティーチング
'    Public MSG_TEACH_LABEL01 As String
'    Public MSG_TEACH_LABEL02 As String
'    Public MSG_TEACH_LABEL03 As String
'    Public MSG_TEACH_LABEL04 As String
'    Public MSG_TEACH_LABEL05 As String

'    ' ■プローブ位置合わせ
'    Public MSG_PROBE_LABEL01 As String
'    Public MSG_PROBE_LABEL02 As String
'    Public MSG_PROBE_LABEL03 As String
'    Public MSG_PROBE_LABEL04 As String
'    Public MSG_PROBE_LABEL05 As String
'    Public MSG_PROBE_LABEL06 As String
'    Public MSG_PROBE_LABEL14 As String

'    Public MSG_PROBE_MSG01 As String
'    Public MSG_PROBE_MSG02 As String
'    Public MSG_PROBE_MSG03 As String
'    Public MSG_PROBE_MSG04 As String
'    Public MSG_PROBE_MSG05 As String
'    Public MSG_PROBE_MSG06 As String
'    Public MSG_PROBE_MSG07 As String
'    Public MSG_PROBE_MSG08 As String
'    Public MSG_PROBE_MSG09 As String
'    Public MSG_PROBE_MSG10 As String
'    Public MSG_PROBE_MSG11 As String
'    Public MSG_PROBE_MSG12 As String
'    Public MSG_PROBE_MSG13 As String
'    Public MSG_PROBE_MSG14 As String
'    Public MSG_PROBE_MSG15 As String
'    Public MSG_PROBE_MSG16 As String

'    ' ■frmMsgBox 画面終了確認
'    Public MSG_CLOSE_LABEL01 As String
'    Public MSG_CLOSE_LABEL02 As String
'    Public MSG_CLOSE_LABEL03 As String
'    ' ■frmReset 原点復帰画面など
'    Public MSG_FRMRESET_LABEL01 As String
'    ' ■LASER_teaching
'    Public MSG_LASER_LABEL01 As String
'    Public MSG_LASER_LABEL02 As String
'    Public MSG_LASER_LABEL03 As String                  ' 加工条件番号
'    Public MSG_LASER_LABEL04 As String                  ' 加工条件
'    Public MSG_LASER_LABEL05 As String                  ' Q SWITCH RATE (KHz)
'    Public MSG_LASER_LABEL06 As String                  ' STEG本数
'    Public MSG_LASER_LABEL07 As String                  ' 電流値(mA)
'    Public MSG_LASER_LABEL08 As String                  ' 加工条件番号を指定して下さい。

'    Public MSG_CUT_1 As String
'    Public LBL_CUT_COPYLINE As String
'    Public LBL_CUT_COPYCOLUMN As String

'    ' ■パワー調整(FL用) ###066
'    Public MSG_AUTOPOWER_01 As String
'    Public MSG_AUTOPOWER_02 As String
'    Public MSG_AUTOPOWER_03 As String
'    Public MSG_AUTOPOWER_04 As String
'    Public MSG_AUTOPOWER_05 As String
'    Public MSG_AUTOPOWER_06 As String 'V1.16.0.0⑧

'    ' 編集画面 カット
'    Public DATAEDIT_ERRMSG01 As String
'    Public DATAEDIT_ERRMSG02 As String
'    Public DATAEDIT_ERRMSG03 As String
'    Public DATAEDIT_ERRMSG04 As String
'    Public DATAEDIT_ERRMSG05 As String
'    Public DATAEDIT_ERRMSG06 As String
'    Public DATAEDIT_ERRMSG07 As String
'    Public DATAEDIT_ERRMSG08 As String
'    Public DATAEDIT_ERRMSG09 As String
'    Public DATAEDIT_ERRMSG10 As String
'    Public DATAEDIT_ERRMSG11 As String

'    '(トリミングパラメータ項目)
'    Public TRIMPARA(150) As String

'    '(入力範囲用メッセージ)
'    Public INPUTAREA(8) As String
'    Public InputErr(5) As String

'    '(確認メッセージ)
'    Public CHKMSG(20) As String

'    '(編集画面ボタン)
'    Public LBL_CMD_OK As String
'    Public LBL_CMD_CANCEL As String
'    Public LBL_CMD_CLEAR As String
'    Public LBL_CMD_LCOPY As String

'    '(編集の操作説明ラベル)
'    Public DE_EXPLANATION(20) As String
'    Public MSG_ONOFF(1) As String

'    '====================================================================
'    '* Memo *
'    '   2005/02/22      basMsgLanguage.basファイル内データを追加
'    '====================================================================
'    'TX,TY,ExCam
'    'Title
'    Public TITLE_TX As String 'チップサイズ(TX)ティーチング
'    Public TITLE_TY As String 'ステップサイズ(TY)ティーチング
'    Public TITLE_ExCam As String '外部カメラによる第1抵抗ティーチング
'    Public TITLE_ExCam1 As String '外部カメラによるティーチング
'    Public TITLE_T_Theta As String 'Tθティーチング
'    'Frame
'    Public FRAME_TX_01 As String '第１抵抗基準点
'    Public FRAME_TX_02 As String '第１グループの最終抵抗基準点
'    Public FRAME_TY_01 As String '第１ブロック基準点
'    Public FRAME_TY_02 As String '第１グループの最終ブロック基準点
'    Public FRAME_TY_03 As String '第２グループの第１ブロック基準点
'    Public FRAME_C_VAL As String '補正値
'    Public FRAME_ExCam_01 As String 'ブロックＮｏ．（範囲は１～９９）
'    Public FRAME_ExCam_02 As String 'ティーチングポイント
'    'Command Button
'    Public CMD_CANCEL As String 'キャンセル
'    'FlexGrid
'    Public FXGRID_TITLE01 As String 'R No.(固定表示)
'    Public FXGRID_TITLE02 As String 'C No.(固定表示)
'    Public FXGRID_TITLE03 As String 'スタートポイントＸ
'    Public FXGRID_TITLE04 As String 'スタートポイントＹ
'    'Label
'    Public LBL_TXTY_TEACH_01 As String '指定座標
'    Public LBL_TXTY_TEACH_02 As String 'ずれ量(μm)
'    Public LBL_TXTY_TEACH_03 As String '補正量
'    Public LBL_TXTY_TEACH_04 As String '補正比率
'    Public LBL_TXTY_TEACH_05 As String 'ﾁｯﾌﾟｻｲｽﾞ (mm)
'    Public LBL_TXTY_TEACH_06 As String 'ﾌﾞﾛｯｸｻｲｽﾞ(mm)
'    Public LBL_TXTY_TEACH_07 As String '補正前
'    Public LBL_TXTY_TEACH_08 As String '補正後
'    Public LBL_TXTY_TEACH_09 As String 'ｸﾞﾙｰﾌﾟｲﾝﾀｰﾊﾞﾙ(mm)
'    Public LBL_TXTY_TEACH_10 As String 'ﾌﾞﾛｯｸｻｲｽﾞ補正(mm)
'    Public LBL_TXTY_TEACH_11 As String 'ステップインターバル(mm)(追加)
'    Public LBL_TXTY_TEACH_12 As String '第１基準点
'    Public LBL_TXTY_TEACH_13 As String '第２基準点
'    Public LBL_TXTY_TEACH_14 As String 'グループ
'    Public LBL_TXTY_TEACH_15 As String '角度補正(deg)
'    Public LBL_Ex_Cam_01 As String      'Ｘ軸
'    Public LBL_Ex_Cam_02 As String      'Ｙ軸
'    'Info
'    Public INFO_MSG01 As String '第１抵抗基準点を合わせてからSTARTキーを押して下さい
'    Public INFO_MSG02 As String '最終抵抗基準点を合わせてからSTARTキーを押して下さい
'    Public INFO_MSG03 As String '補正値を確認してSTARTキーを押下してください
'    Public INFO_MSG04 As String '第１ブロック基準点を合わせてからSTARTキーを押して下さい
'    Public INFO_MSG05 As String '最終ブロック基準点を合わせてからSTARTキーを押して下さい
'    Public INFO_MSG06 As String '第２グループの第１ブロック基準点を合わせてからSTARTキーを押して下さい
'    Public INFO_REC As String '登録:[START]  キャンセル:[RESET]
'    Public INFO_ST As String '開始:[START]  中止:[STOP]
'    Public INFO_MSG07 As String 'コンソールの矢印キーで位置を合わせます
'    Public INFO_MSG08 As String '位置決定：[START]　中止：[RESET]
'    Public INFO_MSG09 As String 'ティーチングを終了します
'    Public INFO_MSG10 As String '[START]キーを押して下さい。[HALT]キーで１つ前のデータに戻れます
'    Public INFO_MSG11 As String 'ブロック数を入力して下さい。[START]キーを押すと次の処理へ進みます
'    Public INFO_MSG12 As String 'ブロック数を入力して下さい。フロントカバーを閉じると次の処理へ進みます

'    '(TXTY仕様変更対応)
'    Public INFO_MSG13 As String '"チップサイズ　ティーチング"
'    Public INFO_MSG14 As String '"ステップ間インターバル　ティーチング"→"ステージグループ間隔ティーチング"
'    Public INFO_MSG15 As String '"ステップオフセット量　ティーチング"
'    Public INFO_MSG16 As String '"基準位置を合わせて下さい。"
'    Public INFO_MSG17 As String '"移動:[矢印]  決定:[START]  中断:[RESET]" & vbCrLf & "[HALT]で１つ前の処理に戻ります。"
'    Public INFO_MSG18 As String '"第1グループ、第1抵抗基準位置のティーチング"
'    Public INFO_MSG19 As String '"第"
'    Public INFO_MSG20 As String '"グループ、最終抵抗基準位置のティーチング"
'    Public INFO_MSG21 As String '"グループ、第1抵抗基準位置のティーチング"
'    Public INFO_MSG22 As String '"最終グループ、最終抵抗位置のティーチング"
'    Public INFO_MSG23 As String '"グループ間インターバル　ティーチング"→"ＢＰグループ間隔ティーチング"
'    Public INFO_MSG24 As String '"第"
'    Public INFO_MSG25 As String '"ブロックのティーチング
'    Public INFO_MSG26 As String '"最終ブロック
'    Public INFO_MSG27 As String '"Tθティーチング
'    Public INFO_MSG28 As String '"グループ、最終端位置のティーチング"
'    Public INFO_MSG29 As String '"グループ、最先端位置のティーチング"
'    Public INFO_MSG30 As String '"サーキット間隔ティーチング"
'    Public INFO_MSG31 As String '"ステップオフセット量のティーチング"
'    Public INFO_MSG32 As String '"(ＴＸ)"   '###084
'    Public INFO_MSG33 As String '"(ＴＹ)"   '###084
'    '----- V1.19.0.0-33↓ -----
'    Public ERR_PROB_EXE As String   '有効な抵抗(1-999)データがないためこのコマンドは実行できません！
'    Public ERR_TEACH_EXE As String  '有効な抵抗(1-999)又はマーキング抵抗データがないためこのコマンドは実行できません！
'    '----- V1.19.0.0-33↑ -----

'    'Err
'    Public ERR_TXNUM_E As String '抵抗数が１のためこのコマンドは実行できません！
'    Public ERR_TXNUM_B As String 'ブロック数が１のためこのコマンドは実行できません！
'    Public ERR_TXNUM_C As String 'サーキット数が１のためこのコマンドは実行できません！
'    Public ERR_TXNUM_S As String 'ステップ測定回数が0のためこのコマンドは実行できません！'V1.13.0.0③

'    'OperationLogging
'    Public MSG_OPLOG_TX_START As String                         'TXﾃｨｰﾁﾝｸﾞ開始
'    Public MSG_OPLOG_TY_START As String                         'TYﾃｨｰﾁﾝｸﾞ開始
'    Public MSG_OPLOG_TY2_START As String                        'TY2ﾃｨｰﾁﾝｸﾞ開始
'    Public MSG_OPLOG_ExCam_START As String                      '外部カメラティーチング開始
'    Public MSG_OPLOG_ExCam1_START As String                     '外部カメラティーチング開始
'    Public MSG_OPLOG_CitTx_START As String                      'サーキットTXティーチング開始
'    Public MSG_OPLOG_CitTe_START As String                      'サーキットティーチング開始
'    Public MSG_OPLOG_T_THETA_START As String                    'Tθﾃｨｰﾁﾝｸﾞ開始
'    'flg
'    'Public ExCamTeach_Type As Short                             ' (0)第１抵抗のみ　(1)全抵抗
'    'Private FmMsgbox As frmMsgbox
'    Public FRAME_PITCH As String                                ' XYZ/BP ピッチ幅設定
'    Public LBL_PICTH(2) As String                               ' (0)LOW (1)HIGH (2)PAUSE

'    ' OperationLogging
'    Public MSG_OPLOG_CUT_POS_CORRECT_START As String ' ｶｯﾄ位置補正開始
'    Public MSG_OPLOG_CALIBRATION_START As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ開始
'    Public MSG_OPLOG_CUT_POS_CORRECT_RECOG_START As String ' ｶｯﾄ位置補正画像登録開始
'    Public MSG_OPLOG_CALIBRATION_RECOG_START As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ画像登録開始

'    ' ｶｯﾄ位置補正画面文字列
'    Public FRM_CUT_POS_CORRECT_TITLE As String ' ｶｯﾄ位置補正ﾌﾚｰﾑﾀｲﾄﾙ
'    Public LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER As String ' ｸﾞﾙｰﾌﾟ内抵抗数
'    Public LBL_CUT_POS_CORRECT_BLOCK_NO_X As String ' ﾌﾞﾛｯｸ番号X軸
'    Public LBL_CUT_POS_CORRECT_BLOCK_NO_Y As String ' ﾌﾞﾛｯｸ番号Y軸
'    Public LBL_CUT_POS_CORRECT_OFFSET_X As String ' ｶｯﾄ位置補正ｵﾌｾｯﾄX
'    Public LBL_CUT_POS_CORRECT_OFFSET_Y As String ' ｶｯﾄ位置補正ｵﾌｾｯﾄY

'    ' ｷｬﾘﾌﾞﾚｰｼｮﾝ画面文字列
'    Public FRM_CALIBRATION_TITLE As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾌﾚｰﾑﾀｲﾄﾙ
'    Public LBL_CALIBRATION_STANDERD_COORDINATES1X As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1X
'    Public LBL_CALIBRATION_STANDERD_COORDINATES1Y As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1Y
'    Public LBL_CALIBRATION_STANDERD_COORDINATES2X As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2X
'    Public LBL_CALIBRATION_STANDERD_COORDINATES2Y As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2Y
'    Public LBL_CALIBRATION_TABLE_OFFSETX As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄX
'    Public LBL_CALIBRATION_TABLE_OFFSETY As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄY
'    Public LBL_CALIBRATION_GAP1X As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量1X
'    Public LBL_CALIBRATION_GAP1Y As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量1Y
'    Public LBL_CALIBRATION_GAP2X As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量2X
'    Public LBL_CALIBRATION_GAP2Y As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝずれ量2Y
'    Public LBL_CALIBRATION_GAINX As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝｹﾞｲﾝ補正係数X
'    Public LBL_CALIBRATION_GAINY As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝｹﾞｲﾝ補正係数Y
'    Public LBL_CALIBRATION_OFFSETX As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝｵﾌｾｯﾄ補正量X
'    Public LBL_CALIBRATION_OFFSETY As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝｵﾌｾｯﾄ補正量Y
'    Public LBL_CALIBRATION_001 As String '【十字カットモード(基準座標１)】
'    Public LBL_CALIBRATION_002 As String '【十字カットモード(基準座標２)】
'    Public LBL_CALIBRATION_003 As String '【画像認識モード(基準座標１)】
'    Public LBL_CALIBRATION_004 As String '【画像認識モード(基準座標２)】

'    ' Recog
'    Public LBL_CALIBRATION_STANDERD_COORDINATES1 As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標1RECOG
'    Public LBL_CALIBRATION_STANDERD_COORDINATES2 As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝ基準座標2RECOG
'    Public LBL_CALIBRATION_TABLE_OFFSET As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝﾃｰﾌﾞﾙｵﾌｾｯﾄRECOG
'    Public LBL_CALIBRATION_CUT As String ' ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄRECOG
'    Public LBL_CALIBRATION_MOVE1 As String ' 基準座標１へ移動
'    Public LBL_CALIBRATION_MOVE2 As String ' 基準座標２へ移動
'    Public LBL_CUT_POS_CORRECT_CUT As String ' 十字ｶｯﾄ
'    Public LBL_CUT_POS_CORRECT_MOVE As String ' 外部ｶﾒﾗへ移動

'    ' ﾒｯｾｰｼﾞ文字列
'    Public MSG_CUT_POS_CORRECT_001 As String ' ﾌﾞﾛｯｸ番号は１～９９で入力して下さい。
'    Public MSG_CUT_POS_CORRECT_002 As String ' ﾌﾞﾛｯｸNo入力後、[ﾌﾞﾛｯｸ移動]を押してください。
'    Public MSG_CUT_POS_CORRECT_003 As String ' 処理を開始します。" & vbCrLf & "[START]続行" & vbCrLf & "[RESET]中止
'    Public MSG_CUT_POS_CORRECT_004 As String ' 警告" & vbCrLf & "[START]十字カット実行" & vbCrLf & "[RESET]中止"
'    Public MSG_CUT_POS_CORRECT_005 As String ' 十字カット実行中" & vbCrLf & "[ADJ]一時停止"
'    Public MSG_CUT_POS_CORRECT_006 As String ' [START]十字カット実行" & vbCrLf & "[RESET]中止" & vbCrLf & "[ADJ]一時停止解除"
'    Public MSG_CUT_POS_CORRECT_007 As String ' [START]画像認識実行" & vbCrLf & "[RESET]中止"
'    Public MSG_CUT_POS_CORRECT_008 As String ' 画像認識実行中" & vbCrLf & "[ADJ]一時停止"
'    Public MSG_CUT_POS_CORRECT_009 As String ' [START]画像認識実行" & vbCrLf & "[RESET]中止" & vbCrLf & "[ADJ]一時停止解除"
'    Public MSG_CUT_POS_CORRECT_010 As String ' 補正値更新エラー" & vbCrLf & "更新結果が範囲を超えています。" & vbCrLf & "最大値、最小値に補正されます。"
'    Public MSG_CUT_POS_CORRECT_011 As String ' 十字カット終了
'    Public MSG_CUT_POS_CORRECT_012 As String ' 画像認識終了"
'    Public MSG_CUT_POS_CORRECT_013 As String ' 画像マッチングエラー"
'    Public MSG_CUT_POS_CORRECT_014 As String ' "ブロック数を入力して下さい" & vbCrLf & "[START]キーを押すと次の処理へ進みます"
'    Public MSG_CUT_POS_CORRECT_015 As String ' "ブロック番号を入力エラー"
'    Public MSG_CUT_POS_CORRECT_016 As String ' "画像認識実行中"
'    Public MSG_CUT_POS_CORRECT_017 As String ' "矢印スイッチ:位置調整, [START]:次項, [HALT]:前項"
'    Public MSG_CUT_POS_CORRECT_018 As String ' "相関係数="
'    Public MSG_CUT_POS_CORRECT_019 As String ' 画像が見つかりません"

'    Public MSG_CALIBRATION_001 As String ' キャリブレーションを実行します。" & vbCrLf & "[START]を押してください。" & vbcrlf & "[RESET]CANCEL"
'    Public MSG_CALIBRATION_002 As String ' 外部カメラでずれ量を検出します。" & vbCrLf & "[START]を押してください。" & vbCrLf & "[RESET]CANCEL"
'    Public MSG_CALIBRATION_003 As String ' "[STARTキー]：画像認識実行，[RESETキー]：中止"
'    Public MSG_CALIBRATION_004 As String ' "外部カメラでずれ量を検出します。(基準座標１)" & vbCrLf & "[STARTキー]：決定，[RESETキー]：中止"
'    Public MSG_CALIBRATION_005 As String ' "外部カメラでずれ量を検出します。(基準座標２)" & vbCrLf & "[STARTキー]：決定，[RESETキー]：中止"
'    Public MSG_CALIBRATION_006 As String ' "キャリブレーションを終了します。" & "データを保持する場合は[START]を、データを保持しない場合は[RESET]を押して下さい。"
'    Public MSG_CALIBRATION_007 As String ' "画像マッチングエラー" & vbCrLf & "処理を続ける場合は[OK]を、中止する場合は[Cancel]を押して下さい。"
'    Public MSG_CALIBRATION_008 As String ' "Resetします。 [Start]:Reset実行 , [Reset]:Cancel実行 "      ' ###078  

'    ' ■自動運転..
'    Public MSG_AUTO_01 As String '動作モード
'    Public MSG_AUTO_02 As String 'マガジンモード
'    Public MSG_AUTO_03 As String 'ロットモード
'    Public MSG_AUTO_04 As String 'エンドレスモード
'    Public MSG_AUTO_05 As String 'データファイル
'    Public MSG_AUTO_06 As String '登録済みデータファイル
'    Public MSG_AUTO_07 As String 'リストの1つ上へ
'    Public MSG_AUTO_08 As String 'リストの1つ下へ
'    Public MSG_AUTO_09 As String 'リストから削除
'    Public MSG_AUTO_10 As String 'リストをクリア
'    Public MSG_AUTO_11 As String '登録
'    Public MSG_AUTO_12 As String 'OK
'    Public MSG_AUTO_13 As String 'キャンセル
'    Public MSG_AUTO_14 As String 'データ選択'
'    Public MSG_AUTO_15 As String '登録リストを全て削除します。
'    Public MSG_AUTO_16 As String 'よろしいですか？
'    Public MSG_AUTO_17 As String 'エンドレスモード時は複数のデータファイルは選択できません。
'    Public MSG_AUTO_18 As String 'データファイルを選択してください。
'    Public MSG_AUTO_19 As String '編集中のデータを保存しますか？
'    Public MSG_AUTO_20 As String '加工条件ファイルが存在しません。

'    ' ■θ補正..
'    Public MSG_THETA_01 As String '補正位置１
'    Public MSG_THETA_02 As String '補正位置２
'    Public MSG_THETA_03 As String '補正値
'    Public MSG_THETA_04 As String 'START
'    Public MSG_THETA_05 As String 'CANCEL
'    Public MSG_THETA_06 As String '指定座標
'    Public MSG_THETA_07 As String 'ずれ量
'    Public MSG_THETA_08 As String '回転補正角度
'    Public MSG_THETA_09 As String '補正位置1
'    Public MSG_THETA_10 As String '補正位置2
'    Public MSG_THETA_11 As String 'トリム位置
'    Public MSG_THETA_12 As String '補正後
'    Public MSG_THETA_13 As String '補正位置1　移動中
'    Public MSG_THETA_14 As String '補正位置2　移動中
'    Public MSG_THETA_15 As String '補正位置１を合わせてからSTARTキーを押してください
'    Public MSG_THETA_16 As String '補正位置２を合わせてからSTARTキーを押してください
'    Public MSG_THETA_17 As String '補正値を確認してSTARTキーを押してください
'    Public MSG_THETA_18 As String 'Matching error
'    Public MSG_THETA_19 As String 'Pattern Matching error(Position1)
'    Public MSG_THETA_20 As String 'Pattern Matching error(Position1)
'    Public MSG_THETA_21 As String 'Pattern Matching OK(Position1)
'    Public MSG_THETA_22 As String 'Pattern Matching
'    ' ■ﾄﾘﾐﾝｸﾞ..
'    Public MSG_TRIM_01 As String 'は加工範囲外です。
'    Public MSG_TRIM_02 As String 'ローダー自動運転の起動ができませんでした。
'    Public MSG_TRIM_03 As String '処理を続行しますか？
'    Public MSG_TRIM_04 As String 'イニシャルテスト　分布図
'    Public MSG_TRIM_05 As String 'ファイナルテスト　分布図
'    Public MSG_TRIM_06 As String '自動ポジション
'    Public BTN_TRIM_01 As String 'ｸﾞﾗﾌﾃﾞｰﾀ ｸﾘｱ
'    Public BTN_TRIM_02 As String 'ﾌﾟﾚｰﾄﾃﾞｰﾀ編集
'    Public BTN_TRIM_03 As String 'ｲﾆｼｬﾙﾃｽﾄ分布表示
'    Public BTN_TRIM_04 As String 'ﾌｧｲﾅﾙﾃｽﾄ分布表示
'    Public PIC_TRIM_01 As String 'イニシャルテスト　分布図
'    Public PIC_TRIM_02 As String 'ファイナルテスト　分布図
'    Public PIC_TRIM_03 As String '良品
'    Public PIC_TRIM_04 As String '不良品
'    Public PIC_TRIM_05 As String '最小%
'    Public PIC_TRIM_06 As String '最大%
'    Public PIC_TRIM_07 As String '平均%
'    Public PIC_TRIM_08 As String '標準偏差
'    Public PIC_TRIM_09 As String '抵抗数
'    Public PIC_TRIM_10 As String '分布図保存 
'    ' ■ﾛｰﾀﾞｰ..
'    Public MSG_LOADER_01 As String '全停止異常発生中
'    Public MSG_LOADER_02 As String 'ｻｲｸﾙ停止異常発生中
'    Public MSG_LOADER_03 As String '軽故障発生中
'    Public MSG_LOADER_04 As String '設定が完了しました。
'    Public MSG_LOADER_05 As String 'ﾛｰﾀﾞｰｴﾗｰ
'    Public MSG_LOADER_06 As String 'ﾛｰﾀﾞｰｲﾝﾀｰﾛｯｸ
'    Public MSG_LOADER_07 As String 'パターンマッチングエラー
'    Public MSG_LOADER_08 As String '連続NG-HIｴﾗｰ
'    Public MSG_LOADER_09 As String 'フルパワー測定 Q Rate 10kHz
'    Public MSG_LOADER_10 As String 'レーザーパワーばらつきエラー
'    Public MSG_LOADER_11 As String 'フルパワー減衰エラー
'    Public MSG_LOADER_12 As String '調整フルパワー測定
'    Public MSG_LOADER_13 As String 'パワー調整
'    Public MSG_LOADER_14 As String 'レーザーパワー調整エラー
'    Public MSG_LOADER_15 As String '自動運転終了
'    Public MSG_LOADER_16 As String 'ﾛｰﾀﾞｰｱﾗｰﾑﾘｽﾄ
'    Public MSG_LOADER_17 As String '載物台上に基板が残っている場合は
'    Public MSG_LOADER_18 As String '取り除いてください
'    Public MSG_LOADER_19 As String '基板品種が変わりました                          ' ###089
'    Public MSG_LOADER_20 As String 'ＮＧ排出ボックスからＮＧ基板を取り除いてから    ' ###089
'    Public MSG_LOADER_21 As String 'ＮＧ排出ボックス満杯                            ' ###089
'    Public MSG_LOADER_22 As String 'STARTキー又はOKボタン押下で原点復帰します。     ' ###124
'    Public MSG_LOADER_23 As String '自動運転を中止します                            ' ###124
'    Public MSG_LOADER_24 As String '載物台クランプ解除                              ' ###144
'    Public MSG_LOADER_25 As String '載物台吸着解除                                  ' ###144
'    Public MSG_LOADER_26 As String 'ハンド吸着解除                                  ' ###144
'    Public MSG_LOADER_27 As String '供給ハンド吸着解除                              ' ###158
'    Public MSG_LOADER_28 As String '収納ハンド吸着解除                              ' ###158
'    Public MSG_LOADER_29 As String '装置内に基板が残っている場合は                  ' ###158
'    Public MSG_LOADER_30 As String '自動運転停止中                            　　　' ###172
'    Public MSG_LOADER_31 As String 'OKボタン押下でアプリケーションを終了します      ' ###175
'    Public MSG_LOADER_32 As String '載物台の基板を取り除いてから"                   ' ###184 
'    Public MSG_LOADER_33 As String '再度自動運転を実行して下さい"                   ' ###184 
'    Public MSG_LOADER_34 As String 'マガジンを基板検出センサＯＦＦ位置まで"         ' ###184 
'    Public MSG_LOADER_35 As String '下げてください"                                 ' ###184 
'    Public MSG_LOADER_36 As String '載物台の基板を取り除いて下さい"                 ' ###184 
'    '----- ###188↓ -----
'    Public MSG_LOADER_37 As String '載物台上に基板が残っています" 
'    Public MSG_LOADER_38 As String 'STARTキー又はOKボタン押下で"                 
'    Public MSG_LOADER_39 As String 'ステージを原点に戻します"                 
'    '----- ###188↑ -----
'    Public MSG_LOADER_40 As String 'ＮＧ基板を取り除いて下さい"                     ' ###197     
'    Public MSG_LOADER_41 As String 'ＮＧ基板を取り除いてから"                       ' ###197     
'    '----- ###240↓ -----
'    Public MSG_LOADER_42 As String '載物台に基板がありません" 
'    Public MSG_LOADER_43 As String '基板を置いて再度実行して下さい"                 
'    '----- ###240↑ -----
'    '----- V1.18.0.0⑨↓ -----
'    Public MSG_LOADER_44 As String 'マガジン終了、マガジン交換後STARTキー又は"
'    Public MSG_LOADER_45 As String 'OKボタン押下で処理を続行します"
'    Public MSG_LOADER_46 As String 'RESETキー又はCancelボタン押下で処理を終了します"
'    '----- V1.18.0.0⑨↑ -----
'    Public MSG_LOADER_47 As String 'プローブチェックエラー" V1.23.0.0⑦
'    Public MSG_LOADER_48 As String 'サイクル停止中" 'V4.0.0.0⑲
'    Public MSG_LOADER_49 As String '割欠検出" 'V4.0.0.0⑲
    Public MSG_LOADER_50 As String '基板を投入してください" 'V4.11.0.0⑥

'    Public FRAME_TY2_01 As String 'ﾃｨｰﾁﾝｸﾞﾎﾟｲﾝﾄ
'    Public INFO_REC_TX As String '登録:[START]  キャンセル:[RESET]  登録+TX2:[HI+ADV]
'    Public INFO_REC_TY As String '登録:[START]  キャンセル:[RESET]  登録+TY2:[HI+ADV]
'    Public INFO_REC_TTHETA As String '登録:[START]  キャンセル:[RESET]
'    Public TITLE_TY2 As String 'ステップサイズ(TY)ティーチング
'    Public MSG_EXECUTE_TXTYLABEL As String 'TX,TY
'    Public LBL_STEP_STEP As String
'    Public LBL_STEP_GROUP As String
'    Public LBL_STEP_TY2 As String
'    Public LBL_ATT As String '減衰率
'    Public LBL_FLCUR As String '電流値

'    Public CIRTEACH_MSG00 As String '第
'    Public CIRTEACH_MSG01 As String 'サーキット基準点ＸＹを合わせてから｢START｣キーを押して下さい

'    Public CIRTY_MSG00 As String '第１サーキット基準点
'    Public CIRTY_MSG01 As String '第１グループの最終サーキット基準点
'    Public CIRTY_MSG02 As String '第２グループの第１サーキット基準点
'    Public CIRTY_MSG03 As String 'ｻｰｷｯﾄｻｲｽﾞ(mm)

'    Public STEP_TITLE01 As String 'サーキット座標
'    Public STEP_TITLE02 As String 'サーキット間インターバル
'    Public STEP_TITLE03 As String 'ステップ間インターバル

'    ' ■ローダアラームメッセージ 
'    Public MSG_LDALARM_00 As String     ' "非常停止"
'    Public MSG_LDALARM_01 As String     ' "マガジン整合性アラーム"
'    Public MSG_LDALARM_02 As String     ' "割れ欠け品発生"
'    Public MSG_LDALARM_03 As String     ' "ハンド１吸着アラーム"
'    Public MSG_LDALARM_04 As String     ' "ハンド２吸着アラーム"
'    Public MSG_LDALARM_05 As String     ' "載物台吸着センサ異常"
'    Public MSG_LDALARM_06 As String     ' "載物台吸着ミス"
'    Public MSG_LDALARM_07 As String     ' "ロボットアラーム"
'    Public MSG_LDALARM_08 As String     ' "工程間監視アラーム"
'    Public MSG_LDALARM_09 As String     ' "エレベータ異常"
'    Public MSG_LDALARM_10 As String     ' "マガジン無し"
'    Public MSG_LDALARM_11 As String     ' "原点復帰タイムアウト"
'    Public MSG_LDALARM_12 As String     ' "クランプ異常" '###125
'    Public MSG_LDALARM_13 As String     ' "エアー圧低下検出"
'    Public MSG_LDALARM_14 As String     ' "未定義アラーム No."
'    Public MSG_LDALARM_15 As String     ' "未定義アラーム No."

'    Public MSG_LDALARM_16 As String     ' "旋回シリンダタイムアウト"
'    Public MSG_LDALARM_17 As String     ' "ハンド１シリンダタイムアウト"
'    Public MSG_LDALARM_18 As String     ' "ハンド２シリンダタイムアウト"
'    Public MSG_LDALARM_19 As String     ' "供給ハンド吸着ミス"
'    Public MSG_LDALARM_20 As String     ' "回収ハンド吸着ミス"
'    Public MSG_LDALARM_21 As String     ' "ＮＧ排出満杯"
'    Public MSG_LDALARM_22 As String     ' "一時停止"
'    Public MSG_LDALARM_23 As String     ' "ドアオープン"
'    Public MSG_LDALARM_24 As String     ' "未定義アラーム No."
'    Public MSG_LDALARM_25 As String     ' "二枚取り検出"
'    Public MSG_LDALARM_26 As String     ' "搬送上下機構アラーム"
'    Public MSG_LDALARM_27 As String     ' "未定義アラーム No."
'    Public MSG_LDALARM_28 As String     ' "未定義アラーム No."
'    Public MSG_LDALARM_29 As String     ' "未定義アラーム No."
'    Public MSG_LDALARM_30 As String     ' "未定義アラーム No."
'    Public MSG_LDALARM_31 As String     ' "未定義アラーム No."
'    Public MSG_LDALARM_UD As String     ' "未定義アラーム No."

'    Public MSG_LDGUID_00 As String     ' "非常停止ＳＷが押されました。"
'    Public MSG_LDGUID_01 As String     ' "マガジンが正規の位置か確認してください。"
'    Public MSG_LDGUID_02 As String     ' "割れ欠け品が指定された枚数発生しました。"
'    Public MSG_LDGUID_03 As String     ' "吸着センサを確認してください。"
'    Public MSG_LDGUID_04 As String     ' "吸着センサを確認してください。"
'    Public MSG_LDGUID_05 As String     ' "吸着センサを確認してください。"
'    Public MSG_LDGUID_06 As String     ' "吸着センサを確認してください。" "トッププレートにキズや基板のかけらが無いか確認してください。"
'    Public MSG_LDGUID_07 As String     ' "ロボットアラームが発生しました。"
'    Public MSG_LDGUID_08 As String     ' "工程間監視でタイムアウトが発生しました。"
'    Public MSG_LDGUID_09 As String     ' "エレベータのセンサを確認してください。"
'    Public MSG_LDGUID_10 As String     ' "マガジン検出ドグが倒れているか、マガジンをセットしてください。"
'    Public MSG_LDGUID_11 As String     ' "原点復帰時にタイムアウトが発生しました。"
'    Public MSG_LDGUID_12 As String     ' "クランプ異常" '###125
'    Public MSG_LDGUID_13 As String     ' "エアーが正しく供給されているか確認してください。"
'    Public MSG_LDGUID_14 As String     ' "想定外アラーム"
'    Public MSG_LDGUID_15 As String     ' "想定外アラーム"

'    Public MSG_LDGUID_16 As String     ' "シリンダセンサを確認してください。"
'    Public MSG_LDGUID_17 As String     ' "シリンダセンサを確認してください。"
'    Public MSG_LDGUID_18 As String     ' "シリンダセンサを確認してください。"
'    Public MSG_LDGUID_19 As String     ' "基板吸着時にタイムアウトが発生しました。"
'    Public MSG_LDGUID_20 As String     ' "基板吸着時にタイムアウトが発生しました。"
'    Public MSG_LDGUID_21 As String     ' "ＮＧ排出ＢＯＸが満杯です。基板を取り除いてください。"
'    Public MSG_LDGUID_22 As String     ' "一時停止中です。"
'    Public MSG_LDGUID_23 As String     ' "ドアオープンが検出されました。ドアを閉じてください。"
'    Public MSG_LDGUID_24 As String     ' "供給ハンドの基板を取り除いて再実行してください。" V1.18.0.0⑪
'    Public MSG_LDGUID_25 As String     ' "搬送上下機構を確認してください。" 'V4.0.0.0-59
'    Public MSG_LDGUID_26 As String     ' "想定外アラーム"
'    Public MSG_LDGUID_27 As String     ' "想定外アラーム"
'    Public MSG_LDGUID_28 As String     ' "想定外アラーム"
'    Public MSG_LDGUID_29 As String     ' "想定外アラーム"
'    Public MSG_LDGUID_30 As String     ' "想定外アラーム"
'    Public MSG_LDGUID_31 As String     ' "想定外アラーム"
'    Public MSG_LDGUID_UD As String     ' "想定外アラーム"

'    Public MSG_LDINFO_UD As String     ' "想定外アラーム"

'    'V1.13.0.0⑥ ADD START
'    ' ■ＩＤリーダーメッセージ 
'    Public LBL_IDREADER_TEACH_01 As String  ' ＩＤ　リーダー　ティーチング
'    Public LBL_IDREADER_TEACH_02 As String  ' 第１ ＩＤ読み取り位置
'    Public LBL_IDREADER_TEACH_03 As String  ' 第２ ＩＤ読み取り位置
'    Public LBL_IDREADER_TEACH_04 As String  ' ＩＤリーダー
'    'V1.13.0.0⑥ ADD END
'    'V2.0.0.0⑰
'    ' ■プローブクリーニングメッセージ 
'    Public LBL_PROBECLEANING_TEACH As String ' プローブクリーニング位置ティーチング

'#End Region

'#Region "メッセージを設定する(日本語/英語)"
'    '''=========================================================================
'    '''<summary>メッセージを設定する(日本語/英語)</summary>
'    '''<param name="language">(INP)言語(0:Japanese, 1:English, 2:Europe)</param>
'    '''=========================================================================
'    Public Sub PrepareMessages(ByVal language As Short)

'        Select Case language
'            Case 0
'                Call PrepareMessagesJapanese()
'            Case 1
'                Call PrepareMessagesEnglish()
'            Case Else
'                Call PrepareMessagesEnglish()
'        End Select

'    End Sub
'#End Region

'#Region "日本語メッセージを設定する"
'    '''=========================================================================
'    '''<summary>日本語メッセージを設定する</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesJapanese()

'        '-----------------------------------------------------------------------
'        '   エラーメッセージ
'        '-----------------------------------------------------------------------
'        MSG_0 = "すでにこのプログラムは実行されています"
'        MSG_11 = "%1行目と抵抗NOが重複しています"
'        MSG_14 = "トリミングパラメータデータをロードするか新規作成してください"
'        MSG_15 = "指定されたファイルは存在しません"
'        MSG_16 = "指定されたファイルはトリミングパラメータのデータではありません"
'        MSG_21 = "ブロックサイズ X を正しく入力してください"
'        MSG_22 = "ブロックサイズ Y を正しく入力してください"
'        MSG_25 = "動作モード（デジスイッチ）の設定を x0～x6 にしてください"
'        MSG_36 = "加工範囲外です"
'        MSG_37 = "対象をクロスラインの中心に合わせて、選択領域がクロスラインの中心を含むようにしてください"
'        MSG_38 = "パターン登録のテンプレートグループ番号（PP37')を確認してください"
'        MSG_39 = "カット位置補正対象の抵抗がありません"
'        MSG_40 = "下降距離制限内で一部のプローブしか接触しませんでした"
'        MSG_41 = "下降距離制限に達しました。自動調整はできませんでした"
'        MSG_42 = "Z軸の下限リミットに達しました。自動調整はできませんでした　"
'        MSG_43 = "再確認時に非接触を検知しました。自動調整はできませんでした　"
'        MSG_44 = "自動調整は完了しました　　　　　　　　　　　　　　　　　　　"
'        MSG_45 = "加工条件データをロードしてください　　　　　　　　　　　　　 "
'        MSG_46 = "加工条件"

'        MSG_101 = "を上書きしますか　　　　　　　　　　　　　　　　　　　　　　"
'        MSG_102 = "アプリケーションを終了してよろしいですか？　　　　　　　　　"
'        MSG_103 = "よろしいですか？　　　　　　　　　　　　　　　　　　　　　　"
'        MSG_104 = "編集したデータが保存されていません　　　　　　　　　　　　　"
'        MSG_105 = "前の画面に戻ります。よろしいですか？　　　　　　　　　　　　"
'        MSG_106 = "この情報を保存して前の画面に戻ります。よろしいですか？　　　"
'        MSG_108 = "累計をクリアしてもよろしいですか？　　　　　　　　　　　　　"

'        MSG_114 = "集塵機異常が発生しました　　　　　　　　　　　　　　　　　　"
'        MSG_115 = "編集中のデータがあります。ロードしますか？　　　　　　　　　"
'        MSG_116 = "データロード確認　　　　　　　　　　　　　　　　　　　　　　"
'        MSG_117 = "編集中のデータがあります。　　　　　　　　　　　　　　　　　"
'        MSG_118 = "補正モードが無しに設定されている場合は、実行できません。　　"
'        MSG_119 = "半角英数1～18文字以内で入力して下さい"
'        MSG_120 = "キャリブレーションを終了します。"
'        MSG_121 = "データを保持する場合は「はい」を、データを保持しない場合は「いいえ」を押して下さい。"
'        MSG_122 = "ｶｯﾄｽﾋﾟｰﾄﾞの上限値が不正です。" & vbCrLf & vbCrLf & "ST2ｶｯﾄを含む場合、ｶｯﾄｽﾋﾟｰﾄﾞの上限値は%1mm/sです。" & vbCrLf & "(対象は、ST2ｶｯﾄより前のｶｯﾄ全てです。)"
'        MSG_126 = "パターン認識エラーです。手動モードで実行しますか？"
'        MSG_127 = "パターン認識エラー" 'V1.13.0.0③

'        MSG_130 = "トリミングデータファイル入力エラー"
'        MSG_131 = "トリミングデータファイル出力エラー"
'        MSG_132 = "加工条件の送信に失敗しました。" + vbCrLf + "再度データをロードするか、編集画面から加工条件の設定を行ってください。"
'        MSG_133 = "中間ファイルの読込みに失敗しました。"
'        MSG_134 = "中間ファイルの書き込みに失敗しました。"
'        MSG_135 = "データ編集エラー。データを再ロードしてください。"
'        MSG_136 = "シリアルポートＯＰＥＮエラー"
'        MSG_137 = "シリアルポートＣＬＯＳＥエラー"
'        MSG_138 = "シリアルポート送信エラー"
'        MSG_139 = "シリアルポート受信エラー"
'        MSG_140 = "ＦＬ側の加工条件の設定がありません。" + vbCrLf + "再度データをロードするか、編集画面から加工条件の設定を行ってください。"
'        MSG_141 = "ＦＬ側加工条件のリードに失敗しました。"
'        MSG_142 = "加工条件ファイルを作成しました"
'        MSG_143 = "データをロードしました"
'        MSG_144 = "データロードＮＧ"
'        MSG_145 = "データをセーブしました"
'        MSG_146 = "データセーブＮＧ"
'        MSG_147 = "ＦＬへ加工条件を送信しました。"
'        MSG_148 = "ＦＬへデータ送信中・・・・・・"
'        MSG_149 = "ファイバレーザの加工条件が変更されている可能性があります。ロードしますか？　　　　　　　　　"
'        MSG_150 = "ＦＬ通信異常。ＦＬとの通信に失敗しました。" + vbCrLf + "ＦＬと正しく接続できているか確認してください。"
'        MSG_151 = "加工条件の設定に失敗しました。"
'        '----- V1.18.0.0③↓ -----
'        MSG_152 = "印刷初期処理に失敗しました。"
'        MSG_153 = "印刷処理に失敗しました。"
'        MSG_154 = "印刷終了処理に失敗しました。"
'        '----- V1.18.0.0③↑ -----
'        MSG_155 = "基板枚数をクリアしてもよろしいですか？" 'V1.18.0.0⑫
'        MSG_156 = "プローブチェックエラー"                 'V1.23.0.0⑦
'        MSG_157 = "指定されたバーコードは存在しません" 'V1.23.0.0①
'        MSG_158 = "加工条件ファイルリードエラー。" 'V2.0.0.0⑤
'        MSG_159 = "ファイルが存在します。上書きしても宜しいですか？" 'V4.0.0.0⑨
'        '----- V4.0.0.0-30↓ -----
'        MSG_160 = "プローブクリーニングのステージ位置を合わせて下さい。"
'        MSG_161 = "移動：[矢印]  決定：[START]  中断：[RESET]"
'        MSG_162 = "プローブクリーニングのプローブ位置を合わせて下さい。"
'        '----- V4.0.0.0-30↑ -----

'        MSGERR_COVER_CLOSE = "スライドカバーの自動クローズに失敗しました。"
'        MSGERR_SEND_TRIMDATA = "トリミングデータの設定に失敗しました。" & vbCrLf & "トリミングデータに問題がないか確認してください。"

'        MSG_PP23 = "プローブZオフセットの距離をZ軸の上昇距離よりも大きくしてください"
'        MSG_PP24_1 = "プローブZ軸の上昇距離がZ軸待機位置の距離よりも越えています"
'        MSG_PP24_2 = "Z軸の上昇距離よりも、待機位置の距離を大きくしてください"

'        '''' NETの場合
'        MSG_TOTAL_CIRCUIT = "サーキット単位"
'        '''' CHIPの場合
'        MSG_TOTAL_REGISTOR = "抵抗単位"

'        MSG_COM = "数値入力範囲エラー（%1～%2）"
'        MSG_PR_COM1 = "数値入力範囲エラー（%1～%2）"
'        MSG_PR04_1 = "%1と同じ番号が指定されました。"
'        '
'        LOAD_MSG01 = "指定したファイルが見つかりません。"

'        MSG_CUT_1 = "コピーできるのは同一抵抗番号間のみです。"

'        '(データ編集画面のエラーMSG)
'        DATAEDIT_ERRMSG01 = "｢RESISTOR｣の処理を行って下さい"
'        DATAEDIT_ERRMSG02 = "｢CUT｣の処理を行って下さい"
'        DATAEDIT_ERRMSG03 = "入力エラー"
'        DATAEDIT_ERRMSG04 = "%1と%2が重複しています"
'        DATAEDIT_ERRMSG05 = "パルス幅時間を "
'        DATAEDIT_ERRMSG06 = " 内で設定して下さい"
'        DATAEDIT_ERRMSG07 = "Ｑレートの再設定"
'        DATAEDIT_ERRMSG08 = "小数点｢.｣が1回以上入力されています"
'        DATAEDIT_ERRMSG09 = "Qレートの理想値は「"
'        DATAEDIT_ERRMSG10 = "」です。"
'        DATAEDIT_ERRMSG11 = "理想値に設定しますか？"

'        INPUTAREA(0) = "数値は "
'        INPUTAREA(1) = " の範囲で入力してください"
'        INPUTAREA(2) = "入力エラー ["
'        INPUTAREA(3) = "]"
'        INPUTAREA(4) = "二重登録エラー ["
'        INPUTAREA(5) = "%1行目とステップ番号が重複しています"
'        INPUTAREA(6) = "ブロック数の合計が "
'        INPUTAREA(7) = " となるように設定してください"
'        INPUTAREA(8) = " XYテーブルの可動範囲を超えています"

'        CHKMSG(0) = "ブロック数のデータはクリアされます"
'        CHKMSG(1) = "位置補正方法のデータはクリアされます"
'        CHKMSG(2) = "補正位置１～回転中心までのデータはクリアされます"
'        CHKMSG(3) = "よろしいですか？"
'        CHKMSG(4) = "確認"
'        CHKMSG(5) = "編集データをクリアすると、新規作成時の状態にもどります"
'        CHKMSG(6) = "実行しますか？"
'        CHKMSG(7) = "削除されたカットデータはクリアされます"
'        CHKMSG(8) = "ＮＧマーキングのデータは消去されます"
'        CHKMSG(9) = "パワー調整モードのデータは消去されます"
'        CHKMSG(10) = "パルス幅時間を　"
'        CHKMSG(11) = "　で設定してください"

'        '-----------------------------------------------------------------------
'        '   タイトルメッセージ
'        '-----------------------------------------------------------------------
'        TITLE_1 = "削除確認"
'        TITLE_2 = "二重登録エラー"
'        TITLE_3 = "入力データ範囲外"
'        TITLE_4 = "中止"
'        TITLE_5 = "ファイルの確認"
'        TITLE_6 = "必須項目エラー"
'        TITLE_7 = "確認"
'        TITLE_8 = "入力項目エラー"
'        TITLE_LASER = "レーザー調整"
'        TITLE_LOGGING = "ロギング処理"

'        '-----------------------------------------------------------------------
'        '   ラベルメッセージ
'        '-----------------------------------------------------------------------
'        ' メイン画面ラベル
'        MSG_MAIN_LABEL01 = "基板枚数="
'        MSG_MAIN_LABEL02 = "NG率="
'        MSG_MAIN_LABEL03 = "IT H NG率="
'        MSG_MAIN_LABEL04 = "IT L NG率="
'        MSG_MAIN_LABEL05 = "FT H NG率="
'        MSG_MAIN_LABEL06 = "FT L NG率="
'        MSG_MAIN_LABEL07 = "OVER NG率="

'        ' 一時停止画面用 ###014
'        LBL_FINEADJ_001 = "データ編集"
'        '----- ###204↓ ----- 
'        LBL_FINEADJ_002 = "調整"
'        LBL_FINEADJ_003 = "０：表示なし"
'        LBL_FINEADJ_004 = "１：ＮＧのみ表示"
'        LBL_FINEADJ_005 = "２：全て表示"
'        LBL_FINEADJ_006 = "基板投入"    ' V4.11.0.0⑥
'        '----- ###204↑ ----- 

'        'teaching
'        LBL_TEACHING_001 = "自動ティーチング"
'        LBL_TEACHING_002 = "矢印スイッチ:BPオフセット, START:次項"
'        LBL_TEACHING_003 = "矢印スイッチ:カットスタート位置移動, START:次項, HALT:前項"
'        'cut pos teaching
'        LBL_CUTPOSTEACH_001 = "カット補正位置ティーチング"
'        LBL_CUTPOSTEACH_002 = "自動ポジション"
'        LBL_CUTPOSTEACH_003 = "START: 次抵抗, HALT: 前抵抗, 矢印スイッチ: BP移動"
'        'LBL_CUTPOSTEACH_004 = "パターン登録(&P)"
'        LBL_CUTPOSTEACH_004 = "パターン登録"

'        'recog
'        LBL_RECOG_001 = "登録"
'        LBL_RECOG_002 = "ｷｬﾝｾﾙ"
'        LBL_RECOG_003 = "ﾊﾟﾀｰﾝﾏｯﾁ試行"
'        LBL_RECOG_004 = "検索範囲設定"
'        LBL_RECOG_005 = "ﾃﾝﾌﾟﾚｰﾄ登録"
'        LBL_RECOG_006 = "登録終了"
'        LBL_RECOG_007 = "ﾃﾝﾌﾟﾚｰﾄ消去"
'        LBL_RECOG_008 = "補正ポジションオフセット"
'        LBL_RECOG_009 = "調整"
'        LBL_RECOG_010 = "XYﾃｰﾌﾞﾙﾎﾟｼﾞｼｮﾝ(ﾄﾘﾑﾎﾟｼﾞｼｮﾝ相対)"
'        LBL_RECOG_011 = "矢印ｽｲｯﾁ:位置合わせ, STARTｽｲｯﾁ:決定, HALT:取り消し"
'        LBL_RECOG_012 = "マウスで青い四角形をドラッグし左上位置を合わせます。右下すみをドラッグして大きさを合わせます。"
'        LBL_RECOG_013 = "マウスで黄色い四角形をドラッグし左上位置を合わせます。右下すみをドラッグして大きさを合わせます。"
'        LBL_RECOG_014 = "設定"
'        LBL_RECOG_015 = "XYテーブルポジション"
'        LBL_RECOG_016 = "グループ番号"
'        LBL_RECOG_017 = "パターン番号"
'        LBL_RECOG_018 = "閾値"
'        LBL_RECOG_019 = "コントラスト"
'        LBL_RECOG_020 = "輝度"

'        MSG_RECOG_001 = "設定が完了しました。"
'        MSG_RECOG_002 = "削除しても良いですか？"
'        'data edit(plate)

'        ' 基板種別対応
'        If gSysPrm.stCTM.giSPECIAL = customASAHI Then
'            LBL_PP_1 = "基板種別"
'        Else
'            LBL_PP_1 = "データ名"
'        End If
'        LBL_PP_2 = "トリムモード"

'        LBL_PP_3 = "ステップ＆リピート"
'        LBL_PP_4 = "プレート数"
'        LBL_PP_5 = "ブロック数"
'        LBL_PP_6 = "プレート間隔(mm)"
'        LBL_PP_8 = "テーブル"
'        LBL_PP_9 = "ビーム"
'        LBL_PP_10 = "アジャスト"
'        LBL_PP_12 = "ＮＧマーキング"
'        LBL_PP_13 = "ディレイトリム"
'        LBL_PP_14 = "ＮＧ判定単位"
'        LBL_PP_15 = "ＮＧ判定基準(%)"
'        LBL_PP_16 = "プローブ Ｚ オフセット(mm)"
'        LBL_PP_17 = "プローブステップ上昇距離(mm)"
'        LBL_PP_18 = "プローブ待機 Ｚ オフセット(mm)"
'        LBL_PP_19 = "抵抗並び方向"
'        LBL_PP_126 = "１ブロック内　サーキット数"
'        LBL_PP_127 = "サーキットサイズ(mm)"
'        LBL_PP_128 = "θ軸（deg）"
'        LBL_PP_20 = "１サーキット内　抵抗数"
'        LBL_PP_21 = "グループ数"
'        LBL_PP_22 = "グループ間隔"
'        LBL_PP_23 = "チップサイズ(mm)"
'        '    LBL_PP_123 = "ステップオフセット量(mm)"
'        LBL_PP_123 = "ステップ(mm)"
'        LBL_PP_24 = "ブロックサイズ(mm)"
'        LBL_PP_25 = "ブロック間隔(mm)"
'        LBL_PP_26 = "連続ＮＧ－ＨＩＧＨ 抵抗ブロック数"
'        LBL_PP_REVISE = "位置補正 "
'        LBL_PP_30 = "位置補正モード"
'        LBL_PP_31 = "位置補正方法"
'        LBL_PP_32 = "補正位置１(mm)"
'        LBL_PP_33 = "補正位置２(mm)"
'        LBL_PP_34 = "補正位置オフセット(mm)"
'        LBL_PP_35 = "表示モード"
'        LBL_PP_36 = "μm／ピクセル"
'        LBL_PP_37 = "パターン番号"
'        LBL_PP_38 = "パターングループ番号"
'        LBL_PP_39 = "回転中心(mm)"
'        '    LBL_PP_40 = "キャリブレーション座標１(mm)"
'        '    LBL_PP_41 = "キャリブレーション座標２(mm)"
'        '    LBL_PP_42 = "キャリブレーション オフセット(mm)"
'        '    LBL_PP_43 = "キャリブレーション パターン番号"
'        '    LBL_PP_44 = "キャリブレーション カット長(mm)"
'        '    LBL_PP_45 = "キャリブレーション カット速度(mm/s)"
'        '    LBL_PP_46 = "キャリブレーション Ｑレート(kHz)"
'        LBL_PP_CARIB = "キャリブレーション "
'        LBL_PP_40 = "座標１(mm)"
'        LBL_PP_41 = "座標２(mm)"
'        LBL_PP_42 = "オフセット(mm)"
'        LBL_PP_43 = "パターン番号"
'        LBL_PP_CARIBGRPNO = "グループ番号"
'        LBL_PP_44 = "カット長(mm)"
'        LBL_PP_45 = "カット速度(mm/s)"
'        LBL_PP_46 = "Ｑレート(kHz)"
'        LBL_PP_47 = "補正オフセット(mm)"
'        LBL_PP_48 = "補正パターン番号"
'        LBL_PP_49 = "補正カット長(mm)"
'        LBL_PP_50 = "補正カット速度(mm/s)"
'        LBL_PP_51 = "補正レーザＱレート(kHz)"
'        LBL_PP_52 = "パターングループ"
'        LBL_PP_53 = "トリミングＮＧカウンタ（上限）"
'        LBL_PP_54 = "割れ欠け排出カウンタ（上限）"
'        LBL_PP_55 = "連続トリミングＮＧ枚数"
'        LBL_PP_115 = "再プロービング回数"
'        LBL_PP_116 = "再プロービング移動量（mm）"
'        '    LBL_PP_117 = "パワー調整モード"
'        LBL_PP_117 = "パワー調整モード(x0,x1,x5)"
'        LBL_PP_118 = "調整目標パワー（W）"
'        LBL_PP_119 = "パワー調整 Ｑレート（kHz）"
'        LBL_PP_120 = "パワー調整許容範囲（W）"
'        LBL_PP_121 = "イニシャルＯＫテスト"
'        LBL_PP_122 = "基板品種"
'        LBL_PP_124 = "4端子オープンチェック"
'        LBL_PP_125 = "ステップ＆リピート移動量（mm）"
'        LBL_PP_126 = "ＬＥＤ制御(自動運転時)"
'        LBL_PP_127 = "θ軸（deg）"

'        LBL_PP_130 = "GP-IB制御"
'        LBL_PP_131 = "初期設定（デリミタ）"
'        LBL_PP_132 = "初期設定（タイムアウト）"
'        LBL_PP_133 = "初期設定（機器アドレス）"
'        LBL_PP_134 = "初期化コマンド"
'        LBL_PP_135 = "トリガコマンド"
'        LBL_PP_136 = "GP-IB制御"
'        LBL_PP_137 = "初期設定（機器アドレス）"
'        LBL_PP_138 = "測定速度"
'        LBL_PP_139 = "測定モード"

'        LBL_PP_140 = "Ｔθオフセット（deg）"
'        LBL_PP_141 = "Ｔθ基準位置１（mm）"
'        LBL_PP_142 = "Ｔθ基準位置２（mm）"

'        'data edit(step)
'        LBL_S_1 = "ｽﾃｯﾌﾟ番号"
'        LBL_S_2 = "ﾌﾞﾛｯｸ数"
'        LBL_S_3 = "ｽﾃｯﾌﾟ間ｲﾝﾀｰﾊﾞﾙ"
'        LBL_S_4 = "ｽﾃｯﾌﾟ" & vbCrLf & "番号"
'        LBL_S_5 = "座標X"
'        LBL_S_6 = "座標Y"
'        LBL_S_7 = "ｽﾃｯﾌﾟ" & vbCrLf & "番号"
'        LBL_S_8 = "ｻｰｷｯﾄ数"
'        LBL_S_9 = "ｸﾞﾙｰﾌﾟ間" & vbCrLf & " ｲﾝﾀｰﾊﾞﾙ" '..2006/04/06..TM(TXTY仕様変更)

'        '(TXTY仕様変更)
'        LBL_G_1 = "ｸﾞﾙｰﾌﾟ番号"
'        LBL_G_2 = "抵抗数"
'        LBL_G_3 = "ｸﾞﾙｰﾌﾟ間ｲﾝﾀｰﾊﾞﾙ"

'        LBL_TY2_1 = "ﾌﾞﾛｯｸ番号"
'        LBL_TY2_2 = "ｽﾃｯﾌﾟ距離"

'        'data edit(registor)
'        MSG_REGISTER_LABEL01 = "抵抗" & vbCrLf & "番号"
'        MSG_REGISTER_LABEL02 = "判定" & vbCrLf & "測定"
'        MSG_REGISTER_LABEL03 = "プローブ番号"
'        MSG_REGISTER_LABEL04 = "EXTERNAL BIT"
'        MSG_REGISTER_LABEL05 = "ポーズ" & vbCrLf & "(ms)"
'        MSG_REGISTER_LABEL06 = "絶" & vbCrLf & "対" & vbCrLf & "値" & vbCrLf & "比"
'        MSG_REGISTER_LABEL07 = "ベース" & vbCrLf & "抵抗番号"
'        MSG_REGISTER_LABEL08 = "トリミング目標値" & vbCrLf & "(ohm)"
'        MSG_REGISTER_LABEL09 = "電圧変化" & vbCrLf & "スロープ"
'        MSG_REGISTER_LABEL10 = "イニシャルテスト(%)"
'        MSG_REGISTER_LABEL11 = "ファイナルテスト(%)"
'        MSG_REGISTER_LABEL12 = "カット数"
'        MSG_REGISTER_LABEL13 = "カット" & vbCrLf & "補正"
'        MSG_REGISTER_LABEL14 = "表示" & vbCrLf & "モード"
'        MSG_REGISTER_LABEL15 = "パターン" & vbCrLf & "番号"
'        MSG_REGISTER_LABEL16 = "カット補正位置"
'        MSG_REGISTER_LABEL17 = "ＮＧ" & vbCrLf & "有/無"
'        MSG_REGISTER_LABEL18 = "ﾘﾐｯﾄ H"
'        MSG_REGISTER_LABEL19 = "ﾘﾐｯﾄ L"
'        MSG_REGISTER_LABEL20 = "イニシャルOKテスト(%)"
'        MSG_REGISTER_LABEL21 = "ΔR" & vbCrLf & "(%)"
'        MSG_REGISTER_LABEL22 = "切上" & vbCrLf & "倍率"

'        ''''2009/07/03 NET
'        MSG_REGISTER_LABEL25 = "所属" & vbCrLf & "ｻｰｷｯﾄ"
'        MSG_REGISTER_LABEL26 = "トリム" & vbCrLf & "モード"
'        MSG_REGISTER_LABEL27 = "ベース" & vbCrLf & "抵抗"
'        'from TKY
'        MSG_REGISTER_LABEL33 = "プローブ確認位置"
'        MSG_REGISTER_LABEL34 = "H-X" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL35 = "H-Y" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL36 = "L-X" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL37 = "L-Y" & vbCrLf & "(mm)"
'        '

'        'data edit(cut)
'        MSG_CUT_LABEL01 = "抵抗" & vbCrLf & "番号"
'        MSG_CUT_LABEL02 = "ｶｯﾄ" & vbCrLf & "番号"
'        MSG_CUT_LABEL03 = "ﾃﾞｨﾚｲ" & vbCrLf & "ﾀｲﾑ" & vbCrLf & "(ms)"
'        MSG_CUT_LABEL04 = "ﾃｨｰﾁﾝｸﾞP" & vbCrLf & "X (mm)"
'        MSG_CUT_LABEL05 = "ﾃｨｰﾁﾝｸﾞP" & vbCrLf & "Y (mm)"
'        MSG_CUT_LABEL06 = "ｽﾀｰﾄP" & vbCrLf & "X (mm)"
'        MSG_CUT_LABEL07 = "ｽﾀｰﾄP" & vbCrLf & "Y (mm)"
'        MSG_CUT_LABEL08 = "ｶｯﾄ" & vbCrLf & "ｽﾋﾟｰﾄﾞ" & vbCrLf & "(mm/s)"
'        MSG_CUT_LABEL09 = "Qﾚｰﾄ" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL10 = "ｶｯﾄｵﾌ" & vbCrLf & "(%)"
'        MSG_CUT_LABEL11 = "ﾊﾟﾙｽ幅" & vbCrLf & "制御"
'        MSG_CUT_LABEL12 = "ﾊﾟﾙｽ幅" & vbCrLf & "時間" & vbCrLf & "(ns)"
'        MSG_CUT_LABEL13 = "LSwﾊﾟﾙｽ幅" & vbCrLf & "時間" & vbCrLf & "(us)"
'        MSG_CUT_LABEL14 = "ﾊﾟﾀｰﾝ" & vbCrLf & "+" & vbCrLf & "方向"
'        MSG_CUT_LABEL15 = "ｶｯﾄ長1" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL16 = "R1" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL17 = "ﾀｰﾝP" & vbCrLf & "(%)"
'        MSG_CUT_LABEL18 = "ｶｯﾄ長2" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL19 = "R2" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL20 = "ｶｯﾄ長3" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL21 = "ｲﾝﾃﾞｯｸｽ" & vbCrLf & "数"
'        MSG_CUT_LABEL22 = "測定" & vbCrLf & "ﾓｰﾄﾞ"
'        MSG_CUT_LABEL23 = "ｶｯﾄ2" & vbCrLf & "ｽﾋﾟｰﾄﾞ"
'        MSG_CUT_LABEL24 = "Qﾚｰﾄ2" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL25 = "斜め" & vbCrLf & "角度"
'        MSG_CUT_LABEL26 = "ﾋﾟｯﾁ"
'        MSG_CUT_LABEL27 = "ス" & vbCrLf & "テ" & vbCrLf & "ッ" & vbCrLf & "プ"
'        MSG_CUT_LABEL28 = "本" & vbCrLf & "数"
'        MSG_CUT_LABEL29 = "ESﾎﾟｲﾝﾄ" & vbCrLf & "(%)"
'        MSG_CUT_LABEL30 = "ES" & vbCrLf & "判定化率" & vbCrLf & "(%)"
'        MSG_CUT_LABEL31 = "ES後" & vbCrLf & "ｶｯﾄ長" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL32 = "倍率" '"Uｶｯﾄ" & vbCrLf & "ﾀﾞﾐｰ1"
'        MSG_CUT_LABEL33 = "文字列" '"Uｶｯﾄ" & vbCrLf & "ﾀﾞﾐｰ2"
'        MSG_CUT_LABEL34 = "Qﾚｰﾄ3" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL35 = "切替え" & vbCrLf & "ﾎﾟｲﾝﾄ(%)"
'        MSG_CUT_LABEL36 = "判定率" & vbCrLf & "(%)"
'        MSG_CUT_LABEL37 = "ES後" & vbCrLf & "判定化率" & vbCrLf & "(%)"
'        MSG_CUT_LABEL38 = "ES後" & vbCrLf & "確認回数"
'        MSG_CUT_LABEL39 = "ﾗﾀﾞｰ間" & vbCrLf & "距離" & vbCrLf & "(μm)"
'        MSG_CUT_LABEL40 = "ｶｯﾄｵﾌ" & vbCrLf & "ｵﾌｾｯﾄ" & vbCrLf & "(%)"

'        'teaching
'        MSG_TEACH_LABEL01 = "ﾋﾞｰﾑﾎﾟｲﾝﾄｵﾌｾｯﾄ"
'        MSG_TEACH_LABEL02 = "ﾄﾘﾐﾝｸﾞｽﾀｰﾄﾎﾟｲﾝﾄ"
'        MSG_TEACH_LABEL03 = "抵抗No."
'        MSG_TEACH_LABEL04 = "ティーチング終了"
'        MSG_TEACH_LABEL05 = "抵抗名称"

'        'plobe position
'        MSG_PROBE_LABEL01 = "手動"
'        MSG_PROBE_LABEL02 = "自動1：Z軸のみ"
'        MSG_PROBE_LABEL03 = "自動2：XY軸"
'        MSG_PROBE_LABEL04 = "測定スタート"
'        MSG_PROBE_LABEL05 = "ティーチング終了"
'        MSG_PROBE_LABEL06 = "プローブ位置合わせ"
'        MSG_PROBE_LABEL14 = "PB TEACH"
'        MSG_PROBE_MSG01 = "自動プローブ位置合わせを実行します"
'        MSG_PROBE_MSG02 = "自動プローブ位置合わせ"
'        MSG_PROBE_MSG03 = "現在位置="
'        MSG_PROBE_MSG04 = "下降距離="
'        MSG_PROBE_MSG05 = "位置合わせモード"
'        MSG_PROBE_MSG06 = "座標"
'        MSG_PROBE_MSG07 = "テーブル移動"
'        MSG_PROBE_MSG08 = "先頭ブロック"
'        MSG_PROBE_MSG09 = "中間ブロック"
'        MSG_PROBE_MSG10 = "最終ブロック"
'        MSG_PROBE_MSG11 = "抵抗値測定"
'        MSG_PROBE_MSG12 = "差電流測定"
'        MSG_PROBE_MSG13 = "連続測定"
'        MSG_PROBE_MSG14 = "全測定"
'        MSG_PROBE_MSG15 = "連続 停止"
'        MSG_PROBE_MSG16 = "抵抗測定エラー"

'        ' frmMsgBox(画面終了確認)
'        MSG_CLOSE_LABEL01 = "画面終了確認"
'        MSG_CLOSE_LABEL02 = "はい(&Y)"
'        MSG_CLOSE_LABEL03 = "いいえ(&N)"
'        ' frmReset(原点復帰画面など)
'        MSG_FRMRESET_LABEL01 = "原点復帰中"
'        ' LASER_teaching
'        MSG_LASER_LABEL01 = "START:レーザー射出"
'        MSG_LASER_LABEL02 = "HALT:レーザー停止"
'        MSG_LASER_LABEL03 = "加工条件番号"
'        MSG_LASER_LABEL04 = "加工条件"
'        MSG_LASER_LABEL05 = "Q SWITCH RATE (KHz)"
'        MSG_LASER_LABEL06 = "STEG本数"
'        MSG_LASER_LABEL07 = "電流値(mA)"
'        MSG_LASER_LABEL08 = "加工条件番号を指定して下さい。"

'        ' ■パワー調整(FL用) ###066
'        MSG_AUTOPOWER_01 = "パワー調整開始"
'        MSG_AUTOPOWER_02 = "加工条件番号"
'        MSG_AUTOPOWER_03 = "レーザパワー設定値"
'        MSG_AUTOPOWER_04 = "電流値"
'        MSG_AUTOPOWER_05 = "パワー調整未完了"
'        MSG_AUTOPOWER_06 = "パワー調整正常終了" 'V1.16.0.0⑧

'        DE_EXPLANATION(0) = "前項　　："
'        DE_EXPLANATION(1) = "次項　　："
'        DE_EXPLANATION(2) = "前ページ："
'        DE_EXPLANATION(3) = "次ページ："
'        DE_EXPLANATION(4) = "リピート："
'        DE_EXPLANATION(5) = "[↑]キー"
'        DE_EXPLANATION(6) = "[↓]キー"
'        DE_EXPLANATION(7) = "[←]キー"
'        DE_EXPLANATION(8) = "[→]キー"
'        DE_EXPLANATION(9) = "[Enter]キー"
'        DE_EXPLANATION(10) = "取消し　："
'        DE_EXPLANATION(11) = "[Esc]キー"

'        MSG_ONOFF(0) = "OFF"
'        MSG_ONOFF(1) = "ON"

'        '-----------------------------------------------------------------------
'        '   ボタン表示
'        '-----------------------------------------------------------------------
'        LBL_CUT_COPYLINE = "行コピー(&L)"
'        LBL_CUT_COPYCOLUMN = "列コピー(&C)"

'        LBL_CMD_OK = "OK (&O)"
'        LBL_CMD_CANCEL = "キャンセル (&Q)"
'        LBL_CMD_CLEAR = "クリア(&K)"
'        LBL_CMD_LCOPY = "列コピー(&L)"

'        '-----------------------------------------------------------------------
'        '   スプラッシュメッセージ
'        '-----------------------------------------------------------------------
'        MSG_SPRASH0 = "原点復帰中"
'        MSG_SPRASH1 = "XYトリム位置リセット中"
'        MSG_SPRASH2 = "トリム位置移動中"
'        MSG_SPRASH3 = "STARTキーを押してください"
'        MSG_SPRASH4 = "STARTキーを押してください"
'        MSG_SPRASH5 = "インターロックが解除されています"
'        MSG_SPRASH6 = "非常停止しました"
'        MSG_SPRASH7 = "確認して非常停止解除ＳＷを押してください"
'        MSG_SPRASH8 = "初期化処理中"
'        MSG_SPRASH9 = "スライドカバーを閉じてください"
'        MSG_SPRASH10 = "筐体カバーを閉じてください"
'        MSG_SPRASH11 = "ローダーを手動モードにしてください"
'        MSG_SPRASH12 = "エアー圧低下検出"
'        MSG_SPRASH13 = "連続NG-HIエラー"
'        MSG_SPRASH14 = "再プロービング失敗"
'        MSG_SPRASH15 = "STARTキーを押すとプログラムを終了します"
'        MSG_SPRASH16 = "RESETキーを押すとプログラムを終了します"
'        MSG_SPRASH17 = "Cancelボタン押下でプログラムを終了します"
'        MSG_SPRASH18 = "ローダ通信タイムアウトエラー"
'        MSG_SPRASH19 = "全停止異常発生中"
'        MSG_SPRASH20 = "サイクル停止異常発生中"
'        MSG_SPRASH21 = "軽故障発生中"
'        MSG_SPRASH22 = "STARTキーを押すと自動運転を開始します"
'        MSG_SPRASH23 = "トリミング実行中にエラーが発生しました。"
'        MSG_SPRASH24 = "ローダ原点復帰中"
'        MSG_SPRASH25 = "インターロック全解除中"
'        MSG_SPRASH26 = "インターロック一部解除中"
'        MSG_SPRASH27 = "筐体カバーが開きました"
'        MSG_SPRASH28 = "スライドカバーが開きました"
'        MSG_SPRASH29 = "カバーを閉じるか、インターロック解除スイッチをＯＮしてください"
'        MSG_SPRASH30 = "PLCステータス異常"
'        MSG_SPRASH31 = "注意！！！"
'        MSG_SPRASH32 = "スライドカバーが自動で閉じます"
'        MSG_SPRASH33 = "RESETキーを押すと処理を終了します"
'        MSG_SPRASH34 = "筐体カバーまたはスライドカバーが開きました"
'        MSG_SPRASH35 = "STARTキー：処理続行，RESETキー：処理終了"       ' ###073
'        MSG_SPRASH36 = "筐体カバーを閉じて"                             ' ###088
'        MSG_SPRASH37 = "ローダ原点復帰未完了"                           ' ###137
'        MSG_SPRASH38 = "ステージ原点移動中"                             ' ###188
'        MSG_SPRASH39 = "一時停止中です"                                 ' V1.13.0.0③
'        MSG_SPRASH40 = "測定のばらつきを検出しました"                   ' V1.13.0.0⑪
'        MSG_SPRASH41 = "測定時オーバロードを検出しました"               ' V1.13.0.0⑪
'        MSG_SPRASH42 = "再プロービングエラー"                           ' V1.13.0.0⑪
'        MSG_SPRASH43 = "前面扉ロックタイムアウト"                       ' V1.18.0.1⑧
'        MSG_SPRASH44 = "前面扉ロック解除タイムアウト"                   ' V1.18.0.1⑧
'        MSG_SPRASH45 = "装置内の基板を取り除いてください"               ' V4.0.0.0-71
'        MSG_SPRASH46 = "装置原点復帰中"                                 ' V4.0.0.0-83
'        MSG_SPRASH47 = "Home位置移動中"                                 ' V4.0.0.0-83
'        MSG_SPRASH48 = "ＱＲコードに対応したトリミングデータがありません"          ' V4.1.0.0①
'        MSG_SPRASH49 = "ファイルを確認してください"                                ' V4.1.0.0①
'        MSG_SPRASH50 = "PLCローバッテリーアラーム"                      ' V4.1.0.0⑦
'        MSG_SPRASH51 = "電池を交換してください"                         ' V4.1.0.0⑦

'        '----- limit.frm用 -----
'        MSG_frmLimit_01 = "BPリミット"
'        MSG_frmLimit_02 = "カットスタートポイントがBPの稼動領域を超えました。 BPオフセットを修正してください"
'        MSG_frmLimit_03 = "X軸リミット"
'        MSG_frmLimit_04 = "Y軸リミット"
'        MSG_frmLimit_05 = "Z軸リミット"
'        MSG_frmLimit_06 = "Z2軸リミット"
'        MSG_frmLimit_07 = "STARTキーを押すか、OKボタンを押して下さい。"

'        '-----------------------------------------------------------------------
'        '   INtime側エラーメッセージ 
'        '-----------------------------------------------------------------------
'        MSG_SRV_ALM = "サーボアラーム"
'        MSG_AXIS_X_SERVO_ALM = "X軸サーボアラーム"
'        MSG_AXIS_Y_SERVO_ALM = "Y軸サーボアラーム"
'        MSG_AXIS_Z_SERVO_ALM = "Z軸サーボアラーム"
'        MSG_AXIS_T_SERVO_ALM = "θ軸サーボアラーム"

'        ' 軸系エラー(タイムアウト)
'        MSG_TIMEOUT_AXIS_X = "X軸タイムアウトエラー"
'        MSG_TIMEOUT_AXIS_Y = "Y軸タイムアウトエラー"
'        MSG_TIMEOUT_AXIS_Z = "Z軸タイムアウトエラー"
'        MSG_TIMEOUT_AXIS_T = "θ軸タイムアウトエラー"
'        MSG_TIMEOUT_AXIS_Z2 = "Z2軸タイムアウトエラー"
'        MSG_TIMEOUT_ATT = "ロータリアッテネータタイムアウトエラー"
'        MSG_TIMEOUT_AXIS_XY = "XY軸タイムアウトエラー"

'        ' 軸系エラー(プラスリミットオーバー)
'        MSG_STG_SOFTLMT_PLUS_AXIS_X = "X軸プラスリミットオーバー"
'        MSG_STG_SOFTLMT_PLUS_AXIS_Y = "Y軸プラスリミットオーバー"
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z = "Z軸プラスリミットオーバー"
'        MSG_STG_SOFTLMT_PLUS_AXIS_T = "θ軸プラスリミットオーバー"
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z2 = "Z2軸プラスリミットオーバー"
'        MSG_STG_SOFTLMT_PLUS = "軸プラスリミットオーバー"

'        ' 軸系エラー(マイナスリミットオーバー)
'        MSG_STG_SOFTLMT_MINUS_AXIS_X = "X軸マイナスリミットオーバー"
'        MSG_STG_SOFTLMT_MINUS_AXIS_Y = "Y軸マイナスリミットオーバー"
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z = "Z軸マイナスリミットオーバー"
'        MSG_STG_SOFTLMT_MINUS_AXIS_T = "θ軸マイナスリミットオーバー"
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z2 = "Z2軸マイナスリミットオーバー"
'        MSG_STG_SOFTLMT_MINUS = "軸マイナスリミットオーバー"

'        ' 軸系エラー(リミット検出)
'        MSG_AXS_LIM_X = "X軸リミット検出"
'        MSG_AXS_LIM_Y = "Y軸リミット検出"
'        MSG_AXS_LIM_Z = "Z軸リミット検出"
'        MSG_AXS_LIM_T = "θ軸リミット検出"
'        MSG_AXS_LIM_Z2 = "Z2軸リミット検出"
'        MSG_AXS_LIM_ATT = "ATT軸リミット検出"

'        ' BP系エラー
'        MSG_BP_MOVE_TIMEOUT = "BP タイムアウト"
'        MSG_BP_GRV_ALARM_X = "ガルバノアラームX"
'        MSG_BP_GRV_ALARM_Y = "ガルバノアラームY"
'        MSG_BP_HARD_LIMITOVER_LO = "BPリミットオーバー(最小可動範囲以下)"
'        MSG_BP_HARD_LIMITOVER_HI = "BPリミットオーバー(最大可動範囲以上)"
'        MSG_BP_LIMITOVER = "BP移動距離設定リミットオーバー"
'        MSG_BP_SOFT_LIMITOVER = "BPソフト可動範囲オーバー"
'        MSG_BP_BSIZE_OVER = "ブロックサイズ設定オーバー(ソフト可動範囲外)"

'        ' カバー開検出
'        MSG_OPN_CVR = "筐体カバー開検出"
'        MSG_OPN_SCVR = "スライドカバー開検出"
'        MSG_OPN_CVRLTC = "カバー開ラッチ検出"


'        MSG_INTIME_ERROR = "INTRIMエラー"

'        '-----------------------------------------------------------------------
'        '   ステータスバーメッセージ
'        '-----------------------------------------------------------------------
'        'data edit(plate 01)
'        '基板種別対応
'        If gSysPrm.stCTM.giSPECIAL = customASAHI Then
'            CONST_PP(glNo_DataNo) = "基板種別を指定（1:50x60/2:78x78）"
'        Else
'            CONST_PP(glNo_DataNo) = "データ名を指定"
'        End If

'        '    CONST_PP(glMeasType) = "トリムモードを指定（0:抵抗/1:DC電圧）"
'        '    CONST_PP(glNo_PlateCntXDir) = "X方向のプレート数を指定"
'        '    CONST_PP(glNo_PlateCntYDir) = "Y方向のプレート数を指定"
'        '    CONST_PP(glPlateInvXDir) = "X方向のプレート間隔をmm単位で指定"
'        '    CONST_PP(glPlateInvYDir) = "Y方向のプレート間隔をmm単位で指定"
'        CONST_PP(glNo_DirStepRepeat) = "テーブルのステップ方向を指定（0:無し/1:X方向/2:Y方向/3:チップ幅+X）" '"テーブルのステップ方向を指定（0:無し/1:X方向/2:Y方向）"
'        CONST_PP(glNo_MeasType) = "測定種別を指定（0:抵抗、1:DC電圧)"
'        CONST_PP(glNo_BlockCntXDir) = "X方向のブロック数を指定"
'        CONST_PP(glNo_BlockCntYDir) = "Y方向のブロック数を指定"
'        CONST_PP(glNo_TableOffSetXDir) = "X方向のテーブルオフセットをmm単位で指定"
'        CONST_PP(glNo_TableOffSetYDir) = "Y方向のテーブルオフセットをmm単位で指定"
'        CONST_PP(glNo_BpOffsetXDir) = "X方向のビームポジションオフセットをmm単位で指定"
'        CONST_PP(glNo_BpOffsetYDir) = "Y方向のビームポジションオフセットをmm単位で指定"
'        CONST_PP(glNo_AdjOffsetXDir) = "X方向のアジャストポイントをmm単位で指定"
'        CONST_PP(glNo_AdjOffsetYDir) = "Y方向のアジャストポイントをmm単位で指定"
'        CONST_PP(glNo_NGMark) = "NGマーキングの実行有無を指定（0:実行しない/1:実行する）"

'        If gSysPrm.stSPF.giDelayTrim2 = 1 Then
'            CONST_PP(glNo_DelayTrim) = "ディレイトリムの実行有無を指定（0:実行しない/1:実行する/2:ﾃﾞｨﾚｲﾄﾘﾑ2を実行する）"
'        Else
'            CONST_PP(glNo_DelayTrim) = "ディレイトリムの実行有無を指定（0:実行しない/1:実行する）"
'        End If

'        CONST_PP(glNo_NgJudgeUnit) = "NG判定の単位を指定（0:ブロック単位/1:プレート単位）"
'        CONST_PP(glNo_NgJudgeLevel) = "NG判定の判定基準を%単位で指定"
'        CONST_PP(glNo_ZOffSet) = "プローブZ軸オフセットをmm単位で指定"
'        CONST_PP(glNo_ZStepUpDist) = "プローブステップ時の上昇距離をmm単位で指定"
'        CONST_PP(glNo_ZWaitOffset) = "プローブZ軸の待機位置をmm単位で指定"
'        'data edit(plate 03)

'        '''''2009/07/03 NETのみ
'        CONST_PP(glNo_LedCtrl) = "１グループ内　サーキット数を指定"
'        '    CONST_PP(glPP127X) = "サーキットサイズＸを指定"
'        '    CONST_PP(glPP127Y) = "サーキットサイズＹを指定"

'        CONST_PP(glNo_ResistDir) = "抵抗並び方向を指定(0:X，1:Y)"
'        If (gTkyKnd = KND_CHIP) Then
'            CONST_PP(glNo_ResistCntInGroup) = "１グループ内抵抗数を指定"
'        ElseIf (gTkyKnd = KND_NET) Then
'            CONST_PP(glNo_ResistCntInGroup) = "１ブロック内サーキットを指定"
'        End If
'        CONST_PP(glNo_GroupCntInBlockXBp) = "ブロック内グループ数Ｘを指定"
'        CONST_PP(glNo_GroupCntInBlockYStage) = "ブロック内グループ数Ｙを指定"
'        CONST_PP(glNo_GroupItvXDir) = "グループ間隔Ｘを㎜単位で指定"
'        CONST_PP(glNo_GroupItvYDir) = "グループ間隔Ｙを㎜単位で指定"
'        CONST_PP(glNo_ChipSizeXDir) = "チップサイズＸを㎜単位で指定"
'        CONST_PP(glNo_ChipSizeYDir) = "チップサイズＹを㎜単位で指定"
'        CONST_PP(glNo_StepOffsetXDir) = "X方向のステップオフセット量をmm単位で指定"
'        CONST_PP(glNo_StepOffsetYDir) = "Y方向のステップオフセット量をmm単位で指定"
'        CONST_PP(glNo_BlockSizeReviseXDir) = "ブロックサイズ補正Ｘを㎜単位で指定"
'        CONST_PP(glNo_BlockSizeReviseYDir) = "ブロックサイズ補正Ｙを㎜単位で指定"
'        CONST_PP(glNo_BlockItvXDir) = "ブロック間隔Ｘを㎜単位で指定"
'        CONST_PP(glNo_BlockItvYDir) = "ブロック間隔Ｙを㎜単位で指定"
'        CONST_PP(glNo_ContHiNgBlockCnt) = "連続ＮＧ－ＨＩＧＨ抵抗ブロック数を指定"
'        'data edit(plate 02)
'        CONST_PP(glNo_ReviseMode) = "補正モードを指定(0:自動,1:手動,2:自動+微調)"
'        CONST_PP(glNo_ManualReviseType) = "手動モードの補正方法を指定(0:補正なし, 1:1回のみ, 2:毎回)"
'        CONST_PP(glNo_ReviseCordnt1XDir) = "位置補正位置１のX座標値をmm単位で指定"
'        CONST_PP(glNo_ReviseCordnt1YDir) = "位置補正位置１のY座標値をmm単位で指定"
'        CONST_PP(glNo_ReviseCordnt2XDir) = "位置補正位置２のX座標値をmm単位で指定"
'        CONST_PP(glNo_ReviseCordnt2YDir) = "位置補正位置２のY座標値をmm単位で指定"
'        CONST_PP(glNo_ReviseOffsetXDir) = "補正ポジションオフセットのX座標値をmm単位で指定"
'        CONST_PP(glNo_ReviseOffsetYDir) = "補正ポジションオフセットのY座標値をmm単位で指定"
'        CONST_PP(glNo_RecogDispMode) = "認識データ表示モードを指定(0:なし,1:CRT,2:CRT&ﾌﾟﾘﾝﾀ)"
'        CONST_PP(glNo_PixelValXDir) = "X方向のピクセル値をμmm単位で指定"
'        CONST_PP(glNo_PixelValYDir) = "Y方向のピクセル値をμmm単位で指定"
'        CONST_PP(glNo_RevisePtnNo1) = "補正位置１のパターン番号を指定"
'        CONST_PP(glNo_RevisePtnNo2) = "補正位置２のパターン番号を指定"
'        CONST_PP(glNo_RevisePtnGrpNo1) = "補正位置１のパターングループ番号を指定"
'        CONST_PP(glNo_RevisePtnGrpNo2) = "補正位置２のパターングループ番号を指定"
'        CONST_PP(glNo_RotateXDir) = "Ｘ方向の回転中心を指定"
'        CONST_PP(glNo_RotateYDir) = "Ｙ方向の回転中心を指定"
'        'data edit(plate 04)
'        CONST_PP(glNo_CaribBaseCordnt1XDir) = "Ｘ方向のキャリブレーション基準座標１を㎜単位で指定"
'        CONST_PP(glNo_CaribBaseCordnt1YDir) = "Ｙ方向のキャリブレーション基準座標１を㎜単位で指定"
'        CONST_PP(glNo_CaribBaseCordnt2XDir) = "X方向のキャリブレーション基準座標２を㎜単位で指定"
'        CONST_PP(glNo_CaribBaseCordnt2YDir) = "Y方向のキャリブレーション基準座標２を㎜単位で指定"
'        CONST_PP(glNo_CaribTableOffsetXDir) = "Ｘ方向のキャリブレーションテーブルオフセットを㎜単位で指定"
'        CONST_PP(glNo_CaribTableOffsetYDir) = "Ｙ方向のキャリブレーションテーブルオフセットを㎜単位で指定"
'        CONST_PP(glNo_CaribPtnNo1) = "キャリブレーションパターン１登録Ｎｏを指定"
'        CONST_PP(glNo_CaribPtnNo2) = "キャリブレーションパターン２登録Ｎｏを指定"
'        CONST_PP(glNo_CaribPtnNo1GroupNo) = "キャリブレーションパターン１登録グループＮｏを指定"
'        CONST_PP(glNo_CaribPtnNo2GroupNo) = "キャリブレーションパターン２登録グループＮｏを指定"
'        CONST_PP(glNo_CaribCutLength) = "キャリブレーションカット長を㎜単位で指定"
'        CONST_PP(glNo_CaribCutSpeed) = "キャリブレーションカット速度を㎜/s単位で指定"
'        CONST_PP(glNo_CaribCutQRate) = "キャリブレーションレーザＱレートをkHz単位で指定"
'        CONST_PP(glNo_CutPosiReviseOffsetXDir) = "Ｘ方向のカット点補正テーブルオフセットを㎜単位で指定"
'        CONST_PP(glNo_CutPosiReviseOffsetYDir) = "Ｙ方向のカット点補正テーブルオフセットを㎜単位で指定"
'        CONST_PP(glNo_CutPosiRevisePtnNo) = "カット点補正パターン登録Ｎｏを指定"
'        CONST_PP(glNo_CutPosiReviseCutLength) = "カット点補正カット長を㎜単位で指定"
'        CONST_PP(glNo_CutPosiReviseCutSpeed) = "カット点補正レーザＱレートをkHz単位で指定"
'        CONST_PP(glNo_CutPosiReviseCutQRate) = "カット点補正カット速度を㎜/s単位で指定"
'        CONST_PP(glNo_CutPosiReviseGroupNo) = "パターングループ番号を指定"
'        'data edit(plate 05)
'        '===　432Kパラメータの為一旦コメント
'        '    CONST_PP(glNo_MaxTrimNgCount) = "トリミングＮＧ回数を指定"
'        '    CONST_PP(glNo_MaxBreakDischargeCount) = "割れ欠け排出回数を指定"
'        '    CONST_PP(glNo_TrimNgCount) = "連続トリミングＮＧ枚数を指定"
'        '    CONST_PP(glNo_InitialOkTestDo) = "イニシャルＯＫテストを指定(0:無し, 1:有り)"
'        '    CONST_PP(glNo_WorkSetByLoader) = "ローダ側で指定した品種を指定"
'        '===

'        CONST_PP(glNo_RetryProbeCount) = "再プロービング回数を指定"
'        CONST_PP(glNo_RetryProbeDistance) = "再プロービング移動量をmm単位で指定"
'        CONST_PP(glNo_PowerAdjustMode) = "パワー調整モードを指定(0:無し, 1:有り)"
'        CONST_PP(glNo_PowerAdjustTarget) = "調整目標パワーをW単位で指定"
'        CONST_PP(glNo_PowerAdjustQRate) = "パワー調整時のレーザＱレートをkHz単位で指定"
'        CONST_PP(glNo_PowerAdjustToleLevel) = "パワー調整許容範囲をW単位で指定"
'        CONST_PP(glNo_OpenCheck) = "4端子オープンチェックを指定(0:無し, 1:有り)"
'        CONST_PP(glNo_DistStepRepeat) = "ステップ＆リピートの移動量をmm単位で指定"
'        CONST_PP(glNo_LedCtrl) = "LEDの制御方法を指定(0:常時ON, 1:常時OFF, 2:画像認識時のみ)"
'        CONST_PP(glNo_RotateTheta) = "θ軸角度を指定"
'        '    CONST_PP(glPP128) = "θ軸角度を指定"
'        CONST_PP(glNo_GpibMode) = "GP-IB制御を指定(0:無し, 1:有り)"
'        CONST_PP(glNo_GpibDelim) = "デリミタのコードを指定(0:CR+LF, 1:CR, 2:LF, 3:無し)"
'        CONST_PP(glNo_GpibTimeOut) = "タイムアウトチェックのリミットを100ms単位で指定"
'        CONST_PP(glNo_GpibAddress) = "機器アドレスを指定"
'        CONST_PP(glNo_GpibInitCmnd) = "初期化コマンドを指定"
'        CONST_PP(glNo_GpibInit2Cmnd) = "初期化コマンドを指定"
'        CONST_PP(glNo_GpibTrigCmnd) = "トリガコマンドを指定"
'        CONST_PP(glNo_Gpib2Mode) = "GP-IB制御を指定(0:無し, 1:有り)"
'        CONST_PP(glNo_Gpib2Address) = "機器アドレスを指定"
'        CONST_PP(glNo_Gpib2MeasSpeed) = "測定速度を指定(0:低速, 1:高速)"
'        CONST_PP(glNo_Gpib2MeasMode) = "測定モードを指定(0:絶対, 1:偏差)"
'        CONST_PP(glNo_TThetaOffset) = "Ｔθ軸角度を指定"
'        CONST_PP(glNo_TThetaBase1XDir) = "Ｔθ基準座標１のX座標値を㎜単位で指定"
'        CONST_PP(glNo_TThetaBase1YDir) = "Ｔθ基準座標１のY座標値を㎜単位で指定"
'        CONST_PP(glNo_TThetaBase2XDir) = "Ｔθ基準座標２のX座標値を㎜単位で指定"
'        CONST_PP(glNo_TThetaBase2YDir) = "Ｔθ基準座標２のY座標値を㎜単位で指定"

'        'data edit(step)
'        CONST_SP(0) = "ステップ番号を指定"
'        CONST_SP(1) = "各ステップに含まれるブロック数を指定"
'        CONST_SP(2) = "各ステップ間のインターバルをmm単位で指定"
'        CONST_CA(0) = "ステップ番号を指定"
'        CONST_CA(1) = "サーキット座標Ｘをmm単位で指定"
'        CONST_CA(2) = "サーキット座標Ｙをmm単位で指定"
'        CONST_CI(0) = "ステップ番号を指定"
'        CONST_CI(1) = "各ステップに含まれるサーキット数を指定"
'        CONST_CI(2) = "各サーキット間のインターバルをmm単位で指定"

'        'data edit(group)
'        CONST_GP(0) = "グループ番号を指定"
'        CONST_GP(1) = "各グループに含まれる抵抗数を指定"
'        CONST_GP(2) = "各グループ間のインターバルをmm単位で指定"
'        CONST_TY2(0) = "ブロック番号を指定"
'        CONST_TY2(1) = "Xの移動距離をmm単位で指定"
'        CONST_TY2(2) = "Yの移動距離をmm単位で指定"

'        'data edit(resistor)
'        CONST_PR1 = "抵抗番号を指定"
'        CONST_PR2 = "測定、判定モードを指定（0:高速/1:高精度）"
'        CONST_PR3 = "抵抗が属するサーキット番号を指定"
'        CONST_PR4H = "ハイ側のプローブ番号を指定"
'        CONST_PR4L = "ロー側のプローブ番号を指定"
'        CONST_PR4G = "第 %1 アクティブガード番号を指定"
'        CONST_PR5 = "EXTERNALビット（16ビット)を指定（16進=1111111111111111）"
'        CONST_PR6 = "EXTERNALビット出力後のﾎﾟｰｽﾞﾀｲﾑをmsec単位で指定"
'        MSG_19 = "EXTERNAL BITを16ビット指定してください"
'        MSG_20 = "EXTERNAL BITSは1/0で入力してください"
'        ''''2009/07/03 NETのみ
'        CONST_PR7 = "トリムモードを指定（0:絶対値/1:レシオ）"
'        CONST_PR8 = "レシオトリミングのベース抵抗番号を指定"
'        CONST_PR9 = "トリミングの目標値を指定"
'        CONST_PR9_1 = "トリミングの倍率を指定"
'        CONST_PR10 = "電圧変化スロープを指定（0:＋スロープ/1:－スロープ）"
'        CONST_PR11H = "イニシャルテストのハイリミットを%単位で指定"
'        CONST_PR11L = "イニシャルテストのローリミットを%単位で指定"
'        CONST_PR12H = "ファイナルテストのハイリミットを%単位で指定"
'        CONST_PR12L = "ファイナルテストのローリミットを%単位で指定"
'        CONST_PR13 = "１抵抗内のカット数を指定"
'        CONST_PR14H = "イニシャルOKテストのハイリミットを%単位で指定"
'        CONST_PR14L = "イニシャルOKテストのローリミットを%単位で指定"
'        CONST_PR10 = "ΔRを%単位で指定"
'        CONST_PR15 = "切り上げ倍率を指定"
'        'data edit(cut)
'        CONST_CP2 = "ディレイトリムをmsec単位で指定"
'        CONST_CP99X = "X方向のティーチングポイントをmm単位で指定"
'        CONST_CP99Y = "Y方向のティーチングポイントをmm単位で指定"
'        CONST_CP4X = "X方向のスタートポイントをmm単位で指定"
'        CONST_CP4Y = "Y方向のスタートポイントをmm単位で指定"
'        CONST_CP5 = "カットスピードをmm/s単位で指定"
'        CONST_CP5_2 = "2回目のカットスピードをmm/s単位で指定"
'        CONST_CP6 = "レーザのＱスイッチレートをkHz単位で指定"
'        CONST_CP6_2 = "2回目のレーザのＱスイッチレートをkHz単位で指定"
'        CONST_CP7 = "カットオフ値を%単位で指定"
'        CONST_CP7_1 = "カットデータ判定（平均化率）基準を%単位で指定"
'        CONST_CP7_2 = "カットオフオフセット値を%単位で指定"
'        CONST_CP50 = "パルス幅制御の実行有無を指定（0:実行しない/1:実行する）"
'        CONST_CP51 = "パルス幅時間をnsec単位で指定"
'        CONST_CP52 = "LSwパルス幅時間をusec単位で指定"
'        CONST_CP9 = "最大カッティング長をmm単位で指定"
'        CONST_CP11 = "Ｌターンポイントを%単位で指定"
'        CONST_CP12 = "Ｌターンポイントまでの最大カッティング長をmm単位で指定"
'        CONST_CP13 = "斜めカットの角度を1度単位で指定"
'        CONST_CP14 = "Ｌターン後の最大カッティング長をmm単位で指定"
'        CONST_CP15 = "フックターン後のカッティング長をmm単位で指定"
'        CONST_CP17 = "インデックス長をmm単位で指定"
'        CONST_CP18 = "インデックス数を指定"
'        CONST_CP19 = "インデックスカット前の測定モード数を指定（0:高速/1:高精度）"
'        CONST_CP30 = "カットパターンを指定"
'        CONST_CP31 = "カット方向を指定"
'        CONST_CP20 = "スキャンカットのピッチを指定"
'        CONST_CP21 = "スキャンカットのステップ方向を指定"
'        CONST_CP22 = "スキャンカットの本数を指定"
'        CONST_CP23 = "文字マーキングの倍率を指定"
'        CONST_CP24 = "文字マーキングの文字列を指定"
'        CONST_CP38 = "エッジセンスポイントを指定"
'        CONST_CP39 = "エッジセンスの判定変化率を指定"
'        CONST_CP40 = "エッジセンス後のカット長を指定"
'        CONST_CPR1 = "Ｌターンポイントからの円弧長をmm単位で指定"
'        CONST_CPR2 = "フックターン前の円弧長をmm単位で指定"
'        CONST_CP55 = "エッジセンス後の判定変化率を指定"
'        CONST_CP56 = "エッジセンス後の確認回数を指定"
'        CONST_CP57 = "ラダー間の距離をμmm単位で指定"
'        CONST_CP53 = "レーザのＱスイッチレートをkHz単位で指定"
'        CONST_CP54 = "切替えポイントを%単位で指定"

'        'data edit
'        STS_21 = "1:+X, 2:-Y, 3:-X, 4:+Y"
'        STS_22 = "1:+X+Y, 2:-Y+X, 3:-X-Y, 4:+Y-X, 5:+X-Y, 6:-Y-X, 7:-X+Y, 8:+Y+X"
'        STS_23 = "1:CW, 2:CCW"
'        STS_24 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, K:ES, M:MARKING" 'ESﾊﾞｰｼﾞｮﾝ
'        STS_26 = "数値は 1:+X, 2:-X, 3:+Y, 4:-Yで入力してください"
'        STS_69 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, S:ES2, M:MARKING" 'ES2ﾊﾞｰｼﾞｮﾝ

'        'GP-IB制御無し？
'        If gSysPrm.stCTM.giGP_IB_flg = 0 Then

'        Else
'            STS_24 = STS_24 & ",T:ST2, X:IX2"
'            STS_69 = STS_69 & ",T:ST2,, X:IX2"
'        End If
'        STS_27 = "全角20文字(半角40文字)以内で入力してください"
'        STS_28 = "半角英数0～20文字以内で入力して下さい"
'        STS_29 = "半角英数0～10文字以内で入力して下さい"

'        '-----------------------------------------------------------------------
'        '   メッセージ
'        '-----------------------------------------------------------------------
'        ' プローブ位置合わせメッセージ
'        MSG_PRB_XYMODE = "ＸＹテーブル移動モード"
'        MSG_PRB_BPMODE = "ポジショナ移動モード"
'        MSG_PRB_ZTMODE = "Ｚティーチングモード"
'        MSG_PRB_THETA = "θ調整モード"
'        ' システムエラーメッセージ
'        MSG_ERR_ZNOTORG = "Ｚ軸原点センサーがＯＮになっていません"
'        ' レーザー調整画面説明文
'        MSG_LASER_LASERON = "START:レーザー射出"
'        MSG_LASER_LASEROFF = "HALT:レーザー停止"
'        MSG_LASEROFF = "レーザー停止中"
'        MSG_LASERON = "レーザー射出中"
'        MSG_ATTRATE = "減衰率" ' ###108
'        MSG_ERRQRATE = "Ｑレート設定値を確認してください"
'        MSG_LOGERROR = "ログファイルの書込みでエラーが発生しました"
'        ' 画像登録画面説明文
'        MSG_PATERN_MSG01 = "設定が完了しました。"


'        ' 操作ログメッセージ
'        MSG_OPLOG_WAKEUP = "トリマ装置起動"
'        MSG_OPLOG_FUNC01 = "データロード"
'        MSG_OPLOG_FUNC02 = "データセーブ"
'        MSG_OPLOG_FUNC03 = "データ編集"
'        MSG_OPLOG_FUNC04 = "システムパラメータ編集"
'        MSG_OPLOG_FUNC05 = "レーザ調整"
'        MSG_OPLOG_FUNC06 = "ロギング"
'        MSG_OPLOG_FUNC07 = "プローブ位置合わせ"
'        MSG_OPLOG_FUNC08 = "ティーチング"
'        MSG_OPLOG_FUNC20 = "カット位置補正用パターン登録"
'        MSG_OPLOG_FUNC09 = "画像登録"
'        MSG_OPLOG_FUNC10 = "トリマ装置停止"
'        MSG_OPLOG_FUNC30 = "サーキットティーチング"
'        MSG_OPLOG_AUTO = "自動運転"
'        MSG_OPLOG_LOADERINIT = "ローダ原点復帰"
'        '----- V1.13.0.0③↓ -----
'        MSG_OPLOG_APROBEREC = "オートプローブ登録"
'        MSG_OPLOG_APROBEEXE = "オートプローブ実行"
'        MSG_OPLOG_IDTEACH = "ＩＤティーチング"
'        MSG_OPLOG_SINSYUKU = "伸縮補正(画像登録)"
'        MSG_OPLOG_MAP = "マップ選択"
'        '----- V1.13.0.0③↑ -----
'        MSG_OPLOG_CLRTOTAL = "集計クリア"
'        MSG_OPLOG_TRIMST = "トリミング"
'        MSG_OPLOG_TRIMRES = "トリミング中RESET SW押下により停止"
'        MSG_OPLOG_HCMD_ERRRST = "HOST CMD: ERR RST"
'        MSG_OPLOG_HCMD_PATCMD = "HOST CMD: PATTERN CMD"
'        MSG_OPLOG_HCMD_LASCMD = "HOST CMD: LASER CMD"
'        MSG_OPLOG_HCMD_MARKCMD = "HOST CMD: MARKING CMD"
'        MSG_OPLOG_HCMD_LODCMD = "HOST CMD: LOAD CMD"
'        MSG_OPLOG_HCMD_TECCMD = "HOST CMD: TEACH CMD"
'        MSG_OPLOG_HCMD_TRMCMD = "HOST CMD: TRM CMD"
'        MSG_OPLOG_HCMD_LSTCMD = "HOST CMD: LOGSTART CMD"
'        MSG_OPLOG_HCMD_LENCMD = "HOST CMD: LOGSTOP CMD"
'        MSG_OPLOG_HCMD_MDAUTO = "HOST CMD: AUTO MODE CHG"
'        MSG_OPLOG_HCMD_MDMANU = "HOST CMD: MANUAL MODE CHG"
'        MSG_OPLOG_HCMD_CPCMD = "HOST CMD: CP TEACH CMD"
'        MSG_POPUP_MESSAGE = "POPUP MESSAGE"
'        MSG_OPLOG_FUNC08S = "カット補正位置ティーチング"

'        '----- V1.18.0.1①↓ -----
'        MSG_F_EXR1 = "外部カメラティーチング(１抵抗)"
'        MSG_F_EXTEACH = "外部カメラティーチング(全抵抗)"
'        MSG_F_CARREC = "キャリブレーション(画像登録)"
'        MSG_F_CAR = "キャリブレーション(補正実行)"
'        MSG_F_CUTREC = "外部カメラカット位置(画像登録)"
'        MSG_F_CUTREV = "外部カメラカット位置(補正実行)"
'        MSG_OPLOG_CMD = "コマンド"
'        '----- V1.18.0.1①↑ -----

'        MSG_ERRTRIMVAL = "トリミング目標値の範囲を超えています"
'        MSG_ERRCHKMEASM = "カットデータの測定モードに「2:外部」を選択している場合は変更できません"
'        MSG_txtLog_BLOCKMOVE = "ブロック移動モード"

'        '(トリミングパラメータ項目)
'        'data edit(resistor)
'        TRIMPARA(0) = "抵抗番号"
'        TRIMPARA(1) = "判定測定"
'        TRIMPARA(2) = "プローブ番号(HIGH)"
'        TRIMPARA(3) = "プローブ番号(LOW)"
'        TRIMPARA(4) = "プローブ番号(AG1)"
'        TRIMPARA(5) = "プローブ番号(AG2)"
'        TRIMPARA(6) = "プローブ番号(AG3)"
'        TRIMPARA(7) = "プローブ番号(AG4)"
'        TRIMPARA(8) = "プローブ番号(AG5)"
'        TRIMPARA(9) = "トリミング目標値"
'        TRIMPARA(52) = "ΔR"
'        TRIMPARA(53) = "切り上げ倍率"
'        TRIMPARA(10) = "イニシャルOKテスト(HIGH)"
'        TRIMPARA(11) = "イニシャルOKテスト(LOW)"
'        TRIMPARA(12) = "イニシャルテスト(HIGH)"
'        TRIMPARA(13) = "イニシャルテスト(LOW)"
'        TRIMPARA(14) = "ファイナルテスト(HIGH)"
'        TRIMPARA(15) = "ファイナルテスト(LOW)"
'        TRIMPARA(16) = "カット数"
'        'data edit(cut)
'        TRIMPARA(17) = "ディレイタイム"
'        TRIMPARA(18) = "ティーチングポイントX"
'        TRIMPARA(19) = "ティーチングポイントY"
'        TRIMPARA(20) = "スタートポイントX"
'        TRIMPARA(21) = "スタートポイントY"
'        TRIMPARA(22) = "カットスピード"
'        TRIMPARA(23) = "Qレート"
'        TRIMPARA(24) = "カットオフ"
'        TRIMPARA(25) = "パルス幅制御"
'        TRIMPARA(26) = "パルス幅時間"
'        TRIMPARA(27) = "LSwパルス幅時間"
'        TRIMPARA(28) = "カットパターン"
'        TRIMPARA(29) = "カット方向"
'        TRIMPARA(30) = "カット長1"
'        TRIMPARA(31) = "R1"
'        TRIMPARA(32) = "ターンポイント"
'        TRIMPARA(33) = "カット長2"
'        TRIMPARA(34) = "R2"
'        TRIMPARA(35) = "カット長3"
'        TRIMPARA(36) = "インデックス数"
'        TRIMPARA(37) = "測定モード"
'        TRIMPARA(38) = "カット2スピード"
'        TRIMPARA(39) = "Qレート2"
'        TRIMPARA(40) = "斜め角度"
'        TRIMPARA(41) = "ピッチ"
'        TRIMPARA(42) = "ステップ"
'        TRIMPARA(43) = "本数"
'        TRIMPARA(44) = "ESポイント"
'        TRIMPARA(45) = "ES判定化率"
'        TRIMPARA(46) = "ES後カット長"
'        TRIMPARA(47) = "倍率"
'        TRIMPARA(48) = "文字列"
'        TRIMPARA(49) = "Qレート3"
'        TRIMPARA(50) = "切替えポイント"
'        TRIMPARA(51) = "判定率"
'        TRIMPARA(52) = "EXTERNAL BIT"
'        TRIMPARA(53) = "ポーズ"
'        TRIMPARA(54) = "ES後判定率" ''''NETでは「トリムモード」
'        TRIMPARA(55) = "ES後確認回数" ''''NETでは「ベース抵抗」
'        TRIMPARA(56) = "ﾗﾀﾞｰ間距離"
'        TRIMPARA(57) = "カットオフオフセット"

'        MSG_BTN_CANCEL = "修正中のデータを元の状態に戻してもよろしいですか？"
'        MSG_AUTO_01 = "動作モード"
'        MSG_AUTO_02 = "マガジンモード"
'        MSG_AUTO_03 = "ロットモード"
'        MSG_AUTO_04 = "エンドレスモード"
'        MSG_AUTO_05 = "データファイル"
'        MSG_AUTO_06 = "登録済みデータファイル"
'        MSG_AUTO_07 = "リストの1つ上へ"
'        MSG_AUTO_08 = "リストの1つ下へ"
'        MSG_AUTO_09 = "リストから削除"
'        MSG_AUTO_10 = "リストをクリア"
'        MSG_AUTO_11 = "↓登録↓"
'        MSG_AUTO_12 = "OK"
'        MSG_AUTO_13 = "キャンセル"
'        MSG_AUTO_14 = "データ選択"
'        MSG_AUTO_15 = "登録リストを全て削除します。"
'        MSG_AUTO_16 = "よろしいですか？"
'        MSG_AUTO_17 = "エンドレスモード時は複数のデータファイルは選択できません。"
'        MSG_AUTO_18 = "データファイルを選択してください。"
'        MSG_AUTO_19 = "編集中のデータを保存しますか？"
'        MSG_AUTO_20 = "加工条件ファイルが存在しません。"

'        MSG_THETA_01 = "補正位置１"
'        MSG_THETA_02 = "補正位置２"
'        MSG_THETA_03 = "補正値"
'        MSG_THETA_04 = "START"
'        MSG_THETA_05 = "CANCEL"
'        MSG_THETA_06 = "指定座標"
'        MSG_THETA_07 = "ずれ量"
'        MSG_THETA_08 = "回転補正角度"
'        MSG_THETA_09 = "補正位置1"
'        MSG_THETA_10 = "補正位置2"
'        MSG_THETA_11 = "トリム位置"
'        MSG_THETA_12 = "補正後"
'        MSG_THETA_13 = "補正位置1　移動中"
'        MSG_THETA_14 = "補正位置2　移動中"
'        MSG_THETA_15 = "補正位置１を合わせてからSTARTキーを押してください"
'        MSG_THETA_16 = "補正位置２を合わせてからSTARTキーを押してください"
'        MSG_THETA_17 = "補正値を確認してSTARTキーを押してください"
'        MSG_THETA_18 = "Matching error"
'        MSG_THETA_19 = "Pattern Matching error(Position1)"
'        MSG_THETA_20 = "Pattern Matching error(Position1)"
'        MSG_THETA_21 = "Pattern Matching OK(Position1)"
'        MSG_THETA_22 = "ﾊﾟﾀｰﾝﾏｯﾁﾝｸﾞ"

'        MSG_TRIM_01 = "は加工範囲外です。"
'        MSG_TRIM_02 = "ローダー自動運転の起動ができませんでした。"
'        MSG_TRIM_03 = "処理を続行しますか？"
'        MSG_TRIM_04 = "イニシャルテスト　分布図"
'        MSG_TRIM_05 = "ファイナルテスト　分布図"
'        MSG_TRIM_06 = "自動ポジション"
'        BTN_TRIM_01 = "ｸﾞﾗﾌﾃﾞｰﾀ ｸﾘｱ"
'        BTN_TRIM_02 = "ﾌﾟﾚｰﾄﾃﾞｰﾀ編集"
'        BTN_TRIM_03 = "ｲﾆｼｬﾙﾃｽﾄ分布表示"
'        BTN_TRIM_04 = "ﾌｧｲﾅﾙﾃｽﾄ分布表示"
'        PIC_TRIM_01 = "イニシャルテスト　分布図"
'        PIC_TRIM_02 = "ファイナルテスト　分布図"
'        PIC_TRIM_03 = "良品"
'        PIC_TRIM_04 = "不良品"
'        PIC_TRIM_05 = "最小%"
'        PIC_TRIM_06 = "最大%"
'        PIC_TRIM_07 = "平均%"
'        PIC_TRIM_08 = "標準偏差"
'        PIC_TRIM_09 = "抵抗数"
'        PIC_TRIM_10 = "分布図保存"

'        MSG_LOADER_01 = "全停止異常発生中"
'        MSG_LOADER_02 = "ｻｲｸﾙ停止異常発生中"
'        MSG_LOADER_03 = "軽故障発生中"
'        MSG_LOADER_04 = "設定が完了しました"
'        MSG_LOADER_05 = "ローダエラー"
'        MSG_LOADER_06 = "ローダインターロック"
'        MSG_LOADER_07 = "パターンマッチングエラー"
'        MSG_LOADER_08 = "連続NG-HIｴﾗｰ"
'        MSG_LOADER_09 = "フルパワー測定 Q Rate 10kHz"
'        MSG_LOADER_10 = "レーザーパワーばらつきエラー"
'        MSG_LOADER_11 = "フルパワー減衰エラー"
'        MSG_LOADER_12 = "調整フルパワー測定"
'        MSG_LOADER_13 = "パワー調整"
'        MSG_LOADER_14 = "レーザーパワー調整エラー"
'        MSG_LOADER_15 = "自動運転終了"
'        MSG_LOADER_16 = "ローダアラームリスト"
'        MSG_LOADER_17 = "載物台上に基板が残っている場合は"
'        MSG_LOADER_18 = "取り除いてください。"
'        MSG_LOADER_19 = "基板品種が変わりました"                            ' ###089
'        MSG_LOADER_20 = "ＮＧ排出ボックスからＮＧ基板を取り除いてから"      ' ###089
'        MSG_LOADER_21 = "ＮＧ排出ボックス満杯"                              ' ###089
'        MSG_LOADER_22 = "STARTキー又はOKボタン押下で原点復帰します。"       ' ###124
'        MSG_LOADER_23 = "自動運転を中止します"                              ' ###124
'        '----- ###187↓ -----
'        MSG_LOADER_24 = "載物台クランプ制御"
'        MSG_LOADER_25 = "載物台吸着制御"
'        MSG_LOADER_26 = "ハンド吸着制御"
'        MSG_LOADER_27 = "供給ハンド吸着制御"
'        MSG_LOADER_28 = "収納ハンド吸着制御"
'        'MSG_LOADER_24 = "載物台クランプ解除"                                ' ###144
'        'MSG_LOADER_25 = "載物台吸着解除"                                    ' ###144
'        'MSG_LOADER_26 = "ハンド吸着解除"                                    ' ###144
'        'MSG_LOADER_27 = "供給ハンド吸着解除"                                ' ###158
'        'MSG_LOADER_28 = "収納ハンド吸着解除"                                ' ###158
'        '----- ###187↑ -----
'        MSG_LOADER_29 = "装置内に基板が残っている場合は"                    ' ###158
'        MSG_LOADER_30 = "自動運転停止中"                                    ' ###172
'        MSG_LOADER_31 = "OKボタン押下でアプリケーションを終了します"        ' ###175
'        MSG_LOADER_32 = "載物台の基板を取り除いてから"                      ' ###184 
'        MSG_LOADER_33 = "再度自動運転を実行して下さい"                      ' ###184 
'        MSG_LOADER_34 = "マガジンを基板検出センサＯＦＦ位置まで"            ' ###184 
'        MSG_LOADER_35 = "下げてから"                                        ' ###184 
'        MSG_LOADER_36 = "載物台の基板を取り除いて下さい"                    ' ###184 
'        '----- ###188↓ -----
'        MSG_LOADER_37 = "載物台上に基板が残っています"
'        MSG_LOADER_38 = "STARTキー又はOKボタン押下で"
'        MSG_LOADER_39 = "ステージを原点に戻します"
'        '----- ###188↑ -----
'        MSG_LOADER_40 = "ＮＧ基板を取り除いて下さい"                        ' ###197 
'        MSG_LOADER_41 = "ＮＧ基板を取り除いてから"                          ' ###197 
'        '----- ###240↓ -----
'        MSG_LOADER_42 = "載物台に基板がありません"
'        MSG_LOADER_43 = "基板を置いて再度実行して下さい"
'        '----- ###240↑ -----
'        '----- V1.18.0.0⑨↓ -----
'        MSG_LOADER_44 = "マガジン終了"
'        MSG_LOADER_45 = "OKボタン押下で自動運転を続行します"
'        MSG_LOADER_46 = "Cancelボタン押下で自動運転を終了します"
'        '----- V1.18.0.0⑨↑ -----
'        MSG_LOADER_47 = "プローブチェックエラー"                            ' V1.23.0.0⑦
'        MSG_LOADER_48 = "サイクル停止中" 'V4.0.0.0⑲
'        MSG_LOADER_49 = "割欠検出"       'V4.0.0.0⑲
'        MSG_LOADER_50 = "基板を投入してください"       'V4.11.0.0⑥

'        MSG_71 = "吸着ミスです。基板をセットし直して下さい。"
'        MSG_72 = "カバー有りの場合はスライドカバーを開いて、基板をセットし直して下さい。"
'        MSG_73 = "U-CUT用パラメータファイルは存在しません。"
'        MSG_74 = "U-CUT用パラメータファイルを開くことが出来ませんでした。"
'        MSG_75 = "U-CUT用パラメータではありません。"

'        ' NETのみ
'        STEP_TITLE01 = "サーキット座標"
'        STEP_TITLE02 = "グループ間インターバル"
'        STEP_TITLE03 = "ステップ間インターバル"
'        ' NETのみ

'        LBL_STEP_STEP = "ステップデータ"
'        LBL_STEP_GROUP = "グループデータ"
'        LBL_STEP_TY2 = "TY2データ"
'        LBL_ATT = "減衰率"
'        LBL_FLCUR = "電流値 "

'        ' ■ローダアラームメッセージ 
'        MSG_LDALARM_00 = "非常停止"
'        MSG_LDALARM_01 = "マガジン整合性アラーム"
'        MSG_LDALARM_02 = "割れ欠け品発生"
'        MSG_LDALARM_03 = "ハンド１吸着アラーム"
'        MSG_LDALARM_04 = "ハンド２吸着アラーム"
'        MSG_LDALARM_05 = "載物台吸着センサ異常"
'        MSG_LDALARM_06 = "載物台吸着ミス"
'        MSG_LDALARM_07 = "ロボットアラーム"
'        MSG_LDALARM_08 = "工程間監視アラーム"
'        MSG_LDALARM_09 = "エレベータ異常"
'        'MSG_LDALARM_10 = "マガジン無し"                                      '###073
'        MSG_LDALARM_10 = "マガジン又は基板無し"                               '###073
'        MSG_LDALARM_11 = "原点復帰タイムアウト"
'        MSG_LDALARM_12 = "クランプ異常" '###125
'        MSG_LDALARM_13 = "エアー圧低下検出"                                     'V4.0.0.0-54
'        MSG_LDALARM_14 = "未定義アラーム No."
'        MSG_LDALARM_15 = "未定義アラーム No."

'        MSG_LDALARM_16 = "旋回シリンダタイムアウト"
'        MSG_LDALARM_17 = "ハンド１シリンダタイムアウト"
'        MSG_LDALARM_18 = "ハンド２シリンダタイムアウト"
'        MSG_LDALARM_19 = "供給ハンド吸着ミス"
'        MSG_LDALARM_20 = "回収ハンド吸着ミス"
'        MSG_LDALARM_21 = "ＮＧ排出満杯"
'        MSG_LDALARM_22 = "一時停止"
'        MSG_LDALARM_23 = "ドアオープン"
'        MSG_LDALARM_24 = "二枚取り検出"
'        MSG_LDALARM_25 = "搬送上下機構アラーム" 'V4.0.0.0-59
'        MSG_LDALARM_26 = "未定義アラーム No."
'        MSG_LDALARM_27 = "未定義アラーム No."
'        MSG_LDALARM_28 = "未定義アラーム No."
'        MSG_LDALARM_29 = "未定義アラーム No."
'        MSG_LDALARM_30 = "未定義アラーム No."
'        MSG_LDALARM_31 = "未定義アラーム No."
'        MSG_LDALARM_UD = "未定義アラーム No."

'        MSG_LDGUID_00 = "非常停止ＳＷが押されました。"
'        MSG_LDGUID_01 = "マガジンが正規の位置か確認してください。"
'        'MSG_LDGUID_02 = "割れ欠け品が指定された枚数発生しました。"             'V1.16.0.0②
'        MSG_LDGUID_02 = "割れ欠け品が発生しました。基板を取り除いてください。"  'V1.16.0.0②
'        MSG_LDGUID_03 = "吸着センサを確認してください。"
'        MSG_LDGUID_04 = "吸着センサを確認してください。"
'        MSG_LDGUID_05 = "吸着センサを確認してください。"
'        MSG_LDGUID_06 = "吸着センサを確認してください。" + vbCrLf + "トッププレートにキズや基板のかけらが無いか確認してください。"
'        MSG_LDGUID_07 = "ロボットアラームが発生しました。"
'        MSG_LDGUID_08 = "工程間監視でタイムアウトが発生しました。"
'        MSG_LDGUID_09 = "エレベータのセンサを確認してください。"
'        'MSG_LDGUID_10 = "マガジン検出ドグが倒れているか、マガジンをセットしてください。"        '###073
'        MSG_LDGUID_10 = "マガジン検出ドグが倒れているか、マガジン又は基板をセットしてください。" '###073
'        '----- V1.18.0.0⑩↓ -----
'        ' ローム殿特注(マガジン検出ドグはないのでメッセージ変更する)
'        If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
'            MSG_LDGUID_10 = "マガジン又は基板をセットしてください。"
'        End If
'        '----- V1.18.0.0⑩↑ -----
'        MSG_LDGUID_11 = "原点復帰時にタイムアウトが発生しました。"
'        MSG_LDGUID_12 = "クランプ異常が発生しました。" '###125
'        MSG_LDGUID_13 = "エアーが正しく供給されているか確認してください。" 'V4.0.0.0-54
'        MSG_LDGUID_14 = "メーカにアラーム番号を問い合わせてください。"
'        MSG_LDGUID_15 = "メーカにアラーム番号を問い合わせてください。"

'        MSG_LDGUID_16 = "シリンダセンサを確認してください。"
'        MSG_LDGUID_17 = "シリンダセンサを確認してください。"
'        MSG_LDGUID_18 = "シリンダセンサを確認してください。"
'        MSG_LDGUID_19 = "基板吸着時にタイムアウトが発生しました。"
'        MSG_LDGUID_20 = "基板吸着時にタイムアウトが発生しました。"
'        MSG_LDGUID_21 = "ＮＧ排出ＢＯＸが満杯です。基板を取り除いてください。"
'        MSG_LDGUID_22 = "一時停止中です。"
'        MSG_LDGUID_23 = "ドアオープンが検出されました。ドアを閉じてください。"
'        MSG_LDGUID_24 = "供給ハンドの基板を取り除いて再実行してください。" ' V1.18.0.0⑪
'        MSG_LDGUID_25 = "搬送上下機構のリミットセンサを確認してください。"  'V4.0.0.0-59
'        MSG_LDGUID_26 = "メーカにアラーム番号を問い合わせてください。"
'        MSG_LDGUID_27 = "メーカにアラーム番号を問い合わせてください。"
'        MSG_LDGUID_28 = "メーカにアラーム番号を問い合わせてください。"
'        MSG_LDGUID_29 = "メーカにアラーム番号を問い合わせてください。"
'        MSG_LDGUID_30 = "メーカにアラーム番号を問い合わせてください。"
'        MSG_LDGUID_31 = "メーカにアラーム番号を問い合わせてください。"
'        MSG_LDGUID_UD = "メーカにアラーム番号を問い合わせてください。"

'        MSG_LDINFO_UD = "想定外アラーム"

'    End Sub
'#End Region

'#Region "英語メッセージを設定する"
'    '''=========================================================================
'    '''<summary>英語メッセージを設定する</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesEnglish()

'        '-----------------------------------------------------------------------
'        '   エラーメッセージ
'        '-----------------------------------------------------------------------
'        MSG_0 = "Already this program is running."
'        MSG_11 = "The resistor number is already used in line %1."
'        MSG_14 = "Trimming parameter data is not loaded."
'        MSG_15 = "Not found the selected file."
'        MSG_16 = "The selected file is not the data of this program."
'        MSG_21 = "Please set the BlockSize_X with the right way"
'        MSG_22 = "Please set the BlockSize_Y with the right way."
'        MSG_25 = "Prease set the setting of MovementMode(DigiSwitch) in from x0 to x6 "
'        MSG_36 = "Out of laser beam area."
'        MSG_37 = "Please include center of cross lines into the pattern matching data area."
'        MSG_38 = "Please make sure pattern template group number."
'        MSG_39 = "No resistor to correct cutting position."
'        MSG_40 = "Not all probe pins does make contact."
'        MSG_41 = "Probe pins could not make contact. correct "
'        MSG_42 = "Z axis reached limit position."
'        MSG_43 = "Tested all pins contact, but detect some pins does not make contact."
'        MSG_44 = "Auto probe position teaching is success."
'        MSG_45 = "Trimming condition data is not loaded."
'        MSG_46 = "Condition data"

'        MSG_101 = "Do you overwrite it?"
'        MSG_102 = "Are you sure quit program?"
'        MSG_103 = "Are you sure?　　　　　　　　　　　　　　　　　　　　　　　　　　　　"
'        MSG_104 = "The editted data is not saving.　　　　　　　　　　　　　　　　　　　"
'        MSG_105 = "This screen returns to the before screen.Are you sure?　　　　　　"
'        MSG_106 = "This data is saved and this screen returns to the before screen.Are you sure?"
'        MSG_107 = "Please set the different PlobeNumber."
'        MSG_108 = "Do you clear the sum total?"

'        MSG_114 = "Dust vaccumer alarm"
'        MSG_115 = "There are data editing. Do you load it?"
'        MSG_116 = "Data load confirmation"
'        MSG_117 = "There are data editing."
'        MSG_118 = "When a revision mode is set by nothing, i cannot carry it out."
'        MSG_119 = "Please input it within half size British number 1-18 characters."
'        MSG_120 = "The calibration is ended."
'        MSG_121 = "Please push ""Yes"" when you maintain data and push ""No"" when you do not maintain data."
'        MSG_122 = "The upper limit value of cut speed is dirty." & vbCrLf & vbCrLf & "A case including a ST2 cut, the upper limit value of cut speed are %1 mm/s." & vbCrLf & "(An object is all the cuts than a ST2 cut in front.)"
'        MSG_126 = "Pattern match error.Do you run manual teaching?"
'        MSG_127 = "Pattern match error." 'V1.13.0.0③


'        MSG_130 = "Trimming Data File Read Error."
'        MSG_131 = "Trimming Data File Write Error."
'        MSG_132 = "It Failed In The Transmission Of The Condition Data." + vbCrLf + "Please Load Data Or Set Condition From Edit Function."
'        MSG_133 = "It Failed In Reading The Intermediate File."
'        MSG_134 = "It Failed In Writting The Intermediate File."
'        MSG_135 = "Data Edit Error. Do The Reload Of Data."
'        MSG_136 = "Serial Port Open Error."
'        MSG_137 = "Serial Port Close Error."
'        MSG_138 = "Serial Port Transmission Error."
'        MSG_139 = "Serial Port Reception Error."
'        MSG_140 = "There Is No Setting On The FL Side." + vbCrLf + "Please Load Data Or Set Condition From Edit Function."
'        MSG_141 = "Condition Reading Error On The FL Side."
'        MSG_142 = "Condition File Was Made."
'        MSG_142 = "Condition File Was Made."
'        MSG_143 = "DATA LOAD OK"
'        MSG_144 = "DATA LOAD NG"
'        MSG_145 = "DATA SAVE OK"
'        MSG_146 = "DATA SAVE NG"
'        MSG_147 = "DATA SEND TO FL"
'        MSG_148 = "Data sending to FL......"
'        MSG_149 = "FiberLaser conditions are editing. Do you load it?"
'        MSG_150 = "Connection error for FiberLaser." + vbCrLf + "Please confirm the connection."
'        MSG_151 = "It Failed In The Setting Of Processing Conditions."
'        '----- V1.18.0.0③↓ -----
'        MSG_152 = "Initial Processing Of Printing Failed."
'        MSG_153 = "The Printing Job Failed."
'        MSG_154 = "Printing Termination Processing Failed."
'        '----- V1.18.0.0③↑ -----
'        MSG_155 = "The number of substrates is initialized." + vbCrLf + "Is it all right ?" 'V1.18.0.0⑫
'        MSG_156 = "Probe Check Error." 'V1.23.0.0⑦
'        MSG_157 = "Bar Code Does Not Exist." 'V1.23.0.0①
'        MSG_158 = "Condition File Read Error." 'V2.0.0.0⑤
'        MSG_159 = "A File Exists. Is It Overwritten ?" 'V4.0.0.0⑨
'        '----- V4.0.0.0-30↓ -----
'        MSG_160 = "Please adjust the probe cleaning position.(XY TABLE)"
'        MSG_161 = "MOVE:[Arrow]  OK:[START]  CANCEL:[RESET]"
'        MSG_162 = "Please adjust the probe cleaning position.(PROBE)"
'        '----- V4.0.0.0-30↑ -----

'        MSG_PP23 = "Please make the probe Z offset distance as longer than the upward distance. "
'        MSG_PP24_1 = "The probe Z-axis upward distance is exceeding the Z-axis waiting position."
'        MSG_PP24_2 = "Please make the waiting position distance as longer than the z-axis upward distance."

'        MSGERR_COVER_CLOSE = "Failure to close the slide cover."
'        MSGERR_SEND_TRIMDATA = "Failure to set the trimming data." & vbCrLf & "Please confirm the trimming data."

'        MSG_TOTAL_CIRCUIT = "The unit of the circuit."
'        MSG_TOTAL_REGISTOR = "The unit of the resistor."
'        MSG_COM = "The inputed number is not in the input number area.（%1~%2）"
'        MSG_PR_COM1 = "Invalid data. (%1 - %2)"
'        MSG_PR04_1 = "Same value as %1."

'        LOAD_MSG01 = "The specified file is not found."

'        MSG_CUT_1 = "Can't copy datas to another number resistor."

'        '(データ編集画面のエラーMSG)
'        DATAEDIT_ERRMSG01 = "Please process resistor."
'        DATAEDIT_ERRMSG02 = "Please process cut."
'        DATAEDIT_ERRMSG03 = "Input Error!"
'        DATAEDIT_ERRMSG04 = "The %1 is already used in %2."
'        ''''2009/7/03 NETのみ
'        DATAEDIT_ERRMSG05 = "Please set pulse width time in"
'        DATAEDIT_ERRMSG06 = "xx-xx."
'        DATAEDIT_ERRMSG07 = "A re-setup of Q rate."
'        DATAEDIT_ERRMSG08 = "The decimal point '.' is inputted once or more."
'        DATAEDIT_ERRMSG09 = "An ideal value of a Q rate is "
'        DATAEDIT_ERRMSG10 = " []."
'        DATAEDIT_ERRMSG11 = "Is it set as an ideal value?"
'        ''''2009/7/03 NETのみ

'        INPUTAREA(0) = "Please input in the range of "
'        INPUTAREA(1) = " ."
'        INPUTAREA(2) = "INPUT ERROR ["
'        INPUTAREA(3) = "]"
'        INPUTAREA(4) = "DOUBLE REGISTRATION ERROR ["
'        INPUTAREA(5) = "The step number is already used in line %1."
'        INPUTAREA(6) = "Please set it so that a total of the number of the block becomes "
'        INPUTAREA(7) = "."
'        INPUTAREA(8) = " I surpass a mobile range of an XY table."

'        CHKMSG(0) = "Block numerical data are cleared."
'        CHKMSG(1) = "Data of a position revision method are cleared."
'        CHKMSG(2) = "Data to one or more revision position turn center are cleared."
'        CHKMSG(3) = "Are you sure quit program?"
'        CHKMSG(4) = "CONFIRMATION"
'        CHKMSG(5) = "I return to a state at the time of new making when I clear editing data."
'        CHKMSG(6) = "Does it perform?"
'        CHKMSG(7) = "Deleted cut data are cleared."
'        CHKMSG(8) = "Data of a NG marking are erased."
'        CHKMSG(9) = "Data of a power adjustment mode are erased."
'        CHKMSG(10) = "Please set pulse width time with "
'        CHKMSG(11) = " XX"

'        '-----------------------------------------------------------------------
'        '   タイトルメッセージ
'        '-----------------------------------------------------------------------
'        TITLE_1 = "Deletion confirmation"
'        TITLE_2 = "Double registration error"
'        TITLE_3 = "Out of the inputted data."
'        TITLE_4 = "Abort"
'        TITLE_5 = "File confirmation"
'        TITLE_6 = "Necessary item error"
'        TITLE_7 = "Confirmation"
'        TITLE_8 = "The inputted item error"
'        TITLE_LASER = "Laser adjustment"
'        TITLE_LOGGING = "Logging process"

'        '-----------------------------------------------------------------------
'        '   ラベルメッセージ
'        '-----------------------------------------------------------------------
'        'メイン画面ラベル
'        MSG_MAIN_LABEL01 = "PLATE ="
'        MSG_MAIN_LABEL02 = "NG(%)="
'        MSG_MAIN_LABEL03 = "IT H NG(%)="
'        MSG_MAIN_LABEL04 = "IT L NG(%)="
'        MSG_MAIN_LABEL05 = "FT H NG(%)="
'        MSG_MAIN_LABEL06 = "FT L NG(%)="
'        MSG_MAIN_LABEL07 = "OVER NG(%)="

'        ' 一時停止画面用 ###014
'        LBL_FINEADJ_001 = "DATA EDIT"
'        '----- ###204↓ ----- 
'        LBL_FINEADJ_002 = "ADJUSTMENT"
'        LBL_FINEADJ_003 = "0:No Display"
'        LBL_FINEADJ_004 = "1:Display only NG Logs"
'        LBL_FINEADJ_005 = "2:Display All Logs"
'        '----- ###204↑ ----- 
'        LBL_FINEADJ_006 = "Substrate Set"    ' V4.11.0.0⑥
'        'teaching
'        LBL_TEACHING_001 = "AUTO NEXT CUT POSITIONING"
'        LBL_TEACHING_002 = "ARROW SW:BPOFFSET, START:NEXT"
'        LBL_TEACHING_003 = "ARROW SW:CUT START POS, START:NEXT, HALT:PREV"
'        'cut pos teaching
'        LBL_CUTPOSTEACH_001 = "CUT POSITION CORRECTION TEACHING"
'        LBL_CUTPOSTEACH_002 = "AUTO POSITIONING"
'        LBL_CUTPOSTEACH_003 = "START: NEXT, HALT: PREV, ARROW SW:BP MOVE"
'        'LBL_CUTPOSTEACH_004 = "PATTERN TEACHING(&P)"
'        LBL_CUTPOSTEACH_004 = "PATTERN TEACHING"

'        'recog
'        LBL_RECOG_001 = "OK(&E)"
'        LBL_RECOG_002 = "Cancel(&C)"
'        LBL_RECOG_003 = "Matching test(&T)"
'        LBL_RECOG_004 = "Search area(&S)"
'        LBL_RECOG_005 = "Template regist(&R)"
'        LBL_RECOG_006 = "Exit(&X)"
'        LBL_RECOG_007 = "Delete template(&D)"
'        LBL_RECOG_008 = "Position offset"
'        LBL_RECOG_009 = "Adjust(&P)"
'        LBL_RECOG_010 = "XY table position"
'        LBL_RECOG_011 = "ARROW SW:Adjust BP position, START:OK, HALT:Cancel"
'        LBL_RECOG_012 = "Move mouse and Drag the blue rectangle to fit to the pattern."
'        LBL_RECOG_013 = "Move mouse and Drag the yellow rectangle to fit to the pattern."
'        LBL_RECOG_014 = "SET"
'        LBL_RECOG_015 = "XY Table position"
'        LBL_RECOG_016 = "GroupNo"
'        LBL_RECOG_017 = "TempNo"
'        LBL_RECOG_018 = "Thresh"
'        LBL_RECOG_019 = "Contrast"
'        LBL_RECOG_020 = "Brightness"

'        MSG_RECOG_001 = "Setting was competed."
'        MSG_RECOG_002 = "May I delete it?"
'        'data edit(plate)

'        '基板種別対応
'        If gSysPrm.stCTM.giSPECIAL = customASAHI Then
'            LBL_PP_1 = "PLATE TYPE"
'        Else
'            LBL_PP_1 = "DATA NAME"
'        End If
'        LBL_PP_2 = "TRIM MODE"
'        LBL_PP_3 = "STEP＆REPEAT"
'        LBL_PP_4 = "PLATE NUMBER"
'        LBL_PP_5 = "BLOCK"
'        LBL_PP_6 = "PLATE INTERVAL"
'        LBL_PP_8 = "TABLE OFFSET (mm)"
'        LBL_PP_9 = "BEAM OFFSET (mm)"
'        LBL_PP_10 = "ADJUST POINT (mm)"
'        LBL_PP_12 = "NG MARKING"
'        LBL_PP_13 = "DELAY TRIM"
'        LBL_PP_14 = "NG JUDGMENT UNIT"
'        LBL_PP_15 = "NG JUDGMENT STANDARD (%)"
'        LBL_PP_16 = "PROBE Z OFF1 (mm)"
'        LBL_PP_17 = "PROBE STEP Z UP (mm)"
'        LBL_PP_18 = "PROBE Z OFF2 (mm)"
'        LBL_PP_19 = "RESISTOR AXIS" '"R-AXIS"

'        ''''2009/07/03 NET のみ
'        LBL_PP_126 = "CIRCUIT NUMBER" '"１グループ内　サーキット数"
'        LBL_PP_127 = "CIRCUIT SIZE(mm)" 'サーキットサイズ(mm)"
'        '..

'        LBL_PP_128 = "THETA (deg）"

'        LBL_PP_20 = "RESISTOR NUMBER" '"R-NUM"
'        LBL_PP_21 = "GROUP NUMBER" '"NUMBER OF GROUP"
'        LBL_PP_22 = "GROUP INTERVAL"
'        LBL_PP_23 = "CHIP SIZE (mm)"
'        LBL_PP_123 = "STEP OFFSET (mm)"
'        LBL_PP_24 = "ADJ. BLOCK SIZE (mm)"
'        LBL_PP_25 = "BLOCK INTERVAL (mm)"
'        LBL_PP_26 = "CONST NG-H"
'        LBL_PP_REVISE = "POSI REVISE "
'        LBL_PP_30 = "ADJ.MODE"
'        LBL_PP_31 = "ANG.ADJ."
'        LBL_PP_32 = "ADJUST POINT1 (mm)"
'        LBL_PP_33 = "ADJUST POINT2 (mm)"
'        LBL_PP_34 = "ADJUST POS. OFFSET (mm)"
'        LBL_PP_35 = "DISPLAY MODE"
'        LBL_PP_36 = "PIXEL(μm)"
'        LBL_PP_37 = "PATTERN NO. "
'        LBL_PP_38 = "PATTERN GROUP NO"
'        LBL_PP_39 = "ROTATION AXIS (mm)"
'        'Calibration
'        LBL_PP_CARIB = "Calibration "
'        LBL_PP_40 = "C-REF.COORDINATE1 (mm)"
'        LBL_PP_41 = "C-REF.COORDINATE2 (mm)"
'        LBL_PP_42 = "C-TABLE OFFSET XY (mm)"
'        LBL_PP_43 = "C-REGIST NO."
'        LBL_PP_CARIBGRPNO = "C-REGIST GrpNo."
'        LBL_PP_44 = "C-LENGTH (mm)"
'        LBL_PP_45 = "C-SPEED (mm/s)"
'        LBL_PP_46 = "C-QRATE (kHz)"
'        LBL_PP_47 = "TT-ADJUST OFFSET (mm)"
'        LBL_PP_48 = "TT-REGIST NO. PTN"
'        LBL_PP_49 = "TT-LENGTH (mm)"
'        LBL_PP_50 = "TT-SPEED (mm/s)"
'        LBL_PP_51 = "TT-QRATE (kHz)"
'        LBL_PP_52 = "GROUP NO. PTN"
'        LBL_PP_53 = "TRIMMING NG COUNT (Maximum)"
'        LBL_PP_54 = "CRACK CHIP DISCHARGE COUNT (Maximum)"
'        LBL_PP_55 = "CONST NG-TRIMMING" '"Continuation trimming NG number of sheets"
'        LBL_PP_115 = "RE-PROBING NUMBER"
'        LBL_PP_116 = "RE-PROBING OFFSET.(mm)"
'        LBL_PP_117 = "POWER ADJUSTMENT MODE(x0,x1,x5)"
'        LBL_PP_118 = "ADJUSTMENT POWER.(W)"
'        LBL_PP_119 = "POWER ADJUSTMENT QRATE"
'        LBL_PP_120 = "ADJUSTMENT TOLERANCE"
'        LBL_PP_121 = "INITIAL OK TEST"
'        LBL_PP_122 = "BASE KIND" '"SELECT THE KIND APPOINTED ON THE LOADER SIDE."
'        LBL_PP_124 = "4 TERMINALS OPEN CHECK"
'        LBL_PP_125 = "STEP＆REPEAT OFFSET（mm）"
'        LBL_PP_126 = "LED CONTROL(AUTO MODE)"
'        LBL_PP_127 = "THETA (deg）"
'        LBL_PP_130 = "GP-IB" 'GP-IB制御
'        LBL_PP_131 = "DELIMITER" '初期設定(デリミタ)
'        LBL_PP_132 = "TIME OUT" '初期設定(タイムアウト)
'        LBL_PP_133 = "ADDRESS" 'G初期設定(機器アドレス)
'        LBL_PP_134 = "INI COMMAND" '初期化コマンド
'        LBL_PP_135 = "TRIGGER COMMAND" 'トリガコマンド
'        LBL_PP_136 = "GP-IB" 'GP-IB制御
'        LBL_PP_137 = "ADDRESS" '機器アドレス
'        LBL_PP_138 = "MEASUREMENT SPEED" '測定速度
'        LBL_PP_139 = "MEASUREMENT MODE" '測定モード

'        LBL_PP_140 = "T_THETA(deg)" '(Ｔθオフセット)
'        LBL_PP_141 = "T_THETA ADJUST POINT1(mm)" '(Ｔθ基準位置１)
'        LBL_PP_142 = "T_THETA ADJUST POINT2(mm)" '(Ｔθ基準位置１)

'        'data edit(step)
'        LBL_S_1 = "STEP NO."
'        LBL_S_2 = "BLOCK"
'        LBL_S_3 = "INTERVAL"

'        LBL_S_4 = "STEP NO."
'        LBL_S_5 = "DIRECTION X"
'        LBL_S_6 = "DIRECTION Y"

'        LBL_S_7 = "STEP NO."
'        LBL_S_8 = "CIRCUIT" & vbCrLf & "NUMBER"
'        LBL_S_9 = "INTERVAL"


'        '(TXTY仕様変更)
'        LBL_G_1 = "GROUP NO."
'        LBL_G_2 = "RESISTOR"
'        LBL_G_3 = "INTERVAL"

'        LBL_TY2_1 = "BLOCK NO."
'        LBL_TY2_2 = "Step Interval"

'        'data edit(registor)
'        MSG_REGISTER_LABEL01 = "Res No."
'        MSG_REGISTER_LABEL02 = "Meas"
'        MSG_REGISTER_LABEL03 = "Probe No."
'        MSG_REGISTER_LABEL04 = "EXTERNAL BIT"
'        MSG_REGISTER_LABEL05 = "Pause" & vbCrLf & "Time" & vbCrLf & "(msec)"
'        MSG_REGISTER_LABEL06 = "Abs." & vbCrLf & "Ratio"
'        MSG_REGISTER_LABEL07 = "Base res." & vbCrLf & "No"
'        MSG_REGISTER_LABEL08 = "Target Value"
'        MSG_REGISTER_LABEL09 = "Voltage Slope"
'        MSG_REGISTER_LABEL10 = "Initial Test (%)"
'        MSG_REGISTER_LABEL11 = "Final Test (%)"
'        MSG_REGISTER_LABEL12 = "Cut No."
'        MSG_REGISTER_LABEL13 = "Cut" & vbCrLf & "Corr"
'        MSG_REGISTER_LABEL14 = "Disp" & vbCrLf & "Mode"
'        MSG_REGISTER_LABEL15 = "Pat No."
'        MSG_REGISTER_LABEL16 = "Cut Correct Pos"
'        MSG_REGISTER_LABEL17 = "NG" & vbCrLf & "Mode"
'        MSG_REGISTER_LABEL18 = "Hi Lmit"
'        MSG_REGISTER_LABEL19 = "Low Limit"
'        MSG_REGISTER_LABEL20 = "Initial OK Test (%)"
'        MSG_REGISTER_LABEL21 = "ΔR" & vbCrLf & "(%)"
'        MSG_REGISTER_LABEL22 = "Mag" & vbCrLf & ""
'        ''''2009/07/03 NETのみ
'        MSG_REGISTER_LABEL25 = "Cir" & vbCrLf & "No."
'        MSG_REGISTER_LABEL26 = "Trim" & vbCrLf & "Mode"
'        MSG_REGISTER_LABEL27 = "Base" & vbCrLf & "Res"
'        '..
'        'from TKY
'        MSG_REGISTER_LABEL33 = "Probe confirmation position"
'        MSG_REGISTER_LABEL34 = "H-X" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL35 = "H-Y" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL36 = "L-X" & vbCrLf & "(mm)"
'        MSG_REGISTER_LABEL37 = "L-Y" & vbCrLf & "(mm)"

'        'data edit(cut)
'        MSG_CUT_LABEL01 = "Res" & vbCrLf & "No."
'        MSG_CUT_LABEL02 = "Cut" & vbCrLf & "No."
'        MSG_CUT_LABEL03 = "Delay" & vbCrLf & "Time" & vbCrLf & "(ms)"
'        MSG_CUT_LABEL04 = "Teaching" & vbCrLf & "Point X" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL05 = "Teaching" & vbCrLf & "Point Y" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL06 = "Start" & vbCrLf & "Point X" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL07 = "Start" & vbCrLf & "Point Y" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL08 = "Cut" & vbCrLf & "Speed" & vbCrLf & "(mm/s)"
'        MSG_CUT_LABEL09 = "Q" & vbCrLf & "Rate" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL10 = "Cut Off" & vbCrLf & "(%)"
'        MSG_CUT_LABEL11 = "Pulse W"
'        MSG_CUT_LABEL12 = "Pulse W" & vbCrLf & "Time" & vbCrLf & "(ns)"
'        MSG_CUT_LABEL13 = "LSwPulse W" & vbCrLf & "Time" & vbCrLf & "(us)"
'        MSG_CUT_LABEL14 = "Pat" & vbCrLf & "+" & vbCrLf & "Direc"
'        MSG_CUT_LABEL15 = "Cut" & vbCrLf & "Len1" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL16 = "R1" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL17 = "Turn" & vbCrLf & "Point" & vbCrLf & "(%)"
'        MSG_CUT_LABEL18 = "Cut" & vbCrLf & "Len2" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL19 = "R2" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL20 = "Cut" & vbCrLf & "Len3" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL21 = "Index" & vbCrLf & "No."
'        MSG_CUT_LABEL22 = "Meas." & vbCrLf & "Mode"
'        MSG_CUT_LABEL23 = "Cut2" & vbCrLf & "Speed"
'        MSG_CUT_LABEL24 = "Q" & vbCrLf & "Rate2" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL25 = "Ang"
'        MSG_CUT_LABEL26 = "Pitch"
'        MSG_CUT_LABEL27 = "S" & vbCrLf & "t" & vbCrLf & "e" & vbCrLf & "p"
'        MSG_CUT_LABEL28 = "Lines"
'        MSG_CUT_LABEL29 = "ES- Point" & vbCrLf & "(%)"
'        MSG_CUT_LABEL30 = "ES- %"
'        MSG_CUT_LABEL31 = "ES- Len2" & vbCrLf & "(mm)"
'        MSG_CUT_LABEL32 = "Mag" '”"U Cut" & vbCrLf & "Dummy1"
'        MSG_CUT_LABEL33 = "Charactors" '"U Cut" & vbCrLf & "Dummy2"
'        MSG_CUT_LABEL34 = "Q" & vbCrLf & "Rate3" & vbCrLf & "(kHz)"
'        MSG_CUT_LABEL35 = "Change" & vbCrLf & "Point" & vbCrLf & "(%)"
'        MSG_CUT_LABEL36 = "Cut Off" & vbCrLf & "Average" & vbCrLf & "(%)"
'        MSG_CUT_LABEL37 = "ES-" & vbCrLf & "%" & vbCrLf & "(After)"
'        MSG_CUT_LABEL38 = "ES-" & vbCrLf & "Cnt" & vbCrLf & "(After)"
'        MSG_CUT_LABEL39 = "Distance" & vbCrLf & "between" & vbCrLf & "ladders" & vbCrLf & "(micron)"
'        MSG_CUT_LABEL40 = "Cut Off" & vbCrLf & "Offset" & vbCrLf & "(%)"

'        'teaching
'        MSG_TEACH_LABEL01 = "Beam Point Offset"
'        MSG_TEACH_LABEL02 = "Trimming Start Point"
'        MSG_TEACH_LABEL03 = "Resistor No."
'        MSG_TEACH_LABEL04 = "Finish Teaching"
'        MSG_TEACH_LABEL05 = "Resistor Name"

'        'plobe position
'        MSG_PROBE_LABEL01 = "MANUAL"
'        MSG_PROBE_LABEL02 = "AUTO 1:Z"
'        MSG_PROBE_LABEL03 = "AUTO 2:XY"
'        MSG_PROBE_LABEL05 = "TEACHING FINISH"
'        MSG_PROBE_LABEL06 = "PROBE "
'        MSG_PROBE_LABEL14 = "PB TEACH"
'        MSG_PROBE_MSG01 = "AUTO PROBE POSITION ADJUSTMENT"
'        MSG_PROBE_MSG02 = "AUTO PROBE POSITION ADJUSTMENT"
'        MSG_PROBE_MSG03 = "CURRENT POS="
'        MSG_PROBE_MSG04 = "DOWN DIST="
'        MSG_PROBE_MSG05 = "A mode to aline"
'        MSG_PROBE_MSG06 = "Position"
'        MSG_PROBE_MSG07 = "Table move"
'        MSG_PROBE_MSG08 = "Top block"
'        MSG_PROBE_MSG09 = "Middle block"
'        MSG_PROBE_MSG10 = "End block"
'        MSG_PROBE_MSG11 = "The resistance value measurement"
'        MSG_PROBE_MSG12 = "DRM-KGD" '"The difference electric current measurement"
'        MSG_PROBE_LABEL04 = "MEASURE"
'        MSG_PROBE_MSG13 = "CONSECUTIVE MEASURE"
'        MSG_PROBE_MSG14 = "ALL MEASURE"
'        MSG_PROBE_MSG15 = "CONSECUTIVE STOP"
'        MSG_PROBE_MSG16 = "A resistance measurement error."
'        'frmMsgBox(画面終了確認)
'        MSG_CLOSE_LABEL01 = "Exit?"
'        MSG_CLOSE_LABEL02 = "Yes(&Y)"
'        MSG_CLOSE_LABEL03 = "No(&N)"
'        'frmReset(原点復帰画面など)
'        MSG_FRMRESET_LABEL01 = "INITIALIZING"
'        'LASER_teaching
'        MSG_LASER_LABEL01 = "START:LASER ON"
'        MSG_LASER_LABEL02 = "HALT:LASER OFF"
'        MSG_LASER_LABEL03 = "Condition No."
'        MSG_LASER_LABEL04 = "Set Condition"
'        MSG_LASER_LABEL05 = "Q SWITCH RATE (kHz)"
'        MSG_LASER_LABEL06 = "STEG Count"
'        MSG_LASER_LABEL07 = "Current(mA)"
'        MSG_LASER_LABEL08 = "Please set the number of process."

'        ' ■パワー調整(FL用) ###066
'        MSG_AUTOPOWER_01 = "Start Power Adjustment"
'        MSG_AUTOPOWER_02 = "Condition No."
'        MSG_AUTOPOWER_03 = "Laser Power"
'        MSG_AUTOPOWER_04 = "Current"
'        MSG_AUTOPOWER_05 = "Power Adjustment Failed."
'        MSG_AUTOPOWER_06 = "Power Adjustment Normal End" 'V1.16.0.0⑧

'        DE_EXPLANATION(0) = "PREV      :"
'        DE_EXPLANATION(1) = "NEXT      :"
'        DE_EXPLANATION(2) = "PREV PAGE :"
'        DE_EXPLANATION(3) = "NEXT PAGE :"
'        DE_EXPLANATION(4) = "Repeat    :"
'        DE_EXPLANATION(5) = "[↑]key"
'        DE_EXPLANATION(6) = "[↓]key"
'        DE_EXPLANATION(7) = "[←]key"
'        DE_EXPLANATION(8) = "[→]key"
'        DE_EXPLANATION(9) = "[Enter]key"
'        DE_EXPLANATION(10) = "Cancel    :"
'        DE_EXPLANATION(11) = "[Esc]key"
'        MSG_ONOFF(0) = "OFF"
'        MSG_ONOFF(1) = "ON"

'        '-----------------------------------------------------------------------
'        '   ボタン表示
'        '-----------------------------------------------------------------------
'        LBL_CUT_COPYLINE = "Copy Line(&L)"
'        LBL_CUT_COPYCOLUMN = "Copy Column(&C)"

'        LBL_CMD_OK = "OK (&O)"
'        LBL_CMD_CANCEL = "Cancel (&Q)"
'        LBL_CMD_CLEAR = "Clear(&K)"
'        LBL_CMD_LCOPY = "LineCopy(&L)"

'        '-----------------------------------------------------------------------
'        '   スプラッシュメッセージ
'        '-----------------------------------------------------------------------
'        MSG_SPRASH0 = "INITIALIZING"
'        MSG_SPRASH1 = "Under the resetting for XY trim position"
'        MSG_SPRASH2 = "Under moving for trim position"
'        MSG_SPRASH3 = "PUSH START SW"
'        MSG_SPRASH4 = "PUSH START SW"
'        MSG_SPRASH5 = "The interlock is canceled"
'        MSG_SPRASH6 = "EMERGENCY"
'        MSG_SPRASH7 = "Please check and push the emergency stop release SW"
'        MSG_SPRASH8 = "Under initialization processing"
'        MSG_SPRASH9 = "Please close the slide cover"
'        MSG_SPRASH10 = "Please close the enclosure cover"
'        MSG_SPRASH11 = "Please change the AUTO LOADER mode to MANUAL"
'        MSG_SPRASH12 = "Fail the air pressure"
'        MSG_SPRASH13 = "Consecutive NG-HI Errors."
'        MSG_SPRASH14 = "Re Probing NG"
'        MSG_SPRASH15 = "PUSH START SW TO QUIT PROGRAM"
'        MSG_SPRASH16 = "PUSH RESET SW TO QUIT PROGRAM"
'        MSG_SPRASH17 = "If The Cancel Button Is Pressed, The Program Is Ended."
'        MSG_SPRASH18 = "Loader Communication Timeout Error."
'        MSG_SPRASH19 = "In Abnormal All Stops, Occurring."
'        MSG_SPRASH20 = "In Abnormal A Cycle Stop, Occurring."
'        MSG_SPRASH21 = "In Light Trouble Outbreak."
'        MSG_SPRASH22 = "Push START SW To Start The Automatic Operation."
'        MSG_SPRASH23 = "The Error Occurred In Trimming."
'        MSG_SPRASH24 = "Loader Initializing"
'        MSG_SPRASH25 = "Under Interlock Release"
'        MSG_SPRASH26 = "Under Interlock Part Release"
'        MSG_SPRASH27 = "SAFETY COVER OPENED"
'        MSG_SPRASH28 = "SLIDE COVER OPENED"
'        MSG_SPRASH29 = "Please Close The Cover, Or Turn On The Interlock Switch."
'        MSG_SPRASH30 = "PLC status error."
'        MSG_SPRASH31 = "Cautions !!!"
'        MSG_SPRASH32 = "Slide Cover Closes Automatically."
'        MSG_SPRASH33 = "PUSH RESET SW TO QUIT PROCESS"
'        MSG_SPRASH34 = "SAFETY COVER or SLIDE COVER OPENED"
'        MSG_SPRASH35 = "[START]:Continue, [RESET]:End"                  ' ###073
'        MSG_SPRASH36 = "Please Close The Cover"                         ' ###073
'        MSG_SPRASH37 = "Loader Initializing Uncompleted."               ' ###137 
'        MSG_SPRASH38 = "It Is Moving To XY Origin Position"             ' ###188
'        MSG_SPRASH39 = "Under Stop."                                    ' V1.13.0.0③
'        MSG_SPRASH40 = "CV Error Detection."                            ' V1.13.0.0⑪
'        MSG_SPRASH41 = "Over Load Detection."                           ' V1.13.0.0⑪
'        MSG_SPRASH42 = "ReProbing Error."                               ' V1.13.0.0⑪
'        MSG_SPRASH43 = "Front Door Lock Time Out"                       ' V1.18.0.1⑧
'        MSG_SPRASH44 = "Front Door Unlock Time Out"                     ' V1.18.0.1⑧
'        MSG_SPRASH45 = "Please remove substrate in EQ"                  ' V4.0.0.0-71
'        MSG_SPRASH46 = "Equipment Initializing"                         ' V4.0.0.0-83
'        MSG_SPRASH47 = "Moving to Home Position"                        ' V4.0.0.0-83
'        MSG_SPRASH48 = "Trimming data is not exist read from QR Code Reader."       ' V4.1.0.0①
'        MSG_SPRASH49 = "Please confirm trimming data file."                         ' V4.1.0.0①
'        MSG_SPRASH50 = "PLC Low Battery Detect"                         ' V4.1.0.0⑦
'        MSG_SPRASH51 = "Please Change Battery"                          ' V4.1.0.0⑦

'        '----- limit.frm用 -----
'        MSG_frmLimit_01 = "BEAM POSITION LIMIT"
'        MSG_frmLimit_02 = "Cut start point(s) is out of the BP moving area. Please correct BP offset."
'        MSG_frmLimit_03 = "X AXIS LIMIT"
'        MSG_frmLimit_04 = "Y AXIS LIMIT"
'        MSG_frmLimit_05 = "Z AXIS LIMIT"
'        MSG_frmLimit_06 = "Z2 AXIS LIMIT"
'        MSG_frmLimit_07 = "PUSH START SW or OK BUTTON"

'        '----- INtime側エラーメッセージ -----
'        MSG_SRV_ALM = "Servo Alarm."
'        MSG_AXIS_X_SERVO_ALM = "X Axis Servo Alarm."
'        MSG_AXIS_Y_SERVO_ALM = "Y Axis Servo Alarm."
'        MSG_AXIS_Z_SERVO_ALM = "Z Axis Servo Alarm."
'        MSG_AXIS_T_SERVO_ALM = "Theta Axis Servo Alarm."

'        ' 軸系エラー(タイムアウト)
'        MSG_TIMEOUT_AXIS_X = "X Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_Y = "Y Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_Z = "Z Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_T = "Theta Axis Error(Time Out)"
'        MSG_TIMEOUT_AXIS_Z2 = "Z2 Axis Error(Time Out)"
'        MSG_TIMEOUT_ATT = "Rotary Attenuater Error(Time Out)"
'        MSG_TIMEOUT_AXIS_XY = "XY Axis Error(Time Out)"

'        ' 軸系エラー(プラスリミットオーバー)
'        MSG_STG_SOFTLMT_PLUS_AXIS_X = "X Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_Y = "Y Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z = "Z Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_T = "Theta Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS_AXIS_Z2 = "Z2 Axis +Limit Over. "
'        MSG_STG_SOFTLMT_PLUS = "Axis +Limit Over."

'        ' 軸系エラー(マイナスリミットオーバー)
'        MSG_STG_SOFTLMT_MINUS_AXIS_X = "X Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS_AXIS_Y = "Y Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z = "Z Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS_AXIS_T = "Theta Axis -Limit Over."
'        MSG_STG_SOFTLMT_MINUS_AXIS_Z2 = "Z2 Axis -Limit Over. "
'        MSG_STG_SOFTLMT_MINUS = "Axis -Limit Over."

'        ' 軸系エラー(リミット検出)
'        MSG_AXS_LIM_X = "X-axis limit detection."
'        MSG_AXS_LIM_Y = "Y-axis limit detection."
'        MSG_AXS_LIM_Z = "Z-axis limit detection."
'        MSG_AXS_LIM_T = "Theta-axis limit detection."
'        MSG_AXS_LIM_Z2 = "Z2-axis limit detection."
'        MSG_AXS_LIM_ATT = "ATT-axis limit detection."

'        ' BP系エラー
'        MSG_BP_MOVE_TIMEOUT = "BP Time Out."
'        MSG_BP_GRV_ALARM_X = "Galvano Alarm X."
'        MSG_BP_GRV_ALARM_Y = "Galvano Alarm Y."
'        MSG_BP_HARD_LIMITOVER_LO = "BP Limit Over.(Minimum)"
'        MSG_BP_HARD_LIMITOVER_HI = "BP Limit Over.(Maximum)"
'        MSG_BP_LIMITOVER = "BP Moved Distance Setting Limit Exaggerated."
'        MSG_BP_SOFT_LIMITOVER = "Moveble BP Soft Range Exaggerated."
'        MSG_BP_BSIZE_OVER = "Exaggerated Setting Of Block Size."

'        ' カバー開検出
'        MSG_OPN_CVR = "Cover open detection."
'        MSG_OPN_SCVR = "Slidecover open detection."
'        MSG_OPN_CVRLTC = "Coveropen latch detection."

'        MSG_INTIME_ERROR = "INTRIM Error"

'        '-----------------------------------------------------------------------
'        '   ステータスバーメッセージ
'        '-----------------------------------------------------------------------
'        'data edit(plate 01)
'        '基板種別対応
'        If gSysPrm.stCTM.giSPECIAL = customASAHI Then
'            CONST_PP(glNo_DataNo) = "Appoint the plate type.(1:50x60/2:78x78)"
'        Else
'            CONST_PP(glNo_DataNo) = "Appoint the data name."
'        End If
'        '    CONST_PP(glMeasType) = "Appoint a trim mode.(0:RESISTOR, 1:DC VOLTAGE)"
'        '    CONST_PP(glNo_PlateCntXDir) = "Appoint the number of the plates of an X direction."
'        '    CONST_PP(glNo_PlateCntYDir) = "Appoint the number of the plates of an Y direction."
'        '    CONST_PP(glPlateInvXDir) = "Appoint plate distance of an X direction by a mm unit."
'        '    CONST_PP(glPlateInvYDir) = "Appoint plate distance of an X direction by a mm unit."

'        '(ﾒｯｾｰｼﾞ変更)
'        CONST_PP(glNo_DirStepRepeat) = "Appoint the table step direction.(0:NO, 1:X, 2:Y, 3:CHIP WIDTH+X)" '"APPOINT THE TABLE STEP DIRECTION.(0:NO, 1:X, 2:Y)"
'        CONST_PP(glNo_MeasType) = "Appoint the meas type(0:resistor , 1:DC voltage)"

'        CONST_PP(glNo_BlockCntXDir) = "Appoint the block No. of X-direction."
'        CONST_PP(glNo_BlockCntYDir) = "Appoint the block No. of Y-direction."
'        CONST_PP(glNo_TableOffSetXDir) = "Appoint the table offset in X-direction by mm unit."
'        CONST_PP(glNo_TableOffSetYDir) = "Appoint the table offset in Y-direction by mm unit."
'        CONST_PP(glNo_BpOffsetXDir) = "Appoint the beam position offset in X-direction by mm unit."
'        CONST_PP(glNo_BpOffsetYDir) = "Appoint the beam position offset in Y-direction by mm unit."
'        CONST_PP(glNo_AdjOffsetXDir) = "Appoint the adjust point of X-direction by mm unit."
'        CONST_PP(glNo_AdjOffsetYDir) = "Appoint the adjust point of Y-direction by mm unit."
'        CONST_PP(glNo_NGMark) = "NG Marking?（0:NO, 1:YES）"

'        If gSysPrm.stSPF.giDelayTrim2 = 1 Then
'            CONST_PP(glNo_DelayTrim) = "Delay trim ? (0:NO, 1:YES, 2:Delay trim2 YES)"
'        Else
'            CONST_PP(glNo_DelayTrim) = "Delay trim ? (0:NO, 1:YES)"
'        End If

'        CONST_PP(glNo_NgJudgeUnit) = "Appoint the unit for NG judgment. (0:BLOCK, 1:PLATE)"
'        CONST_PP(glNo_NgJudgeLevel) = "Appoint the NG judgment standard by % unit."
'        CONST_PP(glNo_ZOffSet) = "Appoint probe Z-SHAFT offset by mm unit."
'        CONST_PP(glNo_ZStepUpDist) = "Appoint rising distance of probe step by mm unit."
'        CONST_PP(glNo_ZWaitOffset) = "Appoint stand by position distance of probe Z-SHAFT by mm unit."
'        CONST_PP(glNo_LedCtrl) = "Appoint the number of the circuits in one group." '"１グループ内　サーキット数を指定"
'        '    CONST_PP(glPP127X) = "Appoint circuit size X." '"サーキットサイズＸを指定"
'        '    CONST_PP(glPP127Y) = "Appoint circuit size Y." '"サーキットサイズＹを指定"
'        'data edit(plate 03)
'        CONST_PP(glNo_ResistDir) = "Appoint the direction of row resistor(0:X，1:Y)"
'        If (gTkyKnd = KND_CHIP) Then
'            CONST_PP(glNo_ResistCntInGroup) = "Appoint the number of resistor trimming by one probing."
'        ElseIf (gTkyKnd = KND_NET) Then
'            CONST_PP(glNo_ResistCntInGroup) = "Appoint the number of circuit trimming by one probing."
'        End If
'        CONST_PP(glNo_GroupCntInBlockXBp) = "Appoint the number of group in 1 block of the X direction."
'        CONST_PP(glNo_GroupCntInBlockYStage) = "Appoint the number of group in 1 block of the Y direction."
'        CONST_PP(glNo_GroupItvXDir) = "Appoint the group interval in the X direction by mm unit."
'        CONST_PP(glNo_GroupItvYDir) = "Appoint the group interval in the Y direction by mm unit."
'        CONST_PP(glNo_ChipSizeXDir) = "Appoint the chip size in the X direction by mm unit."
'        CONST_PP(glNo_ChipSizeYDir) = "Appoint the chip size in the Y direction by mm unit."
'        CONST_PP(glNo_StepOffsetXDir) = "Appoint the step offset in X direction by mm unit."
'        CONST_PP(glNo_StepOffsetYDir) = "Appoint the step offset in Y direction by mm unit."
'        CONST_PP(glNo_BlockSizeReviseXDir) = "Appoint the block correction offsets in the X direction by mm unit."
'        CONST_PP(glNo_BlockSizeReviseYDir) = "Appoint the block correction offsets in the Y direction by mm unit."
'        CONST_PP(glNo_BlockItvXDir) = "Appoint the block interval in the X direction by mm unit."
'        CONST_PP(glNo_BlockItvYDir) = "Appoint the block interval in the Y direction by mm unit."
'        CONST_PP(glNo_ContHiNgBlockCnt) = "Appoint the continuous NG-HIGH resistor check blocks."
'        'data edit(plate 02)
'        CONST_PP(glNo_ReviseMode) = "Specify the correction mode(0:AUTO,1:MANUAL,2:AUTO+FINE)"
'        CONST_PP(glNo_ManualReviseType) = "Specify the manual correction method(0:NONE, 1:ONE TIME, 2:EVERY TIME)"
'        CONST_PP(glNo_ReviseCordnt1XDir) = "Specify the correct position-1 X (mm)"
'        CONST_PP(glNo_ReviseCordnt1YDir) = "Specify the correct position-1 Y (mm)"
'        CONST_PP(glNo_ReviseCordnt2XDir) = "Specify the correct position-2 X (mm)"
'        CONST_PP(glNo_ReviseCordnt2YDir) = "Specify the correct position-2 Y (mm)"
'        CONST_PP(glNo_ReviseOffsetXDir) = "Specify the correct position offset X (mm)"
'        CONST_PP(glNo_ReviseOffsetYDir) = "Specify the correct position offset Y (mm)"
'        CONST_PP(glNo_RecogDispMode) = "Pattern matching display mode(0:NONE, 1:DISPLAY)"
'        CONST_PP(glNo_PixelValXDir) = "Pixel resolution X in micron"
'        CONST_PP(glNo_PixelValYDir) = "Pixel resolution Y in micron"
'        CONST_PP(glNo_RevisePtnNo1) = "Position 1 Pattern No."
'        CONST_PP(glNo_RevisePtnNo2) = "Position 2 Pattern No."
'        CONST_PP(glNo_RevisePtnGrpNo1) = "Position 1 Pattern Group No."
'        CONST_PP(glNo_RevisePtnGrpNo2) = "Position 2 Pattern Group No."
'        CONST_PP(glNo_RotateXDir) = "Appoint the rotation axis in X-direction."
'        CONST_PP(glNo_RotateYDir) = "Appoint the rotation axis in Y-direction."
'        'data edit(plate 04)
'        CONST_PP(glNo_CaribBaseCordnt1XDir) = "Appoint calibration reference coordinate 1 in X-direction by mm unit."
'        CONST_PP(glNo_CaribBaseCordnt1YDir) = "Appoint calibration reference coordinate 1 in Y-direction by mm unit."
'        CONST_PP(glNo_CaribBaseCordnt2XDir) = "Appoint calibration reference coordinate 2 in X-direction by mm unit."
'        CONST_PP(glNo_CaribBaseCordnt2YDir) = "Appoint calibration reference coordinate 2 in Y-direction by mm unit."
'        CONST_PP(glNo_CaribTableOffsetXDir) = "Appoint the calibration offset in X-direction by mm unit."
'        CONST_PP(glNo_CaribTableOffsetYDir) = "Appoint the calibration offset in Y-direction by mm unit."
'        CONST_PP(glNo_CaribPtnNo1) = "Appoint the calibration pattern 1 regist group No."
'        CONST_PP(glNo_CaribPtnNo2) = "Appoint the calibration pattern 2 regist group No."
'        CONST_PP(glNo_CaribPtnNo1GroupNo) = "Appoint the calibration pattern 1 regist No."
'        CONST_PP(glNo_CaribPtnNo2GroupNo) = "Appoint the calibration pattern 2 regist No."
'        CONST_PP(glNo_CaribCutLength) = "Appoint the calibration cutting length by mm unit."
'        CONST_PP(glNo_CaribCutSpeed) = "Appoint the calibration cutting speed by mm/sec unit."
'        CONST_PP(glNo_CaribCutQRate) = "Appoint the calibration laser pulse rate by kHz unit."
'        CONST_PP(glNo_CutPosiReviseOffsetXDir) = "Appoint the cut adjust offset in X-direction by mm unit."
'        CONST_PP(glNo_CutPosiReviseOffsetYDir) = "Appoint the cut adjust offset in Y-direction by mm unit."
'        CONST_PP(glNo_CutPosiRevisePtnNo) = "Appoint the cut adjust pattern regist No in X and Y-direction."
'        CONST_PP(glNo_CutPosiReviseCutLength) = "Appoint the cut adjust cutting length by mm unit."
'        CONST_PP(glNo_CutPosiReviseCutSpeed) = "Appoint the cut adjust cutting speed by mm/sec unit."
'        CONST_PP(glNo_CutPosiReviseCutQRate) = "Appoint the cut adjust laser pulse rate by kHz unit."
'        CONST_PP(glNo_CutPosiReviseGroupNo) = "Pattern Group No."
'        'data edit(plate 05)
'        '=== 436Kパラメータの為一旦コメント
'        '    CONST_PP(glNo_MaxTrimNgCount) = "Appoint the trimming NG counter.(MAX)"
'        '    CONST_PP(glNo_MaxBreakDischargeCount) = "Appoint the crack chip discharge counter.(MAX)"
'        '    CONST_PP(glNo_TrimNgCount) = "Appoint the continuation trimming NG number."
'        '    CONST_PP(glNo_InitialOkTestDo) = "Appoint the initial OK test.(0:NO, 1:YES)"
'        '    CONST_PP(glNo_WorkSetByLoader) = "Appoint the kind of a substrate." '"Select the kind appointed on the loader side."

'        CONST_PP(glNo_RetryProbeCount) = "Appoint the Re-probing number."
'        CONST_PP(glNo_RetryProbeDistance) = "Appoint the Re-probing offset.(mm)"
'        CONST_PP(glNo_PowerAdjustMode) = "Appoint the power adjustment mode.(0:NO, 1:YES)"
'        CONST_PP(glNo_PowerAdjustTarget) = "Appoint the adjustment power.(W)"
'        CONST_PP(glNo_PowerAdjustQRate) = "Appoint the adjustment laser pulse rate.(kHz)"
'        CONST_PP(glNo_PowerAdjustToleLevel) = "Appoint the adjustment tolerance."
'        CONST_PP(glNo_OpenCheck) = "Appoint the 4 terminals open check.(0:NO, 1:YES)"
'        CONST_PP(glNo_DistStepRepeat) = "Appoint the table step by mm unit."
'        CONST_PP(glNo_LedCtrl) = "Appoint a control method of LED.(0:Regular ON, 1:Regular OFF, 2:Only at the time of image recognition.)" '"LEDの制御方法を指定(0:常時ON, 1:常時OFF, 2:画像認識時のみ)"
'        CONST_PP(glNo_RotateTheta) = "Specify the correct theta position (deg)"
'        '    CONST_PP(glPP128) = "Specify the correct theta position (deg)"
'        CONST_PP(glNo_GpibMode) = "Appoint GP-IB control.(0:NO, 1:YES)"
'        CONST_PP(glNo_GpibDelim) = "Appoint a cord of a delimiter.(0:CR+LF, 1:CR, 2:LF, 3:NONE)"
'        CONST_PP(glNo_GpibTimeOut) = "Appoint a limit value of time-out by a 100ms unit."
'        CONST_PP(glNo_GpibAddress) = "Please address it."
'        CONST_PP(glNo_GpibInitCmnd) = "Appoint an initialization command."
'        CONST_PP(glNo_GpibInit2Cmnd) = "Appoint an initialization command."
'        CONST_PP(glNo_GpibTrigCmnd) = "Appoint a trigger command."
'        CONST_PP(glNo_Gpib2Mode) = "Appoint GP-IB control.(0:NO, 1:YES)"
'        CONST_PP(glNo_Gpib2Address) = "Please address it."
'        CONST_PP(glNo_Gpib2MeasSpeed) = "Appoint the measurement speed. (0: Low speed, 1: High speed)"
'        CONST_PP(glNo_Gpib2MeasMode) = "Appoint the measurement speed. (0: Absolute, 1: Deviation)"
'        CONST_PP(glNo_TThetaOffset) = "Specify the correct T_theta position (deg)"
'        CONST_PP(glNo_TThetaBase1XDir) = "Appoint T_theta reference coordinate 1 in X-direction by mm unit."
'        CONST_PP(glNo_TThetaBase1YDir) = "Appoint T_theta reference coordinate 1 in Y-direction by mm unit."
'        CONST_PP(glNo_TThetaBase2XDir) = "Appoint T_theta reference coordinate 2 in X-direction by mm unit."
'        CONST_PP(glNo_TThetaBase2YDir) = "Appoint T_theta reference coordinate 2 in Y-direction by mm unit."

'        'data edit(step)
'        CONST_SP(0) = "STEP NO."
'        CONST_SP(1) = "BLOCK"
'        CONST_SP(2) = "INTERVAL"
'        CONST_CA(0) = "Appoint a step number." '"ステップ番号を指定"
'        CONST_CA(1) = "Appoint circuit coordinate X by a mm unit." '"サーキット座標Ｘをmm単位で指定"
'        CONST_CA(2) = "Appoint circuit coordinate Y by a mm unit." '"サーキット座標Ｙをmm単位で指定"
'        CONST_CI(0) = "Appoint a step number." '"ステップ番号を指定"
'        CONST_CI(1) = "Appoint the number of the circuits had by a step." '"各ステップに含まれるサーキット数を指定"
'        CONST_CI(2) = "Appoint an interval between circuits by a mm unit." '"各サーキット間のインターバルをmm単位で指定"

'        'data edit(group)
'        CONST_GP(0) = "GROUP NO."
'        CONST_GP(1) = "RESISTOR"
'        CONST_GP(2) = "INTERVAL"

'        CONST_TY2(0) = "BLOCK NO."
'        CONST_TY2(1) = "Appoint X-direction starting point by mm unit"
'        CONST_TY2(2) = "Appoint Y-direction starting point by mm unit"

'        'data edit(resistor)
'        CONST_PR1 = "Appoint the resistor No. (After 1000 Marking data)"
'        CONST_PR2 = "Appoint the measurement, judgment mode.(0:SPEED, 1:ACCURACY)"
'        CONST_PR3 = "Appoint the circuit No. Which the resistor belongs."
'        CONST_PR4H = "Appoint the probe No. on high side."
'        CONST_PR4L = "Appoint the probe No. on low side."
'        CONST_PR4G = "Appoint the first active guard No."
'        CONST_PR5 = "Appoint the EXTERNAL BIT(16 bit)."
'        CONST_PR6 = "Appoint the pause time after the EXTERNAL BIT output by msec unit."
'        MSG_19 = "Please set the EXTERNAL BIT by 16bit."
'        MSG_20 = "Please set the EXTERNAL BIT by 1 or 0."

'        ''''2009/07/03 NET のみ
'        CONST_PR7 = "Appoint a mode to trim. (0:Absolute value/1:Ratio)" '"トリムモードを指定（0:絶対値/1:レシオ）"
'        CONST_PR8 = "Appoint a base resistance number of ratio trimming." '"レシオトリミングのベース抵抗番号を指定"

'        CONST_PR9 = "Appoint the target value of trimming."
'        CONST_PR9_1 = "Appoint the magnification of trimming."

'        CONST_PR11H = "Appoint the high limit of the initial test by % unit."
'        CONST_PR11L = "Appoint the low limit of the initial test by % unit."
'        CONST_PR12H = "Appoint the high limit of the final test by % unit."
'        CONST_PR12L = "Appoint the low limit of the final test by % unit."
'        CONST_PR13 = "Appoint the cut No. in one resistor."
'        CONST_PR10 = "Appoint ΔR by a % unit."
'        CONST_PR14H = "Appoint the high limit of the initial OK test by % unit."
'        CONST_PR14L = "Appoint the low limit of the initial OK test by % unit."
'        CONST_PR15 = "Appoint close magnification."
'        'data edit(cut)
'        CONST_CP2 = "Appoint the delay time by msec unit."
'        CONST_CP99X = "Appoint X-direction teaching point by mm unit."
'        CONST_CP99Y = "Appoint Y-direction teaching point by mm unit."
'        CONST_CP4X = "Appoint X-direction starting point by mm unit."
'        CONST_CP4Y = "Appoint Y-direction starting point by mm unit."
'        CONST_CP5 = "Appoint cut speed by mm/s unit."
'        CONST_CP5_2 = "Specify second cut speed by mm/s"
'        CONST_CP6 = "Appoint laser Q-switch rate by kHz unit."
'        CONST_CP6_2 = "Appoint second laser Q-switch rate by kHz unit."
'        CONST_CP7 = "Appoint cut off value by % unit."
'        CONST_CP7_1 = "Appoint cut off average by % unit."
'        CONST_CP7_2 = "Appoint cut off offset value by % unit."
'        CONST_CP50 = "Appoint practice presence of pulse width cuntrol by % unit.(0:NO, 1:YES)"
'        CONST_CP51 = "Appoint pulse width time by nsec unit."
'        CONST_CP52 = "Appoint LSw pulse width time by usec unit."
'        CONST_CP9 = "Appoint the max cutting length by mm unit."
'        CONST_CP11 = "Appoint L-turn point by % unit."
'        CONST_CP12 = "Appoint the max cutting length to L-turn point by mm unit."
'        CONST_CP13 = "Specify cut angle by degree."
'        CONST_CP14 = "Appoint the max cutting length after L-turn by mm unit."
'        CONST_CP15 = "Appoint cutting length after hook turn by mm unit."
'        CONST_CP17 = "Appoint index length by mm unit."
'        CONST_CP18 = "Appoint the max index number."
'        CONST_CP19 = "Appoint the measure mode before the index cut.(0:SPEED 1:ACCURACY)"
'        CONST_CP19_A = "Appoint the measure mode before the index cut.(0:SPEED 1:ACCURACY, 2:External)"
'        CONST_CP30 = "Cut pattern"
'        CONST_CP31 = "Cut Direction"
'        CONST_CP20 = "Scan cut pitch(mm)"
'        CONST_CP21 = "Scan cut step direction"
'        CONST_CP22 = "Scan cut lines"
'        CONST_CP23 = "Marking character magnify"
'        CONST_CP24 = "Marking characters"
'        CONST_CP38 = "Appoint an edge sence point."
'        CONST_CP39 = "Appoint a judgment change rate of an edge sence."
'        CONST_CP40 = "Appoint a cut length after an edge sence."
'        CONST_CPR1 = "Specify the circular arc length as mm unit from L-turn point."
'        CONST_CPR2 = "Specify the circular arc length as mm unit before hook-turn point."

'        CONST_CP53 = "Appoint a Q switch rate of a laser by a kHz unit." '"レーザのＱスイッチレートをkHz単位で指定"
'        CONST_CP54 = "Appoint a change point by a % unit." '"切替えポイントを%単位で指定"
'        CONST_CP55 = "The change rate after the edge sense is specified. "
'        CONST_CP56 = "The confirmation frequency after the edge sense is specified. "
'        CONST_CP57 = "Please appoint distance between ladders by a micron unit."

'        'data edit
'        STS_21 = "1:+X, 2:-Y, 3:-X, 4:+Y"
'        STS_22 = "1:+X+Y, 2:-Y+X, 3:-X-Y, 4:+Y-X, 5:+X-Y, 6:-Y-X, 7:-X+Y, 8:+Y+X"
'        STS_23 = "1:CW, 2:CCW"
'        STS_24 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, K:ES, M:MARKING" 'ESﾊﾞｰｼﾞｮﾝ
'        STS_69 = "1:ST, 2:L, 3:HK, 4:IX, 5:SC, 6:ST(TU), 7:L(TU), 8:ST(TR), 9:L(TR), A:AST, H:UCUT, S:ES2, M:MARKING" 'ES2ﾊﾞｰｼﾞｮﾝ

'        'GP-IB制御無し？
'        If gSysPrm.stCTM.giGP_IB_flg = 0 Then

'        Else
'            'ﾎﾟｼﾞｼｮﾆﾝｸﾞ無しｲﾝﾃﾞｯｸｽ追加
'            STS_24 = STS_24 & ", X:ST2, X:IX2"
'            STS_69 = STS_69 & ", X:ST2, X:IX2"
'        End If

'        STS_26 = "Please specify a value within the limits of 1 and 4 (1:+X, 2:-X, 3:+Y, 4:-Y)"
'        STS_27 = "Please input within the full size of 20 characters(half size of 40 characters)."

'        '-----------------------------------------------------------------------
'        '   メッセージ
'        '-----------------------------------------------------------------------
'        'プローブ位置合わせメッセージ
'        MSG_PRB_XYMODE = "The XY table movement mode."
'        MSG_PRB_BPMODE = "The positioner movement mode."
'        MSG_PRB_ZTMODE = "Z teaching mode"
'        MSG_PRB_THETA = "THETA movement mode."
'        'システムエラーメッセージ
'        MSG_ERR_ZNOTORG = "The Z-axis origin point sensor is not turned on."
'        'レーザー調整画面説明文
'        MSG_LASER_LASERON = "START: Laser irradiation"
'        MSG_LASER_LASEROFF = "HALT: Laser stop"
'        MSG_LASEROFF = "The laser is under stop."
'        MSG_LASERON = "Under laser irradiation "
'        MSG_ATTRATE = "Rate"
'        MSG_ERRQRATE = "Please confirm the Q-rate preset value."
'        MSG_LOGERROR = "An error occurred in writing for the log file."
'        ' 画像登録画面説明文
'        MSG_PATERN_MSG01 = "Setting was competed."

'        '操作ログメッセージ
'        MSG_OPLOG_WAKEUP = "TRIMMER WAKEUP"
'        MSG_OPLOG_FUNC01 = "LOAD"
'        MSG_OPLOG_FUNC02 = "SAVE"
'        MSG_OPLOG_FUNC03 = "EDIT"
'        MSG_OPLOG_FUNC04 = "PARAM"
'        MSG_OPLOG_FUNC05 = "LASER"
'        MSG_OPLOG_FUNC06 = "LOG"
'        MSG_OPLOG_FUNC07 = "PROBE"
'        MSG_OPLOG_FUNC08 = "TEACH"
'        MSG_OPLOG_FUNC20 = "Recog Pattern for cut position revise"
'        MSG_OPLOG_FUNC09 = "RECOG"
'        MSG_OPLOG_FUNC10 = "TRIMMER SHUTDOWN"
'        MSG_OPLOG_FUNC30 = "Circuit Teaching"
'        MSG_OPLOG_AUTO = "AUTO"
'        MSG_OPLOG_LOADERINIT = "LOADER INIT"
'        '----- V1.13.0.0③↓ -----
'        MSG_OPLOG_APROBEREC = "AUTO PROBE RECOG"
'        MSG_OPLOG_APROBEEXE = "AUTO PROBE EXECUTE"
'        MSG_OPLOG_IDTEACH = "ID TEACH"
'        MSG_OPLOG_SINSYUKU = "ELASTIC COMPENSATION(RECOG)"
'        MSG_OPLOG_MAP = "MAP Select"
'        '----- V1.13.0.0③↑ -----
'        MSG_OPLOG_CLRTOTAL = "COUNTER CLEARED"
'        MSG_OPLOG_TRIMST = "TRIMMING"
'        MSG_OPLOG_TRIMRES = "Stopped by RESET SW during the trimming."
'        MSG_OPLOG_HCMD_ERRRST = "HOST ERR RST CMD"
'        MSG_OPLOG_HCMD_PATCMD = "HOST PATTERN CMD"
'        MSG_OPLOG_HCMD_LASCMD = "HOST LASER CMD"
'        MSG_OPLOG_HCMD_MARKCMD = "HOST MARKING CMD"
'        MSG_OPLOG_HCMD_LODCMD = "HOST LOAD CMD"
'        MSG_OPLOG_HCMD_TECCMD = "HOST TEACH CMD"
'        MSG_OPLOG_HCMD_TRMCMD = "HOST TRM CMD"
'        MSG_OPLOG_HCMD_LSTCMD = "HOST CMD: LOGSTART CMD"
'        MSG_OPLOG_HCMD_LENCMD = "HOST CMD: LOGSTOP CMD"
'        MSG_OPLOG_HCMD_MDAUTO = "HOST CMD: AUTO MODE CHG"
'        MSG_OPLOG_HCMD_MDMANU = "HOST CMD: MANUAL MODE CHG"
'        MSG_OPLOG_HCMD_CPCMD = "HOST CMD: CP TEACH CMD"
'        MSG_POPUP_MESSAGE = "POPUP MESSAGE"
'        MSG_OPLOG_FUNC08S = "CUTTING POSITION CORRECTION TEACHING"

'        '----- V1.18.0.1①↓ -----
'        MSG_F_EXR1 = "ExCamera Teaching(R1)"
'        MSG_F_EXTEACH = "ExCamera Teaching(ALL)"
'        MSG_F_CARREC = "Calibration(RECOG)"
'        MSG_F_CAR = "Calibration(CORRECT)"
'        MSG_F_CUTREC = "ExCamCutPosition(RECOG)"
'        MSG_F_CUTREV = "ExCamCutPosition(CORRECT)"
'        MSG_OPLOG_CMD = " COMMAND"
'        '----- V1.18.0.1①↑ -----

'        'BLICK MOVE処理用メッセージ
'        MSG_txtLog_BLOCKMOVE = "BLOCKMOVE MODE"

'        MSG_ERRTRIMVAL = "I surpass the trimming targeted value."
'        MSG_ERRCHKMEASM = "It cannot change, when 2:exterior is chosen as the measurement mode of cut data."

'        '(トリミングパラメータ項目)
'        'data edit(resistor)
'        TRIMPARA(0) = "RES No." '"抵抗番号"
'        TRIMPARA(1) = "JUDGMENT MEASUREMENT" '"判定測定"
'        TRIMPARA(2) = "PROBE No(HIGH)" '"プローブ番号(HIGH)"
'        TRIMPARA(3) = "PROBE No(LOW)" '"プローブ番号(LOW)"
'        TRIMPARA(4) = "PROBE No(AG1)" '"プローブ番号(AG1)"
'        TRIMPARA(5) = "PROBE No(AG2)" '"プローブ番号(AG2)"
'        TRIMPARA(6) = "PROBE No(AG3)" '"プローブ番号(AG3)"
'        TRIMPARA(7) = "PROBE No(AG4)" '"プローブ番号(AG4)"
'        TRIMPARA(8) = "PROBE No(AG5)" '"プローブ番号(AG5)"
'        TRIMPARA(9) = "TRIMMING TARGET VALUE" '"トリミング目標値"
'        TRIMPARA(52) = "ΔR"
'        TRIMPARA(53) = "Mag"
'        TRIMPARA(10) = "INITIAL OK TEST(HIGH)" '"イニシャルOKテスト(HIGH)"
'        TRIMPARA(11) = "INITIAL OK TEST(LOW)" '"イニシャルOKテスト(LOW)"
'        TRIMPARA(12) = "INITIAL TEST(HIGH)" '"イニシャルテスト(HIGH)"
'        TRIMPARA(13) = "INITIAL TEST(LOW)" '"イニシャルテスト(LOW)"
'        TRIMPARA(14) = "FINAL TEST(HIGH)" '"ファイナルテスト(HIGH)"
'        TRIMPARA(15) = "FINAL TEST(LOW)" '"ファイナルテスト(LOW)"
'        TRIMPARA(16) = "CUT NUM" '"カット数"
'        'data edit(cut)
'        TRIMPARA(17) = "DELAY TIME" '"ディレイタイム"
'        TRIMPARA(18) = "TEACHING POINT X" '"ティーチングポイントX"
'        TRIMPARA(19) = "TEACHING POINT Y" '"ティーチングポイントY"
'        TRIMPARA(20) = "START POINT X" '"スタートポイントX"
'        TRIMPARA(21) = "START POINT Y" '"スタートポイントY"
'        TRIMPARA(22) = "CUT SPEED" '"カットスピード"
'        TRIMPARA(23) = "Q RATE" '"Qレート"
'        TRIMPARA(24) = "CUT OFF" '"カットオフ"
'        TRIMPARA(25) = "PULSE WIDTH CUNTROL" '"パルス幅制御"
'        TRIMPARA(26) = "PULSE WIDTH TIME" '"パルス幅時間"
'        TRIMPARA(27) = "LSw PULSE WIDTH TIME" '"LSwパルス幅時間"
'        TRIMPARA(28) = "CUT PAT" '"カットパターン"
'        TRIMPARA(29) = "CUT DEC" '"カット方向"
'        TRIMPARA(30) = "CUT LEN1" '"カット長1"
'        TRIMPARA(31) = "R1"
'        TRIMPARA(32) = "TURN POINT" '"ターンポイント"
'        TRIMPARA(33) = "CUT LEN2" '"カット長2"
'        TRIMPARA(34) = "R2"
'        TRIMPARA(35) = "CUT LEN3" '"カット長3"
'        TRIMPARA(36) = "INDEX No." '"インデックス数"
'        TRIMPARA(37) = "MEAS MODE" '"測定モード"
'        TRIMPARA(38) = "CUT SPEED2" '"カット2スピード"
'        TRIMPARA(39) = "Q RATE2" '"Qレート2"
'        TRIMPARA(40) = "ANGLE" '"斜め角度"
'        TRIMPARA(41) = "PITCH" '"ピッチ"
'        TRIMPARA(42) = "STEP" '"ステップ"
'        TRIMPARA(43) = "LINES" '"本数"
'        TRIMPARA(44) = "ES POINT" '"ESポイント"
'        TRIMPARA(45) = "ES " '"ES判定化率"
'        TRIMPARA(46) = "ES-LEN" '"ES後カット長"
'        TRIMPARA(47) = "MAG" '"Uカットダミー1"
'        TRIMPARA(48) = "CHARACTERS" '"Uカットダミー1"
'        TRIMPARA(49) = "Q RATE3" '"Qレート3"
'        TRIMPARA(50) = "CHANGE POINT" '"切替えポイント"

'        TRIMPARA(51) = "Abs. Ratio"
'        TRIMPARA(52) = "EXTERNAL BIT"
'        TRIMPARA(53) = "Pause Time"
'        TRIMPARA(54) = "ES- %(After)" '''' NETでは「Trim Mode」
'        TRIMPARA(55) = "ES- Cnt(After)" '''' NETでは「Base res No.」
'        TRIMPARA(56) = "Ladders"
'        TRIMPARA(57) = "CUT OFF OFFSET"

'        MSG_BTN_CANCEL = "Do you return the data under the correcting to the original state? "

'        MSG_AUTO_01 = "Movement Mode"
'        MSG_AUTO_02 = "Magazine Mode"
'        MSG_AUTO_03 = "Lot Mode"
'        MSG_AUTO_04 = "Endless Mode"
'        MSG_AUTO_05 = "Data Files"
'        MSG_AUTO_06 = "Registered Data Files"
'        MSG_AUTO_07 = "To The One Of The Lists Top"
'        MSG_AUTO_08 = "To A One Of The Lists Bottom"
'        MSG_AUTO_09 = "Delete From The List"
'        MSG_AUTO_10 = "Clear The List"
'        MSG_AUTO_11 = "Registration"
'        MSG_AUTO_12 = "OK"
'        MSG_AUTO_13 = "Cancel"
'        MSG_AUTO_14 = "Data Select"
'        MSG_AUTO_15 = "All Lists Are Deleted"
'        MSG_AUTO_16 = "OK ?"
'        MSG_AUTO_17 = "Two Or More Data Files Cannot Be Chosen At The Endless Mode."
'        MSG_AUTO_18 = "Please Choose A Data File."
'        MSG_AUTO_19 = "Is Updating Data Saved ?"
'        MSG_AUTO_20 = "A Condition File Does Not Exist."

'        MSG_THETA_01 = "Adjust point 1"
'        MSG_THETA_02 = "Adjust point 2"
'        MSG_THETA_03 = "Correct value"
'        MSG_THETA_04 = "START"
'        MSG_THETA_05 = "CANCEL"
'        MSG_THETA_06 = "Appoint coordinate"
'        MSG_THETA_07 = "Difference quantity"
'        MSG_THETA_08 = "Turn revision angle"
'        MSG_THETA_09 = "Adjust point 1"
'        MSG_THETA_10 = "Adjust point 2"
'        MSG_THETA_11 = "Trim point"
'        MSG_THETA_12 = "After"
'        MSG_THETA_13 = "Adjust point 1. In movement."
'        MSG_THETA_14 = "Adjust point 2. In movement."
'        MSG_THETA_15 = "Please Push an START key after merging revision position 1."
'        MSG_THETA_16 = "Please Push an START key after merging revision position 2."
'        MSG_THETA_17 = "I confirm revision calue, and please push down an START key."
'        MSG_THETA_18 = "Matching error"
'        MSG_THETA_19 = "Pattern Matching error(Position1)"
'        MSG_THETA_20 = "Pattern Matching error(Position1)"
'        MSG_THETA_21 = "Pattern Matching OK(Position1)"
'        MSG_THETA_22 = "Pattern Matching"

'        MSG_TRIM_01 = "are out of a processing range."
'        MSG_TRIM_02 = "I was not able to do start of loader automatic driving,"
'        MSG_TRIM_03 = "Do you continue processing?"
'        MSG_TRIM_04 = "INITIAL TEST DISTRIBUTION MAP"
'        MSG_TRIM_05 = "FINAL TEST DISTRIBUTION MAP"
'        MSG_TRIM_06 = "Auto Position" '"自動ポジション"
'        BTN_TRIM_01 = "Graph Data Clear" '"ｸﾞﾗﾌﾃﾞｰﾀ ｸﾘｱ"
'        BTN_TRIM_02 = "Plate Data Editing" '"ﾌﾟﾚｰﾄﾃﾞｰﾀ編集"
'        BTN_TRIM_03 = "Initial Test Distribution Map" '"ｲﾆｼｬﾙﾃｽﾄ分布表示"
'        BTN_TRIM_04 = "Final Test Distribution Map" '"ﾌｧｲﾅﾙﾃｽﾄ分布表示"
'        PIC_TRIM_01 = "INITIAL TEST DISTRIBUTION MAP"
'        PIC_TRIM_02 = "FINAL TEST DISTRIBUTION MAP"
'        PIC_TRIM_03 = "OK" '"良品"
'        PIC_TRIM_04 = "NG" '"不良品"
'        PIC_TRIM_05 = "MIN %" '"最小%"
'        PIC_TRIM_06 = "MAX %" '"最大%"
'        PIC_TRIM_07 = "AVG %" '"平均%"
'        PIC_TRIM_08 = "Std Dev" '"標準偏差"
'        PIC_TRIM_09 = "Res Num" '"抵抗数"
'        PIC_TRIM_10 = "DISTRIBUTION MAP SAVE"

'        MSG_LOADER_01 = "In abnormal all stops, occurring."
'        MSG_LOADER_02 = "In abnormal a cycle stop, occurring."
'        MSG_LOADER_03 = "In light trouble outbreak."
'        MSG_LOADER_04 = "Setting was completed."
'        MSG_LOADER_05 = "Loader error."
'        MSG_LOADER_06 = "Loader interlock."
'        MSG_LOADER_07 = "A pattern matching error."
'        MSG_LOADER_08 = "Consecutive NG-HI errors."
'        MSG_LOADER_09 = "A full power measurement (Q Rate 10kHz)"
'        MSG_LOADER_10 = "A full power unevenness error."
'        MSG_LOADER_11 = "A full power decrement error."
'        MSG_LOADER_12 = "The adjustment full power measurement."
'        MSG_LOADER_13 = "Power adjustment"
'        MSG_LOADER_14 = "A laser power adjustment error."
'        MSG_LOADER_15 = "The automatic driving end."
'        MSG_LOADER_16 = "Loader Alarm List"
'        MSG_LOADER_17 = "When A Board Is Left On The Work Table,"
'        MSG_LOADER_18 = "Please Remove It."
'        MSG_LOADER_19 = "The Substrate Kind Changed."                       ' ###089
'        'MSG_LOADER_20 = "After Removing Substrate From NG Discharge Box"   ' ###089
'        MSG_LOADER_20 = "Remove Substrate From NG Discharge Box"            ' ###124
'        MSG_LOADER_21 = "NG Discharge Box Is Full."                         ' ###089
'        'MSG_LOADER_22 = "PUSH START SW or OK BUTTON, Then Origin Return"   ' ###124
'        MSG_LOADER_22 = "PUSH START SW(OK BUTTON) Then Origin Return"       ' ###124
'        MSG_LOADER_23 = "Automatic Driving Is Stopped"                      ' ###124
'        '----- ###187↓ -----
'        MSG_LOADER_24 = "Table Clamp Ctrl"
'        MSG_LOADER_25 = "Table Vacume Ctrl"
'        MSG_LOADER_26 = "Hand Vacume Ctrl"
'        MSG_LOADER_27 = "Hand1 Vacume Ctrl"
'        MSG_LOADER_28 = "Hand2 Vacume Ctrl"
'        'MSG_LOADER_24 = "Table Clamp Off"                                   ' ###144
'        'MSG_LOADER_25 = "Table Vacume Off"                                  ' ###144
'        'MSG_LOADER_26 = "Hand Vacume Off"                                   ' ###144
'        'MSG_LOADER_27 = "Hand1 Vacume Off"                                  ' ###158
'        'MSG_LOADER_28 = "Hand2 Vacume Off"                                  ' ###158
'        '----- ###187↑ -----
'        MSG_LOADER_29 = "When A Substrate Is Left In The Equipment,"        ' ###158
'        MSG_LOADER_30 = "Under Automatic Driving Stop"                      ' ###172
'        MSG_LOADER_31 = "PUSH START SW(OK BUTTON) Then Quits the program."  ' ###175
'        MSG_LOADER_32 = "Please Perform Automatic Driving"                  ' ###184 
'        MSG_LOADER_33 = "After Removing Substrate."                         ' ###184 
'        MSG_LOADER_34 = "Please Lower A Magazine To A Substrate Detection"  ' ###184 
'        MSG_LOADER_35 = "Sensor-Off Position."                              ' ###184 
'        MSG_LOADER_36 = "Please Remove Substrate."                          ' ###184 
'        '----- ###188↓ -----
'        MSG_LOADER_37 = "A Substrate Is Left On The XY Table"
'        MSG_LOADER_38 = "Push START SW(OK BUTTON) Then Origin Return "
'        MSG_LOADER_39 = ""
'        '----- ###188↑ -----
'        MSG_LOADER_40 = "Please Remove NG Substrate"                        ' ###197 
'        MSG_LOADER_41 = "After Removing Substrate"                          ' ###197 
'        '----- ###240↓ -----
'        MSG_LOADER_42 = "A Substrate Is Not Shown On The XY Table"
'        MSG_LOADER_43 = "Please Perform Again"
'        '----- ###240↑ -----
'        '----- V1.18.0.0⑨↓ -----
'        MSG_LOADER_44 = "Magazine End"
'        MSG_LOADER_45 = "Processing Is Continued By OK Button,"
'        MSG_LOADER_46 = "Or Ended By Cancel Button."
'        '----- V1.18.0.0⑨↑ -----
'        MSG_LOADER_47 = "Probe Check Error"                                 ' V1.23.0.0⑦
'        MSG_LOADER_48 = "During A Cycle Stop."      'V4.0.0.0⑲
'        MSG_LOADER_49 = "Breaking Was Detected."    'V4.0.0.0⑲
'        MSG_LOADER_50 = "Please Set Substrates."    'V4.11.0.0⑥

'        MSG_71 = "It is an adsorption mistake.Try to set the substrate."
'        MSG_72 = "When be the cover, try to open the slide cover, and to set the substrate."
'        MSG_73 = "U-CUT Parameter file is not exist."
'        MSG_74 = "U-CUT Parameter file can not open."
'        MSG_75 = "This file is not parameter file for U-CUT."

'        STEP_TITLE01 = "CIRCUIT DIRECTION"
'        STEP_TITLE02 = "GROUP INTERVAL"
'        STEP_TITLE03 = "STEP INTERVAL"
'        LBL_STEP_STEP = "Step Data"
'        LBL_STEP_GROUP = "Group Data"
'        LBL_STEP_TY2 = "TY2 Data"

'        LBL_ATT = "ATT."
'        LBL_FLCUR = "CURRENT "

'        ' ■ローダアラームメッセージ 
'        MSG_LDALARM_00 = "Emergency Stop."
'        MSG_LDALARM_01 = "Magazine Compatibility Alarm."
'        MSG_LDALARM_02 = "Breaking miss article outbreak."
'        MSG_LDALARM_03 = "Hand1 Absorption Alarm."
'        MSG_LDALARM_04 = "Hand2 Absorption Alarm."
'        MSG_LDALARM_05 = "Table Absorption Sensor Error."
'        MSG_LDALARM_06 = "Table Absorption Error."
'        MSG_LDALARM_07 = "Robot Alarm."
'        MSG_LDALARM_08 = "Process Surveillance Alarm."
'        MSG_LDALARM_09 = "Elevator Abnormalities."
'        'MSG_LDALARM_10 = "No Magazine Or No."              '###073
'        MSG_LDALARM_10 = "No Magazine Or No Substrate."     '###073
'        MSG_LDALARM_11 = "Origin Return Timeout."
'        MSG_LDALARM_12 = "Clamp Alarm." '###125
'        MSG_LDALARM_13 = "the air pressure fall was detected." 'V4.0.0.0-54
'        MSG_LDALARM_14 = "Undefined alarm No."
'        MSG_LDALARM_15 = "Undefined alarm No."

'        MSG_LDALARM_16 = "Circles Cylinder Timeout."
'        MSG_LDALARM_17 = "Hand1 Cylinder Timeout."
'        MSG_LDALARM_18 = "Hand2 Cylinder Timeout."
'        MSG_LDALARM_19 = "Supply Hand Absorption Error."
'        MSG_LDALARM_20 = "Storing Hand Absorption Error."
'        MSG_LDALARM_21 = "NG Discharge Is Full."
'        MSG_LDALARM_22 = "Under Stop."
'        MSG_LDALARM_23 = "The Door Is Opening."
'        MSG_LDALARM_24 = "Two-Sheet Picking Detection." ' V1.18.0.0⑪
'        MSG_LDALARM_25 = "Conveyance Top And Bottom Alarm." 'V4.0.0.0-59
'        MSG_LDALARM_26 = "Undefined alarm No."
'        MSG_LDALARM_27 = "Undefined alarm No."
'        MSG_LDALARM_28 = "Undefined alarm No."
'        MSG_LDALARM_29 = "Undefined alarm No."
'        MSG_LDALARM_30 = "Undefined alarm No."
'        MSG_LDALARM_31 = "Undefined alarm No."
'        MSG_LDALARM_UD = "Undefined alarm No."

'        MSG_LDGUID_00 = "The Emergency Stop SW Was Pushed."
'        MSG_LDGUID_01 = "Please Check The Normal Position Of Magazine."
'        'MSG_LDGUID_02 = "The Broken Or Missing Product Was Outbreak By The Specified Number."                  'V1.16.0.0②
'        MSG_LDGUID_02 = "The Broken Or Missing Product Was Outbreak." + vbCrLf + "Please Remove A Substrate."   'V1.16.0.0②
'        MSG_LDGUID_03 = "Please Check The Adsorption Sensor."
'        MSG_LDGUID_04 = "Please Check The Adsorption Sensor."
'        MSG_LDGUID_05 = "Please Check The Adsorption Sensor."
'        MSG_LDGUID_06 = "Please Check The Adsorption Sensor." + vbCrLf + "Please Confirm Whether A Top Plate Does Not Have A Wound And A Fragment Of A Board."
'        MSG_LDGUID_07 = "Robot Alarm Occurred."
'        MSG_LDGUID_08 = "Timeout Occurred Under Process Surveillance."
'        MSG_LDGUID_09 = "Please Check The Sensor Of An Elevator."
'        'MSG_LDGUID_10 = "Please Set A Magazine Whether Magazine Search DOGU Falls Down."              '###073
'        MSG_LDGUID_10 = "Please Set A Magazine Or A Substrate Whether Magazine Search DOGU Falls Down." '###073
'        '----- V1.18.0.0⑩↓ -----
'        ' ローム殿特注(マガジン検出ドグはないのでメッセージ変更する)
'        If (gSysPrm.stCTM.giSPECIAL = customROHM) Then
'            MSG_LDGUID_10 = "Please Set A Magazine Or A Substrate."
'        End If
'        '----- V1.18.0.0⑩↑ -----
'        MSG_LDGUID_11 = "Timeout Occurred At The Origin Return."
'        MSG_LDGUID_12 = "The Abnormalities In A Clamp." '###125
'        MSG_LDGUID_13 = "Please check whether air is supplied correctly." 'V4.0.0.0-54
'        MSG_LDGUID_14 = "Please Refer To A Maker For An Alarm Number."
'        MSG_LDGUID_15 = "Please Refer To A Maker For An Alarm Number."

'        MSG_LDGUID_16 = "Please Check A Cylinder Sensor."
'        MSG_LDGUID_17 = "Please Check A Cylinder Sensor."
'        MSG_LDGUID_18 = "Please Check A Cylinder Sensor."
'        MSG_LDGUID_19 = "Timeout Occurred At The Substrate Adsorption."
'        MSG_LDGUID_20 = "Timeout Occurred At The Substrate Adsorption."
'        MSG_LDGUID_21 = "NG Discharge Box Is Full." + vbCrLf + "Please Remove A Substrate."
'        MSG_LDGUID_22 = "Under Stop."
'        MSG_LDGUID_23 = "Plese Close The Door."
'        MSG_LDGUID_24 = "Please Remove The Substrate Of A Supply Hand. Then Rerun." ' V1.18.0.0⑪
'        MSG_LDGUID_25 = "Please Check The Limit Sensor Of Conveyance Top And Bottom." 'V4.0.0.0-59
'        MSG_LDGUID_26 = "Please Refer To A Maker For An Alarm Number."
'        MSG_LDGUID_27 = "Please Refer To A Maker For An Alarm Number."
'        MSG_LDGUID_28 = "Please Refer To A Maker For An Alarm Number."
'        MSG_LDGUID_29 = "Please Refer To A Maker For An Alarm Number."
'        MSG_LDGUID_30 = "Please Refer To A Maker For An Alarm Number."
'        MSG_LDGUID_31 = "Please Refer To A Maker For An Alarm Number."
'        MSG_LDGUID_UD = "Please Refer To A Maker For An Alarm Number."

'        MSG_LDINFO_UD = "Alarm Out Of Assumption."

'    End Sub
'#End Region

'#Region "メッセージを設定する(日本語/英語) NET用"
'    '''=========================================================================
'    '''<summary>メッセージを設定する(日本語/英語) NET用</summary>
'    '''<param name="language">言語番号(0:日本語 1:英語 その他:英語)</param>
'    '''=========================================================================
'    Public Sub PrepareMessages_N(ByVal language As Short)

'        'Language Type Check
'        Select Case language
'            Case 0 'Japanese
'                Call MessagesJapanese()
'            Case 1 'English
'                Call MessagesEnglish()
'            Case Else 'English
'                Call MessagesEnglish()
'        End Select
'    End Sub
'#End Region

'#Region "日本語メッセージを設定する NET用"
'    '''=========================================================================
'    '''<summary>日本語メッセージを設定する NET用</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub MessagesJapanese()

'        '⇒Title
'        TITLE_TX = "チップサイズ(TX)ティーチング"
'        TITLE_TY = "ステップサイズ(TY)ティーチング"
'        TITLE_ExCam = "外部カメラによる第1抵抗ティーチング"
'        TITLE_ExCam1 = "外部カメラによるティーチング"
'        TITLE_T_Theta = "Tθティーチング"
'        '⇒Frame
'        FRAME_TX_01 = "第１抵抗基準点"
'        FRAME_TX_02 = "第１グループの最終抵抗基準点"
'        FRAME_TY_01 = "第１ブロック基準点"
'        FRAME_TY_02 = "第１グループの最終ブロック基準点"
'        FRAME_TY_03 = "第２グループの第１ブロック基準点"
'        FRAME_C_VAL = "補正値"
'        FRAME_ExCam_01 = "ブロックＮｏ．（範囲は１～９９）"
'        FRAME_ExCam_02 = "ティーチングポイント"

'        '⇒Command Button
'        CMD_CANCEL = "Cancel"

'        '⇒FlexGrid
'        FXGRID_TITLE01 = "R No."
'        FXGRID_TITLE02 = "C No."
'        FXGRID_TITLE03 = "ｽﾀｰﾄ X"
'        FXGRID_TITLE04 = "ｽﾀｰﾄ Y"

'        '⇒Label
'        LBL_TXTY_TEACH_01 = "指定座標"
'        LBL_TXTY_TEACH_02 = "ずれ量(μm)"
'        LBL_TXTY_TEACH_03 = "補正量"
'        LBL_TXTY_TEACH_04 = "補正比率"
'        LBL_TXTY_TEACH_05 = "チップサイズ(mm)"
'        LBL_TXTY_TEACH_06 = "ﾌﾞﾛｯｸｻｲｽﾞ(mm)"
'        LBL_TXTY_TEACH_07 = "補正前"
'        LBL_TXTY_TEACH_08 = "補正後"
'        LBL_TXTY_TEACH_09 = "グループインターバル"
'        LBL_TXTY_TEACH_10 = "ﾌﾞﾛｯｸｻｲｽﾞ補正(mm)"
'        LBL_TXTY_TEACH_11 = "ステップインターバル"
'        LBL_TXTY_TEACH_12 = "第１基準点"
'        LBL_TXTY_TEACH_13 = "第２基準点"
'        LBL_TXTY_TEACH_14 = "グループ"
'        LBL_Ex_Cam_01 = "Ｘ軸"
'        LBL_Ex_Cam_02 = "Ｙ軸"

'        INFO_REC = "登録:[START]  キャンセル:[RESET]"

'        INFO_MSG01 = "第１サーキット基準点を合わせてから｢START｣キーを押して下さい"
'        INFO_MSG02 = "最終サーキット基準点を合わせてから｢START｣キーを押して下さい"
'        INFO_MSG03 = "補正値を確認して｢START｣キーを押下して下さい"
'        INFO_MSG04 = "第１ブロック基準点を合わせてから｢START｣キーを押して下さい"
'        INFO_MSG05 = "最終ブロック基準点を合わせてから｢START｣キーを押して下さい"
'        INFO_MSG06 = "第２グループの第１ブロック基準点を合わせてから｢START｣キーを押して下さい"
'        INFO_REC = "登録:[START]  キャンセル:[RESET]"
'        INFO_ST = "開始:[START]  中止:[RESET]"
'        INFO_MSG07 = "コンソールの矢印キーで位置を合わせます"
'        INFO_MSG08 = "　[START]  ：位置決定" & vbCrLf & "　[RESET]：中止"
'        INFO_MSG09 = "　ティーチングを終了します"
'        INFO_MSG10 = "　[START]キーを押して下さい" & vbCrLf & "　[HALT]キーで１つ前のデータに戻れます"
'        INFO_MSG11 = "　ブロック数を入力して下さい" & vbCrLf & "　[START]キーを押すと次の処理へ進みます"
'        INFO_MSG12 = "　ブロック数を入力して下さい" & vbCrLf & "フロントカバーを閉じると次の処理へ進みます"
'        INFO_MSG13 = "チップサイズ　ティーチング"
'        INFO_MSG14 = "ステージグループ間隔ティーチング"
'        INFO_MSG15 = "ステップオフセット量　ティーチング"
'        INFO_MSG16 = "　　基準位置を合わせて下さい。"
'        'INFO_MSG17 = "　　移動：[矢印]  決定：[START]  中断：[RESET]" '& vbCrLf & "　　[HALT]で１つ前の処理に戻ります。"'V1.24.0.0④
'        INFO_MSG17 = "　　移動：[矢印]  決定：[START]  中断：[RESET]  前項:[HALT]" 'V1.24.0.0④

'        If (gTkyKnd = KND_CHIP) Then
'            INFO_MSG18 = "第1グループ、第1抵抗基準位置のティーチング"
'            INFO_MSG20 = "グループ、最終抵抗基準位置のティーチング"
'            INFO_MSG21 = "グループ、第1抵抗基準位置のティーチング"
'            INFO_MSG22 = "最終グループ、最終抵抗位置のティーチング"
'            INFO_MSG28 = "グループ、最終端位置のティーチング"
'            INFO_MSG29 = "グループ、最先端位置のティーチング"
'            ERR_TXNUM_E = "抵抗数が１のためこのコマンドは実行できません！"
'        ElseIf (gTkyKnd = KND_NET) Then
'            INFO_MSG18 = "第1グループ、第1サーキット位置のティーチング"
'            INFO_MSG20 = "グループ、最終サーキット基準位置のティーチング"
'            INFO_MSG21 = "グループ、第1サーキット基準位置のティーチング"
'            INFO_MSG22 = "最終グループ、最終サーキット位置のティーチング"
'            ERR_TXNUM_E = "サーキット数が１のためこのコマンドは実行できません！"
'        End If
'        ERR_TXNUM_C = "サーキット数が１のためこのコマンドは実行できません！"

'        INFO_MSG19 = "第"
'        INFO_MSG23 = "ＢＰグループ間隔ティーチング"
'        INFO_MSG30 = "サーキット間隔ティーチング"
'        INFO_MSG31 = "ステップオフセット量のティーチング"
'        INFO_MSG32 = " (ＴＸ)"  '###084
'        INFO_MSG33 = " (ＴＹ)"  '###084

'        ERR_TXNUM_B = "ブロック数が１のためこのコマンドは実行できません！"
'        ERR_TXNUM_S = "ステップ測定回数が0のためこのコマンドは実行できません！" 'V1.13.0.0③
'        '----- V1.19.0.0-33↓ -----
'        ERR_PROB_EXE = "有効な抵抗(1-999)データがないためこのコマンドは実行できません！"
'        ERR_TEACH_EXE = "有効な抵抗(1-999)又はマーキング抵抗データがないためこのコマンドは実行できません！"
'        '----- V1.19.0.0-33↑ -----

'        '⇒OperationLogging
'        MSG_OPLOG_TX_START = "TXティーチング"
'        MSG_OPLOG_TY_START = "TYティーチング"
'        MSG_OPLOG_TY2_START = "TY2ティーチング"
'        MSG_OPLOG_ExCam_START = "外部カメラティーチング(第1抵抗)"
'        MSG_OPLOG_ExCam1_START = "外部カメラティーチング(全抵抗)"
'        MSG_OPLOG_CitTx_START = "サーキットTXティーチング"
'        MSG_OPLOG_CitTe_START = "サーキットティーチング"
'        MSG_OPLOG_T_THETA_START = "Tθティーチング"

'        '(Pitch Size)
'        FRAME_PITCH = "XYZ/BP ピッチ幅設定"
'        LBL_PICTH(0) = "LOWピッチ"
'        LBL_PICTH(1) = "HIGHピッチ"
'        LBL_PICTH(2) = "ポーズ時間"

'        CIRTEACH_MSG00 = "第"
'        CIRTEACH_MSG01 = "サーキット基準点ＸＹを合わせてから｢START｣キーを押して下さい"

'        CIRTY_MSG00 = "第１サーキット基準点"
'        CIRTY_MSG01 = "第１グループの最終サーキット基準点"
'        CIRTY_MSG02 = "第２グループの第１サーキット基準点"
'        CIRTY_MSG03 = "ｻｰｷｯﾄｻｲｽﾞ(mm)"

'        FRAME_TY2_01 = "ティーチングポイント"
'        If (gTkyKnd = KND_CHIP) Then
'            INFO_REC_TX = "登録:[START]  キャンセル:[RESET]" & vbCrLf & "登録+TX2:[HI+START] "
'            INFO_REC_TY = "登録:[START]  キャンセル:[RESET]" & vbCrLf & "登録+TY2:[HI+START] "
'            INFO_REC_TTHETA = "登録:[START]  キャンセル:[RESET]"
'        ElseIf (gTkyKnd = KND_NET) Then
'            INFO_REC_TX = "登録:[START]  キャンセル:[RESET]"
'            INFO_REC_TY = "登録:[START]  キャンセル:[RESET]"
'            INFO_REC_TTHETA = "登録:[START]  キャンセル:[RESET]"
'        End If

'        INFO_MSG24 = "第"
'        INFO_MSG25 = "ブロックのティーチング"
'        INFO_MSG26 = "最終"
'        INFO_MSG27 = "Tθティーチング"
'        TITLE_TY2 = "ステップサイズ(TY2)ティーチング"

'        'V1.13.0.0⑥ ADD START
'        LBL_IDREADER_TEACH_01 = "ＩＤ リーダー ティーチング"
'        LBL_IDREADER_TEACH_02 = "第１ ＩＤ読み取り位置"
'        LBL_IDREADER_TEACH_03 = "第２ ＩＤ読み取り位置"
'        LBL_IDREADER_TEACH_04 = "ＩＤリーダー"
'        'V1.13.0.0⑥ ADD END
'        LBL_PROBECLEANING_TEACH = "プローブクリーニング位置ティーチング" 'V4.0.0.0-30

'    End Sub
'#End Region

'#Region "英語メッセージを設定する NET用"
'    '''=========================================================================
'    '''<summary>メッセージを設定する(日本語/英語) NET用</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub MessagesEnglish()

'        '⇒Title
'        TITLE_TX = "Chip size (TX) Teaching"
'        TITLE_TY = "Step size (TY) Teaching"
'        TITLE_ExCam = "The 1st resistance teaching with an external camera"
'        TITLE_ExCam1 = "Teaching with an external camera."
'        TITLE_T_Theta = "T_Theta Teaching"

'        '⇒Frame
'        FRAME_TX_01 = "The 1st resistance datum point"
'        FRAME_TX_02 = "The last resistance datum point of the 1st group"
'        FRAME_TY_01 = "The 1st block datum point"
'        FRAME_TY_02 = "The last block datum point of the 1st group"
'        FRAME_TY_03 = "1st block reference point of the second group"
'        FRAME_C_VAL = "Correct Value"
'        FRAME_ExCam_01 = "Block No. (The range is 1-99.)"
'        FRAME_ExCam_02 = "Teaching Point"

'        '⇒Command Button
'        CMD_CANCEL = "Cancel"

'        '⇒FlexGrid
'        FXGRID_TITLE01 = "R No."
'        FXGRID_TITLE02 = "C No."
'        FXGRID_TITLE03 = "START X"
'        FXGRID_TITLE04 = "START Y"

'        '⇒Label
'        LBL_TXTY_TEACH_01 = "Appoint coordinate"
'        LBL_TXTY_TEACH_02 = "Difference quantity(μm)"
'        LBL_TXTY_TEACH_03 = "Correct quantity"
'        LBL_TXTY_TEACH_04 = "Correct ratio"
'        LBL_TXTY_TEACH_05 = "Chip size(mm)"
'        LBL_TXTY_TEACH_06 = "Block size(mm)"
'        LBL_TXTY_TEACH_07 = "Before"
'        LBL_TXTY_TEACH_08 = "After"
'        LBL_TXTY_TEACH_09 = "Group interval(mm)"
'        LBL_TXTY_TEACH_10 = "Block size correct(mm)"
'        LBL_TXTY_TEACH_11 = "Step interval"
'        LBL_TXTY_TEACH_12 = "The 1st datum point."
'        LBL_TXTY_TEACH_13 = "The 2nd datum point."
'        LBL_TXTY_TEACH_14 = "Group"
'        LBL_TXTY_TEACH_15 = "Angle(deg)"
'        LBL_Ex_Cam_01 = "X-axis"
'        LBL_Ex_Cam_02 = "Y-axis"

'        INFO_MSG01 = "Please push the START key after uniting the 1st resistance datum point."
'        INFO_MSG02 = "Please push the START key after uniting the last resistance datum point."
'        INFO_MSG03 = "Please check a compensation value and push the START key."
'        INFO_MSG04 = "Please push the START key after uniting the 1st block datum point."
'        INFO_MSG05 = "Please push the START key after uniting the last block datum point."
'        INFO_MSG06 = "Please push the START key after uniting the 1st block datum point of the 2nd group."
'        INFO_REC = "REGISTER:[START]   CANCEL:[RESET]"
'        INFO_ST = "START:[START]  STOP:[RESET]"

'        INFO_MSG07 = "A position is united by the arrow key of a console."
'        INFO_MSG08 = "OK：[START]　STOP：[RESET]"
'        INFO_MSG09 = "Teaching is ended."
'        INFO_MSG10 = "Please push the [START] key. It can return to the data in front of one by the [HALT] key."
'        INFO_MSG11 = "Please input the number of blocks." & vbCrLf & "If the [START] key is pushed, it will progress to the next processing."
'        INFO_MSG12 = "Please input the number of blocks." & vbCrLf & "Next Process when the slide cover is shut."
'        INFO_MSG13 = "CHIP SIZE TEACHING"
'        INFO_MSG14 = "STAGE INTERVAL TEACHING"
'        INFO_MSG15 = "STEP OFFSET TEACHING"

'        INFO_MSG16 = "    Please unite a standard position."
'        'INFO_MSG17 = "    MOVE:[Arrow]  OK:[START]  CANCEL:[RESET]" '& vbCrLf & "    It returns to the processing before one by the HALT key."'V1.24.0.0④
'        INFO_MSG17 = "    MOVE:[Arrow]  OK:[START]  CANCEL:[RESET]  PREV:[HALT]" ''V1.24.0.0④

'        If (gTkyKnd = KND_CHIP) Then
'            INFO_MSG18 = "<Group No.1> The 1st resistance standard position." ''''2009/07/03 NETでは「resistance→circuit」(18,20-22)
'            INFO_MSG20 = "> The last resistance standard position."
'            INFO_MSG21 = "> The 1st resistance standard position."
'            INFO_MSG22 = "<Last Group> The last resistance standard position."
'            INFO_MSG28 = "> The Final Edge Positionlast."
'            INFO_MSG29 = "> The State-Of-The-Art Position."
'        ElseIf (gTkyKnd = KND_NET) Then
'            INFO_MSG18 = "<Group No.1> The 1st circuit standard position."
'            INFO_MSG20 = "> The last circuit standard position."
'            INFO_MSG21 = "> The 1st circuit standard position."
'            INFO_MSG22 = "<Last Group> The last circuit standard position."
'        End If
'        INFO_MSG19 = "<Group No."
'        INFO_MSG23 = "BP GROUP INTERVAL TEACHING"
'        INFO_MSG30 = "CIRCUIT INTERVAL TEACHING"
'        INFO_MSG31 = "STEP OFFSET TEACHING"
'        INFO_MSG32 = " (TX)"  '###084
'        INFO_MSG33 = " (TY)"  '###084

'        ERR_TXNUM_E = "Can't execute this command since the resistor number is 1."
'        ERR_TXNUM_B = "Can't execute this command since the block number is 1."
'        ERR_TXNUM_C = "Can't execute this command since the Circuit number is 1."
'        ERR_TXNUM_S = "Can't execute this command since the Step number is 0." 'V1.13.0.0③
'        '----- V1.19.0.0-33↓ -----
'        ERR_PROB_EXE = "Can't execute this command since there is no resistance data(1-999)."
'        ERR_TEACH_EXE = "Can't execute this command since there is no resistance data(1-999) or marking resistance data."
'        '----- V1.19.0.0-33↑ -----

'        '⇒OperationLogging
'        MSG_OPLOG_TX_START = "TX TEACHING"
'        MSG_OPLOG_TY_START = "TY TEACHING"
'        MSG_OPLOG_TY2_START = "TY2 TEACHING"
'        MSG_OPLOG_ExCam_START = "THE 1ST RESISTANCE TEACHING WITH AN EXTERNAL CAMERA"
'        MSG_OPLOG_ExCam1_START = "TEACHING WITH AN EXTERNAL CAMERA"
'        MSG_OPLOG_CitTx_START = "CIRCUIT TX TEACHING"
'        MSG_OPLOG_CitTe_START = "CIRCUIT TEACHING"
'        MSG_OPLOG_T_THETA_START = "T_THETA TEACHING"
'        FRAME_PITCH = "XYZ/BP MOVING PITCH"
'        LBL_PICTH(0) = "LOW PITCH"
'        LBL_PICTH(1) = "HIGH PITCH"
'        LBL_PICTH(2) = "PAUSE TIME"

'        CIRTEACH_MSG00 = "No."
'        CIRTEACH_MSG01 = "Please push the START key after uniting the circuit standard point."

'        FRAME_TY2_01 = "Teaching Point"

'        INFO_REC_TX = "REGISTER:[START]   CANCEL:[RESET]" & vbCrLf & "REGISTER+TX2:[HI+START]"
'        INFO_REC_TY = "REGISTER:[START]   CANCEL:[RESET]" & vbCrLf & "REGISTER+TY2:[HI+START]"
'        INFO_REC_TTHETA = "REGISTER:[START]   CANCEL:[RESET]"

'        INFO_MSG24 = "<Block No."
'        INFO_MSG25 = "> The resistance standard position."
'        INFO_MSG26 = "<End Block"
'        INFO_MSG27 = "T_THETA TEACHING"
'        TITLE_TY2 = "Step size (TY2) Teaching"

'        'V1.13.0.0⑥ ADD START
'        LBL_IDREADER_TEACH_01 = "ID READER TEACHING"
'        LBL_IDREADER_TEACH_02 = "ID READER POTITION No1"
'        LBL_IDREADER_TEACH_03 = "ID READER POTITION No2"
'        LBL_IDREADER_TEACH_04 = "ID READER"
'        'V1.13.0.0⑥ ADD END
'        LBL_PROBECLEANING_TEACH = "PROBE CLEANING POSITION TEACHING" 'V4.0.0.0-30

'    End Sub

'#End Region

'#Region "ティーチング完了(START または RESET 押下時)"
'    '''=========================================================================
'    '''<summary>TX/TYティーチング完了(START または RESET 押下時)</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Public Function FmTxTyTeachOkClick() As Short

'        'ExitFlag = False
'        'FmTxTyMsgbox.ShowDialog()

'        'If FmTxTyMsgbox.sGetReturn = True Then
'        '    ExitFlag = True
'        'Else
'        '    ExitFlag = False
'        'End If

'        'If FmTxTyMsgbox.sGetTxTyReturn = True Then
'        '    FmTxTyTeachOkClick = 1
'        'Else
'        '    FmTxTyTeachOkClick = 0
'        'End If

'    End Function
'#End Region

'    '**********************************************************
'    '   ｻﾌﾞﾙｰﾁﾝ群
'    '**********************************************************
'#Region "言語ﾌﾗｸﾞより文字列を設定"
'    '''=========================================================================
'    '''<summary>言語ﾌﾗｸﾞより文字列を設定</summary>
'    '''<param name="language">(INP)言語番号(0:日本語 1:英語 その他:英語)</param>
'    '''<remarks>ﾌｫｰﾑﾛｰﾄﾞ時に実行する
'    '''         ｶｯﾄ位置補正､ｷｬﾘﾌﾞﾚｰｼｮﾝのみ設定</remarks>
'    '''=========================================================================
'    Public Sub PrepareMessagesCutPosCorrect_Calibration(ByVal language As Short)

'        Select Case language
'            Case 0
'                Call PrepareMessagesJapaneseCutPosCorrect_Calibration()
'            Case 1
'                Call PrepareMessagesEnglishCutPosCorrect_Calibration()
'            Case Else
'                Call PrepareMessagesEnglishCutPosCorrect_Calibration()
'        End Select

'    End Sub
'#End Region

'#Region "日本語文字列を設定する CutPosCorrect_Calibration"
'    '''=========================================================================
'    '''<summary>日本語文字列を設定する CutPosCorrect_Calibration</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesJapaneseCutPosCorrect_Calibration()
'        ' ｶｯﾄ位置補正文字列設定
'        FRM_CUT_POS_CORRECT_TITLE = "カット位置補正"

'        LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER = "グループ内抵抗数"

'        'LBL_CUT_POS_CORRECT_BLOCK_NO_X = "ブロックNo（X軸 1～99）"
'        'LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "ブロックNo（Y軸 1～99）"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_X = "ブロックNo X軸"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "ブロックNo Y軸"

'        LBL_CUT_POS_CORRECT_OFFSET_X = "カット位置補正テーブルオフセットＸ[mm]"
'        LBL_CUT_POS_CORRECT_OFFSET_Y = "カット位置補正テーブルオフセットＹ[mm]"

'        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ文字列設定
'        FRM_CALIBRATION_TITLE = "キャリブレーション"

'        LBL_CALIBRATION_STANDERD_COORDINATES1X = "基準座標１Ｘ[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES1Y = "基準座標１Ｙ[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2X = "基準座標２Ｘ[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2Y = "基準座標２Ｙ[mm]"

'        LBL_CALIBRATION_TABLE_OFFSETX = "テーブルオフセットＸ[mm]"
'        LBL_CALIBRATION_TABLE_OFFSETY = "テーブルオフセットＹ[mm]"

'        LBL_CALIBRATION_GAP1X = "基準座標１Ｘずれ量[mm]"
'        LBL_CALIBRATION_GAP1Y = "基準座標１Ｙずれ量[mm]"
'        LBL_CALIBRATION_GAP2X = "基準座標２Ｘずれ量[mm]"
'        LBL_CALIBRATION_GAP2Y = "基準座標２Ｙずれ量[mm]"

'        LBL_CALIBRATION_GAINX = "ゲイン補正係数Ｘ"
'        LBL_CALIBRATION_GAINY = "ゲイン補正係数Ｙ"
'        LBL_CALIBRATION_OFFSETX = "オフセット補正量Ｘ[mm]"
'        LBL_CALIBRATION_OFFSETY = "オフセット補正量Ｙ[mm]"
'        LBL_CALIBRATION_001 = "【十字カットモード(基準座標１)】"
'        LBL_CALIBRATION_002 = "【十字カットモード(基準座標２)】"
'        LBL_CALIBRATION_003 = "【画像認識モード(基準座標１)】"
'        LBL_CALIBRATION_004 = "【画像認識モード(基準座標２)】"

'        MSG_CUT_POS_CORRECT_001 = "ブロックNo.は１～９９で入力して下さい。"
'        MSG_CUT_POS_CORRECT_002 = "ブロックNo.入力後、[ブロック移動]を押してください。"
'        MSG_CUT_POS_CORRECT_003 = "処理を開始します。" & vbCrLf & "[START]：続行" & vbCrLf & "[RESET]：中止"
'        MSG_CUT_POS_CORRECT_004 = "【警告】" & "[START]：十字カット実行，[RESET]：中止"
'        MSG_CUT_POS_CORRECT_005 = "十字カット実行中" & vbCrLf & "[HALT]：一時停止"
'        MSG_CUT_POS_CORRECT_006 = "[START]：十字カット実行" & vbCrLf & "[RESET]：中止" & vbCrLf & "[ADJ]一時停止解除"
'        MSG_CUT_POS_CORRECT_007 = "[START]：画像認識実行" & vbCrLf & "[RESET]：中止"
'        MSG_CUT_POS_CORRECT_008 = "画像認識実行中" & vbCrLf & "[ADJ]一時停止"
'        MSG_CUT_POS_CORRECT_009 = "[START]画像認識実行" & vbCrLf & "[RESET]中止" & vbCrLf & "[ADJ]一時停止解除"
'        MSG_CUT_POS_CORRECT_010 = "補正値更新エラー" & vbCrLf & "更新結果が範囲を超えています。" & vbCrLf & "最大値、最小値に補正されます。"
'        MSG_CUT_POS_CORRECT_011 = "十字カット終了" & vbCrLf & "[START]画像認識" & vbCrLf & "[RESET]中止"
'        MSG_CUT_POS_CORRECT_012 = "画像認識終了"
'        MSG_CUT_POS_CORRECT_013 = "画像マッチングエラー"
'        MSG_CUT_POS_CORRECT_014 = "ブロック番号を入力後、[START]キーを押してください"
'        MSG_CUT_POS_CORRECT_015 = "ブロック番号を入力エラー"
'        MSG_CUT_POS_CORRECT_016 = "画像認識実行中"
'        MSG_CUT_POS_CORRECT_017 = "矢印スイッチ:位置調整, [START]:次項, [HALT]:前項"
'        MSG_CUT_POS_CORRECT_018 = "相関係数="
'        MSG_CUT_POS_CORRECT_019 = "画像が見つかりません"

'        MSG_CALIBRATION_001 = "キャリブレーションを実行します。" & vbCrLf & "[START]を押してください。" & vbCrLf & "[RESET]：中止"
'        MSG_CALIBRATION_002 = "外部カメラでずれ量を検出します。" & vbCrLf & "[START]を押してください。" & vbCrLf & "[RESET]：中止"
'        MSG_CALIBRATION_003 = "[START]：画像認識実行，[RESET]：中止"
'        MSG_CALIBRATION_004 = "外部カメラでずれ量を検出します。(基準座標１)" & vbCrLf & "[START]：決定，[RESET]：中止"
'        MSG_CALIBRATION_005 = "外部カメラでずれ量を検出します。(基準座標２)" & vbCrLf & "[START]：決定，[RESET]：中止"
'        MSG_CALIBRATION_006 = "キャリブレーションを終了します。" & vbCrLf & "データを保持する場合は[OK]を、データを保持しない場合は[Cancel]を押して下さい。"
'        MSG_CALIBRATION_007 = "画像マッチングエラー" & vbCrLf & "処理を続ける場合は[OK]を、中止する場合は[Cancel]を押して下さい。"
'        MSG_CALIBRATION_008 = "Resetします。 [Start]:Reset実行 , [Reset]:Cancel実行 "      ' ' ###078 
'        ' Recog
'        LBL_CALIBRATION_STANDERD_COORDINATES1 = "基準座標１[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2 = "基準座標２[mm]"
'        LBL_CALIBRATION_TABLE_OFFSET = "テーブルオフセット[mm]"
'        LBL_CALIBRATION_CUT = "ｷｬﾘﾌﾞﾚｰｼｮﾝｶｯﾄ"
'        LBL_CALIBRATION_MOVE1 = "基準座標１へ移動"
'        LBL_CALIBRATION_MOVE2 = "基準座標２へ移動"
'        LBL_CUT_POS_CORRECT_CUT = "十字カット"
'        LBL_CUT_POS_CORRECT_MOVE = "外部カメラへ移動"

'        ' LOG
'        MSG_OPLOG_CUT_POS_CORRECT_START = "カット位置補正"
'        MSG_OPLOG_CALIBRATION_START = "キャリブレーション"
'        MSG_OPLOG_CUT_POS_CORRECT_RECOG_START = "カット位置補正画像登録"
'        MSG_OPLOG_CALIBRATION_RECOG_START = "キャリブレーション画像登録"
'    End Sub
'#End Region

'#Region "英語文字列を設定する CutPosCorrect_Calibration"
'    '''=========================================================================
'    '''<summary>英語文字列を設定する CutPosCorrect_Calibration</summary>
'    '''<remarks></remarks>
'    '''=========================================================================
'    Private Sub PrepareMessagesEnglishCutPosCorrect_Calibration()
'        ' ｶｯﾄ位置補正文字列設定
'        FRM_CUT_POS_CORRECT_TITLE = "Cut Position Correct"

'        LBL_CUT_POS_CORRECT_GROUP_RESISTOR_NUMBER = "Resistor Number in Group"

'        'LBL_CUT_POS_CORRECT_BLOCK_NO_X = "Block Number (X Axis 1～99)"
'        'LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "Block Number (Y Axis 1～99)"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_X = "Block Number X Axis"
'        LBL_CUT_POS_CORRECT_BLOCK_NO_Y = "Block Number Y Axis"

'        LBL_CUT_POS_CORRECT_OFFSET_X = "Cut Position Correct Table Offset X[mm]"
'        LBL_CUT_POS_CORRECT_OFFSET_Y = "Cut Position Correct Table Offset Y[mm]"

'        ' ｷｬﾘﾌﾞﾚｰｼｮﾝ文字列設定
'        FRM_CALIBRATION_TITLE = "Calibration"

'        LBL_CALIBRATION_STANDERD_COORDINATES1X = "Standerd Coordinates 1X[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES1Y = "Standerd Coordinates 1Y[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2X = "Standerd Coordinates 2X[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2Y = "Standerd Coordinates 2Y[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES1 = "Standerd Coordinates 1[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2 = "Standerd Coordinates 2[mm]"

'        LBL_CALIBRATION_TABLE_OFFSETX = "Table Offset X[mm]"
'        LBL_CALIBRATION_TABLE_OFFSETY = "Table Offset Y[mm]"
'        LBL_CALIBRATION_TABLE_OFFSET = "Table Offset[mm]"

'        LBL_CALIBRATION_GAP1X = "Standerd Coordinates 1X Gap[mm]"
'        LBL_CALIBRATION_GAP1Y = "Standerd Coordinates 1Y Gap[mm]"
'        LBL_CALIBRATION_GAP2X = "Standerd Coordinates 2X Gap[mm]"
'        LBL_CALIBRATION_GAP2Y = "Standerd Coordinates 2Y Gap[mm]"

'        LBL_CALIBRATION_GAINX = "Gain Revision Coefficient X"
'        LBL_CALIBRATION_GAINY = "Gain Revision Coefficient Y"
'        LBL_CALIBRATION_OFFSETX = "Offset Revision Quantity X [mm]"
'        LBL_CALIBRATION_OFFSETY = "Offset Revision Quantity Y [mm]"
'        LBL_CALIBRATION_001 = "[Cross Cut Mode(Position1)]"
'        LBL_CALIBRATION_002 = "[Cross Cut Mode(Position2)]"
'        LBL_CALIBRATION_003 = "[Cross Cut Mode(Position1)]"
'        LBL_CALIBRATION_004 = "[Cross Cut Mode(Position2)]"

'        MSG_CUT_POS_CORRECT_001 = "Input Block Number Range 1-99"
'        MSG_CUT_POS_CORRECT_002 = "Push [Block Move]Button After Input Block Number"
'        MSG_CUT_POS_CORRECT_003 = "Start Proccess" & vbCrLf & "[START]:Continue" & vbCrLf & "[RESET]:Cancel"
'        MSG_CUT_POS_CORRECT_004 = "WARNING!! " & "[START]:Cross Cut Execution, [RESET]:Cancel"
'        MSG_CUT_POS_CORRECT_005 = "Cross Cut Executing" & vbCrLf & "[HALT]:Stop"
'        MSG_CUT_POS_CORRECT_006 = "[START]Cross Cut Execution" & vbCrLf & "[RESET]:Cancel" & vbCrLf & "[ADJ]Disable A While Stop"
'        MSG_CUT_POS_CORRECT_007 = "[START]A Picture Recognition Execution" & vbCrLf & "[RESET]:Cancel"
'        MSG_CUT_POS_CORRECT_008 = "Picture Recognition Executiing" & vbCrLf & "[ADJ]A While Stop"
'        MSG_CUT_POS_CORRECT_009 = "[START]A Picture Recognition Execution" & vbCrLf & "[RESET]Cancel" & vbCrLf & "[ADJ]Disable A While Stop"
'        MSG_CUT_POS_CORRECT_010 = "Correct Renewal Error" & vbCrLf & "Renewal Result Overflow" & vbCrLf & "Value is Correct with Maximum Value or Minimum Value"
'        MSG_CUT_POS_CORRECT_011 = "Cross Cut End" & vbCrLf & "[START]:Picture Recognition" & vbCrLf & "[RESET]:Cancel"
'        MSG_CUT_POS_CORRECT_012 = "Picture Recognition End"
'        MSG_CUT_POS_CORRECT_013 = "Picture Matching Error"
'        MSG_CUT_POS_CORRECT_014 = "Push [START]Button After Input Block Number"
'        MSG_CUT_POS_CORRECT_015 = "Block Number Input Error"
'        MSG_CUT_POS_CORRECT_016 = "Picture Recognition Execution"
'        MSG_CUT_POS_CORRECT_017 = "ARROW SW:Position Ajustment, [START]:NEXT, [HALT]:PREV"
'        MSG_CUT_POS_CORRECT_018 = "Correlation="
'        MSG_CUT_POS_CORRECT_019 = "A Picture Is Not Found."

'        MSG_CALIBRATION_001 = "Execution Calibration" & vbCrLf & "Push [START]" & vbCrLf & "[RESET]:Cancel"
'        MSG_CALIBRATION_002 = "Detection Gap with External Camera" & vbCrLf & "Push [START]" & vbCrLf & "[RESET]:Cancel"
'        MSG_CALIBRATION_003 = "[START]:A Picture Recognition Execution, [RESET]:Cancel"
'        MSG_CALIBRATION_004 = "Detection Gap with External Camera.(Position1)" & vbCrLf & "[START]:Determination, [RESET]:Cancel"
'        MSG_CALIBRATION_005 = "Detection Gap with External Camera.(Position2)" & vbCrLf & "[START]:Determination, [RESET]:Cancel"
'        MSG_CALIBRATION_006 = "The calibration is ended." & vbCrLf & "Please push ""OK"" when you maintain data and push ""Cancel"" when you do not maintain data."
'        MSG_CALIBRATION_007 = "Picture Matching Error." & vbCrLf & "Please push ""OK"" when you continue processing and push ""Cancel"" when you stop."
'        MSG_CALIBRATION_008 = "Exeute Reset [Start]:Reset , [Reset]:Cancel of Reset"        '  ' ###078 

'        ' Recog
'        LBL_CALIBRATION_STANDERD_COORDINATES1 = "Standerd Coordinates 1[mm]"
'        LBL_CALIBRATION_STANDERD_COORDINATES2 = "Standerd Coordinates 2[mm]"
'        LBL_CALIBRATION_TABLE_OFFSET = "Table Offset[mm]"
'        LBL_CALIBRATION_CUT = "Calibration CUT"
'        LBL_CALIBRATION_MOVE1 = "Move Std. Pos. 1"
'        LBL_CALIBRATION_MOVE2 = "Move Std. Pos. 2"
'        LBL_CUT_POS_CORRECT_CUT = "Cross Cut"
'        LBL_CUT_POS_CORRECT_MOVE = "Move Ext. Camera"

'        ' LOG
'        MSG_OPLOG_CUT_POS_CORRECT_START = "CUT Position Correct"
'        MSG_OPLOG_CALIBRATION_START = "Calibration"
'        MSG_OPLOG_CUT_POS_CORRECT_RECOG_START = "CUT Position Correct Recog"
'        MSG_OPLOG_CALIBRATION_RECOG_START = "Calibration Recog"
'    End Sub
'#End Region

'End Module